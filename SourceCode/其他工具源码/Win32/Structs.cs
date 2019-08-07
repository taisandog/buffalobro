using System;
using System.Drawing;
using System.Runtime.InteropServices;


namespace Buffalo.Kernel.Win32
{

    /// <summary>
    /// Structures to interoperate with the Windows 32 API  
    /// </summary>

    /*
     * HANDLE - IntPtr
     * BYTE - Byte
     * SHORT - Int16
     * WORD - UInt16
     * INT - Int32
     * UINT - UInt32
     * LONG - INt32
     * BOOL - Int32
     * int - UInt32
     * ULONG - UInt32
     * CHAR - Char
     * LPSTR - String
     * FLOAT - Single
     * DOUBLE - Double
    */

    #region CREATESTRUCT
    [StructLayout(LayoutKind.Sequential)]
    public struct CREATESTRUCT
    {
        IntPtr lpCreateParams;
        IntPtr hInstance;
        IntPtr hMenu;
        IntPtr hwndParent;
        int cy;
        int cx;
        int y;
        int x;
        Int32 style;
        string lpszName;
        string lpszClass;
        UInt32 dwExStyle;
    }
    #endregion

    #region SpinControls
    [StructLayout(LayoutKind.Sequential)]
    public struct UDACCEL
    {
        public UInt32 nSec;
        public UInt32 nInc;
    }
    #endregion

    #region SIZE
    [StructLayout(LayoutKind.Sequential)]
    public struct SIZE
    {
        public int cx;
        public int cy;
    }
    #endregion

    #region RECT
    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;

        public static implicit operator Rectangle(RECT rect)
        {
            return Rectangle.FromLTRB(rect.left, rect.top, rect.right, rect.bottom);
        }

        public static implicit operator Size(RECT rect)
        {
            return new Size(rect.right - rect.left, rect.bottom - rect.top);
        }

