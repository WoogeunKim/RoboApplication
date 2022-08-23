using AquilaErpWpfApp3.Util;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Man;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;
using ModelsLibrary.Sale;

namespace AquilaErpWpfApp3.View.M.Dialog
{
    public partial class M66540MasterDialog : DXWindow
    {

        private string _title = "작업지시서 발행(사출)";

        //private static ManServiceClient manClient = SystemProperties.ManClient;
        private ManVo orgDao;
        private bool isEdit = false;
        //private ManVo updateDao;

        public M66540MasterDialog(ManVo Dao)
        {
            InitializeComponent();

            this.orgDao = Dao;

            SYSTEM_CODE_VO();
            //
            //
            //ManVo copyDao = new ManVo()
            //{
            //    PCK_PLST_CLSS_CD = Dao.PCK_PLST_CLSS_CD,
            //    PCK_PLST_CLSS_NM = Dao.PCK_PLST_CLSS_NM,
            //    PCK_PLST_CD = Dao.PCK_PLST_CD,
            //    PCK_PLST_NM = Dao.PCK_PLST_NM,
            //    PCK_PLST_VAL = Dao.PCK_PLST_VAL,
            //    CRE_DT = Dao.CRE_DT,
            //    CHNL_CD = Dao.CHNL_CD,
            //    CRE_USR_ID = Dao.CRE_USR_ID,
            //    UPD_USR_ID = Dao.UPD_USR_ID
            //};

            //수정
            //if (copyDao.PCK_PLST_CLSS_CD != null)
            //{
            //    //this.combo_PCK_PLST_CLSS_CD.IsReadOnly = true;
            //    //this.text_PCK_PLST_CD.IsReadOnly = true;
            //    this.isEdit = true;
            //    //
            //    //copyDao.EQ_PUR_YRMON = DateTime.ParseExact(Dao.EQ_PUR_YRMON + "01", "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None).ToString("yyyy-MM-dd");
            //}
            //else
            //{
            //    //추가
            //    //this.text_ClssTpCd.IsReadOnly = false;
            //    //this.isEdit = false;
            //    //copyDao.EQ_PUR_YRMON = System.DateTime.Now.ToString("yyyy-MM-dd");
            //    copyDao.PCK_PLST_VAL = 0;
            //    //copyDao.EQ_PUR_AMT = 0;
            //}
            //this.configCode.DataContext = copyDao;
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
                ////ProgramVo resultVo;
                if (isEdit == false)
                {
                    //IList<ManVo> _voList = this.ViewGridDtl.SelectedItems as IList<ManVo>;
                    if (WinUIMessageBox.Show("(사출) 작업 지시서 생성 하시겠습니까?", _title, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        //월 생산 계획
                        //SELECT* FROM TB_SL_MON_PLN
                        SaleVo _saleVo = new SaleVo();

                        _saleVo.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
                        _saleVo.CRE_USR_ID = SystemProperties.USER;
                        _saleVo.UPD_USR_ID = SystemProperties.USER;
                        _saleVo.ITM_CD = this.orgDao.ITM_CD;
                        _saleVo.SL_PLN_YRMON = this.orgDao.SL_PLN_YRMON;
                        _saleVo.SL_CO_CD = this.orgDao.SL_CO_CD;
                        _saleVo.SL_PLN_QTY = this.orgDao.PROD_PLN_QTY;
                        _saleVo.SL_PLN_CD = "U";
                        _saleVo.CLZ_FLG = "N";
                        _saleVo.SL_ORD_NO =  this.orgDao.SL_ORD_NO;
                        _saleVo.SL_ORD_SEQ = this.orgDao.SL_ORD_SEQ;
                        _saleVo.GBN = this.orgDao.SL_PLN_NO;

                        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m66540/mst/tmp1/i", new StringContent(JsonConvert.SerializeObject(_saleVo), System.Text.Encoding.UTF8, "application/json")))
                        {
                            //if (response.IsSuccessStatusCode)
                            //{
                            //    string result = await response.Content.ReadAsStringAsync();
                            //    if (int.TryParse(result, out _Num) == false)
                            //    {
                            //        //실패
                            //        return;
                            //    }
                            //}
                        }

                        ManVo _manVo = new ManVo();
                        _manVo.PROD_PLN_NO = this.orgDao.SL_PLN_NO;
                        _manVo.SL_PLN_NO = this.orgDao.SL_PLN_NO;
                        _manVo.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
                        _manVo.CRE_USR_ID = SystemProperties.USER;
                        _manVo.UPD_USR_ID = SystemProperties.USER;
                        _manVo.PROD_PLN_QTY = this.orgDao.PROD_PLN_QTY;
                        _manVo.CLZ_FLG = "N";
                        _manVo.MCHN_NO = this.orgDao.MCHN_NO;


                        //일 생산 계획
                        //SELECT * FROM TB_PROD_MON_PLN
                        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m66540/mst/tmp2/i", new StringContent(JsonConvert.SerializeObject(_manVo), System.Text.Encoding.UTF8, "application/json")))
                        {
                            //if (response.IsSuccessStatusCode)
                            //{
                            //    string result = await response.Content.ReadAsStringAsync();
                            //    if (int.TryParse(result, out _Num) == false)
                            //    {
                            //        //실패
                            //        return;
                            //    }
                            //}
                        }


                        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m66540/mst/m", new StringContent(JsonConvert.SerializeObject(this.ViewGridDtl.SelectedItems), System.Text.Encoding.UTF8, "application/json")))
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
                }
                else
                {
                //    this.updateDao = getDomain();

                //    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6625/u", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
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
                }

                //ManVo resultVo;
                //if (isEdit == false)
                //{
                //    this.updateDao = getDomain();//this.updateDao
                //    resultVo = manClient.M6625InsertMaster(this.updateDao);
                //    if (!resultVo.isSuccess)
                //    {
                //        //실패
                //        WinUIMessageBox.Show(resultVo.Message, "[" + SystemProperties.PROGRAM_TITLE + "]포장 코드 관리", MessageBoxButton.OK, MessageBoxImage.Error);
                //        return;
                //    }
                //    //성공
                //    WinUIMessageBox.Show("완료 되었습니다", "[추가]포장 코드 관리", MessageBoxButton.OK, MessageBoxImage.Information);
                //}
                //else
                //{
                //    this.updateDao = getDomain();
                //    resultVo = manClient.M6625UpdateMaster(this.updateDao);
                //    if (!resultVo.isSuccess)
                //    {
                //        //실패
                //        WinUIMessageBox.Show(resultVo.Message, "[" + SystemProperties.PROGRAM_TITLE + "]포장 코드 관리", MessageBoxButton.OK, MessageBoxImage.Error);
                //        return;
                //    }
                //    //성공
                //    WinUIMessageBox.Show("완료 되었습니다", "[수정]포장 코드 관리", MessageBoxButton.OK, MessageBoxImage.Information);
                //    //
                //    this.orgDao.PCK_PLST_CLSS_CD = this.updateDao.PCK_PLST_CLSS_CD;
                //    this.orgDao.PCK_PLST_CLSS_NM = this.updateDao.PCK_PLST_CLSS_NM;
                //    this.orgDao.PCK_PLST_CD = this.updateDao.PCK_PLST_CD;
                //    this.orgDao.PCK_PLST_NM = this.updateDao.PCK_PLST_NM;
                //    this.orgDao.PCK_PLST_VAL = this.updateDao.PCK_PLST_VAL;
                //    this.orgDao.CRE_DT = this.updateDao.CRE_DT;
                //    this.orgDao.CRE_USR_ID = this.updateDao.CRE_USR_ID;
                //    this.orgDao.UPD_USR_ID = this.updateDao.UPD_USR_ID;
                //    this.orgDao.CHNL_CD = this.updateDao.CHNL_CD;
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

            if (this.ViewGridDtl.SelectedItems.Count <= 0)
            {
                    WinUIMessageBox.Show("[선택 공정] 입력 값이 맞지 않습니다.", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;

            }

            //if (string.IsNullOrEmpty(this.combo_PCK_PLST_CLSS_CD.Text))
            //{
            //    WinUIMessageBox.Show("[구분] 입력 값이 맞지 않습니다.", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.combo_PCK_PLST_CLSS_CD.IsTabStop = true;
            //    this.combo_PCK_PLST_CLSS_CD.Focus();
            //    return false;
            //}
            //else if (string.IsNullOrEmpty(this.text_PCK_PLST_CD.Text))
            //{
            //    WinUIMessageBox.Show("[포장 코드] 입력 값이 맞지 않습니다.", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.text_PCK_PLST_CD.IsTabStop = true;
            //    this.text_PCK_PLST_CD.Focus();
            //    return false;
            //}
            //else if (this.text_SysFlg.Text == null || this.text_SysFlg.Text.Trim().Length == 0)
            //{
            //    WinUIMessageBox.Show("[구분] 입력 값이 맞지 않습니다.", "[유효검사]시스템 분류 코드", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.text_SysFlg.IsTabStop = true;
            //    this.text_SysFlg.Focus();
            //    return false;
            //}
            //else if (this.text_SysAreaCd.Text == null || this.text_SysAreaCd.Text.Trim().Length == 0)
            //{
            //    WinUIMessageBox.Show("[업무 분야] 입력 값이 맞지 않습니다.", "[유효검사]시스템 분류 코드", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.text_SysAreaCd.IsTabStop = true;
            //    this.text_SysAreaCd.Focus();
            //    return false;
            //}
            //else if (this.combo_deltFlg.Text == null || this.combo_deltFlg.Text.Trim().Length == 0)
            //{
            //    WinUIMessageBox.Show("[사용 여부] 입력 값이 맞지 않습니다.", "[유효검사]시스템 분류 코드", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.combo_deltFlg.IsTabStop = true;
            //    this.combo_deltFlg.Focus();
            //    return false;
            //}
            //else
            //{
            //    if (this.isEdit == false)
            //    {
            //        //CodeDao PCK_PLST_CLSS_CD = this.combo_PCK_PLST_CLSS_CD.SelectedItem as CodeDao;

            //        //ManVo dao = new ManVo()
            //        //{
            //        //    PCK_PLST_CLSS_CD = PCK_PLST_CLSS_CD.CLSS_CD,
            //        //    PCK_PLST_CD = this.text_PCK_PLST_CD.Text,
            //        //    CHNL_CD = SystemProperties.USER_VO.CHNL_CD
            //        //};
            //        //IList<ManVo> daoList = (IList<ManVo>)manClient.M6625SelectMaster(dao);
            //        //if (daoList.Count != 0)
            //        //{
            //        //    WinUIMessageBox.Show("[코드 - 중복] 코드를 다시 입력 하십시오.", "[중복검사]포장 코드 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
            //        //    this.text_PCK_PLST_CD.IsTabStop = true;
            //        //    this.text_PCK_PLST_CD.Focus();
            //        //    return false;
            //        //}
            //    }
            //}
            return true;
        }
        #endregion

        #region Functon (getDomain - ConfigView1Dao)
        //private ManVo getDomain()
        //{
        //    ManVo Dao = new ManVo();
        //    //Dao.PCK_PLST_CD = this.text_PCK_PLST_CD.Text;
        //    //Dao.PCK_PLST_NM = this.text_PCK_PLST_NM.Text;
        //    ////Dao.EQ_LOC_CD = this.text_EQ_LOC_CD.Text;
        //    //SystemCodeVo PCK_PLST_CLSS_CD = this.combo_PCK_PLST_CLSS_CD.SelectedItem as SystemCodeVo;
        //    //if (PCK_PLST_CLSS_CD != null)
        //    //{
        //    //    Dao.PCK_PLST_CLSS_NM = PCK_PLST_CLSS_CD.CLSS_DESC;
        //    //    Dao.PCK_PLST_CLSS_CD = PCK_PLST_CLSS_CD.CLSS_CD;
        //    //}

        //    //Dao.PCK_PLST_VAL = int.Parse((string.IsNullOrEmpty(this.text_PCK_PLST_VAL.Text) ? "0" : this.text_PCK_PLST_VAL.Text));

        //    //
        //    Dao.CRE_USR_ID = SystemProperties.USER;
        //    Dao.UPD_USR_ID = SystemProperties.USER;
        //    Dao.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;

        //    return Dao;
        //}
        #endregion


        public async void SYSTEM_CODE_VO()
        {
            //this.combo_PCK_PLST_CLSS_CD.ItemsSource = SystemProperties.SYSTEM_CODE_VO("M-009");
            //using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "M-009"))
            //{
            //    if (response.IsSuccessStatusCode)
            //    {
            //        this.combo_PCK_PLST_CLSS_CD.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
            //    }
            //}

            this.orgDao.CRE_USR_ID = SystemProperties.USER;
            this.orgDao.UPD_USR_ID = SystemProperties.USER;
            this.orgDao.PCK_FLG = "C";
            //this.orgDao.ROUT_CD = "IN";
            this.orgDao.A_ROUT_CD = new string[] { "IN" };

            //
            //ViewGridDtl
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m66540/popup", new StringContent(JsonConvert.SerializeObject(this.orgDao), System.Text.Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.ViewGridDtl.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
                }

            }

            //설비
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6622", new StringContent(JsonConvert.SerializeObject(new ManVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.lue_PROD_EQ_NO.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
                }
            }

