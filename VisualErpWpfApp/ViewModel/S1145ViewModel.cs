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
using AquilaErpWpfApp3.S.View.Dialog;
using AquilaErpWpfApp3.Util;

namespace AquilaErpWpfApp3.ViewModel
{
    public sealed class S1145ViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private string title = "제품 단가 등록";

        //private static CodeServiceClient codeClient = SystemProperties.CodeClient;
        private IList<SystemCodeVo> selectedMstList = new List<SystemCodeVo>();

        //private ObservableCollection<PurVo> selectedDtlList = new ObservableCollection<PurVo>();
        private IList<SystemCodeVo> selectedDtlList = new List<SystemCodeVo>();

        private S1145MasterDialog_2 masterDialog_2;
        private S1145MasterDialog_1 masterDialog_1;
        private S1145DetailDialog detailDialog;


        //private S1145MasterCopyDialog copyDialog;
        //private S1145MasterDelDialog delDialog;



        ////Menu Dialog
        //private ICommand _searchDialogCommand;
        //private ICommand _newDialogCommand;
        //private ICommand _editDialogCommand;

        //private ICommand _editDetailDialogCommand;
        //private ICommand _editDetailDialogCommand2;
        //private ICommand _delDialogCommand;



        //private ICommand _copyDialogCommand;
        //private ICommand _mDelDialogCommand;


        public S1145ViewModel() 
        {
            //StartDt = System.DateTime.Now;
            EndDt = System.DateTime.Now;

            //사업장
            //구 분
            SYSTEM_CODE_VO();
            // - Refresh();
        }

