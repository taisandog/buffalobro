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
        private BQLEntityParamHandle _multipleRate = null;
        /// <summary>
        ///����
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
        ///����
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
        ///�������
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
        ///�û�id
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
        ///������
        /// </summary>
        public BQLEntityParamHandle RoomName
        {
            get
            {
                return _roomName;
            }
         }



		/// <summary>
        /// ��ʼ���������Ϣ
        /// </summary>
        /// <param name="parent">������Ϣ</param>
        /// <param name="propertyName">������</param>
        public BQL_Ymrrankinglist(BQLEntityTableHandle parent,string propertyName) 
        :this(typeof(TestLib.Ymrrankinglist),parent,propertyName)
        {
			
        }
        /// <summary>
        /// ��ʼ���������Ϣ
        /// </summary>
        /// <param name="parent">������Ϣ</param>
        /// <param name="propertyName">������</param>
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
        /// ��ʼ���������Ϣ
        /// </summary>
        public BQL_Ymrrankinglist() 
            :this(null,null)
        {
        }
    }
}
