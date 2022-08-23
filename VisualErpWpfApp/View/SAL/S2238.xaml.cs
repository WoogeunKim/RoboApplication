using DevExpress.Data;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Sale;
using System;
using System.Windows;
using System.Windows.Controls;
using AquilaErpWpfApp3.Util;
using AquilaErpWpfApp3.ViewModel;

namespace AquilaErpWpfApp3.View.SAL
{
    public partial class S2238 : UserControl
    {
        //private static SaleOrderServiceClient saleOrderClient = SystemProperties.SaleOrderClient;
        private string title = "미수금현황";
        public S2238()
        {
            DataContext = new S2238ViewModel();
            //
            InitializeComponent();
           
        }

        private void CheckEdit_Checked(object sender, RoutedEventArgs e)
        {
            //CheckEdit checkEdit = sender as CheckEdit;
            //JobVo tmpImsi;
            //JobVo resultVo;
            //for (int x = 0; x < this.ViewGridDtl.VisibleRowCount; x++)
            //{
            //    int rowHandle = this.ViewGridDtl.GetRowHandleByVisibleIndex(x);
            //    if (rowHandle > -1)
            //    {
            //        tmpImsi = this.ViewGridDtl.GetRow(rowHandle) as JobVo;
            //        if (checkEdit.IsChecked == true)
            //        {
            //            tmpImsi.CLZ_FLG = "Y";
            //            tmpImsi.isCheckd = true;
            //        }
            //        else
            //        {
            //            tmpImsi.CLZ_FLG = "N";
            //            tmpImsi.isCheckd = false;
            //        }
            //        //
            //        tmpImsi.CRE_USR_ID = SystemProperties.USER;
            //        tmpImsi.UPD_USR_ID = SystemProperties.USER;
            //        //
            //        //
            //        resultVo = saleOrderClient.S2211UpdateDtl(tmpImsi);
            //        if (!resultVo.isSuccess)
            //        {
            //            //실패
            //            WinUIMessageBox.Show(resultVo.Message, "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
            //            return;
            //        }
            //    }
            //}
        }

        //private void M_REFRESH_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        //{
        //    try
        //    {
        //        //this.ViewGridMst.ShowLoadingPanel = true;
        //        this.ViewTableMst.SearchString = (this.M_SEARCH_TEXT.EditValue == null ? "" : this.M_SEARCH_TEXT.EditValue.ToString());
        //        //this.txt_Search.SelectAll();
        //        this.M_SEARCH_TEXT.Focus();
        //        //this.ViewGridMst.ShowLoadingPanel = false;

        //        ((S2238ViewModel)this.DataContext).setTitle();
        //    }
        //    catch (Exception eLog)
        //    {
        //        //this.ViewGridMst.ShowLoadingPanel = false;
        //        WinUIMessageBox.Show(eLog.Message, "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
        //        //this.M_SEARCH_TEXT.SelectAll();
        //        this.M_SEARCH_TEXT.Focus();
        //        return;
        //    }
        //}


        private void isCheckEdit_DtlClzFlg(object sender, RoutedEventArgs e)
        {
            //CheckEdit checkEdit = sender as CheckEdit;
            //JobVo masterDomain = (JobVo)ViewGridDtl.GetFocusedRow();
            //JobVo resultVo;
            ////
            //if (checkEdit.IsChecked == true)
            //{
            //    //Y
            //    masterDomain.CLZ_FLG = "Y";
            //    masterDomain.isCheckd = true;
            //}
            //else
            //{
            //    //N
            //    masterDomain.CLZ_FLG = "N";
            //    masterDomain.isCheckd = false;
            //}

            ////
            //masterDomain.CRE_USR_ID = SystemProperties.USER;
            //masterDomain.UPD_USR_ID = SystemProperties.USER;
            ////
            ////
            //resultVo = saleOrderClient.S2211UpdateDtl(masterDomain);
            //if (!resultVo.isSuccess)
            //{
            //    //실패
            //    WinUIMessageBox.Show(resultVo.Message, "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
            //    return;
            //}
        }

