using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Buffalo.Kernel.FastReflection.ClassInfos
{
    /// <summary>
    /// 远程程序集
    /// </summary>
    public class ProxyAssembly
    {
        Assembly assembly;

        /// <summary>
        /// 关联的程序集信息
        /// </summary>
        public Assembly Assembly
        {
            get { return assembly; }
        }

        
        /// <summary>
        /// 加载文件
        /// </summary>
        /// <param name="filePath"></param>
        public ProxyAssembly(string filePath) 
        {
            assembly = Assembly.LoadFile(filePath);
        }
        /// <summary>
        /// 加载文件内容
        /// </summary>
        /// <param name="fileContent"></param>
        public ProxyAssembly(byte[] fileContent)
        {
            assembly = Assembly.Load(fileContent);
        }

        /// <summary>
        /// 获取类型的实体
        /// </summary>
        /// <param name="typeName">类型名</param>
        /// <param name="args">构造函数参数</param>
        /// <returns></returns>
        public ProxyObject GetObjectInstance(string typeName,params object[] args) 
        {
            Type objType=assembly.GetType(typeName);
            return new ProxyObject(objType,args);
        }

    }
}
