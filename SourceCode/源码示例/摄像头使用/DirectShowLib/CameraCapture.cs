using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;

namespace DirectShowLib
{
    ///  Summary description for MainForm.  
    public class CameraCapture : ISampleGrabberCB, IDisposable
    {
        #region Member variables

        ///  graph builder interface.  
        private IFilterGraph2 m_graphBuilder = null;

        // Used to snap picture on Still pin 
        private IAMVideoControl m_VidControl = null;
        private IPin m_pinStill = null;

        ///  so we can wait for the async job to finish  
        private ManualResetEvent m_PictureReady = null;

        private bool m_WantOne = false;

        ///  Dimensions of the image, calculated once in constructor for perf.  
        private int m_videoWidth;
        private int m_videoHeight;
        private int m_stride;

        ///  buffer for bitmap data.  Always release by caller 
        private IntPtr m_ipBuffer = IntPtr.Zero;

#if DEBUG
        // Allow you to "Connect to remote graph" from GraphEdit 
        DsROTEntry m_rot = null;
#endif
        #endregion

        #region APIs
        [DllImport("Kernel32.dll", EntryPoint = "RtlMoveMemory")]
        private static extern void CopyMemory(IntPtr Destination, IntPtr Source, [MarshalAs(UnmanagedType.U4)] int Length);
        #endregion
        const int VIDEODEVICE = 0; // zero based index of video capture device to use 
        //const int VIDEOWIDTH = 640; // Depends on video device caps 
        //const int VIDEOHEIGHT = 480; // Depends on video device caps 
        const int VIDEOBITSPERPIXEL = 24; // BitsPerPixel values determined by device 
        const int VIDEODEVICE1 = 1;

        public static CameraCapture CreateCapture(Control hControl, int width, int height) 
        {
            return new CameraCapture(VIDEODEVICE, width, height, VIDEOBITSPERPIXEL, hControl); 
        }

        /// <summary>
        /// 创建视频
        /// </summary>
        /// <param name="hControl"></param>
        /// <returns></returns>
        public static CameraCapture CreateCapture(Control hControl)
        {
            return CreateCapture(hControl, hControl.Width, hControl.Height);
        }
        // Zero based device index and device params and output window 
        public CameraCapture(int iDeviceNum, int iWidth, int iHeight, short iBPP, Control hControl)
        {
            DsDevice[] capDevices;

            // Get the collection of video devices 
            capDevices = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);

            if (iDeviceNum + 1 > capDevices.Length)
            {
                throw new Exception("No video capture devices found at that index!");
            }

            try
            {
                // Set up the capture graph 
                SetupGraph(capDevices[iDeviceNum], iWidth, iHeight, iBPP, hControl);

                // tell the callback to ignore new images 
                m_PictureReady = new ManualResetEvent(false);
            }
            catch(Exception ex)
            {
                Dispose();
                throw ex;
            }
        }

        ///  release everything.  
        public void Dispose()
        {
#if DEBUG
            if (m_rot != null)
            {
                m_rot.Dispose();
            }
#endif
            CloseInterfaces();
            if (m_PictureReady != null)
            {
                m_PictureReady.Close();
            }
        }
        // Destructor 
        ~CameraCapture()
        {
            Dispose();
        }
        IntPtr curCaptureImageHandle = IntPtr.Zero;//当前截图的句柄

        /// <summary>
        /// 释放当前的截图
        /// </summary>
        public void FreeCaptureImage() 
        {
            if (curCaptureImageHandle != IntPtr.Zero)
            {
                Marshal.FreeCoTaskMem(curCaptureImageHandle);
                curCaptureImageHandle = IntPtr.Zero;
            }
        }

        /// <summary>
        /// 获取当前图片
        /// </summary>
        /// <returns></returns>
        public Image GetCaptureImage() 
        {
            
            IntPtr m_ip = Click();
            Bitmap bmp = new Bitmap(Width, Height, Stride, PixelFormat.Format24bppRgb, m_ip);
            bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);

