using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Buffalo.Kernel.FastReflection;
using System.Collections.Concurrent;

namespace Buffalo.Win32Kernel
{
    public class SingleForms
    {
        private static ConcurrentDictionary<string, Control> dicForms = new ConcurrentDictionary<string, Control>();
        private static AssemblyTypeLoader _assemblyTypeLoader = AssemblyTypeLoader.Default;

        private static ConcurrentDictionary<string, Type> dicType = new ConcurrentDictionary<string, Type>();

        /// �жϴ����Ƿ����
        /// </summary>
        /// <param name="frm">����</param>
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
        /// ����Form�����ͻ�ȡ��ʵ��
        /// </summary>
        /// <param name="typeName">������</param>
        /// <param name="args">����</param>
        /// <returns></returns>
        public static Control GetControl(string typeName, params object[] args) 
        {
            string key = typeName;//��ȡ�������FullName
            Type ctrType = null;
            if (!dicType.TryGetValue(key, out ctrType)) 
            {
                ctrType = _assemblyTypeLoader.LoadType(typeName);
            }
            return GetControl(ctrType, args);
        }

        /// <summary>
        /// ����Form�����ͻ�ȡ��ʵ��
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Control GetControl(Type type,params object[] args) 
        {
            string key = type.FullName;//��ȡ�������FullName
            Control ctr = null;
            dicForms.TryGetValue(key, out ctr);//���ԴӾ�̬������߻�ȡ����
            if (!IsControlExists(ctr))//������岻���ڻ����Ѿ����ر���ʵ��������
            {
                ctr = (Control)Activator.CreateInstance(type, args);//ʵ��������
                dicForms[key] = ctr;
            }
            return ctr;
        }

        /// <summary>
        /// ��ȡָ������
        /// </summary>
        /// <typeparam name="T">��������</typeparam>
        /// <returns></returns>
        public static T GetControl<T>(params object[] args) where T : Control
        {
            Type type = typeof(T);//��ȡ�˴������͵�Type

            return (T)GetControl(type, args);
        }

        /// <summary>
        /// ��ȡָ������
        /// </summary>
        /// <typeparam name="T">��������</typeparam>
        /// <returns></returns>
        public static T GetForm<T>(params object[] args) where T : Form
        {
           Type type = typeof(T);//��ȡ�˴������͵�Type

           return (T)GetControl(type, args);
        }
    }
}
