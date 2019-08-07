using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Buffalo.Kernel.Media
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct AVISTREAMINFOW
    {
        public UInt32 fccType;
        public UInt32 fccHandler;
        public UInt32 dwFlags;
        public UInt32 dwCaps;
        public UInt16 wPriority;
        public UInt16 wLanguage;
        public UInt32 dwScale;
        public UInt32 dwRate;
        public UInt32 dwStart;
        public UInt32 dwLength;
        public UInt32 dwInitialFrames;
        public UInt32 dwSuggestedBufferSize;
        public UInt32 dwQuality;
        public UInt32 dwSampleSize;
        public UInt32 rect_left;
        public UInt32 rect_top;
        public UInt32 rect_right;
        public UInt32 rect_bottom;
        public UInt32 dwEditCount;
        public UInt32 dwFormatChangeCount;
        public UInt16 szName0;
        public UInt16 szName1;
        public UInt16 szName2;
        public UInt16 szName3;
        public UInt16 szName4;
        public UInt16 szName5;
        public UInt16 szName6;
        public UInt16 szName7;
        public UInt16 szName8;
        public UInt16 szName9;
        public UInt16 szName10;
        public UInt16 szName11;
        public UInt16 szName12;
        public UInt16 szName13;
        public UInt16 szName14;
        public UInt16 szName15;
        public UInt16 szName16;
        public UInt16 szName17;
        public UInt16 szName18;
        public UInt16 szName19;
        public UInt16 szName20;
        public UInt16 szName21;
        public UInt16 szName22;
        public UInt16 szName23;
        public UInt16 szName24;
        public UInt16 szName25;
        public UInt16 szName26;
        public UInt16 szName27;
        public UInt16 szName28;
        public UInt16 szName29;
        public UInt16 szName30;
        public UInt16 szName31;
        public UInt16 szName32;
        public UInt16 szName33;
        public UInt16 szName34;
        public UInt16 szName35;
        public UInt16 szName36;
        public UInt16 szName37;
        public UInt16 szName38;
        public UInt16 szName39;
        public UInt16 szName40;
        public UInt16 szName41;
        public UInt16 szName42;
        public UInt16 szName43;
        public UInt16 szName44;
        public UInt16 szName45;
        public UInt16 szName46;
        public UInt16 szName47;
        public UInt16 szName48;
        public UInt16 szName49;
        public UInt16 szName50;
        public UInt16 szName51;
        public UInt16 szName52;
        public UInt16 szName53;
        public UInt16 szName54;
        public UInt16 szName55;
        public UInt16 szName56;
        public UInt16 szName57;
        public UInt16 szName58;
        public UInt16 szName59;
        public UInt16 szName60;
        public UInt16 szName61;
        public UInt16 szName62;
        public UInt16 szName63;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct AVICOMPRESSOPTIONS
    {
        public UInt32 fccType;
        public UInt32 fccHandler;
        public UInt32 dwKeyFrameEvery;

        public UInt32 dwQuality;
        public UInt32 dwBytesPerSecond;

        public UInt32 dwFlags;
        public IntPtr lpFormat;
        public UInt32 cbFormat;
        public IntPtr lpParms;
        public UInt32 cbParms;
        public UInt32 dwInterleaveEvery;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct BITMAPINFOHEADER
    {
        public UInt32 biSize;
        public Int32 biWidth;
        public Int32 biHeight;
        public Int16 biPlanes;
        public Int16 biBitCount;
        public UInt32 biCompression;
        public UInt32 biSizeImage;
        public Int32 biXPelsPerMeter;
        public Int32 biYPelsPerMeter;
        public UInt32 biClrUsed;
        public UInt32 biClrImportant;
    }
    /// <summary>
    /// AVIWriter 的摘要说明，chenpeng,Email:ceponline@yahoo.com.cn。
    /// </summary>
    public class AVIWriter
    {
        const string AVIFILE32 = "AVIFIL32";
        private int _pfile = 0;
        private IntPtr _ps = new IntPtr(0);
        private IntPtr _psCompressed = new IntPtr(0);
        private UInt32 _frameRate = 0;
        private int _count = 0;
        private UInt32 _width = 0;
        private UInt32 _stride = 0;
        private UInt32 _height = 0;
        //avi标识
        private UInt32 _fccType = 1935960438; // vids
        private UInt32 _fccHandler = 808810089;// IV50
    
        private Bitmap _bmp;

        [DllImport(AVIFILE32)]
        private static extern void AVIFileInit();

        [DllImport(AVIFILE32)]
        private static extern int AVIFileOpenW(ref int ptr_pfile, [MarshalAs(UnmanagedType.LPWStr)]string fileName, int flags, int dummy);

        [DllImport(AVIFILE32)]
        private static extern int AVIFileCreateStream(int ptr_pfile, out IntPtr ptr_ptr_avi, ref AVISTREAMINFOW ptr_streaminfo);
        [DllImport(AVIFILE32)]
        private static extern int AVIMakeCompressedStream(out IntPtr ppsCompressed, IntPtr aviStream, ref AVICOMPRESSOPTIONS ao, int dummy);

        [DllImport(AVIFILE32)]
        private static extern int AVIStreamSetFormat(IntPtr aviStream, Int32 lPos, ref BITMAPINFOHEADER lpFormat, Int32 cbFormat);

        [DllImport(AVIFILE32)]
        unsafe private static extern int AVISaveOptions(int hwnd, UInt32 flags,int nStreams, IntPtr* ptr_ptr_avi, AVICOMPRESSOPTIONS** ao);

        [DllImport(AVIFILE32)]
        private static extern int AVIStreamWrite(IntPtr aviStream, Int32 lStart,Int32 lSamples, IntPtr lpBuffer, Int32 cbBuffer, Int32 dwFlags, Int32 dummy1, Int32 dummy2);

        [DllImport(AVIFILE32)]
        private static extern int AVIStreamRelease(IntPtr aviStream);

        [DllImport(AVIFILE32)]
        private static extern int AVIFileRelease(int pfile);

        [DllImport(AVIFILE32)]
        private static extern void AVIFileExit();



        public class AviException : ApplicationException
        {
            public AviException(string s) : base(s) { }
            public AviException(string s, Int32 hr)
                : base(s)
            {

                if (hr == AVIERR_BADPARAM)
                {
                    err_msg = "AVIERR_BADPARAM";
                }
                else
                {
                    err_msg = "unknown";
                }
            }

            public string ErrMsg()
            {
                return err_msg;
            }
            private const Int32 AVIERR_BADPARAM = -2147205018;
            private string err_msg;
        }

        public Bitmap Create(string fileName, UInt32 frameRate, int width, int
            height)
        {
            _frameRate = frameRate;
            _width = (UInt32)width;
            _height = (UInt32)height;
            _bmp = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            //锁定为24位位图
            BitmapData bmpDat = _bmp.LockBits(new Rectangle(0, 0, width,
                height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            _stride = (UInt32)bmpDat.Stride;
            _bmp.UnlockBits(bmpDat);
            AVIFileInit();
            int hr = AVIFileOpenW(ref _pfile, fileName, 4097, 0);
            if (hr != 0)
            {
                throw new AviException("Create错误!");
            }

            CreateStream();
            SetOptions();

            return _bmp;
        }

        public void AddFrame()
        {

            BitmapData bmpDat = _bmp.LockBits(
                new Rectangle(0, 0, (int)_width, (int)_height),
                ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

            int hr = AVIStreamWrite(_psCompressed, _count, 1,
                bmpDat.Scan0, 
                (Int32)(_stride * _height),
                0, 
                0,
                0);

            if (hr != 0)
            {
                throw new AviException("AVIStreamWrite");
            }

            _bmp.UnlockBits(bmpDat);

            _count++;
        }

        public void LoadFrame(Bitmap nextframe)
        {

            _bmp = new Bitmap(nextframe);
        }

        public void Close()
        {
            AVIStreamRelease(_ps);
            AVIStreamRelease(_psCompressed);

            AVIFileRelease(_pfile);
            AVIFileExit();

            
        }

        /// <summary>
        /// 创建流文件
        /// </summary>
        private void CreateStream()
        {
            AVISTREAMINFOW strhdr = new AVISTREAMINFOW();
            strhdr.fccType = _fccType;
            strhdr.fccHandler = _fccHandler;
            strhdr.dwFlags = 0;
            strhdr.dwCaps = 0;
            strhdr.wPriority = 0;
            strhdr.wLanguage = 0;
            strhdr.dwScale = 1;
            strhdr.dwRate = _frameRate; 
            strhdr.dwStart = 0;
            strhdr.dwLength = 0;
            strhdr.dwInitialFrames = 0;
            strhdr.dwSuggestedBufferSize = _height * _stride;
            strhdr.dwQuality = 0xffffffff; 
            strhdr.dwSampleSize = 0;
            strhdr.rect_top = 0;
            strhdr.rect_left = 0;
            strhdr.rect_bottom = _height;
            strhdr.rect_right = _width;
            strhdr.dwEditCount = 0;
            strhdr.dwFormatChangeCount = 0;
            strhdr.szName0 = 0;
            strhdr.szName1 = 0;

            int hr = AVIFileCreateStream(_pfile, out _ps, ref strhdr);

            if (hr != 0)
            {
                throw new AviException("AVIFileCreateStream");
            }
        }


        private byte[] _compressedOptionData;

        /// <summary>
        /// 压缩选项数据
        /// </summary>
        public byte[] CompressedOptionData
        {
            get { return _compressedOptionData; }
            set { _compressedOptionData = value; }
        }



        unsafe private AVICOMPRESSOPTIONS GetOption()
        {
            IntPtr x = _ps;
            IntPtr* ptr_ps = &x;
            AVICOMPRESSOPTIONS _compressedOption;
            if (_compressedOptionData != null )
            {
                try
                {
                    _compressedOption = CommonMethods.RawDeserialize<AVICOMPRESSOPTIONS>(_compressedOptionData);
                    return _compressedOption;
                }
                catch { }
            }

            _compressedOption = new AVICOMPRESSOPTIONS();
            _compressedOption.fccType = _fccType;
            _compressedOption.fccHandler = 0;
            _compressedOption.dwKeyFrameEvery = 0;
            _compressedOption.dwQuality = 0;
            _compressedOption.dwFlags = 0;
            _compressedOption.dwBytesPerSecond = 0;
            _compressedOption.lpFormat = new IntPtr(0);
            _compressedOption.cbFormat = 0;
            _compressedOption.lpParms = new IntPtr(0);
            _compressedOption.cbParms = 0;
            _compressedOption.dwInterleaveEvery = 0;

            AVICOMPRESSOPTIONS* p = &_compressedOption;
            AVICOMPRESSOPTIONS** pp = &p;

            AVISaveOptions(0, 0, 1, ptr_ps, pp);
            
             _compressedOptionData = CommonMethods.RawSerialize(_compressedOption);
            
            return _compressedOption;

        }


        /// <summary>
        /// 设置参数
        /// </summary>
        unsafe private void SetOptions()
        {
            

            AVICOMPRESSOPTIONS opts = GetOption();

            

            //AVISaveOptions(0, 0, 1, ptr_ps, pp);

            int hr = AVIMakeCompressedStream(out _psCompressed, _ps, ref
                opts, 0);
            if (hr != 0)
            {
                throw new AviException("AVIMakeCompressedStream");
            }

            BITMAPINFOHEADER bi = new BITMAPINFOHEADER();
            bi.biSize = 40;
            bi.biWidth = (Int32)_width;
            bi.biHeight = (Int32)_height;
            bi.biPlanes = 1;
            bi.biBitCount = 24;
            bi.biCompression = 0; 
            bi.biSizeImage = _stride * _height;
            bi.biXPelsPerMeter = 0;
            bi.biYPelsPerMeter = 0;
            bi.biClrUsed = 0;
            bi.biClrImportant = 0;

            hr = AVIStreamSetFormat(_psCompressed, 0, ref bi, 40);
            if (hr != 0)
            {
                throw new AviException("AVIStreamSetFormat", hr);
            }
        }

    }
}
