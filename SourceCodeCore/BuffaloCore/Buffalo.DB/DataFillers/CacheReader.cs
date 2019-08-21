using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

using Buffalo.DB.CommBase;
using Buffalo.DB.PropertyAttributes;
using System.Collections;
using System.Runtime.InteropServices;
using Buffalo.DB.EntityInfos;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;
using Buffalo.Kernel.Defaults;
namespace Buffalo.DB.DataFillers
{
    /// <summary>
    /// Ϊ��װ���󴴽�������Ϣ
    /// </summary>
    public class CacheReader
    {
        //internal static Dictionary<string, List<PropertyInfoHandle>> dicReaderCache = new Dictionary<string, List<PropertyInfoHandle>>();

        /// <summary>
        /// ����Reader�ṹ����DataTable
        /// </summary>
        /// <param name="reader">Reader</param>
        /// <param name="datatableName">���ݱ���</param>
        /// <param name="isEmpty">�Ƿ����ɿյ�DataTable</param>
        /// <returns></returns>
        public static DataTable GenerateDataTable(IDataReader reader,string datatableName,bool isEmpty) 
        {
            DataTable dt = new DataTable();
            if (!string.IsNullOrEmpty(datatableName))
            {
                dt.TableName = datatableName;
            }
            
            dt.BeginLoadData();
            int fieldCount = reader.FieldCount;
            for (int i = 0; i < fieldCount; i++) 
            {
                dt.Columns.Add(reader.GetName(i), reader.GetFieldType(i));
            }
            if (!isEmpty) 
            {
                
                while (reader.Read())
                {
                    DataRow dr = dt.NewRow();
                    
                    for (int i = 0; i < fieldCount; i++)
                    {
                        if (!reader.IsDBNull(i))
                        {
                            dr[i] = reader[i];
                        }
                    }
                    dt.Rows.Add(dr);
                    dt.AcceptChanges();
                }
                
            }
            dt.EndLoadData();
            return dt;
        }
        /// <summary>
        /// ����Reader�ṹ����DataTable
        /// </summary>
        /// <param name="reader">Reader</param>
        /// <param name="datatableName">���ݱ���</param>
        /// <param name="isEmpty">�Ƿ����ɿյ�DataTable</param>
        /// <returns></returns>
        public static DataSet GenerateDataSet(IDataReader reader, bool isEmpty)
        {
            
            DataSet ds = new DataSet();
            int index=0;
            do
            {
                DataTable dt = GenerateDataTable(reader, "table" + index,isEmpty);
                ds.Tables.Add(dt);
            } while (reader.NextResult());
            
            return ds;
        }

