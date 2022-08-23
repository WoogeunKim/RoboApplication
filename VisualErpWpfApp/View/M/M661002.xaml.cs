using AquilaErpWpfApp3.ViewModel;
using DevExpress.Xpf.Printing;
using DevExpress.XtraPrinting.Drawing;
using System.Drawing;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace AquilaErpWpfApp3.View.M
{
    public partial class M661002 : UserControl
    {
        //private static SaleOrderServiceClient saleOrderClient = SystemProperties.SaleOrderClient;
        private string title = "실적현황 및 작업 지시서(일괄 발행)";

        //private string isDetailView;


        public M661002()
        {
            InitializeComponent();

            //this.ViewGridMst.SelectedItemChanged += ViewGridMst_SelectedItemChanged;
            //
            DataContext = new M661002ViewModel();
            
        }


        private void M_MST_PRINT_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            //this.ViewTableMst.PrintAutoWidth = true;
            ////this.tableView.AutoWidth = true;
            //this.ViewTableMst.BestFitColumns();
            ////IList<GridColumn> columns = this.tableView.VisibleColumns;
            ////columns[0].Visible = false;
            ////columns[0].AllowEditing = DevExpress.Utils.DefaultBoolean.True;

            StringBuilder sb = new StringBuilder();
            sb.Append("<DataTemplate xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" xmlns:dxe=\"http://schemas.devexpress.com/winfx/2008/xaml/editors\">");
            //sb.Append("<dxe:TextEdit Text=\"" + "[일자(From) : " + Convert.ToDateTime(M_FM_DT.EditValue).ToString("yyyy-MM-dd hh:mm") + "~" + "(To) " + Convert.ToDateTime(M_TO_DT.EditValue).ToString("yyyy-MM-dd hh:mm") + ", 사업장 : " + (string.IsNullOrEmpty(M_AREA_NM.EditValue.ToString()) ? "전체" : M_AREA_NM.EditValue) + "] " + "\" FontWeight=\"Bold\"  FontSize=\"10\"  />");
            sb.Append("</DataTemplate>");
            DataTemplate templateHeader = (DataTemplate)System.Windows.Markup.XamlReader.Parse(sb.ToString());

            sb = new StringBuilder();
            sb.Append("<DataTemplate xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" xmlns:dxe=\"http://schemas.devexpress.com/winfx/2008/xaml/editors\">");
            sb.Append("<dxe:TextEdit Text=\"" + System.DateTime.Now.ToString("yyyy-MM-dd hh:mm") + "\"/>");
            sb.Append("</DataTemplate>");
            DataTemplate templateFooter = (DataTemplate)System.Windows.Markup.XamlReader.Parse(sb.ToString());


            using (PrintableControlLink prtLink = new PrintableControlLink(this.ViewTableMst))
            {
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

                prtLink.DocumentName = title +" Print";
                //prtLink.PrintingSystem.Watermark.Text = "한양화스너공업(주)";
                prtLink.PrintingSystem.Watermark.TextDirection = DirectionMode.ForwardDiagonal;
                prtLink.PrintingSystem.Watermark.Font = new Font(prtLink.PrintingSystem.Watermark.Font.FontFamily, 40);
                prtLink.PrintingSystem.Watermark.ForeColor = System.Drawing.Color.PaleTurquoise;
                prtLink.PrintingSystem.Watermark.TextTransparency = 150;

                prtLink.Landscape = true;
                prtLink.PrintingSystem.ShowPrintStatusDialog = true;

                prtLink.PaperKind = System.Drawing.Printing.PaperKind.A4;
                prtLink.CreateDocument(true);
                prtLink.ShowPrintPreviewDialog(Application.Current.MainWindow, this.title);
            }
        }

        //void ViewGridMst_SelectedItemChanged(object sender, DevExpress.Xpf.Grid.SelectedItemChangedEventArgs e)
        //{
        //    this.isDetailView = ((S2221ViewModel)this.DataContext).DetailView;

        //    if (isDetailView.Equals("1"))
        //    {
        //        this.DetailView1.Visibility = System.Windows.Visibility.Visible;
        //        this.DetailView2.Visibility = System.Windows.Visibility.Hidden;
        //        this.DetailView1.Content = ((S2221ViewModel)this.DataContext).SelectedMstItem;
        //    }
        //    else if (isDetailView.Equals("2"))
        //    {
        //        this.DetailView1.Visibility = System.Windows.Visibility.Hidden;
        //        this.DetailView2.Visibility = System.Windows.Visibility.Visible;
        //        this.DetailView2.Content = ((S2221ViewModel)this.DataContext).SelectedMstItem;
        //    }
        //    else
        //    {
        //        this.DetailView1.Visibility = System.Windows.Visibility.Visible;
        //        this.DetailView2.Visibility = System.Windows.Visibility.Hidden;
        //        this.DetailView1.Content = ((S2221ViewModel)this.DataContext).SelectedMstItem;
        //    }
        //}

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
        //        this.ViewGridMst.ShowLoadingPanel = true;
        //        this.ViewTableMst.SearchString = (this.M_SEARCH_TEXT.EditValue == null ? "" : this.M_SEARCH_TEXT.EditValue.ToString());
        //        //this.txt_Search.SelectAll();
        //        this.M_SEARCH_TEXT.Focus();
        //        this.ViewGridMst.ShowLoadingPanel = false;

        //        ((S2222ViewModel)this.DataContext).setTitle();
        //    }
        //    catch (Exception eLog)
        //    {
        //        this.ViewGridMst.ShowLoadingPanel = false;
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

    }
}