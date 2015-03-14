using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.GeneratorInfo
{
    /// <summary>
    /// 属性的外键信息
    /// </summary>
    public class RelationInfo
    {

        /// <summary>
        /// 关联信息
        /// </summary>
        /// <param name="targetName">目标实体的属性名</param>
        /// <param name="sourceName">本实体的关联属性</param>
        /// <param name="isParent">是否主表属性</param>
        /// <param name="sourceType">源属性的类型</param>
        /// <param name="sourceTypeFullName">源属性的类型全名</param>
        public RelationInfo(string targetName,
            string sourceName, bool isParent, string sourceType,string sourceTypeFullName) 
        {
            _targetName = targetName;
            _sourceName = sourceName;
            _isParent = IsParent;
            _sourceType = sourceType;
            _sourceTypeFullName = sourceTypeFullName;
        }
        
        private string _targetName;
        /// <summary>
        /// 目标实体的属性名
        /// </summary>
        public string TargetName
        {
            get { return _targetName; }
        }

        
        private string _sourceName;
        /// <summary>
        /// 本实体的关联属性
        /// </summary>
        public string SourceName
        {
            get { return _sourceName; }
        }

        private string _sourceType;
        /// <summary>
        /// 关联属性的数值类型
        /// </summary>
        public string SourceType
        {
            get { return _sourceType; }
        }

        private string _sourceTypeFullName;
        /// <summary>
        /// 关联属性的数值类型全名
        /// </summary>
        public string SourceTypeFullName
        {
            get { return _sourceTypeFullName; }
        }
        private bool _isParent;
        /// <summary>
        /// 是否主表属性
        /// </summary>
        public bool IsParent
        {
            get { return _isParent; }
        }


    }
}
