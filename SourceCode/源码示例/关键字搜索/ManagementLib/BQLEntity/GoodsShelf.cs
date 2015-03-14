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
        private static Management_GoodsShelf _GoodsShelf = new Management_GoodsShelf();
    
        public static Management_GoodsShelf GoodsShelf
        {
            get
            {
                return _GoodsShelf;
            }
        }
    }

    /// <summary>
    ///  
    /// </summary>
    public partial class Management_GoodsShelf : BQLEntityTableHandle
    {
        private BQLEntityParamHandle _id = null;
        /// <summary>
        /// 主键ID,自动增长
        /// </summary>
        public BQLEntityParamHandle Id
        {
            get
            {
                return _id;
            }
         }
        private BQLEntityParamHandle _goodShelf = null;
        /// <summary>
        /// 货架号码
        /// </summary>
        public BQLEntityParamHandle GoodShelf
        {
            get
            {
                return _goodShelf;
            }
         }
        private BQLEntityParamHandle _storageId = null;
        /// <summary>
        /// 所属仓库
        /// </summary>
        public BQLEntityParamHandle StorageId
        {
            get
            {
                return _storageId;
            }
         }

        /// <summary>
        /// 
        /// </summary>
        public Management_Storage BelongStorage_StorageId_Id
        {
            get
            {
               return new Management_Storage(this,"BelongStorage_StorageId_Id");
            }
         }


		/// <summary>
        /// 初始化本类的信息
        /// </summary>
        /// <param name="parent">父表信息</param>
        /// <param name="propertyName">属性名</param>
        public Management_GoodsShelf(BQLEntityTableHandle parent,string propertyName) 
        :this(typeof(ManagementLib.GoodsShelf),parent,propertyName)
        {
			
        }
        /// <summary>
        /// 初始化本类的信息
        /// </summary>
        /// <param name="parent">父表信息</param>
        /// <param name="propertyName">属性名</param>
        public Management_GoodsShelf(Type entityType,BQLEntityTableHandle parent,string propertyName) 
        :base(entityType,parent,propertyName)
        {
            _id=CreateProperty("Id");
            _goodShelf=CreateProperty("GoodShelf");
            _storageId=CreateProperty("StorageId");

        }
        
        /// <summary>
        /// 初始化本类的信息
        /// </summary>
        public Management_GoodsShelf() 
            :this(null,null)
        {
        }
    }
}
