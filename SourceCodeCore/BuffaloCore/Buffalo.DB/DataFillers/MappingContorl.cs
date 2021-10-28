using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

using Buffalo.DB.CommBase;
using Buffalo.DB.PropertyAttributes;
using System.Collections;
using Buffalo.DB.EntityInfos;
using System.Diagnostics;
using System.Reflection;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.DB.CommBase.DataAccessBases;
using Buffalo.DB.DbCommon;
using Buffalo.Kernel.FastReflection;
using Buffalo.DB.BQLCommon;
using Buffalo.DB.BQLCommon.BQLConditionCommon;
using Buffalo.DB.QueryConditions;
using Buffalo.DB.BQLCommon.BQLKeyWordCommon;
using Buffalo.DB.BQLCommon.BQLBaseFunction;
namespace Buffalo.DB.DataFillers
{
    /// <summary>
    /// 填充子项和父项的类
    /// </summary>
    public class MappingContorl
    {
        /// <summary>
        /// 获取当前调用填充的属性名
        /// </summary>
        /// <returns></returns>
        private static string GetPropertyName ()
        {
            StackTrace st = new StackTrace(true);
            for (int i = 1; i < st.FrameCount; i++)
            {
                StackFrame frame = st.GetFrame(i);//获取调用此函数的函数
                MethodBase method = frame.GetMethod();
                
                
                string mname=method.Name;
                if (mname.IndexOf("get_") == 0) 
                {
                    return mname.Substring(4);
                }
            }
            throw new Exception("找不到属性");
        }

        #region 填充子项
        /// <summary>
        /// 填充该列表的子类
        /// </summary>
        /// <param name="sender">发送者</param>
        public static void FillChildList(string propertyName, EntityBase sender)
        {

            Type senderType = CH.GetRealType(sender);//发送类的类型
            EntityInfoHandle senderHandle = EntityInfoManager.GetEntityHandle(senderType);//获取发送类的信息
            EntityMappingInfo mappingInfo = senderHandle.MappingInfo[propertyName];

            if (mappingInfo != null)
            {
                EntityPropertyInfo pkHandle = mappingInfo.SourceProperty;//获取实体主键属性句柄
                object lst=Activator.CreateInstance(mappingInfo.FieldType) ;
                mappingInfo.SetValue(sender, lst);
                object pk = pkHandle.GetValue(sender);
                EntityInfoHandle childHandle = mappingInfo.TargetProperty.BelongInfo;//获取子元素的信息
                FillChilds(pk, childHandle, mappingInfo,sender,propertyName);
            }

        }

        /// <summary>
        /// 填充字类列表
        /// </summary>
        /// <param name="pks">ID集合</param>
        /// <param name="childHandle">子元素的信息句柄</param>
        /// <param name="mappingInfo">映射信息</param>
        /// <param name="dicElement">元素</param>
        private static void FillChilds(object pk, EntityInfoHandle childHandle, EntityMappingInfo mappingInfo,
            EntityBase sender, string propertyName)
        {
            EntityInfoHandle childInfo = mappingInfo.TargetProperty.BelongInfo;
            DBInfo db = childInfo.DBInfo;
            DataBaseOperate oper = childHandle.DBInfo.DefaultOperate;
            BQLDbBase dao = new BQLDbBase(oper);
            ScopeList lstScope = new ScopeList();

            sender.OnFillChild(propertyName, lstScope);
            lstScope.AddEqual(mappingInfo.TargetProperty.PropertyName, pk);


            IDataReader reader = dao.QueryReader(lstScope, childInfo.EntityType);
            try
            {
                string fullName = mappingInfo.TargetProperty.BelongInfo.EntityType.FullName;
                Type childType = mappingInfo.TargetProperty.BelongInfo.EntityType;

                //获取子表的get列表
                List<EntityPropertyInfo> lstParamNames = CacheReader.GenerateCache(reader, childInfo);//创建一个缓存数值列表
                IList lst = (IList)mappingInfo.GetValue(sender);
                while (reader.Read())
                {
                    object obj = childInfo.CreateSelectProxyInstance();
                    //string fk = reader[mappingInfo.TargetProperty.ParamName].ToString();
                    CacheReader.FillObjectFromReader(reader, lstParamNames, obj, db);
                    lst.Add(obj);
                }

            }
            finally
            {
                reader.Close();
                oper.AutoClose();
            }
        }
        /// <summary>
        /// 获取指定对应字段名的属性句柄
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="pkName">字段名</param>
        /// <returns></returns>
        private static EntityPropertyInfo GetPkHandle(Type type, string pkName)
        {
            EntityInfoHandle classInfo = EntityInfoManager.GetEntityHandle(type);
            foreach (EntityPropertyInfo info in classInfo.PropertyInfo)
            {
                if (pkName == info.ParamName) 
                {
                    return info;
                }
            }
            return null;
        }

        
        #endregion

        #region 填充父项

