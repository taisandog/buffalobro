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
        ///�û�id���������ö��Ÿ���
        ///</summary>
        protected string _userIds;

        /// <summary>
        ///�û�id���������ö��Ÿ���
        ///</summary>
        public virtual string UserIds
        {
            get{ return _userIds;}
            set{ _userIds=value;}
        }
        ///<summary>
        ///��������
        ///</summary>
        protected int _multiples;

        /// <summary>
        ///��������
        ///</summary>
        public virtual int Multiples
        {
            get{ return _multiples;}
            set{ _multiples=value;}
        }
        ///<summary>
        ///��С����
        ///</summary>
        protected int _minMultiple;

        /// <summary>
        ///��С����
        ///</summary>
        public virtual int MinMultiple
        {
            get{ return _minMultiple;}
            set{ _minMultiple=value;}
        }
        ///<summary>
        ///�Ƿ�����
        ///</summary>
        protected bool _isEnables;

        /// <summary>
        ///�Ƿ�����
        ///</summary>
        public virtual bool IsEnables
        {
            get{ return _isEnables;}
            set{ _isEnables=value;}
        }
        ///<summary>
        ///�Ƿ�Ϊ�ڲ��˺�
        ///</summary>
        protected bool _isInside;

        /// <summary>
        ///�Ƿ�Ϊ�ڲ��˺�
        ///</summary>
        public virtual bool IsInside
        {
            get{ return _isInside;}
            set{ _isInside=value;}
        }
        ///<summary>
        ///�Ƿ����ö�������
        ///</summary>
        protected bool? _independentRoom;

        /// <summary>
        ///�Ƿ����ö�������
        ///</summary>
        public virtual bool? IndependentRoom
        {
            get{ return _independentRoom;}
            set{ _independentRoom=value;}
        }





    }
}
