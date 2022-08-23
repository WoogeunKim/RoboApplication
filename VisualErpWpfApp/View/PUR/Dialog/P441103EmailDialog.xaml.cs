using AquilaErpWpfApp3.Util;
using AquilaErpWpfApp3.View.PUR.Report;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Code;
using ModelsLibrary.Pur;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Windows;
using System.Windows.Input;


namespace AquilaErpWpfApp3.View.PUR.Dialog
{
    public partial class P441103EmailDialog : DXWindow
    {

        //private string EXCEL_FILE = string.Empty;
        private string PDF_FILE = string.Empty;
        private string TAG_FILE = string.Empty;

        //private IList<PurVo> excelList = new List<PurVo>();
        //private IList<PurVo> reportList = new List<PurVo>();
        private PurVo orgVo;

        private string _title = "부자재 발주서 등록 - E-mail 관리";


        public P441103EmailDialog(PurVo Dao)
        {
            InitializeComponent();


            this.orgVo = Dao;

            //받는 사람
            this.text_To.Text = Dao.CNTC_MAN_EML;
            //제목
            this.text_Title.Text = "";
            //내용
            this.text_Contents.Text = "";
            //첨부 파일
            setAttachment();

            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
            this.OKButton.Click += new RoutedEventHandler(OKButton_Click);
            this.CancelButton.Click += new RoutedEventHandler(CancelButton_Click);


            this.ConfigButton.Click += new RoutedEventHandler(ConfigButton_Click);
        }