        [Command]
        public async void Refresh(string _ITM_CD = null)
        {
            try
            {
                //DXSplashScreen.Show<ProgressWindow>();
                //SearchDetail = null;
                //SelectDtlList = null;


                SystemCodeVo _param = new SystemCodeVo();
                ////사업장
                ////_param.AREA_CD = _AreaMap[M_SL_AREA_NM];
                ////_param.AREA_NM = M_SL_AREA_NM;
                //if (TXT_SL_AREA_NM == null)
                //{
                //    WinUIMessageBox.Show("[사업장] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                //    return;
                //}
                //else
                //{
                _param.AREA_CD = M_SL_AREA_VO.CLSS_CD;
                _param.AREA_NM = M_SL_AREA_VO.CLSS_DESC;
                //}

                ////구분
                ////_param.ITM_GRP_CLSS_CD = (M_ITM_GRP_CLSS_CD.Equals("전체") ? null : _GrpClssMap[M_ITM_GRP_CLSS_CD]);
                ////_param.ITM_GRP_CLSS_NM = M_SL_TRSP_NM;
                //if (TXT_ITM_GRP_CLSS_CD == null)
                //{
                //    WinUIMessageBox.Show("[구분] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                //    return;
                //}
                //else
                //{
                //    _param.ITM_GRP_CLSS_CD = _GrpClssMap[TXT_ITM_GRP_CLSS_CD];
                //    _param.ITM_GRP_CLSS_NM = TXT_ITM_GRP_CLSS_CD;
                //}

                ////대분류
                //if (TXT_N1ST_ITM_GRP_CD == null)
                //{
                //    WinUIMessageBox.Show("[대분류] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                //    return;
                //}
                //else
                //{
                //    _param.N1ST_ITM_GRP_CD = _N1stItmGrpMap[TXT_N1ST_ITM_GRP_CD];
                //    _param.N1ST_ITM_GRP_NM = TXT_N1ST_ITM_GRP_CD;
                //}

                ////기준 일자
                //if (M_ST_APLY_DT == null)
                //{
                //    //WinUIMessageBox.Show("[기준 일자] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                //    SelectAplyDtList();
                //}

                //if (M_ST_APLY_DT == null)
                //{
                //    WinUIMessageBox.Show("[기준 일자] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                //    return;
                //}
                //else
                //{
                //    _param.FM_DT = Convert.ToDateTime(M_ST_APLY_DT.CLSS_DESC).ToString("yyyy-MM-dd");
                //    _param.TO_DT = Convert.ToDateTime(M_ST_APLY_DT.CLSS_DESC).ToString("yyyy-MM-dd");
                //}

                _param.FM_DT = (EndDt).ToString("yyyy-MM-dd");
                _param.TO_DT = (EndDt).ToString("yyyy-MM-dd");
                _param.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;

                //
                //SelectMstList = codeClient.S1145SelectMstList(_param);


                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s1145", new StringContent(JsonConvert.SerializeObject(_param), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectMstList = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                    }


                    ////
                    //Title = "[기간]" + Convert.ToDateTime(M_ST_APLY_DT.CLSS_DESC).ToString("yyyy-MM-dd") + ",  [사업장]" + TXT_SL_AREA_NM + ",  [구분]" + TXT_ITM_GRP_CLSS_CD + ",  [대분류]" + TXT_N1ST_ITM_GRP_CD + (string.IsNullOrEmpty(M_SEARCH_TEXT) ? "" : (",  [검색]" + M_SEARCH_TEXT)); 
                    Title = "[기간]" + EndDt.ToString("yyyy-MM-dd") + ",  [사업장]" + M_SL_AREA_VO.CLSS_DESC + ",  [구분]" + M_ITM_GRP_CLSS_VO.CLSS_DESC;

                    if (SelectMstList.Count >= 1)
                    {
                        isM_UPDATE = true;
                        isM_DELETE = true;

                        SelectedMstItem = SelectMstList[0];
                    }
                    else
                    {
                        SelectDtlList = null;
                        SearchDetail = null;

                        isM_UPDATE = false;
                        isM_DELETE = false;
                        //
                        isD_UPDATE = false;
                        isD_DELETE = false;
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


        ////기준 일자
        //private ObservableCollection<CodeDao> _AplyDtList = new ObservableCollection<CodeDao>();
        //public ObservableCollection<CodeDao> AplyDtList
        //{
        //    get { return _AplyDtList; }
        //    set { SetProperty(ref _AplyDtList, value, () => AplyDtList); }
        //}
        ////기준 일자
        //private CodeDao _M_ST_APLY_DT;
        //public CodeDao M_ST_APLY_DT
        //{
        //    get { return _M_ST_APLY_DT; }
        //    set { SetProperty(ref _M_ST_APLY_DT, value, () => M_ST_APLY_DT); }
        //}
        ////기준 일자 
        //private string _TXT_ST_APLY_DT = string.Empty;
        //public string TXT_ST_APLY_DT
        //{
        //    get { return _TXT_ST_APLY_DT; }
        //    set { SetProperty(ref _TXT_ST_APLY_DT, value, () => TXT_ST_APLY_DT); }
        //}

        //#region Functon <Master List>
        //DateTime _startDt;
        //public DateTime StartDt
        //{
        //    get { return _startDt; }
        //    set { SetProperty(ref _startDt, value, () => StartDt); }
        //}

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

        ////
        ////
        ////사업장
        //private Dictionary<string, string> _AreaMap = new Dictionary<string, string>();
        private IList<SystemCodeVo> _AreaCd = new List<SystemCodeVo>();
        public IList<SystemCodeVo> SL_AREA_LIST
        {
            get { return _AreaCd; }
            set { SetProperty(ref _AreaCd, value, () => SL_AREA_LIST); }
        }
        //사업장
        private SystemCodeVo _M_SL_AREA_NM;
        public SystemCodeVo M_SL_AREA_VO
        {
            get { return _M_SL_AREA_NM; }
            set { SetProperty(ref _M_SL_AREA_NM, value, () => M_SL_AREA_VO); }
        }
        //사업장 
        //private string _TXT_SL_AREA_NM = string.Empty;
        //public string TXT_SL_AREA_NM
        //{
        //    get { return _TXT_SL_AREA_NM; }
        //    set { SetProperty(ref _TXT_SL_AREA_NM, value, () => TXT_SL_AREA_NM); }
        //}
        //private void SelectAplyDtList()
        //{
        //    try
        //    {
        //        if (TXT_SL_AREA_NM == null)
        //        {
        //            return;
        //        }

        //        ObservableCollection<CodeDao> _AplyDtList = new ObservableCollection<CodeDao>();
        //        IList<SystemCodeVo> tmpList = codeClient.S1145SelectAplyDtList(new SystemCodeVo() { AREA_CD = _AreaMap[TXT_SL_AREA_NM] });
        //        for (int x = 0; x < tmpList.Count; x++)
        //        {
        //            _AplyDtList.Add(new CodeDao() { CLSS_CD = tmpList[x].RN + "", CLSS_DESC = tmpList[x].FM_DT });
        //        }
        //        if (_AplyDtList.Count > 0)
        //        {
        //            TXT_ST_APLY_DT = _AplyDtList[0].CLSS_DESC;
        //            M_ST_APLY_DT = _AplyDtList[0];
        //        }
        //        AplyDtList = _AplyDtList;
        //    }
        //    catch (Exception)
        //    {
        //        //WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
        //        return;
        //    }
        //}

        //
        //
        //구분
        //private Dictionary<string, string> _GrpClssMap = new Dictionary<string, string>();
        private IList<SystemCodeVo> _GrpClss = new List<SystemCodeVo>();
        public IList<SystemCodeVo> ITM_GRP_CLSS_LIST
        {
            get { return _GrpClss; }
            set { SetProperty(ref _GrpClss, value, () => ITM_GRP_CLSS_LIST); }
        }
        //구분
        private SystemCodeVo _M_ITM_GRP_CLSS_CD;
        public SystemCodeVo M_ITM_GRP_CLSS_VO
        {
            get { return _M_ITM_GRP_CLSS_CD; }
            set
            {
                if (value != null)
                {
                    SetProperty(ref _M_ITM_GRP_CLSS_CD, value, () => M_ITM_GRP_CLSS_VO);
                }
            }
        }
        //구분
        //private string _TXT_ITM_GRP_CLSS_CD = string.Empty;
        //public string TXT_ITM_GRP_CLSS_CD
        //{
        //    get { return _TXT_ITM_GRP_CLSS_CD; }
        //    set { SetProperty(ref _TXT_ITM_GRP_CLSS_CD, value, () => TXT_ITM_GRP_CLSS_CD, SelectGrpDetail); }
        //}
        //private void SelectGrpDetail()
        //{
        //    try
        //    {
        //        _N1stItmGrpMap = SystemProperties.ITM_N1ST_CODE_MAP(_GrpClssMap[TXT_ITM_GRP_CLSS_CD]);
        //        N1stItmGrpList = SystemProperties.ITM_N1ST_CODE_VO(_GrpClssMap[TXT_ITM_GRP_CLSS_CD]);
        //        if (N1stItmGrpList.Count > 0)
        //        {
        //            TXT_N1ST_ITM_GRP_CD = N1stItmGrpList[0].CLSS_DESC;
        //            //M_N1ST_ITM_GRP_CD = N1stItmGrpList[0];
        //        }
        //    }
        //    catch (Exception eLog)
        //    {
        //        WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
        //        return;
        //    }
        //}



        //
        ////
        ////대분류
        //private Dictionary<string, string> _N1stItmGrpMap = new Dictionary<string, string>();
        //private IList<CodeDao> _N1stItmGrpList = new List<CodeDao>();
        //public IList<CodeDao> N1stItmGrpList
        //{
        //    get { return _N1stItmGrpList; }
        //    set { SetProperty(ref _N1stItmGrpList, value, () => N1stItmGrpList); }
        //}
        //////대분류
        ////private CodeDao _M_N1ST_ITM_GRP_CD;
        ////public CodeDao M_N1ST_ITM_GRP_CD
        ////{
        ////    get { return _M_N1ST_ITM_GRP_CD; }
        ////    set { SetProperty(ref _M_N1ST_ITM_GRP_CD, value, () => M_N1ST_ITM_GRP_CD); }
        ////}
        ////대분류
        //private string _TXT_N1ST_ITM_GRP_CD = string.Empty;
        //public string TXT_N1ST_ITM_GRP_CD
        //{
        //    get { return _TXT_N1ST_ITM_GRP_CD; }
        //    set { SetProperty(ref _TXT_N1ST_ITM_GRP_CD, value, () => TXT_N1ST_ITM_GRP_CD); }
        //}







        private void SelectMstDetail()
        {
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
        public IList<SystemCodeVo> SelectDtlList
        {
            get { return selectedDtlList; }
            set { SetProperty(ref selectedDtlList, value, () => SelectDtlList); }
        }

        SystemCodeVo _searchDetail;
        public SystemCodeVo SearchDetail
        {
            get
            {
                return _searchDetail;
            }
            set
            {
                if (value != null)
                {
                    SetProperty(ref _searchDetail, value, () => SearchDetail);
                }
            }
        }
        //#endregion


        string _Title = string.Empty;
        public string Title
        {
            get { return _Title; }
            set { SetProperty(ref _Title, value, () => Title); }
        }



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

        [Command]
        public void NewContact()
        {
            //if (SelectedMstItem == null)
            //{
            //    return;
            //}

            //    //string itmCd = SelectedMstItem.ITM_CD;
            masterDialog_2 = new S1145MasterDialog_2(new SystemCodeVo() { AREA_CD = M_SL_AREA_VO.CLSS_CD, AREA_NM = M_SL_AREA_VO.CLSS_DESC, ITM_GRP_CLSS_NM = M_ITM_GRP_CLSS_VO.CLSS_DESC, ITM_GRP_CLSS_CD = M_ITM_GRP_CLSS_VO.CLSS_CD });
            masterDialog_2.Title = title + " - 추가 (사업장)";
            masterDialog_2.Owner = Application.Current.MainWindow;
            masterDialog_2.BorderEffect = BorderEffect.Default;
            masterDialog_2.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            masterDialog_2.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            bool isDialog = (bool)masterDialog_2.ShowDialog();
            if (isDialog)
            {
                Refresh();
                //        //if (masterDialog.IsEdit == false)
                //        {
                //            try
                //            {
                //                DXSplashScreen.Show<ProgressWindow>();
                //                Refresh();
                //                DXSplashScreen.Close();

                //                //for (int x = 0; x < SelectMstList.Count; x++)
                //                //{
                //                //    if (masterDialog.ResultDao.ITM_CD.Equals(SelectMstList[x].ITM_CD))
                //                //    {
                //                //        SelectedMstItem = SelectMstList[x];
                //                //        return;
                //                //    }
                //                //}

                //            }
                //            catch (System.Exception eLog)
                //            {
                //                DXSplashScreen.Close();
                //                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                //                return;
                //            }
                //        }
            }
        }

        //public ICommand EditDialogCommand
        //{
        //    get
        //    {
        //        if (_editDialogCommand == null)
        //            _editDialogCommand = new DelegateCommand(EditContact);
        //        return _editDialogCommand;
        //    }
        //}

        [Command]
        public void EditContact()
        {
            masterDialog_1 = new S1145MasterDialog_1(new SystemCodeVo() { AREA_CD = M_SL_AREA_VO.CLSS_CD, AREA_NM = M_SL_AREA_VO.CLSS_DESC, ITM_GRP_CLSS_NM = M_ITM_GRP_CLSS_VO.CLSS_DESC, ITM_GRP_CLSS_CD = M_ITM_GRP_CLSS_VO.CLSS_CD });
            masterDialog_1.Title = title + " - 추가 (공통)";
            masterDialog_1.Owner = Application.Current.MainWindow;
            masterDialog_1.BorderEffect = BorderEffect.Default;
            masterDialog_1.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            masterDialog_1.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            bool isDialog = (bool)masterDialog_1.ShowDialog();
            if (isDialog)
            {
                Refresh();
                //Refresh(masterDialog_1.ResultDao.ITM_CD);

                ////if (masterDialog.IsEdit == false)
                //{
                //    try
                //    {
                //        DXSplashScreen.Show<ProgressWindow>();
                //        Refresh();
                //        DXSplashScreen.Close();

                //    }
                //    catch (System.Exception eLog)
                //    {
                //        DXSplashScreen.Close();
                //        WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                //        return;
                //    }
                //}
            }
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
        //    if (SelectedMstItem == null)
        //    {
        //        return;
        //    }


        //    I5511DialogExcel dialogExcel = new I5511DialogExcel(SelectedMstItem, true);
        //    dialogExcel.Title = title + "-엑셀업로드(사업장)";

        //    dialogExcel.Owner = Application.Current.MainWindow;

        //    bool isDialog = (bool)dialogExcel.ShowDialog();
        //    if (isDialog)
        //    {
        //        try
        //        {
        //            //DXSplashScreen.Show<ProgressWindow>();
        //            //Refresh();
        //            //DXSplashScreen.Close();
        //        }
        //        catch (System.Exception eLog)
        //        {
        //            DXSplashScreen.Close();
        //            WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
        //            return;
        //        }
        //    }

        //    //masterDialog = new S1145MasterDialog(SelectedMstItem);
        //    ////([품번]" + SelectedMstItem.ITM_CD + ",  [품명]" + SelectedMstItem.ITM_NM + ")"
        //    //masterDialog.Title = title + " - 수정";
        //    //masterDialog.Owner = Application.Current.MainWindow;
        //    //masterDialog.BorderEffect = BorderEffect.Default;
        //    ////masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
        //    ////masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
        //    //bool isDialog = (bool)masterDialog.ShowDialog();
        //    ////if (isDialog)
        //    //{
        //    //    //if (masterDialog.IsEdit == false)
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

        //public ICommand EditDtlDialog2Command
        //{
        //    get
        //    {
        //        if (_editDetailDialogCommand2 == null)
        //            _editDetailDialogCommand2 = new DelegateCommand(EditDtlContact2);
        //        return _editDetailDialogCommand2;
        //    }
        //}

        //public void EditDtlContact2()
        //{
        //    if (SelectedMstItem == null)
        //    {
        //        return;
        //    }


        //    I5511DialogExcel dialogExcel = new I5511DialogExcel(SelectedMstItem, false);
        //    dialogExcel.Title = title + "-엑셀업로드(공통)";

        //    dialogExcel.Owner = Application.Current.MainWindow;

        //    bool isDialog = (bool)dialogExcel.ShowDialog();
        //    if (isDialog)
        //    {
        //        try
        //        {
        //            //DXSplashScreen.Show<ProgressWindow>();
        //            //Refresh();
        //            //DXSplashScreen.Close();
        //        }
        //        catch (System.Exception eLog)
        //        {
        //            DXSplashScreen.Close();
        //            WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
        //            return;
        //        }
        //    }

        //    //masterDialog = new S1145MasterDialog(SelectedMstItem);
        //    ////([품번]" + SelectedMstItem.ITM_CD + ",  [품명]" + SelectedMstItem.ITM_NM + ")"
        //    //masterDialog.Title = title + " - 수정";
        //    //masterDialog.Owner = Application.Current.MainWindow;
        //    //masterDialog.BorderEffect = BorderEffect.Default;
        //    ////masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
        //    ////masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
        //    //bool isDialog = (bool)masterDialog.ShowDialog();
        //    ////if (isDialog)
        //    //{
        //    //    //if (masterDialog.IsEdit == false)
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


        //public ICommand DelDialogCommand
        //{
        //    get
        //    {
        //        if (_delDialogCommand == null)
        //            _delDialogCommand = new DelegateCommand(DelContact);
        //        return _delDialogCommand;
        //    }
        //}

        [Command]
        public void DelContact()
        {
            if (SelectedMstItem != null)
            {
                //        //MessageBoxResult result = WinUIMessageBox.Show("[" + SelectedMstItem.ST_APLY_DT + " ~ " + SelectedMstItem.END_APLY_DT + "] " + SelectedMstItem.ITM_CD + " / " + SelectedMstItem.ITM_NM + " 정말로 삭제 하시겠습니까?", "[삭제]" + title, MessageBoxButton.YesNo, MessageBoxImage.Question);
                //        //if (result == MessageBoxResult.Yes)
                //        //{
                //        //    codeClient.S1145DeleteMst(SelectedMstItem);
                //        //    WinUIMessageBox.Show("삭제가 완료되었습니다.", "[삭제]" + title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                //        //    Refresh();
                //        //}


                detailDialog = new S1145DetailDialog(SelectedMstItem);
                detailDialog.Title = title + " - 단가 이력([품번]" + SelectedMstItem.ITM_CD + ",  [품명]" + SelectedMstItem.ITM_NM + ")";
                detailDialog.Owner = Application.Current.MainWindow;
                detailDialog.BorderEffect = BorderEffect.Default;
                detailDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
                detailDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
                bool isDialog = (bool)detailDialog.ShowDialog();


            }
        }




        //// 단가 복사
        //public ICommand CopyDialogCommand
        //{
        //    get
        //    {
        //        if (_copyDialogCommand == null)
        //            _copyDialogCommand = new DelegateCommand(CopyContact);
        //        return _copyDialogCommand;
        //    }
        //}

        //public void CopyContact()
        //{
        //      try
        //      {
        //            if (SelectedMstItem == null)
        //            {
        //                return;
        //            }

        //            //string itmCd = SelectedMstItem.ITM_CD;
        //            copyDialog = new S1145MasterCopyDialog(new SystemCodeVo() { FM_DT = SelectedMstItem.ST_APLY_DT, AREA_CD = _AreaMap[TXT_SL_AREA_NM], AREA_NM = TXT_SL_AREA_NM });
        //            copyDialog.Title = title + " - 단가 복사";
        //            copyDialog.Owner = Application.Current.MainWindow;
        //            copyDialog.BorderEffect = BorderEffect.Default;
        //            //masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
        //            //masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
        //            bool isDialog = (bool)copyDialog.ShowDialog();
        //            //if (isDialog)
        //            {
        //                //if (masterDialog.IsEdit == false)
        //                {

        //                        DXSplashScreen.Show<ProgressWindow>();
        //                        Refresh();
        //                        SelectAplyDtList();
        //                        DXSplashScreen.Close();

        //                        //for (int x = 0; x < SelectMstList.Count; x++)
        //                        //{
        //                        //    if (masterDialog.ResultDao.ITM_CD.Equals(SelectMstList[x].ITM_CD))
        //                        //    {
        //                        //        SelectedMstItem = SelectMstList[x];
        //                        //        return;
        //                        //    }
        //                        //}
        //                }
        //            }
        //      }
        //      catch (System.Exception eLog)
        //      {
        //          //DXSplashScreen.Close();
        //          WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
        //          return;
        //      }
        //}



        //// 단가 삭제
        //public ICommand MdelDialogCommand
        //{
        //    get
        //    {
        //        if (_mDelDialogCommand == null)
        //            _mDelDialogCommand = new DelegateCommand(M_DelContact);
        //        return _mDelDialogCommand;
        //    }
        //}

        //public void M_DelContact()
        //{
        //    try
        //    {
        //        //if (SelectedMstItem == null)
        //        //{
        //        //    return;
        //        //}

        //        //string itmCd = SelectedMstItem.ITM_CD;
        //        delDialog = new S1145MasterDelDialog(new SystemCodeVo() { FM_DT = SelectedMstItem.ST_APLY_DT, AREA_CD = _AreaMap[TXT_SL_AREA_NM], AREA_NM = TXT_SL_AREA_NM });
        //        delDialog.Title = title + " - 단가 삭제";
        //        delDialog.Owner = Application.Current.MainWindow;
        //        delDialog.BorderEffect = BorderEffect.Default;
        //        //masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
        //        //masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
        //        bool isDialog = (bool)delDialog.ShowDialog();
        //        //if (isDialog)
        //        {
        //            //if (masterDialog.IsEdit == false)
        //            {
        //                   DXSplashScreen.Show<ProgressWindow>();
        //                    Refresh();
        //                    SelectAplyDtList();
        //                    DXSplashScreen.Close();

        //                    //for (int x = 0; x < SelectMstList.Count; x++)
        //                    //{
        //                    //    if (masterDialog.ResultDao.ITM_CD.Equals(SelectMstList[x].ITM_CD))
        //                    //    {
        //                    //        SelectedMstItem = SelectMstList[x];
        //                    //        return;
        //                    //    }
        //                    //}


        //            }
        //        }
        //    }
        //    catch (System.Exception eLog)
        //    {
        //        //DXSplashScreen.Close();
        //        WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
        //        return;
        //    }
        //}



        public async void SYSTEM_CODE_VO()
        {
            // ITM_GRP_CLSS_LIST = SystemProperties.SYSTEM_CODE_VO("L-001");
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "L-001"))
            {
                if (response.IsSuccessStatusCode)
                {
                    ITM_GRP_CLSS_LIST = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                    if (ITM_GRP_CLSS_LIST.Count > 0)
                    {
                        M_ITM_GRP_CLSS_VO = ITM_GRP_CLSS_LIST[0];
                    }
                }
            }

            //SL_AREA_LIST = SystemProperties.SYSTEM_CODE_VO("L-002");
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "L-002"))
            {
                if (response.IsSuccessStatusCode)
                {
                    SL_AREA_LIST = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                    if (SL_AREA_LIST.Count > 0)
                    {
                        M_SL_AREA_VO = SL_AREA_LIST[0];
                    }
                }
            }

            Refresh();
        }

    }
}
