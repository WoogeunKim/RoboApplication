using AquilaErpWpfApp3.Util;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Man;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;

namespace AquilaErpWpfApp3.M.View.Dialog
{
    public partial class M6511MasterDialog : DXWindow
    {
        //private static SaleOrderServiceClient saleClient = SystemProperties.SaleOrderClient;
        private ManVo orgDao;
        private bool isEdit = false;
        private ManVo updateDao;
        //private IList<ManVo> saveList;

        private string title = "조립 작업지시등록";

        public M6511MasterDialog(ManVo Dao)
        {
            InitializeComponent();
            //
            SYSTEM_CODE_VO();


            this.orgDao = Dao;
            ManVo copyDao = new ManVo()
            {
                MOLD_NO = Dao.MOLD_NO,
                MOLD_NM = Dao.MOLD_NM,
                MOLD_SZ = Dao.MOLD_SZ,
                CMPO_CD = Dao.CMPO_CD,
                ITM_CD = Dao.ITM_CD,
                ITM_NM = Dao.ITM_NM,
                //MOLD_MAKE_DT = Dao.MOLD_MAKE_DT,
                SL_ORD_NO = Dao.SL_ORD_NO,
                SL_ORD_SEQ = Dao.SL_ORD_SEQ,
                PCK_FLG = Dao.PCK_FLG,
                PROD_QTY = Dao.PROD_QTY,
                MM_RMK = Dao.MM_RMK,
                DY_NGT_FLG = Dao.DY_NGT_FLG,
                MOLD_CD = Dao.MOLD_CD,
                EQ_NO = Dao.EQ_NO,
                PRSURE_VAL = Dao.PRSURE_VAL,
                LOT_DIV_QTY = Dao.LOT_DIV_QTY,
                LOT_DIV_NO = Dao.LOT_DIV_NO,
                LOT_DIV_SEQ = Dao.LOT_DIV_SEQ,
                MAKE_ST_DT = Dao.MAKE_ST_DT,
                DY_NGT_NM = Dao.DY_NGT_NM,
                PROD_ORD_NO = Dao.PROD_ORD_NO,
                PROD_PLN_DT = Dao.PROD_PLN_DT,
                CRE_USR_ID = Dao.CRE_USR_ID,
                UPD_USR_ID = Dao.UPD_USR_ID,
                GBN = Dao.GBN
            };

            if (Dao.LOT_DIV_NO != null)
            {
                this.isEdit = true;
                //this.navigator.IsMultiSelect = false;
                //this.navigator.FocusedDate = Convert.ToDateTime(copyDao.PROD_PLN_DT);
                //this.combo_SL_PLN_NO.NullText = copyDao.SL_PLN_NO;
                //this.combo_SL_PLN_NO.IsEnabled = false;
                //this.navigator.SelectedDates = new List<DateTime>() { Convert.ToDateTime(copyDao.PROD_PLN_DT) };
                //copyDao.SL_PLN_YRMON = 
               // this.text_MOLD_NO.IsEnabled = false;
                //this.text_MOLD_NO.Background = Brushes.DarkGray;
            }
            else
            {
                this.isEdit = false;
                //this.navigator.IsMultiSelect = true;
                //copyDao.SL_PLN_QTY = 0;
                copyDao.DELT_FLG = "사용";
            }

            this.configCode.DataContext = copyDao;
            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
            this.OKButton.Click += new RoutedEventHandler(OKButton_Click);
            this.CancelButton.Click += new RoutedEventHandler(CancelButton_Click);
        }

      

