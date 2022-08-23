using AquilaErpWpfApp3.Util;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Tec;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;

namespace AquilaErpWpfApp3.View.TEC.Dialog
{
    /// <summary>
    /// Interaction logic for ConfigView1Dialog.xaml
    /// </summary>
    public partial class T91101MasterDialog : DXWindow
    {
        //private static CodeServiceClient codeClient = SystemProperties.CodeClient;
        //private SystemCodeVo orgDao;
        //private bool isEdit = false;
        //private SystemCodeVo updateDao;
        private IList<TecVo> ChekList;
        private string _title = "부자재 품질검사";

        private IList<TecVo> _badList = new List<TecVo>();

        public T91101MasterDialog(IList<TecVo> _chekList)
        {
            InitializeComponent();
            //
            //this.orgDao = Dao;
            //SystemCodeVo copyDao = new SystemCodeVo()
            //{
            //    CLSS_TP_CD = Dao.CLSS_TP_CD,
            //    CLSS_TP_NM = Dao.CLSS_TP_NM,
            //    SYS_FLG = Dao.SYS_FLG,
            //    SYS_AREA_CD = Dao.SYS_AREA_CD,
            //    CLSS_CD_DESC = Dao.CLSS_CD_DESC,
            //    DELT_FLG = Dao.DELT_FLG,
            //    CRE_USR_ID = Dao.CRE_USR_ID,
            //    UPD_USR_ID = Dao.UPD_USR_ID,
            //    CHNL_CD = Dao.CHNL_CD
            //};

            this.ChekList = _chekList;


            ////수정
            //if (copyDao.CLSS_TP_CD != null)
            //{
            //    this.text_ClssTpCd.IsReadOnly = true;
            //    this.isEdit = true;
            //}
            //else
            //{
            //    //추가
            //    //this.text_ClssTpCd.IsReadOnly = false;
            //    //this.isEdit = false;
            //    copyDao.DELT_FLG = "사용";
            //}
            //
            this.text_ITM_QTY.Text = this.ChekList.Sum<TecVo>(x => Convert.ToInt32(x.ITM_QTY)).ToString();

            SYSTEM_CODE_VO();

            //판정일자 공백이면
            if (string.IsNullOrEmpty(_chekList[0].INSP_DT))
            {
                _chekList[0].INSP_DT = System.DateTime.Now.ToString("yyyy-MM-dd");
            }

            //_chekList[0].GBN = "적합";

            this.configCode.DataContext = _chekList[0];
            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
            this.OKButton.Click += new RoutedEventHandler(OKButton_Click);
            this.CancelButton.Click += new RoutedEventHandler(CancelButton_Click);



            //button
            this.ADDButton.Click += ADDButton_Click;
            this.DELButton.Click += DELButton_Click;
            //
            this.ApplyButton.Click += ApplyButton_Click;
        }

        private async void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //검사 불려오기
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("t91100/popup/ok", new StringContent(JsonConvert.SerializeObject(new TecVo() { DELT_FLG = "N", AREA_CD = SystemProperties.USER_VO.EMPE_PLC_NM, CHNL_CD = SystemProperties.USER_VO.CHNL_CD, ITM_QTY = this.text_ITM_QTY.Text, BAD_QTY =  this._badList.Sum<TecVo>(x => Convert.ToInt32(x.BAD_QTY)) }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        TecVo _inspVo = JsonConvert.DeserializeObject<TecVo>(await response.Content.ReadAsStringAsync());
                        if (_inspVo != null)
                        {
                            this.text_INSP_QTY.Text = _inspVo.INSP_QTY.ToString();
                            this.combo_INSP_FLG.Text = (_inspVo.INSP_FLG.Equals("Y") ? "적합" : (_inspVo.INSP_FLG.Equals("N") ? "부적합" : "시험중") );
                            //this.lue_BAD_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<TecVo>>(await response.Content.ReadAsStringAsync()).Cast<TecVo>().ToList();
                        }
                    }
                }
            }
            catch (Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, _title, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

        }

