using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
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
using AquilaErpWpfApp3.Util;

namespace AquilaErpWpfApp3.ViewModel
{
    public sealed class I6210ViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private string _title = "자재수불장";

        //private static InvServiceClient invClient = SystemProperties.InvClient;
        private IList<InvVo> selectedMstList = new List<InvVo>();
        private IList<InvVo> selectedTmpList = new List<InvVo>();

        private int nDiffMonth;
        private List<string> diffMonth = new List<string>();

        //private ObservableCollection<PurVo> selectedDtlList = new ObservableCollection<PurVo>();
        //private IList<InvVo> selectedDtlList = new List<InvVo>();

        //private I5514MasterDialog masterDialog;
        //private I5513DetailDialog detailDialog;

        ////Menu Dialog
        //private ICommand _searchDialogCommand;
        //private ICommand _newDialogCommand;
        //private ICommand _editDialogCommand;
        //private ICommand _delDialogCommand;

        //private ICommand _searchDetailDialogCommand;
        //private ICommand _newDetailDialogCommand;
        //private ICommand _editDetailDialogCommand;
        //private ICommand _delDetailDialogCommand;

        //private ICommand reportDialogCommand;

        ////private ICommand _revListDetailDialogCommand;
        ////private ICommand _revNewDetailDialogCommand;

        //private I6629MasterDialog masterDialog;
        //private P41DetailDialog detailDialog;
        ////private A21JobItemRevDialog jobItemRevDialog;

        //private P41ReportDialog reportDialog;

        public I6210ViewModel() 
        {
            StartDt = new DateTime(System.DateTime.Now.Year, 1, 1);
            EndDt = System.DateTime.Now;

            SYSTEM_CODE_VO();

            ////거래처
            //this.CoNmList = SystemProperties.CUSTOMER_CODE_VO(null);
            //this.CoNmList.Insert(0, new CustomerCodeDao() { CO_NO = " " });
            //this.M_SL_CO_NM = this.CoNmList[0];
            //this.TXT_SL_CO_NM = this.CoNmList[0].CO_NO;


            ////활성 여부
            //this.ActList = new List<CodeDao>();
            //this.ActList.Insert(0, new CodeDao() { CLSS_CD= "예", CLSS_DESC = "Y" });
            //this.ActList.Insert(1, new CodeDao() { CLSS_CD= "아니오", CLSS_DESC = "N" });

            ////요청 등록자
            //this.UsrList = SystemProperties.USER_CODE_VO();

            ////품목 구분
            //ItmGrpList = SystemProperties.SYSTEM_CODE_VO("L-001");
            //if (ItmGrpList.Count > 0)
            //{
            //    M_ITM_GRP_DESC = ItmGrpList[0];
            //    TXT_ITM_GRP_CLSS_NM = ItmGrpList[0].CLSS_DESC;
            //}

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

            //Refresh();

            //if(SystemProperties.SEEK_I6620 != null)
            //{
            //    M_SEARCH_TEXT = SystemProperties.SEEK_I6620.ITM_CD;
            //    M_ITM_CD = SystemProperties.SEEK_I6620;
            //    SystemProperties.SEEK_I6620 = null;
            //    IS_SEEK_I6620 = false;
            //    Refresh();
            //}
        }

