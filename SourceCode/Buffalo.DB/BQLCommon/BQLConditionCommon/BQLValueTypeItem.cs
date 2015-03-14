using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Buffalo.Kernel.Defaults;
using Buffalo.DB.CommBase.DataAccessBases;
using Buffalo.Kernel;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.DB.DbCommon;

namespace Buffalo.DB.BQLCommon.BQLConditionCommon
{
    /// <summary>
    /// 值类型项
    /// </summary>
    public class BQLValueTypeItem:BQLValueItem
    {
        private object itemValue;

        public object ItemValue
        {
            get { return itemValue; }
            set { itemValue = value; }
        }

        internal override bool IsNullValue()
        {
            return itemValue==null;
        }
        /// <summary>
        /// 值类型项
        /// </summary>
        /// <param name="itemValue">值</param>
        public BQLValueTypeItem(object itemValue)
        {

            this.itemValue = itemValue;

        }
        ///// <summary>
        ///// 此项的值
        ///// </summary>
        //public object ItemValue 
        //{
        //    get 
        //    {
        //        return itemValue;
        //    }
        //}

        internal override void FillInfo(KeyWordInfomation info)
        {
            if (_valueDbType == DbType.Object)
            {
                _valueDbType = DefaultType.ToDbType(itemValue.GetType());//自匹配类型
            }
        }

        internal override string DisplayValue(KeyWordInfomation info)
        {
            //string ret = null;
            
            if (info.ParamList != null && _valueDbType != DbType.Object && !info.OutPutModle) 
            {
                DBParameter dbPrm=info.ParamList.NewParameter(_valueDbType, itemValue,info.DBInfo);
                return dbPrm.ValueName;
            }

            return FormatValueType(info);
        }

        


        /// <summary>
        /// 格式化值项
        /// </summary>
        /// <param name="valueItem"></param>
        /// <returns></returns>
        private string FormatValueType(KeyWordInfomation info)
        {
            Type valueDataType = null;
            if (itemValue != null) 
            {
                valueDataType = itemValue.GetType();
            }
            if (DefaultType.EqualType(valueDataType , DefaultType.StringType) || DefaultType.EqualType(valueDataType , DefaultType.GUIDType))
            {
                return DataAccessCommon.FormatValue(itemValue, DbType.String, info.DBInfo);
            }
            else if (DefaultType.EqualType(valueDataType , DefaultType.DateTimeType))
            {
                return DataAccessCommon.FormatValue(itemValue, DbType.DateTime, info.DBInfo);
            }
            else if (DefaultType.EqualType(valueDataType , DefaultType.BytesType))
            {
                return DataAccessCommon.FormatValue(itemValue, DbType.Binary, info.DBInfo);
            }

            else if (DefaultType.EqualType(valueDataType , DefaultType.BooleanType) )
            {
                return DataAccessCommon.FormatValue(itemValue, DbType.Boolean, info.DBInfo);
            }
            else if (DefaultType.EqualType(valueDataType, DefaultType.GUIDType))
            {
                return DataAccessCommon.FormatValue(itemValue, DbType.Guid, info.DBInfo);
            }
            else if (valueDataType == null) 
            {
                return "null";
            }
            return itemValue.ToString();
        }
    }
}
