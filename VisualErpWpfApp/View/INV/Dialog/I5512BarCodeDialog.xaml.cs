using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Auth;
using ModelsLibrary.Code;
using ModelsLibrary.Inv;
using Neodynamic.SDK.Printing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;
using AquilaErpWpfApp3.Util;

namespace AquilaErpWpfApp3.View.INV.Dialog
{
    public partial class I5512BarCodeDialog : DXWindow
    {


        private string sFont = "맑은 고딕";
        private int fontSize = 15;
        private double boder = 0.3;

        private string _title = "품목 출고 관리";
        //private static InvServiceClient invClient = SystemProperties.InvClient;
        private InvVo orgDao;
        //private bool isEdit = false;
        //private InvVo updateDao;

        private PrintJob pj;
        private PrinterSettings _printerSettings;

        public I5512BarCodeDialog(InvVo Dao)
        {
            InitializeComponent();

            //this.combo_RQST_EMPE_NM.ItemsSource = SystemProperties.USER_CODE_VO();
            //this.combo_AREA_CD.ItemsSource = SystemProperties.SYSTEM_CODE_VO("L-002");
            //this.combo_CO_NO.ItemsSource = SystemProperties.CUSTOMER_CODE_VO("AP");
            //this.combo_DO_RQST_TP_NM.ItemsSource = SystemProperties.SYSTEM_CODE_VO("INV07"); 
            //this.combo_RQST_EMPE_ID.ItemsSource = SystemProperties.SYSTEM_CODE_VO("HR04");

            this.orgDao = Dao;

            //InvVo copyDao = new InvVo()
            //{
            //    INSRL_NO = Dao.INSRL_NO,
            //    INAUD_DT = Dao.INAUD_DT,
            //    RQST_EMPE_ID = Dao.RQST_EMPE_ID,
            //    RQST_EMPE_NM = Dao.RQST_EMPE_NM,
            //    N1ST_ITM_GRP_CD = Dao.N1ST_ITM_GRP_CD,
            //    N1ST_ITM_GRP_NM = Dao.N1ST_ITM_GRP_NM,
            //    IO_CD = Dao.IO_CD,
            //    INAUD_RMK = Dao.INAUD_RMK,
            //    AREA_CD = Dao.AREA_CD,
            //    AREA_NM = Dao.AREA_NM,
            //    CO_CD = Dao.CO_CD,
            //    CO_NM = Dao.CO_NM
            //};

            this.lab_N1ST_ITM_GRP_NM.Text = Dao.N1ST_ITM_GRP_NM;
            this.BarCd.EditValue = Dao.INSRL_NO;

            this.text_Cnt.Text = "1";

            //수정
            //if (Dao.INSRL_NO != null)
            //{
            //    this.text_INSRL_NO.IsReadOnly = true;
            //    this.isEdit = true;
            //    //
            //    ////마감 처리 후 수정 불가능
            //    //if (Dao.MODI_FLG.Equals("N"))
            //    //{
            //    //    this.OKButton.IsEnabled = false;
            //    //}
            //    this.INSRL_NO = this.text_INSRL_NO.Text;
            //}
            //else
            //{
            //    //추가
            //    this.text_INSRL_NO.IsReadOnly = true;
            //    this.isEdit = false;
            //    copyDao.INAUD_DT = System.DateTime.Now.ToString("yyyy-MM-dd");

            //    //
            //    //Dao.DO_RQST_GRP_NM = SystemProperties.USERVO.GRP_NM;
            //    //Dao.DO_RQST_USR_NM = SystemProperties.USERVO.USR_N1ST_NM;
            //    //this.combo_DO_RQST_USR_NM.SelectedItem = ((IList<CodeDao>)combo_DO_RQST_USR_NM.ItemsSource)[2];
            //}

            //SYSTEM_CODE_VO();
            //this.combo_CO_NM.ItemsSource = SystemProperties.CUSTOMER_CODE_VO(null, copyDao.AREA_CD);

            //this.configCode.DataContext = copyDao;
            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
            this.CONFIGButton.Click += new RoutedEventHandler(CONFIGButton_Click);
            this.OKButton.Click += new RoutedEventHandler(OKButton_Click);
            this.CancelButton.Click += new RoutedEventHandler(CancelButton_Click);
        }

