using AquilaErpWpfApp3.Util;
using DevExpress.BarCodes;
using DevExpress.Spreadsheet;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Printing;
using DevExpress.Xpf.WindowsUI;
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
using System.Windows;

namespace AquilaErpWpfApp3.View.M.Dialog
{
    /// <summary>
    /// Interaction logic for ConfigView1Dialog.xaml
    /// </summary>
    public partial class M66530PrintDialog : DXWindow
    {
        //private static CodeServiceClient codeClient = SystemProperties.CodeClient;
        private string _title = "칭량 작업";

        //private SystemCodeVo orgDao;

        //private bool isEdit = false;
        //private byte[] fileByte = null;
        //private SystemCodeVo updateDao;

        private System.IO.Stream streamFile;

        public M66530PrintDialog(IList<ManVo> _items)
        {
            InitializeComponent();
            //

            //칭량지시 및 기록서
            EXCEL_REPORT(_items);

            //this.configCode.DataContext = copyDao;
            //this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
            //this.OKButton.Click += new RoutedEventHandler(OKButton_Click);
            //this.CancelButton.Click += new RoutedEventHandler(CancelButton_Click);
        }



        #region Functon (OKButton_Click, CancelButton_Click)
        //private async void OKButton_Click(object sender, RoutedEventArgs e)
        //{
        //    if (ValueCheckd())
        //    {
        //        //int _Num = 0;
        //        //ProgramVo resultVo;
        //        //if (isEdit == false)
        //        //{
        //        //    this.updateDao = getDomain();//this.updateDao
        //        //    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s1412/mst/i", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
        //        //    {
        //        //        if (response.IsSuccessStatusCode)
        //        //        {
        //        //            string result = await response.Content.ReadAsStringAsync();
        //        //            if (int.TryParse(result, out _Num) == false)
        //        //            {
        //        //                //실패
        //        //                WinUIMessageBox.Show(result, _title, MessageBoxButton.OK, MessageBoxImage.Error);
        //        //                return;
        //        //            }

        //        //            //성공
        //        //            WinUIMessageBox.Show("완료 되었습니다", _title, MessageBoxButton.OK, MessageBoxImage.Information);
        //        //        }
        //        //    }
        //        //}
        //        //else
        //        //{
        //        //    this.updateDao = getDomain();

        //        //    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s1412/mst/u", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
        //        //    {
        //        //        if (response.IsSuccessStatusCode)
        //        //        {
        //        //            string result = await response.Content.ReadAsStringAsync();
        //        //            if (int.TryParse(result, out _Num) == false)
        //        //            {
        //        //                //실패
        //        //                WinUIMessageBox.Show(result, _title, MessageBoxButton.OK, MessageBoxImage.Error);
        //        //                return;
        //        //            }

        //        //            //성공
        //        //            WinUIMessageBox.Show("완료 되었습니다", _title, MessageBoxButton.OK, MessageBoxImage.Information);
        //        //        }
        //        //    }
        //        //}
        //        this.DialogResult = true;
        //        this.Close();
        //    }
        //}

        //private void CancelButton_Click(object sender, RoutedEventArgs e)
        //{
        //    this.DialogResult = false;
        //    this.Close();
        //}

        //private void HandleEsc(object sender, KeyEventArgs e)
        //{
        //    if (e.Key == Key.Escape)
        //    {
        //        this.DialogResult = false;
        //        Close();
        //    }
        //}
        #endregion

        #region Functon (ValueCheckd)
        public Boolean ValueCheckd()
        {
            return true;
        }
        #endregion

        #region Functon (getDomain - ConfigView1Dao)
        private SystemCodeVo getDomain()
        {
            SystemCodeVo Dao = new SystemCodeVo();
            //Dao.RPT_CD = this.text_RPT_CD.Text;
            //Dao.RPT_NM = this.text_RPT_NM.Text;

            //Dao.RPT_FLG = "X";
            //Dao.RPT_FILE = this.spreadsheetControl1.Document.SaveDocument(DevExpress.Spreadsheet.DocumentFormat.Xlsx);
            //Dao.TIT_DESC = this.text_TIT_DESC.Text;
            //Dao.SUBJ_DESC = this.richEdit.RtfText;
            //Dao.RPT_FILE = this.spreadsheetControl1.sa
            //using (System.IO.Stream streamFile = new FileStream("Documents\\SavedDocument.xlsx", FileMode.Create, FileAccess.ReadWrite))
            //{

            //}


            //this.spreadsheetControl1.SaveDocument();


            //streamFile = new System.IO.MemoryStream(Dao.RPT_FILE);
            //this.spreadsheetControl1.SaveDocument(streamFile, DocumentFormat.);

            //GroupUserVo inpIdVo = this.combo_INP_ID.SelectedItem as GroupUserVo;
            //if (inpIdVo != null)
            //{
            //    Dao.INP_ID = inpIdVo.USR_ID;
            //    Dao.INP_NM = inpIdVo.USR_N1ST_NM;
            //}

            //Dao.FILE_NM = this.text_FILE_NM.Text;
            //if (fileByte == null)
            //{
            //    Dao.NTC_FILE = new byte[0];
            //}
            //else
            //{
            //    Dao.NTC_FILE = this.fileByte;
            //}


            Dao.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
            Dao.CRE_USR_ID = SystemProperties.USER;
            Dao.UPD_USR_ID = SystemProperties.USER;
            return Dao;
        }
        #endregion



