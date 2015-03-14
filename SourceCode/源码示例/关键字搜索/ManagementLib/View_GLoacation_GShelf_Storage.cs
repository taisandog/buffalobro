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
    public partial class View_GLoacation_GShelf_Storage: Buffalo.DB.CommBase.BusinessBases.ThinModelBase
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
        ///<summary>
        ///
        ///</summary>
        protected int? _goodsShelfId;

        /// <summary>
        ///
        ///</summary>
        public int? GoodsShelfId
        {
            get
            {
                return _goodsShelfId;
            }
            set
            {
                _goodsShelfId=value;
                OnPropertyUpdated("GoodsShelfId");
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
        protected string _remark;

        /// <summary>
        ///
        ///</summary>
        public string Remark
        {
            get
            {
                return _remark;
            }
            set
            {
                _remark=value;
                OnPropertyUpdated("Remark");
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




        private static ModelContext<View_GLoacation_GShelf_Storage> _____baseContext=new ModelContext<View_GLoacation_GShelf_Storage>();
        /// <summary>
        /// 获取查询关联类
        /// </summary>
        /// <returns></returns>
        public static ModelContext<View_GLoacation_GShelf_Storage> GetContext() 
        {
            return _____baseContext;
        }

    }
}
