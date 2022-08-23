using AquilaErpWpfApp3.M.View.Dialog;
using AquilaErpWpfApp3.Util;
using AquilaErpWpfApp3.View.M.Report;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Code;
using ModelsLibrary.Man;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Media;

namespace AquilaErpWpfApp3.ViewModel
{
    //설비 관리
    public sealed class M6500ViewModel : ViewModelBase, INotifyPropertyChanged
    {

        private string _title = "사출 작업지시등록";

        //private static ManServiceClient manClient = SystemProperties.ManClient;

        private IList<ManVo> selectedMasterViewList;
        private IList<ManVo> selectedDetailViewListt;

        ////Master Dialog
        //private ICommand masterSearchDialogCommand;
        //private ICommand masterNewDialogCommand;
        //private ICommand masterEditDialogCommand;
        //private ICommand masterDelDialogCommand;
        ////
        private M6500MasterDialog masterDialog;


        public M6500ViewModel() 
        {
            SYSTEM_CODE_VO();
            //Refresh();
        }

        [Command]
       public async void Refresh()
        {
            try
            {
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6500/mst", new StringContent(JsonConvert.SerializeObject(new ManVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD, ROUT_CD = "IN", A_ROUT_CD = new string[] { "IN" } }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectedMasterViewList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
                    }
                    //SelectedMasterViewList = manClient.M6625SelectMaster(new ManVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD });
                    if (SelectedMasterViewList.Count > 0)
                    {
                        isM_UPDATE = true;
                        isM_DELETE = true;

                        isM_REPORT = true;


                        //this.N1ST_PLN_TITLE = Convert.ToDateTime(M_PLN_YRMON.FM_DT).AddDays(0).ToString("yyyy-MM-dd");
                        //this.N2ND_PLN_TITLE = Convert.ToDateTime(M_PLN_YRMON.FM_DT).AddDays(1).ToString("yyyy-MM-dd");
                        //this.N3RD_PLN_TITLE = Convert.ToDateTime(M_PLN_YRMON.FM_DT).AddDays(2).ToString("yyyy-MM-dd");
                        //this.N4TH_PLN_TITLE = Convert.ToDateTime(M_PLN_YRMON.FM_DT).AddDays(3).ToString("yyyy-MM-dd");
                        //this.N5TH_PLN_TITLE = Convert.ToDateTime(M_PLN_YRMON.FM_DT).AddDays(4).ToString("yyyy-MM-dd");
                        //this.N6TH_PLN_TITLE = Convert.ToDateTime(M_PLN_YRMON.FM_DT).AddDays(5).ToString("yyyy-MM-dd");
                        //this.N7TH_PLN_TITLE = Convert.ToDateTime(M_PLN_YRMON.FM_DT).AddDays(6).ToString("yyyy-MM-dd");



                        //if (string.IsNullOrEmpty(_PCK_PLST_CLSS_CD_PCK_PLST_CD))
                        //{
                        SelectedMasterItem = SelectedMasterViewList[0];
                        //}
                        //else
                        //{
                        //    SelectedMasterItem = SelectedMasterViewList.Where(x => (x.PCK_PLST_CLSS_CD + "_" + x.PCK_PLST_CD).Equals(_PCK_PLST_CLSS_CD_PCK_PLST_CD)).LastOrDefault<ManVo>();
                        //}
                    }
                    else
                    {
                        isM_UPDATE = false;
                        isM_DELETE = false;

                        isM_REPORT = false;
                    }
                }
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }

        #region Functon (Master Add, Edit, Del)
        public IList<ManVo> SelectedMasterViewList
        {
            get { return selectedMasterViewList; }
            private set { SetProperty(ref selectedMasterViewList, value, () => SelectedMasterViewList); }
        }

