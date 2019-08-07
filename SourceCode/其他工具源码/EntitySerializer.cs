using System;
using System.Collections.Generic;
using Buffalo.Kernel.FastReflection;
using System.Collections;
using Buffalo.Kernel.FastReflection.ClassInfos;
using System.Data;

namespace Buffalo.Kernel
{
    /// <summary>
    /// ��ʽ�����͵ķ���
    /// </summary>
    /// <param name="key">Dictionary��</param>
    /// <param name="propertyName">������(�磺User.Name)</param>
    /// <param name="entity">ʵ��</param>
    /// <param name="value">ֵ</param>
    /// <param name="newValue">��ֵ</param>
    /// <returns>�Ƿ��ʽ���ɹ�(ʧ�ܵĻ��Ͳ���ֵ)</returns>
    public delegate object DelFormatValue(string key, string propertyName, object entity, object value);

    /// <summary>
    /// ʵ�����л���
    /// </summary>
    public class EntitySerializer
    {
        /// <summary>
        /// �Ѽ�����Ϣ���ʵ�弯��
        /// </summary>
        /// <typeparam name="T">ʵ��</typeparam>
        /// <param name="dic">��Ϣ����</param>
        /// <param name="propertyCollection">��Ҫ��Ӧ���ֵ��ֵ���磺new string[]{"user=User.Name","uid=UserId"}��</param>
        /// <returns></returns>
        public static List<T> DeserializeToList<T>(List<Dictionary<string, object>> lstDic, IEnumerable<string> propertyCollection, DelFormatValue formatValue) 
            where T:new()
        {
            List<T> lstRet=new List<T>();
            if (lstDic == null) 
            {
                return lstRet;
            }

            Type objType = typeof(T);

            List<EntitySerializerInfo> lstInfo = null;
            if (propertyCollection == null)
            {
                lstInfo = GetTypeInfos(objType);
            }
            else
            {
                lstInfo = GetDicInfos(objType, propertyCollection);
            }

            object value = null;
            foreach (Dictionary<string, object> dic in lstDic)
            {
                T entity = (T)Activator.CreateInstance(objType);

                foreach (EntitySerializerInfo info in lstInfo)
                {
                    if (!dic.TryGetValue(info.Name, out value)) 
                    {
                        continue;
                    }
                    value = SetValue(info, entity, value);
                    if (formatValue != null)
                    {
                        value = formatValue(info.Name, info.PropertyName, entity, value);
                    }
                }
                lstRet.Add(entity);
            }
            return lstRet;
        }
        /// <summary>
        /// �Ѽ�����Ϣ���ʵ�弯��
        /// </summary>
        /// <typeparam name="T">ʵ��</typeparam>
        /// <param name="dic">��Ϣ����</param>
        /// <param name="propertyCollection">��Ҫ��Ӧ���ֵ��ֵ���磺new string[]{"user=User.Name","uid=UserId"}��</param>
        /// <returns></returns>
        public static List<T> DeserializeToList<T>(List<Dictionary<string, object>> dic, IEnumerable<string> propertyCollection)
            where T : new()
        {
            return DeserializeToList<T>(dic, propertyCollection, DefaultFormatValue);
        }
        /// <summary>
        /// �Ѽ�����Ϣ���ʵ�弯��
        /// </summary>
        /// <typeparam name="T">ʵ��</typeparam>
        /// <param name="dic">��Ϣ����</param>
        /// <param name="propertyCollection">��Ҫ��Ӧ���ֵ��ֵ���磺new string[]{"user=User.Name","uid=UserId"}��</param>
        /// <returns></returns>
        public static void DeserializeToObject(object obj, Dictionary<string, object> dic, IEnumerable<string> propertyCollection)
        {
            DeserializeToObject(obj, dic, propertyCollection, DefaultFormatValue);
        }
        /// <summary>
        /// �Ѽ�����Ϣ���ʵ�弯��
        /// </summary>
        /// <typeparam name="T">ʵ��</typeparam>
        /// <param name="dic">��Ϣ����</param>
        /// <param name="propertyCollection">��Ҫ��Ӧ���ֵ��ֵ���磺new string[]{"user=User.Name","uid=UserId"}��</param>
        /// <returns></returns>
        public static void DeserializeToObject(object obj, Dictionary<string, object> dic, IEnumerable<string> propertyCollection, DelFormatValue formatValue)
        {
            Type objType = obj.GetType();
            List<EntitySerializerInfo> lstInfo = null;
            if (propertyCollection == null)
            {
                lstInfo = GetTypeInfos(objType);
            }
            else
            {
                lstInfo = GetDicInfos(objType, propertyCollection);
            }

            object value = null;
            Dictionary<string, object> item = new Dictionary<string, object>();
            foreach (EntitySerializerInfo info in lstInfo)
            {
                if (!dic.TryGetValue(info.Name, out value))
                {
                    continue;
                }
                value = SetValue(info, obj, value);
                if (formatValue != null)
                {
                    value = formatValue(info.Name, info.PropertyName, obj, value);
                }
                item[info.Name] = value;
            }
        }

