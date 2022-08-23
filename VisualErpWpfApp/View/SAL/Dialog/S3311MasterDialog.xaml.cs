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

namespace AquilaErpWpfApp3.View.SAL.Dialog
{
    public partial class S3311MasterDialog : DXWindow
    {
        //private static SaleOrderServiceClient saleOrderClient = SystemProperties.SaleOrderClient;
        private SaleVo orgDao;
        private bool isEdit = false;
        public SaleVo updateDao;

        private string title = "거래처별 판가기준표";

        public S3311MasterDialog(SaleVo Dao)
        {
            InitializeComponent();

            SYSTEM_CODE_VO();

            //this.combo_CLT_CO_NM.ItemsSource = SystemProperties.CUSTOMER_CODE_VO("AR", SystemProperties.USER_VO.EMPE_PLC_NM);
            //this.combo_AREA_NM.ItemsSource = SystemProperties.SYSTEM_CODE_VO("L-002");

            //this.combo_CLT_NM.ItemsSource = SystemProperties.SYSTEM_CODE_VO("S-034");
            //this.combo_CLT_BIL_NM.ItemsSource = SystemProperties.SYSTEM_CODE_VO("S-035");

            this.orgDao = Dao;

            SaleVo copyDao = new SaleVo()
            {
                SL_AREA_CD = Dao.SL_AREA_CD,
                SL_AREA_NM = Dao.SL_AREA_NM,
                CO_NO = Dao.CO_NO,
                CO_NM = Dao.CO_NM,
                FM_DT = Dao.FM_DT,
                DELT_FLG = Dao.DELT_FLG,
                CRE_USR_ID = Dao.CRE_USR_ID,
                UPD_USR_ID = Dao.UPD_USR_ID,
            };

            ////수정
            //if (copyDao.CLT_BIL_DELT_NO != null)
            //{
            //    //this.text_CLT_BIL_DELT_NO.IsReadOnly = true;
            //    this.isEdit = true;
            //    //CLT_BIL_DELT_NO = this.orgDao.CLT_BIL_DELT_NO;
            //}
            //else
            //{
            //    //추가
            //    //this.text_CLT_BIL_DELT_NO.IsReadOnly = true;
                this.isEdit = false;
            //    //copyDao.FM_DT = System.DateTime.Now.ToString("yyyy-MM-dd");

            //    //copyDao.BIL_ST_DT = System.DateTime.Now.ToString("yyyy-MM-dd");
            //    //copyDao.BIL_END_DT = System.DateTime.Now.AddYears(1).ToString("yyyy-MM-dd");

            //    //copyDao.CLT_AMT = 0;
            //    //copyDao.CLT_DELT_AMT = 0;
            //}
            this.configCode.DataContext = copyDao;
            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
            this.OKButton.Click += new RoutedEventHandler(OKButton_Click);
            this.CancelButton.Click += new RoutedEventHandler(CancelButton_Click);

            //this.combo_CLT_BIL_NM.SelectedIndexChanged += combo_CLT_BIL_NM_SelectedIndexChanged;
            this.combo_AREA_NM.SelectedIndexChanged += combo_AREA_NM_SelectedIndexChanged;


            //this.FM_DT.Focus();
        }


