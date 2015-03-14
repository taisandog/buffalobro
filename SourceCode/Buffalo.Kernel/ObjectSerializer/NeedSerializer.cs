using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.Kernel.ObjectSerializer
{
    /// <summary>
    /// 标记为需要序列化
    /// </summary>
    public class NeedSerializer:System.Attribute
    {
        /// <summary>
        /// 标记为需要序列化
        /// </summary>
        /// <param name="name">标记名</param>
        public NeedSerializer(string name) 
        {
            _name = name;
        }
        /// <summary>
        /// 标记为需要序列化
        /// </summary>
        /// <param name="name">标记名</param>
        public NeedSerializer():this(null)
        {
        }

        private string _name;
        /// <summary>
        /// 标记名
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        
    }
}
