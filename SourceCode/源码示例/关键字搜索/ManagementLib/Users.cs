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
    public partial class Users: Buffalo.DB.CommBase.BusinessBases.ThinModelBase
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
        ///用户登录的账号
        ///</summary>
        protected string _userID;

        /// <summary>
        ///用户登录的账号
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
        ///用户真实姓名
        ///</summary>
        protected string _userName;

        /// <summary>
        ///用户真实姓名
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
        ///用户登录密码
        ///</summary>
        protected string _userPwd;

        /// <summary>
        ///用户登录密码
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
        ///仓库的ID
        ///</summary>
        protected int? _storageId;

        /// <summary>
        ///仓库的ID
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
        ///权限(1为系统管理员，2为普通用户)
        ///</summary>
        protected int? _authority;

        /// <summary>
        ///权限(1为系统管理员，2为普通用户)
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
        ///此登录账号是否可用(0为不可用,1为可用)
        ///</summary>
        protected bool? _isEnable;

        /// <summary>
        ///此登录账号是否可用(0为不可用,1为可用)
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
        protected int? _classId;

        /// <summary>
        ///
        ///</summary>
        public int? ClassId
        {
            get
            {
                return _classId;
            }
            set
            {
                _classId=value;
                OnPropertyUpdated("ClassId");
            }
        }


        /// <summary>
        /// 
        /// </summary>
        protected List<SampleRecord> _lstSampleRecordId_S_userId;

        /// <summary>
        /// 
        /// </summary>
        public List<SampleRecord> LstSampleRecordId_S_userId
        {
            get
            {
               if (_lstSampleRecordId_S_userId == null)
               {
                   FillChild("LstSampleRecordId_S_userId");
               }
               return _lstSampleRecordId_S_userId;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        protected List<OutRecode> _lstOutRecodeId_UserId;

        /// <summary>
        /// 
        /// </summary>
        public List<OutRecode> LstOutRecodeId_UserId
        {
            get
            {
               if (_lstOutRecodeId_UserId == null)
               {
                   FillChild("LstOutRecodeId_UserId");
               }
               return _lstOutRecodeId_UserId;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        protected Users _belongUsers_Id_Id;

        /// <summary>
        /// 
        /// </summary>
        public Users BelongUsers_Id_Id
        {
            get
            {
               if (_belongUsers_Id_Id == null)
               {
                   FillParent("BelongUsers_Id_Id");
               }
               return _belongUsers_Id_Id;
            }
            set
            {
               _belongUsers_Id_Id = value;
               OnPropertyUpdated("BelongUsers_Id_Id");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        protected List<Users> _lstUsersId_Id;

        /// <summary>
        /// 
        /// </summary>
        public List<Users> LstUsersId_Id
        {
            get
            {
               if (_lstUsersId_Id == null)
               {
                   FillChild("LstUsersId_Id");
               }
               return _lstUsersId_Id;
            }
        }


        private static ModelContext<Users> _____baseContext=new ModelContext<Users>();
        /// <summary>
        /// 获取查询关联类
        /// </summary>
        /// <returns></returns>
        public static ModelContext<Users> GetContext() 
        {
            return _____baseContext;
        }

    }
}
