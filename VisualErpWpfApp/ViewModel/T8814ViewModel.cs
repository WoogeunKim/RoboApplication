using AquilaErpWpfApp3.Util;
using AquilaErpWpfApp3.Utillity;
using AquilaErpWpfApp3.View.TEC.Dialog;
using AquilaErpWpfApp3.View.TEC.Report;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using DevExpress.XtraReports.UI;
using ModelsLibrary.Code;
using ModelsLibrary.Tec;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Windows;

namespace AquilaErpWpfApp3.ViewModel
{
    public sealed class T8814ViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private string _title = "벌크 품질검사";

        //private static InvServiceClient invClient = SystemProperties.InvClient;
        private IList<TecVo> selectedMstList = new List<TecVo>();
        private IList<TecVo> selectedMstItemsList = new List<TecVo>();

        private T8814MasterDialog detailDialog;

        //private ObservableCollection<PurVo> selectedDtlList = new ObservableCollection<PurVo>();
        //private IList<InvVo> selectedDtlList = new List<InvVo>();

        //private I6610DetailPurDialog detailPurDialog;
        //private I6610DetailImpDialog detailImpDialog;


        //private ICommand _newDetailPurDialogCommand;
        //private ICommand _newDetailImpDialogCommand;

        ////Menu Dialog
        //private ICommand _searchDialogCommand;
        //private ICommand _newDialogCommand;
        //private ICommand _editDialogCommand;
        //private ICommand _delDialogCommand;
        //private ICommand reportDialogCommand;

        //private ICommand _searchDetailDialogCommand;
        //private ICommand _newDetailDialogCommand;
        //private ICommand _editDetailDialogCommand;
        //private ICommand _delDetailDialogCommand;

        //private ICommand reportDialogCommand;

        ////private ICommand _revListDetailDialogCommand;
        ////private ICommand _revNewDetailDialogCommand;

        //private P41MasterDialog masterDialog;
        //private P41DetailDialog detailDialog;
        ////private A21JobItemRevDialog jobItemRevDialog;

        //private P41ReportDialog reportDialog;

        public T8814ViewModel() 
        {
            StartDt = System.DateTime.Now;
            EndDt = System.DateTime.Now;

            //IN_FLG = false;

            SYSTEM_CODE_VO();
           
        }

