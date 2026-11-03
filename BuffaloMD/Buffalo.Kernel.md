# Buffalo.Kernel 核心基础库调用指南

`Buffalo.Kernel` 是整个 BuffaloCore 框架的底层核心基石，没有任何项目依赖，被所有上层模块引用。
在编写任何业务代码或底层扩展时，**必须优先复用本库中的 API，严禁重复造轮子**。

---

## 1. 核心命名空间与主要职责

- 默认命名空间：`Buffalo.Kernel`
- 全局扩展（无命名空间，直接全局调用）：`ValueConvertExtend`
- 快速反射与元数据：`Buffalo.Kernel.FastReflection`、`Buffalo.Kernel.FastReflection.ClassInfos`
- 线程池与并发管理：`Buffalo.Kernel.TreadPoolManager`
- 集合与锁扩展：`Buffalo.Kernel.Collections`、`Buffalo.Kernel.Defaults`

---

## 2. 常用工具类与核心 API 详解

### 2.1 全局类型转换：`ValueConvertExtend`（无命名空间）
统一处理 `null`、`DBNull`、空字符串转换，失败时返回类型默认值或指定默认值。

| 方法签名 | 说明 | 示例 |
|---|---|---|
| `T ConvertValue<T>(object value, T defaultValue = default)` | 通用安全类型转换 | `int n = ValueConvertExtend.ConvertValue<int>(val, 0);` |
| `T ConvertTo<T>(this object value, T defaultValue = default)` | 扩展方法版通用转换 | `long len = reader["DATA_LENGTH"].ConvertTo<long>();` |
| `T GetRowValue<T>(this DataRow row, string columnName, T defaultValue = default)` | DataRow 安全读值 | `string name = row.GetRowValue<string>("NAME");` |
| `T GetMapValue<T>(this IDictionary ht, object key, T defaultValue = default)` | 非泛型字典读值 | `int flag = ht.GetMapValue<int>("flag", 0);` |
| `K GetDicValue<T, K>(this IDictionary<T, K> dic, T key, K defaultValue = default)` | 泛型字典安全读值 | `string val = dic.GetDicValue("token", "");` |

> [!WARNING]
> 严禁使用 `Convert.ChangeType`、`Convert.ToInt32` 或裸 `(int)obj` 进行可能为空的类型转换。

---

### 2.2 基础工具集：`CommonMethods` (`Buffalo.Kernel`)

| 方法签名 | 功能与使用说明 |
|---|---|
| `string GetBaseRoot()` / `GetBaseRoot(string configRoot)` | 获取程序基目录。Web 程序必须在启动时调用 `ConfigureServices`；入参为相对路径时会自动与基路径拼接。 |
| `void ConfigureServices(IHostEnvironment hostingEnvironment)` | Web/Host 启动时注入环境，使 `IsWebContext` 为 true。 |
| `bool IsNullOrWhiteSpace(string value)` | 判断字符串是否为 null、空或空白字符。 |
| `bool IsCollectionNullOrEmpty(ICollection lst)` | 判断集合是否为 null 或空。 |
| `string GuidToString(Guid id, bool isUpper = true)` | GUID 转 32 位 Hex（**基于字节流，不可用 Guid.ToString("N") 替代**）。 |
| `Guid StringToGuid(string id)` | 32 位 Hex 转回 GUID。 |
| `DateTime ConvertIntDateTime(double d, bool useSecond = true, bool isUTC = true)` | Unix 时间戳转为 DateTime（支持秒/毫秒、UTC/本地）。 |
| `double ConvertDateTimeInt(DateTime time, bool useSecond = true, bool isUTC = true)` | DateTime 转为 Unix 时间戳。 |
| `byte[] LoadStreamData(Stream stm)` | 一次性读取已知 Length 的流到 byte 数组。 |
| `byte[] LoadStreamData2(Stream stm)` | 缓冲读取未知长度（如网络流）到 byte 数组。 |
| `void CopyStreamData(Stream src, Stream dst, long length = -1)` | 流拷贝，length < 0 表示拷贝到末尾。 |
| `string BytesToHexString(byte[] bye, bool isUpper = true)` | 字节数组转 Hex 字符串。 |
| `byte[] HexStringToBytes(string str)` | Hex 字符串转字节数组。 |
| `void ClearAllEvents(object objectHasEvents)` | 利用反射安全清空某个对象上的所有事件订阅。 |

---

### 2.3 哈希与加密：`PasswordHash` (`Buffalo.Kernel`)
内部提供线程本地缓存优化，支持批量高效哈希。

