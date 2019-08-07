using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using Buffalo.Kernel.FastReflection;
using Commons;
using Buffalo.Kernel.FastReflection.ClassInfos;
using Buffalo.WebKernel.WebCommons.ContorlCommons;
using Buffalo.Kernel.UpdateModelUnit;
using Buffalo.Kernel;

namespace Buffalo.WebKernel.WebModels
{
    
    internal class WebModel
    {
        //private static Dictionary<string, Dictionary<string, ModelInfo>> dicCache = new Dictionary<string, Dictionary<string, ModelInfo>>();

        /// <summary>
        /// ���ʵ���ҳ��Ķ�Ӧ��
        /// </summary>
        /// <param name="objPage">ҳ����</param>
        /// <param name="modleType">ʵ������</param>
        private static Dictionary<string, ModelInfo> GetPageMapInfo(Control objControl, Type modleType, PrefixType pType) 
        {
            Dictionary<string, ModelInfo> handleMapping = new Dictionary<string, ModelInfo>();
            ClassInfoHandle handle = ClassInfoManager.GetClassHandle(modleType);
            List<Control> lstCtrs = GetAllContorl(objControl);
            foreach (Control ctr in lstCtrs)
            {
                string id = UpdateModelInfo.GetKey(ctr.ID, pType);
                if (!string.IsNullOrEmpty(id))
                {
                    ModelInfo info = new ModelInfo();
                    PropertyInfoHandle pInfo = handle.PropertyInfo[id];
                    if (pInfo != null)
                    {
                        info.Phandle = pInfo;
                        ContorlDefaultPropertyInfo ctrInfo = ControlDefaultValue.GetDefaultPropertyInfoWithoutCache(ctr);
                        info.CtrHandle = ctrInfo;
                        info.Ctr = ctr;
                        handleMapping[ctr.ID] = info;
                    }
                }
            }
            return handleMapping;
        }

        
        /// <summary>
        /// ��ȡҳ�����пռ���Ϣ
        /// </summary>
        /// <returns></returns>
        private static List<Control> GetAllContorl(Control objControl)
        {
            List<Control> lst = new List<Control>();
            foreach (Control ctr in objControl.Controls)
            {
                FillContorl(lst, ctr);
                lst.Add(ctr);
            }
            return lst;
        }

        /// <summary>
        /// �ݹ��ȡ�ؼ���Ϣ
        /// </summary>
        /// <param name="lst"></param>
        /// <param name="curContorl"></param>
        private static void FillContorl(List<Control> lst, Control curContorl)
        {
            foreach (Control ctr in curContorl.Controls)
            {
                lst.Add(ctr);
                FillContorl(lst, ctr);
            }
        }

        /// <summary>
        /// ����ҳ����Ϣ��ʵ��
        /// </summary>
        /// <param name="objPage">ҳ��</param>
        /// <param name="modle">ʵ��</param>
        public static void UpdateModel(Control objControl, object modle, PrefixType pType) 
        {
            if (modle == null) 
            {
                throw new Exception("������Ϊ��");
            }
            Dictionary<string, ModelInfo> dicInfo = GetPageMapInfo(objControl, modle.GetType(), pType);
            Dictionary<string, ModelInfo>.Enumerator enumer = dicInfo.GetEnumerator();
            while (enumer.MoveNext()) 
            {
                string key = enumer.Current.Key;
                ModelInfo info = enumer.Current.Value;
                
                object value = ControlDefaultValue.GetControlDefaultPropertyValue(info.Ctr, info.CtrHandle);
                if (value!=null) 
                {
                    object ovalue = CommonMethods.EntityProChangeType(value, info.Phandle.PropertyType);
                    info.Phandle.SetValue(modle, ovalue);
                }
            }
        }
    }
}
