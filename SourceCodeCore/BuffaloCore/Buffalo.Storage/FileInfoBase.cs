using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffalo.Storage
{
    /// <summary>
    /// 文件信息
    /// </summary>
    public abstract class FileInfoBase
    {
        /// <summary>
        /// 大文件大小
        /// </summary>
        public const int SLICE_UPLOAD_FILE_SIZE = 8 * 1024 * 1024;
        /// <summary>
        /// 相对路径
        /// </summary>
        protected string _relativePath;
        /// <summary>
        /// 相对路径
        /// </summary>
        public string RelativePath
        {
            get
            {
                return _relativePath;
            }
        }
        /// <summary>
        /// 文件路径
        /// </summary>
        protected string _filePath;
        /// <summary>
        /// 文件路径
        /// </summary>
        public virtual string FilePath
        {
            get { return _filePath; }
        }
        /// <summary>
        /// 文件名
        /// </summary>
        protected string _fileName;
        /// <summary>
        /// 文件名
        /// </summary>
        public virtual string FileName
        {
            get { return _fileName; }
        }
        /// <summary>
        /// 扩展名
        /// </summary>
        protected string _extendName;
        /// <summary>
        /// 扩展名
        /// </summary>
        public virtual string ExtendName
        {
            get { return _extendName; }
        }

        /// <summary>
        /// 哈希码
        /// </summary>
        public abstract string Hash
        {
            get;
        }
        /// <summary>
        /// 文件长度
        /// </summary>
        protected long _length;
        /// <summary>
        /// 文件长度
        /// </summary>
        public virtual long Length
        {
            get { return _length; }
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        protected DateTime _createTime;
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateTime
        {
            get { return _createTime; }
        }

        /// <summary>
        /// 修改时间
        /// </summary>
        protected DateTime _updateTime;
        /// <summary>
        /// 修改时间
        /// </summary>
        public virtual DateTime UpdateTime
        {
            get { return _updateTime; }
        }

        /// <summary>
        /// 获取文件名
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetFileName(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return null;
            }
            url = url.Trim(' ', '\r', '\n');
            string[] paths = url.Split('/', '\\');

            return paths[paths.Length - 1];
        }

        /// <summary>
        /// 编码文件名
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string EncodeFileName(string name)
        {
            return System.Web.HttpUtility.UrlEncode(name);
        }

        public override string ToString()
        {
            return _relativePath;
        }
        /// <summary>
        /// 获取范围的结尾
        /// </summary>
        /// <param name="postion"></param>
        /// <param name="length"></param>
        /// <param name="fileLength"></param>
        /// <returns></returns>
        public static long GetRangeEnd(long postion,long length,long fileLength)
        {
            long fileEnd = fileLength - 1;
            if (length <= 0)
            {
                return fileEnd;
            }
            long end = postion + length-1;
            if(end> fileEnd)
            {
                end = fileEnd;
            }
            
            return end;
        }
        /// <summary>
        /// 拼接网址
        /// </summary>
        /// <param name="baseUri"></param>
        /// <param name="relativeOrAbsoluteUri"></param>
        /// <returns></returns>
        public static string CombineUriToString(params string[] values)
        {
            Uri uri = null;
            Uri cururi = null;
            foreach (string value in values)
            {
                if (uri != null)
                {
                    cururi = new Uri(uri, value);
                }
                else
                {
                    cururi = new Uri(value);
                }
                uri = cururi;
            }
            if (cururi == null)
            {
                return "";
            }
            return cururi.ToString();
        }

        public static readonly DateTime DefaultDate = new DateTime(1970, 1, 1);




        /// <summary>
        /// 格式化Key
        /// </summary>
        /// <param name="url"></param>
        public static string FormatKey(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return url;
            }
            return url.TrimStart(' ', '/', '\\');
        }
        /// <summary>
        /// 格式化Key
        /// </summary>
        /// <param name="url"></param>
        public static string FormatPathKey(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return "/";
            }
            if (url.Length > 1)
            {
                url = url.TrimStart(' ', '/', '\\');
            }
            if (url.Length < 1 || url[url.Length - 1] != '/')
            {
                url = url + "/";
            }
            return url;
        }
    }
}
