using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.Printing;
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
using System.Windows.Input;
using AquilaErpWpfApp3.Util;
using AquilaErpWpfApp3.View.SAL.Report;

namespace AquilaErpWpfApp3.ViewModel
{
    public sealed class S2212ViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private string title = "출고처리/거래명세서발행";

        //private static SaleOrderServiceClient saleOrderClient = SystemProperties.SaleOrderClient;
        private IList<SaleVo> selectedMstList = new List<SaleVo>();

        //private ObservableCollection<PurVo> selectedDtlList = new ObservableCollection<PurVo>();
        private IList<SaleVo> selectedDtlList = new List<SaleVo>();
        private IList<SaleVo> selectedDtlItems = new List<SaleVo>();


        private IList<SaleVo> selectedCoCdList = new List<SaleVo>();

        //private S2212DetailDialog detailDialog;

        //private S2212ReceiptDialog receiptDialog;


        ////Menu Dialog
        private ICommand _searchDialogCommand;

        //private ICommand _newDialogCommand;

        private ICommand _newDetailDialogCommand;
        private ICommand _editDetailDialogCommand;

        private ICommand _report1DetailDialogCommand;
        private ICommand _report2DetailDialogCommand;
        private ICommand _report3DetailDialogCommand;

        private ICommand _okDialogCommand;

        //private ICommand reportDialogCommand;


        public S2212ViewModel()
        {
            StartDt = System.DateTime.Now;
            EndDt = System.DateTime.Now;

            //사업장
            //구 분
            SYSTEM_CODE_VO();
            // - Refresh();
        }

        [Command]
        public async  void Refresh()
        {
            try
            {
                //DXSplashScreen.Show<ProgressWindow>();
                SearchDetail = null;
                SelectDtlList = null;


                SaleVo _param = new SaleVo();
                _param.FM_DT = (StartDt).ToString("yyyy-MM-dd");
                _param.TO_DT = (EndDt).ToString("yyyy-MM-dd");
                _param.RLSE_CMD_NO = "Y";
                //사업장
                _param.SL_AREA_CD = M_SL_AREA_NM.CLSS_CD;
                _param.SL_AREA_NM = M_SL_AREA_NM.CLSS_DESC;
                //운송 구분
                _param.SL_TRSP_CD = M_SL_TRSP_NM.CLSS_CD;
                _param.SL_TRSP_NM = M_SL_TRSP_NM.CLSS_DESC;
                //운송 업체
                //_param.SL_TRSP_VEH_CD = M_SL_TRSP_VEH_NM.CLSS_CD;
                //_param.SL_TRSP_VEH_NM = M_SL_TRSP_VEH_NM.CLSS_DESC;
                //하차 지역
                _param.SL_TRSP_AREA_CD = M_SL_TRSP_AREA_NM.CLSS_CD;
                _param.SL_TRSP_AREA_NM = M_SL_TRSP_AREA_NM.CLSS_DESC;
                //출하 시간
                //_param.SL_TRSP_TM_CD = M_SL_TRSP_TM_NM.CLSS_CD;
                //_param.SL_TRSP_TM_NM = M_SL_TRSP_TM_NM.CLSS_DESC;
                //거래처
                _param.SL_CO_CD = M_SL_CO_CD.CLSS_CD;
                _param.SL_CO_NM = M_SL_CO_CD.CLSS_DESC;
                //채널
                _param.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
                _param.CRE_USR_ID = SystemProperties.USER;
                //
                //_param.AREA_CD = SystemProperties.USER_VO.EMPE_PLC_NM;


                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s2212/mst", new StringContent(JsonConvert.SerializeObject(_param), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectMstList = JsonConvert.DeserializeObject<IEnumerable<SaleVo>>(await response.Content.ReadAsStringAsync()).Cast<SaleVo>().ToList();
                    }

                    ////
                    Title = "[기간]" + (StartDt).ToString("yyyy-MM-dd") + "~" + (EndDt).ToString("yyyy-MM-dd") + ", [사업장]" + M_SL_AREA_NM.CLSS_DESC + ", [거래처]" + M_SL_CO_CD.CO_NM + ", [운송 구분]" + M_SL_TRSP_NM.CLSS_DESC /*+ " " + M_SL_TRSP_VEH_NM.CLSS_DESC*/ + ", [하차 지역]" + M_SL_TRSP_AREA_NM.CLSS_DESC /*+ " " + M_SL_TRSP_TM_NM.CLSS_DESC*/ + ", [출력 유무]" + M_RLSE_PRT_FLG.CLSS_DESC;

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
                        SelectCoCdList = null;

                        isM_UPDATE = false;
                        isM_DELETE = false;
                        //
                        isD_UPDATE = false;
                        isD_DELETE = false;
                    }
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
                    SetProperty(ref _selectedMstItem, value, () => SelectedMstItem, SelectCoCdDetail);
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
        //사업장
        //private Dictionary<string, string> _AreaMap = new Dictionary<string, string>();
        private IList<SystemCodeVo> _AreaCd = new List<SystemCodeVo>();
        public IList<SystemCodeVo> AreaList
        {
            get { return _AreaCd; }
            set { SetProperty(ref _AreaCd, value, () => AreaList); }
        }

        //사업장
        private SystemCodeVo _M_SL_AREA_NMT = new SystemCodeVo();
        public SystemCodeVo M_SL_AREA_NM
        {
            get { return _M_SL_AREA_NMT; }
            set { SetProperty(ref _M_SL_AREA_NMT, value, () => M_SL_AREA_NM, RefreshCoNm); }
        }
        private void RefreshCoNm()
        {
            if (M_SL_AREA_NM != null)
            {
                //_CustomerList = SystemProperties.CUSTOMER_CODE_VO("AR", _AreaMap[M_SL_AREA_NM]);
                //_CustomerList.Insert(0, new CustomerCodeDao() { CO_NO = "전체" });
                //CustomerList = _CustomerList;

                //_CustomerMap = SystemProperties.CUSTOMER_CODE_MAP("AR", _AreaMap[M_SL_AREA_NM]);
                //if (_CustomerList.Count > 0)
                //{
                //    M_SL_CO_CD = CustomerList[0].CO_NO;
                //}
            }
        }

        //
        //운송 구분
        //private Dictionary<string, string> _TrspMap = new Dictionary<string, string>();
        private IList<SystemCodeVo> _TrspCd = new List<SystemCodeVo>();
        public IList<SystemCodeVo> TrspNmList
        {
            get { return _TrspCd; }
            set { SetProperty(ref _TrspCd, value, () => TrspNmList); }
        }

        //운송 구분
        private SystemCodeVo _M_SL_TRSP_NM = new SystemCodeVo();
        public SystemCodeVo M_SL_TRSP_NM
        {
            get { return _M_SL_TRSP_NM; }
            set { SetProperty(ref _M_SL_TRSP_NM, value, () => M_SL_TRSP_NM, isTrspCd); }
        }

        private bool? _IsTrspCd = false;
        public bool? IsTrspCd
        {
            get { return _IsTrspCd; }
            set { SetProperty(ref _IsTrspCd, value, () => IsTrspCd); }
        }
        private void isTrspCd()
        {
            if (!M_SL_TRSP_NM.Equals("화물"))
            {
                IsTrspCd = false;
            }
            else
            {
                IsTrspCd = true;
            }
        }


        //
        //운송 업체
        //private Dictionary<string, string> _TrspVehMap = new Dictionary<string, string>();
        private IList<SystemCodeVo> _TrspVehCd = new List<SystemCodeVo>();
        public IList<SystemCodeVo> TrspVehNmList
        {
            get { return _TrspVehCd; }
            set { SetProperty(ref _TrspVehCd, value, () => TrspVehNmList); }
        }

        //운송 업체
        private SystemCodeVo _M_SL_TRSP_VEH_NM = new SystemCodeVo();
        public SystemCodeVo M_SL_TRSP_VEH_NM
        {
            get { return _M_SL_TRSP_VEH_NM; }
            set { SetProperty(ref _M_SL_TRSP_VEH_NM, value, () => M_SL_TRSP_VEH_NM); }
        }


        //
        //하차 지역
        //private Dictionary<string, string> _TrspAreaMap = new Dictionary<string, string>();
        private IList<SystemCodeVo> _TrspAreaCd = new List<SystemCodeVo>();
        public IList<SystemCodeVo> TrspAreaNmList
        {
            get { return _TrspAreaCd; }
            set { SetProperty(ref _TrspAreaCd, value, () => TrspAreaNmList); }
        }

        //하차 지역
        private SystemCodeVo _M_SL_TRSP_AREA_NM = new SystemCodeVo();
        public SystemCodeVo M_SL_TRSP_AREA_NM
        {
            get { return _M_SL_TRSP_AREA_NM; }
            set { SetProperty(ref _M_SL_TRSP_AREA_NM, value, () => M_SL_TRSP_AREA_NM, isTrspArea); }
        }

        private bool? _IsTrspArea = false;
        public bool? IsTrspArea
        {
            get { return _IsTrspArea; }
            set { SetProperty(ref _IsTrspArea, value, () => IsTrspArea); }
        }
        private void isTrspArea()
        {
            if (M_SL_TRSP_AREA_NM.Equals("전체"))
            {
                IsTrspArea = false;
            }
            else
            {
                IsTrspArea = true;
            }
        }



        //출하 시간
        //private Dictionary<string, string> _TrspTmMap = new Dictionary<string, string>();
        private IList<SystemCodeVo> _TrspTmCd = new List<SystemCodeVo>();
        public IList<SystemCodeVo> TrspTmNmList
        {
            get { return _TrspTmCd; }
            set { SetProperty(ref _TrspTmCd, value, () => TrspTmNmList); }
        }

        //출하 시간
        private SystemCodeVo _M_SL_TRSP_TM_NM = new SystemCodeVo();
        public SystemCodeVo M_SL_TRSP_TM_NM
        {
            get { return _M_SL_TRSP_TM_NM; }
            set { SetProperty(ref _M_SL_TRSP_TM_NM, value, () => M_SL_TRSP_TM_NM); }
        }


        //거래처 
        //private Dictionary<string, string> _CustomerMap = new Dictionary<string, string>();
        private IList<SystemCodeVo> _CustomerList = new List<SystemCodeVo>();
        public IList<SystemCodeVo> CustomerList
        {
            get { return _CustomerList; }
            set { SetProperty(ref _CustomerList, value, () => CustomerList); }
        }

        private SystemCodeVo _M_SL_CO_CD = new SystemCodeVo();
        public SystemCodeVo M_SL_CO_CD
        {
            get { return _M_SL_CO_CD; }
            set { SetProperty(ref _M_SL_CO_CD, value, () => M_SL_CO_CD); }
        }


        //발행 유무
        //private Dictionary<string, string> _TrspTmMap = new Dictionary<string, string>();
        private IList<SystemCodeVo> _RlseCmdNoCd = new List<SystemCodeVo>();
        public IList<SystemCodeVo> RlseCmdNoList
        {
            get { return _RlseCmdNoCd; }
            set { SetProperty(ref _RlseCmdNoCd, value, () => RlseCmdNoList); }
        }

        private SystemCodeVo _M_RLSE_CMD_NO = new SystemCodeVo();
        public SystemCodeVo M_RLSE_CMD_NO
        {
            get { return _M_RLSE_CMD_NO; }
            set { SetProperty(ref _M_RLSE_CMD_NO, value, () => M_RLSE_CMD_NO); }
        }


        //출력 유무
        //private Dictionary<string, string> _TrspTmMap = new Dictionary<string, string>();
        private IList<SystemCodeVo> _RlsePrtFlgList = new List<SystemCodeVo>();
        public IList<SystemCodeVo> RlsePrtFlgList
        {
            get { return _RlsePrtFlgList; }
            set { SetProperty(ref _RlsePrtFlgList, value, () => RlsePrtFlgList); }
        }

        private SystemCodeVo _M_RLSE_PRT_FLG = new SystemCodeVo();
        public SystemCodeVo M_RLSE_PRT_FLG
        {
            get { return _M_RLSE_PRT_FLG; }
            set { SetProperty(ref _M_RLSE_PRT_FLG, value, () => M_RLSE_PRT_FLG); }
        }


