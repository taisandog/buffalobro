using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.BQLCommon.BQLConditionCommon;
using Buffalo.DB.BQLCommon.IdentityInfos;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;
using Buffalo.DB.DataBaseAdapter;
using System.Data;

namespace Buffalo.DB.BQLCommon.BQLKeyWordCommon
{
    public class KeyWordInsertValueItem : BQLQuery
    {
        private BQLValueItem[] valueHandles;
        /// <summary>
        /// Insert���ֶιؼ�����
        /// </summary>
        /// <param name="valueHandles">ֵ�ļ���</param>
        /// <param name="previous">��һ���ؼ���</param>
        internal KeyWordInsertValueItem(BQLValueItem[] valueHandles, BQLQuery previous)
            : base( previous) 
        {
            this.valueHandles = valueHandles;
        }
        internal override void LoadInfo(KeyWordInfomation info)
        {

        }
        ///// <summary>
        ///// Ҫ�����ֵ
        ///// </summary>
        //internal BQLValueTypeItem[] ValueHandles 
        //{
        //    get 
        //    {
        //        return valueHandles;
        //    }
        //}
        /// <summary>
        /// �Զ��������ֶ�ֵ
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
