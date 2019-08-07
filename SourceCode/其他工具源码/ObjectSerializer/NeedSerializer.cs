using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.Kernel.ObjectSerializer
{
    /// <summary>
    /// ���Ϊ��Ҫ���л�
    /// </summary>
    public class NeedSerializer:System.Attribute
    {
        /// <summary>
        /// ���Ϊ��Ҫ���л�
        /// </summary>
        /// <param name="name">�����</param>
        public NeedSerializer(string name) 
        {
            _name = name;
        }
        /// <summary>
        /// ���Ϊ��Ҫ���л�
        /// </summary>
        /// <param name="name">�����</param>
        public NeedSerializer():this(null)
        {
        }

        private string _name;
        /// <summary>
        /// �����
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        
    }
}
