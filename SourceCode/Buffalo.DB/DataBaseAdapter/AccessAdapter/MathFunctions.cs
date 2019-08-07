using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;

namespace Buffalo.DB.DataBaseAdapter.AccessAdapter
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
            return "Abs(" + values[0] + ")";
        }
        /// <summary>
        /// ����Ceil����
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public virtual string DoCeil(string[] values)
        {
            return "int(2*" + values[0] + "+0.5)/2";
            //return "ceiling(" + values[0] + ")";
        }
        /// <summary>
        /// ����floor����
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public virtual string DoFloor(string[] values)
        {
            return "Int(" + values[0] + ")";
        }
        /// <summary>
        /// ����round����
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public virtual string DoRound(string[] values)
        {
            return "Round(" + values[0] + ")";
        }
        /// <summary>
        /// ����exp����
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public virtual string DoExp(string[] values)
        {
            return "Exp(" + values[0] + ")";
        }
        /// <summary>
        /// ����ln����
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public virtual string DoLn(string[] values)
        {
            return "Log(" + values[0] + ")";
        }
        /// <summary>
        /// ����log����
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public virtual string DoLog10(string[] values)
        {
            return "Log(" + values [0]+ ") / Log(n)";
        }

        /// <summary>
        /// ����SQRT����
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public virtual string DoSqrt(string[] values)
        {
            return "Sqr(" + values[0] + ")";
        }

        /// <summary>
        /// ����power����
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public virtual string DoPower(string[] values)
        {
            return values[0] + "^" + values[1];
        }

        /// <summary>
        /// �����������
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public virtual string DoRandom(string[] values)
        {
            return "Rnd()";
        }

        /// <summary>
        /// ����Sign����
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public virtual string DoSign(string[] values)
        {
            throw new Exception("Access��֧��Sign����");
        }
        /// <summary>
        /// ����Sin����
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public virtual string DoSin(string[] values)
        {
            return "sin(" + values[0] + ")";
        }
        /// <summary>
        /// ����Cos����
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public virtual string DoCos(string[] values)
        {
            return "cos(" + values[0] + ")";
        }
        /// <summary>
        /// ����Tan����
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public virtual string DoTan(string[] values)
        {
            return "Tan(" + values[0] + ")";
        }
        /// <summary>
        /// ����Asin����
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public virtual string DoAsin(string[] values)
        {
            throw new Exception("Access��֧��ASin����");
        }
        /// <summary>
        /// ����Acos����
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public virtual string DoAcos(string[] values)
        {
            throw new Exception("Access��֧��ACos����");
        }
        /// <summary>
        /// ����Atan����
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public virtual string DoAtan(string[] values)
        {
            throw new Exception("Access��֧��ATan����");
        }
        /// <summary>
        /// ����Atan2����
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public virtual string DoAtan2(string[] values)
        {
            throw new Exception("Access��֧��Atan2����");
        }

        /// <summary>
        /// ��������ַ�������
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public virtual string IndexOf(string[] values)
        {
            return " InStr(" + values[0] + "," + values[1] + "," + values[2] + ")";
        }

        /// <summary>
        /// �����ȡ�ַ�������
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public virtual string SubString(string[] values)
        {
            return " Mid(" + values[0] + "," + values[1] + "," + values[2] + ")";
        }

        public virtual string DoMod(string[] values)
        {
            return "(" + values[0] + " Mod " + values[1] + ")";
        }
        public virtual string BitAND(string[] values) 
        {
            return "(" + values[0] + " band " + values[1] + ")";
        }
        public virtual string BitOR(string[] values)
        {
            return "(" + values[0] + " bor " + values[1] + ")";
        }
        public virtual string BitXOR(string[] values)
        {
            return "(" + values[0] + " bxor " + values[1] + ")";
        }
        public virtual string BitNot(string[] values)
        {
            return "(bnot " + values[0] + ")";
        }
    }
}