        public static explicit operator RECT(Rectangle rect)
        {
            RECT rc = new RECT();

            rc.left = rect.Left;
            rc.right = rect.Right;
            rc.top = rect.Top;
            rc.bottom = rect.Bottom;

            return rc;
        }
    }

    #endregion

    #region INITCOMMONCONTROLSEX
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class INITCOMMONCONTROLSEX
    {
        public int dwSize;
        public int dwICC;
    }
    #endregion

    #region TBBUTTON
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TBBUTTON
    {
        public int iBitmap;
        public int idCommand;
        public byte fsState;
        public byte fsStyle;
        public byte bReserved0;
        public byte bReserved1;
        public int dwData;
        public int iString;
    }
    #endregion

    #region POINT
    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int x;
        public int y;


        public POINT(int X, int Y)
        {
            x = X;
            y = Y;
        }
        /// <summary>
        /// Point creation from lParam `s data
        /// </summary>
        /// <param name="lParam"></param>
        public POINT(int lParam)
        {
            x = (lParam & 0xffff);
            y = (lParam >> 16);
        }

        public static implicit operator System.Drawing.Point(POINT p)
        {
            return new System.Drawing.Point(p.x, p.y);
        }

        public static implicit operator POINT(System.Drawing.Point p)
        {
            return new POINT(p.X, p.Y);
        }

    }
    [StructLayout(LayoutKind.Sequential)]
    public struct CURSORINFO
    {
        public Int32 cbSize;        // Specifies the size, in bytes, of the structure. 
        // The caller must set this to Marshal.SizeOf(typeof(CURSORINFO)).
        public Int32 flags;         // Specifies the cursor state. This parameter can be one of the following values:
        //    0             The cursor is hidden.
        //    CURSOR_SHOWING    The cursor is showing.
        public IntPtr hCursor;          // Handle to the cursor. 
        public POINT ptScreenPos;       // A POINT structure that receives the screen coordinates of the cursor. 

        /// <summary>
        /// 绘制鼠标
        /// </summary>
        /// <param name="grp">图像</param>
        /// <param name="x">位置x</param>
        /// <param name="y">位置y</param>
        public void DrawMouseIcon(Graphics grp,int x,int y) 
        {
            IntPtr iCursor = WindowsAPI.CopyIcon(hCursor);
            if (iCursor == IntPtr.Zero) 
            {
                return;
            }
            //Icon objIcon = Icon.FromHandle(iCursor);
            WindowsAPI.DrawIcon(grp.GetHdc(), x, y, iCursor);
            
            //grp.DrawIcon(objIcon, x, y);
            WindowsAPI.DestroyCursor(iCursor);
        }

        /// <summary>
        /// 绘制鼠标
        /// </summary>
        /// <param name="grp">图像</param>
        public void DrawMouseIcon(Graphics grp)
        {
            DrawMouseIcon(grp, ptScreenPos.x, ptScreenPos.y);
        }
    }
    #endregion

    #region NMHDR
    [StructLayout(LayoutKind.Sequential)]
    public struct NMHDR
    {
        public IntPtr hwndFrom;
        public int idFrom;
        public int code;
    }
    #endregion

    #region NMCUSTOMDRAW
    [StructLayout(LayoutKind.Sequential)]
    public struct NMCUSTOMDRAW
    {
        public NMHDR hdr;
        public int dwDrawStage;
        public IntPtr hdc;
        public RECT rc;
        public int dwItemSpec;
        public int uItemState;
        public int lItemlParam;
    }
    #endregion

    #region NMTBCUSTOMDRAW
    [StructLayout(LayoutKind.Sequential)]
    public struct NMTBCUSTOMDRAW
    {
        public NMCUSTOMDRAW nmcd;
        public IntPtr hbrMonoDither;
        public IntPtr hbrLines;
        public IntPtr hpenLines;
        public int clrText;
        public int clrMark;
        public int clrTextHighlight;
        public int clrBtnFace;
        public int clrBtnHighlight;
        public int clrHighlightHotTrack;
        public RECT rcText;
        public int nStringBkMode;
        public int nHLStringBkMode;
    }
    #endregion

    #region NMLVCUSTOMDRAW
    [StructLayout(LayoutKind.Sequential)]
    public struct NMLVCUSTOMDRAW
    {
        public NMCUSTOMDRAW nmcd;
        public uint clrText;
        public uint clrTextBk;
        public int iSubItem;
    }
    #endregion

    #region TOOLTIP CONTROL STRUCTS
    #region TOOLTIPTEXTA || NMTTDISPINFO
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct TOOLTIPTEXTA
    {
        public NMHDR hdr;
        public IntPtr lpszText;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
        public string szText;
        public IntPtr hinst;
        public int uFlags;
    }
    #endregion

    #region TOOLTIPTEXT || NMTTDISPINFO
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct TOOLTIPTEXT
    {
        public NMHDR hdr;
        public IntPtr lpszText;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
        public string szText;
        public IntPtr hinst;
        public int uFlags;
    }
    #endregion

    #region NMTTCUSTOMDRAW
    [StructLayout(LayoutKind.Sequential)]
    public struct NMTTCUSTOMDRAW
    {
        public NMCUSTOMDRAW nmcd;
        public UInt32 uDrawFlags;
    }
    #endregion

    #region NMTTDISPINFO
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct NMTTDISPINFO
    {
        public NMHDR hdr;
        public IntPtr lpszText;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
        public string szText;
        public IntPtr hinst;
        public int uFlags;
    }
    #endregion

    #region TOOLINFO
    [StructLayout(LayoutKind.Sequential)]
    public struct TOOLINFO
    {
        public UInt32 cbSize;
        public UInt32 uFlags;
        public IntPtr hwnd;
        public IntPtr uId;       // UINT_PTR
        public RECT rect;
        public IntPtr hinst;
        [MarshalAs(UnmanagedType.LPTStr)]
        public string lpszText;
        //public IntPtr      lpszText; 
        public IntPtr lParam;
    }
    #endregion

    #region TTHITTESTINFO
    [StructLayout(LayoutKind.Sequential)]
    public struct TTHITTESTINFO
    {
        public IntPtr hwnd;
        public POINT pt;
        public TOOLINFO ti;
    }
    #endregion

    #region TTGETTITLE
    [StructLayout(LayoutKind.Sequential)]
    public struct TTGETTITLE
    {
        public UInt32 dwSize;
        public UInt32 uTitleBitmap;
        public UInt32 cch;
        public IntPtr pszTitle;
    }
    #endregion
    #endregion

    #region TBBUTTONINFO
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct TBBUTTONINFO
    {
        public int cbSize;
        public int dwMask;
        public int idCommand;
        public int iImage;
        public byte fsState;
        public byte fsStyle;
        public short cx;
        public IntPtr lParam;
        public IntPtr pszText;
        public int cchText;
    }
    #endregion

    #region REBARBANDINFO
    [StructLayout(LayoutKind.Sequential)]
    public struct REBARBANDINFO
    {
        public int cbSize;
        public int fMask;
        public int fStyle;
        public int clrFore;
        public int clrBack;
        public IntPtr lpText;
        public int cch;
        public int iImage;
        public IntPtr hwndChild;
        public int cxMinChild;
        public int cyMinChild;
        public int cx;
        public IntPtr hbmBack;
        public int wID;
        public int cyChild;
        public int cyMaxChild;
        public int cyIntegral;
        public int cxIdeal;
        public int lParam;
        public int cxHeader;
    }
    #endregion

    #region MOUSEHOOKSTRUCT
    [StructLayout(LayoutKind.Sequential)]
    public struct MOUSEHOOKSTRUCT
    {
        public POINT pt;
        public IntPtr hwnd;
        public int wHitTestCode;
        public IntPtr dwExtraInfo;
    }
    #endregion

    #region NMTOOLBAR
    [StructLayout(LayoutKind.Sequential)]
    public struct NMTOOLBAR
    {
        public NMHDR hdr;
        public int iItem;
        public TBBUTTON tbButton;
        public int cchText;
        public IntPtr pszText;
        public RECT rcButton;
    }
    #endregion

    #region NMREBARCHEVRON
    [StructLayout(LayoutKind.Sequential)]
    public struct NMREBARCHEVRON
    {
        public NMHDR hdr;
        public int uBand;
        public int wID;
        public int lParam;
        public RECT rc;
        public int lParamNM;
    }
    #endregion

    #region NMREBARCHILDSIZE
    [StructLayout(LayoutKind.Sequential)]
    public struct NMREBARCHILDSIZE
    {
        public NMHDR hdr;
        public UInt32 uBand;
        public UInt32 wID;
        public RECT rcChild;
        public RECT rcBand;
    };
    #endregion

    #region BITMAP
    [StructLayout(LayoutKind.Sequential)]
    public struct BITMAP
    {
        public long bmType;
        public long bmWidth;
        public long bmHeight;
        public long bmWidthBytes;
        public short bmPlanes;
        public short bmBitsPixel;
        public IntPtr bmBits;
    }
    #endregion

    #region BITMAPINFO_FLAT
    [StructLayout(LayoutKind.Sequential)]
    public struct BITMAPINFO_FLAT
    {
        public int bmiHeader_biSize;
        public int bmiHeader_biWidth;
        public int bmiHeader_biHeight;
        public short bmiHeader_biPlanes;
        public short bmiHeader_biBitCount;
        public int bmiHeader_biCompression;
        public int bmiHeader_biSizeImage;
        public int bmiHeader_biXPelsPerMeter;
        public int bmiHeader_biYPelsPerMeter;
        public int bmiHeader_biClrUsed;
        public int bmiHeader_biClrImportant;
        [MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = 1024)]
        public byte[] bmiColors;
    }
    #endregion

    #region RGBQUAD
    public struct RGBQUAD
    {
        public byte rgbBlue;
        public byte rgbGreen;
        public byte rgbRed;
        public byte rgbReserved;
    }
    #endregion

    #region BITMAPINFOHEADER
    [StructLayout(LayoutKind.Sequential)]
    public class BITMAPINFOHEADER
    {
        public int biSize = Marshal.SizeOf(typeof(BITMAPINFOHEADER));
        public int biWidth;
        public int biHeight;
        public short biPlanes;
        public short biBitCount;
        public int biCompression;
        public int biSizeImage;
        public int biXPelsPerMeter;
        public int biYPelsPerMeter;
        public int biClrUsed;
        public int biClrImportant;
    }
    #endregion

    #region BITMAPINFO
    [StructLayout(LayoutKind.Sequential)]
    public class BITMAPINFO
    {
        public BITMAPINFOHEADER bmiHeader = new BITMAPINFOHEADER();
        [MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = 1024)]
        public byte[] bmiColors;
    }
    #endregion

    #region PALETTEENTRY
    [StructLayout(LayoutKind.Sequential)]
    public struct PALETTEENTRY
    {
        public byte peRed;
        public byte peGreen;
        public byte peBlue;
        public byte peFlags;
    }
    #endregion

    #region MSG
    [StructLayout(LayoutKind.Sequential)]
    public struct MSG
    {
        public IntPtr hwnd;
        public int message;
        public IntPtr wParam;
        public IntPtr lParam;
        public int time;
        public int pt_x;
        public int pt_y;
    }
    #endregion

    #region HD_HITTESTINFO
    [StructLayout(LayoutKind.Sequential)]
    public struct HD_HITTESTINFO
    {
        public POINT pt;
        public uint flags;
        public int iItem;
    }
    #endregion

    #region DLLVERSIONINFO
    [StructLayout(LayoutKind.Sequential)]
    public struct DLLVERSIONINFO
    {
        public int cbSize;
        public int dwMajorVersion;
        public int dwMinorVersion;
        public int dwBuildNumber;
        public int dwPlatformID;
    }
    #endregion

    #region PAINTSTRUCT
    [StructLayout(LayoutKind.Sequential)]
    public struct PAINTSTRUCT
    {
        public IntPtr hdc;
        public int fErase;
        public Rectangle rcPaint;
        public int fRestore;
        public int fIncUpdate;
        public int Reserved1;
        public int Reserved2;
        public int Reserved3;
        public int Reserved4;
        public int Reserved5;
        public int Reserved6;
        public int Reserved7;
        public int Reserved8;
    }
    #endregion

    #region BLENDFUNCTION
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct BLENDFUNCTION
    {
        public byte BlendOp;
        public byte BlendFlags;
        public byte SourceConstantAlpha;
        public byte AlphaFormat;
    }

    #endregion

    #region TRACKMOUSEEVENTS
    [StructLayout(LayoutKind.Sequential)]
    public struct TRACKMOUSEEVENTS
    {
        public uint cbSize;
        public uint dwFlags;
        public IntPtr hWnd;
        public uint dwHoverTime;
    }
    #endregion

    #region STRINGBUFFER
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct STRINGBUFFER
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
        public string szText;
    }
    #endregion

    #region NMTVCUSTOMDRAW
    [StructLayout(LayoutKind.Sequential)]
    public struct NMTVCUSTOMDRAW
    {
        public NMCUSTOMDRAW nmcd;
        public uint clrText;
        public uint clrTextBk;
        public int iLevel;
    }
    #endregion

    #region NMTREEVIEW
    [StructLayout(LayoutKind.Sequential)]
    public struct NMTREEVIEW
    {
        public NMHDR hdr;
        public uint action;
        public TVITEM itemOld;
        public TVITEM itemNew;
        public POINT ptDrag;
    };

    #endregion

    #region TVITEM
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct TVITEM
    {
        public uint mask;
        public IntPtr hItem;
        public uint state;
        public uint stateMask;
        public IntPtr pszText;
        public int cchTextMax;
        public int iImage;
        public int iSelectedImage;
        public int cChildren;
        public int lParam;
    }
    #endregion

    #region TVHITTESTINFO
    [StructLayout(LayoutKind.Sequential)]
    public struct TVHITTESTINFO
    {
        public POINT pt;
        public UInt32 flags;
        public IntPtr hItem;

        public TVHitTestFlags Flags
        {
            get
            {
                return (TVHitTestFlags)flags;
            }
            set
            {
                flags = (UInt32)value;
            }
        }

    };

    #endregion

    #region TVINSERTSTRUCT
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct TVINSERTSTRUCT
    {
        int hParent;
        int hInsertAfter;
        TVITEM item;
    }
    #endregion

    #region LVITEM
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct LVITEM
    {
        public uint mask;
        public int iItem;
        public int iSubItem;
        public uint state;
        public uint stateMask;
        public IntPtr pszText;
        public int cchTextMax;
        public int iImage;
        public int lParam;
        public int iIndent;
    }
    #endregion

    #region HDITEM
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct HDITEM
    {
        public uint mask;
        public int cxy;
        public IntPtr pszText;
        public IntPtr hbm;
        public int cchTextMax;
        public int fmt;
        public int lParam;
        public int iImage;
        public int iOrder;
    }
    #endregion

    #region WINDOWPLACEMENT
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct WINDOWPLACEMENT
    {
        public uint length;
        public uint flags;
        public uint showCmd;
        public POINT ptMinPosition;
        public POINT ptMaxPosition;
        public RECT rcNormalPosition;
    }
    #endregion

    #region SCROLLINFO
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct SCROLLINFO
    {
        public uint cbSize;
        public uint fMask;
        public int nMin;
        public int nMax;
        public uint nPage;
        public int nPos;
        public int nTrackPos;
    }
    #endregion

    #region NOTIFYICONDATA
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct NOTIFYICONDATA
    {
        public UInt32 cbSize;                       // int
        public IntPtr hWnd;                         // HWND
        public UInt32 uID;                          // UINT
        public NotifyFlags uFlags;                       // UINT
        public UInt32 uCallbackMessage;             // UINT
        public IntPtr hIcon;                        // HICON
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string szTip;                        // char[128]
        public NotifyState dwState;                      // int   
        public NotifyState dwStateMask;                  // int
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string szInfo;                       // char[256]
        public UInt32 uTimeoutOrVersion;            // UINT
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string szInfoTitle;                  // char[64]
        public NotifyInfoFlags dwInfoFlags;                  // int
    }
    #endregion

    #region PARAFORMAT2
    /*
  typedef struct _paraformat { 
    UINT cbSize; 
    int dwMask; 
    WORD  wNumbering; 
    WORD  wEffects; 
    LONG  dxStartIndent; 
    LONG  dxRightIndent; 
    LONG  dxOffset; 
    WORD  wAlignment; 
    SHORT cTabCount; 
    LONG  rgxTabs[MAX_TAB_STOPS]; 
    LONG  dySpaceBefore; 
    LONG  dySpaceAfter; 
    LONG  dyLineSpacing; 
    SHORT sStyle; 
    BYTE  bLineSpacingRule; 
    BYTE  bOutlineLevel; 
    WORD  wShadingWeight; 
    WORD  wShadingStyle;
    WORD  wNumberingStart; 
    WORD  wNumberingStyle; 
    WORD  wNumberingTab; 
    WORD  wBorderSpace; 
    WORD  wBorderWidth; 
    WORD  wBorders; 
  } PARAFORMAT2;
 */


    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct PARAFORMAT2
    {
        public UInt32 cbSize;
        public UInt32 dwMask;
        public UInt16 wNumbering;
        public UInt16 wEffects;
        public Int32 dxStartIndent;
        public Int32 dxRightIndent;
        public Int32 dxOffset;
        public UInt16 wAlignment;
        public Int16 cTabCount;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public Int32[] rgxTabs;
        public Int32 dySpaceBefore;
        public Int32 dySpaceAfter;
        public Int32 dyLineSpacing;
        public Int16 sStyle;
        public Byte bLineSpacingRule;
        public Byte bOutlineLevel;
        public UInt16 wShadingWeight;
        public UInt16 wShadingStyle;
        public UInt16 wNumberingStart;
        public UInt16 wNumberingStyle;
        public UInt16 wNumberingTab;
        public UInt16 wBorderSpace;
        public UInt16 wBorderWidth;
        public UInt16 wBorders;
    }
    #endregion
    public struct COPYDATASTRUCT
    {
        public IntPtr dwData;
        public int cbData;
        [MarshalAs(UnmanagedType.LPStr)]
        public string lpData;

    }
    
    public struct SHFILEOPSTRUCT
    {
        public IntPtr hwnd;
        public WFunc wFunc;
        public string pFrom;
        public string pTo;
        public FILEOP_FLAGS fFlags;
        public bool fAnyOperationsAborted;
        public IntPtr hNameMappings;
        public string lpszProgressTitle;
    }
    /// <summary>
    /// 键盘钩子信息结构体
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct KBDLLHOOKSTRUCT
    {
        ///<summary>Especifica el codigo de tecla virtual, el codigo esta entre 1 y 254.</summary>
        public int vkCode;
        ///<summary>Especifica el escaneo de hardware del codigo de la tecla</summary>
        public int scanCode;
        ///<summary>especifica flags extendidos, mirar la estrucutra 'KBDLLHOOKSTRUCT' para mas info</summary>
        public int flags;
        ///<summary>especifica el time stamp para este mensaje.</summary>
        public int time;
        ///<summary>Especifica informacion extra para el mensaje</summary>
        public int dwExtraInfo;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct CHARRANGE
    {
        public int cpMin;
        public int cpMax;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct FORMATRANGE
    {
        public IntPtr hdc;
        public IntPtr hdcTarget;
        public RECT rc;
        public RECT rcPage;
        public CHARRANGE chrg;
    }
    
    /// <summary>
    /// 进程令牌
    /// </summary>
    unsafe public struct TOKEN_PRIVILEGES
    {
        public UInt32 PrivilegeCount;

        //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public LUID_AND_ATTRIBUTES Privileges;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct SECURITY_ATTRIBUTES
    {
        public int nLength;
        public IntPtr lpSecurityDescriptor;
        public int bInheritHandle;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct LUID_AND_ATTRIBUTES
    {
        public LUID Luid;
        public UInt32 Attributes;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct LUID
    {
        public uint LowPart;
        public int HighPart;
    }

    /// <summary>
    /// 系统名名
    /// </summary>
    public class SystemName
    {
        public const string SE_CREATE_TOKEN_NAME = "SeCreateTokenPrivilege";
        public const string SE_ASSIGNPRIMARYTOKEN_NAME = "SeAssignPrimaryTokenPrivilege";
        public const string SE_LOCK_MEMORY_NAME = "SeLockMemoryPrivilege";
        public const string SE_INCREASE_QUOTA_NAME = "SeIncreaseQuotaPrivilege";
        public const string SE_UNSOLICITED_INPUT_NAME = "SeUnsolicitedInputPrivilege";
        public const string SE_MACHINE_ACCOUNT_NAME = "SeMachineAccountPrivilege";
        public const string SE_TCB_NAME = "SeTcbPrivilege";
        public const string SE_SECURITY_NAME = "SeSecurityPrivilege";
        public const string SE_TAKE_OWNERSHIP_NAME = "SeTakeOwnershipPrivilege";
        public const string SE_LOAD_DRIVER_NAME = "SeLoadDriverPrivilege";
        public const string SE_SYSTEM_PROFILE_NAME = "SeSystemProfilePrivilege";
        public const string SE_SYSTEMTIME_NAME = "SeSystemtimePrivilege";
        public const string SE_PROF_SINGLE_PROCESS_NAME = "SeProfileSingleProcessPrivilege";
        public const string SE_INC_BASE_PRIORITY_NAME = "SeIncreaseBasePriorityPrivilege";
        public const string SE_CREATE_PAGEFILE_NAME = "SeCreatePagefilePrivilege";
        public const string SE_CREATE_PERMANENT_NAME = "SeCreatePermanentPrivilege";
        public const string SE_BACKUP_NAME = "SeBackupPrivilege";
        public const string SE_RESTORE_NAME = "SeRestorePrivilege";
        public const string SE_SHUTDOWN_NAME = "SeShutdownPrivilege";
        public const string SE_DEBUG_NAME = "SeDebugPrivilege";
        public const string SE_AUDIT_NAME = "SeAuditPrivilege";
        public const string SE_SYSTEM_ENVIRONMENT_NAME = "SeSystemEnvironmentPrivilege";
        public const string SE_CHANGE_NOTIFY_NAME = "SeChangeNotifyPrivilege";
        public const string SE_REMOTE_SHUTDOWN_NAME = "SeRemoteShutdownPrivilege";
        public const string SE_UNDOCK_NAME = "SeUndockPrivilege";
        public const string SE_SYNC_AGENT_NAME = "SeSyncAgentPrivilege";
        public const string SE_ENABLE_DELEGATION_NAME = "SeEnableDelegationPrivilege";
        public const string SE_MANAGE_VOLUME_NAME = "SeManageVolumePrivilege";
        public const string SE_IMPERSONATE_NAME = "SeImpersonatePrivilege";
        public const string SE_CREATE_GLOBAL_NAME = "SeCreateGlobalPrivilege";
    }

    #region Shell
    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("00000002-0000-0000-C000-000000000046")]
    public interface IMalloc
    {
        // Allocates a block of memory.
        // Return value: a pointer to the allocated memory block.
        [PreserveSig]
        IntPtr Alloc(
          UInt32 cb);        // Size, in bytes, of the memory block to be allocated. 

        // Changes the size of a previously allocated memory block.
        // Return value:  Reallocated memory block 
        [PreserveSig]
        IntPtr Realloc(
          IntPtr pv,         // Pointer to the memory block to be reallocated.
          UInt32 cb);        // Size of the memory block (in bytes) to be reallocated.

        // Frees a previously allocated block of memory.
        [PreserveSig]
        void Free(
          IntPtr pv);        // Pointer to the memory block to be freed.

        // This method returns the size (in bytes) of a memory block previously
        // allocated with IMalloc::Alloc or IMalloc::Realloc.
        // Return value: The size of the allocated memory block in bytes 
        [PreserveSig]
        UInt32 GetSize(
          IntPtr pv);        // Pointer to the memory block for which the size
        // is requested.

        // This method determines whether this allocator was used to allocate
        // the specified block of memory.
        // Return value: 1 - allocated 0 - not allocated by this IMalloc instance. 
        [PreserveSig]
        Int16 DidAlloc(
          IntPtr pv);        // Pointer to the memory block

        // This method minimizes the heap as much as possible by releasing unused
        // memory to the operating system, 
        // coalescing adjacent free blocks and committing free pages.
        [PreserveSig]
        void HeapMinimize();
    }


    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("000214E6-0000-0000-C000-000000000046")]
    public interface IShellFolder
    {
        // Translates a file object's or folder's display name into an item identifier list.
        // Return value: error code, if any
        [PreserveSig]
        Int32 ParseDisplayName(
          IntPtr hwnd,            // Optional window handle
          IntPtr pbc,                 // Optional bind context that controls the
            // parsing operation. This parameter is 
            // normally set to NULL. 
          [MarshalAs(UnmanagedType.LPWStr)] 
      String pszDisplayName,    // Null-terminated UNICODE string with the
            // display name.
          ref UInt32 pchEaten,    // Pointer to a ULONG value that receives the
            // number of characters of the 
            // display name that was parsed.
          out IntPtr ppidl,           // Pointer to an ITEMIDLIST pointer that receives
            // the item identifier list for 
            // the object.
          ref UInt32 pdwAttributes); // Optional parameter that can be used to
        // query for file attributes.
        // this can be values from the SFGAO enum

        // Allows a client to determine the contents of a folder by creating an item
        // identifier enumeration object and returning its IEnumIDList interface.
        // Return value: error code, if any
        [PreserveSig]
        Int32 EnumObjects(
          IntPtr hwnd,            // If user input is required to perform the
            // enumeration, this window handle 
            // should be used by the enumeration object as
            // the parent window to take 
            // user input.
          Int32 grfFlags,             // Flags indicating which items to include in the
            // enumeration. For a list 
            // of possible values, see the SHCONTF enum. 
          out IntPtr ppenumIDList); // Address that receives a pointer to the
        // IEnumIDList interface of the 
        // enumeration object created by this method. 

        // Retrieves an IShellFolder object for a subfolder.
        // Return value: error code, if any
        [PreserveSig]
        Int32 BindToObject(
          IntPtr pidl,            // Address of an ITEMIDLIST structure (PIDL)
            // that identifies the subfolder.
          IntPtr pbc,                // Optional address of an IBindCtx interface on
            // a bind context object to be 
            // used during this operation.
          Guid riid,                  // Identifier of the interface to return. 
          out IntPtr ppv);        // Address that receives the interface pointer.

        // Requests a pointer to an object's storage interface. 
        // Return value: error code, if any
        [PreserveSig]
        Int32 BindToStorage(
          IntPtr pidl,            // Address of an ITEMIDLIST structure that
            // identifies the subfolder relative 
            // to its parent folder. 
          IntPtr pbc,                // Optional address of an IBindCtx interface on a
            // bind context object to be 
            // used during this operation.
          Guid riid,                  // Interface identifier (IID) of the requested
            // storage interface.
          out IntPtr ppv);        // Address that receives the interface pointer specified by riid.

        // Determines the relative order of two file objects or folders, given their
        // item identifier lists. Return value: If this method is successful, the
        // CODE field of the HRESULT contains one of the following values (the code
        // can be retrived using the helper function GetHResultCode): Negative A
        // negative return value indicates that the first item should precede
        // the second (pidl1 < pidl2). 

        // Positive A positive return value indicates that the first item should
        // follow the second (pidl1 > pidl2).  Zero A return value of zero
        // indicates that the two items are the same (pidl1 = pidl2). 
        [PreserveSig]
        Int32 CompareIDs(
          Int32 lParam,               // Value that specifies how the comparison
            // should be performed. The lower 
            // Sixteen bits of lParam define the sorting rule.
            // The upper sixteen bits of 
            // lParam are used for flags that modify the
            // sorting rule. values can be from 
            // the SHCIDS enum
          IntPtr pidl1,               // Pointer to the first item's ITEMIDLIST structure.
          IntPtr pidl2);              // Pointer to the second item's ITEMIDLIST structure.

        // Requests an object that can be used to obtain information from or interact
        // with a folder object.
        // Return value: error code, if any
        [PreserveSig]
        Int32 CreateViewObject(
          IntPtr hwndOwner,           // Handle to the owner window.
          Guid riid,                  // Identifier of the requested interface. 
          out IntPtr ppv);        // Address of a pointer to the requested interface. 

        // Retrieves the attributes of one or more file objects or subfolders. 
        // Return value: error code, if any
        [PreserveSig]
        Int32 GetAttributesOf(
          UInt32 cidl,            // Number of file objects from which to retrieve
            // attributes. 
          [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)]
      IntPtr[] apidl,            // Address of an array of pointers to ITEMIDLIST
            // structures, each of which 
            // uniquely identifies a file object relative to
            // the parent folder.
          ref UInt32 rgfInOut);    // Address of a single ULONG value that, on entry,
        // contains the attributes that 
        // the caller is requesting. On exit, this value
        // contains the requested 
        // attributes that are common to all of the
        // specified objects. this value can
        // be from the SFGAO enum

        // Retrieves an OLE interface that can be used to carry out actions on the
        // specified file objects or folders.
        // Return value: error code, if any
        [PreserveSig]
        Int32 GetUIObjectOf(
          IntPtr hwndOwner,        // Handle to the owner window that the client
            // should specify if it displays 
            // a dialog box or message box.
          UInt32 cidl,            // Number of file objects or subfolders specified
            // in the apidl parameter. 
          IntPtr[] apidl,            // Address of an array of pointers to ITEMIDLIST
            // structures, each of which 
            // uniquely identifies a file object or subfolder
            // relative to the parent folder.
          Guid riid,                // Identifier of the COM interface object to return.
          ref UInt32 rgfReserved,    // Reserved. 
          out IntPtr ppv);        // Pointer to the requested interface.

        // Retrieves the display name for the specified file object or subfolder. 
        // Return value: error code, if any
        [PreserveSig]
        Int32 GetDisplayNameOf(
          IntPtr pidl,            // Address of an ITEMIDLIST structure (PIDL)
            // that uniquely identifies the file 
            // object or subfolder relative to the parent folder. 
          UInt32 uFlags,              // Flags used to request the type of display name
            // to return. For a list of 
            // possible values, see the SHGNO enum. 
          out STRRET pName); // Address of a STRRET structure in which to
        // return the display name.

        // Sets the display name of a file object or subfolder, changing the item
        // identifier in the process.
        // Return value: error code, if any
        [PreserveSig]
        Int32 SetNameOf(
          IntPtr hwnd,            // Handle to the owner window of any dialog or
            // message boxes that the client 
            // displays.
          IntPtr pidl,            // Pointer to an ITEMIDLIST structure that uniquely
            // identifies the file object
            // or subfolder relative to the parent folder. 
          [MarshalAs(UnmanagedType.LPWStr)] 
      String pszName,            // Pointer to a null-terminated string that
            // specifies the new display name. 
          UInt32 uFlags,            // Flags indicating the type of name specified by
            // the lpszName parameter. For a list of possible
            // values, see the description of the SHGNO enum. 
          out IntPtr ppidlOut);   // Address of a pointer to an ITEMIDLIST structure
        // which receives the new ITEMIDLIST. 
    }


    /// <summary>
    /// Contains parameters for the SHBrowseForFolder function and
    /// receives information about the folder selected 
    /// by the user.
    /// </summary>

    //[StructLayout(LayoutKind.Sequential)]
    public struct BROWSEINFO
    {
        public IntPtr hwndOwner;                // Handle to the owner window for the
        // dialog box.
        public IntPtr pidlRoot;                    // Pointer to an item identifier list
        // (PIDL) specifying the location of
        // the root folder from which to start
        // browsing.
        [MarshalAs(UnmanagedType.LPStr)]        // Address of a buffer to receive the
        // display name of the 
        public String pszDisplayName;            // folder selected by the user.
        [MarshalAs(UnmanagedType.LPStr)]        // Address of a null-terminated string
        // that is displayed 
        public String lpszTitle;                // above the tree view control in the
        // dialog box.
        public UInt32 ulFlags;                    // Flags specifying the options for the
        // dialog box.
        [MarshalAs(UnmanagedType.FunctionPtr)]    // Address of an application-defined
        // function that the 
        public WindowsAPI.BrowseCallbackProc lpfn;               // dialog box calls when an event occurs.
        public Int32 lParam;                    // Application-defined value that the
        // dialog box passes to 
        // the callback function
        public Int32 iImage;                    // Variable to receive the image
        // associated with the selected folder.
    }


    [StructLayout(LayoutKind.Explicit)]
    public struct STRRET
    {
        [FieldOffset(0)]
        public UInt32 uType;                                            // One of the STRRET_* values

        [FieldOffset(4)]
        public IntPtr pOleStr;                                          // must be freed by caller of GetDisplayNameOf

        [FieldOffset(4)]
        public IntPtr pStr;                                                     // NOT USED

        [FieldOffset(4)]
        public UInt32 uOffset;                                          // Offset into SHITEMID

        [FieldOffset(4)]
        public IntPtr cStr;                                                     // Buffer to fill in (ANSI)
    }



    #endregion
    #region Equipment
    public enum NCBCONST
    {
        NCBNAMSZ = 16,      /* absolute length of a net name         */
        MAX_LANA = 254,      /* lana's in range 0 to MAX_LANA inclusive   */
        NCBENUM = 0x37,      /* NCB ENUMERATE LANA NUMBERS            */
        NRC_GOODRET = 0x00,      /* good return                              */
        NCBRESET = 0x32,      /* NCB RESET                        */
        NCBASTAT = 0x33,      /* NCB ADAPTER STATUS                  */
        NUM_NAMEBUF = 30,      /* Number of NAME's BUFFER               */
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ADAPTER_STATUS
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public byte[] adapter_address;
        public byte rev_major;
        public byte reserved0;
        public byte adapter_type;
        public byte rev_minor;
        public ushort duration;
        public ushort frmr_recv;
        public ushort frmr_xmit;
        public ushort iframe_recv_err;
        public ushort xmit_aborts;
        public uint xmit_success;
        public uint recv_success;
        public ushort iframe_xmit_err;
        public ushort recv_buff_unavail;
        public ushort t1_timeouts;
        public ushort ti_timeouts;
        public uint reserved1;
        public ushort free_ncbs;
        public ushort max_cfg_ncbs;
        public ushort max_ncbs;
        public ushort xmit_buf_unavail;
        public ushort max_dgram_size;
        public ushort pending_sess;
        public ushort max_cfg_sess;
        public ushort max_sess;
        public ushort max_sess_pkt_size;
        public ushort name_count;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct NAME_BUFFER
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)NCBCONST.NCBNAMSZ)]
        public byte[] name;
        public byte name_num;
        public byte name_flags;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct NCB
    {
        public byte ncb_command;
        public byte ncb_retcode;
        public byte ncb_lsn;
        public byte ncb_num;
        public IntPtr ncb_buffer;
        public ushort ncb_length;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)NCBCONST.NCBNAMSZ)]
        public byte[] ncb_callname;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)NCBCONST.NCBNAMSZ)]
        public byte[] ncb_name;
        public byte ncb_rto;
        public byte ncb_sto;
        public IntPtr ncb_post;
        public byte ncb_lana_num;
        public byte ncb_cmd_cplt;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public byte[] ncb_reserve;
        public IntPtr ncb_event;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct LANA_ENUM
    {
        public byte length;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)NCBCONST.MAX_LANA)]
        public byte[] lana;
    }

    [StructLayout(LayoutKind.Auto)]
    public struct ASTAT
    {
        public ADAPTER_STATUS adapt;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = (int)NCBCONST.NUM_NAMEBUF)]
        public NAME_BUFFER[] NameBuff;
    }
    #endregion

    #region DebugAPI
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct CONTEXT
    {
        public ContextFlags ContextFlags;
        //;----------------------------------------------------------------------------------------------------------
        //;当ContextFlags包含CONTEXT_DEBUG_REGISTERS，返回本部分 
        //;----------------------------------------------------------------------------------------------------------
        public UInt32 Dr0;
        public UInt32 Dr1;
        public UInt32 Dr2;
        public UInt32 Dr3;
        public UInt32 Dr6;
        public UInt32 Dr7;
        //;----------------------------------------------------------------------------------------------------------
        //;当ContextFlags包含CONTEXT_FLOATING_POINT，返回本部分 
        //;-----------------------------------------------------------------------------------------------------------
        [MarshalAs(UnmanagedType.Struct)]
        public FLOATING_SAVE_AREA FloatSave;
        //;----------------------------------------------------------------------------------------------------------
        //;当ContextFlags包含CONTEXT_SEGMENTS，返回本部分 
        //;-----------------------------------------------------------------------------------------------------------
        public UInt32 SegGs;
        public UInt32 SegFs;
        public UInt32 SegEs;
        public UInt32 SegDs;
        //;----------------------------------------------------------------------------------------------------------
        //;当ContextFlags包含CONTEXT_INTEGER，返回本部分 
        //;-----------------------------------------------------------------------------------------------------------
        public UInt32 Edi;
        public UInt32 Esi;
        public UInt32 Ebx;
        public UInt32 Edx;
        public UInt32 Ecx;
        public UInt32 Eax;
        //;----------------------------------------------------------------------------------------------------------
        //;当ContextFlags包含CONTEXT_CONTROL，返回本部分 
        //;-----------------------------------------------------------------------------------------------------------
        public UInt32 Ebp;
        public UInt32 Eip;
        public UInt32 SegCs;
        public UInt32 EFlags;
        public UInt32 Esp;
        public UInt32 SegSs;
        //;----------------------------------------------------------------------------------------------------------
        //;当ContextFlags包含CONTEXT_EXTENDED_REGISTERS，返回本部分 
        //;-----------------------------------------------------------------------------------------------------------
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 512)]//MAXIMUM_SUPPORTED_EXTENSION 512
        public byte[] ExtendedRegisters;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct FLOATING_SAVE_AREA
    {

        public UInt32 ControlWord;
        public UInt32 StatusWord;
        public UInt32 TagWord;
        public UInt32 ErrorOffset;
        public UInt32 ErrorSelector;
        public UInt32 DataOffset;
        public UInt32 DataSelector;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 80)]//const int SIZE_OF_80387_REGISTERS = 80;
        public byte[] RegisterArea;
        public UInt32 Cr0NpxState;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct DEBUG_EVENT
    {
        public uint dwDebugEventCode;
        public uint dwProcessId;
        public uint dwThreadId;
        public Union u;
    }
    [StructLayout(LayoutKind.Explicit)]
    public struct Union
    {
        [FieldOffset(0)]
        public EXCEPTION_DEBUG_INFO Exception;
        [FieldOffset(0)]
        public CREATE_THREAD_DEBUG_INFO CreateThread;
        //[FieldOffset(0)]
        //public CREATE_PROCESS_DEBUG_INFO CreateProcessInfo;
        //[FieldOffset(0)]
        //public EXIT_THREAD_DEBUG_INFO ExitThread;
        //[FieldOffset(0)]
        //public EXIT_PROCESS_DEBUG_INFO ExitProcess;
        //[FieldOffset(0)]
        //public LOAD_DLL_DEBUG_INFO LoadDll;
        //[FieldOffset(0)]
        //public UNLOAD_DLL_DEBUG_INFO UnloadDll;
        //[FieldOffset(0)]
        //public OUTPUT_DEBUG_STRING_INFO DebugString;
        //[FieldOffset(0)]
        //public RIP_INFO RipInfo;
    }
    //1:EXCEPTION_DEBUG_INFO
    [StructLayout(LayoutKind.Sequential)]
    public struct EXCEPTION_DEBUG_INFO
    {
        public EXCEPTION_RECORD ExceptionRecord;
        public uint dwFirstChance;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct EXCEPTION_RECORD
    {
        public uint ExceptionCode;
        public uint ExceptionFlags;
        public IntPtr ExceptionRecord;
        public IntPtr ExceptionAddress;
        public uint NumberParameters;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15, ArraySubType = UnmanagedType.U4)]
        public uint[] ExceptionInformation;
    }
    //2:CREATE_THREAD_DEBUG_INFO
    public delegate uint PTHREAD_START_ROUTINE(IntPtr lpThreadParameter);
    [StructLayout(LayoutKind.Sequential)]
    public struct CREATE_THREAD_DEBUG_INFO
    {
        public IntPtr hThread;
        public IntPtr lpThreadLocalBase;
        public PTHREAD_START_ROUTINE lpStartAddress;
    }
    //3:CREATE_PROCESS_DEBUG_INFO CreateProcessInfo;
    public struct CREATE_PROCESS_DEBUG_INFO
    {
        public IntPtr hFile;
        public IntPtr hProcess;
        public IntPtr hThread;
        public IntPtr lpBaseOfImage;
        public uint dwDebugInfoFileOffset;
        public uint nDebugInfoSize;
        public IntPtr lpThreadLocalBase;
        public PTHREAD_START_ROUTINE lpStartAddress;
        public IntPtr lpImageName;
        public ushort fUnicode;
    }
    //4:EXIT_THREAD_DEBUG_INFO ExitThread;
    [StructLayout(LayoutKind.Sequential)]
    public struct EXIT_THREAD_DEBUG_INFO
    {
        public uint dwExitCode;
    }
    //5:public EXIT_PROCESS_DEBUG_INFO ExitProcess;
    [StructLayout(LayoutKind.Sequential)]
    public struct EXIT_PROCESS_DEBUG_INFO
    {
        public uint dwExitCode;
    }
    //6:public LOAD_DLL_DEBUG_INFO LoadDll;
    [StructLayout(LayoutKind.Sequential)]
    public struct LOAD_DLL_DEBUG_INFO
    {
        public IntPtr hFile;
        public IntPtr lpBaseOfDll;
        public uint dwDebugInfoFileOffset;
        public uint nDebugInfoSize;
        public IntPtr lpImageName;
        public ushort fUnicode;
    }
    //7:public UNLOAD_DLL_DEBUG_INFO UnloadDll;
    [StructLayout(LayoutKind.Sequential)]
    public struct UNLOAD_DLL_DEBUG_INFO
    {
        public IntPtr lpBaseOfDll;
    }
    //8:public OUTPUT_DEBUG_STRING_INFO DebugString;
    [StructLayout(LayoutKind.Sequential)]
    public struct OUTPUT_DEBUG_STRING_INFO
    {
        [MarshalAs(UnmanagedType.LPStr)]
        public string lpDebugStringData;
        public ushort fUnicode;
        public ushort nDebugStringLength;
    }
    //9:public RIP_INFO RipInfo;
    [StructLayout(LayoutKind.Sequential)]
    public struct RIP_INFO
    {
        public uint dwError;
        public uint dwType;
    }
    #endregion

    #region 并口

    [StructLayout(LayoutKind.Sequential)]
    public struct OVERLAPPED
    {
        int Internal;
        int InternalHigh;
        int Offset;
        int OffSetHigh;
        int hEvent;
    }


    #endregion
}

