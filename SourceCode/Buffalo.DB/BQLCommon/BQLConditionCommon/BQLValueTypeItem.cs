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
        private object _itemValue;

        /// <summary>
        /// 值
        /// </summary>
        public object ItemValue
        {
            get { return _itemValue; }
            set { _itemValue = value; }
        }

        internal override bool IsNullValue()
        {
            return _itemValue==null;
        }
        /// <summary>
        /// 值类型项
        /// </summary>
        /// <param name="itemValue">值</param>
        public BQLValueTypeItem(object itemValue)
        {

            this._itemValue = itemValue;

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
            if (_valueDbType == DbType.Object && _itemValue!=null)
            {
                Type eType = _itemValue.GetType();
                if (_itemValue is Enum)
                {
                    eType = Enum.GetUnderlyingType(eType);
                }
                _valueDbType = DefaultType.ToDbType(eType);//自匹配类型
            }
        }

        internal override string DisplayValue(KeyWordInfomation info)
        {
            //string ret = null;

            if (info.ParamList != null && _valueDbType != DbType.Object && !info.OutPutModle && _itemValue!=null) 
            {
                if(_itemValue is Enum)
                {
                    Type eType = _itemValue.GetType();
                    _itemValue =Convert.ChangeType(_itemValue,Enum.GetUnderlyingType(eType));
                }
                DBParameter dbPrm=info.ParamList.NewParameter(_valueDbType, _itemValue,info.DBInfo);
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
            object value = _itemValue;
            if (value == null)
            {
                return "null";
            }

            if (value is Enum)
            {
                valueDataType = value.GetType();
                valueDataType = Enum.GetUnderlyingType(valueDataType);
                value = Convert.ChangeType(value, valueDataType);
            }
            else
            {
                valueDataType = _itemValue.GetType();
            }

            if (DefaultType.EqualType(valueDataType, DefaultType.StringType) || DefaultType.EqualType(valueDataType, DefaultType.GUIDType))
            {
                return DataAccessCommon.FormatValue(value, DbType.String, info.DBInfo);
            }
            else if (DefaultType.EqualType(valueDataType, DefaultType.DateTimeType))
            {
                return DataAccessCommon.FormatValue(value, DbType.DateTime, info.DBInfo);
            }
            else if (DefaultType.EqualType(valueDataType, DefaultType.BytesType))
            {
                return DataAccessCommon.FormatValue(value, DbType.Binary, info.DBInfo);
            }

            else if (DefaultType.EqualType(valueDataType, DefaultType.BooleanType))
            {
                return DataAccessCommon.FormatValue(value, DbType.Boolean, info.DBInfo);
            }
            else if (DefaultType.EqualType(valueDataType, DefaultType.GUIDType))
            {
                return DataAccessCommon.FormatValue(value, DbType.Guid, info.DBInfo);
            }


            return value.ToString();
        }
    }
}
