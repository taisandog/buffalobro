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
    public partial class Ymrinsideconfig: TestLib.YMRBase
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
        ///用户id，多个玩家用逗号隔开
        ///</summary>
        protected string _userIds;

        /// <summary>
        ///用户id，多个玩家用逗号隔开
        ///</summary>
        public virtual string UserIds
        {
            get{ return _userIds;}
            set{ _userIds=value;}
        }
        ///<summary>
        ///调整倍率
        ///</summary>
        protected int _multiples;

        /// <summary>
        ///调整倍率
        ///</summary>
        public virtual int Multiples
        {
            get{ return _multiples;}
            set{ _multiples=value;}
        }
        ///<summary>
        ///最小倍率
        ///</summary>
        protected int _minMultiple;

        /// <summary>
        ///最小倍率
        ///</summary>
        public virtual int MinMultiple
        {
            get{ return _minMultiple;}
            set{ _minMultiple=value;}
        }
        ///<summary>
        ///是否启用
        ///</summary>
        protected bool _isEnables;

        /// <summary>
        ///是否启用
        ///</summary>
        public virtual bool IsEnables
        {
            get{ return _isEnables;}
            set{ _isEnables=value;}
        }
        ///<summary>
        ///是否为内部账号
        ///</summary>
        protected bool _isInside;

        /// <summary>
        ///是否为内部账号
        ///</summary>
        public virtual bool IsInside
        {
            get{ return _isInside;}
            set{ _isInside=value;}
        }
        ///<summary>
        ///是否启用独立房间
        ///</summary>
        protected bool? _independentRoom;

        /// <summary>
        ///是否启用独立房间
        ///</summary>
        public virtual bool? IndependentRoom
        {
            get{ return _independentRoom;}
            set{ _independentRoom=value;}
        }





    }
}
