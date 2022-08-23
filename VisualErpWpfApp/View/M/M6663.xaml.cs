using DevExpress.Xpf.Printing;
using ModelsLibrary.Man;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using AquilaErpWpfApp3.ViewModel;

namespace AquilaErpWpfApp3.View.S
{
    /// <summary>
    /// S1147.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class M6663 : UserControl
    {
        public M6663()
        {
            DataContext = new M6663ViewModel();
            InitializeComponent();
        }

        private void M_MST_PRINT_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            //this.ViewTableMst.PrintAutoWidth = true;
            ////this.tableView.AutoWidth = true;
            //this.ViewTableMst.BestFitColumns();
            ////IList<GridColumn> columns = this.tableView.VisibleColumns;
            ////columns[0].Visible = false;
            ////columns[0].AllowEditing = DevExpress.Utils.DefaultBoolean.True;
            ManVo selVo = (ManVo)this.ViewGridMst.SelectedItem;
            if (selVo != null)
            {


                StringBuilder sb = new StringBuilder();
                sb.Append("<DataTemplate xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" xmlns:dxe=\"http://schemas.devexpress.com/winfx/2008/xaml/editors\">");
                sb.Append("<dxe:TextEdit Text=\"" + "\" FontWeight=\"Bold\"  FontSize=\"10\"  />");
                sb.Append("</DataTemplate>");
                DataTemplate templateHeader = (DataTemplate)System.Windows.Markup.XamlReader.Parse(sb.ToString());

                sb = new StringBuilder();
                sb.Append("<DataTemplate xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" xmlns:dxe=\"http://schemas.devexpress.com/winfx/2008/xaml/editors\">");
                sb.Append("<dxe:TextEdit Text=\"" + System.DateTime.Now.ToString("yyyy-MM-dd hh:mm") + "\"/>");
                sb.Append("</DataTemplate>");
                DataTemplate templateFooter = (DataTemplate)System.Windows.Markup.XamlReader.Parse(sb.ToString());


                using (PrintableControlLink prtLink = new PrintableControlLink(this.ViewTableMst))
                {
                    //  prtLink.PageHeaderTemplate = templateHeader;
                    //prtLink.PageFooterData = "" + System.DateTime.Now.ToString("yyyy-MM-dd hh:mm");
                    //prtLink.PageFooterTemplate = Resources["PageFooter"] as DataTemplate;
                    prtLink.PageFooterTemplate = templateFooter;
                    prtLink.PageHeaderData = null;
                    prtLink.PageFooterData = null;

                    prtLink.Margins.Top = 8;
                    prtLink.Margins.Bottom = 8;
                    prtLink.Margins.Left = 5;
                    prtLink.Margins.Right = 5;

                    prtLink.DocumentName = "금형위치관리 - Print";
                    //prtLink.PrintingSystem.Watermark.Text = "한양화스너공업(주)";
                    //prtLink.PrintingSystem.Watermark.TextDirection = DirectionMode.ForwardDiagonal;
                    //prtLink.PrintingSystem.Watermark.Font = new Font(prtLink.PrintingSystem.Watermark.Font.FontFamily, 40);
                    //prtLink.PrintingSystem.Watermark.ForeColor = System.Drawing.Color.PaleTurquoise;
                    //prtLink.PrintingSystem.Watermark.TextTransparency = 150;

                    prtLink.Landscape = true;
                    prtLink.PrintingSystem.ShowPrintStatusDialog = true;

                    prtLink.PaperKind = System.Drawing.Printing.PaperKind.A4;
                    prtLink.CreateDocument(true);
                    prtLink.ShowPrintPreviewDialog(Application.Current.MainWindow, "금형위치관리");
                }
            }
        }
    }
}
