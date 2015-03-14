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
        private static Management_BarCodeGauge _BarCodeGauge = new Management_BarCodeGauge();
    
        public static Management_BarCodeGauge BarCodeGauge
        {
            get
            {
                return _BarCodeGauge;
            }
        }
    }

    /// <summary>
    ///  
    /// </summary>
    public partial class Management_BarCodeGauge : BQLEntityTableHandle
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
        private BQLEntityParamHandle _customCode = null;
        /// <summary>
        /// 用户自定义的条码表头
        /// </summary>
        public BQLEntityParamHandle CustomCode
        {
            get
            {
                return _customCode;
            }
         }
        private BQLEntityParamHandle _state = null;
        /// <summary>
        /// 是否有效
        /// </summary>
        public BQLEntityParamHandle State
        {
            get
            {
                return _state;
            }
         }
        private BQLEntityParamHandle _maxSerialnumber = null;
        /// <summary>
        /// 最大流水号
        /// </summary>
        public BQLEntityParamHandle MaxSerialnumber
        {
            get
            {
                return _maxSerialnumber;
            }
         }



		/// <summary>
        /// 初始化本类的信息
        /// </summary>
        /// <param name="parent">父表信息</param>
        /// <param name="propertyName">属性名</param>
        public Management_BarCodeGauge(BQLEntityTableHandle parent,string propertyName) 
        :this(typeof(ManagementLib.BarCodeGauge),parent,propertyName)
        {
			
        }
        /// <summary>
        /// 初始化本类的信息
        /// </summary>
        /// <param name="parent">父表信息</param>
        /// <param name="propertyName">属性名</param>
        public Management_BarCodeGauge(Type entityType,BQLEntityTableHandle parent,string propertyName) 
        :base(entityType,parent,propertyName)
        {
            _id=CreateProperty("Id");
            _customCode=CreateProperty("CustomCode");
            _state=CreateProperty("State");
            _maxSerialnumber=CreateProperty("MaxSerialnumber");

        }
        
        /// <summary>
        /// 初始化本类的信息
        /// </summary>
        public Management_BarCodeGauge() 
            :this(null,null)
        {
        }
    }
}
