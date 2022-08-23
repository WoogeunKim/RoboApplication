using DevExpress.Xpf.Printing;
using DevExpress.Xpf.WindowsUI;
using DevExpress.XtraPrinting.Drawing;
using System;
using System.Drawing;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using AquilaErpWpfApp3.ViewModel;

namespace AquilaErpWpfApp3.View.INV
{
    public partial class I6210 : UserControl
    {
        //private I6210ItemDialog itemsDialog;
        
        //private static InvServiceClient invClient = SystemProperties.InvClient;
        private string _title = "자재수불장";
        private bool isCheckColumn = false;
        private string _itmNm = string.Empty;
        private string _itmSzNm = string.Empty;



        public I6210()
        {
            DataContext = new I6210ViewModel();
            //
            InitializeComponent();

        }

        private void M_REFRESH_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            try
            {
                //this.ViewGridMst.ShowLoadingPanel = true;
                //this.ViewTableMst.SearchString = (this.M_SEARCH_TEXT.EditValue == null ? "" : this.M_SEARCH_TEXT.EditValue.ToString());
                //this.txt_Search.SelectAll();
                //this.M_SEARCH_TEXT.Focus();
                //this.ViewGridMst.ShowLoadingPanel = false;
            }
            catch (Exception eLog)
            {
                //this.ViewGridMst.ShowLoadingPanel = false;
                WinUIMessageBox.Show(eLog.Message, "[에러]" + _title, MessageBoxButton.OK, MessageBoxImage.Error);
                //this.M_SEARCH_TEXT.SelectAll();
               // this.M_SEARCH_TEXT.Focus();
                return;
            }
        }