        private void ADDButton_Click(object sender, RoutedEventArgs e)
        {
            this._badList.Add(new TecVo() { BAD_QTY = 0, GRP_SER_NO = this.ChekList[0].GRP_SER_NO, ROUT_CD = this.ChekList[0].ROUT_CD, CRE_USR_ID = SystemProperties.USER_VO.USR_ID, UPD_USR_ID = SystemProperties.USER_VO.USR_ID });
            this.ConfigViewPage1Edit_Popup.ItemsSource = this._badList;
            this.ConfigViewPage1Edit_Popup.RefreshData();
            if (this._badList.Count > 0)
            {
                this.DELButton.IsEnabled = true;
            }
        }

        private void DELButton_Click(object sender, RoutedEventArgs e)
        {
            this._badList.Remove(this.ConfigViewPage1Edit_Popup.SelectedItem as TecVo);
            this.ConfigViewPage1Edit_Popup.RefreshData();
            if (this._badList.Count > 0)
            {
                this.DELButton.IsEnabled = true;
            }
            else
            {
                this.DELButton.IsEnabled = false;
            }
        }

        #region Functon (OKButton_Click, CancelButton_Click)
        private async void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValueCheckd())
            {
                int _Num = 0;
                ////SystemCodeVo resultVo;
                //if (isEdit == false)
                //{
                //    this.updateDao = getDomain();

                //    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s131/mst/i", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
                //    {
                //        if (response.IsSuccessStatusCode)
                //        {
                //            string result = await response.Content.ReadAsStringAsync();
                //            if (int.TryParse(result, out _Num) == false)
                //            {
                //                //실패
                //                WinUIMessageBox.Show(result, _title, MessageBoxButton.OK, MessageBoxImage.Error);
                //                return;
                //            }

                //            //성공
                //            WinUIMessageBox.Show("완료 되었습니다", _title, MessageBoxButton.OK, MessageBoxImage.Information);
                //        }
                //    }
                //}
                //else
                //{
                //this.updateDao = getDomain();

                // SystemCodeVo _badNm = this.combo_BAD_NM.SelectedItem as SystemCodeVo;

                foreach (TecVo item in this.ChekList)
                {
                    //item.MTRL_MAKE_DT = Convert.ToDateTime(this.text_MTRL_MAKE_DT.Text).ToString("yyyy-MM-dd");
                    //item.MTRL_EXP_DT = Convert.ToDateTime(this.text_MTRL_EXP_DT.).ToString("yyyy-MM-dd");
                    //Dao.ITM_IN_DT = Convert.ToDateTime(ITM_IN_DT).ToString("yyyy-MM-dd");

                    //item.INSP_NO = this.text_INSP_NO.Text;

                    //Dao.INSP_NO = INSP_NO;
                    //item.MTRL_EXP_DY = this.text_MTRL_EXP_DY;

                    //item.INSP_DT = Convert.ToDateTime(this.text_INSP_DT.Text).ToString("yyyy-MM-dd");
                    //item.INSP_FLG = this.combo_INSP_FLG.Text;

                    if (string.IsNullOrEmpty(this.text_INSP_DT.Text))
                    {
                        item.INSP_DT = null;
                    }
                    else
                    {
                        item.INSP_DT = Convert.ToDateTime(this.text_INSP_DT.Text).ToString("yyyy-MM-dd");
                    }

                    item.INSP_FLG = (this.combo_INSP_FLG.Text.Equals("시험중") ? "Z" : (this.combo_INSP_FLG.Text.Equals("적합") ? "Y" : "N"));
                    item.ITM_RMK = this.text_ITM_RMK.Text;

                    item.INSP_QTY = this.text_INSP_QTY.Text;
                    item.ITM_QTY = this.text_ITM_QTY.Text;

                    //if (_badNm != null)
                    //{
                    //    item.BAD_CD = _badNm.CLSS_CD;
                    //    item.BAD_NM = _badNm.CLSS_DESC;
                    //}

                    //item.BAD_QTY = this.text_BAD_QTY.Text;

                    //item.INAUD_TMP_NO = INAUD_TMP_NO;
                    //item.INAUD_TMP_SEQ = INAUD_TMP_SEQ;
                    item.CRE_USR_ID = SystemProperties.USER_VO.USR_ID;
                    item.UPD_USR_ID = SystemProperties.USER_VO.USR_ID;
                    item.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
                }

                //
                //
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("t91101/m", new StringContent(JsonConvert.SerializeObject(this.ChekList), System.Text.Encoding.UTF8, "application/json")))
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


