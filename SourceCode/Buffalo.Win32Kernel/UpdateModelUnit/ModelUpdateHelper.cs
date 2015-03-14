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
    /// 实体填充帮助类
    /// </summary>
    public class ModelUpdateHelper
    {

         /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="container">容器</param>
        /// <param name="model">实体</param>
        /// <param name="type">命名类型</param>
        public static void UpdateModel(Control container, object model)
        {
            UpdateModel(container, model, PrefixType.Camel);
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="container">容器</param>
        /// <param name="model">实体</param>
        /// <param name="type">命名类型</param>
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
                if (handle == null || !handle.HasSetHandle)//搜索里层
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
        /// 给模型赋值
        /// </summary>
        /// <param name="ctr">控件</param>
        /// <param name="model">模型</param>
        /// <param name="handle">属性信息</param>
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
        /// 实体信息填充到表单
        /// </summary>
        /// <param name="container">容器</param>
        /// <param name="model">实体</param>
        public static void FillForm(Control container, object model)
        {
            FillForm(container, model, PrefixType.Camel);
        }
        /// <summary>
        /// 实体信息填充到表单
        /// </summary>
        /// <param name="container">容器</param>
        /// <param name="model">实体</param>
        /// <param name="type">命名类型</param>
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
                if (handle == null || !handle.HasGetHandle)//搜索里层
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


//*************用法******************

//*****实体信息填充到表单********
//SampleInfo info = new SampleInfo();
//info.Id = 20;
//info.ProjectName = "工程X";
//info.SamplingUser = "5";
//info.SamplingTime = DateTime.Now.AddDays(-20);
//info.PrintState = true;
//info.Sampledescribe = "ertgfdfds\nedfdsfdsf";
//ModelUpdateHelper.FillForm(panel1, info);

//******表单信息填充到实体*******
//SampleInfo info = new SampleInfo();
//ModelUpdateHelper.UpdateModel(panel1, info);