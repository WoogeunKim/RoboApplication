﻿using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Printing;
using ModelsLibrary.Inv;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using AquilaErpWpfApp3.ViewModel;

namespace AquilaErpWpfApp3.View.INV
{
    public partial class I5512 : UserControl
    {
        //private CustomerDialog customersDialog;
        //private I5512DetailReportDialog detailReportDialog;
        public I5512()
        {
            DataContext = new I5512ViewModel();
            //
            InitializeComponent();

        }

        private void CheckEdit_Checked(object sender, RoutedEventArgs e)
        {
            CheckEdit checkEdit = sender as CheckEdit;
            InvVo tmpImsi;
            for (int x = 0; x < this.ViewGridDtl.VisibleRowCount; x++)
            {
                int rowHandle = this.ViewGridDtl.GetRowHandleByVisibleIndex(x);
                if (rowHandle > -1)
                {
                    tmpImsi = this.ViewGridDtl.GetRow(rowHandle) as InvVo;
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

        //private void M_REFRESH_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        //{
        //      try
        //      {
        //          this.ViewGridMst.ShowLoadingPanel = true;
        //          this.ViewTableMst.SearchString = (this.M_SEARCH_TEXT.EditValue == null ? "" : this.M_SEARCH_TEXT.EditValue.ToString());
        //          //this.txt_Search.SelectAll();
        //          this.M_SEARCH_TEXT.Focus();
        //          this.ViewGridMst.ShowLoadingPanel = false;
        //      }
        //      catch (Exception eLog)
        //      {
        //          this.ViewGridMst.ShowLoadingPanel = false;
        //          WinUIMessageBox.Show(eLog.Message, "[에러]품목 출고 관리", MessageBoxButton.OK, MessageBoxImage.Error);
        //          //this.M_SEARCH_TEXT.SelectAll();
        //          this.M_SEARCH_TEXT.Focus();
        //          return;
        //      }
        //}

        private void M_MST_REPORT_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            //InvVo masterDomain = (InvVo)ViewGridMst.GetFocusedRow(); 
            //InvVo detailDomain = (InvVo)ViewGridDtl.GetFocusedRow();
            //detailReportDialog = new I5512DetailReportDialog(masterDomain);

            //if (detailDomain != null)
            //{
            //    StringBuilder sb = new StringBuilder();
            //    sb.Append("<DataTemplate xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" xmlns:dxe=\"http://schemas.devexpress.com/winfx/2008/xaml/editors\">");
            //    sb.Append("<dxe:TextEdit Text=\"" + "거래처 : " + masterDomain.CO_NM + ", 출력 일시 :" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "\" FontWeight=\"Bold\"  FontSize=\"8\"  />");
            //    sb.Append("</DataTemplate>");
            //    DataTemplate templateHeader = (DataTemplate)System.Windows.Markup.XamlReader.Parse(sb.ToString());

            //    sb = new StringBuilder();
            //    sb.Append("<DataTemplate xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" xmlns:dxe=\"http://schemas.devexpress.com/winfx/2008/xaml/editors\">");
            //    sb.Append("<dxe:TextEdit Text=\"" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "\"/>");
            //    sb.Append("</DataTemplate>");
            //    DataTemplate templateFooter = (DataTemplate)System.Windows.Markup.XamlReader.Parse(sb.ToString());
            //    this.ViewTableDtl.PrintAutoWidth = true;
            //    using (PrintableControlLink prtLink = new PrintableControlLink(detailReportDialog.ViewTableDtl))
            //    {
            //        //prtLink.PageHeaderData = masterDomain.YRMON + "/" + masterDomain.INSRL_NM + "/" + masterDomain.CO_NM;
            //        prtLink.PageHeaderTemplate = templateHeader;
            //        prtLink.PageFooterData = "" + System.DateTime.Now.ToString("yyyy-MM-dd hh:mm");
            //        prtLink.PageFooterTemplate = templateFooter;

            //        prtLink.Margins.Top = 8;
            //        prtLink.Margins.Bottom = 8;
            //        prtLink.Margins.Left = 5;
            //        prtLink.Margins.Right = 5;
            //        prtLink.DocumentName = "품목 출고 관리 Print";

            //        prtLink.PrintingSystem.Watermark.Text = "한양화스너공업(주)";
            //        prtLink.PrintingSystem.Watermark.TextDirection = DirectionMode.ForwardDiagonal;
            //        prtLink.PrintingSystem.Watermark.Font = new Font(prtLink.PrintingSystem.Watermark.Font.FontFamily, 40);
            //        prtLink.PrintingSystem.Watermark.ForeColor = System.Drawing.Color.PaleTurquoise;
            //        prtLink.PrintingSystem.Watermark.TextTransparency = 150;

            //        prtLink.Landscape = true;
            //        prtLink.PrintingSystem.ShowPrintStatusDialog = true;

            //        prtLink.PaperKind = System.Drawing.Printing.PaperKind.A4;
            //        prtLink.CreateDocument(true);
            //        prtLink.ShowPrintPreviewDialog(Application.Current.MainWindow, "품목 출고 관리");
            //    }
            //}


            InvVo mstDomain = (InvVo)ViewGridMst.GetFocusedRow();
            InvVo masterDomain = (InvVo)ViewGridDtl.GetFocusedRow();
            if (masterDomain != null)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<DataTemplate xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" xmlns:dxe=\"http://schemas.devexpress.com/winfx/2008/xaml/editors\">");
                sb.Append("<dxe:TextEdit Text=\"" + "거래처 : " + mstDomain.CO_NM + ", 출력 일시 :" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "\" FontWeight=\"Bold\"  FontSize=\"8\"  />");
                sb.Append("</DataTemplate>");
                DataTemplate templateHeader = (DataTemplate)System.Windows.Markup.XamlReader.Parse(sb.ToString());

                sb = new StringBuilder();
                sb.Append("<DataTemplate xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" xmlns:dxe=\"http://schemas.devexpress.com/winfx/2008/xaml/editors\">");
                sb.Append("<dxe:TextEdit Text=\"" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "\"/>");
                sb.Append("</DataTemplate>");
                DataTemplate templateFooter = (DataTemplate)System.Windows.Markup.XamlReader.Parse(sb.ToString());
                this.ViewTableDtl.PrintAutoWidth = true;

                using (PrintableControlLink prtLink = new PrintableControlLink(this.ViewTableDtl))
                {
                    //prtLink.PageHeaderData = "인쇄 일자 : " + Convert.ToDateTime(this.txt_stDate.Text).ToString("yyyy-MM-dd");
                    //prtLink.PageHeaderTemplate = Resources["PageHeader"] as DataTemplate;
                    //prtLink.PageHeaderData = masterDomain.YRMON + "/" + masterDomain.INSRL_NM + "/" + masterDomain.CO_NM;
                    prtLink.PageHeaderTemplate = templateHeader;
                    prtLink.PageFooterData = "" + System.DateTime.Now.ToString("yyyy-MM-dd hh:mm");
                    prtLink.PageFooterTemplate = templateFooter;

                    prtLink.Margins.Top = 8;
                    prtLink.Margins.Bottom = 8;
                    prtLink.Margins.Left = 5;
                    prtLink.Margins.Right = 5;
                    prtLink.DocumentName = "품목 출고 관리 Print";

                    //prtLink.PrintingSystem.Watermark.Text = "한양화스너공업(주)";
                    //prtLink.PrintingSystem.Watermark.TextDirection = DirectionMode.ForwardDiagonal;
                    //prtLink.PrintingSystem.Watermark.Font = new Font(prtLink.PrintingSystem.Watermark.Font.FontFamily, 40);
                    //prtLink.PrintingSystem.Watermark.ForeColor = System.Drawing.Color.PaleTurquoise;
                    //prtLink.PrintingSystem.Watermark.TextTransparency = 150;

                    prtLink.Landscape = true;
                    prtLink.PrintingSystem.ShowPrintStatusDialog = true;

                    prtLink.PaperKind = System.Drawing.Printing.PaperKind.A4;
                    prtLink.CreateDocument(true);
                    prtLink.ShowPrintPreviewDialog(Application.Current.MainWindow, "품목 출고 관리");
                }
            }

        }
        //private void CoNm_Click(object sender, RoutedEventArgs e)
        //{
        //    PurVo masterDomain = (PurVo)ViewGridDtl.GetFocusedRow();
        //    if (masterDomain == null) { return; }
        //    int rowHandle = this.ViewTableDtl.FocusedRowHandle;
        //    //
        //    customersDialog = new CustomerDialog();
        //    customersDialog.Title = "거래처 코드";
        //    customersDialog.Owner = Application.Current.MainWindow;
        //    customersDialog.BorderEffect = BorderEffect.Default;
        //    customersDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
        //    customersDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
        //    bool isDialog = (bool)customersDialog.ShowDialog();
        //    if (isDialog)
        //    {
        //        CustomerCodeVo coNmDao = customersDialog.ResultDao;
        //        if (coNmDao != null)
        //        {
        //            masterDomain.CO_NO = coNmDao.CO_NO;
        //            masterDomain.CO_NM = coNmDao.CO_NM;
        //            //masterDomain.PRSD_NM = coNmDao.PRSD_NM;
        //            //
        //            this.ViewTableDtl.CommitEditing();
        //            this.ViewGridDtl.RefreshRow(rowHandle);
        //            this.M_SAVE.Focus();
        //        }
        //        //
        //        //
        //        masterDomain.isCheckd = true;
        //        this.M_SAVE.IsEnabled = true;
        //    }
        //}

