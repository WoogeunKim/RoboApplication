using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Code;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;
using AquilaErpWpfApp3.Util;

namespace AquilaErpWpfApp3.S.View.Dialog
{
    public partial class S1145DetailDialog : DXWindow
    {
        //private static CodeServiceClient codeClient = SystemProperties.CodeClient;

        private string title = "단가 이력";

        public S1145DetailDialog(SystemCodeVo Dao)
        {
            InitializeComponent();
            //
           
            try
            {
                Refresh(Dao);
                //this.ViewGridDtl.ItemsSource = codeClient.S1145SelectViewList(Dao);

                this.text_ITM_CD.Text = Dao.ITM_CD;
                this.text_ITM_NM.Text = Dao.ITM_NM;

                this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
                this.CancelButton.Click += new RoutedEventHandler(CancelButton_Click);
            }
            catch (System.Exception eLog)
            {
                //DXSplashScreen.Close();
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
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

        private async void Refresh(SystemCodeVo Dao)
        {
            //this.ViewGridDtl.ItemsSource = codeClient.S1145SelectViewList(Dao);
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s1145/dtl", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { AREA_CD = Dao.AREA_CD, CHNL_CD = Dao.CHNL_CD, ITM_CD = Dao.ITM_CD }), System.Text.Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.ViewGridDtl.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                }

            }
        }

    }
}
