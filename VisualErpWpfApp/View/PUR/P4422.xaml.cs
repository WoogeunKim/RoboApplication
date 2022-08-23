using DevExpress.Xpf.Printing;
using DevExpress.XtraPrinting.Drawing;
using ModelsLibrary.Code;
using ModelsLibrary.Pur;
using Newtonsoft.Json;
using System;
using System.Drawing;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using AquilaErpWpfApp3.Util;
using AquilaErpWpfApp3.ViewModel;

namespace AquilaErpWpfApp3.View.PUR
{
    public partial class P4422 : UserControl
    {
        //private CustomerDialog customersDialog;

        //private static PurServiceClient purClient = SystemProperties.PurClient;
        private string _title = "매입정산마감";
        //private bool isCheckColumn = false;

        public P4422()
        {
            DataContext = new P4422ViewModel();
            //
            InitializeComponent();


           // this.ViewTableMst.MouseDown += ViewTableMst_MouseClick;
        }

        //void ViewTableMst_MouseClick(object sender, MouseButtonEventArgs e)
        //{
        //    PurVo masterDomain = (PurVo)ViewGridMst.GetFocusedRow();
        //    if (masterDomain.CLZ_FLG.Equals("Y"))
        //    {
        //        ViewTableMst.NavigationStyle = DevExpress.Xpf.Grid.GridViewNavigationStyle.Row;
        //    }
        //    else
        //    {
        //        ViewTableMst.NavigationStyle = DevExpress.Xpf.Grid.GridViewNavigationStyle.Cell;
        //    }
        //}

        //private void M_REFRESH_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        //{
        //    try
        //    {
        //        this.ViewGridMst.ShowLoadingPanel = true;
        //        this.ViewTableMst.SearchString = (this.M_SEARCH_TEXT.EditValue == null ? "" : this.M_SEARCH_TEXT.EditValue.ToString());
        //        //this.txt_Search.SelectAll();
        //        this.M_SEARCH_TEXT.Focus();
        //        this.ViewGridMst.ShowLoadingPanel = false;
        //    }
        //    catch (Exception eLog)
        //    {
        //        this.ViewGridMst.ShowLoadingPanel = false;
        //        WinUIMessageBox.Show(eLog.Message, "[에러]" + _title, MessageBoxButton.OK, MessageBoxImage.Error);
        //        //this.M_SEARCH_TEXT.SelectAll();
        //        this.M_SEARCH_TEXT.Focus();
        //        return;
        //    }
        //}

        //private void M_SAVE_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        //{
        //    this.ViewTableMst.Focus();
        //}

        private void M_PRINT_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            this.ViewTableMst.PrintAutoWidth = true;



            StringBuilder sb = new StringBuilder();
            sb.Append("<DataTemplate xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" xmlns:dxe=\"http://schemas.devexpress.com/winfx/2008/xaml/editors\">");
            sb.Append("<dxe:TextEdit Text=\"" + "[ 마감년월 : " + Convert.ToDateTime(M_FM_DT.EditValue).ToString("yyyy-MM") + "] " + "\" FontWeight=\"Bold\"  FontSize=\"11\"  />");
            sb.Append("</DataTemplate>");
            DataTemplate templateHeader = (DataTemplate)System.Windows.Markup.XamlReader.Parse(sb.ToString());

            sb = new StringBuilder();
            sb.Append("<DataTemplate xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" xmlns:dxe=\"http://schemas.devexpress.com/winfx/2008/xaml/editors\">");
            sb.Append("<dxe:TextEdit Text=\"" + System.DateTime.Now.ToString("yyyy-MM-dd hh:mm") + "\"/>");
            sb.Append("</DataTemplate>");
            DataTemplate templateFooter = (DataTemplate)System.Windows.Markup.XamlReader.Parse(sb.ToString());

            this.ViewTableMst.PrintAutoWidth = true;
            this.ViewTableMst.BestFitColumns();


