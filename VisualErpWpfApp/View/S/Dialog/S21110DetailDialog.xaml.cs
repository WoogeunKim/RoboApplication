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
using System.Windows.Media;
using AquilaErpWpfApp3.Util;

namespace AquilaErpWpfApp3.View.S.Dialog
{
    public partial class S21110DetailDialog : DXWindow
    {
        //private static PurServiceClient purClient = SystemProperties.PurClient;
        private PurVo orgDao;
        private bool isEdit = false;
        private PurVo updateDao;

        //
        //private CustomerDialog customersDialog;

        public S21110DetailDialog(PurVo Dao)
        {
            InitializeComponent();


            SYSTEM_CODE_VO();
            //this.btn_CO_NM.Click += btn_CO_NM_Click;
            //this.combo_CURR_CD.ItemsSource = SystemProperties.SYSTEM_CODE_VO("S-007");

            this.orgDao = Dao;
            PurVo copyDao = new PurVo()
            {
                ITM_CD = Dao.ITM_CD,
                CO_ITM_CD = Dao.CO_ITM_CD,
                CO_NO = Dao.CO_NO,
                CO_NM = Dao.CO_NM,
                CNG_APLY_DT = Dao.CNG_APLY_DT,
                ITM_BZTP_FLG = Dao.ITM_BZTP_FLG,
                CO_UT_PRC = Dao.CO_UT_PRC,
                CURR_CD = Dao.CURR_CD,
                CNG_HIS_DESC = Dao.CNG_HIS_DESC,
                CO_ITM_NM = Dao.CO_ITM_NM,
                MN_CO_FLG = Dao.MN_CO_FLG,
                CRNT_PRC_FLG = Dao.CRNT_PRC_FLG,
                CHNL_CD = SystemProperties.USER_VO.CHNL_CD
            };


            //if (Dao.MN_CO_FLG.Equals(1))
            if (Convert.ToBoolean(Dao.MN_CO_FLG) == true)
            {
                text_MN_CO_FLG.IsChecked = true;
            }
            else
            {
                text_MN_CO_FLG.IsChecked = false;
            }
            //
            //
            //if (Dao.CRNT_PRC_FLG.Equals(1))
            if (Convert.ToBoolean(Dao.CRNT_PRC_FLG) == true)
            {
                text_CRNT_PRC_FLG.IsChecked = true;
            }
            else
            {
                text_CRNT_PRC_FLG.IsChecked = false;
            }

            //수정
            if (Dao.CO_NO != null)
            {
                this.combo_CO_NO.IsEnabled = false;
                //
                this.text_CNG_APLY_DT.Foreground = Brushes.DarkGray;
                this.text_CNG_APLY_DT.IsReadOnly = true;
                this.text_CNG_APLY_DT.IsEnabled = false;
                //
                this.text_CO_ITM_CD.Foreground = Brushes.DarkGray;
                this.text_CO_ITM_CD.IsReadOnly = true;
                this.text_CO_ITM_CD.IsEnabled = false;
                //
                this.text_CO_ITM_NM.Foreground = Brushes.DarkGray;
                this.text_CO_ITM_NM.IsReadOnly = true;
                this.text_CO_ITM_NM.IsEnabled = false;
                //
                this.isEdit = true;
            }
            else
            {
                //추가
                //this.text_ClssTpCd.IsReadOnly = false;
                this.isEdit = false;
            }
            this.configCode.DataContext = copyDao;
            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
            this.OKButton.Click += new RoutedEventHandler(OKButton_Click);
            this.CancelButton.Click += new RoutedEventHandler(CancelButton_Click);
        }

        //void btn_CO_NM_Click(object sender, RoutedEventArgs e)
        //{
            //customersDialog = new CustomerDialog();
            //customersDialog.Title = "거래처 코드";
            //customersDialog.Owner = Application.Current.MainWindow;
            //customersDialog.BorderEffect = BorderEffect.Default;
            ////customersDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            ////customersDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            //bool isDialog = (bool)customersDialog.ShowDialog();
            //if (isDialog)
            //{
            //    CustomerCodeVo coNmDao = customersDialog.ResultDao;
            //    if (coNmDao != null)
            //    {
            //        this.text_CO_NO.Text = coNmDao.CO_NO;
            //        this.text_CO_NM.Text = coNmDao.CO_NM;
            //    }
            //}
        //}

