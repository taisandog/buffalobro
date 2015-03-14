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
        #region 函数解释
        
        
        /// <summary>
        /// 运算符函数
        /// </summary>
        /// <param name="fHandle">函数信息</param>
        /// <param name="connect">连接符号</param>
        /// <returns></returns>
        private static string OperatorFunction(BQLOperatorHandle fHandle, string connect, KeyWordInfomation info)
        {
            fHandle.ValueDbType = DbType.Double;
            return CustomerConnectFunction(fHandle.GetParameters(), connect, info, fHandle.PriorityLevel);
        }
        /// <summary>
        /// 运算符函数
        /// </summary>
        /// <param name="fHandle">函数信息</param>
        /// <param name="connect">连接符号</param>
        /// <returns></returns>
        private static string ConditionsFunction(BQLComparItem fHandle, string connect, KeyWordInfomation info)
        {
            fHandle.ValueDbType = DbType.Boolean;
            return CustomerConnectFunction(fHandle.GetParameters(), connect, info, fHandle.PriorityLevel);
        }
        /// <summary>
        /// 进行加法运算
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        internal static string DoAdd(BQLOperatorHandle fHandle, KeyWordInfomation info)
        {
            BQLValueItem[] parameters=fHandle.GetParameters();
            BQLValueItem item1 = parameters[0];
            BQLValueItem item2 = parameters[1];

            if (IsStringDBType(item1.ValueDbType) || IsStringDBType(item2.ValueDbType)) //字符串拼合
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
        /// 判断是否字符串类型
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
        /// Not运算
        /// </summary>
        /// <param name="fun"></param>
        /// <returns></returns>
        internal static string DoNot(BQLComparItem handle, KeyWordInfomation info) 
        {
            IList<BQLValueItem> items = handle.GetParameters();
            if (items != null && items.Count > 0) 
            {
                return " not " + items[0].DisplayValue(info);
            }
            return "";
        }

        /// <summary>
        /// 进行加法运算
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        internal static string DoSub(BQLOperatorHandle handle, KeyWordInfomation info)
        {
            return OperatorFunction(handle, "-", info);
        }
        /// <summary>
        /// 进行乘法运算
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        internal static string DoMul(BQLOperatorHandle handle, KeyWordInfomation info)
        {
            return OperatorFunction(handle, "*", info);
        }
        /// <summary>
        /// 进行除法运算
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        internal static string DoDiv(BQLOperatorHandle handle, KeyWordInfomation info)
        {
            return OperatorFunction(handle, "/", info);
        }

        /// <summary>
        /// 进行除法运算
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
        /// 进行等于运算
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
        /// 不等于
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
        /// and 连接
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        internal static string DoAnd(BQLComparItem handle, KeyWordInfomation info)
        {
            return ConditionsFunction(handle, " and ",info);
        }
        /// <summary>
        /// or 连接
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        internal static string DoOr(BQLComparItem handle, KeyWordInfomation info)
        {
            return ConditionsFunction(handle, " or ",info);
        }
        /// <summary>
        /// 大于
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        internal static string DoMore(BQLComparItem handle, KeyWordInfomation info)
        {
            return ConditionsFunction(handle, ">",info);
        }
        /// <summary>
        /// 大于等于
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        internal static string DoMorethen(BQLComparItem handle, KeyWordInfomation info)
        {
            return ConditionsFunction(handle, ">=",info);
        }
        /// <summary>
        /// 小于
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        internal static string DoLess(BQLComparItem handle, KeyWordInfomation info)
        {
            return ConditionsFunction(handle, "<",info);
        }
        /// <summary>
        /// 小于等于
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        internal static string DoLessThen(BQLComparItem handle, KeyWordInfomation info)
        {
            return ConditionsFunction(handle, "<=",info);
        }
        /// <summary>
        /// 普通连接函数
        /// </summary>
        /// <param name="handle">函数</param>
        /// <param name="connect">连接符</param>
        /// <param name="operLevel">优先级</param>
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
