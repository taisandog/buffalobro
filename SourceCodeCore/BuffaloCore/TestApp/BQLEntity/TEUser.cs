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
    ///  用户
    /// </summary>
    public partial class BQL_TEUser : TestApp.BQLEntity.BQL_TEBase
    {
        private BQLEntityParamHandle _name = null;
        /// <summary>
        /// 用户名
        /// </summary>
        public BQLEntityParamHandle Name
        {
            get
            {
                return _name;
            }
         }



		/// <summary>
        /// 初始化本类的信息
        /// </summary>
        /// <param name="parent">父表信息</param>
        /// <param name="propertyName">属性名</param>
        public BQL_TEUser(BQLEntityTableHandle parent,string propertyName) 
        :this(typeof(TestApp.TEUser),parent,propertyName)
        {
			
        }
        /// <summary>
        /// 初始化本类的信息
        /// </summary>
        /// <param name="parent">父表信息</param>
        /// <param name="propertyName">属性名</param>
        public BQL_TEUser(Type entityType,BQLEntityTableHandle parent,string propertyName) 
        :base(entityType,parent,propertyName)
        {
            _name=CreateProperty("Name");

        }
        
        /// <summary>
        /// 初始化本类的信息
        /// </summary>
        public BQL_TEUser() 
            :this(null,null)
        {
        }
    }
}
