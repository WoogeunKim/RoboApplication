using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Man;
using System;
using System.IO.Ports;
using System.Windows;
using System.Windows.Input;


namespace AquilaErpWpfApp3.View.M.Dialog
{
    /// <summary>
    /// Interaction logic for ConfigView1Dialog.xaml
    /// </summary>
    public partial class M6632WeighingConfigDialog : DXWindow
    {
        //private static ManServiceClient manClient = SystemProperties.ManClient;
        //private ManVo orgDao;
        //private bool isEdit = false;
        //private ManVo updateDao;

        private M6632WeighingConfigTestDialog _dialog;

        public M6632WeighingConfigDialog()
        {
            InitializeComponent();

            //
            string[] ports = SerialPort.GetPortNames();
            foreach (string port in ports)
            {
                this.combo_1_Port.Items.Add(port);
                this.combo_2_Port.Items.Add(port);
            }
            //

            this.combo_1_Port.Text = Properties.Settings.Default.str_weighing_Port_1;
            this.combo_1_BaudRate.Text = Properties.Settings.Default.str_weighing_BaudRate_1;
            this.combo_1_Parity.Text = Properties.Settings.Default.str_weighing_Parity_1;
            this.combo_1_Length.Text = Properties.Settings.Default.str_weighing_Length_1;
            this.combo_1_StopBit.Text = Properties.Settings.Default.str_weighing_StopBit_1;
            this.combo_1_Type.Text = Properties.Settings.Default.str_weighing_Type_1;
            //
            //
            if (Properties.Settings.Default.is_weighing_Checkd_2 == true)
            {
                this.combo_2_Port.Text = Properties.Settings.Default.str_weighing_Port_2;
                this.combo_2_BaudRate.Text = Properties.Settings.Default.str_weighing_BaudRate_2;
                this.combo_2_Parity.Text = Properties.Settings.Default.str_weighing_Parity_2;
                this.combo_2_Length.Text = Properties.Settings.Default.str_weighing_Length_2;
                this.combo_2_StopBit.Text = Properties.Settings.Default.str_weighing_StopBit_2;
                this.combo_2_Type.Text = Properties.Settings.Default.str_weighing_Type_2;
                this.chk_Tab2.IsChecked = Properties.Settings.Default.is_weighing_Checkd_2;
            }


            btn_isEnable(false);
            //
            this.combo_1_Port.SelectedIndexChanged += combo_isCheck_SelectedIndexChanged;
            this.combo_1_BaudRate.SelectedIndexChanged += combo_isCheck_SelectedIndexChanged;
            this.combo_1_Parity.SelectedIndexChanged += combo_isCheck_SelectedIndexChanged;
            this.combo_1_Length.SelectedIndexChanged += combo_isCheck_SelectedIndexChanged;
            this.combo_1_StopBit.SelectedIndexChanged += combo_isCheck_SelectedIndexChanged;
            this.combo_1_Type.SelectedIndexChanged += combo_isCheck_SelectedIndexChanged;
            //
            //
            this.combo_2_Port.SelectedIndexChanged += combo_isCheck_SelectedIndexChanged;
            this.combo_2_BaudRate.SelectedIndexChanged += combo_isCheck_SelectedIndexChanged;
            this.combo_2_Parity.SelectedIndexChanged += combo_isCheck_SelectedIndexChanged;
            this.combo_2_Length.SelectedIndexChanged += combo_isCheck_SelectedIndexChanged;
            this.combo_2_StopBit.SelectedIndexChanged += combo_isCheck_SelectedIndexChanged;
            this.combo_2_Type.SelectedIndexChanged += combo_isCheck_SelectedIndexChanged;


            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
            this.OKButton.Click += new RoutedEventHandler(OKButton_Click);
            this.CancelButton.Click += new RoutedEventHandler(CancelButton_Click);

            this.OKTest.Click += new RoutedEventHandler(OKTest_Click);
        }

