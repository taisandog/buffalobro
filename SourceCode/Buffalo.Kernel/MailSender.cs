using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;
namespace Buffalo.Kernel
{
    /// <summary>
    /// 邮件发送类
    /// </summary>
    public class MailSender
    {
        private static string mailSource = null;

        /// <summary>
        /// 发送源邮箱
        /// </summary>
        private static string MailSource
        {
            get 
            {
                if (mailSource == null)
                {
                    mailSource = System.Configuration.ConfigurationManager.AppSettings["MailSource"];
                }
                return mailSource; 
            }
            
        }

        private static string mailUser = null;

        /// <summary>
        /// 发送源邮箱登录名
        /// </summary>
        private static string MailUser
        {
            get
            {
                if (mailUser == null)
                {
                    mailUser = System.Configuration.ConfigurationManager.AppSettings["MailUser"];
                }
                return mailUser;
            }

        }

        private static string mailPass = null;

        /// <summary>
        /// 发送源邮箱登录名
        /// </summary>
        private static string MailPass
        {
            get
            {
                if (mailPass == null)
                {
                    mailPass = System.Configuration.ConfigurationManager.AppSettings["MailPass"];
                }
                return mailPass;
            }

        }
        private static string mailSmtp = null;

        /// <summary>
        /// 发送源邮箱登录名
        /// </summary>
        private static string MailSmtp
        {
            get
            {
                if (mailSmtp == null)
                {
                    mailSmtp = System.Configuration.ConfigurationManager.AppSettings["MailSMTP"];
                }
                return mailSmtp;
            }

        }
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="mailTo">发送到</param>
        /// <param name="title">标题</param>
        /// <param name="contact">正文</param>
        public static void SendMail(string mailTo, string title,string contact)
        {
            //发送邮件,直接发送带有用户名，密码的邮件，因为密码没有加密。
            SmtpClient client = new SmtpClient(MailSmtp);
            
            client.UseDefaultCredentials = false;
            //下面的用户名密码填写自己在163的用户名密码，也可以修改上面的SMTP服务器
            if (MailUser != null)
            {
                client.Credentials = new System.Net.NetworkCredential(MailUser, MailPass);
            }
            
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            //创建MailMessage对象，参数分别为发件人地址，信件标题，信件正文
            MailMessage message = new MailMessage(MailSource, mailTo,title, contact);
            message.BodyEncoding = System.Text.Encoding.GetEncoding("gb2312");    //编码
            
            message.IsBodyHtml = true; //是否是HTML代码
            client.Send(message);   //发送
        }


        
    }
}
