using AquilaErpWpfApp3.Util;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Auth;
using ModelsLibrary.Code;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace AquilaErpWpfApp3.View.S.Dialog
{
    /// <summary>
    /// Interaction logic for ConfigView1Dialog.xaml
    /// </summary>
    public partial class S143CustomerDialog : DXWindow
    {
        private string title = "거래처 관리";
        //private static CodeServiceClient customerClient = SystemProperties.CodeClient;
        private SystemCodeVo orgDao;
        private bool isEdit = false;
        //private JusoDialog jusoDialog;
        private SystemCodeVo updateVo;

        //
        private string HDQTR_OLD_ADDR = string.Empty;
        private string N1ST_AREA_OLD_ADDR = string.Empty;
        private string N2ND_AREA_OLD_ADDR = string.Empty;

        public S143CustomerDialog(SystemCodeVo Dao)
        {
            InitializeComponent();


            SYSTEM_CODE_VO();
            USER_CODE_VO();
            //this.combo_AREA_NM.ItemsSource = SystemProperties.SYSTEM_CODE_VO("L-002");
            //this.combo_SL_AREA_NM.ItemsSource = SystemProperties.SYSTEM_CODE_VO("L-009");
            //this.combo_BILL_NM.ItemsSource = SystemProperties.SYSTEM_CODE_VO("S-031");
            //this.combo_PAY_TP_NM.ItemsSource = SystemProperties.SYSTEM_CODE_VO("L-011");
            //this.combo_N1ST_BIZ_COND_NM.ItemsSource = SystemProperties.SYSTEM_CODE_VO("S-002");
            //this.combo_N1ST_BZTP_NM.ItemsSource = SystemProperties.SYSTEM_CODE_VO("S-003");

            //this.combo_TEAM_NM.ItemsSource = SystemProperties.SYSTEM_CODE_VO("L-010");
            //this.combo_TRD_TP_NM.ItemsSource = SystemProperties.SYSTEM_CODE_VO("L-012");
            //this.combo_TRD_CATE_NM.ItemsSource = SystemProperties.SYSTEM_CODE_VO("L-013");

            //this.combo_BANK_NM.ItemsSource = SystemProperties.SYSTEM_CODE_VO("L-014");

            //this.combo_N1ST_SLS_MAN.ItemsSource = SystemProperties.USER_CODE_VO();
            //this.combo_N2ND_SLS_MAN.ItemsSource = SystemProperties.USER_CODE_VO();



            this.HelpButton.Click += HelpButton_Click;
           
            this.TAB1Button.Click += TAB1Button_Click;
            this.TAB2Button.Click += TAB2Button_Click;
            this.TAB3Button.Click += TAB3Button_Click;

            this.orgDao = Dao;
            //
            SystemCodeVo copyDao = new SystemCodeVo()
            {
                CO_NO = Dao.CO_NO,
                CO_NM = Dao.CO_NM,
                CO_RGST_NO = Dao.CO_RGST_NO,
                PRSD_NM = Dao.PRSD_NM,
                HDQTR_PHN_NO =  Dao.HDQTR_PHN_NO,
                //법인 등록 번호
                CORP_CD = Dao.CORP_CD,
                //영업 담당자 : (정)      
                N1ST_SLS_MAN = Dao.N1ST_SLS_MAN,
                N1ST_SLS_MAN_CD = Dao.N1ST_SLS_MAN_CD,
                //영업 담당자 : (부)      
                N2ND_SLS_MAN = Dao.N2ND_SLS_MAN,
                N2ND_SLS_MAN_CD = Dao.N2ND_SLS_MAN_CD,
                //지역 구분
                AREA_CD = Dao.AREA_CD,
                AREA_NM = Dao.AREA_NM,
                //업체 구분
                PAY_TP_CD = Dao.PAY_TP_CD,
                PAY_TP_NM = Dao.PAY_TP_NM,
                //거래 구분
                TRD_TP_CD = Dao.TRD_TP_CD,
                TRD_TP_NM = Dao.TRD_TP_NM,
                //거래처 유형
                CO_TP_CD = Dao.CO_TP_CD,
                CO_TP_NM = Dao.CO_TP_NM,
                //팀 구분
                TEAM_CD = Dao.TEAM_CD,
                TEAM_NM = Dao.TEAM_NM,
                //에누리
                DC_FLG = Dao.DC_FLG,
                //거래 구분
                TRD_CATE_CD = Dao.TRD_CATE_CD,
                TRD_CATE_NM = Dao.TRD_CATE_NM,
                //사업자 구분
                CO_CLSS_CD = Dao.CO_CLSS_CD,
                CO_CLSS_NM = Dao.CO_CLSS_NM,
                //업태
                N1ST_BIZ_COND_CD = Dao.N1ST_BIZ_COND_CD,
                N1ST_BIZ_COND_NM = Dao.N1ST_BIZ_COND_NM,
                //종목
                N1ST_BZTP_CD = Dao.N1ST_BZTP_CD,
                N1ST_BZTP_NM = Dao.N1ST_BZTP_NM,
                //통화 코드
                BSE_CURR_CD = Dao.BSE_CURR_CD,
                BSE_CURR_NM = Dao.BSE_CURR_NM,
                //통화 코드
                INTERRLT_CO_CD = Dao.INTERRLT_CO_CD,
                INTERRLT_CO_NM = Dao.INTERRLT_CO_NM,
                //마감 구분
                PUR_CLZ_CD = Dao.PUR_CLZ_CD,
                PUR_CLZ_NM = Dao.PUR_CLZ_NM,
                //납기 한도 일자
                DUE_DT_PRD_DY = Dao.DUE_DT_PRD_DY,
                //비고
                CO_RMK = Dao.CO_RMK,
                //담당자1
                CNTC_MAN_NM = Dao.CNTC_MAN_NM,
                CNTC_MAN_PSN_NM = Dao.CNTC_MAN_PSN_NM,
                CNTC_MAN_PHN_NO = Dao.CNTC_MAN_PHN_NO,
                CNTC_MAN_EML = Dao.CNTC_MAN_EML,
                //담당자2
                //N2ND_CNTC_MAN_NM = Dao.N2ND_CNTC_MAN_NM,
                //N2ND_CNTC_MAN_PSN_NM = Dao.N2ND_CNTC_MAN_PSN_NM,
                //N2ND_CNTC_MAN_PHN_NO = Dao.N2ND_CNTC_MAN_PHN_NO,
                //N2ND_CNTC_MAN_EML = Dao.N2ND_CNTC_MAN_EML,
                //담당자[계산서]
                TAX_MAN_NM = Dao.TAX_MAN_NM,
                TAX_MAN_PSN_NM = Dao.TAX_MAN_PSN_NM,
                TAX_MAN_PHN_NO = Dao.TAX_MAN_PHN_NO,
                TAX_MAN_EML = Dao.TAX_MAN_EML,
                //은행
                BANK_CD = Dao.BANK_CD,
                BANK_NM = Dao.BANK_NM,
                //예금주
                ACCT_HLD_NM = Dao.ACCT_HLD_NM,
                ACCT_NO = Dao.ACCT_NO,
                //본사
                HDQTR_AREA_NM = Dao.HDQTR_AREA_NM,
                HDQTR_FAX_NO = Dao.HDQTR_FAX_NO,
                HDQTR_ADDR = Dao.HDQTR_ADDR,
                HDQTR_PST_NO = Dao.HDQTR_PST_NO,
                HDQTR_OLD_ADDR = Dao.HDQTR_OLD_ADDR,

                //공장1
                N1ST_CNTC_MAN_NM = Dao.N1ST_CNTC_MAN_NM,
                N1ST_CNTC_MAN_PSN_NM = Dao.N1ST_CNTC_MAN_PSN_NM,
                N1ST_CNTC_MAN_PHN_NO = Dao.N1ST_CNTC_MAN_PHN_NO,
                N1ST_CNTC_MAN_EML = Dao.N1ST_CNTC_MAN_EML,
                
                N1ST_AREA_NM = Dao.N1ST_AREA_NM,
                N1ST_AREA_PHN_NO = Dao.N1ST_AREA_PHN_NO,
                N1ST_AREA_FAX_NO = Dao.N1ST_AREA_FAX_NO,
                N1ST_AREA_ADDR = Dao.N1ST_AREA_ADDR,
                N1ST_AREA_PST_NO = Dao.N1ST_AREA_PST_NO,
                N1ST_AREA_OLD_ADDR = Dao.N1ST_AREA_OLD_ADDR,
                //공장2
                N2ND_AREA_NM = Dao.N2ND_AREA_NM,
                N2ND_AREA_PHN_NO = Dao.N2ND_AREA_PHN_NO,
                N2ND_AREA_FAX_NO = Dao.N2ND_AREA_FAX_NO,
                N2ND_AREA_ADDR = Dao.N2ND_AREA_ADDR,
                N2ND_AREA_PST_NO = Dao.N2ND_AREA_PST_NO,
                N2ND_AREA_OLD_ADDR = Dao.N2ND_AREA_OLD_ADDR,

                N2ND_CNTC_MAN_NM = Dao.N2ND_CNTC_MAN_NM,
                N2ND_CNTC_MAN_PSN_NM = Dao.N2ND_CNTC_MAN_PSN_NM,
                N2ND_CNTC_MAN_PHN_NO = Dao.N2ND_CNTC_MAN_PHN_NO,
                N2ND_CNTC_MAN_EML = Dao.N2ND_CNTC_MAN_EML,
                
                //
                DELT_FLG = Dao.DELT_FLG,
                CRE_USR_ID = Dao.CRE_USR_ID,
                UPD_USR_ID = Dao.UPD_USR_ID,
                HMPG_ADDR = Dao.HMPG_ADDR,

                SL_AREA_CD = Dao.SL_AREA_CD,
                SL_AREA_NM = Dao.SL_AREA_NM,

                BILL_CD = Dao.BILL_CD,
                BILL_NM = Dao.BILL_NM,
                CHNL_CD = SystemProperties.USER_VO.CHNL_CD
            };

            ////수정
            if (Dao.CO_NO != null)
            {
                this.isEdit = true;
                this.text_CO_NO.IsReadOnly = true;
                this.text_CO_NO.Background = Brushes.DarkGray;
            }
            else
            {
                //추가
                this.isEdit = false;
                copyDao.DELT_FLG = "사용";
                copyDao.DC_FLG = "사용";
               //Dao.JOIN_CO_DT = System.DateTime.Now.ToString("yyyy-MM-dd");
            }

            //this.configCode.DataContext = copyDao; configCode 에러
            this.DataContext = copyDao;
            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
            this.OKButton.Click += new RoutedEventHandler(OKButton_Click);
            this.CancelButton.Click += new RoutedEventHandler(CancelButton_Click);
        }

        void TAB3Button_Click(object sender, RoutedEventArgs e)
        {
            //jusoDialog = new JusoDialog();
            //jusoDialog.Title = "도로명 주소 / 지번 주소  - 공장2";
            //if (jusoDialog.ShowDialog() == true)
            //{
            //    SystemCodeVo resultVo = jusoDialog.ResultDao;

            //    this.text_N2ND_AREA_PST_NO.Text = resultVo.ZIPCODE;
            //    this.text_N2ND_AREA_ADDR.Text = resultVo.SIDO + " "
            //                                  + resultVo.GUNGU + " "
            //                                  + resultVo.DORO_NM + " "
            //                                  + resultVo.BLDG_NO + " "
            //                                  + resultVo.BLDG_NM
            //                                  + (string.IsNullOrEmpty(resultVo.HAENG_DONG_NM) ? "" : "(" + resultVo.HAENG_DONG_NM + ")") + "\r\n"
            //                                  + resultVo.Message;

            //    this.N2ND_AREA_OLD_ADDR = resultVo.SIDO + " "
            //                            + resultVo.GUNGU + " "
            //                            + resultVo.LAW_DONG_NM + " "
            //                            + resultVo.LAW_RI_NM + " "
            //                            + resultVo.JIBUN_NO + "\r\n"
            //                            + resultVo.Message;
            //}

        }

        void TAB2Button_Click(object sender, RoutedEventArgs e)
        {

         
            //jusoDialog = new JusoDialog();
            //jusoDialog.Title = "도로명 주소 / 지번 주소  - 공장1";
            //if (jusoDialog.ShowDialog() == true)
            //{
            //    SystemCodeVo resultVo = jusoDialog.ResultDao;

            //    this.text_N1ST_AREA_PST_NO.Text = resultVo.ZIPCODE;
            //    this.text_N1ST_AREA_ADDR.Text = resultVo.SIDO + " "
            //                                  + resultVo.GUNGU + " "
            //                                  + resultVo.DORO_NM + " "
            //                                  + resultVo.BLDG_NO + " "
            //                                  + resultVo.BLDG_NM
            //                                  + (string.IsNullOrEmpty(resultVo.HAENG_DONG_NM) ? "" : "(" + resultVo.HAENG_DONG_NM + ")") + "\r\n"
            //                                  + resultVo.Message;

            //    this.N1ST_AREA_OLD_ADDR = resultVo.SIDO + " "
            //                            + resultVo.GUNGU + " "
            //                            + resultVo.LAW_DONG_NM + " "
            //                            + resultVo.LAW_RI_NM + " "
            //                            + resultVo.JIBUN_NO + "\r\n"
            //                            + resultVo.Message;
            //}

        }

        void TAB1Button_Click(object sender, RoutedEventArgs e)
        {
            //jusoDialog = new JusoDialog();
            //jusoDialog.Title = "도로명 주소 / 지번 주소  - 본사";
            //if (jusoDialog.ShowDialog() == true)
            //{
            //    SystemCodeVo resultVo = jusoDialog.ResultDao;

            //    this.text_HDQTR_PST_NO.Text = resultVo.ZIPCODE;
            //    this.text_HDQTR_ADDR.Text =     resultVo.SIDO + " "
            //                                  + resultVo.GUNGU + " "
            //                                  + resultVo.DORO_NM + " "
            //                                  + resultVo.BLDG_NO + " "
            //                                  + resultVo.BLDG_NM
            //                                  + (string.IsNullOrEmpty(resultVo.HAENG_DONG_NM) ? "" : "(" + resultVo.HAENG_DONG_NM + ")") + "\r\n"
            //                                  + resultVo.Message;

            //    this.HDQTR_OLD_ADDR = resultVo.SIDO + " "
            //                            + resultVo.GUNGU + " "
            //                            + resultVo.LAW_DONG_NM + " "
            //                            + resultVo.LAW_RI_NM + " "
            //                            + resultVo.JIBUN_NO + "\r\n"
            //                            + resultVo.Message;
            //}
        }

        void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("http://www.juso.go.kr/openIndexPage.do");
            }
            catch (Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #region Functon (OKButton_Click, CancelButton_Click)
        private async void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValueCheckd())
            {
                int _Num = 0;
                //SystemCodeVo resultVo;
                if (isEdit == false)
                {
                    this.updateVo = getDomain();

                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s143/i", new StringContent(JsonConvert.SerializeObject(this.updateVo), System.Text.Encoding.UTF8, "application/json")))
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
                    this.updateVo = getDomain();

                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s143/u", new StringContent(JsonConvert.SerializeObject(this.updateVo), System.Text.Encoding.UTF8, "application/json")))
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





                //SystemCodeVo resultVo;
                //if (isEdit == false)
                //{
                //    ResultDao = getDomain();

                //    resultVo = customerClient.InsertCustomerDetailCode(ResultDao);
                //    if (!resultVo.isSuccess)
                //    {
                //        //실패
                //        WinUIMessageBox.Show(resultVo.Message, "[" + SystemProperties.PROGRAM_TITLE + "]거래처 관리", MessageBoxButton.OK, MessageBoxImage.Error);
                //        return;
                //    }
                //    //성공
                //    WinUIMessageBox.Show("완료 되었습니다", "[추가]거래처 관리", MessageBoxButton.OK, MessageBoxImage.Information);
                //}
                //else
                //{
                //    SystemCodeVo updateVo = getDomain();
                //    resultVo = customerClient.UpdateCustomerDetailCode(updateVo);
                //    if (!resultVo.isSuccess)
                //    {
                //        //실패
                //        WinUIMessageBox.Show(resultVo.Message, "[" + SystemProperties.PROGRAM_TITLE + "]거래처 관리", MessageBoxButton.OK, MessageBoxImage.Error);
                //        return;
                //    }

                //    ResultDao = updateVo;
                //    //성공
                //    WinUIMessageBox.Show("완료 되었습니다", "[수정]거래처 관리", MessageBoxButton.OK, MessageBoxImage.Information);

                //    this.orgDao.CO_NO = updateVo.CO_NO;
                //    this.orgDao.CO_NM = updateVo.CO_NM;
                //    this.orgDao.CO_RGST_NO = updateVo.CO_RGST_NO;
                //    this.orgDao.PRSD_NM = updateVo.PRSD_NM;
                //    this.orgDao.HDQTR_PHN_NO =  updateVo.HDQTR_PHN_NO;
                //    //거래처 유형
                //    this.orgDao.CO_TP_CD = updateVo.CO_TP_CD;
                //    this.orgDao.CO_TP_NM = updateVo.CO_TP_NM;
                //    //사업자 구분
                //    this.orgDao.CO_CLSS_CD = updateVo.CO_CLSS_CD;
                //    this.orgDao.CO_CLSS_NM = updateVo.CO_CLSS_NM;

                //    this.orgDao.HDQTR_PHN_NO =  updateVo.HDQTR_PHN_NO;
                //    //법인 등록 번호
                //    this.orgDao.CORP_CD = updateVo.CORP_CD;
                //    //영업 담당자 : (정)
                //    this.orgDao.N1ST_SLS_MAN = updateVo.N1ST_SLS_MAN;
                //    //영업 담당자 : (부)
                //    this.orgDao.N2ND_SLS_MAN = updateVo.N2ND_SLS_MAN;
                //    //지역 구분
                //    this.orgDao.AREA_CD = updateVo.AREA_CD;
                //    this.orgDao.AREA_NM = updateVo.AREA_NM;
                //    //업체 구분
                //    this.orgDao.PAY_TP_CD = updateVo.PAY_TP_CD;
                //    this.orgDao.PAY_TP_NM = updateVo.PAY_TP_NM;
                //    //거래 구분
                //    this.orgDao.TRD_TP_CD = updateVo.TRD_TP_CD;
                //    this.orgDao.TRD_TP_NM = updateVo.TRD_TP_NM;
                //    //팀 구분
                //    this.orgDao.TEAM_CD = updateVo.TEAM_CD;
                //    this.orgDao.TEAM_NM = updateVo.TEAM_NM;
                //    //에누리
                //    this.orgDao.DC_FLG = (updateVo.DC_FLG.Equals("Y") ? "사용" : "미사용");
                //     //은행
                //    this.orgDao.BANK_CD = updateVo.BANK_CD;
                //    this.orgDao.BANK_NM = updateVo.BANK_NM;
                //    //예금주
                //    this.orgDao.ACCT_HLD_NM = updateVo.ACCT_HLD_NM;
                //    this.orgDao.ACCT_NO = updateVo.ACCT_NO;
                //    //거래 구분
                //    this.orgDao.TRD_CATE_CD = updateVo.TRD_CATE_CD;
                //    this.orgDao.TRD_CATE_NM = updateVo.TRD_CATE_NM;
                //    //업태
                //    this.orgDao.N1ST_BIZ_COND_CD = updateVo.N1ST_BIZ_COND_CD;
                //    this.orgDao.N1ST_BIZ_COND_NM = updateVo.N1ST_BIZ_COND_NM;
                //    //종목
                //    this.orgDao.N1ST_BZTP_CD = updateVo.N1ST_BZTP_CD;
                //    this.orgDao.N1ST_BZTP_NM = updateVo.N1ST_BZTP_NM;
                //    //통화 코드
                //    this.orgDao.BSE_CURR_CD = updateVo.BSE_CURR_CD;
                //    this.orgDao.BSE_CURR_NM = updateVo.BSE_CURR_NM;
                //    //통화 코드
                //    this.orgDao.INTERRLT_CO_CD = updateVo.INTERRLT_CO_CD;
                //    this.orgDao.INTERRLT_CO_NM = updateVo.INTERRLT_CO_NM;
                //    //마감 구분
                //    this.orgDao.PUR_CLZ_CD = updateVo.PUR_CLZ_CD;
                //    this.orgDao.PUR_CLZ_NM = updateVo.PUR_CLZ_NM;
                //    //납기 한도 일자
                //    this.orgDao.DUE_DT_PRD_DY = updateVo.DUE_DT_PRD_DY;
                //    //비고
                //    this.orgDao.CO_RMK = updateVo.CO_RMK;
                //    //담당자1
                //    this.orgDao.CNTC_MAN_NM = updateVo.CNTC_MAN_NM;
                //    this.orgDao.CNTC_MAN_PSN_NM = updateVo.CNTC_MAN_PSN_NM;
                //    this.orgDao.CNTC_MAN_PHN_NO = updateVo.CNTC_MAN_PHN_NO;
                //    this.orgDao.CNTC_MAN_EML = updateVo.CNTC_MAN_EML;
                //    //담당자[계산서]
                //    this.orgDao.TAX_MAN_NM = updateVo.TAX_MAN_NM;
                //    this.orgDao.TAX_MAN_PSN_NM = updateVo.TAX_MAN_PSN_NM;
                //    this.orgDao.TAX_MAN_PHN_NO = updateVo.TAX_MAN_PHN_NO;
                //    this.orgDao.TAX_MAN_EML = updateVo.TAX_MAN_EML;
                //    //본사
                //    this.orgDao.HDQTR_AREA_NM = updateVo.HDQTR_AREA_NM;
                //    //Dao.HDQTR_PHN_NO = this.text_HDQTR_PHN_NO.Text;
                //    this.orgDao.HDQTR_FAX_NO = updateVo.HDQTR_FAX_NO;
                //    this.orgDao.HDQTR_ADDR = updateVo.HDQTR_ADDR;
                //    this.orgDao.HDQTR_OLD_ADDR = updateVo.HDQTR_OLD_ADDR;
                //    this.orgDao.HDQTR_PST_NO = updateVo.HDQTR_PST_NO;
                //    //공장1
                //    this.orgDao.N1ST_CNTC_MAN_NM = updateVo.N1ST_CNTC_MAN_NM;
                //    this.orgDao.N1ST_CNTC_MAN_PSN_NM = updateVo.N1ST_CNTC_MAN_PSN_NM;
                //    this.orgDao.N1ST_CNTC_MAN_PHN_NO = updateVo.N1ST_CNTC_MAN_PHN_NO;
                //    this.orgDao.N1ST_CNTC_MAN_EML = updateVo.N1ST_CNTC_MAN_EML;
                //    this.orgDao.N1ST_AREA_NM = updateVo.N1ST_AREA_NM;
                //    this.orgDao.N1ST_AREA_PHN_NO = updateVo.N1ST_AREA_PHN_NO;
                //    this.orgDao.N1ST_AREA_FAX_NO = updateVo.N1ST_AREA_FAX_NO;
                //    this.orgDao.N1ST_AREA_ADDR = updateVo.N1ST_AREA_ADDR;
                //    this.orgDao.N1ST_AREA_OLD_ADDR = updateVo.N1ST_AREA_OLD_ADDR;
                //    this.orgDao.N1ST_AREA_PST_NO = updateVo.N1ST_AREA_PST_NO;
                //    //공장2
                //    this.orgDao.N2ND_AREA_NM = updateVo.N2ND_AREA_NM;
                //    this.orgDao.N2ND_CNTC_MAN_NM = updateVo.N2ND_CNTC_MAN_NM;
                //    this.orgDao.N2ND_CNTC_MAN_PSN_NM = updateVo.N2ND_CNTC_MAN_PSN_NM;
                //    this.orgDao.N2ND_CNTC_MAN_PHN_NO = updateVo.N2ND_CNTC_MAN_PHN_NO;
                //    this.orgDao.N2ND_CNTC_MAN_EML = updateVo.N2ND_CNTC_MAN_EML;
                //    this.orgDao.N2ND_AREA_PHN_NO = updateVo.N2ND_AREA_PHN_NO;
                //    this.orgDao.N2ND_AREA_FAX_NO = updateVo.N2ND_AREA_FAX_NO;
                //    this.orgDao.N2ND_AREA_ADDR = updateVo.N2ND_AREA_ADDR;
                //    this.orgDao.N2ND_AREA_OLD_ADDR = updateVo.N2ND_AREA_OLD_ADDR;
                //    this.orgDao.N2ND_AREA_PST_NO = updateVo.N2ND_AREA_PST_NO;
                //    //
                //    this.orgDao.DELT_FLG = (updateVo.DELT_FLG.Equals("N") ? "사용" : "미사용");
                //    this.orgDao.CRE_USR_ID = updateVo.CRE_USR_ID;
                //    this.orgDao.UPD_USR_ID = updateVo.UPD_USR_ID;
                //    this.orgDao.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;

                //    this.orgDao.HMPG_ADDR = updateVo.HMPG_ADDR;

                //    this.orgDao.SL_AREA_CD = updateVo.SL_AREA_CD;
                //    this.orgDao.SL_AREA_NM = updateVo.SL_AREA_NM;

                //    this.orgDao.BILL_CD = updateVo.BILL_CD;
                //    this.orgDao.BILL_NM = updateVo.BILL_NM;

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
            if (string.IsNullOrEmpty(this.text_CO_NO.Text))
            {
               WinUIMessageBox.Show("[거래처 코드] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.text_CO_NO.IsTabStop = true;
                this.text_CO_NO.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(this.text_CO_NM.Text))
            {
                WinUIMessageBox.Show("[거래처 명] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.text_CO_NM.IsTabStop = true;
                this.text_CO_NM.Focus();
                return false;
            }
            //else if (string.IsNullOrEmpty(this.text_CO_RGST_NO.Text))
            //{
            //    WinUIMessageBox.Show("[사업자 등록 번호] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.text_CO_RGST_NO.IsTabStop = true;
            //    this.text_CO_RGST_NO.Focus();
            //    return false;
            //}
            //else if (string.IsNullOrEmpty(this.text_PRSD_NM.Text))
            //{
            //    WinUIMessageBox.Show("[대표자 명] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.text_PRSD_NM.IsTabStop = true;
            //    this.text_PRSD_NM.Focus();
            //    return false;
            //}
            //else if (string.IsNullOrEmpty(this.text_HDQTR_PHN_NO.Text))
            //{
            //    WinUIMessageBox.Show("[대표 전화] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.text_HDQTR_PHN_NO.IsTabStop = true;
            //    this.text_HDQTR_PHN_NO.Focus();
            //    return false;
            //}
            //else if (string.IsNullOrEmpty(this.text_JOIN_CO_DT.Text))
            //{
            //    WinUIMessageBox.Show("[입사일] 입력 값이 맞지 않습니다.", "[유효검사]사용자 권한 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.text_JOIN_CO_DT.IsTabStop = true;
            //    this.text_JOIN_CO_DT.Focus();
            //    return false;
            //}
            //else if (string.IsNullOrEmpty(this.combo_OFC_PSN_NM.Text))
            //{
            //    WinUIMessageBox.Show("[직책] 입력 값이 맞지 않습니다.", "[유효검사]사용자 권한 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.combo_OFC_PSN_NM.IsTabStop = true;
            //    this.combo_OFC_PSN_NM.Focus();
            //    return false;
            //}
            //else if (string.IsNullOrEmpty(this.text_USR_N1ST_NM.Text))
            //{
            //    WinUIMessageBox.Show("[이름] 입력 값이 맞지 않습니다.", "[유효검사]사용자 권한 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.text_USR_N1ST_NM.IsTabStop = true;
            //    this.text_USR_N1ST_NM.Focus();
            //    return false;
            //}
            //else if (string.IsNullOrEmpty(this.text_EMPE_NO.Text))
            //{
            //    WinUIMessageBox.Show("[사번] 입력 값이 맞지 않습니다.", "[유효검사]사용자 권한 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.text_EMPE_NO.IsTabStop = true;
            //    this.text_EMPE_NO.Focus();
            //    return false;
            //}
            //else if (ImageToByte(this.text_Image.Source).Length > 2197152)
            //{
            //    WinUIMessageBox.Show("이미지 파일 크기가 2Mbyte 초과 하였습니다.", "[유효검사]사용자 권한 관리", MessageBoxButton.OK, MessageBoxImage.Information);
            //    return false;
            //}
            //else if (string.IsNullOrEmpty(this.text_PHN_NO.Text))
            //{
            //    WinUIMessageBox.Show("[전화번호] 입력 값이 맞지 않습니다.", "[유효검사]사용자 권한 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.text_PHN_NO.IsTabStop = true;
            //    this.text_PHN_NO.Focus();
            //    return false;
            //}
            //else if (string.IsNullOrEmpty(this.text_CELL_PHN_NO.Text))
            //{
            //    WinUIMessageBox.Show("[핸드폰] 입력 값이 맞지 않습니다.", "[유효검사]사용자 권한 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.text_CELL_PHN_NO.IsTabStop = true;
            //    this.text_CELL_PHN_NO.Focus();
            //    return false;
            //}
            //else if (string.IsNullOrEmpty(this.text_EML_ID.Text))
            //{
            //    WinUIMessageBox.Show("[이메일] 입력 값이 맞지 않습니다.", "[유효검사]사용자 권한 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.text_EML_ID.IsTabStop = true;
            //    this.text_EML_ID.Focus();
            //    return false;
            //}
            //else if (string.IsNullOrEmpty(this.text_ADDR.Text))
            //{
            //    WinUIMessageBox.Show("[주소] 입력 값이 맞지 않습니다.", "[유효검사]시스템 권한 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.text_ADDR.IsTabStop = true;
            //    this.text_ADDR.Focus();
            //    return false;
            //}
            else
            {
                //if (this.isEdit == false)
                //{
                //    SystemCodeVo dao = new SystemCodeVo()
                //    {
                //        CO_NO = this.text_CO_NO.Text,
                //        CHNL_CD = SystemProperties.USER_VO.CHNL_CD
                //    };
                //    IList<SystemCodeVo> daoList = (IList<SystemCodeVo>)customerClient.SelectCodeList(dao);
                //    if (daoList.Count != 0)
                //    {
                //        WinUIMessageBox.Show("[코드 - 중복] 코드를 다시 입력 하십시오.", "[중복검사]거래처 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
                //        this.text_CO_NO.IsTabStop = true;
                //        this.text_CO_NO.Focus();
                //        return false;
                //    }
                //}
            }
            return true;
        }
        #endregion

        #region Functon (getDomain - SystemCodeVo)
        private SystemCodeVo getDomain()
        {
            SystemCodeVo Dao = new SystemCodeVo();

            SystemCodeVo SL_AREA_NMVo = this.combo_SL_AREA_NM.SelectedItem as SystemCodeVo;
            SystemCodeVo PAY_TP_NMVo = this.combo_PAY_TP_NM.SelectedItem as SystemCodeVo;
            SystemCodeVo N1ST_BIZ_COND_NMVo = this.combo_N1ST_BIZ_COND_NM.SelectedItem as SystemCodeVo;
            SystemCodeVo N1ST_BZTP_NMVo = this.combo_N1ST_BZTP_NM.SelectedItem as SystemCodeVo;

            SystemCodeVo TEAM_NMVo = this.combo_TEAM_NM.SelectedItem as SystemCodeVo;
            SystemCodeVo TRD_TP_NMVo = this.combo_TRD_TP_NM.SelectedItem as SystemCodeVo;
            SystemCodeVo TRD_CATE_NMVo = this.combo_TRD_CATE_NM.SelectedItem as SystemCodeVo;
            SystemCodeVo BANK_NMVo = this.combo_BANK_NM.SelectedItem as SystemCodeVo;

            Dao.CO_NO = this.text_CO_NO.Text;
            Dao.CO_NM = this.text_CO_NM.Text;
            Dao.CO_RGST_NO = this.text_CO_RGST_NO.Text;
            Dao.CORP_CD = this.text_CORP_CD.Text;
            Dao.PRSD_NM = this.text_PRSD_NM.Text;
            Dao.HDQTR_PHN_NO = this.text_HDQTR_PHN_NO.Text;
            Dao.HMPG_ADDR = this.text_HMPG_ADDR.Text;
            //법인 등록 번호
            Dao.CORP_CD = this.text_CORP_CD.Text;
            //영업 담당자 : (정)
            Dao.N1ST_SLS_MAN_CD = this.combo_N1ST_SLS_MAN.Text;
            //영업 담당자 : (부)
            Dao.N2ND_SLS_MAN_CD = this.combo_N2ND_SLS_MAN.Text;

            if (SL_AREA_NMVo != null)
            {
                //거래처 유형
                Dao.SL_AREA_CD = SL_AREA_NMVo.CLSS_CD;
                Dao.SL_AREA_NM = SL_AREA_NMVo.CLSS_DESC;
            }
            if (PAY_TP_NMVo != null)
            {
                //사업자 구분
                Dao.PAY_TP_CD = PAY_TP_NMVo.CLSS_CD;
                Dao.PAY_TP_NM = PAY_TP_NMVo.CLSS_DESC;
            }
            if (N1ST_BIZ_COND_NMVo != null)
            {
                //업태
                Dao.N1ST_BIZ_COND_CD = N1ST_BIZ_COND_NMVo.CLSS_CD;
                Dao.N1ST_BIZ_COND_NM = N1ST_BIZ_COND_NMVo.CLSS_DESC;
            }
            if (N1ST_BZTP_NMVo != null)
            {
                //종목
                Dao.N1ST_BZTP_CD = N1ST_BZTP_NMVo.CLSS_CD;
                Dao.N1ST_BZTP_NM = N1ST_BZTP_NMVo.CLSS_DESC;
            }
            if (TEAM_NMVo != null)
            {
                //통화 코드
                Dao.TEAM_CD = TEAM_NMVo.CLSS_CD;
                Dao.TEAM_NM = TEAM_NMVo.CLSS_DESC;
            }
            if (TRD_TP_NMVo != null)
            {
                //통화 코드
                Dao.TRD_TP_CD = TRD_TP_NMVo.CLSS_CD;
                Dao.TRD_TP_NM = TRD_TP_NMVo.CLSS_DESC;
            }
            if (TRD_CATE_NMVo != null)
            {
                //마감 구분
                Dao.TRD_CATE_CD = TRD_CATE_NMVo.CLSS_CD;
                Dao.TRD_CATE_NM = TRD_CATE_NMVo.CLSS_DESC;
            }
            if (BANK_NMVo != null)
            {
                //마감 구분
                Dao.BANK_CD = BANK_NMVo.CLSS_CD;
                Dao.BANK_NM = BANK_NMVo.CLSS_DESC;
            }

            //납기 한도 일자
            //Dao.DUE_DT_PRD_DY = this.text_DUE_DT_PRD_DY.Text;
            //비고
            Dao.CO_RMK = this.text_CO_RMK.Text;

            Dao.ACCT_HLD_NM = this.text_ACCT_HLD_NM.Text;
            Dao.ACCT_NO = this.text_ACCT_NO.Text;

            //담당자1
            Dao.CNTC_MAN_NM = this.text_CNTC_MAN_NM.Text;
            Dao.CNTC_MAN_PSN_NM = this.text_CNTC_MAN_PSN_NM.Text;
            Dao.CNTC_MAN_PHN_NO = this.text_CNTC_MAN_PHN_NO.Text;
            Dao.CNTC_MAN_EML = this.text_CNTC_MAN_EML.Text;
            ////담당자2
            //Dao.N2ND_CNTC_MAN_NM = this.text_N2ND_CNTC_MAN_NM.Text;
            //Dao.N2ND_CNTC_MAN_PSN_NM = this.text_N2ND_CNTC_MAN_NM.Text;
            //Dao.N2ND_CNTC_MAN_PHN_NO = this.text_N2ND_CNTC_MAN_PHN_NO.Text;
            //Dao.N2ND_CNTC_MAN_EML = this.text_N2ND_CNTC_MAN_EML.Text;
            //담당자[계산서]
            Dao.TAX_MAN_NM = this.text_TAX_MAN_NM.Text;
            Dao.TAX_MAN_PSN_NM = this.text_TAX_MAN_PSN_NM.Text;
            Dao.TAX_MAN_PHN_NO = this.text_TAX_MAN_PHN_NO.Text;
            Dao.TAX_MAN_EML = this.text_TAX_MAN_EML.Text;

            //본사
            Dao.HDQTR_AREA_NM = this.text_HDQTR_AREA_NM.Text;
            //Dao.HDQTR_PHN_NO = this.text_HDQTR_PHN_NO.Text;
            Dao.HDQTR_FAX_NO = this.text_HDQTR_FAX_NO.Text;
            Dao.HDQTR_ADDR = this.text_HDQTR_ADDR.Text;
            Dao.HDQTR_PST_NO = this.text_HDQTR_PST_NO.Text;
            Dao.HDQTR_OLD_ADDR = this.HDQTR_OLD_ADDR;

            //공장1
            Dao.N1ST_AREA_NM = this.text_N1ST_AREA_NM.Text;
            Dao.N1ST_AREA_PHN_NO = this.text_N1ST_AREA_PHN_NO.Text;
            Dao.N1ST_CNTC_MAN_NM = this.text_N1ST_CNTC_MAN_NM.Text;
            Dao.N1ST_CNTC_MAN_PSN_NM = this.text_N1ST_CNTC_MAN_PSN_NM.Text;
            Dao.N1ST_CNTC_MAN_EML = this.text_N1ST_CNTC_MAN_EML.Text;
            Dao.N1ST_CNTC_MAN_PHN_NO = this.text_N1ST_CNTC_MAN_PHN_NO.Text;
            Dao.N1ST_AREA_FAX_NO = this.text_N1ST_AREA_FAX_NO.Text;
            Dao.N1ST_AREA_ADDR = this.text_N1ST_AREA_ADDR.Text;
            Dao.N1ST_AREA_PST_NO = this.text_N1ST_AREA_PST_NO.Text;
            Dao.N1ST_AREA_OLD_ADDR = this.N1ST_AREA_OLD_ADDR;
            
            //공장2
            Dao.N2ND_AREA_NM = this.text_N2ND_AREA_NM.Text;
            Dao.N2ND_AREA_PHN_NO = this.text_N2ND_AREA_PHN_NO.Text;
            Dao.N2ND_CNTC_MAN_NM = this.text_N2ND_CNTC_MAN_NM.Text;
            Dao.N2ND_CNTC_MAN_PSN_NM = this.text_N2ND_CNTC_MAN_PSN_NM.Text;
            Dao.N2ND_CNTC_MAN_EML = this.text_N2ND_CNTC_MAN_EML.Text;
            Dao.N2ND_CNTC_MAN_PHN_NO = this.text_N2ND_CNTC_MAN_PHN_NO.Text;
            Dao.N2ND_AREA_FAX_NO = this.text_N2ND_AREA_FAX_NO.Text;
            Dao.N2ND_AREA_ADDR = this.text_N2ND_AREA_ADDR.Text;
            Dao.N2ND_AREA_PST_NO = this.text_N2ND_AREA_PST_NO.Text;
            Dao.N2ND_AREA_OLD_ADDR = this.N2ND_AREA_OLD_ADDR;
            //Dao.N2ND_AREA_NM = this.text_N2ND_AREA_NM.Text;
            //Dao.N2ND_AREA_PHN_NO = this.text_N2ND_PHN_NO.Text;
            //Dao.N2ND_AREA_FAX_NO = this.text_N2ND_FAX_NO.Text;
            //Dao.N2ND_AREA_ADDR = this.text_N2ND_ADDR.Text;


            SystemCodeVo AREA_NMVo = this.combo_AREA_NM.SelectedItem as SystemCodeVo;
            if (AREA_NMVo != null)
            {
                //거래처 유형
                Dao.AREA_CD = AREA_NMVo.CLSS_CD;
                Dao.AREA_NM = AREA_NMVo.CLSS_DESC;
            }

            SystemCodeVo BILL_NMVo = this.combo_BILL_NM.SelectedItem as SystemCodeVo;
            if (BILL_NMVo != null)
            {
                Dao.BILL_CD = BILL_NMVo.CLSS_CD;
                Dao.BILL_NM = BILL_NMVo.CLSS_DESC;
            }

            Dao.DELT_FLG = (this.combo_deltFlg.Text.Equals("사용") ? "N" : "Y");
            Dao.DC_FLG = (this.combo_DC_FLG.Text.Equals("사용") ? "Y" : "N");
            Dao.CRE_USR_ID = SystemProperties.USER;
            Dao.UPD_USR_ID = SystemProperties.USER;
            Dao.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
            return Dao;
        }
        #endregion



        public async void SYSTEM_CODE_VO()
        {
            //this.combo_AREA_NM.ItemsSource = SystemProperties.SYSTEM_CODE_VO("L-002");
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "L-002"))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.combo_AREA_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                }
            }

            //this.combo_SL_AREA_NM.ItemsSource = SystemProperties.SYSTEM_CODE_VO("L-009");
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "L-009"))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.combo_SL_AREA_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                }
            }

            //this.combo_BILL_NM.ItemsSource = SystemProperties.SYSTEM_CODE_VO("S-031");
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "S-031"))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.combo_BILL_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                }
            }

            //this.combo_PAY_TP_NM.ItemsSource = SystemProperties.SYSTEM_CODE_VO("L-011");
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "L-011"))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.combo_PAY_TP_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                }
            }

            //this.combo_N1ST_BIZ_COND_NM.ItemsSource = SystemProperties.SYSTEM_CODE_VO("S-002");
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "S-002"))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.combo_N1ST_BIZ_COND_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                }
            }

            //this.combo_N1ST_BZTP_NM.ItemsSource = SystemProperties.SYSTEM_CODE_VO("S-003");
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "S-003"))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.combo_N1ST_BZTP_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                }
            }

            //this.combo_TEAM_NM.ItemsSource = SystemProperties.SYSTEM_CODE_VO("L-010");
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "L-010"))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.combo_TEAM_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                }
            }

            //this.combo_TRD_TP_NM.ItemsSource = SystemProperties.SYSTEM_CODE_VO("L-012");
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "L-012"))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.combo_TRD_TP_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                }
            }

            //this.combo_TRD_CATE_NM.ItemsSource = SystemProperties.SYSTEM_CODE_VO("L-013");
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "L-013"))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.combo_TRD_CATE_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                }
            }

            //this.combo_BANK_NM.ItemsSource = SystemProperties.SYSTEM_CODE_VO("L-014");
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "L-014"))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.combo_BANK_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                }
            }
        }

        public async void USER_CODE_VO()
        {
            //this.combo_N1ST_SLS_MAN.ItemsSource = SystemProperties.USER_CODE_VO();
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s136/usr", new StringContent(JsonConvert.SerializeObject(new GroupUserVo() { DELT_FLG = "N", CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.combo_N1ST_SLS_MAN.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<GroupUserVo>>(await response.Content.ReadAsStringAsync()).Cast<GroupUserVo>().ToList();
                }
            }

            //this.combo_N2ND_SLS_MAN.ItemsSource = SystemProperties.USER_CODE_VO();
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s136/usr", new StringContent(JsonConvert.SerializeObject(new GroupUserVo() { DELT_FLG = "N", CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.combo_N2ND_SLS_MAN.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<GroupUserVo>>(await response.Content.ReadAsStringAsync()).Cast<GroupUserVo>().ToList();
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

        public SystemCodeVo resultDao
        {
            get
            {
                return this.updateVo;
            }
        }

        //public SystemCodeVo ResultDao
        //{
        //    get;
        //    set;
        //}
    }
}