       [Command]
       public async void Refresh()
        {
            try
            {

                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("t8814/", new StringContent(JsonConvert.SerializeObject(new TecVo() {CHNL_CD = SystemProperties.USER_VO.CHNL_CD, FM_DT = (StartDt).ToString("yyyy-MM-dd"), TO_DT = (EndDt).ToString("yyyy-MM-dd"), AREA_CD = M_SL_AREA_NM.CLSS_CD, LOC_CD = M_SL_LOC_NM.CLSS_CD }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectMstList = JsonConvert.DeserializeObject<IEnumerable<TecVo>>(await response.Content.ReadAsStringAsync()).Cast<TecVo>().ToList();
                    }

                    //SelectMstList = invClient.I6610SelectMstList(new InvVo() { FM_DT = (StartDt).ToString("yyyy-MM-dd"), TO_DT = (EndDt).ToString("yyyy-MM-dd"), AREA_CD = (string.IsNullOrEmpty(TXT_SL_AREA_NM) ? null : _AreaMap[TXT_SL_AREA_NM]), GBN = (string.IsNullOrEmpty(M_CHECKD) ? "%" : M_CHECKD), LOC_CD = (string.IsNullOrEmpty(TXT_SL_LOC_NM) ? null : _LocMap[TXT_SL_LOC_NM]), IN_FLG = (IN_FLG == true ? "Y" : "N") });
                    ////+ ",   [입고여부]" + (IN_FLG == true ? "Y" : "N") 
                    Title = "[기간]" + (StartDt).ToString("yyyy-MM-dd") + "~" + (EndDt).ToString("yyyy-MM-dd") + ",   [사업장]" + M_SL_AREA_NM.CLSS_DESC + ",   [창고]" + M_SL_LOC_NM.CLSS_DESC;

                    if (SelectMstList.Count >= 1)
                    {
                        isM_UPDATE = true;
                        isM_DELETE = true;
                        //SelectedMstItem = SelectMstList[0];
                        //for (int x = 0; x < SelectMstList.Count; x++)
                        //{
                        //    SelectMstList[x].DTL_DATA = invClient.I6610SelectDtlList(SelectedMstItem);
                        //}
                    }
                    else
                    {
                        isM_UPDATE = false;
                        isM_DELETE = false;
                    }
                    //DXSplashScreen.Close();
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

        //창고
        //private Dictionary<string, string> _LocMap = new Dictionary<string, string>();
        private IList<SystemCodeVo> _LocCd = new List<SystemCodeVo>();
        public IList<SystemCodeVo> LocList
        {
            get { return _LocCd; }
            set { SetProperty(ref _LocCd, value, () => LocList); }
        }
        //창고
        //private CodeDao _M_SL_AREA_NM;
        //public CodeDao M_SL_AREA_NM
        //{
        //    get { return _M_SL_AREA_NM; }
        //    set { SetProperty(ref _M_SL_AREA_NM, value, () => M_SL_AREA_NM); }
        //}
        //창고 
        private SystemCodeVo _M_SL_LOC_NM;
        public SystemCodeVo M_SL_LOC_NM
        {
            get { return _M_SL_LOC_NM; }
            set { SetProperty(ref _M_SL_LOC_NM, value, () => M_SL_LOC_NM); }
        }

        ////구분
        //private bool? _M_CHECKD_ALL = true;
        //public bool? M_CHECKD_ALL
        //{
        //    get { return _M_CHECKD_ALL; }
        //    set { SetProperty(ref _M_CHECKD_ALL, value, () => M_CHECKD_ALL, SelectCheckdAll); }
        //}
        //private bool? _M_CHECKD_PO = false;
        //public bool? M_CHECKD_PO
        //{
        //    get { return _M_CHECKD_PO; }
        //    set { SetProperty(ref _M_CHECKD_PO, value, () => M_CHECKD_PO, SelectCheckdPo); }
        //}
        //private bool? _M_CHECKD_IV = false;
        //public bool? M_CHECKD_IV
        //{
        //    get { return _M_CHECKD_IV; }
        //    set { SetProperty(ref _M_CHECKD_IV, value, () => M_CHECKD_IV, SelectCheckdIv); }
        //}


        //private String M_CHECKD = string.Empty;
        //private String M_CHECKD_NAME = string.Empty;
        //private void SelectCheckdAll()
        //{
        //    if (M_CHECKD_ALL == true)
        //    {
        //        //M_CHECKD_ALL = true;
        //        M_CHECKD_PO = false;
        //        M_CHECKD_IV = false;
        //        //
        //        M_CHECKD = "";
        //        M_CHECKD_NAME = "전체";
        //    }

        //    if (M_CHECKD_ALL == false && M_CHECKD_PO == false && M_CHECKD_IV == false)
        //    {
        //        M_CHECKD_ALL = true;
        //        M_CHECKD_NAME = "전체";
        //    }
            
        //    //else
        //    //{
        //    //    M_CHECKD_PO = true;
        //    //    M_CHECKD = "PO";
        //    //    M_CHECKD_NAME = "국내";
        //    //}
        //}
        //private void SelectCheckdPo()
        //{
        //    if (M_CHECKD_PO == true)
        //    {
        //        //M_CHECKD_PO = true;
        //        M_CHECKD_ALL = false;
        //        M_CHECKD_IV = false;
        //        //
        //        M_CHECKD = "PO";
        //        M_CHECKD_NAME = "국내";
        //    }

        //    if (M_CHECKD_ALL == false && M_CHECKD_PO == false && M_CHECKD_IV == false)
        //    {
        //        M_CHECKD_ALL = true;
        //        M_CHECKD_NAME = "전체";
        //    }
        //    //else
        //    //{
        //    //    M_CHECKD_ALL = true;
        //    //    M_CHECKD = "";
        //    //    M_CHECKD_NAME = "전체";
        //    //}
        //}
        //private void SelectCheckdIv()
        //{
        //    if (M_CHECKD_IV == true)
        //    {
        //        //M_CHECKD_IV = true;
        //        M_CHECKD_PO = false;
        //        M_CHECKD_ALL = false;
        //        //
        //        M_CHECKD = "IV";
        //        M_CHECKD_NAME = "수입";
        //    }

        //    if (M_CHECKD_ALL == false && M_CHECKD_PO == false && M_CHECKD_IV == false)
        //    {
        //        M_CHECKD_ALL = true;
        //        M_CHECKD_NAME = "전체";
        //    }
        //    //else
        //    //{
        //    //    M_CHECKD_ALL = true;
        //    //    M_CHECKD = "";
        //    //    M_CHECKD_NAME = "전체";
        //    //}
        //}



        public IList<TecVo> SelectMstList
        {
            get { return selectedMstList; }
            set { SetProperty(ref selectedMstList, value, () => SelectMstList); }
        }

        TecVo _selectedMstItem;
        public TecVo SelectedMstItem
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

        public IList<TecVo> SelectedMstItems
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


        //출력 수
        private string _M_PRINT_CNT = "1";
        public string M_PRINT_CNT
        {
            get { return _M_PRINT_CNT; }
            set { SetProperty(ref _M_PRINT_CNT, value, () => M_PRINT_CNT); }
        }


        ////입고 여부
        //private bool? _IN_FLG = false;
        //public bool? IN_FLG
        //{
        //    get { return _IN_FLG; }
        //    set { SetProperty(ref _IN_FLG, value, () => IN_FLG); }
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

             //       SearchDetail = SelectDtlList[0];
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
            if (SelectedMstItems.Count > 0)
            {
              //  string _coNm = this.SelectedMstItems[0].LOT_NO;


                detailDialog = new T8814MasterDialog(this.SelectedMstItems);
                detailDialog.Title = this._title + " - [ 총 : " + SelectedMstItems.Count + " ]";
                detailDialog.Owner = Application.Current.MainWindow;
                detailDialog.BorderEffect = BorderEffect.Default;
                bool isDialog = (bool)detailDialog.ShowDialog();
                if (isDialog)
                {
                    Refresh();

                    //this.SelectedMstItem = this.SelectMstList.Where<TecVo>(w => w.CO_NM.Equals(_coNm)).FirstOrDefault<TecVo>();
                }
            }
            else
            {
                WinUIMessageBox.Show("품질 검사 대상이 존재 하지 않습니다", _title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                return;
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

        //public void EditContact()
        //{
        //    if (SelectedMstItem == null)
        //    {
        //        return;
        //    }
        //    else if (SelectMstList.Count <= 0)
        //    {
        //        return;
        //    }

        //    masterDialog = new I5513MasterDialog(SelectedMstItem);
        //    masterDialog.Title = this._title + " - 수정";
        //    masterDialog.Owner = Application.Current.MainWindow;
        //    masterDialog.BorderEffect = BorderEffect.Default;
        //    ////masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
        //    ////masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
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
        //                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
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
        [Command]
        public async void BarPrintConfig()
        {

            try
            {
                System.Windows.Controls.PrintDialog dialogue = new System.Windows.Controls.PrintDialog();
                if (dialogue.ShowDialog() == true)
                {
                    Properties.Settings.Default.str_PrnNm = dialogue.PrintQueue.FullName;
                    Properties.Settings.Default.Save();
                }
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[에러]" + _Title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }

        #region [2021-09-09]  바코드 속도 개선 작업 전
        //[Command]
        //public async void BarPrint()
        //{
        //    try
        //    {
        //        if (string.IsNullOrEmpty(Properties.Settings.Default.str_PrnNm))
        //        {
        //            System.Windows.Controls.PrintDialog dialogue = new System.Windows.Controls.PrintDialog();
        //            if (dialogue.ShowDialog() == true)
        //            {
        //                Properties.Settings.Default.str_PrnNm = dialogue.PrintQueue.FullName;
        //                Properties.Settings.Default.Save();
        //            }
        //        }



        //        if (SelectedMstItems.Count > 0)
        //        {
        //            BarPrint _bar = new BarPrint();

        //            MessageBoxResult result = WinUIMessageBox.Show("[ 총 :" + SelectedMstItems.Count + "개 / (" + M_PRINT_CNT + "장) ] 정말로 바코드 하시겠습니까?", "[바코드 - 출력]" + _title, MessageBoxButton.YesNo, MessageBoxImage.Question);
        //            if (result == MessageBoxResult.Yes)
        //            {
        //                DXSplashScreen.Show<ProgressWindow>();

        //                LabelDao labelDao = new LabelDao();
        //                foreach (TecVo _item in SelectedMstItems)
        //                {
        //                    //벌크코드
        //                    labelDao.A01 = _item.CUST_ITM_CD ?? " ";
        //                    //제품명 
        //                    labelDao.A02 = _item.ITM_NM ?? " ";
        //                    labelDao.A03 = _item.CUST_ITM_SZ_NM ?? " ";

        //                    //제조량
        //                    labelDao.A08 = _item.MIX_WEIH_VAL;
        //                    //수득량
        //                    labelDao.A09 = _item.ITM_QTY;

        //                    //labelDao.A04 = SelectedMasterItem.LOC_NM + " / " + SelectedMasterItem.N1ST_LOC_NM;
        //                    //labelDao.A05 = SelectedMasterItem.CO_NM;
        //                    //labelDao.A06 = SelectedMasterItem.CO_MAKE_NO ?? " ";
        //                    //제조일
        //                    labelDao.A07 = _item.MAKE_DT ?? " ";
        //                    //labelDao.A08 = SelectedMasterItem.INSP_NO ?? " ";
        //                    //사용기한
        //                    labelDao.A06 = _item.MTRL_EXP_DT ?? " ";

        //                    //참고사항                
        //                    labelDao.A12 = "";

        //                    //판정일
        //                    labelDao.A10 = _item.INSP_DT ?? " ";

        //                    //참고사항
        //                    labelDao.A11 = string.IsNullOrEmpty(_item.ITM_RMK) ? " " : _item.ITM_RMK;

        //                    //시험번호
        //                    labelDao.A13 = _item.INSP_NO ?? " ";
        //                    //바코드
        //                    //labelDao.A14 = (SelectedMasterItem.ITM_GRP_NM.Equals("원자재") ? SelectedMasterItem.MTRL_LOT_NO : SelectedMasterItem.CO_MAKE_NO);
        //                    //LOT
        //                    labelDao.A14 = _item.INP_LOT_NO ?? " ";

        //                    labelDao.A15 = _item.GBN;

        //                    //바코드
        //                    //labelDao.A05 = _item.LOT_NO;
        //                    labelDao.A05 = _item.STK_SER_NO;

        //                    //벌크 출력
        //                    _bar.Bulk_Godex(labelDao, Convert.ToInt16(M_PRINT_CNT));
        //                }


        //                DXSplashScreen.Close();
        //            }
        //        }
        //    }
        //    catch (System.Exception)
        //    {
        //        if (DXSplashScreen.IsActive)
        //        {
        //            DXSplashScreen.Close();
        //        }
        //        return;
        //    }
        //} 
        #endregion

        [Command]
        public async void BarPrint()
        {
            try
            {
                if (string.IsNullOrEmpty(Properties.Settings.Default.str_PrnNm))
                {
                    System.Windows.Controls.PrintDialog dialogue = new System.Windows.Controls.PrintDialog();
                    if (dialogue.ShowDialog() == true)
                    {
                        Properties.Settings.Default.str_PrnNm = dialogue.PrintQueue.FullName;
                        Properties.Settings.Default.Save();
                    }
                }


                if (SelectedMstItems.Count > 0)
                {
                    BarPrint _bar = new BarPrint();

                    MessageBoxResult result = WinUIMessageBox.Show("[ 총 :" + SelectedMstItems.Count + "개 / (" + M_PRINT_CNT + "장) ] 정말로 바코드 하시겠습니까?", "[바코드 - 출력]" + _title, MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        //DXSplashScreen.Show<ProgressWindow>();

                        //T8814BarCodeReport BarCodeReport;
                        //for (int x = 0; x < Convert.ToInt32(M_PRINT_CNT); x++)
                        //{
                        T8814BarCodeReport BarCodeReport = new T8814BarCodeReport(SelectedMstItems);
                            //var window = new DevExpress.Xpf.Printing.DocumentPreviewWindow();
                            //window.PreviewControl.DocumentSource = BarCodeReport;
                        ReportPrintTool printTool = new ReportPrintTool(BarCodeReport);

                        printTool.PrinterSettings.Copies = Convert.ToInt16(M_PRINT_CNT);
                        printTool.PrintingSystem.ShowPrintStatusDialog = false;
                        printTool.PrintingSystem.ShowMarginsWarning = false;
                      
                        printTool.Print(Properties.Settings.Default.str_PrnNm);
                        //}

                        //DSplashScreen.Close();
                    }
                }
            }
            catch (System.Exception)
            {
                //if (DXSplashScreen.IsActive)
                //{
                //    DXSplashScreen.Close();
                //}
                return;
            }
        }






        //public ICommand NewDtlPurDialogCommand
        //{
        //    get
        //    {
        //        if (_newDetailPurDialogCommand == null)
        //            _newDetailPurDialogCommand = new DelegateCommand(NewDtlPurContact);
        //        return _newDetailPurDialogCommand;
        //    }
        //}
        [Command]
        public void NewDtlPurContact()
        {
            try
            {
                //detailPurDialog = new I6610DetailPurDialog(new InvVo() {CHNL_CD = SystemProperties.USER_VO.CHNL_CD, FM_DT = (StartDt).ToString("yyyy-MM-dd"), TO_DT = (EndDt).ToString("yyyy-MM-dd") });
                //detailPurDialog.Title = "구매 입고 자재 관리 ";
                //detailPurDialog.Owner = Application.Current.MainWindow;
                //detailPurDialog.BorderEffect = BorderEffect.Default;
                //detailPurDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
                //detailPurDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
                //bool isDialog = (bool)detailPurDialog.ShowDialog();
                ////if (isDialog)
                //{
                //    Refresh();
                //}
            }
            catch (System.Exception)
            {
                return;
            }
        }



        //public ICommand NewDtlImpDialogCommand
        //{
        //    get
        //    {
        //        if (_newDetailImpDialogCommand == null)
        //            _newDetailImpDialogCommand = new DelegateCommand(NewDtlImpContact);
        //        return _newDetailImpDialogCommand;
        //    }
        //}

        //public void NewDtlImpContact()
        //{
        //    ////if (SelectedMstItem == null)
        //    ////{
        //    ////    return;
        //    ////}

            
        //    ////SelectedMstItem.FM_DT = (StartDt).ToString("yyyy-MM-dd");
        //    ////SelectedMstItem.TO_DT = (EndDt).ToString("yyyy-MM-dd");
        //    ////SelectedMstItem.GRP_ID = (string.IsNullOrEmpty(M_DEPT_DESC) ? null : _DeptMap[M_DEPT_DESC]);
        //    ////SelectedMstItem.GRP_NM = M_DEPT_DESC;


        //    ////if (string.IsNullOrEmpty(SelectedMstItem.GRP_ID))
        //    ////{
        //    ////    WinUIMessageBox.Show("[부서]전체를 선택 하실수 없습니다", "[조회 조건]품목 입고 관리", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
        //    ////    return;
        //    ////}
        //    //try
        //    //{
        //    //    detailImpDialog = new I6610DetailImpDialog(new InvVo() { FM_DT = (StartDt).ToString("yyyy-MM-dd"), TO_DT = (EndDt).ToString("yyyy-MM-dd") });
        //    //    detailImpDialog.Title = "수입 입고 자재 관리 ";
        //    //    detailImpDialog.Owner = Application.Current.MainWindow;
        //    //    detailImpDialog.BorderEffect = BorderEffect.Default;
        //    //    ////masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
        //    //    ////masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
        //    //    bool isDialog = (bool)detailImpDialog.ShowDialog();
        //    //    //if (isDialog)
        //    //    {
              
        //    //            DXSplashScreen.Show<ProgressWindow>();
        //    //            Refresh();
        //    //            DXSplashScreen.Close();
               
        //    //    }
        //    //}
        //    //catch (System.Exception)
        //    //{
        //    //      DXSplashScreen.Close();
        //    //      //WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
        //    //      return;
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
        public void ReportContact()
        {
            try
            {
                //int tmpIMP_ITM_AMT = 0;
                //int tmpITM_QTY = 0;
                //if (SelectedMstItem == null)
                //{
                //    return;
                //}

                //IList<InvVo> printDao = new List<InvVo>();
                //if (SelectedMstItem != null)
                //{
                //    if (SelectedMstItems.Count > 0)
                //    {
                //        for (int x = 0; x < SelectedMstItems.Count; x++)
                //        {
                //            SelectedMstItems[x].GRP_NM = "[가입고 일자 (From) " + (StartDt).ToString("yyyy-MM-dd HH:mm") + " ~ (To) " + (EndDt).ToString("yyyy-MM-dd HH:mm") + ", 사업장: " + SelectedMstItems[x].AREA_NM + "]";
                //            tmpIMP_ITM_AMT += Convert.ToInt32(SelectedMstItems[x].IMP_ITM_AMT);
                //            tmpITM_QTY += Convert.ToInt32(SelectedMstItems[x].ITM_QTY);

                //            SelectedMstItems[x].TMP_A_QTY = tmpIMP_ITM_AMT;
                //            SelectedMstItems[x].TMP_B_QTY = tmpITM_QTY;
                //        }

                //        I6610Report report = new I6610Report(SelectedMstItems);
                //        report.Margins.Top = 0;
                //        report.Margins.Bottom = 0;
                //        report.Margins.Left = 30;
                //        report.Margins.Right = 0;
                //        report.Landscape = true;
                //        report.PrintingSystem.ShowPrintStatusDialog = true;
                //        report.PaperKind = System.Drawing.Printing.PaperKind.A4;

                //        var window = new DocumentPreviewWindow();
                //        window.PreviewControl.DocumentSource = report;
                //        report.CreateDocument(true);
                //        window.Title = "품목가입고 출력";
                //        window.Owner = Application.Current.MainWindow;
                //        window.ShowDialog();
                //        //XtraReportPreviewModel model = new XtraReportPreviewModel(report);
                //        //DocumentPreviewWindow window = new DocumentPreviewWindow() { Model = model };
                //        //report.CreateDocument(true);
                //        //window.Owner = Application.Current.MainWindow;
                //        //window.Title = "품목가입고 출력";
                //        //window.ShowDialog();
                //    }
                //}
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public async void SYSTEM_CODE_VO()
        {
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

            //LocList = SystemProperties.SYSTEM_CODE_VO("P-008");
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "P-008"))
            {
                if (response.IsSuccessStatusCode)
                {
                    LocList = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                    if (LocList.Count > 0)
                    {
                        M_SL_LOC_NM = LocList[0];
                    }
                }
            }

            Refresh();
        }

    }
}
