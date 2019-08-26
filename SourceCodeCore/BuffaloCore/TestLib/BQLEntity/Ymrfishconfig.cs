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
        private static BQL_Ymrfishconfig _Ymrfishconfig = new BQL_Ymrfishconfig();
    
        public static BQL_Ymrfishconfig Ymrfishconfig
        {
            get
            {
                return _Ymrfishconfig;
            }
        }
    }

    /// <summary>
    ///  
    /// </summary>
    public partial class BQL_Ymrfishconfig : TestLib.BQLEntity.BQL_YMRBase
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
        private BQLEntityParamHandle _fishID = null;
        /// <summary>
        ///鱼编号
        /// </summary>
        public BQLEntityParamHandle FishID
        {
            get
            {
                return _fishID;
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
        private BQLEntityParamHandle _minScore = null;
        /// <summary>
        ///最小分(倍)
        /// </summary>
        public BQLEntityParamHandle MinScore
        {
            get
            {
                return _minScore;
            }
         }
        private BQLEntityParamHandle _maxScore = null;
        /// <summary>
        ///最大分(倍)
        /// </summary>
        public BQLEntityParamHandle MaxScore
        {
            get
            {
                return _maxScore;
            }
         }
        private BQLEntityParamHandle _minNumber = null;
        /// <summary>
        ///最小数量
        /// </summary>
        public BQLEntityParamHandle MinNumber
        {
            get
            {
                return _minNumber;
            }
         }
        private BQLEntityParamHandle _maxNumber = null;
        /// <summary>
        ///最大数量
        /// </summary>
        public BQLEntityParamHandle MaxNumber
        {
            get
            {
                return _maxNumber;
            }
         }
        private BQLEntityParamHandle _createOdds = null;
        /// <summary>
        ///出现几率
        /// </summary>
        public BQLEntityParamHandle CreateOdds
        {
            get
            {
                return _createOdds;
            }
         }
        private BQLEntityParamHandle _realOdds = null;
        /// <summary>
        ///实际几率
        /// </summary>
        public BQLEntityParamHandle RealOdds
        {
            get
            {
                return _realOdds;
            }
         }
        private BQLEntityParamHandle _props = null;
        /// <summary>
        ///道具鱼
        /// </summary>
        public BQLEntityParamHandle Props
        {
            get
            {
                return _props;
            }
         }
        private BQLEntityParamHandle _isRainbow = null;
        /// <summary>
        ///是否彩虹
        /// </summary>
        public BQLEntityParamHandle IsRainbow
        {
            get
            {
                return _isRainbow;
            }
         }
        private BQLEntityParamHandle _isEnable = null;
        /// <summary>
        ///是否启用
        /// </summary>
        public BQLEntityParamHandle IsEnable
        {
            get
            {
                return _isEnable;
            }
         }
        private BQLEntityParamHandle _isUploadRankingList = null;
        /// <summary>
        ///是否上传到排行榜
        /// </summary>
        public BQLEntityParamHandle IsUploadRankingList
        {
            get
            {
                return _isUploadRankingList;
            }
         }



		/// <summary>
        /// 初始化本类的信息
        /// </summary>
        /// <param name="parent">父表信息</param>
        /// <param name="propertyName">属性名</param>
        public BQL_Ymrfishconfig(BQLEntityTableHandle parent,string propertyName) 
        :this(typeof(TestLib.Ymrfishconfig),parent,propertyName)
        {
			
        }
        /// <summary>
        /// 初始化本类的信息
        /// </summary>
        /// <param name="parent">父表信息</param>
        /// <param name="propertyName">属性名</param>
        public BQL_Ymrfishconfig(Type entityType,BQLEntityTableHandle parent,string propertyName) 
        :base(entityType,parent,propertyName)
        {
            _id=CreateProperty("Id");
            _createDate=CreateProperty("CreateDate");
            _lastDate=CreateProperty("LastDate");
            _fishID=CreateProperty("FishID");
            _fishName=CreateProperty("FishName");
            _minScore=CreateProperty("MinScore");
            _maxScore=CreateProperty("MaxScore");
            _minNumber=CreateProperty("MinNumber");
            _maxNumber=CreateProperty("MaxNumber");
            _createOdds=CreateProperty("CreateOdds");
            _realOdds=CreateProperty("RealOdds");
            _props=CreateProperty("Props");
            _isRainbow=CreateProperty("IsRainbow");
            _isEnable=CreateProperty("IsEnable");
            _isUploadRankingList=CreateProperty("IsUploadRankingList");

        }
        
        /// <summary>
        /// 初始化本类的信息
        /// </summary>
        public BQL_Ymrfishconfig() 
            :this(null,null)
        {
        }
    }
}
