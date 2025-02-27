using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.BQLCommon.BQLConditionCommon;
using Buffalo.DB.BQLCommon.IdentityInfos;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;
using Buffalo.DB.DataBaseAdapter;
using System.Data;
using Buffalo.Kernel;

namespace Buffalo.DB.BQLCommon.BQLKeyWordCommon
{
    public class KeyWordInsertValueItem : BQLQuery
    {
        private BQLValueItem[] valueHandles;
        /// <summary>
        /// Insert的字段关键字项
        /// </summary>
        /// <param name="valueHandles">值的集合</param>
        /// <param name="previous">上一个关键字</param>
        internal KeyWordInsertValueItem(BQLValueItem[] valueHandles, BQLQuery previous)
            : base( previous) 
        {
            this.valueHandles = valueHandles;
        }
        
        internal override void LoadInfo(KeyWordInfomation info)
        {

        }
        ///// <summary>
        ///// 要插入的值
        ///// </summary>
        //internal BQLValueTypeItem[] ValueHandles 
        //{
        //    get 
        //    {
        //        return valueHandles;
        //    }
        //}
        /// <summary>
        /// 自动增长的字段值
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        internal string IdentityValueString(KeyWordInfomation info)
        {
            StringBuilder ret = new StringBuilder(500);
            IDBAdapter idb = info.DBInfo.CurrentDbAdapter;
            string prmValue = null;
            foreach (IdentityInfo identInfo in info.IdentityInfos)
            {
                //IdentityInfo identInfo = info.IdentityInfos[i];
                prmValue = idb.GetIdentityParamValue(identInfo.EntityInfo, identInfo.PropertyInfo);
                if (string.IsNullOrEmpty(prmValue))
                {
                    return "";
                }
                ret.Append(prmValue);
                
                ret.Append(',');
                
            }
            return ret.ToString();
        }
        internal override void Tran(KeyWordInfomation info)
        {
            
            StringBuilder condition = new StringBuilder();
            List<DbType> lstParamType = info.Condition.ParamTypes;
            for ( int i=0;i< valueHandles.Length;i++)
            {
                BQLValueItem vItem = valueHandles[i];
                if (lstParamType != null && lstParamType.Count > i) 
                {
                    vItem.ValueDbType = lstParamType[i];
                }
                condition.Append(vItem.DisplayValue(info));
                condition.Append(',');
            }
            condition.Append(IdentityValueString(info));
            if (condition.Length > 0)
            {
                condition.Remove(condition.Length - 1, 1);
            }
            info.Condition.SqlValues.Append(condition.ToString());
            //return " values (" + condition.ToString()+")";
        
        }
    }
}
