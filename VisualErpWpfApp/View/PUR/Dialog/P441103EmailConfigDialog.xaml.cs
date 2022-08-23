using AquilaErpWpfApp3.Util;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace AquilaErpWpfApp3.View.PUR.Dialog
{
    public partial class P441103EmailConfigDialog : DXWindow
    {
        //private static PurServiceClient purClient = SystemProperties.PurClient;
        //private PurVo orgDao;
        //private bool isEdit = false;
        //private PurVo updateDao;

        private string _title = "부자재 발주서 등록 - E-mail 설정";

        public P441103EmailConfigDialog()
        {
            InitializeComponent();

            //SYSTEM_CODE_VO();

            //this.orgDao = Dao;

            this.text_E_MAIL.Text = Properties.Settings.Default.str_email_id;
            this.text_Pass.Text = Properties.Settings.Default.str_email_password;
            this.text_Stmp.Text = Properties.Settings.Default.str_email_stmp;


            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
            this.OKButton.Click += new RoutedEventHandler(OKButton_Click);
            this.CancelButton.Click += new RoutedEventHandler(CancelButton_Click);

            //
            this.TestButton.Click += new RoutedEventHandler(TestButton_Click);
        }

        private void TestButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (SmtpClient smtp = new SmtpClient(this.text_Stmp.Text, 25))
                {
                    //주소설정
                    smtp.TargetName = this.text_Stmp.Text;
                    smtp.Host = this.text_Stmp.Text;
                    smtp.Port = 25;
                    smtp.EnableSsl = false;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                    smtp.UseDefaultCredentials = true;

                    NetworkCredential basicAuthInfo = new NetworkCredential(this.text_E_MAIL.Text, this.text_Pass.Text);
                    smtp.Credentials = basicAuthInfo;
                    //smtp.Credentials = new NetworkCredential("goyh32", "qwer_1234");
                    //smtp.DeliveryMethod = SmtpDeliveryMethod.Network;


                    using (MailMessage mail = new MailMessage())
                    {
                        //보내는 사람
                        mail.From = new MailAddress(this.text_E_MAIL.Text);
                        //mail.From = new MailAddress(SystemProperties.USERVO.EML_ID);

                        //받는 사람
                        mail.To.Add(this.text_E_MAIL.Text);
                        //mail.To.Add(this.text_To.Text);

                        mail.SubjectEncoding = Encoding.UTF8;
                        //mail.Subject = this.text_Title.Text;
                        mail.Subject = "테스트 메일 입니다.";

                        mail.BodyEncoding = Encoding.UTF8;
                        mail.Body = "테스트 설정이 성공적으로 되었습니다";
                        //mail.Body = this.text_Contents.Text;

                        mail.IsBodyHtml = false;

                        //첨부파일
                        //Attachment Attachment = new Attachment(this.EXCEL_FILE);
                        //발주서
                        //mail.Attachments.Add(new Attachment(this.PDF_FILE));
                        ////엑셀
                        //mail.Attachments.Add(new Attachment(this.EXCEL_FILE));

                        smtp.Send(mail);
                    }
                }

                WinUIMessageBox.Show("[" + this.text_E_MAIL.Text + "] 메일이 전송 되었습니다", _title, MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }



}

        #region Functon (OKButton_Click, CancelButton_Click)
        private async void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValueCheckd())
            {
                Properties.Settings.Default.str_email_id = this.text_E_MAIL.Text;
                Properties.Settings.Default.str_email_password = this.text_Pass.Text;
                Properties.Settings.Default.str_email_stmp = this.text_Stmp.Text;

                Properties.Settings.Default.Save();

                WinUIMessageBox.Show("완료 되었습니다", _title, MessageBoxButton.OK, MessageBoxImage.Information);


                this.DialogResult = true;
                this.Close();
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
            if (string.IsNullOrEmpty(this.text_E_MAIL.Text))
            {
                WinUIMessageBox.Show("[E-mail] 입력 값이 맞지 않습니다.", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.text_E_MAIL.IsTabStop = true;
                this.text_E_MAIL.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(this.text_Pass.Text))
            {
                WinUIMessageBox.Show("[Password] 입력 값이 맞지 않습니다.", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.text_Pass.IsTabStop = true;
                this.text_Pass.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(this.text_Stmp.Text))
            {
                WinUIMessageBox.Show("[STMP] 입력 값이 맞지 않습니다.", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.text_Stmp.IsTabStop = true;
                this.text_Stmp.Focus();
                return false;
            }
            return true;
        }
        #endregion

    }
}
