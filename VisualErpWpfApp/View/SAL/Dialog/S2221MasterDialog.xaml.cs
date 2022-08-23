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
using System.Windows.Media;
using AquilaErpWpfApp3.Util;

namespace AquilaErpWpfApp3.View.SAL.Dialog
{
    public partial class S2221MasterDialog : DXWindow
    {
        //private static SaleOrderServiceClient saleOrderClient = SystemProperties.SaleOrderClient;
        private SaleVo orgDao;
        private bool isEdit = false;
        private SaleVo updateDao;

        private string title = "수금관리";

        public S2221MasterDialog(SaleVo Dao)
        {
            InitializeComponent();

            SYSTEM_CODE_VO();

            this.combo_BIL_RMK_NM.SelectedIndexChanged += combo_BIL_RMK_NM_SelectedIndexChanged;

            this.orgDao = Dao;

            SaleVo copyDao = new SaleVo()
            {
                CLT_BIL_NO = Dao.CLT_BIL_NO,
                CLT_BIL_DT = Dao.CLT_BIL_DT,
                AREA_CD = Dao.AREA_CD,
                AREA_NM = Dao.AREA_NM,
                CLT_CO_CD = Dao.CLT_CO_CD,
                CLT_CO_NM = Dao.CLT_CO_NM,
                CLT_USR_NM = Dao.CLT_USR_NM,
                CLT_CD = Dao.CLT_CD,
                CLT_NM = Dao.CLT_NM,
                CLT_RMN_AMT = Dao.CLT_RMN_AMT,
                CLT_AMT = Dao.CLT_AMT,
                CLT_DC_RT = Dao.CLT_DC_RT,
                CLT_DC_AMT = Dao.CLT_DC_AMT,
                CLT_BIL_CD = Dao.CLT_BIL_CD,
                CLT_BIL_NM = Dao.CLT_BIL_NM,
                BANK_ACCT_CD = Dao.BANK_ACCT_CD,
                BANK_ACCT_NM = Dao.BANK_ACCT_NM,
                BIL_TP_CD = Dao.BIL_TP_CD,
                BIL_TP_NM = Dao.BIL_TP_NM,
                BIL_ACCT_NO = Dao.BIL_ACCT_NO,
                BIL_ST_DT = Dao.BIL_ST_DT,
                BIL_END_DT = Dao.BIL_END_DT,
                BIL_USR_NM = Dao.BIL_USR_NM,
                BIL_BANK_NM = Dao.BIL_BANK_NM,
                BIL_IN_NO = Dao.BIL_IN_NO,
                BIL_RMK_CD = Dao.BIL_RMK_CD,
                BIL_RMK_NM = Dao.BIL_RMK_NM,
                BIL_RMK = Dao.BIL_RMK,

                CRE_USR_ID = Dao.CRE_USR_ID,
                UPD_USR_ID = Dao.UPD_USR_ID,
            };

            //수정
            if (copyDao.CLT_BIL_NO != null)
            {
                this.text_CLT_BIL_NO.IsReadOnly = true;
                this.isEdit = true;
                CLT_BIL_NO = this.orgDao.CLT_BIL_NO;
            }
            else
            {
                //추가
                this.text_CLT_BIL_NO.IsReadOnly = true;
                this.isEdit = false;
                //copyDao.CLT_BIL_DT = System.DateTime.Now.ToString("yyyy-MM-dd");

                copyDao.CLT_BIL_DT = Dao.CLT_BIL_DT;

                copyDao.BIL_ST_DT = System.DateTime.Now.ToString("yyyy-MM-dd");
                copyDao.BIL_END_DT = System.DateTime.Now.AddYears(1).ToString("yyyy-MM-dd");

                copyDao.CLT_AMT = 0;
                copyDao.CLT_DC_RT = 0;
                copyDao.CLT_DC_AMT = 0;
                copyDao.CLT_RMN_AMT = 0;

                copyDao.CLT_NM = "대금";
                copyDao.CLT_BIL_NM = "통장";
            }
            this.configCode.DataContext = copyDao;
            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
            this.OKButton.Click += new RoutedEventHandler(OKButton_Click);
            this.CancelButton.Click += new RoutedEventHandler(CancelButton_Click);

            this.combo_CLT_BIL_NM.SelectedIndexChanged += combo_CLT_BIL_NM_SelectedIndexChanged;
            this.combo_CLT_CO_NM.SelectedIndexChanged += combo_CLT_CO_NM_SelectedIndexChanged;


            //this.text_CLT_AMT.KeyDown += text_CLT_DC_AMT_KeyDown;
            //this.text_CLT_DC_RT.KeyDown += text_CLT_DC_AMT_KeyDown;

            this.combo_AREA_NM.SelectedIndexChanged += combo_AREA_NM_SelectedIndexChanged;


            this.text_CLT_RMN_AMT.KeyUp += text_CLT_RMN_AMT_KeyDown;
            this.text_CLT_AMT.KeyUp += text_CLT_AMT_KeyDown;
            this.text_CLT_DC_RT.KeyUp += text_CLT_DC_RT_KeyDown;
            this.text_CLT_DC_AMT.KeyUp += text_CLT_DC_AMT_KeyDown;


            this.text_CLT_BIL_DT.Focus();
        }

