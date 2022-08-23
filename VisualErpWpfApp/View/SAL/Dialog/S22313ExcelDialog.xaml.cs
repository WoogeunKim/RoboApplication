using DevExpress.Spreadsheet;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Sale;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Windows;
using AquilaErpWpfApp3.Util;

namespace AquilaErpWpfApp3.View.SAL.Dialog
{
    public partial class S22313ExcelDialog : DXWindow
    {
        //private static SaleOrderServiceClient saleOrderClient = SystemProperties.SaleOrderClient;
        private IList<SaleVo> TmpList;

        private string PATH = string.Empty;
        private string _AreaCd = string.Empty;
        private DateTime _SlYrMon;
        private string _BilCd = string.Empty;
        private string _GrpBilNo = string.Empty;
        private string _ClzFlg = string.Empty;

        public S22313ExcelDialog(string _path, string _areaCd, DateTime _slYrMon, string _bilCd, string _grpBilNo, string _clzFlg)
        {
            InitializeComponent();
            this.PATH = _path;
            this._AreaCd = _areaCd;
            this._SlYrMon = _slYrMon;
            this._BilCd = _bilCd;
            this._GrpBilNo = _grpBilNo;
            this._ClzFlg = _clzFlg;
            //this.DocNm.Text = _path;

            btn_link.Click += btn_link_Click;
            btn_save.Click += btn_save_Click;
            btn_close.Click += btn_close_Click;
            excelLoad();
        }

        void btn_close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        void btn_link_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", "https://hometax.go.kr/");
        }

