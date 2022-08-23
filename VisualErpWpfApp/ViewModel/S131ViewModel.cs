using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Code;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Media;
using AquilaErpWpfApp3.S.View.Dialog;
using AquilaErpWpfApp3.Util;
using AquilaErpWpfApp3.View.S.Dialog;

namespace AquilaErpWpfApp3.ViewModel
{
    //시스템 분류 코드
    public sealed class S131ViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private string title = "시스템 분류 코드";

        private IList<SystemCodeVo> selectedMasterViewList;
        private IList<SystemCodeVo> selectedDetailViewList;
       
        //Master Dialog
        private S131MasterDialog masterDialog;

        //Detail Dialog
        private S131DetailDialog detailDialog;

        public S131ViewModel() 
        {
            MstRefresh();
        }

        [Command]
        public async void MstRefresh(string _CLSS_TP_CD = null)
        {
            try
            {
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/mst/" + Properties.Settings.Default.SettingChnl))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectedMasterViewList = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                    }

                    //
                    if (SelectedMasterViewList.Count > 0)
                    {
                        isM_UPDATE = true;
                        isM_DELETE = true;

                        if (string.IsNullOrEmpty(_CLSS_TP_CD))
                        {
                            SelectedMasterItem = SelectedMasterViewList[0];
                        }
                        else
                        {
                            SelectedMasterItem = SelectedMasterViewList.Where(x => x.CLSS_TP_CD.Equals(_CLSS_TP_CD)).LastOrDefault<SystemCodeVo>();
                        }
                    }
                    else
                    {
                        SelectedDetailItem = null;
                        SelectedDetailViewList = null;

                        isM_UPDATE = false;
                        isM_DELETE = false;
                        //
                        isD_UPDATE = false;
                        isD_DELETE = false;

                    }
                }
            }
            catch (System.Exception eLog)
            {
                //program.Close();
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }

