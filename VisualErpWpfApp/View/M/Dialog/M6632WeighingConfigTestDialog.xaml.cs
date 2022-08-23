using DevExpress.Xpf.Core;
using System;
using System.IO.Ports;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;


namespace AquilaErpWpfApp3.View.M.Dialog
{
    public partial class M6632WeighingConfigTestDialog : DXWindow
    {
        private SerialPort serial = null;
        private string recieved_data = String.Empty;
        private delegate void UpdateUiTextDelegate(string text);


        public M6632WeighingConfigTestDialog(int _nIndex)
        {
            InitializeComponent();

            //
            if (_nIndex == 0)
            {
                openSerialPort_1();
            }
            else if (_nIndex == 1)
            {
                openSerialPort_2();
            }

            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
            this.OKButton.Click += new RoutedEventHandler(OKButton_Click);
            this.Closed += Window_Closed;
        }

        #region Functon (OKButton_Click, CancelButton_Click)
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            //closeSerialPort();
            //
            this.DialogResult = true;
            this.Close();
            //}
        }

        private void HandleEsc(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                //closeSerialPort();

                this.DialogResult = false;
                Close();
            }
        }
        #endregion

        private void openSerialPort_1()
        {
            try
            {
                string tmpPort = Properties.Settings.Default.str_weighing_Port_1;
                int tmpBaudRate = int.Parse(Properties.Settings.Default.str_weighing_BaudRate_1);
                Parity tmpParity;
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
                int tmpLength = int.Parse(Properties.Settings.Default.str_weighing_Length_1);
                StopBits tmpStopBits;
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
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message, "[저울 테스트] 칭량 작업", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void openSerialPort_2()
        {
            try
            {
                string tmpPort = Properties.Settings.Default.str_weighing_Port_2;
                int tmpBaudRate = int.Parse(Properties.Settings.Default.str_weighing_BaudRate_2);
                Parity tmpParity;
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
                int tmpLength = int.Parse(Properties.Settings.Default.str_weighing_Length_2);
                StopBits tmpStopBits;
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
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message, "[저울 테스트] 칭량 작업", MessageBoxButton.OK, MessageBoxImage.Error);
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
                        Thread.Sleep(500);
                    }
                }
            }
            catch (Exception ex)
            {
                DXMessageBox.Show(ex.Message, "[저울 테스트] 칭량 작업", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            closeSerialPort();
        }

        private void Recieve(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            try
            {
                if (this.serial != null)
                {
                    if (this.serial.IsOpen)
                    {
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
            Thread.Sleep(20);
            this.text_Weighing.Text = text;
        }
    }
}
