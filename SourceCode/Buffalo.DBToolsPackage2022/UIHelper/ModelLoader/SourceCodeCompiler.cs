using System;
using System.Collections.Generic;
using System.Text;
using System.CodeDom.Compiler;
using System.Reflection;
using Buffalo.Kernel.FastReflection.ClassInfos;
using System.IO;
using Microsoft.CSharp;

namespace Buffalo.DBTools.UIHelper.ModelLoader
{
    /// <summary>
    /// 源码编译器
    /// </summary>
    public class SourceCodeCompiler
    {

        private static Dictionary<string, string> _codeProviderVersion = ProviderVersion();
        private static Dictionary<string, string> ProviderVersion() 
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            StringBuilder version = new StringBuilder(10);
            version.Append("v");
            version.Append(System.Environment.Version.Major.ToString());
            version.Append(".");
            version.Append(System.Environment.Version.MajorRevision.ToString());

            dic["CompilerVersion"] = version.ToString();
           
            return dic;
        }

        /// <summary>
        /// 编译
        /// </summary>
        /// <param name="code">代码</param>
        /// <param name="errorMessage">错误信息</param>
        /// <returns></returns>
        public static CodeGenInfo DoCompiler(string code, string className, List<string> dllReference, StringBuilder errorMessage)
        {

            using (CodeDomProvider cdp = new CSharpCodeProvider(_codeProviderVersion))
            {

                // 编译器的参数 
                CompilerParameters cp = new CompilerParameters();
                cp.ReferencedAssemblies.Add("System.dll");
                cp.ReferencedAssemblies.Add("System.Data.dll");
                cp.ReferencedAssemblies.Add("System.Xml.dll");
                foreach (string link in dllReference)
                {


                    if (!string.IsNullOrEmpty(link))
                    {
                        string elink = link.Trim('\r', '\n', ' ');
                        if (!string.IsNullOrEmpty(elink))
                        {
                            cp.ReferencedAssemblies.Add(elink);
                        }
                    }
                }

                cp.GenerateExecutable = false;
                cp.GenerateInMemory = true;
                // 编译结果 
                CompilerResults cr = cdp.CompileAssemblyFromSource(cp, code);

                if (cr.Errors.HasErrors)
                {
                    foreach (CompilerError cerror in cr.Errors)
                    {

                        errorMessage.AppendLine(cerror.ToString());
                    }
                    cdp.Dispose();
                    return null;
                }
                else
                {
                    Assembly ass = cr.CompiledAssembly;
                    Type objType = ass.GetType(className);
                    CodeGenInfo info = new CodeGenInfo(objType, code);
                    return info;
                }
            }
        }
    }
}
