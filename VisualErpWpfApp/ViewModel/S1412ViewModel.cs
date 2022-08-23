using AquilaErpWpfApp3.Util;
using AquilaErpWpfApp3.View.S.Dialog;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Spreadsheet;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Code;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Media;

namespace AquilaErpWpfApp3.ViewModel
{
    public sealed class S1412ViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private string title = "바코드 양식";

        //private static CodeServiceClient codeClient = SystemProperties.CodeClient;
        private IList<SystemCodeVo> selectedMstList = new List<SystemCodeVo>();

        //private ObservableCollection<PurVo> selectedDtlList = new ObservableCollection<PurVo>();

        private System.IO.Stream streamFile;
        private S1412MasterDialog masterDialog;
        //private P4411DetailDialog detailDialog;

        ////Menu Dialog
        //private ICommand _searchDialogCommand;
        //private ICommand _newDialogCommand;
        //private ICommand _editDialogCommand;
        //private ICommand _delDialogCommand;

        //private ICommand _printDialogCommand;

        public S1412ViewModel() 
        {
            //StartDt = System.DateTime.Now.AddDays(-10);
            //EndDt = System.DateTime.Now;

            //DeptList = SystemProperties.SYSTEM_DEPT_VO();
            //DeptList.Insert(0, new CodeDao() { CLSS_CD = "", CLSS_DESC = "" });
            //_DeptMap = SystemProperties.SYSTEM_DEPT_MAP();

            //M_DEPT_DESC = SystemProperties.USER_VO.GRP_NM;
            Refresh();
        }

