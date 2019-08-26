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
        private BQLEntityParamHandle _enable = null;
        /// <summary>
        ///是否可用
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
        ///是否可以进第一个场
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
        ///是否可以进第二个场
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
        ///是否可以进第三个场
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
        ///是否可以进第四个场
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
        ///最小游戏时间（分钟）
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
        ///最大游戏时间（分钟）
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
        ///锁定模式
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
        ///射击模式
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
        ///随机锁定最小时间（秒）
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
        ///随机锁定最大时间（秒）
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
        ///随机模式最小时间（秒）
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
        ///随机模式最大时间（秒）
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
        ///是否打圈圈鱼
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
        ///停止发炮最小间隔（秒）
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
        ///停止发炮最大间隔（秒）
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
        ///停止发炮最小时间（秒）
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
        ///停止发炮最大时间（秒）
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
        ///最大有效射击范围（像素）
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
        ///随机发炮间隔最小时间（秒）
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
        ///随机发炮间隔最大时间（秒）
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
        ///选中该配置的概率
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
        ///打圈圈鱼最小发炮间隔（秒）
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
        ///打圈圈鱼最大发炮间隔（秒）
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
        ///圈圈鱼优先级
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
        ///打圈圈鱼的时候是否找更高优先级的鱼
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
        ///打圈圈鱼的时候是否模仿人
        /// </summary>
        public BQLEntityParamHandle CircleIsCanImitatePeople
        {
            get
            {
                return _circleIsCanImitatePeople;
            }
         }



		/// <summary>
        /// 初始化本类的信息
        /// </summary>
        /// <param name="parent">父表信息</param>
        /// <param name="propertyName">属性名</param>
        public BQL_Yqsrobotconfig(BQLEntityTableHandle parent,string propertyName) 
        :this(typeof(TestLib.Yqsrobotconfig),parent,propertyName)
        {
			
        }
        /// <summary>
        /// 初始化本类的信息
        /// </summary>
        /// <param name="parent">父表信息</param>
        /// <param name="propertyName">属性名</param>
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
        /// 初始化本类的信息
        /// </summary>
        public BQL_Yqsrobotconfig() 
            :this(null,null)
        {
        }
    }
}
