﻿using System;
using System.Collections.Generic;

using System.Text;
using Buffalo.WebKernel.WebCommons.ContorlCommons;
using System.IO;
using Buffalo.Kernel.ZipUnit;
using Buffalo.WebKernel.WebCommons.PageBases;
using System.Web.UI;
using Buffalo.Kernel;
using System.Collections.Concurrent;

namespace Buffalo.WebKernel.ARTDialog
{
    /// <summary>
    /// ArtDialog类
    /// </summary>
    public class ArtDialog
    {
        /// <summary>
        /// 皮肤信息集合
        /// </summary>
        private IDictionary<int, EnumInfo> _dicSkin = GetSkin();

        private static bool _isInit = InitDialog();

        /// <summary>
        /// 初始化对话框
        /// </summary>
        /// <returns></returns>
        private static bool InitDialog()
        {
            string dir = JsSaver.BaseSavePath;
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            dir = JsSaver.BaseSavePath + "artDialog/";
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            else
            {
                return true;
            }

            using (MemoryStream mem = new MemoryStream(ARTDialogReource.artdialog))
            {
                SharpUnZipFile unZip = new SharpUnZipFile(mem);
                unZip.UnZipFiles(dir);
                File.WriteAllText(dir + "artDialogShow.js", ARTDialogReource.artdialogshow, Encoding.UTF8);
            }

            return true;
        }
        /// <summary>
        /// 获取皮肤信息
        /// </summary>
        /// <returns></returns>
        private static IDictionary<int, EnumInfo> GetSkin() 
        {
            List<EnumInfo> lstInfo = EnumUnit.GetEnumInfos(typeof(DialogSkin));

            //ConcurrentDictionary<int, EnumInfo> dic = CommonMethods.ListToDictionary<int, EnumInfo>(lstInfo, "Value");
            ConcurrentDictionary<int, EnumInfo> dic = new ConcurrentDictionary<int, EnumInfo>();
            foreach(EnumInfo info in lstInfo)
            {
                dic[(int)info.Value] = info;
            }
            return dic;
        }
        /// <summary>
        /// ArtDialog
        /// </summary>
        public ArtDialog():this(DialogSkin.Default)
        {
        }

        private string _defaultTitle;

        /// <summary>
        /// 默认标题
        /// </summary>
        public string DefaultTitle
        {
            get { return _defaultTitle; }
            set { _defaultTitle = value; }
        }

        /// <summary>
        /// ArtDialog
        /// </summary>
        public ArtDialog(DialogSkin skin) 
        {
            _curPage = System.Web.HttpContext.Current.Handler as Page;

            
            string jsName = "artdialog/artDialog.source.js?skin="+_dicSkin[(int)skin].Description;
            RegisterJS(jsName);
            jsName = "artdialog/plugins/iframeTools.source.js";
            RegisterJS(jsName);
            jsName = "artdialog/artDialogShow.js";
            RegisterJS(jsName);
        }
        /// <summary>
        /// ArtDialog
        /// </summary>
        public ArtDialog(string skinName)
        {
            _curPage = System.Web.HttpContext.Current.Handler as Page;


            string jsName = "artdialog/artDialog.source.js?skin=" + skinName;
            RegisterJS(jsName);
            jsName = "artdialog/plugins/iframeTools.source.js";
            RegisterJS(jsName);
            jsName = "artdialog/artDialogShow.js";
            RegisterJS(jsName);
        }
        /// <summary>
        /// 获取关闭窗体的JS
        /// </summary>
        /// <param name="top">是否关闭顶层窗口</param>
        /// <returns></returns>
        public string GetCloseDialogJS(bool top) 
        {
            StringBuilder js = new StringBuilder();
            if (top)
            {
                js.Append("window.top.");
                
            }
            js.Append("artShow_CloseCurDialog()");
            return js.ToString();
        }
        /// <summary>
        /// 获取关闭窗体的JS
        /// </summary>
        /// <returns></returns>
        public string GetCloseDialogJS()
        {
            return GetCloseDialogJS(true);
        }
        /// <summary>
        /// 引用JS文件
        /// </summary>
        /// <param name="jsName">文件名</param>
        private void RegisterJS(string jsName)
        {
            if (!_curPage.ClientScript.IsClientScriptIncludeRegistered(jsName + "Include"))
            {
                _curPage.ClientScript.RegisterClientScriptInclude(jsName + "Include", JsSaver.GetDefualtJsUrl(jsName));
            }
        }

       


        private Page _curPage = null;
        /// <summary>
        /// 当前页面
        /// </summary>
        public Page CurPage 
        {
            get 
            {
                return _curPage;
            }
        }

