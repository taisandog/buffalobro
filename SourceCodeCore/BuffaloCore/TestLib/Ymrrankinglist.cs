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
        ///倍率
        ///</summary>
        protected int _multipleRate;

        /// <summary>
        ///倍率
        ///</summary>
        public virtual int MultipleRate
        {
            get{ return _multipleRate;}
            set{ _multipleRate=value;}
        }
        ///<summary>
        ///分数
        ///</summary>
        protected long _score;

        /// <summary>
        ///分数
        ///</summary>
        public virtual long Score
        {
            get{ return _score;}
            set{ _score=value;}
        }
        ///<summary>
        ///鱼的名字
        ///</summary>
        protected string _fishName;

        /// <summary>
        ///鱼的名字
        ///</summary>
        public virtual string FishName
        {
            get{ return _fishName;}
            set{ _fishName=value;}
        }
        ///<summary>
        ///用户id
        ///</summary>
        protected int _userId;

        /// <summary>
        ///用户id
        ///</summary>
        public virtual int UserId
        {
            get{ return _userId;}
            set{ _userId=value;}
        }
        ///<summary>
        ///房间名
        ///</summary>
        protected string _roomName;

        /// <summary>
        ///房间名
        ///</summary>
        public virtual string RoomName
        {
            get{ return _roomName;}
            set{ _roomName=value;}
        }





    }
}
