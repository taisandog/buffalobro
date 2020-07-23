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
    /// alert table��
    /// </summary>
    public class KeyWordAlterTableItem : BQLQuery
    {
        private string _tableName;

        /// <summary>
        /// ����
        /// </summary>
        public string TableName
        {
            get { return _tableName; }
        }
        /// <summary>
        /// alert table��
        /// </summary>
        /// <param name="tableName">����</param>
        /// <param name="previous">��һ��</param>
        internal KeyWordAlterTableItem(string tableName, BQLQuery previous)
            : base(previous) 
        {
            this._tableName = tableName;
        }
        
        /// <summary>
        /// �ֶ�
        /// </summary>
        /// <param name="paramName">�ֶ���</param>
        /// <param name="dbType">���ݿ�����</param>
        /// <param name="allowNull">�����</param>
        /// <param name="type">����</param>
        /// <param name="length">����</param>
        /// <param name="defaultValue">Ĭ��ֵ</param>
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
        /// ����ֶ�
        /// </summary>
        /// <param name="info">�ֶ���Ϣ</param>
        /// <returns></returns>
        public KeyWordAddParamItem AddParam(EntityParam info)
        {
            KeyWordAddParamItem item = new KeyWordAddParamItem(info,_tableName, this);
            return item;
        }

        /// <summary>
        /// ������
        /// </summary>
        /// <param name="info">�����Ϣ</param>
        /// <returns></returns>
        public KeyWordAddForeignkeyItem AddForeignkey(TableRelationAttribute info) 
        {
            return new KeyWordAddForeignkeyItem(info, this);
        }
        /// <summary>
        /// �������
        /// </summary>
        /// <param name="pkParams">��������</param>
        /// <param name="name">����</param>
        /// <returns></returns>
        public KeyWordAddPrimarykeyItem AddPrimaryKey(IEnumerable<string> pkParams,string name) 
        {
            return new KeyWordAddPrimarykeyItem(pkParams, name, this);
        }

        /// <summary>
        /// ���Լ��
        /// </summary>
        /// <param name="name">Լ����</param>
        /// <param name="parentTable">����</param>
        /// <param name="childTable">�ӱ�</param>
        /// <param name="parentParam">�����</param>
        /// <param name="childParam">�ӱ��</param>
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
