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
        private static BQL_Yqsrobotconfig _Yqsrobotconfig = new BQL_Yqsrobotconfig();
    
        public static BQL_Yqsrobotconfig Yqsrobotconfig
        {
            get
            {
                return _Yqsrobotconfig;
            }
        }
    }

    /// <summary>
    ///  
    /// </summary>
    public partial class BQL_Yqsrobotconfig : TestLib.BQLEntity.BQL_YMRBase
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
        private BQLEntityParamHandle _enable = null;
        /// <summary>
        ///�Ƿ����
        /// </summary>
        public BQLEntityParamHandle Enable
        {
            get
            {
                return _enable;
            }
         }
        private BQLEntityParamHandle _joinRoomZero = null;
        /// <summary>
        ///�Ƿ���Խ���һ����
        /// </summary>
        public BQLEntityParamHandle JoinRoomZero
        {
            get
            {
                return _joinRoomZero;
            }
         }
        private BQLEntityParamHandle _joinRoomOne = null;
        /// <summary>
        ///�Ƿ���Խ��ڶ�����
        /// </summary>
        public BQLEntityParamHandle JoinRoomOne
        {
            get
            {
                return _joinRoomOne;
            }
         }
        private BQLEntityParamHandle _joinRoomTwo = null;
        /// <summary>
        ///�Ƿ���Խ���������
        /// </summary>
        public BQLEntityParamHandle JoinRoomTwo
        {
            get
            {
                return _joinRoomTwo;
            }
         }
        private BQLEntityParamHandle _joinRoomThree = null;
        /// <summary>
        ///�Ƿ���Խ����ĸ���
        /// </summary>
        public BQLEntityParamHandle JoinRoomThree
        {
            get
            {
                return _joinRoomThree;
            }
         }
        private BQLEntityParamHandle _minPlayGameTime = null;
        /// <summary>
        ///��С��Ϸʱ�䣨���ӣ�
        /// </summary>
        public BQLEntityParamHandle MinPlayGameTime
        {
            get
            {
                return _minPlayGameTime;
            }
         }
        private BQLEntityParamHandle _maxPlayGameTime = null;
        /// <summary>
        ///�����Ϸʱ�䣨���ӣ�
        /// </summary>
        public BQLEntityParamHandle MaxPlayGameTime
        {
            get
            {
                return _maxPlayGameTime;
            }
         }
        private BQLEntityParamHandle _lockState = null;
        /// <summary>
        ///����ģʽ
        /// </summary>
        public BQLEntityParamHandle LockState
        {
            get
            {
                return _lockState;
            }
         }
        private BQLEntityParamHandle _shootState = null;
        /// <summary>
        ///���ģʽ
        /// </summary>
        public BQLEntityParamHandle ShootState
        {
            get
            {
                return _shootState;
            }
         }
        private BQLEntityParamHandle _randomLockMinTime = null;
        /// <summary>
        ///���������Сʱ�䣨�룩
        /// </summary>
        public BQLEntityParamHandle RandomLockMinTime
        {
            get
            {
                return _randomLockMinTime;
            }
         }
        private BQLEntityParamHandle _randomLockMaxTime = null;
        /// <summary>
        ///����������ʱ�䣨�룩
        /// </summary>
        public BQLEntityParamHandle RandomLockMaxTime
        {
            get
            {
                return _randomLockMaxTime;
            }
         }
        private BQLEntityParamHandle _randomPatternMinTime = null;
        /// <summary>
        ///���ģʽ��Сʱ�䣨�룩
        /// </summary>
        public BQLEntityParamHandle RandomPatternMinTime
        {
            get
            {
                return _randomPatternMinTime;
            }
         }
        private BQLEntityParamHandle _randomPatternMaxTime = null;
        /// <summary>
        ///���ģʽ���ʱ�䣨�룩
        /// </summary>
        public BQLEntityParamHandle RandomPatternMaxTime
        {
            get
            {
                return _randomPatternMaxTime;
            }
         }
        private BQLEntityParamHandle _canShootCircleFish = null;
        /// <summary>
        ///�Ƿ��ȦȦ��
        /// </summary>
        public BQLEntityParamHandle CanShootCircleFish
        {
            get
            {
                return _canShootCircleFish;
            }
         }
        private BQLEntityParamHandle _stopShootMinInterval = null;
        /// <summary>
        ///ֹͣ������С������룩
        /// </summary>
        public BQLEntityParamHandle StopShootMinInterval
        {
            get
            {
                return _stopShootMinInterval;
            }
         }
        private BQLEntityParamHandle _stopShootMaxInterval = null;
        /// <summary>
        ///ֹͣ������������룩
        /// </summary>
        public BQLEntityParamHandle StopShootMaxInterval
        {
            get
            {
                return _stopShootMaxInterval;
            }
         }
        private BQLEntityParamHandle _stopShootMinTime = null;
        /// <summary>
        ///ֹͣ������Сʱ�䣨�룩
        /// </summary>
        public BQLEntityParamHandle StopShootMinTime
        {
            get
            {
                return _stopShootMinTime;
            }
         }
        private BQLEntityParamHandle _stopShootMaxTime = null;
        /// <summary>
        ///ֹͣ�������ʱ�䣨�룩
        /// </summary>
        public BQLEntityParamHandle StopShootMaxTime
        {
            get
            {
                return _stopShootMaxTime;
            }
         }
        private BQLEntityParamHandle _straightLineMaxValidShootScope = null;
        /// <summary>
        ///�����Ч�����Χ�����أ�
        /// </summary>
        public BQLEntityParamHandle StraightLineMaxValidShootScope
        {
            get
            {
                return _straightLineMaxValidShootScope;
            }
         }
        private BQLEntityParamHandle _randomShootMinTime = null;
        /// <summary>
        ///������ڼ����Сʱ�䣨�룩
        /// </summary>
        public BQLEntityParamHandle RandomShootMinTime
        {
            get
            {
                return _randomShootMinTime;
            }
         }
        private BQLEntityParamHandle _randomShootMaxTime = null;
        /// <summary>
        ///������ڼ�����ʱ�䣨�룩
        /// </summary>
        public BQLEntityParamHandle RandomShootMaxTime
        {
            get
            {
                return _randomShootMaxTime;
            }
         }
        private BQLEntityParamHandle _probability = null;
        /// <summary>
        ///ѡ�и����õĸ���
        /// </summary>
        public BQLEntityParamHandle Probability
        {
            get
            {
                return _probability;
            }
         }
        private BQLEntityParamHandle _shootCircleMinInterval = null;
        /// <summary>
        ///��ȦȦ����С���ڼ�����룩
        /// </summary>
        public BQLEntityParamHandle ShootCircleMinInterval
        {
            get
            {
                return _shootCircleMinInterval;
            }
         }
        private BQLEntityParamHandle _shootCircleMaxInterval = null;
        /// <summary>
        ///��ȦȦ������ڼ�����룩
        /// </summary>
        public BQLEntityParamHandle ShootCircleMaxInterval
        {
            get
            {
                return _shootCircleMaxInterval;
            }
         }
        private BQLEntityParamHandle _shootCirclePriority = null;
        /// <summary>
        ///ȦȦ�����ȼ�
        /// </summary>
        public BQLEntityParamHandle ShootCirclePriority
        {
            get
            {
                return _shootCirclePriority;
            }
         }
        private BQLEntityParamHandle _circleIsFindHigherLevel = null;
        /// <summary>
        ///��ȦȦ���ʱ���Ƿ��Ҹ������ȼ�����
        /// </summary>
        public BQLEntityParamHandle CircleIsFindHigherLevel
        {
            get
            {
                return _circleIsFindHigherLevel;
            }
         }
        private BQLEntityParamHandle _circleIsCanImitatePeople = null;
        /// <summary>
        ///��ȦȦ���ʱ���Ƿ�ģ����
        /// </summary>
        public BQLEntityParamHandle CircleIsCanImitatePeople
        {
            get
            {
                return _circleIsCanImitatePeople;
            }
         }



		/// <summary>
        /// ��ʼ���������Ϣ
        /// </summary>
        /// <param name="parent">������Ϣ</param>
        /// <param name="propertyName">������</param>
        public BQL_Yqsrobotconfig(BQLEntityTableHandle parent,string propertyName) 
        :this(typeof(TestLib.Yqsrobotconfig),parent,propertyName)
        {
			
        }
        /// <summary>
        /// ��ʼ���������Ϣ
        /// </summary>
        /// <param name="parent">������Ϣ</param>
        /// <param name="propertyName">������</param>
        public BQL_Yqsrobotconfig(Type entityType,BQLEntityTableHandle parent,string propertyName) 
        :base(entityType,parent,propertyName)
        {
            _id=CreateProperty("Id");
            _createDate=CreateProperty("CreateDate");
            _lastDate=CreateProperty("LastDate");
            _enable=CreateProperty("Enable");
            _joinRoomZero=CreateProperty("JoinRoomZero");
            _joinRoomOne=CreateProperty("JoinRoomOne");
            _joinRoomTwo=CreateProperty("JoinRoomTwo");
            _joinRoomThree=CreateProperty("JoinRoomThree");
            _minPlayGameTime=CreateProperty("MinPlayGameTime");
            _maxPlayGameTime=CreateProperty("MaxPlayGameTime");
            _lockState=CreateProperty("LockState");
            _shootState=CreateProperty("ShootState");
            _randomLockMinTime=CreateProperty("RandomLockMinTime");
            _randomLockMaxTime=CreateProperty("RandomLockMaxTime");
            _randomPatternMinTime=CreateProperty("RandomPatternMinTime");
            _randomPatternMaxTime=CreateProperty("RandomPatternMaxTime");
            _canShootCircleFish=CreateProperty("CanShootCircleFish");
            _stopShootMinInterval=CreateProperty("StopShootMinInterval");
            _stopShootMaxInterval=CreateProperty("StopShootMaxInterval");
            _stopShootMinTime=CreateProperty("StopShootMinTime");
            _stopShootMaxTime=CreateProperty("StopShootMaxTime");
            _straightLineMaxValidShootScope=CreateProperty("StraightLineMaxValidShootScope");
            _randomShootMinTime=CreateProperty("RandomShootMinTime");
            _randomShootMaxTime=CreateProperty("RandomShootMaxTime");
            _probability=CreateProperty("Probability");
            _shootCircleMinInterval=CreateProperty("ShootCircleMinInterval");
            _shootCircleMaxInterval=CreateProperty("ShootCircleMaxInterval");
            _shootCirclePriority=CreateProperty("ShootCirclePriority");
            _circleIsFindHigherLevel=CreateProperty("CircleIsFindHigherLevel");
            _circleIsCanImitatePeople=CreateProperty("CircleIsCanImitatePeople");

        }
        
        /// <summary>
        /// ��ʼ���������Ϣ
        /// </summary>
        public BQL_Yqsrobotconfig() 
            :this(null,null)
        {
        }
    }
}
