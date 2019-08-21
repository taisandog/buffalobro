using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.DataBaseAdapter;

namespace Buffalo.DB.DBFunction
{

    /// <summary>
    /// 数学函数的调用
    /// </summary>
    public class DBMathFunction
    {
        /// <summary>
        /// 绝对值
        /// </summary>
        /// <param name="values"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static string DoAbs(string[] values,DBInfo info) 
        {
            return info.Math.DoAbs(values);
        }
        /// <summary>
        /// 反余弦值
        /// </summary>
        /// <param name="values"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static string DoAcos(string[] values,DBInfo info) 
        {
            return info.Math.DoAcos(values);
        }
        /// <summary>
        /// 反正弦值
        /// </summary>
        /// <param name="values"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static string DoAsin(string[] values,DBInfo info) 
        {
            return info.Math.DoAsin(values);
        }
        /// <summary>
        /// 反正切值
        /// </summary>
        /// <param name="values"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static string DoAtan(string[] values,DBInfo info) 
        {
            return info.Math.DoAtan(values);
        }
        /// <summary>
        /// 反正切值2
        /// </summary>
        /// <param name="values"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static string DoAtan2(string[] values,DBInfo info) 
        {
            return info.Math.DoAtan2(values);
        }
        /// <summary>
        /// 返回不小于x 的下一个整数
        /// </summary>
        /// <param name="values"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static string DoCeil(string[] values,DBInfo info) 
        {
            return info.Math.DoCeil(values);
        }
        /// <summary>
        /// 余弦
        /// </summary>
        /// <param name="values"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static string DoCos(string[] values,DBInfo info) 
        {
            return info.Math.DoCos(values);
        }
        /// <summary>
        /// 指数函数，自然常数e为底的指数函数
        /// </summary>
        /// <param name="values"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static string DoExp(string[] values,DBInfo info) 
        {
            return info.Math.DoExp(values);
        }
        /// <summary>
        /// 返回比参数小的最大整数用
        /// </summary>
        /// <param name="values"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static string DoFloor(string[] values,DBInfo info) 
        {
            return info.Math.DoFloor(values);
        }
        /// <summary>
        /// 返回指定数字的自然对数（底为 e）。
        /// </summary>
        /// <param name="values"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static string DoLn(string[] values,DBInfo info)
        {
            return info.Math.DoLn(values);
        }
        /// <summary>
        ///  返回指定数字以 10 为底的对数。
        /// </summary>
        /// <param name="values"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static string DoLog10(string[] values,DBInfo info)
        {
            return info.Math.DoLog10(values);
        }
        /// <summary>
        /// 返回指定数字的指定次幂。
        /// </summary>
        /// <param name="values"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static string DoPower(string[] values,DBInfo info)
        {
            return info.Math.DoPower(values);
        }
        /// <summary>
        /// 返回随机数
        /// </summary>
        /// <param name="values"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static string DoRandom(string[] values,DBInfo info)
        {
            return info.Math.DoRandom(values);
        }
        /// <summary>
        /// 将小数值舍入到最接近的整数。
        /// </summary>
        /// <param name="values"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static string DoRound(string[] values,DBInfo info)
        {
            return info.Math.DoRound(values);
        }
        /// <summary>
        /// 返回表示数字符号的值。
        /// </summary>
        /// <param name="values"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static string DoSign(string[] values,DBInfo info)
        {
            return info.Math.DoSign(values);
        }
        /// <summary>
        /// 正弦函数
        /// </summary>
        /// <param name="values"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static string DoSin(string[] values,DBInfo info)
        {
            return info.Math.DoSin(values);
        }
        /// <summary>
        /// 返回指定数字的平方根。
        /// </summary>
        /// <param name="values"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static string DoSqrt(string[] values,DBInfo info)
        {
            return info.Math.DoSqrt(values);
        }
        /// <summary>
        /// 正切函数
        /// </summary>
        /// <param name="values"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static string DoTan(string[] values,DBInfo info)
        {
            return info.Math.DoTan(values);
        }
        /// <summary>
        /// 报告指定 Unicode 字符在此字符串中的第一个匹配项的索引。
        /// </summary>
        /// <param name="values"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static string IndexOf(string[] values,DBInfo info)
        {
            return info.Math.IndexOf(values);
        }
        /// <summary>
        /// 求模运算
        /// </summary>
        /// <param name="values"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static string DoMod(string[] values, DBInfo info)
        {
            return info.Math.DoMod(values);
        }
        /// <summary>
        /// 按位与运算
        /// </summary>
        /// <param name="values"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static string BitAND(string[] values, DBInfo info)
        {
            return info.Math.BitAND(values);
        }
        /// <summary>
        /// 非运算
        /// </summary>
        /// <param name="values"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static string BitNot(string[] values, DBInfo info)
        {
            return info.Math.BitNot(values);
        }
        /// <summary>
        /// 按位或运算
        /// </summary>
        /// <param name="values"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static string BitOR(string[] values, DBInfo info)
        {
            return info.Math.BitOR(values);
        }
        /// <summary>
        /// 按位异或运算
        /// </summary>
        /// <param name="values"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static string BitXOR(string[] values, DBInfo info)
        {
            return info.Math.BitXOR(values);
        }
        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="values"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static string SubString(string[] values,DBInfo info)
        {
            return info.Math.SubString(values);
        }
        
    }
}
