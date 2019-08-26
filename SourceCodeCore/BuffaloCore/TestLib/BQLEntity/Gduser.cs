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
        private BQLEntityParamHandle _isLoginin = null;
        /// <summary>
        ///�Ƿ��Ѿ�����
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
        private BQLEntityParamHandle _boundCoins = null;
        /// <summary>
        ///����
        /// </summary>
        public BQLEntityParamHandle BoundCoins
        {
            get
            {
                return _boundCoins;
            }
         }



		/// <summary>
        /// ��ʼ���������Ϣ
        /// </summary>
        /// <param name="parent">������Ϣ</param>
        /// <param name="propertyName">������</param>
        public BQL_Gduser(BQLEntityTableHandle parent,string propertyName) 
        :this(typeof(TestLib.Gduser),parent,propertyName)
        {
			
        }
        /// <summary>
        /// ��ʼ���������Ϣ
        /// </summary>
        /// <param name="parent">������Ϣ</param>
        /// <param name="propertyName">������</param>
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
        /// ��ʼ���������Ϣ
        /// </summary>
        public BQL_Gduser() 
            :this(null,null)
        {
        }
    }
}
