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
using System.Threading.Tasks;

namespace Buffalo.Data.PostgreSQL
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
        public virtual bool KeyWordDEFAULTFront()
        {
            return false;
        }
        // <summary>
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
        /// 获取当前时间
        /// </summary>
        /// <returns></returns>
        public string GetNowDate(DbType dbType)
        {
            return "now()";
        }
        
        /// <summary>
        /// 获取当前时间
        /// </summary>
        /// <returns></returns>
        public string GetUTCDate(DbType dbType)
        {
            return "(now() at time zone 'utc')";
        }
        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <returns></returns>
        public string GetTimeStamp(DbType dbType)
        {
            return "extract(epoch FROM now())";
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
        /// 重建参数集合
        /// </summary>
        /// <param name="lstPrm"></param>
        /// <returns></returns>
        public virtual ParamList RebuildParamList(ref string sql, ParamList lstPrm)
        {
            return lstPrm;
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
        //public ParamList BQLSelectParamList
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

            IDataParameter newParam = new NpgsqlParameter();
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
        public string GetTopSelectSql(SelectCondition sql,int top) 
        {
            StringBuilder sbSql = new StringBuilder(sql.GetSelect());
            //sbSql.Append(sql);
            sbSql.Append(" limit " + top + " offset 0");
            return sbSql.ToString();
        }

        /// <summary>
        /// 把数据类型转换成当前数据库支持的类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public DbType ToCurrentDbType(DbType type)
        {
            return type;
        }
        /// <summary>
        /// 获取SQL命令类
        /// </summary>
        /// <returns></returns>
        public DbCommand GetCommand() 
        {
            DbCommand comm = new NpgsqlCommand();
            return comm;
        }
        /// <summary>
        /// 获取SQL连接
        /// </summary>
        /// <returns></returns>
        public DbConnection GetConnection(DBInfo db)
        {
            DbConnection conn = new NpgsqlConnection();
            return conn;
        }
        /// <summary>
        /// 获取SQL适配器
        /// </summary>
        /// <returns></returns>
        public IDbDataAdapter GetAdapter()
        {
            IDbDataAdapter adapter = new NpgsqlDataAdapter();
            return adapter;
        }

        /// <summary>
        /// 格式化字段名
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public string FormatParam(string paramName) 
        {

            return "\"" + paramName + "\"";
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
            return ":" + pname;
        }

        /// <summary>
        /// 格式化变量的键名
        /// </summary>
        /// <param name="pname"></param>
        /// <returns></returns>
        public string FormatParamKeyName(string pname)
        {
            return pname;
        }
        /// <summary>
        /// 返回全文检索的查询语句
        /// </summary>
        /// <param name="paranName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public string ContainsLike(string paranName, string value) 
        {
            return " to_tsvector(" + paranName + ") @@ to_tsquery("+value+")";
        }
        /// <summary>
        /// 返回全文检索的查询语句
        /// </summary>
        /// <param name="paranName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public string FreeTextLike(string paranName, string value)
        {
            throw new NotImplementedException("PostgreSQL不包含FreeText方法");
        }
        

        /// <summary>
        /// 查询并且返回DataSet(游标分页)
        /// </summary>
        /// <param name="sql">要查询的SQL语句</param>
        /// <param name="objPage">分页对象</param>
        /// <param name="oper">数据库对象</param>
        /// <param name="curType">映射的实体类型(如果用回数据库的原列名，则此为null)</param>
        /// <returns></returns>
        public DataTable QueryDataTable(string sql,ParamList lstParam, PageContent objPage, DataBaseOperate oper, Type curType)
        {
            return CursorPageCutter.QueryDataTable(sql, lstParam, objPage, oper, curType);
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
        public DbDataReader Query(string sql,ParamList lstParam, PageContent objPage, DataBaseOperate oper)
        {
            return CursorPageCutter.Query(sql, lstParam, objPage, oper);
        }

       
        /// <summary>
        /// 生成分页SQL语句
        /// </summary>
        /// <param name="list">参数列表</param>
        /// <param name="oper">连接对象</param>
        /// <param name="objCondition">条件对象</param>
        /// <param name="objPage">分页记录类</param>
        /// <returns></returns>
        public virtual string CreatePageSql(ParamList list, DataBaseOperate oper,
            SelectCondition objCondition, PageContent objPage,bool useCache) 
        {
            return CutPageSqlCreater.CreatePageSql(list, oper, objCondition, objPage, useCache);
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
        /// 把变量转变成SQL语句中的时间表达式
        /// </summary>
        /// <returns></returns>
        public string GetDateTimeString(object value)
        {
            return "'" + value.ToString().Replace("'","") + "'";
        }

        // <summary>
        /// 插入时候自动增长的字段名
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public string GetIdentityParamName(EntityPropertyInfo info) 
        {
            return "\"" + info.ParamName + "\"";
            //return null;
        }
        /// <summary>
        /// 插入时候的主键字段值
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public string GetInsertPKParamValue(EntityPropertyInfo info)
        {
            return null;
        }
        /// <summary>
        /// 获取自动增长值的SQL
        /// </summary>
        /// <returns></returns>
        public string GetIdentityValueSQL(EntityPropertyInfo info)
        {
            if (info == null)
            {
                throw new Exception("找不到自增属性");
            }
            return "select nextval('\"" + GetSequenceName(info) + "\"')";
            
        }
        /// <summary>
        /// 获取自动增长的SQL
        /// </summary>
        /// <returns></returns>
        public string GetIdentitySQL(EntityPropertyInfo info)
        {
            if (info == null)
            {
                throw new Exception("找不到自增属性");
            }
            return "select currval('\"" + GetSequenceName(info) + "\"')";
        }
        /// <summary>
        /// 根据Reader的内容把数值赋进实体
        /// </summary>
        /// <param name="reader">Reader</param>
        /// <param name="index">当前Reader的索引</param>
        /// <param name="arg">目标对象</param>
        /// <param name="info">目标属性的句柄</param>
        public void SetObjectValueFromReader(IDataReader reader, int index, object arg, EntityPropertyInfo info, bool needChangeType)
        {
            Buffalo.DB.DataBaseAdapter.SqlServer2KAdapter.DBAdapter.ValueFromReader(reader, index, arg, info,needChangeType);
        }
        /// <summary>
        /// 根据Reader的内容把数值赋进实体
        /// </summary>
        /// <param name="reader">Reader</param>
        /// <param name="index">当前Reader的索引</param>
        /// <param name="arg">目标对象</param>
        /// <param name="info">目标属性的句柄</param>
        public async Task SetObjectValueFromReaderAsync(DbDataReader reader, int index, object arg, EntityPropertyInfo info, bool needChangeType)
        {
            await Buffalo.DB.DataBaseAdapter.SqlServer2KAdapter.DBAdapter.ValueFromReaderAsync(reader, index, arg, info, needChangeType);
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
            if (pInfo == null)
            {
                return "COMMENT ON TABLE " + FormatTableName(table.TableName) + " IS " + descriptionValue;
            }
            return "COMMENT ON COLUMN " + FormatTableName(table.TableName) + "." + FormatParam(pInfo.ParamName) + " IS " + descriptionValue;
        }
        #region IDBAdapter 成员


        public string GetIdentityParamValue(EntityInfoHandle entityInfo, EntityPropertyInfo info)
        {
            return "nextval('\"" + GetSequenceName(info) + "\"')";
        }

        public string GetSequenceName(EntityPropertyInfo info)
        {
            return SequenceManager.GetSequenceName(info);
        }

        /// <summary>
        /// 初始化序列名
        /// </summary>
        /// <param name="seqName"></param>
        public string GetSequenceInit(string seqName, EntityParam prm, DataBaseOperate oper)
        {
            return SequenceManager.GetInitSequence(seqName,prm, oper);
            
        }
        /// <summary>
        ///  获取默认序列名
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="paramName">字段名</param>
        /// <returns></returns>
        public string GetDefaultSequenceName(string tableName, string paramName)
        {
            return SequenceManager.GetDefaultName(tableName,paramName);
        }


        /// <summary>
        /// 获取数据库的自增长字段的信息
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
        /// 创建表语句的结束
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public string CreateTableSQLEnd(DBInfo info)
        {
            return null;
        }

        /// <summary>
        /// like不区分大小写
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

        public string ShowFromLockUpdate(BQLLockType lockType, DBInfo info)
        {
            return "";
        }

        public string LockUpdate(BQLLockType lockType, DBInfo info)
        {
            switch (lockType)
            {
                case BQLLockType.LockUpdate:
                    return "for update";
                case BQLLockType.LockUpdateNoWait:
                    return "for update nowait";
                case BQLLockType.LockUpdateSkipLock:
                    return "for update skip locked";
                default:
                    return "";
            }
        }

        public Task<string> CreatePageSqlAsync(ParamList list, DataBaseOperate oper, SelectCondition objCondition, PageContent objPage, bool useCache)
        {
            return CutPageSqlCreater.CreatePageSqlAsync(list, oper, objCondition, objPage, useCache);
        }

        public Task<DbDataReader> QueryAsync(string sql, ParamList lstParam, PageContent objPage, DataBaseOperate oper)
        {
            return CursorPageCutter.QueryAsync(sql, lstParam, objPage, oper);
        }

        public Task<DataTable> QueryDataTableAsync(string sql, ParamList lstParam, PageContent objPage, DataBaseOperate oper, Type curType)
        {
            return CursorPageCutter.QueryDataTableAsync(sql, null, objPage, oper, curType);
        }
    }
}
