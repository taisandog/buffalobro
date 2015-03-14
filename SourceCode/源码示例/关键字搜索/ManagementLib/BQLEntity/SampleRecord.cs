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
        private static Management_SampleRecord _SampleRecord = new Management_SampleRecord();
    
        public static Management_SampleRecord SampleRecord
        {
            get
            {
                return _SampleRecord;
            }
        }
    }

    /// <summary>
    ///  
    /// </summary>
    public partial class Management_SampleRecord : BQLEntityTableHandle
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
        private BQLEntityParamHandle _s_userId = null;
        /// <summary>
        /// 取样者ID(与User表的id对应)
        /// </summary>
        public BQLEntityParamHandle S_userId
        {
            get
            {
                return _s_userId;
            }
         }
        private BQLEntityParamHandle _sampleCode = null;
        /// <summary>
        /// 样品编号(与SampleInfo表的sampleCode对应)
        /// </summary>
        public BQLEntityParamHandle SampleCode
        {
            get
            {
                return _sampleCode;
            }
         }
        private BQLEntityParamHandle _s_remark = null;
        /// <summary>
        /// 取样备注
        /// </summary>
        public BQLEntityParamHandle S_remark
        {
            get
            {
                return _s_remark;
            }
         }
        private BQLEntityParamHandle _employTime = null;
        /// <summary>
        /// 取样时间(系统自动生成，使用时间)
        /// </summary>
        public BQLEntityParamHandle EmployTime
        {
            get
            {
                return _employTime;
            }
         }
        private BQLEntityParamHandle _recordType = null;
        /// <summary>
        /// 样品借出还是归还(1为借出,0为归还)
        /// </summary>
        public BQLEntityParamHandle RecordType
        {
            get
            {
                return _recordType;
            }
         }

        /// <summary>
        /// 
        /// </summary>
        public Management_Users BelongUsers_S_userId_Id
        {
            get
            {
               return new Management_Users(this,"BelongUsers_S_userId_Id");
            }
         }


		/// <summary>
        /// 初始化本类的信息
        /// </summary>
        /// <param name="parent">父表信息</param>
        /// <param name="propertyName">属性名</param>
        public Management_SampleRecord(BQLEntityTableHandle parent,string propertyName) 
        :this(typeof(ManagementLib.SampleRecord),parent,propertyName)
        {
			
        }
        /// <summary>
        /// 初始化本类的信息
        /// </summary>
        /// <param name="parent">父表信息</param>
        /// <param name="propertyName">属性名</param>
        public Management_SampleRecord(Type entityType,BQLEntityTableHandle parent,string propertyName) 
        :base(entityType,parent,propertyName)
        {
            _id=CreateProperty("Id");
            _s_userId=CreateProperty("S_userId");
            _sampleCode=CreateProperty("SampleCode");
            _s_remark=CreateProperty("S_remark");
            _employTime=CreateProperty("EmployTime");
            _recordType=CreateProperty("RecordType");

        }
        
        /// <summary>
        /// 初始化本类的信息
        /// </summary>
        public Management_SampleRecord() 
            :this(null,null)
        {
        }
    }
}
