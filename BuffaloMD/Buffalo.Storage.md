# Buffalo.Storage 统一对象存储调用指南

`Buffalo.Storage` 为各类文件与对象存储（阿里云 OSS、腾讯云 COS、华为云 OBS、AWS S3 以及本地文件系统 Local）提供了高度统一的多后端抽象层。所有文件变更接口均统一返回 `Buffalo.ArgCommon.APIResault`。

---

## 1. 核心命名空间与类型

- 命名空间：`Buffalo.Storage`
- 存储工厂类：`Buffalo.Storage.FSCreater`
- 核心操作接口：`Buffalo.Storage.IFileStorage`
- 文件信息实体：`Buffalo.Storage.FileInfoBase`

---

## 2. 存储工厂初始化：`FSCreater.Create`

严禁在业务中直接引用具体的云厂商 SDK（如 `Aliyun.OSS` 或 `QCloud.CosApi`）进行业务开发，统一通过工厂创建：

```csharp
using Buffalo.Storage;

// 支持的类型名称（不区分大小写）：
// "local" -> 本地磁盘文件存储
// "OSS"   -> 阿里云 OSS
// "COS"   -> 腾讯云 COS
// "OBS"   -> 华为云 OBS

// 连接串示例：
string ossConn = "Server=oss-cn-shenzhen.aliyuncs.com;SecretId=LTAIxxxx;SecretKey=xxxx;BucketName=my-bucket;InternetUrl=https://my-bucket.oss-cn-shenzhen.aliyuncs.com;";
IFileStorage storage = FSCreater.Create("OSS", ossConn);
```

---

## 3. 核心 API 方法速查

`IFileStorage` 提供了全套同步与异步方法，方法签名完全对称：

### 3.1 文件保存与上传
| 同步方法 | 异步方法 | 说明 |
|---|---|---|
| `APIResault SaveFile(string path, Stream content, long contentLength)` | `Task<APIResault> SaveFileAsync(...)` | 将流数据保存到指定云端路径 |
| `APIResault SaveFile(string sourcePath, string targetPath)` | `Task<APIResault> SaveFileAsync(...)` | 将本地文件上传到存储路径 |
| `APIResault AppendFile(string path, Stream content)` | `Task<APIResault> AppendFileAsync(...)` | 追加写入文件 |

### 3.2 文件读取与下载
| 同步方法 | 异步方法 | 说明 |
|---|---|---|
| `Stream GetFileStream(string path)` | `Task<Stream> GetFileStreamAsync(...)` | 获取文件的只读网络流 |
| `void ReadFileToStream(string path, Stream targetStream)` | `Task ReadFileToStreamAsync(...)` | 将远端文件读取并写入指定目标流 |
| `FileInfoBase GetFileInfo(string path)` | `Task<FileInfoBase> GetFileInfoAsync(...)` | 获取指定文件的详细元数据（大小、哈希、修改时间等） |
| `List<FileInfoBase> GetFiles(string path, SearchOption opt)` | `Task<List<FileInfoBase>> GetFilesAsync(...)` | 遍历指定目录下的所有文件 |

### 3.3 文件与目录维护
| 同步方法 | 异步方法 | 说明 |
|---|---|---|
| `APIResault RemoveFile(string path)` | `Task<APIResault> RemoveFileAsync(...)` | 删除单个文件 |
| `APIResault RenameFile(string source, string target)` | `Task<APIResault> RenameFileAsync(...)` | 重命名/移动文件 |
| `bool ExistsFile(string path)` | `Task<bool> ExistsFileAsync(...)` | 判断文件是否存在 |
| `APIResault CreateDirectory(string folder)` | `Task<APIResault> CreateDirectoryAsync(...)` | 创建目录 |
| `APIResault RemoveDirectory(string path)` | `Task<APIResault> RemoveDirectoryAsync(...)` | 删除目录 |
| `bool ExistDirectory(string folder)` | `Task<bool> ExistDirectoryAsync(...)` | 判断目录是否存在 |

---

## 4. 业务项目典型范例（Golden Snippet）

在 WebAPI 中接收上传文件，并通过 `Buffalo.Storage` 保存到云端：

```csharp
using Buffalo.Storage;
using Buffalo.ArgCommon;
using System.IO;
using System.Threading.Tasks;

public class FileService
{
    private readonly IFileStorage _storage;

    public FileService()
    {
        // 实际配置建议从 appsettings 或配置管理器中获取
        string conn = "Server=oss-cn-shenzhen.aliyuncs.com;SecretId=xxx;SecretKey=xxx;BucketName=my-bucket;InternetUrl=https://cdn.xxx.com;";
        _storage = FSCreater.Create("OSS", conn);
    }

    /// <summary>
    /// 上传用户头像并返回外网 CDN 访问地址
    /// </summary>
    public async Task<APIResault> UploadAvatarAsync(long userId, Stream fileStream, string originalFileName)
    {
        string ext = Path.GetExtension(originalFileName);
        string cloudPath = $"/avatars/{userId}_{DateTime.UtcNow.Ticks}{ext}";

        // 异步保存流
        APIResault res = await _storage.SaveFileAsync(cloudPath, fileStream, fileStream.Length);
        if (!res.IsSuccess)
        {
            return res; // 保存失败，直接返回错误信息
        }

        // 拼接公网 URL 返回
        string publicUrl = _storage.InternetUrl.TrimEnd('/') + cloudPath;
        return ApiCommon.GetSuccess("上传成功", publicUrl);
    }

    /// <summary>
    /// 检查并安全删除旧文件
    /// </summary>
    public async Task<APIResault> DeleteFileSafelyAsync(string cloudPath)
    {
        if (await _storage.ExistsFileAsync(cloudPath))
        {
            return await _storage.RemoveFileAsync(cloudPath);
        }
        return ApiCommon.GetSuccess("文件不存在，跳过删除");
    }
}
```

---

## 5. AI 避坑指南

1. **绝对禁止在业务层硬编码 SDK 专有类**：例如不要在业务代码中出现 `OssClient`、`PutObjectRequest`、`CosXmlServer` 等，所有操作统一调用 `IFileStorage` 接口。
2. **返回值校验统一用 `APIResault.IsSuccess`**：存储操作不会裸抛常规业务异常，操作失败会返回包含错误信息的 `APIResault`，调用方必须判断 `res.IsSuccess`。
3. **资源释放**：`IFileStorage` 实现了 `IDisposable` 和 `IAsyncDisposable`，在作用域生命周期内建议配合 `using` 或由容器管理单例。