                        if (this._badList.Count > 0)
                        {
                            //
                            //불량 유형 (삭제 -> 저장)
                            using (HttpResponseMessage response_Y = await SystemProperties.PROGRAM_HTTP.PostAsync("t91101/bad/m", new StringContent(JsonConvert.SerializeObject(this._badList), System.Text.Encoding.UTF8, "application/json")))
                            {
                                if (response_Y.IsSuccessStatusCode)
                                {
                                    result = await response_Y.Content.ReadAsStringAsync();
                                    if (int.TryParse(result, out _Num) == false)
                                    {
                                        //실패
                                        WinUIMessageBox.Show(result, _title, MessageBoxButton.OK, MessageBoxImage.Error);
                                        return;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (this._badList.Count == 0)
                            {
                                using (HttpResponseMessage response_Y = await SystemProperties.PROGRAM_HTTP.PostAsync("t91101/bad/d", new StringContent(JsonConvert.SerializeObject(new TecVo() { CHNL_CD = this.ChekList[0].CHNL_CD, GRP_SER_NO = this.ChekList[0].GRP_SER_NO, SCM_PUR_ITM_CD = this.ChekList[0].SCM_PUR_ITM_CD }), System.Text.Encoding.UTF8, "application/json")))
                                {
                                    if (response_Y.IsSuccessStatusCode)
                                    {
                                        result = await response_Y.Content.ReadAsStringAsync();
                                        if (int.TryParse(result, out _Num) == false)
                                        {
                                            //실패
                                            WinUIMessageBox.Show(result, _title, MessageBoxButton.OK, MessageBoxImage.Error);
                                            return;
                                        }
                                    }
                                }
                            }

                        }

                        //성공
                        WinUIMessageBox.Show("완료 되었습니다", _title, MessageBoxButton.OK, MessageBoxImage.Information);
                        this.DialogResult = true;
                        this.Close();
                    }
                }
                //
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
            //if (string.IsNullOrEmpty(this.text_INSP_NO.Text))
            //{
            //    //UOM_CD = new CodeDao() { CLSS_DESC = ITM_NM.UOM_NM };
            //    //MTRL_MAKE_DT = Convert.ToDateTime(MTRL_MAKE_DT).AddDays(MTRL_EXP_DY).ToString("yyyy-MM-dd");
            //    WinUIMessageBox.Show("[시험번호] 입력 값이 맞지 않습니다", "[유효검사] " + this._title, MessageBoxButton.OK, MessageBoxImage.Warning);
            //    //MSG = "[배치코드] 입력 값이 맞지 않습니다";
            //    return false;
            //}

            //if (string.IsNullOrEmpty(MTRL_MAKE_DT))
            //{
            //    //UOM_CD = new CodeDao() { CLSS_DESC = ITM_NM.UOM_NM };
            //    //MTRL_MAKE_DT = Convert.ToDateTime(MTRL_MAKE_DT).AddDays(MTRL_EXP_DY).ToString("yyyy-MM-dd");
            //    //WinUIMessageBox.Show("[구분] 입력 값이 맞지 않습니다.", "[유효검사] 시스템 분류 코드", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    MSG = "[제조 일자] 입력 값이 맞지 않습니다";
            //    return false;
            //}


            //if (string.IsNullOrEmpty(MTRL_EXP_DY + ""))
            //{
            //    //UOM_CD = new CodeDao() { CLSS_DESC = ITM_NM.UOM_NM };
            //    //MTRL_MAKE_DT = Convert.ToDateTime(MTRL_MAKE_DT).AddDays(MTRL_EXP_DY).ToString("yyyy-MM-dd");
            //    //WinUIMessageBox.Show("[구분] 입력 값이 맞지 않습니다.", "[유효검사] 시스템 분류 코드", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    MSG = "[유효기간(일수)] 입력 값이 맞지 않습니다";
            //    return false;
            //}
            //else
            //{
            //    MTRL_EXP_DT = Convert.ToDateTime(MTRL_MAKE_DT).AddDays(MTRL_EXP_DY).ToString("yyyy-MM-dd");
            //}



            //if (string.IsNullOrEmpty(INSP_NO))
            //{
            //    //UOM_CD = new CodeDao() { CLSS_DESC = ITM_NM.UOM_NM };
            //    //MTRL_MAKE_DT = Convert.ToDateTime(MTRL_MAKE_DT).AddDays(MTRL_EXP_DY).ToString("yyyy-MM-dd");
            //    //WinUIMessageBox.Show("[구분] 입력 값이 맞지 않습니다.", "[유효검사] 시스템 분류 코드", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    MSG = "[시험 번호] 입력 값이 맞지 않습니다";
            //    return false;
            //}

            if (string.IsNullOrEmpty(this.text_INSP_DT.Text))
            {
                //UOM_CD = new CodeDao() { CLSS_DESC = ITM_NM.UOM_NM };
                //MTRL_MAKE_DT = Convert.ToDateTime(MTRL_MAKE_DT).AddDays(MTRL_EXP_DY).ToString("yyyy-MM-dd");
                WinUIMessageBox.Show("[판정 일자] 입력 값이 맞지 않습니다", "[유효검사] " + this._title, MessageBoxButton.OK, MessageBoxImage.Warning);
                //MSG = "[판정 일자] 입력 값이 맞지 않습니다";
                return false;
            }
            else if (string.IsNullOrEmpty(this.combo_INSP_FLG.Text))
            {
                //UOM_CD = new CodeDao() { CLSS_DESC = ITM_NM.UOM_NM };
                //MTRL_MAKE_DT = Convert.ToDateTime(MTRL_MAKE_DT).AddDays(MTRL_EXP_DY).ToString("yyyy-MM-dd");
                WinUIMessageBox.Show("[판정] 입력 값이 맞지 않습니다", "[유효검사] " + this._title, MessageBoxButton.OK, MessageBoxImage.Warning);
                //MSG = "[판정] 입력 값이 맞지 않습니다";
                return false;
            }


            return true;
        }
        #endregion


