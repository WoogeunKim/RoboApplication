using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Code;
using ModelsLibrary.Inv;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Media;
using AquilaErpWpfApp3.Util;
using AquilaErpWpfApp3.View.INV.Dialog;

namespace AquilaErpWpfApp3.ViewModel
{
    public sealed class I5512ViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private string _title = "품목 출고 관리";

        //private static InvServiceClient invClient = SystemProperties.InvClient;
        private IList<InvVo> selectedMstList = new List<InvVo>();

        //private ObservableCollection<PurVo> selectedDtlList = new ObservableCollection<PurVo>();
        private IList<InvVo> selectedDtlList = new List<InvVo>();

        private I5512MasterDialog masterDialog;
        
        private I5512NextMonthDialog masterNextMonthDialog;
        private I5512DetailEtcDialog detailEtcDialog;
        private I5512DetailOutDialog detailOutDialog;
        private I5512DetailSampleOutDialog detailSampleOutDialog;


        private I5512BarCodeDialog barcodeDialog;

        ////Menu Dialog
        //private ICommand _searchDialogCommand;
        //private ICommand _newDialogCommand;
        //private ICommand _editDialogCommand;
        //private ICommand _delDialogCommand;

        //private ICommand _newDetailRelDialogCommand;
        //private ICommand _newDetailOthDialogCommand;

        //private ICommand _delDetailDialogCommand;

        //private ICommand _newDetailIGCDialogCommand;


        //private ICommand _ItmEditDialogCommand;
        //private ICommand _nextMonthDialogCommand;

        //private ICommand _searchDetailDialogCommand;
        //private ICommand _newDetailDialogCommand;
        //private ICommand _editDetailDialogCommand;
        //private ICommand _delDetailDialogCommand;

        //private ICommand _newDetailReqDialogCommand;

        //private ICommand reportDialogCommand;

        ////private ICommand _revListDetailDialogCommand;
        ////private ICommand _revNewDetailDialogCommand;

        //private P41MasterDialog masterDialog;
        //private P41DetailDialog detailDialog;
        ////private A21JobItemRevDialog jobItemRevDialog;

        //private P41ReportDialog reportDialog;

        public I5512ViewModel() 
        {
            StartDt = System.DateTime.Now;
            EndDt = System.DateTime.Now;

            SYSTEM_CODE_VO();

        }

