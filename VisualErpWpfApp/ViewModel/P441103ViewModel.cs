using AquilaErpWpfApp3.Util;
using AquilaErpWpfApp3.View.PUR.Dialog;
using AquilaErpWpfApp3.View.PUR.Report;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Printing;
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
using System.Windows.Media;

namespace AquilaErpWpfApp3.ViewModel
{
    public sealed class P441103ViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private string _title = "부자재 발주서 등록";

        //private static PurServiceClient purClient = SystemProperties.PurClient;
        private IList<PurVo> selectedMstList = new List<PurVo>();

        //private ObservableCollection<PurVo> selectedDtlList = new ObservableCollection<PurVo>();
        private IList<PurVo> selectedDtlList = new List<PurVo>();


        //private IList<SystemCodeVo> customerList = new List<SystemCodeVo>();
        private IList<SystemCodeVo> selectCustomerList = new List<SystemCodeVo>();

        private P441103MasterDialog masterDialog;
        private P441103DetailDialog detailDialog;

        private P441103EmailDialog emailDialog;
        private P441103Detail_2Dialog detailDialog_2;

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

        public P441103ViewModel() 
        {

            //WeekDt = System.DateTime.Now;

            StartDt = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy-MM-01"));
            EndDt = System.DateTime.Now;

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
        public async void Refresh(string _PUR_ORD_NO = null)
        {
            try
            {
                //DXSplashScreen.Show<ProgressWindow>();
                SearchDetail = null;
                SelectDtlList = null;
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("p441103/mst", new StringContent(JsonConvert.SerializeObject(new PurVo() { FM_DT = (StartDt).ToString("yyyy-MM-dd"), TO_DT = (EndDt).ToString("yyyy-MM-dd"), AREA_CD = M_SL_AREA_NM.CLSS_CD, CHNL_CD = SystemProperties.USER_VO.CHNL_CD, PUR_ITM_CD = "B" }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectMstList = JsonConvert.DeserializeObject<IEnumerable<PurVo>>(await response.Content.ReadAsStringAsync()).Cast<PurVo>().ToList();
                    }
                    //string tmpCO_NO = "";
                    //if (SelectedItemDept != null) { tmpCO_NO=SelectedItemDept.CO_NO; }
                    //SelectMstList = purClient.P4411SelectMstList(new PurVo() { FM_DT = (StartDt).ToString("yyyy-MM-dd"), TO_DT = (EndDt).ToString("yyyy-MM-dd"), AREA_CD = (string.IsNullOrEmpty(TXT_SL_AREA_NM) ? null : _AreaMap[TXT_SL_AREA_NM]), PUR_CO_CD = (string.IsNullOrEmpty(TXT_DEPT_DESC) ? null : TXT_DEPT_DESC) });
                    //Title = "[기간]" + (StartDt).ToString("yyyy-MM-dd") + "~" + (EndDt).ToString("yyyy-MM-dd") + ",   [사업장]" + (string.IsNullOrEmpty(TXT_SL_AREA_NM) ? "전체" : TXT_SL_AREA_NM) + ",   [매입처]" + (string.IsNullOrEmpty(TXT_DEPT_DESC) ? "전체" : _DeptMap[TXT_DEPT_DESC]) + (string.IsNullOrEmpty(M_SEARCH_TEXT) ? "" : (",   [검 색]" + M_SEARCH_TEXT));
                    //SelectMstList = purClient.P4411SelectMstList(new PurVo() { FM_DT = (StartDt).ToString("yyyy-MM-dd"), TO_DT = (EndDt).ToString("yyyy-MM-dd"), AREA_CD = (string.IsNullOrEmpty(TXT_SL_AREA_NM) ? null : _AreaMap[TXT_SL_AREA_NM]), PUR_CO_CD = (string.IsNullOrEmpty(tmpCO_NO) ? null : tmpCO_NO) });
                    Title = "[기간]" + (StartDt).ToString("yyyy-MM-dd") + "~" + (EndDt).ToString("yyyy-MM-dd") + ",   [사업장]" + M_SL_AREA_NM.CLSS_DESC + ",   [발주 품목]부자재";

                    if (SelectMstList.Count >= 1)
                    {
                        isM_UPDATE = true;
                        isM_DELETE = true;

                        if (string.IsNullOrEmpty(_PUR_ORD_NO))
                        {
                            SelectedMstItem = SelectMstList[0];
                        }
                        else
                        {
                            SelectedMstItem = SelectMstList.Where(x => x.PUR_ORD_NO.Equals(_PUR_ORD_NO)).LastOrDefault<PurVo>();
                        }
                    }
                    else
                    {
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

        private string _M_SEARCH_TEXT = string.Empty;
        public string M_SEARCH_TEXT
        {
            get { return _M_SEARCH_TEXT; }
            set { SetProperty(ref _M_SEARCH_TEXT, value, () => M_SEARCH_TEXT); }
        }

        //출력 수
        private string _M_BAR_TEXT = "1";
        public string M_BAR_TEXT
        {
            get { return _M_BAR_TEXT; }
            set { SetProperty(ref _M_BAR_TEXT, value, () => M_BAR_TEXT); }
        }


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

        //
        //
        //
        #region 바코드 (현품표)
        //
        private bool? _IS_AUTO_PRN = true;
        public bool? IS_AUTO_PRN
        {
            get { return _IS_AUTO_PRN; }
            set { SetProperty(ref _IS_AUTO_PRN, value, () => IS_AUTO_PRN, CheckdAutoPrn); }
        }

        private bool? _IS_SALF_PRN = false;
        public bool? IS_SALF_PRN
        {
            get { return _IS_SALF_PRN; }
            set { SetProperty(ref _IS_SALF_PRN, value, () => IS_SALF_PRN, CheckdSalfPrn); }
        }

        public void CheckdAutoPrn()
        {
            if (this.IS_AUTO_PRN == true)
            {
                //this.IS_AUTO_PRN = true;
                this.IS_SALF_PRN = false;
            }

            if (this.IS_AUTO_PRN == false && this.IS_SALF_PRN == false)
            {
                this.IS_AUTO_PRN = true;
            }
        }

        public void CheckdSalfPrn()
        {
            if (this.IS_SALF_PRN == true)
            {
                //this.IS_SALF_PRN = true;
                this.IS_AUTO_PRN = false;
            }

            if (this.IS_AUTO_PRN == false && this.IS_SALF_PRN == false)
            {
                this.IS_SALF_PRN = true;
            }
        }

        #endregion


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

                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("p441103/dtl", new StringContent(JsonConvert.SerializeObject(SelectedMstItem), System.Text.Encoding.UTF8, "application/json")))
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
            masterDialog = new P441103MasterDialog(new PurVo() { AREA_CD = M_SL_AREA_NM.CLSS_CD, AREA_NM = M_SL_AREA_NM.CLSS_DESC, PUR_DT = System.DateTime.Now.ToString("yyyy-MM-dd"), PUR_CLZ_FLG = "N", PUR_EMPE_ID = SystemProperties.USER_VO.USR_ID });
            masterDialog.Title = _title + " - 추가";
            masterDialog.Owner = Application.Current.MainWindow;
            masterDialog.BorderEffect = BorderEffect.Default;
            masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            bool isDialog = (bool)masterDialog.ShowDialog();
            if (isDialog)
            {
                Refresh(masterDialog.PUR_ORD_NO);
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
            if (SelectedMstItem == null)
            {
                return;
            }
            //else if (SelectMstList.Count <= 0)
            //{
            //    return;
            //}

            masterDialog = new P441103MasterDialog(SelectedMstItem);
            masterDialog.Title = _title + " - 수정";
            masterDialog.Owner = Application.Current.MainWindow;
            masterDialog.BorderEffect = BorderEffect.Default;
            masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            bool isDialog = (bool)masterDialog.ShowDialog();
            if (isDialog)
            {
                Refresh(masterDialog.PUR_ORD_NO);
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
            try
            {
                if (SelectedMstItem == null) 
                {
                    return; 
                }
                PurVo delDao = SelectedMstItem;
                if (delDao != null)
                {
                    MessageBoxResult result = WinUIMessageBox.Show("[" + delDao.PUR_ORD_NO + "]" + " 정말로 삭제 하시겠습니까?", "[삭제]" + _title, MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        SelectedMstItem.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
                        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("p441103/mst/d", new StringContent(JsonConvert.SerializeObject(SelectedMstItem), System.Text.Encoding.UTF8, "application/json")))
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
                                Refresh();

                                //성공
                                WinUIMessageBox.Show("삭제가 완료되었습니다.", _title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                            }
                        }


                        //try
                        //{
                        //    DXSplashScreen.Show<ProgressWindow>();
                        //    PurVo resultVo = purClient.P4411DeleteDtl(new PurVo() { PUR_ORD_NO = delDao.PUR_ORD_NO });
                        //    if (!resultVo.isSuccess)
                        //    {
                        //        DXSplashScreen.Close();
                        //        //실패
                        //        WinUIMessageBox.Show(resultVo.Message, "[에러]" + this._title, MessageBoxButton.OK, MessageBoxImage.Error);
                        //        return;
                        //    }
                        //    SelectMstDetail();
                        //    resultVo = purClient.P4411DeleteMst(new PurVo() { PUR_ORD_NO = delDao.PUR_ORD_NO });
                        //    if (!resultVo.isSuccess)
                        //    {
                        //        DXSplashScreen.Close();
                        //        //실패
                        //        WinUIMessageBox.Show(resultVo.Message, "[에러]" + this._title, MessageBoxButton.OK, MessageBoxImage.Error);
                        //        return;
                        //    }
                        //    Refresh();

                        //    DXSplashScreen.Close();
                        //    WinUIMessageBox.Show("삭제가 완료되었습니다.", "[삭제]" + _title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                        //}
                        //catch (System.Exception eLog)
                        //{
                        //    DXSplashScreen.Close();
                        //    WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                        //    return;
                        //}
                    }
                }
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
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


        [Command]
        public async void PrintTagContact(string _obj)
        {
            try
            {
                if (SearchDetail == null)
                {
                    return;
                }
                else if (string.IsNullOrEmpty(M_BAR_TEXT))
                {
                    return;
                }
                else if (Convert.ToInt32(M_BAR_TEXT) <= 0)
                {
                    return;
                }

                //
                List<PurVo> _barCodeList;
                P441103TagReport report;
                //출력 수
                if (IS_SALF_PRN == true)
                {

                    MessageBoxResult result = WinUIMessageBox.Show("[출력 수 : " + M_BAR_TEXT + "]" + " 정말로 생성 하시겠습니까?", _title, MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        if (_obj.Equals("A"))
                        {
                            //바코드 출력
                            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("p441103/dtl/barcode", new StringContent(JsonConvert.SerializeObject(new PurVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD, PUR_ORD_NO = SearchDetail.PUR_ORD_NO, RN = Convert.ToInt32(M_BAR_TEXT), R_MM_11 = "SALF" }), System.Text.Encoding.UTF8, "application/json")))
                            {
                                if (response.IsSuccessStatusCode)
                                {
                                    _barCodeList = JsonConvert.DeserializeObject<IEnumerable<PurVo>>(await response.Content.ReadAsStringAsync()).Cast<PurVo>().ToList();


                                    report = new P441103TagReport(SelectDtlList);
                                    report.Margins.Top = 0;
                                    report.Margins.Bottom = 0;
                                    report.Margins.Left = 20;
                                    report.Margins.Right = 0;
                                    report.Landscape = true;
                                    report.PrintingSystem.ShowPrintStatusDialog = true;
                                    report.PaperKind = System.Drawing.Printing.PaperKind.A5;

                                    report.Watermark.Text = Properties.Settings.Default.SettingCompany;
                                    report.Watermark.TextDirection = DevExpress.XtraPrinting.Drawing.DirectionMode.ForwardDiagonal;
                                    report.Watermark.Font = new System.Drawing.Font(report.PrintingSystem.Watermark.Font.FontFamily, 40);
                                    report.Watermark.ForeColor = System.Drawing.Color.PaleTurquoise;
                                    report.Watermark.TextTransparency = 150;

                                    var window = new DocumentPreviewWindow();
                                    window.PreviewControl.DocumentSource = report;
                                    report.CreateDocument(true);
                                    window.Title = "현품표(A5) [" + SelectedMstItem.PUR_ORD_NO + "] ";
                                    window.Owner = Application.Current.MainWindow;
                                    window.ShowDialog();
                                }
                            }
                        }
                        else if (_obj.Equals("B"))
                        {
                            SearchDetail.RN = Convert.ToInt32(M_BAR_TEXT);
                            SearchDetail.R_MM_11 = "SALF";
                            //바코드 출력
                            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("p441103/dtl/barcode", new StringContent(JsonConvert.SerializeObject(SearchDetail), System.Text.Encoding.UTF8, "application/json")))
                            {
                                if (response.IsSuccessStatusCode)
                                {
                                    _barCodeList = JsonConvert.DeserializeObject<IEnumerable<PurVo>>(await response.Content.ReadAsStringAsync()).Cast<PurVo>().ToList();


                                    report = new P441103TagReport(_barCodeList);
                                    report.Margins.Top = 0;
                                    report.Margins.Bottom = 0;
                                    report.Margins.Left = 20;
                                    report.Margins.Right = 0;
                                    report.Landscape = true;
                                    report.PrintingSystem.ShowPrintStatusDialog = true;
                                    report.PaperKind = System.Drawing.Printing.PaperKind.A5;

                                    report.Watermark.Text = Properties.Settings.Default.SettingCompany;
                                    report.Watermark.TextDirection = DevExpress.XtraPrinting.Drawing.DirectionMode.ForwardDiagonal;
                                    report.Watermark.Font = new System.Drawing.Font(report.PrintingSystem.Watermark.Font.FontFamily, 40);
                                    report.Watermark.ForeColor = System.Drawing.Color.PaleTurquoise;
                                    report.Watermark.TextTransparency = 150;

                                    var window = new DocumentPreviewWindow();
                                    window.PreviewControl.DocumentSource = report;
                                    report.CreateDocument(true);
                                    window.Title = "현품표(A5) [" + SelectedMstItem.PUR_ORD_NO + "/" + SelectedMstItem.PUR_ORD_SEQ + "] ";
                                    window.Owner = Application.Current.MainWindow;
                                    window.ShowDialog();
                                }
                            }
                        }
                    }
                }

                else if (this.IS_AUTO_PRN == true)
                {
                    MessageBoxResult result = WinUIMessageBox.Show("[포장 단위]" + " 정말로 생성 하시겠습니까?", _title, MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        //자동 계산
                        //SelectDtlList
                        if (_obj.Equals("A"))
                        {
                            _barCodeList = new List<PurVo>();

                            for (int x = 0; x < this.SelectDtlList.Count; x++)
                            {
                                if (Convert.ToInt32(this.SelectDtlList[x].PK_PER_QTY) <= 0)
                                {
                                    continue;
                                }

                                this.SelectDtlList[x].R_MM_11 = "AUTO";
                                this.SelectDtlList[x].SL_ITM_QTY = Convert.ToInt32(this.SelectDtlList[x].PK_PER_QTY);
                                this.SelectDtlList[x].RN = Convert.ToInt32(this.SelectDtlList[x].PUR_QTY) / Convert.ToInt32(this.SelectDtlList[x].PK_PER_QTY) + ((Convert.ToInt32(this.SelectDtlList[x].PUR_QTY) % Convert.ToInt32(this.SelectDtlList[x].PK_PER_QTY)) > 0 ? 1 : 0);

                                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("p441102/dtl/barcode", new StringContent(JsonConvert.SerializeObject(this.SelectDtlList[x]), System.Text.Encoding.UTF8, "application/json")))
                                {
                                    if (response.IsSuccessStatusCode)
                                    {
                                        _barCodeList.AddRange(JsonConvert.DeserializeObject<IEnumerable<PurVo>>(await response.Content.ReadAsStringAsync()).Cast<PurVo>().ToList());

                                        //
                                        if (Convert.ToInt32(this.SearchDetail.PUR_QTY) % Convert.ToInt32(this.SearchDetail.PK_PER_QTY) > 0)
                                        {
                                            _barCodeList[_barCodeList.Count - 1].SL_ITM_QTY = Convert.ToInt32(this.SearchDetail.PUR_QTY) % Convert.ToInt32(this.SearchDetail.PK_PER_QTY);
                                        }
                                    }
                                }
                            }

                            //Report
                            report = new P441103TagReport(_barCodeList);
                            report.Margins.Top = 0;
                            report.Margins.Bottom = 0;
                            report.Margins.Left = 20;
                            report.Margins.Right = 0;
                            report.Landscape = true;
                            report.PrintingSystem.ShowPrintStatusDialog = true;
                            report.PaperKind = System.Drawing.Printing.PaperKind.A5;

                            report.Watermark.Text = Properties.Settings.Default.SettingCompany;
                            report.Watermark.TextDirection = DevExpress.XtraPrinting.Drawing.DirectionMode.ForwardDiagonal;
                            report.Watermark.Font = new System.Drawing.Font(report.PrintingSystem.Watermark.Font.FontFamily, 40);
                            report.Watermark.ForeColor = System.Drawing.Color.PaleTurquoise;
                            report.Watermark.TextTransparency = 150;

                            var window = new DocumentPreviewWindow();
                            window.PreviewControl.DocumentSource = report;
                            report.CreateDocument(true);
                            window.Title = "현품표(A5) [" + SelectedMstItem.PUR_ORD_NO + "] ";
                            window.Owner = Application.Current.MainWindow;
                            window.ShowDialog();


                        }
                        else if (_obj.Equals("B"))
                        {
                            //바코드 출력 - 개별
                            this.SearchDetail.R_MM_11 = "AUTO";
                            this.SearchDetail.SL_ITM_QTY = Convert.ToInt32(this.SearchDetail.PK_PER_QTY);
                            this.SearchDetail.RN = Convert.ToInt32(this.SearchDetail.PUR_QTY) / Convert.ToInt32(this.SearchDetail.PK_PER_QTY) + ((Convert.ToInt32(this.SearchDetail.PUR_QTY) % Convert.ToInt32(this.SearchDetail.PK_PER_QTY)) > 0 ? 1 : 0);

                            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("p441102/dtl/barcode", new StringContent(JsonConvert.SerializeObject(SearchDetail), System.Text.Encoding.UTF8, "application/json")))
                            {
                                if (response.IsSuccessStatusCode)
                                {
                                    _barCodeList = JsonConvert.DeserializeObject<IEnumerable<PurVo>>(await response.Content.ReadAsStringAsync()).Cast<PurVo>().ToList();

                                    //
                                    if(Convert.ToInt32(this.SearchDetail.PUR_QTY) % Convert.ToInt32(this.SearchDetail.PK_PER_QTY) > 0)
                                    {
                                        _barCodeList[_barCodeList.Count - 1].SL_ITM_QTY = Convert.ToInt32(this.SearchDetail.PUR_QTY) % Convert.ToInt32(this.SearchDetail.PK_PER_QTY);
                                    }

                                    report = new P441103TagReport(_barCodeList);
                                    report.Margins.Top = 0;
                                    report.Margins.Bottom = 0;
                                    report.Margins.Left = 20;
                                    report.Margins.Right = 0;
                                    report.Landscape = true;
                                    report.PrintingSystem.ShowPrintStatusDialog = true;
                                    report.PaperKind = System.Drawing.Printing.PaperKind.A5;

                                    report.Watermark.Text = Properties.Settings.Default.SettingCompany;
                                    report.Watermark.TextDirection = DevExpress.XtraPrinting.Drawing.DirectionMode.ForwardDiagonal;
                                    report.Watermark.Font = new System.Drawing.Font(report.PrintingSystem.Watermark.Font.FontFamily, 40);
                                    report.Watermark.ForeColor = System.Drawing.Color.PaleTurquoise;
                                    report.Watermark.TextTransparency = 150;

                                    var window = new DocumentPreviewWindow();
                                    window.PreviewControl.DocumentSource = report;
                                    report.CreateDocument(true);
                                    window.Title = "현품표(A5) [" + SelectedMstItem.PUR_ORD_NO + "/" + SelectedMstItem.PUR_ORD_SEQ + "] ";
                                    window.Owner = Application.Current.MainWindow;
                                    window.ShowDialog();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }


        [Command]
        public async void PrintContact()
        {
            try
            {

                if (SearchDetail == null)
                {
                    return;
                }

                SystemCodeVo coNm = new SystemCodeVo();
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s143/dtl", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { DELT_FLG = "N", CO_NO = this.SelectedMstItem.PUR_CO_CD, AREA_CD = SystemProperties.USER_VO.EMPE_PLC_NM, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        coNm = JsonConvert.DeserializeObject<SystemCodeVo>(await response.Content.ReadAsStringAsync());
                    }
                    //매입처
                    SelectDtlList[0].R_MM_04 = coNm.CO_NM;
                    //전화번호 / 팩스번호
                    SelectDtlList[0].R_MM_05 = "( 전화번호 : " + coNm.HDQTR_PHN_NO + "  /   " + "팩스번호 : " + coNm.HDQTR_FAX_NO + " )";
                    //대표자 명
                    SelectDtlList[0].R_MM_06 = coNm.PRSD_NM;
                }

                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s143/dtl", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { DELT_FLG = "N", CO_NO = "99999", AREA_CD = SystemProperties.USER_VO.EMPE_PLC_NM, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        coNm = JsonConvert.DeserializeObject<SystemCodeVo>(await response.Content.ReadAsStringAsync());
                    }
                    //화성코스메틱 주식회사
                    SelectDtlList[0].R_MM_07 = coNm.CO_NM;
                    SelectDtlList[0].R_MM_08 = coNm.PRSD_NM;
                    SelectDtlList[0].R_MM_09 = "(" + coNm.HDQTR_PST_NO + ")  " + coNm.HDQTR_ADDR;
                    SelectDtlList[0].R_MM_10 = coNm.HDQTR_PHN_NO;
                    SelectDtlList[0].R_MM_11 = coNm.HDQTR_FAX_NO;
                }

                //
                SelectDtlList[SelectDtlList.Count - 1].GBN = "합계금액 : " + String.Format("{0:#,#}", SelectDtlList.Sum<PurVo>(s => Convert.ToDouble(s.PUR_AMT)));
                SelectDtlList[SelectDtlList.Count - 1].R_MM_02 = "발주담당자명 : " + this.SelectedMstItem.CRE_USR_NM;
                SelectDtlList[SelectDtlList.Count - 1].R_MM_01 = System.DateTime.Now.ToString("yyyy년   MM월   dd일");
                SelectDtlList[SelectDtlList.Count - 1].PUR_RMK = this.SelectedMstItem.PUR_RMK;

                P441103Report report = new P441103Report(SelectDtlList);
                report.Margins.Top = 30;
                report.Margins.Bottom = 10;
                report.Margins.Left = 120;
                report.Margins.Right = 10;
                report.Landscape = true;
                report.PrintingSystem.ShowPrintStatusDialog = true;
                report.PaperKind = System.Drawing.Printing.PaperKind.A4;

                report.Watermark.Text = Properties.Settings.Default.SettingCompany;
                report.Watermark.TextDirection = DevExpress.XtraPrinting.Drawing.DirectionMode.ForwardDiagonal;
                report.Watermark.Font = new System.Drawing.Font(report.PrintingSystem.Watermark.Font.FontFamily, 40);
                report.Watermark.ForeColor = System.Drawing.Color.PaleTurquoise;
                report.Watermark.TextTransparency = 150;

                var window = new DocumentPreviewWindow();
                window.PreviewControl.DocumentSource = report;
                report.CreateDocument(true);
                window.Title = "부자재 발주서 [" + SelectedMstItem.PUR_ORD_NO + "] ";
                window.Owner = Application.Current.MainWindow;
                window.ShowDialog();
            }
            catch
            {
                return;
            }

        }

        //#region Functon Command <add, Edit, Del>
        //public ICommand SearchDtlDialogCommand
        //{
        //    get
        //    {
        //        if (_searchDetailDialogCommand == null)
        //            _searchDetailDialogCommand = new DelegateCommand(SelectMstDetail);
        //        return _searchDetailDialogCommand;
        //    }
        //}

        //public ICommand NewDtlDialogCommand
        //{
        //    get
        //    {
        //        if (_newDetailDialogCommand == null)
        //            _newDetailDialogCommand = new DelegateCommand(NewDtlContact);
        //        return _newDetailDialogCommand;
        //    }
        //}

        [Command]
        public void NewDtlContact()
        {
            if (SelectedMstItem == null)
            {
                return;
            }

            detailDialog = new P441103DetailDialog(new PurVo() { PUR_ORD_NO = SelectedMstItem.PUR_ORD_NO, CO_CD = SelectedMstItem.PUR_CO_CD, CHNL_CD = SystemProperties.USER_VO.CHNL_CD, CRE_USR_ID = SystemProperties.USER_VO.USR_ID, UPD_USR_ID = SystemProperties.USER_VO.USR_ID });
            detailDialog.Title = "발주 자재 관리 - " + SelectedMstItem.CO_NM + " / 부자재 소요량 전개[" + SelectedMstItem.PUR_ORD_NO + "]";
            detailDialog.Owner = Application.Current.MainWindow;
            detailDialog.BorderEffect = BorderEffect.Default;
            detailDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            detailDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            bool isDialog = (bool)detailDialog.ShowDialog();
            if (isDialog)
            {
                SelectMstDetail();
                //
                SelectedMstItem.PUR_SUM_AMT = this.SelectDtlList.Sum<PurVo>(s => Convert.ToDouble(s.PUR_AMT));
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
        [Command]
        public void EditDtlContact()
        {
            if (SelectedMstItem == null)
            {
                return;
            }

            detailDialog_2 = new P441103Detail_2Dialog(new PurVo() { PUR_ORD_NO = SelectedMstItem.PUR_ORD_NO, CHNL_CD = SystemProperties.USER_VO.CHNL_CD, CRE_USR_ID = SystemProperties.USER_VO.USR_ID, UPD_USR_ID = SystemProperties.USER_VO.USR_ID });
            detailDialog_2.Title = "발주 자재 관리 - 기타 발주 등록[" + SelectedMstItem.PUR_ORD_NO + "]";
            detailDialog_2.Owner = Application.Current.MainWindow;
            detailDialog_2.BorderEffect = BorderEffect.Default;
            detailDialog_2.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            detailDialog_2.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            bool isDialog = (bool)detailDialog_2.ShowDialog();
            if (isDialog)
            {
                SelectMstDetail();
            }
        }



        //public ICommand ExcelDtlDialogCommand
        //{
        //    get
        //    {
        //        if (_excelDetailDialogCommand == null)
        //            _excelDetailDialogCommand = new DelegateCommand(ExcelDtlContact);
        //        return _excelDetailDialogCommand;
        //    }
        //}

        public void ExcelDtlContact()
        {
            //if (SelectedMstItem == null)
            //{
            //    return;
            //}

            //excelDialog = new P4411ExcelDialog(SelectedMstItem);
            //excelDialog.Title = "엑셀 업로드[품번/수량/단가/비고] - " + SelectedMstItem.PUR_ORD_NO;
            //excelDialog.Owner = Application.Current.MainWindow;
            //excelDialog.BorderEffect = BorderEffect.Default;
            //////masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            //////masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            //bool isDialog = (bool)excelDialog.ShowDialog();
            //if (isDialog)
            //{
            //    SelectMstDetail();
            //    //if (masterDialog.IsEdit == false)
            //    //{
            //    //    Refresh();
            //    //}
            //}
        }

        
        //public ICommand DelDtlDialogCommand
        //{
        //    get
        //    {
        //        if (_delDetailDialogCommand == null)
        //            _delDetailDialogCommand = new DelegateCommand(DelDtlContact);
        //        return _delDetailDialogCommand;
        //    }
        //}

        //public void DelDtlContact()
        //{
        //    if (SelectedMstItem == null)
        //    {
        //        return;
        //    }
        //    PurVo delDao = SearchDetail;
        //    if (delDao != null)
        //    {
        //        MessageBoxResult result = WinUIMessageBox.Show("[" + delDao.PUR_ORD_NO + "/" + delDao.PUR_ORD_SEQ + "]" + " 정말로 삭제 하시겠습니까?", "[삭제]" + _title, MessageBoxButton.YesNo, MessageBoxImage.Question);
        //        if (result == MessageBoxResult.Yes)
        //        {
        //            try
        //            {
        //                DXSplashScreen.Show<ProgressWindow>();
        //                PurVo resultVo = purClient.P4411DeleteDtl(delDao);
        //                if (!resultVo.isSuccess)
        //                {
        //                    DXSplashScreen.Close();
        //                    //실패
        //                    WinUIMessageBox.Show(resultVo.Message, "[에러]" + this._title, MessageBoxButton.OK, MessageBoxImage.Error);
        //                    return;
        //                }
        //                SelectMstDetail();
        //                //purClient.P4411DeleteMst(delDao);
        //                //Refresh();
        //                DXSplashScreen.Close();

        //                WinUIMessageBox.Show("삭제가 완료되었습니다.", "[삭제]" + _title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
        //            }
        //            catch (System.Exception eLog)
        //            {
        //                DXSplashScreen.Close();
        //                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
        //                return;
        //            }
        //        }
        //    }
        //}

        [Command]
        public async void DelDtlContact()
        {
            try
            {
                //수정 삭제
                if (SearchDetail == null)
                {
                    return;
                }

                MessageBoxResult result = WinUIMessageBox.Show("[" + SearchDetail.PUR_ORD_NO + "/" + SearchDetail.PUR_ORD_SEQ + "]" + " 정말로 삭제 하시겠습니까?", "[삭제]" + _title, MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    SelectedMstItem.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("p441103/dtl/d", new StringContent(JsonConvert.SerializeObject(SearchDetail), System.Text.Encoding.UTF8, "application/json")))
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
                            //Refresh();
                            SelectMstDetail();
                            //
                            SelectedMstItem.PUR_SUM_AMT = this.SelectDtlList.Sum<PurVo>(s => Convert.ToDouble(s.PUR_AMT));

                            //성공
                            WinUIMessageBox.Show("삭제가 완료되었습니다.", _title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
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


        //public void ShowMasterDialog(PurVo dao)
        //{
        //    masterDialog = new P41MasterDialog(dao);
        //    masterDialog.Title = "견적 마스터 관리";
        //    masterDialog.Owner = Application.Current.MainWindow;
        //    masterDialog.BorderEffect = BorderEffect.Default;
        //    ////masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
        //    ////masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
        //    bool isDialog = (bool)masterDialog.ShowDialog();
        //    if (isDialog)
        //    {
        //        if (masterDialog.IsEdit == false)
        //        {
        //            Refresh();
        //        }
        //    }
        //}

        //public ICommand DelDialogCommand
        //{
        //    get
        //    {
        //        if (_delDialogCommand == null)
        //            _delDialogCommand = new DelegateCommand(DelMasterContact);
        //        return _delDialogCommand;
        //    }
        //}

        //public void DelMasterContact()
        //{
        //    if (SelectedMstJobItem == null) { return; }
        //    PurVo delDao = SelectedMstJobItem;
        //    if (delDao != null)
        //    {
        //        MessageBoxResult result = WinUIMessageBox.Show("[" + delDao.PUR_ESTM_NO + "]" + " 정말로 삭제 하시겠습니까?", "[삭제]견적 관리", MessageBoxButton.YesNo, MessageBoxImage.Question);
        //        if (result == MessageBoxResult.Yes)
        //        {
        //            purClient.DeletePurEstmDtl(delDao);
        //            DetailRefresh();

        //            purClient.DeletePurEstmMast(delDao);
        //            WinUIMessageBox.Show("삭제가 완료되었습니다.", "[삭제]견적 관리", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
        //            Refresh();
        //            _selectedMstJobItem = new PurVo();
        //        }
        //    }
        //}


        //public ICommand NewDetailDialogCommand
        //{
        //    get
        //    {
        //        if (_newDetailDialogCommand == null)
        //            _newDetailDialogCommand = new DelegateCommand(NewDetailContact);
        //        return _newDetailDialogCommand;
        //    }
        //}

        //public void NewDetailContact()
        //{
        //    if (SelectedMstJobItem == null) { return; }
        //    PurVo itemDao = SelectedMstJobItem;
        //    ShowMasterItemDialog(new PurVo() { PUR_ESTM_NO = itemDao.PUR_ESTM_NO });
        //}

        //public ICommand EditDetailDialogCommand
        //{
        //    get
        //    {
        //        if (_editDetailDialogCommand == null)
        //            _editDetailDialogCommand = new DelegateCommand(EditDetailContact);
        //        return _editDetailDialogCommand;
        //    }
        //}

        //public void EditDetailContact()
        //{
        //    if (SelectedMstJobItem == null) { return; }
        //    if (SearchDetailJob == null) { return; }
        //    PurVo editDao = SearchDetailJob;
        //    if (editDao != null)
        //    {
        //        ShowMasterItemDialog(editDao);
        //    }
        //}


        //public void ShowMasterItemDialog(PurVo editDao)
        //{
        //    if (SelectedMstJobItem == null) { return; }
        //    if (string.IsNullOrEmpty(editDao.PUR_ESTM_NO))
        //    {
        //        return;
        //    }
        //    ////JobVo itemDao = SearchMstDetailJob;
        //    ////
        //    detailDialog = new P41DetailDialog(editDao);
        //    detailDialog.Title = "견적 의뢰 내역 관리 [" + editDao.PUR_ESTM_NO + "]";
        //    detailDialog.Owner = Application.Current.MainWindow;
        //    detailDialog.BorderEffect = BorderEffect.Default;
        //    //jobItemDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
        //    //jobItemDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
        //    bool isDialog = (bool)detailDialog.ShowDialog();
        //    if (isDialog)
        //    {
        //        if (detailDialog.IsEdit == false)
        //        {
        //            DetailRefresh();
        //        }
        //    }
        //}

        //public ICommand SearchDetailDialogCommand
        //{
        //    get
        //    {
        //        if (_searchDetailDialogCommand == null)
        //            _searchDetailDialogCommand = new DelegateCommand(DetailRefresh);
        //        return _searchDetailDialogCommand;
        //    }
        //}

        //private void DetailRefresh()
        //{
        //    SelectDtlItmList = purClient.SelectPurEstmDtlList(this._selectedMstJobItem);
        //}


        //public ICommand DelDetailDialogCommand
        //{
        //    get
        //    {
        //        if (_delDetailDialogCommand == null)
        //            _delDetailDialogCommand = new DelegateCommand(DelDetailContact);
        //        return _delDetailDialogCommand;
        //    }
        //}

        //public void DelDetailContact()
        //{
        //    if (SelectedMstJobItem == null) { return; }
        //    if (SearchDetailJob == null) { return; }
        //    PurVo delDao = SearchDetailJob;
        //    if (delDao != null)
        //    {
        //        MessageBoxResult result = WinUIMessageBox.Show("[" + delDao.PUR_ESTM_NO + "]" + delDao.PUR_ESTM_SEQ + " 정말로 삭제 하시겠습니까?", "[삭제]견적 관리", MessageBoxButton.YesNo, MessageBoxImage.Question);
        //        if (result == MessageBoxResult.Yes)
        //        {
        //            purClient.DeletePurEstmDtl(delDao);
        //            WinUIMessageBox.Show("삭제가 완료되었습니다.", "[삭제]견적 관리", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
        //            DetailRefresh();
        //        }
        //    }
        //}
        //#endregion


        //public ICommand ReportDialogCommand
        //{
        //    get
        //    {
        //        if (reportDialogCommand == null)
        //            reportDialogCommand = new DelegateCommand(ReportContact);
        //        return reportDialogCommand;
        //    }
        //}

        //public void ReportContact()
        //{
        //    if (SelectedMstJobItem == null) { return; }
        //    ShowReportDialog(SelectedMstJobItem);
        //}

        //public void ShowReportDialog(PurVo dao)
        //{
        //    reportDialog = new P41ReportDialog(dao);
        //    reportDialog.Title = "견적서";
        //    reportDialog.Owner = Application.Current.MainWindow;
        //    reportDialog.BorderEffect = BorderEffect.Default;
        //    //masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
        //    //masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
        //    bool isDialog = (bool)reportDialog.ShowDialog();
        //    //if (isDialog)
        //    //{
        //    //    SearchDetailItem();
        //    //}
        //}

        [Command]
        public void EmailContact()
        {
            if (SelectedMstItem == null)
            {
                return;
            }

            emailDialog = new P441103EmailDialog(SelectedMstItem);
            emailDialog.Title = "E-MAIL 관리 [" + SelectedMstItem.PUR_ORD_NO + "]";
            emailDialog.Owner = Application.Current.MainWindow;
            emailDialog.BorderEffect = BorderEffect.Default;
            emailDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            emailDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));

            bool isDialog = (bool)emailDialog.ShowDialog();
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

            ////CustomerList = SystemProperties.SYSTEM_CODE_VO("L-002");
            //using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s143", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { DELT_FLG = "N", SEEK_AP = "AP", SEEK = "AP", CO_TP_CD = "AP", AREA_CD = SystemProperties.USER_VO.EMPE_PLC_NM, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
            //{
            //    if (response.IsSuccessStatusCode)
            //    {
            //        this.CustomerList = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
            //    }
            //}

            Refresh();
        }
    }
}