        async void combo_AREA_NM_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            SystemCodeVo areaNmVo = this.combo_AREA_NM.SelectedItem as SystemCodeVo;
            if (areaNmVo != null)
            {
                //this.combo_CLT_CO_NM.ItemsSource = SystemProperties.CUSTOMER_CODE_VO("AR", areaNmVo.CLSS_CD);
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s143", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { DELT_FLG = "N", CO_TP_CD = "AR", AREA_CD = areaNmVo.CLSS_CD, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.combo_CO_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                    }
                }
            }
        }

        //void combo_CLT_BIL_NM_SelectedIndexChanged(object sender, RoutedEventArgs e)
        //{
        //    CodeDao cltBilNmVo = this.combo_CLT_BIL_NM.SelectedItem as CodeDao;
        //    if (cltBilNmVo != null)
        //    {
        //        if (cltBilNmVo.CLSS_DESC.Equals("통장") || (cltBilNmVo.CLSS_DESC.Equals("카드")))
        //        {
        //            this.label_BANK_ACCT_NM.Foreground = Brushes.Black;
        //            this.combo_BANK_ACCT_NM.IsEnabled = true;

        //            //
        //            this.label_BIL_TP_NM.Foreground = Brushes.DarkGray;
        //            this.label_BIL_ACCT_NO.Foreground = Brushes.DarkGray;
        //            this.label_BIL_ST_DT.Foreground = Brushes.DarkGray;
        //            this.label_BIL_END_DT.Foreground = Brushes.DarkGray;
        //            this.label_BIL_USR_NM.Foreground = Brushes.DarkGray;
        //            this.label_BIL_BANK_NM.Foreground = Brushes.DarkGray;
        //            this.label_BIL_IN_NO.Foreground = Brushes.DarkGray;
        //            //
        //            this.combo_BIL_TP_NM.IsEnabled = false;
        //            this.text_BIL_ACCT_NO.IsEnabled = false;
        //            this.text_BIL_ST_DT.IsEnabled = false;
        //            this.text_BIL_END_DT.IsEnabled = false;
        //            this.text_BIL_USR_NM.IsEnabled = false;
        //            this.text_BIL_BANK_NM.IsEnabled = false;
        //            this.text_BIL_IN_NO.IsEnabled = false; 
        //        }
        //        else if (cltBilNmVo.CLSS_DESC.Equals("어음(자수)") || (cltBilNmVo.CLSS_DESC.Equals("어음(타수)")))
        //        {
        //            this.label_BIL_TP_NM.Foreground = Brushes.Black;
        //            this.label_BIL_ACCT_NO.Foreground = Brushes.Black;
        //            this.label_BIL_ST_DT.Foreground = Brushes.Black;
        //            this.label_BIL_END_DT.Foreground = Brushes.Black;
        //            this.label_BIL_USR_NM.Foreground = Brushes.Black;
        //            this.label_BIL_BANK_NM.Foreground = Brushes.Black;
        //            this.label_BIL_IN_NO.Foreground = Brushes.Black;
        //            //
        //            this.combo_BIL_TP_NM.IsEnabled = true;
        //            this.text_BIL_ACCT_NO.IsEnabled = true;
        //            this.text_BIL_ST_DT.IsEnabled = true;
        //            this.text_BIL_END_DT.IsEnabled = true;
        //            this.text_BIL_USR_NM.IsEnabled = true;
        //            this.text_BIL_BANK_NM.IsEnabled = true;
        //            this.text_BIL_IN_NO.IsEnabled = true; 

        //            //
        //            this.label_BANK_ACCT_NM.Foreground = Brushes.DarkGray;
        //            this.combo_BANK_ACCT_NM.IsEnabled = false;
        //        }
        //        else
        //        {
        //            this.label_BIL_TP_NM.Foreground = Brushes.DarkGray;
        //            this.label_BIL_ACCT_NO.Foreground = Brushes.DarkGray;
        //            this.label_BIL_ST_DT.Foreground = Brushes.DarkGray;
        //            this.label_BIL_END_DT.Foreground = Brushes.DarkGray;
        //            this.label_BIL_USR_NM.Foreground = Brushes.DarkGray;
        //            this.label_BIL_BANK_NM.Foreground = Brushes.DarkGray;
        //            this.label_BIL_IN_NO.Foreground = Brushes.DarkGray;
        //            //
        //            this.combo_BIL_TP_NM.IsEnabled = false;
        //            this.text_BIL_ACCT_NO.IsEnabled = false;
        //            this.text_BIL_ST_DT.IsEnabled = false;
        //            this.text_BIL_END_DT.IsEnabled = false;
        //            this.text_BIL_USR_NM.IsEnabled = false;
        //            this.text_BIL_BANK_NM.IsEnabled = false;
        //            this.text_BIL_IN_NO.IsEnabled = false;

        //            //
        //            this.label_BANK_ACCT_NM.Foreground = Brushes.DarkGray;
        //            this.combo_BANK_ACCT_NM.IsEnabled = false;
        //        }
        //    }
        //}

        #region Functon (OKButton_Click, CancelButton_Click)
        private async void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValueCheckd())
            {
                //JobVo resultVo;
                ////PurVo updateDao;
                //if (isEdit == false)
                //{
                //    updateDao = getDomain();//this.updateDao

                //    // 자동 번호 할당
                //    resultVo = saleOrderClient.S2222SelectCltBilDeltNo(updateDao);
                //    if (!resultVo.isSuccess)
                //    {
                //        //실패
                //        WinUIMessageBox.Show(resultVo.Message, "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
                //        return;
                //    }
                //    CLT_BIL_DELT_NO = resultVo.LST_FMT_NO;
                //    updateDao.CLT_BIL_DELT_NO = resultVo.LST_FMT_NO;
                //    this.text_CLT_BIL_DELT_NO.Text = resultVo.LST_FMT_NO;
                //    //MessageBoxResult result = WinUIMessageBox.Show("[" + updateDao.INSRL_NO + "] 저장 하시겠습니까?", "[전표 번호]창고간 이동", MessageBoxButton.YesNo, MessageBoxImage.Question);
                //    //if (result == MessageBoxResult.Yes)
                //    //{
                //    resultVo = saleOrderClient.S2222InsertMst(updateDao);
                //    if (!resultVo.isSuccess)
                //    {
                //        //실패
                //        WinUIMessageBox.Show(resultVo.Message, "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
                //        return;
                //    }
                //    //성공
                //    WinUIMessageBox.Show("[상계 번호 : " + updateDao.CLT_BIL_DELT_NO + "] 완료 되었습니다", "[추가]" + title, MessageBoxButton.OK, MessageBoxImage.Information);
                //    //}
                //}
                //else
                //{
                //    updateDao = getDomain();
                //    resultVo = saleOrderClient.S2222UpdateMst(updateDao);
                //    if (!resultVo.isSuccess)
                //    {
                //        //실패
                //        WinUIMessageBox.Show(resultVo.Message, "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
                //        return;
                //    }
                //    //성공
                //    WinUIMessageBox.Show("완료 되었습니다", "[수정]" + title, MessageBoxButton.OK, MessageBoxImage.Information);



                //    this.orgDao.CLT_BIL_DELT_NO = this.updateDao.CLT_BIL_DELT_NO;
                //    this.orgDao.CLT_BIL_DELT_DT = this.updateDao.CLT_BIL_DELT_DT;
                //    this.orgDao.AREA_CD = this.updateDao.AREA_CD;
                //    this.orgDao.AREA_NM = this.updateDao.AREA_NM;
                //    this.orgDao.CLT_CO_CD = this.updateDao.CLT_CO_CD;
                //    this.orgDao.CLT_CO_NM = this.updateDao.CLT_CO_NM;
                //    this.orgDao.CLT_USR_NM = this.updateDao.CLT_USR_NM;
                //    this.orgDao.CLT_CD = this.updateDao.CLT_CD;
                //    this.orgDao.CLT_NM = this.updateDao.CLT_NM;
                //    this.orgDao.CLT_RMN_AMT = this.updateDao.CLT_RMN_AMT;
                //    this.orgDao.CLT_DELT_AMT = this.updateDao.CLT_DELT_AMT;
                //    this.orgDao.CLT_BIL_CD = this.updateDao.CLT_BIL_CD;
                //    this.orgDao.CLT_BIL_NM = this.updateDao.CLT_BIL_NM;

                //    this.orgDao.BIL_IN_NO = this.updateDao.BIL_IN_NO;

                //    this.orgDao.CLT_DELT_RMK = this.updateDao.CLT_DELT_RMK;
                //    this.orgDao.CRE_USR_ID = this.updateDao.CRE_USR_ID;
                //    this.orgDao.UPD_USR_ID = this.updateDao.UPD_USR_ID;
                //}

                int _Num = 0;
                //SystemCodeVo resultVo;
                //if (isEdit == false)
                //{
                    this.updateDao = getDomain();

                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s3311/mst/i", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
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
                //}
                //else
                //{
                //    this.updateDao = getDomain();

                //    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s2222/mst/u", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
                //    {
                //        if (response.IsSuccessStatusCode)
                //        {
                //            string result = await response.Content.ReadAsStringAsync();
                //            if (int.TryParse(result, out _Num) == false)
                //            {
                //                //실패
                //                WinUIMessageBox.Show(result, title, MessageBoxButton.OK, MessageBoxImage.Error);
                //                return;
                //            }

                //            //성공
                //            WinUIMessageBox.Show("완료 되었습니다", title, MessageBoxButton.OK, MessageBoxImage.Information);
                //        }
                //    }
                //}

                ////
                //if (updateDao.CLZ_FLG.Equals("Y"))
                //{
                //    //수주 자재 리스트  (자동 마감)
                //    resultVo = saleOrderClient.S2211UpdateDtl(new JobVo() { JB_NO = this.orgDao.JB_NO, CLZ_FLG = "Y", CRE_USR_ID = SystemProperties.USER, UPD_USR_ID = SystemProperties.USER });
                //    if (!resultVo.isSuccess)
                //    {
                //        //실패
                //        WinUIMessageBox.Show(resultVo.Message, "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
                //        return;
                //    }
                //}

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
            //if (string.IsNullOrEmpty(this.text_SL_BIL_RTN_NO.Text))
            //{
            //    WinUIMessageBox.Show("[반품 번호] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.text_SL_BIL_RTN_NO.IsTabStop = true;
            //    this.text_SL_BIL_RTN_NO.Focus();
            //    return false;
            //}
            //else 
            if (string.IsNullOrEmpty(this.text_FM_DT.Text))
            {
                WinUIMessageBox.Show("[기준 일자] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.text_FM_DT.IsTabStop = true;
                this.text_FM_DT.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(this.combo_AREA_NM.Text))
            {
                WinUIMessageBox.Show("[사업장] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.combo_AREA_NM.IsTabStop = true;
                this.combo_AREA_NM.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(this.combo_CO_NM.Text))
            {
                WinUIMessageBox.Show("[거래처] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.combo_CO_NM.IsTabStop = true;
                this.combo_CO_NM.Focus();
                return false;
            }
            //else if (string.IsNullOrEmpty(this.combo_RGST_USR_ID.Text))
            //{
            //    WinUIMessageBox.Show("[작성자] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.combo_RGST_USR_ID.IsTabStop = true;
            //    this.combo_RGST_USR_ID.Focus();
            //    return false;
            //}
            //else if (string.IsNullOrEmpty(this.text_CLZ_FLG.Text))
            //{
            //    WinUIMessageBox.Show("[마감 유무] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.text_CLZ_FLG.IsTabStop = true;
            //    this.text_CLZ_FLG.Focus();
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

            //Dao.CLT_BIL_DELT_NO = this.text_CLT_BIL_DELT_NO.Text;

            Dao.FM_DT = Convert.ToDateTime(this.text_FM_DT.Text).ToString("yyyy-MM-dd");

            SystemCodeVo coNmVo = this.combo_CO_NM.SelectedItem as SystemCodeVo;
            if (coNmVo != null)
            {
                Dao.CO_CD = coNmVo.CO_NO;
                Dao.CO_NO = coNmVo.CO_NO;
                Dao.CO_NM = coNmVo.CO_NM;
            }

            SystemCodeVo areaNmVo = this.combo_AREA_NM.SelectedItem as SystemCodeVo;
            if (areaNmVo != null)
            {
                Dao.SL_AREA_CD = areaNmVo.CLSS_CD;
                Dao.SL_AREA_NM = areaNmVo.CLSS_DESC;
            }

            //Dao.CLT_USR_NM = this.text_CLT_USR_NM.Text;

            //SystemCodeVo cltNmVo = this.combo_CLT_NM.SelectedItem as SystemCodeVo;
            //if (cltNmVo != null)
            //{
            //    Dao.CLT_CD = cltNmVo.CLSS_CD;
            //    Dao.CLT_NM = cltNmVo.CLSS_DESC;
            //}
            //Dao.CLT_DELT_AMT = (this.text_CLT_DELT_AMT.Text);
            ////Dao.CLT_DC_RT = (this.text_CLT_DC_RT.Text);

            //SystemCodeVo cltBilNmVo = this.combo_CLT_BIL_NM.SelectedItem as SystemCodeVo;
            //if (cltBilNmVo != null)
            //{
            //    Dao.CLT_BIL_CD = cltBilNmVo.CLSS_CD;
            //    Dao.CLT_BIL_NM = cltBilNmVo.CLSS_DESC;
            //}

            //Dao.BIL_IN_NO = this.text_BIL_IN_NO.Text;

            //Dao.CLT_DELT_RMK = this.text_CLT_DELT_RMK.Text;

            //if (cltBilNmVo.CLSS_DESC.Equals("통장") || (cltBilNmVo.CLSS_DESC.Equals("카드")))
            //{
            //    BankCodeDao bankVo = this.combo_BANK_ACCT_NM.SelectedItem as BankCodeDao;
            //    if (bankVo != null)
            //    {
            //        Dao.BANK_ACCT_CD = bankVo.BANK_ACCT_CD;
            //        Dao.BANK_ACCT_NM = bankVo.BANK_ACCT_NM;

            //        //
            //        Dao.BIL_TP_NM = "";
            //        Dao.BIL_ACCT_NO = "";
            //        Dao.BIL_ST_DT = "";
            //        Dao.BIL_END_DT = "";
            //        Dao.BIL_USR_NM = "";
            //        Dao.BIL_BANK_NM = "";
            //        Dao.BIL_IN_NO = "";

            //    }
            //}
            //else if (cltBilNmVo.CLSS_DESC.Equals("어음(자수)") || (cltBilNmVo.CLSS_DESC.Equals("어음(타수)")))
            //{
            //    Dao.BANK_ACCT_CD = "";
            //    Dao.BANK_ACCT_NM = "";

            //    //
            //    CodeDao bilTpNmVo = this.combo_BIL_TP_NM.SelectedItem as CodeDao;
            //    if (bilTpNmVo != null)
            //    {
            //        Dao.BIL_TP_CD = bilTpNmVo.CLSS_CD;
            //        Dao.BIL_TP_NM = bilTpNmVo.CLSS_DESC;
            //    }

            //    Dao.BIL_ACCT_NO = this.text_BIL_ACCT_NO.Text;
            //    Dao.BIL_ST_DT = Convert.ToDateTime(this.text_BIL_ST_DT.Text).ToString("yyyy-MM-dd");
            //    Dao.BIL_END_DT = Convert.ToDateTime(this.text_BIL_END_DT.Text).ToString("yyyy-MM-dd");

            //    Dao.BIL_USR_NM = this.text_BIL_USR_NM.Text;
            //    Dao.BIL_BANK_NM = this.text_BIL_BANK_NM.Text;
            //    Dao.BIL_IN_NO = this.text_BIL_IN_NO.Text;
            //}

            //CodeDao bilRmkNmVo = this.combo_BIL_RMK_NM.SelectedItem as CodeDao;
            //if (bilRmkNmVo != null)
            //{
            //    Dao.BIL_RMK_CD = bilRmkNmVo.CLSS_CD;
            //    Dao.BIL_RMK_NM = bilRmkNmVo.CLSS_DESC;
            //}

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

        //public string CLT_BIL_DELT_NO
        //{ get; set; }



        public async void SYSTEM_CODE_VO()
        {
            //this.combo_CLT_CO_NM.ItemsSource = SystemProperties.CUSTOMER_CODE_VO("AR", SystemProperties.USER_VO.EMPE_PLC_NM);
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s143", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { DELT_FLG = "N", CO_TP_CD = "AR", AREA_CD = "100", CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.combo_CO_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                }
            }

            //this.combo_AREA_NM.ItemsSource = SystemProperties.SYSTEM_CODE_VO("L-002");
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "L-002"))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.combo_AREA_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                }
            }

            //this.combo_CLT_NM.ItemsSource = SystemProperties.SYSTEM_CODE_VO("S-034");
            //using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "S-034"))
            //{
            //    if (response.IsSuccessStatusCode)
            //    {
            //        this.combo_CLT_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
            //    }
            //}

            ////this.combo_CLT_BIL_NM.ItemsSource = SystemProperties.SYSTEM_CODE_VO("S-035");
            //using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "S-035"))
            //{
            //    if (response.IsSuccessStatusCode)
            //    {
            //        this.combo_CLT_BIL_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
            //    }
            //}

        }

    }
}
