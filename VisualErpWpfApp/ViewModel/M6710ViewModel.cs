using AquilaErpWpfApp3.Util;
using AquilaErpWpfApp3.View.M.Dialog;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.Core;
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

namespace AquilaErpWpfApp3.ViewModel
{
    //설비 관리
    public sealed class M6710ViewModel : ViewModelBase, INotifyPropertyChanged
    {

        private string _title = "실적등록";

        //private static ManServiceClient manClient = SystemProperties.ManClient;

        private IList<ManVo> selectedViewList;
        private IList<ManVo> selectedMasterViewList;

        private IList<ManVo> selectedPopup1ViewList;
        private IList<ManVo> selectedPopup2ViewList;
        private IList<ManVo> selectedPopup3ViewList;

        ////Master Dialog
        //private ICommand masterSearchDialogCommand;
        //private ICommand masterNewDialogCommand;
        //private ICommand masterEditDialogCommand;
        //private ICommand masterDelDialogCommand;
        ////
        private M6710MasterDialog masterDialog;
        private M6710DetailBadDialog badDialog;
        private M6710DetailIdleDialog idleDialog;
        private M6710DetailBadItmDialog badItmDialog;
        private M6710DetailInItmDialog inItmDialog;

        public M6710ViewModel()
        {
            StartDt = System.DateTime.Now;
            EndDt = System.DateTime.Now;

            SYSTEM_CODE_VO();
            //Refresh();
        }