        /// <summary>
        /// ����Reader�ṹ��ʵ�����Ե�ӳ�����ɿյ�DataTable
        /// </summary>
        /// <param name="reader">Reader</param>
        /// <param name="datatableName">���ݱ���</param>
        /// <param name="entityType">ʵ����</param>
        /// <param name="isEmpty">�Ƿ����ɿյ�DataTable</param>
        /// <returns></returns>
        public static DataTable GenerateDataTable(IDataReader reader, string datatableName, Type entityType, bool isEmpty)
        {
            DataTable dt = new DataTable();
            EntityInfoHandle entityInfo = EntityInfoManager.GetEntityHandle(entityType);
            List<EntityPropertyInfo> lstParamNames = GenerateCache(reader, entityInfo);
            dt.BeginLoadData();
            foreach (EntityPropertyInfo info in lstParamNames)
            {
                if (info != null)
                {
                    Type fieldType = info.FieldType;
                    if (DefaultType.EqualType(fieldType , DefaultType.BooleanType))
                    {
                        fieldType = typeof(bool);
                    }
                    dt.Columns.Add(info.PropertyName, fieldType);
                }
            }
            if (!isEmpty)
            {
                
                while (reader.Read())
                {
                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < lstParamNames.Count; i++)
                    {
                        if (!reader.IsDBNull(i) && lstParamNames[i] != null)
                        {
                            dr[i] = reader[i];
                        }
                    }
                    dt.Rows.Add(dr);
                    dt.AcceptChanges();
                }
                
            }
            dt.EndLoadData();
            return dt;
        }
        /// <summary>
        /// ����Reader����
        /// </summary>
        internal static List<EntityPropertyInfo> GenerateCache(IDataReader reader, EntityInfoHandle entityInfo)
        {
            IDBAdapter idb = entityInfo.DBInfo.CurrentDbAdapter;
            Dictionary<string, EntityPropertyInfo> dicParamHandle = new Dictionary<string, EntityPropertyInfo>();//��ȡ�ֶ��������Զ��յļ���
            //Dictionary<string, EntityPropertyInfo>.Enumerator enums = entityInfo.PropertyInfo.GetPropertyEnumerator();
            foreach (EntityPropertyInfo info in entityInfo.PropertyInfo) 
            {
                //EntityPropertyInfo info=enums.Current.Value;
                dicParamHandle.Add(info.ParamName, info);
                dicParamHandle[idb.FormatParam(info.ParamName)]=info;
            }

            List<EntityPropertyInfo> cacheReader = new List<EntityPropertyInfo>();
            
            for (int i = 0; i < reader.FieldCount; i++) //�����԰���ָ���ֶ���˳�����е�List���
            {
                EntityPropertyInfo info=null;
                if (dicParamHandle.TryGetValue(reader.GetName(i), out info))//��ȡ���ݿ��ֶ�����Ӧ��ʵ���ֶη���
                {
                    cacheReader.Add(info);
                }
                else 
                {
                    cacheReader.Add(null);
                }
            }
            return cacheReader;
            //dicReaderCache.Add(type.FullName,cacheReader);
        }

        /// <summary>
        /// ��Reader��߶�ȡһ����������
        /// </summary>
        /// <typeparam name="T">����</typeparam>
        /// <param name="reader">reader</param>
        /// <returns></returns>
        public static T LoadFormReader<T>(IDataReader reader,EntityInfoHandle entityInfo) where T : EntityBase, new()
        {
            //string fullName = typeof(T).FullName;
            if (reader != null && !reader.IsClosed)
            {
                List<EntityPropertyInfo> lstParamNames = GenerateCache(reader, entityInfo);
                T ret = (T)entityInfo.CreateSelectProxyInstance();//ʵ��������
                FillObjectFromReader(reader, lstParamNames, ret, entityInfo.DBInfo);
                return ret;
            }
            return default(T);
        }
        /// <summary>
        /// ��Reader��߶�ȡһ����������
        /// </summary>
        /// <typeparam name="T">����</typeparam>
        /// <param name="reader">reader</param>
        /// <returns></returns>
        public static T LoadFormReader<T>(IDataReader reader) where T : EntityBase, new()
        {
            EntityInfoHandle entityInfo = EntityInfoManager.GetEntityHandle(typeof(T));
            if (reader != null && !reader.IsClosed)
            {
                List<EntityPropertyInfo> lstParamNames = GenerateCache(reader, entityInfo);
                T ret = (T)entityInfo.CreateSelectProxyInstance();//ʵ��������
                FillObjectFromReader(reader, lstParamNames, ret, entityInfo.DBInfo);
                return ret;
            }
            return default(T);
        }
        public static void FillInfoFromReader(IDataReader reader, EntityInfoHandle entityInfo, object obj) 
        {
            if (reader != null && !reader.IsClosed && obj!=null)
            {
                List<EntityPropertyInfo> lstParamNames = GenerateCache(reader, entityInfo);

                FillObjectFromReader(reader, lstParamNames, obj, entityInfo.DBInfo);
                
            }
        }


        /// <summary>
        /// ��Reader��߶�ȡ���ݼ���(����)
        /// </summary>
        /// <typeparam name="T">����</typeparam>
        /// <param name="reader">reader</param>
        /// <returns></returns>
        public static List<T> LoadFormReaderList<T>(IDataReader reader) where T : EntityBase, new()
        {
            EntityInfoHandle entityInfo = EntityInfoManager.GetEntityHandle(typeof(T));
            List<T> retLst = new List<T>();
            if (reader != null && !reader.IsClosed)
            {
                List<EntityPropertyInfo> lstParamNames = GenerateCache(reader, entityInfo);
               
                while (reader.Read())
                {
                    T obj = entityInfo.CreateSelectProxyInstance() as T;
                    FillObjectFromReader(reader, lstParamNames, obj, entityInfo.DBInfo);
                    retLst.Add(obj);
                }
            }
            return retLst;
        }

        