        //private void GridColumn_Validate(object sender, DevExpress.Xpf.Grid.GridCellValidationEventArgs e)
        //{
        //    try
        //    {
        //        PurVo masterDomain = (PurVo)ViewGridDtl.GetFocusedRow();

        //        //거래처 명
        //        bool coNm = (e.Column.FieldName.ToString().Equals("CO_NM") ? true : false);
        //        //통화
        //        bool currCd = (e.Column.FieldName.ToString().Equals("CURR_CD") ? true : false);
        //        //적용 일자
        //        bool cngAplyDt = (e.Column.FieldName.ToString().Equals("CNG_APLY_DT") ? true : false);
        //        //단가
        //        bool coUtPrc = (e.Column.FieldName.ToString().Equals("CO_UT_PRC") ? true : false);
        //        //주거래처
        //        bool mnCoFlg = (e.Column.FieldName.ToString().Equals("MN_CO_FLG") ? true : false);
        //        //현재단가
        //        bool crntPrcFlg = (e.Column.FieldName.ToString().Equals("CRNT_PRC_FLG") ? true : false);
        //        //변경사유
        //        bool chgHisDesc = (e.Column.FieldName.ToString().Equals("CNG_HIS_DESC") ? true : false);
        //        //품번(거래처)
        //        bool coItmCd = (e.Column.FieldName.ToString().Equals("CO_ITM_CD") ? true : false);
        //        //품명(거래처)
        //        bool coItmNm = (e.Column.FieldName.ToString().Equals("CO_ITM_NM") ? true : false);






