using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Buffalo.DBTools.UIHelper.ModelLoader
{
    /// <summary>
    /// 编译错误的异常
    /// </summary>
    public class CompileException:Exception
    {

        public CompileException():base()
        {

        }
        
        public CompileException(string message)
            :base(message)
        {

        }

        protected CompileException(SerializationInfo info, StreamingContext context) 
            :base(info,context)
        {

        }

        public CompileException(string message, Exception innerException) 
            :base(message,innerException)
        {

        }

        private string _code;
        /// <summary>
        /// 生成代码
        /// </summary>
        public string Code
        {
            get { return _code; }
            internal set { _code = value; }
        }
    }
}
