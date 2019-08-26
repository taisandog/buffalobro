using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Buffalo.DB.CommBase;
using Buffalo.Kernel.Defaults;
using Buffalo.DB.PropertyAttributes;
using Buffalo.DB.CommBase.BusinessBases;
namespace TestLib
{
	/// <summary>
    /// 
    /// </summary>
    public partial class Gduser: TestLib.YMRBase
    {
        ///<summary>
        ///ID
        ///</summary>
        protected int _id;

        /// <summary>
        ///ID
        ///</summary>
        public virtual int Id
        {
            get{ return _id;}
            set{ _id=value;}
        }
        ///<summary>
        ///玩家ID
        ///</summary>
        protected int? _userId;

        /// <summary>
        ///玩家ID
        ///</summary>
        public virtual int? UserId
        {
            get{ return _userId;}
            set{ _userId=value;}
        }
        ///<summary>
        ///用户名
        ///</summary>
        protected string _name;

        /// <summary>
        ///用户名
        ///</summary>
        public virtual string Name
        {
            get{ return _name;}
            set{ _name=value;}
        }
        ///<summary>
        ///昵称
        ///</summary>
        protected string _nickName;

        /// <summary>
        ///昵称
        ///</summary>
        public virtual string NickName
        {
            get{ return _nickName;}
            set{ _nickName=value;}
        }
        ///<summary>
        ///用户标识
        ///</summary>
        protected string _token;

        /// <summary>
        ///用户标识
        ///</summary>
        public virtual string Token
        {
            get{ return _token;}
            set{ _token=value;}
        }
        ///<summary>
        ///推荐人
        ///</summary>
        protected int? _agents;

        /// <summary>
        ///推荐人
        ///</summary>
        public virtual int? Agents
        {
            get{ return _agents;}
            set{ _agents=value;}
        }
        ///<summary>
        ///是否已经登入
        ///</summary>
        protected bool? _isLoginin;

        /// <summary>
        ///是否已经登入
        ///</summary>
        public virtual bool? IsLoginin
        {
            get{ return _isLoginin;}
            set{ _isLoginin=value;}
        }
        ///<summary>
        ///登陆时候的金币
        ///</summary>
        protected long? _loginCoin;

        /// <summary>
        ///登陆时候的金币
        ///</summary>
        public virtual long? LoginCoin
        {
            get{ return _loginCoin;}
            set{ _loginCoin=value;}
        }
        ///<summary>
        ///登陆时候的钻石
        ///</summary>
        protected int? _loginDiamond;

        /// <summary>
        ///登陆时候的钻石
        ///</summary>
        public virtual int? LoginDiamond
        {
            get{ return _loginDiamond;}
            set{ _loginDiamond=value;}
        }
        ///<summary>
        ///所在服务器(多服务器)
        ///</summary>
        protected string _server;

        /// <summary>
        ///所在服务器(多服务器)
        ///</summary>
        public virtual string Server
        {
            get{ return _server;}
            set{ _server=value;}
        }
        ///<summary>
        ///创建时间
        ///</summary>
        protected DateTime? _createDate;

        /// <summary>
        ///创建时间
        ///</summary>
        public virtual DateTime? CreateDate
        {
            get{ return _createDate;}
            set{ _createDate=value;}
        }
        ///<summary>
        ///最后更新时间
        ///</summary>
        protected DateTime? _lastDate;

        /// <summary>
        ///最后更新时间
        ///</summary>
        public virtual DateTime? LastDate
        {
            get{ return _lastDate;}
            set{ _lastDate=value;}
        }
        ///<summary>
        ///所在房间
        ///</summary>
        protected string _room;

        /// <summary>
        ///所在房间
        ///</summary>
        public virtual string Room
        {
            get{ return _room;}
            set{ _room=value;}
        }
        ///<summary>
        ///币数
        ///</summary>
        protected long? _boundCoins;

        /// <summary>
        ///币数
        ///</summary>
        public virtual long? BoundCoins
        {
            get{ return _boundCoins;}
            set{ _boundCoins=value;}
        }





    }
}
