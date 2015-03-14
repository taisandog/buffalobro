using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Buffalo.DB.CommBase;
using Buffalo.Kernel.Defaults;
using Buffalo.DB.PropertyAttributes;
using Buffalo.DB.CommBase.BusinessBases;
namespace ManagementLib
{
	/// <summary>
    /// 
    /// </summary>
    public partial class View_Message: Buffalo.DB.CommBase.BusinessBases.ThinModelBase
    {
        ///<summary>
        ///
        ///</summary>
        protected int? _id;

        /// <summary>
        ///
        ///</summary>
        public int? Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id=value;
                OnPropertyUpdated("Id");
            }
        }
        ///<summary>
        ///
        ///</summary>
        protected bool? _state;

        /// <summary>
        ///
        ///</summary>
        public bool? State
        {
            get
            {
                return _state;
            }
            set
            {
                _state=value;
                OnPropertyUpdated("State");
            }
        }
        ///<summary>
        ///
        ///</summary>
        protected DateTime? _lastUpdate;

        /// <summary>
        ///
        ///</summary>
        public DateTime? LastUpdate
        {
            get
            {
                return _lastUpdate;
            }
            set
            {
                _lastUpdate=value;
                OnPropertyUpdated("LastUpdate");
            }
        }
        ///<summary>
        ///
        ///</summary>
        protected string _title;

        /// <summary>
        ///
        ///</summary>
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title=value;
                OnPropertyUpdated("Title");
            }
        }
        ///<summary>
        ///
        ///</summary>
        protected string _content;

        /// <summary>
        ///
        ///</summary>
        public string Content
        {
            get
            {
                return _content;
            }
            set
            {
                _content=value;
                OnPropertyUpdated("Content");
            }
        }
        ///<summary>
        ///
        ///</summary>
        protected int? _belongUser;

        /// <summary>
        ///
        ///</summary>
        public int? BelongUser
        {
            get
            {
                return _belongUser;
            }
            set
            {
                _belongUser=value;
                OnPropertyUpdated("BelongUser");
            }
        }
        ///<summary>
        ///
        ///</summary>
        protected string _userName;

        /// <summary>
        ///
        ///</summary>
        public string UserName
        {
            get
            {
                return _userName;
            }
            set
            {
                _userName=value;
                OnPropertyUpdated("UserName");
            }
        }




        private static ModelContext<View_Message> _____baseContext=new ModelContext<View_Message>();
        /// <summary>
        /// 获取查询关联类
        /// </summary>
        /// <returns></returns>
        public static ModelContext<View_Message> GetContext() 
        {
            return _____baseContext;
        }

    }
}
