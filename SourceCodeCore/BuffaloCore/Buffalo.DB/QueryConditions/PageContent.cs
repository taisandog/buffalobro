using System;
using System.Data;
using Buffalo.DB;
using System.Data.SqlClient;
using Buffalo.DB.DbCommon;
using Buffalo.Kernel.Defaults;

namespace Buffalo.DB.QueryConditions
{
	/// <summary>
	/// 查询类型
	/// </summary>
	public enum SearchType
	{
		/// <summary>
		/// 精确
		/// </summary>
		Precision,
		/// <summary>
		/// 模糊
		/// </summary>
		Faintness
	}
	/// <summary>
	/// 分页类
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
        /// 分页起始值
        /// </summary>
        public int PagerIndex
        {
            get { return _pagerIndex; }
            set { _pagerIndex = value; }
        }

        /// <summary>
        /// 获取起始查询的索引(从0开始)
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
		/// 初始化数据
		/// </summary>
		/// <param name="ds">数据集</param>
		private void InitData()
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ds">指定一个DataSet来填充数据</param>
		public PageContent()
		{
			InitData();
		}

        /// <summary>
        /// 起始查询条数(从0开始)
        /// </summary>
        public long StarIndex
        {
            get { return _startIndex; }
            set { _startIndex = value; }
        }
		
		/// <summary>
		/// 页大小
		/// </summary>
        public long PageSize
		{
			get { return _pageSize; }
			set { _pageSize=value; }
		}
		/// <summary>
		/// 页数，从0开始算
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
		/// 该查询的总记录数
		/// </summary>
        public long TotalRecords
		{
            set { _totalRecords = value; }
			get { return _totalRecords; }
		}
        /// <summary>
        /// 总页数
        /// </summary>
        public long TotalPage
        {
            //set { _totalPage = value; }
            get { return (long)Math.Ceiling((double)_totalRecords / (double)_pageSize); }
        }
		/// <summary>
		/// 最大查询条数
		/// </summary>
        public long MaxSelectRecords
		{
            set { _maxSelectRecords = value; }
            get { return _maxSelectRecords; }
		}

        /// <summary>
        /// 是否查出总条数
        /// </summary>
        public bool IsFillTotalRecords
        {
            get { return _isFillTotalRecords; }
            set { _isFillTotalRecords = value; }
        }

	}
}
