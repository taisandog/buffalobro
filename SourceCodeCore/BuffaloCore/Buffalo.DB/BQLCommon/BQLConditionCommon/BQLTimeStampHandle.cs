using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;

namespace Buffalo.DB.BQLCommon.BQLConditionCommon
{
    /// <summary>
    /// ʱ���
    /// </summary>
    public class BQLTimeStampHandle: BQLParamHandle
    {
        /// <summary>
        /// ��ǰʱ��
        /// </summary>
        /// <param name="dbType">����</param>
        /// <param name="isUCT">�Ƿ��������ʱ��</param>
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