        private void M_SAVE_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            this.ViewTableMst.Focus();
        }

        private void M_PRINT_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            this.ViewTableMst.PrintAutoWidth = true;



            StringBuilder sb = new StringBuilder();
            sb.Append("<DataTemplate xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" xmlns:dxe=\"http://schemas.devexpress.com/winfx/2008/xaml/editors\">");
            sb.Append("<dxe:TextEdit Text=\"" + "[ 일자 : " + Convert.ToDateTime(M_FM_DT.EditValue).ToString("yyyy-MM-dd") + "~" + Convert.ToDateTime(M_TO_DT.EditValue).ToString("yyyy-MM-dd") + ", 품목 명 : " + _itmNm + _itmSzNm + "] " + "\" FontWeight=\"Bold\"  FontSize=\"8\"  />");
            sb.Append("</DataTemplate>");
            DataTemplate templateHeader = (DataTemplate)System.Windows.Markup.XamlReader.Parse(sb.ToString());

            sb = new StringBuilder();
            sb.Append("<DataTemplate xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" xmlns:dxe=\"http://schemas.devexpress.com/winfx/2008/xaml/editors\">");
            sb.Append("<dxe:TextEdit Text=\"" + System.DateTime.Now.ToString("yyyy-MM-dd hh:mm") + "\"/>");
            sb.Append("</DataTemplate>");
            DataTemplate templateFooter = (DataTemplate)System.Windows.Markup.XamlReader.Parse(sb.ToString());

            this.ViewTableMst.PrintAutoWidth = true;
            this.ViewTableMst.BestFitColumns();


            using (PrintableControlLink prtLink = new PrintableControlLink(this.ViewTableMst))
            {
                //prtLink.PageHeaderData = "인쇄 일자 : " + Convert.ToDateTime(this.txt_stDate.Text).ToString("yyyy-MM-dd");
                //prtLink.PageHeaderTemplate = Resources["PageHeader"] as DataTemplate;
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
                prtLink.DocumentName =  _title + " Print";

                //prtLink.PrintingSystem.Watermark.Text = Properties.Resources.ResourceManager.;
                prtLink.PrintingSystem.Watermark.TextDirection = DirectionMode.ForwardDiagonal;
                prtLink.PrintingSystem.Watermark.Font = new Font(prtLink.PrintingSystem.Watermark.Font.FontFamily, 40);
                prtLink.PrintingSystem.Watermark.ForeColor = System.Drawing.Color.PaleTurquoise;
                prtLink.PrintingSystem.Watermark.TextTransparency = 150;

                prtLink.Landscape = true;
                prtLink.PrintingSystem.ShowPrintStatusDialog = true;

                prtLink.PaperKind = System.Drawing.Printing.PaperKind.A4;
                prtLink.CreateDocument(true);
                prtLink.ShowPrintPreviewDialog(Application.Current.MainWindow, _title);
            }
        }

        private void TextEdit_KeyDown(object sender, KeyEventArgs e)
        {
            this.ViewTableMst.Focus();


        }

        private void M_MST_COLUMN_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            //ViewGridMst.View.ClearValue(GridViewBase.ColumnChooserFactoryProperty);
            //this.ViewTableMst.ColumnChooserFactory = new CustomColumnChooser(columnChooser);


            if (isCheckColumn)
            {
                isCheckColumn = false;
                this.ViewTableMst.HideColumnChooser();
            }
            else
            {
                isCheckColumn = true;
                this.ViewTableMst.ShowColumnChooser();
            }

        }

        private void M_SEARCH_TEXT_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                M_REFRESH_ItemClick(sender, null);
            }
        }

        private void Button_itm_Click(object sender, RoutedEventArgs e)
        {

            //itemsDialog = new I6210ItemDialog();
            //itemsDialog.Title = this._title + " - 품목 선택";
            //itemsDialog.Owner = Application.Current.MainWindow;
            //itemsDialog.BorderEffect = BorderEffect.Default;
            //////masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            //////masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            //bool isDialog = (bool)itemsDialog.ShowDialog();
            //if (isDialog)
            //{
            //    this.M_SEARCH_TEXT.EditValue = itemsDialog.ITM_CD.ITM_CD;
            //    this._itmNm = itemsDialog.ITM_CD.ITM_NM;
            //    this._itmSzNm = itemsDialog.ITM_CD.ITM_SZ_NM;

            //    ((I6210ViewModel)this.DataContext).M_SEARCH_TEXT = itemsDialog.ITM_CD.ITM_CD;
            //    ((I6210ViewModel)this.DataContext).M_ITM_CD = itemsDialog.ITM_CD;
            //}
        }




        //private void GridColumn_Validate(object sender, DevExpress.Xpf.Grid.GridCellValidationEventArgs e)
        //{
        //    try
        //    {
        //        InvVo resultVo;
        //        InvVo masterDomain = (InvVo)ViewGridMst.GetFocusedRow();

        //        //bool costEtcNm = (e.Column.FieldName.ToString().Equals("COST_ETC_NM") ? true : false);
        //        bool reqRmk = (e.Column.FieldName.ToString().Equals("REQ_RMK") ? true : false);
        //        bool itmReqQty = (e.Column.FieldName.ToString().Equals("ITM_REQ_QTY") ? true : false);
        //        bool itmSlPlnAmt = (e.Column.FieldName.ToString().Equals("ITM_SL_PLN_AMT") ? true : false);
        //        bool itmSlAmt = (e.Column.FieldName.ToString().Equals("ITM_SL_AMT") ? true : false);
        //        //bool itmYrUseQty = (e.Column.FieldName.ToString().Equals("YR_USE_QTY") ? true : false);

        //        bool coNm = (e.Column.FieldName.ToString().Equals("CO_NM") ? true : false);

        //        bool reqUsrId = (e.Column.FieldName.ToString().Equals("REQ_USR_ID") ? true : false);
        //        bool actFlg = (e.Column.FieldName.ToString().Equals("ACT_FLG") ? true : false);

        //        //
        //        if (reqRmk)
        //        {
        //            if (e.IsValid)
        //            {
        //                if (string.IsNullOrEmpty(masterDomain.REQ_RMK))
        //                {
        //                    masterDomain.REQ_RMK = "";
        //                }
        //                //
        //                if (!masterDomain.REQ_RMK.Equals((e.Value == null ? "" : e.Value.ToString())))
        //                {
        //                    //masterDomain.IS_CHECK = true;
        //                    //this.btn_save.IsEnabled = true;
        //                    masterDomain.REQ_RMK = e.Value.ToString();
        //                    resultVo = invClient.I6629UpdateMst(masterDomain);
        //                    if (!resultVo.isSuccess)
        //                    {
        //                        //실패
        //                        //WinUIMessageBox.Show(resultVo.Message, "[에러]" + _title, MessageBoxButton.OK, MessageBoxImage.Error);
        //                        e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
        //                        e.ErrorContent = resultVo.Message;
        //                        e.SetError(e.ErrorContent, e.ErrorType);
        //                        return;
        //                    }
        //                }
        //            }
        //        }
        //        else if (itmReqQty)
        //        {
        //            if (string.IsNullOrEmpty(masterDomain.ITM_REQ_QTY + ""))
        //            {
        //                masterDomain.ITM_REQ_QTY = 0;
        //            }
        //            //
        //            if (!masterDomain.ITM_REQ_QTY.Equals((e.Value == null ? "" : e.Value.ToString())))
        //            {
        //                //masterDomain.IS_CHECK = true;
        //                //this.btn_save.IsEnabled = true;

        //                try
        //                {
        //                    masterDomain.ITM_REQ_QTY = e.Value.ToString();
        //                    masterDomain.YR_USE_QTY = Convert.ToInt32(masterDomain.ITM_REQ_QTY) * Convert.ToInt32(masterDomain.ITM_SL_AMT);
        //                }
        //                catch
        //                {
        //                    masterDomain.YR_USE_QTY = 0;
        //                }

        //                resultVo = invClient.I6629UpdateMst(masterDomain);
        //                if (!resultVo.isSuccess)
        //                {
        //                    //실패
        //                    //WinUIMessageBox.Show(resultVo.Message, "[에러]" + _title, MessageBoxButton.OK, MessageBoxImage.Error);
        //                    e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
        //                    e.ErrorContent = resultVo.Message;
        //                    e.SetError(e.ErrorContent, e.ErrorType);
        //                    return;
        //                }
        //            }
        //        }
        //        else if (itmSlPlnAmt)
        //        {
        //            if (string.IsNullOrEmpty(masterDomain.ITM_SL_PLN_AMT + ""))
        //            {
        //                masterDomain.ITM_SL_PLN_AMT = 0;
        //            }
        //            //
        //            if (!masterDomain.ITM_SL_PLN_AMT.Equals((e.Value == null ? "" : e.Value.ToString())))
        //            {
        //                //masterDomain.IS_CHECK = true;
        //                //this.btn_save.IsEnabled = true;
        //                masterDomain.ITM_SL_PLN_AMT = e.Value.ToString();
        //                resultVo = invClient.I6629UpdateMst(masterDomain);
        //                if (!resultVo.isSuccess)
        //                {
        //                    //실패
        //                    //WinUIMessageBox.Show(resultVo.Message, "[에러]" + _title, MessageBoxButton.OK, MessageBoxImage.Error);
        //                    e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
        //                    e.ErrorContent = resultVo.Message;
        //                    e.SetError(e.ErrorContent, e.ErrorType);
        //                    return;
        //                }
        //            }
        //        }
        //        else if (itmSlAmt)
        //        {
        //            if (string.IsNullOrEmpty(masterDomain.ITM_SL_AMT + ""))
        //            {
        //                masterDomain.ITM_SL_AMT = 0;
        //            }
        //            //
        //            if (!masterDomain.ITM_SL_AMT.Equals((e.Value == null ? "" : e.Value.ToString())))
        //            {
        //                //masterDomain.IS_CHECK = true;
        //                //this.btn_save.IsEnabled = true;
        //                try
        //                {
        //                    masterDomain.ITM_SL_AMT = e.Value.ToString();
        //                    masterDomain.YR_USE_QTY = Convert.ToInt32(masterDomain.ITM_REQ_QTY) * Convert.ToInt32(masterDomain.ITM_SL_AMT);
        //                }
        //                catch
        //                {
        //                    masterDomain.YR_USE_QTY = 0;
        //                }

        //                resultVo = invClient.I6629UpdateMst(masterDomain);
        //                if (!resultVo.isSuccess)
        //                {
        //                    //실패
        //                    //WinUIMessageBox.Show(resultVo.Message, "[에러]" + _title, MessageBoxButton.OK, MessageBoxImage.Error);
        //                    e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
        //                    e.ErrorContent = resultVo.Message;
        //                    e.SetError(e.ErrorContent, e.ErrorType);
        //                    return;
        //                }
        //            }
        //        }
        //        //else if (itmYrUseQty)
        //        //{
        //        //    if (string.IsNullOrEmpty(masterDomain.YR_USE_QTY + ""))
        //        //    {
        //        //        masterDomain.YR_USE_QTY = 0;
        //        //    }
        //        //    //
        //        //    if (!masterDomain.YR_USE_QTY.Equals((e.Value == null ? "" : e.Value.ToString())))
        //        //    {
        //        //        //masterDomain.IS_CHECK = true;
        //        //        //this.btn_save.IsEnabled = true;
        //        //        masterDomain.YR_USE_QTY = e.Value.ToString();
        //        //        resultVo = invClient.I6629UpdateMst(masterDomain);
        //        //        if (!resultVo.isSuccess)
        //        //        {
        //        //            //실패
        //        //            //WinUIMessageBox.Show(resultVo.Message, "[에러]" + _title, MessageBoxButton.OK, MessageBoxImage.Error);
        //        //            e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
        //        //            e.ErrorContent = resultVo.Message;
        //        //            e.SetError(e.ErrorContent, e.ErrorType);
        //        //            return;
        //        //        }
        //        //    }
        //        //}
        //        else if (coNm)
        //        {
        //            if (e.IsValid)
        //            {
        //                if (string.IsNullOrEmpty(masterDomain.CO_NM))
        //                {
        //                    masterDomain.CO_CD = "";
        //                    masterDomain.CO_NM = "";
        //                }
        //                //
        //                if (!masterDomain.CO_NM.Equals((e.Value == null ? "" : e.Value.ToString())))
        //                {
        //                    CustomerCodeDao bankIoDao = this.lue_CoNm.GetItemFromValue(e.Value) as CustomerCodeDao;
        //                    //
        //                    masterDomain.CO_CD = bankIoDao.CO_NO;
        //                    masterDomain.CO_NM = bankIoDao.CO_NM;
        //                    //
        //                    resultVo = invClient.I6629UpdateMst(masterDomain);
        //                    if (!resultVo.isSuccess)
        //                    {
        //                        //실패
        //                        //WinUIMessageBox.Show(resultVo.Message, "[에러]" + _title, MessageBoxButton.OK, MessageBoxImage.Error);
        //                        e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
        //                        e.ErrorContent = resultVo.Message;
        //                        e.SetError(e.ErrorContent, e.ErrorType);
        //                        return;
        //                    }
        //                    //masterDomain.IS_CHECK = true;
        //                    //this.btn_save.IsEnabled = true;
        //                }
        //            }
        //        }
        //        else if (reqUsrId)
        //        {
        //            if (e.IsValid)
        //            {
        //                if (string.IsNullOrEmpty(masterDomain.REQ_USR_ID))
        //                {
        //                    masterDomain.REQ_USR_NM = "";
        //                    masterDomain.REQ_USR_ID = "";
        //                }
        //                //
        //                if (!masterDomain.REQ_USR_ID.Equals((e.Value == null ? "" : e.Value.ToString())))
        //                {
        //                    UserCodeDao bankIoDao = this.lue_User.GetItemFromValue(e.Value) as UserCodeDao;
        //                    //
        //                    masterDomain.REQ_USR_ID = bankIoDao.USR_ID;
        //                    masterDomain.REQ_USR_NM = bankIoDao.USR_N1ST_NM;
        //                    //
        //                    resultVo = invClient.I6629UpdateMst(masterDomain);
        //                    if (!resultVo.isSuccess)
        //                    {
        //                        //실패
        //                        //WinUIMessageBox.Show(resultVo.Message, "[에러]" + _title, MessageBoxButton.OK, MessageBoxImage.Error);
        //                        e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
        //                        e.ErrorContent = resultVo.Message;
        //                        e.SetError(e.ErrorContent, e.ErrorType);
        //                        return;
        //                    }
        //                    //masterDomain.IS_CHECK = true;
        //                    //this.btn_save.IsEnabled = true;
        //                }
        //            }
        //        }
        //        else if (actFlg)
        //        {
        //            if (e.IsValid)
        //            {
        //                if (string.IsNullOrEmpty(masterDomain.ACT_FLG))
        //                {
        //                    masterDomain.ACT_FLG = "N";
        //                }
        //                //
        //                if (!masterDomain.ACT_FLG.Equals((e.Value == null ? "" : e.Value.ToString())))
        //                {
        //                    //CodeDao bankIoDao = this.lue_User.GetItemFromValue(e.Value) as CodeDao;
        //                    //
        //                    masterDomain.ACT_FLG = e.Value.ToString();
        //                    //
        //                    resultVo = invClient.I6629UpdateMst(masterDomain);
        //                    if (!resultVo.isSuccess)
        //                    {
        //                        //실패
        //                        //WinUIMessageBox.Show(resultVo.Message, "[에러]" + _title, MessageBoxButton.OK, MessageBoxImage.Error);
        //                        e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
        //                        e.ErrorContent = resultVo.Message;
        //                        e.SetError(e.ErrorContent, e.ErrorType);
        //                        return;
        //                    }
        //                    //masterDomain.IS_CHECK = true;
        //                    //this.btn_save.IsEnabled = true;
        //                }
        //            }
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


        //private void viewPage1EditView_HiddenEditor(object sender, DevExpress.Xpf.Grid.EditorEventArgs e)
        //{
        //    this.ViewTableMst.CommitEditing();

         
        //    bool itmReqQty = (e.Column.FieldName.ToString().Equals("ITM_REQ_QTY") ? true : false);
        //    bool itmSlPlnAmt = (e.Column.FieldName.ToString().Equals("ITM_SL_PLN_AMT") ? true : false);
        //    bool itmSlAmt = (e.Column.FieldName.ToString().Equals("ITM_SL_AMT") ? true : false);
        //    //bool itmYrUseQty = (e.Column.FieldName.ToString().Equals("YR_USE_QTY") ? true : false);

        //    bool coNm = (e.Column.FieldName.ToString().Equals("CO_NM") ? true : false);

        //    bool reqUsrId = (e.Column.FieldName.ToString().Equals("REQ_USR_ID") ? true : false);
        //    bool actFlg = (e.Column.FieldName.ToString().Equals("ACT_FLG") ? true : false);

        //    bool reqRmk = (e.Column.FieldName.ToString().Equals("REQ_RMK") ? true : false);


        //    int rowHandle = this.ViewTableMst.FocusedRowHandle;
           

        //    if (itmReqQty)
        //    {
        //        this.ViewGridMst.CurrentColumn = this.ViewGridMst.Columns["ITM_SL_PLN_AMT"];
        //    }
        //    else if (itmSlPlnAmt)
        //    {
        //        this.ViewGridMst.CurrentColumn = this.ViewGridMst.Columns["ITM_SL_AMT"];
        //    }
        //    else if (itmSlAmt)
        //    {
        //        this.ViewGridMst.CurrentColumn = this.ViewGridMst.Columns["CO_NM"];
        //        //this.ViewGridMst.CurrentColumn = this.ViewGridMst.Columns["YR_USE_QTY"];
        //    }
        //    //else if (itmYrUseQty)
        //    //{
        //    //    this.ViewGridMst.CurrentColumn = this.ViewGridMst.Columns["CO_NM"];
        //    //}
        //    else if (coNm)
        //    {
        //        this.ViewGridMst.CurrentColumn = this.ViewGridMst.Columns["REQ_USR_ID"];
        //    }
        //    else if (reqUsrId)
        //    {
        //        this.ViewGridMst.CurrentColumn = this.ViewGridMst.Columns["REQ_RMK"];
        //    }
        //    else if (reqRmk)
        //    {
        //        this.ViewGridMst.CurrentColumn = this.ViewGridMst.Columns["ACT_FLG"];
        //    }
        //    else if (actFlg)
        //    {
        //        rowHandle = this.ViewTableMst.FocusedRowHandle + 1;
        //        this.ViewGridMst.CurrentColumn = this.ViewGridMst.Columns["ITM_REQ_QTY"];
        //        this.ViewGridMst.RefreshRow(rowHandle - 1);
        //    }

        //    this.ViewGridMst.RefreshRow(rowHandle);
        //    this.ViewTableMst.FocusedRowHandle = rowHandle;
        //}

    }
}