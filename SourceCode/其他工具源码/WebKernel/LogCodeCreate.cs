using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Web;
namespace Buffalo.WebKernel.WebCommons
{
	/// <summary>
	/// LogCodeCreate ��ժҪ˵����
	/// </summary>
	public class LogCodeCreate
	{
		private LogCodeCreate()
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
			//
		}

        /// <summary>
        /// �������Ч�����ַ���
        /// </summary>
        /// <returns>�������Ч�����ַ���</returns>
        private static string GenerateCheckCode()
        {
            int number;
            string strCode = string.Empty;

            //���������
            Random random = new Random();

            for (int i = 0; i < 4; i++)//Ч���볤��Ϊ4
            {
                //���������
                number = random.Next();

                //�ַ���0-9,A-Z�������������Ӧ��ASCII��ֱ�Ϊ
                //48��57��65��90
                number = number % 10;

                //if (number < 10)
                //{
                //    number += 48;
                //}
                //else
                //{
                //    number += 55;
                //}
                strCode += number.ToString();
            }
            return strCode;
        }

        /// <summary>
        /// ����Ч�������ͼƬ
        /// </summary>
        /// <param name="checkCode">���������Ч����</param>
        private static Bitmap CreateCheckCodeImage(string checkCode)
        {
            //���Ч����Ϊ�գ���ֱ�ӷ���
            if (checkCode == null || checkCode.Trim() == string.Empty)
                return null ;

            //����Ч����ĳ���ȷ�����ͼƬ�ĳ���
            System.Drawing.Bitmap image = new System.Drawing.Bitmap((int)Math.Ceiling((double)(checkCode.Length * 15)), 20);
            //����Graphics����
            Graphics g = Graphics.FromImage(image);
            try
            {
                //�������������
                Random random = new Random();
                //���ͼƬ����ɫ
                g.Clear(Color.White);

                //��ͼƬ�ı���������10��
                for (int i = 0; i < 10; i++)
                {
                    //�������(X1,Y1)���յ�(X2,Y2)
                    int X1 = random.Next(image.Width);
                    int Y1 = random.Next(image.Width);
                    int X2 = random.Next(image.Width);
                    int Y2 = random.Next(image.Width);
                    //����ɫ����������
                    g.DrawLine(new Pen(Color.Silver), X1, Y1, X2, Y2);
                }

                //���ͼƬ��Ч���������
                Font font = new Font("Arial", 12, (FontStyle.Bold | FontStyle.Italic));

                //���Լ��仭ˢ
                LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), Color.Blue, Color.Purple, 1.2f, true);

                g.DrawString(checkCode, font, brush, 2, 2);

                //��ͼƬ�ı���������50��
                for (int i = 0; i < 50; i++)
                {

                    int X = random.Next(image.Width);
                    int Y = random.Next(image.Height);
                    image.SetPixel(X, Y, Color.FromArgb(random.Next()));

                }

                //��ͼƬ�ı߿���
                g.DrawRectangle(new Pen(Color.SaddleBrown), 0, 0, image.Width - 1, image.Height - 1);

                ////�����ڴ��������ͼƬ
                //using (MemoryStream ms = new MemoryStream())
                //{
                //    //ͼƬ��ʽָ��ΪPNG
                //    image.Save(ms, ImageFormat.Png);
                //    //������������������
                //    Response.ClearContent();
                //    //�������HTTP MIME��������Ϊ��image/Png��
                //    Response.ContentType = "image/Png";
                //    //���ͼƬ�Ķ�������
                //    Response.BinaryWrite(ms.ToArray());
                //}
                
            }
            finally
            {
                //�ͷ�BITMAP�����Graphics����
                g.Dispose();
                //image.Dispose();
            }
            return image;
        }

        /// <summary>
        /// ��ȡ��һ�����ͼƬ
        /// </summary>
        /// <returns></returns>
        public static Bitmap NextCodeImage() 
        {
            HttpContext.Current.Session["Code"] = GenerateCheckCode();
            string str = HttpContext.Current.Session["Code"].ToString();
            //Response.ContentType="application/octet-stream";
            Bitmap bp = CreateCheckCodeImage(str);
            return bp;
        }

        /// <summary>
        /// ��ȡ��ǰ����֤��
        /// </summary>
        public static string CurrentCode 
        {
            get
            {
                string ret = null;
                if (HttpContext.Current.Session["Code"] != null)
                {
                    ret = HttpContext.Current.Session["Code"].ToString();
                }
                return ret;
            }
        }
	}
}
