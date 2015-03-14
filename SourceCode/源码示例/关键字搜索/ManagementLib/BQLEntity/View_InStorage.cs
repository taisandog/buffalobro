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
        private static Management_View_InStorage _View_InStorage = new Management_View_InStorage();
    
        public static Management_View_InStorage View_InStorage
        {
            get
            {
                return _View_InStorage;
            }
        }
    }

    /// <summary>
    ///  
    /// </summary>
    public partial class Management_View_InStorage : BQLEntityTableHandle
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
        private BQLEntityParamHandle _sampleCode = null;
        /// <summary>
        /// 
        /// </summary>
        public BQLEntityParamHandle SampleCode
        {
            get
            {
                return _sampleCode;
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
        private BQLEntityParamHandle _goodShelfId = null;
        /// <summary>
        /// 
        /// </summary>
        public BQLEntityParamHandle GoodShelfId
        {
            get
            {
                return _goodShelfId;
            }
         }
        private BQLEntityParamHandle _goodLocationId = null;
        /// <summary>
        /// 
        /// </summary>
        public BQLEntityParamHandle GoodLocationId
        {
            get
            {
                return _goodLocationId;
            }
         }
        private BQLEntityParamHandle _userId = null;
        /// <summary>
        /// 
        /// </summary>
        public BQLEntityParamHandle UserId
        {
            get
            {
                return _userId;
            }
         }
        private BQLEntityParamHandle _state = null;
        /// <summary>
        /// 
        /// </summary>
        public BQLEntityParamHandle State
        {
            get
            {
                return _state;
            }
         }
        private BQLEntityParamHandle _instorageTime = null;
        /// <summary>
        /// 
        /// </summary>
        public BQLEntityParamHandle InstorageTime
        {
            get
            {
                return _instorageTime;
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



		/// <summary>
        /// 初始化本类的信息
        /// </summary>
        /// <param name="parent">父表信息</param>
        /// <param name="propertyName">属性名</param>
        public Management_View_InStorage(BQLEntityTableHandle parent,string propertyName) 
        :this(typeof(ManagementLib.View_InStorage),parent,propertyName)
        {
			
        }
        /// <summary>
        /// 初始化本类的信息
        /// </summary>
        /// <param name="parent">父表信息</param>
        /// <param name="propertyName">属性名</param>
        public Management_View_InStorage(Type entityType,BQLEntityTableHandle parent,string propertyName) 
        :base(entityType,parent,propertyName)
        {
            _id=CreateProperty("Id");
            _sampleCode=CreateProperty("SampleCode");
            _storageId=CreateProperty("StorageId");
            _goodShelfId=CreateProperty("GoodShelfId");
            _goodLocationId=CreateProperty("GoodLocationId");
            _userId=CreateProperty("UserId");
            _state=CreateProperty("State");
            _instorageTime=CreateProperty("InstorageTime");
            _storageName=CreateProperty("StorageName");
            _goodShelf=CreateProperty("GoodShelf");
            _locationCode=CreateProperty("LocationCode");
            _boxNum=CreateProperty("BoxNum");

        }
        
        /// <summary>
        /// 初始化本类的信息
        /// </summary>
        public Management_View_InStorage() 
            :this(null,null)
        {
        }
    }
}
