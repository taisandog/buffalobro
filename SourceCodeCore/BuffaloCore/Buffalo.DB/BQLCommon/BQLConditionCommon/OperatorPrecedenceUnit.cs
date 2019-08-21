using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.DB.BQLCommon.BQLConditionCommon
{
    /// <summary>
    /// 运算符优先级处理类
    /// </summary>
    public class OperatorPrecedenceUnit
    {
        /// <summary>
        /// 填充值的括号
        /// </summary>
        /// <param name="left">连接符左边</param>
        /// <param name="isLeft">是否在运算符左边</param>
        /// <param name="operLevel">当前符号优先级</param>
        /// <param name="info">输出信息</param>
        /// <returns></returns>
        public static string FillBreak(BQLValueItem value, bool isLeft, int operLevel, KeyWordInfomation info)
        {
            IOperatorPriorityLevel pl = value as IOperatorPriorityLevel;

            if (pl == null)
            {
                return value.DisplayValue(info);
            }

            if ((!info.DBInfo.OperatorPrecedence) || (isLeft && pl.PriorityLevel < operLevel)||
                ((!isLeft) && pl.PriorityLevel <= operLevel)
                )
            {
                return "(" + value.DisplayValue(info) + ")";
            }
            return value.DisplayValue(info);
        }

        

        /// <summary>
        /// 优先级数组
        /// </summary>
        private static int[] _arrPrecedence = InitPrecedence();
        /// <summary>
        /// 初始化优先级数组
        /// </summary>
        /// <returns></returns>
        private static int[] InitPrecedence() 
        {
            int[] arr = new int[256];
            arr[(int)'+'] = 8;
            arr[(int)'-'] = 8;
            arr[(int)'*'] = 9;
            arr[(int)'/'] = 9;
            arr[(int)'%'] = 9;//取模被识别为函数，不做任何优先级处理
            arr[(int)'='] = 4;
            arr[(int)'!'+128] = 4;//!=
            arr[(int)'&'] = 2;
            arr[(int)'|'] = 1;
            arr[(int)'>'] = 4;
            arr[(int)'>'+128] = 4;//>=
            arr[(int)'<'] = 4;
            arr[(int)'<'+128] = 4;//<=
            return arr;
        }

        /// <summary>
        /// 获取运算符优先级
        /// </summary>
        /// <param name="oper">运算符</param>
        /// <returns></returns>
        public static int GetPrecedence(string oper) 
        {
            int index = 0;
            if(string.IsNullOrEmpty(oper))
            {
                return 0;
            }
            if (oper.Length > 1)
            {
                index += 128;
            }

            index += (int)oper[0];
            return _arrPrecedence[index];
            
        }

    }
}
