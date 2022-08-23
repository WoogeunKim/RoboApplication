using AquilaErpWpfApp3.Util;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Man;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using ModelsLibrary.Fproof;
using System.Linq;

namespace AquilaErpWpfApp3.View.M.Dialog
{
    // 작성자 : 정응현
    // 설명 : UIO 입력 패킷으로부터 데이타를 얻기 위한 구조체 
    public struct USB_INPUT
    {
        public int ProductID;   // 장치 ID 
        public Byte Status;     // 패킷 수신 상태값  0=입력 변화에 의한 수신, 1=데이타 재전송 요구에 의한 수신 
        public Byte Button;     // 입력 버턴값
        public Byte Output;     // USB 장치의 입출력 상태값
        public Byte Mask;       // 포트의 입출력 설정값. bit값이 '0'이면 출력, '1'이면 입력
    };

    public partial class M6632WeighingDialog : DXWindow
    {
        /// uio.dll을 사용하기 위한 선언부입니다.
        /// 해당 파일은 프로젝트 Root 폴더에 있음
        [DllImport("uio32.dll")]
        private static extern int usb_io_init(int pID);
        [DllImport("uio32.dll")]
        private static extern void set_usb_events(int hWnd);
        [DllImport("uio32.dll")]
        private static extern void get_usb_input(int lParam, ref USB_INPUT uInput);
        [DllImport("uio32.dll")]
        private static extern bool usb_io_output(int pID, int cmd, int io1, int io2, int io3, int io4);
        [DllImport("uio32.dll")]
        private static extern bool usb_io_reset(int pID);
        [DllImport("uio32.dll")]
        private static extern bool usb_in_request(int pID);
        /// 여기까지 uio.dll을 사용하기 위한 선언부
        /// 
        //private BarPrint _barCode = new BarPrint();
        //private static ManServiceClient manClient = SystemProperties.ManClient;

        private SerialPort serial = null;
        private string recieved_data = String.Empty;
        private delegate void UpdateUiTextDelegate(string text);
        //
        private int numCheck = 8;
        private int numCount = 0;
        private double oldData = 0;
        //string newData = string.Empty;

        private double tmpNum = 0;
        private double tmpWeighing1 = 0;
        private double tmpWeighing2 = 0;

        private NumberPadDialog editDialog;
        //
        private bool isEndCheck = false;
        //
        private bool isWeighingCheck = false;

        private BarPrint _barPrint = new BarPrint();
        //private IList<CodeDao> useList;


        //private Dictionary<string, string> _ErrMap = new Dictionary<string, string>();

        private int nWeigh = -1;

        public M6632WeighingDialog(IList<ManVo> items)
        {
            InitializeComponent();


            //this._ErrMap = SystemProperties.SYSTEM_CODE("P-003");




            //
            this.ViewJOB_ITEMEdit.ItemsSource = items;
            this.ViewJOB_ITEMEdit.SelectedItemChanged += ViewJOB_ITEMEdit_SelectedItemChanged;

            //
            openSerialPort(0);

            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
            //this.OKButton.Click += new RoutedEventHandler(OKButton_Click);
            this.CancelButton.Click += new RoutedEventHandler(CancelButton_Click);
            this.Closed += Window_Closed;
            this.btn_itm_check.Click += new RoutedEventHandler(btn_itm_check_Click);
            this.btn_barcode_check.Click += new RoutedEventHandler(btn_barcode_check_Click);

            ViewJOB_ITEMEdit_SelectedItemChanged(null, null);


            //칭량 직접 입력
            this.viewJOB_ITEMView.MouseDoubleClick += viewJOB_ITEMView_MouseDoubleClick;



            this.lab_Weighing.MouseLeftButtonDown += Lab_Weighing_MouseLeftButtonDown;


            //IList<PckCodeDao> PCK_CODE_LIST = SystemProperties.PCK_CODE_VO();
            //PCK_CODE_LIST.Insert(0, new PckCodeDao() { PCK_PLST_VAL = 0 });
            //this.combo_PCK_PLST_CD.ItemsSource = PCK_CODE_LIST;
            //this.combo_PCK_PLST_CD.SelectedIndexChanged += combo_PCK_PLST_CD_SelectedIndexChanged;

            //this.viewJOB_ITEMView.KeyDown += viewJOB_ITEMView_KeyDown;
        }

        private void Lab_Weighing_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (this.lab_Weighing.Text.StartsWith("칭량1"))
            {
                this.lab_Weighing.Text = "칭량2 : ";

                //
                if (Properties.Settings.Default.is_weighing_Checkd_2 == true)
                {
                    closeSerialPort();
                    openSerialPort(1);
                }
                else
                {
                    WinUIMessageBox.Show("[칭량2] 설정이 활성화 되지 않았습니다 ", "칭량 작업", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.None, MessageBoxOptions.None);
                    return;
                }

            }
            else if (this.lab_Weighing.Text.StartsWith("칭량2"))
            {
                this.lab_Weighing.Text = "칭량1 : ";

                closeSerialPort();
                openSerialPort(0);
            }
        }

        //void viewJOB_ITEMView_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if (e.Key == Key.Enter)
        //    {
        //        if (this.btn_itm_check.IsEnabled == true)
        //        {
        //            ManVo selVo = (ManVo)this.ViewJOB_ITEMEdit.GetFocusedRow();
        //            if (selVo != null)
        //            {
        //                btn_isEnable(false);
        //                selVo.WEIH_VAL = this.text_WeighingAuto.Text;

        //                //포커스 이동
        //                int nextInd = this.viewJOB_ITEMView.FocusedRowHandle + 1;
        //                this.viewJOB_ITEMView.FocusedRowHandle = nextInd;
        //                this.viewJOB_ITEMView.TopRowIndex = nextInd;
        //            }
        //        }
        //    }
        //}

