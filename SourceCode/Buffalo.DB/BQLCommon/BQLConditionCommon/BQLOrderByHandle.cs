using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.EntityInfos;
using Buffalo.DB.QueryConditions;
using Buffalo.Kernel.Defaults;
using Buffalo.DB.DataBaseAdapter;

namespace Buffalo.DB.BQLCommon.BQLConditionCommon
{
    public class BQLOrderByHandle : BQLParamHandle
    {
        private SortType _sortType;
        private BQLParamHandle _arg;
        internal BQLOrderByHandle(BQLParamHandle arg, SortType sortType)
        {
            
            this._sortType = sortType;
            this._arg = arg;
            //this.valueType = BQLValueType.OrderBy;
        }

        /// <summary>
        /// 项的排序类型
        /// </summary>
        internal SortType SortType
        {
            get
            {
                return _sortType;
            }
        }

        /// <summary>
        /// 项的属性
        /// </summary>
        internal BQLParamHandle Param
        {
            get
            {
                return _arg;
            }
        }
       
        internal override string DisplayValue(KeyWordInfomation info)
        {
            info.IsWhere = true;
            string orderby = _arg.DisplayValue(info);
            if (_sortType == SortType.DESC)
            {
                orderby += " desc";
            }
            info.IsWhere = false;
            return orderby;
        }


        internal override void FillInfo(KeyWordInfomation info)
        {
            BQLValueItem.DoFillInfo(_arg, info);
        }
    }
}