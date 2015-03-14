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
    public partial class View_InStorage: Buffalo.DB.CommBase.BusinessBases.ThinModelBase
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
        protected int? _storageId;

        /// <summary>
        ///
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
        ///
        ///</summary>
        protected int? _goodShelfId;

        /// <summary>
        ///
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
        ///
        ///</summary>
        protected int? _goodLocationId;

        /// <summary>
        ///
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
        protected int? _state;

        /// <summary>
        ///
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
        ///
        ///</summary>
        protected DateTime? _instorageTime;

        /// <summary>
        ///
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
        ///<summary>
        ///
        ///</summary>
        protected string _storageName;

        /// <summary>
        ///
        ///</summary>
        public string StorageName
        {
            get
            {
                return _storageName;
            }
            set
            {
                _storageName=value;
                OnPropertyUpdated("StorageName");
            }
        }
        ///<summary>
        ///
        ///</summary>
        protected string _goodShelf;

        /// <summary>
        ///
        ///</summary>
        public string GoodShelf
        {
            get
            {
                return _goodShelf;
            }
            set
            {
                _goodShelf=value;
                OnPropertyUpdated("GoodShelf");
            }
        }
        ///<summary>
        ///
        ///</summary>
        protected string _locationCode;

        /// <summary>
        ///
        ///</summary>
        public string LocationCode
        {
            get
            {
                return _locationCode;
            }
            set
            {
                _locationCode=value;
                OnPropertyUpdated("LocationCode");
            }
        }
        ///<summary>
        ///
        ///</summary>
        protected string _boxNum;

        /// <summary>
        ///
        ///</summary>
        public string BoxNum
        {
            get
            {
                return _boxNum;
            }
            set
            {
                _boxNum=value;
                OnPropertyUpdated("BoxNum");
            }
        }




        private static ModelContext<View_InStorage> _____baseContext=new ModelContext<View_InStorage>();
        /// <summary>
        /// 获取查询关联类
        /// </summary>
        /// <returns></returns>
        public static ModelContext<View_InStorage> GetContext() 
        {
            return _____baseContext;
        }

    }
}
