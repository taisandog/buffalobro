using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Threading;

namespace Buffalo.Kernel
{
    /// <summary>
    /// 散列辅助类
    /// </summary>
    public class PasswordHash
    {
        private static Encoding DefaultEncoding = Encoding.UTF8;
        /// <summary>
        /// 对字符串进行MD5散列
        /// </summary>
        /// <param name="str">要散列的字符串</param>
        /// <param name="isUpper">字母是否大写</param>
        /// <returns></returns>
        public static string ToMD5String(string str, bool isUpper=true)
        {
            return ToMD5String(DefaultEncoding.GetBytes(str),isUpper);
        }
        /// <summary>
        /// 对内容进行MD5散列
        /// </summary>
        /// <param name="content">要散列的内容</param>
        /// <param name="isUpper">散列结果是否大写</param>
        /// <returns></returns>
        public static string ToMD5String(byte[] content, bool isUpper=true)
        {
            byte[] retBytes = ToMD5Bytes(content);
            return CommonMethods.BytesToHexString(retBytes,isUpper);
        }
        
        /// <summary>
        /// 对字符串进行MD5散列
        /// </summary>
        /// <param name="stm">要散列的流</param>
        ///  <param name="isUpper">散列结果是否大写</param>
        /// <returns></returns>
        public static string ToMD5String(Stream stm, bool isUpper = true)
        {
            byte[] retBytes = ToMD5Bytes(stm);
            return CommonMethods.BytesToHexString(retBytes,isUpper);
        }

        /// <summary>
        /// 开启批量哈希
        /// </summary>
        /// <returns></returns>
        public static BatchHash StartBatchHash() 
        {
            return new BatchHash();
        }

        /// <summary>
        /// 对字符串进行MD5散列
        /// </summary>
        /// <param name="str">要散列的字符串</param>
        /// <param name="isUpper">字母是否大写</param>
        /// <returns></returns>
        public static byte[] ToMD5Bytes(string str)
        {
            return ToMD5Bytes(DefaultEncoding.GetBytes(str));
        }
        /// <summary>
        /// 对内容进行MD5散列
        /// </summary>
        /// <param name="content">要散列的内容</param>
        /// <param name="isUpper">散列结果是否大写</param>
        /// <returns></returns>
        public static byte[] ToMD5Bytes(byte[] content)
        {
            MD5 md5Hash = BatchHash.GetMD5();
            byte[] retBytes = null;
            try
            {
                retBytes = md5Hash.ComputeHash(content);
            }
            finally
            {
                BatchHash.AutoClose(md5Hash);
            }
            return retBytes;
        }

        /// <summary>
        /// 对字符串进行MD5散列
        /// </summary>
        /// <param name="stm">要散列的流</param>
        ///  <param name="isUpper">散列结果是否大写</param>
        /// <returns></returns>
        public static byte[] ToMD5Bytes(Stream stm)
        {
            MD5 md5Hash = BatchHash.GetMD5();
            byte[] retBytes = null;
            try
            {
                retBytes = md5Hash.ComputeHash(stm);
            }
            finally
            {
                BatchHash.AutoClose(md5Hash);
            }
            return retBytes;

        }

        /// <summary>
        /// 对字符串进行SHA1散列
        /// </summary>
        /// <param name="str">要散列的字符串</param>
        /// <param name="isUpper">散列结果是否大写</param>
        /// <returns></returns>
        public static string ToSHA1String(string str, bool isUpper = true)
        {
            return ToSHA1String(Encoding.Default.GetBytes(str),isUpper);
        }
        

        /// <summary>
        /// 对内容进行SHA1散列
        /// </summary>
        /// <param name="content">要散列的内容</param>
        /// <param name="isUpper">散列结果是否大写</param>
        /// <returns></returns>
        public static string ToSHA1String(byte[] content, bool isUpper = true)
        {
            
            byte[] retBytes = ToSHA1Bytes(content);
            return CommonMethods.BytesToHexString(retBytes, isUpper);
        }
        
        /// <summary>
        /// 对内容进行SHA1散列
        /// </summary>
        /// <param name="stm">要散列的流</param>
        /// <param name="isUpper">散列结果是否大写</param>
        /// <returns></returns>
        public static string ToSHA1String(Stream stm,bool isUpper = true)
        {
            byte[] retBytes = ToSHA1Bytes(stm);
            return CommonMethods.BytesToHexString(retBytes, isUpper);
        }
        /// <summary>
        /// 对字符串进行SHA1散列
        /// </summary>
        /// <param name="str">要散列的字符串</param>
        /// <param name="isUpper">散列结果是否大写</param>
        /// <returns></returns>
        public static byte[] ToSHA1Bytes(string str)
        {
            return ToSHA1Bytes(Encoding.Default.GetBytes(str));
        }


        /// <summary>
        /// 对内容进行SHA1散列
        /// </summary>
        /// <param name="content">要散列的内容</param>
        /// <param name="isUpper">散列结果是否大写</param>
        /// <returns></returns>
        public static byte[] ToSHA1Bytes(byte[] content)
        {
           

            SHA1 hash = BatchHash.GetSHA1();
            byte[] retBytes = null;
            try
            {
                retBytes = hash.ComputeHash(content);
            }
            finally
            {
                BatchHash.AutoClose(hash);
            }
            return retBytes;
        }

