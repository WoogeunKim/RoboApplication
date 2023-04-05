using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Code;
using ModelsLibrary.Man;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;
using AquilaErpWpfApp3.Util;
using AquilaErpWpfApp3.View.M.Dialog;
using AquilaErpWpfApp3.M.View.Dialog;
using DevExpress.Xpf.Core;
using System.Windows.Media;

namespace AquilaErpWpfApp3.ViewModel
{
    public sealed class M66311ViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private string _title = "BarList 추출검증";

        private IList<ManVo> selectedMstList = new List<ManVo>();

        private M66311MasterDialog masterDialog;
        public M66311ViewModel()
        {
            SYSTEM_CODE_VO();
        }

        // Master
        [Command]
        public async void Refresh(string _OPMZ_NO)
        {
            try
            {
                if (DXSplashScreen.IsActive == false) DXSplashScreen.Show<ProgressWindow>();

                ManVo _param = new ManVo();
                _param.OPMZ_NO = _OPMZ_NO;
                _param.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;

                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("M66311/mst", new StringContent(JsonConvert.SerializeObject(_param), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectMstList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();

                    }
                }

                if (DXSplashScreen.IsActive == true) DXSplashScreen.Close();
            }
            catch(System.Exception eLog)
            {
                if (DXSplashScreen.IsActive == true) DXSplashScreen.Close();

                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }

        // Dialog
        [Command]
        public async void NewContact()
        {
            masterDialog = new M66311MasterDialog();
            masterDialog.Title = _title + " - 선택";
            masterDialog.Owner = Application.Current.MainWindow;
            masterDialog.BorderEffect = BorderEffect.Default;
            masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            bool isDialog = (bool)masterDialog.ShowDialog();
            if (isDialog)
            {
                string Selected_OPMZ_NO = masterDialog.resultDomain.OPMZ_NO;

                Refresh(Selected_OPMZ_NO);
            }
        }

        private async void SYSTEM_CODE_VO()
        {
            try
            {
                // 절단설비
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("M66311/n1st/eq"
                                                                                                   , new StringContent(JsonConvert.SerializeObject(new ManVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectN1stList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();

                    }
                }

                // 가공설비
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("M66311/n2nd/eq"
                                                                                                   , new StringContent(JsonConvert.SerializeObject(new ManVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectN2ndList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();

                    }

                }
            }
            catch(Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }









        // MVVM 방식으로 화면과 바이딩을 합니다.

        private string _M_SEARCH_TEXT = string.Empty;
        public string M_SEARCH_TEXT
        {
            get { return _M_SEARCH_TEXT; }
            set { SetProperty(ref _M_SEARCH_TEXT, value, () => M_SEARCH_TEXT); }
        }

        public IList<ManVo> SelectMstList
        {
            get { return selectedMstList; }
            set { SetProperty(ref selectedMstList, value, () => SelectMstList); }
        }

        ManVo _selectedMstItem;
        public ManVo SelectedMstItem
        {
            get { return _selectedMstItem; }
            set { SetProperty(ref _selectedMstItem, value, () => SelectedMstItem); }
        }

        private IList<ManVo> _selectN1stList = new List<ManVo>();
        public IList<ManVo> SelectN1stList
        {
            get { return _selectN1stList; }
            set { SetProperty(ref _selectN1stList, value, () => SelectN1stList); }
        }
        private IList<ManVo> _selectN2ndList = new List<ManVo>();
        public IList<ManVo> SelectN2ndList
        {
            get { return _selectN2ndList; }
            set { SetProperty(ref _selectN2ndList, value, () => SelectN2ndList); }
        }

        string _Title = string.Empty;
        public string Title
        {
            get { return _Title; }
            set { SetProperty(ref _Title, value, () => Title); }
        }
    }  
}
