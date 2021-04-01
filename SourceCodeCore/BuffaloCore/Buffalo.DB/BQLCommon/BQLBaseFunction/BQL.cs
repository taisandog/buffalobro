using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.BQLCommon.BQLKeyWordCommon;
using Buffalo.DB.BQLCommon.BQLConditionCommon;
using Buffalo.DB.BQLCommon.BQLAggregateFunctions;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;
using Buffalo.DB.BQLCommon.BQLExtendFunction;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.Kernel.Defaults;
using Buffalo.DB.QueryConditions;
using System.Data;
using Buffalo.DB.DBFunction;
using Buffalo.Kernel;

namespace Buffalo.DB.BQLCommon.BQLBaseFunction
{
    /// <summary>
    /// BQL关键字
    /// </summary>
    public class BQL
    {
        /// <summary>
        /// Select表
        /// </summary>
        /// <param name="args">要输出的字段</param>
        /// <returns></returns>
        public static KeyWordSelectItem Select(params BQLParamHandle[] args)
        {
            KeyWordSelectItem selectItem = new KeyWordSelectItem(args, null);
            return selectItem;
        }
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="arg">要被插入的表</param>
        /// <returns></returns>
        public static KeyWordInserItem InsertInto(BQLTableHandle arg)
        {
            KeyWordInserItem item = new KeyWordInserItem(arg, null);
            return item;
        }

