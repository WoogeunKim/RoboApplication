using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Code;
using ModelsLibrary.Pur;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;
using AquilaErpWpfApp3.Util;
using ModelsLibrary.Man;

namespace AquilaErpWpfApp3.View.PUR.Dialog
{
    public partial class P414101MasterDialog : DXWindow
    {
        //private static PurServiceClient purClient = SystemProperties.PurClient;
        private PurVo orgDao;
        private bool isEdit = false;
        private PurVo updateDao;

        private SystemCodeVo _itm;

        private string _title = "BOM등록(구매)";

        public P414101MasterDialog(PurVo Dao)
        {
            InitializeComponent();

          

            this.combo_ITM_GRP_CLSS_NM.SelectedIndexChanged += combo_ITM_GRP_CLSS_NM_SelectedIndexChanged;

            this.orgDao = Dao;

            PurVo copyDao = new PurVo()
            {
                ITM_CD = Dao.ITM_CD,
                ITM_GRP_CLSS_CD = Dao.ITM_GRP_CLSS_CD,
                ASSY_SEQ = Dao.ASSY_SEQ,
                CMPO_CD = Dao.CMPO_CD,
                GBN = Dao.GBN,
                ITM_NM = Dao.ITM_NM,
                ITM_SZ_NM = Dao.ITM_SZ_NM,
                INP_QTY = Convert.ToDecimal(Dao.INP_QTY).ToString("N6"),
                UPD_DT = Dao.UPD_DT,
                UPD_USR_ID = Dao.UPD_USR_ID,
                CHNL_CD = SystemProperties.USER_VO.CHNL_CD,
                ROUT_CD = Dao.ROUT_CD,
                ROUT_NM = Dao.ROUT_NM,
                PRNT_ROUT_CD = Dao.PRNT_ROUT_CD,
                PRNT_ROUT_NM = Dao.PRNT_ROUT_NM,
                ITM_GRP_CLSS_NM = Dao.ITM_GRP_CLSS_NM,
            };

            //수정
            if (Dao.CMPO_CD != null)
            {
                this.combo_ITM_GRP_CLSS_NM.IsReadOnly = true;
                //this.combo_CMPO_CD.IsReadOnly = true;

                this.CmpoNmButton.IsEnabled = false;

                this.isEdit = true;
                //
                ////마감 처리 후 수정 불가능
                //if (Dao.MODI_FLG.Equals("N"))
                //{
                //    this.OKButton.IsEnabled = false;
                //}
            }
            else
            {
                //추가
                //this.text_PUR_ORD_NO.IsReadOnly = true;
                this.isEdit = false;

                copyDao.ITM_GRP_CLSS_NM = "부자재";

                //Dao.PUR_DT = System.DateTime.Now.ToString("yyyy-MM-dd");
                //Dao.PUR_DUE_DT = System.DateTime.Now.ToString("yyyy-MM-dd");
                //Dao.PUR_EMPE_ID = SystemProperties.USER;

                //Dao.PUR_WK_CD = 1;

                //Dao.PUR_ITM_NM = "원자재";
                //
                //Dao.DO_RQST_GRP_NM = SystemProperties.USERVO.GRP_NM;
                //Dao.DO_RQST_USR_NM = SystemProperties.USERVO.USR_N1ST_NM;
                //this.combo_DO_RQST_USR_NM.SelectedItem = ((IList<CodeDao>)combo_DO_RQST_USR_NM.ItemsSource)[2];
            }

            SYSTEM_CODE_VO();

            //품목 호출
            this.CmpoNmButton.Click += new RoutedEventHandler(CmpoNmButton_Click);
            //품목 조회 호출
            this.FindButton.Click += new RoutedEventHandler(FindButton_Click);
            //폼목 조회
            this.ViewGridItm.MouseDoubleClick += ViewGridItm_MouseDoubleClick;

            this.configCode.DataContext = copyDao;
            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
            this.OKButton.Click += new RoutedEventHandler(OKButton_Click);
            this.CancelButton.Click += new RoutedEventHandler(CancelButton_Click);
        }

