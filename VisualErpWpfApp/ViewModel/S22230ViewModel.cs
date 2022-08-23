using AquilaErpWpfApp3.Util;
using AquilaErpWpfApp3.View.PUR.Report;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Printing;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Code;
using ModelsLibrary.Pur;
using ModelsLibrary.Sale;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Windows;

namespace AquilaErpWpfApp3.ViewModel
{
    public sealed class S22230ViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private string title = "(화성판매)현품표출력";

        //private static SaleOrderServiceClient saleOrderClient = SystemProperties.SaleOrderClient;
        private IList<SaleVo> selectedMstList = new List<SaleVo>();

        //private ObservableCollection<PurVo> selectedDtlList = new ObservableCollection<PurVo>();
        //private IList<JobVo> selectedDtlList = new List<JobVo>();

        //private S2221MasterDialog masterDialog;

        ////Menu Dialog
        //private ICommand _searchDialogCommand;
        //private ICommand _newDialogCommand;
        //private ICommand _editDetailDialogCommand;
        //private ICommand _delDialogCommand;

        public S22230ViewModel() 
        {
            StartDt = System.DateTime.Now;
            EndDt = System.DateTime.Now;

            SYSTEM_CODE_VO();
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
                DXSplashScreen.Show<ProgressWindow>();
                //SaleVo _param = new SaleVo();
                //_param.FM_DT = (StartDt).ToString("yyyy-MM-dd");
                //_param.TO_DT = (EndDt).ToString("yyyy-MM-dd");

                ////사업장
                //_param.AREA_CD = M_SL_AREA_NM.CLSS_CD;
                //_param.AREA_NM = M_SL_AREA_NM.CLSS_DESC;

                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s22230", new StringContent(JsonConvert.SerializeObject(new SaleVo() { FM_DT = (StartDt).ToString("yyyy-MM-dd"), TO_DT = (EndDt).ToString("yyyy-MM-dd"), AREA_CD = M_SL_AREA_NM.CLSS_CD, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectMstList = JsonConvert.DeserializeObject<IEnumerable<SaleVo>>(await response.Content.ReadAsStringAsync()).Cast<SaleVo>().ToList();
                    }
                    ////
                    //SelectMstList = saleOrderClient.S2227SelectMstList(_param);

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

