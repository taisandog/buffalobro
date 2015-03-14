using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace Buffalo.Kernel
{
    public class PasswordHash
    {
        /// <summary>
        /// 对字符串进行MD5加密
        /// </summary>
        /// <param name="str">要加密的字符串</param>
        /// <returns></returns>
        public static string ToMD5String(string str)
        {

            return ToMD5String(Encoding.Default.GetBytes(str));
        }
        private static MD5 md5Hash = MD5CryptoServiceProvider.Create();
        /// <summary>
        /// 对字符串进行MD5加密
        /// </summary>
        /// <param name="str">要加密的字符串</param>
        /// <returns></returns>
        public static string ToMD5String(byte[] content)
        {

            byte[] retBytes = md5Hash.ComputeHash(content);
            return CommonMethods.BytesToHexString(retBytes);
        }

        /// <summary>
        /// 对字符串进行MD5加密
        /// </summary>
        /// <param name="str">要加密的字符串</param>
        /// <returns></returns>
        public static string ToSHA1String(string str)
        {
            return ToSHA1String(Encoding.Default.GetBytes(str));
        }
        private static SHA1 sha1Hash = SHA1CryptoServiceProvider.Create();
        /// <summary>
        /// 对字符串进行MD5加密
        /// </summary>
        /// <param name="str">要加密的字符串</param>
        /// <returns></returns>
        public static string ToSHA1String(byte[] content)
        {

            byte[] retBytes = sha1Hash.ComputeHash(content);
            return CommonMethods.BytesToHexString(retBytes);
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
