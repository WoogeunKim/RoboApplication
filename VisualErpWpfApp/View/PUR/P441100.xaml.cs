using AquilaErpWpfApp3.Util;
using AquilaErpWpfApp3.ViewModel;
using DevExpress.Xpf.Printing;
using DevExpress.XtraPrinting.Drawing;
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
    /// <summary>
    /// P4415.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class P441100 : UserControl
    {
        //private static PurServiceClient purClient = SystemProperties.PurClient;
        //private IList<PurVo> selectedMstList = new List<PurVo>();
        public P441100()
        {
            DataContext = new P441100ViewModel();
            InitializeComponent();
        }


        private async void GridColumn_Validate(object sender, DevExpress.Xpf.Grid.GridCellValidationEventArgs e)
        {
            try
            {
                PurVo masterDomain = (PurVo)ViewGridDtl.GetFocusedRow();
                bool purItmRmk = (e.Column.FieldName.ToString().Equals("PUR_ITM_RMK") ? true : false);


                bool lssVal = (e.Column.FieldName.ToString().Equals("LSS_VAL") ? true : false);
                bool reqOrdQty = (e.Column.FieldName.ToString().Equals("REQ_ORD_QTY") ? true : false);

                if (purItmRmk)
                {
                    masterDomain = (PurVo)ViewGridMst.GetFocusedRow();
                    if (e.IsValid)
                    {
                        if (string.IsNullOrEmpty(masterDomain.PUR_ITM_RMK + ""))
                        {
                            masterDomain.PUR_ITM_RMK = "";
                        }
                        //
                        if (!masterDomain.PUR_ITM_RMK.Equals((e.Value == null ? "" : e.Value.ToString())))
                        {
                            //double? tmpInt = System.Convert.ToDouble(e.Value.ToString());
                            //if (tmpInt <= -1)
                            //{
                            //    e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                            //    e.ErrorContent = "[단가]" + tmpInt + " -값은 입력 하실수 없습니다";
                            //    e.SetError(e.ErrorContent, e.ErrorType);
                            //    return;
                            //}
                            //masterDomain.CO_UT_PRC = tmpInt;
                            //masterDomain.isCheckd = true;
                            //에러 체크
                            try
                            {
                                masterDomain.CRE_USR_ID = SystemProperties.USER;
                                masterDomain.UPD_USR_ID = SystemProperties.USER;

                                masterDomain.PUR_ITM_RMK = e.Value.ToString();

                                //
                                int _Num = 0;
                                string resultMsg = "";
                                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("p441100/mst/memo", new StringContent(JsonConvert.SerializeObject(masterDomain), System.Text.Encoding.UTF8, "application/json")))
                                {
                                    if (response.IsSuccessStatusCode)
                                    {
                                        resultMsg = await response.Content.ReadAsStringAsync();
                                        if (int.TryParse(resultMsg, out _Num) == false)
                                        {
                                            //실패
                                            e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                                            e.ErrorContent = resultMsg;
                                            e.SetError(e.ErrorContent, e.ErrorType);
                                            return;
                                        }

                                        this.ViewGridDtl.RefreshData();
                                    }
                                }
                            }
                            catch (System.Exception eLog)
                            {
                                //masterDomain.LSS_VAL = 0;
                                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                                e.ErrorContent = eLog.Message;
                                e.SetError(e.ErrorContent, e.ErrorType);
                                return;
                            }
                        }
                    }
                }
               else if (lssVal)
                {
                    if (e.IsValid)
                    {
                        if (string.IsNullOrEmpty(masterDomain.LSS_VAL + ""))
                        {
                            masterDomain.LSS_VAL = 0;
                        }
                        //
                        if (!masterDomain.LSS_VAL.Equals((e.Value == null ? "" : e.Value.ToString())))
                        {
                            //double? tmpInt = System.Convert.ToDouble(e.Value.ToString());
                            //if (tmpInt <= -1)
                            //{
                            //    e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                            //    e.ErrorContent = "[단가]" + tmpInt + " -값은 입력 하실수 없습니다";
                            //    e.SetError(e.ErrorContent, e.ErrorType);
                            //    return;
                            //}
                            //masterDomain.CO_UT_PRC = tmpInt;
                            //masterDomain.isCheckd = true;
                            //에러 체크
                            try
                            {
                                PurVo _domain = (PurVo)ViewGridMst.GetFocusedRow();
                                _domain.LSS_VAL = e.Value;

                                masterDomain.LSS_VAL = e.Value;
                                masterDomain.TOT_USE_QTY = Convert.ToDouble(masterDomain.LSS_VAL) + Convert.ToDouble(masterDomain.ITM_USE_QTY);
                                masterDomain.UPD_USR_ID = SystemProperties.USER;
                                masterDomain.UN_FOL_NO = _domain.UN_FOL_NO;
                                //masterDomain.SL_ITM_QTY = _domain.SL_ITM_QTY;

                                PurVo _domainBulk = (PurVo)ViewGridBulk.GetFocusedRow();
                                _domain.GBN = _domainBulk.ITM_CD;

                                int _Num = 0;
                                string resultMsg = "";
                                if (masterDomain.ITM_GRP_CLSS_NM.Equals("벌크(반제품)"))
                                {
                                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("p441100/dtl/loss/u", new StringContent(JsonConvert.SerializeObject(_domain), System.Text.Encoding.UTF8, "application/json")))
                                    {
                                        if (response.IsSuccessStatusCode)
                                        {
                                            resultMsg = await response.Content.ReadAsStringAsync();
                                            if (int.TryParse(resultMsg, out _Num) == false)
                                            {
                                                //실패
                                                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                                                e.ErrorContent = resultMsg;
                                                e.SetError(e.ErrorContent, e.ErrorType);
                                                return;
                                            }

                                            (this.DataContext as P441100ViewModel).onSelDtl();
                                            this.ViewGridDtl.RefreshData();
                                        }
                                    }
                                }
                                else
                                {
                                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("p441100/dtl/u", new StringContent(JsonConvert.SerializeObject(masterDomain), System.Text.Encoding.UTF8, "application/json")))
                                    {
                                        if (response.IsSuccessStatusCode)
                                        {
                                            resultMsg = await response.Content.ReadAsStringAsync();
                                            if (int.TryParse(resultMsg, out _Num) == false)
                                            {
                                                //실패
                                                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                                                e.ErrorContent = resultMsg;
                                                e.SetError(e.ErrorContent, e.ErrorType);
                                                return;
                                            }

                                            this.ViewGridDtl.RefreshData();
                                        }
                                    }
                                }
                            }
                            catch (System.Exception eLog)
                            {
                                //masterDomain.LSS_VAL = 0;
                                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                                e.ErrorContent = eLog.Message;
                                e.SetError(e.ErrorContent, e.ErrorType);
                                return;
                            }
                        }
                    }
                }
                else if (reqOrdQty)
                {
                    if (e.IsValid)
                    {
                        if (string.IsNullOrEmpty(masterDomain.REQ_ORD_QTY + ""))
                        {
                            masterDomain.REQ_ORD_QTY = 0;
                        }
                        //
                        if (!masterDomain.REQ_ORD_QTY.Equals((e.Value == null ? "" : e.Value.ToString())))
                        {
                            //double? tmpInt = System.Convert.ToDouble(e.Value.ToString());
                            //if (tmpInt <= -1)
                            //{
                            //    e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                            //    e.ErrorContent = "[단가]" + tmpInt + " -값은 입력 하실수 없습니다";
                            //    e.SetError(e.ErrorContent, e.ErrorType);
                            //    return;
                            //}
                            //masterDomain.CO_UT_PRC = tmpInt;
                            //masterDomain.isCheckd = true;
                            //에러 체크
                            try
                            {
                                masterDomain.REQ_ORD_QTY = e.Value;
                                //masterDomain.IMP_ITM_QTY = Convert.ToDouble(masterDomain.SL_ITM_QTY) + Convert.ToDouble(masterDomain.REQ_ORD_QTY);

                                //
                                int _Num = 0;
                                string resultMsg = "";
                                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("p441100/dtl/u", new StringContent(JsonConvert.SerializeObject(masterDomain), System.Text.Encoding.UTF8, "application/json")))
                                {
                                    if (response.IsSuccessStatusCode)
                                    {
                                        resultMsg = await response.Content.ReadAsStringAsync();
                                        if (int.TryParse(resultMsg, out _Num) == false)
                                        {
                                            //실패
                                            e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                                            e.ErrorContent = resultMsg;
                                            e.SetError(e.ErrorContent, e.ErrorType);
                                            return;
                                        }

                                        this.ViewGridDtl.RefreshData();
                                    }
                                }
                            }
                            catch (System.Exception eLog)
                            {
                                //masterDomain.LSS_VAL = 0;
                                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                                e.ErrorContent = eLog.Message;
                                e.SetError(e.ErrorContent, e.ErrorType);
                                return;
                            }
                        }
                    }
                }
            }
            catch (System.Exception eLog)
            {
                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                e.ErrorContent = eLog.Message;
                e.SetError(e.ErrorContent, e.ErrorType);
                return;
            }
        }

        private void viewPage1EditView_HiddenEditor(object sender, DevExpress.Xpf.Grid.EditorEventArgs e)
        {
            this.ViewTableDtl.CommitEditing();

            bool lssVal = (e.Column.FieldName.ToString().Equals("LSS_VAL") ? true : false);
            int rowHandle = this.ViewTableDtl.FocusedRowHandle + 1;
            this.ViewGridDtl.RefreshRow(rowHandle - 1);
            //this.ViewTableDtl.FocusedRowHandle = rowHandle;
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


            using (PrintableControlLink prtLink = new PrintableControlLink(this.ViewTableMst))
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

                prtLink.DocumentName = "원자재 소요량 전개 Print";
                //prtLink.PrintingSystem.Watermark.Text = "한양화스너공업(주)";
                prtLink.PrintingSystem.Watermark.TextDirection = DirectionMode.ForwardDiagonal;
                prtLink.PrintingSystem.Watermark.Font = new Font(prtLink.PrintingSystem.Watermark.Font.FontFamily, 40);
                prtLink.PrintingSystem.Watermark.ForeColor = System.Drawing.Color.PaleTurquoise;
                prtLink.PrintingSystem.Watermark.TextTransparency = 150;

                prtLink.Landscape = true;
                prtLink.PrintingSystem.ShowPrintStatusDialog = true;

                prtLink.PaperKind = System.Drawing.Printing.PaperKind.A4;
                prtLink.CreateDocument(true);
                prtLink.ShowPrintPreviewDialog(Application.Current.MainWindow, "원자재 소요량 전개");
            }
        }

    }
}