            return bmp;
        }

        ///  
        /// Get the image from the Still pin.  The returned image can turned into a bitmap with 
        /// Bitmap b = new Bitmap(cam.Width, cam.Height, cam.Stride, PixelFormat.Format24bppRgb, m_ip); 
        /// If the image is upside down, you can fix it with 
        /// b.RotateFlip(RotateFlipType.RotateNoneFlipY); 
        ///  
        /// Returned pointer to be freed by caller with Marshal.FreeCoTaskMem 
        private IntPtr Click()
        {
            int hr;

            // get ready to wait for new image 
            m_PictureReady.Reset();
            m_ipBuffer = Marshal.AllocCoTaskMem(Math.Abs(m_stride) * m_videoHeight);

            try
            {
                m_WantOne = true;

                // If we are using a still pin, ask for a picture 
                if (m_VidControl != null)
                {
                    // Tell the camera to send an image 
                    hr = m_VidControl.SetMode(m_pinStill, VideoControlFlags.Trigger);
                    DsError.ThrowExceptionForHR(hr);
                }

                // Start waiting 
                if (!m_PictureReady.WaitOne(9000, false))
                {
                    throw new Exception("Timeout waiting to get picture");
                }
            }
            catch
            {
                Marshal.FreeCoTaskMem(m_ipBuffer);
                m_ipBuffer = IntPtr.Zero;
            }

            // Got one 
            return m_ipBuffer;
        }

        

        public int Width
        {
            get
            {
                return m_videoWidth;
            }
        }
        public int Height
        {
            get
            {
                return m_videoHeight;
            }
        }
        public int Stride
        {
            get
            {
                return m_stride;
            }
        }


        ///  build the capture graph for grabber.  
        private void SetupGraph(DsDevice dev, int iWidth, int iHeight, short iBPP, Control hControl)
        {
            int hr;

            ISampleGrabber sampGrabber = null;
            IBaseFilter capFilter = null;
            IPin pCaptureOut = null;
            IPin pSampleIn = null;
            IPin pRenderIn = null;

            // Get the graphbuilder object 
            m_graphBuilder = new FilterGraph() as IFilterGraph2;

            try
            {
#if DEBUG
                m_rot = new DsROTEntry(m_graphBuilder);
#endif
                // add the video input device 
                hr = m_graphBuilder.AddSourceFilterForMoniker(dev.Mon, null, dev.Name, out capFilter);
                DsError.ThrowExceptionForHR(hr);

                // Find the still pin 
                m_pinStill = DsFindPin.ByCategory(capFilter, PinCategory.Still, 0);

                // Didn't find one.  Is there a preview pin? 
                if (m_pinStill == null)
                {
                    m_pinStill = DsFindPin.ByCategory(capFilter, PinCategory.Preview, 0);
                }

                // Still haven't found one.  Need to put a splitter in so we have 
                // one stream to capture the bitmap from, and one to display.  Ok, we 
                // don't *have* to do it that way, but we are going to anyway. 
                if (m_pinStill == null)
                {
                    IPin pRaw = null;
                    IPin pSmart = null;

                    // There is no still pin 
                    m_VidControl = null;

                    // Add a splitter 
                    IBaseFilter iSmartTee = (IBaseFilter)new SmartTee();

                    try
                    {
                        hr = m_graphBuilder.AddFilter(iSmartTee, "SmartTee");
                        DsError.ThrowExceptionForHR(hr);

                        // Find the find the capture pin from the video device and the 
                        // input pin for the splitter, and connnect them 
                        pRaw = DsFindPin.ByCategory(capFilter, PinCategory.Capture, 0);
                        pSmart = DsFindPin.ByDirection(iSmartTee, PinDirection.Input, 0);

                        hr = m_graphBuilder.Connect(pRaw, pSmart);
                        DsError.ThrowExceptionForHR(hr);

                        // Now set the capture and still pins (from the splitter) 
                        m_pinStill = DsFindPin.ByName(iSmartTee, "Preview");
                        pCaptureOut = DsFindPin.ByName(iSmartTee, "Capture");

                        // If any of the default config items are set, perform the config 
                        // on the actual video device (rather than the splitter) 
                        if (iHeight + iWidth + iBPP > 0)
                        {
                            SetConfigParms(pRaw, iWidth, iHeight, iBPP);
                        }
                    }
                    finally
                    {
                        if (pRaw != null)
                        {
                            Marshal.ReleaseComObject(pRaw);
                        }
                        if (pRaw != pSmart)
                        {
                            Marshal.ReleaseComObject(pSmart);
                        }
                        if (pRaw != iSmartTee)
                        {
                            Marshal.ReleaseComObject(iSmartTee);
                        }
                    }
                }
                else
                {
                    // Get a control pointer (used in Click()) 
                    m_VidControl = capFilter as IAMVideoControl;

                    pCaptureOut = DsFindPin.ByCategory(capFilter, PinCategory.Capture, 0);

                    // If any of the default config items are set 
                    if (iHeight + iWidth + iBPP > 0)
                    {
                        SetConfigParms(m_pinStill, iWidth, iHeight, iBPP);
                    }
                }

                // Get the SampleGrabber interface 
                sampGrabber = new SampleGrabber() as ISampleGrabber;

                // Configure the sample grabber 
                IBaseFilter baseGrabFlt = sampGrabber as IBaseFilter;
                ConfigureSampleGrabber(sampGrabber);
                pSampleIn = DsFindPin.ByDirection(baseGrabFlt, PinDirection.Input, 0);

                // Get the default video renderer 
                IBaseFilter pRenderer = new VideoRendererDefault() as IBaseFilter;
                hr = m_graphBuilder.AddFilter(pRenderer, "Renderer");
                DsError.ThrowExceptionForHR(hr);

                pRenderIn = DsFindPin.ByDirection(pRenderer, PinDirection.Input, 0);

                // Add the sample grabber to the graph 
                hr = m_graphBuilder.AddFilter(baseGrabFlt, "Ds.NET Grabber");
                DsError.ThrowExceptionForHR(hr);

                if (m_VidControl == null)
                {
                    // Connect the Still pin to the sample grabber 
                    hr = m_graphBuilder.Connect(m_pinStill, pSampleIn);
                    DsError.ThrowExceptionForHR(hr);

                    // Connect the capture pin to the renderer 
                    hr = m_graphBuilder.Connect(pCaptureOut, pRenderIn);
                    DsError.ThrowExceptionForHR(hr);
                }
                else
                {
                    // Connect the capture pin to the renderer 
                    hr = m_graphBuilder.Connect(pCaptureOut, pRenderIn);
                    DsError.ThrowExceptionForHR(hr);

                    // Connect the Still pin to the sample grabber 
                    hr = m_graphBuilder.Connect(m_pinStill, pSampleIn);
                    DsError.ThrowExceptionForHR(hr);
                }

                // Learn the video properties 
                SaveSizeInfo(sampGrabber);
                ConfigVideoWindow(hControl);

                // Start the graph 
                IMediaControl mediaCtrl = m_graphBuilder as IMediaControl;
                hr = mediaCtrl.Run();
                DsError.ThrowExceptionForHR(hr);
            }
            finally
            {
                if (sampGrabber != null)
                {
                    Marshal.ReleaseComObject(sampGrabber);
                    sampGrabber = null;
                }
                if (pCaptureOut != null)
                {
                    Marshal.ReleaseComObject(pCaptureOut);
                    pCaptureOut = null;
                }
                if (pRenderIn != null)
                {
                    Marshal.ReleaseComObject(pRenderIn);
                    pRenderIn = null;
                }
                if (pSampleIn != null)
                {
                    Marshal.ReleaseComObject(pSampleIn);
                    pSampleIn = null;
                }
            }
        }

        private void SaveSizeInfo(ISampleGrabber sampGrabber)
        {
            int hr;

            // Get the media type from the SampleGrabber 
            AMMediaType media = new AMMediaType();

            hr = sampGrabber.GetConnectedMediaType(media);
            DsError.ThrowExceptionForHR(hr);

            if ((media.formatType != FormatType.VideoInfo) || (media.formatPtr == IntPtr.Zero))
            {
                throw new NotSupportedException("Unknown Grabber Media Format");
            }

            // Grab the size info 
            VideoInfoHeader videoInfoHeader = (VideoInfoHeader)Marshal.PtrToStructure(media.formatPtr, typeof(VideoInfoHeader));
            m_videoWidth = videoInfoHeader.BmiHeader.Width;
            m_videoHeight = videoInfoHeader.BmiHeader.Height;
            m_stride = m_videoWidth * (videoInfoHeader.BmiHeader.BitCount / 8);

            DsUtils.FreeAMMediaType(media);
            media = null;
        }

        // Set the video window within the control specified by hControl 
        private void ConfigVideoWindow(Control hControl)
        {
            int hr;

            IVideoWindow ivw = m_graphBuilder as IVideoWindow;

            // Set the parent 
            hr = ivw.put_Owner(hControl.Handle);
            DsError.ThrowExceptionForHR(hr);

            // Turn off captions, etc 
            hr = ivw.put_WindowStyle(WindowStyle.Child | WindowStyle.ClipChildren | WindowStyle.ClipSiblings);
            DsError.ThrowExceptionForHR(hr);

            // Yes, make it visible 
            hr = ivw.put_Visible(OABool.True);
            DsError.ThrowExceptionForHR(hr);

            // Move to upper left corner 
            Rectangle rc = hControl.ClientRectangle;
            hr = ivw.SetWindowPosition(0, 0, rc.Right, rc.Bottom);
            DsError.ThrowExceptionForHR(hr);
        }

        private void ConfigureSampleGrabber(ISampleGrabber sampGrabber)
        {
            int hr;
            AMMediaType media = new AMMediaType();

            // Set the media type to Video/RBG24 
            media.majorType = MediaType.Video;
            media.subType = MediaSubType.RGB24;
            media.formatType = FormatType.VideoInfo;
            hr = sampGrabber.SetMediaType(media);
            DsError.ThrowExceptionForHR(hr);

            DsUtils.FreeAMMediaType(media);
            media = null;

            // Configure the samplegrabber 
            hr = sampGrabber.SetCallback(this, 1);
            DsError.ThrowExceptionForHR(hr);
        }

        // Set the Framerate, and video size 
        private void SetConfigParms(IPin pStill, int iWidth, int iHeight, short iBPP)
        {
            int hr;
            AMMediaType media;
            VideoInfoHeader v;

            IAMStreamConfig videoStreamConfig = pStill as IAMStreamConfig;

            // Get the existing format block 
            hr = videoStreamConfig.GetFormat(out media);
            DsError.ThrowExceptionForHR(hr);

            try
            {
                // copy out the videoinfoheader 
                v = new VideoInfoHeader();
                Marshal.PtrToStructure(media.formatPtr, v);

                // if overriding the width, set the width 
                if (iWidth > 0)
                {
                    v.BmiHeader.Width = iWidth;
                }

                // if overriding the Height, set the Height 
                if (iHeight > 0)
                {
                    v.BmiHeader.Height = iHeight;
                }

                // if overriding the bits per pixel 
                if (iBPP > 0)
                {
                    v.BmiHeader.BitCount = iBPP;
                }

                // Copy the media structure back 
                Marshal.StructureToPtr(v, media.formatPtr, false);

                // Set the new format 
                hr = videoStreamConfig.SetFormat(media);
                DsError.ThrowExceptionForHR(hr);
            }
            finally
            {
                DsUtils.FreeAMMediaType(media);
                media = null;
            }
        }

        ///  Shut down capture  
        private void CloseInterfaces()
        {
            int hr;

            try
            {
                if (m_graphBuilder != null)
                {
                    IMediaControl mediaCtrl = m_graphBuilder as IMediaControl;

                    // Stop the graph 
                    hr = mediaCtrl.Stop();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            if (m_graphBuilder != null)
            {
                Marshal.ReleaseComObject(m_graphBuilder);
                m_graphBuilder = null;
            }

            if (m_VidControl != null)
            {
                Marshal.ReleaseComObject(m_VidControl);
                m_VidControl = null;
            }

            if (m_pinStill != null)
            {
                Marshal.ReleaseComObject(m_pinStill);
                m_pinStill = null;
            }
        }

        ///  sample callback, NOT USED.  
        int ISampleGrabberCB.SampleCB(double SampleTime, IMediaSample pSample)
        {
            Marshal.ReleaseComObject(pSample);
            return 0;
        }

        ///  buffer callback, COULD BE FROM FOREIGN THREAD.  
        int ISampleGrabberCB.BufferCB(double SampleTime, IntPtr pBuffer, int BufferLen)
        {
            // Note that we depend on only being called once per call to Click.  Otherwise 
            // a second call can overwrite the previous image. 
            Debug.Assert(BufferLen == Math.Abs(m_stride) * m_videoHeight, "Incorrect buffer length");

            if (m_WantOne)
            {
                m_WantOne = false;
                Debug.Assert(m_ipBuffer != IntPtr.Zero, "Unitialized buffer");

                // Save the buffer 
                CopyMemory(m_ipBuffer, pBuffer, BufferLen);

                // Picture is ready. 
                m_PictureReady.Set();
            }

            return 0;
        }
    }
}
