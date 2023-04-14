using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Auth;
using ModelsLibrary.Code;
using ModelsLibrary.Inv;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;
using AquilaErpWpfApp3.Util;

namespace AquilaErpWpfApp3.View.INV.Dialog
{
    public partial class I6611DetailOtherCaseDialog : DXWindow
    {
        private string _title = "기타 입고";

        private List<InvVo> updateDao = new List<InvVo>();

        public I6611DetailOtherCaseDialog()
        {
            InitializeComponent();


            this.text_INAUD_DT.Text = DateTime.Now.ToString("yyyy-MM-dd");


            SYSTEM_CODE_VO();

            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
            this.OKButton.Click += new RoutedEventHandler(OKButton_Click);
            this.CancelButton.Click += new RoutedEventHandler(CancelButton_Click);

            this.combo_N1ST_ITM_GRP_NM.SelectedIndexChanged += new RoutedEventHandler(N1stItmGrpNm_Changed);
            this.Refrush_Button.Click += new RoutedEventHandler(RefrushButton_Click);
        }

        private void N1stItmGrpNm_Changed(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.combo_N1ST_ITM_GRP_NM.SelectedItem == null) return;

                // 최기화
                this.combo_N2ND_ITM_GRP_NM.ItemsSource = null;
                this.combo_N2ND_ITM_GRP_NM.SelectedItem = null;
                this.text_ITM_LEN.Text = "";
                this.text_ITM_CD.Text = "";

                var N1stItmCd = (this.combo_N1ST_ITM_GRP_NM.SelectedItem as SystemCodeVo).ITM_GRP_CD;

