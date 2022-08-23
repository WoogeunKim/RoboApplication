using AquilaErpWpfApp3.Util;
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

namespace AquilaErpWpfApp3.S.View.Dialog
{
    /// <summary>
    /// Interaction logic for S1147MasterDialog.xaml
    /// </summary>
    public partial class S14130MasterDialog : DXWindow
    {
        //private static SaleOrderServiceClient saleOrderClient = SystemProperties.SaleOrderClient;
        private SystemCodeVo orgDao;
        private bool isEdit = false;
        private SystemCodeVo updateDao;
        private string title = "완제품코드등록";

        public S14130MasterDialog(SystemCodeVo Dao)
        {
            InitializeComponent();

            //this.combo_AREA_NM.ItemsSource = SystemProperties.SYSTEM_CODE_VO("L-002");
            SYSTEM_CODE_VO();

            this.orgDao = Dao;

            SystemCodeVo copyDao = new SystemCodeVo()
            {
                AREA_CD = Dao.AREA_CD,
                AREA_NM = Dao.AREA_NM,
                CO_NO = Dao.CO_NO,
                CO_NM = Dao.CO_NM,
                CAR_ITM_NM = Dao.CAR_ITM_NM,
                MDL_NM = Dao.MDL_NM,
                ITM_CD = Dao.ITM_CD,
                ITM_NM = Dao.ITM_NM,
                N1ST_ITM_GRP_CD = Dao.N1ST_ITM_GRP_CD,
                N2ND_SUB_ITM_NM = Dao.N2ND_SUB_ITM_NM,
                ITM_SZ_NM = Dao.ITM_SZ_NM,
                ITM_TMP_NM = Dao.ITM_TMP_NM,
                UOM_NM = Dao.UOM_NM,
                BRND_NM = Dao.BRND_NM,
                CRE_DT = Dao.CRE_DT,
                ITM_DESC = Dao.ITM_DESC,
                CHNL_CD = Dao.CHNL_CD,
                CRE_USR_ID = Dao.CRE_USR_ID,
                UPD_USR_ID = Dao.UPD_USR_ID,
                CUST_ITM_CD = Dao.CUST_ITM_CD,
                CUST_ITM_NM = Dao.CUST_ITM_NM,
                CUST_ITM_RMK = Dao.CUST_ITM_RMK
            };

            if (Dao.ITM_CD != null)
            {
                this.isEdit = true;
            }
            else
            {
                //추가
                this.isEdit = false;
            }

            this.configCode.DataContext = copyDao;
            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
            this.OKButton.Click += new RoutedEventHandler(OKButton_Click);
            this.CancelButton.Click += new RoutedEventHandler(CancelButton_Click);
        }

