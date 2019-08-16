using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.Kernel;

namespace Buffalo.DBTools.UIHelper.ModelLoader
{
    public class CodesManger
    {

        private List<string> _sbLink = new List<string>();
        /// <summary>
        /// �������
        /// </summary>
        public List<string> Link
        {
            get { return _sbLink; }
        }

        private StringBuilder _sbUsing = new StringBuilder();
        /// <summary>
        /// ��Ҫusing����
        /// </summary>
        public StringBuilder Using
        {
            get { return _sbUsing; }
        }

        private StringBuilder _sbCode = new StringBuilder();
        /// <summary>
        /// ���ɵ����ģ��
        /// </summary>
        public StringBuilder Code
        {
            get { return _sbCode; }
        }
        private StringBuilder _sbMethod = new StringBuilder();
        /// <summary>
        /// ������
        /// </summary>
        public StringBuilder Method
        {
            get { return _sbMethod; }
        }
        /// <summary>
        /// ����������ռ�
        /// </summary>
        public const string CompilerNamespace = "Buffalo.DBTools.UIHelper.ModelLoaderItems";
        /// <summary>
        /// ���뺯���������
        /// </summary>
        public static readonly string DoCompilerName = "__"+CommonMethods.GuidToString(Guid.NewGuid());
        public string ToCode(string className) 
        {
            StringBuilder sbCode = new StringBuilder(65535);
            //using
            GetUsingCode(sbCode);


            sbCode.AppendLine("namespace " + CompilerNamespace);
            sbCode.AppendLine("{");
            sbCode.AppendLine("    public class " + className);
            sbCode.AppendLine("    {");
            //GetGreanMain(sbCode);
            GetGreanCode(sbCode);
            GetPropertyCode(sbCode);
            sbCode.AppendLine(Method.ToString());

            sbCode.AppendLine("    }");
            sbCode.AppendLine("}");
            return sbCode.ToString();
        }

        /// <summary>
        /// ��ȡUsing���ִ���
        /// </summary>
        /// <param name="sbCode"></param>
        private void GetUsingCode(StringBuilder sbCode) 
        {
            sbCode.AppendLine("using System;");
            sbCode.AppendLine("using System.Collections.Generic;");
            sbCode.AppendLine("using System.Text;");
            sbCode.AppendLine("using Buffalo.GeneratorInfo;");
            sbCode.AppendLine(Using.ToString());
        }

        /// <summary>
        /// ��ȡ���ɵĴ���
        /// </summary>
        private void GetGreanCode(StringBuilder sbCode) 
        {
            sbCode.AppendLine("        public string " + DoCompilerName + "(object entity, object selectedPropertys)");
            sbCode.AppendLine("        {");
            sbCode.AppendLine("            _entity=entity as EntityInfo;");
            sbCode.AppendLine("            _selectedPropertys=selectedPropertys as List<Property>;");

            sbCode.AppendLine("            StringBuilder SystemOut = new StringBuilder(65535);");
            sbCode.AppendLine(Code.ToString());
            sbCode.AppendLine("            return SystemOut.ToString();");
            sbCode.AppendLine("        }");
        }



        /// <summary>
        /// ��ȡ���Դ���
        /// </summary>
        private void GetPropertyCode(StringBuilder sbCode)
        {

            sbCode.AppendLine("        private EntityInfo _entity;");
            sbCode.AppendLine("        public EntityInfo Entity");
            sbCode.AppendLine("        {");
            sbCode.AppendLine("            get ");
            sbCode.AppendLine("            {");
            sbCode.AppendLine("                return _entity;");
            sbCode.AppendLine("            }");
            sbCode.AppendLine("        }");

            sbCode.AppendLine("        private List<Property> _selectedPropertys;");
            sbCode.AppendLine("        public List<Property> SelectedPropertys");
            sbCode.AppendLine("        {");
            sbCode.AppendLine("            get ");
            sbCode.AppendLine("            {");
            sbCode.AppendLine("                return _selectedPropertys;");
            sbCode.AppendLine("            }");
            sbCode.AppendLine("        }");
        }

        

        ///// <summary>
        ///// ��ȡ���ɵĴ���
        ///// </summary>
        //private void GetGreanMain(StringBuilder sbCode)
        //{
        //    sbCode.AppendLine("public static void Main()");
        //    sbCode.AppendLine("{");
        //    sbCode.AppendLine("}");
        //}
    }
}
