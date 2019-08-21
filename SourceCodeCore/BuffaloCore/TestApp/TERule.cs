using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Buffalo.DB.CommBase;
using Buffalo.Kernel.Defaults;
using Buffalo.DB.PropertyAttributes;
using Buffalo.DB.CommBase.BusinessBases;
namespace TestApp
{
	/// <summary>
    /// 
    /// </summary>
    public partial class TERule: TestApp.TEBase
    {
        ///<summary>
        ///
        ///</summary>
        protected DateTime? _datetime;

        /// <summary>
        ///
        ///</summary>
        public virtual DateTime? Datetime
        {
            get{ return _datetime;}
            set{ _datetime=value;}
        }
        ///<summary>
        ///
        ///</summary>
        protected string _grouprule;

        /// <summary>
        ///
        ///</summary>
        public virtual string Grouprule
        {
            get{ return _grouprule;}
            set{ _grouprule=value;}
        }





    }
}
