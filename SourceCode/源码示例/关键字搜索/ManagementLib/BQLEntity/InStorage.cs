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
        private static Management_InStorage _InStorage = new Management_InStorage();
    
        public static Management_InStorage InStorage
        {
            get
            {
                return _InStorage;
            }
        }
    }

    /// <summary>
    ///  
    /// </summary>
    public partial class Management_InStorage : BQLEntityTableHandle
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
        private BQLEntityParamHandle _sampleCode = null;
        /// <summary>
        /// 样品的条码
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
        /// 仓库ID
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
        /// 货架ID
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
        /// 货位ID
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
        /// 入库操作者的ID(与用户表ID对应)
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
        /// 是否在仓库里面(0在库,1不在库,2样品借出)
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
        /// 入库时间(系统自动生成)
        /// </summary>
        public BQLEntityParamHandle InstorageTime
        {
            get
            {
                return _instorageTime;
            }
         }



		/// <summary>
        /// 初始化本类的信息
        /// </summary>
        /// <param name="parent">父表信息</param>
        /// <param name="propertyName">属性名</param>
        public Management_InStorage(BQLEntityTableHandle parent,string propertyName) 
        :this(typeof(ManagementLib.InStorage),parent,propertyName)
        {
			
        }
        /// <summary>
        /// 初始化本类的信息
        /// </summary>
        /// <param name="parent">父表信息</param>
        /// <param name="propertyName">属性名</param>
        public Management_InStorage(Type entityType,BQLEntityTableHandle parent,string propertyName) 
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

        }
        
        /// <summary>
        /// 初始化本类的信息
        /// </summary>
        public Management_InStorage() 
            :this(null,null)
        {
        }
    }
}
