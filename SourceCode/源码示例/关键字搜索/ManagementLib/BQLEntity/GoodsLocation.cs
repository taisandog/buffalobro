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
        private static Management_GoodsLocation _GoodsLocation = new Management_GoodsLocation();
    
        public static Management_GoodsLocation GoodsLocation
        {
            get
            {
                return _GoodsLocation;
            }
        }
    }

    /// <summary>
    ///  
    /// </summary>
    public partial class Management_GoodsLocation : BQLEntityTableHandle
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
        private BQLEntityParamHandle _locationCode = null;
        /// <summary>
        /// 货位条码
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
        /// 箱号
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
        /// 货架ID
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
        /// 所属仓库
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
        /// 备注
        /// </summary>
        public BQLEntityParamHandle Remark
        {
            get
            {
                return _remark;
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
        /// 
        /// </summary>
        public Management_GoodsShelf BelongGoodsShelf_GoodsShelfId_Id
        {
            get
            {
               return new Management_GoodsShelf(this,"BelongGoodsShelf_GoodsShelfId_Id");
            }
         }


		/// <summary>
        /// 初始化本类的信息
        /// </summary>
        /// <param name="parent">父表信息</param>
        /// <param name="propertyName">属性名</param>
        public Management_GoodsLocation(BQLEntityTableHandle parent,string propertyName) 
        :this(typeof(ManagementLib.GoodsLocation),parent,propertyName)
        {
			
        }
        /// <summary>
        /// 初始化本类的信息
        /// </summary>
        /// <param name="parent">父表信息</param>
        /// <param name="propertyName">属性名</param>
        public Management_GoodsLocation(Type entityType,BQLEntityTableHandle parent,string propertyName) 
        :base(entityType,parent,propertyName)
        {
            _id=CreateProperty("Id");
            _locationCode=CreateProperty("LocationCode");
            _boxNum=CreateProperty("BoxNum");
            _goodsShelfId=CreateProperty("GoodsShelfId");
            _storageId=CreateProperty("StorageId");
            _remark=CreateProperty("Remark");

        }
        
        /// <summary>
        /// 初始化本类的信息
        /// </summary>
        public Management_GoodsLocation() 
            :this(null,null)
        {
        }
    }
}
