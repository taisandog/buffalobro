# BuffaloCore

Buffalo 基础库集合，面向 .NET 应用的基础设施组件，覆盖 ORM、通用工具、网络通信、MongoDB、对象存储、缓存与消息队列等常见能力。

各业务能力采用**统一抽象 + 多后端切换**的设计：业务代码依赖接口，具体实现（Redis / Memcached、Kafka / RabbitMQ、OSS / S3 等）通过配置或工厂选择，便于在不同环境与云厂商之间迁移。

**目标框架：** .NET 8.0  
**作者：** taisandog  
**解决方案：** `BuffaloCore.sln`

---

## 模块一览

| 模块 | 说明 |
|------|------|
| **Buffalo.Kernel** | 基础类库：反射、集合、锁、类型转换、线程池等通用能力 |
| **Buffalo.DB** | ORM 实现：实体映射、BQL 查询、事务、分页、数据库适配 |
| **Buffalo.IOCP** | 网络库：基于 Socket / IOCP 的高性能服务端通信 |
| **Buffalo.MongoDB** | MongoDB 操作库：连接、集合操作、查询条件、事务等封装 |
| **Buffalo.Storage** | 多切换存储库：统一文件存储接口，对接多家对象存储与本地磁盘 |
| **Buffalo.QueryCache** | 多切换缓存库：统一缓存适配，支持 Redis、Memcached、内存等 |
| **Buffalo.MQ** | 多切换消息队列库：统一收发与监听，支持 Kafka、RabbitMQ、Redis、MQTT |

### 配套与扩展模块

| 模块 | 说明 |
|------|------|
| **Buffalo.ArgCommon** | 通用参数与 API 结果封装（如 `APIResault`） |
| **Buffalo.Data.MySQL** | Buffalo.DB 的 MySQL 适配 |
| **Buffalo.Data.Oracle** | Buffalo.DB 的 Oracle 适配 |
| **Buffalo.Data.PostgreSQL** | Buffalo.DB 的 PostgreSQL 适配 |
| **Buffalo.Data.SQLite** | Buffalo.DB 的 SQLite 适配 |
| **Buffalo.Data.DB2** | Buffalo.DB 的 DB2 适配 |

> SQL Server 适配已内置于 `Buffalo.DB`（SqlServer 2K / 2K5 / 2K8 / 2K12 等）。

---

## 核心模块说明

### Buffalo.Kernel — 基础类库

通用底层工具，被多数 Buffalo 组件依赖。

主要能力包括：

- **快速反射**（`FastReflection`）：属性/字段读写、动态调用
- **集合与并发**（`Collections`、`Lock`、`AsyncTaskLock`）
- **类型与值转换**（`ValueConvert`、`Defaults`）
- **代理与拦截**（`ClassProxyBuilder`）
- **线程池与批量任务**（`TreadPoolManager`、`MassManager`）
- 密码哈希、枚举工具、连接串过滤等常用辅助

### Buffalo.DB — ORM

自研 ORM 核心，提供实体映射与数据库访问抽象。

主要能力包括：

- 实体基类、业务模型（`EntityBase`、`BusinessModelBase` 等）
- **BQL** 链式/关键字式查询构建（Select / Insert / Update / Delete、Join、分页等）
- 条件、排序、范围查询与数据填充
- 事务与批量操作
- 多数据库适配架构（`IDBAdapter` + 各 `Buffalo.Data.*` 驱动包）
- 查询缓存接口扩展点（与 `Buffalo.QueryCache` 配合）

### Buffalo.IOCP — 网络库

基于 Socket 的高性能网络通信组件，适用于自定义协议的长连接服务。

主要能力包括：

- `ServerSocket` 服务端：接入、收发、断开、错误与通知事件
- 数据协议封装（`DataProtocol`）
- 心跳管理、字节缓冲、连接信息管理
- 与业务层通过事件回调解耦

### Buffalo.MongoDB — MongoDB 操作库

对 MongoDB Driver 的封装，风格与 Buffalo 数据访问体系对齐。

主要能力包括：

- 连接与库信息管理（`MongoConnection`、`MongoDBManager`）
- 集合操作（`MGCollection`、`MongoDBOperate`）
- 查询条件构建（`QueryCondition`）
- 自增、事务、文档扩展操作等

### Buffalo.Storage — 多切换存储库

统一对象/文件存储抽象 `IFileStorage`，同一套上传、下载、追加、删除等 API，可切换后端。

| 后端 | 说明 |
|------|------|
| 本地磁盘 | `LocalFileManager` |
| 阿里云 OSS | `AliCloud.OssAPI` |
| 腾讯云 COS | `QCloud.CosApi` |
| 华为云 OBS | `HW.OBS` |
| AWS S3 | `AWS.S3` |

通过 `FSCreater` 等工厂按配置创建对应适配器，业务侧无需绑定具体云厂商 SDK。

### Buffalo.QueryCache — 多切换缓存库

为 Buffalo.DB 提供可插拔的查询/通用缓存实现，由 `CacheLoader` 按类型创建适配器。

