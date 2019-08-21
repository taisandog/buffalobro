using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.EntityInfos;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.DB.BQLCommon.BQLKeyWordCommon;
using System.Data;
using Buffalo.Kernel;

namespace Buffalo.DB.BQLCommon.BQLConditionCommon
{
    public abstract class BQLTableHandle : BQLValueItem
    {
        
        /// <summary>
        /// 获取对应的实体属性
        /// </summary>
        /// <returns></returns>
        internal abstract List<ParamInfo> GetParamInfoHandle();
        /// <summary>
        /// 给这个表定义一个别名
        /// </summary>
        /// <param name="asName">别名</param>
        /// <returns></returns>
        public BQLAliasHandle AS(string asName)
        {
            BQLAliasHandle item = new BQLAliasHandle(this, asName);
            return item;
        }

        /// <summary>
        /// 获取字段
        /// </summary>
        /// <param name="paramName">字段名</param>
        /// <returns></returns>
        public virtual BQLParamHandle this[string paramName]
        {
            get
            {
                return new BQLOtherParamHandle(this, paramName);
            }
        }
        /// <summary>
        /// 主键
        /// </summary>
        public virtual List<string> GetPrimaryParam()
        {

            return null;

        }

        /// <summary>
        /// 获取字段
        /// </summary>
        /// <param name="paramName">字段名</param>
        /// <returns></returns>
        public virtual BQLParamHandle this[string paramName,DbType dbType]
        {
            get
            {
                BQLOtherParamHandle prm = new BQLOtherParamHandle(this, paramName);
                prm.ValueDbType = dbType;
                return prm;
            }
        }
        protected BQLParamHandle __ = null;
        /// <summary>
        /// 获取所有字段
        /// </summary>
        /// <returns></returns>
        public virtual BQLParamHandle _
        {
            get
            {
                if (CommonMethods.IsNull(__))
                {
                    __ = new BQLOtherParamHandle(this, "*");
                }
                return __;
            }
        }
    }
}
