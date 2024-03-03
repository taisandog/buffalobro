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
        /// 全文搜索时候排序字段是否显示表达式
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
        /// 获取数据库的自增长字段的信息
        /// </summary>
        /// <returns></returns>
        public virtual string DBIdentity(string tableName, string paramName)
        {

            return "IDENTITY(1,1)";
        }
        /// <summary>
        /// 重建参数集合
        /// </summary>
        /// <param name="lstPrm"></param>
        /// <returns></returns>
        public virtual ParamList RebuildParamList(ref string sql, ParamList lstPrm)
        {
            return lstPrm;
        }
        /// <summary>
        /// 获取在字段添加SQL
        /// </summary>
        /// <param name="table">表</param>
        /// <param name="pInfo">字段（如果为空则设置表注释）</param>
        /// <returns></returns>
        public virtual string GetColumnDescriptionSQL( EntityParam pInfo, DBInfo info) 
        {
            return null;
        }
        /// <summary>
        /// 把DBType类型转成对应的SQLType
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
        /// 清空表
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
        /// 把DBType转成本数据库的实际类型
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
        /// 是否记录自增长字段作手动处理
        /// </summary>
        public bool IsSaveIdentityParam
        {
            get 
            {
                return false;
            }
        }

        ///// <summary>
        ///// 获取变量列表
        ///// </summary>
        //public virtual ParamList BQLSelectParamList
        //{
        //    get 
        //    {
        //        return new ParamList();
        //    }
        //}
        /// <summary>
        /// 获取参数类
        /// </summary>
        /// <param name="paramName">参数名</param>
        /// <param name="type">参数数据库类型</param>
        /// <param name="paramValue">参数值</param>
        /// <param name="paramDir">参数进出类型</param>
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
        /// 获取top的查询字符串
        /// </summary>
        /// <param name="sql">查询字符串</param>
        /// <param name="top">top值</param>
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
        /// 获取序列名
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="entityInfo">字段名</param>
        /// <returns></returns>
        public string GetSequenceName(EntityPropertyInfo info)
        {
            return null;
        }
        /// <summary>
        ///  获取默认序列名
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="paramName">字段名</param>
        /// <returns></returns>
        public string GetDefaultSequenceName(string tableName, string paramName) 
        {
            return null;
        }
        /// <summary>
        /// 初始化序列名
        /// </summary>
        /// <param name="seqName"></param>
        public string GetSequenceInit(string seqName,EntityParam prm, DataBaseOperate oper)
        {
            return null;
        }
        /// <summary>
        /// 把数据类型转换成当前数据库支持的类型
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
        /// 获取SQL命令类
        /// </summary>
        /// <returns></returns>
        public IDbCommand GetCommand() 
        {
            IDbCommand comm = new SqlCommand();
            return comm;
        }
        /// <summary>
        /// 获取SQL连接
        /// </summary>
        /// <returns></returns>
        public virtual DbConnection GetConnection(DBInfo db)
        {
            DbConnection conn = new SqlConnection();
            return conn;
        }
        /// <summary>
        /// 获取SQL适配器
        /// </summary>
        /// <returns></returns>
        public IDbDataAdapter GetAdapter()
        {
            IDbDataAdapter adapter = new SqlDataAdapter();
            return adapter;
        }

        /// <summary>
        /// 格式化字段名
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public string FormatParam(string paramName) 
        {
            
            return "[" + paramName + "]";
        }

        /// <summary>
        /// 格式化表格名
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public string FormatTableName(string tableName)
        {
            return FormatParam(tableName);
        }
        /// <summary>
        /// 格式化变量名
        /// </summary>
        /// <param name="pname"></param>
        /// <returns></returns>
        public string FormatValueName(string pname)
        {
            return "@" + pname;
        }

        /// <summary>
        /// 格式化变量的键名
        /// </summary>
        /// <param name="pname"></param>
        /// <returns></returns>
        public string FormatParamKeyName(string pname)
        {
            return "@" + pname;
        }
        /// <summary>
        /// 返回全文检索的查询语句
        /// </summary>
        /// <param name="paranName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public string FreeTextLike(string param,string value) 
        {
            return " (freetext(" + param + "," + value + "))";
        }
        /// <summary>
        /// 返回全文检索的查询语句
        /// </summary>
        /// <param name="paranName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public string ContainsLike(string paranName, string value)
        {
            return " (contains(" + paranName + "," + value + "))";
        }
        /// <summary>
        /// 获取当前时间
        /// </summary>
        /// <returns></returns>
        public string GetNowDate(DbType dbType)
        {
            return "getdate()";
        }

        /// <summary>
        /// 获取当前时间
        /// </summary>
        /// <returns></returns>
        public string GetUTCDate(DbType dbType)
        {
            return "getutcdate()";
        }
        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <returns></returns>
        public string GetTimeStamp(DbType dbType)
        {
            return "DATEDIFF(s, '1970-01-01 00:00:00', getutcdate())";
        }
        /// <summary>
        /// 游标分页
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="sql">sql语句</param>
        /// <param name="objPage">分页实体</param>
        /// <param name="oper">数据库链接</param>
        /// <returns></returns>
        public IDataReader Query(string sql, PageContent objPage, DataBaseOperate oper)
        {
            return CursorPageCutter.Query(sql, objPage, oper);
        }

        /// <summary>
        /// 查询并且返回DataSet(游标分页)
        /// </summary>
        /// <param name="sql">要查询的SQL语句</param>
        /// <param name="objPage">分页对象</param>
        /// <param name="oper">数据库对象</param>
        /// <param name="curType">映射的实体类型(如果用回数据库的原列名，则此为null)</param>
        /// <returns></returns>
        public DataTable QueryDataTable(string sql, PageContent objPage, DataBaseOperate oper, Type curType)
        {
            return CursorPageCutter.QueryDataTable(sql, objPage, oper, curType);
        }
        /// <summary>
        /// 游标分页
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="lstParam">参数集合</param>
        /// <param name="sql">sql语句</param>
        /// <param name="objPage">分页实体</param>
        /// <param name="oper">数据库链接</param>
        /// <returns></returns>
        public IDataReader Query(string sql,ParamList lstParam, PageContent objPage, DataBaseOperate oper)
        {
            throw new Exception("SqlServer不支持带参数的游标分页");
        }

        /// <summary>
        /// 查询并且返回DataSet(游标分页)
        /// </summary>
        /// <param name="sql">要查询的SQL语句</param>
        /// <param name="lstParam">参数集合</param>
        /// <param name="objPage">分页对象</param>
        /// <param name="oper">数据库对象</param>
        /// <param name="curType">映射的实体类型(如果用回数据库的原列名，则此为null)</param>
        /// <returns></returns>
        public DataTable QueryDataTable(string sql,ParamList lstParam, PageContent objPage, DataBaseOperate oper, Type curType)
        {
            throw new Exception("SqlServer不支持带参数的游标分页");
        }
        /// <summary>
        /// 生成分页SQL语句
        /// </summary>
        /// <param name="list">参数列表</param>
        /// <param name="oper">连接对象</param>
        /// <param name="objCondition">条件对象</param>
        /// <param name="objPage">分页记录类</param>
        /// <returns></returns>
        public virtual string CreatePageSql(ParamList list, DataBaseOperate oper, SelectCondition objCondition, PageContent objPage,bool useCache) 
        {
            return CutPageSqlCreater.CreatePageSql(list, oper, objCondition, objPage,useCache?objCondition.CacheTables:null);
        }


        
        
        /// <summary>
        /// 获取字符串拼接SQl语句
        /// </summary>
        /// <param name="str">字符串集合</param>
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
        /// 获取自动增长的SQL
        /// </summary>
        /// <returns></returns>
        public string GetIdentitySQL(EntityPropertyInfo info) 
        {
            return "SELECT @@IDENTITY";
        }
        /// <summary>
        /// 获取自动增长值的SQL
        /// </summary>
        /// <returns></returns>
        public string GetIdentityValueSQL(EntityPropertyInfo info)
        {
            return null;
        }
        /// <summary>
        /// 把变量转变成SQL语句中的时间表达式
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
        /// 插入时候的主键字段名
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public string GetIdentityParamName(EntityPropertyInfo info) 
        {
            return null;
        }
        /// <summary>
        /// 插入时候的主键字段值
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public string GetIdentityParamValue(EntityInfoHandle entityInfo, EntityPropertyInfo info)
        {
            return null;
        }

        /// <summary>
        /// 根据Reader的内容把数值赋进实体
        /// </summary>
        /// <param name="reader">Reader</param>
        /// <param name="index">当前Reader的索引</param>
        /// <param name="arg">目标对象</param>
        /// <param name="info">目标属性的句柄</param>
        public static void ValueFromReader(IDataReader reader,int index,object arg,EntityPropertyInfo info,bool needChangeType) 
        {
            object val = reader.GetValue(index);
            //if (val is DBNull || val == null) 
            //{
            //    return;
            //}
            if (needChangeType)
            {
                Type resType = info.RealFieldType;//字段值类型
                val = CommonMethods.ChangeType(val, resType);
            }
            info.SetValue(arg, val);
        }
        /// <summary>
        /// 获取创建注释的SQL
        /// </summary>
        /// <param name="table">表</param>
        /// <param name="paramName">字段(如果为空则给表设置注释)</param>
        /// <param name="description">注释</param>
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
        /// 根据Reader的内容把数值赋进实体
        /// </summary>
        /// <param name="reader">Reader</param>
        /// <param name="index">当前Reader的索引</param>
        /// <param name="arg">目标对象</param>
        /// <param name="info">目标属性的句柄</param>
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
        /// 创建表语句的结束
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
        /// 是否检查过排序
        /// </summary>
        private bool _isCheckcollation=false;
        /// <summary>
        /// 区分大小写排序名
        /// </summary>
        private string _collationCaseName=null;
        /// <summary>
        /// 不区分大小写排序名
        /// </summary>
        private string _collationIgnoreName=null;
        /// <summary>
        /// 排序名
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
        /// 获取区分大小写SQL
        /// </summary>
        /// <param name="iscase">是否区分</param>
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
        ///// 区分大小写
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
        /// 获取like的参数
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
