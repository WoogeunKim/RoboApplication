using AquilaErpWpfApp3.ViewModel;
using DevExpress.Xpf.Printing;
using ModelsLibrary.Inv;
using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace AquilaErpWpfApp3.View.INV
{
    /// <summary>
    /// P4415.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class I6618 : UserControl
    {
        //private static PurServiceClient purClient = SystemProperties.PurClient;
        //private static SaleOrderServiceClient saleOrderClient = SystemProperties.SaleOrderClient;
        //private IList<PurVo> selectedMstList = new List<PurVo>();



        public I6618()
        {
            DataContext = new I6618ViewModel();
            InitializeComponent();
        }

        //private void M_SEARCH_TEXT_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        //{
        //    if (e.Key == Key.Enter)
        //    {
        //        M_REFRESH_ItemClick(sender, null);
        //    }
        //}

        //private void M_REFRESH_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        //{
        //    try
        //    {
        //        this.ConfigViewPage1Edit_Master.ShowLoadingPanel = true;
        //        this.configViewPage1EditView_Master.SearchString = (this.M_SEARCH_TEXT.EditValue == null ? "" : this.M_SEARCH_TEXT.EditValue.ToString());
        //        //this.txt_Search.SelectAll();
        //        this.M_SEARCH_TEXT.Focus();
        //        this.ConfigViewPage1Edit_Master.ShowLoadingPanel = false;
        //    }
        //    catch (Exception eLog)
        //    {
        //        this.ConfigViewPage1Edit_Master.ShowLoadingPanel = false;
        //        WinUIMessageBox.Show(eLog.Message, "[에러]" , MessageBoxButton.OK, MessageBoxImage.Error);
        //        //this.M_SEARCH_TEXT.SelectAll();
        //        this.M_SEARCH_TEXT.Focus();
        //        return;
        //    }
        //}

        private void M_MST_PRINT_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            //this.ViewTableMst.PrintAutoWidth = true;
            ////this.tableView.AutoWidth = true;
            //this.ViewTableMst.BestFitColumns();
            ////IList<GridColumn> columns = this.tableView.VisibleColumns;
            ////columns[0].Visible = false;
            ////columns[0].AllowEditing = DevExpress.Utils.DefaultBoolean.True;

            //InvVo mstDomain = (InvVo)ConfigViewPage1Edit_Master.GetFocusedRow();

            //StringBuilder sb = new StringBuilder();
            //sb.Append("<DataTemplate xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" xmlns:dxe=\"http://schemas.devexpress.com/winfx/2008/xaml/editors\">");
            ////sb.Append("<dxe:TextEdit Text=\"" + "[일자(From) : " + Convert.ToDateTime(M_FM_DT.EditValue).ToString("yyyy-MM-dd hh:mm") + "~" + "(To) " + Convert.ToDateTime(M_TO_DT.EditValue).ToString("yyyy-MM-dd hh:mm") + ", 거래처 : " + mstDomain.CO_NM + "] " + "\" FontWeight=\"Bold\"  FontSize=\"8\"  />");
            //sb.Append("<dxe:TextEdit Text=\"" + "[일자(From) : " + Convert.ToDateTime(M_FM_DT.EditValue).ToString("yyyy-MM-dd") + "~" + "(To) " + Convert.ToDateTime(M_TO_DT.EditValue).ToString("yyyy-MM-dd") + ", 거래처 : " + mstDomain.CO_NM + "] " + "\" FontWeight=\"Bold\"  FontSize=\"8\"  />");
            //sb.Append("</DataTemplate>");
            //DataTemplate templateHeader = (DataTemplate)System.Windows.Markup.XamlReader.Parse(sb.ToString());

            //sb = new StringBuilder();
            //sb.Append("<DataTemplate xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" xmlns:dxe=\"http://schemas.devexpress.com/winfx/2008/xaml/editors\">");
            //sb.Append("<dxe:TextEdit Text=\""+"인쇄일자 : " + System.DateTime.Now.ToString("yyyy-MM-dd hh:mm") + "\"/>");
            //sb.Append("</DataTemplate>");
            //DataTemplate templateFooter = (DataTemplate)System.Windows.Markup.XamlReader.Parse(sb.ToString());


            //using (PrintableControlLink prtLink = new PrintableControlLink(this.configViewPage1EditView_Detail))
            //{
            //    prtLink.PageHeaderTemplate = templateHeader;
            //    //prtLink.PageFooterData = "" + System.DateTime.Now.ToString("yyyy-MM-dd hh:mm");
            //    //prtLink.PageFooterTemplate = Resources["PageFooter"] as DataTemplate;
            //    prtLink.PageFooterTemplate = templateFooter;
            //    prtLink.PageHeaderData = null;
            //    prtLink.PageFooterData = null;

            //    prtLink.Margins.Top = 8;
            //    prtLink.Margins.Bottom = 8;
            //    prtLink.Margins.Left = 5;
            //    prtLink.Margins.Right = 5;

            //    prtLink.DocumentName = "주문대비 출고현황 Print";
            //    //prtLink.PrintingSystem.Watermark.Text = "한양화스너공업(주)";
            //    //prtLink.PrintingSystem.Watermark.TextDirection = DirectionMode.ForwardDiagonal;
            //    //prtLink.PrintingSystem.Watermark.Font = new Font(prtLink.PrintingSystem.Watermark.Font.FontFamily, 40);
            //    //prtLink.PrintingSystem.Watermark.ForeColor = System.Drawing.Color.PaleTurquoise;
            //    //prtLink.PrintingSystem.Watermark.TextTransparency = 150;

            //    prtLink.Landscape = true;
            //    prtLink.PrintingSystem.ShowPrintStatusDialog = true;

            //    prtLink.PaperKind = System.Drawing.Printing.PaperKind.A4;
            //    prtLink.CreateDocument(true);
            //    prtLink.ShowPrintPreviewDialog(Application.Current.MainWindow, "주문대비 출고현황");
            //}
        }

    }
}
