using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;
using Buffalo.DB.CommBase;
using Buffalo.DB.EntityInfos;
using Buffalo.DB.QueryConditions;
using Buffalo.DB.DbCommon;
using Buffalo.Kernel;
using System.Data.Common;
using Buffalo.DB.PropertyAttributes;
using Buffalo.DB.CommBase.DataAccessBases;
using Buffalo.DB.BQLCommon.BQLKeyWordCommon;
using Buffalo.DB.BQLCommon.BQLConditions;

namespace Buffalo.DB.DataBaseAdapter.SqlServer2KAdapter
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

        /// <summary>
        /// ��ȡ���ݿ���������ֶε���Ϣ
        /// </summary>
        /// <returns></returns>
        public virtual string DBIdentity(string tableName, string paramName)
        {

            return "IDENTITY(1,1)";
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
        public virtual string GetColumnDescriptionSQL( EntityParam pInfo, DBInfo info) 
        {
            return null;
        }
        /// <summary>
        /// ��DBType����ת�ɶ�Ӧ��SQLType
        /// </summary>
        /// <param name="dbType"></param>
        /// <returns></returns>
        public virtual string DBTypeToSQL(DbType dbType,long length,bool canNull) 
        {
            int type = ToRealDbType(dbType,length);
            SqlDbType stype = (SqlDbType)type;
            switch (stype) 
            {
                case SqlDbType.VarChar:
                    return stype.ToString() + "(" + length + ")";
                case SqlDbType.Char:
                    return stype.ToString() + "(" + length + ")";
                case SqlDbType.Binary:
                    return stype.ToString() + "(" + length + ")";
                case SqlDbType.Decimal:
                    if (length <= 0) 
                    {
                        length = 180002;
                    }
                    return DBInfo.GetNumberLengthType(stype.ToString(), length); 
                case SqlDbType.NVarChar:
                    return stype.ToString() + "(" + length + ")";
                case SqlDbType.NChar:
                    return stype.ToString() + "(" + length + ")";
                case SqlDbType.UniqueIdentifier:
                    return "varchar(64)";
                default:
                    return stype.ToString();
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
        /// <summary>
        /// ��DBTypeת�ɱ����ݿ��ʵ������
        /// </summary>
        /// <param name="dbType"></param>
        /// <returns></returns>
        public virtual int ToRealDbType(DbType dbType,long length) 
        {
            switch (dbType) 
            {
                case DbType.AnsiString:
                    if (length < 8000)
                    {
                        return (int)SqlDbType.VarChar;
                    }
                    else 
                    {
                        return (int)SqlDbType.Text;
                    }
                case DbType.AnsiStringFixedLength:
                    if (length < 8000)
                    {
                        return (int)SqlDbType.Char;
                    }
                    else 
                    {
                        return (int)SqlDbType.Text;
                    }
                case DbType.Binary:
                    if (length < 8000)
                    {
                        return (int)SqlDbType.Binary;
                    }
                    else
                    {
                        return (int)SqlDbType.Image;
                    }
                case DbType.Boolean:
                    return (int)SqlDbType.Bit;
                case DbType.Byte:
                    return (int)SqlDbType.TinyInt;
                case DbType.Currency:
                    return (int)SqlDbType.Money;
                case DbType.Date:
                case DbType.DateTime:
                case DbType.DateTime2:
                case DbType.DateTimeOffset:
                case DbType.Time:
                    return (int)SqlDbType.DateTime;
                case DbType.Decimal:
                case DbType.Double:
                    return (int)SqlDbType.Decimal;
                case DbType.Guid:
                    return (int)SqlDbType.UniqueIdentifier;
                case DbType.Int16:
                    return (int)SqlDbType.SmallInt;
                case DbType.UInt16:
                case DbType.Int32:
                    return (int)SqlDbType.Int;
                case DbType.Int64:
                    return (int)SqlDbType.BigInt;
                case DbType.SByte:
                    return (int)SqlDbType.TinyInt;
                case DbType.Single:
                    return (int)SqlDbType.Float;
                case DbType.String:
                    if (length < 4000)
                    {
                        return (int)SqlDbType.NVarChar;
                    }
                    else 
                    {
                        return (int)SqlDbType.NText;
                    }
                case DbType.StringFixedLength:
                    if (length < 4000)
                    {
                        return (int)SqlDbType.NChar;
                    }
                    else
                    {
                        return (int)SqlDbType.NText;
                    }
               
                case DbType.UInt32:
                case DbType.UInt64:
                    return (int)SqlDbType.BigInt;
                case DbType.VarNumeric:
                    return (int)SqlDbType.Real;
                default:
                    return (int)SqlDbType.Structured;

            }
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
        //public virtual ParamList BQLSelectParamList
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
            IDataParameter newParam = new SqlParameter();
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
            StringBuilder sbSql = new StringBuilder(500);
            sbSql.Append("select top ");
            sbSql.Append(top.ToString());
            sbSql.Append(" " + sql.SqlParams.ToString() + " from ");
            sbSql.Append(sql.Tables.ToString());
            if (sql.Condition.Length > 0)
            {
                sbSql.Append(" where " + sql.Condition.ToString());
            }

            if (sql.Orders.Length > 0)
            {
                sbSql.Append(" order by ");
                sbSql.Append(sql.Orders.ToString());
            }
            if (sql.Having.Length > 0)
            {
                sbSql.Append(" having ");
                sbSql.Append(sql.Having.ToString());
            }
            if (sql.LockUpdate.Length > 0)
            {
                sbSql.Append(sql.LockUpdate.ToString());
            }
            return sbSql.ToString();
        }
        /// <summary>
        /// ��ȡ������
        /// </summary>
        /// <param name="tableName">����</param>
        /// <param name="entityInfo">�ֶ���</param>
        /// <returns></returns>
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
        public string GetSequenceInit(string seqName,EntityParam prm, DataBaseOperate oper)
        {
            return null;
        }
        /// <summary>
        /// ����������ת���ɵ�ǰ���ݿ�֧�ֵ�����
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public DbType ToCurrentDbType(DbType type) 
        {
            return CurrentDbType(type);
        }

        internal static DbType CurrentDbType(DbType type) 
        {
            DbType ret = type;
            switch (ret)
            {
                case DbType.Time:
                    ret = DbType.DateTime;
                    break;
                case DbType.UInt16:
                    ret = DbType.Int32;
                    break;
                case DbType.UInt32:
                    ret = DbType.Int64;
                    break;
                case DbType.UInt64:
                    ret = DbType.Int64;
                    break;
                case DbType.Date:
                    ret = DbType.DateTime;
                    break;
                case DbType.SByte:
                    ret = DbType.Byte;
                    break;
                case DbType.VarNumeric:
                    ret = DbType.Currency;
                    break;
                default:
                    break;
            }
            return ret;
        }

        /// <summary>
        /// ��ȡSQL������
        /// </summary>
        /// <returns></returns>
        public IDbCommand GetCommand() 
        {
            IDbCommand comm = new SqlCommand();
            return comm;
        }
        /// <summary>
        /// ��ȡSQL����
        /// </summary>
        /// <returns></returns>
        public virtual DbConnection GetConnection(DBInfo db)
        {
            DbConnection conn = new SqlConnection();
            return conn;
        }
        /// <summary>
        /// ��ȡSQL������
        /// </summary>
        /// <returns></returns>
        public IDbDataAdapter GetAdapter()
        {
            IDbDataAdapter adapter = new SqlDataAdapter();
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
            return "@" + pname;
        }
        /// <summary>
        /// ����ȫ�ļ����Ĳ�ѯ���
        /// </summary>
        /// <param name="paranName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public string FreeTextLike(string param,string value) 
        {
            return " (freetext(" + param + "," + value + "))";
        }
        /// <summary>
        /// ����ȫ�ļ����Ĳ�ѯ���
        /// </summary>
        /// <param name="paranName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public string ContainsLike(string paranName, string value)
        {
            return " (contains(" + paranName + "," + value + "))";
        }
        /// <summary>
        /// ��ȡ��ǰʱ��
        /// </summary>
        /// <returns></returns>
        public string GetNowDate(DbType dbType)
        {
            return "getdate()";
        }

        /// <summary>
        /// ��ȡ��ǰʱ��
        /// </summary>
        /// <returns></returns>
        public string GetUTCDate(DbType dbType)
        {
            return "getutcdate()";
        }
        /// <summary>
        /// ��ȡʱ���
        /// </summary>
        /// <returns></returns>
        public string GetTimeStamp(DbType dbType)
        {
            return "DATEDIFF(s, '1970-01-01 00:00:00', getutcdate())";
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
            return CursorPageCutter.Query(sql, objPage, oper);
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
            return CursorPageCutter.QueryDataTable(sql, objPage, oper, curType);
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
            throw new Exception("SqlServer��֧�ִ��������α��ҳ");
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
            throw new Exception("SqlServer��֧�ִ��������α��ҳ");
        }
        /// <summary>
        /// ���ɷ�ҳSQL���
        /// </summary>
        /// <param name="list">�����б�</param>
        /// <param name="oper">���Ӷ���</param>
        /// <param name="objCondition">��������</param>
        /// <param name="objPage">��ҳ��¼��</param>
        /// <returns></returns>
        public virtual string CreatePageSql(ParamList list, DataBaseOperate oper, SelectCondition objCondition, PageContent objPage,bool useCache) 
        {
            return CutPageSqlCreater.CreatePageSql(list, oper, objCondition, objPage,useCache?objCondition.CacheTables:null);
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
                sbRet.Append(curStr + "+");
            }
            string ret = sbRet.ToString();
            if (ret.Length > 1)
            {
                ret = ret.Substring(0, ret.Length - 1);
            }
            return ret;
        }
        /// <summary>
        /// ��ȡ�Զ�������SQL
        /// </summary>
        /// <returns></returns>
        public string GetIdentitySQL(EntityPropertyInfo info) 
        {
            return "SELECT @@IDENTITY";
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

            return "'" + value.ToString().Replace("'","") + "'";
        }

        internal string DateTimeString(object value) 
        {
            
            DateTime dt=DateTime.MinValue;
            if (value == null)
            {
                dt = DateTime.MinValue;
            }
            else if (value is DateTime)
            {
                dt = (DateTime)value;
            }
            else 
            {
                dt = DateTime.Parse(value.ToString());
            }
            return "'" + dt.ToString("yyyy-MM-dd HH:mm:ss.ms") + "'";
        }

        /// <summary>
        /// ����ʱ��������ֶ���
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
        public string GetIdentityParamValue(EntityInfoHandle entityInfo, EntityPropertyInfo info)
        {
            return null;
        }

        /// <summary>
        /// ����Reader�����ݰ���ֵ����ʵ��
        /// </summary>
        /// <param name="reader">Reader</param>
        /// <param name="index">��ǰReader������</param>
        /// <param name="arg">Ŀ�����</param>
        /// <param name="info">Ŀ�����Եľ��</param>
        public static void ValueFromReader(IDataReader reader,int index,object arg,EntityPropertyInfo info,bool needChangeType) 
        {
            object val = reader.GetValue(index);
            //if (val is DBNull || val == null) 
            //{
            //    return;
            //}
            if (needChangeType)
            {
                Type resType = info.RealFieldType;//�ֶ�ֵ����
                val = CommonMethods.ChangeType(val, resType);
            }
            info.SetValue(arg, val);
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
            if (pInfo==null)
            {

                return "EXECUTE sp_addextendedproperty N'MS_Description', N" + descriptionValue + ", N'SCHEMA', N'dbo', N'TABLE', N" + tableValue + ", NULL, NULL";
            }
            return "EXECUTE sp_addextendedproperty N'MS_Description', N" + descriptionValue + ", N'SCHEMA', N'dbo', N'TABLE', N" + tableValue + ", N'COLUMN', N'" + pInfo.ParamName + "'";
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
            ValueFromReader(reader, index, arg, info,needChangeType);
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

        public virtual bool KeyWordDEFAULTFront()
        {
            return false;
        }
        /// <summary>
        /// �Ƿ��������
        /// </summary>
        private bool _isCheckcollation=false;
        /// <summary>
        /// ���ִ�Сд������
        /// </summary>
        private string _collationCaseName=null;
        /// <summary>
        /// �����ִ�Сд������
        /// </summary>
        private string _collationIgnoreName=null;
        /// <summary>
        /// ������
        /// </summary>
        private void InitCollationName(DBInfo info)
        {
            if (_isCheckcollation)
            {
                return;
            }
            string collationName = null;
            string sql = "SELECT SERVERPROPERTY(N'Collation')";
            using (IDataReader reader = info.DefaultOperate.Query(sql, null, null))
            {
                while (reader.Read())
                {
                    collationName = reader[0] as string;
                }
            }
            if (string.IsNullOrEmpty(collationName))
            {
                return;
            }
            _collationCaseName = collationName.ToUpper().Replace("_CI", "_CS");
            if (_collationCaseName == collationName)
            {
                _collationCaseName = "";
            }
            _collationIgnoreName = collationName.ToUpper().Replace("_CS","_CI");
            if (_collationIgnoreName == collationName)
            {
                _collationIgnoreName = "";
            }
        }
        /// <summary>
        /// ��ȡ���ִ�СдSQL
        /// </summary>
        /// <param name="iscase">�Ƿ�����</param>
        /// <returns></returns>
        private string GetCollate(bool iscase, DBInfo info)
        {
            InitCollationName(info);
            if (iscase && !string.IsNullOrEmpty( _collationCaseName))
            {
                return " collate " + _collationCaseName;
            }
            if (!iscase && !string.IsNullOrEmpty(_collationIgnoreName))
            {
                return " collate " + _collationIgnoreName;
            }
            return "";
        }
        
        ///// <summary>
        ///// ���ִ�Сд
        ///// </summary>
        ///// <param name="source"></param>
        ///// <param name="param"></param>
        ///// <param name="type"></param>
        ///// <param name="info"></param>
        ///// <returns></returns>
        //public string DoLikeCase(string source, string param,BQLLikeType type, DBInfo info)
        //{
        //    StringBuilder sbSql = new StringBuilder();
        //    sbSql.Append(" ");
        //    sbSql.Append(source);
        //    sbSql.Append(GetCollate(true));
        //    sbSql.Append(" like ");
        //    sbSql.Append(GetLikeString(this,type,param));
        //    return sbSql.ToString();
        //}
        /// <summary>
        /// ��ȡlike�Ĳ���
        /// </summary>
        /// <param name="ida"></param>
        /// <param name="type"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string GetLikeString(IDBAdapter ida, BQLLikeType type, string param)
        {
            switch (type)
            {
                case BQLLikeType.StartWith:
                    return ida.ConcatString(param, "'%'");

                case BQLLikeType.EndWith:
                    return ida.ConcatString("'%'",param);
                case BQLLikeType.Like:
                    return ida.ConcatString("'%'", param, "'%'");
                default:
                    return  param;
            }

        }

        public string DoOrderBy(string param, SortType sortType, BQLCaseType caseType, DBInfo info)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" ");
            sb.Append(param);

            switch (caseType)
            {
                case BQLCaseType.CaseIgnore:
                    sb.Append(GetCollate(false, info));
                    break;
                case BQLCaseType.CaseMatch:
                    sb.Append(GetCollate(true, info));
                    break;
                default: break;
            }
            if (sortType == SortType.DESC)
            {
                sb.Append(" desc");
            }
            return sb.ToString();
        }

        public string DoLike(string source, string param, BQLLikeType type, BQLCaseType caseType, DBInfo info)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append(" ");
            sbSql.Append(source);
            switch (caseType)
            {
                case BQLCaseType.CaseIgnore:
                    sbSql.Append(GetCollate(false, info));
                    break;
                case BQLCaseType.CaseMatch:
                    sbSql.Append(GetCollate(true, info));
                    break;
                default: break;
            }

            sbSql.Append(" like ");

            sbSql.Append(GetLikeString(this, type, param));
            return sbSql.ToString();
        }

        

        public string ShowFromLockUpdate(BQLLockType lockType, DBInfo info)
        {
            switch (lockType)
            {
                case BQLLockType.LockUpdate:
                    return "with(updlock,holdlock)";
                case BQLLockType.LockUpdateNoWait:
                    return "with(updlock,rowlock)";
                default:
                    return "";
            }
        }

        public string LockUpdate(BQLLockType lockType, DBInfo info)
        {
            return "";
        }
    }
}
