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
        private static BQL_Ymrrobotfishconfig _Ymrrobotfishconfig = new BQL_Ymrrobotfishconfig();
    
        public static BQL_Ymrrobotfishconfig Ymrrobotfishconfig
        {
            get
            {
                return _Ymrrobotfishconfig;
            }
        }
    }

    /// <summary>
    ///  
    /// </summary>
    public partial class BQL_Ymrrobotfishconfig : TestLib.BQLEntity.BQL_YMRBase
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
        private BQLEntityParamHandle _fishId = null;
        /// <summary>
        ///鱼ID
        /// </summary>
        public BQLEntityParamHandle FishId
        {
            get
            {
                return _fishId;
            }
         }
        private BQLEntityParamHandle _minShootInterval = null;
        /// <summary>
        ///最小发炮间隔（秒）
        /// </summary>
        public BQLEntityParamHandle MinShootInterval
        {
            get
            {
                return _minShootInterval;
            }
         }
        private BQLEntityParamHandle _maxShootInterval = null;
        /// <summary>
        ///最大发炮间隔（秒）
        /// </summary>
        public BQLEntityParamHandle MaxShootInterval
        {
            get
            {
                return _maxShootInterval;
            }
         }
        private BQLEntityParamHandle _canShoot = null;
        /// <summary>
        ///是否可以打这条鱼
        /// </summary>
        public BQLEntityParamHandle CanShoot
        {
            get
            {
                return _canShoot;
            }
         }
        private BQLEntityParamHandle _isFindHigherLevel = null;
        /// <summary>
        ///是否找优先级更高的鱼
        /// </summary>
        public BQLEntityParamHandle IsFindHigherLevel
        {
            get
            {
                return _isFindHigherLevel;
            }
         }
        private BQLEntityParamHandle _priority = null;
        /// <summary>
        ///优先级（值越大优先级越高）
        /// </summary>
        public BQLEntityParamHandle Priority
        {
            get
            {
                return _priority;
            }
         }
        private BQLEntityParamHandle _canImitatePeople = null;
        /// <summary>
        ///是否模仿人
        /// </summary>
        public BQLEntityParamHandle CanImitatePeople
        {
            get
            {
                return _canImitatePeople;
            }
         }
        private BQLEntityParamHandle _robotConfigId = null;
        /// <summary>
        ///对应的机器人配置ID
        /// </summary>
        public BQLEntityParamHandle RobotConfigId
        {
            get
            {
                return _robotConfigId;
            }
         }
        private BQLEntityParamHandle _fishName = null;
        /// <summary>
        ///鱼名称
        /// </summary>
        public BQLEntityParamHandle FishName
        {
            get
            {
                return _fishName;
            }
         }



		/// <summary>
        /// 初始化本类的信息
        /// </summary>
        /// <param name="parent">父表信息</param>
        /// <param name="propertyName">属性名</param>
        public BQL_Ymrrobotfishconfig(BQLEntityTableHandle parent,string propertyName) 
        :this(typeof(TestLib.Ymrrobotfishconfig),parent,propertyName)
        {
			
        }
        /// <summary>
        /// 初始化本类的信息
        /// </summary>
        /// <param name="parent">父表信息</param>
        /// <param name="propertyName">属性名</param>
        public BQL_Ymrrobotfishconfig(Type entityType,BQLEntityTableHandle parent,string propertyName) 
        :base(entityType,parent,propertyName)
        {
            _id=CreateProperty("Id");
            _createDate=CreateProperty("CreateDate");
            _lastDate=CreateProperty("LastDate");
            _fishId=CreateProperty("FishId");
            _minShootInterval=CreateProperty("MinShootInterval");
            _maxShootInterval=CreateProperty("MaxShootInterval");
            _canShoot=CreateProperty("CanShoot");
            _isFindHigherLevel=CreateProperty("IsFindHigherLevel");
            _priority=CreateProperty("Priority");
            _canImitatePeople=CreateProperty("CanImitatePeople");
            _robotConfigId=CreateProperty("RobotConfigId");
            _fishName=CreateProperty("FishName");

        }
        
        /// <summary>
        /// 初始化本类的信息
        /// </summary>
        public BQL_Ymrrobotfishconfig() 
            :this(null,null)
        {
        }
    }
}
