using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;

namespace Buffalo.DB.DataBaseAdapter.SqlServer2KAdapter
{
    public class MathFunctions : IMathFunctions
    {
        /// <summary>
        /// 处理abs函数
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public virtual string DoAbs(string[] values)
        {
            return "abs(" + values[0] + ")";
        }
        /// <summary>
        /// 处理Ceil函数
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public virtual string DoCeil(string[] values)
        {
            return "ceiling(" + values[0] + ")";
        }
        /// <summary>
        /// 处理floor函数
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public virtual string DoFloor(string[] values)
        {
            return "floor(" + values[0] + ")";
        }
        /// <summary>
        /// 处理round函数
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public virtual string DoRound(string[] values)
        {
            return "round(" + values[0] + ")";
        }
        /// <summary>
        /// 处理exp函数
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public virtual string DoExp(string[] values)
        {
            return "exp(" + values[0] + ")";
        }
        /// <summary>
        /// 处理ln函数
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public virtual string DoLn(string[] values)
        {
            return "log(" + values[0] + ")";
        }
        /// <summary>
        /// 处理log函数
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public virtual string DoLog10(string[] values)
        {
            return "log10(" + values[0] + ")";
        }

        /// <summary>
        /// 处理SQRT函数
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public virtual string DoSqrt(string[] values)
        {
            return "sqrt(" + values[0] + ")";
        }

        /// <summary>
        /// 处理power函数
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public virtual string DoPower(string[] values)
        {
            return "power(" + values[0] + "," + values[1] + ")";
        }

        /// <summary>
        /// 处理随机函数
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public virtual string DoRandom(string[] values)
        {
            return "rand()";
        }

        /// <summary>
        /// 处理Sign函数
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public virtual string DoSign(string[] values)
        {
            return " sign("+values[0]+")";
        }
        /// <summary>
        /// 处理Sin函数
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public virtual string DoSin(string[] values)
        {
            return " sin(" + values[0] + ")";
        }
        /// <summary>
        /// 处理Cos函数
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public virtual string DoCos(string[] values)
        {
            return " cos(" + values[0] + ")";
        }
        /// <summary>
        /// 处理Tan函数
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public virtual string DoTan(string[] values)
        {
            return " tan(" + values[0] + ")";
        }
        /// <summary>
        /// 处理Asin函数
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public virtual string DoAsin(string[] values)
        {
            return " asin(" + values[0] + ")";
        }
        /// <summary>
        /// 处理Acos函数
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public virtual string DoAcos(string[] values)
        {
            return " acos(" + values[0] + ")";
        }
        /// <summary>
        /// 处理Atan函数
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public virtual string DoAtan(string[] values)
        {
            return " atan(" + values[0] + ")";
        }
        /// <summary>
        /// 处理Atan2函数
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public virtual string DoAtan2(string[] values)
        {
            return " atn2(" + values[0] + ")";
        }

        /// <summary>
        /// 处理查找字符串函数
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public virtual string IndexOf(string[] values)
        {
            return " charIndex(" + values[0] + ","+values[1]+","+values[2]+")";
        }

        /// <summary>
        /// 处理截取字符串函数
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
