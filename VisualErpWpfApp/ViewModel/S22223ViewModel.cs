using AquilaErpWpfApp3.Util;
using AquilaErpWpfApp3.View.SAL.Dialog;
using AquilaErpWpfApp3.View.SAL.Report;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Printing;
using DevExpress.Xpf.WindowsUI;
using DevExpress.XtraPrinting.Drawing;
using ModelsLibrary.Code;
using ModelsLibrary.Sale;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Media;

namespace AquilaErpWpfApp3.ViewModel
{
    public sealed class S22223ViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private string title = "주문별 생산공장/GR확정";
        //private IList<CodeDao> UserList;

        //private static SaleOrderServiceClient saleOrderClient = SystemProperties.SaleOrderClient;
        private IList<SaleVo> selectedMstList = new List<SaleVo>();
        //private IList<SaleVo> selectedMstItemsList = new List<SaleVo>();

        //private ObservableCollection<PurVo> selectedDtlList = new ObservableCollection<PurVo>();
        private IList<SaleVo> selectedDtlList = new List<SaleVo>();
        private IList<SaleVo> selectedDtlItemsList = new List<SaleVo>();

        private S22223GrDialog GrDialog;
        private S22223Report1 Report1;
        private S22223Report2 Report2;

        //private S2217MasterDialog masterDialog;
        //private S2217DetailDialog detailDialog;

        //private S2217DetailEditDialog detailEditDialog;

        //private S2216MasterDialog masterUsrDialog;

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

