using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using IBM.Data.DB2.Core;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;
using Buffalo.DB.CommBase;
using Buffalo.DB.EntityInfos;
using Buffalo.DB.QueryConditions;
using Buffalo.DB.DbCommon;
using System.Data.Common;
using Buffalo.DB.PropertyAttributes;
using Buffalo.DB.BQLCommon.BQLKeyWordCommon;
using Buffalo.DB.CommBase.DataAccessBases;
using Buffalo.DB.DataBaseAdapter;

namespace Buffalo.Data.DB2
{
    public class DBAdapter : IDBAdapter
    {
        /// <summary>
        /// ȫ������ʱ�������ֶ��Ƿ���ʾ���ʽ
        /// </summary>
        public virtual bool IsShowExpression
        {
            get
            {
                return false;
            }
        }
        /// <summary>
        /// ��ձ�
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public string GetTruncateTable(string tableName)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("truncate table ");
            sb.Append(FormatTableName(tableName));
            sb.Append(" immediate");
            
            return sb.ToString();
        }
        public bool IdentityIsType
        {
            get { return false; }
        }
        /// <summary>
        /// �Ƿ��¼�������ֶ����ֶ�����
        /// </summary>
        public bool IsSaveIdentityParam
        {
            get
            {
                return false;
            }
        }
        /// <summary>
        /// ��ȡ���ֶ����SQL
        /// </summary>
        /// <param name="table">��</param>
        /// <param name="pInfo">�ֶΣ����Ϊ�������ñ�ע�ͣ�</param>
        /// <returns></returns>
        public virtual string GetColumnDescriptionSQL(EntityParam pInfo, DBInfo info)
        {
            return null;
        }
        /// <summary>
        /// �ؽ���������
        /// </summary>
        /// <param name="lstPrm"></param>
        /// <returns></returns>
        public virtual ParamList RebuildParamList(ref string sql, ParamList lstPrm)
        {
            return lstPrm;
        }
        /// <summary>
        /// ��ȡ��ǰʱ��
        /// </summary>
        /// <returns></returns>
        public string GetNowDate(DbType dbType)
        {
            switch (dbType)
            {
                case DbType.Time:
                    return "current time";
                case DbType.Date:
                    return "current date";
                default:
                    return "current timestamp";
            }
           
        }
        /// <summary>
        /// ��ȡ���ݿ⵱ǰ��������ʱ��
        /// </summary>
        /// <returns></returns>
        public string GetUTCDate(DbType dbType) 
        {
            switch (dbType)
            {
                case DbType.Time:
                    return "(current time - current timezone)";
                case DbType.Date:
                    return "(current date - current timezone)";
                default:
                    return "(current timestamp - current timezone)";
            }
        }
        /// <summary>
        /// ��ȡʱ���
        /// </summary>
        /// <param name="dbType"></param>
        /// <returns></returns>
        public string GetTimeStamp(DbType dbType) 
        {
            return "(cast(days(current timestamp)-days('1970-01-01') as integer) * 86400 + (midnight_seconds(current timestamp-current timezone)))";
        }
        ///// <summary>
        ///// ��ȡ�����б�
        ///// </summary>
        //public ParamList BQLSelectParamList
        //{
        //    get
        //    {
        //        return new ParamList();
        //    }
        //}
        /// <summary>
        /// ��ȡ������
        /// </summary>
        /// <param name="paramName">������</param>
        /// <param name="type">�������ݿ�����</param>
        /// <param name="paramValue">����ֵ</param>
        /// <param name="paramDir">������������</param>
        /// <returns></returns>
        public IDataParameter GetDataParameter(string paramName, DbType type, object paramValue, ParameterDirection paramDir) 
        {
            IDataParameter newParam = new DB2Parameter();
            newParam.ParameterName = paramName;
            newParam.DbType = FormatDbType(type);
            newParam.Value = FormatValue(paramValue);
            newParam.Direction = paramDir;
            return newParam;
        }

        /// <summary>
        /// ��ʽ��ֵ
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private object FormatValue(object value) 
        {
            if (value == null) 
            {
                return null;
            }

            if (value is bool) 
            {
                return (bool)value ? (short)1 : (short)0;
            }
            return value;
        }

        /// <summary>
        /// ��ʽ�����ݿ�����
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private DbType FormatDbType(DbType type) 
        {
            if (type == DbType.Boolean) 
            {
                return DbType.Int16;
            }
            return type;
        }

