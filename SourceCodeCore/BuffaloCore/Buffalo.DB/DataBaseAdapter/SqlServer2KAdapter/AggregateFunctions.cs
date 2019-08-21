using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;

namespace Buffalo.DB.DataBaseAdapter.SqlServer2KAdapter
{
    /// <summary>
    /// �ۺϺ�������
    /// </summary>
    public class AggregateFunctions : IAggregateFunctions
    {
        /// <summary>
        /// ����sum����
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public virtual string DoSum(string paramName) 
        {
            return "sum(" + paramName + ")";
        }
        /// <summary>
        /// ����Count����
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public virtual string DoCount(string paramName)
        {
            return "count(" + paramName + ")";
        }
        /// <summary>
        /// ����Max����
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public virtual string DoMax(string paramName)
        {
            return "max(" + paramName + ")";
        }
        /// <summary>
        /// ����Min����
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public virtual string DoMin(string paramName)
        {
            return "min(" + paramName + ")";
        }
        /// <summary>
        /// ����StdDev����
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public virtual string DoStdDev(string paramName)
        {
            return "stdev(" + paramName + ")";
        }
        /// <summary>
        /// ����Avg����
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public virtual string DoAvg(string paramName)
        {
            return "avg(" + paramName + ")";
        }
    }
}
