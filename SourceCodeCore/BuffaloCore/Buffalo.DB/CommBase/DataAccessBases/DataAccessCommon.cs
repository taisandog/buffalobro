using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data;

using Buffalo.DB.FaintnessSearchConditions;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.DB.QueryConditions;
using Buffalo.DB.DbCommon;
using Buffalo.Kernel.Defaults;
using Buffalo.DB.EntityInfos;
using Buffalo.DB.BQLCommon.BQLConditionCommon;
using Buffalo.Kernel;
using Buffalo.DB.DataFillers;
using Buffalo.DB.BQLCommon;

namespace Buffalo.DB.CommBase.DataAccessBases
{
    public class DataAccessCommon
    {

        /// <summary>
        /// ��ȡ���������ַ���
        /// </summary>
        /// <param name="objScope"></param>
        /// <returns></returns>
        public static string GetConnectString(Scope objScope)
        {
            if (objScope.ConnectType == ConnectType.And)
            {
                return "And";
            }
            return "Or";
        }


        /// <summary>
        /// ��ʽ����������
        /// </summary>
        /// <param name="pam">����</param>
        /// <param name="index">��ǰ�ı�ʶ</param>
        /// <returns></returns>
        public static string FormatParam(string pam, int index)
        {
            return pam + "_" + index.ToString();
        }

        /// <summary>
        /// ����ѯ����������������SQL���( and ��ͷ)
        /// </summary>
        /// <param name="lstParam">�����б�</param>
        /// <param name="lstScope">��Χ��ѯ����</param>
        /// <returns></returns>
        internal static string FillCondition(EntityInfoHandle curEntityInfo,ParamList lstParam, ScopeBaseList lstScope)
        {
            int index = 0;
            return FillCondition(curEntityInfo,lstParam, lstScope, ref index);
        }


        /// <summary>
        /// ����ѯ����������������SQL���( and ��ͷ)
        /// </summary>
        /// <param name="lstParam">�����б�</param>
        /// <param name="lstScope">��Χ��ѯ����</param>
        /// <param name="CurEntityInfo">��ǰʵ����Ϣ</param>
        /// <param name="index">����</param>
        /// <returns></returns>
        internal static string FillCondition(EntityInfoHandle curEntityInfo, ParamList lstParam, ScopeBaseList lstScope, ref int index)
        {
            if (lstScope == null)
            {
                return "";
            }
            StringBuilder ret = new StringBuilder();
            for (int i = 0; i < lstScope.Count; i++)
            {
                Scope objScope = lstScope[i];
                EntityPropertyInfo info = null;
                if (!string.IsNullOrEmpty(objScope.PropertyName))
                {
                    info = curEntityInfo.PropertyInfo[objScope.PropertyName];
                }

                if (objScope.ScopeType == ScopeType.Scope)
                {
                    ScopeList lstInnerScope = objScope.Value1 as ScopeList;
                    if (lstInnerScope != null)
                    {
                        string strSql = FillCondition(curEntityInfo,lstParam, lstInnerScope, ref index);
                        string connectString = DataAccessCommon.GetConnectString(objScope);
                        StringBuilder sbstrSQL = new StringBuilder(strSql);
                        DataAccessCommon.TrimAnd(sbstrSQL);
                        ret.Append(" ");
                        ret.Append(connectString);
                        ret.Append(" (" + sbstrSQL.ToString() + ")");
                    }
                }
                else
                {
                    string pName = (info != null ? info.ParamName : "");
                    DbType dbType = (info != null ? info.SqlType : DbType.Object);
                    ret.Append(FormatScorp(objScope, lstParam, pName, dbType, index, curEntityInfo.EntityType));
                }
                index++;
            }
            return ret.ToString();
        }
        /// <summary>
        /// ɾ����ʼ��������
        /// </summary>
        /// <param name="sb"></param>
        internal static void TrimHead(StringBuilder sb)
        {
            if (sb.Length == 0)
            {
                return;
            }
            int sindex = GetStartIndex(sb);
            const string sqlTrim = " 1=1 and";
            //const string sqlTrim = "1=1 and";
            int len = sb.Length > sqlTrim.Length ? sqlTrim.Length : sb.Length;
            len += sindex;
            string andSql = sb.ToString(0, len);
            
            if (andSql.IndexOf("1=1 and", sindex, StringComparison.CurrentCultureIgnoreCase) == sindex)
            {
                sb.Remove(sindex, sqlTrim.Length - 1);
                return;
            }
            if (andSql.IndexOf("1=1", sindex, StringComparison.CurrentCultureIgnoreCase) == sindex
                && len < sqlTrim.Length)
            {
                sb.Remove(sindex, 3);
                return;
            }
            if (andSql.IndexOf(" 1=1", sindex, StringComparison.CurrentCultureIgnoreCase) == sindex
                && len < sqlTrim.Length)
            {
                sb.Remove(sindex, 4);
                return;
            }
            if (andSql.IndexOf(" 1=1 and", sindex, StringComparison.CurrentCultureIgnoreCase) == sindex)
            {
                sb.Remove(sindex, sqlTrim.Length);
                return;
            }
        }

