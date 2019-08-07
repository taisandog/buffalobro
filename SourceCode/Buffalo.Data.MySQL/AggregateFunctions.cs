
namespace Buffalo.Data.MySQL
{
    /// <summary>
    /// �ۺϺ�������
    /// </summary>
    public class AggregateFunctions : Buffalo.DB.DataBaseAdapter.Oracle9Adapter.AggregateFunctions
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
            //This function is provided for compatibility with Oracle. As of MySQL 5.0.3,
            //the standard SQL function STDDEV_POP() can be used instead.
            return "stddev(" + paramName + ")";
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
