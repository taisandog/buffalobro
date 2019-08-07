using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.Kernel.Defaults;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;
using Buffalo.DB.QueryConditions;
using Buffalo.DB.CommBase.DataAccessBases;
using System.Data;
using Buffalo.Kernel;
using Buffalo.DB.BQLCommon.BQLKeyWordCommon;
using System.Collections;

namespace Buffalo.DB.BQLCommon.BQLConditionCommon
{
    public delegate string DelFunctionHandle(BQLComparItem handle, KeyWordInfomation info);
    public delegate string DelOperatorHandle(BQLOperatorHandle handle, KeyWordInfomation info);
    public class FunctionManager
    {
        #region ��������
        
        
        /// <summary>
        /// ���������
        /// </summary>
        /// <param name="fHandle">������Ϣ</param>
        /// <param name="connect">���ӷ���</param>
        /// <returns></returns>
        private static string OperatorFunction(BQLOperatorHandle fHandle, string connect, KeyWordInfomation info)
        {
            fHandle.ValueDbType = DbType.Double;
            return CustomerConnectFunction(fHandle.GetParameters(), connect, info, fHandle.PriorityLevel);
        }
        /// <summary>
        /// ���������
        /// </summary>
        /// <param name="fHandle">������Ϣ</param>
        /// <param name="connect">���ӷ���</param>
        /// <returns></returns>
        private static string ConditionsFunction(BQLComparItem fHandle, string connect, KeyWordInfomation info)
        {
            fHandle.ValueDbType = DbType.Boolean;
            return CustomerConnectFunction(fHandle.GetParameters(), connect, info, fHandle.PriorityLevel);
        }
        /// <summary>
        /// ���мӷ�����
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        internal static string DoAdd(BQLOperatorHandle fHandle, KeyWordInfomation info)
        {
            BQLValueItem[] parameters=fHandle.GetParameters();
            BQLValueItem item1 = parameters[0];
            BQLValueItem item2 = parameters[1];

            if (IsStringDBType(item1.ValueDbType) || IsStringDBType(item2.ValueDbType)) //�ַ���ƴ��
            {

                string value1 = item1.DisplayValue(info);
                string value2 = item2.DisplayValue(info);
                //if(OperatorPrecedenceUnit.LeftNeedBreak(
                fHandle.ValueDbType = DbType.String;
                return info.DBInfo.CurrentDbAdapter.ConcatString(value1, value2);
            }
            else 
            {
                fHandle.ValueDbType = DbType.Double;
            }
            return CustomerConnectFunction(fHandle.GetParameters(), "+", info, fHandle.PriorityLevel);
        }

        /// <summary>
        /// �ж��Ƿ��ַ�������
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsStringDBType(DbType type) 
        {
            if (type == DbType.AnsiString || type == DbType.AnsiStringFixedLength
                || type == DbType.String || type == DbType.StringFixedLength )
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Not����
        /// </summary>
        /// <param name="fun"></param>
        /// <returns></returns>
        internal static string DoNot(BQLComparItem handle, KeyWordInfomation info) 
        {
            IList<BQLValueItem> items = handle.GetParameters();
            if (items != null && items.Count > 0) 
            {
                return " not (" + items[0].DisplayValue(info)+")";
            }
            return "";
        }

        /// <summary>
        /// ���мӷ�����
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        internal static string DoSub(BQLOperatorHandle handle, KeyWordInfomation info)
        {
            return OperatorFunction(handle, "-", info);
        }
        /// <summary>
        /// ���г˷�����
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        internal static string DoMul(BQLOperatorHandle handle, KeyWordInfomation info)
        {
            return OperatorFunction(handle, "*", info);
        }
        /// <summary>
        /// ���г�������
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        internal static string DoDiv(BQLOperatorHandle handle, KeyWordInfomation info)
        {
            return OperatorFunction(handle, "/", info);
        }

        /// <summary>
        /// ���г�������
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        internal static string DoMod(BQLOperatorHandle handle, KeyWordInfomation info)
        {
            BQLValueItem[] parameters = handle.GetParameters();
            BQLValueItem item1 = parameters[0];
            BQLValueItem item2 = parameters[1];
            string value1 = item1.DisplayValue(info);
            string value2 = item2.DisplayValue(info);
            return info.DBInfo.Math.DoMod(new string[] { value1, value2 });
        }
        /// <summary>
        /// ���е�������
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        internal static string DoEqual(BQLComparItem handle, KeyWordInfomation info)
        {
            IList<BQLValueItem> parameters = handle.GetParameters();
            if (parameters[1].IsNullValue())
            {
                BQLValueItem item = parameters[0];
                return item.DisplayValue(info) + " is null";
            }
            return ConditionsFunction(handle, "=",info);
        }
        /// <summary>
        /// ������
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        internal static string DoNotequal(BQLComparItem handle, KeyWordInfomation info)
        {
            IList<BQLValueItem> parameters = handle.GetParameters();
            if (parameters[1].IsNullValue())
            {
                BQLValueItem item = parameters[0];
                return item.DisplayValue(info) + " is not null";
            }
            return ConditionsFunction(handle, "<>",info);
        }
        /// <summary>
        /// and ����
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        internal static string DoAnd(BQLComparItem handle, KeyWordInfomation info)
        {
            return ConditionsFunction(handle, " and ",info);
        }
        /// <summary>
        /// or ����
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        internal static string DoOr(BQLComparItem handle, KeyWordInfomation info)
        {
            return ConditionsFunction(handle, " or ",info);
        }
        /// <summary>
        /// ����
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        internal static string DoMore(BQLComparItem handle, KeyWordInfomation info)
        {
            return ConditionsFunction(handle, ">",info);
        }
        /// <summary>
        /// ���ڵ���
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        internal static string DoMorethen(BQLComparItem handle, KeyWordInfomation info)
        {
            return ConditionsFunction(handle, ">=",info);
        }
        /// <summary>
        /// С��
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        internal static string DoLess(BQLComparItem handle, KeyWordInfomation info)
        {
            return ConditionsFunction(handle, "<",info);
        }
        /// <summary>
        /// С�ڵ���
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        internal static string DoLessThen(BQLComparItem handle, KeyWordInfomation info)
        {
            return ConditionsFunction(handle, "<=",info);
        }
        /// <summary>
        /// ��ͨ���Ӻ���
        /// </summary>
        /// <param name="handle">����</param>
        /// <param name="connect">���ӷ�</param>
        /// <param name="operLevel">���ȼ�</param>
        /// <returns></returns>
        private static string CustomerConnectFunction(IList<BQLValueItem> parameters, string connect, KeyWordInfomation info, int operLevel)
        {
            //BQLValueItem[] parameters = fHandle.GetParameters();
            string values = "";
            for (int i = 0; i < parameters.Count; i++)
            {
                BQLValueItem item = parameters[i];

                //values += item.DisplayValue(info);
                values += OperatorPrecedenceUnit.FillBreak(item, i == 0, operLevel, info);
                if (i < parameters.Count - 1)
                {
                    values += connect;
                }
            }

            return values;
        }
        
        #endregion
        

        
    
    }
}