        /// <summary>
        /// ��ȡ������ʼ������
        /// </summary>
        /// <param name="sb"></param>
        /// <returns></returns>
        private static int GetStartIndex(StringBuilder sb) 
        {
            char cur = '\0';
            for (int i = 0; i < sb.Length; i++) 
            {
                cur = sb[i];
                if (cur != ' ' && cur != '(') 
                {
                    return i;
                }
            }
            return 0;
        }

        /// <summary>
        /// ɾ����ʼ��������
        /// </summary>
        /// <param name="sb"></param>
        internal static void TrimAnd(StringBuilder sb)
        {
            if (sb.Length == 0) 
            {
                return;
            }
            int len = sb.Length > 5 ? 5 : sb.Length;
            string andSql = sb.ToString(0, len);
            if (andSql.IndexOf(" and ",StringComparison.CurrentCultureIgnoreCase)==0)
            {
                sb.Remove(0, 5);
            }
            else if (andSql.IndexOf("and ", StringComparison.CurrentCultureIgnoreCase) == 0) 
            {
                sb.Remove(0, 4);
            }
            else if (andSql.IndexOf(" or ", StringComparison.CurrentCultureIgnoreCase) == 0)
            {
                sb.Remove(0, 4);
            }
            else if (andSql.IndexOf("or ", StringComparison.CurrentCultureIgnoreCase) == 0)
            {
                sb.Remove(0, 3);
            }
        }

        /// <summary>
        /// ���ص�ǰ�������ַ���
        /// </summary>
        /// <param name="scope">������</param>
        /// <param name="list">�����б�</param>
        /// <param name="paramName">�������ֶ���</param>
        /// <param name="type">��ǰ�����ݿ�����</param>
        /// <param name="lstIndex">��ǰ�����ı�ʶδ���ͬ���ֶεĲ�����������Ϊ0</param>
        /// <param name="entityType">��ǰʵ�������</param>
        /// <returns></returns>
        public static string FormatScorp(Scope scope, ParamList list, string paramName, DbType type, int lstIndex,Type entityType)
        {
            string sql = null;
            ScopeExplainer handle=ExplainScope.GetExplainer(scope);
            if (handle != null) 
            {
                sql = handle(scope, list, entityType, paramName, type, lstIndex);
            }
            
            
            return sql;
        }

        /// <summary>
        /// �Ѳ�ѯ�����ӵ��������ַ�����
        /// </summary>
        /// <param name="value">ֵ</param>
        /// <param name="type">���ݿ���ߵ�����</param>
        /// <returns></returns>
        public static string FormatValue(object value, DbType type,DBInfo db)
        {
            if (value == null)
            {
                return null;
            }

            switch (type)
            {
                case DbType.AnsiString:
                case DbType.AnsiStringFixedLength:
                case DbType.String:
                case DbType.StringFixedLength:
                    return "'" + value.ToString().Replace("'", "''") + "'";
                case DbType.Guid:
                    if (value is Guid)
                    {
                        return Buffalo.Kernel.CommonMethods.GuidToString((Guid)value);
                    }
                    return value.ToString();
                case DbType.DateTime:
                case DbType.Time:
                case DbType.Date:
                case DbType.DateTime2:
                case DbType.DateTimeOffset:
                    return db.CurrentDbAdapter.GetDateTimeString(value);
                case DbType.Decimal:
                case DbType.Double:
                case DbType.Int32:
                case DbType.Int16:
                case DbType.Int64:
                case DbType.SByte:
                case DbType.Byte:
                case DbType.Currency:
                case DbType.UInt16:
                case DbType.UInt32:
                case DbType.UInt64:
                case DbType.VarNumeric:
                case DbType.Single:
                    return value.ToString().Replace(" ","");
                case DbType.Binary:
                    byte[] binaryValue = value as byte[];
                    if (binaryValue != null)
                    {
                        string hexVal = CommonMethods.BytesToHexString(binaryValue,true);
                        return "0x" + hexVal;
                    }
                    return "";
                case DbType.Boolean:
                    bool valBool = Convert.ToBoolean(value);
                    if (valBool == true)
                    {
                        return "1";
                    }
                    else
                    {
                        return "0";
                    }
                default:
                    return null;
            }
        }
        #region �������
        /// <summary>
        /// ����������б�
        /// </summary>
        /// <param name="lst"></param>
        /// <param name="childName"></param>
        public static void FillEntityChidList(IList lst, ScopeList lstScope)
        {
            ShowChildCollection childs = lstScope.ShowChild;
            if(childs==null)
            {
                return;
            }
            foreach (ShowChildItem showTable in childs) 
            {

                FillEntityChidList(lst, showTable);
            }
        }