        ManVo _selectMasterItem;
        public ManVo SelectedMasterItem
        {
            get
            {
                return _selectMasterItem;
            }
            set
            {
                if (value != null)
                {
                    SetProperty(ref _selectMasterItem, value, () => SelectedMasterItem, OnSelectedMasterItem);
                }
            }
        }

        public IList<ManVo> SelectedDetailViewList
        {
            get { return selectedDetailViewListt; }
            private set { SetProperty(ref selectedDetailViewListt, value, () => SelectedDetailViewList); }
        }

        ManVo _selectedDetailItem;
        public ManVo SelectedDetailItem
        {
            get
            {
                return _selectedDetailItem;
            }
            set
            {
                if (value != null)
                {
                    SetProperty(ref _selectedDetailItem, value, () => SelectedDetailItem);
                }
            }
        }

        void OnSelectedMasterItem()
        {
            //mst -> dtl
            OnSelectedMasterItemChanged();
        }


        async void OnSelectedMasterItemChanged(string _LOT_DIV_NO = null)
        {
            try
            {
                if (this.SelectedMasterItem == null) { return; }


                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6500/dtl", new StringContent(JsonConvert.SerializeObject(SelectedMasterItem), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectedDetailViewList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();

                        if (SelectedDetailViewList.Count > 0)
                        {
                            isD_UPDATE = true;
                            isD_DELETE = true;



                            if (string.IsNullOrEmpty(_LOT_DIV_NO))
                            {
                                SelectedDetailItem = SelectedDetailViewList[0];
                            }
                            else
                            {
                                SelectedDetailItem = SelectedDetailViewList.Where(x => (x.LOT_DIV_NO).Equals(_LOT_DIV_NO)).LastOrDefault<ManVo>();
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
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }

        //public ICommand MasterSearchDialogCommand
        //{
        //    get
        //    {
        //        if (masterSearchDialogCommand == null)
        //            masterSearchDialogCommand = new DelegateCommand(Refresh);
        //        return masterSearchDialogCommand;
        //    }
        //}

        //public void SearchMasterContact()
        //{
        //    ProgressWindow program = new ProgressWindow();
        //    try
        //    {
        //        ThreadStart start = delegate()
        //        {
        //            program.Dispatcher.Invoke(DispatcherPriority.Normal,(Action)(() => 
        //            {
        //                program.Show();
        //                Thread.Sleep(100);
        //                SelectedMasterViewList = codeClient.SelectMasterCode(new SystemCodeVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD });
        //                OnSelectedMasterItemChanged();
        //                program.Close();
        //            }));
        //        };
        //        new Thread(start).Start();
        //    }
        //    catch (System.Exception eLog)
        //    {
        //    //    program.Close();
        //        WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]표준 공정 관리", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
        //        return;
        //    }
        //}

        //public ICommand MasterNewDialogCommand
        //{
        //    get
        //    {
        //        if (masterNewDialogCommand == null)
        //            masterNewDialogCommand = new DelegateCommand(NewMasterContact);
        //        return masterNewDialogCommand;
        //    }
        //}
        [Command]
        public void NewMasterContact()
        {
            if (this._selectMasterItem == null) { return; }

            masterDialog = new M6500MasterDialog(new ManVo() { EQ_NO = this._selectMasterItem.EQ_NO, GBN = this._selectMasterItem.GBN, ITM_CD = this._selectMasterItem.ITM_CD, PCK_FLG = this._selectMasterItem.PCK_FLG, SL_ORD_NO = this._selectMasterItem.SL_ORD_NO, SL_ORD_SEQ = this._selectMasterItem.SL_ORD_SEQ, PROD_QTY = this._selectMasterItem.PROD_QTY, LOT_DIV_QTY = 0,  PROD_PLN_DT = this._selectMasterItem.WRK_DT, PROD_ORD_NO = this._selectMasterItem.PROD_ORD_NO, CHNL_CD = this._selectMasterItem.CHNL_CD, DY_NGT_NM ="주간", MAKE_ST_DT = System.DateTime.Now.ToString("yyyy-MM-dd") });
            masterDialog.Title = _title + " - 추가";
            masterDialog.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            masterDialog.Owner = Application.Current.MainWindow;
            masterDialog.BorderEffect = BorderEffect.Default;
            masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            bool isDialog = (bool)masterDialog.ShowDialog();
            if (isDialog)
            {
                OnSelectedMasterItemChanged();
            }
        }

        //public ICommand MasterEditDialogCommand
        //{
        //    get
        //    {
        //        if (masterEditDialogCommand == null)
        //            masterEditDialogCommand = new DelegateCommand(EditMasterContact);
        //        return masterEditDialogCommand;
        //    }
        //}
        [Command]
        public void EditMasterContact()
        {
            if (this._selectMasterItem == null) { return; }
            masterDialog = new M6500MasterDialog(SelectedDetailItem);
            masterDialog.Title = _title + " - 수정";
            masterDialog.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            masterDialog.BorderEffect = BorderEffect.Default;
            masterDialog.Owner = Application.Current.MainWindow;
            masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            bool isDialog = (bool)masterDialog.ShowDialog();
            if (isDialog)
            {
                OnSelectedMasterItemChanged(masterDialog.ResultDao.LOT_DIV_NO);
            }
        }

        //public ICommand MasterDelDialogCommand
        //{
        //    get
        //    {
        //        if (masterDelDialogCommand == null)
        //            masterDelDialogCommand = new DelegateCommand(DelMasterContact);
        //        return masterDelDialogCommand;
        //    }
        //}

        [Command]
        public async void DelMasterContact()
        {
            try
            {
                ManVo delDao = this.SelectedDetailItem;
                if (delDao != null)
                {
                    MessageBoxResult result = WinUIMessageBox.Show("[" + delDao.LOT_DIV_NO + " / " + delDao.MAKE_ST_DT + "] 정말로 삭제 하시겠습니까?", _title, MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        //delDao.RN = x;
                        //delDao.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
                        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6500/dtl/d", new StringContent(JsonConvert.SerializeObject(delDao), System.Text.Encoding.UTF8, "application/json")))
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
                        }
                    }
                    OnSelectedMasterItemChanged();

                    //성공
                    WinUIMessageBox.Show("삭제가 완료되었습니다.", _title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                }
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }
        #endregion


        [Command]
        public async void OpenReportViewContact()
        {
            isPRNT_PLN = true;


            OnSelectedMasterItemChanged();
        }

        [Command]
        public async void ReportContact()
        {
            try
            {
                if (this.SelectedDetailItem == null) { return; }

                MessageBoxResult result = WinUIMessageBox.Show("[" + this.SelectedDetailItem.LOT_DIV_NO + " / " + this.SelectedDetailItem.MAKE_ST_DT + " / " + this.SelectedDetailItem.EQ_NO + "] 작지 발행 하시겠습니까?", _title, MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {

                    //List<ManVo> _paramList = new List<ManVo>();

                    //ManVo _param = new ManVo();
                    //_param.LOT_DIV_NO = this.SelectedDetailItem.LOT_DIV_NO;
                    //_param.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
                    //_param.WKY_YRMON = this.SelectedMasterItem.WKY_YRMON;
                    //_param.WK = this.SelectedMasterItem.WK;
                    //_param.CMPO_CD = this.SelectedMasterItem.CMPO_CD;
                    //_param.PROD_EQ_NO = this.SelectedMasterItem.PROD_EQ_NO;
                    //_param.CRE_USR_ID = SystemProperties.USER_VO.USR_ID;
                    //_param.UPD_USR_ID = SystemProperties.USER_VO.USR_ID;
                    //_param.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
                    //_param.RN = this.SelectedDetailItem.RN;
                    //_param.PROD_PLN_QTY = this.SelectedDetailItem.PROD_PLN_QTY;
                    //_param.INP_STAFF_VAL = 0;
                    //_param.MM_RMK = this.SelectedDetailItem.MM_RMK;
                    //_param.PCK_FLG = "IN";
                    //_param.A_ROUT_CD = new string[] { "IN" };
                    //
                    //_paramList.Add(_param);


                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6500/report", new StringContent(JsonConvert.SerializeObject(SelectedDetailItem), System.Text.Encoding.UTF8, "application/json")))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            //출력물
                            List<ManVo> _reportList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
                            if (_reportList.Count > 0)
                            {
                                //_reportList[0].FM_DT = System.DateTime.Now.ToString("yyyy-MM-dd");
                                _reportList[0].FM_DT = this.SelectedDetailItem.MAKE_ST_DT;
                                _reportList[0].TO_DT = this.SelectedDetailItem.MAKE_END_DT;
                                _reportList[0].ROUT_NM = "사출";

                                //
                                M665101Report report = new M665101Report(_reportList);
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
                                window.Title = _title;
                                window.Owner = Application.Current.MainWindow;
                                window.ShowDialog();

                            }
                            else
                            {
                                WinUIMessageBox.Show("작업지시서 발행이  존재 하지 않습니다", this._title, MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.None, MessageBoxOptions.None);
                                return;
                            }
                            //출력 유무 업데이트
                            //SearchDetailJob.UPD_USR_ID = SystemProperties.USER;
                            //using (HttpResponseMessage responseY = await SystemProperties.PROGRAM_HTTP.PostAsync("m66520/mst/u", new StringContent(JsonConvert.SerializeObject(SearchDetailJob), System.Text.Encoding.UTF8, "application/json")))
                            //{
                            //}
                        }

                        //if (response.IsSuccessStatusCode)
                        //{
                        //    string resultMsg = await response.Content.ReadAsStringAsync();
                        //    if (int.TryParse(resultMsg, out _Num) == false)
                        //    {
                        //        //실패
                        //        WinUIMessageBox.Show(resultMsg, _title, MessageBoxButton.OK, MessageBoxImage.Error);
                        //        return;
                        //    }
                        //    //성공
                        //    WinUIMessageBox.Show("완료 되었습니다", _title, MessageBoxButton.OK, MessageBoxImage.Information);
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

        [Command]
        public async void CloseReportViewContact()
        {
            isPRNT_PLN = false;
        }


        public async void SYSTEM_CODE_VO()
        {
            //사업장
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "A-001"))
            {
                if (response.IsSuccessStatusCode)
                {
                    ProdLocList = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                    if (ProdLocList.Count > 0)
                    {
                        //ProdLocList.Insert(0, new SystemCodeVo() { CLSS_CD = null, CLSS_DESC = "전체" });
                        M_PROD_LOC_NM = ProdLocList[0];
                    }
                }
            }

            //using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("M665100", new StringContent(JsonConvert.SerializeObject(new ManVo() { DELT_FLG = "N", CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
            //{
            //    if (response.IsSuccessStatusCode)
            //    {
            //        PlnYrmonList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
            //        if (PlnYrmonList.Count > 0)
            //        {
            //            //CoNmList.Insert(0, new SystemCodeVo() { CO_NM = "전체", CO_NO = null });
            //            //M_PLN_YRMON = PlnYrmonList[PlnYrmonList.Count - 1];
            //            M_PLN_YRMON = PlnYrmonList.Where<ManVo>(w => w.WKY_YRMON.Equals(System.DateTime.Now.ToString("yyyyMM"))).FirstOrDefault<ManVo>();
            //        }
            //    }
            //}

            Refresh();
        }

        //공장
        private IList<SystemCodeVo> _ProdLocList = new List<SystemCodeVo>();
        public IList<SystemCodeVo> ProdLocList
        {
            get { return _ProdLocList; }
            set { SetProperty(ref _ProdLocList, value, () => ProdLocList); }
        }
        private SystemCodeVo _M_PROD_LOC_NM;
        public SystemCodeVo M_PROD_LOC_NM
        {
            get { return _M_PROD_LOC_NM; }
            set { SetProperty(ref _M_PROD_LOC_NM, value, () => M_PROD_LOC_NM); }
        }


        ////년월 / 주차
        //private IList<ManVo> _PlnYrmonList = new List<ManVo>();
        //public IList<ManVo> PlnYrmonList
        //{
        //    get { return _PlnYrmonList; }
        //    set { SetProperty(ref _PlnYrmonList, value, () => PlnYrmonList); }
        //}
        //private ManVo _M_PLN_YRMON;
        //public ManVo M_PLN_YRMON
        //{
        //    get { return _M_PLN_YRMON; }
        //    set { SetProperty(ref _M_PLN_YRMON, value, () => M_PLN_YRMON); }
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

        private string _M_SEARCH_TEXT = string.Empty;
        public string M_SEARCH_TEXT
        {
            get { return _M_SEARCH_TEXT; }
            set { SetProperty(ref _M_SEARCH_TEXT, value, () => M_SEARCH_TEXT); }
        }



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


        ////TITLE
        //private string _N1ST_PLN_TITLE = string.Empty;
        //public string N1ST_PLN_TITLE
        //{
        //    get { return _N1ST_PLN_TITLE; }
        //    set { SetProperty(ref _N1ST_PLN_TITLE, value, () => N1ST_PLN_TITLE); }
        //}
        //private string _N2ND_PLN_TITLE = string.Empty;
        //public string N2ND_PLN_TITLE
        //{
        //    get { return _N2ND_PLN_TITLE; }
        //    set { SetProperty(ref _N2ND_PLN_TITLE, value, () => N2ND_PLN_TITLE); }
        //}
        //private string _N3RD_PLN_TITLE = string.Empty;
        //public string N3RD_PLN_TITLE
        //{
        //    get { return _N3RD_PLN_TITLE; }
        //    set { SetProperty(ref _N3RD_PLN_TITLE, value, () => N3RD_PLN_TITLE); }
        //}
        //private string _N4TH_PLN_TITLE = string.Empty;
        //public string N4TH_PLN_TITLE
        //{
        //    get { return _N4TH_PLN_TITLE; }
        //    set { SetProperty(ref _N4TH_PLN_TITLE, value, () => N4TH_PLN_TITLE); }
        //}
        //private string _N5TH_PLN_TITLE = string.Empty;
        //public string N5TH_PLN_TITLE
        //{
        //    get { return _N5TH_PLN_TITLE; }
        //    set { SetProperty(ref _N5TH_PLN_TITLE, value, () => N5TH_PLN_TITLE); }
        //}
        //private string _N6TH_PLN_TITLE = string.Empty;
        //public string N6TH_PLN_TITLE
        //{
        //    get { return _N6TH_PLN_TITLE; }
        //    set { SetProperty(ref _N6TH_PLN_TITLE, value, () => N6TH_PLN_TITLE); }
        //}
        //private string _N7TH_PLN_TITLE = string.Empty;
        //public string N7TH_PLN_TITLE
        //{
        //    get { return _N7TH_PLN_TITLE; }
        //    set { SetProperty(ref _N7TH_PLN_TITLE, value, () => N7TH_PLN_TITLE); }
        //}




        //작업지시서 창 - 활성
        private bool? _isPRNT_PLN = true;
        public bool? isPRNT_PLN
        {
            get { return _isPRNT_PLN; }
            set { SetProperty(ref _isPRNT_PLN, value, () => isPRNT_PLN); }
        }

        private bool? _isM_REPORT = false;
        public bool? isM_REPORT
        {
            get { return _isM_REPORT; }
            set { SetProperty(ref _isM_REPORT, value, () => isM_REPORT); }
        }


    }
}
