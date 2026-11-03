# Buffalo.Data.PostgreSQL 数据库方言适配器指南

`Buffalo.Data.PostgreSQL` 是 `Buffalo.DB` 针对 PostgreSQL 数据库的高性能方言适配实现。负责将 BQL 语法树映射为 PostgreSQL 原生 SQL，支持高并发场景下的高效查询与分页。

---

## 1. 核心命名空间与职责

- 命名空间：`Buffalo.Data.PostgreSQL`
- 适配器加载入口：`Buffalo.Data.PostgreSQL.AdapterLoader`（实现 `IAdapterLoader`）
- 适配器执行核心：`Buffalo.Data.PostgreSQL.DBAdapter`
- 序列与自增管理：`Buffalo.Data.PostgreSQL.SequenceManager`
- 分页生成器：`Buffalo.Data.PostgreSQL.CutPageSqlCreater`

### 主要功能特性：
1. **原生 LIMIT / OFFSET 分页**：直接生成符合 PG 标准的 `LIMIT {limit} OFFSET {offset}`。
2. **自增键与 RETURNING 机制**：
   - 插入自增列（`SERIAL` / `BIGSERIAL` / `GENERATED ALWAYS AS IDENTITY`）时支持安全回填主键。
3. **参数化处理**：
   - 自动映射 `@p` 参数占位符，支持 Npgsql 底层类型转换。
4. **函数方言适配**：
   - `COALESCE`、`NOW()`、`EXTRACT`、`TO_CHAR` 等 PG 原生内置函数对齐。
5. **架构（Schema）探测**：支持在 `public` 或自定义 schema 下探测表结构与主键约束。

---

## 2. 配置与连接方式

在业务项目的数据库配置文件或连接串中声明：

```xml
<?xml version="1.0" encoding="utf-8" ?>
<dbinfo type="Buffalo.Data.PostgreSQL.AdapterLoader, Buffalo.Data.PostgreSQL" name="PgDB">
  <connectionString>Host=127.0.0.1;Port=5432;Database=car_db;Username=postgres;Password=123456;Timeout=15;CommandTimeout=30;</connectionString>
</dbinfo>
```

---

## 3. 业务层配合规范

```csharp
// 开发者依然编写标准 BQL：
ScopeList lstScope = new ScopeList();
lstScope.Add(CIWDB.CIWAccount.Status == 1);
lstScope.SortList.Add(CIWDB.CIWAccount.Id, SortType.Desc);
lstScope.Page = new PageContent { CurrentPage = 1, PageSize = 20 };

List<CIWAccount> accounts = accountBo.SelectList(lstScope);
```

---

## 4. AI 避坑指南

1. **大小写敏感与双引号**：PostgreSQL 默认会将无引号标识符转换为小写。如果字段或表名在大写情况下创建，底层会自动使用双引号 `"` 包裹处理，业务层调用 BQL 时切勿手写双引号。
2. **布尔类型处理**：PostgreSQL 原生具备 `boolean` 类型（`true/false`），与 MySQL 惯用的 `TINYINT(1)` 不同，Buffalo.Data.PostgreSQL 会自动完成类型映射转换。
