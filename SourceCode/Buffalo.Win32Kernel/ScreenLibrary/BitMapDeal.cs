using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Buffalo.Win32Kernel.ScreenLibrary
{
    public delegate void OpenBitmatWndHandler(Bitmap bmp);

    /// <summary>
    /// 表示調整大小模式
    /// </summary>
    public enum ResizeType
    {
        Move,
        Left,
        Right,
        Top,
        Bottom,
        LeftTop,
        RightTop,
        LeftBottom,
        RightBottom
    }
    public static class BitMapDeal
    {
        /// <summary>
        /// 截取屏幕
        /// </summary>
        /// <param name="rect">开始截取位置</param>
        /// <returns></returns>
        public static Image PrintScreen(Rectangle rect)
        {
            //Rectangle rect = Screen.PrimaryScreen.Bounds;
            Image memory = new Bitmap(rect.Width, rect.Height);
            Graphics g = Graphics.FromImage(memory);
            g.CopyFromScreen(rect.X + 1, rect.Y + 1, 0, 0, rect.Size);
            return memory;
        }

        /// <summary>
        /// 区域截取屏幕
        /// </summary>
        /// <returns></returns>
        public static Image SelectionPrintScreen() 
        {
            Image ret = null;
            ScreenWnd ScrmapWnd = new ScreenWnd(BitMapDeal.GetScreenBitmap());
            try
            {
                if (ScrmapWnd.ShowDialog() == DialogResult.OK)
                {
                    ret = ScrmapWnd.ResaultPicture;
                }
            }
            finally 
            {
                ScrmapWnd.FreeResource();
            }
            return ret;
        }

        /// <summary>
        /// 取得圖象指定區域的信息，並創建為一個新的圖象返回。
        /// </summary>
        /// <param name="img">圖象。</param>
        /// <param name="x">圖象內起始的 X 坐標處。</param>
        /// <param name="y">圖象內起始的 Y 坐標處</param>
        /// <param name="width">需要獲得的區域寬度。</param>
        /// <param name="height">需要獲得的區域高度。</param>
        /// <returns></returns>
        public static Bitmap BitmapCut(Image img, int x, int y, int width, int height)
        {
            Bitmap bmp = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(bmp);
            g.DrawImage(img, new Rectangle(0, 0, width, height), new Rectangle(x, y, width, height), GraphicsUnit.Pixel);
            return bmp;
        }

        /// <summary>
        /// 截取屏幕图像。
        /// </summary>
        /// <returns></returns>
        public static Bitmap GetScreenBitmap()
        {
            Bitmap scrM = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            Graphics g = Graphics.FromImage(scrM);
            g.CopyFromScreen(0, 0, 0, 0, Screen.PrimaryScreen.Bounds.Size);
            return scrM;
        }
    }

    
}
