using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Buffalo.Kernel.FastReflection.ClassInfos
{
    /// <summary>
    /// Զ�̳���
    /// </summary>
    public class ProxyAssembly
    {
        Assembly assembly;

        /// <summary>
        /// �����ĳ�����Ϣ
        /// </summary>
        public Assembly Assembly
        {
            get { return assembly; }
        }

        
        /// <summary>
        /// �����ļ�
        /// </summary>
        /// <param name="filePath"></param>
        public ProxyAssembly(string filePath) 
        {
            assembly = Assembly.LoadFile(filePath);
        }
        /// <summary>
        /// �����ļ�����
        /// </summary>
        /// <param name="fileContent"></param>
        public ProxyAssembly(byte[] fileContent)
        {
            assembly = Assembly.Load(fileContent);
        }

        /// <summary>
        /// ��ȡ���͵�ʵ��
        /// </summary>
        /// <param name="typeName">������</param>
        /// <param name="args">���캯������</param>
        /// <returns></returns>
        public ProxyObject GetObjectInstance(string typeName,params object[] args) 
        {
            Type objType=assembly.GetType(typeName);
            return new ProxyObject(objType,args);
        }

    }
}
