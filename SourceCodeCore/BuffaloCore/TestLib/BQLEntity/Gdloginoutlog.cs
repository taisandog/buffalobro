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
        ///���ID
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
        ///�û���
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
        ///�ǳ�
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
        ///�û���ʶ
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
        ///�Ƽ���
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
        ///���
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
        ///��ʯ
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
        ///�ı�Ľ��
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
        ///��ʯ
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
        ///�û���ˮ
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
        ///��½ʱ��Ľ��
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
        ///��½ʱ�����ʯ
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
        ///��¼ʱ���Ƿ��Ѿ��ǳ�
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
        ///���ڷ�����(�������)
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
        ///����ʱ��
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
        ///������ʱ��
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
        ///���ڷ���
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
        ///�ı�Ľ�����
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
        ///Ѻע��/������
        /// </summary>
        public BQLEntityParamHandle TotalBet
        {
            get
            {
                return _totalBet;
            }
         }



		/// <summary>
        /// ��ʼ���������Ϣ
        /// </summary>
        /// <param name="parent">������Ϣ</param>
        /// <param name="propertyName">������</param>
        public BQL_Gdloginoutlog(BQLEntityTableHandle parent,string propertyName) 
        :this(typeof(TestLib.Gdloginoutlog),parent,propertyName)
        {
			
        }
        /// <summary>
        /// ��ʼ���������Ϣ
        /// </summary>
        /// <param name="parent">������Ϣ</param>
        /// <param name="propertyName">������</param>
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
        /// ��ʼ���������Ϣ
        /// </summary>
        public BQL_Gdloginoutlog() 
            :this(null,null)
        {
        }
    }
}
