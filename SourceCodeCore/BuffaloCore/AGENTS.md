# AGENT.md — BuffaloCore

本文件给 AI 编码助手和贡献者：项目结构、编码约定，以及 **必须优先复用** 的 `Buffalo.Kernel` / `Buffalo.ArgCommon` API。

给人看的概览、快速开始和许可证见 [README.md](README.md)。

---

## 强制规则

1. **先复用，再手写。** 类型转换、空值判断、哈希、十六进制、时间戳、GUID、连接串解析、锁、反射读写、枚举 Description、API 返回值等，必须用下文列出的现有类，禁止再写一套工具方法。
2. **历史命名不要“纠正”。** `APIResault`、`ResaultType`、`TreadPoolManager`、`Mass*` 等拼写是既有公开 API，改名会破坏兼容。
3. **`ValueConvertExtend` / `JsonValueConvertExtend` 在全局命名空间**（无 `namespace`），直接调用即可，不要包进 `Buffalo.Kernel`。
4. **JSON 用 Newtonsoft.Json**（ArgCommon 已依赖），不要为同一场景引入 `System.Text.Json`。
5. **目标框架是 `net8.0`。** 不要把项目改回 `netstandard`，也不要无故升到更新的 TFM。
6. **多后端走工厂 + 连接串，不绑具体 SDK。** MQ 用 `MQUnit`，存储用 `FSCreater`/`IFileStorage`，缓存用 `CacheLoader`。
7. **不要为“通用工具”新建类库。** 通用能力只加到 Kernel / ArgCommon，且须用户明确要求。

写代码前用下表选型；表中有对应项时，禁止发明新 helper。

---

## 场景 → 必用 API

| 你想做的事 | 用这个 | 不要做 |
|-----------|--------|--------|
| `object` / `DBNull` / 空串 → `T` | `ValueConvertExtend.ConvertValue<T>(v)` 或 `v.ConvertTo<T>()` | `Convert.ChangeType`、`Convert.ToInt32`、手写 try/catch |
| DataRow 取值 | `row.GetRowValue<T>("col")` | `row["col"]` + 自己转类型 |
| `IDictionary` / 泛型字典取值 | `ht.GetMapValue<T>(key)` / `dic.GetDicValue(key)` | `ContainsKey` + 自己转 |
| JSON/`JToken`/`JObject` → `T` | `JsonValueConvertExtend.ConvertJsonTo<T>()` | 先 `ToString` 再 `Deserialize` |
| API 成功/失败/异常/超时 | `ApiCommon.GetSuccess/GetFault/GetException/GetTimeout` | 自造 `{code,msg,data}` 或新 Result 类 |
| 解析 API JSON 入参 | `ApiCommon.GetArgs(json)` → `ArgValues` | 手写 `Dictionary` + 自己取值 |
| 连接串 `a=b;c=d` | `ConnStringFilter.GetConnectInfo(conn)` | `Split(';')` 自己解析 |
| 空串 / 空集合 | `CommonMethods.IsNullOrWhiteSpace` / `IsCollectionNullOrEmpty` | 重复实现 |
| Unix 时间戳 | `CommonMethods.ConvertDateTimeInt` / `ConvertIntDateTime` | 自己减 1970-01-01 |
| GUID ↔ 十六进制 | `CommonMethods.GuidToString` / `StringToGuid` | `Guid.ToString("N")`（字节序不同） |
| bytes ↔ hex | `CommonMethods.BytesToHexString` / `HexStringToBytes` | `BitConverter` 再去 `-`、手写循环 |
| 读流到 `byte[]` | `CommonMethods.LoadStreamData2`（未知长度）或 `LoadStreamData` | 自己 `MemoryStream` 拷贝 |
| 流拷贝 | `CommonMethods.CopyStreamData` | 自己循环 `Read`/`Write` |
| MD5 / SHA1 / AES | `PasswordHash.ToMD5String` 等 | `MD5.Create()` 再转 hex |
| 程序根目录 | `CommonMethods.GetBaseRoot()`（Web 须先 `ConfigureServices`） | `Directory.GetCurrentDirectory()` 当根 |
| 同步锁（可超时） | `using (new Lock(obj))` | 裸 `Monitor.Enter` 且忘 Exit |
| 按 key 异步锁 | `await using`/`using` + `AsyncTaskLock<T>` | 自己维护 `ConcurrentDictionary<T, SemaphoreSlim>` |
| 按 key 取锁对象 | `LockObjects<K>` / `LockObjects<K,V>` | 自己做 key→lock 映射 |
| LRU / 按活跃度淘汰 | `LinkedDictionary<T,K>` + `TrimCount` / `OldestNode` | `Dictionary` + 另维护 LinkedList |
| 热路径属性读写 | `FastValueGetSet.GetValue/SetValue` 或 `PropertyInfoHandle` | 循环里 `PropertyInfo.GetValue` |
| 无参构造 | `FastValueGetSet.CreateObject(type)` | 热路径 `Activator.CreateInstance` |
| 对象浅拷贝 | `ClassInfoManager.ObjectCopyByProperty/Field` | 手写每个属性赋值 |
| 枚举 Description / 标志位 | `EnumUnit` / `MassManager` | 每次 `GetCustomAttribute` |
| 延迟提交副作用 | `MethodTransactions` / `MethodTransactionsAsync` | 自己在 finally 里拼回调队列 |
| 线程上下文变量 | `CallContextSync<T>` / `CallContextAsync<T>` | 混用 `ThreadLocal` 与 `AsyncLocal` |
| 清掉实例上所有事件 | `CommonMethods.ClearAllEvents` / `ObjectEventsUnit` | 反射翻事件字段 |