        /// <summary>
        /// ��ȡtop�Ĳ�ѯ�ַ���
        /// </summary>
        /// <param name="sql">��ѯ�ַ���</param>
        /// <param name="top">topֵ</param>
        /// <returns></returns>
        public string GetTopSelectSql(SelectCondition sql, int top)
        {
            StringBuilder sbSql = new StringBuilder(500);
            sbSql.Append(sql.GetSelect());
            sbSql.Append("  fetch first ");
            sbSql.Append(top.ToString());
            sbSql.Append(" rows only");
            return sbSql.ToString();
        }

        /// <summary>
        /// ����������ת���ɵ�ǰ���ݿ�֧�ֵ�����
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public DbType ToCurrentDbType(DbType type)
        {
            return type;
        }
        /// <summary>
        /// ��ȡSQL������
        /// </summary>
        /// <returns></returns>
        public IDbCommand GetCommand() 
        {
            IDbCommand comm = new DB2Command();
            return comm;
        }
        /// <summary>
        /// ��ȡSQL����
        /// </summary>
        /// <returns></returns>
        public DbConnection GetConnection(DBInfo db)
        {
            DbConnection conn = new DB2Connection();
            return conn;
        }
        /// <summary>
        /// ��ȡSQL������
        /// </summary>
        /// <returns></returns>
        public IDbDataAdapter GetAdapter()
        {
            IDbDataAdapter adapter = new DB2DataAdapter();
            return adapter;
        }

        /// <summary>
        /// ��ʽ���ֶ���
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public string FormatParam(string paramName) 
        {
            return "\"" + paramName + "\"";
        }

        /// <summary>
        /// ��ʽ�������
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public string FormatTableName(string tableName)
        {
            return FormatParam(tableName);
        }

        /// <summary>
        /// ��ʽ��������
        /// </summary>
        /// <param name="pname"></param>
        /// <returns></returns>
        public string FormatValueName(string pname) 
        {
            return "@"+pname;
        }

        /// <summary>
        /// ��ʽ�������ļ���
        /// </summary>
        /// <param name="pname"></param>
        /// <returns></returns>
        public string FormatParamKeyName(string pname)
        {
            return "@" + pname;
        }
        
        /// <summary>
        /// ����ȫ�ļ����Ĳ�ѯ���
        /// </summary>
        /// <param name="paranName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public string ContainsLike(string paranName, string value) 
        {
            return " (CONTAINS(" + paranName + "," + value + ")>0)";
        }
        /// <summary>
        /// ����ȫ�ļ����Ĳ�ѯ���
        /// </summary>
        /// <param name="paranName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public string FreeTextLike(string paranName, string value)
        {
            throw new NotImplementedException("DB2������FreeText����");
        }
        /// <summary>
        /// �α��ҳ
        /// </summary>
        /// <typeparam name="T">ʵ������</typeparam>
        /// <param name="sql">sql���</param>
        /// <param name="objPage">��ҳʵ��</param>
        /// <param name="oper">���ݿ�����</param>
        /// <returns></returns>
        public IDataReader Query(string sql, PageContent objPage, DataBaseOperate oper)
        {
            return CursorPageCutter.Query(sql,null, objPage, oper,null);
        }

        /// <summary>
        /// ��ѯ���ҷ���DataSet(�α��ҳ)
        /// </summary>
        /// <param name="sql">Ҫ��ѯ��SQL���</param>
        /// <param name="objPage">��ҳ����</param>
        /// <param name="oper">���ݿ����</param>
        /// <param name="curType">ӳ���ʵ������(����û����ݿ��ԭ���������Ϊnull)</param>
        /// <returns></returns>
        public DataTable QueryDataTable(string sql, PageContent objPage, DataBaseOperate oper, Type curType)
        {
            return CursorPageCutter.QueryDataTable(sql,null, objPage, oper, curType,null);
        }
        /// <summary>
        /// �α��ҳ
        /// </summary>
        /// <typeparam name="T">ʵ������</typeparam>
        /// <param name="lstParam">��������</param>
        /// <param name="sql">sql���</param>
        /// <param name="objPage">��ҳʵ��</param>
        /// <param name="oper">���ݿ�����</param>
        /// <returns></returns>
        public IDataReader Query(string sql, ParamList lstParam, PageContent objPage, DataBaseOperate oper)
        {
            return CursorPageCutter.Query(sql, lstParam, objPage, oper,null);
        }

