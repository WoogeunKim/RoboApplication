using AquilaErpWpfApp3.Util;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Code;
using ModelsLibrary.Pur;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Windows;
using AquilaErpWpfApp3.View.PUR.Dialog;
using ModelsLibrary.Man;

namespace AquilaErpWpfApp3.ViewModel
{
    public sealed class P414101ViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private string _title = "BOM등록(구매)";

        //private static PurServiceClient purClient = SystemProperties.PurClient;
        private IList<PurVo> selectedMstList = new List<PurVo>();

        //private ObservableCollection<PurVo> selectedDtlList = new ObservableCollection<PurVo>();
        private IList<PurVo> selectedDtlList = new List<PurVo>();

        private IList<ManVo> selectedRoutList = new List<ManVo>();


        //private IList<SystemCodeVo> customerList = new List<SystemCodeVo>();
        //private IList<SystemCodeVo> selectCustomerList = new List<SystemCodeVo>();

        private P414101MasterDialog masterDialog;
        //private P4411Detail_1Dialog detailDialog_1;
        //private P4411Detail_2Dialog detailDialog_2;

        //private P4411Detail_3Dialog detailDialog_3;

        //private P4411ExcelDialog excelDialog;
        //private P4411DetailEditDialog detailEditDialog;

        ////Menu Dialog
        //private ICommand _searchDialogCommand;
        //private ICommand _newDialogCommand;
        //private ICommand _editDialogCommand;
        //private ICommand _delDialogCommand;

        //private ICommand _printDialogCommand;

        //private ICommand _searchDetailDialogCommand;
        //private ICommand _newDetailDialogCommand;
        //private ICommand _editDetailDialogCommand;
        //private ICommand _delDetailDialogCommand;

        //private ICommand _excelDetailDialogCommand;

        //private ICommand reportDialogCommand;

        ////private ICommand _revListDetailDialogCommand;
        ////private ICommand _revNewDetailDialogCommand;

        //private P41MasterDialog masterDialog;
        //private P41DetailDialog detailDialog;
        ////private A21JobItemRevDialog jobItemRevDialog;

        //private P41ReportDialog reportDialog;

        public P414101ViewModel() 
        {

            //WeekDt = System.DateTime.Now;

            //StartDt = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy-MM-01"));
            //EndDt = System.DateTime.Now;

            //사업장
            SYSTEM_CODE_VO();
            // - Refresh();

            ////사업장
            //AreaList = SystemProperties.SYSTEM_CODE_VO("L-002");
            //_AreaMap = SystemProperties.SYSTEM_CODE_MAP("L-002");
            //if (AreaList.Count > 0)
            //{
            //    TXT_SL_AREA_NM = SystemProperties.USER_VO.EMPE_PLC_CD;
            //    //for (int x = 0; x < AreaList.Count; x++)
            //    //{
            //    //    if (TXT_SL_AREA_NM.Equals(AreaList[x].CLSS_DESC))
            //    //    {
            //    //        M_SL_AREA_NM = AreaList[x];
            //    //        break;
            //    //    }
            //    //}
            //}


            ////매입처
            //_DeptMap = SystemProperties.CUSTOMER_CODE_MAP("AP", SystemProperties.USER_VO.EMPE_PLC_NM);
            //DeptList = SystemProperties.CUSTOMER_CODE_VO("AP", SystemProperties.USER_VO.EMPE_PLC_NM);
            //DeptList.Insert(0, new CustomerCodeDao() { CO_NO = "", CO_NM = "" });
            //if (DeptList.Count > 0)
            //{
            //    //M_DEPT_DESC = DeptList[0];
            //    TXT_DEPT_DESC = DeptList[0].CO_NO;

            //    //TXT_SL_AREA_NM = SystemProperties.USER_VO.EMPE_PLC_CD;
            //    //for (int x = 0; x < DeptList.Count; x++)
            //    //{
            //    //    if (TXT_DEPT_DESC.Equals(DeptList[x].CO_NO))
            //    //    {
            //    //        M_SL_AREA_NM = DeptList[x];
            //    //        break;
            //    //    }
            //    //}
            //}


            //Refresh();
        }

