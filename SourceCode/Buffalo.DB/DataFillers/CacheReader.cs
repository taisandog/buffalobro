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
    /// 为封装对象创建缓存信息
    /// </summary>
    public class CacheReader
    {
        //internal static Dictionary<string, List<PropertyInfoHandle>> dicReaderCache = new Dictionary<string, List<PropertyInfoHandle>>();

        /// <summary>
        /// 根据Reader结构生成DataTable
        /// </summary>
        /// <param name="reader">Reader</param>
        /// <param name="datatableName">数据表名</param>
        /// <param name="isEmpty">是否生成空的DataTable</param>
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
        /// 根据Reader结构生成DataTable
        /// </summary>
        /// <param name="reader">Reader</param>
        /// <param name="datatableName">数据表名</param>
        /// <param name="isEmpty">是否生成空的DataTable</param>
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
        /// 根据Reader结构和实体属性的映射生成空的DataTable
        /// </summary>
        /// <param name="reader">Reader</param>
        /// <param name="datatableName">数据表名</param>
        /// <param name="entityType">实体类</param>
        /// <param name="isEmpty">是否生成空的DataTable</param>
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
        /// 创建Reader缓存
        /// </summary>
        internal static List<EntityPropertyInfo> GenerateCache(IDataReader reader, EntityInfoHandle entityInfo)
        {
            IDBAdapter idb = entityInfo.DBInfo.CurrentDbAdapter;
            Dictionary<string, EntityPropertyInfo> dicParamHandle = new Dictionary<string, EntityPropertyInfo>();//获取字段名跟属性对照的集合
            //Dictionary<string, EntityPropertyInfo>.Enumerator enums = entityInfo.PropertyInfo.GetPropertyEnumerator();
            foreach (EntityPropertyInfo info in entityInfo.PropertyInfo) 
            {
                //EntityPropertyInfo info=enums.Current.Value;
                dicParamHandle.Add(info.ParamName, info);
                dicParamHandle[idb.FormatParam(info.ParamName)]=info;
            }

            List<EntityPropertyInfo> cacheReader = new List<EntityPropertyInfo>();
            
            for (int i = 0; i < reader.FieldCount; i++) //把属性按照指定字段名顺序排列到List里边
            {
                EntityPropertyInfo info=null;
                if (dicParamHandle.TryGetValue(reader.GetName(i), out info))//获取数据库字段名对应的实体字段反射
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
        /// 从Reader里边读取一个对象数据
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="reader">reader</param>
        /// <returns></returns>
        public static T LoadFormReader<T>(IDataReader reader,EntityInfoHandle entityInfo) where T : EntityBase, new()
        {
            //string fullName = typeof(T).FullName;
            if (reader != null && !reader.IsClosed)
            {
                List<EntityPropertyInfo> lstParamNames = GenerateCache(reader, entityInfo);
                T ret = (T)entityInfo.CreateSelectProxyInstance();//实例化对象
                FillObjectFromReader(reader, lstParamNames, ret, entityInfo.DBInfo);
                return ret;
            }
            return default(T);
        }
        /// <summary>
        /// 从Reader里边读取一个对象数据
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="reader">reader</param>
        /// <returns></returns>
        public static T LoadFormReader<T>(IDataReader reader) where T : EntityBase, new()
        {
            EntityInfoHandle entityInfo = EntityInfoManager.GetEntityHandle(typeof(T));
            if (reader != null && !reader.IsClosed)
            {
                List<EntityPropertyInfo> lstParamNames = GenerateCache(reader, entityInfo);
                T ret = (T)entityInfo.CreateSelectProxyInstance();//实例化对象
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
        /// 从Reader里边读取数据集合(快速)
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
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
        /// 把Reader的值读到对象
        /// </summary>
        /// <param name="reader">reader</param>
        /// <param name="lstParams">方法对应列表</param>
        /// <param name="obj">对象</param>
        internal static void FillObjectFromReader(IDataReader reader, List<EntityPropertyInfo> lstParams, object obj,DBInfo db) 
        {
            IDBAdapter dbAdapter = db.CurrentDbAdapter;
            for (int i = 0; i < lstParams.Count; i++)//把Reader的值赋到对象中
            {
                if (!reader.IsDBNull(i) && lstParams[i]!=null)
                {
                    dbAdapter.SetObjectValueFromReader(reader, i, obj, lstParams[i], !lstParams[i].TypeEqual(reader, i));
                }
            }
        }

        /// <summary>
        /// 从Reader里边读取数据集合(快速,返回集合的大小)
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="reader">reader</param>
        /// <param name="entityInfo">实体类信息</param>
        /// <param name="totalSize">集合总大小</param>
        /// <returns></returns>
        public static List<T> LoadFormReaderList<T>(IDataReader reader,EntityInfoHandle entityInfo, out int totalSize) where T : EntityBase, new()
        {
            List<T> retLst = new List<T>();
            totalSize = 0;//初始化当前记录集总大小
            if (reader != null && !reader.IsClosed)
            {
                List<EntityPropertyInfo> lstParamNames = GenerateCache(reader, entityInfo);
                
                while (reader.Read())
                {
                    T obj = (T)entityInfo.CreateSelectProxyInstance();
                    int curSize = 0;//获取当前值大小
                    FillObjectFromReader(reader, lstParamNames, obj, out curSize);
                    totalSize += curSize;//加到当前记录总大小里边
                    retLst.Add(obj);
                }
            }
            return retLst;
        }

        /// <summary>
        /// 把Reader的值读到对象
        /// </summary>
        /// <param name="reader">reader</param>
        /// <param name="lstParams">方法对应列表</param>
        /// <param name="obj">对象</param>
        /// <param name="itemSize">本条记录的大小</param>
        internal static void FillObjectFromReader(IDataReader reader, List<EntityPropertyInfo> lstParams, object obj, out int itemSize)
        {
            itemSize = 0;//初始化当前记录总大小
            for (int i = 0; i < lstParams.Count; i++)//把Reader的值赋到对象中
            {
                if (!reader.IsDBNull(i))
                {
                    object val = reader.GetValue(i);
                    int curSize=CurValueSize(val);//获取当前值大小
                    lstParams[i].SetValue(obj,val);
                    itemSize += curSize;//加到当前记录总大小里边
                }
            }
        }

        /// <summary>
        /// 获取此值占用的空间
        /// </summary>
        /// <param name="value">值</param>
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
            if (DefaultType.EqualType(type , DefaultType.StringType)) //计算字符串的空间
            {
                int size = (value.ToString().Length+1) * Marshal.SizeOf(typeof(char));
                return size;
            }
            if (DefaultType.EqualType(type , DefaultType.BytesType)) //计算字节数组的空间
            {
                int size = ((byte[])value).Length * Marshal.SizeOf(typeof(byte));
                return size;
            }
            return 0;
        }

        
    }
}
