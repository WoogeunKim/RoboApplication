using AquilaErpWpfApp3.Util;
using AquilaErpWpfApp3.ViewModel;
using DevExpress.Xpf.Printing;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Man;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
    /// M66311.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class M66311 : UserControl
    {
        private string _title = "가공생산지시관리";

        public M66311()
        {
            InitializeComponent();

            DataContext = new M66311ViewModel();
        }


        private void M_MST_PRINT_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            //this.ViewTableMst.PrintAutoWidth = true;
            ////this.tableView.AutoWidth = true;
            //this.ViewTableMst.BestFitColumns();
            ////IList<GridColumn> columns = this.tableView.VisibleColumns;
            ////columns[0].Visible = false;
            ////columns[0].AllowEditing = DevExpress.Utils.DefaultBoolean.True;
            ManVo selVo = (ManVo)this.ConfigViewPage1Edit_Master.SelectedItem;

            if (selVo != null)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<DataTemplate xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" xmlns:dxe=\"http://schemas.devexpress.com/winfx/2008/xaml/editors\">");
                sb.Append("<dxe:TextEdit Text=\"" + this._title + "\" FontWeight=\"Bold\"  FontSize=\"10\"  />");
                sb.Append("</DataTemplate>");
                DataTemplate templateHeader = (DataTemplate)System.Windows.Markup.XamlReader.Parse(sb.ToString());

                sb = new StringBuilder();
                sb.Append("<DataTemplate xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" xmlns:dxe=\"http://schemas.devexpress.com/winfx/2008/xaml/editors\">");
                sb.Append("<dxe:TextEdit Text=\"" + System.DateTime.Now.ToString("yyyy-MM-dd hh:mm") + "\"/>");
                sb.Append("</DataTemplate>");
                DataTemplate templateFooter = (DataTemplate)System.Windows.Markup.XamlReader.Parse(sb.ToString());


                using (PrintableControlLink prtLink = new PrintableControlLink(this.configViewPage1EditView_Master))
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

                    prtLink.DocumentName = this._title + " - Print";
                    //prtLink.PrintingSystem.Watermark.Text = "한양화스너공업(주)";
                    //prtLink.PrintingSystem.Watermark.TextDirection = DirectionMode.ForwardDiagonal;
                    //prtLink.PrintingSystem.Watermark.Font = new Font(prtLink.PrintingSystem.Watermark.Font.FontFamily, 40);
                    //prtLink.PrintingSystem.Watermark.ForeColor = System.Drawing.Color.PaleTurquoise;
                    //prtLink.PrintingSystem.Watermark.TextTransparency = 150;

                    prtLink.Landscape = true;
                    prtLink.PrintingSystem.ShowPrintStatusDialog = true;

                    prtLink.PaperKind = System.Drawing.Printing.PaperKind.A4;
                    prtLink.CreateDocument(true);
                    prtLink.ShowPrintPreviewDialog(Application.Current.MainWindow, this._title);
                }
            }
        }



        //private async void GridColumn_Validate(object sender, DevExpress.Xpf.Grid.GridCellValidationEventArgs e)
        //{
        //    try
        //    {
        //        ManVo masterDomain = (ManVo)ConfigViewPage1Edit_Master.GetFocusedRow();

        //        bool n1st_eq_no = e.Column.FieldName.ToString().Equals("N1ST_EQ_NM") ? true : false;       // 절단설비
        //        bool n2nd_eq_no = e.Column.FieldName.ToString().Equals("N2ND_EQ_NM") ? true : false;       // 가공설비

        //        if (n1st_eq_no)
        //        {
        //            if (e.IsValid)
        //            {
        //                if (string.IsNullOrEmpty(masterDomain.N1ST_EQ_NM + ""))
        //                {
        //                    masterDomain.N1ST_EQ_NO = "";
        //                    masterDomain.N1ST_EQ_NM = "";
        //                }
        //                if (!masterDomain.N1ST_EQ_NM.Equals((e.Value == null ? "" : e.Value.ToString())))
        //                {
        //                    ManVo eqDao = this.lue_N1ST_EQ_NM.GetItemFromValue(e.Value) as ManVo;

        //                    if (eqDao != null)
        //                    {
        //                        masterDomain.N1ST_EQ_NO = eqDao.PROD_EQ_NO;
        //                        masterDomain.N1ST_EQ_NM = eqDao.EQ_NM;
        //                    }
        //                }
        //            }
        //        }
        //        else if (n2nd_eq_no)
        //        {
        //            if (e.IsValid)
        //            {
        //                if (string.IsNullOrEmpty(masterDomain.N2ND_EQ_NM + ""))
        //                {
        //                    masterDomain.N2ND_EQ_NO = "";
        //                    masterDomain.N2ND_EQ_NM = "";
        //                }
        //                if (!masterDomain.N2ND_EQ_NM.Equals((e.Value == null ? "" : e.Value.ToString())))
        //                {
        //                    ManVo eqDao = this.lue_N2ND_EQ_NM.GetItemFromValue(e.Value) as ManVo;

        //                    if (eqDao != null)
        //                    {
        //                        masterDomain.N2ND_EQ_NO = eqDao.PROD_EQ_NO;
        //                        masterDomain.N2ND_EQ_NM = eqDao.EQ_NM;
        //                    }
        //                }
        //            }
        //        }

        //        bool isChange = false;

        //        if (n1st_eq_no || n2nd_eq_no)
        //        {
        //            isChange = await UpHttpData("m66311/eq/u", masterDomain);
        //        }

        //        this.ConfigViewPage1Edit_Master.RefreshData();

        //        if (isChange)
        //        {
        //            // 리셋
        //            //(DataContext as M66311ViewModel).Refresh(masterDomain.OPMZ_NO);
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


        private async Task<bool> UpHttpData(string Path, object obj)
        {
            var ret = false;

            try
            {
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync(Path
                                                                                                  , new StringContent(JsonConvert.SerializeObject(obj), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        int _Num = 0;
                        string resultMsg = await response.Content.ReadAsStringAsync();
                        if (int.TryParse(resultMsg, out _Num) == false)
                        {
                            //실패
                            WinUIMessageBox.Show(resultMsg, _title + "- 설비수정 오류", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                        {
                            // 성공
                            ret = true;
                        }
                    }
                }
            }
            catch (Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, _title + "- 설비수정 오류", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
            }

            return ret;
        }
    }


}