        #region Functon (OKButton_Click, CancelButton_Click)
        private async void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValueCheckd())
            {
                int _Num = 0;
                //SaleVo resultVo;
                if (isEdit == false)
                {
                    this.updateDao = getDomain();
                    this.ResultDao = this.updateDao;
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6511/dtl/i", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string result = await response.Content.ReadAsStringAsync();
                            if (int.TryParse(result, out _Num) == false)
                            {
                                //실패
                                WinUIMessageBox.Show(result, title, MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }

                            //성공
                            WinUIMessageBox.Show("완료 되었습니다", title, MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }
                else
                {
                    this.updateDao = getDomain();
                    this.ResultDao = this.updateDao;
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6511/dtl/u", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string result = await response.Content.ReadAsStringAsync();
                            if (int.TryParse(result, out _Num) == false)
                            {
                                //실패
                                WinUIMessageBox.Show(result, title, MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }

                            //성공
                            WinUIMessageBox.Show("완료 되었습니다", title, MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }
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
            //if (string.IsNullOrEmpty(this.combo_EQ_NO.Text))
            //{
            //    WinUIMessageBox.Show("[호기] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.combo_EQ_NO.IsTabStop = true;
            //    this.combo_EQ_NO.Focus();
            //    return false;
            //}
            //else if (string.IsNullOrEmpty(this.combo_MOLD_CD.Text))
            //{
            //    WinUIMessageBox.Show("[금형] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.combo_MOLD_CD.IsTabStop = true;
            //    this.combo_MOLD_CD.Focus();
            //    return false;
            //}
            //else 
            if (string.IsNullOrEmpty(this.txt_LOT_DIV_QTY.Text))
            {
                WinUIMessageBox.Show("[수량] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.txt_LOT_DIV_QTY.IsTabStop = true;
                this.txt_LOT_DIV_QTY.Focus();
                return false;
            }
            ////else if (navigator.SelectedDates.Count <= 0)
            ////{
            ////    WinUIMessageBox.Show("[생산계획일] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
            ////    this.navigator.IsTabStop = true;
            ////    this.navigator.Focus();
            ////    return false;
            ////}
            ////else if (string.IsNullOrEmpty(this.text_END_APLY_DT.Text))
            ////{
            ////    WinUIMessageBox.Show("[사용 종료] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
            ////    this.text_END_APLY_DT.IsTabStop = true;
            ////    this.text_END_APLY_DT.Focus();
            ////    return false;
            ////}
            ////else
            //{
            //    //if (this.isEdit == false)
            //    //{
            //    //    SaleVo dao = new SaleVo()
            //    //    {
            //    //        BANK_ACCT_CD = this.text_BANK_ACCT_CD.Text
            //    //    };
            //    //    //ObservableCollection<SystemCodeVo> daoList = service.SearchDetailConfigView1(dao);
            //    //    //IList<SaleVo> daoList = (IList<SaleVo>)saleClient.S2231SelectMstList(dao);
            //    //    //if (daoList.Count != 0)
            //    //    //{
            //    //    //    WinUIMessageBox.Show("[코드 - 중복] 코드를 다시 입력 하십시오.", "[중복검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
            //    //    //    this.text_BANK_ACCT_CD.IsTabStop = true;
            //    //    //    this.text_BANK_ACCT_CD.Focus();
            //    //    //    return false;
            //    //    //}
            //    //}
            //}
            return true;
        }
        #endregion

        #region Functon (getDomain - ConfigView1Dao)
        public ManVo getDomain()
        {
            ManVo Dao = new ManVo();

            Dao.LOT_DIV_NO = this.orgDao.LOT_DIV_NO;
            Dao.PROD_ORD_NO = this.orgDao.PROD_ORD_NO;
            Dao.PROD_PLN_DT = Convert.ToDateTime(this.orgDao.PROD_PLN_DT).ToString("yyyy-MM-dd");

            Dao.MAKE_ST_DT = Convert.ToDateTime(this.text_MAKE_ST_DT.Text).ToString("yyyy-MM-dd");

            Dao.DY_NGT_FLG = (this.combo_DY_NGT_FLG.Text.Equals("야간") ? "N" : "D");
            Dao.DY_NGT_NM = this.combo_DY_NGT_FLG.Text;

            //
            ManVo eqNoVo = this.combo_EQ_NO.SelectedItem as ManVo;
            if (eqNoVo != null)
            {
                Dao.EQ_NO = eqNoVo.PROD_EQ_NO;
                Dao.EQ_NM = eqNoVo.EQ_NM;
            }

            ////
            //ManVo moldVo = this.combo_MOLD_CD.SelectedItem as ManVo;
            //if (moldVo != null)
            //{
            //    Dao.MOLD_CD = moldVo.MOLD_NO;
            //    Dao.MOLD_NM = moldVo.MOLD_NM;
            //    Dao.WEIH_SEQ = moldVo.WEIH_SEQ;
            //}

            Dao.CMPO_CD = this.orgDao.CMPO_CD; 
            Dao.ITM_CD = this.orgDao.ITM_CD;
            Dao.LOT_DIV_QTY = this.txt_LOT_DIV_QTY.Text;
            Dao.MM_RMK = this.text_LOT_DIV_RMK.Text;

            Dao.SL_ORD_NO = this.orgDao.SL_ORD_NO;
            Dao.SL_ORD_SEQ = this.orgDao.SL_ORD_SEQ;
            Dao.PCK_FLG = this.orgDao.PCK_FLG;

            Dao.CRE_USR_ID = SystemProperties.USER;
            Dao.UPD_USR_ID = SystemProperties.USER;
            Dao.CHNL_CD= SystemProperties.USER_VO.CHNL_CD;
            return Dao;
        }
        #endregion

        public async void SYSTEM_CODE_VO()
        {

            //설비
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6622", new StringContent(JsonConvert.SerializeObject(new ManVo() { DELT_FLG = "N", CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.combo_EQ_NO.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
                }
            }

            ////금형
            //using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6661", new StringContent(JsonConvert.SerializeObject(new ManVo() { DELT_FLG = "N", CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
            //{
            //    if (response.IsSuccessStatusCode)
            //    {
            //        this.combo_MOLD_CD.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
            //    }
            //}

            //////품명
            //using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s141", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { DELT_FLG = "N", CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
            //{
            //    if (response.IsSuccessStatusCode)
            //    {
            //        this.combo_ITM_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
            //    }
            //}

            //////제작업체
            //using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s143", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { DELT_FLG = "N", SEEK_AP = null, SEEK = null, CO_TP_CD = null, AREA_CD = SystemProperties.USER_VO.EMPE_PLC_NM, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
            //{
            //    if (response.IsSuccessStatusCode)
            //    {
            //        this.combo_MAKE_CO_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
            //    }
            //}

            ////사용프레스 L-009
            //using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "L-009"))
            //{
            //    if (response.IsSuccessStatusCode)
            //    {
            //        this.combo_PRS_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
            //    }
            //}

        }

        //IsEdit
        public bool IsEdit
        {
            get
            {
                return this.isEdit;
            }
        }

        public ManVo ResultDao
        {
            get;
            set;
        }
        
    }
}
