# Buffalo.Data.Oracle 数据库方言适配器指南

`Buffalo.Data.Oracle` 是 `Buffalo.DB` 针对 Oracle 10g/11g/12c+ 数据库的方言适配实现。负责将 BQL 语法树、参数绑定与分页请求转换为高效的 Oracle PL/SQL 原生语法。

---

## 1. 核心命名空间与职责

- 命名空间：`Buffalo.Data.Oracle`
- 适配器加载入口：`Buffalo.Data.Oracle.AdapterLoader`（实现 `IAdapterLoader`）
- 适配器执行核心：`Buffalo.Data.Oracle.DBAdapter` / `Buffalo.Data.Oracle.DBAdapter11`
- 序列管理器：`Buffalo.Data.Oracle.SequenceManager`
- 分页生成器：`Buffalo.Data.Oracle.CutPageSqlCreater` / `Buffalo.Data.Oracle.CursorPageCutter`

### 主要功能特性：
1. **多版本分页兼容**：
   - 针对 Oracle 11g 及早期版本，采用高效嵌套 `ROWNUM` 子查询方式分页；
   - 针对高版本支持游标分页或 `OFFSET ... FETCH NEXT` 规范。
2. **序列支持（Sequence）**：
   - 提供 `SequenceManager` 自动管理与获取 Oracle 序列当前值与下一个值（`NEXTVAL`），回填实体主键。
3. **参数化前缀转换**：
   - 自动将 SQL 参数占位符规范为 Oracle 专用的冒号 `:`（例如 `:p1`），防止注入。
4. **函数方言适配**：
   - `NVL`、`DECODE`、`TO_CHAR`、`TO_DATE`、`SYSDATE` 等 Oracle 原生函数对齐。
5. **双引号转义**：严格处理 Oracle 的大小写敏感表名与保留关键字转义。

---

## 2. 配置与连接方式

在业务项目的数据库配置文件或连接串中声明：

```xml
<?xml version="1.0" encoding="utf-8" ?>
<dbinfo type="Buffalo.Data.Oracle.AdapterLoader, Buffalo.Data.Oracle" name="OracleDB">
  <connectionString>Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=127.0.0.1)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=orcl)));User Id=myuser;Password=mypassword;</connectionString>
</dbinfo>
```

---

## 3. 业务层配合规范与序列配置

在 Oracle 实体模型中，如果主键通过序列生成，可在 BDM/ETM 配置文件或实体模型特性中指定 Sequence 名称：

```csharp
// 开发者依然编写标准 BQL，适配器会自动处理参数前缀与序列获取：
ScopeList lstScope = new ScopeList();
lstScope.Add(CIWDB.CIWAccount.Age >= 18);
lstScope.Page = new PageContent { CurrentPage = 1, PageSize = 10 };

List<CIWAccount> accounts = accountBo.SelectList(lstScope);
```

---

## 4. AI 避坑指南

1. **日期处理切勿手写字符串格式化**：Oracle 对日期时间格式要求极为严格，必须使用 `Buffalo.Kernel.CommonMethods.ConvertDateTimeInt` 或直接传递 `DateTime` 对象走参数化绑定，严禁手工拼装 `TO_DATE('...', 'YYYY-MM-DD')` 字符串。
2. **表名与字段名保留字**：避免使用 `NUMBER`、`COMMENT`、`USER` 等作为表名或字段名；如必须使用，由底层方言自动加双引号保护。
