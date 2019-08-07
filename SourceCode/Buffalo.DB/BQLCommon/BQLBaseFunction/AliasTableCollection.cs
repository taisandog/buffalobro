using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.BQLCommon.BQLConditionCommon;

namespace Buffalo.DB.BQLCommon.BQLBaseFunction
{
    public class AliasTableCollection
    {
        internal AliasTableCollection() { }
        /// <summary>
        /// 获取别名表
        /// </summary>
        /// <param name="aliasName">别名表名</param>
        /// <returns></returns>
        public AliasTabelParamCollection this[string aliasName] 
        {
            get 
            {
                return new AliasTabelParamCollection(aliasName);
            }
        }
    }

    public class AliasTabelParamCollection 
    {
        private string aliasName;
        /// <summary>
        /// 产生别名表
        /// </summary>
        /// <param name="aliasName">别名</param>
        internal AliasTabelParamCollection(string aliasName) 
        {
            this.aliasName = aliasName;
        }
        /// <summary>
        /// 别名表的属性
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <returns></returns>
        public AliasTabelParamHandle _
        {
            get
            {
                return new AliasTabelParamHandle(aliasName, "*");
            }
        }
        /// <summary>
        /// 别名表的属性
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <returns></returns>
        public AliasTabelParamHandle this[string propertyName] 
        {
            get 
            {
                return new AliasTabelParamHandle(aliasName, propertyName);
            }
        }
    }
}
