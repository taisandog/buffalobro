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
        ///���ID
        ///</summary>
        protected int? _userId;

        /// <summary>
        ///���ID
        ///</summary>
        public virtual int? UserId
        {
            get{ return _userId;}
            set{ _userId=value;}
        }
        ///<summary>
        ///�û���
        ///</summary>
        protected string _name;

        /// <summary>
        ///�û���
        ///</summary>
        public virtual string Name
        {
            get{ return _name;}
            set{ _name=value;}
        }
        ///<summary>
        ///�ǳ�
        ///</summary>
        protected string _nickName;

        /// <summary>
        ///�ǳ�
        ///</summary>
        public virtual string NickName
        {
            get{ return _nickName;}
            set{ _nickName=value;}
        }
        ///<summary>
        ///�û���ʶ
        ///</summary>
        protected string _token;

        /// <summary>
        ///�û���ʶ
        ///</summary>
        public virtual string Token
        {
            get{ return _token;}
            set{ _token=value;}
        }
        ///<summary>
        ///�Ƽ���
        ///</summary>
        protected int? _agents;

        /// <summary>
        ///�Ƽ���
        ///</summary>
        public virtual int? Agents
        {
            get{ return _agents;}
            set{ _agents=value;}
        }
        ///<summary>
        ///�Ƿ��Ѿ�����
        ///</summary>
        protected bool? _isLoginin;

        /// <summary>
        ///�Ƿ��Ѿ�����
        ///</summary>
        public virtual bool? IsLoginin
        {
            get{ return _isLoginin;}
            set{ _isLoginin=value;}
        }
        ///<summary>
        ///��½ʱ��Ľ��
        ///</summary>
        protected long? _loginCoin;

        /// <summary>
        ///��½ʱ��Ľ��
        ///</summary>
        public virtual long? LoginCoin
        {
            get{ return _loginCoin;}
            set{ _loginCoin=value;}
        }
        ///<summary>
        ///��½ʱ�����ʯ
        ///</summary>
        protected int? _loginDiamond;

        /// <summary>
        ///��½ʱ�����ʯ
        ///</summary>
        public virtual int? LoginDiamond
        {
            get{ return _loginDiamond;}
            set{ _loginDiamond=value;}
        }
        ///<summary>
        ///���ڷ�����(�������)
        ///</summary>
        protected string _server;

        /// <summary>
        ///���ڷ�����(�������)
        ///</summary>
        public virtual string Server
        {
            get{ return _server;}
            set{ _server=value;}
        }
        ///<summary>
        ///����ʱ��
        ///</summary>
        protected DateTime? _createDate;

        /// <summary>
        ///����ʱ��
        ///</summary>
        public virtual DateTime? CreateDate
        {
            get{ return _createDate;}
            set{ _createDate=value;}
        }
        ///<summary>
        ///������ʱ��
        ///</summary>
        protected DateTime? _lastDate;

        /// <summary>
        ///������ʱ��
        ///</summary>
        public virtual DateTime? LastDate
        {
            get{ return _lastDate;}
            set{ _lastDate=value;}
        }
        ///<summary>
        ///���ڷ���
        ///</summary>
        protected string _room;

        /// <summary>
        ///���ڷ���
        ///</summary>
        public virtual string Room
        {
            get{ return _room;}
            set{ _room=value;}
        }
        ///<summary>
        ///����
        ///</summary>
        protected long? _boundCoins;

        /// <summary>
        ///����
        ///</summary>
        public virtual long? BoundCoins
        {
            get{ return _boundCoins;}
            set{ _boundCoins=value;}
        }





    }
}
