using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.BQLCommon.BQLKeyWordCommon;
using Buffalo.Kernel.Defaults;
using System.Data;
using Buffalo.DB.DataBaseAdapter;

namespace Buffalo.DB.BQLCommon.BQLConditionCommon
{
    public class BQLCaseHandle:BQLParamHandle
    {
        private BQLValueItem caseItem;
        public BQLCaseHandle(BQLValueItem item)
        {
            this.caseItem = item;
            
            //this.valueType = BQLValueType.Case;
            //this.valueDataType = DefaultType.StringType;
            this._valueDbType = DbType.String;
        }

        ///// <summary>
        ///// CaseµÄÏî
        ///// </summary>
        //internal BQLKeyWordItem CaseItem 
        //{
        //    get 
        //    {
        //        return caseItem;
        //    }
        //}
        internal override void FillInfo(KeyWordInfomation info)
        {
            BQLValueItem.DoFillInfo(caseItem, info);
        }
        internal override string DisplayValue(KeyWordInfomation info)
        {
            KeyWordConver objCover = new KeyWordConver();
            
            StringBuilder sb=new StringBuilder();
            sb.Append("(");
            sb.Append(caseItem.DisplayValue(info));
            sb.Append(" end) ");
            return sb.ToString();
            
        }
    }
}
