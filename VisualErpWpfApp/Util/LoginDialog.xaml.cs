using DevExpress.Xpf.WindowsUI;
using Microsoft.Win32;
using ModelsLibrary.Auth;
using ModelsLibrary.Code;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AquilaErpWpfApp3.Util
{
    public partial class LoginDialog : Window
    {
        //private static AuthorServiceClient authClient = new AuthorServiceClient();

        public LoginDialog()
        {
            InitializeComponent();


            this.Title = SystemProperties.PROGRAM_TITLE + " - " + SystemProperties.ProgramVersion;
            this.text_loginNm.Text = this.Title;
            this.text_CoNm.Text = Properties.Settings.Default.SettingCompany;

            //main_img check
            //if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + Properties.Settings.Default.SettingLogImg) == true)
            if (string.IsNullOrEmpty(Properties.Settings.Default.SettingLogImg) == false)
            {
                try
                {
                    //byte[] binaryData = Convert.FromBase64String(Properties.Settings.Default.SettingLogImg);

                    //BitmapImage biImg = new BitmapImage();
                    //MemoryStream ms = new MemoryStream(binaryData);
                    //biImg.BeginInit();
                    //biImg.StreamSource = ms;
                    //biImg.EndInit();
                    //
                    //this.img_main.Source = biImg;
                    //this.img_main.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + Properties.Settings.Default.SettingLogImg, UriKind.Absolute));

                    this.img_main.Source = ByteToImage(Convert.FromBase64String(Properties.Settings.Default.SettingLogImg));
                }
                catch {  }
            }

            this.txtId.Text = Properties.Settings.Default.SettingId;
  
            this.btn_login.IsEnabled = !string.IsNullOrEmpty(Properties.Settings.Default.SettingChnl);
            this.img_setting.IsEnabled = !string.IsNullOrEmpty(Properties.Settings.Default.SettingChnl);
            this.img_main_setting.IsEnabled = !string.IsNullOrEmpty(Properties.Settings.Default.SettingChnl);
            this.txtId.IsEnabled = !string.IsNullOrEmpty(Properties.Settings.Default.SettingChnl);
            this.txtPass.IsEnabled = !string.IsNullOrEmpty(Properties.Settings.Default.SettingChnl);


            this.img_config.MouseDown += img_config_MouseDown;
            this.img_setting.MouseDown += img_setting_MouseDown;
            this.img_close.MouseDown += img_close_MouseDown;
            this.img_main_setting.MouseDown += Img_main_setting_MouseDown;

            this.Closing += new System.ComponentModel.CancelEventHandler(_Closing);
            this.btn_login.Click += new RoutedEventHandler(btn_login_Click);
            this.txtPass.KeyDown += new KeyEventHandler(_KeyDown);
            this.txtId.KeyDown += new KeyEventHandler(_KeyDown);

            this.titleBar.MouseDown += titleBar_MouseDown;

            //
            //this.combo_CHNL_NM.Text = (string.IsNullOrEmpty(Properties.Settings.Default.SettingChnl) ? "" : Properties.Settings.Default.SettingChnl);
            this.txtId.Text = (string.IsNullOrEmpty(Properties.Settings.Default.SettingId) ? "" : Properties.Settings.Default.SettingId);

            this.txtId.Focus();
        }

        //Main Img
        private async void Img_main_setting_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Title = "이미지 파일";
                ofd.FileName = "";
                //ofd.Filter = "Image File|*.jpg;*jpeg;*.png;*.gif;*.bmp";
                ofd.Filter = "Image File|*.png";

                //OK버튼 클릭시
                if (ofd.ShowDialog() == true)
                {
                    ProgramVo chnlVo = new ProgramVo();
                    chnlVo.CHNL_CD = Properties.Settings.Default.SettingChnl;
                    chnlVo.DELT_FLG = "N";
                    chnlVo.UPD_USR_ID = "MIG_USR";
                    chnlVo.CHNL_LOG_IMG = File.ReadAllBytes(ofd.FileName);

                    if (chnlVo.CHNL_LOG_IMG.Length > 1050000)
                    {
                        WinUIMessageBox.Show("1MB 파일 크기가 초과 하였습니다.", SystemProperties.PROGRAM_TITLE, MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }


                    HttpClient client;
                    HttpResponseMessage response;

                    ////URL Check & CHNL Check
                    using (client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(Properties.Settings.Default.SettingHttp);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        ////URL Check & CHNL Check
                        int _Num = 0;
                        using (response = await client.PostAsync("s136/chnl/u", new StringContent(JsonConvert.SerializeObject(chnlVo), System.Text.Encoding.UTF8, "application/json")))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                string result = await response.Content.ReadAsStringAsync();
                                if (int.TryParse(result, out _Num) == false)
                                {
                                    //실패
                                    WinUIMessageBox.Show(result, SystemProperties.PROGRAM_TITLE, MessageBoxButton.OK, MessageBoxImage.Error);
                                    return;
                                }

                                //Main Img Save
                                if (chnlVo.CHNL_LOG_IMG?.Length > 0)
                                {
                                    //if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory) == true)
                                    //{
                                        //Properties.Settings.Default.SettingLogImg = Guid.NewGuid().ToString() + ".png";
                                        //Properties.Settings.Default.Save();
                                        //
                                        //File.WriteAllBytes(AppDomain.CurrentDomain.BaseDirectory + Properties.Settings.Default.SettingLogImg, chnlVo.CHNL_LOG_IMG);
                                        //this.img_main.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + Properties.Settings.Default.SettingLogImg, UriKind.Absolute));

                                        Properties.Settings.Default.SettingLogImg = Convert.ToBase64String(chnlVo.CHNL_LOG_IMG);
                                        Properties.Settings.Default.Save();
                                    //
                                    //File.WriteAllBytes(AppDomain.CurrentDomain.BaseDirectory + Properties.Settings.Default.SettingLogImg, chnlVo.CHNL_LOG_IMG);
                                    //this.img_main.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + Properties.Settings.Default.SettingLogImg, UriKind.Absolute));


                                    //byte[] binaryData = Convert.FromBase64String(Properties.Settings.Default.SettingLogImg);

                                    if (string.IsNullOrEmpty(Properties.Settings.Default.SettingLogImg) == false)
                                    {
                                        //
                                        this.img_main.Source = ByteToImage(Convert.FromBase64String(Properties.Settings.Default.SettingLogImg));
                                        //
                                    }
                                   //}
                                }

                                //성공
                                WinUIMessageBox.Show("등록 되었습니다", SystemProperties.PROGRAM_TITLE, MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                        }
                    }
                }
            }
            catch (Exception eLog)
            {
                WinUIMessageBox.Show("이미지 파일이 맞지 않습니다", SystemProperties.PROGRAM_TITLE, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
        }

        //채널
        async void img_config_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Title = "설정 파일";
                ofd.FileName = "";
                ofd.Filter = "Config File|*.cpc";

                //OK버튼 클릭시
                if (ofd.ShowDialog() == true)
                {
                    string key = "cpcn.co.kr";
                    string[] lines = System.IO.File.ReadAllLines(ofd.FileName);
                    string url = Decrypt(lines[0], key);
                    string chnl = Decrypt(lines[1], key);
                    string ver = Decrypt(lines[2], key);

                    //해당 ERP 인증서 버전 체크  => F5BD540D-8B27-4E89-9965-53F85AE74388
                    if ("F5BD540D-8B27-4E89-9965-53F85AE74388".Equals(ver) == false)
                    {
                        WinUIMessageBox.Show("해당 CPC 파일은 버전이 맞지 않습니다", SystemProperties.PROGRAM_TITLE, MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    HttpClient client;
                    HttpResponseMessage response;
                    ProgramVo chnlVo;

                    ////URL Check & CHNL Check
                    using (client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(url);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        response = await client.GetAsync("s136/chnl/" + chnl);
                        if (response.IsSuccessStatusCode)
                        {
                            chnlVo = JsonConvert.DeserializeObject<ProgramVo>(await response.Content.ReadAsStringAsync());
                            Properties.Settings.Default.SettingChnl = chnlVo.CHNL_CD;
                            Properties.Settings.Default.SettingCompany = chnlVo.CHNL_NM;
                            Properties.Settings.Default.SettingHttp = url;


                            Properties.Settings.Default.SettingLogImg = (chnlVo.CHNL_LOG_IMG == null ? null : Convert.ToBase64String(chnlVo.CHNL_LOG_IMG));
                            Properties.Settings.Default.Save();

                            //#if DEBUG TEST Server
                            //Properties.Settings.Default.SettingHttp = "http://localhost:44349/";
                            //Properties.Settings.Default.SettingCompany = "Local Test Server...";
                            //#endif

                            //Properties.Settings.Default.Save();

                            //Log Img Save
                            if (chnlVo.CHNL_LOG_IMG?.Length > 0)
                            {
                                //if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory) == true)
                                //{
                                //Properties.Settings.Default.SettingLogImg = Guid.NewGuid().ToString() + ".png";
                                //Properties.Settings.Default.SettingLogImg = Convert.ToBase64String(chnlVo.CHNL_LOG_IMG);
                                //Properties.Settings.Default.Save();
                                //
                                //File.WriteAllBytes(AppDomain.CurrentDomain.BaseDirectory + Properties.Settings.Default.SettingLogImg, chnlVo.CHNL_LOG_IMG);
                                //this.img_main.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + Properties.Settings.Default.SettingLogImg, UriKind.Absolute));


                                //byte[] binaryData = Convert.FromBase64String(Properties.Settings.Default.SettingLogImg);

                                //BitmapImage bi = new BitmapImage();
                                //bi.BeginInit();
                                //using (var stream = new MemoryStream(binaryData))
                                //{
                                //    bi.StreamSource = stream;
                                //}
                                //bi.EndInit();
                                //this.img_main.Source = bi;
                                //if (string.IsNullOrEmpty(Properties.Settings.Default.SettingLogImg) == false)
                                //{
                                    this.img_main.Source = ByteToImage(Convert.FromBase64String(Properties.Settings.Default.SettingLogImg));
                                //}
                                //}
                            }
                            else
                            {
                                Properties.Settings.Default.SettingLogImg = string.Empty;
                                Properties.Settings.Default.Save();

                                this.img_main.Source = null;
                            }

                            //Sign Img Save
                            if (chnlVo.CHNL_SIGN_IMG?.Length > 0)
                            {
                                if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory) == true)
                                {
                                    Properties.Settings.Default.SettingSignImg = Guid.NewGuid().ToString() + ".png";
                                    Properties.Settings.Default.Save();
                                    //
                                    File.WriteAllBytes(AppDomain.CurrentDomain.BaseDirectory + Properties.Settings.Default.SettingSignImg, chnlVo.CHNL_SIGN_IMG);
                                    //this.img_main.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + Properties.Settings.Default.SettingSignImg, UriKind.Absolute));
                                }
                            }
                            else
                            {
                                Properties.Settings.Default.SettingSignImg = string.Empty;
                                Properties.Settings.Default.Save();
                            }

                            //
                            //this.btn_login.IsEnabled = !string.IsNullOrEmpty(Properties.Settings.Default.SettingChnl);
                            ////this.img_setting.IsEnabled = !string.IsNullOrEmpty(Properties.Settings.Default.SettingChnl);
                            //this.txtId.IsEnabled = !string.IsNullOrEmpty(Properties.Settings.Default.SettingChnl);
                            //this.txtPass.IsEnabled = !string.IsNullOrEmpty(Properties.Settings.Default.SettingChnl);

                            //SystemProperties.PROGRAM_HTTP = client;

                            response.Dispose();

                            //활성화
                            this.txtId.Text = Properties.Settings.Default.SettingId;
                            this.text_CoNm.Text = Properties.Settings.Default.SettingCompany;

                            this.btn_login.IsEnabled = !string.IsNullOrEmpty(Properties.Settings.Default.SettingChnl);
                            this.img_setting.IsEnabled = !string.IsNullOrEmpty(Properties.Settings.Default.SettingChnl);
                            this.img_main_setting.IsEnabled = !string.IsNullOrEmpty(Properties.Settings.Default.SettingChnl);
                            this.txtId.IsEnabled = !string.IsNullOrEmpty(Properties.Settings.Default.SettingChnl);
                            this.txtPass.IsEnabled = !string.IsNullOrEmpty(Properties.Settings.Default.SettingChnl);

                        }
                    }
                }
            }
            catch (Exception eLog)
            {
                //WinUIMessageBox.Show(eLog.Message, SystemProperties.PROGRAM_TITLE, MessageBoxButton.OK, MessageBoxImage.Warning);
                WinUIMessageBox.Show("해당 CPC 파일은 버전이 맞지 않습니다", SystemProperties.PROGRAM_TITLE, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
        }

        void img_close_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (SystemProperties.isLOGIN)
            {
                return;
            }
            else
            {
                //MessageBoxResult result = WinUIMessageBox.Show("프로그램을 종료 하시겠습니까? ", "[" + SystemProperties.PROGRAM_TITLE + "]프로그램 종료", MessageBoxButton.YesNo, MessageBoxImage.Question);
                //if (result == MessageBoxResult.Yes)
                {
                    Environment.Exit(0);
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                    this.Close();
                }
                //else if (result == MessageBoxResult.No)
                //{
                //    return;
                //}
            }
        }

        //리포트 Img
        async void img_setting_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Title = "이미지 파일";
                ofd.FileName = "";
                ofd.Filter = "Image File|*.png";
                //ofd.Filter = "Image File|*.jpg;*jpeg;*.png;*.gif;*.bmp";

                //OK버튼 클릭시
                if (ofd.ShowDialog() == true)
                {
                    ProgramVo chnlVo = new ProgramVo();
                    chnlVo.CHNL_CD = Properties.Settings.Default.SettingChnl;
                    chnlVo.DELT_FLG = "N";
                    chnlVo.UPD_USR_ID = "MIG_USR";
                    chnlVo.CHNL_SIGN_IMG = File.ReadAllBytes(ofd.FileName);

                    if (chnlVo.CHNL_SIGN_IMG?.Length > 1050000)
                    {
                        WinUIMessageBox.Show("1MB 파일 크기가 초과 하였습니다.", SystemProperties.PROGRAM_TITLE, MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    HttpClient client;
                    HttpResponseMessage response;

                    ////URL Check & CHNL Check
                    using (client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(Properties.Settings.Default.SettingHttp);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        ////URL Check & CHNL Check
                        int _Num = 0;
                        using (response = await client.PostAsync("s136/chnl/u", new StringContent(JsonConvert.SerializeObject(chnlVo), System.Text.Encoding.UTF8, "application/json")))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                string result = await response.Content.ReadAsStringAsync();
                                if (int.TryParse(result, out _Num) == false)
                                {
                                    //실패
                                    WinUIMessageBox.Show(result, SystemProperties.PROGRAM_TITLE, MessageBoxButton.OK, MessageBoxImage.Error);
                                    return;
                                }

                                //Sign Img Save
                                if (chnlVo.CHNL_SIGN_IMG?.Length > 0)
                                {
                                    if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory) == true)
                                    {
                                        Properties.Settings.Default.SettingSignImg = Guid.NewGuid().ToString() + ".png";
                                        Properties.Settings.Default.Save();
                                        //
                                        File.WriteAllBytes(AppDomain.CurrentDomain.BaseDirectory + Properties.Settings.Default.SettingSignImg, chnlVo.CHNL_SIGN_IMG);
                                        //this.img_main.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + Properties.Settings.Default.SettingSignImg, UriKind.Absolute));
                                    }
                                }

                                //성공
                                WinUIMessageBox.Show("등록 되었습니다", SystemProperties.PROGRAM_TITLE, MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                        }
                    }
                }
            }
            catch (Exception eLog)
            {
                WinUIMessageBox.Show("이미지 파일이 맞지 않습니다", SystemProperties.PROGRAM_TITLE, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
        }

        void titleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        void _KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                userLogin();
            }
        }

        void btn_login_Click(object sender, RoutedEventArgs e)
        {
            userLogin();
        }

        void _Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

            if (SystemProperties.isLOGIN)
            {
                e.Cancel = false;
            }
            else
            {
                //MessageBoxResult result = WinUIMessageBox.Show("프로그램을 종료 하시겠습니까? ", "[" + SystemProperties.PROGRAM_TITLE + "]프로그램 종료", MessageBoxButton.YesNo, MessageBoxImage.Question);
                //if (result == MessageBoxResult.Yes)
                {
                    e.Cancel = false;

                    Environment.Exit(0);
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                    this.Close();
                }
                //else if (result == MessageBoxResult.No)
                //{
                //    e.Cancel = true;
                //}
            }
        }

       async void userLogin()
        {
            try
            {
                this.btn_login.IsEnabled = false;
                this.txtId.IsEnabled = false;
                this.txtPass.IsEnabled = false;
                this.img_setting.IsEnabled = false;
                this.img_main_setting.IsEnabled = false;

                SystemProperties.PROGRAM_HTTP = new HttpClient();
                SystemProperties.PROGRAM_HTTP.BaseAddress = new Uri(Properties.Settings.Default.SettingHttp);
                SystemProperties.PROGRAM_HTTP.DefaultRequestHeaders.Accept.Clear();
                SystemProperties.PROGRAM_HTTP.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s136/login/" + Properties.Settings.Default.SettingChnl + "/" + this.txtId.Text))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        System.Security.Cryptography.SHA256Managed sha256Managed = new System.Security.Cryptography.SHA256Managed();
                        //Dao.USR_PWD = Convert.ToBase64String(sha256Managed.ComputeHash(System.Text.Encoding.UTF8.GetBytes(this.text_USR_PWD.Text)));

                        GroupUserVo usrVo = JsonConvert.DeserializeObject<GroupUserVo>(await response.Content.ReadAsStringAsync());
                        if (usrVo == null)
                        {
                            this.btn_login.IsEnabled = true;
                            this.txtId.IsEnabled = true;
                            this.txtPass.IsEnabled = true;
                            this.img_setting.IsEnabled = true;
                            this.img_main_setting.IsEnabled = true;

                            WinUIMessageBox.Show("아이디가 존재 하지 않습니다.", "[" + SystemProperties.PROGRAM_TITLE + "]실패", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }
                        else if (usrVo.DELT_FLG.Equals("Y"))
                        {
                            this.btn_login.IsEnabled = true;
                            this.txtId.IsEnabled = true;
                            this.txtPass.IsEnabled = true;
                            this.img_setting.IsEnabled = true;
                            this.img_main_setting.IsEnabled = true;

                            WinUIMessageBox.Show("아이디가 존재 하지 않습니다.", "[" + SystemProperties.PROGRAM_TITLE + "]실패", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }


                        //암호화 비교 Convert.ToBase64String(sha256Managed.ComputeHash(System.Text.Encoding.UTF8.GetBytes(   )))
                        if (Convert.ToBase64String(sha256Managed.ComputeHash(System.Text.Encoding.UTF8.GetBytes(this.txtPass.Password))).Equals(usrVo.USR_PWD))
                        {
                            SystemProperties.AUTH = "1";
                            SystemProperties.GUID = Guid.NewGuid().ToString();
                            SystemProperties.USER = this.txtId.Text;
                            SystemProperties.isLOGIN = true;
                            SystemProperties.USER_VO = usrVo;

                            Properties.Settings.Default.SettingId = this.txtId.Text;
                            Properties.Settings.Default.Save();

                            //로그인 현황 저장
                            using (HttpClient client = new HttpClient())
                            {
                                client.BaseAddress = new Uri(Properties.Settings.Default.SettingHttp);
                                client.DefaultRequestHeaders.Accept.Clear();
                                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                                //
                                SystemCodeVo _codeVo = new SystemCodeVo();
                                _codeVo.CHNL_CD = Properties.Settings.Default.SettingChnl;
                                //ERP
                                _codeVo.PGM_CLSS_CD = "A";
                                _codeVo.LGIN_GUID = SystemProperties.GUID;
                                _codeVo.LGIN_USR_ID = SystemProperties.USER;
                                _codeVo.CRE_USR_ID = SystemProperties.USER;
                                _codeVo.UPD_USR_ID = SystemProperties.USER;

                                using (HttpResponseMessage response_X = await client.PostAsync("s11315/i", new StringContent(JsonConvert.SerializeObject(_codeVo), System.Text.Encoding.UTF8, "application/json")))
                                {
                                    if (response_X.IsSuccessStatusCode)
                                    {
                                        //로그인 현황 업데이트

                                        //string uuid = await response.Content.ReadAsStringAsync();

                                    }
                                }

                                //// API KEY 기록 남기기
                                //if (usrVo.API_KEY != null)
                                //{
                                //    IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
                                //    string LocalIP = string.Empty;

                                //    for (int i = 0; i < host.AddressList.Length; i++)
                                //    {
                                //        if (host.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
                                //        {
                                //            LocalIP = host.AddressList[i].ToString();
                                //            break;
                                //        }
                                //    }

                                //    // 인증키 + 시간 + 접속 + 아이디 + IP + 크기(0) 
                                //    string dataApiKey = "{\"crtfcKey\":\"" + usrVo.API_KEY + "\",\"logDt\":\"" + usrVo.API_DT + "\",\"useSe\":\"접속\",\"sysUser\":\"" + usrVo.USR_N1ST_NM + "\",\"conectIp\":\"" + LocalIP + "\",\"dataUsgqty\":\"0\"}";

                                //    string getdata = HttpUtility.UrlEncode(dataApiKey, Encoding.UTF8);
                                //    HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create("https://log.smart-factory.kr/apisvc/sendLogDataJSON.do?logData=" + getdata);
                                //    myReq.Method = "GET";
                                //    HttpWebResponse wRes = (HttpWebResponse)myReq.GetResponse();
                                //    Stream respGetStream = wRes.GetResponseStream();
                                //    StreamReader readerGet = new StreamReader(respGetStream, Encoding.UTF8);
                                //    string resultGet = readerGet.ReadToEnd();
                                //}
                            }

                            //main_Img 다시 저장 하기
                            ProgramVo chnlVo;
                            ////URL Check & CHNL Check
                            using (HttpClient client = new HttpClient())
                            {
                                client.BaseAddress = new Uri(Properties.Settings.Default.SettingHttp);
                                client.DefaultRequestHeaders.Accept.Clear();
                                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                                HttpResponseMessage _response = await client.GetAsync("s136/chnl/" + Properties.Settings.Default.SettingChnl);
                                if (_response.IsSuccessStatusCode)
                                {
                                    chnlVo = JsonConvert.DeserializeObject<ProgramVo>(await _response.Content.ReadAsStringAsync());

                                    ////Folder -> Png 삭제
                                    //string[] files = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.png");
                                    //foreach (string file in files)
                                    //{
                                    //    try
                                    //    {
                                    //        File.Delete(file);
                                    //    }
                                    //    catch
                                    //    {
                                    //        continue;
                                    //    }
                                    //}

                                    //Main Img Save
                                    if (chnlVo.CHNL_LOG_IMG?.Length > 0)
                                    {
                                        //if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory) == true)
                                        //{
                                        //Properties.Settings.Default.SettingLogImg = Guid.NewGuid().ToString() + ".png";
                                        //Properties.Settings.Default.SettingLogImg = Convert.ToBase64String(chnlVo.CHNL_LOG_IMG);
                                        //Properties.Settings.Default.Save();

                                        //byte[] binaryData = Convert.FromBase64String(Properties.Settings.Default.SettingLogImg);

                                        //BitmapImage bi = new BitmapImage();
                                        //bi.BeginInit();
                                        //using (var stream = new MemoryStream(binaryData))
                                        //{
                                        //    bi.StreamSource = stream;
                                        //}
                                        //bi.EndInit();
                                        ////
                                        //this.img_main.Source = bi;
                                        //
                                        //File.WriteAllBytes(AppDomain.CurrentDomain.BaseDirectory + Properties.Settings.Default.SettingLogImg, chnlVo.CHNL_LOG_IMG);
                                        //this.img_main.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + Properties.Settings.Default.SettingLogImg, UriKind.Absolute));
                                        //this.img_main.Source = bi;
                                        if (string.IsNullOrEmpty(Properties.Settings.Default.SettingLogImg) == false)
                                        {
                                            this.img_main.Source = ByteToImage(Convert.FromBase64String(Properties.Settings.Default.SettingLogImg));
                                        }
                                        //}
                                    }
                                    else
                                    {
                                        Properties.Settings.Default.SettingLogImg = "";
                                        Properties.Settings.Default.Save();

                                        this.img_main.Source = null;
                                    }

                                    //Sign Img Save
                                    if (chnlVo.CHNL_SIGN_IMG?.Length > 0)
                                    {
                                        if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory) == true)
                                        {
                                            Properties.Settings.Default.SettingSignImg = Guid.NewGuid().ToString() + ".png";
                                            Properties.Settings.Default.Save();
                                            //
                                            File.WriteAllBytes(AppDomain.CurrentDomain.BaseDirectory + Properties.Settings.Default.SettingSignImg, chnlVo.CHNL_SIGN_IMG);
                                            //this.img_main.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + Properties.Settings.Default.SettingSignImg, UriKind.Absolute));
                                        }
                                    }
                                    else
                                    {
                                        Properties.Settings.Default.SettingSignImg = "";
                                        Properties.Settings.Default.Save();
                                    }
                                }
                            }
                            //로그인 성공
                            this.DialogResult = true;
                        }
                        else
                        {
                            this.btn_login.IsEnabled = true;
                            this.txtId.IsEnabled = true;
                            this.txtPass.IsEnabled = true;
                            this.img_setting.IsEnabled = true;
                            this.img_main_setting.IsEnabled = true;

                            WinUIMessageBox.Show("로그인 실패 했습니다." + Environment.NewLine + Environment.NewLine + "[패스워드 분실]" + Environment.NewLine + "전산실에 문의 주세요", SystemProperties.PROGRAM_TITLE, MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }
                    }
                    else
                    {
                        WinUIMessageBox.Show(await response.Content.ReadAsStringAsync(), SystemProperties.PROGRAM_TITLE, MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                }
            }
            catch (Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, SystemProperties.PROGRAM_TITLE, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
        }

        //복호화
        string Decrypt(string textToDecrypt, string key)
        {
            try
            {
                using (RijndaelManaged rijndaelCipher = new RijndaelManaged())
                {
                    rijndaelCipher.Mode = CipherMode.CBC;
                    rijndaelCipher.Padding = PaddingMode.PKCS7;
                    rijndaelCipher.KeySize = 128;
                    rijndaelCipher.BlockSize = 128;
                    byte[] encryptedData = Convert.FromBase64String(textToDecrypt);
                    byte[] pwdBytes = Encoding.UTF8.GetBytes(key);
                    byte[] keyBytes = new byte[16];
                    int len = pwdBytes.Length;
                    if (len > keyBytes.Length)
                    {
                        len = keyBytes.Length;
                    }
                    Array.Copy(pwdBytes, keyBytes, len);
                    rijndaelCipher.Key = keyBytes;
                    rijndaelCipher.IV = keyBytes;
                    byte[] plainText = rijndaelCipher.CreateDecryptor().TransformFinalBlock(encryptedData, 0, encryptedData.Length);
                    return Encoding.UTF8.GetString(plainText);
                }
            }
            catch (Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, SystemProperties.PROGRAM_TITLE, MessageBoxButton.OK, MessageBoxImage.Warning);
                return "";
            }
        }



        //이미지 변환
        public ImageSource ByteToImage(byte[] imageData)
        {
            BitmapImage biImg = new BitmapImage();
            MemoryStream ms = new MemoryStream(imageData);
            biImg.BeginInit();
            biImg.StreamSource = ms;
            biImg.EndInit();

            //ImageSource imgSrc = biImg as ImageSource;

            return biImg as ImageSource;
        }



    }
}
