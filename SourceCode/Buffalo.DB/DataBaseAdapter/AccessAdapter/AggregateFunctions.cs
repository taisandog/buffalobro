using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;

namespace Buffalo.DB.DataBaseAdapter.AccessAdapter
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
            return "Sum(" + paramName + ")";
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
            return "Max(" + paramName + ")";
        }
        /// <summary>
        /// ����Min����
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public virtual string DoMin(string paramName)
        {
            return "Min(" + paramName + ")";
        }
        /// <summary>
        /// ����StdDev����
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public virtual string DoStdDev(string paramName)
        {
            return "StDev(" + paramName + ")";
        }
        /// <summary>
        /// ����Avg����
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public virtual string DoAvg(string paramName)
        {
            return "Avg(" + paramName + ")";
        }
    }
}
