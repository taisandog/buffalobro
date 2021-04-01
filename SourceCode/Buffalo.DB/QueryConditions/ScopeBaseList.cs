using Buffalo.DB.BQLCommon.BQLConditionCommon;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.Kernel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffalo.DB.QueryConditions
{
    /// <summary>
    /// 条件基集合语句
    /// </summary>
    public class ScopeBaseList : List<Scope>
    {
        private bool _hasInner = false;

        /// <summary>
        /// 判断是否有多表查询
        /// </summary>
        internal bool HasInner
        {
            get
            {
                return _hasInner;
            }
            
        }
        /// <summary>
        /// 添加新条件的范围
        /// </summary>
        /// <param name="lstScope">条件集合</param>
        /// <param name="conntype">连接类型</param>
        /// <returns>返回是否添加成功</returns>
        public bool AddScopeList(ScopeList lstScope, ConnectType conntype)
        {
            this.Add(new Scope(null, lstScope, null, ScopeType.Scope, conntype));
            return true;
        }

        /// <summary>
        /// 添加新条件的范围
        /// </summary>
        /// <param name="where">条件</param>
        /// <param name="conntype">连接类型</param>
        /// <returns>返回是否添加成功</returns>
        public bool Add(BQLCondition where, ConnectType conntype)
        {
            _hasInner = true;
            this.Add(new Scope(null, where, null, ScopeType.Condition, conntype));
            return true;
        }

        /// <summary>
        /// 判断是否多表
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        private bool IsInnerTable(BQLEntityParamHandle property)
        {
            return !CommonMethods.IsNull(property.BelongEntity.GetParentTable());
        }

        /// <summary>
        /// 添加新的范围
        /// </summary>
        /// <param name="property">属性</param>
        /// <param name="minValue">最小值</param>
        /// <param name="maxValue">最大值</param>
        /// <param name="conntype">连接类型</param>
        /// <returns>返回是否添加成功</returns>
        public bool AddBetween(BQLEntityParamHandle property, object minValue, object maxValue, ConnectType conntype)
        {
            if (IsInnerTable(property))
            {
                BQLCondition where = property.Between(minValue, maxValue);
                return Add(where, conntype);
            }

            return AddBetween(property.PInfo.PropertyName, minValue, maxValue, conntype);

        }

        /// <summary>
        /// 添加新的范围
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <param name="minValue">最小值</param>
        /// <param name="maxValue">最大值</param>
        /// <param name="conntype">连接类型</param>
        /// <returns>返回是否添加成功</returns>
        public bool AddBetween(string propertyName, object minValue, object maxValue, ConnectType conntype)
        {
            this.Add(new Scope(propertyName, minValue, maxValue, ScopeType.Between, conntype));
            return true;
        }
        /// <summary>
        /// 添加新的范围
        /// </summary>
        /// <param name="property">属性</param>
        /// <param name="value">值</param>
        /// <param name="conntype">连接类型</param>
        /// <returns>返回是否添加成功</returns>
        public bool AddContains(BQLEntityParamHandle property, object value, ConnectType conntype)
        {
            if (IsInnerTable(property))
            {
                BQLCondition where = property.Contains(value);
                return Add(where, conntype);
            }
            else
            {
                return AddContains(property, value, conntype);
            }
        }
        /// <summary>
        /// 添加新的范围
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <param name="value">值</param>
        /// <param name="conntype">连接类型</param>
        /// <returns>返回是否添加成功</returns>
        public bool AddContains(string propertyName, object value, ConnectType conntype)
        {
            this.Add(new Scope(propertyName, value, null, ScopeType.Contains, conntype));
            return true;
        }
        /// <summary>
        /// 添加新的大于条件
        /// </summary>
        /// <param name="property">属性名</param>
        /// <param name="value">值</param>
        /// <param name="conntype">连接类型</param>
        /// <returns>返回是否添加成功</returns>
        public bool AddMore(BQLEntityParamHandle property, object value, ConnectType conntype)
        {
            if (IsInnerTable(property))
            {
                BQLCondition where = property > value;
                return Add(where, conntype);
            }
            else
            {
                return AddMore(property.PInfo.PropertyName, value, conntype);
            }
        }
        /// <summary>
        /// 添加新的大于条件
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <param name="value">值</param>
        /// <param name="conntype">连接类型</param>
        /// <returns>返回是否添加成功</returns>
        public bool AddMore(string propertyName, object value, ConnectType conntype)
        {
            this.Add(new Scope(propertyName, value, null, ScopeType.More, conntype));
            return true;
        }


        /// <summary>
        /// 添加新的大于等于条件
        /// </summary>
        /// <param name="property">属性名</param>
        /// <param name="value">值</param>
        /// <param name="conntype">连接类型</param>
        /// <returns>返回是否添加成功</returns>
        public bool AddMoreThen(BQLEntityParamHandle property, object value, ConnectType conntype)
        {
            if (IsInnerTable(property))
            {
                BQLCondition where = property >= value;
                return Add(where, conntype);
            }
            else
            {
                return AddMoreThen(property.PInfo.PropertyName, value, conntype);
            }
        }

        /// <summary>
        /// 添加新的大于等于条件
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <param name="value">值</param>
        /// <param name="conntype">连接类型</param>
        /// <returns>返回是否添加成功</returns>
        public bool AddMoreThen(string propertyName, object value, ConnectType conntype)
        {
            this.Add(new Scope(propertyName, value, null, ScopeType.MoreThen, conntype));
            return true;
        }

        /// <summary>
        /// 添加新的小于等于条件
        /// </summary>
        /// <param name="property">属性名</param>
        /// <param name="value">值</param>
        /// <param name="conntype">连接类型</param>
        /// <returns>返回是否添加成功</returns>
        public bool AddLessThen(BQLEntityParamHandle property, object value, ConnectType conntype)
        {
            if (IsInnerTable(property))
            {
                BQLCondition where = property <= value;
                return Add(where, conntype);
            }
            else
            {
                return AddLessThen(property.PInfo.PropertyName, value, conntype);
            }
        }
        /// <summary>
        /// 添加新的小于等于条件
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <param name="value">值</param>
        /// <param name="conntype">连接类型</param>
        /// <returns>返回是否添加成功</returns>
        public bool AddLessThen(string propertyName, object value, ConnectType conntype)
        {

            this.Add(new Scope(propertyName, value, null, ScopeType.LessThen, conntype));
            return true;
        }
        /// <summary>
        /// 添加新的小于条件
        /// </summary>
        /// <param name="property">属性名</param>
        /// <param name="value">值</param>
        /// <param name="conntype">连接类型</param>
        /// <returns>返回是否添加成功</returns>
        public bool AddLess(BQLEntityParamHandle property, object value, ConnectType conntype)
        {
            if (IsInnerTable(property))
            {
                BQLCondition where = property < value;
                return Add(where, conntype);
            }
            else
            {
                return AddLess(property.PInfo.PropertyName, value, conntype);
            }
        }

        /// <summary>
        /// 添加新的小于条件
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <param name="value">值</param>
        /// <param name="conntype">连接类型</param>
        /// <returns>返回是否添加成功</returns>
        public bool AddLess(string propertyName, object value, ConnectType conntype)
        {

            this.Add(new Scope(propertyName, value, null, ScopeType.Less, conntype));
            return true;
        }
        /// <summary>
        /// 添加新的In条件(如果集合为空，则返回1=2)
        /// </summary>
        /// <param name="property">属性名</param>
        /// <param name="valuesCollection">值集合</param>
        /// <param name="conntype">连接类型</param>
        /// <returns>返回是否添加成功</returns>
        public bool AddIn(BQLEntityParamHandle property, IEnumerable valuesCollection, ConnectType conntype)
        {
            if (IsInnerTable(property))
            {

                BQLCondition where = property.In(valuesCollection);
                return Add(where, conntype);
            }
            else
            {
                return AddIn(property.PInfo.PropertyName, valuesCollection, conntype);
            }
        }

        /// <summary>
        /// 添加新的In条件(如果集合为空，则返回1=2)
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <param name="valuesCollection">值集合</param>
        /// <param name="conntype">连接类型</param>
        /// <returns>返回是否添加成功</returns>
        public bool AddIn(string propertyName, IEnumerable valuesCollection, ConnectType conntype)
        {

            this.Add(new Scope(propertyName, valuesCollection, null, ScopeType.IN, conntype));
            return true;
        }

        /// <summary>
        /// 添加新的NotIn条件(如果集合为空，则返回1=1)
        /// </summary>
        /// <param name="property">属性名</param>
        /// <param name="valuesCollection">值集合</param>
        /// <param name="conntype">连接类型</param>
        /// <returns>返回是否添加成功</returns>
        public bool AddNotIn(BQLEntityParamHandle property, IEnumerable valuesCollection, ConnectType conntype)
        {
            if (IsInnerTable(property))
            {
                BQLCondition where = property.NotIn(valuesCollection);
                return Add(where, conntype);
            }
            else
            {
                return AddNotIn(property.PInfo.PropertyName, valuesCollection, conntype);
            }
        }

        /// <summary>
        /// 添加新的NotIn条件(如果集合为空，则返回1=1)
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <param name="valuesCollection">值集合</param>
        /// <param name="conntype">连接类型</param>
        /// <returns>返回是否添加成功</returns>
        public bool AddNotIn(string propertyName, IEnumerable valuesCollection, ConnectType conntype)
        {

            this.Add(new Scope(propertyName, valuesCollection, null, ScopeType.NotIn, conntype));
            return true;
        }

        /// <summary>
        /// 添加新的不等于条件
        /// </summary>
        /// <param name="property">属性名</param>
        /// <param name="value">值</param>
        /// <param name="conntype">连接类型</param>
        /// <returns>返回是否添加成功</returns>
        public bool AddNotEqual(BQLEntityParamHandle property, object value, ConnectType conntype)
        {
            if (IsInnerTable(property))
            {
                BQLCondition where = property != value;
                return Add(where, conntype);
            }
            else
            {
                return AddNotEqual(property.PInfo.PropertyName, value, conntype);
            }
        }

        /// <summary>
        /// 添加新的不等于条件
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <param name="value">值</param>
        /// <param name="conntype">连接类型</param>
        /// <returns>返回是否添加成功</returns>
        public bool AddNotEqual(string propertyName, object value, ConnectType conntype)
        {

            this.Add(new Scope(propertyName, value, null, ScopeType.NotEqual, conntype));
            return true;
        }

        /// <summary>
        /// 添加新的等于条件
        /// </summary>
        /// <param name="property">属性名</param>
        /// <param name="value">值</param>
        /// <param name="conntype">连接类型</param>
        /// <returns>返回是否添加成功</returns>
        public bool AddEqual(BQLEntityParamHandle property, object value, ConnectType conntype)
        {
            if (IsInnerTable(property))
            {
                BQLCondition where = property == value;
                return Add(where, conntype);
            }
            else
            {
                return AddEqual(property.PInfo.PropertyName, value, conntype);
            }
        }

        /// <summary>
        /// 添加新的等于条件
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <param name="value">值</param>
        /// <param name="conntype">连接类型</param>
        /// <returns>返回是否添加成功</returns>
        public bool AddEqual(string propertyName, object value, ConnectType conntype)
        {

            this.Add(new Scope(propertyName, value, null, ScopeType.Equal, conntype));
            return true;
        }

        /// <summary>
        /// 添加新的Like条件
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <param name="value">值</param>
        /// <param name="conntype">连接类型</param>
        /// <returns>返回是否添加成功</returns>
        public bool AddLike(BQLEntityParamHandle property, object value, ConnectType conntype)
        {
            if (IsInnerTable(property))
            {
                BQLCondition where = property.Like(value);
                return Add(where, conntype);
            }
            else
            {
                return AddLike(property.PInfo.PropertyName, value, conntype);
            }
        }

        /// <summary>
        /// 添加新的Like条件
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <param name="value">值</param>
        /// <param name="conntype">连接类型</param>
        /// <returns>返回是否添加成功</returns>
        public bool AddLike(string propertyName, object value, ConnectType conntype)
        {

            this.Add(new Scope(propertyName, value, null, ScopeType.Like, conntype));
            return true;
        }


        /// <summary>
        /// 添加新的StarWith条件
        /// </summary>
        /// <param name="property">属性名</param>
        /// <param name="value">值</param>
        /// <param name="conntype">连接类型</param>
        /// <returns>返回是否添加成功</returns>
        public bool AddStarWith(BQLEntityParamHandle property, object value, ConnectType conntype)
        {
            if (IsInnerTable(property))
            {
                BQLCondition where = property.Like(value, BQLLikeType.StartWith);
                return Add(where, conntype);
            }
            else
            {
                return AddStarWith(property.PInfo.PropertyName, value, conntype);
            }
        }

        /// <summary>
        /// 添加新的StarWith条件
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <param name="value">值</param>
        /// <param name="conntype">连接类型</param>
        /// <returns>返回是否添加成功</returns>
        public bool AddStarWith(string propertyName, object value, ConnectType conntype)
        {

            this.Add(new Scope(propertyName, value, null, ScopeType.StarWith, conntype));
            return true;
        }

        /// <summary>
        /// 添加新的EndWith条件
        /// </summary>
        /// <param name="property">属性名</param>
        /// <param name="value">值</param>
        /// <param name="conntype">连接类型</param>
        /// <returns>返回是否添加成功</returns>
        public bool AddEndWith(BQLEntityParamHandle property, object value, ConnectType conntype)
        {
            if (IsInnerTable(property))
            {
                BQLCondition where = property.Like(value, BQLLikeType.EndWith);
                return Add(where, conntype);
            }
            else
            {
                return AddEndWith(property.PInfo.PropertyName, value, conntype);
            }
        }

        /// <summary>
        /// 添加新的EndWith条件
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <param name="value">值</param>
        /// <param name="conntype">连接类型</param>
        /// <returns>返回是否添加成功</returns>
        public bool AddEndWith(string propertyName, object value, ConnectType conntype)
        {

            this.Add(new Scope(propertyName, value, null, ScopeType.EndWith, conntype));
            return true;
        }
        /// <summary>
        /// 添加新的范围
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <param name="minValue">最小值</param>
        /// <param name="maxValue">最大值</param>
        /// <returns>返回是否添加成功</returns>
        public bool AddBetween(string propertyName, object minValue, object maxValue)
        {

            return AddBetween(propertyName, minValue, maxValue, ConnectType.And);
        }
        /// <summary>
        /// 添加新的范围
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <param name="value">值</param>
        /// <returns>返回是否添加成功</returns>
        public bool AddContains(string propertyName, object value)
        {
            this.Add(new Scope(propertyName, value, null, ScopeType.Contains, ConnectType.And));
            return true;
        }
        /// <summary>
        /// 添加新的大于条件
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <param name="value">值</param>
        /// <returns>返回是否添加成功</returns>
        public bool AddMore(string propertyName, object value)
        {
            return AddMore(propertyName, value, ConnectType.And); ;
        }

        /// <summary>
        /// 添加新的大于等于条件
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <param name="value">值</param>
        /// <returns>返回是否添加成功</returns>
        public bool AddMoreThen(string propertyName, object value)
        {
            return AddMoreThen(propertyName, value, ConnectType.And);
        }

        /// <summary>
        /// 添加新的小于等于条件
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <param name="value">值</param>
        /// <returns>返回是否添加成功</returns>
        public bool AddLessThen(string propertyName, object value)
        {
            return AddLessThen(propertyName, value, ConnectType.And);
        }

        /// <summary>
        /// 添加新的小于条件
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <param name="value">值</param>
        /// <returns>返回是否添加成功</returns>
        public bool AddLess(string propertyName, object value)
        {
            return AddLess(propertyName, value, ConnectType.And);
        }

        /// <summary>
        /// 添加新的In条件(如果集合为空，则返回1=2)
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <param name="valuesCollection">值集合</param>
        /// <returns>返回是否添加成功</returns>
        public bool AddIn(string propertyName, IEnumerable valuesCollection)
        {
            return AddIn(propertyName, valuesCollection, ConnectType.And);
        }
        /// <summary>
        /// 添加新的NotIn条件(如果集合为空，则返回1=1)
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <param name="valuesCollection">值集合</param>
        /// <returns>返回是否添加成功</returns>
        public bool AddNotIn(string propertyName, IEnumerable valuesCollection)
        {
            return AddNotIn(propertyName, valuesCollection, ConnectType.And);
        }
        /// <summary>
        /// 添加新的不等于条件
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <param name="value">值</param>
        /// <returns>返回是否添加成功</returns>
        public bool AddNotEqual(string propertyName, object value)
        {
            return AddNotEqual(propertyName, value, ConnectType.And);
        }

        /// <summary>
        /// 添加新的等于条件
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <param name="value">值</param>
        /// <returns>返回是否添加成功</returns>
        public bool AddEqual(string propertyName, object value)
        {
            return AddEqual(propertyName, value, ConnectType.And);
        }
        /// <summary>
        /// 添加新的等于条件
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <param name="value">值</param>
        /// <returns>返回是否添加成功</returns>
        public bool AddLike(string propertyName, object value)
        {
            return AddLike(propertyName, value, ConnectType.And);
        }



        /// <summary>
        /// 添加新的范围
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <param name="minValue">最小值</param>
        /// <param name="maxValue">最大值</param>
        /// <returns>返回是否添加成功</returns>
        public bool AddBetween(BQLEntityParamHandle property, object minValue, object maxValue)
        {

            return AddBetween(property, minValue, maxValue, ConnectType.And);
        }
        /// <summary>
        /// 添加新的范围
        /// </summary>
        /// <param name="property">属性</param>
        /// <param name="value">最小值</param>
        /// <param name="conntype">连接类型</param>
        /// <returns>返回是否添加成功</returns>
        public bool AddContains(BQLEntityParamHandle property, object value)
        {

            return AddContains(property, value, ConnectType.And);

        }
        /// <summary>
        /// 添加新的大于条件
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <param name="value">值</param>
        /// <returns>返回是否添加成功</returns>
        public bool AddMore(BQLEntityParamHandle property, object value)
        {
            return AddMore(property, value, ConnectType.And); ;
        }

        /// <summary>
        /// 添加新的大于等于条件
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <param name="value">值</param>
        /// <returns>返回是否添加成功</returns>
        public bool AddMoreThen(BQLEntityParamHandle property, object value)
        {
            return AddMoreThen(property, value, ConnectType.And);
        }

        /// <summary>
        /// 添加新的小于等于条件
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <param name="value">值</param>
        /// <returns>返回是否添加成功</returns>
        public bool AddLessThen(BQLEntityParamHandle property, object value)
        {
            return AddLessThen(property, value, ConnectType.And);
        }

        /// <summary>
        /// 添加新的小于条件
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <param name="value">值</param>
        /// <returns>返回是否添加成功</returns>
        public bool AddLess(BQLEntityParamHandle property, object value)
        {
            return AddLess(property, value, ConnectType.And);
        }

        /// <summary>
        /// 添加新的In条件(如果集合为空，则返回1=2)
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <param name="valuesCollection">值集合</param>
        /// <returns>返回是否添加成功</returns>
        public bool AddIn(BQLEntityParamHandle property, IEnumerable valuesCollection)
        {
            return AddIn(property, valuesCollection, ConnectType.And);
        }
        /// <summary>
        /// 添加新的NotIn条件(如果集合为空，则返回1=1)
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <param name="valuesCollection">值集合</param>
        /// <returns>返回是否添加成功</returns>
        public bool AddNotIn(BQLEntityParamHandle property, IEnumerable valuesCollection)
        {
            return AddNotIn(property, valuesCollection, ConnectType.And);
        }
        /// <summary>
        /// 添加新的不等于条件
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <param name="value">值</param>
        /// <returns>返回是否添加成功</returns>
        public bool AddNotEqual(BQLEntityParamHandle property, object value)
        {
            return AddNotEqual(property, value, ConnectType.And);
        }

        /// <summary>
        /// 添加新的等于条件
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <param name="value">值</param>
        /// <returns>返回是否添加成功</returns>
        public bool AddEqual(BQLEntityParamHandle property, object value)
        {
            return AddEqual(property, value, ConnectType.And);
        }
        /// <summary>
        /// 添加新的等于条件
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <param name="value">值</param>
        /// <returns>返回是否添加成功</returns>
        public bool AddLike(BQLEntityParamHandle property, object value)
        {
            return AddLike(property, value, ConnectType.And);
        }

        /// <summary>
        /// 添加新的StarWith条件
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <param name="value">值</param>
        /// <param name="conntype">连接类型</param>
        /// <returns>返回是否添加成功</returns>
        public bool AddStarWith(BQLEntityParamHandle property, object value)
        {
            return AddStarWith(property, value, ConnectType.And);
        }

        /// <summary>
        /// 添加新的StarWith条件
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <param name="value">值</param>
        /// <param name="conntype">连接类型</param>
        /// <returns>返回是否添加成功</returns>
        public bool AddStarWith(string propertyName, object value)
        {
            return AddStarWith(propertyName, value, ConnectType.And);
        }

        /// <summary>
        /// 添加新的EndWith条件
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <param name="value">值</param>
        /// <param name="conntype">连接类型</param>
        /// <returns>返回是否添加成功</returns>
        public bool AddEndWith(BQLEntityParamHandle property, object value)
        {
            return AddEndWith(property, value, ConnectType.And);

        }

        /// <summary>
        /// 添加新的EndWith条件
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <param name="value">值</param>
        /// <param name="conntype">连接类型</param>
        /// <returns>返回是否添加成功</returns>
        public bool AddEndWith(string propertyName, object value)
        {
            return AddEndWith(propertyName, value, ConnectType.And);
        }
        /// <summary>
        /// 添加新条件的范围
        /// </summary>
        /// <param name="lstScope">条件集合</param>
        /// <returns>返回是否添加成功</returns>
        public bool AddScopeList(ScopeList lstScope)
        {
            if (lstScope.HasInner)
            {
                _hasInner = lstScope.HasInner;
            }
            this.Add(new Scope(null, lstScope, null, ScopeType.Scope, ConnectType.And));
            return true;
        }

        /// <summary>
        /// 添加新条件的范围
        /// </summary>
        /// <param name="where">条件</param>
        /// <param name="conntype">连接类型</param>
        /// <returns>返回是否添加成功</returns>
        public bool Add(BQLCondition where)
        {
            this.Add(where, ConnectType.And);
            return true;
        }
    }
}
