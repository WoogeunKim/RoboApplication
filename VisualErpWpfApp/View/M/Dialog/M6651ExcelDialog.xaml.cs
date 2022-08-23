using DevExpress.Spreadsheet;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Printing;
using DevExpress.Xpf.WindowsUI;
using DevExpress.XtraPrinting.Drawing;
using ModelsLibrary.Man;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Linq;
using AquilaErpWpfApp3.Util;

namespace AquilaErpWpfApp3.View.M.Dialog
{
    public partial class M6651ExcelDialog : DXWindow
    {
        //private static FproofServiceClient fproofClient = SystemProperties.FproofClient;

        private IWorkbook workbook;
        //private Range rangeTmp;

        public M6651ExcelDialog(IList<ManVo> _items)
        {
            InitializeComponent();

            //this.FULL_FILE_PATH = _FULL_FILE_PATH;


            this.btn_close.Click += btn_close_Click;
            //this.btn_saveAs.Click += btn_saveAs_Click;
            this.btn_print.Click += btn_print_Click;


            //Excel 세팅
            try
            {
                DXSplashScreen.Show<ProgressWindow>();

                this.spreadsheetControl.CreateNewDocument();
                this.spreadsheetControl.ActiveWorksheet.ActiveView.Zoom = 95;
                this.spreadsheetControl.ActiveViewZoom = 95;

                workbook = this.spreadsheetControl.Document;

                workbook.Worksheets.ActiveWorksheet.PrintOptions.FitToPage = true;
                //workbook.Worksheets.ActiveWorksheet.PrintOptions.FitToHeight = true;
                //workbook.Worksheets.ActiveWorksheet.PrintOptions.FitToWidth = true;
                workbook.Worksheets.ActiveWorksheet.PrintOptions.PrintGridlines = false;
                workbook.Worksheets.ActiveWorksheet.ActiveView.ShowGridlines = false;


                #region 제목 단
                //A1 축소
                workbook.Worksheets.ActiveWorksheet.Cells["A1"].ColumnWidth = 30;

                //대제목 - 2020年 09月 生産計劃 對 實績
                workbook.Worksheets.ActiveWorksheet.MergeCells(workbook.Worksheets.ActiveWorksheet.Range["F1:AB2"]);
                workbook.Worksheets.ActiveWorksheet.Cells["F1"].Value = Convert.ToDateTime(_items[0].PROD_PLN_DT).ToString("yyyy") + "年 " + Convert.ToDateTime(_items[0].PROD_PLN_DT).ToString("MM") + "月 生産計劃 對 實績";
                workbook.Worksheets.ActiveWorksheet.Cells["F1"].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                workbook.Worksheets.ActiveWorksheet.Cells["F1"].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                workbook.Worksheets.ActiveWorksheet.Cells["F1"].Font.Size = 36;
                workbook.Worksheets.ActiveWorksheet.Cells["F1"].Font.Bold = true;

                //결재
                workbook.Worksheets.ActiveWorksheet.MergeCells(workbook.Worksheets.ActiveWorksheet.Range["AI1:AN1"]);
                workbook.Worksheets.ActiveWorksheet.Cells["AI1"].Value = "결           재";
                workbook.Worksheets.ActiveWorksheet.Cells["AI1"].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                workbook.Worksheets.ActiveWorksheet.Cells["AI1"].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                workbook.Worksheets.ActiveWorksheet.Cells["AI1"].Font.Size = 20;
                workbook.Worksheets.ActiveWorksheet.Range["AI1:AN1"].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                //
                workbook.Worksheets.ActiveWorksheet.MergeCells(workbook.Worksheets.ActiveWorksheet.Range["AI2:AN2"]);
                workbook.Worksheets.ActiveWorksheet.Cells["AN2"].RowHeight = 200;
                workbook.Worksheets.ActiveWorksheet.Range["AI2:AN2"].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);

                //공정명 : FORMING
                workbook.Worksheets.ActiveWorksheet.MergeCells(workbook.Worksheets.ActiveWorksheet.Range["B2:E2"]);
                workbook.Worksheets.ActiveWorksheet.Cells["B2"].Value = "";
                workbook.Worksheets.ActiveWorksheet.Cells["B2"].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                workbook.Worksheets.ActiveWorksheet.Cells["B2"].Alignment.Vertical = SpreadsheetVerticalAlignment.Bottom;
                workbook.Worksheets.ActiveWorksheet.Cells["B2"].Font.Size = 16;
                workbook.Worksheets.ActiveWorksheet.Cells["B2"].Font.UnderlineType = UnderlineType.Single;
                workbook.Worksheets.ActiveWorksheet.Cells["B2"].Font.Bold = true;


                //A3 축소
                workbook.Worksheets.ActiveWorksheet.Cells["A3"].RowHeight = 10;

                //소제목 - 설비명 / 차종 / 발주량 / 고객사 재고 / 재고 / 생산 계획
                //설비명
                workbook.Worksheets.ActiveWorksheet.MergeCells(workbook.Worksheets.ActiveWorksheet.Range["B4:B5"]);
                workbook.Worksheets.ActiveWorksheet.Range["B4:B5"].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                workbook.Worksheets.ActiveWorksheet.Cells["B4"].Value = "설비명";
                workbook.Worksheets.ActiveWorksheet.Cells["B4"].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                workbook.Worksheets.ActiveWorksheet.Cells["B4"].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                workbook.Worksheets.ActiveWorksheet.Cells["B4"].Font.Size = 11;
                workbook.Worksheets.ActiveWorksheet.Cells["B4"].Alignment.WrapText = true;
                //차종
                workbook.Worksheets.ActiveWorksheet.MergeCells(workbook.Worksheets.ActiveWorksheet.Range["C4:C5"]);
                workbook.Worksheets.ActiveWorksheet.Range["C4:C5"].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                workbook.Worksheets.ActiveWorksheet.Cells["C4"].Value = "품명";
                workbook.Worksheets.ActiveWorksheet.Cells["C4"].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                workbook.Worksheets.ActiveWorksheet.Cells["C4"].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                workbook.Worksheets.ActiveWorksheet.Cells["C4"].Font.Size = 11;
                workbook.Worksheets.ActiveWorksheet.Cells["C4"].Alignment.WrapText = true;
                //발주량
                workbook.Worksheets.ActiveWorksheet.MergeCells(workbook.Worksheets.ActiveWorksheet.Range["D4:D5"]);
                workbook.Worksheets.ActiveWorksheet.Range["D4:D5"].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                workbook.Worksheets.ActiveWorksheet.Cells["D4"].Value = "발주량";
                workbook.Worksheets.ActiveWorksheet.Cells["D4"].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                workbook.Worksheets.ActiveWorksheet.Cells["D4"].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                workbook.Worksheets.ActiveWorksheet.Cells["D4"].Font.Size = 11;
                workbook.Worksheets.ActiveWorksheet.Cells["D4"].ColumnWidth = 150;
                workbook.Worksheets.ActiveWorksheet.Cells["D4"].Alignment.WrapText = true;
                //고객사 재고
                workbook.Worksheets.ActiveWorksheet.MergeCells(workbook.Worksheets.ActiveWorksheet.Range["E4:E5"]);
                workbook.Worksheets.ActiveWorksheet.Range["E4:E5"].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                workbook.Worksheets.ActiveWorksheet.Cells["E4"].Value = "고객사" + System.Environment.NewLine + "재  고";
                workbook.Worksheets.ActiveWorksheet.Cells["E4"].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                workbook.Worksheets.ActiveWorksheet.Cells["E4"].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                workbook.Worksheets.ActiveWorksheet.Cells["E4"].Font.Size = 11;
                workbook.Worksheets.ActiveWorksheet.Cells["E4"].ColumnWidth = 150;
                workbook.Worksheets.ActiveWorksheet.Cells["E4"].Alignment.WrapText = true;
                //재고
                workbook.Worksheets.ActiveWorksheet.MergeCells(workbook.Worksheets.ActiveWorksheet.Range["F4:F5"]);
                workbook.Worksheets.ActiveWorksheet.Range["F4:F5"].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                workbook.Worksheets.ActiveWorksheet.Cells["F4"].Value = "재고";
                workbook.Worksheets.ActiveWorksheet.Cells["F4"].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                workbook.Worksheets.ActiveWorksheet.Cells["F4"].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                workbook.Worksheets.ActiveWorksheet.Cells["F4"].Font.Size = 11;
                workbook.Worksheets.ActiveWorksheet.Cells["F4"].ColumnWidth = 150;
                workbook.Worksheets.ActiveWorksheet.Cells["F4"].Alignment.WrapText = true;
                //생산 계획
                workbook.Worksheets.ActiveWorksheet.MergeCells(workbook.Worksheets.ActiveWorksheet.Range["G4:G5"]);
                workbook.Worksheets.ActiveWorksheet.Range["G4:G5"].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                workbook.Worksheets.ActiveWorksheet.Cells["G4"].Value = "생 산" + System.Environment.NewLine + "계 획";
                workbook.Worksheets.ActiveWorksheet.Cells["G4"].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                workbook.Worksheets.ActiveWorksheet.Cells["G4"].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                workbook.Worksheets.ActiveWorksheet.Cells["G4"].Font.Size = 11;
                workbook.Worksheets.ActiveWorksheet.Cells["G4"].ColumnWidth = 150;
                workbook.Worksheets.ActiveWorksheet.Cells["G4"].Alignment.WrapText = true;


                //일일생산계획
                workbook.Worksheets.ActiveWorksheet.MergeCells(workbook.Worksheets.ActiveWorksheet.Range["H4:AL4"]);
                workbook.Worksheets.ActiveWorksheet.Range["H4:AL4"].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                workbook.Worksheets.ActiveWorksheet.Cells["H4"].Value = "일   일   생   산   계   획";
                workbook.Worksheets.ActiveWorksheet.Cells["H4"].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                workbook.Worksheets.ActiveWorksheet.Cells["H4"].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                workbook.Worksheets.ActiveWorksheet.Cells["H4"].Font.Size = 11;
                workbook.Worksheets.ActiveWorksheet.Cells["H4"].Alignment.WrapText = true;

                DateTime _tmpDt;
                for (int x = 1; x <= 31; x++)
                {
                    try
                    {
                        _tmpDt = DateTime.ParseExact(Convert.ToDateTime(_items[0].PROD_PLN_DT).ToString("yyyy-MM") + "-" + x.ToString("D2"), "yyyy-MM-dd", null);

                        //1 ~ 31 => 요일 표시
                        workbook.Worksheets.ActiveWorksheet.Range[ColumnIndexToColumnLetter(7 + x) + "5"].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                        workbook.Worksheets.ActiveWorksheet.Cells[ColumnIndexToColumnLetter(7 + x) + "5"].Value = "" + x + "(" + WeekDay(_tmpDt.DayOfWeek) + ")";
                        workbook.Worksheets.ActiveWorksheet.Cells[ColumnIndexToColumnLetter(7 + x) + "5"].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                        workbook.Worksheets.ActiveWorksheet.Cells[ColumnIndexToColumnLetter(7 + x) + "5"].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                        workbook.Worksheets.ActiveWorksheet.Cells[ColumnIndexToColumnLetter(7 + x) + "5"].Font.Size = 6;
                        workbook.Worksheets.ActiveWorksheet.Cells[ColumnIndexToColumnLetter(7 + x) + "5"].ColumnWidth = 90;

                        if ((DayOfWeek.Saturday == _tmpDt.DayOfWeek) || (DayOfWeek.Sunday == _tmpDt.DayOfWeek))
                        {
                            //노랑 표시
                            workbook.Worksheets.ActiveWorksheet.Cells[ColumnIndexToColumnLetter(7 + x) + "5"].FillColor = Color.Yellow;
                        }
                    }
                    catch
                    {
                        //1 ~ 31 => 요일 표시
                        workbook.Worksheets.ActiveWorksheet.Range[ColumnIndexToColumnLetter(7 + x) + "5"].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                        workbook.Worksheets.ActiveWorksheet.Cells[ColumnIndexToColumnLetter(7 + x) + "5"].Value = "";
                        workbook.Worksheets.ActiveWorksheet.Cells[ColumnIndexToColumnLetter(7 + x) + "5"].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                        workbook.Worksheets.ActiveWorksheet.Cells[ColumnIndexToColumnLetter(7 + x) + "5"].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                        workbook.Worksheets.ActiveWorksheet.Cells[ColumnIndexToColumnLetter(7 + x) + "5"].Font.Size = 6;
                        workbook.Worksheets.ActiveWorksheet.Cells[ColumnIndexToColumnLetter(7 + x) + "5"].ColumnWidth = 90;
                    }
                }
                //

                //실적현황
                workbook.Worksheets.ActiveWorksheet.MergeCells(workbook.Worksheets.ActiveWorksheet.Range["AM4:AN4"]);
                workbook.Worksheets.ActiveWorksheet.Range["AM4:AN4"].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                workbook.Worksheets.ActiveWorksheet.Cells["AM4"].Value = "실 적 현 황";
                workbook.Worksheets.ActiveWorksheet.Cells["AM4"].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                workbook.Worksheets.ActiveWorksheet.Cells["AM4"].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                workbook.Worksheets.ActiveWorksheet.Cells["AM4"].Font.Size = 10;
                workbook.Worksheets.ActiveWorksheet.Cells["AM4"].Alignment.WrapText = true;
                //
                workbook.Worksheets.ActiveWorksheet.Range["AM5"].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                workbook.Worksheets.ActiveWorksheet.Cells["AM5"].Value = "수  량";
                workbook.Worksheets.ActiveWorksheet.Cells["AM5"].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                workbook.Worksheets.ActiveWorksheet.Cells["AM5"].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                workbook.Worksheets.ActiveWorksheet.Cells["AM5"].Font.Size = 8;
                workbook.Worksheets.ActiveWorksheet.Cells["AM5"].Alignment.WrapText = true;
                //
                workbook.Worksheets.ActiveWorksheet.Range["AN5"].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                workbook.Worksheets.ActiveWorksheet.Cells["AN5"].Value = "%";
                workbook.Worksheets.ActiveWorksheet.Cells["AN5"].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                workbook.Worksheets.ActiveWorksheet.Cells["AN5"].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                workbook.Worksheets.ActiveWorksheet.Cells["AN5"].Font.Size = 10;
                workbook.Worksheets.ActiveWorksheet.Cells["AN5"].Alignment.WrapText = true;

                #endregion

                
                //
                //1차 그룹 핑 설비
                var results_X = _items.GroupBy(p => p.MCHN_NO, p => p.ITM_CD, (key, value) => new { MCHN_NO = key, ITM_CD = value });
                #region 내용 단

                //생산 계획
                int nPROD_PLN_QTY = 0;
                int nTOTAL_PROD_PLN_QTY = 0;

                //발주량
                int nSL_PLN_QTY = 0;

                //int nRowContents = 0;
                IList< ManVo> searchItems;
                int nResult_Y = 0;
                int Y = 6;
                //IList<ManVo> searchItems;
                foreach (var item_X in results_X)
                {
                    //2차 그룹 핑 설비
                    var results_Y = _items.Where<ManVo>(w => w.MCHN_NO.Equals(item_X.MCHN_NO)).GroupBy(p => p.MCHN_NO, p => p.MCHN_NM, (key, value) => new { MCHN_NM = key, MCHN_NO = value });
                    
                    nResult_Y = results_Y.Count();
                    //설비명
                    workbook.Worksheets.ActiveWorksheet.MergeCells(workbook.Worksheets.ActiveWorksheet.Range["B" + (Y) + ":B" + ((Y) + nResult_Y)]);
                    workbook.Worksheets.ActiveWorksheet.Range["B" + (Y) + ":B" + ((Y) + nResult_Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                    ////
                    workbook.Worksheets.ActiveWorksheet.Cells["B" + (Y)].Value = item_X.MCHN_NO;
                    workbook.Worksheets.ActiveWorksheet.Cells["B" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                    workbook.Worksheets.ActiveWorksheet.Cells["B" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                    workbook.Worksheets.ActiveWorksheet.Cells["B" + (Y)].Font.Size = 11;
                    workbook.Worksheets.ActiveWorksheet.Cells["B" + (Y)].Alignment.WrapText = true;
                    workbook.Worksheets.ActiveWorksheet.Cells["B" + (Y)].ColumnWidth = 180;


                    nPROD_PLN_QTY = 0;
                    //차종, //발주량, //고객사 재고, //재고, //생산 계획
                    foreach (var item_Y in results_Y)
                    {
                        //차종
                        workbook.Worksheets.ActiveWorksheet.Range["C" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                        //
                        workbook.Worksheets.ActiveWorksheet.Cells["C" + (Y)].Value = "";
                        workbook.Worksheets.ActiveWorksheet.Cells["C" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                        workbook.Worksheets.ActiveWorksheet.Cells["C" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                        workbook.Worksheets.ActiveWorksheet.Cells["C" + (Y)].Font.Size = 10;
                        workbook.Worksheets.ActiveWorksheet.Cells["C" + (Y)].ColumnWidth = 300;
                        workbook.Worksheets.ActiveWorksheet.Cells["C" + (Y)].Alignment.WrapText = true;


                        //검색 - [//발주량, //고객사 재고, //재고, //생산 계획]  //&& w.MCHN_NM.Equals(item_Y.MCHN_NM)
                        searchItems = _items.Where<ManVo>(w => w.MCHN_NO.Equals(item_X.MCHN_NO) ).ToList<ManVo>();
                        if (searchItems.Count > 0)
                        {
                            //
                            //발주량
                            workbook.Worksheets.ActiveWorksheet.Range["D" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                            //
                            workbook.Worksheets.ActiveWorksheet.Cells["D" + (Y)].Value = (Convert.ToInt32(searchItems[0].SL_PLN_QTY));
                            workbook.Worksheets.ActiveWorksheet.Cells["D" + (Y)].NumberFormat = "###,###";
                            workbook.Worksheets.ActiveWorksheet.Cells["D" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                            workbook.Worksheets.ActiveWorksheet.Cells["D" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                            workbook.Worksheets.ActiveWorksheet.Cells["D" + (Y)].Font.Size = 6;
                            //workbook.Worksheets.ActiveWorksheet.Cells["D" + (Y)].ColumnWidth = 300;
                            workbook.Worksheets.ActiveWorksheet.Cells["D" + (Y)].Alignment.WrapText = true;
                            nSL_PLN_QTY += Convert.ToInt32(searchItems[0].SL_PLN_QTY);

                            //
                            //고객사 재고
                            workbook.Worksheets.ActiveWorksheet.Range["E" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                            //
                            workbook.Worksheets.ActiveWorksheet.Cells["E" + (Y)].Value = "";
                            workbook.Worksheets.ActiveWorksheet.Cells["E" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                            workbook.Worksheets.ActiveWorksheet.Cells["E" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                            workbook.Worksheets.ActiveWorksheet.Cells["E" + (Y)].Font.Size = 6;
                            //workbook.Worksheets.ActiveWorksheet.Cells["E" + (Y)].ColumnWidth = 300;
                            workbook.Worksheets.ActiveWorksheet.Cells["E" + (Y)].Alignment.WrapText = true;

                            //
                            //재고
                            workbook.Worksheets.ActiveWorksheet.Range["F" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                            //
                            workbook.Worksheets.ActiveWorksheet.Cells["F" + (Y)].Value = (Convert.ToInt32(searchItems[0].N1ST_ITM_STK_QTY));
                            workbook.Worksheets.ActiveWorksheet.Cells["F" + (Y)].NumberFormat = "###,###";
                            workbook.Worksheets.ActiveWorksheet.Cells["F" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                            workbook.Worksheets.ActiveWorksheet.Cells["F" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                            workbook.Worksheets.ActiveWorksheet.Cells["F" + (Y)].Font.Size = 6;
                            //workbook.Worksheets.ActiveWorksheet.Cells["F" + (Y)].ColumnWidth = 300;
                            workbook.Worksheets.ActiveWorksheet.Cells["F" + (Y)].Alignment.WrapText = true;

                            //
                            //생산 계획
                            workbook.Worksheets.ActiveWorksheet.Range["G" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                            //
                            workbook.Worksheets.ActiveWorksheet.Cells["G" + (Y)].Value = (searchItems.Sum(s => Convert.ToInt32(s.PROD_PLN_QTY)));
                            workbook.Worksheets.ActiveWorksheet.Cells["G" + (Y)].NumberFormat = "###,###";
                            workbook.Worksheets.ActiveWorksheet.Cells["G" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                            workbook.Worksheets.ActiveWorksheet.Cells["G" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                            workbook.Worksheets.ActiveWorksheet.Cells["G" + (Y)].Font.Size = 6;
                            //workbook.Worksheets.ActiveWorksheet.Cells["G" + (Y)].ColumnWidth = 300;
                            workbook.Worksheets.ActiveWorksheet.Cells["G" + (Y)].Alignment.WrapText = true;
                            nPROD_PLN_QTY += searchItems.Sum(s => Convert.ToInt32(s.PROD_PLN_QTY));


                            #region 1~31 [초기 세팅] 
                            //1
                            workbook.Worksheets.ActiveWorksheet.Range["H" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                            //
                            //2
                            workbook.Worksheets.ActiveWorksheet.Range["I" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                            //
                            //3
                            workbook.Worksheets.ActiveWorksheet.Range["J" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                            //
                            //4
                            workbook.Worksheets.ActiveWorksheet.Range["K" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                            //
                            //5
                            workbook.Worksheets.ActiveWorksheet.Range["L" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                            //
                            //6
                            workbook.Worksheets.ActiveWorksheet.Range["M" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                            //
                            //7
                            workbook.Worksheets.ActiveWorksheet.Range["N" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                            //
                            //8
                            workbook.Worksheets.ActiveWorksheet.Range["O" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                            //
                            //9
                            workbook.Worksheets.ActiveWorksheet.Range["P" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                            //
                            //10
                            workbook.Worksheets.ActiveWorksheet.Range["Q" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                            //
                            //11
                            workbook.Worksheets.ActiveWorksheet.Range["R" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                            //
                            //12
                            workbook.Worksheets.ActiveWorksheet.Range["S" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                            //
                            //13
                            workbook.Worksheets.ActiveWorksheet.Range["T" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                            //
                            //
                            //14
                            workbook.Worksheets.ActiveWorksheet.Range["U" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                            //
                            //15
                            workbook.Worksheets.ActiveWorksheet.Range["V" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                            //
                            //16
                            workbook.Worksheets.ActiveWorksheet.Range["W" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                            //
                            //17
                            workbook.Worksheets.ActiveWorksheet.Range["X" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                            //
                            //18
                            workbook.Worksheets.ActiveWorksheet.Range["Y" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                            //
                            //19
                            workbook.Worksheets.ActiveWorksheet.Range["Z" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                            //
                            //20
                            workbook.Worksheets.ActiveWorksheet.Range["AA" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                            //
                            //21
                            workbook.Worksheets.ActiveWorksheet.Range["AB" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                            //
                            //22
                            workbook.Worksheets.ActiveWorksheet.Range["AC" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                            //
                            //23
                            workbook.Worksheets.ActiveWorksheet.Range["AD" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                            //
                            //24
                            workbook.Worksheets.ActiveWorksheet.Range["AE" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                            //
                            //25
                            workbook.Worksheets.ActiveWorksheet.Range["AF" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                            //
                            //26
                            workbook.Worksheets.ActiveWorksheet.Range["AG" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                            //
                            //27
                            workbook.Worksheets.ActiveWorksheet.Range["AH" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                            //
                            //28
                            workbook.Worksheets.ActiveWorksheet.Range["AI" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                            //
                            //29
                            workbook.Worksheets.ActiveWorksheet.Range["AJ" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                            //
                            //30
                            workbook.Worksheets.ActiveWorksheet.Range["AK" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                            //
                            //31
                            workbook.Worksheets.ActiveWorksheet.Range["AL" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                            //
                            //실적현황 - 수량
                            workbook.Worksheets.ActiveWorksheet.Range["AM" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                            //
                            //실적현황 - %
                            workbook.Worksheets.ActiveWorksheet.Range["AN" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);

                            #endregion
                            //일일 표시
                            foreach (ManVo item in searchItems)
                            {
                                //1~31
                                #region 1~31
                                switch (Convert.ToDateTime(item.PROD_PLN_DT).Day)
                                {
                                    case 1:
                                        //1
                                        workbook.Worksheets.ActiveWorksheet.Range["H" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                                        //
                                        workbook.Worksheets.ActiveWorksheet.Cells["H" + (Y)].Value = Convert.ToInt32(item.PROD_PLN_QTY)/*.ToString("###,###")*/;
                                        workbook.Worksheets.ActiveWorksheet.Cells["H" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                                        workbook.Worksheets.ActiveWorksheet.Cells["H" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                                        workbook.Worksheets.ActiveWorksheet.Cells["H" + (Y)].Font.Size = 6;
                                        workbook.Worksheets.ActiveWorksheet.Cells["H" + (Y)].NumberFormat = "###,###";
                                        //workbook.Worksheets.ActiveWorksheet.Cells["H" + (Y)].ColumnWidth = 300;
                                        workbook.Worksheets.ActiveWorksheet.Cells["H" + (Y)].Alignment.WrapText = true;
                                        break;

                                    case 2:
                                        //2
                                        workbook.Worksheets.ActiveWorksheet.Range["I" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                                        //
                                        workbook.Worksheets.ActiveWorksheet.Cells["I" + (Y)].Value = Convert.ToInt32(item.PROD_PLN_QTY)/*.ToString("###,###")*/;
                                        workbook.Worksheets.ActiveWorksheet.Cells["I" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                                        workbook.Worksheets.ActiveWorksheet.Cells["I" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                                        workbook.Worksheets.ActiveWorksheet.Cells["I" + (Y)].Font.Size = 6;
                                        workbook.Worksheets.ActiveWorksheet.Cells["I" + (Y)].NumberFormat = "###,###";
                                        //workbook.Worksheets.ActiveWorksheet.Cells["I" + (Y)].ColumnWidth = 300;
                                        workbook.Worksheets.ActiveWorksheet.Cells["I" + (Y)].Alignment.WrapText = true;
                                        break;

                                    case 3:
                                        //3
                                        workbook.Worksheets.ActiveWorksheet.Range["J" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                                        //
                                        workbook.Worksheets.ActiveWorksheet.Cells["J" + (Y)].Value = Convert.ToInt32(item.PROD_PLN_QTY)/*.ToString("###,###")*/;
                                        workbook.Worksheets.ActiveWorksheet.Cells["J" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                                        workbook.Worksheets.ActiveWorksheet.Cells["J" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                                        workbook.Worksheets.ActiveWorksheet.Cells["J" + (Y)].Font.Size = 6;
                                        workbook.Worksheets.ActiveWorksheet.Cells["J" + (Y)].NumberFormat = "###,###";
                                        //workbook.Worksheets.ActiveWorksheet.Cells["J" + (Y)].ColumnWidth = 300;
                                        workbook.Worksheets.ActiveWorksheet.Cells["J" + (Y)].Alignment.WrapText = true;
                                        break;

                                    case 4:
                                        //4
                                        workbook.Worksheets.ActiveWorksheet.Range["K" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                                        //
                                        workbook.Worksheets.ActiveWorksheet.Cells["K" + (Y)].Value = Convert.ToInt32(item.PROD_PLN_QTY)/*.ToString("###,###")*/;
                                        workbook.Worksheets.ActiveWorksheet.Cells["K" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                                        workbook.Worksheets.ActiveWorksheet.Cells["K" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                                        workbook.Worksheets.ActiveWorksheet.Cells["K" + (Y)].Font.Size = 6;
                                        workbook.Worksheets.ActiveWorksheet.Cells["K" + (Y)].NumberFormat = "###,###";
                                        //workbook.Worksheets.ActiveWorksheet.Cells["K" + (Y)].ColumnWidth = 300;
                                        workbook.Worksheets.ActiveWorksheet.Cells["K" + (Y)].Alignment.WrapText = true;
                                        break;

                                    case 5:
                                        //5
                                        workbook.Worksheets.ActiveWorksheet.Range["L" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                                        //
                                        workbook.Worksheets.ActiveWorksheet.Cells["L" + (Y)].Value = Convert.ToInt32(item.PROD_PLN_QTY)/*.ToString("###,###")*/;
                                        workbook.Worksheets.ActiveWorksheet.Cells["L" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                                        workbook.Worksheets.ActiveWorksheet.Cells["L" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                                        workbook.Worksheets.ActiveWorksheet.Cells["L" + (Y)].Font.Size = 6;
                                        workbook.Worksheets.ActiveWorksheet.Cells["L" + (Y)].NumberFormat = "###,###";
                                        //workbook.Worksheets.ActiveWorksheet.Cells["L" + (Y)].ColumnWidth = 300;
                                        workbook.Worksheets.ActiveWorksheet.Cells["L" + (Y)].Alignment.WrapText = true;
                                        break;

                                    case 6:
                                        //6
                                        workbook.Worksheets.ActiveWorksheet.Range["M" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                                        //
                                        workbook.Worksheets.ActiveWorksheet.Cells["M" + (Y)].Value = Convert.ToInt32(item.PROD_PLN_QTY)/*.ToString("###,###")*/;
                                        workbook.Worksheets.ActiveWorksheet.Cells["M" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                                        workbook.Worksheets.ActiveWorksheet.Cells["M" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                                        workbook.Worksheets.ActiveWorksheet.Cells["M" + (Y)].Font.Size = 6;
                                        workbook.Worksheets.ActiveWorksheet.Cells["M" + (Y)].NumberFormat = "###,###";
                                        //workbook.Worksheets.ActiveWorksheet.Cells["M" + (Y)].ColumnWidth = 300;
                                        workbook.Worksheets.ActiveWorksheet.Cells["M" + (Y)].Alignment.WrapText = true;
                                        break;

                                    case 7:
                                        //7
                                        workbook.Worksheets.ActiveWorksheet.Range["N" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                                        //
                                        workbook.Worksheets.ActiveWorksheet.Cells["N" + (Y)].Value = Convert.ToInt32(item.PROD_PLN_QTY)/*.ToString("###,###")*/;
                                        workbook.Worksheets.ActiveWorksheet.Cells["N" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                                        workbook.Worksheets.ActiveWorksheet.Cells["N" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                                        workbook.Worksheets.ActiveWorksheet.Cells["N" + (Y)].Font.Size = 6;
                                        workbook.Worksheets.ActiveWorksheet.Cells["N" + (Y)].NumberFormat = "###,###";
                                        //workbook.Worksheets.ActiveWorksheet.Cells["N" + (Y)].ColumnWidth = 300;
                                        workbook.Worksheets.ActiveWorksheet.Cells["N" + (Y)].Alignment.WrapText = true;
                                        break;

                                    case 8:
                                        //8
                                        workbook.Worksheets.ActiveWorksheet.Range["O" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                                        //
                                        workbook.Worksheets.ActiveWorksheet.Cells["O" + (Y)].Value = Convert.ToInt32(item.PROD_PLN_QTY)/*.ToString("###,###")*/;
                                        workbook.Worksheets.ActiveWorksheet.Cells["O" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                                        workbook.Worksheets.ActiveWorksheet.Cells["O" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                                        workbook.Worksheets.ActiveWorksheet.Cells["O" + (Y)].Font.Size = 6;
                                        workbook.Worksheets.ActiveWorksheet.Cells["O" + (Y)].NumberFormat = "###,###";
                                        //workbook.Worksheets.ActiveWorksheet.Cells["O" + (Y)].ColumnWidth = 300;
                                        workbook.Worksheets.ActiveWorksheet.Cells["O" + (Y)].Alignment.WrapText = true;
                                        break;

                                    case 9:
                                        //9
                                        workbook.Worksheets.ActiveWorksheet.Range["P" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                                        //
                                        workbook.Worksheets.ActiveWorksheet.Cells["P" + (Y)].Value = Convert.ToInt32(item.PROD_PLN_QTY)/*.ToString("###,###")*/;
                                        workbook.Worksheets.ActiveWorksheet.Cells["P" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                                        workbook.Worksheets.ActiveWorksheet.Cells["P" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                                        workbook.Worksheets.ActiveWorksheet.Cells["P" + (Y)].Font.Size = 6;
                                        workbook.Worksheets.ActiveWorksheet.Cells["P" + (Y)].NumberFormat = "###,###";
                                        //workbook.Worksheets.ActiveWorksheet.Cells["P" + (Y)].ColumnWidth = 300;
                                        workbook.Worksheets.ActiveWorksheet.Cells["P" + (Y)].Alignment.WrapText = true;
                                        break;

                                    case 10:
                                        //10
                                        workbook.Worksheets.ActiveWorksheet.Range["Q" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                                        //
                                        workbook.Worksheets.ActiveWorksheet.Cells["Q" + (Y)].Value = Convert.ToInt32(item.PROD_PLN_QTY)/*.ToString("###,###")*/;
                                        workbook.Worksheets.ActiveWorksheet.Cells["Q" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                                        workbook.Worksheets.ActiveWorksheet.Cells["Q" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                                        workbook.Worksheets.ActiveWorksheet.Cells["Q" + (Y)].Font.Size = 6;
                                        workbook.Worksheets.ActiveWorksheet.Cells["Q" + (Y)].NumberFormat = "###,###";
                                        //workbook.Worksheets.ActiveWorksheet.Cells["Q" + (Y)].ColumnWidth = 300;
                                        workbook.Worksheets.ActiveWorksheet.Cells["Q" + (Y)].Alignment.WrapText = true;
                                        break;

                                    case 11:
                                        //11
                                        workbook.Worksheets.ActiveWorksheet.Range["R" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                                        //
                                        workbook.Worksheets.ActiveWorksheet.Cells["R" + (Y)].Value = Convert.ToInt32(item.PROD_PLN_QTY)/*.ToString("###,###")*/;
                                        workbook.Worksheets.ActiveWorksheet.Cells["R" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                                        workbook.Worksheets.ActiveWorksheet.Cells["R" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                                        workbook.Worksheets.ActiveWorksheet.Cells["R" + (Y)].Font.Size = 6;
                                        workbook.Worksheets.ActiveWorksheet.Cells["R" + (Y)].NumberFormat = "###,###";
                                        //workbook.Worksheets.ActiveWorksheet.Cells["R" + (Y)].ColumnWidth = 300;
                                        workbook.Worksheets.ActiveWorksheet.Cells["R" + (Y)].Alignment.WrapText = true;
                                        break;

                                    case 12:
                                        //12
                                        workbook.Worksheets.ActiveWorksheet.Range["S" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                                        //
                                        workbook.Worksheets.ActiveWorksheet.Cells["S" + (Y)].Value = Convert.ToInt32(item.PROD_PLN_QTY)/*.ToString("###,###")*/;
                                        workbook.Worksheets.ActiveWorksheet.Cells["S" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                                        workbook.Worksheets.ActiveWorksheet.Cells["S" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                                        workbook.Worksheets.ActiveWorksheet.Cells["S" + (Y)].Font.Size = 6;
                                        workbook.Worksheets.ActiveWorksheet.Cells["S" + (Y)].NumberFormat = "###,###";
                                        //workbook.Worksheets.ActiveWorksheet.Cells["S" + (Y)].ColumnWidth = 300;
                                        workbook.Worksheets.ActiveWorksheet.Cells["S" + (Y)].Alignment.WrapText = true;
                                        break;

                                    case 13:
                                        //13
                                        workbook.Worksheets.ActiveWorksheet.Range["T" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                                        //
                                        workbook.Worksheets.ActiveWorksheet.Cells["T" + (Y)].Value = Convert.ToInt32(item.PROD_PLN_QTY)/*.ToString("###,###")*/;
                                        workbook.Worksheets.ActiveWorksheet.Cells["T" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                                        workbook.Worksheets.ActiveWorksheet.Cells["T" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                                        workbook.Worksheets.ActiveWorksheet.Cells["T" + (Y)].Font.Size = 6;
                                        workbook.Worksheets.ActiveWorksheet.Cells["T" + (Y)].NumberFormat = "###,###";
                                        //workbook.Worksheets.ActiveWorksheet.Cells["T" + (Y)].ColumnWidth = 300;
                                        workbook.Worksheets.ActiveWorksheet.Cells["T" + (Y)].Alignment.WrapText = true;
                                        break;

                                    case 14:
                                        //14
                                        workbook.Worksheets.ActiveWorksheet.Range["U" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                                        //
                                        workbook.Worksheets.ActiveWorksheet.Cells["U" + (Y)].Value = Convert.ToInt32(item.PROD_PLN_QTY)/*.ToString("###,###")*/;
                                        workbook.Worksheets.ActiveWorksheet.Cells["U" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                                        workbook.Worksheets.ActiveWorksheet.Cells["U" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                                        workbook.Worksheets.ActiveWorksheet.Cells["U" + (Y)].Font.Size = 6;
                                        workbook.Worksheets.ActiveWorksheet.Cells["U" + (Y)].NumberFormat = "###,###";
                                        //workbook.Worksheets.ActiveWorksheet.Cells["U" + (Y)].ColumnWidth = 300;
                                        workbook.Worksheets.ActiveWorksheet.Cells["U" + (Y)].Alignment.WrapText = true;
                                        break;

                                    case 15:
                                        //15
                                        workbook.Worksheets.ActiveWorksheet.Range["V" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                                        //
                                        workbook.Worksheets.ActiveWorksheet.Cells["V" + (Y)].Value = Convert.ToInt32(item.PROD_PLN_QTY)/*.ToString("###,###")*/;
                                        workbook.Worksheets.ActiveWorksheet.Cells["V" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                                        workbook.Worksheets.ActiveWorksheet.Cells["V" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                                        workbook.Worksheets.ActiveWorksheet.Cells["V" + (Y)].Font.Size = 6;
                                        workbook.Worksheets.ActiveWorksheet.Cells["V" + (Y)].NumberFormat = "###,###";
                                        //workbook.Worksheets.ActiveWorksheet.Cells["V" + (Y)].ColumnWidth = 300;
                                        workbook.Worksheets.ActiveWorksheet.Cells["V" + (Y)].Alignment.WrapText = true;
                                        break;

                                    case 16:
                                        //16
                                        workbook.Worksheets.ActiveWorksheet.Range["W" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                                        //
                                        workbook.Worksheets.ActiveWorksheet.Cells["W" + (Y)].Value = Convert.ToInt32(item.PROD_PLN_QTY)/*.ToString("###,###")*/;
                                        workbook.Worksheets.ActiveWorksheet.Cells["W" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                                        workbook.Worksheets.ActiveWorksheet.Cells["W" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                                        workbook.Worksheets.ActiveWorksheet.Cells["W" + (Y)].Font.Size = 6;
                                        workbook.Worksheets.ActiveWorksheet.Cells["W" + (Y)].NumberFormat = "###,###";
                                        //workbook.Worksheets.ActiveWorksheet.Cells["W" + (Y)].ColumnWidth = 300;
                                        workbook.Worksheets.ActiveWorksheet.Cells["W" + (Y)].Alignment.WrapText = true;
                                        break;

                                    case 17:
                                        //17
                                        workbook.Worksheets.ActiveWorksheet.Range["X" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                                        //
                                        workbook.Worksheets.ActiveWorksheet.Cells["X" + (Y)].Value = Convert.ToInt32(item.PROD_PLN_QTY)/*.ToString("###,###")*/;
                                        workbook.Worksheets.ActiveWorksheet.Cells["X" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                                        workbook.Worksheets.ActiveWorksheet.Cells["X" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                                        workbook.Worksheets.ActiveWorksheet.Cells["X" + (Y)].Font.Size = 6;
                                        workbook.Worksheets.ActiveWorksheet.Cells["X" + (Y)].NumberFormat = "###,###";
                                        //workbook.Worksheets.ActiveWorksheet.Cells["X" + (Y)].ColumnWidth = 300;
                                        workbook.Worksheets.ActiveWorksheet.Cells["X" + (Y)].Alignment.WrapText = true;
                                        break;

                                    case 18:
                                        //18
                                        workbook.Worksheets.ActiveWorksheet.Range["Y" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                                        //
                                        workbook.Worksheets.ActiveWorksheet.Cells["Y" + (Y)].Value = Convert.ToInt32(item.PROD_PLN_QTY)/*.ToString("###,###")*/;
                                        workbook.Worksheets.ActiveWorksheet.Cells["Y" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                                        workbook.Worksheets.ActiveWorksheet.Cells["Y" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                                        workbook.Worksheets.ActiveWorksheet.Cells["Y" + (Y)].Font.Size = 6;
                                        workbook.Worksheets.ActiveWorksheet.Cells["Y" + (Y)].NumberFormat = "###,###";
                                        //workbook.Worksheets.ActiveWorksheet.Cells["Y" + (Y)].ColumnWidth = 300;
                                        workbook.Worksheets.ActiveWorksheet.Cells["Y" + (Y)].Alignment.WrapText = true;
                                        break;

                                    case 19:
                                        //19
                                        workbook.Worksheets.ActiveWorksheet.Range["Z" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                                        //
                                        workbook.Worksheets.ActiveWorksheet.Cells["Z" + (Y)].Value = Convert.ToInt32(item.PROD_PLN_QTY)/*.ToString("###,###")*/;
                                        workbook.Worksheets.ActiveWorksheet.Cells["Z" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                                        workbook.Worksheets.ActiveWorksheet.Cells["Z" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                                        workbook.Worksheets.ActiveWorksheet.Cells["Z" + (Y)].Font.Size = 6;
                                        workbook.Worksheets.ActiveWorksheet.Cells["Z" + (Y)].NumberFormat = "###,###";
                                        //workbook.Worksheets.ActiveWorksheet.Cells["Z" + (Y)].ColumnWidth = 300;
                                        workbook.Worksheets.ActiveWorksheet.Cells["Z" + (Y)].Alignment.WrapText = true;
                                        break;

                                    case 20:
                                        //20
                                        workbook.Worksheets.ActiveWorksheet.Range["AA" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                                        //
                                        workbook.Worksheets.ActiveWorksheet.Cells["AA" + (Y)].Value = Convert.ToInt32(item.PROD_PLN_QTY)/*.ToString("###,###")*/;
                                        workbook.Worksheets.ActiveWorksheet.Cells["AA" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                                        workbook.Worksheets.ActiveWorksheet.Cells["AA" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                                        workbook.Worksheets.ActiveWorksheet.Cells["AA" + (Y)].Font.Size = 6;
                                        workbook.Worksheets.ActiveWorksheet.Cells["AA" + (Y)].NumberFormat = "###,###";
                                        //workbook.Worksheets.ActiveWorksheet.Cells["AA" + (Y)].ColumnWidth = 300;
                                        workbook.Worksheets.ActiveWorksheet.Cells["AA" + (Y)].Alignment.WrapText = true;
                                        break;

                                    case 21:
                                        //21
                                        workbook.Worksheets.ActiveWorksheet.Range["AB" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                                        //
                                        workbook.Worksheets.ActiveWorksheet.Cells["AB" + (Y)].Value = Convert.ToInt32(item.PROD_PLN_QTY)/*.ToString("###,###")*/;
                                        workbook.Worksheets.ActiveWorksheet.Cells["AB" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                                        workbook.Worksheets.ActiveWorksheet.Cells["AB" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                                        workbook.Worksheets.ActiveWorksheet.Cells["AB" + (Y)].Font.Size = 6;
                                        workbook.Worksheets.ActiveWorksheet.Cells["AB" + (Y)].NumberFormat = "###,###";
                                        //workbook.Worksheets.ActiveWorksheet.Cells["G" + (Y)].ColumnWidth = 300;
                                        workbook.Worksheets.ActiveWorksheet.Cells["AB" + (Y)].Alignment.WrapText = true;
                                        break;

                                    case 22:
                                        //22
                                        workbook.Worksheets.ActiveWorksheet.Range["AC" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                                        //
                                        workbook.Worksheets.ActiveWorksheet.Cells["AC" + (Y)].Value = Convert.ToInt32(item.PROD_PLN_QTY)/*.ToString("###,###")*/;
                                        workbook.Worksheets.ActiveWorksheet.Cells["AC" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                                        workbook.Worksheets.ActiveWorksheet.Cells["AC" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                                        workbook.Worksheets.ActiveWorksheet.Cells["AC" + (Y)].Font.Size = 6;
                                        workbook.Worksheets.ActiveWorksheet.Cells["AC" + (Y)].NumberFormat = "###,###";
                                        //workbook.Worksheets.ActiveWorksheet.Cells["AC" + (Y)].ColumnWidth = 300;
                                        workbook.Worksheets.ActiveWorksheet.Cells["AC" + (Y)].Alignment.WrapText = true;
                                        break;

                                    case 23:
                                        //23
                                        workbook.Worksheets.ActiveWorksheet.Range["AD" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                                        //
                                        workbook.Worksheets.ActiveWorksheet.Cells["AD" + (Y)].Value = Convert.ToInt32(item.PROD_PLN_QTY)/*.ToString("###,###")*/;
                                        workbook.Worksheets.ActiveWorksheet.Cells["AD" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                                        workbook.Worksheets.ActiveWorksheet.Cells["AD" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                                        workbook.Worksheets.ActiveWorksheet.Cells["AD" + (Y)].Font.Size = 6;
                                        workbook.Worksheets.ActiveWorksheet.Cells["AD" + (Y)].NumberFormat = "###,###";
                                        //workbook.Worksheets.ActiveWorksheet.Cells["AD" + (Y)].ColumnWidth = 300;
                                        workbook.Worksheets.ActiveWorksheet.Cells["AD" + (Y)].Alignment.WrapText = true;
                                        break;
                                        
                                    case 24:
                                        //24
                                        workbook.Worksheets.ActiveWorksheet.Range["AE" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                                        //
                                        workbook.Worksheets.ActiveWorksheet.Cells["AE" + (Y)].Value = Convert.ToInt32(item.PROD_PLN_QTY)/*.ToString("###,###")*/;
                                        workbook.Worksheets.ActiveWorksheet.Cells["AE" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                                        workbook.Worksheets.ActiveWorksheet.Cells["AE" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                                        workbook.Worksheets.ActiveWorksheet.Cells["AE" + (Y)].Font.Size = 6;
                                        workbook.Worksheets.ActiveWorksheet.Cells["AE" + (Y)].NumberFormat = "###,###";
                                        //workbook.Worksheets.ActiveWorksheet.Cells["AE" + (Y)].ColumnWidth = 300;
                                        workbook.Worksheets.ActiveWorksheet.Cells["AE" + (Y)].Alignment.WrapText = true;
                                        break;

                                    case 25:
                                        //25
                                        workbook.Worksheets.ActiveWorksheet.Range["AF" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                                        //
                                        workbook.Worksheets.ActiveWorksheet.Cells["AF" + (Y)].Value = Convert.ToInt32(item.PROD_PLN_QTY)/*.ToString("###,###")*/;
                                        workbook.Worksheets.ActiveWorksheet.Cells["AF" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                                        workbook.Worksheets.ActiveWorksheet.Cells["AF" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                                        workbook.Worksheets.ActiveWorksheet.Cells["AF" + (Y)].Font.Size = 6;
                                        workbook.Worksheets.ActiveWorksheet.Cells["AF" + (Y)].NumberFormat = "###,###";
                                        //workbook.Worksheets.ActiveWorksheet.Cells["AF" + (Y)].ColumnWidth = 300;
                                        workbook.Worksheets.ActiveWorksheet.Cells["AF" + (Y)].Alignment.WrapText = true;
                                        break;

                                    case 26:
                                        //26
                                        workbook.Worksheets.ActiveWorksheet.Range["AG" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                                        //
                                        workbook.Worksheets.ActiveWorksheet.Cells["AG" + (Y)].Value = Convert.ToInt32(item.PROD_PLN_QTY)/*.ToString("###,###")*/;
                                        workbook.Worksheets.ActiveWorksheet.Cells["AG" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                                        workbook.Worksheets.ActiveWorksheet.Cells["AG" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                                        workbook.Worksheets.ActiveWorksheet.Cells["AG" + (Y)].Font.Size = 6;
                                        workbook.Worksheets.ActiveWorksheet.Cells["AG" + (Y)].NumberFormat = "###,###";
                                        //workbook.Worksheets.ActiveWorksheet.Cells["AG" + (Y)].ColumnWidth = 300;
                                        workbook.Worksheets.ActiveWorksheet.Cells["AG" + (Y)].Alignment.WrapText = true;
                                        break;

                                    case 27:
                                        //27
                                        workbook.Worksheets.ActiveWorksheet.Range["AH" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                                        //
                                        workbook.Worksheets.ActiveWorksheet.Cells["AH" + (Y)].Value = Convert.ToInt32(item.PROD_PLN_QTY)/*.ToString("###,###")*/;
                                        workbook.Worksheets.ActiveWorksheet.Cells["AH" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                                        workbook.Worksheets.ActiveWorksheet.Cells["AH" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                                        workbook.Worksheets.ActiveWorksheet.Cells["AH" + (Y)].Font.Size = 6;
                                        workbook.Worksheets.ActiveWorksheet.Cells["AH" + (Y)].NumberFormat = "###,###";
                                        //workbook.Worksheets.ActiveWorksheet.Cells["AH" + (Y)].ColumnWidth = 300;
                                        workbook.Worksheets.ActiveWorksheet.Cells["AH" + (Y)].Alignment.WrapText = true;
                                        break;

                                    case 28:
                                        //28
                                        workbook.Worksheets.ActiveWorksheet.Range["AI" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                                        //
                                        workbook.Worksheets.ActiveWorksheet.Cells["AI" + (Y)].Value = Convert.ToInt32(item.PROD_PLN_QTY)/*.ToString("###,###")*/;
                                        workbook.Worksheets.ActiveWorksheet.Cells["AI" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                                        workbook.Worksheets.ActiveWorksheet.Cells["AI" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                                        workbook.Worksheets.ActiveWorksheet.Cells["AI" + (Y)].Font.Size = 6;
                                        workbook.Worksheets.ActiveWorksheet.Cells["AI" + (Y)].NumberFormat = "###,###";
                                        //workbook.Worksheets.ActiveWorksheet.Cells["AI" + (Y)].ColumnWidth = 300;
                                        workbook.Worksheets.ActiveWorksheet.Cells["AI" + (Y)].Alignment.WrapText = true;
                                        break;

                                    case 29:
                                        //29
                                        workbook.Worksheets.ActiveWorksheet.Range["AJ" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                                        //
                                        workbook.Worksheets.ActiveWorksheet.Cells["AJ" + (Y)].Value = Convert.ToInt32(item.PROD_PLN_QTY)/*.ToString("###,###")*/;
                                        workbook.Worksheets.ActiveWorksheet.Cells["AJ" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                                        workbook.Worksheets.ActiveWorksheet.Cells["AJ" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                                        workbook.Worksheets.ActiveWorksheet.Cells["AJ" + (Y)].Font.Size = 6;
                                        workbook.Worksheets.ActiveWorksheet.Cells["AJ" + (Y)].NumberFormat = "###,###";
                                        //workbook.Worksheets.ActiveWorksheet.Cells["AJ" + (Y)].ColumnWidth = 300;
                                        workbook.Worksheets.ActiveWorksheet.Cells["AJ" + (Y)].Alignment.WrapText = true;
                                        break;

                                    case 30:
                                        //30
                                        workbook.Worksheets.ActiveWorksheet.Range["AK" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                                        //
                                        workbook.Worksheets.ActiveWorksheet.Cells["AK" + (Y)].Value = Convert.ToInt32(item.PROD_PLN_QTY)/*.ToString("###,###")*/;
                                        workbook.Worksheets.ActiveWorksheet.Cells["AK" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                                        workbook.Worksheets.ActiveWorksheet.Cells["AK" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                                        workbook.Worksheets.ActiveWorksheet.Cells["AK" + (Y)].Font.Size = 6;
                                        workbook.Worksheets.ActiveWorksheet.Cells["AK" + (Y)].NumberFormat = "###,###";
                                        //workbook.Worksheets.ActiveWorksheet.Cells["AK" + (Y)].ColumnWidth = 300;
                                        workbook.Worksheets.ActiveWorksheet.Cells["AK" + (Y)].Alignment.WrapText = true;
                                        break;

                                    case 31:
                                        //31
                                        workbook.Worksheets.ActiveWorksheet.Range["AL" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                                        //
                                        workbook.Worksheets.ActiveWorksheet.Cells["AL" + (Y)].Value = Convert.ToInt32(item.PROD_PLN_QTY)/*.ToString("###,###")*/;
                                        workbook.Worksheets.ActiveWorksheet.Cells["AL" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                                        workbook.Worksheets.ActiveWorksheet.Cells["AL" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                                        workbook.Worksheets.ActiveWorksheet.Cells["AL" + (Y)].Font.Size = 6;
                                        workbook.Worksheets.ActiveWorksheet.Cells["AL" + (Y)].NumberFormat = "###,###";
                                        //workbook.Worksheets.ActiveWorksheet.Cells["AL" + (Y)].ColumnWidth = 300;
                                        workbook.Worksheets.ActiveWorksheet.Cells["AL" + (Y)].Alignment.WrapText = true;
                                        break;
                                    default:
                                        break;
                                }
                                #endregion
                            }
                            //실적 현황 - 합계
                            workbook.Worksheets.ActiveWorksheet.Range["AM" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                            //
                            //workbook.Worksheets.ActiveWorksheet.Cells["AM" + (Y)].Value = ("" + Convert.ToInt32(searchItems.Sum(s => Convert.ToInt32(s.PROD_PLN_QTY)).ToString("###,###")));
                            workbook.Worksheets.ActiveWorksheet.Cells["AM" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                            workbook.Worksheets.ActiveWorksheet.Cells["AM" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                            workbook.Worksheets.ActiveWorksheet.Cells["AM" + (Y)].Font.Size = 6;
                            workbook.Worksheets.ActiveWorksheet.Cells["AM" + (Y)].Formula = "=Sum(H" + (Y) + ":AL" + (Y) + ")";
                            workbook.Worksheets.ActiveWorksheet.Cells["AM" + (Y)].NumberFormat = "###,###";
                            //workbook.Worksheets.ActiveWorksheet.Cells["AM" + (Y)].ColumnWidth = 300;
                            workbook.Worksheets.ActiveWorksheet.Cells["AM" + (Y)].Alignment.WrapText = true;



                            //실적 현황 - %
                            workbook.Worksheets.ActiveWorksheet.Range["AN" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                            //
                            //workbook.Worksheets.ActiveWorksheet.Cells["AM" + (Y)].Value = ("" + Convert.ToInt32(searchItems.Sum(s => Convert.ToInt32(s.PROD_PLN_QTY)).ToString("###,###")));
                            workbook.Worksheets.ActiveWorksheet.Cells["AN" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                            workbook.Worksheets.ActiveWorksheet.Cells["AN" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                            workbook.Worksheets.ActiveWorksheet.Cells["AN" + (Y)].Font.Size = 8;
                            workbook.Worksheets.ActiveWorksheet.Cells["AN" + (Y)].Formula = "=(AM" + (Y) + "/D" + (Y) + ") * 100";
                            workbook.Worksheets.ActiveWorksheet.Cells["AN" + (Y)].NumberFormat = "###,###";
                            //workbook.Worksheets.ActiveWorksheet.Cells["AM" + (Y)].ColumnWidth = 300;
                            workbook.Worksheets.ActiveWorksheet.Cells["AN" + (Y)].Alignment.WrapText = true;
                        }

                        Y++; //6 +1
                        //nRowContents += results_Y.Count();
                        //
                    }

                    ////
                    ////차종 합계
                    //nRowContents++;
                    workbook.Worksheets.ActiveWorksheet.MergeCells(workbook.Worksheets.ActiveWorksheet.Range["C" + (Y) + ":F" + (Y)]);
                    workbook.Worksheets.ActiveWorksheet.Range["C" + (Y) + ":F" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                    //
                    workbook.Worksheets.ActiveWorksheet.Cells["C" + (Y)].Value = "실        적";
                    workbook.Worksheets.ActiveWorksheet.Cells["C" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                    workbook.Worksheets.ActiveWorksheet.Cells["C" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                    workbook.Worksheets.ActiveWorksheet.Cells["C" + (Y)].Font.Size = 9;
                    workbook.Worksheets.ActiveWorksheet.Cells["C" + (Y)].Alignment.WrapText = true;
                    workbook.Worksheets.ActiveWorksheet.Cells["C" + (Y)].Font.Bold = true;
                    workbook.Worksheets.ActiveWorksheet.Cells["C" + (Y)].FillColor = Color.LightGray;
                    //
                    //Total - Sum ***
                    //workbook.Worksheets.ActiveWorksheet.MergeCells(workbook.Worksheets.ActiveWorksheet.Range["C" + (Y) + ":F" + (Y)]);
                    workbook.Worksheets.ActiveWorksheet.Range["G" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                    //
                    workbook.Worksheets.ActiveWorksheet.Cells["G" + (Y)].Value = nPROD_PLN_QTY;
                    //workbook.Worksheets.ActiveWorksheet.Cells["G" + (Y)].Formula = "=Sum(G" + (Y) + ":G" + (Y) + ")";
                    workbook.Worksheets.ActiveWorksheet.Cells["G" + (Y)].NumberFormat = "###,###";
                    workbook.Worksheets.ActiveWorksheet.Cells["G" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                    workbook.Worksheets.ActiveWorksheet.Cells["G" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                    workbook.Worksheets.ActiveWorksheet.Cells["G" + (Y)].Font.Size = 9;
                    workbook.Worksheets.ActiveWorksheet.Cells["G" + (Y)].Alignment.WrapText = true;
                    workbook.Worksheets.ActiveWorksheet.Cells["G" + (Y)].Font.Bold = true;
                    workbook.Worksheets.ActiveWorksheet.Cells["G" + (Y)].FillColor = Color.LightGray;


                    nTOTAL_PROD_PLN_QTY += nPROD_PLN_QTY;

                    //1~31 [실 적]
                    #region 1~31 [실 적]
                    //1
                    workbook.Worksheets.ActiveWorksheet.Range["H" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                    //
                    workbook.Worksheets.ActiveWorksheet.Cells["H" + (Y)].Value = "";
                    workbook.Worksheets.ActiveWorksheet.Cells["H" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                    workbook.Worksheets.ActiveWorksheet.Cells["H" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                    workbook.Worksheets.ActiveWorksheet.Cells["H" + (Y)].Font.Size = 6;
                    workbook.Worksheets.ActiveWorksheet.Cells["H" + (Y)].NumberFormat = "###,###";
                    //workbook.Worksheets.ActiveWorksheet.Cells["H" + (Y)].ColumnWidth = 300;
                    workbook.Worksheets.ActiveWorksheet.Cells["H" + (Y)].Alignment.WrapText = true;
                    //
                    //2
                    workbook.Worksheets.ActiveWorksheet.Range["I" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                    //
                    workbook.Worksheets.ActiveWorksheet.Cells["I" + (Y)].Value = "";
                    workbook.Worksheets.ActiveWorksheet.Cells["I" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                    workbook.Worksheets.ActiveWorksheet.Cells["I" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                    workbook.Worksheets.ActiveWorksheet.Cells["I" + (Y)].Font.Size = 6;
                    workbook.Worksheets.ActiveWorksheet.Cells["I" + (Y)].NumberFormat = "###,###";
                    //workbook.Worksheets.ActiveWorksheet.Cells["I" + (Y)].ColumnWidth = 300;
                    workbook.Worksheets.ActiveWorksheet.Cells["I" + (Y)].Alignment.WrapText = true;
                    //
                    //3
                    workbook.Worksheets.ActiveWorksheet.Range["J" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                    //
                    workbook.Worksheets.ActiveWorksheet.Cells["J" + (Y)].Value = "";
                    workbook.Worksheets.ActiveWorksheet.Cells["J" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                    workbook.Worksheets.ActiveWorksheet.Cells["J" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                    workbook.Worksheets.ActiveWorksheet.Cells["J" + (Y)].Font.Size = 6;
                    workbook.Worksheets.ActiveWorksheet.Cells["J" + (Y)].NumberFormat = "###,###";
                    //workbook.Worksheets.ActiveWorksheet.Cells["J" + (Y)].ColumnWidth = 300;
                    workbook.Worksheets.ActiveWorksheet.Cells["J" + (Y)].Alignment.WrapText = true;
                    //
                    //4
                    workbook.Worksheets.ActiveWorksheet.Range["K" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                    //
                    workbook.Worksheets.ActiveWorksheet.Cells["K" + (Y)].Value = "";
                    workbook.Worksheets.ActiveWorksheet.Cells["K" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                    workbook.Worksheets.ActiveWorksheet.Cells["K" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                    workbook.Worksheets.ActiveWorksheet.Cells["K" + (Y)].Font.Size = 6;
                    workbook.Worksheets.ActiveWorksheet.Cells["K" + (Y)].NumberFormat = "###,###";
                    //workbook.Worksheets.ActiveWorksheet.Cells["K" + (Y)].ColumnWidth = 300;
                    workbook.Worksheets.ActiveWorksheet.Cells["K" + (Y)].Alignment.WrapText = true;
                    //
                    //5
                    workbook.Worksheets.ActiveWorksheet.Range["L" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                    //
                    workbook.Worksheets.ActiveWorksheet.Cells["L" + (Y)].Value = "";
                    workbook.Worksheets.ActiveWorksheet.Cells["L" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                    workbook.Worksheets.ActiveWorksheet.Cells["L" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                    workbook.Worksheets.ActiveWorksheet.Cells["L" + (Y)].Font.Size = 6;
                    workbook.Worksheets.ActiveWorksheet.Cells["L" + (Y)].NumberFormat = "###,###";
                    //workbook.Worksheets.ActiveWorksheet.Cells["L" + (Y)].ColumnWidth = 300;
                    workbook.Worksheets.ActiveWorksheet.Cells["L" + (Y)].Alignment.WrapText = true;
                    //
                    //6
                    workbook.Worksheets.ActiveWorksheet.Range["M" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                    //
                    workbook.Worksheets.ActiveWorksheet.Cells["M" + (Y)].Value = "";
                    workbook.Worksheets.ActiveWorksheet.Cells["M" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                    workbook.Worksheets.ActiveWorksheet.Cells["M" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                    workbook.Worksheets.ActiveWorksheet.Cells["M" + (Y)].Font.Size = 6;
                    workbook.Worksheets.ActiveWorksheet.Cells["M" + (Y)].NumberFormat = "###,###";
                    //workbook.Worksheets.ActiveWorksheet.Cells["M" + (Y)].ColumnWidth = 300;
                    workbook.Worksheets.ActiveWorksheet.Cells["M" + (Y)].Alignment.WrapText = true;
                    //
                    //7
                    workbook.Worksheets.ActiveWorksheet.Range["N" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                    //
                    workbook.Worksheets.ActiveWorksheet.Cells["N" + (Y)].Value = "";
                    workbook.Worksheets.ActiveWorksheet.Cells["N" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                    workbook.Worksheets.ActiveWorksheet.Cells["N" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                    workbook.Worksheets.ActiveWorksheet.Cells["N" + (Y)].Font.Size = 6;
                    workbook.Worksheets.ActiveWorksheet.Cells["N" + (Y)].NumberFormat = "###,###";
                    //workbook.Worksheets.ActiveWorksheet.Cells["N" + (Y)].ColumnWidth = 300;
                    workbook.Worksheets.ActiveWorksheet.Cells["N" + (Y)].Alignment.WrapText = true;
                    //
                    //8
                    workbook.Worksheets.ActiveWorksheet.Range["O" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                    //
                    workbook.Worksheets.ActiveWorksheet.Cells["O" + (Y)].Value = "";
                    workbook.Worksheets.ActiveWorksheet.Cells["O" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                    workbook.Worksheets.ActiveWorksheet.Cells["O" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                    workbook.Worksheets.ActiveWorksheet.Cells["O" + (Y)].Font.Size = 6;
                    workbook.Worksheets.ActiveWorksheet.Cells["O" + (Y)].NumberFormat = "###,###";
                    //workbook.Worksheets.ActiveWorksheet.Cells["O" + (Y)].ColumnWidth = 300;
                    workbook.Worksheets.ActiveWorksheet.Cells["O" + (Y)].Alignment.WrapText = true;
                    //
                    //9
                    workbook.Worksheets.ActiveWorksheet.Range["P" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                    //
                    workbook.Worksheets.ActiveWorksheet.Cells["P" + (Y)].Value = "";
                    workbook.Worksheets.ActiveWorksheet.Cells["P" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                    workbook.Worksheets.ActiveWorksheet.Cells["P" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                    workbook.Worksheets.ActiveWorksheet.Cells["P" + (Y)].Font.Size = 6;
                    workbook.Worksheets.ActiveWorksheet.Cells["P" + (Y)].NumberFormat = "###,###";
                    //workbook.Worksheets.ActiveWorksheet.Cells["P" + (Y)].ColumnWidth = 300;
                    workbook.Worksheets.ActiveWorksheet.Cells["P" + (Y)].Alignment.WrapText = true;
                    //
                    //10
                    workbook.Worksheets.ActiveWorksheet.Range["Q" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                    //
                    workbook.Worksheets.ActiveWorksheet.Cells["Q" + (Y)].Value = "";
                    workbook.Worksheets.ActiveWorksheet.Cells["Q" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                    workbook.Worksheets.ActiveWorksheet.Cells["Q" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                    workbook.Worksheets.ActiveWorksheet.Cells["Q" + (Y)].Font.Size = 6;
                    workbook.Worksheets.ActiveWorksheet.Cells["Q" + (Y)].NumberFormat = "###,###";
                    //workbook.Worksheets.ActiveWorksheet.Cells["Q" + (Y)].ColumnWidth = 300;
                    workbook.Worksheets.ActiveWorksheet.Cells["Q" + (Y)].Alignment.WrapText = true;
                    //
                    //11
                    workbook.Worksheets.ActiveWorksheet.Range["R" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                    //
                    workbook.Worksheets.ActiveWorksheet.Cells["R" + (Y)].Value = "";
                    workbook.Worksheets.ActiveWorksheet.Cells["R" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                    workbook.Worksheets.ActiveWorksheet.Cells["R" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                    workbook.Worksheets.ActiveWorksheet.Cells["R" + (Y)].Font.Size = 6;
                    workbook.Worksheets.ActiveWorksheet.Cells["R" + (Y)].NumberFormat = "###,###";
                    //workbook.Worksheets.ActiveWorksheet.Cells["R" + (Y)].ColumnWidth = 300;
                    workbook.Worksheets.ActiveWorksheet.Cells["R" + (Y)].Alignment.WrapText = true;
                    //
                    //12
                    workbook.Worksheets.ActiveWorksheet.Range["S" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                    //
                    workbook.Worksheets.ActiveWorksheet.Cells["S" + (Y)].Value = "";
                    workbook.Worksheets.ActiveWorksheet.Cells["S" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                    workbook.Worksheets.ActiveWorksheet.Cells["S" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                    workbook.Worksheets.ActiveWorksheet.Cells["S" + (Y)].Font.Size = 6;
                    workbook.Worksheets.ActiveWorksheet.Cells["S" + (Y)].NumberFormat = "###,###";
                    //workbook.Worksheets.ActiveWorksheet.Cells["S" + (Y)].ColumnWidth = 300;
                    workbook.Worksheets.ActiveWorksheet.Cells["S" + (Y)].Alignment.WrapText = true;
                    //
                    //13
                    workbook.Worksheets.ActiveWorksheet.Range["T" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                    //
                    workbook.Worksheets.ActiveWorksheet.Cells["T" + (Y)].Value = "";
                    workbook.Worksheets.ActiveWorksheet.Cells["T" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                    workbook.Worksheets.ActiveWorksheet.Cells["T" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                    workbook.Worksheets.ActiveWorksheet.Cells["T" + (Y)].Font.Size = 6;
                    workbook.Worksheets.ActiveWorksheet.Cells["T" + (Y)].NumberFormat = "###,###";
                    //workbook.Worksheets.ActiveWorksheet.Cells["T" + (Y)].ColumnWidth = 300;
                    workbook.Worksheets.ActiveWorksheet.Cells["T" + (Y)].Alignment.WrapText = true;
                    //
                    //14
                    workbook.Worksheets.ActiveWorksheet.Range["U" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                    //
                    workbook.Worksheets.ActiveWorksheet.Cells["U" + (Y)].Value = "";
                    workbook.Worksheets.ActiveWorksheet.Cells["U" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                    workbook.Worksheets.ActiveWorksheet.Cells["U" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                    workbook.Worksheets.ActiveWorksheet.Cells["U" + (Y)].Font.Size = 6;
                    workbook.Worksheets.ActiveWorksheet.Cells["U" + (Y)].NumberFormat = "###,###";
                    //workbook.Worksheets.ActiveWorksheet.Cells["U" + (Y)].ColumnWidth = 300;
                    workbook.Worksheets.ActiveWorksheet.Cells["U" + (Y)].Alignment.WrapText = true;
                    //
                    //15
                    workbook.Worksheets.ActiveWorksheet.Range["V" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                    //
                    workbook.Worksheets.ActiveWorksheet.Cells["V" + (Y)].Value = "";
                    workbook.Worksheets.ActiveWorksheet.Cells["V" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                    workbook.Worksheets.ActiveWorksheet.Cells["V" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                    workbook.Worksheets.ActiveWorksheet.Cells["V" + (Y)].Font.Size = 6;
                    workbook.Worksheets.ActiveWorksheet.Cells["V" + (Y)].NumberFormat = "###,###";
                    //workbook.Worksheets.ActiveWorksheet.Cells["V" + (Y)].ColumnWidth = 300;
                    workbook.Worksheets.ActiveWorksheet.Cells["V" + (Y)].Alignment.WrapText = true;
                    //
                    //16
                    workbook.Worksheets.ActiveWorksheet.Range["W" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                    //
                    workbook.Worksheets.ActiveWorksheet.Cells["W" + (Y)].Value = "";
                    workbook.Worksheets.ActiveWorksheet.Cells["W" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                    workbook.Worksheets.ActiveWorksheet.Cells["W" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                    workbook.Worksheets.ActiveWorksheet.Cells["W" + (Y)].Font.Size = 6;
                    workbook.Worksheets.ActiveWorksheet.Cells["W" + (Y)].NumberFormat = "###,###";
                    //workbook.Worksheets.ActiveWorksheet.Cells["W" + (Y)].ColumnWidth = 300;
                    workbook.Worksheets.ActiveWorksheet.Cells["W" + (Y)].Alignment.WrapText = true;
                    //
                    //17
                    workbook.Worksheets.ActiveWorksheet.Range["X" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                    //
                    workbook.Worksheets.ActiveWorksheet.Cells["X" + (Y)].Value = "";
                    workbook.Worksheets.ActiveWorksheet.Cells["X" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                    workbook.Worksheets.ActiveWorksheet.Cells["X" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                    workbook.Worksheets.ActiveWorksheet.Cells["X" + (Y)].Font.Size = 6;
                    workbook.Worksheets.ActiveWorksheet.Cells["X" + (Y)].NumberFormat = "###,###";
                    //workbook.Worksheets.ActiveWorksheet.Cells["X" + (Y)].ColumnWidth = 300;
                    workbook.Worksheets.ActiveWorksheet.Cells["X" + (Y)].Alignment.WrapText = true;
                    //
                    //18
                    workbook.Worksheets.ActiveWorksheet.Range["Y" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                    //
                    workbook.Worksheets.ActiveWorksheet.Cells["Y" + (Y)].Value = "";
                    workbook.Worksheets.ActiveWorksheet.Cells["Y" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                    workbook.Worksheets.ActiveWorksheet.Cells["Y" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                    workbook.Worksheets.ActiveWorksheet.Cells["Y" + (Y)].Font.Size = 6;
                    workbook.Worksheets.ActiveWorksheet.Cells["Y" + (Y)].NumberFormat = "###,###";
                    //workbook.Worksheets.ActiveWorksheet.Cells["Y" + (Y)].ColumnWidth = 300;
                    workbook.Worksheets.ActiveWorksheet.Cells["Y" + (Y)].Alignment.WrapText = true;
                    //
                    //19
                    workbook.Worksheets.ActiveWorksheet.Range["Z" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                    //
                    workbook.Worksheets.ActiveWorksheet.Cells["Z" + (Y)].Value = "";
                    workbook.Worksheets.ActiveWorksheet.Cells["Z" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                    workbook.Worksheets.ActiveWorksheet.Cells["Z" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                    workbook.Worksheets.ActiveWorksheet.Cells["Z" + (Y)].Font.Size = 6;
                    workbook.Worksheets.ActiveWorksheet.Cells["Z" + (Y)].NumberFormat = "###,###";
                    //workbook.Worksheets.ActiveWorksheet.Cells["Z" + (Y)].ColumnWidth = 300;
                    workbook.Worksheets.ActiveWorksheet.Cells["Z" + (Y)].Alignment.WrapText = true;
                    //
                    //20
                    workbook.Worksheets.ActiveWorksheet.Range["AA" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                    //
                    workbook.Worksheets.ActiveWorksheet.Cells["AA" + (Y)].Value = "";
                    workbook.Worksheets.ActiveWorksheet.Cells["AA" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                    workbook.Worksheets.ActiveWorksheet.Cells["AA" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                    workbook.Worksheets.ActiveWorksheet.Cells["AA" + (Y)].Font.Size = 6;
                    workbook.Worksheets.ActiveWorksheet.Cells["AA" + (Y)].NumberFormat = "###,###";
                    //workbook.Worksheets.ActiveWorksheet.Cells["AA" + (Y)].ColumnWidth = 300;
                    workbook.Worksheets.ActiveWorksheet.Cells["AA" + (Y)].Alignment.WrapText = true;
                    //
                    //21
                    workbook.Worksheets.ActiveWorksheet.Range["AB" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                    //
                    workbook.Worksheets.ActiveWorksheet.Cells["AB" + (Y)].Value = "";
                    workbook.Worksheets.ActiveWorksheet.Cells["AB" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                    workbook.Worksheets.ActiveWorksheet.Cells["AB" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                    workbook.Worksheets.ActiveWorksheet.Cells["AB" + (Y)].Font.Size = 6;
                    workbook.Worksheets.ActiveWorksheet.Cells["AB" + (Y)].NumberFormat = "###,###";
                    //workbook.Worksheets.ActiveWorksheet.Cells["G" + (Y)].ColumnWidth = 300;
                    workbook.Worksheets.ActiveWorksheet.Cells["AB" + (Y)].Alignment.WrapText = true;
                    //
                    //22
                    workbook.Worksheets.ActiveWorksheet.Range["AC" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                    //
                    workbook.Worksheets.ActiveWorksheet.Cells["AC" + (Y)].Value = "";
                    workbook.Worksheets.ActiveWorksheet.Cells["AC" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                    workbook.Worksheets.ActiveWorksheet.Cells["AC" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                    workbook.Worksheets.ActiveWorksheet.Cells["AC" + (Y)].Font.Size = 6;
                    workbook.Worksheets.ActiveWorksheet.Cells["AC" + (Y)].NumberFormat = "###,###";
                    //workbook.Worksheets.ActiveWorksheet.Cells["AC" + (Y)].ColumnWidth = 300;
                    workbook.Worksheets.ActiveWorksheet.Cells["AC" + (Y)].Alignment.WrapText = true;
                    //
                    //23
                    workbook.Worksheets.ActiveWorksheet.Range["AD" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                    //
                    workbook.Worksheets.ActiveWorksheet.Cells["AD" + (Y)].Value = "";
                    workbook.Worksheets.ActiveWorksheet.Cells["AD" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                    workbook.Worksheets.ActiveWorksheet.Cells["AD" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                    workbook.Worksheets.ActiveWorksheet.Cells["AD" + (Y)].Font.Size = 6;
                    workbook.Worksheets.ActiveWorksheet.Cells["AD" + (Y)].NumberFormat = "###,###";
                    //workbook.Worksheets.ActiveWorksheet.Cells["AD" + (Y)].ColumnWidth = 300;
                    workbook.Worksheets.ActiveWorksheet.Cells["AD" + (Y)].Alignment.WrapText = true;
                    //
                    //24
                    workbook.Worksheets.ActiveWorksheet.Range["AE" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                    //
                    workbook.Worksheets.ActiveWorksheet.Cells["AE" + (Y)].Value = "";
                    workbook.Worksheets.ActiveWorksheet.Cells["AE" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                    workbook.Worksheets.ActiveWorksheet.Cells["AE" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                    workbook.Worksheets.ActiveWorksheet.Cells["AE" + (Y)].Font.Size = 6;
                    workbook.Worksheets.ActiveWorksheet.Cells["AE" + (Y)].NumberFormat = "###,###";
                    //workbook.Worksheets.ActiveWorksheet.Cells["AE" + (Y)].ColumnWidth = 300;
                    workbook.Worksheets.ActiveWorksheet.Cells["AE" + (Y)].Alignment.WrapText = true;
                    //
                    //25
                    workbook.Worksheets.ActiveWorksheet.Range["AF" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                    //
                    workbook.Worksheets.ActiveWorksheet.Cells["AF" + (Y)].Value = "";
                    workbook.Worksheets.ActiveWorksheet.Cells["AF" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                    workbook.Worksheets.ActiveWorksheet.Cells["AF" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                    workbook.Worksheets.ActiveWorksheet.Cells["AF" + (Y)].Font.Size = 6;
                    workbook.Worksheets.ActiveWorksheet.Cells["AF" + (Y)].NumberFormat = "###,###";
                    //workbook.Worksheets.ActiveWorksheet.Cells["AF" + (Y)].ColumnWidth = 300;
                    workbook.Worksheets.ActiveWorksheet.Cells["AF" + (Y)].Alignment.WrapText = true;
                    //
                    //26
                    workbook.Worksheets.ActiveWorksheet.Range["AG" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                    //
                    workbook.Worksheets.ActiveWorksheet.Cells["AG" + (Y)].Value = "";
                    workbook.Worksheets.ActiveWorksheet.Cells["AG" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                    workbook.Worksheets.ActiveWorksheet.Cells["AG" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                    workbook.Worksheets.ActiveWorksheet.Cells["AG" + (Y)].Font.Size = 6;
                    workbook.Worksheets.ActiveWorksheet.Cells["AG" + (Y)].NumberFormat = "###,###";
                    //workbook.Worksheets.ActiveWorksheet.Cells["AG" + (Y)].ColumnWidth = 300;
                    workbook.Worksheets.ActiveWorksheet.Cells["AG" + (Y)].Alignment.WrapText = true;
                    //
                    //27
                    workbook.Worksheets.ActiveWorksheet.Range["AH" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                    //
                    workbook.Worksheets.ActiveWorksheet.Cells["AH" + (Y)].Value = "";
                    workbook.Worksheets.ActiveWorksheet.Cells["AH" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                    workbook.Worksheets.ActiveWorksheet.Cells["AH" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                    workbook.Worksheets.ActiveWorksheet.Cells["AH" + (Y)].Font.Size = 6;
                    workbook.Worksheets.ActiveWorksheet.Cells["AH" + (Y)].NumberFormat = "###,###";
                    //workbook.Worksheets.ActiveWorksheet.Cells["AH" + (Y)].ColumnWidth = 300;
                    workbook.Worksheets.ActiveWorksheet.Cells["AH" + (Y)].Alignment.WrapText = true;
                    //
                    //28
                    workbook.Worksheets.ActiveWorksheet.Range["AI" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                    //
                    workbook.Worksheets.ActiveWorksheet.Cells["AI" + (Y)].Value = "";
                    workbook.Worksheets.ActiveWorksheet.Cells["AI" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                    workbook.Worksheets.ActiveWorksheet.Cells["AI" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                    workbook.Worksheets.ActiveWorksheet.Cells["AI" + (Y)].Font.Size = 6;
                    workbook.Worksheets.ActiveWorksheet.Cells["AI" + (Y)].NumberFormat = "###,###";
                    //workbook.Worksheets.ActiveWorksheet.Cells["AI" + (Y)].ColumnWidth = 300;
                    workbook.Worksheets.ActiveWorksheet.Cells["AI" + (Y)].Alignment.WrapText = true;
                    //
                    //29
                    workbook.Worksheets.ActiveWorksheet.Range["AJ" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                    //
                    workbook.Worksheets.ActiveWorksheet.Cells["AJ" + (Y)].Value = "";
                    workbook.Worksheets.ActiveWorksheet.Cells["AJ" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                    workbook.Worksheets.ActiveWorksheet.Cells["AJ" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                    workbook.Worksheets.ActiveWorksheet.Cells["AJ" + (Y)].Font.Size = 6;
                    workbook.Worksheets.ActiveWorksheet.Cells["AJ" + (Y)].NumberFormat = "###,###";
                    //workbook.Worksheets.ActiveWorksheet.Cells["AJ" + (Y)].ColumnWidth = 300;
                    workbook.Worksheets.ActiveWorksheet.Cells["AJ" + (Y)].Alignment.WrapText = true;
                    //
                    //30
                    workbook.Worksheets.ActiveWorksheet.Range["AK" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                    //
                    workbook.Worksheets.ActiveWorksheet.Cells["AK" + (Y)].Value = "";
                    workbook.Worksheets.ActiveWorksheet.Cells["AK" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                    workbook.Worksheets.ActiveWorksheet.Cells["AK" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                    workbook.Worksheets.ActiveWorksheet.Cells["AK" + (Y)].Font.Size = 6;
                    workbook.Worksheets.ActiveWorksheet.Cells["AK" + (Y)].NumberFormat = "###,###";
                    //workbook.Worksheets.ActiveWorksheet.Cells["AK" + (Y)].ColumnWidth = 300;
                    workbook.Worksheets.ActiveWorksheet.Cells["AK" + (Y)].Alignment.WrapText = true;
                    //
                    //31
                    workbook.Worksheets.ActiveWorksheet.Range["AL" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                    //
                    workbook.Worksheets.ActiveWorksheet.Cells["AL" + (Y)].Value = "";
                    workbook.Worksheets.ActiveWorksheet.Cells["AL" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                    workbook.Worksheets.ActiveWorksheet.Cells["AL" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                    workbook.Worksheets.ActiveWorksheet.Cells["AL" + (Y)].Font.Size = 6;
                    workbook.Worksheets.ActiveWorksheet.Cells["AL" + (Y)].NumberFormat = "###,###";
                    //workbook.Worksheets.ActiveWorksheet.Cells["AL" + (Y)].ColumnWidth = 300;
                    workbook.Worksheets.ActiveWorksheet.Cells["AL" + (Y)].Alignment.WrapText = true;
                    //
                    //실적현황 - 수량
                    workbook.Worksheets.ActiveWorksheet.Range["AM" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                    //
                    //workbook.Worksheets.ActiveWorksheet.Cells["AM" + (Y)].Value = "";
                    workbook.Worksheets.ActiveWorksheet.Cells["AM" + (Y)].Formula = "=Sum(H" + (Y) + ":AL" + (Y) + ")";
                    workbook.Worksheets.ActiveWorksheet.Cells["AM" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                    workbook.Worksheets.ActiveWorksheet.Cells["AM" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                    workbook.Worksheets.ActiveWorksheet.Cells["AM" + (Y)].Font.Size = 6;
                    workbook.Worksheets.ActiveWorksheet.Cells["AM" + (Y)].NumberFormat = "###,###";
                    //workbook.Worksheets.ActiveWorksheet.Cells["AM" + (Y)].ColumnWidth = 300;
                    workbook.Worksheets.ActiveWorksheet.Cells["AM" + (Y)].Alignment.WrapText = true;
                    //
                    //실적현황 - %
                    workbook.Worksheets.ActiveWorksheet.Range["AN" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                    //
                    //workbook.Worksheets.ActiveWorksheet.Cells["AN" + (Y)].Value = "";
                    workbook.Worksheets.ActiveWorksheet.Cells["AN" + (Y)].Formula = "=Sum(H" + (Y) + ":AL" + (Y) + ")";
                    workbook.Worksheets.ActiveWorksheet.Cells["AN" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                    workbook.Worksheets.ActiveWorksheet.Cells["AN" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                    workbook.Worksheets.ActiveWorksheet.Cells["AN" + (Y)].Font.Size = 6;
                    workbook.Worksheets.ActiveWorksheet.Cells["AN" + (Y)].NumberFormat = "###,###";
                    //workbook.Worksheets.ActiveWorksheet.Cells["AN" + (Y)].ColumnWidth = 300;
                    workbook.Worksheets.ActiveWorksheet.Cells["AN" + (Y)].Alignment.WrapText = true;

                    #endregion

                    Y++;

                    //if (searchItems.Count > 0)
                    //{
                    //    workbook.Worksheets.ActiveWorksheet.Cells["B" + (6 + nRowContents)].Value = item.MCHN_NM;
                    //    workbook.Worksheets.ActiveWorksheet.Cells["B" + (6 + nRowContents)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                    //    workbook.Worksheets.ActiveWorksheet.Cells["B" + (6 + nRowContents)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                    //    workbook.Worksheets.ActiveWorksheet.Cells["B" + (6 + nRowContents)].Font.Size = 11;
                    //    workbook.Worksheets.ActiveWorksheet.Cells["B" + (6 + nRowContents)].Alignment.WrapText = true;

                    //    workbook.Worksheets.ActiveWorksheet.MergeCells(workbook.Worksheets.ActiveWorksheet.Range["B" + (6 + nRowContents) + ":B" + ((6 + nRowContents) + searchItems.Count)]);
                    //    workbook.Worksheets.ActiveWorksheet.Range["B" + (6 + nRowContents) + ":B" + ((6 + nRowContents) + searchItems.Count)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);

                    //    nRowContents += searchItems.Count;
                    //    //
                    //}

                }

                //생산 계획
                workbook.Worksheets.ActiveWorksheet.MergeCells(workbook.Worksheets.ActiveWorksheet.Range["B" + (Y) + ":C" + (Y)]);
                workbook.Worksheets.ActiveWorksheet.Range["B" + (Y) + ":C" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                //
                workbook.Worksheets.ActiveWorksheet.Cells["B" + (Y)].Value = "생산계획";
                workbook.Worksheets.ActiveWorksheet.Cells["B" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                workbook.Worksheets.ActiveWorksheet.Cells["B" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                workbook.Worksheets.ActiveWorksheet.Cells["B" + (Y)].Font.Size = 10;
                workbook.Worksheets.ActiveWorksheet.Cells["B" + (Y)].Alignment.WrapText = true;
                workbook.Worksheets.ActiveWorksheet.Cells["B" + (Y)].Font.Bold = true;
                workbook.Worksheets.ActiveWorksheet.Cells["B" + (Y)].FillColor = Color.LightGray;
                //
                ////
                workbook.Worksheets.ActiveWorksheet.MergeCells(workbook.Worksheets.ActiveWorksheet.Range["D" + (Y) + ":E" + (Y)]);
                workbook.Worksheets.ActiveWorksheet.Range["D" + (Y) + ":E" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                //
                workbook.Worksheets.ActiveWorksheet.Cells["D" + (Y)].Value = nSL_PLN_QTY;
                workbook.Worksheets.ActiveWorksheet.Cells["D" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                workbook.Worksheets.ActiveWorksheet.Cells["D" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                workbook.Worksheets.ActiveWorksheet.Cells["D" + (Y)].Font.Size = 10;
                workbook.Worksheets.ActiveWorksheet.Cells["D" + (Y)].Alignment.WrapText = true;
                workbook.Worksheets.ActiveWorksheet.Cells["D" + (Y)].NumberFormat = "###,###";
                workbook.Worksheets.ActiveWorksheet.Cells["D" + (Y)].Font.Bold = true;
                workbook.Worksheets.ActiveWorksheet.Cells["D" + (Y)].FillColor = Color.LightGray;
                //
                ////
                workbook.Worksheets.ActiveWorksheet.MergeCells(workbook.Worksheets.ActiveWorksheet.Range["F" + (Y) + ":G" + (Y)]);
                workbook.Worksheets.ActiveWorksheet.Range["F" + (Y) + ":G" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                //
                workbook.Worksheets.ActiveWorksheet.Cells["F" + (Y)].Value = nTOTAL_PROD_PLN_QTY;
                workbook.Worksheets.ActiveWorksheet.Cells["F" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                workbook.Worksheets.ActiveWorksheet.Cells["F" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                workbook.Worksheets.ActiveWorksheet.Cells["F" + (Y)].Font.Size = 10;
                workbook.Worksheets.ActiveWorksheet.Cells["F" + (Y)].Alignment.WrapText = true;
                workbook.Worksheets.ActiveWorksheet.Cells["F" + (Y)].NumberFormat = "###,###";
                workbook.Worksheets.ActiveWorksheet.Cells["F" + (Y)].Font.Bold = true;
                workbook.Worksheets.ActiveWorksheet.Cells["F" + (Y)].FillColor = Color.LightGray;

                //
                //1~31  [생산실적]
                #region 1~31  [생산실적]
                //1
                workbook.Worksheets.ActiveWorksheet.Range["H" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                //
                workbook.Worksheets.ActiveWorksheet.Cells["H" + (Y)].Formula = "=Sum(H6" + ":H" + (Y-1) + ")";
                workbook.Worksheets.ActiveWorksheet.Cells["H" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                workbook.Worksheets.ActiveWorksheet.Cells["H" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                workbook.Worksheets.ActiveWorksheet.Cells["H" + (Y)].Font.Size = 6;
                workbook.Worksheets.ActiveWorksheet.Cells["H" + (Y)].NumberFormat = "###,###";
                //workbook.Worksheets.ActiveWorksheet.Cells["H" + (Y)].ColumnWidth = 300;
                workbook.Worksheets.ActiveWorksheet.Cells["H" + (Y)].Alignment.WrapText = true;
                //
                //2
                workbook.Worksheets.ActiveWorksheet.Range["I" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                //
                workbook.Worksheets.ActiveWorksheet.Cells["I" + (Y)].Formula = "=Sum(I6" + ":I" + (Y - 1) + ")";
                workbook.Worksheets.ActiveWorksheet.Cells["I" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                workbook.Worksheets.ActiveWorksheet.Cells["I" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                workbook.Worksheets.ActiveWorksheet.Cells["I" + (Y)].Font.Size = 6;
                workbook.Worksheets.ActiveWorksheet.Cells["I" + (Y)].NumberFormat = "###,###";
                //workbook.Worksheets.ActiveWorksheet.Cells["I" + (Y)].ColumnWidth = 300;
                workbook.Worksheets.ActiveWorksheet.Cells["I" + (Y)].Alignment.WrapText = true;
                //
                //3
                workbook.Worksheets.ActiveWorksheet.Range["J" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                //
                workbook.Worksheets.ActiveWorksheet.Cells["J" + (Y)].Formula = "=Sum(J6" + ":J" + (Y - 1) + ")";
                workbook.Worksheets.ActiveWorksheet.Cells["J" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                workbook.Worksheets.ActiveWorksheet.Cells["J" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                workbook.Worksheets.ActiveWorksheet.Cells["J" + (Y)].Font.Size = 6;
                workbook.Worksheets.ActiveWorksheet.Cells["J" + (Y)].NumberFormat = "###,###";
                //workbook.Worksheets.ActiveWorksheet.Cells["J" + (Y)].ColumnWidth = 300;
                workbook.Worksheets.ActiveWorksheet.Cells["J" + (Y)].Alignment.WrapText = true;
                //
                //4
                workbook.Worksheets.ActiveWorksheet.Range["K" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                //
                workbook.Worksheets.ActiveWorksheet.Cells["K" + (Y)].Formula = "=Sum(K6" + ":K" + (Y - 1) + ")";
                workbook.Worksheets.ActiveWorksheet.Cells["K" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                workbook.Worksheets.ActiveWorksheet.Cells["K" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                workbook.Worksheets.ActiveWorksheet.Cells["K" + (Y)].Font.Size = 6;
                workbook.Worksheets.ActiveWorksheet.Cells["K" + (Y)].NumberFormat = "###,###";
                //workbook.Worksheets.ActiveWorksheet.Cells["K" + (Y)].ColumnWidth = 300;
                workbook.Worksheets.ActiveWorksheet.Cells["K" + (Y)].Alignment.WrapText = true;
                //
                //5
                workbook.Worksheets.ActiveWorksheet.Range["L" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                //
                workbook.Worksheets.ActiveWorksheet.Cells["L" + (Y)].Formula = "=Sum(L6" + ":L" + (Y - 1) + ")";
                workbook.Worksheets.ActiveWorksheet.Cells["L" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                workbook.Worksheets.ActiveWorksheet.Cells["L" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                workbook.Worksheets.ActiveWorksheet.Cells["L" + (Y)].Font.Size = 6;
                workbook.Worksheets.ActiveWorksheet.Cells["L" + (Y)].NumberFormat = "###,###";
                //workbook.Worksheets.ActiveWorksheet.Cells["L" + (Y)].ColumnWidth = 300;
                workbook.Worksheets.ActiveWorksheet.Cells["L" + (Y)].Alignment.WrapText = true;
                //
                //6
                workbook.Worksheets.ActiveWorksheet.Range["M" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                //
                workbook.Worksheets.ActiveWorksheet.Cells["M" + (Y)].Formula = "=Sum(M6" + ":M" + (Y - 1) + ")";
                workbook.Worksheets.ActiveWorksheet.Cells["M" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                workbook.Worksheets.ActiveWorksheet.Cells["M" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                workbook.Worksheets.ActiveWorksheet.Cells["M" + (Y)].Font.Size = 6;
                workbook.Worksheets.ActiveWorksheet.Cells["M" + (Y)].NumberFormat = "###,###";
                //workbook.Worksheets.ActiveWorksheet.Cells["M" + (Y)].ColumnWidth = 300;
                workbook.Worksheets.ActiveWorksheet.Cells["M" + (Y)].Alignment.WrapText = true;
                //
                //7
                workbook.Worksheets.ActiveWorksheet.Range["N" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                //
                workbook.Worksheets.ActiveWorksheet.Cells["N" + (Y)].Formula = "=Sum(N6" + ":N" + (Y - 1) + ")";
                workbook.Worksheets.ActiveWorksheet.Cells["N" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                workbook.Worksheets.ActiveWorksheet.Cells["N" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                workbook.Worksheets.ActiveWorksheet.Cells["N" + (Y)].Font.Size = 6;
                workbook.Worksheets.ActiveWorksheet.Cells["N" + (Y)].NumberFormat = "###,###";
                //workbook.Worksheets.ActiveWorksheet.Cells["N" + (Y)].ColumnWidth = 300;
                workbook.Worksheets.ActiveWorksheet.Cells["N" + (Y)].Alignment.WrapText = true;
                //
                //8
                workbook.Worksheets.ActiveWorksheet.Range["O" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                //
                workbook.Worksheets.ActiveWorksheet.Cells["O" + (Y)].Formula = "=Sum(O6" + ":O" + (Y - 1) + ")";
                workbook.Worksheets.ActiveWorksheet.Cells["O" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                workbook.Worksheets.ActiveWorksheet.Cells["O" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                workbook.Worksheets.ActiveWorksheet.Cells["O" + (Y)].Font.Size = 6;
                workbook.Worksheets.ActiveWorksheet.Cells["O" + (Y)].NumberFormat = "###,###";
                //workbook.Worksheets.ActiveWorksheet.Cells["O" + (Y)].ColumnWidth = 300;
                workbook.Worksheets.ActiveWorksheet.Cells["O" + (Y)].Alignment.WrapText = true;
                //
                //9
                workbook.Worksheets.ActiveWorksheet.Range["P" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                //
                workbook.Worksheets.ActiveWorksheet.Cells["P" + (Y)].Formula = "=Sum(P6" + ":P" + (Y - 1) + ")";
                workbook.Worksheets.ActiveWorksheet.Cells["P" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                workbook.Worksheets.ActiveWorksheet.Cells["P" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                workbook.Worksheets.ActiveWorksheet.Cells["P" + (Y)].Font.Size = 6;
                workbook.Worksheets.ActiveWorksheet.Cells["P" + (Y)].NumberFormat = "###,###";
                //workbook.Worksheets.ActiveWorksheet.Cells["P" + (Y)].ColumnWidth = 300;
                workbook.Worksheets.ActiveWorksheet.Cells["P" + (Y)].Alignment.WrapText = true;
                //
                //10
                workbook.Worksheets.ActiveWorksheet.Range["Q" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                //
                workbook.Worksheets.ActiveWorksheet.Cells["Q" + (Y)].Formula = "=Sum(Q6" + ":Q" + (Y - 1) + ")";
                workbook.Worksheets.ActiveWorksheet.Cells["Q" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                workbook.Worksheets.ActiveWorksheet.Cells["Q" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                workbook.Worksheets.ActiveWorksheet.Cells["Q" + (Y)].Font.Size = 6;
                workbook.Worksheets.ActiveWorksheet.Cells["Q" + (Y)].NumberFormat = "###,###";
                //workbook.Worksheets.ActiveWorksheet.Cells["Q" + (Y)].ColumnWidth = 300;
                workbook.Worksheets.ActiveWorksheet.Cells["Q" + (Y)].Alignment.WrapText = true;
                //
                //11
                workbook.Worksheets.ActiveWorksheet.Range["R" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                //
                workbook.Worksheets.ActiveWorksheet.Cells["R" + (Y)].Formula = "=Sum(R6" + ":R" + (Y - 1) + ")";
                workbook.Worksheets.ActiveWorksheet.Cells["R" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                workbook.Worksheets.ActiveWorksheet.Cells["R" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                workbook.Worksheets.ActiveWorksheet.Cells["R" + (Y)].Font.Size = 6;
                workbook.Worksheets.ActiveWorksheet.Cells["R" + (Y)].NumberFormat = "###,###";
                //workbook.Worksheets.ActiveWorksheet.Cells["R" + (Y)].ColumnWidth = 300;
                workbook.Worksheets.ActiveWorksheet.Cells["R" + (Y)].Alignment.WrapText = true;
                //
                //12
                workbook.Worksheets.ActiveWorksheet.Range["S" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                //
                workbook.Worksheets.ActiveWorksheet.Cells["S" + (Y)].Formula = "=Sum(S6" + ":S" + (Y - 1) + ")";
                workbook.Worksheets.ActiveWorksheet.Cells["S" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                workbook.Worksheets.ActiveWorksheet.Cells["S" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                workbook.Worksheets.ActiveWorksheet.Cells["S" + (Y)].Font.Size = 6;
                workbook.Worksheets.ActiveWorksheet.Cells["S" + (Y)].NumberFormat = "###,###";
                //workbook.Worksheets.ActiveWorksheet.Cells["S" + (Y)].ColumnWidth = 300;
                workbook.Worksheets.ActiveWorksheet.Cells["S" + (Y)].Alignment.WrapText = true;
                //
                //13
                workbook.Worksheets.ActiveWorksheet.Range["T" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                //
                workbook.Worksheets.ActiveWorksheet.Cells["T" + (Y)].Formula = "=Sum(T6" + ":T" + (Y - 1) + ")";
                workbook.Worksheets.ActiveWorksheet.Cells["T" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                workbook.Worksheets.ActiveWorksheet.Cells["T" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                workbook.Worksheets.ActiveWorksheet.Cells["T" + (Y)].Font.Size = 6;
                workbook.Worksheets.ActiveWorksheet.Cells["T" + (Y)].NumberFormat = "###,###";
                //workbook.Worksheets.ActiveWorksheet.Cells["T" + (Y)].ColumnWidth = 300;
                workbook.Worksheets.ActiveWorksheet.Cells["T" + (Y)].Alignment.WrapText = true;
                //
                //14
                workbook.Worksheets.ActiveWorksheet.Range["U" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                //
                workbook.Worksheets.ActiveWorksheet.Cells["U" + (Y)].Formula = "=Sum(U6" + ":U" + (Y - 1) + ")";
                workbook.Worksheets.ActiveWorksheet.Cells["U" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                workbook.Worksheets.ActiveWorksheet.Cells["U" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                workbook.Worksheets.ActiveWorksheet.Cells["U" + (Y)].Font.Size = 6;
                workbook.Worksheets.ActiveWorksheet.Cells["U" + (Y)].NumberFormat = "###,###";
                //workbook.Worksheets.ActiveWorksheet.Cells["U" + (Y)].ColumnWidth = 300;
                workbook.Worksheets.ActiveWorksheet.Cells["U" + (Y)].Alignment.WrapText = true;
                //
                //15
                workbook.Worksheets.ActiveWorksheet.Range["V" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                //
                workbook.Worksheets.ActiveWorksheet.Cells["V" + (Y)].Formula = "=Sum(V6" + ":V" + (Y - 1) + ")";
                workbook.Worksheets.ActiveWorksheet.Cells["V" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                workbook.Worksheets.ActiveWorksheet.Cells["V" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                workbook.Worksheets.ActiveWorksheet.Cells["V" + (Y)].Font.Size = 6;
                workbook.Worksheets.ActiveWorksheet.Cells["V" + (Y)].NumberFormat = "###,###";
                //workbook.Worksheets.ActiveWorksheet.Cells["V" + (Y)].ColumnWidth = 300;
                workbook.Worksheets.ActiveWorksheet.Cells["V" + (Y)].Alignment.WrapText = true;
                //
                //16
                workbook.Worksheets.ActiveWorksheet.Range["W" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                //
                workbook.Worksheets.ActiveWorksheet.Cells["W" + (Y)].Formula = "=Sum(W6" + ":W" + (Y - 1) + ")";
                workbook.Worksheets.ActiveWorksheet.Cells["W" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                workbook.Worksheets.ActiveWorksheet.Cells["W" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                workbook.Worksheets.ActiveWorksheet.Cells["W" + (Y)].Font.Size = 6;
                workbook.Worksheets.ActiveWorksheet.Cells["W" + (Y)].NumberFormat = "###,###";
                //workbook.Worksheets.ActiveWorksheet.Cells["W" + (Y)].ColumnWidth = 300;
                workbook.Worksheets.ActiveWorksheet.Cells["W" + (Y)].Alignment.WrapText = true;
                //
                //17
                workbook.Worksheets.ActiveWorksheet.Range["X" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                //
                workbook.Worksheets.ActiveWorksheet.Cells["X" + (Y)].Formula = "=Sum(X6" + ":X" + (Y - 1) + ")";
                workbook.Worksheets.ActiveWorksheet.Cells["X" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                workbook.Worksheets.ActiveWorksheet.Cells["X" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                workbook.Worksheets.ActiveWorksheet.Cells["X" + (Y)].Font.Size = 6;
                workbook.Worksheets.ActiveWorksheet.Cells["X" + (Y)].NumberFormat = "###,###";
                //workbook.Worksheets.ActiveWorksheet.Cells["X" + (Y)].ColumnWidth = 300;
                workbook.Worksheets.ActiveWorksheet.Cells["X" + (Y)].Alignment.WrapText = true;
                //
                //18
                workbook.Worksheets.ActiveWorksheet.Range["Y" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                //
                workbook.Worksheets.ActiveWorksheet.Cells["Y" + (Y)].Formula = "=Sum(Y6" + ":Y" + (Y - 1) + ")";
                workbook.Worksheets.ActiveWorksheet.Cells["Y" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                workbook.Worksheets.ActiveWorksheet.Cells["Y" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                workbook.Worksheets.ActiveWorksheet.Cells["Y" + (Y)].Font.Size = 6;
                workbook.Worksheets.ActiveWorksheet.Cells["Y" + (Y)].NumberFormat = "###,###";
                //workbook.Worksheets.ActiveWorksheet.Cells["Y" + (Y)].ColumnWidth = 300;
                workbook.Worksheets.ActiveWorksheet.Cells["Y" + (Y)].Alignment.WrapText = true;
                //
                //19
                workbook.Worksheets.ActiveWorksheet.Range["Z" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                //
                workbook.Worksheets.ActiveWorksheet.Cells["Z" + (Y)].Formula = "=Sum(Z6" + ":Z" + (Y - 1) + ")";
                workbook.Worksheets.ActiveWorksheet.Cells["Z" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                workbook.Worksheets.ActiveWorksheet.Cells["Z" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                workbook.Worksheets.ActiveWorksheet.Cells["Z" + (Y)].Font.Size = 6;
                workbook.Worksheets.ActiveWorksheet.Cells["Z" + (Y)].NumberFormat = "###,###";
                //workbook.Worksheets.ActiveWorksheet.Cells["Z" + (Y)].ColumnWidth = 300;
                workbook.Worksheets.ActiveWorksheet.Cells["Z" + (Y)].Alignment.WrapText = true;
                //
                //20
                workbook.Worksheets.ActiveWorksheet.Range["AA" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                //
                workbook.Worksheets.ActiveWorksheet.Cells["AA" + (Y)].Formula = "=Sum(AA6" + ":AA" + (Y - 1) + ")";
                workbook.Worksheets.ActiveWorksheet.Cells["AA" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                workbook.Worksheets.ActiveWorksheet.Cells["AA" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                workbook.Worksheets.ActiveWorksheet.Cells["AA" + (Y)].Font.Size = 6;
                //workbook.Worksheets.ActiveWorksheet.Cells["AA" + (Y)].ColumnWidth = 300;
                workbook.Worksheets.ActiveWorksheet.Cells["AA" + (Y)].Alignment.WrapText = true;
                //
                //21
                workbook.Worksheets.ActiveWorksheet.Range["AB" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                //
                workbook.Worksheets.ActiveWorksheet.Cells["AB" + (Y)].Formula = "=Sum(AB6" + ":AB" + (Y - 1) + ")";
                workbook.Worksheets.ActiveWorksheet.Cells["AB" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                workbook.Worksheets.ActiveWorksheet.Cells["AB" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                workbook.Worksheets.ActiveWorksheet.Cells["AB" + (Y)].Font.Size = 6;
                workbook.Worksheets.ActiveWorksheet.Cells["AB" + (Y)].NumberFormat = "###,###";
                //workbook.Worksheets.ActiveWorksheet.Cells["G" + (Y)].ColumnWidth = 300;
                workbook.Worksheets.ActiveWorksheet.Cells["AB" + (Y)].Alignment.WrapText = true;
                //
                //22
                workbook.Worksheets.ActiveWorksheet.Range["AC" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                //
                workbook.Worksheets.ActiveWorksheet.Cells["AC" + (Y)].Formula = "=Sum(AC6" + ":AC" + (Y - 1) + ")";
                workbook.Worksheets.ActiveWorksheet.Cells["AC" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                workbook.Worksheets.ActiveWorksheet.Cells["AC" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                workbook.Worksheets.ActiveWorksheet.Cells["AC" + (Y)].Font.Size = 6;
                workbook.Worksheets.ActiveWorksheet.Cells["AC" + (Y)].NumberFormat = "###,###";
                //workbook.Worksheets.ActiveWorksheet.Cells["AC" + (Y)].ColumnWidth = 300;
                workbook.Worksheets.ActiveWorksheet.Cells["AC" + (Y)].Alignment.WrapText = true;
                //
                //23
                workbook.Worksheets.ActiveWorksheet.Range["AD" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                //
                workbook.Worksheets.ActiveWorksheet.Cells["AD" + (Y)].Formula = "=Sum(AD6" + ":AD" + (Y - 1) + ")";
                workbook.Worksheets.ActiveWorksheet.Cells["AD" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                workbook.Worksheets.ActiveWorksheet.Cells["AD" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                workbook.Worksheets.ActiveWorksheet.Cells["AD" + (Y)].Font.Size = 6;
                workbook.Worksheets.ActiveWorksheet.Cells["AD" + (Y)].NumberFormat = "###,###";
                //workbook.Worksheets.ActiveWorksheet.Cells["AD" + (Y)].ColumnWidth = 300;
                workbook.Worksheets.ActiveWorksheet.Cells["AD" + (Y)].Alignment.WrapText = true;
                //
                //24
                workbook.Worksheets.ActiveWorksheet.Range["AE" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                //
                workbook.Worksheets.ActiveWorksheet.Cells["AE" + (Y)].Formula = "=Sum(AE6" + ":AE" + (Y - 1) + ")";
                workbook.Worksheets.ActiveWorksheet.Cells["AE" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                workbook.Worksheets.ActiveWorksheet.Cells["AE" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                workbook.Worksheets.ActiveWorksheet.Cells["AE" + (Y)].Font.Size = 6;
                workbook.Worksheets.ActiveWorksheet.Cells["AE" + (Y)].NumberFormat = "###,###";
                //workbook.Worksheets.ActiveWorksheet.Cells["AE" + (Y)].ColumnWidth = 300;
                workbook.Worksheets.ActiveWorksheet.Cells["AE" + (Y)].Alignment.WrapText = true;
                //
                //25
                workbook.Worksheets.ActiveWorksheet.Range["AF" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                //
                workbook.Worksheets.ActiveWorksheet.Cells["AF" + (Y)].Formula = "=Sum(AF6" + ":AF" + (Y - 1) + ")";
                workbook.Worksheets.ActiveWorksheet.Cells["AF" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                workbook.Worksheets.ActiveWorksheet.Cells["AF" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                workbook.Worksheets.ActiveWorksheet.Cells["AF" + (Y)].Font.Size = 6;
                workbook.Worksheets.ActiveWorksheet.Cells["AF" + (Y)].NumberFormat = "###,###";
                //workbook.Worksheets.ActiveWorksheet.Cells["AF" + (Y)].ColumnWidth = 300;
                workbook.Worksheets.ActiveWorksheet.Cells["AF" + (Y)].Alignment.WrapText = true;
                //
                //26
                workbook.Worksheets.ActiveWorksheet.Range["AG" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                //
                workbook.Worksheets.ActiveWorksheet.Cells["AG" + (Y)].Formula = "=Sum(AG6" + ":AG" + (Y - 1) + ")";
                workbook.Worksheets.ActiveWorksheet.Cells["AG" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                workbook.Worksheets.ActiveWorksheet.Cells["AG" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                workbook.Worksheets.ActiveWorksheet.Cells["AG" + (Y)].Font.Size = 6;
                workbook.Worksheets.ActiveWorksheet.Cells["AG" + (Y)].NumberFormat = "###,###";
                //workbook.Worksheets.ActiveWorksheet.Cells["AG" + (Y)].ColumnWidth = 300;
                workbook.Worksheets.ActiveWorksheet.Cells["AG" + (Y)].Alignment.WrapText = true;
                //
                //27
                workbook.Worksheets.ActiveWorksheet.Range["AH" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                //
                workbook.Worksheets.ActiveWorksheet.Cells["AH" + (Y)].Formula = "=Sum(AH6" + ":AH" + (Y - 1) + ")";
                workbook.Worksheets.ActiveWorksheet.Cells["AH" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                workbook.Worksheets.ActiveWorksheet.Cells["AH" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                workbook.Worksheets.ActiveWorksheet.Cells["AH" + (Y)].Font.Size = 6;
                workbook.Worksheets.ActiveWorksheet.Cells["AH" + (Y)].NumberFormat = "###,###";
                //workbook.Worksheets.ActiveWorksheet.Cells["AH" + (Y)].ColumnWidth = 300;
                workbook.Worksheets.ActiveWorksheet.Cells["AH" + (Y)].Alignment.WrapText = true;
                //
                //28
                workbook.Worksheets.ActiveWorksheet.Range["AI" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                //
                workbook.Worksheets.ActiveWorksheet.Cells["AI" + (Y)].Formula = "=Sum(AI6" + ":AI" + (Y - 1) + ")";
                workbook.Worksheets.ActiveWorksheet.Cells["AI" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                workbook.Worksheets.ActiveWorksheet.Cells["AI" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                workbook.Worksheets.ActiveWorksheet.Cells["AI" + (Y)].Font.Size = 6;
                workbook.Worksheets.ActiveWorksheet.Cells["AI" + (Y)].NumberFormat = "###,###";
                //workbook.Worksheets.ActiveWorksheet.Cells["AI" + (Y)].ColumnWidth = 300;
                workbook.Worksheets.ActiveWorksheet.Cells["AI" + (Y)].Alignment.WrapText = true;
                //
                //29
                workbook.Worksheets.ActiveWorksheet.Range["AJ" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                //
                workbook.Worksheets.ActiveWorksheet.Cells["AJ" + (Y)].Formula = "=Sum(AJ6" + ":AJ" + (Y - 1) + ")";
                workbook.Worksheets.ActiveWorksheet.Cells["AJ" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                workbook.Worksheets.ActiveWorksheet.Cells["AJ" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                workbook.Worksheets.ActiveWorksheet.Cells["AJ" + (Y)].Font.Size = 6;
                workbook.Worksheets.ActiveWorksheet.Cells["AJ" + (Y)].NumberFormat = "###,###";
                //workbook.Worksheets.ActiveWorksheet.Cells["AJ" + (Y)].ColumnWidth = 300;
                workbook.Worksheets.ActiveWorksheet.Cells["AJ" + (Y)].Alignment.WrapText = true;
                //
                //30
                workbook.Worksheets.ActiveWorksheet.Range["AK" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                //
                workbook.Worksheets.ActiveWorksheet.Cells["AK" + (Y)].Formula = "=Sum(AK6" + ":AK" + (Y - 1) + ")";
                workbook.Worksheets.ActiveWorksheet.Cells["AK" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                workbook.Worksheets.ActiveWorksheet.Cells["AK" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                workbook.Worksheets.ActiveWorksheet.Cells["AK" + (Y)].Font.Size = 6;
                workbook.Worksheets.ActiveWorksheet.Cells["AK" + (Y)].NumberFormat = "###,###";
                //workbook.Worksheets.ActiveWorksheet.Cells["AK" + (Y)].ColumnWidth = 300;
                workbook.Worksheets.ActiveWorksheet.Cells["AK" + (Y)].Alignment.WrapText = true;
                //
                //31
                workbook.Worksheets.ActiveWorksheet.Range["AL" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                //
                workbook.Worksheets.ActiveWorksheet.Cells["AL" + (Y)].Formula = "=Sum(AL6" + ":AL" + (Y - 1) + ")";
                workbook.Worksheets.ActiveWorksheet.Cells["AL" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                workbook.Worksheets.ActiveWorksheet.Cells["AL" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                workbook.Worksheets.ActiveWorksheet.Cells["AL" + (Y)].Font.Size = 6;
                workbook.Worksheets.ActiveWorksheet.Cells["AL" + (Y)].NumberFormat = "###,###";
                //workbook.Worksheets.ActiveWorksheet.Cells["AL" + (Y)].ColumnWidth = 300;
                workbook.Worksheets.ActiveWorksheet.Cells["AL" + (Y)].Alignment.WrapText = true;
                //
                //실적현황 - 수량
                workbook.Worksheets.ActiveWorksheet.Range["AM" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                //
                //workbook.Worksheets.ActiveWorksheet.Cells["AM" + (Y)].Value = "";
                //workbook.Worksheets.ActiveWorksheet.Cells["AM" + (Y)].Formula = "=Sum(H" + (Y) + ":AL" + (Y) + ")";
                workbook.Worksheets.ActiveWorksheet.Cells["AM" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                workbook.Worksheets.ActiveWorksheet.Cells["AM" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                workbook.Worksheets.ActiveWorksheet.Cells["AM" + (Y)].Font.Size = 6;
                //workbook.Worksheets.ActiveWorksheet.Cells["AM" + (Y)].ColumnWidth = 300;
                workbook.Worksheets.ActiveWorksheet.Cells["AM" + (Y)].Alignment.WrapText = true;
                //
                //실적현황 - %
                workbook.Worksheets.ActiveWorksheet.Range["AN" + (Y)].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);
                //
                //workbook.Worksheets.ActiveWorksheet.Cells["AN" + (Y)].Value = "";
                //workbook.Worksheets.ActiveWorksheet.Cells["AN" + (Y)].Formula = "=Sum(H" + (Y) + ":AL" + (Y) + ")";
                workbook.Worksheets.ActiveWorksheet.Cells["AN" + (Y)].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Right;
                workbook.Worksheets.ActiveWorksheet.Cells["AN" + (Y)].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                workbook.Worksheets.ActiveWorksheet.Cells["AN" + (Y)].Font.Size = 6;
                //workbook.Worksheets.ActiveWorksheet.Cells["AN" + (Y)].ColumnWidth = 300;
                workbook.Worksheets.ActiveWorksheet.Cells["AN" + (Y)].Alignment.WrapText = true;
                #endregion

                #endregion

                DXSplashScreen.Close();
            }
            catch (System.Exception eLog)
            {
                if (DXSplashScreen.IsActive == true)
                {
                    DXSplashScreen.Close();
                }
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + this.Title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }

        //void btn_save_Click(object sender, RoutedEventArgs e)
        //{
        //    MessageBoxResult result = WinUIMessageBox.Show("저장 하시겠습니까?", "[파일 저장]엑셀", MessageBoxButton.YesNo, MessageBoxImage.Question);
        //    if (result == MessageBoxResult.Yes)
        //    {
        //        ////임시 파일 저장
        //        //IWorkbook workbook = spreadsheetControl.Document;
        //        //using (FileStream stream = new FileStream(FULL_FILE_PATH, FileMode.Create, FileAccess.ReadWrite))
        //        //{
        //        //    workbook.SaveDocument(stream, DocumentFormat.OpenXml);
        //        //}

        //        //this.IMAGE = FileToByteArray(FULL_FILE_PATH);

        //        this.DialogResult = true;
        //        this.Close();
        //    }
        //}

        void btn_print_Click(object sender, RoutedEventArgs e)
        {
            Worksheet worksheet = this.spreadsheetControl.ActiveWorksheet;

            worksheet.ActiveView.Orientation = PageOrientation.Landscape;
            worksheet.ActiveView.ShowHeadings = true;
            worksheet.ActiveView.PaperKind = System.Drawing.Printing.PaperKind.A3;
            WorksheetPrintOptions printOptions = worksheet.PrintOptions;
            printOptions.BlackAndWhite = true;
            printOptions.PrintGridlines = false;
            printOptions.FitToPage = true;
            printOptions.FitToWidth = 2;
            printOptions.ErrorsPrintMode = ErrorsPrintMode.Dash;

            using (LegacyPrintableComponentLink link = new LegacyPrintableComponentLink(this.spreadsheetControl.ActiveWorksheet.Workbook))
            {
                link.PrintingSystem.Watermark.Text = Properties.Settings.Default.SettingCompany;
                link.PrintingSystem.Watermark.TextDirection = DirectionMode.ForwardDiagonal;
                link.PrintingSystem.Watermark.Font = new Font(link.PrintingSystem.Watermark.Font.FontFamily, 40);
                link.PrintingSystem.Watermark.ForeColor = System.Drawing.Color.PaleTurquoise;
                link.PrintingSystem.Watermark.TextTransparency = 150;

                link.CreateDocument();
                link.ShowPrintPreviewDialog(Application.Current.MainWindow, this.Title);
            }
        }

        void btn_saveAs_Click(object sender, RoutedEventArgs e)
        {

            //this.spreadsheetControl.ex

            //ManVo Dao = this.orgVo;
            //if (Dao == null)
            //{
            //    return;
            //}

            //if (Dao != null)
            //{
                //System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
                //dialog.ShowNewFolderButton = true;
                //dialog.Description = "[" + Dao.PROD_EQ_FILE_NM + " 파일] 저장 폴더를 선택 해 주세요.";
                //dialog.RootFolder = Environment.SpecialFolder.Desktop;
                //dialog.ShowDialog();
                //if (!string.IsNullOrEmpty(dialog.SelectedPath))
                //{
                //    if (!string.IsNullOrEmpty(Dao.PROD_EQ_FILE_NM))
                //    {
                //        int ArraySize = Dao.PROD_EQ_IMG.Length;
                //        FileStream fs = new FileStream(dialog.SelectedPath + "/" + Dao.PROD_EQ_FILE_NM, FileMode.OpenOrCreate, FileAccess.Write);
                //        fs.Write(Dao.PROD_EQ_IMG, 0, ArraySize);
                //        fs.Close();
                //    }
                //    System.Diagnostics.Process.Start(dialog.SelectedPath);
                //}
            //}
        }


        //private byte[] FileToByteArray(string filePath)
        //{
        //    using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
        //    {
        //        int length = Convert.ToInt32(fs.Length);
        //        BinaryReader br = new BinaryReader(fs);
        //        byte[] buff = br.ReadBytes(length);
        //        fs.Close();

        //        return buff;
        //    }
        //}

        void btn_close_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }


        private string WeekDay(DayOfWeek dt)
        {
            string weekDay = string.Empty;
            switch (dt)
            {
                case DayOfWeek.Monday:
                    weekDay = "월";
                    break;
                case DayOfWeek.Tuesday:
                    weekDay = "화";
                    break;
                case DayOfWeek.Wednesday:
                    weekDay = "수";
                    break;
                case DayOfWeek.Thursday:
                    weekDay = "목";
                    break;
                case DayOfWeek.Friday:
                    weekDay = "금";
                    break;
                case DayOfWeek.Saturday:
                    weekDay = "토";
                    break;
                case DayOfWeek.Sunday:
                    weekDay = "일";
                    break;
            }
            return weekDay;
        }


        private string ColumnIndexToColumnLetter(int colIndex)
        {
            int div = colIndex;
            string colLetter = String.Empty;
            int mod = 0;

            while (div > 0)
            {
                mod = (div - 1) % 26;
                colLetter = (char)(65 + mod) + colLetter;
                div = (int)((div - mod) / 26);
            }
            return colLetter;
        }

        //public byte[] IMAGE
        //{
        //    get;
        //    set;
        //}
    }

}
