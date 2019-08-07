using System;
using System.Data;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Buffalo.Kernel
{
    public class Picture
    {
	    private Picture()
	    {
		    //
		    // TODO: 在此处添加构造函数逻辑
		    //
	    }
        /// <summary>
        /// 规范尺寸
        /// </summary>
        /// <param name="maxWidth">最大宽度</param>
        /// <param name="maxHeight">最大高度</param>
        /// <param name="size">待规范的尺寸</param>
        private static Size ScopeSize(int maxWidth, int maxHeight, Size size) 
        {

            Size ret=new Size();
            ret.Height = size.Height;
            ret.Width = size.Width;

            if (size.Height > maxHeight)//先尝试按高缩小
            {
                ret.Width = maxHeight * size.Width / size.Height;
                ret.Height = maxHeight;
            }

            if (ret.Width > maxWidth) //然后判断宽是否合规格
            {
                ret.Height = maxWidth * size.Height / size.Width;
                ret.Width = maxWidth;
            }
            return ret;
        }

        /// <summary>
        /// 把图片转换大小(大小不能超过指定值)
        /// </summary>
        /// <param name="img"></param>
        /// <param name="maxWidth"></param>
        /// <param name="maxHeight"></param>
        /// <returns></returns>
        public static Image ReSizePictureInScope(Image imgSource, int maxWidth, int maxHeight) 
        {
            Size objSize = new Size(imgSource.Width, imgSource.Height);//获取当前图片尺寸
            Size retSize = ScopeSize(maxWidth, maxHeight, objSize);
            return ChangeSize(imgSource, retSize);
        }


        /// <summary>
        /// 把图片流转成字节
        /// </summary>
        /// <param name="fileStm">图片流</param>
        /// <param name="width">转换后的宽度</param>
        /// <param name="height">转换后的高度</param>
        /// <returns></returns>
        public static byte[] PictureToBytes(Stream fileStm,int width,int height,ImageFormat format)
        {

            Image bmpSource = null;
            Image bmpRet = null;
			byte[] imgBytes=null;
			MemoryStream stmTmp=new MemoryStream();
			try
			{
				bmpSource = new Bitmap(fileStm);
				bmpRet = ChangeSize(bmpSource,new Size(width,height));
                bmpRet.Save(stmTmp, format);
				imgBytes=new byte[stmTmp.Length];
				stmTmp.Position=0;
				stmTmp.Read(imgBytes,0,imgBytes.Length);
			}
			finally
			{
				stmTmp.Flush();
				stmTmp.Close();
				bmpRet.Dispose();
				bmpSource.Dispose();
			}
			return imgBytes;
        }

        /// <summary>
		/// 把图片转换成字节
		/// </summary>
		/// <param name="img">图片</param>
		/// <returns></returns>
        public static byte[] PictureToBytes(Image img)
        {
            return PictureToBytes(img, System.Drawing.Imaging.ImageFormat.Jpeg);
        }
        /// <summary>
        /// 获取特定的图像编解码信息
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        public static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        

		/// <summary>
		/// 把图片转换成字节
		/// </summary>
		/// <param name="img">图片</param>
		/// <returns></returns>
        public static byte[] PictureToBytes(Image img, System.Drawing.Imaging.ImageFormat format)
		{
			byte[] imgBytes=null;
			MemoryStream stm=new MemoryStream();
			try
			{
                img.Save(stm, format);
                
				imgBytes=new byte[stm.Length];
				stm.Position=0;
				stm.Read(imgBytes,0,imgBytes.Length);
				
			}
			finally
			{
				stm.Flush();
				stm.Close();
				//img.Dispose();
			}
			return imgBytes;
		}

        /// <summary>
        /// 图片转换为字节
        /// </summary>
        /// <param name="img">图片</param>
        /// <param name="codeInfo">编码</param>
        /// <param name="qty">质量</param>
        /// <returns></returns>
        public static byte[] PictureToBytes(Image img, ImageCodecInfo codeInfo, long qty) 
        {
            byte[] imgBytes = null;
            MemoryStream stm = new MemoryStream();
            try
            {
                WhitePictureToStream(stm, img, codeInfo, qty);

                imgBytes = new byte[stm.Length];
                stm.Position = 0;
                stm.Read(imgBytes, 0, imgBytes.Length);

            }
            finally
            {
                stm.Flush();
                stm.Close();
                //img.Dispose();
            }
            return imgBytes;
        }

        /// <summary>
        /// 把图片写入流
        /// </summary>
        /// <param name="img"></param>
        /// <param name="codeInfo"></param>
        /// <param name="qty"></param>
        public static void WhitePictureToStream(Stream stm,Image img, ImageCodecInfo codeInfo,long qty) 
        {
            Encoder picEncoder = Encoder.Quality;
            EncoderParameters picEncoderParameters=new EncoderParameters(1);
            EncoderParameter qtyEncoderParameter = new EncoderParameter(picEncoder, qty); //指定质量
            picEncoderParameters.Param[0] = qtyEncoderParameter;
            img.Save(stm, codeInfo, picEncoderParameters);
        }
		
		/// <summary>
		/// 把字节转换为图片
		/// </summary>
		/// <param name="byteImage">图片字节</param>
		/// <returns></returns>
        public static Bitmap BytesToPicture(byte[] byteImage)
		{
			if(byteImage==null)
			{
				return null;
			}
			Bitmap bmp=null;
			
			MemoryStream stm=null;
			try
			{
				stm=new MemoryStream(byteImage);
				
				stm.Position=0;
				bmp=new Bitmap(stm);
				//img=Image.FromStream(stm);
				
			}
			finally
			{
				stm.Flush();
				stm.Close();
			}
			return bmp;
		}

		/// <summary>
		/// 变换图片大小
		/// </summary>
		/// <param name="img">源图片</param>
		/// <param name="width">变换后的宽</param>
		/// <param name="height">变换后的高</param>
		/// <returns></returns>
		public static Bitmap ReSizePicturee(Image img,int width,int height)
		{
			Size newSize=new Size(width,height);
			return new Bitmap(img,newSize);
		}

		/// <summary>
		/// 读取文件
		/// </summary>
		/// <param name="root">文件路径</param>
		/// <returns></returns>
		public static byte[] ReadFile(string root)
		{
			FileStream file=new FileStream(root,FileMode.Open,FileAccess.Read);
			byte[] byes=new byte[file.Length];
			try
			{
				file.Position=0;
				file.Read(byes,0,byes.Length);
			}
			finally
			{
				file.Flush();
				file.Close();
			}
			return byes;

		}


        

        /// <summary>
        /// 改变图片尺寸
        /// </summary>
        /// <param name="originalImage">源图片</param>
        /// <param name="picSize">尺寸</param>
        /// <returns></returns>
        private static Image ChangeSize(Image originalImage, Size picSize) 
        {
            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;
            //新建一个bmp图片
            Image bitmap = new System.Drawing.Bitmap(picSize.Width, picSize.Height);
            //新建一个画板
            Graphics g = System.Drawing.Graphics.FromImage(bitmap);
            try
            {
                //设置高质量插值法
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;

                //设置高质量,低速度呈现平滑程度
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                //清空画布并以透明背景色填充
                g.Clear(Color.Transparent);

                //在指定位置并且按指定大小绘制原图片的指定部分
                g.DrawImage(originalImage, new Rectangle(0, 0, picSize.Width, picSize.Height),
                    new Rectangle(x, y, ow, oh),
                    GraphicsUnit.Pixel);
            }
            finally
            {
                g.Dispose();
            }
            return bitmap;
        }


        /// <summary>
        /// 获取编码信息
        /// </summary>
        /// <param name="mimeType">类型</param>
        /// <returns></returns>
        private static ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }

        /// <summary>
        /// 原样保存图片，不做任何格式修改
        /// </summary>
        /// <param name="stmImg">图片流</param>
        /// <param name="root">保存路径</param>
        public static void SaveImage(Stream stmImg, string root) 
        {
            FileStream file = new FileStream(root, FileMode.Create, FileAccess.Write);
            int tmp = -1;
            try
            {
                while ((tmp = stmImg.ReadByte()) >= 0)
                {
                    file.WriteByte((byte)tmp);
                }
            }
            finally 
            {
                file.Flush();
                file.Close();
            }

        }
    }
}