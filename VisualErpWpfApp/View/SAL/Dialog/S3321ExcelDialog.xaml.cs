using AquilaErpWpfApp3.Util;
using DevExpress.Spreadsheet;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Spreadsheet;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Pur;
using ModelsLibrary.Sale;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Windows;

namespace AquilaErpWpfApp3.View.SAL.Dialog
{
    public partial class S3321ExcelDialog : DXWindow
    {

        private List<PurVo> orgList;
        private PurVo orgVo;
        private string fileDxf;

        public S3321ExcelDialog(List<PurVo> _purList)
        {
            InitializeComponent();


            this.orgList = _purList;

            excelLoad();
            //btn_link.Click += btn_link_Click;
            btn_save.Click += btn_save_Click;
            btn_close.Click += btn_close_Click;
            //excelLoad();
        }

        public S3321ExcelDialog(string _fileLoc, PurVo _orgVo, string _fileDxf)
        {
            InitializeComponent();


            //this.orgList = _purList;
            this.orgVo = _orgVo;
            //
            this.fileDxf = _fileDxf;

            //if (File.Exists(_fileLoc) == true)
            //{
                byte[] xlsxFile = File.ReadAllBytes(_fileLoc);
                this.spreadsheetControl.DocumentSource = new SpreadsheetDocumentSource(new System.IO.MemoryStream(xlsxFile), DevExpress.Spreadsheet.DocumentFormat.Xlsx);

                excelLoad();
            //}

            //excelLoad();
            //btn_link.Click += btn_link_Click;
            btn_open.Click += btn_open_Click;
            btn_save.Click += btn_save_Click;
            btn_close.Click += btn_close_Click;
            //excelLoad();
        }


        void btn_open_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                Process ExternalProcess = new Process();
                ExternalProcess.StartInfo.FileName = this.fileDxf;
                ExternalProcess.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
                ExternalProcess.Start();
                //ExternalProcess.WaitForExit();
            }
            catch (Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + this.Title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }


        }


        void btn_close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        async void excelLoad()
        {
            try
            {
                IWorkbook _workbook = this.spreadsheetControl.Document;

                int _rowCnt = 500;
                foreach (Worksheet worksheet in _workbook.Worksheets)
                {

                    worksheet.Cells["L" + 1].Value = "납기일";
                    worksheet.Cells["L" + 1].Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
                    worksheet.Cells["L" + 1].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                    worksheet.Cells["L" + 1].Font.Size = 12;
                    worksheet.Cells["L" + 1].ColumnWidth = 300;
                    worksheet.Cells["L" + 1].FillColor = Color.DarkGray;
                    worksheet.Cells["L" + 1].Alignment.WrapText = true;
                    worksheet.Cells["L" + 1].Font.Bold = true;
                    worksheet.Cells["L" + 1].Borders.SetAllBorders(Color.Black, BorderLineStyle.Thin);

                    _rowCnt = worksheet.GetUsedRange().RowCount;
                    for (int x = 2; x <= _rowCnt; x++)
                    {
                        if (string.IsNullOrEmpty(worksheet.Cells["B" + x].Value.ToString()))
                        {
                            continue;
                        }

                        worksheet.Cells["L" + x].Value = this.orgVo.IN_REQ_DT;
                        worksheet.Cells["L" + x].FillColor = Color.Yellow;
                        worksheet.Cells["L" + x].Font.Bold = true;
                        worksheet.Cells["L" + x].NumberFormat = "yyyy-MM-dd";
                    }
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
                MessageBoxResult result = WinUIMessageBox.Show("[주문등록 → 저장] 하시겠습니까?", "[" + SystemProperties.PROGRAM_TITLE + "] " + this.Title, MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {

                    if (DXSplashScreen.IsActive == false)
                    {
                        DXSplashScreen.Show<ProgressWindow>();
                    }

                    //DB 저장
                    IList<SaleVo> _saveList = new List<SaleVo>();
                    IWorkbook _workbook = this.spreadsheetControl.Document;
                    //Worksheet worksheet = workbook.Worksheets.ActiveWorksheet;

                    //Row 줄 표시
                    //WinUIMessageBox.Show(worksheet.GetUsedRange().RowCount + "", "[" + SystemProperties.PROGRAM_TITLE + "]", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);

                    int _rowCnt = 500;
                    foreach (Worksheet worksheet in _workbook.Worksheets)
                    {
                        _saveList = new List<SaleVo>();

                        //Row 줄 표시 -> 값 저장

                        _rowCnt = worksheet.GetUsedRange().RowCount;
                        for (int x = 2; x <= _rowCnt; x++)
                        {
                            if (string.IsNullOrEmpty(worksheet.Cells["B" + x].Value.ToString()))
                            {
                                continue;
                            }

                            SaleVo _saleVo = new SaleVo();
                            //_saleVo.SL_RLSE_NO = this.orgVo.SL_RLSE_NO;
                            _saleVo.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
                            _saleVo.CRE_USR_ID = SystemProperties.USER;
                            _saleVo.UPD_USR_ID = SystemProperties.USER;

                            _saleVo.SL_RLSE_NO = this.orgVo.GBN;
                            _saleVo.PUR_ORD_NO = this.orgVo.PUR_NO;
                            _saleVo.PUR_ORD_SEQ = this.orgVo.PUR_SEQ;
                            _saleVo.CNTR_NM = this.orgVo.CNTR_NM;
                            //_saleVo.CNTR_PSN_NM = this.orgVo.CNTR_PSN_NM;
                            _saleVo.FLR_NO = this.orgVo.FLR_NO;
                            //_saleVo.IN_REQ_DT = this.orgVo.IN_REQ_DT;
                          

                            // 부호
                            // 형번
                            _saleVo.SL_ITM_CD = worksheet.Cells["C" + x].Value.ToString();
                            // 가공정보
                            _saleVo.ITM_STL_PRD_DESC = worksheet.Cells["D" + x].Value.ToString();
                            // 철근규격
                            _saleVo.ITM_STL_SZ_CD = worksheet.Cells["E" + x].Value.ToString();
                            // 길이
                            _saleVo.ITM_STL_LENG = Convert.ToDouble(worksheet.Cells["F" + x].Value.ToString());
                            // 수량
                            _saleVo.A1 = Convert.ToDouble(worksheet.Cells["G" + x].Value.ToString());
                            // 총중량
                            _saleVo.SL_ITM_WGT = Convert.ToDouble(worksheet.Cells["I" + x].Value.ToString());
                            // 위치
                            _saleVo.CNTR_PSN_NM = worksheet.Cells["J" + x].Value.ToString();
                            // 강종
                            _saleVo.ITM_STL_CD = worksheet.Cells["K" + x].Value.ToString();
                            //납기일
                            _saleVo.IN_REQ_DT = Convert.ToDateTime(worksheet.Cells["L" + x].Value.ToString()).ToString("yyyy-MM-dd");

                            _saveList.Add(_saleVo);
                        }


                        //
                        //저장 DB
                        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s3321/dtl/i", new StringContent(JsonConvert.SerializeObject(_saveList), System.Text.Encoding.UTF8, "application/json")))
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