        /// <summary>
        /// ��Reader��ֵ��������
        /// </summary>
        /// <param name="reader">reader</param>
        /// <param name="lstParams">������Ӧ�б�</param>
        /// <param name="obj">����</param>
        internal static void FillObjectFromReader(IDataReader reader, List<EntityPropertyInfo> lstParams, object obj,DBInfo db) 
        {
            IDBAdapter dbAdapter = db.CurrentDbAdapter;
            for (int i = 0; i < lstParams.Count; i++)//��Reader��ֵ����������
            {
                if (!reader.IsDBNull(i) && lstParams[i]!=null)
                {
                    dbAdapter.SetObjectValueFromReader(reader, i, obj, lstParams[i], !lstParams[i].TypeEqual(reader, i));
                }
            }
        }

        /// <summary>
        /// ��Reader��߶�ȡ���ݼ���(����,���ؼ��ϵĴ�С)
        /// </summary>
        /// <typeparam name="T">����</typeparam>
        /// <param name="reader">reader</param>
        /// <param name="entityInfo">ʵ������Ϣ</param>
        /// <param name="totalSize">�����ܴ�С</param>
        /// <returns></returns>
        public static List<T> LoadFormReaderList<T>(IDataReader reader,EntityInfoHandle entityInfo, out int totalSize) where T : EntityBase, new()
        {
            List<T> retLst = new List<T>();
            totalSize = 0;//��ʼ����ǰ��¼���ܴ�С
            if (reader != null && !reader.IsClosed)
            {
                List<EntityPropertyInfo> lstParamNames = GenerateCache(reader, entityInfo);
                
                while (reader.Read())
                {
                    T obj = (T)entityInfo.CreateSelectProxyInstance();
                    int curSize = 0;//��ȡ��ǰֵ��С
                    FillObjectFromReader(reader, lstParamNames, obj, out curSize);
                    totalSize += curSize;//�ӵ���ǰ��¼�ܴ�С���
                    retLst.Add(obj);
                }
            }
            return retLst;
        }

        /// <summary>
        /// ��Reader��ֵ��������
        /// </summary>
        /// <param name="reader">reader</param>
        /// <param name="lstParams">������Ӧ�б�</param>
        /// <param name="obj">����</param>
        /// <param name="itemSize">������¼�Ĵ�С</param>
        internal static void FillObjectFromReader(IDataReader reader, List<EntityPropertyInfo> lstParams, object obj, out int itemSize)
        {
            itemSize = 0;//��ʼ����ǰ��¼�ܴ�С
            for (int i = 0; i < lstParams.Count; i++)//��Reader��ֵ����������
            {
                if (!reader.IsDBNull(i))
                {
                    object val = reader.GetValue(i);
                    int curSize=CurValueSize(val);//��ȡ��ǰֵ��С
                    lstParams[i].SetValue(obj,val);
                    itemSize += curSize;//�ӵ���ǰ��¼�ܴ�С���
                }
            }
        }

        /// <summary>
        /// ��ȡ��ֵռ�õĿռ�
        /// </summary>
        /// <param name="value">ֵ</param>
        /// <returns></returns>
        internal static int CurValueSize(object value) 
        {
            if (value == null) 
            {
                return 0;
            }
            Type type = value.GetType();
            if (type.IsValueType)
            {
                return Marshal.SizeOf(type);
            }
            if (DefaultType.EqualType(type , DefaultType.StringType)) //�����ַ����Ŀռ�
            {
                int size = (value.ToString().Length+1) * Marshal.SizeOf(typeof(char));
                return size;
            }
            if (DefaultType.EqualType(type , DefaultType.BytesType)) //�����ֽ�����Ŀռ�
            {
                int size = ((byte[])value).Length * Marshal.SizeOf(typeof(byte));
                return size;
            }
            return 0;
        }

        
    }
}
