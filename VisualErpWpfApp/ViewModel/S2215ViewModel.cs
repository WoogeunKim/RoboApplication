using AquilaErpWpfApp3.Util;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Code;
using ModelsLibrary.Sale;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Windows;

namespace AquilaErpWpfApp3.ViewModel
{
    public sealed class S2215ViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private string title = "수주현황";

        //private static SaleOrderServiceClient saleOrderClient = SystemProperties.SaleOrderClient;
        private IList<SaleVo> selectedMstList = new List<SaleVo>();

        //private ObservableCollection<PurVo> selectedDtlList = new ObservableCollection<PurVo>();
        //private IList<JobVo> selectedDtlList = new List<JobVo>();

        //private S2221MasterDialog masterDialog;

        ////Menu Dialog
        //private ICommand _searchDialogCommand;
        //private ICommand _newDialogCommand;
        //private ICommand _editDetailDialogCommand;
        //private ICommand _delDialogCommand;

        public S2215ViewModel() 
        {
            StartDt = System.DateTime.Now;
            EndDt = System.DateTime.Now;


            ////사업장
            //AreaList = SystemProperties.SYSTEM_CODE_VO("L-002");
            //_AreaMap = SystemProperties.SYSTEM_CODE_MAP("L-002");
            //if (AreaList.Count > 0)
            //{
            //    M_SL_AREA_NM = SystemProperties.USER_VO.EMPE_PLC_CD;
            //}
            //사업장
            SYSTEM_CODE_VO();
            // - Refresh();
        }