        async void excelLoad()
        {
            try
            {
                //Excel
                IWorkbook workbook = spreadsheetControl.Document;
                using (FileStream stream = new FileStream(this.PATH, FileMode.Open))
                {
                    workbook.LoadDocument(stream, DocumentFormat.OpenXml);
                }

                Worksheet worksheet = this.spreadsheetControl.Document.Worksheets[0];

                worksheet.Cells["A1"].Value = "엑셀 업로드 양식(전자세금계산서-일반(영세율))";
                worksheet.Cells["A1"].Font.Size = 20;
                worksheet.MergeCells(worksheet.Range["A1:G1"]);

                worksheet.Cells["A2"].Value = "★주황색으로 표시된 부분은 필수입력항목으로 반드시 입력하셔야 합니다. \n★아래 '항목설명' 시트를 참고하여 작성하시기 바랍니다.";
                worksheet.Cells["A2"].RowHeight = 140;
                worksheet.Cells["A2"].Font.Color = Color.Red;
                worksheet.MergeCells(worksheet.Range["A2:L2"]);
                worksheet.Cells["A3"].Value = "★실제 업로드할 DATA는 7행부터 입력하여야 합니다. 최대 100건까지 입력이 가능하나, 발급은 최대 10건씩 처리가 됩니다.(100건 초과 자료는 처리 안됨) \n★임의로 행을 추가하거나 삭제하는 경우 파일을 제대로 읽지 못하는 경우가 있으므로, 주어진 양식안에 반드시 작성을 하시기 바랍니다.";
                worksheet.Cells["A3"].RowHeight = 140;
                worksheet.Cells["A3"].Font.Color = Color.Red;
                worksheet.MergeCells(worksheet.Range["A3:L3"]);
                worksheet.Cells["A4"].Value = "★전자(세금)계산서 종류는 엑셀 업로드 양식에 따라 해당 전자(세금)계산서 종류코드를 반드시 입력하셔야 합니다. \n★품목은 1건이상 입력해야 합니다. \n★공급받는자 등록번호는 사업자등록번호, 주민등록번호를 입력할 수 있습니다. \n 외국인인 경우 '9999999999999'를 입력하시고, 비고란에  외국인등록번호 또는 여권번호를 입력하시기 바랍니다.";
                worksheet.Cells["A4"].RowHeight = 280;
                worksheet.Cells["A4"].Font.Color = Color.Red;
                worksheet.MergeCells(worksheet.Range["A4:L4"]);

                ////Color
                CellRange header = worksheet["$A6:$BG6"];
                header.Font.Size = 8;
                header.FillColor = Color.PaleGreen;
                header.RowHeight = 140;
                CellRange header2 = worksheet["$A6,$B6,$C6,$E6,$F6,$K6,$M6,$N6,$T6,$U6,$W6,$AB6,$AC6,$BG6"];
                header2.FillColor = Color.Bisque;


                header[0].Value = "전자(세금)계산서 종류\n(01:일반, 02:영세율)";
                header[0].ColumnWidth = 500;

                header[1].Value = "작성일자";
                header[1].ColumnWidth = 200;

                header[2].Value = "공급자 등록번호 \n ('-' 없이 입력)";
                header[2].ColumnWidth = 400;

                header[4].Value = "공급자 상호";
                header[4].ColumnWidth = 300;

                header[5].Value = "공급자 성명";
                header[5].ColumnWidth = 350;

                header[10].Value = "공급받는자 등록번호 \n ('-' 없이 입력)";
                header[10].ColumnWidth = 450;

                header[12].Value = "공급받는자 상호";
                header[12].ColumnWidth = 300;

                header[13].Value = "공급받는자 성명";
                header[13].ColumnWidth = 300;

                header[19].Value = "공급가액";
                header[19].ColumnWidth = 200;

                header[20].Value = "세액";
                header[20].ColumnWidth = 200;

                header[22].Value = "일자1\n(2자리,작성\n년월 제외)";
                header[22].ColumnWidth = 200;

                header[27].Value = "공급가액1";
                header[27].ColumnWidth = 200;

                header[28].Value = "세액1";
                header[28].ColumnWidth = 100;

                header[58].Value = "영수(01)\n청구(02)";
                header[58].ColumnWidth = 200;

                header[3].Value = "공급자\n종사업장번호";
                header[3].ColumnWidth = 400;


                header[6].Value = "공급자 사업장주소";
                header[6].ColumnWidth = 400;

                header[7].Value = "공급자 업태";
                header[7].ColumnWidth = 200;

                header[8].Value = "공급자 종목";
                header[8].ColumnWidth = 200;

                header[9].Value = "공급자 이메일";
                header[9].ColumnWidth = 300;

                header[11].Value = "공급받는자\n종사업장번호";
                header[11].ColumnWidth = 400;

                header[14].Value = "공급받는자\n사업장주소";
                header[14].ColumnWidth = 300;

                header[15].Value = "공급받는자\n업태";
                header[15].ColumnWidth = 400;

                header[16].Value = "공급받는자\n종목";
                header[16].ColumnWidth = 400;

                header[17].Value = "공급받는자\n이메일1";
                header[17].ColumnWidth = 400;

                header[18].Value = "공급받는자\n이메일2";
                header[18].ColumnWidth = 400;

                header[21].Value = "비고";
                header[21].ColumnWidth = 50;

                header[23].Value = "품목1";
                header[23].ColumnWidth = 200;

                header[24].Value = "규격1";
                header[24].ColumnWidth = 250;

                header[25].Value = "수량1";
                header[25].ColumnWidth = 80;

                header[26].Value = "단가1";
                header[26].ColumnWidth = 80;

                header[29].Value = "품목비고1";
                header[29].ColumnWidth = 200;

                header[30].Value = "일자2\n(2자리,작성\n년월 제외)";
                header[30].ColumnWidth = 200;

                header[31].Value = "품목2";
                header[31].ColumnWidth = 200;

                header[32].Value = "규격2";
                header[32].ColumnWidth = 250;

                header[33].Value = "수량2";
                header[33].ColumnWidth = 80;

                header[34].Value = "단가2";
                header[34].ColumnWidth = 80;

                header[35].Value = "공급가액2";
                header[35].ColumnWidth = 200;

                header[36].Value = "세액2";
                header[36].ColumnWidth = 100;

                header[37].Value = "품목비고2";
                header[37].ColumnWidth = 200;

                header[38].Value = "일자3\n(2자리,작성\n년월 제외)";
                header[38].ColumnWidth = 200;

                header[39].Value = "품목3";
                header[39].ColumnWidth = 200;

                header[40].Value = "규격3";
                header[40].ColumnWidth = 250;

                header[41].Value = "수량3";
                header[41].ColumnWidth = 80;

                header[42].Value = "단가3";
                header[42].ColumnWidth = 80;

                header[43].Value = "공급가액3";
                header[43].ColumnWidth = 200;

                header[44].Value = "세액3";
                header[44].ColumnWidth = 100;

                header[45].Value = "품목비고3";
                header[45].ColumnWidth = 200;


                header[46].Value = "일자4\n(2자리,작성\n년월 제외)";
                header[46].ColumnWidth = 200;

                header[47].Value = "품목4";
                header[47].ColumnWidth = 200;

                header[48].Value = "규격4";
                header[48].ColumnWidth = 250;

                header[49].Value = "수량4";
                header[49].ColumnWidth = 80;

                header[50].Value = "단가4";
                header[50].ColumnWidth = 80;

                header[51].Value = "공급가액4";
                header[51].ColumnWidth = 200;

                header[52].Value = "세액4";
                header[52].ColumnWidth = 100;

                header[53].Value = "품목비고4";
                header[53].ColumnWidth = 200;

                header[54].Value = "현금";
                header[54].ColumnWidth = 200;

                header[55].Value = "수표";
                header[55].ColumnWidth = 200;

                header[56].Value = "어음";
                header[56].ColumnWidth = 200;

                header[57].Value = "외상미수금";
                header[57].ColumnWidth = 200;


                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s22313/mst/excel", new StringContent(JsonConvert.SerializeObject(new SaleVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD, AREA_CD = this._AreaCd, SL_DC_YRMON = (this._SlYrMon).ToString("yyyyMM"), BIL_CD = this._BilCd, GRP_BIL_NO = this._GrpBilNo, CLZ_FLG = this._ClzFlg }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        TmpList = JsonConvert.DeserializeObject<IEnumerable<SaleVo>>(await response.Content.ReadAsStringAsync()).Cast<SaleVo>().ToList();
                    }
                }
                    //TmpList = saleOrderClient.S2225SelectExportExcelList(new JobVo() { AREA_CD = this._AreaCd, SL_DC_YRMON = (this._SlYrMon).ToString("yyyyMM"), BIL_CD = this._BilCd, GRP_BIL_NO = this._GrpBilNo, CLZ_FLG = this._ClzFlg });



