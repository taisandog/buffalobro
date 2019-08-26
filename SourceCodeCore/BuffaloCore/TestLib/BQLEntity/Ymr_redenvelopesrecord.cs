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
        private BQLEntityParamHandle _alreadyRobbedUserIDs = null;
        /// <summary>
        ///已经抢包玩家id列表
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
        ///未抢包玩家id列表
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
        ///所有包金额
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
        ///红包总金额
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
        ///房间等级
        /// </summary>
        public BQLEntityParamHandle RoomLevel
        {
            get
            {
                return _roomLevel;
            }
         }



		/// <summary>
        /// 初始化本类的信息
        /// </summary>
        /// <param name="parent">父表信息</param>
        /// <param name="propertyName">属性名</param>
        public BQL_Ymr_redenvelopesrecord(BQLEntityTableHandle parent,string propertyName) 
        :this(typeof(TestLib.Ymr_redenvelopesrecord),parent,propertyName)
        {
			
        }
        /// <summary>
        /// 初始化本类的信息
        /// </summary>
        /// <param name="parent">父表信息</param>
        /// <param name="propertyName">属性名</param>
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
        /// 初始化本类的信息
        /// </summary>
        public BQL_Ymr_redenvelopesrecord() 
            :this(null,null)
        {
        }
    }
}