            //금형
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6661", new StringContent(JsonConvert.SerializeObject(new ManVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.lue_MOLD_NO.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
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

        private void GridColumn_Validate(object sender, DevExpress.Xpf.Grid.GridCellValidationEventArgs e)
        {
            try
            {
                ManVo masterDomain = (ManVo)ViewGridDtl.GetFocusedRow();
                bool gbn = (e.Column.FieldName.ToString().Equals("GBN") ? true : false);
                bool prodEqNo = (e.Column.FieldName.ToString().Equals("MCHN_NO") ? true : false);
                bool moldNo = (e.Column.FieldName.ToString().Equals("MOLD_CD") ? true : false);
                //bool isCheckd = (e.Column.FieldName.ToString().Equals("isCheckd") ? true : false);

                //InvVo resultVo;
                //InvVo insertVo;
                //
                //if (rmnQty)
                //{
                //    if (e.IsValid)
                //    {
                //        if (string.IsNullOrEmpty(masterDomain.IN_QTY + ""))
                //        {
                //            masterDomain.IN_QTY = 0;
                //        }
                //        //
                //        if (!masterDomain.IN_QTY.Equals((e.Value == null ? "" : e.Value.ToString())))
                //        {
                //            int? tmpInt = Convert.ToInt32(e.Value.ToString());
                //            //if (int.Parse(e.Value.ToString()) > masterDomain.PUR_QTY)
                //            //{
                //            //    e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                //            //    e.ErrorContent = "[발주수량]" + masterDomain.PUR_QTY + " 보다 큰 값은 입력 하실수 없습니다";
                //            //    e.SetError(e.ErrorContent, e.ErrorType);
                //            //    return;
                //            //}
                //            //else if(e.Value.ToString().Equals("0"))
                //            //{
                //            //    e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                //            //    e.ErrorContent = "0 보다 작은 값은 입력 하실수 없습니다";
                //            //    e.SetError(e.ErrorContent, e.ErrorType);
                //            //    return;
                //            //}

                //            masterDomain.IN_QTY = tmpInt;

                //            try
                //            {
                //                masterDomain.IMP_ITM_AMT = tmpInt * Convert.ToDouble(masterDomain.IMP_ITM_PRC);
                //            }
                //            catch (Exception)
                //            {
                //                masterDomain.IMP_ITM_AMT = 0;
                //            }

                //            masterDomain.isCheckd = true;
                //            this.OKButton.IsEnabled = true;
                //            //insertVo = new InvVo() {
                //            //          INSRL_NO = this.orgVo.INSRL_NO
                //            //        , INAUD_CD = masterDomain.INAUD_CD
                //            //        , ITM_CD = masterDomain.ITM_CD
                //            //        , CO_UT_PRC = masterDomain.CO_UT_PRC
                //            //        , PUR_AMT = masterDomain.PUR_AMT
                //            //        , PUR_ORD_NO = masterDomain.PUR_ORD_NO
                //            //        , PUR_ORD_SEQ = masterDomain.PUR_ORD_SEQ
                //            //        , RMN_QTY = int.Parse(e.Value.ToString())
                //            //        , CRE_USR_ID = SystemProperties.USER
                //            //        , UPD_USR_ID = SystemProperties.USER };

                //            //resultVo = invClient.I5511InsertPurDtl(insertVo);
                //            //if (!resultVo.isSuccess)
                //            //{
                //            //    //실패
                //            //    WinUIMessageBox.Show(resultVo.Message, "[에러]품목 입고 관리", MessageBoxButton.OK, MessageBoxImage.Error);
                //            //    return;
                //            //}
                //        }
                //    }
                //}
                //else
                
                if (gbn)
                {
                    //if (e.IsValid)
                    //{
                    //    if (string.IsNullOrEmpty(masterDomain.INAUD_CD + ""))
                    //    {
                    //        masterDomain.INAUD_NM = "";
                    //        masterDomain.INAUD_CD = "";
                    //    }
                    //    //
                    //    if (!masterDomain.INAUD_CD.Equals((e.Value == null ? "" : e.Value.ToString())))
                    //    {
                    //        SystemCodeVo bankIoDao = this.lue_INAUD_NM.GetItemFromValue(e.Value) as SystemCodeVo;
                    //        //
                    //        if (bankIoDao != null)
                    //        {
                    //            masterDomain.INAUD_CD = bankIoDao.CLSS_CD;
                    //            masterDomain.INAUD_NM = bankIoDao.CLSS_DESC;
                    //        }

                    //        masterDomain.isCheckd = true;
                    //        this.OKButton.IsEnabled = true;
                    //    }
                    //}
                }
                else if (prodEqNo)
                {
                    if (e.IsValid)
                    {
                        if (string.IsNullOrEmpty(masterDomain.MCHN_NO))
                        {
                            masterDomain.MCHN_NM = "";
                            masterDomain.MCHN_NO = "";
                        }
                        //
                        if (!masterDomain.MCHN_NO.Equals((e.Value == null ? "" : e.Value.ToString())))
                        {
                            ManVo bankIoDao = this.lue_PROD_EQ_NO.GetItemFromValue(e.Value) as ManVo;
                            //
                            if (bankIoDao != null)
                            {
                                masterDomain.MCHN_NO = bankIoDao.PROD_EQ_NO;
                                masterDomain.MCHN_NM = bankIoDao.EQ_NM;

                                this.orgDao.MCHN_NO = bankIoDao.PROD_EQ_NO;
                                this.orgDao.MCHN_NM = bankIoDao.EQ_NM;
                            }

                            masterDomain.isCheckd = true;
                            this.OKButton.IsEnabled = true;
                        }
                    }
                }
                else if (moldNo)
                {
                    if (e.IsValid)
                    {
                        if (string.IsNullOrEmpty(masterDomain.MOLD_CD))
                        {
                            masterDomain.MOLD_CD = "";
                            masterDomain.MOLD_NM = "";
                        }
                        //
                        if (!masterDomain.MOLD_CD.Equals((e.Value == null ? "" : e.Value.ToString())))
                        {
                            ManVo bankIoDao = this.lue_MOLD_NO.GetItemFromValue(e.Value) as ManVo;
                            //
                            if (bankIoDao != null)
                            {
                                masterDomain.MOLD_CD = bankIoDao.MOLD_NO;
                                masterDomain.MOLD_NM = bankIoDao.MOLD_NM;
                            }

                            masterDomain.isCheckd = true;
                            this.OKButton.IsEnabled = true;
                        }
                    }
                }
                //else if (impItmPrc)
                //{
                //    if (e.IsValid)
                //    {
                //        if (string.IsNullOrEmpty(masterDomain.IMP_ITM_PRC + ""))
                //        {
                //            masterDomain.IMP_ITM_PRC = 0;
                //        }
                //        //
                //        if (!masterDomain.IMP_ITM_PRC.Equals((e.Value == null ? "" : e.Value.ToString())))
                //        {
                //            double? tmpInt = Convert.ToDouble(e.Value.ToString());
                //            //if (tmpInt <= -1)
                //            //{
                //            //    e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                //            //    e.ErrorContent = "[단가]" + tmpInt + " -값은 입력 하실수 없습니다";
                //            //    e.SetError(e.ErrorContent, e.ErrorType);
                //            //    return;
                //            //}

                //            masterDomain.IMP_ITM_PRC = tmpInt;

                //            try
                //            {
                //                masterDomain.IMP_ITM_AMT = Convert.ToInt32(masterDomain.IN_QTY) * tmpInt;
                //            }
                //            catch (Exception)
                //            {
                //                masterDomain.IMP_ITM_AMT = 0;
                //            }

                //            masterDomain.isCheckd = true;
                //            this.OKButton.IsEnabled = true;
                //        }
                //    }
                //}

                this.ViewGridDtl.RefreshData();
            }
            catch (Exception eLog)
            {
                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                e.ErrorContent = eLog.Message;
                //this.MSG.Text = eLog.Message;
                e.SetError(e.ErrorContent, e.ErrorType);
                return;
            }
        }

        private void viewPage1EditView_HiddenEditor(object sender, DevExpress.Xpf.Grid.EditorEventArgs e)
        {
            this.ViewTableDtl.CommitEditing();

            //bool rmnQty = (e.Column.FieldName.ToString().Equals("RMN_QTY") ? true : false);

            //int rowHandle = this.viewJOB_ITEMView.FocusedRowHandle + 1;

            //if (rmnQty)
            //{
            //    this.ViewJOB_ITEMEdit.CurrentColumn = this.ViewJOB_ITEMEdit.Columns["RMN_QTY"];
            //}

            //this.ViewJOB_ITEMEdit.RefreshRow(rowHandle - 1);
            //this.viewJOB_ITEMView.FocusedRowHandle = rowHandle;
        }

        //Vo
        //public ManVo resultDomain
        //{
        //    get
        //    {
        //        return this.updateDao;
        //    }
        //}

    }
}