        void OKTest_Click(object sender, RoutedEventArgs e)
        {
            if (ValueCheckd())
            {
                MessageBoxResult result = WinUIMessageBox.Show("테스트를 진행 하시겠습니까?", "[저울 테스트] 칭량 관리", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    btn_Save();
                    //
                    if (this.tabControl.SelectedIndex == 0)
                    {
                        _dialog = new M6632WeighingConfigTestDialog(0);
                        _dialog.Title = "[저울1 테스트] 칭량 설정";
                        _dialog.Owner = Application.Current.MainWindow;
                        _dialog.BorderEffect = BorderEffect.Default;
                        ////masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
                        ////masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
                        bool isDialog = (bool)_dialog.ShowDialog();
                        if (isDialog)
                        {
                            //if (masterDialog.IsEdit == false)
                            //{
                            //}
                        }
                    }
                    else if( this.tabControl.SelectedIndex == 1)
                    {
                        _dialog = new M6632WeighingConfigTestDialog(1);
                        _dialog.Title = "[저울2 테스트] 칭량 설정";
                        _dialog.Owner = Application.Current.MainWindow;
                        _dialog.BorderEffect = BorderEffect.Default;
                        ////masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
                        ////masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
                        bool isDialog = (bool)_dialog.ShowDialog();
                        if (isDialog)
                        {
                            //if (masterDialog.IsEdit == false)
                            //{
                            //}
                        }
                    }
                }
            }
        }

        void combo_isCheck_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            btn_isEnable(true);
        }

        #region Functon (OKButton_Click, CancelButton_Click)
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            //if (ValueCheckd())
            //{
            //    ManVo resultVo;
            //    if (isEdit == false)
            //    {
            //        this.updateDao = getDomain();
            //        resultVo = manClient.InsertProdWeihTbl(this.updateDao);
            //        if (!resultVo.isSuccess)
            //        {
            //            //실패
            //            WinUIMessageBox.Show(resultVo.Message, "[자재 관리 시스템] 배합표", MessageBoxButton.OK, MessageBoxImage.Error);
            //            return;
            //        }

            //        //성공
            //        WinUIMessageBox.Show("완료 되었습니다", "[추가] 배합표", MessageBoxButton.OK, MessageBoxImage.Information);
            //    }
            //    else
            //    {
            //        this.updateDao = getDomain();
            //        resultVo = manClient.UpdateProdWeihTbl(this.updateDao);
            //        if (!resultVo.isSuccess)
            //        {
            //            //실패
            //            WinUIMessageBox.Show(resultVo.Message, "[자재 관리 시스템] 배합표", MessageBoxButton.OK, MessageBoxImage.Error);
            //            return;
            //        }
            //        //성공
            //        WinUIMessageBox.Show("완료 되었습니다", "[수정] 배합표", MessageBoxButton.OK, MessageBoxImage.Information);