            using (PrintableControlLink prtLink = new PrintableControlLink(this.ViewTableMst))
            {
                //prtLink.PageHeaderData = "인쇄 일자 : " + Convert.ToDateTime(this.txt_stDate.Text).ToString("yyyy-MM-dd");
                //prtLink.PageHeaderTemplate = Resources["PageHeader"] as DataTemplate;
                prtLink.PageHeaderTemplate = templateHeader;
                //prtLink.PageFooterData = "" + System.DateTime.Now.ToString("yyyy-MM-dd hh:mm");
                //prtLink.PageFooterTemplate = Resources["PageFooter"] as DataTemplate;
                prtLink.PageFooterTemplate = templateFooter;
                prtLink.PageHeaderData = null;
                prtLink.PageFooterData = null;

                prtLink.Margins.Top = 8;
                prtLink.Margins.Bottom = 8;
                prtLink.Margins.Left = 5;
                prtLink.Margins.Right = 5;
                prtLink.DocumentName =  _title + " Print";

                prtLink.PrintingSystem.Watermark.Text = "한양화스너공업(주)";
                prtLink.PrintingSystem.Watermark.TextDirection = DirectionMode.ForwardDiagonal;
                prtLink.PrintingSystem.Watermark.Font = new Font(prtLink.PrintingSystem.Watermark.Font.FontFamily, 40);
                prtLink.PrintingSystem.Watermark.ForeColor = System.Drawing.Color.PaleTurquoise;
                prtLink.PrintingSystem.Watermark.TextTransparency = 150;

                prtLink.Landscape = true;
                prtLink.PrintingSystem.ShowPrintStatusDialog = true;

                prtLink.PaperKind = System.Drawing.Printing.PaperKind.A4;
                prtLink.CreateDocument(true);
                prtLink.ShowPrintPreviewDialog(Application.Current.MainWindow, _title);
            }
        }

        private void TextEdit_KeyDown(object sender, KeyEventArgs e)
        {
            this.ViewTableMst.Focus();


        }

        //private void M_MST_COLUMN_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        //{
        //    //ViewGridMst.View.ClearValue(GridViewBase.ColumnChooserFactoryProperty);
        //    //this.ViewTableMst.ColumnChooserFactory = new CustomColumnChooser(columnChooser);


        //    if (isCheckColumn)
        //    {
        //        isCheckColumn = false;
        //        this.ViewTableMst.HideColumnChooser();
        //    }
        //    else
        //    {
        //        isCheckColumn = true;
        //        this.ViewTableMst.ShowColumnChooser();
        //    }

        //}

        //private void M_SEARCH_TEXT_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        //{
        //    if (e.Key == Key.Enter)
        //    {
        //        M_REFRESH_ItemClick(sender, null);
        //    }
        //}




