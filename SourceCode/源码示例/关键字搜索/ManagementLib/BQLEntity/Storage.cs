using System;
using System.Data;
using System.Configuration;
using Buffalo.DB.EntityInfos;
using Buffalo.DB.BQLCommon;
using Buffalo.DB.BQLCommon.BQLConditionCommon;
using Buffalo.DB.PropertyAttributes;
namespace ManagementLib.BQLEntity
{

    public partial class Management
    {
        private static Management_Storage _Storage = new Management_Storage();
    
        public static Management_Storage Storage
        {
            get
            {
                return _Storage;
            }
        }
    }

    /// <summary>
    ///  
    /// </summary>
    public partial class Management_Storage : BQLEntityTableHandle
    {
        private BQLEntityParamHandle _id = null;
        /// <summary>
        /// 主键ID，自动增长
        /// </summary>
        public BQLEntityParamHandle Id
        {
            get
            {
                return _id;
            }
         }
        private BQLEntityParamHandle _storageName = null;
        /// <summary>
        /// 仓库名称
        /// </summary>
        public BQLEntityParamHandle StorageName
        {
            get
            {
                return _storageName;
            }
         }



		/// <summary>
        /// 初始化本类的信息
        /// </summary>
        /// <param name="parent">父表信息</param>
        /// <param name="propertyName">属性名</param>
        public Management_Storage(BQLEntityTableHandle parent,string propertyName) 
        :this(typeof(ManagementLib.Storage),parent,propertyName)
        {
			
        }
        /// <summary>
        /// 初始化本类的信息
        /// </summary>
        /// <param name="parent">父表信息</param>
        /// <param name="propertyName">属性名</param>
        public Management_Storage(Type entityType,BQLEntityTableHandle parent,string propertyName) 
        :base(entityType,parent,propertyName)
        {
            _id=CreateProperty("Id");
            _storageName=CreateProperty("StorageName");

        }
        
        /// <summary>
        /// 初始化本类的信息
        /// </summary>
        public Management_Storage() 
            :this(null,null)
        {
        }
    }
}
