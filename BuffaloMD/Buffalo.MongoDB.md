# Buffalo.MongoDB 文档数据库操作指南

`Buffalo.MongoDB` 提供了面向 MongoDB 的强类型封装，包含实体映射、自增主键支持、条件查询构造器 `ConditionList`、更新器 `UpdateList`、事务支持以及生命周期管理。

---

## 1. 核心命名空间与类型

- 核心管理器：`Buffalo.MongoDB.MongoDBManager`
- 实体基类：`Buffalo.MongoDB.ProxyBase.MongoEntityBase`
- 操作执行器：`Buffalo.MongoDB.MongoDBOperate<T>`
- 条件与更新容器：`Buffalo.MongoDB.QueryCondition.ConditionList<T>`、`Buffalo.MongoDB.QueryCondition.UpdateList<T>`
- 事务支持：`Buffalo.MongoDB.MongoTransaction`

---

## 2. 数据库配置与初始化：`MongoDBManager`

在应用启动时，通过 `MongoDBManager.AddConfig` 注册 Mongo 连接与实体所在的程序集：

```csharp
using Buffalo.MongoDB;

// 注册 MongoDB 连接
// key: 数据库配置唯一键
// connectionString: mongodb://username:pwd@127.0.0.1:27017
// dbName: 数据库名
// hasTransaction: 是否启用集群事务支持
// params Assembly[]: 实体所在的程序集（用于自动初始化实体元数据与自增列）
MongoDBManager.AddConfig("MainMongo", "mongodb://127.0.0.1:27017", "LogDatabase", false, typeof(LogMongoEntity).Assembly);
```

---

## 3. 实体定义规范：`MongoEntityBase`

所有存储到 MongoDB 的文档实体，必须继承自 `MongoEntityBase`：

```csharp
using Buffalo.MongoDB.ProxyBase;
using MongoDB.Bson;
using System;

public class UserActionLog : MongoEntityBase
{
    // Mongo 内部主键 _id 映射在基类的 Id (ObjectId) 属性中
    
    public long UserId { get; set; }
    public string Action { get; set; }
    public string IP { get; set; }
    public DateTime CreateTime { get; set; }
    public string Remark { get; set; }
}
```

---

## 4. 核心操作类：`MongoDBOperate<T>`

获取操作器：统一使用 `MongoDBManager.CreateOperate<T>()`：

```csharp
MongoDBOperate<UserActionLog> operate = MongoDBManager.CreateOperate<UserActionLog>();
```

### 4.1 核心 API 速查
| 方法 | 说明 |
|---|---|
| `void Insert(TDocument entity, bool hasIdentity = true)` | 插入单条实体（`hasIdentity=true` 支持数字自增列回填） |
| `void InsertList(IEnumerable<TDocument> lstEntity, ...)` | 批量插入实体 |
| `List<TDocument> SelectList(ConditionList<TDocument> query)` | 根据条件查询实体列表 |
| `TDocument GetUnique(ConditionList<TDocument> query)` | 查询符合条件的单条记录 |
| `void Update(ConditionList<TDocument> query, UpdateList<TDocument> updateList)` | 批量更新满足条件的文档 |
| `void UpdateOne(ConditionList<TDocument> query, UpdateList<TDocument> updateList)` | 更新满足条件的单条文档 |
| `void Delete(ConditionList<TDocument> query)` | 批量删除满足条件的文档 |
| `void Delete(ObjectId id)` | 根据 ObjectId 主键删除文档 |
| `MongoTransaction StartTransaction()` | 开启 MongoDB 会话级事务（需副本集支持） |

---

## 5. 标准编码范例（Golden Snippet）

```csharp
using Buffalo.MongoDB;
using Buffalo.MongoDB.QueryCondition;
using MongoDB.Bson;
using System;
using System.Collections.Generic;

public class MongoLogService
{
    private readonly MongoDBOperate<UserActionLog> _oper = MongoDBManager.CreateOperate<UserActionLog>();

    // 1. 插入日志
    public void AddLog(long userId, string action, string ip)
    {
        UserActionLog log = new UserActionLog
        {
            UserId = userId,
            Action = action,
            IP = ip,
            CreateTime = DateTime.UtcNow
        };
        _oper.Insert(log);
    }

    // 2. 多条件检索
    public List<UserActionLog> QueryLogs(long userId, DateTime startTime, DateTime endTime)
    {
        ConditionList<UserActionLog> conditions = new ConditionList<UserActionLog>();
        conditions.Add(x => x.UserId == userId);
        conditions.Add(x => x.CreateTime >= startTime);
        conditions.Add(x => x.CreateTime <= endTime);

        return _oper.SelectList(conditions);
    }

    // 3. 部分字段条件更新
    public void UpdateLogRemark(long userId, string newRemark)
    {
        ConditionList<UserActionLog> query = new ConditionList<UserActionLog>();
        query.Add(x => x.UserId == userId);

        UpdateList<UserActionLog> updates = new UpdateList<UserActionLog>();
        updates.Add(x => x.Remark, newRemark);

        _oper.Update(query, updates);
    }

    // 4. 事务操作
    public void TransactionOperation()
    {
        using (MongoTransaction tran = _oper.StartTransaction())
        {
            try
            {
                // 事务内的插入与更新
                _oper.Insert(new UserActionLog { Action = "step1" });
                _oper.Insert(new UserActionLog { Action = "step2" });
                
                tran.Commit();
            }
            catch
            {
                tran.Rollback();
                throw;
            }
        }
    }
}
```

---

## 6. AI 避坑指南

1. **实体基类继承**：必须继承 `MongoEntityBase`，禁止直接写无基类实体通过裸 `MongoClient` 读写。
2. **条件查询构造**：使用 `ConditionList<T>` 配合 Lambda 表达式构建过滤，避免手写 BsonDocument JSON 拼接。
3. **事务依赖**：`StartTransaction` 依赖 MongoDB 副本集（Replica Set）环境，单机 Standalone 实例不要开启 `hasTransaction: true`。