        private async void GridColumn_Validate(object sender, DevExpress.Xpf.Grid.GridCellValidationEventArgs e)
        {
            try
            {
                int _Num = 0;
                PurVo resultVo;
                PurVo masterDomain = (PurVo)ViewGridDtl.GetFocusedRow();

                bool slAdjNm = (e.Column.FieldName.ToString().Equals("SL_ADJ_NM") ? true : false);
                //bool areaNm = (e.Column.FieldName.ToString().Equals("AREA_NM") ? true : false);
                //bool slAdjResonNm = (e.Column.FieldName.ToString().Equals("SL_ADJ_RESON_NM") ? true : false);
                //bool slItmPrc = (e.Column.FieldName.ToString().Equals("SL_ITM_PRC") ? true : false);
                //bool slItmQty = (e.Column.FieldName.ToString().Equals("SL_ITM_QTY") ? true : false);
                bool slItmAmt = (e.Column.FieldName.ToString().Equals("SL_ITM_AMT") ? true : false);
                bool inNo = (e.Column.FieldName.ToString().Equals("IN_NO") ? true : false);
                bool inaudRmk = (e.Column.FieldName.ToString().Equals("INAUD_RMK") ? true : false);
                bool slitmtax = (e.Column.FieldName.ToString().Equals("SL_ITM_TAX_AMT") ? true : false);



                if (slAdjNm)
                {
                    if (e.IsValid)
                    {
                        if (string.IsNullOrEmpty(masterDomain.SL_ADJ_CD))
                        {
                            masterDomain.SL_ADJ_CD = "";
                            masterDomain.SL_ADJ_NM = "";
                        }
                        //
                        if (!masterDomain.SL_ADJ_CD.Equals((e.Value == null ? "" : e.Value.ToString())))
                        {
                            SystemCodeVo bankIoDao = this.lue_AdjNm.GetItemFromValue(e.Value) as SystemCodeVo;
                            //
                            masterDomain.YRMON = bankIoDao.CLSS_CD;
                            masterDomain.SL_ADJ_CD = bankIoDao.CLSS_CD;
                            masterDomain.SL_ADJ_NM = bankIoDao.CLSS_DESC;
                            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("p4422/dtl/u", new StringContent(JsonConvert.SerializeObject(masterDomain), System.Text.Encoding.UTF8, "application/json")))
                            {
                                if (response.IsSuccessStatusCode)
                                {
                                    string resultMsg = await response.Content.ReadAsStringAsync();
                                    if (int.TryParse(resultMsg, out _Num) == false)
                                    {
                                        //실패
                                        e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                                        e.ErrorContent = resultMsg;
                                        e.SetError(e.ErrorContent, e.ErrorType);
                                        return;
                                    }
                                }
                            }

                            //masterDomain.SL_ADJ_NM = bankIoDao.CLSS_DESC;
                            //
                            //resultVo = purClient.P4422UpdateDtl(masterDomain);
                            //if (!resultVo.isSuccess)
                            //{
                            //    //실패
                            //    //WinUIMessageBox.Show(resultVo.Message, "[에러]" + _title, MessageBoxButton.OK, MessageBoxImage.Error);
                            //    e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                            //    e.ErrorContent = resultVo.Message;
                            //    e.SetError(e.ErrorContent, e.ErrorType);
                            //    return;
                            //}
                            //masterDomain.IS_CHECK = true;
                            //this.btn_save.IsEnabled = true;
                        }
                    }
                }
                //else if (areaNm)
                //{
                //    if (e.IsValid)
                //    {
                //        if (string.IsNullOrEmpty(masterDomain.AREA_CD))
                //        {
                //            masterDomain.AREA_CD = "";
                //            masterDomain.AREA_NM = "";
                //        }
                //        //
                //        if (!masterDomain.AREA_CD.Equals((e.Value == null ? "" : e.Value.ToString())))
                //        {
                //            CodeDao bankIoDao = this.lue_AreaNm.GetItemFromValue(e.Value) as CodeDao;
                //            //
                //            masterDomain.GBN = bankIoDao.CLSS_CD;
                //            //masterDomain.AREA_NM = bankIoDao.CLSS_DESC;
                //            //
                //            resultVo = purClient.P4422UpdateDtl(masterDomain);
                //            if (!resultVo.isSuccess)
                //            {
                //                //실패
                //                //WinUIMessageBox.Show(resultVo.Message, "[에러]" + _title, MessageBoxButton.OK, MessageBoxImage.Error);
                //                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                //                e.ErrorContent = resultVo.Message;
                //                e.SetError(e.ErrorContent, e.ErrorType);
                //                return;
                //            }
                //            masterDomain.AREA_CD = bankIoDao.CLSS_CD;
                //            masterDomain.AREA_NM = bankIoDao.CLSS_DESC;

                //            //masterDomain.IS_CHECK = true;
                //            //this.btn_save.IsEnabled = true;
                //        }
                //    }
                //}
                //else if (slAdjResonNm)
                //{
                //    if (e.IsValid)
                //    {
                //        if (string.IsNullOrEmpty(masterDomain.SL_ADJ_RESON_CD))
                //        {
                //            masterDomain.SL_ADJ_RESON_CD = "";
                //            masterDomain.SL_ADJ_RESON_NM = "";
                //        }
                //        //
                //        if (!masterDomain.SL_ADJ_RESON_CD.Equals((e.Value == null ? "" : e.Value.ToString())))
                //        {
                //            CodeDao bankIoDao = this.lue_AdjResonNm.GetItemFromValue(e.Value) as CodeDao;
                //            //
                //            masterDomain.SL_ADJ_RESON_CD = bankIoDao.CLSS_CD;
                //            masterDomain.SL_ADJ_RESON_NM = bankIoDao.CLSS_DESC;
                //            //
                //            resultVo = purClient.P4422UpdateDtl(masterDomain);
                //            if (!resultVo.isSuccess)
                //            {
                //                //실패
                //                //WinUIMessageBox.Show(resultVo.Message, "[에러]" + _title, MessageBoxButton.OK, MessageBoxImage.Error);
                //                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                //                e.ErrorContent = resultVo.Message;
                //                e.SetError(e.ErrorContent, e.ErrorType);
                //                return;
                //            }
                //            //masterDomain.IS_CHECK = true;
                //            //this.btn_save.IsEnabled = true;
                //        }
                //    }
                //}
                //else if (slItmPrc)
                //{
                //    if (string.IsNullOrEmpty(masterDomain.SL_ITM_PRC + ""))
                //    {
                //        masterDomain.SL_ITM_PRC = 0;
                //    }
                //    //
                //    if (!masterDomain.SL_ITM_PRC.Equals((e.Value == null ? "" : e.Value.ToString())))
                //    {
                //        //masterDomain.IS_CHECK = true;
                //        //this.btn_save.IsEnabled = true;
                //        masterDomain.SL_ITM_PRC = e.Value.ToString();
                //        resultVo = purClient.P4422UpdateDtl(masterDomain);
                //        if (!resultVo.isSuccess)
                //        {
                //            //실패
                //            //WinUIMessageBox.Show(resultVo.Message, "[에러]" + _title, MessageBoxButton.OK, MessageBoxImage.Error);
                //            e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                //            e.ErrorContent = resultVo.Message;
                //            e.SetError(e.ErrorContent, e.ErrorType);
                //            return;
                //        }
                //    }
                //}
                //else if (slItmQty)
                //{
                //    if (string.IsNullOrEmpty(masterDomain.SL_ITM_QTY + ""))
                //    {
                //        masterDomain.SL_ITM_QTY = 0;
                //    }
                //    //
                //    if (!masterDomain.SL_ITM_QTY.Equals((e.Value == null ? "" : e.Value.ToString())))
                //    {
                //        //masterDomain.IS_CHECK = true;
                //        //this.btn_save.IsEnabled = true;
                //        masterDomain.SL_ITM_QTY = e.Value.ToString();
                //        resultVo = purClient.P4422UpdateDtl(masterDomain);
                //        if (!resultVo.isSuccess)
                //        {
                //            //실패
                //            //WinUIMessageBox.Show(resultVo.Message, "[에러]" + _title, MessageBoxButton.OK, MessageBoxImage.Error);
                //            e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                //            e.ErrorContent = resultVo.Message;
                //            e.SetError(e.ErrorContent, e.ErrorType);
                //            return;
                //        }
                //    }
                //}
                else if (slItmAmt)
                {
                    if (string.IsNullOrEmpty(masterDomain.SL_ITM_AMT + ""))
                    {
                        masterDomain.SL_ITM_AMT = 0;
                    }
                    //
                    if (!masterDomain.SL_ITM_AMT.Equals((e.Value == null ? "" : e.Value.ToString())))
                    {
                        //masterDomain.IS_CHECK = true;
                        //this.btn_save.IsEnabled = true;
                        masterDomain.SL_ITM_AMT = e.Value.ToString();
                        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("p4422/dtl/u", new StringContent(JsonConvert.SerializeObject(masterDomain), System.Text.Encoding.UTF8, "application/json")))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                string resultMsg = await response.Content.ReadAsStringAsync();
                                if (int.TryParse(resultMsg, out _Num) == false)
                                {
                                    //실패
                                    e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                                    e.ErrorContent = resultMsg;
                                    e.SetError(e.ErrorContent, e.ErrorType);
                                    return;
                                }
                            }
                        }
                        //resultVo = purClient.P4422UpdateDtl(masterDomain);
                        //if (!resultVo.isSuccess)
                        //{
                        //    //실패
                        //    //WinUIMessageBox.Show(resultVo.Message, "[에러]" + _title, MessageBoxButton.OK, MessageBoxImage.Error);
                        //    e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                        //    e.ErrorContent = resultVo.Message;
                        //    e.SetError(e.ErrorContent, e.ErrorType);
                        //    return;
                        //}
                    }
                }
                else if (inNo)
                {
                    if (string.IsNullOrEmpty(masterDomain.IN_NO))
                    {
                        masterDomain.IN_NO = "";
                    }
                    //
                    if (!masterDomain.IN_NO.Equals((e.Value == null ? "" : e.Value.ToString())))
                    {
                        //masterDomain.IS_CHECK = true;
                        //this.btn_save.IsEnabled = true;
                        masterDomain.IN_NO = e.Value.ToString();
                        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("p4422/dtl/u", new StringContent(JsonConvert.SerializeObject(masterDomain), System.Text.Encoding.UTF8, "application/json")))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                string resultMsg = await response.Content.ReadAsStringAsync();
                                if (int.TryParse(resultMsg, out _Num) == false)
                                {
                                    //실패
                                    e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                                    e.ErrorContent = resultMsg;
                                    e.SetError(e.ErrorContent, e.ErrorType);
                                    return;
                                }
                            }
                        }
                        //resultVo = purClient.P4422UpdateDtl(masterDomain);
                        //if (!resultVo.isSuccess)
                        //{
                        //    //실패
                        //    //WinUIMessageBox.Show(resultVo.Message, "[에러]" + _title, MessageBoxButton.OK, MessageBoxImage.Error);
                        //    e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                        //    e.ErrorContent = resultVo.Message;
                        //    e.SetError(e.ErrorContent, e.ErrorType);
                        //    return;
                        //}
                    }
                }
                else if (inaudRmk)
                {
                    if (string.IsNullOrEmpty(masterDomain.INAUD_RMK))
                    {
                        masterDomain.INAUD_RMK = "";
                    }
                    //
                    if (!masterDomain.INAUD_RMK.Equals((e.Value == null ? "" : e.Value.ToString())))
                    {
                        //masterDomain.IS_CHECK = true;
                        //this.btn_save.IsEnabled = true;
                        masterDomain.INAUD_RMK = e.Value.ToString();
                        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("p4422/dtl/u", new StringContent(JsonConvert.SerializeObject(masterDomain), System.Text.Encoding.UTF8, "application/json")))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                string resultMsg = await response.Content.ReadAsStringAsync();
                                if (int.TryParse(resultMsg, out _Num) == false)
                                {
                                    //실패
                                    e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                                    e.ErrorContent = resultMsg;
                                    e.SetError(e.ErrorContent, e.ErrorType);
                                    return;
                                }
                            }
                        }
                        //resultVo = purClient.P4422UpdateDtl(masterDomain);
                        //if (!resultVo.isSuccess)
                        //{
                        //    //실패
                        //    //WinUIMessageBox.Show(resultVo.Message, "[에러]" + _title, MessageBoxButton.OK, MessageBoxImage.Error);
                        //    e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                        //    e.ErrorContent = resultVo.Message;
                        //    e.SetError(e.ErrorContent, e.ErrorType);
                        //    return;
                        //}
                    }
                }
                else if (slitmtax)
                {
                    if (string.IsNullOrEmpty(masterDomain.SL_ITM_TAX_AMT+""))
                    {
                        masterDomain.SL_ITM_TAX_AMT = 0;
                    }
                    //
                    if (!masterDomain.SL_ITM_TAX_AMT.Equals((e.Value == null ? "" : e.Value.ToString())))
                    {
                        //masterDomain.IS_CHECK = true;
                        //this.btn_save.IsEnabled = true;
                        masterDomain.SL_ITM_TAX_AMT = e.Value.ToString();
                        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("p4422/dtl/u", new StringContent(JsonConvert.SerializeObject(masterDomain), System.Text.Encoding.UTF8, "application/json")))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                string resultMsg = await response.Content.ReadAsStringAsync();
                                if (int.TryParse(resultMsg, out _Num) == false)
                                {
                                    //실패
                                    e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                                    e.ErrorContent = resultMsg;
                                    e.SetError(e.ErrorContent, e.ErrorType);
                                    return;
                                }
                            }
                        }
                        //resultVo = purClient.P4422UpdateDtl(masterDomain);
                        //if (!resultVo.isSuccess)
                        //{
                        //    //실패
                        //    //WinUIMessageBox.Show(resultVo.Message, "[에러]" + _title, MessageBoxButton.OK, MessageBoxImage.Error);
                        //    e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                        //    e.ErrorContent = resultVo.Message;
                        //    e.SetError(e.ErrorContent, e.ErrorType);
                        //    return;
                        //}
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
            this.ViewTableDtl.CommitEditing();

            //bool slAdjNm = (e.Column.FieldName.ToString().Equals("SL_ADJ_NM") ? true : false);
            //bool areaNm = (e.Column.FieldName.ToString().Equals("AREA_NM") ? true : false);
            //bool slAdjResonNm = (e.Column.FieldName.ToString().Equals("SL_ADJ_RESON_NM") ? true : false);
            //bool slItmPrc = (e.Column.FieldName.ToString().Equals("SL_ITM_PRC") ? true : false);
            //bool slItmQty = (e.Column.FieldName.ToString().Equals("SL_ITM_QTY") ? true : false);
            //bool slItmAmt = (e.Column.FieldName.ToString().Equals("SL_ITM_AMT") ? true : false);

            //int rowHandle = this.ViewTableDtl.FocusedRowHandle;


            //if (slAdjNm)
            //{
            //    this.ViewGridDtl.CurrentColumn = this.ViewGridDtl.Columns["AREA_NM"];
            //}
            //else if (areaNm)
            //{
            //    this.ViewGridDtl.CurrentColumn = this.ViewGridDtl.Columns["SL_ADJ_RESON_NM"];
            //}
            //else if (slAdjResonNm)
            //{
            //    this.ViewGridDtl.CurrentColumn = this.ViewGridDtl.Columns["SL_ITM_PRC"];
            //}
            //else if (slItmPrc)
            //{
            //    this.ViewGridDtl.CurrentColumn = this.ViewGridDtl.Columns["SL_ITM_QTY"];
            //}
            //else if (slItmQty)
            //{
            //    this.ViewGridDtl.CurrentColumn = this.ViewGridDtl.Columns["SL_ITM_AMT"];
            //}
            //else if (slItmAmt)
            //{
            //    rowHandle = this.ViewTableDtl.FocusedRowHandle + 1;
            //    this.ViewGridDtl.CurrentColumn = this.ViewGridDtl.Columns["SL_ADJ_NM"];
            //    this.ViewGridDtl.RefreshRow(rowHandle - 1);
            //}

            //this.ViewGridDtl.RefreshRow(rowHandle);
            //this.ViewTableDtl.FocusedRowHandle = rowHandle;
        }

    }
}