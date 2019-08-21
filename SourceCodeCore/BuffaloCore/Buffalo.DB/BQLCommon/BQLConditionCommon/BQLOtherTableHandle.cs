using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;
using Buffalo.DB.DataBaseAdapter;

namespace Buffalo.DB.BQLCommon.BQLConditionCommon
{
    public class BQLOtherTableHandle:BQLTableHandle
    {
        string tableName;
        public BQLOtherTableHandle(string tableName)
        {
            this.tableName = tableName;
            //this.valueType = BQLValueType.Table;
        }
        ///// <summary>
        ///// ��Ӧ��ʵ������
        ///// </summary>
        //internal Type __EntityType__
        //{
        //    get
        //    {
        //        return entityInfo.EntityType;
        //    }
        //}

        ///// <summary>
        ///// ��Ӧ�ı���
        ///// </summary>
        //internal string __TableName__
        //{
        //    get
        //    {
        //        return entityInfo.TableName;
        //    }
        //}
        internal override void FillInfo(KeyWordInfomation info)
        {
            //if (_db != null && info.DBInfo == null) 
            //{
            //    info.DBInfo = _db;
            //}
        }
        /// <summary>
        /// ��ȡ��Ӧ��ʵ������
        /// </summary>
        /// <returns></returns>
        internal override List<ParamInfo> GetParamInfoHandle()
        {
            List<ParamInfo> lst = new List<ParamInfo>();
            return lst;
        }
       
        internal override string DisplayValue(KeyWordInfomation info)
        {
            IDBAdapter idba = info.DBInfo.CurrentDbAdapter;
            info.ContainTables[tableName] = true;
            return idba.FormatTableName(tableName);
        }

        

        ///// <summary>
        ///// ���������һ������
        ///// </summary>
        ///// <param name="asName">����</param>
        ///// <returns></returns>
        //public BQLAliasHandle AS(string asName)
        //{
        //    BQLAliasHandle item = new BQLAliasHandle(this, asName);
        //    return item;
        //}

        
    }
}
