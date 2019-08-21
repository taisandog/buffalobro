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
        private static BQL_TEUser _TEUser = new BQL_TEUser();
    
        public static BQL_TEUser TEUser
        {
            get
            {
                return _TEUser;
            }
        }
    }

    /// <summary>
    ///  �û�
    /// </summary>
    public partial class BQL_TEUser : TestApp.BQLEntity.BQL_TEBase
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
        public BQL_TEUser(BQLEntityTableHandle parent,string propertyName) 
        :this(typeof(TestApp.TEUser),parent,propertyName)
        {
			
        }
        /// <summary>
        /// ��ʼ���������Ϣ
        /// </summary>
        /// <param name="parent">������Ϣ</param>
        /// <param name="propertyName">������</param>
        public BQL_TEUser(Type entityType,BQLEntityTableHandle parent,string propertyName) 
        :base(entityType,parent,propertyName)
        {
            _name=CreateProperty("Name");

        }
        
        /// <summary>
        /// ��ʼ���������Ϣ
        /// </summary>
        public BQL_TEUser() 
            :this(null,null)
        {
        }
    }
}
