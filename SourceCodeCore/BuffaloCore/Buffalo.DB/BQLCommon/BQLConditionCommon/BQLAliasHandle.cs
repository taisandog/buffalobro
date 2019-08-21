using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.BQLCommon.BQLKeyWordCommon;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.DB.QueryConditions;
using Buffalo.Kernel;

namespace Buffalo.DB.BQLCommon.BQLConditionCommon
{
    /// <summary>
    /// 别名
    /// </summary>
    public class BQLAliasHandle : BQLTableHandle
    {
        private BQLQuery _query;
        private string _aliasName;
        private BQLTableHandle _table;

        /// <summary>
        /// 获取别名
        /// </summary>
        internal string GetAliasName() 
        {
           
                return _aliasName;
            
        }
        /// <summary>
        /// 主键
        /// </summary>
        public override List<string> GetPrimaryParam()
        {
            return _table.GetPrimaryParam();
        }
        public BQLAliasHandle(BQLQuery query, string aliasName)
        {
            this._query = query;
            this._aliasName = aliasName;
        }

        public BQLAliasHandle(BQLTableHandle table, string aliasName)
        {
            this._table = table;
            this._aliasName = aliasName;
        }

        internal override List<ParamInfo> GetParamInfoHandle()
        {
            if (!CommonMethods.IsNull(_table))
            {
                return _table.GetParamInfoHandle();
            }
            return new List<ParamInfo>();
        }
        internal override void FillInfo(KeyWordInfomation info)
        {
            BQLValueItem.DoFillInfo(_table, info);

        }
        internal override string DisplayValue(KeyWordInfomation info)
        {
            SelectCondition con = info.Condition as SelectCondition;
            bool isGroup = false;
            if (con != null) 
            {
                isGroup = con.HasGroup;
            }
            if (info.Condition.PrimaryKey.Count <= 0 && !CommonMethods.IsNull(_table) && !isGroup)
            {
                foreach (string pks in _table.GetPrimaryParam())
                {
                    StringBuilder pkStr = new StringBuilder();
                    if (!string.IsNullOrEmpty(_aliasName))
                    {
                        
                        pkStr.Append(info.DBInfo.CurrentDbAdapter.FormatTableName(_aliasName) + ".");
                    }
                    else
                    {
                        pkStr.Append(_table.DisplayValue(info) + ".");
                    }
                    pkStr.Append(pks);
                    info.Condition.PrimaryKey.Add(pkStr.ToString());
                }
            }

            string result = null;
            if (!CommonMethods.IsNull(_table))
            {
                
                result = _table.DisplayValue(info);
                
            }
            else if (_query != null)
            {
                KeyWordInfomation qinfo = info.Clone() as KeyWordInfomation;
                qinfo.Condition = new SelectCondition(info.DBInfo);
                qinfo.ParamList = info.ParamList;
                KeyWordConver objCover = new KeyWordConver();
                string sql = objCover.ToConver(_query, qinfo).GetSql(false);
                foreach (KeyValuePair<string, bool> kvp in qinfo.ContainTables) 
                {
                    info.ContainTables[kvp.Key] = true;
                }
                 result = "(" + sql + ")";
                
            }
            if (!string.IsNullOrEmpty(_aliasName)) 
            {
                result += " " + info.DBInfo.CurrentDbAdapter.FormatTableName(_aliasName);
            }

            return result;
        }
    }
}
