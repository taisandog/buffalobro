using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.BQLCommon.BQLConditionCommon;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.DB.DbCommon;
using Buffalo.Kernel;
using Buffalo.DB.QueryConditions;

namespace Buffalo.DB.BQLCommon.BQLKeyWordCommon
{
    /// <summary>
    /// Select关键字项
    /// </summary>
    public class KeyWordSelectItem:BQLQuery
    {
        private BQLParamHandle[] parameters;

        /// <summary>
        /// Select关键字项
        /// </summary>
        /// <param name="prmsHandle">字段集合</param>
        /// <param name="previous">上一个关键字</param>
        internal KeyWordSelectItem(BQLParamHandle[] parameters,BQLQuery previous)
            : base(previous) 
        {
            this.parameters = parameters;
        }

        ///// <summary>
        ///// 要查询的字段
        ///// </summary>
        //internal BQLParamHandle[] Parameters 
        //{
        //    get 
        //    {
        //        return parameters;
        //    }
        //}
        /// <summary>
        /// From哪些表
        /// </summary>
        /// <param name="tables">表</param>
        /// <returns></returns>
        public KeyWordFromItem From(params BQLTableHandle[] tables)
        {
            KeyWordFromItem fromItem = new KeyWordFromItem(tables,this);
            return fromItem;
        }

        /// <summary>
        /// 加载本次查询要返回的字段信息
        /// </summary>
        /// <param name="info"></param>
        internal override void LoadInfo(KeyWordInfomation info) 
        {
            foreach (BQLParamHandle prm in parameters) 
            {
                BQLValueItem.DoFillInfo(prm, info);
            }

        }

        /// <summary>
        /// select关键字
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        internal override void Tran(KeyWordInfomation info)
        {
            if (info.Condition == null)
            {
                SelectCondition con=new Buffalo.DB.QueryConditions.SelectCondition(info.DBInfo);
                con.HasGroup = info.HasGroup;
                info.Condition = con;
                
            }
            if (info.ParamList == null)
            {
                info.ParamList = new ParamList();
            }
            
            StringBuilder ret = new StringBuilder();

            IEnumerable<BQLParamHandle> coll = parameters;

            if (info.AliasManager != null&& !info.Infos.IsPutPropertyName) 
            {
                coll = info.AliasManager.GetPrimaryAliasParamHandle(parameters);
            }
            foreach (BQLParamHandle prm in coll)
            {
                //BQLParamHandle prm = parameters[i];
                
                    ret.Append(prm.DisplayValue(info));
                

                ret.Append(",");

            }
            if (ret.Length > 0) 
            {
                ret.Remove(ret.Length - 1, 1);
            }
            info.Condition.SqlParams.Append(ret);
            //return "select " + ret;
        }
    }
}
