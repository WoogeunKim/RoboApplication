using AquilaErpWpfApp3.Util;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Code;
using ModelsLibrary.Man;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;

namespace AquilaErpWpfApp3.View.M.Dialog
{
    public partial class M66520MasterDialog : DXWindow
    {

        private string _title = "작업지시서 발행(충전)";

        //private static ManServiceClient manClient = SystemProperties.ManClient;
        private ManVo orgDao;
        private bool isEdit = false;
        //private ManVo updateDao;

        public M66520MasterDialog(ManVo Dao)
        {
            InitializeComponent();

            this.orgDao = Dao;

            this.text_PROD_PLN_QTY.Text =  "" + this.orgDao.PROD_PLN_QTY;

            this.text_PROD_PLN_QTY_A.Text = "0";
            this.text_PROD_PLN_QTY_B.Text =  "0";
            this.text_PROD_PLN_QTY_C.Text =  "0";

            this.text_PROD_PLN_QTY_EX.Text = "" + this.orgDao.PROD_PLN_QTY;

            this.text_INP_STAFF_VAL.Text = "1";

            this.text_PROD_PLN_DT.Text = DateTime.Now.ToString("yyyy-MM-dd");

            SYSTEM_CODE_VO();
            //
            PopupRoutQty();
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


            this.ResetButton.Click += new RoutedEventHandler(ResetButton_Click);
        }

        private void GridColumn_Validate(object sender, DevExpress.Xpf.Grid.GridCellValidationEventArgs e)
        {
            try
            {
                ManVo masterDomain = (ManVo)ViewGridDtl.GetFocusedRow();
                bool inpStaffVal = (e.Column.FieldName.ToString().Equals("INP_STAFF_VAL") ? true : false);
                bool dyNgtNm = (e.Column.FieldName.ToString().Equals("DY_NGT_NM") ? true : false);
                bool prodEqNo = (e.Column.FieldName.ToString().Equals("PROD_EQ_NO") ? true : false);
                //bool isCheckd = (e.Column.FieldName.ToString().Equals("isCheckd") ? true : false);

                //InvVo resultVo;
                //InvVo insertVo;
                //
                if (inpStaffVal)
                {
                    if (e.IsValid)
                    {
                        if (string.IsNullOrEmpty(masterDomain.INP_STAFF_VAL + ""))
                        {
                            masterDomain.INP_STAFF_VAL = 0;
                        }
                        //
                        if (!masterDomain.INP_STAFF_VAL.Equals((e.Value == null ? "" : e.Value.ToString())))
                        {
                            masterDomain.INP_STAFF_VAL = e.Value.ToString();

                            this.OKButton.IsEnabled = true;
                        }
                    }
                }
                //else if (dyNgtNm)
                //{
                //    if (e.IsValid)
                //    {
                //        if (string.IsNullOrEmpty(masterDomain.DY_NGT_NM))
                //        {
                //            masterDomain.DY_NGT_NM = "";
                //            masterDomain.DY_NGT_FLG = "";
                //        }
                //        //
                //        if (!masterDomain.DY_NGT_NM.Equals((e.Value == null ? "" : e.Value.ToString())))
                //        {
                //            SystemCodeVo bankIoDao = this.lue_DY_NGT_NM.GetItemFromValue(e.Value) as SystemCodeVo;
                //            //
                //            if (bankIoDao != null)
                //            {
                //                masterDomain.DY_NGT_FLG = bankIoDao.CLSS_CD;
                //                masterDomain.DY_NGT_NM = bankIoDao.CLSS_DESC;
                //            }

                //            masterDomain.isCheckd = true;
                //            this.OKButton.IsEnabled = true;
                //        }
                //    }
                //}
                else if (prodEqNo)
                {
                    if (e.IsValid)
                    {
                        if (string.IsNullOrEmpty(masterDomain.PROD_EQ_NO))
                        {
                            masterDomain.PROD_EQ_NO = "";
                            masterDomain.EQ_NO = "";
                            masterDomain.EQ_NM = "";
                        }
                        //
                        if (!masterDomain.PROD_EQ_NO.Equals((e.Value == null ? "" : e.Value.ToString())))
                        {
                            ManVo bankIoDao = this.lue_PROD_EQ_NO.GetItemFromValue(e.Value) as ManVo;
                            //
                            if (bankIoDao != null)
                            {
                                masterDomain.PROD_EQ_NO = bankIoDao.PROD_EQ_NO;
                                masterDomain.EQ_NO = bankIoDao.PROD_EQ_NO;
                                masterDomain.EQ_NM = bankIoDao.EQ_NM;
                            }

                            masterDomain.isCheckd = true;
                            this.OKButton.IsEnabled = true;
                        }
                    }
                }
                //else if (isCheckd)
                //{
                //    this.OKButton.IsEnabled = true;
                //}
            }
            catch (Exception eLog)
            {
                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                e.ErrorContent = eLog.Message;
                e.SetError(e.ErrorContent, e.ErrorType);
                return;
            }
        }

