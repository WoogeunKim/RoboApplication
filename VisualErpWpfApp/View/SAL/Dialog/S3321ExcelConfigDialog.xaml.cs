using AquilaErpWpfApp3.Util;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Spreadsheet;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Pur;
using System;
using System.IO;
using System.Windows;

namespace AquilaErpWpfApp3.View.SAL.Dialog
{
    public partial class S3321ExcelConfigDialog : DXWindow
    {

        private string filePath;
        //private List<PurVo> orgList;
        //private PurVo orgVo;

        //public S3321ExcelConfigDialog(List<PurVo> _purList)
        //{
        //    InitializeComponent();


        //    this.orgList = _purList;

        //    excelLoad();
        //    //btn_link.Click += btn_link_Click;
        //    btn_save.Click += btn_save_Click;
        //    btn_close.Click += btn_close_Click;
        //    //excelLoad();
        //}

        public S3321ExcelConfigDialog(string _fileLoc)
        {
            InitializeComponent();

            this.filePath = _fileLoc;
            //this.orgList = _purList;
            //this.orgVo = _orgVo;
            //if (File.Exists(_fileLoc) == true)
            //{
                byte[] xlsxFile = File.ReadAllBytes(_fileLoc);
                this.spreadsheetControl.DocumentSource = new SpreadsheetDocumentSource(new System.IO.MemoryStream(xlsxFile), DevExpress.Spreadsheet.DocumentFormat.Xlsx);
            //}


            //excelLoad();
            //btn_link.Click += btn_link_Click;
            btn_save.Click += btn_save_Click;
            btn_close.Click += btn_close_Click;
            //excelLoad();
        }


        void btn_close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //async void excelLoad()
        //{
        //    try
        //    {
        //        Workbook _book;
        //        Workbook _result;
        //        List<Workbook> _workbooks = new List<Workbook>();
        //        foreach (PurVo _item in this.orgList)
        //        {
        //            _book = new Workbook();
        //            _book.LoadDocument(_item.FLR_FILE, DocumentFormat.Xlsx);
        //            _workbooks.Add(_book);

        //            //File.WriteAllBytes(Path.Combine(_di.FullName, "TMP_" + System.DateTime.Now.ToString("yyyyMMddhhmmssfff") + "_" + _item.FLR_NM), _item.FLR_FILE);
        //        }

        //        //Excel 파일 병합 하기
        //        _result = Workbook.Merge(_workbooks.ToArray());

        //        this.spreadsheetControl.LoadDocument(new System.IO.MemoryStream(_result.SaveDocument(DevExpress.Spreadsheet.DocumentFormat.Xlsx)), DevExpress.Spreadsheet.DocumentFormat.Xlsx);
        //    }
        //    catch (Exception eLog)
        //    {
        //        WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "] " + this.Title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
        //        return;
        //    }
        //}

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
                MessageBoxResult result = WinUIMessageBox.Show("저장 하시겠습니까?", "[" + SystemProperties.PROGRAM_TITLE + "] " + this.Title, MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {

                    if (DXSplashScreen.IsActive == false)
                    {
                        DXSplashScreen.Show<ProgressWindow>();
                    }


                    this.spreadsheetControl.SaveDocument(filePath);




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
