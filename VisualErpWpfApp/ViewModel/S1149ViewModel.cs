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
using AquilaErpWpfApp3.Util;
using AquilaErpWpfApp3.View.S.Dialog;

namespace AquilaErpWpfApp3.ViewModel
{
    public sealed class S1149ViewModel : ViewModelBase, INotifyPropertyChanged 
    {
        private string title = "품목 포장 관리";
        //private static CodeServiceClient codeClient = SystemProperties.CodeClient;

        //private Dictionary<string, string> _ItemDivision = new Dictionary<string, string>();
        //private Dictionary<string, string> _ItemN1st = new Dictionary<string, string>();
        //private IList<CodeDao> itemN1st;
        private IList<SystemCodeVo> selectedMenuViewList;

        private IList<SystemCodeVo> selectedDtlViewList;
        //Menu Dialog
        //private ICommand searchDialogCommand;
        //private ICommand newDialogCommand;
        //private ICommand editDialogCommand;
        //private ICommand delDialogCommand;

        //private ICommand fileDownloadCommand;

        //private ICommand classDialogCommand;

        private S1149MasterDialog masterDialog;
        //private S143ClassDialog classDialog;

        //private bool? seek_ap = true;
        //private bool? seek_ar = true;
        //private bool? seek_or = true;
        //private bool? seek_su = true;



        public S1149ViewModel() 
        {

            //
            //IList<SystemCodeVo> ItemDetailVo = codeClient.SelectDetailCode(new SystemCodeVo() { DELT_FLG = "N", CLSS_TP_CD = "L-001", CHNL_CD = SystemProperties.USER_VO.CHNL_CD });
            //int nCnt = ItemDetailVo.Count;
            //SystemCodeVo tmpVo;

            //this._ItemDivision.Clear();
            //for (int x = 0; x < nCnt; x++)
            //{
            //    tmpVo = ItemDetailVo[x];
            //    this._ItemDivision.Add(tmpVo.CLSS_DESC, tmpVo.CLSS_CD);
            //}
            ////
            //if (nCnt > 0)
            //{
            //    SelectedTypeItem = new List<string>(_ItemDivision.Keys)[0];
            //}

            //AreaList = SystemProperties.SYSTEM_CODE_VO("L-001");
            //_AreaMap = SystemProperties.SYSTEM_CODE_MAP("L-001");
            //if (AreaList.Count > 0)
            //{
            //    M_SL_AREA_NM = AreaList[0].CLSS_DESC;
            //}

            SYSTEM_CODE_VO("L-001");
        }

