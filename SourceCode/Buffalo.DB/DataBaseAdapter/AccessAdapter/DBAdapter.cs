using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OleDb;
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
namespace Buffalo.DB.DataBaseAdapter.AccessAdapter
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
            get { return true; }
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
        /// 获取数据库的自增长字段的信息
        /// </summary>
        /// <returns></returns>
        public virtual string DBIdentity(string tableName, string paramName) 
        {
            return "autoincrement(1,1)";
        }

        /// <summary>
        /// 重建参数集合
        /// </summary>
        /// <param name="lstPrm"></param>
        /// <returns></returns>
        public virtual ParamList RebuildParamList(ref string sql,ParamList lstPrm) 
        {
            ParamList lstRet = new ParamList();
            StringBuilder newSql = new StringBuilder();
            Dictionary<string, DBParameter> dicPrm = new Dictionary<string, DBParameter>();
            foreach (DBParameter prm in lstPrm) 
            {
                dicPrm[prm.ParameterName] = prm;
            }
            Queue<RebuildParamInfo> queStrPrm = FindAllParams(sql);
            DBParameter curPrm=null;

            RebuildParamInfo curprmInfo=null;
            if(queStrPrm.Count>0)
            {
                curprmInfo=queStrPrm.Dequeue();
            }
            for (int i = 0; i < sql.Length; i++)
            {
                if (curprmInfo != null && curprmInfo.Index==i) 
                {
                    if (dicPrm.TryGetValue(curprmInfo.ParamName, out curPrm)) 
                    {
                        string pName="P"+lstRet.Count;
                        DBParameter newPrm = lstRet.AddNew(FormatParamKeyName(pName), curPrm.DbType, curPrm.Value, curPrm.Direction);
                        newPrm.ValueName = FormatValueName(pName);
                        newSql.Append(newPrm.ValueName);
                        i += curprmInfo.ParamName.Length -1;

                        if (queStrPrm.Count > 0)
                        {
                            curprmInfo = queStrPrm.Dequeue();
                        }
                        continue;
                    }
                    
                }
                newSql.Append(sql[i]);
            }
            sql = newSql.ToString();
            return lstRet;
        }

        private static Dictionary<char, bool> _dicPrm = InitPrm();
        /// <summary>
        /// 变量名称可用字符
        /// </summary>
        /// <returns></returns>
        private static Dictionary<char, bool> InitPrm()
        {
            Dictionary<char, bool> ret = new Dictionary<char, bool>();
            string chars = "abcdefghijklmnopqrstuvwxyz";
            foreach (char chr in chars)
            {
                ret[chr] = true;
            }
            chars = chars.ToUpper();
            foreach (char chr in chars)
            {
                ret[chr] = true;
            }
            chars = "0123456789_";
            foreach (char chr in chars)
            {
                ret[chr] = true;
            }
            return ret;
        }
        /// <summary>
        /// 收集所有变量
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        private static Queue<RebuildParamInfo> FindAllParams(string sql)
        {
            CharCollectionEnumerator cenum = new CharCollectionEnumerator(sql);
            Queue<RebuildParamInfo> prms = new Queue<RebuildParamInfo>();
            
            StringBuilder buffer = new StringBuilder();
            while (cenum.MoveNext())
            {
                if (cenum.CurrentChar == '@')
                {
                    RebuildParamInfo rpf = new RebuildParamInfo();
                    rpf.Index = cenum.CurIndex;
                    buffer.Append(cenum.Current);
                    while (cenum.MoveNext())
                    {
                        if (_dicPrm.ContainsKey(cenum.CurrentChar))
                        {
                            buffer.Append(cenum.Current);
                        }
                        else
                        {
                            break;
                        }
                    }
                    
                    rpf.ParamName = buffer.ToString();
                    prms.Enqueue(rpf);
                    buffer.Remove(0, buffer.Length);
                }
                else if (cenum.CurrentChar == '\'')
                {
                    bool hasChar = false;//判断是否遇到过单引号
                    while (cenum.MoveNext())
                    {
                        if (cenum.CurrentChar == '\'')
                        {
                            hasChar = !hasChar;
                        }
                        else
                        {
                            if (hasChar) //如果本字符不是单引号，且上一个字符是单引号，则结束字符串
                            {
                                break;
                            }
                        }
                    }
                }
            }
            return prms;
        }


        /// <summary>
        /// 把DBType类型转成对应的SQLType
        /// </summary>
        /// <param name="dbType"></param>
        /// <returns></returns>
        public virtual string DBTypeToSQL(DbType dbType,long length) 
        {
            int type = ToRealDbType(dbType,length);
            DbType stype = (DbType)type;
            switch (stype) 
            {
                case DbType.AnsiString:
                case DbType.AnsiStringFixedLength:
                case DbType.String:
                case DbType.StringFixedLength:
                    if (length <= 255) 
                    {
                        return "Text(" + length + ")";
                    }
                    return "Memo";

                case DbType.Boolean:
                    return "YesNo";

                case DbType.Byte:
                    return "Byte";

                case DbType.Currency:
                    return "Currency";

                case DbType.Date:
                    return "Date";

                case DbType.DateTime:
                case DbType.DateTime2:
                case DbType.DateTimeOffset:
                case DbType.Time:
                    return "DateTime";

                case DbType.Single:
                    return "Single";
               

                case DbType.Double:
                    return "Double";

                case DbType.SByte:
                case DbType.Int16:
                    return "Integer";

                case DbType.UInt16:
                case DbType.Int32:
                    return "Long";

                case DbType.Int64:
                case DbType.UInt32:
                case DbType.UInt64:
                case DbType.Decimal:
                case DbType.VarNumeric:
                    return "Decimal";

                case DbType.Binary:
                    return "OLEObject";

                case DbType.Guid:
                    return "ReplicationID";
                default:
                    return stype.ToString();
            }

           
        }

        /// <summary>
        /// 把DBType转成本数据库的实际类型
        /// </summary>
        /// <param name="dbType"></param>
        /// <returns></returns>
        public virtual int ToRealDbType(DbType dbType,long length) 
        {
            //switch (dbType) 
            //{
            //    case DbType.AnsiString:
            //        if (length < 8000)
            //        {
            //            return (int)OleDbType.Char;
            //        }
            //        else 
            //        {
            //            return (int)OleDbType.LongVarChar;
            //        }
            //    case DbType.AnsiStringFixedLength:
            //        if (length < 8000)
            //        {
            //            return (int)OleDbType.Char;
            //        }
            //        else 
            //        {
            //            return (int)OleDbType.LongVarChar;
            //        }
            //    case DbType.Binary:
            //        if (length < 8000)
            //        {
            //            return (int)OleDbType.Binary;
            //        }
            //        else
            //        {
            //            return (int)OleDbType.LongVarBinary;
            //        }
            //    case DbType.Boolean:
            //        return (int)OleDbType.Boolean;
            //    case DbType.Byte:
            //        return (int)OleDbType.UnsignedTinyInt;
            //    case DbType.Currency:
            //        return (int)OleDbType.Currency;
            //    case DbType.Date:
            //        return (int)OleDbType.DBDate;
            //    case DbType.DateTime:
            //    case DbType.DateTime2:
            //    case DbType.DateTimeOffset:
            //        return (int)OleDbType.DBTimeStamp;
            //    case DbType.Time:
            //        return (int)OleDbType.DBTime;
            //    case DbType.Decimal:
            //        return (int)OleDbType.Decimal;
            //    case DbType.Double:
            //        return (int)OleDbType.Double;
            //    case DbType.Guid:
            //        return (int)OleDbType.Guid;
            //    case DbType.Int16:
            //        return (int)OleDbType.SmallInt;
            //    case DbType.UInt16:
            //        return (int)OleDbType.UnsignedSmallInt;
            //    case DbType.Int32:
            //        return (int)OleDbType.Integer;
            //    case DbType.Int64:
            //        return (int)OleDbType.BigInt;
            //    case DbType.SByte:
            //        return (int)OleDbType.TinyInt;
            //    case DbType.Single:
            //        return (int)OleDbType.Single;
            //    case DbType.String:
            //        if (length < 4000)
            //        {
            //            return (int)OleDbType.VarWChar;
            //        }
            //        else 
            //        {
            //            return (int)OleDbType.LongVarWChar;
            //        }
            //    case DbType.StringFixedLength:
            //        if (length < 4000)
            //        {
            //            return (int)OleDbType.WChar;
            //        }
            //        else
            //        {
            //            return (int)OleDbType.LongVarWChar;
            //        }
               
            //    case DbType.UInt32:
            //        return (int)OleDbType.UnsignedInt;
            //    case DbType.UInt64:
            //        return (int)OleDbType.UnsignedBigInt;
            //    case DbType.VarNumeric:
            //        return (int)OleDbType.VarNumeric;
            //    default:
            //        return (int)OleDbType.PropVariant;

            //}
            return (int)dbType;
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
        //        return null;
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
            OleDbParameter newParam = new OleDbParameter();
            newParam.ParameterName = paramName;
            newParam.DbType = type;
            if (newParam.OleDbType == OleDbType.DBTimeStamp && paramValue is DateTime)
            {
                newParam.Value = ((DateTime)paramValue).ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {
                newParam.Value = paramValue;
            }
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
            IDbCommand comm = new OleDbCommand();
            return comm;
        }
        /// <summary>
        /// 获取SQL连接
        /// </summary>
        /// <returns></returns>
        public virtual DbConnection GetConnection(DBInfo db)
        {
            DbConnection conn = new OleDbConnection();
            return conn;
        }
        /// <summary>
        /// 获取SQL适配器
        /// </summary>
        /// <returns></returns>
        public IDbDataAdapter GetAdapter()
        {
            IDbDataAdapter adapter = new OleDbDataAdapter();
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
            return "now()";
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
            throw new NotSupportedException("不支持游标分页");
            //return CursorPageCutter.Query(sql, objPage, oper);
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
            throw new NotSupportedException("不支持游标分页");
            //return CursorPageCutter.QueryDataTable(sql, objPage, oper, curType);
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
            throw new Exception("Access不支持带参数的游标分页");
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
            throw new Exception("Access不支持带参数的游标分页");
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
            return CutPageSqlCreater.CreatePageSql(list, oper, objCondition, objPage,objCondition.CacheTables);
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
            //string tableValue = DataAccessCommon.FormatValue(table.TableName, DbType.AnsiString, info);
            //string description = pInfo == null ? table.Description : pInfo.Description;
            
            //string descriptionValue = DataAccessCommon.FormatValue(description, DbType.AnsiString, info);
            //if (pInfo==null)
            //{

            //    return "EXECUTE sp_addextendedproperty N'MS_Description', N" + descriptionValue + ", N'SCHEMA', N'dbo', N'TABLE', N" + tableValue + ", NULL, NULL";
            //}
            //return "EXECUTE sp_addextendedproperty N'MS_Description', N" + descriptionValue + ", N'SCHEMA', N'dbo', N'TABLE', N" + tableValue + ", N'COLUMN', N'" + pInfo.ParamName + "'";
            return "";
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
            ValueFromReader(reader, index, arg, info, needChangeType);
        }
        public bool OnConnectionClosed(DbConnection conn, DBInfo db)
        {
            conn.Dispose();
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
