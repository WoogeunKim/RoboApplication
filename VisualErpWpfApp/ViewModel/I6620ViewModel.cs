using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.Printing;
using DevExpress.Xpf.WindowsUI;
using DevExpress.XtraPrinting.Drawing;
using ModelsLibrary.Code;
using ModelsLibrary.Inv;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Windows;
using AquilaErpWpfApp3.Util;
using AquilaErpWpfApp3.View.INV.Report;
using DevExpress.Xpf.Core;

namespace AquilaErpWpfApp3.ViewModel
{
    public sealed class I6620ViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private string title = "재고장";

        //private static SaleOrderServiceClient saleOrderClient = SystemProperties.SaleOrderClient;
        //private static ItemCodeServiceClient itemClient = SystemProperties.ItemClient;
        //private static InvServiceClient invClient = SystemProperties.InvClient;


        private IList<InvVo> selectedMstList = new List<InvVo>();

        //private ObservableCollection<PurVo> selectedDtlList = new ObservableCollection<PurVo>();
        private IList<InvVo> selectedDtlList = new List<InvVo>();

        //private S2211MasterDialog masterDialog;
        //private S2211DetailDialog detailDialog;

        //private S2211DetailExcelDialog excelDialog;

        //private S2211NextMonthDialog masterNextMonthDialog;


        //private S2211DetailQuickDialog detailQuickDialog;

        //private string[,] _loadSave;

        ////Menu Dialog
        //private ICommand _searchDialogCommand;

        //private ICommand _I6210DialogCommand;
        //private DocumentPanel panel;
        //private ICommand _newDialogCommand;
        //private ICommand _editDialogCommand;
        //private ICommand _CO_DialogCommand;
        //private ICommand reportDialogCommand;

        //private ICommand _delDialogCommand;
        //private ICommand _excelDialogCommand;

        //private ICommand _copyDialogCommand;

        //private ICommand _searchDetailDialogCommand;
        //private ICommand _newDetailDialogCommand;
        ////private ICommand _editDetailDialogCommand;
        //private ICommand _delDetailDialogCommand;

        //private ICommand _nextMonthDialogCommand;

        //private ICommand reportDialogCommand;

        ////private ICommand _revListDetailDialogCommand;
        ////private ICommand _revNewDetailDialogCommand;

        //private P41MasterDialog masterDialog;
        //private P41DetailDialog detailDialog;
        ////private A21JobItemRevDialog jobItemRevDialog;

        //private P41ReportDialog reportDialog;

