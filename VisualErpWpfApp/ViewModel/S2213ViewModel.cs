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
using AquilaErpWpfApp3.Util;

namespace HyfErp.ViewModel
{
    public sealed class S2213ViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private string title = "세금계산서발행관리";

        //private static SaleOrderServiceClient saleOrderClient = SystemProperties.SaleOrderClient;
        private IList<SaleVo> selectedMstList = new List<SaleVo>();

        //private ObservableCollection<PurVo> selectedDtlList = new ObservableCollection<PurVo>();
        private IList<SaleVo> selectedDtlList = new List<SaleVo>();

        //private S2225ExcelDialog excelDialog;

        //private S2211MasterDialog masterDialog;
        //private S2211DetailDialog detailDialog;

        //private S2211DetailExcelDialog excelDialog;

        //private S2211MasterCopyDialog masterCopyDialog;

        //private string[,] _loadSave;

        ////Menu Dialog
        //private ICommand _searchDialogCommand;
        //private ICommand _newDialogCommand;
        //private ICommand _editDialogCommand;
        //private ICommand _delDialogCommand;
        //private ICommand _excelDtlDialogCommand;

        //private ICommand _copyDialogCommand;

        //private ICommand _searchDetailDialogCommand;

        //private ICommand reportDialogCommand;



        public S2213ViewModel()
        {
            StartDt = System.DateTime.Now;
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

            //Refresh();
        }

