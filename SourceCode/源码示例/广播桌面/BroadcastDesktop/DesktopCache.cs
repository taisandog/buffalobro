using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;
using Buffalo.Win32Kernel.Win32;
using Buffalo.Kernel;
using System.Threading;
using Buffalo.Kernel.Win32;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;

namespace BroadcastDesktop
{
    /// <summary>
    /// 桌面缓存
    /// </summary>
    public class DesktopCache
    {
        private byte[] _msCache = null;
        private Thread _updateThread;
        private bool _isRunning=false;
        private int _sleeptime;
        private bool _isDrawMouse;
        private long _qty=100L;
        ImageCodecInfo _codecInfo = null;
        /// <summary>
        /// 质量
        /// </summary>
        public long Qty
        {
            get { return _qty; }
            set { _qty = value; }
        }
        /// <summary>
        /// 屏幕桌面缓存
        /// </summary>
        /// <param name="sleeptime">截取时间间隔(毫秒)</param>
        /// <param name="isDrawMouse">是否绘制鼠标</param>
        public DesktopCache(int sleeptime,bool isDrawMouse) 
        {
            _sleeptime = sleeptime;
            _isDrawMouse = isDrawMouse;
            _codecInfo = Picture.GetEncoder(ImageFormat.Png);
        }

        

        /// <summary>
        /// 当前桌面
        /// </summary>
        public byte[] CurrentDesktop 
        {
            get 
            {
                return _msCache;
            }
        }

        /// <summary>
        /// 开始更新
        /// </summary>
        public void StarUpdate() 
        {
            _isRunning = true;
            _updateThread = new Thread(new ThreadStart(UpdatePrintScreen));
            _updateThread.Start();
        }

        /// <summary>
        /// 刷新屏幕数据
        /// </summary>
        private void UpdatePrintScreen()
        {
            while (_isRunning)
            {
                Image img = WindowsApplication.PrintScreen();
                if (_isDrawMouse) 
                {
                    DrawMousePoint(img);
                }
                if (_qty > 0)
                {
                    _msCache = Picture.PictureToBytes(img, _codecInfo, _qty);
                }
                else 
                {
                    _msCache = Picture.PictureToBytes(img,ImageFormat.Png);
                }
                Thread.Sleep(_sleeptime);
            }
        }

        /// <summary>
        /// 开始更新
        /// </summary>
        public void StopUpdate()
        {
            _isRunning = false;
            if (_updateThread != null)
            {
                _updateThread.Abort();
                _updateThread = null;
                Thread.Sleep(100);
            }
        }

        private void DrawMousePoint(Image img) 
        {
            CURSORINFO pci;
            pci.cbSize = Marshal.SizeOf(typeof(CURSORINFO));
            WindowsAPI.GetCursorInfo(out pci);
            using (Graphics grp = Graphics.FromImage(img)) 
            {
                
                pci.DrawMouseIcon(grp);
            }
        }

    }
}