        [Command]
        public async void Refresh(string _RPT_CD = null)
        {
            try
            {
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s1412/mst", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectMstList = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                    }

                    //DXSplashScreen.Show<ProgressWindow>();
                    //SelectMstList = codeClient.S1310SelectMstList(new SystemCodeVo() { FM_DT = (StartDt).ToString("yyyy-MM-dd"), TO_DT = (EndDt).ToString("yyyy-MM-dd"), CHNL_CD = SystemProperties.USER_VO.CHNL_CD, NTC_CLSS_CD = "A", NTC_CD = "A" });
                    ////
                    //Title = "[기간]" + (StartDt).ToString("yyyy-MM-dd") + "~" + (EndDt).ToString("yyyy-MM-dd") + ",  [검 색]" + M_SEARCH_TEXT;

                    if (SelectMstList.Count >= 1)
                    {
                        isM_UPDATE = true;
                        isM_DELETE = true;

                        if (string.IsNullOrEmpty(_RPT_CD))
                        {
                            SelectedMstItem = SelectMstList[0];
                        }
                        else
                        {
                            SelectedMstItem = SelectMstList.Where(x => x.RPT_CD.Equals(_RPT_CD)).LastOrDefault<SystemCodeVo>();
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
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }

        //DateTime _startDt;
        //public DateTime StartDt
        //{
        //    get { return _startDt; }
        //    set { SetProperty(ref _startDt, value, () => StartDt); }
        //}

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

        //private bool? _M_SEARCH_CHECKD = false;
        //public bool? M_SEARCH_CHECKD
        //{
        //    get { return _M_SEARCH_CHECKD; }
        //    set { SetProperty(ref _M_SEARCH_CHECKD, value, () => M_SEARCH_CHECKD); }
        //}


        public IList<SystemCodeVo> SelectMstList
        {
            get { return selectedMstList; }
            set { SetProperty(ref selectedMstList, value, () => SelectMstList); }
        }

        SystemCodeVo _selectedMstItem;
        public SystemCodeVo SelectedMstItem
        {
            get
            {
                return _selectedMstItem;
            }
            set
            {
                if (value != null)
                {
                    SetProperty(ref _selectedMstItem, value, () => SelectedMstItem, setRpt);
                }
            }
        }

        public void setRpt()
        {
            try
            {
                if (SelectedMstItem == null)
                {
                    return;
                }

                streamFile = new System.IO.MemoryStream(SelectedMstItem.RPT_FILE);

                //System.IO.FileStream stream = new System.IO.FileStream(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\testDocument2.xlsx", System.IO.FileMode.Open);
                SourceStream = new SpreadsheetDocumentSource(streamFile, DevExpress.Spreadsheet.DocumentFormat.Xlsx);
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }

        SpreadsheetDocumentSource _sourceStream;
        public SpreadsheetDocumentSource SourceStream
        {
            get
            {
                return _sourceStream;
            }
            set
            {
                if (value != null)
                {
                    SetProperty(ref _sourceStream, value, () => SourceStream);
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
            //PUR_DT = System.DateTime.Now.ToString("yyyy-MM-dd"), PUR_CLZ_FLG = "N", PUR_EMPE_ID = SystemProperties.USER_VO.USR_ID }
            masterDialog = new S1412MasterDialog(new SystemCodeVo() { CRE_DT = System.DateTime.Now.ToString("yyyy-MM-dd"), INP_ID = SystemProperties.USER_VO.USR_ID, INP_NM = SystemProperties.USER_VO.USR_N1ST_NM, CHNL_CD = SystemProperties.USER_VO.CHNL_CD, NTC_SEQ = 1 });
            masterDialog.Title = title + " - 추가";
            masterDialog.Owner = Application.Current.MainWindow;
            masterDialog.BorderEffect = BorderEffect.Default;
            masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            bool isDialog = (bool)masterDialog.ShowDialog();
            if (isDialog)
            {
                Refresh(masterDialog.ResultVo.TIT_DESC);

                //if (masterDialog.IsEdit == false)
                //{
                //    try
                //    {
                //        DXSplashScreen.Show<ProgressWindow>();
                //        Refresh();

                //        for (int x = SelectMstList.Count-1; x >= 0; x--)
                //        {
                //            if (masterDialog.ResultVo.TIT_DESC.Equals(SelectMstList[x].TIT_DESC))
                //            {
                //                SelectedMstItem = SelectMstList[x];
                //                break;
                //            }
                //        }
                //        DXSplashScreen.Close();
                //    }
                //    catch (System.Exception eLog)
                //    {
                //        DXSplashScreen.Close();
                //        WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]공지 사항 관리", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                //        return;
                //    }
                //}
            }
        }

        //public ICommand EditDialogCommand
        //{
        //    get
        //    {
        //        if (_editDialogCommand == null)
        //            _editDialogCommand = new DelegateCommand(EditContact);
        //        return _editDialogCommand;
        //    }
        //}

        [Command]
        public void EditContact()
        {
            if (SelectedMstItem == null)
            {
                return;
            }
            else if (SelectMstList.Count <= 0)
            {
                return;
            }

            masterDialog = new S1412MasterDialog(SelectedMstItem);
            masterDialog.Title = title + " - 수정";
            masterDialog.Owner = Application.Current.MainWindow;
            masterDialog.BorderEffect = BorderEffect.Default;
            masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            bool isDialog = (bool)masterDialog.ShowDialog();
            if (isDialog)
            {
                Refresh(masterDialog.ResultVo.TIT_DESC);
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
                //        WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]공지 사항 관리", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                //        return;
                //    }
                //}
            }
        }



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
            if (SelectedMstItem == null)
            {
                return;
            }
            SystemCodeVo delDao = SelectedMstItem;
            if (delDao != null)
            {
                MessageBoxResult result = WinUIMessageBox.Show("[" + delDao.RPT_CD + "/" + delDao.RPT_NM + "]" + " 정말로 삭제 하시겠습니까?", title, MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        //delDao.CRE_USR_ID = SystemProperties.USER_VO.CRE_USR_ID;
                        //delDao.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;

                        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s1412/mst/d", new StringContent(JsonConvert.SerializeObject(delDao), System.Text.Encoding.UTF8, "application/json")))
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

                                //
                                SourceStream = new SpreadsheetDocumentSource(new System.IO.MemoryStream(), DevExpress.Spreadsheet.DocumentFormat.Xlsx);
                                Refresh();

                                //성공
                                WinUIMessageBox.Show("삭제가 완료되었습니다.", title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                            }
                        }
                    }
                    catch (System.Exception eLog)
                    {
                        WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                        return;
                    }
                }
            }
        }


        //public ICommand PrintDialogCommand
        //{
        //    get
        //    {
        //        if (_printDialogCommand == null)
        //            _printDialogCommand = new DelegateCommand(PrintContact);
        //        return _printDialogCommand;
        //    }
        //}

        //public void PrintContact()
        //{
            //if (SelectedMstItem == null)
            //{
            //    return;
            //}
            //PurVo printDao = SelectedMstItem;
            //if (printDao != null)
            //{
            //    ObservableCollection<PurVo> allItems = new ObservableCollection<PurVo>(purClient.P4411SelectMstReport(printDao));
            //    if (allItems == null)
            //    {
            //        return;
            //    }
            //    else if (allItems.Count <= 0)
            //    {
            //        WinUIMessageBox.Show("데이터가 존재 하지 않습니다.", "[에러]발주서 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
            //        return;
            //    }
            //    else if (allItems.Count < 12)
            //    {
            //        for (int x = 1; x < 12; x++)
            //        {
            //            allItems.Insert(allItems.Count, new PurVo());
            //        }
            //    }
            //    allItems[allItems.Count - 1].QC_CD = allItems[0].QC_CD;
            //    allItems[allItems.Count - 1].PUR_ORD_NO = allItems[0].PUR_ORD_NO;
            //    ////

            //    P4411Report report = new P4411Report(allItems);
            //    report.Margins.Top = 0;
            //    report.Margins.Bottom = 0;
            //    report.Margins.Left = 40;
            //    report.Margins.Right = 0;
            //    report.Landscape = false;
            //    report.PrintingSystem.ShowPrintStatusDialog = true;
            //    report.PaperKind = System.Drawing.Printing.PaperKind.A4;
            //    //데모 시연 문서 표시 가능
            //    //report.Watermark.Text = "DAEGIL";
            //    //report.Watermark.TextDirection = DirectionMode.ForwardDiagonal;
            //    //report.Watermark.Font = new Font(report.Watermark.Font.FontFamily, 40);
            //    ////report.Watermark.ForeColor = Color.DodgerBlue;
            //    //report.Watermark.ForeColor = Color.PaleTurquoise;
            //    //report.Watermark.TextTransparency = 150;
            //    //report.Watermark.ShowBehind = false;
            //    //report.Watermark.PageRange = "1,3-5";


            //    XtraReportPreviewModel model = new XtraReportPreviewModel(report);
            //    DocumentPreviewWindow window = new DocumentPreviewWindow() { Model = model };
            //    report.CreateDocument(true);
            //    window.Title = "발주서 [" + printDao.PUR_ORD_NO + "] ";
            //    window.ShowDialog();

            //}
        //}

    }
}
