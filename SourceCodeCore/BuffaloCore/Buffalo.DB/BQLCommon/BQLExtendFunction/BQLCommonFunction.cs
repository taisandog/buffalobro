using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.BQLCommon.BQLConditionCommon;
using Buffalo.DB.BQLCommon.BQLAggregateFunctions;
using System.Data;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.DB.QueryConditions;

namespace Buffalo.DB.BQLCommon.BQLExtendFunction
{
    public delegate string DelCommonFunction(string[] items,DBInfo info);
    public class CsqCommonFunction : BQLParamHandle
    {
        public DelCommonFunction funHandle;
        private BQLValueItem[] values;
        public CsqCommonFunction(BQLValueItem[] values, DelCommonFunction funHandle,DbType dbType)
        {
            this.funHandle = funHandle;
            this.values = values;
            this._valueDbType = dbType;
        }

        internal override void FillInfo(KeyWordInfomation info)
        {
            foreach (BQLValueItem item in values) 
            {
                BQLValueItem.DoFillInfo(item, info);
            }
        }

        internal override string DisplayValue(KeyWordInfomation info)
        {
            DelCommonFunction handle = funHandle;
            if (handle != null)
            {
                List<string> lstParams = new List<string>();
                foreach (BQLValueItem item in values) 
                {
                    lstParams.Add(item.DisplayValue(info));
                }
                //SelectCondition con = info.Condition as SelectCondition;
                //if (con != null) 
                //{
                //    con.HasGroup = true;
                //}
                return handle(lstParams.ToArray(),info.DBInfo);
            }
            return null;
        }
    }
}