        [Command]
        public async void Refresh(string _ITM_CD = null)
        {
            try
            {
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s141", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { ITM_GRP_CLSS_CD = M_SL_AREA_NM.CLSS_CD, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectedMenuViewList = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                    }
                    //SelectedMenuViewList = codeClient.SelectItemList(new SystemCodeVo() { ITM_GRP_CLSS_CD = this._AreaMap[M_SL_AREA_NM], CHNL_CD = SystemProperties.USER_VO.CHNL_CD });
                    //SearchMenuContact();
                    if (SelectedMenuViewList.Count > 0)
                    {
                        //isM_UPDATE = true;
                        //isM_DELETE = true;
                        isM_UPDATE = false;
                        isM_DELETE = false;
                        if (string.IsNullOrEmpty(_ITM_CD))
                        {
                            SelectedMenuItem = SelectedMenuViewList[0];
                        }
                        else
                        {
                            SelectedMenuItem = SelectedMenuViewList.Where(x => x.ITM_CD.Equals(_ITM_CD)).LastOrDefault<SystemCodeVo>();
                        }
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
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }

        #region Functon (Menu Add, Edit, Del)
        //public IList<string> ItemDivision
        //{
        //    get { return new List<string>(_ItemDivision.Keys); }
        //}

        //string _typeItem;
        //public string SelectedTypeItem
        //{
        //    get { return _typeItem; }
        //    set { SetProperty(ref _typeItem, value, () => SelectedTypeItem, N1stList); }
        //}

        //private Dictionary<string, string> _AreaMap = new Dictionary<string, string>();
        private IList<SystemCodeVo> _AreaCd = new List<SystemCodeVo>();
        public IList<SystemCodeVo> AreaList
        {
            get { return _AreaCd; }
            set { SetProperty(ref _AreaCd, value, () => AreaList); }
        }

        //사업장
        private SystemCodeVo _M_SL_AREA_NM;
        public SystemCodeVo M_SL_AREA_NM
        {
            get { return _M_SL_AREA_NM; }
            set { SetProperty(ref _M_SL_AREA_NM, value, () => M_SL_AREA_NM); }
        }


        //void N1stList()
        //{
            //if (string.IsNullOrEmpty(SelectedTypeItem))
            //{
            //    return;
            //}

            //IList<SystemCodeVo> N1stVoList = codeClient.SelectCodeItemGroupList(new SystemCodeVo() { DELT_FLG = "N", ITM_GRP_CLSS_CD = this._ItemDivision[SelectedTypeItem], CRE_USR_ID = "", CHNL_CD = SystemProperties.USER_VO.CHNL_CD });
            //int nCnt = N1stVoList.Count;
            //SystemCodeVo tmpVo;

            ////this._ItemN1st.Clear();
            //ItemN1st = new List<CodeDao>();
            //for (int x = 0; x < nCnt; x++)
            //{
            //    tmpVo = N1stVoList[x];
            //    ItemN1st.Add(new CodeDao (){ CLSS_DESC = tmpVo.ITM_GRP_RP_NM, CLSS_CD = tmpVo.ITM_GRP_CD});
            //}
            ////
            //if (nCnt > 0)
            //{
            //    SelectedN1stItem = ItemN1st[0];
            //}
        //}

        //public IList<CodeDao> ItemN1st
        //{
        //    get { return itemN1st; }
        //    set { SetProperty(ref itemN1st, value, () => ItemN1st); }
        //}


        //CodeDao _selectedN1stItem;
        //public CodeDao SelectedN1stItem
        //{
        //    get { return _selectedN1stItem; }
        //    set { SetProperty(ref _selectedN1stItem, value, () => SelectedN1stItem); }
        //}

        //
        public IList<SystemCodeVo> SelectedMenuViewList
        {
            get { return selectedMenuViewList; }
            set { SetProperty(ref selectedMenuViewList, value, () => SelectedMenuViewList); }
        }

        SystemCodeVo _selectMenuItem;
        public SystemCodeVo SelectedMenuItem
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


        //
        public IList<SystemCodeVo> SelectedDtlViewList
        {
            get { return selectedDtlViewList; }
            set { SetProperty(ref selectedDtlViewList, value, () => SelectedDtlViewList); }
        }

        SystemCodeVo _selectDtlItem;
        public SystemCodeVo SelectedDtlItem
        {
            get
            {
                return _selectDtlItem;
            }
            set
            {
                if (value != null)
                {
                    SetProperty(ref _selectDtlItem, value, () => SelectedDtlItem);
                }
            }
        }



        public async void SearchDetailItem()
        {
            if (this.SelectedMenuItem == null)
            {
                return;
            }

            this.SelectedDtlItem = new SystemCodeVo() { N1ST_UOM_CD = "", N1ST_UOM_NM = "", N1ST_QTY = 0, N1ST_WGT = 0, N2ND_UOM_CD = "", N2ND_UOM_NM = "", N2ND_QTY = 0, N2ND_WGT = 0 };
            //    SearchItem = customerClient.SelectCustomerDetailCode(this._selectMenuItem);
            if (SelectedMenuItem.RN > 0)
            {
                isM_UPDATE = true;
                isM_DELETE = true;

                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s1149", new StringContent(JsonConvert.SerializeObject(SelectedMenuItem), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectedDtlViewList = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();

                        if (this.SelectedDtlViewList.Count > 0)
                        {
                            this.SelectedDtlItem = this.SelectedDtlViewList[0];
                        }
                    }
                }
            }
            else
            {
                isM_UPDATE = false;
                isM_DELETE = false;
            }
        }

        //ItemCodeVo _searchItem;
        //public ItemCodeVo SearchItem
        //{
        //    get
        //    {
        //        return _searchItem;
        //    }
        //    set
        //    {
        //        if (value != null)
        //        {
        //            SetProperty(ref _searchItem, value, () => SearchItem);
        //        }
        //    }
        //}

        //public ICommand SearchDialogCommand
        //{
        //    get
        //    {
        //        if (searchDialogCommand == null)
        //            searchDialogCommand = new DelegateCommand(SearchMenuContact);
        //        return searchDialogCommand;
        //    }
        //}

        //public void SearchMenuContact()
        //{
        //    try
        //    {
        //        DXSplashScreen.Show<ProgressWindow>();
        //        //WinUIMessageBox.Show("매입처 : " + seek_M + ", 매출처 : " + seek_C + ", 구입처 : " + seek_W, "검색 조건", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
        //        //string _ap = (seek_ap == true ? "AP" : "X");
        //        //string _ar = (seek_ar == true ? "AR" : "X");
        //        //string _or = (seek_or == true ? "OR" : "X");
        //        //string _su = (seek_su == true ? "SU" : "X");
        //        Refresh();

