using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Printing;
using DevExpress.Xpf.WindowsUI;
using DevExpress.XtraPrinting.Drawing;
using ModelsLibrary.Pur;
using System;
using System.Drawing;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using AquilaErpWpfApp3.Util;
using AquilaErpWpfApp3.ViewModel;

namespace AquilaErpWpfApp3.View.PUR
{
    public partial class P4426 : UserControl
    {
        //private CustomerDialog customersDialog;
        //private static PurServiceClient purClient = SystemProperties.PurClient;

        public P4426()
        {

            DataContext = new P4426ViewModel();
            //
            InitializeComponent();
            //
            //this.combo_ProdNm.ItemsSource = SystemProperties.SYSTEM_CODE_VO("M-001");
            //this.combo_CoNm.ItemsSource = SystemProperties.SYSTEM_DEPT_VO();
            
        }

        private void CheckEdit_Checked(object sender, RoutedEventArgs e)
        {
            CheckEdit checkEdit = sender as CheckEdit;
            PurVo tmpImsi;
            PurVo resultVo;
            for (int x = 0; x < this.ViewGridMst.VisibleRowCount; x++)
            {
                int rowHandle = this.ViewGridMst.GetRowHandleByVisibleIndex(x);
                if (rowHandle > -1)
                {
                    tmpImsi = this.ViewGridMst.GetRow(rowHandle) as PurVo;
                    if (checkEdit.IsChecked == true)
                    {
                        tmpImsi.PUR_CLZ_FLG = "Y";
                        tmpImsi.isCheckd = true;
                    }
                    else
                    {
                        tmpImsi.PUR_CLZ_FLG = "N";
                        tmpImsi.isCheckd = false;
                    }
                    //
                    tmpImsi.CRE_USR_ID = SystemProperties.USER;
                    tmpImsi.UPD_USR_ID = SystemProperties.USER;
                    //
                    //
                    //resultVo = purClient.P4412UpdateMst(tmpImsi);
                    //if (!resultVo.isSuccess)
                    //{
                    //    //실패
                    //    WinUIMessageBox.Show(resultVo.Message, "[에러]매입원장[일자]", MessageBoxButton.OK, MessageBoxImage.Error);
                    //    return;
                    //}
                }
            }
            //((P4412ViewModel)this.DataContext).Refresh();
        }

        private void M_REFRESH_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
              try
              {
                  this.ViewGridMst.ShowLoadingPanel = true;
                  this.ViewTableMst.SearchString = (this.M_SEARCH_TEXT.EditValue == null ? "" : this.M_SEARCH_TEXT.EditValue.ToString());
                  //this.txt_Search.SelectAll();
                  this.M_SEARCH_TEXT.Focus();
                  this.ViewGridMst.ShowLoadingPanel = false;
              }
              catch (Exception eLog)
              {
                  this.ViewGridMst.ShowLoadingPanel = false;
                  WinUIMessageBox.Show(eLog.Message, "[에러]매입원장[일자]", MessageBoxButton.OK, MessageBoxImage.Error);
                  //this.M_SEARCH_TEXT.SelectAll();
                  this.M_SEARCH_TEXT.Focus();
                  return;
              }
        }

