using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Code;
using ModelsLibrary.Man;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Media;
using AquilaErpWpfApp3.Util;
using AquilaErpWpfApp3.View.M.Dialog;

namespace AquilaErpWpfApp3.ViewModel
{
    //표준 공정 관리
    public sealed class M66212ViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private string _title = "(BOM)공정제품연결";
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
        private M66212MasterDialog masterDialog;


        public M66212ViewModel() 
        {
            SYSTEM_CODE_VO();
            // -- Refresh();
        }

        [Command]
       public async void Refresh( string _ITM_CD = null )
        {
            try
            {
                DXSplashScreen.Show<ProgressWindow>();
                //MessageBox.Show(M_SEARCH);

                //제품 목록
                using (HttpResponseMessage responseX = await SystemProperties.PROGRAM_HTTP.PostAsync("m66212/mst", new StringContent(JsonConvert.SerializeObject(new ManVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD, CRE_USR_ID = SystemProperties.USER, UPD_USR_ID = SystemProperties.USER  }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (responseX.IsSuccessStatusCode)
                    {
                        this.SelectedMasterViewList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await responseX.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
                    }
                    //SelectedMasterViewList = manClient.M6611SelectMaster(new ManVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD });
                    if (SelectedMasterViewList.Count > 0)
                    {

                        //T_SEARCH = (string.IsNullOrEmpty(M_SEARCH) ? null : M_SEARCH);

                        if (string.IsNullOrEmpty(_ITM_CD))
                        {
                            SelectedMasterItem = SelectedMasterViewList[0];
                        }
                        else
                        {
                            SelectedMasterItem = SelectedMasterViewList.Where(x => x.ITM_CD.Equals(_ITM_CD)).LastOrDefault<ManVo>();
                        }

                        //isM_UPDATE = true;
                        //isM_DELETE = true;
                        //SelectedMasterItem = SelectedMasterViewList[0];
                    }
                    else
                    {
                        isM_UPDATE = false;
                        isM_DELETE = false;

                    }
                }

                DXSplashScreen.Close();
            }
            catch (System.Exception eLog)
            {
                if (DXSplashScreen.IsActive == true)
                {
                    DXSplashScreen.Close();
                }
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


                M_SEARCH_TEXT = SelectedMasterItem.N1ST_ITM_GRP_NM;
                ////
                ////투입 자재
                //using (HttpResponseMessage responseX = await SystemProperties.PROGRAM_HTTP.PostAsync("s141", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD, ITM_GRP_CLSS_CD = "M" /*N1ST_ITM_GRP_CD = SelectedMasterItem.N1ST_ITM_GRP_CD*/ }), System.Text.Encoding.UTF8, "application/json")))
                //{
                //    if (responseX.IsSuccessStatusCode)
                //    {
                //        this.MtrlItmNnList = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await responseX.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                //    }
                //}


                //공정 유형
                using (HttpResponseMessage responseX = await SystemProperties.PROGRAM_HTTP.PostAsync("m66212/popup", new StringContent(JsonConvert.SerializeObject(SelectedMasterItem), System.Text.Encoding.UTF8, "application/json")))
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


                using (HttpResponseMessage responseX = await SystemProperties.PROGRAM_HTTP.PostAsync("m66212/dtl", new StringContent(JsonConvert.SerializeObject(SelectedMasterItem), System.Text.Encoding.UTF8, "application/json")))
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


        //
        private IList<SystemCodeVo> prodClssCdList;
        public IList<SystemCodeVo> ProdClssCdList
        {
            get { return prodClssCdList; }
            private set { SetProperty(ref prodClssCdList, value, () => ProdClssCdList); }
        }

        SystemCodeVo _prodClssCdItem;
        public SystemCodeVo ProdClssCdItem
        {
            get
            {
                return _prodClssCdItem;
            }
            set
            {
                if (value != null)
                {
                    SetProperty(ref _prodClssCdItem, value, () => ProdClssCdItem);
                }
            }
        }


        private IList<SystemCodeVo> _mtrlItmNm;
        public IList<SystemCodeVo> MtrlItmNnList
        {
            get { return _mtrlItmNm; }
            private set { SetProperty(ref _mtrlItmNm, value, () => MtrlItmNnList); }
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
        public void AddMasterContact()
        {
            if (this.SelectedMasterItem == null) { return; }

            masterDialog = new M66212MasterDialog(SelectedMasterItem);
            masterDialog.Title = _title + " - " + SelectedMasterItem.ITM_CD + " / " + SelectedMasterItem.ITM_NM;
            masterDialog.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            masterDialog.Owner = Application.Current.MainWindow;
            masterDialog.BorderEffect = BorderEffect.Default;
            masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            bool isDialog = (bool)masterDialog.ShowDialog();
            if (isDialog)
            {
                Refresh(SelectedMasterItem.ITM_CD);

                //if (masterDialog.IsEdit == false)
                //{
                //    Refresh();
                //}
            }
        }


        [Command]
        public async void NewMasterContact()
        {
            if (SelectedPopupItem == null) { return; }

            //생산구분
            SelectedPopupItem.PROD_CLSS_CD = ProdClssCdItem.CLSS_CD;
            SelectedPopupItem.PROD_CLSS_NM = ProdClssCdItem.CLSS_DESC;

            int _Num = 0;
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m66212/dtl/i", new StringContent(JsonConvert.SerializeObject(SelectedPopupItem), System.Text.Encoding.UTF8, "application/json")))
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
                    using (HttpResponseMessage responseX = await SystemProperties.PROGRAM_HTTP.PostAsync("m66212/dtl", new StringContent(JsonConvert.SerializeObject(SelectedMasterItem), System.Text.Encoding.UTF8, "application/json")))
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

                    //BOM / ITM 동기화
                    using (HttpResponseMessage responseZ = await SystemProperties.PROGRAM_HTTP.PostAsync("m66212/dtl/sync", new StringContent(JsonConvert.SerializeObject(new ManVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD, CRE_USR_ID = SystemProperties.USER, UPD_USR_ID = SystemProperties.USER }), System.Text.Encoding.UTF8, "application/json")))
                    {
                        if (responseZ.IsSuccessStatusCode)
                        {
                            if (int.TryParse(result, out _Num) == false)
                            {
                                //실패
                                WinUIMessageBox.Show(result, _title, MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
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
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m66212/dtl/d", new StringContent(JsonConvert.SerializeObject(SelectedDetailItem), System.Text.Encoding.UTF8, "application/json")))
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
                        using (HttpResponseMessage responseX = await SystemProperties.PROGRAM_HTTP.PostAsync("m66212/dtl", new StringContent(JsonConvert.SerializeObject(SelectedMasterItem), System.Text.Encoding.UTF8, "application/json")))
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

                        //BOM / ITM 동기화
                        using (HttpResponseMessage responseZ = await SystemProperties.PROGRAM_HTTP.PostAsync("m66212/dtl/sync", new StringContent(JsonConvert.SerializeObject(new ManVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD, CRE_USR_ID = SystemProperties.USER, UPD_USR_ID = SystemProperties.USER }), System.Text.Encoding.UTF8, "application/json")))
                        {
                            if (responseZ.IsSuccessStatusCode)
                            {
                                if (int.TryParse(resultMsg, out _Num) == false)
                                {
                                    //실패
                                    WinUIMessageBox.Show(resultMsg, _title, MessageBoxButton.OK, MessageBoxImage.Error);
                                    return;
                                }
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


        [Command]
        public async void UpMasterContact()
        {
            try
            {
                if (SelectedMasterItem == null) { return; }
                if (SelectedDetailItem == null) { return; }

                //ManVo resultVo;
                int _Num = 0;
                string _ROUT_CD = SelectedDetailItem.ROUT_CD;

                int _FindIdx = SelectedDetailViewList.IndexOf(SelectedDetailItem);
                if (_FindIdx <= 0)
                {
                    SelectedDetailItem.ROUT_SEQ = 1;
                }
                else
                {
                    int? _Temp = SelectedDetailViewList[_FindIdx].ROUT_SEQ;

                    SelectedDetailViewList[_FindIdx].ROUT_SEQ = SelectedDetailViewList[_FindIdx - 1].ROUT_SEQ;
                    SelectedDetailViewList[_FindIdx].CRE_USR_ID = SystemProperties.USER;
                    SelectedDetailViewList[_FindIdx].UPD_USR_ID = SystemProperties.USER;
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m66212/dtl/u", new StringContent(JsonConvert.SerializeObject(SelectedDetailViewList[_FindIdx]), System.Text.Encoding.UTF8, "application/json")))
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
                        }
                    }
 

                    SelectedDetailViewList[_FindIdx - 1].ROUT_SEQ = _Temp;
                    SelectedDetailViewList[_FindIdx - 1].CRE_USR_ID = SystemProperties.USER;
                    SelectedDetailViewList[_FindIdx - 1].UPD_USR_ID = SystemProperties.USER;
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m66212/dtl/u", new StringContent(JsonConvert.SerializeObject(SelectedDetailViewList[_FindIdx - 1]), System.Text.Encoding.UTF8, "application/json")))
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
                        }
                    }
                }

                //OnSelectedMasterItemChanged
                //isM_UPDATE = false;
                isM_DELETE = false;
                using (HttpResponseMessage responseX = await SystemProperties.PROGRAM_HTTP.PostAsync("m66212/dtl", new StringContent(JsonConvert.SerializeObject(SelectedMasterItem), System.Text.Encoding.UTF8, "application/json")))
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

                        SelectedDetailItem = SelectedDetailViewList.Where<ManVo>(x => x.ROUT_CD.Equals(_ROUT_CD)).FirstOrDefault<ManVo>();
                        //SelectedDetailItem = SelectedDetailViewList[0];
                    }
                }
            }
            catch
            {
                return;
            }
        }

        [Command]
        public async void DownMasterContact()
        {
            try
            {
                if (SelectedMasterItem == null) { return; }
                if (SelectedDetailItem == null) { return; }

                //ManVo resultVo;
                int _Num = 0;
                string _ROUT_CD = SelectedDetailItem.ROUT_CD;

                int _FindIdx = SelectedDetailViewList.IndexOf(SelectedDetailItem);
                if (_FindIdx >= SelectedDetailViewList.Count - 1)
                {
                    SelectedDetailItem.ROUT_SEQ = SelectedDetailViewList.Count;
                }
                else
                {
                    int? _Temp = SelectedDetailViewList[_FindIdx].ROUT_SEQ;

                    SelectedDetailViewList[_FindIdx].ROUT_SEQ = SelectedDetailViewList[_FindIdx + 1].ROUT_SEQ;
                    SelectedDetailViewList[_FindIdx].CRE_USR_ID = SystemProperties.USER;
                    SelectedDetailViewList[_FindIdx].UPD_USR_ID = SystemProperties.USER;
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m66212/dtl/u", new StringContent(JsonConvert.SerializeObject(SelectedDetailViewList[_FindIdx]), System.Text.Encoding.UTF8, "application/json")))
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
                        }
                    }
                 
                    SelectedDetailViewList[_FindIdx + 1].ROUT_SEQ = _Temp;
                    SelectedDetailViewList[_FindIdx + 1].CRE_USR_ID = SystemProperties.USER;
                    SelectedDetailViewList[_FindIdx + 1].UPD_USR_ID = SystemProperties.USER;
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m66212/dtl/u", new StringContent(JsonConvert.SerializeObject(SelectedDetailViewList[_FindIdx + 1]), System.Text.Encoding.UTF8, "application/json")))
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
                        }
                    }
                }


                //OnSelectedMasterItemChanged
                //isM_UPDATE = false;
                isM_DELETE = false;
                using (HttpResponseMessage responseX = await SystemProperties.PROGRAM_HTTP.PostAsync("m66212/dtl", new StringContent(JsonConvert.SerializeObject(SelectedMasterItem), System.Text.Encoding.UTF8, "application/json")))
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

                        SelectedDetailItem = SelectedDetailViewList.Where<ManVo>(x => x.ROUT_CD.Equals(_ROUT_CD)).FirstOrDefault<ManVo>();
                        //SelectedDetailItem = SelectedDetailViewList[0];
                    }
                }

            }
            catch
            {
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


   
        public async void SYSTEM_CODE_VO()
        {
            //SL_AREA_LIST = SystemProperties.SYSTEM_CODE_VO("L-002");
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "L-005"))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.ProdClssCdList = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                    if (this.ProdClssCdList.Count > 0)
                    {
                        this.ProdClssCdItem = this.ProdClssCdList[0];
                    }
                }
            }

            //
            //투입 자재
            using (HttpResponseMessage responseX = await SystemProperties.PROGRAM_HTTP.PostAsync("s141", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD, ITM_GRP_CLSS_CD = "M" /*N1ST_ITM_GRP_CD = SelectedMasterItem.N1ST_ITM_GRP_CD*/ }), System.Text.Encoding.UTF8, "application/json")))
            {
                if (responseX.IsSuccessStatusCode)
                {
                    this.MtrlItmNnList = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await responseX.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                }
            }


            Refresh();
        }

    }
}
