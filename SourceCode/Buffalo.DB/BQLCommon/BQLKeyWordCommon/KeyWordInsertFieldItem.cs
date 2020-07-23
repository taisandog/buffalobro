using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.BQLCommon.BQLConditionCommon;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.DB.BQLCommon.IdentityInfos;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;
using Buffalo.DB.DbCommon;
using Buffalo.DB.QueryConditions;
using System.Data;
using Buffalo.Kernel;

namespace Buffalo.DB.BQLCommon.BQLKeyWordCommon
{
    public class KeyWordInsertFieldItem : BQLQuery
    {
        private BQLParamHandle[] paramHandles;
        /// <summary>
        /// Insert的字段关键字项
        /// </summary>
        /// <param name="paramHandles">字段集合</param>
        /// <param name="previous">上一个关键字</param>
        internal KeyWordInsertFieldItem(BQLParamHandle[] paramHandles, BQLQuery previous)
            : base(previous) 
        {
            this.paramHandles = paramHandles;
        }
        
        internal override void LoadInfo(KeyWordInfomation info)
        {

        }

        ///// <summary>
        ///// 要插入的字段
        ///// </summary>
        //internal BQLParamHandle[] ParamHandles 
        //{
        //    get 
        //    {
        //        return paramHandles;
        //    }
        //}

        /// <summary>
        /// 值
        /// </summary>
        /// <param name="values">值</param>
        /// <returns></returns>
        public KeyWordInsertValueItem Values(params object[] values)
        {
            BQLValueItem[] vitems = new BQLValueItem[values.Length];
            for (int i = 0; i < values.Length;i++ )
            {

                if (values[i] == null) 
                {
                    vitems[i] = new BQLValueTypeItem(null);
                }
                else if (values[i] is BQLValueItem) 
                {
                    vitems[i] = values[i] as BQLValueItem;
                }
                else if (values[i].GetType().IsEnum)
                {
                    vitems[i] = new BQLValueTypeItem((int)values[i]);
                }
                else
                {
                    vitems[i] = new BQLValueTypeItem(values[i]);
                }
            }
            KeyWordInsertValueItem valuesItem = new KeyWordInsertValueItem(vitems, this);
            return valuesItem;
        }
        /// <summary>
        /// 插入一个查询集合
        /// </summary>
        /// <param name="query">查询</param>
        /// <returns></returns>
        public KeyWordInsertQueryItem ByQuery(BQLQuery query)
        {
            return new KeyWordInsertQueryItem(query, this);
        }
        /// <summary>
        /// 自动增长的字段名
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        internal string IdentityFieldString(KeyWordInfomation info) 
        {
            StringBuilder ret=new StringBuilder(500);
            IDBAdapter idb = info.DBInfo.CurrentDbAdapter;
            string prmName=null;
            foreach(IdentityInfo identInfo in info.IdentityInfos)
            {
                //IdentityInfo identInfo = info.IdentityInfos[i];

                prmName=idb.GetIdentityParamName(identInfo.PropertyInfo);
                if(string.IsNullOrEmpty(prmName))
                {
                    return "";
                }
                ret.Append(idb.FormatTableName(identInfo.EntityInfo.TableName));
                ret.Append(".");
                ret.Append(prmName);
                ret.Append(',');
                
            }
            return ret.ToString();
        }

        internal override void Tran(KeyWordInfomation info)
        {
            
            StringBuilder condition = new StringBuilder();
            List<DbType> lstParamType=info.Condition.ParamTypes;
            foreach (BQLParamHandle vItem in paramHandles)
            {
                //BQLParamHandle vItem = paramHandles[i];
                condition.Append(vItem.DisplayValue(info));
                
                condition.Append(',');

                
                if (lstParamType != null) 
                {
                    lstParamType.Add(vItem.ValueDbType);
                }
            }
            
            condition.Append(IdentityFieldString(info));
            if (condition.Length > 0) 
            {
                condition.Remove(condition.Length - 1, 1);
            }
            info.Condition.SqlParams.Append( condition);
        
        }
    }
}