        /// <summary>
        /// ��ѯ���ҷ���DataSet(�α��ҳ)
        /// </summary>
        /// <param name="sql">Ҫ��ѯ��SQL���</param>
        /// <param name="lstParam">��������</param>
        /// <param name="objPage">��ҳ����</param>
        /// <param name="oper">���ݿ����</param>
        /// <param name="curType">ӳ���ʵ������(����û����ݿ��ԭ���������Ϊnull)</param>
        /// <returns></returns>
        public DataTable QueryDataTable(string sql, ParamList lstParam, PageContent objPage, DataBaseOperate oper, Type curType)
        {
            return CursorPageCutter.QueryDataTable(sql, lstParam, objPage, oper, curType, null);
        }
        /// <summary>
        /// ���ɷ�ҳSQL���
        /// </summary>
        /// <param name="list">�����б�</param>
        /// <param name="oper">���Ӷ���</param>
        /// <param name="objCondition">��������</param>
        /// <param name="objPage">��ҳ��¼��</param>
        /// <returns></returns>
        public string CreatePageSql(ParamList list, DataBaseOperate oper, SelectCondition objCondition,
            PageContent objPage,bool useCache) 
        {
            return CutPageSqlCreater.CreatePageSql(list, oper, objCondition, objPage,useCache);
        }
        

        /// <summary>
        /// ��ȡ�ַ���ƴ��SQl���
        /// </summary>
        /// <param name="str">�ַ�������</param>
        /// <returns></returns>
        public string ConcatString(params string[] strs) 
        {
            StringBuilder sbRet = new StringBuilder();
            foreach (string curStr in strs) 
            {
                sbRet.Append(curStr + "||");
            }
            string ret = sbRet.ToString();
            if (ret.Length > 2) 
            {
                ret = ret.Substring(0, ret.Length - 2);
            }
            return ret;
        }
        
        /// <summary>
        /// ��ȡ�Զ�������SQL
        /// </summary>
        /// <returns></returns>
        public string GetIdentitySQL(EntityPropertyInfo info) 
        {
            //if (info == null)
            //{
            //    throw new Exception("�Ҳ�����������");
            //}
            //return "select LASTASSIGNEDVAL from SYSIBM.SYSSEQUENCES where SEQNAME='" + GetSequenceName(info.BelongInfo.TableName, info.ParamName) + "'";
            return "VALUES IDENTITY_VAL_LOCAL()";
        }

        /// <summary>
        /// ��ȡ�Զ�����ֵ��SQL
        /// </summary>
        /// <returns></returns>
        public string GetIdentityValueSQL(EntityPropertyInfo info)
        {
            //if (info == null)
            //{
            //    throw new Exception("�Ҳ�����������");
            //}
            //return "PREVIOUS VALUE FOR \"" + GetSequenceName(info.BelongInfo.TableName, info.ParamName) + "\"";
            return null;
        }

        /// <summary>
        /// �ѱ���ת���SQL����е�ʱ����ʽ
        /// </summary>
        /// <returns></returns>
        public string GetDateTimeString(object value)
        {
            DateTime dt;
            if (!(value is DateTime))
            {
                dt = Convert.ToDateTime(value);
            }
            else
            {
                dt = (DateTime)value;
            }
            return "TIMESTAMP('" + dt.ToString("yyyy-MM-dd HH:mm:ss.ms") + "')";
        }

        /// <summary>
        /// ����ʱ���Զ��������ֶ���
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public string GetIdentityParamName(EntityPropertyInfo info)
        {
            //return "\"" + info.ParamName + "\"";
            return null;
        }
        /// <summary>
        /// ����ʱ���Զ��������ֶ�ֵ
        /// </summary>
        /// <param name="entityInfo">ʵ����Ϣ</param>
        /// <param name="info">������Ϣ</param>
        /// <returns></returns>
        public string GetIdentityParamValue(EntityInfoHandle entityInfo, EntityPropertyInfo info)
        {
            //return "NEXT VALUE FOR \"" + GetSequenceName(entityInfo.TableName, info.ParamName) + "\"";
            return null;
        }

        /// <summary>
        /// ����Reader�����ݰ���ֵ����ʵ��
        /// </summary>
        /// <param name="reader">Reader</param>
        /// <param name="index">��ǰReader������</param>
        /// <param name="arg">Ŀ�����</param>
        /// <param name="info">Ŀ�����Եľ��</param>
        public void SetObjectValueFromReader(IDataReader reader, int index, object arg, EntityPropertyInfo info, bool needChangeType)
        {
            Buffalo.DB.DataBaseAdapter.SqlServer2KAdapter.DBAdapter.ValueFromReader(reader, index, arg, info,needChangeType);
        }

