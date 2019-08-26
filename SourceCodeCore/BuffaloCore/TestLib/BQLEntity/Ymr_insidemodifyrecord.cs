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
        private static BQL_Ymr_insidemodifyrecord _Ymr_insidemodifyrecord = new BQL_Ymr_insidemodifyrecord();
    
        public static BQL_Ymr_insidemodifyrecord Ymr_insidemodifyrecord
        {
            get
            {
                return _Ymr_insidemodifyrecord;
            }
        }
    }

    /// <summary>
    ///  
    /// </summary>
    public partial class BQL_Ymr_insidemodifyrecord : TestLib.BQLEntity.BQL_YMRBase
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
        private BQLEntityParamHandle _afterChange = null;
        /// <summary>
        ///���ݸı��json��
        /// </summary>
        public BQLEntityParamHandle AfterChange
        {
            get
            {
                return _afterChange;
            }
         }
        private BQLEntityParamHandle _beforeChange = null;
        /// <summary>
        ///���ݸı�ǰ��json��
        /// </summary>
        public BQLEntityParamHandle BeforeChange
        {
            get
            {
                return _beforeChange;
            }
         }



		/// <summary>
        /// ��ʼ���������Ϣ
        /// </summary>
        /// <param name="parent">������Ϣ</param>
        /// <param name="propertyName">������</param>
        public BQL_Ymr_insidemodifyrecord(BQLEntityTableHandle parent,string propertyName) 
        :this(typeof(TestLib.Ymr_insidemodifyrecord),parent,propertyName)
        {
			
        }
        /// <summary>
        /// ��ʼ���������Ϣ
        /// </summary>
        /// <param name="parent">������Ϣ</param>
        /// <param name="propertyName">������</param>
        public BQL_Ymr_insidemodifyrecord(Type entityType,BQLEntityTableHandle parent,string propertyName) 
        :base(entityType,parent,propertyName)
        {
            _id=CreateProperty("Id");
            _createDate=CreateProperty("CreateDate");
            _lastDate=CreateProperty("LastDate");
            _afterChange=CreateProperty("AfterChange");
            _beforeChange=CreateProperty("BeforeChange");

        }
        
        /// <summary>
        /// ��ʼ���������Ϣ
        /// </summary>
        public BQL_Ymr_insidemodifyrecord() 
            :this(null,null)
        {
        }
    }
}
