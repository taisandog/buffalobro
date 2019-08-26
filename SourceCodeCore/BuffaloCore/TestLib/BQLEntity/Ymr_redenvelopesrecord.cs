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
        private static BQL_Ymr_redenvelopesrecord _Ymr_redenvelopesrecord = new BQL_Ymr_redenvelopesrecord();
    
        public static BQL_Ymr_redenvelopesrecord Ymr_redenvelopesrecord
        {
            get
            {
                return _Ymr_redenvelopesrecord;
            }
        }
    }

    /// <summary>
    ///  
    /// </summary>
    public partial class BQL_Ymr_redenvelopesrecord : TestLib.BQLEntity.BQL_YMRBase
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
        private BQLEntityParamHandle _alreadyRobbedUserIDs = null;
        /// <summary>
        ///�Ѿ��������id�б�
        /// </summary>
        public BQLEntityParamHandle AlreadyRobbedUserIDs
        {
            get
            {
                return _alreadyRobbedUserIDs;
            }
         }
        private BQLEntityParamHandle _noRobberyUserIDs = null;
        /// <summary>
        ///δ�������id�б�
        /// </summary>
        public BQLEntityParamHandle NoRobberyUserIDs
        {
            get
            {
                return _noRobberyUserIDs;
            }
         }
        private BQLEntityParamHandle _redEnvelopesMoney = null;
        /// <summary>
        ///���а����
        /// </summary>
        public BQLEntityParamHandle RedEnvelopesMoney
        {
            get
            {
                return _redEnvelopesMoney;
            }
         }
        private BQLEntityParamHandle _redEnvelopesSumGold = null;
        /// <summary>
        ///����ܽ��
        /// </summary>
        public BQLEntityParamHandle RedEnvelopesSumGold
        {
            get
            {
                return _redEnvelopesSumGold;
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



		/// <summary>
        /// ��ʼ���������Ϣ
        /// </summary>
        /// <param name="parent">������Ϣ</param>
        /// <param name="propertyName">������</param>
        public BQL_Ymr_redenvelopesrecord(BQLEntityTableHandle parent,string propertyName) 
        :this(typeof(TestLib.Ymr_redenvelopesrecord),parent,propertyName)
        {
			
        }
        /// <summary>
        /// ��ʼ���������Ϣ
        /// </summary>
        /// <param name="parent">������Ϣ</param>
        /// <param name="propertyName">������</param>
        public BQL_Ymr_redenvelopesrecord(Type entityType,BQLEntityTableHandle parent,string propertyName) 
        :base(entityType,parent,propertyName)
        {
            _id=CreateProperty("Id");
            _createDate=CreateProperty("CreateDate");
            _lastDate=CreateProperty("LastDate");
            _alreadyRobbedUserIDs=CreateProperty("AlreadyRobbedUserIDs");
            _noRobberyUserIDs=CreateProperty("NoRobberyUserIDs");
            _redEnvelopesMoney=CreateProperty("RedEnvelopesMoney");
            _redEnvelopesSumGold=CreateProperty("RedEnvelopesSumGold");
            _roomLevel=CreateProperty("RoomLevel");

        }
        
        /// <summary>
        /// ��ʼ���������Ϣ
        /// </summary>
        public BQL_Ymr_redenvelopesrecord() 
            :this(null,null)
        {
        }
    }
}
