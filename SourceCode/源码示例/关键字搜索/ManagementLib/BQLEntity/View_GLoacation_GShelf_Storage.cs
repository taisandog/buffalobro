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
        private static Management_View_GLoacation_GShelf_Storage _View_GLoacation_GShelf_Storage = new Management_View_GLoacation_GShelf_Storage();
    
        public static Management_View_GLoacation_GShelf_Storage View_GLoacation_GShelf_Storage
        {
            get
            {
                return _View_GLoacation_GShelf_Storage;
            }
        }
    }

    /// <summary>
    ///  
    /// </summary>
    public partial class Management_View_GLoacation_GShelf_Storage : BQLEntityTableHandle
    {
        private BQLEntityParamHandle _id = null;
        /// <summary>
        /// 
        /// </summary>
        public BQLEntityParamHandle Id
        {
            get
            {
                return _id;
            }
         }
        private BQLEntityParamHandle _locationCode = null;
        /// <summary>
        /// 
        /// </summary>
        public BQLEntityParamHandle LocationCode
        {
            get
            {
                return _locationCode;
            }
         }
        private BQLEntityParamHandle _boxNum = null;
        /// <summary>
        /// 
        /// </summary>
        public BQLEntityParamHandle BoxNum
        {
            get
            {
                return _boxNum;
            }
         }
        private BQLEntityParamHandle _goodsShelfId = null;
        /// <summary>
        /// 
        /// </summary>
        public BQLEntityParamHandle GoodsShelfId
        {
            get
            {
                return _goodsShelfId;
            }
         }
        private BQLEntityParamHandle _storageId = null;
        /// <summary>
        /// 
        /// </summary>
        public BQLEntityParamHandle StorageId
        {
            get
            {
                return _storageId;
            }
         }
        private BQLEntityParamHandle _remark = null;
        /// <summary>
        /// 
        /// </summary>
        public BQLEntityParamHandle Remark
        {
            get
            {
                return _remark;
            }
         }
        private BQLEntityParamHandle _storageName = null;
        /// <summary>
        /// 
        /// </summary>
        public BQLEntityParamHandle StorageName
        {
            get
            {
                return _storageName;
            }
         }
        private BQLEntityParamHandle _goodShelf = null;
        /// <summary>
        /// 
        /// </summary>
        public BQLEntityParamHandle GoodShelf
        {
            get
            {
                return _goodShelf;
            }
         }



		/// <summary>
        /// 初始化本类的信息
        /// </summary>
        /// <param name="parent">父表信息</param>
        /// <param name="propertyName">属性名</param>
        public Management_View_GLoacation_GShelf_Storage(BQLEntityTableHandle parent,string propertyName) 
        :this(typeof(ManagementLib.View_GLoacation_GShelf_Storage),parent,propertyName)
        {
			
        }
        /// <summary>
        /// 初始化本类的信息
        /// </summary>
        /// <param name="parent">父表信息</param>
        /// <param name="propertyName">属性名</param>
        public Management_View_GLoacation_GShelf_Storage(Type entityType,BQLEntityTableHandle parent,string propertyName) 
        :base(entityType,parent,propertyName)
        {
            _id=CreateProperty("Id");
            _locationCode=CreateProperty("LocationCode");
            _boxNum=CreateProperty("BoxNum");
            _goodsShelfId=CreateProperty("GoodsShelfId");
            _storageId=CreateProperty("StorageId");
            _remark=CreateProperty("Remark");
            _storageName=CreateProperty("StorageName");
            _goodShelf=CreateProperty("GoodShelf");

        }
        
        /// <summary>
        /// 初始化本类的信息
        /// </summary>
        public Management_View_GLoacation_GShelf_Storage() 
            :this(null,null)
        {
        }
    }
}
