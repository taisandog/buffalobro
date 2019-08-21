using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using System.Diagnostics;

namespace Buffalo.Kernel.FastReflection
{
    /// <summary>
    /// ���򼯵����ͼ�����
    /// </summary>
    public class AssemblyTypeLoader
    {
        private static AssemblyTypeLoader _default;

        /// <summary>
        /// Ĭ��ȫ�����򼯵����ͼ�����
        /// </summary>
        public static AssemblyTypeLoader Default 
        {
            get 
            {
                if (_default == null) 
                {
                    _default = new AssemblyTypeLoader(null);
                }
                return _default;
            }
        }


        private Dictionary<string, Assembly> _dicAssembly = null;

        /// <summary>
        /// ���򼯵����ͼ�����
        /// </summary>
        /// <param name="assNames">ָ���ĳ���</param>
        public AssemblyTypeLoader(string[] assNames)
        {
            _dicAssembly = LoadAllAssembly(assNames);

        }

        /// <summary>
        /// ��Ŀ¼
        /// </summary>
        /// <returns></returns>
        private string GetBaseRoot() 
        {
            if (CommonMethods.IsWebContext) 
            {
                return AppDomain.CurrentDomain.DynamicDirectory;
            }
            return AppDomain.CurrentDomain.BaseDirectory;
        }

        /// <summary>
        /// ��ȡ����Ŀ���г���
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, Assembly> LoadAllAssembly(string[] assNames) 
        {
            IEnumerable<Assembly> arrAss = null;
            Dictionary<string, Assembly> dicAss = new Dictionary<string, Assembly>();
            if (assNames == null)
            {
                arrAss = AppDomain.CurrentDomain.GetAssemblies();
            }
            else 
            {
                
                foreach (string assName in assNames) 
                {
                    try
                    {
                        Assembly ass = Assembly.Load(assName);
                        if (ass != null)
                        {
                            dicAss[assName]=ass;
                        }
                    }
                    catch { }
                }
                return dicAss;
            }
            
            string baseRoot = GetBaseRoot();//����Ŀ���ڵ�·��
            foreach (Assembly ass in arrAss) 
            {
                try
                {

                    if (ass.Location.IndexOf(baseRoot) == 0) //����˳������ڵ��ļ��ڱ���Ŀ·������ӵ������ֵ�
                    {
                        dicAss[ass.GetName().Name] = ass;
#if DEBUG
                        Debug.WriteLine("SQLCommon:�ѷ���ģ��" + ass.FullName);
#endif
                    }
                }
                catch { }
            }
            return dicAss;
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="typeName">������</param>
        /// <returns></returns>
        public Type LoadType(string typeName)
        {
            foreach (KeyValuePair<string, Assembly> assPair in _dicAssembly)
            {
                Assembly ass = assPair.Value;
                Type curType = ass.GetType(typeName);
                if (curType != null)
                {
                    return curType;
                }
            }
            return null;
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="typeName">��������</param>
        /// <param name="assemblyName">������</param>
        /// <returns></returns>
        public Type LoadType(string typeName, string assemblyName)
        {
            Assembly ass = null;
            Type ret = null;
            if (!_dicAssembly.TryGetValue(assemblyName, out ass))
            {
                ass = Assembly.Load(assemblyName);
                if (ass != null)
                {
                    _dicAssembly[assemblyName] = ass;
                }
                else
                {
                    return null;
                }
            }

            ret = ass.GetType(typeName);
            return ret;
        }

        /// <summary>
        /// ��ȡ���صĳ��������е�����
        /// </summary>
        /// <returns></returns>
        public List<Type> GetTypes()
        {
            List<Type> lstType = new List<Type>(1024);
            foreach (KeyValuePair<string, Assembly> assPair in _dicAssembly)
            {

                Assembly ass = assPair.Value;
                lstType.AddRange(ass.GetTypes());
            }
            return lstType;
        }

    }
}
