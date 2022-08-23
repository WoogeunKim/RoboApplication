using AquilaErpWpfApp3.Util;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Auth;
using ModelsLibrary.Code;
using ModelsLibrary.Man;
using ModelsLibrary.Pur;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace AquilaErpWpfApp3.View.M.Dialog
{
    /// <summary>
    /// Interaction logic for ConfigView1Dialog.xaml
    /// </summary>
    public partial class M66550MasterDialog : DXWindow
    {
        //private static CodeServiceClient codeClient = SystemProperties.CodeClient;
        //private static ManServiceClient manClient = SystemProperties.ManClient;
        private ManVo orgDao;
        private bool isEdit = false;
        public ManVo updateDao;

        //private ObservableCollection<CodeDao> N1ST_ITM_GRP_LIST;
        //private ObservableCollection<WhihCodeDao> ASSY_ITM_LIST;

        //private P692Dialog _dialog;

        public M66550MasterDialog(ManVo Dao)
        {
            InitializeComponent();

            //this.combo_CO_NM.ItemsSource = SystemProperties.CUSTOMER_CODE_VO("AP,SU");
            //this.combo_INAUD_PLC_NM.ItemsSource = SystemProperties.SYSTEM_CODE_VO("L-002");
            //this.combo_WRK_MAN_NM.ItemsSource = SystemProperties.USER_CODE_VO();
         


            this.orgDao = Dao;
            ManVo copyDao = new ManVo()
            {
                GBN = Dao.GBN,
                //ASSY_ITM_CD = Dao.ASSY_ITM_CD,
                //ASSY_ITM_NM = Dao.ASSY_ITM_NM,
                //ASSY_ITM_SEQ = Dao.ASSY_ITM_SEQ,
                //BSE_WEIH_VAL = Dao.BSE_WEIH_VAL,
                //MIX_CD = Dao.MIX_CD,
                //MIX_NM = Dao.MIX_NM,
                //CMPO_CD = Dao.CMPO_CD,
                //CMPO_NM = Dao.CMPO_NM,
                //WEIH_VAL = Dao.WEIH_VAL,
                //CMPO_RMK = Dao.CMPO_RMK,
                //WEIH_BSE_VAL = Dao.WEIH_BSE_VAL,
                //WRK_DT = Dao.WRK_DT,
                //WRK_END_FLG = Dao.WRK_END_FLG,
                //WRK_MAN_NM = Dao.WRK_MAN_NM,
                //WRK_MAN_ID = Dao.WRK_MAN_ID,
                //TOT_WEIH_VAL = Dao.TOT_WEIH_VAL,
                //MIX_WEIH_VAL = Dao.MIX_WEIH_VAL,
                //WRK_DESC = Dao.WRK_DESC,
                //SL_ORD_NO = Dao.SL_ORD_NO,
                //SL_ORD_SEQ = Dao.SL_ORD_SEQ,
                //INP_LOT_NO = Dao.INP_LOT_NO,
                CHNL_CD = Dao.CHNL_CD
            };

            ////수정
            //if (Dao.LOT_NO != null)
            //{
            //    this.combo_ITM_GRP_CLSS_CD.IsReadOnly = true;
            //    this.combo_N1ST_ITM_GRP_CD.IsReadOnly = true;
            //    this.combo_ASSY_ITM_CD.IsReadOnly = true;

            //    this.text_TOT_WEIH_VAL.IsReadOnly = true;
            //    this.text_MIX_WEIH_VAL.IsReadOnly = true;
            //    //this.combo_MIX_NM.IsReadOnly = true;
            //    this.text_WRK_DT.IsReadOnly = true;
            //    this.combo_WRK_MAN_NM.IsReadOnly = true;

            //    this.combo_SL_ORD_NO.IsReadOnly = true;

            //    this.isEdit = true;

            //    this.combo_ITM_GRP_CLSS_CD.Background = Brushes.DarkGray;
            //    this.combo_N1ST_ITM_GRP_CD.Background = Brushes.DarkGray;
            //    this.combo_ASSY_ITM_CD.Background = Brushes.DarkGray;

            //    this.text_TOT_WEIH_VAL.Background = Brushes.DarkGray;
            //    this.text_MIX_WEIH_VAL.Background = Brushes.DarkGray;
            //    //this.combo_MIX_NM.Background = Brushes.DarkGray;
            //    this.text_WRK_DT.Background = Brushes.DarkGray;
            //    this.combo_WRK_MAN_NM.Background = Brushes.DarkGray;

            //    //
            //    //IList<ManVo> N1stVoList = manClient.M6623SelectMaster(new ManVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD });
            //    //int nCnt = N1stVoList.Count;
            //    //ManVo tmpVo;

            //    //ASSY_ITM_LIST = new ObservableCollection<WhihCodeDao>();
            //    //for (int x = 0; x < nCnt; x++)
            //    //{
            //    //    tmpVo = N1stVoList[x];
            //    //    ASSY_ITM_LIST.Add(new WhihCodeDao() { ITM_CD = tmpVo.ASSY_ITM_CD, ITM_NM = tmpVo.ITM_NM, BSE_WEIH_VAL = tmpVo.BSE_WEIH_VAL });
            //    //}
            //    //this.combo_ASSY_ITM_CD.ItemsSource = ASSY_ITM_LIST;
            //}
            //else
            //{
            //    //추가
            //    //this.text_ClssTpCd.IsReadOnly = false;
            //    this.isEdit = false;
            //    //copyDao.WRK_MAN_NM = SystemProperties.USER;
            //    copyDao.WRK_MAN_ID = SystemProperties.USER_VO.USR_ID;
            //    copyDao.TOT_WEIH_VAL = 0;
            //    copyDao.MIX_WEIH_VAL = 0;
            //    //copyDao.CMPO_TOR_VAL = 0;
            //    copyDao.WRK_DT = System.DateTime.Now.ToString("yyyy-MM-dd");
            //    copyDao.WRK_END_FLG = "아니요";
            //    //copyDao.MIX_NM = "신내리";


            //    this.combo_WRK_END_FLG.IsEnabled = false;

            //   // this.combo_ITM_GRP_CLSS_CD.ItemsSource = SystemProperties.SYSTEM_CODE_VO("L-001");
            //    //this.combo_ITM_GRP_CLSS_CD.SelectedIndexChanged += combo_ITM_GRP_CLSS_CD_SelectedIndexChanged;
            //    //this.combo_N1ST_ITM_GRP_CD.SelectedIndexChanged += combo_N1ST_ITM_GRP_CD_SelectedIndexChanged;

            //    //this.combo_ITM_GRP_CLSS_CD.Text = "완제품";
            //    //this.combo_N1ST_ITM_GRP_CD.Text = "혼련[디와이]";
            //    //this.combo_MIX_NM.Text = "신내리";
            //}


            SYSTEM_CODE_VO();


            this.configCode.DataContext = copyDao;
            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
            this.OKButton.Click += new RoutedEventHandler(OKButton_Click);
            this.CancelButton.Click += new RoutedEventHandler(CancelButton_Click);
        }

        //void combo_N1ST_ITM_GRP_CD_SelectedIndexChanged(object sender, RoutedEventArgs e)
        //{
            //CodeDao ITM_GRP_CLSS_NM = this.combo_ITM_GRP_CLSS_CD.SelectedItem as CodeDao;
            //if (ITM_GRP_CLSS_NM == null)
            //{
            //    return;
            //}

            //CodeDao N1ST_ITM_GRP_NM = this.combo_N1ST_ITM_GRP_CD.SelectedItem as CodeDao;
            //if (N1ST_ITM_GRP_NM == null)
            //{
            //    return;
            //}

            //IList<ManVo> N1stVoList = manClient.M6623SelectMaster(new ManVo() { ITM_GRP_CLSS_CD = ITM_GRP_CLSS_NM.CLSS_CD, N1ST_ITM_GRP_CD = N1ST_ITM_GRP_NM.CLSS_CD, CHNL_CD = SystemProperties.USER_VO.CHNL_CD });
            //int nCnt = N1stVoList.Count;
            //ManVo tmpVo;

            //ASSY_ITM_LIST = new ObservableCollection<WhihCodeDao>();
            //for (int x = 0; x < nCnt; x++)
            //{
            //    tmpVo = N1stVoList[x];
            //    ASSY_ITM_LIST.Add(new WhihCodeDao() { ITM_CD = tmpVo.ASSY_ITM_CD, ITM_NM = tmpVo.ITM_NM, BSE_WEIH_VAL = tmpVo.BSE_WEIH_VAL });
            //}
            //this.combo_ASSY_ITM_CD.ItemsSource = ASSY_ITM_LIST;
        //}
        //void combo_ITM_GRP_CLSS_CD_SelectedIndexChanged(object sender, RoutedEventArgs e)
        //{
            //CodeDao ITM_GRP_CLSS_NM = this.combo_ITM_GRP_CLSS_CD.SelectedItem as CodeDao;
            //if (ITM_GRP_CLSS_NM != null)
            //{
            //    IList<SystemCodeVo> N1stVoList = codeClient.SelectCodeItemGroupList(new SystemCodeVo() { DELT_FLG = "N", ITM_GRP_CLSS_CD = ITM_GRP_CLSS_NM.CLSS_CD, CRE_USR_ID = "", CHNL_CD = SystemProperties.USER_VO.CHNL_CD });
            //    int nCnt = N1stVoList.Count;
            //    SystemCodeVo tmpVo;

            //    N1ST_ITM_GRP_LIST = new ObservableCollection<CodeDao>();
            //    for (int x = 0; x < nCnt; x++)
            //    {
            //        tmpVo = N1stVoList[x];
            //        N1ST_ITM_GRP_LIST.Add(new CodeDao() { CLSS_DESC = tmpVo.ITM_GRP_NM, CLSS_CD = tmpVo.ITM_GRP_CD });
            //    }
            //    this.combo_N1ST_ITM_GRP_CD.ItemsSource = N1ST_ITM_GRP_LIST;
            //}
        //}

        #region Functon (OKButton_Click, CancelButton_Click)
        private async void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValueCheckd())
            {
                int _Num = 0;
                this.updateDao = getDomain();
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m665101/i", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();
                        if (int.TryParse(result, out _Num) == false)
                        {
                            //실패
                            WinUIMessageBox.Show(result, "[" + SystemProperties.PROGRAM_TITLE + "] " + this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        //성공
                        WinUIMessageBox.Show("완료 되었습니다", this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
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
            if (string.IsNullOrEmpty(this.combo_PROD_ORD_NO.Text))
            {
                WinUIMessageBox.Show("[수주 번호] 입력 값이 맞지 않습니다.", "[유효검사]" + this.Title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.combo_PROD_ORD_NO.IsTabStop = true;
                this.combo_PROD_ORD_NO.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(this.combo_PROD_LOC_NM.Text))
            {
                WinUIMessageBox.Show("[공장] 입력 값이 맞지 않습니다.", "[유효검사]" + this.Title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.combo_PROD_LOC_NM.IsTabStop = true;
                this.combo_PROD_LOC_NM.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(this.combo_WKY_YRMON.Text))
            {
                WinUIMessageBox.Show("[년월 / 주차] 입력 값이 맞지 않습니다.", "[유효검사]" + this.Title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.combo_WKY_YRMON.IsTabStop = true;
                this.combo_WKY_YRMON.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(this.combo_PROD_EQ_NO.Text))
            {
                WinUIMessageBox.Show("[설비] 입력 값이 맞지 않습니다.", "[유효검사] " + this.Title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.combo_PROD_EQ_NO.IsTabStop = true;
                this.combo_PROD_EQ_NO.Focus();
                return false;
            }
            //else if (string.IsNullOrEmpty(this.text_WRK_DT.Text))
            //{
            //    WinUIMessageBox.Show("[작업 일자] 입력 값이 맞지 않습니다.", "[유효검사] 칭량 작업 계획", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.text_WRK_DT.IsTabStop = true;
            //    this.text_WRK_DT.Focus();
            //    return false;
            //}
            //else if (string.IsNullOrEmpty(this.combo_WRK_MAN_NM.Text))
            //{
            //    WinUIMessageBox.Show("[작업자] 입력 값이 맞지 않습니다.", "[유효검사] 칭량 작업 계획", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.combo_WRK_MAN_NM.IsTabStop = true;
            //    this.combo_WRK_MAN_NM.Focus();
            //    return false;
            //}
            //else
            //{
            //    if (this.isEdit == false)
            //    {
            //        ManVo dao = new ManVo()
            //        {
            //            ASSY_ITM_CD = this.orgDao.ASSY_ITM_CD,
            //            BSE_WEIH_VAL = int.Parse(this.text_BSE_WEIH_VAL.Text),
            //            ASSY_ITM_SEQ = int.Parse(this.text_ASSY_ITM_SEQ.Text)

            //        };
            //        IList<ManVo> daoList = (IList<ManVo>)manClient.SelectProdWeihTbl(dao);
            //        if (daoList.Count != 0)
            //        {
            //            WinUIMessageBox.Show("[배합표 No. - 중복] 코드를 다시 입력 하십시오.", "[중복검사] 배합표", MessageBoxButton.OK, MessageBoxImage.Warning);
            //            this.text_BSE_WEIH_VAL.IsTabStop = true;
            //            this.text_BSE_WEIH_VAL.Focus();
            //            return false;
            //        }
            //    }
            //}
            return true;
        }
        #endregion

        #region Functon (getDomain - ConfigView1Dao)
        private ManVo getDomain()
        {
            ManVo Dao = new ManVo();


            ManVo prodOrdNoItmCd = this.combo_PROD_ORD_NO.SelectedItem as ManVo;
            if (prodOrdNoItmCd != null)
            {
                Dao.PROD_ORD_NO = prodOrdNoItmCd.PROD_ORD_NO;
                Dao.CMPO_CD = prodOrdNoItmCd.CMPO_CD;
                Dao.SL_ORD_NO = prodOrdNoItmCd.SL_ORD_NO;
                Dao.SL_ORD_SEQ = prodOrdNoItmCd.SL_ORD_SEQ;
            }

            SystemCodeVo prodLocNm = this.combo_PROD_LOC_NM.SelectedItem as SystemCodeVo;
            if (prodLocNm != null)
            {
                Dao.PROD_LOC_CD = prodLocNm.CLSS_CD;
            }


            ManVo wkyYrmonNm = this.combo_WKY_YRMON.SelectedItem as ManVo;
            if (wkyYrmonNm != null)
            {
                Dao.WKY_YRMON = wkyYrmonNm.WKY_YRMON;
                Dao.WK = wkyYrmonNm.WK;
            }

            ManVo eqNo = this.combo_PROD_EQ_NO.SelectedItem as ManVo;
            if (eqNo != null)
            {
                Dao.PROD_EQ_NO = eqNo.PROD_EQ_NO;
                Dao.EQ_NO = eqNo.PROD_EQ_NO;
                Dao.EQ_NM = eqNo.EQ_NM;
            }

            Dao.PROD_BSE_MOLD_NO = string.Empty;
            Dao.PROD_CORE_MOLD_NO = string.Empty;

            Dao.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
            Dao.CRE_USR_ID = SystemProperties.USER;
            Dao.UPD_USR_ID = SystemProperties.USER;


            return Dao;
        }
        #endregion


        public async void SYSTEM_CODE_VO()
        {
            try
            {
                DXSplashScreen.Show<ProgressWindow>();

                if (isEdit == false)
                {
                    //수주 번호
                    //
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("M665101/popup", new StringContent(JsonConvert.SerializeObject(new ManVo() { FM_DT = "", CLZ_FLG = "N", UPD_USR_ID = SystemProperties.USER, CHNL_CD = SystemProperties.USER_VO.CHNL_CD, A_ROUT_CD = new string[] { "TA", "SA" } }), System.Text.Encoding.UTF8, "application/json")))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            this.combo_PROD_ORD_NO.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
                            this.combo_PROD_ORD_NO.SelectedItem = (this.combo_PROD_ORD_NO.ItemsSource as List<ManVo>).Where(x => x.PROD_ORD_NO.Equals(this.orgDao.PROD_ORD_NO)).FirstOrDefault<ManVo>();
                        }
                    }
                }
                else
                {
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("M665101/popup", new StringContent(JsonConvert.SerializeObject(new ManVo() { PROD_ORD_NO = this.orgDao.PROD_ORD_NO, UPD_USR_ID = SystemProperties.USER, CHNL_CD = SystemProperties.USER_VO.CHNL_CD, A_ROUT_CD = new string[] { "TA", "SA" } }), System.Text.Encoding.UTF8, "application/json")))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            this.combo_PROD_ORD_NO.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
                            this.combo_PROD_ORD_NO.SelectedItem = (this.combo_PROD_ORD_NO.ItemsSource as List<ManVo>).Where(x => x.PROD_ORD_NO.Equals(this.orgDao.PROD_ORD_NO)).FirstOrDefault<ManVo>();
                        }
                    }

                }

                //공장
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "A-001"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.combo_PROD_LOC_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                        this.combo_PROD_LOC_NM.SelectedItem = (this.combo_PROD_LOC_NM.ItemsSource as List<SystemCodeVo>).Where(x => x.CLSS_CD.Equals(this.orgDao.PROD_LOC_CD)).FirstOrDefault<SystemCodeVo>();
                    }
                }


                //설비
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6622", new StringContent(JsonConvert.SerializeObject(new ManVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.combo_PROD_EQ_NO.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
                        this.combo_PROD_EQ_NO.SelectedItem = (this.combo_PROD_EQ_NO.ItemsSource as List<ManVo>).Where(x => x.PROD_EQ_NO.Equals(this.orgDao.PROD_EQ_NO)).LastOrDefault<ManVo>();
                    }
                }

                //년월 / 주차
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("M665100", new StringContent(JsonConvert.SerializeObject(new ManVo() { DELT_FLG = "N", CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.combo_WKY_YRMON.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
                        this.combo_WKY_YRMON.SelectedItem = (this.combo_WKY_YRMON.ItemsSource as List<ManVo>).Where(x => x.GBN.Equals(this.orgDao.GBN)).LastOrDefault<ManVo>();
                    }
                }

                ////this.combo_ITM_GRP_CLSS_CD_1.ItemsSource = SystemProperties.SYSTEM_CODE_VO("L-001");
                //using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "L-001"))
                //{
                //    if (response.IsSuccessStatusCode)
                //    {
                //        this.combo_ITM_GRP_CLSS_CD.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                //        this.combo_ITM_GRP_CLSS_CD.SelectedItem = (this.combo_ITM_GRP_CLSS_CD.ItemsSource as List<SystemCodeVo>).Where(x => x.CLSS_CD.Equals("W")).FirstOrDefault<SystemCodeVo>();
                //    }

                //    //
                //    using (HttpResponseMessage responseX = await SystemProperties.PROGRAM_HTTP.PostAsync("s133", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { DELT_FLG = "N", ITM_GRP_CLSS_CD = "W", CRE_USR_ID = "", CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                //    {
                //        if (responseX.IsSuccessStatusCode)
                //        {
                //            this.combo_N1ST_ITM_GRP_CD.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await responseX.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                //            this.combo_N1ST_ITM_GRP_CD.SelectedItem = (this.combo_N1ST_ITM_GRP_CD.ItemsSource as List<SystemCodeVo>)[0];
                //            //this.combo_N1ST_ITM_GRP_CD.SelectedItem = (this.combo_N1ST_ITM_GRP_CD.ItemsSource as List<SystemCodeVo>).Where(x => x.ITM_GRP_CD.Equals(this.orgDao.N1ST_ITM_GRP_CD)).LastOrDefault<SystemCodeVo>();
                //        }
                //    }

                //    //
                //    using (HttpResponseMessage responseY = await SystemProperties.PROGRAM_HTTP.PostAsync("m6623/mst", new StringContent(JsonConvert.SerializeObject(new ManVo() { DELT_FLG = "N", ITM_GRP_CLSS_CD = "W", N1ST_ITM_GRP_CD = (this.combo_N1ST_ITM_GRP_CD.SelectedItem as SystemCodeVo).CLSS_CD, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                //    {
                //        if (responseY.IsSuccessStatusCode)
                //        {
                //            this.combo_ASSY_ITM_CD.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await responseY.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
                //        }
                //    }

                //}

                ////
                ////작업자
                //using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s136/usr", new StringContent(JsonConvert.SerializeObject(new GroupUserVo() { DELT_FLG = "N", CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                //{
                //    if (response.IsSuccessStatusCode)
                //    {
                //        this.combo_WRK_MAN_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<GroupUserVo>>(await response.Content.ReadAsStringAsync()).Cast<GroupUserVo>().ToList();
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
    }
}