        [Command]
        public async void Refresh()
        {
            try
            {
                //DXSplashScreen.Show<ProgressWindow>();
                SearchDetail = null;
                SelectDtlList = null;

                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s2213/mst", new StringContent(JsonConvert.SerializeObject(new SaleVo() { FM_DT = (StartDt).ToString("yyyy-MM-dd"), TO_DT = (EndDt).ToString("yyyy-MM-dd"), SL_AREA_CD = M_SL_AREA_NM.CLSS_CD, CRE_USR_ID = SystemProperties.USER, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectMstList = JsonConvert.DeserializeObject<IEnumerable<SaleVo>>(await response.Content.ReadAsStringAsync()).Cast<SaleVo>().ToList();
                    }

                    //SelectMstList = saleOrderClient.S2213SelectMstList(new JobVo() { FM_DT = (StartDt).ToString("yyyy-MM-dd"), TO_DT = (EndDt).ToString("yyyy-MM-dd"), SL_AREA_CD = (string.IsNullOrEmpty(_AreaMap[TXT_SL_AREA_NM]) ? null : _AreaMap[TXT_SL_AREA_NM]), CRE_USR_ID = SystemProperties.USER });
                    ////
                    Title = "[기간]" + (StartDt).ToString("yyyy-MM-dd") + "~" + (EndDt).ToString("yyyy-MM-dd") + ",   [사업장]" + M_SL_AREA_NM.CLSS_DESC;

                    if (SelectMstList.Count >= 1)
                    {
                        isM_UPDATE = true;
                        isM_DELETE = true;

                        SelectedMstItem = SelectMstList[0];
                    }
                    else
                    {
                        SearchDetail = null;
                        SelectDtlList = null;

                        isM_UPDATE = false;
                        isM_DELETE = false;
                        //
                        isD_UPDATE = false;
                        isD_DELETE = false;
                    }
                }
                //DXSplashScreen.Close();
            }
            catch (System.Exception eLog)
            {
                //DXSplashScreen.Close();
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }


        //void Excel()
        //{
        //    try
        //    {
        //        string _path = System.Windows.Forms.Application.StartupPath + @"\cpcn.xls";
        //        if (System.IO.File.Exists(_path))
        //        {
        //            //파일 삭제
        //            System.IO.File.Delete(_path);
        //        }

        //        System.IO.File.Create(_path).Close();
        //        string _areaCd ="";
        //        DateTime _slYrMon = DateTime.Now;
        //        string _bilCd = "";
        //        string _grpBilNo = null;
        //        string _clzFlg = null;

        //        excelDialog = new S2225ExcelDialog(_path, _areaCd, _slYrMon, _bilCd, _grpBilNo,_clzFlg);
        //        excelDialog.Title = "[엑셀]" + this.title;
        //        excelDialog.Owner = Application.Current.MainWindow;
        //        excelDialog.BorderEffect = BorderEffect.Default;
        //        excelDialog.BorderEffectActiveColor = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 128, 0));
        //        excelDialog.BorderEffectInactiveColor = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 170, 170));
        //        bool isDialog = (bool)excelDialog.ShowDialog();
        //        //if (isDialog)
        //        {
        //            if (System.IO.File.Exists(_path))
        //            {
        //                //파일 삭제
        //                System.IO.File.Delete(_path);
        //            }
        //            Refresh();
        //        }

        //    }
        //    catch (System.Exception eLog)
        //    {
        //        WinUIMessageBox.Show(eLog.Message, "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
        //        return;
        //    }


        //}

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
            set { SetProperty(ref _M_SL_AREA_NMT, value, () => M_SL_AREA_NM); }
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
                    SetProperty(ref _selectedMstItem, value, () => SelectedMstItem, SelectMstDetail);
                }
            }
        }

        SaleVo _selectedMstViewItem;
        public SaleVo SelectedMstViewItem
        {
            get
            {
                return _selectedMstViewItem;
            }
            set
            {
                if (value != null)
                {
                    SetProperty(ref _selectedMstViewItem, value, () => SelectedMstViewItem);
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

                //SelectedMstViewItem = saleOrderClient.S2213SelectMstView(SelectedMstItem);
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s2213/mst/view", new StringContent(JsonConvert.SerializeObject(SelectedMstItem), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectedMstViewItem = JsonConvert.DeserializeObject<SaleVo>(await response.Content.ReadAsStringAsync());
                    }
                }

                //SelectDtlList = saleOrderClient.S2213SelectDtlList(SelectedMstItem);
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s2213/dtl", new StringContent(JsonConvert.SerializeObject(SelectedMstItem), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectDtlList = JsonConvert.DeserializeObject<IEnumerable<SaleVo>>(await response.Content.ReadAsStringAsync()).Cast<SaleVo>().ToList();
                    }

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

        //public ICommand ExcelDtlDialogCommand
        //{
        //    get
        //    {
        //        if (_excelDtlDialogCommand == null)
        //            _excelDtlDialogCommand = new DelegateCommand(Excel);
        //        return _excelDtlDialogCommand;
        //    }
        //}

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
        //    //if (SelectedMstItem == null)
        //    //{
        //    //    return;
        //    //}

        //    //string PRN_FLG = saleOrderClient.S2213SelectCheck(SelectedMstItem).PRN_FLG;

        //    //if (PRN_FLG.Equals("Y"))
        //    //{
        //    //    WinUIMessageBox.Show("거래명세서가 발행 되었습니다\r\n삭제 하실 수 없습니다", "[삭제]" + title, MessageBoxButton.OK, MessageBoxImage.Information);
        //    //    return;
        //    //}


        //    //MessageBoxResult result = WinUIMessageBox.Show("[" + SelectedMstItem.SL_BIL_NO + "]" + " 정말로 삭제 하시겠습니까?", "[삭제]" + title, MessageBoxButton.YesNo, MessageBoxImage.Question);
        //    //if (result == MessageBoxResult.Yes)
        //    //{
        //    //    //JobVo resultVo = saleOrderClient.S2213DeleteDtl(SelectedMstItem);
        //    //    JobVo resultVo = saleOrderClient.ProcS2213(SelectedMstItem);
        //    //    if (!resultVo.isSuccess)
        //    //    {
        //    //        //실패
        //    //        WinUIMessageBox.Show(resultVo.Message, "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
        //    //        return;
        //    //    }
        //    //}
        //}

        public async void SYSTEM_CODE_VO()
        {

            ////사업장
            //AreaList = SystemProperties.SYSTEM_CODE_VO("L-002");
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + AquilaErpWpfApp3.Properties.Settings.Default.SettingChnl + "/" + "L-002"))
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


        #region MyRegion
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
        //    if (SelectedMstItem == null) { return; }

        //    SelectedMstItem.CRE_USR_ID = SystemProperties.USER;

        //    ObservableCollection<SpecificationOnTransVo> reportItems = new ObservableCollection<SpecificationOnTransVo>();
        //    ObservableCollection<SpecificationOnTransVo> allItems = new ObservableCollection<SpecificationOnTransVo>(saleOrderClient.S221SelectMstSpecificationOnTransReportList(SelectedMstItem));

        //    string PRN_FLG = saleOrderClient.S2213SelectCheck(SelectedMstItem).PRN_FLG;


        //    if (allItems == null)
        //    {
        //        return;
        //    }
        //    else if (allItems.Count <= 0)
        //    {
        //        WinUIMessageBox.Show("데이터가 존재 하지 않습니다.", "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
        //        return;
        //    }
        //    else if (allItems.Count <= 10)
        //    {
        //        reportItems.Add(new SpecificationOnTransVo());
        //        for (int x = 0; x < allItems.Count; x++)
        //        {
        //            if (x == 0)
        //            {
        //                reportItems[0].A1 = allItems[x].RN;
        //                reportItems[0].A2 = allItems[x].ITM_NM;
        //                reportItems[0].A3 = allItems[x].ITM_SZ_NM;
        //                reportItems[0].A4 = allItems[x].SL_ITM_QTY;
        //                reportItems[0].A5 = allItems[x].SL_ITM_PRC_DC_VAL;
        //                reportItems[0].A6 = allItems[x].SL_ITM_AMT;
        //                reportItems[0].A7 = allItems[x].SL_ITM_TAX_AMT;
        //                reportItems[0].A8 = allItems[x].MD_QTY;

        //                //
        //                reportItems[0].O1 = reportItems[0].A6;
        //                reportItems[0].O2 = reportItems[0].A7;
        //                reportItems[0].O3 = float.Parse(reportItems[0].O1.ToString()) + float.Parse(reportItems[0].O2.ToString());
        //                //
        //                reportItems[0].B_CO_RGST_NO = allItems[x].B_CO_RGST_NO;
        //                reportItems[0].B_CO_NM = allItems[x].B_CO_NM;
        //                reportItems[0].B_PRSD_NM = allItems[x].B_PRSD_NM;
        //                reportItems[0].B_HDQTR_ADDR = allItems[x].B_HDQTR_ADDR;
        //                reportItems[0].B_N1ST_BIZ_COND_NM = allItems[x].B_N1ST_BIZ_COND_NM;
        //                reportItems[0].B_N1ST_BZTP_NM = allItems[x].B_N1ST_BZTP_NM;
        //                reportItems[0].B_HDQTR_PHN_NO = allItems[x].B_HDQTR_PHN_NO;
        //                reportItems[0].B_HDQTR_FAX_NO = allItems[x].B_HDQTR_FAX_NO;
        //                //
        //                reportItems[0].C_CO_RGST_NO = allItems[x].C_CO_RGST_NO;
        //                reportItems[0].C_CO_NM = allItems[x].C_CO_NM;
        //                reportItems[0].C_PRSD_NM = allItems[x].C_PRSD_NM;
        //                reportItems[0].C_HDQTR_ADDR = allItems[x].C_HDQTR_ADDR;
        //                reportItems[0].C_N1ST_BIZ_COND_NM = allItems[x].C_N1ST_BIZ_COND_NM;
        //                reportItems[0].C_N1ST_BZTP_NM = allItems[x].C_N1ST_BZTP_NM;
        //                reportItems[0].C_HDQTR_PHN_NO = allItems[x].C_HDQTR_PHN_NO;
        //                reportItems[0].C_HDQTR_FAX_NO = allItems[x].C_HDQTR_FAX_NO;

        //                reportItems[0].SL_BIL_RMK = "비 고 : " + allItems[x].SL_BIL_RMK;
        //                reportItems[0].SL_BIL_NO = allItems[x].SL_BIL_NO;
        //                reportItems[0].PRN_DT = "출력일시 : " + allItems[x].PRN_DT;
        //                reportItems[0].CRE_USR_NM = allItems[x].CRE_USR_NM;

        //                reportItems[0].SL_NXT_CLZ_FLG = allItems[x].SL_NXT_CLZ_FLG;
        //                reportItems[0].NXT_MON_DT = "이월일자 : " + allItems[x].NXT_MON_DT;
        //            }
        //            else if (x == 1)
        //            {
        //                reportItems[0].B1 = allItems[x].RN;
        //                reportItems[0].B2 = allItems[x].ITM_NM;
        //                reportItems[0].B3 = allItems[x].ITM_SZ_NM;
        //                reportItems[0].B4 = allItems[x].SL_ITM_QTY;
        //                reportItems[0].B5 = allItems[x].SL_ITM_PRC_DC_VAL;
        //                reportItems[0].B6 = allItems[x].SL_ITM_AMT;
        //                reportItems[0].B7 = allItems[x].SL_ITM_TAX_AMT;
        //                reportItems[0].B8 = allItems[x].MD_QTY;

        //                ////
        //                reportItems[0].O1 = (string.IsNullOrEmpty(reportItems[0].A6.ToString()) ? 0 : float.Parse(reportItems[0].A6.ToString()))
        //                              + (string.IsNullOrEmpty(reportItems[0].B6.ToString()) ? 0 : float.Parse(reportItems[0].B6.ToString()));
        //                //+ (string.IsNullOrEmpty(reportItems[0].C6.ToString()) ? 0 : float.Parse(reportItems[0].C6.ToString()))
        //                //+ (string.IsNullOrEmpty(reportItems[0].D6.ToString()) ? 0 : float.Parse(reportItems[0].D6.ToString()))
        //                //+ (string.IsNullOrEmpty(reportItems[0].E6.ToString()) ? 0 : float.Parse(reportItems[0].E6.ToString()))
        //                //+ (string.IsNullOrEmpty(reportItems[0].F6.ToString()) ? 0 : float.Parse(reportItems[0].F6.ToString()))
        //                //+ (string.IsNullOrEmpty(reportItems[0].G6.ToString()) ? 0 : float.Parse(reportItems[0].G6.ToString()))
        //                //+ (string.IsNullOrEmpty(reportItems[0].H6.ToString()) ? 0 : float.Parse(reportItems[0].H6.ToString()))
        //                //+ (string.IsNullOrEmpty(reportItems[0].I6.ToString()) ? 0 : float.Parse(reportItems[0].I6.ToString()))
        //                //+ (string.IsNullOrEmpty(reportItems[0].J6.ToString()) ? 0 : float.Parse(reportItems[0].J6.ToString()));


        //                reportItems[0].O2 = (string.IsNullOrEmpty(reportItems[0].A7.ToString()) ? 0 : float.Parse(reportItems[0].A7.ToString()))
        //                                  + (string.IsNullOrEmpty(reportItems[0].B7.ToString()) ? 0 : float.Parse(reportItems[0].B7.ToString()));
        //                //+ (string.IsNullOrEmpty(reportItems[0].C7.ToString()) ? 0 : float.Parse(reportItems[0].C7.ToString()))
        //                //+ (string.IsNullOrEmpty(reportItems[0].D7.ToString()) ? 0 : float.Parse(reportItems[0].D7.ToString()))
        //                //+ (string.IsNullOrEmpty(reportItems[0].E7.ToString()) ? 0 : float.Parse(reportItems[0].E7.ToString()))
        //                //+ (string.IsNullOrEmpty(reportItems[0].F7.ToString()) ? 0 : float.Parse(reportItems[0].F7.ToString()))
        //                //+ (string.IsNullOrEmpty(reportItems[0].G7.ToString()) ? 0 : float.Parse(reportItems[0].G7.ToString()))
        //                //+ (string.IsNullOrEmpty(reportItems[0].H7.ToString()) ? 0 : float.Parse(reportItems[0].H7.ToString()))
        //                //+ (string.IsNullOrEmpty(reportItems[0].I7.ToString()) ? 0 : float.Parse(reportItems[0].I7.ToString()))
        //                //+ (string.IsNullOrEmpty(reportItems[0].J7.ToString()) ? 0 : float.Parse(reportItems[0].J7.ToString()));

        //                reportItems[0].O3 = (string.IsNullOrEmpty(reportItems[0].O1.ToString()) ? 0 : float.Parse(reportItems[0].O1.ToString())) + (string.IsNullOrEmpty(reportItems[0].O2.ToString()) ? 0 : float.Parse(reportItems[0].O2.ToString()));

        //            }
        //            else if (x == 2)
        //            {
        //                reportItems[0].C1 = allItems[x].RN;
        //                reportItems[0].C2 = allItems[x].ITM_NM;
        //                reportItems[0].C3 = allItems[x].ITM_SZ_NM;
        //                reportItems[0].C4 = allItems[x].SL_ITM_QTY;
        //                reportItems[0].C5 = allItems[x].SL_ITM_PRC_DC_VAL;
        //                reportItems[0].C6 = allItems[x].SL_ITM_AMT;
        //                reportItems[0].C7 = allItems[x].SL_ITM_TAX_AMT;
        //                reportItems[0].C8 = allItems[x].MD_QTY;
        //                ////
        //                reportItems[0].O1 = (string.IsNullOrEmpty(reportItems[0].A6.ToString()) ? 0 : float.Parse(reportItems[0].A6.ToString()))
        //                              + (string.IsNullOrEmpty(reportItems[0].B6.ToString()) ? 0 : float.Parse(reportItems[0].B6.ToString()))
        //                              + (string.IsNullOrEmpty(reportItems[0].C6.ToString()) ? 0 : float.Parse(reportItems[0].C6.ToString()));
        //                //+ (string.IsNullOrEmpty(reportItems[0].D6.ToString()) ? 0 : float.Parse(reportItems[0].D6.ToString()))
        //                //+ (string.IsNullOrEmpty(reportItems[0].E6.ToString()) ? 0 : float.Parse(reportItems[0].E6.ToString()))
        //                //+ (string.IsNullOrEmpty(reportItems[0].F6.ToString()) ? 0 : float.Parse(reportItems[0].F6.ToString()))
        //                //+ (string.IsNullOrEmpty(reportItems[0].G6.ToString()) ? 0 : float.Parse(reportItems[0].G6.ToString()))
        //                //+ (string.IsNullOrEmpty(reportItems[0].H6.ToString()) ? 0 : float.Parse(reportItems[0].H6.ToString()))
        //                //+ (string.IsNullOrEmpty(reportItems[0].I6.ToString()) ? 0 : float.Parse(reportItems[0].I6.ToString()))
        //                //+ (string.IsNullOrEmpty(reportItems[0].J6.ToString()) ? 0 : float.Parse(reportItems[0].J6.ToString()));


        //                reportItems[0].O2 = (string.IsNullOrEmpty(reportItems[0].A7.ToString()) ? 0 : float.Parse(reportItems[0].A7.ToString()))
        //                                  + (string.IsNullOrEmpty(reportItems[0].B7.ToString()) ? 0 : float.Parse(reportItems[0].B7.ToString()))
        //                                  + (string.IsNullOrEmpty(reportItems[0].C7.ToString()) ? 0 : float.Parse(reportItems[0].C7.ToString()));
        //                //+ (string.IsNullOrEmpty(reportItems[0].D7.ToString()) ? 0 : float.Parse(reportItems[0].D7.ToString()))
        //                //+ (string.IsNullOrEmpty(reportItems[0].E7.ToString()) ? 0 : float.Parse(reportItems[0].E7.ToString()))
        //                //+ (string.IsNullOrEmpty(reportItems[0].F7.ToString()) ? 0 : float.Parse(reportItems[0].F7.ToString()))
        //                //+ (string.IsNullOrEmpty(reportItems[0].G7.ToString()) ? 0 : float.Parse(reportItems[0].G7.ToString()))
        //                //+ (string.IsNullOrEmpty(reportItems[0].H7.ToString()) ? 0 : float.Parse(reportItems[0].H7.ToString()))
        //                //+ (string.IsNullOrEmpty(reportItems[0].I7.ToString()) ? 0 : float.Parse(reportItems[0].I7.ToString()))
        //                //+ (string.IsNullOrEmpty(reportItems[0].J7.ToString()) ? 0 : float.Parse(reportItems[0].J7.ToString()));

        //                reportItems[0].O3 = (string.IsNullOrEmpty(reportItems[0].O1.ToString()) ? 0 : float.Parse(reportItems[0].O1.ToString())) + (string.IsNullOrEmpty(reportItems[0].O2.ToString()) ? 0 : float.Parse(reportItems[0].O2.ToString()));

        //            }
        //            else if (x == 3)
        //            {
        //                reportItems[0].D1 = allItems[x].RN;
        //                reportItems[0].D2 = allItems[x].ITM_NM;
        //                reportItems[0].D3 = allItems[x].ITM_SZ_NM;
        //                reportItems[0].D4 = allItems[x].SL_ITM_QTY;
        //                reportItems[0].D5 = allItems[x].SL_ITM_PRC_DC_VAL;
        //                reportItems[0].D6 = allItems[x].SL_ITM_AMT;
        //                reportItems[0].D7 = allItems[x].SL_ITM_TAX_AMT;
        //                reportItems[0].D8 = allItems[x].MD_QTY;
        //                ////
        //                reportItems[0].O1 = (string.IsNullOrEmpty(reportItems[0].A6.ToString()) ? 0 : float.Parse(reportItems[0].A6.ToString()))
        //                              + (string.IsNullOrEmpty(reportItems[0].B6.ToString()) ? 0 : float.Parse(reportItems[0].B6.ToString()))
        //                              + (string.IsNullOrEmpty(reportItems[0].C6.ToString()) ? 0 : float.Parse(reportItems[0].C6.ToString()))
        //                              + (string.IsNullOrEmpty(reportItems[0].D6.ToString()) ? 0 : float.Parse(reportItems[0].D6.ToString()));
        //                //+ (string.IsNullOrEmpty(reportItems[0].E6.ToString()) ? 0 : float.Parse(reportItems[0].E6.ToString()))
        //                //+ (string.IsNullOrEmpty(reportItems[0].F6.ToString()) ? 0 : float.Parse(reportItems[0].F6.ToString()))
        //                //+ (string.IsNullOrEmpty(reportItems[0].G6.ToString()) ? 0 : float.Parse(reportItems[0].G6.ToString()))
        //                //+ (string.IsNullOrEmpty(reportItems[0].H6.ToString()) ? 0 : float.Parse(reportItems[0].H6.ToString()))
        //                //+ (string.IsNullOrEmpty(reportItems[0].I6.ToString()) ? 0 : float.Parse(reportItems[0].I6.ToString()))
        //                //+ (string.IsNullOrEmpty(reportItems[0].J6.ToString()) ? 0 : float.Parse(reportItems[0].J6.ToString()));


        //                reportItems[0].O2 = (string.IsNullOrEmpty(reportItems[0].A7.ToString()) ? 0 : float.Parse(reportItems[0].A7.ToString()))
        //                                  + (string.IsNullOrEmpty(reportItems[0].B7.ToString()) ? 0 : float.Parse(reportItems[0].B7.ToString()))
        //                                  + (string.IsNullOrEmpty(reportItems[0].C7.ToString()) ? 0 : float.Parse(reportItems[0].C7.ToString()))
        //                                  + (string.IsNullOrEmpty(reportItems[0].D7.ToString()) ? 0 : float.Parse(reportItems[0].D7.ToString()));
        //                //+ (string.IsNullOrEmpty(reportItems[0].E7.ToString()) ? 0 : float.Parse(reportItems[0].E7.ToString()))
        //                //+ (string.IsNullOrEmpty(reportItems[0].F7.ToString()) ? 0 : float.Parse(reportItems[0].F7.ToString()))
        //                //+ (string.IsNullOrEmpty(reportItems[0].G7.ToString()) ? 0 : float.Parse(reportItems[0].G7.ToString()))
        //                //+ (string.IsNullOrEmpty(reportItems[0].H7.ToString()) ? 0 : float.Parse(reportItems[0].H7.ToString()))
        //                //+ (string.IsNullOrEmpty(reportItems[0].I7.ToString()) ? 0 : float.Parse(reportItems[0].I7.ToString()))
        //                //+ (string.IsNullOrEmpty(reportItems[0].J7.ToString()) ? 0 : float.Parse(reportItems[0].J7.ToString()));

        //                reportItems[0].O3 = (string.IsNullOrEmpty(reportItems[0].O1.ToString()) ? 0 : float.Parse(reportItems[0].O1.ToString())) + (string.IsNullOrEmpty(reportItems[0].O2.ToString()) ? 0 : float.Parse(reportItems[0].O2.ToString()));

        //            }
        //            else if (x == 4)
        //            {
        //                reportItems[0].E1 = allItems[x].RN;
        //                reportItems[0].E2 = allItems[x].ITM_NM;
        //                reportItems[0].E3 = allItems[x].ITM_SZ_NM;
        //                reportItems[0].E4 = allItems[x].SL_ITM_QTY;
        //                reportItems[0].E5 = allItems[x].SL_ITM_PRC_DC_VAL;
        //                reportItems[0].E6 = allItems[x].SL_ITM_AMT;
        //                reportItems[0].E7 = allItems[x].SL_ITM_TAX_AMT;
        //                reportItems[0].E8 = allItems[x].MD_QTY;
        //                ////
        //                reportItems[0].O1 = (string.IsNullOrEmpty(reportItems[0].A6.ToString()) ? 0 : float.Parse(reportItems[0].A6.ToString()))
        //                              + (string.IsNullOrEmpty(reportItems[0].B6.ToString()) ? 0 : float.Parse(reportItems[0].B6.ToString()))
        //                              + (string.IsNullOrEmpty(reportItems[0].C6.ToString()) ? 0 : float.Parse(reportItems[0].C6.ToString()))
        //                              + (string.IsNullOrEmpty(reportItems[0].D6.ToString()) ? 0 : float.Parse(reportItems[0].D6.ToString()))
        //                              + (string.IsNullOrEmpty(reportItems[0].E6.ToString()) ? 0 : float.Parse(reportItems[0].E6.ToString()));
        //                //+ (string.IsNullOrEmpty(reportItems[0].F6.ToString()) ? 0 : float.Parse(reportItems[0].F6.ToString()))
        //                //+ (string.IsNullOrEmpty(reportItems[0].G6.ToString()) ? 0 : float.Parse(reportItems[0].G6.ToString()))
        //                //+ (string.IsNullOrEmpty(reportItems[0].H6.ToString()) ? 0 : float.Parse(reportItems[0].H6.ToString()))
        //                //+ (string.IsNullOrEmpty(reportItems[0].I6.ToString()) ? 0 : float.Parse(reportItems[0].I6.ToString()))
        //                //+ (string.IsNullOrEmpty(reportItems[0].J6.ToString()) ? 0 : float.Parse(reportItems[0].J6.ToString()));


        //                reportItems[0].O2 = (string.IsNullOrEmpty(reportItems[0].A7.ToString()) ? 0 : float.Parse(reportItems[0].A7.ToString()))
        //                                  + (string.IsNullOrEmpty(reportItems[0].B7.ToString()) ? 0 : float.Parse(reportItems[0].B7.ToString()))
        //                                  + (string.IsNullOrEmpty(reportItems[0].C7.ToString()) ? 0 : float.Parse(reportItems[0].C7.ToString()))
        //                                  + (string.IsNullOrEmpty(reportItems[0].D7.ToString()) ? 0 : float.Parse(reportItems[0].D7.ToString()))
        //                                  + (string.IsNullOrEmpty(reportItems[0].E7.ToString()) ? 0 : float.Parse(reportItems[0].E7.ToString()));
        //                //+ (string.IsNullOrEmpty(reportItems[0].F7.ToString()) ? 0 : float.Parse(reportItems[0].F7.ToString()))
        //                //+ (string.IsNullOrEmpty(reportItems[0].G7.ToString()) ? 0 : float.Parse(reportItems[0].G7.ToString()))
        //                //+ (string.IsNullOrEmpty(reportItems[0].H7.ToString()) ? 0 : float.Parse(reportItems[0].H7.ToString()))
        //                //+ (string.IsNullOrEmpty(reportItems[0].I7.ToString()) ? 0 : float.Parse(reportItems[0].I7.ToString()))
        //                //+ (string.IsNullOrEmpty(reportItems[0].J7.ToString()) ? 0 : float.Parse(reportItems[0].J7.ToString()));

        //                reportItems[0].O3 = (string.IsNullOrEmpty(reportItems[0].O1.ToString()) ? 0 : float.Parse(reportItems[0].O1.ToString())) + (string.IsNullOrEmpty(reportItems[0].O2.ToString()) ? 0 : float.Parse(reportItems[0].O2.ToString()));

        //            }
        //            else if (x == 5)
        //            {
        //                reportItems[0].F1 = allItems[x].RN;
        //                reportItems[0].F2 = allItems[x].ITM_NM;
        //                reportItems[0].F3 = allItems[x].ITM_SZ_NM;
        //                reportItems[0].F4 = allItems[x].SL_ITM_QTY;
        //                reportItems[0].F5 = allItems[x].SL_ITM_PRC_DC_VAL;
        //                reportItems[0].F6 = allItems[x].SL_ITM_AMT;
        //                reportItems[0].F7 = allItems[x].SL_ITM_TAX_AMT;
        //                reportItems[0].F8 = allItems[x].MD_QTY;
        //                ////
        //                reportItems[0].O1 = (string.IsNullOrEmpty(reportItems[0].A6.ToString()) ? 0 : float.Parse(reportItems[0].A6.ToString()))
        //                              + (string.IsNullOrEmpty(reportItems[0].B6.ToString()) ? 0 : float.Parse(reportItems[0].B6.ToString()))
        //                              + (string.IsNullOrEmpty(reportItems[0].C6.ToString()) ? 0 : float.Parse(reportItems[0].C6.ToString()))
        //                              + (string.IsNullOrEmpty(reportItems[0].D6.ToString()) ? 0 : float.Parse(reportItems[0].D6.ToString()))
        //                              + (string.IsNullOrEmpty(reportItems[0].E6.ToString()) ? 0 : float.Parse(reportItems[0].E6.ToString()))
        //                              + (string.IsNullOrEmpty(reportItems[0].F6.ToString()) ? 0 : float.Parse(reportItems[0].F6.ToString()));
        //                //+ (string.IsNullOrEmpty(reportItems[0].G6.ToString()) ? 0 : float.Parse(reportItems[0].G6.ToString()))
        //                //+ (string.IsNullOrEmpty(reportItems[0].H6.ToString()) ? 0 : float.Parse(reportItems[0].H6.ToString()))
        //                //+ (string.IsNullOrEmpty(reportItems[0].I6.ToString()) ? 0 : float.Parse(reportItems[0].I6.ToString()))
        //                //+ (string.IsNullOrEmpty(reportItems[0].J6.ToString()) ? 0 : float.Parse(reportItems[0].J6.ToString()));


        //                reportItems[0].O2 = (string.IsNullOrEmpty(reportItems[0].A7.ToString()) ? 0 : float.Parse(reportItems[0].A7.ToString()))
        //                                  + (string.IsNullOrEmpty(reportItems[0].B7.ToString()) ? 0 : float.Parse(reportItems[0].B7.ToString()))
        //                                  + (string.IsNullOrEmpty(reportItems[0].C7.ToString()) ? 0 : float.Parse(reportItems[0].C7.ToString()))
        //                                  + (string.IsNullOrEmpty(reportItems[0].D7.ToString()) ? 0 : float.Parse(reportItems[0].D7.ToString()))
        //                                  + (string.IsNullOrEmpty(reportItems[0].E7.ToString()) ? 0 : float.Parse(reportItems[0].E7.ToString()))
        //                                  + (string.IsNullOrEmpty(reportItems[0].F7.ToString()) ? 0 : float.Parse(reportItems[0].F7.ToString()));
        //                //+ (string.IsNullOrEmpty(reportItems[0].G7.ToString()) ? 0 : float.Parse(reportItems[0].G7.ToString()))
        //                //+ (string.IsNullOrEmpty(reportItems[0].H7.ToString()) ? 0 : float.Parse(reportItems[0].H7.ToString()))
        //                //+ (string.IsNullOrEmpty(reportItems[0].I7.ToString()) ? 0 : float.Parse(reportItems[0].I7.ToString()))
        //                //+ (string.IsNullOrEmpty(reportItems[0].J7.ToString()) ? 0 : float.Parse(reportItems[0].J7.ToString()));

        //                reportItems[0].O3 = (string.IsNullOrEmpty(reportItems[0].O1.ToString()) ? 0 : float.Parse(reportItems[0].O1.ToString())) + (string.IsNullOrEmpty(reportItems[0].O2.ToString()) ? 0 : float.Parse(reportItems[0].O2.ToString()));

        //            }
        //            else if (x == 6)
        //            {
        //                reportItems[0].G1 = allItems[x].RN;
        //                reportItems[0].G2 = allItems[x].ITM_NM;
        //                reportItems[0].G3 = allItems[x].ITM_SZ_NM;
        //                reportItems[0].G4 = allItems[x].SL_ITM_QTY;
        //                reportItems[0].G5 = allItems[x].SL_ITM_PRC_DC_VAL;
        //                reportItems[0].G6 = allItems[x].SL_ITM_AMT;
        //                reportItems[0].G7 = allItems[x].SL_ITM_TAX_AMT;
        //                reportItems[0].G8 = allItems[x].MD_QTY;
        //                ////
        //                reportItems[0].O1 = (string.IsNullOrEmpty(reportItems[0].A6.ToString()) ? 0 : float.Parse(reportItems[0].A6.ToString()))
        //                              + (string.IsNullOrEmpty(reportItems[0].B6.ToString()) ? 0 : float.Parse(reportItems[0].B6.ToString()))
        //                              + (string.IsNullOrEmpty(reportItems[0].C6.ToString()) ? 0 : float.Parse(reportItems[0].C6.ToString()))
        //                              + (string.IsNullOrEmpty(reportItems[0].D6.ToString()) ? 0 : float.Parse(reportItems[0].D6.ToString()))
        //                              + (string.IsNullOrEmpty(reportItems[0].E6.ToString()) ? 0 : float.Parse(reportItems[0].E6.ToString()))
        //                              + (string.IsNullOrEmpty(reportItems[0].F6.ToString()) ? 0 : float.Parse(reportItems[0].F6.ToString()))
        //                              + (string.IsNullOrEmpty(reportItems[0].G6.ToString()) ? 0 : float.Parse(reportItems[0].G6.ToString()));
        //                //+ (string.IsNullOrEmpty(reportItems[0].H6.ToString()) ? 0 : float.Parse(reportItems[0].H6.ToString()))
        //                //+ (string.IsNullOrEmpty(reportItems[0].I6.ToString()) ? 0 : float.Parse(reportItems[0].I6.ToString()))
        //                //+ (string.IsNullOrEmpty(reportItems[0].J6.ToString()) ? 0 : float.Parse(reportItems[0].J6.ToString()));


        //                reportItems[0].O2 = (string.IsNullOrEmpty(reportItems[0].A7.ToString()) ? 0 : float.Parse(reportItems[0].A7.ToString()))
        //                                  + (string.IsNullOrEmpty(reportItems[0].B7.ToString()) ? 0 : float.Parse(reportItems[0].B7.ToString()))
        //                                  + (string.IsNullOrEmpty(reportItems[0].C7.ToString()) ? 0 : float.Parse(reportItems[0].C7.ToString()))
        //                                  + (string.IsNullOrEmpty(reportItems[0].D7.ToString()) ? 0 : float.Parse(reportItems[0].D7.ToString()))
        //                                  + (string.IsNullOrEmpty(reportItems[0].E7.ToString()) ? 0 : float.Parse(reportItems[0].E7.ToString()))
        //                                  + (string.IsNullOrEmpty(reportItems[0].F7.ToString()) ? 0 : float.Parse(reportItems[0].F7.ToString()))
        //                                  + (string.IsNullOrEmpty(reportItems[0].G7.ToString()) ? 0 : float.Parse(reportItems[0].G7.ToString()));
        //                //+ (string.IsNullOrEmpty(reportItems[0].H7.ToString()) ? 0 : float.Parse(reportItems[0].H7.ToString()))
        //                //+ (string.IsNullOrEmpty(reportItems[0].I7.ToString()) ? 0 : float.Parse(reportItems[0].I7.ToString()))
        //                //+ (string.IsNullOrEmpty(reportItems[0].J7.ToString()) ? 0 : float.Parse(reportItems[0].J7.ToString()));

        //                reportItems[0].O3 = (string.IsNullOrEmpty(reportItems[0].O1.ToString()) ? 0 : float.Parse(reportItems[0].O1.ToString())) + (string.IsNullOrEmpty(reportItems[0].O2.ToString()) ? 0 : float.Parse(reportItems[0].O2.ToString()));

        //            }
        //            else if (x == 7)
        //            {
        //                reportItems[0].H1 = allItems[x].RN;
        //                reportItems[0].H2 = allItems[x].ITM_NM;
        //                reportItems[0].H3 = allItems[x].ITM_SZ_NM;
        //                reportItems[0].H4 = allItems[x].SL_ITM_QTY;
        //                reportItems[0].H5 = allItems[x].SL_ITM_PRC_DC_VAL;
        //                reportItems[0].H6 = allItems[x].SL_ITM_AMT;
        //                reportItems[0].H7 = allItems[x].SL_ITM_TAX_AMT;
        //                reportItems[0].H8 = allItems[x].MD_QTY;
        //                ////
        //                reportItems[0].O1 = (string.IsNullOrEmpty(reportItems[0].A6.ToString()) ? 0 : float.Parse(reportItems[0].A6.ToString()))
        //                              + (string.IsNullOrEmpty(reportItems[0].B6.ToString()) ? 0 : float.Parse(reportItems[0].B6.ToString()))
        //                              + (string.IsNullOrEmpty(reportItems[0].C6.ToString()) ? 0 : float.Parse(reportItems[0].C6.ToString()))
        //                              + (string.IsNullOrEmpty(reportItems[0].D6.ToString()) ? 0 : float.Parse(reportItems[0].D6.ToString()))
        //                              + (string.IsNullOrEmpty(reportItems[0].E6.ToString()) ? 0 : float.Parse(reportItems[0].E6.ToString()))
        //                              + (string.IsNullOrEmpty(reportItems[0].F6.ToString()) ? 0 : float.Parse(reportItems[0].F6.ToString()))
        //                              + (string.IsNullOrEmpty(reportItems[0].G6.ToString()) ? 0 : float.Parse(reportItems[0].G6.ToString()))
        //                              + (string.IsNullOrEmpty(reportItems[0].H6.ToString()) ? 0 : float.Parse(reportItems[0].H6.ToString()));
        //                //+ (string.IsNullOrEmpty(reportItems[0].I6.ToString()) ? 0 : float.Parse(reportItems[0].I6.ToString()))
        //                //+ (string.IsNullOrEmpty(reportItems[0].J6.ToString()) ? 0 : float.Parse(reportItems[0].J6.ToString()));


        //                reportItems[0].O2 = (string.IsNullOrEmpty(reportItems[0].A7.ToString()) ? 0 : float.Parse(reportItems[0].A7.ToString()))
        //                                  + (string.IsNullOrEmpty(reportItems[0].B7.ToString()) ? 0 : float.Parse(reportItems[0].B7.ToString()))
        //                                  + (string.IsNullOrEmpty(reportItems[0].C7.ToString()) ? 0 : float.Parse(reportItems[0].C7.ToString()))
        //                                  + (string.IsNullOrEmpty(reportItems[0].D7.ToString()) ? 0 : float.Parse(reportItems[0].D7.ToString()))
        //                                  + (string.IsNullOrEmpty(reportItems[0].E7.ToString()) ? 0 : float.Parse(reportItems[0].E7.ToString()))
        //                                  + (string.IsNullOrEmpty(reportItems[0].F7.ToString()) ? 0 : float.Parse(reportItems[0].F7.ToString()))
        //                                  + (string.IsNullOrEmpty(reportItems[0].G7.ToString()) ? 0 : float.Parse(reportItems[0].G7.ToString()))
        //                                  + (string.IsNullOrEmpty(reportItems[0].H7.ToString()) ? 0 : float.Parse(reportItems[0].H7.ToString()));
        //                //+ (string.IsNullOrEmpty(reportItems[0].I7.ToString()) ? 0 : float.Parse(reportItems[0].I7.ToString()))
        //                //+ (string.IsNullOrEmpty(reportItems[0].J7.ToString()) ? 0 : float.Parse(reportItems[0].J7.ToString()));

        //                reportItems[0].O3 = (string.IsNullOrEmpty(reportItems[0].O1.ToString()) ? 0 : float.Parse(reportItems[0].O1.ToString())) + (string.IsNullOrEmpty(reportItems[0].O2.ToString()) ? 0 : float.Parse(reportItems[0].O2.ToString()));

        //            }
        //            else if (x == 8)
        //            {
        //                reportItems[0].I1 = allItems[x].RN;
        //                reportItems[0].I2 = allItems[x].ITM_NM;
        //                reportItems[0].I3 = allItems[x].ITM_SZ_NM;
        //                reportItems[0].I4 = allItems[x].SL_ITM_QTY;
        //                reportItems[0].I5 = allItems[x].SL_ITM_PRC_DC_VAL;
        //                reportItems[0].I6 = allItems[x].SL_ITM_AMT;
        //                reportItems[0].I7 = allItems[x].SL_ITM_TAX_AMT;
        //                reportItems[0].I8 = allItems[x].MD_QTY;
        //                ////
        //                reportItems[0].O1 = (string.IsNullOrEmpty(reportItems[0].A6.ToString()) ? 0 : float.Parse(reportItems[0].A6.ToString()))
        //                              + (string.IsNullOrEmpty(reportItems[0].B6.ToString()) ? 0 : float.Parse(reportItems[0].B6.ToString()))
        //                              + (string.IsNullOrEmpty(reportItems[0].C6.ToString()) ? 0 : float.Parse(reportItems[0].C6.ToString()))
        //                              + (string.IsNullOrEmpty(reportItems[0].D6.ToString()) ? 0 : float.Parse(reportItems[0].D6.ToString()))
        //                              + (string.IsNullOrEmpty(reportItems[0].E6.ToString()) ? 0 : float.Parse(reportItems[0].E6.ToString()))
        //                              + (string.IsNullOrEmpty(reportItems[0].F6.ToString()) ? 0 : float.Parse(reportItems[0].F6.ToString()))
        //                              + (string.IsNullOrEmpty(reportItems[0].G6.ToString()) ? 0 : float.Parse(reportItems[0].G6.ToString()))
        //                              + (string.IsNullOrEmpty(reportItems[0].H6.ToString()) ? 0 : float.Parse(reportItems[0].H6.ToString()))
        //                              + (string.IsNullOrEmpty(reportItems[0].I6.ToString()) ? 0 : float.Parse(reportItems[0].I6.ToString()));
        //                //+ (string.IsNullOrEmpty(reportItems[0].J6.ToString()) ? 0 : float.Parse(reportItems[0].J6.ToString()));


        //                reportItems[0].O2 = (string.IsNullOrEmpty(reportItems[0].A7.ToString()) ? 0 : float.Parse(reportItems[0].A7.ToString()))
        //                                  + (string.IsNullOrEmpty(reportItems[0].B7.ToString()) ? 0 : float.Parse(reportItems[0].B7.ToString()))
        //                                  + (string.IsNullOrEmpty(reportItems[0].C7.ToString()) ? 0 : float.Parse(reportItems[0].C7.ToString()))
        //                                  + (string.IsNullOrEmpty(reportItems[0].D7.ToString()) ? 0 : float.Parse(reportItems[0].D7.ToString()))
        //                                  + (string.IsNullOrEmpty(reportItems[0].E7.ToString()) ? 0 : float.Parse(reportItems[0].E7.ToString()))
        //                                  + (string.IsNullOrEmpty(reportItems[0].F7.ToString()) ? 0 : float.Parse(reportItems[0].F7.ToString()))
        //                                  + (string.IsNullOrEmpty(reportItems[0].G7.ToString()) ? 0 : float.Parse(reportItems[0].G7.ToString()))
        //                                  + (string.IsNullOrEmpty(reportItems[0].H7.ToString()) ? 0 : float.Parse(reportItems[0].H7.ToString()))
        //                                  + (string.IsNullOrEmpty(reportItems[0].I7.ToString()) ? 0 : float.Parse(reportItems[0].I7.ToString()));
        //                //+ (string.IsNullOrEmpty(reportItems[0].J7.ToString()) ? 0 : float.Parse(reportItems[0].J7.ToString()));

        //                reportItems[0].O3 = (string.IsNullOrEmpty(reportItems[0].O1.ToString()) ? 0 : float.Parse(reportItems[0].O1.ToString())) + (string.IsNullOrEmpty(reportItems[0].O2.ToString()) ? 0 : float.Parse(reportItems[0].O2.ToString()));

        //            }
        //            else if (x == 9)
        //            {
        //                reportItems[0].J1 = allItems[x].RN;
        //                reportItems[0].J2 = allItems[x].ITM_NM;
        //                reportItems[0].J3 = allItems[x].ITM_SZ_NM;
        //                reportItems[0].J4 = allItems[x].SL_ITM_QTY;
        //                reportItems[0].J5 = allItems[x].SL_ITM_PRC_DC_VAL;
        //                reportItems[0].J6 = allItems[x].SL_ITM_AMT;
        //                reportItems[0].J7 = allItems[x].SL_ITM_TAX_AMT;
        //                reportItems[0].J8 = allItems[x].MD_QTY;
        //                ////
        //                reportItems[0].O1 = (string.IsNullOrEmpty(reportItems[0].A6.ToString()) ? 0 : float.Parse(reportItems[0].A6.ToString()))
        //                              + (string.IsNullOrEmpty(reportItems[0].B6.ToString()) ? 0 : float.Parse(reportItems[0].B6.ToString()))
        //                              + (string.IsNullOrEmpty(reportItems[0].C6.ToString()) ? 0 : float.Parse(reportItems[0].C6.ToString()))
        //                              + (string.IsNullOrEmpty(reportItems[0].D6.ToString()) ? 0 : float.Parse(reportItems[0].D6.ToString()))
        //                              + (string.IsNullOrEmpty(reportItems[0].E6.ToString()) ? 0 : float.Parse(reportItems[0].E6.ToString()))
        //                              + (string.IsNullOrEmpty(reportItems[0].F6.ToString()) ? 0 : float.Parse(reportItems[0].F6.ToString()))
        //                              + (string.IsNullOrEmpty(reportItems[0].G6.ToString()) ? 0 : float.Parse(reportItems[0].G6.ToString()))
        //                              + (string.IsNullOrEmpty(reportItems[0].H6.ToString()) ? 0 : float.Parse(reportItems[0].H6.ToString()))
        //                              + (string.IsNullOrEmpty(reportItems[0].I6.ToString()) ? 0 : float.Parse(reportItems[0].I6.ToString()))
        //                              + (string.IsNullOrEmpty(reportItems[0].J6.ToString()) ? 0 : float.Parse(reportItems[0].J6.ToString()));


        //                reportItems[0].O2 = (string.IsNullOrEmpty(reportItems[0].A7.ToString()) ? 0 : float.Parse(reportItems[0].A7.ToString()))
        //                                  + (string.IsNullOrEmpty(reportItems[0].B7.ToString()) ? 0 : float.Parse(reportItems[0].B7.ToString()))
        //                                  + (string.IsNullOrEmpty(reportItems[0].C7.ToString()) ? 0 : float.Parse(reportItems[0].C7.ToString()))
        //                                  + (string.IsNullOrEmpty(reportItems[0].D7.ToString()) ? 0 : float.Parse(reportItems[0].D7.ToString()))
        //                                  + (string.IsNullOrEmpty(reportItems[0].E7.ToString()) ? 0 : float.Parse(reportItems[0].E7.ToString()))
        //                                  + (string.IsNullOrEmpty(reportItems[0].F7.ToString()) ? 0 : float.Parse(reportItems[0].F7.ToString()))
        //                                  + (string.IsNullOrEmpty(reportItems[0].G7.ToString()) ? 0 : float.Parse(reportItems[0].G7.ToString()))
        //                                  + (string.IsNullOrEmpty(reportItems[0].H7.ToString()) ? 0 : float.Parse(reportItems[0].H7.ToString()))
        //                                  + (string.IsNullOrEmpty(reportItems[0].I7.ToString()) ? 0 : float.Parse(reportItems[0].I7.ToString()))
        //                                  + (string.IsNullOrEmpty(reportItems[0].J7.ToString()) ? 0 : float.Parse(reportItems[0].J7.ToString()));

        //                reportItems[0].O3 = (string.IsNullOrEmpty(reportItems[0].O1.ToString()) ? 0 : float.Parse(reportItems[0].O1.ToString())) + (string.IsNullOrEmpty(reportItems[0].O2.ToString()) ? 0 : float.Parse(reportItems[0].O2.ToString()));

        //            }
        //            //else if (x == 10)
        //            //{
        //            //    reportItems[0].K1 = allItems[x].RN;
        //            //    reportItems[0].K2 = allItems[x].ITM_NM;
        //            //    reportItems[0].K3 = allItems[x].ITM_SZ_NM;
        //            //    reportItems[0].K4 = allItems[x].SL_ITM_QTY;
        //            //    reportItems[0].K5 = allItems[x].SL_ITM_PRC_DC_VAL;
        //            //    reportItems[0].K6 = allItems[x].SL_ITM_AMT;
        //            //    reportItems[0].K7 = allItems[x].SL_ITM_TAX_AMT;
        //            //    reportItems[0].K8 = allItems[x].MD_QTY;
        //            //    ////
        //            //    reportItems[0].O1 = (string.IsNullOrEmpty(reportItems[0].A6.ToString()) ? 0 : float.Parse(reportItems[0].A6.ToString()))
        //            //                  + (string.IsNullOrEmpty(reportItems[0].B6.ToString()) ? 0 : float.Parse(reportItems[0].B6.ToString()))
        //            //                  + (string.IsNullOrEmpty(reportItems[0].C6.ToString()) ? 0 : float.Parse(reportItems[0].C6.ToString()))
        //            //                  + (string.IsNullOrEmpty(reportItems[0].D6.ToString()) ? 0 : float.Parse(reportItems[0].D6.ToString()))
        //            //                  + (string.IsNullOrEmpty(reportItems[0].E6.ToString()) ? 0 : float.Parse(reportItems[0].E6.ToString()))
        //            //                  + (string.IsNullOrEmpty(reportItems[0].F6.ToString()) ? 0 : float.Parse(reportItems[0].F6.ToString()))
        //            //                  + (string.IsNullOrEmpty(reportItems[0].G6.ToString()) ? 0 : float.Parse(reportItems[0].G6.ToString()))
        //            //                  + (string.IsNullOrEmpty(reportItems[0].H6.ToString()) ? 0 : float.Parse(reportItems[0].H6.ToString()))
        //            //                  + (string.IsNullOrEmpty(reportItems[0].I6.ToString()) ? 0 : float.Parse(reportItems[0].I6.ToString()))
        //            //                  + (string.IsNullOrEmpty(reportItems[0].J6.ToString()) ? 0 : float.Parse(reportItems[0].J6.ToString()))
        //            //                  + (string.IsNullOrEmpty(reportItems[0].K6.ToString()) ? 0 : float.Parse(reportItems[0].K6.ToString()));


        //            //    reportItems[0].O2 = (string.IsNullOrEmpty(reportItems[0].A7.ToString()) ? 0 : float.Parse(reportItems[0].A7.ToString()))
        //            //                      + (string.IsNullOrEmpty(reportItems[0].B7.ToString()) ? 0 : float.Parse(reportItems[0].B7.ToString()))
        //            //                      + (string.IsNullOrEmpty(reportItems[0].C7.ToString()) ? 0 : float.Parse(reportItems[0].C7.ToString()))
        //            //                      + (string.IsNullOrEmpty(reportItems[0].D7.ToString()) ? 0 : float.Parse(reportItems[0].D7.ToString()))
        //            //                      + (string.IsNullOrEmpty(reportItems[0].E7.ToString()) ? 0 : float.Parse(reportItems[0].E7.ToString()))
        //            //                      + (string.IsNullOrEmpty(reportItems[0].F7.ToString()) ? 0 : float.Parse(reportItems[0].F7.ToString()))
        //            //                      + (string.IsNullOrEmpty(reportItems[0].G7.ToString()) ? 0 : float.Parse(reportItems[0].G7.ToString()))
        //            //                      + (string.IsNullOrEmpty(reportItems[0].H7.ToString()) ? 0 : float.Parse(reportItems[0].H7.ToString()))
        //            //                      + (string.IsNullOrEmpty(reportItems[0].I7.ToString()) ? 0 : float.Parse(reportItems[0].I7.ToString()))
        //            //                      + (string.IsNullOrEmpty(reportItems[0].J7.ToString()) ? 0 : float.Parse(reportItems[0].J7.ToString()))
        //            //                      + (string.IsNullOrEmpty(reportItems[0].K7.ToString()) ? 0 : float.Parse(reportItems[0].K7.ToString()));

        //            //    reportItems[0].O3 = (string.IsNullOrEmpty(reportItems[0].O1.ToString()) ? 0 : float.Parse(reportItems[0].O1.ToString())) + (string.IsNullOrEmpty(reportItems[0].O2.ToString()) ? 0 : float.Parse(reportItems[0].O2.ToString()));

        //            //}
        //            //else if (x == 11)
        //            //{
        //            //    reportItems[0].L1 = allItems[x].RN;
        //            //    reportItems[0].L2 = allItems[x].ITM_NM;
        //            //    reportItems[0].L3 = allItems[x].ITM_SZ_NM;
        //            //    reportItems[0].L4 = allItems[x].SL_ITM_QTY;
        //            //    reportItems[0].L5 = allItems[x].SL_ITM_PRC_DC_VAL;
        //            //    reportItems[0].L6 = allItems[x].SL_ITM_AMT;
        //            //    reportItems[0].L7 = allItems[x].SL_ITM_TAX_AMT;
        //            //    reportItems[0].L8 = allItems[x].MD_QTY;
        //            //    ////
        //            //    reportItems[0].O1 = (string.IsNullOrEmpty(reportItems[0].A6.ToString()) ? 0 : float.Parse(reportItems[0].A6.ToString()))
        //            //                  + (string.IsNullOrEmpty(reportItems[0].B6.ToString()) ? 0 : float.Parse(reportItems[0].B6.ToString()))
        //            //                  + (string.IsNullOrEmpty(reportItems[0].C6.ToString()) ? 0 : float.Parse(reportItems[0].C6.ToString()))
        //            //                  + (string.IsNullOrEmpty(reportItems[0].D6.ToString()) ? 0 : float.Parse(reportItems[0].D6.ToString()))
        //            //                  + (string.IsNullOrEmpty(reportItems[0].E6.ToString()) ? 0 : float.Parse(reportItems[0].E6.ToString()))
        //            //                  + (string.IsNullOrEmpty(reportItems[0].F6.ToString()) ? 0 : float.Parse(reportItems[0].F6.ToString()))
        //            //                  + (string.IsNullOrEmpty(reportItems[0].G6.ToString()) ? 0 : float.Parse(reportItems[0].G6.ToString()))
        //            //                  + (string.IsNullOrEmpty(reportItems[0].H6.ToString()) ? 0 : float.Parse(reportItems[0].H6.ToString()))
        //            //                  + (string.IsNullOrEmpty(reportItems[0].I6.ToString()) ? 0 : float.Parse(reportItems[0].I6.ToString()))
        //            //                  + (string.IsNullOrEmpty(reportItems[0].J6.ToString()) ? 0 : float.Parse(reportItems[0].J6.ToString()))
        //            //                  + (string.IsNullOrEmpty(reportItems[0].K6.ToString()) ? 0 : float.Parse(reportItems[0].K6.ToString()))
        //            //                  + (string.IsNullOrEmpty(reportItems[0].L6.ToString()) ? 0 : float.Parse(reportItems[0].L6.ToString()));


        //            //    reportItems[0].O2 = (string.IsNullOrEmpty(reportItems[0].A7.ToString()) ? 0 : float.Parse(reportItems[0].A7.ToString()))
        //            //                      + (string.IsNullOrEmpty(reportItems[0].B7.ToString()) ? 0 : float.Parse(reportItems[0].B7.ToString()))
        //            //                      + (string.IsNullOrEmpty(reportItems[0].C7.ToString()) ? 0 : float.Parse(reportItems[0].C7.ToString()))
        //            //                      + (string.IsNullOrEmpty(reportItems[0].D7.ToString()) ? 0 : float.Parse(reportItems[0].D7.ToString()))
        //            //                      + (string.IsNullOrEmpty(reportItems[0].E7.ToString()) ? 0 : float.Parse(reportItems[0].E7.ToString()))
        //            //                      + (string.IsNullOrEmpty(reportItems[0].F7.ToString()) ? 0 : float.Parse(reportItems[0].F7.ToString()))
        //            //                      + (string.IsNullOrEmpty(reportItems[0].G7.ToString()) ? 0 : float.Parse(reportItems[0].G7.ToString()))
        //            //                      + (string.IsNullOrEmpty(reportItems[0].H7.ToString()) ? 0 : float.Parse(reportItems[0].H7.ToString()))
        //            //                      + (string.IsNullOrEmpty(reportItems[0].I7.ToString()) ? 0 : float.Parse(reportItems[0].I7.ToString()))
        //            //                      + (string.IsNullOrEmpty(reportItems[0].J7.ToString()) ? 0 : float.Parse(reportItems[0].J7.ToString()))
        //            //                      + (string.IsNullOrEmpty(reportItems[0].K7.ToString()) ? 0 : float.Parse(reportItems[0].K7.ToString()))
        //            //                      + (string.IsNullOrEmpty(reportItems[0].L7.ToString()) ? 0 : float.Parse(reportItems[0].L7.ToString()));

        //            //    reportItems[0].O3 = (string.IsNullOrEmpty(reportItems[0].O1.ToString()) ? 0 : float.Parse(reportItems[0].O1.ToString())) + (string.IsNullOrEmpty(reportItems[0].O2.ToString()) ? 0 : float.Parse(reportItems[0].O2.ToString()));

        //            //}
        //            //
        //            //reportItems[0].O1 = (string.IsNullOrEmpty(reportItems[0].A6.ToString()) ? 0 : float.Parse(reportItems[0].A6.ToString()))
        //            //                  + (string.IsNullOrEmpty(reportItems[0].B6.ToString()) ? 0 : float.Parse(reportItems[0].B6.ToString()))
        //            //                  + (string.IsNullOrEmpty(reportItems[0].C6.ToString()) ? 0 : float.Parse(reportItems[0].C6.ToString()))
        //            //                  + (string.IsNullOrEmpty(reportItems[0].D6.ToString()) ? 0 : float.Parse(reportItems[0].D6.ToString()))
        //            //                  + (string.IsNullOrEmpty(reportItems[0].E6.ToString()) ? 0 : float.Parse(reportItems[0].E6.ToString()))
        //            //                  + (string.IsNullOrEmpty(reportItems[0].F6.ToString()) ? 0 : float.Parse(reportItems[0].F6.ToString()))
        //            //                  + (string.IsNullOrEmpty(reportItems[0].G6.ToString()) ? 0 : float.Parse(reportItems[0].G6.ToString()))
        //            //                  + (string.IsNullOrEmpty(reportItems[0].H6.ToString()) ? 0 : float.Parse(reportItems[0].H6.ToString()))
        //            //                  + (string.IsNullOrEmpty(reportItems[0].I6.ToString()) ? 0 : float.Parse(reportItems[0].I6.ToString()))
        //            //                  + (string.IsNullOrEmpty(reportItems[0].J6.ToString()) ? 0 : float.Parse(reportItems[0].J6.ToString()));


        //            //reportItems[0].O2 = (string.IsNullOrEmpty(reportItems[0].A7.ToString()) ? 0 : float.Parse(reportItems[0].A7.ToString()))
        //            //                  + (string.IsNullOrEmpty(reportItems[0].B7.ToString()) ? 0 : float.Parse(reportItems[0].B7.ToString()))
        //            //                  + (string.IsNullOrEmpty(reportItems[0].C7.ToString()) ? 0 : float.Parse(reportItems[0].C7.ToString()))
        //            //                  + (string.IsNullOrEmpty(reportItems[0].D7.ToString()) ? 0 : float.Parse(reportItems[0].D7.ToString()))
        //            //                  + (string.IsNullOrEmpty(reportItems[0].E7.ToString()) ? 0 : float.Parse(reportItems[0].E7.ToString()))
        //            //                  + (string.IsNullOrEmpty(reportItems[0].F7.ToString()) ? 0 : float.Parse(reportItems[0].F7.ToString()))
        //            //                  + (string.IsNullOrEmpty(reportItems[0].G7.ToString()) ? 0 : float.Parse(reportItems[0].G7.ToString()))
        //            //                  + (string.IsNullOrEmpty(reportItems[0].H7.ToString()) ? 0 : float.Parse(reportItems[0].H7.ToString()))
        //            //                  + (string.IsNullOrEmpty(reportItems[0].I7.ToString()) ? 0 : float.Parse(reportItems[0].I7.ToString()))
        //            //                  + (string.IsNullOrEmpty(reportItems[0].J7.ToString()) ? 0 : float.Parse(reportItems[0].J7.ToString()));

        //            //reportItems[0].O3 = (string.IsNullOrEmpty(reportItems[0].O1.ToString()) ? 0 : float.Parse(reportItems[0].O1.ToString())) + (string.IsNullOrEmpty(reportItems[0].O2.ToString()) ? 0 : float.Parse(reportItems[0].O2.ToString()));


        //            //else if (x == 12)
        //            //{
        //            //    reportItems[0].M1 = allItems[x].RN;
        //            //    reportItems[0].M2 = allItems[x].ITM_NM;
        //            //    reportItems[0].M3 = allItems[x].ITM_SZ_NM;
        //            //    reportItems[0].M4 = allItems[x].SL_ITM_QTY;
        //            //    reportItems[0].M5 = allItems[x].SL_ITM_PRC_DC_VAL;
        //            //    reportItems[0].M6 = allItems[x].SL_ITM_AMT;
        //            //    reportItems[0].M7 = allItems[x].SL_ITM_TAX_AMT;
        //            //    reportItems[0].M8 = allItems[x].MD_QTY;
        //            //}
        //            //else if (x == 13)
        //            //{
        //            //    reportItems[0].B1 = allItems[x].RN;
        //            //    reportItems[0].B2 = allItems[x].ITM_NM;
        //            //    reportItems[0].B3 = allItems[x].ITM_SZ_NM;
        //            //    reportItems[0].B4 = allItems[x].UOM_NM;
        //            //    reportItems[0].B5 = allItems[x].SL_ITM_QTY;
        //            //    reportItems[0].B6 = allItems[x].MD_QTY;
        //            //}
        //        }
        //        //allItems[0].PAGE_NUM = "1/1";
        //    }
        //    else
        //    {
        //        //Page 나누기
        //        int nTotal = allItems.Count;
        //        int nPage = nTotal / 10;
        //        int mod = ((int)nTotal % 10);


        //        int min = 0;
        //        int max = 0;

        //        for (int z = 0; z < nPage; z++)
        //        {
        //            min = z * 10;
        //            max = min + 10;

        //            reportItems.Insert(z, new SpecificationOnTransVo());
        //            for (int x = min; x < max; x++)
        //            {
        //                //allItems[z].PAGE_NUM =  (z + 1) + "/" + nPage;
        //                if ((x % 10) == 0)
        //                {
        //                    reportItems[z].A1 = allItems[x].RN;
        //                    reportItems[z].A2 = allItems[x].ITM_NM;
        //                    reportItems[z].A3 = allItems[x].ITM_SZ_NM;
        //                    reportItems[z].A4 = allItems[x].SL_ITM_QTY;
        //                    reportItems[z].A5 = allItems[x].SL_ITM_PRC_DC_VAL;
        //                    reportItems[z].A6 = allItems[x].SL_ITM_AMT;
        //                    reportItems[z].A7 = allItems[x].SL_ITM_TAX_AMT;
        //                    reportItems[z].A8 = allItems[x].MD_QTY;
        //                    //
        //                    reportItems[z].O1 = reportItems[z].A6;
        //                    reportItems[z].O2 = reportItems[z].A7;
        //                    reportItems[z].O3 = float.Parse(reportItems[z].O1.ToString()) + float.Parse(reportItems[z].O2.ToString());
        //                    //
        //                    reportItems[z].B_CO_RGST_NO = allItems[x].B_CO_RGST_NO;
        //                    reportItems[z].B_CO_NM = allItems[x].B_CO_NM;
        //                    reportItems[z].B_PRSD_NM = allItems[x].B_PRSD_NM;
        //                    reportItems[z].B_HDQTR_ADDR = allItems[x].B_HDQTR_ADDR;
        //                    reportItems[z].B_N1ST_BIZ_COND_NM = allItems[x].B_N1ST_BIZ_COND_NM;
        //                    reportItems[z].B_N1ST_BZTP_NM = allItems[x].B_N1ST_BZTP_NM;
        //                    reportItems[z].B_HDQTR_PHN_NO = allItems[x].B_HDQTR_PHN_NO;
        //                    reportItems[z].B_HDQTR_FAX_NO = allItems[x].B_HDQTR_FAX_NO;
        //                    //
        //                    reportItems[z].C_CO_RGST_NO = allItems[x].C_CO_RGST_NO;
        //                    reportItems[z].C_CO_NM = allItems[x].C_CO_NM;
        //                    reportItems[z].C_PRSD_NM = allItems[x].C_PRSD_NM;
        //                    reportItems[z].C_HDQTR_ADDR = allItems[x].C_HDQTR_ADDR;
        //                    reportItems[z].C_N1ST_BIZ_COND_NM = allItems[x].C_N1ST_BIZ_COND_NM;
        //                    reportItems[z].C_N1ST_BZTP_NM = allItems[x].C_N1ST_BZTP_NM;
        //                    reportItems[z].C_HDQTR_PHN_NO = allItems[x].C_HDQTR_PHN_NO;
        //                    reportItems[z].C_HDQTR_FAX_NO = allItems[x].C_HDQTR_FAX_NO;

        //                    reportItems[z].SL_BIL_RMK = "비 고 : " + allItems[x].SL_BIL_RMK;
        //                    reportItems[z].SL_BIL_NO = allItems[x].SL_BIL_NO;
        //                    reportItems[z].PRN_DT = "출력일시 : " + allItems[x].PRN_DT;
        //                    reportItems[z].CRE_USR_NM = allItems[x].CRE_USR_NM;

        //                    reportItems[z].SL_NXT_CLZ_FLG = allItems[x].SL_NXT_CLZ_FLG;
        //                    reportItems[z].NXT_MON_DT = "이월일자 : " + allItems[x].NXT_MON_DT;
        //                }
        //                else if ((x % 10) == 1)
        //                {
        //                    reportItems[z].B1 = allItems[x].RN;
        //                    reportItems[z].B2 = allItems[x].ITM_NM;
        //                    reportItems[z].B3 = allItems[x].ITM_SZ_NM;
        //                    reportItems[z].B4 = allItems[x].SL_ITM_QTY;
        //                    reportItems[z].B5 = allItems[x].SL_ITM_PRC_DC_VAL;
        //                    reportItems[z].B6 = allItems[x].SL_ITM_AMT;
        //                    reportItems[z].B7 = allItems[x].SL_ITM_TAX_AMT;
        //                    reportItems[z].B8 = allItems[x].MD_QTY;
        //                    //
        //                    reportItems[z].O1 = float.Parse(reportItems[z].O1.ToString()) + float.Parse(reportItems[z].B6.ToString());
        //                    reportItems[z].O2 = float.Parse(reportItems[z].O2.ToString()) + float.Parse(reportItems[z].B7.ToString());
        //                    reportItems[z].O3 = float.Parse(reportItems[z].O1.ToString()) + float.Parse(reportItems[z].O2.ToString());
        //                }
        //                else if ((x % 10) == 2)
        //                {
        //                    reportItems[z].C1 = allItems[x].RN;
        //                    reportItems[z].C2 = allItems[x].ITM_NM;
        //                    reportItems[z].C3 = allItems[x].ITM_SZ_NM;
        //                    reportItems[z].C4 = allItems[x].SL_ITM_QTY;
        //                    reportItems[z].C5 = allItems[x].SL_ITM_PRC_DC_VAL;
        //                    reportItems[z].C6 = allItems[x].SL_ITM_AMT;
        //                    reportItems[z].C7 = allItems[x].SL_ITM_TAX_AMT;
        //                    reportItems[z].C8 = allItems[x].MD_QTY;
        //                    //
        //                    reportItems[z].O1 = float.Parse(reportItems[z].O1.ToString()) + float.Parse(reportItems[z].C6.ToString());
        //                    reportItems[z].O2 = float.Parse(reportItems[z].O2.ToString()) + float.Parse(reportItems[z].C7.ToString());
        //                    reportItems[z].O3 = float.Parse(reportItems[z].O1.ToString()) + float.Parse(reportItems[z].O2.ToString());
        //                }
        //                else if ((x % 10) == 3)
        //                {
        //                    reportItems[z].D1 = allItems[x].RN;
        //                    reportItems[z].D2 = allItems[x].ITM_NM;
        //                    reportItems[z].D3 = allItems[x].ITM_SZ_NM;
        //                    reportItems[z].D4 = allItems[x].SL_ITM_QTY;
        //                    reportItems[z].D5 = allItems[x].SL_ITM_PRC_DC_VAL;
        //                    reportItems[z].D6 = allItems[x].SL_ITM_AMT;
        //                    reportItems[z].D7 = allItems[x].SL_ITM_TAX_AMT;
        //                    reportItems[z].D8 = allItems[x].MD_QTY;
        //                    //
        //                    reportItems[z].O1 = float.Parse(reportItems[z].O1.ToString()) + float.Parse(reportItems[z].D6.ToString());
        //                    reportItems[z].O2 = float.Parse(reportItems[z].O2.ToString()) + float.Parse(reportItems[z].D7.ToString());
        //                    reportItems[z].O3 = float.Parse(reportItems[z].O1.ToString()) + float.Parse(reportItems[z].O2.ToString());
        //                }
        //                else if ((x % 10) == 4)
        //                {
        //                    reportItems[z].E1 = allItems[x].RN;
        //                    reportItems[z].E2 = allItems[x].ITM_NM;
        //                    reportItems[z].E3 = allItems[x].ITM_SZ_NM;
        //                    reportItems[z].E4 = allItems[x].SL_ITM_QTY;
        //                    reportItems[z].E5 = allItems[x].SL_ITM_PRC_DC_VAL;
        //                    reportItems[z].E6 = allItems[x].SL_ITM_AMT;
        //                    reportItems[z].E7 = allItems[x].SL_ITM_TAX_AMT;
        //                    reportItems[z].E8 = allItems[x].MD_QTY;
        //                    //
        //                    reportItems[z].O1 = float.Parse(reportItems[z].O1.ToString()) + float.Parse(reportItems[z].E6.ToString());
        //                    reportItems[z].O2 = float.Parse(reportItems[z].O2.ToString()) + float.Parse(reportItems[z].E7.ToString());
        //                    reportItems[z].O3 = float.Parse(reportItems[z].O1.ToString()) + float.Parse(reportItems[z].O2.ToString());
        //                }
        //                else if ((x % 10) == 5)
        //                {
        //                    reportItems[z].F1 = allItems[x].RN;
        //                    reportItems[z].F2 = allItems[x].ITM_NM;
        //                    reportItems[z].F3 = allItems[x].ITM_SZ_NM;
        //                    reportItems[z].F4 = allItems[x].SL_ITM_QTY;
        //                    reportItems[z].F5 = allItems[x].SL_ITM_PRC_DC_VAL;
        //                    reportItems[z].F6 = allItems[x].SL_ITM_AMT;
        //                    reportItems[z].F7 = allItems[x].SL_ITM_TAX_AMT;
        //                    reportItems[z].F8 = allItems[x].MD_QTY;
        //                    //
        //                    reportItems[z].O1 = float.Parse(reportItems[z].O1.ToString()) + float.Parse(reportItems[z].F6.ToString());
        //                    reportItems[z].O2 = float.Parse(reportItems[z].O2.ToString()) + float.Parse(reportItems[z].F7.ToString());
        //                    reportItems[z].O3 = float.Parse(reportItems[z].O1.ToString()) + float.Parse(reportItems[z].O2.ToString());
        //                }
        //                else if ((x % 10) == 6)
        //                {
        //                    reportItems[z].G1 = allItems[x].RN;
        //                    reportItems[z].G2 = allItems[x].ITM_NM;
        //                    reportItems[z].G3 = allItems[x].ITM_SZ_NM;
        //                    reportItems[z].G4 = allItems[x].SL_ITM_QTY;
        //                    reportItems[z].G5 = allItems[x].SL_ITM_PRC_DC_VAL;
        //                    reportItems[z].G6 = allItems[x].SL_ITM_AMT;
        //                    reportItems[z].G7 = allItems[x].SL_ITM_TAX_AMT;
        //                    reportItems[z].G8 = allItems[x].MD_QTY;
        //                    //
        //                    reportItems[z].O1 = float.Parse(reportItems[z].O1.ToString()) + float.Parse(reportItems[z].G6.ToString());
        //                    reportItems[z].O2 = float.Parse(reportItems[z].O2.ToString()) + float.Parse(reportItems[z].G7.ToString());
        //                    reportItems[z].O3 = float.Parse(reportItems[z].O1.ToString()) + float.Parse(reportItems[z].O2.ToString());
        //                }
        //                else if ((x % 10) == 7)
        //                {
        //                    reportItems[z].H1 = allItems[x].RN;
        //                    reportItems[z].H2 = allItems[x].ITM_NM;
        //                    reportItems[z].H3 = allItems[x].ITM_SZ_NM;
        //                    reportItems[z].H4 = allItems[x].SL_ITM_QTY;
        //                    reportItems[z].H5 = allItems[x].SL_ITM_PRC_DC_VAL;
        //                    reportItems[z].H6 = allItems[x].SL_ITM_AMT;
        //                    reportItems[z].H7 = allItems[x].SL_ITM_TAX_AMT;
        //                    reportItems[z].H8 = allItems[x].MD_QTY;
        //                    //
        //                    reportItems[z].O1 = float.Parse(reportItems[z].O1.ToString()) + float.Parse(reportItems[z].H6.ToString());
        //                    reportItems[z].O2 = float.Parse(reportItems[z].O2.ToString()) + float.Parse(reportItems[z].H7.ToString());
        //                    reportItems[z].O3 = float.Parse(reportItems[z].O1.ToString()) + float.Parse(reportItems[z].O2.ToString());
        //                }
        //                else if ((x % 10) == 8)
        //                {
        //                    reportItems[z].I1 = allItems[x].RN;
        //                    reportItems[z].I2 = allItems[x].ITM_NM;
        //                    reportItems[z].I3 = allItems[x].ITM_SZ_NM;
        //                    reportItems[z].I4 = allItems[x].SL_ITM_QTY;
        //                    reportItems[z].I5 = allItems[x].SL_ITM_PRC_DC_VAL;
        //                    reportItems[z].I6 = allItems[x].SL_ITM_AMT;
        //                    reportItems[z].I7 = allItems[x].SL_ITM_TAX_AMT;
        //                    reportItems[z].I8 = allItems[x].MD_QTY;
        //                    //
        //                    reportItems[z].O1 = float.Parse(reportItems[z].O1.ToString()) + float.Parse(reportItems[z].I6.ToString());
        //                    reportItems[z].O2 = float.Parse(reportItems[z].O2.ToString()) + float.Parse(reportItems[z].I7.ToString());
        //                    reportItems[z].O3 = float.Parse(reportItems[z].O1.ToString()) + float.Parse(reportItems[z].O2.ToString());
        //                }
        //                else if ((x % 10) == 9)
        //                {
        //                    reportItems[z].J1 = allItems[x].RN;
        //                    reportItems[z].J2 = allItems[x].ITM_NM;
        //                    reportItems[z].J3 = allItems[x].ITM_SZ_NM;
        //                    reportItems[z].J4 = allItems[x].SL_ITM_QTY;
        //                    reportItems[z].J5 = allItems[x].SL_ITM_PRC_DC_VAL;
        //                    reportItems[z].J6 = allItems[x].SL_ITM_AMT;
        //                    reportItems[z].J7 = allItems[x].SL_ITM_TAX_AMT;
        //                    reportItems[z].J8 = allItems[x].MD_QTY;
        //                    //
        //                    reportItems[z].O1 = float.Parse(reportItems[z].O1.ToString()) + float.Parse(reportItems[z].J6.ToString());
        //                    reportItems[z].O2 = float.Parse(reportItems[z].O2.ToString()) + float.Parse(reportItems[z].J7.ToString());
        //                    reportItems[z].O3 = float.Parse(reportItems[z].O1.ToString()) + float.Parse(reportItems[z].O2.ToString());
        //                }
        //                //else if ((x % 10) == 10)
        //                //{
        //                //    reportItems[z].K1 = allItems[x].RN;
        //                //    reportItems[z].K2 = allItems[x].ITM_NM;
        //                //    reportItems[z].K3 = allItems[x].ITM_SZ_NM;
        //                //    reportItems[z].K4 = allItems[x].SL_ITM_QTY;
        //                //    reportItems[z].K5 = allItems[x].SL_ITM_PRC_DC_VAL;
        //                //    reportItems[z].K6 = allItems[x].SL_ITM_AMT;
        //                //    reportItems[z].K7 = allItems[x].SL_ITM_TAX_AMT;
        //                //    reportItems[z].K8 = allItems[x].MD_QTY;
        //                //}
        //                //else if ((x % 10) == 11)
        //                //{
        //                //    reportItems[z].L1 = allItems[x].RN;
        //                //    reportItems[z].L2 = allItems[x].ITM_NM;
        //                //    reportItems[z].L3 = allItems[x].ITM_SZ_NM;
        //                //    reportItems[z].L4 = allItems[x].SL_ITM_QTY;
        //                //    reportItems[z].L5 = allItems[x].SL_ITM_PRC_DC_VAL;
        //                //    reportItems[z].L6 = allItems[x].SL_ITM_AMT;
        //                //    reportItems[z].L7 = allItems[x].SL_ITM_TAX_AMT;
        //                //    reportItems[z].L8 = allItems[x].MD_QTY;
        //                //}
        //                //else if ((x % 10) == 12)
        //                //{
        //                //    reportItems[z].M1 = allItems[x].RN;
        //                //    reportItems[z].M2 = allItems[x].ITM_NM;
        //                //    reportItems[z].M3 = allItems[x].ITM_SZ_NM;
        //                //    reportItems[z].M4 = allItems[x].SL_ITM_QTY;
        //                //    reportItems[z].M5 = allItems[x].SL_ITM_PRC_DC_VAL;
        //                //    reportItems[z].M6 = allItems[x].SL_ITM_AMT;
        //                //    reportItems[z].M7 = allItems[x].SL_ITM_TAX_AMT;
        //                //    reportItems[z].M8 = allItems[x].MD_QTY;
        //                //}
        //                //else if ((x % 10) == 13)
        //                //{
        //                //    allItems[0].B1 = allItems[x].RN;
        //                //    allItems[0].B2 = allItems[x].ITM_NM;
        //                //    allItems[0].B3 = allItems[x].ITM_SZ_NM;
        //                //    allItems[0].B4 = allItems[x].UOM_NM;
        //                //    allItems[0].B5 = allItems[x].SL_ITM_QTY;
        //                //    allItems[0].B6 = allItems[x].MD_QTY;
        //                //}
        //            }
        //        }

        //        //나머지 계산
        //        if (mod != 0)
        //        {
        //            min = nPage * 10;
        //            reportItems.Insert(nPage, new SpecificationOnTransVo());
        //            for (int x = min; x < allItems.Count; x++)
        //            {
        //                if ((x % 10) == 0)
        //                {
        //                    reportItems[nPage].A1 = allItems[x].RN;
        //                    reportItems[nPage].A2 = allItems[x].ITM_NM;
        //                    reportItems[nPage].A3 = allItems[x].ITM_SZ_NM;
        //                    reportItems[nPage].A4 = allItems[x].SL_ITM_QTY;
        //                    reportItems[nPage].A5 = allItems[x].SL_ITM_PRC_DC_VAL;
        //                    reportItems[nPage].A6 = allItems[x].SL_ITM_AMT;
        //                    reportItems[nPage].A7 = allItems[x].SL_ITM_TAX_AMT;
        //                    reportItems[nPage].A8 = allItems[x].MD_QTY;
        //                    //
        //                    //
        //                    reportItems[nPage].O1 = reportItems[nPage].A6;
        //                    reportItems[nPage].O2 = reportItems[nPage].A7;
        //                    reportItems[nPage].O3 = float.Parse(reportItems[nPage].O1.ToString()) + float.Parse(reportItems[nPage].O2.ToString());
        //                    //
        //                    reportItems[nPage].B_CO_RGST_NO = allItems[x].B_CO_RGST_NO;
        //                    reportItems[nPage].B_CO_NM = allItems[x].B_CO_NM;
        //                    reportItems[nPage].B_PRSD_NM = allItems[x].B_PRSD_NM;
        //                    reportItems[nPage].B_HDQTR_ADDR = allItems[x].B_HDQTR_ADDR;
        //                    reportItems[nPage].B_N1ST_BIZ_COND_NM = allItems[x].B_N1ST_BIZ_COND_NM;
        //                    reportItems[nPage].B_N1ST_BZTP_NM = allItems[x].B_N1ST_BZTP_NM;
        //                    reportItems[nPage].B_HDQTR_PHN_NO = allItems[x].B_HDQTR_PHN_NO;
        //                    reportItems[nPage].B_HDQTR_FAX_NO = allItems[x].B_HDQTR_FAX_NO;
        //                    //
        //                    reportItems[nPage].C_CO_RGST_NO = allItems[x].C_CO_RGST_NO;
        //                    reportItems[nPage].C_CO_NM = allItems[x].C_CO_NM;
        //                    reportItems[nPage].C_PRSD_NM = allItems[x].C_PRSD_NM;
        //                    reportItems[nPage].C_HDQTR_ADDR = allItems[x].C_HDQTR_ADDR;
        //                    reportItems[nPage].C_N1ST_BIZ_COND_NM = allItems[x].C_N1ST_BIZ_COND_NM;
        //                    reportItems[nPage].C_N1ST_BZTP_NM = allItems[x].C_N1ST_BZTP_NM;
        //                    reportItems[nPage].C_HDQTR_PHN_NO = allItems[x].C_HDQTR_PHN_NO;
        //                    reportItems[nPage].C_HDQTR_FAX_NO = allItems[x].C_HDQTR_FAX_NO;

        //                    reportItems[nPage].SL_BIL_RMK = "비 고 : " + allItems[x].SL_BIL_RMK;
        //                    reportItems[nPage].SL_BIL_NO = allItems[x].SL_BIL_NO;
        //                    reportItems[nPage].PRN_DT = "출력일시 : " + allItems[x].PRN_DT;
        //                    reportItems[nPage].CRE_USR_NM = allItems[x].CRE_USR_NM;

        //                    reportItems[nPage].SL_NXT_CLZ_FLG = allItems[x].SL_NXT_CLZ_FLG;
        //                    reportItems[nPage].NXT_MON_DT = "이월일자 : " + allItems[x].NXT_MON_DT;
        //                }
        //                else if ((x % 10) == 1)
        //                {
        //                    reportItems[nPage].B1 = allItems[x].RN;
        //                    reportItems[nPage].B2 = allItems[x].ITM_NM;
        //                    reportItems[nPage].B3 = allItems[x].ITM_SZ_NM;
        //                    reportItems[nPage].B4 = allItems[x].SL_ITM_QTY;
        //                    reportItems[nPage].B5 = allItems[x].SL_ITM_PRC_DC_VAL;
        //                    reportItems[nPage].B6 = allItems[x].SL_ITM_AMT;
        //                    reportItems[nPage].B7 = allItems[x].SL_ITM_TAX_AMT;
        //                    reportItems[nPage].B8 = allItems[x].MD_QTY;
        //                    //
        //                    reportItems[nPage].O1 = float.Parse(reportItems[nPage].O1.ToString()) + float.Parse(reportItems[nPage].B6.ToString());
        //                    reportItems[nPage].O2 = float.Parse(reportItems[nPage].O2.ToString()) + float.Parse(reportItems[nPage].B7.ToString());
        //                    reportItems[nPage].O3 = float.Parse(reportItems[nPage].O1.ToString()) + float.Parse(reportItems[nPage].O2.ToString());
        //                }
        //                else if ((x % 10) == 2)
        //                {
        //                    reportItems[nPage].C1 = allItems[x].RN;
        //                    reportItems[nPage].C2 = allItems[x].ITM_NM;
        //                    reportItems[nPage].C3 = allItems[x].ITM_SZ_NM;
        //                    reportItems[nPage].C4 = allItems[x].SL_ITM_QTY;
        //                    reportItems[nPage].C5 = allItems[x].SL_ITM_PRC_DC_VAL;
        //                    reportItems[nPage].C6 = allItems[x].SL_ITM_AMT;
        //                    reportItems[nPage].C7 = allItems[x].SL_ITM_TAX_AMT;
        //                    reportItems[nPage].C8 = allItems[x].MD_QTY;
        //                    //
        //                    reportItems[nPage].O1 = float.Parse(reportItems[nPage].O1.ToString()) + float.Parse(reportItems[nPage].C6.ToString());
        //                    reportItems[nPage].O2 = float.Parse(reportItems[nPage].O2.ToString()) + float.Parse(reportItems[nPage].C7.ToString());
        //                    reportItems[nPage].O3 = float.Parse(reportItems[nPage].O1.ToString()) + float.Parse(reportItems[nPage].O2.ToString());
        //                }
        //                else if ((x % 10) == 3)
        //                {
        //                    reportItems[nPage].D1 = allItems[x].RN;
        //                    reportItems[nPage].D2 = allItems[x].ITM_NM;
        //                    reportItems[nPage].D3 = allItems[x].ITM_SZ_NM;
        //                    reportItems[nPage].D4 = allItems[x].SL_ITM_QTY;
        //                    reportItems[nPage].D5 = allItems[x].SL_ITM_PRC_DC_VAL;
        //                    reportItems[nPage].D6 = allItems[x].SL_ITM_AMT;
        //                    reportItems[nPage].D7 = allItems[x].SL_ITM_TAX_AMT;
        //                    reportItems[nPage].D8 = allItems[x].MD_QTY;
        //                    //
        //                    reportItems[nPage].O1 = float.Parse(reportItems[nPage].O1.ToString()) + float.Parse(reportItems[nPage].D6.ToString());
        //                    reportItems[nPage].O2 = float.Parse(reportItems[nPage].O2.ToString()) + float.Parse(reportItems[nPage].D7.ToString());
        //                    reportItems[nPage].O3 = float.Parse(reportItems[nPage].O1.ToString()) + float.Parse(reportItems[nPage].O2.ToString());
        //                }
        //                else if ((x % 10) == 4)
        //                {
        //                    reportItems[nPage].E1 = allItems[x].RN;
        //                    reportItems[nPage].E2 = allItems[x].ITM_NM;
        //                    reportItems[nPage].E3 = allItems[x].ITM_SZ_NM;
        //                    reportItems[nPage].E4 = allItems[x].SL_ITM_QTY;
        //                    reportItems[nPage].E5 = allItems[x].SL_ITM_PRC_DC_VAL;
        //                    reportItems[nPage].E6 = allItems[x].SL_ITM_AMT;
        //                    reportItems[nPage].E7 = allItems[x].SL_ITM_TAX_AMT;
        //                    reportItems[nPage].E8 = allItems[x].MD_QTY;
        //                    //
        //                    reportItems[nPage].O1 = float.Parse(reportItems[nPage].O1.ToString()) + float.Parse(reportItems[nPage].E6.ToString());
        //                    reportItems[nPage].O2 = float.Parse(reportItems[nPage].O2.ToString()) + float.Parse(reportItems[nPage].E7.ToString());
        //                    reportItems[nPage].O3 = float.Parse(reportItems[nPage].O1.ToString()) + float.Parse(reportItems[nPage].O2.ToString());
        //                }
        //                else if ((x % 10) == 5)
        //                {
        //                    reportItems[nPage].F1 = allItems[x].RN;
        //                    reportItems[nPage].F2 = allItems[x].ITM_NM;
        //                    reportItems[nPage].F3 = allItems[x].ITM_SZ_NM;
        //                    reportItems[nPage].F4 = allItems[x].SL_ITM_QTY;
        //                    reportItems[nPage].F5 = allItems[x].SL_ITM_PRC_DC_VAL;
        //                    reportItems[nPage].F6 = allItems[x].SL_ITM_AMT;
        //                    reportItems[nPage].F7 = allItems[x].SL_ITM_TAX_AMT;
        //                    reportItems[nPage].F8 = allItems[x].MD_QTY;
        //                    //
        //                    reportItems[nPage].O1 = float.Parse(reportItems[nPage].O1.ToString()) + float.Parse(reportItems[nPage].F6.ToString());
        //                    reportItems[nPage].O2 = float.Parse(reportItems[nPage].O2.ToString()) + float.Parse(reportItems[nPage].F7.ToString());
        //                    reportItems[nPage].O3 = float.Parse(reportItems[nPage].O1.ToString()) + float.Parse(reportItems[nPage].O2.ToString());
        //                }
        //                else if ((x % 10) == 6)
        //                {
        //                    reportItems[nPage].G1 = allItems[x].RN;
        //                    reportItems[nPage].G2 = allItems[x].ITM_NM;
        //                    reportItems[nPage].G3 = allItems[x].ITM_SZ_NM;
        //                    reportItems[nPage].G4 = allItems[x].SL_ITM_QTY;
        //                    reportItems[nPage].G5 = allItems[x].SL_ITM_PRC_DC_VAL;
        //                    reportItems[nPage].G6 = allItems[x].SL_ITM_AMT;
        //                    reportItems[nPage].G7 = allItems[x].SL_ITM_TAX_AMT;
        //                    reportItems[nPage].G8 = allItems[x].MD_QTY;
        //                    //
        //                    reportItems[nPage].O1 = float.Parse(reportItems[nPage].O1.ToString()) + float.Parse(reportItems[nPage].G6.ToString());
        //                    reportItems[nPage].O2 = float.Parse(reportItems[nPage].O2.ToString()) + float.Parse(reportItems[nPage].G7.ToString());
        //                    reportItems[nPage].O3 = float.Parse(reportItems[nPage].O1.ToString()) + float.Parse(reportItems[nPage].O2.ToString());
        //                }
        //                else if ((x % 10) == 7)
        //                {
        //                    reportItems[nPage].H1 = allItems[x].RN;
        //                    reportItems[nPage].H2 = allItems[x].ITM_NM;
        //                    reportItems[nPage].H3 = allItems[x].ITM_SZ_NM;
        //                    reportItems[nPage].H4 = allItems[x].SL_ITM_QTY;
        //                    reportItems[nPage].H5 = allItems[x].SL_ITM_PRC_DC_VAL;
        //                    reportItems[nPage].H6 = allItems[x].SL_ITM_AMT;
        //                    reportItems[nPage].H7 = allItems[x].SL_ITM_TAX_AMT;
        //                    reportItems[nPage].H8 = allItems[x].MD_QTY;
        //                    //
        //                    reportItems[nPage].O1 = float.Parse(reportItems[nPage].O1.ToString()) + float.Parse(reportItems[nPage].H6.ToString());
        //                    reportItems[nPage].O2 = float.Parse(reportItems[nPage].O2.ToString()) + float.Parse(reportItems[nPage].H7.ToString());
        //                    reportItems[nPage].O3 = float.Parse(reportItems[nPage].O1.ToString()) + float.Parse(reportItems[nPage].O2.ToString());
        //                }
        //                else if ((x % 10) == 8)
        //                {
        //                    reportItems[nPage].I1 = allItems[x].RN;
        //                    reportItems[nPage].I2 = allItems[x].ITM_NM;
        //                    reportItems[nPage].I3 = allItems[x].ITM_SZ_NM;
        //                    reportItems[nPage].I4 = allItems[x].SL_ITM_QTY;
        //                    reportItems[nPage].I5 = allItems[x].SL_ITM_PRC_DC_VAL;
        //                    reportItems[nPage].I6 = allItems[x].SL_ITM_AMT;
        //                    reportItems[nPage].I7 = allItems[x].SL_ITM_TAX_AMT;
        //                    reportItems[nPage].I8 = allItems[x].MD_QTY;
        //                    //
        //                    reportItems[nPage].O1 = float.Parse(reportItems[nPage].O1.ToString()) + float.Parse(reportItems[nPage].I6.ToString());
        //                    reportItems[nPage].O2 = float.Parse(reportItems[nPage].O2.ToString()) + float.Parse(reportItems[nPage].I7.ToString());
        //                    reportItems[nPage].O3 = float.Parse(reportItems[nPage].O1.ToString()) + float.Parse(reportItems[nPage].O2.ToString());
        //                }
        //                else if ((x % 10) == 9)
        //                {
        //                    reportItems[nPage].J1 = allItems[x].RN;
        //                    reportItems[nPage].J2 = allItems[x].ITM_NM;
        //                    reportItems[nPage].J3 = allItems[x].ITM_SZ_NM;
        //                    reportItems[nPage].J4 = allItems[x].SL_ITM_QTY;
        //                    reportItems[nPage].J5 = allItems[x].SL_ITM_PRC_DC_VAL;
        //                    reportItems[nPage].J6 = allItems[x].SL_ITM_AMT;
        //                    reportItems[nPage].J7 = allItems[x].SL_ITM_TAX_AMT;
        //                    reportItems[nPage].J8 = allItems[x].MD_QTY;
        //                    //
        //                    reportItems[nPage].O1 = float.Parse(reportItems[nPage].O1.ToString()) + float.Parse(reportItems[nPage].J6.ToString());
        //                    reportItems[nPage].O2 = float.Parse(reportItems[nPage].O2.ToString()) + float.Parse(reportItems[nPage].J7.ToString());
        //                    reportItems[nPage].O3 = float.Parse(reportItems[nPage].O1.ToString()) + float.Parse(reportItems[nPage].O2.ToString());
        //                }
        //                //else if ((x % 10) == 10)
        //                //{
        //                //    reportItems[nPage].K1 = allItems[x].RN;
        //                //    reportItems[nPage].K2 = allItems[x].ITM_NM;
        //                //    reportItems[nPage].K3 = allItems[x].ITM_SZ_NM;
        //                //    reportItems[nPage].K4 = allItems[x].SL_ITM_QTY;
        //                //    reportItems[nPage].K5 = allItems[x].SL_ITM_PRC_DC_VAL;
        //                //    reportItems[nPage].K6 = allItems[x].SL_ITM_AMT;
        //                //    reportItems[nPage].K7 = allItems[x].SL_ITM_TAX_AMT;
        //                //    reportItems[nPage].K8 = allItems[x].MD_QTY;
        //                //}
        //                //else if ((x % 10) == 11)
        //                //{
        //                //    reportItems[nPage].L1 = allItems[x].RN;
        //                //    reportItems[nPage].L2 = allItems[x].ITM_NM;
        //                //    reportItems[nPage].L3 = allItems[x].ITM_SZ_NM;
        //                //    reportItems[nPage].L4 = allItems[x].SL_ITM_QTY;
        //                //    reportItems[nPage].L5 = allItems[x].SL_ITM_PRC_DC_VAL;
        //                //    reportItems[nPage].L6 = allItems[x].SL_ITM_AMT;
        //                //    reportItems[nPage].L7 = allItems[x].SL_ITM_TAX_AMT;
        //                //    reportItems[nPage].L8 = allItems[x].MD_QTY;
        //                //}
        //                //else if ((x % 10) == 12)
        //                //{
        //                //    reportItems[nPage].M1 = allItems[x].RN;
        //                //    reportItems[nPage].M2 = allItems[x].ITM_NM;
        //                //    reportItems[nPage].M3 = allItems[x].ITM_SZ_NM;
        //                //    reportItems[nPage].M4 = allItems[x].SL_ITM_QTY;
        //                //    reportItems[nPage].M5 = allItems[x].SL_ITM_PRC_DC_VAL;
        //                //    reportItems[nPage].M6 = allItems[x].SL_ITM_AMT;
        //                //    reportItems[nPage].M7 = allItems[x].SL_ITM_TAX_AMT;
        //                //    reportItems[nPage].M8 = allItems[x].MD_QTY;
        //                //}
        //                //else if ((x % 10) == 13)
        //                //{
        //                //    reportItems[nPage].B1 = allItems[x].RN;
        //                //    reportItems[nPage].B2 = allItems[x].ITM_NM;
        //                //    reportItems[nPage].B3 = allItems[x].ITM_SZ_NM;
        //                //    reportItems[nPage].B4 = allItems[x].UOM_NM;
        //                //    reportItems[nPage].B5 = allItems[x].SL_ITM_QTY;
        //                //    reportItems[nPage].B6 = allItems[x].MD_QTY;
        //                //}
        //            }
        //        }
        //    }


        //    //공급가액
        //    long _O1 = 0;
        //    //세액
        //    long _O2 = 0;
        //    //합계
        //    long _O3 = 0;
        //    //현잔금
        //    long _O4 = 0;
        //    //총계
        //    long _O5 = 0;
        //    //합계 마지막 폐이지 이동
        //    for (int x = 0; x < reportItems.Count; x++)
        //    {
        //        _O1 += Convert.ToInt32(reportItems[x].O1);
        //        _O2 += Convert.ToInt32(reportItems[x].O2);
        //        _O3 += Convert.ToInt32(reportItems[x].O3);

        //        reportItems[x].O1 = "---";
        //        reportItems[x].O2 = "---";
        //        reportItems[x].O3 = "---";
        //    }
        //    reportItems[reportItems.Count - 1].O1 = _O1;
        //    reportItems[reportItems.Count - 1].O2 = _O2;
        //    reportItems[reportItems.Count - 1].O3 = _O3;


        //    S2213SpecificationOnTransReport report = new S2213SpecificationOnTransReport(reportItems);
        //    report.Margins.Top = 0;
        //    report.Margins.Bottom = 0;
        //    report.Margins.Left = 80;
        //    report.Margins.Right = 0;
        //    report.Landscape = false;
        //    report.PrintingSystem.ShowPrintStatusDialog = true;
        //    report.PaperKind = System.Drawing.Printing.PaperKind.A4;

        //    if (PRN_FLG.Equals("Y"))
        //    {
        //        //데모 시연 문서 표시 가능
        //        report.Watermark.Text = "재발행";
        //        report.Watermark.TextDirection = DirectionMode.ForwardDiagonal;
        //        report.Watermark.Font = new Font(report.Watermark.Font.FontFamily, 150);
        //        report.Watermark.ForeColor = System.Drawing.Color.DodgerBlue;
        //        ////report.Watermark.ForeColor = Color.PaleTurquoise;
        //        report.Watermark.TextTransparency = 190;
        //        ////report.Watermark.ShowBehind = false;
        //        ////report.Watermark.PageRange = "1,3-5";
        //    }

        //    XtraReportPreviewModel model = new XtraReportPreviewModel(report);
        //    DocumentPreviewWindow window = new DocumentPreviewWindow() { Model = model };
        //    report.CreateDocument(true);
        //    window.Title = "거래명세서 [" + SelectedMstItem.SL_BIL_NO + "/" + SelectedMstItem.SL_CO_NM + "]";
        //    window.Owner = Application.Current.MainWindow;
        //    window.ShowDialog();

        //    SelectedMstItem.PRN_FLG = "Y";
        //    JobVo resultVo = saleOrderClient.S2213UpdateDtl(new JobVo() { SL_BIL_NO = SelectedMstItem.SL_BIL_NO, PRN_FLG = "Y", UPD_USR_ID = SystemProperties.USER });
        //    if (!resultVo.isSuccess)
        //    {
        //        //실패
        //        WinUIMessageBox.Show(resultVo.Message, "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
        //        return;
        //    }
        //} 
        #endregion

    }
    }
