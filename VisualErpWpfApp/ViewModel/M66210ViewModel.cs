using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Man;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Windows;
using AquilaErpWpfApp3.Util;

namespace AquilaErpWpfApp3.ViewModel
{
    //표준 공정 관리
    public sealed class M66210ViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private string _title = "공정제품맵핑";
        //private static ManServiceClient manClient = SystemProperties.ManClient;

        private IList<ManVo> selectedMasterViewList;
        private IList<ManVo> selectedPopupViewList;
        private IList<ManVo> selectedDetailViewList;

        //Master Dialog
        //private ICommand masterSearchDialogCommand;
        //private ICommand masterNewDialogCommand;
        //private ICommand masterEditDialogCommand;
        //private ICommand masterDelDialogCommand;
        //
        //private M6611MasterDialog masterDialog;

       
        public M66210ViewModel() 
        {
            Refresh();
        }

        [Command]
       public async void Refresh()
        {
            try
            {
                //제품 목록
                using (HttpResponseMessage responseX = await SystemProperties.PROGRAM_HTTP.PostAsync("m66210/mst", new StringContent(JsonConvert.SerializeObject(new ManVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD, CRE_USR_ID = SystemProperties.USER, UPD_USR_ID = SystemProperties.USER }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (responseX.IsSuccessStatusCode)
                    {
                        this.SelectedMasterViewList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await responseX.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
                    }
                    //SelectedMasterViewList = manClient.M6611SelectMaster(new ManVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD });
                    if (SelectedMasterViewList.Count > 0)
                    {
                        //isM_UPDATE = true;
                        //isM_DELETE = true;
                        SelectedMasterItem = SelectedMasterViewList[0];
                    }
                    else
                    {
                        isM_UPDATE = false;
                        isM_DELETE = false;

                    }
                }
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }

        #region Functon (Master Add, Edit, Del)
        public IList<ManVo> SelectedMasterViewList
        {
            get { return selectedMasterViewList; }
            private set { SetProperty(ref selectedMasterViewList, value, () => SelectedMasterViewList); }
        }

        ManVo _selectMasterItem;
        public ManVo SelectedMasterItem
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

        async void OnSelectedMasterItemChanged()
        {
            try
            {
                isM_UPDATE = false;
                isM_DELETE = false;

                if (SelectedMasterItem == null) { return; }

                //공정 유형
                using (HttpResponseMessage responseX = await SystemProperties.PROGRAM_HTTP.PostAsync("m66210/popup", new StringContent(JsonConvert.SerializeObject(SelectedMasterItem), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (responseX.IsSuccessStatusCode)
                    {
                        this.SelectedPopupViewList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await responseX.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
                    }
                    if (SelectedPopupViewList.Count > 0)
                    {
                        isM_UPDATE = true;
                        //isM_DELETE = true;

                        SelectedPopupItem = SelectedPopupViewList[0];
                    }
                }


                using (HttpResponseMessage responseX = await SystemProperties.PROGRAM_HTTP.PostAsync("m66210/dtl", new StringContent(JsonConvert.SerializeObject(SelectedMasterItem), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (responseX.IsSuccessStatusCode)
                    {
                        this.SelectedDetailViewList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await responseX.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
                    }
                    //
                    if (SelectedDetailViewList.Count > 0)
                    {
                        //isM_UPDATE = true;
                        isM_DELETE = true;

                        SelectedDetailItem = SelectedDetailViewList[0];
                    }
                }
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }

        public IList<ManVo> SelectedDetailViewList
        {
            get { return selectedDetailViewList; }
            private set { SetProperty(ref selectedDetailViewList, value, () => SelectedDetailViewList); }
        }

        ManVo _selectDetailItem;
        public ManVo SelectedDetailItem
        {
            get
            {
                return _selectDetailItem;
            }
            set
            {
                if (value != null)
                {
                    SetProperty(ref _selectDetailItem, value, () => SelectedDetailItem);
                }
            }
        }


        //
        public IList<ManVo> SelectedPopupViewList
        {
            get { return selectedPopupViewList; }
            private set { SetProperty(ref selectedPopupViewList, value, () => SelectedPopupViewList); }
        }

        ManVo _selectPopupItem;
        public ManVo SelectedPopupItem
        {
            get
            {
                return _selectPopupItem;
            }
            set
            {
                if (value != null)
                {
                    SetProperty(ref _selectPopupItem, value, () => SelectedPopupItem);
                }
            }
        }


        //public ICommand MasterSearchDialogCommand
        //{
        //    get
        //    {
        //        if (masterSearchDialogCommand == null)
        //            masterSearchDialogCommand = new DelegateCommand(Refresh);
        //        return masterSearchDialogCommand;
        //    }
        //}

        //public void SearchMasterContact()
        //{
        //    ProgressWindow program = new ProgressWindow();
        //    try
        //    {
        //        ThreadStart start = delegate()
        //        {
        //            program.Dispatcher.Invoke(DispatcherPriority.Normal,(Action)(() => 
        //            {
        //                program.Show();
        //                Thread.Sleep(100);
        //                SelectedMasterViewList = codeClient.SelectMasterCode(new SystemCodeVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD });
        //                OnSelectedMasterItemChanged();
        //                program.Close();
        //            }));
        //        };
        //        new Thread(start).Start();
        //    }
        //    catch (System.Exception eLog)
        //    {
        //    //    program.Close();
        //        WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]표준 공정 관리", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
        //        return;
        //    }
        //}

        //public ICommand MasterNewDialogCommand
        //{
        //    get
        //    {
        //        if (masterNewDialogCommand == null)
        //            masterNewDialogCommand = new DelegateCommand(NewMasterContact);
        //        return masterNewDialogCommand;
        //    }
        //}
        [Command]
        public async void NewMasterContact()
        {
            if (SelectedPopupItem == null) { return; }

            int _Num = 0;
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m66210/mst/i", new StringContent(JsonConvert.SerializeObject(SelectedPopupItem), System.Text.Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    if (int.TryParse(result, out _Num) == false)
                    {
                        //실패
                        WinUIMessageBox.Show(result, _title, MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    //OnSelectedMasterItemChanged();
                    using (HttpResponseMessage responseX = await SystemProperties.PROGRAM_HTTP.PostAsync("m66210/dtl", new StringContent(JsonConvert.SerializeObject(SelectedMasterItem), System.Text.Encoding.UTF8, "application/json")))
                    {
                        if (responseX.IsSuccessStatusCode)
                        {
                            this.SelectedDetailViewList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await responseX.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
                        }
                        //
                        if (SelectedDetailViewList.Count > 0)
                        {
                            //isM_UPDATE = true;
                            isM_DELETE = true;

                            SelectedDetailItem = SelectedDetailViewList[SelectedDetailViewList.Count -1];
                        }
                        else
                        {
                            //isM_UPDATE = false;
                            isM_DELETE = false;
                        }
                    }
                    //성공
                    //WinUIMessageBox.Show("완료 되었습니다", _title, MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        //public ICommand MasterEditDialogCommand
        //{
        //    get
        //    {
        //        if (masterEditDialogCommand == null)
        //            masterEditDialogCommand = new DelegateCommand(EditMasterContact);
        //        return masterEditDialogCommand;
        //    }
        //}

        //[Command]
        //public void EditMasterContact()
        //{
        //    //if (this._selectMasterItem == null) { return; }
        //    //ManVo editDao = this._selectMasterItem;
        //    //if (editDao != null)
        //    //{
        //    //    masterDialog = new M6611MasterDialog(editDao);
        //    //    masterDialog.Title = _title + " - 수정";
        //    //    masterDialog.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        //    //    masterDialog.Owner = Application.Current.MainWindow;
        //    //    masterDialog.BorderEffect = BorderEffect.Default;
        //    //    masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
        //    //    masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
        //    //    bool isDialog = (bool)masterDialog.ShowDialog();
        //    //    if (isDialog)
        //    //    {
        //    //        Refresh(masterDialog.resultDomain.ROUT_CD);

        //    //        //if (masterDialog.IsEdit == false)
        //    //        //{
        //    //        //    Refresh();
        //    //        //}
        //    //    }
        //    //}
        //}

        //public ICommand MasterDelDialogCommand
        //{
        //    get
        //    {
        //        if (masterDelDialogCommand == null)
        //            masterDelDialogCommand = new DelegateCommand(DelMasterContact);
        //        return masterDelDialogCommand;
        //    }
        //}


        [Command]
        public async void DelMasterContact()
        {
            try
            {
                if (SelectedDetailItem == null) { return; }
                //ManVo delDao = this._selectMasterItem;
                //if (delDao != null)
                //{
                //    MessageBoxResult result = WinUIMessageBox.Show(delDao.ROUT_CD + " 정말로 삭제 하시겠습니까?", _title, MessageBoxButton.YesNo, MessageBoxImage.Question);
                //    if (result == MessageBoxResult.Yes)
                //    {
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m66210/mst/d", new StringContent(JsonConvert.SerializeObject(SelectedDetailItem), System.Text.Encoding.UTF8, "application/json")))
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


                        //OnSelectedMasterItemChanged();
                        using (HttpResponseMessage responseX = await SystemProperties.PROGRAM_HTTP.PostAsync("m66210/dtl", new StringContent(JsonConvert.SerializeObject(SelectedMasterItem), System.Text.Encoding.UTF8, "application/json")))
                        {
                            if (responseX.IsSuccessStatusCode)
                            {
                                this.SelectedDetailViewList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await responseX.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
                            }
                            //
                            if (SelectedDetailViewList.Count > 0)
                            {
                                //isM_UPDATE = true;
                                isM_DELETE = true;

                                SelectedDetailItem = SelectedDetailViewList[SelectedDetailViewList.Count - 1];
                            }
                            else
                            {
                                //isM_UPDATE = false;
                                isM_DELETE = false;
                            }
                        }

                        //성공
                        //WinUIMessageBox.Show("삭제가 완료되었습니다.", _title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                    }
                }
                //    }
                //}
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }
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

        private string _M_SEARCH_TEXT = string.Empty;
        public string M_SEARCH_TEXT
        {
            get { return _M_SEARCH_TEXT; }
            set { SetProperty(ref _M_SEARCH_TEXT, value, () => M_SEARCH_TEXT); }
        }
    }
}
