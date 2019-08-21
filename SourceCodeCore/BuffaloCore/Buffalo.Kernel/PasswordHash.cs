using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;

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
        public static string ToMD5String(string str, bool isUpper)
        {
            return ToMD5String(DefaultEncoding.GetBytes(str),isUpper);
        }
        /// <summary>
        /// 对字符串进行MD5散列
        /// </summary>
        /// <param name="str">要散列的字符串</param>
        /// <returns></returns>
        public static string ToMD5String(string str)
        {
            return ToMD5String(str, true);
        }

        /// <summary>
        /// 对内容进行MD5散列
        /// </summary>
        /// <param name="content">要散列的内容</param>
        /// <param name="isUpper">散列结果是否大写</param>
        /// <returns></returns>
        public static string ToMD5String(byte[] content, bool isUpper)
        {
             MD5 md5Hash = MD5CryptoServiceProvider.Create();
            byte[] retBytes = md5Hash.ComputeHash(content);
            return CommonMethods.BytesToHexString(retBytes,isUpper);
        }
        /// <summary>
        /// 对内容进行MD5散列
        /// </summary>
        /// <param name="content">要散列的内容</param>
        /// <returns></returns>
        public static string ToMD5String(byte[] content)
        {
            return ToMD5String(content, true);
        }
        /// <summary>
        /// 对字符串进行MD5散列
        /// </summary>
        /// <param name="stm">要散列的流</param>
        ///  <param name="isUpper">散列结果是否大写</param>
        /// <returns></returns>
        public static string ToMD5String(Stream stm, bool isUpper)
        {
            MD5 md5Hash = MD5CryptoServiceProvider.Create();
            byte[] retBytes = md5Hash.ComputeHash(stm);
            return CommonMethods.BytesToHexString(retBytes,isUpper);
        }
        /// <summary>
        /// 对字符串进行MD5散列
        /// </summary>
        /// <param name="stm">要散列的流</param>
        /// <returns></returns>
        public static string ToMD5String(Stream stm)
        {
            return ToMD5String(stm, true);
        }

        /// <summary>
        /// 对字符串进行SHA1散列
        /// </summary>
        /// <param name="str">要散列的字符串</param>
        /// <param name="isUpper">散列结果是否大写</param>
        /// <returns></returns>
        public static string ToSHA1String(string str, bool isUpper)
        {
            return ToSHA1String(Encoding.Default.GetBytes(str),isUpper);
        }
        /// <summary>
        /// 对字符串进行SHA1散列
        /// </summary>
        /// <param name="str">要散列的字符串</param>
        /// <returns></returns>
        public static string ToSHA1String(string str)
        {
            return ToSHA1String(str, true);
        }

        /// <summary>
        /// 对内容进行SHA1散列
        /// </summary>
        /// <param name="content">要散列的内容</param>
        /// <param name="isUpper">散列结果是否大写</param>
        /// <returns></returns>
        public static string ToSHA1String(byte[] content, bool isUpper)
        {
            SHA1 sha1Hash = SHA1CryptoServiceProvider.Create();
            byte[] retBytes = sha1Hash.ComputeHash(content);
            return CommonMethods.BytesToHexString(retBytes, isUpper);
        }
        /// <summary>
        /// 对内容进行SHA1散列
        /// </summary>
        /// <param name="content">要散列的内容</param>
        /// <returns></returns>
        public static string ToSHA1String(byte[] content)
        {
            return ToSHA1String(content, true);
        }
        /// <summary>
        /// 对内容进行SHA1散列
        /// </summary>
        /// <param name="stm">要散列的流</param>
        /// <param name="isUpper">散列结果是否大写</param>
        /// <returns></returns>
        public static string ToSHA1String(Stream stm,bool isUpper)
        {
            SHA1 sha1Hash = SHA1CryptoServiceProvider.Create();
            byte[] retBytes = sha1Hash.ComputeHash(stm);
            return CommonMethods.BytesToHexString(retBytes, isUpper);
        }
        /// <summary>
        /// 对内容进行SHA1散列
        /// </summary>
        /// <param name="stm">要散列的流</param>
        /// <param name="isUpper">散列结果是否大写</param>
        /// <returns></returns>
        public static string ToSHA1String(Stream stm)
        {
            return ToSHA1String(stm, true);
        }
        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="toDecrypt">要解密的数据</param>
        /// <param name="pwd">解密密码(32字节数组)</param>
        /// <returns></returns>
        public static byte[] AESDecrypt(byte[] toDecrypt,byte[] pwd)
        {
            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = pwd;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = rDel.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toDecrypt, 0, toDecrypt.Length);

            return resultArray;
        }

        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="toEncrypt">要加密的数据</param>
        /// <param name="pwd">密码(32字节数组)</param>
        /// <returns></returns>
        public static byte[] AESEncrypt(byte[] toEncrypt, byte[] pwd)
        {
            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = pwd;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = rDel.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncrypt, 0, toEncrypt.Length);

            return resultArray;
        }
    }
}
