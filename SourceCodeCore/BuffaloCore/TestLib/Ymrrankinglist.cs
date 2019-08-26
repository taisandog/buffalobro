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
    public partial class Ymrrankinglist: TestLib.YMRBase
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
        ///����
        ///</summary>
        protected int _multipleRate;

        /// <summary>
        ///����
        ///</summary>
        public virtual int MultipleRate
        {
            get{ return _multipleRate;}
            set{ _multipleRate=value;}
        }
        ///<summary>
        ///����
        ///</summary>
        protected long _score;

        /// <summary>
        ///����
        ///</summary>
        public virtual long Score
        {
            get{ return _score;}
            set{ _score=value;}
        }
        ///<summary>
        ///�������
        ///</summary>
        protected string _fishName;

        /// <summary>
        ///�������
        ///</summary>
        public virtual string FishName
        {
            get{ return _fishName;}
            set{ _fishName=value;}
        }
        ///<summary>
        ///�û�id
        ///</summary>
        protected int _userId;

        /// <summary>
        ///�û�id
        ///</summary>
        public virtual int UserId
        {
            get{ return _userId;}
            set{ _userId=value;}
        }
        ///<summary>
        ///������
        ///</summary>
        protected string _roomName;

        /// <summary>
        ///������
        ///</summary>
        public virtual string RoomName
        {
            get{ return _roomName;}
            set{ _roomName=value;}
        }





    }
}
