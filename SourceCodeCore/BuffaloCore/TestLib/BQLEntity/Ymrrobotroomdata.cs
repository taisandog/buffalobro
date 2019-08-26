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
        private BQLEntityParamHandle _minJoinExitDeskTime = null;
        /// <summary>
        ///��С����������ʱ�䣨�룩
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
        ///������������ʱ�䣨�룩
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
        ///����ȼ�
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
        ///ÿ����������������
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
        ///������Ա���������������
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
        ///���뷿����С��Ԫ��
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
        ///���뷿�������Ԫ��
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
        ///�����С���ʣ���ұ���Ϊ����ڽ����֣�
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
        ///�������ʣ���ұ���Ϊ����ڽ����֣�
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
        ///ǿ���˳�Ӯ����С����
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
        ///ǿ���˳�Ӯ�������
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
        ///���������С����
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
        ///�������������
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
        ///��ѯ���뷿����Сʱ�䣨���ӣ�
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
        ///��ѯ���뷿�����ʱ�䣨���ӣ�
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
        ///ͨ����Сͣ��ʱ�䣨�룬��Ҫ���ڴ�ȡ�Һ�ͣ��÷��ڣ��˳�����ǰͣ����ã�
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
        ///ͨ�����ͣ��ʱ�䣨�룬��Ҫ���ڴ�ȡ�Һ�ͣ��÷��ڣ��˳�����ǰͣ����ã�
        /// </summary>
        public BQLEntityParamHandle CommonMaxStayTime
        {
            get
            {
                return _commonMaxStayTime;
            }
         }



		/// <summary>
        /// ��ʼ���������Ϣ
        /// </summary>
        /// <param name="parent">������Ϣ</param>
        /// <param name="propertyName">������</param>
        public BQL_Ymrrobotroomdata(BQLEntityTableHandle parent,string propertyName) 
        :this(typeof(TestLib.Ymrrobotroomdata),parent,propertyName)
        {
			
        }
        /// <summary>
        /// ��ʼ���������Ϣ
        /// </summary>
        /// <param name="parent">������Ϣ</param>
        /// <param name="propertyName">������</param>
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
        /// ��ʼ���������Ϣ
        /// </summary>
        public BQL_Ymrrobotroomdata() 
            :this(null,null)
        {
        }
    }
}
