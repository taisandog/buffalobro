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
    /// BQL�ؼ���
    /// </summary>
    public class BQL
    {
        /// <summary>
        /// Select��
        /// </summary>
        /// <param name="args">Ҫ������ֶ�</param>
        /// <returns></returns>
        public static KeyWordSelectItem Select(params BQLParamHandle[] args)
        {
            KeyWordSelectItem selectItem = new KeyWordSelectItem(args, null);
            return selectItem;
        }
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="arg">Ҫ������ı�</param>
        /// <returns></returns>
        public static KeyWordInserItem InsertInto(BQLTableHandle arg)
        {
            KeyWordInserItem item = new KeyWordInserItem(arg, null);
            return item;
        }

        /// <summary>
        /// ������
        /// </summary>
        /// <param name="tableName">����</param>
        /// <returns></returns>
        public static KeyWordCreateTableItem CreateTable(string tableName) 
        {
            KeyWordCreateTableItem item = new KeyWordCreateTableItem(tableName, null);
            return item;
        }
        /// <summary>
        /// �޸ı�
        /// </summary>
        /// <param name="tableName">����</param>
        /// <returns></returns>
        public static KeyWordAlterTableItem AlterTable(string tableName) 
        {
            KeyWordAlterTableItem item = new KeyWordAlterTableItem(tableName, null);
            return item;
        }
        
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="arg">Ҫ�����µı�</param>
        /// <returns></returns>
        public static KeyWordUpdateItem Update(BQLTableHandle arg)
        {
            KeyWordUpdateItem item = new KeyWordUpdateItem(arg, null);
            return item;
        }
        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="arg">Ҫ��ɾ�����ݵı�</param>
        /// <returns></returns>
        public static KeyWordDeleteItem DeleteFrom(BQLTableHandle arg)
        {
            KeyWordDeleteItem item = new KeyWordDeleteItem(arg, null);
            return item;
        }

        /// <summary>
        /// Case
        /// </summary>
        /// <param name="value">Ҫѡ���ֵ����ʽ</param>
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


        #region ����
        /// <summary>
        /// һ��������
        /// </summary>
        /// <param name="table">��</param>
        /// <param name="asName">����</param>
        /// <returns></returns>
        public static BQLAliasHandle TableAs(BQLEntityTableHandle table, string asName)
        {
            BQLAliasHandle item = new BQLAliasHandle(table, asName);
            return item;
        }
        /// <summary>
        /// ������ѯ
        /// </summary>
        /// <param name="query">��ѯ</param>
        /// <param name="asName">����</param>
        /// <returns></returns>
        public static BQLAliasHandle TableAs(BQLQuery query, string asName)
        {
            BQLAliasHandle item = new BQLAliasHandle(query, asName);
            return item;
        }
        /// <summary>
        /// �����ֶ�
        /// </summary>
        /// <param name="param">�ֶ�</param>
        /// <param name="asName">����</param>
        /// <returns></returns>
        public static BQLAliasParamHandle ParamAs(BQLParamHandle param, string asName)
        {
            BQLAliasParamHandle item = new BQLAliasParamHandle(param, asName);
            return item;
        }
        private static AliasTableCollection tabColl = new AliasTableCollection();
        /// <summary>
        /// ������
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
        /// ���ֶ���תΪParam����
        /// </summary>
        public static BQLOtherParamHandle ToParam(string paramName)
        {
            return new BQLOtherParamHandle(null, paramName);
        }
        //private static BQLAlias _alias = new BQLAlias();
        ///// <summary>
        ///// ��������
        ///// </summary>
        //protected static BQLAlias ALIAS
        //{
        //    get
        //    {
        //        return _alias;
        //    }
        //}
        #endregion

        #region ���ú���

        //ICommonFunction common = DbAdapterLoader.Common;
        /// <summary>
        /// �ж��Ƿ�Ϊ��
        /// </summary>
        /// <param name="source">Դֵ</param>
        /// <param name="nullValue">���Ϊ�յĻ������ֵ</param>
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
        /// �ж��Ƿ�Ϊ��
        /// </summary>
        /// <param name="source">Դֵ</param>
        /// <returns></returns>
        public static CsqCommonFunction Length(BQLValueItem source)
        {

            CsqCommonFunction handle = new CsqCommonFunction(new BQLValueItem[] { source }, DBCommonFunction.Len, DbType.Int32);

            //handle.ValueDataType = DefaultType.IntType;

            return handle;
        }

        /// <summary>
        /// distinct�����ظ��ֶ�
        /// </summary>
        /// <param name="param">�ֶ�</param>
        /// <returns></returns>
        public static CsqCommonFunction Distinct(params BQLParamHandle[] cparams)
        {
            CsqCommonFunction handle = new CsqCommonFunction(cparams, DBCommonFunction.Distinct, DbType.Object);
            return handle;
        }
        /// <summary>
        /// �����Զ��庯��
        /// </summary>
        /// <param name="functionName">������</param>
        /// <param name="returnType">����ֵ����</param>
        /// <param name="values">����ֵ</param>
        /// <returns></returns>
        public static BQLCustomizeFunction Call(string functionName,DbType returnType, params BQLValueItem[] values)
        {
            BQLCustomizeFunction handle = new BQLCustomizeFunction(functionName, returnType, values);
            return handle;
        }
        /// <summary>
        /// �����Զ��庯��
        /// </summary>
        /// <param name="functionName">������</param>
        /// <param name="returnType">����ֵ����</param>
        /// <param name="values">����ֵ</param>
        /// <returns></returns>
        public static BQLCustomizeFunction Call(string functionName)
        {
            BQLCustomizeFunction handle = new BQLCustomizeFunction(functionName, DbType.Object, null);
            return handle;
        }

        /// <summary>
        /// �����Զ��庯��
        /// </summary>
        /// <param name="handle">����</param>
        /// <param name="returnType">����ֵ����</param>
        /// <param name="values">����ֵ</param>
        /// <returns></returns>
        public static BQLCustomizeFunction Call(DelCustomizeFunction handle, DbType returnType, params BQLValueItem[] values)
        {
            BQLCustomizeFunction chandle = new BQLCustomizeFunction(handle, returnType, values);
            return chandle;
        }
        /// <summary>
        /// �����Զ��庯��
        /// </summary>
        /// <param name="handle">����</param>
        /// <returns></returns>
        public static BQLCustomizeFunction Call(DelCustomizeFunction handle)
        {
            BQLCustomizeFunction chandle = new BQLCustomizeFunction(handle, DbType.Object, null);
            return chandle;
        }

        #endregion

        #region �ۺϺ���
        //IAggregateFunctions agf = DbAdapterLoader.Aggregate;
        /// <summary>
        /// �ܼ�
        /// </summary>
        /// <param name="functionHandle">����</param>
        /// <param name="param">�ֶ�</param>
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
        /// <param name="functionHandle">����</param>
        /// <param name="param">�ֶ�</param>
        /// <returns></returns>
        public static BQLAggregateFunction StdDev(BQLParamHandle param)
        {
            BQLAggregateFunction item = new BQLAggregateFunction(DBAggregateFunction.DoStdDev, param);
            item.ValueDbType = DbType.Double;
            return item;
        }

        /// <summary>
        /// ��С
        /// </summary>
        /// <param name="functionHandle">����</param>
        /// <param name="param">�ֶ�</param>
        /// <returns></returns>
        public static BQLAggregateFunction Min(BQLParamHandle param)
        {
            BQLAggregateFunction item = new BQLAggregateFunction(DBAggregateFunction.DoMin, param);
            return item;
        }
        /// <summary>
        /// ���
        /// </summary>
        /// <param name="functionHandle">����</param>
        /// <param name="param">�ֶ�</param>
        /// <returns></returns>
        public static BQLAggregateFunction Max(BQLParamHandle param)
        {
            BQLAggregateFunction item = new BQLAggregateFunction(DBAggregateFunction.DoMax, param);
            return item;
        }
        /// <summary>
        /// ������
        /// </summary>
        /// <param name="functionHandle">����</param>
        /// <param name="param">�ֶ�</param>
        /// <returns></returns>
        public static BQLAggregateFunction Count(BQLParamHandle param)
        {
            BQLAggregateFunction item = new BQLAggregateFunction(DBAggregateFunction.DoCount, param);
            item.ValueDbType = DbType.Int32;
            return item;
        }
        /// <summary>
        /// ������
        /// </summary>
        /// <param name="functionHandle">����</param>
        /// <param name="param">�ֶ�</param>
        /// <returns></returns>
        public static BQLAggregateFunction Count()
        {

            return Count(null);
        }
        /// <summary>
        /// ƽ��ֵ
        /// </summary>
        /// <param name="functionHandle">����</param>
        /// <param name="param">�ֶ�</param>
        /// <returns></returns>
        public static BQLAggregateFunction Avg(BQLParamHandle param)
        {
            BQLAggregateFunction item = new BQLAggregateFunction(DBAggregateFunction.DoAvg, param);
            item.ValueDbType = DbType.Double;
            return item;
        }
        


        #endregion

        #region ��ѧ����
        /// <summary>
        /// ��ǰʱ��
        /// </summary>
        /// <param name="dbType">ʱ������</param>
        /// <param name="isUTC">�Ƿ��������ʱ��</param>
        /// <returns></returns>
        public static BQLNowDateHandle NowDate(DbType dbType,bool isUTC)
        {
            BQLNowDateHandle handle = new BQLNowDateHandle(dbType,isUTC);
            
            //handle.ValueDbType = DbType.Double;
            return handle;
        }
        /// <summary>
        /// ��ǰʱ��
        /// </summary>
        /// <returns></returns>
        public static BQLNowDateHandle NowDate()
        {
            return NowDate(DbType.DateTime,false);
        }
        /// <summary>
        /// ��ǰʱ���
        /// </summary>
        /// <returns></returns>
        public static BQLTimeStampHandle NowTimeStamp()
        {
            BQLTimeStampHandle handle = new BQLTimeStampHandle();

            //handle.ValueDbType = DbType.Double;
            return handle;
        }
        /// <summary>
        /// ����ֵ
        /// </summary>
        /// <param name="value">�����ֵ</param>
        /// <returns></returns>
        public static CsqCommonFunction Abs(BQLValueItem value)
        {
            value.ValueDbType = DbType.Double;
            CsqCommonFunction handle = new CsqCommonFunction(new BQLValueItem[] { value }, DBMathFunction.DoAbs, DbType.Double);
            //handle.ValueDbType = DbType.Double;
            return handle;
        }
        /// <summary>
        /// �����Һ���
        /// </summary>
        /// <param name="value">����</param>
        /// <returns></returns>
        public static CsqCommonFunction Acos(BQLValueItem value)
        {
            value.ValueDbType = DbType.Double;
            CsqCommonFunction handle = new CsqCommonFunction(new BQLValueItem[] { value }, DBMathFunction.DoAcos, DbType.Double);
            //handle.ValueDbType = DbType.Double;
            return handle;
        }

        /// <summary>
        /// �����Һ���
        /// </summary>
        /// <param name="value">����</param>
        /// <returns></returns>
        public static CsqCommonFunction Asin(BQLValueItem value)
        {
            value.ValueDbType = DbType.Double;
            CsqCommonFunction handle = new CsqCommonFunction(new BQLValueItem[] { value }, DBMathFunction.DoAsin, DbType.Double);
            //handle.ValueDbType = DbType.Double;
            return handle;
        }

        /// <summary>
        /// ȡģ����
        /// </summary>
        /// <param name="value">����</param>
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
        /// ������
        /// </summary>
        /// <param name="value">����</param>
        /// <returns></returns>
        public static CsqCommonFunction Atan(BQLValueItem value)
        {
            value.ValueDbType = DbType.Double;
            CsqCommonFunction handle = new CsqCommonFunction(new BQLValueItem[] { value }, DBMathFunction.DoAtan, DbType.Double);
            //handle.ValueDbType = DbType.Double;
            return handle;
        }

        /// <summary>
        /// ������������ x �� y �ķ�����ֵ
        /// </summary>
        /// <param name="y">����� y ����</param>
        /// <param name="x">����� x ����</param>
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
        /// ���ش��ڻ���ڴ���ֵ������
        /// </summary>
        /// <param name="value">ֵ</param>
        /// <returns></returns>
        public static CsqCommonFunction Ceil(BQLValueItem value)
        {
            value.ValueDbType = DbType.Double;
            CsqCommonFunction handle = new CsqCommonFunction(new BQLValueItem[] { value }, DBMathFunction.DoCeil, DbType.Int32);
            //handle.ValueDataType = DefaultType.IntType;
            return handle;
        }
        /// <summary>
        /// ����
        /// </summary>
        /// <param name="value">����</param>
        /// <returns></returns>
        public static CsqCommonFunction Cos(BQLValueItem value)
        {
            value.ValueDbType = DbType.Double;
            CsqCommonFunction handle = new CsqCommonFunction(new BQLValueItem[] { value }, DBMathFunction.DoCos, DbType.Double);
            //handle.ValueDataType = DefaultType.DoubleType;
            return handle;
        }
        /// <summary>
        /// ����ֵ��eΪ�׵��� 
        /// </summary>
        /// <param name="values">��ֵ</param>
        /// <returns></returns>
        public static CsqCommonFunction DoExp(BQLValueItem value)
        {
            value.ValueDbType = DbType.Double;
            CsqCommonFunction handle = new CsqCommonFunction(new BQLValueItem[] { value }, DBMathFunction.DoExp, DbType.Double);
            //handle.ValueDataType = DefaultType.DoubleType;
            return handle;
        }

        /// <summary>
        /// ����С�ڻ���ڴ���ֵ������
        /// </summary>
        /// <param name="value">��ֵ</param>
        /// <returns></returns>
        public static CsqCommonFunction DoFloor(BQLValueItem value)
        {
            value.ValueDbType = DbType.Double;
            CsqCommonFunction handle = new CsqCommonFunction(new BQLValueItem[] { value }, DBMathFunction.DoFloor, DbType.Int32);
            //handle.ValueDataType = DefaultType.IntType;
            return handle;
        }

        /// <summary>
        /// ȡeΪ�׵Ķ��� 
        /// </summary>
        /// <param name="values">��ֵ</param>
        /// <returns></returns>
        public static CsqCommonFunction DoLn(BQLValueItem value)
        {
            value.ValueDbType = DbType.Double;
            CsqCommonFunction handle = new CsqCommonFunction(new BQLValueItem[] { value }, DBMathFunction.DoLn, DbType.Double);
            //handle.ValueDataType = DefaultType.DoubleType;
            return handle;
        }

        /// <summary>
        /// ��10Ϊ�����Ķ��� 
        /// </summary>
        /// <param name="values">��ֵ</param>
        /// <returns></returns>
        public static CsqCommonFunction DoLog10(BQLValueItem value)
        {
            value.ValueDbType = DbType.Double;
            CsqCommonFunction handle = new CsqCommonFunction(new BQLValueItem[] { value }, DBMathFunction.DoLog10, DbType.Double);
            //handle.ValueDataType = DefaultType.DoubleType;
            return handle;
        }

        /// <summary>
        /// ��ģ����
        /// </summary>
        /// <param name="value1">Ҫ�������</param>
        /// <param name="value2">������</param>
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
        /// ��λ������
        /// </summary>
        /// <param name="value1">����1</param>
        /// <param name="value2">����2</param>
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
        /// ������
        /// </summary>
        /// <param name="value1">����1</param>
        /// <param name="value2">����2</param>
        /// <returns></returns>
        public static CsqCommonFunction BitNot(BQLValueItem value)
        {
            value.ValueDbType = DbType.Double;
            CsqCommonFunction handle = new CsqCommonFunction(new BQLValueItem[] { value }, DBMathFunction.BitNot, DbType.Double);
            //handle.ValueDataType = DefaultType.DoubleType;
            return handle;
        }
        /// <summary>
        /// ��λ������
        /// </summary>
        /// <param name="value1">����1</param>
        /// <param name="value2">����2</param>
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
        /// ��λ�������
        /// </summary>
        /// <param name="value1">����1</param>
        /// <param name="value2">����2</param>
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
        /// ��������Ϊ�׵��� 
        /// </summary>
        /// <param name="value">��ֵ</param>
        /// <param name="value2">���η�</param>
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
        /// �����
        /// </summary>
        /// <returns></returns>
        public static CsqCommonFunction Random()
        {

            CsqCommonFunction handle = new CsqCommonFunction(new BQLValueItem[] { }, DBMathFunction.DoRandom, DbType.Double);
            //handle.ValueDataType = DefaultType.DoubleType;
            return handle;
        }

        /// <summary>
        /// �������� 
        /// </summary>
        /// <param name="value">ֵ</param>
        /// <returns></returns>
        public static CsqCommonFunction Round(BQLValueItem value)
        {
            value.ValueDbType = DbType.Double;
            CsqCommonFunction handle = new CsqCommonFunction(new BQLValueItem[] { value }, DBMathFunction.DoRound, DbType.Double);
            //handle.ValueDataType = DefaultType.DoubleType;
            return handle;
        }

        /// <summary>
        /// ȡ����
        /// </summary>
        /// <param name="value">ֵ</param>
        /// <returns></returns>
        public static CsqCommonFunction Sign(BQLValueItem value)
        {
            value.ValueDbType = DbType.Double;
            CsqCommonFunction handle = new CsqCommonFunction(new BQLValueItem[] { value }, DBMathFunction.DoSign, DbType.Double);
            //handle.ValueDataType = DefaultType.DoubleType;
            return handle;
        }

        /// <summary>
        /// ���Һ���
        /// </summary>
        /// <param name="value">����</param>
        /// <returns></returns>
        public static CsqCommonFunction Sin(BQLValueItem value)
        {
            value.ValueDbType = DbType.Double;
            CsqCommonFunction handle = new CsqCommonFunction(new BQLValueItem[] { value }, DBMathFunction.DoSin, DbType.Double);
            //handle.ValueDataType = DefaultType.DoubleType;
            return handle;
        }

        /// <summary>
        /// ƽ����
        /// </summary>
        /// <param name="value">ֵ</param>
        /// <returns></returns>
        public static CsqCommonFunction Sqrt(BQLValueItem value)
        {
            value.ValueDbType = DbType.Double;
            CsqCommonFunction handle = new CsqCommonFunction(new BQLValueItem[] { value }, DBMathFunction.DoSqrt, DbType.Double);
            //handle.ValueDataType = DefaultType.DoubleType;
            return handle;
        }
        /// <summary>
        /// ����
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
        /////��ѧ����
        ///// </summary>
        //protected static BQLMaths CMath
        //{
        //    get
        //    {
        //        return _BQLMaths;
        //    }
        //}
        #endregion

        #region ����ʽ
        /// <summary>
        /// ����˳���������
        /// </summary>
        /// <param name="arg">�ֶ�</param>
        /// <returns></returns>
        public static BQLOrderByHandle ASC(BQLParamHandle arg)
        {
            BQLOrderByHandle orderHandle = new BQLOrderByHandle(arg, SortType.ASC);
            return orderHandle;
        }
        /// <summary>
        /// ���������������
        /// </summary>
        /// <param name="arg">�ֶ�</param>
        /// <returns></returns>
        public static BQLOrderByHandle DESC(BQLParamHandle arg)
        {
            BQLOrderByHandle orderHandle = new BQLOrderByHandle(arg, SortType.DESC);
            return orderHandle;
        }

        ///// <summary>
        ///// BQL�ؼ���
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
        ///// ����ʽ
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
