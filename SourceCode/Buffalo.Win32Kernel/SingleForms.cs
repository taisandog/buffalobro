using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Buffalo.Kernel.FastReflection;

namespace Buffalo.Win32Kernel
{
    public class SingleForms
    {
        private static Dictionary<string, Control> dicForms = new Dictionary<string, Control>();
        private static AssemblyTypeLoader _assemblyTypeLoader = AssemblyTypeLoader.Default;

        private static Dictionary<string, Type> dicType = new Dictionary<string, Type>();

        /// 判断窗体是否存在
        /// </summary>
        /// <param name="frm">窗体</param>
        /// <returns></returns>
        private static bool IsControlExists(Control ctr)
        {
            if (ctr == null)
           {
               return false;
           }
           if (ctr.IsDisposed)
           {
               return false;
           }
           return true;
        }

        /// <summary>
        /// 根据Form的类型获取其实例
        /// </summary>
        /// <param name="typeName">类型名</param>
        /// <param name="args">参数</param>
        /// <returns></returns>
        public static Control GetControl(string typeName, params object[] args) 
        {
            string key = typeName;//获取窗体类的FullName
            Type ctrType = null;
            if (!dicType.TryGetValue(key, out ctrType)) 
            {
                ctrType = _assemblyTypeLoader.LoadType(typeName);
            }
            return GetControl(ctrType, args);
        }

        /// <summary>
        /// 根据Form的类型获取其实例
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Control GetControl(Type type,params object[] args) 
        {
            string key = type.FullName;//获取窗体类的FullName
            Control ctr = null;
            dicForms.TryGetValue(key, out ctr);//尝试从静态集合里边获取窗体
            if (!IsControlExists(ctr))//如果窗体不存在或者已经被关闭则实例化窗体
            {
                ctr = (Control)Activator.CreateInstance(type, args);//实例化对象
                dicForms[key] = ctr;
            }
            return ctr;
        }

        /// <summary>
        /// 获取指定窗体
        /// </summary>
        /// <typeparam name="T">窗体类型</typeparam>
        /// <returns></returns>
        public static T GetControl<T>(params object[] args) where T : Control
        {
            Type type = typeof(T);//获取此窗体类型的Type

            return (T)GetControl(type, args);
        }

        /// <summary>
        /// 获取指定窗体
        /// </summary>
        /// <typeparam name="T">窗体类型</typeparam>
        /// <returns></returns>
        public static T GetForm<T>(params object[] args) where T : Form
        {
           Type type = typeof(T);//获取此窗体类型的Type

           return (T)GetControl(type, args);
        }
    }
}
