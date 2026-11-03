# Buffalo.IOCP 高性能网络通信组件调用指南

`Buffalo.IOCP` 是基于 Windows IOCP（完成端口）与高性能异步 Socket 模型的网络通信框架，支持高并发 TCP/UDP 服务端、客户端、心跳保活检测以及二进制自定义协议（`FastNetAdapter`）与 WebSocket 协议（`WebSocketAdapter`）。

---

## 1. 核心命名空间与类型

- 核心网络层：`Buffalo.IOCP`
  - `ServerSocket`：异步 Socket 服务端监听器
  - `HeartManager`：客户端连接心跳管理与死连接巡检淘汰
  - `SocketInfo`：连接上下文与状态记录
- 协议与数据包：`Buffalo.IOCP.DataProtocol`
  - `INetProtocol`：协议适配器基类接口
  - `FastNetAdapter`：高性能私有二进制协议适配器（防粘包、拆包封包）
  - `WebSocketAdapter`：WebSocket 协议适配器
  - `ClientSocketBase` / `FastClientSocket`：客户端连接对象抽象
  - `DataPacketBase` / `FastDataPacket`：网络数据报文封装

---

## 2. 协议与适配器选择

Buffalo.IOCP 内置了两套成熟的协议栈：
1. **`FastNetAdapter`（私有高性能协议）**：专为硬件设备通信、内网微服务 RPC、游戏服务器设计，内置指令号、校验码与长度头，自动解决 TCP 粘包与分包。
2. **`WebSocketAdapter`（Web 实时通信）**：与浏览器前端直接通过 WebSocket 标准握手协议通信。

---

## 3. 服务端编写范例（基于 FastNetAdapter）

以下展示启动一个监听端口并处理数据收发、心跳与异常断开的完整服务端：

```csharp
using Buffalo.IOCP;
using Buffalo.IOCP.DataProtocol;
using System;
using System.Net.Sockets;
using System.Text;

public class MyTcpServer
{
    private ServerSocket _server;
    private HeartManager _heart;
    private FastNetAdapter _adapter;

    public void Start(int port)
    {
        // 1. 初始化协议适配器
        _adapter = new FastNetAdapter();

        // 2. 初始化心跳管理器（心跳超时时间, 检测间隔等参数）
        _heart = new HeartManager(500000, 2000, 1000, 0, null);
        _heart.StartHeart();

        // 3. 构建并启动 ServerSocket (支持 TCP 或 UDP)
        _server = new ServerSocket(
            ServerSocket.AnyIPV4, 
            port, 
            _heart, 
            _adapter, 
            null, 
            SocketType.Stream, 
            ProtocolType.Tcp
        );

        // 4. 绑定生命周期事件
        _server.OnAccept += OnClientAccept;
        _server.OnReceiveData += OnClientReceiveData;
        _server.OnClose += OnClientClose;
        _server.OnError += OnServerError;

        // 5. 启动服务
        _server.Start();
    }

    private void OnClientAccept(ClientSocketBase client)
    {
        Console.WriteLine($"[连接接入] 客户端 IP: {client.RemoteEndPoint}");
    }

    private void OnClientReceiveData(ClientSocketBase client, DataPacketBase data)
    {
        // 解包获取数据内容
        byte[] payload = data.Data;
        string message = Encoding.UTF8.GetString(payload);
        Console.WriteLine($"[收到数据] 来自 {client.RemoteEndPoint}: {message}");

        // 回复客户端响应报文
        byte[] responseBytes = Encoding.UTF8.GetBytes("ACK:" + message);
        FastDataPacket responsePacket = new FastDataPacket(data.Command, responseBytes, false, _adapter);
        client.SendPacket(responsePacket);
    }

    private void OnClientClose(ClientSocketBase client)
    {
        Console.WriteLine($"[连接关闭] 客户端断开: {client.RemoteEndPoint}");
    }

    private void OnServerError(ClientSocketBase client, Exception ex)
    {
        Console.WriteLine($"[通信异常] {ex.Message}");
    }

    public void Stop()
    {
        _server?.Stop();
        _heart?.StopHeart();
    }
}
```

---

## 4. 客户端编写范例：`FastClientSocket`

```csharp
using Buffalo.IOCP.DataProtocol;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class MyTcpClient
{
    private FastClientSocket _client;
    private FastNetAdapter _adapter = new FastNetAdapter();

    public void Connect(string ip, int port)
    {
        Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        sock.Connect(new IPEndPoint(IPAddress.Parse(ip), port));

        _client = new FastClientSocket(sock, null, false, _adapter);
        _client.OnReceiveData += OnReceiveData;
        _client.OnClose += OnClose;
    }

    public void Send(string message)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(message);
        FastDataPacket packet = new FastDataPacket(1, bytes, false, _adapter);
        _client.SendPacket(packet);
    }

    private void OnReceiveData(ClientSocketBase client, DataPacketBase data)
    {
        string resp = Encoding.UTF8.GetString(data.Data);
        Console.WriteLine("收到服务端回复: " + resp);
    }

    private void OnClose(ClientSocketBase client)
    {
        Console.WriteLine("与服务端连接断开");
    }

    public void Close()
    {
        _client?.Close();
    }
}
```

---

## 5. AI 避坑指南

1. **绝对禁止裸写底层 Socket 循环 `Receive/Send`**：不要自己写 `while(true) socket.Receive()` 并手写字节拼装，Buffalo.IOCP 内部通过完成端口事件与缓冲池驱动，所有报文收发统一在 `OnReceiveData` 和 `SendPacket` 层面处理。
2. **切勿遗漏 HeartManager 启停**：服务端如果配置了心跳检测，启动前必须调用 `_heart.StartHeart()`，停止服务时必须调用 `_heart.StopHeart()` 释放定时轮询资源。
3. **协议配对一致**：客户端与服务端的 `INetProtocol` 实例类型必须保持一致（例如服务端使用 `FastNetAdapter`，客户端也必须使用 `FastNetAdapter`），否则会导致握手解包失败。