        public async void SYSTEM_CODE_VO()
        {
            //using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "Q-002"))
            //{
            //    if (response.IsSuccessStatusCode)
            //    {
            //        this.combo_BAD_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
            //    }
            //}
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("t91103", new StringContent(JsonConvert.SerializeObject(new TecVo() { DELT_FLG = "N", AREA_CD = SystemProperties.USER_VO.EMPE_PLC_NM, CHNL_CD = SystemProperties.USER_VO.CHNL_CD,  ROUT_CD = this.ChekList[0].ROUT_CD }), System.Text.Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.lue_BAD_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<TecVo>>(await response.Content.ReadAsStringAsync()).Cast<TecVo>().ToList();
                }
            }

            //
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("t91101/bad", new StringContent(JsonConvert.SerializeObject(this.ChekList[0]), System.Text.Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    this._badList = JsonConvert.DeserializeObject<IEnumerable<TecVo>>(await response.Content.ReadAsStringAsync()).Cast<TecVo>().ToList();
                    this.ConfigViewPage1Edit_Popup.ItemsSource = this._badList;
                    if (this._badList.Count > 0)
                    {
                        this.DELButton.IsEnabled = true;
                    }

                    //this.ConfigViewPage1Edit_Popup.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<TecVo>>(await response.Content.ReadAsStringAsync()).Cast<TecVo>().ToList();
                }
            }
            //this.combo_ITM_GRP_CLSS_CD.ItemsSource = SystemProperties.SYSTEM_CODE_VO("L-001");
            //using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "L-001"))
            //{
            //    if (response.IsSuccessStatusCode)
            //    {
            //        this.combo_ITM_GRP_CLSS_CD.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
            //        this.combo_ITM_GRP_CLSS_CD.SelectedIndex = 0;
            //    }
            //}
            //btn_ITEMS_Click(null, null);

            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("t91100/popup/ok", new StringContent(JsonConvert.SerializeObject(new TecVo() { DELT_FLG = "N", AREA_CD = SystemProperties.USER_VO.EMPE_PLC_NM, CHNL_CD = SystemProperties.USER_VO.CHNL_CD, ITM_QTY = this.text_ITM_QTY.Text, BAD_QTY = this._badList.Sum<TecVo>(x => Convert.ToInt32(x.BAD_QTY)) }), System.Text.Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    TecVo _inspVo = JsonConvert.DeserializeObject<TecVo>(await response.Content.ReadAsStringAsync());
                    if (_inspVo != null)
                    {
                        this.text_INSP_QTY.Text = _inspVo.INSP_QTY.ToString();
                        this.combo_INSP_FLG.Text = (_inspVo.INSP_FLG.Equals("Y") ? "적합" : (_inspVo.INSP_FLG.Equals("N") ? "부적합" : "시험중"));
                        //this.lue_BAD_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<TecVo>>(await response.Content.ReadAsStringAsync()).Cast<TecVo>().ToList();
                    }
                }
            }

        }