        #region Functon (ValueCheckd)
        public Boolean ValueCheckd()
        {
            if (string.IsNullOrEmpty(this.combo_CO_NO.Text))
            {
                WinUIMessageBox.Show("[매출처] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.combo_CO_NO.IsTabStop = true;
                this.combo_CO_NO.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(this.combo_CAR_TP_NM.Text))
            {
                WinUIMessageBox.Show("[모델] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.combo_CAR_TP_NM.IsTabStop = true;
                this.combo_CAR_TP_NM.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(this.combo_N2ND_SUB_ITM_NM.Text))
            {
                WinUIMessageBox.Show("[출고구분] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.combo_N2ND_SUB_ITM_NM.IsTabStop = true;
                this.combo_N2ND_SUB_ITM_NM.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(this.combo_ITM_GRP_NM.Text))
            {
                WinUIMessageBox.Show("[브랜드] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.combo_ITM_GRP_NM.IsTabStop = true;
                this.combo_ITM_GRP_NM.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(this.text_ITM_TMP_NM.Text))
            {
                WinUIMessageBox.Show("[임시 제품명] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.text_ITM_TMP_NM.IsTabStop = true;
                this.text_ITM_TMP_NM.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(this.text_ITM_SZ_NM.Text))
            {
                WinUIMessageBox.Show("[규격/홋수] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.text_ITM_SZ_NM.IsTabStop = true;
                this.text_ITM_SZ_NM.Focus();
                return false;
            }
            //else
            {
                //if (this.isEdit == false)
                //{
                //    SaleVo dao = new SaleVo();
                //    //CustomerCodeDao coNmVo = this.combo_CLT_CO_NM.SelectedItem as CustomerCodeDao;
                //    //if (coNmVo != null)
                //    //{
                //    //    dao.CO_CD = coNmVo.CO_NO;
                //    //    dao.CO_NM = coNmVo.CO_NM;
                //    //}

                //    CodeDao areaNmVo = this.combo_AREA_NM.SelectedItem as CodeDao;
                //    if (areaNmVo != null)
                //    {
                //        dao.AREA_CD = areaNmVo.CLSS_CD;
                //        dao.AREA_NM = areaNmVo.CLSS_DESC;
                //    }

                //    //JobVo daoList = (JobVo)saleOrderClient.S2219SelectCheck(dao);
                //    //if (daoList != null)
                //    //{
                //    //    WinUIMessageBox.Show("[코드 - 중복] 코드를 다시 입력 하십시오.", "[중복검사]" + this.title, MessageBoxButton.OK, MessageBoxImage.Warning);
                //    //    this.combo_AREA_NM.IsTabStop = true;
                //    //    this.combo_AREA_NM.Focus();
                //    //    return false;
                //    //}
                //}
            }
            return true;
        }
        #endregion

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

                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s14130/i", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
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

                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s14130/u", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
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

        #region Functon (getDomain - ConfigView1Dao)
        private SystemCodeVo getDomain()
        {
            SystemCodeVo Dao = new SystemCodeVo();

            //매출처
            SystemCodeVo _coNo = this.combo_CO_NO.SelectedItem as SystemCodeVo;
            if (_coNo != null)
            {
                Dao.CO_NO = _coNo.CO_NO;
                Dao.CO_NM = _coNo.CO_NM;
            }

            //모델
            SaleVo _carTpNm = this.combo_CAR_TP_NM.SelectedItem as SaleVo;
            if (_carTpNm != null)
            {
                Dao.CAR_TP_CD = _carTpNm.CAR_TP_CD;
                Dao.MDL_NM = _carTpNm.CAR_TP_NM;
            }

            //출고구분
            SystemCodeVo _n2ndSubItmNm = this.combo_N2ND_SUB_ITM_NM.SelectedItem as SystemCodeVo;
            if (_n2ndSubItmNm != null)
            {
                Dao.N2ND_SUB_ITM_CD = _n2ndSubItmNm.CLSS_CD;
                Dao.N2ND_SUB_ITM_NM = _n2ndSubItmNm.CLSS_DESC;
            }

            //브랜드
            SystemCodeVo _itmGrpNm = this.combo_ITM_GRP_NM.SelectedItem as SystemCodeVo;
            if (_itmGrpNm != null)
            {

                Dao.N1ST_ITM_GRP_CD = _itmGrpNm.ITM_GRP_CD;
                Dao.N1ST_ITM_GRP_NM = _itmGrpNm.ITM_GRP_NM;

            }

            Dao.ITM_TMP_NM = this.text_ITM_TMP_NM.Text;
            Dao.ITM_SZ_NM = this.text_ITM_SZ_NM.Text;
            Dao.ITM_DESC = this.text_ITM_DESC.Text;

            //
            Dao.CUST_ITM_CD = this.text_CUST_ITM_CD.Text;
            Dao.CUST_ITM_NM = this.text_CUST_ITM_NM.Text;
            Dao.CUST_ITM_RMK = this.text_CUST_ITM_RMK.Text;

            ////단위
            //SystemCodeVo _uomNm = this.combo_UOM_NM.SelectedItem as SystemCodeVo;
            //if (_uomNm != null)
            //{
            //    Dao.UOM_CD = _uomNm.CLSS_CD;
            //    Dao.UOM_NM = _uomNm.CLSS_DESC;
            //}

            Dao.ITM_CD = this.orgDao.ITM_CD;
            Dao.CRE_USR_ID = SystemProperties.USER;
            Dao.UPD_USR_ID = SystemProperties.USER;
            Dao.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;

            return Dao;
        }
        #endregion


        public async void SYSTEM_CODE_VO()
        {
            //출고 구분
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "L-800"))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.combo_N2ND_SUB_ITM_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                }
            }

            //단위
            //using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "L-003"))
            //{
            //    if (response.IsSuccessStatusCode)
            //    {
            //        this.combo_UOM_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
            //    }
            //}

            //매출처
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s143", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { DELT_FLG = "N", SEEK = "", CO_TP_CD = "AR", SEEK_AR = "AR", AREA_CD = SystemProperties.USER_VO.EMPE_PLC_NM, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.combo_CO_NO.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                }
            }

            //모델
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s11410", new StringContent(JsonConvert.SerializeObject(new SaleVo() { DELT_FLG = "N", CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.combo_CAR_TP_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SaleVo>>(await response.Content.ReadAsStringAsync()).Cast<SaleVo>().ToList();
                }
            }

            //브랜드
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s133", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { ITM_GRP_CLSS_CD = "G", CRE_USR_ID = "1", CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.combo_ITM_GRP_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
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
                return this.updateDao;
            }
        }
    }
}