---

## 解决方案结构

```text
BuffaloCore/
├── BuffaloCore.sln
├── Buffalo.Kernel/          # 无项目依赖；被几乎所有模块引用
├── Buffalo.ArgCommon/       # 依赖 Kernel + Newtonsoft.Json
├── Buffalo.DB/              # ORM 核心；依赖 Kernel、ArgCommon
├── Buffalo.Data.*           # DB 方言适配（MySQL/Oracle/PostgreSQL/SQLite/DB2）
├── Buffalo.QueryCache/      # 缓存适配；依赖 DB
├── Buffalo.MongoDB/         # Mongo 封装；依赖 DB
├── Buffalo.Storage/         # 对象存储；依赖 ArgCommon（返回 APIResault）
├── Buffalo.MQ/              # 消息队列；依赖 Kernel
├── Buffalo.IOCP/            # Socket 服务端；依赖 Kernel
└── ThirdParty/              # 第三方源码（如 Enyim.Caching），不要当业务代码改
```

SQL Server 适配在 `Buffalo.DB/DataBaseAdapter/SqlServer2K*Adapter/`，没有独立 `Buffalo.Data.SqlServer` 项目。

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

改 Kernel / ArgCommon 的公开签名等于改全解决方案，默认只扩展、不删不改名。

---

## 模块入口（改功能时从这里找）

| 模块 | 入口类型 | 要点 |
|------|----------|------|
| Kernel | `CommonMethods`、`ValueConvertExtend`、`PasswordHash`、`ConnStringFilter`、`Lock`、`AsyncTaskLock<T>` | 见下文 API |
| ArgCommon | `ApiCommon`、`APIResault`、`ArgValues` | 统一 API 协议 `state/message/data` |
| DB | `EntityBase`、`BusinessModelBase`、`BQL`、`DataAccessLoader` | BQL 在 `BQLCommon/` |
| Data.* | `AdapterLoader`、`DBAdapter`、`CutPageSqlCreater` | 分页/函数/结构与 SQL Server 适配器对齐 |
| QueryCache | `CacheLoader.GetCache(info, type, conn)` | 类型：`redis` / `memcached` / `web` |
| MongoDB | `MongoDBManager.AddConfig` / `CreateOperate<T>` | 实体继承 `MongoEntityBase` |
| Storage | `FSCreater.Create(type, conn)` → `IFileStorage` | 类型：`local` / `COS` / `OSS` / `OBS`；方法返回 `APIResault` |
| MQ | `MQUnit.SetMQInfo` / `GetMQConnection` / `GetMQListener` | 类型：`kafkamq` / `rabbitmq` / `redismq` / `mqttmq` |
| IOCP | `ServerSocket` + `INetProtocol` | 事件：`OnAccept` / `OnReceiveData` / `OnClose` / `OnError` |

