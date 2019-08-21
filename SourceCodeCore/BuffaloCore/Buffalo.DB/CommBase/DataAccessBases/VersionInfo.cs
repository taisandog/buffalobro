using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.EntityInfos;

namespace Buffalo.DB.CommBase.DataAccessBases
{
    /// <summary>
    /// ����������Ϣ
    /// </summary>
    public class VersionInfo
    {
        /// <summary>
        /// �汾��Ϣ
        /// </summary>
        /// <param name="info">������Ϣ</param>
        /// <param name="oldValue">��ֵ</param>
        /// <param name="newValue">��ֵ</param>
        public VersionInfo(EntityPropertyInfo info, object oldValue, object newValue) 
        {
            _info = info;
            _oldValue = oldValue;
            _newValue=newValue;
        }
        EntityPropertyInfo _info;
        /// <summary>
        /// ������Ϣ
        /// </summary>
        public EntityPropertyInfo Info
        {
            get { return _info; }
            set { _info = value; }
        }
        object _newValue;
        /// <summary>
        /// ��ֵ
        /// </summary>
        public object NewValue
        {
            get { return _newValue; }
            set { _newValue = value; }
        }
        object _oldValue;
        /// <summary>
        /// ��ֵ
        /// </summary>
        public object OldValue
        {
            get { return _oldValue; }
            set { _oldValue = value; }
        }

    }
}
