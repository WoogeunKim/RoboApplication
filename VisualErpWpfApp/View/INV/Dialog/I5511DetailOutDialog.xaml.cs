using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.WindowsUI;
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
    public partial class I5511DetailOutDialog : DXWindow
    {
        //외주 입고
        //private static InvServiceClient invClient = SystemProperties.InvClient;
        private InvVo orgVo;
        private string _title = "품목 입고 관리";

        public I5511DetailOutDialog(InvVo vo)
        {
            InitializeComponent();

            this.orgVo = vo;

            this.txt_stDate.Text = System.DateTime.Now.AddYears(-2).ToString("yyyy-MM-dd");
            this.txt_enDate.Text = System.DateTime.Now.ToString("yyyy-MM-dd");

            //this.txt_stDate.Text = vo.FM_DT;
            //this.txt_enDate.Text = vo.TO_DT;

            //IList<CustomerCodeDao> tmpList = SystemProperties.CUSTOMER_CODE_VO("OR");
            //tmpList.Insert(0, new CustomerCodeDao() { CO_NO = "", CO_NM = "" });
            //IList<CodeDao> tmpList = new List<CodeDao>();
            //tmpList.Insert(0, new CodeDao() { CLSS_CD = "", CLSS_DESC = "" });
            //tmpList.Insert(1, new CodeDao() { CLSS_CD = "PO", CLSS_DESC = "국내" });
            //tmpList.Insert(2, new CodeDao() { CLSS_CD = "IV", CLSS_DESC = "수입" });

            //this.combo_GRP_NM.ItemsSource = tmpList;
            //this.combo_GRP_NM.Text = "";

            SYSTEM_CODE_VO();
            //this.lue_INAUD_ORG_NM.ItemsSource = SystemProperties.SYSTEM_CODE_VO("L-100");
            //this.combo_INAUD_ORG_NM.ItemsSource = SystemProperties.SYSTEM_CODE_VO("L-100");

            //searchItem();


            this.btn_reset.Click += btn_reset_Click;
            this.btn_apply.Click += btn_apply_Click;

            //this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
            this.OKButton.Click += new RoutedEventHandler(OKButton_Click);
            this.CancelButton.Click += new RoutedEventHandler(CancelButton_Click);


            this.OKButton.IsEnabled = false;
        }

        void btn_reset_Click(object sender, RoutedEventArgs e)
        {
            searchItem();

        }

        private void btn_apply_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //if (this.check_IN_QTY.IsChecked == true)
                //{
                //    if (string.IsNullOrEmpty(this.text_IN_QTY.Text))
                //    {
                //        WinUIMessageBox.Show("[입고수량]입력 값이 맞지 안습니다", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                //        this.MSG.Text = "[입고수량]입력 값이 맞지 안습니다";
                //        return;
                //    }
                //}

                if (this.check_INAUD_ORG_NM.IsChecked == true)
                {
                    if (string.IsNullOrEmpty(this.combo_INAUD_ORG_NM.Text))
                    {
                        WinUIMessageBox.Show("[입고창고]입력 값이 맞지 안습니다", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                        this.MSG.Text = "[입고창고]입력 값이 맞지 안습니다";
                        return;
                    }


                }



                IList<InvVo> _checkList = ((IList<InvVo>)this.ViewJOB_ITEMEdit.ItemsSource).Where(w => w.isCheckd == true).ToList<InvVo>();
                InvVo _tmpVo;
                for (int x = 0; x < _checkList.Count; x++)
                {
                    _tmpVo = _checkList[x];
                    //if (this.check_IN_QTY.IsChecked == true)
                    //{
                    //    _tmpVo.IN_QTY = Convert.ToInt32(string.IsNullOrEmpty(this.text_IN_QTY.Text) ? "0" : this.text_IN_QTY.Text);

                    //    _tmpVo.isCheckd = true;
                    //    this.OKButton.IsEnabled = true;
                    //}

                    if (this.check_INAUD_ORG_NM.IsChecked == true)
                    {
                        SystemCodeVo inaudOrgVo = this.combo_INAUD_ORG_NM.SelectedItem as SystemCodeVo;
                        if (inaudOrgVo != null)
                        {
                            _tmpVo.INAUD_ORG_NM = inaudOrgVo.CLSS_DESC;
                            _tmpVo.INAUD_ORG_NO = inaudOrgVo.CLSS_CD;

                            _tmpVo.isCheckd = true;
                            this.OKButton.IsEnabled = true;
                        }
                    }

                    if (this.check_INAUD_RMK.IsChecked == true)
                    {
                        _tmpVo.INAUD_RMK = this.text_INAUD_RMK.Text;

                        _tmpVo.isCheckd = true;
                        this.OKButton.IsEnabled = true;
                    }
                }
                this.ViewJOB_ITEMEdit.RefreshData();
            }
            catch (Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[에러]" + _title, MessageBoxButton.OK, MessageBoxImage.Error);
                this.MSG.Text = eLog.Message;
                return;
            }
        }



        private async void searchItem()
        {
            try
            {
                InvVo vo = new InvVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD,  AREA_CD = SystemProperties.USER_VO.EMPE_PLC_NM, FM_DT = Convert.ToDateTime(this.txt_stDate.Text).ToString("yyyy-MM-dd"), TO_DT = Convert.ToDateTime(this.txt_enDate.Text).ToString("yyyy-MM-dd") };

                this.search_title.Text = "[조회 조건]   " + "기간 : " + Convert.ToDateTime(this.txt_stDate.Text).ToString("yyyy-MM-dd") + "~" + Convert.ToDateTime(this.txt_enDate.Text).ToString("yyyy-MM-dd");

                //CodeDao grpCdVo = this.combo_GRP_NM.SelectedItem as CodeDao;
                //if (grpCdVo != null)
                //{
                //    vo.INAUD_TP_NM = (string.IsNullOrEmpty(grpCdVo.CLSS_CD) ? null : grpCdVo.CLSS_CD);
                //}
                //else
                //{
                //    WinUIMessageBox.Show("[부서 명]을 다시 선택 해주세요", "[조회 조건]품목 입고 관리", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                //    return;
                //}

                //this.ViewJOB_ITEMEdit.ItemsSource = invClient.I5511SelectDtlOutList(vo);

                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("i5511/popup/out", new StringContent(JsonConvert.SerializeObject(vo), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.ViewJOB_ITEMEdit.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<InvVo>>(await response.Content.ReadAsStringAsync()).Cast<InvVo>().ToList();
                    }
                }

            }
            catch (Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[에러]" + _title, MessageBoxButton.OK, MessageBoxImage.Error);
                this.MSG.Text = eLog.Message;
                return;
            }
        }

        #region Functon (OKButton_Click, CancelButton_Click)
        private async void OKButton_Click(object sender, RoutedEventArgs e)
        {
            bool isMsg = false;
            IList<InvVo> checkList = (IList<InvVo>)this.ViewJOB_ITEMEdit.ItemsSource;
            List<InvVo> saveList = new List<InvVo>();
            int nSize = checkList.Count;
            if (nSize <= 0)
            {
                WinUIMessageBox.Show("데이터가 존재 하지 않습니다.", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.MSG.Text = "데이터가 존재 하지 않습니다.";
                return;
            }


            MessageBoxResult result = WinUIMessageBox.Show("정말로 저장 하시겠습니까?", "[저장]" + _title, MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                InvVo tmpVo;
                for (int x = 0; x < nSize; x++)
                {
                    tmpVo = checkList[x];
                    if (tmpVo.isCheckd)
                    {
                        if (string.IsNullOrEmpty(tmpVo.INAUD_ORG_NO))
                        {
                            WinUIMessageBox.Show("[입고 창고] 입력 값이 맞지 않습니다", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                            this.MSG.Text = "[입고 창고] 입력 값이 맞지 않습니다";
                            return;
                        }


                        isMsg = true;
                        //
                        InvVo insertVo = new InvVo()
                        {
                              INSRL_NO = this.orgVo.INSRL_NO
                            , INAUD_CD = "RGH"
                            , LOC_CD = tmpVo.INAUD_ORG_NO
                            , CO_NO = tmpVo.CO_NO
                            , ITM_CD = tmpVo.ITM_CD
                            , INAUD_QTY = tmpVo.IN_QTY
                            , ORD_NO = tmpVo.INSRL_NO
                            , ORD_SEQ = tmpVo.INSRL_SEQ
                            , INAUD_RMK = tmpVo.INAUD_RMK
                            , IMP_ITM_PRC = tmpVo.INAUD_PRC
                            , IMP_ITM_AMT = tmpVo.INAUD_AMT
                                //, IMP_ITM_AMT = tmpVo.INAUD_AMT
                            , ROUT_CD = tmpVo.ROUT_CD
                            , CRE_USR_ID = SystemProperties.USER
                            , UPD_USR_ID = SystemProperties.USER
                            , CHNL_CD = SystemProperties.USER_VO.CHNL_CD
                        };

                        //resultVo = invClient.I5511InsertPurDtl(insertVo);
                        //if (!resultVo.isSuccess)
                        //{
                        //    //실패
                        //    WinUIMessageBox.Show(resultVo.Message, "[에러]품목 입고 관리", MessageBoxButton.OK, MessageBoxImage.Error);
                        //    return;
                        //}
                        saveList.Add(insertVo);
                        tmpVo.isCheckd = false;
                    }
                }


                if (isMsg)
                {
                    int _Num = 0;
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("i5511/dtl/i", new StringContent(JsonConvert.SerializeObject(saveList.ToArray()), System.Text.Encoding.UTF8, "application/json")))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string resultMsg = await response.Content.ReadAsStringAsync();
                            if (int.TryParse(resultMsg, out _Num) == false)
                            {
                                //실패
                                WinUIMessageBox.Show(resultMsg, _title, MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                        }
                        //InvVo resultVo = invClient.I5511InsertDtl_Transaction(saveList.ToArray());
                        //if (!resultVo.isSuccess)
                        //{
                        //    //실패
                        //    WinUIMessageBox.Show(resultVo.Message, "[에러]" + _title, MessageBoxButton.OK, MessageBoxImage.Error);
                        //    this.MSG.Text = resultVo.Message;
                        //    return;
                        //}

                        WinUIMessageBox.Show("[총 계수 : " + saveList.Count + "] - 저장 완료 되었습니다", "[수정]" + _title, MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    //
                    this.OKButton.IsEnabled = false;
                }
            }

            // 종료 여부
            if (isMsg)
            {
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


        private void GridColumn_Validate(object sender, DevExpress.Xpf.Grid.GridCellValidationEventArgs e)
        {
            try
            {
                InvVo masterDomain = (InvVo)ViewJOB_ITEMEdit.GetFocusedRow();
                bool inaudPrc = (e.Column.FieldName.ToString().Equals("INAUD_PRC") ? true : false);
                //bool inaudQty = (e.Column.FieldName.ToString().Equals("IN_QTY") ? true : false);
                bool inQty = (e.Column.FieldName.ToString().Equals("IN_QTY") ? true : false);
                bool inaudOrgNm = (e.Column.FieldName.ToString().Equals("INAUD_ORG_NM") ? true : false);
                bool inaudRmk = (e.Column.FieldName.ToString().Equals("INAUD_RMK") ? true : false);
                //bool isCheckd = (e.Column.FieldName.ToString().Equals("isCheckd") ? true : false);

                //InvVo resultVo;
                //InvVo insertVo;
                //
                //if (inaudQty)
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
                //            //float? tmpInt = float.Parse(masterDomain.INV_RMN_QTY.ToString()) - float.Parse(e.Value.ToString());

                //            //if (tmpInt <= 0 )
                //            //{
                //            //    e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                //            //    e.ErrorContent = "[잔량]" + tmpInt + " -값은 입력 하실수 없습니다";
                //            //    e.SetError(e.ErrorContent, e.ErrorType);
                //            //    return;
                //            //}

                //            masterDomain.IN_QTY = e.Value.ToString();

                //            masterDomain.RMN_QTY = Convert.ToDecimal(masterDomain.TMP_RMK_QTY) - Convert.ToDecimal(masterDomain.IN_QTY);

                //            masterDomain.INAUD_AMT = Convert.ToDecimal(masterDomain.IN_QTY) * Convert.ToDecimal(masterDomain.INAUD_PRC);

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
                if (inaudPrc)
                {
                    if (e.IsValid)
                    {
                        if (string.IsNullOrEmpty(masterDomain.INAUD_PRC + ""))
                        {
                            masterDomain.INAUD_PRC = 0;
                        }
                        //
                        if (!masterDomain.INAUD_PRC.Equals((e.Value == null ? "" : e.Value.ToString())))
                        {
                            //float? tmpInt = float.Parse(masterDomain.INV_RMN_QTY.ToString()) - float.Parse(e.Value.ToString());

                            //if (tmpInt <= 0 )
                            //{
                            //    e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                            //    e.ErrorContent = "[잔량]" + tmpInt + " -값은 입력 하실수 없습니다";
                            //    e.SetError(e.ErrorContent, e.ErrorType);
                            //    return;
                            //}

                            masterDomain.INAUD_PRC = e.Value.ToString();

                            //masterDomain.RMN_QTY = Convert.ToDecimal(masterDomain.TMP_RMK_QTY) - Convert.ToDecimal(masterDomain.IN_QTY);
                            //masterDomain.INAUD_AMT = Convert.ToDecimal(masterDomain.IN_QTY) * Convert.ToDecimal(masterDomain.INAUD_PRC);
                            masterDomain.INAUD_AMT = Convert.ToDecimal(masterDomain.IN_QTY) * Convert.ToDecimal(masterDomain.INAUD_PRC);
                            masterDomain.isCheckd = true;
                            this.OKButton.IsEnabled = true;
                            //insertVo = new InvVo() {
                            //          INSRL_NO = this.orgVo.INSRL_NO
                            //        , INAUD_CD = masterDomain.INAUD_CD
                            //        , ITM_CD = masterDomain.ITM_CD
                            //        , CO_UT_PRC = masterDomain.CO_UT_PRC
                            //        , PUR_AMT = masterDomain.PUR_AMT
                            //        , PUR_ORD_NO = masterDomain.PUR_ORD_NO
                            //        , PUR_ORD_SEQ = masterDomain.PUR_ORD_SEQ
                            //        , RMN_QTY = int.Parse(e.Value.ToString())
                            //        , CRE_USR_ID = SystemProperties.USER
                            //        , UPD_USR_ID = SystemProperties.USER };

                            //resultVo = invClient.I5511InsertPurDtl(insertVo);
                            //if (!resultVo.isSuccess)
                            //{
                            //    //실패
                            //    WinUIMessageBox.Show(resultVo.Message, "[에러]품목 입고 관리", MessageBoxButton.OK, MessageBoxImage.Error);
                            //    return;
                            //}
                        }
                    }
                }
                else if (inaudOrgNm)
                {
                    if (e.IsValid)
                    {
                        if (string.IsNullOrEmpty(masterDomain.INAUD_ORG_NO))
                        {
                            masterDomain.INAUD_ORG_NM = "";
                            masterDomain.INAUD_ORG_NO = "";
                        }
                        //
                        if (!masterDomain.INAUD_ORG_NO.Equals((e.Value == null ? "" : e.Value.ToString())))
                        {
                            SystemCodeVo bankIoDao = this.lue_INAUD_ORG_NM.GetItemFromValue(e.Value) as SystemCodeVo;
                            //
                            if (bankIoDao != null)
                            {
                                masterDomain.INAUD_ORG_NO = bankIoDao.CLSS_CD;
                                masterDomain.INAUD_ORG_NM = bankIoDao.CLSS_DESC;
                            }

                            masterDomain.isCheckd = true;
                            this.OKButton.IsEnabled = true;
                        }
                    }
                }
                else if (inaudRmk)
                {
                    if (e.IsValid)
                    {
                        if (string.IsNullOrEmpty(masterDomain.INAUD_RMK))
                        {
                            masterDomain.INAUD_RMK = "";
                        }
                        //
                        if (!masterDomain.INAUD_RMK.Equals((e.Value == null ? "" : e.Value.ToString())))
                        {
                            masterDomain.INAUD_RMK = e.Value.ToString();
                            masterDomain.isCheckd = true;
                            this.OKButton.IsEnabled = true;
                        }
                    }
                }
                //else if (isCheckd)
                //{
                //    this.OKButton.IsEnabled = true;
                //}
                else if (inQty)
                {
                    if (e.IsValid)
                    {
                        if (string.IsNullOrEmpty(masterDomain.IN_QTY + ""))
                        {
                            masterDomain.IN_QTY = 0;
                        }

                        if (!masterDomain.IN_QTY.Equals((e.Value == null ? "" : e.Value.ToString())))
                        {


                            //masterDomain.RMN_QTY = Convert.ToDecimal(masterDomain.INAUD_QTY) - Convert.ToDecimal(e.Value);
                            masterDomain.RMN_QTY = Convert.ToDecimal(masterDomain.TMP_RMN_QTY) - Convert.ToDecimal(e.Value);

                            //if (Convert.ToDecimal(masterDomain.RMN_QTY) < 0)
                            //{
                            //    WinUIMessageBox.Show("입고수량이 출고수량보다 많습니다.", "[에러]" + _title, MessageBoxButton.OK, MessageBoxImage.Error);
                            //    searchItem();

                            //}
                            //else
                            //{
                            masterDomain.IN_QTY = e.Value.ToString();
                            //}

                            masterDomain.INAUD_AMT = Convert.ToDecimal(masterDomain.IN_QTY) * Convert.ToDecimal(masterDomain.INAUD_PRC);
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
                this.MSG.Text = eLog.Message;
                e.SetError(e.ErrorContent, e.ErrorType);
                return;
            }
        }

        private void viewPage1EditView_HiddenEditor(object sender, DevExpress.Xpf.Grid.EditorEventArgs e)
        {
            this.viewJOB_ITEMView.CommitEditing();

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


        private void CheckEdit_Checked(object sender, RoutedEventArgs e)
        {
            CheckEdit checkEdit = sender as CheckEdit;
            InvVo tmpImsi;
            for (int x = 0; x < this.ViewJOB_ITEMEdit.VisibleRowCount; x++)
            {
                int rowHandle = this.ViewJOB_ITEMEdit.GetRowHandleByVisibleIndex(x);
                if (rowHandle > -1)
                {
                    tmpImsi = this.ViewJOB_ITEMEdit.GetRow(rowHandle) as InvVo;
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

        private void ButtonInfo_Click(object sender, RoutedEventArgs e)
        {
            InvVo selVo = (InvVo)ViewJOB_ITEMEdit.GetFocusedRow();
            if (selVo != null)
            {
                selVo.isCheckd = true;

                if (Convert.ToInt32(selVo.IN_QTY) < 0)
                {
                    selVo.IN_QTY = 0;
                }
                else
                {
                    //selVo.IN_QTY = selVo.RMN_QTY;
                    //selVo.CTRT_RMN_QTY = 0;


                    selVo.IN_QTY = selVo.INAUD_QTY;

                }
                //selVo.IMP_ITM_AMT = Convert.ToInt32(selVo.IMP_INV_QTY) * Convert.ToDouble(selVo.IMP_ITM_PRC) / 1000;
                //selVo.RMN_QTY = Convert.ToDecimal(selVo.TMP_RMK_QTY) - Convert.ToDecimal(selVo.IN_QTY);

                selVo.RMN_QTY = Convert.ToDecimal(selVo.INAUD_QTY) - Convert.ToDecimal(selVo.IN_QTY);
                selVo.INAUD_AMT = Convert.ToDecimal(selVo.IN_QTY) * Convert.ToDecimal(selVo.INAUD_PRC);

                this.OKButton.IsEnabled = true;

                this.viewJOB_ITEMView.CommitEditing();
                this.ViewJOB_ITEMEdit.RefreshData();
                this.viewJOB_ITEMView.FocusedRowHandle = this.viewJOB_ITEMView.FocusedRowHandle;
            }
        }


        public async void SYSTEM_CODE_VO()
        {
            //this.lue_INAUD_ORG_NM.ItemsSource = SystemProperties.SYSTEM_CODE_VO("L-100");
            //this.combo_INAUD_ORG_NM.ItemsSource = SystemProperties.SYSTEM_CODE_VO("L-100");
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "L-100"))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.lue_INAUD_ORG_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                    this.combo_INAUD_ORG_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                }
            }
        }


    }
}