            //        this.orgDao.ASSY_ITM_CD = this.updateDao.ASSY_ITM_CD;
            //        this.orgDao.ASSY_ITM_NM = this.updateDao.ASSY_ITM_NM;
            //        this.orgDao.BSE_WEIH_VAL = this.updateDao.BSE_WEIH_VAL;
            //        this.orgDao.ASSY_ITM_SEQ = this.updateDao.ASSY_ITM_SEQ;
            //        this.orgDao.CMPO_CD = this.updateDao.CMPO_CD;
            //        this.orgDao.CMPO_NM = this.updateDao.CMPO_NM;
            //        this.orgDao.WEIH_VAL = this.updateDao.WEIH_VAL;
            //        this.orgDao.CMPO_TOR_VAL = this.updateDao.CMPO_TOR_VAL;
            //        this.orgDao.CMPO_RMK = this.updateDao.CMPO_RMK;
            //    }
            if (ValueCheckd())
            {
                btn_Save();
                //
                this.DialogResult = true;
                this.Close();
            }
            //}
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void HandleEsc(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.DialogResult = false;
                Close();
            }
        }
        #endregion

        #region Functon (ValueCheckd)
        public Boolean ValueCheckd()
        {
            if (string.IsNullOrEmpty(this.combo_1_Port.Text))
            {
                WinUIMessageBox.Show("[Port] 입력 값이 맞지 않습니다.", "[유효검사] 칭량 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
                this.combo_1_Port.IsTabStop = true;
                this.combo_1_Port.Focus();
                //
                this.tabControl.SelectedIndex = 0;
                return false;
            }
            else if (string.IsNullOrEmpty(this.combo_1_BaudRate.Text))
            {
                WinUIMessageBox.Show("[Baud Rate] 입력 값이 맞지 않습니다.", "[유효검사] 칭량 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
                this.combo_1_BaudRate.IsTabStop = true;
                this.combo_1_BaudRate.Focus();
                //
                this.tabControl.SelectedIndex = 0;
                return false;
            }
            else if (string.IsNullOrEmpty(this.combo_1_Parity.Text))
            {
                WinUIMessageBox.Show("[Parity] 입력 값이 맞지 않습니다.", "[유효검사] 칭량 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
                this.combo_1_Parity.IsTabStop = true;
                this.combo_1_Parity.Focus();
                //
                this.tabControl.SelectedIndex = 0;
                return false;
            }
            else if (string.IsNullOrEmpty(this.combo_1_Length.Text))
            {
                WinUIMessageBox.Show("[Length] 입력 값이 맞지 않습니다.", "[유효검사] 칭량 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
                this.combo_1_Length.IsTabStop = true;
                this.combo_1_Length.Focus();
                //
                this.tabControl.SelectedIndex = 0;
                return false;
            }
            else if (string.IsNullOrEmpty(this.combo_1_StopBit.Text))
            {
                WinUIMessageBox.Show("[Stop Bit] 입력 값이 맞지 않습니다.", "[유효검사] 칭량 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
                this.combo_1_StopBit.IsTabStop = true;
                this.combo_1_StopBit.Focus();
                //
                this.tabControl.SelectedIndex = 0;
                return false;
            }
            else if (string.IsNullOrEmpty(this.combo_1_Type.Text))
            {
                WinUIMessageBox.Show("[전자저울] 입력 값이 맞지 않습니다.", "[유효검사] 칭량 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
                this.combo_1_Type.IsTabStop = true;
                this.combo_1_Type.Focus();
                //
                this.tabControl.SelectedIndex = 0;
                return false;
            }
            else if (this.chk_Tab2.IsChecked == true)
            {
                if (string.IsNullOrEmpty(this.combo_2_Port.Text))
                {
                    WinUIMessageBox.Show("[Port] 입력 값이 맞지 않습니다.", "[유효검사] 칭량 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
                    this.combo_2_Port.IsTabStop = true;
                    this.combo_2_Port.Focus();
                    //
                    this.tabControl.SelectedIndex = 1;
                    return false;
                }
                else if (string.IsNullOrEmpty(this.combo_2_BaudRate.Text))
                {
                    WinUIMessageBox.Show("[Baud Rate] 입력 값이 맞지 않습니다.", "[유효검사] 칭량 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
                    this.combo_2_BaudRate.IsTabStop = true;
                    this.combo_2_BaudRate.Focus();
                    //
                    this.tabControl.SelectedIndex = 1;
                    return false;
                }
                else if (string.IsNullOrEmpty(this.combo_2_Parity.Text))
                {
                    WinUIMessageBox.Show("[Parity] 입력 값이 맞지 않습니다.", "[유효검사] 칭량 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
                    this.combo_2_Parity.IsTabStop = true;
                    this.combo_2_Parity.Focus();
                    //
                    this.tabControl.SelectedIndex = 1;
                    return false;
                }
                else if (string.IsNullOrEmpty(this.combo_2_Length.Text))
                {
                    WinUIMessageBox.Show("[Length] 입력 값이 맞지 않습니다.", "[유효검사] 칭량 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
                    this.combo_2_Length.IsTabStop = true;
                    this.combo_2_Length.Focus();
                    //
                    this.tabControl.SelectedIndex = 1;
                    return false;
                }
                else if (string.IsNullOrEmpty(this.combo_2_StopBit.Text))
                {
                    WinUIMessageBox.Show("[Stop Bit] 입력 값이 맞지 않습니다.", "[유효검사] 칭량 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
                    this.combo_2_StopBit.IsTabStop = true;
                    this.combo_2_StopBit.Focus();
                    //
                    this.tabControl.SelectedIndex = 1;
                    return false;
                }
                else if (string.IsNullOrEmpty(this.combo_2_Type.Text))
                {
                    WinUIMessageBox.Show("[전자저울] 입력 값이 맞지 않습니다.", "[유효검사] 칭량 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
                    this.combo_2_Type.IsTabStop = true;
                    this.combo_2_Type.Focus();
                    //
                    this.tabControl.SelectedIndex = 1;
                    return false;
                }
            }
            return true;
        }


        private void btn_isEnable(bool isCheck)
        {
            this.OKButton.IsEnabled = isCheck;
        }

        private void btn_Save()
        {
            Properties.Settings.Default.str_weighing_Port_1 = this.combo_1_Port.Text;
            Properties.Settings.Default.str_weighing_BaudRate_1 = this.combo_1_BaudRate.Text;
            Properties.Settings.Default.str_weighing_Parity_1 = this.combo_1_Parity.Text;
            Properties.Settings.Default.str_weighing_Length_1 = this.combo_1_Length.Text;
            Properties.Settings.Default.str_weighing_StopBit_1 = this.combo_1_StopBit.Text;
            Properties.Settings.Default.str_weighing_Type_1 = this.combo_1_Type.Text;
            //
            //
            Properties.Settings.Default.str_weighing_Port_2 = this.combo_2_Port.Text;
            Properties.Settings.Default.str_weighing_BaudRate_2 = this.combo_2_BaudRate.Text;
            Properties.Settings.Default.str_weighing_Parity_2 = this.combo_2_Parity.Text;
            Properties.Settings.Default.str_weighing_Length_2 = this.combo_2_Length.Text;
            Properties.Settings.Default.str_weighing_StopBit_2 = this.combo_2_StopBit.Text;
            Properties.Settings.Default.str_weighing_Type_2 = this.combo_2_Type.Text;
            Properties.Settings.Default.is_weighing_Checkd_2 = (this.chk_Tab2.IsChecked == null ? false : true);


            Properties.Settings.Default.Save();
        }


        #endregion

        #region Functon (getDomain - ConfigView1Dao)
        private ManVo getDomain()
        {
            ManVo Dao = new ManVo();
            //Dao.ASSY_ITM_CD = this.orgDao.ASSY_ITM_CD;
            //Dao.BSE_WEIH_VAL = int.Parse(this.text_BSE_WEIH_VAL.Text);
            //Dao.ASSY_ITM_SEQ = int.Parse(this.text_ASSY_ITM_SEQ.Text);
            ////
            //Dao.CMPO_CD = this.text_CMPO_CD.Text;
            //Dao.CMPO_NM = this.text_CMPO_NM.Text;


            //Dao.WEIH_VAL = this.text_WEIH_VAL.Text;
            //Dao.CMPO_TOR_VAL = this.text_CMPO_TOR_VAL.Text;

            //Dao.CMPO_RMK = this.text_CMPO_RMK.Text;
            //Dao.CRE_USR_ID = SystemProperties.USER;
            //Dao.UPD_USR_ID = SystemProperties.USER;
            return Dao;
        }
        #endregion

        private void chk_Tab2_Checked(object sender, RoutedEventArgs e)
        {
            this.tabControl.SelectedIndex = 1;
        }
    }
}
