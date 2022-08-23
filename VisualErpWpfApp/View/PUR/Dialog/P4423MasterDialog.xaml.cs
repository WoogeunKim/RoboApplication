using DevExpress.Xpf.Core;
using ModelsLibrary.Pur;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Input;
using AquilaErpWpfApp3.Util;

namespace AquilaErpWpfApp3.View.PUR.Dialog
{
    public partial class P4423MasterDialog : DXWindow
    {
        //private static SaleOrderServiceClient saleOrderClient = SystemProperties.SaleOrderClient;
        private PurVo orgDao;

        private string title = "구매실적관리";

        public P4423MasterDialog(PurVo Dao)
        {
            InitializeComponent();

            this.orgDao = Dao;

            this.text_AREA_NM.Text = this.orgDao.AREA_NM;
            this.text_CO_NM.Text = this.orgDao.CO_NM;

            this.text_FM_DT.Text = DateTime.ParseExact(Dao.FM_DT, "yyyyMMdd", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
            
            //if (Dao.SL_RLSE_NO != null)
            //{
            //    //수정
            //    //this.isEdit = true;
            //    Dao.NXT_MON_DT = Dao.NXT_MON_DT ?? System.DateTime.Now.AddMonths(1).ToString("yyyy-MM-01");

            //    this.text_NXT_MON_DT.Text = Convert.ToDateTime(Dao.NXT_MON_DT).ToString("yyyy-MM-dd");
            //}
            //else
            //{
            //    //추가
            //    //this.isEdit = false;

            //    this.text_NXT_MON_DT.Text = System.DateTime.Now.AddMonths(1).ToString("yyyy-MM-01");
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

                this.FM_DT = this.text_FM_DT.Text;
                this.AREA_NM = this.text_AREA_NM.Text;

                this.CO_NM = this.text_CO_NM.Text;
                this.SL_ITM_AMT = this.txt_SL_ITM_AMT.Text;
                //WinUIMessageBox.Show("완료 되었습니다", "[매입부가세]" + title, MessageBoxButton.OK, MessageBoxImage.Information);

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
        private PurVo getDomain()
        {
            PurVo Dao = new PurVo();


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

        public string FM_DT
        {
            get;
            set;
        }
        public string AREA_NM
        {
            get;
            set;
        }
        public string CO_NM
        {
            get;
            set;
        }
        public string SL_ITM_AMT
        {
            get;
            set;
        }
    }
}
