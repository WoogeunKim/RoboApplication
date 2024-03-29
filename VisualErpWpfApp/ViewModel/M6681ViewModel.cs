using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Man;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Media;
using AquilaErpWpfApp3.Util;
using AquilaErpWpfApp3.View.M.Dialog;

namespace AquilaErpWpfApp3.ViewModel
{
    public sealed class M6681ViewModel : ViewModelBase, INotifyPropertyChanged
    {

        //private static CodeServiceClient codeClient = SystemProperties.CodeClient;
        //private static MakeServiceClient makeClient = SystemProperties.MakeClient;

        //private Dictionary<string, string> _ItemDivision = new Dictionary<string, string>();
        //private Dictionary<string, string> _ItemN1st = new Dictionary<string, string>();

        //private IList<string> itemDivision;

        //private IList<SystemCodeVo> itemN1st;
        private IList<ManVo> selectedMenuViewList;
        private IList<ManVo> selectedDwgViewList;
        private IList<ManVo> selectedWrkViewList;
        //private IList<MakeVo> selectedWrkRvisViewList;

        //Menu Dialog
        //private ICommand searchDialogCommand;

        //금형도면이력관리
        //private ICommand searchDwgRvisDialogCommand;
        //private ICommand newDwgRvisDialogCommand;
        //private ICommand editDwgRvisDialogCommand;
        //private ICommand delDwgRvisDialogCommand;

        ////작업표준서관리
        //private ICommand searchWrkRvisDialogCommand;
        //private ICommand newWrkRvisDialogCommand;
        //private ICommand editWrkRvisDialogCommand;
        //private ICommand delWrkRvisDialogCommand;



        //private ICommand<string> downloadRvisDialogCommand;
        //private ICommand<string> imageRvisDialogCommand;


        // private P4413EqDialog dwgDialog;
        private M6681WrkDialog wrkDialog;
        private M6681DwgDialog dwgDialog;

        private System.Windows.Forms.FolderBrowserDialog downloaddialog;
        private FileStream fs;

        public M6681ViewModel()
        {
            //Division();
            //this.itemDivision = new List<string>();
            //this.itemDivision.Add("생산");
            //this.itemDivision.Add("상품");


            //SelectedTypeItem = itemDivision[0];

            //N1stList();
            Refresh();
        }

        [Command]
        public async void Refresh(string _ITM_CD =null)
        {
            try
            {
                //if (SelectedN1stItem == null)
                //{
                //    return;
                //}

                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6681", new StringContent(JsonConvert.SerializeObject(new ManVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        SelectedMenuViewList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
                        if (SelectedMenuViewList.Count > 0)
                        {
                            //SelectedMenuItem = SelectedMenuViewList[0];
                            this.IS_MENU = true;
                            if (string.IsNullOrEmpty(_ITM_CD))
                            {
                                SelectedMenuItem = SelectedMenuViewList[0];
                            }
                            else
                            {
                                SelectedMenuItem = SelectedMenuViewList.Where(x => (x.ITM_CD).Equals(_ITM_CD)).LastOrDefault<ManVo>();
                            }
                        }
                        else
                        {
                            SelectedMenuItem = null;
                            this.IS_MENU = false;
                        }
                    }
                }
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[에러]제품도면관리(작업표준서)", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }

        #region Functon (Menu Add, Edit, Del)

        //void Division()
        //{
        //   // itemDivision = codeClient.SelectItemGroupList(new SystemCodeVo() { DELT_FLG = "N" });
        //   //if (itemDivision.Count > 0)
        //   //{
        //   //     SelectedN1stItem = itemDivision[0];
        //   //}

        //    itemDivision = new List<string>();
        //    itemDivision.Add("생산");
        //    itemDivision.Add("상품");


        //    SelectedTypeItem = itemDivision[0];
        //}

        bool _isMenu;
        public bool IS_MENU
        {
            get
            {
                return _isMenu;
            }
            set
            {
                if (value != null)
                {
                    SetProperty(ref _isMenu, value, () => IS_MENU);
                }
            }
        }

