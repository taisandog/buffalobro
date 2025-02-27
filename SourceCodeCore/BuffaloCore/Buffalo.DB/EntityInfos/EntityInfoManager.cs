using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.CacheManager;
using System.Reflection;
using Buffalo.DB.PropertyAttributes;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;
using Buffalo.Kernel;
using Buffalo.Kernel.FastReflection;
using Buffalo.DB.DbCommon;
using Buffalo.Kernel.Defaults;
using Buffalo.DB.CommBase;
using Buffalo.DB.CommBase.BusinessBases;
using System.Xml;
using System.IO;
using System.Data;
using Buffalo.DB.ProxyBuilder;
using System.Collections.Concurrent;

namespace Buffalo.DB.EntityInfos
{
    /// <summary>
    /// ʵ�����Թ���
    /// </summary>
    public class EntityInfoManager
    {
        private static Dictionary<string, EntityInfoHandle> _dicClass = new Dictionary<string, EntityInfoHandle>();//��¼�Ѿ���ʼ����������

        /// <summary>
        /// ��ȡʵ������ߵ�������Ϣ
        /// </summary>
        /// <param name="type">����</param>
        /// <param name="isThrowException">�Ƿ��׳��쳣</param>
        /// <returns></returns>
        public static EntityInfoHandle GetEntityHandle(Type type,bool isThrowException)
        {
            
            string fullName = type.FullName;
            EntityInfoHandle classHandle = null;

            //if (!DataAccessLoader.HasInit)
            //{
            //    DataAccessLoader.InitConfig();
            //}
            if (isThrowException && (!_dicClass.TryGetValue(fullName, out classHandle)))
            {
                throw new Exception("�Ҳ���ʵ��" + fullName + "���������ļ�");
            }

            return classHandle;
        }
        /// <summary>
        /// ��ȡʵ������ߵ�������Ϣ
        /// </summary>
        /// <param name="type">����</param>
        /// <returns></returns>
        public static EntityInfoHandle GetEntityHandle(Type type)
        {
            return GetEntityHandle(type,true);
        }
        /// <summary>
        /// ����ʵ�����Ϣ
        /// </summary>
        internal static Dictionary<string, EntityInfoHandle> AllEntity
        {
            get 
            {
                return _dicClass;
            }
        }
        public readonly static Type EntityBaseType = typeof(EntityBase);
        public readonly static Type ThinEntityBaseType = typeof(ThinModelBase);
        /// <summary>
        /// �ж��Ƿ�ϵͳ����
        /// </summary>
        /// <param name="objType"></param>
        /// <returns></returns>
        private static bool IsSysBaseType(Type objType) 
        {

            if (objType.IsGenericType && objType == ThinEntityBaseType) 
            {
                return true;
            }
            if (objType == EntityBaseType) 
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// �������Ϣ
        /// </summary>
        /// <param name="dicParam">�ֶ�</param>
        /// <param name="dicRelation">��ϵ</param>
        private static void FillEntityInfos(Dictionary<string, EntityParam> dicParam,
            Dictionary<string, TableRelationAttribute> dicRelation, Type type,
            TableAttribute tableAtt, Dictionary<string, EntityConfigInfo> dicConfigs) 
        {
            string key=type.FullName;

            Stack<XmlDocument> stkXml = new Stack<XmlDocument>();//����ջ

            EntityConfigInfo curConfig = null;
            if (!dicConfigs.TryGetValue(key, out curConfig))
            {
                throw new Exception("�Ҳ�����:"+key+"�����������ļ�");
            }
            XmlDocument docCur = curConfig.ConfigXML;
            FillEntityInfo(docCur, tableAtt);
            stkXml.Push(docCur);
            
            Type baseType = type.BaseType;
            
            List<TableRelationAttribute> lstSetAtt = new List<TableRelationAttribute>();
            List<EntityParam> lstepAtt = new List<EntityParam>();
            object[] attRels = type.GetCustomAttributes(false);
            foreach (object objAtt in attRels) 
            {
                TableRelationAttribute tr = objAtt as TableRelationAttribute;
                if (tr != null) 
                {
                    lstSetAtt.Add(tr);
                    continue;
                }
                EntityParam ep = objAtt as EntityParam;
                if (ep != null)
                {
                    lstepAtt.Add(ep);
                    continue;
                }
            }


            while (baseType != null && !IsSysBaseType(baseType)) //��丸������
            {
                EntityConfigInfo config = null;
                string baseKey =null;
                if (baseType.IsGenericType)
                {
                    baseKey = baseType.GetGenericTypeDefinition().FullName;
                }
                else 
                {
                    baseKey = baseType.FullName;
                }
                if (dicConfigs.TryGetValue(baseKey, out config))
                {
                    stkXml.Push(config.ConfigXML);
                }
                
                baseType = baseType.BaseType;
            }
            

            while (stkXml.Count > 0)
            {
                XmlDocument doc = stkXml.Pop();
                //��ʼ������
                FillPropertyInfo(doc, dicParam);
                FillRelationInfo(doc, dicRelation);
            }

            foreach (TableRelationAttribute tra in lstSetAtt) 
            {
                dicRelation[tra.FieldName] = tra;
            }

            foreach (EntityParam ep in lstepAtt)
            {
                dicParam[ep.FieldName] = ep;
            }
        }

        /// <summary>
        /// ���ʵ����Ϣ
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="dicRelation"></param>
        internal static void FillEntityInfo(XmlDocument doc, TableAttribute tableAtt)
        {
            XmlNodeList nodes = doc.GetElementsByTagName("class");
            if (nodes.Count > 0)
            {
                XmlNode node = nodes[0];
                XmlAttribute att = node.Attributes["TableName"];
                if (att != null)
                {
                    tableAtt.TableName = att.InnerText;
                }

                att = node.Attributes["BelongDB"];
                if (att != null)
                {
                    tableAtt.BelongDB = att.InnerText;
                }
                att = node.Attributes["Description"];
                if (att != null)
                {
                    tableAtt.Description = att.InnerText;
                }
                att = node.Attributes["lazy"];
                if (att != null)
                {
                    tableAtt.AllowLazy = att.InnerText=="1";
                }

                att = node.Attributes["UseCache"];
                if (att != null)
                {
                    DBInfo db = DataAccessLoader.GetDBInfo(tableAtt.BelongDB);
                    if (db != null)
                    {
                        if (att.InnerText == "1")
                        {
                            db.QueryCache.SetCacheTable(tableAtt.TableName);
                        }
                    }
                }
                
            }
        }

        /// <summary>
        /// ���ӳ����Ϣ
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="entity"></param>
        internal static void FillRelationInfo(XmlDocument doc, Dictionary<string, TableRelationAttribute> dicRelation)
        {
            XmlNodeList lstProperty = doc.GetElementsByTagName("relation");
            foreach (XmlNode node in lstProperty)
            {
                XmlAttribute att = node.Attributes["FieldName"];
                if (att == null)
                {
                    continue;
                }
                string fName = att.InnerText;
                if (string.IsNullOrEmpty(fName))
                {
                    continue;
                }
                TableRelationAttribute tr = new TableRelationAttribute();

                tr.FieldName = fName;

                att = node.Attributes["PropertyName"];
                if (att != null)
                {
                    tr.PropertyName = att.InnerText;
                }
                att = node.Attributes["SourceProperty"];
                if (att != null)
                {
                    tr.SourceName = att.InnerText;
                }

                att = node.Attributes["TargetProperty"];
                if (att != null)
                {
                    tr.TargetName = att.InnerText;
                }

                att = node.Attributes["IsParent"];
                if (att != null)
                {
                    tr.IsParent = att.InnerText == "1";
                }
                att = node.Attributes["IsToDB"];
                if (att != null)
                {
                    tr.IsToDB = att.InnerText == "1";
                }
                att = node.Attributes["Description"];
                if (att != null)
                {
                    tr.Description = att.InnerText;
                }
                dicRelation[tr.FieldName] = tr;

            }
        }

        /// <summary>
        /// ���������Ϣ
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="entity"></param>
        internal static void FillPropertyInfo(XmlDocument doc, Dictionary<string, EntityParam> dicParam)
        {
            XmlNodeList lstProperty = doc.GetElementsByTagName("property");
            foreach (XmlNode node in lstProperty)
            {
                XmlAttribute att = node.Attributes["FieldName"];
                if (att == null)
                {
                    continue;
                }
                string fName = att.InnerText;
                if (string.IsNullOrEmpty(fName))
                {
                    continue;
                }
                EntityParam ep = new EntityParam();
                ep.FieldName = fName;

                att = node.Attributes["PropertyName"];
                if (att != null)
                {
                    ep.PropertyName = att.InnerText;
                }
                att = node.Attributes["DbType"];
                if (att != null)
                {
                    ep.SqlType =(DbType)Enum.Parse(typeof(DbType),att.InnerText,true);
                }
                att = node.Attributes["Length"];
                if (att != null)
                {
                    long len = 0;
                    long.TryParse(att.InnerText, out len);
                    ep.Length = len;
                }
                att = node.Attributes["EntityPropertyType"];
                if (att != null)
                {
                    int type = 0;
                    int.TryParse(att.InnerText, out type);
                    ep.PropertyType = (EntityPropertyType)type;
                }
                att = node.Attributes["ParamName"];
                if (att != null)
                {
                    ep.ParamName = att.InnerText;
                }
                att = node.Attributes["ReadOnly"];
                if (att != null)
                {
                    ep.ReadOnly = att.InnerText=="1";
                }
                att = node.Attributes["Description"];
                if (att != null)
                {
                    ep.Description = att.InnerText;
                }
                att = node.Attributes["DefaultValue"];
                if (att != null)
                {
                    ep.DefaultValue = att.InnerText;
                }
                ep.AllowNull = true;

                dicParam[ep.FieldName] = ep;
            }
        }

        

        /// <summary>
        /// ��ʼ������ʵ��
        /// </summary>
        /// <param name="dicConfigs"></param>
        internal static void InitAllEntity(Dictionary<string, EntityConfigInfo> dicConfigs) 
        {
            Queue<EntityInfoHandle> queEntitys = new Queue<EntityInfoHandle>();
            
            foreach (KeyValuePair<string, EntityConfigInfo> item in dicConfigs) 
            {
                EntityConfigInfo info = item.Value;
                if (info.Type != null)
                {
                    queEntitys.Enqueue(InitEntityPropertyInfos(info.Type, dicConfigs));
                }
            }
            EntityProxyBuilder proxyBuilder = new EntityProxyBuilder();
            while (queEntitys.Count > 0) 
            {
                EntityInfoHandle handle = queEntitys.Dequeue();
                handle.InitInfo();
                handle.InitProxyType(proxyBuilder);
                
                _dicClass[handle.ProxyType.FullName] = handle;
            }

        }

        /// <summary>
        /// ���û�ҵ����ֶκ͹�ϵ
        /// </summary>
        /// <param name="dicParams">�ֶ�����</param>
        /// <param name="dicRelation">��ϵ����</param>
        /// <param name="dicNotFindParam">û�ҵ����ֶ�</param>
        /// <param name="dicNotFindRelation">û�ҵ��Ĺ�ϵ</param>
        private static void FillNotFoundField(Dictionary<string, EntityParam> dicParams,
            Dictionary<string, TableRelationAttribute> dicRelation, Dictionary<string, bool> dicNotFoundParam,
            Dictionary<string, bool> dicNotFoundRelation) 
        {
            foreach (KeyValuePair<string, EntityParam> kvpPrm in dicParams) 
            {
                dicNotFoundParam[kvpPrm.Key] = true;
            }
            foreach (KeyValuePair<string, TableRelationAttribute> kvpTr in dicRelation)
            {
                dicNotFoundRelation[kvpTr.Key] = true;
            }
        }

        /// <summary>
        /// ��ʼ�����͵�������Ϣ
        /// </summary>
        /// <param name="type">����</param>
        /// <returns>����Ѿ���ʼ�����෵��false</returns>
        private static EntityInfoHandle InitEntityPropertyInfos(Type type,
            Dictionary<string, EntityConfigInfo> dicConfigs)
        {
            if (type == null)
            {
                return null;
            }


            string fullName = type.FullName;
            TableAttribute tableAtt = new TableAttribute();
            CreateInstanceHandler createrHandle = null;
            //ʵ���������͵ľ��
            if (!type.IsGenericType)
            {
                createrHandle = FastValueGetSet.GetCreateInstanceHandlerWithOutCache(type);
            }
            Dictionary<string, EntityPropertyInfo> dicPropertys = new Dictionary<string, EntityPropertyInfo>();
            Dictionary<string, EntityMappingInfo> dicMapping = new Dictionary<string, EntityMappingInfo>();

            Dictionary<string, EntityParam> dicParamsInfo = new Dictionary<string, EntityParam>();
            Dictionary<string, TableRelationAttribute> dicRelationInfo = new Dictionary<string, TableRelationAttribute>();
            FillEntityInfos(dicParamsInfo, dicRelationInfo, type, tableAtt, dicConfigs);
            DBInfo db = DataAccessLoader.GetDBInfo(tableAtt.BelongDB);
            if (db == null) 
            {
                throw new DataException("���������ݿ�����:" + tableAtt.BelongDB);
            }
            IDBAdapter idb = db.CurrentDbAdapter;
            EntityInfoHandle classInfo = new EntityInfoHandle(type, createrHandle, tableAtt, db);

            Dictionary<string, bool> dicNotFoundParam=new Dictionary<string,bool>();
            Dictionary<string, bool> dicNotFoundRelation = new Dictionary<string, bool>();
            FillNotFoundField(dicParamsInfo, dicRelationInfo, dicNotFoundParam, dicNotFoundRelation);

            //������Ϣ���
            List<FieldInfoHandle> lstFields=FieldInfoHandle.GetFieldInfos(type, FastValueGetSet.AllBindingFlags, true);
            DataBaseOperate oper = db.DefaultOperate;
            ConcurrentDictionary<string, PropertyInfo> dicPros = new ConcurrentDictionary<string, PropertyInfo>();
            PropertyInfo[] pinfos = type.GetProperties(FastValueGetSet.AllBindingFlags);
            foreach (PropertyInfo info in pinfos)
            {
                MethodInfo metinfo=info.GetGetMethod();
                if (metinfo == null) 
                {
                    continue;
                }
                ParameterInfo[] mpinfos = metinfo.GetParameters();
                if (mpinfos != null && mpinfos.Length > 0) //�����������ԣ�����this[arg]
                {
                    continue;
                }

                dicPros[info.Name] = info;
            }
                ///��ȡ���Ա���
            foreach (FieldInfoHandle finf in lstFields)
            {

                ///ͨ������������
                EntityParam ep = null;


                if (dicParamsInfo.TryGetValue(finf.FieldName, out ep))
                {
                    //if (tableAtt.IsParamNameUpper)
                    //{
                    //    ep.ParamName = ep.ParamName.ToUpper();
                    //}
                    string proName = ep.PropertyName;
                    //GetFieldValueHandle getHandle = FastFieldGetSet.GetGetValueHandle(finf);
                    //SetFieldValueHandle setHandle = FastFieldGetSet.GetSetValueHandle(finf);
                    if (finf.HasGetHandle || finf.HasSetHandle)
                    {
                        //PropertyInfo pinfo = type.GetProperty(ep.PropertyName,FastValueGetSet.AllBindingFlags);
                        PropertyInfo pinfo = ValueConvertExtend.GetDicDataValue<string, PropertyInfo>(dicPros,ep.PropertyName);
                        if (pinfo == null)
                        {
                            throw new Exception("�Ҳ�������:" + ep.PropertyName);
                        }
                        EntityPropertyInfo entityProperty = new EntityPropertyInfo(
                            classInfo, finf.GetHandle,finf.SetHandle, ep, finf.FieldType, finf.FieldName,
                            finf.BelongFieldInfo, pinfo);
                        dicPropertys[proName]= entityProperty;
                        dicNotFoundParam.Remove(finf.FieldName);
                    }
                }
                else
                {
                    TableRelationAttribute tableMappingAtt = null;

                    if (dicRelationInfo.TryGetValue(finf.FieldName, out tableMappingAtt))
                    {
                        Type targetType = DefaultType.GetRealValueType(finf.FieldType);
                        tableMappingAtt.SetEntity(type, targetType);
                        //GetFieldValueHandle getHandle = FastFieldGetSet.GetGetValueHandle(finf);
                        //SetFieldValueHandle setHandle = FastFieldGetSet.GetSetValueHandle(finf);
                        //PropertyInfo pinfo = type.GetProperty(tableMappingAtt.PropertyName, FastValueGetSet.AllBindingFlags);
                        PropertyInfo pinfo = ValueConvertExtend.GetDicDataValue<string, PropertyInfo>(dicPros, tableMappingAtt.PropertyName);
                        if (pinfo == null)
                        {
                            throw new Exception("�Ҳ�������:" + ep.PropertyName);
                        }
                        EntityMappingInfo entityMappingInfo = new EntityMappingInfo(
                            type, finf.GetHandle, finf.SetHandle, tableMappingAtt,
                            finf.FieldName, finf.FieldType, finf.BelongFieldInfo, pinfo);
                        dicMapping[tableMappingAtt.PropertyName]=entityMappingInfo;
                        dicNotFoundRelation.Remove(finf.FieldName);
                    }

                }

            }


            

            if (dicNotFoundParam.Count > 0 || dicNotFoundRelation.Count > 0) 
            {
                StringBuilder message = new StringBuilder();
                
                foreach(KeyValuePair<string,bool> kvp in dicNotFoundParam)
                {
                    message.Append(kvp.Key + "��");
                }
                

                foreach (KeyValuePair<string, bool> kvp in dicNotFoundRelation)
                {
                    message.Append(kvp.Key + "��");
                }
                if (message.Length > 0)
                {
                    message.Remove(message.Length - 1, 1);
                }
                message.Insert(0, "��:" + type.FullName + " �Ҳ����ֶ�");
                throw new MissingFieldException(message.ToString());
            }
            classInfo.SetInfoHandles(dicPropertys, dicMapping);
            FillAttributeInfo(type, classInfo);
            _dicClass[fullName] = classInfo;
            return classInfo;
        }

        /// <summary>
        /// ���������Ϣָ����ǩ
        /// </summary>
        /// <param name="type"></param>
        /// <param name="dicParamsInfo"></param>
        /// <param name="tableAtt"></param>
        private static void FillAttributeInfo(Type type, EntityInfoHandle classInfo) 
        {
            object[] atts = type.GetCustomAttributes(true);
            foreach (object objAtt in atts) 
            {
                SequenceAttributes satt = objAtt as SequenceAttributes;
                if (satt != null)
                {
                    EntityPropertyInfo info = classInfo.PropertyInfo[satt.PropertyName];
                    if (info == null)
                    {
                        throw new System.MissingMemberException("ʵ��:" + classInfo.EntityType.FullName
                            + "  ,�����������Ҳ�������:" + satt.PropertyName);
                    }
                    info.ParamInfo.SequenceName = satt.SequenceName;
                    continue;
                }

                EntityParam ep = objAtt as EntityParam;
                if (ep != null)
                {
                    EntityPropertyInfo info = classInfo.PropertyInfo[ep.PropertyName];
                    if (info == null)
                    {
                        throw new System.MissingMemberException("ʵ��:" + classInfo.EntityType.FullName
                            + "  ,�����������Ҳ�������:" + ep.PropertyName);
                    }
                    info.ParamInfo = ep;
                    continue;
                }

                TableRelationAttribute tra = objAtt as TableRelationAttribute;
                if (tra != null)
                {
                    EntityMappingInfo info = classInfo.MappingInfo[tra.PropertyName];
                    if (info == null)
                    {
                        throw new System.MissingMemberException("ʵ��:" + classInfo.EntityType.FullName
                            + "  ,�����������Ҳ�������:" + tra.PropertyName);
                    }
                    info.MappingInfo = tra;
                    continue;
                }
            }
        }

    

        /// <summary>
        /// ��ȡĳ������������
        /// </summary>
        /// <param name="finf"></param>
        /// <returns></returns>
        private static TableRelationAttribute GetMappingParam(FieldInfo finf)
        {
            object entityParam = FastInvoke.GetPropertyAttribute(finf, typeof(TableRelationAttribute));
            if (entityParam != null)
            {
                return (TableRelationAttribute)entityParam;
            }
            return null;
        }
    }
}