        //        //if (string.IsNullOrEmpty(SelectedTypeItem)){ return; }
        //        //if (SelectedN1stItem == null) { return; }
        //        ////
        //        //SelectedMenuViewList = codeClient.SelectItemList(new SystemCodeVo() { ITM_GRP_CLSS_CD = this._ItemDivision[SelectedTypeItem], N1ST_ITM_GRP_CD = SelectedN1stItem.CLSS_CD, CHNL_CD =SystemProperties.USER_VO.CHNL_CD });
        //        DXSplashScreen.Close();
        //    }
        //    catch
        //    {
        //        DXSplashScreen.Close();
        //        return;
        //    }
        //}

        //public ICommand NewDialogCommand
        //{
        //    get
        //    {
        //        if (newDialogCommand == null)
        //            newDialogCommand = new DelegateCommand(NewContact);
        //        return newDialogCommand;
        //    }
        //}

        [Command]
        public void NewContact()
        {
            //masterDialog = new S1149MasterDialog(new SystemCodeVo() { ITM_GRP_CLSS_CD = M_SL_AREA_NM.CLSS_CD, ITM_GRP_CLSS_NM = M_SL_AREA_NM.CLSS_DESC, CHNL_CD = SystemProperties.USER_VO.CHNL_CD });
            //masterDialog.Title = "품목 포장 관리 - 추가";
            //masterDialog.Owner = Application.Current.MainWindow;
            //masterDialog.BorderEffect = BorderEffect.Default;
            //masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            //masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            //bool isDialog = (bool)masterDialog.ShowDialog();
            //if (isDialog)
            //{
            //    Refresh(masterDialog.resultDao.ITM_CD);
            //    //if (masterDialog.IsEdit == false)
            //    //{
            //    //    SearchMenuContact();

            //    //    for (int x = 0; x < SelectedMenuViewList.Count; x++)
            //    //    {
            //    //        if (SelectedMenuViewList[x].ITM_CD.Equals(masterDialog.resultDao.ITM_CD))
            //    //        {
            //    //            SelectedMenuItem = SelectedMenuViewList[x];
            //    //            break;
            //    //        }

            //    //    }
            //    //}
            //}
        }

        //public ICommand EditDialogCommand
        //{
        //    get
        //    {
        //        if (editDialogCommand == null)
        //            editDialogCommand = new DelegateCommand(EditMasterContact);
        //        return editDialogCommand;
        //    }
        //}

        [Command]
        public void EditMasterContact()
        {
            if (SelectedMenuItem == null) { return; }
            else if (SelectedDtlItem == null) { return; }

            //포장 단위
            SelectedMenuItem.N1ST_UOM_CD = SelectedDtlItem.N1ST_UOM_CD;
            SelectedMenuItem.N1ST_UOM_NM = SelectedDtlItem.N1ST_UOM_NM;
            SelectedMenuItem.N1ST_QTY = SelectedDtlItem.N1ST_QTY;
            SelectedMenuItem.N1ST_WGT = SelectedDtlItem.N1ST_WGT;

            //바코드 포장
            SelectedMenuItem.N2ND_UOM_CD = SelectedDtlItem.N2ND_UOM_CD;
            SelectedMenuItem.N2ND_UOM_NM = SelectedDtlItem.N2ND_UOM_NM;
            SelectedMenuItem.N2ND_QTY = SelectedDtlItem.N2ND_QTY;
            SelectedMenuItem.N2ND_WGT = SelectedDtlItem.N2ND_WGT;

            //ShowMasterDialog(SelectedMenuItem);
            masterDialog = new S1149MasterDialog(SelectedMenuItem);
            masterDialog.Title = "품목 포장 관리 - 수정";
            masterDialog.Owner = Application.Current.MainWindow;
            masterDialog.BorderEffect = BorderEffect.Default;
            masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            bool isDialog = (bool)masterDialog.ShowDialog();
            if (isDialog)
            {
                //SearchDetailItem();
                Refresh(SelectedMenuItem.ITM_CD);
                //if (masterDialog.IsEdit == true)
                //{
                //    SelectedMenuItem.DELT_FLG = SelectedMenuItem.DELT_FLG;
                //}
            }
            
        }

        //public void ShowMasterDialog(ItemCodeVo dao)
        //{
        //    masterDialog = new S141MasterDialog(dao);
        //    masterDialog.Title = "품목 마스터 관리";
        //    masterDialog.Owner = Application.Current.MainWindow;
        //    masterDialog.BorderEffect = BorderEffect.Default;
        //    //masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
        //    //masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
        //    bool isDialog = (bool)masterDialog.ShowDialog();
        //    if (isDialog)
        //    {
        //        if (masterDialog.IsEdit == false)
        //        {
        //            SearchMenuContact();
        //        }
        //        else
        //        {
        //            SelectedMenuItem.DELT_FLG = dao.DELT_FLG;
        //        }
        //    }
        //}

