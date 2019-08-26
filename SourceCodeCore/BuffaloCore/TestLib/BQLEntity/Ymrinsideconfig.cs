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
        private static BQL_Ymrinsideconfig _Ymrinsideconfig = new BQL_Ymrinsideconfig();
    
        public static BQL_Ymrinsideconfig Ymrinsideconfig
        {
            get
            {
                return _Ymrinsideconfig;
            }
        }
    }

    /// <summary>
    ///  
    /// </summary>
    public partial class BQL_Ymrinsideconfig : TestLib.BQLEntity.BQL_YMRBase
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
        private BQLEntityParamHandle _userIds = null;
        /// <summary>
        ///�û�id���������ö��Ÿ���
        /// </summary>
        public BQLEntityParamHandle UserIds
        {
            get
            {
                return _userIds;
            }
         }
        private BQLEntityParamHandle _multiples = null;
        /// <summary>
        ///��������
        /// </summary>
        public BQLEntityParamHandle Multiples
        {
            get
            {
                return _multiples;
            }
         }
        private BQLEntityParamHandle _minMultiple = null;
        /// <summary>
        ///��С����
        /// </summary>
        public BQLEntityParamHandle MinMultiple
        {
            get
            {
                return _minMultiple;
            }
         }
        private BQLEntityParamHandle _isEnables = null;
        /// <summary>
        ///�Ƿ�����
        /// </summary>
        public BQLEntityParamHandle IsEnables
        {
            get
            {
                return _isEnables;
            }
         }
        private BQLEntityParamHandle _isInside = null;
        /// <summary>
        ///�Ƿ�Ϊ�ڲ��˺�
        /// </summary>
        public BQLEntityParamHandle IsInside
        {
            get
            {
                return _isInside;
            }
         }
        private BQLEntityParamHandle _independentRoom = null;
        /// <summary>
        ///�Ƿ����ö�������
        /// </summary>
        public BQLEntityParamHandle IndependentRoom
        {
            get
            {
                return _independentRoom;
            }
         }



		/// <summary>
        /// ��ʼ���������Ϣ
        /// </summary>
        /// <param name="parent">������Ϣ</param>
        /// <param name="propertyName">������</param>
        public BQL_Ymrinsideconfig(BQLEntityTableHandle parent,string propertyName) 
        :this(typeof(TestLib.Ymrinsideconfig),parent,propertyName)
        {
			
        }
        /// <summary>
        /// ��ʼ���������Ϣ
        /// </summary>
        /// <param name="parent">������Ϣ</param>
        /// <param name="propertyName">������</param>
        public BQL_Ymrinsideconfig(Type entityType,BQLEntityTableHandle parent,string propertyName) 
        :base(entityType,parent,propertyName)
        {
            _id=CreateProperty("Id");
            _createDate=CreateProperty("CreateDate");
            _lastDate=CreateProperty("LastDate");
            _userIds=CreateProperty("UserIds");
            _multiples=CreateProperty("Multiples");
            _minMultiple=CreateProperty("MinMultiple");
            _isEnables=CreateProperty("IsEnables");
            _isInside=CreateProperty("IsInside");
            _independentRoom=CreateProperty("IndependentRoom");

        }
        
        /// <summary>
        /// ��ʼ���������Ϣ
        /// </summary>
        public BQL_Ymrinsideconfig() 
            :this(null,null)
        {
        }
    }
}
