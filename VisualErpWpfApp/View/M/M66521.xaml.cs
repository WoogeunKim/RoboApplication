using AquilaErpWpfApp3.ViewModel;
using DevExpress.Xpf.Printing;
using DevExpress.XtraPrinting.Drawing;
using System.Drawing;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace AquilaErpWpfApp3.View.M
{
    /// <summary>
    /// M66521.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class M66521 : UserControl
    {
        private string title = "계획대비 실적 리포트(충전)";

        public M66521()
        {                 
             InitializeComponent();

             DataContext = new M66521ViewModel();
        }


        private void M_MST_REPORT_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            this.ViewTableMst.PrintAutoWidth = true;

            using (PrintableControlLink prtLink = new PrintableControlLink(this.ViewTableMst))
            {

                StringBuilder sb = new StringBuilder();
                sb.Append("<DataTemplate xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" xmlns:dxe=\"http://schemas.devexpress.com/winfx/2008/xaml/editors\">");
                sb.Append("<dxe:TextEdit Text=\"" + "계획대비 실적 리포트(충전)" + "\" FontWeight=\"Bold\"  FontSize=\"10\"  />");
                sb.Append("</DataTemplate>");
                DataTemplate templateHeader = (DataTemplate)System.Windows.Markup.XamlReader.Parse(sb.ToString());

                sb = new StringBuilder();
                sb.Append("<DataTemplate xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" xmlns:dxe=\"http://schemas.devexpress.com/winfx/2008/xaml/editors\">");
                sb.Append("<dxe:TextEdit Text=\"" + "인쇄 일자 : " + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "\"/>");
                sb.Append("</DataTemplate>");
                DataTemplate templateFooter = (DataTemplate)System.Windows.Markup.XamlReader.Parse(sb.ToString());

                prtLink.PageHeaderTemplate = templateHeader;
                prtLink.PageFooterTemplate = templateFooter;

                prtLink.Margins.Top = 8;
                prtLink.Margins.Bottom = 8;
                prtLink.Margins.Left = 5;
                prtLink.Margins.Right = 5;
                prtLink.DocumentName = this.title + " Print";

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


    }
}
