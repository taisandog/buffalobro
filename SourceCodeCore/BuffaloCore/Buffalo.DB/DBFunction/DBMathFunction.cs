using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.DataBaseAdapter;

namespace Buffalo.DB.DBFunction
{

    /// <summary>
    /// ��ѧ�����ĵ���
    /// </summary>
    public class DBMathFunction
    {
        /// <summary>
        /// ����ֵ
        /// </summary>
        /// <param name="values"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static string DoAbs(string[] values,DBInfo info) 
        {
            return info.Math.DoAbs(values);
        }
        /// <summary>
        /// ������ֵ
        /// </summary>
        /// <param name="values"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static string DoAcos(string[] values,DBInfo info) 
        {
            return info.Math.DoAcos(values);
        }
        /// <summary>
        /// ������ֵ
        /// </summary>
        /// <param name="values"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static string DoAsin(string[] values,DBInfo info) 
        {
            return info.Math.DoAsin(values);
        }
        /// <summary>
        /// ������ֵ
        /// </summary>
        /// <param name="values"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static string DoAtan(string[] values,DBInfo info) 
        {
            return info.Math.DoAtan(values);
        }
        /// <summary>
        /// ������ֵ2
        /// </summary>
        /// <param name="values"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static string DoAtan2(string[] values,DBInfo info) 
        {
            return info.Math.DoAtan2(values);
        }
        /// <summary>
        /// ���ز�С��x ����һ������
        /// </summary>
        /// <param name="values"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static string DoCeil(string[] values,DBInfo info) 
        {
            return info.Math.DoCeil(values);
        }
        /// <summary>
        /// ����
        /// </summary>
        /// <param name="values"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static string DoCos(string[] values,DBInfo info) 
        {
            return info.Math.DoCos(values);
        }
        /// <summary>
        /// ָ����������Ȼ����eΪ�׵�ָ������
        /// </summary>
        /// <param name="values"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static string DoExp(string[] values,DBInfo info) 
        {
            return info.Math.DoExp(values);
        }
        /// <summary>
        /// ���رȲ���С�����������
        /// </summary>
        /// <param name="values"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static string DoFloor(string[] values,DBInfo info) 
        {
            return info.Math.DoFloor(values);
        }
        /// <summary>
        /// ����ָ�����ֵ���Ȼ��������Ϊ e����
        /// </summary>
        /// <param name="values"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static string DoLn(string[] values,DBInfo info)
        {
            return info.Math.DoLn(values);
        }
        /// <summary>
        ///  ����ָ�������� 10 Ϊ�׵Ķ�����
        /// </summary>
        /// <param name="values"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static string DoLog10(string[] values,DBInfo info)
        {
            return info.Math.DoLog10(values);
        }
        /// <summary>
        /// ����ָ�����ֵ�ָ�����ݡ�
        /// </summary>
        /// <param name="values"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static string DoPower(string[] values,DBInfo info)
        {
            return info.Math.DoPower(values);
        }
        /// <summary>
        /// ���������
        /// </summary>
        /// <param name="values"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static string DoRandom(string[] values,DBInfo info)
        {
            return info.Math.DoRandom(values);
        }
        /// <summary>
        /// ��С��ֵ���뵽��ӽ���������
        /// </summary>
        /// <param name="values"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static string DoRound(string[] values,DBInfo info)
        {
            return info.Math.DoRound(values);
        }
        /// <summary>
        /// ���ر�ʾ���ַ��ŵ�ֵ��
        /// </summary>
        /// <param name="values"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static string DoSign(string[] values,DBInfo info)
        {
            return info.Math.DoSign(values);
        }
        /// <summary>
        /// ���Һ���
        /// </summary>
        /// <param name="values"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static string DoSin(string[] values,DBInfo info)
        {
            return info.Math.DoSin(values);
        }
        /// <summary>
        /// ����ָ�����ֵ�ƽ������
        /// </summary>
        /// <param name="values"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static string DoSqrt(string[] values,DBInfo info)
        {
            return info.Math.DoSqrt(values);
        }
        /// <summary>
        /// ���к���
        /// </summary>
        /// <param name="values"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static string DoTan(string[] values,DBInfo info)
        {
            return info.Math.DoTan(values);
        }
        /// <summary>
        /// ����ָ�� Unicode �ַ��ڴ��ַ����еĵ�һ��ƥ�����������
        /// </summary>
        /// <param name="values"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static string IndexOf(string[] values,DBInfo info)
        {
            return info.Math.IndexOf(values);
        }
        /// <summary>
        /// ��ģ����
        /// </summary>
        /// <param name="values"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static string DoMod(string[] values, DBInfo info)
        {
            return info.Math.DoMod(values);
        }
        /// <summary>
        /// ��λ������
        /// </summary>
        /// <param name="values"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static string BitAND(string[] values, DBInfo info)
        {
            return info.Math.BitAND(values);
        }
        /// <summary>
        /// ������
        /// </summary>
        /// <param name="values"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static string BitNot(string[] values, DBInfo info)
        {
            return info.Math.BitNot(values);
        }
        /// <summary>
        /// ��λ������
        /// </summary>
        /// <param name="values"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static string BitOR(string[] values, DBInfo info)
        {
            return info.Math.BitOR(values);
        }
        /// <summary>
        /// ��λ�������
        /// </summary>
        /// <param name="values"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static string BitXOR(string[] values, DBInfo info)
        {
            return info.Math.BitXOR(values);
        }
        /// <summary>
        /// ��ȡ�ַ���
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
