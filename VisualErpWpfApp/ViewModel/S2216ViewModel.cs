using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.Core;
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
using System.Windows.Media;
using AquilaErpWpfApp3.Util;
using AquilaErpWpfApp3.View.SAL.Dialog;

namespace AquilaErpWpfApp3.ViewModel
{
    public sealed class S2216ViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private string title = "반품등록관리";
        //private IList<CodeDao> UserList;

        //private static SaleOrderServiceClient saleOrderClient = SystemProperties.SaleOrderClient;
        private IList<SaleVo> selectedMstList = new List<SaleVo>();

        //private ObservableCollection<PurVo> selectedDtlList = new ObservableCollection<PurVo>();
        private IList<SaleVo> selectedDtlList = new List<SaleVo>();

        //private S2217MasterDialog masterDialog;
        //private S2217DetailDialog detailDialog;

        //private S2217DetailEditDialog detailEditDialog;

        private S2216MasterDialog masterUsrDialog;

        //private string[,] _loadSave;

        ////Menu Dialog
        //private ICommand _searchDialogCommand;
        //private ICommand _newDialogCommand;
        //private ICommand _editDialogCommand;
        //private ICommand _delDialogCommand;
        ////private ICommand _excelDialogCommand;

        ////private ICommand _copyDialogCommand;

        //private ICommand _editCommand;

        //private ICommand _searchDetailDialogCommand;
        //private ICommand _newDetailDialogCommand;
        //private ICommand _editDetailDialogCommand;
        //private ICommand _delDetailDialogCommand;
        //private ICommand _CancelDialogCommand;

        //private ICommand reportDialogCommand;
        //private ICommand report_1DialogCommand;


        ////private ICommand _revListDetailDialogCommand;
        ////private ICommand _revNewDetailDialogCommand;

        //private P41MasterDialog masterDialog;
        //private P41DetailDialog detailDialog;
        ////private A21JobItemRevDialog jobItemRevDialog;

        //private P41ReportDialog reportDialog;

        public S2216ViewModel()
        {
            StartDt = System.DateTime.Now;
            EndDt = System.DateTime.Now;

            //반품승인관리해제
            //UserList = SystemProperties.SYSTEM_CODE_VO("S-046");

            SYSTEM_CODE_VO();
            // - Refresh

            //M_SEARCH_TEXT = "";
            //DeptList = SystemProperties.SYSTEM_DEPT_VO();
            //DeptList.Insert(0, new CodeDao() { CLSS_CD = "", CLSS_DESC = "" });
            //_DeptMap = SystemProperties.SYSTEM_DEPT_MAP();

            //M_DEPT_DESC = SystemProperties.USER_VO.GRP_NM;
            //Refresh();
        }

        [Command]
        public async void Refresh(string _SL_BIL_RTN_NO = null)
        {
            try
            {
                //DXSplashScreen.Show<ProgressWindow>();
                SearchDetail = null;
                SelectDtlList = null;

                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s2217/mst", new StringContent(JsonConvert.SerializeObject(new SaleVo() { FM_DT = (StartDt).ToString("yyyy-MM-dd"), TO_DT = (EndDt).ToString("yyyy-MM-dd"), AREA_CD = M_SL_AREA_NM.CLSS_CD, GBN = "Q", CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectMstList = JsonConvert.DeserializeObject<IEnumerable<SaleVo>>(await response.Content.ReadAsStringAsync()).Cast<SaleVo>().ToList();
                    }

                    //SelectMstList = saleOrderClient.S2217SelectMstList(new SaleVo() { FM_DT = (StartDt).ToString("yyyy-MM-dd"), TO_DT = (EndDt).ToString("yyyy-MM-dd"), AREA_CD = _AreaMap[TXT_SL_AREA_NM], GBN = "Q" });
                    ////
                    setTitle();
                    //Title = "[기간]" + (StartDt).ToString("yyyy-MM-dd") + "~" + (EndDt).ToString("yyyy-MM-dd") + ", " + (string.IsNullOrEmpty(M_SEARCH_TEXT) ? "" : (",   [검 색]" + M_SEARCH_TEXT));

                    if (SelectMstList.Count >= 1)
                    {
                        isM_UPDATE = true;
                        isM_DELETE = true;
                        isM_SAVE = true;

                        if (string.IsNullOrEmpty(_SL_BIL_RTN_NO))
                        {
                            SelectedMstItem = SelectMstList[0];
                        }
                        else
                        {
                            SelectedMstItem = SelectMstList.Where(x => x.SL_BIL_RTN_NO.Equals(_SL_BIL_RTN_NO)).LastOrDefault<SaleVo>();
                        }
                    }
                    else
                    {
                        SearchDetail = null;
                        SelectDtlList = null;

                        isM_UPDATE = false;
                        isM_DELETE = false;
                        isM_SAVE = false;
                        //
                        isD_UPDATE = false;
                        isD_DELETE = false;
                    }
                }

                //if (SystemProperties.USER == "134")
                ////if (UserList.Any<CodeDao>(x => x.CLSS_CD.Equals(SystemProperties.USER)))
                ////{
                //isM_Cancel = true;
                ////}

