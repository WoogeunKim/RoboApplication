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
using DevExpress.Xpf.Core;
using System.Windows.Media;
using AquilaErpWpfApp3.View.M.Dialog;


namespace AquilaErpWpfApp3.ViewModel
{
    public sealed class M66107ViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private string title = "Loss 최적화 수행";

        private IList<ManVo> selectedMstList = new List<ManVo>();

        private M66107MasterDialog masterDialog;
        private M66107DetailDialog detailDialog;
        private M66107OptiDialog optiRunDialog;

        public M66107ViewModel()
        {
            StartDt = System.DateTime.Now;
            EndDt = System.DateTime.Now;

            SYSTEM_CODE_VO();
        }

        [Command]
        public async void Refresh()
        {
            try
            {
                ManVo _param = new ManVo();
                _param.FM_DT = (StartDt).ToString("yyyy-MM-dd");
                _param.TO_DT = (EndDt).ToString("yyyy-MM-dd");

                _param.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;

                if (EndDt < StartDt)
                {
                    WinUIMessageBox.Show("조회일자가 올바르지 않습니다.", title, MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("M66107/mst", new StringContent(JsonConvert.SerializeObject(_param), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectMstList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();

                        Title = "[기간]" + (StartDt).ToString("yyyy-MM-dd") + "~" + (EndDt).ToString("yyyy-MM-dd");

                        if (SelectMstList.Count > 0)
                        {
                            isM_UPDATE = true;
                        }
                        else
                        {
                            isM_UPDATE = false;
                            SelectDtlList = null;
                        }

                    }
                }
            }
            catch (System.Exception eLog)
            {
                //DXSplashScreen.Close();
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _Title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }

        public async void DtListRefresh()
        {
            try
            {
                if (SelectedMstItem == null) return;
                if (DXSplashScreen.IsActive == false) DXSplashScreen.Show<ProgressWindow>();

                // 해당 최적화 건에 로그상태
                using (HttpResponseMessage responseLog = await SystemProperties.PROGRAM_HTTP.PostAsync("m66107/mst/log", new StringContent(JsonConvert.SerializeObject(SelectedMstItem), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (responseLog.IsSuccessStatusCode)
                    {
                        this.SelectMstLogList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await responseLog.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
                    }
                }

                // 최적화 해당 자재 리스트
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m66107/dtl", new StringContent(JsonConvert.SerializeObject(SelectedMstItem), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectDtlList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
                    }
                }

                if (DXSplashScreen.IsActive == true) DXSplashScreen.Close();
            }
            catch (System.Exception eLog)
            {
                if (DXSplashScreen.IsActive == true) DXSplashScreen.Close();
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }

        // MST 추가
        [Command]
        public void NewContact()
        {
            try
            {
                masterDialog = new M66107MasterDialog(new ManVo() { });
                masterDialog.Title = title + " - 추가";
                masterDialog.Owner = Application.Current.MainWindow;
                masterDialog.BorderEffect = BorderEffect.Default;
                masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
                masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
                bool isDialog = (bool)masterDialog.ShowDialog();
                if (isDialog)
                {
                    Refresh();
                }
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }

        // MST 수정
        [Command]
        public void EditContact()
        {
            try
            {
                if (SelectedMstItem == null) return;

                masterDialog = new M66107MasterDialog(SelectedMstItem);
                masterDialog.Title = title + " - 수정";
                masterDialog.Owner = Application.Current.MainWindow;
                masterDialog.BorderEffect = BorderEffect.Default;
                masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
                masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
                bool isDialog = (bool)masterDialog.ShowDialog();
                if (isDialog)
                {
                    Refresh();
                }
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }

        // dtl 추가
        [Command]
        public void New2Contact()
        {
            try
            {
                detailDialog = new M66107DetailDialog(SelectedMstItem);
                detailDialog.Title = title + " - 추가";
                detailDialog.Owner = Application.Current.MainWindow;
                detailDialog.BorderEffect = BorderEffect.Default;
                detailDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
                detailDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
                bool isDialog = (bool)detailDialog.ShowDialog();
                if (isDialog)
                {
                    Refresh();
                }
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }

        // dtl 추가
        [Command]
        public void OptiRun()
        {
            try
            {
                if (SelectedMstItem == null || SelectDtlList.Count == 0)
                {
                    WinUIMessageBox.Show("조회된 데이터가 없습니다.", "[유효검사]", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.None, MessageBoxOptions.None);
                    return;
                }

                optiRunDialog = new M66107OptiDialog(SelectedMstItem);
                optiRunDialog.Title = title + " - Opti Run";
                optiRunDialog.Owner = Application.Current.MainWindow;
                optiRunDialog.BorderEffect = BorderEffect.Default;
                optiRunDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
                optiRunDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
                bool isDialog = (bool)optiRunDialog.ShowDialog();
                if (isDialog)
                {
                    Refresh();
                }
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }

        public async void SYSTEM_CODE_VO()
        {
            Refresh();
        }


        //#region Functon <Master List>
        DateTime _startDt;
        public DateTime StartDt
        {
            get { return _startDt; }
            set { SetProperty(ref _startDt, value, () => StartDt); }
        }

        DateTime _endDt;
        public DateTime EndDt
        {
            get { return _endDt; }
            set { SetProperty(ref _endDt, value, () => EndDt); }
        }

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
            get
            {
                return _selectedMstItem;
            }
            set
            {
                SetProperty(ref _selectedMstItem, value, () => SelectedMstItem); DtListRefresh();

            }
        }

        IList<ManVo> _selectMstLogList = new List<ManVo>();
        public IList<ManVo> SelectMstLogList
        {
            get { return _selectMstLogList; }
            set { SetProperty(ref _selectMstLogList, value, () => SelectMstLogList); }
        }

        private IList<ManVo> selectedDtlList = new List<ManVo>();

        public IList<ManVo> SelectDtlList
        {
            get { return selectedDtlList; }
            set { SetProperty(ref selectedDtlList, value, () => SelectDtlList); }
        }

        string _Title = string.Empty;
        public string Title
        {
            get { return _Title; }
            set { SetProperty(ref _Title, value, () => Title); }
        }

        bool _isM_UPDATE; 
        public bool isM_UPDATE
        {
            get { return _isM_UPDATE; }
            set { SetProperty(ref _isM_UPDATE, value, () => isM_UPDATE);}
        }

    }
    
}
