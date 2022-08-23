using AquilaErpWpfApp3.Util;
using DevExpress.Spreadsheet;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Spreadsheet;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Sale;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Windows;

namespace AquilaErpWpfApp3.View.SAL.Dialog
{
    public partial class S22226ExcelInputDialog : DXWindow
    {

        private IList<SaleVo> orgList;
        private SaleVo orgVo;

        public S22226ExcelInputDialog(IList<SaleVo> _saleList)
        {
            InitializeComponent();


            this.orgList = _saleList;

            excelLoad();
            //btn_link.Click += btn_link_Click;
            btn_save.Click += btn_save_Click;
            btn_close.Click += btn_close_Click;
            //excelLoad();
        }

        //public S22226ExcelInputDialog(string _fileLoc, SaleVo _orgVo)
        //{
        //    InitializeComponent();


        //    //this.orgList = _purList;
        //    this.orgVo = _orgVo;

        //    byte[] xlsxFile = File.ReadAllBytes(_fileLoc);
        //    this.spreadsheetControl.DocumentSource = new SpreadsheetDocumentSource(new System.IO.MemoryStream(xlsxFile), DevExpress.Spreadsheet.DocumentFormat.Xlsx);

        //    //excelLoad();
        //    //btn_link.Click += btn_link_Click;
        //    btn_save.Click += btn_save_Click;
        //    btn_close.Click += btn_close_Click;
        //    //excelLoad();
        //}