        /// <summary>
        /// �Ѽ�����Ϣ���ʵ�弯��
        /// </summary>
        /// <typeparam name="T">ʵ��</typeparam>
        /// <param name="dic">��Ϣ����</param>
        /// <param name="propertyCollection">��Ҫ��Ӧ���ֵ��ֵ���磺new string[]{"user=User.Name","uid=UserId"}��</param>
        /// <returns></returns>
        public static T DeserializeToObject<T>(Dictionary<string, object> dic, IEnumerable<string> propertyCollection, DelFormatValue formatValue)
            where T : new()
        {
            
            if (dic == null)
            {
                return default(T);
            }

            Type objType = typeof(T);
            T entity = (T)Activator.CreateInstance(objType);
            DeserializeToObject(entity, dic, propertyCollection, formatValue);
           
            return entity;
        }
        /// <summary>
        /// �Ѽ�����Ϣ���ʵ�弯��
        /// </summary>
        /// <typeparam name="T">ʵ��</typeparam>
        /// <param name="dic">��Ϣ����</param>
        /// <param name="propertyCollection">��Ҫ��Ӧ���ֵ��ֵ���磺new string[]{"user=User.Name","uid=UserId"}��</param>
        /// <returns></returns>
        public static T DeserializeToObject<T>(Dictionary<string, object> dic, IEnumerable<string> propertyCollection)
            where T : new()
        {
            return DeserializeToObject<T>(dic, propertyCollection, DefaultFormatValue);
        }
        /// <summary>
        /// ���ݷ�����Ϣ��ȡֵ(��ʽ)
        /// </summary>
        /// <param name="info"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        private static bool SetValue(EntitySerializerInfo info, object obj,object value)
        {
            object tmpObj = obj;
            int hcount=info.PropertyInfos.Count;
            if (hcount > 1)
            {
                for (int i = 0; i < hcount - 1; i++)
                {
                    PropertyInfoHandle pHandle = info.PropertyInfos[i];
                    tmpObj = pHandle.GetValue(value);
                    if (tmpObj == null)
                    {
                        return false;
                    }
                }
            }
            PropertyInfoHandle sethandle = info.PropertyInfos[hcount - 1];
            sethandle.SetValue(tmpObj, value);
            return true;
        }

