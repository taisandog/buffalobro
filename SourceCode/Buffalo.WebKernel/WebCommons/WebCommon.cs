using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI.HtmlControls;

using System.Web.UI;

using System.Collections;
using Buffalo.Kernel.FastReflection;
using System.Web.UI.WebControls;
using System.Collections.Concurrent;

namespace Buffalo.WebKernel.WebCommons
{
    public class WebCommon
    {
        
        /// <summary>
        /// ��ԭ��ת����HTML����
        /// </summary>
        /// <param name="str">ԭ��</param>
        /// <returns></returns>
        public static string HTMLEncode(object objStr)
        {
            if (objStr == null)
            {
                return "";
            }
            string str = objStr.ToString();
            //&����
            str = str.Replace("&", "&amp;");
            // ����ո�
            str = str.Replace(" ", "&nbsp;");
            //����˫����
            str = str.Replace("\"", "&quot;");
            //html��Ƿ�
            str = str.Replace("<", "&lt;");
            str = str.Replace(">", "&gt;");
            str = str.Replace("\'", "&#39;");
            str = str.Replace("\"", "&quot;");
            //����
            str = str.Replace("\r\n", "<br/>");
            str = str.Replace("\n", "<br/>");
            return str;
        }

        private static ConcurrentDictionary<string, string> _dicContentType = null;

