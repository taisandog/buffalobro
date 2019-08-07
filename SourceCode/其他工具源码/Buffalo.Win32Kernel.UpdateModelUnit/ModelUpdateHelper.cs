using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Buffalo.Kernel.UpdateModelUnit;
using System.ComponentModel;
using Buffalo.Kernel.FastReflection.ClassInfos;
using Buffalo.Kernel.FastReflection;
using Buffalo.Kernel.Defaults;
using Buffalo.Kernel;

namespace Buffalo.Win32Kernel.UpdateModelUnit
{
    /// <summary>
    /// ʵ����������
    /// </summary>
    public class ModelUpdateHelper
    {

         /// <summary>
        /// ����ʵ��
        /// </summary>
        /// <param name="container">����</param>
        /// <param name="model">ʵ��</param>
        /// <param name="type">��������</param>
        public static void UpdateModel(Control container, object model)
        {
            UpdateModel(container, model, PrefixType.Camel);
        }

        /// <summary>
        /// ����ʵ��
        /// </summary>
        /// <param name="container">����</param>
        /// <param name="model">ʵ��</param>
        /// <param name="type">��������</param>
        public static void UpdateModel(Control container,object model,PrefixType type) 
        {
            Control.ControlCollection ctrs = container.Controls;
            if (ctrs == null || ctrs.Count <= 0) 
            {
                return;
            }
            ClassInfoHandle classHandle = ClassInfoManager.GetClassHandle(model.GetType());
            foreach (Control ctr in ctrs) 
            {
                string proName = UpdateModelInfo.GetKey(ctr.Name, type);
                if (string.IsNullOrEmpty(proName)) 
                {
                    continue;
                }
                PropertyInfoHandle handle = classHandle.PropertyInfo[proName];
                if (handle == null || !handle.HasSetHandle)//�������
                {
                    UpdateModel(ctr, model, type);
                }
                else
                {
                    try
                    {
                        SetModelValue(ctr, model, handle);
                    }
                    catch { }
                }

            }
        }

        /// <summary>
        /// ��ģ�͸�ֵ
        /// </summary>
        /// <param name="ctr">�ؼ�</param>
        /// <param name="model">ģ��</param>
        /// <param name="handle">������Ϣ</param>
        private static void SetModelValue(Control ctr, object model, PropertyInfoHandle handle) 
        {
            object value = ControlDefaultValue.GetControlDefaultPropertyValue(ctr);
            if (value == null) 
            {
                return;
            }
            object readValue = CommonMethods.EntityProChangeType(value, handle.PropertyType);
            handle.SetValue(model, readValue);
            
        }
        /// <summary>
        /// ʵ����Ϣ��䵽��
        /// </summary>
        /// <param name="container">����</param>
        /// <param name="model">ʵ��</param>
        public static void FillForm(Control container, object model)
        {
            FillForm(container, model, PrefixType.Camel);
        }
        /// <summary>
        /// ʵ����Ϣ��䵽��
        /// </summary>
        /// <param name="container">����</param>
        /// <param name="model">ʵ��</param>
        /// <param name="type">��������</param>
        public static void FillForm(Control container, object model, PrefixType type)
        {
            Control.ControlCollection ctrs = container.Controls;
            if (ctrs == null || ctrs.Count <= 0)
            {
                return;
            }
            ClassInfoHandle classHandle = ClassInfoManager.GetClassHandle(model.GetType());
            foreach (Control ctr in ctrs)
            {
                string proName = UpdateModelInfo.GetKey(ctr.Name, type);
                if (string.IsNullOrEmpty(proName))
                {
                    continue;
                }
                PropertyInfoHandle handle = classHandle.PropertyInfo[proName];
                if (handle == null || !handle.HasGetHandle)//�������
                {
                    FillForm(ctr, model, type);
                }
                else
                {
                    try
                    {
                        object value=handle.GetValue(model);
                        if (value != null)
                        {
                            ControlDefaultValue.SetControlDefaultPropertyValue(ctr, value);
                        }
                    }
                    catch { }
                }

            }
        }

    }
}


//*************�÷�******************

//*****ʵ����Ϣ��䵽��********
//SampleInfo info = new SampleInfo();
//info.Id = 20;
//info.ProjectName = "����X";
//info.SamplingUser = "5";
//info.SamplingTime = DateTime.Now.AddDays(-20);
//info.PrintState = true;
//info.Sampledescribe = "ertgfdfds\nedfdsfdsf";
//ModelUpdateHelper.FillForm(panel1, info);

//******����Ϣ��䵽ʵ��*******
//SampleInfo info = new SampleInfo();
//ModelUpdateHelper.UpdateModel(panel1, info);