                    int rowNum;


                //int totalNumber = TmpList.Count;
                //int pageNumber = totalNumber / 100;
                //int remainder = 0;
                //int start = 0;
                //int end = 0;

                ////나머지가 있을경우 나머지 숫자를 저장하고 페이지 넘버를 하나더 추가
                //if (totalNumber % 100 > 0)
                //{
                //    remainder = totalNumber % 100;
                //    pageNumber = pageNumber + 1;
                //}

                //S2225Vo[] arrList = new S2225Vo[totalNumber];
                //arrList = TmpList.ToArray();

                //IList<S2225Vo> voList = new List<S2225Vo>();


                //for (int x = 0; x < pageNumber; x++)
                //{
                //    start = ((x + 1) * 100) - 99;
                //    end = (x + 1) * 100;


                //    //나머지가 있을 경우 
                //    if (x == (pageNumber - 1) && remainder > 0)
                //    {    
                //        end = ((x + 1) * 100) + (remainder-100);

                //    }




                //    for (int y = start; y < end; y++)
                //    {
                //        voList.Add(arrList[y]);
                //    }


                //    voList.Clear();


                //}









                for (int x = 0; x < TmpList.Count; x++)
                {
                    rowNum = x + 6;
                    worksheet[rowNum, 0].Value = (string)TmpList[x].XLS_A;
                    worksheet[rowNum, 1].Value = (string)TmpList[x].XLS_B;
                    worksheet[rowNum, 2].Value = (string)TmpList[x].XLS_C;
                    worksheet[rowNum, 3].Value = (string)TmpList[x].XLS_D;
                    worksheet[rowNum, 4].Value = (string)TmpList[x].XLS_E;
                    worksheet[rowNum, 5].Value = (string)TmpList[x].XLS_F;
                    worksheet[rowNum, 6].Value = (string)TmpList[x].XLS_G;
                    worksheet[rowNum, 7].Value = (string)TmpList[x].XLS_H;
                    worksheet[rowNum, 8].Value = (string)TmpList[x].XLS_I;
                    worksheet[rowNum, 9].Value = (string)TmpList[x].XLS_J;
                    worksheet[rowNum, 10].Value = (string)TmpList[x].XLS_K;
                    worksheet[rowNum, 11].Value = (string)TmpList[x].XLS_L;
                    worksheet[rowNum, 12].Value = (string)TmpList[x].XLS_M;
                    worksheet[rowNum, 13].Value = (string)TmpList[x].XLS_N;
                    worksheet[rowNum, 14].Value = (string)TmpList[x].XLS_O;
                    worksheet[rowNum, 15].Value = (string)TmpList[x].XLS_P;
                    worksheet[rowNum, 16].Value = (string)TmpList[x].XLS_Q;
                    worksheet[rowNum, 17].Value = (string)TmpList[x].XLS_R;
                    worksheet[rowNum, 18].Value = (string)TmpList[x].XLS_S;
                    worksheet[rowNum, 19].Value = Convert.ToDecimal(TmpList[x].XLS_T).ToString();
                    worksheet[rowNum, 20].Value = Convert.ToDecimal(TmpList[x].XLS_U).ToString();
                    worksheet[rowNum, 21].Value = (string)TmpList[x].XLS_V;
                    worksheet[rowNum, 22].Value = (string)TmpList[x].XLS_W;
                    worksheet[rowNum, 23].Value = (string)TmpList[x].XLS_X;
                    worksheet[rowNum, 24].Value = (string)TmpList[x].XLS_Y;
                    worksheet[rowNum, 25].Value = (string)TmpList[x].XLS_Z;
                    worksheet[rowNum, 26].Value = (string)TmpList[x].XLS_AA;
                    worksheet[rowNum, 27].Value = Convert.ToDecimal(TmpList[x].XLS_AB).ToString();
                    worksheet[rowNum, 28].Value = Convert.ToDecimal(TmpList[x].XLS_AC).ToString();
                    //worksheet[rowNum, 29].Value = (string)TmpList[x].XLS_AD;
                    //worksheet[rowNum, 30].Value = (string)TmpList[x].XLS_AE;
                    //worksheet[rowNum, 31].Value = (string)TmpList[x].XLS_AF;
                    //worksheet[rowNum, 32].Value = (string)TmpList[x].XLS_AG;
                    //worksheet[rowNum, 33].Value = (string)TmpList[x].XLS_AH;
                    //worksheet[rowNum, 34].Value = (string)TmpList[x].XLS_AI;
                    //worksheet[rowNum, 35].Value = (string)TmpList[x].XLS_AJ;
                    //worksheet[rowNum, 36].Value = (string)TmpList[x].XLS_AK;
                    //worksheet[rowNum, 37].Value = (string)TmpList[x].XLS_AL;
                    //worksheet[rowNum, 38].Value = (string)TmpList[x].XLS_AM;
                    //worksheet[rowNum, 39].Value = (string)TmpList[x].XLS_AN;
                    //worksheet[rowNum, 40].Value = (string)TmpList[x].XLS_AO;
                    //worksheet[rowNum, 41].Value = (string)TmpList[x].XLS_AP;
                    //worksheet[rowNum, 42].Value = (string)TmpList[x].XLS_AQ;
                    //worksheet[rowNum, 43].Value = (string)TmpList[x].XLS_AR;
                    //worksheet[rowNum, 44].Value = (string)TmpList[x ].XLS_AS;
                    //worksheet[rowNum, 45].Value = (string)TmpList[x].XLS_AT;
                    //worksheet[rowNum, 46].Value = (string)TmpList[x].XLS_AU;
                    //worksheet[rowNum, 47].Value = (string)TmpList[x].XLS_AV;
                    //worksheet[rowNum, 48].Value = (string)TmpList[x].XLS_AW;
                    //worksheet[rowNum, 49].Value = (string)TmpList[x].XLS_AX;
                    //worksheet[rowNum, 50].Value = (string)TmpList[x].XLS_AY;
                    //worksheet[rowNum, 51].Value = (string)TmpList[x].XLS_AZ;
                    //worksheet[rowNum, 52].Value = (string)TmpList[x].XLS_BA;
                    //worksheet[rowNum, 53].Value = (string)TmpList[x].XLS_BB;
                    //worksheet[rowNum, 54].Value = (string)TmpList[x].XLS_BC;
                    //worksheet[rowNum, 55].Value = (string)TmpList[x].XLS_BD;
                    //worksheet[rowNum, 56].Value = (string)TmpList[x].XLS_BE;
                    //worksheet[rowNum, 57].Value = (string)TmpList[x].XLS_BF;
                    worksheet[rowNum, 58].Value = (string)TmpList[x].XLS_BG;
                }










