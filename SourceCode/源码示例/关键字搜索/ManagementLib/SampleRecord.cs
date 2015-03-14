using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Buffalo.DB.CommBase;
using Buffalo.Kernel.Defaults;
using Buffalo.DB.PropertyAttributes;
using Buffalo.DB.CommBase.BusinessBases;
namespace ManagementLib
{
	/// <summary>
    /// 
    /// </summary>
    public partial class SampleRecord: Buffalo.DB.CommBase.BusinessBases.ThinModelBase
    {
        ///<summary>
        ///主键ID，自动增长
        ///</summary>
        protected int? _id;

        /// <summary>
        ///主键ID，自动增长
        ///</summary>
        public int? Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id=value;
                OnPropertyUpdated("Id");
            }
        }
        ///<summary>
        ///取样者ID(与User表的id对应)
        ///</summary>
        protected int? _s_userId;

        /// <summary>
        ///取样者ID(与User表的id对应)
        ///</summary>
        public int? S_userId
        {
            get
            {
                return _s_userId;
            }
            set
            {
                _s_userId=value;
                OnPropertyUpdated("S_userId");
            }
        }
        ///<summary>
        ///样品编号(与SampleInfo表的sampleCode对应)
        ///</summary>
        protected string _sampleCode;

        /// <summary>
        ///样品编号(与SampleInfo表的sampleCode对应)
        ///</summary>
        public string SampleCode
        {
            get
            {
                return _sampleCode;
            }
            set
            {
                _sampleCode=value;
                OnPropertyUpdated("SampleCode");
            }
        }
        ///<summary>
        ///取样备注
        ///</summary>
        protected string _s_remark;

        /// <summary>
        ///取样备注
        ///</summary>
        public string S_remark
        {
            get
            {
                return _s_remark;
            }
            set
            {
                _s_remark=value;
                OnPropertyUpdated("S_remark");
            }
        }
        ///<summary>
        ///取样时间(系统自动生成，使用时间)
        ///</summary>
        protected DateTime? _employTime;

        /// <summary>
        ///取样时间(系统自动生成，使用时间)
        ///</summary>
        public DateTime? EmployTime
        {
            get
            {
                return _employTime;
            }
            set
            {
                _employTime=value;
                OnPropertyUpdated("EmployTime");
            }
        }
        ///<summary>
        ///样品借出还是归还(1为借出,0为归还)
        ///</summary>
        protected int? _recordType;

        /// <summary>
        ///样品借出还是归还(1为借出,0为归还)
        ///</summary>
        public int? RecordType
        {
            get
            {
                return _recordType;
            }
            set
            {
                _recordType=value;
                OnPropertyUpdated("RecordType");
            }
        }


        /// <summary>
        /// 
        /// </summary>
        protected Users _belongUsers_S_userId_Id;

        /// <summary>
        /// 
        /// </summary>
        public Users BelongUsers_S_userId_Id
        {
            get
            {
               if (_belongUsers_S_userId_Id == null)
               {
                   FillParent("BelongUsers_S_userId_Id");
               }
               return _belongUsers_S_userId_Id;
            }
            set
            {
               _belongUsers_S_userId_Id = value;
               OnPropertyUpdated("BelongUsers_S_userId_Id");
            }
        }


        private static ModelContext<SampleRecord> _____baseContext=new ModelContext<SampleRecord>();
        /// <summary>
        /// 获取查询关联类
        /// </summary>
        /// <returns></returns>
        public static ModelContext<SampleRecord> GetContext() 
        {
            return _____baseContext;
        }

    }
}