        void combo_BIL_RMK_NM_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.combo_BIL_RMK_NM.Text.Equals("차손금"))
                {

                    this.text_BIL_RMK.Text = this.combo_BIL_RMK_NM.Text + " " +  this.text_CLT_DC_RT.Text + "%";
                }
                else
                {
                    this.text_BIL_RMK.Text = string.Empty;
                }
            }
            catch (Exception)
            {
                return;
            }
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
                        this.combo_CLT_CO_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                    }
                }
            }
        }


        //계산
        void text_CLT_DC_AMT_KeyDown(object sender, KeyEventArgs e)
        {
            ResultAmt();
        }

        void text_CLT_DC_RT_KeyDown(object sender, KeyEventArgs e)
        {
            ResultAmt();
        }

        void text_CLT_AMT_KeyDown(object sender, KeyEventArgs e)
        {
            ResultAmt();
        }

        void text_CLT_RMN_AMT_KeyDown(object sender, KeyEventArgs e)
        {
            this.text_CLT_AMT.Text = this.text_CLT_RMN_AMT.Text;

            ResultAmt();
        }


        void ResultAmt()
        {
            try
            {
                //대상 금액
                int tmpCltRmnAmt = int.Parse(this.text_CLT_RMN_AMT.Text);
                //수금액
                int tmpCltAmt = int.Parse(this.text_CLT_AMT.Text);
                //할인액
                this.text_CLT_DC_AMT.Text = (tmpCltAmt - tmpCltRmnAmt).ToString("#,###,###,###,###,##0");
                //할인율
                double a = Math.Abs(tmpCltAmt - tmpCltRmnAmt);

                double c = 0;
                double b = 0;
                if (tmpCltRmnAmt == 0)
                {
                    c = 0;
                }
                else
                {
                    b = Math.Round((a / tmpCltRmnAmt), 3);
                    c = b * 100;
                }

                //
                this.text_CLT_DC_RT.Text = "" + c.ToString("#,###,###,###,###,##0.00");


                //double cltDcRt  = (((Math.Abs(tmpCltAmt - tmpCltRmnAmt) / tmpCltRmnAmt) * 100));
                ////할인율
                ////double tmpCltDcRt = double.Parse(this.text_CLT_DC_RT.Text);
                ////this.label_CLT_DC_RT.Text = string.Format(this.text_CLT_DC_RT.Text, "#,###,###,###,###,##0.##");
                ////할인액
                ////int tmpCltDcAmt = int.Parse(this.text_CLT_DC_AMT.Text);
                ////this.label_CLT_DC_AMT.Text = string.Format(this.text_CLT_DC_AMT.Text, "#,###,###,###,###,##0");

                ////
                ////
                ////if (tmpCltDcRt > 0 && tmpCltRmnAmt > 0)
                ////{
                //    //대상 금액 계산
                //    this.label_CLT_RMN_AMT.Text = Math.Round(tmpCltDcAmt / tmpCltDcRt).ToString("#,###,###,###,###,##0");
                //    //할인율 계산   + 0.005
                //    this.label_CLT_DC_RT.Text = "" + ((float.Parse(tmpCltDcAmt.ToString()) / float.Parse(tmpCltRmnAmt.ToString()))).ToString("#,###,###,###,###,##0.##");
                //    //할인액 계산
                //    this.label_CLT_DC_AMT.Text = Math.Round(tmpCltRmnAmt * tmpCltDcRt).ToString("#,###,###,###,###,##0");
                ////}
            }
            catch (Exception)
            {
                //WinUIMessageBox.Show(eLog.Message, "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        //void text_CLT_DC_AMT_KeyDown(object sender, KeyEventArgs e)
        //{
        //    try
        //    {
        //        this.text_CLT_DC_AMT.Text = "" + (int.Parse(this.text_CLT_AMT.Text) * (float.Parse(this.text_CLT_DC_RT.Text) / 100));
        //    }
        //    catch (Exception)
        //    {
        //        this.text_CLT_DC_AMT.Text = "0";
        //    }
        //}

        async void combo_CLT_CO_NM_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            SystemCodeVo coNmVo = this.combo_CLT_CO_NM.SelectedItem as SystemCodeVo;
            if (coNmVo != null)
            {
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s2221/mst/amt", new StringContent(JsonConvert.SerializeObject(new SaleVo() { CLT_CO_CD = coNmVo.CO_NO, CLT_CO_NM = coNmVo.CO_NM, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.text_CLT_RMN_AMT.Text = JsonConvert.DeserializeObject<string>(await response.Content.ReadAsStringAsync());
                    }
                }
                //JobVo resultVo = saleOrderClient.S2221SelectCltRmnAmt(new JobVo() { CLT_CO_CD = coNmVo.CO_NO, CLT_CO_NM = coNmVo.CO_NM });
                //if (!resultVo.isSuccess)
                //{
                //    //실패
                //    WinUIMessageBox.Show(resultVo.Message, "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
                //    return;
                //}
                //this.text_CLT_RMN_AMT.Text = resultVo.LST_FMT_NO;
            }
        }

        void combo_CLT_BIL_NM_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            SystemCodeVo cltBilNmVo = this.combo_CLT_BIL_NM.SelectedItem as SystemCodeVo;
            if (cltBilNmVo != null)
            {
                if (cltBilNmVo.CLSS_DESC.Equals("통장") || (cltBilNmVo.CLSS_DESC.Equals("카드")))
                {
                    this.label_BANK_ACCT_NM.Foreground = Brushes.Black;
                    this.combo_BANK_ACCT_NM.IsEnabled = true;

                    //
                    this.label_BIL_TP_NM.Foreground = Brushes.DarkGray;
                    this.label_BIL_ACCT_NO.Foreground = Brushes.DarkGray;
                    this.label_BIL_ST_DT.Foreground = Brushes.DarkGray;
                    this.label_BIL_END_DT.Foreground = Brushes.DarkGray;
                    this.label_BIL_USR_NM.Foreground = Brushes.DarkGray;
                    this.label_BIL_BANK_NM.Foreground = Brushes.DarkGray;
                    //this.label_BIL_IN_NO.Foreground = Brushes.DarkGray;
                    //
                    this.combo_BIL_TP_NM.IsEnabled = false;
                    this.text_BIL_ACCT_NO.IsEnabled = false;
                    this.text_BIL_ST_DT.IsEnabled = false;
                    this.text_BIL_END_DT.IsEnabled = false;
                    this.text_BIL_USR_NM.IsEnabled = false;
                    this.text_BIL_BANK_NM.IsEnabled = false;
                    //this.text_BIL_IN_NO.IsEnabled = false; 
                }
                else if (cltBilNmVo.CLSS_DESC.Equals("어음(자수)") || (cltBilNmVo.CLSS_DESC.Equals("어음(타수)")))
                {
                    this.label_BIL_TP_NM.Foreground = Brushes.Black;
                    this.label_BIL_ACCT_NO.Foreground = Brushes.Black;
                    this.label_BIL_ST_DT.Foreground = Brushes.Black;
                    this.label_BIL_END_DT.Foreground = Brushes.Black;
                    this.label_BIL_USR_NM.Foreground = Brushes.Black;
                    this.label_BIL_BANK_NM.Foreground = Brushes.Black;
                    //this.label_BIL_IN_NO.Foreground = Brushes.Black;
                    //
                    this.combo_BIL_TP_NM.IsEnabled = true;
                    this.text_BIL_ACCT_NO.IsEnabled = true;
                    this.text_BIL_ST_DT.IsEnabled = true;
                    this.text_BIL_END_DT.IsEnabled = true;
                    this.text_BIL_USR_NM.IsEnabled = true;
                    this.text_BIL_BANK_NM.IsEnabled = true;
                   // this.text_BIL_IN_NO.IsEnabled = true; 

                    //
                    this.label_BANK_ACCT_NM.Foreground = Brushes.DarkGray;
                    this.combo_BANK_ACCT_NM.IsEnabled = false;
                }
                else
                {
                    this.label_BIL_TP_NM.Foreground = Brushes.DarkGray;
                    this.label_BIL_ACCT_NO.Foreground = Brushes.DarkGray;
                    this.label_BIL_ST_DT.Foreground = Brushes.DarkGray;
                    this.label_BIL_END_DT.Foreground = Brushes.DarkGray;
                    this.label_BIL_USR_NM.Foreground = Brushes.DarkGray;
                    this.label_BIL_BANK_NM.Foreground = Brushes.DarkGray;
                    //this.label_BIL_IN_NO.Foreground = Brushes.DarkGray;
                    //
                    this.combo_BIL_TP_NM.IsEnabled = false;
                    this.text_BIL_ACCT_NO.IsEnabled = false;
                    this.text_BIL_ST_DT.IsEnabled = false;
                    this.text_BIL_END_DT.IsEnabled = false;
                    this.text_BIL_USR_NM.IsEnabled = false;
                    this.text_BIL_BANK_NM.IsEnabled = false;
                    //this.text_BIL_IN_NO.IsEnabled = false;

                    //
                    this.label_BANK_ACCT_NM.Foreground = Brushes.DarkGray;
                    this.combo_BANK_ACCT_NM.IsEnabled = false;
                }
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
                    this.updateDao = getDomain();

                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s2221/mst/i", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
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

                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s2221/mst/u", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
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


                //SaleVo resultVo;
                ////PurVo updateDao;
                //if (isEdit == false)
                //{
                //    updateDao = getDomain();//this.updateDao

                //    // 자동 번호 할당
                //    resultVo = saleOrderClient.S2221SelectCltBilNo(updateDao);
                //    if (!resultVo.isSuccess)
                //    {
                //        //실패
                //        WinUIMessageBox.Show(resultVo.Message, "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
                //        return;
                //    }
                //    CLT_BIL_NO = resultVo.LST_FMT_NO;
                //    updateDao.CLT_BIL_NO = resultVo.LST_FMT_NO;
                //    this.text_CLT_BIL_NO.Text = resultVo.LST_FMT_NO;
                //    //MessageBoxResult result = WinUIMessageBox.Show("[" + updateDao.INSRL_NO + "] 저장 하시겠습니까?", "[전표 번호]창고간 이동", MessageBoxButton.YesNo, MessageBoxImage.Question);
                //    //if (result == MessageBoxResult.Yes)
                //    //{
                //    resultVo = saleOrderClient.S2221InsertMst(updateDao);
                //    if (!resultVo.isSuccess)
                //    {
                //        //실패
                //        WinUIMessageBox.Show(resultVo.Message, "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
                //        return;
                //    }
                //    //성공
                //    WinUIMessageBox.Show("[수금 번호 : " + updateDao.CLT_BIL_NO + "] 완료 되었습니다", "[추가]" + title, MessageBoxButton.OK, MessageBoxImage.Information);
                //    //}
                //}
                //else
                //{
                //    updateDao = getDomain();
                //    resultVo = saleOrderClient.S2221UpdateMst(updateDao);
                //    if (!resultVo.isSuccess)
                //    {
                //        //실패
                //        WinUIMessageBox.Show(resultVo.Message, "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
                //        return;
                //    }
                //    //성공
                //    WinUIMessageBox.Show("완료 되었습니다", "[수정]" + title, MessageBoxButton.OK, MessageBoxImage.Information);



                //    this.orgDao.CLT_BIL_NO = this.updateDao.CLT_BIL_NO;
                //    this.orgDao.CLT_BIL_DT = Convert.ToDateTime(this.updateDao.CLT_BIL_DT).ToString("yyyy-MM-dd");
                //    this.orgDao.AREA_CD = this.updateDao.AREA_CD;
                //    this.orgDao.AREA_NM = this.updateDao.AREA_NM;
                //    this.orgDao.CLT_CO_CD = this.updateDao.CLT_CO_CD;
                //    this.orgDao.CLT_CO_NM = this.updateDao.CLT_CO_NM;
                //    this.orgDao.CLT_USR_NM = this.updateDao.CLT_USR_NM;
                //    this.orgDao.CLT_CD = this.updateDao.CLT_CD;
                //    this.orgDao.CLT_NM = this.updateDao.CLT_NM;
                //    this.orgDao.CLT_RMN_AMT = this.updateDao.CLT_RMN_AMT;
                //    this.orgDao.CLT_AMT = this.updateDao.CLT_AMT;
                //    this.orgDao.CLT_DC_RT = this.updateDao.CLT_DC_RT;
                //    this.orgDao.CLT_DC_AMT = this.updateDao.CLT_DC_AMT;
                //    this.orgDao.CLT_BIL_CD = this.updateDao.CLT_BIL_CD;
                //    this.orgDao.CLT_BIL_NM = this.updateDao.CLT_BIL_NM;
                //    this.orgDao.BANK_ACCT_CD = this.updateDao.BANK_ACCT_CD;
                //    this.orgDao.BANK_ACCT_NM = this.updateDao.BANK_ACCT_NM;
                //    this.orgDao.BIL_TP_CD = this.updateDao.BIL_TP_CD;
                //    this.orgDao.BIL_TP_NM = this.updateDao.BIL_TP_NM;
                //    this.orgDao.BIL_ACCT_NO = this.updateDao.BIL_ACCT_NO;
                //    if (!string.IsNullOrEmpty(this.updateDao.BIL_ST_DT))
                //    {
                //        this.orgDao.BIL_ST_DT = Convert.ToDateTime(this.updateDao.BIL_ST_DT).ToString("yyyy-MM-dd");
                //        this.orgDao.BIL_END_DT = Convert.ToDateTime(this.updateDao.BIL_END_DT).ToString("yyyy-MM-dd");
                //    }
                //    this.orgDao.BIL_USR_NM = this.updateDao.BIL_USR_NM;
                //    this.orgDao.BIL_BANK_NM = this.updateDao.BIL_BANK_NM;
                //    this.orgDao.BIL_IN_NO = this.updateDao.BIL_IN_NO;
                //    this.orgDao.BIL_RMK_CD = this.updateDao.BIL_RMK_CD;
                //    this.orgDao.BIL_RMK_NM = this.updateDao.BIL_RMK_NM;
                //    this.orgDao.BIL_RMK = this.updateDao.BIL_RMK;
                //    this.orgDao.CRE_USR_ID = this.updateDao.CRE_USR_ID;
                //    this.orgDao.UPD_USR_ID = this.updateDao.UPD_USR_ID;
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
            if (string.IsNullOrEmpty(this.text_CLT_BIL_DT.Text))
            {
                WinUIMessageBox.Show("[수금 일자] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.text_CLT_BIL_DT.IsTabStop = true;
                this.text_CLT_BIL_DT.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(this.combo_AREA_NM.Text))
            {
                WinUIMessageBox.Show("[사업장] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.combo_AREA_NM.IsTabStop = true;
                this.combo_AREA_NM.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(this.combo_CLT_CO_NM.Text))
            {
                WinUIMessageBox.Show("[거래처] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.combo_CLT_CO_NM.IsTabStop = true;
                this.combo_CLT_CO_NM.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(this.combo_CLT_BIL_NM.Text))
            {
                WinUIMessageBox.Show("[입금 방법] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.combo_CLT_BIL_NM.IsTabStop = true;
                this.combo_CLT_BIL_NM.Focus();
                return false;
            }
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

            Dao.CLT_BIL_NO = this.text_CLT_BIL_NO.Text;

            Dao.CLT_BIL_DT = Convert.ToDateTime(this.text_CLT_BIL_DT.Text).ToString("yyyy-MM-dd");

            SystemCodeVo coNmVo = this.combo_CLT_CO_NM.SelectedItem as SystemCodeVo;
            if (coNmVo != null)
            {
                Dao.CLT_CO_CD = coNmVo.CO_NO;
                Dao.CLT_CO_NM = coNmVo.CO_NM;
            }

            SystemCodeVo areaNmVo = this.combo_AREA_NM.SelectedItem as SystemCodeVo;
            if (areaNmVo != null)
            {
                Dao.AREA_CD = areaNmVo.CLSS_CD;
                Dao.AREA_NM = areaNmVo.CLSS_DESC;
            }

            Dao.CLT_USR_NM = this.combo_CLT_USR_NM.Text;

            SystemCodeVo cltNmVo = this.combo_CLT_NM.SelectedItem as SystemCodeVo;
            if (cltNmVo != null)
            {
                Dao.CLT_CD = cltNmVo.CLSS_CD;
                Dao.CLT_NM = cltNmVo.CLSS_DESC;
            }

            Dao.CLT_RMN_AMT = this.text_CLT_RMN_AMT.Text;
            Dao.CLT_AMT = this.text_CLT_AMT.Text;

            Dao.CLT_DC_RT = (this.text_CLT_DC_RT.Text);
            Dao.CLT_DC_AMT = (this.text_CLT_DC_AMT.Text);


            SystemCodeVo cltBilNmVo = this.combo_CLT_BIL_NM.SelectedItem as SystemCodeVo;
            if (cltBilNmVo != null)
            {
                Dao.CLT_BIL_CD = cltBilNmVo.CLSS_CD;
                Dao.CLT_BIL_NM = cltBilNmVo.CLSS_DESC;
            }


            if (cltBilNmVo.CLSS_DESC.Equals("통장") || (cltBilNmVo.CLSS_DESC.Equals("카드")))
            {
                SystemCodeVo bankVo = this.combo_BANK_ACCT_NM.SelectedItem as SystemCodeVo;
                if (bankVo != null)
                {
                    //Dao.BANK_ACCT_CD = bankVo.BANK_ACCT_CD;
                    //Dao.BANK_ACCT_NM = bankVo.BANK_ACCT_NM;

                    //
                    Dao.BIL_TP_NM = "";
                    Dao.BIL_ACCT_NO = "";
                    Dao.BIL_ST_DT = "";
                    Dao.BIL_END_DT = "";
                    Dao.BIL_USR_NM = "";
                    Dao.BIL_BANK_NM = "";
                    //Dao.BIL_IN_NO = "";

                }
            }
            else if (cltBilNmVo.CLSS_DESC.Equals("어음(자수)") || (cltBilNmVo.CLSS_DESC.Equals("어음(타수)")))
            {
                Dao.BANK_ACCT_CD = "";
                Dao.BANK_ACCT_NM = "";

                //
                SystemCodeVo bilTpNmVo = this.combo_BIL_TP_NM.SelectedItem as SystemCodeVo;
                if (bilTpNmVo != null)
                {
                    Dao.BIL_TP_CD = bilTpNmVo.CLSS_CD;
                    Dao.BIL_TP_NM = bilTpNmVo.CLSS_DESC;
                }

                Dao.BIL_ACCT_NO = this.text_BIL_ACCT_NO.Text;
                Dao.BIL_ST_DT = Convert.ToDateTime(this.text_BIL_ST_DT.Text).ToString("yyyy-MM-dd");
                Dao.BIL_END_DT = Convert.ToDateTime(this.text_BIL_END_DT.Text).ToString("yyyy-MM-dd");

                Dao.BIL_USR_NM = this.text_BIL_USR_NM.Text;
                Dao.BIL_BANK_NM = this.text_BIL_BANK_NM.Text;
                //Dao.BIL_IN_NO = this.text_BIL_IN_NO.Text;
            }

            SystemCodeVo bilRmkNmVo = this.combo_BIL_RMK_NM.SelectedItem as SystemCodeVo;
            if (bilRmkNmVo != null)
            {
                Dao.BIL_RMK_CD = bilRmkNmVo.CLSS_CD;
                Dao.BIL_RMK_NM = bilRmkNmVo.CLSS_DESC;
            }

            Dao.BIL_IN_NO = this.text_BIL_IN_NO.Text;

            Dao.BIL_RMK = this.text_BIL_RMK.Text;

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

        public string CLT_BIL_NO
        { get; set; }

   
        private void text_Not_Spin(object sender, DevExpress.Xpf.Editors.SpinEventArgs e)
        {
            e.Handled = true;
        }


        public async void SYSTEM_CODE_VO()
        {
            //this.combo_CLT_CO_NM.ItemsSource = SystemProperties.CUSTOMER_CODE_VO("AR", SystemProperties.USER_VO.EMPE_PLC_NM);
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s143", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { DELT_FLG = "N", CO_TP_CD = "AR", AREA_CD = "100", CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.combo_CLT_CO_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
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
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "S-034"))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.combo_CLT_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                }
            }

            // this.combo_CLT_BIL_NM.ItemsSource = SystemProperties.SYSTEM_CODE_VO("S-035");
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "S-035"))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.combo_CLT_BIL_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                }
            }

            //this.combo_BIL_TP_NM.ItemsSource = SystemProperties.SYSTEM_CODE_VO("S-036");
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "S-036"))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.combo_BIL_TP_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                }
            }

            //this.combo_CLT_USR_NM.ItemsSource = SystemProperties.SYSTEM_CODE_VO("S-042");
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "S-042"))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.combo_CLT_USR_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                }
            }

            //IList<CodeDao> bilRmkList = SystemProperties.SYSTEM_CODE_VO("S-037");
            //bilRmkList.Insert(0, new CodeDao() { CLSS_CD = "", CLSS_DESC = "" });
            //this.combo_BIL_RMK_NM.ItemsSource = bilRmkList;
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "S-037"))
            {
                if (response.IsSuccessStatusCode)
                {
                    IList<SystemCodeVo> bilRmkList = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                    bilRmkList.Insert(0, new SystemCodeVo() { CLSS_CD = "", CLSS_DESC = "" });
                    this.combo_BIL_RMK_NM.ItemsSource = bilRmkList;
                }
            }

            //this.combo_BANK_ACCT_NM.ItemsSource = SystemProperties.BANK_CODE_VO();
        }
    }
}
