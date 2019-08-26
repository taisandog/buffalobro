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
    public partial class Ymrfishconfig: TestLib.YMRBase
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
        protected int _fishID;

        /// <summary>
        ///����
        ///</summary>
        public virtual int FishID
        {
            get{ return _fishID;}
            set{ _fishID=value;}
        }
        ///<summary>
        ///������
        ///</summary>
        protected string _fishName;

        /// <summary>
        ///������
        ///</summary>
        public virtual string FishName
        {
            get{ return _fishName;}
            set{ _fishName=value;}
        }
        ///<summary>
        ///��С��(��)
        ///</summary>
        protected int _minScore;

        /// <summary>
        ///��С��(��)
        ///</summary>
        public virtual int MinScore
        {
            get{ return _minScore;}
            set{ _minScore=value;}
        }
        ///<summary>
        ///����(��)
        ///</summary>
        protected int _maxScore;

        /// <summary>
        ///����(��)
        ///</summary>
        public virtual int MaxScore
        {
            get{ return _maxScore;}
            set{ _maxScore=value;}
        }
        ///<summary>
        ///��С����
        ///</summary>
        protected int _minNumber;

        /// <summary>
        ///��С����
        ///</summary>
        public virtual int MinNumber
        {
            get{ return _minNumber;}
            set{ _minNumber=value;}
        }
        ///<summary>
        ///�������
        ///</summary>
        protected int _maxNumber;

        /// <summary>
        ///�������
        ///</summary>
        public virtual int MaxNumber
        {
            get{ return _maxNumber;}
            set{ _maxNumber=value;}
        }
        ///<summary>
        ///���ּ���
        ///</summary>
        protected int _createOdds;

        /// <summary>
        ///���ּ���
        ///</summary>
        public virtual int CreateOdds
        {
            get{ return _createOdds;}
            set{ _createOdds=value;}
        }
        ///<summary>
        ///ʵ�ʼ���
        ///</summary>
        protected double _realOdds;

        /// <summary>
        ///ʵ�ʼ���
        ///</summary>
        public virtual double RealOdds
        {
            get{ return _realOdds;}
            set{ _realOdds=value;}
        }
        ///<summary>
        ///������
        ///</summary>
        protected int _props;

        /// <summary>
        ///������
        ///</summary>
        public virtual int Props
        {
            get{ return _props;}
            set{ _props=value;}
        }
        ///<summary>
        ///�Ƿ�ʺ�
        ///</summary>
        protected bool _isRainbow;

        /// <summary>
        ///�Ƿ�ʺ�
        ///</summary>
        public virtual bool IsRainbow
        {
            get{ return _isRainbow;}
            set{ _isRainbow=value;}
        }
        ///<summary>
        ///�Ƿ�����
        ///</summary>
        protected bool _isEnable;

        /// <summary>
        ///�Ƿ�����
        ///</summary>
        public virtual bool IsEnable
        {
            get{ return _isEnable;}
            set{ _isEnable=value;}
        }
        ///<summary>
        ///�Ƿ��ϴ������а�
        ///</summary>
        protected bool _isUploadRankingList;

        /// <summary>
        ///�Ƿ��ϴ������а�
        ///</summary>
        public virtual bool IsUploadRankingList
        {
            get{ return _isUploadRankingList;}
            set{ _isUploadRankingList=value;}
        }





    }
}
