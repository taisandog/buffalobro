using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Npgsql;
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
using Buffalo.DB.BQLCommon.BQLConditions;

namespace Buffalo.Data.PostgreSQL
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
        public bool IdentityIsType
        {
            get { return false; }
        }
        public virtual bool KeyWordDEFAULTFront()
        {
            return false;
        }
        // <summary>
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
        /// <summary>
        /// ��ȡ��ǰʱ��
        /// </summary>
        /// <returns></returns>
        public string GetNowDate(DbType dbType)
        {
            return "now()";
        }
        
        /// <summary>
        /// ��ȡ��ǰʱ��
        /// </summary>
        /// <returns></returns>
        public string GetUTCDate(DbType dbType)
        {
            return "(now() at time zone 'utc')";
        }
        /// <summary>
        /// ��ȡʱ���
        /// </summary>
        /// <returns></returns>
        public string GetTimeStamp(DbType dbType)
        {
            return "extract(epoch FROM now())";
        }
        
        /// <summary>
        /// ��ȡ���ֶ����SQL
        /// </summary>
        /// <param name="table">��</param>
        /// <param name="pInfo">�ֶΣ����Ϊ�������ñ�ע�ͣ�</param>
        /// <returns></returns>
        public virtual string GetColumnDescriptionSQL( EntityParam pInfo, DBInfo info)
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
        /// �Ƿ��¼�������ֶ����ֶ�����
        /// </summary>
        public bool IsSaveIdentityParam
        {
            get
            {
                return false;
            }
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

            IDataParameter newParam = new NpgsqlParameter();
            newParam.ParameterName = paramName;
            newParam.DbType = type;
            newParam.Value = paramValue;
            newParam.Direction = paramDir;
            return newParam;
        }

        /// <summary>
        /// ��ȡtop�Ĳ�ѯ�ַ���
        /// </summary>
        /// <param name="sql">��ѯ�ַ���</param>
        /// <param name="top">topֵ</param>
        /// <returns></returns>
        public string GetTopSelectSql(SelectCondition sql,int top) 
        {
            StringBuilder sbSql = new StringBuilder(sql.GetSelect());
            //sbSql.Append(sql);
            sbSql.Append(" limit " + top + " offset 0");
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
            IDbCommand comm = new NpgsqlCommand();
            return comm;
        }
        /// <summary>
        /// ��ȡSQL����
        /// </summary>
        /// <returns></returns>
        public DbConnection GetConnection(DBInfo db)
        {
            DbConnection conn = new NpgsqlConnection();
            return conn;
        }
        /// <summary>
        /// ��ȡSQL������
        /// </summary>
        /// <returns></returns>
        public IDbDataAdapter GetAdapter()
        {
            IDbDataAdapter adapter = new NpgsqlDataAdapter();
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
        public string ContainsLike(string paranName, string value) 
        {
            return " to_tsvector(" + paranName + ") @@ to_tsquery("+value+")";
        }
        /// <summary>
        /// ����ȫ�ļ����Ĳ�ѯ���
        /// </summary>
        /// <param name="paranName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public string FreeTextLike(string paranName, string value)
        {
            throw new NotImplementedException("PostgreSQL������FreeText����");
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
            return CursorPageCutter.Query(sql,null, objPage, oper);
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
            return CursorPageCutter.QueryDataTable(sql,null, objPage, oper, curType);
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
        public IDataReader Query(string sql,ParamList lstParam, PageContent objPage, DataBaseOperate oper)
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
        public DataTable QueryDataTable(string sql,ParamList lstParam, PageContent objPage, DataBaseOperate oper, Type curType)
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
        public virtual string CreatePageSql(ParamList list, DataBaseOperate oper,
            SelectCondition objCondition, PageContent objPage,bool useCache) 
        {
            return CutPageSqlCreater.CreatePageSql(list, oper, objCondition, objPage, useCache);
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
        /// �ѱ���ת���SQL����е�ʱ����ʽ
        /// </summary>
        /// <returns></returns>
        public string GetDateTimeString(object value)
        {
            return "'" + value.ToString().Replace("'","") + "'";
        }

        // <summary>
        /// ����ʱ���Զ��������ֶ���
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public string GetIdentityParamName(EntityPropertyInfo info) 
        {
            return "\"" + info.ParamName + "\"";
            //return null;
        }
        /// <summary>
        /// ����ʱ��������ֶ�ֵ
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public string GetInsertPKParamValue(EntityPropertyInfo info)
        {
            return null;
        }
        /// <summary>
        /// ��ȡ�Զ�����ֵ��SQL
        /// </summary>
        /// <returns></returns>
        public string GetIdentityValueSQL(EntityPropertyInfo info)
        {
            if (info == null)
            {
                throw new Exception("�Ҳ�����������");
            }
            return "select nextval('\"" + GetSequenceName(info) + "\"')";
            
        }
        /// <summary>
        /// ��ȡ�Զ�������SQL
        /// </summary>
        /// <returns></returns>
        public string GetIdentitySQL(EntityPropertyInfo info)
        {
            if (info == null)
            {
                throw new Exception("�Ҳ�����������");
            }
            return "select currval('\"" + GetSequenceName(info) + "\"')";
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
            return "COMMENT ON COLUMN " + FormatTableName(table.TableName) + "." + FormatParam(pInfo.ParamName) + " IS " + descriptionValue;
        }
        #region IDBAdapter ��Ա


        public string GetIdentityParamValue(EntityInfoHandle entityInfo, EntityPropertyInfo info)
        {
            return "nextval('\"" + GetSequenceName(info) + "\"')";
        }

        public string GetSequenceName(EntityPropertyInfo info)
        {
            return SequenceManager.GetSequenceName(info);
        }

        /// <summary>
        /// ��ʼ��������
        /// </summary>
        /// <param name="seqName"></param>
        public string GetSequenceInit(string seqName, EntityParam prm, DataBaseOperate oper)
        {
            return SequenceManager.GetInitSequence(seqName,prm, oper);
            
        }
        /// <summary>
        ///  ��ȡĬ��������
        /// </summary>
        /// <param name="tableName">����</param>
        /// <param name="paramName">�ֶ���</param>
        /// <returns></returns>
        public string GetDefaultSequenceName(string tableName, string paramName)
        {
            return SequenceManager.GetDefaultName(tableName,paramName);
        }


        /// <summary>
        /// ��ȡ���ݿ���������ֶε���Ϣ
        /// </summary>
        /// <returns></returns>
        public virtual string DBIdentity(string tableName, string paramName)
        {
            return null;
        }

        public string DBTypeToSQL(DbType dbType, long length,bool canNull)
        {
            switch (dbType)
            {
                case DbType.Boolean:
                    return "bool";

                case DbType.Byte:
                    return "int2";

                case DbType.SByte:
                    return "int2";

                case DbType.UInt16:
                    return "int4";
                case DbType.Int16:
                    return "int2";

                case DbType.UInt32:
                    return "int8";
                case DbType.Int32:
                    return "int4";

                case DbType.UInt64:
                    return "int8";
                case DbType.Int64:
                    return "int8";

                case DbType.Single:
                    return "float4";

                case DbType.Double:
                    return "float8";
                case DbType.Currency:
                case DbType.VarNumeric:
                    return "numeric";
                case DbType.Decimal:
                    if (length <= 0)
                    {
                        return "numeric";

                    }
                    return DBInfo.GetNumberLengthType("numeric", length);

                case DbType.Date:
                    return "date";

                case DbType.DateTime:
                    return "timestamp";
                case DbType.DateTimeOffset:
                case DbType.DateTime2:
                    return "timestamptz";
                case DbType.Time:
                    return "time";
                case DbType.AnsiStringFixedLength:
                case DbType.StringFixedLength:
                    if (length > 8000)
                    {
                        return "text";
                    }
                    return "character(" + length + ")";


                case DbType.AnsiString:
                case DbType.String:
                    if (length > 8000)
                    {
                        return "text";
                    }
                    return "varchar(" + length + ")";
                case DbType.Binary:
                    return "bytea";
                default:
                    return "";
            }
        }

        public int ToRealDbType(DbType dbType, long length)
        {

            NpgsqlParameter prm = new NpgsqlParameter();
            prm.DbType = dbType;
            prm.ParameterName = "name";
            return (int)prm.NpgsqlDbType;
        }
        public bool OnConnectionClosed(DbConnection conn, DBInfo db)
        {
            //conn.Dispose();
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

        /// <summary>
        /// like�����ִ�Сд
        /// </summary>
        /// <param name="source"></param>
        /// <param name="lstParam"></param>
        /// <returns></returns>
        public string DoLike(string source, string param, BQLLikeType type, BQLCaseType caseType, DBInfo info)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append(source);
            if (caseType == BQLCaseType.CaseIgnore)
            {
                sbSql.Append(" ilike ");
            }
            else
            {
                sbSql.Append(" like ");
            }
            sbSql.Append(Buffalo.DB.DataBaseAdapter.SqlServer2KAdapter.DBAdapter.GetLikeString(this, type, param));
            return sbSql.ToString();
        }
        

        public string DoOrderBy(string param, SortType sortType, BQLCaseType caseType, DBInfo info)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" ");
            if (caseType== BQLCaseType.CaseIgnore)
            {
                sb.Append("LOWER(");
                sb.Append(param);
                sb.Append(")");
            }
            else
            {
                sb.Append(param);
            }
            if (sortType == SortType.DESC)
            {
                sb.Append(" desc");
            }
            return sb.ToString();
        }
    }
}