        private async void setAttachment()
        {
            try
            {
                // 발주서
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("p441102/dtl", new StringContent(JsonConvert.SerializeObject(this.orgVo), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        IList<PurVo> reportList = new List<PurVo>();

                        reportList = JsonConvert.DeserializeObject<IEnumerable<PurVo>>(await response.Content.ReadAsStringAsync()).Cast<PurVo>().ToList();

                        SystemCodeVo coNm = new SystemCodeVo();
                        using (HttpResponseMessage responseX = await SystemProperties.PROGRAM_HTTP.PostAsync("s143/dtl", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { DELT_FLG = "N", CO_NO = this.orgVo.PUR_CO_CD, AREA_CD = SystemProperties.USER_VO.EMPE_PLC_NM, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                        {
                            if (responseX.IsSuccessStatusCode)
                            {
                                coNm = JsonConvert.DeserializeObject<SystemCodeVo>(await responseX.Content.ReadAsStringAsync());

                                //담당자 E-MAil 등록 유무 판단
                                this.text_To.Text = coNm.CNTC_MAN_EML;
                            }
                            //매입처
                            reportList[0].R_MM_04 = coNm.CO_NM;
                            //전화번호 / 팩스번호
                            reportList[0].R_MM_05 = "( 전화번호 : " + coNm.HDQTR_PHN_NO + "  /   " + "팩스번호 : " + coNm.HDQTR_FAX_NO + " )";
                            //대표자 명
                            reportList[0].R_MM_06 = coNm.PRSD_NM;
                        }

                        using (HttpResponseMessage responseY = await SystemProperties.PROGRAM_HTTP.PostAsync("s143/dtl", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { DELT_FLG = "N", CO_NO = "99999", AREA_CD = SystemProperties.USER_VO.EMPE_PLC_NM, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                        {
                            if (responseY.IsSuccessStatusCode)
                            {
                                coNm = JsonConvert.DeserializeObject<SystemCodeVo>(await responseY.Content.ReadAsStringAsync());
                            }
                            //화성코스메틱 주식회사
                            reportList[0].R_MM_07 = coNm.CO_NM;
                            reportList[0].R_MM_08 = coNm.PRSD_NM;
                            reportList[0].R_MM_09 = "(" + coNm.HDQTR_PST_NO + ")  " + coNm.HDQTR_ADDR;
                            reportList[0].R_MM_10 = coNm.HDQTR_PHN_NO;
                            reportList[0].R_MM_11 = coNm.HDQTR_FAX_NO;
                        }

                        //
                        reportList[reportList.Count - 1].GBN = "합계금액 : " + String.Format("{0:#,#}", reportList.Sum<PurVo>(s => Convert.ToDouble(s.PUR_AMT)));
                        reportList[reportList.Count - 1].R_MM_02 = "발주담당자명 : " + this.orgVo.CRE_USR_NM;
                        reportList[reportList.Count - 1].R_MM_01 = System.DateTime.Now.ToString("yyyy년   MM월   dd일");
                        reportList[reportList.Count - 1].PUR_RMK = this.orgVo.PUR_RMK;


                        P441103Report report = new P441103Report(reportList);
                        report.Margins.Top = 30;
                        report.Margins.Bottom = 10;
                        report.Margins.Left = 120;
                        report.Margins.Right = 10;
                        report.Landscape = true;
                        report.PrintingSystem.ShowPrintStatusDialog = true;
                        report.PaperKind = System.Drawing.Printing.PaperKind.A4;

                        report.Watermark.Text = Properties.Settings.Default.SettingCompany;
                        report.Watermark.TextDirection = DevExpress.XtraPrinting.Drawing.DirectionMode.ForwardDiagonal;
                        report.Watermark.Font = new System.Drawing.Font(report.PrintingSystem.Watermark.Font.FontFamily, 40);
                        report.Watermark.ForeColor = System.Drawing.Color.PaleTurquoise;
                        report.Watermark.TextTransparency = 150;
                        report.CreateDocument();

                        //발주서
                        this.PDF_FILE = @"C:\\temp\\" + this.orgVo.PUR_ORD_NO + "_PUR_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png";
                        report.ExportToImage(this.PDF_FILE);

                        this.text_Po.Text = this.PDF_FILE;

                        this.documentPreviewControl_2.DocumentSource = report;

                        //var window = new DocumentPreviewWindow();
                        //window.PreviewControl.DocumentSource = report;
                        //window.Title = "원재료 발주서 [" + this.orgVo.PUR_ORD_NO + "] ";
                        //window.Owner = Application.Current.MainWindow;
                        //window.ShowDialog();
                    }
                }

                ////현품표
                //using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("p441102/dtl/barcode/report", new StringContent(JsonConvert.SerializeObject(this.orgVo), System.Text.Encoding.UTF8, "application/json")))
                //{
                //    if (response.IsSuccessStatusCode)
                //    {
                //        IList<PurVo> reportList = new List<PurVo>();
                //        reportList = JsonConvert.DeserializeObject<IEnumerable<PurVo>>(await response.Content.ReadAsStringAsync()).Cast<PurVo>().ToList();


                //        P441103TagReport report = new P441103TagReport(reportList);
                //        report.Margins.Top = 0;
                //        report.Margins.Bottom = 0;
                //        report.Margins.Left = 20;
                //        report.Margins.Right = 0;
                //        report.Landscape = true;
                //        report.PrintingSystem.ShowPrintStatusDialog = true;
                //        report.PaperKind = System.Drawing.Printing.PaperKind.A5;

                //        report.Watermark.Text = Properties.Settings.Default.SettingCompany;
                //        report.Watermark.TextDirection = DevExpress.XtraPrinting.Drawing.DirectionMode.ForwardDiagonal;
                //        report.Watermark.Font = new System.Drawing.Font(report.PrintingSystem.Watermark.Font.FontFamily, 40);
                //        report.Watermark.ForeColor = System.Drawing.Color.PaleTurquoise;
                //        report.Watermark.TextTransparency = 150;

                //        report.CreateDocument();

                //        //현품표
                //        this.TAG_FILE = @"C:\\temp\\" + this.orgVo.PUR_ORD_NO + "_TAG_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".rtf";
                //        report.ExportToRtf(this.TAG_FILE);

                //        this.text_Tag.Text = this.TAG_FILE;

                //        this.documentPreviewControl_2.DocumentSource = report;

                //        //var window = new DocumentPreviewWindow();
                //        //window.PreviewControl.DocumentSource = report;
                //        //report.CreateDocument(true);
                //        //window.Title = "현품표(A5) [" + SelectedMstItem.PUR_ORD_NO + "] ";
                //        //window.Owner = Application.Current.MainWindow;
                //        //window.ShowDialog();

                //    }
                //}
            }
            catch (Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[에러]" + _title, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }


        private void ConfigButton_Click(object sender, RoutedEventArgs e)
        {
            P441103EmailConfigDialog emailDialog = new P441103EmailConfigDialog();
            emailDialog.Title = "E-MAIL 설정";
            emailDialog.Owner = Application.Current.MainWindow;
            emailDialog.BorderEffect = BorderEffect.Default;
            //jobItemDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            //jobItemDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            bool isDialog = (bool)emailDialog.ShowDialog();
            //if (isDialog)
            //{

            //}
        }


        #region Functon (OKButton_Click, CancelButton_Click)
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(this.text_To.Text))
                {
                    //받는 사람
                    WinUIMessageBox.Show("[받는 사람] 입력 값이 맞지 않습니다.", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                    this.text_To.IsTabStop = true;
                    this.text_To.Focus();
                    return;
                }
                else if (string.IsNullOrEmpty(this.text_Title.Text))
                {
                    //제목
                    WinUIMessageBox.Show("[제목] 입력 값이 맞지 않습니다.", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                    this.text_Title.IsTabStop = true;
                    this.text_Title.Focus();
                    return;
                }
                else if (string.IsNullOrEmpty(Properties.Settings.Default.str_email_id))
                {
                    //제목
                    WinUIMessageBox.Show("[설정 - 보내는 사람] 입력 값이 맞지 않습니다.", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                    this.text_Title.IsTabStop = true;
                    this.text_Title.Focus();
                    return;
                }
                else
                {
                    //특정 폴더에 Excel 파일 저장 하기
                    DirectoryInfo di = new DirectoryInfo(@"C:\\temp\\");
                    if (di.Exists == false)
                    {
                        di.Create();
                    }

                    //var pdfExportOptions = new PdfExportOptions();
                    //pdfExportOptions.NeverEmbeddedFonts = "Arial;Tahoma;Calibri";
                    //// Specify the quality of exported images.
                    //pdfOptions.ConvertImagesToJpeg = false;
                    //pdfOptions.ImageQuality = PdfJpegImageQuality.Medium;
                    //// Specify the PDF/A-compatibility.
                    //pdfOptions.PdfACompatibility = PdfACompatibility.PdfA3b;

                    //pdfOptions.NeverEmbeddedFonts = "Arial;Tahoma;Calibri";
                    //// Specify the document options.
                    //pdfOptions.DocumentOptions.Application = Application.Current.MainWindow.Title;
                    //pdfOptions.DocumentOptions.Author = Application.Current.MainWindow.Title;
                    //pdfOptions.DocumentOptions.Keywords = Application.Current.MainWindow.Title;
                    //pdfOptions.DocumentOptions.Producer = Environment.UserName.ToString();
                    //pdfOptions.DocumentOptions.Subject = Application.Current.MainWindow.Title;
                    //pdfOptions.DocumentOptions.Title = Application.Current.MainWindow.Title;


                    ////발주서
                    //this.PDF_FILE = @"C:\\temp\\" + this.orgVo.PUR_ORD_NO + "_PUR_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png";
                    //(this.documentPreviewControl_1.DocumentSource as P441102Report).ExportToImage(this.PDF_FILE);
                    ////현품표
                    //this.TAG_FILE = @"C:\\temp\\" + this.orgVo.PUR_ORD_NO + "_TAG_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".rtf";
                    //(this.documentPreviewControl_2.DocumentSource as P441102TagReport).ExportToRtf(this.TAG_FILE);


                    using (SmtpClient smtp = new SmtpClient(Properties.Settings.Default.str_email_stmp, 25))
                    {
                        //주소설정
                        smtp.TargetName = Properties.Settings.Default.str_email_stmp;
                        smtp.Host = Properties.Settings.Default.str_email_stmp;
                        smtp.Port = 25;
                        smtp.EnableSsl = false;
                        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                        smtp.UseDefaultCredentials = true;

                        NetworkCredential basicAuthInfo = new NetworkCredential(Properties.Settings.Default.str_email_id, Properties.Settings.Default.str_email_password);
                        smtp.Credentials = basicAuthInfo;
                        //smtp.Credentials = new NetworkCredential("goyh32", "qwer_1234");
                        //smtp.DeliveryMethod = SmtpDeliveryMethod.Network;


                        using (MailMessage mail = new MailMessage())
                        {
                            //메일내용
                            mail.From = new MailAddress(Properties.Settings.Default.str_email_id);
                            mail.To.Add(this.text_To.Text);
                            //mail.To.Add(Properties.Settings.Default.str_email_id);
                            mail.SubjectEncoding = Encoding.UTF8;
                            mail.Subject = this.text_Title.Text;

                            mail.BodyEncoding = Encoding.UTF8;
                            mail.Body = this.text_Contents.Text;

                            mail.IsBodyHtml = false;

                            //첨부파일
                            //Attachment Attachment = new Attachment(this.EXCEL_FILE);
                            //발주서
                            mail.Attachments.Add(new Attachment(this.PDF_FILE));
                            //현품표
                            mail.Attachments.Add(new Attachment(this.TAG_FILE));

                            smtp.Send(mail);

                            //this.orgVo.CLZ_FLG = "Y";
                            //MakeVo resultVo = SystemProperties.MakeClient.P4422_UpdateMst(this.orgVo);
                            //if (!resultVo.isSuccess)
                            //{
                            //    //실패
                            //    WinUIMessageBox.Show(resultVo.Message, "[에러]국내발주관리(원자재,기타)", MessageBoxButton.OK, MessageBoxImage.Error);
                            //    return;
                            //}

                            //System.IO.File.Delete(this.EXCEL_FILE);
                            //this.DialogResult = true;
                            //this.Close();
                        }
                    }
                    WinUIMessageBox.Show("완료 되었습니다", _title, MessageBoxButton.OK, MessageBoxImage.Information);
                    this.DialogResult = true;
                    this.Close();
                }
            }
            catch (SmtpException eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[에러]" + _title, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void HandleEsc(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.DialogResult = false;
                this.Close();
            }
        }
        #endregion
    }
}
