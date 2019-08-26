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
        private BQLEntityParamHandle _fishId = null;
        /// <summary>
        ///��ID
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
        ///��С���ڼ�����룩
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
        ///����ڼ�����룩
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
        ///�Ƿ���Դ�������
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
        ///�Ƿ������ȼ����ߵ���
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
        ///���ȼ���ֵԽ�����ȼ�Խ�ߣ�
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
        ///�Ƿ�ģ����
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
        ///��Ӧ�Ļ���������ID
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
        ///������
        /// </summary>
        public BQLEntityParamHandle FishName
        {
            get
            {
                return _fishName;
            }
         }



		/// <summary>
        /// ��ʼ���������Ϣ
        /// </summary>
        /// <param name="parent">������Ϣ</param>
        /// <param name="propertyName">������</param>
        public BQL_Ymrrobotfishconfig(BQLEntityTableHandle parent,string propertyName) 
        :this(typeof(TestLib.Ymrrobotfishconfig),parent,propertyName)
        {
			
        }
        /// <summary>
        /// ��ʼ���������Ϣ
        /// </summary>
        /// <param name="parent">������Ϣ</param>
        /// <param name="propertyName">������</param>
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
        /// ��ʼ���������Ϣ
        /// </summary>
        public BQL_Ymrrobotfishconfig() 
            :this(null,null)
        {
        }
    }
}
