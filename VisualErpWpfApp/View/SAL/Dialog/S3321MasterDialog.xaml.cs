using AquilaErpWpfApp3.Util;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Code;
using ModelsLibrary.Pur;
using ModelsLibrary.Sale;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;

namespace AquilaErpWpfApp3.View.SAL.Dialog
{
    public partial class S3321MasterDialog : DXWindow
    {
        //private static PurServiceClient purClient = SystemProperties.PurClient;
        private SaleVo orgDao;
        private bool isEdit = false;
        private SaleVo updateDao;

        private string _title = "수주등록";

        public S3321MasterDialog(SaleVo Dao)
        {
            InitializeComponent();

            SYSTEM_CODE_VO();

            this.orgDao = Dao;

            SaleVo copyDao = new SaleVo()
            {
                SL_RLSE_NO = Dao.SL_RLSE_NO,
                SL_RLSE_DT = Dao.SL_RLSE_DT,
                DUE_DT = Dao.DUE_DT,
                SL_CO_CD = Dao.SL_CO_CD,
                CO_NM = Dao.CO_NM,
                SL_RMK = Dao.SL_RMK,
                SL_AREA_CD = Dao.SL_AREA_CD,
                SL_AREA_NM = Dao.SL_AREA_NM,
                AREA_CD = Dao.AREA_CD,
                AREA_NM = Dao.AREA_NM,
                CHNL_CD = SystemProperties.USER_VO.CHNL_CD
            };

            //수정
            if (Dao.SL_RLSE_NO != null)
            {
                this.text_SL_RLSE_NO.IsReadOnly = true;
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
                this.text_SL_RLSE_NO.IsReadOnly = true;
                this.isEdit = false;

                Dao.SL_RLSE_DT = System.DateTime.Now.ToString("yyyy-MM-dd");
              
               //  Dao.PUR_ITM_NM = "원자재";
               // this.combo_PUR_ITM_NM.IsReadOnly = true;
                //
                //Dao.DO_RQST_GRP_NM = SystemProperties.USERVO.GRP_NM;
                //Dao.DO_RQST_USR_NM = SystemProperties.USERVO.USR_N1ST_NM;
                //this.combo_DO_RQST_USR_NM.SelectedItem = ((IList<CodeDao>)combo_DO_RQST_USR_NM.ItemsSource)[2];
            }
            this.configCode.DataContext = orgDao;
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
                //ProgramVo resultVo;
                if (isEdit == false)
                {
                    this.updateDao = getDomain();//this.updateDao
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("S3321/mst/i", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
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

                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("S3321/mst/u", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
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
                SL_RLSE_NO = this.updateDao.SL_RLSE_NO;
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

            if (string.IsNullOrEmpty(this.text_SL_RLSE_DT.Text))
            {
                WinUIMessageBox.Show("[수주 일자] 입력 값이 맞지 않습니다.", _title  , MessageBoxButton.OK, MessageBoxImage.Warning);
                this.text_SL_RLSE_DT.IsTabStop = true;
                this.text_SL_RLSE_DT.Focus();
                return false;
            }
            //else if (string.IsNullOrEmpty(this.text_PUR_DUE_DT.Text))
            //{
            //    WinUIMessageBox.Show("[납기일] 입력 값이 맞지 않습니다.", _title, MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.text_PUR_DUE_DT.IsTabStop = true;
            //    this.text_PUR_DUE_DT.Focus();
            //    return false;
            //}
            else if (string.IsNullOrEmpty(this.combo_CO_NO.Text))
            {
                WinUIMessageBox.Show("[거래처] 입력 값이 맞지 않습니다.", _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.combo_CO_NO.IsTabStop = true;
                this.combo_CO_NO.Focus();
                return false;
            }
            //else if (string.IsNullOrEmpty(this.combo_PUR_ITM_NM.Text))
            //{
            //    WinUIMessageBox.Show("[발주 품목] 입력 값이 맞지 않습니다.", _title, MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.combo_PUR_ITM_NM.IsTabStop = true;
            //    this.combo_PUR_ITM_NM.Focus();
            //    return false;
            //}
            //else if (string.IsNullOrEmpty(this.text_QC_CD.Text))
            //{
            //    WinUIMessageBox.Show("[품질 등급] 입력 값이 맞지 않습니다.", "[유효검사]발주 등록 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.text_QC_CD.IsTabStop = true;
            //    this.text_QC_CD.Focus();
            //    return false;
            //}
            //else if (string.IsNullOrEmpty(this.text_PUR_CLZ_FLG.Text))
            //{
            //    WinUIMessageBox.Show("[마감 유무] 입력 값이 맞지 않습니다.", "[유효검사]원자재 발주서 등록", MessageBoxButton.OK, MessageBoxImage.Warning);
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
        private SaleVo getDomain()
        {
            SaleVo Dao = new SaleVo();

            //Dao.PUR_ORD_NO = this.text_PUR_NO.Text;
            Dao.SL_RLSE_NO = this.text_SL_RLSE_NO.Text;

            SystemCodeVo areaVo = this.combo_SL_AREA_CD.SelectedItem as SystemCodeVo;
            Dao.SL_AREA_CD = areaVo.CLSS_CD;
            Dao.SL_AREA_NM = areaVo.CLSS_DESC;

            Dao.SL_RLSE_DT = Convert.ToDateTime(this.text_SL_RLSE_DT.Text).ToString("yyyy-MM-dd");
            //Dao.PUR_DUE_DT = Convert.ToDateTime(this.text_PUR_DUE_DT.Text).ToString("yyyy-MM-dd");
            //Dao.DUE_DT = Convert.ToDateTime(this.text_PUR_DUE_DT.Text).ToString("yyyy-MM-dd");

            SystemCodeVo coNmVo = this.combo_CO_NO.SelectedItem as SystemCodeVo;
            Dao.SL_CO_CD = coNmVo.CO_NO;
            Dao.CO_NO = coNmVo.CO_NO;
            Dao.CO_NM = coNmVo.CO_NM;

           // SystemCodeVo itmVo = this.combo_PUR_ITM_NM.SelectedItem as SystemCodeVo;
            //Dao.PUR_ITM_CD = itmVo.CLSS_CD;
            //Dao.PUR_ITM_NM = itmVo.CLSS_DESC;


            //UserCodeDao purEmpeIdVo = this.combo_PUR_EMPE_ID.SelectedItem as UserCodeDao;
            //Dao.PUR_EMPE_ID = purEmpeIdVo.USR_ID;
            //Dao.USR_NM = purEmpeIdVo.USR_N1ST_NM;

            Dao.SL_RMK = this.text_SL_RMK.Text;

            //Dao.PUR_CLZ_FLG = this.text_PUR_CLZ_FLG.Text;
            //Dao.DELT_FLG = this.text_PUR_CLZ_FLG.Text;

            Dao.CRE_USR_ID = SystemProperties.USER;
            Dao.UPD_USR_ID = SystemProperties.USER;
            Dao.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
            Dao.CRE_DT = DateTime.Now;
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


        public string SL_RLSE_NO
        {
            get;
            set;
        }


        public async void SYSTEM_CODE_VO()
        {
            //this.combo_AREA_CD.ItemsSource = SystemProperties.SYSTEM_CODE_VO("L-002");
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "L-002"))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.combo_SL_AREA_CD.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                }
            }

            //this.combo_PUR_ITM_NM.ItemsSource = SystemProperties.SYSTEM_CODE_VO("L-111");
            //using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "L-111"))
            //{
            //    if (response.IsSuccessStatusCode)
            //    {
            //        //this.combo_PUR_ITM_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
            //        //this.combo_PUR_ITM_NM.SelectedIndex = 0;
            //        //this.combo_PUR_ITM_NM.IsReadOnly = true;
            //    }
            //}

            //this.combo_CO_NO.ItemsSource = SystemProperties.CUSTOMER_CODE_VO("AP", SystemProperties.USER_VO.EMPE_PLC_NM);
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s143", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { DELT_FLG = "N", SEEK_AP = "AP", SEEK = "AP", CO_TP_CD = "AP", AREA_CD = SystemProperties.USER_VO.EMPE_PLC_NM, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.combo_CO_NO.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                }
            }
        }

    }
}
