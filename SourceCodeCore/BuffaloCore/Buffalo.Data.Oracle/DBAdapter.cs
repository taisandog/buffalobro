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
        /// 全文搜索时候排序字段是否显示表达式
        /// </summary>
        public virtual bool IsShowExpression
        {
            get
            {
                return false;
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
        public bool IdentityIsType
        {
            get { return false; }
        }
        /// <summary>
        /// 是否记录自增长字段作手动处理
        /// </summary>
        public bool IsSaveIdentityParam
        {
            get
            {
                return true;
            }

        }
        /// <summary>
        /// 获取在字段添加SQL
        /// </summary>
        /// <param name="table">表</param>
        /// <param name="pInfo">字段（如果为空则设置表注释）</param>
        /// <returns></returns>
        public virtual string GetColumnDescriptionSQL(EntityParam pInfo, DBInfo info)
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
        /// 格式化数据库类型
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
        /// 获取top的查询字符串
        /// </summary>
        /// <param name="sql">查询字符串</param>
        /// <param name="top">top值</param>
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
        public virtual IDbCommand GetCommand()
        {
            IDbCommand comm = new OracleCommand();
            return comm;
        }
        /// <summary>
        /// 获取SQL连接
        /// </summary>
        /// <returns></returns>
        public virtual DbConnection GetConnection(DBInfo db)
        {
            DbConnection conn = new OracleConnection();
            return conn;
        }
        /// <summary>
        /// 获取SQL适配器
        /// </summary>
        /// <returns></returns>
        public virtual IDbDataAdapter GetAdapter()
        {
            IDbDataAdapter adapter = new OracleDataAdapter();
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
        public string FreeTextLike(string paranName, string value)
        {
            throw new NotImplementedException("Oracle不包含FreeText方法");
        }
        /// <summary>
        /// 返回全文检索的查询语句
        /// </summary>
        /// <param name="paranName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public string ContainsLike(string paranName, string value)
        {
            return " (contains(" + paranName + "," + value + ")>0)";
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
            return CursorPageCutter.Query(sql, null, objPage, oper);
        }
        /// <summary>
        /// 获取当前时间
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
        /// 获取当前时间
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
        /// 获取时间戳
        /// </summary>
        /// <returns></returns>
        public string GetTimeStamp(DbType dbType)
        {
            return "((sysdate -TO_DATE('19700101','yyyymmdd'))*86400 - TO_NUMBER(SUBSTR(TZ_OFFSET(sessiontimezone),1,3))*3600)";
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
            return CursorPageCutter.QueryDataTable(sql, null, objPage, oper, curType);
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
        public IDataReader Query(string sql, ParamList lstParam, PageContent objPage, DataBaseOperate oper)
        {
            return CursorPageCutter.Query(sql, lstParam, objPage, oper);
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
        public DataTable QueryDataTable(string sql, ParamList lstParam, PageContent objPage, DataBaseOperate oper, Type curType)
        {
            return CursorPageCutter.QueryDataTable(sql, lstParam, objPage, oper, curType);
        }
        /// <summary>
        /// 生成分页SQL语句
        /// </summary>
        /// <param name="list">参数列表</param>
        /// <param name="oper">连接对象</param>
        /// <param name="objCondition">条件对象</param>
        /// <param name="objPage">分页记录类</param>
        /// <returns></returns>
        public string CreatePageSql(ParamList list, DataBaseOperate oper, SelectCondition objCondition,
            PageContent objPage,bool useCache)
        {
            return CutPageSqlCreater.CreatePageSql(list, oper, objCondition, objPage,useCache);
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
        /// 获取自动增长的SQL
        /// </summary>
        /// <returns></returns>
        public string GetIdentitySQL(EntityPropertyInfo pkInfo)
        {
            
            if (pkInfo == null)
            {
                throw new Exception("找不到主键属性");
            }
            return "select \"" + GetSequenceName(pkInfo) + "\".currval as curVal from dual";
        }
        /// <summary>
        /// 获取自动增长值的SQL
        /// </summary>
        /// <returns></returns>
        public string GetIdentityValueSQL(EntityPropertyInfo pkInfo)
        {
            
            if (pkInfo == null)
            {
                throw new Exception("找不到主键属性");
            }
            return "select \"" + GetSequenceName(pkInfo) + "\".nextval as curVal from dual";
        }
        /// <summary>
        /// 获取序列名
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="entityInfo">字段名</param>
        /// <returns></returns>
        public string GetSequenceName(EntityPropertyInfo info)
        {
            return SequenceManager.GetSequenceName(info);
        }

        /// <summary>
        ///  获取默认序列名
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="paramName">字段名</param>
        /// <returns></returns>
        public string GetDefaultSequenceName(string tableName, string paramName)
        {
            return SequenceManager.GetDefaultName(tableName, paramName);
        }
        /// <summary>
        /// 初始化序列名
        /// </summary>
        /// <param name="seqName"></param>
        public string GetSequenceInit(string seqName, EntityParam prm, DataBaseOperate oper)
        {
            return SequenceManager.GetInitSequence(seqName, oper);
        }

        /// <summary>
        /// 把变量转变成SQL语句中的时间表达式
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
        /// 插入时候自动增长的字段名
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public string GetIdentityParamName(EntityPropertyInfo info)
        {
            return "\"" + info.ParamName + "\"";
        }
        /// <summary>
        /// 插入时候自动增长的字段值
        /// </summary>
        /// <param name="entityInfo">实体信息</param>
        /// <param name="info">属性信息</param>
        /// <returns></returns>
        public string GetIdentityParamValue(EntityInfoHandle entityInfo, EntityPropertyInfo info)
        {
            return "\"" + GetSequenceName(info) + "\".nextval";
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
            DB.DataBaseAdapter.SqlServer2KAdapter.DBAdapter.ValueFromReader(reader, index, arg, info, needChangeType);
        }



        /// <summary>
        /// 获取数据库的自增长字段的信息
        /// </summary>
        /// <returns></returns>
        public virtual string DBIdentity(string tableName, string paramName)
        {

            return "";
        }

        /// <summary>
        /// 把DBType类型转成对应的SQLType
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
        /// 获取创建注释的SQL
        /// </summary>
        /// <param name="table">表</param>
        /// <param name="paramName">字段(如果为空则给表设置注释)</param>
        /// <param name="description">注释</param>
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
        /// 创建表语句的结束
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public string CreateTableSQLEnd(DBInfo info)
        {
            return null;
        }
    }
}
