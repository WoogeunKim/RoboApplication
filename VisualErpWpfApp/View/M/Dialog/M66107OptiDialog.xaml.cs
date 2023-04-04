using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Auth;
using ModelsLibrary.Code;
using ModelsLibrary.Man;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;
using AquilaErpWpfApp3.Util;
using System.Net;

namespace AquilaErpWpfApp3.View.M.Dialog
{
    /// <summary>
    /// M66107OptiDialog.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class M66107OptiDialog : DXWindow
    {
        private string _title = "Loss 최적화 수행";
        private ManVo orgDao;

        public M66107OptiDialog(ManVo Dao)
        {
            InitializeComponent();

            orgDao = new ManVo()
            {
                  OPMZ_NO = Dao.OPMZ_NO
            };

            this.configCode.DataContext = orgDao;
            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
            this.OKButton.Click += new RoutedEventHandler(OKButton_Click);
            this.CancelButton.Click += new RoutedEventHandler(CancelButton_Click);
        }


        #region Functon (getDomain - ConfigView1Dao)
        private ManVo getDomain()
        {
            try
            {
                ManVo Dao = new ManVo();
                Dao = orgDao;

                Dao.OPMZ_NO = orgDao.OPMZ_NO;

                Dao.CRE_USR_ID = SystemProperties.USER;
                Dao.UPD_USR_ID = SystemProperties.USER;
                Dao.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;

                return Dao;

            }
            catch (Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return new ManVo();
            }
        }
        #endregion

        #region Functon (OKButton_Click, CancelButton_Click)
        private async void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (DXSplashScreen.IsActive == false) DXSplashScreen.Show<ProgressWindow>();

                string _isChecked = is_checked.IsChecked == true ? "Y" : "N";

                HttpClient httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                //string url = "http://210.217.42.139:8880/robocon/api/optimize_barlist/v2?";  // 2023-04-04 호출주소 변경
                //string value = "OPMZ_NO=" + orgDao.OPMZ_NO + "&" + "APPLY_ELON=" + _isChecked;
                string url = "http://aiblue.ddns.net:8880/robocon/api/optimize_barlist/v3?";
                string value = "OPMZ_NO=" + orgDao.OPMZ_NO + "&" + "PLANNING_MODE=" + _isChecked;

                httpClient.GetAsync(url + value);

                //using (var playResponse = await httpClient.GetAsync(url + value))
                //{
                //    if (HttpStatusCode.OK != playResponse.StatusCode)
                //    {
                //        if (DXSplashScreen.IsActive == true) DXSplashScreen.Close();
                //        WinUIMessageBox.Show(playResponse.ReasonPhrase, _title, MessageBoxButton.OK, MessageBoxImage.Error);
                //        return;
                //    }
                //}

                if (DXSplashScreen.IsActive == true) DXSplashScreen.Close();



                this.DialogResult = true;
                this.Close();

            }
            catch (System.Exception eLog)
            {
                if (DXSplashScreen.IsActive == true) DXSplashScreen.Close();

                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
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

        public string OPMZ_NO
        {
            get;
            set;
        }
    }
}
