# Buffalo.ArgCommon 统一通讯协议库调用指南

`Buffalo.ArgCommon` 规范了整个系统所有 API 接口、控制器以及跨模块调用的请求入参解析与统一返回结果格式。
基于 `Newtonsoft.Json` 构建，依赖 `Buffalo.Kernel`。

---

## 1. 核心命名空间与设计规范

- 默认命名空间：`Buffalo.ArgCommon`
- 全局扩展（无命名空间，直接全局调用）：`JsonValueConvertExtend`
- 核心规范：所有 API 返回结果统一为结构化 JSON：
  ```json
  {
    "state": 1,
    "message": "提示信息",
    "data": { ... }
  }
  ```
  `state` 对应 `ResaultType` 枚举：
  - `None = 0`
  - `Success = 1`（成功）
  - `Fault = 2`（业务失败/校验未通过）
  - `Exception = 3`（系统异常）
  - `Timeout = 4`（超时/登录凭证过期）

---

## 2. 核心类与方法详解

### 2.1 工厂类：`ApiCommon`
严禁通过 `new APIResault()` 手动拼装返回结果，统一使用静态工厂方法创建：

| 静态方法 | 用途 | 典型代码 |
|---|---|---|
| `APIResault GetSuccess(string message = null, object data = null)` | 返回成功 | `return ApiCommon.GetSuccess("操作成功", user);` |
| `APIResault GetFault(string message, object data = null)` | 返回业务失败 | `return ApiCommon.GetFault("密码错误");` |
| `APIResault GetException(Exception ex, string message = null)` | 返回异常 | `return ApiCommon.GetException(ex);` |
| `APIResault GetTimeout(string message = null, object data = null)` | 返回超时或认证失效 | `return ApiCommon.GetTimeout("会话已过期，请重新登录");` |
| `ArgValues GetArgs(string json)` | 解析客户端 JSON 入参 | `ArgValues args = ApiCommon.GetArgs(jsonString);` |
| `XmlDocument NewXmlDocument()` | 创建规范的 XmlDocument 实例 | `XmlDocument doc = ApiCommon.NewXmlDocument();` |
| `Dictionary<string, string> XmlToDictionary(string xml)` | XML 转字典 | 解析简单参数 XML |
| `string DictionaryToXml(IDictionary dic)` | 字典转 XML | 打包参数 XML |

---

### 2.2 结果承载实体：`APIResault`
包含了状态、提示信息与泛型载荷数据，支持链式序列化定制。

#### 核心属性
- `ResaultType Resault`：结果状态枚举（序列化时对应 json 的 `state`）。
- `bool IsSuccess`：快捷判断是否成功（`Resault == ResaultType.Success`）。
- `string Message`：文本提示信息。
- `object Data`：载荷数据。

#### 核心方法与链式扩展
| 方法 | 功能与说明 |
|---|---|
| `T GetValue<T>()` | 将 `Data` 转为指定的强类型对象（底层走 `JsonValueConvertExtend`，支持 JObject/JArray/字典）。 |
| `APIResault SetSuccess(string message, object data)` | 设置为成功。 |
| `APIResault SetFault(string message, object data)` | 设置为业务失败。 |
| `APIResault SetException(Exception ex)` | 设置为异常。 |
| `string ToJson()` | 序列化为规范 JSON 字符串。 |
| `APIResault FromShowProperty(IEnumerable<string> props)` | 序列化白名单：仅序列化指定属性。 |
| `APIResault FromIgnoreProperty(IEnumerable<string> props)` | 序列化黑名单：忽略某些敏感属性（如 Password、Salt）。 |
| `string ToJsonShow(IEnumerable<string> props)` | 仅序列化指定字段为 JSON。 |
| `string ToJsonIgnore(IEnumerable<string> props)` | 排除指定字段后序列化为 JSON。 |
| `APIResault FromDataToString()` | 将 `Data` 预先序列化为 JSON 字符串，保证传输层嵌套安全。 |

---

### 2.3 入参字典封装：`ArgValues`
继承自 `Dictionary<string, object>`，专为解析 WebAPI 客户端传入的 JSON/表单参数设计。

```csharp
// 典型提取入参方式：
ArgValues args = ApiCommon.GetArgs(json);

string token = args.Token;                        // 快捷读取键 "Token"
T model = args.GetDataValue<T>();                 // 读取键 "data" 并反序列化为 T
int userId = args.GetDataValue<int>("id", 0);      // 安全读取指定 key
string keyword = args.GetDataValue<string>("key");
object raw = args.GetObject("customParam");
```

---

### 2.4 全局 JSON 转换扩展：`JsonValueConvertExtend`（无命名空间）
提供超越普通类型转换的高级反序列化，能自动处理 `JToken`、`JObject`、`JArray` 以及原生 JSON 字符串：

```csharp
// 1. 扩展方法形式
T entity = obj.ConvertJsonTo<T>();

// 2. 静态方法形式
T entity = JsonValueConvertExtend.ConvertJsonValue<T>(obj, defaultValue);
```

---

## 3. WebAPI 标准编码范例（Golden Snippet）

在 Controller 中接收参数与返回结果的规范写法：

```csharp
using Buffalo.ArgCommon;
using Buffalo.Kernel;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    [HttpPost("login")]
    public string Login([FromBody] Dictionary<string, object> oargs)
    {
        if (oargs == null)
        {
            return ApiCommon.GetFault("参数不能为空").ToJson();
        }

        try
        {
            ArgValues args = new ArgValues(oargs);
            string userCode = args.GetDataValue<string>("userCode");
            string password = args.GetDataValue<string>("password");

            if (CommonMethods.IsNullOrWhiteSpace(userCode))
            {
                return ApiCommon.GetFault("用户名不可为空").ToJson();
            }

            // 调用 BusinessModel 层处理业务
            AccountBusiness bo = new AccountBusiness();
            APIResault res = bo.DoLogin(userCode, password);

            // 过滤敏感字段输出
            return res.FromIgnoreProperty(new[] { "Password", "Salt" }).ToJson();
        }
        catch (Exception ex)
        {
            return ApiCommon.GetException(ex).ToJson();
        }
    }
}
```

---

## 4. AI 避坑指南

1. **不要混用 `System.Text.Json`**：BuffaloCore 规范全链路基于 `Newtonsoft.Json`。
2. **严禁自定义返回格式**：禁止手动定义 `{ code = 200, msg = "ok", data = ... }`，必须统一使用 `ApiCommon.GetSuccess` 等方法返回 `APIResault`。
3. **敏感属性过滤**：返回前利用 `.FromIgnoreProperty(...)` 清除密码、密钥等属性，切勿手写属性置空。
4. **方法命名兼容**：注意类名为 `APIResault`，枚举为 `ResaultType`（历史公开拼写，请勿改动）。
