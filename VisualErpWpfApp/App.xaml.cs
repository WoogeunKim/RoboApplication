using DevExpress.Xpf.Core;
using Neodynamic.SDK.Printing;
using System;
using System.Windows;
using AquilaErpWpfApp3.Util;
using System.Globalization;

namespace AquilaErpWpfApp3
{
    /// <summary>
    /// App.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class App : Application
    {
        private static LoginDialog loginDialog;

        protected override void OnStartup(StartupEventArgs e)
        {
            Start(() => base.OnStartup(e));
        }

        public static void Start(Action baseStart)
        {
            ThermalLabel.LicenseOwner = "CPCN-Ultimate Edition-Developer License";
            ThermalLabel.LicenseKey = "LHTLJXMSFS84ZC4J2P3N42ESZ75DPZMX633KKMWAMWSPM66AUYDA";

            ThemeManager.ApplicationThemeName = Theme.Office2016ColorfulSE.Name;

            Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;

            #region 다국어 (한국)
            CultureInfo culture = CultureInfo.CreateSpecificCulture("ko-KR");
            System.Threading.Thread.CurrentThread.CurrentUICulture = culture;
            System.Threading.Thread.CurrentThread.CurrentCulture = culture;

            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
            #endregion

            SystemProperties.USER = "";
            //사용자 조회 
            loginDialog = new LoginDialog();
            loginDialog.Topmost = true;
            if (loginDialog.ShowDialog() == true)
            {
                DXSplashScreen.Show<ProgressWindow>();
                baseStart();
            }
        }
    }
}
