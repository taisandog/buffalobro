using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.Kernel;

namespace Buffalo.DB.BQLCommon.BQLConditionCommon
{
    public class BQLOtherParamHandle:BQLParamHandle
    {
        BQLTableHandle table;
        string paramName;
        /// <summary>
        /// 字段名
        /// </summary>
        public string ParamName
        {
            get { return paramName; }
        }
        public BQLOtherParamHandle(BQLTableHandle table, string paramName)
        {
            this.table = table;
            this.paramName = paramName;
        }
        internal override string DisplayValue(KeyWordInfomation info)
        {
            IDBAdapter idba = info.DBInfo.CurrentDbAdapter;
            if (paramName == "*")//查询全部字段时候
            {
                return table.DisplayValue(info) + ".*";
            }
            StringBuilder sb = new StringBuilder();
            if (!CommonMethods.IsNull(table) )
            {
                BQLAliasHandle handle = info.AliasManager.GetPrimaryAliasHandle(table);
                if (Buffalo.Kernel.CommonMethods.IsNull(handle))
                {
                    sb.Append(table.DisplayValue(info));
                    sb.Append(".");
                }
                else 
                {

                    sb.Append(handle.GetAliasName());
                    sb.Append(".");
                }
            }
            sb.Append(idba.FormatParam(paramName));
            return sb.ToString();
        }

        internal override void FillInfo(KeyWordInfomation info)
        {
            BQLValueItem.DoFillInfo(table, info);

        }
    }
}
