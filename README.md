<div align="center">

# Buffalo Framework

**面向现代 .NET 的全栈基础设施组件库 & 高性能自研 BQL ORM 框架**  
*支持 .NET 8.0 (主干推荐) 与 .NET Framework 2.0 ~ 4.8 经典运行时*

[![.NET 8.0](https://img.shields.io/badge/.NET-8.0-blue.svg)](https://dotnet.microsoft.com/download)
[![C# 12](https://img.shields.io/badge/C%23-12.0-orange.svg)](https://docs.microsoft.com/dotnet/csharp/)
[![AI-Agent Ready](https://img.shields.io/badge/AI%20Agent-Native%20Ready-success.svg)](#-ai-agent-深度适配与使用指南cursor--copilot--claude--antigravity)
[![License](https://img.shields.io/badge/License-MIT%20%2F%20Custom-green.svg)](#-开源协议与使用声明)
[![PRs Welcome](https://img.shields.io/badge/PRs-welcome-brightgreen.svg)](https://github.com/taisandog/buffalobro/pulls)

</div>

---

## 📖 框架简介

**Buffalo** 是一个经过大规模生产实践检验的 .NET 企业级全栈基础设施库。它不仅包含一套完全面向对象的自研强类型 ORM 引擎（**BQL - Buffalo Query Language**），还深度覆盖了多云对象存储、多级分布式缓存、多端消息队列、MongoDB 驱动、高并发 IOCP 网络通信以及底层高效工具链。

### 🌟 核心设计理念
1. **统一抽象，多后端透明切换**：业务代码仅依赖抽象接口，具体底层实现（如 Redis / Memcached、Kafka / RabbitMQ、阿里云 OSS / 腾讯云 COS / 本地存储）完全通过连接串或工厂动态切换，杜绝与厂商 SDK 深度绑定。
2. **极端性能优化**：底层深度应用 IL 动态发射反射（FastReflection）、线程安全无锁/细粒度锁设计、内存缓冲池与 Windows IOCP 高性能完成端口模型。
3. **规范三层 + 代码生成插件**：提供配套的可视化实体与代码生成工具，自动生成 `BQLEntity`、`DataAccess` 与 `BusinessModelBase`，告别原生 SQL 拼接与重复模板代码。
4. **🤖 AI Agent-Native 时代原生适配**：业界首个深度针对现代 AI 编码助手（**Cursor、Claude Code、GitHub Copilot、Google Antigravity**）深度定制的自研库体系，提供完整的“反向约束规则表”与“代码范例卡片”，杜绝 LLM 幻觉，让 AI 像资深架构师一样顺畅写出规范的 Buffalo 业务代码。

---

## 🏗️ 架构拓扑与组件依赖

```text
                           Buffalo.Kernel (底层通用核心)
                                  │
                    ┌─────────────┼─────────────┐
                    ▼             ▼             ▼
           Buffalo.ArgCommon  Buffalo.DB   Buffalo.IOCP (网络库)
                    │             │
                    │     ┌───────┼───────────┬────────────┐
                    │     ▼       ▼           ▼            ▼
                    │  Data.*  QueryCache  MongoDB        MQ
                    ▼
               Buffalo.Storage
```

- **现代主干版本**：位于 [`SourceCodeCore/BuffaloCore/`](SourceCodeCore/BuffaloCore/)（基于 **.NET 8.0** 构建，推荐所有新项目使用）。
- **经典历史版本**：位于 [`SourceCode/`](SourceCode/)（兼容 .NET Framework 2.0 ~ 4.8）。

---

## 📦 核心功能模块一览

每个模块均提供独立、详细的针对人与 AI 的规范文档（点击链接即可查阅）：

| 组件 | 功能定位 | 支持的后端实现 / 特性 | 详细使用文档 |
|---|---|---|:---:|
| **Buffalo.DB** | **核心自研 ORM 引擎** | 强类型 BQL 语法树、实体脏属性自动跟踪、悲观排它锁（`LockUpdate`）、批量操作、分布式事务 | [📘 Buffalo.DB.md](BuffaloMD/Buffalo.DB.md) |
| **Buffalo.Kernel** | **底层基础工具链** | 安全类型转换（`ValueConvertExtend`）、快速反射（IL 发射）、并发上下文锁、MD5/AES 加密、连接串解析 | [📘 Buffalo.Kernel.md](BuffaloMD/Buffalo.Kernel.md) |
| **Buffalo.ArgCommon** | **统一通信与结果协议** | 统一 API 返回格式 `APIResault`（`state/message/data`）、`ArgValues` 请求参数解析、JSON 序列化过滤 | [📘 Buffalo.ArgCommon.md](BuffaloMD/Buffalo.ArgCommon.md) |
| **Buffalo.Storage** | **统一对象存储抽象** | 阿里云 OSS、腾讯云 COS、华为云 OBS、AWS S3、本地磁盘/局域网共享，全套同步与异步 API | [📘 Buffalo.Storage.md](BuffaloMD/Buffalo.Storage.md) |
| **Buffalo.QueryCache** | **多级可插拔缓存** | Redis（StackExchange.Redis）、Memcached、进程内内存，内置 Hash、List、ZSet 与分布式锁 | [📘 Buffalo.QueryCache.md](BuffaloMD/Buffalo.QueryCache.md) |
| **Buffalo.MQ** | **分布式消息队列总线** | Apache Kafka、RabbitMQ、Redis Stream/List、MQTT 协议，支持可靠 ACK 确认与死信重试 | [📘 Buffalo.MQ.md](BuffaloMD/Buffalo.MQ.md) |
| **Buffalo.MongoDB** | **MongoDB 强类型封装** | 文档实体映射、自增主键支持、`ConditionList` 条件构造器、批量安全更新、副本集事务 | [📘 Buffalo.MongoDB.md](BuffaloMD/Buffalo.MongoDB.md) |
| **Buffalo.IOCP** | **高性能异步网络服务** | 基于 Windows 完成端口（IOCP），内置私有防粘包二进制协议 `FastNetAdapter` 与 WebSocket 协议 | [📘 Buffalo.IOCP.md](BuffaloMD/Buffalo.IOCP.md) |

### 🗄️ 多数据库方言适配驱动（自动适配 BQL 分页、系统函数与元数据探测）

| 数据库 | 驱动适配包 | 特性与语法支持 | 详细文档 |
|---|---|---|:---:|
| **MySQL / MariaDB** | `Buffalo.Data.MySQL` | 原生 `LIMIT` 分页、`utf8mb4` 全字符集、`LAST_INSERT_ID()` 回填 | [📘 文档](BuffaloMD/Buffalo.Data.MySQL.md) |
| **Oracle** | `Buffalo.Data.Oracle` | 兼容 10g/11g/12c+、`ROWNUM` 分页、序列 `SequenceManager`、冒号参数 | [📘 文档](BuffaloMD/Buffalo.Data.Oracle.md) |
| **PostgreSQL** | `Buffalo.Data.PostgreSQL` | 原生 `LIMIT OFFSET` 分页、自增序列、模式 Schema 探测 | [📘 文档](BuffaloMD/Buffalo.Data.PostgreSQL.md) |
| **SQLite** | `Buffalo.Data.SQLite` | 嵌入式单文件数据库、WAL 日志并发模式、客户端本地轻量存储 | [📘 文档](BuffaloMD/Buffalo.Data.SQLite.md) |
| **IBM DB2** | `Buffalo.Data.DB2` | `ROW_NUMBER() OVER()` 高精度窗口分页、SYSCAT 系统目录探测 | [📘 文档](BuffaloMD/Buffalo.Data.DB2.md) |
| **SQL Server** | `Buffalo.DB`（内置） | 内置支持 SqlServer 2000 / 2005 / 2008 / 2012 / 2019+ | [📘 文档](BuffaloMD/Buffalo.DB.md) |

---

## 🤖 【重磅特色】AI Agent 深度适配与使用指南（Cursor / Copilot / Claude / Antigravity）

### 💡 为什么需要专门为 AI 准备规则体系？
在日常使用 Cursor、Claude Code、GitHub Copilot 等 AI 辅助编写业务代码时，由于大模型的预训练语料库中几乎**没有**企业自研私有库的权重，AI 会产生极其强烈的**“偏航惯性”**：
- ❌ 盲目退回 .NET 官方库（写原生 SQL、新建 DbContext、手写 `Convert.ToInt32` 报空引用）；
- ❌ 引入不一致的第三方库（自己引用 `StackExchange.Redis`、裸写 `Aliyun.OSS` 客户端）；
- ❌ 自定义混乱的接口返回值（拼装 `{ code: 200, message: "ok" }` 而不是统一规范）。

为此，Buffalo 打造了 **AI Agent 规范文档体系**（全部放置在 [**`BuffaloMD/`**](BuffaloMD/) 目录下），通过**“负向红线约束（不要做什么） + Golden Snippet（标准代码卡片） + 概念映射字典”**，实现让 AI 助手 100% 准确调用 BuffaloCore！

---

### 🚀 3 步在你的业务项目中激活 AI 规则

#### 步骤 1：拷贝规范文档分册
将本仓库根目录的 [**`BuffaloMD/`**](BuffaloMD/) 文件夹，拷贝到你的业务项目根目录下（例如放到 `YourProject/BuffaloMD/`）。

#### 步骤 2：在业务项目根目录创建 `AGENTS.md`（或 `.cursorrules`）
在业务项目解决方案根目录下创建 `AGENTS.md`，将以下内容直接粘贴进去：

```markdown
# 业务项目 AI 开发规范与 BuffaloCore 框架规则

本项目基于自主研发的 BuffaloCore 基础架构与插件自动生成的三层架构。
AI 在生成、修改业务代码时，必须严格遵守以下规范，严禁退回 .NET 官方常规写法。

## 1. 架构定位与层级规范
- 实体与 BQL 查询层：位于 `[项目].BQLEntity`，由代码生成插件自动维护。
  - 数据库统一句柄（如 `CIWDB`）继承 `BQLDataBaseHandle`，静态暴露各表操作句柄。
- 数据访问层：位于 `[项目].DataAccess`，继承自 `BQLDataAccessBase<T>`，由工厂反射管理。
- 业务逻辑层（BO）：位于 `[项目].Business`，必须继承 `BusinessModelBase<T>`。
  - 所有数据库增删改查必须在 Business 层进行，Controller 严禁跨层直接操作数据库。
- 表现层（API 控制器）：位于 `[项目].Controller`，继承 `ControllerBase`。
  - 所有对外接口统一返回 `APIResault.ToJson()` 或 `res.ToMessJson()`。

## 2. 核心红线（禁止事项 vs 必用规范）
| 禁止行为（严禁出现） | 必须使用的规范（强制遵守） |
|---|---|
| 手写原生 SQL、手写拼接字符串 | 使用 BQL 语法树，如 `CIWDB.表名.列名 == 变量` |
| 使用 EF Core、Dapper、DbContext | 使用 `BusinessModelBase<T>` 提供的 `SelectList`、`Insert`、`Update`、`Delete` |
| 自造 `{ code, msg, data }` 结构 | 使用 `Buffalo.ArgCommon.ApiCommon.GetSuccess/GetFault/GetException` |
| 裸类型转换 `Convert.ChangeType` | 使用全局扩展 `ValueConvertExtend.ConvertValue<T>` 或 `val.ConvertTo<T>()` |
| 裸 JSON 转换 `System.Text.Json` | 使用全局扩展 `val.ConvertJsonTo<T>()` 或 `JsonValueConvertExtend` |
| 自己解析连接串 `Split(';')` | 使用 `ConnStringFilter.GetConnectInfo(conn)` |
| 时间戳自己加减 1970 年 | 使用 `CommonMethods.ConvertDateTimeInt` / `ConvertIntDateTime` |
| 裸写 `StackExchange.Redis` | 使用 `CacheLoader.GetCache(null, "redis", conn)` |
| 裸写云厂商对象存储 SDK | 使用 `FSCreater.Create("OSS", conn)` 返回 `IFileStorage` |
| 裸写 RabbitMQ/Kafka 原生客户端 | 使用 `MQUnit.GetMQConnection` / `GetMQListener` |

## 3. 标准业务代码编写范例（Golden Snippet）

### A. 业务层 BO 查询范例
```csharp
using Buffalo.DB.QueryConditions;
using Buffalo.ArgCommon;

public class AccountBusiness : BusinessModelBase<CIWAccount>
{
    public List<CIWAccount> QueryUsers(string keyword, int pageIndex, int pageSize, out long total)
    {
        ScopeList lstScope = new ScopeList();
        if (!CommonMethods.IsNullOrWhiteSpace(keyword))
        {
            lstScope.Add(CIWDB.CIWAccount.Name.Like("%" + keyword + "%"));
        }
        lstScope.Add(CIWDB.CIWAccount.Status == 1);
        lstScope.SortList.Add(CIWDB.CIWAccount.Id, SortType.Desc);
        lstScope.Page = new PageContent { CurrentPage = pageIndex, PageSize = pageSize };

        List<CIWAccount> list = SelectList(lstScope);
        total = lstScope.Page.TotalRecord;
        return list;
    }
}
```

### B. 控制器 Controller 范例
```csharp
[HttpPost("login")]
public string Login([FromBody] Dictionary<string, object> oargs)
{
    if (oargs == null) return ApiCommon.GetFault("参数不能为空").ToJson();
    try
    {
        ArgValues args = new ArgValues(oargs);
        string name = args.GetDataValue<string>("name");
        string pwd = args.GetDataValue<string>("pwd");

        AccountBusiness bo = new AccountBusiness();
        APIResault res = bo.DoLogin(name, pwd);
        return res.ToJson();
    }
    catch (Exception ex)
    {
        return ApiCommon.GetException(ex).ToJson();
    }
}
```

## 4. 底层组件详细 API 分册文档索引
如需了解各底层组件的详细方法定义与参数，直接读取以下分册：
- 基础通用工具：BuffaloMD/Buffalo.Kernel.md
- 统一通讯协议：BuffaloMD/Buffalo.ArgCommon.md
- ORM 与 BQL 查询：BuffaloMD/Buffalo.DB.md
- 缓存组件：BuffaloMD/Buffalo.QueryCache.md
- 对象存储：BuffaloMD/Buffalo.Storage.md
- 消息队列：BuffaloMD/Buffalo.MQ.md
- MongoDB：BuffaloMD/Buffalo.MongoDB.md
- Socket通信：BuffaloMD/Buffalo.IOCP.md
```

#### 步骤 3：在各 AI 编程工具中无缝使用
- **Google Antigravity**：自动原生加载工作区中的 `AGENTS.md`。
- **Cursor**：可在根目录下同时建立 `.cursorrules`（内容与 `AGENTS.md` 保持一致），或者在 Cursor Settings 中引用。
- **Claude Code / Cline**：会自动读取工作区根目录的 `CLAUDE.md` 或 `AGENTS.md`。
- **GitHub Copilot**：软链接或复制到 `.github/copilot-instructions.md` 即可全员生效。

---

## ⚡ 极速代码体验（典型三层架构 Golden Snippets）

### 1. BQL 强类型实体查询与排它锁（防并发）
```csharp
using Buffalo.DB.QueryConditions;

public class OrderBusiness : BusinessModelBase<OrderInfo>
{
    // 加排它锁查询未支付订单（生成 SELECT ... FOR UPDATE）
    public OrderInfo GetOrderForPay(long orderId)
    {
        ScopeList lstScope = new ScopeList();
        lstScope.Add(MyProjectDB.OrderInfo.Id == orderId);
        lstScope.Add(MyProjectDB.OrderInfo.PayStatus == 0);
        lstScope.ForUpdate = BQLLockType.LockUpdate;

        return GetUnique(lstScope);
    }
}
```

### 2. 统一多云对象存储（一行切换 OSS / COS / 本地）
```csharp
using Buffalo.Storage;
using Buffalo.ArgCommon;

// 无论底层配置为阿里云 OSS、腾讯云 COS 还是华为云 OBS，业务层代码 100% 相同：
IFileStorage storage = FSCreater.Create("OSS", connString);

// 异步上传并返回规范结果
await using (FileStream stream = File.OpenRead("avatar.png"))
{
    APIResault res = await storage.SaveFileAsync("/avatars/u1001.png", stream, stream.Length);
    if (res.IsSuccess)
    {
        Console.WriteLine("公网访问地址: " + storage.InternetUrl + "/avatars/u1001.png");
    }
}
```

### 3. 多级缓存与分布式锁
```csharp
using Buffalo.QueryCache;
using Buffalo.DB.CacheManager;

ICacheAdaper cache = CacheLoader.GetCache(null, "redis", connString);

// 分布式排它锁保护关键业务
QueryCacheLockAsync cacheLock = cache.GetCacheLockAsync("lock:order:1001", null);
if (await cacheLock.LockAsync(TimeSpan.FromSeconds(5)))
{
    try
    {
        // 临界区业务处理
    }
    finally
    {
        await cacheLock.UnLockAsync();
    }
}
```

---

## 🛠️ 快速编译与运行

### 1. 环境准备
- 安装 [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Visual Studio 2022 (v17.8+) 或带有 C# Dev Kit 的 VS Code / JetBrains Rider

### 2. 编译项目
```bash
# 克隆仓库
git clone https://github.com/taisandog/buffalobro.git
cd buffalobro/SourceCodeCore/BuffaloCore

# 还原依赖并编译 Release 版本
dotnet restore BuffaloCore.sln
dotnet build BuffaloCore.sln -c Release
```
> 各组件已开启 `GeneratePackageOnBuild`，Release 编译完成后将在各子项目的 `bin/Release` 目录下自动生成对应的 `.nupkg` NuGet 包，便于搭建企业私有包仓库或直接引用。

---

## 📂 代码库目录导航

```text
buffalobro/
├── SourceCodeCore/
│   └── BuffaloCore/               # 【核心推荐】面向 .NET 8.0 现代运行时的完整基础库源码
│       ├── Buffalo.Kernel/        # 核心通用工具与反射
│       ├── Buffalo.ArgCommon/     # 统一通信参数与 APIResault
│       ├── Buffalo.DB/            # ORM 核心框架
│       ├── Buffalo.Data.*/        # MySQL/Oracle/PostgreSQL/SQLite/DB2 方言适配器
│       ├── Buffalo.Storage/       # 统一对象存储抽象
│       ├── Buffalo.QueryCache/    # 统一缓存抽象
│       ├── Buffalo.MQ/            # 统一消息队列抽象
│       ├── Buffalo.IOCP/          # 高性能网络通信组件
│       ├── Buffalo.MongoDB/       # MongoDB 强类型包装
│       ├── 说明/                  # 包含全套组件的 AI Agent Markdown 说明分册
│       └── AGENTS.md              # 面向 AI 与贡献者的架构复用规则
├── BuffaloMD/                     # 全套 AI Agent 说明文档分册（方便 GitHub 在线查阅）
├── SourceCode/                    # .NET Framework 2.0 ~ 4.8 经典源码版本
├── OtherPorject/                  # 辅助工具、插件代码与 Demo 演示项目
└── 使用文档/                      # 历史部署与配置说明参考文档
```

---

## 📄 开源协议与使用声明

本项目遵循开源社区规范。商业应用或引入前，欢迎与作者联系了解更多最佳实践支持。

- **作者**：taisandog
- **GitHub 主页**：[https://github.com/taisandog](https://github.com/taisandog)
- **代码仓库**：[https://github.com/taisandog/buffalobro](https://github.com/taisandog/buffalobro)
- **历史下载与教程**：[https://github.com/taisandog/buffalodownload](https://github.com/taisandog/buffalodownload)