        //public ICommand DelDialogCommand
        //{
        //    get
        //    {
        //        if (delDialogCommand == null)
        //            delDialogCommand = new DelegateCommand(DelMasterContact);
        //        return delDialogCommand;
        //    }
        //}

        [Command]
        public async void DelMasterContact()
        {
            SystemCodeVo delDao = SelectedMenuItem;
            if (delDao != null)
            {
                MessageBoxResult result = WinUIMessageBox.Show("[" + delDao.ITM_CD + "]" + delDao.ITM_NM + " 정말로 삭제 하시겠습니까?", title, MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    int _Num = 0;
                    string resultMsg = "";
                    //codeClient.DeleteItemCode(delDao);
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s1149/d", new StringContent(JsonConvert.SerializeObject(delDao), System.Text.Encoding.UTF8, "application/json")))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            resultMsg = await response.Content.ReadAsStringAsync();
                            if (int.TryParse(resultMsg, out _Num) == false)
                            {
                                //실패
                                WinUIMessageBox.Show(resultMsg, this.title, MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                            //Refresh();
                            //성공
                            //WinUIMessageBox.Show("삭제가 완료되었습니다.", this.title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                        }
                    }

                    Refresh();
                    WinUIMessageBox.Show("삭제가 완료되었습니다.", title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                    //SearchMenuContact();
                }
            }
        }
        #endregion


        //public ICommand FileDownloadCommand
        //{
        //    get
        //    {
        //        if (fileDownloadCommand == null)
        //            fileDownloadCommand = new DelegateCommand(FileDownload);
        //        return fileDownloadCommand;
        //    }
        //}

        //public void FileDownload()
        //{
        //    ItemCodeVo selDao = SelectedMenuItem;
        //    if (selDao != null)
        //    {
        //        WinUIMessageBox.Show("파일 테스트", "[파일 저장]품목 마스터 등록", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
        //        return;

        //    }
        //}

        //public ICommand ClassDialogCommand
        //{
        //    get
        //    {
        //        if (classDialogCommand == null)
        //            classDialogCommand = new DelegateCommand(EditClassContact);
        //        return classDialogCommand;
        //    }
        //}

        //public void EditClassContact()
        //{
        //    if (SearchItem == null) { return; }
        //    ShowClassDialog(SearchItem);

        //}

        //public void ShowClassDialog(CustomerCodeVo dao)
        //{
        //    classDialog = new S143ClassDialog(dao);
        //    classDialog.Title = "거래처 등급 관리 - " + dao.CO_NM;
        //    classDialog.Owner = Application.Current.MainWindow;
        //    classDialog.BorderEffect = BorderEffect.Default;
        //    //masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
        //    //masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
        //    bool isDialog = (bool)classDialog.ShowDialog();
        //    //if (isDialog)
        //    //{
        //    //    if (masterDialog.IsEdit == false)
        //    //    {
        //    //        SearchMenuContact();
        //    //    }
        //    //    else
        //    //    {
        //    //        SelectedMenuItem.DELT_FLG = dao.DELT_FLG;
        //    //    }
        //    //}
        //}




        //public bool? SEEK_AP
        //{
        //    get
        //    {
        //        return seek_ap;
        //    }
        //    set
        //    {
        //        if (value != null)
        //        {
        //            SetProperty(ref seek_ap, value, () => SEEK_AP);
        //        }
        //    }
        //}

        //public bool? SEEK_AR
        //{
        //    get
        //    {
        //        return seek_ar;
        //    }
        //    set
        //    {
        //        if (value != null)
        //        {
        //            SetProperty(ref seek_ar, value, () => SEEK_AR);
        //        }
        //    }
        //}

        //public bool? SEEK_OR
        //{
        //    get
        //    {
        //        return seek_or;
        //    }
        //    set
        //    {
        //        if (value != null)
        //        {
        //            SetProperty(ref seek_or, value, () => SEEK_OR);
        //        }
        //    }
        //}

        //public bool? SEEK_SU
        //{
        //    get
        //    {
        //        return seek_su;
        //    }
        //    set
        //    {
        //        if (value != null)
        //        {
        //            SetProperty(ref seek_su, value, () => SEEK_SU);
        //        }
        //    }
        //}



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



        public async void SYSTEM_CODE_VO(string _CLSS_TP_CD)
        {
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + _CLSS_TP_CD))
            {
                if (response.IsSuccessStatusCode)
                {
                    AreaList = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                    if (AreaList.Count > 0)
                    {
                        M_SL_AREA_NM = AreaList[0];
                    }

                    //비동기 
                    Refresh();
                }
            }
        }
    }
}