连接串统一为 `key=value;key2=value2`，用 `ConnStringFilter.GetConnectInfo` 解析（键值会 `UrlDecode`，默认忽略大小写）。

---

## 编码约定

- 语言：C#，注释与用户可见文案用中文。
- 风格：跟随周边文件（大括号、命名、`#if` 历史分支），不要为“现代化”整文件重写。
- 同步/异步成对时，跟现有 `Xxx` / `XxxAsync` 命名。
- Web 程序要拿根路径时，在 `Startup.Configure` / host 启动处调用 `CommonMethods.ConfigureServices(IHostEnvironment)`。
- 新增对外能力优先扩展现有类（`partial`、新方法），避免平行的 `XxxHelper`。
- 不要格式化无关文件，不要改 `.bak` / `obj` / `bin` / `.vs`。

---

## Buffalo.ArgCommon

命名空间：`Buffalo.ArgCommon`（`JsonValueConvertExtend` 除外，它在全局命名空间）。

统一 JSON 形状：

```json
{ "state": 1, "message": "...", "data": {} }
```

`state` 对应 `ResaultType`：`None=0`、`Success=1`、`Fault=2`、`Exception=3`、`Timeout=4`。

### `ApiCommon` — 工厂，优先用静态方法而不是 `new APIResault()`

```csharp
ArgValues args = ApiCommon.GetArgs(json);

return ApiCommon.GetSuccess("ok", data);
return ApiCommon.GetFault("参数错误");
return ApiCommon.GetException(ex);
return ApiCommon.GetTimeout("会话过期");
```

XML 辅助：`NewXmlDocument()`、`DictionaryToXml` / `XmlToDictionary<T>`、`ListToXml` / `XmlToList<T>`（节点为 `<item name="" value="" />`）。

### `APIResault`

| 成员 | 用途 |
|------|------|
| `Resault`（JSON `state`） | 状态枚举 |
| `Message` / `Data` | 信息与载荷 |
| `IsSuccess` | `Resault == Success` |
| `GetValue<T>()` | 把 `Data` 转成 `T`（走 `JsonValueConvertExtend`） |
| `SetSuccess/SetFault/SetTimeout/SetException/SetJson` | 填充状态 |
| `ToJson()` / `ToObject()` | 输出 |
| `FromShowProperty` / `FromIgnoreProperty` / `ToJsonShow` / `ToJsonIgnore` | 序列化时只出或排除部分属性 |
| `FromDataToString()` | 非字符串 `data` 先打成 JSON 字符串 |
| `FromFormatting` / `FromSettings` | 链式配置 Newtonsoft |

```csharp
return ApiCommon.GetSuccess(null, user)
    .FromIgnoreProperty(new[] { "Password", "Salt" })
    .ToJson();
```

### `ArgValues` : `Dictionary<string, object>`

```csharp
string token = args.Token;                 // 键 "Token"
T data = args.GetDataValue<T>();           // 键 "data"
int id = args.GetDataValue<int>("id", 0);
object raw = args.GetObject("key");
```

`GetDataValue` 能消化 `JToken` 和枚举底层类型。

### 其它

- `ExcludePropertiesContractResolver`：Newtonsoft 合同，`JsonParseType.Show` 只序列化名单，`Ignore` 排除名单。一般通过 `APIResault.FromShowProperty/FromIgnoreProperty` 使用。
- `JsonValueConvertExtend.ConvertJsonValue<T>(value)` / `value.ConvertJsonTo<T>()`：`JObject`/`JArray`/JSON 字符串/`object` → `T`，否则回落到 `ValueConvertExtend`。

---

## Buffalo.Kernel

### 类型转换（全局类 `ValueConvertExtend`）

处理 `null`、`DBNull`、空字符串，失败前先给默认值。

```csharp
int n = ValueConvertExtend.ConvertValue<int>(obj, 0);
long len = reader["DATA_LENGTH"].ConvertTo<long>();
string name = row.GetRowValue<string>("NAME");
int flag = ht.GetMapValue<int>("flag", 0);
string v = dic.GetDicValue("key", "");
```

