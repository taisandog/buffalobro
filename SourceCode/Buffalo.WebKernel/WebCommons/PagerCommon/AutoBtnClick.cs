using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.WebKernel.WebCommons.ContorlCommons;

namespace Buffalo.WebKernel.WebCommons.PagerCommon
{
    public class AutoBtnClick
    {
        private static bool isCheck;
        public const string JsName = "pagerAutoClick.js";
        /// <summary>
        /// 保存成JS文件
        /// </su
        public static void SaveJs()
        {
            if (!isCheck)
            {
                JsSaver.SaveJS(JsName, CreateJS());
                isCheck = true;
            }
        }

        /// <summary>
        /// 生成提交的JS
        /// </summary>
        /// <returns></returns>
        private static string CreateJS()
        {
            StringBuilder js = new StringBuilder(1024);
            js.Append("function onPage(clientId,e)\n");
            js.Append("{\n");
            js.Append("    var eve=window.event||e;\n");
            js.Append("    var keycode=eve.keyCode;\n");
            js.Append("    if (keycode==13)\n");
            js.Append("    {\n");
            js.Append("	    var btn=document.getElementById(clientId);\n");
            js.Append("        \n");
            js.Append("        if(window.event)\n");
            js.Append("        {\n");
            js.Append("            eve.returnValue=false; //cancel Enter action\n");
            js.Append("            btn.click();\n");
            js.Append("	    }\n");
            js.Append("        else\n");
            js.Append("        {\n");
            js.Append("            eve.preventDefault();\n");
            js.Append("            var e = document.createEvent(\"MouseEvents\"); \n");
            js.Append("            e.initEvent(\"click\", true, true); \n");
            js.Append("            btn.dispatchEvent(e);\n");
            js.Append("        }\n");
            js.Append("        return false;\n");
            js.Append("    }\n");
            js.Append("    return true;\n");
            js.Append("}\n");
            return js.ToString();
        }
    }
}
