using AquilaErpWpfApp3.Utillity;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Code;
using ModelsLibrary.Man;
using Neodynamic.SDK.Printing;
using System;
using System.Threading;
using System.Windows;

namespace AquilaErpWpfApp3.Util
{
    class BarPrint
    {
        //private double _dpi = 96;
        //private ThermalLabel _currentThermalLabel = null;
        //private int _currentDemoIndex = -1;
        //private ImageSettings _imgSettings = new ImageSettings();

        private PrintJob pj;
        //private System.IO.MemoryStream ms;
        private PrinterSettings _printerSettings;

        private string sFont = "맑은 고딕";
        private int fontSize = 15;
        private double boder = 0.3;



        //원자재
        public bool M_Godex(LabelDao _vo, int _cnt = 1)
        {
            try
            {


                _printerSettings = new PrinterSettings();
                _printerSettings.Communication.CommunicationType = CommunicationType.USB;
                _printerSettings.PrinterName = Properties.Settings.Default.str_PrnNm;

                using (pj = new PrintJob(_printerSettings))
                {
                    pj.BufferOutput = true;
                    pj.Copies = _cnt;
                    pj.Replicates = 1;
                    pj.PrintOrientation = PrintOrientation.Portrait;
                    pj.ThermalLabel = GenerateBasicThermalLabel_M(_vo);
                    pj.Print();

                    return true;
                }

            }
            catch (Exception eLog)
            {
                //WinUIMessageBox.Show(eLog.Message, "[에러]" + SystemProperties.PROGRAM_TITLE, MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
        }

        #region 품질팀 요청
        ////원자재
        //private ThermalLabel GenerateBasicThermalLabel_M(LabelDao _vo)
        //{
        //    ThermalLabel tLabel = new ThermalLabel(UnitType.Mm, 100, 80);
        //    tLabel.GapLength = 0.1;


        //    TextItem txtItem_ITDSC = new TextItem(4.0, 1.0, 1.0, 1.0, "품명");
        //    txtItem_ITDSC.Font.Name = this.sFont;
        //    txtItem_ITDSC.Font.Size = this.fontSize - 3;
        //    txtItem_ITDSC.Font.Bold = true;
        //    txtItem_ITDSC.Height = 9;
        //    txtItem_ITDSC.Width = 20;
        //    txtItem_ITDSC.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
        //    txtItem_ITDSC.TextPadding = new FrameThickness(0, txtItem_ITDSC.Height / 4, 0, 0);
        //    txtItem_ITDSC.BorderThickness = new FrameThickness(boder);

        //    TextItem txtItem_ITDSC1 = new TextItem(4.0, 10.0, 1.0, 1.0, "품번");
        //    txtItem_ITDSC1.Font.Name = this.sFont;
        //    txtItem_ITDSC1.Font.Size = this.fontSize - 3;
        //    txtItem_ITDSC1.Font.Bold = true;
        //    txtItem_ITDSC1.Height = 9;
        //    txtItem_ITDSC1.Width = 20;
        //    txtItem_ITDSC1.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
        //    txtItem_ITDSC1.TextPadding = new FrameThickness(0, txtItem_ITDSC.Height / 4, 0, 0);
        //    txtItem_ITDSC1.BorderThickness = new FrameThickness(boder);

        //    TextItem txtItem_ITDSC2 = new TextItem(4.0, 19.0, 1.0, 1.0, "수량");
        //    txtItem_ITDSC2.Font.Name = this.sFont;
        //    txtItem_ITDSC2.Font.Size = this.fontSize - 3;
        //    txtItem_ITDSC2.Font.Bold = true;
        //    txtItem_ITDSC2.Height = 9;
        //    txtItem_ITDSC2.Width = 20;
        //    txtItem_ITDSC2.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
        //    txtItem_ITDSC2.TextPadding = new FrameThickness(0, txtItem_ITDSC.Height / 4, 0, 0);
        //    txtItem_ITDSC2.BorderThickness = new FrameThickness(boder);

        //    TextItem txtItem_ITDSC3 = new TextItem(4.0, 28.0, 1.0, 1.0, "입고일자");
        //    txtItem_ITDSC3.Font.Name = this.sFont;
        //    txtItem_ITDSC3.Font.Size = this.fontSize - 3;
        //    txtItem_ITDSC3.Font.Bold = true;
        //    txtItem_ITDSC3.Height = 9;
        //    txtItem_ITDSC3.Width = 20;
        //    txtItem_ITDSC3.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
        //    txtItem_ITDSC3.TextPadding = new FrameThickness(0, txtItem_ITDSC.Height / 4, 0, 0);
        //    txtItem_ITDSC3.BorderThickness = new FrameThickness(boder);

        //    TextItem txtItem_ITDSC4 = new TextItem(4.0, 37.0, 1.0, 1.0, "제조원");
        //    txtItem_ITDSC4.Font.Name = this.sFont;
        //    txtItem_ITDSC4.Font.Size = this.fontSize - 3;
        //    txtItem_ITDSC4.Font.Bold = true;
        //    txtItem_ITDSC4.Height = 9;
        //    txtItem_ITDSC4.Width = 20;
        //    txtItem_ITDSC4.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
        //    txtItem_ITDSC4.TextPadding = new FrameThickness(0, txtItem_ITDSC.Height / 4, 0, 0);
        //    txtItem_ITDSC4.BorderThickness = new FrameThickness(boder);


        //    TextItem txtItem_ITDSC5 = new TextItem(4.0, 46.0, 1.0, 1.0, "시험일");
        //    txtItem_ITDSC5.Font.Name = this.sFont;
        //    txtItem_ITDSC5.Font.Size = this.fontSize - 3;
        //    txtItem_ITDSC5.Font.Bold = true;
        //    txtItem_ITDSC5.Height = 9;
        //    txtItem_ITDSC5.Width = 20;
        //    txtItem_ITDSC5.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
        //    txtItem_ITDSC5.TextPadding = new FrameThickness(0, txtItem_ITDSC.Height / 4, 0, 0);
        //    txtItem_ITDSC5.BorderThickness = new FrameThickness(boder);


        //    TextItem txtItem_ITDSC6 = new TextItem(4.0, 55.0, 1.0, 1.0, "참고사항");
        //    txtItem_ITDSC6.Font.Name = this.sFont;
        //    txtItem_ITDSC6.Font.Size = this.fontSize - 3;
        //    txtItem_ITDSC6.Font.Bold = true;
        //    txtItem_ITDSC6.Height = 9;
        //    txtItem_ITDSC6.Width = 20;
        //    txtItem_ITDSC6.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
        //    txtItem_ITDSC6.TextPadding = new FrameThickness(0, txtItem_ITDSC.Height / 4, 0, 0);
        //    txtItem_ITDSC6.BorderThickness = new FrameThickness(boder);

        //    TextItem txtItem_ITDSC7 = new TextItem(4.0, 64.0, 1.0, 1.0, "분류");
        //    txtItem_ITDSC7.Font.Name = this.sFont;
        //    txtItem_ITDSC7.Font.Size = this.fontSize - 3;
        //    txtItem_ITDSC7.Font.Bold = true;
        //    txtItem_ITDSC7.Height = 10;
        //    txtItem_ITDSC7.Width = 20;
        //    txtItem_ITDSC7.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
        //    txtItem_ITDSC7.TextPadding = new FrameThickness(0, txtItem_ITDSC.Height / 4, 0, 0);
        //    txtItem_ITDSC7.BorderThickness = new FrameThickness(boder);



        //    TextItem txtItem_ITDSC16 = new TextItem(52.0, 19.0, 1.0, 1.0, "보관위치");
        //    txtItem_ITDSC16.Font.Name = this.sFont;
        //    txtItem_ITDSC16.Font.Size = this.fontSize - 3;
        //    txtItem_ITDSC16.Font.Bold = true;
        //    txtItem_ITDSC16.Height = 9;
        //    txtItem_ITDSC16.Width = 20;
        //    txtItem_ITDSC16.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
        //    txtItem_ITDSC16.TextPadding = new FrameThickness(0, txtItem_ITDSC.Height / 4, 0, 0);
        //    txtItem_ITDSC16.BorderThickness = new FrameThickness(boder);

        //    TextItem txtItem_ITDSC17 = new TextItem(52.0, 28.0, 1.0, 1.0, "유효기간");
        //    txtItem_ITDSC17.Font.Name = this.sFont;
        //    txtItem_ITDSC17.Font.Size = this.fontSize - 3;
        //    txtItem_ITDSC17.Font.Bold = true;
        //    txtItem_ITDSC17.Height = 9;
        //    txtItem_ITDSC17.Width = 20;
        //    txtItem_ITDSC17.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
        //    txtItem_ITDSC17.TextPadding = new FrameThickness(0, txtItem_ITDSC.Height / 4, 0, 0);
        //    txtItem_ITDSC17.BorderThickness = new FrameThickness(boder);


        //    TextItem txtItem_ITDSC18 = new TextItem(52.0, 37.0, 1.0, 1.0, "제조번호");
        //    txtItem_ITDSC18.Font.Name = this.sFont;
        //    txtItem_ITDSC18.Font.Size = this.fontSize - 3;
        //    txtItem_ITDSC18.Font.Bold = true;
        //    txtItem_ITDSC18.Height = 9;
        //    txtItem_ITDSC18.Width = 20;
        //    txtItem_ITDSC18.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
        //    txtItem_ITDSC18.TextPadding = new FrameThickness(0, txtItem_ITDSC.Height / 4, 0, 0);
        //    txtItem_ITDSC18.BorderThickness = new FrameThickness(boder);


        //    TextItem txtItem_ITDSC19 = new TextItem(52.0, 46.0, 1.0, 1.0, "시험번호");
        //    txtItem_ITDSC19.Font.Name = this.sFont;
        //    txtItem_ITDSC19.Font.Size = this.fontSize - 3;
        //    txtItem_ITDSC19.Font.Bold = true;
        //    txtItem_ITDSC19.Height = 9;
        //    txtItem_ITDSC19.Width = 20;
        //    txtItem_ITDSC19.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
        //    txtItem_ITDSC19.TextPadding = new FrameThickness(0, txtItem_ITDSC.Height / 4, 0, 0);
        //    txtItem_ITDSC19.BorderThickness = new FrameThickness(boder);


        //    //품명
        //    TextItem txtItem_ITDSC8 = new TextItem(24.0, 1.0, 1.0, 1.0, _vo.A01.ToString());
        //    txtItem_ITDSC8.Font.Name = this.sFont;
        //    txtItem_ITDSC8.Font.Size = this.fontSize - 3;
        //    txtItem_ITDSC8.Font.Bold = true;
        //    txtItem_ITDSC8.Height = 9;
        //    txtItem_ITDSC8.Width = 57;
        //    txtItem_ITDSC8.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
        //    txtItem_ITDSC8.TextPadding = new FrameThickness(0, txtItem_ITDSC8.Height / 4, 0, 0);
        //    txtItem_ITDSC8.BorderThickness = new FrameThickness(boder);

        //    //품번
        //    TextItem txtItem_ITDSC9 = new TextItem(24.0, 10.0, 1.0, 1.0, _vo.A02.ToString());
        //    txtItem_ITDSC9.Font.Name = this.sFont;
        //    txtItem_ITDSC9.Font.Size = this.fontSize - 3;
        //    txtItem_ITDSC9.Font.Bold = true;
        //    txtItem_ITDSC9.Height = 9;
        //    txtItem_ITDSC9.Width = 57;
        //    txtItem_ITDSC9.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
        //    txtItem_ITDSC9.TextPadding = new FrameThickness(0, txtItem_ITDSC9.Height / 4, 0, 0);
        //    txtItem_ITDSC9.BorderThickness = new FrameThickness(boder);

        //    //수량
        //    TextItem txtItem_ITDSC10 = new TextItem(24.0, 19.0, 1.0, 1.0, String.Format("{0:##,###,###,###,##0.00}", Convert.ToDouble(_vo.A03)));
        //    txtItem_ITDSC10.Font.Name = this.sFont;
        //    txtItem_ITDSC10.Font.Size = this.fontSize - 3;
        //    txtItem_ITDSC10.Font.Bold = true;
        //    txtItem_ITDSC10.Height = 9;
        //    txtItem_ITDSC10.Width = 28;
        //    txtItem_ITDSC10.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
        //    txtItem_ITDSC10.TextPadding = new FrameThickness(0, txtItem_ITDSC10.Height / 4, 0, 0);
        //    txtItem_ITDSC10.BorderThickness = new FrameThickness(boder);

        //    //입고일자
        //    TextItem txtItem_ITDSC11 = new TextItem(24.0, 28.0, 1.0, 1.0, (_vo.A11 == null ? "" : _vo.A11.ToString()));
        //    txtItem_ITDSC11.Font.Name = this.sFont;
        //    txtItem_ITDSC11.Font.Size = this.fontSize - 3;
        //    txtItem_ITDSC11.Font.Bold = true;
        //    txtItem_ITDSC11.Height = 9;
        //    txtItem_ITDSC11.Width = 28;
        //    txtItem_ITDSC11.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
        //    txtItem_ITDSC11.TextPadding = new FrameThickness(0, txtItem_ITDSC11.Height / 4, 0, 0);
        //    txtItem_ITDSC11.BorderThickness = new FrameThickness(boder);

        //    //제조원
        //    TextItem txtItem_ITDSC12 = new TextItem(24.0, 37.0, 1.0, 1.0, (_vo.A05 == null ? "" : _vo.A05.ToString()));
        //    txtItem_ITDSC12.Font.Name = this.sFont;
        //    txtItem_ITDSC12.Font.Size = this.fontSize - 3;
        //    txtItem_ITDSC12.Font.Bold = true;
        //    txtItem_ITDSC12.Height = 9;
        //    txtItem_ITDSC12.Width = 28;
        //    txtItem_ITDSC12.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
        //    txtItem_ITDSC12.TextPadding = new FrameThickness(0, txtItem_ITDSC12.Height / 4, 0, 0);
        //    txtItem_ITDSC12.BorderThickness = new FrameThickness(boder);

        //    //시험일
        //    TextItem txtItem_ITDSC13 = new TextItem(24.0, 46.0, 1.0, 1.0, (_vo.A07 == null ? "" : _vo.A07.ToString()));
        //    txtItem_ITDSC13.Font.Name = this.sFont;
        //    txtItem_ITDSC13.Font.Size = this.fontSize - 3;
        //    txtItem_ITDSC13.Font.Bold = true;
        //    txtItem_ITDSC13.Height = 9;
        //    txtItem_ITDSC13.Width = 28;
        //    txtItem_ITDSC13.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
        //    txtItem_ITDSC13.TextPadding = new FrameThickness(0, txtItem_ITDSC13.Height / 4, 0, 0);
        //    txtItem_ITDSC13.BorderThickness = new FrameThickness(boder);

        //    //참고사항
        //    TextItem txtItem_ITDSC14 = new TextItem(24.0, 55.0, 1.0, 1.0, (_vo.A09 == null ? "" : _vo.A09.ToString()));
        //    txtItem_ITDSC14.Font.Name = this.sFont;
        //    txtItem_ITDSC14.Font.Size = this.fontSize - 3;
        //    txtItem_ITDSC14.Font.Bold = true;
        //    txtItem_ITDSC14.Height = 9;
        //    txtItem_ITDSC14.Width = 75;
        //    txtItem_ITDSC14.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
        //    txtItem_ITDSC14.TextPadding = new FrameThickness(0, txtItem_ITDSC14.Height / 4, 0, 0);
        //    txtItem_ITDSC14.BorderThickness = new FrameThickness(boder);

        //    //분류
        //    TextItem txtItem_ITDSC15 = new TextItem(24.0, 64.0, 1.0, 1.0, _vo.A10.ToString());
        //    txtItem_ITDSC15.Font.Name = this.sFont;
        //    txtItem_ITDSC15.Font.Size = this.fontSize - 3;
        //    txtItem_ITDSC15.Font.Bold = true;
        //    txtItem_ITDSC15.Height = 10;
        //    txtItem_ITDSC15.Width = 75;
        //    txtItem_ITDSC15.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
        //    txtItem_ITDSC15.TextPadding = new FrameThickness(0, txtItem_ITDSC15.Height / 4, 0, 0);
        //    txtItem_ITDSC15.BorderThickness = new FrameThickness(boder);

        //    //보관위치
        //    TextItem txtItem_ITDSC20 = new TextItem(72.0, 19.0, 1.0, 1.0, (_vo.A04 == null ? "" : _vo.A04.ToString()));
        //    txtItem_ITDSC20.Font.Name = this.sFont;
        //    txtItem_ITDSC20.Font.Size = this.fontSize - 8;
        //    txtItem_ITDSC20.Font.Bold = true;
        //    txtItem_ITDSC20.Height = 9;
        //    txtItem_ITDSC20.Width = 27;
        //    txtItem_ITDSC20.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
        //    txtItem_ITDSC20.TextPadding = new FrameThickness(0, txtItem_ITDSC20.Height / 4, 0, 0);
        //    txtItem_ITDSC20.BorderThickness = new FrameThickness(boder);

        //    //유효기간
        //    TextItem txtItem_ITDSC21 = new TextItem(72.0, 28.0, 1.0, 1.0, (_vo.A12 == null ? "" : _vo.A12.ToString()));
        //    txtItem_ITDSC21.Font.Name = this.sFont;
        //    txtItem_ITDSC21.Font.Size = this.fontSize - 3;
        //    txtItem_ITDSC21.Font.Bold = true;
        //    txtItem_ITDSC21.Height = 9;
        //    txtItem_ITDSC21.Width = 27;
        //    txtItem_ITDSC21.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
        //    txtItem_ITDSC21.TextPadding = new FrameThickness(0, txtItem_ITDSC21.Height / 4, 0, 0);
        //    txtItem_ITDSC21.BorderThickness = new FrameThickness(boder);

        //    //제조번호
        //    TextItem txtItem_ITDSC22 = new TextItem(72.0, 37.0, 1.0, 1.0, (_vo.A06 == null ? "" : _vo.A06.ToString()));
        //    txtItem_ITDSC22.Font.Name = this.sFont;
        //    txtItem_ITDSC22.Font.Size = this.fontSize - 3;
        //    txtItem_ITDSC22.Font.Bold = true;
        //    txtItem_ITDSC22.Height = 9;
        //    txtItem_ITDSC22.Width = 27;
        //    txtItem_ITDSC22.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
        //    txtItem_ITDSC22.TextPadding = new FrameThickness(0, txtItem_ITDSC22.Height / 4, 0, 0);
        //    txtItem_ITDSC22.BorderThickness = new FrameThickness(boder);


        //    //시험번호
        //    TextItem txtItem_ITDSC23 = new TextItem(72.0, 46.0, 1.0, 1.0, (_vo.A08 == null ? "" : _vo.A08.ToString()));
        //    txtItem_ITDSC23.Font.Name = this.sFont;
        //    txtItem_ITDSC23.Font.Size = this.fontSize - 3;
        //    txtItem_ITDSC23.Font.Bold = true;
        //    txtItem_ITDSC23.Height = 9;
        //    txtItem_ITDSC23.Width = 27;
        //    txtItem_ITDSC23.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
        //    txtItem_ITDSC23.TextPadding = new FrameThickness(0, txtItem_ITDSC23.Height / 4, 0, 0);
        //    txtItem_ITDSC23.BorderThickness = new FrameThickness(boder);


        //    //바코드
        //    BarcodeItem ITDSC24 = new BarcodeItem(82, 2.0, 2.0, 2.0, BarcodeSymbology.QRCode, _vo.A14.ToString());
        //    //ITDSC24.DataMatrixModuleSize = 0.82;
        //    ITDSC24.QRCodeModuleSize = 0.8;
        //    ITDSC24.QRCodeEncoding = QRCodeEncoding.Byte;

        //    tLabel.Items.Add(txtItem_ITDSC);  // 품명
        //    tLabel.Items.Add(txtItem_ITDSC1); //품번 
        //    tLabel.Items.Add(txtItem_ITDSC2); // 수량
        //    tLabel.Items.Add(txtItem_ITDSC3); //입고일자
        //    tLabel.Items.Add(txtItem_ITDSC4); //제조원
        //    tLabel.Items.Add(txtItem_ITDSC5); //시험일
        //    tLabel.Items.Add(txtItem_ITDSC6); //참고사항
        //    tLabel.Items.Add(txtItem_ITDSC7); //분류
        //    tLabel.Items.Add(txtItem_ITDSC16); //보관위치
        //    tLabel.Items.Add(txtItem_ITDSC17); //유효기간
        //    tLabel.Items.Add(txtItem_ITDSC18); //제조번호
        //    tLabel.Items.Add(txtItem_ITDSC19); //시험번호

        //    tLabel.Items.Add(txtItem_ITDSC8); //품명D
        //    tLabel.Items.Add(txtItem_ITDSC9); //품번D
        //    tLabel.Items.Add(txtItem_ITDSC10); //수량D
        //    tLabel.Items.Add(txtItem_ITDSC11); //입고일자D
        //    tLabel.Items.Add(txtItem_ITDSC12); //제조원D
        //    tLabel.Items.Add(txtItem_ITDSC13); //시험일D
        //    tLabel.Items.Add(txtItem_ITDSC14); //참고사항D
        //    tLabel.Items.Add(txtItem_ITDSC15); //분류D
        //    tLabel.Items.Add(txtItem_ITDSC20); //보관위치D
        //    tLabel.Items.Add(txtItem_ITDSC21); //유효기간D
        //    tLabel.Items.Add(txtItem_ITDSC22); //제조번호D
        //    tLabel.Items.Add(txtItem_ITDSC23); //시험번호D

        //    tLabel.Items.Add(ITDSC24); //바코드

        //    return tLabel;

        //} 
        #endregion
        //원자재
        private ThermalLabel GenerateBasicThermalLabel_M(LabelDao _vo)
        {
            ThermalLabel tLabel = new ThermalLabel(UnitType.Mm, 100, 80);
            tLabel.GapLength = 0.1;


            TextItem txtItem_ITDSC = new TextItem(4.0, 1.0, 1.0, 1.0, "품명");
            txtItem_ITDSC.Font.Name = this.sFont;
            txtItem_ITDSC.Font.Size = this.fontSize - 3;
            txtItem_ITDSC.Font.Bold = true;
            txtItem_ITDSC.Height = 9;
            txtItem_ITDSC.Width = 20;
            txtItem_ITDSC.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC.TextPadding = new FrameThickness(0, txtItem_ITDSC.Height / 4, 0, 0);
            txtItem_ITDSC.BorderThickness = new FrameThickness(boder);

            TextItem txtItem_ITDSC1 = new TextItem(4.0, 10.0, 1.0, 1.0, "품번");
            txtItem_ITDSC1.Font.Name = this.sFont;
            txtItem_ITDSC1.Font.Size = this.fontSize - 3;
            txtItem_ITDSC1.Font.Bold = true;
            txtItem_ITDSC1.Height = 9;
            txtItem_ITDSC1.Width = 20;
            txtItem_ITDSC1.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC1.TextPadding = new FrameThickness(0, txtItem_ITDSC.Height / 4, 0, 0);
            txtItem_ITDSC1.BorderThickness = new FrameThickness(boder);

            TextItem txtItem_ITDSC2 = new TextItem(4.0, 19.0, 1.0, 1.0, "일자");
            txtItem_ITDSC2.Font.Name = this.sFont;
            txtItem_ITDSC2.Font.Size = this.fontSize - 3;
            txtItem_ITDSC2.Font.Bold = true;
            txtItem_ITDSC2.Height = 9;
            txtItem_ITDSC2.Width = 20;
            txtItem_ITDSC2.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC2.TextPadding = new FrameThickness(0, txtItem_ITDSC.Height / 4, 0, 0);
            txtItem_ITDSC2.BorderThickness = new FrameThickness(boder);

            TextItem txtItem_ITDSC3 = new TextItem(4.0, 28.0, 1.0, 1.0, "제조번호");
            txtItem_ITDSC3.Font.Name = this.sFont;
            txtItem_ITDSC3.Font.Size = this.fontSize - 3;
            txtItem_ITDSC3.Font.Bold = true;
            txtItem_ITDSC3.Height = 9;
            txtItem_ITDSC3.Width = 20;
            txtItem_ITDSC3.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC3.TextPadding = new FrameThickness(0, txtItem_ITDSC.Height / 4, 0, 0);
            txtItem_ITDSC3.BorderThickness = new FrameThickness(boder);

            TextItem txtItem_ITDSC4 = new TextItem(4.0, 37.0, 1.0, 1.0, "제조원");
            txtItem_ITDSC4.Font.Name = this.sFont;
            txtItem_ITDSC4.Font.Size = this.fontSize - 3;
            txtItem_ITDSC4.Font.Bold = true;
            txtItem_ITDSC4.Height = 9;
            txtItem_ITDSC4.Width = 20;
            txtItem_ITDSC4.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC4.TextPadding = new FrameThickness(0, txtItem_ITDSC.Height / 4, 0, 0);
            txtItem_ITDSC4.BorderThickness = new FrameThickness(boder);


            TextItem txtItem_ITDSC5 = new TextItem(4.0, 46.0, 1.0, 1.0, "보관위치");
            txtItem_ITDSC5.Font.Name = this.sFont;
            txtItem_ITDSC5.Font.Size = this.fontSize - 3;
            txtItem_ITDSC5.Font.Bold = true;
            txtItem_ITDSC5.Height = 9;
            txtItem_ITDSC5.Width = 20;
            txtItem_ITDSC5.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC5.TextPadding = new FrameThickness(0, txtItem_ITDSC.Height / 4, 0, 0);
            txtItem_ITDSC5.BorderThickness = new FrameThickness(boder);


            TextItem txtItem_ITDSC6 = new TextItem(4.0, 55.0, 1.0, 1.0, "참고사항");
            txtItem_ITDSC6.Font.Name = this.sFont;
            txtItem_ITDSC6.Font.Size = this.fontSize - 3;
            txtItem_ITDSC6.Font.Bold = true;
            txtItem_ITDSC6.Height = 9;
            txtItem_ITDSC6.Width = 20;
            txtItem_ITDSC6.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC6.TextPadding = new FrameThickness(0, txtItem_ITDSC.Height / 4, 0, 0);
            txtItem_ITDSC6.BorderThickness = new FrameThickness(boder);

            TextItem txtItem_ITDSC7 = new TextItem(4.0, 64.0, 1.0, 1.0, "판정결과");
            txtItem_ITDSC7.Font.Name = this.sFont;
            txtItem_ITDSC7.Font.Size = this.fontSize - 3;
            txtItem_ITDSC7.Font.Bold = true;
            txtItem_ITDSC7.Height = 10;
            txtItem_ITDSC7.Width = 20;
            txtItem_ITDSC7.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC7.TextPadding = new FrameThickness(0, txtItem_ITDSC.Height / 4, 0, 0);
            txtItem_ITDSC7.BorderThickness = new FrameThickness(boder);



            TextItem txtItem_ITDSC16 = new TextItem(52.0, 19.0, 1.0, 1.0, "수량");
            txtItem_ITDSC16.Font.Name = this.sFont;
            txtItem_ITDSC16.Font.Size = this.fontSize - 3;
            txtItem_ITDSC16.Font.Bold = true;
            txtItem_ITDSC16.Height = 9;
            txtItem_ITDSC16.Width = 20;
            txtItem_ITDSC16.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC16.TextPadding = new FrameThickness(0, txtItem_ITDSC.Height / 4, 0, 0);
            txtItem_ITDSC16.BorderThickness = new FrameThickness(boder);

            TextItem txtItem_ITDSC17 = new TextItem(52.0, 28.0, 1.0, 1.0, "시험번호");
            txtItem_ITDSC17.Font.Name = this.sFont;
            txtItem_ITDSC17.Font.Size = this.fontSize - 3;
            txtItem_ITDSC17.Font.Bold = true;
            txtItem_ITDSC17.Height = 9;
            txtItem_ITDSC17.Width = 20;
            txtItem_ITDSC17.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC17.TextPadding = new FrameThickness(0, txtItem_ITDSC.Height / 4, 0, 0);
            txtItem_ITDSC17.BorderThickness = new FrameThickness(boder);


            TextItem txtItem_ITDSC18 = new TextItem(52.0, 37.0, 1.0, 1.0, "사용기한");
            txtItem_ITDSC18.Font.Name = this.sFont;
            txtItem_ITDSC18.Font.Size = this.fontSize - 3;
            txtItem_ITDSC18.Font.Bold = true;
            txtItem_ITDSC18.Height = 9;
            txtItem_ITDSC18.Width = 20;
            txtItem_ITDSC18.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC18.TextPadding = new FrameThickness(0, txtItem_ITDSC.Height / 4, 0, 0);
            txtItem_ITDSC18.BorderThickness = new FrameThickness(boder);


            TextItem txtItem_ITDSC19 = new TextItem(52.0, 46.0, 1.0, 1.0, "보관조건");
            txtItem_ITDSC19.Font.Name = this.sFont;
            txtItem_ITDSC19.Font.Size = this.fontSize - 3;
            txtItem_ITDSC19.Font.Bold = true;
            txtItem_ITDSC19.Height = 9;
            txtItem_ITDSC19.Width = 20;
            txtItem_ITDSC19.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC19.TextPadding = new FrameThickness(0, txtItem_ITDSC.Height / 4, 0, 0);
            txtItem_ITDSC19.BorderThickness = new FrameThickness(boder);




            TextItem txtItem_KOCD1 = new TextItem(52.0, 55.0, 1.0, 1.0, "판정일");
            txtItem_KOCD1.Font.Name = this.sFont;
            txtItem_KOCD1.Font.Size = this.fontSize - 3;
            txtItem_KOCD1.Font.Bold = true;
            txtItem_KOCD1.Height = 9;
            txtItem_KOCD1.Width = 20;
            txtItem_KOCD1.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_KOCD1.TextPadding = new FrameThickness(0, txtItem_ITDSC.Height / 4, 0, 0);
            txtItem_KOCD1.BorderThickness = new FrameThickness(boder);



            //품명
            TextItem txtItem_ITDSC8 = new TextItem(24.0, 1.0, 1.0, 1.0, _vo.A01.ToString());
            txtItem_ITDSC8.Font.Name = this.sFont;
            txtItem_ITDSC8.Font.Size = this.fontSize - 6;
            txtItem_ITDSC8.Font.Bold = true;
            txtItem_ITDSC8.Height = 9;
            txtItem_ITDSC8.Width = 57;
            txtItem_ITDSC8.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC8.TextPadding = new FrameThickness(0, 0.2, 0, 0);
            txtItem_ITDSC8.BorderThickness = new FrameThickness(boder);

            //품번 
            TextItem txtItem_ITDSC9 = new TextItem(24.0, 10.0, 1.0, 1.0, _vo.A02.ToString());
            txtItem_ITDSC9.Font.Name = this.sFont;
            txtItem_ITDSC9.Font.Size = this.fontSize - 3;
            txtItem_ITDSC9.Font.Bold = true;
            txtItem_ITDSC9.Height = 9;
            txtItem_ITDSC9.Width = 57;
            txtItem_ITDSC9.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC9.TextPadding = new FrameThickness(0, txtItem_ITDSC9.Height / 4, 0, 0);
            txtItem_ITDSC9.BorderThickness = new FrameThickness(boder);

            //수량 --> 일자
            // TextItem txtItem_ITDSC10 = new TextItem(24.0, 19.0, 1.0, 1.0, String.Format("{0:##,###,###,###,##0.00}", Convert.ToDouble(_vo.A03)));
            TextItem txtItem_ITDSC10 = new TextItem(24.0, 19.0, 1.0, 1.0, (_vo.A03 == null ? "" : _vo.A03.ToString()));
            txtItem_ITDSC10.Font.Name = this.sFont;
            txtItem_ITDSC10.Font.Size = this.fontSize - 3;
            txtItem_ITDSC10.Font.Bold = true;
            txtItem_ITDSC10.Height = 9;
            txtItem_ITDSC10.Width = 28;
            txtItem_ITDSC10.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC10.TextPadding = new FrameThickness(0, txtItem_ITDSC10.Height / 4, 0, 0);
            txtItem_ITDSC10.BorderThickness = new FrameThickness(boder);

            //입고일자  -->제조번호
            //TextItem txtItem_ITDSC11 = new TextItem(24.0, 28.0, 1.0, 1.0, (_vo.A11 == null ? "" : _vo.A11.ToString())); 
            TextItem txtItem_ITDSC11 = new TextItem(24.0, 28.0, 1.0, 1.0, (_vo.A05 == null ? "" : _vo.A05.ToString()));
            txtItem_ITDSC11.Font.Name = this.sFont;
            txtItem_ITDSC11.Font.Size = this.fontSize - 6;
            txtItem_ITDSC11.Font.Bold = true;
            txtItem_ITDSC11.Height = 9;
            txtItem_ITDSC11.Width = 28;
            txtItem_ITDSC11.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC11.TextPadding = new FrameThickness(0, txtItem_ITDSC11.Height / 6, 0, 0);
            txtItem_ITDSC11.BorderThickness = new FrameThickness(boder);

            //제조원
            TextItem txtItem_ITDSC12 = new TextItem(24.0, 37.0, 1.0, 1.0, (_vo.A07 == null ? "" : _vo.A07.ToString()));
            txtItem_ITDSC12.Font.Name = this.sFont;
            txtItem_ITDSC12.Font.Size = this.fontSize - 6;
            txtItem_ITDSC12.Font.Bold = true;
            txtItem_ITDSC12.Height = 9;
            txtItem_ITDSC12.Width = 28;
            txtItem_ITDSC12.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC12.TextPadding = new FrameThickness(0, 0.2, 0, 0);
            txtItem_ITDSC12.BorderThickness = new FrameThickness(boder);

            //시험일 -->보관위치
            // TextItem txtItem_ITDSC13 = new TextItem(24.0, 46.0, 1.0, 1.0, (_vo.A07 == null ? "" : _vo.A07.ToString()));
            TextItem txtItem_ITDSC13 = new TextItem(24.0, 46.0, 1.0, 1.0, (_vo.A09 == null ? "" : _vo.A09.ToString()));
            txtItem_ITDSC13.Font.Name = this.sFont;
            txtItem_ITDSC13.Font.Size = this.fontSize - 3;
            txtItem_ITDSC13.Font.Bold = true;
            txtItem_ITDSC13.Height = 9;
            txtItem_ITDSC13.Width = 28;
            txtItem_ITDSC13.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC13.TextPadding = new FrameThickness(0, txtItem_ITDSC13.Height / 4, 0, 0);
            txtItem_ITDSC13.BorderThickness = new FrameThickness(boder);

            //참고사항
            TextItem txtItem_ITDSC14 = new TextItem(24.0, 55.0, 1.0, 1.0, (_vo.A11 == null ? "" : _vo.A11.ToString()));
            txtItem_ITDSC14.Font.Name = this.sFont;
            txtItem_ITDSC14.Font.Size = this.fontSize - 3;
            txtItem_ITDSC14.Font.Bold = true;
            txtItem_ITDSC14.Height = 9;
            txtItem_ITDSC14.Width = 28;
            txtItem_ITDSC14.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC14.TextPadding = new FrameThickness(0, txtItem_ITDSC14.Height / 4, 0, 0);
            txtItem_ITDSC14.BorderThickness = new FrameThickness(boder);



            //분류 ---> 판정결과
            TextItem txtItem_ITDSC15 = new TextItem(24.0, 64.0, 1.0, 1.0, (_vo.A15 == null ? "" : _vo.A15.ToString()));
            txtItem_ITDSC15.Font.Name = this.sFont;
            txtItem_ITDSC15.Font.Size = this.fontSize - 3;
            txtItem_ITDSC15.Font.Bold = true;
            txtItem_ITDSC15.Height = 10;
            txtItem_ITDSC15.Width = 75;
            txtItem_ITDSC15.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC15.TextPadding = new FrameThickness(0, txtItem_ITDSC15.Height / 4, 0, 0);
            txtItem_ITDSC15.BorderThickness = new FrameThickness(boder);




            //보관위치 --> 수량 (데이터 못넣었음)
            TextItem txtItem_ITDSC20 = new TextItem(72.0, 19.0, 1.0, 1.0,  String.Format("{0:##,###,###,###,##0.00}", Convert.ToDouble(_vo.A04)) );
            txtItem_ITDSC20.Font.Name = this.sFont;
            txtItem_ITDSC20.Font.Size = this.fontSize - 3;
            txtItem_ITDSC20.Font.Bold = true;
            txtItem_ITDSC20.Height = 9;
            txtItem_ITDSC20.Width = 27;
            txtItem_ITDSC20.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC20.TextPadding = new FrameThickness(0, txtItem_ITDSC20.Height / 4, 0, 0);
            txtItem_ITDSC20.BorderThickness = new FrameThickness(boder);



            //유효기간 -->시험번호
            // TextItem txtItem_ITDSC21 = new TextItem(72.0, 28.0, 1.0, 1.0, (_vo.A12 == null ? "" : _vo.A12.ToString()));
            TextItem txtItem_ITDSC21 = new TextItem(72.0, 28.0, 1.0, 1.0, (_vo.A06 == null ? "" : _vo.A06.ToString()));
            txtItem_ITDSC21.Font.Name = this.sFont;
            txtItem_ITDSC21.Font.Size = this.fontSize - 3;
            txtItem_ITDSC21.Font.Bold = true;
            txtItem_ITDSC21.Height = 9;
            txtItem_ITDSC21.Width = 27;
            txtItem_ITDSC21.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC21.TextPadding = new FrameThickness(0, txtItem_ITDSC21.Height / 4, 0, 0);
            txtItem_ITDSC21.BorderThickness = new FrameThickness(boder);



            //제조번호 -->사용기한 데이터 못넣었음
            TextItem txtItem_ITDSC22 = new TextItem(72.0, 37.0, 1.0, 1.0, (_vo.A08 == null ? "" : _vo.A08.ToString()));
            txtItem_ITDSC22.Font.Name = this.sFont;
            txtItem_ITDSC22.Font.Size = this.fontSize - 3;
            txtItem_ITDSC22.Font.Bold = true;
            txtItem_ITDSC22.Height = 9;
            txtItem_ITDSC22.Width = 27;
            txtItem_ITDSC22.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC22.TextPadding = new FrameThickness(0, txtItem_ITDSC22.Height / 4, 0, 0);
            txtItem_ITDSC22.BorderThickness = new FrameThickness(boder);



            //시험번호 -->보관조건  데이터 못넣었음
            TextItem txtItem_ITDSC23 = new TextItem(72.0, 46.0, 1.0, 1.0, (_vo.A10 == null ? "" : _vo.A10.ToString()));
            txtItem_ITDSC23.Font.Name = this.sFont;
            txtItem_ITDSC23.Font.Size = this.fontSize - 3;
            txtItem_ITDSC23.Font.Bold = true;
            txtItem_ITDSC23.Height = 9;
            txtItem_ITDSC23.Width = 27;
            txtItem_ITDSC23.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC23.TextPadding = new FrameThickness(0, txtItem_ITDSC23.Height / 4, 0, 0);
            txtItem_ITDSC23.BorderThickness = new FrameThickness(boder);


            //판정일  데이터 못넣었음
            TextItem txtItem_KODCE1 = new TextItem(72.0, 55.0, 1.0, 1.0, (_vo.A12 == null ? "" : _vo.A12.ToString()));
            txtItem_KODCE1.Font.Name = this.sFont;
            txtItem_KODCE1.Font.Size = this.fontSize - 3;
            txtItem_KODCE1.Font.Bold = true;
            txtItem_KODCE1.Height = 9;
            txtItem_KODCE1.Width = 27;
            txtItem_KODCE1.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_KODCE1.TextPadding = new FrameThickness(0, txtItem_ITDSC23.Height / 4, 0, 0);
            txtItem_KODCE1.BorderThickness = new FrameThickness(boder);





            //바코드
            BarcodeItem ITDSC24 = new BarcodeItem(82, 2.0, 2.0, 2.0, BarcodeSymbology.QRCode, _vo.A14.ToString());
            //ITDSC24.DataMatrixModuleSize = 0.82;
            ITDSC24.QRCodeModuleSize = 0.6;
            ITDSC24.QRCodeEncoding = QRCodeEncoding.Byte;

            tLabel.Items.Add(txtItem_ITDSC);  // 품명
            tLabel.Items.Add(txtItem_ITDSC1); //품번 
            tLabel.Items.Add(txtItem_ITDSC2); // 수량
            tLabel.Items.Add(txtItem_ITDSC3); //입고일자
            tLabel.Items.Add(txtItem_ITDSC4); //제조원
            tLabel.Items.Add(txtItem_ITDSC5); //시험일
            tLabel.Items.Add(txtItem_ITDSC6); //참고사항
            tLabel.Items.Add(txtItem_ITDSC7); //분류
            tLabel.Items.Add(txtItem_ITDSC16); //보관위치
            tLabel.Items.Add(txtItem_ITDSC17); //유효기간
            tLabel.Items.Add(txtItem_ITDSC18); //제조번호
            tLabel.Items.Add(txtItem_ITDSC19); //시험번호
            tLabel.Items.Add(txtItem_KOCD1); //판정일

            tLabel.Items.Add(txtItem_ITDSC8); //품명D
            tLabel.Items.Add(txtItem_ITDSC9); //품번D
            tLabel.Items.Add(txtItem_ITDSC10); //수량D
            tLabel.Items.Add(txtItem_ITDSC11); //입고일자D
            tLabel.Items.Add(txtItem_ITDSC12); //제조원D
            tLabel.Items.Add(txtItem_ITDSC13); //시험일D
            tLabel.Items.Add(txtItem_ITDSC14); //참고사항D
            tLabel.Items.Add(txtItem_ITDSC15); //분류D
            tLabel.Items.Add(txtItem_ITDSC20); //보관위치D
            tLabel.Items.Add(txtItem_ITDSC21); //유효기간D
            tLabel.Items.Add(txtItem_ITDSC22); //제조번호D
            tLabel.Items.Add(txtItem_ITDSC23); //시험번호D
            tLabel.Items.Add(txtItem_KODCE1); //판정일D


            tLabel.Items.Add(ITDSC24); //바코드

            return tLabel;

        }






        //부자재
        public bool B_Godex(LabelDao _vo, int _cnt = 1)
        {
            try
            {


                _printerSettings = new PrinterSettings();
                _printerSettings.Communication.CommunicationType = CommunicationType.USB;
                _printerSettings.PrinterName = Properties.Settings.Default.str_PrnNm;

                using (pj = new PrintJob(_printerSettings))
                {
                    pj.BufferOutput = true;
                    pj.Copies = _cnt;
                    pj.Replicates = 1;
                    pj.PrintOrientation = PrintOrientation.Portrait;
                    pj.ThermalLabel = GenerateBasicThermalLabel_B(_vo);
                    pj.Print();

                    return true;
                }

            }
            catch (Exception eLog)
            {
                //WinUIMessageBox.Show(eLog.Message, "[에러]" + SystemProperties.PROGRAM_TITLE, MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
        }

        //부자재
        private ThermalLabel GenerateBasicThermalLabel_B(LabelDao _vo)
        {
            //Define a ThermalLabel object and set unit to inch and label size
            ThermalLabel tLabel = new ThermalLabel(UnitType.Mm, 100, 80);
            tLabel.GapLength = 0.1;

            TextItem txtItem_ITDSC = new TextItem(4.0, 1.0, 1.0, 1.0, "품명");
            txtItem_ITDSC.Font.Name = this.sFont;
            txtItem_ITDSC.Font.Size = this.fontSize - 3;
            txtItem_ITDSC.Font.Bold = true;
            txtItem_ITDSC.Height = 14;
            txtItem_ITDSC.Width = 20;
            txtItem_ITDSC.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC.TextPadding = new FrameThickness(0, txtItem_ITDSC.Height / 4, 0, 0);
            txtItem_ITDSC.BorderThickness = new FrameThickness(boder);
            //txtItem_MTRL_LOT_NO.BackColor = Color.Black;
            //txtItem_MTRL_LOT_NO.ForeColor = Color.White;
            //txtItem_MTRL_LOT_NO.BorderColor = Color.White;

            TextItem txtItem_ITDSC1 = new TextItem(4.0, 15.0, 2.0, 2.0, "품번");
            txtItem_ITDSC1.Font.Name = this.sFont;
            txtItem_ITDSC1.Font.Size = this.fontSize - 3;
            txtItem_ITDSC1.Font.Bold = true;
            txtItem_ITDSC1.Height = 15;
            txtItem_ITDSC1.Width = 20;
            txtItem_ITDSC1.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC1.TextPadding = new FrameThickness(0, txtItem_ITDSC1.Height / 4, 0, 0);
            txtItem_ITDSC1.BorderThickness = new FrameThickness(boder);

            TextItem txtItem_ITDSC2 = new TextItem(4.0, 30.0, 2.0, 2.0, "수량");
            txtItem_ITDSC2.Font.Name = this.sFont;
            txtItem_ITDSC2.Font.Size = this.fontSize - 4;
            txtItem_ITDSC2.Font.Bold = true;
            txtItem_ITDSC2.Height = 7;
            txtItem_ITDSC2.Width = 20;
            txtItem_ITDSC2.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC2.TextPadding = new FrameThickness(0, 0.2, 0, 0);
            txtItem_ITDSC2.BorderThickness = new FrameThickness(boder);

            TextItem txtItem_ITDSC2_1 = new TextItem(4.0, 37.0, 2.0, 2.0, "BOX 수량");
            txtItem_ITDSC2_1.Font.Name = this.sFont;
            txtItem_ITDSC2_1.Font.Size = this.fontSize - 4;
            txtItem_ITDSC2_1.Font.Bold = true;
            txtItem_ITDSC2_1.Height = 8;
            txtItem_ITDSC2_1.Width = 20;
            txtItem_ITDSC2_1.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC2_1.TextPadding = new FrameThickness(0, 0.2, 0, 0);
            txtItem_ITDSC2_1.BorderThickness = new FrameThickness(boder);


            TextItem txtItem_ITDSC3 = new TextItem(4.0, 45.0, 2.0, 2.0, "입고일자");
            txtItem_ITDSC3.Font.Name = this.sFont;
            txtItem_ITDSC3.Font.Size = this.fontSize - 3;
            txtItem_ITDSC3.Font.Bold = true;
            txtItem_ITDSC3.Height = 15;
            txtItem_ITDSC3.Width = 20;
            txtItem_ITDSC3.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC3.TextPadding = new FrameThickness(0, txtItem_ITDSC3.Height / 4, 0, 0);
            txtItem_ITDSC3.BorderThickness = new FrameThickness(boder);

            TextItem txtItem_ITDSC4 = new TextItem(4.0, 60.0, 2.0, 2.0, "배치코드");
            txtItem_ITDSC4.Font.Name = this.sFont;
            txtItem_ITDSC4.Font.Size = this.fontSize - 3;
            txtItem_ITDSC4.Font.Bold = true;
            txtItem_ITDSC4.Height = 16;
            txtItem_ITDSC4.Width = 20;
            txtItem_ITDSC4.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC4.TextPadding = new FrameThickness(0, txtItem_ITDSC4.Height / 4, 0, 0);
            txtItem_ITDSC4.BorderThickness = new FrameThickness(boder);

            TextItem txtItem_ITDSC10 = new TextItem(52.0, 30.0, 2.0, 2.0, "규격");
            txtItem_ITDSC10.Font.Name = this.sFont;
            txtItem_ITDSC10.Font.Size = this.fontSize - 3;
            txtItem_ITDSC10.Font.Bold = true;
            txtItem_ITDSC10.Height = 15;
            txtItem_ITDSC10.Width = 20;
            txtItem_ITDSC10.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC10.TextPadding = new FrameThickness(0, txtItem_ITDSC10.Height / 4, 0, 0);
            txtItem_ITDSC10.BorderThickness = new FrameThickness(boder);

            TextItem txtItem_ITDSC11 = new TextItem(52.0, 45.0, 2.0, 2.0, "유효기간");
            txtItem_ITDSC11.Font.Name = this.sFont;
            txtItem_ITDSC11.Font.Size = this.fontSize - 2;
            txtItem_ITDSC11.Font.Bold = true;
            txtItem_ITDSC11.Height = 15;
            txtItem_ITDSC11.Width = 20;
            txtItem_ITDSC11.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC11.TextPadding = new FrameThickness(0, txtItem_ITDSC11.Height / 4, 0, 0);
            txtItem_ITDSC11.BorderThickness = new FrameThickness(boder);

            TextItem txtItem_ITDSC12 = new TextItem(52.0, 60.0, 2.0, 2.0, "공급업체");
            txtItem_ITDSC12.Font.Name = this.sFont;
            txtItem_ITDSC12.Font.Size = this.fontSize - 2;
            txtItem_ITDSC12.Font.Bold = true;
            txtItem_ITDSC12.Height = 16;
            txtItem_ITDSC12.Width = 20;
            txtItem_ITDSC12.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC12.TextPadding = new FrameThickness(0, txtItem_ITDSC12.Height / 4, 0, 0);
            txtItem_ITDSC12.BorderThickness = new FrameThickness(boder);


            //품명 데이터
            TextItem txtItem_ITDSC5 = new TextItem(24.0, 1.0, 1.0, 1.0, _vo.A01.ToString());
            txtItem_ITDSC5.Font.Name = this.sFont;
            txtItem_ITDSC5.Font.Size = this.fontSize - 5;
            txtItem_ITDSC5.Font.Bold = true;
            txtItem_ITDSC5.Height = 14;
            txtItem_ITDSC5.Width = 75;
            txtItem_ITDSC5.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC5.TextPadding = new FrameThickness(0, txtItem_ITDSC5.Height / 4, 0, 0);
            txtItem_ITDSC5.BorderThickness = new FrameThickness(boder);

            //품번 데이터
            TextItem txtItem_ITDSC6 = new TextItem(24.0, 15.0, 1.0, 1.0, _vo.A02.ToString());
            txtItem_ITDSC6.Font.Name = this.sFont;
            txtItem_ITDSC6.Font.Size = this.fontSize - 3;
            txtItem_ITDSC6.Font.Bold = true;
            txtItem_ITDSC6.Height = 15;
            txtItem_ITDSC6.Width = 75;
            txtItem_ITDSC6.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC6.TextPadding = new FrameThickness(0, txtItem_ITDSC6.Height / 4, 0, 0);
            txtItem_ITDSC6.BorderThickness = new FrameThickness(boder);

            //수량 데이터
            TextItem txtItem_ITDSC7 = new TextItem(24.0, 30.0, 2.0, 2.0, String.Format("{0:##,###,###,###,##0.00}", Convert.ToDouble(_vo.A03)));
            txtItem_ITDSC7.Font.Name = this.sFont;
            txtItem_ITDSC7.Font.Size = this.fontSize - 3;
            txtItem_ITDSC7.Font.Bold = true;
            txtItem_ITDSC7.Height = 7;
            txtItem_ITDSC7.Width = 28;
            txtItem_ITDSC7.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC7.TextPadding = new FrameThickness(0, 0.1, 0, 0);
            txtItem_ITDSC7.BorderThickness = new FrameThickness(boder);

            //수량 데이터
            TextItem txtItem_ITDSC7_1 = new TextItem(24.0, 37.0, 2.0, 2.0, (_vo.A16.ToString().Equals("0") ? " " : String.Format("{0:##,###,###,###,##0}", Convert.ToDouble(_vo.A16))));
            txtItem_ITDSC7_1.Font.Name = this.sFont;
            txtItem_ITDSC7_1.Font.Size = this.fontSize - 3;
            txtItem_ITDSC7_1.Font.Bold = true;
            txtItem_ITDSC7_1.Height = 8;
            txtItem_ITDSC7_1.Width = 28;
            txtItem_ITDSC7_1.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC7_1.TextPadding = new FrameThickness(0, 0.1, 0, 0);
            txtItem_ITDSC7_1.BorderThickness = new FrameThickness(boder);

            //입고일자 데이터
            TextItem txtItem_ITDSC8 = new TextItem(24.0, 45.0, 2.0, 2.0, _vo.A11.ToString());
            txtItem_ITDSC8.Font.Name = this.sFont;
            txtItem_ITDSC8.Font.Size = this.fontSize - 3;
            txtItem_ITDSC8.Font.Bold = true;
            txtItem_ITDSC8.Height = 15;
            txtItem_ITDSC8.Width = 28;
            txtItem_ITDSC8.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC8.TextPadding = new FrameThickness(0, txtItem_ITDSC8.Height / 4, 0, 0);
            txtItem_ITDSC8.BorderThickness = new FrameThickness(boder);

            //배치코드 데이터
            TextItem txtItem_ITDSC9 = new TextItem(24.0, 60.0, 2.0, 2.0, _vo.A13.ToString());
            txtItem_ITDSC9.Font.Name = this.sFont;
            txtItem_ITDSC9.Font.Size = this.fontSize - 3;
            txtItem_ITDSC9.Font.Bold = true;
            txtItem_ITDSC9.Height = 16;
            txtItem_ITDSC9.Width = 28;
            txtItem_ITDSC9.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC9.TextPadding = new FrameThickness(0, txtItem_ITDSC9.Height / 4, 0, 0);
            txtItem_ITDSC9.BorderThickness = new FrameThickness(boder);

            //규격 데이터
            TextItem txtItem_ITDSC13 = new TextItem(72.0, 30.0, 2.0, 2.0, _vo.A15.ToString());
            txtItem_ITDSC13.Font.Name = this.sFont;
            txtItem_ITDSC13.Font.Size = this.fontSize - 6;
            txtItem_ITDSC13.Font.Bold = true;
            txtItem_ITDSC13.Height = 15;
            txtItem_ITDSC13.Width = 27;
            txtItem_ITDSC13.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC13.TextPadding = new FrameThickness(0, 0.1, 0, 0);
            txtItem_ITDSC13.BorderThickness = new FrameThickness(boder);

            //유효기간 데이터
            TextItem txtItem_ITDSC14 = new TextItem(72.0, 45.0, 2.0, 2.0, _vo.A12.ToString());
            txtItem_ITDSC14.Font.Name = this.sFont;
            txtItem_ITDSC14.Font.Size = this.fontSize - 2;
            txtItem_ITDSC14.Font.Bold = true;
            txtItem_ITDSC14.Height = 15;
            txtItem_ITDSC14.Width = 27;
            txtItem_ITDSC14.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC14.TextPadding = new FrameThickness(0, txtItem_ITDSC14.Height / 4, 0, 0);
            txtItem_ITDSC14.BorderThickness = new FrameThickness(boder);

            //공급업체 데이터
            TextItem txtItem_ITDSC15 = new TextItem(72.0, 60.0, 2.0, 2.0, _vo.A05.ToString());
            txtItem_ITDSC15.Font.Name = this.sFont;
            txtItem_ITDSC15.Font.Size = this.fontSize - 6;
            txtItem_ITDSC15.Font.Bold = true;
            txtItem_ITDSC15.Height = 16;
            txtItem_ITDSC15.Width = 27;
            txtItem_ITDSC15.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC15.TextPadding = new FrameThickness(0, txtItem_ITDSC15.Height / 4, 0, 0);
            txtItem_ITDSC15.BorderThickness = new FrameThickness(boder);

            //바코드
            BarcodeItem ITDSC16 = new BarcodeItem(82, 15.5, 2.0, 2.0, BarcodeSymbology.QRCode, _vo.A14.ToString());
            //ITDSC16.DataMatrixModuleSize = 0.70;
            ITDSC16.QRCodeModuleSize = 0.55;
            ITDSC16.QRCodeEncoding = QRCodeEncoding.Byte;



            tLabel.Items.Add(txtItem_ITDSC);  //품명
            tLabel.Items.Add(txtItem_ITDSC1); //품번
            tLabel.Items.Add(txtItem_ITDSC2); //수량
            tLabel.Items.Add(txtItem_ITDSC3); //입고일자
            tLabel.Items.Add(txtItem_ITDSC4); //배치코드
            tLabel.Items.Add(txtItem_ITDSC10); //규격
            tLabel.Items.Add(txtItem_ITDSC11); //유효기간
            tLabel.Items.Add(txtItem_ITDSC12); //공급업체
            tLabel.Items.Add(txtItem_ITDSC2_1); //유효기간

            tLabel.Items.Add(txtItem_ITDSC5); //품명 컬럼
            tLabel.Items.Add(txtItem_ITDSC6); //품번 컬럼
            tLabel.Items.Add(txtItem_ITDSC7); //수량 컬럼
            tLabel.Items.Add(txtItem_ITDSC8); //입고일자 컬럼
            tLabel.Items.Add(txtItem_ITDSC9); //배치코드 컬럼
            tLabel.Items.Add(txtItem_ITDSC13); //규격 컬럼
            tLabel.Items.Add(txtItem_ITDSC14); //유효기간 컬럼
            tLabel.Items.Add(txtItem_ITDSC15); //공급업체 컬럼
            tLabel.Items.Add(txtItem_ITDSC7_1); //공급업체 컬럼

            tLabel.Items.Add(ITDSC16);  //바코드
            //tLabel.DataSource = Vo;

            return tLabel;
        }


        //벌크
        public bool Bulk_Godex(LabelDao _vo, int _cnt = 1)
        {
            try
            {


                _printerSettings = new PrinterSettings();
                _printerSettings.Communication.CommunicationType = CommunicationType.USB;
                _printerSettings.PrinterName = Properties.Settings.Default.str_PrnNm;

                using (pj = new PrintJob(_printerSettings))
                {
                    pj.BufferOutput = true;
                    pj.Copies = _cnt;
                    pj.Replicates = 1;
                    pj.PrintOrientation = PrintOrientation.Portrait;
                    pj.ThermalLabel = GenerateBasicThermalLabel_Bulk(_vo);
                    pj.Print();

                    return true;
                }

            }
            catch (Exception eLog)
            {
                //WinUIMessageBox.Show(eLog.Message, "[에러]" + SystemProperties.PROGRAM_TITLE, MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
        }


        //벌크
        private ThermalLabel GenerateBasicThermalLabel_Bulk(LabelDao _vo)
        {
            ThermalLabel tLabel = new ThermalLabel(UnitType.Mm, 100, 80);
            tLabel.GapLength = 0.1;


            TextItem txtItem_ITDSC = new TextItem(4.0, 0, 1.0, 1.0, "벌크코드");
            txtItem_ITDSC.Font.Name = this.sFont;
            txtItem_ITDSC.Font.Size = this.fontSize - 2;
            txtItem_ITDSC.Font.Bold = true;
            txtItem_ITDSC.Height = 7;
            txtItem_ITDSC.Width = 20;
            txtItem_ITDSC.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC.TextPadding = new FrameThickness(0, txtItem_ITDSC.Height / 8, 0, 0);
            txtItem_ITDSC.BorderThickness = new FrameThickness(boder);

            TextItem txtItem_ITDSC1 = new TextItem(4.0, 7.0, 1.0, 1.0, "제품명");
            txtItem_ITDSC1.Font.Name = this.sFont;
            txtItem_ITDSC1.Font.Size = this.fontSize - 3;
            txtItem_ITDSC1.Font.Bold = true;
            txtItem_ITDSC1.Height = 9;
            txtItem_ITDSC1.Width = 20;
            txtItem_ITDSC1.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC1.TextPadding = new FrameThickness(0, txtItem_ITDSC.Height / 4, 0, 0);
            txtItem_ITDSC1.BorderThickness = new FrameThickness(boder);


            TextItem txtItem_ITDSCA = new TextItem(4.0, 16.0, 1.0, 1.0, "홋수");
            txtItem_ITDSCA.Font.Name = this.sFont;
            txtItem_ITDSCA.Font.Size = this.fontSize - 6;
            txtItem_ITDSCA.Font.Bold = true;
            txtItem_ITDSCA.Height = 6;
            txtItem_ITDSCA.Width = 20;
            txtItem_ITDSCA.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSCA.TextPadding = new FrameThickness(0, txtItem_ITDSCA.Height / 8, 0, 0);
            txtItem_ITDSCA.BorderThickness = new FrameThickness(boder);



            TextItem txtItem_ITDSC2 = new TextItem(4.0, 22.0, 1.0, 1.0, "제조일");
            txtItem_ITDSC2.Font.Name = this.sFont;
            txtItem_ITDSC2.Font.Size = this.fontSize - 5;
            txtItem_ITDSC2.Font.Bold = true;
            txtItem_ITDSC2.Height = 6;
            txtItem_ITDSC2.Width = 20;
            txtItem_ITDSC2.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC2.TextPadding = new FrameThickness(0, txtItem_ITDSC.Height / 7, 0, 0);
            txtItem_ITDSC2.BorderThickness = new FrameThickness(boder);

            TextItem txtItem_ITDSC3 = new TextItem(4.0, 28.0, 1.0, 1.0, "제조량(Kg)");
            txtItem_ITDSC3.Font.Name = this.sFont;
            txtItem_ITDSC3.Font.Size = this.fontSize - 3;
            txtItem_ITDSC3.Font.Bold = true;
            txtItem_ITDSC3.Height = 9;
            txtItem_ITDSC3.Width = 20;
            txtItem_ITDSC3.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC3.TextPadding = new FrameThickness(0, txtItem_ITDSC.Height / 4, 0, 0);
            txtItem_ITDSC3.BorderThickness = new FrameThickness(boder);

            TextItem txtItem_ITDSC4 = new TextItem(4.0, 37.0, 1.0, 1.0, "수득량(Kg)");
            txtItem_ITDSC4.Font.Name = this.sFont;
            txtItem_ITDSC4.Font.Size = this.fontSize - 3;
            txtItem_ITDSC4.Font.Bold = true;
            txtItem_ITDSC4.Height = 9;
            txtItem_ITDSC4.Width = 20;
            txtItem_ITDSC4.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC4.TextPadding = new FrameThickness(0, txtItem_ITDSC.Height / 4, 0, 0);
            txtItem_ITDSC4.BorderThickness = new FrameThickness(boder);


            TextItem txtItem_ITDSC5 = new TextItem(4.0, 46.0, 1.0, 1.0, "판정일");
            txtItem_ITDSC5.Font.Name = this.sFont;
            txtItem_ITDSC5.Font.Size = this.fontSize - 3;
            txtItem_ITDSC5.Font.Bold = true;
            txtItem_ITDSC5.Height = 9;
            txtItem_ITDSC5.Width = 20;
            txtItem_ITDSC5.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC5.TextPadding = new FrameThickness(0, txtItem_ITDSC.Height / 4, 0, 0);
            txtItem_ITDSC5.BorderThickness = new FrameThickness(boder);


            TextItem txtItem_ITDSC6 = new TextItem(4.0, 55.0, 1.0, 1.0, "참고사항");
            txtItem_ITDSC6.Font.Name = this.sFont;
            txtItem_ITDSC6.Font.Size = this.fontSize - 3;
            txtItem_ITDSC6.Font.Bold = true;
            txtItem_ITDSC6.Height = 9;
            txtItem_ITDSC6.Width = 20;
            txtItem_ITDSC6.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC6.TextPadding = new FrameThickness(0, txtItem_ITDSC.Height / 4, 0, 0);
            txtItem_ITDSC6.BorderThickness = new FrameThickness(boder);

            TextItem txtItem_ITDSC7 = new TextItem(4.0, 64.0, 1.0, 1.0, "판정결과");
            txtItem_ITDSC7.Font.Name = this.sFont;
            txtItem_ITDSC7.Font.Size = this.fontSize - 3;
            txtItem_ITDSC7.Font.Bold = true;
            txtItem_ITDSC7.Height = 10;
            txtItem_ITDSC7.Width = 20;
            txtItem_ITDSC7.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC7.TextPadding = new FrameThickness(0, txtItem_ITDSC.Height / 4, 0, 0);
            txtItem_ITDSC7.BorderThickness = new FrameThickness(boder);



            TextItem txtItem_ITDSC16 = new TextItem(52.0, 22.0, 1.0, 1.0, "제조번호");
            txtItem_ITDSC16.Font.Name = this.sFont;
            txtItem_ITDSC16.Font.Size = this.fontSize - 5;
            txtItem_ITDSC16.Font.Bold = true;
            txtItem_ITDSC16.Height = 6;
            txtItem_ITDSC16.Width = 20;
            txtItem_ITDSC16.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC16.TextPadding = new FrameThickness(0, txtItem_ITDSC.Height / 7, 0, 0);
            txtItem_ITDSC16.BorderThickness = new FrameThickness(boder);

            TextItem txtItem_ITDSC17 = new TextItem(52.0, 28.0, 1.0, 1.0, "시험번호");
            txtItem_ITDSC17.Font.Name = this.sFont;
            txtItem_ITDSC17.Font.Size = this.fontSize - 3;
            txtItem_ITDSC17.Font.Bold = true;
            txtItem_ITDSC17.Height = 9;
            txtItem_ITDSC17.Width = 20;
            txtItem_ITDSC17.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC17.TextPadding = new FrameThickness(0, txtItem_ITDSC.Height / 4, 0, 0);
            txtItem_ITDSC17.BorderThickness = new FrameThickness(boder);


            TextItem txtItem_ITDSC18 = new TextItem(52.0, 37.0, 1.0, 1.0, "사용기한");
            txtItem_ITDSC18.Font.Name = this.sFont;
            txtItem_ITDSC18.Font.Size = this.fontSize - 3;
            txtItem_ITDSC18.Font.Bold = true;
            txtItem_ITDSC18.Height = 9;
            txtItem_ITDSC18.Width = 20;
            txtItem_ITDSC18.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC18.TextPadding = new FrameThickness(0, txtItem_ITDSC.Height / 4, 0, 0);
            txtItem_ITDSC18.BorderThickness = new FrameThickness(boder);


            //TextItem txtItem_ITDSC19 = new TextItem(52.0, 46.0, 1.0, 1.0, "판정사항");
            //txtItem_ITDSC19.Font.Name = this.sFont;
            //txtItem_ITDSC19.Font.Size = this.fontSize - 3;
            //txtItem_ITDSC19.Font.Bold = true;
            //txtItem_ITDSC19.Height = 9;
            //txtItem_ITDSC19.Width = 20;
            //txtItem_ITDSC19.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            //txtItem_ITDSC19.TextPadding = new FrameThickness(0, txtItem_ITDSC.Height / 4, 0, 0);
            //txtItem_ITDSC19.BorderThickness = new FrameThickness(boder);


            //벌크코드
            TextItem txtItem_ITDSC8 = new TextItem(24.0, 0, 1.0, 1.0, _vo.A01.ToString());
            txtItem_ITDSC8.Font.Name = this.sFont;
            txtItem_ITDSC8.Font.Size = this.fontSize - 2;
            txtItem_ITDSC8.Font.Bold = true;
            txtItem_ITDSC8.Height = 7;
            txtItem_ITDSC8.Width = 57;
            txtItem_ITDSC8.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC8.TextPadding = new FrameThickness(0, txtItem_ITDSC8.Height / 8, 0, 0);
            txtItem_ITDSC8.BorderThickness = new FrameThickness(boder);

            //제품명
            TextItem txtItem_ITDSC9 = new TextItem(24.0, 7.0, 1.0, 1.0, _vo.A02.ToString());
            txtItem_ITDSC9.Font.Name = this.sFont;
            txtItem_ITDSC9.Font.Size = this.fontSize - 6;
            txtItem_ITDSC9.Font.Bold = true;
            txtItem_ITDSC9.Height = 9;
            txtItem_ITDSC9.Width = 57;
            txtItem_ITDSC9.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC9.TextPadding = new FrameThickness(0, /*txtItem_ITDSC9.Height / 23*/0.1, 0, 0);
            txtItem_ITDSC9.BorderThickness = new FrameThickness(boder);

            TextItem txtItem_ITDSC9_1 = new TextItem(24.0, 16.0, 1.0, 1.0, _vo.A03.ToString());
            txtItem_ITDSC9_1.Font.Name = this.sFont;
            txtItem_ITDSC9_1.Font.Size = this.fontSize - 6;
            txtItem_ITDSC9_1.Font.Bold = true;
            txtItem_ITDSC9_1.Height = 6;
            txtItem_ITDSC9_1.Width = 57;
            txtItem_ITDSC9_1.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC9_1.TextPadding = new FrameThickness(0, txtItem_ITDSC9_1.Height / 8, 0, 0);
            txtItem_ITDSC9_1.BorderThickness = new FrameThickness(boder);

            //제조일
            TextItem txtItem_ITDSC10 = new TextItem(24.0, 22.0, 1.0, 1.0, _vo.A07.ToString());
            txtItem_ITDSC10.Font.Name = this.sFont;
            txtItem_ITDSC10.Font.Size = this.fontSize - 5;
            txtItem_ITDSC10.Font.Bold = true;
            txtItem_ITDSC10.Height = 6;
            txtItem_ITDSC10.Width = 28;
            txtItem_ITDSC10.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC10.TextPadding = new FrameThickness(0, txtItem_ITDSC10.Height / 7, 0, 0);
            txtItem_ITDSC10.BorderThickness = new FrameThickness(boder);

            //제조량
            TextItem txtItem_ITDSC11 = new TextItem(24.0, 28.0, 1.0, 1.0, String.Format("{0:##,###,###,###,##0.00}", Convert.ToDouble(_vo.A08)));
            txtItem_ITDSC11.Font.Name = this.sFont;
            txtItem_ITDSC11.Font.Size = this.fontSize - 3;
            txtItem_ITDSC11.Font.Bold = true;
            txtItem_ITDSC11.Height = 9;
            txtItem_ITDSC11.Width = 28;
            txtItem_ITDSC11.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC11.TextPadding = new FrameThickness(0, txtItem_ITDSC11.Height / 4, 0, 0);
            txtItem_ITDSC11.BorderThickness = new FrameThickness(boder);

            //수득량
            TextItem txtItem_ITDSC12 = new TextItem(24.0, 37.0, 1.0, 1.0, String.Format("{0:##,###,###,###,##0.00}", Convert.ToDouble(_vo.A09)));
            txtItem_ITDSC12.Font.Name = this.sFont;
            txtItem_ITDSC12.Font.Size = this.fontSize - 3;
            txtItem_ITDSC12.Font.Bold = true;
            txtItem_ITDSC12.Height = 9;
            txtItem_ITDSC12.Width = 28;
            txtItem_ITDSC12.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC12.TextPadding = new FrameThickness(0, txtItem_ITDSC12.Height / 4, 0, 0);
            txtItem_ITDSC12.BorderThickness = new FrameThickness(boder);

            //판정일
            TextItem txtItem_ITDSC13 = new TextItem(24.0, 46.0, 1.0, 1.0, _vo.A10.ToString());
            txtItem_ITDSC13.Font.Name = this.sFont;
            txtItem_ITDSC13.Font.Size = this.fontSize - 3;
            txtItem_ITDSC13.Font.Bold = true;
            txtItem_ITDSC13.Height = 9;
            txtItem_ITDSC13.Width = 75;
            txtItem_ITDSC13.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC13.TextPadding = new FrameThickness(0, txtItem_ITDSC13.Height / 4, 0, 0);
            txtItem_ITDSC13.BorderThickness = new FrameThickness(boder);

            //참고사항
            TextItem txtItem_ITDSC14 = new TextItem(24.0, 55.0, 1.0, 1.0, _vo.A11.ToString());
            txtItem_ITDSC14.Font.Name = this.sFont;
            txtItem_ITDSC14.Font.Size = this.fontSize - 3;
            txtItem_ITDSC14.Font.Bold = true;
            txtItem_ITDSC14.Height = 9;
            txtItem_ITDSC14.Width = 75;
            txtItem_ITDSC14.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC14.TextPadding = new FrameThickness(0, txtItem_ITDSC14.Height / 4, 0, 0);
            txtItem_ITDSC14.BorderThickness = new FrameThickness(boder);

            //분류
            TextItem txtItem_ITDSC15 = new TextItem(24.0, 64.0, 1.0, 1.0, _vo.A15.ToString());
            txtItem_ITDSC15.Font.Name = this.sFont;
            txtItem_ITDSC15.Font.Size = this.fontSize - 5;
            txtItem_ITDSC15.Font.Bold = true;
            txtItem_ITDSC15.Height = 10;
            txtItem_ITDSC15.Width = 75;
            txtItem_ITDSC15.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC15.TextPadding = new FrameThickness(0, txtItem_ITDSC15.Height / 4, 0, 0);
            txtItem_ITDSC15.BorderThickness = new FrameThickness(boder);

            //제조번호
            TextItem txtItem_ITDSC20 = new TextItem(72.0, 22.0, 1.0, 1.0, _vo.A14.ToString());
            txtItem_ITDSC20.Font.Name = this.sFont;
            txtItem_ITDSC20.Font.Size = this.fontSize - 5;
            txtItem_ITDSC20.Font.Bold = true;
            txtItem_ITDSC20.Height =6 ;
            txtItem_ITDSC20.Width = 27;
            txtItem_ITDSC20.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC20.TextPadding = new FrameThickness(0, txtItem_ITDSC20.Height / 7, 0, 0);
            txtItem_ITDSC20.BorderThickness = new FrameThickness(boder);

            //시험번호
            TextItem txtItem_ITDSC21 = new TextItem(72.0, 28.0, 1.0, 1.0, _vo.A13.ToString());
            txtItem_ITDSC21.Font.Name = this.sFont;
            txtItem_ITDSC21.Font.Size = this.fontSize - 3;
            txtItem_ITDSC21.Font.Bold = true;
            txtItem_ITDSC21.Height = 9;
            txtItem_ITDSC21.Width = 27;
            txtItem_ITDSC21.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC21.TextPadding = new FrameThickness(0, txtItem_ITDSC21.Height / 4, 0, 0);
            txtItem_ITDSC21.BorderThickness = new FrameThickness(boder);

            //사용기간
            TextItem txtItem_ITDSC22 = new TextItem(72.0, 37.0, 1.0, 1.0, _vo.A06.ToString());
            txtItem_ITDSC22.Font.Name = this.sFont;
            txtItem_ITDSC22.Font.Size = this.fontSize - 3;
            txtItem_ITDSC22.Font.Bold = true;
            txtItem_ITDSC22.Height = 9;
            txtItem_ITDSC22.Width = 27;
            txtItem_ITDSC22.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC22.TextPadding = new FrameThickness(0, txtItem_ITDSC22.Height / 4, 0, 0);
            txtItem_ITDSC22.BorderThickness = new FrameThickness(boder);


            ////판정사항
            //TextItem txtItem_ITDSC23 = new TextItem(72.0, 46.0, 1.0, 1.0, _vo.A12.ToString());
            //txtItem_ITDSC23.Font.Name = this.sFont;
            //txtItem_ITDSC23.Font.Size = this.fontSize - 8;
            //txtItem_ITDSC23.Font.Bold = true;
            //txtItem_ITDSC23.Height = 9;
            //txtItem_ITDSC23.Width = 27;
            //txtItem_ITDSC23.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            //txtItem_ITDSC23.TextPadding = new FrameThickness(0, txtItem_ITDSC23.Height / 8, 0, 0);
            //txtItem_ITDSC23.BorderThickness = new FrameThickness(boder);


            //바코드
            BarcodeItem ITDSC24 = new BarcodeItem(82, 2.0, 2.0, 2.0, BarcodeSymbology.QRCode, _vo.A05.ToString());
            //ITDSC24.DataMatrixModuleSize = 0.80;
            ITDSC24.QRCodeModuleSize = 0.6;
            ITDSC24.QRCodeEncoding = QRCodeEncoding.Byte;


            tLabel.Items.Add(txtItem_ITDSC);  // 품명
            tLabel.Items.Add(txtItem_ITDSC1); //품번 
            tLabel.Items.Add(txtItem_ITDSC2); // 수량
            tLabel.Items.Add(txtItem_ITDSC3); //입고일자
            tLabel.Items.Add(txtItem_ITDSC4); //제조원
            tLabel.Items.Add(txtItem_ITDSC5); //시험일
            tLabel.Items.Add(txtItem_ITDSC6); //참고사항
            tLabel.Items.Add(txtItem_ITDSC7); //분류
            tLabel.Items.Add(txtItem_ITDSC16); //보관위치
            tLabel.Items.Add(txtItem_ITDSC17); //유효기간
            tLabel.Items.Add(txtItem_ITDSC18); //제조번호
            tLabel.Items.Add(txtItem_ITDSCA); //시험번호

            tLabel.Items.Add(txtItem_ITDSC8); //품명D
            tLabel.Items.Add(txtItem_ITDSC9); //품번D
            tLabel.Items.Add(txtItem_ITDSC9_1); //품번D
            tLabel.Items.Add(txtItem_ITDSC10); //수량D
            tLabel.Items.Add(txtItem_ITDSC11); //입고일자D
            tLabel.Items.Add(txtItem_ITDSC12); //제조원D
            tLabel.Items.Add(txtItem_ITDSC13); //시험일D
            tLabel.Items.Add(txtItem_ITDSC14); //참고사항D
            tLabel.Items.Add(txtItem_ITDSC15); //분류D
            tLabel.Items.Add(txtItem_ITDSC20); //보관위치D
            tLabel.Items.Add(txtItem_ITDSC21); //유효기간D
            tLabel.Items.Add(txtItem_ITDSC22); //제조번호D
            //tLabel.Items.Add(txtItem_ITDSC23); //시험번호D

            tLabel.Items.Add(ITDSC24); //바코드

            return tLabel;
        }



        //본사
        public bool SmallPackingPrint_Godex(ManVo Vo, int CNT = 0)
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
                    pj.BufferOutput = true;
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


        private ThermalLabel GenerateBasicThermalLabel(ManVo Vo)
        {
            //Define a ThermalLabel object and set unit to inch and label size
            ThermalLabel tLabel = new ThermalLabel(UnitType.Mm, 100, 80);
            tLabel.GapLength = 0.1;

            TextItem txtItem_ITDSC = new TextItem(1.7, 1.5, 2.5, 0.5, "벌크코드");
            txtItem_ITDSC.Font.Name = this.sFont;
            txtItem_ITDSC.Font.Size = this.fontSize - 3;
            txtItem_ITDSC.Font.Bold = true;
            txtItem_ITDSC.Height = 14;
            txtItem_ITDSC.Width = 20;
            txtItem_ITDSC.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC.TextPadding = new FrameThickness(0, txtItem_ITDSC.Height / 4, 0, 0);
            txtItem_ITDSC.BorderThickness = new FrameThickness(boder);
            //txtItem_MTRL_LOT_NO.BackColor = Color.Black;
            //txtItem_MTRL_LOT_NO.ForeColor = Color.White;
            //txtItem_MTRL_LOT_NO.BorderColor = Color.White;

            TextItem txtItem_ITDSC_VAL = new TextItem(21.4, 1.5, 2.5, 0.5, " " + Vo.ASSY_ITM_CD);
            txtItem_ITDSC_VAL.Font.Name = this.sFont;
            txtItem_ITDSC_VAL.Font.Size = this.fontSize + 3;
            txtItem_ITDSC_VAL.Font.Bold = true;
            txtItem_ITDSC_VAL.Height = 14;
            txtItem_ITDSC_VAL.Width = 73;
            //txtItem_ITDSC_VAL.DataField = "ITDSC";
            txtItem_ITDSC_VAL.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Left;
            txtItem_ITDSC_VAL.TextPadding = new FrameThickness(0, txtItem_ITDSC_VAL.Height / 7, 0, 0);
            txtItem_ITDSC_VAL.BorderThickness = new FrameThickness(boder);



            TextItem txtItem_ISPEC = new TextItem(1.7, 15.5, 2.5, 0.5, "제조번호");
            txtItem_ISPEC.Font.Name = this.sFont;
            txtItem_ISPEC.Font.Size = this.fontSize - 3;
            txtItem_ISPEC.Font.Bold = true;
            txtItem_ISPEC.Height = 14;
            txtItem_ISPEC.Width = 20;
            txtItem_ISPEC.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ISPEC.TextPadding = new FrameThickness(0, txtItem_ISPEC.Height / 4, 0, 0);
            txtItem_ISPEC.BorderThickness = new FrameThickness(boder);
            //txtItem_MTRL_LOT_NO.BackColor = Color.Black;
            //txtItem_MTRL_LOT_NO.ForeColor = Color.White;
            //txtItem_MTRL_LOT_NO.BorderColor = Color.White;

            TextItem txtItem_ISPEC_VAL = new TextItem(21.4, 15.5, 2.5, 0.5, " " + Vo.INP_LOT_NO);
            txtItem_ISPEC_VAL.Font.Name = this.sFont;
            txtItem_ISPEC_VAL.Font.Size = this.fontSize - 1;
            //txtItem_ISPEC_VAL.DataField = "ISPEC";
            txtItem_ISPEC_VAL.Height = 14;
            txtItem_ISPEC_VAL.Width = 73;
            txtItem_ISPEC_VAL.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Left;
            txtItem_ISPEC_VAL.TextPadding = new FrameThickness(0, txtItem_ISPEC_VAL.Height / 4, 0, 0);
            txtItem_ISPEC_VAL.BorderThickness = new FrameThickness(boder);


            //상
            TextItem txtItem_ISPE = new TextItem(55, 15.5, 2.5, 0.5, "상");
            txtItem_ISPE.Font.Name = this.sFont;
            txtItem_ISPE.Font.Size = this.fontSize + 12;
            txtItem_ISPE.Font.Bold = true;
            txtItem_ISPE.Height = 14;
            txtItem_ISPE.Width = 20;
            txtItem_ISPE.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ISPE.TextPadding = new FrameThickness(0.2);
            txtItem_ISPE.BorderThickness = new FrameThickness(boder);

          
             TextItem txtItem_ISP = new TextItem(74.7, 15.5, 2.5, 0.5, ""+Vo.ORD_CLS_CD);
            txtItem_ISP.Font.Name = this.sFont;
            txtItem_ISP.Font.Size = this.fontSize + 12;
            txtItem_ISP.Font.Bold = true;
            txtItem_ISP.Height = 14;
            txtItem_ISP.Width = 19.6;
            txtItem_ISP.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ISP.TextPadding = new FrameThickness(0.2);
            txtItem_ISP.BorderThickness = new FrameThickness(boder);




            TextItem txtItem_MAXQT = new TextItem(1.7, 29.5, 2.5, 0.5, "원료코드");
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

            //TextItem txtItem_MAXQT_VAL = new TextItem(21.4, 29.5, 2.5, 0.5, " " + String.Format("{0:##,###,###,###,##0}", 100));
            TextItem txtItem_MAXQT_VAL = new TextItem(21.4, 29.5, 2.5, 0.5, " " + Vo.CMPO_CD);
            txtItem_MAXQT_VAL.Font.Name = this.sFont;
            txtItem_MAXQT_VAL.Font.Size = this.fontSize - 1;
            //txtItem_MAXQT_VAL.DataField = "MAXQT";
            //txtItem_MAXQT_VAL.DataFieldFormatString = "{0:##,###,###,###,##0}";
            txtItem_MAXQT_VAL.Height = 14;
            txtItem_MAXQT_VAL.Width = 73;
            txtItem_MAXQT_VAL.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Left;
            txtItem_MAXQT_VAL.TextPadding = new FrameThickness(0, txtItem_MAXQT_VAL.Height / 4, 0, 0);
            txtItem_MAXQT_VAL.BorderThickness = new FrameThickness(boder);


            TextItem txtItem_Test = new TextItem(55, 29.5, 2.5, 0.5, "시험번호");
            txtItem_Test.Font.Name = this.sFont;
            txtItem_Test.Font.Size = this.fontSize - 2;
            txtItem_Test.Font.Bold = true;
            txtItem_Test.Height = 14;
            txtItem_Test.Width = 20;
            txtItem_Test.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_Test.TextPadding = new FrameThickness(0, txtItem_Test.Height / 4, 0, 0);
            txtItem_Test.BorderThickness = new FrameThickness(boder);

            TextItem txtItem_TestVal = new TextItem(74.7, 29.5, 2.5, 0.5, " " + Vo.INSP_NO);
            txtItem_TestVal.Font.Name = this.sFont;
            txtItem_TestVal.Font.Size = this.fontSize - 5;
            txtItem_TestVal.Height = 14;
            txtItem_TestVal.Width = 19.6;
            txtItem_TestVal.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Left;
            txtItem_TestVal.TextPadding = new FrameThickness(0, 0.2, 0, 0);
            txtItem_TestVal.BorderThickness = new FrameThickness(boder);






            TextItem txtItem_MAXQT1 = new TextItem(1.7, 43.5, 2.5, 0.5, "칭량량(g)");
            txtItem_MAXQT1.Font.Name = this.sFont;
            txtItem_MAXQT1.Font.Size = this.fontSize - 3;
            txtItem_MAXQT1.Font.Bold = true;
            txtItem_MAXQT1.Height = 14;
            txtItem_MAXQT1.Width = 20;
            txtItem_MAXQT1.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_MAXQT1.TextPadding = new FrameThickness(0, txtItem_MAXQT1.Height / 4, 0, 0);
            txtItem_MAXQT1.BorderThickness = new FrameThickness(boder);
            //txtItem_MTRL_LOT_NO.BackColor = Color.Black;
            //txtItem_MTRL_LOT_NO.ForeColor = Color.White;
            //txtItem_MTRL_LOT_NO.BorderColor = Color.White;

            TextItem txtItem_MAXQT_VAL2 = new TextItem(21.4, 43.5, 2.5, 0.5, " " + String.Format("{0:###,##0.##}", Convert.ToDecimal(Vo.WEIH_VAL)));
            //TextItem txtItem_MAXQT_VAL2 = new TextItem(21.4, 43.5, 2.5, 0.5, " " + Vo.CMPO_CD);
            txtItem_MAXQT_VAL2.Font.Name = this.sFont;
            txtItem_MAXQT_VAL2.Font.Size = this.fontSize + 3;
            //txtItem_MAXQT_VAL.DataField = "MAXQT";
            //txtItem_MAXQT_VAL.DataFieldFormatString = "{0:##,###,###,###,##0}";
            txtItem_MAXQT_VAL2.Height = 14;
            txtItem_MAXQT_VAL2.Width = 73;
            txtItem_MAXQT_VAL2.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Left;
            txtItem_MAXQT_VAL2.TextPadding = new FrameThickness(0, txtItem_MAXQT_VAL2.Height / 6, 0, 0);
            txtItem_MAXQT_VAL2.BorderThickness = new FrameThickness(boder);


            TextItem txtItem_Tes3t = new TextItem(55, 43.5, 2.5, 0.5, "칭량일");
            txtItem_Tes3t.Font.Name = this.sFont;
            txtItem_Tes3t.Font.Size = this.fontSize - 2;
            txtItem_Tes3t.Font.Bold = true;
            txtItem_Tes3t.Height = 14;
            txtItem_Tes3t.Width = 20;
            txtItem_Tes3t.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_Tes3t.TextPadding = new FrameThickness(0, txtItem_Tes3t.Height / 4, 0, 0);
            txtItem_Tes3t.BorderThickness = new FrameThickness(boder);

            TextItem txtItem_TestVal4 = new TextItem(74.7, 43.5, 2.5, 0.5, " " + System.DateTime.Now.ToString("yyyy.MM.dd"));
            txtItem_TestVal4.Font.Name = this.sFont;
            txtItem_TestVal4.Font.Size = this.fontSize -5;
            txtItem_TestVal4.Height = 14;
            txtItem_TestVal4.Width = 19.6;
            txtItem_TestVal4.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Left;
            txtItem_TestVal4.TextPadding = new FrameThickness(0, txtItem_TestVal4.Height / 4, 0, 0);
            txtItem_TestVal4.BorderThickness = new FrameThickness(boder);



            //TextItem txtItem_ITNBR = new TextItem(1.7, 57.5, 2.5, 0.5, "품  번");
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
            ////BarcodeItem bcItem_ITNBR_VAL = new BarcodeItem(21.4, 3.5, 73, 14, BarcodeSymbology.Code128, Vo.ITNBR);
            //////bcItem_ITNBR_VAL.Height = 14;
            //////bcItem_ITNBR_VAL.Width = 73;
            //////Set bars height to .75inch
            ////bcItem_ITNBR_VAL.BarHeight = 0.75;
            //////Set bars width to 0.0104inch
            ////bcItem_ITNBR_VAL.BarWidth = 0.0104;
            ////bcItem_ITNBR_VAL.Sizing = BarcodeSizing.Fill;

            //TextItem txtItem_ITNBR_VAL = new TextItem(21.4, 43.5, 2.5, 0.5, "      " + "Vo.ITNBR");
            //txtItem_ITNBR_VAL.Font.Name = this.sFont;
            ////txtItem_ITNBR_VAL.DataField = "ITNBR";
            //txtItem_ITNBR_VAL.Font.Size = 8;
            //txtItem_ITNBR_VAL.Height = 14;
            //txtItem_ITNBR_VAL.Width = 73;
            //txtItem_ITNBR_VAL.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Left;
            //txtItem_ITNBR_VAL.TextPadding = new FrameThickness(0, txtItem_ITNBR_VAL.Height - 4, 0, 0);
            //txtItem_ITNBR_VAL.BorderThickness = new FrameThickness(boder);


            //BarcodeItem bc2 = new BarcodeItem(3, 0.2, 2.5, 1.5, BarcodeSymbology.DataMatrix, "0123456789ABCDEF");
            //bc2.DataMatrixModuleSize = 0.04;

            //Define a BarcodeItem object
            BarcodeItem bcItem_ITNBR_VAL = new BarcodeItem(5, 60, 30, 11, BarcodeSymbology.QRCode, Vo.MTRL_LOT_NO);
            bcItem_ITNBR_VAL.QRCodeModuleSize = 0.7;
            bcItem_ITNBR_VAL.QRCodeEncoding = QRCodeEncoding.Byte;


            //bcItem_ITNBR_VAL.QRCodeModuleSize = 0.6;
            //set counter step for increasing by 1
            //bcItem_ITNBR_VAL.CounterStep = 1;
            //bcItem_ITNBR_VAL.DataField = "ITNBR";
            //set barcode size
            //bcItem_ITNBR_VAL.BarWidth = 0.02;
            //bcItem_ITNBR_VAL.BarHeight = 0.75;
            //bcItem_ITNBR_VAL.Sizing = BarcodeSizing.Fill;
            //set barcode alignment
            //bcItem_ITNBR_VAL.BarcodeAlignment = BarcodeAlignment.MiddleCenter;
            ////set font
            //bcItem_ITNBR_VAL.Font.Name = this.sFont;
            //bcItem_ITNBR_VAL.Font.Unit = FontUnit.Point;
            //bcItem_ITNBR_VAL.Font.Size = 4;
            //bcItem_ITNBR_VAL.DisplayCode = false;

            //bcItem_ITNBR_VAL.BarcodeAlignment = BarcodeAlignment.MiddleCenter;
            //bcItem_ITNBR_VAL.BorderThickness = new FrameThickness(boder);

            TextItem txtItem_Tes5t = new TextItem(1.7, 57.5, 2.5, 0.5, " ");
            txtItem_Tes5t.Font.Name = this.sFont;
            txtItem_Tes5t.Font.Size = this.fontSize - 2;
            txtItem_Tes5t.Font.Bold = true;
            txtItem_Tes5t.Height = 20;
            txtItem_Tes5t.Width = 92.6;
            txtItem_Tes5t.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_Tes5t.TextPadding = new FrameThickness(0, txtItem_Tes5t.Height / 4, 0, 0);
            txtItem_Tes5t.BorderThickness = new FrameThickness(boder);


            //TextItem txtItem_REMARK = new TextItem(1.7, 57.5, 2.5, 0.5, "株式會社  大 吉 通 商");
            TextItem txtItem_REMARK = new TextItem(23, 57.5, 2.5, 0.5, "화성코스메틱(주)");
            txtItem_REMARK.Font.Name = this.sFont;
            txtItem_REMARK.Font.Size = this.fontSize + 8;
            txtItem_REMARK.Font.Bold = true;
            txtItem_REMARK.Height = 20;
            txtItem_REMARK.Width = 80;
            txtItem_REMARK.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_REMARK.TextPadding = new FrameThickness(0, 5, 0, 0);
            //txtItem_REMARK.BorderThickness = new FrameThickness(boder);

            //TextItem txtItem_REMARK1 = new TextItem(1.7, 68.5, 92.5, 21, " 우)15089  경기도 시흥시 소망공원로 159");
            //txtItem_REMARK1.Font.Name = this.sFont;
            //txtItem_REMARK1.Font.Size = this.fontSize - 5;
            //txtItem_REMARK1.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Left;
            //txtItem_REMARK1.TextPadding = new FrameThickness(5, 0, 0, 0);

            //TextItem txtItem_REMARK2 = new TextItem(1.7, 72.5, 92.5, 21, " TEL : (031)499-1011");
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

            tLabel.Items.Add(txtItem_MAXQT);
            tLabel.Items.Add(txtItem_MAXQT_VAL);

            tLabel.Items.Add(txtItem_Test);
            tLabel.Items.Add(txtItem_TestVal);


            tLabel.Items.Add(txtItem_MAXQT1);
            tLabel.Items.Add(txtItem_MAXQT_VAL2);
            tLabel.Items.Add(txtItem_Tes3t);
            tLabel.Items.Add(txtItem_TestVal4);


            tLabel.Items.Add(bcItem_ITNBR_VAL);
            tLabel.Items.Add(txtItem_REMARK);

            tLabel.Items.Add(txtItem_Tes5t);

            tLabel.Items.Add(txtItem_ISPE);
            tLabel.Items.Add(txtItem_ISP);
            //tLabel.Items.Add(txtItem_REMARK1);
            //tLabel.Items.Add(txtItem_REMARK2);


            //tLabel.DataSource = Vo;

            return tLabel;
        }





        //원자재
        public bool M_Godex(SystemCodeVo _vo, int _cnt = 1)
        {
            try
            {


                _printerSettings = new PrinterSettings();
                _printerSettings.Communication.CommunicationType = CommunicationType.USB;
                _printerSettings.PrinterName = Properties.Settings.Default.str_PrnNm;

                using (pj = new PrintJob(_printerSettings))
                {
                    pj.BufferOutput = true;
                    pj.Copies = _cnt;
                    pj.Replicates = 1;
                    pj.PrintOrientation = PrintOrientation.Portrait;
                    pj.ThermalLabel = GenerateBasicThermalLabel_M(_vo);
                    pj.Print();

                    return true;
                }

            }
            catch (Exception eLog)
            {
                //WinUIMessageBox.Show(eLog.Message, "[에러]" + SystemProperties.PROGRAM_TITLE, MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
        }


        //부자재
        public bool B_Godex(SystemCodeVo _vo, int _cnt = 1)
        {
            try
            {


                _printerSettings = new PrinterSettings();
                _printerSettings.Communication.CommunicationType = CommunicationType.USB;
                _printerSettings.PrinterName = Properties.Settings.Default.str_PrnNm;

                using (pj = new PrintJob(_printerSettings))
                {
                    pj.BufferOutput = true;
                    pj.Copies = _cnt;
                    pj.Replicates = 1;
                    pj.PrintOrientation = PrintOrientation.Portrait;
                    pj.ThermalLabel = GenerateBasicThermalLabel_B(_vo);
                    pj.Print();

                    return true;
                }

            }
            catch (Exception eLog)
            {
                //WinUIMessageBox.Show(eLog.Message, "[에러]" + SystemProperties.PROGRAM_TITLE, MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
        }


        //원자재
        private ThermalLabel GenerateBasicThermalLabel_M(SystemCodeVo _vo)
        {
            ThermalLabel tLabel = new ThermalLabel(UnitType.Mm, 100, 80);
            tLabel.GapLength = 0.1;


            TextItem txtItem_ITDSC = new TextItem(4.0, 1.0, 1.0, 1.0, "품명");
            txtItem_ITDSC.Font.Name = this.sFont;
            txtItem_ITDSC.Font.Size = this.fontSize - 3;
            txtItem_ITDSC.Font.Bold = true;
            txtItem_ITDSC.Height = 9;
            txtItem_ITDSC.Width = 20;
            txtItem_ITDSC.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC.TextPadding = new FrameThickness(0, txtItem_ITDSC.Height / 4, 0, 0);
            txtItem_ITDSC.BorderThickness = new FrameThickness(boder);

            TextItem txtItem_ITDSC1 = new TextItem(4.0, 10.0, 1.0, 1.0, "품번");
            txtItem_ITDSC1.Font.Name = this.sFont;
            txtItem_ITDSC1.Font.Size = this.fontSize - 3;
            txtItem_ITDSC1.Font.Bold = true;
            txtItem_ITDSC1.Height = 9;
            txtItem_ITDSC1.Width = 20;
            txtItem_ITDSC1.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC1.TextPadding = new FrameThickness(0, txtItem_ITDSC.Height / 4, 0, 0);
            txtItem_ITDSC1.BorderThickness = new FrameThickness(boder);

            TextItem txtItem_ITDSC2 = new TextItem(4.0, 19.0, 1.0, 1.0, "수량");
            txtItem_ITDSC2.Font.Name = this.sFont;
            txtItem_ITDSC2.Font.Size = this.fontSize - 3;
            txtItem_ITDSC2.Font.Bold = true;
            txtItem_ITDSC2.Height = 9;
            txtItem_ITDSC2.Width = 20;
            txtItem_ITDSC2.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC2.TextPadding = new FrameThickness(0, txtItem_ITDSC.Height / 4, 0, 0);
            txtItem_ITDSC2.BorderThickness = new FrameThickness(boder);

            TextItem txtItem_ITDSC3 = new TextItem(4.0, 28.0, 1.0, 1.0, "입고일자");
            txtItem_ITDSC3.Font.Name = this.sFont;
            txtItem_ITDSC3.Font.Size = this.fontSize - 3;
            txtItem_ITDSC3.Font.Bold = true;
            txtItem_ITDSC3.Height = 9;
            txtItem_ITDSC3.Width = 20;
            txtItem_ITDSC3.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC3.TextPadding = new FrameThickness(0, txtItem_ITDSC.Height / 4, 0, 0);
            txtItem_ITDSC3.BorderThickness = new FrameThickness(boder);

            TextItem txtItem_ITDSC4 = new TextItem(4.0, 37.0, 1.0, 1.0, "제조원");
            txtItem_ITDSC4.Font.Name = this.sFont;
            txtItem_ITDSC4.Font.Size = this.fontSize - 3;
            txtItem_ITDSC4.Font.Bold = true;
            txtItem_ITDSC4.Height = 9;
            txtItem_ITDSC4.Width = 20;
            txtItem_ITDSC4.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC4.TextPadding = new FrameThickness(0, txtItem_ITDSC.Height / 4, 0, 0);
            txtItem_ITDSC4.BorderThickness = new FrameThickness(boder);


            TextItem txtItem_ITDSC5 = new TextItem(4.0, 46.0, 1.0, 1.0, "시험일");
            txtItem_ITDSC5.Font.Name = this.sFont;
            txtItem_ITDSC5.Font.Size = this.fontSize - 3;
            txtItem_ITDSC5.Font.Bold = true;
            txtItem_ITDSC5.Height = 9;
            txtItem_ITDSC5.Width = 20;
            txtItem_ITDSC5.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC5.TextPadding = new FrameThickness(0, txtItem_ITDSC.Height / 4, 0, 0);
            txtItem_ITDSC5.BorderThickness = new FrameThickness(boder);


            TextItem txtItem_ITDSC6 = new TextItem(4.0, 55.0, 1.0, 1.0, "참고사항");
            txtItem_ITDSC6.Font.Name = this.sFont;
            txtItem_ITDSC6.Font.Size = this.fontSize - 3;
            txtItem_ITDSC6.Font.Bold = true;
            txtItem_ITDSC6.Height = 9;
            txtItem_ITDSC6.Width = 20;
            txtItem_ITDSC6.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC6.TextPadding = new FrameThickness(0, txtItem_ITDSC.Height / 4, 0, 0);
            txtItem_ITDSC6.BorderThickness = new FrameThickness(boder);

            TextItem txtItem_ITDSC7 = new TextItem(4.0, 64.0, 1.0, 1.0, "분류");
            txtItem_ITDSC7.Font.Name = this.sFont;
            txtItem_ITDSC7.Font.Size = this.fontSize - 3;
            txtItem_ITDSC7.Font.Bold = true;
            txtItem_ITDSC7.Height = 10;
            txtItem_ITDSC7.Width = 20;
            txtItem_ITDSC7.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC7.TextPadding = new FrameThickness(0, txtItem_ITDSC.Height / 4, 0, 0);
            txtItem_ITDSC7.BorderThickness = new FrameThickness(boder);



            TextItem txtItem_ITDSC16 = new TextItem(52.0, 19.0, 1.0, 1.0, "보관위치");
            txtItem_ITDSC16.Font.Name = this.sFont;
            txtItem_ITDSC16.Font.Size = this.fontSize - 3;
            txtItem_ITDSC16.Font.Bold = true;
            txtItem_ITDSC16.Height = 9;
            txtItem_ITDSC16.Width = 20;
            txtItem_ITDSC16.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC16.TextPadding = new FrameThickness(0, txtItem_ITDSC.Height / 4, 0, 0);
            txtItem_ITDSC16.BorderThickness = new FrameThickness(boder);

            TextItem txtItem_ITDSC17 = new TextItem(52.0, 28.0, 1.0, 1.0, "유효기간");
            txtItem_ITDSC17.Font.Name = this.sFont;
            txtItem_ITDSC17.Font.Size = this.fontSize - 3;
            txtItem_ITDSC17.Font.Bold = true;
            txtItem_ITDSC17.Height = 9;
            txtItem_ITDSC17.Width = 20;
            txtItem_ITDSC17.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC17.TextPadding = new FrameThickness(0, txtItem_ITDSC.Height / 4, 0, 0);
            txtItem_ITDSC17.BorderThickness = new FrameThickness(boder);


            TextItem txtItem_ITDSC18 = new TextItem(52.0, 37.0, 1.0, 1.0, "제조번호");
            txtItem_ITDSC18.Font.Name = this.sFont;
            txtItem_ITDSC18.Font.Size = this.fontSize - 3;
            txtItem_ITDSC18.Font.Bold = true;
            txtItem_ITDSC18.Height = 9;
            txtItem_ITDSC18.Width = 20;
            txtItem_ITDSC18.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC18.TextPadding = new FrameThickness(0, txtItem_ITDSC.Height / 4, 0, 0);
            txtItem_ITDSC18.BorderThickness = new FrameThickness(boder);


            TextItem txtItem_ITDSC19 = new TextItem(52.0, 46.0, 1.0, 1.0, "시험번호");
            txtItem_ITDSC19.Font.Name = this.sFont;
            txtItem_ITDSC19.Font.Size = this.fontSize - 3;
            txtItem_ITDSC19.Font.Bold = true;
            txtItem_ITDSC19.Height = 9;
            txtItem_ITDSC19.Width = 20;
            txtItem_ITDSC19.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC19.TextPadding = new FrameThickness(0, txtItem_ITDSC.Height / 4, 0, 0);
            txtItem_ITDSC19.BorderThickness = new FrameThickness(boder);


            //품명
            TextItem txtItem_ITDSC8 = new TextItem(24.0, 1.0, 1.0, 1.0, _vo.ITM_CD.ToString());
            txtItem_ITDSC8.Font.Name = this.sFont;
            txtItem_ITDSC8.Font.Size = this.fontSize - 3;
            txtItem_ITDSC8.Font.Bold = true;
            txtItem_ITDSC8.Height = 9;
            txtItem_ITDSC8.Width = 57;
            txtItem_ITDSC8.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC8.TextPadding = new FrameThickness(0, txtItem_ITDSC8.Height / 4, 0, 0);
            txtItem_ITDSC8.BorderThickness = new FrameThickness(boder);

            //품번
            TextItem txtItem_ITDSC9 = new TextItem(24.0, 10.0, 1.0, 1.0, _vo.ITM_NM.ToString());
            txtItem_ITDSC9.Font.Name = this.sFont;
            txtItem_ITDSC9.Font.Size = this.fontSize - 3;
            txtItem_ITDSC9.Font.Bold = true;
            txtItem_ITDSC9.Height = 9;
            txtItem_ITDSC9.Width = 57;
            txtItem_ITDSC9.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC9.TextPadding = new FrameThickness(0, txtItem_ITDSC9.Height / 4, 0, 0);
            txtItem_ITDSC9.BorderThickness = new FrameThickness(boder);

            //수량
            TextItem txtItem_ITDSC10 = new TextItem(24.0, 19.0, 1.0, 1.0, " ");
            txtItem_ITDSC10.Font.Name = this.sFont;
            txtItem_ITDSC10.Font.Size = this.fontSize - 3;
            txtItem_ITDSC10.Font.Bold = true;
            txtItem_ITDSC10.Height = 9;
            txtItem_ITDSC10.Width = 28;
            txtItem_ITDSC10.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC10.TextPadding = new FrameThickness(0, txtItem_ITDSC10.Height / 4, 0, 0);
            txtItem_ITDSC10.BorderThickness = new FrameThickness(boder);

            //입고일자
            TextItem txtItem_ITDSC11 = new TextItem(24.0, 28.0, 1.0, 1.0, " ");
            txtItem_ITDSC11.Font.Name = this.sFont;
            txtItem_ITDSC11.Font.Size = this.fontSize - 3;
            txtItem_ITDSC11.Font.Bold = true;
            txtItem_ITDSC11.Height = 9;
            txtItem_ITDSC11.Width = 28;
            txtItem_ITDSC11.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC11.TextPadding = new FrameThickness(0, txtItem_ITDSC11.Height / 4, 0, 0);
            txtItem_ITDSC11.BorderThickness = new FrameThickness(boder);

            //제조원
            TextItem txtItem_ITDSC12 = new TextItem(24.0, 37.0, 1.0, 1.0, " ");
            txtItem_ITDSC12.Font.Name = this.sFont;
            txtItem_ITDSC12.Font.Size = this.fontSize - 3;
            txtItem_ITDSC12.Font.Bold = true;
            txtItem_ITDSC12.Height = 9;
            txtItem_ITDSC12.Width = 28;
            txtItem_ITDSC12.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC12.TextPadding = new FrameThickness(0, txtItem_ITDSC12.Height / 4, 0, 0);
            txtItem_ITDSC12.BorderThickness = new FrameThickness(boder);

            //시험일
            TextItem txtItem_ITDSC13 = new TextItem(24.0, 46.0, 1.0, 1.0, " ");
            txtItem_ITDSC13.Font.Name = this.sFont;
            txtItem_ITDSC13.Font.Size = this.fontSize - 3;
            txtItem_ITDSC13.Font.Bold = true;
            txtItem_ITDSC13.Height = 9;
            txtItem_ITDSC13.Width = 28;
            txtItem_ITDSC13.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC13.TextPadding = new FrameThickness(0, txtItem_ITDSC13.Height / 4, 0, 0);
            txtItem_ITDSC13.BorderThickness = new FrameThickness(boder);

            //참고사항
            TextItem txtItem_ITDSC14 = new TextItem(24.0, 55.0, 1.0, 1.0, " ");
            txtItem_ITDSC14.Font.Name = this.sFont;
            txtItem_ITDSC14.Font.Size = this.fontSize - 3;
            txtItem_ITDSC14.Font.Bold = true;
            txtItem_ITDSC14.Height = 9;
            txtItem_ITDSC14.Width = 75;
            txtItem_ITDSC14.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC14.TextPadding = new FrameThickness(0, txtItem_ITDSC14.Height / 4, 0, 0);
            txtItem_ITDSC14.BorderThickness = new FrameThickness(boder);

            //분류
            TextItem txtItem_ITDSC15 = new TextItem(24.0, 64.0, 1.0, 1.0, _vo.ITM_GRP_CLSS_NM.ToString());
            txtItem_ITDSC15.Font.Name = this.sFont;
            txtItem_ITDSC15.Font.Size = this.fontSize - 3;
            txtItem_ITDSC15.Font.Bold = true;
            txtItem_ITDSC15.Height = 10;
            txtItem_ITDSC15.Width = 75;
            txtItem_ITDSC15.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC15.TextPadding = new FrameThickness(0, txtItem_ITDSC15.Height / 4, 0, 0);
            txtItem_ITDSC15.BorderThickness = new FrameThickness(boder);

            //보관위치
            TextItem txtItem_ITDSC20 = new TextItem(72.0, 19.0, 1.0, 1.0, " ");
            txtItem_ITDSC20.Font.Name = this.sFont;
            txtItem_ITDSC20.Font.Size = this.fontSize - 8;
            txtItem_ITDSC20.Font.Bold = true;
            txtItem_ITDSC20.Height = 9;
            txtItem_ITDSC20.Width = 27;
            txtItem_ITDSC20.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC20.TextPadding = new FrameThickness(0, txtItem_ITDSC20.Height / 4, 0, 0);
            txtItem_ITDSC20.BorderThickness = new FrameThickness(boder);

            //유효기간
            TextItem txtItem_ITDSC21 = new TextItem(72.0, 28.0, 1.0, 1.0, " ");
            txtItem_ITDSC21.Font.Name = this.sFont;
            txtItem_ITDSC21.Font.Size = this.fontSize - 3;
            txtItem_ITDSC21.Font.Bold = true;
            txtItem_ITDSC21.Height = 9;
            txtItem_ITDSC21.Width = 27;
            txtItem_ITDSC21.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC21.TextPadding = new FrameThickness(0, txtItem_ITDSC21.Height / 4, 0, 0);
            txtItem_ITDSC21.BorderThickness = new FrameThickness(boder);

            //제조번호
            TextItem txtItem_ITDSC22 = new TextItem(72.0, 37.0, 1.0, 1.0, " ");
            txtItem_ITDSC22.Font.Name = this.sFont;
            txtItem_ITDSC22.Font.Size = this.fontSize - 3;
            txtItem_ITDSC22.Font.Bold = true;
            txtItem_ITDSC22.Height = 9;
            txtItem_ITDSC22.Width = 27;
            txtItem_ITDSC22.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC22.TextPadding = new FrameThickness(0, txtItem_ITDSC22.Height / 4, 0, 0);
            txtItem_ITDSC22.BorderThickness = new FrameThickness(boder);


            //시험번호
            TextItem txtItem_ITDSC23 = new TextItem(72.0, 46.0, 1.0, 1.0, " ");
            txtItem_ITDSC23.Font.Name = this.sFont;
            txtItem_ITDSC23.Font.Size = this.fontSize - 3;
            txtItem_ITDSC23.Font.Bold = true;
            txtItem_ITDSC23.Height = 9;
            txtItem_ITDSC23.Width = 27;
            txtItem_ITDSC23.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC23.TextPadding = new FrameThickness(0, txtItem_ITDSC23.Height / 4, 0, 0);
            txtItem_ITDSC23.BorderThickness = new FrameThickness(boder);


            //바코드
            BarcodeItem ITDSC24 = new BarcodeItem(82, 2.0, 2.0, 2.0, BarcodeSymbology.QRCode, _vo.ITM_CD.ToString());
            ITDSC24.QRCodeModuleSize = 0.8;
            ITDSC24.QRCodeEncoding = QRCodeEncoding.Byte;

            tLabel.Items.Add(txtItem_ITDSC);  // 품명
            tLabel.Items.Add(txtItem_ITDSC1); //품번 
            tLabel.Items.Add(txtItem_ITDSC2); // 수량
            tLabel.Items.Add(txtItem_ITDSC3); //입고일자
            tLabel.Items.Add(txtItem_ITDSC4); //제조원
            tLabel.Items.Add(txtItem_ITDSC5); //시험일
            tLabel.Items.Add(txtItem_ITDSC6); //참고사항
            tLabel.Items.Add(txtItem_ITDSC7); //분류
            tLabel.Items.Add(txtItem_ITDSC16); //보관위치
            tLabel.Items.Add(txtItem_ITDSC17); //유효기간
            tLabel.Items.Add(txtItem_ITDSC18); //제조번호
            tLabel.Items.Add(txtItem_ITDSC19); //시험번호

            tLabel.Items.Add(txtItem_ITDSC8); //품명D
            tLabel.Items.Add(txtItem_ITDSC9); //품번D
            tLabel.Items.Add(txtItem_ITDSC10); //수량D
            tLabel.Items.Add(txtItem_ITDSC11); //입고일자D
            tLabel.Items.Add(txtItem_ITDSC12); //제조원D
            tLabel.Items.Add(txtItem_ITDSC13); //시험일D
            tLabel.Items.Add(txtItem_ITDSC14); //참고사항D
            tLabel.Items.Add(txtItem_ITDSC15); //분류D
            tLabel.Items.Add(txtItem_ITDSC20); //보관위치D
            tLabel.Items.Add(txtItem_ITDSC21); //유효기간D
            tLabel.Items.Add(txtItem_ITDSC22); //제조번호D
            tLabel.Items.Add(txtItem_ITDSC23); //시험번호D

            tLabel.Items.Add(ITDSC24); //바코드

            return tLabel;

        }


        //부자재
        private ThermalLabel GenerateBasicThermalLabel_B(SystemCodeVo _vo)
        {
            //Define a ThermalLabel object and set unit to inch and label size
            ThermalLabel tLabel = new ThermalLabel(UnitType.Mm, 100, 80);
            tLabel.GapLength = 0.1;

            TextItem txtItem_ITDSC = new TextItem(4.0, 1.0, 1.0, 1.0, "품명");
            txtItem_ITDSC.Font.Name = this.sFont;
            txtItem_ITDSC.Font.Size = this.fontSize - 3;
            txtItem_ITDSC.Font.Bold = true;
            txtItem_ITDSC.Height = 14;
            txtItem_ITDSC.Width = 20;
            txtItem_ITDSC.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC.TextPadding = new FrameThickness(0, txtItem_ITDSC.Height / 4, 0, 0);
            txtItem_ITDSC.BorderThickness = new FrameThickness(boder);
            //txtItem_MTRL_LOT_NO.BackColor = Color.Black;
            //txtItem_MTRL_LOT_NO.ForeColor = Color.White;
            //txtItem_MTRL_LOT_NO.BorderColor = Color.White;

            TextItem txtItem_ITDSC1 = new TextItem(4.0, 15.0, 2.0, 2.0, "품번");
            txtItem_ITDSC1.Font.Name = this.sFont;
            txtItem_ITDSC1.Font.Size = this.fontSize - 3;
            txtItem_ITDSC1.Font.Bold = true;
            txtItem_ITDSC1.Height = 15;
            txtItem_ITDSC1.Width = 20;
            txtItem_ITDSC1.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC1.TextPadding = new FrameThickness(0, txtItem_ITDSC1.Height / 4, 0, 0);
            txtItem_ITDSC1.BorderThickness = new FrameThickness(boder);

            TextItem txtItem_ITDSC2 = new TextItem(4.0, 30.0, 2.0, 2.0, "수량");
            txtItem_ITDSC2.Font.Name = this.sFont;
            txtItem_ITDSC2.Font.Size = this.fontSize - 3;
            txtItem_ITDSC2.Font.Bold = true;
            txtItem_ITDSC2.Height = 15;
            txtItem_ITDSC2.Width = 20;
            txtItem_ITDSC2.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC2.TextPadding = new FrameThickness(0, txtItem_ITDSC2.Height / 4, 0, 0);
            txtItem_ITDSC2.BorderThickness = new FrameThickness(boder);

            TextItem txtItem_ITDSC3 = new TextItem(4.0, 45.0, 2.0, 2.0, "입고일자");
            txtItem_ITDSC3.Font.Name = this.sFont;
            txtItem_ITDSC3.Font.Size = this.fontSize - 3;
            txtItem_ITDSC3.Font.Bold = true;
            txtItem_ITDSC3.Height = 15;
            txtItem_ITDSC3.Width = 20;
            txtItem_ITDSC3.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC3.TextPadding = new FrameThickness(0, txtItem_ITDSC3.Height / 4, 0, 0);
            txtItem_ITDSC3.BorderThickness = new FrameThickness(boder);

            TextItem txtItem_ITDSC4 = new TextItem(4.0, 60.0, 2.0, 2.0, "배치코드");
            txtItem_ITDSC4.Font.Name = this.sFont;
            txtItem_ITDSC4.Font.Size = this.fontSize - 3;
            txtItem_ITDSC4.Font.Bold = true;
            txtItem_ITDSC4.Height = 16;
            txtItem_ITDSC4.Width = 20;
            txtItem_ITDSC4.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC4.TextPadding = new FrameThickness(0, txtItem_ITDSC4.Height / 4, 0, 0);
            txtItem_ITDSC4.BorderThickness = new FrameThickness(boder);

            TextItem txtItem_ITDSC10 = new TextItem(52.0, 30.0, 2.0, 2.0, "규격");
            txtItem_ITDSC10.Font.Name = this.sFont;
            txtItem_ITDSC10.Font.Size = this.fontSize - 3;
            txtItem_ITDSC10.Font.Bold = true;
            txtItem_ITDSC10.Height = 15;
            txtItem_ITDSC10.Width = 20;
            txtItem_ITDSC10.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC10.TextPadding = new FrameThickness(0, txtItem_ITDSC10.Height / 4, 0, 0);
            txtItem_ITDSC10.BorderThickness = new FrameThickness(boder);

            TextItem txtItem_ITDSC11 = new TextItem(52.0, 45.0, 2.0, 2.0, "유효기간");
            txtItem_ITDSC11.Font.Name = this.sFont;
            txtItem_ITDSC11.Font.Size = this.fontSize - 2;
            txtItem_ITDSC11.Font.Bold = true;
            txtItem_ITDSC11.Height = 15;
            txtItem_ITDSC11.Width = 20;
            txtItem_ITDSC11.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC11.TextPadding = new FrameThickness(0, txtItem_ITDSC11.Height / 4, 0, 0);
            txtItem_ITDSC11.BorderThickness = new FrameThickness(boder);

            TextItem txtItem_ITDSC12 = new TextItem(52.0, 60.0, 2.0, 2.0, "공급업체");
            txtItem_ITDSC12.Font.Name = this.sFont;
            txtItem_ITDSC12.Font.Size = this.fontSize - 2;
            txtItem_ITDSC12.Font.Bold = true;
            txtItem_ITDSC12.Height = 16;
            txtItem_ITDSC12.Width = 20;
            txtItem_ITDSC12.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC12.TextPadding = new FrameThickness(0, txtItem_ITDSC12.Height / 4, 0, 0);
            txtItem_ITDSC12.BorderThickness = new FrameThickness(boder);


            //품명 데이터
            TextItem txtItem_ITDSC5 = new TextItem(24.0, 1.0, 1.0, 1.0, _vo.ITM_CD.ToString());
            txtItem_ITDSC5.Font.Name = this.sFont;
            txtItem_ITDSC5.Font.Size = this.fontSize - 3;
            txtItem_ITDSC5.Font.Bold = true;
            txtItem_ITDSC5.Height = 14;
            txtItem_ITDSC5.Width = 75;
            txtItem_ITDSC5.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC5.TextPadding = new FrameThickness(0, txtItem_ITDSC5.Height / 4, 0, 0);
            txtItem_ITDSC5.BorderThickness = new FrameThickness(boder);

            //품번 데이터
            TextItem txtItem_ITDSC6 = new TextItem(24.0, 15.0, 1.0, 1.0, (string.IsNullOrEmpty(_vo.ITM_NM) ? " " : _vo.ITM_NM.ToString()));
            txtItem_ITDSC6.Font.Name = this.sFont;
            txtItem_ITDSC6.Font.Size = this.fontSize - 3;
            txtItem_ITDSC6.Font.Bold = true;
            txtItem_ITDSC6.Height = 15;
            txtItem_ITDSC6.Width = 75;
            txtItem_ITDSC6.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC6.TextPadding = new FrameThickness(0, txtItem_ITDSC6.Height / 4, 0, 0);
            txtItem_ITDSC6.BorderThickness = new FrameThickness(boder);

            //수량 데이터
            TextItem txtItem_ITDSC7 = new TextItem(24.0, 30.0, 2.0, 2.0, " " );
            txtItem_ITDSC7.Font.Name = this.sFont;
            txtItem_ITDSC7.Font.Size = this.fontSize - 3;
            txtItem_ITDSC7.Font.Bold = true;
            txtItem_ITDSC7.Height = 15;
            txtItem_ITDSC7.Width = 28;
            txtItem_ITDSC7.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC7.TextPadding = new FrameThickness(0, txtItem_ITDSC7.Height / 4, 0, 0);
            txtItem_ITDSC7.BorderThickness = new FrameThickness(boder);


            //입고일자 데이터
            TextItem txtItem_ITDSC8 = new TextItem(24.0, 45.0, 2.0, 2.0, " " );
            txtItem_ITDSC8.Font.Name = this.sFont;
            txtItem_ITDSC8.Font.Size = this.fontSize - 3;
            txtItem_ITDSC8.Font.Bold = true;
            txtItem_ITDSC8.Height = 15;
            txtItem_ITDSC8.Width = 28;
            txtItem_ITDSC8.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC8.TextPadding = new FrameThickness(0, txtItem_ITDSC8.Height / 4, 0, 0);
            txtItem_ITDSC8.BorderThickness = new FrameThickness(boder);

            //배치코드 데이터
            TextItem txtItem_ITDSC9 = new TextItem(24.0, 60.0, 2.0, 2.0, " " );
            txtItem_ITDSC9.Font.Name = this.sFont;
            txtItem_ITDSC9.Font.Size = this.fontSize - 3;
            txtItem_ITDSC9.Font.Bold = true;
            txtItem_ITDSC9.Height = 16;
            txtItem_ITDSC9.Width = 28;
            txtItem_ITDSC9.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC9.TextPadding = new FrameThickness(0, txtItem_ITDSC9.Height / 4, 0, 0);
            txtItem_ITDSC9.BorderThickness = new FrameThickness(boder);

            //규격 데이터
            TextItem txtItem_ITDSC13 = new TextItem(72.0, 30.0, 2.0, 2.0, (string.IsNullOrEmpty(_vo.ITM_SZ_NM) ? " " : _vo.ITM_SZ_NM.ToString()));
            txtItem_ITDSC13.Font.Name = this.sFont;
            txtItem_ITDSC13.Font.Size = this.fontSize - 2;
            txtItem_ITDSC13.Font.Bold = true;
            txtItem_ITDSC13.Height = 15;
            txtItem_ITDSC13.Width = 27;
            txtItem_ITDSC13.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC13.TextPadding = new FrameThickness(0, txtItem_ITDSC13.Height / 4, 0, 0);
            txtItem_ITDSC13.BorderThickness = new FrameThickness(boder);

            //유효기간 데이터
            TextItem txtItem_ITDSC14 = new TextItem(72.0, 45.0, 2.0, 2.0, " ");
            txtItem_ITDSC14.Font.Name = this.sFont;
            txtItem_ITDSC14.Font.Size = this.fontSize - 2;
            txtItem_ITDSC14.Font.Bold = true;
            txtItem_ITDSC14.Height = 15;
            txtItem_ITDSC14.Width = 27;
            txtItem_ITDSC14.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC14.TextPadding = new FrameThickness(0, txtItem_ITDSC14.Height / 4, 0, 0);
            txtItem_ITDSC14.BorderThickness = new FrameThickness(boder);

            //공급업체 데이터
            TextItem txtItem_ITDSC15 = new TextItem(72.0, 60.0, 2.0, 2.0, " " );
            txtItem_ITDSC15.Font.Name = this.sFont;
            txtItem_ITDSC15.Font.Size = this.fontSize - 6;
            txtItem_ITDSC15.Font.Bold = true;
            txtItem_ITDSC15.Height = 16;
            txtItem_ITDSC15.Width = 27;
            txtItem_ITDSC15.TextAlignment = Neodynamic.SDK.Printing.TextAlignment.Center;
            txtItem_ITDSC15.TextPadding = new FrameThickness(0, txtItem_ITDSC15.Height / 4, 0, 0);
            txtItem_ITDSC15.BorderThickness = new FrameThickness(boder);

            //바코드
            BarcodeItem ITDSC16 = new BarcodeItem(82, 15.5, 2.0, 2.0, BarcodeSymbology.QRCode, _vo.ITM_CD.ToString());
            //ITDSC16.DataMatrixModuleSize = 0.70;
            ITDSC16.QRCodeModuleSize = 0.7;
            ITDSC16.QRCodeEncoding = QRCodeEncoding.Byte;

            tLabel.Items.Add(txtItem_ITDSC);  //품명
            tLabel.Items.Add(txtItem_ITDSC1); //품번
            tLabel.Items.Add(txtItem_ITDSC2); //수량
            tLabel.Items.Add(txtItem_ITDSC3); //입고일자
            tLabel.Items.Add(txtItem_ITDSC4); //배치코드
            tLabel.Items.Add(txtItem_ITDSC10); //규격
            tLabel.Items.Add(txtItem_ITDSC11); //유효기간
            tLabel.Items.Add(txtItem_ITDSC12); //공급업체

            tLabel.Items.Add(txtItem_ITDSC5); //품명 컬럼
            tLabel.Items.Add(txtItem_ITDSC6); //품번 컬럼
            tLabel.Items.Add(txtItem_ITDSC7); //수량 컬럼
            tLabel.Items.Add(txtItem_ITDSC8); //입고일자 컬럼
            tLabel.Items.Add(txtItem_ITDSC9); //배치코드 컬럼
            tLabel.Items.Add(txtItem_ITDSC13); //규격 컬럼
            tLabel.Items.Add(txtItem_ITDSC14); //유효기간 컬럼
            tLabel.Items.Add(txtItem_ITDSC15); //공급업체 컬럼
            tLabel.Items.Add(ITDSC16);  //바코드
            //tLabel.DataSource = Vo;

            return tLabel;
        }





    }
}