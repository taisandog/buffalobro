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
    public partial class InStorage: Buffalo.DB.CommBase.BusinessBases.ThinModelBase
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
        ///样品的条码
        ///</summary>
        protected string _sampleCode;

        /// <summary>
        ///样品的条码
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
        ///仓库ID
        ///</summary>
        protected int? _storageId;

        /// <summary>
        ///仓库ID
        ///</summary>
        public int? StorageId
        {
            get
            {
                return _storageId;
            }
            set
            {
                _storageId=value;
                OnPropertyUpdated("StorageId");
            }
        }
        ///<summary>
        ///货架ID
        ///</summary>
        protected int? _goodShelfId;

        /// <summary>
        ///货架ID
        ///</summary>
        public int? GoodShelfId
        {
            get
            {
                return _goodShelfId;
            }
            set
            {
                _goodShelfId=value;
                OnPropertyUpdated("GoodShelfId");
            }
        }
        ///<summary>
        ///货位ID
        ///</summary>
        protected int? _goodLocationId;

        /// <summary>
        ///货位ID
        ///</summary>
        public int? GoodLocationId
        {
            get
            {
                return _goodLocationId;
            }
            set
            {
                _goodLocationId=value;
                OnPropertyUpdated("GoodLocationId");
            }
        }
        ///<summary>
        ///入库操作者的ID(与用户表ID对应)
        ///</summary>
        protected int? _userId;

        /// <summary>
        ///入库操作者的ID(与用户表ID对应)
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
        ///是否在仓库里面(0在库,1不在库,2样品借出)
        ///</summary>
        protected int? _state;

        /// <summary>
        ///是否在仓库里面(0在库,1不在库,2样品借出)
        ///</summary>
        public int? State
        {
            get
            {
                return _state;
            }
            set
            {
                _state=value;
                OnPropertyUpdated("State");
            }
        }
        ///<summary>
        ///入库时间(系统自动生成)
        ///</summary>
        protected DateTime? _instorageTime;

        /// <summary>
        ///入库时间(系统自动生成)
        ///</summary>
        public DateTime? InstorageTime
        {
            get
            {
                return _instorageTime;
            }
            set
            {
                _instorageTime=value;
                OnPropertyUpdated("InstorageTime");
            }
        }




        private static ModelContext<InStorage> _____baseContext=new ModelContext<InStorage>();
        /// <summary>
        /// 获取查询关联类
        /// </summary>
        /// <returns></returns>
        public static ModelContext<InStorage> GetContext() 
        {
            return _____baseContext;
        }

    }
}
