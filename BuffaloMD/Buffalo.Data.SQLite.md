# Buffalo.Data.SQLite 数据库方言适配器指南

`Buffalo.Data.SQLite` 是 `Buffalo.DB` 针对 SQLite 嵌入式本地数据库的方言适配实现。专为桌面客户端、本地单机服务、移动端工具或离线缓存设计。

---

## 1. 核心命名空间与职责

- 命名空间：`Buffalo.Data.SQLite`
- 适配器加载入口：`Buffalo.Data.SQLite.AdapterLoader`（实现 `IAdapterLoader`）
- 适配器执行核心：`Buffalo.Data.SQLite.DBAdapter`
- 分页生成器：`Buffalo.Data.SQLite.CutPageSqlCreater`

### 主要功能特性：
1. **轻量级零配置部署**：无需独立数据库服务端，基于单文件运行。
2. **高效 LIMIT / OFFSET 分页**：自动生成 `LIMIT {limit} OFFSET {offset}`。
3. **自增主键支持**：
   - 插入支持 `INTEGER PRIMARY KEY AUTOINCREMENT`，并通过 `last_insert_rowid()` 自动回填。
4. **内置函数适配**：
   - `IFNULL`、`DATETIME('now')`、`SUBSTR` 等 SQLite 支持的函数映射。
5. **动态类型兼容**：处理 SQLite 弱类型（Manifest Typing）与 C# 强类型之间的自动安全转换。

---

## 2. 配置与连接方式

在桌面应用（如 WinForms/WPF）或离线服务的配置文件或连接串中声明：

```xml
<?xml version="1.0" encoding="utf-8" ?>
<dbinfo type="Buffalo.Data.SQLite.AdapterLoader, Buffalo.Data.SQLite" name="LocalDB">
  <connectionString>Data Source=C:\data\local_app.db;Version=3;Journal Mode=WAL;Pooling=True;</connectionString>
</dbinfo>
```

> [!TIP]
> 推荐在连接串中开启 `Journal Mode=WAL`（Write-Ahead Logging），能大幅提升多线程并发读写的性能并避免文件锁死。

---

## 3. 业务层配合规范

```csharp
// 开发者依然编写标准 BQL：
ScopeList lstScope = new ScopeList();
lstScope.Add(CIWDB.CIWAccount.Status == 1);
lstScope.Page = new PageContent { CurrentPage = 1, PageSize = 50 };

List<CIWAccount> list = accountBo.SelectList(lstScope);
```

---

## 4. AI 避坑指南

1. **并发写入文件锁（Database is locked）**：SQLite 是单写并发读的轻量数据库，高频并发写入时必须结合 `Buffalo.Kernel.Lock` 或连接池串行化，不可同时开多事务并行写入同一个文件。
2. **避免大批量长事务**：事务必须尽快 Commit，避免长时间独占数据库文件句柄。