        #region Functon (Master Add, Edit, Del)
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
                    SetProperty(ref _selectMasterItem, value, () => SelectedMasterItem, OnSelectedMasterItemChanged);
                }
            }
        }

        void OnSelectedMasterItemChanged()
        {
            DtlRefresh();
        }

       [Command]
       public async void DtlRefresh(string _CLSS_CD = null)
       {
            try
            {
                if (this.SelectedMasterItem == null) { return; }
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + this.SelectedMasterItem.CLSS_TP_CD + "/ALL"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectedDetailViewList = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                    }

                    //
                    if (SelectedDetailViewList.Count > 0)
                    {
                        isD_UPDATE = true;
                        isD_DELETE = true;

                        if (string.IsNullOrEmpty(_CLSS_CD))
                        {
                            SelectedDetailItem = SelectedDetailViewList[0];
                        }
                        else
                        {
                            SelectedDetailItem = SelectedDetailViewList.Where(x => x.CLSS_CD.Equals(_CLSS_CD)).FirstOrDefault<SystemCodeVo>();
                        }
                    }
                    else
                    {
                        isD_UPDATE = false;
                        isD_DELETE = false;
                    }
                }
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }

        [Command]
        public void NewMasterContact()
        {
            ShowMasterDialog(new SystemCodeVo());
        }

        [Command]
        public void EditMasterContact()
        {
            if (this._selectMasterItem == null) { return; }
            SystemCodeVo editDao = this._selectMasterItem;
            if (editDao != null)
            {
                ShowMasterDialog(editDao);
            }
        }

        public void ShowMasterDialog(SystemCodeVo dao)
        {
            masterDialog = new S131MasterDialog(dao);
            masterDialog.Title = "Master Code";
            masterDialog.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            masterDialog.Owner = Application.Current.MainWindow;
            masterDialog.BorderEffect = BorderEffect.Default;
            masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            bool isDialog = (bool)masterDialog.ShowDialog();
            if (isDialog)
            {
                MstRefresh(masterDialog.resultDomain.CLSS_TP_CD);
                //SelectedMasterItem = SelectedMasterViewList.Where(x => x.CLSS_TP_CD.Equals(masterDialog.resultDomain.CLSS_TP_CD)).FirstOrDefault<SystemCodeVo>();
            }
        }

        //public ICommand MasterDelDialogCommand
        //{
        //    get
        //    {
        //        if (masterDelDialogCommand == null)
        //            masterDelDialogCommand = new DelegateCommand(DelMasterContact);
        //        return masterDelDialogCommand;
        //    }
        //}

        //[Command]
        //public void DelMasterContact()
        //{
        //    try
        //    {
        //        SystemCodeVo delDao = this._selectMasterItem;
        //        if (delDao != null)
        //        {
        //            if (delDao.DELT_FLG.Equals("미사용"))
        //            {
        //                WinUIMessageBox.Show("현재 미사용 코드 입니다.", "[삭제 - Master]" + title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
        //                return;
        //            }

        //            MessageBoxResult result = WinUIMessageBox.Show(delDao.CLSS_TP_CD + " 정말로 삭제 하시겠습니까?", "[삭제 - Master]" + title, MessageBoxButton.YesNo, MessageBoxImage.Question);
        //            if (result == MessageBoxResult.Yes)
        //            {
        //                delDao.DELT_FLG = "Y";
        //                delDao.USR_ID = SystemProperties.USER;
        //                //codeClient.UpdateMasterCode(delDao);
        //                WinUIMessageBox.Show("삭제가 완료되었습니다.", "[삭제 - Master]" + title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
        //                //
        //                delDao.DELT_FLG = "미사용";
        //            }
        //        }
        //    }
        //     catch (System.Exception eLog)
        //     {
        //         WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
        //         return;
        //     }
        //}
        #endregion

        #region Functon (Detail Add, Edit, Del)
        public IList<SystemCodeVo> SelectedDetailViewList
        {
            get { return selectedDetailViewList; }
            private set { SetProperty(ref selectedDetailViewList, value, () => SelectedDetailViewList); }
        }

        SystemCodeVo _selectedDetailItem;
        public SystemCodeVo SelectedDetailItem
        {
            get
            {
                return _selectedDetailItem;
            }
            set
            {
                if (value != null)
                {
                    SetProperty(ref _selectedDetailItem, value, () => SelectedDetailItem);
                }
            }
        }

        //public ICommand DetailSearchDialogCommand
        //{
        //    get
        //    {
        //        if (detailSearchDialogCommand == null)
        //            detailSearchDialogCommand = new DelegateCommand(SearchDetailContact);
        //        return detailSearchDialogCommand;
        //    }
        //}

        //public void SearchDetailContact()
        //{
        //    OnSelectedMasterItemChanged();
        //}

        //public ICommand DetailNewDialogCommand
        //{
        //    get
        //    {
        //        if (detailNewDialogCommand == null)
        //            detailNewDialogCommand = new DelegateCommand(NewDetailContact);
        //        return detailNewDialogCommand;
        //    }
        //}

        [Command]
        public void NewDetailContact()
        {
            if (this._selectMasterItem == null) { return; }
            ShowDetailDialog(new SystemCodeVo() { CLSS_TP_CD = _selectMasterItem.CLSS_TP_CD });
        }

        //public ICommand DetailEditDialogCommand
        //{
        //    get
        //    {
        //        if (detailEditDialogCommand == null)
        //            detailEditDialogCommand = new DelegateCommand(EditDetailContact, (SelectedDetailItem == null ? false : true));
        //        return detailEditDialogCommand;
        //    }
        //}

        [Command]
        public void EditDetailContact()
        {
            SystemCodeVo editDao = SelectedDetailItem;
            if (editDao != null)
            {
                ShowDetailDialog(editDao);
            }
        }

        public void ShowDetailDialog(SystemCodeVo dao)
        {
            detailDialog = new S131DetailDialog(dao);
            detailDialog.Title = "Detail Code (분류 코드 : " + _selectMasterItem.CLSS_TP_CD + ")";
            detailDialog.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            detailDialog.Owner = Application.Current.MainWindow;
            detailDialog.BorderEffect = BorderEffect.Default;
            detailDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            detailDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            bool isDialog = (bool)detailDialog.ShowDialog();
            if (isDialog)
            {
                DtlRefresh(detailDialog.resultDomain.CLSS_CD);
                //SelectedDetailItem = SelectedDetailViewList.Where(x => x.CLSS_CD.Equals(detailDialog.resultDomain.CLSS_CD)).FirstOrDefault<SystemCodeVo>();
            }
        }

        //public ICommand DetailDelDialogCommand
        //{
        //    get
        //    {
        //        if (detailDelDialogCommand == null)
        //            detailDelDialogCommand = new DelegateCommand(DelDetailContact);
        //        return detailDelDialogCommand;
        //    }
        //}

        //public void DelDetailContact()
        //{
        //    try
        //    {
        //        SystemCodeVo delDao = this.SelectedDetailItem;
        //        if (delDao != null)
        //        {
        //            if (delDao.DELT_FLG.Equals("미사용"))
        //            {
        //                WinUIMessageBox.Show("현재 미사용 코드 입니다.", "[삭제 - Detail]" + title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
        //                return;
        //            }
        //            //
        //            MessageBoxResult result = WinUIMessageBox.Show(delDao.CLSS_CD + " 정말로 삭제 하시겠습니까?", "[삭제 - Detail]" + title, MessageBoxButton.YesNo, MessageBoxImage.Question);
        //            if (result == MessageBoxResult.Yes)
        //            {
        //                delDao.DELT_FLG = "Y";
        //                delDao.USR_ID = SystemProperties.USER;
        //                //codeClient.UpdateDetailCode(delDao);
        //                WinUIMessageBox.Show("삭제가 완료되었습니다.", "[삭제 - Detail]" + title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
        //                //
        //                delDao.DELT_FLG = "미사용";
        //            }
        //        }
        //    }
        //    catch (System.Exception eLog)
        //    {
        //        WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
        //         return;
        //    }
        //}
        #endregion


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

        //
        private bool? _isD_UPDATE = false;
        public bool? isD_UPDATE
        {
            get { return _isD_UPDATE; }
            set { SetProperty(ref _isD_UPDATE, value, () => isD_UPDATE); }
        }

        private bool? _isD_DELETE = false;
        public bool? isD_DELETE
        {
            get { return _isD_DELETE; }
            set { SetProperty(ref _isD_DELETE, value, () => isD_DELETE); }
        }


        private string _M_SEARCH_TEXT = string.Empty;
        public string M_SEARCH_TEXT
        {
            get { return _M_SEARCH_TEXT; }
            set { SetProperty(ref _M_SEARCH_TEXT, value, () => M_SEARCH_TEXT); }
        }

        private string _D_SEARCH_TEXT = string.Empty;
        public string D_SEARCH_TEXT
        {
            get { return _D_SEARCH_TEXT; }
            set { SetProperty(ref _D_SEARCH_TEXT, value, () => D_SEARCH_TEXT); }
        }
    }
}