        /// <summary>
        /// ֵ����ת��Ϊ�ֵ伯��(�ʺ�JavaScriptSerializer)
        /// </summary>
        /// <param name="lstEntity">ʵ��</param>
        /// <param name="propertyMap">Key������</param>
        /// <param name="formatValue">��ʽ��ֵ�ķ���</param>
        /// <returns></returns>
        public static List<Dictionary<string, object>> SerializeListToDictionary(IList lstValue,
            string propertyMap, DelFormatValue formatValue)
        {
            if (lstValue.Count == 0)
            {
                return new List<Dictionary<string, object>>();
            }
            List<Dictionary<string, object>> lstDic = new List<Dictionary<string, object>>(lstValue.Count);
            object value = null;

            foreach (object obj in lstValue)
            {
                Dictionary<string, object> item = new Dictionary<string, object>();
                value =obj;
                if (formatValue != null)
                {
                    value = formatValue(propertyMap, "value", value, value);
                }
                item[propertyMap] = value;
                lstDic.Add(item);
            }
            return lstDic;
        }
         /// <summary>
        /// ֵ����ת��Ϊ�ֵ伯��(�ʺ�JavaScriptSerializer)
        /// </summary>
        /// <param name="lstEntity">ʵ��</param>
        /// <param name="propertyMap">Key������</param>
        /// <returns></returns>
        public static List<Dictionary<string, object>> SerializeListToDictionary(IList lstValue,
            string propertyMap)
        {
            return SerializeListToDictionary(lstValue, propertyMap, DefaultFormatValue);
        }
        /// <summary>
        /// ʵ��ת��Ϊ�ֵ伯��(�ʺ�JavaScriptSerializer)
        /// </summary>
        /// <param name="lstEntity">ʵ��</param>
        /// <param name="propertyCollection">��Ҫ��Ӧ���ֵ��ֵ���磺new string[]{"user=User.Name","uid=UserId"}��</param>
        /// <param name="formatValue">��ʽ��ֵ�ķ���</param>
        /// <returns></returns>
        public static List<Dictionary<string, object>> SerializeListToDictionary(IList lstEntity,
            IEnumerable<string> propertyCollection,DelFormatValue formatValue)
        {
            if (lstEntity.Count == 0)
            {
                return new List<Dictionary<string, object>>();
            }
            List<Dictionary<string, object>> lstDic = new List<Dictionary<string, object>>(lstEntity.Count);
            Type objType = lstEntity[0].GetType();
            List<EntitySerializerInfo> lstInfo = null;
            if (propertyCollection == null)
            {
                lstInfo = GetTypeInfos(objType);
            }
            else
            {
                lstInfo = GetDicInfos(objType, propertyCollection);
            }
            object value = null;

            foreach (object obj in lstEntity)
            {
                Dictionary<string, object> item = new Dictionary<string, object>();
                foreach (EntitySerializerInfo info in lstInfo)
                {
                    value=GetValue(info, obj);
                    if(formatValue!=null)
                    {
                        value = formatValue(info.Name, info.PropertyName, obj, value);
                    }
                    item[info.Name] = value;
                }
                lstDic.Add(item);
            }
            return lstDic;

        }
        /// <summary>
        /// ʵ��ת��ΪDataTable
        /// </summary>
        /// <param name="lstEntity">ʵ��</param>
        /// <param name="propertyCollection">��Ҫ��Ӧ���ֵ��ֵ���磺new string[]{"user=User.Name","uid=UserId"}��</param>
        /// <param name="formatValue">��ʽ��ֵ�ķ���</param>
        /// <returns></returns>
        public static DataTable SerializeListToDataTable(IList lstEntity,
            IEnumerable<string> propertyCollection, DelFormatValue formatValue)
        {
            if (lstEntity.Count == 0)
            {
                return new DataTable();
            }
            DataTable dt = new DataTable("DS");
            Type objType = lstEntity[0].GetType();
            List<EntitySerializerInfo> lstInfo = null;
            if (propertyCollection == null)
            {
                lstInfo = GetTypeInfos(objType);
            }
            else
            {
                lstInfo = GetDicInfos(objType, propertyCollection);
            }

            foreach (EntitySerializerInfo info in lstInfo)
            {
                DataColumn dc = new DataColumn(info.Name, info.PropertyInfos[info.PropertyInfos.Count - 1].PropertyType);
                dt.Columns.Add(dc);
            }
            object value = null;

            dt.BeginLoadData();
            foreach (object obj in lstEntity)
            {
                DataRow dr = dt.NewRow();
                foreach (EntitySerializerInfo info in lstInfo)
                {
                    value = GetValue(info, obj);
                    if (formatValue != null)
                    {
                        value = formatValue(info.Name, info.PropertyName, obj, value);
                    }
                    dr[info.Name] = value;
                }
                dt.Rows.Add(dr);
            }
            dt.EndLoadData();
            return dt;

        }
        /// <summary>
        /// ʵ��ת��Ϊ�ֵ伯��
        /// </summary>
        /// <param name="lstEntity">ʵ��</param>
        /// <param name="propertyCollection">��Ҫ��Ӧ���ֵ��ֵ���磺new string[]{"user=User.Name","uid=UserId"}��</param>
        /// <param name="formatValue">��ʽ��ֵ�ķ���</param>
        /// <returns></returns>
        public static DataTable SerializeListToDataTable(IList lstEntity,
            IEnumerable<string> propertyCollection)
        {
            return SerializeListToDataTable(lstEntity, propertyCollection, null);
        }
        /// <summary>
        /// ʵ��ת��Ϊ�ֵ伯��(�ʺ�JavaScriptSerializer)
        /// </summary>
        /// <param name="lstEntity">ʵ��</param>
        /// <param name="propertyCollection">��Ҫ��Ӧ���ֵ��ֵ���磺new string[]{"user=User.Name","uid=UserId"}��</param>
        /// <param name="formatValue">��ʽ��ֵ�ķ���</param>
        /// <returns></returns>
        public static Dictionary<string, object> SerializeToDictionary(object entity,
            IEnumerable<string> propertyCollection)
        {
            return SerializeToDictionary(entity, propertyCollection, DefaultFormatValue);
        }

