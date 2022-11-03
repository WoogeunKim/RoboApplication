using AquilaErpWpfApp3.Util;
using DevExpress.Spreadsheet;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Code;
using ModelsLibrary.Pur;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using DevExpress.Xpf.Spreadsheet;

namespace AquilaErpWpfApp3.View.PUR.Dialog
{
    /// <summary>
    /// P4402ExcelDialog.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class P4402ExcelDialog : DXWindow
    {
        PurVo purVo = new PurVo();

        public P4402ExcelDialog()
        {
            InitializeComponent();

            SYSTEM_CODE_VO();
        }

        public P4402ExcelDialog(PurVo vo)
        {
            InitializeComponent();

            purVo = vo;

            SYSTEM_CODE_VO();
        }

        public async void SYSTEM_CODE_VO()
        {
            //CO_NO
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("p4402/dig/co", new StringContent(JsonConvert.SerializeObject(new PurVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.combo_CO_NO.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<PurVo>>(await response.Content.ReadAsStringAsync()).Cast<PurVo>().ToList();
                }
            }

            // 파일 넣기
            Stream streamFile;
            streamFile = new System.IO.MemoryStream(purVo.PRC_FILE);
            this.spreadsheetControl1.DocumentSource = new SpreadsheetDocumentSource(streamFile, DevExpress.Spreadsheet.DocumentFormat.Xlsx);
        }
        // 확인 버튼 이벤트
        private async void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValueCheckd())
            {
                try
                {
                    DXSplashScreen.Show<ProgressWindow>();

                    List<PurVo> _saveVo = new List<PurVo>();

                    //Excel 세팅
                    IWorkbook workbook = this.spreadsheetControl1.Document;
                    Worksheet worksheet = workbook.Worksheets.ActiveWorksheet;

                    int _nCnt = 4;
                    int _yCnt = 0;

                    int _xCnt = 67; 
                    int _xCcnt = 64;

                    string _CMPO_RMK = string.Empty;

                    // Y (평량) 계산
                    while (true)
                    {
                        int i = 0;
                        if (int.TryParse(worksheet.Cells["A" + (_nCnt + _yCnt)].Value.ToString(), out i))
                        {
                            _yCnt++;
                        }
                        else
                        {
                            break;
                        }
                    }

                    // X (대,중분류) 계산
                    while (true)
                    {
                        string _xTtxt = "";
                        string _xTxt = Convert.ToChar(_xCnt).ToString();
                        if (_xCcnt > 64)
                        {
                            _xTtxt = Convert.ToChar(_xCcnt).ToString();
                        }

                        if(!string.IsNullOrEmpty(worksheet.Cells[_xTtxt + _xTxt + "3"].Value.ToString()))
                        {
                            _xCnt++;

                            if(_xCnt > 90)
                            {
                                _xCcnt++;
                                _xCnt = 65;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    int _ySum = (((_xCcnt - 64) * 26) + (_xCnt - 67)) / 2;


                    int _xCell = 67;
                    int _xCcell = 64;

                    // cell 읽기
                    for (int y = 0; y < _ySum; y++)
                    {

                        string _xTtxt = "";
                        if (_xCcell > 64)
                        {
                            _xTtxt = Convert.ToChar(_xCcell).ToString();
                        }


                        for (int x = 0; x < _yCnt; x++)
                        {
                            string _xTxt = Convert.ToChar(_xCell).ToString();
                            PurVo _vo = new PurVo();

                            int i = 0;

                            // 평량(BSS_WGT), 연량(PPR_KNT_PER_WGT), 대분류(N1ST_ITM_GRP_CD), 중분류(N2ND_ITM_GRP_CD)
                            _vo.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
                            _vo.CO_NO = (this.combo_CO_NO.SelectedItem as PurVo).CO_NO;
                            _vo.CRE_USR_ID = SystemProperties.USER;
                            _vo.UPD_USR_ID = SystemProperties.USER;
                            _vo.N1ST_ITM_GRP_CD = worksheet.Cells[_xTtxt + _xTxt + "1"].Value.ToString();
                            _vo.N2ND_ITM_GRP_CD = worksheet.Cells[_xTtxt + _xTxt + "2"].Value.ToString();
                            _vo.BSS_WGT = int.Parse(worksheet.Cells["A" + (_nCnt + x)].Value.ToString());
                            _vo.PPR_KNT_PER_WGT = int.Parse(worksheet.Cells["B" + (_nCnt + x)].Value.ToString());

                            if (int.TryParse(worksheet.Cells[_xTtxt + _xTxt + (_nCnt + x)].Value.ToString(), out i))
                            {
                                //평량 당 가격 (원/kg) (WGT_PER_PRC)
                                _vo.WGT_PER_PRC = int.Parse(worksheet.Cells[_xTtxt + _xTxt + (_nCnt + x)].Value.ToString());
                                if (_vo.WGT_PER_PRC.Equals(0)) 
                                {
                                    _vo.WGT_PER_PRC = null;
                                }
                            }

                            _xTxt = Convert.ToChar(_xCell+1).ToString();
                            if (int.TryParse(worksheet.Cells[_xTtxt + _xTxt + (_nCnt + x)].Value.ToString(), out i))
                            {
                                //연량 당 가격 (원/R) (ROLL_PER_PRC)
                                _vo.ROLL_PER_PRC = int.Parse(worksheet.Cells[_xTtxt + _xTxt + (_nCnt + x)].Value.ToString());
                                if (_vo.ROLL_PER_PRC.Equals(0))
                                {
                                    _vo.ROLL_PER_PRC = null;
                                }
                            }

                            if(_vo.WGT_PER_PRC != null || _vo.ROLL_PER_PRC != null )
                            {
                                _saveVo.Add(_vo);
                            }
                        }

                        // 대분류 구분을 위해 2칸 띄기
                        _xCell = _xCell + 2;
                        if (_xCell > 90)
                        {
                            _xCcell++;
                            _xCell = 65;
                        }
                    }

                    // CHNL_CD, CO_NO, PRC_FILE, CRE_USR_ID, UPD_USR_ID
                    // 데이터 보내기
                    if (_saveVo.Count > 0)
                    {
                        _saveVo[0].PRC_FILE = this.spreadsheetControl1.Document.SaveDocument(DevExpress.Spreadsheet.DocumentFormat.Xlsx);
                    }
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("p4402/dtl/i", new StringContent(JsonConvert.SerializeObject(_saveVo), System.Text.Encoding.UTF8, "application/json")))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            int _Num = 0;
                            string result = await response.Content.ReadAsStringAsync();
                            if (int.TryParse(result, out _Num) == false)
                            {
                                if (DXSplashScreen.IsActive == true)
                                {
                                    DXSplashScreen.Close();
                                }

                                //실패
                                WinUIMessageBox.Show(result, Title, MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }

                            DXSplashScreen.Close();
                            //성공
                            WinUIMessageBox.Show("완료 되었습니다", Title, MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }

                    this.DialogResult = true;
                    this.Close();

                }
                catch (Exception eLog)
                {
                    if (DXSplashScreen.IsActive == true)
                    {
                        DXSplashScreen.Close();
                    }
                    WinUIMessageBox.Show(eLog.Message, Title, MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            } 
        }

        public Boolean ValueCheckd()
        {
            if (string.IsNullOrEmpty(this.combo_CO_NO.Text))
            {
                WinUIMessageBox.Show("매입처명을 선택하세요.", "[유효검사]" + Title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.combo_CO_NO.IsTabStop = true;
                this.combo_CO_NO.Focus();
                return false;
            }
            return true;
        }

        // 취소 버튼 이벤트
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

    }
}