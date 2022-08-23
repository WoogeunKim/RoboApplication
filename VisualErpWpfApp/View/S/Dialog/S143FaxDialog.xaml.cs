using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using AquilaErpWpfApp3.Util;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using Microsoft.Win32;
using ModelsLibrary.Code;
using Popbill;
using Popbill.Fax;

namespace AquilaErpWpfApp3.View.S.Dialog
{
    /// <summary>
    /// S143FaxDialog.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class S143FaxDialog : DXWindow
    {
        private string title = "팩스 전송";


        private FaxService faxService;
        // 연동신청 아이디
        private string LinkID = "IEGKR";
        // 연동신청시 발급받은 비밀키
        private string SecretKey = "sw6Z8bchID7OirlGE0cIyr4SfdZw0axN/MP6mRcaJ/I=";
        // 파일위치 원본 담을 변수 / text 에는 파일명만 추출
        private string fileLoc;

        public S143FaxDialog(SystemCodeVo Dao)
        {
            InitializeComponent();

            faxService = new FaxService(LinkID, SecretKey);
            // true - 개발자  false - 상업용
            faxService.IsTest = true;
            // 발급된 토큰에 대한 IP 제한기능 사용여부, 권장(True)
            faxService.IPRestrictOnOff = true;
            // 로컬PC 시간 사용 여부 true(사용), false(기본값) - 미사용
            faxService.UseLocalTimeYN = false;

            //거래처(받는사람)
            text_ReceiverNM.Text = "[" + Dao.CO_NO + "] " + Dao.CO_NM;
            text_ReceiverNum.Text = "070-4275-5084";

            //보내는사람
            text_SenderNum.Text = "02-6442-9700";

            this.btn_File.Click += Btn_File_Click;
            this.OKButton.Click += new RoutedEventHandler(OKButton_Click);
            this.CancelButton.Click += new RoutedEventHandler(CancelButton_Click);
        }

        private void Btn_File_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog fileShow = new OpenFileDialog();
                if (fileShow.ShowDialog(this) == true)
                {
                    // 파일명 + 파일 확장자
                    fileLoc = fileShow.FileName;
                    text_File.Text = System.IO.Path.GetFileName(fileLoc);
                }
            }
            catch (Exception eLog)
            {
                WinUIMessageBox.Show("파일이 아닙니다", SystemProperties.PROGRAM_TITLE, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }


        }

        #region OKButton_Click
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (text_ReceiverNum.Text == null) return;
            else if (text_File.Text == null) return;

            // 회원 사업자번호
            String corpNum = "2308108091";

            // 회원 아이디
            String userID = "Ieg123";

            // 발신번호
            String senderNum = Regex.Replace(text_SenderNum.Text, @"\D", "");

            // String receiverNum = "07042755084"; //024230905
            String receiverNum = Regex.Replace(text_ReceiverNum.Text, @"\D", "");

            String receiverName = text_ReceiverNM.Text;

            // 광고팩스 전송여부
            bool adsYN = false;

            // 팩스제목
            String faxTitle = text_Title.Text;

            String requestNum = "";

            DateTime? reserveDT = null;

            String strFileName = fileLoc;

            try
            {
                String receipthNum = faxService.SendFAX(corpNum, senderNum, receiverNum, receiverName, strFileName, reserveDT, userID, adsYN, faxTitle, requestNum);
                MessageBox.Show("접수번호 : " + receipthNum, "팩스 전송");

                this.DialogResult = false;
                this.Close();
            }
            catch (PopbillException ex)
            {
                MessageBox.Show("음답코드 :" + ex.code.ToString() + "\r\n" + "응답메시지 : " + ex.Message, "팩스전송");
            }
        }
        #endregion

        #region CancelButton_Click
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        } 
        #endregion





    }
}