        /// <summary>
        /// ����Ӽ���
        /// </summary>
        /// <param name="showTable"></param>
        /// <param name="?"></param>
        public static void FillEntityChidList(IList lst, ShowChildItem showTable)
        {
            Stack<BQLEntityTableHandle> stkTables = new Stack<BQLEntityTableHandle>();
            BQLEntityTableHandle curTable = showTable.ChildItem;
            while (true)
            {
                stkTables.Push(curTable);
                curTable = curTable.GetParentTable();
                if (CommonMethods.IsNull(curTable) || string.IsNullOrEmpty(curTable.GetPropertyName())) 
                {
                    break;
                }
            }
            IEnumerable curList = lst;
            Queue<object> lastObjects = new Queue<object>();
            ScopeList empty = new ScopeList();
            while (stkTables.Count > 0)
            {
                BQLEntityTableHandle table = stkTables.Pop();
                ScopeList lstScope = null;
                if (stkTables.Count == 0)
                {
                    lstScope = showTable.FilterScope;
                }
                else 
                {
                    lstScope = empty;
                }
                FillEntityChidList(curList, table.GetPropertyName(), lastObjects, table.GetParentTable().GetEntityInfo().EntityType, lstScope);
                curList = lastObjects;
                lastObjects=new Queue<object>();
            }

        }

        /// <summary>
        /// ����������б�
        /// </summary>
        /// <param name="lst">����</param>
        /// <param name="childPropertyName">��������</param>
        /// <param name="objs">ʵ��</param>
        /// <param name="objType">����</param>
        /// <param name="filter">ɸѡ����</param>
        private static void FillEntityChidList(IEnumerable lst, string childPropertyName, Queue<object> objs, Type objType,ScopeList filter)
        {
            if (lst == null)
            {
                return;
            }
            
            EntityInfoHandle entityInfo = EntityInfoManager.GetEntityHandle(objType);
            if (entityInfo == null)
            {
                throw new Exception("�Ҳ�����:" + objType.FullName + "��ӳ��");
            }
            EntityMappingInfo mappingInfo = entityInfo.MappingInfo[childPropertyName];
            if (mappingInfo == null)
            {
                throw new Exception("�Ҳ���������:" + childPropertyName);
            }
            Queue<object> pks = CollectFks(lst, mappingInfo.SourceProperty);
            EntityInfoHandle childHandle = mappingInfo.TargetProperty.BelongInfo;//��ȡ��Ԫ�ص���Ϣ
            Dictionary<string, List<object>> dic = GetEntityDictionary(lst, mappingInfo);
            FillChilds(pks, childHandle, mappingInfo, dic, childPropertyName, objs, filter);
        }

        /// <summary>
        /// �Ѽ���ת���ֵ���ʽ
        /// </summary>
        /// <param name="lst">����</param>
        /// <param name="mappingInfo">ӳ��</param>
        /// <returns></returns>
        private static Dictionary<string, List<object>> GetEntityDictionary(IEnumerable lst, EntityMappingInfo mappingInfo) 
        {
            Dictionary<string, List<object>> dic = new Dictionary<string, List<object>>();
            string propertyName=mappingInfo.PropertyName;

            List<object> lstCur = null;
            foreach (object obj in lst) 
            {
                EntityBase entity = obj as EntityBase;
                if(entity==null)
                {
                    continue;
                }
                string key = mappingInfo.SourceProperty.GetValue(entity).ToString();
                if (!dic.TryGetValue(key, out lstCur)) 
                {
                    lstCur = new List<object>();
                    dic[key] = lstCur;
                }
                lstCur.Add(entity);
                //�����Ӽ���
                if (!mappingInfo.IsParent)
                {
                    object childlst = Activator.CreateInstance(mappingInfo.FieldType);
                    mappingInfo.SetValue(entity, childlst);
                    entity.OnFillChild(propertyName, new ScopeList());
                }
            }
            return dic;
        }

        /// <summary>
        /// �ռ����ֵ
        /// </summary>
        /// <param name="lst"></param>
        /// <param name="pk"></param>
        /// <returns></returns>
        private static Queue<object> CollectFks(IEnumerable lst, EntityPropertyInfo pk)
        {
            Queue<object> pks = new Queue<object>();
            foreach (object obj in lst)
            {
                object value = pk.GetValue(obj);
                pks.Enqueue(value);
            }
            return pks;
        }

        private const int MaxID =500;

