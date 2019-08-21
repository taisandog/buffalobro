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
        private static BQL_TEAdmin _TEAdmin = new BQL_TEAdmin();
    
        public static BQL_TEAdmin TEAdmin
        {
            get
            {
                return _TEAdmin;
            }
        }
    }

    /// <summary>
    ///  
    /// </summary>
    public partial class BQL_TEAdmin : TestApp.BQLEntity.BQL_TEBase
    {
        private BQLEntityParamHandle _name = null;
        /// <summary>
        /// �û���
        /// </summary>
        public BQLEntityParamHandle Name
        {
            get
            {
                return _name;
            }
         }



		/// <summary>
        /// ��ʼ���������Ϣ
        /// </summary>
        /// <param name="parent">������Ϣ</param>
        /// <param name="propertyName">������</param>
        public BQL_TEAdmin(BQLEntityTableHandle parent,string propertyName) 
        :this(typeof(TestApp.TEAdmin),parent,propertyName)
        {
			
        }
        /// <summary>
        /// ��ʼ���������Ϣ
        /// </summary>
        /// <param name="parent">������Ϣ</param>
        /// <param name="propertyName">������</param>
        public BQL_TEAdmin(Type entityType,BQLEntityTableHandle parent,string propertyName) 
        :base(entityType,parent,propertyName)
        {
            _name=CreateProperty("Name");

        }
        
        /// <summary>
        /// ��ʼ���������Ϣ
        /// </summary>
        public BQL_TEAdmin() 
            :this(null,null)
        {
        }
    }
}
