using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Buffalo.WinFormsControl.PagingBar
{
    public partial class MediumPagingBar : UserControl
    {
        public MediumPagingBar()
        {
            InitializeComponent();
        }
        #region ��������
        public event PageIndexChange OnPageIndexChange;

        /// <summary>
        /// ��ҳ��
        /// </summary>
        public long TotalPage
        {
            get
            {
                if (_pageSize > 0)
                {
                    return (long)Math.Ceiling((double)_totalRecords / (double)_pageSize);
                }
                return 0;
            }

        }

        private long _currentPage = 0;
        /// <summary>
        ///��ǰҳ��
        /// </summary>
        public long CurrentPage
        {
            get
            {
                return _currentPage;
            }
            set
            {
                _currentPage = value;
            }
        }


        private int _pageSize;
        /// <summary>
        /// ����Դ
        /// </summary>
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = value;
            }
        }

        private long _totalRecords;
        /// <summary>
        /// �ܼ�¼����
        /// </summary>
        public long TotalRecords
        {
            get
            {
                return _totalRecords;
            }
            set
            {
                _totalRecords = value;

                
            }
        }

        /// <summary>
        /// �Կؼ����з�ҳ����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="currentIndex"></param>
        protected void DoPageIndexChange(object sender, long currentIndex)
        {

            this.CurrentPage = currentIndex;
            if (OnPageIndexChange != null)
            {
                OnPageIndexChange(sender, currentIndex);
            }
        }

        /// <summary>
        /// ��ȡ�ַ�����ߵ���������
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetAllNumber(string str)
        {
            if (str == null || str == "")
            {
                return "0";
            }
            string ret = "";
            for (int i = 0; i < str.Length; i++)
            {
                if (char.IsDigit(str, i))
                {
                    ret += str[i];
                }
            }
            if (ret.Length <= 0)
            {
                ret = "0";
            }
            return ret;
        }
        #endregion
        /// <summary>
        /// �������
        /// </summary>
        public void BindData()
        {
            long curPage = CurrentPage + 1;
            long totalPage = TotalPage;
            this.Visible = true;
            if (TotalRecords == 0)
            {
                this.Visible = false;
                return;
            }
            CurrentPage = CurrentPage;
            lblPage.Text = "/" + totalPage.ToString() + "ҳ";
            labInfo.Text = "��[" + TotalRecords.ToString() + "]��";
            txtPage.Text = curPage.ToString();
            btnLast.Enabled = true;
            btnNext.Enabled = true;
            btnFirsh.Enabled = true;
            btnUp.Enabled = true;
            if (CurrentPage == 0)
            {
                //lbPri.Enabled = false;

                btnFirsh.Enabled = false;
                btnUp.Enabled = false;
            }
            if (CurrentPage >= TotalPage - 1)
            {
                //lbNext.Enabled = false;
                btnLast.Enabled = false;
                btnNext.Enabled = false;
            }
        }

        private void btnFirsh_Click(object sender, EventArgs e)
        {
            DoPageIndexChange(this, 0);
        }

        private void txtPage_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                int page = Convert.ToInt32(GetAllNumber(txtPage.Text));

                long total = TotalPage;
                if (page > total)
                {
                    page = 0;
                }
                DoPageIndexChange(this, page - 1);
            }
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            long page = CurrentPage - 1;
            if (page < 0)
            {
                page = 0;
            }
            DoPageIndexChange(this, page);
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            long page = CurrentPage + 1;
            if (page > TotalPage - 1)
            {
                page = TotalPage - 1;
            }

            DoPageIndexChange(this, page);
        }

        private void btnLast_Click(object sender, EventArgs e)
        {

            DoPageIndexChange(this, TotalPage - 1);
        }
    }
}
