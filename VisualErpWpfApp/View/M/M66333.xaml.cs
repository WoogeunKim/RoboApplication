using AquilaErpWpfApp3.Util;
using AquilaErpWpfApp3.ViewModel;
using DevExpress.Xpf.Printing;
using DevExpress.XtraPrinting.Drawing;
using ModelsLibrary.Code;
using ModelsLibrary.Man;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AquilaErpWpfApp3.View.M
{


    /// <summary>
    /// M66333.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class M66333 : UserControl
    {
        private string title = "수동수주출력";

        public M66333()
        {
            DataContext = new M66333ViewModel();
            InitializeComponent();
        }

        private async void GridColumn_Validate(object sender, DevExpress.Xpf.Grid.GridCellValidationEventArgs e)
        {
            try
            {
                ManVo masterDomain = (ManVo)DetailTable.GetFocusedRow();

                bool itmshpcd = (e.Column.FieldName.ToString().Equals("ITM_SHP_CD") ? true : false);
                bool itmstlcd = (e.Column.FieldName.ToString().Equals("ITM_STL_CD") ? true : false);
                bool itmstlszcd = (e.Column.FieldName.ToString().Equals("ITM_STL_SZ_CD") ? true : false);

                int _Num;

                if (itmshpcd)
                {
                    if (e.IsValid)
                    {
                        if (string.IsNullOrEmpty(masterDomain.ITM_SHP_CD + ""))
                        {
                            masterDomain.ITM_SHP_CD = "";
                            masterDomain.ITM_SHP_NM = "";
                        }
                        if (!masterDomain.ITM_SHP_CD.Equals((e.Value == null ? "" : e.Value.ToString())))
                        {
                            ManVo ItmShpDao = this.lue_ITM_SHP_CD.GetItemFromValue(e.Value) as ManVo;
                            if (ItmShpDao != null)
                            {
                                masterDomain.ITM_SHP_CD = ItmShpDao.ITM_CD;
                                masterDomain.ITM_SHP_NM = ItmShpDao.ITM_NM;
                            }
                        }
                    }
                }
                else if (itmstlcd)
                {

                    if (e.IsValid)
                    {
                        if (string.IsNullOrEmpty(masterDomain.ITM_STL_CD + ""))
                        {
                            masterDomain.ITM_STL_CD = "";
                            masterDomain.ITM_STL_NM = "";
                        }
                        if (!masterDomain.ITM_STL_CD.Equals((e.Value == null ? "" : e.Value.ToString())))
                        {
                            SystemCodeVo ItmShpDao = this.lue_ITM_STL_CD.GetItemFromValue(e.Value) as SystemCodeVo;
                            if (ItmShpDao != null)
                            {
                                masterDomain.ITM_STL_CD = ItmShpDao.ITM_GRP_CD;
                                masterDomain.ITM_STL_NM = ItmShpDao.ITM_GRP_NM;
                            }
                        }
                    }
                }

                else if (itmstlszcd)
                {

                    if (e.IsValid)
                    {
                        if (string.IsNullOrEmpty(masterDomain.ITM_STL_SZ_CD + ""))
                        {
                            masterDomain.ITM_STL_SZ_CD = "";
                            masterDomain.ITM_STL_SZ_NM = "";
                        }
                        if (!masterDomain.ITM_STL_SZ_CD.Equals((e.Value == null ? "" : e.Value.ToString())))
                        {
                            SystemCodeVo ItmShpDao = this.lue_ITM_STL_SZ_CD.GetItemFromValue(e.Value) as SystemCodeVo;
                            if (ItmShpDao != null)
                            {
                                masterDomain.ITM_STL_SZ_CD = ItmShpDao.ITM_GRP_CD;
                                masterDomain.ITM_STL_SZ_NM = ItmShpDao.ITM_GRP_NM;
                            }
                        }
                    }
                }
                if (itmshpcd || itmstlcd || itmstlszcd)
                {
                    masterDomain.UPD_USR_ID = SystemProperties.USER;

                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m66333/dtl/u", new StringContent(JsonConvert.SerializeObject(masterDomain), System.Text.Encoding.UTF8, "application/json")))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string result = await response.Content.ReadAsStringAsync();
                            if (int.TryParse(result, out _Num) == false)
                            {
                                //실패
                                //WinUIMessageBox.Show(result, _title, MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                        }
                    }
                }
                this.DetailTable.RefreshData();

            }
            catch (Exception eLog)
            {
                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                e.ErrorContent = eLog.Message;
                e.SetError(e.ErrorContent, e.ErrorType);
                return;
            }
        }


        private void M_DTL_PRINT_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            if ((DetailTable.ItemsSource as List<ManVo>).Count.Equals(0)) return;

            this.ViewDetail.PrintAutoWidth = true;
            //this.tableView.AutoWidth = true;
            this.ViewDetail.BestFitColumns();
            //IList<GridColumn> columns = this.tableView.VisibleColumns;
            //columns[0].Visible = false;
            //columns[0].AllowEditing = DevExpress.Utils.DefaultBoolean.True;


            //P4426ViewModel viewModel = (P4426ViewModel)this.DataContext;
            ManVo vo = new ManVo();
            vo = this.MasterTable.SelectedItem as ManVo;

            StringBuilder sb = new StringBuilder();
            sb.Append("<DataTemplate xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" xmlns:dxe=\"http://schemas.devexpress.com/winfx/2008/xaml/editors\">");
            sb.Append("<dxe:TextEdit Text=\"" + "[수주번호 : " + vo.SL_RLSE_NO + ", 수주일자 : " + vo.SL_RLSE_DT + ", 거래처 : " + vo.CO_NM + "] " + "\" FontWeight=\"Bold\"  FontSize=\"8\"  />");
            sb.Append("</DataTemplate>");
            DataTemplate templateHeader = (DataTemplate)System.Windows.Markup.XamlReader.Parse(sb.ToString());

            sb = new StringBuilder();
            sb.Append("<DataTemplate xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" xmlns:dxe=\"http://schemas.devexpress.com/winfx/2008/xaml/editors\">");
            sb.Append("<dxe:TextEdit Text=\"" + System.DateTime.Now.ToString("yyyy-MM-dd hh:mm") + "\"/>");
            sb.Append("</DataTemplate>");
            DataTemplate templateFooter = (DataTemplate)System.Windows.Markup.XamlReader.Parse(sb.ToString());

            using (PrintableControlLink prtLink = new PrintableControlLink(this.ViewDetail))
            {
                //prtLink.PageHeaderData = "인쇄 일자 : " + Convert.ToDateTime(this.txt_stDate.Text).ToString("yyyy-MM-dd");
                prtLink.PageHeaderTemplate = templateHeader;
                prtLink.PageFooterTemplate = templateFooter;
                prtLink.PageHeaderData = null;
                prtLink.PageFooterData = null;


                prtLink.Margins.Top = 8;
                prtLink.Margins.Bottom = 8;
                prtLink.Margins.Left = 5;
                prtLink.Margins.Right = 5;

                prtLink.DocumentName = "수동수주출력-Print";
                prtLink.PrintingSystem.Watermark.TextDirection = DirectionMode.ForwardDiagonal;
                prtLink.PrintingSystem.Watermark.Font = new Font(prtLink.PrintingSystem.Watermark.Font.FontFamily, 40);
                prtLink.PrintingSystem.Watermark.ForeColor = System.Drawing.Color.PaleTurquoise;
                prtLink.PrintingSystem.Watermark.TextTransparency = 150;

                prtLink.Landscape = true;
                prtLink.PrintingSystem.ShowPrintStatusDialog = true;

                prtLink.PaperKind = System.Drawing.Printing.PaperKind.A4;
                prtLink.CreateDocument(true);
                prtLink.ShowPrintPreviewDialog(Application.Current.MainWindow, "수동수주출력");
            }


        }
    }



}
