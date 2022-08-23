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
using DevExpress.Xpf.Grid;

namespace AquilaErpWpfApp3.ViewModel
{
    //표준 공정 관리
    public sealed class M66213ViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private string _title = "BOM등록";
        //private static ManServiceClient manClient = SystemProperties.ManClient;

        private IList<ManVo> selectedMasterViewList;
        private IList<ManVo> selectedTreeViewList;
        private IList<ManVo> selectedDetailViewList;

        private M66213MasterDialog masterDialog;
        private M66213DetailDialog detailDialog;
        private M66213CopyDialog copyDialog;


        public M66213ViewModel() 
        {
            SYSTEM_CODE_VO();
            //Refresh();
        }

        [Command]
       public async void Refresh( string _ITM_CD = null )
        {
            try
            {

                //if (M_MDL == null)
                //{
                //    return;
                //}

                //DXSplashScreen.Show<ProgressWindow>();
                //MessageBox.Show(M_SEARCH);
                SelectedTreeItem = null;

                //제품 목록
                using (HttpResponseMessage responseX = await SystemProperties.PROGRAM_HTTP.PostAsync("m66213/mst", new StringContent(JsonConvert.SerializeObject(new ManVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD, CRE_USR_ID = SystemProperties.USER, UPD_USR_ID = SystemProperties.USER, MDL_CD = M_MDL?.MDL_CD }), System.Text.Encoding.UTF8, "application/json")))
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

                        this.M_SEARCH_TEXT = M_MDL.MDL_CD + " / " + M_MDL.MDL_NM + " / " + SelectedMasterItem?.ITM_NM;

                        isM_UPDATE = true;
                        isM_DELETE = true;
                        //SelectedMasterItem = SelectedMasterViewList[0];



                        //Dtl
                        using (HttpResponseMessage responseY = await SystemProperties.PROGRAM_HTTP.PostAsync("m66213/dtl", new StringContent(JsonConvert.SerializeObject(SelectedMasterItem), System.Text.Encoding.UTF8, "application/json")))
                        {
                            if (responseY.IsSuccessStatusCode)
                            {
                                this.SelectedDetailViewList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await responseY.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
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
                    else
                    {
                        isM_UPDATE = false;
                        isM_DELETE = false;

                    }
                }

                //DXSplashScreen.Close();
            }
            catch (System.Exception eLog)
            {
               //DXSplashScreen.Close();
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


                //SelectedTreeItem = null;


                this.M_SEARCH_TEXT = M_MDL.MDL_CD + " / " + M_MDL.MDL_NM + " / " + SelectedMasterItem?.ITM_NM;

                //M_SEARCH_TEXT = SelectedMasterItem.N1ST_ITM_GRP_NM;
                ////
                ////투입 자재
                //using (HttpResponseMessage responseX = await SystemProperties.PROGRAM_HTTP.PostAsync("s141", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD, ITM_GRP_CLSS_CD = "M" /*N1ST_ITM_GRP_CD = SelectedMasterItem.N1ST_ITM_GRP_CD*/ }), System.Text.Encoding.UTF8, "application/json")))
                //{
                //    if (responseX.IsSuccessStatusCode)
                //    {
                //        this.MtrlItmNnList = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await responseX.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                //    }
                //}


                //Tree
                using (HttpResponseMessage responseX = await SystemProperties.PROGRAM_HTTP.PostAsync("m66213/tree", new StringContent(JsonConvert.SerializeObject(SelectedMasterItem), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (responseX.IsSuccessStatusCode)
                    {
                        this.SelectedTreeViewList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await responseX.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
                    }
                    if (SelectedTreeViewList.Count > 0)
                    {
                        isM_UPDATE = true;
                        //isM_DELETE = true;

                        SelectedTreeItem = SelectedTreeViewList[0];
                    }
                }

                ////Dtl
                //using (HttpResponseMessage responseX = await SystemProperties.PROGRAM_HTTP.PostAsync("m66213/dtl", new StringContent(JsonConvert.SerializeObject(SelectedMasterItem), System.Text.Encoding.UTF8, "application/json")))
                //{
                //    if (responseX.IsSuccessStatusCode)
                //    {
                //        this.SelectedDetailViewList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await responseX.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
                //    }
                //    //
                //    if (SelectedDetailViewList.Count > 0)
                //    {
                //        //isM_UPDATE = true;
                //        isM_DELETE = true;

                //        SelectedDetailItem = SelectedDetailViewList[0];
                //    }
                //}
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
        public IList<ManVo> SelectedTreeViewList
        {
            get { return selectedTreeViewList; }
            private set { SetProperty(ref selectedTreeViewList, value, () => SelectedTreeViewList); }
        }

        ManVo _selectTreeItem;
        public ManVo SelectedTreeItem
        {
            get
            {
                return _selectTreeItem;
            }
            set
            {
                if (value != null)
                {
                    SetProperty(ref _selectTreeItem, value, () => SelectedTreeItem);
                }
            }
        }


        //
        private IList<ManVo> _MdlList;
        public IList<ManVo> MdlList
        {
            get { return _MdlList; }
            private set { SetProperty(ref _MdlList, value, () => MdlList); }
        }

        ManVo _M_MDL;
        public ManVo M_MDL
        {
            get
            {
                return _M_MDL;
            }
            set
            {
                if (value != null)
                {
                    SetProperty(ref _M_MDL, value, () => M_MDL);
                }
            }
        }


        [Command]
        public async void NewMasterContact(string _obj)
        {

            if (_obj.Equals("A"))
            {
                if (SelectedMasterItem == null) { return; }

                //SelectedMasterItem.ASSY_CD = SelectedTreeItem.CMPO_CD;
                //SelectedMasterItem.ASSY_NM = SelectedTreeItem.CMPO_NM;
                SelectedMasterItem.ASSY_CD = (SelectedTreeItem == null ? SelectedMasterItem.ITM_CD : SelectedTreeItem.CMPO_CD);
                SelectedMasterItem.ASSY_NM = (SelectedTreeItem == null ? SelectedMasterItem.ITM_NM : SelectedTreeItem.CMPO_NM);

                SelectedMasterItem.ROUT_ITM_CD = SelectedMasterItem.ITM_CD;

                //
                //
                string _index = SelectedTreeItem?.CMPO_NM;

                //
                masterDialog = new M66213MasterDialog(SelectedMasterItem, false);
                masterDialog.Title = _title + " - BOM 추가";
                masterDialog.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                masterDialog.Owner = Application.Current.MainWindow;
                masterDialog.BorderEffect = BorderEffect.Default;
                masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
                masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
                bool isDialog = (bool)masterDialog.ShowDialog();
                if (isDialog)
                {
                    //OnSelectedMasterItemChanged();

                    using (HttpResponseMessage responseX = await SystemProperties.PROGRAM_HTTP.PostAsync("m66213/tree", new StringContent(JsonConvert.SerializeObject(SelectedMasterItem), System.Text.Encoding.UTF8, "application/json")))
                    {
                        if (responseX.IsSuccessStatusCode)
                        {
                            this.SelectedTreeViewList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await responseX.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
                            
                            if (SelectedTreeViewList.Count > 0)
                            {
                                isM_UPDATE = true;
                                //isM_DELETE = true;

                                SelectedTreeItem = SelectedTreeViewList.Where(x => (x.CMPO_NM).Equals(_index)).LastOrDefault<ManVo>();
                            }
                        }
                    }

                }
            }
            else if(_obj.Equals("B"))
            {
                if (SelectedDetailItem == null) { return; }

                using (HttpResponseMessage responseX = await SystemProperties.PROGRAM_HTTP.PostAsync("m66213/tree/mst", new StringContent(JsonConvert.SerializeObject(SelectedDetailItem), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (responseX.IsSuccessStatusCode)
                    {
                        ManVo _manVo = JsonConvert.DeserializeObject<ManVo>(await responseX.Content.ReadAsStringAsync());

                        //
                        _manVo.MDL_CD = SelectedMasterItem.MDL_CD;
                        _manVo.MDL_NM = SelectedMasterItem.MDL_NM;
                        _manVo.ROUT_ITM_CD = SelectedMasterItem.ITM_CD;
                        _manVo.ITM_CD = SelectedMasterItem.ITM_CD;
                        _manVo.ITM_NM = SelectedMasterItem.ITM_NM;
                        _manVo.UOM_NM = SelectedMasterItem.UOM_NM;

                        _manVo.MSEQ = SelectedMasterItem.MSEQ;
                        _manVo.UPSEQ = SelectedTreeItem.UPSEQ;
                        //_manVo.ASSY_NM = SelectedTreeItem.CMPO_NM;
                        _manVo.ASSY_CD = (SelectedTreeItem == null ? SelectedMasterItem.ITM_CD : SelectedTreeItem.CMPO_CD);
                        _manVo.ASSY_NM = (SelectedTreeItem == null ? SelectedMasterItem.ITM_NM : SelectedTreeItem.CMPO_NM);

                        //
                        //
                        string _index = SelectedTreeItem?.CMPO_NM;
                        //
                        masterDialog = new M66213MasterDialog(_manVo, false);
                        masterDialog.Title = _title + " - BOM 추가";
                        masterDialog.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                        masterDialog.Owner = Application.Current.MainWindow;
                        masterDialog.BorderEffect = BorderEffect.Default;
                        masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
                        masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
                        bool isDialog = (bool)masterDialog.ShowDialog();
                        if (isDialog)
                        {
                            //OnSelectedMasterItemChanged();
                            using (HttpResponseMessage responseY = await SystemProperties.PROGRAM_HTTP.PostAsync("m66213/tree", new StringContent(JsonConvert.SerializeObject(SelectedMasterItem), System.Text.Encoding.UTF8, "application/json")))
                            {
                                if (responseY.IsSuccessStatusCode)
                                {
                                    this.SelectedTreeViewList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await responseY.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();

                                    if (SelectedTreeViewList.Count > 0)
                                    {
                                        isM_UPDATE = true;
                                        //isM_DELETE = true;

                                        SelectedTreeItem = SelectedTreeViewList.Where(x => (x.CMPO_NM).Equals(_index)).LastOrDefault<ManVo>();
                                    }
                                }
                            }
                        }
                    }
                }


            }
        }

        [Command]
        public async void EditMasterContact()
        {
            if (SelectedTreeItem == null) { return; }

            using (HttpResponseMessage responseX = await SystemProperties.PROGRAM_HTTP.PostAsync("m66213/tree/mst", new StringContent(JsonConvert.SerializeObject(new ManVo() { SEQ = SelectedTreeItem.SEQ, CHNL_CD = SystemProperties.USER_VO.CHNL_CD, CMPO_CD = SelectedTreeItem.GBN }), System.Text.Encoding.UTF8, "application/json")))
            {
                if (responseX.IsSuccessStatusCode)
                {
                    ManVo _manVo = JsonConvert.DeserializeObject<ManVo>(await responseX.Content.ReadAsStringAsync());

                    //
                    _manVo.MDL_CD = SelectedMasterItem.MDL_CD;
                    _manVo.MDL_NM = SelectedMasterItem.MDL_NM;
                    _manVo.ROUT_ITM_CD = SelectedMasterItem.ITM_CD;
                    _manVo.ITM_CD = SelectedMasterItem.ITM_CD;
                    _manVo.UOM_NM = SelectedMasterItem.UOM_NM;
                    //_manVo.CMPO_CD = SelectedTreeItem.GBN;
                    //_manVo.ASSY_CD = SelectedTreeItem.CMPO_CD;
                    //_manVo.ASSY_NM = SelectedTreeItem.CMPO_NM;

                    //string _index = SelectedTreeItem.CMPO_NM;
                    //
                    masterDialog = new M66213MasterDialog(_manVo, true);
                    masterDialog.Title = _title + " - BOM 수정";
                    masterDialog.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    masterDialog.Owner = Application.Current.MainWindow;
                    masterDialog.BorderEffect = BorderEffect.Default;
                    masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
                    masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
                    bool isDialog = (bool)masterDialog.ShowDialog();
                    if (isDialog)
                    {
                        //OnSelectedMasterItemChanged();

                        //SelectedTreeItem = SelectedTreeViewList.Where(x => (x.CMPO_NM).Equals(_index)).LastOrDefault<ManVo>();
                    }
                }
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


        [Command]
        public async void DelMasterContact()
        {
            try
            {
                if (SelectedTreeItem == null) { return; }

                ManVo delDao = SelectedTreeItem;
                if (delDao != null)
                {
                    MessageBoxResult result = WinUIMessageBox.Show(delDao.CMPO_CD + "(" + delDao.CMPO_NM + ") 정말로 삭제 하시겠습니까?", _title, MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m66213/mst/d", new StringContent(JsonConvert.SerializeObject(new ManVo() { SEQ = delDao.SEQ, ROUT_ITM_CD = SelectedMasterItem.ITM_CD, CMPO_CD = delDao.CMPO_CD, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
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
                            }
                            OnSelectedMasterItemChanged();
                            WinUIMessageBox.Show("완료 되었습니다", _title, MessageBoxButton.OK, MessageBoxImage.Information);
                            return;
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


        [Command]
        public async void IntoItem()
        {
            try
            {
                if (SelectedMasterItem == null) { return; }

                detailDialog = new M66213DetailDialog(SelectedMasterItem);
                detailDialog.Title = _title + " - " + SelectedMasterItem.ITM_CD + " / " + SelectedMasterItem.ITM_NM; ;
                detailDialog.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                detailDialog.Owner = Application.Current.MainWindow;
                detailDialog.BorderEffect = BorderEffect.Default;
                detailDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
                detailDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
                bool isDialog = (bool)detailDialog.ShowDialog();
                if (isDialog)
                {
                    Refresh(SelectedMasterItem.ITM_CD);

                    //OnSelectedMasterItemChanged();
                    //SelectedTreeItem = SelectedTreeViewList.Where(x => (x.CMPO_NM).Equals(_index)).LastOrDefault<ManVo>();
                }
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }



        [Command]
        public async void CopyItem()
        {
            try
            {
                copyDialog = new M66213CopyDialog(SelectedMasterItem);
                copyDialog.Title = _title + " - 복사";
                copyDialog.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                copyDialog.Owner = Application.Current.MainWindow;
                copyDialog.BorderEffect = BorderEffect.Default;
                copyDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
                copyDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
                bool isDialog = (bool)copyDialog.ShowDialog();
                if (isDialog)
                {




                }
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }

        //[Command]
        //public async void UpMasterContact()
        //{
        //try
        //{
        //    if (SelectedMasterItem == null) { return; }
        //    if (SelectedDetailItem == null) { return; }

        //    //ManVo resultVo;
        //    int _Num = 0;
        //    string _ROUT_CD = SelectedDetailItem.ROUT_CD;

        //    int _FindIdx = SelectedDetailViewList.IndexOf(SelectedDetailItem);
        //    if (_FindIdx <= 0)
        //    {
        //        SelectedDetailItem.ROUT_SEQ = 1;
        //    }
        //    else
        //    {
        //        int? _Temp = SelectedDetailViewList[_FindIdx].ROUT_SEQ;

        //        SelectedDetailViewList[_FindIdx].ROUT_SEQ = SelectedDetailViewList[_FindIdx - 1].ROUT_SEQ;
        //        SelectedDetailViewList[_FindIdx].CRE_USR_ID = SystemProperties.USER;
        //        SelectedDetailViewList[_FindIdx].UPD_USR_ID = SystemProperties.USER;
        //        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m66212/dtl/u", new StringContent(JsonConvert.SerializeObject(SelectedDetailViewList[_FindIdx]), System.Text.Encoding.UTF8, "application/json")))
        //        {
        //            if (response.IsSuccessStatusCode)
        //            {
        //                string result = await response.Content.ReadAsStringAsync();
        //                if (int.TryParse(result, out _Num) == false)
        //                {
        //                    //실패
        //                    WinUIMessageBox.Show(result, _title, MessageBoxButton.OK, MessageBoxImage.Error);
        //                    return;
        //                }
        //            }
        //        }


        //        SelectedDetailViewList[_FindIdx - 1].ROUT_SEQ = _Temp;
        //        SelectedDetailViewList[_FindIdx - 1].CRE_USR_ID = SystemProperties.USER;
        //        SelectedDetailViewList[_FindIdx - 1].UPD_USR_ID = SystemProperties.USER;
        //        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m66212/dtl/u", new StringContent(JsonConvert.SerializeObject(SelectedDetailViewList[_FindIdx - 1]), System.Text.Encoding.UTF8, "application/json")))
        //        {
        //            if (response.IsSuccessStatusCode)
        //            {
        //                string result = await response.Content.ReadAsStringAsync();
        //                if (int.TryParse(result, out _Num) == false)
        //                {
        //                    //실패
        //                    WinUIMessageBox.Show(result, _title, MessageBoxButton.OK, MessageBoxImage.Error);
        //                    return;
        //                }
        //            }
        //        }
        //    }

        //    //OnSelectedMasterItemChanged
        //    //isM_UPDATE = false;
        //    isM_DELETE = false;
        //    using (HttpResponseMessage responseX = await SystemProperties.PROGRAM_HTTP.PostAsync("m66212/dtl", new StringContent(JsonConvert.SerializeObject(SelectedMasterItem), System.Text.Encoding.UTF8, "application/json")))
        //    {
        //        if (responseX.IsSuccessStatusCode)
        //        {
        //            this.SelectedDetailViewList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await responseX.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
        //        }
        //        //
        //        if (SelectedDetailViewList.Count > 0)
        //        {
        //            //isM_UPDATE = true;
        //            isM_DELETE = true;

        //            SelectedDetailItem = SelectedDetailViewList.Where<ManVo>(x => x.ROUT_CD.Equals(_ROUT_CD)).FirstOrDefault<ManVo>();
        //            //SelectedDetailItem = SelectedDetailViewList[0];
        //        }
        //    }
        //}
        //catch
        //{
        //    return;
        //}
        //}

        //[Command]
        //public async void DownMasterContact()
        //{
        //try
        //{
        //    if (SelectedMasterItem == null) { return; }
        //    if (SelectedDetailItem == null) { return; }

        //    //ManVo resultVo;
        //    int _Num = 0;
        //    string _ROUT_CD = SelectedDetailItem.ROUT_CD;

        //    int _FindIdx = SelectedDetailViewList.IndexOf(SelectedDetailItem);
        //    if (_FindIdx >= SelectedDetailViewList.Count - 1)
        //    {
        //        SelectedDetailItem.ROUT_SEQ = SelectedDetailViewList.Count;
        //    }
        //    else
        //    {
        //        int? _Temp = SelectedDetailViewList[_FindIdx].ROUT_SEQ;

        //        SelectedDetailViewList[_FindIdx].ROUT_SEQ = SelectedDetailViewList[_FindIdx + 1].ROUT_SEQ;
        //        SelectedDetailViewList[_FindIdx].CRE_USR_ID = SystemProperties.USER;
        //        SelectedDetailViewList[_FindIdx].UPD_USR_ID = SystemProperties.USER;
        //        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m66212/dtl/u", new StringContent(JsonConvert.SerializeObject(SelectedDetailViewList[_FindIdx]), System.Text.Encoding.UTF8, "application/json")))
        //        {
        //            if (response.IsSuccessStatusCode)
        //            {
        //                string result = await response.Content.ReadAsStringAsync();
        //                if (int.TryParse(result, out _Num) == false)
        //                {
        //                    //실패
        //                    WinUIMessageBox.Show(result, _title, MessageBoxButton.OK, MessageBoxImage.Error);
        //                    return;
        //                }
        //            }
        //        }

        //        SelectedDetailViewList[_FindIdx + 1].ROUT_SEQ = _Temp;
        //        SelectedDetailViewList[_FindIdx + 1].CRE_USR_ID = SystemProperties.USER;
        //        SelectedDetailViewList[_FindIdx + 1].UPD_USR_ID = SystemProperties.USER;
        //        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m66212/dtl/u", new StringContent(JsonConvert.SerializeObject(SelectedDetailViewList[_FindIdx + 1]), System.Text.Encoding.UTF8, "application/json")))
        //        {
        //            if (response.IsSuccessStatusCode)
        //            {
        //                string result = await response.Content.ReadAsStringAsync();
        //                if (int.TryParse(result, out _Num) == false)
        //                {
        //                    //실패
        //                    WinUIMessageBox.Show(result, _title, MessageBoxButton.OK, MessageBoxImage.Error);
        //                    return;
        //                }
        //            }
        //        }
        //    }


        //    //OnSelectedMasterItemChanged
        //    //isM_UPDATE = false;
        //    isM_DELETE = false;
        //    using (HttpResponseMessage responseX = await SystemProperties.PROGRAM_HTTP.PostAsync("m66212/dtl", new StringContent(JsonConvert.SerializeObject(SelectedMasterItem), System.Text.Encoding.UTF8, "application/json")))
        //    {
        //        if (responseX.IsSuccessStatusCode)
        //        {
        //            this.SelectedDetailViewList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await responseX.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
        //        }
        //        //
        //        if (SelectedDetailViewList.Count > 0)
        //        {
        //            //isM_UPDATE = true;
        //            isM_DELETE = true;

        //            SelectedDetailItem = SelectedDetailViewList.Where<ManVo>(x => x.ROUT_CD.Equals(_ROUT_CD)).FirstOrDefault<ManVo>();
        //            //SelectedDetailItem = SelectedDetailViewList[0];
        //        }
        //    }

        //}
        //catch
        //{
        //    return;
        //}
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

        private string _M_SEARCH_TEXT = string.Empty;
        public string M_SEARCH_TEXT
        {
            get { return _M_SEARCH_TEXT; }
            set { SetProperty(ref _M_SEARCH_TEXT, value, () => M_SEARCH_TEXT); }
        }


        public async void SYSTEM_CODE_VO()
        {
            ////SL_AREA_LIST = SystemProperties.SYSTEM_CODE_VO("L-002");
            //using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "L-005"))
            //{
            //    if (response.IsSuccessStatusCode)
            //    {
            //        this.ProdClssCdList = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
            //        if (this.ProdClssCdList.Count > 0)
            //        {
            //            this.ProdClssCdItem = this.ProdClssCdList[0];
            //        }
            //    }
            //}

            //
            //모델
            using (HttpResponseMessage responseX = await SystemProperties.PROGRAM_HTTP.PostAsync("m66213/mdl", new StringContent(JsonConvert.SerializeObject(new ManVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
            {
                if (responseX.IsSuccessStatusCode)
                {
                    this.MdlList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await responseX.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
                    this.M_MDL = this.MdlList[0];
                }
            }


            Refresh();
        }

    }
}
