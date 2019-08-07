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
    /// ����ʵ���������Ϣ
    /// </summary>
    public class ModelInfo
    {
        PropertyInfoHandle phandle;
        ContorlDefaultPropertyInfo ctrHandle;
        Control ctr;

        /// <summary>
        /// ��Ӧ��������Ϣ
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
        /// ��Ӧ�Ŀؼ���Ϣ
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
        /// ��Ӧ�Ŀؼ�
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
