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
        private static Management_View_Users_Storage _View_Users_Storage = new Management_View_Users_Storage();
    
        public static Management_View_Users_Storage View_Users_Storage
        {
            get
            {
                return _View_Users_Storage;
            }
        }
    }

    /// <summary>
    ///  
    /// </summary>
    public partial class Management_View_Users_Storage : BQLEntityTableHandle
    {
        private BQLEntityParamHandle _storageName = null;
        /// <summary>
        /// 
        /// </summary>
        public BQLEntityParamHandle StorageName
        {
            get
            {
                return _storageName;
            }
         }
        private BQLEntityParamHandle _id = null;
        /// <summary>
        /// 
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
        /// 
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
        /// 
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
        /// 
        /// </summary>
        public BQLEntityParamHandle UserPwd
        {
            get
            {
                return _userPwd;
            }
         }
        private BQLEntityParamHandle _authority = null;
        /// <summary>
        /// 
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
        /// 
        /// </summary>
        public BQLEntityParamHandle IsEnable
        {
            get
            {
                return _isEnable;
            }
         }
        private BQLEntityParamHandle _storageId = null;
        /// <summary>
        /// 
        /// </summary>
        public BQLEntityParamHandle StorageId
        {
            get
            {
                return _storageId;
            }
         }



		/// <summary>
        /// 初始化本类的信息
        /// </summary>
        /// <param name="parent">父表信息</param>
        /// <param name="propertyName">属性名</param>
        public Management_View_Users_Storage(BQLEntityTableHandle parent,string propertyName) 
        :this(typeof(ManagementLib.View_Users_Storage),parent,propertyName)
        {
			
        }
        /// <summary>
        /// 初始化本类的信息
        /// </summary>
        /// <param name="parent">父表信息</param>
        /// <param name="propertyName">属性名</param>
        public Management_View_Users_Storage(Type entityType,BQLEntityTableHandle parent,string propertyName) 
        :base(entityType,parent,propertyName)
        {
            _storageName=CreateProperty("StorageName");
            _id=CreateProperty("Id");
            _userID=CreateProperty("UserID");
            _userName=CreateProperty("UserName");
            _userPwd=CreateProperty("UserPwd");
            _authority=CreateProperty("Authority");
            _isEnable=CreateProperty("IsEnable");
            _storageId=CreateProperty("StorageId");

        }
        
        /// <summary>
        /// 初始化本类的信息
        /// </summary>
        public Management_View_Users_Storage() 
            :this(null,null)
        {
        }
    }
}
