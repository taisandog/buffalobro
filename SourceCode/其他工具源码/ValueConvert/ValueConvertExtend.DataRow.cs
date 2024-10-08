﻿using System;
using System.Collections;
using System.Data;
using System.Text;
namespace Buffalo.Kernel
{
    /// <summary>
    /// 类型转换扩展
    /// </summary>
    public static partial class ValueConvertExtend
    {

        /// <summary>
        /// 获取DataRow的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="row">DataRow</param>
        /// <param name="name">指定列名</param>
        /// <param name="defalutValue"></param>
        /// <returns></returns>
        public static T GetRowValueData<T>(DataRow row, string name, T defalutValue = default(T))
        {

            if (!row.Table.Columns.Contains(name))
            {
                return defalutValue;
            }
            object value = row[name];
            
            return ConvertValue<T>(value, defalutValue);
        }

        /// <summary>
        /// 获取DataRow的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="row">DataRow</param>
        /// <param name="name">指定列名</param>
        /// <param name="defalutValue">找不到时候返回的默认值</param>
        /// <returns></returns>
        public static T GetRowValue<T>(this DataRow row, string name, T defalutValue = default(T))
        {

            return GetRowValueData<T>(row, name, defalutValue);
        }
        

    }

}