                //DXSplashScreen.Close();
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
                    SetProperty(ref _selectedMstItem, value, () => SelectedMstItem, SelectMstDetail);
                }
            }
        }

        private bool? _isM_SAVE = false;
        public bool? isM_SAVE
        {
            get { return _isM_SAVE; }
            set { SetProperty(ref _isM_SAVE, value, () => isM_SAVE); }
        }

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


        private bool? _isM_Cancel = false;
        public bool? isM_Cancel
        {
            get { return _isM_Cancel; }
            set { SetProperty(ref _isM_Cancel, value, () => isM_Cancel); }
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
        private SystemCodeVo _M_SL_AREA_NM;
        public SystemCodeVo M_SL_AREA_NM
        {
            get { return _M_SL_AREA_NM; }
            set { SetProperty(ref _M_SL_AREA_NM, value, () => M_SL_AREA_NM); }
        }

        ////사업장
        //private CodeDao _M_SL_AREA_NM;
        //public CodeDao M_SL_AREA_NM
        //{
        //    get { return _M_SL_AREA_NM; }
        //    set { SetProperty(ref _M_SL_AREA_NM, value, () => M_SL_AREA_NM); }
        //}


        [Command]
        public async void SelectMstDetail()
        {
            try
            {
                //DXSplashScreen.Show<ProgressWindow>();
                if (SelectedMstItem == null)
                {
                    return;
                }
                else if (SelectedMstItem.RTN_APRO_FLG.Equals("N"))
                {
                    this.isD_UPDATE = true;

                    //
                    isM_UPDATE = true;
                    isM_DELETE = true;

                    isM_Cancel = false;
                }
                else
                {
                    this.isD_UPDATE = false;

                    //
                    isM_UPDATE = false;
                    isM_DELETE = false;

                    isM_Cancel = true;
                }


                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s2217/dtl", new StringContent(JsonConvert.SerializeObject(SelectedMstItem), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectDtlList = JsonConvert.DeserializeObject<IEnumerable<SaleVo>>(await response.Content.ReadAsStringAsync()).Cast<SaleVo>().ToList();
                    }

                    //SelectDtlList = new ObservableCollection<PurVo>(purClient.P4401SelectDtlList(new PurVo() { ITM_CD = this._selectedMstItem.}));
                    //SelectDtlList = saleOrderClient.S2217SelectDtlList(SelectedMstItem);
                    // //
                    if (SelectDtlList.Count >= 1)
                    {
                        //isD_UPDATE = true;
                        isD_DELETE = true;

                        SearchDetail = SelectDtlList[0];
                    }
                    else
                    {
                        //isD_UPDATE = false;
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

        [Command]
        public async void CancelContact()
        {
            try
            {
                if (SelectedMstItem == null)
                {
                    return;
                }
           
                MessageBoxResult result = WinUIMessageBox.Show("[요청 번호 명 : " + SelectedMstItem.SL_BIL_RTN_NO + "] 정말로 승인해제 하시겠습니까? ", this.title, MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    SelectedMstItem.CRE_USR_ID = SystemProperties.USER;
                    SelectedMstItem.UPD_USR_ID = SystemProperties.USER;
                    SelectedMstItem.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;

                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s2217/Cancel", new StringContent(JsonConvert.SerializeObject(SelectedMstItem), System.Text.Encoding.UTF8, "application/json")))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            JsonConvert.DeserializeObject<int?>(await response.Content.ReadAsStringAsync());
                        }
                        WinUIMessageBox.Show("[요청 번호 명 : " + SelectedMstItem.SL_BIL_RTN_NO + "] 승인해제 하였습니다.", title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                        Refresh(SelectedMstItem.SL_BIL_RTN_NO);
                    }
                }
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }
        
        [Command]
        public async void ApplyContact()
        {
            try
            {
                if (SelectedMstItem == null)
                {
                    return;
                }

                MessageBoxResult result = WinUIMessageBox.Show("[" + SelectedMstItem.SL_BIL_RTN_NO + "/" + SelectedMstItem.SL_CO_NM + "]" + " 정말로 입고 승인 하시겠습니까?", title, MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    SelectedMstItem.CRE_USR_ID = SystemProperties.USER;
                    SelectedMstItem.UPD_USR_ID = SystemProperties.USER;
                    SelectedMstItem.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;

                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s2217/Apply", new StringContent(JsonConvert.SerializeObject(SelectedMstItem), System.Text.Encoding.UTF8, "application/json")))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            JsonConvert.DeserializeObject<int?>(await response.Content.ReadAsStringAsync());
                        }
                        WinUIMessageBox.Show("입고 승인 완료되었습니다.", title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                        Refresh(SelectedMstItem.SL_BIL_RTN_NO);
                    }
                }
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
}


        
        [Command]
        public void EditUsrContact()
        {
            if (SelectedMstItem == null)
            {
                return;
            }
            //else if (SelectMstList.Count <= 0)
            //{
            //    return;
            //}

            masterUsrDialog = new S2216MasterDialog(SelectedMstItem);
            masterUsrDialog.Title = "반품 입고 담당자 등록 - [반품 번호 : " + SelectedMstItem.SL_BIL_RTN_NO + "]";
            masterUsrDialog.Owner = Application.Current.MainWindow;
            masterUsrDialog.BorderEffect = BorderEffect.Default;
            masterUsrDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            masterUsrDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            bool isDialog = (bool)masterUsrDialog.ShowDialog();
            if (isDialog)
            {
                Refresh(masterUsrDialog.SL_BIL_RTN_NO);
            }


                //    //if (masterDialog.IsEdit == false)
                //    //{
                //    //    try
                //    //    {
                //    //        DXSplashScreen.Show<ProgressWindow>();
                //    //        Refresh();
                //    //        DXSplashScreen.Close();
                //    //    }
                //    //    catch (System.Exception eLog)
                //    //    {
                //    //        DXSplashScreen.Close();
                //    //        WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                //    //        return;
                //    //    }
                //    //}
                //    //else
                //    //{
                //    //    if (SelectedMstItem.CLZ_FLG.Equals("Y"))
                //    //    {
                //    //        try
                //    //        {
                //    //            DXSplashScreen.Show<ProgressWindow>();
                //    //            SelectMstDetail();
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
            }


        public void setTitle()
        {
            Title = "[기간]" + (StartDt).ToString("yyyy-MM-dd") + "~" + (EndDt).ToString("yyyy-MM-dd") + ",    [사업장]" + M_SL_AREA_NM.CLSS_DESC;
        }


        //[Command]
        //public void ReportContact()
        //{
        //    if (SelectedMstItem == null) { return; }

        //    //return chit
        //    ObservableCollection<SaleVo> reportItems = new ObservableCollection<SaleVo>();
        //    IList<SaleVo> allItems = SelectDtlList;

        //    if (allItems == null)
        //    {
        //        return;
        //    }
        //    else if (allItems.Count <= 0)
        //    {
        //        WinUIMessageBox.Show("데이터가 존재 하지 않습니다.", "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
        //        return;
        //    }
        //    else if (allItems.Count <= 12)
        //    {
        //        reportItems.Add(new SaleVo());
        //        for (int x = 0; x < allItems.Count; x++)
        //        {
        //            if (x == 0)
        //            {
        //                reportItems[0].A1 = allItems[x].RN;
        //                reportItems[0].A2 = allItems[x].ITM_NM;
        //                reportItems[0].A3 = allItems[x].ITM_SZ_NM;
        //                reportItems[0].A4 = allItems[x].UOM_NM;
        //                reportItems[0].A5 = allItems[x].SL_RTN_ITM_QTY;
        //                reportItems[0].A6 = allItems[x].MD_QTY;
        //                reportItems[0].A7 = allItems[x].SL_RTN_NM;
        //                reportItems[0].A8 = allItems[x].SL_BAD_ITM_NM;
        //                reportItems[0].A9 = allItems[x].ITM_LOC_NM;


        //                reportItems[0].O1 = reportItems[0].A6;
        //                reportItems[0].O2 = reportItems[0].A7;
        //                reportItems[0].O3 = float.Parse(reportItems[0].O1.ToString()) + float.Parse(reportItems[0].O2.ToString());

        //                reportItems[0].SL_BIL_RTN_DT = allItems[x].SL_BIL_RTN_DT;
        //                reportItems[0].CO_NM = allItems[x].CO_NM;
        //                reportItems[0].SL_BIL_DT = allItems[x].SL_BIL_DT;
        //                reportItems[0].SL_RTN_NM = allItems[x].SL_RTN_NM;

        //                reportItems[0].SL_BIL_NO = allItems[x].SL_BIL_NO;
        //                reportItems[0].RTN_AFT_DESC = SelectedMstItem.RTN_AFT_DESC;
        //                reportItems[0].RTN_AFT_A_DESC = SelectedMstItem.RTN_AFT_A_DESC;
        //                reportItems[0].RTN_AFT_B_DESC = SelectedMstItem.RTN_AFT_B_DESC;
        //                reportItems[0].RTN_AFT_C_DESC = SelectedMstItem.RTN_AFT_C_DESC;
        //                reportItems[0].SL_ITM_RMK = allItems[x].SL_ITM_RMK;
        //                reportItems[0].PRN_DT = "출력일시 : " + DateTime.Now.ToString("yyyy-MM-dd HH:mm");
        //                reportItems[0].PRN_DT = "출력일시 : " + allItems[x].PRN_DT;
        //            }
        //            else if (x == 1)
        //            {
        //                reportItems[0].B1 = allItems[x].RN;
        //                reportItems[0].B2 = allItems[x].ITM_NM;
        //                reportItems[0].B3 = allItems[x].ITM_SZ_NM;
        //                reportItems[0].B4 = allItems[x].UOM_NM;
        //                reportItems[0].B5 = allItems[x].SL_RTN_ITM_QTY;
        //                reportItems[0].B6 = allItems[x].MD_QTY;
        //                reportItems[0].B7 = allItems[x].SL_RTN_NM;
        //                reportItems[0].B8 = allItems[x].SL_BAD_ITM_NM;
        //                reportItems[0].B9 = allItems[x].ITM_LOC_NM;
        //            }
        //            else if (x == 2)
        //            {
        //                reportItems[0].C1 = allItems[x].RN;
        //                reportItems[0].C2 = allItems[x].ITM_NM;
        //                reportItems[0].C3 = allItems[x].ITM_SZ_NM;
        //                reportItems[0].C4 = allItems[x].UOM_NM;
        //                reportItems[0].C5 = allItems[x].SL_RTN_ITM_QTY;
        //                reportItems[0].C6 = allItems[x].MD_QTY;
        //                reportItems[0].C7 = allItems[x].SL_RTN_NM;
        //                reportItems[0].C8 = allItems[x].SL_BAD_ITM_NM;
        //                reportItems[0].C9 = allItems[x].ITM_LOC_NM;
        //            }
        //            else if (x == 3)
        //            {
        //                reportItems[0].D1 = allItems[x].RN;
        //                reportItems[0].D2 = allItems[x].ITM_NM;
        //                reportItems[0].D3 = allItems[x].ITM_SZ_NM;
        //                reportItems[0].D4 = allItems[x].UOM_NM;
        //                reportItems[0].D5 = allItems[x].SL_RTN_ITM_QTY;
        //                reportItems[0].D6 = allItems[x].MD_QTY;
        //                reportItems[0].D7 = allItems[x].SL_RTN_NM;
        //                reportItems[0].D8 = allItems[x].SL_BAD_ITM_NM;
        //                reportItems[0].D9 = allItems[x].ITM_LOC_NM;
        //            }
        //            else if (x == 4)
        //            {
        //                reportItems[0].E1 = allItems[x].RN;
        //                reportItems[0].E2 = allItems[x].ITM_NM;
        //                reportItems[0].E3 = allItems[x].ITM_SZ_NM;
        //                reportItems[0].E4 = allItems[x].UOM_NM;
        //                reportItems[0].E5 = allItems[x].SL_RTN_ITM_QTY;
        //                reportItems[0].E6 = allItems[x].MD_QTY;
        //                reportItems[0].E7 = allItems[x].SL_RTN_NM;
        //                reportItems[0].E8 = allItems[x].SL_BAD_ITM_NM;
        //                reportItems[0].E9 = allItems[x].ITM_LOC_NM;
        //            }
        //            else if (x == 5)
        //            {
        //                reportItems[0].F1 = allItems[x].RN;
        //                reportItems[0].F2 = allItems[x].ITM_NM;
        //                reportItems[0].F3 = allItems[x].ITM_SZ_NM;
        //                reportItems[0].F4 = allItems[x].UOM_NM;
        //                reportItems[0].F5 = allItems[x].SL_RTN_ITM_QTY;
        //                reportItems[0].F6 = allItems[x].MD_QTY;
        //                reportItems[0].F7 = allItems[x].SL_RTN_NM;
        //                reportItems[0].F8 = allItems[x].SL_BAD_ITM_NM;
        //                reportItems[0].F9 = allItems[x].ITM_LOC_NM;
        //            }
        //            else if (x == 6)
        //            {
        //                reportItems[0].G1 = allItems[x].RN;
        //                reportItems[0].G2 = allItems[x].ITM_NM;
        //                reportItems[0].G3 = allItems[x].ITM_SZ_NM;
        //                reportItems[0].G4 = allItems[x].UOM_NM;
        //                reportItems[0].G5 = allItems[x].SL_RTN_ITM_QTY;
        //                reportItems[0].G6 = allItems[x].MD_QTY;
        //                reportItems[0].G7 = allItems[x].SL_RTN_NM;
        //                reportItems[0].G8 = allItems[x].SL_BAD_ITM_NM;
        //                reportItems[0].G9 = allItems[x].ITM_LOC_NM;
        //            }
        //            else if (x == 7)
        //            {
        //                reportItems[0].H1 = allItems[x].RN;
        //                reportItems[0].H2 = allItems[x].ITM_NM;
        //                reportItems[0].H3 = allItems[x].ITM_SZ_NM;
        //                reportItems[0].H4 = allItems[x].UOM_NM;
        //                reportItems[0].H5 = allItems[x].SL_RTN_ITM_QTY;
        //                reportItems[0].H6 = allItems[x].MD_QTY;
        //                reportItems[0].H7 = allItems[x].SL_RTN_NM;
        //                reportItems[0].H8 = allItems[x].SL_BAD_ITM_NM;
        //                reportItems[0].H9 = allItems[x].ITM_LOC_NM;
        //            }
        //            else if (x == 8)
        //            {
        //                reportItems[0].I1 = allItems[x].RN;
        //                reportItems[0].I2 = allItems[x].ITM_NM;
        //                reportItems[0].I3 = allItems[x].ITM_SZ_NM;
        //                reportItems[0].I4 = allItems[x].UOM_NM;
        //                reportItems[0].I5 = allItems[x].SL_RTN_ITM_QTY;
        //                reportItems[0].I6 = allItems[x].MD_QTY;
        //                reportItems[0].I7 = allItems[x].SL_RTN_NM;
        //                reportItems[0].I8 = allItems[x].SL_BAD_ITM_NM;
        //                reportItems[0].I9 = allItems[x].ITM_LOC_NM;
        //            }
        //            else if (x == 9)
        //            {
        //                reportItems[0].J1 = allItems[x].RN;
        //                reportItems[0].J2 = allItems[x].ITM_NM;
        //                reportItems[0].J3 = allItems[x].ITM_SZ_NM;
        //                reportItems[0].J4 = allItems[x].UOM_NM;
        //                reportItems[0].J5 = allItems[x].SL_RTN_ITM_QTY;
        //                reportItems[0].J6 = allItems[x].MD_QTY;
        //                reportItems[0].J7 = allItems[x].SL_RTN_NM;
        //                reportItems[0].J8 = allItems[x].SL_BAD_ITM_NM;
        //                reportItems[0].J9 = allItems[x].ITM_LOC_NM;
        //            }
        //            else if (x == 10)
        //            {
        //                reportItems[0].K1 = allItems[x].RN;
        //                reportItems[0].K2 = allItems[x].ITM_NM;
        //                reportItems[0].K3 = allItems[x].ITM_SZ_NM;
        //                reportItems[0].K4 = allItems[x].UOM_NM;
        //                reportItems[0].K5 = allItems[x].SL_RTN_ITM_QTY;
        //                reportItems[0].K6 = allItems[x].MD_QTY;
        //                reportItems[0].K7 = allItems[x].SL_RTN_NM;
        //                reportItems[0].K8 = allItems[x].SL_BAD_ITM_NM;
        //                reportItems[0].K9 = allItems[x].ITM_LOC_NM;
        //            }
        //            else if (x == 11)
        //            {
        //                reportItems[0].L1 = allItems[x].RN;
        //                reportItems[0].L2 = allItems[x].ITM_NM;
        //                reportItems[0].L3 = allItems[x].ITM_SZ_NM;
        //                reportItems[0].L4 = allItems[x].UOM_NM;
        //                reportItems[0].L5 = allItems[x].SL_RTN_ITM_QTY;
        //                reportItems[0].L6 = allItems[x].MD_QTY;
        //                reportItems[0].L7 = allItems[x].SL_RTN_NM;
        //                reportItems[0].L8 = allItems[x].SL_BAD_ITM_NM;
        //                reportItems[0].L9 = allItems[x].ITM_LOC_NM;
        //            }
        //        }
        //        allItems[0].PAGE_NUM = "1/1";
        //    }
        //    else
        //    {
        //        Page 나누기
        //        int nTotal = allItems.Count;
        //        int nPage = nTotal / 12;
        //        int mod = ((int)nTotal % 12);


        //        int min = 0;
        //        int max = 0;

        //        for (int z = 0; z < nPage; z++)
        //        {
        //            min = z * 12;
        //            max = min + 12;

        //            reportItems.Insert(z, new SaleVo());
        //            for (int x = min; x < max; x++)
        //            {
        //                allItems[z].PAGE_NUM = (z + 1) + "/" + nPage;
        //                if ((x % 12) == 0)
        //                {
        //                    reportItems[z].A1 = allItems[x].RN;
        //                    reportItems[z].A2 = allItems[x].ITM_NM;
        //                    reportItems[z].A3 = allItems[x].ITM_SZ_NM;
        //                    reportItems[z].A4 = allItems[x].UOM_NM;
        //                    reportItems[z].A5 = allItems[x].SL_RTN_ITM_QTY;
        //                    reportItems[z].A6 = allItems[x].MD_QTY;
        //                    reportItems[z].A7 = allItems[x].SL_RTN_NM;
        //                    reportItems[z].A8 = allItems[x].SL_BAD_ITM_NM;
        //                    reportItems[z].A9 = allItems[x].ITM_LOC_NM;

        //                    reportItems[z].O1 = reportItems[z].A6;
        //                    reportItems[z].O2 = reportItems[z].A7;
        //                    reportItems[z].O3 = float.Parse(reportItems[z].O1.ToString()) + float.Parse(reportItems[z].O2.ToString());

        //                    reportItems[z].SL_BIL_RTN_DT = allItems[x].SL_BIL_RTN_DT;
        //                    reportItems[z].CO_NM = allItems[x].CO_NM;
        //                    reportItems[z].SL_BIL_DT = allItems[x].SL_BIL_DT;
        //                    reportItems[z].SL_RTN_NM = allItems[x].SL_RTN_NM;

        //                    reportItems[z].SL_BIL_NO = allItems[x].SL_BIL_NO;
        //                    reportItems[z].RTN_AFT_DESC = SelectedMstItem.RTN_AFT_DESC;
        //                    reportItems[z].RTN_AFT_A_DESC = SelectedMstItem.RTN_AFT_A_DESC;
        //                    reportItems[z].RTN_AFT_B_DESC = SelectedMstItem.RTN_AFT_B_DESC;
        //                    reportItems[z].RTN_AFT_C_DESC = SelectedMstItem.RTN_AFT_C_DESC;
        //                    reportItems[z].SL_ITM_RMK = allItems[x].SL_ITM_RMK;
        //                    reportItems[z].PRN_DT = "출력일시 : " + DateTime.Now.ToString("yyyy-MM-dd HH:mm");
        //                    reportItems[z].PRN_DT = "출력일시 : " + allItems[x].PRN_DT;
        //                }
        //                else if ((x % 12) == 1)
        //                {
        //                    reportItems[z].B1 = allItems[x].RN;
        //                    reportItems[z].B2 = allItems[x].ITM_NM;
        //                    reportItems[z].B3 = allItems[x].ITM_SZ_NM;
        //                    reportItems[z].B4 = allItems[x].UOM_NM;
        //                    reportItems[z].B5 = allItems[x].SL_RTN_ITM_QTY;
        //                    reportItems[z].B6 = allItems[x].MD_QTY;
        //                    reportItems[z].B7 = allItems[x].SL_RTN_NM;
        //                    reportItems[z].B8 = allItems[x].SL_BAD_ITM_NM;
        //                    reportItems[z].B9 = allItems[x].ITM_LOC_NM;
        //                }
        //                else if ((x % 12) == 2)
        //                {
        //                    reportItems[z].C1 = allItems[x].RN;
        //                    reportItems[z].C2 = allItems[x].ITM_NM;
        //                    reportItems[z].C3 = allItems[x].ITM_SZ_NM;
        //                    reportItems[z].C4 = allItems[x].UOM_NM;
        //                    reportItems[z].C5 = allItems[x].SL_RTN_ITM_QTY;
        //                    reportItems[z].C6 = allItems[x].MD_QTY;
        //                    reportItems[z].C7 = allItems[x].SL_RTN_NM;
        //                    reportItems[z].C8 = allItems[x].SL_BAD_ITM_NM;
        //                    reportItems[z].C9 = allItems[x].ITM_LOC_NM;
        //                }
        //                else if ((x % 12) == 3)
        //                {
        //                    reportItems[z].D1 = allItems[x].RN;
        //                    reportItems[z].D2 = allItems[x].ITM_NM;
        //                    reportItems[z].D3 = allItems[x].ITM_SZ_NM;
        //                    reportItems[z].D4 = allItems[x].UOM_NM;
        //                    reportItems[z].D5 = allItems[x].SL_RTN_ITM_QTY;
        //                    reportItems[z].D6 = allItems[x].MD_QTY;
        //                    reportItems[z].D7 = allItems[x].SL_RTN_NM;
        //                    reportItems[z].D8 = allItems[x].SL_BAD_ITM_NM;
        //                    reportItems[z].D9 = allItems[x].ITM_LOC_NM;
        //                }
        //                else if ((x % 12) == 4)
        //                {
        //                    reportItems[z].E1 = allItems[x].RN;
        //                    reportItems[z].E2 = allItems[x].ITM_NM;
        //                    reportItems[z].E3 = allItems[x].ITM_SZ_NM;
        //                    reportItems[z].E4 = allItems[x].UOM_NM;
        //                    reportItems[z].E5 = allItems[x].SL_RTN_ITM_QTY;
        //                    reportItems[z].E6 = allItems[x].MD_QTY;
        //                    reportItems[z].E7 = allItems[x].SL_RTN_NM;
        //                    reportItems[z].E8 = allItems[x].SL_BAD_ITM_NM;
        //                    reportItems[z].E9 = allItems[x].ITM_LOC_NM;
        //                }
        //                else if ((x % 12) == 5)
        //                {
        //                    reportItems[z].F1 = allItems[x].RN;
        //                    reportItems[z].F2 = allItems[x].ITM_NM;
        //                    reportItems[z].F3 = allItems[x].ITM_SZ_NM;
        //                    reportItems[z].F4 = allItems[x].UOM_NM;
        //                    reportItems[z].F5 = allItems[x].SL_RTN_ITM_QTY;
        //                    reportItems[z].F6 = allItems[x].MD_QTY;
        //                    reportItems[z].F7 = allItems[x].SL_RTN_NM;
        //                    reportItems[z].E8 = allItems[x].SL_BAD_ITM_NM;
        //                    reportItems[z].E9 = allItems[x].ITM_LOC_NM;
        //                }
        //                else if ((x % 12) == 6)
        //                {
        //                    reportItems[z].G1 = allItems[x].RN;
        //                    reportItems[z].G2 = allItems[x].ITM_NM;
        //                    reportItems[z].G3 = allItems[x].ITM_SZ_NM;
        //                    reportItems[z].G4 = allItems[x].UOM_NM;
        //                    reportItems[z].G5 = allItems[x].SL_RTN_ITM_QTY;
        //                    reportItems[z].G6 = allItems[x].MD_QTY;
        //                    reportItems[z].G7 = allItems[x].SL_RTN_NM;
        //                    reportItems[z].G8 = allItems[x].SL_BAD_ITM_NM;
        //                    reportItems[z].G9 = allItems[x].ITM_LOC_NM;
        //                }
        //                else if ((x % 12) == 7)
        //                {
        //                    reportItems[z].H1 = allItems[x].RN;
        //                    reportItems[z].H2 = allItems[x].ITM_NM;
        //                    reportItems[z].H3 = allItems[x].ITM_SZ_NM;
        //                    reportItems[z].H4 = allItems[x].UOM_NM;
        //                    reportItems[z].H5 = allItems[x].SL_RTN_ITM_QTY;
        //                    reportItems[z].H6 = allItems[x].MD_QTY;
        //                    reportItems[z].H7 = allItems[x].SL_RTN_NM;
        //                    reportItems[z].H8 = allItems[x].SL_BAD_ITM_NM;
        //                    reportItems[z].H9 = allItems[x].ITM_LOC_NM;
        //                }
        //                else if ((x % 12) == 8)
        //                {
        //                    reportItems[z].I1 = allItems[x].RN;
        //                    reportItems[z].I2 = allItems[x].ITM_NM;
        //                    reportItems[z].I3 = allItems[x].ITM_SZ_NM;
        //                    reportItems[z].I4 = allItems[x].UOM_NM;
        //                    reportItems[z].I5 = allItems[x].SL_RTN_ITM_QTY;
        //                    reportItems[z].I6 = allItems[x].MD_QTY;
        //                    reportItems[z].I7 = allItems[x].SL_RTN_NM;
        //                    reportItems[z].I8 = allItems[x].SL_BAD_ITM_NM;
        //                    reportItems[z].I9 = allItems[x].ITM_LOC_NM;
        //                }
        //                else if ((x % 12) == 9)
        //                {
        //                    reportItems[z].J1 = allItems[x].RN;
        //                    reportItems[z].J2 = allItems[x].ITM_NM;
        //                    reportItems[z].J3 = allItems[x].ITM_SZ_NM;
        //                    reportItems[z].J4 = allItems[x].UOM_NM;
        //                    reportItems[z].J5 = allItems[x].SL_RTN_ITM_QTY;
        //                    reportItems[z].J6 = allItems[x].MD_QTY;
        //                    reportItems[z].J7 = allItems[x].SL_RTN_NM;
        //                    reportItems[z].J8 = allItems[x].SL_BAD_ITM_NM;
        //                    reportItems[z].J9 = allItems[x].ITM_LOC_NM;
        //                }
        //                else if ((x % 12) == 10)
        //                {
        //                    reportItems[z].K1 = allItems[x].RN;
        //                    reportItems[z].K2 = allItems[x].ITM_NM;
        //                    reportItems[z].K3 = allItems[x].ITM_SZ_NM;
        //                    reportItems[z].K4 = allItems[x].UOM_NM;
        //                    reportItems[z].K5 = allItems[x].SL_RTN_ITM_QTY;
        //                    reportItems[z].K6 = allItems[x].MD_QTY;
        //                    reportItems[z].K7 = allItems[x].SL_RTN_NM;
        //                    reportItems[z].K8 = allItems[x].SL_BAD_ITM_NM;
        //                    reportItems[z].K9 = allItems[x].ITM_LOC_NM;
        //                }
        //                else if ((x % 12) == 11)
        //                {
        //                    reportItems[z].L1 = allItems[x].RN;
        //                    reportItems[z].L2 = allItems[x].ITM_NM;
        //                    reportItems[z].L3 = allItems[x].ITM_SZ_NM;
        //                    reportItems[z].L4 = allItems[x].UOM_NM;
        //                    reportItems[z].L5 = allItems[x].SL_RTN_ITM_QTY;
        //                    reportItems[z].L6 = allItems[x].MD_QTY;
        //                    reportItems[z].L7 = allItems[x].SL_RTN_NM;
        //                    reportItems[z].L8 = allItems[x].SL_BAD_ITM_NM;
        //                    reportItems[z].L9 = allItems[x].ITM_LOC_NM;
        //                }
        //            }
        //        }

        //        나머지 계산
        //        if (mod != 0)
        //        {
        //            min = nPage * 12;
        //            reportItems.Insert(nPage, new SaleVo());
        //            for (int x = min; x < allItems.Count; x++)
        //            {
        //                if ((x % 12) == 0)
        //                {
        //                    reportItems[nPage].A1 = allItems[x].RN;
        //                    reportItems[nPage].A2 = allItems[x].ITM_NM;
        //                    reportItems[nPage].A3 = allItems[x].ITM_SZ_NM;
        //                    reportItems[nPage].A4 = allItems[x].UOM_NM;
        //                    reportItems[nPage].A5 = allItems[x].SL_RTN_ITM_QTY;
        //                    reportItems[nPage].A6 = allItems[x].MD_QTY;
        //                    reportItems[nPage].A7 = allItems[x].SL_RTN_NM;
        //                    reportItems[nPage].A8 = allItems[x].SL_BAD_ITM_NM;
        //                    reportItems[nPage].A9 = allItems[x].ITM_LOC_NM;


        //                    reportItems[nPage].SL_BIL_RTN_DT = allItems[x].SL_BIL_RTN_DT;
        //                    reportItems[nPage].CO_NM = allItems[x].CO_NM;
        //                    reportItems[nPage].SL_BIL_DT = allItems[x].SL_BIL_DT;
        //                    reportItems[nPage].SL_RTN_NM = allItems[x].SL_RTN_NM;

        //                    reportItems[nPage].SL_BIL_NO = allItems[x].SL_BIL_NO;
        //                    reportItems[nPage].RTN_AFT_DESC = SelectedMstItem.RTN_AFT_DESC;
        //                    reportItems[nPage].RTN_AFT_A_DESC = SelectedMstItem.RTN_AFT_A_DESC;
        //                    reportItems[nPage].RTN_AFT_B_DESC = SelectedMstItem.RTN_AFT_B_DESC;
        //                    reportItems[nPage].RTN_AFT_C_DESC = SelectedMstItem.RTN_AFT_C_DESC;
        //                    reportItems[nPage].SL_ITM_RMK = allItems[x].SL_ITM_RMK;
        //                    reportItems[nPage].PRN_DT = "출력일시 : " + DateTime.Now.ToString("yyyy-MM-dd HH:mm");
        //                    reportItems[nPage].PRN_DT = "출력일시 : " + allItems[x].PRN_DT;
        //                }
        //                else if ((x % 12) == 1)
        //                {
        //                    reportItems[nPage].B1 = allItems[x].RN;
        //                    reportItems[nPage].B2 = allItems[x].ITM_NM;
        //                    reportItems[nPage].B3 = allItems[x].ITM_SZ_NM;
        //                    reportItems[nPage].B4 = allItems[x].UOM_NM;
        //                    reportItems[nPage].B5 = allItems[x].SL_RTN_ITM_QTY;
        //                    reportItems[nPage].B6 = allItems[x].MD_QTY;
        //                    reportItems[nPage].B7 = allItems[x].SL_RTN_NM;
        //                    reportItems[nPage].B8 = allItems[x].SL_BAD_ITM_NM;
        //                    reportItems[nPage].B9 = allItems[x].ITM_LOC_NM;
        //                }
        //                else if ((x % 12) == 2)
        //                {
        //                    reportItems[nPage].C1 = allItems[x].RN;
        //                    reportItems[nPage].C2 = allItems[x].ITM_NM;
        //                    reportItems[nPage].C3 = allItems[x].ITM_SZ_NM;
        //                    reportItems[nPage].C4 = allItems[x].UOM_NM;
        //                    reportItems[nPage].C5 = allItems[x].SL_RTN_ITM_QTY;
        //                    reportItems[nPage].C6 = allItems[x].MD_QTY;
        //                    reportItems[nPage].C7 = allItems[x].SL_RTN_NM;
        //                    reportItems[nPage].C8 = allItems[x].SL_BAD_ITM_NM;
        //                    reportItems[nPage].C9 = allItems[x].ITM_LOC_NM;
        //                }
        //                else if ((x % 12) == 3)
        //                {
        //                    reportItems[nPage].D1 = allItems[x].RN;
        //                    reportItems[nPage].D2 = allItems[x].ITM_NM;
        //                    reportItems[nPage].D3 = allItems[x].ITM_SZ_NM;
        //                    reportItems[nPage].D4 = allItems[x].UOM_NM;
        //                    reportItems[nPage].D5 = allItems[x].SL_RTN_ITM_QTY;
        //                    reportItems[nPage].D6 = allItems[x].MD_QTY;
        //                    reportItems[nPage].D7 = allItems[x].SL_RTN_NM;
        //                    reportItems[nPage].D8 = allItems[x].SL_BAD_ITM_NM;
        //                    reportItems[nPage].D9 = allItems[x].ITM_LOC_NM;
        //                }
        //                else if ((x % 12) == 4)
        //                {
        //                    reportItems[nPage].E1 = allItems[x].RN;
        //                    reportItems[nPage].E2 = allItems[x].ITM_NM;
        //                    reportItems[nPage].E3 = allItems[x].ITM_SZ_NM;
        //                    reportItems[nPage].E4 = allItems[x].UOM_NM;
        //                    reportItems[nPage].E5 = allItems[x].SL_RTN_ITM_QTY;
        //                    reportItems[nPage].E6 = allItems[x].MD_QTY;
        //                    reportItems[nPage].E7 = allItems[x].SL_RTN_NM;
        //                    reportItems[nPage].E8 = allItems[x].SL_BAD_ITM_NM;
        //                    reportItems[nPage].E9 = allItems[x].ITM_LOC_NM;
        //                }
        //                else if ((x % 12) == 5)
        //                {
        //                    reportItems[nPage].F1 = allItems[x].RN;
        //                    reportItems[nPage].F2 = allItems[x].ITM_NM;
        //                    reportItems[nPage].F3 = allItems[x].ITM_SZ_NM;
        //                    reportItems[nPage].F4 = allItems[x].UOM_NM;
        //                    reportItems[nPage].F5 = allItems[x].SL_RTN_ITM_QTY;
        //                    reportItems[nPage].F6 = allItems[x].MD_QTY;
        //                    reportItems[nPage].F7 = allItems[x].SL_RTN_NM;
        //                    reportItems[nPage].F8 = allItems[x].SL_BAD_ITM_NM;
        //                    reportItems[nPage].F9 = allItems[x].ITM_LOC_NM;
        //                }
        //                else if ((x % 12) == 6)
        //                {
        //                    reportItems[nPage].G1 = allItems[x].RN;
        //                    reportItems[nPage].G2 = allItems[x].ITM_NM;
        //                    reportItems[nPage].G3 = allItems[x].ITM_SZ_NM;
        //                    reportItems[nPage].G4 = allItems[x].UOM_NM;
        //                    reportItems[nPage].G5 = allItems[x].SL_RTN_ITM_QTY;
        //                    reportItems[nPage].G6 = allItems[x].MD_QTY;
        //                    reportItems[nPage].G7 = allItems[x].SL_RTN_NM;
        //                    reportItems[nPage].G8 = allItems[x].SL_BAD_ITM_NM;
        //                    reportItems[nPage].G9 = allItems[x].ITM_LOC_NM;
        //                }
        //                else if ((x % 12) == 7)
        //                {
        //                    reportItems[nPage].H1 = allItems[x].RN;
        //                    reportItems[nPage].H2 = allItems[x].ITM_NM;
        //                    reportItems[nPage].H3 = allItems[x].ITM_SZ_NM;
        //                    reportItems[nPage].H4 = allItems[x].UOM_NM;
        //                    reportItems[nPage].H5 = allItems[x].SL_RTN_ITM_QTY;
        //                    reportItems[nPage].H6 = allItems[x].MD_QTY;
        //                    reportItems[nPage].H7 = allItems[x].SL_RTN_NM;
        //                    reportItems[nPage].H8 = allItems[x].SL_BAD_ITM_NM;
        //                    reportItems[nPage].H9 = allItems[x].ITM_LOC_NM;
        //                }
        //                else if ((x % 12) == 8)
        //                {
        //                    reportItems[nPage].I1 = allItems[x].RN;
        //                    reportItems[nPage].I2 = allItems[x].ITM_NM;
        //                    reportItems[nPage].I3 = allItems[x].ITM_SZ_NM;
        //                    reportItems[nPage].I4 = allItems[x].UOM_NM;
        //                    reportItems[nPage].I5 = allItems[x].SL_RTN_ITM_QTY;
        //                    reportItems[nPage].I6 = allItems[x].MD_QTY;
        //                    reportItems[nPage].I7 = allItems[x].SL_RTN_NM;
        //                    reportItems[nPage].I8 = allItems[x].SL_BAD_ITM_NM;
        //                    reportItems[nPage].I9 = allItems[x].ITM_LOC_NM;
        //                }
        //                else if ((x % 12) == 9)
        //                {
        //                    reportItems[nPage].J1 = allItems[x].RN;
        //                    reportItems[nPage].J2 = allItems[x].ITM_NM;
        //                    reportItems[nPage].J3 = allItems[x].ITM_SZ_NM;
        //                    reportItems[nPage].J4 = allItems[x].UOM_NM;
        //                    reportItems[nPage].J5 = allItems[x].SL_RTN_ITM_QTY;
        //                    reportItems[nPage].J6 = allItems[x].MD_QTY;
        //                    reportItems[nPage].J7 = allItems[x].SL_RTN_NM;
        //                    reportItems[nPage].J8 = allItems[x].SL_BAD_ITM_NM;
        //                    reportItems[nPage].J9 = allItems[x].ITM_LOC_NM;
        //                }
        //                else if ((x % 12) == 10)
        //                {
        //                    reportItems[nPage].K1 = allItems[x].RN;
        //                    reportItems[nPage].K2 = allItems[x].ITM_NM;
        //                    reportItems[nPage].K3 = allItems[x].ITM_SZ_NM;
        //                    reportItems[nPage].K4 = allItems[x].UOM_NM;
        //                    reportItems[nPage].K5 = allItems[x].SL_RTN_ITM_QTY;
        //                    reportItems[nPage].K6 = allItems[x].MD_QTY;
        //                    reportItems[nPage].K7 = allItems[x].SL_RTN_NM;
        //                    reportItems[nPage].K8 = allItems[x].SL_BAD_ITM_NM;
        //                    reportItems[nPage].K9 = allItems[x].ITM_LOC_NM;
        //                }
        //                else if ((x % 12) == 11)
        //                {
        //                    reportItems[nPage].L1 = allItems[x].RN;
        //                    reportItems[nPage].L2 = allItems[x].ITM_NM;
        //                    reportItems[nPage].L3 = allItems[x].ITM_SZ_NM;
        //                    reportItems[nPage].L4 = allItems[x].UOM_NM;
        //                    reportItems[nPage].L5 = allItems[x].SL_RTN_ITM_QTY;
        //                    reportItems[nPage].L6 = allItems[x].MD_QTY;
        //                    reportItems[nPage].L7 = allItems[x].SL_RTN_NM;
        //                    reportItems[nPage].L8 = allItems[x].SL_BAD_ITM_NM;
        //                    reportItems[nPage].L9 = allItems[x].ITM_LOC_NM;
        //                }
        //            }
        //        }
        //    }

        //    S2217ReturnChitReport report = new S2217ReturnChitReport(reportItems);
        //    report.Margins.Top = 0;
        //    report.Margins.Bottom = 0;
        //    report.Margins.Left = 80;
        //    report.Margins.Right = 0;
        //    report.Landscape = false;
        //    report.PrintingSystem.ShowPrintStatusDialog = true;
        //    report.PaperKind = System.Drawing.Printing.PaperKind.A4;

        //    //if (PRN_FLG.Equals("Y"))
        //    //{
        //    //    //데모 시연 문서 표시 가능
        //    //    report.Watermark.Text = "재발행";
        //    //    report.Watermark.TextDirection = DirectionMode.ForwardDiagonal;
        //    //    report.Watermark.Font = new Font(report.Watermark.Font.FontFamily, 150);
        //    //    report.Watermark.ForeColor = Color.DodgerBlue;
        //    //    ////report.Watermark.ForeColor = Color.PaleTurquoise;
        //    //    report.Watermark.TextTransparency = 190;
        //    //    ////report.Watermark.ShowBehind = false;
        //    //    ////report.Watermark.PageRange = "1,3-5";
        //    //}

        //    XtraReportPreviewModel model = new XtraReportPreviewModel(report);
        //    DocumentPreviewWindow window = new DocumentPreviewWindow() { Model = model };
        //    report.CreateDocument(true);
        //    window.Title = "반품전표 [" + SelectedMstItem.SL_BIL_RTN_NO + "/" + SelectedMstItem.CO_NM + "]";
        //    window.Owner = Application.Current.MainWindow;
        //    window.ShowDialog();
        //}

        public async void SYSTEM_CODE_VO()
        {
            ////사업장
            //AreaList = SystemProperties.SYSTEM_CODE_VO("L-002");
            //_AreaMap = SystemProperties.SYSTEM_CODE_MAP("L-002");
            //if (AreaList.Count > 0)
            //{
            //    M_SL_AREA_NM = SystemProperties.USER_VO.EMPE_PLC_CD;
            //    this._areaCd = SystemProperties.USER_VO.EMPE_PLC_NM;
            //}
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
    }
}
