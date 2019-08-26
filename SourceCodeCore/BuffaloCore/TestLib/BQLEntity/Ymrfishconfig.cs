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
        private BQLEntityParamHandle _fishID = null;
        /// <summary>
        ///����
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
        ///������
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
        ///��С��(��)
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
        ///����(��)
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
        ///��С����
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
        ///�������
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
        ///���ּ���
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
        ///ʵ�ʼ���
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
        ///������
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
        ///�Ƿ�ʺ�
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
        ///�Ƿ�����
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
        ///�Ƿ��ϴ������а�
        /// </summary>
        public BQLEntityParamHandle IsUploadRankingList
        {
            get
            {
                return _isUploadRankingList;
            }
         }



		/// <summary>
        /// ��ʼ���������Ϣ
        /// </summary>
        /// <param name="parent">������Ϣ</param>
        /// <param name="propertyName">������</param>
        public BQL_Ymrfishconfig(BQLEntityTableHandle parent,string propertyName) 
        :this(typeof(TestLib.Ymrfishconfig),parent,propertyName)
        {
			
        }
        /// <summary>
        /// ��ʼ���������Ϣ
        /// </summary>
        /// <param name="parent">������Ϣ</param>
        /// <param name="propertyName">������</param>
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
        /// ��ʼ���������Ϣ
        /// </summary>
        public BQL_Ymrfishconfig() 
            :this(null,null)
        {
        }
    }
}