        /// <summary>
        /// ��ʼ������
        /// </summary>
        /// <returns></returns>
        private static ConcurrentDictionary<string, string> InitContentType() 
        {
            ConcurrentDictionary<string, string> dic = new ConcurrentDictionary<string, string>();
            dic["ez"] = "application/andrew-inset";
            dic["hqx"] = "application/mac-binhex40";
            dic["cpt"] = "application/mac-compactpro";
            dic["doc"] = "application/msword";
            dic["bin"] = "application/octet-stream";
            dic["dms"] = "application/octet-stream";
            dic["lha"] = "application/octet-stream";
            dic["lzh"] = "application/octet-stream";
            dic["exe"] = "application/octet-stream";
            dic["class"] = "application/octet-stream";
            dic["so"] = "application/octet-stream";
            dic["dll"] = "application/octet-stream";
            dic["oda"] = "application/oda";
            dic["pdf"] = "application/pdf";
            dic["ai"] = "application/postscript";
            dic["eps"] = "application/postscript";
            dic["ps"] = "application/postscript";
            dic["smi"] = "application/smil";
            dic["smil"] = "application/smil";
            dic["mif"] = "application/vnd.mif";
            dic["xls"] = "application/vnd.ms-excel";
            dic["ppt"] = "application/vnd.ms-powerpoint";
            dic["wbxml"] = "application/vnd.wap.wbxml";
            dic["wmlc"] = "application/vnd.wap.wmlc";
            dic["wmlsc"] = "application/vnd.wap.wmlscriptc";
            dic["bcpio"] = "application/x-bcpio";
            dic["vcd"] = "application/x-cdlink";
            dic["pgn"] = "application/x-chess-pgn";
            dic["cpio"] = "application/x-cpio";
            dic["csh"] = "application/x-csh";
            dic["dcr"] = "application/x-director";
            dic["dir"] = "application/x-director";
            dic["dxr"] = "application/x-director";
            dic["dvi"] = "application/x-dvi";
            dic["spl"] = "application/x-futuresplash";
            dic["gtar"] = "application/x-gtar";
            dic["hdf"] = "application/x-hdf";
            dic["js"] = "application/x-javascript";
            dic["skp"] = "application/x-koan";
            dic["skd"] = "application/x-koan";
            dic["skt"] = "application/x-koan";
            dic["skm"] = "application/x-koan";
            dic["latex"] = "application/x-latex";
            dic["nc"] = "application/x-netcdf";
            dic["cdf"] = "application/x-netcdf";
            dic["sh"] = "application/x-sh";
            dic["shar"] = "application/x-shar";
            dic["swf"] = "application/x-shockwave-flash";
            dic["sit"] = "application/x-stuffit";
            dic["sv4cpio"] = "application/x-sv4cpio";
            dic["sv4crc"] = "application/x-sv4crc";
            dic["tar"] = "application/x-tar";
            dic["tcl"] = "application/x-tcl";
            dic["tex"] = "application/x-tex";
            dic["texinfo"] = "application/x-texinfo";
            dic["texi"] = "application/x-texinfo";
            dic["t"] = "application/x-troff";
            dic["tr"] = "application/x-troff";
            dic["roff"] = "application/x-troff";
            dic["man"] = "application/x-troff-man";
            dic["me"] = "application/x-troff-me";
            dic["ms"] = "application/x-troff-ms";
            dic["ustar"] = "application/x-ustar";
            dic["src"] = "application/x-wais-source";
            dic["xhtml"] = "application/xhtml+xml";
            dic["xht"] = "application/xhtml+xml";
            dic["zip"] = "application/zip";
            dic["au"] = "audio/basic";
            dic["snd"] = "audio/basic";
            dic["mid"] = "audio/midi";
            dic["midi"] = "audio/midi";
            dic["kar"] = "audio/midi";
            dic["mpga"] = "audio/mpeg";
            dic["mp2"] = "audio/mpeg";
            dic["mp3"] = "audio/mpeg";
            dic["aif"] = "audio/x-aiff";
            dic["aiff"] = "audio/x-aiff";
            dic["aifc"] = "audio/x-aiff";
            dic["m3u"] = "audio/x-mpegurl";
            dic["ram"] = "audio/x-pn-realaudio";
            dic["rm"] = "audio/x-pn-realaudio";
            dic["rpm"] = "audio/x-pn-realaudio-plugin";
            dic["ra"] = "audio/x-realaudio";
            dic["wav"] = "audio/x-wav";
            dic["pdb"] = "chemical/x-pdb";
            dic["xyz"] = "chemical/x-xyz";
            dic["bmp"] = "image/bmp";
            dic["gif"] = "image/gif";
            dic["ief"] = "image/ief";
            dic["jpeg"] = "image/jpeg";
            dic["jpg"] = "image/jpeg";
            dic["jpe"] = "image/jpeg";
            dic["png"] = "image/png";
            dic["tiff"] = "image/tiff";
            dic["tif"] = "image/tiff";
            dic["djvu"] = "image/vnd.djvu";
            dic["djv"] = "image/vnd.djvu";
            dic["wbmp"] = "image/vnd.wap.wbmp";
            dic["ras"] = "image/x-cmu-raster";
            dic["pnm"] = "image/x-portable-anymap";
            dic["pbm"] = "image/x-portable-bitmap";
            dic["pgm"] = "image/x-portable-graymap";
            dic["ppm"] = "image/x-portable-pixmap";
            dic["rgb"] = "image/x-rgb";
            dic["xbm"] = "image/x-xbitmap";
            dic["xpm"] = "image/x-xpixmap";
            dic["xwd"] = "image/x-xwindowdump";
            dic["igs"] = "model/iges";
            dic["iges"] = "model/iges";
            dic["msh"] = "model/mesh";
            dic["mesh"] = "model/mesh";
            dic["silo"] = "model/mesh";
            dic["wrl"] = "model/vrml";
            dic["vrml"] = "model/vrml";
            dic["css"] = "text/css";
            dic["html"] = "text/html";
            dic["htm"] = "text/html";
            dic["asc"] = "text/plain";
            dic["txt"] = "text/plain";
            dic["rtx"] = "text/richtext";
            dic["rtf"] = "text/rtf";
            dic["sgml"] = "text/sgml";
            dic["sgm"] = "text/sgml";
            dic["tsv"] = "text/tab-separated-values";
            dic["wml"] = "text/vnd.wap.wml";
            dic["wmls"] = "text/vnd.wap.wmlscript";
            dic["etx"] = "text/x-setext";
            dic["xsl"] = "text/xml";
            dic["xml"] = "text/xml";
            dic["mpeg"] = "video/mpeg";
            dic["mpg"] = "video/mpeg";
            dic["mpe"] = "video/mpeg";
            dic["qt"] = "video/quicktime";
            dic["mov"] = "video/quicktime";
            dic["mxu"] = "video/vnd.mpegurl";
            dic["avi"] = "video/x-msvideo";
            dic["movie"] = "video/x-sgi-movie";
            dic["ice"] = "x-conference/x-cooltalk";
            return dic;
        }
        /// <summary>
        /// ��ȡ�������
        /// </summary>
        /// <param name="exName">��չ��</param>
        /// <returns></returns>
        public static string GetContentType(string exName) 
        {
            if (_dicContentType == null) 
            {
                _dicContentType = InitContentType();
            }
            if (string.IsNullOrEmpty(exName)) 
            {
                return "application/octet-stream";
            }
            string ret = null;
            if (_dicContentType.TryGetValue(exName.ToLower(), out ret)) 
            {
                return ret;
            }
            return "application/octet-stream";
        }

