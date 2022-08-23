using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Code;
using ModelsLibrary.Sale;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;
using AquilaErpWpfApp3.Util;
using ModelsLibrary.Pur;

namespace AquilaErpWpfApp3.SAL.View.Dialog
{
    public partial class S2220MasterDialog : DXWindow
    {
        //private static SaleOrderServiceClient saleClient = SystemProperties.SaleOrderClient;
        private SaleVo orgDao;
        private bool isEdit = false;
        private SaleVo updateDao;

        private string title = "(월)판매계획";

        public S2220MasterDialog(SaleVo Dao)
        {
            InitializeComponent();
            //
            this.orgDao = Dao;

            SYSTEM_CODE_VO();

            SaleVo copyDao = new SaleVo()
            {
                SL_PLN_NO = Dao.SL_PLN_NO,
                SL_PLN_YRMON =  DateTime.ParseExact(Dao.SL_PLN_YRMON + "-01", "yyyy-MM-dd", null).ToString("yyyy-MM-dd"),
                N1ST_ITM_GRP_CD = Dao.N1ST_ITM_GRP_CD,
                N1ST_ITM_GRP_NM = Dao.N1ST_ITM_GRP_NM,
                ITM_CD = Dao.ITM_CD,
                ITM_NM = Dao.ITM_NM,
                CO_CD = Dao.CO_CD,
                CO_NM = Dao.CO_NM,
                SL_PLN_CD = Dao.SL_PLN_CD,
                SL_PLN_NM = Dao.SL_PLN_NM,
                SL_PLN_QTY = Dao.SL_PLN_QTY,
                ITM_STK_QTY = Dao.ITM_STK_QTY,
                ITM_ROUT_QTY = Dao.ITM_ROUT_QTY,
                ITM_IN_QTY = Dao.ITM_IN_QTY,
                ITM_OUT_QTY = Dao.ITM_OUT_QTY,
                SL_PLN_RMK = Dao.SL_PLN_RMK,
                CLZ_FLG = Dao.CLZ_FLG,
                CRE_USR_ID = Dao.CRE_USR_ID,
                UPD_USR_ID = Dao.UPD_USR_ID,
                SL_ORD_NO = Dao.SL_ORD_NO,
                SL_ORD_SEQ = Dao.SL_ORD_SEQ

            };

            if (Dao.SL_ORD_NO != null)
            {
                this.isEdit = true;
                this.combo_SL_ORD_NO.IsReadOnly = true;

                //copyDao.SL_PLN_YRMON = 
                //this.text_CAR_TP_CD.IsEnabled = false;
                //this.text_CAR_TP_CD.Background = Brushes.DarkGray;
            }
            else
            {
                this.isEdit = false;
                //copyDao.SL_PLN_QTY = 0;
                //copyDao.DELT_FLG = "사용";
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
                    this.updateDao = getDomain();//this.updateDao
                    this.ResultDao = this.updateDao;
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s2220/i", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
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
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s2220/u", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
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
            if (string.IsNullOrEmpty(this.text_SL_PLN_YRMON.Text))
            {
                WinUIMessageBox.Show("[년-도] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.text_SL_PLN_YRMON.IsTabStop = true;
                this.text_SL_PLN_YRMON.Focus();
                return false;
            }
            //else if (string.IsNullOrEmpty(this.combo_CO_NM.Text))
            //{
            //    WinUIMessageBox.Show("[업체명] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.combo_CO_NM.IsTabStop = true;
            //    this.combo_CO_NM.Focus();
            //    return false;
            //}
            //else if (string.IsNullOrEmpty(this.combo_N1ST_ITM_GRP_NM.Text))
            //{
            //    WinUIMessageBox.Show("[품목] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.combo_N1ST_ITM_GRP_NM.IsTabStop = true;
            //    this.combo_N1ST_ITM_GRP_NM.Focus();
            //    return false;
            //}
            else if (string.IsNullOrEmpty(this.combo_SL_PLN_NM.Text))
            {
                WinUIMessageBox.Show("[구분] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.combo_SL_PLN_NM.IsTabStop = true;
                this.combo_SL_PLN_NM.Focus();
                return false;
            }
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
        public SaleVo getDomain()
        {
            SaleVo Dao = new SaleVo();

            Dao.SL_PLN_NO = this.orgDao.SL_PLN_NO;
            Dao.SL_PLN_YRMON = Convert.ToDateTime(this.text_SL_PLN_YRMON.Text + "-01").ToString("yyyyMM");

            PurVo slOrdNo = this.combo_SL_ORD_NO.SelectedItem as PurVo;
            if (slOrdNo != null)
            {
                Dao.SL_ORD_NO = slOrdNo.SL_ORD_NO;
                Dao.SL_ORD_SEQ = slOrdNo.SL_ORD_SEQ;
                Dao.ITM_CD = slOrdNo.ITM_CD;
                Dao.CO_CD = slOrdNo.CO_NO;
            }

            //Dao.CAR_TP_CD = this.text_CAR_TP_CD.Text;
            //Dao.CAR_TP_NM = this.text_CAR_TP_NM.Text;

            //Dao.CAR_TP_SUB_NM = this.text_CAR_TP_SUB_NM.Text;

            //SaleVo n1stItmGrpNmVo = this.combo_N1ST_ITM_GRP_NM.SelectedItem as SaleVo;
            //if (n1stItmGrpNmVo != null)
            //{
            //    Dao.ITM_CD = n1stItmGrpNmVo.ITM_CD;
            //    Dao.ITM_NM = n1stItmGrpNmVo.ITM_NM;
            //    //Dao.N1ST_ITM_GRP_CD = n1stItmGrpNmVo.CAR_TP_CD;
            //    //Dao.N1ST_ITM_GRP_NM = n1stItmGrpNmVo.CAR_TP_NM;
            //    //Dao.CO_CD = n1stItmGrpNmVo.CO_CD;
            //    //Dao.CO_NM = n1stItmGrpNmVo.CO_NM;
            //}

            //SystemCodeVo coNmVo = this.combo_CO_NM.SelectedItem as SystemCodeVo;
            //if (coNmVo != null)
            //{
            //    Dao.CO_CD = coNmVo.CLSS_CD;
            //    Dao.CO_NM = coNmVo.CLSS_DESC;
            //}

            SystemCodeVo slPlnNmVo = this.combo_SL_PLN_NM.SelectedItem as SystemCodeVo;
            if (slPlnNmVo != null)
            {
                Dao.SL_PLN_CD = slPlnNmVo.CLSS_CD;
                Dao.SL_PLN_NM = slPlnNmVo.CLSS_DESC;
            }

            Dao.SL_PLN_QTY = this.text_SL_PLN_QTY.Text;
            Dao.SL_PLN_RMK = this.text_SL_PLN_RMK.Text;
            Dao.CLZ_FLG = "N";
            //Dao.DELT_FLG = (this.combo_DELT_FLG.Text.Equals("사용") ? "N" : "Y");
            Dao.CRE_USR_ID = SystemProperties.USER;
            Dao.UPD_USR_ID = SystemProperties.USER;
            Dao.CHNL_CD= SystemProperties.USER_VO.CHNL_CD;
            return Dao;
        }
        #endregion

        public async void SYSTEM_CODE_VO()
        {
            try
            {
                DXSplashScreen.Show<ProgressWindow>();
                //this.combo_SL_CO_NM.ItemsSource = SystemProperties.CUSTOMER_CODE_VO("AR", "100");
                //using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s143", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { DELT_FLG = "N", SEEK = "", CO_TP_CD = "AR", SEEK_AR = "AR", AREA_CD = SystemProperties.USER_VO.EMPE_PLC_NM, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                //{
                //    if (response.IsSuccessStatusCode)
                //    {
                //        this.combo_CO_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                //    }
                //}

                //차종
                //using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s11410", new StringContent(JsonConvert.SerializeObject(new SaleVo() { DELT_FLG = "N", CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                //{
                //    if (response.IsSuccessStatusCode)
                //    {
                //        this.combo_N1ST_ITM_GRP_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SaleVo>>(await response.Content.ReadAsStringAsync()).Cast<SaleVo>().ToList();
                //    }
                //}
                //품목
                //using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s141", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { DELT_FLG = "N", CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                //{
                //    if (response.IsSuccessStatusCode)
                //    {
                //        this.combo_N1ST_ITM_GRP_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                //    }
                //}

                //if (isEdit == false)
                if (this.orgDao.SL_ORD_NO == null)
                {
                    //추가
                    //수주 번호
                    //
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("p44011/mst", new StringContent(JsonConvert.SerializeObject(new PurVo() { FM_DT = System.DateTime.Now.AddMonths(-10).ToString("yyyyMMdd"), TO_DT = System.DateTime.Now.ToString("yyyyMMdd"), CLZ_FLG = "N", UPD_USR_ID = SystemProperties.USER, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            this.combo_SL_ORD_NO.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<PurVo>>(await response.Content.ReadAsStringAsync()).Cast<PurVo>().ToList();
                            this.combo_SL_ORD_NO.SelectedItem = (this.combo_SL_ORD_NO.ItemsSource as List<PurVo>).Where(x => x.SL_ORD_NO.Equals(this.orgDao.SL_ORD_NO) && x.SL_ORD_SEQ == this.orgDao.SL_ORD_SEQ).FirstOrDefault<PurVo>();
                        }
                    }
                }
                else
                {
                    //수정
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("p44011/mst", new StringContent(JsonConvert.SerializeObject(new PurVo() { SL_ORD_NO = this.orgDao.SL_ORD_NO, UPD_USR_ID = SystemProperties.USER, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            this.combo_SL_ORD_NO.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<PurVo>>(await response.Content.ReadAsStringAsync()).Cast<PurVo>().ToList();
                            this.combo_SL_ORD_NO.SelectedItem = (this.combo_SL_ORD_NO.ItemsSource as List<PurVo>).Where(x => x.SL_ORD_NO.Equals(this.orgDao.SL_ORD_NO) && x.SL_ORD_SEQ == this.orgDao.SL_ORD_SEQ).FirstOrDefault<PurVo>();
                        }
                    }

                }


                //구분 S-050
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "S-050"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.combo_SL_PLN_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                    }
                }

            DXSplashScreen.Close();
            }
            catch (Exception)
            {
                if (DXSplashScreen.IsActive == true)
                {
                    DXSplashScreen.Close();
                }
                return;
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

        public SaleVo ResultDao
        {
            get;
            set;
        }
        
    }
}