| 方法签名 | 说明 |
|---|---|
| `string ToMD5String(string text, Encoding encoding = null)` | 默认转大写 32 位 MD5 Hex |
| `byte[] ToMD5Bytes(byte[] bytes)` | 计算 MD5 原始字节 |
| `string ToSHA1String(string text)` / `byte[] ToSHA1Bytes(...)` | SHA1 算法计算 |
| `byte[] AESEncrypt(byte[] data, byte[] key, byte[] iv, ...)` | AES 加密（默认 key 为 32 字节） |
| `byte[] AESDecrypt(byte[] data, byte[] key, byte[] iv, ...)` | AES 解密 |
| `IDisposable StartBatchHash()` | 开启同一线程内复用 MD5/SHA1 实例的高性能上下文 |

---

### 2.4 连接串解析：`ConnStringFilter` (`Buffalo.Kernel`)
统一解析形如 `server=127.0.0.1;port=3306;pwd=123;` 的连接配置。

```csharp
// 解析为不区分大小写的键值字典（值会自动 UrlDecode）
Dictionary<string, string> connInfo = ConnStringFilter.GetConnectInfo(connString);
string server = connInfo.GetDicValue("server", "127.0.0.1");
```

---

### 2.5 并发与锁控制

1. **`Lock`**：带超时的同步对象锁（默认 15 秒超时抛 `TimeoutException`，杜绝死锁）：
   ```csharp
   using (new Lock(syncRoot))
   {
       // 临界区逻辑
   }
   // 或者尝试拿锁不抛异常：
   using (Lock lck = new Lock(syncRoot, 2000, false))
   {
       if (lck.IsTimeout) { return; }
       // 拿锁成功
   }
   ```
2. **`AsyncTaskLock<T>`**：基于 Key 的异步上下文锁（同一个 async 调用链路内相同 key 不重复死锁）：
   ```csharp
   using (AsyncTaskLock<string> lck = new AsyncTaskLock<string>(userId))
   {
       await lck.LockAsync(cancellationToken);
       // 异步临界区
   }
   ```
3. **`LockObjects<TKey>`**：按 Key 获取独立互斥对象：
   ```csharp
   private static LockObjects<string> _locks = new LockObjects<string>();
   lock (_locks.GetObject(accountNumber))
   {
       // 仅针对此 accountNumber 互斥
   }
   ```
4. **`LinkedDictionary<TKey, TValue>`**：带活跃度淘汰机制的 LRU 字典：
   ```csharp
   LinkedDictionary<string, byte[]> lru = new LinkedDictionary<string, byte[]>();
   lru.TrimCount(5000); // 保留最新访问的 5000 条，淘汰冷数据
   ```

---

### 2.6 高性能反射：`Buffalo.Kernel.FastReflection`
在热路径中禁止直接使用原生 `System.Reflection`，统一使用动态发射 IL 缓存的快速反射：

```csharp
// 1. 动态读写属性/字段
FastValueGetSet.SetValue(user, "Tom", "Name", typeof(UserInfo));
object name = FastValueGetSet.GetValue(user, "Name", typeof(UserInfo));

// 2. 句柄缓存优化（推荐循环中调用）
PropertyInfoHandle handle = FastValueGetSet.GetPropertyInfoHandle("Name", typeof(UserInfo));
handle.SetValue(user, "Tom");

// 3. 快速构造对象（比 Activator.CreateInstance 快数倍）
object instance = FastValueGetSet.CreateObject(typeof(UserInfo));

// 4. 对象浅拷贝
ClassInfoManager.ObjectCopyByProperty(sourceObj, targetObj);
```

---

### 2.7 延迟动作与事务后置补偿：`MethodTransactions` / `MethodTransactionsAsync`
用于在事务或复杂业务完成后，按优先级有序触发后置副作用（如记录日志、发消息、清理缓存等）：

```csharp
using (MethodTransactions tx = new MethodTransactions())
{
    // High 先执行，Low 后执行
    tx.AddArgsAction(args => SendSms((string)args[0]), new object[] { "13800000000" }, DelayActionLevel.High);
    
    // 执行业务...
    
    tx.Commit(); // 统一执行队列动作；若失败调用 tx.Rollback() 清空
}
```

---

### 2.8 线程池：`BlockThreadPool` (`Buffalo.Kernel.TreadPoolManager`)
可监控、可限制并发上限并具备阻塞等待回收特性的线程池管理器：
```csharp
BlockThreadPool pool = new BlockThreadPool(5, 10);
pool.RunParamThread(new ParameterizedThreadStart(DoWork), state);
pool.StopAll();
```

---

## 3. AI 编码规则与禁令

1. **绝对禁止自造工具函数**：判断空串、类型转换、时间戳、Hex/Guid 互转、流拷贝、MD5/SHA1 加密必须使用 `CommonMethods`、`ValueConvertExtend`、`PasswordHash`。
2. **尊重历史拼写**：`TreadPoolManager`、`MassManager`、`APIResault` 等为既有公开 API，切勿“纠正”拼写。
3. **全局扩展命名空间**：`ValueConvertExtend` 位于全局命名空间，不需要加 `using Buffalo.Kernel.ValueConvert;`。
