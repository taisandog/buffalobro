using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.Win32Kernel;

namespace Buffalo.DBTools.HelperKernel
{

    public class ComboBoxItemCollection : List<ComboBoxItem>
    {
        /// <summary>
        /// 名称获取项
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ComboBoxItem FindByText(string text) 
        {
            foreach (ComboBoxItem item in this) 
            {
                if (item.Text == text) 
                {
                    return item;
                }
            }
            return null;
        }
        /// <summary>
        /// 名称获取项
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ComboBoxItem FindByValue(object value)
        {
            foreach (ComboBoxItem item in this)
            {
                if (item.Value == null && value==null) 
                {
                    return item;
                }
                if (item.Value.Equals(value))
                {
                    return item;
                }
            }
            return null;
        }
    }



   
}
