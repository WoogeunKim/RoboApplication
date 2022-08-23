using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Code;
using ModelsLibrary.Man;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;
using AquilaErpWpfApp3.Util;

namespace AquilaErpWpfApp3.M.View.Dialog
{
    public partial class M6661MasterDialog : DXWindow
    {
        //private static SaleOrderServiceClient saleClient = SystemProperties.SaleOrderClient;
        private ManVo orgDao;
        private bool isEdit = false;
        private ManVo updateDao;
        //private IList<ManVo> saveList;

        private string title = "금형코드관리";

        public M6661MasterDialog(ManVo Dao)
        {
            InitializeComponent();
            //
            SYSTEM_CODE_VO();


            this.orgDao = Dao;
            ManVo copyDao = new ManVo()
            {
                MOLD_NO = Dao.MOLD_NO,
                MOLD_NM = Dao.MOLD_NM,
                WEIH_SEQ = Dao.WEIH_SEQ,
                MOLD_SZ = Dao.MOLD_SZ,
                ITM_CD = Dao.ITM_CD,
                ITM_NM = Dao.ITM_NM,
                MOLD_MAKE_DT = Dao.MOLD_MAKE_DT,
                MOLD_GRD_CD = Dao.MOLD_GRD_CD,
                GNTE_SHOT_QTY = Dao.GNTE_SHOT_QTY,
                MAKE_CO_CD = Dao.MAKE_CO_CD,
                MAKE_CO_NM = Dao.MAKE_CO_NM,
                USE_SHOT_QTY = Dao.USE_SHOT_QTY,
                MOLD_CAPA_QTY = Dao.MOLD_CAPA_QTY,
                PRS_CD = Dao.PRS_CD,
                PRS_NM = Dao.PRS_NM,
                PRSURE_VAL = Dao.PRSURE_VAL,
                DISCRD_DT = Dao.DISCRD_DT,
                DISCRD_RSN = Dao.DISCRD_RSN,
                MOLD_RMK = Dao.MOLD_RMK,
                DELT_FLG = Dao.DELT_FLG,
                CYC_TIME = Dao.CYC_TIME,
                GBN = Dao.GBN,
                UOM_WGT = Dao.UOM_WGT,
                UOM_RUN_WGT = Dao.UOM_RUN_WGT,
                CRE_USR_ID = Dao.CRE_USR_ID,
                UPD_USR_ID = Dao.UPD_USR_ID
            };

            if (Dao.MOLD_NO != null)
            {
                this.isEdit = true;
                
                //this.navigator.IsMultiSelect = false;
                //this.navigator.FocusedDate = Convert.ToDateTime(copyDao.PROD_PLN_DT);
                //this.combo_SL_PLN_NO.NullText = copyDao.SL_PLN_NO;
                //this.combo_SL_PLN_NO.IsEnabled = false;
                //this.navigator.SelectedDates = new List<DateTime>() { Convert.ToDateTime(copyDao.PROD_PLN_DT) };
                //copyDao.SL_PLN_YRMON = 
                this.text_MOLD_NO.IsEnabled = false;
                //this.text_MOLD_NO.Background = Brushes.DarkGray;
            }
            else
            {
                this.isEdit = false;
                copyDao.WEIH_SEQ = 0;
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
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6661/i", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
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
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6661/u", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
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
            if (string.IsNullOrEmpty(this.text_MOLD_NO.Text))
            {
                WinUIMessageBox.Show("[금형 번호] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.text_MOLD_NO.IsTabStop = true;
                this.text_MOLD_NO.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(this.text_MOLD_SEQ.Text))
            {
                WinUIMessageBox.Show("[금형 번호] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.text_MOLD_NO.IsTabStop = true;
                this.text_MOLD_NO.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(this.text_MOLD_NM.Text))
            {
                WinUIMessageBox.Show("[금형 명] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.text_MOLD_NM.IsTabStop = true;
                this.text_MOLD_NM.Focus();
                return false;
            }
            //else if (string.IsNullOrEmpty(this.combo_ITM_NM.Text))
            //{
            //    WinUIMessageBox.Show("[품명] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.combo_ITM_NM.IsTabStop = true;
            //    this.combo_ITM_NM.Focus();
            //    return false;
            //}
            //else if (string.IsNullOrEmpty(this.text_PROD_PLN_QTY.Text))
            //{
            //    WinUIMessageBox.Show("[생산계획수량] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.text_PROD_PLN_QTY.IsTabStop = true;
            //    this.text_PROD_PLN_QTY.Focus();
            //    return false;
            //}
            //else if (navigator.SelectedDates.Count <= 0)
            //{
            //    WinUIMessageBox.Show("[생산계획일] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.navigator.IsTabStop = true;
            //    this.navigator.Focus();
            //    return false;
            //}
            //else if (string.IsNullOrEmpty(this.text_END_APLY_DT.Text))
            //{
            //    WinUIMessageBox.Show("[사용 종료] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.text_END_APLY_DT.IsTabStop = true;
            //    this.text_END_APLY_DT.Focus();
            //    return false;
            //}
            //else
            {
                //if (this.isEdit == false)
                //{
                //    SaleVo dao = new SaleVo()
                //    {
                //        BANK_ACCT_CD = this.text_BANK_ACCT_CD.Text
                //    };
                //    //ObservableCollection<SystemCodeVo> daoList = service.SearchDetailConfigView1(dao);
                //    //IList<SaleVo> daoList = (IList<SaleVo>)saleClient.S2231SelectMstList(dao);
                //    //if (daoList.Count != 0)
                //    //{
                //    //    WinUIMessageBox.Show("[코드 - 중복] 코드를 다시 입력 하십시오.", "[중복검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                //    //    this.text_BANK_ACCT_CD.IsTabStop = true;
                //    //    this.text_BANK_ACCT_CD.Focus();
                //    //    return false;
                //    //}
                //}
            }
            return true;
        }
        #endregion

        #region Functon (getDomain - ConfigView1Dao)
        public ManVo getDomain()
        {
            ManVo Dao = new ManVo();

            //Dao.SL_PLN_NO = this.orgDao.SL_PLN_NO;
            //Dao.SL_PLN_YRMON = Convert.ToDateTime(this.text_SL_PLN_YRMON.Text + "-01").ToString("yyyyMM");

            Dao.MOLD_NO = this.text_MOLD_NO.Text;
            Dao.MOLD_NM = this.text_MOLD_NM.Text;
            Dao.MOLD_SZ = this.text_MOLD_SZ.Text;
            Dao.WEIH_SEQ = Convert.ToInt16(this.text_MOLD_SEQ.Text);

            ////Dao.CAR_TP_SUB_NM = this.text_CAR_TP_SUB_NM.Text;

            //Dao.PROD_PLN_DT = this.navigator.FocusedDate.ToString("yyyy-MM-dd");
            //Dao.PROD_PLN_NO = this.orgDao.PROD_PLN_NO;

            SystemCodeVo itmVo = this.combo_ITM_NM.SelectedItem as SystemCodeVo;
            if (itmVo != null)
            {
                Dao.ITM_CD = itmVo.ITM_CD;
                Dao.ITM_NM = itmVo.ITM_NM;
            }

            SystemCodeVo coNoVo = this.combo_MAKE_CO_NM.SelectedItem as SystemCodeVo;
            if (coNoVo != null)
            {
                Dao.MAKE_CO_CD = coNoVo.CO_NO;
                Dao.MAKE_CO_NM = coNoVo.CO_NM;
            }


            Dao.GNTE_SHOT_QTY = Convert.ToInt32(this.text_GNTE_SHOT_QTY.Text);
            Dao.USE_SHOT_QTY = Convert.ToInt32(this.text_USE_SHOT_QTY.Text);
            Dao.MOLD_CAPA_QTY = Convert.ToInt32(this.text_MOLD_CAPA_QTY.Text);

            //L-009 이상함
            //SystemCodeVo prsNmVo = this.combo_PRS_NM.SelectedItem as SystemCodeVo;
            //if (itmVo != null)
            //{
            //    Dao.PRS_CD = prsNmVo.CLSS_CD;
            //    Dao.PRS_NM = prsNmVo.CLSS_DESC;
            //}
            Dao.PRSURE_VAL = Convert.ToInt32(this.text_PRSURE_VAL.Text);
            Dao.DISCRD_DT = this.text_DISCRD_DT.Text;

            Dao.DISCRD_RSN = this.text_DISCRD_RSN.Text;
            Dao.MOLD_RMK = this.text_MOLD_RMK.Text;
            Dao.CYC_TIME = this.text_CYC_TIME.Text;

            Dao.UOM_WGT = this.text_UOM_WGT.Text;
            Dao.UOM_RUN_WGT = this.text_UOM_RUN_WGT.Text;

            //Dao.CLZ_FLG = "N";
            Dao.DELT_FLG = (this.combo_DELT_FLG.Text.Equals("사용") ? "N" : "Y");
            Dao.CRE_USR_ID = SystemProperties.USER;
            Dao.UPD_USR_ID = SystemProperties.USER;
            Dao.CHNL_CD= SystemProperties.USER_VO.CHNL_CD;

            Dao.GBN = this.orgDao.GBN;

            return Dao;
        }
        #endregion

        public async void SYSTEM_CODE_VO()
        {
            ////품명
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s141/mini", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { DELT_FLG = "N", CHNL_CD = SystemProperties.USER_VO.CHNL_CD, GBN = "IN", ITM_GRP_CLSS_CD = "W" }), System.Text.Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.combo_ITM_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                }
            }

            ////제작업체
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s143", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { DELT_FLG = "N", SEEK_AP = null, SEEK = null, CO_TP_CD = null, AREA_CD = SystemProperties.USER_VO.EMPE_PLC_NM, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.combo_MAKE_CO_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                }
            }

            //사용프레스 L-009
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "L-009"))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.combo_PRS_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                }
            }

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
