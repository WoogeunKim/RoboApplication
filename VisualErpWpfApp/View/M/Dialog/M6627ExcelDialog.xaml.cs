using DevExpress.Spreadsheet;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Printing;
using DevExpress.Xpf.WindowsUI;
using System;
using System.IO;
using System.Windows;


namespace AquilaErpWpfApp3.View.M.Dialog
{
    public partial class M6627ExcelDialog : DXWindow
    {
        //private static FproofServiceClient fproofClient = SystemProperties.FproofClient;

        private string FULL_FILE_PATH;

        public M6627ExcelDialog(string _FULL_FILE_PATH)
        {
            InitializeComponent();

            this.FULL_FILE_PATH = _FULL_FILE_PATH;


            this.btn_close.Click += btn_close_Click;
            //this.btn_saveAs.Click += btn_saveAs_Click;
            this.btn_print.Click += btn_print_Click;
            this.btn_save.Click += btn_save_Click;

            IWorkbook workbook = spreadsheetControl.Document;
            using (FileStream stream = new FileStream(_FULL_FILE_PATH, FileMode.Open))
            {
                workbook.LoadDocument(stream, DocumentFormat.OpenXml);
            }
        }

        void btn_save_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = WinUIMessageBox.Show("저장 하시겠습니까?", "[파일 저장]엑셀", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                //임시 파일 저장
                IWorkbook workbook = spreadsheetControl.Document;
                using (FileStream stream = new FileStream(FULL_FILE_PATH, FileMode.Create, FileAccess.ReadWrite))
                {
                    workbook.SaveDocument(stream, DocumentFormat.OpenXml);
                }

                this.IMAGE = FileToByteArray(FULL_FILE_PATH);

                this.DialogResult = true;
                this.Close();
            }
        }

        void btn_print_Click(object sender, RoutedEventArgs e)
        {
            Worksheet worksheet = this.spreadsheetControl.ActiveWorksheet;

            worksheet.ActiveView.Orientation = PageOrientation.Landscape;
            worksheet.ActiveView.ShowHeadings = true;
            worksheet.ActiveView.PaperKind = System.Drawing.Printing.PaperKind.A4;
            WorksheetPrintOptions printOptions = worksheet.PrintOptions;
            printOptions.BlackAndWhite = true;
            printOptions.PrintGridlines = false;
            printOptions.FitToPage = true;
            printOptions.FitToWidth = 2;
            printOptions.ErrorsPrintMode = ErrorsPrintMode.Dash;

            using (LegacyPrintableComponentLink link = new LegacyPrintableComponentLink(this.spreadsheetControl.ActiveWorksheet.Workbook))
            {
                //link.PrintingSystem.Watermark.Text = Properties.Settings.Default.SettingChnl;
                //link.PrintingSystem.Watermark.TextDirection = DirectionMode.ForwardDiagonal;
                //link.PrintingSystem.Watermark.Font = new Font(link.PrintingSystem.Watermark.Font.FontFamily, 40);
                //link.PrintingSystem.Watermark.ForeColor = System.Drawing.Color.PaleTurquoise;
                //link.PrintingSystem.Watermark.TextTransparency = 150;

                link.CreateDocument();
                link.ShowPrintPreviewDialog(Application.Current.MainWindow);
            }
        }

        //void btn_saveAs_Click(object sender, RoutedEventArgs e)
        //{
        //    FproofVo Dao = this.orgVo;
        //    if (Dao == null)
        //    {
        //        return;
        //    }

        //    if (Dao != null)
        //    {
        //        System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
        //        dialog.ShowNewFolderButton = true;
        //        dialog.Description = "[" + Dao.PROD_EQ_FILE_NM + " 파일] 저장 폴더를 선택 해 주세요.";
        //        dialog.RootFolder = Environment.SpecialFolder.Desktop;
        //        dialog.ShowDialog();
        //        if (!string.IsNullOrEmpty(dialog.SelectedPath))
        //        {
        //            if (!string.IsNullOrEmpty(Dao.PROD_EQ_FILE_NM))
        //            {
        //                int ArraySize = Dao.PROD_EQ_IMG.Length;
        //                FileStream fs = new FileStream(dialog.SelectedPath + "/" + Dao.PROD_EQ_FILE_NM, FileMode.OpenOrCreate, FileAccess.Write);
        //                fs.Write(Dao.PROD_EQ_IMG, 0, ArraySize);
        //                fs.Close();
        //            }
        //            System.Diagnostics.Process.Start(dialog.SelectedPath);
        //        }
        //    }
        //}


        private byte[] FileToByteArray(string filePath)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                int length = Convert.ToInt32(fs.Length);
                BinaryReader br = new BinaryReader(fs);
                byte[] buff = br.ReadBytes(length);
                fs.Close();

                return buff;
            }
        }

        void btn_close_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }


        public byte[] IMAGE
        {
            get;
            set;
        }
    }

}
