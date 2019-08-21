using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;

namespace Buffalo.DB.DataBaseAdapter.SqlServer2KAdapter
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
            return "ceiling(" + values[0] + ")";
        }
        /// <summary>
        /// ����floor����
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public virtual string DoFloor(string[] values)
        {
            return "floor(" + values[0] + ")";
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
            return "exp(" + values[0] + ")";
        }
        /// <summary>
        /// ����ln����
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public virtual string DoLn(string[] values)
        {
            return "log(" + values[0] + ")";
        }
        /// <summary>
        /// ����log����
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public virtual string DoLog10(string[] values)
        {
            return "log10(" + values[0] + ")";
        }

        /// <summary>
        /// ����SQRT����
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public virtual string DoSqrt(string[] values)
        {
            return "sqrt(" + values[0] + ")";
        }

        /// <summary>
        /// ����power����
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public virtual string DoPower(string[] values)
        {
            return "power(" + values[0] + "," + values[1] + ")";
        }

        /// <summary>
        /// �����������
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public virtual string DoRandom(string[] values)
        {
            return "rand()";
        }

        /// <summary>
        /// ����Sign����
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public virtual string DoSign(string[] values)
        {
            return " sign("+values[0]+")";
        }
        /// <summary>
        /// ����Sin����
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public virtual string DoSin(string[] values)
        {
            return " sin(" + values[0] + ")";
        }
        /// <summary>
        /// ����Cos����
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public virtual string DoCos(string[] values)
        {
            return " cos(" + values[0] + ")";
        }
        /// <summary>
        /// ����Tan����
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public virtual string DoTan(string[] values)
        {
            return " tan(" + values[0] + ")";
        }
        /// <summary>
        /// ����Asin����
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public virtual string DoAsin(string[] values)
        {
            return " asin(" + values[0] + ")";
        }
        /// <summary>
        /// ����Acos����
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public virtual string DoAcos(string[] values)
        {
            return " acos(" + values[0] + ")";
        }
        /// <summary>
        /// ����Atan����
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public virtual string DoAtan(string[] values)
        {
            return " atan(" + values[0] + ")";
        }
        /// <summary>
        /// ����Atan2����
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public virtual string DoAtan2(string[] values)
        {
            return " atn2(" + values[0] + ")";
        }

        /// <summary>
        /// ��������ַ�������
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public virtual string IndexOf(string[] values)
        {
            return " charIndex(" + values[0] + ","+values[1]+","+values[2]+")";
        }

        /// <summary>
        /// �����ȡ�ַ�������
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public virtual string SubString(string[] values)
        {
            return " substring(" + values[0] + "," + values[1] + "," + values[2] + ")";
        }
        public virtual string DoMod(string[] values)
        {
            return "(" + values[0] + " % " + values[1] + ")";
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
