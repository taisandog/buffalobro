# Buffalo.QueryCache 缓存适配器调用指南

`Buffalo.QueryCache` 是 Buffalo 框架的分布式/本地缓存统一适配器，支持 Redis（基于 StackExchange.Redis）、Memcached、Web 内存缓存，并与 `Buffalo.DB` 的二级查询缓存无缝集成。

---

## 1. 核心命名空间与类型

- 适配器工厂：`Buffalo.QueryCache`（类 `CacheLoader`）
- 缓存接口与数据结构：`Buffalo.DB.CacheManager`、`Buffalo.DB.CacheManager.CacheCollection`
  - `ICacheAdaper`：统一缓存操作接口（同步与异步）
  - `ICacheHash`：Hash 表操作接口
  - `ICacheList`：List 队列操作接口
  - `ICacheSortedSet`：有序集合操作接口
  - `QueryCacheLock` / `QueryCacheLockAsync`：分布式锁实现

---

## 2. 缓存工厂初始化：`CacheLoader`

严禁直接通过 `new RedisClient()` 或外部第三方库构建缓存客户端，统一使用 `CacheLoader.GetCache` 工厂：

```csharp
using Buffalo.QueryCache;
using Buffalo.DB.CacheManager;

// 支持的类型标识：
// "redis"     -> 基于 StackExchange.Redis 的分布式缓存
// "memcached" -> Memcached 分布式缓存
// "web"       -> ASP.NET / 进程内本地内存缓存

string connString = "server=127.0.0.1:6379;pwd=123456;database=1;timeout=5000";
ICacheAdaper cache = CacheLoader.GetCache(null, "redis", connString);
```

---

## 3. 核心 API 与方法速查

### 3.1 键值基础操作（KV）
| 同步方法 | 异步方法 | 说明 |
|---|---|---|
| `E GetValue<E>(string key, E defaultValue, DataBaseOperate oper)` | `Task<E> GetValueAsync<E>(...)` | 读取并转换为强类型 E |
| `bool SetValue<E>(string key, E val, SetValueType type, TimeSpan expir, ...)` | `Task<bool> SetValueAsync<E>(...)` | 写入缓存并指定过期时长 |
| `bool ExistsKey(string key, DataBaseOperate oper)` | `Task<bool> ExistsKeyAsync(...)` | 判断 Key 是否存在 |
| `bool DeleteValue(string key, DataBaseOperate oper)` | `Task<bool> DeleteValueAsync(...)` | 删除 Key |
| `bool SetKeyExpire(string key, TimeSpan expir, ...)` | `Task<bool> SetKeyExpireAsync(...)` | 延长或更新 Key 过期时间 |
| `long DoIncrement(string key, ulong inc, ...)` | `Task<long> DoIncrementAsync(...)` | 原子自增 |
| `long DoDecrement(string key, ulong dec, ...)` | `Task<long> DoDecrementAsync(...)` | 原子自减 |
| `void ClearAll()` | `Task ClearAllAsync()` | 清空整个缓存库（慎用） |

> [!NOTE]
> 在独立作为缓存使用时，`DataBaseOperate oper` 参数通常传 `null` 即可。

---

### 3.2 集合与高级结构操作

`ICacheAdaper` 支持丰富的 Redis 数据结构：

#### 1. Hash 表：`GetHash(string key, DataBaseOperate oper)`
```csharp
ICacheHash hash = cache.GetHash("user:info:1001", null);
hash.SetValue("name", "张三");
string name = hash.GetValue<string>("name");
hash.DeleteValue("name");
```

#### 2. 列表队列：`GetList(string key, DataBaseOperate oper)`
```csharp
ICacheList list = cache.GetList("queue:tasks", null);
list.AddRight("task1");
string item = list.PopLeft<string>();
```

#### 3. 有序集合：`GetSortedSet(string key, DataBaseOperate oper)`
```csharp
ICacheSortedSet zset = cache.GetSortedSet("rank:board", null);
zset.Add(100.5, "player_A");
zset.Add(98.0, "player_B");
```

#### 4. 分布式锁：`GetCacheLock` / `GetCacheLockAsync`
高并发下防止超卖、防穿透的分布式排它锁：

```csharp
// 异步分布式锁标准写法：
QueryCacheLockAsync cacheLock = cache.GetCacheLockAsync("lock:order:10001", null);
if (await cacheLock.LockAsync(TimeSpan.FromSeconds(10)))
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

## 4. 业务项目典型封装范例（以单例 CacheManager 为例）

在三层架构业务项目中，通常会将 `ICacheAdaper` 包装在业务 Cache 单例中：

```csharp
public class UserTokenCache
{
    private static readonly ICacheAdaper _cache;

    static UserTokenCache()
    {
        string conn = "server=127.0.0.1:6379;pwd=xxxx;database=2";
        _cache = CacheLoader.GetCache(null, "redis", conn);
    }

    public static async Task SetUserTokenAsync(long userId, string token)
    {
        string key = $"token:user:{userId}";
        await _cache.SetValueAsync(key, token, SetValueType.Set, TimeSpan.FromDays(7), null);
    }

    public static async Task<string> GetUserTokenAsync(long userId)
    {
        string key = $"token:user:{userId}";
        return await _cache.GetValueAsync<string>(key, null, null);
    }

    public static async Task RemoveTokenAsync(long userId)
    {
        string key = $"token:user:{userId}";
        await _cache.DeleteValueAsync(key, null);
    }
}
```

---

## 5. AI 避坑指南

1. **严禁在业务中引入 `StackExchange.Redis` 直接编码**：缓存的读写必须统一使用 `CacheLoader.GetCache(...)` 获取的 `ICacheAdaper`，以保持多环境（本地开发可用 `web` 内存模式，线上环境切 `redis`）的无缝透明切换。
2. **连接串解析格式**：遵循 `server=...;pwd=...;database=...;` 键值连接串，由 Buffalo 底层自动通过 `ConnStringFilter` 规范解析。
3. **安全使用 `SetValueType`**：`SetValueType.Set` 为无条件覆盖写入，`SetValueType.AddNew` 为仅当不存在时写入（NX 模式）。