        [Command]
        public async void Refresh()
        {
            try
            {

                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6710/", new StringContent(JsonConvert.SerializeObject(new ManVo() { FM_DT = (StartDt).ToString("yyyy-MM-dd"), TO_DT = (EndDt).ToString("yyyy-MM-dd"), CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectedViewList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();

                        //SelectedMasterViewList = manClient.M6625SelectMaster(new ManVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD });
                        if (SelectedViewList.Count > 0)
                        {
                            isM_UPDATE = true;
                            isM_DELETE = false;

                            //isM_REPORT = true;

                            SelectedItem = SelectedViewList[0];

                        }
                        else
                        {
                            isM_UPDATE = false;
                            isM_DELETE = false;

                            // isM_REPORT = false;

                            this.isD_UPDATE = false;
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
        public async void MstRefresh(string _LOT_DIV_NO = null)
        {
            try
            {
                SelectedPopup1ViewList = null;
                SelectedPopup2ViewList = null;
                SelectedPopup3ViewList = null;
                SelectedPopup3Item = null;

                if (SelectedItem == null)
                {
                    return;
                }


                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6710/mst", new StringContent(JsonConvert.SerializeObject(SelectedItem), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectedMasterViewList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();

                        //SelectedMasterViewList = manClient.M6625SelectMaster(new ManVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD });
                        if (SelectedMasterViewList.Count > 0)
                        {
                            //isM_UPDATE = true;
                            isM_DELETE = true;

                            //isM_REPORT = true;
                            if (string.IsNullOrEmpty(_LOT_DIV_NO))
                            {
                                SelectedMasterItem = SelectedMasterViewList[0];
                            }
                            else
                            {
                                SelectedMasterItem = SelectedMasterViewList.Where(x => (x.LOT_DIV_NO + "_" + x.LOT_DIV_SEQ).Equals(_LOT_DIV_NO)).LastOrDefault<ManVo>();
                            }
                        }
                        else
                        {
                            //isM_UPDATE = false;
                            //isM_DELETE = false;

                            // isM_REPORT = false;
                            isM_DELETE = false;

                            this.isD_UPDATE = false;
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

        #region Functon (Master Add, Edit, Del)
        public IList<ManVo> SelectedViewList
        {
            get { return selectedViewList; }
            private set { SetProperty(ref selectedViewList, value, () => SelectedViewList); }
        }

        ManVo _selectItem;
        public ManVo SelectedItem
        {
            get
            {
                return _selectItem;
            }
            set
            {
                if (value != null)
                {
                    SetProperty(ref _selectItem, value, () => SelectedItem, () => MstRefresh());
                }
            }
        }

        //
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


        private string _PANEL_TITLE_1 = "공정 불량 현황";
        public string PANEL_TITLE_1
        {
            get { return _PANEL_TITLE_1; }
            set { SetProperty(ref _PANEL_TITLE_1, value, () => PANEL_TITLE_1); }
        }
        //불량현황
        public IList<ManVo> SelectedPopup1ViewList
        {
            get { return selectedPopup1ViewList; }
            private set { SetProperty(ref selectedPopup1ViewList, value, () => SelectedPopup1ViewList); }
        }

        private string _PANEL_TITLE_2 = "비가동 현황";
        public string PANEL_TITLE_2
        {
            get { return _PANEL_TITLE_2; }
            set { SetProperty(ref _PANEL_TITLE_2, value, () => PANEL_TITLE_2); }
        }
        //비가동 현황
        public IList<ManVo> SelectedPopup2ViewList
        {
            get { return selectedPopup2ViewList; }
            private set { SetProperty(ref selectedPopup2ViewList, value, () => SelectedPopup2ViewList); }
        }


        private string _PANEL_TITLE_3 = "투입수량 현황 / 투입자재 불량 현황";
        public string PANEL_TITLE_3
        {
            get { return _PANEL_TITLE_3; }
            set { SetProperty(ref _PANEL_TITLE_3, value, () => PANEL_TITLE_3); }
        }
        //투입자재 불량 현황
        public IList<ManVo> SelectedPopup3ViewList
        {
            get { return selectedPopup3ViewList; }
            private set { SetProperty(ref selectedPopup3ViewList, value, () => SelectedPopup3ViewList); }
        }


        ManVo _selectPopup3Item;
        public ManVo SelectedPopup3Item
        {
            get
            {
                return _selectPopup3Item;
            }
            set
            {
                if (value != null)
                {
                    SetProperty(ref _selectPopup3Item, value, () => SelectedPopup3Item);
                }
            }
        }


        //금형
        private IList<ManVo> selectedMoldViewList;
        public IList<ManVo> MoldList
        {
            get { return selectedMoldViewList; }
            private set { SetProperty(ref selectedMoldViewList, value, () => MoldList); }
        }

        //금형
        private IList<ManVo> selectedRoutViewList;
        public IList<ManVo> RoutList
        {
            get { return selectedRoutViewList; }
            private set { SetProperty(ref selectedRoutViewList, value, () => RoutList); }
        }


        //public IList<ManVo> SelectedPopupViewList
        //{
        //    get { return selectedPopupViewList; }
        //    private set { SetProperty(ref selectedPopupViewList, value, () => SelectedPopupViewList); }
        //}

        //ManVo _selectPopupItem;
        //public ManVo SelectedPopupItem
        //{
        //    get
        //    {
        //        return _selectPopupItem;
        //    }
        //    set
        //    {
        //        if (value != null)
        //        {
        //            SetProperty(ref _selectPopupItem, value, () => SelectedPopupItem);
        //        }
        //    }
        //}

        async void OnSelectedMasterItemChanged()
        {
            try
            {
                if (this.SelectedMasterItem == null) { return; }

                SelectedPopup1ViewList = null;
                SelectedPopup2ViewList = null;
                SelectedPopup3ViewList = null;
                SelectedPopup3Item = null;


                //불량 현황
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6710/dtl2", new StringContent(JsonConvert.SerializeObject(this.SelectedMasterItem), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectedPopup1ViewList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
                        this.PANEL_TITLE_1 = "공정 불량 현황(" + this.SelectedPopup1ViewList.Count + ")";

                    }

                }

                //비가동 현황
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6710/dtl1", new StringContent(JsonConvert.SerializeObject(this.SelectedMasterItem), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectedPopup2ViewList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
                        this.PANEL_TITLE_2 = "비가동 현황(" + this.SelectedPopup2ViewList.Count + ")";
                    }

                }


                //투입 자재 - 불량 현황
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6710/dtl3/mst", new StringContent(JsonConvert.SerializeObject(this.SelectedMasterItem), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectedPopup3ViewList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
                        if (this.SelectedPopup3ViewList.Count > 0)
                        {
                            SelectedPopup3Item = this.SelectedPopup3ViewList[0];

                            this.isD_UPDATE = true;
                        }
                        else
                        {
                            this.isD_UPDATE = false;
                            SelectedPopup3Item = null;
                        }
                        this.PANEL_TITLE_3 = "투입수량 현황 / 투입자재 불량 현황(" + this.SelectedPopup3ViewList.Count + ")";
                    }

                }
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
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
        public async void AddContact()
        {
            try
            {
                if (SelectedItem == null)
                {
                    return;
                }

                MessageBoxResult result = WinUIMessageBox.Show("(지시 일자 : " + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm") + ")  추가 하시겠습니까?", _title, MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6710/mst/m", new StringContent(JsonConvert.SerializeObject(new List<ManVo>() { new ManVo() { UPD_USR_ID = SystemProperties.USER, CRE_USR_ID = SystemProperties.USER, CHNL_CD = SystemProperties.USER_VO.CHNL_CD, LOT_DIV_NO = this.SelectedItem.LOT_DIV_NO, ROUT_CD = this.SelectedItem.ROUT_CD, EQ_NO = this.SelectedItem.EQ_NO, MOLD_CD = this.SelectedItem.MOLD_CD, ITM_MOLD_SEQ = this.SelectedItem.ITM_MOLD_SEQ } }), System.Text.Encoding.UTF8, "application/json")))
                    {
                        int _Num;
                        if (response.IsSuccessStatusCode)
                        {
                            string resultMsg = await response.Content.ReadAsStringAsync();
                            if (int.TryParse(resultMsg, out _Num) == false)
                            {
                                //실패
                                WinUIMessageBox.Show(resultMsg, _title, MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                        }

                        //WinUIMessageBox.Show("저장 완료 되었습니다", _title, MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    MstRefresh();
                }
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }

        [Command]
        public async void DelContact()
        {
            try
            {
                ManVo delDao = this.SelectedMasterItem;
                if (delDao != null)
                {
                    MessageBoxResult result = WinUIMessageBox.Show("(" + delDao.RN + " / " + delDao.UPD_DT + " / " + delDao.LOT_DIV_NO + ")  정말로 삭제 하시겠습니까?", _title, MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {

                        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6710/mst/d", new StringContent(JsonConvert.SerializeObject(delDao), System.Text.Encoding.UTF8, "application/json")))
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
                        }
                        MstRefresh();

                        //성공
                        WinUIMessageBox.Show("삭제가 완료되었습니다.", _title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                    }
                }
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }



        //[Command]
        //public void DelContact()
        //{
        //    try
        //    {
        //        if (SelectedMasterItem == null)
        //        {
        //            return;
        //        }





        //    }
        //    catch (System.Exception eLog)
        //    {
        //        WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
        //        return;
        //    }
        //}


        [Command]
        public void NewMasterContact()
        {
            masterDialog = new M6710MasterDialog(new ManVo());
            masterDialog.Title = _title + " - 추가";
            masterDialog.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            masterDialog.Owner = Application.Current.MainWindow;
            masterDialog.BorderEffect = BorderEffect.Default;
            //masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            //masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            bool isDialog = (bool)masterDialog.ShowDialog();
            if (isDialog)
            {
                MstRefresh();



                //    // Refresh(masterDialog.resultDomain.PCK_PLST_CLSS_CD + "_" + masterDialog.resultDomain.PCK_PLST_CD);

                //    //if (masterDialog.IsEdit == false)
                //    //{
                //    //    Refresh();

                //    //    for (int x = 0; x < SelectedMasterViewList.Count; x++)
                //    //    {
                //    //        if ((masterDialog.resultDomain.PCK_PLST_CLSS_CD + "_" + masterDialog.resultDomain.PCK_PLST_CD).Equals(SelectedMasterViewList[x].PCK_PLST_CLSS_CD + "_" + SelectedMasterViewList[x].PCK_PLST_CD))
                //    //        {
                //    //            SelectedMasterItem = SelectedMasterViewList[x];
                //    //            break;
                //    //        }
                //    //    }
                //    //}
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
        [Command]
        public void EditMasterContact()
        {
            //if (this._selectMasterItem == null) { return; }
            //ManVo editDao = this._selectMasterItem;
            //if (editDao != null)
            //{
            //    masterDialog = new M665101MasterDialog(editDao);
            //    masterDialog.Title = _title + " - 수정";
            //    masterDialog.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            //    masterDialog.BorderEffect = BorderEffect.Default;
            //    masterDialog.Owner = Application.Current.MainWindow;
            //    masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            //    masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            //    bool isDialog = (bool)masterDialog.ShowDialog();
            //    if (isDialog)
            //    {
            //        //Refresh(masterDialog.resultDomain.PCK_PLST_CLSS_CD + "_" + masterDialog.resultDomain.PCK_PLST_CD);
            //        //if (masterDialog.IsEdit == false)
            //        //{
            //        //    Refresh();
            //        //}
            //    }
            //}
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



        //불량 현황
        [Command]
        public async void Bad()
        {
            try
            {
                ManVo badDao = this._selectMasterItem;
                if (badDao != null)
                {
                    //
                    badDialog = new M6710DetailBadDialog(badDao);
                    badDialog.Title = _title + " - 공정 불량 현황";
                    badDialog.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    badDialog.BorderEffect = BorderEffect.Default;
                    badDialog.Owner = Application.Current.MainWindow;
                    //badDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
                    //badDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
                    bool isDialog = (bool)badDialog.ShowDialog();
                    if (isDialog)
                    {
                        //공정 불량 현황
                        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6710/dtl2", new StringContent(JsonConvert.SerializeObject(this.SelectedMasterItem), System.Text.Encoding.UTF8, "application/json")))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                this.SelectedPopup1ViewList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
                                this.PANEL_TITLE_1 = "공정 불량 현황(" + this.SelectedPopup1ViewList.Count + ")";

                            }

                        }

                        //OnSelectedMasterItemChanged();
                        //        //Refresh(masterDialog.resultDomain.PCK_PLST_CLSS_CD + "_" + masterDialog.resultDomain.PCK_PLST_CD);
                        //        //if (masterDialog.IsEdit == false)
                        //        //{
                        //        //    Refresh();
                        //        //}
                    }
                }
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }

        //비가동 현황
        [Command]
        public async void Idle()
        {
            try
            {
                ManVo badDao = this._selectMasterItem;
                if (badDao != null)
                {
                    //
                    idleDialog = new M6710DetailIdleDialog(badDao);
                    idleDialog.Title = _title + " - 비가동 현황";
                    idleDialog.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    idleDialog.BorderEffect = BorderEffect.Default;
                    idleDialog.Owner = Application.Current.MainWindow;
                    //badDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
                    //badDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
                    bool isDialog = (bool)idleDialog.ShowDialog();
                    if (isDialog)
                    {
                        // OnSelectedMasterItemChanged();
                        string _LOT_DIV_NO = badDao.LOT_DIV_NO;
                        int? _LOT_DIV_SEQ = badDao.LOT_DIV_SEQ;

                        //업데이트
                        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6710/mst/r/u", new StringContent(JsonConvert.SerializeObject(badDao), System.Text.Encoding.UTF8, "application/json")))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                MstRefresh(_LOT_DIV_NO + "_" + _LOT_DIV_SEQ);
                                //  this.SelectedMasterItem.WRK_HRS = JsonConvert.DeserializeObject<string>(await response.Content.ReadAsStringAsync());

                                //int _Num = 0;
                                //string resultMsg = await response.Content.ReadAsStringAsync();
                                //if (int.TryParse(resultMsg, out _Num) == false)
                                //{
                                //    //실패
                                //    WinUIMessageBox.Show(resultMsg, _title, MessageBoxButton.OK, MessageBoxImage.Error);
                                //    return;
                                //}
                            }
                        }

                        //Refresh(_LOT_DIV_NO);


                        //        //Refresh(masterDialog.resultDomain.PCK_PLST_CLSS_CD + "_" + masterDialog.resultDomain.PCK_PLST_CD);
                        //        //if (masterDialog.IsEdit == false)
                        //        //{
                        //        //    Refresh();
                        //        //}
                    }
                }
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }



        //투입 자재 불량 현황
        [Command]
        public async void BadItm()
        {
            try
            {
                if (this.SelectedPopup3ViewList.Count > 0)
                {
                    ManVo badDao = this.SelectedPopup3Item;
                    if (badDao != null)
                    {
                        //
                        badItmDialog = new M6710DetailBadItmDialog(badDao);
                        badItmDialog.Title = _title + " - 투입자재 불량 현황 [" + SelectedPopup3Item.ROUT_ORD_SEQ + " / " + SelectedPopup3Item.ITM_CD + "]";
                        badItmDialog.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                        badItmDialog.BorderEffect = BorderEffect.Default;
                        badItmDialog.Owner = Application.Current.MainWindow;
                        //badDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
                        //badDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
                        bool isDialog = (bool)badItmDialog.ShowDialog();
                        if (isDialog)
                        {
                            OnSelectedMasterItemChanged();
                            //        //Refresh(masterDialog.resultDomain.PCK_PLST_CLSS_CD + "_" + masterDialog.resultDomain.PCK_PLST_CD);
                            //        //if (masterDialog.IsEdit == false)
                            //        //{
                            //        //    Refresh();
                            //        //}
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


        //투입수량 현황
        [Command]
        public async void InItm()
        {
            try
            {
                if (this.SelectedPopup3ViewList.Count > 0)
                {
                    ManVo badDao = this.SelectedPopup3Item;
                    if (badDao != null)
                    {
                        //
                        inItmDialog = new M6710DetailInItmDialog(badDao);
                        inItmDialog.Title = _title + " - 투입수량 현황 [" + SelectedPopup3Item.ROUT_ORD_SEQ + " / " + SelectedPopup3Item.ITM_CD + "]";
                        inItmDialog.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                        inItmDialog.BorderEffect = BorderEffect.Default;
                        inItmDialog.Owner = Application.Current.MainWindow;
                        //badDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
                        //badDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
                        bool isDialog = (bool)inItmDialog.ShowDialog();
                        if (isDialog)
                        {
                            OnSelectedMasterItemChanged();
                            //        //Refresh(masterDialog.resultDomain.PCK_PLST_CLSS_CD + "_" + masterDialog.resultDomain.PCK_PLST_CD);
                            //        //if (masterDialog.IsEdit == false)
                            //        //{
                            //        //    Refresh();
                            //        //}
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


        //[Command]
        //public async void OpenReportViewContact()
        //{
        //    isPRNT_PLN = true;


        //    OnSelectedMasterItemChanged();
        //}

        //[Command]
        //public async void ReportContact()
        //{
        //    try
        //    {
        //        if (this.SelectedMasterItem == null) { return; }

        //        MessageBoxResult result = WinUIMessageBox.Show("[" + this.SelectedMasterItem.PROD_EQ_NO + " / " + this.SelectedPopupItem.PROD_PLN_DT + " / " + this.SelectedPopupItem.PROD_PLN_QTY + "] 작지 발행 하시겠습니까?", _title, MessageBoxButton.YesNo, MessageBoxImage.Question);
        //        if (result == MessageBoxResult.Yes)
        //        {

        //            List<ManVo> _paramList = new List<ManVo>();

        //            ManVo _param = new ManVo();
        //            _param.PROD_LOC_CD = this.SelectedMasterItem.PROD_LOC_CD;
        //            _param.WKY_YRMON = this.SelectedMasterItem.WKY_YRMON;
        //            _param.WK = this.SelectedMasterItem.WK;
        //            _param.CMPO_CD = this.SelectedMasterItem.CMPO_CD;
        //            _param.PROD_EQ_NO = this.SelectedMasterItem.PROD_EQ_NO;
        //            _param.CRE_USR_ID = SystemProperties.USER_VO.USR_ID;
        //            _param.UPD_USR_ID = SystemProperties.USER_VO.USR_ID;
        //            _param.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
        //            _param.RN = this.SelectedPopupItem.RN;
        //            _param.PROD_PLN_QTY = this.SelectedPopupItem.PROD_PLN_QTY;
        //            _param.INP_STAFF_VAL = 0;
        //            _param.MM_RMK = this.SelectedPopupItem.MM_RMK;
        //            _param.PCK_FLG = "IN";
        //            _param.A_ROUT_CD = new string[] { "IN" };

        //            //
        //            _paramList.Add(_param);


        //            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m665101/wrk", new StringContent(JsonConvert.SerializeObject(_paramList), System.Text.Encoding.UTF8, "application/json")))
        //            {
        //                if (response.IsSuccessStatusCode)
        //                {
        //                    //출력물
        //                    List<ManVo>  _reportList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();

        //                    //_reportList[0].FM_DT = System.DateTime.Now.ToString("yyyy-MM-dd");
        //                    _reportList[0].FM_DT = this.SelectedPopupItem.PROD_PLN_DT;
        //                    _reportList[0].MM_01 = "사출";

        //                    //
        //                    M665101Report report = new M665101Report(_reportList);
        //                    report.Margins.Top = 20;
        //                    report.Margins.Bottom = 20;
        //                    report.Margins.Left = 50;
        //                    report.Margins.Right = 20;
        //                    report.Landscape = false;

        //                    report.PrintingSystem.ShowPrintStatusDialog = true;
        //                    report.PaperKind = System.Drawing.Printing.PaperKind.A4;
        //                    report.Watermark.Text = Properties.Settings.Default.SettingCompany;
        //                    report.Watermark.TextDirection = DevExpress.XtraPrinting.Drawing.DirectionMode.ForwardDiagonal;
        //                    report.Watermark.Font = new System.Drawing.Font(report.PrintingSystem.Watermark.Font.FontFamily, 40);
        //                    report.Watermark.ForeColor = System.Drawing.Color.PaleTurquoise;
        //                    report.Watermark.TextTransparency = 150;


        //                    var window = new DevExpress.Xpf.Printing.DocumentPreviewWindow();
        //                    window.PreviewControl.DocumentSource = report;
        //                    report.CreateDocument(true);
        //                    window.Title = _title;
        //                    window.Owner = Application.Current.MainWindow;
        //                    window.ShowDialog();

        //                    //출력 유무 업데이트
        //                    //SearchDetailJob.UPD_USR_ID = SystemProperties.USER;
        //                    //using (HttpResponseMessage responseY = await SystemProperties.PROGRAM_HTTP.PostAsync("m66520/mst/u", new StringContent(JsonConvert.SerializeObject(SearchDetailJob), System.Text.Encoding.UTF8, "application/json")))
        //                    //{
        //                    //}
        //                }

        //                    //if (response.IsSuccessStatusCode)
        //                    //{
        //                    //    string resultMsg = await response.Content.ReadAsStringAsync();
        //                    //    if (int.TryParse(resultMsg, out _Num) == false)
        //                    //    {
        //                    //        //실패
        //                    //        WinUIMessageBox.Show(resultMsg, _title, MessageBoxButton.OK, MessageBoxImage.Error);
        //                    //        return;
        //                    //    }

        //                    //    //성공
        //                    //    WinUIMessageBox.Show("완료 되었습니다", _title, MessageBoxButton.OK, MessageBoxImage.Information);
        //                    //}
        //                }
        //        }

        //    }
        //    catch (System.Exception eLog)
        //    {
        //        WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
        //        return;
        //    }
        //}

        //[Command]
        //public async void CloseReportViewContact()
        //{
        //    isPRNT_PLN = false;
        //}


        public async void SYSTEM_CODE_VO()
        {
            ////사업장
            //using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "A-001"))
            //{
            //    if (response.IsSuccessStatusCode)
            //    {
            //        ProdLocList = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
            //        if (ProdLocList.Count > 0)
            //        {
            //            //ProdLocList.Insert(0, new SystemCodeVo() { CLSS_CD = null, CLSS_DESC = "전체" });
            //            M_PROD_LOC_NM = ProdLocList[0];
            //        }
            //    }
            //}



            //SL_AREA_LIST = SystemProperties.SYSTEM_CODE_VO("L-002");
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "L-002"))
            {
                if (response.IsSuccessStatusCode)
                {
                    SL_AREA_LIST = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                    if (SL_AREA_LIST.Count > 0)
                    {
                        M_SL_AREA_NM = SL_AREA_LIST[0];
                    }
                }
            }

            //금형
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6661", new StringContent(JsonConvert.SerializeObject(new ManVo() { DELT_FLG = "N", CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.MoldList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
                }
            }

            //금형
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6611", new StringContent(JsonConvert.SerializeObject(new ManVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.RoutList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
                }
            }

            //using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("M665100", new StringContent(JsonConvert.SerializeObject(new ManVo() { DELT_FLG = "N", CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
            //{
            //    if (response.IsSuccessStatusCode)
            //    {
            //        PlnYrmonList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
            //        if (PlnYrmonList.Count > 0)
            //        {
            //            //CoNmList.Insert(0, new SystemCodeVo() { CO_NM = "전체", CO_NO = null });
            //            //M_PLN_YRMON = PlnYrmonList[PlnYrmonList.Count - 1];
            //            M_PLN_YRMON = PlnYrmonList.Where<ManVo>(w => w.WKY_YRMON.Equals(System.DateTime.Now.ToString("yyyyMM"))).FirstOrDefault<ManVo>();
            //        }
            //    }
            //}

            Refresh();
        }


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

        private IList<SystemCodeVo> _AreaCd = new List<SystemCodeVo>();
        public IList<SystemCodeVo> SL_AREA_LIST
        {
            get { return _AreaCd; }
            set { SetProperty(ref _AreaCd, value, () => SL_AREA_LIST); }
        }

        private SystemCodeVo _M_SL_AREA_NM;
        public SystemCodeVo M_SL_AREA_NM
        {
            get { return _M_SL_AREA_NM; }
            set { SetProperty(ref _M_SL_AREA_NM, value, () => M_SL_AREA_NM); }
        }

        //MST
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

        //DTL
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


        ////TITLE
        //private string _N1ST_PLN_TITLE = string.Empty;
        //public string N1ST_PLN_TITLE
        //{
        //    get { return _N1ST_PLN_TITLE; }
        //    set { SetProperty(ref _N1ST_PLN_TITLE, value, () => N1ST_PLN_TITLE); }
        //}
        //private string _N2ND_PLN_TITLE = string.Empty;
        //public string N2ND_PLN_TITLE
        //{
        //    get { return _N2ND_PLN_TITLE; }
        //    set { SetProperty(ref _N2ND_PLN_TITLE, value, () => N2ND_PLN_TITLE); }
        //}
        //private string _N3RD_PLN_TITLE = string.Empty;
        //public string N3RD_PLN_TITLE
        //{
        //    get { return _N3RD_PLN_TITLE; }
        //    set { SetProperty(ref _N3RD_PLN_TITLE, value, () => N3RD_PLN_TITLE); }
        //}
        //private string _N4TH_PLN_TITLE = string.Empty;
        //public string N4TH_PLN_TITLE
        //{
        //    get { return _N4TH_PLN_TITLE; }
        //    set { SetProperty(ref _N4TH_PLN_TITLE, value, () => N4TH_PLN_TITLE); }
        //}
        //private string _N5TH_PLN_TITLE = string.Empty;
        //public string N5TH_PLN_TITLE
        //{
        //    get { return _N5TH_PLN_TITLE; }
        //    set { SetProperty(ref _N5TH_PLN_TITLE, value, () => N5TH_PLN_TITLE); }
        //}
        //private string _N6TH_PLN_TITLE = string.Empty;
        //public string N6TH_PLN_TITLE
        //{
        //    get { return _N6TH_PLN_TITLE; }
        //    set { SetProperty(ref _N6TH_PLN_TITLE, value, () => N6TH_PLN_TITLE); }
        //}
        //private string _N7TH_PLN_TITLE = string.Empty;
        //public string N7TH_PLN_TITLE
        //{
        //    get { return _N7TH_PLN_TITLE; }
        //    set { SetProperty(ref _N7TH_PLN_TITLE, value, () => N7TH_PLN_TITLE); }
        //}
        //
        ////작업지시서 창 - 활성
        //private bool? _isPRNT_PLN = false;
        //public bool? isPRNT_PLN
        //{
        //    get { return _isPRNT_PLN; }
        //    set { SetProperty(ref _isPRNT_PLN, value, () => isPRNT_PLN); }
        //}

        //private bool? _isM_REPORT = false;
        //public bool? isM_REPORT
        //{
        //    get { return _isM_REPORT; }
        //    set { SetProperty(ref _isM_REPORT, value, () => isM_REPORT); }
        //}


    }
}
