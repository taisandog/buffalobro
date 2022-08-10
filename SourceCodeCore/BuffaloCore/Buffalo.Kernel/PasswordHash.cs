using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Threading;

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
        public static string ToMD5String(string str, bool isUpper=true)
        {
            return ToMD5String(DefaultEncoding.GetBytes(str),isUpper);
        }
        /// <summary>
        /// �����ݽ���MD5ɢ��
        /// </summary>
        /// <param name="content">Ҫɢ�е�����</param>
        /// <param name="isUpper">ɢ�н���Ƿ��д</param>
        /// <returns></returns>
        public static string ToMD5String(byte[] content, bool isUpper=true)
        {
            byte[] retBytes = ToMD5Bytes(content);
            return CommonMethods.BytesToHexString(retBytes,isUpper);
        }
        
        /// <summary>
        /// ���ַ�������MD5ɢ��
        /// </summary>
        /// <param name="stm">Ҫɢ�е���</param>
        ///  <param name="isUpper">ɢ�н���Ƿ��д</param>
        /// <returns></returns>
        public static string ToMD5String(Stream stm, bool isUpper = true)
        {
            byte[] retBytes = ToMD5Bytes(stm);
            return CommonMethods.BytesToHexString(retBytes,isUpper);
        }

        /// <summary>
        /// ����������ϣ
        /// </summary>
        /// <returns></returns>
        public static BatchHash StartBatchHash() 
        {
            return new BatchHash();
        }

        /// <summary>
        /// ���ַ�������MD5ɢ��
        /// </summary>
        /// <param name="str">Ҫɢ�е��ַ���</param>
        /// <param name="isUpper">��ĸ�Ƿ��д</param>
        /// <returns></returns>
        public static byte[] ToMD5Bytes(string str)
        {
            return ToMD5Bytes(DefaultEncoding.GetBytes(str));
        }
        /// <summary>
        /// �����ݽ���MD5ɢ��
        /// </summary>
        /// <param name="content">Ҫɢ�е�����</param>
        /// <param name="isUpper">ɢ�н���Ƿ��д</param>
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
        /// ���ַ�������MD5ɢ��
        /// </summary>
        /// <param name="stm">Ҫɢ�е���</param>
        ///  <param name="isUpper">ɢ�н���Ƿ��д</param>
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
        /// ���ַ�������SHA1ɢ��
        /// </summary>
        /// <param name="str">Ҫɢ�е��ַ���</param>
        /// <param name="isUpper">ɢ�н���Ƿ��д</param>
        /// <returns></returns>
        public static string ToSHA1String(string str, bool isUpper = true)
        {
            return ToSHA1String(Encoding.Default.GetBytes(str),isUpper);
        }
        

        /// <summary>
        /// �����ݽ���SHA1ɢ��
        /// </summary>
        /// <param name="content">Ҫɢ�е�����</param>
        /// <param name="isUpper">ɢ�н���Ƿ��д</param>
        /// <returns></returns>
        public static string ToSHA1String(byte[] content, bool isUpper = true)
        {
            
            byte[] retBytes = ToSHA1Bytes(content);
            return CommonMethods.BytesToHexString(retBytes, isUpper);
        }
        
        /// <summary>
        /// �����ݽ���SHA1ɢ��
        /// </summary>
        /// <param name="stm">Ҫɢ�е���</param>
        /// <param name="isUpper">ɢ�н���Ƿ��д</param>
        /// <returns></returns>
        public static string ToSHA1String(Stream stm,bool isUpper = true)
        {
            byte[] retBytes = ToSHA1Bytes(stm);
            return CommonMethods.BytesToHexString(retBytes, isUpper);
        }
        /// <summary>
        /// ���ַ�������SHA1ɢ��
        /// </summary>
        /// <param name="str">Ҫɢ�е��ַ���</param>
        /// <param name="isUpper">ɢ�н���Ƿ��д</param>
        /// <returns></returns>
        public static byte[] ToSHA1Bytes(string str)
        {
            return ToSHA1Bytes(Encoding.Default.GetBytes(str));
        }


        /// <summary>
        /// �����ݽ���SHA1ɢ��
        /// </summary>
        /// <param name="content">Ҫɢ�е�����</param>
        /// <param name="isUpper">ɢ�н���Ƿ��д</param>
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
        /// �����ݽ���SHA1ɢ��
        /// </summary>
        /// <param name="stm">Ҫɢ�е���</param>
        /// <param name="isUpper">ɢ�н���Ƿ��д</param>
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
        /// AES����
        /// </summary>
        /// <param name="toDecrypt">Ҫ���ܵ�����</param>
        /// <param name="pwd">��������(32�ֽ�����)</param>
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
        /// AES����
        /// </summary>
        /// <param name="toEncrypt">Ҫ���ܵ�����</param>
        /// <param name="pwd">����(32�ֽ�����)</param>
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
    /// ������ϣֵ
    /// </summary>
    public class BatchHash :IDisposable
    {
        /// <summary>
        /// �̱߳���:�Ƿ�������
        /// </summary>
        private static ThreadLocal<bool> _tlIsBatch = new ThreadLocal<bool>();

        /// <summary>
        /// �̱߳���:MD5
        /// </summary>
        private static ThreadLocal<MD5> _tlMD5= new ThreadLocal<MD5>();

        /// <summary>
        /// �̱߳���:SHA1
        /// </summary>
        private static ThreadLocal<SHA1> _tlSHA1 = new ThreadLocal<SHA1>();
        /// <summary>
        /// �Ƿ�������������
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
        /// ��ȡMD5��
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
        /// ��ȡMD5��
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
        /// �Զ��ر�
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
        /// �Ƿ���࿪������������
        /// </summary>
        private bool _isOpenBatch = false;

        /// <summary>
        /// ����������ϣ����
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
        /// �������
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
