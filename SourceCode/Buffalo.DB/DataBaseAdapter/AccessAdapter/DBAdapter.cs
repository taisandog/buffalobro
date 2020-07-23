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
using System.Collections.Concurrent;

namespace Buffalo.DB.DataBaseAdapter.AccessAdapter
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
            get { return true; }
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
        /// ��ȡ���ݿ���������ֶε���Ϣ
        /// </summary>
        /// <returns></returns>
        public virtual string DBIdentity(string tableName, string paramName) 
        {
            return "autoincrement(1,1)";
        }

        /// <summary>
        /// �ؽ���������
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

        private static ConcurrentDictionary<char, bool> _dicPrm = InitPrm();
        /// <summary>
        /// �������ƿ����ַ�
        /// </summary>
        /// <returns></returns>
        private static ConcurrentDictionary<char, bool> InitPrm()
        {
            ConcurrentDictionary<char, bool> ret = new ConcurrentDictionary<char, bool>();
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
        /// �ռ����б���
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
                    bool hasChar = false;//�ж��Ƿ�������������
                    while (cenum.MoveNext())
                    {
                        if (cenum.CurrentChar == '\'')
                        {
                            hasChar = !hasChar;
                        }
                        else
                        {
                            if (hasChar) //������ַ����ǵ����ţ�����һ���ַ��ǵ����ţ�������ַ���
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
        /// ��DBType����ת�ɶ�Ӧ��SQLType
        /// </summary>
        /// <param name="dbType"></param>
        /// <returns></returns>
        public virtual string DBTypeToSQL(DbType dbType, long length, bool canNull) 
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
        /// ��DBTypeת�ɱ����ݿ��ʵ������
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
        //        return null;
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
            IDbCommand comm = new OleDbCommand();
            return comm;
        }
        /// <summary>
        /// ��ȡSQL����
        /// </summary>
        /// <returns></returns>
        public virtual DbConnection GetConnection(DBInfo db)
        {
            DbConnection conn = new OleDbConnection();
            return conn;
        }
        /// <summary>
        /// ��ȡSQL������
        /// </summary>
        /// <returns></returns>
        public IDbDataAdapter GetAdapter()
        {
            IDbDataAdapter adapter = new OleDbDataAdapter();
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
            switch (dbType)
            {
                case DbType.Time:
                    return "time()";
                case DbType.Date:
                    return "date()";
                default:
                    return "now()";
            }
        }

        /// <summary>
        /// ��ȡ��ǰʱ��
        /// </summary>
        /// <returns></returns>
        public string GetUTCDate(DbType dbType)
        {
            throw new NotSupportedException("UTCʱ���ȡ");
            //return "now()";
        }
        /// <summary>
        /// ��ȡʱ���
        /// </summary>
        /// <returns></returns>
        public string GetTimeStamp(DbType dbType)
        {
            throw new NotSupportedException("ʱ�����ȡ");
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
            throw new NotSupportedException("��֧���α��ҳ");
            //return CursorPageCutter.Query(sql, objPage, oper);
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
            throw new NotSupportedException("��֧���α��ҳ");
            //return CursorPageCutter.QueryDataTable(sql, objPage, oper, curType);
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
            throw new Exception("Access��֧�ִ��������α��ҳ");
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
            throw new Exception("Access��֧�ִ��������α��ҳ");
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
            return CutPageSqlCreater.CreatePageSql(list, oper, objCondition, objPage,objCondition.CacheTables);
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
        /// ����Reader�����ݰ���ֵ����ʵ��
        /// </summary>
        /// <param name="reader">Reader</param>
        /// <param name="index">��ǰReader������</param>
        /// <param name="arg">Ŀ�����</param>
        /// <param name="info">Ŀ�����Եľ��</param>
        public void SetObjectValueFromReader(IDataReader reader, int index, object arg, EntityPropertyInfo info,bool needChangeType)
        {
            ValueFromReader(reader, index, arg, info, needChangeType);
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
        /// like�����ִ�Сд
        /// </summary>
        /// <param name="source"></param>
        /// <param name="lstParam"></param>
        /// <returns></returns>
        public string DoLike(string source, string param, BQLLikeType type, BQLCaseType caseType, DBInfo info)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append(" ");
            if (caseType == BQLCaseType.CaseIgnore)
            {
                sbSql.Append("lower(");
                sbSql.Append(source);
                sbSql.Append(") like lower(");
            }
            else
            {
                sbSql.Append(source);
            }
            sbSql.Append(Buffalo.DB.DataBaseAdapter.SqlServer2KAdapter.DBAdapter.GetLikeString(this, type, param));
            sbSql.Append(")");
            return sbSql.ToString();
        }


        public string DoOrderBy(string param, SortType sortType, BQLCaseType caseType, DBInfo info)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" ");
            if (caseType == BQLCaseType.CaseIgnore)
            {
                sb.Append("lower(");
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
