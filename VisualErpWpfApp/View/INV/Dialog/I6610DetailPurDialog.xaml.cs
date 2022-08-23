using AquilaErpWpfApp3.Util;
using DevExpress.Data;
using DevExpress.Mvvm.Native;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
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

namespace AquilaErpWpfApp3.View.INV.Dialog
{
    public partial class I6610DetailPurDialog : DXWindow
    {
        //private static InvServiceClient invClient = SystemProperties.InvClient;
        private InvVo orgVo;
        private string _title = "품목 가입고 관리";

        public I6610DetailPurDialog(InvVo vo)
        {
            InitializeComponent();

            this.orgVo = vo;

            this.txt_stDate.Text = System.DateTime.Now.AddYears(-2).ToString("yyyy-MM-dd");
            this.txt_enDate.Text = System.DateTime.Now.ToString("yyyy-MM-dd");

            //this.txt_stDate.Text = vo.FM_DT;
            //this.txt_enDate.Text = vo.TO_DT;

            SYSTEM_CODE_VO();

            searchItem();


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
                        WinUIMessageBox.Show("[가입고창고]입력 값이 맞지 안습니다", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                        this.MSG.Text = "[가입고창고]입력 값이 맞지 안습니다";
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
                        _tmpVo.PUR_RMK = this.text_INAUD_RMK.Text;

                        _tmpVo.isCheckd = true;
                        this.OKButton.IsEnabled = true;
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

        private async void searchItem()
        {
            try
            {
                InvVo vo = new InvVo() { FM_DT = Convert.ToDateTime(this.txt_stDate.Text).ToString("yyyy-MM-dd"), TO_DT = Convert.ToDateTime(this.txt_enDate.Text).ToString("yyyy-MM-dd") };

                SystemCodeVo grpCdVo = this.combo_GRP_NM.SelectedItem as SystemCodeVo;
                if (grpCdVo != null)
                {
                    vo.CO_NO = (string.IsNullOrEmpty(grpCdVo.CO_NO) ? null : grpCdVo.CO_NO);
                    vo.CO_CD = (string.IsNullOrEmpty(grpCdVo.CO_NO) ? null : grpCdVo.CO_NO);
                    vo.CO_NM = (string.IsNullOrEmpty(grpCdVo.CO_NM) ? null : grpCdVo.CO_NM);
                }

                vo.AREA_CD = SystemProperties.USER_VO.EMPE_PLC_NM;
                vo.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
                //else
                //{
                //    WinUIMessageBox.Show("[부서 명]을 다시 선택 해주세요", "[조회 조건]품목 입고 관리", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                //    return;
                //}
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("i6610/pur", new StringContent(JsonConvert.SerializeObject(vo), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.ViewJOB_ITEMEdit.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<InvVo>>(await response.Content.ReadAsStringAsync()).Cast<InvVo>().ToList();
                    }
                }
                    //this.ViewJOB_ITEMEdit.ItemsSource = invClient.I6610SelectDtlPurList(vo);
            }
            catch (Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[에러]" + _title, MessageBoxButton.OK, MessageBoxImage.Error);
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
                        isMsg = true;
                        //
                        InvVo insertVo = new InvVo()
                        {
                              PUR_ORD_NO = tmpVo.PUR_ORD_NO
                            , PUR_SEQ = tmpVo.PUR_ORD_SEQ
                            , CO_CD   = tmpVo.CO_CD
                            , ITM_CD = tmpVo.ITM_CD
                            , LOC_CD = tmpVo.INAUD_ORG_NO
                            , CO_UT_PRC = tmpVo.CO_UT_PRC
                            , ITM_QTY = tmpVo.ITM_QTY
                            , ITM_RMK = tmpVo.PUR_RMK
                            , AREA_CD = SystemProperties.USER_VO.EMPE_PLC_NM
                            , CRE_USR_ID = SystemProperties.USER
                            , UPD_USR_ID = SystemProperties.USER
                            , CHNL_CD = SystemProperties.USER_VO.CHNL_CD
                        };

                        //resultVo = invClient.I5511InsertPurDtl(insertVo);
                        //if (!resultVo.isSuccess)
                        //{
                        //    //실패
                        //    WinUIMessageBox.Show(resultVo.Message, "[에러]" + _title, MessageBoxButton.OK, MessageBoxImage.Error);
                        //    return;
                        //}
                        saveList.Add(insertVo);
                        tmpVo.isCheckd = false;
                    }
                }

                if (isMsg)
                {
                    int _Num = 0;
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("i6610/mst/i", new StringContent(JsonConvert.SerializeObject(saveList.ToArray()), System.Text.Encoding.UTF8, "application/json")))
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
                        //InvVo resultVo = invClient.I6610InsertPurMst_Transaction(saveList.ToArray());
                        //if (!resultVo.isSuccess)
                        //{
                        //    //실패
                        //    WinUIMessageBox.Show(resultVo.Message, "[에러]" + _title, MessageBoxButton.OK, MessageBoxImage.Error);
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
                bool itmQty = (e.Column.FieldName.ToString().Equals("ITM_QTY") ? true : false);
                bool inaudRmk = (e.Column.FieldName.ToString().Equals("PUR_RMK") ? true : false);
                bool inaudOrgNm = (e.Column.FieldName.ToString().Equals("INAUD_ORG_NM") ? true : false);
                bool coUtPrc = (e.Column.FieldName.ToString().Equals("CO_UT_PRC") ? true : false);
                //bool isCheckd = (e.Column.FieldName.ToString().Equals("isCheckd") ? true : false);

                //InvVo resultVo;
                //InvVo insertVo;
                //
                if (itmQty)
                {
                    if (e.IsValid)
                    {
                        if (string.IsNullOrEmpty(masterDomain.ITM_QTY + ""))
                        {
                            masterDomain.ITM_QTY = 0;
                        }
                        //
                        if (!masterDomain.ITM_QTY.Equals((e.Value == null ? "" : e.Value.ToString())))
                        {
                            //if (int.Parse(e.Value.ToString()) > masterDomain.PUR_QTY)
                            //{
                            //    e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                            //    e.ErrorContent = "[발주수량]" + masterDomain.PUR_QTY + " 보다 큰 값은 입력 하실수 없습니다";
                            //    e.SetError(e.ErrorContent, e.ErrorType);
                            //    return;
                            //}
                            //else if(e.Value.ToString().Equals("0"))
                            //{
                            //    e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                            //    e.ErrorContent = "0 보다 작은 값은 입력 하실수 없습니다";
                            //    e.SetError(e.ErrorContent, e.ErrorType);
                            //    return;
                            //}

                            masterDomain.ITM_QTY = e.Value.ToString();

                            masterDomain.RMN_QTY = Convert.ToDecimal(masterDomain.TMP_RMK_QTY) - Convert.ToDecimal(masterDomain.ITM_QTY);
                            masterDomain.PUR_AMT = Convert.ToDecimal(masterDomain.ITM_QTY) * Convert.ToDecimal(masterDomain.CO_UT_PRC);

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
                        if (string.IsNullOrEmpty(masterDomain.PUR_RMK))
                        {
                            masterDomain.PUR_RMK = "";
                        }
                        //
                        if (!masterDomain.PUR_RMK.Equals((e.Value == null ? "" : e.Value.ToString())))
                        {
                            masterDomain.PUR_RMK = e.Value.ToString();
                            masterDomain.isCheckd = true;
                            this.OKButton.IsEnabled = true;
                        }
                    }
                }
                else if (coUtPrc)
                {
                    if (e.IsValid)
                    {
                        if (string.IsNullOrEmpty(masterDomain.CO_UT_PRC + ""))
                        {
                            masterDomain.CO_UT_PRC = 0;
                        }

                        if (!masterDomain.CO_UT_PRC.Equals((e.Value == null ? "" : e.Value.ToString())))
                        {

                            masterDomain.CO_UT_PRC = e.Value.ToString();
                            masterDomain.PUR_AMT = Convert.ToDecimal(masterDomain.ITM_QTY) * Convert.ToDecimal(masterDomain.CO_UT_PRC);

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

        private void viewPage1EditView_HiddenEditor(object sender, DevExpress.Xpf.Grid.EditorEventArgs e)
        {
            this.viewJOB_ITEMView.CommitEditing();

            bool itmQty = (e.Column.FieldName.ToString().Equals("ITM_QTY") ? true : false);
            bool inaudOrgNm = (e.Column.FieldName.ToString().Equals("INAUD_ORG_NM") ? true : false);
            bool inaudRmk = (e.Column.FieldName.ToString().Equals("PUR_RMK") ? true : false);

            int rowHandle = this.viewJOB_ITEMView.FocusedRowHandle + 1;

            if (itmQty)
            {
                this.ViewJOB_ITEMEdit.CurrentColumn = this.ViewJOB_ITEMEdit.Columns["ITM_QTY"];
            }
            else if (inaudOrgNm)
            {
                this.ViewJOB_ITEMEdit.CurrentColumn = this.ViewJOB_ITEMEdit.Columns["INAUD_ORG_NM"];
            }
            else if (inaudRmk)
            {
                this.ViewJOB_ITEMEdit.CurrentColumn = this.ViewJOB_ITEMEdit.Columns["PUR_RMK"];
            }

            this.ViewJOB_ITEMEdit.RefreshRow(rowHandle - 1);
            this.viewJOB_ITEMView.FocusedRowHandle = rowHandle;
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

        private decimal plnAmtTotal = 0;
        private void grid_CustomSummary(object sender, CustomSummaryEventArgs e)
        {
              if (((GridSummaryItem)e.Item).FieldName.Equals("ITM_QTY"))
                {
                    if (e.IsTotalSummary)
                    {
                        if (e.SummaryProcess == CustomSummaryProcess.Start)
                        {
                            plnAmtTotal = 0;
                            InvVo tmpImsi;
                            for (int x = 0; x < this.ViewJOB_ITEMEdit.VisibleRowCount; x++)
                            {
                                int rowHandle = this.ViewJOB_ITEMEdit.GetRowHandleByVisibleIndex(x);
                                if (rowHandle > -1)
                                {
                                    tmpImsi = this.ViewJOB_ITEMEdit.GetRow(rowHandle) as InvVo;
                                    if (tmpImsi.isCheckd == true)
                                    {
                                        plnAmtTotal = plnAmtTotal + Convert.ToDecimal(tmpImsi.ITM_QTY);
                                    }
                                }
                            }
                            if (plnAmtTotal > 0)
                            {
                                e.TotalValue = plnAmtTotal;
                            }
                            else
                            {
                                e.TotalValue = 0;
                            }
                        }
                        if (e.SummaryProcess == CustomSummaryProcess.Calculate)
                        {                          
                        }
                    }                 
                }
              else if (((GridSummaryItem)e.Item).FieldName.Equals("PUR_AMT"))
              {
                  if (e.IsTotalSummary)
                  {
                      if (e.SummaryProcess == CustomSummaryProcess.Start)
                      {
                          plnAmtTotal = 0;
                          InvVo tmpImsi;
                          for (int x = 0; x < this.ViewJOB_ITEMEdit.VisibleRowCount; x++)
                          {
                              int rowHandle = this.ViewJOB_ITEMEdit.GetRowHandleByVisibleIndex(x);
                              if (rowHandle > -1)
                              {
                                  tmpImsi = this.ViewJOB_ITEMEdit.GetRow(rowHandle) as InvVo;
                                  if (tmpImsi.isCheckd == true)
                                  {
                                      plnAmtTotal = plnAmtTotal + Convert.ToDecimal(tmpImsi.PUR_AMT);
                                  }
                              }
                          }
                          if (plnAmtTotal > 0)
                          {
                              e.TotalValue = plnAmtTotal;
                          }
                          else
                          {
                              e.TotalValue = 0;
                          }
                      }
                      if (e.SummaryProcess == CustomSummaryProcess.Calculate)
                      {
                      }
                  }
              }
            }




        public async void SYSTEM_CODE_VO()
        {
            //this.lue_INAUD_ORG_NM.ItemsSource = SystemProperties.SYSTEM_CODE_VO("P-008");
            //this.combo_INAUD_ORG_NM.ItemsSource = SystemProperties.SYSTEM_CODE_VO("P-008");
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "P-008"))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.combo_INAUD_ORG_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                    this.lue_INAUD_ORG_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                }
            }

            //IList<CustomerCodeDao> tmpList = SystemProperties.CUSTOMER_CODE_VO("AP", null);
            //tmpList.Insert(0, new CustomerCodeDao() { CO_NO = "", CO_NM = "" });
            //this.combo_GRP_NM.ItemsSource = tmpList;
            //this.combo_GRP_NM.Text = "";
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s143", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { DELT_FLG = "N", CO_TP_CD = "AR", AREA_CD = SystemProperties.USER_VO.EMPE_PLC_NM, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    IList<SystemCodeVo> tmpList = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                    tmpList.Insert(0, new SystemCodeVo() { CO_NO = "", CO_NM = "" });
                    //
                    this.combo_GRP_NM.ItemsSource = tmpList;
                }
            }
        }


    }
}
