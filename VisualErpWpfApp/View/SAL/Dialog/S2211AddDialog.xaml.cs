using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Sale;
using System;
using System.Windows;
using System.Windows.Input;
using AquilaErpWpfApp3.Util;

namespace AquilaErpWpfApp3.View.SAL.Dialog
{
    public partial class S2211AddDialog : DXWindow
    {
        //private static SaleOrderServiceClient saleOrderClient = SystemProperties.SaleOrderClient;
        private SaleVo orgDao;

        private string title = "출하 의뢰 등록";

        public S2211AddDialog(SaleVo Dao)
        {
            InitializeComponent();

            this.orgDao = Dao;

            this.SL_RLSE_NM.Text = this.orgDao.SL_RLSE_NM;
            this.SL_CO_NM.Text = this.orgDao.SL_CO_NM;

            //this.orgDao.ADD_FLG = (string.IsNullOrEmpty(this.orgDao.ADD_FLG) ? "Y" : this.orgDao.ADD_FLG);
            //this.orgDao.ADD_FLG = "Y";
            //this.text_ADD_FLG.Text = (this.orgDao.ADD_FLG.Equals("Y") ? "Y" : "N");
            this.text_ADD_FLG.Text = "Y";
            //if (Dao.SL_RLSE_NO != null)
            //{
            //    //수정
            //    //this.isEdit = true;
            //    //Dao.NXT_MON_DT = Dao.NXT_MON_DT ?? System.DateTime.Now.AddMonths(1).ToString("yyyy-MM-01");
            //    //this.text_NXT_MON_DT.Text = Convert.ToDateTime(Dao.NXT_MON_DT).ToString("yyyy-MM-dd");
            //}
            //else
            //{
            //    //추가
            //    //this.isEdit = false;

            //    //this.text_NXT_MON_DT.Text = System.DateTime.Now.AddMonths(1).ToString("yyyy-MM-01");
            //}
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

                this.ADD_FLG = this.text_ADD_FLG.Text;

                WinUIMessageBox.Show("완료 되었습니다", "[추가 주문]" + title, MessageBoxButton.OK, MessageBoxImage.Information);

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
        private SaleVo getDomain()
        {
            SaleVo Dao = new SaleVo();


            Dao.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
            Dao.CRE_USR_ID = SystemProperties.USER;
            Dao.UPD_USR_ID = SystemProperties.USER;

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

        public string ADD_FLG
        {
            get;
            set;
        }


    }
}
