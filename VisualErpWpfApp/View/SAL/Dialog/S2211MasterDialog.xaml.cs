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
    public partial class S2211MasterDialog : DXWindow
    {
        //private static SaleOrderServiceClient saleOrderClient = SystemProperties.SaleOrderClient;
        private SaleVo orgDao;
        private bool isEdit = false;
        private SaleVo updateDao;
        private IList<SystemCodeVo> slTrspAreaList = new List<SystemCodeVo>();

        //private JusoDialog jusoDialog;

        private string title = "출하 의뢰 등록";

        public S2211MasterDialog(SaleVo Dao)
        {
            InitializeComponent();

            SYSTEM_CODE_VO();
            //USER_CODE_VO();
            this.combo_SL_TRSP_NM.SelectedIndexChanged += combo_SL_TRSP_NM_SelectedIndexChanged;

            this.orgDao = Dao;

            SaleVo copyDao = new SaleVo()
            {
                SL_RLSE_NO = Dao.SL_RLSE_NO,
                SL_RLSE_DT = Dao.SL_RLSE_DT,
                SL_AREA_NM = Dao.SL_AREA_NM,
                SL_AREA_CD = Dao.SL_AREA_CD,
                SL_TRSP_NM = Dao.SL_TRSP_NM,
                SL_TRSP_CD = Dao.SL_TRSP_CD,
                SL_TRSP_VEH_NM = Dao.SL_TRSP_VEH_NM,
                SL_TRSP_VEH_CD = Dao.SL_TRSP_VEH_CD,
                SL_TRSP_AREA_NM = Dao.SL_TRSP_AREA_NM,
                SL_TRSP_AREA_CD = Dao.SL_TRSP_AREA_CD,
                SL_TRSP_TM_NM = Dao.SL_TRSP_TM_NM,
                SL_TRSP_TM_CD = Dao.SL_TRSP_TM_CD,
                SL_RMK = Dao.SL_RMK,
                SL_CO_RLSE_FLG = Dao.SL_CO_RLSE_FLG,
                SL_RLSE_FLG = Dao.SL_RLSE_FLG,
                SL_NXT_CLZ_FLG = Dao.SL_NXT_CLZ_FLG,

                JB_NO = Dao.JB_NO,
                JB_NM = Dao.JB_NM,
                JB_CTRT_DT = Dao.JB_CTRT_DT,
                SL_CO_NO = Dao.SL_CO_NO,
                SL_CO_CD = Dao.SL_CO_CD,
                SL_CO_NM = Dao.SL_CO_NM,
                SL_DUE_DT = Dao.SL_DUE_DT,
                RGST_USR_ID = Dao.RGST_USR_ID,
                RGST_USR_NM = Dao.RGST_USR_NM,
                CLZ_FLG = Dao.CLZ_FLG,
                JB_RMK = Dao.JB_RMK,
                CRE_USR_ID = Dao.CRE_USR_ID,
                UPD_USR_ID = Dao.UPD_USR_ID,

                PCK_FLG = Dao.PCK_FLG,
                RCV_PPR_FLG = Dao.RCV_PPR_FLG,
                PCK_DEST_PHN_NO = Dao.PCK_DEST_PHN_NO,
                PCK_DEST_ADDR = Dao.PCK_DEST_ADDR,
                PCK_DEST_RMK = Dao.PCK_DEST_RMK,

                DUE_DT = Dao.DUE_DT
            };

            //수정
            if (copyDao.SL_RLSE_NO != null)
            {

                //this.text_SL_CO_RLSE_FLG.IsEnabled = false;
                //this.text_SL_RLSE_FLG.IsEnabled = false;
                this.text_SL_RLSE_NO.IsReadOnly = true;
                this.isEdit = true;
                //
                ////마감 처리 후 수정 불가능
                //if (Dao.MODI_FLG.Equals("N"))
                //{
                //    this.OKButton.IsEnabled = false;
                //}

                this.radioSL_TRSP_TM_NM_1.IsChecked = false;
                this.radioSL_TRSP_TM_NM_2.IsChecked = false;
                this.radioSL_TRSP_TM_NM_3.IsChecked = false;
                this.radioSL_TRSP_TM_NM_4.IsChecked = false;
                //출하 시간
                if (!string.IsNullOrEmpty(copyDao.SL_TRSP_TM_CD))
                {
                    if (copyDao.SL_TRSP_TM_CD.Equals("100"))
                    {
                        this.radioSL_TRSP_TM_NM_1.IsChecked = true;
                    }
                    else if (copyDao.SL_TRSP_TM_CD.Equals("200"))
                    {
                        this.radioSL_TRSP_TM_NM_2.IsChecked = true;
                    }
                    else if (copyDao.SL_TRSP_TM_CD.Equals("300"))
                    {
                        this.radioSL_TRSP_TM_NM_3.IsChecked = true;
                    }
                    else if (copyDao.SL_TRSP_TM_CD.Equals("400"))
                    {
                        this.radioSL_TRSP_TM_NM_4.IsChecked = true;
                    }
                    else if (copyDao.SL_TRSP_TM_CD.Equals("500"))
                    {
                        this.radioSL_TRSP_TM_NM_5.IsChecked = true;
                    }
                    else if (copyDao.SL_TRSP_TM_CD.Equals("600"))
                    {
                        this.radioSL_TRSP_TM_NM_6.IsChecked = true;
                    }
                }

                this.radioPCK_FLG_1.IsChecked = false;
                this.radioPCK_FLG_2.IsChecked = false;
                //팔래트 작업
                if (copyDao.PCK_FLG.Equals("Y"))
                {
                    this.radioPCK_FLG_1.IsChecked = true;
                }
                else
                {
                    this.text_PCK_DEST_PHN_NO.IsEnabled = false;
                    this.text_PCK_DEST_ADDR.IsEnabled = false;
                    this.ButtonAddr.IsEnabled = false;
                }

                if (copyDao.RCV_PPR_FLG.Equals("Y"))
                {
                    this.radioPCK_FLG_2.IsChecked = true;
                }

                this.text_PCK_DEST_PHN_NO.IsEnabled = false;
                this.text_PCK_DEST_ADDR.IsEnabled = false;
                this.ButtonAddr.IsEnabled = false;

                //수정 시 체크 
                if (copyDao.CLZ_FLG.Equals("Y"))
                {
                    this.OKButton.IsEnabled = false;
                    this.ButtonAddr.IsEnabled = false;
                }
            }
            else
            {
                //추가
                this.text_SL_RLSE_NO.IsReadOnly = true;
                this.isEdit = false;
                copyDao.JB_CTRT_DT = System.DateTime.Now.ToString("yyyy-MM-dd");
                copyDao.SL_RLSE_DT = System.DateTime.Now.ToString("yyyy-MM-dd");
                copyDao.SL_DUE_DT = System.DateTime.Now.ToString("yyyy-MM-dd");
                copyDao.DUE_DT = System.DateTime.Now.ToString("yyyy-MM-dd");
                copyDao.SL_TRSP_NM = "자차";

                copyDao.CLZ_FLG = "N";
                copyDao.RGST_USR_ID = SystemProperties.USER_VO.USR_ID;
                copyDao.RGST_USR_NM = SystemProperties.USER_VO.USR_N1ST_NM;

                copyDao.SL_CO_RLSE_FLG = "N";
                copyDao.SL_RLSE_FLG = "N";
                copyDao.SL_NXT_CLZ_FLG = "N";
                copyDao.PCK_FLG = "N";

                this.text_PCK_DEST_PHN_NO.IsEnabled = false;
                this.text_PCK_DEST_ADDR.IsEnabled = false;
                this.ButtonAddr.IsEnabled = false;

                this.textBlock_PCK_DEST_PHN_NO.Foreground = Brushes.DarkGray;
                this.textBlock_PCK_DEST_ADDR.Foreground = Brushes.DarkGray;

                this.radioPCK_FLG_1.IsChecked = true;
                this.radioPCK_FLG_2.IsChecked = true;
                //
                //Dao.DO_RQST_GRP_NM = SystemProperties.USERVO.GRP_NM;
                //Dao.DO_RQST_USR_NM = SystemProperties.USERVO.USR_N1ST_NM;
                //this.combo_DO_RQST_USR_NM.SelectedItem = ((IList<CodeDao>)combo_DO_RQST_USR_NM.ItemsSource)[2];
            }
            this.configCode.DataContext = copyDao;
            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
            this.OKButton.Click += new RoutedEventHandler(OKButton_Click);
            this.CancelButton.Click += new RoutedEventHandler(CancelButton_Click);

            this.ButtonAddr.Click += ButtonAddr_Click;

            this.combo_SL_CO_NM.SelectedIndexChanged += combo_SL_CO_NM_SelectedIndexChanged;

            this.combo_SL_AREA_NM.SelectedIndexChanged += combo_SL_AREA_NM_SelectedIndexChanged;

            this.combo_SL_CO_NM.Focus();
        }


        async void combo_SL_AREA_NM_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            SystemCodeVo areaNmVo = this.combo_SL_AREA_NM.SelectedItem as SystemCodeVo;
             if (areaNmVo != null)
             {
                if (SystemProperties.USER_VO.EMPE_PLC_NM.Equals("001"))
                {
                    //본사
                    //this.combo_SL_TRSP_AREA_NM.ItemsSource = SystemProperties.SYSTEM_CODE_VO("L-009");
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "L-009"))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            this.combo_SL_TRSP_AREA_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                        }
                    }

                    //this.combo_SL_CO_NM.ItemsSource = SystemProperties.CUSTOMER_CODE_VO("AR", "100");
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s143", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { DELT_FLG = "N", CO_TP_CD = "AR", AREA_CD = SystemProperties.USER_VO.EMPE_PLC_NM, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            this.combo_SL_CO_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                        }
                    }
                }
                //else if (SystemProperties.USER_VO.EMPE_PLC_NM.Equals("200"))
                //{
                //    //부산
                //    //this.combo_SL_TRSP_AREA_NM.ItemsSource = SystemProperties.SYSTEM_CODE_VO("L-112");
                //    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "L-112"))
                //    {
                //        if (response.IsSuccessStatusCode)
                //        {
                //            this.combo_SL_TRSP_AREA_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                //        }
                //    }

                //    //this.combo_SL_CO_NM.ItemsSource = SystemProperties.CUSTOMER_CODE_VO("AR", "200");
                //    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s143", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { DELT_FLG = "N", CO_TP_CD = "AR", AREA_CD = "200", CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                //    {
                //        if (response.IsSuccessStatusCode)
                //        {
                //            this.combo_SL_CO_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                //        }
                //    }
                //}
                //else if (SystemProperties.USER_VO.EMPE_PLC_NM.Equals("300"))
                //{
                //    //대구
                //    //this.combo_SL_TRSP_AREA_NM.ItemsSource = SystemProperties.SYSTEM_CODE_VO("L-113");
                //    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "L-113"))
                //    {
                //        if (response.IsSuccessStatusCode)
                //        {
                //            this.combo_SL_TRSP_AREA_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                //        }
                //    }

                //    //this.combo_SL_CO_NM.ItemsSource = SystemProperties.CUSTOMER_CODE_VO("AR", "300");
                //    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s143", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { DELT_FLG = "N", CO_TP_CD = "AR", AREA_CD = "300", CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                //    {
                //        if (response.IsSuccessStatusCode)
                //        {
                //            this.combo_SL_CO_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                //        }
                //    }
                //}
            }
        }

        void combo_SL_CO_NM_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            SystemCodeVo coNmVo = this.combo_SL_CO_NM.SelectedItem as SystemCodeVo;
            if (coNmVo != null)
            {
                this.combo_SL_TRSP_AREA_NM.Text = coNmVo.SL_AREA_NM;
            }
        }

        void ButtonAddr_Click(object sender, RoutedEventArgs e)
        {
            //jusoDialog = new JusoDialog();
            //jusoDialog.Title = "도로명 주소 / 지번 주소";
            //if (jusoDialog.ShowDialog() == true)
            //{
            //    CustomerCodeVo resultVo = jusoDialog.ResultDao;
            //    this.text_PCK_DEST_ADDR.Text = "(우)" + resultVo.ZIPCODE + "\r\n"
            //                                  + resultVo.SIDO + " "
            //                                  + resultVo.GUNGU + " "
            //                                  + resultVo.DORO_NM + " "
            //                                  + resultVo.BLDG_NO + " "
            //                                  + resultVo.BLDG_NM
            //                                  + (string.IsNullOrEmpty(resultVo.HAENG_DONG_NM) ? "" : "(" + resultVo.HAENG_DONG_NM + ")") + "\r\n"
            //                                  + resultVo.Message + "\r\n";
            //                                  //+ "(" + resultVo.SIDO + " "
            //                                  //      + resultVo.GUNGU + " "
            //                                  //      + resultVo.LAW_DONG_NM + " "
            //                                  //      + resultVo.LAW_RI_NM + " "
            //                                  //      + resultVo.JIBUN_NO
            //                                  //+ ")";
            //}

        }

        void combo_SL_TRSP_NM_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            if (this.combo_SL_TRSP_NM.Text.Equals("화물"))
            {
                this.text_SL_TRSP_VEH_NM.Foreground = Brushes.Black;
                this.combo_SL_TRSP_VEH_NM.IsEnabled = true;
            }
            else
            {
                this.text_SL_TRSP_VEH_NM.Foreground = Brushes.DarkGray;
                this.combo_SL_TRSP_VEH_NM.IsEnabled = false;
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
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s2211/mst/i", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
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

                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s2211/mst/u", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
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
                //    resultVo = saleOrderClient.S2211SelectJbNo(updateDao);
                //    if (!resultVo.isSuccess)
                //    {
                //        //실패
                //        WinUIMessageBox.Show(resultVo.Message, "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
                //        return;
                //    }
                //    SL_RLSE_NO = resultVo.LST_FMT_NO;
                //    updateDao.SL_RLSE_NO = resultVo.LST_FMT_NO;
                //    this.text_SL_RLSE_NO.Text = resultVo.LST_FMT_NO;
                //    //MessageBoxResult result = WinUIMessageBox.Show("[" + updateDao.INSRL_NO + "] 저장 하시겠습니까?", "[전표 번호]창고간 이동", MessageBoxButton.YesNo, MessageBoxImage.Question);
                //    //if (result == MessageBoxResult.Yes)
                //    //{
                //    resultVo = saleOrderClient.S2211InsertMst(updateDao);
                //    if (!resultVo.isSuccess)
                //    {
                //        //실패
                //        WinUIMessageBox.Show(resultVo.Message, "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
                //        return;
                //    }
                //    //성공
                //    WinUIMessageBox.Show("[요청 번호 : " + updateDao.SL_RLSE_NO + "] 완료 되었습니다", "[추가]" + title, MessageBoxButton.OK, MessageBoxImage.Information);
                //    //}
                //}
                //else
                //{
                //    updateDao = getDomain();
                //    resultVo = saleOrderClient.S2211UpdateMst(updateDao);
                //    if (!resultVo.isSuccess)
                //    {
                //        //실패
                //        WinUIMessageBox.Show(resultVo.Message, "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
                //        return;
                //    }
                //    //성공
                //    WinUIMessageBox.Show("완료 되었습니다", "[수정]" + title, MessageBoxButton.OK, MessageBoxImage.Information);


                //    this.orgDao.JB_NO = this.updateDao.JB_NO;
                //    this.orgDao.JB_NM = this.updateDao.JB_NM;
                //    this.orgDao.JB_CTRT_DT = this.updateDao.JB_CTRT_DT;
                //    this.orgDao.SL_CO_NO = this.updateDao.SL_CO_NO;
                //    this.orgDao.SL_CO_CD = this.updateDao.SL_CO_CD;
                //    this.orgDao.SL_CO_NM = this.updateDao.SL_CO_NM;
                //    this.orgDao.SL_DUE_DT = this.updateDao.SL_DUE_DT;
                //    this.orgDao.RGST_USR_ID = this.updateDao.RGST_USR_ID;
                //    this.orgDao.RGST_USR_NM = this.updateDao.RGST_USR_NM;
                //    this.orgDao.CLZ_FLG = this.updateDao.CLZ_FLG;
                //    this.orgDao.JB_RMK = this.updateDao.JB_RMK;
                //    this.orgDao.CRE_USR_ID = this.updateDao.CRE_USR_ID;
                //    this.orgDao.UPD_USR_ID = this.updateDao.UPD_USR_ID;

                //    this.orgDao.SL_RLSE_NO = this.updateDao.SL_RLSE_NO;
                //    this.orgDao.SL_RLSE_DT = this.updateDao.SL_RLSE_DT;
                //    this.orgDao.SL_AREA_NM = this.updateDao.SL_AREA_NM;
                //    this.orgDao.SL_AREA_CD = this.updateDao.SL_AREA_CD;
                //    this.orgDao.SL_TRSP_NM = this.updateDao.SL_TRSP_NM;
                //    this.orgDao.SL_TRSP_CD = this.updateDao.SL_TRSP_CD;
                //    this.orgDao.SL_TRSP_VEH_NM = this.updateDao.SL_TRSP_VEH_NM;
                //    this.orgDao.SL_TRSP_VEH_CD = this.updateDao.SL_TRSP_VEH_CD;
                //    this.orgDao.SL_TRSP_AREA_NM = this.updateDao.SL_TRSP_AREA_NM;
                //    this.orgDao.SL_TRSP_AREA_CD = this.updateDao.SL_TRSP_AREA_CD;
                //    this.orgDao.SL_TRSP_TM_NM = this.updateDao.SL_TRSP_TM_NM;
                //    this.orgDao.SL_TRSP_TM_CD = this.updateDao.SL_TRSP_TM_CD;
                //    this.orgDao.SL_RMK = this.updateDao.SL_RMK;
                //    this.orgDao.SL_CO_RLSE_FLG = this.updateDao.SL_CO_RLSE_FLG;
                //    this.orgDao.SL_RLSE_FLG = this.updateDao.SL_RLSE_FLG;
                //    this.orgDao.SL_NXT_CLZ_FLG = this.updateDao.SL_NXT_CLZ_FLG;

                //    this.orgDao.PCK_FLG = this.updateDao.PCK_FLG;
                //    this.orgDao.RCV_PPR_FLG = this.updateDao.RCV_PPR_FLG;
                //    //this.orgDao.PCK_FLG_NM = this.updateDao.PCK_FLG_NM;

                //    this.orgDao.PCK_DEST_PHN_NO = this.updateDao.PCK_DEST_PHN_NO;
                //    this.orgDao.PCK_DEST_ADDR = this.updateDao.PCK_DEST_ADDR;
                //    this.orgDao.PCK_DEST_RMK = this.updateDao.PCK_DEST_RMK;
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
                SL_RLSE_NO = this.updateDao.SL_RLSE_NO;
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
            //if (string.IsNullOrEmpty(this.text_SL_RLSE_NO.Text))
            //{
            //    WinUIMessageBox.Show("[요청 번호] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.text_SL_RLSE_NO.IsTabStop = true;
            //    this.text_SL_RLSE_NO.Focus();
            //    return false;
            //}
            //else if (string.IsNullOrEmpty(this.text_JB_CTRT_DT.Text))
            //{
            //    WinUIMessageBox.Show("[등록(계약)일자] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.text_JB_CTRT_DT.IsTabStop = true;
            //    this.text_JB_CTRT_DT.Focus();
            //    return false;
            //}
            //else if (string.IsNullOrEmpty(this.combo_SL_CO_NM.Text))
            //{
            //    WinUIMessageBox.Show("[수주처] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.combo_SL_CO_NM.IsTabStop = true;
            //    this.combo_SL_CO_NM.Focus();
            //    return false;
            //}
            //else if (string.IsNullOrEmpty(this.text_SL_DUE_DT.Text))
            //{
            //    WinUIMessageBox.Show("[납기일] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.text_SL_DUE_DT.IsTabStop = true;
            //    this.text_SL_DUE_DT.Focus();
            //    return false;
            //}
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

            //Dao.JB_NO = this.text_JB_NO.Text;
            //Dao.JB_NM = this.text_JB_NM.Text;
            //Dao.JB_CTRT_DT = Convert.ToDateTime(this.text_JB_CTRT_DT.Text).ToString("yyyy-MM-dd");

            //CustomerCodeDao coNmVo = this.combo_SL_CO_NM.SelectedItem as CustomerCodeDao;
            //Dao.SL_CO_NO = coNmVo.CO_NO;
            //Dao.SL_CO_NM = coNmVo.CO_NM;

            //Dao.SL_DUE_DT = Convert.ToDateTime(this.text_SL_DUE_DT.Text).ToString("yyyy-MM-dd");

            //UserCodeDao rgstUsrIdVo = this.combo_RGST_USR_ID.SelectedItem as UserCodeDao;
            //Dao.RGST_USR_ID = rgstUsrIdVo.USR_ID;
            //Dao.RGST_USR_NM = rgstUsrIdVo.USR_N1ST_NM;

            //Dao.JB_RMK = this.text_JB_RMK.Text;

            //Dao.CLZ_FLG = this.text_CLZ_FLG.Text;




            Dao.SL_RLSE_NO = this.text_SL_RLSE_NO.Text;
            Dao.SL_RLSE_DT = Convert.ToDateTime(this.text_SL_RLSE_DT.Text).ToString("yyyy-MM-dd");

            Dao.DUE_DT = Convert.ToDateTime(this.text_DUE_DT.Text).ToString("yyyy-MM-dd");

            SystemCodeVo coNmVo = this.combo_SL_CO_NM.SelectedItem as SystemCodeVo;
            if (coNmVo != null)
            {
                Dao.SL_CO_NO = coNmVo.CO_NO;
                Dao.SL_CO_CD = coNmVo.CO_NO;
                Dao.SL_CO_NM = coNmVo.CO_NM;
            }

            SystemCodeVo areaNmVo = this.combo_SL_AREA_NM.SelectedItem as SystemCodeVo;
            if (areaNmVo != null)
            {
                //Dao.AREA_CD = areaNmVo.CLSS_CD;
                Dao.SL_AREA_CD = areaNmVo.CLSS_CD;
                Dao.SL_AREA_NM = areaNmVo.CLSS_DESC;
            }

            SystemCodeVo slTrspNmVo = this.combo_SL_TRSP_NM.SelectedItem as SystemCodeVo;
            if (slTrspNmVo != null)
            {
                Dao.SL_TRSP_CD = slTrspNmVo.CLSS_CD;
                Dao.SL_TRSP_NM = slTrspNmVo.CLSS_DESC;
            }

            if (this.combo_SL_TRSP_NM.Text.Equals("화물"))
            {
                SystemCodeVo slTrspVehNmVo = this.combo_SL_TRSP_VEH_NM.SelectedItem as SystemCodeVo;
                if (slTrspVehNmVo != null)
                {
                    Dao.SL_TRSP_VEH_CD = slTrspVehNmVo.CLSS_CD;
                    Dao.SL_TRSP_VEH_NM = slTrspVehNmVo.CLSS_DESC;
                }
            }

            SystemCodeVo slTrspAreaNmVo = this.combo_SL_TRSP_AREA_NM.SelectedItem as SystemCodeVo;
            if (slTrspAreaNmVo != null)
            {
                Dao.SL_TRSP_AREA_CD = slTrspAreaNmVo.CLSS_CD;
                Dao.SL_TRSP_AREA_NM = slTrspAreaNmVo.CLSS_DESC;
            }



            //출하 시간
            if (this.radioSL_TRSP_TM_NM_1.IsChecked == true)
            {
                Dao.SL_TRSP_TM_CD = this.slTrspAreaList[0].CLSS_CD;
                Dao.SL_TRSP_TM_NM = this.slTrspAreaList[0].CLSS_DESC;
            }
            else if (this.radioSL_TRSP_TM_NM_2.IsChecked == true)
            {
                Dao.SL_TRSP_TM_CD = this.slTrspAreaList[1].CLSS_CD;
                Dao.SL_TRSP_TM_NM = this.slTrspAreaList[1].CLSS_DESC;
            }
            else if (this.radioSL_TRSP_TM_NM_3.IsChecked == true)
            {
                Dao.SL_TRSP_TM_CD = this.slTrspAreaList[2].CLSS_CD;
                Dao.SL_TRSP_TM_NM = this.slTrspAreaList[2].CLSS_DESC;
            }
            else if (this.radioSL_TRSP_TM_NM_4.IsChecked == true)
            {
                Dao.SL_TRSP_TM_CD = this.slTrspAreaList[3].CLSS_CD;
                Dao.SL_TRSP_TM_NM = this.slTrspAreaList[3].CLSS_DESC;
            }
            else if (this.radioSL_TRSP_TM_NM_5.IsChecked == true)
            {
                Dao.SL_TRSP_TM_CD = this.slTrspAreaList[4].CLSS_CD;
                Dao.SL_TRSP_TM_NM = this.slTrspAreaList[4].CLSS_DESC;
            }
            else if (this.radioSL_TRSP_TM_NM_6.IsChecked == true)
            {
                Dao.SL_TRSP_TM_CD = this.slTrspAreaList[5].CLSS_CD;
                Dao.SL_TRSP_TM_NM = this.slTrspAreaList[5].CLSS_DESC;
            }
            //if (copyDao.SL_TRSP_TM_CD.Equals("100"))
            //{
            //    this.radioSL_TRSP_TM_NM_1.IsChecked = true;
            //}
            //else if (copyDao.SL_TRSP_TM_CD.Equals("200"))
            //{
            //    this.radioSL_TRSP_TM_NM_2.IsChecked = true;
            //}
            //else if (copyDao.SL_TRSP_TM_CD.Equals("300"))
            //{
            //    this.radioSL_TRSP_TM_NM_3.IsChecked = true;
            //}
            //else if (copyDao.SL_TRSP_TM_CD.Equals("400"))
            //{
            //    this.radioSL_TRSP_TM_NM_4.IsChecked = true;
            //}

            //CodeDao slTrspTmNmVo = this.combo_SL_TRSP_TM_NM.SelectedItem as CodeDao;
            //if (slTrspTmNmVo != null)
            //{
            //    Dao.SL_TRSP_TM_CD = slTrspTmNmVo.CLSS_CD;
            //    Dao.SL_TRSP_TM_NM = slTrspTmNmVo.CLSS_DESC;
            //}

            Dao.SL_RMK = this.text_SL_RMK.Text;

            Dao.SL_CO_RLSE_FLG = this.text_SL_CO_RLSE_FLG.Text;
            Dao.SL_RLSE_FLG = this.text_SL_RLSE_FLG.Text;
            Dao.SL_NXT_CLZ_FLG = this.text_SL_NXT_CLZ_FLG.Text;

            //팔레트/인수증 작업 여부
            //Dao.PCK_FLG = this.text_PCK_FLG.Text;
            if (this.radioPCK_FLG_1.IsChecked == true)
            {
                Dao.PCK_FLG = "Y";
                //Dao.PCK_FLG_NM = "팔레트";
                Dao.PCK_DEST_PHN_NO = this.text_PCK_DEST_PHN_NO.Text;
                Dao.PCK_DEST_ADDR = this.text_PCK_DEST_ADDR.Text;
            }
            else
            {
                Dao.PCK_FLG = "N";
                Dao.PCK_DEST_PHN_NO = "";
                Dao.PCK_DEST_ADDR = "";
            }

            if (this.radioPCK_FLG_2.IsChecked == true)
            {
                Dao.RCV_PPR_FLG = "Y";
                //Dao.PCK_FLG_NM = "인수증";
                //Dao.PCK_DEST_PHN_NO = this.text_PCK_DEST_PHN_NO.Text;
                //Dao.PCK_DEST_ADDR = this.text_PCK_DEST_ADDR.Text;
            }
            else
            {
                Dao.RCV_PPR_FLG = "N";
            }

            //Dao.PCK_DEST_PHN_NO = this.text_PCK_DEST_PHN_NO.Text;
            //Dao.PCK_DEST_ADDR = this.text_PCK_DEST_ADDR.Text;
            Dao.PCK_DEST_RMK = this.text_PCK_DEST_RMK.Text;

            Dao.CRE_USR_ID = SystemProperties.USER;
            Dao.UPD_USR_ID = SystemProperties.USER;

            Dao.RGST_USR_ID = SystemProperties.USER;
            Dao.RGST_USR_NM = SystemProperties.USER_VO.USR_N1ST_NM;

            Dao.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;

            Dao.CLZ_FLG = this.orgDao.CLZ_FLG;

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


        #region Functon (출하 시간 체크 박스)
        private void radioSL_TRSP_TM_NM_1_Checked(object sender, RoutedEventArgs e)
        {
            this.radioSL_TRSP_TM_NM_1.IsChecked = true;
            this.radioSL_TRSP_TM_NM_2.IsChecked = false;
            this.radioSL_TRSP_TM_NM_3.IsChecked = false;
            this.radioSL_TRSP_TM_NM_4.IsChecked = false;
            this.radioSL_TRSP_TM_NM_5.IsChecked = false;
            this.radioSL_TRSP_TM_NM_6.IsChecked = false;
        }
        private void radioSL_TRSP_TM_NM_2_Checked(object sender, RoutedEventArgs e)
        {
            this.radioSL_TRSP_TM_NM_1.IsChecked = false;
            this.radioSL_TRSP_TM_NM_2.IsChecked = true;
            this.radioSL_TRSP_TM_NM_3.IsChecked = false;
            this.radioSL_TRSP_TM_NM_4.IsChecked = false;
            this.radioSL_TRSP_TM_NM_5.IsChecked = false;
            this.radioSL_TRSP_TM_NM_6.IsChecked = false;
        }
        private void radioSL_TRSP_TM_NM_3_Checked(object sender, RoutedEventArgs e)
        {
            this.radioSL_TRSP_TM_NM_1.IsChecked = false;
            this.radioSL_TRSP_TM_NM_2.IsChecked = false;
            this.radioSL_TRSP_TM_NM_3.IsChecked = true;
            this.radioSL_TRSP_TM_NM_4.IsChecked = false;
            this.radioSL_TRSP_TM_NM_5.IsChecked = false;
            this.radioSL_TRSP_TM_NM_6.IsChecked = false;
        }
        private void radioSL_TRSP_TM_NM_4_Checked(object sender, RoutedEventArgs e)
        {
            this.radioSL_TRSP_TM_NM_1.IsChecked = false;
            this.radioSL_TRSP_TM_NM_2.IsChecked = false;
            this.radioSL_TRSP_TM_NM_3.IsChecked = false;
            this.radioSL_TRSP_TM_NM_4.IsChecked = true;
            this.radioSL_TRSP_TM_NM_5.IsChecked = false;
            this.radioSL_TRSP_TM_NM_6.IsChecked = false;
        }
        private void radioSL_TRSP_TM_NM_5_Checked(object sender, RoutedEventArgs e)
        {
            this.radioSL_TRSP_TM_NM_1.IsChecked = false;
            this.radioSL_TRSP_TM_NM_2.IsChecked = false;
            this.radioSL_TRSP_TM_NM_3.IsChecked = false;
            this.radioSL_TRSP_TM_NM_4.IsChecked = false;
            this.radioSL_TRSP_TM_NM_5.IsChecked = true;
            this.radioSL_TRSP_TM_NM_6.IsChecked = false;
        }
        private void radioSL_TRSP_TM_NM_6_Checked(object sender, RoutedEventArgs e)
        {
            this.radioSL_TRSP_TM_NM_1.IsChecked = false;
            this.radioSL_TRSP_TM_NM_2.IsChecked = false;
            this.radioSL_TRSP_TM_NM_3.IsChecked = false;
            this.radioSL_TRSP_TM_NM_4.IsChecked = false;
            this.radioSL_TRSP_TM_NM_5.IsChecked = false;
            this.radioSL_TRSP_TM_NM_6.IsChecked = true;
        }
        #endregion


        #region Functon (출하 시간 체크 박스)
        private void radioPCK_FLG_1_Checked(object sender, RoutedEventArgs e)
        {
            this.radioPCK_FLG_1.IsChecked = true;
            //this.radioPCK_FLG_2.IsChecked = false;

            this.text_PCK_DEST_PHN_NO.IsEnabled = true;
            this.text_PCK_DEST_ADDR.IsEnabled = true;
            this.ButtonAddr.IsEnabled = true;

            this.textBlock_PCK_DEST_PHN_NO.Foreground = Brushes.Black;
            this.textBlock_PCK_DEST_ADDR.Foreground = Brushes.Black;
        }
        private void radioPCK_FLG_2_Checked(object sender, RoutedEventArgs e)
        {
            //this.radioPCK_FLG_1.IsChecked = false;
            this.radioPCK_FLG_2.IsChecked = true;

            //this.text_PCK_DEST_PHN_NO.IsEnabled = false;
            //this.text_PCK_DEST_ADDR.IsEnabled = false;
            //this.ButtonAddr.IsEnabled = false;

            //this.textBlock_PCK_DEST_PHN_NO.Foreground = Brushes.DarkGray;
            //this.textBlock_PCK_DEST_ADDR.Foreground = Brushes.DarkGray;
        }
        #endregion

        private void radioPCK_FLG_1_Unchecked(object sender, RoutedEventArgs e)
        {
            this.radioPCK_FLG_1.IsChecked = false;

            this.text_PCK_DEST_PHN_NO.IsEnabled = false;
            this.text_PCK_DEST_ADDR.IsEnabled = false;
            this.ButtonAddr.IsEnabled = false;

            this.textBlock_PCK_DEST_PHN_NO.Foreground = Brushes.DarkGray;
            this.textBlock_PCK_DEST_ADDR.Foreground = Brushes.DarkGray;
        }

        public async void SYSTEM_CODE_VO()
        {
            //this.combo_SL_AREA_NM.ItemsSource = SystemProperties.SYSTEM_CODE_VO("L-002");
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "L-002"))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.combo_SL_AREA_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                }
            }

            //this.combo_SL_TRSP_NM.ItemsSource = SystemProperties.SYSTEM_CODE_VO("S-025");
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "S-025"))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.combo_SL_TRSP_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                }
            }
            //this.combo_SL_TRSP_NM.SelectedIndexChanged += combo_SL_TRSP_NM_SelectedIndexChanged;


            if (SystemProperties.USER_VO.EMPE_PLC_NM.Equals("001"))
            {
                //본사
                //this.combo_SL_TRSP_AREA_NM.ItemsSource = SystemProperties.SYSTEM_CODE_VO("L-009");
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "L-009"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.combo_SL_TRSP_AREA_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                    }
                }

                //this.combo_SL_CO_NM.ItemsSource = SystemProperties.CUSTOMER_CODE_VO("AR", "100");
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s143", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { DELT_FLG = "N", SEEK = "", CO_TP_CD = "AR", SEEK_AR = "AR", AREA_CD = SystemProperties.USER_VO.EMPE_PLC_NM, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.combo_SL_CO_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                    }
                }
            }
            //else if (SystemProperties.USER_VO.EMPE_PLC_NM.Equals("200"))
            //{
            //    //부산
            //    //this.combo_SL_TRSP_AREA_NM.ItemsSource = SystemProperties.SYSTEM_CODE_VO("L-112");
            //    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "L-112"))
            //    {
            //        if (response.IsSuccessStatusCode)
            //        {
            //            this.combo_SL_TRSP_AREA_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
            //        }
            //    }

            //    //this.combo_SL_CO_NM.ItemsSource = SystemProperties.CUSTOMER_CODE_VO("AR", "200");
            //    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s143", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { DELT_FLG = "N", CO_TP_CD = "AR", AREA_CD = "200", CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
            //    {
            //        if (response.IsSuccessStatusCode)
            //        {
            //            this.combo_SL_CO_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
            //        }
            //    }
            //}
            //else if (SystemProperties.USER_VO.EMPE_PLC_NM.Equals("300"))
            //{
            //    //대구
            //    //this.combo_SL_TRSP_AREA_NM.ItemsSource = SystemProperties.SYSTEM_CODE_VO("L-113");
            //    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "L-113"))
            //    {
            //        if (response.IsSuccessStatusCode)
            //        {
            //            this.combo_SL_TRSP_AREA_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
            //        }
            //    }

            //    //this.combo_SL_CO_NM.ItemsSource = SystemProperties.CUSTOMER_CODE_VO("AR", "300");
            //    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s143", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { DELT_FLG = "N", CO_TP_CD = "AR", AREA_CD = "300", CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
            //    {
            //        if (response.IsSuccessStatusCode)
            //        {
            //            this.combo_SL_CO_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
            //        }
            //    }
            //}



            //
            //this.slTrspAreaList = SystemProperties.SYSTEM_CODE_VO("S-027");
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "S-027"))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.slTrspAreaList = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                }
            }

            //this.combo_SL_TRSP_VEH_NM.ItemsSource = SystemProperties.SYSTEM_CODE_VO("S-028");
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "S-028"))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.combo_SL_TRSP_VEH_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                }
            }
        }
        //public async void USER_CODE_VO()
        //{
        //    //this.combo_RGST_USR_ID.ItemsSource = SystemProperties.USER_CODE_VO();
        //    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s136/usr", new StringContent(JsonConvert.SerializeObject(new GroupUserVo() { DELT_FLG = "N", CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
        //    {
        //        if (response.IsSuccessStatusCode)
        //        {
        //            this.combo_INP_ID.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<GroupUserVo>>(await response.Content.ReadAsStringAsync()).Cast<GroupUserVo>().ToList();
        //        }
        //    }
        //}

    }
}