        bool _isDwgMenu;
        public bool IS_DWG_MENU
        {
            get
            {
                return _isDwgMenu;
            }
            set
            {
                if (value != null)
                {
                    SetProperty(ref _isDwgMenu, value, () => IS_DWG_MENU);
                }
            }
        }

        bool _isWrkMenu;
        public bool IS_WRK_MENU
        {
            get
            {
                return _isWrkMenu;
            }
            set
            {
                if (value != null)
                {
                    SetProperty(ref _isWrkMenu, value, () => IS_WRK_MENU);
                }
            }
        }
        //bool _isWrkRvisMenu;
        //public bool IS_WRK_RVIS_MENU
        //{
        //    get
        //    {
        //        return _isWrkRvisMenu;
        //    }
        //    set
        //    {
        //        if (value != null)
        //        {
        //            SetProperty(ref _isWrkRvisMenu, value, () => IS_WRK_RVIS_MENU);
        //        }
        //    }
        //}

        //string _selectedTypeItem;
        //public string SelectedTypeItem
        //{
        //    get { return _selectedTypeItem; }
        //    set { SetProperty(ref _selectedTypeItem, value, () => SelectedTypeItem, N1stList); }
        //}

        //public IList<string> ItemDivision
        //{
        //    get { return itemDivision; }
        //    set { SetProperty(ref itemDivision, value, () => ItemDivision); }
        //}



        //public async void N1stList()
        //{
        //    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "L-700"))
        //    {
        //        if (response.IsSuccessStatusCode)
        //        {
        //            ItemN1st = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
        //            if (ItemN1st.Count > 0)
        //            {
        //                SelectedN1stItem = ItemN1st[0];
        //            }
        //        }
        //    }



        //    //if (SelectedTypeItem == null)
        //    //{
        //    //    return;
        //    //}

        //    //ItemN1st = makeClient.P4413_SelectMstList(new MakeVo() { DELT_FLG = "N" });
        //    //if (ItemN1st.Count > 0)
        //    //{
        //    //    SelectedN1stItem = ItemN1st[0];
        //    //}
        //}

        //public IList<SystemCodeVo> ItemN1st
        //{
        //    get { return itemN1st; }
        //    set { SetProperty(ref itemN1st, value, () => ItemN1st); }
        //}


        //SystemCodeVo _selectedN1stItem;
        //public SystemCodeVo SelectedN1stItem
        //{
        //    get { return _selectedN1stItem; }
        //    set { SetProperty(ref _selectedN1stItem, value, () => SelectedN1stItem, Refresh); }
        //}

        public IList<ManVo> SelectedMenuViewList
        {
            get { return selectedMenuViewList; }
            set { SetProperty(ref selectedMenuViewList, value, () => SelectedMenuViewList); }
        }

        ManVo _selectMenuItem;
        public ManVo SelectedMenuItem
        {
            get
            {
                return _selectMenuItem;
            }
            set
            {
                if (value != null)
                {
                    SetProperty(ref _selectMenuItem, value, () => SelectedMenuItem, SearchDetailItem);
                    //SearchDetailItem();
                }
            }
        }

        public void SearchDetailItem()
        {
            DwgRefresh();
            WrkRefresh();
        }

