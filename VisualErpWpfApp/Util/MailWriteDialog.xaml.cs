using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Code;
using Newtonsoft.Json;
using Popbill;
using Popbill.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Windows;


namespace AquilaErpWpfApp3.Util
{
    /// <summary>
    /// S136MailWriteDialog.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MailWriteDialog : DXWindow
    {
        private string title = "메시지 쓰기 CS";

        // 연동신청시 발급받은 링크아이디로 수정.
        private string LinkID = "IEGKR";

        // 연동신청시 발급받은 비밀키로 수정.
        private string SecretKey = "sw6Z8bchID7OirlGE0cIyr4SfdZw0axN/MP6mRcaJ/I=";

        // 문자 서비스 객체 선언
        private MessageService messageService;

        public MailWriteDialog()
        {
            InitializeComponent();


            // 문자 서비스 객체 초기화
            messageService = new MessageService(LinkID, SecretKey);

            // 연동환경 설정값, true - 개발용(테스트베드), false - 상업용(실서비스)
            messageService.IsTest = true;

            // 발급된 토큰에 대한 IP 제한기능 사용여부, 권장(True)
            messageService.IPRestrictOnOff = true;

            // 로컬PC 시간 사용 여부 true(사용), false(기본값) - 미사용
            messageService.UseLocalTimeYN = false;

            Refresh();

            this.SendButton.Click += new RoutedEventHandler(Button_Click);
        }

        public async void Refresh()
        {
            try
            {
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s1134/snd/usr", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SendUsrSelectList.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                        this.SendUsrSelectList.SelectedItems = new List<SystemCodeVo>();


                    }
                }
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrEmpty(SendUsrSelectList.SelectedItem.ToString()))
            {
                WinUIMessageBox.Show("메세지를 보낼 유저를 선택해주세요", title, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }


            // 팝빌회원 사업자번호(하이픈('-') 제외)
            string corpNum = "2308108091";

            // 팝빌회원 아이디
            string userID = "Ieg123";

            // 발신번호(사이트에서 사전번호 등록해야함)
            string senderNum = "01049017846";

            // 수신번호
            string receiver = "";

            // 수신자명
            string receiverName = SystemProperties.USER_VO.USR_N1ST_NM;


            // 메시지내용, 단문(SMS) 메시지는 90byte초과된 내용은 삭제되어 전송됨.
            //string contents = "단문 문자 메시지 내용. 90byte 초과시 삭제되어 전송";
            string contents = text_TIT_MSG.Text;

            // 전송요청번호, 파트너가 전송요청에 대한 관리번호를 직접 할당하여 관리하는 경우 기재
            // 최대 36자리, 영문, 숫자, 언더바('_'), 하이픈('-')을 조합하여 사업자별로 중복되지 않도록 구성

            string requestNum = "";


            // 전송예약일시 (null : 즉시전송)
            DateTime? reserveDT = null;

            // 광고문자여부 (기본값 false)
            Boolean adsYN = false;
            try
            {
                // SendSMS : 문자보내기 함수
                //string receiptNum = messageService.SendSMS(corpNum, senderNum, receiver,
                //   receiverName, contents, reserveDT, userID, requestNum, adsYN);

                //MessageBox.Show("접수번호 : " + receiptNum, "단문(SMS) 전송");

                List<SystemCodeVo> SendMailList = new List<SystemCodeVo>();
                SendMailList.Add(new SystemCodeVo() { TIT_MSG = text_TIT_MSG.Text, WRITE_ID = SystemProperties.USER, CHNL_CD = SystemProperties.USER_VO.CHNL_CD });

                foreach(SystemCodeVo item in SendUsrSelectList.SelectedItems as List<SystemCodeVo>)
                {
                    item.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
                    item.LGIN_ID = item.USR_ID;

                    receiver = item.CELL_PHN_NO;

                    //WinUIMessageBox.Show(receiver, title, MessageBoxButton.OK, MessageBoxImage.Information);

                    string receiptNum = messageService.SendSMS(corpNum, senderNum, receiver,
                       receiverName, contents, reserveDT, userID, requestNum, adsYN);

                    SendMailList.Add(item);
                }

                int _Num = 0;
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s1134/msg/i", new StringContent(JsonConvert.SerializeObject(SendMailList), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();
                        if (int.TryParse(result, out _Num) == false)
                        {
                            //실패
                            WinUIMessageBox.Show(result, title, MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        //성공
                        WinUIMessageBox.Show("완료 되었습니다", title, MessageBoxButton.OK, MessageBoxImage.Information);
                        text_TIT_MSG.Text = "";
                    }
                }
            }
            catch (PopbillException ex)
            {
                MessageBox.Show("응답코드(code) : " + ex.code.ToString() + "\r\n" +
                                "응답메시지(message) : " + ex.Message, "단문(SMS) 전송");
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
          
        }
    }

}
