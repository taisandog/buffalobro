using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;

namespace Buffalo.DB.BQLCommon.BQLConditionCommon
{
    /// <summary>
    /// 时间戳
    /// </summary>
    public class BQLTimeStampHandle: BQLParamHandle
    {
        /// <summary>
        /// 当前时间
        /// </summary>
        /// <param name="dbType">类型</param>
        /// <param name="isUCT">是否格林威治时间</param>
        public BQLTimeStampHandle() 
        {
           
        }

        internal override void FillInfo(KeyWordInfomation info)
        {
        }

        internal override string DisplayValue(KeyWordInfomation info)
        {
            IDBAdapter idba = info.DBInfo.CurrentDbAdapter;

            return idba.GetTimeStamp(System.Data.DbType.DateTime);
        }
    }
}
