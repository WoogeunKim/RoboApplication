using DevExpress.Xpf.Printing;
using ModelsLibrary.Inv;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using AquilaErpWpfApp3.ViewModel;

namespace AquilaErpWpfApp3.View.M
{
    public partial class M66560 : UserControl
    {
        //private CustomerDialog customersDialog;
        //private IList<CodeDao> UserList;

        public M66560()
        {
            DataContext = new M66560ViewModel();
            //
            InitializeComponent();

            //UserList = SystemProperties.SYSTEM_CODE_VO("S-047");
            //if (UserList.Any<CodeDao>(x => x.CLSS_CD.Equals(SystemProperties.USER)))
            //{
            //    this.G_DTL_INSERT_INV.IsVisible = true;
            //    this.M_DTL_INSERT_INV.IsVisible = true;
            //}
            //else
            //{
            //    this.G_DTL_INSERT_INV.IsVisible = false;
            //    this.M_DTL_INSERT_INV.IsVisible = false;
            //}
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
        //          WinUIMessageBox.Show(eLog.Message, "[에러]품목 입고 관리", MessageBoxButton.OK, MessageBoxImage.Error);
        //          //this.M_SEARCH_TEXT.SelectAll();
        //          this.M_SEARCH_TEXT.Focus();
        //          return;
        //      }
        //}


        private void M_MST_REPORT_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
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
                    prtLink.DocumentName = "자재 청구 등록 Print";

                    //prtLink.PrintingSystem.Watermark.Text = "한양화스너공업(주)";
                    //prtLink.PrintingSystem.Watermark.TextDirection = DirectionMode.ForwardDiagonal;
                    //prtLink.PrintingSystem.Watermark.Font = new Font(prtLink.PrintingSystem.Watermark.Font.FontFamily, 40);
                    //prtLink.PrintingSystem.Watermark.ForeColor = System.Drawing.Color.PaleTurquoise;
                    //prtLink.PrintingSystem.Watermark.TextTransparency = 150;

                    prtLink.Landscape = true;
                    prtLink.PrintingSystem.ShowPrintStatusDialog = true;

                    prtLink.PaperKind = System.Drawing.Printing.PaperKind.A4;
                    prtLink.CreateDocument(true);
                    prtLink.ShowPrintPreviewDialog(Application.Current.MainWindow, "자재 청구 등록");
                }
            }
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