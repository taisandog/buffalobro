using System;
namespace Buffalo.Storage
{
    public interface IFileStorage:IDisposable
    {
        /// <summary>
        /// 追加文件
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="content">文件内容</param>
        /// <param name="postion">写入起始位置</param>
        /// <returns></returns>
        bool AppendFile(string path, byte[] content, long postion);
        /// <summary>
        /// 追加到文件末尾
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="content">文件内容</param>
        /// <returns></returns>
        bool AppendFile(string path, byte[] content);
        /// <summary>
        /// 关闭连接
        /// </summary>
        void Close();
        /// <summary>
        /// 获取所有目录
        /// </summary>
        /// <param name="path">基础目录</param>
        /// <param name="searchOption">查找选项</param>
        /// <returns></returns>
        System.Collections.Generic.List<string> GetDirectories(string path, System.IO.SearchOption searchOption);
        /// <summary>
        /// 获取文件流
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="postion"></param>
        /// <returns></returns>
        System.IO.Stream GetFile(string path);
        /// <summary>
        /// 获取文件流
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="postion"></param>
        /// <returns></returns>
        System.IO.Stream GetFile(string path, long postion);
        /// <summary>
        /// 获取所有文件
        /// </summary>
        /// <param name="path">文件夹</param>
        /// <param name="searchOption">查找选项</param>
        /// <returns></returns>
        System.Collections.Generic.List<string> GetFiles(string path, System.IO.SearchOption searchOption);
        /// <summary>
        /// 打开链接
        /// </summary>
        /// <returns></returns>
        void Open();

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="path">路径</param>
        void RemoveFile(string path);
        /// <summary>
        /// 重命名文件
        /// </summary>
        /// <param name="source">原文件路径</param>
        /// <param name="target">目标文件路径</param>
        void ReNameFile(string source, string target);
        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="content">文件内容</param>
        /// <returns></returns>
        bool SaveFile(string path, byte[] content);
    }
}
