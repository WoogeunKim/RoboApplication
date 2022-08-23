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
    /// Interaction logic for S1431MasterDialog.xaml
    /// </summary>
    public partial class S1431MasterDialog : DXWindow
    {
        private string title     = "차량 마스터 관리";
        private bool isEdit      = false;
        private SystemCodeVo orgVo;

        private SystemCodeVo updateVo;

        public S1431MasterDialog(SystemCodeVo vo)
        {
            InitializeComponent();


            this.orgVo = vo;
            SYSTEM_CODE_VO();

            SystemCodeVo copyVo = new SystemCodeVo()
            {
                CAR_NO           = vo.CAR_NO,
                CAR_NM           = vo.CAR_NM,
                CAR_OWN_CLSS     = vo.CAR_OWN_CLSS,
                REP_CAR_NO       = vo.REP_CAR_NO,
                CAR_MAK_CD       = vo.CAR_MAK_CD,
                CAR_CLSS_CD      = vo.CAR_CLSS_CD,
                STK_WGT          = vo.STK_WGT,
                BIZ_CLSS_CD      = vo.BIZ_CLSS_CD,
                TRNS_MAN_NM      = vo.TRNS_MAN_NM,
                TRNS_PHN_NO      = vo.TRNS_PHN_NO,
                TRNS_LIC_NO      = vo.TRNS_LIC_NO,
                TRNS_MAN_ADDR    = vo.TRNS_MAN_ADDR,
                TRNS_BIZ_LIC     = vo.TRNS_BIZ_LIC,
                CHNL_CD          = SystemProperties.USER_VO.CHNL_CD,
                TRNS_RMK         = vo.TRNS_RMK,
                CO_NO            = vo.CO_NO,
                DELT_FLG         = vo.DELT_FLG,
                CRE_USR_ID       = vo.CRE_USR_ID,
                UPD_USR_ID       = vo.UPD_USR_ID,
            };

            if(vo.CAR_NO != null)
            {
                this.isEdit = true;
                this.text_CAR_NO.IsReadOnly = true;
                this.text_CAR_NO.Background = Brushes.DarkGray;
            }
            else
            {
                this.isEdit = false;
                copyVo.DELT_FLG = "사용";
            }                                                    // CAR_NO값이 있으면 수정 , 없으면 추가
            this.DataContext = copyVo;
            this.OKButton.Click += new RoutedEventHandler(OKButton_Click);
            this.CancelButton.Click += new RoutedEventHandler(CancelButton_Click);
        }
        #region 확인 버튼, 취소 버튼
        private async void OKButton_Click(object sender, RoutedEventArgs e)
        {
            int _Num = 0;
            if (ValueCheckd())
            {
            this.updateVo = getDomain();
                if (isEdit == false)
                {
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s1431/mst/i", new StringContent(JsonConvert.SerializeObject(this.updateVo), System.Text.Encoding.UTF8, "application/json")))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string result = await response.Content.ReadAsStringAsync();
                            if (int.TryParse(result, out _Num) == false)
                            {
                                WinUIMessageBox.Show(result, title, MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                            WinUIMessageBox.Show("완료 되었습니다.", title, MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }
                else
                {
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s1431/mst/u", new StringContent(JsonConvert.SerializeObject(this.updateVo), System.Text.Encoding.UTF8, "application/json")))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string result = await response.Content.ReadAsStringAsync();
                            if (int.TryParse(result, out _Num) == false)
                            {
                                WinUIMessageBox.Show(result, title, MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                            WinUIMessageBox.Show("완료 되었습니다.", title, MessageBoxButton.OK, MessageBoxImage.Information);
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
        #endregion

        #region 화면에있는 데이터 가져오는 함수
        private SystemCodeVo getDomain()
        {
            SystemCodeVo vo = new SystemCodeVo();

            SystemCodeVo OWN_CLSS_vo = this.text_CAR_OWN_CLSS.SelectedItem as SystemCodeVo;     
            SystemCodeVo CAR_MAK_vo = this.text_CAR_MAK_CD.SelectedItem as SystemCodeVo;
            SystemCodeVo CAR_CLSS_vo = this.text_CAR_CLSS_CD.SelectedItem as SystemCodeVo;
            SystemCodeVo CO_NO_vo = this.text_CO_NO.SelectedItem as SystemCodeVo;



            vo.CAR_NO = this.text_CAR_NO.Text;
            vo.CAR_NM = this.text_CAR_NM.Text;
            vo.REP_CAR_NO = this.text_REP_CAR_NO.Text;
            vo.STK_WGT = Int32.Parse(this.text_STK_WGT.Text);
            vo.BIZ_CLSS_CD = this.text_BIZ_CLSS_CD.Text;

            vo.TRNS_RMK = this.text_TRNS_RMK.Text;

            vo.TRNS_MAN_NM = this.text_TRNS_MAN_MN.Text;
            vo.TRNS_PHN_NO = this.text_TRNS_PHN_NO.Text;
            vo.TRNS_LIC_NO = this.text_TRNS_LIC_NO.Text;
            vo.TRNS_BIZ_LIC = this.text_TRNS_BIZ_LIC.Text;
            vo.TRNS_MAN_ADDR = this.text_TRNS_MAN_ADDR.Text;



            //소유 구분
            if (OWN_CLSS_vo != null)
            {
                vo.CAR_OWN_CLSS = OWN_CLSS_vo.CLSS_CD;
            }
            //메이커

            if (CAR_MAK_vo != null)
            {
                vo.CAR_MAK_CD = CAR_MAK_vo.CLSS_CD;
            }

            // 차량 구분
            if (CAR_CLSS_vo != null)
            {
                vo.CAR_CLSS_CD = CAR_CLSS_vo.CLSS_CD;
            }

            // 거래처
            if(CO_NO_vo != null)
            {
                vo.CO_NO = CO_NO_vo.CO_NO;
            }

            vo.DELT_FLG = (this.combo_deltFlg.Text.Equals("사용") ? "N" : "Y");
            vo.CRE_USR_ID = SystemProperties.USER;
            vo.UPD_USR_ID = SystemProperties.USER;
            vo.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
            vo.CRE_DT = DateTime.Now;
            vo.UPD_DT = DateTime.Now;

            return vo;
        }
        #endregion


        #region SYSTEM_CODE_VO
        public async void SYSTEM_CODE_VO()
        {
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + AquilaErpWpfApp3.Properties.Settings.Default.SettingChnl + "/" + "V-001"))
            {
                if (response.IsSuccessStatusCode)
                {
                    text_CAR_OWN_CLSS.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                }
            }
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + AquilaErpWpfApp3.Properties.Settings.Default.SettingChnl + "/" + "V-002"))
            {
                if (response.IsSuccessStatusCode)
                {
                    text_CAR_MAK_CD.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                }
            }
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + AquilaErpWpfApp3.Properties.Settings.Default.SettingChnl + "/" + "V-003"))
            {
                if (response.IsSuccessStatusCode)
                {
                    text_CAR_CLSS_CD.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                }
            }

            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s143", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { DELT_FLG = "N", CO_TP_CD = "AR", AREA_CD = SystemProperties.USER_VO.EMPE_PLC_NM, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.text_CO_NO.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                }
            }
        }
        #endregion

        #region Functon (ValueCheckd)
        public Boolean ValueCheckd()
        {
            if (string.IsNullOrEmpty(this.text_CAR_NO.Text))
            {
                WinUIMessageBox.Show("[차량 번호] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.text_CAR_NO.IsTabStop = true;
                this.text_CAR_NO.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(this.text_STK_WGT.Text))
            {
                this.text_STK_WGT.Text = "0";
                //WinUIMessageBox.Show("[적재 중량] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                //this.text_STK_WGT.IsTabStop = true;
                //this.text_STK_WGT.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(this.text_CAR_OWN_CLSS.Text))
            {
                WinUIMessageBox.Show("[소유 구분] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.text_CAR_OWN_CLSS.IsTabStop = true;
                this.text_CAR_OWN_CLSS.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(this.text_CAR_MAK_CD.Text))
            {
                WinUIMessageBox.Show("[메이커] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.text_CAR_MAK_CD.IsTabStop = true;
                this.text_CAR_MAK_CD.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(this.text_CAR_CLSS_CD.Text))
            {
                WinUIMessageBox.Show("[차량 구분] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.text_CAR_CLSS_CD.IsTabStop = true;
                this.text_CAR_CLSS_CD.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(this.text_CO_NO.Text))
            {
                WinUIMessageBox.Show("[거래처] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.text_CO_NO.IsTabStop = true;
                this.text_CO_NO.Focus();
                return false;
            }
            return true;
        }
        #endregion
    }
}
