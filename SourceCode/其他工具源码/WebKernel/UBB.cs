using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Buffalo.WebKernel.WebCommons
{
    public class UBB
    {
        private static string dvHTMLEncode(string fString)
        {
            if (fString != string.Empty)
            {
                fString.Replace("<", "&lt;");
                fString.Replace(">", "&rt;");
                fString.Replace(((char)34).ToString(), "&quot;");
                fString.Replace(((char)39).ToString(), "'");
                fString.Replace(((char)13).ToString(), "");
                fString.Replace(((char)10).ToString(), "<BR/> ");

            }
            return (fString);
        }
        private static string[] searcharray={"[/url]","[/email]","[/color]", "[/size]", "[/font]", "[/align]", "[b]", "[/b]","[i]", "[/i]", "[u]", "[/u]", "[list]", "[list=1]", "[list=a]","[list=A]", "[*]", "[/list]", "[indent]", "[/indent]",
            //"[code]","[/code]",
            "[quote]","[/quote]","[table]","[tr]","[td]","[/tr]","[/td]","[/table]"};
	    private static string[] replacearray={"</a>","</a>","</font>", "</font>", "</font>", "</div>", "<b>", "</b>", "<i>","</i>", "<u>", "</u>", "<ul>", "<ol type=1>", "<ol type=a>","<ol type=A>", "<li>", "</ul></ol>", "<blockquote>", "</blockquote>",
            //@"<div><textarea name=""codes"" id=""codes"" rows=""12"" cols=""65"">",
            //@"</textarea><br/><input type=""button"" value=""运行代码"" onclick=""RunCode()""> <input type=""button"" value=""复制代码"" onclick=""CopyCode()""> <input type=""button"" value=""另存代码"" onclick=""SaveCode()""> &nbsp;提示：您可以先修改部分代码再运行</div>",
            //@"<div style=""background:#E2F2FF;width:90%;height:300px;border:1px solid #3CAAEC"">",
            @"<div style=""background:#E2F2FF;width:90%;border:1px solid #3CAAEC"">",
            "</div>","<table>","<tr>","<td>","</tr>","</td>","</table>"};
        /// <summary>
        /// 把UBB代码解释成HTML代码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string UbbDecode(string str)
        {
            string chr = WebCommon.HTMLEncode(str);
            if (!string.IsNullOrEmpty(chr))
            {
                chr = Regex.Replace(chr, @"\[url=(?<x>[^\]]*)\](?<y>[^\]]*)\[/url\]", @"<a href=$1 target=_blank>$2</a>", RegexOptions.IgnoreCase);
                //图片UBB
                chr = Regex.Replace(chr, @"\[img\](http|https|ftp):\/\/(.[^\[]*)\[\/img\]", @"<a onfocus=""this.blur()"" href=""$1://$2"" target=new><img src=""$1://$2"" border=""0"" alt=""按此在新窗口浏览图片"" onload=""javascript:if(this.width>screen.width-333)this.width=screen.width-333""></a>");
                chr = Regex.Replace(chr, @"\[img=*([0-9]*),*([0-9]*)\](.+?)\[/img\]", "<img src=\"$3\" style=\"width:$1px;height:$2px\" alt=\"\" />");
                //链接UBB
                chr = Regex.Replace(chr, @"(\[url\])(.[^\[]*)(\[url\])", @"<a href=""$2"" target=""new"">$2</a>");
                chr = Regex.Replace(chr, @"\[url=(.[^\[]*)\]", @"<a href=""$1"" target=""new"">");
	            
	            //邮箱UBB
                chr = Regex.Replace(chr, @"(\[email\])(.*?)(\[\/email\])", @"<img align=""absmiddle"" ""src=image/email1.gif""><a href=""mailto:$2"">$2</a>");
                chr = Regex.Replace(chr, @"\[email=(.[^\[]*)\]", @"<img align=""absmiddle"" src=""image/email1.gif""><a href=""mailto:$1"" target=""new"">");
	            //QQ号码UBB
                //chr = Regex.Replace(chr, @"\[qq=([1-9]*)\]([1-9]*)\[\/qq\]", @"<a target=""new"" href=""tencent://message/?uin=$2&Site=www.mtm.net&Menu=yes""><img border=""0"" src=""http://wpa.qq.com/pa?p=1:$2:$1"" alt=""点击这里给我发消息""></a>");
                chr = Regex.Replace(chr, @"(\[qq\])(.*?)(\[\/qq\])", @"<a target=""new"" href=""tencent://message/?uin=$2&Site=www.mtm.net&Menu=yes""><img border=""0"" src=""http://wpa.qq.com/pa?p=1:$2:1"" alt=""点击这里给我发消息""></a>");
                //颜色UBB
                chr = Regex.Replace(chr, @"\[color=(.[^\[]*)\]", @"<font color=""$1"">");
	            //文字字体UBB
                chr = Regex.Replace(chr, @"\[font=(.[^\[]*)\]", @"<font face=""$1"">");
	            //文字大小UBB
                chr = Regex.Replace(chr, @"\[size=([1-7])\]", @"<font size=""$1"">");
	            //文字对齐方式UBB
                chr = Regex.Replace(chr, @"\[align=(center|left|right)\]", @"<div align=""$1"">");
	            //表格UBB
                chr = Regex.Replace(chr, @"\[table=(.[^\[]*)\]", @"<table width=""$1"">");

                //FLASH动画UBB
                chr = Regex.Replace(chr, @"(\[flash\])(http://.[^\[]*(.swf))(\[\/flash\])", @"<a href=""$2"" target=""new""><img src=""http://www.adobe.com/images/shared/product_mnemonics/20x20/flash_20x20.jpg"" border=""0"" alt=""点击开新窗口欣赏该flash动画!"" height=""16"" width=""16"">[全屏欣赏]</a><br><center><object codebase=""http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=4,0,2,0"" classid=""clsid:d27cdb6e-ae6d-11cf-96b8-444553540000"" width=""300"" height=""200""><param name=""movie"" value=""$2""><param name=""quality"" value=""high""><embed src=""$2"" quality=""high"" pluginspage=""http://www.macromedia.com/shockwave/download/index.cgi?p1_prod_version=shockwaveflash"" type=""application/x-shockwave-flash"" width=""300"" height=""200"">$2</embed></object></center>");

                chr = Regex.Replace(chr, @"(\[flash=*([0-9]*),*([0-9]*)\])(http://.[^\[]*(.swf))(\[\/flash\])", @"<a href=""$4"" target=""new""><img src=""http://www.adobe.com/images/shared/product_mnemonics/20x20/flash_20x20.jpg"" border=""0"" alt=""点击开新窗口欣赏该flash动画!"" height=""16"" width=""16"">[全屏欣赏]</a><br><center><object codebase=""http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=4,0,2,0"" classid=""clsid:d27cdb6e-ae6d-11cf-96b8-444553540000"" width=""$2"" height=""$3""><param name=""movie"" value=""$4""><param name=quality value=high><embed src=""$4"" quality=""high"" pluginspage=""http://www.macromedia.com/shockwave/download/index.cgi?p1_prod_version=shockwaveflash"" type=""application/x-shockwave-flash"" width=""$2"" height=""$3"">$4</embed></object></center>");
                //引用
                chr = Regex.Replace(chr, @"(\[quote=(.[^\[]*)\,(.[^\[]*)\])", @"<div style=""background:$2;width:90%;border:1px solid $3"">");
               
                
                //MEDIA PLAY播放UBB
                chr = Regex.Replace(chr, @"\[wmv\](.[^\[]*)\[\/wmv]", @"<object align=""middle"" classid=""clsid:22d6f312-b0f6-11d0-94ab-0080c74c7e95"" class=""object"" id=""mediaplayer"" width=""300"" height=""200"" ><param name=""showstatusbar"" value=""-1""><param name=""filename"" value=""$1""><embed type=""application/x-oleobject"" codebase=""http://activex.microsoft.com/activex/controls/mplayer/en/nsmp2inf.cab#version=5,1,52,701"" flename=""mp"" src=""$1""  width=""300"" height=""200""></embed></object>");
                chr = Regex.Replace(chr, @"\[wmv=*([0-9]*),*([0-9]*)\](.[^\[]*)\[\/wmv]", @"<object align=""middle"" classid=""clsid:22d6f312-b0f6-11d0-94ab-0080c74c7e95"" class=""object"" id=""mediaplayer"" width=""$1"" height=""$2"" ><param name=""showstatusbar"" value=""-1""><param name=""filename"" value=""$3""><embed type=""application/x-oleobject"" codebase=""http://activex.microsoft.com/activex/controls/mplayer/en/nsmp2inf.cab#version=5,1,52,701"" flename=""mp"" src=""$3""  width=""$1"" height=""$2""></embed></object>");
                
	            //REALPLAY 播放UBB
                chr = Regex.Replace(chr, @"\[rm\](.[^\[]*)\[\/rm]", @"<object classid=""clsid:cfcdaa03-8be4-11cf-b84b-0020afbbccfa"" class=""object"" id=""raocx"" width=""300"" height=""200""><param name=""src"" value=""$1""><param name=""console"" value=""clip1""><param name=""controls"" value=""imagewindow""><param name=""autostart"" value=""true""></object><br><object classid=""clsid:cfcdaa03-8be4-11cf-b84b-0020afbbccfa"" height=""32"" id=""video2"" width=""300""><param name=""src"" value=""$1""><param name=""autostart"" value=""-1""><param name=""controls"" value=""controlpanel""><param name=""console"" value=""clip1""></object>");
                chr = Regex.Replace(chr, @"\[rm=*([0-9]*),*([0-9]*)\](.[^\[]*)\[\/rm]", @"<object classid=""clsid:cfcdaa03-8be4-11cf-b84b-0020afbbccfa"" class=""object"" id=""raocx"" width=""$1"" height=""$2""><param name=""src"" value=""$3""><param name=""console"" value=""clip1""><param name=""controls"" value=""imagewindow""><param name=""autostart"" value=""true""></object><br><object classid=""clsid:cfcdaa03-8be4-11cf-b84b-0020afbbccfa"" height=""32"" id=""video2"" width=""$1""><param name=""src"" value=""$3""><param name=""autostart"" value=""-1""><param name=""controls"" value=""controlpanel""><param name=""console"" value=""clip1""></object>");

                for(int i=0;i<searcharray.Length;i++)
                {
                    chr=chr.Replace(searcharray[i],replacearray[i]);
                }
            }
            return (chr);
        }
    }
    
}