        private void viewPage1EditView_HiddenEditor(object sender, DevExpress.Xpf.Grid.EditorEventArgs e)
        {
            this.ViewTableDtl.CommitEditing();

            //bool inaudQty = (e.Column.FieldName.ToString().Equals("IN_QTY") ? true : false);
            //bool inaudOrgNm = (e.Column.FieldName.ToString().Equals("INAUD_ORG_NM") ? true : false);
            //bool inaudRmk = (e.Column.FieldName.ToString().Equals("ITM_RMK") ? true : false);

            //int rowHandle = this.viewJOB_ITEMView.FocusedRowHandle + 1;

            //if (inaudQty)
            //{
            //    this.ViewJOB_ITEMEdit.CurrentColumn = this.ViewJOB_ITEMEdit.Columns["IN_QTY"];
            //}
            //else if (inaudOrgNm)
            //{
            //    this.ViewJOB_ITEMEdit.CurrentColumn = this.ViewJOB_ITEMEdit.Columns["INAUD_ORG_NM"];
            //}
            //else if (inaudRmk)
            //{
            //    this.ViewJOB_ITEMEdit.CurrentColumn = this.ViewJOB_ITEMEdit.Columns["ITM_RMK"];
            //}

            //this.ViewJOB_ITEMEdit.RefreshRow(rowHandle - 1);
            //this.viewJOB_ITEMView.FocusedRowHandle = rowHandle;
        }


        private async void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            this.orgDao.CRE_USR_ID = SystemProperties.USER;
            this.orgDao.UPD_USR_ID = SystemProperties.USER;
            this.orgDao.PCK_FLG = "C";

