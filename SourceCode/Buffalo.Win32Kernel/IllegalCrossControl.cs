using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Buffalo.Kernel.FastReflection.ClassInfos;
using Buffalo.Kernel.FastReflection;

namespace Buffalo.Win32Kernel
{
    internal delegate void DelSetProperty(Control ctr, string propertyName, object value) ;
    internal delegate object DelInvokeMethod(Control ctr, string methodName, Type[] parametersType, object[] args);
    public class IllegalCrossControl
    {

        /// <summary>
        /// 设置控件的属性
        /// </summary>
        /// <param name="ctr">要设置的控件</param>
        /// <param name="propertyName">属性名</param>
        /// <param name="value">属性值</param>
        public static void SetProperty(Control ctr, string propertyName, object value) 
        {
            if (ctr.InvokeRequired)
            {
                DelSetProperty del = new DelSetProperty(SetProperty);
                ctr.Parent.Invoke(del, new object[] { ctr, propertyName, value });
            }
            else 
            {
                FastValueGetSet.SetValue(ctr, value, propertyName, ctr.GetType());
            }
        }

        /// <summary>
        /// 设置控件的属性
        /// </summary>
        /// <param name="ctr">要设置的控件</param>
        /// <param name="methodName">函数名</param>
        /// <param name="parametersType">参数类型</param>
        /// <param name="args">参数</param>
        public static object InvokeMethod(Control ctr, string methodName,Type[] parametersType, object[] args)
        {
            object ret = null;
            if (ctr.InvokeRequired)
            {
                DelInvokeMethod del = new DelInvokeMethod(InvokeMethod);
                ret = ctr.Parent.Invoke(del, new object[] { ctr, methodName, args });
            }
            else
            {
                ret=FastValueGetSet.GetCustomerMethodInfo(ctr.GetType(), methodName, parametersType).Invoke(ctr, args);
            }
            return ret;
        }

    }
}
