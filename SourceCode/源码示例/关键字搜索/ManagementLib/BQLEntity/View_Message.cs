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
        private static Management_View_Message _View_Message = new Management_View_Message();
    
        public static Management_View_Message View_Message
        {
            get
            {
                return _View_Message;
            }
        }
    }

    /// <summary>
    ///  
    /// </summary>
    public partial class Management_View_Message : BQLEntityTableHandle
    {
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
        private BQLEntityParamHandle _state = null;
        /// <summary>
        /// 
        /// </summary>
        public BQLEntityParamHandle State
        {
            get
            {
                return _state;
            }
         }
        private BQLEntityParamHandle _lastUpdate = null;
        /// <summary>
        /// 
        /// </summary>
        public BQLEntityParamHandle LastUpdate
        {
            get
            {
                return _lastUpdate;
            }
         }
        private BQLEntityParamHandle _title = null;
        /// <summary>
        /// 
        /// </summary>
        public BQLEntityParamHandle Title
        {
            get
            {
                return _title;
            }
         }
        private BQLEntityParamHandle _content = null;
        /// <summary>
        /// 
        /// </summary>
        public BQLEntityParamHandle Content
        {
            get
            {
                return _content;
            }
         }
        private BQLEntityParamHandle _belongUser = null;
        /// <summary>
        /// 
        /// </summary>
        public BQLEntityParamHandle BelongUser
        {
            get
            {
                return _belongUser;
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



		/// <summary>
        /// 初始化本类的信息
        /// </summary>
        /// <param name="parent">父表信息</param>
        /// <param name="propertyName">属性名</param>
        public Management_View_Message(BQLEntityTableHandle parent,string propertyName) 
        :this(typeof(ManagementLib.View_Message),parent,propertyName)
        {
			
        }
        /// <summary>
        /// 初始化本类的信息
        /// </summary>
        /// <param name="parent">父表信息</param>
        /// <param name="propertyName">属性名</param>
        public Management_View_Message(Type entityType,BQLEntityTableHandle parent,string propertyName) 
        :base(entityType,parent,propertyName)
        {
            _id=CreateProperty("Id");
            _state=CreateProperty("State");
            _lastUpdate=CreateProperty("LastUpdate");
            _title=CreateProperty("Title");
            _content=CreateProperty("Content");
            _belongUser=CreateProperty("BelongUser");
            _userName=CreateProperty("UserName");

        }
        
        /// <summary>
        /// 初始化本类的信息
        /// </summary>
        public Management_View_Message() 
            :this(null,null)
        {
        }
    }
}