        void viewJOB_ITEMView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {

                //    this.useList = SystemProperties.SYSTEM_CODE_VO("A-002");
                //    if (!(this.useList.FindIndex(x => x.CLSS_CD.ToLower().Equals(SystemProperties.USER.ToLower())) >= 0))
                //    {
                //        //WinUIMessageBox.Show("칭량이 완료 되었습니다.", "[칭량 관리] 칭량 설정 ", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.None, MessageBoxOptions.None);
                //        return;
                //    }

                //원재료 투입 후 수동 입력
                if (isWeighingCheck == true)
                {
                    ManVo selVo = (ManVo)this.ViewJOB_ITEMEdit.GetFocusedRow();
                    if (selVo != null)
                    {
                        //5번 원료   - 색소
                        if (selVo.CMPO_CD.StartsWith("5-") || selVo.N1ST_ITM_GRP_CD.Equals("002") || selVo.CMPO_CD.Equals("0-ADIW-001"))
                        {
                            editDialog = new NumberPadDialog((string.IsNullOrEmpty(Convert.ToString(selVo.WEIH_VAL)) ? "" : Convert.ToString(selVo.WEIH_VAL)), false, true, 30);
                            editDialog.Title = "칭량 수동 입력";
                            editDialog.Owner = this;
                            editDialog.BorderEffect = BorderEffect.Default;
                            //        ////masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
                            //        ////masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
                            bool isDialog = (bool)editDialog.ShowDialog();
                            if (!isDialog)
                            {
                                selVo.WEIH_VAL = editDialog.editContent;
                                selVo.isCheckd = true;
                            }
                            else
                            {
                                selVo.WEIH_VAL = editDialog.orgContent;
                            }

                            btn_isEnable(true);

                            this.ViewJOB_ITEMEdit.RefreshData();
                        }
                        else
                        {
                            return;
                        }
                    }
                }
            }
            catch (Exception eLog)
            {
                this.txt_state.Foreground = Brushes.DeepPink;
                this.txt_state.Text = "바코드 출력 실패 - [Error]" + eLog.Message;
                return;
            }
        }

       async void btn_itm_check_Click(object sender, RoutedEventArgs e)
        {
            
            try
            {
                //bool prtChk = false;

                ManVo selVo = (ManVo)this.ViewJOB_ITEMEdit.GetFocusedRow();
                if (selVo != null)
                {
                    IsWeighingCheck(true);
                    btn_isEnable(false);


                    if (this.nWeigh == 5)
                    {
                        //Precisa
                        selVo.WEIH_VAL = this.text_WeighingAuto.Text;
                    }

                    using (HttpResponseMessage responseX = await SystemProperties.PROGRAM_HTTP.PostAsync("m6632/dtl/insp", new StringContent(JsonConvert.SerializeObject(new ManVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD, MTRL_LOT_NO = this.text_BarCode.Text }), System.Text.Encoding.UTF8, "application/json")))
                    {
                        if (responseX.IsSuccessStatusCode)
                        {
                            if (selVo.isCheckd)
                            {
                                //수동 입력
                                //selVo.WEIH_VAL = selVo.WEIH_VAL;
                            }
                            else
                            {
                                //저울 입력
                                selVo.WEIH_VAL = this.text_WeighingAuto.Text;
                            }

                            selVo.GRP_LOT_NO = this.text_BarCode.Text;
                            selVo.MTRL_LOT_NO = this.text_BarCode.Text;

                            selVo.INSP_NO = JsonConvert.DeserializeObject<String>(await responseX.Content.ReadAsStringAsync());

                            if (string.IsNullOrEmpty(selVo.INSP_NO))
                            {
                                selVo.INSP_NO = "";
                            }

                            // 실시간 저장
                            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6632/dtl/u", new StringContent(JsonConvert.SerializeObject(selVo), System.Text.Encoding.UTF8, "application/json")))
                            {
                                //if (response.IsSuccessStatusCode)
                                //{
                                //}
                            }

                                    //벌크 코드
                                    //selVo.MM_01 = SelectedMenuItem.ASSY_ITM_CD;
                                    //제조 번호
                                    //selVo.MM_02 = SelectedMenuItem.INP_LOT_NO;

                                    //강제 출력
                                    //_barPrint.SmallPackingPrint_Godex(selVo);



                                    //if (_barPrint.SmallPackingPrint_Godex(SearchDetailJob) == true)
                                    //{
                                    //WinUIMessageBox.Show("완료되었습니다.", "칭량 작업", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                                    //return;
                                    //}
                                    //자동 출력
                                    //prtChk = this._barCode.M6632WeightPrint(selVo);
                                    //if (prtChk)
                                    //{
                                    //this.ViewJOB_ITEMEdit.RefreshRow(this.viewJOB_ITEMView.FocusedRowHandle);
                            this.ViewJOB_ITEMEdit.RefreshData();

                            //포커스 이동
                            //int nextInd = this.viewJOB_ITEMView.FocusedRowHandle + 1;
                            //this.viewJOB_ITEMView.FocusedRowHandle = nextInd;
                            //this.viewJOB_ITEMView.TopRowIndex = nextInd;
                            //}

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //DXMessageBox.Show(ex.Message, "[칭량 관리] 칭량 ", MessageBoxButton.OK, MessageBoxImage.Error);
                this.txt_state.Foreground = Brushes.DeepPink;
                this.txt_state.Text = "바코드 출력 실패 - [Error]" + ex.Message;
                this.Background = Brushes.LightPink;
                //this.text_WeighingAuto.Background = Brushes.DeepPink;
                return;
            }
        }

        void ViewJOB_ITEMEdit_SelectedItemChanged(object sender, DevExpress.Xpf.Grid.SelectedItemChangedEventArgs e)
        {
            try
            {
                ManVo selVo = (ManVo)this.ViewJOB_ITEMEdit.GetFocusedRow();
                if (selVo != null)
                {
                    FailureSound();
                    this.Background = Brushes.LightBlue;
                    this.txt_state.Foreground = Brushes.DodgerBlue;
                    this.txt_state.Text = "[" + selVo.CMPO_CD + "] " + selVo.ITM_NM + " 칭량 준비 중 입니다.";
                    IsWeighingCheck(true);
                    //
                    btn_isEnable(false);
                    this.group_weighing.Header = "[" + selVo.CMPO_CD + "] " + selVo.ITM_NM;
                    //
                    this.text_Weighing1.Text = "" + (double.Parse(selVo.WEIH_BSE_VAL.ToString()) - double.Parse(selVo.MIN_TOR_VAL.ToString()));
                    this.text_Weighing2.Text = "" + (double.Parse(selVo.WEIH_BSE_VAL.ToString()) + double.Parse(selVo.MAX_TOR_VAL.ToString()));

                //    this.combo_PCK_PLST_CD.Text = selVo.PCK_PLST_NM;
                }
            }
            catch (Exception eLog)
            {
                this.txt_state.Text = eLog.Message;
                this.text_WeighingAuto.Background = Brushes.DeepPink;
                this.Background = Brushes.LightPink;
                WinUIMessageBox.Show(eLog.Message, "칭량 작업", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
        }

        //void combo_PCK_PLST_CD_SelectedIndexChanged(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        //ManVo selVo = (ManVo)this.ViewJOB_ITEMEdit.GetFocusedRow();
        //        //if (selVo != null)
        //        //{
        //        //    PckCodeDao pckVo = this.combo_PCK_PLST_CD.SelectedItem as PckCodeDao;
        //        //    if (pckVo != null)
        //        //    {
        //        //        selVo.PCK_PLST_CD = pckVo.PCK_PLST_CD;
        //        //        selVo.PCK_PLST_NM = pckVo.PCK_PLST_NM;
        //        //        //
        //        //        this.text_Weighing1.Text = "" + (double.Parse(selVo.WEIH_BSE_VAL.ToString()) - double.Parse(selVo.MIN_TOR_VAL.ToString()) + pckVo.PCK_PLST_VAL);
        //        //        this.text_Weighing2.Text = "" + (double.Parse(selVo.WEIH_BSE_VAL.ToString()) + double.Parse(selVo.MAX_TOR_VAL.ToString()) + pckVo.PCK_PLST_VAL);
        //        //    }
        //        //}
        //    }
        //    catch (Exception eLog)
        //    {
        //        this.txt_state.Text = eLog.Message;
        //        this.text_WeighingAuto.Background = Brushes.DeepPink;
        //        this.Background = Brushes.LightPink;
        //        WinUIMessageBox.Show(eLog.Message, "칭량 작업", MessageBoxButton.OK, MessageBoxImage.Warning);
        //        return;
        //    }
        //}


        #region Functon (OKButton_Click, CancelButton_Click)
        private async void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //int _Num = 0;
                //using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6632/dtl/m", new StringContent(JsonConvert.SerializeObject(this.ViewJOB_ITEMEdit.ItemsSource), System.Text.Encoding.UTF8, "application/json")))
                //{
                //    if (response.IsSuccessStatusCode)
                //    {
                //        string result = await response.Content.ReadAsStringAsync();
                //        if (int.TryParse(result, out _Num) == false)
                //        {
                //            //실패
                //            WinUIMessageBox.Show(result, "칭량 작업", MessageBoxButton.OK, MessageBoxImage.Error);
                //            return;
                //        }

                //        //성공
                //        WinUIMessageBox.Show("완료 되었습니다", "칭량 작업", MessageBoxButton.OK, MessageBoxImage.Information);
                //        this.DialogResult = true;
                //        this.Close();
                //    }
                //}




                //DB 업데이트 작업 하기
                //ManVo resultVo = new ManVo();
                //IList<ManVo> items = (IList<ManVo>)this.ViewJOB_ITEMEdit.ItemsSource;
                //resultVo = manClient.TransactionM6632(new List<ManVo>(items).ToArray());
                //if (!resultVo.isSuccess)
                //{
                //    //실패
                //    WinUIMessageBox.Show(resultVo.Message, "[칭량 관리] 칭량", MessageBoxButton.OK, MessageBoxImage.Error);
                //    return;
                //}

                //if (resultVo.isSuccess)
                //{
                //    //성공
                //    WinUIMessageBox.Show("완료 되었습니다", "[수정] 칭량", MessageBoxButton.OK, MessageBoxImage.Information);

                //    SoundExit();
                //    //
                //    this.DialogResult = true;
                //    this.Close();
                //}
            }
            catch ( Exception eLog)
            {
                DXMessageBox.Show(eLog.Message, "칭량 작업 ", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            SoundExit();
            //
            this.DialogResult = true;
            this.Close();
        }

        private void HandleEsc(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                //바코드 검사
                if (text_BarCode.Text.Length > 0)
                {
                    btn_barcode_check_Click(sender, e);
                }

                this.text_BarCode.SelectAll();
                this.text_BarCode.Focus();
            }
            else if (e.Key == Key.Escape)
            {
                SoundExit();
                //
                this.DialogResult = false;
                Close();
            }
        }
        #endregion

        private void btn_isEnable(bool isCheck)
        {
            this.btn_itm_check.IsEnabled = isCheck;
        }

        private void openSerialPort(int _num)
        {
            try
            {
                string tmpPort;
                int tmpBaudRate;
                Parity tmpParity;
                int tmpLength;
                StopBits tmpStopBits;

                if (_num == 0)
                {
                    //
                    // 칭량 1
                    tmpPort = Properties.Settings.Default.str_weighing_Port_1;
                    if (string.IsNullOrEmpty(tmpPort))
                    {
                        WinUIMessageBox.Show("[Port] 입력 값이 맞지 않습니다.", "칭량 작업", MessageBoxButton.OK, MessageBoxImage.Warning);
                        this.txt_state.Foreground = Brushes.DeepPink;
                        this.txt_state.Text = "연결 실패 - [Port] 입력 값이 맞지 않습니다.";
                        this.text_WeighingAuto.Background = Brushes.DeepPink;
                        this.Background = Brushes.LightPink;
                        return;
                    }
                    tmpBaudRate = int.Parse(Properties.Settings.Default.str_weighing_BaudRate_1);
                    //Parity tmpParity;
                    switch (Properties.Settings.Default.str_weighing_Parity_1)
                    {
                        case "E":
                            tmpParity = Parity.Even;
                            break;
                        case "O":
                            tmpParity = Parity.Odd;
                            break;
                        default:
                            tmpParity = Parity.None;
                            break;
                    }
                    tmpLength = int.Parse(Properties.Settings.Default.str_weighing_Length_1);
                    //StopBits tmpStopBits;
                    switch (Properties.Settings.Default.str_weighing_StopBit_1)
                    {
                        case "2":
                            tmpStopBits = StopBits.Two;
                            break;
                        case "1.5":
                            tmpStopBits = StopBits.OnePointFive;
                            break;
                        default:
                            tmpStopBits = StopBits.One;
                            break;
                    }

                    //this.serial = new SerialPort("COM3", 2400, Parity.Even, 7, StopBits.Two);
                    this.serial = new SerialPort(tmpPort, tmpBaudRate, tmpParity, tmpLength, tmpStopBits);
                    this.serial.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(Recieve);

                    if (this.serial.IsOpen)
                    {
                        this.serial.Close();
                    }
                    this.serial.Open();
                    this.serial.DiscardInBuffer(); //입력버퍼 클리어
                    this.serial.DiscardOutBuffer(); //출력버퍼 클리어

                    //
                    //저울 종류
                    if (Properties.Settings.Default.str_weighing_Type_1.Equals("GF Series"))
                    {
                        this.nWeigh = 1;
                    }
                    else if (Properties.Settings.Default.str_weighing_Type_1.Equals("CB Series"))
                    {
                        this.nWeigh = 2;
                    }
                    else if (Properties.Settings.Default.str_weighing_Type_1.Equals("FG Series"))
                    {
                        this.nWeigh = 3;
                    }
                    else if (Properties.Settings.Default.str_weighing_Type_1.Equals("CAS"))
                    {
                        this.nWeigh = 4;
                    }
                    else if (Properties.Settings.Default.str_weighing_Type_1.Equals("Precisa"))
                    {
                        this.nWeigh = 5;
                    }
                    else
                    {
                        this.txt_state.Foreground = Brushes.OrangeRed;
                        this.txt_state.Text = "연결 실패 - [전자저울] GF Series/CB Series/FG Series/CAS/Precisa 저울을 선택 되지 않았습니다.";
                        return;
                    }
                    //
                    this.Background = Brushes.LightBlue;
                    this.txt_state.Foreground = Brushes.DodgerBlue;
                    this.txt_state.Text = "연결 성공 - " + tmpPort + " 연결 되었습니다.";
                    this.text_WeighingAuto.Background = Brushes.DodgerBlue;
                }
                else if (_num == 1)
                {
                    //
                    //칭량 2
                    tmpPort = Properties.Settings.Default.str_weighing_Port_2;
                    if (string.IsNullOrEmpty(tmpPort))
                    {
                        WinUIMessageBox.Show("[Port] 입력 값이 맞지 않습니다.", "칭량 작업", MessageBoxButton.OK, MessageBoxImage.Warning);
                        this.txt_state.Foreground = Brushes.DeepPink;
                        this.txt_state.Text = "연결 실패 - [Port] 입력 값이 맞지 않습니다.";
                        this.text_WeighingAuto.Background = Brushes.DeepPink;
                        this.Background = Brushes.LightPink;
                        return;
                    }
                    tmpBaudRate = int.Parse(Properties.Settings.Default.str_weighing_BaudRate_2);
                    //Parity tmpParity;
                    switch (Properties.Settings.Default.str_weighing_Parity_2)
                    {
                        case "E":
                            tmpParity = Parity.Even;
                            break;
                        case "O":
                            tmpParity = Parity.Odd;
                            break;
                        default:
                            tmpParity = Parity.None;
                            break;
                    }
                    tmpLength = int.Parse(Properties.Settings.Default.str_weighing_Length_2);
                    //StopBits tmpStopBits;
                    switch (Properties.Settings.Default.str_weighing_StopBit_2)
                    {
                        case "2":
                            tmpStopBits = StopBits.Two;
                            break;
                        case "1.5":
                            tmpStopBits = StopBits.OnePointFive;
                            break;
                        default:
                            tmpStopBits = StopBits.One;
                            break;
                    }

                    //this.serial = new SerialPort("COM3", 2400, Parity.Even, 7, StopBits.Two);
                    this.serial = new SerialPort(tmpPort, tmpBaudRate, tmpParity, tmpLength, tmpStopBits);
                    this.serial.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(Recieve);

                    if (this.serial.IsOpen)
                    {
                        this.serial.Close();
                    }
                    this.serial.Open();
                    this.serial.DiscardInBuffer(); //입력버퍼 클리어
                    this.serial.DiscardOutBuffer(); //출력버퍼 클리어

                    //
                    //저울 종류
                    if (Properties.Settings.Default.str_weighing_Type_2.Equals("GF Series"))
                    {
                        this.nWeigh = 1;
                    }
                    else if (Properties.Settings.Default.str_weighing_Type_2.Equals("CB Series"))
                    {
                        this.nWeigh = 2;
                    }
                    else if (Properties.Settings.Default.str_weighing_Type_2.Equals("FG Series"))
                    {
                        this.nWeigh = 3;
                    }
                    else if (Properties.Settings.Default.str_weighing_Type_2.Equals("CAS"))
                    {
                        this.nWeigh = 4;
                    }
                    else if (Properties.Settings.Default.str_weighing_Type_2.Equals("Precisa"))
                    {
                        this.nWeigh = 5;
                    }
                    else
                    {
                        this.txt_state.Foreground = Brushes.OrangeRed;
                        this.txt_state.Text = "연결 실패 - [전자저울] GF Series/CB Series/FG Series/CAS/Precisa 저울을 선택 되지 않았습니다.";
                        return;
                    }
                    //
                    this.Background = Brushes.LightBlue;
                    this.txt_state.Foreground = Brushes.DodgerBlue;
                    this.txt_state.Text = "연결 성공 - " + tmpPort + " 연결 되었습니다.";
                    this.text_WeighingAuto.Background = Brushes.DodgerBlue;
                }
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message, "칭량 작업", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Background = Brushes.LightPink;
                this.txt_state.Foreground = Brushes.DeepPink;
                this.txt_state.Text = "연결 실패 - [Error]" + ex.Message;
                this.text_WeighingAuto.Background = Brushes.DeepPink;
                this.Background = Brushes.LightPink;
                return;
            }
        }

        private void closeSerialPort()
        {
            try
            {
                if (this.serial != null)
                {
                    if (this.serial.IsOpen)
                    {
                        //this.text_Weighing.Dispatcher.DisableProcessing();
                        //this.setText = null;
                        //this.setText.EndInvoke(null);
                        //this.text_Weighing.Dispatcher.Invoke(this.setText, new object[] { null });
                        //this.serial.Write("R\r");
                        this.serial.DataReceived -= Recieve;
                        this.serial.DtrEnable = false;
                        this.serial.RtsEnable = false;
                        this.serial.DiscardInBuffer();
                        this.serial.DiscardOutBuffer();
                        this.serial.Close();
                    }
                    //
                    if (this.serial != null)
                    {
                        this.serial.Dispose();
                        this.serial = null;
                        //Thread.Sleep(500);
                    }
                }
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message, "칭량 작업", MessageBoxButton.OK, MessageBoxImage.Error);
                this.txt_state.Foreground = Brushes.DeepPink;
                this.txt_state.Text = ex.Message;
                this.text_WeighingAuto.Background = Brushes.DeepPink;
                this.Background = Brushes.LightPink;
                return;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            try
            {
                SoundExit();
                closeSerialPort();
            }
            catch (Exception)
            {
                return;
            }
        }

        private void Recieve(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            try
            {
                if (this.serial != null)
                {
                    if (this.serial.IsOpen)
                    {
                        //카운터 작업 하기
                        this.recieved_data = this.serial.ReadLine();
                        Dispatcher.BeginInvoke(DispatcherPriority.Send, new UpdateUiTextDelegate(WriteData), this.recieved_data);
                    }
                }
            }
            catch (Exception)
            {
                return;
            }
        }

        private void WriteData(string text)
        {
            try
            {
                //Thread.Sleep(100);
                try
                {
                    //작은 저울 값
                    if (this.nWeigh == 1)
                    {
                        this.text_WeighingAuto.Text = (double.Parse(text.Replace("+", "")) + "");
                    }
                    else if (this.nWeigh == 2)
                    {
                        this.text_WeighingAuto.Text = ((double.Parse(((text.Split(','))[1].Split(' '))[0].Replace("+", ""))) + "");
                    }
                    else if (this.nWeigh == 3)
                    {
                        //큰 저울 값
                        this.text_WeighingAuto.Text = ((double.Parse(((text.Split(','))[1].Split(' '))[0].Replace("+", "")) * 1000) + "");
                    }
                    else if (this.nWeigh == 4)
                    {
                        //CAD
                        this.text_WeighingAuto.Text = (double.Parse((text.Split(','))[2].Replace("+", "").Replace("g", "").Trim()) + "");
                    }
                    else if (this.nWeigh == 5)
                    {
                        //Precisa
                        this.text_WeighingAuto.Text = ((double.Parse(text.Replace("-", "").Replace("+", "").Replace("g", "").Trim())) + "");
                        //this.text_WeighingAuto.Text = (text.Replace("9", "") + "");
                        //if (this.text_WeighingAuto.Text.TrimStart().StartsWith("-"))
                        //{
                        //    this.text_WeighingAuto.Text = double.Parse(this.text_WeighingAuto.Text.Replace("+", "")) + "";
                        //}
                        //else if (this.text_WeighingAuto.Text.TrimStart().StartsWith("+"))
                        //{
                        //    this.text_WeighingAuto.Text = double.Parse(this.text_WeighingAuto.Text.Replace("-", "")) + "";
                        //}
                    }
                    //else if (this.nWeigh == 5)
                    //{
                    //    //Precisa
                    //    this.text_WeighingAuto.Text = ((double.Parse((text.Split(','))[2].Replace("+", "").Replace("g", "").Trim()) * 1000) + "");
                    //}
                    else
                    {
                        this.text_WeighingAuto.Text = "Error";
                    }
                }
                catch
                {
                    //this.txt_state.Text = this.text_WeighingAuto.Text + "  : Error";
                    this.text_WeighingAuto.Text = "Error";
                }


                //this.numCheck = 8;
                //this.numCount = 0;
                //this.oldData = string.Empty;
                //string newData = string.Empty;
                try
                {
                    //작은 저울
                    if (this.nWeigh == 1)
                    {
                        this.tmpNum = double.Parse(text.Replace("+", ""));
                    }
                    else if (this.nWeigh == 2)
                    {
                        this.tmpNum = (double.Parse(((text.Split(','))[1].Split(' '))[0].Replace("+", "")));
                    }
                    else if (this.nWeigh == 3)
                    {
                        //큰 저울 값
                        this.tmpNum = (double.Parse(((text.Split(','))[1].Split(' '))[0].Replace("+", "")) * 1000);
                    }
                    else if (this.nWeigh == 4)
                    {
                        //CAD
                        this.tmpNum = (double.Parse((text.Split(','))[2].Replace("+", "").Replace("g", "").Trim()));
                    }
                    else if (this.nWeigh == 5)
                    {
                        //Precisa
                        this.tmpNum = double.Parse(this.text_WeighingAuto.Text);
                        this.oldData = double.Parse(this.text_WeighingAuto.Text);
                        this.numCount = 10;
                        ////CAD
                        //this.tmpNum = ((double.Parse((text.Split(','))[2].Replace("+", "").Replace("g", "").Trim()) * 1000));
                    }
                    else
                    {
                        this.tmpNum = 0;
                    }
                }
                catch
                {
                    this.tmpNum = 0;
                }

                this.tmpWeighing1 = double.Parse(string.IsNullOrEmpty(this.text_Weighing1.Text) ? "0" : this.text_Weighing1.Text);
                this.tmpWeighing2 = double.Parse(string.IsNullOrEmpty(this.text_Weighing2.Text) ? "0" : this.text_Weighing2.Text);

                //if (isWeighingCheck == false) { return; }
                if ((this.tmpNum >= this.tmpWeighing1) && (this.tmpNum <= this.tmpWeighing2))
                {
                    //this.txt_count.Text = "3";
                    if (this.oldData == this.tmpNum)
                    {
                        this.numCount++;
                        if (this.numCount >= this.numCheck)
                        {
                            btn_isEnable(true);
                            this.Background = Brushes.LightBlue;
                            this.txt_state.Foreground = Brushes.DodgerBlue;
                            this.txt_state.Text = "칭량 완료 되었습니다.";

                            this.text_WeighingAuto.Background = Brushes.DodgerBlue;
                            //Thread.Sleep(700);
                            SuccessSound();
                        }
                    }
                    else
                    {

                        try
                        {
                            //작은 저울
                            if (this.nWeigh == 1)
                            {
                                this.oldData = double.Parse(text.Replace("+", ""));
                            }
                            else if (this.nWeigh == 2)
                            {
                                this.oldData = (double.Parse(((text.Split(','))[1].Split(' '))[0].Replace("+", "")));
                            }
                            else if (this.nWeigh == 3)
                            {
                                //큰 저울 값
                                this.oldData = (double.Parse(((text.Split(','))[1].Split(' '))[0].Replace("+", "")) * 1000);
                            }
                            else if (this.nWeigh == 4)
                            {
                                //CAD
                                this.oldData = (double.Parse((text.Split(','))[2].Replace("+", "").Replace("g", "").Trim()));
                            }
                            else if (this.nWeigh == 5)
                            {
                                //Precisa
                                this.tmpNum = double.Parse(this.text_WeighingAuto.Text);
                                this.oldData = double.Parse(this.text_WeighingAuto.Text);
                                this.numCount = 10;
                                //    //CAD
                                //     this.oldData = ((double.Parse((text.Split(','))[2].Replace("+", "").Replace("g", "").Trim()) * 1000));
                            }
                            else
                            {
                                this.oldData = 0;
                            }
                        }
                        catch
                        {
                            this.oldData = 0;
                        }

                        this.numCount = 0;
                        btn_isEnable(false);
                        //this.txt_state.Foreground = Brushes.DeepPink;
                        //this.txt_state.Text = "칭량 준비 중 입니다.";

                        this.text_WeighingAuto.Background = Brushes.DeepPink;
                        this.Background = Brushes.LightPink;
                        //FailureSound();
                        //노란색
                        ProgressSound();
                    }
                }
                else
                {
                    //this.txt_state.Foreground = Brushes.DeepPink;
                    //this.txt_state.Text = "칭량 준비 중 입니다.";
                    btn_isEnable(false);
                    this.Background = Brushes.LightPink;
                    this.text_WeighingAuto.Background = Brushes.DeepPink;
                    FailureSound();
                    //빨강색 0 제외 하기
                }
            }
            catch (Exception eLog)
            {
                DXMessageBox.Show(eLog.Message, "칭량 작업", MessageBoxButton.OK, MessageBoxImage.Error);
                this.txt_state.Foreground = Brushes.DeepPink;
                this.txt_state.Text = eLog.Message;
                this.text_WeighingAuto.Background = Brushes.DeepPink;
                this.Background = Brushes.LightPink;
                return;
            }
        }


        public void SoundExit()
        {
            try
            {
                int selectID = 609;
                //bool result;
                // Blink Sound Off 
                //result = usb_io_output(selectID, 0, 1, 0, 0, 0);
                //result = usb_io_output(selectID, 0, -1, 0, 0, 0);
                //// Blink Red On
                //result = usb_io_output(selectID, 0, 2, 0, 0, 0);
                //result = usb_io_output(selectID, 0, -2, 0, 0, 0);
                //result = usb_io_output(selectID, 0, 4, 0, 0, 0);
                //result = usb_io_output(selectID, 0, -4, 0, 0, 0);
                isEndCheck = usb_io_reset(selectID);
            }
            catch (Exception eLog)
            {
                DXMessageBox.Show(eLog.Message, "칭량 작업", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
        // 작성자 : 정응현
        // 내  용 : F/PROOF 실패시 BLINK Sound 매 5초 마다 ,  적색 점멸등 매 5초마다 점멸 진행
        private void FailureSound()
        {
            try
            {
                int selectID = 609;
                int blink;
                bool result;
                String blinkString = "5";

                blink = Convert.ToInt32(blinkString, 10) * 16;
                blink += Convert.ToInt32(blinkString, 10);
                // Blink Sound On
                //result = usb_io_output(selectID, blink, 1, 0, 0, 0);
                //result = usb_io_output(selectID, 0, -1, 0, 0, 0);
                // Blink Red On
                result = usb_io_output(selectID, blink, 2, 0, 0, 0);

                // Blink Sound Off 
                //result = usb_io_output(selectID, blink, -1, 0, 0, 0);
                // Blink Red On
                //result = usb_io_output(selectID, blink, -2, 0, 0, 0);

                if (isEndCheck)
                {
                    SoundExit();
                }
            }
            catch (Exception eLog)
            {
                DXMessageBox.Show(eLog.Message, "칭량 작업", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void ProgressSound()
        {
            try
            {
                int selectID = 609;
                int blink;
                bool result;
                String blinkString = "5";

                blink = Convert.ToInt32(blinkString, 10) * 16;
                blink += Convert.ToInt32(blinkString, 10);
                // Blink Sound On
                //result = usb_io_output(selectID, blink, 1, 0, 0, 0);
                // 성공 통과음 Off
                //result = usb_io_output(selectID, 0, -1, 0, 0, 0);
                // Blink Red On
                result = usb_io_output(selectID, blink, -3, 0, 0, 0);
                // Blink Sound Off 
                //result = usb_io_output(selectID, blink, -1, 0, 0, 0);
                // Blink Red On
                //result = usb_io_output(selectID, blink, -2, 0, 0, 0);
                if (isEndCheck)
                {
                    SoundExit();
                }
            }
            catch (Exception eLog)
            {
                DXMessageBox.Show(eLog.Message, "칭량 작업", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void SuccessSound()
        {
            try
            {
                int selectID = 609;
                bool result;
                //// 성공 통과음 On
                //result = usb_io_output(selectID, 0, 1, 0, 0, 0);
                //Thread.Sleep(300);
                // 성공 통과음 Off
                //result = usb_io_output(selectID, 0, -1, 0, 0, 0);

                // 성공 녹색 점멸 On 
                //result = usb_io_output(selectID, 0, 4, 0, 0, 0);
                // 성공 녹색 점멸 Off : Ok입력시 해제를 한다. 
                //result = usb_io_output(selectID, 0, -4, 0, 0, 0);
                result = usb_io_output(selectID, 13, 1, 4, 0, 0);
                result = usb_io_output(selectID, 0, -1, 0, 0, 0);
                if (isEndCheck)
                {
                    SoundExit();
                }
            }
            catch (Exception eLog)
            {
                DXMessageBox.Show(eLog.Message, "칭량 작업", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }


        private void IsWeighingCheck(bool isCheck)
        {
            try
            {
                ManVo selVo = (ManVo)this.ViewJOB_ITEMEdit.GetFocusedRow();
                if (selVo != null)
                {
                    if (isCheck)
                    {
                        //this.txt_state.Foreground = Brushes.DodgerBlue;
                        //this.txt_state.Text = "[" + selVo.CMPO_CD + "] " + selVo.CMPO_NM + " 칭량 준비 중 입니다.";

                        this.isWeighingCheck = false;
                        //1단계 검사
                        this.checkd_ONE.Visibility = System.Windows.Visibility.Visible;
                        this.checkd_TWO.Visibility = System.Windows.Visibility.Hidden;
                        //
                        this.btn_barcode_check.Visibility = System.Windows.Visibility.Visible;
                        this.btn_itm_check.Visibility = System.Windows.Visibility.Hidden;
                        //
                        this.text_BarCode.Background = null;
                        this.text_BarCode.IsReadOnly = false;
                        //
                        //this.combo_PCK_PLST_CD.IsEnabled = false;
                        //this.text_PCK_PLST_CD.Foreground = Brushes.DarkGray;
                    }
                    else
                    {
                        this.isWeighingCheck = true;
                        //2단계 검사
                        this.checkd_ONE.Visibility = System.Windows.Visibility.Hidden;
                        this.checkd_TWO.Visibility = System.Windows.Visibility.Visible;
                        //
                        this.btn_barcode_check.Visibility = System.Windows.Visibility.Hidden;
                        this.btn_itm_check.Visibility = System.Windows.Visibility.Visible;
                        //
                        this.text_BarCode.Background = Brushes.DarkGray;
                        this.text_BarCode.IsReadOnly = true;
                        //
                        //this.combo_PCK_PLST_CD.IsEnabled = true;
                        //this.text_PCK_PLST_CD.Foreground = Brushes.Black;

                    }
                }
            }
            catch(Exception)
            {
                //DXMessageBox.Show(eLog.Message, "[칭량 관리] 칭량 ", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        async void  btn_barcode_check_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ManVo selVo = (ManVo)this.ViewJOB_ITEMEdit.GetFocusedRow();
                if (selVo != null)
                {
                    if (this.text_BarCode.Text.Length > 0)
                    {
                        int _Num;
                        string resultMSG;
                        ManVo resultVo = new ManVo();

                        resultVo.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
                        resultVo.CRE_USR_ID = SystemProperties.USER;
                        resultVo.LOT_NO = selVo.LOT_NO;
                        resultVo.GRP_LOT_NO = this.text_BarCode.Text;
                        resultVo.ITM_CD = selVo.CMPO_CD;
                        resultVo.ASSY_ITM_SEQ = selVo.ASSY_ITM_SEQ;


                        //
                        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6632/mst/proc", new StringContent(JsonConvert.SerializeObject(resultVo), System.Text.Encoding.UTF8, "application/json")))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                resultMSG = await response.Content.ReadAsStringAsync();
                                if (int.TryParse(resultMSG, out _Num) == false)
                                {
                                    //실패
                                    //WinUIMessageBox.Show(resultMSG, "[" + SystemProperties.PROGRAM_TITLE + "] 칭량 작업 계획", MessageBoxButton.OK, MessageBoxImage.Error);
                                    this.Background = Brushes.LightPink;
                                    this.txt_state.Foreground = Brushes.DeepPink;
                                    this.txt_state.Text = "[" + selVo.CMPO_CD + "] " + selVo.ITM_NM + "  " + resultMSG;
                                    IsWeighingCheck(true);
                                    return;
                                }

                                //
                                //
                                //FproofVo tmpVo;
                                //using (HttpResponseMessage responseY = await SystemProperties.PROGRAM_HTTP.PostAsync("m6632/mst/item", new StringContent(JsonConvert.SerializeObject(new FproofVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD, CRE_USR_ID = SystemProperties.USER, MTRL_LOT_NO = this.text_BarCode.Text }), System.Text.Encoding.UTF8, "application/json")))
                                //{
                                //    if (responseY.IsSuccessStatusCode)
                                //    {
                                //        tmpVo = JsonConvert.DeserializeObject<IEnumerable<FproofVo>>(await responseY.Content.ReadAsStringAsync()).Cast<FproofVo>().ToList()[0];

                                //        시험 번호 업데이트
                                //        selVo.
                                //    }
                                //}

                                //WinUIMessageBox.Show("완료 되었습니다", "[추가] 칭량 작업 계획", MessageBoxButton.OK, MessageBoxImage.Information);
                                //성공
                                selVo.GRP_LOT_NO = this.text_BarCode.Text;
                                this.txt_state.Foreground = Brushes.DodgerBlue;
                                this.Background = Brushes.LightBlue;
                                this.txt_state.Text = "칭량 준비 되었습니다.";
                                IsWeighingCheck(false);
                            }
                        }
                    }
                }
            }
            catch (Exception eLog)
            {
                this.txt_state.Foreground = Brushes.DeepPink;
                this.txt_state.Text = eLog.Message;
                this.Background = Brushes.LightPink;
                //DXMessageBox.Show(eLog.Message, "칭량 작업", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private async void ButtonBarcode_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ManVo selVo = (ManVo)this.ViewJOB_ITEMEdit.GetFocusedRow();
                if (selVo != null)
                {
                    if (string.IsNullOrEmpty(selVo.MTRL_LOT_NO))
                    {
                        WinUIMessageBox.Show("해당 원재료는 칭량이 완료 되지 않았습니다.", "칭량 작업", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.None, MessageBoxOptions.None);
                        return;
                    }

                    //MessageBoxResult result = WinUIMessageBox.Show("[" + selVo.PRN_LOT_NO + " / " + selVo.ITM_NM + "] 바코드 출력 하시겠습니까?", "[바코드 - " + Properties.Settings.Default.str_PrnNm + "]" + "칭량 작업", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    //if (result == MessageBoxResult.Yes)
                    //{
                        //벌크 코드
                        //SearchDetailJob.MM_01 = SelectedMenuItem.ASSY_ITM_CD;
                        //제조 번호
                        //SearchDetailJob.MM_02 = SelectedMenuItem.INP_LOT_NO;

                    DXSplashScreen.Show<ProgressWindow>();

                    //실시간 저장
                    //int _Num = 0;
                    //selVo.GRP_LOT_NO = this.text_BarCode.Text;
                    //selVo.GRP_LOT_NO = T3.MTRL_LOT_NO
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6632/dtl/u", new StringContent(JsonConvert.SerializeObject(selVo), System.Text.Encoding.UTF8, "application/json")))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            //강제 출력
                            if (_barPrint.SmallPackingPrint_Godex(selVo) == true)
                            {
                                int nextInd = this.viewJOB_ITEMView.FocusedRowHandle + 1;
                                this.viewJOB_ITEMView.FocusedRowHandle = nextInd;
                                this.viewJOB_ITEMView.TopRowIndex = nextInd;

                                //WinUIMessageBox.Show("완료되었습니다.", "칭량 작업", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                                //return;
                            }

                            //string result = await response.Content.ReadAsStringAsync();
                            //if (int.TryParse(result, out _Num) == false)
                            //{
                            //    //실패
                            //    WinUIMessageBox.Show(result, "칭량 작업", MessageBoxButton.OK, MessageBoxImage.Error);
                            //    return;
                            //}
                        }
                    }

                    DXSplashScreen.Close();
                    //    //    _barCode.M6632WeightPrint(SearchDetailJob);
                    //}
                }
            }
            catch (System.Exception eLog)
            {
                if (DXSplashScreen.IsActive == true)
                {
                    DXSplashScreen.Close();
                }
                WinUIMessageBox.Show(eLog.Message, "칭량 작업", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }
    }
}
