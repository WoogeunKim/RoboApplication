using AquilaErpWpfApp3.Util;
using AquilaErpWpfApp3.View.PUR.Dialog;
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
using System.Windows.Media;

namespace AquilaErpWpfApp3.ViewModel
{
    public sealed class P4422ViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private string _title = "매입정산마감";

        //private static PurServiceClient purClient = SystemProperties.PurClient;

        private IList<PurVo> selectedMstList = new List<PurVo>();
        private IList<PurVo> selectedDtlList = new List<PurVo>();

        private P4422MasterDialog masterDialog;

        //private P4422PurchaseTaxApplyDialog detailDialog;

        ////Menu Dialog
        //private ICommand _searchDialogCommand;
        //private ICommand _newDialogCommand;
        //private ICommand _editDialogCommand;
        //private ICommand _delDialogCommand;
        //private ICommand _loadDialogCommand;

        //private ICommand _finishDialogCommand;
        //private ICommand _closeDialogCommand;

        //private ICommand _addDialogCommand;



        //private ICommand _paymentDialogCommand;
        //private ICommand _paymentOffsetDialogCommand;
        //private ICommand _bookkeepingDialogCommand;
        //private ICommand _purchaseTaxDialogCommand;




        public P4422ViewModel() 
        {
            StartDt = System.DateTime.Now;
            EndDt = System.DateTime.Now;

            SYSTEM_CODE_VO();
            //--Refresh();
        }