        //        //거래처 명
        //        if (coNm)
        //        {
        //            if (e.IsValid)
        //            {
        //                if (e.Value == null)
        //                {
        //                    masterDomain.CO_NM = "";
        //                    masterDomain.CO_NO = "";
        //                }
        //                else
        //                {
        //                    if (masterDomain.CO_NM == null)
        //                    {
        //                        masterDomain.CO_NM = string.Empty;
        //                        masterDomain.CO_NO = string.Empty;
        //                    }
        //                    //
        //                    if (!masterDomain.CO_NM.Equals((e.Value == null ? "" : e.Value.ToString())))
        //                    {
        //                        masterDomain.isCheckd = true;
        //                        this.M_SAVE.IsEnabled = true;
        //                    }
        //                }
        //            }
        //        }

        //        //통화
        //        if (currCd)
        //        {
        //            if (e.IsValid)
        //            {
        //                if (e.Value == null)
        //                {
        //                    masterDomain.CURR_CD = "";
        //                    masterDomain.CURR_NM = "";
        //                }
        //                else
        //                {
        //                    if (masterDomain.CURR_CD == null)
        //                    {
        //                        masterDomain.CURR_CD = string.Empty;
        //                        masterDomain.CURR_NM = string.Empty;
        //                    }
        //                    //
        //                    if (!masterDomain.CURR_CD.Equals((e.Value == null ? "" : e.Value.ToString())))
        //                    {
        //                        masterDomain.isCheckd = true;
        //                        this.M_SAVE.IsEnabled = true;
        //                    }
        //                }
        //            }
        //        }

        //        //적용 일자
        //        if (cngAplyDt)
        //        {
        //            if (e.IsValid)
        //            {
        //                if (e.Value == null)
        //                {
        //                    masterDomain.CNG_APLY_DT = "";
        //                }
        //                else
        //                {
        //                    if (masterDomain.CNG_APLY_DT == null)
        //                    {
        //                        masterDomain.CNG_APLY_DT = string.Empty;
        //                    }
        //                    //
        //                    if (!masterDomain.CURR_CD.Equals((e.Value == null ? "" : e.Value.ToString())))
        //                    {
        //                        masterDomain.isCheckd = true;
        //                        this.M_SAVE.IsEnabled = true;
        //                    }
        //                }
        //            }
        //        }

        //        //단가
        //        if (coUtPrc)
        //        {
        //            if (e.IsValid)
        //            {
        //                if (e.Value == null)
        //                {
        //                    masterDomain.CO_UT_PRC = 0;
        //                }
        //                else
        //                {
        //                    if (masterDomain.CO_UT_PRC == null)
        //                    {
        //                        masterDomain.CO_UT_PRC = 0;
        //                    }
        //                    //
        //                    if (!masterDomain.CO_UT_PRC.Equals((e.Value == null ? "" : e.Value.ToString())))
        //                    {
        //                        masterDomain.isCheckd = true;
        //                        this.M_SAVE.IsEnabled = true;
        //                    }
        //                }
        //            }
        //        }

