using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using System.Diagnostics;

namespace Buffalo.Kernel.FastReflection
{
    /// <summary>
    /// 程序集的类型加载器
    /// </summary>
    public class AssemblyTypeLoader
    {
        private static AssemblyTypeLoader _default;

        /// <summary>
        /// 默认全部程序集的类型加载器
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
        /// 程序集的类型加载器
        /// </summary>
        /// <param name="assNames">指定的程序集</param>
        public AssemblyTypeLoader(string[] assNames)
        {
            _dicAssembly = LoadAllAssembly(assNames);

        }

        /// <summary>
        /// 基目录
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
        /// 获取本项目所有程序集
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
            
            string baseRoot = GetBaseRoot();//本项目所在的路径
            foreach (Assembly ass in arrAss) 
            {
                try
                {

                    if (ass.Location.IndexOf(baseRoot) == 0) //如果此程序集所在的文件在本项目路径下则加到程序集字典
                    {
                        dicAss[ass.GetName().Name] = ass;
#if DEBUG
                        Debug.WriteLine("SQLCommon:已发现模块" + ass.FullName);
#endif
                    }
                }
                catch { }
            }
            return dicAss;
        }

        /// <summary>
        /// 加载类型
        /// </summary>
        /// <param name="typeName">类型名</param>
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
        /// 加载类型
        /// </summary>
        /// <param name="typeName">类型名称</param>
        /// <param name="assemblyName">程序集名</param>
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
        /// 获取加载的程序集中所有的类型
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