        [Command]
        public async void Refresh()
        {
            try
            {
                DXSplashScreen.Show<ProgressWindow>();

                SearchDetail = null;
                SelectDtlList = null;
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("p414101/mst", new StringContent(JsonConvert.SerializeObject(new PurVo() { AREA_CD = M_SL_AREA_NM.CLSS_CD, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectMstList = JsonConvert.DeserializeObject<IEnumerable<PurVo>>(await response.Content.ReadAsStringAsync()).Cast<PurVo>().ToList();
                    }
                    Title = "[사업장]" + M_SL_AREA_NM.CLSS_DESC;

                    if (SelectMstList.Count >= 1)
                    {
                        isM_UPDATE = true;
                        isM_DELETE = true;

                        SelectedMstItem = SelectMstList[0];
                    }
                    else
                    {
                        isM_UPDATE = false;
                        isM_DELETE = false;
                        //
                        isD_UPDATE = false;
                        isD_DELETE = false;
                    }

                    DXSplashScreen.Close();
                }
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

        ////발주서 - 월
        //DateTime _weekDt;
        //public DateTime WeekDt
        //{
        //    get { return _weekDt; }
        //    set { SetProperty(ref _weekDt, value, () => WeekDt); }
        //}

        //private string _M_SEARCH_TEXT = string.Empty;
        //public string M_SEARCH_TEXT
        //{
        //    get { return _M_SEARCH_TEXT; }
        //    set { SetProperty(ref _M_SEARCH_TEXT, value, () => M_SEARCH_TEXT); }
        //}


        //사업장
        private Dictionary<string, string> _AreaMap = new Dictionary<string, string>();
        private IList<SystemCodeVo> _AreaCd = new List<SystemCodeVo>();
        public IList<SystemCodeVo> AreaList
        {
            get { return _AreaCd; }
            set { SetProperty(ref _AreaCd, value, () => AreaList); }
        }
        ////사업장
        //private CodeDao _M_SL_AREA_NM;
        //public CodeDao M_SL_AREA_NM
        //{
        //    get { return _M_SL_AREA_NM; }
        //    set { SetProperty(ref _M_SL_AREA_NM, value, () => M_SL_AREA_NM, RefreshCoNm); }
        //}
        //사업장 
        private SystemCodeVo _M_SL_AREA_NM;
        public SystemCodeVo M_SL_AREA_NM
        {
            get { return _M_SL_AREA_NM; }
            set { SetProperty(ref _M_SL_AREA_NM, value, () => M_SL_AREA_NM, RefreshCoNm); }
        }
        private void RefreshCoNm()
        {
            //if (TXT_SL_AREA_NM != null)
            //{
            //    _DeptMap = SystemProperties.CUSTOMER_CODE_MAP("AP", _AreaMap[TXT_SL_AREA_NM]);
            //    DeptList = SystemProperties.CUSTOMER_CODE_VO("AP", _AreaMap[TXT_SL_AREA_NM]);
            //    DeptList.Insert(0, new CustomerCodeDao() { CO_NO = "", CO_NM = "" });
            //    if (DeptList.Count > 0)
            //    {
            //        //M_DEPT_DESC = DeptList[0];
            //        //TXT_DEPT_DESC = DeptList[0].CO_NO;
            //    }
            //}
        }

        ////매입처
        //private Dictionary<string, string> _DeptMap = new Dictionary<string, string>();
        //private IList<CustomerCodeDao> _DeptCd = new List<CustomerCodeDao>();
        //public IList<CustomerCodeDao> DeptList
        //{
        //    get { return _DeptCd; }
        //    set { SetProperty(ref _DeptCd, value, () => DeptList); }
        //}

        //private CustomerCodeDao _SelectedItemDept = new CustomerCodeDao();
        //public CustomerCodeDao SelectedItemDept
        //{
        //    get { return _SelectedItemDept; }
        //    set { SetProperty(ref _SelectedItemDept, value, () => SelectedItemDept); }
        //}
        ////매입처
        //private CustomerCodeDao _M_DEPT_DESC;
        //public CustomerCodeDao M_DEPT_DESC
        //{
        //    get { return _M_DEPT_DESC; }
        //    set { SetProperty(ref _M_DEPT_DESC, value, () => M_DEPT_DESC); }
        //}
        //매입처
        //private string _TXT_DEPT_DESC = string.Empty;
        //public string TXT_DEPT_DESC
        //{
        //    get { return _TXT_DEPT_DESC; }
        //    set { SetProperty(ref _TXT_DEPT_DESC, value, () => TXT_DEPT_DESC); }
        //}


        //private bool? _M_SEARCH_CHECKD = false;
        //public bool? M_SEARCH_CHECKD
        //{
        //    get { return _M_SEARCH_CHECKD; }
        //    set { SetProperty(ref _M_SEARCH_CHECKD, value, () => M_SEARCH_CHECKD); }
        //}


        public IList<PurVo> SelectMstList
        {
            get { return selectedMstList; }
            set { SetProperty(ref selectedMstList, value, () => SelectMstList); }
        }

        PurVo _selectedMstItem;
        public PurVo SelectedMstItem
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

        [Command]
        public async void SelectMstDetail()
        {
             try
             {
                 //DXSplashScreen.Show<ProgressWindow>();

                if (this._selectedMstItem == null)
                {
                    return;
                }


                //수정 화면 - (실적공정 호출)
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m66212/dtl", new StringContent(JsonConvert.SerializeObject(SelectedMstItem), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectRoutList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
                    }

                }


                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("p414101/dtl", new StringContent(JsonConvert.SerializeObject(SelectedMstItem), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectDtlList = JsonConvert.DeserializeObject<IEnumerable<PurVo>>(await response.Content.ReadAsStringAsync()).Cast<PurVo>().ToList();
                    }

                    //SelectDtlList = purClient.P4411SelectDtlList(SelectedMstItem);
                    // //
                    if (SelectDtlList.Count >= 1)
                    {
                        isD_UPDATE = true;
                        isD_DELETE = true;

                        SearchDetail = SelectDtlList[0];
                    }
                    else
                    {
                        isD_UPDATE = false;
                        isD_DELETE = false;
                    }
                }

                //DXSplashScreen.Close();
             }
             catch (System.Exception eLog)
             {
                 //DXSplashScreen.Close();
                 //
                 WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                 return;
             }
        }
        //#endregion


        //#region Functon <Detail List>
        public IList<PurVo> SelectDtlList
        {
            get { return selectedDtlList; }
            set { SetProperty(ref selectedDtlList, value, () => SelectDtlList); }
        }

        PurVo _searchDetail;
        public PurVo SearchDetail
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




        //#region Functon <Detail List>
        public IList<ManVo> SelectRoutList
        {
            get { return selectedRoutList; }
            set { SetProperty(ref selectedRoutList, value, () => SelectRoutList); }
        }

        //public IList<SystemCodeVo> CustomerList
        //{
        //    get { return customerList; }
        //    set { SetProperty(ref customerList, value, () => CustomerList); }
        //}


        //SystemCodeVo _searchItemCo;
        //public SystemCodeVo SelectedItemCo
        //{
        //    get
        //    {
        //        return _searchItemCo;
        //    }
        //    set
        //    {
        //        if (value != null)
        //        {
        //            SetProperty(ref _searchItemCo, value, () => SelectedItemCo, SaveCoNm);
        //        }
        //    }
        //}


        //string _TXT_CO_CD;
        //public string TXT_CO_CD
        //{
        //    get { return _TXT_CO_CD; }
        //    set { SetProperty(ref _TXT_CO_CD, value, () => TXT_CO_CD); }
        //}

        //private IList<SystemCodeVo> _weekCoNo = new List<SystemCodeVo>();
        //public IList<SystemCodeVo> WeekCoNo
        //{
        //    get { return _weekCoNo; }
        //    set { SetProperty(ref _weekCoNo, value, () => WeekCoNo); }
        //}


        //void SaveCoNm()
        //{

        //    WinUIMessageBox.Show("삭제가 완료되었습니다.", _title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);

        //    //WeekCoNo.Add(CoNo);



        //}

        // PurVo _searchDetail;
        //public PurVo SearchDetail
        //{
        //    get
        //    {
        //       return _searchDetail;
        //   }
        //   set
        //   {
        //       if (value != null)
        //       {
        //           SetProperty(ref _searchDetail, value, () => SearchDetail);
        //       }
        //   }
        //}

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

            if (this._selectedMstItem == null)
            {
                return;
            }

            masterDialog = new P414101MasterDialog(new PurVo() { AREA_CD = M_SL_AREA_NM.CLSS_CD, AREA_NM = M_SL_AREA_NM.CLSS_DESC, ITM_CD = this._selectedMstItem.ITM_CD });
            masterDialog.Title = _title + " - 추가";
            masterDialog.Owner = Application.Current.MainWindow;
            masterDialog.BorderEffect = BorderEffect.Default;
            //masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            //masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            bool isDialog = (bool)masterDialog.ShowDialog();
            if (isDialog)
            {
                SelectMstDetail();
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
            if (SearchDetail == null)
            {
                return;
            }
            ////else if (SelectMstList.Count <= 0)
            ////{
            ////    return;
            ////}

            masterDialog = new P414101MasterDialog(SearchDetail);
            masterDialog.Title = _title + " - 수정";
            masterDialog.Owner = Application.Current.MainWindow;
            masterDialog.BorderEffect = BorderEffect.Default;
            //masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            //masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            bool isDialog = (bool)masterDialog.ShowDialog();
            if (isDialog)
            {
                SelectMstDetail();
            }
        }



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
        public async void DelContact()
        {
            if (SearchDetail == null)
            {
                return;
            }
            PurVo delDao = SearchDetail;
            if (delDao != null)
            {
                MessageBoxResult result = WinUIMessageBox.Show("[" + delDao.ASSY_SEQ + "/" + delDao.ITM_NM + "]" + " 정말로 삭제 하시겠습니까?", "[삭제]" + _title, MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    SelectedMstItem.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("p414101/dtl/d", new StringContent(JsonConvert.SerializeObject(delDao), System.Text.Encoding.UTF8, "application/json")))
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
                            SelectMstDetail();

                            //성공
                            WinUIMessageBox.Show("삭제가 완료되었습니다.", _title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                        }
                    }


                    //        //try
                    //        //{
                    //        //    DXSplashScreen.Show<ProgressWindow>();
                    //        //    PurVo resultVo = purClient.P4411DeleteDtl(new PurVo() { PUR_ORD_NO = delDao.PUR_ORD_NO });
                    //        //    if (!resultVo.isSuccess)
                    //        //    {
                    //        //        DXSplashScreen.Close();
                    //        //        //실패
                    //        //        WinUIMessageBox.Show(resultVo.Message, "[에러]" + this._title, MessageBoxButton.OK, MessageBoxImage.Error);
                    //        //        return;
                    //        //    }
                    //        //    SelectMstDetail();
                    //        //    resultVo = purClient.P4411DeleteMst(new PurVo() { PUR_ORD_NO = delDao.PUR_ORD_NO });
                    //        //    if (!resultVo.isSuccess)
                    //        //    {
                    //        //        DXSplashScreen.Close();
                    //        //        //실패
                    //        //        WinUIMessageBox.Show(resultVo.Message, "[에러]" + this._title, MessageBoxButton.OK, MessageBoxImage.Error);
                    //        //        return;
                    //        //    }
                    //        //    Refresh();

                    //        //    DXSplashScreen.Close();
                    //        //    WinUIMessageBox.Show("삭제가 완료되었습니다.", "[삭제]" + _title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                    //        //}
                    //        //catch (System.Exception eLog)
                    //        //{
                    //        //    DXSplashScreen.Close();
                    //        //    WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                    //        //    return;
                    //        //}
                    //    }
                }
            }
        }


        //public ICommand PrintDialogCommand
        //{
        //    get
        //    {
        //        if (_printDialogCommand == null)
        //            _printDialogCommand = new DelegateCommand(PrintContact);
        //        return _printDialogCommand;
        //    }
        //}


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

            ////CustomerList = SystemProperties.SYSTEM_CODE_VO("L-002");
            //using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s143", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { DELT_FLG = "N", SEEK_AP = "AP", SEEK = "AP", CO_TP_CD = "AP", AREA_CD = SystemProperties.USER_VO.EMPE_PLC_NM, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
            //{
            //    if (response.IsSuccessStatusCode)
            //    {
            //        this.CustomerList = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
            //    }
            //}

            //Refresh();
        }
    }
}