        private int _defauleWidth=300;
        /// <summary>
        /// 默认宽度
        /// </summary>
        public int DefauleWidth
        {
            get { return _defauleWidth; }
            set { _defauleWidth = value; }
        }
        private int _defauleHeight=100;
        /// <summary>
        /// 默认高度
        /// </summary>
        public int DefauleHeight
        {
            get { return _defauleHeight; }
            set { _defauleHeight = value; }
        }
        /// <summary>
        /// 获取布尔型的字符串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private string GetBooleanString(bool value) 
        {
            return value ? "true" : "false";
        }

        /// <summary>
        /// 获取值的JS表示
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="isString">是否字符串</param>
        /// <returns></returns>
        private string GetJSValue(string value,bool isString) 
        {
            string ret = "null";
            if (!string.IsNullOrEmpty(value))
            {
                if (isString)
                {
                    ret = "'" + value.Replace("'", "\\'") + "'";
                }
                else 
                {
                    ret =  value ;
                }
            }
            return ret;
        }

        /// <summary>
        /// 显示提示框
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <param name="width">宽</param>
        /// <param name="height">高</param>
        /// <param name="isDialog">是否模态窗口</param>
        /// <param name="findParent">查找父窗体</param>
        ///  <param name="icon">图标标识(ArtIcons)</param>
        ///  <param name="closeHandle">关闭时候触发的JS</param>
        public void ShowDialog(string title,string content,int width,int height,bool isDialog,string icon,bool top,string closeHandle) 
        {
            string sicon = GetJSValue(icon, true);
            string handle = GetJSValue(closeHandle, false);
            StringBuilder js = new StringBuilder();
            if (top) 
            {
                js.Append("window.top.");
                
            }
            js.Append("artShow_AlertDialog('" + title.Replace("'", "\\'") + "','" + content.Replace("'", "\\'") + "'," + width + "," + height + ","
                + GetBooleanString(isDialog) + "," + sicon + "," + handle + ");");
            CurPage.ClientScript.RegisterStartupScript(System.Web.HttpContext.Current.Handler.GetType(), content, js.ToString(), true);
            
        }

        /// <summary>
        /// 显示提示框
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="top">是否顶层窗口弹出对话框</param>
        /// <param name="content">内容</param>
        public void Alert(string title, bool top, string content) 
        {
            ShowDialog(title, content, _defauleWidth, _defauleHeight, true, null,top,null);
        }

        /// <summary>
        /// 显示提示框
        /// </summary>
        /// <param name="content">内容</param>
        public void Alert( string content)
        {
            Alert(_defaultTitle, true, content);
        } 

        /// <summary>
        /// 显示提示框
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="top">是否顶层窗口弹出对话框</param>
        /// <param name="content">内容</param>
        public void SuccessDialog(string title,bool top, string content)
        {
            ShowDialog(title, content, _defauleWidth, _defauleHeight, true, ArtIcons.Succeed, top, null);
        }
        /// <summary>
        /// 显示提示框
        /// </summary>
        /// <param name="content">内容</param>
        public void SuccessDialog(string content)
        {
            SuccessDialog(_defaultTitle, true, content);
        }
        /// <summary>
        /// 显示提示框
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="top">是否顶层窗口弹出对话框</param>
        /// <param name="content">内容</param>
        public void ErrorDialog(string title, bool top, string content)
        {
            ShowDialog(title, content, _defauleWidth, _defauleHeight, true, ArtIcons.Error, top, null);
        }
        /// <summary>
        /// 显示提示框
        /// </summary>
        /// <param name="content">内容</param>
        public void ErrorDialog( string content)
        {
            ErrorDialog(_defaultTitle,true,content);
        }
        /// <summary>
        /// 显示提示框
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="top">是否顶层窗口弹出对话框</param>
        /// <param name="content">内容</param>
        public void WarningDialog(string title, bool top, string content)
        {
            ShowDialog(title, content, _defauleWidth, _defauleHeight, true, ArtIcons.Warning, top, null);
        }
        /// <summary>
        /// 显示提示框
        /// </summary>
        /// <param name="content">内容</param>
        public void WarningDialog( string content)
        {
            WarningDialog(_defaultTitle,true,content);
        }
        /// <summary>
        /// 获取iframe弹出窗的JS
        /// </summary>
        /// <param name="url">iframe地址</param>
        /// <param name="title">标题</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <param name="isDialog">是否模态窗口</param>
        /// <param name="closeHandle">关闭时候执行的方法</param>
        /// <returns></returns>
        public string GetFrameDialog(string url, string title, int width, int height, bool isDialog, string closeHandle) 
        {
            return "artShow_ShowIFrameDialog('"+url+"','"+title.Replace("'", "\\'") +"',"+ width + ","
                + height + "," + GetBooleanString(isDialog) + "," + GetJSValue(closeHandle,false) + ")";
        }
        
    }
}
