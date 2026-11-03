# Buffalo.DB ORM 核心框架调用指南

`Buffalo.DB` 是 BuffaloCore 的核心自研 ORM 引擎，提供基于面向对象的强类型查询语言 **BQL**（Buffalo Query Language）、实体状态跟踪、三层架构与业务模型基类封装、多数据库适配机制以及事务支持。

---

## 1. 架构定位与核心体系

Buffalo.DB 与代码生成插件配合，形成清晰的三层 + 工厂架构：

```text
┌────────────────────────────────────────────────────────┐
│               业务层 Business (BusinessModelBase<T>)   │
└──────────────────────────┬─────────────────────────────┘
                           │ 调用查询、增删改、事务
┌──────────────────────────▼─────────────────────────────┐
│          数据访问层 DataAccess (BQLDataAccessBase<T>)  │
│          由 DataAccessLoader 反射或工厂统一加载          │
└──────────────────────────┬─────────────────────────────┘
                           │ 组装 SQL / 参数
┌──────────────────────────▼─────────────────────────────┐
│   实体与查询句柄 BQLEntity / BQL (BQLDataBaseHandle)    │
│   实体类继承 EntityBase；数据库表操作句柄继承 BQLEntity  │
└────────────────────────────────────────────────────────┘
```

---

## 2. 核心类与方法详解

### 2.1 业务模型基类：`BusinessModelBase<T>` / `BusinessModelBaseForSelect<T>`
业务逻辑层（BO 类）直接继承该类，即可获得开箱即用的 CRUD、强类型查询与事务支持。

#### 核心查询方法
| 方法签名 | 说明 |
|---|---|
| `T GetEntityById(object id)` / `Task<T> GetEntityByIdAsync(object id)` | 根据主键获取实体 |
| `T GetUnique(ScopeList lstScope)` / `Task<T> GetUniqueAsync(ScopeList lstScope)` | 根据条件查询单条实体（若多条取首条） |
| `List<T> SelectList(ScopeList lstScope)` / `Task<List<T>> SelectListAsync(...)` | 根据条件和分页/排序查询实体列表 |
| `DataSet Select(ScopeList lstScope)` / `Task<DataSet> SelectAsync(...)` | 根据条件查询 DataSet |
| `long SelectCount(ScopeList lstScope)` / `Task<long> SelectCountAsync(...)` | 统计满足条件的记录总数 |
| `bool ExistsRecord(ScopeList lstScope)` / `Task<bool> ExistsRecordAsync(...)` | 判断是否存在满足条件的记录 |

#### 核心增删改方法
| 方法签名 | 说明 |
|---|---|
| `object Insert(T entity, ValueSetList setList = null, bool fillIdentity = false)` | 新增实体（`fillIdentity=true` 回填自增 ID） |
| `Task<object> InsertAsync(T entity, ...)` | 异步新增实体 |
| `object Update(T entity, ScopeList scorpList = null, ...)` | 更新修改过的脏属性，或按指定 ScopeList 更新 |
| `Task<object> UpdateAsync(T entity, ...)` | 异步更新实体 |
| `object Delete(T entity)` / `Task<object> DeleteAsync(T entity)` | 删除指定实体 |
| `object DeleteById(object id)` / `Task<object> DeleteByIdAsync(object id)` | 按主键删除实体 |
| `object Delete(ScopeList lstScope)` / `Task<object> DeleteAsync(ScopeList lstScope)` | 按批量条件删除记录 |

---

### 2.2 条件构建：`ScopeList`、`SortList`、`PageContent`

在 Buffalo.DB 中，所有查询、批量更新、批量删除的条件均通过 `ScopeList` 承载：

#### 核心属性与用法
- `lstScope.Add(BQLCondition)`：追加查询条件（相当于 SQL 中的 AND）。
- `lstScope.Page = new PageContent { PageSize = 20, CurrentPage = 1 }`：配置分页。
  - 查询后，`lstScope.Page.TotalRecord` 会自动填充为总记录数。
- `lstScope.SortList.Add(BQLParamHandle, SortType.Desc)`：添加排序字段（`SortType.Asc` 或 `SortType.Desc`）。
- `lstScope.ForUpdate = BQLLockType.LockUpdate`：加排它锁（`SELECT ... FOR UPDATE`），用于高并发防超卖/防重复处理。
- `lstScope.UseCache = false`：是否绕过二级缓存。

---

### 2.3 强类型查询语言：`BQL` 与 `BQLDataBaseHandle`

BQL 是 Buffalo 区别于传统 ORM 的核心特色。代码生成插件会为每个数据库生成一个 `BQLDataBaseHandle`（例如 `CIWDB`），每个表生成静态属性（例如 `CIWDB.CIWAccount`）：