        private void GridColumn_Validate(object sender, DevExpress.Xpf.Grid.GridCellValidationEventArgs e)
        {
            try
            {
                TecVo masterDomain = (TecVo)ConfigViewPage1Edit_Popup.GetFocusedRow();
                bool badQty = (e.Column.FieldName.ToString().Equals("BAD_QTY") ? true : false);
                bool badNm = (e.Column.FieldName.ToString().Equals("BAD_NM") ? true : false);

                if (badQty)
                {
                    if (e.IsValid)
                    {
                        if (string.IsNullOrEmpty(masterDomain.BAD_QTY + ""))
                        {
                            masterDomain.BAD_QTY = 0;
                        }
                        //
                        if (!masterDomain.BAD_QTY.Equals((e.Value == null ? "" : e.Value.ToString())))
                        {
                            masterDomain.BAD_QTY = e.Value.ToString();

                            masterDomain.CRE_USR_ID = SystemProperties.USER_VO.USR_ID;
                            masterDomain.UPD_USR_ID = SystemProperties.USER_VO.USR_ID;
                            masterDomain.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
                            masterDomain.ITM_CD = this.ChekList[0].ITM_CD;

                            masterDomain.isCheckd = true;
                            this.OKButton.IsEnabled = true;
                        }
                    }
                }
                else if (badNm)
                {
                    if (e.IsValid)
                    {
                        if (string.IsNullOrEmpty(masterDomain.BAD_NM))
                        {
                            masterDomain.BAD_NM = "";
                            masterDomain.BAD_CD = "";
                        }
                        //
                        if (!masterDomain.BAD_NM.Equals((e.Value == null ? "" : e.Value.ToString())))
                        {
                            TecVo bankIoDao = this.lue_BAD_NM.GetItemFromValue(e.Value) as TecVo;
                            //
                            if (bankIoDao != null)
                            {
                                masterDomain.BAD_CD = bankIoDao.BAD_CD;
                                masterDomain.BAD_NM = bankIoDao.BAD_NM;
                            }

                            masterDomain.CRE_USR_ID = SystemProperties.USER_VO.USR_ID;
                            masterDomain.UPD_USR_ID = SystemProperties.USER_VO.USR_ID;
                            masterDomain.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
                            masterDomain.ITM_CD = this.ChekList[0].ITM_CD;

                            masterDomain.isCheckd = true;
                            this.OKButton.IsEnabled = true;
                        }
                    }
                }
            }
            catch (Exception eLog)
            {
                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                e.ErrorContent = eLog.Message;
                e.SetError(e.ErrorContent, e.ErrorType);
                return;
            }
    }
    //#region Functon (getDomain - ConfigView1Dao)
    //private TecVo getDomain()
    //{
    //    TecVo Dao = new TecVo();
    //    //Dao.CLSS_TP_CD = this.text_ClssTpCd.Text;
    //    //Dao.CLSS_TP_NM = this.text_ClssTpNm.Text;
    //    //Dao.SYS_FLG = this.text_SysFlg.Text;
    //    //Dao.SYS_AREA_CD = this.text_SysAreaCd.Text;
    //    //Dao.CLSS_CD_DESC = this.text_ClssCdDesc.Text;
    //    //Dao.DELT_FLG = (this.combo_deltFlg.Text.Equals("사용") ? "N" : "Y");
    //    //Dao.USR_ID = SystemProperties.USER;
    //    Dao.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
    //    return Dao;
    //}
    //#endregion

    //Vo
    //public SystemCodeVo resultDomain
    //{
    //    get
    //    {
    //        return this.updateDao;
    //    }
    //}

    }
}
