using Buffalo.DB.DbCommon;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffalo.DB.CacheManager.CacheCollection
{
    /// <summary>
    /// List方式的缓存项
    /// </summary>
    public interface ICacheList
    {
        /// <summary>
        /// 增加到列表
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="index">索引(0为增加到头部，-1为增加到尾部)</param>
        /// <param name="value">值</param>
        /// <param name="setType">设置值方式</param>
        /// <returns></returns>
        long AddValue(long index, object value, SetValueType setType);

        /// <summary>
        /// 增加到列表
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="index">索引(0为增加到头部，-1为增加到尾部)</param>
        /// <param name="values">值</param>
        /// <param name="setType">设置值方式</param>
        /// <returns></returns>
        long AddRangValue( long index,  IEnumerable values, SetValueType setType);

        /// <summary>
        /// 获取值
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="index">值位置</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        E GetValue<E>(long index, E defaultValue);

        /// <summary>
        /// 获取集合长度
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        long GetLength();
        

        /// <summary>
        /// 移除值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="count">要移除几个，0则为全部移除</param>
        /// <returns></returns>
        long RemoveValue(object value, long count);



        /// <summary>
        /// 获取集合所有值
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="key">键</param>
        /// <param name="start">起始位置(默认0)</param>
        /// <param name="end">结束位置(-1则为读到末尾)</param>
        /// <returns></returns>
        List<E> AllValues<E>( long start, long end);

        /// <summary>
        /// 加入到这个值的后边
        /// </summary>
        /// <param name="pivot"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        long InsertAfter(object pivot, object value);
        /// <summary>
        /// 加入到这个值的前边
        /// </summary>
        /// <param name="pivot"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        long InsertBefore( object pivot, object value);
        /// <summary>
        /// 移除一个元素并返回
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="isPopEnd">是否从尾部弹出</param>
        /// <param name="defaultValue">没元素时候的默认值</param>
        /// <returns></returns>
        E PopValue<E>(bool isPopEnd, E defaultValue);
       
    }
}
