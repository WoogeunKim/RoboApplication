using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Inv;
using System;
using System.Windows;
using System.Windows.Input;
using AquilaErpWpfApp3.Util;

namespace AquilaErpWpfApp3.View.INV.Dialog
{
    public partial class I5511NextMonthDialog : DXWindow
    {
        //private static SaleOrderServiceClient saleOrderClient = SystemProperties.SaleOrderClient;
        private InvVo orgDao;

        private string title = "이월등록";

        public I5511NextMonthDialog(InvVo Dao)
        {
            InitializeComponent();

            this.orgDao = Dao;

            this.SL_RLSE_NM.Text = this.orgDao.INSRL_NM;
            this.SL_CO_NM.Text = this.orgDao.CO_NM;


            if (Dao.INSRL_NO != null)
            {
                //수정
                //this.isEdit = true;
                Dao.INAUD_DT = Dao.INAUD_DT ?? System.DateTime.Now.AddMonths(1).ToString("yyyy-MM-01");

                this.text_NXT_MON_DT.Text = Convert.ToDateTime(Dao.INAUD_DT).ToString("yyyy-MM-dd");
            }
            else
            {
                //추가
                //this.isEdit = false;

                this.text_NXT_MON_DT.Text = System.DateTime.Now.AddMonths(1).ToString("yyyy-MM-01");
            }
            //this.configCode.DataContext = Dao;
            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
            this.OKButton.Click += new RoutedEventHandler(OKButton_Click);
            this.CancelButton.Click += new RoutedEventHandler(CancelButton_Click);
        }

       

        #region Functon (OKButton_Click, CancelButton_Click)
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValueCheckd())
            {

                this.NXT_MON_DT = Convert.ToDateTime(this.text_NXT_MON_DT.Text).ToString("yyyy-MM-dd");

                WinUIMessageBox.Show("이월 완료 되었습니다", title, MessageBoxButton.OK, MessageBoxImage.Information);

                this.DialogResult = true;
                this.Close();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void HandleEsc(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.DialogResult = false;
                Close();
            }
        }
        #endregion

        #region Functon (ValueCheckd)
        public Boolean ValueCheckd()
        {
            return true;
        }
        #endregion

        #region Functon (getDomain - ConfigView1Dao)
        private InvVo getDomain()
        {
            InvVo Dao = new InvVo();

            Dao.CRE_USR_ID = SystemProperties.USER;
            Dao.UPD_USR_ID = SystemProperties.USER;
            Dao.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;

            return Dao;
        }
        #endregion

        //IsEdit
        //public bool IsEdit
        //{
        //    get
        //    {
        //        return this.isEdit;
        //    }
        //}

        public string NXT_MON_DT
        {
            get;
            set;
        }
    }
}
