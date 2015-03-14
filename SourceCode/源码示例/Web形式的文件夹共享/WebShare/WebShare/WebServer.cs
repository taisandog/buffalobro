using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.Kernel.HttpServerModel;
using Buffalo.Kernel;
using System.IO;
using System.Web;
using Buffalo.WebKernel.WebCommons;
using System.Net.Sockets;

namespace WebShare
{
    public class WebServer
    {
        /// <summary>
        /// 模版
        /// </summary>
        private string _model = null;

        /// <summary>
        /// 密码模版
        /// </summary>
        private string _pwdmodel = null;
        /// <summary>
        /// 服务器
        /// </summary>
        private ServerModel _server;

        /// <summary>
        /// 服务器模块
        /// </summary>
        public ServerModel Server
        {
            get { return _server; }
        }


        private ConfigManager _config;
        /// <summary>
        /// 配置信息
        /// </summary>
        public ConfigManager Config
        {
            get { return _config; }
        }

        /// <summary>
        /// 加载模版
        /// </summary>
        private static string LoadModel() 
        {
            string path = CommonMethods.GetBaseRoot("model.htm");
            return File.ReadAllText(path, Encoding.UTF8);
        }
        /// <summary>
        /// 加载模版
        /// </summary>
        private static string LoadPwdModel()
        {
            string path = CommonMethods.GetBaseRoot("modelPwd.htm");
            return File.ReadAllText(path, Encoding.UTF8);
        }

        public WebServer(ConfigManager config) 
        {
            _config = config;
            
        }
        private const string DownPage = "download.htm";
        /// <summary>
        /// 开启服务
        /// </summary>
        public void Start() 
        {
            Stop();
            _model = LoadModel();
            _pwdmodel = LoadPwdModel();
            _server = null;

            _server = new ServerModel(_config.BindIP, _config.BindPort);
            _server.OnRequestProcessing += new RequestProcessingHandle(_server_OnRequestProcessing);
            _server.OnRequestSendingBody += new RequestSendingBodyHandle(_server_OnRequestSendingBody);
            _server.StarServer();
        }