                //header.Style = workbook.Styles[BuiltInStyleId.Input];
                //range.FillColor = DevExpress.Utils.DXColor.LightGray;
                //worksheet.AutoFilter.Apply(range);



            }
            catch (Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }

        async void btn_save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DevExpress.Xpf.Dialogs.DXFolderBrowserDialog dialog = new DevExpress.Xpf.Dialogs.DXFolderBrowserDialog();
                //System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
                dialog.ShowNewFolderButton = true;
                dialog.Description = "저장 폴더를 선택 해 주세요.";

                if (dialog.ShowDialog() == true)
                {
                    DXSplashScreen.Show<ProgressWindow>();

                    string fn = dialog.SelectedPath + "\\" + System.DateTime.Now.ToString("yyyyMMddHHmmss");

                    //pivotGrid.ExportToXlsx(fn);
                    //DXSplashScreen.Close();
                    //Process.Start(fn);



                    int totalNumber = TmpList.Count;
                    int pageNumber = totalNumber / 100;
                    int remainder = 0;
                    int start = 0;
                    int end = 0;
                    int fileEnd = 100;


                    Microsoft.Office.Interop.Excel.Application _eApp;
                    Microsoft.Office.Interop.Excel.Workbook _eWorkbook;
                    Microsoft.Office.Interop.Excel.Worksheet _eWorkSheet;
                    string[,] _data;

                    //나머지가 있을경우 나머지 숫자를 저장하고 페이지 넘버를 하나더 추가
                    if (totalNumber % 100 > 0)
                    {
                        remainder = totalNumber % 100;
                        pageNumber = pageNumber + 1;
                    }

                    SaleVo[] arrList = new SaleVo[totalNumber];
                    arrList = TmpList.ToArray();

                    IList<SaleVo> voList = new List<SaleVo>();


                    for (int x = 0; x < pageNumber; x++)
                    {
                        start = ((x + 1) * 100) - 100;
                        end = (x + 1) * 100;


                        //나머지가 있을 경우 
                        if (x == (pageNumber - 1) && remainder > 0)
                        {
                            end = ((x + 1) * 100) + (remainder - 101);
                            fileEnd = remainder;

                        }




                        for (int y = start; y <= end; y++)
                        {
                            voList.Add(arrList[y]);
                        }


                        //엑셀 저장
                        _eApp = new Microsoft.Office.Interop.Excel.Application();
                        _eWorkbook = _eApp.Workbooks.Add(true);
                        _eWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)_eWorkbook.Sheets[1]; // Excel Sheet 배열은 1부터 시작한다.

                        _data = new string[120, 59];

                        try
                        {




                            int rowNum = 0;
                            for (int z = 0; z < fileEnd; z++)
                            {

                                rowNum = z + 6;
                                _data[rowNum, 0] = (string)voList[z].XLS_A;
                                _data[rowNum, 1] = (string)voList[z].XLS_B;
                                _data[rowNum, 2] = (string)voList[z].XLS_C;
                                _data[rowNum, 3] = (string)voList[z].XLS_D;
                                _data[rowNum, 4] = (string)voList[z].XLS_E;
                                _data[rowNum, 5] = (string)voList[z].XLS_F;
                                _data[rowNum, 6] = (string)voList[z].XLS_G;
                                _data[rowNum, 7] = (string)voList[z].XLS_H;
                                _data[rowNum, 8] = (string)voList[z].XLS_I;
                                _data[rowNum, 9] = (string)voList[z].XLS_J;
                                _data[rowNum, 10] = (string)voList[z].XLS_K;
                                _data[rowNum, 11] = (string)voList[z].XLS_L;
                                _data[rowNum, 12] = (string)voList[z].XLS_M;
                                _data[rowNum, 13] = (string)voList[z].XLS_N;
                                _data[rowNum, 14] = (string)voList[z].XLS_O;
                                _data[rowNum, 15] = (string)voList[z].XLS_P;
                                _data[rowNum, 16] = (string)voList[z].XLS_Q;
                                _data[rowNum, 17] = (string)voList[z].XLS_R;
                                _data[rowNum, 18] = (string)voList[z].XLS_S;
                                _data[rowNum, 19] = Convert.ToDecimal(voList[z].XLS_T).ToString();
                                _data[rowNum, 20] = Convert.ToDecimal(voList[z].XLS_U).ToString();
                                _data[rowNum, 21] = (string)voList[z].XLS_V;
                                _data[rowNum, 22] = (string)voList[z].XLS_W;
                                _data[rowNum, 23] = (string)voList[z].XLS_X;
                                _data[rowNum, 24] = (string)voList[z].XLS_Y;
                                _data[rowNum, 25] = (string)voList[z].XLS_Z;
                                _data[rowNum, 26] = (string)voList[z].XLS_AA;
                                _data[rowNum, 27] = Convert.ToDecimal(voList[z].XLS_AB).ToString();
                                _data[rowNum, 28] = Convert.ToDecimal(voList[z].XLS_AC).ToString();
                                _data[rowNum, 58] = (string)voList[z].XLS_BG;
                            }



                            _eWorkSheet.get_Range("A1:BG106").Value2 = _data;


                            _eWorkSheet.get_Range("$A6:$BG6").Interior.Color = Color.PaleGreen;
                            _eWorkSheet.get_Range("$A6:$A106,$B6:$B106,$C6:$C106,$E6:$E106,$F6:$F106,$K6:$K106,$M6:$M106,$N6:$N106,$T6:$T106,$U6:$U106,$W6:$W106,$AB6:$AB106,$AC6:$AC106,$BG6:$BG106").Interior.Color = Color.Bisque;



                            _eWorkSheet.get_Range("A6").ColumnWidth = 18.78;
                            _eWorkSheet.get_Range("B6").ColumnWidth = 8.89;
                            _eWorkSheet.get_Range("C6").ColumnWidth = 13.78;
                            _eWorkSheet.get_Range("D6").ColumnWidth = 12;
                            _eWorkSheet.get_Range("E6").ColumnWidth = 12;

                            _eWorkSheet.get_Range("F6").ColumnWidth = 10;
                            _eWorkSheet.get_Range("G6").ColumnWidth = 23.11;
                            _eWorkSheet.get_Range("H6").ColumnWidth = 10;
                            _eWorkSheet.get_Range("I6").ColumnWidth = 10;
                            _eWorkSheet.get_Range("J6").ColumnWidth = 16.56;

                            _eWorkSheet.get_Range("K6").ColumnWidth = 17.78;
                            _eWorkSheet.get_Range("L6").ColumnWidth = 11.33;
                            _eWorkSheet.get_Range("M6").ColumnWidth = 13.78;
                            _eWorkSheet.get_Range("N6").ColumnWidth = 13.78;
                            _eWorkSheet.get_Range("O6").ColumnWidth = 19.67;

                            _eWorkSheet.get_Range("P6").ColumnWidth = 13.78;
                            _eWorkSheet.get_Range("Q6").ColumnWidth = 13.78;
                            _eWorkSheet.get_Range("R6").ColumnWidth = 16.67;
                            _eWorkSheet.get_Range("S6").ColumnWidth = 18.89;
                            _eWorkSheet.get_Range("T6").ColumnWidth = 8.89;

                            _eWorkSheet.get_Range("U6").ColumnWidth = 6.67;
                            _eWorkSheet.get_Range("V6").ColumnWidth = 4.22;
                            _eWorkSheet.get_Range("W6").ColumnWidth = 10.44;
                            _eWorkSheet.get_Range("X6").ColumnWidth = 10;
                            _eWorkSheet.get_Range("Y6").ColumnWidth = 13;

                            _eWorkSheet.get_Range("Z6").ColumnWidth = 5.33;
                            _eWorkSheet.get_Range("AA6").ColumnWidth = 5.78;
                            _eWorkSheet.get_Range("AB6").ColumnWidth = 8.44;
                            _eWorkSheet.get_Range("AC6").ColumnWidth = 5.11;
                            _eWorkSheet.get_Range("AD6").ColumnWidth = 8.44;

                            _eWorkSheet.get_Range("AE6").ColumnWidth = 10.33;
                            _eWorkSheet.get_Range("AF6").ColumnWidth = 10;
                            _eWorkSheet.get_Range("AG6").ColumnWidth = 13;
                            _eWorkSheet.get_Range("AH6").ColumnWidth = 5.11;
                            _eWorkSheet.get_Range("AI6").ColumnWidth = 5.78;

                            _eWorkSheet.get_Range("AJ6").ColumnWidth = 8.44;
                            _eWorkSheet.get_Range("AK6").ColumnWidth = 5.11;
                            _eWorkSheet.get_Range("AL6").ColumnWidth = 8.44;
                            _eWorkSheet.get_Range("AM6").ColumnWidth = 10;
                            _eWorkSheet.get_Range("AN6").ColumnWidth = 9.33;

                            _eWorkSheet.get_Range("AO6").ColumnWidth = 10.89;
                            _eWorkSheet.get_Range("AP6").ColumnWidth = 5.11;
                            _eWorkSheet.get_Range("AQ6").ColumnWidth = 5.78;
                            _eWorkSheet.get_Range("AR6").ColumnWidth = 8.44;
                            _eWorkSheet.get_Range("AS6").ColumnWidth = 5.11;

                            _eWorkSheet.get_Range("AT6").ColumnWidth = 8.44;
                            _eWorkSheet.get_Range("AU6").ColumnWidth = 9.56;
                            _eWorkSheet.get_Range("AV6").ColumnWidth = 9.33;
                            _eWorkSheet.get_Range("AW6").ColumnWidth = 10.89;
                            _eWorkSheet.get_Range("AX6").ColumnWidth = 5.11;

                            _eWorkSheet.get_Range("AY6").ColumnWidth = 5.78;
                            _eWorkSheet.get_Range("AZ6").ColumnWidth = 8.44;
                            _eWorkSheet.get_Range("BA6").ColumnWidth = 5.11;
                            _eWorkSheet.get_Range("BB6").ColumnWidth = 8.44;
                            _eWorkSheet.get_Range("BC6").ColumnWidth = 7.78;

                            _eWorkSheet.get_Range("BD6").ColumnWidth = 6.78;
                            _eWorkSheet.get_Range("BE6").ColumnWidth = 7.78;
                            _eWorkSheet.get_Range("BF6").ColumnWidth = 9.33;
                            _eWorkSheet.get_Range("BG6").ColumnWidth = 8;


                            _eWorkSheet.get_Range("A6").Value = "전자(세금)계산서 종류\n(01:일반, 02:영세율)";
                            _eWorkSheet.get_Range("B6").Value = "작성일자";
                            _eWorkSheet.get_Range("C6").Value = "공급자 등록번호 \n ('-' 없이 입력)";
                            _eWorkSheet.get_Range("D6").Value = "공급자\n종사업장번호";
                            _eWorkSheet.get_Range("E6").Value = "공급자 상호";

                            _eWorkSheet.get_Range("F6").Value = "공급자 성명";
                            _eWorkSheet.get_Range("G6").Value = "공급자 사업장주소";
                            _eWorkSheet.get_Range("H6").Value = "공급자 업태";
                            _eWorkSheet.get_Range("I6").Value = "공급자 종목";
                            _eWorkSheet.get_Range("J6").Value = "공급자 이메일";

                            _eWorkSheet.get_Range("K6").Value = "공급받는자 등록번호 \n ('-' 없이 입력)";
                            _eWorkSheet.get_Range("L6").Value = "공급받는자\n종사업장번호";
                            _eWorkSheet.get_Range("M6").Value = "공급받는자 상호";
                            _eWorkSheet.get_Range("N6").Value = "공급받는자 성명";
                            _eWorkSheet.get_Range("O6").Value = "공급받는자\n사업장주소";

                            _eWorkSheet.get_Range("P6").Value = "공급받는자\n업태";
                            _eWorkSheet.get_Range("Q6").Value = "공급받는자\n종목";
                            _eWorkSheet.get_Range("R6").Value = "공급받는자\n이메일1";
                            _eWorkSheet.get_Range("S6").Value = "공급받는자\n이메일2";
                            _eWorkSheet.get_Range("T6").Value = "공급가액";

                            _eWorkSheet.get_Range("U6").Value = "세액";
                            _eWorkSheet.get_Range("V6").Value = "비고";
                            _eWorkSheet.get_Range("W6").Value = "일자1\n(2자리,작성\n년월 제외)";
                            _eWorkSheet.get_Range("X6").Value = "품목1";
                            _eWorkSheet.get_Range("Y6").Value = "규격1";

                            _eWorkSheet.get_Range("Z6").Value = "수량1";
                            _eWorkSheet.get_Range("AA6").Value = "단가1";
                            _eWorkSheet.get_Range("AB6").Value = "공급가액1";
                            _eWorkSheet.get_Range("AC6").Value = "세액1";
                            _eWorkSheet.get_Range("AD6").Value = "품목비고1";

                            _eWorkSheet.get_Range("AE6").Value = "일자2\n(2자리,작성\n년월 제외)";
                            _eWorkSheet.get_Range("AF6").Value = "품목2";
                            _eWorkSheet.get_Range("AG6").Value = "규격2";
                            _eWorkSheet.get_Range("AH6").Value = "수량2";
                            _eWorkSheet.get_Range("AI6").Value = "단가2";

                            _eWorkSheet.get_Range("AJ6").Value = "공급가액2";
                            _eWorkSheet.get_Range("AK6").Value = "세액2";
                            _eWorkSheet.get_Range("AL6").Value = "품목비고2";
                            _eWorkSheet.get_Range("AM6").Value = "일자3\n(2자리,작성\n년월 제외)";
                            _eWorkSheet.get_Range("AN6").Value = "품목3";

                            _eWorkSheet.get_Range("AO6").Value = "규격3";
                            _eWorkSheet.get_Range("AP6").Value = "수량3";
                            _eWorkSheet.get_Range("AQ6").Value = "단가3";
                            _eWorkSheet.get_Range("AR6").Value = "공급가액3";
                            _eWorkSheet.get_Range("AS6").Value = "세액3";

                            _eWorkSheet.get_Range("AT6").Value = "품목비고3";
                            _eWorkSheet.get_Range("AU6").Value = "일자4\n(2자리,작성\n년월 제외)";
                            _eWorkSheet.get_Range("AV6").Value = "품목4";
                            _eWorkSheet.get_Range("AW6").Value = "규격4";
                            _eWorkSheet.get_Range("AX6").Value = "수량4";

                            _eWorkSheet.get_Range("AY6").Value = "단가4";
                            _eWorkSheet.get_Range("AZ6").Value = "공급가액4";
                            _eWorkSheet.get_Range("BA6").Value = "세액4";
                            _eWorkSheet.get_Range("BB6").Value = "품목비고4";
                            _eWorkSheet.get_Range("BC6").Value = "현금";

                            _eWorkSheet.get_Range("BD6").Value = "수표";
                            _eWorkSheet.get_Range("BE6").Value = "어음";
                            _eWorkSheet.get_Range("BF6").Value = "외상미수금";
                            _eWorkSheet.get_Range("BG6").Value = "영수(01)\n청구(02)";

                            _eWorkSheet.get_Range("A1").Value = "엑셀 업로드 양식(전자세금계산서-일반(영세율))";
                            _eWorkSheet.get_Range("A1").Font.Size = 24;
                            _eWorkSheet.get_Range("A1:G1").Merge();

                            _eWorkSheet.get_Range("A2").Value = "★주황색으로 표시된 부분은 필수입력항목으로 반드시 입력하셔야 합니다. \n★아래 '항목설명' 시트를 참고하여 작성하시기 바랍니다.";
                            _eWorkSheet.get_Range("A1").Font.Size = 14;
                            _eWorkSheet.get_Range("A2").RowHeight = 42;
                            _eWorkSheet.get_Range("A2").Font.Color = Color.Red;
                            _eWorkSheet.get_Range("A2:L2").Merge();

                            _eWorkSheet.get_Range("A3").Value = "★실제 업로드할 DATA는 7행부터 입력하여야 합니다. 최대 100건까지 입력이 가능하나, 발급은 최대 10건씩 처리가 됩니다.(100건 초과 자료는 처리 안됨) \n★임의로 행을 추가하거나 삭제하는 경우 파일을 제대로 읽지 못하는 경우가 있으므로, 주어진 양식안에 반드시 작성을 하시기 바랍니다.";
                            _eWorkSheet.get_Range("A1").Font.Size = 14;
                            _eWorkSheet.get_Range("A3").RowHeight = 45;
                            _eWorkSheet.get_Range("A3").Font.Color = Color.Red;
                            _eWorkSheet.get_Range("A3:L3").Merge();

                            _eWorkSheet.get_Range("A4").Value = "★전자(세금)계산서 종류는 엑셀 업로드 양식에 따라 해당 전자(세금)계산서 종류코드를 반드시 입력하셔야 합니다. \n★품목은 1건이상 입력해야 합니다. \n★공급받는자 등록번호는 사업자등록번호, 주민등록번호를 입력할 수 있습니다. \n 외국인인 경우 '9999999999999'를 입력하시고, 비고란에  외국인등록번호 또는 여권번호를 입력하시기 바랍니다.";
                            _eWorkSheet.get_Range("A1").Font.Size = 14;
                            _eWorkSheet.get_Range("A4").RowHeight = 90.75;
                            _eWorkSheet.get_Range("A4").Font.Color = Color.Red;
                            _eWorkSheet.get_Range("A4:L4").Merge();

                            _eWorkSheet.get_Range("A5").RowHeight = 14.25;
                            _eWorkSheet.get_Range("A6").RowHeight = 47.25;
                            _eWorkSheet.get_Range("$A7:BG106").RowHeight = 24;
                            _eWorkSheet.get_Range("$A6:BG106").Font.Size = 11;
                            _eWorkSheet.get_Range("$A6:BG106").BorderAround2();


                            IList<string> strList = new List<string>();
                            string str = "";

                            for (int _x = 65; _x < 91; _x++)
                            {
                                str = "$" + Convert.ToChar(_x).ToString() + "6:" + Convert.ToChar(_x).ToString() + "106";
                                strList.Add(str);
                            }

                            for (int _x = 65; _x < 91; _x++)
                            {

                                str = "$A" + Convert.ToChar(_x).ToString() + "6:A" + Convert.ToChar(_x).ToString() + "106";
                                strList.Add(str);
                            }

                            for (int _x = 65; _x < 72; _x++)
                            {

                                str = "$B" + Convert.ToChar(_x).ToString() + "6:B" + Convert.ToChar(_x).ToString() + "106";
                                strList.Add(str);
                            }



                            for (int _x = 0; _x < strList.Count; _x++)
                            {
                                _eWorkSheet.get_Range(strList[_x]).Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlDot;
                                _eWorkSheet.get_Range(strList[_x]).Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight).LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlDot;
                                _eWorkSheet.get_Range(strList[_x]).Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideHorizontal).LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                                _eWorkSheet.get_Range(strList[_x]).Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlInsideVertical).LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                            }


                            strList = new List<string>();
                            str = "";

                            for (int _x = 65; _x < 91; _x++)
                            {
                                str = "$" + Convert.ToChar(_x).ToString() + "6";
                                strList.Add(str);
                            }

                            for (int _x = 65; _x < 91; _x++)
                            {

                                str = "$A" + Convert.ToChar(_x).ToString() + "6";
                                strList.Add(str);
                            }

                            for (int _x = 65; _x < 72; _x++)
                            {

                                str = "$B" + Convert.ToChar(_x).ToString() + "6";
                                strList.Add(str);
                            }

                            for (int _x = 0; _x < strList.Count; _x++)
                            {
                                _eWorkSheet.get_Range(strList[_x]).Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                                _eWorkSheet.get_Range(strList[_x]).Borders.get_Item(Microsoft.Office.Interop.Excel.XlBordersIndex.xlEdgeRight).LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                            }

                            _eWorkbook.Windows[1].Zoom = 75;

                            // FileFormat에 XlFileFormat.xlWorkbookDefault을 입력하면 .xlsx ( 통합양식 )으로 처리되고 Excel.XlFileFormat.xlWorkbookNormal을 입력하면 .xls ( 2003 이하? )으로 처리된다.
                            //_eWorkbook.SaveAs(saveFileDialog1.FileName, Excel.XlFileFormat.xlWorkbookDefault, Type.Missing, Type.Missing, false, false, Excel.XlSaveAsAccessMode.xlShared, false, false, Type.Missing, Type.Missing, Type.Missing);
                            //저장파일
                            _eWorkbook.CheckCompatibility = false;
                            _eWorkbook.SaveAs(fn + "_" + (x + 1) + ".xls", Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, Type.Missing, Type.Missing, false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                            
                        }
                        catch (Exception eLog)
                        {
                            
                            WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                            return;
                        }
                        finally
                        {
                            // 종료할때는 Excel Application을 종료해준다.
                            // 해당 처리를 안하면 보이지 않은 엑셀이 종료되지 않음
                            _eWorkbook.Close(false, Type.Missing, Type.Missing);
                            _eApp.Quit();
                        }

                        voList.Clear();

                    }


                    int _Num = 0;
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s22313/mst/excel/u", new StringContent(JsonConvert.SerializeObject(new SaleVo() {CHNL_CD = SystemProperties.USER_VO.CHNL_CD, AREA_CD = this._AreaCd, SL_DC_YRMON = (this._SlYrMon).ToString("yyyyMM"), BIL_CD = this._BilCd, GRP_BIL_NO = this._GrpBilNo, CLZ_FLG = "Y" }), System.Text.Encoding.UTF8, "application/json")))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string result = await response.Content.ReadAsStringAsync();
                            if (int.TryParse(result, out _Num) == false)
                            {
                                //실패
                                WinUIMessageBox.Show(result, "[" + SystemProperties.PROGRAM_TITLE + "]", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                        }
                    }
                    //saleOrderClient.S2225UpdateBill(new JobVo() { AREA_CD = this._AreaCd, SL_DC_YRMON = (this._SlYrMon).ToString("yyyyMM"), BIL_CD = this._BilCd, GRP_BIL_NO = this._GrpBilNo, CLZ_FLG = "Y" });
                    DXSplashScreen.Close();
                    WinUIMessageBox.Show(dialog.SelectedPath + "에 저장 되었습니다.", "[" + SystemProperties.PROGRAM_TITLE + "]", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                    System.Diagnostics.Process.Start(dialog.SelectedPath);


                }


            }
            catch (Exception eLog)
            {
                DXSplashScreen.Close();
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }

        }



    }
}