        [Command]
        public async void Proc()
        {
            try
            {
                if (SelectedMstItem == null)
                {
                    return;
                }

                MessageBoxResult result = WinUIMessageBox.Show("[" + SelectedMstItem.SCM_RLSE_NO + " / " + SelectedMstItem.PUR_ORD_NO + " / " + SelectedMstItem.PUR_ORD_SEQ + "] 정말로 출고 하시겠습니까?", title, MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    DXSplashScreen.Show<ProgressWindow>();

                    SelectedMstItem.CRE_USR_ID = SystemProperties.USER_VO.USR_ID;
                    SelectedMstItem.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;

                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s22230/proc", new StringContent(JsonConvert.SerializeObject(SelectedMstItem), System.Text.Encoding.UTF8, "application/json")))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            //this.SelectMstList = JsonConvert.DeserializeObject<IEnumerable<SaleVo>>(await response.Content.ReadAsStringAsync()).Cast<SaleVo>().ToList();

                            //Refresh();
                        }
                    }

                   DXSplashScreen.Close();
                   Refresh();
                   WinUIMessageBox.Show("완료되었습니다.", title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                }
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

        [Command]
        public async void BarCode()
        {
            try
            {
                if (SelectedMstItem == null)
                {
                    return;
                }

                //바코드 출력
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s22230/barcode", new StringContent(JsonConvert.SerializeObject(SelectedMstItem), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        List<PurVo> _barCodeList = JsonConvert.DeserializeObject<IEnumerable<PurVo>>(await response.Content.ReadAsStringAsync()).Cast<PurVo>().ToList();


                        P441103TagReport report = new P441103TagReport(_barCodeList);
                        report.Margins.Top = 0;
                        report.Margins.Bottom = 0;
                        report.Margins.Left = 20;
                        report.Margins.Right = 0;
                        report.Landscape = true;
                        report.PrintingSystem.ShowPrintStatusDialog = true;
                        report.PaperKind = System.Drawing.Printing.PaperKind.A5;

                        report.Watermark.Text = Properties.Settings.Default.SettingCompany;
                        report.Watermark.TextDirection = DevExpress.XtraPrinting.Drawing.DirectionMode.ForwardDiagonal;
                        report.Watermark.Font = new System.Drawing.Font(report.PrintingSystem.Watermark.Font.FontFamily, 40);
                        report.Watermark.ForeColor = System.Drawing.Color.PaleTurquoise;
                        report.Watermark.TextTransparency = 150;

                        var window = new DocumentPreviewWindow();
                        window.PreviewControl.DocumentSource = report;
                        report.CreateDocument(true);
                        window.Title = "현품표(A5) [" + SelectedMstItem.PUR_ORD_NO + "] ";
                        window.Owner = Application.Current.MainWindow;
                        window.ShowDialog();
                    }
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
            set { SetProperty(ref _M_SL_AREA_NM, value, () => M_SL_AREA_NM/*, RefreshCoNm*/); }
        }

        ////사업장
        //private CodeDao _M_SL_AREA_NM;
        //public CodeDao M_SL_AREA_NM
        //{
        //    get { return _M_SL_AREA_NM; }
        //    set { SetProperty(ref _M_SL_AREA_NM, value, () => M_SL_AREA_NM); }
        //}

        private void SelectMstDetail()
        {
            this.isD_UPDATE = false;
            if (string.IsNullOrEmpty(SelectedMstItem.SL_RLSE_DT))
            {
                this.isD_UPDATE = true;
            }
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
        //    masterDialog = new S2221MasterDialog(new JobVo() { AREA_NM = SystemProperties.USER_VO.EMPE_PLC_CD });
        //    masterDialog.Title = title + " - 추가";
        //    masterDialog.Owner = Application.Current.MainWindow;
        //    masterDialog.BorderEffect = BorderEffect.Default;
        //    //masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
        //    //masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
        //    bool isDialog = (bool)masterDialog.ShowDialog();
        //    if (isDialog)
        //    {
        //        if (masterDialog.IsEdit == false)
        //        {
        //            try
        //            {
        //                DXSplashScreen.Show<ProgressWindow>();
        //                Refresh();
        //                DXSplashScreen.Close();

        //                for (int x = 0; x < SelectMstList.Count; x++)
        //                {
        //                    if ((masterDialog.CLT_BIL_NO).Equals(SelectMstList[x].CLT_BIL_NO))
        //                    {
        //                        SelectedMstItem = SelectMstList[x];
        //                        return;
        //                    }
        //                }

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

        //    masterDialog = new S2221MasterDialog(SelectedMstItem);
        //    masterDialog.Title = title + " - 수정(" + SelectedMstItem.CLT_BIL_NO + ")";
        //    masterDialog.Owner = Application.Current.MainWindow;
        //    masterDialog.BorderEffect = BorderEffect.Default;
        //    //masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
        //    //masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
        //    bool isDialog = (bool)masterDialog.ShowDialog();
        //    if (isDialog)
        //    {
        //        if (masterDialog.IsEdit == false)
        //        {
        //            try
        //            {
        //                DXSplashScreen.Show<ProgressWindow>();
        //                Refresh();
        //                DXSplashScreen.Close();
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
            Title = "[기간]" + (StartDt).ToString("yyyy-MM-dd") + "~" + (EndDt).ToString("yyyy-MM-dd") + ", [사업장]" + M_SL_AREA_NM.CLSS_DESC;
        }

        public async void SYSTEM_CODE_VO()
        {
            //사업장
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
        }

    }
}
