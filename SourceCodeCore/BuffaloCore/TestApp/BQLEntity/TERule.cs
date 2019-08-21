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
        private static BQL_TERule _TERule = new BQL_TERule();
    
        public static BQL_TERule TERule
        {
            get
            {
                return _TERule;
            }
        }
    }

    /// <summary>
    ///   
    /// </summary>
    public partial class BQL_TERule : TestApp.BQLEntity.BQL_TEBase
    {
        private BQLEntityParamHandle _grouprule = null;
        /// <summary>
        ///
        /// </summary>
        public BQLEntityParamHandle Grouprule
        {
            get
            {
                return _grouprule;
            }
         }



		/// <summary>
        /// 初始化本类的信息
        /// </summary>
        /// <param name="parent">父表信息</param>
        /// <param name="propertyName">属性名</param>
        public BQL_TERule(BQLEntityTableHandle parent,string propertyName) 
        :this(typeof(TestApp.TERule),parent,propertyName)
        {
			
        }
        /// <summary>
        /// 初始化本类的信息
        /// </summary>
        /// <param name="parent">父表信息</param>
        /// <param name="propertyName">属性名</param>
        public BQL_TERule(Type entityType,BQLEntityTableHandle parent,string propertyName) 
        :base(entityType,parent,propertyName)
        {
            _grouprule=CreateProperty("Grouprule");

        }
        
        /// <summary>
        /// 初始化本类的信息
        /// </summary>
        public BQL_TERule() 
            :this(null,null)
        {
        }
    }
}