        /// <summary>
        /// 填充该列表的子类
        /// </summary>
        /// <param name="sender">发送者</param>
        public static void FillParent(string propertyName, EntityBase sender)
        {
            //if (sender.GetEntityBaseInfo().HasPropertyChange(propertyName))
            //{
            //    return;
            //}
            if (sender.GetEntityBaseInfo()._dicFilledParent___.ContainsKey(propertyName))
            {
                return;
            }
            Type senderType =CH.GetRealType(sender);//发送者类型
            EntityInfoHandle senderInfo = EntityInfoManager.GetEntityHandle(senderType);//获取发送类的信息
            EntityMappingInfo mappingInfo = senderInfo.MappingInfo[propertyName];

            //if (mappingInfo.GetValue(sender) != null) 
            //{
            //    return;
            //}
            //IList baseList = sender.GetBaseList();//获取上一次查询的结果集合
            //if (baseList == null) 
            //{
            //    baseList = new ArrayList();
            //    baseList.Add(sender);
            //}
            //Dictionary<string, ArrayList> dicElement = new Dictionary<string, ArrayList>();
            
            if (mappingInfo != null)
            {

                EntityPropertyInfo senderHandle = mappingInfo.SourceProperty;//本类的主键属性句柄

                //IList pks = CollectFks(baseList, senderHandle, mappingInfo, dicElement, senderInfo.DBInfo);
                EntityInfoHandle fatherInfo =mappingInfo.TargetProperty.BelongInfo;
                object pk = senderHandle.GetValue(sender);
                FillParent(pk, fatherInfo, mappingInfo,propertyName,sender);
            }
            sender.GetEntityBaseInfo()._dicFilledParent___[propertyName] = true;
        }

        /// <summary>
        /// 填充字类列表
        /// </summary>
        /// <param name="pks">ID集合</param>
        /// <param name="fatherInfo">父表对应类的信息</param>
        /// <param name="dicElement">元素</param>
        /// <param name="mappingInfo">当前父表对应属性的映射信息</param>
        private static void FillParent(object pk, EntityInfoHandle fatherInfo,
             EntityMappingInfo mappingInfo, string propertyName, EntityBase sender)
        {
            DBInfo db = fatherInfo.DBInfo;
            DataBaseOperate oper = fatherInfo.DBInfo.DefaultOperate;
            BQLDbBase dao = new BQLDbBase(oper);
            ScopeList lstScope = new ScopeList();

            sender.OnFillParent(propertyName, lstScope);
            lstScope.AddEqual(mappingInfo.TargetProperty.PropertyName, pk);
            IDataReader reader = dao.QueryReader(lstScope, fatherInfo.EntityType);
            try
            {
                //获取子表的get列表
                List<EntityPropertyInfo> lstParamNames = CacheReader.GenerateCache(reader, fatherInfo);//创建一个缓存数值列表
                while (reader.Read())
                {
                    object newObj = fatherInfo.CreateSelectProxyInstance();
                    mappingInfo.SetValue(sender, newObj);
                    CacheReader.FillObjectFromReader(reader, lstParamNames, newObj, db);
                }
                sender.OnPropertyUpdated(mappingInfo.PropertyName);
            }
            finally
            {
                reader.Close();
                oper.AutoClose();
            }

        }

        

        ///// <summary>
        ///// 收集键列表，把字字段实例化
        ///// </summary>
        ///// <param name="curList">当前集合</param>
        ///// <param name="senderHandle">发动元素的信息</param>
        ///// <param name="mappingInfo">设置对应元素的的属性句柄</param>
        ///// <param name="dicElement">元素列表</param>
        ///// <returns></returns>
        //private static IList CollectFks(IList curList, EntityPropertyInfo senderHandle, 
        //    EntityMappingInfo mappingInfo, Dictionary<string, ArrayList> dicElement,DBInfo db)
        //{
        //    EntityInfoHandle classInfo = EntityInfoManager.GetEntityHandle(mappingInfo.FieldType);
        //    ArrayList ret = new ArrayList();
        //    if (senderHandle != null)
        //    {
        //        foreach (object obj in curList)
        //        {
        //            //实例化父表元素对应类，并赋到集合里边以作初值
        //            //object newObj = classInfo.CreateProxyInstance();
        //            //mappingInfo.SetValue(obj, newObj);

        //            //获取对应字段的值以返回做查询条件
        //            object curObj = senderHandle.GetValue(obj);
        //            if (curObj != null)
        //            {
        //                string key = curObj.ToString();
        //                ArrayList lst = null;
        //                if (!dicElement.TryGetValue(key, out lst))
        //                {
        //                    lst = new ArrayList();
        //                    //把当前元素以主键作为标识放在Dic里边
        //                    dicElement.Add(curObj.ToString(), lst);
        //                    ret.Add(curObj);
        //                }

        //                lst.Add(obj);
        //            }
        //        }
        //    }
        //    return ret;

        //}


        #endregion
    }
}
