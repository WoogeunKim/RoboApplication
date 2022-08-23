using AquilaErpWpfApp3.Util;
using AquilaErpWpfApp3.ViewModel;
using ModelsLibrary.Sale;
using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Net.Http;
using System.Windows.Controls;

namespace AquilaErpWpfApp3.View.SAL
{
    public partial class S3311 : UserControl
    {
        //private CustomerDialog customersDialog;
        private string _title = "거래처별 판가기준표";

        public S3311()
        {
            DataContext = new S3311ViewModel();
            //
            InitializeComponent();

        }

        //private void CheckEdit_Checked(object sender, RoutedEventArgs e)
        //{
        //    CheckEdit checkEdit = sender as CheckEdit;
        //    PurVo tmpImsi;
        //    for (int x = 0; x < this.ViewGridDtl.VisibleRowCount; x++)
        //    {
        //        int rowHandle = this.ViewGridDtl.GetRowHandleByVisibleIndex(x);
        //        if (rowHandle > -1)
        //        {
        //            tmpImsi = this.ViewGridDtl.GetRow(rowHandle) as PurVo;
        //            if (checkEdit.IsChecked == true)
        //            {
        //                tmpImsi.isCheckd = true;
        //            }
        //            else
        //            {
        //                tmpImsi.isCheckd = false;
        //            }
        //        }
        //    }
        //}

        private async void GridColumn_Validate(object sender, DevExpress.Xpf.Grid.GridCellValidationEventArgs e)
        {
            try
            {
                SaleVo mstDomain = (SaleVo)ViewGridMst.GetFocusedRow();
                SaleVo dtlDomain = (SaleVo)ViewGridDtl.GetFocusedRow();

                bool d3Qty = (e.Column.FieldName.ToString().Equals("A1") ? true : false);
                bool d4Qty = (e.Column.FieldName.ToString().Equals("A2") ? true : false);
                bool d5Qty = (e.Column.FieldName.ToString().Equals("A3") ? true : false);
                bool d6Qty = (e.Column.FieldName.ToString().Equals("A4") ? true : false);

                if (d3Qty)
                {
                    if (e.IsValid)
                    {
                        if (dtlDomain.A1 == null)
                        {
                            dtlDomain.A1 = 0;
                        }
                        //
                        if (!dtlDomain.A1.Equals((e.Value == null ? "" : e.Value.ToString())))
                        {
                            try
                            {
                                //
                                dtlDomain.CO_NO = mstDomain.CO_NO;
                                dtlDomain.FM_DT = mstDomain.FM_DT;


                                dtlDomain.A1 = e.Value.ToString();
                                dtlDomain.CO_UT_PRC = e.Value.ToString();

                                dtlDomain.XLS_A = 1;
                                dtlDomain.XLS_B = getFindN2ndIndex(dtlDomain.GBN);

                                dtlDomain.UPD_USR_ID = SystemProperties.USER;
                                dtlDomain.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
                                //
                                int _Num = 0;
                                string resultMsg = "";
                                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s3311/mst/u", new StringContent(JsonConvert.SerializeObject(dtlDomain), System.Text.Encoding.UTF8, "application/json")))
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
                else if (d4Qty)
                {
                    if (e.IsValid)
                    {
                        if (dtlDomain.A2 == null)
                        {
                            dtlDomain.A2 = 0;
                        }
                        //
                        if (!dtlDomain.A2.Equals((e.Value == null ? "" : e.Value.ToString())))
                        {
                            try
                            {
                                //
                                dtlDomain.CO_NO = mstDomain.CO_NO;
                                dtlDomain.FM_DT = mstDomain.FM_DT;


                                dtlDomain.A2 = e.Value.ToString();
                                dtlDomain.CO_UT_PRC = e.Value.ToString();

                                dtlDomain.XLS_A = 2;
                                dtlDomain.XLS_B = getFindN2ndIndex(dtlDomain.GBN);

                                dtlDomain.UPD_USR_ID = SystemProperties.USER;
                                dtlDomain.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
                                ////
                                int _Num = 0;
                                string resultMsg = "";
                                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s3311/mst/u", new StringContent(JsonConvert.SerializeObject(dtlDomain), System.Text.Encoding.UTF8, "application/json")))
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
                else if (d5Qty)
                {
                    if (e.IsValid)
                    {
                        if (dtlDomain.A3 == null)
                        {
                            dtlDomain.A3 = 0;
                        }
                        //
                        if (!dtlDomain.A3.Equals((e.Value == null ? "" : e.Value.ToString())))
                        {
                            try
                            {
                                //
                                dtlDomain.CO_NO = mstDomain.CO_NO;
                                dtlDomain.FM_DT = mstDomain.FM_DT;


                                dtlDomain.A3 = e.Value.ToString();
                                dtlDomain.CO_UT_PRC = e.Value.ToString();

                                dtlDomain.XLS_A = 3;
                                dtlDomain.XLS_B = getFindN2ndIndex(dtlDomain.GBN);

                                dtlDomain.UPD_USR_ID = SystemProperties.USER;
                                dtlDomain.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
                                ////
                                int _Num = 0;
                                string resultMsg = "";
                                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s3311/mst/u", new StringContent(JsonConvert.SerializeObject(dtlDomain), System.Text.Encoding.UTF8, "application/json")))
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
                else if (d6Qty)
                {
                    if (e.IsValid)
                    {
                        if (dtlDomain.A4 == null)
                        {
                            dtlDomain.A4 = 0;
                        }
                        //
                        if (!dtlDomain.A4.Equals((e.Value == null ? "" : e.Value.ToString())))
                        {
                            try
                            {
                                //
                                dtlDomain.CO_NO = mstDomain.CO_NO;
                                dtlDomain.FM_DT = mstDomain.FM_DT;


                                dtlDomain.A4 = e.Value.ToString();
                                dtlDomain.CO_UT_PRC = e.Value.ToString();

                                dtlDomain.XLS_A = 4;
                                dtlDomain.XLS_B = getFindN2ndIndex(dtlDomain.GBN);

                                dtlDomain.UPD_USR_ID = SystemProperties.USER;
                                dtlDomain.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
                                ////
                                int _Num = 0;
                                string resultMsg = "";
                                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s3311/mst/u", new StringContent(JsonConvert.SerializeObject(dtlDomain), System.Text.Encoding.UTF8, "application/json")))
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
           // int rowHandle = this.ViewTableDtl.FocusedRowHandle + 1;
            //this.ViewGridDtl.RefreshRow(rowHandle - 1);
            //this.ViewTableDtl.FocusedRowHandle = rowHandle;
        }

        ////N1ST_SRT_SEQ
        //private string getFindN1stIndex()
        //{
        //    return "";
        //}

        //N2ND_SRT_SEQ
        private string getFindN2ndIndex(string _gbn)
        {
            if (_gbn.Equals("D10"))
            {
                return "1";
            }
            else if (_gbn.Equals("D13"))
            {
                return "2";
            }
            else if (_gbn.Equals("D16"))
            {
                return "3";
            }
            else if (_gbn.Equals("D19"))
            {
                return "4";
            }
            else if (_gbn.Equals("D22"))
            {
                return "5";
            }
            else if (_gbn.Equals("D25"))
            {
                return "6";
            }
            else if (_gbn.Equals("D29"))
            {
                return "7";
            }
            else if (_gbn.Equals("D32"))
            {
                return "8";
            }
            return "";
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