        [Command]
        public async void Refresh()
        {
            try
            {
                selectedTmpList = new List<InvVo>();
                SelectMstList = new List<InvVo>();

                if (M_ITEM_NM == null)
                {
                    WinUIMessageBox.Show("품목코드를 입력 해주세요", _title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                    return;
                }

                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("i6210", new StringContent(JsonConvert.SerializeObject(new InvVo() { FM_DT = (StartDt).ToString("yyyyMMdd") , TO_DT = (EndDt).ToString("yyyyMMdd") , YRMON = (StartDt).ToString("yyyyMM") , AREA_CD = M_SL_AREA_NM.CLSS_CD , ITM_CD = M_ITEM_NM.ITM_CD, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        selectedTmpList = JsonConvert.DeserializeObject<IEnumerable<InvVo>>(await response.Content.ReadAsStringAsync()).Cast<InvVo>().ToList();
                    }

                    //SelectMstList = invClient.I6610SelectMstList(new InvVo() { FM_DT = (StartDt).ToString("yyyy-MM-dd"), TO_DT = (EndDt).ToString("yyyy-MM-dd"), AREA_CD = (string.IsNullOrEmpty(TXT_SL_AREA_NM) ? null : _AreaMap[TXT_SL_AREA_NM]), GBN = (string.IsNullOrEmpty(M_CHECKD) ? "%" : M_CHECKD), LOC_CD = (string.IsNullOrEmpty(TXT_SL_LOC_NM) ? null : _LocMap[TXT_SL_LOC_NM]), IN_FLG = (IN_FLG == true ? "Y" : "N") });
                    ////+ ",   [입고여부]" + (IN_FLG == true ? "Y" : "N") 
                    //Title = "[기간]" + (StartDt).ToString("yyyy-MM-dd") + "~" + (EndDt).ToString("yyyy-MM-dd") + ",   [사업장]" + M_SL_AREA_NM.CLSS_DESC ;

                    //if (SelectMstList.Count >= 1)
                    //{
                    //    isM_UPDATE = true;
                    //    isM_DELETE = true;
                    //    //SelectedMstItem = SelectMstList[0];
                    //    //for (int x = 0; x < SelectMstList.Count; x++)
                    //    //{
                    //    //    SelectMstList[x].DTL_DATA = invClient.I6610SelectDtlList(SelectedMstItem);
                    //    //}
                    //}
                    //else
                    //{
                    //    isM_UPDATE = false;
                    //    isM_DELETE = false;
                    //}
                //DXSplashScreen.Close();
            }


                //selectedTmpList = invClient.I6210SelectMstList(new InvVo() { FM_DT = (StartDt).ToString("yyyyMMdd")
                //                                                         , TO_DT = (EndDt).ToString("yyyyMMdd")
                //                                                         , YRMON = (StartDt).ToString("yyyyMM")
                //                                                         , AREA_CD = (string.IsNullOrEmpty(TXT_SL_AREA_NM) ? null : _AreaMap[TXT_SL_AREA_NM])
                //                                                         , ITM_CD = M_SEARCH_TEXT
                //                                                        });

                Title = "[일자]" + (StartDt).ToString("yyyy-MM-dd") + " ~ " + (EndDt).ToString("yyyy-MM-dd") + ",   [사업장]" + (M_SL_AREA_NM.CLSS_CD_DESC) + ",   [품목]" + (string.IsNullOrEmpty(M_SEARCH_TEXT) ? "" : M_SEARCH_TEXT /*(",   [품목]" + M_ITM_CD.ITM_NM + "/" + M_ITM_CD.ITM_SZ_NM)*/);

                if (selectedTmpList.Count > 0)
                {
                    //이월
                    if (selectedTmpList[0].INAUD_NM.Equals("( 이 월 )"))
                    {
                        selectedTmpList[0].YRMON = "0";
                        SelectMstList.Add(selectedTmpList[0]);
                    }

                    //월수
                    this.diffMonth.Clear();
                    this.nDiffMonth = (12 * (EndDt.Year - StartDt.Year)) + (EndDt.Month - StartDt.Month);
                    for (int x = 0; x <= this.nDiffMonth; x++)
                    {
                        this.diffMonth.Insert(x, StartDt.AddMonths(x).ToString("yyyyMM"));
                    }

                    //계산
                    IList<InvVo> tmpList = new List<InvVo>();
                    object TTL_IN_QTY = 0;
                    object TTL_IN_AMT = 0;
                    object TTL_OUT_QTY = 0;
                    object TTL_OUT_AMT = 0;
                    object TTL_STK_QTY = 0;
                    object TTL_STK_AMT = 0;
                    if (selectedTmpList[0].INAUD_NM.Equals("( 이 월 )"))
                    {
                        TTL_IN_QTY = SelectMstList[0].IN_QTY;
                        TTL_IN_AMT = SelectMstList[0].IN_AMT;
                        TTL_OUT_QTY = SelectMstList[0].OUT_QTY;
                        TTL_OUT_AMT = SelectMstList[0].OUT_AMT;
                        TTL_STK_QTY = SelectMstList[0].STK_QTY;
                        TTL_STK_AMT = SelectMstList[0].STK_AMT;
                    }
                    //
                    object MON_IN_QTY = 0;
                    object MON_IN_AMT = 0;
                    object MON_OUT_QTY = 0;
                    object MON_OUT_AMT = 0;
                    object MON_STK_QTY = 0;
                    object MON_STK_AMT = 0;
                    for (int x = 0; x < this.diffMonth.Count; x++)
                    {
                        MON_IN_QTY = 0;
                        MON_IN_AMT = 0;
                        MON_OUT_QTY = 0;
                        MON_OUT_AMT = 0;
                        MON_STK_QTY = 0;
                        MON_STK_AMT = 0;
                        tmpList = selectedTmpList.Where(z => z.YRMON == this.diffMonth[x]).OrderBy(z => z.INAUD_DT).ToList<InvVo>();
                        for (int y = 0; y < tmpList.Count; y++)
                        {
                            SelectMstList.Add(tmpList[y]);
                            //(월계)
                            MON_IN_QTY = Convert.ToInt64(MON_IN_QTY) + Convert.ToInt64(tmpList[y].IN_QTY);
                            MON_IN_AMT = Convert.ToDouble(MON_IN_AMT) + Convert.ToDouble(tmpList[y].IN_AMT);
                            MON_OUT_QTY = Convert.ToInt64(MON_OUT_QTY) + Convert.ToInt64(tmpList[y].OUT_QTY);
                            MON_OUT_AMT = Convert.ToDouble(MON_OUT_AMT) + Convert.ToDouble(tmpList[y].OUT_AMT);
                            MON_STK_QTY = Convert.ToInt64(MON_STK_QTY) + Convert.ToInt64(tmpList[y].STK_QTY);
                            MON_STK_AMT = Convert.ToDouble(MON_STK_AMT) + Convert.ToDouble(tmpList[y].STK_AMT);
                            //(누계)
                            TTL_IN_QTY = Convert.ToInt64(TTL_IN_QTY) + Convert.ToInt64(tmpList[y].IN_QTY);
                            TTL_IN_AMT = Convert.ToDouble(TTL_IN_AMT) + Convert.ToDouble(tmpList[y].IN_AMT);
                            TTL_OUT_QTY = Convert.ToInt64(TTL_OUT_QTY) + Convert.ToInt64(tmpList[y].OUT_QTY);
                            TTL_OUT_AMT = Convert.ToDouble(TTL_OUT_AMT) + Convert.ToDouble(tmpList[y].OUT_AMT);
                            TTL_STK_QTY = Convert.ToInt64(TTL_STK_QTY) + Convert.ToInt64(tmpList[y].STK_QTY);
                            TTL_STK_AMT = Convert.ToDouble(TTL_STK_AMT) + Convert.ToDouble(tmpList[y].STK_AMT);
                            //(월계) && (합계)
                            if ((tmpList.Count -1) == y)
                            {
                                SelectMstList.Add(new InvVo() { YRMON = "1", INAUD_NM = this.diffMonth[x] + " (월계)", IN_QTY = MON_IN_QTY, IN_AMT = MON_IN_AMT, OUT_QTY = Convert.ToInt64(MON_OUT_QTY), OUT_AMT = MON_OUT_AMT, STK_QTY = Convert.ToInt64(MON_STK_QTY), STK_AMT = Convert.ToInt64(MON_STK_AMT) });
                                SelectMstList.Add(new InvVo() { YRMON = "2", INAUD_NM = " (누계)", IN_QTY = TTL_IN_QTY, IN_AMT = TTL_IN_AMT, OUT_QTY = Convert.ToInt64(TTL_OUT_QTY), OUT_AMT = TTL_OUT_AMT, STK_QTY = Convert.ToInt64(TTL_STK_QTY), STK_AMT = Convert.ToInt64(TTL_STK_AMT) });
                            }
                        }

                    }

                    if (SelectMstList.Count >= 1)
                    {

                        this.nDiffMonth = (12 * (EndDt.Year - StartDt.Year)) + (EndDt.Month - StartDt.Month);
                        //this.diffMonth
                        for (int x = 0; x <= this.nDiffMonth; x++)
                        {
                            this.diffMonth.Insert(x, StartDt.AddMonths(x).ToString("yyyyMM"));
                        }

                        isM_UPDATE = true;
                        isM_DELETE = true;

                        SelectedMstItem = SelectMstList[0];
                    }
                    else
                    {
                        isM_UPDATE = false;
                        isM_DELETE = false;
                        //
                        //isD_UPDATE = false;
                        //isD_DELETE = false;
                    }
                }
            }
            catch (System.Exception eLog)
            {
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
        //사업장
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


        //
        //품목
        //private Dictionary<string, string> _AreaMap = new Dictionary<string, string>();
        private IList<SystemCodeVo> _ItemCd = new List<SystemCodeVo>();
        public IList<SystemCodeVo> ItemsList
        {
            get { return _ItemCd; }
            set { SetProperty(ref _ItemCd, value, () => ItemsList); }
        }
        //사업장
        //private CodeDao _M_SL_AREA_NM;
        //public CodeDao M_SL_AREA_NM
        //{
        //    get { return _M_SL_AREA_NM; }
        //    set { SetProperty(ref _M_SL_AREA_NM, value, () => M_SL_AREA_NM); }
        //}
        //품목 
        private SystemCodeVo _M_ITEM_NM;
        public SystemCodeVo M_ITEM_NM
        {
            get { return _M_ITEM_NM; }
            set { SetProperty(ref _M_ITEM_NM, value, () => M_ITEM_NM); }
        }

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

        //private ItemCodeVo _M_ITM_CD;
        //public ItemCodeVo M_ITM_CD
        //{
        //    get { return _M_ITM_CD; }
        //    set { SetProperty(ref _M_ITM_CD, value, () => M_ITM_CD); }
        //}

        ////거래처
        ////private Dictionary<string, string> _AreaMap = new Dictionary<string, string>();
        //private IList<CustomerCodeDao> _CoNmList = new List<CustomerCodeDao>();
        //public IList<CustomerCodeDao> CoNmList
        //{
        //    get { return _CoNmList; }
        //    set { SetProperty(ref _CoNmList, value, () => CoNmList); }
        //}
        ////거래처
        //private CustomerCodeDao _M_SL_CO_NM;
        //public CustomerCodeDao M_SL_CO_NM
        //{
        //    get { return _M_SL_CO_NM; }
        //    set { SetProperty(ref _M_SL_CO_NM, value, () => M_SL_CO_NM); }
        //}
        ////거래처
        //private string _TXT_SL_CO_NM = string.Empty;
        //public string TXT_SL_CO_NM
        //{
        //    get { return _TXT_SL_CO_NM; }
        //    set { SetProperty(ref _TXT_SL_CO_NM, value, () => TXT_SL_CO_NM); }
        //}

        ////요청 등록자
        //private IList<UserCodeDao> _UsrList = new List<UserCodeDao>();
        //public IList<UserCodeDao> UsrList
        //{
        //    get { return _UsrList; }
        //    set { SetProperty(ref _UsrList, value, () => UsrList); }
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

        ////활성 여부
        //IList<CodeDao> _ActList;
        //public IList<CodeDao> ActList
        //{
        //    get { return _ActList; }
        //    set { SetProperty(ref _ActList, value, () => ActList); }
        //}

        private void SelectMstDetail()
        {
             //try
             //{
             //    //DXSplashScreen.Show<ProgressWindow>();

             //   if (this._selectedMstItem == null)
             //   {
             //       return;
             //   }

             //   //SelectDtlList = new ObservableCollection<PurVo>(purClient.P4401SelectDtlList(new PurVo() { ITM_CD = this._selectedMstItem.}));
             //   SelectDtlList = invClient.I5513SelectDtlList(SelectedMstItem);
             //   // //
             //   if (SelectDtlList.Count >= 1)
             //   {
             //       isD_UPDATE = true;
             //       isD_DELETE = true;
             //   }
             //   else
             //   {
             //       isD_UPDATE = false;
             //       isD_DELETE = false;
             //   }

             //   //DXSplashScreen.Close();
             //}
             //catch (System.Exception eLog)
             //{
             //    //DXSplashScreen.Close();
             //    //
             //    WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]출고의뢰내역등록", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
             //    return;
             //}
        }
        //#endregion


        ////#region Functon <Detail List>
        //public IList<InvVo> SelectDtlList
        //{
        //    get { return selectedDtlList; }
        //    set { SetProperty(ref selectedDtlList, value, () => SelectDtlList); }
        //}

        //InvVo _searchDetail;
        //public InvVo SearchDetail
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
        ////#endregion


        string _Title = string.Empty;
        public string Title
        {
            get { return _Title; }
            set { SetProperty(ref _Title, value, () => Title); }
        }




        bool _IS_SEEK_I6620 = true;
        public bool IS_SEEK_I6620
        {
            get { return _IS_SEEK_I6620; }
            set { SetProperty(ref _IS_SEEK_I6620, value, () => IS_SEEK_I6620); }
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
            //
            //품목
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s141", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    ItemsList = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                    if (ItemsList.Count > 0)
                    {
                        M_ITEM_NM = ItemsList[0];
                    }
                }
            }
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

            //    WinUIMessageBox.Show("", "[" + SystemProperties.PROGRAM_TITLE + "]창고간 이동", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);




            ////    masterDialog = new I6629MasterDialog(new InvVo());
            ////    masterDialog.Title = "품목 등록 - " + _title;
            ////    masterDialog.Owner = Application.Current.MainWindow;
            ////    masterDialog.BorderEffect = BorderEffect.Default;
            ////    ////masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            ////    ////masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            ////    bool isDialog = (bool)masterDialog.ShowDialog();
            ////    if (isDialog)
            ////    {
            ////    //    if (masterDialog.IsEdit == false)
            ////    //    {
            ////    //        try
            ////    //        {
            ////    //            string itmCd = SelectedMstItem.ITM_CD;

            ////    //            DXSplashScreen.Show<ProgressWindow>();
            ////                Refresh();
            ////    //            DXSplashScreen.Close();
            ////    //            //
            ////    //            //
            ////    //            for (int x = 0; x < SelectMstList.Count; x++)
            ////    //            {
            ////    //                if (itmCd.Equals(SelectMstList[x].ITM_CD))
            ////    //                {
            ////    //                    SelectedMstItem = SelectMstList[x];
            ////    //                    break;
            ////    //                }
            ////    //            }
            ////    //        }
            ////    //        catch (System.Exception eLog)
            ////    //        {
            ////    //            DXSplashScreen.Close();
            ////    //            WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]창고간 이동", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
            ////    //            return;
            ////    //        }
            ////    //    }
            ////    }
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

            //public void DelContact()
            //{
            //    InvVo delDao = SelectedMstItem;
            //    if (delDao != null)
            //    {
            //        MessageBoxResult result = WinUIMessageBox.Show("[" + delDao.MEGA_NO + "]" + " 정말로 삭제 하시겠습니까?", "[삭제]" + this._title, MessageBoxButton.YesNo, MessageBoxImage.Question);
            //        if (result == MessageBoxResult.Yes)
            //        {
            //            try
            //            {
            //                DXSplashScreen.Show<ProgressWindow>();
            //                invClient.I6629DeleteMst(delDao);
            //                Refresh();
            //                DXSplashScreen.Close();
            //                WinUIMessageBox.Show("삭제가 완료되었습니다.", "[삭제]" + this._title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
            //            }
            //            catch (System.Exception eLog)
            //            {
            //                DXSplashScreen.Close();
            //                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + this._title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
            //                return;
            //            }
            //        }
            //    }
            //}
        }
}