                //조회
                this.combo_N2ND_ITM_GRP_NM.ItemsSource = this.N2ndGrpList.Where(x => x.PRNT_ITM_GRP_CD.Equals(N1stItmCd)).ToList<SystemCodeVo>();
            }
            catch (Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, _title, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private async void RefrushButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.combo_N1ST_ITM_GRP_NM.SelectedItem == null) return;
                if (this.combo_N2ND_ITM_GRP_NM.SelectedItem == null) return;
                int _Num = 0;
                if (int.TryParse(this.text_ITM_LEN.Text, out _Num) == false)
                {
                    WinUIMessageBox.Show("길이를 다시 입력하세요.", "[유효검사]", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var n1stNm = (this.combo_N1ST_ITM_GRP_NM.SelectedItem as SystemCodeVo).ITM_GRP_CD;
                var n2ndNm = (this.combo_N2ND_ITM_GRP_NM.SelectedItem as SystemCodeVo).ITM_GRP_CD;
                var itmLen = int.Parse(this.text_ITM_LEN.Text);

                var obj = new SystemCodeVo()
                {
                    N1ST_ITM_GRP_CD = (this.combo_N1ST_ITM_GRP_NM.SelectedItem as SystemCodeVo).ITM_GRP_CD
                    ,
                    N2ND_ITM_GRP_CD = (this.combo_N2ND_ITM_GRP_NM.SelectedItem as SystemCodeVo).ITM_GRP_CD
                    ,
                    ITM_LEN = int.Parse(this.text_ITM_LEN.Text)
                   ,
                    DELT_FLG = "N"
                   ,
                    ITM_GRP_CLSS_CD = "M"
                   ,
                    CHNL_CD = SystemProperties.USER_VO.CHNL_CD
                };

                var itmList = new List<SystemCodeVo>();

                ///품명
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s141/robo", new StringContent(JsonConvert.SerializeObject(obj), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        itmList = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                    }
                }

                if (itmList.Count <= 0)
                {
                    // 품목마스터 추가
                    MessageBoxResult result = WinUIMessageBox.Show("길이 : " + int.Parse(this.text_ITM_LEN.Text).ToString() + " 등록하겠습니까?", "[신규 품목 등록]" + _title, MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        //itm/i
                        var InvitmDao = new InvVo()
                        {
                            N1ST_ITM_GRP_CD = obj.N1ST_ITM_GRP_CD,
                            N2ND_ITM_GRP_CD = obj.N2ND_ITM_GRP_CD,
                            ITM_LEN = obj.ITM_LEN,
                            ITM_GRP_CLSS_CD = obj.ITM_GRP_CLSS_CD,
                            CHNL_CD = obj.CHNL_CD,
                            CRE_USR_ID = SystemProperties.USER,
                            UPD_USR_ID = SystemProperties.USER
                        };


                        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("i6611/itm/i", new StringContent(JsonConvert.SerializeObject(InvitmDao), System.Text.Encoding.UTF8, "application/json")))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                string itmResult = await response.Content.ReadAsStringAsync();
                                if (int.TryParse(itmResult, out _Num) == false)
                                {
                                    //실패
                                    WinUIMessageBox.Show(itmResult, _title, MessageBoxButton.OK, MessageBoxImage.Error);
                                    return;
                                }

                                //성공
                                WinUIMessageBox.Show("등록 되었습니다", _title, MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                        }

                        /// 다시 조회
                        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s141/robo", new StringContent(JsonConvert.SerializeObject(obj), System.Text.Encoding.UTF8, "application/json")))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                itmList = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                            }
                        }
                    }
                    else
                    {
                        return;
                    }
                }

                // 등록하기
                if(itmList.Count > 0)
                {
                    this.text_ITM_CD.Text = itmList[0].ITM_CD;
                }
            }
            catch (Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, _title, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }


        #region Functon (OKButton_Click, CancelButton_Click)
        private async  void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValueCheckd())
            {
                int _Num = 0;

                this.updateDao = getDomain();
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("i6611/mst/i", new StringContent(JsonConvert.SerializeObject(updateDao), System.Text.Encoding.UTF8, "application/json")))
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
            if (string.IsNullOrEmpty(this.combo_N1ST_ITM_GRP_NM.Text))
            {
                WinUIMessageBox.Show("[강종] 입력 값이 맞지 않습니다.", "[유효검사]품목 입고 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
                this.combo_N1ST_ITM_GRP_NM.IsTabStop = true;
                this.combo_N1ST_ITM_GRP_NM.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(this.combo_N2ND_ITM_GRP_NM.Text))
            {
                WinUIMessageBox.Show("[규격] 입력 값이 맞지 않습니다.", "[유효검사]품목 입고 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
                this.combo_N2ND_ITM_GRP_NM.IsTabStop = true;
                this.combo_N2ND_ITM_GRP_NM.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(this.text_ITM_LEN.Text))
            {
                WinUIMessageBox.Show("[길이] 입력 값이 맞지 않습니다.", "[유효검사]품목 입고 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
                this.text_ITM_LEN.IsTabStop = true;
                this.text_ITM_LEN.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(this.text_ITM_CD.Text))
            {
                WinUIMessageBox.Show("[품목] 입력 값이 맞지 않습니다.", "[유효검사]품목 입고 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
                this.text_ITM_CD.IsTabStop = true;
                this.text_ITM_CD.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(this.combo_INAUD_NM.Text))
            {
                WinUIMessageBox.Show("[수불유형] 입력 값이 맞지 않습니다.", "[유효검사]품목 입고 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
                this.combo_INAUD_NM.IsTabStop = true;
                this.combo_INAUD_NM.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(this.combo_LOC_NM.Text))
            {
                WinUIMessageBox.Show("[창고] 입력 값이 맞지 않습니다.", "[유효검사]품목 입고 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
                this.combo_LOC_NM.IsTabStop = true;
                this.combo_LOC_NM.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(this.text_INAUD_DT.Text))
            {
                WinUIMessageBox.Show("[입고일자] 입력 값이 맞지 않습니다.", "[유효검사]품목 입고 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
                this.text_INAUD_DT.IsTabStop = true;
                this.text_INAUD_DT.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(this.text_ITM_QTY.Text))
            {
                WinUIMessageBox.Show("[본수] 입력 값이 맞지 않습니다.", "[유효검사]품목 입고 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
                this.text_ITM_QTY.IsTabStop = true;
                this.text_ITM_QTY.Focus();
                return false;
            }
            else if (Convert.ToDouble(this.text_ITM_QTY.Text) <= 0)
            {
                WinUIMessageBox.Show("[본수] 입력 값이 맞지 않습니다.", "[유효검사]품목 입고 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
                this.text_ITM_QTY.IsTabStop = true;
                this.text_ITM_QTY.Focus();
                return false;
            }

            return true;
        }
        #endregion

        #region Functon (getDomain - ConfigView1Dao)
        private List<InvVo> getDomain()
        {
            InvVo Dao = new InvVo();

            Dao.ITM_CD = this.text_ITM_CD.Text;
            Dao.INAUD_DT = Convert.ToDateTime(this.text_INAUD_DT.Text).ToString("yyyy-MM-dd");

            SystemCodeVo locVo = this.combo_LOC_NM.SelectedItem as SystemCodeVo;
            Dao.LOC_CD = locVo.CLSS_CD;
            Dao.LOC_NO = locVo.CLSS_CD;
            Dao.LOC_NM = locVo.CLSS_DESC;


            SystemCodeVo inaudVo = this.combo_INAUD_NM.SelectedItem as SystemCodeVo;
            Dao.INAUD_CD = inaudVo.CLSS_CD;
            Dao.INAUD_NM = inaudVo.CLSS_DESC;


            Dao.BAR_QTY = int.Parse(this.text_ITM_QTY.Text);


            Dao.AREA_CD = "002";
            Dao.CO_CD = "999";
            Dao.ITM_QTY = 1;

            Dao.CRE_USR_ID = SystemProperties.USER;
            Dao.UPD_USR_ID = SystemProperties.USER;
            Dao.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;


            var DaoList = new List<InvVo>();
            DaoList.Add(Dao);

            return DaoList;
        }
        #endregion


        public async void SYSTEM_CODE_VO()
        {
            try
            {
                this.N2ndGrpList = new List<SystemCodeVo>();

                // 강종
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s133", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { DELT_FLG = "N", CRE_USR_ID = "", ITM_GRP_CLSS_CD = "M", CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.combo_N1ST_ITM_GRP_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                    }
                }

                // 규격
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s133", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { DELT_FLG = "N", CRE_USR_ID = "", ITM_GRP_CLSS_CD = "M", PRNT_ITM_GRP_CD = "", CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.N2ndGrpList = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                    }
                }

                //this.combo_INAUD_NM.ItemsSource = SystemProperties.SYSTEM_CODE_VO("L-007");
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "L-007"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.combo_INAUD_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Where(x=>x.CLSS_CD.Substring(0,1).Equals("R") && x.CLSS_CD != "RGU").Cast<SystemCodeVo>().ToList();
                    }
                }

                //this.combo_LOC_NM.ItemsSource = SystemProperties.SYSTEM_CODE_VO("P-008");
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "P-008"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.combo_LOC_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                    }
                }
            }
            catch (Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, _title, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        public List<SystemCodeVo> N2ndGrpList
        {
            get;
            set;
        }


        public string INSRL_NO
        { 
            get; 
            set;
        }

    }
}