        private void ViewGridItm_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ItemCodeVo _selItm = this.ViewGridItm.SelectedItem as ItemCodeVo;
            if (_selItm != null)
            {
                this.text_CMPO_CD.Text = _selItm.ITM_CD;
                this.lab_gbn.Text = _selItm.ITM_NM;
            }
        }

        private async void CmpoNmButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.addFalyoutCmpoNm.IsOpen = true;

                if (this.ViewGridItm.ItemsSource == null)
                {
                    this.ViewGridItm.ShowLoadingPanel = true;
                    using (HttpResponseMessage responseY = await SystemProperties.PROGRAM_HTTP.PostAsync("s141/mini", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { DELT_FLG = "N", ITM_GRP_CLSS_CD = "B", CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                    {
                        if (responseY.IsSuccessStatusCode)
                        {
                            this.ViewGridItm.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<ItemCodeVo>>(await responseY.Content.ReadAsStringAsync()).Cast<ItemCodeVo>().ToList();
                            this.ViewGridItm.ShowLoadingPanel = false;
                        }
                    }
                }
            }
            catch(Exception eLog)
            {
                return;
            }
        }

        private async void FindButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //if (this.ViewGridDtl.ItemsSource == null)
                //{
                this.ViewGridItm.ShowLoadingPanel = true;
                SystemCodeVo ITM_GRP_CLSS_NM = this.combo_ITM_GRP_CLSS_NM.SelectedItem as SystemCodeVo;
                if (ITM_GRP_CLSS_NM != null)
                {
                    if (ITM_GRP_CLSS_NM.CLSS_CD.Equals("H"))
                    {
                        this.ViewTableItm.SearchString = this.orgDao.ITM_CD;
                    }
                    else
                    {
                        this.ViewTableItm.SearchString = "";
                    }

                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s141/mini", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { DELT_FLG = "N", ITM_GRP_CLSS_CD = ITM_GRP_CLSS_NM.CLSS_CD, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            this.ViewGridItm.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<ItemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<ItemCodeVo>().ToList();
                            this.ViewGridItm.ShowLoadingPanel = false;
                        }
                    }
                }
                //}
            }
            catch (Exception eLog)
            {
                return;
            }
        }




        #region Functon (OKButton_Click, CancelButton_Click)
        private async void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValueCheckd())
            {
                int _Num = 0;
                //ProgramVo resultVo;
                if (isEdit == false)
                {
                    this.updateDao = getDomain();//this.updateDao
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("p414101/dtl/i", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string result = await response.Content.ReadAsStringAsync();
                            if (int.TryParse(result, out _Num) == false)
                            {
                                //실패
                                WinUIMessageBox.Show(result, _title, MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }

                            //성공
                            WinUIMessageBox.Show("완료 되었습니다", _title, MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }
                else
                {
                    this.updateDao = getDomain();

                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("p414101/dtl/u", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string result = await response.Content.ReadAsStringAsync();
                            if (int.TryParse(result, out _Num) == false)
                            {
                                //실패
                                WinUIMessageBox.Show(result, _title, MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }

                            //성공
                            WinUIMessageBox.Show("완료 되었습니다", _title, MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }


                //PurVo resultVo;
                ////PurVo updateDao;
                //if (isEdit == false)
                //{
                //    updateDao = getDomain();//this.updateDao

                //    // 자동 번호 할당
                //    resultVo = purClient.P4411SelectPurOrdNo(updateDao);
                //    if (!resultVo.isSuccess)
                //    {
                //        //실패
                //        WinUIMessageBox.Show("[발주 번호]" + resultVo.Message, "[에러]발주 등록 관리", MessageBoxButton.OK, MessageBoxImage.Error);
                //        return;
                //    }
                //    updateDao.PUR_ORD_NO = resultVo.LST_FMT_NO;
                //    this.PUR_ORD_NO = updateDao.PUR_ORD_NO;
                //    this.text_PUR_ORD_NO.Text = resultVo.LST_FMT_NO;
                //    //MessageBoxResult result = WinUIMessageBox.Show("[" + updateDao.INSRL_NO + "] 저장 하시겠습니까?", "[전표 번호]창고간 이동", MessageBoxButton.YesNo, MessageBoxImage.Question);
                //    //if (result == MessageBoxResult.Yes)
                //    //{
                //    resultVo = purClient.P4411InsertMst(updateDao);
                //    if (!resultVo.isSuccess)
                //    {
                //        //실패
                //        WinUIMessageBox.Show(resultVo.Message, "[에러]발주 등록 관리", MessageBoxButton.OK, MessageBoxImage.Error);
                //        return;
                //    }
                //    //성공
                //    WinUIMessageBox.Show("[발주 번호 : " + updateDao.PUR_ORD_NO + "] 완료 되었습니다", "[추가]발주 등록 관리", MessageBoxButton.OK, MessageBoxImage.Information);
                //    //}
                //}
                //else
                //{
                //    updateDao = getDomain();
                //    resultVo = purClient.P4411UpdateMst(updateDao);
                //    if (!resultVo.isSuccess)
                //    {
                //        //실패
                //        WinUIMessageBox.Show(resultVo.Message, "[에러]발주 등록 관리", MessageBoxButton.OK, MessageBoxImage.Error);
                //        return;
                //    }
                //    //성공
                //    WinUIMessageBox.Show("완료 되었습니다", "[수정]발주 등록 관리", MessageBoxButton.OK, MessageBoxImage.Information);


                //    this.orgDao.PUR_ORD_NO = this.updateDao.PUR_ORD_NO;
                //    this.orgDao.PUR_DT = this.updateDao.PUR_DT;
                //    this.orgDao.PUR_DUE_DT = this.updateDao.PUR_DUE_DT;
                //    this.orgDao.PUR_CO_CD = this.updateDao.PUR_CO_CD;
                //    this.orgDao.CO_NO = this.updateDao.CO_NO;
                //    this.orgDao.CO_NM = this.updateDao.CO_NM;
                //    this.orgDao.PUR_ITM_CD = this.updateDao.PUR_ITM_CD;
                //    this.orgDao.PUR_ITM_NM = this.updateDao.PUR_ITM_NM;
                //    this.orgDao.PUR_RMK = this.updateDao.PUR_RMK;
                //    this.orgDao.PUR_AMT = this.updateDao.PUR_AMT;
                //    this.orgDao.AREA_CD = this.updateDao.AREA_CD;
                //    this.orgDao.AREA_NM = this.updateDao.AREA_NM;
                //    this.orgDao.PUR_EMPE_ID = this.updateDao.PUR_EMPE_ID;
                //    this.orgDao.USR_NM = this.updateDao.USR_NM;
                //    this.orgDao.PUR_CLZ_FLG = this.updateDao.PUR_CLZ_FLG;
                //    this.orgDao.ORD_RMK = this.updateDao.ORD_RMK;
                //    this.orgDao.QC_CD = this.updateDao.QC_CD;
                //    this.orgDao.ORD_RMK = this.updateDao.ORD_RMK;
                //    this.orgDao.QC_CD = this.updateDao.QC_CD;
                //    this.orgDao.ORD_RMK = this.updateDao.ORD_RMK;
                //    this.orgDao.QC_CD = this.updateDao.QC_CD;
                //    this.orgDao.ORD_RMK = this.updateDao.ORD_RMK;
                //    this.orgDao.QC_CD = this.updateDao.QC_CD;
                //    this.orgDao.ORD_RMK = this.updateDao.ORD_RMK;
                //    this.orgDao.QC_CD = this.updateDao.QC_CD;
                //}
                //PUR_ORD_NO = this.updateDao.PUR_ORD_NO;
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

        async void combo_ITM_GRP_CLSS_NM_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            //SystemCodeVo ITM_GRP_CLSS_NM = this.combo_ITM_GRP_CLSS_NM.SelectedItem as SystemCodeVo;
            //if (ITM_GRP_CLSS_NM != null)
            //{
            //    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s141", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { DELT_FLG = "N", ITM_GRP_CLSS_CD = ITM_GRP_CLSS_NM.CLSS_CD, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
            //    {
            //        if (response.IsSuccessStatusCode)
            //        {
            //            this.combo_CMPO_CD.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
            //        }
            //    }
            //}
        }


        #region Functon (ValueCheckd)
        public Boolean ValueCheckd()
        {
            if (string.IsNullOrEmpty(this.text_ASSY_SEQ.Text))
            {
                WinUIMessageBox.Show("[순번] 입력 값이 맞지 않습니다.", "[유효검사]창고간 이동", MessageBoxButton.OK, MessageBoxImage.Warning);
                this.text_ASSY_SEQ.IsTabStop = true;
                this.text_ASSY_SEQ.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(this.text_CMPO_CD.Text))
            {
                WinUIMessageBox.Show("[품목] 입력 값이 맞지 않습니다.", "[유효검사]발주 등록 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
                this.text_CMPO_CD.IsTabStop = true;
                this.text_CMPO_CD.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(this.combo_ROUT_NM.Text))
            {
                WinUIMessageBox.Show("[실적 공정] 입력 값이 맞지 않습니다.", "[유효검사]발주 등록 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
                this.combo_ROUT_NM.IsTabStop = true;
                this.combo_ROUT_NM.Focus();
                return false;
            }
            //else if (string.IsNullOrEmpty(this.combo_CO_NO.Text))
            //{
            //    WinUIMessageBox.Show("[매입처] 입력 값이 맞지 않습니다.", "[유효검사]발주 등록 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.combo_CO_NO.IsTabStop = true;
            //    this.combo_CO_NO.Focus();
            //    return false;
            //}
            //else if (string.IsNullOrEmpty(this.combo_PUR_ITM_NM.Text))
            //{
            //    WinUIMessageBox.Show("[발주 품목] 입력 값이 맞지 않습니다.", "[유효검사]발주 등록 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.combo_PUR_ITM_NM.IsTabStop = true;
            //    this.combo_PUR_ITM_NM.Focus();
            //    return false;
            //}
            ////else if (string.IsNullOrEmpty(this.text_QC_CD.Text))
            ////{
            ////    WinUIMessageBox.Show("[품질 등급] 입력 값이 맞지 않습니다.", "[유효검사]발주 등록 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
            ////    this.text_QC_CD.IsTabStop = true;
            ////    this.text_QC_CD.Focus();
            ////    return false;
            ////}
            //else if (string.IsNullOrEmpty(this.text_PUR_CLZ_FLG.Text))
            //{
            //    WinUIMessageBox.Show("[마감 유무] 입력 값이 맞지 않습니다.", "[유효검사]발주 등록 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.text_PUR_CLZ_FLG.IsTabStop = true;
            //    this.text_PUR_CLZ_FLG.Focus();
            //    return false;
            //}
            //else if (string.IsNullOrEmpty(this.combo_RQST_EMPE_ID.Text))
            //{
            //    WinUIMessageBox.Show("[요청자] 입력 값이 맞지 않습니다.", "[유효검사]창고간 이동", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.combo_RQST_EMPE_ID.IsTabStop = true;
            //    this.combo_RQST_EMPE_ID.Focus();
            //    return false;
            //}
            //else if (string.IsNullOrEmpty(this.text_CAR_NO.Text))
            //{
            //    WinUIMessageBox.Show("[차량 넘버] 입력 값이 맞지 않습니다.", "[유효검사]창고간 이동", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.text_CAR_NO.IsTabStop = true;
            //    this.text_CAR_NO.Focus();
            //    return false;
            //}
            //else
            //{
            //    //서버와 날짜 체크

            //    if (this.isEdit == false)
            //    {
            //        InauditVo dao = new InauditVo()
            //        {
            //            INAUD_DT = Convert.ToDateTime(this.text_INAUD_DT.Text)
            //        };

            //        InauditVo daoList = inauditclient.SelectInvtInaudCheckTime(dao);
            //        if (daoList.MODI_FLG.Equals("N"))
            //        {
            //            WinUIMessageBox.Show("[이동 일자] 일자를 다시 입력 하십시오.", "[날짜검사]창고간 이동", MessageBoxButton.OK, MessageBoxImage.Warning);
            //            this.text_INAUD_DT.IsTabStop = true;
            //            this.text_INAUD_DT.Focus();
            //            return false;
            //        }
            //    }
            //    //if (this.isEdit == false)
            //    //{
            //    //    InauditVo dao = new InauditVo()
            //    //    {
            //    //        INSRL_NO = this.text_INSRL_NO.Text,
            //    //    };
            //    //    IList<InauditVo> daoList = (IList<InauditVo>)inauditclient.SelectInvtInaudMastList(dao);
            //    //    if (daoList.Count != 0)
            //    //    {
            //    //        WinUIMessageBox.Show("[코드 - 중복] 코드를 다시 입력 하십시오.", "[중복검사]창고간 이동", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    //        this.text_INSRL_NO.IsTabStop = true;
            //    //        this.text_INSRL_NO.Focus();
            //    //        return false;
            //    //    }
            //    //}
            //}
            return true;
        }
        #endregion

        #region Functon (getDomain - ConfigView1Dao)
        private PurVo getDomain()
        {
            PurVo Dao = new PurVo();

            Dao.ITM_CD = this.orgDao.ITM_CD;
            Dao.INP_QTY = Convert.ToDecimal(this.text_INP_QTY.Text);
            Dao.ASSY_SEQ = Convert.ToInt32( this.text_ASSY_SEQ.Text);

            Dao.CMPO_CD = this.text_CMPO_CD.Text;

            SystemCodeVo prntRoutVo = this.combo_PRNT_ROUT_NM.SelectedItem as SystemCodeVo;
            if (prntRoutVo != null)
            {
                Dao.PRNT_ROUT_CD = prntRoutVo.CLSS_CD;
                Dao.PRNT_ROUT_NM = prntRoutVo.CLSS_DESC;
            }

            //SystemCodeVo cmpoVo = this.combo_CMPO_CD.SelectedItem as SystemCodeVo;
            //if (cmpoVo != null)
            //{
            //    Dao.CMPO_CD = cmpoVo.ITM_CD;
            //}

            ManVo routVo = this.combo_ROUT_NM.SelectedItem as ManVo;
            if (routVo != null)
            {
                Dao.ROUT_CD = routVo.ROUT_CD;
                Dao.ROUT_NM = routVo.ROUT_NM;
            }

            Dao.CRE_USR_ID = SystemProperties.USER;
            Dao.UPD_USR_ID = SystemProperties.USER;
            Dao.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;

            return Dao;
        }
        #endregion

        //IsEdit
        public bool IsEdit
        {
            get
            {
                return this.isEdit;
            }
        }

        public async void SYSTEM_CODE_VO()
        {
            try
            {
                DXSplashScreen.Show<ProgressWindow>();

                //this.combo_AREA_CD.ItemsSource = SystemProperties.SYSTEM_CODE_VO("L-002");
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "L-908"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.combo_PRNT_ROUT_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                        this.combo_PRNT_ROUT_NM.SelectedItem = (this.combo_PRNT_ROUT_NM.ItemsSource as List<SystemCodeVo>).Where(x => x.CLSS_CD.Equals(this.orgDao.PRNT_ROUT_CD)).LastOrDefault<SystemCodeVo>();
                        //this.combo_ITM_GRP_CLSS_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                        //this.combo_PRNT_ROUT_NM.SelectedIndex = 1;
                    }
                }

                IList<SystemCodeVo> _CodeList = new List<SystemCodeVo>();
                _CodeList.Add(new SystemCodeVo() { CLSS_CD = "B" , CLSS_DESC = "부자재" });
                _CodeList.Add(new SystemCodeVo() { CLSS_CD = "W" , CLSS_DESC = "벌크(반제품)" });
                _CodeList.Add(new SystemCodeVo() { CLSS_CD = "H", CLSS_DESC = "재공품" });

                this.combo_ITM_GRP_CLSS_NM.ItemsSource = _CodeList;
                //this.combo_ITM_GRP_CLSS_NM.SelectedIndex = 1;

                //}
                //}


                //수정
                //if (this.orgDao.CMPO_CD != null)
                //{
                //    using (HttpResponseMessage responseY = await SystemProperties.PROGRAM_HTTP.PostAsync("s141", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { DELT_FLG = "N", ITM_GRP_CLSS_CD = this.orgDao.ITM_GRP_CLSS_CD, ITM_CD = this.orgDao.CMPO_CD, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                //    {
                //        if (responseY.IsSuccessStatusCode)
                //        {
                //            this.combo_CMPO_CD.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await responseY.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                //            //if (string.IsNullOrEmpty(this.orgDao.ASSY_ITM_CD))
                //            //{
                //            //    this.combo_ASSY_ITM_CD.SelectedItem = (this.combo_ASSY_ITM_CD.ItemsSource as List<SystemCodeVo>)[0];
                //            //    //this.combo_ASSY_ITM_CD.SelectedItem = (this.combo_ASSY_ITM_CD.ItemsSource as List<SystemCodeVo>).Where(x => x.ITM_CD.Equals(this.orgDao.ASSY_ITM_CD)).LastOrDefault<SystemCodeVo>();
                //            //}
                //        }
                //    }
                //}
                //else
                //{
                //    using (HttpResponseMessage responseY = await SystemProperties.PROGRAM_HTTP.PostAsync("s141", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { DELT_FLG = "N", ITM_GRP_CLSS_CD = "B", CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                //    {
                //        if (responseY.IsSuccessStatusCode)
                //        {
                //            this.combo_CMPO_CD.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await responseY.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                //            //if (string.IsNullOrEmpty(this.orgDao.ASSY_ITM_CD))
                //            //{
                //            //    this.combo_ASSY_ITM_CD.SelectedItem = (this.combo_ASSY_ITM_CD.ItemsSource as List<SystemCodeVo>)[0];
                //            //    //this.combo_ASSY_ITM_CD.SelectedItem = (this.combo_ASSY_ITM_CD.ItemsSource as List<SystemCodeVo>).Where(x => x.ITM_CD.Equals(this.orgDao.ASSY_ITM_CD)).LastOrDefault<SystemCodeVo>();
                //            //}
                //        }
                //    }
                //}


                ////수정 -- 공정
                //using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6611", new StringContent(JsonConvert.SerializeObject(new ManVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                //{
                //    if (response.IsSuccessStatusCode)
                //    {
                //        this.combo_ROUT_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
                //    }
                //}

                //, ITM_CD = this.orgDao.ITM_CD
                using (HttpResponseMessage responseX = await SystemProperties.PROGRAM_HTTP.PostAsync("m66212/dtl", new StringContent(JsonConvert.SerializeObject(new ManVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD, ITM_CD = this.orgDao.ITM_CD }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (responseX.IsSuccessStatusCode)
                    {
                        this.combo_ROUT_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await responseX.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
                    }
                }


                DXSplashScreen.Close();
            }
            catch (Exception)
            {
                DXSplashScreen.Close();
                return;
            }
        }

    }
}
