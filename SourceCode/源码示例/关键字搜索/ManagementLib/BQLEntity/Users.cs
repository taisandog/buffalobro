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
        private static Management_Users _Users = new Management_Users();
    
        public static Management_Users Users
        {
            get
            {
                return _Users;
            }
        }
    }

    /// <summary>
    ///  
    /// </summary>
    public partial class Management_Users : BQLEntityTableHandle
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
        private BQLEntityParamHandle _userID = null;
        /// <summary>
        /// 用户登录的账号
        /// </summary>
        public BQLEntityParamHandle UserID
        {
            get
            {
                return _userID;
            }
         }
        private BQLEntityParamHandle _userName = null;
        /// <summary>
        /// 用户真实姓名
        /// </summary>
        public BQLEntityParamHandle UserName
        {
            get
            {
                return _userName;
            }
         }
        private BQLEntityParamHandle _userPwd = null;
        /// <summary>
        /// 用户登录密码
        /// </summary>
        public BQLEntityParamHandle UserPwd
        {
            get
            {
                return _userPwd;
            }
         }
        private BQLEntityParamHandle _storageId = null;
        /// <summary>
        /// 仓库的ID
        /// </summary>
        public BQLEntityParamHandle StorageId
        {
            get
            {
                return _storageId;
            }
         }
        private BQLEntityParamHandle _authority = null;
        /// <summary>
        /// 权限(1为系统管理员，2为普通用户)
        /// </summary>
        public BQLEntityParamHandle Authority
        {
            get
            {
                return _authority;
            }
         }
        private BQLEntityParamHandle _isEnable = null;
        /// <summary>
        /// 此登录账号是否可用(0为不可用,1为可用)
        /// </summary>
        public BQLEntityParamHandle IsEnable
        {
            get
            {
                return _isEnable;
            }
         }
        private BQLEntityParamHandle _classId = null;
        /// <summary>
        /// 
        /// </summary>
        public BQLEntityParamHandle ClassId
        {
            get
            {
                return _classId;
            }
         }

        /// <summary>
        /// 
        /// </summary>
        public Management_Users BelongUsers_Id_Id
        {
            get
            {
               return new Management_Users(this,"BelongUsers_Id_Id");
            }
         }


		/// <summary>
        /// 初始化本类的信息
        /// </summary>
        /// <param name="parent">父表信息</param>
        /// <param name="propertyName">属性名</param>
        public Management_Users(BQLEntityTableHandle parent,string propertyName) 
        :this(typeof(ManagementLib.Users),parent,propertyName)
        {
			
        }
        /// <summary>
        /// 初始化本类的信息
        /// </summary>
        /// <param name="parent">父表信息</param>
        /// <param name="propertyName">属性名</param>
        public Management_Users(Type entityType,BQLEntityTableHandle parent,string propertyName) 
        :base(entityType,parent,propertyName)
        {
            _id=CreateProperty("Id");
            _userID=CreateProperty("UserID");
            _userName=CreateProperty("UserName");
            _userPwd=CreateProperty("UserPwd");
            _storageId=CreateProperty("StorageId");
            _authority=CreateProperty("Authority");
            _isEnable=CreateProperty("IsEnable");
            _classId=CreateProperty("ClassId");

        }
        
        /// <summary>
        /// 初始化本类的信息
        /// </summary>
        public Management_Users() 
            :this(null,null)
        {
        }
    }
}
