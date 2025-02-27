using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.BQLCommon.BQLConditionCommon;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.Kernel.Defaults;
using System.Data;
using Buffalo.Kernel;

namespace Buffalo.DB.BQLCommon.BQLAggregateFunctions
{
    public delegate string DelAggregateFunctionHandle(string paramName,DBInfo info);
    public class BQLAggregateFunction : BQLParamHandle
    {
        private DelAggregateFunctionHandle functionHandle;
        private BQLParamHandle param;
        /// <summary>
        /// 聚合函数
        /// </summary>
        /// <param name="functionName">函数名</param>
        /// <param name="param"></param>
        public BQLAggregateFunction(DelAggregateFunctionHandle functionHandle, BQLParamHandle param)

        {
            this.functionHandle = functionHandle;
            this.param = param;
        }
        internal override void FillInfo(KeyWordInfomation info)
        {
            BQLValueItem.DoFillInfo(param, info);
        }

        internal override string DisplayValue(KeyWordInfomation info)
        {
            DelAggregateFunctionHandle handle = functionHandle;
            if (handle != null) 
            {
                string strParam = null;
                if (!CommonMethods.IsNull(param))
                {
                    strParam = param.DisplayValue(info);
                }
                else 
                {
                    strParam = "*";
                }
                if (this.ValueDbType == DbType.Object) //如果此函数为未知返回类型时候就自动转为字段类型
                {
                    _valueDbType = param.ValueDbType;
                }
                return handle(strParam,info.DBInfo);
            }
            return null;
        }

        
    }
}
