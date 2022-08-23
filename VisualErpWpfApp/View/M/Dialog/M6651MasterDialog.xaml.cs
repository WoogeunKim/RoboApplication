using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Man;
using ModelsLibrary.Sale;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;
using AquilaErpWpfApp3.Util;
using ModelsLibrary.Code;
using ModelsLibrary.Pur;

namespace AquilaErpWpfApp3.M.View.Dialog
{
    public partial class M6651MasterDialog : DXWindow
    {
        //private static SaleOrderServiceClient saleClient = SystemProperties.SaleOrderClient;
        private ManVo orgDao;
        private bool isEdit = false;
        private ManVo updateDao;
        private IList<ManVo> saveList;

        private string title = "생산계획수립";

        public M6651MasterDialog(ManVo Dao)
        {
            InitializeComponent();
            //
            SYSTEM_CODE_VO();


            this.orgDao = Dao;
            ManVo copyDao = new ManVo()
            {
                SL_PLN_NO = Dao.SL_PLN_NO,
                //SL_PLN_YRMON =  DateTime.ParseExact(Dao.SL_PLN_YRMON + "01", "yyyyMMdd", null).ToString("yyyy-MM-dd"),
                N1ST_ITM_GRP_CD = Dao.N1ST_ITM_GRP_CD,
                N1ST_ITM_GRP_NM = Dao.N1ST_ITM_GRP_NM,
                CO_CD = Dao.CO_CD,
                CO_NM = Dao.CO_NM,
                PROD_PLN_NO = Dao.PROD_PLN_NO,
                PLN_APLY_ST_DT = Dao.PLN_APLY_ST_DT,
                PLN_APLY_END_DT = Dao.PLN_APLY_END_DT,
                PLN_DYS = Dao.PLN_DYS,
                MCHN_NO = Dao.MCHN_NO,
                MCHN_NM = Dao.MCHN_NM,
                PROD_PLN_QTY = Dao.PROD_PLN_QTY,
                PROD_PLN_RMK = Dao.PROD_PLN_RMK,
                PROD_PLN_DT = Dao.PROD_PLN_DT,
                //ITM_OUT_QTY = Dao.ITM_OUT_QTY,
                SL_PLN_RMK = Dao.SL_PLN_RMK,
                CLZ_FLG = Dao.CLZ_FLG,
                CRE_USR_ID = Dao.CRE_USR_ID,
                UPD_USR_ID = Dao.UPD_USR_ID,
                SL_ORD_NO = Dao.SL_ORD_NO,
                SL_ORD_SEQ = Dao.SL_ORD_SEQ,
                SL_PLN_CD = Dao.SL_PLN_CD,
                SL_PLN_NM = Dao.SL_PLN_NM,
                INP_STAFF_VAL = Dao.INP_STAFF_VAL,
                WRK_ST_DT = Dao.WRK_ST_DT,
                WRK_END_DT = Dao.WRK_END_DT

            };

            if (Dao.SL_PLN_NO != null)
            {
                this.isEdit = true;
                this.navigator.IsMultiSelect = false;
                this.navigator.FocusedDate = Convert.ToDateTime(copyDao.PROD_PLN_DT);
                //this.combo_SL_PLN_NO.NullText = copyDao.SL_PLN_NO;
                this.combo_SL_PLN_NO.IsEnabled = false;
                //this.navigator.SelectedDates = new List<DateTime>() { Convert.ToDateTime(copyDao.PROD_PLN_DT) };
                //copyDao.SL_PLN_YRMON = 
                //this.text_CAR_TP_CD.IsEnabled = false;
                //this.text_CAR_TP_CD.Background = Brushes.DarkGray;
                if (string.IsNullOrEmpty(copyDao.WRK_ST_DT))
                {
                    copyDao.WRK_ST_DT = "00:00";
                }
                //
                if (string.IsNullOrEmpty(copyDao.WRK_END_DT))
                {
                    copyDao.WRK_END_DT = "00:00";
                }
            }
            else
            {
                this.isEdit = false;
                this.navigator.IsMultiSelect = true;

                //copyDao.WRK_DT = System.DateTime.Now.ToString("yyyy-MM-dd");
                copyDao.WRK_ST_DT = System.DateTime.Now.ToString("08:00");
                copyDao.WRK_END_DT = System.DateTime.Now.ToString("HH:mm");
                copyDao.INP_STAFF_VAL = 0;

                copyDao.SL_PLN_QTY = 0;
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
                //SaleVo _saleDao = getSaleDomain();
                //SaleVo resultVo;
                if (isEdit == false)
                {
                    saveList = new List<ManVo>();
                    //

                    SaleVo slPlnNoVo = this.combo_SL_PLN_NO.SelectedItem as SaleVo;
                    if (slPlnNoVo != null)
                    {
                        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6651", new StringContent(JsonConvert.SerializeObject(new ManVo() { SL_PLN_NO = slPlnNoVo.SL_PLN_NO, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                _Num = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList().Count;
                            }
                        }
                    }

                    //this.updateDao = getDomain();//this.updateDao
                    //int nCnt = 0;
                    ManVo _tmpVo = new ManVo();
                    foreach (DateTime item in this.navigator.SelectedDates)
                    {
                        _tmpVo = getDomain();
                        //
                        _tmpVo.PROD_PLN_DT = item.ToString("yyyy-MM-dd");
                        _tmpVo.PROD_PLN_NO = _tmpVo.SL_PLN_NO + _Num.ToString("D3");
                        saveList.Add(_tmpVo);

                        _Num++;
                    }

                    this.ResultDao = saveList[0];
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6651/i", new StringContent(JsonConvert.SerializeObject(this.saveList), System.Text.Encoding.UTF8, "application/json")))
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
                    this.updateDao.SL_PLN_NO = this.orgDao.SL_PLN_NO;
                    this.ResultDao = this.updateDao;
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6651/u", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
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

                //
                //
                ////영업
                ////월 생산 총 수량
                //_saleDao = getSaleDomain();
                //using (HttpResponseMessage responseX = await SystemProperties.PROGRAM_HTTP.PostAsync("m6651", new StringContent(JsonConvert.SerializeObject(new ManVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD, SL_PLN_NO = _saleDao.SL_PLN_NO }), System.Text.Encoding.UTF8, "application/json")))
                //{
                //    if (responseX.IsSuccessStatusCode)
                //    {
                //        _saleDao.SL_PLN_QTY = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await responseX.Content.ReadAsStringAsync()).Cast<ManVo>().ToList().Sum<ManVo>(sum => Convert.ToInt32(sum.SL_PLN_QTY));

                //        //수정
                //        using (HttpResponseMessage responseY = await SystemProperties.PROGRAM_HTTP.PostAsync("s2220/u", new StringContent(JsonConvert.SerializeObject(_saleDao), System.Text.Encoding.UTF8, "application/json")))
                //        {
                //            if (responseY.IsSuccessStatusCode)
                //            {
                //                string result = await responseY.Content.ReadAsStringAsync();
                //                if (int.TryParse(result, out _Num) == false)
                //                {
                //                    //실패
                //                    WinUIMessageBox.Show(result, title, MessageBoxButton.OK, MessageBoxImage.Error);
                //                    return;
                //                }
                //            }
                //        }
                //    }
                //}
                //

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
            if (string.IsNullOrEmpty(this.combo_SL_PLN_NO.Text))
            {
                WinUIMessageBox.Show("[(월)판매계획] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.combo_SL_PLN_NO.IsTabStop = true;
                this.combo_SL_PLN_NO.Focus();
                return false;
            }
            //else if (string.IsNullOrEmpty(this.combo_SL_PLN_NM.Text))
            //{
            //    WinUIMessageBox.Show("[구분] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.combo_SL_PLN_NM.IsTabStop = true;
            //    this.combo_SL_PLN_NM.Focus();
            //    return false;
            //}
            else if (string.IsNullOrEmpty(this.text_PROD_PLN_QTY.Text))
            {
                WinUIMessageBox.Show("[계획수량] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.text_PROD_PLN_QTY.IsTabStop = true;
                this.text_PROD_PLN_QTY.Focus();
                return false;
            }
            else if (navigator.SelectedDates.Count <= 0)
            {
                WinUIMessageBox.Show("[생산계획일] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.navigator.IsTabStop = true;
                this.navigator.Focus();
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
            //{
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
            //}
            return true;
        }
        #endregion

        #region Functon (getDomain - ConfigView1Dao)
        //생산
        public ManVo getDomain()
        {
            ManVo Dao = new ManVo();

            Dao.PROD_PLN_DT = this.navigator.FocusedDate.ToString("yyyy-MM-dd");
            Dao.PROD_PLN_NO = this.orgDao.PROD_PLN_NO;

            SaleVo slPlnNoVo = this.combo_SL_PLN_NO.SelectedItem as SaleVo;
            if (slPlnNoVo != null)
            {
                Dao.SL_PLN_NO = slPlnNoVo.SL_PLN_NO;
            }

            ManVo mchnNmVo = this.combo_MCHN_NM.SelectedItem as ManVo;
            if (mchnNmVo != null)
            {
                Dao.MCHN_NO = mchnNmVo.PROD_EQ_NO;
                Dao.MCHN_NM = mchnNmVo.EQ_NM;
            }

            Dao.PLN_DYS = this.navigator.SelectedDates.Count;

            Dao.INP_STAFF_VAL = this.text_INP_STAFF_VAL.Text;
            Dao.WRK_ST_DT = Convert.ToDateTime(this.navigator.SelectedDates[this.navigator.SelectedDates.Count - 1].ToString("yyyy-MM-dd") + " " + this.text_WRK_ST_DT.Text).ToString("yyyy-MM-dd HH:mm:00");
            Dao.WRK_END_DT = Convert.ToDateTime(this.navigator.SelectedDates[this.navigator.SelectedDates.Count - 1].ToString("yyyy-MM-dd") + " " + this.text_WRK_END_DT.Text).ToString("yyyy-MM-dd HH:mm:00");

            Dao.PLN_APLY_ST_DT = this.navigator.SelectedDates[0].ToString("yyyy-MM-dd");
            Dao.PLN_APLY_END_DT = this.navigator.SelectedDates[this.navigator.SelectedDates.Count-1].ToString("yyyy-MM-dd");

            Dao.PROD_PLN_QTY = Convert.ToInt32(this.text_PROD_PLN_QTY.Text) / this.navigator.SelectedDates.Count;
            Dao.PROD_PLN_RMK = this.text_PROD_PLN_RMK.Text;
            Dao.CLZ_FLG = "N";
            //Dao.DELT_FLG = (this.combo_DELT_FLG.Text.Equals("사용") ? "N" : "Y");
            Dao.CRE_USR_ID = SystemProperties.USER;
            Dao.UPD_USR_ID = SystemProperties.USER;
            Dao.CHNL_CD= SystemProperties.USER_VO.CHNL_CD;

            return Dao;
        }

        ////영업
        //public SaleVo getSaleDomain()
        //{
        //    SaleVo Dao = new SaleVo();

        //    //Dao.SL_PLN_NO = this.orgDao.SL_PLN_NO;
        //    Dao.SL_PLN_YRMON = this.navigator.SelectedDates[0].ToString("yyyyMM");

        //    //PurVo slOrdNo = this.combo_SL_ORD_NO.SelectedItem as PurVo;
        //    //if (slOrdNo != null)
        //    //{
        //    //    Dao.SL_ORD_NO = slOrdNo.SL_ORD_NO;
        //    //    Dao.SL_ORD_SEQ = slOrdNo.SL_ORD_SEQ;
        //    //    Dao.ITM_CD = slOrdNo.ITM_CD;
        //    //    Dao.CO_CD = slOrdNo.CO_NO;
        //    //}

        //    //SystemCodeVo slPlnNmVo = this.combo_SL_PLN_NM.SelectedItem as SystemCodeVo;
        //    //if (slPlnNmVo != null)
        //    //{
        //    //    Dao.SL_PLN_CD = slPlnNmVo.CLSS_CD;
        //    //    Dao.SL_PLN_NM = slPlnNmVo.CLSS_DESC;
        //    //}

        //    Dao.SL_PLN_QTY = Convert.ToInt32(this.text_PROD_PLN_QTY.Text);
        //    Dao.SL_PLN_RMK = this.text_PROD_PLN_RMK.Text;

        //    Dao.SL_PLN_NO = Dao.SL_PLN_CD + "F" + Dao.SL_PLN_YRMON.Substring(2) + "-" + Dao.ITM_CD;

        //    Dao.CLZ_FLG = "N";
        //    //Dao.DELT_FLG = (this.combo_DELT_FLG.Text.Equals("사용") ? "N" : "Y");
        //    Dao.CRE_USR_ID = SystemProperties.USER;
        //    Dao.UPD_USR_ID = SystemProperties.USER;
        //    Dao.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
        //    return Dao;
        //}
        #endregion

        public async void SYSTEM_CODE_VO()
        {

            try
            {
                DXSplashScreen.Show<ProgressWindow>();

                //if (isEdit == false)
                //{
                //    //수주 번호
                //    //
                //    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("p44011/mst", new StringContent(JsonConvert.SerializeObject(new PurVo() { FM_DT = System.DateTime.Now.AddMonths(-10).ToString("yyyyMMdd"), TO_DT = System.DateTime.Now.ToString("yyyyMMdd"), CLZ_FLG = "N", UPD_USR_ID = SystemProperties.USER, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                //    {
                //        if (response.IsSuccessStatusCode)
                //        {
                //            this.combo_SL_ORD_NO.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<PurVo>>(await response.Content.ReadAsStringAsync()).Cast<PurVo>().ToList();
                //            this.combo_SL_ORD_NO.SelectedItem = (this.combo_SL_ORD_NO.ItemsSource as List<PurVo>).Where(x => x.SL_ORD_NO.Equals(this.orgDao.SL_ORD_NO) && x.SL_ORD_SEQ == this.orgDao.SL_ORD_SEQ).FirstOrDefault<PurVo>();
                //        }
                //    }
                //}
                //else
                //{
                //    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("p44011/mst", new StringContent(JsonConvert.SerializeObject(new PurVo() { SL_ORD_NO = this.orgDao.SL_ORD_NO, UPD_USR_ID = SystemProperties.USER, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                //    {
                //        if (response.IsSuccessStatusCode)
                //        {
                //            this.combo_SL_ORD_NO.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<PurVo>>(await response.Content.ReadAsStringAsync()).Cast<PurVo>().ToList();
                //            this.combo_SL_ORD_NO.SelectedItem = (this.combo_SL_ORD_NO.ItemsSource as List<PurVo>).Where(x => x.SL_ORD_NO.Equals(this.orgDao.SL_ORD_NO) && x.SL_ORD_SEQ == this.orgDao.SL_ORD_SEQ).FirstOrDefault<PurVo>();
                //        }
                //    }

                //}

                ////구분 S-050
                //using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "S-050"))
                //{
                //    if (response.IsSuccessStatusCode)
                //    {
                //        this.combo_SL_PLN_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                //    }
                //}


                //영업 판매 계획 GBN = "N"
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s2220", new StringContent(JsonConvert.SerializeObject(new SaleVo() { DELT_FLG = "N", CHNL_CD = SystemProperties.USER_VO.CHNL_CD, SL_PLN_YRMON = Convert.ToDateTime(System.DateTime.Now).ToString("yyyyMM") }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.combo_SL_PLN_NO.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SaleVo>>(await response.Content.ReadAsStringAsync()).Cast<SaleVo>().ToList();
                    }
                }

                //설비
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6622", new StringContent(JsonConvert.SerializeObject(new ManVo() { DELT_FLG = "N", CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.combo_MCHN_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
                    }
                }

                ////구분 S-050
                //using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "S-050"))
                //{
                //    if (response.IsSuccessStatusCode)
                //    {
                //        this.combo_SL_PLN_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                //    }
                //}

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

        public ManVo ResultDao
        {
            get;
            set;
        }
        
    }
}