        public I6620ViewModel() 
        {
            StartDt = System.DateTime.Now;
            EndDt = System.DateTime.Now;

            SYSTEM_CODE_VO();

            //구분
            //_ItmGrpClssMap = SystemProperties.SYSTEM_CODE_MAP("L-001");
            //ItmGrpClssList = SystemProperties.SYSTEM_CODE_VO("L-001");
            ////M_ITM_GRP_CLSS_NM = ItmGrpClssList[0];
            //TXT_ITM_GRP_CLSS_NM = "볼트";

            //ItmGrpClssList = SystemProperties.SYSTEM_CODE_VO("L-001");
            //ItmGrpClssList.Insert(0, new CodeDao() { CLSS_CD = null, CLSS_DESC = "전체" });
            //_ItmGrpClssMap = SystemProperties.SYSTEM_CODE_MAP("L-001");
            ////GrpClssList.Insert(0, new CodeDao() { CLSS_CD = "", CLSS_DESC = "" });
            //if (ItmGrpClssList.Count > 0)
            //{
            //    //M_ITM_GRP_CLSS_CD = GrpClssList[0];
            //    TXT_ITM_GRP_CLSS_NM = ItmGrpClssList[0].CLSS_DESC;
            //}

            ////호칭
            //_HdMap = SystemProperties.SYSTEM_CODE_MAP("C-002");
            //HdList = SystemProperties.SYSTEM_CODE_VO("C-002");
            //HdList.Insert(0, new CodeDao() { CLSS_DESC = "전체" });
            //TXT_HD_LEN_NM = "전체";
            ////M_HD_LEN_NM = HdList[0];

            ////사업장
            //AreaList = SystemProperties.SYSTEM_CODE_VO("L-002");
            ////_AreaMap = SystemProperties.SYSTEM_CODE_MAP("L-002");
            //if (AreaList.Count > 0)
            //{
            //    TXT_SL_AREA_NM = SystemProperties.USER_VO.EMPE_PLC_CD;
            //    for (int x = 0; x < AreaList.Count; x++)
            //    {
            //        if (TXT_SL_AREA_NM.Equals(AreaList[x].CLSS_DESC))
            //        {
            //            M_SL_AREA_NM = AreaList[x];
            //            break;
            //        }
            //    }
            //}

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
                SearchDetail = null;
                SelectDtlList = null;

                DXSplashScreen.Show<ProgressWindow>();
                //using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("i6620/mst", new StringContent(JsonConvert.SerializeObject(new InvVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD, ITM_GRP_CLSS_CD = (TXT_ITM_GRP_CLSS_NM.CLSS_DESC.Equals("전체") ? null : TXT_ITM_GRP_CLSS_NM.CLSS_CD), DELT_FLG = "N" }), System.Text.Encoding.UTF8, "application/json")))
                //{
                //    if (response.IsSuccessStatusCode)
                //    {
                //        SelectMstList = JsonConvert.DeserializeObject<IEnumerable<InvVo>>(await response.Content.ReadAsStringAsync()).Cast<InvVo>().ToList();
                //    }

                //    //SelectMstList = itemClient.S2211SelectCodeItemGroupList(new ItemCodeVo() { ITM_GRP_CLSS_CD = (TXT_ITM_GRP_CLSS_NM.Equals("전체") ? null : _ItmGrpClssMap[TXT_ITM_GRP_CLSS_NM]) });
                //    ////

                //SelectMstDetail();




                Title = "[기간]" + (StartDt).ToString("yyyy-MM-dd") + ",    [구분]" + TXT_ITM_GRP_CLSS_NM.CLSS_DESC; //+ ",    [호칭]" + TXT_HD_LEN_NM.CLSS_DESC;

                InvVo _vo = new InvVo()
                {
                    ITM_GRP_CLSS_CD = TXT_ITM_GRP_CLSS_NM.CLSS_CD,
                    //N1ST_ITM_GRP_CD = SelectedMstItem.N1ST_ITM_GRP_CD,
                    //N2ND_ITM_GRP_CD = SelectedMstItem.N2ND_ITM_GRP_CD,
                    //HD_LEN_CD = TXT_HD_LEN_NM.CLSS_CD,
                    AREA_CD = SystemProperties.USER_VO.EMPE_PLC_NM,
                    CHNL_CD = SystemProperties.USER_VO.CHNL_CD
                    //SL_RLSE_DT = (StartDt).ToString("yyyy-MM-dd")
                };

                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("i6620/dtl", new StringContent(JsonConvert.SerializeObject(_vo), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        SelectDtlList = JsonConvert.DeserializeObject<IEnumerable<InvVo>>(await response.Content.ReadAsStringAsync()).Cast<InvVo>().ToList();
                    }

                    //DXSplashScreen.Show<ProgressWindow>();

                    //if (this._selectedMstItem == null)
                    //{
                    //    return;
                    //}

                    //SelectDtlList = itemClient.S2211SelectCodeItemDtlGroupList(new ItemCodeVo()
                    //{
                    //    ITM_GRP_CLSS_CD = _ItmGrpClssMap[TXT_ITM_GRP_CLSS_NM],
                    //    N1ST_ITM_GRP_CD = SelectedMstItem.N1ST_ITM_GRP_CD,
                    //    N2ND_ITM_GRP_CD = SelectedMstItem.N2ND_ITM_GRP_CD,
                    //    HD_LEN_CD = (TXT_HD_LEN_NM.Equals("전체") ? null : _HdMap[TXT_HD_LEN_NM]),
                    //    AREA_CD = SystemProperties.USER_VO.EMPE_PLC_NM,
                    //    SL_RLSE_DT = (StartDt).ToString("yyyy-MM-dd")
                    //});


                    ////SelectDtlList = new ObservableCollection<PurVo>(purClient.P4401SelectDtlList(new PurVo() { ITM_CD = this._selectedMstItem.}));
                    ////SelectDtlList = saleOrderClient.S2211SelectDtlList(SelectedMstItem);
                    //// //
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

                    DXSplashScreen.Close();
                }





                //    if (SelectMstList.Count >= 1)
                //    {
                //        isM_UPDATE = true;
                //        isM_DELETE = true;

                //        SelectedMstItem = SelectMstList[0];
                //    }
                //    else
                //    {
                //        SearchDetail = null;
                //        SelectDtlList = null;

                //        isM_UPDATE = false;
                //        isM_DELETE = false;
                //        //
                //        isD_UPDATE = false;
                //        isD_DELETE = false;
                //    }
                //    //DXSplashScreen.Close();
                //}
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


        //public IList<InvVo> SelectMstList
        //{
        //    get { return selectedMstList; }
        //    set { SetProperty(ref selectedMstList, value, () => SelectMstList); }
        //}

        //InvVo _selectedMstItem;
        //public InvVo SelectedMstItem
        //{
        //    get
        //    {
        //        return _selectedMstItem;
        //    }
        //    set
        //    {
        //        if (value != null)
        //        {
        //            SetProperty(ref _selectedMstItem, value, () => SelectedMstItem, SelectMstDetail);
        //        }
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

        //구분
        //private Dictionary<string, string> _ItmGrpClssMap = new Dictionary<string, string>();
        private IList<SystemCodeVo> _ItmGrpClssList = new List<SystemCodeVo>();
        public IList<SystemCodeVo> ItmGrpClssList
        {
            get { return _ItmGrpClssList; }
            set { SetProperty(ref _ItmGrpClssList, value, () => ItmGrpClssList); }
        }

        //구분
        private SystemCodeVo _TXT_ITM_GRP_CLSS_NM;
        public SystemCodeVo TXT_ITM_GRP_CLSS_NM
        {
            get { return _TXT_ITM_GRP_CLSS_NM; }
            set { SetProperty(ref _TXT_ITM_GRP_CLSS_NM, value, () => TXT_ITM_GRP_CLSS_NM); }
        }

        ////구분
        //private CodeDao _M_ITM_GRP_CLSS_NM;
        //public CodeDao M_ITM_GRP_CLSS_NM
        //{
        //    get { return _M_ITM_GRP_CLSS_NM; }
        //    set { SetProperty(ref _M_ITM_GRP_CLSS_NM, value, () => M_ITM_GRP_CLSS_NM); }
        //}


        //호칭
        //private Dictionary<string, string> _HdMap = new Dictionary<string, string>();
        private IList<SystemCodeVo> _HdList = new List<SystemCodeVo>();
        public IList<SystemCodeVo> HdList
        {
            get { return _HdList; }
            set { SetProperty(ref _HdList, value, () => HdList); }
        }

        //호칭
        private SystemCodeVo _TXT_HD_LEN_NM;
        public SystemCodeVo TXT_HD_LEN_NM
        {
            get { return _TXT_HD_LEN_NM; }
            set { SetProperty(ref _TXT_HD_LEN_NM, value, () => TXT_HD_LEN_NM); }
        }

        ////호칭
        //private CodeDao _M_HD_LEN_NM;
        //public CodeDao M_HD_LEN_NM
        //{
        //    get { return _M_HD_LEN_NM; }
        //    set { SetProperty(ref _M_HD_LEN_NM, value, () => M_HD_LEN_NM); }
        //}


        //public ICommand EditDialogCommand
        //{
        //    get
        //    {
        //        if (_editDialogCommand == null)
        //            _editDialogCommand = new DelegateCommand(EditContact);
        //        return _editDialogCommand;
        //    }
        //}

        public void EditContact()
        {
            //try
            //{

            //    DXSplashScreen.Show<ProgressWindow>();

            //    if (this._selectedMstItem == null)
            //    {
            //        return;
            //    }

            //    SelectDtlList = itemClient.S2211SelectCodeItemDtlGroupList2(new ItemCodeVo()
            //    {
            //        ITM_GRP_CLSS_CD = (TXT_ITM_GRP_CLSS_NM.Equals("전체") ? null :_ItmGrpClssMap[TXT_ITM_GRP_CLSS_NM]),
            //        N1ST_ITM_GRP_CD = SelectedMstItem.N1ST_ITM_GRP_CD,
            //        N2ND_ITM_GRP_CD = SelectedMstItem.N2ND_ITM_GRP_CD,
            //        HD_LEN_CD = (TXT_HD_LEN_NM.Equals("전체") ? null : _HdMap[TXT_HD_LEN_NM]),
            //        AREA_CD = SystemProperties.USER_VO.EMPE_PLC_NM,
            //        SL_RLSE_DT = (StartDt).ToString("yyyy-MM-dd")
            //    });


            //    //SelectDtlList = new ObservableCollection<PurVo>(purClient.P4401SelectDtlList(new PurVo() { ITM_CD = this._selectedMstItem.}));
            //    //SelectDtlList = saleOrderClient.S2211SelectDtlList(SelectedMstItem);
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

            //    DXSplashScreen.Close();

            //}
            //catch (System.Exception eLog)
            //{
            //    DXSplashScreen.Close();
            //    WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
            //    return;
            //}
        }



        private async void SelectMstDetail()
        {
            try
            {
                //if (SelectedMstItem == null)
                //{
                //    return;
                //}

                InvVo _vo = new InvVo()
                {
                    ITM_GRP_CLSS_CD = TXT_ITM_GRP_CLSS_NM.CLSS_CD,
                    //N1ST_ITM_GRP_CD = SelectedMstItem.N1ST_ITM_GRP_CD,
                    //N2ND_ITM_GRP_CD = SelectedMstItem.N2ND_ITM_GRP_CD,
                    //HD_LEN_CD = TXT_HD_LEN_NM.CLSS_CD,
                    AREA_CD = SystemProperties.USER_VO.EMPE_PLC_NM,
                    CHNL_CD = SystemProperties.USER_VO.CHNL_CD
                    //SL_RLSE_DT = (StartDt).ToString("yyyy-MM-dd")
                };

                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("i6620/dtl", new StringContent(JsonConvert.SerializeObject(_vo), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        SelectDtlList = JsonConvert.DeserializeObject<IEnumerable<InvVo>>(await response.Content.ReadAsStringAsync()).Cast<InvVo>().ToList();
                    }

                    //DXSplashScreen.Show<ProgressWindow>();

                    //if (this._selectedMstItem == null)
                    //{
                    //    return;
                    //}

                    //SelectDtlList = itemClient.S2211SelectCodeItemDtlGroupList(new ItemCodeVo()
                    //{
                    //    ITM_GRP_CLSS_CD = _ItmGrpClssMap[TXT_ITM_GRP_CLSS_NM],
                    //    N1ST_ITM_GRP_CD = SelectedMstItem.N1ST_ITM_GRP_CD,
                    //    N2ND_ITM_GRP_CD = SelectedMstItem.N2ND_ITM_GRP_CD,
                    //    HD_LEN_CD = (TXT_HD_LEN_NM.Equals("전체") ? null : _HdMap[TXT_HD_LEN_NM]),
                    //    AREA_CD = SystemProperties.USER_VO.EMPE_PLC_NM,
                    //    SL_RLSE_DT = (StartDt).ToString("yyyy-MM-dd")
                    //});


                    ////SelectDtlList = new ObservableCollection<PurVo>(purClient.P4401SelectDtlList(new PurVo() { ITM_CD = this._selectedMstItem.}));
                    ////SelectDtlList = saleOrderClient.S2211SelectDtlList(SelectedMstItem);
                    //// //
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

                    //DXSplashScreen.Close();
                }
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
        public IList<InvVo> SelectDtlList
        {
            get { return selectedDtlList; }
            set { SetProperty(ref selectedDtlList, value, () => SelectDtlList); }
        }

        InvVo _searchDetail;
        public InvVo SearchDetail
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


        //public ICommand I6210DialogCommand
        //{
        //    get
        //    {
        //        if (_I6210DialogCommand == null)
        //            _I6210DialogCommand = new DelegateCommand(CALL_I6210);
        //        return _I6210DialogCommand;
        //    }
        //}


        //private void CALL_I6210()
        //{
        //    if (SearchDetail == null)
        //    {
        //        return;
        //    }

        //    SystemProperties.SEEK_I6620 = SearchDetail;

        //    SystemProperties.DOCK_MANGER.ClosedPanels.Clear();
        //    BaseLayoutItem item = SystemProperties.DOCK_MANGER.GetItem("재고일자별원장");
        //    if (item != null)
        //    {
        //        //SystemProperties.DOCK_MANGER.ActiveMDIItem = item;
        //        //item.AllowSelection = true;
        //        //item.IsActive = true;
        //        //return;
        //        item.Closed = true;
        //    }


        //    this.panel = SystemProperties.DOCK_MANGER.DockController.AddDocumentPanel(SystemProperties.DOCK_GROUP, new Uri(SystemProperties.PROGRAM_NAME + "View/INV/I6210.xaml", UriKind.Relative));
        //    this.panel.Name = "재고일자별원장"; //[" + SearchDetail.ITM_CD + "]" +
        //    this.panel.ToolTip = "재고일자별원장 - " + SearchDetail.ITM_NM + "/" + SearchDetail.ITM_SZ_NM;
        //    this.panel.Caption = "재고일자별원장 - " + SearchDetail.ITM_NM + "/" + SearchDetail.ITM_SZ_NM;
        //    this.panel.AllowContextMenu = false;
        //    //panel.CaptionImage = DecodePhoto(dao.IMAGE);
        //    //panel.CaptionImage = new BitmapImage(new Uri(SystemProperties.PROGRAM_NAME + "Images/Menu/" + dao.PGM_IMG_NM + ".png", UriKind.Relative));

        //    SystemProperties.DOCK_MANGER.Activate(panel);


        //}


        //public ICommand CO_DialogCommand
        //{
        //    get
        //    {
        //        if (_CO_DialogCommand == null)
        //            _CO_DialogCommand = new DelegateCommand(I6620Contact);
        //        return _CO_DialogCommand;
        //    }
        //}


        //private void I6620Contact()
        //{
        //    if (SearchDetail != null) {
        //        I6620DetailDialog detailDialog = new I6620DetailDialog(SearchDetail);
        //    detailDialog.Title = "재고장 거래처 조회";
        //    detailDialog.Owner = Application.Current.MainWindow;
        //    detailDialog.BorderEffect = BorderEffect.Default;
        //    ////masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
        //    ////masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
        //    //detailDialog.IsEdit = false;
        //    bool isDialog = (bool)detailDialog.ShowDialog();
        //    if (isDialog)
        //    {
        //        ////if (detailDialog.IsEdit == false)
        //        ////{
        //        try
        //        {
        //        //    DXSplashScreen.Show<ProgressWindow>();
        //        //    Refresh();

        //        //    for (int x = 0; x < SelectMstList.Count; x++)
        //        //    {
        //        //        if (detailDialog.masterDao.SL_RLSE_NO.Equals(SelectMstList[x].SL_RLSE_NO))
        //        //        {
        //        //            SelectedMstItem = SelectMstList[x];
        //        //            break;
        //        //        }
        //        //    }

        //        //    JobVo resultVo = saleOrderClient.ProcS2211(new JobVo() { SL_RLSE_NO = detailQuickDialog.masterDao.SL_RLSE_NO, CRE_USR_ID = SystemProperties.USER, UPD_USR_ID = SystemProperties.USER });
        //        //    if (!resultVo.isSuccess)
        //        //    {
        //        //        //실패
        //        //        WinUIMessageBox.Show(resultVo.Message, "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
        //        //        return;


        //        //    SelectMstDetail();

        //        //    DXSplashScreen.Close();
        //        }
        //        catch (System.Exception eLog)
        //        {
        //            DXSplashScreen.Close();
        //            WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
        //            return;
        //        }
        //      }
        //    }
        //}

        //private ICommand _newDialogCommand;
        //private I6620ExcelDialog masterDialog;
        //public ICommand ExcelDialogCommand
        //{
        //    get
        //    {
        //        if (_newDialogCommand == null)
        //            _newDialogCommand = new DelegateCommand(NewContact);
        //        return _newDialogCommand;
        //    }
        //}
        //I6620ExcelDialog EXDialog=new I6620ExcelDialog();
        //public void NewContact()
        //{
        //    EXDialog.ViewTableDtl.PrintAutoWidth = true;

        //    using (PrintableControlLink prtLink = new PrintableControlLink(EXDialog.ViewTableDtl))
        //    {
        //        //prtLink.PageHeaderData = "인쇄 일자 : " + Convert.ToDateTime(this.txt_stDate.Text).ToString("yyyy-MM-dd");
        //       // prtLink.PageHeaderTemplate = Resources["PageHeader"] as DataTemplate;
        //        prtLink.PageFooterData = "" + System.DateTime.Now.ToString("yyyy-MM-dd hh:mm");
        //        //prtLink.PageFooterTemplate = Resources["PageFooter"] as DataTemplate;

        //        prtLink.Margins.Top = 8;
        //        prtLink.Margins.Bottom = 8;
        //        prtLink.Margins.Left = 5;
        //        prtLink.Margins.Right = 5;
        //        prtLink.DocumentName = " Print";

        //        //prtLink.PrintingSystem.Watermark.Text = "한양화스너공업(주)";
        //        prtLink.PrintingSystem.Watermark.TextDirection = DirectionMode.ForwardDiagonal;
        //        prtLink.PrintingSystem.Watermark.Font = new Font(prtLink.PrintingSystem.Watermark.Font.FontFamily, 40);
        //        prtLink.PrintingSystem.Watermark.ForeColor = System.Drawing.Color.PaleTurquoise;
        //        prtLink.PrintingSystem.Watermark.TextTransparency = 150;

        //        prtLink.Landscape = true;
        //        prtLink.PrintingSystem.ShowPrintStatusDialog = true;

        //        prtLink.PaperKind = System.Drawing.Printing.PaperKind.A4;
        //        prtLink.CreateDocument(true);
        //        prtLink.ShowPrintPreviewDialog(Application.Current.MainWindow);
        //    }
        //    //masterDialog = new I6620ExcelDialog();
        //    //masterDialog.Title = "현재고";
        //    //masterDialog.Owner = Application.Current.MainWindow;
        //    //masterDialog.BorderEffect = BorderEffect.Default;
        //    //bool isDialog = (bool)masterDialog.ShowDialog();
        //    //if (isDialog)
        //    //{
        //    //    {
        //    //        try
        //    //        {

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
        //public ICommand ReportDialogCommand
        //{
        //    get
        //    {
        //        if (reportDialogCommand == null)
        //            reportDialogCommand = new DelegateCommand(ReportContact);
        //        return reportDialogCommand;
        //    }
        //}
        [Command]
        public async void Report()
        {

            //if (SelectedMstItem == null)
            //{
            //    return;
            //}

            IList<InvVo> _reportList = new List<InvVo>();

            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("i5521/report", new StringContent(JsonConvert.SerializeObject(new InvVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD, AREA_CD = SystemProperties.USER_VO.EMPE_PLC_NM, ITM_GRP_CLSS_CD = TXT_ITM_GRP_CLSS_NM.CLSS_CD, /*N1ST_ITM_GRP_CD = SelectedMstItem.N1ST_ITM_GRP_CD, N2ND_ITM_GRP_CD = SelectedMstItem.N2ND_ITM_GRP_CD*/ }), System.Text.Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    _reportList = JsonConvert.DeserializeObject<IEnumerable<InvVo>>(await response.Content.ReadAsStringAsync()).Cast<InvVo>().ToList();
                }

                //_reportList = invClient.I5521SelectStkSilsa(new InvVo()
                //{
                //    AREA_CD = SystemProperties.USER_VO.EMPE_PLC_NM
                //,
                //    ITM_GRP_CLSS_CD = (TXT_ITM_GRP_CLSS_NM.Equals("전체") ? null : _ItmGrpClssMap[TXT_ITM_GRP_CLSS_NM])
                //,
                //    N1ST_ITM_GRP_CD = SelectedMstItem.N1ST_ITM_GRP_CD
                //,
                //    N2ND_ITM_GRP_CD = SelectedMstItem.N2ND_ITM_GRP_CD
                //});

                // 재고조사용
                I5521Report report = new I5521Report(_reportList);
                report.Margins.Top = 2;
                report.Margins.Bottom = 0;
                report.Margins.Left = 1;
                report.Margins.Right = 1;
                report.Landscape = true;
                report.PrintingSystem.ShowPrintStatusDialog = true;
                report.PaperKind = System.Drawing.Printing.PaperKind.A4;
                //데모 시연 문서 표시 가능
                //report.Watermark.Text = "한양화스너공업(주)";
                report.Watermark.TextDirection = DirectionMode.ForwardDiagonal;
                report.Watermark.Font = new Font(report.Watermark.Font.FontFamily, 40);
                ////report.Watermark.ForeColor = Color.DodgerBlue;
                report.Watermark.ForeColor = System.Drawing.Color.PaleTurquoise;
                report.Watermark.TextTransparency = 180;
                report.Watermark.ShowBehind = false;
                //report.Watermark.PageRange = "1,3-5";

                var window = new DocumentPreviewWindow();
                window.PreviewControl.DocumentSource = report;
                report.CreateDocument(true);
                window.Title = "연말 재고조사용";
                window.Owner = Application.Current.MainWindow;
                window.ShowDialog();
            }

        }


        public async void SYSTEM_CODE_VO()
        {

            //ItmGrpClssList = SystemProperties.SYSTEM_CODE_VO("L-001");
            //ItmGrpClssList.Insert(0, new CodeDao() { CLSS_CD = null, CLSS_DESC = "전체" });
            //_ItmGrpClssMap = SystemProperties.SYSTEM_CODE_MAP("L-001");
            ////GrpClssList.Insert(0, new CodeDao() { CLSS_CD = "", CLSS_DESC = "" });
            //if (ItmGrpClssList.Count > 0)
            //{
            //    //M_ITM_GRP_CLSS_CD = GrpClssList[0];
            //    TXT_ITM_GRP_CLSS_NM = ItmGrpClssList[0].CLSS_DESC;
            //}

            ////호칭
            //_HdMap = SystemProperties.SYSTEM_CODE_MAP("C-002");
            //HdList = SystemProperties.SYSTEM_CODE_VO("C-002");
            //HdList.Insert(0, new CodeDao() { CLSS_DESC = "전체" });
            //TXT_HD_LEN_NM = "전체";
            ////M_HD_LEN_NM = HdList[0];

            //구분
            //AreaList = SystemProperties.SYSTEM_CODE_VO("L-002");
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "L-001"))
            {
                if (response.IsSuccessStatusCode)
                {
                    ItmGrpClssList = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                    if (ItmGrpClssList.Count > 0)
                    {
                        ItmGrpClssList.Insert(0, new SystemCodeVo() { CLSS_CD = null, CLSS_DESC = "전체" });
                        TXT_ITM_GRP_CLSS_NM = ItmGrpClssList[3];
                    }
                }
            }

            //호칭
            //AreaList = SystemProperties.SYSTEM_CODE_VO("C-002");
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "C-001"))
            {
                if (response.IsSuccessStatusCode)
                {
                    HdList = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                    if (HdList.Count > 0)
                    {
                        HdList.Insert(0, new SystemCodeVo() { CLSS_CD = null, CLSS_DESC = "전체" });
                        TXT_HD_LEN_NM = HdList[0];
                    }
                }
            }

            //Refresh();
        }
    }
}
