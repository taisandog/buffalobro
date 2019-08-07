using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.EntityInfos;

namespace Buffalo.DB.CommBase.DataAccessBases
{
    /// <summary>
    /// 并发更新信息
    /// </summary>
    public class VersionInfo
    {
        /// <summary>
        /// 版本信息
        /// </summary>
        /// <param name="info">属性信息</param>
        /// <param name="oldValue">旧值</param>
        /// <param name="newValue">新值</param>
        public VersionInfo(EntityPropertyInfo info, object oldValue, object newValue) 
        {
            _info = info;
            _oldValue = oldValue;
            _newValue=newValue;
        }
        EntityPropertyInfo _info;
        /// <summary>
        /// 属性信息
        /// </summary>
        public EntityPropertyInfo Info
        {
            get { return _info; }
            set { _info = value; }
        }
        object _newValue;
        /// <summary>
        /// 新值
        /// </summary>
        public object NewValue
        {
            get { return _newValue; }
            set { _newValue = value; }
        }
        object _oldValue;
        /// <summary>
        /// 旧值
        /// </summary>
        public object OldValue
        {
            get { return _oldValue; }
            set { _oldValue = value; }
        }

    }
}
