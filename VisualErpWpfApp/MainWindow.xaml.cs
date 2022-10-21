using DevExpress.Xpf.Core;
using DevExpress.Xpf.Docking;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Ribbon;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Auth;
using ModelsLibrary.Code;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using AquilaErpWpfApp3.Util;
using AquilaErpWpfApp3.View.S.Dialog;
using System.Net;
using System.Web;
using System.Net.Sockets;
using System.Text;

namespace AquilaErpWpfApp3
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리 
    /// </summary>
    public partial class MainWindow : DXRibbonWindow
    {

        private MailWriteDialog msgWriteDialog;
        private MailDialog msgGetDialog;



        public DocumentPanel panel;
        private S136UserDialog userDialog;

        public MainWindow()
        {
            InitializeComponent();


            //this.wbSite.Navigate("http://digitalieg.com/web/");

            this.Closing += new System.ComponentModel.CancelEventHandler(_Closing);
            this.GridEdit_menu.MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(GridEdit_menu_MouseDoubleClick);

            this.Title = SystemProperties.PROGRAM_TITLE;
            this.p_menu.Caption = Properties.Settings.Default.SettingCompany;

            //MAIN - IMAGE
            //this.img_main.ImageSource  = new BitmapImage(new Uri(@"Images\logo.ico", UriKind.Relative));

            MenuSearch();

            //this.bLogout.Content = SystemProperties.USER + "님 로그인 하셨습니다.";
            //
            if (DXSplashScreen.IsActive == true)
            {
                DXSplashScreen.Close();
            }

        }

       async void MenuSearch()
        {
            try
            {
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s136/menu/" + Properties.Settings.Default.SettingChnl + "/" + Properties.Settings.Default.SettingId))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.GridEdit_menu.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<ProgramVo>>(await response.Content.ReadAsStringAsync());
                    }
                }
            }
            catch (Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, SystemProperties.PROGRAM_TITLE, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }


        void GridEdit_menu_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            TreeListNode node = this.GridEditView_menu.FocusedNode;
            ProgramVo dao = (ProgramVo)this.GridEdit_menu.GetFocusedRow();
            try
            {
                if (dao.PGM_CD.Equals("A"))
                {
                    this.dockManager.ClosedPanels.Clear();
                    BaseLayoutItem item = this.dockManager.GetItem(dao.PGM_NM);
                    if (item != null)
                    {
                        this.dockManager.ActiveMDIItem = item;
                        item.AllowSelection = true;
                        item.IsActive = true;
                        return;
                    }


                    DXSplashScreen.Show<ProgressWindow>();
                    //
                    this.panel = this.dockManager.DockController.AddDocumentPanel(documentContainer, new Uri(SystemProperties.PROGRAM_NAME + "View/" + dao.SYS_AREA_CD + "/" + dao.PGM_NM + ".xaml", UriKind.Relative));
                    this.panel.Name = dao.PGM_NM;
                    this.panel.ToolTip = dao.MDL_DESC;
                    this.panel.Caption = dao.MDL_NM;
                    this.panel.AllowContextMenu = false;
                    this.panel.CaptionImage = DecodePhoto(dao.IMAGE);
                    //panel.CaptionImage = new BitmapImage(new Uri(SystemProperties.PROGRAM_NAME + "Images/Menu/" + dao.PGM_IMG_NM + ".png", UriKind.Relative));


                    this.dockManager.Activate(panel);
                    //
                    this.panel.Loaded += new RoutedEventHandler(panel_Loaded);
                }
                else
                {
                    if (node.IsExpanded)
                    {
                        //this.GridEdit_menu.CollapseMasterRow(this.GridEditView_menu.FocusedRowHandle);
                        node.CollapseAll();
                    }
                    else
                    {
                        //this.GridEdit_menu.ExpandMasterRow(this.GridEditView_menu.FocusedRowHandle);
                        node.ExpandAll();
                    }
                }
            }
            catch (Exception eLog)
            {
                if (DXSplashScreen.IsActive)
                {
                    DXSplashScreen.Close();
                }
                WinUIMessageBox.Show(eLog.Message, SystemProperties.PROGRAM_TITLE, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        void panel_Loaded(object sender, RoutedEventArgs e)
        {
            panel.Loaded -= panel_Loaded;
            if (DXSplashScreen.IsActive == true)
            {
                DXSplashScreen.Close();
            }
        }

        async void _Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (SystemProperties.isLOGIN && SystemProperties.isEND)
            {
                e.Cancel = false;

                Environment.Exit(0);
                System.Diagnostics.Process.GetCurrentProcess().Kill();
                this.Close();
            }

            MessageBoxResult result = WinUIMessageBox.Show("프로그램을 종료 하시겠습니까? ", "[" + SystemProperties.PROGRAM_TITLE + "] 프로그램 종료", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                e.Cancel = false;


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

                    using (HttpResponseMessage response_X = await client.PostAsync("s11315/u", new StringContent(JsonConvert.SerializeObject(_codeVo), System.Text.Encoding.UTF8, "application/json")))
                    {
                        if (response_X.IsSuccessStatusCode)
                        {
                            //로그인 현황 업데이트
                        }
                    }

                    //// API KEY 종료 로그 남기기
                    //using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s136/login/" + Properties.Settings.Default.SettingChnl + "/" + SystemProperties.USER))
                    //{
                    //    if (response.IsSuccessStatusCode)
                    //    {
                    //        GroupUserVo usrVo = JsonConvert.DeserializeObject<GroupUserVo>(await response.Content.ReadAsStringAsync());

                    //        if (usrVo.API_KEY != null)
                    //        {
                    //            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
                    //            string LocalIP = string.Empty;

                    //            for (int i = 0; i < host.AddressList.Length; i++)
                    //            {
                    //                if (host.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
                    //                {
                    //                    LocalIP = host.AddressList[i].ToString();
                    //                    break;
                    //                }
                    //            }

                    //            // 인증키 + 시간 + 접속 + 아이디 + IP + 크기(0) 
                    //            string dataApiKey = "{\"crtfcKey\":\"" + usrVo.API_KEY + "\",\"logDt\":\"" + usrVo.API_DT + "\",\"useSe\":\"쫑료\",\"sysUser\":\"" + usrVo.USR_N1ST_NM + "\",\"conectIp\":\"" + LocalIP + "\",\"dataUsgqty\":\"0\"}";

                    //            string getdata = HttpUtility.UrlEncode(dataApiKey, Encoding.UTF8);
                    //            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create("https://log.smart-factory.kr/apisvc/sendLogDataJSON.do?logData=" + getdata);
                    //            myReq.Method = "GET";
                    //            HttpWebResponse wRes = (HttpWebResponse)myReq.GetResponse();
                    //            Stream respGetStream = wRes.GetResponseStream();
                    //            StreamReader readerGet = new StreamReader(respGetStream, Encoding.UTF8);
                    //            string resultGet = readerGet.ReadToEnd();
                    //        }
                    //    }
                    //}

                    //메모리 정리
                    SystemProperties.PROGRAM_HTTP.Dispose();

                    Environment.Exit(0);
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                    this.Close();
                }
            }
            else if (result == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
        }

        private BitmapImage DecodePhoto(byte[] byteVal)
        {
            if (byteVal == null) return new BitmapImage();

            try
            {
                MemoryStream strmImg = new MemoryStream(byteVal);
                BitmapImage myBitmapImage = new BitmapImage();
                myBitmapImage.BeginInit();
                myBitmapImage.StreamSource = strmImg;
                myBitmapImage.DecodePixelWidth = 18;
                myBitmapImage.DecodePixelHeight = 18;
                myBitmapImage.EndInit();
                return myBitmapImage;
            }
            catch
            {
                return new BitmapImage();
            }
        }



        private void b_Refresh_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            MenuSearch();
        }

        private void b_UserInfo_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            this.userDialog = new S136UserDialog(SystemProperties.USER_VO);
            this.userDialog.Title = "사용자 정보 [" + SystemProperties.USER + "님]";
            this.userDialog.Owner = Application.Current.MainWindow;
            this.userDialog.BorderEffect = BorderEffect.Default;
            this.userDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            this.userDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            this.userDialog.ShowDialog();
        }

        private void Mail_write_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            this.msgWriteDialog = new MailWriteDialog();
            this.msgWriteDialog.Owner = Application.Current.MainWindow;
            this.msgWriteDialog.BorderEffect = BorderEffect.Default;
            this.msgWriteDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            this.msgWriteDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            this.msgWriteDialog.ShowDialog();
        }

        private void Mail_get_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            this.msgGetDialog = new MailDialog();
            this.msgGetDialog.Owner = Application.Current.MainWindow;
            this.msgGetDialog.BorderEffect = BorderEffect.Default;
            this.msgGetDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            this.msgGetDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            this.msgGetDialog.ShowDialog();
        }

    }
}
