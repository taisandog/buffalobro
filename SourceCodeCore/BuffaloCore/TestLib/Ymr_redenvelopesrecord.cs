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
    public partial class Ymr_redenvelopesrecord: TestLib.YMRBase
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
        ///�Ѿ��������id�б�
        ///</summary>
        protected string _alreadyRobbedUserIDs;

        /// <summary>
        ///�Ѿ��������id�б�
        ///</summary>
        public virtual string AlreadyRobbedUserIDs
        {
            get{ return _alreadyRobbedUserIDs;}
            set{ _alreadyRobbedUserIDs=value;}
        }
        ///<summary>
        ///δ�������id�б�
        ///</summary>
        protected string _noRobberyUserIDs;

        /// <summary>
        ///δ�������id�б�
        ///</summary>
        public virtual string NoRobberyUserIDs
        {
            get{ return _noRobberyUserIDs;}
            set{ _noRobberyUserIDs=value;}
        }
        ///<summary>
        ///���а����
        ///</summary>
        protected string _redEnvelopesMoney;

        /// <summary>
        ///���а����
        ///</summary>
        public virtual string RedEnvelopesMoney
        {
            get{ return _redEnvelopesMoney;}
            set{ _redEnvelopesMoney=value;}
        }
        ///<summary>
        ///����ܽ��
        ///</summary>
        protected long _redEnvelopesSumGold;

        /// <summary>
        ///����ܽ��
        ///</summary>
        public virtual long RedEnvelopesSumGold
        {
            get{ return _redEnvelopesSumGold;}
            set{ _redEnvelopesSumGold=value;}
        }
        ///<summary>
        ///����ȼ�
        ///</summary>
        protected int? _roomLevel;

        /// <summary>
        ///����ȼ�
        ///</summary>
        public virtual int? RoomLevel
        {
            get{ return _roomLevel;}
            set{ _roomLevel=value;}
        }





    }
}
