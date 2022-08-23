using AquilaErpWpfApp3.Util;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Pur;
using ModelsLibrary.Sale;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace AquilaErpWpfApp3.View.SAL.Dialog
{
    public partial class S3321DetailDialog : DXWindow
    {
        //private static SaleOrderServiceClient saleOrderClient = SystemProperties.SaleOrderClient;
        private SaleVo orgVo;
        private string title = "수주등록";

        private S3321ExcelDialog excelDialog;


        public S3321DetailDialog(SaleVo vo)
        {
            InitializeComponent();

            this.orgVo = vo;

            //this.txt_stDate.Text = System.DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            //this.txt_enDate.Text = System.DateTime.Now.ToString("yyyy-MM-dd");

            //SYSTEM_CODE_VO();

            this.btn_reset.Click += btn_reset_Click;
            //this.btn_apply.Click += btn_apply_Click;

            //this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
            //this.OKButton.Click += new RoutedEventHandler(OKButton_Click);
            this.BlcButton.Click += BlcButton_Click;
            this.CancelButton.Click += CancelButton_Click;


            //this.OKButton.IsEnabled = false;


            this.txt_stDate.Text = Convert.ToDateTime(vo.FM_DT).AddMonths(-1).ToString("yyyy-MM-dd");
            this.txt_enDate.Text = vo.TO_DT;
            //this.combo_GRP_NM.Text = vo.SL_CO_CD;

            searchItem();
        }

        private void btn_reset_Click(object sender, RoutedEventArgs e)
        {
            searchItem();

        }

        private async void searchItem()
        {
            try
            {
                PurVo vo = new PurVo() {CHNL_CD = SystemProperties.USER_VO.CHNL_CD, FM_DT = Convert.ToDateTime(this.txt_stDate.Text).ToString("yyyy-MM-dd"), TO_DT = Convert.ToDateTime(this.txt_enDate.Text).ToString("yyyy-MM-dd"), GBN = this.orgVo.SL_RLSE_NO };

                //SystemCodeVo grpCdVo = this.combo_GRP_NM.SelectedItem as SystemCodeVo;
                //if (grpCdVo != null)
                //{
                //    vo.SL_CO_CD = grpCdVo.CO_NO;
                //    vo.SL_CO_NM = grpCdVo.CO_NM;
                //}
                //else
                //{
                    //vo.SL_CO_CD = orgVo.SL_CO_CD;
                    //vo.SL_CO_NM = orgVo.SL_CO_NM;
                    //WinUIMessageBox.Show("[거래처]을 다시 선택 해주세요", "[조회 조건]" + title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                    //this.MSG.Text = "[거래처]을 다시 선택 해주세요";
                    //return;
                //}

                this.search_title.Text = "[조회 조건]   " + "기간 : " + Convert.ToDateTime(this.txt_stDate.Text).ToString("yyyy-MM-dd") + "~" + Convert.ToDateTime(this.txt_enDate.Text).ToString("yyyy-MM-dd");
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s3321/popup", new StringContent(JsonConvert.SerializeObject(vo), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.ViewJOB_ITEMEdit.SelectedItems = new List<PurVo>();
                        this.ViewJOB_ITEMEdit.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<PurVo>>(await response.Content.ReadAsStringAsync()).Cast<PurVo>().ToList();
                    }
                }

                    //this.ViewJOB_ITEMEdit.ItemsSource = saleOrderClient.S2217SelectDtlViewList(vo);
                }
            catch (Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
                this.MSG.Text = eLog.Message;
                return;
            }
        }

        #region Functon (OKButton_Click, CancelButton_Click)
        //private async void OKButton_Click(object sender, RoutedEventArgs e)
        //{

            //List<PurVo> _selectItems = (List<PurVo>)this.ViewJOB_ITEMEdit.SelectedItems;
            //int _size = _selectItems.Count;
            //if (_size <= 0)
            //{
            //    WinUIMessageBox.Show("데이터가 존재 하지 않습니다", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
            //    return;
            //}

            ////
            ////MessageBoxResult result = WinUIMessageBox.Show("정말로 주문등록 하시겠습니까?", title, MessageBoxButton.YesNo, MessageBoxImage.Question);
            ////if (result == MessageBoxResult.Yes)
            ////{
            //    excelDialog = new S3321ExcelDialog(_selectItems);
            //    excelDialog.Title = title;
            //    excelDialog.Owner = this;
            //    excelDialog.BorderEffect = BorderEffect.Default;
            //    //excelDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            //    //excelDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            //    bool isDialog = (bool)excelDialog.ShowDialog();
            //    if (isDialog)
            //    {
            //        //성공
            //        WinUIMessageBox.Show("주문등록 완료되었습니다.", title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);

            //        this.DialogResult = true;
            //        this.Close();
            //    }
        //}

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void HandleEsc(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.DialogResult = true;
                Close();
            }
        }


        private async void BlcButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.ViewJOB_ITEMEdit.SelectedItem == null)
                {
                    return;
                }

                //
                //if (DXSplashScreen.IsActive == false)
                //{
                    DXSplashScreen.Show<SplashScreenView>();
                    DXSplashScreen.UseDefaultAltTabBehavior = true;
                    DXSplashScreen.UseLegacyLocationLogic = true;
                    DXSplashScreen.UIThreadReleaseMode = UIThreadReleaseMode.WaitForSplashScreenLoaded;
                //}

                PurVo _downFileVo = new PurVo();
                //DXF 파일 다운로드 -> ADEM 폴더 복사 
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("p441106/dtl/file", new StringContent(JsonConvert.SerializeObject(this.ViewJOB_ITEMEdit.SelectedItem), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        _downFileVo = JsonConvert.DeserializeObject<PurVo>(await response.Content.ReadAsStringAsync());
                        if (_downFileVo.isSuccess == true)
                        {
                            DXSplashScreen.SetState("[" + _downFileVo.FLR_NM + "] File Download..");
                            DXSplashScreen.Progress(30);
                            File.WriteAllBytes(Properties.Settings.Default.SettingAdemPath + @"\input\" + _downFileVo.FLR_NM, _downFileVo.FLR_FILE);
                        }
                        else
                        {
                            if (DXSplashScreen.IsActive == true)
                            {
                                DXSplashScreen.Close();
                            }
                            //실패
                            WinUIMessageBox.Show(_downFileVo.Message, title, MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }

                    }
                }

                DXSplashScreen.SetState("설계도면 자동추출 프로그램 Start..");
                DXSplashScreen.Progress(60);
                //외부프로그램 실행
                ProcessStartInfo cmd = new ProcessStartInfo();
                Process process = new Process();
                cmd.FileName = @"cmd";
                cmd.CreateNoWindow = true;                               // cmd창을 띄우지 안도록 하기

                cmd.UseShellExecute = false;
                cmd.RedirectStandardOutput = true;          // cmd창에서 데이터를 가져오기
                cmd.RedirectStandardInput = true;           // cmd창으로 데이터 보내기
                cmd.RedirectStandardError = true;           // cmd창에서 오류 내용 가져오기

                process.OutputDataReceived += new DataReceivedEventHandler(OutputHandler);
                //process.ErrorDataReceived += new DataReceivedEventHandler(OutputHandler);


                process.EnableRaisingEvents = false;
                process.StartInfo = cmd;
                process.Start();
                process.BeginOutputReadLine();
                //process.BeginErrorReadLine();

                string progLoc = Properties.Settings.Default.SettingAdemPath + @"\main.exe";                       // 프로그램 위치
                string drawType = "ExceRebar";                              // 도면형식
                string dxfFile = Properties.Settings.Default.SettingAdemPath + @"\input\" + _downFileVo.FLR_NM;            // 도면
                string typeFile = Properties.Settings.Default.SettingAdemPath + @"\shape_codes.xlsx";              // 도면형식파일
                string newXlsxFile = Properties.Settings.Default.SettingAdemPath + @"\result\ExceReBar.xlsx";      // 추출엑셀파일

                // 추출파일 만들기
                byte[] xlsxFile = File.ReadAllBytes(Properties.Settings.Default.SettingAdemPath + @"\report\base.xlsx");
                File.WriteAllBytes(newXlsxFile, xlsxFile);


                process.StandardInput.Write(progLoc + " " + drawType + " " + dxfFile + " " + typeFile + " " + newXlsxFile + Environment.NewLine);
                // 명령어를 보낼때는 꼭 마무리를 해줘야 한다. 그래서 마지막에 NewLine가 필요하다
                process.StandardInput.Close();

                //string result = process.StandardOutput.ReadToEnd();

                DXSplashScreen.SetState("설계도면 자동추출 프로그램 Running..");
                DXSplashScreen.Progress(80);

                //File.AppendAllText(Properties.Settings.Default.SettingAdemPath + @"\log.txt", result);
       

                //StringBuilder sb = new StringBuilder();
                //sb.Append("[Result Info]" + DateTime.Now + Environment.NewLine);
                //sb.Append(result);
                //sb.Append("\r\n");
                //Console.WriteLine(result);

                process.WaitForExit();

                DXSplashScreen.SetState("설계도면 자동추출 프로그램 End..");
                DXSplashScreen.Progress(100);

                process.Close();



                if (DXSplashScreen.IsActive == true)
                {
                    DXSplashScreen.Close();
                }

                /////////////////////////////  최적화 종료  >>>  newXlsxFile 추출 엑셀 파일
                excelDialog = new S3321ExcelDialog(newXlsxFile, (this.ViewJOB_ITEMEdit.SelectedItem as PurVo), dxfFile);
                excelDialog.Title = this.Title;
                excelDialog.Owner = this;
                excelDialog.BorderEffect = BorderEffect.Default;
                bool isDialog = (bool)excelDialog.ShowDialog();
                if (isDialog)
                {
                    (this.ViewJOB_ITEMEdit.SelectedItem as PurVo).isSuccess = true;

                    //int nFocuse = this.viewJOB_ITEMView.FocusedRowHandle;
                    this.ViewJOB_ITEMEdit.RefreshRow(this.viewJOB_ITEMView.FocusedRowHandle);
                    //WinUIMessageBox.Show("완료되었습니다.", title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                    //this.DialogResult = true;
                    //this.Close();
                }
            }
            catch (Exception eLog)
            {
                if (DXSplashScreen.IsActive == true)
                {
                    DXSplashScreen.Close();
                }
                WinUIMessageBox.Show(eLog.Message, "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
                this.MSG.Text = eLog.Message;
                return;
            }
        }

        async void OutputHandler(object sendingProcess, DataReceivedEventArgs outLine)
        {
            //* Do your stuff with the output (write to console/log/StringBuilder)
            //Console.WriteLine(outLine.Data);
            DXSplashScreen.SetState("설계도면 자동추출 프로그램 Running.." + Environment.NewLine + outLine.Data);
            File.AppendAllText(Properties.Settings.Default.SettingAdemPath + @"\log.txt", (outLine.Data +  Environment.NewLine));
        }


        #endregion
    }
}