        /// <summary>
        /// ʵ��ת��Ϊ�ֵ伯��(�ʺ�JavaScriptSerializer)
        /// </summary>
        /// <param name="lstEntity">ʵ��</param>
        /// <param name="propertyCollection">��Ҫ��Ӧ���ֵ��ֵ���磺new string[]{"user=User.Name","uid=UserId"}��</param>
        /// <param name="formatValue">��ʽ��ֵ�ķ���</param>
        /// <returns></returns>
        public static Dictionary<string, object> SerializeToDictionary(object entity,
            IEnumerable<string> propertyCollection, DelFormatValue formatValue)
        {
            if (entity == null)
            {
                return new Dictionary<string, object>();
            }

            Type objType = entity.GetType();
            List<EntitySerializerInfo> lstInfo =null;
            if (propertyCollection == null)
            {
                lstInfo = GetTypeInfos(objType);
            }
            else 
            {
                lstInfo = GetDicInfos(objType, propertyCollection);
            }
            object value = null;


            Dictionary<string, object> item = new Dictionary<string, object>();
            foreach (EntitySerializerInfo info in lstInfo)
            {
                value = GetValue(info, entity);
                if (formatValue != null)
                {
                    value = formatValue(info.Name, info.PropertyName, entity, value);
                }
                item[info.Name] = value;
            }
            return item;
        }

