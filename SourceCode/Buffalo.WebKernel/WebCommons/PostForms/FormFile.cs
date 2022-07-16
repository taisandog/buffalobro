using System;
using System.Data;
using System.Configuration;
using System.IO;

namespace Buffalo.WebKernel.WebCommons.PostForms
{
    /// <summary>
    /// 发送请求的文件类
    /// </summary>
    public class FormFile:IDisposable
    {
        /// <summary>
        /// 上传文件的数据
        /// </summary>
        private byte[] _data;

        /// <summary>
        /// 文件名称
        /// </summary>
        private string _fileName;
        /// <summary>
        /// 表单字段名称
        /// </summary>
        private string _formName;
        /// <summary>
        /// 内容类型
        /// </summary>
        private String _contentType = "application/octet-stream"; //需要查阅相关的资料  

        /// <summary>
        /// 流
        /// </summary>
        private Stream _dataStream;


        public FormFile(string filename, byte[] data,Stream dataStream, String formName, String contentType)
        {
            this._data = data;
            this._fileName = filename;
            this._formName = formName;
            _dataStream=dataStream; 

            if (contentType != null) this._contentType = contentType;
        }
        public FormFile(string filename, byte[] data, String formName)
            : this(filename, data,null, formName, null)
        {
        }
        public FormFile(string filename, Stream dataStream, int length, String formName)
            : this(filename, null, dataStream, formName, null)
        {
        }

        /// <summary>
        /// 上传文件的数据
        /// </summary>
        public byte[] Data
        {
            get
            {
                return _data;
            }

        }
        
        /// <summary>
        /// 上传文件的数据流
        /// </summary>
        public Stream DataStream
        {
            get
            {
                return _dataStream;
            }

        }
        /// <summary>
        /// 文件名称
        /// </summary>
        public String FileName
        {
            get
            {
                return _fileName;
            }

        }

        /// <summary>
        /// 表单字段名称
        /// </summary>
        public string FormName
        {
            get
            {
                return _formName;
            }

        }

        /// <summary>
        /// 内容类型
        /// </summary>
        public string ContentType
        {
            get
            {
                return _contentType;
            }

        }

        public void Dispose()
        {
            _data = null;
            _dataStream = null;
        }
    }

}