        //        //주거래처
        //        if (mnCoFlg)
        //        {
        //            if (e.IsValid)
        //            {
        //                if (e.Value == null)
        //                {
        //                    masterDomain.MN_CO_FLG = false;
        //                }
        //                else
        //                {
        //                    if (masterDomain.MN_CO_FLG == null)
        //                    {
        //                        masterDomain.MN_CO_FLG = false;
        //                    }
        //                    //
        //                    if (!masterDomain.MN_CO_FLG.Equals((e.Value == null ? "" : e.Value.ToString())))
        //                    {
        //                        masterDomain.isCheckd = true;
        //                        this.M_SAVE.IsEnabled = true;
        //                    }
        //                }
        //            }
        //        }

        //        //현재단가
        //        if (crntPrcFlg)
        //        {
        //            if (e.IsValid)
        //            {
        //                if (e.Value == null)
        //                {
        //                    masterDomain.CRNT_PRC_FLG = false;
        //                }
        //                else
        //                {
        //                    if (masterDomain.CRNT_PRC_FLG == null)
        //                    {
        //                        masterDomain.CRNT_PRC_FLG = false;
        //                    }
        //                    //
        //                    if (!masterDomain.CRNT_PRC_FLG.Equals((e.Value == null ? "" : e.Value.ToString())))
        //                    {
        //                        masterDomain.isCheckd = true;
        //                        this.M_SAVE.IsEnabled = true;
        //                    }
        //                }
        //            }
        //        }

        //        //변경사유
        //        if (chgHisDesc)
        //        {
        //            if (e.IsValid)
        //            {
        //                if (e.Value == null)
        //                {
        //                    masterDomain.CNG_HIS_DESC = "";
        //                }
        //                else
        //                {
        //                    if (masterDomain.CNG_HIS_DESC == null)
        //                    {
        //                        masterDomain.CNG_HIS_DESC = "";
        //                    }
        //                    //
        //                    if (!masterDomain.CNG_HIS_DESC.Equals((e.Value == null ? "" : e.Value.ToString())))
        //                    {
        //                        masterDomain.isCheckd = true;
        //                        this.M_SAVE.IsEnabled = true;
        //                    }
        //                }
        //            }
        //        }

        //        //품번(거래처)
        //        if (coItmCd)
        //        {
        //            if (e.IsValid)
        //            {
        //                if (e.Value == null)
        //                {
        //                    masterDomain.CO_ITM_CD = "";
        //                }
        //                else
        //                {
        //                    if (masterDomain.CO_ITM_CD == null)
        //                    {
        //                        masterDomain.CO_ITM_CD = "";
        //                    }
        //                    //
        //                    if (!masterDomain.CO_ITM_CD.Equals((e.Value == null ? "" : e.Value.ToString())))
        //                    {
        //                        masterDomain.isCheckd = true;
        //                        this.M_SAVE.IsEnabled = true;
        //                    }
        //                }
        //            }
        //        }


        //        //품명(거래처)
        //        if (coItmNm)
        //        {
        //            if (e.IsValid)
        //            {
        //                if (e.Value == null)
        //                {
        //                    masterDomain.CO_ITM_NM = "";
        //                }
        //                else
        //                {
        //                    if (masterDomain.CO_ITM_NM == null)
        //                    {
        //                        masterDomain.CO_ITM_NM = "";
        //                    }
        //                    //
        //                    if (!masterDomain.CO_ITM_NM.Equals((e.Value == null ? "" : e.Value.ToString())))
        //                    {
        //                        masterDomain.isCheckd = true;
        //                        this.M_SAVE.IsEnabled = true;
        //                    }
        //                }
        //            }
        //        }

        //    }
        //    catch (Exception eLog)
        //    {
        //        e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
        //        e.ErrorContent = eLog.Message;
        //        e.SetError(e.ErrorContent, e.ErrorType);
        //        return;
        //    }
        //}
        //private void viewPage1EditView_HiddenEditor(object sender, DevExpress.Xpf.Grid.EditorEventArgs e)
        //{
        //    this.ViewTableDtl.CommitEditing();
        //    int rowHandle = this.ViewTableDtl.FocusedRowHandle + 1;

        //    //TbBatSerGenDao masterDomain = (TbBatSerGenDao)ConfigViewPage1Edit_Master.GetFocusedRow();
        //    //if (masterDomain == null) { return; }
        //    //masterDomain.IS_CHECK = true;
        //    this.ViewGridDtl.RefreshRow(rowHandle - 1);
        //    //
        //    //this.btn_save.IsEnabled = true;
        //    //this.tableView.FocusedRowHandle = rowHandle;
        //    this.ViewTableDtl.TopRowIndex = (rowHandle - 8);
        //}

        //private void M_SAVE_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        //{
        //    this.ViewTableDtl.Focus();
        //}

        //private void M_SEARCH_TEXT_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        //{
        //    if (e.Key == Key.Enter)
        //    {
        //        M_REFRESH_ItemClick(sender, null);
        //    }
        //}

    }
}