using System;
using System.Data;
using System.Configuration;
using Buffalo.DB.EntityInfos;
using Buffalo.DB.BQLCommon;
using Buffalo.DB.BQLCommon.BQLConditionCommon;
using Buffalo.DB.PropertyAttributes;
namespace TestLib.BQLEntity
{

    public partial class YMRDB 
    {
        private static BQL_Gduser _Gduser = new BQL_Gduser();
    
        public static BQL_Gduser Gduser
        {
            get
            {
                return _Gduser;
            }
        }
    }

    /// <summary>
    ///  
    /// </summary>
    public partial class BQL_Gduser : TestLib.BQLEntity.BQL_YMRBase
    {
        private BQLEntityParamHandle _id = null;
        /// <summary>
        ///ID
        /// </summary>
        public BQLEntityParamHandle Id
        {
            get
            {
                return _id;
            }
         }
        private BQLEntityParamHandle _userId = null;
        /// <summary>
        ///玩家ID
        /// </summary>
        public BQLEntityParamHandle UserId
        {
            get
            {
                return _userId;
            }
         }
        private BQLEntityParamHandle _name = null;
        /// <summary>
        ///用户名
        /// </summary>
        public BQLEntityParamHandle Name
        {
            get
            {
                return _name;
            }
         }
        private BQLEntityParamHandle _nickName = null;
        /// <summary>
        ///昵称
        /// </summary>
        public BQLEntityParamHandle NickName
        {
            get
            {
                return _nickName;
            }
         }
        private BQLEntityParamHandle _token = null;
        /// <summary>
        ///用户标识
        /// </summary>
        public BQLEntityParamHandle Token
        {
            get
            {
                return _token;
            }
         }
        private BQLEntityParamHandle _agents = null;
        /// <summary>
        ///推荐人
        /// </summary>
        public BQLEntityParamHandle Agents
        {
            get
            {
                return _agents;
            }
         }
        private BQLEntityParamHandle _isLoginin = null;
        /// <summary>
        ///是否已经登入
        /// </summary>
        public BQLEntityParamHandle IsLoginin
        {
            get
            {
                return _isLoginin;
            }
         }
        private BQLEntityParamHandle _loginCoin = null;
        /// <summary>
        ///登陆时候的金币
        /// </summary>
        public BQLEntityParamHandle LoginCoin
        {
            get
            {
                return _loginCoin;
            }
         }
        private BQLEntityParamHandle _loginDiamond = null;
        /// <summary>
        ///登陆时候的钻石
        /// </summary>
        public BQLEntityParamHandle LoginDiamond
        {
            get
            {
                return _loginDiamond;
            }
         }
        private BQLEntityParamHandle _server = null;
        /// <summary>
        ///所在服务器(多服务器)
        /// </summary>
        public BQLEntityParamHandle Server
        {
            get
            {
                return _server;
            }
         }
        private BQLEntityParamHandle _createDate = null;
        /// <summary>
        ///创建时间
        /// </summary>
        public BQLEntityParamHandle CreateDate
        {
            get
            {
                return _createDate;
            }
         }
        private BQLEntityParamHandle _lastDate = null;
        /// <summary>
        ///最后更新时间
        /// </summary>
        public BQLEntityParamHandle LastDate
        {
            get
            {
                return _lastDate;
            }
         }
        private BQLEntityParamHandle _room = null;
        /// <summary>
        ///所在房间
        /// </summary>
        public BQLEntityParamHandle Room
        {
            get
            {
                return _room;
            }
         }
        private BQLEntityParamHandle _boundCoins = null;
        /// <summary>
        ///币数
        /// </summary>
        public BQLEntityParamHandle BoundCoins
        {
            get
            {
                return _boundCoins;
            }
         }



		/// <summary>
        /// 初始化本类的信息
        /// </summary>
        /// <param name="parent">父表信息</param>
        /// <param name="propertyName">属性名</param>
        public BQL_Gduser(BQLEntityTableHandle parent,string propertyName) 
        :this(typeof(TestLib.Gduser),parent,propertyName)
        {
			
        }
        /// <summary>
        /// 初始化本类的信息
        /// </summary>
        /// <param name="parent">父表信息</param>
        /// <param name="propertyName">属性名</param>
        public BQL_Gduser(Type entityType,BQLEntityTableHandle parent,string propertyName) 
        :base(entityType,parent,propertyName)
        {
            _id=CreateProperty("Id");
            _userId=CreateProperty("UserId");
            _name=CreateProperty("Name");
            _nickName=CreateProperty("NickName");
            _token=CreateProperty("Token");
            _agents=CreateProperty("Agents");
            _isLoginin=CreateProperty("IsLoginin");
            _loginCoin=CreateProperty("LoginCoin");
            _loginDiamond=CreateProperty("LoginDiamond");
            _server=CreateProperty("Server");
            _createDate=CreateProperty("CreateDate");
            _lastDate=CreateProperty("LastDate");
            _room=CreateProperty("Room");
            _boundCoins=CreateProperty("BoundCoins");

        }
        
        /// <summary>
        /// 初始化本类的信息
        /// </summary>
        public BQL_Gduser() 
            :this(null,null)
        {
        }
    }
}
