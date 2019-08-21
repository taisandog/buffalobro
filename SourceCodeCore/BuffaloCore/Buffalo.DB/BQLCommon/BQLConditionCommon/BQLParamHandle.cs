using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.BQLCommon.BQLConditionCommon;
using Buffalo.DB.QueryConditions;
using Buffalo.DB.BQLCommon.BQLConditions;
using Buffalo.DB.BQLCommon.BQLKeyWordCommon;
using Buffalo.DB.BQLCommon.BQLExtendFunction;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.Kernel.Defaults;
using System.Data;
using System.Collections;
using Buffalo.DB.DBFunction;

namespace Buffalo.DB.BQLCommon.BQLConditionCommon
{
    public abstract class BQLParamHandle:BQLValueItem
    {

        //private IMathFunctions maths = null;
        //private IConvertFunction convert=null;
        public BQLParamHandle() 
        {
            //IMathFunctions maths = db.Math;
            //IConvertFunction convert = db.Convert;
        }
        

        

        /// <summary>
        /// 降序排序
        /// </summary>
        public BQLOrderByHandle DESC 
        {
            get 
            {
                return new BQLOrderByHandle(this, SortType.DESC);
            }
        }
        /// <summary>
        /// 升序排序
        /// </summary>
        public BQLOrderByHandle ASC
        {
            get
            {
                return new BQLOrderByHandle(this, SortType.ASC);
            }
        }

        //public BQLConditionItem 
        /// <summary>
        /// In条件(如果集合为空，则返回1=2)
        /// </summary>
        /// <param name="lstParam"></param>
        /// <returns></returns>
        public BQLConditionItem In(params ValueType[] values) 
        {
            return new BQLConditionItem(this, values, BQLConditionManager.DoIn);
        }
        /// <summary>
        /// In条件(如果集合为空，则返回1=2)
        /// </summary>
        /// <param name="lstParam"></param>
        /// <returns></returns>
        public BQLConditionItem In(params string[] values)
        {
            return new BQLConditionItem(this, values, BQLConditionManager.DoIn);
        }
        /// <summary>
        /// In条件(如果集合为空，则返回1=2)
        /// </summary>
        /// <param name="lstParam"></param>
        /// <returns></returns>
        public BQLConditionItem In(IEnumerable collection)
        {
            return new BQLConditionItem(this, collection, BQLConditionManager.DoIn);
        }
        /// <summary>
        /// In条件(如果集合为空，则返回1=2)
        /// </summary>
        /// <param name="lstParam"></param>
        /// <returns></returns>
        public BQLConditionItem In(BQLQuery item)
        {
            return new BQLConditionItem(this, item, BQLConditionManager.DoIn);
        }
        /// <summary>
        /// NotIn条件(如果集合为空，则返回1=1)
        /// </summary>
        /// <param name="lstParam"></param>
        /// <returns></returns>
        public BQLConditionItem NotIn(params ValueType[] values)
        {
            return new BQLConditionItem(this, values, BQLConditionManager.DoNotIn);
        }
        /// <summary>
        /// NotIn条件(如果集合为空，则返回1=1)
        /// </summary>
        /// <param name="lstParam"></param>
        /// <returns></returns>
        public BQLConditionItem NotIn(params string[] values)
        {
            return new BQLConditionItem(this, values, BQLConditionManager.DoNotIn);
        }
        /// <summary>
        /// NotIn条件(如果集合为空，则返回1=1)
        /// </summary>
        /// <param name="lstParam"></param>
        /// <returns></returns>
        public BQLConditionItem NotIn(IEnumerable collection)
        {
            return new BQLConditionItem(this, collection, BQLConditionManager.DoNotIn);
        }
        /// <summary>
        /// NotIn条件(如果集合为空，则返回1=1)
        /// </summary>
        /// <param name="lstParam"></param>
        /// <returns></returns>
        public BQLConditionItem NotIn(BQLQuery item)
        {
            return new BQLConditionItem(this, item, BQLConditionManager.DoNotIn);
        }

        
        
    }
}