        [Command]
        public async void Refresh(string _INSRL_NO = null)
        {
            try
            {
                //DXSplashScreen.Show<ProgressWindow>();
                SearchDetail = null;
                SelectDtlList = null;

                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("i5512/mst", new StringContent(JsonConvert.SerializeObject(new InvVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD, FM_DT = (StartDt).ToString("yyyy-MM-dd"), TO_DT = (EndDt).ToString("yyyy-MM-dd"), AREA_CD = M_SL_AREA_NM.CLSS_CD, CO_TP_CD = (string.IsNullOrEmpty(M_CHECKD) ? "%" : M_CHECKD) }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectMstList = JsonConvert.DeserializeObject<IEnumerable<InvVo>>(await response.Content.ReadAsStringAsync()).Cast<InvVo>().ToList();
                    }   ///
                    //SelectMstList = invClient.I5512SelectMstList(new InvVo() { FM_DT = (StartDt).ToString("yyyy-MM-dd"), TO_DT = (EndDt).ToString("yyyy-MM-dd"), AREA_CD = _AreaMap[TXT_SL_AREA_NM], CO_CD = (M_SL_CO_CD.Equals("전체") ? null : M_SL_CO_CD), CO_TP_CD = (string.IsNullOrEmpty(M_CHECKD) ? "%" : M_CHECKD) });
                    ////
                    Title = "[기간]" + (StartDt).ToString("yyyy-MM-dd") + "~" + (EndDt).ToString("yyyy-MM-dd") + ",   [사업장]" + M_SL_AREA_NM.CLSS_DESC ;

                    if (SelectMstList.Count >= 1)
                    {
                        isM_UPDATE = true;
                        isM_DELETE = true;

                        if (string.IsNullOrEmpty(_INSRL_NO))
                        {
                            SelectedMstItem = SelectMstList[0];
                        }
                        else
                        {
                            SelectedMstItem = SelectMstList.Where(x => x.INSRL_NO.Equals(_INSRL_NO)).LastOrDefault<InvVo>();
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

        //사업장
        //private Dictionary<string, string> _AreaMap = new Dictionary<string, string>();
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
        //    set { SetProperty(ref _M_SL_AREA_NM, value, () => M_SL_AREA_NM); }
        //}
        //사업장 
        private SystemCodeVo _M_SL_AREA_NM;
        public SystemCodeVo M_SL_AREA_NM
        {
            get { return _M_SL_AREA_NM; }
            set { SetProperty(ref _M_SL_AREA_NM, value, () => M_SL_AREA_NM); }
        }

        //구분
        private bool? _M_CHECKD_ALL = true;
        public bool? M_CHECKD_ALL
        {
            get { return _M_CHECKD_ALL; }
            set { SetProperty(ref _M_CHECKD_ALL, value, () => M_CHECKD_ALL, SelectCheckdAll); }
        }
        private bool? _M_CHECKD_PO = false;
        public bool? M_CHECKD_PO
        {
            get { return _M_CHECKD_PO; }
            set { SetProperty(ref _M_CHECKD_PO, value, () => M_CHECKD_PO, SelectCheckdPo); }
        }
        private bool? _M_CHECKD_IV = false;
        public bool? M_CHECKD_IV
        {
            get { return _M_CHECKD_IV; }
            set { SetProperty(ref _M_CHECKD_IV, value, () => M_CHECKD_IV, SelectCheckdIv); }
        }

        private String M_CHECKD = string.Empty;
        private String M_CHECKD_NAME = string.Empty;
        private void SelectCheckdAll()
        {
            if (M_CHECKD_ALL == true)
            {
                //M_CHECKD_ALL = true;
                M_CHECKD_PO = false;
                M_CHECKD_IV = false;
                //
                M_CHECKD = "";
                M_CHECKD_NAME = "전체";
            }

            if (M_CHECKD_ALL == false && M_CHECKD_PO == false && M_CHECKD_IV == false)
            {
                M_CHECKD_ALL = true;
                M_CHECKD_NAME = "전체";
            }

            //else
            //{
            //    M_CHECKD_PO = true;
            //    M_CHECKD = "PO";
            //    M_CHECKD_NAME = "국내";
            //}
        }
        private void SelectCheckdPo()
        {
            if (M_CHECKD_PO == true)
            {
                //M_CHECKD_PO = true;
                M_CHECKD_ALL = false;
                M_CHECKD_IV = false;
                //
                M_CHECKD = "AP";
                M_CHECKD_NAME = "국내";
            }

            if (M_CHECKD_ALL == false && M_CHECKD_PO == false && M_CHECKD_IV == false)
            {
                M_CHECKD_ALL = true;
                M_CHECKD_NAME = "전체";
            }
            //else
            //{
            //    M_CHECKD_ALL = true;
            //    M_CHECKD = "";
            //    M_CHECKD_NAME = "전체";
            //}
        }
        private void SelectCheckdIv()
        {
            if (M_CHECKD_IV == true)
            {
                //M_CHECKD_IV = true;
                M_CHECKD_PO = false;
                M_CHECKD_ALL = false;
                //
                M_CHECKD = "OR";
                M_CHECKD_NAME = "수입";
            }

            if (M_CHECKD_ALL == false && M_CHECKD_PO == false && M_CHECKD_IV == false)
            {
                M_CHECKD_ALL = true;
                M_CHECKD_NAME = "전체";
            }
            //else
            //{
            //    M_CHECKD_ALL = true;
            //    M_CHECKD = "";
            //    M_CHECKD_NAME = "전체";
            //}
        }

        //private void RefreshCoNm()
        //{
        //    if (TXT_SL_AREA_NM != null)
        //    {
        //        CustomerList = SystemProperties.CUSTOMER_CODE_VO(null, _AreaMap[TXT_SL_AREA_NM]);
        //        CustomerList.Insert(0, new CustomerCodeDao() { CO_NO = "전체" });
        //        _CustomerMap = SystemProperties.CUSTOMER_CODE_MAP(null, _AreaMap[TXT_SL_AREA_NM]);
        //        if (CustomerList.Count > 0)
        //        {
        //            M_SL_CO_CD = CustomerList[0].CO_NO;
        //        }
        //    }
        //}

        ////거래처 
        //private Dictionary<string, string> _CustomerMap = new Dictionary<string, string>();
        //private IList<CustomerCodeDao> _CustomerList = new List<CustomerCodeDao>();
        //public IList<CustomerCodeDao> CustomerList
        //{
        //    get { return _CustomerList; }
        //    set { SetProperty(ref _CustomerList, value, () => CustomerList); }
        //}

        //private string _M_SL_CO_CD = string.Empty;
        //public string M_SL_CO_CD
        //{
        //    get { return _M_SL_CO_CD; }
        //    set { SetProperty(ref _M_SL_CO_CD, value, () => M_SL_CO_CD); }
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


        public IList<InvVo> SelectMstList
        {
            get { return selectedMstList; }
            set { SetProperty(ref selectedMstList, value, () => SelectMstList); }
        }

        InvVo _selectedMstItem;
        public InvVo SelectedMstItem
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
                 else
                 {
                     if (this.SelectedMstItem.CLZ_FLG.Equals("Y"))
                     {
                         isM_UPDATE = false;
                         isM_DELETE = false;
                     }
                     else
                     {
                         isM_UPDATE = true;
                         isM_DELETE = true;
                     }
                 }

                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("i5512/dtl", new StringContent(JsonConvert.SerializeObject(SelectedMstItem), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectDtlList = JsonConvert.DeserializeObject<IEnumerable<InvVo>>(await response.Content.ReadAsStringAsync()).Cast<InvVo>().ToList();

                        //SelectDtlList = new ObservableCollection<PurVo>(purClient.P4401SelectDtlList(new PurVo() { ITM_CD = this._selectedMstItem.}));
                        //SelectDtlList = invClient.I5512SelectDtlList(SelectedMstItem);
                        // //
                        if (SelectDtlList?.Count >= 1)
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
                        //DXSplashScreen.Close();
                    }
                }
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
        public IList<InvVo> SelectDtlList
        {
            get { return selectedDtlList; }
            set { SetProperty(ref selectedDtlList, value, () => SelectDtlList); }
        }

        InvVo _searchDetail;
        public InvVo SearchDetail
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
            masterDialog = new I5512MasterDialog(new InvVo() { INAUD_DT = System.DateTime.Now.ToString("yyyy-MM-dd"), RQST_EMPE_ID = SystemProperties.USER_VO.USR_ID, AREA_NM = SystemProperties.USER_VO.EMPE_PLC_CD, AREA_CD = SystemProperties.USER_VO.EMPE_PLC_NM });
            masterDialog.Title = _title + " - 추가";
            masterDialog.Owner = Application.Current.MainWindow;
            masterDialog.BorderEffect = BorderEffect.Default;
            masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            bool isDialog = (bool)masterDialog.ShowDialog();
            if (isDialog)
            {
                Refresh(masterDialog.INSRL_NO);
                //if (masterDialog.IsEdit == false)
                //{
                //    try
                //    {
                //        DXSplashScreen.Show<ProgressWindow>();

                //        DXSplashScreen.Close();
                //    }
                //    catch (System.Exception eLog)
                //    {
                //        DXSplashScreen.Close();
                //        WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                //        return;
                //    }
                //}
            }
        }

        [Command]
        public void EditContact()
        {
            if (SelectedMstItem == null)
            {
                return;
            }

            masterDialog = new I5512MasterDialog(SelectedMstItem);
            masterDialog.Title = _title + " - 수정";
            masterDialog.Owner = Application.Current.MainWindow;
            masterDialog.BorderEffect = BorderEffect.Default;
            masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            bool isDialog = (bool)masterDialog.ShowDialog();
            if (isDialog)
            {
                Refresh(masterDialog.INSRL_NO);
                //if (masterDialog.IsEdit == false)
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
                //        WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                //        return;
                //    }
                //}
            }
        }




        [Command]
        public async void DelContact()
        {
            if (SelectedMstItem == null) 
            {
                return; 
            }
            InvVo delDao = SelectedMstItem;
            if (delDao != null)
            {
                MessageBoxResult result = WinUIMessageBox.Show("[" + delDao.INSRL_NO + "]" + " 정말로 삭제 하시겠습니까?", "[삭제]" + _title, MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("i5512/mst/d", new StringContent(JsonConvert.SerializeObject(SelectedMstItem), System.Text.Encoding.UTF8, "application/json")))
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

                        //InvVo resultVo = invClient.I5512DeleteDtl(new InvVo() { INSRL_NO = delDao.INSRL_NO });
                        //if (!resultVo.isSuccess)
                        //{
                        //    //실패
                        //    WinUIMessageBox.Show(resultVo.Message, "[에러]" + _title, MessageBoxButton.OK, MessageBoxImage.Error);
                        //    return;
                        //}

                        //resultVo = invClient.I5512DeleteMst(new InvVo() { INSRL_NO = delDao.INSRL_NO });
                        //if (!resultVo.isSuccess)
                        //{
                        //    //실패
                        //    WinUIMessageBox.Show(resultVo.Message, "[에러]" + _title, MessageBoxButton.OK, MessageBoxImage.Error);
                        //    return;
                        //}

                        //DXSplashScreen.Show<ProgressWindow>();
                        //SelectMstDetail();
                        //Refresh();
                        //DXSplashScreen.Close();
                        //WinUIMessageBox.Show("삭제가 완료되었습니다.", "[삭제]" + _title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                    }
                    catch (System.Exception eLog)
                    {
                        WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                        return;
                    }
                }
            }
        }



        [Command]
        public void NewEtcContact()
        {
            try
            {
                if (SelectedMstItem == null)
                {
                    return;
                }

                SelectedMstItem.FM_DT = (StartDt).ToString("yyyy-MM-dd");
                SelectedMstItem.TO_DT = (EndDt).ToString("yyyy-MM-dd");
                //SelectedMstItem.GRP_ID = (string.IsNullOrEmpty(M_DEPT_DESC) ? null : _DeptMap[M_DEPT_DESC]);
                //SelectedMstItem.GRP_NM = M_DEPT_DESC;


                //if (string.IsNullOrEmpty(SelectedMstItem.GRP_ID))
                //{
                //    WinUIMessageBox.Show("[부서]전체를 선택 하실수 없습니다", "[조회 조건]품목 입고 관리", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                //    return;
                //}

                detailEtcDialog = new I5512DetailEtcDialog(SelectedMstItem);
                detailEtcDialog.Title = "기타 출고 - " + SelectedMstItem.INSRL_NO;
                detailEtcDialog.Owner = Application.Current.MainWindow;
                detailEtcDialog.BorderEffect = BorderEffect.Default;
                detailEtcDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
                detailEtcDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
                bool isDialog = (bool)detailEtcDialog.ShowDialog();
                //if (isDialog)
                {

                    //DXSplashScreen.Show<ProgressWindow>();
                    SelectMstDetail();
                    //DXSplashScreen.Close();

                }
            }
            catch (System.Exception)
            {
                //DXSplashScreen.Close();
                //WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }


        }

        [Command]
        public void NewOutContact()
        {
            try
            {
                if (SelectedMstItem == null)
                {
                    return;
                }

                SelectedMstItem.FM_DT = (StartDt).ToString("yyyy-MM-dd");
                SelectedMstItem.TO_DT = (EndDt).ToString("yyyy-MM-dd");
                //SelectedMstItem.GRP_ID = (string.IsNullOrEmpty(M_DEPT_DESC) ? null : _DeptMap[M_DEPT_DESC]);
                //SelectedMstItem.GRP_NM = M_DEPT_DESC;


                //if (string.IsNullOrEmpty(SelectedMstItem.GRP_ID))
                //{
                //    WinUIMessageBox.Show("[부서]전체를 선택 하실수 없습니다", "[조회 조건]품목 입고 관리", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                //    return;
                //}


                detailOutDialog = new I5512DetailOutDialog(SelectedMstItem);
                detailOutDialog.Title = "외주 출고 - " + SelectedMstItem.INSRL_NO;
                detailOutDialog.Owner = Application.Current.MainWindow;
                detailOutDialog.BorderEffect = BorderEffect.Default;
                detailOutDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
                detailOutDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
                bool isDialog = (bool)detailOutDialog.ShowDialog();
                //if (isDialog)
                {

                    //DXSplashScreen.Show<ProgressWindow>();
                    SelectMstDetail();
                    //DXSplashScreen.Close();

                }
            }
            catch (System.Exception)
            {
                //DXSplashScreen.Close();
                //WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }


        }


        [Command]
        public void NewSampleOutContact()
        {
            try
            {
                if (SelectedMstItem == null)
                {
                    return;
                }

                SelectedMstItem.FM_DT = (StartDt).ToString("yyyy-MM-dd");
                SelectedMstItem.TO_DT = (EndDt).ToString("yyyy-MM-dd");
                //SelectedMstItem.GRP_ID = (string.IsNullOrEmpty(M_DEPT_DESC) ? null : _DeptMap[M_DEPT_DESC]);
                //SelectedMstItem.GRP_NM = M_DEPT_DESC;


                //if (string.IsNullOrEmpty(SelectedMstItem.GRP_ID))
                //{
                //    WinUIMessageBox.Show("[부서]전체를 선택 하실수 없습니다", "[조회 조건]품목 입고 관리", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                //    return;
                //}


                detailSampleOutDialog = new I5512DetailSampleOutDialog(SelectedMstItem);
                detailSampleOutDialog.Title = "샘플 출고 - " + SelectedMstItem.INSRL_NO;
                detailSampleOutDialog.Owner = Application.Current.MainWindow;
                detailSampleOutDialog.BorderEffect = BorderEffect.Default;
                detailSampleOutDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
                detailSampleOutDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
                bool isDialog = (bool)detailSampleOutDialog.ShowDialog();
                //if (isDialog)
                {

                    //DXSplashScreen.Show<ProgressWindow>();
                    SelectMstDetail();
                    //DXSplashScreen.Close();

                }
            }
            catch (System.Exception)
            {
                //DXSplashScreen.Close();
                //WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }


        }




        [Command]
        public async void DelDtlContact()
        {
            try
            {
                if (SearchDetail == null)
                {
                    return;
                }

                MessageBoxResult result = WinUIMessageBox.Show("[" + SearchDetail.INSRL_NO + "/" + SearchDetail.INSRL_SEQ + "]" + " 정말로 삭제 하시겠습니까?", "[삭제]" + _title, MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("i5512/dtl/d", new StringContent(JsonConvert.SerializeObject(SearchDetail), System.Text.Encoding.UTF8, "application/json")))
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

                        SelectMstDetail();
                        WinUIMessageBox.Show("삭제가 완료되었습니다.", "[삭제]" + _title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                    }
                }


                //if (SelectedMstItem == null)
                //{
                //    return;
                //}
                //InvVo delDao = SearchDetail;
                //if (delDao != null)
                //{
                //    MessageBoxResult result = WinUIMessageBox.Show("[" + delDao.INSRL_NO + "/" + delDao.INSRL_SEQ + "]" + " 정말로 삭제 하시겠습니까?", "[삭제]품목 출고 관리", MessageBoxButton.YesNo, MessageBoxImage.Question);
                //    if (result == MessageBoxResult.Yes)
                //    {
                //        InvVo resultVo = invClient.I5512DeleteDtl(delDao);
                //        if (!resultVo.isSuccess)
                //        {
                //            //실패
                //            WinUIMessageBox.Show(resultVo.Message, "[에러]" + _title, MessageBoxButton.OK, MessageBoxImage.Error);
                //            return;
                //        }

                //        DXSplashScreen.Show<ProgressWindow>();
                //        SelectMstDetail();
                //        //purClient.P4411DeleteMst(delDao);
                //        //Refresh();
                //        DXSplashScreen.Close();

                //        WinUIMessageBox.Show("삭제가 완료되었습니다.", "[삭제]" + _title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);

                //    }
                //}

                ////수정 삭제
                //if (SelectedMstItem == null)
                //{
                //    return;
                //}

                //detailEditDialog = new I5512DetailEditDialog(SelectedMstItem);
                //detailEditDialog.Title = this._title + " - 수정/삭제 [출고 번호 : " + SelectedMstItem.INSRL_NM + "]";
                //detailEditDialog.Owner = Application.Current.MainWindow;
                //detailEditDialog.BorderEffect = BorderEffect.Default;
                //////masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
                //////masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
                //bool isDialog = (bool)detailEditDialog.ShowDialog();
                //if (isDialog)
                //{
                //    DXSplashScreen.Show<ProgressWindow>();
                //    SelectMstDetail();
                //    DXSplashScreen.Close();

                //}
            }
            catch (System.Exception)
            {
                //DXSplashScreen.Close();
                //WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]품목 출고 관리", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }



     
        [Command]
        public async void NextMonthContact()
        {
            if (this.SelectedMstItem == null)
            {
                return;
            }
            else if (this.SelectedMstItem.CLZ_FLG.Equals("Y"))
            {
                return;
            }

            masterNextMonthDialog = new I5512NextMonthDialog(SelectedMstItem);
            masterNextMonthDialog.Title = "이월 등록";
            masterNextMonthDialog.Owner = Application.Current.MainWindow;
            masterNextMonthDialog.BorderEffect = BorderEffect.Default;
            masterNextMonthDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            masterNextMonthDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            //detailDialog.IsEdit = false;
            bool isDialog = (bool)masterNextMonthDialog.ShowDialog();
            if (isDialog)
            {
                //if (detailDialog.IsEdit == false)
                //{
                try
                {
                    int _Num = 0;
                    string _INSRL_NO = SelectedMstItem.INSRL_NO;

                    SelectedMstItem.INAUD_RMK = masterNextMonthDialog.NXT_MON_DT + " 일자 이월건";
                    SelectedMstItem.INAUD_DT = masterNextMonthDialog.NXT_MON_DT;
                    SelectedMstItem.CRE_USR_ID = SystemProperties.USER;
                    SelectedMstItem.UPD_USR_ID = SystemProperties.USER;
                    SelectedMstItem.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;

                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("i5512/mst/u", new StringContent(JsonConvert.SerializeObject(SelectedMstItem), System.Text.Encoding.UTF8, "application/json")))
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
                        Refresh(_INSRL_NO);
                    }
                    //InvVo resultVo = invClient.I5511UpdateMst(SelectedMstItem);
                    //if (!resultVo.isSuccess)
                    //{
                    //    //실패
                    //    WinUIMessageBox.Show(resultVo.Message, "[에러]" + _title, MessageBoxButton.OK, MessageBoxImage.Error);
                    //    DXSplashScreen.Close();
                    //    return;
                    //}

                    //DXSplashScreen.Close();
                }
                catch (System.Exception eLog)
                {
                    WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                    return;
                }
                //}
            }

        }


        [Command]
        public void BarCode()
        {
            try
            {
                if (SearchDetail == null)
                {
                    return;
                }

                barcodeDialog = new I5512BarCodeDialog(SearchDetail);
                barcodeDialog.Title = _title + " - 바코드";
                barcodeDialog.Owner = Application.Current.MainWindow;
                barcodeDialog.BorderEffect = BorderEffect.Default;
                barcodeDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
                barcodeDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
                bool isDialog = (bool)barcodeDialog.ShowDialog();
                //if (isDialog)
                //{ }
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }

        public async void SYSTEM_CODE_VO()
        {
            //사업장
            //AreaList = SystemProperties.SYSTEM_CODE_VO("L-002");
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
