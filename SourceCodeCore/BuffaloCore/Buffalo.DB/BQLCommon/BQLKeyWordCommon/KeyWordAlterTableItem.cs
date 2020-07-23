using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.BQLCommon.BQLConditionCommon;
using Buffalo.DB.DbCommon;
using Buffalo.DB.QueryConditions;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;
using Buffalo.DB.PropertyAttributes;
using System.Data;

namespace Buffalo.DB.BQLCommon.BQLKeyWordCommon
{
    /// <summary>
    /// alert table项
    /// </summary>
    public class KeyWordAlterTableItem : BQLQuery
    {
        private string _tableName;

        /// <summary>
        /// 表名
        /// </summary>
        public string TableName
        {
            get { return _tableName; }
        }
        /// <summary>
        /// alert table项
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="previous">上一级</param>
        internal KeyWordAlterTableItem(string tableName, BQLQuery previous)
            : base(previous) 
        {
            this._tableName = tableName;
        }
        
        /// <summary>
        /// 字段
        /// </summary>
        /// <param name="paramName">字段名</param>
        /// <param name="dbType">数据库类型</param>
        /// <param name="allowNull">允许空</param>
        /// <param name="type">类型</param>
        /// <param name="length">长度</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public KeyWordAddParamItem AddParam(string paramName, 
            DbType dbType, bool allowNull, EntityPropertyType type,int length,string defaultValue)
        {
            EntityParam info = new EntityParam("",paramName,"",
                dbType, type,"",length, allowNull, false, defaultValue);
            KeyWordAddParamItem item = new KeyWordAddParamItem(info,_tableName, this);
            return item;
        }
        /// <summary>
        /// 添加字段
        /// </summary>
        /// <param name="info">字段信息</param>
        /// <returns></returns>
        public KeyWordAddParamItem AddParam(EntityParam info)
        {
            KeyWordAddParamItem item = new KeyWordAddParamItem(info,_tableName, this);
            return item;
        }

        /// <summary>
        /// 添加外键
        /// </summary>
        /// <param name="info">外键信息</param>
        /// <returns></returns>
        public KeyWordAddForeignkeyItem AddForeignkey(TableRelationAttribute info) 
        {
            return new KeyWordAddForeignkeyItem(info, this);
        }
        /// <summary>
        /// 添加主键
        /// </summary>
        /// <param name="pkParams">主键集合</param>
        /// <param name="name">名称</param>
        /// <returns></returns>
        public KeyWordAddPrimarykeyItem AddPrimaryKey(IEnumerable<string> pkParams,string name) 
        {
            return new KeyWordAddPrimarykeyItem(pkParams, name, this);
        }

        /// <summary>
        /// 添加约束
        /// </summary>
        /// <param name="name">约束名</param>
        /// <param name="parentTable">主表</param>
        /// <param name="childTable">子表</param>
        /// <param name="parentParam">主表键</param>
        /// <param name="childParam">子表键</param>
        /// <returns></returns>
        public KeyWordAddForeignkeyItem AddConstraint(string name, string parentTable, string childTable,
            string parentParam, string childParam)
        {
            return new KeyWordAddForeignkeyItem(name,parentTable,childTable,parentParam,childParam, this);
        }
        internal override void LoadInfo(KeyWordInfomation info)
        {
            
        }
        internal override void Tran(KeyWordInfomation info)
        {
            if (info.Condition == null)
            {
                info.Condition = new Buffalo.DB.QueryConditions.AlterTableCondition(info.DBInfo);
            }
            if (info.ParamList == null)
            {
                info.ParamList = new Buffalo.DB.DbCommon.ParamList();
            }
            IDBAdapter idba = info.DBInfo.CurrentDbAdapter;
            
            info.Condition.Tables.Append(idba.FormatTableName(_tableName));
            
        }
    }
}