        [Command]
        public async void Refresh()
        {
            try
            {
                SaleVo _param = new SaleVo();
                _param.FM_DT = (StartDt).ToString("yyyy-MM-dd");
                _param.TO_DT = (EndDt).ToString("yyyy-MM-dd");
                //사업장
                _param.AREA_CD = M_SL_AREA_NM.CLSS_CD;
                _param.AREA_NM = M_SL_AREA_NM.CLSS_DESC;
                //채널
                _param.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;

                if (EndDt < StartDt)
                {
                    WinUIMessageBox.Show("조회일자가 올바르지 않습니다.", title, MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                ////
                //SelectMstList = saleOrderClient.S2221SelectMstList(_param);
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("S2215/mst", new StringContent(JsonConvert.SerializeObject(_param), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectMstList = JsonConvert.DeserializeObject<IEnumerable<SaleVo>>(await response.Content.ReadAsStringAsync()).Cast<SaleVo>().ToList();


                        setTitle();

                        if (SelectMstList.Count >= 1)
                        {
                            isM_UPDATE = true;
                            isM_DELETE = true;


                            //if (string.IsNullOrEmpty(_CLT_BIL_NO))
                            //{
                                SelectedMstItem = SelectMstList[0];
                            //}
                            //else
                            //{
                            //    SelectedMstItem = SelectMstList.Where(x => x.CLT_BIL_NO.Equals(_CLT_BIL_NO)).LastOrDefault<SaleVo>();
                            //}
                        }
                        else
                        {
                            //SelectDtlList = null;
                            //SearchDetail = null;

                            isM_UPDATE = false;
                            isM_DELETE = false;
                            //
                            //isD_UPDATE = false;
                            //isD_DELETE = false;
                        }
                    }
                    //DXSplashScreen.Close();
                }
            }
            catch (System.Exception eLog)
            {
                //DXSplashScreen.Close();
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
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

        //private Dictionary<string, string> _DeptMap = new Dictionary<string, string>();
        //private IList<CodeDao> _DeptCd = new List<CodeDao>();
        //public IList<CodeDao> DeptList
        //{
        //    get { return _DeptCd; }
        //    set { SetProperty(ref _DeptCd, value, () => DeptList); }
        //}

        //private string _M_DEPT_DESC;
        //public string M_DEPT_DESC
        //{
        //    get { return _M_DEPT_DESC; }
        //    set { SetProperty(ref _M_DEPT_DESC, value, () => M_DEPT_DESC); }
        //}


        private string _M_SEARCH_TEXT = string.Empty;
        public string M_SEARCH_TEXT
        {
            get { return _M_SEARCH_TEXT; }
            set { SetProperty(ref _M_SEARCH_TEXT, value, () => M_SEARCH_TEXT); }
        }

        //private bool? _M_SEARCH_CHECKD = false;
        //public bool? M_SEARCH_CHECKD
        //{
        //    get { return _M_SEARCH_CHECKD; }
        //    set { SetProperty(ref _M_SEARCH_CHECKD, value, () => M_SEARCH_CHECKD); }
        //}


        public IList<SaleVo> SelectMstList
        {
            get { return selectedMstList; }
            set { SetProperty(ref selectedMstList, value, () => SelectMstList); }
        }

        SaleVo _selectedMstItem;
        public SaleVo SelectedMstItem
        {
            get
            {
                return _selectedMstItem;
            }
            set
            {
                if (value != null)
                {
                    SetProperty(ref _selectedMstItem, value, () => SelectedMstItem, SelectMstDetail);
                }
            }
        }

        //private bool? _isM_SAVE = false;
        //public bool? isM_SAVE
        //{
        //    get { return _isM_SAVE; }
        //    set { SetProperty(ref _isM_SAVE, value, () => isM_SAVE); }
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

        //
        //private bool? _isD_UPDATE = false;
        //public bool? isD_UPDATE
        //{
        //    get { return _isD_UPDATE; }
        //    set { SetProperty(ref _isD_UPDATE, value, () => isD_UPDATE); }
        //}

        //private bool? _isD_DELETE = false;
        //public bool? isD_DELETE
        //{
        //    get { return _isD_DELETE; }
        //    set { SetProperty(ref _isD_DELETE, value, () => isD_DELETE); }
        //}


        //
        //사업장
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

        private void SelectMstDetail()
        {
            //if (this.SelectedMstItem != null)
            //{
            //    if (this.SelectedMstItem.CLT_BIL_NM == null)
            //    {
            //        this.DetailView = "1";
            //    }
            //    else if (this.SelectedMstItem.CLT_BIL_NM.Equals("통장") || this.SelectedMstItem.CLT_BIL_NM.Equals("카드"))
            //    {
            //        this.DetailView = "1";
            //    }
            //    else if (this.SelectedMstItem.CLT_BIL_NM.Equals("어음(자수)") || this.SelectedMstItem.CLT_BIL_NM.Equals("어음(타수)"))
            //    {
            //        this.DetailView = "2";
            //    }
            //    else
            //    {
            //        this.DetailView = "1";
            //    }
            //}

            //try
            //{
            //    //DXSplashScreen.Show<ProgressWindow>();

            //    if (this._selectedMstItem == null)
            //    {
            //        return;
            //    }
            //    Title = "[기간]" + (StartDt).ToString("yyyy-MM-dd") + "~" + (EndDt).ToString("yyyy-MM-dd") + ",  [사업장]" + M_SL_AREA_NM + ",  [구분]" + M_ITM_GRP_CLSS_CD + ",  [품번]" + SelectedMstItem.ITM_CD + ",  [품명]" + SelectedMstItem.ITM_NM + ",  [규격]" + SelectedMstItem.ITM_SZ_NM + (string.IsNullOrEmpty(M_SEARCH_TEXT) ? "" : (",   [검색]" + M_SEARCH_TEXT)); 

            //    //SelectDtlList = new ObservableCollection<PurVo>(purClient.P4401SelectDtlList(new PurVo() { ITM_CD = this._selectedMstItem.}));
            //    SelectDtlList = codeClient.S1144SelectDtlList(SelectedMstItem);
            //    // //
            //    if (SelectDtlList.Count >= 1)
            //    {
            //        isD_UPDATE = true;
            //        isD_DELETE = true;

            //        SearchDetail = SelectDtlList[0];
            //    }
            //    else
            //    {
            //        isD_UPDATE = false;
            //        isD_DELETE = false;
            //    }

            //    //DXSplashScreen.Close();
            //}
            //catch (System.Exception eLog)
            //{
            //    //DXSplashScreen.Close();
            //    //
            //    WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
            //    return;
            //}
        }
        //#endregion


        //#region Functon <Detail List>
        //public IList<JobVo> SelectDtlList
        //{
        //    get { return selectedDtlList; }
        //    set { SetProperty(ref selectedDtlList, value, () => SelectDtlList); }
        //}

        //JobVo _searchDetail;
        //public JobVo SearchDetail
        //{
        //    get
        //    {
        //        return _searchDetail;
        //    }
        //    set
        //    {
        //        if (value != null)
        //        {
        //            SetProperty(ref _searchDetail, value, () => SearchDetail);
        //        }
        //    }
        //}
        //#endregion


        string _Title = string.Empty;
        public string Title
        {
            get { return _Title; }
            set { SetProperty(ref _Title, value, () => Title); }
        }


        //string _isDetailView = string.Empty;
        //public string DetailView
        //{
        //    get { return _isDetailView; }
        //    set { SetProperty(ref _isDetailView, value, () => DetailView); }
        //}


        //#region Functon Command <add, Edit, Del>
        //public ICommand SearchDialogCommand
        //{
        //    get
        //    {
        //        if (_searchDialogCommand == null)
        //            _searchDialogCommand = new DelegateCommand(Refresh);
        //        return _searchDialogCommand;
        //    }
        //}

        //public ICommand NewDialogCommand
        //{
        //    get
        //    {
        //        if (_newDialogCommand == null)
        //            _newDialogCommand = new DelegateCommand(NewContact);
        //        return _newDialogCommand;
        //    }
        //}

        //[Command]
        //public void NewContact()
        //{
        //    masterDialog = new S2221MasterDialog(new SaleVo() { AREA_NM = M_SL_AREA_NM.CLSS_DESC, AREA_CD = M_SL_AREA_NM.CLSS_CD, CLT_BIL_DT = (StartDt).ToString("yyyy-MM-dd") });
        //    masterDialog.Title = title + " - 추가";
        //    masterDialog.Owner = Application.Current.MainWindow;
        //    masterDialog.BorderEffect = BorderEffect.Default;
        //    masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
        //    masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
        //    bool isDialog = (bool)masterDialog.ShowDialog();
        //    if (isDialog)
        //    {
        //        Refresh(masterDialog.CLT_BIL_NO);
        //    }
        //        //    if (masterDialog.IsEdit == false)
        //        //    {
        //        //        try
        //        //        {
        //        //            DXSplashScreen.Show<ProgressWindow>();
        //        //            Refresh();
        //        //            DXSplashScreen.Close();

        //        //            for (int x = 0; x < SelectMstList.Count; x++)
        //        //            {
        //        //                if ((masterDialog.CLT_BIL_NO).Equals(SelectMstList[x].CLT_BIL_NO))
        //        //                {
        //        //                    SelectedMstItem = SelectMstList[x];
        //        //                    return;
        //        //                }
        //        //            }

        //        //        }
        //        //        catch (System.Exception eLog)
        //        //        {
        //        //            DXSplashScreen.Close();
        //        //            WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
        //        //            return;
        //        //        }
        //        //    }
        //        //}
        //}

        ////public ICommand EditDtlDialogCommand
        ////{
        ////    get
        ////    {
        ////        if (_editDetailDialogCommand == null)
        ////            _editDetailDialogCommand = new DelegateCommand(EditDtlContact);
        ////        return _editDetailDialogCommand;
        ////    }
        ////}

        //[Command]
        //public void EditDtlContact()
        //{
        //    if (SelectedMstItem == null)
        //    {
        //        return;
        //    }

        //    masterDialog = new S2221MasterDialog(SelectedMstItem);
        //    masterDialog.Title = title + " - 수정(" + SelectedMstItem.CLT_BIL_NO + ")";
        //    masterDialog.Owner = Application.Current.MainWindow;
        //    masterDialog.BorderEffect = BorderEffect.Default;
        //    masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
        //    masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
        //    bool isDialog = (bool)masterDialog.ShowDialog();
        //    if (isDialog)
        //    {
        //        Refresh(masterDialog.CLT_BIL_NO);
        //    }
        //    //    if (masterDialog.IsEdit == false)
        //    //    {
        //    //        try
        //    //        {
        //    //            DXSplashScreen.Show<ProgressWindow>();
        //    //            Refresh();
        //    //            DXSplashScreen.Close();
        //    //        }
        //    //        catch (System.Exception eLog)
        //    //        {
        //    //            DXSplashScreen.Close();
        //    //            WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
        //    //            return;
        //    //        }
        //    //    }
        //    //}
        //}

        ////public ICommand DelDialogCommand
        ////{
        ////    get
        ////    {
        ////        if (_delDialogCommand == null)
        ////            _delDialogCommand = new DelegateCommand(DelContact);
        ////        return _delDialogCommand;
        ////    }
        ////}

        //[Command]
        //public async void DelContact()
        //{
        //    if (SelectedMstItem != null)
        //    {
        //        MessageBoxResult result = WinUIMessageBox.Show("[" + SelectedMstItem.CLT_BIL_NO + "] 정말로 삭제 하시겠습니까?", title, MessageBoxButton.YesNo, MessageBoxImage.Question);
        //        if (result == MessageBoxResult.Yes)
        //        {
        //                    try
        //                    {
        //                        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s2221/mst/d", new StringContent(JsonConvert.SerializeObject(SelectedMstItem), System.Text.Encoding.UTF8, "application/json")))
        //                        {
        //                            if (response.IsSuccessStatusCode)
        //                            {
        //                                int _Num = 0;
        //                                string resultMsg = await response.Content.ReadAsStringAsync();
        //                                if (int.TryParse(resultMsg, out _Num) == false)
        //                                {
        //                                    //실패
        //                                    WinUIMessageBox.Show(resultMsg, title, MessageBoxButton.OK, MessageBoxImage.Error);
        //                                    return;
        //                                }
        //                                Refresh();

        //                                //성공
        //                                WinUIMessageBox.Show("삭제가 완료되었습니다.", title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
        //                            }
        //                        }
        //                    }
        //                    catch (System.Exception eLog)
        //                    {
        //                        WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
        //                        return;
        //                    }
        //            //saleOrderClient.S2221DeleteMst(SelectedMstItem);
        //            //WinUIMessageBox.Show("삭제가 완료되었습니다.", title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
        //            //Refresh();
        //        }
        //    }
        //}



        public void setTitle()
        {
            Title = "[기간]" + (StartDt).ToString("yyyy-MM-dd") + "~" + (EndDt).ToString("yyyy-MM-dd") + ", [사업장]" + M_SL_AREA_NM.CLSS_DESC ;
        }

        public async void SYSTEM_CODE_VO()
        {
            //SL_AREA_LIST = SystemProperties.SYSTEM_CODE_VO("L-002");
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "L-002"))
            {
                if (response.IsSuccessStatusCode)
                {
                    AreaList = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                    if (AreaList.Count > 0)
                    {
                        M_SL_AREA_NM = AreaList[0];
                    }
                }
            }
            Refresh();
        }

    }
}