        void btn_close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        async void excelLoad()
        {
            try
            {

                this.spreadsheetControl.CreateNewDocument();


                #region 헤더 부분  - [ 주문 GR 번호, 납품처, 공사부위, 생산확정일, 형상, 강종, 규격, 길이, 수량, 중량 ]

                IWorkbook workbook = this.spreadsheetControl.Document;
                Worksheet worksheet = workbook.Worksheets.ActiveWorksheet;
                int _xRow = 1;

                //Borders
                worksheet.Range["A1:N1"].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                worksheet.Range["A1:N1"].Font.Size = 12;
                worksheet.Range["A1:N1"].Font.Bold = true;


                worksheet.Cells["A" + _xRow].Value = "주문GR번호";
                worksheet.Cells["A" + _xRow].ColumnWidth = 350;
                worksheet.Cells["A" + _xRow].FillColor = Color.LightGray;
                worksheet.Cells["A" + _xRow].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                worksheet.Cells["A" + _xRow].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;

                worksheet.Cells["D" + _xRow].Value = "납품처";
                worksheet.Cells["D" + _xRow].ColumnWidth = 610;
                worksheet.Cells["D" + _xRow].FillColor = Color.LightGray;
                worksheet.Cells["D" + _xRow].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                worksheet.Cells["D" + _xRow].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;

                worksheet.Cells["E" + _xRow].Value = "공사부위";
                worksheet.Cells["E" + _xRow].ColumnWidth = 710;
                worksheet.Cells["E" + _xRow].FillColor = Color.LightGray;
                worksheet.Cells["E" + _xRow].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                worksheet.Cells["E" + _xRow].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;

                worksheet.Cells["F" + _xRow].Value = "생산확정일";
                worksheet.Cells["F" + _xRow].ColumnWidth = 310;
                worksheet.Cells["F" + _xRow].FillColor = Color.LightGray;
                worksheet.Cells["F" + _xRow].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                worksheet.Cells["F" + _xRow].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;

                worksheet.Cells["H" + _xRow].Value = "바코드";
                worksheet.Cells["H" + _xRow].ColumnWidth = 350;
                worksheet.Cells["H" + _xRow].FillColor = Color.LightGray;
                worksheet.Cells["H" + _xRow].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                worksheet.Cells["H" + _xRow].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;

                worksheet.Cells["I" + _xRow].Value = "형상";
                worksheet.Cells["I" + _xRow].ColumnWidth = 210;
                worksheet.Cells["I" + _xRow].FillColor = Color.LightGray;
                worksheet.Cells["I" + _xRow].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                worksheet.Cells["I" + _xRow].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;

                worksheet.Cells["J" + _xRow].Value = "강종";
                worksheet.Cells["J" + _xRow].ColumnWidth = 210;
                worksheet.Cells["J" + _xRow].FillColor = Color.LightGray;
                worksheet.Cells["J" + _xRow].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                worksheet.Cells["J" + _xRow].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;

                worksheet.Cells["K" + _xRow].Value = "규격";
                worksheet.Cells["K" + _xRow].ColumnWidth = 210;
                worksheet.Cells["K" + _xRow].FillColor = Color.LightGray;
                worksheet.Cells["K" + _xRow].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                worksheet.Cells["K" + _xRow].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;

                worksheet.Cells["L" + _xRow].Value = "길이";
                worksheet.Cells["L" + _xRow].ColumnWidth = 210;
                worksheet.Cells["L" + _xRow].FillColor = Color.LightGray;
                worksheet.Cells["L" + _xRow].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                worksheet.Cells["L" + _xRow].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;

                worksheet.Cells["M" + _xRow].Value = "수량";
                worksheet.Cells["M" + _xRow].ColumnWidth = 210;
                worksheet.Cells["M" + _xRow].FillColor = Color.LightGray;
                worksheet.Cells["M" + _xRow].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                worksheet.Cells["M" + _xRow].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;

                worksheet.Cells["N" + _xRow].Value = "중량";
                worksheet.Cells["N" + _xRow].ColumnWidth = 210;
                worksheet.Cells["N" + _xRow].FillColor = Color.LightGray;
                worksheet.Cells["N" + _xRow].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                worksheet.Cells["N" + _xRow].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;

                #endregion

                _xRow = 2;
                foreach (SaleVo _item in this.orgList)
                {
                    //주문GR번호
                    worksheet.Cells["A" + _xRow].Value = _item.RLSE_CMD_NO;
                    worksheet.Cells["A" + _xRow].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                    worksheet.Cells["A" + _xRow].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                    worksheet.Cells["A" + _xRow].Font.Size = 9;
                    worksheet.Cells["A" + _xRow].Alignment.WrapText = true;
                    worksheet.Cells["A" + _xRow].Font.Bold = true;


                    //납품처
                    worksheet.Cells["D" + _xRow].Value = _item.CNTR_NM;
                    worksheet.Cells["D" + _xRow].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                    worksheet.Cells["D" + _xRow].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                    worksheet.Cells["D" + _xRow].Font.Size = 9;
                    worksheet.Cells["D" + _xRow].Alignment.WrapText = true;
                    worksheet.Cells["D" + _xRow].Font.Bold = true;


                    //공사부위
                    worksheet.Cells["E" + _xRow].Value = _item.CNTR_PSN_NM;
                    worksheet.Cells["E" + _xRow].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                    worksheet.Cells["E" + _xRow].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                    worksheet.Cells["E" + _xRow].Font.Size = 9;
                    worksheet.Cells["E" + _xRow].Alignment.WrapText = true;
                    worksheet.Cells["E" + _xRow].Font.Bold = true;


                    //생산확정일
                    worksheet.Cells["F" + _xRow].Value = _item.RLSE_DIV_DT;
                    worksheet.Cells["F" + _xRow].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                    worksheet.Cells["F" + _xRow].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                    worksheet.Cells["F" + _xRow].Font.Size = 9;
                    worksheet.Cells["F" + _xRow].Alignment.WrapText = true;
                    worksheet.Cells["F" + _xRow].Font.Bold = true;


                    //바코드
                    worksheet.Cells["H" + _xRow].Value = _item.RLSR_BCD_NO;
                    worksheet.Cells["H" + _xRow].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                    worksheet.Cells["H" + _xRow].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                    worksheet.Cells["H" + _xRow].Font.Size = 9;
                    worksheet.Cells["H" + _xRow].Alignment.WrapText = true;
                    worksheet.Cells["H" + _xRow].Font.Bold = true;


                    //형상
                    worksheet.Cells["I" + _xRow].Value = _item.SL_ITM_CD;
                    worksheet.Cells["I" + _xRow].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                    worksheet.Cells["I" + _xRow].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                    worksheet.Cells["I" + _xRow].Font.Size = 9;
                    worksheet.Cells["I" + _xRow].Alignment.WrapText = true;
                    worksheet.Cells["I" + _xRow].Font.Bold = true;


                    //강종
                    worksheet.Cells["J" + _xRow].Value = _item.ITM_STL_CD;
                    worksheet.Cells["J" + _xRow].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                    worksheet.Cells["J" + _xRow].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                    worksheet.Cells["J" + _xRow].Font.Size = 9;
                    worksheet.Cells["J" + _xRow].Alignment.WrapText = true;
                    worksheet.Cells["J" + _xRow].Font.Bold = true;


                    //규격
                    worksheet.Cells["K" + _xRow].Value = _item.ITM_STL_SZ_CD.Substring(_item.ITM_STL_SZ_CD.Length-3);
                    worksheet.Cells["K" + _xRow].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                    worksheet.Cells["K" + _xRow].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                    worksheet.Cells["K" + _xRow].Font.Size = 9;
                    worksheet.Cells["K" + _xRow].Alignment.WrapText = true;
                    worksheet.Cells["K" + _xRow].Font.Bold = true;


                    //길이
                    worksheet.Cells["L" + _xRow].Value = Convert.ToInt32(_item.ITM_STL_LENG);
                    worksheet.Cells["L" + _xRow].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                    worksheet.Cells["L" + _xRow].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                    worksheet.Cells["L" + _xRow].Font.Size = 9;
                    worksheet.Cells["L" + _xRow].Alignment.WrapText = true;
                    worksheet.Cells["L" + _xRow].Font.Bold = true;
                    worksheet.Cells["L" + _xRow].NumberFormat = "###,###,###";


                    //수량
                    worksheet.Cells["M" + _xRow].Value = Convert.ToInt32(_item.SL_ITM_QTY);
                    worksheet.Cells["M" + _xRow].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                    worksheet.Cells["M" + _xRow].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                    worksheet.Cells["M" + _xRow].Font.Size = 9;
                    worksheet.Cells["M" + _xRow].Alignment.WrapText = true;
                    worksheet.Cells["M" + _xRow].Font.Bold = true;
                    worksheet.Cells["M" + _xRow].NumberFormat = "###,###,###";

                    //중량
                    worksheet.Cells["N" + _xRow].Value = Convert.ToInt32(_item.SL_ITM_WGT);
                    worksheet.Cells["N" + _xRow].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                    worksheet.Cells["N" + _xRow].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                    worksheet.Cells["N" + _xRow].Font.Size = 9;
                    worksheet.Cells["N" + _xRow].Alignment.WrapText = true;
                    worksheet.Cells["N" + _xRow].Font.Bold = true;
                    worksheet.Cells["N" + _xRow].NumberFormat = "###,###,###";


                    _xRow++;
                }





                //Workbook _book;
                //Workbook _result;
                //List<Workbook> _workbooks = new List<Workbook>();
                //foreach (SaleVo _item in this.orgList)
                //{
                //    _book = new Workbook();
                //    _book.LoadDocument(_item.FLR_FILE, DocumentFormat.Xlsx);
                //    _workbooks.Add(_book);

                //    //File.WriteAllBytes(Path.Combine(_di.FullName, "TMP_" + System.DateTime.Now.ToString("yyyyMMddhhmmssfff") + "_" + _item.FLR_NM), _item.FLR_FILE);
                //}

                ////Excel 파일 병합 하기
                //_result = Workbook.Merge(_workbooks.ToArray());

                //this.spreadsheetControl.LoadDocument(new System.IO.MemoryStream(_result.SaveDocument(DevExpress.Spreadsheet.DocumentFormat.Xlsx)), DevExpress.Spreadsheet.DocumentFormat.Xlsx);
            }
            catch (Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "] " + this.Title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }

        #region 이전 작업
        //async void btn_save_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {

        //        MessageBoxResult result = WinUIMessageBox.Show("[주문등록 → 저장] 하시겠습니까?", "[" + SystemProperties.PROGRAM_TITLE + "] " + this.Title, MessageBoxButton.YesNo, MessageBoxImage.Question);
        //        if (result == MessageBoxResult.Yes)
        //        {

        //            if (DXSplashScreen.IsActive == false)
        //            {
        //                DXSplashScreen.Show<ProgressWindow>();
        //            }

        //            //DB 저장
        //            IList<SaleVo> _saveList = new List<SaleVo>();
        //            IWorkbook _workbook = this.spreadsheetControl.Document;
        //            //Worksheet worksheet = workbook.Worksheets.ActiveWorksheet;

        //            //Row 줄 표시
        //            //WinUIMessageBox.Show(worksheet.GetUsedRange().RowCount + "", "[" + SystemProperties.PROGRAM_TITLE + "]", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
        //            int _nCnt = 0;
        //            int _rowCnt = 0;
        //            foreach (Worksheet worksheet in _workbook.Worksheets)
        //            {
        //                _saveList = new List<SaleVo>();

        //                //SaleVo _saleVo = new SaleVo();
        //                //_saleVo.SL_RLSE_NO = this.orgList[_nCnt].GBN;
        //                //_saleVo.PUR_ORD_NO = this.orgList[_nCnt].PUR_NO;
        //                //_saleVo.PUR_ORD_SEQ = this.orgList[_nCnt].PUR_SEQ;

        //                //_saleVo.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
        //                //_saleVo.CRE_USR_ID = SystemProperties.USER;
        //                //_saleVo.UPD_USR_ID = SystemProperties.USER;

        //                //Row 줄 표시 -> 값 저장
        //                _rowCnt = worksheet.GetUsedRange().RowCount;
        //                for (int x = 4; x <= _rowCnt; x++)
        //                {
        //                    if (string.IsNullOrEmpty(worksheet.Cells["A" + x].Value.ToString()))
        //                    {
        //                        continue;
        //                    }

        //                    SaleVo _saleVo = new SaleVo();
        //                    _saleVo.SL_RLSE_NO = this.orgList[_nCnt].GBN;
        //                    _saleVo.PUR_ORD_NO = this.orgList[_nCnt].PUR_NO;
        //                    _saleVo.PUR_ORD_SEQ = this.orgList[_nCnt].PUR_SEQ;

        //                    _saleVo.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
        //                    _saleVo.CRE_USR_ID = SystemProperties.USER;
        //                    _saleVo.UPD_USR_ID = SystemProperties.USER;

        //                    //도면 번호
        //                    _saleVo.A1 = worksheet.Cells["A" + x].Value.ToString();
        //                    //공사명
        //                    _saleVo.A2 = worksheet.Cells["C" + x].Value.ToString();
        //                    //공사부위
        //                    _saleVo.A3 = worksheet.Cells["D" + x].Value.ToString();
        //                    //부호
        //                    _saleVo.A4 = worksheet.Cells["E" + x].Value.ToString();
        //                    //강종
        //                    _saleVo.A5 = worksheet.Cells["K" + x].Value.ToString();
        //                    //규격
        //                    _saleVo.A6 = worksheet.Cells["L" + x].Value.ToString();
        //                    //길이
        //                    _saleVo.A7 = worksheet.Cells["M" + x].Value.ToString();
        //                    //품명
        //                    _saleVo.A8 = worksheet.Cells["N" + x].Value.ToString();
        //                    //수주수량
        //                    _saleVo.A9 = worksheet.Cells["P" + x].Value.ToString();
        //                    //수주중량
        //                    _saleVo.B1 = worksheet.Cells["Q" + x].Value.ToString();
        //                    _saveList.Add(_saleVo);
        //                }
        //                _nCnt++;

        //                //
        //                //저장 DB
        //                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s3321/dtl/i", new StringContent(JsonConvert.SerializeObject(_saveList), System.Text.Encoding.UTF8, "application/json")))
        //                {
        //                    if (response.IsSuccessStatusCode)
        //                    {
        //                        int _Num = 0;
        //                        string resultMsg = await response.Content.ReadAsStringAsync();
        //                        if (int.TryParse(resultMsg, out _Num) == false)
        //                        {
        //                            //실패
        //                            WinUIMessageBox.Show(resultMsg, "[" + SystemProperties.PROGRAM_TITLE + "] " + this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
        //                            return;
        //                        }
        //                    }
        //                }
        //            }

        //            //
        //            if (DXSplashScreen.IsActive == true)
        //            {
        //                DXSplashScreen.Close();
        //            }

        //            this.DialogResult = true;
        //            this.Close();
        //        }
        //    }
        //    catch (Exception eLog)
        //    {

        //        if (DXSplashScreen.IsActive == true)
        //        {
        //            DXSplashScreen.Close();
        //        }
        //        WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "] " + this.Title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
        //        return;
        //    }

        //}
        #endregion


        async void btn_save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //MessageBoxResult result = WinUIMessageBox.Show("생산 최적화 하시겠습니까?", "[" + SystemProperties.PROGRAM_TITLE + "] " + this.Title, MessageBoxButton.YesNo, MessageBoxImage.Question);
                //if (result == MessageBoxResult.Yes)
                //{

                    //if (DXSplashScreen.IsActive == false)
                    //{
                    //    DXSplashScreen.Show<SplashScreenView>();
                    //    DXSplashScreen.UseDefaultAltTabBehavior = true;
                    //    DXSplashScreen.UseLegacyLocationLogic = true;
                    //    DXSplashScreen.UIThreadReleaseMode = UIThreadReleaseMode.WaitForSplashScreenLoaded;
                    //}

                    ////DB 저장
                    //IList<SaleVo> _saveList = new List<SaleVo>();
                    //IWorkbook _workbook = this.spreadsheetControl.Document;
                    //Worksheet worksheet = workbook.Worksheets.ActiveWorksheet;

                    //Row 줄 표시
                    //WinUIMessageBox.Show(worksheet.GetUsedRange().RowCount + "", "[" + SystemProperties.PROGRAM_TITLE + "]", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);

                    //int _rowCnt = 500;
                    //foreach (Worksheet worksheet in _workbook.Worksheets)
                    //{
                    //    _saveList = new List<SaleVo>();

                    //    //Row 줄 표시 -> 값 저장

                    //    _rowCnt = worksheet.GetUsedRange().RowCount;
                    //    for (int x = 2; x <= _rowCnt; x++)
                    //    {
                    //        if (string.IsNullOrEmpty(worksheet.Cells["B" + x].Value.ToString()))
                    //        {
                    //            continue;
                    //        }

                    //        SaleVo _saleVo = new SaleVo();
                    //        //_saleVo.SL_RLSE_NO = this.orgVo.SL_RLSE_NO;
                    //        _saleVo.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
                    //        _saleVo.CRE_USR_ID = SystemProperties.USER;
                    //        _saleVo.UPD_USR_ID = SystemProperties.USER;

                    //        _saleVo.SL_RLSE_NO = this.orgVo.GBN;
                    //        //_saleVo.PUR_ORD_NO = this.orgVo.PUR_NO;
                    //        //_saleVo.PUR_ORD_SEQ = this.orgVo.PUR_SEQ;
                    //        _saleVo.CNTR_NM = this.orgVo.CNTR_NM;
                    //        //_saleVo.CNTR_PSN_NM = this.orgVo.CNTR_PSN_NM;
                    //        _saleVo.FLR_NO = this.orgVo.FLR_NO;
                    //        _saleVo.IN_REQ_DT = this.orgVo.IN_REQ_DT;


                    //        // 부호
                    //        // 형번
                    //        // 가공정보
                    //        _saleVo.SL_ITM_CD = worksheet.Cells["C" + x].Value.ToString();
                    //        // 철근규격
                    //        _saleVo.ITM_STL_SZ_CD = worksheet.Cells["E" + x].Value.ToString();
                    //        // 길이
                    //        _saleVo.ITM_STL_LENG = Convert.ToDouble(worksheet.Cells["F" + x].Value.ToString());
                    //        // 수량
                    //        _saleVo.A1 = Convert.ToDouble(worksheet.Cells["G" + x].Value.ToString());
                    //        // 총중량
                    //        _saleVo.SL_ITM_WGT = Convert.ToDouble(worksheet.Cells["I" + x].Value.ToString());
                    //        // 위치
                    //        _saleVo.CNTR_PSN_NM = worksheet.Cells["J" + x].Value.ToString();
                    //        // 강종
                    //        _saleVo.ITM_STL_CD = worksheet.Cells["K" + x].Value.ToString();

                    //        _saveList.Add(_saleVo);
                    //    }


                    //    //
                    //    //저장 DB
                    //    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s3321/dtl/i", new StringContent(JsonConvert.SerializeObject(_saveList), System.Text.Encoding.UTF8, "application/json")))
                    //    {
                    //        if (response.IsSuccessStatusCode)
                    //        {
                    //            int _Num = 0;
                    //            string resultMsg = await response.Content.ReadAsStringAsync();
                    //            if (int.TryParse(resultMsg, out _Num) == false)
                    //            {
                    //                //실패
                    //                WinUIMessageBox.Show(resultMsg, "[" + SystemProperties.PROGRAM_TITLE + "] " + this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                    //                return;
                    //            }
                    //        }
                    //    }
                    //}

                    //
                    //if (DXSplashScreen.IsActive == true)
                    //{
                    //    DXSplashScreen.Close();
                    //}

                    this.DialogResult = true;
                    this.Close();
                //}
            }
            catch (Exception eLog)
            {

                if (DXSplashScreen.IsActive == true)
                {
                    DXSplashScreen.Close();
                }
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "] " + this.Title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }

        }

    }
}
