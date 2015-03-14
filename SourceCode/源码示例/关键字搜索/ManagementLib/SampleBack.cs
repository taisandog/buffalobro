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
    public partial class SampleBack: Buffalo.DB.CommBase.BusinessBases.ThinModelBase
    {
        ///<summary>
        ///
        ///</summary>
        protected int? _id;

        /// <summary>
        ///
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
        ///
        ///</summary>
        protected string _sampleCode;

        /// <summary>
        ///
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
        ///
        ///</summary>
        protected int? _userId;

        /// <summary>
        ///
        ///</summary>
        public int? UserId
        {
            get
            {
                return _userId;
            }
            set
            {
                _userId=value;
                OnPropertyUpdated("UserId");
            }
        }
        ///<summary>
        ///
        ///</summary>
        protected DateTime? _backTime;

        /// <summary>
        ///
        ///</summary>
        public DateTime? BackTime
        {
            get
            {
                return _backTime;
            }
            set
            {
                _backTime=value;
                OnPropertyUpdated("BackTime");
            }
        }
        ///<summary>
        ///
        ///</summary>
        protected string _b_remark;

        /// <summary>
        ///
        ///</summary>
        public string B_remark
        {
            get
            {
                return _b_remark;
            }
            set
            {
                _b_remark=value;
                OnPropertyUpdated("B_remark");
            }
        }




        private static ModelContext<SampleBack> _____baseContext=new ModelContext<SampleBack>();
        /// <summary>
        /// 获取查询关联类
        /// </summary>
        /// <returns></returns>
        public static ModelContext<SampleBack> GetContext() 
        {
            return _____baseContext;
        }

    }
}
