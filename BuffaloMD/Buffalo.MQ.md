# Buffalo.MQ 消息队列统一接入指南

`Buffalo.MQ` 为微服务与分布式应用提供跨多种消息中间件的统一抽象。通过工厂 + 统一连接串模式，支持 **Kafka**、**RabbitMQ**、**Redis MQ** 以及 **MQTT**，实现生产者发布与消费者监听的无感切换。

---

## 1. 核心命名空间与类型

- 核心工厂与管理：`Buffalo.MQ`（类 `MQUnit`）
- 生产者连接：`Buffalo.MQ.MQConnection`
- 消费者监听器：`Buffalo.MQ.MQListener`
- 消息载体与回调：`Buffalo.MQ.MQCallBackMessage`
- 具体中间件配置（可选高级定制）：
  - Kafka: `Buffalo.MQ.KafkaMQ.KafkaMQConfig`
  - RabbitMQ: `Buffalo.MQ.RabbitMQ.RabbitMQConfig`
  - Redis: `Buffalo.MQ.RedisMQ.RedisMQConfig`
  - MQTT: `Buffalo.MQ.MQTTLib.MQTTConfig`

---

## 2. 统一初始化与注册：`MQUnit.SetMQInfo`

在程序启动时（`Program.cs` 或初始化模块），注册统一 MQ 连接标识：

```csharp
using Buffalo.MQ;

// 支持的类型标识（type）：
// "redismq"  -> 基于 Redis Stream 或 List 的消息队列
// "kafkamq"  -> Apache Kafka
// "rabbitmq" -> RabbitMQ
// "mqttmq"   -> MQTT 协议通信

// 1. 注册 Redis MQ 示例：
string redisConn = "server=127.0.0.1:6379;pwd=123456;database=9;consumerName=worker1;consumerGroupName=workerGroup;";
MQUnit.SetMQInfo("OrderMQ", "redismq", redisConn);

// 2. 注册 RabbitMQ 示例：
string rabbitConn = "server=127.0.0.1:5672;uid=guest;pwd=guest;virtualHost=/;";
MQUnit.SetMQInfo("EventMQ", "rabbitmq", rabbitConn);

// 3. 注册 MQTT 示例：
string mqttConn = "server=127.0.0.1:1883;uid=test;pwd=test;clientId=client_001;keepAlive=60";
MQUnit.SetMQInfo("DeviceMQ", "mqttmq", mqttConn);
```

---

## 3. 消息生产者：`MQConnection`

使用 `MQUnit.GetMQConnection` 获取连接，统一使用 `using` 确保连接回收或线程池复用：

### 核心方法
| 方法签名 | 说明 |
|---|---|
| `APIResault Send(string topic, string message)` | 发送字符串消息（返回 `APIResault`） |
| `APIResault Send(string topic, byte[] body)` | 发送原始字节流消息 |
| `Task<APIResault> SendAsync(string topic, string message)` | 异步发送消息 |
| `Task<APIResault> SendAsync(string topic, byte[] body)` | 异步发送字节流 |

### 生产者代码范例
```csharp
using Buffalo.MQ;
using Buffalo.ArgCommon;

public class OrderEventProducer
{
    public void PublishOrderPaid(long orderId, decimal amount)
    {
        string topic = "order.events.paid";
        string payload = Newtonsoft.Json.JsonConvert.SerializeObject(new { OrderId = orderId, Amount = amount });

        // 获取连接并发送
        using (MQConnection conn = MQUnit.GetMQConnection("OrderMQ"))
        {
            APIResault res = conn.Send(topic, payload);
            if (!res.IsSuccess)
            {
                // 发送失败处理
                throw new Exception("MQ 发送失败: " + res.Message);
            }
        }
    }
}
```

---

## 4. 消息消费者：`MQListener`

使用 `MQUnit.GetMQListener` 获取监听器，支持同步和异步回调事件，并提供精确的 ACK 机制。

### 核心方法与事件
- `void StartListend(string[] topics)`：开始订阅并监听一个或多个 Topic。
- `event MQReceived OnMQReceived`：同步接收消息事件。
- `event MQReceivedAsync OnMQReceivedAsync`：异步接收消息事件（推荐）。
- `void Close()`：关闭并释放监听器连接。

### 消费者标准监听范例（带 ACK 确认）
```csharp
using Buffalo.MQ;
using System.Text;
using System.Threading.Tasks;

public class OrderConsumerService
{
    private MQListener _listener;

    public void Start()
    {
        _listener = MQUnit.GetMQListener("OrderMQ");
        
        // 绑定异步事件处理
        _listener.OnMQReceivedAsync += OnMessageReceivedAsync;
        
        // 开始监听指定的 Topics
        _listener.StartListend(new string[] { "order.events.paid", "order.events.created" });
    }

    private async Task OnMessageReceivedAsync(MQListener sender, MQCallBackMessage message)
    {
        try
        {
            string json = Encoding.UTF8.GetString(message.Body);
            // 处理业务逻辑...
            
            // 业务处理成功后，显式进行 ACK 确认提交
            await message.CommitAsync();
        }
        catch (Exception ex)
        {
            // 处理异常时不调用 Commit，根据 MQ 配置支持重试或移入死信
        }
    }

    public void Stop()
    {
        _listener?.Close();
    }
}
```

---

## 5. AI 避坑指南

1. **严禁在业务中直接引入具体 MQ SDK**：不要在业务里写 `EventingBasicConsumer`（RabbitMQ）或 `ProducerBuilder`（Confluent.Kafka），所有收发统一经由 `MQUnit`。
2. **务必显式调用 `message.Commit()` / `CommitAsync()`**：Buffalo.MQ 默认采用可靠交付模式，消费处理完成后若不执行 Commit，消息将在下次重启或超时后被重新投递。
3. **连接字符串使用 `ConnStringFilter` 规范**：参数之间使用分号 `;` 分隔，键值大小写不敏感。
