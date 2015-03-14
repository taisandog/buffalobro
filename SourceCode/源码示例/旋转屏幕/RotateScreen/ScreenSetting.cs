using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace CheckSQLiteDB
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct DEVMODE
    {
        //public const int DM_DISPLAYFREQUENCY = 0x400000;
        //public const int DM_PELSWIDTH = 0x80000;
        //public const int DM_PELSHEIGHT = 0x100000;
        //public const int DM_BITSPERPEL = 262144;
        //private const int CCHDEVICENAME = 32;
        //private const int CCHFORMNAME = 32;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string dmDeviceName;

        public short dmSpecVersion;
        public short dmDriverVersion;
        public short dmSize;
        public short dmDriverExtra;
        public int dmFields;
        public int dmPositionX;
        public int dmPositionY;
        public int dmDisplayOrientation;
        public int dmDisplayFixedOutput;
        public short dmColor;
        public short dmDuplex;
        public short dmYResolution;
        public short dmTTOption;
        public short dmCollate;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string dmFormName;

        public short dmLogPixels;
        public short dmBitsPerPel;
        public int dmPelsWidth;
        public int dmPelsHeight;
        public int dmDisplayFlags;
        public int dmDisplayFrequency;
        public int dmICMMethod;
        public int dmICMIntent;
        public int dmMediaType;
        public int dmDitherType;
        public int dmReserved1;
        public int dmReserved2;
        public int dmPanningWidth;
        public int dmPanningHeight;
    };

    /// <summary>
    /// 屏幕方向
    /// </summary>
    public enum DisplayOrientation 
    {
        DmdoDEFAULT = 0,
        Dmdo90 = 1,
        Dmdo180 = 2,
        Dmdo270 = 3,
    }
    public enum DeviceFlags
    {
        CDS_FULLSCREEN = 0x4,
        CDS_GLOBAL = 0x8,
        CDS_NORESET = 0x10000000,
        CDS_RESET = 0x40000000,
        CDS_SET_PRIMARY = 0x10,
        CDS_TEST = 0x2,
        CDS_UPDATEREGISTRY = 0x1,
        CDS_VIDEOPARAMETERS = 0x20,
    }
    
    /// <summary>
    /// 屏幕设置
    /// </summary>
    public class ScreenSetting
    {
        [DllImport("user32.dll", CharSet = CharSet.Ansi)]
        public static extern int EnumDisplaySettings(string lpszDeviceName, int iModeNum, ref DEVMODE lpDevMode);

        [DllImport("user32.dll")]
        public static extern int ChangeDisplaySettingsEx(string lpszDeviceName, ref DEVMODE lpDevMode, IntPtr hwnd, int dwflags, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Ansi)]
        public static extern int ChangeDisplaySettings(ref DEVMODE lpDevMode, int dwFlags);
        public const int ENUM_CURRENT_SETTINGS = -1;


        public static DEVMODE CreateDevmode()
        {
            DEVMODE dm = new DEVMODE();
            dm.dmDeviceName = new String(new char[32]);
            dm.dmFormName = new String(new char[32]);
            dm.dmSize = (short)Marshal.SizeOf(dm);
            return dm;
        }

        /// <summary>
        /// 修改屏幕方向
        /// </summary>
        /// <param name="i"></param>
        public static Boolean Orientation(DisplayOrientation ori)
        {
            DEVMODE dm = new DEVMODE();
            dm.dmDeviceName = new string(new char[32]);
            dm.dmFormName = new string(new char[32]);
            dm.dmSize = (short)Marshal.SizeOf(dm);
            if (0 != EnumDisplaySettings(null, ENUM_CURRENT_SETTINGS, ref dm))
            {
                //if (dm.dmDisplayOrientation == (int)ori)
                //{
                //    return false;
                //}
                if (dm.dmDisplayOrientation % 2 != (int)ori % 2)
                {
                    int temp = dm.dmPelsHeight;
                    dm.dmPelsHeight = dm.dmPelsWidth;
                    dm.dmPelsWidth = temp;
                }
                Screen scr = Screen.PrimaryScreen;
                //dm.dmFields = DEVMODE.DM_PELSWIDTH | DEVMODE.DM_PELSHEIGHT | DEVMODE.DM_DISPLAYFREQUENCY | DEVMODE.DM_BITSPERPEL;
                dm.dmDisplayOrientation = (int)ori;
                //dm.dmFields = 0x00800000;
                //int iRet = ChangeDisplaySettings(ref dm, 0);
                ChangeDisplaySettingsEx(scr.DeviceName, ref dm, IntPtr.Zero, (int)(DeviceFlags.CDS_UPDATEREGISTRY), IntPtr.Zero);
                //CommitChange(scr);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 获取当前屏幕的旋转度
        /// </summary>
        /// <returns></returns>
        public static DisplayOrientation GetScreenOrientation() 
        {
            DEVMODE dm = new DEVMODE();
            dm.dmDeviceName = new string(new char[32]);
            dm.dmFormName = new string(new char[32]);
            dm.dmSize = (short)Marshal.SizeOf(dm);
            if ( EnumDisplaySettings(null, ENUM_CURRENT_SETTINGS, ref dm)!=0)
            {
                return (DisplayOrientation)dm.dmDisplayOrientation;
            }
            return DisplayOrientation.DmdoDEFAULT;
        }

        /// <summary>
        /// 修改屏幕分辨
        /// </summary>
        /// <param name="i"></param>
        public static Boolean ChangeScreen(int width, int height)
        {
            DEVMODE dm = new DEVMODE();
            dm.dmDeviceName = new string(new char[32]);
            dm.dmFormName = new string(new char[32]);
            dm.dmSize = (short)Marshal.SizeOf(dm);
            if (0 != EnumDisplaySettings(null, ENUM_CURRENT_SETTINGS, ref dm))
            {
                if (dm.dmPelsHeight == height && dm.dmPelsWidth == width)
                {
                    return false;
                }
                dm.dmPelsHeight = height;
                dm.dmPelsWidth = width;
                
                int iRet = ChangeDisplaySettings(ref dm, 0);
                return true;
            }
            return false;
        }
        private static void CommitChange(Screen screen)
        {
            DEVMODE ndm5 = CreateDevmode();
            ChangeDisplaySettingsEx(screen.DeviceName, ref ndm5, (IntPtr)null, (int)(DeviceFlags.CDS_UPDATEREGISTRY), (IntPtr)null);

        }
        
    }
}
