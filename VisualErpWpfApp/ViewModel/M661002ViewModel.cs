using AquilaErpWpfApp3.Util;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Code;
using ModelsLibrary.Man;
using DevExpress.Xpf.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Windows;
using AquilaErpWpfApp3.View.M.Report;
using DevExpress.XtraReports.UI;

namespace AquilaErpWpfApp3.ViewModel
{
    public sealed class M661002ViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private string title = "실적현황 및 작업 지시서(일괄 발행)";

        //private static SaleOrderServiceClient saleOrderClient = SystemProperties.SaleOrderClient;
        private IList<ManVo> selectedMstList = new List<ManVo>();
        private IList<ManVo> selectedMstItemsList = new List<ManVo>();

        //private ObservableCollection<PurVo> selectedDtlList = new ObservableCollection<PurVo>();
        //private IList<JobVo> selectedDtlList = new List<JobVo>();

        //private S2222MasterDialog masterDialog;

        ////Menu Dialog
        //private ICommand _searchDialogCommand;
        //private ICommand _newDialogCommand;
        //private ICommand _editDetailDialogCommand;
        //private ICommand _delDialogCommand;

        public M661002ViewModel() 
        {
            StartDt = System.DateTime.Now.AddMonths(-1);
            EndDt = System.DateTime.Now;


            //사업장
            SYSTEM_CODE_VO();
            // - Refresh();
        }

