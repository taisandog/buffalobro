using System;
using System.Data;
using System.Configuration;


namespace Buffalo.WebKernel.WebCommons.PostForms
{
    /// <summary>
    /// 发送请求的文件类
    /// </summary>
    public class FormFile
    {
        /* 上传文件的数据 */
        private byte[] _data;
        /* 文件名称 */
        private string _fileName;
        /* 表单字段名称*/
        private string _formName;
        /* 内容类型 */
        private String _contentType = "application/octet-stream"; //需要查阅相关的资料  

        public FormFile(string filename, byte[] data, String formName, String contentType)
        {
            this._data = data;
            this._fileName = filename;
            this._formName = formName;
            if (contentType != null) this._contentType = contentType;
        }
        public FormFile(string filename, byte[] data, String formName)
            : this(filename, data, formName, null)
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
            set
            {
                _data = value;
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
            set
            {
                _fileName = value;
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
            set
            {
                _formName = value;
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
            set
            {
                _contentType = value;
            }
        }

    }

}