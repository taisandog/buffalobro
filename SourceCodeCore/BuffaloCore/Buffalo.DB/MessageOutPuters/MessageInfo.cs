using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.DB.MessageOutPuters
{
    /// <summary>
    /// �����Ϣ
    /// </summary>
    public class MessageInfo:Dictionary<string,object>
    {
        //public const string Type = "Type";
        //public const string ExtendType = "ExtendType";
        //public const string Value = "Value";
        /// <summary>
        /// ��ȡֵ
        /// </summary>
        /// <param name="key">��</param>
        /// <returns></returns>
        public object GetValue(string key) 
        {
            object ret = null;
            if(this.TryGetValue(key,out ret))
            {
                return ret;
            }
            return ret;
        }

        private object _type;
        /// <summary>
        /// ����
        /// </summary>
        public object Type
        {
            get { return _type; }
            set { _type = value; }
        }
        private object _extendType;
        /// <summary>
        /// ��չ����
        /// </summary>
        public object ExtendType
        {
            get { return _extendType; }
            set { _extendType = value; }
        }

        private object _value;
        /// <summary>
        /// ֵ
        /// </summary>
        public object Value
        {
            get { return _value; }
            set { _value = value; }
        }
    }
}