#### 运算符重载支持
- 等于：`CIWDB.CIWAccount.UserCode == code`
- 不等于：`CIWDB.CIWAccount.Status != 0`
- 大于/小于：`CIWDB.CIWAccount.Age >= 18`、`CIWDB.CIWAccount.Score < 60`
- 模糊查询（Like）：`CIWDB.CIWAccount.Name.Like("%张%")`
- 包含（In）：`CIWDB.CIWAccount.Id.In(new object[] { 1, 2, 3 })`
- 判空：`CIWDB.CIWAccount.Phone.IsNull()` / `CIWDB.CIWAccount.Phone.IsNotNull()`
- 与或组合：`(CIWDB.User.Age > 20) & (CIWDB.User.Status == 1)` 或 `(A == 1) | (B == 2)`

---

### 2.4 事务管理：`DBTransation` / `DBTransactionAsync`

```csharp
// 1. 同步事务
using (DBTransation trans = new DBTransation())
{
    try
    {
        AccountBusiness accountBo = new AccountBusiness();
        OrderBusiness orderBo = new OrderBusiness();
        
        accountBo.Update(user);
        orderBo.Insert(order);

        trans.Commit(); // 提交事务
    }
    catch (Exception ex)
    {
        trans.Rollback(); // 出现异常回滚
        throw;
    }
}

// 2. 异步事务
await using (DBTransactionAsync trans = new DBTransactionAsync())
{
    try
    {
        await accountBo.UpdateAsync(user);
        await orderBo.InsertAsync(order);
        await trans.CommitAsync();
    }
    catch
    {
        await trans.RollbackAsync();
        throw;
    }
}
```

---

### 2.5 数据访问加载器：`DataAccessLoader`

```csharp
// 初始化加载数据库配置
DataAccessLoader.AddDBConfig(new string[] { "CIWDB.config.xml" });

// 获取指定实体的 DataAccess 实例
IDataAccessModel<AccountInfo> da = DataAccessLoader.GetInstanceByEntity<AccountInfo>();
```

---

## 3. 标准三层业务编码范例（Golden Snippets）

### 3.1 典型业务查询与分页
```csharp
using Buffalo.DB.QueryConditions;
using Buffalo.ArgCommon;
using System.Collections.Generic;

public class AccountBusiness : BusinessModelBase<CIWAccount>
{
    /// <summary>
    /// 分页搜索用户列表
    /// </summary>
    public List<CIWAccount> SearchUsers(string keyword, int pageIndex, int pageSize, out long total)
    {
        ScopeList lstScope = new ScopeList();
        
        // 1. 动态追加条件
        if (!CommonMethods.IsNullOrWhiteSpace(keyword))
        {
            lstScope.Add(CIWDB.CIWAccount.Name.Like("%" + keyword + "%") | 
                         CIWDB.CIWAccount.Phone == keyword);
        }
        lstScope.Add(CIWDB.CIWAccount.Status == 1);

        // 2. 排序
        lstScope.SortList.Add(CIWDB.CIWAccount.CreateTime, SortType.Desc);

        // 3. 分页
        lstScope.Page = new PageContent { CurrentPage = pageIndex, PageSize = pageSize };

        // 4. 执行查询
        List<CIWAccount> list = SelectList(lstScope);
        total = lstScope.Page.TotalRecord;
        return list;
    }

    /// <summary>
    /// 加悲观排它锁读取用户（防并发篡改）
    /// </summary>
    public CIWAccount GetUserForUpdate(long userId)
    {
        ScopeList lstScope = new ScopeList();
        lstScope.Add(CIWDB.CIWAccount.Id == userId);
        lstScope.ForUpdate = BQLLockType.LockUpdate; // SELECT ... FOR UPDATE

        return GetUnique(lstScope);
    }
}
```

### 3.2 实体增删改
```csharp
AccountBusiness bo = new AccountBusiness();

// 1. 新增
CIWAccount user = new CIWAccount();
user.UserCode = "U001";
user.Name = "张三";
bo.Insert(user, fillIdentity: true); // user.Id 会自动回填

// 2. 按实体更新脏属性
user.Name = "李四";
bo.Update(user);

// 3. 批量条件更新
ScopeList updateScope = new ScopeList();
updateScope.Add(CIWDB.CIWAccount.Status == 0);

ValueSetList values = new ValueSetList();
values.Add(CIWDB.CIWAccount.Status, 1);
bo.Update(updateScope, values);
```

---

## 4. AI 避坑指南

1. **绝对禁止手写原生 SQL 拼接**：不要写 `SELECT * FROM User WHERE ...`，所有的单表/多表查询统一通过 `ScopeList` 和 `CIWDB.表名.列名` 表达式构建。
2. **禁止裸调 `new SqlConnection()` 或 `DbContext`**：Buffalo.DB 自带连接池与底层适配，所有的业务操作必须封装在继承自 `BusinessModelBase<T>` 的 BO 类中。
3. **主键自增回填**：需要获取插入后的主键自增 ID 时，必须传递 `fillIdentity: true`。
4. **分页结果总数提取**：调用 `SelectList(lstScope)` 后，总数会自动写回到 `lstScope.Page.TotalRecord`，无需二次调用 `SelectCount`。
