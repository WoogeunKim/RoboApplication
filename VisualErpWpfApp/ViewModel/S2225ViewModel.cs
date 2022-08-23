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
    public sealed class S2225ViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private string title = "세금계산서발행 - 일괄(Home Tax)";

        //private static SaleOrderServiceClient saleOrderClient = SystemProperties.SaleOrderClient;
        private IList<SaleVo> selectedMstList = new List<SaleVo>();

        //private ObservableCollection<PurVo> selectedDtlList = new ObservableCollection<PurVo>();
        //private IList<JobVo> selectedDtlList = new List<JobVo>();

        //private S2221MasterDialog masterDialog;
        private S22313ExcelDialog excelDialog;

        ////Menu Dialog
        //private ICommand _searchDialogCommand;
        //private ICommand _newDialogCommand;
        //private ICommand _editDetailDialogCommand;
        //private ICommand _delDialogCommand;
        //private ICommand _billCommand;
        //private ICommand _billHistoryCommand;
        //private string _areaCd = string.Empty;

        private S2225BillDialog s2225BillDialog;
        private S2225BillHistoryDialog s2225BillHistoryDialog;


        public S2225ViewModel()
        {
            StartDt = System.DateTime.Now;
            //EndDt = System.DateTime.Now;

            SYSTEM_CODE_VO();
            // - Refresh
        }

       [Command]
       public async void Refresh()
        {
            try
            {
                SaleVo _param = new SaleVo();
                _param.SL_DC_YRMON = (StartDt).ToString("yyyyMM");
                //_param.TO_DT = (EndDt).ToString("yyyyMM");
                //사업장
                _param.AREA_CD = M_SL_AREA_NM.CLSS_CD;
                _param.AREA_NM = M_SL_AREA_NM.CLSS_DESC;
                _param.BIL_CD = "A";
                _param.CLZ_FLG = (this.M_RLSE_PRT_FLG.Equals("전체") ? null : (this.M_RLSE_PRT_FLG.Equals("발행") ? "Y" : "N"));
                _param.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
                //
                ////
                //SelectMstList = saleOrderClient.S2225SelectMstList(_param);

                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s22313/mst", new StringContent(JsonConvert.SerializeObject(_param), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectMstList = JsonConvert.DeserializeObject<IEnumerable<SaleVo>>(await response.Content.ReadAsStringAsync()).Cast<SaleVo>().ToList();
                    }
                    setTitle();


                    if (SelectMstList.Count >= 1)
                    {
                        isM_UPDATE = true;
                        isM_DELETE = true;

                        SelectedMstItem = SelectMstList[0];
                    }
                    else
                    {
                        //SelectDtlList = null;
                        //SearchDetail = null;

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


        [Command]
        public async void Bill()
        {

            //try
            //{

            //    S2225BillDialogViewModel s2225BillDialogViewModel = new S2225BillDialogViewModel();
            //    s2225BillDialogViewModel.SlDcYrMon = (this.StartDt).ToString("yyyyMM");
            //    s2225BillDialogViewModel.AreaCd = this._areaCd;
            //    s2225BillDialogViewModel.Refresh();


            s2225BillDialog = new S2225BillDialog(new SaleVo() { SL_DC_YRMON = (this.StartDt).ToString("yyyyMM"), AREA_CD = M_SL_AREA_NM.CLSS_CD, CHNL_CD = SystemProperties.USER_VO.CHNL_CD, CRE_USR_ID = SystemProperties.USER });
            s2225BillDialog.Title = this.title;
            s2225BillDialog.Owner = Application.Current.MainWindow;
            s2225BillDialog.BorderEffect = BorderEffect.Default;
            bool isDialog = (bool)s2225BillDialog.ShowDialog();
            if (isDialog)
            {

            }

                //    }

                //    Refresh();

                //}
                //catch (System.Exception eLog)
                //{
                //    //DXSplashScreen.Close();
                //    WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                //    return;
                //}



                //MessageBoxResult result = WinUIMessageBox.Show("전자계산서 집계를 진행하시겠습니까?", "[계산서생성]", MessageBoxButton.YesNo, MessageBoxImage.Question);
                //if (result == MessageBoxResult.Yes)
                //{
                //    JobVo Vo = saleOrderClient.ProcS2225(new JobVo() { AREA_CD = _AreaMap[M_SL_AREA_NM], SL_DC_YRMON = (StartDt).ToString("yyyyMM"), CRE_USR_ID = SystemProperties.USER });
                //    WinUIMessageBox.Show("전자계산서 집계완료했습니다", "[계산서생성]", MessageBoxButton.OK, MessageBoxImage.Information);
                //}
                //else
                //    return;
            }

        [Command]
        public async void BillHistory()
        {
            try
            {
                if (SelectedMstItem == null)
                {
                    WinUIMessageBox.Show("선택한 거래처가 없습니다.", "[경고]" + this.title, MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.None, MessageBoxOptions.None);
                    return;
                }

                s2225BillHistoryDialog = new S2225BillHistoryDialog(new SaleVo() { CO_CD = SelectedMstItem.CO_CD, CO_NO = SelectedMstItem.CO_CD, SL_DC_YRMON = (this.StartDt).ToString("yyyyMM"), AREA_CD = M_SL_AREA_NM.CLSS_CD, CHNL_CD = SystemProperties.USER_VO.CHNL_CD });
                s2225BillHistoryDialog.Title = this.title + " - 거래내역확인";
                s2225BillHistoryDialog.Owner = Application.Current.MainWindow;
                s2225BillHistoryDialog.BorderEffect = BorderEffect.Default;
                bool isDialog = (bool)s2225BillHistoryDialog.ShowDialog();
                if (isDialog)
                {

                }
            }
            catch (System.Exception eLog)
            {
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

        //DateTime _endDt;
        //public DateTime EndDt
        //{
        //    get { return _endDt; }
        //    set { SetProperty(ref _endDt, value, () => EndDt); }
        //}

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

        //private string _M_DC_FLG = "사용";
        //public string M_DC_FLG
        //{
        //    get { return _M_DC_FLG; }
        //    set { SetProperty(ref _M_DC_FLG, value, () => M_DC_FLG); }
        //}

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
        private SystemCodeVo _M_SL_AREA_NMT;
        public SystemCodeVo M_SL_AREA_NM
        {
            get { return _M_SL_AREA_NMT; }
            set { SetProperty(ref _M_SL_AREA_NMT, value, () => M_SL_AREA_NM); }
        }

        //출력유무
        private IList<SystemCodeVo> _RlsePrtFlgList = new List<SystemCodeVo>();
        public IList<SystemCodeVo> RlsePrtFlgList
        {
            get { return _RlsePrtFlgList; }
            set { SetProperty(ref _RlsePrtFlgList, value, () => RlsePrtFlgList); }
        }

        private SystemCodeVo _M_RLSE_PRT_FLG ;
        public SystemCodeVo M_RLSE_PRT_FLG
        {
            get { return _M_RLSE_PRT_FLG; }
            set { SetProperty(ref _M_RLSE_PRT_FLG, value, () => M_RLSE_PRT_FLG); }
        }

        private void SelectMstDetail()
        {

        }
        //#endregion


        //#region Functon <Detail List>
        //public IList<JobVo> SelectDtlList
        //{
        //    get { return selectedDtlList; }
        //    set { SetProperty(ref selectedDtlList, value, () => SelectDtlList); }
        //}

        //JobVo _searchDetail;
        //public JobVo SearchDetail
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
        //#endregion


        string _Title = string.Empty;
        public string Title
        {
            get { return _Title; }
            set { SetProperty(ref _Title, value, () => Title); }
        }


        string _isDetailView = string.Empty;
        public string DetailView
        {
            get { return _isDetailView; }
            set { SetProperty(ref _isDetailView, value, () => DetailView); }
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

        //public ICommand BillCommand
        //{
        //    get
        //    {
        //        if (_billCommand == null)
        //            _billCommand = new DelegateCommand(Bill);
        //        return _billCommand;
        //    }
        //}

        //public ICommand BillHistoryCommand
        //{
        //    get
        //    {
        //        if (_billHistoryCommand == null)
        //            _billHistoryCommand = new DelegateCommand(BillHistory);
        //        return _billHistoryCommand;
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

        #region DelDialog()
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
                if (SelectedMstItem != null)
                {
                    MessageBoxResult result = WinUIMessageBox.Show(" 정말로 삭제 하시겠습니까?", "[삭제]", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        try
                        {
                            SelectedMstItem.SL_CLSS_CD = (SelectedMstItem.GBN.Equals("매출") ? "A" : (SelectedMstItem.GBN.Equals("수금할인") ? "B" : null));
                            SelectedMstItem.BIL_CD = "A";
                            SelectedMstItem.AREA_CD = SystemProperties.USER_VO.EMPE_PLC_NM;

                            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s22313/mst/d", new StringContent(JsonConvert.SerializeObject(SelectedMstItem), System.Text.Encoding.UTF8, "application/json")))
                            {
                                if (response.IsSuccessStatusCode)
                                {
                                    int _Num = 0;
                                    string resultMsg = await response.Content.ReadAsStringAsync();
                                    if (int.TryParse(resultMsg, out _Num) == false)
                                    {
                                        //실패
                                        WinUIMessageBox.Show(resultMsg, title, MessageBoxButton.OK, MessageBoxImage.Error);
                                        return;
                                    }
                                    Refresh();

                                    //성공
                                    WinUIMessageBox.Show("삭제가 완료되었습니다.", title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                                }
                            }
                        }
                        catch (System.Exception eLog)
                        {
                            WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                            return;
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
        #endregion

        //public void NewContact()
        //{

        //    MessageBoxResult result = WinUIMessageBox.Show("[" + (StartDt).ToString("yyyy-MM") + "/" + M_SL_AREA_NM + "] 정말로 일괄발행 하시겠습니까?", "[일괄발행]" + title, MessageBoxButton.YesNo, MessageBoxImage.Question);
        //    if (result == MessageBoxResult.Yes)
        //    {

        //        Excel.Application excelApp = null;
        //        Excel.Workbook wb = null;
        //        Excel.Worksheet ws = null;


        //        try
        //        {
        //            JobVo _param = new JobVo();
        //            _param.SL_DC_YRMON = (StartDt).ToString("yyyyMM");
        //            //사업장
        //            _param.AREA_CD = _AreaMap[M_SL_AREA_NM];
        //            _param.AREA_NM = M_SL_AREA_NM;

        //            string Path = string.Empty;
        //            System.Windows.Forms.SaveFileDialog dialog = new System.Windows.Forms.SaveFileDialog();
        //            dialog.InitialDirectory = "c:/desktop";
        //            dialog.Title = "Select Where To Save File";
        //            dialog.Filter = "Excel Files (*.xls;*.xlsx)|*.xls;*.xlsx|All files (*.*)|*.*";
        //            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        //            {
        //                Path = dialog.FileName;

        //                //
        //                IList<S2225Vo> excelList = saleOrderClient.S2225SelectExportExcelList(_param);

        //                //Save
        //                int r = 8;
        //                S2225Vo tmpVo;

        //                if (excelList.Count < 100)
        //                {

        //                    excelApp = new Excel.Application();
        //                    wb = excelApp.Workbooks.Add();
        //                    ws = wb.Worksheets.get_Item(1) as Excel.Worksheet;


        //                    for (int x = 0; x < excelList.Count; x++)
        //                    {
        //                        tmpVo = excelList[x];

        //                        ws.Cells[r, 1] = tmpVo.XLS_A;
        //                        ws.Cells[r, 2] = tmpVo.XLS_B;
        //                        ws.Cells[r, 3] = tmpVo.XLS_C;
        //                        ws.Cells[r, 4] = tmpVo.XLS_D;
        //                        ws.Cells[r, 5] = tmpVo.XLS_E;
        //                        ws.Cells[r, 6] = tmpVo.XLS_F;
        //                        ws.Cells[r, 7] = tmpVo.XLS_G;
        //                        ws.Cells[r, 8] = tmpVo.XLS_H;
        //                        ws.Cells[r, 9] = tmpVo.XLS_I;
        //                        ws.Cells[r, 10] = tmpVo.XLS_J;
        //                        ws.Cells[r, 11] = tmpVo.XLS_K;
        //                        ws.Cells[r, 12] = tmpVo.XLS_L;
        //                        ws.Cells[r, 13] = tmpVo.XLS_M;
        //                        ws.Cells[r, 14] = tmpVo.XLS_N;
        //                        ws.Cells[r, 15] = tmpVo.XLS_O;
        //                        ws.Cells[r, 16] = tmpVo.XLS_P;
        //                        ws.Cells[r, 17] = tmpVo.XLS_Q;
        //                        ws.Cells[r, 18] = tmpVo.XLS_R;
        //                        ws.Cells[r, 19] = tmpVo.XLS_S;
        //                        ws.Cells[r, 20] = tmpVo.XLS_T;
        //                        ws.Cells[r, 21] = tmpVo.XLS_U;
        //                        ws.Cells[r, 22] = tmpVo.XLS_V;
        //                        ws.Cells[r, 23] = tmpVo.XLS_W;
        //                        ws.Cells[r, 24] = tmpVo.XLS_X;
        //                        ws.Cells[r, 25] = tmpVo.XLS_Y;
        //                        ws.Cells[r, 26] = tmpVo.XLS_Z;

        //                        ws.Cells[r, 26] = tmpVo.XLS_AA;
        //                        ws.Cells[r, 27] = tmpVo.XLS_AB;
        //                        ws.Cells[r, 28] = tmpVo.XLS_AC;
        //                        //ws.Cells[r, 29] = tmpVo.XLS_AD;
        //                        //ws.Cells[r, 30] = tmpVo.XLS_AE;
        //                        //ws.Cells[r, 31] = tmpVo.XLS_AF;
        //                        //ws.Cells[r, 32] = tmpVo.XLS_AG;
        //                        //ws.Cells[r, 33] = tmpVo.XLS_AH;
        //                        //ws.Cells[r, 34] = tmpVo.XLS_AI;
        //                        //ws.Cells[r, 35] = tmpVo.XLS_AJ;
        //                        //ws.Cells[r, 36] = tmpVo.XLS_AC;
        //                        //ws.Cells[r, 37] = tmpVo.XLS_AC;
        //                        //ws.Cells[r, 38] = tmpVo.XLS_AC;
        //                        //ws.Cells[r, 39] = tmpVo.XLS_AC;
        //                        //ws.Cells[r, 40] = tmpVo.XLS_AC;
        //                        //ws.Cells[r, 41] = tmpVo.XLS_AC;
        //                        //ws.Cells[r, 42] = tmpVo.XLS_AC;
        //                        //ws.Cells[r, 43] = tmpVo.XLS_AC;
        //                        //ws.Cells[r, 44] = tmpVo.XLS_AC;
        //                        //ws.Cells[r, 45] = tmpVo.XLS_AC;
        //                        //ws.Cells[r, 46] = tmpVo.XLS_AC;
        //                        //ws.Cells[r, 47] = tmpVo.XLS_AC;
        //                        //ws.Cells[r, 48] = tmpVo.XLS_AC;
        //                        //ws.Cells[r, 49] = tmpVo.XLS_AC;
        //                        //ws.Cells[r, 51] = tmpVo.XLS_AC;

        //                        ws.Cells[r, 59] = tmpVo.XLS_BG;

        //                        r++;

        //                    }
        //                    wb.SaveAs(@Path, Excel.XlFileFormat.xlWorkbookNormal);
        //                    wb.Close(true);
        //                    excelApp.Quit();

        //                }
        //                //폐이지 처리 하기
        //                else if (excelList.Count > 100)
        //                {
        //                    int nCnt = excelList.Count;

        //                    int nPage = excelList.Count / 100;
        //                    int nMod = excelList.Count % 100;



        //                    int min = 0;
        //                    int max = 0;

        //                    for (int w = 0; w < nPage; w++)
        //                    {
        //                        excelApp = new Excel.Application();
        //                        wb = excelApp.Workbooks.Add();
        //                        ws = wb.Worksheets.get_Item(1) as Excel.Worksheet;


        //                        min = w * 100;
        //                        max = min + 100;

        //                        r = 8;
        //                        for (int x = min; x < max; x++)
        //                        {
        //                            tmpVo = excelList[x];

        //                            ws.Cells[r, 1] = tmpVo.XLS_A;
        //                            ws.Cells[r, 2] = tmpVo.XLS_B;
        //                            ws.Cells[r, 3] = tmpVo.XLS_C;
        //                            ws.Cells[r, 4] = tmpVo.XLS_D;
        //                            ws.Cells[r, 5] = tmpVo.XLS_E;
        //                            ws.Cells[r, 6] = tmpVo.XLS_F;
        //                            ws.Cells[r, 7] = tmpVo.XLS_G;
        //                            ws.Cells[r, 8] = tmpVo.XLS_H;
        //                            ws.Cells[r, 9] = tmpVo.XLS_I;
        //                            ws.Cells[r, 10] = tmpVo.XLS_J;
        //                            ws.Cells[r, 11] = tmpVo.XLS_K;
        //                            ws.Cells[r, 12] = tmpVo.XLS_L;
        //                            ws.Cells[r, 13] = tmpVo.XLS_M;
        //                            ws.Cells[r, 14] = tmpVo.XLS_N;
        //                            ws.Cells[r, 15] = tmpVo.XLS_O;
        //                            ws.Cells[r, 16] = tmpVo.XLS_P;
        //                            ws.Cells[r, 17] = tmpVo.XLS_Q;
        //                            ws.Cells[r, 18] = tmpVo.XLS_R;
        //                            ws.Cells[r, 19] = tmpVo.XLS_S;
        //                            ws.Cells[r, 20] = tmpVo.XLS_T;
        //                            ws.Cells[r, 21] = tmpVo.XLS_U;
        //                            ws.Cells[r, 22] = tmpVo.XLS_V;
        //                            ws.Cells[r, 23] = tmpVo.XLS_W;
        //                            ws.Cells[r, 24] = tmpVo.XLS_X;
        //                            ws.Cells[r, 25] = tmpVo.XLS_Y;
        //                            ws.Cells[r, 26] = tmpVo.XLS_Z;

        //                            ws.Cells[r, 26] = tmpVo.XLS_AA;
        //                            ws.Cells[r, 27] = tmpVo.XLS_AB;
        //                            ws.Cells[r, 28] = tmpVo.XLS_AC;
        //                            //ws.Cells[r, 29] = tmpVo.XLS_AD;
        //                            //ws.Cells[r, 30] = tmpVo.XLS_AE;
        //                            //ws.Cells[r, 31] = tmpVo.XLS_AF;
        //                            //ws.Cells[r, 32] = tmpVo.XLS_AG;
        //                            //ws.Cells[r, 33] = tmpVo.XLS_AH;
        //                            //ws.Cells[r, 34] = tmpVo.XLS_AI;
        //                            //ws.Cells[r, 35] = tmpVo.XLS_AJ;
        //                            //ws.Cells[r, 36] = tmpVo.XLS_AC;
        //                            //ws.Cells[r, 37] = tmpVo.XLS_AC;
        //                            //ws.Cells[r, 38] = tmpVo.XLS_AC;
        //                            //ws.Cells[r, 39] = tmpVo.XLS_AC;
        //                            //ws.Cells[r, 40] = tmpVo.XLS_AC;
        //                            //ws.Cells[r, 41] = tmpVo.XLS_AC;
        //                            //ws.Cells[r, 42] = tmpVo.XLS_AC;
        //                            //ws.Cells[r, 43] = tmpVo.XLS_AC;
        //                            //ws.Cells[r, 44] = tmpVo.XLS_AC;
        //                            //ws.Cells[r, 45] = tmpVo.XLS_AC;
        //                            //ws.Cells[r, 46] = tmpVo.XLS_AC;
        //                            //ws.Cells[r, 47] = tmpVo.XLS_AC;
        //                            //ws.Cells[r, 48] = tmpVo.XLS_AC;
        //                            //ws.Cells[r, 49] = tmpVo.XLS_AC;
        //                            //ws.Cells[r, 51] = tmpVo.XLS_AC;

        //                            ws.Cells[r, 59] = tmpVo.XLS_BG;

        //                            r++;

        //                        }
        //                        wb.SaveAs(@Path + w, Excel.XlFileFormat.xlWorkbookNormal);
        //                        wb.Close(true);
        //                        excelApp.Quit();
        //                    }


        //                    if (nMod != 0)
        //                    {
        //                        min = nPage * 100;

        //                        r = 8;
        //                        for (int x = min; x < excelList.Count; x++)
        //                        {
        //                            tmpVo = excelList[x];

        //                            ws.Cells[r, 1] = tmpVo.XLS_A;
        //                            ws.Cells[r, 2] = tmpVo.XLS_B;
        //                            ws.Cells[r, 3] = tmpVo.XLS_C;
        //                            ws.Cells[r, 4] = tmpVo.XLS_D;
        //                            ws.Cells[r, 5] = tmpVo.XLS_E;
        //                            ws.Cells[r, 6] = tmpVo.XLS_F;
        //                            ws.Cells[r, 7] = tmpVo.XLS_G;
        //                            ws.Cells[r, 8] = tmpVo.XLS_H;
        //                            ws.Cells[r, 9] = tmpVo.XLS_I;
        //                            ws.Cells[r, 10] = tmpVo.XLS_J;
        //                            ws.Cells[r, 11] = tmpVo.XLS_K;
        //                            ws.Cells[r, 12] = tmpVo.XLS_L;
        //                            ws.Cells[r, 13] = tmpVo.XLS_M;
        //                            ws.Cells[r, 14] = tmpVo.XLS_N;
        //                            ws.Cells[r, 15] = tmpVo.XLS_O;
        //                            ws.Cells[r, 16] = tmpVo.XLS_P;
        //                            ws.Cells[r, 17] = tmpVo.XLS_Q;
        //                            ws.Cells[r, 18] = tmpVo.XLS_R;
        //                            ws.Cells[r, 19] = tmpVo.XLS_S;
        //                            ws.Cells[r, 20] = tmpVo.XLS_T;
        //                            ws.Cells[r, 21] = tmpVo.XLS_U;
        //                            ws.Cells[r, 22] = tmpVo.XLS_V;
        //                            ws.Cells[r, 23] = tmpVo.XLS_W;
        //                            ws.Cells[r, 24] = tmpVo.XLS_X;
        //                            ws.Cells[r, 25] = tmpVo.XLS_Y;
        //                            ws.Cells[r, 26] = tmpVo.XLS_Z;

        //                            ws.Cells[r, 26] = tmpVo.XLS_AA;
        //                            ws.Cells[r, 27] = tmpVo.XLS_AB;
        //                            ws.Cells[r, 28] = tmpVo.XLS_AC;
        //                            //ws.Cells[r, 29] = tmpVo.XLS_AD;
        //                            //ws.Cells[r, 30] = tmpVo.XLS_AE;
        //                            //ws.Cells[r, 31] = tmpVo.XLS_AF;
        //                            //ws.Cells[r, 32] = tmpVo.XLS_AG;
        //                            //ws.Cells[r, 33] = tmpVo.XLS_AH;
        //                            //ws.Cells[r, 34] = tmpVo.XLS_AI;
        //                            //ws.Cells[r, 35] = tmpVo.XLS_AJ;
        //                            //ws.Cells[r, 36] = tmpVo.XLS_AC;
        //                            //ws.Cells[r, 37] = tmpVo.XLS_AC;
        //                            //ws.Cells[r, 38] = tmpVo.XLS_AC;
        //                            //ws.Cells[r, 39] = tmpVo.XLS_AC;
        //                            //ws.Cells[r, 40] = tmpVo.XLS_AC;
        //                            //ws.Cells[r, 41] = tmpVo.XLS_AC;
        //                            //ws.Cells[r, 42] = tmpVo.XLS_AC;
        //                            //ws.Cells[r, 43] = tmpVo.XLS_AC;
        //                            //ws.Cells[r, 44] = tmpVo.XLS_AC;
        //                            //ws.Cells[r, 45] = tmpVo.XLS_AC;
        //                            //ws.Cells[r, 46] = tmpVo.XLS_AC;
        //                            //ws.Cells[r, 47] = tmpVo.XLS_AC;
        //                            //ws.Cells[r, 48] = tmpVo.XLS_AC;
        //                            //ws.Cells[r, 49] = tmpVo.XLS_AC;
        //                            //ws.Cells[r, 51] = tmpVo.XLS_AC;

        //                            ws.Cells[r, 59] = tmpVo.XLS_BG;

        //                            r++;

        //                        }
        //                        wb.SaveAs(@Path + (nPage), Excel.XlFileFormat.xlWorkbookNormal);
        //                        wb.Close(true);
        //                        excelApp.Quit();
        //                    }
        //                }

        //            }

        //            WinUIMessageBox.Show("[" + @Path + "] 저장 되었습니다" , title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
        //        }
        //        catch (System.Exception eLog)
        //        {
        //            //DXSplashScreen.Close();
        //            WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
        //            return;
        //        }
        //        finally
        //        {
        //            // Clean up
        //            ReleaseExcelObject(ws);
        //            ReleaseExcelObject(wb);
        //            ReleaseExcelObject(excelApp);
        //        }
        //    }
        //}

       [Command]
       public async void NewContact()
        {
            try
            {
                string _path = System.Windows.Forms.Application.StartupPath + @"\cpcn.xls";
                DateTime _slYrMon = StartDt;
                string _bilCd = "A";
                string _grpBilNo = null;
                string _clzFlg = (this.M_RLSE_PRT_FLG.Equals("전체") ? null : (this.M_RLSE_PRT_FLG.Equals("출력") ? "Y" : "N"));



                //MessageBoxResult result = WinUIMessageBox.Show("이미 출력이 된 계산서가 있습니다. 재발행 하시겠습니까?", "[일괄발행]" + title, MessageBoxButton.YesNo, MessageBoxImage.Question);
                //if (result == MessageBoxResult.Yes)
                //{
                //    return;
                //}



                if (System.IO.File.Exists(_path))
                {
                    //파일 삭제
                    System.IO.File.Delete(_path);
                }

                System.IO.File.Create(_path).Close();

                excelDialog = new S22313ExcelDialog(_path, SystemProperties.USER_VO.EMPE_PLC_NM, _slYrMon, _bilCd, _grpBilNo, _clzFlg);
                excelDialog.Title = "[엑셀]" + this.title;
                excelDialog.Owner = Application.Current.MainWindow;
                excelDialog.BorderEffect = BorderEffect.Default;
                excelDialog.BorderEffectActiveColor = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 128, 0));
                excelDialog.BorderEffectInactiveColor = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 170, 170));
                bool isDialog = (bool)excelDialog.ShowDialog();
                //if (isDialog)
                {
                    if (System.IO.File.Exists(_path))
                    {
                        //파일 삭제
                        System.IO.File.Delete(_path);
                    }
                    Refresh();
                }
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }


        }


        private static void ReleaseExcelObject(object obj)
        {
            try
            {
                if (obj != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                    obj = null;
                }
            }
            catch (Exception ex)
            {
                obj = null;
                throw ex;
            }
            finally
            {
                GC.Collect();
            }
        }

        private SystemCodeVo _SelectedItemAreaCd;
        public SystemCodeVo SelectedItemAreaCd
        {
            get { return _SelectedItemAreaCd; }
            set
            {
                if (value != null)
                {
                    SetProperty(ref _SelectedItemAreaCd, value, () => SelectedItemAreaCd);
                }
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

        //    MessageBoxResult result = WinUIMessageBox.Show("[" + (StartDt).ToString("yyyy-MM") + "/" + M_SL_AREA_NM + "] 정말로 마감 하시겠습니까?", "[마감]" + title, MessageBoxButton.YesNo, MessageBoxImage.Question);
        //    if (result == MessageBoxResult.Yes)
        //    {
        //        JobVo _param = new JobVo();
        //        for (int x = 0; x < SelectMstList.Count; x++)
        //        {
        //            if (SelectMstList[x].DC_CHK)
        //            {
        //                _param.CO_CD = SelectMstList[x].CO_CD;
        //                _param.AREA_CD = SelectMstList[x].AREA_CD;
        //                _param.SL_DC_YRMON = Convert.ToDateTime(SelectMstList[x].SL_DC_YRMON).ToString("yyyyMM");

        //                JobVo resultVo = saleOrderClient.S2223UpdateMst(SelectMstList[x]);
        //                if (!resultVo.isSuccess)
        //                {
        //                    //실패
        //                    WinUIMessageBox.Show(resultVo.Message, "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
        //                    return;
        //                }

        //            }
        //        }
        //        Refresh();
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
        //    if (SelectedMstItem != null)
        //    {
        //        //MessageBoxResult result = WinUIMessageBox.Show("[" + SelectedMstItem.CLT_BIL_NO + "] 정말로 삭제 하시겠습니까?", "[삭제]" + title, MessageBoxButton.YesNo, MessageBoxImage.Question);
        //        //if (result == MessageBoxResult.Yes)
        //        //{
        //        //    saleOrderClient.S2221DeleteMst(SelectedMstItem);
        //        //    WinUIMessageBox.Show("삭제가 완료되었습니다.", "[삭제]" + title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
        //        //    Refresh();
        //        //}
        //    }
        //}


        public void setTitle()
        {
            Title = "[기간]" + (StartDt).ToString("yyyy-MM") + ", [사업장]" + M_SL_AREA_NM.CLSS_DESC;
        }


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

            //RlsePrtFlgList = new List<CodeDao>()
            //                    {
            //                            new CodeDao(){ CLSS_CD = null, CLSS_DESC = "전체"}
            //                        ,   new CodeDao(){ CLSS_CD = "Y", CLSS_DESC = "발행"}
            //                        ,   new CodeDao(){CLSS_CD = "N", CLSS_DESC = "미발행"}
            //                    };
            //M_RLSE_PRT_FLG = "미발행";
            RlsePrtFlgList = new List<SystemCodeVo>()
            {
                    new SystemCodeVo(){ CLSS_CD = null, CLSS_DESC = "전체"}
                ,   new SystemCodeVo(){ CLSS_CD = "Y", CLSS_DESC = "발행"}
                ,   new SystemCodeVo(){CLSS_CD = "N", CLSS_DESC = "미발행"}
            };
            this.M_RLSE_PRT_FLG = RlsePrtFlgList[2];

            Refresh();
        }

    }
}
