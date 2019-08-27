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
            sb.Append(" immediate");
            
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
                return false;
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
        /// <summary>
        /// 获取当前时间
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
        /// 获取数据库当前格林威治时间
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
        /// 获取时间戳
        /// </summary>
        /// <param name="dbType"></param>
        /// <returns></returns>
        public string GetTimeStamp(DbType dbType) 
        {
            return "(cast(days(current timestamp)-days('1970-01-01') as integer) * 86400 + (midnight_seconds(current timestamp-current timezone)))";
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
            IDataParameter newParam = new DB2Parameter();
            newParam.ParameterName = paramName;
            newParam.DbType = FormatDbType(type);
            newParam.Value = FormatValue(paramValue);
            newParam.Direction = paramDir;
            return newParam;
        }

        /// <summary>
        /// 格式化值
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
        /// 格式化数据库类型
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
        /// 获取top的查询字符串
        /// </summary>
        /// <param name="sql">查询字符串</param>
        /// <param name="top">top值</param>
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
        public IDbCommand GetCommand() 
        {
            IDbCommand comm = new DB2Command();
            return comm;
        }
        /// <summary>
        /// 获取SQL连接
        /// </summary>
        /// <returns></returns>
        public DbConnection GetConnection(DBInfo db)
        {
            DbConnection conn = new DB2Connection();
            return conn;
        }
        /// <summary>
        /// 获取SQL适配器
        /// </summary>
        /// <returns></returns>
        public IDbDataAdapter GetAdapter()
        {
            IDbDataAdapter adapter = new DB2DataAdapter();
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
            return "@"+pname;
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
        public string ContainsLike(string paranName, string value) 
        {
            return " (CONTAINS(" + paranName + "," + value + ")>0)";
        }
        /// <summary>
        /// 返回全文检索的查询语句
        /// </summary>
        /// <param name="paranName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public string FreeTextLike(string paranName, string value)
        {
            throw new NotImplementedException("DB2不包含FreeText方法");
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
            return CursorPageCutter.Query(sql,null, objPage, oper,null);
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
            return CursorPageCutter.QueryDataTable(sql,null, objPage, oper, curType,null);
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
            return CursorPageCutter.Query(sql, lstParam, objPage, oper,null);
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
            return CursorPageCutter.QueryDataTable(sql, lstParam, objPage, oper, curType, null);
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
        public string GetIdentitySQL(EntityPropertyInfo info) 
        {
            //if (info == null)
            //{
            //    throw new Exception("找不到主键属性");
            //}
            //return "select LASTASSIGNEDVAL from SYSIBM.SYSSEQUENCES where SEQNAME='" + GetSequenceName(info.BelongInfo.TableName, info.ParamName) + "'";
            return "VALUES IDENTITY_VAL_LOCAL()";
        }

        /// <summary>
        /// 获取自动增长值的SQL
        /// </summary>
        /// <returns></returns>
        public string GetIdentityValueSQL(EntityPropertyInfo info)
        {
            //if (info == null)
            //{
            //    throw new Exception("找不到主键属性");
            //}
            //return "PREVIOUS VALUE FOR \"" + GetSequenceName(info.BelongInfo.TableName, info.ParamName) + "\"";
            return null;
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
            return "TIMESTAMP('" + dt.ToString("yyyy-MM-dd HH:mm:ss.ms") + "')";
        }

        /// <summary>
        /// 插入时候自动增长的字段名
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public string GetIdentityParamName(EntityPropertyInfo info)
        {
            //return "\"" + info.ParamName + "\"";
            return null;
        }
        /// <summary>
        /// 插入时候自动增长的字段值
        /// </summary>
        /// <param name="entityInfo">实体信息</param>
        /// <param name="info">属性信息</param>
        /// <returns></returns>
        public string GetIdentityParamValue(EntityInfoHandle entityInfo, EntityPropertyInfo info)
        {
            //return "NEXT VALUE FOR \"" + GetSequenceName(entityInfo.TableName, info.ParamName) + "\"";
            return null;
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
        /// 获取数据库的自增长字段的信息
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
            return "COMMENT ON " + FormatTableName(table.TableName) + "(" + FormatParam(pInfo.ParamName) + " IS '标记')";
        }
        #region IDBAdapter 成员


        public bool OnConnectionClosed(DbConnection conn, DBInfo db)
        {
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
        public virtual bool KeyWordDEFAULTFront()
        {
            return true;
        }
    }
}
