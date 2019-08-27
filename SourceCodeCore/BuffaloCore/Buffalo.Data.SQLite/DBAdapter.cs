using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;
using Buffalo.DB.CommBase;
using Buffalo.DB.EntityInfos;
using Buffalo.DB.QueryConditions;
using Buffalo.DB.DbCommon;
using System.Data.Common;
using Buffalo.DB.PropertyAttributes;
using System.Data.SQLite;
using Buffalo.DB.BQLCommon.BQLKeyWordCommon;
using Buffalo.DB.DataBaseAdapter;
namespace Buffalo.Data.SQLite
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
            sb.Append("delete from ");
            sb.Append(FormatTableName(tableName));
            return sb.ToString();
        }
        public bool IdentityIsType
        {
            get { return false; }
        }
        /// <summary>
        /// ��ȡ��ǰʱ��
        /// </summary>
        /// <returns></returns>
        public string GetNowDate(DbType dbType)
        {
            return "datetime('now', 'localtime')";
            //return "CURRENT_TIMESTAMP";
        }
        /// <summary>
        /// ��ȡ��������ʱ��
        /// </summary>
        /// <returns></returns>
        public string GetUTCDate(DbType dbType)
        {
            return "CURRENT_TIMESTAMP";
        }

        /// <summary>
        /// ��ȡunixʱ���
        /// </summary>
        /// <returns></returns>
        public string GetTimeStamp(DbType dbType)
        {
            return "strftime('%s', 'now')";
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
            IDataParameter newParam = new SQLiteParameter();
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
        public string GetTopSelectSql(SelectCondition sql, int top)
        {
            StringBuilder sbSql = new StringBuilder(sql.GetSelect());
            //sbSql.Append(sql);
            sbSql.Append(" LIMIT 0, " + top);
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
            IDbCommand comm = new SQLiteCommand();
            return comm;
        }
        /// <summary>
        /// ��ȡSQL����
        /// </summary>
        /// <returns></returns>
        public DbConnection GetConnection(DBInfo db)
        {

            DbConnection conn = new SQLiteConnection();
            return conn;
        }
        /// <summary>
        /// ��ȡSQL������
        /// </summary>
        /// <returns></returns>
        public IDbDataAdapter GetAdapter()
        {
            IDbDataAdapter adapter =new SQLiteDataAdapter();
            return adapter;
        }

        /// <summary>
        /// ��ʽ���ֶ���
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public string FormatParam(string paramName)
        {

            return "[" + paramName + "]";
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
            return "@" + pname;
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
            throw new NotImplementedException("SQLite������ȫ�ļ�������");
        }
        /// <summary>
        /// ����ȫ�ļ����Ĳ�ѯ���
        /// </summary>
        /// <param name="paranName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public string FreeTextLike(string paranName, string value)
        {
            throw new NotImplementedException("SQLite������ȫ�ļ�������");
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
            return CursorPageCutter.Query(sql, null, objPage, oper,null);
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
            return CursorPageCutter.QueryDataTable(sql, null, objPage, oper, curType, null);
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
        public IDataReader Query(string sql, ParamList lstParam, PageContent objPage,
            DataBaseOperate oper)
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
        public virtual string CreatePageSql(ParamList list, DataBaseOperate oper,
            SelectCondition objCondition, PageContent objPage,bool useCache)
        {
            return CutPageSqlCreater.CreatePageSql(list, oper, objCondition, objPage,useCache);
        }
        /// <summary>
        /// ��ȡ��ʼ���Ʒֺ�����SQL���
        /// </summary>
        /// <returns></returns>
        public string GetInitPointFunSql()
        {
            return null;
        }

        /// <summary>
        /// ��ȡ�жϼƷֺ����Ƿ���ڵ�SQL
        /// </summary>
        /// <returns></returns>
        public string GetPiontFunSqlExistsSql()
        {
            return null;
        }

        /// <summary>
        /// ��ȡ�Ʒֺ���������
        /// </summary>
        public string PointFunName
        {
            get
            {
                return null;
            }
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
            if (ret.Length > 1)
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
            return "select LAST_INSERT_ROWID()";
        }
        /// <summary>
        /// ��ȡ�Զ�����ֵ��SQL
        /// </summary>
        /// <returns></returns>
        public string GetIdentityValueSQL(EntityPropertyInfo info)
        {
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
            return "'" + dt.ToString("s") + "'";
        }

        // <summary>
        /// ����ʱ���Զ��������ֶ���
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public string GetIdentityParamName(EntityPropertyInfo info)
        {
            return null;
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
        /// ��ȡ����ע�͵�SQL
        /// </summary>
        /// <param name="table">��</param>
        /// <param name="paramName">�ֶ�(���Ϊ�����������ע��)</param>
        /// <param name="description">ע��</param>
        /// <returns></returns>
        public string GetAddDescriptionSQL(KeyWordTableParamItem table, EntityParam pInfo, DBInfo info)
        {
            return "";
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

        #region IDBAdapter ��Ա


        public string GetIdentityParamValue(EntityInfoHandle entityInfo, EntityPropertyInfo info)
        {
            return "";
        }

        public string GetSequenceName(EntityPropertyInfo info)
        {
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
        /// ��ʼ��������
        /// </summary>
        /// <param name="seqName"></param>
        public string GetSequenceInit(string seqName, EntityParam prm, DataBaseOperate oper)
        {
            return null;
        }

        #endregion

        #region IDBAdapter ��Ա


        /// <summary>
        /// ��ȡ���ݿ���������ֶε���Ϣ
        /// </summary>
        /// <returns></returns>
        public virtual string DBIdentity(string tableName, string paramName)
        {
            return " AUTOINCREMENT";
        }

        public string DBTypeToSQL(DbType dbType, long length,bool canNull)
        {
            //int type = ToRealDbType(dbType, length);            
            //SqliteType stype = (SqlDbType)type;            
            switch (dbType)
            {
                case DbType.AnsiString:
                case DbType.AnsiStringFixedLength:
                    if (length > 8000)
                    {
                        return "TEXT";
                    }
                    else
                    {
                        return "VARCHAR" + "(" + length + ")";
                    }
                case DbType.Binary:
                    return "BLOB";
                case DbType.Boolean:
                    return "BOOLEAN";
                case DbType.Byte:
                case DbType.Int16:
                case DbType.Int32:
                case DbType.UInt16:
                case DbType.SByte:
                    return "INTEGER";
                case DbType.Decimal:
                case DbType.Currency:
                case DbType.Double:
                case DbType.Int64:
                case DbType.UInt32:
                case DbType.UInt64:
                case DbType.VarNumeric:
                    return "REAL";
                case DbType.Date:
                    return "DATE";
                case DbType.DateTime2:
                case DbType.DateTimeOffset:
                case DbType.DateTime:
                    return "TIMESTAMP";
                case DbType.Guid:
                    return "VARCHAR(64)";
                case DbType.Single:
                    return "FLOAT";
                case DbType.String:
                case DbType.StringFixedLength:
                    return "NVARCHAR(" + length + ")";
                case DbType.Time:
                    return "TIME";
                default:
                    return "NUMERIC";
            }

        }

        

        public int ToRealDbType(DbType dbType, long length)
        {
            return (int)dbType;
        }
        public bool OnConnectionClosed(DbConnection conn, DBInfo db)
        {
            //DBInfoLocker.FreeConnection(db);
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
            return false;
        }
    }
}
