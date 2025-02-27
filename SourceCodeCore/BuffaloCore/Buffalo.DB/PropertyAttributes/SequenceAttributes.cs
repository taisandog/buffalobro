using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.DB.PropertyAttributes
{
    using Sequence = SequenceAttributes;

    /// <summary>
    /// 序列标识
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class SequenceAttributes:System.Attribute
    {
        /// <summary>
        /// 属性名
        /// </summary>
        private string _propertyName;
        /// <summary>
        /// 属性名
        /// </summary>
        public string PropertyName
        {
            get { return _propertyName; }
        }
        /// <summary>
        /// 序列名
        /// </summary>
        private string _sequenceName;
        /// <summary>
        /// 序列名
        /// </summary>
        public string SequenceName
        {
            get { return _sequenceName; }
        }
        /// <summary>
        /// 序列标识
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
