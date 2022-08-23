using AquilaErpWpfApp3.Util;
using AquilaErpWpfApp3.ViewModel;
using DevExpress.Xpf.Editors;
using ModelsLibrary.Pur;
using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;

namespace AquilaErpWpfApp3.View.PUR
{
    public partial class P441102 : UserControl
    {
        //private CustomerDialog customersDialog;
        private string _title = "원자재 발주서 등록";

        public P441102()
        {
            DataContext = new P441102ViewModel();
            //
            InitializeComponent();

        }

        private void CheckEdit_Checked(object sender, RoutedEventArgs e)
        {
            CheckEdit checkEdit = sender as CheckEdit;
            PurVo tmpImsi;
            for (int x = 0; x < this.ViewGridDtl.VisibleRowCount; x++)
            {
                int rowHandle = this.ViewGridDtl.GetRowHandleByVisibleIndex(x);
                if (rowHandle > -1)
                {
                    tmpImsi = this.ViewGridDtl.GetRow(rowHandle) as PurVo;
                    if (checkEdit.IsChecked == true)
                    {
                        tmpImsi.isCheckd = true;
                    }
                    else
                    {
                        tmpImsi.isCheckd = false;
                    }
                }
            }
        }

        private async void GridColumn_Validate(object sender, DevExpress.Xpf.Grid.GridCellValidationEventArgs e)
        {
            try
            {
                PurVo masterDomain = (PurVo)ViewGridDtl.GetFocusedRow();
                bool purItmRmk = (e.Column.FieldName.ToString().Equals("PUR_ITM_RMK") ? true : false);
                bool inReqDt = (e.Column.FieldName.ToString().Equals("IN_REQ_DT") ? true : false);
                bool purQty = (e.Column.FieldName.ToString().Equals("PUR_QTY") ? true : false);
       
                if (purItmRmk)
                {
                    if (e.IsValid)
                    {
                        if (string.IsNullOrEmpty(masterDomain.PUR_ITM_RMK))
                        {
                            masterDomain.PUR_ITM_RMK = string.Empty;
                        }
                        //
                        if (!masterDomain.PUR_ITM_RMK.Equals((e.Value == null ? "" : e.Value.ToString())))
                        {
                            try
                            {
                                masterDomain.PUR_ITM_RMK = e.Value.ToString();
                                masterDomain.UPD_USR_ID = SystemProperties.USER;
                                masterDomain.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
                                //
                                int _Num = 0;
                                string resultMsg = "";
                                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("p441102/dtl/u", new StringContent(JsonConvert.SerializeObject(masterDomain), System.Text.Encoding.UTF8, "application/json")))
                                {
                                    if (response.IsSuccessStatusCode)
                                    {
                                        resultMsg = await response.Content.ReadAsStringAsync();
                                        if (int.TryParse(resultMsg, out _Num) == false)
                                        {
                                            //실패
                                            e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                                            e.ErrorContent = resultMsg;
                                            e.SetError(e.ErrorContent, e.ErrorType);
                                            return;
                                        }

                                        this.ViewGridDtl.RefreshData();
                                    }
                                }
                            }
                            catch (System.Exception eLog)
                            {
                                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                                e.ErrorContent = eLog.Message;
                                e.SetError(e.ErrorContent, e.ErrorType);
                                return;
                            }
                        }
                    }
                }
                else if (purQty)
                {
                    if (e.IsValid)
                    {
                        if (masterDomain.PUR_QTY == null)
                        {
                            masterDomain.PUR_QTY = 0;
                        }
                        //
                        if (!masterDomain.PUR_QTY.Equals((e.Value == null ? "" : e.Value.ToString())))
                        {
                            try
                            {
                                masterDomain.PUR_QTY = e.Value.ToString();
                                masterDomain.PUR_AMT = Convert.ToInt32(masterDomain.PUR_QTY) * Convert.ToDecimal(masterDomain.CO_UT_PRC);
                                masterDomain.VAT_AMT = Convert.ToDouble(masterDomain.PUR_AMT) * 0.1;
                                masterDomain.TOT_AMT = (Convert.ToDouble(masterDomain.PUR_AMT) * 0.1) + Convert.ToDouble(masterDomain.PUR_AMT);


                                masterDomain.UPD_USR_ID = SystemProperties.USER;
                                masterDomain.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
                                //
                                int _Num = 0;
                                string resultMsg = "";
                                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("p441102/dtl/u", new StringContent(JsonConvert.SerializeObject(masterDomain), System.Text.Encoding.UTF8, "application/json")))
                                {
                                    if (response.IsSuccessStatusCode)
                                    {
                                        resultMsg = await response.Content.ReadAsStringAsync();
                                        if (int.TryParse(resultMsg, out _Num) == false)
                                        {
                                            //실패
                                            e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                                            e.ErrorContent = resultMsg;
                                            e.SetError(e.ErrorContent, e.ErrorType);
                                            return;
                                        }

                                        this.ViewGridDtl.RefreshData();
                                    }
                                }
                            }
                            catch (System.Exception eLog)
                            {
                                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                                e.ErrorContent = eLog.Message;
                                e.SetError(e.ErrorContent, e.ErrorType);
                                return;
                            }
                        }
                    }
                }
                else if (inReqDt)
                {
                    if (e.IsValid)
                    {
                        if (string.IsNullOrEmpty(masterDomain.IN_REQ_DT))
                        {
                            masterDomain.IN_REQ_DT = string.Empty;
                        }
                        //
                        if (!masterDomain.IN_REQ_DT.Equals((e.Value == null ? "" : e.Value.ToString())))
                        {
                            try
                            {
                                DateTime dtRtn;
                                if (!DateTime.TryParseExact(e.Value.ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dtRtn))
                                {
                                    e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                                    e.ErrorContent = "[입고요청일] " + e.Value.ToString() + "입력 값이 맞지 않습니다.";
                                    e.SetError(e.ErrorContent, e.ErrorType);
                                    return;
                                }

                                masterDomain.IN_REQ_DT = e.Value.ToString();
                                masterDomain.UPD_USR_ID = SystemProperties.USER;
                                masterDomain.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
                                //
                                int _Num = 0;
                                string resultMsg = "";
                                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("p441102/dtl/u", new StringContent(JsonConvert.SerializeObject(masterDomain), System.Text.Encoding.UTF8, "application/json")))
                                {
                                    if (response.IsSuccessStatusCode)
                                    {
                                        resultMsg = await response.Content.ReadAsStringAsync();
                                        if (int.TryParse(resultMsg, out _Num) == false)
                                        {
                                            //실패
                                            e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                                            e.ErrorContent = resultMsg;
                                            e.SetError(e.ErrorContent, e.ErrorType);
                                            return;
                                        }

                                        this.ViewGridDtl.RefreshData();
                                    }
                                }
                            }
                            catch (System.Exception eLog)
                            {
                                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                                e.ErrorContent = eLog.Message;
                                e.SetError(e.ErrorContent, e.ErrorType);
                                return;
                            }
                        }
                    }
                }


            }
            catch (System.Exception eLog)
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

            //bool lssVal = (e.Column.FieldName.ToString().Equals("LSS_VAL") ? true : false);
            int rowHandle = this.ViewTableDtl.FocusedRowHandle + 1;
            this.ViewGridDtl.RefreshRow(rowHandle - 1);
            //this.ViewTableDtl.FocusedRowHandle = rowHandle;
        }

        //private void M_SEARCH_TEXT_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        //{
        //    if (e.Key == Key.Enter)
        //    {
        //        M_REFRESH_ItemClick(sender, null);
        //    }
        //}

        //private void M_SAVE_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        //{
        //    this.ViewTableDtl.Focus();
        //}

    }
}