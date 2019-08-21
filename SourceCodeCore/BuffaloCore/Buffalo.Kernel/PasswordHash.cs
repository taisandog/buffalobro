using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace Buffalo.Kernel
{
    /// <summary>
    /// ɢ�и�����
    /// </summary>
    public class PasswordHash
    {
        private static Encoding DefaultEncoding = Encoding.UTF8;
        /// <summary>
        /// ���ַ�������MD5ɢ��
        /// </summary>
        /// <param name="str">Ҫɢ�е��ַ���</param>
        /// <param name="isUpper">��ĸ�Ƿ��д</param>
        /// <returns></returns>
        public static string ToMD5String(string str, bool isUpper)
        {
            return ToMD5String(DefaultEncoding.GetBytes(str),isUpper);
        }
        /// <summary>
        /// ���ַ�������MD5ɢ��
        /// </summary>
        /// <param name="str">Ҫɢ�е��ַ���</param>
        /// <returns></returns>
        public static string ToMD5String(string str)
        {
            return ToMD5String(str, true);
        }

        /// <summary>
        /// �����ݽ���MD5ɢ��
        /// </summary>
        /// <param name="content">Ҫɢ�е�����</param>
        /// <param name="isUpper">ɢ�н���Ƿ��д</param>
        /// <returns></returns>
        public static string ToMD5String(byte[] content, bool isUpper)
        {
             MD5 md5Hash = MD5CryptoServiceProvider.Create();
            byte[] retBytes = md5Hash.ComputeHash(content);
            return CommonMethods.BytesToHexString(retBytes,isUpper);
        }
        /// <summary>
        /// �����ݽ���MD5ɢ��
        /// </summary>
        /// <param name="content">Ҫɢ�е�����</param>
        /// <returns></returns>
        public static string ToMD5String(byte[] content)
        {
            return ToMD5String(content, true);
        }
        /// <summary>
        /// ���ַ�������MD5ɢ��
        /// </summary>
        /// <param name="stm">Ҫɢ�е���</param>
        ///  <param name="isUpper">ɢ�н���Ƿ��д</param>
        /// <returns></returns>
        public static string ToMD5String(Stream stm, bool isUpper)
        {
            MD5 md5Hash = MD5CryptoServiceProvider.Create();
            byte[] retBytes = md5Hash.ComputeHash(stm);
            return CommonMethods.BytesToHexString(retBytes,isUpper);
        }
        /// <summary>
        /// ���ַ�������MD5ɢ��
        /// </summary>
        /// <param name="stm">Ҫɢ�е���</param>
        /// <returns></returns>
        public static string ToMD5String(Stream stm)
        {
            return ToMD5String(stm, true);
        }

        /// <summary>
        /// ���ַ�������SHA1ɢ��
        /// </summary>
        /// <param name="str">Ҫɢ�е��ַ���</param>
        /// <param name="isUpper">ɢ�н���Ƿ��д</param>
        /// <returns></returns>
        public static string ToSHA1String(string str, bool isUpper)
        {
            return ToSHA1String(Encoding.Default.GetBytes(str),isUpper);
        }
        /// <summary>
        /// ���ַ�������SHA1ɢ��
        /// </summary>
        /// <param name="str">Ҫɢ�е��ַ���</param>
        /// <returns></returns>
        public static string ToSHA1String(string str)
        {
            return ToSHA1String(str, true);
        }

        /// <summary>
        /// �����ݽ���SHA1ɢ��
        /// </summary>
        /// <param name="content">Ҫɢ�е�����</param>
        /// <param name="isUpper">ɢ�н���Ƿ��д</param>
        /// <returns></returns>
        public static string ToSHA1String(byte[] content, bool isUpper)
        {
            SHA1 sha1Hash = SHA1CryptoServiceProvider.Create();
            byte[] retBytes = sha1Hash.ComputeHash(content);
            return CommonMethods.BytesToHexString(retBytes, isUpper);
        }
        /// <summary>
        /// �����ݽ���SHA1ɢ��
        /// </summary>
        /// <param name="content">Ҫɢ�е�����</param>
        /// <returns></returns>
        public static string ToSHA1String(byte[] content)
        {
            return ToSHA1String(content, true);
        }
        /// <summary>
        /// �����ݽ���SHA1ɢ��
        /// </summary>
        /// <param name="stm">Ҫɢ�е���</param>
        /// <param name="isUpper">ɢ�н���Ƿ��д</param>
        /// <returns></returns>
        public static string ToSHA1String(Stream stm,bool isUpper)
        {
            SHA1 sha1Hash = SHA1CryptoServiceProvider.Create();
            byte[] retBytes = sha1Hash.ComputeHash(stm);
            return CommonMethods.BytesToHexString(retBytes, isUpper);
        }
        /// <summary>
        /// �����ݽ���SHA1ɢ��
        /// </summary>
        /// <param name="stm">Ҫɢ�е���</param>
        /// <param name="isUpper">ɢ�н���Ƿ��д</param>
        /// <returns></returns>
        public static string ToSHA1String(Stream stm)
        {
            return ToSHA1String(stm, true);
        }
        /// <summary>
        /// AES����
        /// </summary>
        /// <param name="toDecrypt">Ҫ���ܵ�����</param>
        /// <param name="pwd">��������(32�ֽ�����)</param>
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
        /// AES����
        /// </summary>
        /// <param name="toEncrypt">Ҫ���ܵ�����</param>
        /// <param name="pwd">����(32�ֽ�����)</param>
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