        [Command]
        public async void DwgRefresh(string _DWG_NM = null)
        {
            if (SelectedMenuItem == null)
            {
                return;
            }

            //
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6681/dwg", new StringContent(JsonConvert.SerializeObject(new ManVo() { ITM_CD = SelectedMenuItem.ITM_CD, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    SelectedDwgViewList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
                    if (SelectedDwgViewList.Count > 0)
                    {
                        this.IS_DWG_MENU = true;

                        if (string.IsNullOrEmpty(_DWG_NM))
                        {
                            SelectedDwgItem = SelectedDwgViewList[0];
                        }
                        else
                        {
                            SelectedDwgItem = SelectedDwgViewList.Where(x => (x.DWG_NM).Equals(_DWG_NM)).LastOrDefault<ManVo>();
                        }
                    }
                    else
                    {
                        SelectedDwgItem = null;
                        this.IS_DWG_MENU = false;
                    }
                }
            }
        }


        [Command]
        public async void WrkRefresh(string _WRK_DOC_NM = null)
        {
            if (SelectedMenuItem == null)
            {
                return;
            }

            //
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6681/wrk", new StringContent(JsonConvert.SerializeObject(new ManVo() { ITM_CD = SelectedMenuItem.ITM_CD, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    SelectedWrkViewList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
                    if (SelectedWrkViewList.Count > 0)
                    {
                        this.IS_WRK_MENU = true;

                        if (string.IsNullOrEmpty(_WRK_DOC_NM))
                        {
                            SelectedWrkItem = SelectedWrkViewList[0];
                        }
                        else
                        {
                            SelectedWrkItem = SelectedWrkViewList.Where(x => (x.WRK_DOC_NM).Equals(_WRK_DOC_NM)).LastOrDefault<ManVo>();
                        }
                    }
                    else
                    {
                        SelectedWrkItem = null;
                        this.IS_WRK_MENU = false;
                    }
                }
            }
        }

        //도면 이력관리
        public IList<ManVo> SelectedDwgViewList
        {
            get { return selectedDwgViewList; }
            set { SetProperty(ref selectedDwgViewList, value, () => SelectedDwgViewList); }
        }

        ManVo _selectedDwgItem;
        public ManVo SelectedDwgItem
        {
            get
            {
                return _selectedDwgItem;
            }
            set
            {
                if (value != null)
                {
                    SetProperty(ref _selectedDwgItem, value, () => SelectedDwgItem);
                    //SearchDetailItem();
                }
            }
        }

        //작업표준서 이력관리
        public IList<ManVo> SelectedWrkViewList
        {
            get { return selectedWrkViewList; }
            set { SetProperty(ref selectedWrkViewList, value, () => SelectedWrkViewList); }
        }

        ManVo _selectedWrkItem;
        public ManVo SelectedWrkItem
        {
            get
            {
                return _selectedWrkItem;
            }
            set
            {
                if (value != null)
                {
                    SetProperty(ref _selectedWrkItem, value, () => SelectedWrkItem);
                }
            }
        }


        //public void SearchMenuContact()
        //{
        //    try
        //    {
        //        if (SelectedN1stItem == null)
        //        {
        //            return;
        //        }

        //        DXSplashScreen.Show<ProgressWindow>();

        //        Refresh();
        //        DXSplashScreen.Close();
        //    }
        //    catch
        //    {
        //        DXSplashScreen.Close();
        //        return;
        //    }
        //}

        //도면 이력관리
        //public ICommand SearchDwgRvisDialogCommand
        //{
        //    get
        //    {
        //        if (searchDwgRvisDialogCommand == null)
        //            searchDwgRvisDialogCommand = new DelegateCommand(SearchDwgRvisContact);
        //        return searchDwgRvisDialogCommand;
        //    }
        //}
        //public void SearchDwgRvisContact()
        //{
        //    try
        //    {
        //        RvisRefresh();
        //    }
        //    catch
        //    {
        //        return;
        //    }
        //}

        //public ICommand NewDwgRvisDialogCommand
        //{
        //    get
        //    {
        //        if (newDwgRvisDialogCommand == null)
        //            newDwgRvisDialogCommand = new DelegateCommand(NewDwgRvisContact);
        //        return newDwgRvisDialogCommand;
        //    }
        //}

        [Command]
        public void NewDwgContact()
        {
            if (SelectedMenuItem == null)
            {
                return;
            }

            dwgDialog = new M6681DwgDialog(new ManVo() { ITM_CD = SelectedMenuItem.ITM_CD, ITM_NM = SelectedMenuItem.ITM_NM });
            dwgDialog.Title = "제품도면관리(작업표준서) - 추가 [" + SelectedMenuItem.ITM_CD + " / " + SelectedMenuItem.ITM_NM + "]";
            dwgDialog.Owner = Application.Current.MainWindow;
            dwgDialog.BorderEffect = BorderEffect.Default;
            dwgDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            dwgDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            bool isDialog = (bool)dwgDialog.ShowDialog();
            if (isDialog)
            {
                Refresh(dwgDialog.updateDao.ITM_CD);
                //
                //DwgRefresh(dwgDialog.updateDao.DWG_NM);
                //SearchRvisContact();
                //Refresh();
                //if (SelectedMenuViewList.Count > 0)
                //{
                //    SelectedMenuItem = SelectedMenuViewList.Where<MakeVo>(x => x.EQ_CD.Equals(dwgDialog._EQ_CD)).First<MakeVo>();
                //}
                ////도면 이력 조회
                //if (SelectedRvisViewList.Count > 0)
                //{
                //    SelectedRvisItem = SelectedRvisViewList[0];
                //}

            }
        }

        [Command]
        public void NewWrkContact()
        {
            if (SelectedMenuItem == null)
            {
                return;
            }

            wrkDialog = new M6681WrkDialog(new ManVo() { ITM_CD = SelectedMenuItem.ITM_CD, ITM_NM = SelectedMenuItem.ITM_NM });
            wrkDialog.Title = "제품도면관리(작업표준서) - 추가 [" + SelectedMenuItem.ITM_CD + " / " + SelectedMenuItem.ITM_NM + "]";
            wrkDialog.Owner = Application.Current.MainWindow;
            wrkDialog.BorderEffect = BorderEffect.Default;
            wrkDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            wrkDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            bool isDialog = (bool)wrkDialog.ShowDialog();
            if (isDialog)
            {
                WrkRefresh(wrkDialog.updateDao.WRK_DOC_NM);
                //SearchRvisContact();
                //Refresh();
                //if (SelectedMenuViewList.Count > 0)
                //{
                //    SelectedMenuItem = SelectedMenuViewList.Where<MakeVo>(x => x.EQ_CD.Equals(dwgDialog._EQ_CD)).First<MakeVo>();
                //}
                ////도면 이력 조회
                //if (SelectedRvisViewList.Count > 0)
                //{
                //    SelectedRvisItem = SelectedRvisViewList[0];
                //}

            }
        }

        //public ICommand EditDwgRvisDialogCommand
        //{
        //    get
        //    {
        //        if (editDwgRvisDialogCommand == null)
        //            editDwgRvisDialogCommand = new DelegateCommand(EditDwgRvisContact);
        //        return editDwgRvisDialogCommand;
        //    }
        //}

        [Command]
        public async void EditDwgContact()
        {
            if (SelectedDwgItem != null)
            {
                MessageBoxResult result = WinUIMessageBox.Show("[" + SelectedDwgItem.RN + "/" + SelectedDwgItem.DWG_NM + "]" + " 정말로 삭제 하시겠습니까?", "제품도면관리(작업표준서)", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    SelectedDwgItem.DELT_FLG = "Y";
                    SelectedDwgItem.UPD_USR_ID = SystemProperties.USER;
                    SelectedDwgItem.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;

                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6681/dwg/u", new StringContent(JsonConvert.SerializeObject(SelectedDwgItem), System.Text.Encoding.UTF8, "application/json")))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            int _Num = 0;
                            string resultMsg = await response.Content.ReadAsStringAsync();
                            if (int.TryParse(resultMsg, out _Num) == false)
                            {
                                //실패
                                WinUIMessageBox.Show(resultMsg, "제품도면관리(작업표준서)", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                            DwgRefresh();

                            //성공
                            WinUIMessageBox.Show("삭제가 완료되었습니다.", "제품도면관리(작업표준서)", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                        }
                    }

                }
            }

        }


        [Command]
        public async void EditWrkContact()
        {
            if (SelectedWrkItem != null)
            {
                MessageBoxResult result = WinUIMessageBox.Show("[" + SelectedWrkItem.RN + "/" + SelectedWrkItem.WRK_DOC_NM + "]" + " 정말로 삭제 하시겠습니까?", "제품도면관리(작업표준서)", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    SelectedWrkItem.DELT_FLG = "Y";
                    SelectedWrkItem.UPD_USR_ID = SystemProperties.USER;
                    SelectedWrkItem.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;

                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6681/wrk/u", new StringContent(JsonConvert.SerializeObject(SelectedWrkItem), System.Text.Encoding.UTF8, "application/json")))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            int _Num = 0;
                            string resultMsg = await response.Content.ReadAsStringAsync();
                            if (int.TryParse(resultMsg, out _Num) == false)
                            {
                                //실패
                                WinUIMessageBox.Show(resultMsg, "제품도면관리(작업표준서)", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                            WrkRefresh();

                            //성공
                            WinUIMessageBox.Show("삭제가 완료되었습니다.", "제품도면관리(작업표준서)", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                        }
                    }

                }
            }



            //if (SelectedMenuItem == null) 
            //{
            //    return; 
            //}
            ////ShowMasterDialog(SelectedMenuItem);
            //detailDialog = new P4411DetailDialog(SelectedMenuItem);
            //detailDialog.Title = "도면 이력 - 수정 [" + SelectedMenuItem.ITM_CD + " / " + SelectedMenuItem.ITM_NM + "]";
            //detailDialog.Owner = Application.Current.MainWindow;
            //detailDialog.BorderEffect = BorderEffect.Default;
            ////masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            ////masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            //bool isDialog = (bool)detailDialog.ShowDialog();
            //if (isDialog)
            //{
            //    SearchRvisContact();
            //}
        }

        //public ICommand DelRvisDialogCommand
        //{
        //    get
        //    {
        //        if (delDwgRvisDialogCommand == null)
        //            delDwgRvisDialogCommand = new DelegateCommand(DelRvisContact);
        //        return delDwgRvisDialogCommand;
        //    }
        //}


        //public void DelRvisContact()
        //{
        ////    SystemCodeVo delDao = SelectedMenuItem;
        ////    if (delDao != null)
        ////    {
        ////        MessageBoxResult result = WinUIMessageBox.Show("[" + delDao.ITM_CD + "]" + delDao.ITM_NM + " 정말로 삭제 하시겠습니까?", "[삭제]도면이력관리", MessageBoxButton.YesNo, MessageBoxImage.Question);
        ////        if (result == MessageBoxResult.Yes)
        ////        {
        ////            codeClient.DeleteItemImg(delDao);
        ////            //codeClient.DeleteItemCode(delDao);

        ////            WinUIMessageBox.Show("삭제가 완료되었습니다.", "[삭제]도면이력관리", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
        ////            SearchMenuContact();
        ////        }
        ////    }
        //}


        ////작업표준서 이력관리
        //public ICommand SearchWrkRvisDialogCommand
        //{
        //    get
        //    {
        //        if (searchWrkRvisDialogCommand == null)
        //            searchWrkRvisDialogCommand = new DelegateCommand(SearchWrkRvisContact);
        //        return searchWrkRvisDialogCommand;
        //    }
        //}
        //public void SearchWrkRvisContact()
        //{
        //    try
        //    {
        //        RvisRefresh();
        //    }
        //    catch
        //    {
        //        return;
        //    }
        //}

        //public ICommand NewWrkRvisDialogCommand
        //{
        //    get
        //    {
        //        if (newWrkRvisDialogCommand == null)
        //            newWrkRvisDialogCommand = new DelegateCommand(NewWrkRvisContact);
        //        return newWrkRvisDialogCommand;
        //    }
        //}

        //public void NewWrkRvisContact()
        //{
        //    if (SelectedMenuItem == null)
        //    {
        //        return;
        //    }

        //    wrkDialog = new P4411WrkDialog(new MakeVo() { ITM_CD = SelectedMenuItem.ITM_CD, ITM_NM = SelectedMenuItem.ITM_NM });
        //    wrkDialog.Title = "작업표준서 이력 - 추가 [" + SelectedMenuItem.ITM_CD + " / " + SelectedMenuItem.ITM_NM + "]";
        //    wrkDialog.Owner = Application.Current.MainWindow;
        //    wrkDialog.BorderEffect = BorderEffect.Default;
        //    //masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
        //    //masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
        //    bool isDialog = (bool)wrkDialog.ShowDialog();
        //    if (isDialog)
        //    {
        //        //SearchRvisContact();
        //        Refresh();
        //        if (SelectedMenuViewList.Count > 0)
        //        {
        //            SelectedMenuItem = SelectedMenuViewList.Where<MakeVo>(x => x.ITM_CD.Equals(wrkDialog._ITM_CD)).First<MakeVo>();
        //        }
        //        ////도면 이력 조회
        //        //if (SelectedRvisViewList.Count > 0)
        //        //{
        //        //    SelectedRvisItem = SelectedRvisViewList[0];
        //        //}

        //    }
        //}

        //public ICommand EditWrkRvisDialogCommand
        //{
        //    get
        //    {
        //        if (editWrkRvisDialogCommand == null)
        //            editWrkRvisDialogCommand = new DelegateCommand(EditWrkRvisContact);
        //        return editWrkRvisDialogCommand;
        //    }
        //}

        //public void EditWrkRvisContact()
        //{
        //    MakeVo delDao = SelectedWrkRvisItem;
        //    if (delDao != null)
        //    {
        //        MessageBoxResult result = WinUIMessageBox.Show("[" + delDao.RN + "/" + delDao.WRK_DOC_NM + "]" + " 정말로 삭제 하시겠습니까?", "[삭제]도면이력관리", MessageBoxButton.YesNo, MessageBoxImage.Question);
        //        if (result == MessageBoxResult.Yes)
        //        {
        //            delDao.DELT_FLG = "Y";
        //            delDao.UPD_USR_ID = SystemProperties.USER;
        //            MakeVo resultVo = makeClient.P4411_UpdateWrk(delDao);
        //            if (!resultVo.isSuccess)
        //            {
        //                //실패
        //                WinUIMessageBox.Show(resultVo.Message, "[에러]도면이력관리", MessageBoxButton.OK, MessageBoxImage.Error);
        //                return;
        //            }
        //            //성공
        //            WinUIMessageBox.Show("삭제가 완료되었습니다.", "[삭제]도면이력관리", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);

        //            Refresh();
        //            if (SelectedMenuViewList.Count > 0)
        //            {
        //                SelectedMenuItem = SelectedMenuViewList.Where<MakeVo>(x => x.ITM_CD.Equals(delDao.ITM_CD)).First<MakeVo>();
        //            }
        //        }
        //    }
        //}

        //public ICommand DelRvisDialogCommand
        //{
        //    get
        //    {
        //        if (delDwgRvisDialogCommand == null)
        //            delDwgRvisDialogCommand = new DelegateCommand(DelRvisContact);
        //        return delDwgRvisDialogCommand;
        //    }
        //}


        //public void DelRvisContact()
        //{
        //    //    SystemCodeVo delDao = SelectedMenuItem;
        //    //    if (delDao != null)
        //    //    {
        //    //        MessageBoxResult result = WinUIMessageBox.Show("[" + delDao.ITM_CD + "]" + delDao.ITM_NM + " 정말로 삭제 하시겠습니까?", "[삭제]도면이력관리", MessageBoxButton.YesNo, MessageBoxImage.Question);
        //    //        if (result == MessageBoxResult.Yes)
        //    //        {
        //    //            codeClient.DeleteItemImg(delDao);
        //    //            //codeClient.DeleteItemCode(delDao);

        //    //            WinUIMessageBox.Show("삭제가 완료되었습니다.", "[삭제]도면이력관리", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
        //    //            SearchMenuContact();
        //    //        }
        //    //    }
        //}

        //public ICommand DownloadRvisDialogCommand
        //{
        //    get
        //    {
        //        if (downloadRvisDialogCommand == null)
        //            downloadRvisDialogCommand = new DelegateCommand<string>(DownloadRvisContact);
        //        return downloadRvisDialogCommand;
        //    }
        //}

        [Command]
        public void DownloadRvisContact(string parameter)
        {
            if (parameter.Equals("D"))
            {
                ManVo dao = SelectedDwgItem;
                if (dao != null)
                {
                    downloaddialog = new System.Windows.Forms.FolderBrowserDialog();
                    downloaddialog.ShowNewFolderButton = true;
                    downloaddialog.Description = "[파일 : " + dao.DWG_FILE_NM + "] 저장 폴더를 선택 해 주세요.";
                    downloaddialog.RootFolder = System.Environment.SpecialFolder.Desktop;
                    downloaddialog.ShowDialog();
                    if (!string.IsNullOrEmpty(downloaddialog.SelectedPath))
                    {
                        if (!string.IsNullOrEmpty(dao.DWG_FILE_NM))
                        {
                            int ArraySize = dao.DWG_FILE.Length;
                            this.fs = new FileStream(downloaddialog.SelectedPath + "/" + dao.DWG_FILE_NM, FileMode.OpenOrCreate, FileAccess.Write);
                            this.fs.Write(dao.DWG_FILE, 0, ArraySize);
                            this.fs.Close();
                        }
                        System.Diagnostics.Process.Start(downloaddialog.SelectedPath);
                    }
                }
                return;
            }
            else if (parameter.Equals("W"))
            {
                ManVo dao = SelectedWrkItem;
                if (dao != null)
                {
                    downloaddialog = new System.Windows.Forms.FolderBrowserDialog();
                    downloaddialog.ShowNewFolderButton = true;
                    downloaddialog.Description = "[파일 : " + dao.WRK_DOC_FILE_NM + "] 저장 폴더를 선택 해 주세요.";
                    downloaddialog.RootFolder = System.Environment.SpecialFolder.Desktop;
                    downloaddialog.ShowDialog();
                    if (!string.IsNullOrEmpty(downloaddialog.SelectedPath))
                    {
                        if (!string.IsNullOrEmpty(dao.WRK_DOC_FILE_NM))
                        {
                            int ArraySize = dao.WRK_DOC_FILE.Length;
                            this.fs = new FileStream(downloaddialog.SelectedPath + "/" + dao.WRK_DOC_FILE_NM, FileMode.OpenOrCreate, FileAccess.Write);
                            this.fs.Write(dao.WRK_DOC_FILE, 0, ArraySize);
                            this.fs.Close();
                        }
                        System.Diagnostics.Process.Start(downloaddialog.SelectedPath);
                    }
                }
                return;
            }
        }

        //public ICommand ImageRvisDialogCommand
        //{
        //    get
        //    {
        //        if (imageRvisDialogCommand == null)
        //            imageRvisDialogCommand = new DelegateCommand<string>(ImageRvisContact);
        //        return imageRvisDialogCommand;
        //    }
        //}

        public void ImageRvisContact(string parameter)
        {
            //if (parameter.Equals("D"))
            //{
                //MakeVo dao = SelectedDwgRvisItem;
                //if (dao != null)
                //{
                //    imgDetailDialog = new P4413ImageDialog(dao);
                //    imgDetailDialog.Title = "파일 - 이미지 [" + dao.EQ_FILE_NM + "]";
                //    imgDetailDialog.Owner = Application.Current.MainWindow;
                //    imgDetailDialog.BorderEffect = BorderEffect.Default;
                //    //masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
                //    //masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
                //    bool isDialog = (bool)imgDetailDialog.ShowDialog();
                //    if (isDialog)
                //    {

                //    }
                //}
                //return;
            //}
            //else if (parameter.Equals("W"))
            //{
            //    MakeVo dao = SelectedWrkRvisItem;
            //    if (dao != null)
            //    {
            //        dao.DWG_FILE = dao.WRK_DOC_FILE;
            //        imgDetailDialog = new P4411ImageDialog(dao);
            //        imgDetailDialog.Title = "파일 - 이미지 [" + dao.WRK_DOC_NM + "]";
            //        imgDetailDialog.Owner = Application.Current.MainWindow;
            //        imgDetailDialog.BorderEffect = BorderEffect.Default;
            //        //masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            //        //masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            //        bool isDialog = (bool)imgDetailDialog.ShowDialog();
            //        if (isDialog)
            //        {

            //        }
            //    }
            //    return;
            //}
        }

        #endregion
    }
}
