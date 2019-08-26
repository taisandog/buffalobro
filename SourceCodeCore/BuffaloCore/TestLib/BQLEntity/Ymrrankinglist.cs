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
        private static BQL_Ymrrankinglist _Ymrrankinglist = new BQL_Ymrrankinglist();
    
        public static BQL_Ymrrankinglist Ymrrankinglist
        {
            get
            {
                return _Ymrrankinglist;
            }
        }
    }

    /// <summary>
    ///  
    /// </summary>
    public partial class BQL_Ymrrankinglist : TestLib.BQLEntity.BQL_YMRBase
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
        private BQLEntityParamHandle _multipleRate = null;
        /// <summary>
        ///倍率
        /// </summary>
        public BQLEntityParamHandle MultipleRate
        {
            get
            {
                return _multipleRate;
            }
         }
        private BQLEntityParamHandle _score = null;
        /// <summary>
        ///分数
        /// </summary>
        public BQLEntityParamHandle Score
        {
            get
            {
                return _score;
            }
         }
        private BQLEntityParamHandle _fishName = null;
        /// <summary>
        ///鱼的名字
        /// </summary>
        public BQLEntityParamHandle FishName
        {
            get
            {
                return _fishName;
            }
         }
        private BQLEntityParamHandle _userId = null;
        /// <summary>
        ///用户id
        /// </summary>
        public BQLEntityParamHandle UserId
        {
            get
            {
                return _userId;
            }
         }
        private BQLEntityParamHandle _roomName = null;
        /// <summary>
        ///房间名
        /// </summary>
        public BQLEntityParamHandle RoomName
        {
            get
            {
                return _roomName;
            }
         }



		/// <summary>
        /// 初始化本类的信息
        /// </summary>
        /// <param name="parent">父表信息</param>
        /// <param name="propertyName">属性名</param>
        public BQL_Ymrrankinglist(BQLEntityTableHandle parent,string propertyName) 
        :this(typeof(TestLib.Ymrrankinglist),parent,propertyName)
        {
			
        }
        /// <summary>
        /// 初始化本类的信息
        /// </summary>
        /// <param name="parent">父表信息</param>
        /// <param name="propertyName">属性名</param>
        public BQL_Ymrrankinglist(Type entityType,BQLEntityTableHandle parent,string propertyName) 
        :base(entityType,parent,propertyName)
        {
            _id=CreateProperty("Id");
            _createDate=CreateProperty("CreateDate");
            _lastDate=CreateProperty("LastDate");
            _multipleRate=CreateProperty("MultipleRate");
            _score=CreateProperty("Score");
            _fishName=CreateProperty("FishName");
            _userId=CreateProperty("UserId");
            _roomName=CreateProperty("RoomName");

        }
        
        /// <summary>
        /// 初始化本类的信息
        /// </summary>
        public BQL_Ymrrankinglist() 
            :this(null,null)
        {
        }
    }
}