| 类型标识 | 后端 |
|----------|------|
| `redis` | Redis（StackExchange.Redis） |
| `memcached` | Memcached |
| `web` | 进程内 / 内存缓存 |

支持 Hash、List、锁、SortedSet 等与缓存管理相关的抽象能力（见 `Buffalo.DB.CacheManager` 接口约定）。

### Buffalo.MQ — 多切换消息队列库

统一消息队列抽象：连接、发送、监听、批量与事务等，通过 `MQUnit` 按类型注册与创建。

| 类型标识 | 后端 |
|----------|------|
| `kafkamq` | Apache Kafka（Confluent.Kafka） |
| `rabbitmq` | RabbitMQ |
| `redismq` | Redis Stream / 队列 |
| `mqttmq` | MQTT（MQTTnet） |

业务代码面向 `MQConnection` / `MQListener` 等抽象编程，更换中间件时主要改配置即可。

---

## 依赖关系（简图）

```text
                    Buffalo.Kernel
                          │
            ┌─────────────┼─────────────┐
            ▼             ▼             ▼
   Buffalo.ArgCommon  Buffalo.DB   Buffalo.IOCP
            │             │
            │     ┌───────┼───────────┬────────────┐
            │     ▼       ▼           ▼            ▼
            │  Data.*  QueryCache  MongoDB        MQ
            ▼
       Buffalo.Storage
```

- **Kernel** 为公共基础。
- **DB** 依赖 Kernel、ArgCommon；各 `Data.*` 与 QueryCache、MongoDB 依赖 DB。
- **MQ / IOCP** 主要依赖 Kernel（及 ArgCommon）。
- **Storage** 依赖 ArgCommon，并引入各云厂商对象存储 SDK。

---

## 快速开始

### 环境要求

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- Visual Studio 2022+ 或 VS Code / Rider 等

### 打开与编译

```bash
# 克隆仓库后进入解决方案目录
cd BuffaloCore

# 还原并编译
dotnet restore BuffaloCore.sln
dotnet build BuffaloCore.sln -c Release
```

各项目开启了 `GeneratePackageOnBuild`，Release 构建时会生成对应 NuGet 包（`.nupkg`）。

### 按需引用

不必引用全部模块，按业务选型即可，例如：

| 场景 | 建议引用 |
|------|----------|
| 仅工具与通用逻辑 | `Buffalo.Kernel` |
| 关系型数据库 ORM | `Buffalo.DB` + 对应 `Buffalo.Data.*` |
| MongoDB | `Buffalo.MongoDB` |
| 文件 / 对象存储 | `Buffalo.Storage` |
| 分布式缓存 | `Buffalo.QueryCache`（常与 DB 一起使用） |
| 消息队列 | `Buffalo.MQ` |
| 自定义协议网络服务 | `Buffalo.IOCP` |

项目引用示例：

```xml
<ItemGroup>
  <ProjectReference Include="..\Buffalo.Kernel\Buffalo.Kernel.csproj" />
  <ProjectReference Include="..\Buffalo.DB\Buffalo.DB.csproj" />
  <ProjectReference Include="..\Buffalo.Data.MySQL\Buffalo.Data.MySQL.csproj" />
</ItemGroup>
```

---

## 设计理念

1. **统一抽象**  
   存储、缓存、MQ 均以接口/基类对外，业务与具体中间件解耦。

2. **多后端可切换**  
   通过类型字符串或配置选择实现（如 `redis` / `memcached`，`kafkamq` / `rabbitmq`），适配多云与多环境。

3. **模块化、可组合**  
   各库职责清晰，可单独打包与升级；ORM 与缓存、Mongo、MQ 等按需组合。

4. **面向 .NET 现代运行时**  
   主目标框架为 .NET 8，便于在 ASP.NET Core、控制台、Windows 服务等场景使用。

---

## 项目结构

```text
BuffaloCore/
├── BuffaloCore.sln
├── Buffalo.Kernel/          # 基础类库
├── Buffalo.ArgCommon/       # 参数与 API 结果
├── Buffalo.DB/              # ORM 核心
├── Buffalo.Data.MySQL/      # MySQL 适配
├── Buffalo.Data.Oracle/     # Oracle 适配
├── Buffalo.Data.PostgreSQL/ # PostgreSQL 适配
├── Buffalo.Data.SQLite/     # SQLite 适配
├── Buffalo.Data.DB2/        # DB2 适配
├── Buffalo.MongoDB/         # MongoDB
├── Buffalo.QueryCache/      # 多切换缓存
├── Buffalo.Storage/         # 多切换对象存储
├── Buffalo.MQ/              # 多切换消息队列
├── Buffalo.IOCP/            # 网络库
└── ThirdParty/              # 第三方源码依赖（如 Enyim.Caching 等）
```

---

## 相关链接

- 作者 / 组织：[github.com/taisandog](https://github.com/taisandog)
- 存储包相关仓库示例：[github.com/taisandog/buffalobro](https://github.com/taisandog/buffalobro)

---

## 许可证

请以仓库内实际 LICENSE 文件为准。若尚未添加许可证文件，使用前请与作者确认授权方式。
