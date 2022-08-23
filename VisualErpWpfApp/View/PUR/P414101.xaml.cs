using AquilaErpWpfApp3.Util;
using AquilaErpWpfApp3.ViewModel;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Printing;
using DevExpress.XtraPrinting.Drawing;
using ModelsLibrary.Man;
using ModelsLibrary.Pur;
using Newtonsoft.Json;
using System;
using System.Drawing;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace AquilaErpWpfApp3.View.PUR
{
    public partial class P414101 : UserControl
    {
        //private CustomerDialog customersDialog;
        private string _title = "BOM등록(구매)";

        public P414101()
        {
            DataContext = new P414101ViewModel();
            //
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


            using (PrintableControlLink prtLink = new PrintableControlLink(this.ViewTableDtl))
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

                prtLink.DocumentName = "BOM등록(구매) Print";
                //prtLink.PrintingSystem.Watermark.Text = "한양화스너공업(주)";
                prtLink.PrintingSystem.Watermark.TextDirection = DirectionMode.ForwardDiagonal;
                prtLink.PrintingSystem.Watermark.Font = new Font(prtLink.PrintingSystem.Watermark.Font.FontFamily, 40);
                prtLink.PrintingSystem.Watermark.ForeColor = System.Drawing.Color.PaleTurquoise;
                prtLink.PrintingSystem.Watermark.TextTransparency = 150;

                prtLink.Landscape = true;
                prtLink.PrintingSystem.ShowPrintStatusDialog = true;

                prtLink.PaperKind = System.Drawing.Printing.PaperKind.A4;
                prtLink.CreateDocument(true);
                prtLink.ShowPrintPreviewDialog(Application.Current.MainWindow, "BOM등록(구매)");
            }
        }


        void OnCustomColumnSort(object sender, CustomColumnSortEventArgs e)
        {
            if (e.Column.FieldName == "ORD_ITM_CD")
            {
                int dayIndex1 = GetIndex((string)e.Value1);
                int dayIndex2 = GetIndex((string)e.Value2);
                e.Result = dayIndex1.CompareTo(dayIndex2);
                e.Handled = true;
            }
        }

        int GetIndex(string day)
        {
            if (string.IsNullOrEmpty(day))
            {
                return 0;
            }
            else if (day.Contains("선착인"))
            {
                return 1;
            }
            else if (day.Contains("충전1"))
            {
                return 2;
            }
            else if (day.Contains("충전2"))
            {
                return 3;
            }
            else if (day.Contains("착인"))
            {
                return 4;
            }
            else if (day.Contains("선공정"))
            {
                return 5;
            }
            else if (day.Contains("포장"))
            {
                return 6;
            }
            return 0;
            //return (int)Enum.Parse(typeof(DayOfWeek), day);
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
        //          WinUIMessageBox.Show(eLog.Message, _title, MessageBoxButton.OK, MessageBoxImage.Error);
        //          //this.M_SEARCH_TEXT.SelectAll();
        //          this.M_SEARCH_TEXT.Focus();
        //          return;
        //      }
        //}


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

        private async void GridColumn_Validate(object sender, DevExpress.Xpf.Grid.GridCellValidationEventArgs e)
        {
            try
            {
                PurVo detailDomain = (PurVo)this.ViewGridDtl.GetFocusedRow();

                //실적 공정
                bool routNm = (e.Column.FieldName.ToString().Equals("ROUT_NM") ? true : false);
                //소요량
                bool inpQty = (e.Column.FieldName.ToString().Equals("INP_QTY") ? true : false);

                if (routNm)
                {
                    if (e.IsValid)
                    {
                        if (string.IsNullOrEmpty(detailDomain.ROUT_CD + ""))
                        {
                            detailDomain.ROUT_NM = "";
                            detailDomain.ROUT_CD = "";
                        }
                        //
                        if (!detailDomain.ROUT_CD.Equals((e.Value == null ? "" : e.Value.ToString())))
                        {
                            ManVo bankIoDao = this.lue_ROUT_NM.GetItemFromValue(e.Value) as ManVo;
                            //
                            if (bankIoDao != null)
                            {
                                detailDomain.ROUT_CD = bankIoDao.ROUT_CD;
                                detailDomain.ROUT_NM = bankIoDao.ROUT_NM;

                                detailDomain.CRE_USR_ID = SystemProperties.USER;
                                detailDomain.UPD_USR_ID = SystemProperties.USER;
                                detailDomain.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;

                                //update
                                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("p414101/dtl/u", new StringContent(JsonConvert.SerializeObject(detailDomain), System.Text.Encoding.UTF8, "application/json")))
                                {
                                    int _Num = 0;
                                    if (response.IsSuccessStatusCode)
                                    {
                                        string result = await response.Content.ReadAsStringAsync();
                                        if (int.TryParse(result, out _Num) == false)
                                        {
                                            //실패
                                            e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                                            e.ErrorContent = result;
                                            e.SetError(e.ErrorContent, e.ErrorType);
                                            return;
                                        }

                                        //성공
                                       // WinUIMessageBox.Show("완료 되었습니다", _title, MessageBoxButton.OK, MessageBoxImage.Information);
                                    }
                                }

                            }
                        }
                    }
                }
                else if (inpQty)
                {
                    if (e.IsValid)
                    {
                        if (string.IsNullOrEmpty(detailDomain.INP_QTY + ""))
                        {
                            detailDomain.INP_QTY = 0;
                        }
                        //
                        if (!detailDomain.INP_QTY.Equals((e.Value == null ? "" : e.Value.ToString())))
                        {
                            detailDomain.INP_QTY = e.Value;

                            detailDomain.CRE_USR_ID = SystemProperties.USER;
                            detailDomain.UPD_USR_ID = SystemProperties.USER;
                            detailDomain.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;

                            //update
                            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("p414101/dtl/u", new StringContent(JsonConvert.SerializeObject(detailDomain), System.Text.Encoding.UTF8, "application/json")))
                            {
                                int _Num = 0;
                                if (response.IsSuccessStatusCode)
                                {
                                    string result = await response.Content.ReadAsStringAsync();
                                    if (int.TryParse(result, out _Num) == false)
                                    {
                                        //실패
                                        e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                                        e.ErrorContent = result;
                                        e.SetError(e.ErrorContent, e.ErrorType);
                                        return;
                                    }

                                    //성공
                                    // WinUIMessageBox.Show("완료 되었습니다", _title, MessageBoxButton.OK, MessageBoxImage.Information);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception eLog)
            {
                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                e.ErrorContent = eLog.Message;
                e.SetError(e.ErrorContent, e.ErrorType);
                return;
            }
        }
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