        private void CONFIGButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Windows.Controls.PrintDialog dialogue = new System.Windows.Controls.PrintDialog();
                if (dialogue.ShowDialog() == true)
                {
                    Properties.Settings.Default.str_PrnNm = dialogue.PrintQueue.FullName;
                    Properties.Settings.Default.Save();
                }
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[에러]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }

        #region Functon (OKButton_Click, CancelButton_Click)
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValueCheckd())
            {
                if (Print_Godex(this.orgDao, Convert.ToInt32( this.text_Cnt.Text)) == true)
                {
                    //성공
                    WinUIMessageBox.Show("완료 되었습니다", _title, MessageBoxButton.OK, MessageBoxImage.Information);

                    this.DialogResult = true;
                    this.Close();
                }
            }
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
            ////if (string.IsNullOrEmpty(this.text_DO_RQST_NO.Text))
            ////{
            ////    WinUIMessageBox.Show("[전표 번호] 입력 값이 맞지 않습니다.", "[유효검사]창고간 이동", MessageBoxButton.OK, MessageBoxImage.Warning);
            ////    this.text_DO_RQST_NO.IsTabStop = true;
            ////    this.text_DO_RQST_NO.Focus();
            ////    return false;
            ////}
            ////else
            //if (string.IsNullOrEmpty(this.text_INAUD_DT.Text))
            //{
            //    WinUIMessageBox.Show("[출고 일자] 입력 값이 맞지 않습니다.", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.text_INAUD_DT.IsTabStop = true;
            //    this.text_INAUD_DT.Focus();
            //    return false;
            //}
            //else if (string.IsNullOrEmpty(this.combo_CO_NM.Text))
            //{
            //    WinUIMessageBox.Show("[거래처] 입력 값이 맞지 않습니다.", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.combo_CO_NM.IsTabStop = true;
            //    this.combo_CO_NM.Focus();
            //    return false;
            //}
            //else if (string.IsNullOrEmpty(this.combo_RQST_EMPE_NM.Text))
            //{
            //    WinUIMessageBox.Show("[작성자] 입력 값이 맞지 않습니다.", "[유효검사]" +_title, MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.combo_RQST_EMPE_NM.IsTabStop = true;
            //    this.combo_RQST_EMPE_NM.Focus();
            //    return false;
            //}
            ////else if (string.IsNullOrEmpty(this.text_PUR_CLZ_FLG.Text))
            ////{
            ////    WinUIMessageBox.Show("[마감 유무] 입력 값이 맞지 않습니다.", "[유효검사]품목 입고 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
            ////    this.text_PUR_CLZ_FLG.IsTabStop = true;
            ////    this.text_PUR_CLZ_FLG.Focus();
            ////    return false;
            ////}
            //    //else if (string.IsNullOrEmpty(this.combo_RQST_EMPE_ID.Text))
            //    //{
            //    //    WinUIMessageBox.Show("[요청자] 입력 값이 맞지 않습니다.", "[유효검사]창고간 이동", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    //    this.combo_RQST_EMPE_ID.IsTabStop = true;
            //    //    this.combo_RQST_EMPE_ID.Focus();
            //    //    return false;
            //    //}
            //    //else if (string.IsNullOrEmpty(this.text_CAR_NO.Text))
            //    //{
            //    //    WinUIMessageBox.Show("[차량 넘버] 입력 값이 맞지 않습니다.", "[유효검사]창고간 이동", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    //    this.text_CAR_NO.IsTabStop = true;
            //    //    this.text_CAR_NO.Focus();
            //    //    return false;
            //    //}
            ////else
            ////{
            ////    //서버와 날짜 체크

            ////    if (this.isEdit == false)
            ////    {
            ////        InauditVo dao = new InauditVo()
            ////        {
            ////            INAUD_DT = Convert.ToDateTime(this.text_INAUD_DT.Text)
            ////        };

            ////        InauditVo daoList = inauditclient.SelectInvtInaudCheckTime(dao);
            ////        if (daoList.MODI_FLG.Equals("N"))
            ////        {
            ////            WinUIMessageBox.Show("[이동 일자] 일자를 다시 입력 하십시오.", "[날짜검사]창고간 이동", MessageBoxButton.OK, MessageBoxImage.Warning);
            ////            this.text_INAUD_DT.IsTabStop = true;
            ////            this.text_INAUD_DT.Focus();
            ////            return false;
            ////        }
            ////    }
            ////    //if (this.isEdit == false)
            ////    //{
            ////    //    InauditVo dao = new InauditVo()
            ////    //    {
            ////    //        INSRL_NO = this.text_INSRL_NO.Text,
            ////    //    };
            ////    //    IList<InauditVo> daoList = (IList<InauditVo>)inauditclient.SelectInvtInaudMastList(dao);
            ////    //    if (daoList.Count != 0)
            ////    //    {
            ////    //        WinUIMessageBox.Show("[코드 - 중복] 코드를 다시 입력 하십시오.", "[중복검사]창고간 이동", MessageBoxButton.OK, MessageBoxImage.Warning);
            ////    //        this.text_INSRL_NO.IsTabStop = true;
            ////    //        this.text_INSRL_NO.Focus();
            ////    //        return false;
            ////    //    }
            ////    //}
            ////}
            return true;
        }
        #endregion

        #region Functon (getDomain - ConfigView1Dao)
        //private InvVo getDomain()
        //{
        //    InvVo Dao = new InvVo();

        //    //Dao.INSRL_NO = this.text_INSRL_NO.Text;
        //    //Dao.INAUD_DT = Convert.ToDateTime(this.text_INAUD_DT.Text).ToString("yyyy-MM-dd");

        //    //SystemCodeVo areaVo = this.combo_AREA_CD.SelectedItem as SystemCodeVo;
        //    //Dao.AREA_CD = areaVo.CLSS_CD;
        //    //Dao.AREA_NM = areaVo.CLSS_DESC;

        //    //SystemCodeVo coNmVo = this.combo_CO_NM.SelectedItem as SystemCodeVo;
        //    //Dao.CO_NO = coNmVo.CO_NO;
        //    //Dao.CO_NM = coNmVo.CO_NM;

        //    //GroupUserVo purEmpeIdVo = this.combo_RQST_EMPE_NM.SelectedItem as GroupUserVo;
        //    //Dao.RQST_EMPE_ID = purEmpeIdVo.USR_ID;
        //    //Dao.RQST_EMPE_NM = purEmpeIdVo.USR_N1ST_NM;

        //    //Dao.INAUD_RMK = this.text_INAUD_RMK.Text;

        //    //Dao.IO_CD = "O";

        //    Dao.CRE_USR_ID = SystemProperties.USER;
        //    Dao.UPD_USR_ID = SystemProperties.USER;
        //    Dao.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;

        //    return Dao;
        //}
        #endregion


        //public async void SYSTEM_CODE_VO()
        //{
            //this.combo_RQST_EMPE_NM.ItemsSource = SystemProperties.USER_CODE_VO();
            //using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s136/usr", new StringContent(JsonConvert.SerializeObject(new GroupUserVo() { DELT_FLG = "N", CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
            //{
            //    if (response.IsSuccessStatusCode)
            //    {
            //        this.combo_RQST_EMPE_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<GroupUserVo>>(await response.Content.ReadAsStringAsync()).Cast<GroupUserVo>().ToList();
            //    }
            //}

            ////this.combo_AREA_CD.ItemsSource = SystemProperties.SYSTEM_CODE_VO("L-002");
            //using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "L-002"))
            //{
            //    if (response.IsSuccessStatusCode)
            //    {
            //        this.combo_AREA_CD.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
            //    }
            //}

            ////this.combo_CO_NM.ItemsSource = SystemProperties.CUSTOMER_CODE_VO(null, null);
            //using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s143", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { DELT_FLG = "N", CO_TP_CD = null, AREA_CD = SystemProperties.USER_VO.EMPE_PLC_NM, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
            //{
            //    if (response.IsSuccessStatusCode)
            //    {
            //        this.combo_CO_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
            //    }
            //}
        //}


        public bool Print_Godex(InvVo Vo, int CNT = 0)
        {
            try
            {
                if (string.IsNullOrEmpty(Properties.Settings.Default.str_PrnNm))
                {
                    WinUIMessageBox.Show("[바코드 - 설정]바코드 프린터가 연결 되지 않았습니다.", "[장치연결]바코드", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }

                _printerSettings = new PrinterSettings();
                _printerSettings.Communication.CommunicationType = CommunicationType.USB;
                _printerSettings.PrinterName = Properties.Settings.Default.str_PrnNm;

                using (pj = new PrintJob(_printerSettings))
                {
                    //pj.Copies = 1;
                    pj.Replicates = CNT;
                    pj.PrintOrientation = PrintOrientation.Portrait;
                    pj.ThermalLabel = GenerateBasicThermalLabel(Vo);
                    pj.Print();

                    return true;
                }

            }
            catch (Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[에러]" + SystemProperties.PROGRAM_TITLE, MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
        }

        private ThermalLabel GenerateBasicThermalLabel(InvVo Vo)
        {
            //Define a ThermalLabel object and set unit to inch and label size
            ThermalLabel tLabel = new ThermalLabel(UnitType.Mm, 100, 80);
            tLabel.GapLength = 0.1;

            TextItem txtItem_ITDSC = new TextItem(10, 17.5, 2.5, 0.5, Vo.INSRL_NO);
            txtItem_ITDSC.Font.Name = this.sFont;
            txtItem_ITDSC.Font.Size = 9;
            //txtItem_ITDSC.Font.Bold = true;
            txtItem_ITDSC.Height = 14;
            txtItem_ITDSC.Width = 20;
            txtItem_ITDSC.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC.TextPadding = new FrameThickness(0, txtItem_ITDSC.Height / 4, 0, 0);
            //txtItem_ITDSC.BorderThickness = new FrameThickness(boder);
            ////txtItem_MTRL_LOT_NO.BackColor = Color.Black;
            ////txtItem_MTRL_LOT_NO.ForeColor = Color.White;
            ////txtItem_MTRL_LOT_NO.BorderColor = Color.White;

            TextItem txtItem_ITDSC_VAL = new TextItem(1.7, 1.5, 2.5, 0.5, " " );
            txtItem_ITDSC_VAL.Font.Name = this.sFont;
            txtItem_ITDSC_VAL.Font.Size = this.fontSize - 1;
            txtItem_ITDSC_VAL.Height = 25;
            txtItem_ITDSC_VAL.Width = 93;
            txtItem_ITDSC_VAL.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Left;
            txtItem_ITDSC_VAL.TextPadding = new FrameThickness(0, txtItem_ITDSC_VAL.Height / 4, 0, 0);
            txtItem_ITDSC_VAL.BorderThickness = new FrameThickness(boder);


            //Define a BarcodeItem object
            BarcodeItem bcItem_ITNBR_VAL = new BarcodeItem(10, 2, 30, 20, BarcodeSymbology.Code128, Vo.INSRL_NO);
            //set counter step for increasing by 1
            bcItem_ITNBR_VAL.CounterStep = 1;
            //set barcode size
            bcItem_ITNBR_VAL.BarWidth = 0.02;
            bcItem_ITNBR_VAL.BarHeight = 0.75;
            bcItem_ITNBR_VAL.Sizing = BarcodeSizing.Fill;
            //set barcode alignment
            bcItem_ITNBR_VAL.BarcodeAlignment = BarcodeAlignment.MiddleCenter;
            ////set font
            //bcItem_ITNBR_VAL.Font.Name = this.sFont;
            //bcItem_ITNBR_VAL.Font.Unit = FontUnit.Point;
            //bcItem_ITNBR_VAL.Font.Size = 4;
            bcItem_ITNBR_VAL.DisplayCode = false;



            TextItem txtItem_ISPEC = new TextItem(1.7, 26.5, 2.5, 0.5, "차종 품명");
            txtItem_ISPEC.Font.Name = this.sFont;
            txtItem_ISPEC.Font.Size = this.fontSize - 3;
            //txtItem_ISPEC.Font.Bold = true;
            txtItem_ISPEC.Height = 14;
            txtItem_ISPEC.Width = 40;
            txtItem_ISPEC.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ISPEC.TextPadding = new FrameThickness(0, txtItem_ISPEC.Height / 4, 0, 0);
            txtItem_ISPEC.BorderThickness = new FrameThickness(boder);
            //txtItem_MTRL_LOT_NO.BackColor = Color.Black;
            //txtItem_MTRL_LOT_NO.ForeColor = Color.White;
            //txtItem_MTRL_LOT_NO.BorderColor = Color.White;


            TextItem txtItem_ISPEC_VAL = new TextItem(41.4, 26.5, 2.5, 0.5, "BOX");
            txtItem_ISPEC_VAL.Font.Name = this.sFont;
            txtItem_ISPEC_VAL.Font.Size = this.fontSize - 3;
            txtItem_ISPEC_VAL.Height = 14;
            txtItem_ISPEC_VAL.Width = 13;
            txtItem_ISPEC_VAL.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ISPEC_VAL.TextPadding = new FrameThickness(0, txtItem_ISPEC_VAL.Height / 4, 0, 0);
            txtItem_ISPEC_VAL.BorderThickness = new FrameThickness(boder);




            TextItem txtItem_MAXQT = new TextItem(54.4, 26.5, 2.5, 0.5, "수량");
            txtItem_MAXQT.Font.Name = this.sFont;
            txtItem_MAXQT.Font.Size = this.fontSize - 3;
            txtItem_MAXQT.Font.Bold = true;
            txtItem_MAXQT.Height = 14;
            txtItem_MAXQT.Width = 20;
            txtItem_MAXQT.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_MAXQT.TextPadding = new FrameThickness(0, txtItem_MAXQT.Height / 4, 0, 0);
            txtItem_MAXQT.BorderThickness = new FrameThickness(boder);
            //txtItem_MTRL_LOT_NO.BackColor = Color.Black;
            //txtItem_MTRL_LOT_NO.ForeColor = Color.White;
            //txtItem_MTRL_LOT_NO.BorderColor = Color.White;

            TextItem txtItem_MAXQT_VAL = new TextItem(74.4, 26.5, 2.5, 0.5, "총수량");
            txtItem_MAXQT_VAL.Font.Name = this.sFont;
            txtItem_MAXQT_VAL.Font.Size = this.fontSize - 3;
            txtItem_MAXQT_VAL.Height = 14;
            txtItem_MAXQT_VAL.Width = 20.3;
            txtItem_MAXQT_VAL.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_MAXQT_VAL.TextPadding = new FrameThickness(0, txtItem_MAXQT_VAL.Height / 4, 0, 0);
            txtItem_MAXQT_VAL.BorderThickness = new FrameThickness(boder);



            //
            //
            TextItem txtItem_ISPEC1 = new TextItem(1.7, 40.5, 2.5, 0.5, Vo.N1ST_ITM_GRP_NM);
            txtItem_ISPEC1.Font.Name = this.sFont;
            txtItem_ISPEC1.Font.Size = this.fontSize;
            //txtItem_ISPEC.Font.Bold = true;
            txtItem_ISPEC1.Height = 30;
            txtItem_ISPEC1.Width = 40;
            txtItem_ISPEC1.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ISPEC1.TextPadding = new FrameThickness(0, txtItem_ISPEC1.Height / 4, 0, 0);
            txtItem_ISPEC1.BorderThickness = new FrameThickness(boder);
            //txtItem_MTRL_LOT_NO.BackColor = Color.Black;
            //txtItem_MTRL_LOT_NO.ForeColor = Color.White;
            //txtItem_MTRL_LOT_NO.BorderColor = Color.White;


            TextItem txtItem_ISPEC_VAL1 = new TextItem(41.4, 40.5, 2.5, 0.5, " ");
            txtItem_ISPEC_VAL1.Font.Name = this.sFont;
            txtItem_ISPEC_VAL1.Font.Size = this.fontSize;
            txtItem_ISPEC_VAL1.Height = 30;
            txtItem_ISPEC_VAL1.Width = 13;
            txtItem_ISPEC_VAL1.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ISPEC_VAL1.TextPadding = new FrameThickness(0, txtItem_ISPEC_VAL1.Height / 4, 0, 0);
            txtItem_ISPEC_VAL1.BorderThickness = new FrameThickness(boder);




            TextItem txtItem_MAXQT1 = new TextItem(54.4, 40.5, 2.5, 0.5, " ");
            txtItem_MAXQT1.Font.Name = this.sFont;
            txtItem_MAXQT1.Font.Size = this.fontSize;
            //txtItem_MAXQT1.Font.Bold = true;
            txtItem_MAXQT1.Height = 30;
            txtItem_MAXQT1.Width = 20;
            txtItem_MAXQT1.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_MAXQT1.TextPadding = new FrameThickness(0, txtItem_MAXQT1.Height / 4, 0, 0);
            txtItem_MAXQT1.BorderThickness = new FrameThickness(boder);
            //txtItem_MTRL_LOT_NO.BackColor = Color.Black;
            //txtItem_MTRL_LOT_NO.ForeColor = Color.White;
            //txtItem_MTRL_LOT_NO.BorderColor = Color.White;

            TextItem txtItem_MAXQT_VAL1 = new TextItem(74.4, 40.5, 2.5, 0.5, " ");
            txtItem_MAXQT_VAL1.Font.Name = this.sFont;
            txtItem_MAXQT_VAL1.Font.Size = this.fontSize;
            txtItem_MAXQT_VAL1.Height = 30;
            txtItem_MAXQT_VAL1.Width = 20.3;
            txtItem_MAXQT_VAL1.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_MAXQT_VAL1.TextPadding = new FrameThickness(0, txtItem_MAXQT_VAL1.Height / 4, 0, 0);
            txtItem_MAXQT_VAL1.BorderThickness = new FrameThickness(boder);



            //TextItem txtItem_ITNBR = new TextItem(1.7, 43.5, 2.5, 0.5, "품  번");
            //txtItem_ITNBR.Font.Name = this.sFont;
            //txtItem_ITNBR.Font.Size = this.fontSize;
            //txtItem_ITNBR.Font.Bold = true;
            //txtItem_ITNBR.Height = 14;
            //txtItem_ITNBR.Width = 20;
            //txtItem_ITNBR.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            //txtItem_ITNBR.TextPadding = new FrameThickness(0, txtItem_ITNBR.Height / 4, 0, 0);
            //txtItem_ITNBR.BorderThickness = new FrameThickness(boder);
            ////txtItem_MTRL_LOT_NO.BackColor = Color.Black;
            ////txtItem_MTRL_LOT_NO.ForeColor = Color.White;
            ////txtItem_MTRL_LOT_NO.BorderColor = Color.White;

            //////Define a BarcodeItem object (Linear 1D)
            ////BarcodeItem bcItem_ITNBR_VAL = new BarcodeItem(21.4, 43.5, 73, 14, BarcodeSymbology.Code128, Vo.ITNBR);
            //////bcItem_ITNBR_VAL.Height = 14;
            //////bcItem_ITNBR_VAL.Width = 73;
            //////Set bars height to .75inch
            ////bcItem_ITNBR_VAL.BarHeight = 0.75;
            //////Set bars width to 0.0104inch
            ////bcItem_ITNBR_VAL.BarWidth = 0.0104;
            ////bcItem_ITNBR_VAL.Sizing = BarcodeSizing.Fill;

            //TextItem txtItem_ITNBR_VAL = new TextItem(21.4, 43.5, 2.5, 0.5, "      " + "Vo.ITNBR");
            //txtItem_ITNBR_VAL.Font.Name = this.sFont;
            //txtItem_ITNBR_VAL.Font.Size = 8;
            //txtItem_ITNBR_VAL.Height = 14;
            //txtItem_ITNBR_VAL.Width = 73;
            //txtItem_ITNBR_VAL.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Left;
            //txtItem_ITNBR_VAL.TextPadding = new FrameThickness(0, txtItem_ITNBR_VAL.Height - 4, 0, 0);
            //txtItem_ITNBR_VAL.BorderThickness = new FrameThickness(boder);


            ////Define a BarcodeItem object
            //BarcodeItem bcItem_ITNBR_VAL = new BarcodeItem(25.4, 44, 30, 11, BarcodeSymbology.Code128, "VoITNBR");
            ////set counter step for increasing by 1
            //bcItem_ITNBR_VAL.CounterStep = 1;
            ////set barcode size
            //bcItem_ITNBR_VAL.BarWidth = 0.02;
            //bcItem_ITNBR_VAL.BarHeight = 0.75;
            //bcItem_ITNBR_VAL.Sizing = BarcodeSizing.Fill;
            ////set barcode alignment
            //bcItem_ITNBR_VAL.BarcodeAlignment = BarcodeAlignment.MiddleCenter;
            //////set font
            ////bcItem_ITNBR_VAL.Font.Name = this.sFont;
            ////bcItem_ITNBR_VAL.Font.Unit = FontUnit.Point;
            ////bcItem_ITNBR_VAL.Font.Size = 4;
            //bcItem_ITNBR_VAL.DisplayCode = false;

            ////bcItem_ITNBR_VAL.BarcodeAlignment = BarcodeAlignment.MiddleCenter;
            ////bcItem_ITNBR_VAL.BorderThickness = new FrameThickness(boder);


            ////TextItem txtItem_REMARK = new TextItem(1.7, 57.5, 2.5, 0.5, "株式會社  大 吉 通 商");
            //TextItem txtItem_REMARK = new TextItem(1.7, 57.5, 2.5, 0.5, "(주) 대 길 통 상");
            //txtItem_REMARK.Font.Name = this.sFont;
            //txtItem_REMARK.Font.Size = this.fontSize + 10;
            //txtItem_REMARK.Font.Bold = true;
            //txtItem_REMARK.Height = 21;
            //txtItem_REMARK.Width = 92.5;
            //txtItem_REMARK.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            //txtItem_REMARK.TextPadding = new FrameThickness(0, 0.2, 0, 0);
            //txtItem_REMARK.BorderThickness = new FrameThickness(boder);

            //TextItem txtItem_REMARK1 = new TextItem(1.7, 68.5, 92.5, 21, " 우)46727  부산시 강서구 범방3로 29번길 7");
            //txtItem_REMARK1.Font.Name = this.sFont;
            //txtItem_REMARK1.Font.Size = this.fontSize - 5;
            //txtItem_REMARK1.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Left;
            //txtItem_REMARK1.TextPadding = new FrameThickness(5, 0, 0, 0);

            //TextItem txtItem_REMARK2 = new TextItem(1.7, 72.5, 92.5, 21, " TEL : (051)314-6300");
            //txtItem_REMARK2.Font.Name = this.sFont;
            //txtItem_REMARK2.Font.Size = this.fontSize - 5;
            //txtItem_REMARK2.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Left;
            //txtItem_REMARK2.TextPadding = new FrameThickness(5, 0, 0, 0);



            tLabel.Items.Add(txtItem_ITDSC);
            tLabel.Items.Add(txtItem_ITDSC_VAL);

            tLabel.Items.Add(txtItem_ISPEC);
            tLabel.Items.Add(txtItem_ISPEC_VAL);

            tLabel.Items.Add(txtItem_MAXQT);
            tLabel.Items.Add(txtItem_MAXQT_VAL);

            //
            //
            tLabel.Items.Add(txtItem_ISPEC1);
            tLabel.Items.Add(txtItem_ISPEC_VAL1);

            tLabel.Items.Add(txtItem_MAXQT1);
            tLabel.Items.Add(txtItem_MAXQT_VAL1);


            //tLabel.Items.Add(txtItem_MAXQT);
            //tLabel.Items.Add(txtItem_MAXQT_VAL);

            //tLabel.Items.Add(txtItem_ITNBR);
            tLabel.Items.Add(bcItem_ITNBR_VAL);
            //tLabel.Items.Add(txtItem_ITNBR_VAL);

            //tLabel.Items.Add(txtItem_REMARK);
            //tLabel.Items.Add(txtItem_REMARK1);
            //tLabel.Items.Add(txtItem_REMARK2);



            return tLabel;
        }





        //IsEdit
        //public bool IsEdit
        //{
        //    get
        //    {
        //        return this.isEdit;
        //    }
        //}

        public string INSRL_NO
        {
            get;
            set;
        }
    }
}
