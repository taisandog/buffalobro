using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;

namespace Buffalo.Data.SQLite
{
    public class MathFunctions : IMathFunctions
    {
        /// <summary>
        /// ����abs����
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public virtual string DoAbs(string[] values)
        {
            return "abs(" + values[0] + ")";
        }
        /// <summary>
        /// ����Ceil����
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public virtual string DoCeil(string[] values)
        {
            throw new Exception("SQLite��֧��Ceil����");
        }
        /// <summary>
        /// ����floor����
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public virtual string DoFloor(string[] values)
        {
            throw new Exception("SQLite��֧��floor����");
        }
        /// <summary>
        /// ����round����
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public virtual string DoRound(string[] values)
        {
            return "round(" + values[0] + ")";
        }
        /// <summary>
        /// ����exp����
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public virtual string DoExp(string[] values)
        {
            throw new Exception("SQLite��֧��exp����");
        }
        /// <summary>
        /// ����ln����
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public virtual string DoLn(string[] values)
        {
            throw new Exception("SQLite��֧��ln����");
        }
        /// <summary>
        /// ����log����
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public virtual string DoLog10(string[] values)
        {
            throw new Exception("SQLite��֧��log10����");
        }

        /// <summary>
        /// ����SQRT����
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public virtual string DoSqrt(string[] values)
        {
            throw new Exception("SQLite��֧��sqrt����");
        }

        /// <summary>
        /// ����power����
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public virtual string DoPower(string[] values)
        {
            throw new Exception("SQLite��֧��power����");
        }

        /// <summary>
        /// �����������
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public virtual string DoRandom(string[] values)
        {
            return "random()";
        }

        /// <summary>
        /// ����Sign����
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public virtual string DoSign(string[] values)
        {
            throw new Exception("SQLite��֧��sign����");
        }
        /// <summary>
        /// ����Sin����
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public virtual string DoSin(string[] values)
        {
            throw new Exception("SQLite��֧��sin����");
        }
        /// <summary>
        /// ����Cos����
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public virtual string DoCos(string[] values)
        {
            throw new Exception("SQLite��֧��cos����");
        }
        /// <summary>
        /// ����Tan����
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public virtual string DoTan(string[] values)
        {
            throw new Exception("SQLite��֧��tan����");
        }
        /// <summary>
        /// ����Asin����
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public virtual string DoAsin(string[] values)
        {
            throw new Exception("SQLite��֧��asin����");
        }
        /// <summary>
        /// ����Acos����
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public virtual string DoAcos(string[] values)
        {
            throw new Exception("SQLite��֧��acos����");
        }
        /// <summary>
        /// ����Atan����
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public virtual string DoAtan(string[] values)
        {
            throw new Exception("SQLite��֧��atan����");
        }
        /// <summary>
        /// ����Atan2����
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public virtual string DoAtan2(string[] values)
        {
            throw new Exception("SQLite��֧��atan2����");
        }

        /// <summary>
        /// ��������ַ�������
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public virtual string IndexOf(string[] values)
        {
            throw new Exception("SQLite��֧��IndexOf����");
            //return " charIndex(" + values[0] + "," + values[1] + "," + values[2] + ")";
        }

        /// <summary>
        /// �����ȡ�ַ�������
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public virtual string SubString(string[] values)
        {
            return " substr(" + values[0] + "," + values[1] + "," + values[2] + ")";
        }
        public virtual string DoMod(string[] values) 
        {
            return "(" + values[0] + "%" + values[1] + ")";
        }
        public virtual string BitAND(string[] values)
        {
            return "(" + values[0] + " & " + values[1] + ")";
        }
        public virtual string BitOR(string[] values)
        {
            return "(" + values[0] + " | " + values[1] + ")";
        }
        public virtual string BitXOR(string[] values)
        {
            return "(" + values[0] + " ^ " + values[1] + ")";
        }
        public virtual string BitNot(string[] values)
        {
            return "(~" + values[0] + ")";
        }
    }
}
