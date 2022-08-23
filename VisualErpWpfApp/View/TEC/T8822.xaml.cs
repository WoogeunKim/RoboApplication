using System.Windows.Controls;
using AquilaErpWpfApp3.ViewModel;

namespace AquilaErpWpfApp3.View.TEC
{
    public partial class T8822 : UserControl
    {
        //private CustomerDialog customersDialog;
        private string _title = "수입검사 현황";

        public T8822()
        {
            InitializeComponent();

            DataContext = new T8812ViewModel();
            //

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
        //          WinUIMessageBox.Show(eLog.Message, "[에러]" + this._title, MessageBoxButton.OK, MessageBoxImage.Error);
        //          //this.M_SEARCH_TEXT.SelectAll();
        //          this.M_SEARCH_TEXT.Focus();
        //          return;
        //      }
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


        //private void M_MST_PRINT_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        //{
        //    //this.ViewTableMst.PrintAutoWidth = true;
        //    ////this.tableView.AutoWidth = true;
        //    //this.ViewTableMst.BestFitColumns();
        //    ////IList<GridColumn> columns = this.tableView.VisibleColumns;
        //    ////columns[0].Visible = false;
        //    ////columns[0].AllowEditing = DevExpress.Utils.DefaultBoolean.True;

        //    StringBuilder sb = new StringBuilder();
        //    sb.Append("<DataTemplate xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" xmlns:dxe=\"http://schemas.devexpress.com/winfx/2008/xaml/editors\">");
        //    sb.Append("<dxe:TextEdit Text=\"" + "[가입고 일자(From) : " + Convert.ToDateTime(M_FM_DT.EditValue).ToString("yyyy-MM-dd hh:mm") + "~" + "(To) " + Convert.ToDateTime(M_TO_DT.EditValue).ToString("yyyy-MM-dd hh:mm") + ", 사업장 : " + (string.IsNullOrEmpty(M_ITM_GRP_CLSS_CD.EditValue.ToString()) ? "전체" : M_ITM_GRP_CLSS_CD.EditValue) + "] " + "\" FontWeight=\"Bold\"  FontSize=\"10\"  />");
        //    sb.Append("</DataTemplate>");
        //    DataTemplate templateHeader = (DataTemplate)System.Windows.Markup.XamlReader.Parse(sb.ToString());

        //    sb = new StringBuilder();
        //    sb.Append("<DataTemplate xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" xmlns:dxe=\"http://schemas.devexpress.com/winfx/2008/xaml/editors\">");
        //    sb.Append("<dxe:TextEdit Text=\"" + System.DateTime.Now.ToString("yyyy-MM-dd hh:mm") + "\"/>");
        //    sb.Append("</DataTemplate>");
        //    DataTemplate templateFooter = (DataTemplate)System.Windows.Markup.XamlReader.Parse(sb.ToString());


        //    using (PrintableControlLink prtLink = new PrintableControlLink(this.ViewTableMst))
        //    {
        //        prtLink.PageHeaderTemplate = templateHeader;
        //        //prtLink.PageFooterData = "" + System.DateTime.Now.ToString("yyyy-MM-dd hh:mm");
        //        //prtLink.PageFooterTemplate = Resources["PageFooter"] as DataTemplate;
        //        prtLink.PageFooterTemplate = templateFooter;
        //        prtLink.PageHeaderData = null;
        //        prtLink.PageFooterData = null;

        //        prtLink.Margins.Top = 8;
        //        prtLink.Margins.Bottom = 8;
        //        prtLink.Margins.Left = 5;
        //        prtLink.Margins.Right = 5;

        //        //prtLink.DocumentName = this._title+" Print";
        //        //prtLink.PrintingSystem.Watermark.Text = "한양화스너공업(주)";
        //        //prtLink.PrintingSystem.Watermark.TextDirection = DirectionMode.ForwardDiagonal;
        //        //prtLink.PrintingSystem.Watermark.Font = new Font(prtLink.PrintingSystem.Watermark.Font.FontFamily, 40);
        //        //prtLink.PrintingSystem.Watermark.ForeColor = System.Drawing.Color.PaleTurquoise;
        //        //prtLink.PrintingSystem.Watermark.TextTransparency = 150;

        //        prtLink.Landscape = true;
        //        prtLink.PrintingSystem.ShowPrintStatusDialog = true;

        //        prtLink.PaperKind = System.Drawing.Printing.PaperKind.A4;
        //        prtLink.CreateDocument(true);
        //        prtLink.ShowPrintPreviewDialog(Application.Current.MainWindow, this._title);
        //    }
        //}
    }
}