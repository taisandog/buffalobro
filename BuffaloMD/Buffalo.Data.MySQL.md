# Buffalo.Data.MySQL 数据库方言适配器指南

`Buffalo.Data.MySQL` 是 `Buffalo.DB` 针对 MySQL 及 MariaDB 数据库的方言适配实现。负责将 BQL 语法树与分页请求转换为高效的 MySQL 本地原生 SQL 执行。

---

## 1. 核心命名空间与职责

- 命名空间：`Buffalo.Data.MySQL`
- 适配器加载入口：`Buffalo.Data.MySQL.AdapterLoader`（实现 `IAdapterLoader`）
- 适配器执行核心：`Buffalo.Data.MySQL.DBAdapter`
- 分页生成器：`Buffalo.Data.MySQL.CutPageSqlCreater`

### 主要功能特性：
1. **高效分页**：使用 MySQL 原生 `LIMIT offset, rowCount` 语法构建分页 SQL。
2. **标识符转义**：自动使用反引号 \` 对表名与保留关键字字段名进行转义。
3. **自增键回填**：利用 `SELECT LAST_INSERT_ID()` 自动获取并回填新增实体自增主键。
4. **函数方言适配**：
   - 常用函数（`CommonFunction`）：`CONCAT`、`IFNULL`、`NOW` 等。
   - 转换函数（`ConvertFunction`）：`CAST`、`CONVERT`、日期与字符串格式化。
   - 聚合与数学函数（`AggregateFunctions` / `MathFunctions`）。
5. **元数据读取**：`DBStructure` 实现对 MySQL `information_schema` 的表结构、主外键、字段类型的自动化探测。

---

## 2. 配置与连接方式

在业务项目的数据库配置文件（如 `CIWDB.xml`）或连接串中指定 MySQL 适配类型：

```xml
<?xml version="1.0" encoding="utf-8" ?>
<dbinfo type="Buffalo.Data.MySQL.AdapterLoader, Buffalo.Data.MySQL" name="CIWDB">
  <connectionString>server=127.0.0.1;port=3306;database=CarInstallDB;uid=root;pwd=123456;charset=utf8mb4;SslMode=none;Max Pool Size=100;</connectionString>
</dbinfo>
```

或在 `Program.cs` 启动时直接通过连接串配置：
```csharp
Buffalo.Kernel.AppSetting.Default.FillDBInfo(CIWDB.GetDBinfo(), null);
```

---

## 3. 业务层配合规范

业务层代码（如 `BusinessModelBase<T>`）对具体数据库是透明无感的，所有适配均在底层由 `Buffalo.Data.MySQL` 自动完成：

```csharp
// 开发者只需像往常一样编写 BQL 语句：
ScopeList lstScope = new ScopeList();
lstScope.Add(CIWDB.CIWAccount.Status == 1);
lstScope.Page = new PageContent { CurrentPage = 1, PageSize = 20 };

// 底层自动生成标准 MySQL: SELECT ... FROM `CIWAccount` WHERE `Status` = 1 LIMIT 0, 20
List<CIWAccount> list = accountBo.SelectList(lstScope);
```

---

## 4. AI 避坑指南

1. **连接串务必声明 `charset=utf8mb4`**：为了完整支持 Emoji 表情符号及特殊多字节字符，连接串必须包含 `charset=utf8mb4`。
2. **不要手写 `LIMIT` 或 MySQL 专有函数**：分页必须通过 `lstScope.Page = new PageContent(...)`，禁止在业务层拼接 `LIMIT` 字符串。
