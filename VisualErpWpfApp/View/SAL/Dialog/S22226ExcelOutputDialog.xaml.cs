using AquilaErpWpfApp3.Util;
using DevExpress.Spreadsheet;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Sale;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Windows;

namespace AquilaErpWpfApp3.View.SAL.Dialog
{
    public partial class S22226ExcelOutputDialog : DXWindow
    {

        //private IList<SaleVo> orgList;
        private SaleVo orgVo;

        public S22226ExcelOutputDialog(string _fileNm)
        {
            InitializeComponent();


            //this.orgList = _saleList;

            excelLoad(_fileNm);
            //btn_link.Click += btn_link_Click;
            btn_save.Click += btn_save_Click;
            btn_close.Click += btn_close_Click;

            this.text_OPMZ_DT.Text = System.DateTime.Now.ToString("yyyy-MM-dd");

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

        async void excelLoad(string _fileNm)
        {
            try
            {
                #region MyRegion
                //    this.spreadsheetControl.CreateNewDocument();


                //    #region 헤더 부분  - [ 주문 GR 번호, 납품처, 공사부위, 생산확정일, 형상, 강종, 규격, 길이, 수량, 중량 ]

                //    IWorkbook workbook = this.spreadsheetControl.Document;
                //    Worksheet worksheet = workbook.Worksheets.ActiveWorksheet;
                //    int _xRow = 1;

                //    //Borders
                //    worksheet.Range["A1:J1"].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                //    worksheet.Range["A1:J1"].Font.Size = 12;
                //    worksheet.Range["A1:J1"].Font.Bold = true;


                //    worksheet.Cells["A" + _xRow].Value = "주문GR번호";
                //    worksheet.Cells["A" + _xRow].ColumnWidth = 350;
                //    worksheet.Cells["A" + _xRow].FillColor = Color.LightGray;
                //    worksheet.Cells["A" + _xRow].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                //    worksheet.Cells["A" + _xRow].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;

                //    worksheet.Cells["B" + _xRow].Value = "납품처";
                //    worksheet.Cells["B" + _xRow].ColumnWidth = 610;
                //    worksheet.Cells["B" + _xRow].FillColor = Color.LightGray;
                //    worksheet.Cells["B" + _xRow].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                //    worksheet.Cells["B" + _xRow].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;

                //    worksheet.Cells["C" + _xRow].Value = "공사부위";
                //    worksheet.Cells["C" + _xRow].ColumnWidth = 710;
                //    worksheet.Cells["C" + _xRow].FillColor = Color.LightGray;
                //    worksheet.Cells["C" + _xRow].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                //    worksheet.Cells["C" + _xRow].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;

                //    worksheet.Cells["D" + _xRow].Value = "생산확정일";
                //    worksheet.Cells["D" + _xRow].ColumnWidth = 310;
                //    worksheet.Cells["D" + _xRow].FillColor = Color.LightGray;
                //    worksheet.Cells["D" + _xRow].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                //    worksheet.Cells["D" + _xRow].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;

                //    worksheet.Cells["E" + _xRow].Value = "형상";
                //    worksheet.Cells["E" + _xRow].ColumnWidth = 210;
                //    worksheet.Cells["E" + _xRow].FillColor = Color.LightGray;
                //    worksheet.Cells["E" + _xRow].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                //    worksheet.Cells["E" + _xRow].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;

                //    worksheet.Cells["F" + _xRow].Value = "강종";
                //    worksheet.Cells["F" + _xRow].ColumnWidth = 210;
                //    worksheet.Cells["F" + _xRow].FillColor = Color.LightGray;
                //    worksheet.Cells["F" + _xRow].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                //    worksheet.Cells["F" + _xRow].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;

                //    worksheet.Cells["G" + _xRow].Value = "규격";
                //    worksheet.Cells["G" + _xRow].ColumnWidth = 210;
                //    worksheet.Cells["G" + _xRow].FillColor = Color.LightGray;
                //    worksheet.Cells["G" + _xRow].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                //    worksheet.Cells["G" + _xRow].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;

                //    worksheet.Cells["H" + _xRow].Value = "길이";
                //    worksheet.Cells["H" + _xRow].ColumnWidth = 210;
                //    worksheet.Cells["H" + _xRow].FillColor = Color.LightGray;
                //    worksheet.Cells["H" + _xRow].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                //    worksheet.Cells["H" + _xRow].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;

                //    worksheet.Cells["I" + _xRow].Value = "수량";
                //    worksheet.Cells["I" + _xRow].ColumnWidth = 210;
                //    worksheet.Cells["I" + _xRow].FillColor = Color.LightGray;
                //    worksheet.Cells["I" + _xRow].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                //    worksheet.Cells["I" + _xRow].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;

                //    worksheet.Cells["J" + _xRow].Value = "중량";
                //    worksheet.Cells["J" + _xRow].ColumnWidth = 210;
                //    worksheet.Cells["J" + _xRow].FillColor = Color.LightGray;
                //    worksheet.Cells["J" + _xRow].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                //    worksheet.Cells["J" + _xRow].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;

                //    #endregion

                //    _xRow = 2;
                //    foreach (SaleVo _item in this.orgList)
                //    {
                //        //주문GR번호
                //        worksheet.Cells["A" + _xRow].Value = _item.RLSE_CMD_NO;
                //        worksheet.Cells["A" + _xRow].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                //        worksheet.Cells["A" + _xRow].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                //        worksheet.Cells["A" + _xRow].Font.Size = 9;
                //        worksheet.Cells["A" + _xRow].Alignment.WrapText = true;
                //        worksheet.Cells["A" + _xRow].Font.Bold = true;

                //        //납품처
                //        worksheet.Cells["B" + _xRow].Value = _item.CNTR_NM;
                //        worksheet.Cells["B" + _xRow].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                //        worksheet.Cells["B" + _xRow].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                //        worksheet.Cells["B" + _xRow].Font.Size = 9;
                //        worksheet.Cells["B" + _xRow].Alignment.WrapText = true;
                //        worksheet.Cells["B" + _xRow].Font.Bold = true;

                //        //공사부위
                //        worksheet.Cells["C" + _xRow].Value = _item.CNTR_PSN_NM;
                //        worksheet.Cells["C" + _xRow].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                //        worksheet.Cells["C" + _xRow].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                //        worksheet.Cells["C" + _xRow].Font.Size = 9;
                //        worksheet.Cells["C" + _xRow].Alignment.WrapText = true;
                //        worksheet.Cells["C" + _xRow].Font.Bold = true;

                //        //생산확정일
                //        worksheet.Cells["D" + _xRow].Value = _item.RLSE_DIV_DT;
                //        worksheet.Cells["D" + _xRow].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                //        worksheet.Cells["D" + _xRow].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                //        worksheet.Cells["D" + _xRow].Font.Size = 9;
                //        worksheet.Cells["D" + _xRow].Alignment.WrapText = true;
                //        worksheet.Cells["D" + _xRow].Font.Bold = true;

                //        //형상
                //        worksheet.Cells["E" + _xRow].Value = _item.SL_ITM_CD;
                //        worksheet.Cells["E" + _xRow].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                //        worksheet.Cells["E" + _xRow].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                //        worksheet.Cells["E" + _xRow].Font.Size = 9;
                //        worksheet.Cells["E" + _xRow].Alignment.WrapText = true;
                //        worksheet.Cells["E" + _xRow].Font.Bold = true;

                //        //강종
                //        worksheet.Cells["F" + _xRow].Value = _item.ITM_STL_CD;
                //        worksheet.Cells["F" + _xRow].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                //        worksheet.Cells["F" + _xRow].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                //        worksheet.Cells["F" + _xRow].Font.Size = 9;
                //        worksheet.Cells["F" + _xRow].Alignment.WrapText = true;
                //        worksheet.Cells["F" + _xRow].Font.Bold = true;


                //        //규격
                //        worksheet.Cells["G" + _xRow].Value = _item.ITM_STL_SZ_CD;
                //        worksheet.Cells["G" + _xRow].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                //        worksheet.Cells["G" + _xRow].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                //        worksheet.Cells["G" + _xRow].Font.Size = 9;
                //        worksheet.Cells["G" + _xRow].Alignment.WrapText = true;
                //        worksheet.Cells["G" + _xRow].Font.Bold = true;


                //        //길이
                //        worksheet.Cells["H" + _xRow].Value = Convert.ToInt32(_item.ITM_STL_LENG);
                //        worksheet.Cells["H" + _xRow].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                //        worksheet.Cells["H" + _xRow].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                //        worksheet.Cells["H" + _xRow].Font.Size = 9;
                //        worksheet.Cells["H" + _xRow].Alignment.WrapText = true;
                //        worksheet.Cells["H" + _xRow].Font.Bold = true;
                //        worksheet.Cells["H" + _xRow].NumberFormat = "###,###,###";


                //        //수량
                //        worksheet.Cells["I" + _xRow].Value = Convert.ToInt32(_item.SL_ITM_QTY);
                //        worksheet.Cells["I" + _xRow].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                //        worksheet.Cells["I" + _xRow].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                //        worksheet.Cells["I" + _xRow].Font.Size = 9;
                //        worksheet.Cells["I" + _xRow].Alignment.WrapText = true;
                //        worksheet.Cells["I" + _xRow].Font.Bold = true;
                //        worksheet.Cells["I" + _xRow].NumberFormat = "###,###,###";

                //        //중량
                //        worksheet.Cells["J" + _xRow].Value = Convert.ToInt32(_item.SL_ITM_WGT);
                //        worksheet.Cells["J" + _xRow].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                //        worksheet.Cells["J" + _xRow].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                //        worksheet.Cells["J" + _xRow].Font.Size = 9;
                //        worksheet.Cells["J" + _xRow].Alignment.WrapText = true;
                //        worksheet.Cells["J" + _xRow].Font.Bold = true;
                //        worksheet.Cells["J" + _xRow].NumberFormat = "###,###,###";


                //        _xRow++;
                //    } 
                #endregion

                List<string> _files = Directory.EnumerateFiles(Properties.Settings.Default.SettingOptPath, _fileNm + "*.xlsx").ToList();

                if (_files.Count > 0)
                {
                    Workbook _book;
                    Workbook _result;
                    List<Workbook> _workbooks = new List<Workbook>();
                    foreach (string _item in _files)
                    {
                        _book = new Workbook();
                        _book.LoadDocument(_item, DocumentFormat.Xlsx);
                        _workbooks.Add(_book);
                    }

                    //Excel 파일 병합 하기
                    _result = Workbook.Merge(_workbooks.ToArray());
                    _result.Worksheets.RemoveAt(2);
                    _result.Worksheets.RemoveAt(0);


                    this.spreadsheetControl.LoadDocument(new System.IO.MemoryStream(_result.SaveDocument(DevExpress.Spreadsheet.DocumentFormat.Xlsx)), DevExpress.Spreadsheet.DocumentFormat.Xlsx);
                }
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
                //DB 저장
                IList<SaleVo> _saveList = new List<SaleVo>();
                IWorkbook workbook = this.spreadsheetControl.Document;
                Worksheet worksheet = workbook.Worksheets.ActiveWorksheet;


                if (worksheet.GetUsedRange().RowCount > 0)
                {
                    MessageBoxResult result = WinUIMessageBox.Show("생산 최적화 [ Sheet : " + worksheet.Name + " ] 저장 하시겠습니까?", "[" + SystemProperties.PROGRAM_TITLE + "] " + this.Title, MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {

                        if (DXSplashScreen.IsActive == false)
                        {
                            DXSplashScreen.Show<ProgressWindow>();
                        }

                        ////DB 저장
                        //IList<SaleVo> _saveList = new List<SaleVo>();
                        //IWorkbook _workbook = this.spreadsheetControl.Document;
                        //Worksheet worksheet = workbook.Worksheets.ActiveWorksheet;

                        //Row 줄 표시
                        //WinUIMessageBox.Show(worksheet.GetUsedRange().RowCount + "", "[" + SystemProperties.PROGRAM_TITLE + "]", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);

                        int _rowCnt = 500;
                        string _OPMZ_NO = "RO" + DateTime.Now.ToString("yyyyMMddHHmmssFFF");
                        //foreach (Worksheet _worksheet in workbook.Worksheets)
                        //{
                        //    _saveList = new List<SaleVo>();

                        //    //Row 줄 표시 -> 값 저장



                        _rowCnt = worksheet.GetUsedRange().RowCount;
                            for (int x = 2; x <= _rowCnt; x++)
                            {
                                if (string.IsNullOrEmpty(worksheet.Cells["B" + x].Value.ToString()))
                                {
                                    continue;
                                }

                                SaleVo _saleVo = new SaleVo();
                        //        //_saleVo.SL_RLSE_NO = this.orgVo.SL_RLSE_NO;
                                _saleVo.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
                                _saleVo.CRE_USR_ID = SystemProperties.USER;
                                _saleVo.UPD_USR_ID = SystemProperties.USER;

                            //
                                _saleVo.OPMZ_NO = _OPMZ_NO;
                                _saleVo.OPMZ_DT = Convert.ToDateTime(this.text_OPMZ_DT.Text).ToString("yyyy-MM-dd");
                                _saleVo.OPMZ_RMK = this.text_OPMZ_RMK.Text;


                                //STEP
                                _saleVo.STEP_NO = Convert.ToInt32(worksheet.Cells["A" + x].Value.NumericValue);

                                //SUB_STEP
                                _saleVo.STEP_SEQ = Convert.ToInt32(worksheet.Cells["B" + x].Value.NumericValue);

                                //L1 (1번 철근 길이)
                                _saleVo.STL_N1ST_LENG = Convert.ToDouble(worksheet.Cells["C" + x].Value.NumericValue);

                                //L2 (2번 철근 길이)
                                _saleVo.STL_N2ND_LENG = Convert.ToDouble(worksheet.Cells["D" + x].Value.NumericValue);

                                //GR1(1번 철근 GR번호)
                                _saleVo.RLSE_N1ST_CMD_NO = worksheet.Cells["E" + x].Value.ToString();

                                //GR2(1번 철근 GR번호)
                                _saleVo.RLSE_N2ND_CMD_NO = worksheet.Cells["F" + x].Value.ToString();

                                //SC1(1번 철근 형상)
                                _saleVo.ITM_N1ST_SHP_CD = worksheet.Cells["G" + x].Value.ToString();

                                //SC2(2번 철근 형상)
                                _saleVo.ITM_N2ND_SHP_CD = worksheet.Cells["H" + x].Value.ToString();

                                //Q1(1번 절단 횟수)
                                _saleVo.STL_N1ST_PROC_TM = Convert.ToInt32(worksheet.Cells["I" + x].Value.NumericValue);

                                //Q2(2번 절단 횟수)
                                _saleVo.STL_N2ND_PROC_TM = Convert.ToInt32(worksheet.Cells["J" + x].Value.NumericValue);

                                //P1
                                _saleVo.PROC_N1ST_SET = worksheet.Cells["K" + x].Value.ToString();

                                //P2
                                _saleVo.PROC_N2ND_SET = worksheet.Cells["L" + x].Value.ToString();

                                //P3
                                _saleVo.PROC_N3RD_SET = worksheet.Cells["M" + x].Value.ToString();

                                //P4
                                _saleVo.PROC_N4ND_SET = worksheet.Cells["N" + x].Value.ToString();

                                //SL(투입 철근 길이)
                                _saleVo.IN_STL_LENG = Convert.ToInt32(worksheet.Cells["O" + x].Value.NumericValue);

                                //SQ(투입 철근 수량)
                                _saleVo.IN_STL_QTY = Convert.ToInt32(worksheet.Cells["P" + x].Value.NumericValue);

                                //STEP_LOSS_RATIO(자투리)
                                _saleVo.STEP_LSS_RTO = worksheet.Cells["Q" + x].Value.ToString();

                                //STEP_TOTAL_STOCK(총 길이)
                                _saleVo.STEP_TTL_STK = Convert.ToDouble(worksheet.Cells["R" + x].Value.NumericValue);

                                //STEP_LOSS_TOTAL(로스율)
                                _saleVo.STEP_LSS_TTL = Convert.ToDouble(worksheet.Cells["S" + x].Value.NumericValue);

                                //WAITING_LIST(필요 대기공간)
                                _saleVo.WT_LIST = Convert.ToInt32(worksheet.Cells["T" + x].Value.NumericValue);

                                //BP_WAITING_(대기 GR번호)
                                _saleVo.BP_WT_LIST = worksheet.Cells["U" + x].Value.ToString();

                                _saveList.Add(_saleVo);
                                

                               
                            }

                        //
                        //저장 DB
                        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s22226/popup/m", new StringContent(JsonConvert.SerializeObject(_saveList), System.Text.Encoding.UTF8, "application/json")))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                int _Num = 0;
                                string resultMsg = await response.Content.ReadAsStringAsync();
                                if (int.TryParse(resultMsg, out _Num) == false)
                                {
                                    //실패
                                    WinUIMessageBox.Show(resultMsg, "[" + SystemProperties.PROGRAM_TITLE + "] " + this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                                    return;
                                }
                            }
                        }

                            //
                        if (DXSplashScreen.IsActive == true)
                        {
                            DXSplashScreen.Close();
                        }

                        this.DialogResult = true;
                        this.Close();
                    }
                }
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
