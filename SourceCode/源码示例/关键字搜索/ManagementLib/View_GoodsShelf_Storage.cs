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
    public partial class View_GoodsShelf_Storage: Buffalo.DB.CommBase.BusinessBases.ThinModelBase
    {
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




        private static ModelContext<View_GoodsShelf_Storage> _____baseContext=new ModelContext<View_GoodsShelf_Storage>();
        /// <summary>
        /// 获取查询关联类
        /// </summary>
        /// <returns></returns>
        public static ModelContext<View_GoodsShelf_Storage> GetContext() 
        {
            return _____baseContext;
        }

    }
}
