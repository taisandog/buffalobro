using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.Kernel.FastReflection;
using Buffalo.WebKernel.WebCommons.ContorlCommons;
using System.Web.UI;
using Buffalo.Kernel.UpdateModelUnit;

namespace Buffalo.WebKernel.WebModels
{
    /// <summary>
    /// 更新实体的属性信息
    /// </summary>
    public class ModelInfo
    {
        PropertyInfoHandle phandle;
        ContorlDefaultPropertyInfo ctrHandle;
        Control ctr;

        /// <summary>
        /// 对应的属性信息
        /// </summary>
        public PropertyInfoHandle Phandle 
        {
            get 
            {
                return phandle;
            }
            set 
            {
                phandle = value;
            }
        }

        /// <summary>
        /// 对应的控件信息
        /// </summary>
        public ContorlDefaultPropertyInfo CtrHandle
        {
            get
            {
                return ctrHandle;
            }
            set
            {
                ctrHandle = value;
            }
        }
        /// <summary>
        /// 对应的控件
        /// </summary>
        public Control Ctr
        {
            get
            {
                return ctr;
            }
            set
            {
                ctr = value;
            }
        }
    }
}