        //private void CheckEdit_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        //{
        //    FrameworkElement editor = (FrameworkElement)sender;
        //    GridGroupValueData data = (GridGroupValueData)editor.DataContext;

        //    GridControl grid = ((TableView)data.View).Grid;
        //    int currentGroupRowHandle = data.RowData.RowHandle.Value;

        //    int childRowsCount = grid.GetChildRowCount(currentGroupRowHandle);
        //    for (int index = 0; index < childRowsCount; index++)
        //    {
        //        int childRowHandle = grid.GetChildRowHandle(currentGroupRowHandle, index);
        //        if (grid.IsGroupRowHandle(childRowHandle))
        //        {
        //            GroupRowSelectionHelper helper = (GroupRowSelectionHelper)grid.GetValue(GroupRowSelectionHelper.GroupRowSelectionHelperProperty);
        //            helper.SetNewIsSelectedValue(childRowHandle, (bool)e.NewValue);
        //        }
        //        else
        //        {
        //            grid.SetCellValue(childRowHandle, "IsSelected", e.NewValue);
        //        }
        //    }
        //}


        private void M_PRINT_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            try
            {
                this.ViewTableMst.PrintAutoWidth = true;

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("<DataTemplate xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" xmlns:dxe=\"http://schemas.devexpress.com/winfx/2008/xaml/editors\">");
                sb.Append("<dxe:TextEdit Text=\"" + "[ 미수금현황 - 출력일시 : " + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "] " + "\" FontWeight=\"Bold\"  FontSize=\"11\"  />");
                sb.Append("</DataTemplate>");
                DataTemplate templateHeader = (DataTemplate)System.Windows.Markup.XamlReader.Parse(sb.ToString());

                using (PrintableControlLink prtLink = new PrintableControlLink(this.ViewTableMst))
                {
                    //prtLink.PageHeaderData = "인쇄 일자 : ";// +Convert.ToDateTime(this.txt_stDate.Text).ToString("yyyy-MM-dd");
                    prtLink.PageHeaderTemplate = templateHeader;//Resources["PageHeader"] as DataTemplate;
                    prtLink.PageFooterData = "" + System.DateTime.Now.ToString("yyyy-MM-dd hh:mm");
                    prtLink.PageFooterTemplate = Resources["PageFooter"] as DataTemplate;

                    prtLink.Margins.Top = 8;
                    prtLink.Margins.Bottom = 8;
                    prtLink.Margins.Left = 5;
                    prtLink.Margins.Right = 5;
                    prtLink.DocumentName = this.title + "Print";

                    prtLink.Landscape = true;
                    prtLink.PrintingSystem.ShowPrintStatusDialog = true;

                    prtLink.PaperKind = System.Drawing.Printing.PaperKind.A4;
                    prtLink.CreateDocument(true);
                    prtLink.ShowPrintPreviewDialog(Application.Current.MainWindow, this.title);
                }
            }
            catch (System.Exception eLog)
            {

                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
            
        }

        //private void M_SAVE_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        //{
        //    //this.ViewTableDtl.Focus();
        //}

        //private void M_SEARCH_TEXT_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        //{
        //    if (e.Key == Key.Enter)
        //    {
        //        M_REFRESH_ItemClick(sender, null);
        //    }
        //}

        private SaleVo jobVoGroup;
        private void grid_CustomSummary(object sender, CustomSummaryEventArgs e)
        {
            try
            {
                if (((GridSummaryItem)e.Item).FieldName.Equals("CO_NM"))
                {                   
                    }
                    if (e.IsGroupSummary)
                    {
                        if (e.SummaryProcess == CustomSummaryProcess.Start)
                        {
                        }

                        if (e.SummaryProcess == CustomSummaryProcess.Calculate)
                        {
                            jobVoGroup = e.Row as SaleVo;

                            e.TotalValue = jobVoGroup.SL_AREA_NM + "  소계 :";
                        }
                    }
            }
            catch (Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
    }
}