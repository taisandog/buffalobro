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
    /// 当前时间的句柄
    /// </summary>
    public class BQLNowDateHandle : BQLParamHandle
    {
        bool _isUTC = false;
        /// <summary>
        /// 当前时间
        /// </summary>
        /// <param name="dbType">类型</param>
        /// <param name="isUCT">是否格林威治时间</param>
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
