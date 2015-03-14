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
        private static Management_SampleBack _SampleBack = new Management_SampleBack();
    
        public static Management_SampleBack SampleBack
        {
            get
            {
                return _SampleBack;
            }
        }
    }

    /// <summary>
    ///  
    /// </summary>
    public partial class Management_SampleBack : BQLEntityTableHandle
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
        private BQLEntityParamHandle _backTime = null;
        /// <summary>
        /// 
        /// </summary>
        public BQLEntityParamHandle BackTime
        {
            get
            {
                return _backTime;
            }
         }
        private BQLEntityParamHandle _b_remark = null;
        /// <summary>
        /// 
        /// </summary>
        public BQLEntityParamHandle B_remark
        {
            get
            {
                return _b_remark;
            }
         }



		/// <summary>
        /// 初始化本类的信息
        /// </summary>
        /// <param name="parent">父表信息</param>
        /// <param name="propertyName">属性名</param>
        public Management_SampleBack(BQLEntityTableHandle parent,string propertyName) 
        :this(typeof(ManagementLib.SampleBack),parent,propertyName)
        {
			
        }
        /// <summary>
        /// 初始化本类的信息
        /// </summary>
        /// <param name="parent">父表信息</param>
        /// <param name="propertyName">属性名</param>
        public Management_SampleBack(Type entityType,BQLEntityTableHandle parent,string propertyName) 
        :base(entityType,parent,propertyName)
        {
            _id=CreateProperty("Id");
            _sampleCode=CreateProperty("SampleCode");
            _userId=CreateProperty("UserId");
            _backTime=CreateProperty("BackTime");
            _b_remark=CreateProperty("B_remark");

        }
        
        /// <summary>
        /// 初始化本类的信息
        /// </summary>
        public Management_SampleBack() 
            :this(null,null)
        {
        }
    }
}
