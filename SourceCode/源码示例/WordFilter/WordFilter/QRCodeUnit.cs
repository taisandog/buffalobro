using System;
using System.Collections.Generic;
using System.Text;
using ZXing.QrCode;
using ZXing;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using ZXing.Common;
using Buffalo.Kernel;


namespace WordFilter
{
    /// <summary>
    /// 二维码工具
    /// </summary>
    public class QRCodeUnit
    {
        QrCodeEncodingOptions _options = new QrCodeEncodingOptions();
        /// <summary>
        /// 生成二维码的配置
        /// </summary>
        public QrCodeEncodingOptions Options
        {
            get { return _options; }
        }
        BarcodeWriter _writer = null;

        private readonly static Encoding DefaultEncode = Encoding.UTF8;
        public QRCodeUnit() 
        {
            _options.DisableECI = true;
            _options.CharacterSet = "UTF-8";
            _writer = new BarcodeWriter();
            _writer.Format = BarcodeFormat.QR_CODE;
            _writer.Options = _options;

        }



        /// <summary>
        /// 获取剪贴板的文件
        /// </summary>
        /// <returns></returns>
        private Bitmap GetClipboardBitmap()
        {
            Bitmap bmp = null;
            if (Clipboard.ContainsImage())
            {
                bmp = Clipboard.GetData(DataFormats.Bitmap) as Bitmap;
                if (bmp != null)
                {
                    return bmp;
                }
            }
            if (Clipboard.ContainsFileDropList())
            {
                string[] strs = Clipboard.GetData(DataFormats.FileDrop) as string[];
                if (strs != null && strs.Length > 0)
                {
                    string path = strs[0];
                    try
                    {
                        if (File.Exists(path))
                        {
                            bmp = Bitmap.FromFile(path) as Bitmap;
                            return bmp;
                        }
                    }
                    catch { }
                }
            }
            return null;
        }

        /// <summary>
        /// 获取二维码字符串
        /// </summary>
        /// <returns></returns>
        public string GetQRCodeString()
        {

            Bitmap bmap = GetClipboardBitmap();

            if (bmap == null)
            {
                return null;
            }
            QRCodeReader qrRead = new QRCodeReader();
            BitmapLuminanceSource source = new BitmapLuminanceSource(bmap);
            BinaryBitmap binBitmap = new BinaryBitmap(new HybridBinarizer(source));

            string retString = null;
            try
            {
                Result results = qrRead.decode(binBitmap);
                if (results != null)
                {
                    retString = DeEncryString(results.Text);
                }
            }
            catch (Exception ex)
            {
                return null;
            }

            return retString;
        }

        /// <summary>
        /// 解密字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string DeEncryString(string str) 
        {
            if (str.IndexOf("[Encry]") == 0) 
            {
                string content = str.Substring("[Encry]".Length);
                byte[] arrStr = CommonMethods.HexStringToBytes(content);
                int tmp = 0;
                for (int i = 0; i < arrStr.Length; i++)
                {
                    tmp = arrStr[i] - 128;
                    if (tmp < 0) 
                    {
                        tmp = tmp + 256;
                    }
                    tmp = (tmp ^ 256);
                    arrStr[i] = (byte)tmp;
                }
                return DefaultEncode.GetString(arrStr);
            }
            return str;
        }

        /// <summary>
        /// 根据字符串生成二维码
        /// </summary>
        /// <param name="content">字符串</param>
        /// <returns></returns>
        public Bitmap GetQRCode(string content) 
        {
            Bitmap bmp = _writer.Write(content);
            return bmp;
        }

        /// <summary>
        /// 根据字符串生成加密二维码
        /// </summary>
        /// <param name="content">字符串</param>
        /// <returns></returns>
        public Bitmap GetEncryQRCode(string content)
        {
            StringBuilder sbCode = new StringBuilder(content.Length * 2);
            sbCode.Append("[Encry]");
            sbCode.Append(EncryString(content));
            Bitmap bmp = _writer.Write(sbCode.ToString());
            return bmp;
        }

        

        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        private string EncryString(string content) 
        {
            byte[] arrStr = DefaultEncode.GetBytes(content);
            byte tmp = 0;
            for (int i = 0; i < arrStr.Length; i++) 
            {
                tmp = (byte)(arrStr[i] ^ 256);
                tmp = (byte)((tmp + 128) % 256);
                arrStr[i] = tmp;
            }
            //return Convert.ToBase64String(arrStr);
            return CommonMethods.BytesToHexString(arrStr);
        }
    }
}
