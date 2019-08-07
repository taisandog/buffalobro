using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Reflection;
using Buffalo.Kernel.FastReflection;
using Buffalo.Kernel.UpdateModelUnit;
using System.Windows.Forms;
namespace Buffalo.Win32Kernel.UpdateModelUnit
{
    /// <summary>
    /// ��ȡ�ؼ���Ĭ�����Ե���
    /// </summary>
    public class ControlDefaultValue
    {
        
        private static Dictionary<string, ContorlDefaultPropertyInfo> _dicContorlBinder = new Dictionary<string, ContorlDefaultPropertyInfo>();//�ؼ���Ӧ�Ľӿڵ�ӳ��

        public static void AddDefault() 
        {

        }

        /// <summary>
        /// ��ȡ�����Ĭ��������
        /// </summary>
        /// <param name="ctr">�ؼ�</param>
        /// <returns></returns>
        private static string GetDefaultPropertyName(Control ctr) 
        {
            string cname = null;
            cname = GetDefaultContorltPropertyName(ctr);
            if (!string.IsNullOrEmpty(cname) && cname != "ID")
            {
                return cname;
            }
            object att = null;
            att = FastInvoke.GetClassAttribute(ctr.GetType(), typeof(DefaultPropertyAttribute));
            if (att != null)
            {
                cname = ((DefaultPropertyAttribute)att).Name;
                if (cname != "ID")
                {
                    return cname;
                }
            }

            att = FastInvoke.GetClassAttribute(ctr.GetType(), typeof(DefaultBindingPropertyAttribute));
            if (att != null)
            {
                cname = ((DefaultBindingPropertyAttribute)att).Name;
                if (cname != "ID")
                {
                    return cname;
                }
            }
            
            return null;
        }
        /// <summary>
        /// ��ȡϵͳԤ��Ŀؼ����͵�Ĭ��������
        /// </summary>
        /// <param name="ctr">�ؼ�</param>
        /// <returns></returns>
        private static string GetDefaultContorltPropertyName(Control ctr)
        {
            if (ctr is ListControl)
            {
                return "SelectedValue";
            }
            return null;
        }
        /// <summary>
        /// ��ȡ�����Ե�Ĭ��ֵ
        /// </summary>
        /// <param name="pinf">�ֶ���Ϣ</param>
        /// <returns></returns>
        private static object GetDefaultPropertyValue(PropertyInfo pinf)
        {
            object att = FastInvoke.GetPropertyAttribute(pinf, typeof(DefaultValueAttribute));
            if (att != null)
            {
                return ((DefaultValueAttribute)att).Value;
            }
            return null;
        }

        /// <summary>
        /// ��ȡ�ؼ���Ĭ�����Ե���Ϣ
        /// </summary>
        /// <param name="ctr"></param>
        /// <returns></returns>
        internal static ContorlDefaultPropertyInfo GetDefaultPropertyInfo(Control ctr) 
        {
            if (ctr == null) 
            {
                return null;
            }
            Type crlType = ctr.GetType();
            string key = crlType.FullName;
            ContorlDefaultPropertyInfo defaultInfo=null;
            if (!_dicContorlBinder.TryGetValue(key, out defaultInfo)) 
            {
                defaultInfo = GetDefaultPropertyInfoWithoutCache(ctr);
                _dicContorlBinder.Add(key, defaultInfo);
            }
            return defaultInfo;
        }

        /// <summary>
        /// �޻���Ļ�ȡ�ؼ�Ĭ��ֵ
        /// </summary>
        /// <param name="ctr"></param>
        /// <returns></returns>
        internal static ContorlDefaultPropertyInfo GetDefaultPropertyInfoWithoutCache(Control ctr) 
        {
            Type crlType = ctr.GetType();
            string strProName = GetDefaultPropertyName(ctr);
            ContorlDefaultPropertyInfo defaultInfo = null;
            if (strProName != null)
            {
                defaultInfo = new ContorlDefaultPropertyInfo();
                PropertyInfo[] destproper = crlType.GetProperties();
                ///��ȡ���Ա���
                foreach (PropertyInfo pinf in destproper)
                {
                    if (pinf.Name.Equals( strProName,StringComparison.CurrentCultureIgnoreCase))
                    {
                        defaultInfo.PropertyType = pinf.PropertyType;
                        defaultInfo.PropertyHandle = FastValueGetSet.GetPropertyInfoHandleWithOutCache(strProName, crlType);
                        defaultInfo.DefaultValue = GetDefaultPropertyValue(pinf);
                    }
                }
            }
            return defaultInfo;
        }

        /// <summary>
        /// ���ÿؼ���Ĭ��ֵ
        /// </summary>
        /// <param name="ctr">�ؼ�</param>
        public static void ResetContorlValue(Control ctr) 
        {
            ContorlDefaultPropertyInfo proInfo = GetDefaultPropertyInfo(ctr);
            if (proInfo.DefaultValue != null)
            {
                proInfo.PropertyHandle.SetValue(ctr, proInfo.DefaultValue);
            }
        }

        

        /// <summary>
        /// ��ȡ�ÿؼ���Ĭ������
        /// </summary>
        /// <param name="ctr">�ؼ�</param>
        /// <returns></returns>
        public static object GetControlDefaultPropertyValue(Control ctr) 
        {
            ContorlDefaultPropertyInfo proInfo = GetDefaultPropertyInfo(ctr);

            return GetControlDefaultPropertyValue(ctr,proInfo);
        }

        /// <summary>
        /// ��ȡ�ÿؼ���Ĭ������
        /// </summary>
        /// <param name="ctr">�ؼ�</param>
        /// <returns></returns>
        public static object GetControlDefaultPropertyValue(Control ctr, ContorlDefaultPropertyInfo proInfo)
        {
            //ContorlDefaultPropertyInfo proInfo = GetDefaultPropertyInfo(ctr);
            if (proInfo == null || !proInfo.PropertyHandle.HasGetHandle)
            {
                return null;
            }
            return proInfo.PropertyHandle.GetValue(ctr);
        }

        /// <summary>
        /// ���øÿؼ���Ĭ������
        /// </summary>
        /// <param name="ctr">�ؼ�</param>
        /// <param name="value">����ֵ</param>
        public static void SetControlDefaultPropertyValue(Control ctr, object value) 
        {
            ContorlDefaultPropertyInfo proInfo = GetDefaultPropertyInfo(ctr);
            if (proInfo != null)
            {
                object newValue = Convert.ChangeType(value, proInfo.PropertyType);
                proInfo.PropertyHandle.SetValue(ctr, newValue);
            }
        }

       
    }
}