| 方法 | 输入 |
|------|------|
| `ConvertValue<T>(value, defaultValue?)` | 任意 object |
| `ConvertTo<T>(this object)` | 同上，扩展方法 |
| `GetRowValue<T>(this DataRow, name)` | DataRow |
| `GetMapValue<T>(this IDictionary, key)` | 非泛型字典 |
| `GetDicValue<T,K>(this IDictionary<T,K>, key)` | 泛型字典 |

实体赋值还可用 `CommonMethods.ChangeType` / `EntityProChangeType`（含 GUID 的 hex 字符串）。

### `CommonMethods`（`Buffalo.Kernel`）

| 方法 | 说明 |
|------|------|
| `GetBaseRoot()` / `GetBaseRoot(relativeOrAbsolute)` | 应用根目录；相对路径会拼到根上 |
| `ConfigureServices(IHostEnvironment)` | Web 必须先调，否则 `IsWebContext` 为 false |
| `IsWebContext` | 是否已注入 Host 环境 |
| `PathCombine` | Windows `\`，其它 `/` |
| `IsNullOrWhiteSpace` / `IsCollectionNullOrEmpty` / `IsNull` | 空值 |
| `CopyNewer(source, target)` | 按文件版本号决定是否覆盖拷贝 |
| `GuidToString` / `StringToGuid` | GUID ↔ hex（基于 `ToByteArray`，不是 `N` 格式） |
| `ConvertIntDateTime(d, useSecond, isUTC)` | 时间戳 → DateTime；无参重载为秒 + UTC |
| `ConvertDateTimeInt(time, useSecond, isUTC)` | DateTime → 时间戳 |
| `LoadStreamData` | 按 `stm.Length` 一次读（流必须可 `Length`） |
| `LoadStreamData2` | 缓冲读取，适合未知长度 |
| `CopyStreamData(src, dst, length?)` | 流拷贝，`length<0` 表示全部 |
| `BytesToHexString` / `HexStringToBytes` / `ToByteString` | 十六进制 |
| `ListToDictionary<TKey,TValue>(list, keyProperty)` | 用属性名做字典键（走快速反射） |
| `ClearAllEvents(obj)` | 移除实例全部事件订阅 |

### `PasswordHash` / `BatchHash`

```csharp
string md5 = PasswordHash.ToMD5String(text);          // 默认大写 hex
byte[] sha1 = PasswordHash.ToSHA1Bytes(bytes);
byte[] enc = PasswordHash.AESEncrypt(data, key32, iv, CipherMode.ECB, PaddingMode.PKCS7);
byte[] dec = PasswordHash.AESDecrypt(enc, key32);

using (PasswordHash.StartBatchHash())
{
    // 同一线程复用 MD5/SHA1 实例
    PasswordHash.ToMD5String(a);
    PasswordHash.ToMD5String(b);
}
```

字符串/字节/流三套：`ToMD5String`、`ToMD5Bytes`、`ToSHA1String`、`ToSHA1Bytes`。AES 密钥按注释为 32 字节。

### `ConnStringFilter`

```csharp
Dictionary<string, string> kv = ConnStringFilter.GetConnectInfo(connString);
// 或指定比较器：GetConnectInfo(conn, StringComparer.OrdinalIgnoreCase)
```

### 锁与集合

```csharp
using (new Lock(syncRoot)) { /* 默认 15s，超时抛 TimeoutException */ }
using (new Lock(syncRoot, 0, false)) { if (!...) } // millisecondsTimeout<=0 则一直等

using (AsyncTaskLock<string> lck = new AsyncTaskLock<string>(key))
{
    await lck.LockAsync(cancellationToken); // 同异步上下文同 key 不重入加锁
}

LockObjects<string> locks = new LockObjects<string>();
lock (locks.GetObject(userId)) { }

LinkedDictionary<string, byte[]> cache = new LinkedDictionary<string, byte[]>();
cache.TrimCount(1000); // 丢掉最久未用
var oldest = cache.OldestNode;
```

- `Lock.IsTimeout`：构造时 `throwTimeoutException:false` 且没抢到锁时为 true。
- `LockObjects<K,V>` 可覆写 `CreateInstance` / `CleanSeconds`；`AutoResetLockObject<T>` 已按 `SemaphoreSlim(1,1)` 实现。
- `LinkedDictionary` 按访问更新活跃度；`GetEnumeratorOldToNew` / `GetEnumeratorNewToOld`、`RemoveValue`、`LatestNode`。

### 快速反射（`Buffalo.Kernel.FastReflection`）

```csharp
object val = FastValueGetSet.GetValue(obj, "Name", obj.GetType());
FastValueGetSet.SetValue(obj, "Tom", "Name", obj.GetType());
object inst = FastValueGetSet.CreateObject(typeof(MyType));

