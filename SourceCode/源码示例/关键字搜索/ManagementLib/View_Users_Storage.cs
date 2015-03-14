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
    public partial class View_Users_Storage: Buffalo.DB.CommBase.BusinessBases.ThinModelBase
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
        protected string _userID;

        /// <summary>
        ///
        ///</summary>
        public string UserID
        {
            get
            {
                return _userID;
            }
            set
            {
                _userID=value;
                OnPropertyUpdated("UserID");
            }
        }
        ///<summary>
        ///
        ///</summary>
        protected string _userName;

        /// <summary>
        ///
        ///</summary>
        public string UserName
        {
            get
            {
                return _userName;
            }
            set
            {
                _userName=value;
                OnPropertyUpdated("UserName");
            }
        }
        ///<summary>
        ///
        ///</summary>
        protected string _userPwd;

        /// <summary>
        ///
        ///</summary>
        public string UserPwd
        {
            get
            {
                return _userPwd;
            }
            set
            {
                _userPwd=value;
                OnPropertyUpdated("UserPwd");
            }
        }
        ///<summary>
        ///
        ///</summary>
        protected int? _authority;

        /// <summary>
        ///
        ///</summary>
        public int? Authority
        {
            get
            {
                return _authority;
            }
            set
            {
                _authority=value;
                OnPropertyUpdated("Authority");
            }
        }
        ///<summary>
        ///
        ///</summary>
        protected bool? _isEnable;

        /// <summary>
        ///
        ///</summary>
        public bool? IsEnable
        {
            get
            {
                return _isEnable;
            }
            set
            {
                _isEnable=value;
                OnPropertyUpdated("IsEnable");
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




        private static ModelContext<View_Users_Storage> _____baseContext=new ModelContext<View_Users_Storage>();
        /// <summary>
        /// 获取查询关联类
        /// </summary>
        /// <returns></returns>
        public static ModelContext<View_Users_Storage> GetContext() 
        {
            return _____baseContext;
        }

    }
}