        public string GetSequenceName(EntityPropertyInfo info)
        {
            //return SequenceManager.GetSequenceName(tableName, paramName);
            return null;
        }

        public string GetSequenceInit(string seqName,EntityParam ep, DataBaseOperate oper) 
        {
            //return SequenceManager.GetInitSequence(seqName, prm, oper);
            return null;
        }

        /// <summary>
        ///  ��ȡĬ��������
        /// </summary>
        /// <param name="tableName">����</param>
        /// <param name="paramName">�ֶ���</param>
        /// <returns></returns>
        public string GetDefaultSequenceName(string tableName, string paramName)
        {
            return null;
        }

        /// <summary>
        /// ��ȡ���ݿ���������ֶε���Ϣ
        /// </summary>
        /// <returns></returns>
        public virtual string DBIdentity(string tableName, string paramName)
        {
            return "generated always as identity (start with 1 increment by 1)";
        }

        public string DBTypeToSQL(DbType dbType, long length,bool canNull)
        {
            switch (dbType)
            {

                case DbType.AnsiString:
                    if (length < 8000)
                    {
                        return "VARCHAR(" + length + ")";
                    }
                    else
                    {
                        return "CLOB";
                    }
                case DbType.AnsiStringFixedLength:
                    if (length < 8000)
                    {
                        return "Char(" + length + ")";
                    }
                    else
                    {
                        return "CLOB";
                    }
                case DbType.Binary:
                        return "BLOB";
                case DbType.Boolean:
                case DbType.Byte:
                    return "SMALLINT";
                case DbType.Date:
                    return "DATE";
                case DbType.DateTimeOffset:
                case DbType.DateTime:
                case DbType.DateTime2:
                case DbType.Time:
                    return "TIMESTAMP";
                case DbType.Decimal:
                case DbType.Currency:
                    return "DOUBLE";
                case DbType.Double:
                case DbType.VarNumeric:
                    return "FLOAT";
                case DbType.Single:
                    return "REAL";
                case DbType.Int64:
                case DbType.UInt64:
                    return "BIGINT";

                case DbType.Int16:
                case DbType.UInt16:
                    return "SMALLINT";
                case DbType.Int32:
                case DbType.UInt32:
                    return "INTEGER";
                case DbType.SByte:
                    return "SMALLINT";
                case DbType.Guid:
                    return "VARCHAR(64)";
                case DbType.String:
                    if (length < 8000)
                    {
                        return "NVARCHAR2(" + length + ")";
                    }
                    else
                    {
                        return "DBCLOB";
                    }
                case DbType.StringFixedLength:
                    if (length < 8000)
                    {
                        return "NChar("+length+")";
                    }
                    else
                    {
                        return "DBCLOB";
                    }
                default:
                    return "BLOB";
            
            }
        }

        public int ToRealDbType(DbType dbType, long length)
        {
            DB2Parameter prm = new DB2Parameter();
            prm.DbType = dbType;
            prm.ParameterName = "name";
            return (int)prm.DB2Type;
        }
        /// <summary>
        /// ��ȡ����ע�͵�SQL
        /// </summary>
        /// <param name="table">��</param>
        /// <param name="paramName">�ֶ�(���Ϊ�����������ע��)</param>
        /// <param name="description">ע��</param>
        /// <returns></returns>
        public string GetAddDescriptionSQL(KeyWordTableParamItem table, EntityParam pInfo, DBInfo info)
        {
            string tableValue = DataAccessCommon.FormatValue(table.TableName, DbType.AnsiString, info);
            string description = pInfo == null ? table.Description : pInfo.Description;

            string descriptionValue = DataAccessCommon.FormatValue(description, DbType.AnsiString, info);
            if (pInfo == null)
            {
                return "COMMENT ON TABLE " + FormatTableName(table.TableName) + " IS " + descriptionValue;
            }
            return "COMMENT ON " + FormatTableName(table.TableName) + "(" + FormatParam(pInfo.ParamName) + " IS '���')";
        }
        #region IDBAdapter ��Ա


        public bool OnConnectionClosed(DbConnection conn, DBInfo db)
        {
            return true;
        }

        #endregion
        /// <summary>
        /// ���������Ľ���
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public string CreateTableSQLEnd(DBInfo info)
        {
            return null;
        }
        public virtual bool KeyWordDEFAULTFront()
        {
            return true;
        }
    }
}