        /// <summary>
        /// ��������б�
        /// </summary>
        /// <param name="pks">ID����</param>
        /// <param name="childHandle">��Ԫ�ص���Ϣ���</param>
        /// <param name="mappingInfo">ӳ����Ϣ</param>
        /// <param name="dicElement">Ԫ��</param>
        /// <param name="propertyName">������</param>
        /// <param name="curObjs"></param>
        /// <param name="filter"></param>
        private static void FillChilds(Queue<object> pks, EntityInfoHandle childHandle, EntityMappingInfo mappingInfo,
            Dictionary<string, List<object>> dicEntity, string propertyName, Queue<object> curObjs,ScopeList filter)
        {

            DBInfo db = childHandle.DBInfo;
            DataBaseOperate oper = childHandle.DBInfo.DefaultOperate;
            BQLDbBase dao = new BQLDbBase(oper);
            Queue<object> needCollect = null;
            List<EntityPropertyInfo> lstParamNames=null;
            try
            {
                while (pks.Count>0) 
                {
                    needCollect = GetCurPks(pks);
                    FillChildReader(needCollect, mappingInfo, dicEntity, dao, ref lstParamNames, db,curObjs,filter);
                }
            }
            finally 
            {
                oper.AutoClose();
            }
        }
        /// <summary>
        /// ��ȡ��ǰ��Ҫ��ѯ����������
        /// </summary>
        /// <param name="pks"></param>
        /// <returns></returns>
        private static Queue<object> GetCurPks(Queue<object> pks) 
        {
            Queue<object> needCollect = null;
            int cur = pks.Count;
            if (cur > MaxID) 
            {
                cur = MaxID;
            }
            needCollect = new Queue<object>();
            for (int i = 0; i < cur; i++) 
            {
                needCollect.Enqueue(pks.Dequeue());
            }
            return needCollect;
        }

        /// <summary>
        /// ��ѯ�����������Ϣ
        /// </summary>
        /// <param name="pks"></param>
        /// <param name="mappingInfo"></param>
        /// <param name="dicEntity"></param>
        /// <param name="dao"></param>
        /// <param name="lstParamNames"></param>
        /// <param name="db"></param>
        /// <param name="curObjs"></param>
        /// <param name="filter"></param>
        private static void FillChildReader(Queue<object> pks, EntityMappingInfo mappingInfo, Dictionary<string, List<object>> dicEntity, BQLDbBase dao,
            ref List<EntityPropertyInfo> lstParamNames, DBInfo db,Queue<object> curObjs,ScopeList filter) 
        {
            EntityInfoHandle childInfo = mappingInfo.TargetProperty.BelongInfo;
            string fullName = mappingInfo.TargetProperty.BelongInfo.EntityType.FullName;
            Type childType = mappingInfo.TargetProperty.BelongInfo.EntityType;
            List<object> senders = null;

            while (pks.Count > 0)
            {
                Queue<object> searchPks = GetSearchPKs(pks);
                if (searchPks.Count <= 0) 
                {
                    break;
                }

                ScopeList lstScope = new ScopeList();
                lstScope.AddScopeList(filter);
                lstScope.AddIn(mappingInfo.TargetProperty.PropertyName, searchPks);
                using (IDataReader reader = dao.QueryReader(lstScope, childInfo.EntityType))
                {
                    //��ȡ�ӱ��get�б�
                    if (lstParamNames == null)
                    {
                        lstParamNames = CacheReader.GenerateCache(reader, childInfo);//����һ��������ֵ�б�
                    }

                    
                    while (reader.Read())
                    {
                        string fk = reader[mappingInfo.TargetProperty.ParamName].ToString();
                        if (!dicEntity.TryGetValue(fk, out senders))
                        {
                            continue;
                        }
                        object obj = childInfo.CreateSelectProxyInstance();

                        if (curObjs != null)
                        {
                            curObjs.Enqueue(obj);
                        }
                        CacheReader.FillObjectFromReader(reader, lstParamNames, obj, db);

                        foreach (object sender in senders)
                        {
                            if (mappingInfo.IsParent)
                            {
                                mappingInfo.SetValue(sender, obj);
                            }
                            else
                            {
                                IList lst = (IList)mappingInfo.GetValue(sender);
                                lst.Add(obj);
                            }
                        }
                    }
                }
            }
        
        }

        /// <summary>
        /// ÿ�β�ѯ������
        /// </summary>
        internal const int PreSearch=50;
        /// <summary>
        /// ��ȡ��Ҫ��ѯ������
        /// </summary>
        /// <param name="pks"></param>
        /// <returns></returns>
        private static Queue<object> GetSearchPKs(Queue<object> pks) 
        {
            Queue<object> searchPks = new Queue<object>();
            int copyed = 0;
            while (pks.Count > 0) 
            {
                searchPks.Enqueue(pks.Dequeue());
                copyed++;
                if (copyed >= PreSearch) 
                {
                    break;
                }
            }
            return searchPks;
        }

        #endregion
    }
}
