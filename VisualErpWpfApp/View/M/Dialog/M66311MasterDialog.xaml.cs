using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Code;
using ModelsLibrary.Man;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;
using AquilaErpWpfApp3.Util;
using System.Collections.Generic;
using System.Linq;

namespace AquilaErpWpfApp3.M.View.Dialog
{
    /// <summary>
    /// Interaction logic for ConfigView1Dialog.xaml
    /// </summary>
    public partial class M66311MasterDialog : DXWindow
    {
        private ManVo orgDao;
        private bool isEdit = false;
        private ManVo updateDao;
        private string _title = "가공생산지시관리";

        public M66311MasterDialog()
        {
            InitializeComponent();

            this.text_START_Dt.Text = DateTime.Now.ToString("yyyy-MM-dd");
            this.text_END_Dt.Text = DateTime.Now.ToString("yyyy-MM-dd");


            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
            this.OKButton.Click += new RoutedEventHandler(OKButton_Click);
            this.CancelButton.Click += new RoutedEventHandler(CancelButton_Click);
            this.btn_ITEMS.Click += new RoutedEventHandler(btn_ITEMS_Click);
        }



        private async void btn_ITEMS_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.orgDao = getDomain();
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m66107/mst", new StringContent(JsonConvert.SerializeObject(this.orgDao), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.ViewGridDtl.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();

                    }
                }
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, _title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }

        #region Functon (OKButton_Click, CancelButton_Click)
        private async void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.ViewGridDtl.SelectedItem == null) return;

                updateDao = (this.ViewGridDtl.SelectedItem) as ManVo;


                this.DialogResult = true;
                this.Close();
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, _title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
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

        #region Functon (getDomain - ConfigView1Dao)
        public ManVo getDomain()
        {
            ManVo Dao = new ManVo();

            Dao.FM_DT = Convert.ToDateTime(this.text_START_Dt.Text).ToString("yyyy.MM.dd");
            Dao.TO_DT = Convert.ToDateTime(this.text_END_Dt.Text).ToString("yyyy.MM.dd");

            return Dao;
        }
        #endregion

        //IsEdit
        public bool IsEdit
        {
            get
            {
                return this.isEdit;
            }
        }


        public ManVo resultDomain

        {
            get
            {
                return this.updateDao;
            }
        }
    }
}