        /// <summary>
        /// ��ȡ�ͻ��˵�IP
        /// </summary>
        /// <returns></returns>
        public static string GetClientIP()
        {
            string result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (string.IsNullOrEmpty(result))
            {
                result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }

            if (string.IsNullOrEmpty(result))
            {
                result = HttpContext.Current.Request.UserHostAddress;
            }
            return result;

        }
        /// <summary>
        /// ��HTML����ת����ԭ��
        /// </summary>
        /// <param name="strHTML">html����</param>
        /// <returns></returns>
        public static string HTMLDecode(string str)
        {
            if (str == null)
            {
                return "";
            }
            //html��Ƿ�
            str = str.Replace("&lt;", "<");
            str = str.Replace("&gt;", ">");
            // ����ո�
            str = str.Replace(" ", "&nbsp;");
            //����˫����
            str = str.Replace("&quot;", "\"");
            str = str.Replace("&#39;", "\'");
            str = str.Replace("&quot;", "\"");
            //&����
            str = str.Replace("&amp;", "&");
            return str;
        }
        /// <summary>
        /// ��ʽ�����/�����ʾ�ַ���
        /// </summary>
        /// <param name="str">�ַ���</param>
        /// <param name="count">����</param>
        /// <returns></returns>
        public static string ForamtStringCount(object str, int count) 
        {
            string strRet = ToBreakWord(str, count);
            strRet = HTMLEncode(strRet);
            return strRet;
        }
        /// <summary>
        /// ���ַ����Զ��ӿո����ñ���Զ�����
        /// </summary>
        /// <param name="str">�ַ���</param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static string ToBreakWord(object str, int count) 
        {
            if (str == null) 
            {
                return "";
            }
            if(count<=0)
            {
                return str.ToString();
            }
            int linemax = count - 1;
            string strSource = str.ToString();
            StringBuilder sbRet = new StringBuilder();
            for (int i=0;i<strSource.Length;i++) 
            {
                char chr = strSource[i];
                sbRet.Append(chr);
                if ((i > 0) && (i % linemax) == 0) 
                {
                    sbRet.Append(' ');
                }
            }
            return sbRet.ToString();
        }

        /// <summary>
        /// ��ȡϵͳ������Ŀ¼
        /// </summary>
        /// <returns></returns>
        public static string GetVirPath()
        {
            string appPath = HttpContext.Current.Request.ApplicationPath;
            if (appPath == "/" || appPath == "//")
            {
                appPath = "";
            }
            
            return appPath;
        }

        /// <summary>
        /// ��ǰҳ����
        /// </summary>
        public static string CurrentPageName 
        {
            get 
            {
                string pageName = HttpContext.Current.Request.Url.LocalPath;
                pageName = pageName.Substring(pageName.LastIndexOf("/", pageName.Length) + 1);
                return pageName;
            }
        }

        /// <summary>
        /// ��ȡϵͳ������+����Ŀ¼
        /// </summary>
        /// <returns></returns>
        public static string GetVirRoot()
        {
            string port = "";
            if (HttpContext.Current.Request.Url.Port != 80)
            {
                port = ":"+HttpContext.Current.Request.Url.Port.ToString();
            }

            string url = "http://" + HttpContext.Current.Request.Url.Host + port + GetVirPath();
            return url;
        }

        /// <summary>
        /// �Ƿ��������չ���ϴ�
        /// </summary>
        /// <param name="hifile">�ϴ��ؼ�</param>
        /// <returns></returns>
        public static bool IsAllowedExtension(HttpPostedFile hifile)
        {
            if (hifile.FileName == "" || hifile.FileName == null)
            {
                return false;
            }
            string strOldFilePath = "", strExtension = "";

            //�����ϴ�����չ�������Ըĳɴ������ļ��ж���
            string[] arrExtension = { ".gif", ".jpg", ".jpeg", ".bmp", ".png" };

            if (hifile.FileName != string.Empty)
            {
                strOldFilePath = hifile.FileName;
                //ȡ���ϴ��ļ�����չ��
                strExtension = strOldFilePath.Substring(strOldFilePath.LastIndexOf("."));
                //�жϸ���չ���Ƿ�Ϸ�
                for (int i = 0; i < arrExtension.Length; i++)
                {
                    if (strExtension.ToLower() == arrExtension[i])
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// �ж��ϴ��ļ���С�Ƿ񳬹����ֵ
        /// </summary>
        /// <param name="hifile">HtmlInputFile�ؼ�</param>
        /// <returns>�������ֵ����false,���򷵻�true.</returns>
        public static bool IsAllowedLength(HttpPostedFile hifile)
        {

            //�����ϴ��ļ���С�����ֵ,���Ա�����xml�ļ���,��λΪKB
            int i = 2 * 1024;
            //����ϴ��ļ��Ĵ�С�������ֵ,����flase,���򷵻�true.
            if (hifile.ContentLength > i * 1024)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// ���ļ�д���ش�����
        /// </summary>
        /// <param name="file"></param>
        /// <param name="fileName"></param>
        public static void ShowFile(byte[] file, string fileName)
        {
            ShowFile("application/octet-stream", file, fileName);
        }

        /// <summary>
        /// ���ļ�д���ش�����
        /// </summary>
        /// <param name="file"></param>
        /// <param name="fileName"></param>
        public static void ShowFile(string contentType,byte[] file,string fileName) 
        {
            HttpContext.Current.Response.ContentType = contentType;
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpContext.Current.Server.UrlEncode(fileName));

            HttpContext.Current.Response.OutputStream.Write(file, 0, file.Length);
        }
        
        /// <summary>
        /// ��ʽ��JS���ַ���
        /// </summary>
        /// <param name="js"></param>
        /// <returns></returns>
        public static string JSStringEncode(string js)
        {
            js = js.Replace("\"", "\\\"");
            js = js.Replace("\r", "\\r");
            js = js.Replace("\n", "\\n");
            return js;
        }
    }
}