        public S22223ViewModel()
        {
            //StartSlRlseDt = System.DateTime.Now;
            //EndSlRlseDt = System.DateTime.Now;

            StartInReqDt = System.DateTime.Now;
            EndInReqDt = System.DateTime.Now;

            InReqDt = System.DateTime.Now;

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
        public async void Refresh()
        {
            try
            {
                //DXSplashScreen.Show<ProgressWindow>();
                SelectedDtlItem = null;
                SelectDtlList = null;


                //, Y1_FM_DT = (StartSlRlseDt).ToString("yyyy-MM-dd"), Y1_TO_DT = (EndSlRlseDt).ToString("yyyy-MM-dd")
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s22223/mst", new StringContent(JsonConvert.SerializeObject(new SaleVo() {FM_DT = (StartInReqDt).ToString("yyyy-MM-dd"), TO_DT = (EndInReqDt).ToString("yyyy-MM-dd"), CHNL_CD = SystemProperties.USER_VO.CHNL_CD, UPD_USR_ID = SystemProperties.USER, AREA_CD = "001" }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectMstList = JsonConvert.DeserializeObject<IEnumerable<SaleVo>>(await response.Content.ReadAsStringAsync()).Cast<SaleVo>().ToList();


                        //SelectMstList = saleOrderClient.S2217SelectMstList(new SaleVo() { FM_DT = (StartDt).ToString("yyyy-MM-dd"), TO_DT = (EndDt).ToString("yyyy-MM-dd"), AREA_CD = _AreaMap[TXT_SL_AREA_NM], GBN = "Q" });
                        ////
                        Title = "[납품요청일]" + (StartInReqDt).ToString("yyyy-MM-dd") + "~" + (EndInReqDt).ToString("yyyy-MM-dd");
                        //Title = "[기간]" + (StartDt).ToString("yyyy-MM-dd") + "~" + (EndDt).ToString("yyyy-MM-dd") + ", " + (string.IsNullOrEmpty(M_SEARCH_TEXT) ? "" : (",   [검 색]" + M_SEARCH_TEXT));

                        if (SelectMstList.Count >= 1)
                        {
                            isM_UPDATE = true;
                            isM_DELETE = true;
                            //isM_SAVE = true;

                            isD_Refresh = true;
                            isD_UPDATE = false;
                            isD_DELETE = false;
                            //if (string.IsNullOrEmpty(_SL_BIL_RTN_NO))
                            //{
                            //SelectedMstItem = SelectMstList[0];
                            //}
                            //else
                            //{
                            //    SelectedMstItem = SelectMstList.Where(x => x.SL_BIL_RTN_NO.Equals(_SL_BIL_RTN_NO)).LastOrDefault<SaleVo>();
                            //}
                        }
                        else
                        {
                            SelectedDtlItem = null;
                            SelectDtlList = null;

                            isM_UPDATE = false;
                            isM_DELETE = false;
                            //isM_SAVE = false;

                            isD_Refresh = false;
                            isD_UPDATE = false;
                            isD_DELETE = false;
                        }
                    }
                }
                //if (SystemProperties.USER == "134")
                ////if (UserList.Any<CodeDao>(x => x.CLSS_CD.Equals(SystemProperties.USER)))
                ////{
                ////    isM_Cancel = true;
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
        //
        ////주문 등록일
        //DateTime _startSlRlseDt;
        //public DateTime StartSlRlseDt
        //{
        //    get { return _startSlRlseDt; }
        //    set { SetProperty(ref _startSlRlseDt, value, () => StartSlRlseDt); }
        //}

        //DateTime _endSlRlseDt;
        //public DateTime EndSlRlseDt
        //{
        //    get { return _endSlRlseDt; }
        //    set { SetProperty(ref _endSlRlseDt, value, () => EndSlRlseDt); }
        //}

        //
        //납품 요청일
        DateTime _startInReqDt;
        public DateTime StartInReqDt
        {
            get { return _startInReqDt; }
            set { SetProperty(ref _startInReqDt, value, () => StartInReqDt); }
        }

        DateTime _endInReqDt;
        public DateTime EndInReqDt
        {
            get { return _endInReqDt; }
            set { SetProperty(ref _endInReqDt, value, () => EndInReqDt); }
        }

        //확정납품요청일
        DateTime _inReqDt;
        public DateTime InReqDt
        {
            get { return _inReqDt; }
            set { SetProperty(ref _inReqDt, value, () => InReqDt); }
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
                    //SetProperty(ref _selectedMstItem, value, () => SelectedMstItem, SelectDetailRefresh);
                }
            }
        }


        //public IList<SaleVo> SelectedMstItems
        //{
        //    get
        //    {
        //        return selectedMstItemsList;
        //    }
        //    set
        //    {
        //        SetProperty(ref selectedMstItemsList, value, () => SelectedMstItems);
        //    }
        //}

        //private string[] selectedMstItemsList;
        //public string[] SelectedMstItems
        //{
        //    get
        //    {
        //        return selectedMstItemsList;
        //    }
        //    set
        //    {
        //        SetProperty(ref selectedMstItemsList, value, () => SelectedMstItems, SelectDetailRefresh);
        //    }
        //}


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

        private bool? _isD_Refresh = false;
        public bool? isD_Refresh
        {
            get { return _isD_Refresh; }
            set { SetProperty(ref _isD_Refresh, value, () => isD_Refresh); }
        }

        //private bool? _isM_Cancel = false;
        //public bool? isM_Cancel
        //{
        //    get { return _isM_Cancel; }
        //    set { SetProperty(ref _isM_Cancel, value, () => isM_Cancel); }
        //}

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


        //거래처
        //private IList<SystemCodeVo> _CoNmList = new List<SystemCodeVo>();
        //public IList<SystemCodeVo> CoNmList
        //{
        //    get { return _CoNmList; }
        //    set { SetProperty(ref _CoNmList, value, () => CoNmList); }
        //}
        //private CustomerCodeDao _M_DEPT_DESC;
        //public CustomerCodeDao M_DEPT_DESC
        //{
        //    get { return _M_DEPT_DESC; }
        //    set { SetProperty(ref _M_DEPT_DESC, value, () => M_DEPT_DESC); }
        //}

        //private SystemCodeVo _M_SL_CO_NM;
        //public SystemCodeVo M_SL_CO_NM
        //{
        //    get { return _M_SL_CO_NM; }
        //    set { SetProperty(ref _M_SL_CO_NM, value, () => M_SL_CO_NM); }
        //}

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
            //try
            //{
            //    //if (DXSplashScreen.IsActive == false)
            //    //{
            //    //    DXSplashScreen.Show<ProgressWindow>();
            //    //}


            //    //    if (SelectedMstItem == null)
            //    //    {
            //    //        return;
            //    //    }
            //    //    else if (SelectedMstItems.Count <= 0)
            //    //    {
            //    //        return;
            //    //    }


            //    //    isD_UPDATE = true;



            //    //    //SelectedMstItem.A_GBN = SelectedMstItems.Select(x => x.SL_ORD_NO + "-" + x.SL_ORD_SEQ).ToArray<string>();

            //    //    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("S22223/dtl", new StringContent(JsonConvert.SerializeObject(SelectedMstItem), System.Text.Encoding.UTF8, "application/json")))
            //    //    {
            //    //        if (response.IsSuccessStatusCode)
            //    //        {
            //    //            this.SelectDtlList = JsonConvert.DeserializeObject<IEnumerable<SaleVo>>(await response.Content.ReadAsStringAsync()).Cast<SaleVo>().ToList();
            //    //        }

            //    //        //SelectDtlList = new ObservableCollection<PurVo>(purClient.P4401SelectDtlList(new PurVo() { ITM_CD = this._selectedMstItem.}));
            //    //        //SelectDtlList = saleOrderClient.S2217SelectDtlList(SelectedMstItem);
            //    //        // //
            //    //        if (SelectDtlList.Count >= 1)
            //    //        {
            //    //            //isD_UPDATE = true;
            //    //            isD_DELETE = true;

            //    //            //SearchDetail = SelectDtlList[0];
            //    //        }
            //    //        else
            //    //        {
            //    //            //isD_UPDATE = false;
            //    //            isD_DELETE = false;
            //    //        }
            //    //    }


            //    //    if (DXSplashScreen.IsActive == true)
            //    //    {
            //    //        DXSplashScreen.Close();
            //    //    }
            // }
            //catch (System.Exception eLog)
            //{
            //    if (DXSplashScreen.IsActive == true)
            //    {
            //        DXSplashScreen.Close();
            //    }
            //    //
            //    WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
            //    return;
            //}
        }
        //#endregion


        [Command]
        public async void SelectDetailCheckd()
        {
            if (SelectedMstItem == null)
            {
                return;
            }

            //
            if (SelectedMstItem.isCheckd)
            {
                SelectedMstItem.isCheckd = false;
            }
            else
            {
                SelectedMstItem.isCheckd = true;
            }
            SelectDetailRefresh();
        }

        [Command]
        public async void SelectDetailRefresh(string _RLSE_CMD_NO = null)
        {
            try
            {
                //if (DXSplashScreen.IsActive == false)
                //{
                //    DXSplashScreen.Show<ProgressWindow>();
                //}


                if (this.SelectMstList.Any<SaleVo>(x=> x.isCheckd == true))
                {
                    //SelectedMstItem.A_GBN = SelectedMstItems.Select(x => x.SL_ORD_NO + "-" + x.SL_ORD_SEQ).ToArray<string>();
                    //string[] selList = this.ConfigViewPage1Edit.SelectedItems.OfType<InauditVo>().ToList<InauditVo>().Select(x => x.SL_RLSE_NO).ToArray<string>();

                    SelectedMstItem.A_SL_RLSE_NO = this.SelectMstList.Where<SaleVo>(w => w.isCheckd == true).Select(x => (x.SL_RLSE_NO + "_" + x.CNTR_NM + "_" + x.CNTR_PSN_NM)).ToArray<string>();

                    // using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("S22223/dtl", new StringContent(JsonConvert.SerializeObject(SelectedMstItems), System.Text.Encoding.UTF8, "application/json")))
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("S22223/dtl", new StringContent(JsonConvert.SerializeObject(SelectedMstItem), System.Text.Encoding.UTF8, "application/json")))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            this.SelectDtlList = JsonConvert.DeserializeObject<IEnumerable<SaleVo>>(await response.Content.ReadAsStringAsync()).Cast<SaleVo>().ToList();

                            // //
                            if (SelectDtlList.Count >= 1)
                            {
                                isD_UPDATE = true;
                                isD_DELETE = true;
                                if (!string.IsNullOrEmpty(_RLSE_CMD_NO))
                                {
                                    this.SelectedDtlItem = this.selectedDtlList.Where(x => x.RLSE_CMD_NO.Equals(_RLSE_CMD_NO)).FirstOrDefault<SaleVo>();
                                }
                            }
                            else
                            {
                                isD_UPDATE = false;
                                isD_DELETE = false;
                            }
                        }
                    }

                }
                else
                {
                    this.SelectDtlList = null;
                    this.SelectedDtlItem = null;
                    return;
                }
                //if (DXSplashScreen.IsActive == true)
                //{
                //    DXSplashScreen.Close();
                //}
            }
            catch (System.Exception eLog)
            {
                //if (DXSplashScreen.IsActive == true)
                //{
                //    DXSplashScreen.Close();
                //}
                //
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }


        // GR번호 부여 (번호 순서대로 매기기)
        [Command]
        public async void Apply()
        {
            try
            {

                //if (SelectedDtlItems.Count <= 0)
                //{
                //    return;
                //}

               // MessageBoxResult result = WinUIMessageBox.Show("[확정납품요청일 : " + InReqDt.ToString("yyyy-MM-dd") + "] 정말로 GR 확정 하시겠습니까?", title, MessageBoxButton.YesNo, MessageBoxImage.Question);
               
                MessageBoxResult result = WinUIMessageBox.Show("GR 번호를 부여하시겠습니까?", title, MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    if (DXSplashScreen.IsActive == false)
                    {
                        DXSplashScreen.Show<ProgressWindow>();
                    }


                    //foreach (SaleVo _item in SelectedDtlItems)
                    //foreach (SaleVo _item in SelectDtlList)
                    //{
                    //    _item.UPD_USR_ID = SystemProperties.USER;
                    //    _item.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
                    //    _item.AREA_CD = "001";
                    //    // _item.IN_REQ_DT = InReqDt.ToString("yyyy-MM-dd");
                    //}



                    //using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("S22223/mst/u", new StringContent(JsonConvert.SerializeObject(SelectedDtlItems), System.Text.Encoding.UTF8, "application/json")))
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("S22223/mst/u", new StringContent(JsonConvert.SerializeObject(SelectDtlList), System.Text.Encoding.UTF8, "application/json")))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            int _Num = 0;
                            string resultMsg = await response.Content.ReadAsStringAsync();
                            if (int.TryParse(resultMsg, out _Num) == false)
                            {
                                if (DXSplashScreen.IsActive == true)
                                {
                                    DXSplashScreen.Close();
                                }
                                //실패
                                WinUIMessageBox.Show(resultMsg, title, MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }

                            if (DXSplashScreen.IsActive == true)
                            {
                                DXSplashScreen.Close();
                            }

                            SelectDetailRefresh(SelectDtlList[0].RLSE_CMD_NO);

                            //성공
                            WinUIMessageBox.Show("GR 번호가 부여되었습니다.", title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                        }
                    }
                }
            }
            catch (System.Exception eLog)
            {
                if (DXSplashScreen.IsActive == true)
                {
                    DXSplashScreen.Close();
                }
                //
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }



        // GR번호 확정 (클릭한 데이터끼리 같은 GR번호로 묶임)
        [Command]
        public async void DtlGR()
        {
            try
            {
                //WinUIMessageBox.Show("GR확정");

                // 01.26 DTL을 클릭해야 활성화하는것을 못 찾음 => 일단 DTL 클릭안하고 버튼 눌렀을때는 return 하도록 해둠
                if (SelectedDtlItems.Count <= 0)
                {
                    return;
                }

                MessageBoxResult result = WinUIMessageBox.Show("GR 번호를 확정하시겠습니까?", title, MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    if (DXSplashScreen.IsActive == false)
                    {
                        DXSplashScreen.Show<ProgressWindow>();
                    }

                    // 첫번째 GR번호를 다른 데이터의 GR번호로 바꿀예정
                    SelectedDtlItem.RLSE_CMD_NO = SelectedDtlItems[0].RLSE_CMD_NO;

                    //SelectedDtlItem.A_SL_RLSE_NO = SelectedDtlItems.Select(x => x.SL_RLSE_NO).ToArray<string>();
                    //SelectedDtlItem.A_SL_RLSE_SEQ = SelectedDtlItems.Select(x => x.SL_RLSE_SEQ).ToArray<int?>();  
                    SelectedDtlItem.A_SL_RLSE_NO = SelectedDtlItems.Select(x => x.SL_RLSE_NO + "_" + x.SL_RLSE_SEQ.ToString()).ToArray<string>();

                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("S22223/mst/gr", new StringContent(JsonConvert.SerializeObject(SelectedDtlItem), System.Text.Encoding.UTF8, "application/json")))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            int _Num = 0;
                            string resultMsg = await response.Content.ReadAsStringAsync();
                            if (int.TryParse(resultMsg, out _Num) == false)
                            {
                                if (DXSplashScreen.IsActive == true)
                                {
                                    DXSplashScreen.Close();
                                }
                                //실패
                                WinUIMessageBox.Show(resultMsg, title, MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                            SelectDetailRefresh(SelectedDtlItem.RLSE_CMD_NO);

                            //성공
                            WinUIMessageBox.Show("GR 번호가 확정되었습니다.", title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                        }
                    }
                }
                if (DXSplashScreen.IsActive == true)
                {
                    DXSplashScreen.Close();
                }

            }
            catch (Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }



        // 납품확인서 출력 ( Dialog → Report1 → Report2 )
        [Command]
        public async void DtlRpt()
        {
            try
            {
                // DTL 선택된 데이터가 없거나 해당 GR번호가 없을 경우 
                if (this.SelectedDtlItem == null) return;
                //// 
                // GR번호 경우 확정일 때만 출력되도록 추후 수정 필요할 것으로 판단됨. *******************************************************************************************************
                if (this.SelectedDtlItem.RLSE_CMD_NO == null)
                {
                    WinUIMessageBox.Show("[GR번호] 가 필요합니다.", "유효검사", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.None, MessageBoxOptions.None);
                    return;
                }

                // GR 조회 Vo
                SaleVo rptGRDao = new SaleVo() { RLSE_CMD_NO = this.SelectedDtlItem.RLSE_CMD_NO, CHNL_CD = SystemProperties.USER_VO.CHNL_CD };

                // GR 이미 입력된 정보 존재 여부 (추가&수정)
                SaleVo dlgDao = rptGRDao;


                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("S22223/dlg", new StringContent(JsonConvert.SerializeObject(rptGRDao), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        // GR 이미 입력된 정보가 있을 경우 (수정)
                        if (JsonConvert.DeserializeObject<SaleVo>(await response.Content.ReadAsStringAsync()) != null)
                        {
                            dlgDao = JsonConvert.DeserializeObject<SaleVo>(await response.Content.ReadAsStringAsync());
                        }
                    }
                }

                // 납품확인서 필요한 데이터 입력 Dialog
                GrDialog = new S22223GrDialog(dlgDao);
                GrDialog.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                GrDialog.Owner = Application.Current.MainWindow;
                GrDialog.BorderEffect = BorderEffect.Default;
                GrDialog.BorderEffectActiveColor = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 128, 0));
                GrDialog.BorderEffectInactiveColor = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 170, 170));
                bool isDialog = (bool)GrDialog.ShowDialog();
                if (!isDialog)
                {
                    // False : 입력을 취소할 경우
                    return;
                }



                // 납품확인서 리포트1 조회 
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("S22223/rpt1", new StringContent(JsonConvert.SerializeObject(rptGRDao), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        // 납품확인서 리포트1 조회
                        IList<SaleVo> rpt1List = new List<SaleVo>();
                        rpt1List = JsonConvert.DeserializeObject<IEnumerable<SaleVo>>(await response.Content.ReadAsStringAsync()).Cast<SaleVo>().ToList();

                        if(rpt1List.Count > 0)
                        {
                            // 납품확인서 리포트 1번 출력...
                            Report1 = new S22223Report1(rpt1List);
                            Report1.Margins.Top = 2;
                            Report1.Margins.Bottom = 0;
                            Report1.Margins.Left = 40;
                            Report1.Margins.Right = 1;
                            Report1.Landscape = false;
                            Report1.PrintingSystem.ShowPrintStatusDialog = true;
                            Report1.PaperKind = System.Drawing.Printing.PaperKind.A4;
                            
                            //데모 시연 문서 표시 가능
                            Report1.Watermark.Text = "로보콘 주식회사";
                            Report1.Watermark.TextDirection = DirectionMode.ForwardDiagonal;
                            Report1.Watermark.Font = new Font(Report1.Watermark.Font.FontFamily, 40);
                            ////Report1.Watermark.ForeColor = Color.DodgerBlue;
                            Report1.Watermark.ForeColor = System.Drawing.Color.PaleTurquoise;
                            Report1.Watermark.TextTransparency = 180;
                            Report1.Watermark.ShowBehind = false;
                            //Report1.Watermark.PageRange = "1,3-5";

                            var window = new DocumentPreviewWindow();
                            window.PreviewControl.DocumentSource = Report1;
                            Report1.CreateDocument(true);
                            window.Title = "철근납품확인서";
                            window.Owner = Application.Current.MainWindow;
                            window.ShowDialog();
                        }
                        else
                        {
                            // 데이터가 없어 납품확인서 리포트 1번 출력 안함.
                            WinUIMessageBox.Show("정보가 없어 [납품확인서] 출력은 하지 않습니다.", "[철근납품확인서]", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                }


                // 납품리스트 리포트2 조회 
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("S22223/rpt2", new StringContent(JsonConvert.SerializeObject(rptGRDao), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        // 납품리스트 리포트2 조회
                        IList<SaleVo> rpt2List = new List<SaleVo>();
                        rpt2List = JsonConvert.DeserializeObject<IEnumerable<SaleVo>>(await response.Content.ReadAsStringAsync()).Cast<SaleVo>().ToList();

                        if (rpt2List.Count > 0)
                        {
                            // 납품리스트 리포트 2번 출력...  ****************************************************************** 상단에 조회한 Data
                            Report2 = new S22223Report2(rpt2List);
                            Report2.Margins.Top = 2;
                            Report2.Margins.Bottom = 0;
                            Report2.Margins.Left = 40;
                            Report2.Margins.Right = 1;
                            Report2.Landscape = false;
                            Report2.PrintingSystem.ShowPrintStatusDialog = true;
                            Report2.PaperKind = System.Drawing.Printing.PaperKind.A4;

                            //데모 시연 문서 표시 가능
                            Report2.Watermark.Text = "로보콘 주식회사";
                            Report2.Watermark.TextDirection = DirectionMode.ForwardDiagonal;
                            Report2.Watermark.Font = new Font(Report2.Watermark.Font.FontFamily, 40);
                            ////Report2.Watermark.ForeColor = Color.DodgerBlue;
                            Report2.Watermark.ForeColor = System.Drawing.Color.PaleTurquoise;
                            Report2.Watermark.TextTransparency = 180;
                            Report2.Watermark.ShowBehind = false;
                            //Report2.Watermark.PageRange = "1,3-5";

                            var window = new DocumentPreviewWindow();
                            window.PreviewControl.DocumentSource = Report2;
                            Report2.CreateDocument(true);
                            window.Title = "납품리스트";
                            window.Owner = Application.Current.MainWindow;
                            window.ShowDialog();
                        }
                        else
                        {
                            // 데이터가 없어 납품확인서 리포트 2번 출력 안함.
                            WinUIMessageBox.Show("정보가 없어 [납품리스트] 출력은 하지 않습니다.", "[납품리스트]", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                }
            }
            catch (Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }




        //#region Functon <Detail List>
        public IList<SaleVo> SelectDtlList
        {
            get { return selectedDtlList; }
            set { SetProperty(ref selectedDtlList, value, () => SelectDtlList); }
        }


        SaleVo _selectedDtlItem;
        public SaleVo SelectedDtlItem
        {
            get
            {
                return _selectedDtlItem;
            }
            set
            {
                SetProperty(ref _selectedDtlItem, value, () => SelectedDtlItem);
            }
        }


        public IList<SaleVo> SelectedDtlItems
        {
            get { return selectedDtlItemsList; }
            set { SetProperty(ref selectedDtlItemsList, value, () => SelectedDtlItems); }
        }

        //#endregion


        string _Title = string.Empty;
        public string Title
        {
            get { return _Title; }
            set { SetProperty(ref _Title, value, () => Title); }
        }



        //#region Functon Command <add, Edit, Del>
        //[Command]
        // public void NewContact()
        // {
        //     try
        //     {
        //         //if (SelectedMstItem == null)
        //         //{
        //         //    return;
        //         //}
        //         masterDialog = new S2217MasterDialog(new SaleVo() { AREA_NM = SystemProperties.USER_VO.EMPE_PLC_CD, SL_CO_NM = "재고수정용", SL_CO_CD = "10000" });
        //         //masterDialog = new S2217MasterDialog(new JobVo() { AREA_NM = SystemProperties.USER_VO.EMPE_PLC_CD, SL_CO_NM = SelectedMstItem.SL_CO_NM, SL_CO_CD = SelectedMstItem.SL_CO_CD });
        //         masterDialog.Title = "반품 리스트 - 추가";
        //         masterDialog.Owner = Application.Current.MainWindow;
        //         masterDialog.BorderEffect = BorderEffect.Default;
        //         masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
        //         masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
        //         bool isDialog = (bool)masterDialog.ShowDialog();
        //         if (isDialog)
        //         {
        //             Refresh(masterDialog.SL_BIL_RTN_NO);
        //         }
        //     }
        //     catch (System.Exception eLog)
        //     {
        //         //DXSplashScreen.Close();
        //         WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
        //         return;
        //     }
        // }

        // [Command]
        // public void EditContact()
        // {
        //     if (SelectedMstItem == null)
        //     {
        //         return;
        //     }
        //     //else if (SelectMstList.Count <= 0)
        //     //{
        //     //    return;
        //     //}

        //     masterDialog = new S2217MasterDialog(SelectedMstItem);
        //     masterDialog.Title = "반품 리스트 - 수정 [반품 번호 : " + SelectedMstItem.SL_BIL_RTN_NO + "]";
        //     masterDialog.Owner = Application.Current.MainWindow;
        //     masterDialog.BorderEffect = BorderEffect.Default;
        //     masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
        //     masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
        //     bool isDialog = (bool)masterDialog.ShowDialog();
        //     if (isDialog)
        //     {
        //         Refresh(masterDialog.SL_BIL_RTN_NO);
        //     }
        // }

        // [Command]
        // public async void DelContact()
        // {
        //     if (SelectedMstItem == null)
        //     {
        //         return;
        //     }
        //     //     //JobVo delDao = SelectedMstItem;
        //     //     //if (delDao != null)
        //     //     //{
        //     MessageBoxResult result = WinUIMessageBox.Show("[반품 번호" + SelectedMstItem.SL_BIL_RTN_NO + "] 정말로 삭제 하시겠습니까?", "[삭제]" + title, MessageBoxButton.YesNo, MessageBoxImage.Question);
        //     if (result == MessageBoxResult.Yes)
        //     {
        //         SelectedMstItem.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
        //         using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s2217/mst/d", new StringContent(JsonConvert.SerializeObject(SelectedMstItem), System.Text.Encoding.UTF8, "application/json")))
        //         {
        //             if (response.IsSuccessStatusCode)
        //             {
        //                 int _Num = 0;
        //                 string resultMsg = await response.Content.ReadAsStringAsync();
        //                 if (int.TryParse(resultMsg, out _Num) == false)
        //                 {
        //                     //실패
        //                     WinUIMessageBox.Show(resultMsg, title, MessageBoxButton.OK, MessageBoxImage.Error);
        //                     return;
        //                 }
        //                 Refresh();

        //                 //성공
        //                 WinUIMessageBox.Show("삭제가 완료되었습니다.", title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
        //             }
        //         }

        //         //     //        try
        //         //     //        {
        //         //     //            DXSplashScreen.Show<ProgressWindow>();
        //         //     //            JobVo resultVo = saleOrderClient.S2217DeleteDtl(new JobVo() { SL_BIL_RTN_NO = delDao.SL_BIL_RTN_NO });
        //         //     //            if (!resultVo.isSuccess)
        //         //     //            {
        //         //     //                //실패
        //         //     //                WinUIMessageBox.Show(resultVo.Message, "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
        //         //     //                return;
        //         //     //            }
        //         //     //            SelectMstDetail();

        //         //     //            resultVo = saleOrderClient.S2217DeleteMst(new JobVo() { SL_BIL_RTN_NO = delDao.SL_BIL_RTN_NO, AREA_CD = delDao.AREA_CD });
        //         //     //            if (!resultVo.isSuccess)
        //         //     //            {
        //         //     //                //실패
        //         //     //                WinUIMessageBox.Show(resultVo.Message, "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
        //         //     //                return;
        //         //     //            }
        //         //     //            Refresh();

        //         //     //            DXSplashScreen.Close();
        //         //     //            WinUIMessageBox.Show("삭제가 완료되었습니다.", "[삭제]" + title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
        //         //     //        }
        //         //     //        catch (System.Exception eLog)
        //         //     //        {
        //         //     //            DXSplashScreen.Close();
        //         //     //            WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
        //         //     //            return;
        //         //     //        }
        //         //     //    }
        //     }
        // }


        // [Command]
        // public void NewDtlContact()
        // {
        //     if (SelectedMstItem == null)
        //     {
        //         return;
        //     }


        //     SelectedMstItem.FM_DT = (StartDt).ToString("yyyy-MM-dd");
        //     SelectedMstItem.TO_DT = (EndDt).ToString("yyyy-MM-dd");

        //     detailDialog = new S2217DetailDialog(SelectedMstItem);
        //     detailDialog.Title = "반품 물품 관리 - [반품 번호 : " + SelectedMstItem.SL_BIL_RTN_NO + "]";
        //     detailDialog.Owner = Application.Current.MainWindow;
        //     detailDialog.BorderEffect = BorderEffect.Default;
        //     detailDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
        //     detailDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
        //     bool isDialog = (bool)detailDialog.ShowDialog();
        //     if (isDialog)
        //     {
        //         SelectMstDetail();
        //     }
        // }

        // [Command]
        // public async void EditDtlContact()
        // {
        //     if (SelectedMstItem == null)
        //     {
        //         return;
        //     }

        //     if (SelectedDtlItem == null)
        //     {
        //         return;
        //     }

        //     try
        //     {
        //         MessageBoxResult result = WinUIMessageBox.Show("[반품 번호" + SelectedMstItem.SL_BIL_RTN_NO + "/" + SelectedDtlItem.SL_BIL_RTN_SEQ + "] 정말로 삭제 하시겠습니까? ", "[삭제]" + this.title, MessageBoxButton.YesNo, MessageBoxImage.Question);
        //         if (result == MessageBoxResult.Yes)
        //         {
        //             SelectedMstItem.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
        //             using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s2217/dtl/d", new StringContent(JsonConvert.SerializeObject(SelectedMstItem), System.Text.Encoding.UTF8, "application/json")))
        //             {
        //                 if (response.IsSuccessStatusCode)
        //                 {
        //                     int _Num = 0;
        //                     string resultMsg = await response.Content.ReadAsStringAsync();
        //                     if (int.TryParse(resultMsg, out _Num) == false)
        //                     {
        //                         //실패
        //                         WinUIMessageBox.Show(resultMsg, title, MessageBoxButton.OK, MessageBoxImage.Error);
        //                         return;
        //                     }
        //                     Refresh();

        //                     //성공
        //                     WinUIMessageBox.Show("삭제가 완료되었습니다.", title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
        //                 }
        //             }
        //         }
        //     }
        //     catch (System.Exception eLog)
        //     {
        //         WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
        //         return;
        //     }
        // }




        //[Command]
        //public void CancelContact()
        //{
        //    //if (SelectedMstItem == null)
        //    //{
        //    //    return;
        //    //}


        //    //try
        //    //{
        //    //    MessageBoxResult result = WinUIMessageBox.Show("[요청 번호 명 : " + SelectedMstItem.SL_BIL_RTN_NO + "] 정말로 승인해제 하시겠습니까? ", "[승인해제]" + this.title, MessageBoxButton.YesNo, MessageBoxImage.Question);
        //    //    if (result == MessageBoxResult.Yes)
        //    //    {
        //    //        JobVo resultVo = saleOrderClient.ProcS2216Delete(new JobVo() { SL_BIL_RTN_NO = SelectedMstItem.SL_BIL_RTN_NO, CRE_USR_ID = SystemProperties.USER });
        //    //        if (!resultVo.isSuccess)
        //    //        {
        //    //            //실패
        //    //            WinUIMessageBox.Show(resultVo.Message, "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
        //    //            return;
        //    //        }
        //    //        WinUIMessageBox.Show("[요청 번호 명 : " + SelectedMstItem.SL_BIL_RTN_NO + "] 승인해제 하였습니다.", "[승인해제]" + title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
        //    //        Refresh();

        //    //    }
        //    //    else if (result == MessageBoxResult.No)
        //    //    {
        //    //        return;
        //    //    }
        //    //}
        //    //catch (System.Exception eLog)
        //    //{
        //    //    DXSplashScreen.Close();
        //    //    WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
        //    //    return;
        //    //}
        //}

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
        //    JobVo delDao = SearchDetail;
        //    if (delDao != null)
        //    {
        //        MessageBoxResult result = WinUIMessageBox.Show("[" + delDao.JB_NO + "/" + delDao.JB_SEQ + "]" + " 정말로 삭제 하시겠습니까?", "[삭제]" + title, MessageBoxButton.YesNo, MessageBoxImage.Question);
        //        if (result == MessageBoxResult.Yes)
        //        {
        //            try
        //            {
        //                DXSplashScreen.Show<ProgressWindow>();
        //                saleOrderClient.S2217DeleteDtl(delDao);
        //                SelectMstDetail();
        //                //purClient.P4411DeleteMst(delDao);
        //                //Refresh();
        //                DXSplashScreen.Close();

        //                WinUIMessageBox.Show("삭제가 완료되었습니다.", "[삭제]" + title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
        //            }
        //            catch (System.Exception eLog)
        //            {
        //                DXSplashScreen.Close();
        //                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
        //                return;
        //            }
        //        }
        //    }
        //}

        [Command]
        public void ApplyContact()
        {
            //if (SelectedMstItem == null)
            //{
            //    return;
            //}
            //else if (SelectDtlList.Count == 0)
            //{
            //    WinUIMessageBox.Show("반품 승인 할 품목이 없습니다", "[입고 승인]" + title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
            //    return;
            //}


            //MessageBoxResult result = WinUIMessageBox.Show("[" + SelectedMstItem.SL_BIL_RTN_NO + "/" + SelectedMstItem.SL_CO_NM + "]" + " 정말로 입고 승인 하시겠습니까?", "[입고 승인]" + title, MessageBoxButton.YesNo, MessageBoxImage.Question);
            //if (result == MessageBoxResult.Yes)
            //{
            //    try
            //    {
            //        DXSplashScreen.Show<ProgressWindow>();

            //        SelectedMstItem.CRE_USR_ID = SystemProperties.USER;
            //        SelectedMstItem.UPD_USR_ID = SystemProperties.USER;
            //        JobVo resultVo = saleOrderClient.ProcS2217IoApply(SelectedMstItem);
            //        if (!resultVo.isSuccess)
            //        {
            //            //실패
            //            WinUIMessageBox.Show(resultVo.Message, "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
            //            return;
            //        }
            //        DXSplashScreen.Close();

            //        WinUIMessageBox.Show("입고 승인 완료되었습니다.", "[입고 승인]" + title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
            //    }
            //    catch (System.Exception eLog)
            //    {
            //        DXSplashScreen.Close();
            //        WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
            //        return;
            //    }
            //}
        }


        
        [Command]
        public void EditUsrContact()
        {
            //if (SelectedMstItem == null)
            //{
            //    return;
            //}
            ////else if (SelectMstList.Count <= 0)
            ////{
            ////    return;
            ////}

            //masterUsrDialog = new S2216MasterDialog(SelectedMstItem);
            //masterUsrDialog.Title = "반품 입고 담당자 등록 - [반품 번호 : " + SelectedMstItem.SL_BIL_RTN_NO + "]";
            //masterUsrDialog.Owner = Application.Current.MainWindow;
            //masterUsrDialog.BorderEffect = BorderEffect.Default;
            //////masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            //////masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            //bool isDialog = (bool)masterUsrDialog.ShowDialog();
            //if (isDialog)
            //{




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




        //[Command]
        //public void ReportContact()
        //{
        //    if (SelectedMstItem == null) { return; }

        //    ////return chit
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

        //                //
        //                //reportItems[0].O1 = reportItems[0].A6;
        //                //reportItems[0].O2 = reportItems[0].A7;
        //                //reportItems[0].O3 = float.Parse(reportItems[0].O1.ToString()) + float.Parse(reportItems[0].O2.ToString());
        //                //
        //                reportItems[0].SL_BIL_RTN_DT = allItems[x].SL_BIL_RTN_DT;
        //                reportItems[0].CO_NM = allItems[x].CO_NM;
        //                reportItems[0].SL_BIL_DT = allItems[x].SL_BIL_DT;
        //                //reportItems[0].SL_RTN_NM = allItems[x].SL_RTN_NM;

        //                reportItems[0].SL_BIL_NO = allItems[x].SL_BIL_NO;
        //                reportItems[0].RTN_AFT_DESC = SelectedMstItem.RTN_AFT_DESC;
        //                reportItems[0].RTN_AFT_A_DESC = SelectedMstItem.RTN_AFT_A_DESC;
        //                reportItems[0].RTN_AFT_B_DESC = SelectedMstItem.RTN_AFT_B_DESC;
        //                reportItems[0].RTN_AFT_C_DESC = SelectedMstItem.RTN_AFT_C_DESC;
        //                reportItems[0].SL_ITM_RMK = allItems[x].SL_ITM_RMK;
        //                reportItems[0].PRN_DT = "출력일시 : " + DateTime.Now.ToString("yyyy-MM-dd HH:mm");
        //                //reportItems[0].PRN_DT = "출력일시 : " + allItems[x].PRN_DT;
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
        //        //allItems[0].PAGE_NUM = "1/1";
        //    }
        //    else
        //    {
        //        //Page 나누기
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
        //                //allItems[z].PAGE_NUM =  (z + 1) + "/" + nPage;
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
        //                    //
        //                    //reportItems[z].O1 = reportItems[z].A6;
        //                    //reportItems[z].O2 = reportItems[z].A7;
        //                    //reportItems[z].O3 = float.Parse(reportItems[z].O1.ToString()) + float.Parse(reportItems[z].O2.ToString());
        //                    //
        //                    reportItems[z].SL_BIL_RTN_DT = allItems[x].SL_BIL_RTN_DT;
        //                    reportItems[z].CO_NM = allItems[x].CO_NM;
        //                    reportItems[z].SL_BIL_DT = allItems[x].SL_BIL_DT;
        //                    //reportItems[z].SL_RTN_NM = allItems[x].SL_RTN_NM;

        //                    reportItems[z].SL_BIL_NO = allItems[x].SL_BIL_NO;
        //                    reportItems[z].RTN_AFT_DESC = SelectedMstItem.RTN_AFT_DESC;
        //                    reportItems[z].RTN_AFT_A_DESC = SelectedMstItem.RTN_AFT_A_DESC;
        //                    reportItems[z].RTN_AFT_B_DESC = SelectedMstItem.RTN_AFT_B_DESC;
        //                    reportItems[z].RTN_AFT_C_DESC = SelectedMstItem.RTN_AFT_C_DESC;
        //                    reportItems[z].SL_ITM_RMK = allItems[x].SL_ITM_RMK;
        //                    reportItems[z].PRN_DT = "출력일시 : " + DateTime.Now.ToString("yyyy-MM-dd HH:mm");
        //                    //reportItems[z].PRN_DT = "출력일시 : " + allItems[x].PRN_DT;
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

        //        //나머지 계산
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

        //                    //
        //                    reportItems[nPage].SL_BIL_RTN_DT = allItems[x].SL_BIL_RTN_DT;
        //                    reportItems[nPage].CO_NM = allItems[x].CO_NM;
        //                    reportItems[nPage].SL_BIL_DT = allItems[x].SL_BIL_DT;
        //                    //reportItems[nPage].SL_RTN_NM = allItems[x].SL_RTN_NM;

        //                    reportItems[nPage].SL_BIL_NO = allItems[x].SL_BIL_NO;
        //                    reportItems[nPage].RTN_AFT_DESC = SelectedMstItem.RTN_AFT_DESC;
        //                    reportItems[nPage].RTN_AFT_A_DESC = SelectedMstItem.RTN_AFT_A_DESC;
        //                    reportItems[nPage].RTN_AFT_B_DESC = SelectedMstItem.RTN_AFT_B_DESC;
        //                    reportItems[nPage].RTN_AFT_C_DESC = SelectedMstItem.RTN_AFT_C_DESC;
        //                    reportItems[nPage].SL_ITM_RMK = allItems[x].SL_ITM_RMK;
        //                    reportItems[nPage].PRN_DT = "출력일시 : " + DateTime.Now.ToString("yyyy-MM-dd HH:mm");
        //                    //reportItems[nPage].PRN_DT = "출력일시 : " + allItems[x].PRN_DT;
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

        //    S2217Report report = new S2217Report(reportItems);
        //    report.Margins.Top = 0;
        //    report.Margins.Bottom = 0;
        //    report.Margins.Left = 80;
        //    report.Margins.Right = 0;
        //    report.Landscape = false;
        //    report.PrintingSystem.ShowPrintStatusDialog = true;
        //    report.PaperKind = System.Drawing.Printing.PaperKind.A4;

        //    var window = new DocumentPreviewWindow();
        //    window.PreviewControl.DocumentSource = report;
        //    report.CreateDocument(true);
        //    window.Title = "반품전표 [" + SelectedMstItem.SL_BIL_RTN_NO + "/" + SelectedMstItem.SL_CO_NM + "]";
        //    window.Owner = Application.Current.MainWindow;
        //    window.ShowDialog();



        //    ////if (PRN_FLG.Equals("Y"))
        //    ////{
        //    ////    //데모 시연 문서 표시 가능
        //    ////    report.Watermark.Text = "재발행";
        //    ////    report.Watermark.TextDirection = DirectionMode.ForwardDiagonal;
        //    ////    report.Watermark.Font = new Font(report.Watermark.Font.FontFamily, 150);
        //    ////    report.Watermark.ForeColor = Color.DodgerBlue;
        //    ////    ////report.Watermark.ForeColor = Color.PaleTurquoise;
        //    ////    report.Watermark.TextTransparency = 190;
        //    ////    ////report.Watermark.ShowBehind = false;
        //    ////    ////report.Watermark.PageRange = "1,3-5";
        //    ////}

        //    //XtraReportPreviewModel model = new XtraReportPreviewModel(report);
        //    //DocumentPreviewWindow window = new DocumentPreviewWindow() { Model = model };
        //    //report.CreateDocument(true);
        //    //window.Title = "반품전표 [" + SelectedMstItem.SL_BIL_RTN_NO + "/" + SelectedMstItem.CO_NM + "]";
        //    //window.Owner = Application.Current.MainWindow;
        //    //window.ShowDialog();
        //}

        
        
        public async void SYSTEM_CODE_VO()
        {
            //사업장
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


            //using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s143", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { DELT_FLG = "N", /*CO_TP_CD = "AR", */ AREA_CD = SystemProperties.USER_VO.EMPE_PLC_NM, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
            //{
            //    if (response.IsSuccessStatusCode)
            //    {
            //        CoNmList = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();

            //        CoNmList.Insert(0, new SystemCodeVo() { CO_NM = "전체", CO_NO = "" });
            //        this.M_SL_CO_NM = CoNmList[0];
            //    }
            //}
            Refresh();
        }
    }
}
