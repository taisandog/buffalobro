﻿using System;
using System.IO;
using Buffalo.ArgCommon;
using System.Security.Cryptography;

namespace Buffalo.Storage
{
    public abstract class IFileStorage : IDisposable
    {
        /// <summary>
        /// 追加文件
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="content">文件内容</param>
        /// <param name="postion">写入起始位置</param>
        /// <returns></returns>
        public abstract APIResault AppendFile(string path, Stream content, long postion);
        /// <summary>
        /// 追加到文件末尾
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="content">文件内容</param>
        /// <returns></returns>
        public abstract APIResault AppendFile(string path, Stream content);
        /// <summary>
        /// 关闭连接
        /// </summary>
        public abstract APIResault Close();

        /// <summary>
        /// 哈希类型
        /// </summary>
        public abstract string HashName { get; }
        /// <summary>
        /// 获取哈希类
        /// </summary>
        public abstract HashAlgorithm GetHash();
        /// <summary>
        /// 获取所有目录
        /// </summary>
        /// <param name="path">基础目录</param>
        /// <param name="searchOption">查找选项</param>
        /// <returns></returns>
        public abstract System.Collections.Generic.List<string> GetDirectories(string path, System.IO.SearchOption searchOption);
        /// <summary>
        /// 获取文件流
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="postion"></param>
        /// <returns></returns>
        public abstract System.IO.Stream GetFileStream(string path);
        /// <summary>
        /// 获取文件流
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="postion">位置</param>
        /// <returns></returns>
        public abstract System.IO.Stream GetFileStream(string path, long postion);
        /// <summary>
        /// 把文件读取到流
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="stm">要写入的流</param>
        /// <param name="postion">读取的起始位置(0则从头开始)</param>
        /// <param name="length">要读取的长度(小于等于0则表示要读完)</param>
        public abstract void ReadFileToStream(string path, Stream stm, long postion, long length);

        /// <summary>
        /// 获取文件信息
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public abstract FileInfoBase GetFileInfo(string path);
        /// <summary>
        /// 获取所有文件
        /// </summary>
        /// <param name="path">文件夹</param>
        /// <param name="searchOption">查找选项</param>
        /// <returns></returns>
        public abstract System.Collections.Generic.List<FileInfoBase> GetFiles(string path, System.IO.SearchOption searchOption);
        /// <summary>
        /// 打开链接
        /// </summary>
        /// <returns></returns>
        public abstract APIResault Open();

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="path">路径</param>
        public abstract APIResault RemoveFile(string path);
        /// <summary>
        /// 重命名文件
        /// </summary>
        /// <param name="source">原文件路径</param>
        /// <param name="target">目标文件路径</param>
        public abstract APIResault RenameFile(string source, string target);
        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="content">文件内容</param>
        /// <param name="contentLength">内容长度，为0时候则从流获取长度</param>
        /// <returns></returns>
        public abstract APIResault SaveFile(string path, Stream content,long contentLength);
        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="sourcePath">原路径</param>
        /// <param name="targetPath">目标路径</param>
        /// <returns></returns>
        public abstract APIResault SaveFile(string sourcePath, string targetPath);
        /// <summary>
        /// 删除文件夹
        /// </summary>
        /// <param name="path">路径</param>
        public abstract APIResault RemoveDirectory(string path);
        /// <summary>
        /// 文件夹是否存在
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        public abstract bool ExistDirectory(string folder);
        /// <summary>
        /// 文件是否存在
        /// </summary>
        /// <param name="path">文件夹</param>
        /// <returns></returns>
        public abstract bool ExistsFile(string path);
        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        public abstract APIResault CreateDirectory(string folder);

        public abstract void Dispose();


        /// <summary>
        /// 把文件读取到流
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="stm">要写入的流</param>
        public void ReadFileToStream(string path, Stream stm)
        {
            ReadFileToStream(path, stm, 0, -1);
        }
        /// <summary>
        /// 把文件读取到流
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="stm">要写入的流</param>
        /// <param name="postion">读取的起始位置(0则从头开始)</param>
        public void ReadFileToStream(string path, Stream stm,long postion)
        {
            ReadFileToStream(path, stm, postion, -1);
        }
    }
}