PropertyInfoHandle h = FastValueGetSet.GetPropertyInfoHandle("Name", type);
h.GetValue(obj); h.SetValue(obj, "Tom");

FastInvokeHandler invoke = FastValueGetSet.GetCustomerMethodInfo(type, "Foo", new[] { typeof(int) });
invoke(obj, new object[] { 1 });

ClassInfoManager.ObjectCopyByProperty(src, dest);
T attr = FastInvoke.GetClassAttribute<T>(type);
```

还有 `FastFieldGetSet`（字段）、`ClassInfoHandle` / `ClassInfoManager.GetClassHandle`（类型元数据缓存）、`AssemblyTypeLoader`。热路径禁止裸反射。

`CallContextSync<T>` = `ThreadLocal`；`CallContextAsync<T>` = `AsyncLocal`。同步调用链和 async/await 分别用对应类型，MQ 的线程连接表就是这样分的。

### 枚举与常量（`EnumUnit` / `MassManager` / `EnumInfo`）

`Mass*` 在本库表示“常量/枚举元数据”，不是物理质量。

```csharp
string desc = EnumUnit.GetEnumDescription(myEnum); // [Description]
EnumInfo info = EnumUnit.GetEnumInfo(myEnum);
List<EnumInfo> flags = EnumUnit.GetContainerValues(myEnum);
int x = EnumUnit.AddValue(src, flag);
x = EnumUnit.DeleteValue(x, flag);
bool has = EnumUnit.ContainerValue(x, flag);

MassInfo mass = MassManager.GetMassInfos(typeof(MyEnum));
```

`EnumInfo`：`Value`、`FieldName`、`Description`、`DisplayName`、`FindCustomerAttribute<T>()`。对固定类型可用 `MassBase<T>.GetDescription(value)`。

### 延迟动作

```csharp
using (MethodTransactions tx = new MethodTransactions())
{
    tx.AddArgsAction(args => SendMail((string)args[0]), new object[] { to }, DelayActionLevel.High);
    tx.Commit();   // 失败用 Rollback()
}

using (MethodTransactionsAsync tx = new MethodTransactionsAsync())
{
    tx.AddArgsAction(async args => await SendMailAsync((string)args[0]), new object[] { to });
    await tx.Commit();
}
```

嵌套时只有最外层 `Dispose` 清队列。优先级：`Low=1`、`Medium=2`、`High=3`，数字越大越先执行。

### 其它 Kernel 类型

| 类型 | 何时用 |
|------|--------|
| `DefaultType` | `IsDefaultValue`、`ToDbType`、`GetRealValueType`、`GetNullableRealType`、`IsImplement`、`EqualType`；以及缓存的 `StringType`/`GUIDType` 等 |
| `DefaultValue` | ORM 可空默认（`DefaultIntValue` 等为 null） |
| `BlockThreadPool`（命名空间 `Buffalo.Kernel.TreadPoolManager`） | 可跟踪/超时回收的线程：`RunThread` / `RunParamThread` / `StopAll` |
| `DefaultProxyBuilder` + `IInterceptor` | IL 动态代理；`BeforeCall` / `AfterCall` |
| `ObjectEventsUnit.ClearAllEvents` | 与 `CommonMethods.ClearAllEvents` 相同 |

---

## 实现新功能时的检查清单

1. 是否已有模块能覆盖？存储/缓存/MQ/DB 走对应工厂，不要新包一层 SDK。
2. 工具方法是否已在 Kernel / ArgCommon？对照上面的表。
3. 返回给调用方的结果能否用 `APIResault`？Storage 已经这样做。
4. 连接串能否用 `ConnStringFilter`？新参数用 `key=value;` 追加，不要换格式。
5. 热路径反射是否改成 `FastValueGetSet` / `ClassInfoManager`？
6. 公开命名是否与历史拼写一致？
)
