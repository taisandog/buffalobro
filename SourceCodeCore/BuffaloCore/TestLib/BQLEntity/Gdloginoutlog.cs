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
        private static BQL_Gdloginoutlog _Gdloginoutlog = new BQL_Gdloginoutlog();
    
        public static BQL_Gdloginoutlog Gdloginoutlog
        {
            get
            {
                return _Gdloginoutlog;
            }
        }
    }

    /// <summary>
    ///  
    /// </summary>
    public partial class BQL_Gdloginoutlog : TestLib.BQLEntity.BQL_YMRBase
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
        private BQLEntityParamHandle _coin = null;
        /// <summary>
        ///金币
        /// </summary>
        public BQLEntityParamHandle Coin
        {
            get
            {
                return _coin;
            }
         }
        private BQLEntityParamHandle _diamond = null;
        /// <summary>
        ///钻石
        /// </summary>
        public BQLEntityParamHandle Diamond
        {
            get
            {
                return _diamond;
            }
         }
        private BQLEntityParamHandle _changeCoin = null;
        /// <summary>
        ///改变的金币
        /// </summary>
        public BQLEntityParamHandle ChangeCoin
        {
            get
            {
                return _changeCoin;
            }
         }
        private BQLEntityParamHandle _changeDiamond = null;
        /// <summary>
        ///钻石
        /// </summary>
        public BQLEntityParamHandle ChangeDiamond
        {
            get
            {
                return _changeDiamond;
            }
         }
        private BQLEntityParamHandle _commission = null;
        /// <summary>
        ///用户抽水
        /// </summary>
        public BQLEntityParamHandle Commission
        {
            get
            {
                return _commission;
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
        private BQLEntityParamHandle _isLoginout = null;
        /// <summary>
        ///记录时候是否已经登出
        /// </summary>
        public BQLEntityParamHandle IsLoginout
        {
            get
            {
                return _isLoginout;
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
        private BQLEntityParamHandle _changeBoundNumber = null;
        /// <summary>
        ///改变的奖励数
        /// </summary>
        public BQLEntityParamHandle ChangeBoundNumber
        {
            get
            {
                return _changeBoundNumber;
            }
         }
        private BQLEntityParamHandle _totalBet = null;
        /// <summary>
        ///押注量/发炮量
        /// </summary>
        public BQLEntityParamHandle TotalBet
        {
            get
            {
                return _totalBet;
            }
         }



		/// <summary>
        /// 初始化本类的信息
        /// </summary>
        /// <param name="parent">父表信息</param>
        /// <param name="propertyName">属性名</param>
        public BQL_Gdloginoutlog(BQLEntityTableHandle parent,string propertyName) 
        :this(typeof(TestLib.Gdloginoutlog),parent,propertyName)
        {
			
        }
        /// <summary>
        /// 初始化本类的信息
        /// </summary>
        /// <param name="parent">父表信息</param>
        /// <param name="propertyName">属性名</param>
        public BQL_Gdloginoutlog(Type entityType,BQLEntityTableHandle parent,string propertyName) 
        :base(entityType,parent,propertyName)
        {
            _id=CreateProperty("Id");
            _userId=CreateProperty("UserId");
            _name=CreateProperty("Name");
            _nickName=CreateProperty("NickName");
            _token=CreateProperty("Token");
            _agents=CreateProperty("Agents");
            _coin=CreateProperty("Coin");
            _diamond=CreateProperty("Diamond");
            _changeCoin=CreateProperty("ChangeCoin");
            _changeDiamond=CreateProperty("ChangeDiamond");
            _commission=CreateProperty("Commission");
            _loginCoin=CreateProperty("LoginCoin");
            _loginDiamond=CreateProperty("LoginDiamond");
            _isLoginout=CreateProperty("IsLoginout");
            _server=CreateProperty("Server");
            _createDate=CreateProperty("CreateDate");
            _lastDate=CreateProperty("LastDate");
            _room=CreateProperty("Room");
            _changeBoundNumber=CreateProperty("ChangeBoundNumber");
            _totalBet=CreateProperty("TotalBet");

        }
        
        /// <summary>
        /// 初始化本类的信息
        /// </summary>
        public BQL_Gdloginoutlog() 
            :this(null,null)
        {
        }
    }
}