        public async void EXCEL_REPORT(IList<ManVo> _items)
        {
            try
            {
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s1412/mst", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD, RPT_CD = "PCK100" }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        SystemCodeVo _reportVo  = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList()[0];

                        //
                        this.streamFile = new System.IO.MemoryStream(_reportVo.RPT_FILE);
                        this.spreadsheetControl1.LoadDocument(streamFile, DevExpress.Spreadsheet.DocumentFormat.Xlsx);


                        //Excel 세팅
                        IWorkbook workbook = this.spreadsheetControl1.Document;
                        Worksheet worksheet = workbook.Worksheets[0];

                        //수주번호 - 수주순번
                        worksheet.Cells["F5"].Value = _items[0].SL_ORD_NO + '-' + _items[0].SL_ORD_SEQ;

                        //지시번호
                        worksheet.Cells["F6"].Value = _items[0].LOT_DIV_NO;
                        worksheet.Cells["F6"].Font.Size = 11;
                        //제품코드
                        worksheet.Cells["F7"].Value = _items[0].ITM_CD;
                        //제품명
                        worksheet.Cells["F8"].Value = _items[0].ITM_NM;
                        //지시일
                        worksheet.Cells["Q6"].Value = _items[0].UPD_DT?.ToString();

                        //포장계획량(개)
                        worksheet.Cells["F10"].Value = Convert.ToDouble(_items[0].SL_PLN_QTY);
                        worksheet.Cells["F10"].NumberFormat = "###,###,##0";

                        BarCode barCode = new BarCode();
                        barCode.Symbology = Symbology.QRCode;
                        barCode.CodeText = _items[0].LOT_DIV_NO;
                        barCode.BackColor = Color.White;
                        barCode.ForeColor = Color.Black;
                        barCode.RotationAngle = 0;
                        barCode.CodeBinaryData = Encoding.Default.GetBytes(barCode.CodeText);
                        barCode.Options.QRCode.CompactionMode = QRCodeCompactionMode.Byte;
                        barCode.Options.QRCode.ErrorLevel = QRCodeErrorLevel.Q;
                        barCode.Options.QRCode.ShowCodeText = false;
                        barCode.DpiX = 72;
                        barCode.DpiY = 72;
                        barCode.Module = 0.6f;

                        //바코드
                        worksheet.Pictures.AddPicture(barCode.BarCodeImage, worksheet.Cells["AO23"]);

                        //AO12

                        //worksheet.Cells["F4"].Value = "GKJ";
                        //worksheet.Cells["F5"].Value = _items[0].ASSY_ITM_NM;
                        ////WRK_DT
                        //worksheet.Cells["Q1"].Value = _items[0].WRK_DT;
                        //worksheet.Cells["Q2"].Value = "";
                        ////MIX_WEIH_VAL
                        //worksheet.Cells["Q3"].Value = _items[0].MIX_WEIH_VAL * 1000;
                        //worksheet.Cells["Q3"].NumberFormat = "###,###,##0";

                        //worksheet.Cells["Q4"].Value = "";


                        int nCell = 12;
                        foreach (ManVo item in _items)
                        {
                            //worksheet.Cells["B" + nCell].Value = item.ORD_CLS_CD;
                            worksheet.Cells["B" + nCell].Value = item.CMPO_CD;
                            worksheet.Cells["G" + nCell].Value = item.CMPO_NM;
                            worksheet.Cells["W" + nCell].Value = item.UOM_NM;
                            worksheet.Cells["X" + nCell].Value = Convert.ToDouble(item.SL_ITM_QTY);
                            worksheet.Cells["X" + nCell].NumberFormat = "###,###,##0";
                            //
                            nCell++;
                        }

                    }
                }

            }
            catch (Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, _title, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

        }


        private void biFileQuickPrint_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {

            this.spreadsheetControl1.Document.SaveDocument(AppDomain.CurrentDomain.BaseDirectory + "m6632weighing");

        


            Worksheet worksheet = this.spreadsheetControl1.ActiveWorksheet;

            worksheet.ActiveView.Orientation = PageOrientation.Landscape;
            worksheet.ActiveView.ShowHeadings = true;
            worksheet.ActiveView.PaperKind = System.Drawing.Printing.PaperKind.A4;


            WorksheetPrintOptions printOptions = worksheet.PrintOptions;
            worksheet.ActiveView.ShowHeadings = true;
            worksheet.ActiveView.ShowGridlines = false;
            //worksheet.PrintOptions.FitToPage = true;
            worksheet.PrintOptions.NumberOfCopies = 1;
            worksheet.PrintOptions.PrintGridlines = false;
            worksheet.PrintOptions.BlackAndWhite = true;
            worksheet.PrintOptions.ErrorsPrintMode = ErrorsPrintMode.Dash;
            worksheet.PrintOptions.FitToWidth = 1;
            //worksheet.PrintOptions.FitToHeight = 1;

            using (LegacyPrintableComponentLink link = new LegacyPrintableComponentLink(this.spreadsheetControl1.Document))
            {
                link.PrintingSystem.Watermark.Text = Properties.Settings.Default.SettingCompany;
                link.PrintingSystem.Watermark.TextDirection = DirectionMode.ForwardDiagonal;
                link.PrintingSystem.Watermark.Font = new Font(link.PrintingSystem.Watermark.Font.FontFamily, 40);
                link.PrintingSystem.Watermark.ForeColor = System.Drawing.Color.PaleTurquoise;
                link.PrintingSystem.Watermark.TextTransparency = 150;

                link.Margins.Top = 5;
                link.Margins.Bottom = 5;
                link.Margins.Left = 5;
                link.Margins.Right = 5;


                link.CreateDocument();
                link.PrintDirect();
            }
        }


    }
}
