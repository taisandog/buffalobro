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
using Buffalo.DB.PropertyAttributes;
using Buffalo.Kernel;
using Buffalo.DB.BQLCommon.BQLBaseFunction;

namespace Buffalo.DB.BQLCommon.BQLKeyWordCommon
{
    /// <summary>
    /// 主键
    /// </summary>
    public class KeyWordAddPrimarykeyItem : BQLQuery
    {
        IEnumerable<string> _pkParams;
        string _name;
        /// <summary>
        /// 外键
        /// </summary>
        /// <param name="pkParams">主键集合</param>
        /// <param name="previous">上一个</param>
        public KeyWordAddPrimarykeyItem(IEnumerable<string> pkParams, string name, BQLQuery previous)
            : base(previous)
        {
            _name=name;
            _pkParams = pkParams;
        }
        internal override void LoadInfo(KeyWordInfomation info)
        {

        }

        internal override void Tran(KeyWordInfomation info)
        {
            AlterTableCondition con = info.Condition as AlterTableCondition;
            if (con == null) 
            {
                return;
            }

            IDBAdapter ida = info.DBInfo.CurrentDbAdapter;

            con.Operator.Append("ADD CONSTRAINT ");
            con.SqlParams.Append(ida.FormatTableName(_name));
            con.SqlParams.Append(" PRIMARY KEY (");
            foreach (string prm in _pkParams) 
            {
                con.SqlParams.Append(ida.FormatParam(prm)+",");
            }
            if (con.SqlParams[con.SqlParams.Length - 1] == ',') 
            {
                con.SqlParams.Remove(con.SqlParams.Length - 1, 1);
            }

            con.SqlParams.Append(")");
        }
    }

    
}
