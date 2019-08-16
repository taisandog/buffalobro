using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Reflection;
using Buffalo.Kernel;

namespace Buffalo.DBTools.UIHelper.ModelLoader
{
    /// <summary>
    /// ģ�浽�����ת����
    /// </summary>
    public class ModelCompiler
    {

        private CodesManger _man = new CodesManger();
        private string _content;//model�ļ�����
        EntityInfo _entityInfo;
        private string _backCode;//model.cs�ļ�����
        /// <summary>
        /// ģ�浽�����ת����
        /// </summary>
        /// <param name="content">model�ı�����</param>
        /// <param name="backCode">��̨����</param>
        /// <param name="entityInfo">ʵ����Ϣ</param>
        public ModelCompiler(string content, string backCode, EntityInfo entityInfo) 
        {
            _content = content;
            _entityInfo = entityInfo;
            _backCode = backCode;
        }


        /// <summary>
        /// ����Script��ǩ
        /// </summary>
        public string GetCode(string className) 
        {
            
            
            
            //����Model.cs��Ϣ
            string strRef = @"(?isx)<[#]script\stype=""(?<type>[^""]+)"">(?<content>(.*?))</[#]script>";
            MatchCollection matches = new Regex(strRef).Matches(_backCode);
            foreach (Match ma in matches)
            {
                if (ma.Groups["type"] == null) 
                {
                    continue;
                }
                string type = ma.Groups["type"].Value;
                string content=ma.Groups["content"].Value;
                CodeGeneration com=new CodeGeneration(content);
                Queue<ExpressionItem> queitem=com.ExpressionItems;
                if (type.Equals("linked",StringComparison.CurrentCultureIgnoreCase)) 
                {
                    LinkOutputer outputer = new LinkOutputer();
                    List<string> str = outputer.GetCode(queitem,_entityInfo);
                    _man.Link.AddRange(str);
                }
                else if (type.Equals("using", StringComparison.CurrentCultureIgnoreCase)) 
                {
                    UsingOutputer outputer = new UsingOutputer();
                    string str = outputer.GetCode(queitem);
                    _man.Using.Append(str);
                }
                else if (_content==null && type.Equals("code", StringComparison.CurrentCultureIgnoreCase))
                {
                    CodeOutputer outputer = new CodeOutputer();
                    string str = outputer.GetCode(queitem);
                    _man.Code.Append(str);
                }
                else if (type.Equals("method", StringComparison.CurrentCultureIgnoreCase))
                {
                    MethodOutputer outputer = new MethodOutputer();
                    string str = outputer.GetCode(queitem);
                    _man.Method.Append(str);
                }
            }
            //����Model��Ϣ
            if (_content != null)
            {
                _man.Code.Append(GetContentCode(_content));
            }
            return _man.ToCode(className);
        }

        /// <summary>
        /// ��ȡ���ݵĴ���
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        private string GetContentCode(string content) 
        {
            CodeGeneration com = new CodeGeneration(content);
            Queue<ExpressionItem> queitem = com.ExpressionItems;
            CodeOutputer outputer = new CodeOutputer();
            string str = outputer.GetCode(queitem);
            return str;
        }

        /// <summary>
        /// ��ӱ���Ŀ������
        /// </summary>
        /// <param name="dll"></param>
        private void AddBuffaloLink() 
        {
            Assembly ass = null;

            //ass = typeof(CodeGeneration).Assembly;
            //string path = new Uri(ass.CodeBase).LocalPath;
            //FileInfo info = new FileInfo(path);
            //DirectoryInfo dic = info.Directory;
            //FileInfo[] files = dic.GetFiles();
            //foreach (FileInfo finfo in files) 
            //{
            //    string fpath = finfo.FullName;
            //    if (fpath.LastIndexOf(".dll") >= 0) 
            //    {
            //        dll.Add(fpath);
            //    }
            //}
            ass = typeof(Buffalo.DBTools.CommandBar).Assembly;
            string path=new Uri(ass.CodeBase).LocalPath;
            FileInfo file = new FileInfo(path);
            path = file.DirectoryName + "\\Buffalo.GeneratorInfo.dll";
            if (!File.Exists(path)) 
            {
                return;
            }
            _man.Link.Add(path);
            //file=new FileInfo(path);
            //string fileName=CommonMethods.GetBaseRoot(file.Name);

            //CommonMethods.CopyNewer(path, fileName);
            //ass = typeof(Buffalo.DB.QueryConditions.ScopeList).Assembly;
            //dll.Add(new Uri(ass.CodeBase).LocalPath);

            //ass = typeof(CodeGeneration).Assembly;
            //dll.Add(new Uri(ass.CodeBase).LocalPath);

            
        }


        /// <summary>
        /// ��ȡ����������
        /// </summary>
        /// <param name="errorMessage">��������Ϊ��Ϣ</param>
        /// <returns></returns>
        public CodeGenInfo GetCompileType(string className,StringBuilder codeCache, StringBuilder errorMessage)
        {
            AddBuffaloLink();
            string code = GetCode(className);
            codeCache.Append(code);
            string ret = null;
            CodeGenInfo info = SourceCodeCompiler.DoCompiler(code, CodesManger.CompilerNamespace + "." + className, _man.Link, errorMessage);

            return info;
        }
    }
}
