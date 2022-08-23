using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
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
using AquilaErpWpfApp3.Util;

namespace AquilaErpWpfApp3.View.SAL.Dialog
{
    public partial class S2217DetailDialog : DXWindow
    {
        //private static SaleOrderServiceClient saleOrderClient = SystemProperties.SaleOrderClient;
        private SaleVo orgVo;
        private string title = "반품등록관리";

        public S2217DetailDialog(SaleVo vo)
        {
            InitializeComponent();

            this.orgVo = vo;

            //this.txt_stDate.Text = System.DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            //this.txt_enDate.Text = System.DateTime.Now.ToString("yyyy-MM-dd");

            SYSTEM_CODE_VO();

            this.btn_reset.Click += btn_reset_Click;
            this.btn_apply.Click += btn_apply_Click;

            //this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
            this.OKButton.Click += new RoutedEventHandler(OKButton_Click);
            this.CancelButton.Click += new RoutedEventHandler(CancelButton_Click);


            this.OKButton.IsEnabled = false;


            this.txt_stDate.Text = Convert.ToDateTime(vo.FM_DT).AddMonths(-1).ToString("yyyy-MM-dd");
            this.txt_enDate.Text = vo.TO_DT;
            //this.combo_GRP_NM.Text = vo.SL_CO_CD;

            searchItem();
        }


