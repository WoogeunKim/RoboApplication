using AquilaErpWpfApp3.Util;
using AquilaErpWpfApp3.View.S.Dialog;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Code;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Media;

namespace AquilaErpWpfApp3.ViewModel
{
 
    public sealed class S11472ViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private string _title = "창고위치관리";
        private IList<SystemCodeVo> selectedMasterViewList;
        private S11472MasterDialog masterDialog;

        public S11472ViewModel()
        {
            Refresh();
        }

        [Command]
        public async void Refresh(string _LOC_NM = null)
        {
            try
            {
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("S11472", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectedMasterViewList = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();


                        if (SelectedMasterViewList.Count > 0)
                        {
                            isM_UPDATE = true;
                            isM_DELETE = true;

                            if (string.IsNullOrEmpty(_LOC_NM))
                            {
                                SelectedMasterItem = SelectedMasterViewList[0];
                            }
                            else
                            {
                                SelectedMasterItem = SelectedMasterViewList.Where(x => x.LOC_NM.Equals(_LOC_NM)).LastOrDefault<SystemCodeVo>();
                            }
                        }
                        else
                        {
                            isM_UPDATE = false;
                            isM_DELETE = false;
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

        #region 추가, 수정(예정), 삭제(예정)
        [Command]
        public async void NewMasterContact()
        {
            masterDialog = new S11472MasterDialog(new SystemCodeVo() { AREA_CD = SystemProperties.USER_VO.EMPE_PLC_NM, AREA_NM = SystemProperties.USER_VO.EMPE_PLC_CD, INV_CAPA_WGT = 0 });
            masterDialog.Title = _title + " - 추가";
            masterDialog.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            masterDialog.Owner = Application.Current.MainWindow;
            masterDialog.BorderEffect = BorderEffect.Default;
            masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            bool isDialog = (bool)masterDialog.ShowDialog();
            if (isDialog)
            {
                Refresh(masterDialog.resultDomain.N1ST_LOC_ID + "-" + masterDialog.resultDomain.N2ND_LOC_ID);
            }
        }


        [Command]
        public async void EditMasterContact()
        {
            masterDialog = new S11472MasterDialog(SelectedMasterItem);
            masterDialog.Title = _title + " - 수정";
            masterDialog.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            masterDialog.Owner = Application.Current.MainWindow;
            masterDialog.BorderEffect = BorderEffect.Default;
            masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            bool isDialog = (bool)masterDialog.ShowDialog();
            if (isDialog)
            {
                Refresh(masterDialog.resultDomain.N1ST_LOC_ID + "-" + masterDialog.resultDomain.N2ND_LOC_ID);
            }
        }

        [Command]
        public async void DelMasterContact()
        {
           
            try
            {
                SystemCodeVo delDao = this._selectMasterItem;
                if (delDao != null)
                {
                    MessageBoxResult result = WinUIMessageBox.Show("[" + delDao.N1ST_LOC_ID + "동 " + delDao.N2ND_LOC_ID + "]" + " 정말로 삭제 하시겠습니까?", _title, MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s11472/d", new StringContent(JsonConvert.SerializeObject(delDao), System.Text.Encoding.UTF8, "application/json")))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                int _Num = 0;
                                string resultMsg = await response.Content.ReadAsStringAsync();
                                if (int.TryParse(resultMsg, out _Num) == false)
                                {
                                    //실패
                                    WinUIMessageBox.Show(resultMsg, _title, MessageBoxButton.OK, MessageBoxImage.Error);
                                    return;
                                }
                                Refresh();

                                //성공
                                WinUIMessageBox.Show("삭제가 완료되었습니다.", _title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                            }
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

        #endregion



        #region 프로퍼티 모음
        public IList<SystemCodeVo> SelectedMasterViewList
        {
            get { return selectedMasterViewList; }
            private set { SetProperty(ref selectedMasterViewList, value, () => SelectedMasterViewList); }
        }

        SystemCodeVo _selectMasterItem;
        public SystemCodeVo SelectedMasterItem
        {
            get
            {
                return _selectMasterItem;
            }
            set
            {
                if (value != null)
                {
                    SetProperty(ref _selectMasterItem, value, () => SelectedMasterItem);
                }
            }
        }

        private bool? _isM_UPDATE = false;
        public bool? isM_UPDATE
        {
            get { return _isM_UPDATE; }
            set { SetProperty(ref _isM_UPDATE, value, () => isM_UPDATE); }
        }

        private bool? _isM_DELETE = false;
        public bool? isM_DELETE
        {
            get { return _isM_DELETE; }
            set { SetProperty(ref _isM_DELETE, value, () => isM_DELETE); }
        }

        private string _M_SEARCH_TEXT = string.Empty;
        public string M_SEARCH_TEXT
        {
            get { return _M_SEARCH_TEXT; }
            set { SetProperty(ref _M_SEARCH_TEXT, value, () => M_SEARCH_TEXT); }
        } 
        #endregion



    }
}