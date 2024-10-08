﻿using System;
using Buffalo.DB.QueryConditions;
using System.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Buffalo.DB.CommBase.DataAccessBases
{

    /// <summary>
    /// 数据层接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IViewDataAccess<T>
     where T :Buffalo.DB.CommBase.EntityBase, new()
    {

               
        
        /// <summary>
        /// 根据主键查找实体
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        T GetEntityById(object id, bool isSearchByCache = false);
        /// <summary>
        /// 根据主键查找实体
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        Task<T> GetEntityByIdAsync(object id, bool isSearchByCache = false);

        /// <summary>
        /// 查找
        /// </summary>
        /// <param name="lstScope">范围集合</param>
        /// <returns></returns>
        DataSet Select(ScopeList lstScope);
        /// <summary>
        /// 查找
        /// </summary>
        /// <param name="lstScope">范围集合</param>
        /// <returns></returns>
        Task<DataSet> SelectAsync(ScopeList lstScope);
        /// 查询集合
        /// </summary>
        /// <param name="scpoeList">范围查找的集合</param>
        /// <returns></returns>
        List<T> SelectList(ScopeList scpoeList);
        /// 查询集合
        /// </summary>
        /// <param name="scpoeList">范围查找的集合</param>
        /// <returns></returns>
        Task<List<T>> SelectListAsync(ScopeList scpoeList);
        /// <summary>
        /// 查询符合指定条件的记录条数
        /// </summary>
        /// <param name="sortList">排序条件</param>
        /// <param name="scpoeList">范围查找的集合</param>
        /// <returns></returns>
        long SelectCount(ScopeList scpoeList);
        /// <summary>
        /// 查询符合指定条件的记录条数
        /// </summary>
        /// <param name="sortList">排序条件</param>
        /// <param name="scpoeList">范围查找的集合</param>
        /// <returns></returns>
        Task<long> SelectCountAsync(ScopeList scpoeList);
        /// <summary>
        /// 查询符合指定条件的记录条数
        /// </summary>
        /// <param name="sortList">排序条件</param>
        /// <param name="scpoeList">范围查找的集合</param>
        /// <returns></returns>
        bool ExistsRecord(ScopeList scpoeList);
        /// <summary>
        /// 查询符合指定条件的记录条数
        /// </summary>
        /// <param name="sortList">排序条件</param>
        /// <param name="scpoeList">范围查找的集合</param>
        /// <returns></returns>
        Task<bool> ExistsRecordAsync(ScopeList scpoeList);
    }
}