        private void M_SAVE_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            this.ViewTableMst.Focus();
        }


        private void isCheckEdit_DtlClzFlg(object sender, RoutedEventArgs e)
        {
            CheckEdit checkEdit = sender as CheckEdit;
            PurVo masterDomain = (PurVo)ViewGridMst.GetFocusedRow();
            PurVo resultVo;

            //
            if (checkEdit.IsChecked == true)
            {
                //
                //MessageBoxResult result = WinUIMessageBox.Show("[" + masterDomain.PUR_ORD_NO + "]" + " 정말로 마감 하시겠습니까?", "[삭제]발주 등록 관리", MessageBoxButton.YesNo, MessageBoxImage.Question);
                //if (result == MessageBoxResult.Yes)
                {
                    //Y
                    masterDomain.PUR_CLZ_FLG = "Y";
                    masterDomain.isCheckd = true;
                    //
                    masterDomain.CRE_USR_ID = SystemProperties.USER;
                    masterDomain.UPD_USR_ID = SystemProperties.USER;
                    //
                    //
                    //resultVo = purClient.P4412UpdateMst(masterDomain);
                    //if (!resultVo.isSuccess)
                    //{
                    //    //실패
                    //    WinUIMessageBox.Show(resultVo.Message, "[에러]매입원장[일자]", MessageBoxButton.OK, MessageBoxImage.Error);
                    //    return;
                    //}
                }

            }
            else
            {
                //N
                masterDomain.PUR_CLZ_FLG = "N";
                masterDomain.isCheckd = false;

                //
                masterDomain.CRE_USR_ID = SystemProperties.USER;
                masterDomain.UPD_USR_ID = SystemProperties.USER;
                //
                //
                //resultVo = purClient.P4412UpdateMst(masterDomain);
                //if (!resultVo.isSuccess)
                //{
                //    //실패
                //    WinUIMessageBox.Show(resultVo.Message, "[에러]매입원장[일자]", MessageBoxButton.OK, MessageBoxImage.Error);
                //    return;
                //}
            }
           //((P4412ViewModel)this.DataContext).Refresh();
        }


        private void GridColumn_Validate(object sender, DevExpress.Xpf.Grid.GridCellValidationEventArgs e)
        {
            //try
            //{
            //    ManVo resultVo;

            //    ManVo masterDomain = (ManVo)ViewGridMst.GetFocusedRow();
            //    bool prodNm = (e.Column.FieldName.ToString().Equals("PROD_NM") ? true : false);
            //    bool prodStDt = (e.Column.FieldName.ToString().Equals("PROD_ST_DT") ? true : false);
            //    bool prodEndDt = (e.Column.FieldName.ToString().Equals("PROD_END_DT") ? true : false);
            //    bool coNm = (e.Column.FieldName.ToString().Equals("CO_NM") ? true : false);

            //    //
            //    if (prodNm)
            //    {
            //        if (e.IsValid)
            //        {

            //            if (string.IsNullOrEmpty(masterDomain.PROD_NM))
            //            {
            //                masterDomain.PROD_CD = "";
            //                masterDomain.PROD_NM = "";
            //            }
            //            //
            //            if (!masterDomain.PROD_NM.Equals((e.Value == null ? "" : e.Value.ToString())))
            //            {
            //                CodeDao prodDao = this.combo_ProdNm.GetItemFromValue(e.Value) as CodeDao;

            //                masterDomain.PROD_CD = prodDao.CLSS_CD;
            //                masterDomain.PROD_NM = prodDao.CLSS_DESC;
            //                masterDomain.CRE_USR_ID = SystemProperties.USER;
            //                masterDomain.UPD_USR_ID = SystemProperties.USER;

            //                resultVo = manClient.M3311UpdateMst(masterDomain);
            //                if (!resultVo.isSuccess)
            //                {
            //                    //실패
            //                    WinUIMessageBox.Show(resultVo.Message, "[에러]오더(수주)생산계획관리", MessageBoxButton.OK, MessageBoxImage.Error);
            //                    return;
            //                }

            //                //masterDomain.isCheckd = true;
            //                //this.M_MST_SAVE.IsEnabled = true;
            //            }
            //        }
            //    }
            //    //
            //    //
            //    if (prodStDt)
            //    {
            //        if (e.IsValid)
            //        {
            //            if (string.IsNullOrEmpty(masterDomain.PROD_ST_DT))
            //            {
            //                masterDomain.PROD_ST_DT = "";
            //            }
            //            //
            //            if (!masterDomain.PROD_ST_DT.Equals((e.Value == null ? "" : e.Value.ToString())))
            //            {
            //                masterDomain.PROD_ST_DT = Convert.ToDateTime(e.Value.ToString()).ToString("yyyy-MM-dd");
            //                masterDomain.CRE_USR_ID = SystemProperties.USER;
            //                masterDomain.UPD_USR_ID = SystemProperties.USER;

            //                resultVo = manClient.M3311UpdateMst(masterDomain);
            //                if (!resultVo.isSuccess)
            //                {
            //                    //실패
            //                    WinUIMessageBox.Show(resultVo.Message, "[에러]오더(수주)생산계획관리", MessageBoxButton.OK, MessageBoxImage.Error);
            //                    return;
            //                }
            //                //masterDomain.isCheckd = true;
            //                //this.M_MST_SAVE.IsEnabled = true;
            //            }
            //        }
            //    }
            //    //
            //    //
            //    if (prodEndDt)
            //    {
            //        if (e.IsValid)
            //        {
            //            if (string.IsNullOrEmpty(masterDomain.PROD_END_DT))
            //            {
            //                masterDomain.PROD_END_DT = "";
            //            }
            //            //
            //            if (!masterDomain.PROD_END_DT.Equals((e.Value == null ? "" : e.Value.ToString())))
            //            {
            //                masterDomain.PROD_END_DT = Convert.ToDateTime(e.Value.ToString()).ToString("yyyy-MM-dd");
            //                masterDomain.CRE_USR_ID = SystemProperties.USER;
            //                masterDomain.UPD_USR_ID = SystemProperties.USER;

            //                resultVo = manClient.M3311UpdateMst(masterDomain);
            //                if (!resultVo.isSuccess)
            //                {
            //                    //실패
            //                    WinUIMessageBox.Show(resultVo.Message, "[에러]오더(수주)생산계획관리", MessageBoxButton.OK, MessageBoxImage.Error);
            //                    return;
            //                }
            //                //masterDomain.isCheckd = true;
            //                //this.M_MST_SAVE.IsEnabled = true;
            //            }
            //        }
            //    }
            //    //
            //    //
            //    if (coNm)
            //    {
            //        if (e.IsValid)
            //        {

            //            if (string.IsNullOrEmpty(masterDomain.CO_NO))
            //            {
            //                masterDomain.CO_NO = "";
            //                masterDomain.CO_NM = "";
            //            }
            //            //
            //            if (!masterDomain.CO_NM.Equals((e.Value == null ? "" : e.Value.ToString())))
            //            {
            //                CodeDao prodDao = this.combo_CoNm.GetItemFromValue(e.Value) as CodeDao;

            //                masterDomain.CO_NO = prodDao.CLSS_CD;
            //                masterDomain.CO_NM = prodDao.CLSS_DESC;
            //                masterDomain.CRE_USR_ID = SystemProperties.USER;
            //                masterDomain.UPD_USR_ID = SystemProperties.USER;

            //                resultVo = manClient.M3311UpdateMst(masterDomain);
            //                if (!resultVo.isSuccess)
            //                {
            //                    //실패
            //                    WinUIMessageBox.Show(resultVo.Message, "[에러]오더(수주)생산계획관리", MessageBoxButton.OK, MessageBoxImage.Error);
            //                    return;
            //                }

            //                //masterDomain.isCheckd = true;
            //                //this.M_MST_SAVE.IsEnabled = true;
            //            }
            //        }
            //    }
            //}
            //catch (Exception eLog)
            //{
            //    e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
            //    e.ErrorContent = eLog.Message;
            //    e.SetError(e.ErrorContent, e.ErrorType);
            //    return;
            //}
        }


        private void viewPage1EditView_HiddenEditor(object sender, DevExpress.Xpf.Grid.EditorEventArgs e)
        {
            //this.ViewTableMst.CommitEditing();

            //bool prodNm = (e.Column.FieldName.ToString().Equals("PROD_NM") ? true : false);
            //bool prodStDt = (e.Column.FieldName.ToString().Equals("PROD_ST_DT") ? true : false);
            //bool prodEndDt = (e.Column.FieldName.ToString().Equals("PROD_END_DT") ? true : false);
            //bool coNm = (e.Column.FieldName.ToString().Equals("CO_NM") ? true : false);

            //int rowHandle = this.ViewTableMst.FocusedRowHandle + 1;

            //if (prodNm)
            //{
            //    this.ViewGridMst.CurrentColumn = this.ViewGridMst.Columns["PROD_NM"];
            //}
            //else if (prodStDt)
            //{
            //    this.ViewGridMst.CurrentColumn = this.ViewGridMst.Columns["PROD_ST_DT"];
            //}
            //else if (prodEndDt)
            //{
            //    this.ViewGridMst.CurrentColumn = this.ViewGridMst.Columns["PROD_END_DT"];
            //}
            //else if (coNm)
            //{
            //    this.ViewGridMst.CurrentColumn = this.ViewGridMst.Columns["CO_NM"];
            //}

            //this.ViewGridMst.RefreshRow(rowHandle - 1);
            //this.ViewTableMst.FocusedRowHandle = rowHandle;
        }
        private void M_SEARCH_TEXT_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                M_REFRESH_ItemClick(sender, null);
            }
        }

        private void M_MST_PRINT_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            this.ViewTableMst.PrintAutoWidth = true;
            //this.tableView.AutoWidth = true;
            this.ViewTableMst.BestFitColumns();
            //IList<GridColumn> columns = this.tableView.VisibleColumns;
            //columns[0].Visible = false;
            //columns[0].AllowEditing = DevExpress.Utils.DefaultBoolean.True;


            //P4426ViewModel viewModel = (P4426ViewModel)this.DataContext;

            StringBuilder sb = new StringBuilder();
            sb.Append("<DataTemplate xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" xmlns:dxe=\"http://schemas.devexpress.com/winfx/2008/xaml/editors\">");
            sb.Append("<dxe:TextEdit Text=\"" + "[일자(From) : " + Convert.ToDateTime(M_FM_DT.EditValue).ToString("yyyy-MM-dd hh:mm") + "~" + "(To) " + Convert.ToDateTime(M_TO_DT.EditValue).ToString("yyyy-MM-dd hh:mm") /* + " / " + (string.IsNullOrEmpty(M_ITM_GRP_CLSS_CD.EditValue.ToString()) ? "전체" : viewModel._DeptMap[M_ITM_GRP_CLSS_CD.EditValue.ToString()])*/ + "] " + "\" FontWeight=\"Bold\"  FontSize=\"8\"  />");
            sb.Append("</DataTemplate>");
            DataTemplate templateHeader = (DataTemplate)System.Windows.Markup.XamlReader.Parse(sb.ToString());

            sb = new StringBuilder();
            sb.Append("<DataTemplate xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" xmlns:dxe=\"http://schemas.devexpress.com/winfx/2008/xaml/editors\">");
            sb.Append("<dxe:TextEdit Text=\"" + System.DateTime.Now.ToString("yyyy-MM-dd hh:mm") + "\"/>");
            sb.Append("</DataTemplate>");
            DataTemplate templateFooter = (DataTemplate)System.Windows.Markup.XamlReader.Parse(sb.ToString());


            using (PrintableControlLink prtLink = new PrintableControlLink(this.ViewTableMst))
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

                prtLink.DocumentName = "매입원장[일자] Print";
                //prtLink.PrintingSystem.Watermark.Text = "한양화스너공업(주)";
                prtLink.PrintingSystem.Watermark.TextDirection = DirectionMode.ForwardDiagonal;
                prtLink.PrintingSystem.Watermark.Font = new Font(prtLink.PrintingSystem.Watermark.Font.FontFamily, 40);
                prtLink.PrintingSystem.Watermark.ForeColor = System.Drawing.Color.PaleTurquoise;
                prtLink.PrintingSystem.Watermark.TextTransparency = 150;

                prtLink.Landscape = true;
                prtLink.PrintingSystem.ShowPrintStatusDialog = true;

                prtLink.PaperKind = System.Drawing.Printing.PaperKind.A4;
                prtLink.CreateDocument(true);
                prtLink.ShowPrintPreviewDialog(Application.Current.MainWindow, "매입원장[일자]");
            }

            //columns[0].Visible = true;

        }

    }
}