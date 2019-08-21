using System;
using System.Data;
using System.Configuration;
using Buffalo.DB.EntityInfos;
using Buffalo.DB.BQLCommon;
using Buffalo.DB.BQLCommon.BQLConditionCommon;
using Buffalo.DB.PropertyAttributes;
namespace TestApp.BQLEntity
{

    public partial class TestDB 
    {
        private static BQL_TEBase _TEBase = new BQL_TEBase();
    
        public static BQL_TEBase TEBase
        {
            get
            {
                return _TEBase;
            }
        }
    }

    /// <summary>
    ///  
    /// </summary>
    public partial class BQL_TEBase : BQLEntityTableHandle
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
        ///��������
        /// </summary>
        public BQLEntityParamHandle CreateDate
        {
            get
            {
                return _createDate;
            }
         }



		/// <summary>
        /// ��ʼ���������Ϣ
        /// </summary>
        /// <param name="parent">������Ϣ</param>
        /// <param name="propertyName">������</param>
        public BQL_TEBase(BQLEntityTableHandle parent,string propertyName) 
        :this(typeof(TestApp.TEBase),parent,propertyName)
        {
			
        }
        /// <summary>
        /// ��ʼ���������Ϣ
        /// </summary>
        /// <param name="parent">������Ϣ</param>
        /// <param name="propertyName">������</param>
        public BQL_TEBase(Type entityType,BQLEntityTableHandle parent,string propertyName) 
        :base(entityType,parent,propertyName)
        {
            _id=CreateProperty("Id");
            _createDate=CreateProperty("CreateDate");

        }
        
        /// <summary>
        /// ��ʼ���������Ϣ
        /// </summary>
        public BQL_TEBase() 
            :this(null,null)
        {
        }
    }
}