        [Command]
        public async void Refresh()
        {
            try
            {
                DXSplashScreen.Show<ProgressWindow>();

                ManVo _param = new ManVo();
                _param.FM_DT = (StartDt).ToString("yyyy-MM-dd");
                _param.TO_DT = (EndDt).ToString("yyyy-MM-dd");
                //사업장
                //_param.AREA_CD = M_SL_AREA_NM.CLSS_CD;
                //_param.AREA_NM = M_SL_AREA_NM.CLSS_DESC;
                //채널
                _param.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
                //
                _param.PCK_FLG = (this.M_GBN.Equals("전체") ? null : this.M_GBN.Equals("충전") ? "C" : "P");

                ////
                //SelectMstList = saleOrderClient.S2222SelectMstList(_param);
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m661002", new StringContent(JsonConvert.SerializeObject(_param), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectMstList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
                    }
                    setTitle();

                    if (SelectMstList.Count >= 1)
                    {
                        isM_UPDATE = true;
                        isM_DELETE = true;
                        //if (string.IsNullOrEmpty(_CLT_BIL_DELT_NO))
                        //{
                            SelectedMstItem = SelectMstList[0];
                        //}
                        //else
                        //{
                        //    SelectedMstItem = SelectMstList.Where(x => x.CLT_BIL_DELT_NO.Equals(_CLT_BIL_DELT_NO)).LastOrDefault<ManVo>();
                        //}
                    }
                    else
                    {
                        //SelectDtlList = null;
                        //SearchDetail = null;

                        isM_UPDATE = false;
                        isM_DELETE = false;
                        //
                        //isD_UPDATE = false;
                        //isD_DELETE = false;
                    }
                    //DXSplashScreen.Close();
                }
                DXSplashScreen.Close();
            }
            catch (System.Exception eLog)
            {
                if (DXSplashScreen.IsActive == true)
                {
                    DXSplashScreen.Close();
                }
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }


        //
        [Command]
        public async void PrintContactWork()
        {
            try
            {
                //DevExpress.XtraReports.UI.XtraReport totalReport = new DevExpress.XtraReports.UI.XtraReport();
                //totalReport.CreateDocument(true);


                IList<ManVo> _ItmList;
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m66520/report", new StringContent(JsonConvert.SerializeObject(SelectedMstItem), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        _ItmList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();

                        if (_ItmList.Count > 0)
                        {

                            _ItmList[0].LOT_DIV_NO = SelectedMstItem.LOT_DIV_NO;
                            _ItmList[0].FM_DT = System.DateTime.Now.ToString("yyyy-MM-dd");
                            _ItmList[0].ROUT_NM = "충전";

                            //
                            M66520Report_Work report = new M66520Report_Work(_ItmList);
                            report.Margins.Top = 20;
                            report.Margins.Bottom = 20;
                            report.Margins.Left = 50;
                            report.Margins.Right = 20;
                            report.Landscape = false;

                            report.PrintingSystem.ShowPrintStatusDialog = true;
                            report.PaperKind = System.Drawing.Printing.PaperKind.A4;
                            report.Watermark.Text = Properties.Settings.Default.SettingCompany;
                            report.Watermark.TextDirection = DevExpress.XtraPrinting.Drawing.DirectionMode.ForwardDiagonal;
                            report.Watermark.Font = new System.Drawing.Font(report.PrintingSystem.Watermark.Font.FontFamily, 40);
                            report.Watermark.ForeColor = System.Drawing.Color.PaleTurquoise;
                            report.Watermark.TextTransparency = 150;


                            //IList<M66520Report_Work> reportList = new List<M66520Report_Work>();
                            //reportList.Add(report);
                            //reportList.Add(report);
                            //reportList.Add(report);
                            //reportList.Add(report);

                            report.CreateDocument(true);

                            var window = new DevExpress.Xpf.Printing.DocumentPreviewWindow();
                            window.PreviewControl.DocumentSource = report;
                            report.CreateDocument(true);
                            window.Title = title;
                            window.Owner = Application.Current.MainWindow;
                            window.ShowDialog();

                            //출력 유무 업데이트
                            SelectedMstItem.UPD_USR_ID = SystemProperties.USER;
                            SelectedMstItem.WRK_DIR_PPR_FLG = "Y";
                            SelectedMstItem.LOT_DIV_SEQ = null;
                            using (HttpResponseMessage responseY = await SystemProperties.PROGRAM_HTTP.PostAsync("m66520/mst/u", new StringContent(JsonConvert.SerializeObject(SelectedMstItem), System.Text.Encoding.UTF8, "application/json")))
                            {
                            }

                        }
                    }
                }

                //var window = new DevExpress.Xpf.Printing.DocumentPreviewWindow();
                //window.PreviewControl.DocumentSource = report;
                //report.CreateDocument(true);
                //window.Title = title;
                //window.Owner = Application.Current.MainWindow;
                //window.ShowDialog();

                //ReportPrintTool printTool = new ReportPrintTool(totalReport);
                //printTool.ShowPreviewDialog();

            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }

        //
        [Command]
        public async void PrintContactBulk()
        {
            try
            {
                IList<ManVo> _ItmList;
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m66520/report", new StringContent(JsonConvert.SerializeObject(SelectedMstItem), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        _ItmList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();

                        if (_ItmList.Count > 0)
                        {

                            _ItmList[0].LOT_DIV_NO = SelectedMstItem.LOT_DIV_NO;
                            _ItmList[0].FM_DT = System.DateTime.Now.ToString("yyyy-MM-dd");

                            //
                            M66520Report_Bulk report = new M66520Report_Bulk(_ItmList);
                            report.Margins.Top = 20;
                            report.Margins.Bottom = 20;
                            report.Margins.Left = 50;
                            report.Margins.Right = 20;
                            report.Landscape = false;

                            report.PrintingSystem.ShowPrintStatusDialog = true;
                            report.PaperKind = System.Drawing.Printing.PaperKind.A4;
                            report.Watermark.Text = Properties.Settings.Default.SettingCompany;
                            report.Watermark.TextDirection = DevExpress.XtraPrinting.Drawing.DirectionMode.ForwardDiagonal;
                            report.Watermark.Font = new System.Drawing.Font(report.PrintingSystem.Watermark.Font.FontFamily, 40);
                            report.Watermark.ForeColor = System.Drawing.Color.PaleTurquoise;
                            report.Watermark.TextTransparency = 150;


                            var window = new DevExpress.Xpf.Printing.DocumentPreviewWindow();
                            window.PreviewControl.DocumentSource = report;
                            report.CreateDocument(true);
                            window.Title = title;
                            window.Owner = Application.Current.MainWindow;
                            window.ShowDialog();


                            //출력 유무 업데이트
                            SelectedMstItem.UPD_USR_ID = SystemProperties.USER;
                            SelectedMstItem.WRK_DIR_PPR_FLG = "Y";
                            SelectedMstItem.LOT_DIV_SEQ = null;
                            using (HttpResponseMessage responseY = await SystemProperties.PROGRAM_HTTP.PostAsync("m66520/mst/u", new StringContent(JsonConvert.SerializeObject(SelectedMstItem), System.Text.Encoding.UTF8, "application/json")))
                            {
                            }

                        }
                    }
                }
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }

        //
        [Command]
        public async void PrintContactItems()
        {
            try
            {

                IList<ManVo> _ItmList;
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m66520/report", new StringContent(JsonConvert.SerializeObject(SelectedMstItem), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        _ItmList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();

                        if (_ItmList.Count > 0)
                        {

                            _ItmList[0].LOT_DIV_NO = SelectedMstItem.LOT_DIV_NO;
                            _ItmList[0].FM_DT = System.DateTime.Now.ToString("yyyy-MM-dd");

                            //
                            M66520Report_Items report = new M66520Report_Items(_ItmList);
                            report.Margins.Top = 20;
                            report.Margins.Bottom = 20;
                            report.Margins.Left = 50;
                            report.Margins.Right = 20;
                            report.Landscape = false;

                            report.PrintingSystem.ShowPrintStatusDialog = true;
                            report.PaperKind = System.Drawing.Printing.PaperKind.A4;
                            report.Watermark.Text = Properties.Settings.Default.SettingCompany;
                            report.Watermark.TextDirection = DevExpress.XtraPrinting.Drawing.DirectionMode.ForwardDiagonal;
                            report.Watermark.Font = new System.Drawing.Font(report.PrintingSystem.Watermark.Font.FontFamily, 40);
                            report.Watermark.ForeColor = System.Drawing.Color.PaleTurquoise;
                            report.Watermark.TextTransparency = 150;


                            var window = new DevExpress.Xpf.Printing.DocumentPreviewWindow();
                            window.PreviewControl.DocumentSource = report;
                            report.CreateDocument(true);
                            window.Title = title;
                            window.Owner = Application.Current.MainWindow;
                            window.ShowDialog();


                            //출력 유무 업데이트
                            SelectedMstItem.UPD_USR_ID = SystemProperties.USER;
                            SelectedMstItem.WRK_DIR_PPR_FLG = "Y";
                            SelectedMstItem.LOT_DIV_SEQ = null;
                            using (HttpResponseMessage responseY = await SystemProperties.PROGRAM_HTTP.PostAsync("m66520/mst/u", new StringContent(JsonConvert.SerializeObject(SelectedMstItem), System.Text.Encoding.UTF8, "application/json")))
                            {
                            }
                        }
                    }
                    //else
                    //{
                    //WinUIMessageBox.Show(", "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                    //    return;
                    //}
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

        private string _M_GBN = "전체";
        public string M_GBN
        {
            get { return _M_GBN; }
            set { SetProperty(ref _M_GBN, value, () => M_GBN); }
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


        public IList<ManVo> SelectMstList
        {
            get { return selectedMstList; }
            set { SetProperty(ref selectedMstList, value, () => SelectMstList); }
        }

        ManVo _selectedMstItem;
        public ManVo SelectedMstItem
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


        public IList<ManVo> SelectedMstItems
        {
            get { return selectedMstItemsList; }
            set { SetProperty(ref selectedMstItemsList, value, () => SelectedMstItems); }
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
        //private bool? _isD_UPDATE = false;
        //public bool? isD_UPDATE
        //{
        //    get { return _isD_UPDATE; }
        //    set { SetProperty(ref _isD_UPDATE, value, () => isD_UPDATE); }
        //}

        //private bool? _isD_DELETE = false;
        //public bool? isD_DELETE
        //{
        //    get { return _isD_DELETE; }
        //    set { SetProperty(ref _isD_DELETE, value, () => isD_DELETE); }
        //}


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

        private void SelectMstDetail()
        {
            //if (this.SelectedMstItem != null)
            //{
            //    if (this.SelectedMstItem.CLT_BIL_NM.Equals("통장") || this.SelectedMstItem.CLT_BIL_NM.Equals("카드"))
            //    {
            //        this.DetailView = "1";
            //    }
            //    else if (this.SelectedMstItem.CLT_BIL_NM.Equals("어음(자수)") || this.SelectedMstItem.CLT_BIL_NM.Equals("어음(타수)"))
            //    {
            //        this.DetailView = "2";
            //    }
            //    else
            //    {
            //        this.DetailView = "1";
            //    }
            //}




            //try
            //{
            //    //DXSplashScreen.Show<ProgressWindow>();

            //    if (this._selectedMstItem == null)
            //    {
            //        return;
            //    }
            //    Title = "[기간]" + (StartDt).ToString("yyyy-MM-dd") + "~" + (EndDt).ToString("yyyy-MM-dd") + ",  [사업장]" + M_SL_AREA_NM + ",  [구분]" + M_ITM_GRP_CLSS_CD + ",  [품번]" + SelectedMstItem.ITM_CD + ",  [품명]" + SelectedMstItem.ITM_NM + ",  [규격]" + SelectedMstItem.ITM_SZ_NM + (string.IsNullOrEmpty(M_SEARCH_TEXT) ? "" : (",   [검색]" + M_SEARCH_TEXT)); 

            //    //SelectDtlList = new ObservableCollection<PurVo>(purClient.P4401SelectDtlList(new PurVo() { ITM_CD = this._selectedMstItem.}));
            //    SelectDtlList = codeClient.S1144SelectDtlList(SelectedMstItem);
            //    // //
            //    if (SelectDtlList.Count >= 1)
            //    {
            //        isD_UPDATE = true;
            //        isD_DELETE = true;

            //        SearchDetail = SelectDtlList[0];
            //    }
            //    else
            //    {
            //        isD_UPDATE = false;
            //        isD_DELETE = false;
            //    }

            //    //DXSplashScreen.Close();
            //}
            //catch (System.Exception eLog)
            //{
            //    //DXSplashScreen.Close();
            //    //
            //    WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
            //    return;
            //}
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

        public void setTitle()
        {
            Title = "[기간]" + (StartDt).ToString("yyyy-MM-dd") + "~" + (EndDt).ToString("yyyy-MM-dd") + ", [사업장]" + M_SL_AREA_NM.CLSS_DESC + ", [작업지시서]" + M_GBN;
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
            //Refresh();
        }
    }
}
