# Buffalo.Data.DB2 数据库方言适配器指南

`Buffalo.Data.DB2` 是 `Buffalo.DB` 针对 IBM DB2 数据库的企业级方言适配实现。负责将 BQL 语法树转换为 DB2 原生 SQL，并适配其特有的分页机制与系统函数。

---

## 1. 核心命名空间与职责

- 命名空间：`Buffalo.Data.DB2`
- 适配器加载入口：`Buffalo.Data.DB2.AdapterLoader`（实现 `IAdapterLoader`）
- 适配器执行核心：`Buffalo.Data.DB2.DBAdapter`
- 分页生成器：`Buffalo.Data.DB2.CutPageSqlCreater` / `Buffalo.Data.DB2.CursorPageCutter`

### 主要功能特性：
1. **企业级开窗函数分页**：
   - 使用 `ROW_NUMBER() OVER(ORDER BY ...)` 窗口函数进行高精度两层嵌套分页，保证在巨量数据下的分页稳定性。
2. **标识符与参数前缀适配**：
   - 参数占位符规范为 `@p` 或 `:p`，自动适配 DB2 字符集与数据类型。
3. **函数方言适配**：
   - `VALUE` / `COALESCE`、`CURRENT TIMESTAMP`、`CHAR`、`VARCHAR_FORMAT` 等 DB2 专有函数映射。
4. **系统目录（SYSCAT）探测**：
   - 通过 `SYSCAT.TABLES` 和 `SYSCAT.COLUMNS` 进行模式探测。

---

## 2. 配置与连接方式

在业务项目的数据库配置文件或连接串中声明：

```xml
<?xml version="1.0" encoding="utf-8" ?>
<dbinfo type="Buffalo.Data.DB2.AdapterLoader, Buffalo.Data.DB2" name="DB2Database">
  <connectionString>Database=SAMPLE;Server=127.0.0.1:50000;UID=db2admin;PWD=123456;CurrentSchema=DB2ADMIN;</connectionString>
</dbinfo>
```

---

## 3. 业务层配合规范

```csharp
// 开发者依然编写标准 BQL：
ScopeList lstScope = new ScopeList();
lstScope.Add(CIWDB.CIWAccount.Status == 1);
lstScope.SortList.Add(CIWDB.CIWAccount.Id, SortType.Asc);
lstScope.Page = new PageContent { CurrentPage = 1, PageSize = 20 };

List<CIWAccount> list = accountBo.SelectList(lstScope);
```

---

## 4. AI 避坑指南

1. **DB2 分页必须显式指定排序列**：由于 DB2 采用 `ROW_NUMBER() OVER()` 语法，如果未给 `lstScope.SortList` 添加排序字段，底层会自动使用主键兜底，建议在业务层始终显式添加排序字段保证结果确定性。
2. **CurrentSchema 指定**：连接串中尽量显式带上 `CurrentSchema=...;`，避免不同账号默认 schema 差异导致的“找不到表”异常。
