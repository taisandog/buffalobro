using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.BQLCommon.BQLConditionCommon;
using Buffalo.DB.QueryConditions;
using Buffalo.DB.BQLCommon.BQLConditions;
using Buffalo.DB.BQLCommon.BQLKeyWordCommon;
using Buffalo.DB.BQLCommon.BQLExtendFunction;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.Kernel.Defaults;
using System.Data;
using System.Collections;
using Buffalo.DB.DBFunction;

namespace Buffalo.DB.BQLCommon.BQLConditionCommon
{
    /// <summary>
    /// ��ǰʱ��ľ��
    /// </summary>
    public class BQLNowDateHandle : BQLParamHandle
    {
        bool _isUTC = false;
        /// <summary>
        /// ��ǰʱ��
        /// </summary>
        /// <param name="dbType">����</param>
        /// <param name="isUCT">�Ƿ��������ʱ��</param>
        public BQLNowDateHandle(DbType dbType,bool isUCT) 
        {
            ValueDbType = dbType;
            _isUTC = isUCT;
        }

        internal override void FillInfo(KeyWordInfomation info)
        {
        }

        internal override string DisplayValue(KeyWordInfomation info)
        {
            IDBAdapter idba = info.DBInfo.CurrentDbAdapter;
            if (!_isUTC)
            {
                return idba.GetNowDate(ValueDbType);
            }
            return idba.GetUTCDate(ValueDbType);

        }
    }

}
