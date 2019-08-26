using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OracleClient;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;
using Buffalo.DB.CommBase;
using Buffalo.DB.EntityInfos;
using Buffalo.DB.QueryConditions;
using Buffalo.DB.DbCommon;
using System.Data.Common;
using Buffalo.DB.PropertyAttributes;
using Buffalo.DB.CommBase.DataAccessBases;
using Buffalo.DB.BQLCommon.BQLKeyWordCommon;
using Buffalo.DB.DataBaseAdapter;
using Oracle.ManagedDataAccess.Client;

namespace Buffalo.Data.Oracle
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
                return true;
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
        public virtual IDataParameter GetDataParameter(string paramName, DbType type, object paramValue, ParameterDirection paramDir)
        {
            OracleParameter newParam = new OracleParameter();
            newParam.ParameterName = paramName;

            newParam.DbType = FormatDbType(type);
            newParam.Value = paramValue;
            newParam.Direction = paramDir;
            return newParam;
        }

        /// <summary>
        /// ��ʽ�����ݿ�����
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected virtual DbType FormatDbType(DbType type)
        {
            switch (type)
            {
                case DbType.UInt16:
                    return DbType.Int16;

                case DbType.UInt64:
                    return DbType.Int64;

                case DbType.UInt32:
                    return DbType.Int32;

                case DbType.DateTime2:
                    return DbType.DateTime;

                case DbType.DateTimeOffset:
                    return DbType.Int32;
                case DbType.Currency:
                case DbType.VarNumeric:
                    return DbType.Decimal;
                case DbType.Guid:
                case DbType.Xml:
                    return DbType.String;
                default:
                    return type;
            }
        }

        /// <summary>
        /// ��ȡtop�Ĳ�ѯ�ַ���
        /// </summary>
        /// <param name="sql">��ѯ�ַ���</param>
        /// <param name="top">topֵ</param>
        /// <returns></returns>
        public string GetTopSelectSql(SelectCondition sql, int top)
        {
            PageContent objPage = new PageContent();
            objPage.IsFillTotalRecords = false;
            objPage.StarIndex = 0;
            objPage.PageSize = top;
            return CutPageSqlCreater.GetCutPageSql(sql.GetSelect(), objPage);
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
        public virtual IDbCommand GetCommand()
        {
            IDbCommand comm = new OracleCommand();
            return comm;
        }
        /// <summary>
        /// ��ȡSQL����
        /// </summary>
        /// <returns></returns>
        public virtual DbConnection GetConnection(DBInfo db)
        {
            DbConnection conn = new OracleConnection();
            return conn;
        }
        /// <summary>
        /// ��ȡSQL������
        /// </summary>
        /// <returns></returns>
        public virtual IDbDataAdapter GetAdapter()
        {
            IDbDataAdapter adapter = new OracleDataAdapter();
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
            return ":" + pname;
        }

        /// <summary>
        /// ��ʽ�������ļ���
        /// </summary>
        /// <param name="pname"></param>
        /// <returns></returns>
        public string FormatParamKeyName(string pname)
        {
            return pname;
        }

        /// <summary>
        /// ����ȫ�ļ����Ĳ�ѯ���
        /// </summary>
        /// <param name="paranName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public string FreeTextLike(string paranName, string value)
        {
            throw new NotImplementedException("Oracle������FreeText����");
        }
        /// <summary>
        /// ����ȫ�ļ����Ĳ�ѯ���
        /// </summary>
        /// <param name="paranName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public string ContainsLike(string paranName, string value)
        {
            return " (contains(" + paranName + "," + value + ")>0)";
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
            return CursorPageCutter.Query(sql, null, objPage, oper);
        }
        /// <summary>
        /// ��ȡ��ǰʱ��
        /// </summary>
        /// <returns></returns>
        public string GetNowDate(DbType dbType)
        {
            switch (dbType)
            {
                
                default:
                    return "sysdate";
            }
        }

         /// <summary>
        /// ��ȡ��ǰʱ��
        /// </summary>
        /// <returns></returns>
        public string GetUTCDate(DbType dbType)
        {
            switch (dbType)
            {
                
                default:
                    return "sys_extract_utc(systimestamp)";
            }
        }
        /// <summary>
        /// ��ȡʱ���
        /// </summary>
        /// <returns></returns>
        public string GetTimeStamp(DbType dbType)
        {
            return "((sysdate -TO_DATE('19700101','yyyymmdd'))*86400 - TO_NUMBER(SUBSTR(TZ_OFFSET(sessiontimezone),1,3))*3600)";
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
            return CursorPageCutter.QueryDataTable(sql, null, objPage, oper, curType);
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
            return CursorPageCutter.Query(sql, lstParam, objPage, oper);
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
            return CursorPageCutter.QueryDataTable(sql, lstParam, objPage, oper, curType);
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
        public string GetIdentitySQL(EntityPropertyInfo pkInfo)
        {
            
            if (pkInfo == null)
            {
                throw new Exception("�Ҳ�����������");
            }
            return "select \"" + GetSequenceName(pkInfo) + "\".currval as curVal from dual";
        }
        /// <summary>
        /// ��ȡ�Զ�����ֵ��SQL
        /// </summary>
        /// <returns></returns>
        public string GetIdentityValueSQL(EntityPropertyInfo pkInfo)
        {
            
            if (pkInfo == null)
            {
                throw new Exception("�Ҳ�����������");
            }
            return "select \"" + GetSequenceName(pkInfo) + "\".nextval as curVal from dual";
        }
        /// <summary>
        /// ��ȡ������
        /// </summary>
        /// <param name="tableName">����</param>
        /// <param name="entityInfo">�ֶ���</param>
        /// <returns></returns>
        public string GetSequenceName(EntityPropertyInfo info)
        {
            return SequenceManager.GetSequenceName(info);
        }

        /// <summary>
        ///  ��ȡĬ��������
        /// </summary>
        /// <param name="tableName">����</param>
        /// <param name="paramName">�ֶ���</param>
        /// <returns></returns>
        public string GetDefaultSequenceName(string tableName, string paramName)
        {
            return SequenceManager.GetDefaultName(tableName, paramName);
        }
        /// <summary>
        /// ��ʼ��������
        /// </summary>
        /// <param name="seqName"></param>
        public string GetSequenceInit(string seqName, EntityParam prm, DataBaseOperate oper)
        {
            return SequenceManager.GetInitSequence(seqName, oper);
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
            return "to_timestamp('" + dt.ToString("yyyy-MM-dd HH:mm:ss.ms") + "','yyyy-mm-dd hh24:mi:ssxff')";
        }

        /// <summary>
        /// ����ʱ���Զ��������ֶ���
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public string GetIdentityParamName(EntityPropertyInfo info)
        {
            return "\"" + info.ParamName + "\"";
        }
        /// <summary>
        /// ����ʱ���Զ��������ֶ�ֵ
        /// </summary>
        /// <param name="entityInfo">ʵ����Ϣ</param>
        /// <param name="info">������Ϣ</param>
        /// <returns></returns>
        public string GetIdentityParamValue(EntityInfoHandle entityInfo, EntityPropertyInfo info)
        {
            return "\"" + GetSequenceName(info) + "\".nextval";
        }

        /// <summary>
        /// ����Reader�����ݰ���ֵ����ʵ��
        /// </summary>
        /// <param name="reader">Reader</param>
        /// <param name="index">��ǰReader������</param>
        /// <param name="arg">Ŀ�����</param>
        /// <param name="info">Ŀ�����Եľ��</param>
        public void SetObjectValueFromReader(IDataReader reader, int index, object arg, EntityPropertyInfo info,bool needChangeType)
        {
            DB.DataBaseAdapter.SqlServer2KAdapter.DBAdapter.ValueFromReader(reader, index, arg, info, needChangeType);
        }



        /// <summary>
        /// ��ȡ���ݿ���������ֶε���Ϣ
        /// </summary>
        /// <returns></returns>
        public virtual string DBIdentity(string tableName, string paramName)
        {

            return "";
        }

        /// <summary>
        /// ��DBType����ת�ɶ�Ӧ��SQLType
        /// </summary>
        /// <param name="dbType"></param>
        /// <returns></returns>
        public virtual string DBTypeToSQL(DbType dbType, long length, bool canNull) 
        {
            switch (dbType)
            {

                case DbType.AnsiString:
                    if (length < 2000)
                    {
                        return "VARCHAR2(" + length + ")";
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
                    if (length < 2000)
                    {
                        return "RAW(" + length + ")";
                    }
                    else
                    {
                        return "BLOB";
                    }
                case DbType.Boolean:
                    return "Number(1,0)";
                case DbType.Byte:
                    return "Number(3,0)";
                case DbType.Date:
                    return "DATE";
                case DbType.DateTimeOffset:
                    return "TIMESTAMP WITH TIME ZONE";
                case DbType.DateTime:
                case DbType.DateTime2:
                case DbType.Time:
                    return "TIMESTAMP";
                case DbType.Decimal:
                case DbType.Currency:
                    return "Number(30,30)";
                case DbType.Double:
                    return "BINARY_DOUBLE";
                case DbType.VarNumeric:
                    return "Number(30,30)";
                case DbType.Single:
                    return "BINARY_FLOAT";
                case DbType.Int64:
                case DbType.UInt64:
                case DbType.UInt32:
                    return "Number(*,0)";

                case DbType.Int16:
                case DbType.UInt16:
                    return "Number(6,0)";
                case DbType.Int32:
                    return "INTEGER";
                
                    
                case DbType.SByte:
                    return "Number(4,0)";
                case DbType.Guid:
                    return "VARCHAR2(64)";
                case DbType.String:
                    if (length < 8000)
                    {
                        return "NVARCHAR2(" + length + ")";
                    }
                    else
                    {
                        return "NCLOB";
                    }
                case DbType.StringFixedLength:
                    if (length < 8000)
                    {
                        return "NChar("+length+")";
                    }
                    else
                    {
                        return "NCLOB";
                    }
                default:
                    return "BLOB";
            }

           
        
        }
        public int ToRealDbType(DbType dbType, long length)
        {
            switch (dbType)
            {
                case DbType.AnsiString:
                    if (length < 2000)
                    {
                        return (int)OracleDbType.Varchar2;
                    }
                    else
                    {
                        return (int)OracleDbType.Long;
                    }
                case DbType.AnsiStringFixedLength:
                case DbType.Guid:
                    if (length < 8000)
                    {
                        return (int)OracleDbType.Char;
                    }
                    else
                    {
                        return (int)OracleDbType.Varchar2;
                    }
                case DbType.Binary:
                    if (length < 2000)
                    {
                        return (int)OracleDbType.Raw;
                    }
                    else
                    {
                        return (int)OracleDbType.Blob;
                    }
                case DbType.Boolean:
                    return (int)OracleDbType.Byte;
                case DbType.Byte:
                    return (int)OracleDbType.Byte;
                case DbType.Date:
                    return (int)OracleDbType.Date;
                case DbType.DateTime:
                    return (int)OracleDbType.TimeStamp;
                case DbType.DateTime2:
                case DbType.DateTimeOffset:
                case DbType.Time:
                    return (int)OracleDbType.IntervalDS;
                case DbType.Decimal:
                case DbType.Double:
                case DbType.UInt64:
                case DbType.VarNumeric:
                case DbType.Int64:
                case DbType.Currency:
                    return (int)OracleDbType.Decimal;
                
                case DbType.Int16:
                    return (int)OracleDbType.Int16;
                case DbType.UInt16:
                    return (int)OracleDbType.Int32;
                case DbType.Int32:
                    return (int)OracleDbType.Int32;
                case DbType.UInt32:
                    return (int)OracleDbType.Int64;
                case DbType.SByte:
                    return (int)OracleDbType.Int32;
                case DbType.Single:
                    return (int)OracleDbType.Double;
                case DbType.String:
                    if (length < 8000)
                    {
                        return (int)OracleDbType.Varchar2;
                    }
                    else
                    {
                        return (int)OracleDbType.Long;
                    }
                case DbType.StringFixedLength:
                    if (length < 8000)
                    {
                        return (int)OracleDbType.NChar;
                    }
                    else
                    {
                        return (int)OracleDbType.Long;
                    }
                

                default:
                    return (int)OracleDbType.Blob;
            }
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
            string description = pInfo == null ? table.Description : pInfo.Description;

            string descriptionValue = DataAccessCommon.FormatValue(description, DbType.AnsiString, info);
            if (pInfo == null)
            {

                return "comment on table " + FormatTableName(table.TableName) + " is " + descriptionValue;
            }
            return "comment on column " + FormatTableName(table.TableName) + "." + FormatParam(pInfo.ParamName) + " is " + descriptionValue;
        }
        public bool OnConnectionClosed(DbConnection conn, DBInfo db)
        {
            //conn.Dispose();
            return true;
        }
        /// <summary>
        /// ���������Ľ���
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public string CreateTableSQLEnd(DBInfo info)
        {
            return null;
        }
    }
}
