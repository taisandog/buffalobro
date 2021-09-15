using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Buffalo.DBTools.UIHelper.ModelLoader
{
    /// <summary>
    /// ���������쳣
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
        /// ���ɴ���
        /// </summary>
        public string Code
        {
            get { return _code; }
            internal set { _code = value; }
        }
    }
}
