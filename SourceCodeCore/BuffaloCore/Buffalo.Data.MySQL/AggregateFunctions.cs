
namespace Buffalo.Data.MySQL
{
    /// <summary>
    /// 聚合函数处理
    /// </summary>
    public class AggregateFunctions : Buffalo.DB.DataBaseAdapter.SqlServer2KAdapter.AggregateFunctions
    {
        
        /// <summary>
        /// 处理sum函数
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public virtual string DoSum(string paramName)
        {
            return "sum(" + paramName + ")";
        }
        /// <summary>
        /// 处理Count函数
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public virtual string DoCount(string paramName)
        {
            return "count(" + paramName + ")";
        }
        /// <summary>
        /// 处理Max函数
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public virtual string DoMax(string paramName)
        {
            return "max(" + paramName + ")";
        }
        /// <summary>
        /// 处理Min函数
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public virtual string DoMin(string paramName)
        {
            return "min(" + paramName + ")";
        }
        /// <summary>
        /// 处理StdDev函数
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
        /// 处理Avg函数
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public virtual string DoAvg(string paramName)
        {
            return "avg(" + paramName + ")";
        }
    }
}