        private void ButtonInfo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaleVo selVo = (SaleVo)ViewJOB_ITEMEdit.GetFocusedRow();
                if (selVo != null)
                {
                    //selVo.isCheckd = true;

                    if (Convert.ToInt32(selVo.SL_ITM_QTY) < 0)
                    {
                        selVo.SL_RTN_ITM_QTY = 0;
                    }
                    else
                    {
                        selVo.SL_RTN_ITM_QTY = Convert.ToInt32(selVo.TMP_ITM_QTY);
                        //selVo.CTRT_RMN_QTY = 0;
                    }
                    ////selVo.IMP_ITM_AMT = Convert.ToInt32(selVo.IMP_INV_QTY) * Convert.ToDouble(selVo.IMP_ITM_PRC) / 1000;


                    //selVo.RMN_QTY = Convert.ToDecimal(selVo.TMP_RMK_QTY) - Convert.ToDecimal(selVo.IN_QTY);
                    //this.OKButton.IsEnabled = true;

                    //    if (Convert.ToInt32(selVo.IN_QTY) < 0)
                    //    {
                    //        selVo.IN_QTY = 0;
                    //    }
                    //    else
                    //    {
                    //        selVo.IN_QTY = selVo.RMN_QTY;
                    //        //selVo.CTRT_RMN_QTY = 0;
                    //    }
                    //    //selVo.IMP_ITM_AMT = Convert.ToInt32(selVo.IMP_INV_QTY) * Convert.ToDouble(selVo.IMP_ITM_PRC) / 1000;
                    //    selVo.RMN_QTY = Convert.ToDecimal(selVo.TMP_RMK_QTY) - Convert.ToDecimal(selVo.IN_QTY);
                    //    this.OKButton.IsEnabled = true;


                    selVo.SL_ITM_QTY = Convert.ToInt64(selVo.TMP_ITM_QTY) - Convert.ToInt64(selVo.SL_RTN_ITM_QTY);

                    selVo.CRE_USR_ID = SystemProperties.USER;
                    selVo.UPD_USR_ID = SystemProperties.USER;
                    selVo.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
                    selVo.SL_RTN_ITM_CD = selVo.ITM_CD;


                    selVo.isCheckd = true;
                    this.OKButton.IsEnabled = true;

                    this.viewJOB_ITEMView.CommitEditing();
                    this.ViewJOB_ITEMEdit.RefreshData();
                    //this.viewJOB_ITEMView.FocusedRowHandle = this.viewJOB_ITEMView.FocusedRowHandle;
                }
            }
            catch (Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
                this.MSG.Text = eLog.Message;
                return;
            }
        }

        private void view_CellValueChanging(object sender, CellValueChangedEventArgs e)
        {
            if (e.Column.FieldName == "isCheckd")
            {
                TableView view = (TableView)sender;
                view.CloseEditor();
                view.FocusedRowHandle = GridControl.InvalidRowHandle;
                view.FocusedRowHandle = e.RowHandle;
            }
        }

        private void btn_apply_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.check_SL_RTN_NM.IsChecked == true)
                {
                    if (string.IsNullOrEmpty(this.combo_SL_RTN_NM.Text))
                    {
                        WinUIMessageBox.Show("[반품사유]입력 값이 맞지 안습니다", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                        this.MSG.Text = "[반품사유]입력 값이 맞지 안습니다";
                        return;
                    }
                }

                if (this.check_SL_BAD_ITM_NM.IsChecked == true)
                {
                    if (string.IsNullOrEmpty(this.combo_SL_BAD_ITM_NM.Text))
                    {
                        WinUIMessageBox.Show("[불량코드]입력 값이 맞지 안습니다", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                        this.MSG.Text = "[불량코드]입력 값이 맞지 안습니다";
                        return;
                    }


                }

                if (this.check_SL_ITM_QTY.IsChecked == true)
                {
                    if (string.IsNullOrEmpty(this.text_SL_ITM_QTY.Text))
                    {
                        WinUIMessageBox.Show("[반품수량]입력 값이 맞지 안습니다", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                        this.MSG.Text = "[반품수량]입력 값이 맞지 안습니다";
                        return;
                    }
                }


                ObservableCollectionCore<object> _checkList = (ObservableCollectionCore<object>)this.ViewJOB_ITEMEdit.SelectedItems;
                SaleVo _tmpVo;
                for (int x = 0; x < _checkList.Count; x++)
                {
                    _tmpVo = (SaleVo)_checkList[x];
                    if (this.check_SL_RTN_NM.IsChecked == true)
                    {
                        SystemCodeVo slRtnNmVo = this.combo_SL_RTN_NM.SelectedItem as SystemCodeVo;
                        if (slRtnNmVo != null)
                        {
                            _tmpVo.SL_RTN_NM = slRtnNmVo.CLSS_DESC;
                            _tmpVo.SL_RTN_CD = slRtnNmVo.CLSS_CD;
                        }
                        _tmpVo.isCheckd = true;
                        this.OKButton.IsEnabled = true;
                    }

                    if (this.check_SL_BAD_ITM_NM.IsChecked == true)
                    {
                        SystemCodeVo slBadItmNmVo = this.combo_SL_BAD_ITM_NM.SelectedItem as SystemCodeVo;
                        if (slBadItmNmVo != null)
                        {
                            _tmpVo.SL_BAD_ITM_NM = slBadItmNmVo.CLSS_DESC;
                            _tmpVo.SL_BAD_ITM_CD = slBadItmNmVo.CLSS_CD;

                            _tmpVo.isCheckd = true;
                            this.OKButton.IsEnabled = true;
                        }
                    }

                    if (this.check_SL_ITM_QTY.IsChecked == true)
                    {
                        _tmpVo.SL_RTN_ITM_QTY = Convert.ToInt32(string.IsNullOrEmpty(this.text_SL_ITM_QTY.Text) ? "0" : this.text_SL_ITM_QTY.Text);

                        _tmpVo.isCheckd = true;
                        this.OKButton.IsEnabled = true;
                    }

                    if (this.check_SL_RTN_ITM_RMK.IsChecked == true)
                    {
                        _tmpVo.SL_RTN_ITM_RMK = this.text_SL_RTN_ITM_RMK.Text;

                        _tmpVo.isCheckd = true;
                        this.OKButton.IsEnabled = true;
                    }
                }
            }
            catch (Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
                this.MSG.Text = eLog.Message;
                return;
            }
        }

        private void btn_reset_Click(object sender, RoutedEventArgs e)
        {
            searchItem();

        }

        private async void searchItem()
        {
            try
            {
                SaleVo vo = new SaleVo() {CHNL_CD = SystemProperties.USER_VO.CHNL_CD, FM_DT = Convert.ToDateTime(this.txt_stDate.Text).ToString("yyyy-MM-dd"), TO_DT = Convert.ToDateTime(this.txt_enDate.Text).ToString("yyyy-MM-dd") };

                //SystemCodeVo grpCdVo = this.combo_GRP_NM.SelectedItem as SystemCodeVo;
                //if (grpCdVo != null)
                //{
                //    vo.SL_CO_CD = grpCdVo.CO_NO;
                //    vo.SL_CO_NM = grpCdVo.CO_NM;
                //}
                //else
                //{
                    vo.SL_CO_CD = orgVo.SL_CO_CD;
                    vo.SL_CO_NM = orgVo.SL_CO_NM;
                    //WinUIMessageBox.Show("[거래처]을 다시 선택 해주세요", "[조회 조건]" + title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                    //this.MSG.Text = "[거래처]을 다시 선택 해주세요";
                    //return;
                //}

                this.search_title.Text = "[조회 조건]   " + "기간 : " + Convert.ToDateTime(this.txt_stDate.Text).ToString("yyyy-MM-dd") + "~" + Convert.ToDateTime(this.txt_enDate.Text).ToString("yyyy-MM-dd") + ", 거래처 : " + orgVo.SL_CO_CD + "/" + orgVo.SL_CO_NM;
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s2217/popup", new StringContent(JsonConvert.SerializeObject(vo), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.ViewJOB_ITEMEdit.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SaleVo>>(await response.Content.ReadAsStringAsync()).Cast<SaleVo>().ToList();
                    }
                }

                    //this.ViewJOB_ITEMEdit.ItemsSource = saleOrderClient.S2217SelectDtlViewList(vo);
                }
            catch (Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
                this.MSG.Text = eLog.Message;
                return;
            }
        }

        #region Functon (OKButton_Click, CancelButton_Click)
        private async void OKButton_Click(object sender, RoutedEventArgs e)
        {
            bool isMsg = false;
            IList<SaleVo> checkList = (IList<SaleVo>)this.ViewJOB_ITEMEdit.ItemsSource;
            IList<SaleVo> saveList = new List<SaleVo>();
            int nSize = checkList.Count;
            if (nSize <= 0)
            {
                WinUIMessageBox.Show("데이터가 존재 하지 않습니다", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.MSG.Text = "데이터가 존재 하지 않습니다";
                return;
            }


            //MessageBoxResult result = WinUIMessageBox.Show("정말로 저장 하시겠습니까?", "[저장]" + title, MessageBoxButton.YesNo, MessageBoxImage.Question);
            //if (result == MessageBoxResult.Yes)
            //{
            //SaleVo resultVo;
            SaleVo tmpVo;
            for (int x = 0; x < nSize; x++)
            {
                tmpVo = checkList[x];
                if (tmpVo.isCheckd)
                {
                    isMsg = true;
                    tmpVo.SL_BIL_RTN_NO = this.orgVo.SL_BIL_RTN_NO;
                    tmpVo.CRE_USR_ID = SystemProperties.USER;
                    tmpVo.UPD_USR_ID = SystemProperties.USER;
                    tmpVo.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
                    tmpVo.SL_RTN_ITM_CD = tmpVo.ITM_CD;
                    saveList.Add(tmpVo);

                    //        resultVo = saleOrderClient.S2217InsertDtl(tmpVo);
                    //        if (!resultVo.isSuccess)
                    //        {
                    //            //실패
                    //            WinUIMessageBox.Show(resultVo.Message, "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
                    //            this.MSG.Text = resultVo.Message;
                    //            return;
                    //        }
                    //        saveList.Add(tmpVo);
                    //        tmpVo.isCheckd = false;
                    //    }
                }
            }

            if (isMsg)
            {
                int _Num = 0;
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s2217/dtl/i", new StringContent(JsonConvert.SerializeObject(saveList), System.Text.Encoding.UTF8, "application/json")))
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
                   
                        WinUIMessageBox.Show("[총 계수 : " + saveList.Count + "] - 저장 완료 되었습니다", "[수정]" + title, MessageBoxButton.OK, MessageBoxImage.Information);
                        //
                        this.OKButton.IsEnabled = false;

                        this.DialogResult = true;
                        this.Close();
                    }
                }
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


        private void GridColumn_Validate(object sender, DevExpress.Xpf.Grid.GridCellValidationEventArgs e)
        {
            try
            {
                SaleVo masterDomain = (SaleVo)ViewJOB_ITEMEdit.GetFocusedRow();
                bool rmnQty = (e.Column.FieldName.ToString().Equals("SL_RTN_ITM_QTY") ? true : false);
                bool genRmk = (e.Column.FieldName.ToString().Equals("SL_RTN_ITM_RMK") ? true : false);

                bool slRtnNm = (e.Column.FieldName.ToString().Equals("SL_RTN_NM") ? true : false);
                bool slBadItmNm = (e.Column.FieldName.ToString().Equals("SL_BAD_ITM_NM") ? true : false);

                //
                if (rmnQty)
                {
                    if (e.IsValid)
                    {
                        if (string.IsNullOrEmpty(masterDomain.SL_RTN_ITM_QTY + ""))
                        {
                            masterDomain.SL_RTN_ITM_QTY = 0;
                        }
                        //
                        if (!masterDomain.SL_RTN_ITM_QTY.Equals((e.Value == null ? "" : e.Value.ToString())))
                        {
                            if (int.Parse(e.Value.ToString()) > masterDomain.SL_ITM_QTY)
                            {
                                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                                e.ErrorContent = "[수량]" + masterDomain.SL_ITM_QTY + " 이상으로 입력 하실수 없습니다.";
                                this.MSG.Text = e.ErrorContent.ToString();
                                e.SetError(e.ErrorContent, e.ErrorType);
                                return;
                            }
                            masterDomain.SL_RTN_ITM_QTY = Convert.ToInt32(e.Value);

                            masterDomain.SL_ITM_QTY = Convert.ToInt64(masterDomain.TMP_ITM_QTY) - Convert.ToInt64(masterDomain.SL_RTN_ITM_QTY);

                            masterDomain.CRE_USR_ID = SystemProperties.USER;
                            masterDomain.UPD_USR_ID = SystemProperties.USER;
                            masterDomain.SL_RTN_ITM_CD = masterDomain.ITM_CD;

                            masterDomain.isCheckd = true;
                            this.OKButton.IsEnabled = true;



                        }
                    }
                }
                //
                //
                if (genRmk)
                {
                    if (e.IsValid)
                    {
                        //if (string.IsNullOrEmpty(masterDomain.BANK_RMK))
                        if (e.Value == null)
                        {
                            masterDomain.SL_RTN_ITM_RMK = "";
                        }
                        //
                        if (!masterDomain.SL_RTN_ITM_RMK.Equals((e.Value == null ? "" : e.Value.ToString())))
                        {
                            masterDomain.CRE_USR_ID = SystemProperties.USER;
                            masterDomain.UPD_USR_ID = SystemProperties.USER;
                            masterDomain.SL_RTN_ITM_CD = masterDomain.ITM_CD;

                            masterDomain.isCheckd = true;
                            this.OKButton.IsEnabled = true;
                        }
                    }
                }
                //
                //
                if (slRtnNm)
                {
                    if (e.IsValid)
                    {
                        SystemCodeVo bankIoDao = null;
                        if (e.Value != null)
                        {
                            bankIoDao = this.lue_SL_RTN_NM.GetItemFromValue(e.Value) as SystemCodeVo;
                            if (masterDomain.SL_RTN_CD == null)
                            {
                                masterDomain.SL_RTN_CD = string.Empty;
                            }
                            if (masterDomain.SL_RTN_NM == null)
                            {
                                masterDomain.SL_RTN_NM = string.Empty;
                            }
                            //
                            if (!masterDomain.SL_RTN_NM.Equals((e.Value == null ? "" : e.Value.ToString())))
                            {
                                masterDomain.CRE_USR_ID = SystemProperties.USER;
                                masterDomain.UPD_USR_ID = SystemProperties.USER;
                                masterDomain.SL_RTN_ITM_CD = masterDomain.ITM_CD;

                                masterDomain.isCheckd = true;
                                this.OKButton.IsEnabled = true;
                            }
                            //
                            if (bankIoDao != null)
                            {
                                masterDomain.SL_RTN_NM = bankIoDao.CLSS_DESC;
                                masterDomain.SL_RTN_CD = bankIoDao.CLSS_CD;
                            }
                        }
                        //
                    }
                }
                //
                //
                if (slBadItmNm)
                {
                    if (e.IsValid)
                    {
                        SystemCodeVo bankIoDao = null;
                        if (e.Value != null)
                        {
                            bankIoDao = this.lue_SL_BAD_ITM_NM.GetItemFromValue(e.Value) as SystemCodeVo;
                            if (masterDomain.SL_BAD_ITM_CD == null)
                            {
                                masterDomain.SL_BAD_ITM_CD = string.Empty;
                            }
                            if (masterDomain.SL_BAD_ITM_NM == null)
                            {
                                masterDomain.SL_BAD_ITM_NM = string.Empty;
                            }
                            //
                            if (!masterDomain.SL_BAD_ITM_NM.Equals((e.Value == null ? "" : e.Value.ToString())))
                            {
                                masterDomain.CRE_USR_ID = SystemProperties.USER;
                                masterDomain.UPD_USR_ID = SystemProperties.USER;
                                masterDomain.SL_RTN_ITM_CD = masterDomain.ITM_CD;

                                masterDomain.isCheckd = true;
                                this.OKButton.IsEnabled = true;
                            }
                            //
                            if (bankIoDao != null)
                            {
                                masterDomain.SL_BAD_ITM_NM = bankIoDao.CLSS_DESC;
                                masterDomain.SL_BAD_ITM_CD = bankIoDao.CLSS_CD;
                            }
                        }
                        //
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
                this.MSG.Text = e.ErrorContent.ToString();
                e.SetError(e.ErrorContent, e.ErrorType);
                return;
            }
        }

        private void viewPage1EditView_HiddenEditor(object sender, DevExpress.Xpf.Grid.EditorEventArgs e)
        {
            this.viewJOB_ITEMView.CommitEditing();

            //bool rmnQty = (e.Column.FieldName.ToString().Equals("SL_RTN_ITM_QTY") ? true : false);
            //bool genRmk = (e.Column.FieldName.ToString().Equals("SL_RTN_ITM_RMK") ? true : false);

            //bool slRtnNm = (e.Column.FieldName.ToString().Equals("SL_RTN_NM") ? true : false);
            //bool slBadItmNm = (e.Column.FieldName.ToString().Equals("SL_BAD_ITM_NM") ? true : false);


            //int rowHandle = this.viewJOB_ITEMView.FocusedRowHandle + 1;

            //if (rmnQty)
            //{
            //    this.ViewJOB_ITEMEdit.CurrentColumn = this.ViewJOB_ITEMEdit.Columns["SL_RTN_ITM_QTY"];
            //}
            //else if (genRmk)
            //{
            //    this.ViewJOB_ITEMEdit.CurrentColumn = this.ViewJOB_ITEMEdit.Columns["SL_RTN_ITM_RMK"];
            //}
            //else if (slRtnNm)
            //{
            //    this.ViewJOB_ITEMEdit.CurrentColumn = this.ViewJOB_ITEMEdit.Columns["SL_RTN_NM"];
            //}
            //else if (slBadItmNm)
            //{
            //    this.ViewJOB_ITEMEdit.CurrentColumn = this.ViewJOB_ITEMEdit.Columns["SL_BAD_ITM_NM"];
            //}

            //this.ViewJOB_ITEMEdit.RefreshRow(rowHandle - 1);
            //this.viewJOB_ITEMView.FocusedRowHandle = rowHandle;
        }


        private void CheckEdit_Checked(object sender, RoutedEventArgs e)
        {
            CheckEdit checkEdit = sender as CheckEdit;
            SaleVo tmpImsi;
            for (int x = 0; x < this.ViewJOB_ITEMEdit.VisibleRowCount; x++)
            {
                int rowHandle = this.ViewJOB_ITEMEdit.GetRowHandleByVisibleIndex(x);
                if (rowHandle > -1)
                {
                    tmpImsi = this.ViewJOB_ITEMEdit.GetRow(rowHandle) as SaleVo;
                    if (checkEdit.IsChecked == true)
                    {
                        tmpImsi.isCheckd = true;
                        //
                        this.OKButton.IsEnabled = true;
                    }
                    else
                    {
                        tmpImsi.isCheckd = false;
                    }

                }
            }
        }

        private void PART_Editor_Checked(object sender, RoutedEventArgs e)
        {
            this.OKButton.IsEnabled = true;
        }




        public async void SYSTEM_CODE_VO()
        {
            //this.combo_GRP_NM.ItemsSource = SystemProperties.CUSTOMER_CODE_VO("AR", null);
            //using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s143", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { DELT_FLG = "N", CO_TP_CD = "AR", AREA_CD = SystemProperties.USER_VO.EMPE_PLC_NM, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
            //{
            //    if (response.IsSuccessStatusCode)
            //    {
            //        this.combo_GRP_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
            //    }
            //}

            //this.combo_SL_BAD_ITM_NM.ItemsSource = SystemProperties.SYSTEM_CODE_VO("Q-002");
            //this.lue_SL_BAD_ITM_NM.ItemsSource = SystemProperties.SYSTEM_CODE_VO("Q-002");
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "Q-002"))
            {
                if (response.IsSuccessStatusCode)
                {
                    IList<SystemCodeVo> tmpList = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();

                    this.combo_SL_BAD_ITM_NM.ItemsSource = tmpList;
                    this.lue_SL_BAD_ITM_NM.ItemsSource = tmpList;
                }
            }

            //this.combo_SL_RTN_NM.ItemsSource = SystemProperties.SYSTEM_CODE_VO("S-033");
            //this.lue_SL_RTN_NM.ItemsSource = SystemProperties.SYSTEM_CODE_VO("S-033");
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "S-033"))
            {
                if (response.IsSuccessStatusCode)
                {
                    IList<SystemCodeVo> tmpList = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                    this.combo_SL_RTN_NM.ItemsSource = tmpList;
                    this.lue_SL_RTN_NM.ItemsSource = tmpList;
                }
            }
        }

    }
}
