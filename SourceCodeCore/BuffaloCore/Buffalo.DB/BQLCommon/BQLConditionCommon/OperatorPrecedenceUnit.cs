using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.DB.BQLCommon.BQLConditionCommon
{
    /// <summary>
    /// ��������ȼ�������
    /// </summary>
    public class OperatorPrecedenceUnit
    {
        /// <summary>
        /// ���ֵ������
        /// </summary>
        /// <param name="left">���ӷ����</param>
        /// <param name="isLeft">�Ƿ�����������</param>
        /// <param name="operLevel">��ǰ�������ȼ�</param>
        /// <param name="info">�����Ϣ</param>
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
        /// ���ȼ�����
        /// </summary>
        private static int[] _arrPrecedence = InitPrecedence();
        /// <summary>
        /// ��ʼ�����ȼ�����
        /// </summary>
        /// <returns></returns>
        private static int[] InitPrecedence() 
        {
            int[] arr = new int[256];
            arr[(int)'+'] = 8;
            arr[(int)'-'] = 8;
            arr[(int)'*'] = 9;
            arr[(int)'/'] = 9;
            arr[(int)'%'] = 9;//ȡģ��ʶ��Ϊ�����������κ����ȼ�����
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
        /// ��ȡ��������ȼ�
        /// </summary>
        /// <param name="oper">�����</param>
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
