using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;
namespace Buffalo.Kernel
{
    /// <summary>
    /// �ʼ�������
    /// </summary>
    public class MailSender
    {
        private static string mailSource = null;

        /// <summary>
        /// ����Դ����
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
        /// ����Դ�����¼��
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
        /// ����Դ�����¼��
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
        /// ����Դ�����¼��
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
        /// �����ʼ�
        /// </summary>
        /// <param name="mailTo">���͵�</param>
        /// <param name="title">����</param>
        /// <param name="contact">����</param>
        public static void SendMail(string mailTo, string title,string contact)
        {
            //�����ʼ�,ֱ�ӷ��ʹ����û�����������ʼ�����Ϊ����û�м��ܡ�
            SmtpClient client = new SmtpClient(MailSmtp);
            
            client.UseDefaultCredentials = false;
            //������û���������д�Լ���163���û������룬Ҳ�����޸������SMTP������
            if (MailUser != null)
            {
                client.Credentials = new System.Net.NetworkCredential(MailUser, MailPass);
            }
            
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            //����MailMessage���󣬲����ֱ�Ϊ�����˵�ַ���ż����⣬�ż�����
            MailMessage message = new MailMessage(MailSource, mailTo,title, contact);
            message.BodyEncoding = System.Text.Encoding.GetEncoding("gb2312");    //����
            
            message.IsBodyHtml = true; //�Ƿ���HTML����
            client.Send(message);   //����
        }


        
    }
}