            //
            int _tmp;
            try
            {
                if (Int32.TryParse(this.text_PROD_PLN_QTY_A.Text, out _tmp) == false)
                {
                    WinUIMessageBox.Show("[지시수량 - 오전] 입력 값이 맞지 않습니다.", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                else if (Int32.TryParse(this.text_PROD_PLN_QTY_B.Text, out _tmp) == false)
                {
                    WinUIMessageBox.Show("[지시수량 - 오후] 입력 값이 맞지 않습니다.", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                else if (Int32.TryParse(this.text_PROD_PLN_QTY_C.Text, out _tmp) == false)
                {
                    WinUIMessageBox.Show("[지시수량 - 연장] 입력 값이 맞지 않습니다.", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                //this.text_PROD_PLN_QTY.Text = "" + (Convert.ToInt32(this.text_PROD_PLN_QTY_A.Text) + Convert.ToInt32(this.text_PROD_PLN_QTY_B.Text) + Convert.ToInt32(this.text_PROD_PLN_QTY_C.Text));
            }
            catch
            {
                this.text_PROD_PLN_QTY.Text = "0";
            }


            //지시수량
            this.orgDao.PROD_PLN_QTY = Convert.ToInt32(this.text_PROD_PLN_QTY.Text);
            this.orgDao.PROD_PLN_QTY_A = Convert.ToInt32(this.text_PROD_PLN_QTY_A.Text);
            this.orgDao.PROD_PLN_QTY_B = Convert.ToInt32(this.text_PROD_PLN_QTY_B.Text);
            this.orgDao.PROD_PLN_QTY_C = Convert.ToInt32(this.text_PROD_PLN_QTY_C.Text);

            //설비
            ManVo _eqVo = this.combo_PROD_EQ_NO.SelectedItem as ManVo;
            if (_eqVo != null)
            {
                //설비
                this.orgDao.PROD_EQ_NO = _eqVo.PROD_EQ_NO;
                this.orgDao.EQ_NM = _eqVo.EQ_NM;
            }

            //투입인원
            this.orgDao.INP_STAFF_VAL = this.text_INP_STAFF_VAL.Text;


            //if (this.lue_DY_NGT_NM.SelectedItems.Count < 1)
            //{
            //    WinUIMessageBox.Show("[구분] 입력 값이 맞지 않습니다.", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
            //    return;
            //}
            //else
            //{
            //    //
            //    this.orgDao.DY_NGT_NM = (this.lue_DY_NGT_NM.SelectedItem as SystemCodeVo).CLSS_DESC;
            //    this.orgDao.DY_NGT_FLG = (this.lue_DY_NGT_NM.SelectedItem as SystemCodeVo).CLSS_CD;
            //}
            this.orgDao.PROD_PLN_DT = this.text_PROD_PLN_DT.Text;
            //비고
            this.orgDao.MM_RMK = this.text_LOT_DIV_RMK.Text;

            //오전, 오후, 연장
            //this.orgDao.DY_NGT_RMK = String.Join(", ", this.lue_DY_NGT_NM.SelectedItems.OfType<SystemCodeVo>().Select(es => es.CLSS_DESC).ToList());

            //ViewGridDtl
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m66520/popup", new StringContent(JsonConvert.SerializeObject(this.orgDao), System.Text.Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.ViewGridDtl.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
                }

            }


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
                    if (WinUIMessageBox.Show("(충전) 작업 지시서 생성 하시겠습니까?", _title, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m66520/mst/m/c", new StringContent(JsonConvert.SerializeObject(this.ViewGridDtl.SelectedItems), System.Text.Encoding.UTF8, "application/json")))
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
                //this.DialogResult = true;
                //this.Close();


                ResetButton_Click(sender, e);
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

            int? _nA = Convert.ToInt32(this.text_PROD_PLN_QTY_A.Text);
            int? _nB = Convert.ToInt32(this.text_PROD_PLN_QTY_B.Text);
            int? _nC = Convert.ToInt32(this.text_PROD_PLN_QTY_C.Text);

            if ((_nA + _nB + _nC) > Convert.ToInt32(this.text_PROD_PLN_QTY_EX.Text))
            {
                WinUIMessageBox.Show("[지시 수량] 입력 값이 맞지 않습니다.", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
           



            //int? nCnt = this.lue_DY_NGT_NM.SelectedItems.Count;

            //if(this.lue_DY_NGT_NM.SelectedItems.Count < 1)
            //{
            //    WinUIMessageBox.Show("[구분] 입력 값이 맞지 않습니다.", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
            //    return false;
            //}


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

        private async void PopupRoutQty()
        {
            this.orgDao.CRE_USR_ID = SystemProperties.USER;
            this.orgDao.UPD_USR_ID = SystemProperties.USER;
            this.orgDao.PCK_FLG = "C";

            IList<ManVo> _routQtyList;
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m66520/popup/routqty", new StringContent(JsonConvert.SerializeObject(this.orgDao), System.Text.Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    _routQtyList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
                    // 선착인 표시
                    if (_routQtyList.Any<ManVo>(x => x.ROUT_NM.Equals("선착인")) == true)
                    {
                        this.stackpanel_Rout1.Visibility = Visibility.Visible;
                        this.text_A.Text = Convert.ToDecimal(_routQtyList.Where<ManVo>(x => x.ROUT_NM.Equals("선착인")).FirstOrDefault<ManVo>()?.LOT_DIV_QTY).ToString("N0");


                        // Convert.ToDecimal(Dao.INP_QTY).ToString("N0"),
                    }
                    else
                    {
                        this.stackpanel_Rout1.Visibility = Visibility.Collapsed;
                    }


                    // 충전1 표시
                    if (_routQtyList.Any<ManVo>(x => x.ROUT_NM.Equals("충전1")) == true)  // RoutQTYList에 선공정이 하나라도 있는지 확인 후에 T/F 반환(Any)
                    {
                        this.stackpanel_Rout2.Visibility = Visibility.Visible;
                        this.text_B.Text = Convert.ToDecimal(_routQtyList.Where<ManVo>(x => x.ROUT_NM.Equals("충전1")).FirstOrDefault<ManVo>()?.LOT_DIV_QTY).ToString("#,###,##0");
                    }
                    else
                    {
                        this.stackpanel_Rout2.Visibility = Visibility.Collapsed;
                    }


                    // 충전2 표시
                    if (_routQtyList.Any<ManVo>(x => x.ROUT_NM.Equals("충전2")) == true)
                    {
                        this.stackpanel_Rout3.Visibility = Visibility.Visible;
                        this.text_C.Text = Convert.ToDecimal(_routQtyList.Where<ManVo>(x => x.ROUT_NM.Equals("충전2")).FirstOrDefault<ManVo>()?.LOT_DIV_QTY).ToString("#,###,##0");
                    }
                    else
                    {
                        this.stackpanel_Rout3.Visibility = Visibility.Collapsed;
                    }
                }
            }
        }

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

            //구분
            //this.lue_DY_NGT_NM.ItemsSource = new List<SystemCodeVo>() { new SystemCodeVo() { CLSS_CD = "A", CLSS_DESC = "오전" } , new SystemCodeVo() { CLSS_CD = "B", CLSS_DESC = "오후" }, new SystemCodeVo() { CLSS_CD = "C", CLSS_DESC = "연장" } };
            //this.lue_DY_NGT_NM.SelectedItem = (this.lue_DY_NGT_NM.ItemsSource as List<SystemCodeVo>)[0];
            //
            //this.orgDao.DY_NGT_NM = (this.lue_DY_NGT_NM.SelectedItem as SystemCodeVo).CLSS_DESC;
            //this.orgDao.DY_NGT_FLG = (this.lue_DY_NGT_NM.SelectedItem as SystemCodeVo).CLSS_CD;

            //호기
            using (HttpResponseMessage responseY = await SystemProperties.PROGRAM_HTTP.PostAsync("m6622", new StringContent(JsonConvert.SerializeObject(new ManVo() { DELT_FLG = "N", CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
            {
                if (responseY.IsSuccessStatusCode)
                {
                    //
                    this.lue_PROD_EQ_NO.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await responseY.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();

                    //
                    this.combo_PROD_EQ_NO.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await responseY.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
                }
            }

            this.orgDao.CRE_USR_ID = SystemProperties.USER;
            this.orgDao.UPD_USR_ID = SystemProperties.USER;
            this.orgDao.PCK_FLG = "C";

            //설비
            ManVo _eqVo = this.combo_PROD_EQ_NO.SelectedItem as ManVo;
            if (_eqVo != null)
            {
                //설비
                this.orgDao.PROD_EQ_NO = _eqVo.PROD_EQ_NO;
                this.orgDao.EQ_NM = _eqVo.EQ_NM;
            }

            //투입인원
            this.orgDao.INP_STAFF_VAL = this.text_INP_STAFF_VAL.Text;
            //지시 일자
            this.orgDao.PROD_PLN_DT = this.text_PROD_PLN_DT.Text;

            //오전, 오후, 연장
            //this.orgDao.MM_RMK = "오전";
            //this.orgDao.DY_NGT_RMK = String.Join(", ", this.lue_DY_NGT_NM.SelectedItems.OfType<SystemCodeVo>().Select(es => es.CLSS_DESC).ToList());
            //try
            //{
            //    this.text_PROD_PLN_QTY.Text =  "" + ( Convert.ToInt32(this.text_PROD_PLN_QTY_A.Text) + Convert.ToInt32(this.text_PROD_PLN_QTY_B.Text) + Convert.ToInt32(this.text_PROD_PLN_QTY_C.Text) );
            //}
            //catch
            //{
            //    this.text_PROD_PLN_QTY.Text = "0";
            //}


            //비고
            this.orgDao.MM_RMK = this.text_LOT_DIV_RMK.Text;

            //ViewGridDtl
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m66520/popup", new StringContent(JsonConvert.SerializeObject(this.orgDao), System.Text.Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.ViewGridDtl.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
                }

            }

        }

        void OnCustomColumnSort(object sender, CustomColumnSortEventArgs e)
        {
            if (e.Column.FieldName == "ROUT_NM")
            {
                int dayIndex1 = GetIndex((string)e.Value1);
                int dayIndex2 = GetIndex((string)e.Value2);
                e.Result = dayIndex1.CompareTo(dayIndex2);
                e.Handled = true;
            }
        }

        int GetIndex(string day)
        {
            if (string.IsNullOrEmpty(day))
            {
                return 0;
            }
            else if (day.Contains("선착인"))
            {
                return 1;
            }
            else if (day.Contains("충전1"))
            {
                return 2;
            }
            else if (day.Contains("충전2"))
            {
                return 3;
            }
            else if (day.Contains("착인"))
            {
                return 4;
            }
            else if (day.Contains("선공정"))
            {
                return 5;
            }
            else if (day.Contains("포장"))
            {
                return 6;
            }
            return 0;
            //return (int)Enum.Parse(typeof(DayOfWeek), day);
        }


        //IsEdit
        public bool IsEdit
        {
            get
            {
                return this.isEdit;
            }
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
