using DevExpress.Data;
using DevExpress.Mvvm.Native;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.WindowsUI;
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

namespace AquilaErpWpfApp3.View.M.Dialog
{
    /// <summary>
    /// M66107DetailDialog.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class M66107DetailDialog : DXWindow
    {
        private ManVo orgVo;
        private string _title = "Loss 최적화 수행";

        public M66107DetailDialog(ManVo vo)
        {
            InitializeComponent();

            this.orgVo = vo;

            SYSTEM_CODE_VO();

            this.OKButton.Click += new RoutedEventHandler(OKButton_Click);
            this.CancelButton.Click += new RoutedEventHandler(CancelButton_Click);

            this.OKButton.IsEnabled = false;

        }

        #region Functon (OKButton_Click, CancelButton_Click)
        private async void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<ManVo> checkList = (this.ViewJOB_ITEMEdit.ItemsSource as IList<ManVo>).Where(x => x.isCheckd == true).ToList<ManVo>();


                if (checkList.Count <= 0)
                {
                    WinUIMessageBox.Show("데이터가 존재 하지 않습니다.", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                MessageBoxResult result = WinUIMessageBox.Show("정말로 저장 하시겠습니까?", "[저장]" + _title, MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    int _Num = 0;
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m66107/dtl/dia/i", new StringContent(JsonConvert.SerializeObject(checkList), System.Text.Encoding.UTF8, "application/json")))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string resultMsg = await response.Content.ReadAsStringAsync();
                            if (int.TryParse(resultMsg, out _Num) == false)
                            {
                                //실패
                                WinUIMessageBox.Show(resultMsg, _title, MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }

                            this.DialogResult = true;
                            this.Close();
                        }
                    }
                }
            }
            catch (System.Exception eLog)
            {
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

        private void PART_Editor_Checked(object sender, RoutedEventArgs e)
        {
            this.OKButton.IsEnabled = true;
        }

        private void CheckEdit_Checked(object sender, RoutedEventArgs e)
        {
            CheckEdit checkEdit = sender as CheckEdit;
            ManVo tmpImsi;
            for (int x = 0; x < this.ViewJOB_ITEMEdit.VisibleRowCount; x++)
            {
                int rowHandle = this.ViewJOB_ITEMEdit.GetRowHandleByVisibleIndex(x);
                if (rowHandle > -1)
                {
                    tmpImsi = this.ViewJOB_ITEMEdit.GetRow(rowHandle) as ManVo;
                    if (checkEdit.IsChecked == true)
                    {
                        tmpImsi.isCheckd = true;
                        //
                        this.OKButton.IsEnabled = true;
                    }
                    else
                    {
                        tmpImsi.isCheckd = false;
                    }

                }
            }
        }

        private void viewPage1EditView_HiddenEditor(object sender, EditorEventArgs e)
        {
            this.viewJOB_ITEMView.CommitEditing();
        }

        public async void SYSTEM_CODE_VO()
        {
            try
            {
                if (DXSplashScreen.IsActive == false) DXSplashScreen.Show<ProgressWindow>();

                // 메인화면 조회
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m66107/dtl/dia", new StringContent(JsonConvert.SerializeObject(new ManVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD, OPMZ_NO = orgVo.OPMZ_NO, UPD_USR_ID = SystemProperties.USER }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.ViewJOB_ITEMEdit.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
                    }
                }

                if (DXSplashScreen.IsActive == true) DXSplashScreen.Close();
            }
            catch (System.Exception eLog)
            {
                if (DXSplashScreen.IsActive == true) DXSplashScreen.Close();
                //DXSplashScreen.Close();
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }
    }
}
