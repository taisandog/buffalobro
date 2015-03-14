using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace WordFilter
{
    public class WordPicture
    {
        private const float PI = 3.14159265358979f;
        private const float PI2 = 6.28318530717959f;
        public WordPicture() { }
        public WordPicture(Color bcolor, Color fcolor, Font font) 
        {

        }
        Color _bcolor = Color.White;
        /// <summary>
        /// 背景色
        /// </summary>
        public Color Bcolor
        {
            get { return _bcolor; }
            set { _bcolor = value; }
        }
        Color _fcolor = Color.Black;

        /// <summary>
        /// 文字颜色
        /// </summary>
        public Color Fcolor
        {
            get { return _fcolor; }
            set { _fcolor = value; }
        }
        Font _font = new Font("宋体", 12f);

        /// <summary>
        /// 文字字体
        /// </summary>
        public Font Font
        {
            get { return _font; }
            set { _font = value; }
        }

        int _lineAlpha = 255;

        /// <summary>
        /// 干扰线透明度
        /// </summary>
        public int LineAlpha
        {
            get { return _lineAlpha; }
            set { _lineAlpha = value; }
        }

        int _maxCharacter = 18;
        /// <summary>
        /// 每行最大文字数
        /// </summary>
        public int MaxCharacter
        {
            get { return _maxCharacter; }
            set { _maxCharacter = value; }
        }

        int _twist  = 1;
        /// <summary>
        /// 每行最大文字数
        /// </summary>
        public int Twist
        {
            get { return _twist; }
            set { _twist = value; }
        }
        /// <summary>
        /// 画图片
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public Bitmap DrawWordPicture(string content) 
        {
            return DrawWordPicture(content, _maxCharacter, _font, _fcolor, _bcolor, _lineAlpha, _twist);
        }

        /// <summary>
        /// 获取换行符
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        private static string LineMark(string content) 
        {
            if(string.IsNullOrEmpty(content))
            {
                return "\n";
            }
            if(content.IndexOf("\r\n")>=0)
            {
                return "\r\n";
            }
            if (content.IndexOf("\r") >= 0)
            {
                return "\r";
            }
            return "\n";
            
        }

        /// <summary>
        /// 文字转图片
        /// </summary>
        /// <param name="content"></param>
        /// <param name="maxCharacter"></param>
        /// <param name="font"></param>
        /// <param name="fcolor"></param>
        /// <param name="bcolor"></param>
        /// <param name="lineAlpha"></param>
        /// <returns></returns>
        public static Bitmap DrawWordPicture(string content, int maxCharacter, Font font, Color fcolor, Color bcolor, int lineAlpha, int twist)
        {

            Random rnd1 = new Random();
            int fHeight = (int)font.GetHeight();
            int fWidth = (int)font.GetHeight();
            fHeight = (int)(fWidth);
            string[] lines = content.Split(new string[] { LineMark(content) }, StringSplitOptions.None);
            maxCharacter = maxCharacter * 2;
            List<string> readLines = new List<string>();

            StringBuilder tmp = new StringBuilder(maxCharacter);

            foreach (string str in lines)
            {
                int chars = 0;
                Queue<char> que = new Queue<char>(str);
                while (que.Count > 0)
                {
                    while (chars < maxCharacter && que.Count > 0)
                    {
                        char chr = que.Dequeue();

                        tmp.Append(chr);
                        chars++;
                        if ((int)chr > 256)
                        {
                            chars++;
                        }
                    }
                    readLines.Add(tmp.ToString());
                    tmp.Remove(0, tmp.Length);
                    chars = 0;
                }
            }


            int width = 0;
            foreach (string str in readLines)
            {
                int cw = System.Text.Encoding.Default.GetBytes(str).Length * fWidth / 2;
                if (width < cw)
                {
                    width = cw;
                }
            }
            width += 10;
            int height = fHeight * readLines.Count;

            //创建一个图像
            //定义画笔，用于绘制文字
            Brush brush1 = new SolidBrush(Color.Black);
            Bitmap bitmap1 = new System.Drawing.Bitmap(width, height);
            
            //从图像获取一个绘画面
            using (Graphics graphics1 = Graphics.FromImage(bitmap1))
            {
                //清除整个绘图画面并用颜色填充
                graphics1.Clear(bcolor);
                

                PointF Cpoint1 = new PointF(5, 5);
                int y1 = 0;


                int lineWidth = 5;

                brush1 = new SolidBrush(fcolor);
                //绘制文字
                for (int i = 0; i < readLines.Count; i++)
                {
                    using (Bitmap lineImg = new Bitmap(bitmap1.Width, fHeight))
                    {
                        using (Graphics g = Graphics.FromImage(lineImg))
                        {
                            y1 = fWidth * i;
                            for (int j = 0; j < readLines[i].Length; j++)
                            {

                                Cpoint1 = new PointF(lineWidth, 0);

                                char chr = readLines[i][j];
                                g.DrawString(chr.ToString(), font, brush1, Cpoint1);
                                if ((int)chr > 256)
                                {
                                    lineWidth += fWidth;
                                }
                                else
                                {
                                    lineWidth += fWidth / 2;
                                }
                            }
                            Image ret = TwistImage(lineImg, true, twist, twist, bcolor);
                            graphics1.DrawImage(ret, new Point(0, y1));
                        }


                    }

                    lineWidth = 5;
                }
                if (readLines.Count >= 3)
                {
                    DrawLines(80, bitmap1.Size, graphics1, rnd1, readLines.Count);
                }
                DrawPoint(bitmap1, content.Length, rnd1);
            }


            return bitmap1;

        }

        /// <summary>
        /// 扭曲图片
        /// </summary>
        /// <param name="srcBmp">源图片</param>
        /// <param name="bXDir"></param>
        /// <param name="dMultValue"></param>
        /// <param name="dPhase"></param>
        /// <returns></returns>
        private static Bitmap TwistImage(Bitmap srcBmp, bool bXDir, double dMultValue, double dPhase,Color backColor)
        {
            int width =srcBmp.Width ;
            Bitmap destBmp = new Bitmap(width, srcBmp.Height);
            using (Graphics g = Graphics.FromImage(destBmp)) 
            {
                g.Clear(backColor);
            }
            double dBaseAxisLen = bXDir ? Convert.ToDouble(destBmp.Height): Convert.ToDouble(destBmp.Width);
            for (int i = 0; i < srcBmp.Width; i++)
            {
                for (int j = 0; j < srcBmp.Height; j++)
                {
                    double dx = 0;
                    dx = bXDir ?  PI2 * Convert.ToDouble(j) / dBaseAxisLen : PI2 * Convert.ToDouble(i) / dBaseAxisLen;
                    dx += dPhase;
                    double dy = Math.Sin(dx);
                    //取得当前点的颜色
                    int nOldX  = 0;
                    int nOldY  = 0;
                    nOldX = bXDir ? i + Convert.ToInt32(dy * dMultValue) : i;
                    nOldY = bXDir ? j: j + Convert.ToInt32(dy * dMultValue);
                    System.Drawing.Color color = srcBmp.GetPixel(i, j);
                    //if (i == 0 && j == 0 && nOldX<0)
                    //{
                    //    star = 0 - nOldX;

                    //}
                    if (nOldX >= 0 && nOldX < destBmp.Width && nOldY >= 0 && nOldY < destBmp.Height )
                    {
                         destBmp.SetPixel(nOldX, nOldY, color);
                    }
                }
            }
            return destBmp;
        }

        /// <summary>
        /// 绘制干扰点
        /// </summary>
        /// <param name="bmp"></param>
        private static void DrawPoint(Bitmap bmp, int len, Random rnd1) 
        {
            int x1 = 0, y1 = 0;
            for (int i = 0; i < 10 * len; i++)
            {
                //随机干扰位置
                x1 = rnd1.Next(bmp.Width);
                y1 = rnd1.Next(bmp.Height);
                bmp.SetPixel(x1, y1, Color.FromArgb(rnd1.Next(50), rnd1.Next(50), rnd1.Next(50)));
            }
        }


        /// <summary>
        /// 绘制干扰线
        /// </summary>
        /// <param name="bmp"></param>
        private static void DrawLines(int alpha,Size size, Graphics graphics1, Random rnd1, int lines)
        {
            int x1 = 0, y1 = 0;
            lines = (int)Math.Max(lines * 2, 10);

            for (int i = 0; i < lines; i++)
            {
                //随机干扰位置
                x1 = rnd1.Next(10);
                y1 = rnd1.Next(size.Height);
                int x2 = size.Width-rnd1.Next(10);
                int y2 = rnd1.Next(size.Height);
                //随机干扰线颜色
                Pen pen1 = new Pen(Color.FromArgb(alpha, rnd1.Next(0, 50), rnd1.Next(0, 50), rnd1.Next(0, 50)));
                
                graphics1.DrawLine(pen1, x1, y1, x2, y2);
            }
        }
    }
}
