using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Code;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Windows;
using AquilaErpWpfApp3.Util;

namespace AquilaErpWpfApp3.ViewModel
{
    public sealed class S11315ViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private string title = "로그인 현황";

        //private static CodeServiceClient codeClient = SystemProperties.CodeClient;
        private IList<SystemCodeVo> selectedMstList = new List<SystemCodeVo>();

        //private ObservableCollection<PurVo> selectedDtlList = new ObservableCollection<PurVo>();
        //private IList<SystemCodeVo> selectedDtlList = new List<SystemCodeVo>();

        //private S1144DetailDialog masterDialog;
        //private S1144DetailHistoryDialog historyDialog;

        ////Menu Dialog
        //private ICommand _searchDialogCommand;
        //private ICommand _newDialogCommand;
        //private ICommand _historyDialogCommand;

        public S11315ViewModel() 
        {
            StartDt = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy-MM-01"));
            EndDt = System.DateTime.Now;

            //사업장
            //구 분
            //SYSTEM_CODE_VO();
            Refresh();
        }

        [Command]
       public async void Refresh()
        {
            
            try
            {
                //DXSplashScreen.Show<ProgressWindow>();
                //SearchDetail = null;
                //SelectDtlList = null;


                SystemCodeVo _param = new SystemCodeVo();
                //사업장
                // _param.AREA_CD = _AreaMap[TXT_SL_AREA_NM];
                //_param.AREA_CD = M_SL_AREA_NM.CLSS_CD;
                //_param.AREA_NM = M_SL_AREA_NM.CLSS_DESC;
                ////구분
                //_param.ITM_GRP_CLSS_CD = M_ITM_GRP_NM.CLSS_CD;
                //_param.ITM_GRP_CLSS_NM = M_ITM_GRP_NM.CLSS_DESC;
                //채널
                _param.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;

                _param.FM_DT = (StartDt).ToString("yyyy-MM-dd");
                _param.TO_DT = (EndDt).ToString("yyyy-MM-dd");
                //if (TXT_ITM_GRP_NM.Equals("전체"))
                //{
                //    _param.N1ST_ITM_GRP_CD = null;
                //}
                //else
                //{
                //    _param.N1ST_ITM_GRP_CD = _N1stItmGrpMap[TXT_N1ST_ITM_GRP_NM];
                //}

                //_param.N1ST_ITM_GRP_CD
                //_param.ITM_GRP_CLSS_CD = (string.IsNullOrEmpty(TXT_ITM_GRP_NM) ? null : _GrpClssMap[TXT_ITM_GRP_NM]);

                //_param.ITM_GRP_CLSS_NM = M_SL_TRSP_NM;

                //
                //SelectMstList = codeClient.S1144SelectMstList(_param);

                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s11315", new StringContent(JsonConvert.SerializeObject(_param), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectMstList = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                    }
                    //SelectedMenuViewList = codeClient.SelectItemList(new SystemCodeVo() { ITM_GRP_CLSS_CD = this._AreaMap[M_SL_AREA_NM], CHNL_CD = SystemProperties.USER_VO.CHNL_CD });
                    //SearchMenuContact();
                    ////
                    Title = "[기간]" + _param.FM_DT + " ~ " + _param.TO_DT;

                    if (SelectMstList.Count >= 1)
                    {
                        isM_UPDATE = true;
                        isM_DELETE = true;

                        //if(string.IsNullOrEmpty(_ITM_CD))
                        //{
                            SelectedMstItem = SelectMstList[0];
                        //}
                        //else
                        //{
                            //SelectedMstItem = SelectMstList.Where(x => x.ITM_CD.Equals(_ITM_CD)).LastOrDefault<SystemCodeVo>();
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

        private string _M_DEPT_DESC;
        public string M_DEPT_DESC
        {
            get { return _M_DEPT_DESC; }
            set { SetProperty(ref _M_DEPT_DESC, value, () => M_DEPT_DESC); }
        }


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


        public IList<SystemCodeVo> SelectMstList
        {
            get { return selectedMstList; }
            set { SetProperty(ref selectedMstList, value, () => SelectMstList); }
        }

        SystemCodeVo _selectedMstItem;
        public SystemCodeVo SelectedMstItem
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

        ////
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

        ////
        ////사업장
        ////private Dictionary<string, string> _AreaMap = new Dictionary<string, string>();
        //private IList<SystemCodeVo> _AreaCd = new List<SystemCodeVo>();
        //public IList<SystemCodeVo> AreaList
        //{
        //    get { return _AreaCd; }
        //    set { SetProperty(ref _AreaCd, value, () => AreaList); }
        //}
        //////사업장
        ////private CodeDao _M_SL_AREA_NM;
        ////public CodeDao M_SL_AREA_NM
        ////{
        ////    get { return _M_SL_AREA_NM; }
        ////    set { SetProperty(ref _M_SL_AREA_NM, value, () => M_SL_AREA_NM); }
        ////}
        ////사업장 
        //private SystemCodeVo _M_SL_AREA_NM;
        //public SystemCodeVo M_SL_AREA_NM
        //{
        //    get { return _M_SL_AREA_NM; }
        //    set { SetProperty(ref _M_SL_AREA_NM, value, () => M_SL_AREA_NM); }
        //}

        ////
        ////
        ////구분
        ////private Dictionary<string, string> _GrpClssMap = new Dictionary<string, string>();
        //private IList<SystemCodeVo> _GrpClss = new List<SystemCodeVo>();
        //public IList<SystemCodeVo> GrpClssList
        //{
        //    get { return _GrpClss; }
        //    set { SetProperty(ref _GrpClss, value, () => GrpClssList); }
        //} 
        //////구분
        ////private CodeDao _M_ITM_GRP_CLSS_CD;
        ////public CodeDao M_ITM_GRP_CLSS_CD
        ////{
        ////    get { return _M_ITM_GRP_CLSS_CD; }
        ////    set { SetProperty(ref _M_ITM_GRP_CLSS_CD, value, () => M_ITM_GRP_CLSS_CD); }
        ////}
        ////구분
        //private SystemCodeVo _M_ITM_GRP_NM;
        //public SystemCodeVo M_ITM_GRP_NM
        //{
        //    get { return _M_ITM_GRP_NM; }
        //    set { SetProperty(ref _M_ITM_GRP_NM, value, () => M_ITM_GRP_NM/*, SelectGrpDetail*/); }
        //}

        ////대그룹
        //private Dictionary<string, string> _N1stItmGrpMap = new Dictionary<string, string>();
        //private IList<CodeDao> _N1stItmGrpList = new List<CodeDao>();
        //public IList<CodeDao> N1stItmGrpList
        //{
        //    get { return _N1stItmGrpList; }
        //    set { SetProperty(ref _N1stItmGrpList, value, () => N1stItmGrpList); }
        //}
       
        //private string _TXT_N1ST_ITM_GRP_NM = string.Empty;
        //public string TXT_N1ST_ITM_GRP_NM
        //{
        //    get { return _TXT_N1ST_ITM_GRP_NM; }
        //    set { SetProperty(ref _TXT_N1ST_ITM_GRP_NM, value, () => TXT_N1ST_ITM_GRP_NM); }
        //}
        //private void SelectGrpDetail()
        //{
        //    try
        //    {
        //        if (TXT_ITM_GRP_NM.Equals("전체"))
        //        {
        //            N1stItmGrpList.Insert(0, new CodeDao() { CLSS_CD = "", CLSS_DESC = "" });
        //            TXT_N1ST_ITM_GRP_NM = N1stItmGrpList[0].CLSS_DESC;
        //            return;
        //        }
        //        _N1stItmGrpMap = SystemProperties.ITM_N1ST_CODE_MAP(_GrpClssMap[TXT_ITM_GRP_NM]);
        //        N1stItmGrpList = SystemProperties.ITM_N1ST_CODE_VO(_GrpClssMap[TXT_ITM_GRP_NM]);
        //        if (N1stItmGrpList.Count > 0)
        //        {
        //            TXT_N1ST_ITM_GRP_NM = N1stItmGrpList[0].CLSS_DESC;
        //            //M_N1ST_ITM_GRP_CD = N1stItmGrpList[0];
        //        }
        //    }
        //    catch (Exception eLog)
        //    {
        //        WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
        //        return;
        //    }
        //}

     


        private async void SelectMstDetail()
        {
            try
            {
                ////DXSplashScreen.Show<ProgressWindow>();

                //if (this._selectedMstItem == null)
                //{
                //    return;
                //}

                //using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s1144/dtl", new StringContent(JsonConvert.SerializeObject(SelectedMstItem), System.Text.Encoding.UTF8, "application/json")))
                //{
                //    if (response.IsSuccessStatusCode)
                //    {
                //        this.SelectDtlList = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                //    }

                //    Title = "[사업장]" + M_SL_AREA_NM.CLSS_DESC + ",  [구분]" + M_ITM_GRP_NM.CLSS_DESC + ",  [품번]" + SelectedMstItem.ITM_CD + ",  [품명]" + SelectedMstItem.ITM_NM + ",  [규격]" + SelectedMstItem.ITM_SZ_NM;

                //    //SelectedMstItem.AREA_CD = (TXT_SL_AREA_NM.Equals("전체") ? null : _AreaMap[TXT_SL_AREA_NM]);
                //    //SelectDtlList = new ObservableCollection<PurVo>(purClient.P4401SelectDtlList(new PurVo() { ITM_CD = this._selectedMstItem.}));
                //    //SelectDtlList = codeClient.S1144SelectDtlList(SelectedMstItem);
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
            }
            catch (System.Exception eLog)
            {
                //DXSplashScreen.Close();
                //
                //WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                //return;
            }
        }
        //#endregion


        //#region Functon <Detail List>
        //public IList<SystemCodeVo> SelectDtlList
        //{
        //    get { return selectedDtlList; }
        //    set { SetProperty(ref selectedDtlList, value, () => SelectDtlList); }
        //}

        //SystemCodeVo _searchDetail;
        //public SystemCodeVo SearchDetail
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



        ////#region Functon Command <add, Edit, Del>
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
        //    try
        //    {
        //        if (SelectedMstItem == null)
        //        {
        //            return;
        //        }

        //        string itmCd = SelectedMstItem.ITM_CD;
        //        masterDialog = new S1144DetailDialog(SelectedMstItem);
        //        masterDialog.Title = title + " -[사업장]" + SystemProperties.USER_VO.EMPE_PLC_CD + ", [품번]" + SelectedMstItem.ITM_CD + ", [품명]" + SelectedMstItem.ITM_NM + ", [규격]" + SelectedMstItem.ITM_SZ_NM;
        //        masterDialog.Owner = Application.Current.MainWindow;
        //        masterDialog.BorderEffect = BorderEffect.Default;
        //        masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
        //        masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
        //        bool isDialog = (bool)masterDialog.ShowDialog();
        //        if (isDialog)
        //        {
        //            Refresh(itmCd);

        //            //if (masterDialog.IsEdit == false)
        //            //{

        //            //    DXSplashScreen.Show<ProgressWindow>();
        //            //    Refresh();
        //            //    DXSplashScreen.Close();

        //            //    for (int x = 0; x < SelectMstList.Count; x++)
        //            //    {
        //            //        if (itmCd.Equals(SelectMstList[x].ITM_CD))
        //            //        {
        //            //            SelectedMstItem = SelectMstList[x];
        //            //            return;
        //            //        }
        //            //    }
        //            //}
        //        }
        //    }
        //    catch (Exception eLog)//System.Exception)
        //    {
        //       // DXSplashScreen.Close();
        //        WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
        //        return;
        //    }
        //}

        ////public ICommand HistoryDialogCommand
        ////{
        ////    get
        ////    {
        ////        if (_historyDialogCommand == null)
        ////            _historyDialogCommand = new DelegateCommand(HistoryContact);
        ////        return _historyDialogCommand;
        ////    }
        ////}
        //[Command]
        //public void HistoryContact()
        //{
        //    try
        //    {
        //        if (SelectedMstItem == null)
        //        {
        //            return;
        //        }

        //        historyDialog = new S1144DetailHistoryDialog(SelectedMstItem);
        //        historyDialog.Title = "이력 관리" + " -[사업장]" + SystemProperties.USER_VO.EMPE_PLC_CD + ", [품번]" + SelectedMstItem.ITM_CD + ", [품명]" + SelectedMstItem.ITM_NM + ", [규격]" + SelectedMstItem.ITM_SZ_NM;
        //        historyDialog.Owner = Application.Current.MainWindow;
        //        historyDialog.BorderEffect = BorderEffect.Default;
        //        historyDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
        //        historyDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
        //        bool isDialog = (bool)historyDialog.ShowDialog();
        //        if (isDialog)
        //        {
        //            //if (masterDialog.IsEdit == false)
        //            //{

        //            //    DXSplashScreen.Show<ProgressWindow>();
        //            //    Refresh();
        //            //    DXSplashScreen.Close();

        //            //    for (int x = 0; x < SelectMstList.Count; x++)
        //            //    {
        //            //        if (itmCd.Equals(SelectMstList[x].ITM_CD))
        //            //        {
        //            //            SelectedMstItem = SelectMstList[x];
        //            //            return;
        //            //        }
        //            //    }
        //            //}
        //        }
        //    }
        //     catch (System.Exception eLog)
        //    {
        //        WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
        //        return;
        //    }
        //}

        public async void SYSTEM_CODE_VO()
        {
            //using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "L-002"))
            //{
            //    if (response.IsSuccessStatusCode)
            //    {
            //        AreaList = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
            //        if (AreaList.Count > 0)
            //        {
            //            M_SL_AREA_NM = AreaList[0];
            //        }

            //        //비동기 
            //        //Refresh();
            //    }
            //}

            //using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "L-001"))
            //{
            //    if (response.IsSuccessStatusCode)
            //    {
            //        GrpClssList = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
            //        if (GrpClssList.Count > 0)
            //        {
            //            M_ITM_GRP_NM = GrpClssList[0];
            //        }

            //        //비동기 
            //        Refresh();
            //    }
            //}

        }

        //public ICommand EditDtlDialogCommand
        //{
        //    get
        //    {
        //        if (_editDetailDialogCommand == null)
        //            _editDetailDialogCommand = new DelegateCommand(EditDtlContact);
        //        return _editDetailDialogCommand;
        //    }
        //}

        //public void EditDtlContact()
        //{
        //     try
        //    {
        //        if (SelectedMstItem == null)
        //        {
        //            return;
        //        }
        //        else if (SearchDetail == null)
        //        {
        //            return;
        //        }

        //        masterDialog = new S1144DetailDialog(new SystemCodeVo() { AREA_CD = M_SL_AREA_NM.CLSS_CD, ITM_CD = SelectedMstItem.ITM_CD });
        //        masterDialog.Title = title + " - 수정([품번]" + SelectedMstItem.ITM_CD + ",  [품명]" + SelectedMstItem.ITM_NM + ")";
        //        masterDialog.Owner = Application.Current.MainWindow;
        //        masterDialog.BorderEffect = BorderEffect.Default;
        //        //masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
        //        //masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
        //        bool isDialog = (bool)masterDialog.ShowDialog();
        //        if (isDialog)
        //        {
        //            //if (masterDialog.IsEdit == false)
        //            {

        //                DXSplashScreen.Show<ProgressWindow>();
        //                Refresh();
        //                DXSplashScreen.Close();

        //            }
        //        }

        //    }
        //    catch (System.Exception)
        //    {
        //         DXSplashScreen.Close();
        //    //     WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
        //         return;
        //    }
        //}

    }
}