        #region Functon (OKButton_Click, CancelButton_Click)
        private async void OKButton_Click(object sender, RoutedEventArgs e)
        {
            int _Num = 0;
            if (ValueCheckd())
            {
                //PurVo resultVo;
                if (isEdit == false)
                {
                    this.updateDao = getDomain();//this.updateDao
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("p4401/dtl/i", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string result = await response.Content.ReadAsStringAsync();
                            if (int.TryParse(result, out _Num) == false)
                            {
                                //실패
                                WinUIMessageBox.Show(result, "매입 단가 관리", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }

                            //성공
                            WinUIMessageBox.Show("완료 되었습니다", "매입 단가 관리", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }
                else
                {
                    this.updateDao = getDomain();

                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("p4401/dtl/u", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string result = await response.Content.ReadAsStringAsync();
                            if (int.TryParse(result, out _Num) == false)
                            {
                                //실패
                                WinUIMessageBox.Show(result, "매입 단가 관리", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }

                            //성공
                            WinUIMessageBox.Show("완료 되었습니다", "매입 단가 관리", MessageBoxButton.OK, MessageBoxImage.Information);
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
            if (string.IsNullOrEmpty(this.combo_CO_NO.Text))
            {
                WinUIMessageBox.Show("[거래처 명] 입력 값이 맞지 않습니다.", "[유효검사]매입 단가 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
                this.combo_CO_NO.IsTabStop = true;
                this.combo_CO_NO.Focus();
                return false;
            }
            //else if (string.IsNullOrEmpty(this.text_CO_NM.Text))
            //{
            //    WinUIMessageBox.Show("[거래처 명] 입력 값이 맞지 않습니다.", "[유효검사]매입 단가 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.text_CO_NM.IsTabStop = true;
            //    this.text_CO_NM.Focus();
            //    return false;
            //}
            else if (string.IsNullOrEmpty(this.text_CNG_APLY_DT.Text))
            {
                WinUIMessageBox.Show("[적용일자] 입력 값이 맞지 않습니다.", "[유효검사]매입 단가 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
                this.text_CNG_APLY_DT.IsTabStop = true;
                this.text_CNG_APLY_DT.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(this.combo_CURR_CD.Text))
            {
                WinUIMessageBox.Show("[통화] 입력 값이 맞지 않습니다.", "[유효검사]매입 단가 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
                this.combo_CURR_CD.IsTabStop = true;
                this.combo_CURR_CD.Focus();
                return false;
            }
            //else if (string.IsNullOrEmpty(this.combo_CRE_USR_ID.Text))
            //{
            //    WinUIMessageBox.Show("[작성자] 입력 값이 맞지 않습니다.", "[유효검사]견적 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.combo_CRE_USR_ID.IsTabStop = true;
            //    this.combo_CRE_USR_ID.Focus();
            //    return false;
            //}
            //else if (this.combo_deltFlg.Text == null || this.combo_deltFlg.Text.Trim().Length == 0)
            //{
            //    WinUIMessageBox.Show("[사용 여부] 입력 값이 맞지 않습니다.", "[유효검사]견적 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.combo_deltFlg.IsTabStop = true;
            //    this.combo_deltFlg.Focus();
            //    return false;
            //}
            else
            {
                //if (this.isEdit == false)
                //{
                //    PurVo dao = new PurVo()
                //    {
                //        ITM_CD = this.orgDao.ITM_CD,
                //        CO_ITM_CD = this.text_CO_ITM_CD.Text,
                //        CO_NO = this.text_CO_NO.Text,
                //        CNG_APLY_DT = Convert.ToDateTime(this.text_CNG_APLY_DT.Text).ToString("yyyy-MM-dd"),
                //        ITM_BZTP_FLG = "P"
                //    };
                //    IList<PurVo> daoList = (IList<PurVo>)purClient.P4401SelectDtlList(dao);
                //    if (daoList.Count != 0)
                //    {
                //        WinUIMessageBox.Show("[매입 단가 관리 - 중복] 코드를 다시 입력 하십시오.", "[중복검사]매입 단가 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
                //        this.text_CO_NO.IsTabStop = true;
                //        this.text_CO_NO.Focus();
                //        return false;
                //    }
                //}
            }
            return true;
        }
        #endregion

        #region Functon (getDomain - ConfigView1Dao)
        private PurVo getDomain()
        {
            PurVo Dao = new PurVo();
            Dao.ITM_CD = this.orgDao.ITM_CD;

            SystemCodeVo CO_CD_VO = this.combo_CO_NO.SelectedItem as SystemCodeVo;
            if (CO_CD_VO != null)
            {
                Dao.CO_NO = CO_CD_VO.CO_NO;
                Dao.CO_NM = CO_CD_VO.CO_NM;
            }
            //Dao.CO_NO = this.text_CO_NO.Text;
            //Dao.CO_NM = this.text_CO_NM.Text;
            Dao.ITM_BZTP_FLG = "S";
            Dao.CNG_APLY_DT = Convert.ToDateTime(this.text_CNG_APLY_DT.Text).ToString("yyyy-MM-dd");
            Dao.CO_UT_PRC = double.Parse(text_CO_UT_PRC.Text);
            Dao.MN_CO_FLG = (this.text_MN_CO_FLG.IsChecked == true ? "Y" : "N");
            Dao.CRNT_PRC_FLG = (this.text_CRNT_PRC_FLG.IsChecked == true ? "Y" : "N");
            Dao.CNG_HIS_DESC = this.text_CNG_HIS_DESC.Text;
            Dao.CO_ITM_CD = this.text_CO_ITM_CD.Text;
            Dao.CO_ITM_NM = this.text_CO_ITM_NM.Text;

            //
            SystemCodeVo CURR_CD_VO = this.combo_CURR_CD.SelectedItem as SystemCodeVo;
            if (CURR_CD_VO != null)
            {
                Dao.CURR_NM = CURR_CD_VO.CLSS_DESC;
                Dao.CURR_CD = CURR_CD_VO.CLSS_CD;
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
            // //this.combo_CURR_CD.ItemsSource = SystemProperties.SYSTEM_CODE_VO("S-007");
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "S-007"))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.combo_CURR_CD.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                }
            }

            //CO_NO
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s143", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { DELT_FLG = "N", CO_TP_CD = "AR", AREA_CD = SystemProperties.USER_VO.EMPE_PLC_NM, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.combo_CO_NO.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                }
            }
        }

    }
}
