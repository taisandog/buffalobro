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
        private static Management_SampleGression _SampleGression = new Management_SampleGression();
    
        public static Management_SampleGression SampleGression
        {
            get
            {
                return _SampleGression;
            }
        }
    }

    /// <summary>
    ///  
    /// </summary>
    public partial class Management_SampleGression : BQLEntityTableHandle
    {
        private BQLEntityParamHandle _id = null;
        /// <summary>
        /// 自动增长的ID
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

        /// <summary>
        /// 
        /// </summary>
        public Management_GoodsLocation BelongGoodsLocation_GoodLocationId_Id
        {
            get
            {
               return new Management_GoodsLocation(this,"BelongGoodsLocation_GoodLocationId_Id");
            }
         }


		/// <summary>
        /// 初始化本类的信息
        /// </summary>
        /// <param name="parent">父表信息</param>
        /// <param name="propertyName">属性名</param>
        public Management_SampleGression(BQLEntityTableHandle parent,string propertyName) 
        :this(typeof(ManagementLib.SampleGression),parent,propertyName)
        {
			
        }
        /// <summary>
        /// 初始化本类的信息
        /// </summary>
        /// <param name="parent">父表信息</param>
        /// <param name="propertyName">属性名</param>
        public Management_SampleGression(Type entityType,BQLEntityTableHandle parent,string propertyName) 
        :base(entityType,parent,propertyName)
        {
            _id=CreateProperty("Id");
            _sampleCode=CreateProperty("SampleCode");
            _storageId=CreateProperty("StorageId");
            _goodShelfId=CreateProperty("GoodShelfId");
            _goodLocationId=CreateProperty("GoodLocationId");

        }
        
        /// <summary>
        /// 初始化本类的信息
        /// </summary>
        public Management_SampleGression() 
            :this(null,null)
        {
        }
    }
}
