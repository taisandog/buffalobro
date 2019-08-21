using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.DB.PropertyAttributes
{
    using Sequence = SequenceAttributes;

    /// <summary>
    /// ���б�ʶ
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class SequenceAttributes:System.Attribute
    {
        /// <summary>
        /// ������
        /// </summary>
        private string _propertyName;
        /// <summary>
        /// ������
        /// </summary>
        public string PropertyName
        {
            get { return _propertyName; }
        }
        /// <summary>
        /// ������
        /// </summary>
        private string _sequenceName;
        /// <summary>
        /// ������
        /// </summary>
        public string SequenceName
        {
            get { return _sequenceName; }
        }
        /// <summary>
        /// ���б�ʶ
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="sequenceName"></param>
        public SequenceAttributes(string propertyName, string sequenceName) 
        {
            _propertyName = propertyName;
            _sequenceName = sequenceName;
        }
    }
}