        /// <summary>
        /// 创建表
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns></returns>
        public static KeyWordCreateTableItem CreateTable(string tableName) 
        {
            KeyWordCreateTableItem item = new KeyWordCreateTableItem(tableName, null);
            return item;
        }
        /// <summary>
        /// 修改表
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns></returns>
        public static KeyWordAlterTableItem AlterTable(string tableName) 
        {
            KeyWordAlterTableItem item = new KeyWordAlterTableItem(tableName, null);
            return item;
        }
        
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="arg">要被更新的表</param>
        /// <returns></returns>
        public static KeyWordUpdateItem Update(BQLTableHandle arg)
        {
            KeyWordUpdateItem item = new KeyWordUpdateItem(arg, null);
            return item;
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="arg">要被删除数据的表</param>
        /// <returns></returns>
        public static KeyWordDeleteItem DeleteFrom(BQLTableHandle arg)
        {
            KeyWordDeleteItem item = new KeyWordDeleteItem(arg, null);
            return item;
        }

        /// <summary>
        /// Case
        /// </summary>
        /// <param name="value">要选择的值或表达式</param>
        /// <returns></returns>
        public static KeyWordCaseItem Case(BQLValueItem value)
        {
            KeyWordCaseItem item = new KeyWordCaseItem(value, null);
            return item;
        }

        /// <summary>
        /// Not
        /// </summary>
        /// <param name="value">Not</param>
        /// <returns></returns>
        public static BQLComparItem Not(BQLCondition handle)
        {

            BQLComparItem fHandle = new BQLComparItem(FunctionManager.DoNot, new BQLValueItem[] { handle });
            fHandle.PriorityLevel = 4;
            return fHandle;
        }
    
        /// <summary>
        /// Case
        /// </summary>
        /// <returns></returns>
        public static KeyWordCaseItem Case()
        {
            KeyWordCaseItem item = new KeyWordCaseItem(null, null);
            return item;
        }


        #region 别名
        /// <summary>
        /// 一个别名表
        /// </summary>
        /// <param name="table">表</param>
        /// <param name="asName">别名</param>
        /// <returns></returns>
        public static BQLAliasHandle TableAs(BQLEntityTableHandle table, string asName)
        {
            BQLAliasHandle item = new BQLAliasHandle(table, asName);
            return item;
        }
        /// <summary>
        /// 别名查询
        /// </summary>
        /// <param name="query">查询</param>
        /// <param name="asName">别名</param>
        /// <returns></returns>
        public static BQLAliasHandle TableAs(BQLQuery query, string asName)
        {
            BQLAliasHandle item = new BQLAliasHandle(query, asName);
            return item;
        }
        /// <summary>
        /// 别名字段
        /// </summary>
        /// <param name="param">字段</param>
        /// <param name="asName">别名</param>
        /// <returns></returns>
        public static BQLAliasParamHandle ParamAs(BQLParamHandle param, string asName)
        {
            BQLAliasParamHandle item = new BQLAliasParamHandle(param, asName);
            return item;
        }
        private static AliasTableCollection tabColl = new AliasTableCollection();
        /// <summary>
        /// 别名表
        /// </summary>
        public static AliasTableCollection Tables
        {
            get
            {
                return tabColl;
            }
        }

        public static BQLOtherTableHandle ToTable(string tableName)
        {
            return new BQLOtherTableHandle(tableName);

        }
        
        /// <summary>
        /// 把字段名转为Param类型
        /// </summary>
        public static BQLOtherParamHandle ToParam(string paramName)
        {
            return new BQLOtherParamHandle(null, paramName);
        }
        //private static BQLAlias _alias = new BQLAlias();
        ///// <summary>
        ///// 别名函数
        ///// </summary>
        //protected static BQLAlias ALIAS
        //{
        //    get
        //    {
        //        return _alias;
        //    }
        //}
        #endregion

        #region 常用函数

        //ICommonFunction common = DbAdapterLoader.Common;
        /// <summary>
        /// 判断是否为空
        /// </summary>
        /// <param name="source">源值</param>
        /// <param name="nullValue">如果为空的话的输出值</param>
        /// <returns></returns>
        public static CsqCommonFunction IsNull(BQLValueItem source, BQLValueItem nullValue)
        {

            CsqCommonFunction handle = new CsqCommonFunction(new BQLValueItem[] { source, nullValue }, DBCommonFunction.IsNull, source.ValueDbType);
            if (!CommonMethods.IsNull(nullValue))
            {
                handle.ValueDbType = nullValue.ValueDbType;
            }
            else if (!CommonMethods.IsNull(source))
            {
                handle.ValueDbType = source.ValueDbType;
            }
            return handle;
        }
        /// <summary>
        /// 判断是否为空
        /// </summary>
        /// <param name="source">源值</param>
        /// <returns></returns>
        public static CsqCommonFunction Length(BQLValueItem source)
        {

            CsqCommonFunction handle = new CsqCommonFunction(new BQLValueItem[] { source }, DBCommonFunction.Len, DbType.Int32);

            //handle.ValueDataType = DefaultType.IntType;

            return handle;
        }

        /// <summary>
        /// distinct，不重复字段
        /// </summary>
        /// <param name="param">字段</param>
        /// <returns></returns>
        public static CsqCommonFunction Distinct(params BQLParamHandle[] cparams)
        {
            CsqCommonFunction handle = new CsqCommonFunction(cparams, DBCommonFunction.Distinct, DbType.Object);
            return handle;
        }
        /// <summary>
        /// 调用自定义函数
        /// </summary>
        /// <param name="functionName">函数名</param>
        /// <param name="returnType">返回值类型</param>
        /// <param name="values">函数值</param>
        /// <returns></returns>
        public static BQLCustomizeFunction Call(string functionName,DbType returnType, params BQLValueItem[] values)
        {
            BQLCustomizeFunction handle = new BQLCustomizeFunction(functionName, returnType, values);
            return handle;
        }
        /// <summary>
        /// 调用自定义函数
        /// </summary>
        /// <param name="functionName">函数名</param>
        /// <param name="returnType">返回值类型</param>
        /// <param name="values">函数值</param>
        /// <returns></returns>
        public static BQLCustomizeFunction Call(string functionName)
        {
            BQLCustomizeFunction handle = new BQLCustomizeFunction(functionName, DbType.Object, null);
            return handle;
        }

        /// <summary>
        /// 调用自定义函数
        /// </summary>
        /// <param name="handle">函数</param>
        /// <param name="returnType">返回值类型</param>
        /// <param name="values">函数值</param>
        /// <returns></returns>
        public static BQLCustomizeFunction Call(DelCustomizeFunction handle, DbType returnType, params BQLValueItem[] values)
        {
            BQLCustomizeFunction chandle = new BQLCustomizeFunction(handle, returnType, values);
            return chandle;
        }
        /// <summary>
        /// 调用自定义函数
        /// </summary>
        /// <param name="handle">函数</param>
        /// <returns></returns>
        public static BQLCustomizeFunction Call(DelCustomizeFunction handle)
        {
            BQLCustomizeFunction chandle = new BQLCustomizeFunction(handle, DbType.Object, null);
            return chandle;
        }

        #endregion

        #region 聚合函数
        //IAggregateFunctions agf = DbAdapterLoader.Aggregate;
        /// <summary>
        /// 总计
        /// </summary>
        /// <param name="functionHandle">函数</param>
        /// <param name="param">字段</param>
        /// <returns></returns>
        public static BQLAggregateFunction Sum(BQLParamHandle param)
        {
            BQLAggregateFunction item = new BQLAggregateFunction(DBAggregateFunction.DoSum, param);
            item.ValueDbType = DbType.Double;
            //item.ValueDataType = DefaultType.DoubleType;
            return item;
        }
        /// <summary>
        /// StdDev
        /// </summary>
        /// <param name="functionHandle">函数</param>
        /// <param name="param">字段</param>
        /// <returns></returns>
        public static BQLAggregateFunction StdDev(BQLParamHandle param)
        {
            BQLAggregateFunction item = new BQLAggregateFunction(DBAggregateFunction.DoStdDev, param);
            item.ValueDbType = DbType.Double;
            return item;
        }

        /// <summary>
        /// 最小
        /// </summary>
        /// <param name="functionHandle">函数</param>
        /// <param name="param">字段</param>
        /// <returns></returns>
        public static BQLAggregateFunction Min(BQLParamHandle param)
        {
            BQLAggregateFunction item = new BQLAggregateFunction(DBAggregateFunction.DoMin, param);
            return item;
        }
        /// <summary>
        /// 最大
        /// </summary>
        /// <param name="functionHandle">函数</param>
        /// <param name="param">字段</param>
        /// <returns></returns>
        public static BQLAggregateFunction Max(BQLParamHandle param)
        {
            BQLAggregateFunction item = new BQLAggregateFunction(DBAggregateFunction.DoMax, param);
            return item;
        }
        /// <summary>
        /// 总行数
        /// </summary>
        /// <param name="functionHandle">函数</param>
        /// <param name="param">字段</param>
        /// <returns></returns>
        public static BQLAggregateFunction Count(BQLParamHandle param)
        {
            BQLAggregateFunction item = new BQLAggregateFunction(DBAggregateFunction.DoCount, param);
            item.ValueDbType = DbType.Int32;
            return item;
        }
        /// <summary>
        /// 总行数
        /// </summary>
        /// <param name="functionHandle">函数</param>
        /// <param name="param">字段</param>
        /// <returns></returns>
        public static BQLAggregateFunction Count()
        {

            return Count(null);
        }
        /// <summary>
        /// 平均值
        /// </summary>
        /// <param name="functionHandle">函数</param>
        /// <param name="param">字段</param>
        /// <returns></returns>
        public static BQLAggregateFunction Avg(BQLParamHandle param)
        {
            BQLAggregateFunction item = new BQLAggregateFunction(DBAggregateFunction.DoAvg, param);
            item.ValueDbType = DbType.Double;
            return item;
        }
        


        #endregion

        #region 数学函数
        /// <summary>
        /// 当前时间
        /// </summary>
        /// <param name="dbType">时间类型</param>
        /// <param name="isUTC">是否格林威治时间</param>
        /// <returns></returns>
        public static BQLNowDateHandle NowDate(DbType dbType,bool isUTC)
        {
            BQLNowDateHandle handle = new BQLNowDateHandle(dbType,isUTC);
            
            //handle.ValueDbType = DbType.Double;
            return handle;
        }
        /// <summary>
        /// 当前时间
        /// </summary>
        /// <returns></returns>
        public static BQLNowDateHandle NowDate()
        {
            return NowDate(DbType.DateTime,false);
        }
        /// <summary>
        /// 当前时间戳
        /// </summary>
        /// <returns></returns>
        public static BQLTimeStampHandle NowTimeStamp()
        {
            BQLTimeStampHandle handle = new BQLTimeStampHandle();

            //handle.ValueDbType = DbType.Double;
            return handle;
        }
        /// <summary>
        /// 绝对值
        /// </summary>
        /// <param name="value">求绝对值</param>
        /// <returns></returns>
        public static CsqCommonFunction Abs(BQLValueItem value)
        {
            value.ValueDbType = DbType.Double;
            CsqCommonFunction handle = new CsqCommonFunction(new BQLValueItem[] { value }, DBMathFunction.DoAbs, DbType.Double);
            //handle.ValueDbType = DbType.Double;
            return handle;
        }
        /// <summary>
        /// 反余弦函数
        /// </summary>
        /// <param name="value">弧度</param>
        /// <returns></returns>
        public static CsqCommonFunction Acos(BQLValueItem value)
        {
            value.ValueDbType = DbType.Double;
            CsqCommonFunction handle = new CsqCommonFunction(new BQLValueItem[] { value }, DBMathFunction.DoAcos, DbType.Double);
            //handle.ValueDbType = DbType.Double;
            return handle;
        }

        /// <summary>
        /// 反正弦函数
        /// </summary>
        /// <param name="value">弧度</param>
        /// <returns></returns>
        public static CsqCommonFunction Asin(BQLValueItem value)
        {
            value.ValueDbType = DbType.Double;
            CsqCommonFunction handle = new CsqCommonFunction(new BQLValueItem[] { value }, DBMathFunction.DoAsin, DbType.Double);
            //handle.ValueDbType = DbType.Double;
            return handle;
        }

        /// <summary>
        /// 取模运算
        /// </summary>
        /// <param name="value">弧度</param>
        /// <returns></returns>
        public static BQLOperatorHandle MathMod(object value1, object value2)
        {
            
            BQLValueItem oValue1= BQLValueItem.ToValueItem(value1);
            BQLValueItem oValue2 = BQLValueItem.ToValueItem(value2);
            BQLValueItem.UnityDbType(oValue1, oValue2);

            BQLOperatorHandle fHandle = new BQLOperatorHandle(FunctionManager.DoMod, new BQLValueItem[] { oValue1, oValue2 });
            //fHandle.PriorityLevel = 3;
            fHandle.PriorityLevel = OperatorPrecedenceUnit.GetPrecedence("%");
            return fHandle;
        }
        /// <summary>
        /// 反正切
        /// </summary>
        /// <param name="value">弧度</param>
        /// <returns></returns>
        public static CsqCommonFunction Atan(BQLValueItem value)
        {
            value.ValueDbType = DbType.Double;
            CsqCommonFunction handle = new CsqCommonFunction(new BQLValueItem[] { value }, DBMathFunction.DoAtan, DbType.Double);
            //handle.ValueDbType = DbType.Double;
            return handle;
        }

        /// <summary>
        /// 计算两个变量 x 和 y 的反正切值
        /// </summary>
        /// <param name="y">定点的 y 坐标</param>
        /// <param name="x">定点的 x 坐标</param>
        /// <returns></returns>
        public static CsqCommonFunction Atan2(BQLValueItem y, BQLValueItem x)
        {
            x.ValueDbType = DbType.Double;
            y.ValueDbType = DbType.Double;
            CsqCommonFunction handle = new CsqCommonFunction(new BQLValueItem[] { y, x }, DBMathFunction.DoAtan2, DbType.Double);
            //handle.ValueDataType = DefaultType.DoubleType;
            return handle;
        }

        /// <summary>
        /// 返回大于或等于此数值的整数
        /// </summary>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static CsqCommonFunction Ceil(BQLValueItem value)
        {
            value.ValueDbType = DbType.Double;
            CsqCommonFunction handle = new CsqCommonFunction(new BQLValueItem[] { value }, DBMathFunction.DoCeil, DbType.Int32);
            //handle.ValueDataType = DefaultType.IntType;
            return handle;
        }
        /// <summary>
        /// 余弦
        /// </summary>
        /// <param name="value">弧度</param>
        /// <returns></returns>
        public static CsqCommonFunction Cos(BQLValueItem value)
        {
            value.ValueDbType = DbType.Double;
            CsqCommonFunction handle = new CsqCommonFunction(new BQLValueItem[] { value }, DBMathFunction.DoCos, DbType.Double);
            //handle.ValueDataType = DefaultType.DoubleType;
            return handle;
        }
        /// <summary>
        /// 求数值的e为底的幂 
        /// </summary>
        /// <param name="values">数值</param>
        /// <returns></returns>
        public static CsqCommonFunction DoExp(BQLValueItem value)
        {
            value.ValueDbType = DbType.Double;
            CsqCommonFunction handle = new CsqCommonFunction(new BQLValueItem[] { value }, DBMathFunction.DoExp, DbType.Double);
            //handle.ValueDataType = DefaultType.DoubleType;
            return handle;
        }

        /// <summary>
        /// 返回小于或等于此数值的整数
        /// </summary>
        /// <param name="value">数值</param>
        /// <returns></returns>
        public static CsqCommonFunction DoFloor(BQLValueItem value)
        {
            value.ValueDbType = DbType.Double;
            CsqCommonFunction handle = new CsqCommonFunction(new BQLValueItem[] { value }, DBMathFunction.DoFloor, DbType.Int32);
            //handle.ValueDataType = DefaultType.IntType;
            return handle;
        }

        /// <summary>
        /// 取e为底的对数 
        /// </summary>
        /// <param name="values">数值</param>
        /// <returns></returns>
        public static CsqCommonFunction DoLn(BQLValueItem value)
        {
            value.ValueDbType = DbType.Double;
            CsqCommonFunction handle = new CsqCommonFunction(new BQLValueItem[] { value }, DBMathFunction.DoLn, DbType.Double);
            //handle.ValueDataType = DefaultType.DoubleType;
            return handle;
        }

        /// <summary>
        /// 以10为底数的对数 
        /// </summary>
        /// <param name="values">数值</param>
        /// <returns></returns>
        public static CsqCommonFunction DoLog10(BQLValueItem value)
        {
            value.ValueDbType = DbType.Double;
            CsqCommonFunction handle = new CsqCommonFunction(new BQLValueItem[] { value }, DBMathFunction.DoLog10, DbType.Double);
            //handle.ValueDataType = DefaultType.DoubleType;
            return handle;
        }

        /// <summary>
        /// 求模运算
        /// </summary>
        /// <param name="value1">要求的数字</param>
        /// <param name="value2">被除数</param>
        /// <returns></returns>
        public static CsqCommonFunction Mod(BQLValueItem value1, BQLValueItem value2)
        {
            value1.ValueDbType = DbType.Double;
            value2.ValueDbType = DbType.Double;
            CsqCommonFunction handle = new CsqCommonFunction(new BQLValueItem[] { value1, value2 }, DBMathFunction.DoMod, DbType.Double);
            //handle.ValueDataType = DefaultType.DoubleType;
            return handle;
        }
        /// <summary>
        /// 按位与运算
        /// </summary>
        /// <param name="value1">数字1</param>
        /// <param name="value2">数字2</param>
        /// <returns></returns>
        public static CsqCommonFunction BitAND(BQLValueItem value1, BQLValueItem value2)
        {
            value1.ValueDbType = DbType.Double;
            value2.ValueDbType = DbType.Double;
            CsqCommonFunction handle = new CsqCommonFunction(new BQLValueItem[] { value1,value2 }, DBMathFunction.BitAND, DbType.Double);
            //handle.ValueDataType = DefaultType.DoubleType;
            return handle;
        }
        /// <summary>
        /// 非运算
        /// </summary>
        /// <param name="value1">数字1</param>
        /// <param name="value2">数字2</param>
        /// <returns></returns>
        public static CsqCommonFunction BitNot(BQLValueItem value)
        {
            value.ValueDbType = DbType.Double;
            CsqCommonFunction handle = new CsqCommonFunction(new BQLValueItem[] { value }, DBMathFunction.BitNot, DbType.Double);
            //handle.ValueDataType = DefaultType.DoubleType;
            return handle;
        }
        /// <summary>
        /// 按位或运算
        /// </summary>
        /// <param name="value1">数字1</param>
        /// <param name="value2">数字2</param>
        /// <returns></returns>
        public static CsqCommonFunction BitOR(BQLValueItem value1, BQLValueItem value2)
        {
            value1.ValueDbType = DbType.Double;
            value2.ValueDbType = DbType.Double;
            CsqCommonFunction handle = new CsqCommonFunction(new BQLValueItem[] { value1, value2 }, DBMathFunction.BitOR, DbType.Double);
            //handle.ValueDataType = DefaultType.DoubleType;
            return handle;
        }
        /// <summary>
        /// 按位异或运算
        /// </summary>
        /// <param name="value1">数字1</param>
        /// <param name="value2">数字2</param>
        /// <returns></returns>
        public static CsqCommonFunction BitXOR(BQLValueItem value1, BQLValueItem value2)
        {
            value1.ValueDbType = DbType.Double;
            value2.ValueDbType = DbType.Double;
            CsqCommonFunction handle = new CsqCommonFunction(new BQLValueItem[] { value1, value2 }, DBMathFunction.BitXOR, DbType.Double);
            //handle.ValueDataType = DefaultType.DoubleType;
            return handle;
        }
        /// <summary>
        /// 求任意数为底的幂 
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="value2">几次方</param>
        /// <returns></returns>
        public static CsqCommonFunction Power(BQLValueItem value, BQLValueItem value2)
        {
            value.ValueDbType = DbType.Double;
            value2.ValueDbType = DbType.Int32;
            CsqCommonFunction handle = new CsqCommonFunction(new BQLValueItem[] { value, value2 }, DBMathFunction.DoPower, DbType.Double);
            //handle.ValueDataType = DefaultType.DoubleType;
            return handle;
        }

        /// <summary>
        /// 随机数
        /// </summary>
        /// <returns></returns>
        public static CsqCommonFunction Random()
        {

            CsqCommonFunction handle = new CsqCommonFunction(new BQLValueItem[] { }, DBMathFunction.DoRandom, DbType.Double);
            //handle.ValueDataType = DefaultType.DoubleType;
            return handle;
        }

        /// <summary>
        /// 四舍五入 
        /// </summary>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static CsqCommonFunction Round(BQLValueItem value)
        {
            value.ValueDbType = DbType.Double;
            CsqCommonFunction handle = new CsqCommonFunction(new BQLValueItem[] { value }, DBMathFunction.DoRound, DbType.Double);
            //handle.ValueDataType = DefaultType.DoubleType;
            return handle;
        }

        /// <summary>
        /// 取符号
        /// </summary>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static CsqCommonFunction Sign(BQLValueItem value)
        {
            value.ValueDbType = DbType.Double;
            CsqCommonFunction handle = new CsqCommonFunction(new BQLValueItem[] { value }, DBMathFunction.DoSign, DbType.Double);
            //handle.ValueDataType = DefaultType.DoubleType;
            return handle;
        }

        /// <summary>
        /// 正弦函数
        /// </summary>
        /// <param name="value">弧度</param>
        /// <returns></returns>
        public static CsqCommonFunction Sin(BQLValueItem value)
        {
            value.ValueDbType = DbType.Double;
            CsqCommonFunction handle = new CsqCommonFunction(new BQLValueItem[] { value }, DBMathFunction.DoSin, DbType.Double);
            //handle.ValueDataType = DefaultType.DoubleType;
            return handle;
        }

        /// <summary>
        /// 平方根
        /// </summary>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static CsqCommonFunction Sqrt(BQLValueItem value)
        {
            value.ValueDbType = DbType.Double;
            CsqCommonFunction handle = new CsqCommonFunction(new BQLValueItem[] { value }, DBMathFunction.DoSqrt, DbType.Double);
            //handle.ValueDataType = DefaultType.DoubleType;
            return handle;
        }
        /// <summary>
        /// 正切
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static CsqCommonFunction Tan(BQLValueItem value)
        {
            value.ValueDbType = DbType.Double;
            CsqCommonFunction handle = new CsqCommonFunction(new BQLValueItem[] { value }, DBMathFunction.DoTan, DbType.Double);
            //handle.ValueDataType = DefaultType.DoubleType;
            return handle;
        }

        //private static BQLMaths _BQLMaths = new BQLMaths();
        ///// <summary>
        /////数学函数
        ///// </summary>
        //protected static BQLMaths CMath
        //{
        //    get
        //    {
        //        return _BQLMaths;
        //    }
        //}
        #endregion

        #region 排序方式
        /// <summary>
        /// 产生顺序排序的项
        /// </summary>
        /// <param name="arg">字段</param>
        /// <returns></returns>
        public static BQLOrderByHandle ASC(BQLParamHandle arg)
        {
            BQLOrderByHandle orderHandle = new BQLOrderByHandle(arg, SortType.ASC);
            return orderHandle;
        }
        /// <summary>
        /// 产生倒叙排序的项
        /// </summary>
        /// <param name="arg">字段</param>
        /// <returns></returns>
        public static BQLOrderByHandle DESC(BQLParamHandle arg)
        {
            BQLOrderByHandle orderHandle = new BQLOrderByHandle(arg, SortType.DESC);
            return orderHandle;
        }

        ///// <summary>
        ///// BQL关键字
        ///// </summary>
        //protected static BQLKeyWords BQL
        //{
        //    get
        //    {
        //        return _BQL;
        //    }
        //}
        //private static BQLSorts _BQLsort = new BQLSorts();

        ///// <summary>
        ///// 排序方式
        ///// </summary>
        //protected static BQLSorts SORT
        //{
        //    get
        //    {
        //        return _BQLsort;
        //    }
        //}
        #endregion
    }
}