        private async void SelectCoCdDetail()
        {
            try
            {
                if (this.SelectedMstItem == null)
                {
                    return;
                }

                SelectedMstItem.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;

                //SelectCoCdList = saleOrderClient.S2212SelectDtlCoCdList(SelectedMstItem);
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s2212/dtl/co", new StringContent(JsonConvert.SerializeObject(SelectedMstItem), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectCoCdList = JsonConvert.DeserializeObject<IEnumerable<SaleVo>>(await response.Content.ReadAsStringAsync()).Cast<SaleVo>().ToList();
                    }

                    // //
                    if (SelectCoCdList.Count >= 1)
                    {
                        isD_UPDATE = true;
                        isD_DELETE = true;

                        SelectedCoCdItem = SelectCoCdList[0];
                    }
                    else
                    {
                        isD_UPDATE = false;
                        isD_DELETE = false;
                    }
                }
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }


        private async void SelectMstDetail()
        {
            try
            {
                if (this.SelectedMstItem == null)
                {
                    return;
                }

                if (this.SelectedCoCdItem == null)
                {
                    return;
                }

                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s2212/dtl", new StringContent(JsonConvert.SerializeObject(new SaleVo() {CHNL_CD = SystemProperties.USER_VO.CHNL_CD, SL_RLSE_NO = SelectedMstItem.SL_RLSE_NO, RLSE_CMD_NO = SelectedMstItem.RLSE_CMD_NO, RLSE_CMD_SEQ = SelectedMstItem.RLSE_CMD_SEQ, SL_CO_CD = SelectedCoCdItem.SL_CO_CD, AREA_CD = SystemProperties.USER_VO.EMPE_PLC_NM, RLSE_PRT_FLG = (M_RLSE_PRT_FLG.CLSS_DESC.Equals("전체") ? null : (M_RLSE_PRT_FLG.CLSS_DESC.Equals("출력") ? "Y" : "N")) }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectDtlList = JsonConvert.DeserializeObject<IEnumerable<SaleVo>>(await response.Content.ReadAsStringAsync()).Cast<SaleVo>().ToList();
                    }
                    //SelectDtlList = saleOrderClient.S2212SelectDtlList(new JobVo() { SL_RLSE_NO = SelectedMstItem.SL_RLSE_NO, RLSE_CMD_NO = SelectedMstItem.RLSE_CMD_NO, RLSE_CMD_SEQ = SelectedMstItem.RLSE_CMD_SEQ, SL_CO_CD = SelectedCoCdItem.SL_CO_CD, AREA_CD = SystemProperties.USER_VO.EMPE_PLC_NM, RLSE_PRT_FLG = (M_RLSE_PRT_FLG.Equals("전체") ? null : (M_RLSE_PRT_FLG.Equals("출력") ? "Y" : "N")) });

                    // //
                    if (SelectDtlList.Count >= 1)
                    {
                        isD_UPDATE = true;
                        isD_DELETE = true;

                        //SearchDetail = SelectDtlList[0];
                    }
                    else
                    {
                        isD_UPDATE = false;
                        isD_DELETE = false;
                    }
                }
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }

        //#endregion


        //#region Functon <Detail List>
        public IList<SaleVo> SelectDtlList
        {
            get { return selectedDtlList; }
            set { SetProperty(ref selectedDtlList, value, () => SelectDtlList); }
        }

        SaleVo _searchDetail;
        public SaleVo SearchDetail
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

        public IList<SaleVo> SelectedSearchDetails
        {
            get { return selectedDtlItems; }
            set { SetProperty(ref selectedDtlItems, value, () => SelectedSearchDetails); }
        }
        //#endregion


        //#region Functon <Detail List>
        public IList<SaleVo> SelectCoCdList
        {
            get { return selectedCoCdList; }
            set { SetProperty(ref selectedCoCdList, value, () => SelectCoCdList); }
        }

        SaleVo _searchCoCdDetail;
        public SaleVo SelectedCoCdItem
        {
            get
            {
                return _searchCoCdDetail;
            }
            set
            {
                if (value != null)
                {
                    SetProperty(ref _searchCoCdDetail, value, () => SelectedCoCdItem, SelectMstDetail);
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
        public ICommand SearchDialogCommand
        {
            get
            {
                if (_searchDialogCommand == null)
                    _searchDialogCommand = new DelegateCommand(Refresh);
                return _searchDialogCommand;
            }
        }

        public ICommand NewDtlDialogCommand
        {
            get
            {
                if (_newDetailDialogCommand == null)
                    _newDetailDialogCommand = new DelegateCommand(NewDtlContact);
                return _newDetailDialogCommand;
            }
        }

        public void NewDtlContact()
        {
            //SaleVo _param = new SaleVo();
            //_param.FM_DT = (StartDt).ToString("yyyy-MM-dd");
            //_param.TO_DT = (EndDt).ToString("yyyy-MM-dd");
            ////_param.RLSE_CMD_NO = "Y";
            ////사업장
            //_param.SL_AREA_CD = _AreaMap[M_SL_AREA_NM];
            //_param.SL_AREA_NM = M_SL_AREA_NM;
            ////운송 구분
            //_param.SL_TRSP_CD = (M_SL_TRSP_NM.Equals("전체") ? null : _TrspMap[M_SL_TRSP_NM]);
            //_param.SL_TRSP_NM = M_SL_TRSP_NM;
            ////운송 업체
            //_param.SL_TRSP_VEH_CD = (!M_SL_TRSP_NM.Equals("화물") ? null : _TrspVehMap[M_SL_TRSP_VEH_NM]);
            //_param.SL_TRSP_VEH_NM = M_SL_TRSP_VEH_NM;
            ////하차 지역
            //_param.SL_TRSP_AREA_CD = (M_SL_TRSP_AREA_NM.Equals("전체") ? null : _TrspAreaMap[M_SL_TRSP_AREA_NM]);
            //_param.SL_TRSP_AREA_NM = M_SL_TRSP_AREA_NM;
            ////출하 시간
            //_param.SL_TRSP_TM_CD = (M_SL_TRSP_AREA_NM.Equals("전체") ? null : _TrspTmMap[M_SL_TRSP_TM_NM]);
            //_param.SL_TRSP_TM_NM = M_SL_TRSP_TM_NM;
            ////거래처
            //_param.SL_CO_CD = (M_SL_CO_CD.Equals("전체") ? null : _CustomerMap[M_SL_CO_CD]);
            //_param.SL_CO_NM = M_SL_CO_CD;
            ////


            //detailDialog = new S2212DetailDialog(_param);
            //detailDialog.Title = title + " - 추가";
            //detailDialog.Owner = Application.Current.MainWindow;
            //detailDialog.BorderEffect = BorderEffect.Default;
            //////masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            //////masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            ////detailDialog.IsEdit = true;
            //bool isDialog = (bool)detailDialog.ShowDialog();
            ////if (isDialog)
            ////{
            //try
            //{
            //    DXSplashScreen.Show<ProgressWindow>();
            //    SelectMstDetail();
            //    Refresh();
            //    DXSplashScreen.Close();

            //    for (int x = 0; x < SelectMstList.Count; x++)
            //    {
            //        if (detailDialog.RLSE_CMD_NO.Equals(SelectMstList[x].RLSE_CMD_NO))
            //        {
            //            if (detailDialog.RLSE_CMD_SEQ.Equals(SelectMstList[x].RLSE_CMD_SEQ))
            //            {
            //                SelectedMstItem = SelectMstList[x];
            //                return;
            //            }
            //        }
            //    }
            //}
            //catch (System.Exception eLog)
            //{
            //    DXSplashScreen.Close();
            //    WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
            //    return;
            //}
        }

        public ICommand EditDtlDialogCommand
        {
            get
            {
                if (_editDetailDialogCommand == null)
                    _editDetailDialogCommand = new DelegateCommand(EditDtlContact);
                return _editDetailDialogCommand;
            }
        }

        public void EditDtlContact()
        {
            //if (SelectedMstItem == null)
            //{
            //    return;
            //}

            //SaleVo _param = new SaleVo();
            //_param.FM_DT = (StartDt).ToString("yyyy-MM-dd");
            //_param.TO_DT = (EndDt).ToString("yyyy-MM-dd");
            ////_param.RLSE_CMD_NO = "Y";
            ////사업장
            //_param.SL_AREA_CD = _AreaMap[M_SL_AREA_NM];
            //_param.SL_AREA_NM = M_SL_AREA_NM;
            ////운송 구분
            //_param.SL_TRSP_CD = (M_SL_TRSP_NM.Equals("전체") ? null : _TrspMap[M_SL_TRSP_NM]);
            //_param.SL_TRSP_NM = M_SL_TRSP_NM;
            ////운송 업체
            //_param.SL_TRSP_VEH_CD = (!M_SL_TRSP_NM.Equals("화물") ? null : _TrspVehMap[M_SL_TRSP_VEH_NM]);
            //_param.SL_TRSP_VEH_NM = M_SL_TRSP_VEH_NM;
            ////하차 지역
            //_param.SL_TRSP_AREA_CD = (M_SL_TRSP_AREA_NM.Equals("전체") ? null : _TrspAreaMap[M_SL_TRSP_AREA_NM]);
            //_param.SL_TRSP_AREA_NM = M_SL_TRSP_AREA_NM;
            ////출하 시간
            //_param.SL_TRSP_TM_CD = (M_SL_TRSP_AREA_NM.Equals("전체") ? null : _TrspTmMap[M_SL_TRSP_TM_NM]);
            //_param.SL_TRSP_TM_NM = M_SL_TRSP_TM_NM;
            ////거래처
            //_param.SL_CO_CD = (M_SL_CO_CD.Equals("전체") ? null : _CustomerMap[M_SL_CO_CD]);
            //_param.SL_CO_NM = M_SL_CO_CD;
            ////
            //_param.RLSE_CMD_NO = SelectedMstItem.RLSE_CMD_NO;
            //_param.RLSE_CMD_SEQ = SelectedMstItem.RLSE_CMD_SEQ;
            //_param.SL_RLSE_NO = SelectedMstItem.SL_RLSE_NO;

            //detailDialog = new S2212DetailDialog(_param);
            //detailDialog.Title = title + " - 수정 [출하 번호 : " + SelectedMstItem.RLSE_CMD_NO + " / 차수 : " + SelectedMstItem.RLSE_CMD_SEQ + "]";
            //detailDialog.Owner = Application.Current.MainWindow;
            //detailDialog.BorderEffect = BorderEffect.Default;
            //////masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            //////masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            ////detailDialog.IsEdit = true;
            //bool isDialog = (bool)detailDialog.ShowDialog();
            ////if (isDialog)
            ////{
            //try
            //{
            //    DXSplashScreen.Show<ProgressWindow>();
            //    SelectMstDetail();
            //    Refresh();
            //    DXSplashScreen.Close();

            //    for (int x = 0; x < SelectMstList.Count; x++)
            //    {
            //        if (detailDialog.RLSE_CMD_NO.Equals(SelectMstList[x].RLSE_CMD_NO))
            //        {
            //            if (detailDialog.RLSE_CMD_SEQ.Equals(SelectMstList[x].RLSE_CMD_SEQ))
            //            {
            //                SelectedMstItem = SelectMstList[x];
            //                return;
            //            }
            //        }
            //    }
            //}
            //catch (System.Exception eLog)
            //{
            //    DXSplashScreen.Close();
            //    WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
            //    return;
            //}
        }



        //패킹 리스트
        public ICommand Report1DtlDialogCommand
        {
            get
            {
                if (_report1DetailDialogCommand == null)
                    _report1DetailDialogCommand = new DelegateCommand(Report1DtlContact);
                return _report1DetailDialogCommand;
            }
        }

        public void Report1DtlContact()
        {
            //// 재발행 여부 확인

            //if (SelectedMstItem == null)
            //{
            //    return;
            //}
            ////else if (SelectedCoCdItem == null)
            ////{
            ////    return;
            ////}


            //JobVo printDao = SelectedMstItem;
            //if (printDao != null)
            //{

            //    ObservableCollection<string> SL_RLSE_NO_LIST = new ObservableCollection<string>();
            //    ObservableCollection<JobVo> SL_RLSE_NO_VO_LIST = new ObservableCollection<JobVo>();
            //    for (int x = 0; x < SelectedSearchDetails.Count; x++)
            //    {
            //        if (!SL_RLSE_NO_LIST.Contains(SelectedSearchDetails[x].SL_RLSE_NO))
            //        {
            //            SL_RLSE_NO_LIST.Add(SelectedSearchDetails[x].SL_RLSE_NO);
            //            SL_RLSE_NO_VO_LIST.Add(new JobVo() { SL_RLSE_NO = SelectedSearchDetails[x].SL_RLSE_NO, RLSE_CMD_NO = printDao.RLSE_CMD_NO, RLSE_CMD_SEQ = printDao.RLSE_CMD_SEQ });
            //        }
            //    }

            //    ObservableCollection<JobVo> allItems = new ObservableCollection<JobVo>();
            //    for (int x = 0; x < SL_RLSE_NO_VO_LIST.Count; x++)
            //    {
            //        ObservableCollection<JobVo> _allItems = new ObservableCollection<JobVo>(saleOrderClient.S2212SelectMstPackingReportList(SL_RLSE_NO_VO_LIST[x]));
            //        for (int y = 0; y < _allItems.Count; y++)
            //        {
            //            allItems.Add(_allItems[y]);
            //        }
            //    }

            //    //ObservableCollection<JobVo> allItems = new ObservableCollection<JobVo>(saleOrderClient.S2212SelectMstPackingReportList(printDao));
            //    if (allItems == null)
            //    {
            //        return;
            //    }
            //    else if (allItems.Count <= 0)
            //    {
            //        WinUIMessageBox.Show("데이터가 존재 하지 않습니다.", "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
            //        return;
            //    }
            //    else if (allItems.Count < 15)
            //    {
            //        int result = 15 - allItems.Count;

            //        for (int x = 1; x <= result; x++)
            //        {
            //            allItems.Insert(allItems.Count, new JobVo());
            //        }
            //    }
            //    else
            //    {
            //        int nTotal = allItems.Count;
            //        int mod = ((int)nTotal % 15);
            //        int result = 15 - mod;

            //        for (int x = 1; x <= result; x++)
            //        {
            //            allItems.Insert(allItems.Count, new JobVo());
            //        }
            //    }
            //    //allItems[allItems.Count - 1].QC_CD = allItems[0].QC_CD;
            //    //allItems[allItems.Count - 1].PUR_ORD_NO = allItems[0].PUR_ORD_NO;
            //    ////






            //    S2212PackingReport report = new S2212PackingReport(allItems);
            //    report.Margins.Top = 0;
            //    report.Margins.Bottom = 0;
            //    report.Margins.Left = 100;
            //    report.Margins.Right = 0;
            //    report.Landscape = true;
            //    report.PrintingSystem.ShowPrintStatusDialog = true;
            //    report.PaperKind = System.Drawing.Printing.PaperKind.A4;
            //    //if (SelectedCoCdItem.PCK_PRT_FLG.Equals("예"))
            //    //{
            //    //    ////데모 시연 문서 표시 가능
            //    //    //report.Watermark.Text = "재발행";
            //    //    //report.Watermark.TextDirection = DirectionMode.ForwardDiagonal;
            //    //    //report.Watermark.Font = new Font(report.Watermark.Font.FontFamily, 80);
            //    //    //report.Watermark.ForeColor = Color.DodgerBlue;
            //    //    ////report.Watermark.ForeColor = Color.PaleTurquoise;
            //    //    //report.Watermark.TextTransparency = 190;
            //    //    ////report.Watermark.ShowBehind = false;
            //    //    ////report.Watermark.PageRange = "1,3-5";
            //    //}
            //    //else
            //    //{
            //    //    SelectedCoCdItem.PCK_PRT_FLG = "예";
            //    //}


            //    XtraReportPreviewModel model = new XtraReportPreviewModel(report);
            //    DocumentPreviewWindow window = new DocumentPreviewWindow() { Model = model };
            //    report.CreateDocument(true);
            //    window.Title = "패킹 리스트 [" + SelectedMstItem.SL_RLSE_NO + "] ";
            //    window.Owner = Application.Current.MainWindow;
            //    window.ShowDialog();



            //    JobVo resultVo = saleOrderClient.S2212UpdateReport(new JobVo() { SL_RLSE_NO = SelectedMstItem.SL_RLSE_NO, PCK_PRT_FLG = "Y", CRE_USR_ID = SystemProperties.USER, UPD_USR_ID = SystemProperties.USER });
            //    if (!resultVo.isSuccess)
            //    {
            //        //실패
            //        WinUIMessageBox.Show(resultVo.Message, "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
            //        return;
            //    }

            //}
        }


        //인수증 Receipt
        public ICommand Report2DtlDialogCommand
        {
            get
            {
                if (_report2DetailDialogCommand == null)
                    _report2DetailDialogCommand = new DelegateCommand(Report2DtlContact);
                return _report2DetailDialogCommand;
            }
        }

        public void Report2DtlContact()
        {
            //// 재발행 여부 확인

            //if (SelectedMstItem == null)
            //{
            //    return;
            //}
            ////else if (SelectedCoCdItem == null)
            ////{
            ////    return;
            ////}

            //JobVo printDao = SelectedMstItem;
            //if (printDao != null)
            //{

            //    ObservableCollection<string> SL_RLSE_NO_LIST = new ObservableCollection<string>();
            //    for (int x = 0; x < SelectedSearchDetails.Count; x++)
            //    {
            //        if (!SL_RLSE_NO_LIST.Contains(SelectedSearchDetails[x].SL_RLSE_NO))
            //        {
            //            SL_RLSE_NO_LIST.Add(SelectedSearchDetails[x].SL_RLSE_NO);
            //        }
            //    }

            //    if (SL_RLSE_NO_LIST.Count < 0)
            //    {
            //        return;
            //    }


            //    receiptDialog = new S2212ReceiptDialog(SL_RLSE_NO_LIST);
            //    receiptDialog.Title = "인수증 담당자 등록";
            //    receiptDialog.Owner = Application.Current.MainWindow;
            //    receiptDialog.BorderEffect = BorderEffect.Default;
            //    ////masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            //    ////masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            //    //detailDialog.IsEdit = true;
            //    bool isDialog = (bool)receiptDialog.ShowDialog();
            //    if (isDialog)
            //    {
            //        //ObservableCollection<string> SL_RLSE_NO_LIST = new ObservableCollection<string>();
            //        //for (int x = 0; x < SelectedSearchDetails.Count; x++)
            //        //{
            //        //    if (!SL_RLSE_NO_LIST.Contains(SelectedSearchDetails[x].SL_RLSE_NO))
            //        //    {
            //        //        SL_RLSE_NO_LIST.Add(SelectedSearchDetails[x].SL_RLSE_NO);
            //        //    }
            //        //}


            //        ObservableCollection<ReceiptVo> allItems = new ObservableCollection<ReceiptVo>();
            //        for (int x = 0; x < SL_RLSE_NO_LIST.Count; x++)
            //        {
            //            ObservableCollection<ReceiptVo> _allItems = new ObservableCollection<ReceiptVo>(saleOrderClient.S2212SelectMstReceiptReportList(new JobVo() { SL_RLSE_NO = SL_RLSE_NO_LIST[x] }));
            //            for (int y = 0; y < _allItems.Count; y++)
            //            {
            //                allItems.Add(_allItems[y]);
            //            }
            //        }



            //        ObservableCollection<ReceiptVo> reportItems = new ObservableCollection<ReceiptVo>();

            //        //ObservableCollection<ReceiptVo> allItems = new ObservableCollection<ReceiptVo>(saleOrderClient.S2212SelectMstReceiptReportList(printDao));
            //        if (allItems == null)
            //        {
            //            return;
            //        }
            //        else if (allItems.Count <= 0)
            //        {
            //            WinUIMessageBox.Show("데이터가 존재 하지 않습니다.", "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
            //            return;
            //        }
            //        else if (allItems.Count <= 13)
            //        {
            //            reportItems.Add(new ReceiptVo());
            //            for (int x = 0; x < allItems.Count; x++)
            //            {
            //                if (x == 0)
            //                {
            //                    reportItems[0].A1 = allItems[x].RN;
            //                    reportItems[0].A2 = allItems[x].ITM_NM;
            //                    reportItems[0].A3 = allItems[x].ITM_SZ_NM;
            //                    reportItems[0].A4 = allItems[x].UOM_NM;
            //                    reportItems[0].A5 = allItems[x].SL_ITM_QTY;
            //                    reportItems[0].A6 = allItems[x].MD_QTY;
            //                    //
            //                    reportItems[0].SL_RLSE_DT_NM = allItems[x].SL_RLSE_DT_NM;
            //                    reportItems[0].SL_CO_NM = "의뢰업체명 : " + allItems[x].SL_CO_NM;
            //                    reportItems[0].SL_USR_NM = allItems[x].SL_USR_NM;
            //                    reportItems[0].MV_USR_NM = allItems[x].MV_USR_NM;
            //                    reportItems[0].LOC_USR_NM = allItems[x].LOC_USR_NM;
            //                    reportItems[0].PCK_DEST_PHN_NO = "담당자 : " + allItems[x].PCK_DEST_PHN_NO;
            //                    reportItems[0].PCK_DEST_ADDR = "도착지 : " + allItems[x].DEST_USR_NM;
            //                    reportItems[0].PCK_DEST_RMK = "비 고 : " + allItems[x].DEST_RMK;
            //                    reportItems[0].SL_TRSP_TM_NM = "기타사항 : " + allItems[x].SL_TRSP_TM_NM;
            //                    reportItems[0].SL_RLSE_NO = allItems[x].SL_RLSE_NO;
            //                    reportItems[0].PRN_DT = "출력일시 : " + allItems[x].PRN_DT;
            //                }
            //                else if (x == 1)
            //                {
            //                    reportItems[0].B1 = allItems[x].RN;
            //                    reportItems[0].B2 = allItems[x].ITM_NM;
            //                    reportItems[0].B3 = allItems[x].ITM_SZ_NM;
            //                    reportItems[0].B4 = allItems[x].UOM_NM;
            //                    reportItems[0].B5 = allItems[x].SL_ITM_QTY;
            //                    reportItems[0].B6 = allItems[x].MD_QTY;
            //                }
            //                else if (x == 2)
            //                {
            //                    reportItems[0].C1 = allItems[x].RN;
            //                    reportItems[0].C2 = allItems[x].ITM_NM;
            //                    reportItems[0].C3 = allItems[x].ITM_SZ_NM;
            //                    reportItems[0].C4 = allItems[x].UOM_NM;
            //                    reportItems[0].C5 = allItems[x].SL_ITM_QTY;
            //                    reportItems[0].C6 = allItems[x].MD_QTY;
            //                }
            //                else if (x == 3)
            //                {
            //                    reportItems[0].D1 = allItems[x].RN;
            //                    reportItems[0].D2 = allItems[x].ITM_NM;
            //                    reportItems[0].D3 = allItems[x].ITM_SZ_NM;
            //                    reportItems[0].D4 = allItems[x].UOM_NM;
            //                    reportItems[0].D5 = allItems[x].SL_ITM_QTY;
            //                    reportItems[0].D6 = allItems[x].MD_QTY;
            //                }
            //                else if (x == 4)
            //                {
            //                    reportItems[0].E1 = allItems[x].RN;
            //                    reportItems[0].E2 = allItems[x].ITM_NM;
            //                    reportItems[0].E3 = allItems[x].ITM_SZ_NM;
            //                    reportItems[0].E4 = allItems[x].UOM_NM;
            //                    reportItems[0].E5 = allItems[x].SL_ITM_QTY;
            //                    reportItems[0].E6 = allItems[x].MD_QTY;
            //                }
            //                else if (x == 5)
            //                {
            //                    reportItems[0].F1 = allItems[x].RN;
            //                    reportItems[0].F2 = allItems[x].ITM_NM;
            //                    reportItems[0].F3 = allItems[x].ITM_SZ_NM;
            //                    reportItems[0].F4 = allItems[x].UOM_NM;
            //                    reportItems[0].F5 = allItems[x].SL_ITM_QTY;
            //                    reportItems[0].F6 = allItems[x].MD_QTY;
            //                }
            //                else if (x == 6)
            //                {
            //                    reportItems[0].G1 = allItems[x].RN;
            //                    reportItems[0].G2 = allItems[x].ITM_NM;
            //                    reportItems[0].G3 = allItems[x].ITM_SZ_NM;
            //                    reportItems[0].G4 = allItems[x].UOM_NM;
            //                    reportItems[0].G5 = allItems[x].SL_ITM_QTY;
            //                    reportItems[0].G6 = allItems[x].MD_QTY;
            //                }
            //                else if (x == 7)
            //                {
            //                    reportItems[0].H1 = allItems[x].RN;
            //                    reportItems[0].H2 = allItems[x].ITM_NM;
            //                    reportItems[0].H3 = allItems[x].ITM_SZ_NM;
            //                    reportItems[0].H4 = allItems[x].UOM_NM;
            //                    reportItems[0].H5 = allItems[x].SL_ITM_QTY;
            //                    reportItems[0].H6 = allItems[x].MD_QTY;
            //                }
            //                else if (x == 8)
            //                {
            //                    reportItems[0].I1 = allItems[x].RN;
            //                    reportItems[0].I2 = allItems[x].ITM_NM;
            //                    reportItems[0].I3 = allItems[x].ITM_SZ_NM;
            //                    reportItems[0].I4 = allItems[x].UOM_NM;
            //                    reportItems[0].I5 = allItems[x].SL_ITM_QTY;
            //                    reportItems[0].I6 = allItems[x].MD_QTY;
            //                }
            //                else if (x == 9)
            //                {
            //                    reportItems[0].J1 = allItems[x].RN;
            //                    reportItems[0].J2 = allItems[x].ITM_NM;
            //                    reportItems[0].J3 = allItems[x].ITM_SZ_NM;
            //                    reportItems[0].J4 = allItems[x].UOM_NM;
            //                    reportItems[0].J5 = allItems[x].SL_ITM_QTY;
            //                    reportItems[0].J6 = allItems[x].MD_QTY;
            //                }
            //                else if (x == 10)
            //                {
            //                    reportItems[0].K1 = allItems[x].RN;
            //                    reportItems[0].K2 = allItems[x].ITM_NM;
            //                    reportItems[0].K3 = allItems[x].ITM_SZ_NM;
            //                    reportItems[0].K4 = allItems[x].UOM_NM;
            //                    reportItems[0].K5 = allItems[x].SL_ITM_QTY;
            //                    reportItems[0].K6 = allItems[x].MD_QTY;
            //                }
            //                else if (x == 11)
            //                {
            //                    reportItems[0].L1 = allItems[x].RN;
            //                    reportItems[0].L2 = allItems[x].ITM_NM;
            //                    reportItems[0].L3 = allItems[x].ITM_SZ_NM;
            //                    reportItems[0].L4 = allItems[x].UOM_NM;
            //                    reportItems[0].L5 = allItems[x].SL_ITM_QTY;
            //                    reportItems[0].L6 = allItems[x].MD_QTY;
            //                }
            //                else if (x == 12)
            //                {
            //                    reportItems[0].M1 = allItems[x].RN;
            //                    reportItems[0].M2 = allItems[x].ITM_NM;
            //                    reportItems[0].M3 = allItems[x].ITM_SZ_NM;
            //                    reportItems[0].M4 = allItems[x].UOM_NM;
            //                    reportItems[0].M5 = allItems[x].SL_ITM_QTY;
            //                    reportItems[0].M6 = allItems[x].MD_QTY;
            //                }
            //                //else if (x == 13)
            //                //{
            //                //    reportItems[0].B1 = allItems[x].RN;
            //                //    reportItems[0].B2 = allItems[x].ITM_NM;
            //                //    reportItems[0].B3 = allItems[x].ITM_SZ_NM;
            //                //    reportItems[0].B4 = allItems[x].UOM_NM;
            //                //    reportItems[0].B5 = allItems[x].SL_ITM_QTY;
            //                //    reportItems[0].B6 = allItems[x].MD_QTY;
            //                //}
            //            }
            //            //allItems[0].PAGE_NUM = "1/1";
            //        }
            //        else
            //        {
            //            //Page 나누기
            //            int nTotal = allItems.Count;
            //            int nPage = nTotal / 13;
            //            int mod = ((int)nTotal % 13);


            //            int min = 0;
            //            int max = 0;

            //            for (int z = 0; z < nPage; z++)
            //            {
            //                min = z * 13;
            //                max = min + 13;

            //                reportItems.Insert(z, new ReceiptVo());
            //                for (int x = min; x < max; x++)
            //                {
            //                    //allItems[z].PAGE_NUM =  (z + 1) + "/" + nPage;
            //                    if ((x % 13) == 0)
            //                    {
            //                        reportItems[z].A1 = allItems[x].RN;
            //                        reportItems[z].A2 = allItems[x].ITM_NM;
            //                        reportItems[z].A3 = allItems[x].ITM_SZ_NM;
            //                        reportItems[z].A4 = allItems[x].UOM_NM;
            //                        reportItems[z].A5 = allItems[x].SL_ITM_QTY;
            //                        reportItems[z].A6 = allItems[x].MD_QTY;
            //                        //
            //                        reportItems[z].SL_RLSE_DT_NM = allItems[x].SL_RLSE_DT_NM;
            //                        reportItems[z].SL_CO_NM = "의뢰업체명 : " + allItems[x].SL_CO_NM;
            //                        reportItems[z].SL_USR_NM = allItems[x].SL_USR_NM;
            //                        reportItems[z].MV_USR_NM = allItems[x].MV_USR_NM;
            //                        reportItems[z].LOC_USR_NM = allItems[x].LOC_USR_NM;
            //                        reportItems[z].PCK_DEST_PHN_NO = "담당자 : " + allItems[x].PCK_DEST_PHN_NO;
            //                        reportItems[z].PCK_DEST_ADDR = "도착지 : " + allItems[x].DEST_USR_NM;
            //                        reportItems[z].PCK_DEST_RMK = "비 고 : " + allItems[x].DEST_RMK;
            //                        reportItems[z].SL_TRSP_TM_NM = "기타사항 : " + allItems[x].SL_TRSP_TM_NM;
            //                        reportItems[z].SL_RLSE_NO = allItems[x].SL_RLSE_NO;
            //                        reportItems[z].PRN_DT = "출력일시 : " + allItems[x].PRN_DT;
            //                    }
            //                    else if ((x % 13) == 1)
            //                    {
            //                        reportItems[z].B1 = allItems[x].RN;
            //                        reportItems[z].B2 = allItems[x].ITM_NM;
            //                        reportItems[z].B3 = allItems[x].ITM_SZ_NM;
            //                        reportItems[z].B4 = allItems[x].UOM_NM;
            //                        reportItems[z].B5 = allItems[x].SL_ITM_QTY;
            //                        reportItems[z].B6 = allItems[x].MD_QTY;
            //                    }
            //                    else if ((x % 13) == 2)
            //                    {
            //                        reportItems[z].C1 = allItems[x].RN;
            //                        reportItems[z].C2 = allItems[x].ITM_NM;
            //                        reportItems[z].C3 = allItems[x].ITM_SZ_NM;
            //                        reportItems[z].C4 = allItems[x].UOM_NM;
            //                        reportItems[z].C5 = allItems[x].SL_ITM_QTY;
            //                        reportItems[z].C6 = allItems[x].MD_QTY;
            //                    }
            //                    else if ((x % 13) == 3)
            //                    {
            //                        reportItems[z].D1 = allItems[x].RN;
            //                        reportItems[z].D2 = allItems[x].ITM_NM;
            //                        reportItems[z].D3 = allItems[x].ITM_SZ_NM;
            //                        reportItems[z].D4 = allItems[x].UOM_NM;
            //                        reportItems[z].D5 = allItems[x].SL_ITM_QTY;
            //                        reportItems[z].D6 = allItems[x].MD_QTY;
            //                    }
            //                    else if ((x % 13) == 4)
            //                    {
            //                        reportItems[z].E1 = allItems[x].RN;
            //                        reportItems[z].E2 = allItems[x].ITM_NM;
            //                        reportItems[z].E3 = allItems[x].ITM_SZ_NM;
            //                        reportItems[z].E4 = allItems[x].UOM_NM;
            //                        reportItems[z].E5 = allItems[x].SL_ITM_QTY;
            //                        reportItems[z].E6 = allItems[x].MD_QTY;
            //                    }
            //                    else if ((x % 13) == 5)
            //                    {
            //                        reportItems[z].F1 = allItems[x].RN;
            //                        reportItems[z].F2 = allItems[x].ITM_NM;
            //                        reportItems[z].F3 = allItems[x].ITM_SZ_NM;
            //                        reportItems[z].F4 = allItems[x].UOM_NM;
            //                        reportItems[z].F5 = allItems[x].SL_ITM_QTY;
            //                        reportItems[z].F6 = allItems[x].MD_QTY;
            //                    }
            //                    else if ((x % 13) == 6)
            //                    {
            //                        reportItems[z].G1 = allItems[x].RN;
            //                        reportItems[z].G2 = allItems[x].ITM_NM;
            //                        reportItems[z].G3 = allItems[x].ITM_SZ_NM;
            //                        reportItems[z].G4 = allItems[x].UOM_NM;
            //                        reportItems[z].G5 = allItems[x].SL_ITM_QTY;
            //                        reportItems[z].G6 = allItems[x].MD_QTY;
            //                    }
            //                    else if ((x % 13) == 7)
            //                    {
            //                        reportItems[z].H1 = allItems[x].RN;
            //                        reportItems[z].H2 = allItems[x].ITM_NM;
            //                        reportItems[z].H3 = allItems[x].ITM_SZ_NM;
            //                        reportItems[z].H4 = allItems[x].UOM_NM;
            //                        reportItems[z].H5 = allItems[x].SL_ITM_QTY;
            //                        reportItems[z].H6 = allItems[x].MD_QTY;
            //                    }
            //                    else if ((x % 13) == 8)
            //                    {
            //                        reportItems[z].I1 = allItems[x].RN;
            //                        reportItems[z].I2 = allItems[x].ITM_NM;
            //                        reportItems[z].I3 = allItems[x].ITM_SZ_NM;
            //                        reportItems[z].I4 = allItems[x].UOM_NM;
            //                        reportItems[z].I5 = allItems[x].SL_ITM_QTY;
            //                        reportItems[z].I6 = allItems[x].MD_QTY;
            //                    }
            //                    else if ((x % 13) == 9)
            //                    {
            //                        reportItems[z].J1 = allItems[x].RN;
            //                        reportItems[z].J2 = allItems[x].ITM_NM;
            //                        reportItems[z].J3 = allItems[x].ITM_SZ_NM;
            //                        reportItems[z].J4 = allItems[x].UOM_NM;
            //                        reportItems[z].J5 = allItems[x].SL_ITM_QTY;
            //                        reportItems[z].J6 = allItems[x].MD_QTY;
            //                    }
            //                    else if ((x % 13) == 10)
            //                    {
            //                        reportItems[z].K1 = allItems[x].RN;
            //                        reportItems[z].K2 = allItems[x].ITM_NM;
            //                        reportItems[z].K3 = allItems[x].ITM_SZ_NM;
            //                        reportItems[z].K4 = allItems[x].UOM_NM;
            //                        reportItems[z].K5 = allItems[x].SL_ITM_QTY;
            //                        reportItems[z].K6 = allItems[x].MD_QTY;
            //                    }
            //                    else if ((x % 13) == 11)
            //                    {
            //                        reportItems[z].L1 = allItems[x].RN;
            //                        reportItems[z].L2 = allItems[x].ITM_NM;
            //                        reportItems[z].L3 = allItems[x].ITM_SZ_NM;
            //                        reportItems[z].L4 = allItems[x].UOM_NM;
            //                        reportItems[z].L5 = allItems[x].SL_ITM_QTY;
            //                        reportItems[z].L6 = allItems[x].MD_QTY;
            //                    }
            //                    else if ((x % 13) == 12)
            //                    {
            //                        reportItems[z].M1 = allItems[x].RN;
            //                        reportItems[z].M2 = allItems[x].ITM_NM;
            //                        reportItems[z].M3 = allItems[x].ITM_SZ_NM;
            //                        reportItems[z].M4 = allItems[x].UOM_NM;
            //                        reportItems[z].M5 = allItems[x].SL_ITM_QTY;
            //                        reportItems[z].M6 = allItems[x].MD_QTY;
            //                    }
            //                    //else if ((x % 13) == 13)
            //                    //{
            //                    //    allItems[0].B1 = allItems[x].RN;
            //                    //    allItems[0].B2 = allItems[x].ITM_NM;
            //                    //    allItems[0].B3 = allItems[x].ITM_SZ_NM;
            //                    //    allItems[0].B4 = allItems[x].UOM_NM;
            //                    //    allItems[0].B5 = allItems[x].SL_ITM_QTY;
            //                    //    allItems[0].B6 = allItems[x].MD_QTY;
            //                    //}
            //                }
            //            }

            //            //나머지 계산
            //            if (mod != 0)
            //            {
            //                min = nPage * 13;
            //                reportItems.Insert(nPage, new ReceiptVo());
            //                for (int x = min; x < allItems.Count; x++)
            //                {
            //                    if ((x % 13) == 0)
            //                    {
            //                        reportItems[nPage].A1 = allItems[x].RN;
            //                        reportItems[nPage].A2 = allItems[x].ITM_NM;
            //                        reportItems[nPage].A3 = allItems[x].ITM_SZ_NM;
            //                        reportItems[nPage].A4 = allItems[x].UOM_NM;
            //                        reportItems[nPage].A5 = allItems[x].SL_ITM_QTY;
            //                        reportItems[nPage].A6 = allItems[x].MD_QTY;
            //                        //
            //                        reportItems[nPage].SL_RLSE_DT_NM = allItems[x].SL_RLSE_DT_NM;
            //                        reportItems[nPage].SL_CO_NM = "의뢰업체명 : " + allItems[x].SL_CO_NM;
            //                        reportItems[nPage].SL_USR_NM = allItems[x].SL_USR_NM;
            //                        reportItems[nPage].MV_USR_NM = allItems[x].MV_USR_NM;
            //                        reportItems[nPage].LOC_USR_NM = allItems[x].LOC_USR_NM;
            //                        reportItems[nPage].PCK_DEST_PHN_NO = "담당자 : " + allItems[x].PCK_DEST_PHN_NO;
            //                        reportItems[nPage].PCK_DEST_ADDR = "도착지 : " + allItems[x].DEST_USR_NM;
            //                        reportItems[nPage].PCK_DEST_RMK = "비 고 : " + allItems[x].DEST_RMK;
            //                        reportItems[nPage].SL_TRSP_TM_NM = "기타사항 : " + allItems[x].SL_TRSP_TM_NM;
            //                        reportItems[nPage].SL_RLSE_NO = allItems[x].SL_RLSE_NO;
            //                        reportItems[nPage].PRN_DT = "출력일시 : " + allItems[x].PRN_DT;
            //                    }
            //                    else if ((x % 13) == 1)
            //                    {
            //                        reportItems[nPage].B1 = allItems[x].RN;
            //                        reportItems[nPage].B2 = allItems[x].ITM_NM;
            //                        reportItems[nPage].B3 = allItems[x].ITM_SZ_NM;
            //                        reportItems[nPage].B4 = allItems[x].UOM_NM;
            //                        reportItems[nPage].B5 = allItems[x].SL_ITM_QTY;
            //                        reportItems[nPage].B6 = allItems[x].MD_QTY;
            //                    }
            //                    else if ((x % 13) == 2)
            //                    {
            //                        reportItems[nPage].C1 = allItems[x].RN;
            //                        reportItems[nPage].C2 = allItems[x].ITM_NM;
            //                        reportItems[nPage].C3 = allItems[x].ITM_SZ_NM;
            //                        reportItems[nPage].C4 = allItems[x].UOM_NM;
            //                        reportItems[nPage].C5 = allItems[x].SL_ITM_QTY;
            //                        reportItems[nPage].C6 = allItems[x].MD_QTY;
            //                    }
            //                    else if ((x % 13) == 3)
            //                    {
            //                        reportItems[nPage].D1 = allItems[x].RN;
            //                        reportItems[nPage].D2 = allItems[x].ITM_NM;
            //                        reportItems[nPage].D3 = allItems[x].ITM_SZ_NM;
            //                        reportItems[nPage].D4 = allItems[x].UOM_NM;
            //                        reportItems[nPage].D5 = allItems[x].SL_ITM_QTY;
            //                        reportItems[nPage].D6 = allItems[x].MD_QTY;
            //                    }
            //                    else if ((x % 13) == 4)
            //                    {
            //                        reportItems[nPage].E1 = allItems[x].RN;
            //                        reportItems[nPage].E2 = allItems[x].ITM_NM;
            //                        reportItems[nPage].E3 = allItems[x].ITM_SZ_NM;
            //                        reportItems[nPage].E4 = allItems[x].UOM_NM;
            //                        reportItems[nPage].E5 = allItems[x].SL_ITM_QTY;
            //                        reportItems[nPage].E6 = allItems[x].MD_QTY;
            //                    }
            //                    else if ((x % 13) == 5)
            //                    {
            //                        reportItems[nPage].F1 = allItems[x].RN;
            //                        reportItems[nPage].F2 = allItems[x].ITM_NM;
            //                        reportItems[nPage].F3 = allItems[x].ITM_SZ_NM;
            //                        reportItems[nPage].F4 = allItems[x].UOM_NM;
            //                        reportItems[nPage].F5 = allItems[x].SL_ITM_QTY;
            //                        reportItems[nPage].F6 = allItems[x].MD_QTY;
            //                    }
            //                    else if ((x % 13) == 6)
            //                    {
            //                        reportItems[nPage].G1 = allItems[x].RN;
            //                        reportItems[nPage].G2 = allItems[x].ITM_NM;
            //                        reportItems[nPage].G3 = allItems[x].ITM_SZ_NM;
            //                        reportItems[nPage].G4 = allItems[x].UOM_NM;
            //                        reportItems[nPage].G5 = allItems[x].SL_ITM_QTY;
            //                        reportItems[nPage].G6 = allItems[x].MD_QTY;
            //                    }
            //                    else if ((x % 13) == 7)
            //                    {
            //                        reportItems[nPage].H1 = allItems[x].RN;
            //                        reportItems[nPage].H2 = allItems[x].ITM_NM;
            //                        reportItems[nPage].H3 = allItems[x].ITM_SZ_NM;
            //                        reportItems[nPage].H4 = allItems[x].UOM_NM;
            //                        reportItems[nPage].H5 = allItems[x].SL_ITM_QTY;
            //                        reportItems[nPage].H6 = allItems[x].MD_QTY;
            //                    }
            //                    else if ((x % 13) == 8)
            //                    {
            //                        reportItems[nPage].I1 = allItems[x].RN;
            //                        reportItems[nPage].I2 = allItems[x].ITM_NM;
            //                        reportItems[nPage].I3 = allItems[x].ITM_SZ_NM;
            //                        reportItems[nPage].I4 = allItems[x].UOM_NM;
            //                        reportItems[nPage].I5 = allItems[x].SL_ITM_QTY;
            //                        reportItems[nPage].I6 = allItems[x].MD_QTY;
            //                    }
            //                    else if ((x % 13) == 9)
            //                    {
            //                        reportItems[nPage].J1 = allItems[x].RN;
            //                        reportItems[nPage].J2 = allItems[x].ITM_NM;
            //                        reportItems[nPage].J3 = allItems[x].ITM_SZ_NM;
            //                        reportItems[nPage].J4 = allItems[x].UOM_NM;
            //                        reportItems[nPage].J5 = allItems[x].SL_ITM_QTY;
            //                        reportItems[nPage].J6 = allItems[x].MD_QTY;
            //                    }
            //                    else if ((x % 13) == 10)
            //                    {
            //                        reportItems[nPage].K1 = allItems[x].RN;
            //                        reportItems[nPage].K2 = allItems[x].ITM_NM;
            //                        reportItems[nPage].K3 = allItems[x].ITM_SZ_NM;
            //                        reportItems[nPage].K4 = allItems[x].UOM_NM;
            //                        reportItems[nPage].K5 = allItems[x].SL_ITM_QTY;
            //                        reportItems[nPage].K6 = allItems[x].MD_QTY;
            //                    }
            //                    else if ((x % 13) == 11)
            //                    {
            //                        reportItems[nPage].L1 = allItems[x].RN;
            //                        reportItems[nPage].L2 = allItems[x].ITM_NM;
            //                        reportItems[nPage].L3 = allItems[x].ITM_SZ_NM;
            //                        reportItems[nPage].L4 = allItems[x].UOM_NM;
            //                        reportItems[nPage].L5 = allItems[x].SL_ITM_QTY;
            //                        reportItems[nPage].L6 = allItems[x].MD_QTY;
            //                    }
            //                    else if ((x % 13) == 12)
            //                    {
            //                        reportItems[nPage].M1 = allItems[x].RN;
            //                        reportItems[nPage].M2 = allItems[x].ITM_NM;
            //                        reportItems[nPage].M3 = allItems[x].ITM_SZ_NM;
            //                        reportItems[nPage].M4 = allItems[x].UOM_NM;
            //                        reportItems[nPage].M5 = allItems[x].SL_ITM_QTY;
            //                        reportItems[nPage].M6 = allItems[x].MD_QTY;
            //                    }
            //                    //else if ((x % 13) == 13)
            //                    //{
            //                    //    reportItems[nPage].B1 = allItems[x].RN;
            //                    //    reportItems[nPage].B2 = allItems[x].ITM_NM;
            //                    //    reportItems[nPage].B3 = allItems[x].ITM_SZ_NM;
            //                    //    reportItems[nPage].B4 = allItems[x].UOM_NM;
            //                    //    reportItems[nPage].B5 = allItems[x].SL_ITM_QTY;
            //                    //    reportItems[nPage].B6 = allItems[x].MD_QTY;
            //                    //}
            //                }
            //            }



            //        }

            //        S2212ReceiptReport report = new S2212ReceiptReport(reportItems);
            //        report.Margins.Top = 0;
            //        report.Margins.Bottom = 0;
            //        report.Margins.Left = 80;
            //        report.Margins.Right = 0;
            //        report.Landscape = false;
            //        report.PrintingSystem.ShowPrintStatusDialog = true;
            //        report.PaperKind = System.Drawing.Printing.PaperKind.A4;

            //        //if (SelectedCoCdItem.RCV_PRT_FLG.Equals("예"))
            //        //{
            //        //    ////데모 시연 문서 표시 가능
            //        //    //report.Watermark.Text = "재발행";
            //        //    //report.Watermark.TextDirection = DirectionMode.ForwardDiagonal;
            //        //    //report.Watermark.Font = new Font(report.Watermark.Font.FontFamily, 80);
            //        //    //report.Watermark.ForeColor = Color.DodgerBlue;
            //        //    ////report.Watermark.ForeColor = Color.PaleTurquoise;
            //        //    //report.Watermark.TextTransparency = 190;
            //        //    ////report.Watermark.ShowBehind = false;
            //        //    ////report.Watermark.PageRange = "1,3-5";
            //        //}
            //        //else
            //        //{
            //        //    SelectedCoCdItem.RCV_PRT_FLG = "예";
            //        //}


            //        XtraReportPreviewModel model = new XtraReportPreviewModel(report);
            //        DocumentPreviewWindow window = new DocumentPreviewWindow() { Model = model };
            //        report.CreateDocument(true);
            //        window.Owner = Application.Current.MainWindow;
            //        window.Title = "인수증 [" + SelectedMstItem.SL_RLSE_NO + "] ";
            //        window.ShowDialog();


            //        JobVo resultVo = saleOrderClient.S2212UpdateReport(new JobVo() { SL_RLSE_NO = SelectedMstItem.SL_RLSE_NO, RCV_PRT_FLG = "Y", CRE_USR_ID = SystemProperties.USER, UPD_USR_ID = SystemProperties.USER });
            //        if (!resultVo.isSuccess)
            //        {
            //            //실패
            //            WinUIMessageBox.Show(resultVo.Message, "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
            //            return;
            //        }

            //    }
            //}
        }



        //출하 의뢰서
        public ICommand Report3DtlDialogCommand
        {
            get
            {
                if (_report3DetailDialogCommand == null)
                    _report3DetailDialogCommand = new DelegateCommand(Report3DtlContact);
                return _report3DetailDialogCommand;
            }
        }

        public void Report3DtlContact()
        {

            //try
            //{
            //    // 재발행 여부 확인

            //    if (SelectedMstItem == null)
            //    {
            //        return;
            //    }
            //    else if (SelectedCoCdItem == null)
            //    {
            //        return;
            //    }

            //    JobVo printDao = SelectedMstItem;
            //    if (printDao != null)
            //    {
            //        List<string> SL_RLSE_NO_LIST = new List<string>();
            //        for (int x = 0; x < SelectedSearchDetails.Count; x++)
            //        {
            //            if (!SL_RLSE_NO_LIST.Contains(SelectedSearchDetails[x].SL_RLSE_NO))
            //            {
            //                SL_RLSE_NO_LIST.Add(SelectedSearchDetails[x].SL_RLSE_NO);
            //            }
            //        }


            //        //ObservableCollection<JobVo> allItems = new ObservableCollection<JobVo>();
            //        //for (int x = 0; x < SL_RLSE_NO_LIST.Count; x++)
            //        //{
            //        //    ObservableCollection<JobVo> _allItems = new ObservableCollection<JobVo>(saleOrderClient.S2212SelectMstShipmentReportList(new JobVo() { RLSE_CMD_NO = printDao.RLSE_CMD_NO, RLSE_CMD_SEQ = printDao.RLSE_CMD_SEQ, SL_CO_CD = SelectedCoCdItem.SL_CO_CD, SL_RLSE_NO = SL_RLSE_NO_LIST[x], RLSE_PRT_FLG = (M_RLSE_PRT_FLG.Equals("전체") ? null : ( M_RLSE_PRT_FLG.Equals("출력") ? "Y" : "N")) }));
            //        //    for (int y = 0; y < _allItems.Count; y++)
            //        //    {
            //        //        allItems.Add(_allItems[y]);
            //        //    }
            //        //}

            //        if (SL_RLSE_NO_LIST.Count == 0)
            //        {
            //            return;
            //        }
            //        JobVo resultVo1 = saleOrderClient.S2326SelectReportDT(new JobVo() { SL_RLSE_NO = SL_RLSE_NO_LIST[0] });

            //        ObservableCollection<JobVo> allItems = new ObservableCollection<JobVo>(saleOrderClient.S2212SelectMstShipmentReportList(new JobVo() { RLSE_CMD_NO = printDao.RLSE_CMD_NO, RLSE_CMD_SEQ = printDao.RLSE_CMD_SEQ, SL_CO_CD = SelectedCoCdItem.SL_CO_CD, A_SL_RLSE_NO = SL_RLSE_NO_LIST.ToArray(), RLSE_PRT_FLG = (M_RLSE_PRT_FLG.Equals("전체") ? null : (M_RLSE_PRT_FLG.Equals("출력") ? "Y" : "N")) }));

            //        if (allItems == null)
            //        {
            //            return;
            //        }
            //        else if (allItems.Count <= 0)
            //        {
            //            WinUIMessageBox.Show("데이터가 존재 하지 않습니다.", "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
            //            return;
            //        }
            //        else if (allItems.Count < 14)
            //        {
            //            for (int i = 0; i < allItems.Count; i++)
            //            {
            //                string[] Result = allItems[i].JB_NO.Split('-');
            //                allItems[i].UPD_USR_ID = allItems[i].UPD_USR_ID + "-" + Result[1];
            //                allItems[i].SL_RLSE_DT = resultVo1.SL_RLSE_DT;
            //            }
            //            int result = 14 - allItems.Count;

            //            for (int x = 1; x <= result; x++)
            //            {
            //                allItems.Insert(allItems.Count, new JobVo());
            //            }
            //        }
            //        else
            //        {
            //            for (int i = 0; i < allItems.Count; i++)
            //            {
            //                string[] Result = allItems[i].JB_NO.Split('-');
            //                allItems[i].UPD_USR_ID = allItems[i].UPD_USR_ID + "-" + Result[1];
            //                allItems[i].SL_RLSE_DT = resultVo1.SL_RLSE_DT;
            //            }
            //            int nTotal = allItems.Count;
            //            int mod = ((int)nTotal % 14);
            //            int result = 14 - mod;

            //            for (int x = 1; x <= result; x++)
            //            {
            //                allItems.Insert(allItems.Count, new JobVo());
            //            }
            //        }


            //        allItems[allItems.Count - 1].SL_RMK = allItems[0].SL_RMK;
            //        //allItems[allItems.Count - 1].QC_CD = allItems[0].QC_CD;
            //        //allItems[allItems.Count - 1].PUR_ORD_NO = allItems[0].PUR_ORD_NO;
            //        ////
            //        allItems[0].CRE_USR_NM = SystemProperties.USER_VO.USR_N1ST_NM;
            //        // allItems[0].SL_RLSE_DT_NM = SystemProperties.USER_VO.USR_N1ST_NM;

            //        if (SelectedMstItem.RLSE_PRT_FLG.Equals("예"))
            //        {
            //            allItems[0].GBN = "재발행";
            //        }
            //        else
            //        {
            //            allItems[0].GBN = "";
            //        }


            //        S2212ShipmentReport report = new S2212ShipmentReport(allItems);
            //        report.Margins.Top = 0;
            //        report.Margins.Bottom = 0;
            //        report.Margins.Left = 30;
            //        report.Margins.Right = 0;
            //        report.Landscape = true;
            //        report.PrintingSystem.ShowPrintStatusDialog = true;
            //        report.PaperKind = System.Drawing.Printing.PaperKind.A4;

            //        //if (SelectedMstItem.RLSE_PRT_FLG.Equals("예"))
            //        //{
            //        //    ////데모 시연 문서 표시 가능
            //        //    //report.Watermark.Text = "재발행";
            //        //    //report.Watermark.TextDirection = DirectionMode.ForwardDiagonal;
            //        //    //report.Watermark.Font = new Font(report.Watermark.Font.FontFamily, 80);
            //        //    //report.Watermark.ForeColor = Color.DodgerBlue;
            //        //    ////report.Watermark.ForeColor = Color.PaleTurquoise;
            //        //    //report.Watermark.TextTransparency = 190;
            //        //    ////report.Watermark.ShowBehind = false;
            //        //    ////report.Watermark.PageRange = "1,3-5";
            //        //}
            //        //else
            //        //{
            //        //    SelectedMstItem.RLSE_PRT_FLG = "예";
            //        //}

            //        XtraReportPreviewModel model = new XtraReportPreviewModel(report);
            //        DocumentPreviewWindow window = new DocumentPreviewWindow() { Model = model };
            //        report.CreateDocument(true);
            //        window.Owner = Application.Current.MainWindow;
            //        window.Title = "작지 출력";
            //        window.ShowDialog();

            //        JobVo resultVo;
            //        SL_RLSE_NO_LIST = new List<string>();

            //        for (int x = 0; x < SelectedSearchDetails.Count; x++)
            //        {
            //            if (!SL_RLSE_NO_LIST.Contains(SelectedSearchDetails[x].SL_RLSE_NO))
            //            {
            //                resultVo = saleOrderClient.S2212UpdateReport(new JobVo() { RLSE_CMD_NO = SelectedSearchDetails[x].RLSE_CMD_NO, RLSE_CMD_SEQ = SelectedSearchDetails[x].RLSE_CMD_SEQ, RLSE_PRT_FLG = "Y", CRE_USR_ID = SystemProperties.USER, UPD_USR_ID = SystemProperties.USER });
            //                if (!resultVo.isSuccess)
            //                {
            //                    //실패
            //                    WinUIMessageBox.Show(resultVo.Message, "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
            //                    return;
            //                }

            //                resultVo = saleOrderClient.S2212UpdateMstShipmentReport(new JobVo() { SL_RLSE_NO = SelectedSearchDetails[x].SL_RLSE_NO, RLSE_CMD_NO = SelectedSearchDetails[x].RLSE_CMD_NO, RLSE_CMD_SEQ = SelectedSearchDetails[x].RLSE_CMD_SEQ, RLSE_PRT_FLG = "Y", CRE_USR_ID = SystemProperties.USER, UPD_USR_ID = SystemProperties.USER, SL_CO_CD = SelectedCoCdItem.SL_CO_CD });
            //                if (!resultVo.isSuccess)
            //                {
            //                    //실패
            //                    WinUIMessageBox.Show(resultVo.Message, "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
            //                    return;
            //                }
            //                SL_RLSE_NO_LIST.Add(SelectedSearchDetails[x].SL_RLSE_NO);
            //            }
            //        }

            //        //SelectedCoCdItem.RLSE_FLG_KNT = 0;
            //        //SelectMstDetail();

            //        string _coCd = SelectedCoCdItem.SL_CO_CD + "_" + SelectedCoCdItem.SL_CO_NM;
            //        SelectCoCdDetail();

            //        for (int x = 0; x < SelectCoCdList.Count; x++)
            //        {
            //            if (_coCd.Equals(SelectCoCdList[x].SL_CO_CD + "_" + SelectCoCdList[x].SL_CO_NM))
            //            {
            //                if (_coCd.Equals(SelectCoCdList[x].SL_CO_CD + "_" + SelectCoCdList[x].SL_CO_NM))
            //                {
            //                    SelectedCoCdItem = SelectCoCdList[x];
            //                    return;
            //                }
            //            }
            //        }
            //    }
            //}
            //catch (System.Exception eLog)
            //{
            //    //DXSplashScreen.Close();
            //    WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
            //    return;
            //}
        }





        //마감
        public ICommand OkDialogCommand
        {
            get
            {
                if (_okDialogCommand == null)
                    _okDialogCommand = new DelegateCommand(OkContact);
                return _okDialogCommand;
            }
        }

        public async void OkContact()
        {
            try
            {
                bool isChk = false;

                IEnumerable<SaleVo> checkList = SelectMstList.Where<SaleVo>(x => x.isCheckd == true).ToList<SaleVo>();
                if (checkList.Count<SaleVo>() > 0)
                {
                    isChk = true;
                }

                if (isChk == true)
                {
                    if(checkList.Any<SaleVo>(x => x.CLZ_FLG.Equals("Y")))
                    {
                        WinUIMessageBox.Show("체크 항목 중 이미 마감 처리 포함 되었습니다", "[마감]" + title, MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }


                    MessageBoxResult result = WinUIMessageBox.Show("정말로 마감 하시겠습니까?", "[마감]" + title, MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {

                        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s2212/bill", new StringContent(JsonConvert.SerializeObject(checkList), System.Text.Encoding.UTF8, "application/json")))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                int? nResult = JsonConvert.DeserializeObject<int?>(await response.Content.ReadAsStringAsync());
                                WinUIMessageBox.Show("마감 되었습니다", "[마감]" + title, MessageBoxButton.OK, MessageBoxImage.Information);
                            }

                            Refresh();
                        }

                        //
                    
                            //JobVo resultVo;
                            //for (int x = 0; x < SelectMstList.Count; x++)
                            //{
                            //    if (SelectMstList[x].isCheckd)
                            //    {
                            //        resultVo = saleOrderClient.ProcS2212(new JobVo() { RLSE_CMD_NO = SelectMstList[x].RLSE_CMD_NO, RLSE_CMD_SEQ = SelectMstList[x].RLSE_CMD_SEQ, CRE_USR_ID = SystemProperties.USER, UPD_USR_ID = SystemProperties.USER });
                            //        if (!resultVo.isSuccess)
                            //        {
                            //            //실패
                            //            DXSplashScreen.Close();
                            //            WinUIMessageBox.Show(resultVo.Message, "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
                            //            return;
                            //        }
                            //        SelectMstList[x].CLZ_FLG = "Y";
                            //    }
                            //}
                    }
                }
                else
                {
                    WinUIMessageBox.Show("체크 된 데이터가 존재 하지 않습니다", "[마감]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }
            catch (Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }


        [Command]
        public async void ReportContact()
        {
            if (SelectedMstItem == null) { return; }
            else if (SelectedCoCdItem == null) { return; }


            SelectedCoCdItem.CRE_USR_ID = SystemProperties.USER;
            SelectedCoCdItem.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;

            IList<SaleVo> allItems;
            IList<SaleVo> reportItems = new List<SaleVo>();
            //IList<SpecificationOnTransVo> allItems = new List<SpecificationOnTransVo>(saleOrderClient.S221SelectMstSpecificationOnTransReportList(new JobVo() { RLSE_CMD_NO = SelectedMstItem.RLSE_CMD_NO, RLSE_CMD_SEQ = SelectedMstItem.RLSE_CMD_SEQ, SL_CO_CD = SelectedCoCdItem.SL_CO_CD, CRE_USR_ID = SystemProperties.USER }));
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s2212/report", new StringContent(JsonConvert.SerializeObject(new SaleVo() { RLSE_CMD_NO = SelectedMstItem.RLSE_CMD_NO, RLSE_CMD_SEQ = SelectedMstItem.RLSE_CMD_SEQ, SL_CO_CD = SelectedCoCdItem.SL_CO_CD, CRE_USR_ID = SystemProperties.USER, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    allItems = JsonConvert.DeserializeObject<IEnumerable<SaleVo>>(await response.Content.ReadAsStringAsync()).Cast<SaleVo>().ToList();

                    //string PRN_FLG = saleOrderClient.S2213SelectCheck(SelectedCoCdItem).PRN_FLG;
                    if (allItems == null)
                    {
                        return;
                    }
                    else if (allItems.Count <= 0)
                    {
                        WinUIMessageBox.Show("데이터가 존재 하지 않습니다.", "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    else if (allItems.Count <= 10)
                    {
                        reportItems.Add(new SaleVo());
                        for (int x = 0; x < allItems.Count; x++)
                        {
                            if (x == 0)
                            {
                                reportItems[0].A1 = allItems[x].RN;
                                reportItems[0].A2 = allItems[x].ITM_NM;
                                reportItems[0].A3 = allItems[x].ITM_SZ_NM;
                                reportItems[0].A4 = allItems[x].SL_ITM_QTY;
                                reportItems[0].A5 = allItems[x].SL_ITM_PRC_DC_VAL;
                                reportItems[0].A6 = allItems[x].SL_ITM_AMT;
                                reportItems[0].A7 = allItems[x].SL_ITM_TAX_AMT;
                                reportItems[0].A8 = allItems[x].MD_QTY;
                                reportItems[0].A9 = allItems[x].N1ST_ITM_GRP_NM;

                                //
                                reportItems[0].O1 = reportItems[0].A6;
                                reportItems[0].O2 = reportItems[0].A7;
                                reportItems[0].O3 = float.Parse(reportItems[0].O1.ToString()) + float.Parse(reportItems[0].O2.ToString());
                                //
                                reportItems[0].B_CO_RGST_NO = allItems[x].B_CO_RGST_NO;
                                reportItems[0].B_CO_NM = allItems[x].B_CO_NM;
                                reportItems[0].B_PRSD_NM = allItems[x].B_PRSD_NM;
                                reportItems[0].B_HDQTR_ADDR = allItems[x].B_HDQTR_ADDR;
                                reportItems[0].B_N1ST_BIZ_COND_NM = allItems[x].B_N1ST_BIZ_COND_NM;
                                reportItems[0].B_N1ST_BZTP_NM = allItems[x].B_N1ST_BZTP_NM;
                                reportItems[0].B_HDQTR_PHN_NO = allItems[x].B_HDQTR_PHN_NO;
                                reportItems[0].B_HDQTR_FAX_NO = allItems[x].B_HDQTR_FAX_NO;
                                //
                                reportItems[0].C_CO_RGST_NO = allItems[x].C_CO_RGST_NO;
                                reportItems[0].C_CO_NM = allItems[x].C_CO_NM;
                                reportItems[0].C_PRSD_NM = allItems[x].C_PRSD_NM;
                                reportItems[0].C_HDQTR_ADDR = allItems[x].C_HDQTR_ADDR;
                                reportItems[0].C_N1ST_BIZ_COND_NM = allItems[x].C_N1ST_BIZ_COND_NM;
                                reportItems[0].C_N1ST_BZTP_NM = allItems[x].C_N1ST_BZTP_NM;
                                reportItems[0].C_HDQTR_PHN_NO = allItems[x].C_HDQTR_PHN_NO;
                                reportItems[0].C_HDQTR_FAX_NO = allItems[x].C_HDQTR_FAX_NO;

                                reportItems[0].SL_BIL_RMK = "비 고 : " + allItems[x].SL_BIL_RMK;
                                reportItems[0].SL_BIL_NO = allItems[x].SL_BIL_NO;
                                reportItems[0].PRN_DT = "출력일시 : " + allItems[x].PRN_DT;
                                reportItems[0].CRE_USR_NM = allItems[x].CRE_USR_NM;

                                reportItems[0].SL_NXT_CLZ_FLG = allItems[x].SL_NXT_CLZ_FLG;
                                reportItems[0].NXT_MON_DT = "이월일자 : " + allItems[x].NXT_MON_DT;

                                reportItems[0].UPD_USR_NM = SystemProperties.USER_VO.USR_N1ST_NM;
                            }
                            else if (x == 1)
                            {
                                reportItems[0].B1 = allItems[x].RN;
                                reportItems[0].B2 = allItems[x].ITM_NM;
                                reportItems[0].B3 = allItems[x].ITM_SZ_NM;
                                reportItems[0].B4 = allItems[x].SL_ITM_QTY;
                                reportItems[0].B5 = allItems[x].SL_ITM_PRC_DC_VAL;
                                reportItems[0].B6 = allItems[x].SL_ITM_AMT;
                                reportItems[0].B7 = allItems[x].SL_ITM_TAX_AMT;
                                reportItems[0].B8 = allItems[x].MD_QTY;
                                reportItems[0].B9 = allItems[x].N1ST_ITM_GRP_NM;

                                ////
                                reportItems[0].O1 = (string.IsNullOrEmpty(reportItems[0].A6.ToString()) ? 0 : float.Parse(reportItems[0].A6.ToString()))
                                              + (string.IsNullOrEmpty(reportItems[0].B6.ToString()) ? 0 : float.Parse(reportItems[0].B6.ToString()));

                                reportItems[0].O2 = (string.IsNullOrEmpty(reportItems[0].A7.ToString()) ? 0 : float.Parse(reportItems[0].A7.ToString()))
                                                  + (string.IsNullOrEmpty(reportItems[0].B7.ToString()) ? 0 : float.Parse(reportItems[0].B7.ToString()));

                                reportItems[0].O3 = (string.IsNullOrEmpty(reportItems[0].O1.ToString()) ? 0 : float.Parse(reportItems[0].O1.ToString())) + (string.IsNullOrEmpty(reportItems[0].O2.ToString()) ? 0 : float.Parse(reportItems[0].O2.ToString()));

                            }
                            else if (x == 2)
                            {
                                reportItems[0].C1 = allItems[x].RN;
                                reportItems[0].C2 = allItems[x].ITM_NM;
                                reportItems[0].C3 = allItems[x].ITM_SZ_NM;
                                reportItems[0].C4 = allItems[x].SL_ITM_QTY;
                                reportItems[0].C5 = allItems[x].SL_ITM_PRC_DC_VAL;
                                reportItems[0].C6 = allItems[x].SL_ITM_AMT;
                                reportItems[0].C7 = allItems[x].SL_ITM_TAX_AMT;
                                reportItems[0].C8 = allItems[x].MD_QTY;
                                reportItems[0].C9 = allItems[x].N1ST_ITM_GRP_NM;
                                ////
                                reportItems[0].O1 = (string.IsNullOrEmpty(reportItems[0].A6.ToString()) ? 0 : float.Parse(reportItems[0].A6.ToString()))
                                              + (string.IsNullOrEmpty(reportItems[0].B6.ToString()) ? 0 : float.Parse(reportItems[0].B6.ToString()))
                                              + (string.IsNullOrEmpty(reportItems[0].C6.ToString()) ? 0 : float.Parse(reportItems[0].C6.ToString()));

                                reportItems[0].O2 = (string.IsNullOrEmpty(reportItems[0].A7.ToString()) ? 0 : float.Parse(reportItems[0].A7.ToString()))
                                                  + (string.IsNullOrEmpty(reportItems[0].B7.ToString()) ? 0 : float.Parse(reportItems[0].B7.ToString()))
                                                  + (string.IsNullOrEmpty(reportItems[0].C7.ToString()) ? 0 : float.Parse(reportItems[0].C7.ToString()));

                                reportItems[0].O3 = (string.IsNullOrEmpty(reportItems[0].O1.ToString()) ? 0 : float.Parse(reportItems[0].O1.ToString())) + (string.IsNullOrEmpty(reportItems[0].O2.ToString()) ? 0 : float.Parse(reportItems[0].O2.ToString()));

                            }
                            else if (x == 3)
                            {
                                reportItems[0].D1 = allItems[x].RN;
                                reportItems[0].D2 = allItems[x].ITM_NM;
                                reportItems[0].D3 = allItems[x].ITM_SZ_NM;
                                reportItems[0].D4 = allItems[x].SL_ITM_QTY;
                                reportItems[0].D5 = allItems[x].SL_ITM_PRC_DC_VAL;
                                reportItems[0].D6 = allItems[x].SL_ITM_AMT;
                                reportItems[0].D7 = allItems[x].SL_ITM_TAX_AMT;
                                reportItems[0].D8 = allItems[x].MD_QTY;
                                reportItems[0].D9 = allItems[x].N1ST_ITM_GRP_NM;
                                ////
                                reportItems[0].O1 = (string.IsNullOrEmpty(reportItems[0].A6.ToString()) ? 0 : float.Parse(reportItems[0].A6.ToString()))
                                              + (string.IsNullOrEmpty(reportItems[0].B6.ToString()) ? 0 : float.Parse(reportItems[0].B6.ToString()))
                                              + (string.IsNullOrEmpty(reportItems[0].C6.ToString()) ? 0 : float.Parse(reportItems[0].C6.ToString()))
                                              + (string.IsNullOrEmpty(reportItems[0].D6.ToString()) ? 0 : float.Parse(reportItems[0].D6.ToString()));

                                reportItems[0].O2 = (string.IsNullOrEmpty(reportItems[0].A7.ToString()) ? 0 : float.Parse(reportItems[0].A7.ToString()))
                                                  + (string.IsNullOrEmpty(reportItems[0].B7.ToString()) ? 0 : float.Parse(reportItems[0].B7.ToString()))
                                                  + (string.IsNullOrEmpty(reportItems[0].C7.ToString()) ? 0 : float.Parse(reportItems[0].C7.ToString()))
                                                  + (string.IsNullOrEmpty(reportItems[0].D7.ToString()) ? 0 : float.Parse(reportItems[0].D7.ToString()));

                                reportItems[0].O3 = (string.IsNullOrEmpty(reportItems[0].O1.ToString()) ? 0 : float.Parse(reportItems[0].O1.ToString())) + (string.IsNullOrEmpty(reportItems[0].O2.ToString()) ? 0 : float.Parse(reportItems[0].O2.ToString()));

                            }
                            else if (x == 4)
                            {
                                reportItems[0].E1 = allItems[x].RN;
                                reportItems[0].E2 = allItems[x].ITM_NM;
                                reportItems[0].E3 = allItems[x].ITM_SZ_NM;
                                reportItems[0].E4 = allItems[x].SL_ITM_QTY;
                                reportItems[0].E5 = allItems[x].SL_ITM_PRC_DC_VAL;
                                reportItems[0].E6 = allItems[x].SL_ITM_AMT;
                                reportItems[0].E7 = allItems[x].SL_ITM_TAX_AMT;
                                reportItems[0].E8 = allItems[x].MD_QTY;
                                reportItems[0].E9 = allItems[x].N1ST_ITM_GRP_NM;
                                ////
                                reportItems[0].O1 = (string.IsNullOrEmpty(reportItems[0].A6.ToString()) ? 0 : float.Parse(reportItems[0].A6.ToString()))
                                              + (string.IsNullOrEmpty(reportItems[0].B6.ToString()) ? 0 : float.Parse(reportItems[0].B6.ToString()))
                                              + (string.IsNullOrEmpty(reportItems[0].C6.ToString()) ? 0 : float.Parse(reportItems[0].C6.ToString()))
                                              + (string.IsNullOrEmpty(reportItems[0].D6.ToString()) ? 0 : float.Parse(reportItems[0].D6.ToString()))
                                              + (string.IsNullOrEmpty(reportItems[0].E6.ToString()) ? 0 : float.Parse(reportItems[0].E6.ToString()));

                                reportItems[0].O2 = (string.IsNullOrEmpty(reportItems[0].A7.ToString()) ? 0 : float.Parse(reportItems[0].A7.ToString()))
                                                  + (string.IsNullOrEmpty(reportItems[0].B7.ToString()) ? 0 : float.Parse(reportItems[0].B7.ToString()))
                                                  + (string.IsNullOrEmpty(reportItems[0].C7.ToString()) ? 0 : float.Parse(reportItems[0].C7.ToString()))
                                                  + (string.IsNullOrEmpty(reportItems[0].D7.ToString()) ? 0 : float.Parse(reportItems[0].D7.ToString()))
                                                  + (string.IsNullOrEmpty(reportItems[0].E7.ToString()) ? 0 : float.Parse(reportItems[0].E7.ToString()));

                                reportItems[0].O3 = (string.IsNullOrEmpty(reportItems[0].O1.ToString()) ? 0 : float.Parse(reportItems[0].O1.ToString())) + (string.IsNullOrEmpty(reportItems[0].O2.ToString()) ? 0 : float.Parse(reportItems[0].O2.ToString()));

                            }
                            else if (x == 5)
                            {
                                reportItems[0].F1 = allItems[x].RN;
                                reportItems[0].F2 = allItems[x].ITM_NM;
                                reportItems[0].F3 = allItems[x].ITM_SZ_NM;
                                reportItems[0].F4 = allItems[x].SL_ITM_QTY;
                                reportItems[0].F5 = allItems[x].SL_ITM_PRC_DC_VAL;
                                reportItems[0].F6 = allItems[x].SL_ITM_AMT;
                                reportItems[0].F7 = allItems[x].SL_ITM_TAX_AMT;
                                reportItems[0].F8 = allItems[x].MD_QTY;
                                reportItems[0].F9 = allItems[x].N1ST_ITM_GRP_NM;
                                ////
                                reportItems[0].O1 = (string.IsNullOrEmpty(reportItems[0].A6.ToString()) ? 0 : float.Parse(reportItems[0].A6.ToString()))
                                              + (string.IsNullOrEmpty(reportItems[0].B6.ToString()) ? 0 : float.Parse(reportItems[0].B6.ToString()))
                                              + (string.IsNullOrEmpty(reportItems[0].C6.ToString()) ? 0 : float.Parse(reportItems[0].C6.ToString()))
                                              + (string.IsNullOrEmpty(reportItems[0].D6.ToString()) ? 0 : float.Parse(reportItems[0].D6.ToString()))
                                              + (string.IsNullOrEmpty(reportItems[0].E6.ToString()) ? 0 : float.Parse(reportItems[0].E6.ToString()))
                                              + (string.IsNullOrEmpty(reportItems[0].F6.ToString()) ? 0 : float.Parse(reportItems[0].F6.ToString()));

                                reportItems[0].O2 = (string.IsNullOrEmpty(reportItems[0].A7.ToString()) ? 0 : float.Parse(reportItems[0].A7.ToString()))
                                                  + (string.IsNullOrEmpty(reportItems[0].B7.ToString()) ? 0 : float.Parse(reportItems[0].B7.ToString()))
                                                  + (string.IsNullOrEmpty(reportItems[0].C7.ToString()) ? 0 : float.Parse(reportItems[0].C7.ToString()))
                                                  + (string.IsNullOrEmpty(reportItems[0].D7.ToString()) ? 0 : float.Parse(reportItems[0].D7.ToString()))
                                                  + (string.IsNullOrEmpty(reportItems[0].E7.ToString()) ? 0 : float.Parse(reportItems[0].E7.ToString()))
                                                  + (string.IsNullOrEmpty(reportItems[0].F7.ToString()) ? 0 : float.Parse(reportItems[0].F7.ToString()));

                                reportItems[0].O3 = (string.IsNullOrEmpty(reportItems[0].O1.ToString()) ? 0 : float.Parse(reportItems[0].O1.ToString())) + (string.IsNullOrEmpty(reportItems[0].O2.ToString()) ? 0 : float.Parse(reportItems[0].O2.ToString()));

                            }
                            else if (x == 6)
                            {
                                reportItems[0].G1 = allItems[x].RN;
                                reportItems[0].G2 = allItems[x].ITM_NM;
                                reportItems[0].G3 = allItems[x].ITM_SZ_NM;
                                reportItems[0].G4 = allItems[x].SL_ITM_QTY;
                                reportItems[0].G5 = allItems[x].SL_ITM_PRC_DC_VAL;
                                reportItems[0].G6 = allItems[x].SL_ITM_AMT;
                                reportItems[0].G7 = allItems[x].SL_ITM_TAX_AMT;
                                reportItems[0].G8 = allItems[x].MD_QTY;
                                reportItems[0].G9 = allItems[x].N1ST_ITM_GRP_NM;
                                ////
                                reportItems[0].O1 = (string.IsNullOrEmpty(reportItems[0].A6.ToString()) ? 0 : float.Parse(reportItems[0].A6.ToString()))
                                              + (string.IsNullOrEmpty(reportItems[0].B6.ToString()) ? 0 : float.Parse(reportItems[0].B6.ToString()))
                                              + (string.IsNullOrEmpty(reportItems[0].C6.ToString()) ? 0 : float.Parse(reportItems[0].C6.ToString()))
                                              + (string.IsNullOrEmpty(reportItems[0].D6.ToString()) ? 0 : float.Parse(reportItems[0].D6.ToString()))
                                              + (string.IsNullOrEmpty(reportItems[0].E6.ToString()) ? 0 : float.Parse(reportItems[0].E6.ToString()))
                                              + (string.IsNullOrEmpty(reportItems[0].F6.ToString()) ? 0 : float.Parse(reportItems[0].F6.ToString()))
                                              + (string.IsNullOrEmpty(reportItems[0].G6.ToString()) ? 0 : float.Parse(reportItems[0].G6.ToString()));

                                reportItems[0].O2 = (string.IsNullOrEmpty(reportItems[0].A7.ToString()) ? 0 : float.Parse(reportItems[0].A7.ToString()))
                                                  + (string.IsNullOrEmpty(reportItems[0].B7.ToString()) ? 0 : float.Parse(reportItems[0].B7.ToString()))
                                                  + (string.IsNullOrEmpty(reportItems[0].C7.ToString()) ? 0 : float.Parse(reportItems[0].C7.ToString()))
                                                  + (string.IsNullOrEmpty(reportItems[0].D7.ToString()) ? 0 : float.Parse(reportItems[0].D7.ToString()))
                                                  + (string.IsNullOrEmpty(reportItems[0].E7.ToString()) ? 0 : float.Parse(reportItems[0].E7.ToString()))
                                                  + (string.IsNullOrEmpty(reportItems[0].F7.ToString()) ? 0 : float.Parse(reportItems[0].F7.ToString()))
                                                  + (string.IsNullOrEmpty(reportItems[0].G7.ToString()) ? 0 : float.Parse(reportItems[0].G7.ToString()));

                                reportItems[0].O3 = (string.IsNullOrEmpty(reportItems[0].O1.ToString()) ? 0 : float.Parse(reportItems[0].O1.ToString())) + (string.IsNullOrEmpty(reportItems[0].O2.ToString()) ? 0 : float.Parse(reportItems[0].O2.ToString()));

                            }
                            else if (x == 7)
                            {
                                reportItems[0].H1 = allItems[x].RN;
                                reportItems[0].H2 = allItems[x].ITM_NM;
                                reportItems[0].H3 = allItems[x].ITM_SZ_NM;
                                reportItems[0].H4 = allItems[x].SL_ITM_QTY;
                                reportItems[0].H5 = allItems[x].SL_ITM_PRC_DC_VAL;
                                reportItems[0].H6 = allItems[x].SL_ITM_AMT;
                                reportItems[0].H7 = allItems[x].SL_ITM_TAX_AMT;
                                reportItems[0].H8 = allItems[x].MD_QTY;
                                reportItems[0].H9 = allItems[x].N1ST_ITM_GRP_NM;
                                ////
                                reportItems[0].O1 = (string.IsNullOrEmpty(reportItems[0].A6.ToString()) ? 0 : float.Parse(reportItems[0].A6.ToString()))
                                              + (string.IsNullOrEmpty(reportItems[0].B6.ToString()) ? 0 : float.Parse(reportItems[0].B6.ToString()))
                                              + (string.IsNullOrEmpty(reportItems[0].C6.ToString()) ? 0 : float.Parse(reportItems[0].C6.ToString()))
                                              + (string.IsNullOrEmpty(reportItems[0].D6.ToString()) ? 0 : float.Parse(reportItems[0].D6.ToString()))
                                              + (string.IsNullOrEmpty(reportItems[0].E6.ToString()) ? 0 : float.Parse(reportItems[0].E6.ToString()))
                                              + (string.IsNullOrEmpty(reportItems[0].F6.ToString()) ? 0 : float.Parse(reportItems[0].F6.ToString()))
                                              + (string.IsNullOrEmpty(reportItems[0].G6.ToString()) ? 0 : float.Parse(reportItems[0].G6.ToString()))
                                              + (string.IsNullOrEmpty(reportItems[0].H6.ToString()) ? 0 : float.Parse(reportItems[0].H6.ToString()));


                                reportItems[0].O2 = (string.IsNullOrEmpty(reportItems[0].A7.ToString()) ? 0 : float.Parse(reportItems[0].A7.ToString()))
                                                  + (string.IsNullOrEmpty(reportItems[0].B7.ToString()) ? 0 : float.Parse(reportItems[0].B7.ToString()))
                                                  + (string.IsNullOrEmpty(reportItems[0].C7.ToString()) ? 0 : float.Parse(reportItems[0].C7.ToString()))
                                                  + (string.IsNullOrEmpty(reportItems[0].D7.ToString()) ? 0 : float.Parse(reportItems[0].D7.ToString()))
                                                  + (string.IsNullOrEmpty(reportItems[0].E7.ToString()) ? 0 : float.Parse(reportItems[0].E7.ToString()))
                                                  + (string.IsNullOrEmpty(reportItems[0].F7.ToString()) ? 0 : float.Parse(reportItems[0].F7.ToString()))
                                                  + (string.IsNullOrEmpty(reportItems[0].G7.ToString()) ? 0 : float.Parse(reportItems[0].G7.ToString()))
                                                  + (string.IsNullOrEmpty(reportItems[0].H7.ToString()) ? 0 : float.Parse(reportItems[0].H7.ToString()));

                                reportItems[0].O3 = (string.IsNullOrEmpty(reportItems[0].O1.ToString()) ? 0 : float.Parse(reportItems[0].O1.ToString())) + (string.IsNullOrEmpty(reportItems[0].O2.ToString()) ? 0 : float.Parse(reportItems[0].O2.ToString()));

                            }
                            else if (x == 8)
                            {
                                reportItems[0].I1 = allItems[x].RN;
                                reportItems[0].I2 = allItems[x].ITM_NM;
                                reportItems[0].I3 = allItems[x].ITM_SZ_NM;
                                reportItems[0].I4 = allItems[x].SL_ITM_QTY;
                                reportItems[0].I5 = allItems[x].SL_ITM_PRC_DC_VAL;
                                reportItems[0].I6 = allItems[x].SL_ITM_AMT;
                                reportItems[0].I7 = allItems[x].SL_ITM_TAX_AMT;
                                reportItems[0].I8 = allItems[x].MD_QTY;
                                reportItems[0].I9 = allItems[x].N1ST_ITM_GRP_NM;
                                ////
                                reportItems[0].O1 = (string.IsNullOrEmpty(reportItems[0].A6.ToString()) ? 0 : float.Parse(reportItems[0].A6.ToString()))
                                              + (string.IsNullOrEmpty(reportItems[0].B6.ToString()) ? 0 : float.Parse(reportItems[0].B6.ToString()))
                                              + (string.IsNullOrEmpty(reportItems[0].C6.ToString()) ? 0 : float.Parse(reportItems[0].C6.ToString()))
                                              + (string.IsNullOrEmpty(reportItems[0].D6.ToString()) ? 0 : float.Parse(reportItems[0].D6.ToString()))
                                              + (string.IsNullOrEmpty(reportItems[0].E6.ToString()) ? 0 : float.Parse(reportItems[0].E6.ToString()))
                                              + (string.IsNullOrEmpty(reportItems[0].F6.ToString()) ? 0 : float.Parse(reportItems[0].F6.ToString()))
                                              + (string.IsNullOrEmpty(reportItems[0].G6.ToString()) ? 0 : float.Parse(reportItems[0].G6.ToString()))
                                              + (string.IsNullOrEmpty(reportItems[0].H6.ToString()) ? 0 : float.Parse(reportItems[0].H6.ToString()))
                                              + (string.IsNullOrEmpty(reportItems[0].I6.ToString()) ? 0 : float.Parse(reportItems[0].I6.ToString()));


                                reportItems[0].O2 = (string.IsNullOrEmpty(reportItems[0].A7.ToString()) ? 0 : float.Parse(reportItems[0].A7.ToString()))
                                                  + (string.IsNullOrEmpty(reportItems[0].B7.ToString()) ? 0 : float.Parse(reportItems[0].B7.ToString()))
                                                  + (string.IsNullOrEmpty(reportItems[0].C7.ToString()) ? 0 : float.Parse(reportItems[0].C7.ToString()))
                                                  + (string.IsNullOrEmpty(reportItems[0].D7.ToString()) ? 0 : float.Parse(reportItems[0].D7.ToString()))
                                                  + (string.IsNullOrEmpty(reportItems[0].E7.ToString()) ? 0 : float.Parse(reportItems[0].E7.ToString()))
                                                  + (string.IsNullOrEmpty(reportItems[0].F7.ToString()) ? 0 : float.Parse(reportItems[0].F7.ToString()))
                                                  + (string.IsNullOrEmpty(reportItems[0].G7.ToString()) ? 0 : float.Parse(reportItems[0].G7.ToString()))
                                                  + (string.IsNullOrEmpty(reportItems[0].H7.ToString()) ? 0 : float.Parse(reportItems[0].H7.ToString()))
                                                  + (string.IsNullOrEmpty(reportItems[0].I7.ToString()) ? 0 : float.Parse(reportItems[0].I7.ToString()));

                                reportItems[0].O3 = (string.IsNullOrEmpty(reportItems[0].O1.ToString()) ? 0 : float.Parse(reportItems[0].O1.ToString())) + (string.IsNullOrEmpty(reportItems[0].O2.ToString()) ? 0 : float.Parse(reportItems[0].O2.ToString()));

                            }
                            else if (x == 9)
                            {
                                reportItems[0].J1 = allItems[x].RN;
                                reportItems[0].J2 = allItems[x].ITM_NM;
                                reportItems[0].J3 = allItems[x].ITM_SZ_NM;
                                reportItems[0].J4 = allItems[x].SL_ITM_QTY;
                                reportItems[0].J5 = allItems[x].SL_ITM_PRC_DC_VAL;
                                reportItems[0].J6 = allItems[x].SL_ITM_AMT;
                                reportItems[0].J7 = allItems[x].SL_ITM_TAX_AMT;
                                reportItems[0].J8 = allItems[x].MD_QTY;
                                reportItems[0].J9 = allItems[x].N1ST_ITM_GRP_NM;
                                ////
                                reportItems[0].O1 = (string.IsNullOrEmpty(reportItems[0].A6.ToString()) ? 0 : float.Parse(reportItems[0].A6.ToString()))
                                              + (string.IsNullOrEmpty(reportItems[0].B6.ToString()) ? 0 : float.Parse(reportItems[0].B6.ToString()))
                                              + (string.IsNullOrEmpty(reportItems[0].C6.ToString()) ? 0 : float.Parse(reportItems[0].C6.ToString()))
                                              + (string.IsNullOrEmpty(reportItems[0].D6.ToString()) ? 0 : float.Parse(reportItems[0].D6.ToString()))
                                              + (string.IsNullOrEmpty(reportItems[0].E6.ToString()) ? 0 : float.Parse(reportItems[0].E6.ToString()))
                                              + (string.IsNullOrEmpty(reportItems[0].F6.ToString()) ? 0 : float.Parse(reportItems[0].F6.ToString()))
                                              + (string.IsNullOrEmpty(reportItems[0].G6.ToString()) ? 0 : float.Parse(reportItems[0].G6.ToString()))
                                              + (string.IsNullOrEmpty(reportItems[0].H6.ToString()) ? 0 : float.Parse(reportItems[0].H6.ToString()))
                                              + (string.IsNullOrEmpty(reportItems[0].I6.ToString()) ? 0 : float.Parse(reportItems[0].I6.ToString()))
                                              + (string.IsNullOrEmpty(reportItems[0].J6.ToString()) ? 0 : float.Parse(reportItems[0].J6.ToString()));


                                reportItems[0].O2 = (string.IsNullOrEmpty(reportItems[0].A7.ToString()) ? 0 : float.Parse(reportItems[0].A7.ToString()))
                                                  + (string.IsNullOrEmpty(reportItems[0].B7.ToString()) ? 0 : float.Parse(reportItems[0].B7.ToString()))
                                                  + (string.IsNullOrEmpty(reportItems[0].C7.ToString()) ? 0 : float.Parse(reportItems[0].C7.ToString()))
                                                  + (string.IsNullOrEmpty(reportItems[0].D7.ToString()) ? 0 : float.Parse(reportItems[0].D7.ToString()))
                                                  + (string.IsNullOrEmpty(reportItems[0].E7.ToString()) ? 0 : float.Parse(reportItems[0].E7.ToString()))
                                                  + (string.IsNullOrEmpty(reportItems[0].F7.ToString()) ? 0 : float.Parse(reportItems[0].F7.ToString()))
                                                  + (string.IsNullOrEmpty(reportItems[0].G7.ToString()) ? 0 : float.Parse(reportItems[0].G7.ToString()))
                                                  + (string.IsNullOrEmpty(reportItems[0].H7.ToString()) ? 0 : float.Parse(reportItems[0].H7.ToString()))
                                                  + (string.IsNullOrEmpty(reportItems[0].I7.ToString()) ? 0 : float.Parse(reportItems[0].I7.ToString()))
                                                  + (string.IsNullOrEmpty(reportItems[0].J7.ToString()) ? 0 : float.Parse(reportItems[0].J7.ToString()));

                                reportItems[0].O3 = (string.IsNullOrEmpty(reportItems[0].O1.ToString()) ? 0 : float.Parse(reportItems[0].O1.ToString())) + (string.IsNullOrEmpty(reportItems[0].O2.ToString()) ? 0 : float.Parse(reportItems[0].O2.ToString()));

                            }
                        }
                    }
                    else
                    {
                        //Page 나누기
                        int nTotal = allItems.Count;
                        int nPage = nTotal / 10;
                        int mod = ((int)nTotal % 10);


                        int min = 0;
                        int max = 0;

                        for (int z = 0; z < nPage; z++)
                        {
                            min = z * 10;
                            max = min + 10;

                            reportItems.Insert(z, new SaleVo());
                            for (int x = min; x < max; x++)
                            {
                                //allItems[z].PAGE_NUM =  (z + 1) + "/" + nPage;
                                if ((x % 10) == 0)
                                {
                                    reportItems[z].A1 = allItems[x].RN;
                                    reportItems[z].A2 = allItems[x].ITM_NM;
                                    reportItems[z].A3 = allItems[x].ITM_SZ_NM;
                                    reportItems[z].A4 = allItems[x].SL_ITM_QTY;
                                    reportItems[z].A5 = allItems[x].SL_ITM_PRC_DC_VAL;
                                    reportItems[z].A6 = allItems[x].SL_ITM_AMT;
                                    reportItems[z].A7 = allItems[x].SL_ITM_TAX_AMT;
                                    reportItems[z].A8 = allItems[x].MD_QTY;
                                    reportItems[z].A9 = allItems[x].N1ST_ITM_GRP_NM;
                                    //
                                    reportItems[z].O1 = reportItems[z].A6;
                                    reportItems[z].O2 = reportItems[z].A7;
                                    reportItems[z].O3 = float.Parse(reportItems[z].O1.ToString()) + float.Parse(reportItems[z].O2.ToString());
                                    //
                                    reportItems[z].B_CO_RGST_NO = allItems[x].B_CO_RGST_NO;
                                    reportItems[z].B_CO_NM = allItems[x].B_CO_NM;
                                    reportItems[z].B_PRSD_NM = allItems[x].B_PRSD_NM;
                                    reportItems[z].B_HDQTR_ADDR = allItems[x].B_HDQTR_ADDR;
                                    reportItems[z].B_N1ST_BIZ_COND_NM = allItems[x].B_N1ST_BIZ_COND_NM;
                                    reportItems[z].B_N1ST_BZTP_NM = allItems[x].B_N1ST_BZTP_NM;
                                    reportItems[z].B_HDQTR_PHN_NO = allItems[x].B_HDQTR_PHN_NO;
                                    reportItems[z].B_HDQTR_FAX_NO = allItems[x].B_HDQTR_FAX_NO;
                                    //
                                    reportItems[z].C_CO_RGST_NO = allItems[x].C_CO_RGST_NO;
                                    reportItems[z].C_CO_NM = allItems[x].C_CO_NM;
                                    reportItems[z].C_PRSD_NM = allItems[x].C_PRSD_NM;
                                    reportItems[z].C_HDQTR_ADDR = allItems[x].C_HDQTR_ADDR;
                                    reportItems[z].C_N1ST_BIZ_COND_NM = allItems[x].C_N1ST_BIZ_COND_NM;
                                    reportItems[z].C_N1ST_BZTP_NM = allItems[x].C_N1ST_BZTP_NM;
                                    reportItems[z].C_HDQTR_PHN_NO = allItems[x].C_HDQTR_PHN_NO;
                                    reportItems[z].C_HDQTR_FAX_NO = allItems[x].C_HDQTR_FAX_NO;

                                    reportItems[z].SL_BIL_RMK = "비 고 : " + allItems[x].SL_BIL_RMK;
                                    reportItems[z].SL_BIL_NO = allItems[x].SL_BIL_NO;
                                    reportItems[z].PRN_DT = "출력일시 : " + allItems[x].PRN_DT;
                                    reportItems[z].CRE_USR_NM = allItems[x].CRE_USR_NM;

                                    reportItems[z].SL_NXT_CLZ_FLG = allItems[x].SL_NXT_CLZ_FLG;
                                    reportItems[z].NXT_MON_DT = "이월일자 : " + allItems[x].NXT_MON_DT;

                                    reportItems[z].UPD_USR_NM = SystemProperties.USER_VO.USR_N1ST_NM;
                                }
                                else if ((x % 10) == 1)
                                {
                                    reportItems[z].B1 = allItems[x].RN;
                                    reportItems[z].B2 = allItems[x].ITM_NM;
                                    reportItems[z].B3 = allItems[x].ITM_SZ_NM;
                                    reportItems[z].B4 = allItems[x].SL_ITM_QTY;
                                    reportItems[z].B5 = allItems[x].SL_ITM_PRC_DC_VAL;
                                    reportItems[z].B6 = allItems[x].SL_ITM_AMT;
                                    reportItems[z].B7 = allItems[x].SL_ITM_TAX_AMT;
                                    reportItems[z].B8 = allItems[x].MD_QTY;
                                    reportItems[z].B9 = allItems[x].N1ST_ITM_GRP_NM;
                                    //
                                    reportItems[z].O1 = float.Parse(reportItems[z].O1.ToString()) + float.Parse(reportItems[z].B6.ToString());
                                    reportItems[z].O2 = float.Parse(reportItems[z].O2.ToString()) + float.Parse(reportItems[z].B7.ToString());
                                    reportItems[z].O3 = float.Parse(reportItems[z].O1.ToString()) + float.Parse(reportItems[z].O2.ToString());
                                }
                                else if ((x % 10) == 2)
                                {
                                    reportItems[z].C1 = allItems[x].RN;
                                    reportItems[z].C2 = allItems[x].ITM_NM;
                                    reportItems[z].C3 = allItems[x].ITM_SZ_NM;
                                    reportItems[z].C4 = allItems[x].SL_ITM_QTY;
                                    reportItems[z].C5 = allItems[x].SL_ITM_PRC_DC_VAL;
                                    reportItems[z].C6 = allItems[x].SL_ITM_AMT;
                                    reportItems[z].C7 = allItems[x].SL_ITM_TAX_AMT;
                                    reportItems[z].C8 = allItems[x].MD_QTY;
                                    reportItems[z].C9 = allItems[x].N1ST_ITM_GRP_NM;
                                    //
                                    reportItems[z].O1 = float.Parse(reportItems[z].O1.ToString()) + float.Parse(reportItems[z].C6.ToString());
                                    reportItems[z].O2 = float.Parse(reportItems[z].O2.ToString()) + float.Parse(reportItems[z].C7.ToString());
                                    reportItems[z].O3 = float.Parse(reportItems[z].O1.ToString()) + float.Parse(reportItems[z].O2.ToString());
                                }
                                else if ((x % 10) == 3)
                                {
                                    reportItems[z].D1 = allItems[x].RN;
                                    reportItems[z].D2 = allItems[x].ITM_NM;
                                    reportItems[z].D3 = allItems[x].ITM_SZ_NM;
                                    reportItems[z].D4 = allItems[x].SL_ITM_QTY;
                                    reportItems[z].D5 = allItems[x].SL_ITM_PRC_DC_VAL;
                                    reportItems[z].D6 = allItems[x].SL_ITM_AMT;
                                    reportItems[z].D7 = allItems[x].SL_ITM_TAX_AMT;
                                    reportItems[z].D8 = allItems[x].MD_QTY;
                                    reportItems[z].D9 = allItems[x].N1ST_ITM_GRP_NM;
                                    //
                                    reportItems[z].O1 = float.Parse(reportItems[z].O1.ToString()) + float.Parse(reportItems[z].D6.ToString());
                                    reportItems[z].O2 = float.Parse(reportItems[z].O2.ToString()) + float.Parse(reportItems[z].D7.ToString());
                                    reportItems[z].O3 = float.Parse(reportItems[z].O1.ToString()) + float.Parse(reportItems[z].O2.ToString());
                                }
                                else if ((x % 10) == 4)
                                {
                                    reportItems[z].E1 = allItems[x].RN;
                                    reportItems[z].E2 = allItems[x].ITM_NM;
                                    reportItems[z].E3 = allItems[x].ITM_SZ_NM;
                                    reportItems[z].E4 = allItems[x].SL_ITM_QTY;
                                    reportItems[z].E5 = allItems[x].SL_ITM_PRC_DC_VAL;
                                    reportItems[z].E6 = allItems[x].SL_ITM_AMT;
                                    reportItems[z].E7 = allItems[x].SL_ITM_TAX_AMT;
                                    reportItems[z].E8 = allItems[x].MD_QTY;
                                    reportItems[z].E9 = allItems[x].N1ST_ITM_GRP_NM;
                                    //
                                    reportItems[z].O1 = float.Parse(reportItems[z].O1.ToString()) + float.Parse(reportItems[z].E6.ToString());
                                    reportItems[z].O2 = float.Parse(reportItems[z].O2.ToString()) + float.Parse(reportItems[z].E7.ToString());
                                    reportItems[z].O3 = float.Parse(reportItems[z].O1.ToString()) + float.Parse(reportItems[z].O2.ToString());
                                }
                                else if ((x % 10) == 5)
                                {
                                    reportItems[z].F1 = allItems[x].RN;
                                    reportItems[z].F2 = allItems[x].ITM_NM;
                                    reportItems[z].F3 = allItems[x].ITM_SZ_NM;
                                    reportItems[z].F4 = allItems[x].SL_ITM_QTY;
                                    reportItems[z].F5 = allItems[x].SL_ITM_PRC_DC_VAL;
                                    reportItems[z].F6 = allItems[x].SL_ITM_AMT;
                                    reportItems[z].F7 = allItems[x].SL_ITM_TAX_AMT;
                                    reportItems[z].F8 = allItems[x].MD_QTY;
                                    reportItems[z].F9 = allItems[x].N1ST_ITM_GRP_NM;
                                    //
                                    reportItems[z].O1 = float.Parse(reportItems[z].O1.ToString()) + float.Parse(reportItems[z].F6.ToString());
                                    reportItems[z].O2 = float.Parse(reportItems[z].O2.ToString()) + float.Parse(reportItems[z].F7.ToString());
                                    reportItems[z].O3 = float.Parse(reportItems[z].O1.ToString()) + float.Parse(reportItems[z].O2.ToString());
                                }
                                else if ((x % 10) == 6)
                                {
                                    reportItems[z].G1 = allItems[x].RN;
                                    reportItems[z].G2 = allItems[x].ITM_NM;
                                    reportItems[z].G3 = allItems[x].ITM_SZ_NM;
                                    reportItems[z].G4 = allItems[x].SL_ITM_QTY;
                                    reportItems[z].G5 = allItems[x].SL_ITM_PRC_DC_VAL;
                                    reportItems[z].G6 = allItems[x].SL_ITM_AMT;
                                    reportItems[z].G7 = allItems[x].SL_ITM_TAX_AMT;
                                    reportItems[z].G8 = allItems[x].MD_QTY;
                                    reportItems[z].G9 = allItems[x].N1ST_ITM_GRP_NM;
                                    //
                                    reportItems[z].O1 = float.Parse(reportItems[z].O1.ToString()) + float.Parse(reportItems[z].G6.ToString());
                                    reportItems[z].O2 = float.Parse(reportItems[z].O2.ToString()) + float.Parse(reportItems[z].G7.ToString());
                                    reportItems[z].O3 = float.Parse(reportItems[z].O1.ToString()) + float.Parse(reportItems[z].O2.ToString());
                                }
                                else if ((x % 10) == 7)
                                {
                                    reportItems[z].H1 = allItems[x].RN;
                                    reportItems[z].H2 = allItems[x].ITM_NM;
                                    reportItems[z].H3 = allItems[x].ITM_SZ_NM;
                                    reportItems[z].H4 = allItems[x].SL_ITM_QTY;
                                    reportItems[z].H5 = allItems[x].SL_ITM_PRC_DC_VAL;
                                    reportItems[z].H6 = allItems[x].SL_ITM_AMT;
                                    reportItems[z].H7 = allItems[x].SL_ITM_TAX_AMT;
                                    reportItems[z].H8 = allItems[x].MD_QTY;
                                    reportItems[z].H9 = allItems[x].N1ST_ITM_GRP_NM;
                                    //
                                    reportItems[z].O1 = float.Parse(reportItems[z].O1.ToString()) + float.Parse(reportItems[z].H6.ToString());
                                    reportItems[z].O2 = float.Parse(reportItems[z].O2.ToString()) + float.Parse(reportItems[z].H7.ToString());
                                    reportItems[z].O3 = float.Parse(reportItems[z].O1.ToString()) + float.Parse(reportItems[z].O2.ToString());
                                }
                                else if ((x % 10) == 8)
                                {
                                    reportItems[z].I1 = allItems[x].RN;
                                    reportItems[z].I2 = allItems[x].ITM_NM;
                                    reportItems[z].I3 = allItems[x].ITM_SZ_NM;
                                    reportItems[z].I4 = allItems[x].SL_ITM_QTY;
                                    reportItems[z].I5 = allItems[x].SL_ITM_PRC_DC_VAL;
                                    reportItems[z].I6 = allItems[x].SL_ITM_AMT;
                                    reportItems[z].I7 = allItems[x].SL_ITM_TAX_AMT;
                                    reportItems[z].I8 = allItems[x].MD_QTY;
                                    reportItems[z].I9 = allItems[x].N1ST_ITM_GRP_NM;
                                    //
                                    reportItems[z].O1 = float.Parse(reportItems[z].O1.ToString()) + float.Parse(reportItems[z].I6.ToString());
                                    reportItems[z].O2 = float.Parse(reportItems[z].O2.ToString()) + float.Parse(reportItems[z].I7.ToString());
                                    reportItems[z].O3 = float.Parse(reportItems[z].O1.ToString()) + float.Parse(reportItems[z].O2.ToString());
                                }
                                else if ((x % 10) == 9)
                                {
                                    reportItems[z].J1 = allItems[x].RN;
                                    reportItems[z].J2 = allItems[x].ITM_NM;
                                    reportItems[z].J3 = allItems[x].ITM_SZ_NM;
                                    reportItems[z].J4 = allItems[x].SL_ITM_QTY;
                                    reportItems[z].J5 = allItems[x].SL_ITM_PRC_DC_VAL;
                                    reportItems[z].J6 = allItems[x].SL_ITM_AMT;
                                    reportItems[z].J7 = allItems[x].SL_ITM_TAX_AMT;
                                    reportItems[z].J8 = allItems[x].MD_QTY;
                                    reportItems[z].J9 = allItems[x].N1ST_ITM_GRP_NM;
                                    //
                                    reportItems[z].O1 = float.Parse(reportItems[z].O1.ToString()) + float.Parse(reportItems[z].J6.ToString());
                                    reportItems[z].O2 = float.Parse(reportItems[z].O2.ToString()) + float.Parse(reportItems[z].J7.ToString());
                                    reportItems[z].O3 = float.Parse(reportItems[z].O1.ToString()) + float.Parse(reportItems[z].O2.ToString());
                                }
                            }
                        }

                        //나머지 계산
                        if (mod != 0)
                        {
                            min = nPage * 10;
                            reportItems.Insert(nPage, new SaleVo());
                            for (int x = min; x < allItems.Count; x++)
                            {
                                if ((x % 10) == 0)
                                {
                                    reportItems[nPage].A1 = allItems[x].RN;
                                    reportItems[nPage].A2 = allItems[x].ITM_NM;
                                    reportItems[nPage].A3 = allItems[x].ITM_SZ_NM;
                                    reportItems[nPage].A4 = allItems[x].SL_ITM_QTY;
                                    reportItems[nPage].A5 = allItems[x].SL_ITM_PRC_DC_VAL;
                                    reportItems[nPage].A6 = allItems[x].SL_ITM_AMT;
                                    reportItems[nPage].A7 = allItems[x].SL_ITM_TAX_AMT;
                                    reportItems[nPage].A8 = allItems[x].MD_QTY;
                                    reportItems[nPage].A9 = allItems[x].N1ST_ITM_GRP_NM;
                                    //
                                    //
                                    reportItems[nPage].O1 = reportItems[nPage].A6;
                                    reportItems[nPage].O2 = reportItems[nPage].A7;
                                    reportItems[nPage].O3 = float.Parse(reportItems[nPage].O1.ToString()) + float.Parse(reportItems[nPage].O2.ToString());
                                    //
                                    reportItems[nPage].B_CO_RGST_NO = allItems[x].B_CO_RGST_NO;
                                    reportItems[nPage].B_CO_NM = allItems[x].B_CO_NM;
                                    reportItems[nPage].B_PRSD_NM = allItems[x].B_PRSD_NM;
                                    reportItems[nPage].B_HDQTR_ADDR = allItems[x].B_HDQTR_ADDR;
                                    reportItems[nPage].B_N1ST_BIZ_COND_NM = allItems[x].B_N1ST_BIZ_COND_NM;
                                    reportItems[nPage].B_N1ST_BZTP_NM = allItems[x].B_N1ST_BZTP_NM;
                                    reportItems[nPage].B_HDQTR_PHN_NO = allItems[x].B_HDQTR_PHN_NO;
                                    reportItems[nPage].B_HDQTR_FAX_NO = allItems[x].B_HDQTR_FAX_NO;
                                    //
                                    reportItems[nPage].C_CO_RGST_NO = allItems[x].C_CO_RGST_NO;
                                    reportItems[nPage].C_CO_NM = allItems[x].C_CO_NM;
                                    reportItems[nPage].C_PRSD_NM = allItems[x].C_PRSD_NM;
                                    reportItems[nPage].C_HDQTR_ADDR = allItems[x].C_HDQTR_ADDR;
                                    reportItems[nPage].C_N1ST_BIZ_COND_NM = allItems[x].C_N1ST_BIZ_COND_NM;
                                    reportItems[nPage].C_N1ST_BZTP_NM = allItems[x].C_N1ST_BZTP_NM;
                                    reportItems[nPage].C_HDQTR_PHN_NO = allItems[x].C_HDQTR_PHN_NO;
                                    reportItems[nPage].C_HDQTR_FAX_NO = allItems[x].C_HDQTR_FAX_NO;

                                    reportItems[nPage].SL_BIL_RMK = "비 고 : " + allItems[x].SL_BIL_RMK;
                                    reportItems[nPage].SL_BIL_NO = allItems[x].SL_BIL_NO;
                                    reportItems[nPage].PRN_DT = "출력일시 : " + allItems[x].PRN_DT;
                                    reportItems[nPage].CRE_USR_NM = allItems[x].CRE_USR_NM;

                                    reportItems[nPage].SL_NXT_CLZ_FLG = allItems[x].SL_NXT_CLZ_FLG;
                                    reportItems[nPage].NXT_MON_DT = "이월일자 : " + allItems[x].NXT_MON_DT;

                                    reportItems[nPage].UPD_USR_NM = SystemProperties.USER_VO.USR_N1ST_NM;
                                }
                                else if ((x % 10) == 1)
                                {
                                    reportItems[nPage].B1 = allItems[x].RN;
                                    reportItems[nPage].B2 = allItems[x].ITM_NM;
                                    reportItems[nPage].B3 = allItems[x].ITM_SZ_NM;
                                    reportItems[nPage].B4 = allItems[x].SL_ITM_QTY;
                                    reportItems[nPage].B5 = allItems[x].SL_ITM_PRC_DC_VAL;
                                    reportItems[nPage].B6 = allItems[x].SL_ITM_AMT;
                                    reportItems[nPage].B7 = allItems[x].SL_ITM_TAX_AMT;
                                    reportItems[nPage].B8 = allItems[x].MD_QTY;
                                    reportItems[nPage].B9 = allItems[x].N1ST_ITM_GRP_NM;
                                    //
                                    reportItems[nPage].O1 = float.Parse(reportItems[nPage].O1.ToString()) + float.Parse(reportItems[nPage].B6.ToString());
                                    reportItems[nPage].O2 = float.Parse(reportItems[nPage].O2.ToString()) + float.Parse(reportItems[nPage].B7.ToString());
                                    reportItems[nPage].O3 = float.Parse(reportItems[nPage].O1.ToString()) + float.Parse(reportItems[nPage].O2.ToString());
                                }
                                else if ((x % 10) == 2)
                                {
                                    reportItems[nPage].C1 = allItems[x].RN;
                                    reportItems[nPage].C2 = allItems[x].ITM_NM;
                                    reportItems[nPage].C3 = allItems[x].ITM_SZ_NM;
                                    reportItems[nPage].C4 = allItems[x].SL_ITM_QTY;
                                    reportItems[nPage].C5 = allItems[x].SL_ITM_PRC_DC_VAL;
                                    reportItems[nPage].C6 = allItems[x].SL_ITM_AMT;
                                    reportItems[nPage].C7 = allItems[x].SL_ITM_TAX_AMT;
                                    reportItems[nPage].C8 = allItems[x].MD_QTY;
                                    reportItems[nPage].C9 = allItems[x].N1ST_ITM_GRP_NM;
                                    //
                                    reportItems[nPage].O1 = float.Parse(reportItems[nPage].O1.ToString()) + float.Parse(reportItems[nPage].C6.ToString());
                                    reportItems[nPage].O2 = float.Parse(reportItems[nPage].O2.ToString()) + float.Parse(reportItems[nPage].C7.ToString());
                                    reportItems[nPage].O3 = float.Parse(reportItems[nPage].O1.ToString()) + float.Parse(reportItems[nPage].O2.ToString());
                                }
                                else if ((x % 10) == 3)
                                {
                                    reportItems[nPage].D1 = allItems[x].RN;
                                    reportItems[nPage].D2 = allItems[x].ITM_NM;
                                    reportItems[nPage].D3 = allItems[x].ITM_SZ_NM;
                                    reportItems[nPage].D4 = allItems[x].SL_ITM_QTY;
                                    reportItems[nPage].D5 = allItems[x].SL_ITM_PRC_DC_VAL;
                                    reportItems[nPage].D6 = allItems[x].SL_ITM_AMT;
                                    reportItems[nPage].D7 = allItems[x].SL_ITM_TAX_AMT;
                                    reportItems[nPage].D8 = allItems[x].MD_QTY;
                                    reportItems[nPage].D9 = allItems[x].N1ST_ITM_GRP_NM;
                                    //
                                    reportItems[nPage].O1 = float.Parse(reportItems[nPage].O1.ToString()) + float.Parse(reportItems[nPage].D6.ToString());
                                    reportItems[nPage].O2 = float.Parse(reportItems[nPage].O2.ToString()) + float.Parse(reportItems[nPage].D7.ToString());
                                    reportItems[nPage].O3 = float.Parse(reportItems[nPage].O1.ToString()) + float.Parse(reportItems[nPage].O2.ToString());
                                }
                                else if ((x % 10) == 4)
                                {
                                    reportItems[nPage].E1 = allItems[x].RN;
                                    reportItems[nPage].E2 = allItems[x].ITM_NM;
                                    reportItems[nPage].E3 = allItems[x].ITM_SZ_NM;
                                    reportItems[nPage].E4 = allItems[x].SL_ITM_QTY;
                                    reportItems[nPage].E5 = allItems[x].SL_ITM_PRC_DC_VAL;
                                    reportItems[nPage].E6 = allItems[x].SL_ITM_AMT;
                                    reportItems[nPage].E7 = allItems[x].SL_ITM_TAX_AMT;
                                    reportItems[nPage].E8 = allItems[x].MD_QTY;
                                    reportItems[nPage].E9 = allItems[x].N1ST_ITM_GRP_NM;
                                    //
                                    reportItems[nPage].O1 = float.Parse(reportItems[nPage].O1.ToString()) + float.Parse(reportItems[nPage].E6.ToString());
                                    reportItems[nPage].O2 = float.Parse(reportItems[nPage].O2.ToString()) + float.Parse(reportItems[nPage].E7.ToString());
                                    reportItems[nPage].O3 = float.Parse(reportItems[nPage].O1.ToString()) + float.Parse(reportItems[nPage].O2.ToString());
                                }
                                else if ((x % 10) == 5)
                                {
                                    reportItems[nPage].F1 = allItems[x].RN;
                                    reportItems[nPage].F2 = allItems[x].ITM_NM;
                                    reportItems[nPage].F3 = allItems[x].ITM_SZ_NM;
                                    reportItems[nPage].F4 = allItems[x].SL_ITM_QTY;
                                    reportItems[nPage].F5 = allItems[x].SL_ITM_PRC_DC_VAL;
                                    reportItems[nPage].F6 = allItems[x].SL_ITM_AMT;
                                    reportItems[nPage].F7 = allItems[x].SL_ITM_TAX_AMT;
                                    reportItems[nPage].F8 = allItems[x].MD_QTY;
                                    reportItems[nPage].F9 = allItems[x].N1ST_ITM_GRP_NM;
                                    //
                                    reportItems[nPage].O1 = float.Parse(reportItems[nPage].O1.ToString()) + float.Parse(reportItems[nPage].F6.ToString());
                                    reportItems[nPage].O2 = float.Parse(reportItems[nPage].O2.ToString()) + float.Parse(reportItems[nPage].F7.ToString());
                                    reportItems[nPage].O3 = float.Parse(reportItems[nPage].O1.ToString()) + float.Parse(reportItems[nPage].O2.ToString());
                                }
                                else if ((x % 10) == 6)
                                {
                                    reportItems[nPage].G1 = allItems[x].RN;
                                    reportItems[nPage].G2 = allItems[x].ITM_NM;
                                    reportItems[nPage].G3 = allItems[x].ITM_SZ_NM;
                                    reportItems[nPage].G4 = allItems[x].SL_ITM_QTY;
                                    reportItems[nPage].G5 = allItems[x].SL_ITM_PRC_DC_VAL;
                                    reportItems[nPage].G6 = allItems[x].SL_ITM_AMT;
                                    reportItems[nPage].G7 = allItems[x].SL_ITM_TAX_AMT;
                                    reportItems[nPage].G8 = allItems[x].MD_QTY;
                                    reportItems[nPage].G9 = allItems[x].N1ST_ITM_GRP_NM;
                                    //
                                    reportItems[nPage].O1 = float.Parse(reportItems[nPage].O1.ToString()) + float.Parse(reportItems[nPage].G6.ToString());
                                    reportItems[nPage].O2 = float.Parse(reportItems[nPage].O2.ToString()) + float.Parse(reportItems[nPage].G7.ToString());
                                    reportItems[nPage].O3 = float.Parse(reportItems[nPage].O1.ToString()) + float.Parse(reportItems[nPage].O2.ToString());
                                }
                                else if ((x % 10) == 7)
                                {
                                    reportItems[nPage].H1 = allItems[x].RN;
                                    reportItems[nPage].H2 = allItems[x].ITM_NM;
                                    reportItems[nPage].H3 = allItems[x].ITM_SZ_NM;
                                    reportItems[nPage].H4 = allItems[x].SL_ITM_QTY;
                                    reportItems[nPage].H5 = allItems[x].SL_ITM_PRC_DC_VAL;
                                    reportItems[nPage].H6 = allItems[x].SL_ITM_AMT;
                                    reportItems[nPage].H7 = allItems[x].SL_ITM_TAX_AMT;
                                    reportItems[nPage].H8 = allItems[x].MD_QTY;
                                    reportItems[nPage].H9 = allItems[x].N1ST_ITM_GRP_NM;
                                    //
                                    reportItems[nPage].O1 = float.Parse(reportItems[nPage].O1.ToString()) + float.Parse(reportItems[nPage].H6.ToString());
                                    reportItems[nPage].O2 = float.Parse(reportItems[nPage].O2.ToString()) + float.Parse(reportItems[nPage].H7.ToString());
                                    reportItems[nPage].O3 = float.Parse(reportItems[nPage].O1.ToString()) + float.Parse(reportItems[nPage].O2.ToString());
                                }
                                else if ((x % 10) == 8)
                                {
                                    reportItems[nPage].I1 = allItems[x].RN;
                                    reportItems[nPage].I2 = allItems[x].ITM_NM;
                                    reportItems[nPage].I3 = allItems[x].ITM_SZ_NM;
                                    reportItems[nPage].I4 = allItems[x].SL_ITM_QTY;
                                    reportItems[nPage].I5 = allItems[x].SL_ITM_PRC_DC_VAL;
                                    reportItems[nPage].I6 = allItems[x].SL_ITM_AMT;
                                    reportItems[nPage].I7 = allItems[x].SL_ITM_TAX_AMT;
                                    reportItems[nPage].I8 = allItems[x].MD_QTY;
                                    reportItems[nPage].I9 = allItems[x].N1ST_ITM_GRP_NM;
                                    //
                                    reportItems[nPage].O1 = float.Parse(reportItems[nPage].O1.ToString()) + float.Parse(reportItems[nPage].I6.ToString());
                                    reportItems[nPage].O2 = float.Parse(reportItems[nPage].O2.ToString()) + float.Parse(reportItems[nPage].I7.ToString());
                                    reportItems[nPage].O3 = float.Parse(reportItems[nPage].O1.ToString()) + float.Parse(reportItems[nPage].O2.ToString());
                                }
                                else if ((x % 10) == 9)
                                {
                                    reportItems[nPage].J1 = allItems[x].RN;
                                    reportItems[nPage].J2 = allItems[x].ITM_NM;
                                    reportItems[nPage].J3 = allItems[x].ITM_SZ_NM;
                                    reportItems[nPage].J4 = allItems[x].SL_ITM_QTY;
                                    reportItems[nPage].J5 = allItems[x].SL_ITM_PRC_DC_VAL;
                                    reportItems[nPage].J6 = allItems[x].SL_ITM_AMT;
                                    reportItems[nPage].J7 = allItems[x].SL_ITM_TAX_AMT;
                                    reportItems[nPage].J8 = allItems[x].MD_QTY;
                                    reportItems[nPage].J9 = allItems[x].N1ST_ITM_GRP_NM;
                                    //
                                    reportItems[nPage].O1 = float.Parse(reportItems[nPage].O1.ToString()) + float.Parse(reportItems[nPage].J6.ToString());
                                    reportItems[nPage].O2 = float.Parse(reportItems[nPage].O2.ToString()) + float.Parse(reportItems[nPage].J7.ToString());
                                    reportItems[nPage].O3 = float.Parse(reportItems[nPage].O1.ToString()) + float.Parse(reportItems[nPage].O2.ToString());
                                }
                            }
                        }
                    }


                    //공급가액
                    long _O1 = 0;
                    //세액
                    long _O2 = 0;
                    //합계
                    long _O3 = 0;
                    //현잔금
                    long _O4 = 0;
                    //총계
                    long _O5 = 0;
                    //합계 마지막 폐이지 이동
                    for (int x = 0; x < reportItems.Count; x++)
                    {
                        _O1 += Convert.ToInt32(reportItems[x].O1);
                        _O2 += Convert.ToInt32(reportItems[x].O2);
                        _O3 += Convert.ToInt32(reportItems[x].O3);

                        reportItems[x].O1 = "---";
                        reportItems[x].O2 = "---";
                        reportItems[x].O3 = "---";
                    }
                    reportItems[reportItems.Count - 1].O1 = _O1;
                    reportItems[reportItems.Count - 1].O2 = _O2;
                    reportItems[reportItems.Count - 1].O3 = _O3;


                    S2212Report report = new S2212Report(reportItems);
                    report.Margins.Top = 0;
                    report.Margins.Bottom = 0;
                    report.Margins.Left = 80;
                    report.Margins.Right = 0;
                    report.Landscape = false;
                    report.PrintingSystem.ShowPrintStatusDialog = true;
                    report.PaperKind = System.Drawing.Printing.PaperKind.A4;


                    var window = new DocumentPreviewWindow();
                    window.PreviewControl.DocumentSource = report;
                    report.CreateDocument(true);
                    window.Title = "거래명세서 [" + SelectedMstItem.SL_RLSE_NO + "/" + SelectedMstItem.SL_CO_NM + "]";
                    window.Owner = Application.Current.MainWindow;
                    window.ShowDialog();

                    //XtraReportPreviewModel model = new XtraReportPreviewModel(report);
                    //var window = new DocumentPreviewWindow();
                    //window.PreviewControl.DocumentSource = report;
                    //report.CreateDocument();
                    //window.Show();
                    //DocumentPreviewWindow window = new DocumentPreviewWindow() { Model = model };
                    //report.CreateDocument(true);
                    //window.Title = "거래명세서 [" + SelectedMstItem.SL_RLSE_NO + "/" + SelectedMstItem.SL_CO_NM + "]";
                    //window.Owner = Application.Current.MainWindow;
                    //window.ShowDialog();


                }
            }
        }




        public async void SYSTEM_CODE_VO()
        {

            ////사업장
            //AreaList = SystemProperties.SYSTEM_CODE_VO("L-002");
            //_AreaMap = SystemProperties.SYSTEM_CODE_MAP("L-002");
            //if (AreaList.Count > 0)
            //{
            //    M_SL_AREA_NM = SystemProperties.USER_VO.EMPE_PLC_CD;
            //}
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




            ////운송 구분
            //TrspNmList = SystemProperties.SYSTEM_CODE_VO("S-025");
            //TrspNmList.Insert(0, new CodeDao() { CLSS_DESC = "전체", CLSS_CD = "" });
            //_TrspMap = SystemProperties.SYSTEM_CODE_MAP("S-025");
            //if (TrspNmList.Count > 0)
            //{
            //    M_SL_TRSP_NM = TrspNmList[0].CLSS_DESC;
            //}
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "S-025"))
            {
                if (response.IsSuccessStatusCode)
                {
                    TrspNmList = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                    if (TrspNmList.Count > 0)
                    {
                        TrspNmList.Insert(0, new SystemCodeVo() { CLSS_DESC = "전체", CLSS_CD = null });
                        M_SL_TRSP_NM = TrspNmList[0];
                    }
                }
            }


            ////운송 업체
            //TrspVehNmList = SystemProperties.SYSTEM_CODE_VO("S-028");
            ////TrspVehNmList.Insert(0, new CodeDao() { CLSS_DESC = "전체", CLSS_CD = "" });
            //_TrspVehMap = SystemProperties.SYSTEM_CODE_MAP("S-028");
            //if (TrspVehNmList.Count > 0)
            //{
            //    M_SL_TRSP_VEH_NM = TrspVehNmList[0].CLSS_DESC;
            //}
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "S-028"))
            {
                if (response.IsSuccessStatusCode)
                {
                    TrspVehNmList = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                    if (TrspVehNmList.Count > 0)
                    {
                        //TrspVehNmList.Insert(0, new SystemCodeVo() { CLSS_DESC = "전체", CLSS_CD = null });
                        M_SL_TRSP_VEH_NM = TrspVehNmList[0];
                    }
                }
            }


            ////하차 지역
            //TrspAreaNmList = SystemProperties.SYSTEM_CODE_VO("L-009");
            //TrspAreaNmList.Insert(0, new CodeDao() { CLSS_DESC = "전체", CLSS_CD = "" });
            //_TrspAreaMap = SystemProperties.SYSTEM_CODE_MAP("L-009");
            //if (TrspAreaNmList.Count > 0)
            //{
            //    M_SL_TRSP_AREA_NM = TrspAreaNmList[0].CLSS_DESC;
            //}
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "L-009"))
            {
                if (response.IsSuccessStatusCode)
                {
                    TrspAreaNmList = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                    if (TrspAreaNmList.Count > 0)
                    {
                        TrspAreaNmList.Insert(0, new SystemCodeVo() { CLSS_DESC = "전체", CLSS_CD = null });
                        M_SL_TRSP_AREA_NM = TrspAreaNmList[0];
                    }
                }
            }

            ////출하 시간
            //TrspTmNmList = SystemProperties.SYSTEM_CODE_VO("S-027");
            //_TrspTmMap = SystemProperties.SYSTEM_CODE_MAP("S-027");
            //if (TrspTmNmList.Count > 0)
            //{
            //    M_SL_TRSP_TM_NM = TrspTmNmList[0].CLSS_DESC;
            //}
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "S-027"))
            {
                if (response.IsSuccessStatusCode)
                {
                    TrspTmNmList = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                    if (TrspTmNmList.Count > 0)
                    {
                        M_SL_TRSP_TM_NM = TrspTmNmList[0];
                    }
                }
            }

            ////발행 유무
            RlseCmdNoList = new List<SystemCodeVo>()
                                {
                                        new SystemCodeVo(){ CLSS_CD = "Y", CLSS_DESC = "예"}
                                    ,   new SystemCodeVo(){CLSS_CD = "N", CLSS_DESC = "아니오"}
                                };

            ////출력 유무
            RlsePrtFlgList = new List<SystemCodeVo>()
                                {
                                        new SystemCodeVo(){ CLSS_CD = null, CLSS_DESC = "전체"}
                                    ,   new SystemCodeVo(){ CLSS_CD = "Y", CLSS_DESC = "출력"}
                                    ,   new SystemCodeVo(){CLSS_CD = "N", CLSS_DESC = "미출력"}
                                };
            M_RLSE_PRT_FLG = RlsePrtFlgList[0];

            ////거래처
            //CustomerList = SystemProperties.CUSTOMER_CODE_VO("AR", SystemProperties.USER_VO.EMPE_PLC_NM);
            //CustomerList.Insert(0, new CustomerCodeDao() { CO_NO = "전체" });
            //_CustomerMap = SystemProperties.CUSTOMER_CODE_MAP("AR", SystemProperties.USER_VO.EMPE_PLC_NM);
            //if (CustomerList.Count > 0)
            //{
            //    M_SL_CO_CD = CustomerList[0].CO_NO;
            //}
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s143", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { DELT_FLG = "N", CO_TP_CD = "AR", AREA_CD = SystemProperties.USER_VO.EMPE_PLC_NM, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    CustomerList = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                    if (CustomerList.Count > 0)
                    {
                        CustomerList.Insert(0, new SystemCodeVo() { CO_NM = "전체", CO_NO = null });
                        M_SL_CO_CD = CustomerList[0];
                    }
                }
            }

            Refresh();
        }


    }
}
