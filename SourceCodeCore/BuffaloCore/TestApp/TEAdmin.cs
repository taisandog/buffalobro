using System;
using System.Collections.Generic;
using System.Text;

using Buffalo.DB.CommBase;
using Buffalo.Kernel.Defaults;
using Buffalo.DB.PropertyAttributes;
using System.Data;
using Buffalo.DB.CommBase.BusinessBases;
namespace TestApp
{
    public partial class TEAdmin : TEBase
    {
        /// <summary>
        ///  用户名
        /// </summary>
        protected string _name;
        /// <summary>
        ///  用户名
        /// </summary>
        public virtual string Name
        {
            get { return _name; }
            set { _name = value; }
        }

    }
}
