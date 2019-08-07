using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.ComponentModel;
using System.Reflection;

using System.Web.UI.WebControls;
using Buffalo.Kernel.FastReflection;
using Buffalo.DB.PropertyAttributes;
using Buffalo.Kernel.UpdateModelUnit;
namespace Buffalo.WebKernel.WebCommons.ContorlCommons
{
    /// <summary>
    /// ��ȡ�ؼ���Ĭ�����Ե���
    /// </summary>
    public class ControlDefaultValue
    {
        private static Dictionary<string, ContorlDefaultPropertyInfo> dicContorlDefault = new Dictionary<string, ContorlDefaultPropertyInfo>();//�ؼ���Ӧ�Ľӿڵ�ӳ��
        private static Dictionary<string, ContorlDefaultPropertyInfo> dicContorlBinder = new Dictionary<string, ContorlDefaultPropertyInfo>();//�ؼ���Ӧ�Ľӿڵ�ӳ��
        /// <summary>
        /// ��ȡ�����Ĭ��������
        /// </summary>
        /// <param name="ctr">�ؼ�</param>
        /// <returns></returns>
        private static string GetDefaultPropertyName(Control ctr) 
        {
            string name = GetDefaultContorltPropertyName(ctr);
            if (name!=null) 
            {
                return name;
            }
            object att = FastInvoke.GetClassAttribute(ctr.GetType(), typeof(DefaultPropertyAttribute));
            if (att != null)
            {
                string cname = ((DefaultPropertyAttribute)att).Name;
                if (cname != "ID")
                {
                    return cname;
                }
            }
            att = FastInvoke.GetClassAttribute(ctr.GetType(), typeof(ValidationPropertyAttribute));
            if (att != null)
            {
                return ((ValidationPropertyAttribute)att).Name;
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
        private static ContorlDefaultPropertyInfo GetDefaultPropertyInfo(Control ctr) 
        {
            if (ctr == null) 
            {
                return null;
            }
            Type crlType = ctr.GetType();
            string key = crlType.FullName;
            ContorlDefaultPropertyInfo defaultInfo=null;
            if (!dicContorlDefault.TryGetValue(key, out defaultInfo)) 
            {
                defaultInfo = GetDefaultPropertyInfoWithoutCache(ctr);
                dicContorlDefault.Add(key, defaultInfo);
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
                    if (pinf.Name == strProName)
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
            else if(ctr is ITextControl)
            {
                return "Text";
            }
            else if (ctr is ICheckBoxControl) 
            {
                return "Checked";
            }
            return null;
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
            if (proInfo == null)
            {
                return ctr.Page.Request[ctr.UniqueID];
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

        /// <summary>
        /// ���ؼ�������
        /// </summary>
        /// <param name="ctr">Ҫ�󶨵Ŀؼ�</param>
        /// <param name="bindData">Ҫ�󶨵�ֵ</param>
        public static void ContorlBind(Control ctr, object bindData) 
        {
            //ContorlDefaultPropertyInfo info = GetBinderInfo(ctr);
            //if (info != null)
            //{
            //    info.PropertyHandle.SetValue(ctr, bindData);
            //}
            
        }

        /// <summary>
        /// ��ȡ�ÿؼ���Ĭ�ϰ�������
        /// </summary>
        /// <param name="ctr">�ؼ�</param>
        /// <returns></returns>
        private static string GetContorlBinderName(Control ctr)
        {
            Type type=ctr.GetType();
            PropertyInfo[] destproper = type.GetProperties();
            //int index = 0;
            ///��ȡ���Ա���
            foreach (PropertyInfo pinf in destproper)
            {
                object att = FastInvoke.GetPropertyAttribute(pinf, typeof(BindableAttribute));
                if (att != null)
                {
                    if (((BindableAttribute)att).Bindable) 
                    {
                        return pinf.Name;
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// ��ȡ�ؼ���Ĭ�����Ե���Ϣ
        /// </summary>
        /// <param name="ctr"></param>
        /// <returns></returns>
        private static ContorlDefaultPropertyInfo GetBinderInfo(Control ctr)
        {
            if (ctr == null)
            {
                return null;
            }
            Type crlType = ctr.GetType();
            string key = crlType.FullName;
            ContorlDefaultPropertyInfo defaultInfo = null;
            if (!dicContorlBinder.TryGetValue(key, out defaultInfo))
            {
                defaultInfo = new ContorlDefaultPropertyInfo();
                string strProName = GetContorlBinderName(ctr);
                if (strProName == null)
                {
                    return null;
                }
                PropertyInfo[] destproper = crlType.GetProperties();
                ///��ȡ���Ա���
                foreach (PropertyInfo pinf in destproper)
                {
                    if (pinf.Name == strProName)
                    {
                        defaultInfo.PropertyType = pinf.PropertyType;
                        defaultInfo.PropertyHandle = FastValueGetSet.GetPropertyInfoHandleWithOutCache(strProName, crlType);
                        defaultInfo.DefaultValue = GetDefaultPropertyValue(pinf);
                    }
                }
                dicContorlBinder.Add(key, defaultInfo);
            }
            return defaultInfo;
        }
       
    }
}
