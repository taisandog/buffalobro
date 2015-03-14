using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Buffalo.Kernel;

namespace Buffalo.DBTools.UIHelper.ModelLoader
{
    /// <summary>
    /// 模版编译缓存
    /// </summary>
    public class CodeGenCache
    {
        private static Dictionary<string, CodeGenInfo> _dicCodeCache = new Dictionary<string, CodeGenInfo>();

        /// <summary>
        /// 根据文件路径获取生成器
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns></returns>
        public static CodeGenInfo GetGenerationer(string path, EntityInfo entityInfo) 
        {
            CodeGenInfo ret = null;
            if(!_dicCodeCache.TryGetValue(path,out ret))
            {
                ret = CreateGenerationer(path,entityInfo);
                _dicCodeCache[path] = ret;
            }
            return ret;
        }

        /// <summary>
        /// 清空所有缓存
        /// </summary>
        public static void Clear() 
        {
            _dicCodeCache.Clear();
        }

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="path">缓存对应路径</param>
        public static void DeleteGenerationer(string path) 
        {
            _dicCodeCache.Remove(path);
        }

        private static int _classCount = 0;
        /// <summary>
        /// 生成生成器
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static CodeGenInfo CreateGenerationer(string path, EntityInfo entityInfo) 
        {
            FileInfo file = new FileInfo(path);
            string workspace = file.DirectoryName;
            Encoding encoding = CodeFileHelper.GetFileEncoding(path);
            string content = File.ReadAllText(path, encoding);
            string backCodePath=path+"c";
            string backCode="";
            if (File.Exists(backCodePath))
            {
                encoding = CodeFileHelper.GetFileEncoding(backCodePath);
                backCode = File.ReadAllText(backCodePath, encoding);
            }
            //if (backCode == null) 
            //{
            //    backCode = "";
            //}
            ModelCompiler compiler = new ModelCompiler(content, backCode, entityInfo);
            string className = "ModelCompilerClass" + _classCount;
            StringBuilder sbError=new StringBuilder();
            StringBuilder lastCodeCache = new StringBuilder();
            CodeGenInfo info = compiler.GetCompileType(className, lastCodeCache, sbError);
            if (sbError.Length > 0) 
            {
                CompileException ex=new CompileException("模版编译错误:\n" + sbError);
                ex.Code = lastCodeCache.ToString();
                throw ex;
            }
            
            
            _classCount++;
            return info;
        }
    }
}