        [Command]
        public async void Refresh()
        {
            try
            {
                //DXSplashScreen.Show<ProgressWindow>();
                SelectedDtlItem = null;
                SelectDtlList = null;

                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("p4422/mst", new StringContent(JsonConvert.SerializeObject(new PurVo(){ FM_DT = (StartDt).ToString("yyyyMM") , CO_TP_CD = M_CO_TP_NM.CLSS_CD , CO_NO = M_SL_CO_NM?.CO_NO, AREA_CD = M_SL_AREA_NM.CLSS_CD , AREA_NM = M_SL_AREA_NM.CLSS_DESC , CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectMstList = JsonConvert.DeserializeObject<IEnumerable<PurVo>>(await response.Content.ReadAsStringAsync()).Cast<PurVo>().ToList();
                    }
                    //SelectMstList = purClient.P4422SelectMstList(new PurVo() { FM_DT = (StartDt).ToString("yyyyMM")
                    //                                                         , CO_TP_CD = (string.IsNullOrEmpty(TXT_CO_TP_NM.Trim()) ? null : _CoTpCdMap[TXT_CO_TP_NM]) 
                    //                                                         , CO_NO = (string.IsNullOrEmpty(TXT_SL_CO_NM.Trim()) ? "%" : TXT_SL_CO_NM)
                    //                                                         , AREA_CD = (TXT_SL_AREA_NM.Equals("전체") ? null : _AreaMap[TXT_SL_AREA_NM])
                    //                                                         , AREA_NM = TXT_SL_AREA_NM
                    //                                                        });
                    ////////
                    Title = "[마감 년월]" + (StartDt).ToString("yyyy-MM") + ",   [사업장]" + M_SL_AREA_NM.CLSS_DESC + ",   [거래처]" + M_SL_CO_NM?.CO_NM + ",   [유형]" + M_CO_TP_NM.CLSS_DESC;

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


                        isD_FIN_DELETE = false;
                        isD_CLO_DELETE = false;

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
        //    set { SetProperty(ref _M_SL_AREA_NM, value, () => M_SL_AREA_NM, RefreshCoNm); }
        //}
        //사업장 
        private SystemCodeVo _M_SL_AREA_NM ;
        public SystemCodeVo M_SL_AREA_NM
        {
            get { return _M_SL_AREA_NM; }
            set { SetProperty(ref _M_SL_AREA_NM, value, () => M_SL_AREA_NM, RefreshCoNm); }
        }
        private async void RefreshCoNm()
        {
            if (M_SL_AREA_NM != null)
            {
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s143", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { DELT_FLG = "N", /*CO_TP_CD = "AR", */ AREA_CD = SystemProperties.USER_VO.EMPE_PLC_NM, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        CoNmList = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                        if (CoNmList.Count > 0)
                        {
                            CoNmList.Insert(0, new SystemCodeVo() { CO_NM = "전체", CO_NO = null });
                            M_SL_CO_NM = CoNmList[0];
                        }
                    }
                }
                //    _CoNmMap = SystemProperties.CUSTOMER_CODE_MAP(null, (TXT_SL_AREA_NM.Equals("전체") ? null : _AreaMap[TXT_SL_AREA_NM]));
                //    CoNmList = SystemProperties.CUSTOMER_CODE_VO(null, (TXT_SL_AREA_NM.Equals("전체") ? null : _AreaMap[TXT_SL_AREA_NM]));
                //    CoNmList.Insert(0, new CustomerCodeDao() { CO_NO = "", CO_NM = "" });
                //    if (CoNmList.Count > 0)
                //    {
                //        //M_DEPT_DESC = DeptList[0];
                //        TXT_SL_CO_NM = CoNmList[0].CO_NO;
                //    }
            }
        }


        ////품목 구분
        ////private Dictionary<string, string> _ItmGrpMap = new Dictionary<string, string>();
        //private IList<CodeDao> _ItmGrpCd = new List<CodeDao>();
        //public IList<CodeDao> ItmGrpList
        //{
        //    get { return _ItmGrpCd; }
        //    set { SetProperty(ref _ItmGrpCd, value, () => ItmGrpList); }
        //}
        ////품목 구분
        //private CodeDao _M_ITM_GRP_DESC;
        //public CodeDao M_ITM_GRP_DESC
        //{
        //    get { return _M_ITM_GRP_DESC; }
        //    set { SetProperty(ref _M_ITM_GRP_DESC, value, () => M_ITM_GRP_DESC, n1stItmGrpCd); }
        //}
        ////품목 구분
        //private string _TXT_ITM_GRP_CLSS_NM = string.Empty;
        //public string TXT_ITM_GRP_CLSS_NM
        //{
        //    get { return _TXT_ITM_GRP_CLSS_NM; }
        //    set { SetProperty(ref _TXT_ITM_GRP_CLSS_NM, value, () => TXT_ITM_GRP_CLSS_NM); }
        //}
        ////
        //public void n1stItmGrpCd()
        //{
        //    try
        //    {
        //        ItmN1stList.Clear();
        //        //_ItmN1stMap.Clear();

        //        if (M_ITM_GRP_DESC == null)
        //        {
        //            return;
        //        }

        //        ItmN1stList = SystemProperties.ITM_N1ST_CODE_VO((string.IsNullOrEmpty(M_ITM_GRP_DESC.CLSS_CD) ? null : M_ITM_GRP_DESC.CLSS_CD));
        //        ItmN1stList.Insert(0, new CodeDao() { CLSS_CD = "", CLSS_DESC = "" });


        //        M_ITM_N1ST_DESC = ItmN1stList[0];
        //        TXT_ITM_N1ST_DESC = ItmN1stList[0].CLSS_DESC;
        //        //_ItmN1stMap = SystemProperties.ITM_N1ST_CODE_MAP((string.IsNullOrEmpty(M_ITM_GRP_DESC) ? null : _ItmGrpMap[M_ITM_GRP_DESC]));
        //    }
        //     catch (System.Exception eLog)
        //     {
        //         WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
        //         return;
        //     }
        //}



        ////대분류
        ////private Dictionary<string, string> _ItmN1stMap = new Dictionary<string, string>();
        //private IList<CodeDao> _ItmN1stCd = new List<CodeDao>();
        //public IList<CodeDao> ItmN1stList
        //{
        //    get { return _ItmN1stCd; }
        //    set { SetProperty(ref _ItmN1stCd, value, () => ItmN1stList); }
        //}
        ////대분류
        //private CodeDao _M_ITM_N1ST_DESC;
        //public CodeDao M_ITM_N1ST_DESC
        //{
        //    get { return _M_ITM_N1ST_DESC; }
        //    set { SetProperty(ref _M_ITM_N1ST_DESC, value, () => M_ITM_N1ST_DESC); }
        //}
        ////대분류
        //private string _TXT_ITM_N1ST_DESC = string.Empty;
        //public string TXT_ITM_N1ST_DESC
        //{
        //    get { return _TXT_ITM_N1ST_DESC; }
        //    set { SetProperty(ref _TXT_ITM_N1ST_DESC, value, () => TXT_ITM_N1ST_DESC); }
        //}

        private string _M_SEARCH_TEXT = string.Empty;
        public string M_SEARCH_TEXT
        {
            get { return _M_SEARCH_TEXT; }
            set { SetProperty(ref _M_SEARCH_TEXT, value, () => M_SEARCH_TEXT); }
        }

        //거래처
        //private Dictionary<string, string> _CoNmMap = new Dictionary<string, string>();
        private IList<SystemCodeVo> _CoNmList = new List<SystemCodeVo>();
        public IList<SystemCodeVo> CoNmList
        {
            get { return _CoNmList; }
            set { SetProperty(ref _CoNmList, value, () => CoNmList); }
        }
        ////거래처
        //private CustomerCodeDao _M_SL_CO_NM;
        //public CustomerCodeDao M_SL_CO_NM
        //{
        //    get { return _M_SL_CO_NM; }
        //    set { SetProperty(ref _M_SL_CO_NM, value, () => M_SL_CO_NM); }
        //}
        //거래처
        private SystemCodeVo _M_SL_CO_NM;
        public SystemCodeVo M_SL_CO_NM
        {
            get { return _M_SL_CO_NM; }
            set { SetProperty(ref _M_SL_CO_NM, value, () => M_SL_CO_NM); }
        }

       

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


        private bool? _isD_UPDATE = false;
        public bool? isD_UPDATE
        {
            get { return _isD_UPDATE; }
            set { SetProperty(ref _isD_UPDATE, value, () => isD_UPDATE); }
        }

        private bool? _isM_FINISH = true;
        public bool? isM_FINISH
        {
            get { return _isM_FINISH; }
            set { SetProperty(ref _isM_FINISH, value, () => isM_FINISH); }
        }

        private bool? _isD_DELETE = false;
        public bool? isD_DELETE
        {
            get { return _isD_DELETE; }
            set { SetProperty(ref _isD_DELETE, value, () => isD_DELETE); }
        }


        private bool? _isD_FIN_DELETE = false;
        public bool? isD_FIN_DELETE
        {
            get { return _isD_FIN_DELETE; }
            set { SetProperty(ref _isD_FIN_DELETE, value, () => isD_FIN_DELETE); }
        }
        private bool? _isD_CLO_DELETE = false;
        public bool? isD_CLO_DELETE
        {
            get { return _isD_CLO_DELETE; }
            set { SetProperty(ref _isD_CLO_DELETE, value, () => isD_CLO_DELETE); }
        }



        //거래처 유형
        //private Dictionary<string, string> _CoTpCdMap = new Dictionary<string, string>();
        IList<SystemCodeVo> _CoTpCdList;
        public IList<SystemCodeVo> CoTpCdList
        {
            get { return _CoTpCdList; }
            set { SetProperty(ref _CoTpCdList, value, () => CoTpCdList); }
        }
        ////거래처 유형
        //private CodeDao _M_CO_TP_NM;
        //public CodeDao M_CO_TP_NM
        //{
        //    get { return _M_CO_TP_NM; }
        //    set { SetProperty(ref _M_CO_TP_NM, value, () => M_CO_TP_NM); }
        //}
        //거래처 유형
        private SystemCodeVo _M_CO_TP_NM;
        public SystemCodeVo M_CO_TP_NM
        {
            get { return _M_CO_TP_NM; }
            set { SetProperty(ref _M_CO_TP_NM, value, () => M_CO_TP_NM); }
        }

        public async void SelectMstDetail()
        {
            try
            {
                //DXSplashScreen.Show<ProgressWindow>();

                if (this._selectedMstItem == null)
                {
                    return;
                }
                _selectedMstItem.CLZ_FLG = _selectedMstItem.CLZ_FLG == null ? "" : _selectedMstItem.CLZ_FLG;

                if (_selectedMstItem.CLZ_FLG.Equals("Y"))
                    this.isM_FINISH = false;
                else
                    this.isM_FINISH = true;

                //if (SelectedMstItem.CLZ_FLG.Equals("N"))
                //{
                //    isD_FIN_DELETE = true;
                //    isD_CLO_DELETE = false;
                //}
                //else
                //{
                //    isD_FIN_DELETE = false;
                //    isD_CLO_DELETE = true;
                //}


                //if (string.IsNullOrEmpty(SelectedMstItem.PUR_CLZ_YRMON))
                //{
                //    isM_DELETE = false;
                //}
                //else
                //{
                //    isM_DELETE = true;
                //}


                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("p4422/dtl", new StringContent(JsonConvert.SerializeObject(SelectedMstItem), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectDtlList = JsonConvert.DeserializeObject<IEnumerable<PurVo>>(await response.Content.ReadAsStringAsync()).Cast<PurVo>().ToList();
                    }
                    //SelectDtlList = purClient.P4422SelectDtlList(SelectedMstItem);
                    // //
                    if (SelectDtlList.Count >= 1)
                    {
                        SelectedDtlItem = SelectDtlList[0];

                        isD_UPDATE = true;
                        isD_DELETE = true;


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
        public PurVo SelectedDtlItem
        {
            get
            {
                return _searchDetail;
            }
            set
            {
                if (value != null)
                {
                    SetProperty(ref _searchDetail, value, () => SelectedDtlItem);
                }
            }
        }
        //#endregion

        //매입항목
        IList<SystemCodeVo> _AdjNmList;
        public IList<SystemCodeVo> AdjNmList
        {
            get { return _AdjNmList; }
            set { SetProperty(ref _AdjNmList, value, () => AdjNmList); }
        }
        //입고처
        IList<SystemCodeVo> _AreaNmList;
        public IList<SystemCodeVo> AreaNmList
        {
            get { return _AreaNmList; }
            set { SetProperty(ref _AreaNmList, value, () => AreaNmList); }
        }
        //조정사유
        IList<SystemCodeVo> _AdjResonNmList;
        public IList<SystemCodeVo> AdjResonNmList
        {
            get { return _AdjResonNmList; }
            set { SetProperty(ref _AdjResonNmList, value, () => AdjResonNmList); }
        }


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

        //public void NewContact()
        //{

        //    if (this.SelectedMstItem == null)
        //    {
        //        return;
        //    }

        //    masterDialog = new P4422MasterDialog(new PurVo() { PUR_CLZ_YRMON = SelectedMstItem.PUR_CLZ_YRMON, CO_NO = SelectedMstItem.CO_NO, AREA_CD = SystemProperties.USER_VO.EMPE_PLC_NM});
        //    masterDialog.Title = "품목 등록 - " + _title;
        //    masterDialog.Owner = Application.Current.MainWindow;
        //    masterDialog.BorderEffect = BorderEffect.Default;
        //    ////masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
        //    ////masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
        //    bool isDialog = (bool)masterDialog.ShowDialog();
        //    if (isDialog)
        //    {
        //    //    if (masterDialog.IsEdit == false)
        //    //    {
        //    //        try
        //    //        {
        //    //            string itmCd = SelectedMstItem.ITM_CD;

        //    //            DXSplashScreen.Show<ProgressWindow>();
        //        SelectMstDetail();
        //    //            DXSplashScreen.Close();
        //    //            //
        //    //            //
        //    //            for (int x = 0; x < SelectMstList.Count; x++)
        //    //            {
        //    //                if (itmCd.Equals(SelectMstList[x].ITM_CD))
        //    //                {
        //    //                    SelectedMstItem = SelectMstList[x];
        //    //                    break;
        //    //                }
        //    //            }
        //    //        }
        //    //        catch (System.Exception eLog)
        //    //        {
        //    //            DXSplashScreen.Close();
        //    //            WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]창고간 이동", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
        //    //            return;
        //    //        }
        //    //    }
        //    }
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
            PurVo delDao = SelectedDtlItem;
            if (delDao != null)
            {
                MessageBoxResult result = WinUIMessageBox.Show("[" + delDao.SL_ADJ_NM + " / " + delDao.ITM_NM + "]" + " 정말로 삭제 하시겠습니까?", "[삭제]" + this._title, MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        //PurVo resultVo = purClient.P4422DeleteDtl(delDao);
                        //if (!resultVo.isSuccess)
                        //{
                        //    //실패
                        //    WinUIMessageBox.Show(resultVo.Message, "[에러]" + this._title, MessageBoxButton.OK, MessageBoxImage.Error);
                        //    return;
                        //}
                        SelectMstDetail();
                        WinUIMessageBox.Show("삭제가 완료되었습니다.", "[삭제]" + this._title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                    }
                    catch (System.Exception eLog)
                    {
                        WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + this._title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                        return;
                    }
                }
            }
        }





        //public ICommand LoadDialogCommand
        //{
        //    get
        //    {
        //        if (_loadDialogCommand == null)
        //            _loadDialogCommand = new DelegateCommand(LoadContact);
        //        return _loadDialogCommand;
        //    }
        //}

        //public void LoadContact()
        //{
        //    if (this.SelectedMstItem == null)
        //    {
        //        return;
        //    }

        //    MessageBoxResult result = WinUIMessageBox.Show("[" + (StartDt).ToString("yyyy-MM") + "/" + SelectedMstItem.CO_NM + " 매입 정산 불러오기 하시겠습니까?", "[매입 정산 마감]" + this._title, MessageBoxButton.YesNo, MessageBoxImage.Question);
        //    if (result == MessageBoxResult.Yes)
        //    {
        //        try
        //        {
        //            DXSplashScreen.Show<ProgressWindow>();
        //            //PurVo resultVo = purClient.ProcP4422(new PurVo() { FM_DT = (StartDt).ToString("yyyyMM"), CO_NO = SelectedMstItem.CO_NO, CRE_USR_ID = SystemProperties.USER, RN = 1 });
        //            //if (!resultVo.isSuccess)
        //            //{
        //            //    DXSplashScreen.Close();
        //            //    //실패
        //            //    WinUIMessageBox.Show(resultVo.Message, "[에러]" + this._title, MessageBoxButton.OK, MessageBoxImage.Error);
        //            //    return;
        //            //}
        //            Refresh();
        //            DXSplashScreen.Close();
        //            WinUIMessageBox.Show("완료되었습니다.", "[매입 정산 마감]" + this._title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
        //        }
        //        catch (System.Exception eLog)
        //        {
        //            DXSplashScreen.Close();
        //            WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + this._title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
        //            return;
        //        }
        //    }
        //}








        //public ICommand FinishDialogCommand
        //{
        //    get
        //    {
        //        if (_finishDialogCommand == null)
        //            _finishDialogCommand = new DelegateCommand(FinishContact);
        //        return _finishDialogCommand;
        //    }
        //}

        [Command]
        public async void FinishContact()
        {
            if (this.SelectedMstItem == null)
            {
                return;
            }

            MessageBoxResult result = WinUIMessageBox.Show("[" + SelectedMstItem.FM_DT + "/" + SelectedMstItem.CO_NM + "]" + " 마감 하시겠습니까?", "[매입 정산 마감]" + this._title, MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    int _Num = 0;
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("p4422/proc", new StringContent(JsonConvert.SerializeObject(new PurVo() { AREA_CD = SelectedMstItem.AREA_CD, FM_DT = SelectedMstItem.FM_DT, CO_NO = SelectedMstItem.CO_NO, CRE_USR_ID = SystemProperties.USER, RN = 1, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string resultMsg = await response.Content.ReadAsStringAsync();
                            if (int.TryParse(resultMsg, out _Num) == false)
                            {
                                //실패
                                WinUIMessageBox.Show(resultMsg, this._title, MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                            WinUIMessageBox.Show("완료되었습니다.", "[매입 정산 마감]" + this._title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                            //Refresh();
                            //성공
                            //WinUIMessageBox.Show("삭제가 완료되었습니다.", this.title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                        }
                    }
                    //PurVo resultVo = purClient.ProcP4422(new PurVo() { AREA_CD = SelectedMstItem.AREA_CD, FM_DT = SelectedMstItem.FM_DT, CO_NO = SelectedMstItem.CO_NO, CRE_USR_ID = SystemProperties.USER, RN = 1 });
                    //if (!resultVo.isSuccess)
                    //{
                    //    DXSplashScreen.Close();
                    //    //실패
                    //    WinUIMessageBox.Show(resultVo.Message, "[에러]" + this._title, MessageBoxButton.OK, MessageBoxImage.Error);
                    //    return;
                    //}
                }
                catch (System.Exception eLog)
                {
                    WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + this._title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                    return;
                }
            }
        }

        //public ICommand CloseDialogCommand
        //{
        //    get
        //    {
        //        if (_closeDialogCommand == null)
        //            _closeDialogCommand = new DelegateCommand(CloseContact);
        //        return _closeDialogCommand;
        //    }
        //}

        //public void CloseContact()
        //{
        //    if (this.SelectedMstItem == null)
        //    {
        //        return;
        //    }

        //    MessageBoxResult result = WinUIMessageBox.Show("[" + SelectedMstItem.PUR_CLZ_YRMON + "/" + SelectedMstItem.CO_NM + "]" + " 마감 취소 하시겠습니까?", "[매입 정산 마감]" + this._title, MessageBoxButton.YesNo, MessageBoxImage.Question);
        //    if (result == MessageBoxResult.Yes)
        //    {
        //        try
        //        {
        //            //DXSplashScreen.Show<ProgressWindow>();
        //            SelectedMstItem.CLZ_FLG = "N";
        //            //PurVo resultVo = purClient.P4422UpdateMst(SelectedMstItem);
        //            //if (!resultVo.isSuccess)
        //            //{
        //            //    //실패
        //            //    WinUIMessageBox.Show(resultVo.Message, "[에러]" + this._title, MessageBoxButton.OK, MessageBoxImage.Error);
        //            //    return;
        //            //}
        //            //Refresh();
        //            //DXSplashScreen.Close();
        //            isD_FIN_DELETE = true;
        //            isD_CLO_DELETE = false;
        //            WinUIMessageBox.Show("완료되었습니다.", "[매입 정산 마감]" + this._title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
        //        }
        //        catch (System.Exception eLog)
        //        {
        //            WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + this._title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
        //            return;
        //        }
        //    }
        //}





        //public ICommand AddDialogCommand
        //{
        //    get
        //    {
        //        if (_addDialogCommand == null)
        //            _addDialogCommand = new DelegateCommand(AddContact);
        //        return _addDialogCommand;
        //    }
        //}

        //public void AddContact()
        //{
        //    if (this.SelectedMstItem == null)
        //    {
        //        return;
        //    }

        //    MessageBoxResult result = WinUIMessageBox.Show("[" + SelectedMstItem.PUR_CLZ_YRMON + "/" + SelectedMstItem.CO_NM + "]" + " 매입 조정 하시겠습니까?", "[매입 정산 마감]" + this._title, MessageBoxButton.YesNo, MessageBoxImage.Question);
        //    if (result == MessageBoxResult.Yes)
        //    {
        //        try
        //        {
        //            //DXSplashScreen.Show<ProgressWindow>();
        //            //PurVo resultVo = purClient.P4422InsertDtl(new PurVo() { PUR_CLZ_YRMON = SelectedMstItem.PUR_CLZ_YRMON, CO_NO = SelectedMstItem.CO_NO, SL_ADJ_CD = "400", ITM_CD = "할인", AREA_CD = SelectedMstItem.AREA_CD, SL_ADJ_RESON_CD = "100", SL_ITM_QTY = 0, SL_ITM_PRC = 0, SL_ITM_AMT = 0, CRE_USR_ID = SystemProperties.USER, UPD_USR_ID = SystemProperties.USER });
        //            //if (!resultVo.isSuccess)
        //            //{
        //            //    //실패
        //            //    WinUIMessageBox.Show(resultVo.Message, "[에러]" + this._title, MessageBoxButton.OK, MessageBoxImage.Error);
        //            //    return;
        //            //}

        //            SelectMstDetail();

        //            if (SelectDtlList.Count >= 1)
        //            {
        //                SelectedDtlItem = SelectDtlList[SelectDtlList.Count -1];
        //            }

        //            WinUIMessageBox.Show("완료되었습니다.", "[매입 정산 마감]" + this._title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
        //        }
        //        catch (System.Exception eLog)
        //        {
        //            WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + this._title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
        //            return;
        //        }
        //    }
        //}





        //public ICommand PaymentDialogCommand
        //{
        //    get
        //    {
        //        if (_paymentDialogCommand == null)
        //            _paymentDialogCommand = new DelegateCommand(PaymentContact);
        //        return _paymentDialogCommand;
        //    }
        //}
        [Command]
        public async void PaymentContact()
        {
            if (this.SelectedMstItem == null)
            {
                return;
            }
            masterDialog = new P4422MasterDialog(new PurVo() { FM_DT = SelectedMstItem.FM_DT + "01", CO_NM = SelectedMstItem.CO_NM, AREA_NM = SelectedMstItem.AREA_NM });
            masterDialog.Title = this._title + " - 대금결제 ";
            masterDialog.Owner = Application.Current.MainWindow;
            masterDialog.BorderEffect = BorderEffect.Default;
            masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            bool isDialog = (bool)masterDialog.ShowDialog();
            if (isDialog)
            {
                MessageBoxResult result = WinUIMessageBox.Show("[" + SelectedMstItem.FM_DT + "/" + SelectedMstItem.CO_NM + "] 대금결제 하시겠습니까?", "[대금결제]" + this._title, MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        int _Num = 0;
                        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("p4422/proc", new StringContent(JsonConvert.SerializeObject(new PurVo() { AREA_CD = SelectedMstItem.AREA_CD, FM_DT = DateTime.Parse(masterDialog.FM_DT).ToString("yyyyMMdd"), CO_NO = SelectedMstItem.CO_NO, CRE_USR_ID = SystemProperties.USER, GBN = "400", RN = 2, PUR_AMT = masterDialog.SL_ITM_AMT, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                string resultMsg = await response.Content.ReadAsStringAsync();
                                if (int.TryParse(resultMsg, out _Num) == false)
                                {
                                    //실패
                                    WinUIMessageBox.Show(resultMsg, this._title, MessageBoxButton.OK, MessageBoxImage.Error);
                                    return;
                                }
                                SelectMstDetail();
                                WinUIMessageBox.Show("완료되었습니다.", "[대금결제]" + this._title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                            }
                        }
                   }
                    catch (System.Exception eLog)
                    {
                        WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + this._title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                        return;
                    }
                }
            }
        }


        //public ICommand PaymentOffsetDialogCommand
        //{
        //    get
        //    {
        //        if (_paymentOffsetDialogCommand == null)
        //            _paymentOffsetDialogCommand = new DelegateCommand(PaymentOffsetContact);
        //        return _paymentOffsetDialogCommand;
        //    }
        //}
        [Command]
        public async void PaymentOffsetContact()
        {
            if (this.SelectedMstItem == null)
            {
                return;
            }
            masterDialog = new P4422MasterDialog(new PurVo() { FM_DT = SelectedMstItem.FM_DT + "01", CO_NM = SelectedMstItem.CO_NM, AREA_NM = SelectedMstItem.AREA_NM });
            masterDialog.Title = this._title + " - 대금결제(상계) ";
            masterDialog.Owner = Application.Current.MainWindow;
            masterDialog.BorderEffect = BorderEffect.Default;
            masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            bool isDialog = (bool)masterDialog.ShowDialog();
            if (isDialog)
            {

                MessageBoxResult result = WinUIMessageBox.Show("[" + SelectedMstItem.FM_DT + "/" + SelectedMstItem.CO_NM + "] 대금결제(상계) 하시겠습니까?", "[대금결제(상계)]" + this._title, MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        int _Num = 0;
                        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("p4422/proc", new StringContent(JsonConvert.SerializeObject(new PurVo() { AREA_CD = SelectedMstItem.AREA_CD, FM_DT = DateTime.Parse(masterDialog.FM_DT).ToString("yyyyMMdd"), CO_NO = SelectedMstItem.CO_NO, CRE_USR_ID = SystemProperties.USER, GBN = "450", RN = 2, PUR_AMT = masterDialog.SL_ITM_AMT, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                string resultMsg = await response.Content.ReadAsStringAsync();
                                if (int.TryParse(resultMsg, out _Num) == false)
                                {
                                    //실패
                                    WinUIMessageBox.Show(resultMsg, this._title, MessageBoxButton.OK, MessageBoxImage.Error);
                                    return;
                                }
                                SelectMstDetail();
                                WinUIMessageBox.Show("완료되었습니다.", "대금결제(상계)]" + this._title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                            }
                        }
                    }
                    catch (System.Exception eLog)
                    {
                        WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + this._title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                        return;
                    }
                }
            }
            }


        //public ICommand BookkeepingDialogCommand
        //{
        //    get
        //    {
        //        if (_bookkeepingDialogCommand == null)
        //            _bookkeepingDialogCommand = new DelegateCommand(BookkeepingContact);
        //        return _bookkeepingDialogCommand;
        //    }
        //}
        [Command]
        public async void BookkeepingContact()
        {
            if (this.SelectedMstItem == null)
            {
                return;
            }
            masterDialog = new P4422MasterDialog(new PurVo() { FM_DT = SelectedMstItem.FM_DT + "01", CO_NM = SelectedMstItem.CO_NM, AREA_NM = SelectedMstItem.AREA_NM });
            masterDialog.Title = this._title + " - 장부정리 ";
            masterDialog.Owner = Application.Current.MainWindow;
            masterDialog.BorderEffect = BorderEffect.Default;
            masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            bool isDialog = (bool)masterDialog.ShowDialog();
            if (isDialog)
            {
                MessageBoxResult result = WinUIMessageBox.Show("[" + SelectedMstItem.FM_DT + "/" + SelectedMstItem.CO_NM + "] 장부정리 하시겠습니까?", "[장부정리]" + this._title, MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        int _Num = 0;
                        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("p4422/proc", new StringContent(JsonConvert.SerializeObject(new PurVo() { AREA_CD = SelectedMstItem.AREA_CD, FM_DT = DateTime.Parse(masterDialog.FM_DT).ToString("yyyyMMdd"), CO_NO = SelectedMstItem.CO_NO, CRE_USR_ID = SystemProperties.USER, GBN = "800", RN = 2, PUR_AMT = masterDialog.SL_ITM_AMT, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                string resultMsg = await response.Content.ReadAsStringAsync();
                                if (int.TryParse(resultMsg, out _Num) == false)
                                {
                                    //실패
                                    WinUIMessageBox.Show(resultMsg, this._title, MessageBoxButton.OK, MessageBoxImage.Error);
                                    return;
                                }
                                SelectMstDetail();
                                WinUIMessageBox.Show("완료되었습니다.", "장부정리" + this._title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                            }
                        }
                    }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + this._title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }
    }
}







        //public ICommand PurchaseTaxDialogCommand
        //{
        //    get
        //    {
        //        if (_purchaseTaxDialogCommand == null)
        //            _purchaseTaxDialogCommand = new DelegateCommand(PurchaseTaxContact);
        //        return _purchaseTaxDialogCommand;
        //    }
        //}
        [Command]
        public async void PurchaseTaxContact()
        {
            if (this.SelectedMstItem == null)
            {
                return;
            }
            masterDialog = new P4422MasterDialog(new PurVo() { FM_DT = SelectedMstItem.FM_DT + "01", CO_NM = SelectedMstItem.CO_NM, AREA_NM = SelectedMstItem.AREA_NM });
            masterDialog.Title = this._title + " - 매입부가세 ";
            masterDialog.Owner = Application.Current.MainWindow;
            masterDialog.BorderEffect = BorderEffect.Default;
            masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            bool isDialog = (bool)masterDialog.ShowDialog();
            if (isDialog)
            {

                MessageBoxResult result = WinUIMessageBox.Show("[" + DateTime.Parse(masterDialog.FM_DT).ToString("yyyyMMdd") + "/" + masterDialog.AREA_NM + "/" + masterDialog.CO_NM + "] 매입부가세 하시겠습니까?", "[매입부가세]" + this._title, MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        int _Num = 0;
                        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("p4422/proc", new StringContent(JsonConvert.SerializeObject(new PurVo() { AREA_CD = SelectedMstItem.AREA_CD, FM_DT = DateTime.Parse(masterDialog.FM_DT).ToString("yyyyMMdd"), CO_NO = SelectedMstItem.CO_NO, CRE_USR_ID = SystemProperties.USER, GBN = "500", RN = 2, PUR_AMT = masterDialog.SL_ITM_AMT, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                string resultMsg = await response.Content.ReadAsStringAsync();
                                if (int.TryParse(resultMsg, out _Num) == false)
                                {
                                    //실패
                                    WinUIMessageBox.Show(resultMsg, this._title, MessageBoxButton.OK, MessageBoxImage.Error);
                                    return;
                                }
                                SelectMstDetail();
                                WinUIMessageBox.Show("완료되었습니다.", "매입부가세" + this._title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                            }
                        }
                    }
                    catch (System.Exception eLog)
                    {
                        WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                        return;
                    }
                }
            }
        }

        public async void SYSTEM_CODE_VO()
        {
            //사업장
            //AreaList = SystemProperties.SYSTEM_CODE_VO("L-002");
            //AreaList.Insert(0, new CodeDao() { CLSS_CD = null, CLSS_DESC = "전체" });
            //_AreaMap = SystemProperties.SYSTEM_CODE_MAP("L-002");
            //if (AreaList.Count > 0)
            //{
            //    TXT_SL_AREA_NM = "전체";// SystemProperties.USER_VO.EMPE_PLC_CD;
            //    //for (int x = 0; x < AreaList.Count; x++)
            //    //{
            //    //    if (TXT_SL_AREA_NM.Equals(AreaList[x].CLSS_DESC))
            //    //    {
            //    //        M_SL_AREA_NM = AreaList[x];
            //    //        break;
            //    //    }
            //    //}
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

            ////거래처
            //this._CoNmMap = SystemProperties.CUSTOMER_CODE_MAP(null, null);
            //this.CoNmList = SystemProperties.CUSTOMER_CODE_VO(null, null);
            //this.CoNmList.Insert(0, new CustomerCodeDao() { CO_NO = " " });
            ////this.M_SL_CO_NM = this.CoNmList[0];
            //this.TXT_SL_CO_NM = this.CoNmList[0].CO_NO;
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s143", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { DELT_FLG = "N", /*CO_TP_CD = "AR", */ AREA_CD = SystemProperties.USER_VO.EMPE_PLC_NM, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    CoNmList = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                    if (CoNmList.Count > 0)
                    {
                        CoNmList.Insert(0, new SystemCodeVo() { CO_NM = "전체", CO_NO = null });
                        //M_SL_CO_NM = CoNmList[0];
                    }
                }
            }

            this.CoTpCdList = new List<SystemCodeVo>() {   new SystemCodeVo() { CLSS_CD = null, CLSS_DESC = "전체" }
                                                         , new SystemCodeVo() { CLSS_CD = "AP", CLSS_DESC = "내수" }
                                                         , new SystemCodeVo() { CLSS_CD = "OR", CLSS_DESC = "수입" }
                                                         , new SystemCodeVo() { CLSS_CD = "SU", CLSS_DESC = "외주" } };
            this.M_CO_TP_NM = this.CoTpCdList[0];


            //매입항목
            //this.AdjNmList = SystemProperties.SYSTEM_CODE_VO("P-009");
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "P-009"))
            {
                if (response.IsSuccessStatusCode)
                {
                    AdjNmList = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                    //if (AdjNmList.Count > 0)
                    //{
                    //    M_SL_AREA_NM = AreaList[0];
                    //}
                }
            }

            //입고처
            //this.AreaNmList = SystemProperties.SYSTEM_CODE_VO("L-002");
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "L-002"))
            {
                if (response.IsSuccessStatusCode)
                {
                    AreaNmList = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                    //if (AdjNmList.Count > 0)
                    //{
                    //    M_SL_AREA_NM = AreaList[0];
                    //}
                }
            }

            //조정사유
            //this.AdjResonNmList = SystemProperties.SYSTEM_CODE_VO("P-010");
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "P-010"))
            {
                if (response.IsSuccessStatusCode)
                {
                    AdjResonNmList = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                    //if (AdjNmList.Count > 0)
                    //{
                    //    M_SL_AREA_NM = AreaList[0];
                    //}
                }
            }

            Refresh();
        }


    }
}
