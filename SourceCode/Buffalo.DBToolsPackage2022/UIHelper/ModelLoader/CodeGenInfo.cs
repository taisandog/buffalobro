using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.Kernel.FastReflection;
using System.Reflection;

using System.CodeDom.Compiler;
using Buffalo.GeneratorInfo;

namespace Buffalo.DBTools.UIHelper.ModelLoader
{
    /// <summary>
    /// 代码生成的信息
    /// </summary>
    public class CodeGenInfo
    {
        private string _code;
        /// <summary>
        /// 生成代码
        /// </summary>
        public string Code
        {
            get { return _code; }
        }


        public CodeGenInfo(Type classType, string code)
        {
            _classType = classType;
            _codeClass = Activator.CreateInstance(_classType);
            

            MethodInfo info = classType.GetMethod(CodesManger.DoCompilerName, FastValueGetSet.AllBindingFlags);
            _methodInfo = FastInvoke.GetMethodInvoker(info);
        }

        private Type _classType;

        public Type ClassType
        {
            get { return _classType; }
        }

        private object _codeClass;

        private FastInvokeHandler _methodInfo;

        /// <summary>
        /// 运行编译函数并返回结果
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public string Invoke(EntityInfo entityInfo, UIConfigItem classConfig,
            List<UIModelItem> selectPropertys, UIModelItem classInfo) 
        {
            Buffalo.GeneratorInfo.EntityInfo entity = entityInfo.ToGeneratorEntity(classInfo);
            List<Property> lst = new List<Property>(selectPropertys.Count);
            foreach (UIModelItem item in selectPropertys) 
            {
                lst.Add(item.ToGeneratItem());
            }
            try
            {
                string ret = _methodInfo.Invoke(_codeClass, new object[] { entity, lst }) as string;
                return ret;
            }
            catch (Exception ex) 
            {
                throw ex;
            }
        }
        

    }
}