        /// <summary>
        /// 对内容进行SHA1散列
        /// </summary>
        /// <param name="stm">要散列的流</param>
        /// <param name="isUpper">散列结果是否大写</param>
        /// <returns></returns>
        public static byte[] ToSHA1Bytes(Stream stm)
        {
            SHA1 hash = BatchHash.GetSHA1();
            byte[] retBytes = null;
            try
            {
                retBytes = hash.ComputeHash(stm);
            }
            finally
            {
                BatchHash.AutoClose(hash);
            }
            return retBytes;
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="toDecrypt">要解密的数据</param>
        /// <param name="pwd">解密密码(32字节数组)</param>
        /// <returns></returns>
        public static byte[] AESDecrypt(byte[] toDecrypt,byte[] pwd, 
            CipherMode mode = CipherMode.ECB, PaddingMode padding = PaddingMode.PKCS7)
        {
            using (RijndaelManaged rDel = new RijndaelManaged())
            {
                rDel.Key = pwd;
                rDel.Mode = mode;
                rDel.Padding = padding;

                using (ICryptoTransform cTransform = rDel.CreateEncryptor())
                {
                    byte[] resultArray = cTransform.TransformFinalBlock(toDecrypt, 0, toDecrypt.Length);

                    return resultArray;
                }
            }
        }

        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="toEncrypt">要加密的数据</param>
        /// <param name="pwd">密码(32字节数组)</param>
        /// <returns></returns>
        public static byte[] AESEncrypt(byte[] toEncrypt, byte[] pwd, 
            CipherMode mode = CipherMode.ECB, PaddingMode padding = PaddingMode.PKCS7)
        {
            using (RijndaelManaged rDel = new RijndaelManaged())
            {
                rDel.Key = pwd;
                rDel.Mode = mode;
                rDel.Padding = padding;

                using (ICryptoTransform cTransform = rDel.CreateEncryptor())
                {
                    byte[] resultArray = cTransform.TransformFinalBlock(toEncrypt, 0, toEncrypt.Length);

                    return resultArray;
                }
            }
        }
    }

    /// <summary>
    /// 批量哈希值
    /// </summary>
    public class BatchHash :IDisposable
    {
        /// <summary>
        /// 线程变量:是否开启批量
        /// </summary>
        private static ThreadLocal<bool> _tlIsBatch = new ThreadLocal<bool>();

        /// <summary>
        /// 线程变量:MD5
        /// </summary>
        private static ThreadLocal<MD5> _tlMD5= new ThreadLocal<MD5>();

        /// <summary>
        /// 线程变量:SHA1
        /// </summary>
        private static ThreadLocal<SHA1> _tlSHA1 = new ThreadLocal<SHA1>();
        /// <summary>
        /// 是否开启了批量操作
        /// </summary>
        public static bool IsBatch
        {
            get
            {
                if (!_tlIsBatch.IsValueCreated) 
                {
                    return false;
                }
                return _tlIsBatch.Value;
            }
        }

        /// <summary>
        /// 获取MD5类
        /// </summary>
        /// <returns></returns>
        internal static MD5 GetMD5() 
        {
            if (!IsBatch) 
            {
                return MD5.Create();
            }
            MD5 ret = _tlMD5.Value;
            if (ret == null) 
            {
                ret= MD5.Create();
                _tlMD5.Value = ret;
            }
            return ret;
        }
        /// <summary>
        /// 获取MD5类
        /// </summary>
        /// <returns></returns>
        internal static SHA1 GetSHA1()
        {
            if (!IsBatch)
            {
                return SHA1.Create();
            }
            SHA1 ret = _tlSHA1.Value;
            if (ret == null)
            {
                ret = SHA1.Create();
                _tlSHA1.Value = ret;
            }
            return ret;
        }
        /// <summary>
        /// 自动关闭
        /// </summary>
        /// <param name="obj"></param>
        internal static void AutoClose(IDisposable obj) 
        {
            if (!IsBatch)
            {
                obj.Dispose();
            }
        }

        /// <summary>
        /// 是否此类开启的批量操作
        /// </summary>
        private bool _isOpenBatch = false;

        /// <summary>
        /// 开启批量哈希操作
        /// </summary>
        public BatchHash() 
        {
            if (!IsBatch) 
            {
                _tlIsBatch.Value = true;
                _isOpenBatch = true;
            }
        }

        /// <summary>
        /// 清理对象
        /// </summary>
        public void Dispose()
        {
            if (_isOpenBatch) 
            {
                MD5 md5 = _tlMD5.Value;
                if (md5 != null) 
                {
                    md5.Dispose();
                }
                _tlMD5.Value = null;
                SHA1 sha1 = _tlSHA1.Value;
                if (sha1 != null)
                {
                    sha1.Dispose();
                }
                _tlSHA1.Value = null;
            }
        }
    }
}
