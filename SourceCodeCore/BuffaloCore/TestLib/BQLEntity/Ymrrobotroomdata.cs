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
        private static BQL_Ymrrobotroomdata _Ymrrobotroomdata = new BQL_Ymrrobotroomdata();
    
        public static BQL_Ymrrobotroomdata Ymrrobotroomdata
        {
            get
            {
                return _Ymrrobotroomdata;
            }
        }
    }

    /// <summary>
    ///  
    /// </summary>
    public partial class BQL_Ymrrobotroomdata : TestLib.BQLEntity.BQL_YMRBase
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
        private BQLEntityParamHandle _minJoinExitDeskTime = null;
        /// <summary>
        ///最小进、出桌子时间（秒）
        /// </summary>
        public BQLEntityParamHandle MinJoinExitDeskTime
        {
            get
            {
                return _minJoinExitDeskTime;
            }
         }
        private BQLEntityParamHandle _maxJoinExitDeskTime = null;
        /// <summary>
        ///最大进、出桌子时间（秒）
        /// </summary>
        public BQLEntityParamHandle MaxJoinExitDeskTime
        {
            get
            {
                return _maxJoinExitDeskTime;
            }
         }
        private BQLEntityParamHandle _roomLevel = null;
        /// <summary>
        ///房间等级
        /// </summary>
        public BQLEntityParamHandle RoomLevel
        {
            get
            {
                return _roomLevel;
            }
         }
        private BQLEntityParamHandle _roomMaxRobotCount = null;
        /// <summary>
        ///每个场最大机器人数量
        /// </summary>
        public BQLEntityParamHandle RoomMaxRobotCount
        {
            get
            {
                return _roomMaxRobotCount;
            }
         }
        private BQLEntityParamHandle _deskMaxRobotCount = null;
        /// <summary>
        ///桌子满员最大保留机器人数量
        /// </summary>
        public BQLEntityParamHandle DeskMaxRobotCount
        {
            get
            {
                return _deskMaxRobotCount;
            }
         }
        private BQLEntityParamHandle _joinRoomMinMoney = null;
        /// <summary>
        ///进入房间最小余额（元）
        /// </summary>
        public BQLEntityParamHandle JoinRoomMinMoney
        {
            get
            {
                return _joinRoomMinMoney;
            }
         }
        private BQLEntityParamHandle _joinRoomMaxMoney = null;
        /// <summary>
        ///进入房间最大余额（元）
        /// </summary>
        public BQLEntityParamHandle JoinRoomMaxMoney
        {
            get
            {
                return _joinRoomMaxMoney;
            }
         }
        private BQLEntityParamHandle _storageMinMultiple = null;
        /// <summary>
        ///存币最小倍率（存币倍率为相对于进场分）
        /// </summary>
        public BQLEntityParamHandle StorageMinMultiple
        {
            get
            {
                return _storageMinMultiple;
            }
         }
        private BQLEntityParamHandle _storageMaxMultiple = null;
        /// <summary>
        ///存币最大倍率（存币倍率为相对于进场分）
        /// </summary>
        public BQLEntityParamHandle StorageMaxMultiple
        {
            get
            {
                return _storageMaxMultiple;
            }
         }
        private BQLEntityParamHandle _compelExitMinWinMultiple = null;
        /// <summary>
        ///强制退出赢的最小倍率
        /// </summary>
        public BQLEntityParamHandle CompelExitMinWinMultiple
        {
            get
            {
                return _compelExitMinWinMultiple;
            }
         }
        private BQLEntityParamHandle _compelExitMaxWinMultiple = null;
        /// <summary>
        ///强制退出赢的最大倍率
        /// </summary>
        public BQLEntityParamHandle CompelExitMaxWinMultiple
        {
            get
            {
                return _compelExitMaxWinMultiple;
            }
         }
        private BQLEntityParamHandle _withdrawalsMinCount = null;
        /// <summary>
        ///存币上限最小次数
        /// </summary>
        public BQLEntityParamHandle WithdrawalsMinCount
        {
            get
            {
                return _withdrawalsMinCount;
            }
         }
        private BQLEntityParamHandle _withdrawalsMaxCount = null;
        /// <summary>
        ///存币上限最大次数
        /// </summary>
        public BQLEntityParamHandle WithdrawalsMaxCount
        {
            get
            {
                return _withdrawalsMaxCount;
            }
         }
        private BQLEntityParamHandle _pollingJoinRoomMinTime = null;
        /// <summary>
        ///轮询进入房间最小时间（分钟）
        /// </summary>
        public BQLEntityParamHandle PollingJoinRoomMinTime
        {
            get
            {
                return _pollingJoinRoomMinTime;
            }
         }
        private BQLEntityParamHandle _pollingJoinRoomMaxTime = null;
        /// <summary>
        ///轮询进入房间最大时间（分钟）
        /// </summary>
        public BQLEntityParamHandle PollingJoinRoomMaxTime
        {
            get
            {
                return _pollingJoinRoomMaxTime;
            }
         }
        private BQLEntityParamHandle _commonMinStayTime = null;
        /// <summary>
        ///通用最小停留时间（秒，主要用于存取币后停多久发炮，退出桌子前停留多久）
        /// </summary>
        public BQLEntityParamHandle CommonMinStayTime
        {
            get
            {
                return _commonMinStayTime;
            }
         }
        private BQLEntityParamHandle _commonMaxStayTime = null;
        /// <summary>
        ///通用最大停留时间（秒，主要用于存取币后停多久发炮，退出桌子前停留多久）
        /// </summary>
        public BQLEntityParamHandle CommonMaxStayTime
        {
            get
            {
                return _commonMaxStayTime;
            }
         }



		/// <summary>
        /// 初始化本类的信息
        /// </summary>
        /// <param name="parent">父表信息</param>
        /// <param name="propertyName">属性名</param>
        public BQL_Ymrrobotroomdata(BQLEntityTableHandle parent,string propertyName) 
        :this(typeof(TestLib.Ymrrobotroomdata),parent,propertyName)
        {
			
        }
        /// <summary>
        /// 初始化本类的信息
        /// </summary>
        /// <param name="parent">父表信息</param>
        /// <param name="propertyName">属性名</param>
        public BQL_Ymrrobotroomdata(Type entityType,BQLEntityTableHandle parent,string propertyName) 
        :base(entityType,parent,propertyName)
        {
            _id=CreateProperty("Id");
            _createDate=CreateProperty("CreateDate");
            _lastDate=CreateProperty("LastDate");
            _minJoinExitDeskTime=CreateProperty("MinJoinExitDeskTime");
            _maxJoinExitDeskTime=CreateProperty("MaxJoinExitDeskTime");
            _roomLevel=CreateProperty("RoomLevel");
            _roomMaxRobotCount=CreateProperty("RoomMaxRobotCount");
            _deskMaxRobotCount=CreateProperty("DeskMaxRobotCount");
            _joinRoomMinMoney=CreateProperty("JoinRoomMinMoney");
            _joinRoomMaxMoney=CreateProperty("JoinRoomMaxMoney");
            _storageMinMultiple=CreateProperty("StorageMinMultiple");
            _storageMaxMultiple=CreateProperty("StorageMaxMultiple");
            _compelExitMinWinMultiple=CreateProperty("CompelExitMinWinMultiple");
            _compelExitMaxWinMultiple=CreateProperty("CompelExitMaxWinMultiple");
            _withdrawalsMinCount=CreateProperty("WithdrawalsMinCount");
            _withdrawalsMaxCount=CreateProperty("WithdrawalsMaxCount");
            _pollingJoinRoomMinTime=CreateProperty("PollingJoinRoomMinTime");
            _pollingJoinRoomMaxTime=CreateProperty("PollingJoinRoomMaxTime");
            _commonMinStayTime=CreateProperty("CommonMinStayTime");
            _commonMaxStayTime=CreateProperty("CommonMaxStayTime");

        }
        
        /// <summary>
        /// 初始化本类的信息
        /// </summary>
        public BQL_Ymrrobotroomdata() 
            :this(null,null)
        {
        }
    }
}