        public bool Running 
        {
            get 
            {
                if (_server != null) 
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// 关闭服务
        /// </summary>
        public void Stop() 
        {
            if (_server != null)
            {
                _server.StopServer();
            }

        }

        void _server_OnRequestProcessing(RequestInfo request, ResponseInfo response)
        {
            if(request.RequestParam["act"]=="login")
            {
                response.Write(_pwdmodel);
                return;
            }
            //验证密码
            if (!VerifiyPwd(request)) 
            {
                ShowPwdError(request,response);
                return;
            }
            string url = request.Path;
            string fileName=null;
            bool isRoot=false;
            string curDir = GetDirectory(url, out fileName, out isRoot);

            if (fileName == "..") 
            {
                DirectoryInfo dinfo = new DirectoryInfo(curDir);
                if (isRoot)
                {
                    curDir = dinfo.FullName;
                }
                else
                {
                    curDir = dinfo.Parent.FullName;
                }
                fileName = "";
            }
            string strfname = HttpUtility.UrlDecode(fileName);
            if (string.IsNullOrEmpty(fileName) && string.Equals(request.RequestParam["type"],"lst",StringComparison.CurrentCultureIgnoreCase))//输出批量下载文件
            {
                if (PutLst(curDir, request, response, url))
                {
                    return;
                }
            }
            else if (!string.IsNullOrEmpty(fileName))//输出文件
            {
                
                string showFileName = curDir + strfname;

                ShowFile(showFileName, strfname,request, response);
            }
            else
            {
                if (PutPageTo(curDir, request, response, url))
                {
                    return;
                }
            }
            
            //response.Write("没有数据");
        }

        void _server_OnRequestSendingBody(RequestInfo request, ResponseInfo response, System.Net.Sockets.Socket requestSocket)
        {
            if (response.UserTag != null) 
            {
                FileInfo finf = response.UserTag as FileInfo;
                RangeInfo range=null;
                if (request.Header.ContainsKey("range")) 
                {
                    List<RangeInfo> lstRange = ResponseInfo.GetRange(request.Header["Range"], finf.Length);
                    if (lstRange.Count > 0)
                    {
                        range = lstRange[0];
                    }
                    
                }
                if(range==null)
                {
                    range = new RangeInfo();
                    range.End = finf.Length - 1;
                }
                if (finf != null) 
                {
                    byte[] buff = new byte[512 * 1024];
                    int read=0;
                    using (FileStream stm = finf.Open(FileMode.Open)) 
                    {
                        stm.Position = range.Start;
                        long totleLen = range.Length;
                        while (true)
                        {
                            

                            read = stm.Read(buff, 0, buff.Length);
                            if (read > totleLen) 
                            {
                                read = (int)totleLen;
                            }
                            if (read > 0)
                            {
                                requestSocket.Send(buff, 0, read, SocketFlags.None);
                                totleLen -= read;
                            }
                            if(read<=0 || totleLen<=0)
                            {
                                break;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 验证密码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private bool VerifiyPwd(RequestInfo request) 
        {
            string pwd=_config.Password;
            if (string.IsNullOrEmpty(pwd)) 
            {
                return true;
            }
            string cPwd = request.RequestParam["pwd"];
            if (string.IsNullOrEmpty(cPwd)) 
            {
                return false;
            }
            return cPwd.Equals(pwd);
        }
        ///// <summary>
        ///// 获取密码参数
        ///// </summary>
        ///// <returns></returns>
        //private string GetPwdParam() 
        //{
        //    string pwd = _config.Password;

        //    return "pwd=" + pwd;
        //}
        /// <summary>
        /// 显示密码错误
        /// </summary>
        /// <param name="response"></param>
        private void ShowPwdError(RequestInfo reqest, ResponseInfo response) 
        {
            response.StatusCode = 301;
            response.Header["Location:"] = reqest.Path + "?act=login";
        }


        /// <summary>
        /// 输出网页
        /// </summary>
        /// <param name="curDir"></param>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        private bool PutPageTo(string curDir, RequestInfo request, ResponseInfo response, string url)  
        {
            PageModelInfo pageInfo = new PageModelInfo();
            pageInfo.CurLoaction = curDir;
            if (string.IsNullOrEmpty(curDir))
            {
                foreach (ShareInfo dinfo in _config.ShareInfos)
                {
                    DirInfo info = new DirInfo();
                    info.Text = dinfo.Name;
                    info.Url = dinfo.Name;
                    pageInfo.Directorys.Add(info);
                }
            }
            else
            {
                if (Directory.Exists(curDir))
                {
                    string[] strdirs = Directory.GetDirectories(curDir);

                    DirectoryInfo dinfo = new DirectoryInfo(curDir);
                    DirInfo info = new DirInfo();
                    info.Text = "..";
                    info.Url = "..";
                    pageInfo.Directorys.Add(info);
                    foreach (string strDir in strdirs)
                    {
                        dinfo = new DirectoryInfo(strDir);
                        info = new DirInfo();
                        info.Text = dinfo.Name;
                        info.Url = dinfo.Name + "/";
                        pageInfo.Directorys.Add(info);
                    }
                    strdirs = Directory.GetFiles(curDir);
                    foreach (string strDir in strdirs)
                    {
                        FileInfo finf = new FileInfo(strDir);
                        info = new DirInfo();
                        info.Text = finf.Name;
                        info.Url = finf.Name;
                        info.Length = finf.Length;
                        pageInfo.Files.Add(info);
                    }
                }
            }
            PutPage(pageInfo, response);
            return true;
        }

        /// <summary>
        /// 输出文件目录的下载列表
        /// </summary>
        /// <param name="curDir"></param>
        /// <param name="request"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        private bool PutLst(string curDir,RequestInfo request,ResponseInfo response,string url) 
        {
            DirectoryInfo dinfo = new DirectoryInfo(curDir);
            if (dinfo.Exists)
            {
                string content = GetDictoryFiles(request, curDir, HttpUtility.HtmlDecode(url));
                response.MimeType = WebCommon.GetContentType("*");
                response.Header["Content-Disposition"] = "attachment;filename=" + HttpUtility.UrlEncode(dinfo.Name) + ".lst";
                response.Header["Connection"] = "close";

                response.Write(content);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 文件夹下所有文件
        /// </summary>
        /// <returns></returns>
        private string GetDictoryFiles(RequestInfo request,string curDir, string curRrl)
        {
            string[] strdirs = Directory.GetFiles(curDir,"*.*",SearchOption.AllDirectories);
            StringBuilder sbRet = new StringBuilder();
            string url = "http://"+request.Host + curRrl;
            foreach (string strDir in strdirs)
            {
                string cur = strDir;
                cur = cur.Substring(curDir.Length).Trim('\\');
                cur = cur.Replace('\\', '/');
                cur = HttpUtility.UrlEncode(cur);
                cur = url + cur;
                
                sbRet.AppendLine(cur);
            }
            return sbRet.ToString();
        }

        /// <summary>
        /// 显示文件
        /// </summary>
        /// <param name="filename"></param>
        private void ShowFile(string filename, string name,RequestInfo request, ResponseInfo response)
        {
            FileInfo finf = new FileInfo(filename);
            response.MimeType = WebCommon.GetContentType(finf.Extension.Trim('.'));
            string strName = RequestInfo.GetFileName(request,name);
            response.Header["Content-Disposition"] = "attachment;filename=" + strName;
            long len = finf.Length;
            response.Header["Accept-Ranges"] = "bytes";
            response.Header["Content-Transfer-Encoding"]= "binary";
            if (request.Header.ContainsKey("range")) //如果客户端指定了分段下载
            {
                List<RangeInfo> lstRange = ResponseInfo.GetRange(request.Header["Range"], finf.Length);//处理区段信息
                if (lstRange.Count > 0)
                {
                    response.Header["Content-Range"] = "bytes " + lstRange[0].Start + "-" + lstRange[0].End + "/" + finf.Length;//指定区段
                    response.StatusCode = 206;
                }
            }
            try
            {
                response.UserTag = finf;
                response.Length = len;
                response.RangeLength = finf.Length;
            }
            catch (Exception ex)
            {
                response.Write("数据读取失败:" + ex.Message);
            }

        }

        /// <summary>
        /// 获取文件夹
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private string GetDirectory(string url,out string fileName,out bool isRoot) 
        {
            string[] parts = url.Split('/');
            StringBuilder dir = new StringBuilder();
            fileName = "";
            for(int i=0;i<parts.Length;i++) 
            {
                string part = parts[i];
                string cur =HttpUtility.UrlDecode(part.Trim());
                if (string.IsNullOrEmpty(cur)) 
                {
                    continue;
                }
                if (dir.Length == 0)
                {
                    ShareInfo info=_config.ShareInfos.FindItem(cur);
                    if (info != null)
                    {
                        dir.Append(info.Path.TrimEnd('\\', ' '));
                        dir.Append("\\");
                    }
                }
                else if (i >= parts.Length - 1)
                {
                    fileName = cur;
                }
                else 
                {
                    dir.Append(cur);
                    dir.Append("\\");
                }
            }
            isRoot = parts.Length <= 1;
            return dir.ToString();
        }

        /// <summary>
        /// 输出页面
        /// </summary>
        /// <param name="lstDirectory"></param>
        /// <param name="lstFiles"></param>
        private void PutPage(PageModelInfo pageInfo, ResponseInfo response) 
        {
            string content = _model;
            content = content.Replace("<#=Root#>", pageInfo.CurLoaction);
            content = content.Replace("<#=DirectoryItems#>", GenDirectoryHtml(pageInfo.Directorys));
            content = content.Replace("<#=FileItems#>", GenFileHtml(pageInfo.Files));
            response.Write(content);
        }

        /// <summary>
        /// 获取鼠标事件
        /// </summary>
        /// <returns></returns>
        private string GetMouseRoll() 
        {
            return " onmouseover=\"curSty=this.style.backgroundColor;this.style.backgroundColor='#c1ebff';\" onmouseout=\"this.style.backgroundColor=curSty;\" ";
        }
        /// <summary>
        /// 生成目录的html
        /// </summary>
        /// <returns></returns>
        private string GenDirectoryHtml(IList<DirInfo> lstDirectory) 
        {
            StringBuilder sb = new StringBuilder();
            for(int i=0;i<lstDirectory.Count;i++)
            {
                DirInfo info=lstDirectory[i];
                if (i % 2 == 1)
                {
                    sb.AppendLine("            <tr class=\"linePer\"" + GetMouseRoll() + ">");
                }
                else
                {
                    sb.AppendLine("            <tr class=\"line\"" + GetMouseRoll() + ">");
                }
                sb.AppendLine("            <td class=\"dic\">");
                sb.AppendLine("            &nbsp;");
                sb.AppendLine("            </td>");
                sb.AppendLine("                <td style=\" width:500px\" >");
                sb.AppendLine("                    <a href=\"" + System.Web.HttpUtility.UrlEncode(info.Text) + "/\">" + System.Web.HttpUtility.HtmlEncode(info.Text) + "</a>");
                sb.AppendLine("                </td>");
                sb.AppendLine("                <td >");
                if (info.Url == "..")
                {
                    sb.AppendLine("            &nbsp;");
                }
                else
                {
                    sb.AppendLine("                    <a href=\"" + System.Web.HttpUtility.UrlEncode(info.Text) + "/?type=lst\" target=\"_blank\">打包下载</a>");
                }
                sb.AppendLine("                </td>");
                sb.AppendLine("            </tr>");
            }
            return sb.ToString();
        }
        /// <summary>
        /// 生成目录的html
        /// </summary>
        /// <returns></returns>
        private string GenFileHtml(IList<DirInfo> lstFile)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < lstFile.Count; i++)
            {
                DirInfo info = lstFile[i];
                if (i % 2 == 1)
                {
                    sb.AppendLine("            <tr class=\"linePer\"" + GetMouseRoll() + ">");
                }
                else 
                {
                    sb.AppendLine("            <tr class=\"line\"" + GetMouseRoll() + ">");
                }

                sb.AppendLine("            <td class=\"dic\">");
                sb.AppendLine("            &nbsp;");
                sb.AppendLine("            </td>");
                sb.AppendLine("                <td style=\" width:500px\" >");
                sb.AppendLine("                    " + System.Web.HttpUtility.HtmlEncode(info.Text));
                sb.AppendLine("                </td>");
                sb.AppendLine("                <td style=\" width:500px\" >");
                sb.AppendLine("                    " + System.Web.HttpUtility.HtmlEncode(info.LenString));
                sb.AppendLine("                </td>");
                sb.AppendLine("                <td >");
                sb.AppendLine("                    <a href=\"" + System.Web.HttpUtility.UrlEncode(info.Url) + "\" target=\"_blank\">下载</a>");
                sb.AppendLine("                </td>");
                sb.AppendLine("            </tr>");
            }
            return sb.ToString();
        }
    }
}