        /// <summary>
        /// ʵ��ת��Ϊ�ֵ伯��(�ʺ�JavaScriptSerializer)
        /// </summary>
        /// <param name="lstEntity">ʵ��</param>
        /// <param name="propertyCollection">��Ҫ��Ӧ���ֵ��ֵ���磺new string[]{"user=User.Name","uid=UserId"}��</param>
        /// <returns></returns>
        public static List<Dictionary<string, object>> SerializeListToDictionary(IList lstEntity,
            IEnumerable<string> propertyCollection)
        {
            return SerializeListToDictionary(lstEntity, propertyCollection, DefaultFormatValue);
        }
        /// <summary>
        /// Ĭ�ϵ�ֵ��ʽ������
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object DefaultFormatValue(string key, string propertyName, object entity, object value)
        {

            if (value == null)
            {

                return null;
            }
            Type objType = value.GetType();
            if (objType.IsEnum)
            {
                return (int)value;
            }
            else if (value is bool) 
            {
                bool bvalue = (bool)value;
                return bvalue ? 1 : 0;
            }
            else if (value is bool?)
            {
                bool? bvalue = (bool?)value;
                return bvalue.GetValueOrDefault() ? 1 : 0;
            }
            //else if (value is DateTime)
            //{
            //    return ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss.ms");
            //}

            return value;

        }


        /// <summary>
        /// ���ݷ�����Ϣ��ȡֵ(��ʽ)
        /// </summary>
        /// <param name="info"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        private static object GetValue(EntitySerializerInfo info, object obj)
        {
            object value = obj;
            foreach (PropertyInfoHandle pHandle in info.PropertyInfos)
            {
                value = pHandle.GetValue(value);
                if (value == null) 
                {
                    return null;
                }
            }
            return value;
        }
        /// <summary>
        /// ������Ҫ�����ݻ�ȡ������Ϣ
        /// </summary>
        /// <param name="objType"></param>
        /// <param name="propertyCollection"></param>
        /// <returns></returns>
        private static List<EntitySerializerInfo> GetDicInfos(Type objType, IEnumerable<string> propertyCollection)
        {
            List<EntitySerializerInfo> lstInfos = new List<EntitySerializerInfo>(12);
            foreach (string strItem in propertyCollection)
            {
                string[] itemPart = strItem.Split('=');
                if (itemPart.Length < 2)
                {
                    continue;
                }
                EntitySerializerInfo info = new EntitySerializerInfo();
                
                info.Name = itemPart[0];
                info.PropertyName = itemPart[1];
                string[] propertyItems = itemPart[1].Split('.');
                Type curType = objType;
                foreach (string proName in propertyItems)
                {
                    PropertyInfoHandle pinfo = FastValueGetSet.GetPropertyInfoHandle(proName, curType);
                    info.PropertyInfos.Add(pinfo);
                    curType = pinfo.PropertyType;
                }
                lstInfos.Add(info);
            }
            return lstInfos;
        }

        /// <summary>
        /// ��ȡ������������Ϣ
        /// </summary>
        /// <param name="objType">����</param>
        /// <returns></returns>
        private static List<EntitySerializerInfo> GetTypeInfos(Type objType) 
        {
            List<EntitySerializerInfo> lstInfos = new List<EntitySerializerInfo>(12);
            ClassInfoHandle classInfo=ClassInfoManager.GetClassHandle(objType);
            
            foreach (PropertyInfoHandle handle in classInfo.PropertyInfo)
            {
                EntitySerializerInfo info = new EntitySerializerInfo();
                info.PropertyInfos.Add(handle);
                info.Name = handle.PropertyName;
                info.PropertyName = handle.PropertyName;
                lstInfos.Add(info);
            }
            return lstInfos;
        }

    }



    /// <summary>
    /// ʵ�����л���Ϣ
    /// </summary>
    public class EntitySerializerInfo
    {
        private string _name;
        /// <summary>
        /// ����
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }
        private string _propertyName;
        /// <summary>
        /// ��������
        /// </summary>
        public string PropertyName
        {
            get
            {
                return _propertyName;
            }
            set
            {
                _propertyName = value;
            }
        }
        private List<PropertyInfoHandle> _propertyInfos = new List<PropertyInfoHandle>(4);
        /// <summary>
        /// ���������Է���
        /// </summary>
        public List<PropertyInfoHandle> PropertyInfos
        {
            get
            {
                return _propertyInfos;
            }

        }
    }
}