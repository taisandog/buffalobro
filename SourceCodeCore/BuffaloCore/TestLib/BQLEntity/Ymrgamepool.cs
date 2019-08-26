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
        private static BQL_Ymrgamepool _Ymrgamepool = new BQL_Ymrgamepool();
    
        public static BQL_Ymrgamepool Ymrgamepool
        {
            get
            {
                return _Ymrgamepool;
            }
        }
    }

    /// <summary>
    ///  
    /// </summary>
    public partial class BQL_Ymrgamepool : TestLib.BQLEntity.BQL_YMRBase
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
        private BQLEntityParamHandle _level = null;
        /// <summary>
        ///����
        /// </summary>
        public BQLEntityParamHandle Level
        {
            get
            {
                return _level;
            }
         }
        private BQLEntityParamHandle _offsetScore = null;
        /// <summary>
        ///ƫ��ķ���,����ƫ����ٷֿ�ʼ��������
        /// </summary>
        public BQLEntityParamHandle OffsetScore
        {
            get
            {
                return _offsetScore;
            }
         }
        private BQLEntityParamHandle _poolScore3 = null;
        /// <summary>
        ///�������ӷ���
        /// </summary>
        public BQLEntityParamHandle PoolScore3
        {
            get
            {
                return _poolScore3;
            }
         }
        private BQLEntityParamHandle _poolScore2 = null;
        /// <summary>
        ///�������ӷ���
        /// </summary>
        public BQLEntityParamHandle PoolScore2
        {
            get
            {
                return _poolScore2;
            }
         }
        private BQLEntityParamHandle _poolScore2Rake = null;
        /// <summary>
        ///�������ӱ���,���֮
        /// </summary>
        public BQLEntityParamHandle PoolScore2Rake
        {
            get
            {
                return _poolScore2Rake;
            }
         }
        private BQLEntityParamHandle _poolScore2Temp = null;
        /// <summary>
        ///�������ӷ�����ʱ
        /// </summary>
        public BQLEntityParamHandle PoolScore2Temp
        {
            get
            {
                return _poolScore2Temp;
            }
         }
        private BQLEntityParamHandle _nowScore = null;
        /// <summary>
        ///��ǰ��¼�ķ���
        /// </summary>
        public BQLEntityParamHandle NowScore
        {
            get
            {
                return _nowScore;
            }
         }
        private BQLEntityParamHandle _needScore = null;
        /// <summary>
        ///Ҫ��ɵķ���
        /// </summary>
        public BQLEntityParamHandle NeedScore
        {
            get
            {
                return _needScore;
            }
         }
        private BQLEntityParamHandle _max = null;
        /// <summary>
        ///��ɵ�Ŀ�����
        /// </summary>
        public BQLEntityParamHandle Max
        {
            get
            {
                return _max;
            }
         }
        private BQLEntityParamHandle _taget = null;
        /// <summary>
        ///Ҫ��ˮ����
        /// </summary>
        public BQLEntityParamHandle Taget
        {
            get
            {
                return _taget;
            }
         }
        private BQLEntityParamHandle _realScore = null;
        /// <summary>
        ///�ܳ�ɷ�
        /// </summary>
        public BQLEntityParamHandle RealScore
        {
            get
            {
                return _realScore;
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
        private BQLEntityParamHandle _offlineTime = null;
        /// <summary>
        ///�������ʱ��
        /// </summary>
        public BQLEntityParamHandle OfflineTime
        {
            get
            {
                return _offlineTime;
            }
         }
        private BQLEntityParamHandle _poolScoreFullScreenBomb = null;
        /// <summary>
        ///ȫ��ը������
        /// </summary>
        public BQLEntityParamHandle PoolScoreFullScreenBomb
        {
            get
            {
                return _poolScoreFullScreenBomb;
            }
         }
        private BQLEntityParamHandle _poolScoreFullScreenBombRake = null;
        /// <summary>
        ///ȫ��ը��������ǧ��֮
        /// </summary>
        public BQLEntityParamHandle PoolScoreFullScreenBombRake
        {
            get
            {
                return _poolScoreFullScreenBombRake;
            }
         }
        private BQLEntityParamHandle _maxPay = null;
        /// <summary>
        ///�����Ը���ֵ(��λ���)
        /// </summary>
        public BQLEntityParamHandle MaxPay
        {
            get
            {
                return _maxPay;
            }
         }
        private BQLEntityParamHandle _redEnvelopesStartTime = null;
        /// <summary>
        ///�������ʼʱ���
        /// </summary>
        public BQLEntityParamHandle RedEnvelopesStartTime
        {
            get
            {
                return _redEnvelopesStartTime;
            }
         }
        private BQLEntityParamHandle _redEnvelopesEndTime = null;
        /// <summary>
        ///���������ʱ���
        /// </summary>
        public BQLEntityParamHandle RedEnvelopesEndTime
        {
            get
            {
                return _redEnvelopesEndTime;
            }
         }
        private BQLEntityParamHandle _redEnvelopesNowTime = null;
        /// <summary>
        ///��ǰ�����ʱ��
        /// </summary>
        public BQLEntityParamHandle RedEnvelopesNowTime
        {
            get
            {
                return _redEnvelopesNowTime;
            }
         }
        private BQLEntityParamHandle _redEnvelopesCollectTime = null;
        /// <summary>
        ///������ռ�ʱ�䣨�룩
        /// </summary>
        public BQLEntityParamHandle RedEnvelopesCollectTime
        {
            get
            {
                return _redEnvelopesCollectTime;
            }
         }
        private BQLEntityParamHandle _redEnvelopesMinBullet = null;
        /// <summary>
        ///�����������С������
        /// </summary>
        public BQLEntityParamHandle RedEnvelopesMinBullet
        {
            get
            {
                return _redEnvelopesMinBullet;
            }
         }
        private BQLEntityParamHandle _redEnvelopesPool = null;
        /// <summary>
        ///�������
        /// </summary>
        public BQLEntityParamHandle RedEnvelopesPool
        {
            get
            {
                return _redEnvelopesPool;
            }
         }
        private BQLEntityParamHandle _redEnvelopesPoolRake = null;
        /// <summary>
        ///������س�ˮ������ǧ��֮��
        /// </summary>
        public BQLEntityParamHandle RedEnvelopesPoolRake
        {
            get
            {
                return _redEnvelopesPoolRake;
            }
         }
        private BQLEntityParamHandle _redEnvelopesData = null;
        /// <summary>
        ///���������
        /// </summary>
        public BQLEntityParamHandle RedEnvelopesData
        {
            get
            {
                return _redEnvelopesData;
            }
         }



		/// <summary>
        /// ��ʼ���������Ϣ
        /// </summary>
        /// <param name="parent">������Ϣ</param>
        /// <param name="propertyName">������</param>
        public BQL_Ymrgamepool(BQLEntityTableHandle parent,string propertyName) 
        :this(typeof(TestLib.Ymrgamepool),parent,propertyName)
        {
			
        }
        /// <summary>
        /// ��ʼ���������Ϣ
        /// </summary>
        /// <param name="parent">������Ϣ</param>
        /// <param name="propertyName">������</param>
        public BQL_Ymrgamepool(Type entityType,BQLEntityTableHandle parent,string propertyName) 
        :base(entityType,parent,propertyName)
        {
            _id=CreateProperty("Id");
            _level=CreateProperty("Level");
            _offsetScore=CreateProperty("OffsetScore");
            _poolScore3=CreateProperty("PoolScore3");
            _poolScore2=CreateProperty("PoolScore2");
            _poolScore2Rake=CreateProperty("PoolScore2Rake");
            _poolScore2Temp=CreateProperty("PoolScore2Temp");
            _nowScore=CreateProperty("NowScore");
            _needScore=CreateProperty("NeedScore");
            _max=CreateProperty("Max");
            _taget=CreateProperty("Taget");
            _realScore=CreateProperty("RealScore");
            _createDate=CreateProperty("CreateDate");
            _lastDate=CreateProperty("LastDate");
            _offlineTime=CreateProperty("OfflineTime");
            _poolScoreFullScreenBomb=CreateProperty("PoolScoreFullScreenBomb");
            _poolScoreFullScreenBombRake=CreateProperty("PoolScoreFullScreenBombRake");
            _maxPay=CreateProperty("MaxPay");
            _redEnvelopesStartTime=CreateProperty("RedEnvelopesStartTime");
            _redEnvelopesEndTime=CreateProperty("RedEnvelopesEndTime");
            _redEnvelopesNowTime=CreateProperty("RedEnvelopesNowTime");
            _redEnvelopesCollectTime=CreateProperty("RedEnvelopesCollectTime");
            _redEnvelopesMinBullet=CreateProperty("RedEnvelopesMinBullet");
            _redEnvelopesPool=CreateProperty("RedEnvelopesPool");
            _redEnvelopesPoolRake=CreateProperty("RedEnvelopesPoolRake");
            _redEnvelopesData=CreateProperty("RedEnvelopesData");

        }
        
        /// <summary>
        /// ��ʼ���������Ϣ
        /// </summary>
        public BQL_Ymrgamepool() 
            :this(null,null)
        {
        }
    }
}
