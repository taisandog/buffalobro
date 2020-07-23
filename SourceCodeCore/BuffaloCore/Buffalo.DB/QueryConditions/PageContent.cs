using System;
using System.Data;
using Buffalo.DB;
using System.Data.SqlClient;
using Buffalo.DB.DbCommon;
using Buffalo.Kernel.Defaults;

namespace Buffalo.DB.QueryConditions
{
	/// <summary>
	/// ��ѯ����
	/// </summary>
	public enum SearchType
	{
		/// <summary>
		/// ��ȷ
		/// </summary>
		Precision,
		/// <summary>
		/// ģ��
		/// </summary>
		Faintness
	}
	/// <summary>
	/// ��ҳ��
	/// </summary>
	public class PageContent
	{
        private long _pageSize = 0;
        private long _currentPage = 0;
        //private long _totalPage = 0;
        private long _totalRecords = 0;
        private long _maxSelectRecords = 0;
        
        private bool _isFillTotalRecords=true;

        private long _startIndex = DefaultValue.DefaultLong;

        private int _pagerIndex=0;

        /// <summary>
        /// ��ҳ��ʼֵ
        /// </summary>
        public int PagerIndex
        {
            get { return _pagerIndex; }
            set { _pagerIndex = value; }
        }

        /// <summary>
        /// ��ȡ��ʼ��ѯ������(��0��ʼ)
        /// </summary>
        /// <returns></returns>
        public long GetStarIndex() 
        {
            if (_startIndex != DefaultValue.DefaultLong)
            {
                return _startIndex;
            }
            else 
            {
                return _pageSize * _currentPage;
            }
        }

		/// <summary>
		/// ��ʼ������
		/// </summary>
		/// <param name="ds">���ݼ�</param>
		private void InitData()
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ds">ָ��һ��DataSet���������</param>
		public PageContent()
		{
			InitData();
		}

        /// <summary>
        /// ��ʼ��ѯ����(��0��ʼ)
        /// </summary>
        public long StarIndex
        {
            get { return _startIndex; }
            set { _startIndex = value; }
        }
		
		/// <summary>
		/// ҳ��С
		/// </summary>
        public long PageSize
		{
			get { return _pageSize; }
			set { _pageSize=value; }
		}
		/// <summary>
		/// ҳ������0��ʼ��
		/// </summary>
        public long CurrentPage
		{
			get { return _currentPage; }
			set {
                if (value >= 0)
                {
                    _currentPage = value;
                }
            }
		}

		

		/// <summary>
		/// �ò�ѯ���ܼ�¼��
		/// </summary>
        public long TotalRecords
		{
            set { _totalRecords = value; }
			get { return _totalRecords; }
		}
        /// <summary>
        /// ��ҳ��
        /// </summary>
        public long TotalPage
        {
            //set { _totalPage = value; }
            get { return (long)Math.Ceiling((double)_totalRecords / (double)_pageSize); }
        }
		/// <summary>
		/// ����ѯ����
		/// </summary>
        public long MaxSelectRecords
		{
            set { _maxSelectRecords = value; }
            get { return _maxSelectRecords; }
		}

        /// <summary>
        /// �Ƿ���������
        /// </summary>
        public bool IsFillTotalRecords
        {
            get { return _isFillTotalRecords; }
            set { _isFillTotalRecords = value; }
        }

	}
}
