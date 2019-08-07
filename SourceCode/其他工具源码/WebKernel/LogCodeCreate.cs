using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Web;
namespace Buffalo.WebKernel.WebCommons
{
	/// <summary>
	/// LogCodeCreate 的摘要说明。
	/// </summary>
	public class LogCodeCreate
	{
		private LogCodeCreate()
		{
			//
			// TODO: 在此处添加构造函数逻辑
			//
		}

        /// <summary>
        /// 生成随机效验码字符串
        /// </summary>
        /// <returns>生成随机效验码字符串</returns>
        private static string GenerateCheckCode()
        {
            int number;
            string strCode = string.Empty;

            //随机数种子
            Random random = new Random();

            for (int i = 0; i < 4; i++)//效验码长度为4
            {
                //随机的整数
                number = random.Next();

                //字符从0-9,A-Z中随机产生，对应的ASCII码分别为
                //48－57，65－90
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
        /// 根据效验码输出图片
        /// </summary>
        /// <param name="checkCode">产生的随机效验码</param>
        private static Bitmap CreateCheckCodeImage(string checkCode)
        {
            //如果效验码为空，则直接返回
            if (checkCode == null || checkCode.Trim() == string.Empty)
                return null ;

            //根据效验码的长度确定输出图片的长度
            System.Drawing.Bitmap image = new System.Drawing.Bitmap((int)Math.Ceiling((double)(checkCode.Length * 15)), 20);
            //创建Graphics对象
            Graphics g = Graphics.FromImage(image);
            try
            {
                //生成随机数种子
                Random random = new Random();
                //清空图片背景色
                g.Clear(Color.White);

                //画图片的背景噪音线10条
                for (int i = 0; i < 10; i++)
                {
                    //噪音起点(X1,Y1)，终点(X2,Y2)
                    int X1 = random.Next(image.Width);
                    int Y1 = random.Next(image.Width);
                    int X2 = random.Next(image.Width);
                    int Y2 = random.Next(image.Width);
                    //用银色画出噪音线
                    g.DrawLine(new Pen(Color.Silver), X1, Y1, X2, Y2);
                }

                //输出图片中效验码的字体
                Font font = new Font("Arial", 12, (FontStyle.Bold | FontStyle.Italic));

                //线性减变画刷
                LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), Color.Blue, Color.Purple, 1.2f, true);

                g.DrawString(checkCode, font, brush, 2, 2);

                //画图片的背景噪音点50个
                for (int i = 0; i < 50; i++)
                {

                    int X = random.Next(image.Width);
                    int Y = random.Next(image.Height);
                    image.SetPixel(X, Y, Color.FromArgb(random.Next()));

                }

                //画图片的边筐线
                g.DrawRectangle(new Pen(Color.SaddleBrown), 0, 0, image.Width - 1, image.Height - 1);

                ////创建内存用于输出图片
                //using (MemoryStream ms = new MemoryStream())
                //{
                //    //图片格式指定为PNG
                //    image.Save(ms, ImageFormat.Png);
                //    //清除缓冲区的所有输出
                //    Response.ClearContent();
                //    //输出流的HTTP MIME类型设置为“image/Png”
                //    Response.ContentType = "image/Png";
                //    //输出图片的二进制流
                //    Response.BinaryWrite(ms.ToArray());
                //}
                
            }
            finally
            {
                //释放BITMAP对象和Graphics对象
                g.Dispose();
                //image.Dispose();
            }
            return image;
        }

        /// <summary>
        /// 获取下一张随机图片
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
        /// 获取当前的验证码
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
