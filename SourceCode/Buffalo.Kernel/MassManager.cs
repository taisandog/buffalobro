using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using System.Collections.Concurrent;

namespace Buffalo.Kernel
{
    /// <summary>
    /// ����������
    /// </summary>
    public class MassManager
    {
        private static ConcurrentDictionary<string, MassInfo> _dicMass = new ConcurrentDictionary<string, MassInfo>();
        /// <summary>
        /// �������ͻ�ȡ��������
        /// </summary>
        /// <param name="objType"></param>
        /// <returns></returns>
        public static MassInfo GetMassInfos(Type objType) 
        {
            string fullName = objType.FullName;
            MassInfo ret = null;
            if (!_dicMass.TryGetValue(fullName, out ret)) 
            {
                ret = new MassInfo();
                GetInfos(objType, ret.LstInfo,ret.DicInfos);
                _dicMass[fullName] = ret;
            }
            return ret;
        }

        /// <summary>
        /// �������ͻ�ȡ��������
        /// </summary>
        /// <param name="objType"></param>
        /// <returns></returns>
        public static List<EnumInfo> GetInfos(Type objType)
        {
            MassInfo info = GetMassInfos(objType);
            if (info != null) 
            {
                return info.LstInfo;
            }
            return null;
        }

        /// <summary>
        /// ͨ���ֶ�����ȡ�䳣����Ϣ
        /// </summary>
        /// <param name="objType"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static EnumInfo GetInfoByName(Type objType, string fieldName)
        {
            Dictionary<string, EnumInfo> dicInfos = GetMassInfos(objType).DicInfos;
            EnumInfo ret = null;
            string key = "Name:" + fieldName;
            if (dicInfos.TryGetValue(key, out ret)) 
            {
                return ret;
            }
            return null;
        }

        /// <summary>
        /// ͨ��ֵ��ȡ�䳣����Ϣ
        /// </summary>
        /// <param name="objType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static EnumInfo GetInfoByValue(Type objType, object value) 
        {
            Dictionary<string, EnumInfo> dicInfos = GetMassInfos(objType).DicInfos;
            EnumInfo ret = null;
            string key = "Value:" + value.ToString();
            if (dicInfos.TryGetValue(key, out ret))
            {
                return ret;
            }
            return null;
        }

        /// <summary>
        /// ��ȡ����ĳ�����Ϣ
        /// </summary>
        /// <param name="objType"></param>
        /// <returns></returns>
        private static void GetInfos(Type objType, List<EnumInfo> lstInfos, Dictionary<string, EnumInfo> dicInfo) 
        {
            FieldInfo[] fields = objType.GetFields(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
            foreach (FieldInfo field in fields)
            {
                EnumInfo info = new EnumInfo();
                object[] objs = field.GetCustomAttributes(false);

                foreach (object obj in objs) 
                {
                    DescriptionAttribute da = obj as DescriptionAttribute;
                    if (da != null) 
                    {
                        info.Description = da.Description;
                        continue;
                    }
                    DisplayNameAttribute dn = obj as DisplayNameAttribute;
                    if (da != null)
                    {
                        info.DisplayName = dn.DisplayName;
                        continue;
                    }
                    Attribute att = obj as Attribute;
                    if (att == null) 
                    {
                        continue;
                    }
                    info.CustomerAttributes.Add(att);
                }

                info.FieldName = field.Name;
                if (objType.IsEnum)
                {
                    info.Value = Enum.Parse(objType, field.Name);

                }
                else
                {
                    info.Value = field.GetValue(null);
                }
                lstInfos.Add(info);
                dicInfo["Value:"+info.Value.ToString()] = info;
                dicInfo["Name:" + info.FieldName.ToString()] = info;
            }
        }
    }
}
