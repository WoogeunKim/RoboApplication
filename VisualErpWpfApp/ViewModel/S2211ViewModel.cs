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
using System.Windows.Input;
using System.Windows.Media;
using AquilaErpWpfApp3.Util;
using AquilaErpWpfApp3.View.SAL.Dialog;

namespace AquilaErpWpfApp3.ViewModel
{
    public sealed class S2211ViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private string title = "수주 등록";

        //private IList<CodeDao> UserList;

        //private static SaleOrderServiceClient saleOrderClient = SystemProperties.SaleOrderClient;
        private IList<SaleVo> selectedMstList = new List<SaleVo>();

        //private ObservableCollection<PurVo> selectedDtlList = new ObservableCollection<PurVo>();
        private IList<SaleVo> selectedDtlList = new List<SaleVo>();

        private S2211MasterDialog masterDialog;
        private S2211DetailDialog detailDialog;
        //private S2211DetailDialogEdit detailDialogEdit;

        //private S2211DetailExcelDialog excelDialog;

        private S2211NextMonthDialog masterNextMonthDialog;
        private S2211AddDialog masterAddDialog;
        //private S2211DetailEditDialog detailEditDialog;

        //private S2211DetailQuickDialog detailQuickDialog;

        //private string[,] _loadSave;

        ////Menu Dialog
        //private ICommand _searchDialogCommand;
        //private ICommand _newDialogCommand;
        //private ICommand _editDialogCommand;
        //private ICommand _delDialogCommand;
        //private ICommand _excelDialogCommand;

        private ICommand _copyDialogCommand;

        //private ICommand _searchDetailDialogCommand;
        //private ICommand _newDetailDialogCommand;
        //private ICommand _editDetailDialogCommand;
        //private ICommand _delDetailDialogCommand;

        //private ICommand _nextMonthDialogCommand;

        //private ICommand addDialogCommand;
        //private ICommand _CancelDialogCommand;

        //private ICommand reportDialogCommand;

        ////private ICommand _revListDetailDialogCommand;
        ////private ICommand _revNewDetailDialogCommand;

        //private P41MasterDialog masterDialog;
        //private P41DetailDialog detailDialog;
        ////private A21JobItemRevDialog jobItemRevDialog;

        //private P41ReportDialog reportDialog;

        public S2211ViewModel()
        {
            StartDt = System.DateTime.Now;
            EndDt = System.DateTime.Now;

            //사업장
            SYSTEM_CODE_VO();
            // - Refresh();
        }

        [Command]
        public async void Refresh(string _SL_RLSE_NO = null)
        {
            try
            {
                //DXSplashScreen.Show<ProgressWindow>();
                //SearchDetail = null;
                //SelectDtlList = null;



                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s2211/mst", new StringContent(JsonConvert.SerializeObject(new SaleVo() { FM_DT = (StartDt).ToString("yyyy-MM-dd"), TO_DT = (EndDt).ToString("yyyy-MM-dd"), AREA_CD = M_SL_AREA_VO.CLSS_CD, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectMstList = JsonConvert.DeserializeObject<IEnumerable<SaleVo>>(await response.Content.ReadAsStringAsync()).Cast<SaleVo>().ToList();
                    }
                    //SelectMstList = saleOrderClient.S2211SelectMstList(new SaleVo() { FM_DT = (StartDt).ToString("yyyy-MM-dd"), TO_DT = (EndDt).ToString("yyyy-MM-dd"), AREA_CD = _AreaMap[TXT_SL_AREA_NM] });
                    ////
                    Title = "[기간]" + (StartDt).ToString("yyyy-MM-dd") + "~" + (EndDt).ToString("yyyy-MM-dd") + ",    [사업장]" + M_SL_AREA_VO.CLSS_DESC;

                    if (SelectMstList.Count >= 1)
                    {
                        isM_UPDATE = true;
                        isM_DELETE = true;

                        if (string.IsNullOrEmpty(_SL_RLSE_NO))
                        {
                            SelectedMstItem = SelectMstList[0];
                        }
                        else
                        {
                            SelectedMstItem = SelectMstList.Where(x => x.SL_RLSE_NO.Equals(_SL_RLSE_NO)).LastOrDefault<SaleVo>();
                        }
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
                    //DXSplashScreen.Close();

                    //if (SystemProperties.USER == "134" || SystemProperties.USER == "124" || SystemProperties.USER == "101")
                    //if (UserList.Any<CodeDao>(x => x.CLSS_CD.Equals(SystemProperties.USER)))
                    //{
                    //    isM_Cancel = true;
                    //    isM_Ok = true;
                    //}
                }
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

        private bool? _isM_Ok = false;
        public bool? isM_Ok
        {
            get { return _isM_Ok; }
            set { SetProperty(ref _isM_Ok, value, () => isM_Ok); }
        }
        private bool? _isM_Cancel = false;
        public bool? isM_Cancel
        {
            get { return _isM_Cancel; }
            set { SetProperty(ref _isM_Cancel, value, () => isM_Cancel); }
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

        private bool? _IsEnabledNextMonth = true;
        public bool? IsEnabledNextMonth
        {
            get { return _IsEnabledNextMonth; }
            set { SetProperty(ref _IsEnabledNextMonth, value, () => IsEnabledNextMonth); }
        }

        ////사업장
        //private Dictionary<string, string> _AreaMap = new Dictionary<string, string>();
        private IList<SystemCodeVo> _AreaCd = new List<SystemCodeVo>();
        public IList<SystemCodeVo> SL_AREA_LIST
        {
            get { return _AreaCd; }
            set { SetProperty(ref _AreaCd, value, () => SL_AREA_LIST); }
        }

        //사업장
        private SystemCodeVo _M_SL_AREA_NM;
        public SystemCodeVo M_SL_AREA_VO
        {
            get { return _M_SL_AREA_NM; }
            set { SetProperty(ref _M_SL_AREA_NM, value, () => M_SL_AREA_VO); }
        }

        [Command]
        public async void SelectMstDetail()
        {
            try
            {
                if (this._selectedMstItem == null)
                {
                    return;
                }

                // 마감후 이월버튼 비활성화 마감전 이월버튼 활성화 2017-05-02
                if (SelectedMstItem.CLZ_FLG == "Y")
                {
                    IsEnabledNextMonth = false;
                }
                else if (SelectedMstItem.CLZ_FLG == "N")
                {
                    IsEnabledNextMonth = true;
                }

                SelectedMstItem.AREA_CD = M_SL_AREA_VO.CLSS_CD;
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s2211/dtl", new StringContent(JsonConvert.SerializeObject(SelectedMstItem), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectDtlList = JsonConvert.DeserializeObject<IEnumerable<SaleVo>>(await response.Content.ReadAsStringAsync()).Cast<SaleVo>().ToList();
                    }
                    //else
                    //{
                    //    WinUIMessageBox.Show(await response.Content.ReadAsStringAsync());
                    //}

                    //SelectDtlList = new ObservableCollection<PurVo>(purClient.P4401SelectDtlList(new PurVo() { ITM_CD = this._selectedMstItem.}));
                    //SelectDtlList = saleOrderClient.S2211SelectDtlList(SelectedMstItem);
                    // //
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
            }
            catch (System.Exception eLog)
            {
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
            masterDialog = new S2211MasterDialog(new SaleVo() { SL_AREA_NM = SystemProperties.USER_VO.EMPE_PLC_CD });
            masterDialog.Title = title + " 마스터 관리 - 추가";
            masterDialog.Owner = Application.Current.MainWindow;
            masterDialog.BorderEffect = BorderEffect.Default;
            masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            bool isDialog = (bool)masterDialog.ShowDialog();
            if (isDialog)
            {
                Refresh(masterDialog.SL_RLSE_NO);
                //if (masterDialog.IsEdit == false)
                //{
                //    try
                //    {
                //        DXSplashScreen.Show<ProgressWindow>();
                //        Refresh();
                //        DXSplashScreen.Close();

                //        for (int x = 0; x < SelectMstList.Count; x++)
                //        {
                //            if (masterDialog.SL_RLSE_NO.Equals(SelectMstList[x].SL_RLSE_NO))
                //            {
                //                SelectedMstItem = SelectMstList[x];
                //                //
                //                NewDtlContact();
                //                return;
                //            }
                //        }


                //    }
                //    catch (System.Exception eLog)
                //    {
                //        //DXSplashScreen.Close();
                //        WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
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

            try
            {
                if (SelectedMstItem == null)
                {
                    return;
                }
                //else if (SelectMstList.Count <= 0)
                //{
                //    return;
                //}

                masterDialog = new S2211MasterDialog(SelectedMstItem);
                masterDialog.Title = title + " 마스터 관리 - 수정 - [요청 번호 명 : " + SelectedMstItem.SL_RLSE_NM + "]";
                masterDialog.Owner = Application.Current.MainWindow;
                masterDialog.BorderEffect = BorderEffect.Default;
                masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
                masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
                bool isDialog = (bool)masterDialog.ShowDialog();
                if (isDialog)
                {
                    Refresh(masterDialog.SL_RLSE_NO);
                    //if (masterDialog.IsEdit == false)
                    //{
                    //    DXSplashScreen.Show<ProgressWindow>();
                    //    Refresh();
                    //    DXSplashScreen.Close();

                    //    for (int x = 0; x < SelectMstList.Count; x++)
                    //    {
                    //        if (masterDialog.text_SL_RLSE_NO.Equals(SelectMstList[x].SL_RLSE_NO))
                    //        {
                    //            SelectedMstItem = SelectMstList[x];
                    //            //
                    //            NewDtlContact();
                    //            return;
                    //        }
                    //    }

                    //}
                    //else
                    //{
                    //    DXSplashScreen.Show<ProgressWindow>();
                    //    Refresh();
                    //    DXSplashScreen.Close();
                    //    for (int x = 0; x < SelectMstList.Count; x++)
                    //    {
                    //        if (masterDialog.SL_RLSE_NO.Equals(SelectMstList[x].SL_RLSE_NO))
                    //        {
                    //            SelectedMstItem = SelectMstList[x];
                    //        }
                    //    }
                    //    //if (SelectedMstItem.CLZ_FLG.Equals("Y"))
                    //    //{
                    //    //    try
                    //    //    {
                    //    //        DXSplashScreen.Show<ProgressWindow>();
                    //    //        SelectMstDetail();
                    //    //        DXSplashScreen.Close();
                    //    //    }
                    //    //    catch (System.Exception eLog)
                    //    //    {
                    //    //        DXSplashScreen.Close();
                    //    //        WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                    //    //        return;
                    //    //    }
                    //    //}
                    //}
                }
            }
            catch (System.Exception eLog)
            {
                //DXSplashScreen.Close();
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
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
            try
            {
                if (SelectedMstItem == null)
                {
                    return;
                }
                if (SelectDtlList.Count > 0)
                {
                    WinUIMessageBox.Show("수주 등록 물품 내역이 존재하여 삭제할 수 없습니다.", "[경고]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }


                else if (SelectedMstItem.CLZ_FLG.Equals("Y"))
                {
                    WinUIMessageBox.Show("[요청 번호 명 : " + SelectedMstItem.SL_RLSE_NM + "] 마감 처리 되었습니다", "[삭제]" + title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                    return;
                }
                //else if (saleOrderClient.S2211DeleteDtlCheck(new JobVo() { SL_RLSE_NO = SelectedMstItem.SL_RLSE_NO }) > 0)
                //{
                //    WinUIMessageBox.Show("[요청 번호 명 : " + SelectedMstItem.SL_RLSE_NM + "] 거래명세표가 발행 되었습니다", "[삭제]" + title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                //    return;
                //}

                //JobVo delDao = SelectedMstItem;
                //if (delDao != null)
                //{
                    MessageBoxResult result = WinUIMessageBox.Show("[요청 번호 명 : " + SelectedMstItem.SL_RLSE_NM + "]" + " 정말로 삭제 하시겠습니까?", "[삭제]" + title, MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        SelectedMstItem.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
                        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s2211/mst/d", new StringContent(JsonConvert.SerializeObject(SelectedMstItem), System.Text.Encoding.UTF8, "application/json")))
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

                                WinUIMessageBox.Show("삭제가 완료되었습니다.", "[삭제]" + title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                                Refresh();
                            }
                        }

                            //            //DXSplashScreen.Show<ProgressWindow>();
                            //            JobVo resultVo = saleOrderClient.S2211DeleteDtl(new JobVo() { SL_RLSE_NO = delDao.SL_RLSE_NO });
                            //            if (!resultVo.isSuccess)
                            //            {
                            //                //실패
                            //                WinUIMessageBox.Show(resultVo.Message, "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
                            //                return;
                            //            }
                            //            SelectMstDetail();


                            //            resultVo = saleOrderClient.S2211DeleteMst(new JobVo() { SL_RLSE_NO = delDao.SL_RLSE_NO });
                            //            if (!resultVo.isSuccess)
                            //            {
                            //                //실패
                            //                WinUIMessageBox.Show(resultVo.Message, "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
                            //                return;
                            //            }
                            //            Refresh();

                            //            //DXSplashScreen.Close();
                //    }
                    }
            }
            catch (System.Exception eLog)
            {
                //DXSplashScreen.Close();
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }



        //public ICommand ExcelDialogCommand
        //{
        //   get
        //   {
        //       if (_excelDialogCommand == null)
        //           _excelDialogCommand = new DelegateCommand(ExcelContact);
        //       return _excelDialogCommand;
        //   }
        //}

        //public void ExcelContact()
        //{
        //    JobVo excelDao = SelectedMstItem;
        //    if (excelDao != null)
        //    {
        //        try
        //        {
        //           Microsoft.Win32.OpenFileDialog sfd = new Microsoft.Win32.OpenFileDialog();
        //           sfd.DefaultExt = ".xls";
        //           sfd.Filter = "Microsoft Excel 97-2003|*.xls|Microsoft Excel|*.xlsx"; 
        //           if (sfd.ShowDialog() == true)
        //           {
        //               DXSplashScreen.Show<ProgressWindow>();
        //               string[,] _loadSave = SystemProperties.ExcelLoad(sfd.FileName);

        //               IList<JobVo> resultItem = new List<JobVo>();
        //               JobVo tmpVo;
        //               long nRow = _loadSave.GetLongLength(0);
        //               long nColumn = _loadSave.GetLongLength(1);

        //               for (long row = 2; row < nRow; row++)
        //               {
        //                   //for (long column = 0; column < nColumn; column++)
        //                   {
        //                       tmpVo = new JobVo();
        //                       tmpVo.JB_NO = excelDao.JB_NO;
        //                       tmpVo.JB_NM = excelDao.JB_NM;
        //                       tmpVo.CRE_USR_ID = SystemProperties.USER;
        //                       tmpVo.UPD_USR_ID = SystemProperties.USER;
        //                       tmpVo.CLZ_FLG = "N";

        //                       //
        //                       if(!string.IsNullOrEmpty(_loadSave[row, 1].Trim()))
        //                       {
        //                           tmpVo.RN = int.Parse(string.IsNullOrEmpty(_loadSave[row, 1].Trim()) ? "0" : _loadSave[row, 1].Trim());
        //                           tmpVo.ITM_CD = _loadSave[row, 2].Trim();
        //                           tmpVo.ITM_USG_MSG = _loadSave[row, 3].Trim();
        //                           tmpVo.ITM_NM = _loadSave[row, 4].Trim();
        //                           tmpVo.ITM_SZ_NM = _loadSave[row, 5].Trim();
        //                           tmpVo.UOM_NM = _loadSave[row, 6].Trim();
        //                           tmpVo.ITM_QTY = int.Parse(string.IsNullOrEmpty(_loadSave[row, 7].Trim()) ? "0" : _loadSave[row, 7].Trim());
        //                           tmpVo.ALS_NO = _loadSave[row, 8].Trim();
        //                           tmpVo.CO_NO = _loadSave[row, 9].Trim();
        //                           tmpVo.ITM_RMK = _loadSave[row, 10].Trim();

        //                           resultItem.Add(tmpVo);
        //                       }
        //                   }
        //               }

        //               DXSplashScreen.Close();


        //               //
        //               //
        //               excelDialog = new S2211DetailExcelDialog(resultItem);
        //               excelDialog.Title = "CSV파일로 추가하기 - [" + excelDao.JB_NM + "/" + excelDao.JB_NO + "]";
        //               excelDialog.Owner = Application.Current.MainWindow;
        //               excelDialog.BorderEffect = BorderEffect.Default;
        //               //excelDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
        //               //excelDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
        //               bool isDialog = (bool)excelDialog.ShowDialog();
        //               if (isDialog)
        //               {
        //                   JobVo[] resultList = excelDialog.InsertList.ToArray();
        //                   JobVo resultVo = saleOrderClient.S2211TransactionInsertDtl(resultList);

        //                   if (!resultVo.isSuccess)
        //                   {
        //                       //실패
        //                       WinUIMessageBox.Show(resultVo.Message, "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
        //                       return;
        //                   }
        //                   // 성공
        //                   WinUIMessageBox.Show("[ 총 : " + excelDialog.InsertList.Count + " ] 완료 되었습니다", "[추가]" + title, MessageBoxButton.OK, MessageBoxImage.Information);
        //                   ////

        //                   DXSplashScreen.Show<ProgressWindow>();
        //                   SelectMstDetail();
        //                   DXSplashScreen.Close();
        //               }
        //           }
        //        }
        //        catch (Exception eLog)
        //        {
        //            DXSplashScreen.Close();
        //            WinUIMessageBox.Show(eLog.Message, "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
        //            return;
        //        }
        //    }

        //   //JobVo excelDao = SelectedMstItem;
        //   //if (excelDao != null)
        //   //{
        //   //    try
        //   //    {
        //   //        Microsoft.Win32.OpenFileDialog sfd = new Microsoft.Win32.OpenFileDialog();
        //   //        sfd.DefaultExt = ".csv";
        //   //        sfd.Filter = "Comma Separated Values (*.csv)|*.csv";
        //   //        if (sfd.ShowDialog() == true)
        //   //        {
        //   //            IList<JobVo> resultItem = new List<JobVo>();
        //   //            FileStream fs = null;
        //   //            StreamReader sr = null;
        //   //            try
        //   //            {
        //   //                fs = new FileStream(sfd.FileName, FileMode.Open);
        //   //                sr = new StreamReader(fs, Encoding.GetEncoding("euc-kr"));
        //   //                string strData = sr.ReadToEnd();

        //   //                string[] strDataArray = strData.Split(new char[] { ',', '\n' });
        //   //                //data = "순번, 품번, 용도, 품명, SIZE, 단위, 수량, ALS번호, 발주처, 제작참고사항";
        //   //                int nCnt = strDataArray.Length / 10;
        //   //                if ((strDataArray.Length % 10) > 0)
        //   //                {
        //   //                    nCnt++;
        //   //                }

        //   //                JobVo tmpVo;
        //   //                int nLen = 0;
        //   //                for (int x = 0; x < nCnt-1; x++)
        //   //                {
        //   //                    tmpVo = new JobVo();
        //   //                    tmpVo.JB_NO = excelDao.JB_NO;
        //   //                    tmpVo.JB_NM = excelDao.JB_NM;
        //   //                    tmpVo.CRE_USR_ID = SystemProperties.USER;
        //   //                    tmpVo.UPD_USR_ID = SystemProperties.USER;
        //   //                    nLen = strDataArray.Length - 1;
        //   //                    //순번
        //   //                    if (nLen >= (0 + (x * 10)))
        //   //                    {
        //   //                        if (!string.IsNullOrEmpty(strDataArray[0 + (x * 10)].Trim()))
        //   //                        {
        //   //                            tmpVo.RN = int.Parse(string.IsNullOrEmpty(strDataArray[0 + (x * 10)].Trim()) ? "0" : strDataArray[0 + (x * 10)].Trim());
        //   //                        }
        //   //                    }
        //   //                    //품번
        //   //                    if (nLen >= (1 + (x * 10)))
        //   //                    {
        //   //                        tmpVo.ITM_CD = strDataArray[1 + (x * 10)].Trim();
        //   //                    }
        //   //                    //용도
        //   //                    if (nLen >= (2 + (x * 10)))
        //   //                    {
        //   //                        tmpVo.ITM_USG_MSG = strDataArray[2 + (x * 10)].Trim();
        //   //                    }
        //   //                    //품명
        //   //                    if (nLen >= (3 + (x * 10)))
        //   //                    {
        //   //                        tmpVo.ITM_NM = strDataArray[3 + (x * 10)].Trim();
        //   //                    }
        //   //                    //SIZE
        //   //                    if (nLen >= (4 + (x * 10)))
        //   //                    {
        //   //                        tmpVo.ITM_SZ_NM = strDataArray[4 + (x * 10)].Trim();
        //   //                    }
        //   //                    //단위
        //   //                    if (nLen >= (5 + (x * 10)))
        //   //                    {
        //   //                        tmpVo.UOM_NM = strDataArray[5 + (x * 10)].Trim();
        //   //                    }
        //   //                    //수량
        //   //                    if (nLen >= (6 + (x * 10)))
        //   //                    {
        //   //                        tmpVo.ITM_QTY = int.Parse(string.IsNullOrEmpty(strDataArray[6 + (x * 10)].Trim()) ? "0" : strDataArray[6 + (x * 10)].Trim());
        //   //                    }
        //   //                    //ALS번호
        //   //                    if (nLen >= (7 + (x * 10)))
        //   //                    {
        //   //                        tmpVo.ALS_NO = strDataArray[7 + (x * 10)].Trim();
        //   //                    }
        //   //                    //발주처
        //   //                    if (nLen >= (8 + (x * 10)))
        //   //                    {
        //   //                        tmpVo.CO_NO = strDataArray[8 + (x * 10)].Trim();
        //   //                    }
        //   //                    //비고
        //   //                    if (nLen >= (9 + (x * 10)))
        //   //                    {
        //   //                        tmpVo.ITM_RMK = strDataArray[9 + (x * 10)].Trim();
        //   //                    }

        //   //                    resultItem.Add(tmpVo);
        //   //                }
        //   //                //
        //   //                //
        //   //                excelDialog = new S2211DetailExcelDialog(resultItem);
        //   //                excelDialog.Title = "CSV파일로 추가하기 - [" + excelDao.JB_NM + "/" + excelDao.JB_NO + "]";
        //   //                excelDialog.Owner = Application.Current.MainWindow;
        //   //                excelDialog.BorderEffect = BorderEffect.Default;
        //   //                //excelDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
        //   //                //excelDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
        //   //                bool isDialog = (bool)excelDialog.ShowDialog();
        //   //                if (isDialog)
        //   //                {
        //   //                    JobVo[] resultList = excelDialog.InsertList.ToArray();
        //   //                    JobVo resultVo = saleOrderClient.S2211TransactionInsertDtl(resultList);

        //   //                    if (!resultVo.isSuccess)
        //   //                    {
        //   //                        //실패
        //   //                        WinUIMessageBox.Show(resultVo.Message, "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
        //   //                        return;
        //   //                    }
        //   //                    // 성공
        //   //                    WinUIMessageBox.Show("[ 총 : " + excelDialog.InsertList.Count + " ] 완료 되었습니다", "[추가]" + title, MessageBoxButton.OK, MessageBoxImage.Information);
        //   //                    ////

        //   //                    DXSplashScreen.Show<ProgressWindow>();
        //   //                    SelectMstDetail();
        //   //                    DXSplashScreen.Close();

        //   //                }
        //   //            }
        //   //            catch (Exception except)
        //   //            {
        //   //                WinUIMessageBox.Show(except.Message, "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
        //   //                return;
        //   //            }
        //   //            finally
        //   //            {
        //   //                if (fs != null)
        //   //                {
        //   //                    fs.Close();
        //   //                }
        //   //                if (sr != null)
        //   //                {
        //   //                    sr.Close();
        //   //                }
        //   //            }
        //   //        }
        //   //    }
        //   //    catch (Exception eLog)
        //   //    {
        //   //        WinUIMessageBox.Show(eLog.Message, "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
        //   //        return;
        //   //    }
        //   //}
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

        //public ICommand NewDtlDialogCommand
        //{
        //    get
        //    {
        //        if (_newDetailDialogCommand == null)
        //            _newDetailDialogCommand = new DelegateCommand(NewDtlContact);
        //        return _newDetailDialogCommand;
        //    }
        //}

        [Command]
        public void NewDtlContact()
        {
            if (SelectedMstItem == null)
            {
                return;
            }
            else if (SelectedMstItem.CLZ_FLG.Equals("Y"))
            {
                WinUIMessageBox.Show("[요청 번호 명 : " + SelectedMstItem.SL_RLSE_NM + "] 마감 처리 되었습니다", "[수주 등록 물품 관리]" + title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }

            detailDialog = new S2211DetailDialog(SelectedMstItem);
            detailDialog.Title = "수주 등록 물품 관리 - [요청 번호 명 : " + SelectedMstItem.SL_RLSE_NM + "]";
            detailDialog.Owner = Application.Current.MainWindow;
            detailDialog.BorderEffect = BorderEffect.Default;
            detailDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            detailDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            bool isDialog = (bool)detailDialog.ShowDialog();
            if (isDialog)
            {
                SelectMstDetail();
                //    //if (detailDialog.IsEdit == false)
                //    //{
                //        try
                //        {
                //            DXSplashScreen.Show<ProgressWindow>();
                //            JobVo resultVo = saleOrderClient.ProcS2211(new JobVo() { SL_RLSE_NO = SelectedMstItem.SL_RLSE_NO, CRE_USR_ID = SystemProperties.USER, UPD_USR_ID = SystemProperties.USER });
                //            if (!resultVo.isSuccess)
                //            {
                //                //실패
                //                WinUIMessageBox.Show(resultVo.Message, "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
                //                return;
                //            }

                //            SelectMstDetail();
                //            DXSplashScreen.Close();
                //        }
                //        catch (System.Exception eLog)
                //        {
                //            DXSplashScreen.Close();
                //            WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                //            return;
                //        }
                //    //}
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

        //    //else if (SelectedMstItem.CLZ_FLG.Equals("Y"))
        //    //{
        //    //    WinUIMessageBox.Show("[요청 번호 명 : " + SelectedMstItem.SL_RLSE_NM + "] 마감 처리 되었습니다", "[수주 등록 물품 관리]" + title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
        //    //    return;
        //    //}

        //    //detailDialog = new S2211DetailDialog(SelectedMstItem);
        //    //detailDialog.Title = "수주 등록 물품 관리 - [요청 번호 명 : " + SelectedMstItem.SL_RLSE_NM + "]";
        //    //detailDialog.Owner = Application.Current.MainWindow;
        //    //detailDialog.BorderEffect = BorderEffect.Default;
        //    //////masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
        //    //////masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
        //    ////detailDialog.IsEdit = false;
        //    //bool isDialog = (bool)detailDialog.ShowDialog();
        //    ////if (isDialog)
        //    //{
        //    //    //if (detailDialog.IsEdit == false)
        //    //    //{
        //    //    try
        //    //    {
        //    //        DXSplashScreen.Show<ProgressWindow>();
        //    //        JobVo resultVo = saleOrderClient.ProcS2211(new JobVo() { SL_RLSE_NO = SelectedMstItem.SL_RLSE_NO, CRE_USR_ID = SystemProperties.USER, UPD_USR_ID = SystemProperties.USER });
        //    //        if (!resultVo.isSuccess)
        //    //        {
        //    //            //실패
        //    //            WinUIMessageBox.Show(resultVo.Message, "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
        //    //            return;
        //    //        }

        //    //        SelectMstDetail();
        //    //        DXSplashScreen.Close();
        //    //    }
        //    //    catch (System.Exception eLog)
        //    //    {
        //    //        DXSplashScreen.Close();
        //    //        WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
        //    //        return;
        //    //    }
        //    //    //}
        //    //}
        //    try
        //    {
        //        //if (SelectedMstItem == null)
        //        //{
        //        //    return;
        //        //}
        //        //JobVo tmpVo = SearchDetail;
        //        //JobVo tmpVo2 = SelectedMstItem;
        //        //detailDialogEdit = new S2211DetailDialogEdit(SearchDetail, SelectedMstItem);
        //        //detailDialogEdit.Title = title + " 디테일 관리 - 수정 - [요청 번호 명 : " + SelectedMstItem.SL_RLSE_NM + "]";
        //        //detailDialogEdit.Owner = Application.Current.MainWindow;
        //        //detailDialogEdit.BorderEffect = BorderEffect.Default;
        //        //////masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
        //        //////masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
        //        //bool isDialog = (bool)detailDialogEdit.ShowDialog();
        //        //if (isDialog)
        //        //{
        //        //    if (detailDialogEdit.IsEdit == false)
        //        //    {
        //        //        DXSplashScreen.Show<ProgressWindow>();
        //        //        Refresh();

        //        //        DXSplashScreen.Close();
        //        //    }
        //        //}
        //        //Refresh();
        //        //SelectMstDetail();

        //        //for (int x = 0; x < SelectMstList.Count; x++)
        //        //{
        //        //    if (tmpVo2.SL_RLSE_NO.Equals(SelectMstList[x].SL_RLSE_NO))
        //        //    {
        //        //        SelectedMstItem = SelectMstList[x];
        //        //        //

        //        //    }
        //        //}

        //        //for (int x = 0; x < SelectDtlList.Count; x++)
        //        //{
        //        //    if (tmpVo.SL_ITM_CD.Equals(SelectDtlList[x].SL_ITM_CD))
        //        //    {
        //        //        SearchDetail = SelectDtlList[x];
        //        //        //
        //        //    }
        //        //}
        //    }
        //    catch (Exception eLog)
        //    {
        //        WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
        //        return;
        //    }




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

        //    try
        //    {
        //        //if (SelectedMstItem == null)
        //        //{
        //        //    return;
        //        //}

        //        //detailEditDialog = new S2211DetailEditDialog(SelectedMstItem);
        //        //detailEditDialog.Title = this.title + " - 삭제 [입고 번호 : " + SelectedMstItem.SL_RLSE_NM + "]";
        //        //detailEditDialog.Owner = Application.Current.MainWindow;
        //        //detailEditDialog.BorderEffect = BorderEffect.Default;
        //        //bool isDialog = (bool)detailEditDialog.ShowDialog();
        //        //if (isDialog)
        //        //{
        //        //    DXSplashScreen.Show<ProgressWindow>();
        //        //    SelectMstDetail();
        //        //    DXSplashScreen.Close();
        //        //}
        //    }
        //    catch (System.Exception)
        //    {
        //        //DXSplashScreen.Close();
        //        //  WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
        //        return;
        //    }
        //}
        //    else if (SelectedMstItem.CLZ_FLG.Equals("Y"))
        //    {
        //        WinUIMessageBox.Show("[요청 번호 명 : " + SelectedMstItem.SL_RLSE_NM + "] 마감 처리 되었습니다", "[삭제]" + title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
        //        return;
        //    }

        //    JobVo delDao = SearchDetail;
        //    if (delDao != null)
        //    {
        //        MessageBoxResult result = WinUIMessageBox.Show("[요청 번호 명 : " + SearchDetail.ITM_NM + "," +SearchDetail.ITM_SZ_NM+"]" + " 정말로 삭제 하시겠습니까?", "[삭제]" + title, MessageBoxButton.YesNo, MessageBoxImage.Question);
        //        if (result == MessageBoxResult.Yes)
        //        {
        //            try
        //            {
        //                DXSplashScreen.Show<ProgressWindow>();
        //                saleOrderClient.S2211DeleteDtl(delDao);
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





        public ICommand CopyDialogCommand
        {
            get
            {
                if (_copyDialogCommand == null)
                    _copyDialogCommand = new DelegateCommand(CopyContact);
                return _copyDialogCommand;
            }
        }

        public void CopyContact()
        {
            //detailQuickDialog = new S2211DetailQuickDialog(new JobVo());
            //detailQuickDialog.Title = "수주 등록 물품 등록";
            //detailQuickDialog.Owner = Application.Current.MainWindow;
            //detailQuickDialog.BorderEffect = BorderEffect.Default;
            //////masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            //////masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            ////detailDialog.IsEdit = false;
            //bool isDialog = (bool)detailQuickDialog.ShowDialog();
            //if (isDialog)
            //{
            //    //if (detailDialog.IsEdit == false)
            //    //{
            //    try
            //    {
            //        DXSplashScreen.Show<ProgressWindow>();
            //        Refresh();

            //        for (int x = 0; x < SelectMstList.Count; x++)
            //        {
            //            if (detailQuickDialog.masterDao.SL_RLSE_NO.Equals(SelectMstList[x].SL_RLSE_NO))
            //            {
            //                SelectedMstItem = SelectMstList[x];
            //                break;
            //            }
            //        }

            //        JobVo resultVo = saleOrderClient.ProcS2211(new JobVo() { SL_RLSE_NO = detailQuickDialog.masterDao.SL_RLSE_NO, CRE_USR_ID = SystemProperties.USER, UPD_USR_ID = SystemProperties.USER });
            //        if (!resultVo.isSuccess)
            //        {
            //            //실패
            //            WinUIMessageBox.Show(resultVo.Message, "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
            //            return;
            //        }

            //        SelectMstDetail();

            //        DXSplashScreen.Close();
            //    }
            //    catch (System.Exception eLog)
            //    {
            //        DXSplashScreen.Close();
            //        WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
            //        return;
            //    }
            //    //}
            //}

        }


        //public ICommand NextMonthDialogCommand
        //{
        //    get
        //    {
        //        if (_nextMonthDialogCommand == null)
        //            _nextMonthDialogCommand = new DelegateCommand(NextMonthContact);
        //        return _nextMonthDialogCommand;
        //    }
        //}

        [Command]
        public async void NextMonthContact()
        {
            if (SelectedMstItem == null)
            {
                return;
            }

            masterNextMonthDialog = new S2211NextMonthDialog(SelectedMstItem);
            masterNextMonthDialog.Title = "이월 등록";
            masterNextMonthDialog.Owner = Application.Current.MainWindow;
            masterNextMonthDialog.BorderEffect = BorderEffect.Default;
            masterNextMonthDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            masterNextMonthDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            //detailDialog.IsEdit = false;
            bool isDialog = (bool)masterNextMonthDialog.ShowDialog();
            if (isDialog)
            {
                try
                {
                    DXSplashScreen.Show<ProgressWindow>();

                    SelectedMstItem.SL_NXT_CLZ_FLG = "Y";
                    SelectedMstItem.NXT_MON_DT = masterNextMonthDialog.NXT_MON_DT;
                    SelectedMstItem.CRE_USR_ID = SystemProperties.USER;
                    SelectedMstItem.UPD_USR_ID = SystemProperties.USER;
                    SelectedMstItem.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;

                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s2211/mst/u", new StringContent(JsonConvert.SerializeObject(SelectedMstItem), System.Text.Encoding.UTF8, "application/json")))
                    {
                        int _Num = 0; 
                        if (response.IsSuccessStatusCode)
                        {
                            string result = await response.Content.ReadAsStringAsync();
                            if (int.TryParse(result, out _Num) == false)
                            {
                                //실패
                                WinUIMessageBox.Show(result, title, MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }

                            //성공
                            //WinUIMessageBox.Show("완료 되었습니다", title, MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }
                catch (System.Exception eLog)
                {
                    WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                    return;
                }
                //}
            }




            //if (SelectedMstItem == null)
            //{
            //    return;
            //}

            //masterCopyDialog = new S2211MasterCopyDialog();
            //masterCopyDialog.Title = "수주(자재리스트)등록 - 복사";
            //masterCopyDialog.Owner = Application.Current.MainWindow;
            //masterCopyDialog.BorderEffect = BorderEffect.Default;
            //masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            //masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            //detailDialog.IsEdit = false;
            //bool isDialog = (bool)masterCopyDialog.ShowDialog();
            //if (isDialog)
            //{
            //    if (masterCopyDialog.IsEdit == false)
            //    {
            //        try
            //        {
            //            DXSplashScreen.Show<ProgressWindow>();
            //            Refresh();
            //            DXSplashScreen.Close();
            //        }
            //        catch (System.Exception eLog)
            //        {
            //            DXSplashScreen.Close();
            //            WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
            //            return;
            //        }
            //    }
            //}
        }






        //public void ShowMasterDialog(PurVo dao)
        //{
        //    masterDialog = new P41MasterDialog(dao);
        //    masterDialog.Title = "견적 마스터 관리";
        //    masterDialog.Owner = Application.Current.MainWindow;
        //    masterDialog.BorderEffect = BorderEffect.Default;
        //    ////masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
        //    ////masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
        //    bool isDialog = (bool)masterDialog.ShowDialog();
        //    if (isDialog)
        //    {
        //        if (masterDialog.IsEdit == false)
        //        {
        //            Refresh();
        //        }
        //    }
        //}

        //public ICommand DelDialogCommand
        //{
        //    get
        //    {
        //        if (_delDialogCommand == null)
        //            _delDialogCommand = new DelegateCommand(DelMasterContact);
        //        return _delDialogCommand;
        //    }
        //}

        //public void DelMasterContact()
        //{
        //    if (SelectedMstJobItem == null) { return; }
        //    PurVo delDao = SelectedMstJobItem;
        //    if (delDao != null)
        //    {
        //        MessageBoxResult result = WinUIMessageBox.Show("[" + delDao.PUR_ESTM_NO + "]" + " 정말로 삭제 하시겠습니까?", "[삭제]견적 관리", MessageBoxButton.YesNo, MessageBoxImage.Question);
        //        if (result == MessageBoxResult.Yes)
        //        {
        //            purClient.DeletePurEstmDtl(delDao);
        //            DetailRefresh();

        //            purClient.DeletePurEstmMast(delDao);
        //            WinUIMessageBox.Show("삭제가 완료되었습니다.", "[삭제]견적 관리", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
        //            Refresh();
        //            _selectedMstJobItem = new PurVo();
        //        }
        //    }
        //}


        //public ICommand NewDetailDialogCommand
        //{
        //    get
        //    {
        //        if (_newDetailDialogCommand == null)
        //            _newDetailDialogCommand = new DelegateCommand(NewDetailContact);
        //        return _newDetailDialogCommand;
        //    }
        //}

        //public void NewDetailContact()
        //{
        //    if (SelectedMstJobItem == null) { return; }
        //    PurVo itemDao = SelectedMstJobItem;
        //    ShowMasterItemDialog(new PurVo() { PUR_ESTM_NO = itemDao.PUR_ESTM_NO });
        //}

        //public ICommand EditDetailDialogCommand
        //{
        //    get
        //    {
        //        if (_editDetailDialogCommand == null)
        //            _editDetailDialogCommand = new DelegateCommand(EditDetailContact);
        //        return _editDetailDialogCommand;
        //    }
        //}

        //public void EditDetailContact()
        //{
        //    if (SelectedMstJobItem == null) { return; }
        //    if (SearchDetailJob == null) { return; }
        //    PurVo editDao = SearchDetailJob;
        //    if (editDao != null)
        //    {
        //        ShowMasterItemDialog(editDao);
        //    }
        //}


        //public void ShowMasterItemDialog(PurVo editDao)
        //{
        //    if (SelectedMstJobItem == null) { return; }
        //    if (string.IsNullOrEmpty(editDao.PUR_ESTM_NO))
        //    {
        //        return;
        //    }
        //    ////JobVo itemDao = SearchMstDetailJob;
        //    ////
        //    detailDialog = new P41DetailDialog(editDao);
        //    detailDialog.Title = "견적 의뢰 내역 관리 [" + editDao.PUR_ESTM_NO + "]";
        //    detailDialog.Owner = Application.Current.MainWindow;
        //    detailDialog.BorderEffect = BorderEffect.Default;
        //    //jobItemDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
        //    //jobItemDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
        //    bool isDialog = (bool)detailDialog.ShowDialog();
        //    if (isDialog)
        //    {
        //        if (detailDialog.IsEdit == false)
        //        {
        //            DetailRefresh();
        //        }
        //    }
        //}

        //public ICommand SearchDetailDialogCommand
        //{
        //    get
        //    {
        //        if (_searchDetailDialogCommand == null)
        //            _searchDetailDialogCommand = new DelegateCommand(DetailRefresh);
        //        return _searchDetailDialogCommand;
        //    }
        //}

        //private void DetailRefresh()
        //{
        //    SelectDtlItmList = purClient.SelectPurEstmDtlList(this._selectedMstJobItem);
        //}


        //public ICommand DelDetailDialogCommand
        //{
        //    get
        //    {
        //        if (_delDetailDialogCommand == null)
        //            _delDetailDialogCommand = new DelegateCommand(DelDetailContact);
        //        return _delDetailDialogCommand;
        //    }
        //}

        //public void DelDetailContact()
        //{
        //    if (SelectedMstJobItem == null) { return; }
        //    if (SearchDetailJob == null) { return; }
        //    PurVo delDao = SearchDetailJob;
        //    if (delDao != null)
        //    {
        //        MessageBoxResult result = WinUIMessageBox.Show("[" + delDao.PUR_ESTM_NO + "]" + delDao.PUR_ESTM_SEQ + " 정말로 삭제 하시겠습니까?", "[삭제]견적 관리", MessageBoxButton.YesNo, MessageBoxImage.Question);
        //        if (result == MessageBoxResult.Yes)
        //        {
        //            purClient.DeletePurEstmDtl(delDao);
        //            WinUIMessageBox.Show("삭제가 완료되었습니다.", "[삭제]견적 관리", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
        //            DetailRefresh();
        //        }
        //    }
        //}
        //#endregion


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
        //    if (SelectedMstJobItem == null) { return; }
        //    ShowReportDialog(SelectedMstJobItem);
        //}

        //public void ShowReportDialog(PurVo dao)
        //{
        //    reportDialog = new P41ReportDialog(dao);
        //    reportDialog.Title = "견적서";
        //    reportDialog.Owner = Application.Current.MainWindow;
        //    reportDialog.BorderEffect = BorderEffect.Default;
        //    //masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
        //    //masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
        //    bool isDialog = (bool)reportDialog.ShowDialog();
        //    //if (isDialog)
        //    //{
        //    //    SearchDetailItem();
        //    //}
        //}

        //public ICommand AddDialogCommand
        //{
        //    get
        //    {
        //        if (addDialogCommand == null)
        //            addDialogCommand = new DelegateCommand(AddContact);
        //        return addDialogCommand;
        //    }
        //}

        public async void AddContact()
        {
            if (SelectedMstItem == null)
            {
                return;
            }

            masterAddDialog = new S2211AddDialog(SelectedMstItem);
            masterAddDialog.Title = "추가 주문";
            masterAddDialog.Owner = Application.Current.MainWindow;
            masterAddDialog.BorderEffect = BorderEffect.Default;
            masterAddDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            masterAddDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            //detailDialog.IsEdit = false;
            bool isDialog = (bool)masterAddDialog.ShowDialog();
            if (isDialog)
            {
                try
                {
                    SelectedMstItem.ADD_FLG = masterAddDialog.ADD_FLG;
                    //SelectedMstItem.NXT_MON_DT = masterAddDialog.NXT_MON_DT;
                    SelectedMstItem.CRE_USR_ID = SystemProperties.USER;
                    SelectedMstItem.UPD_USR_ID = SystemProperties.USER;
                    SelectedMstItem.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;


                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s2211/mst/u", new StringContent(JsonConvert.SerializeObject(SelectedMstItem), System.Text.Encoding.UTF8, "application/json")))
                    {
                        int _Num = 0;                        if (response.IsSuccessStatusCode)
                        {
                            string result = await response.Content.ReadAsStringAsync();
                            if (int.TryParse(result, out _Num) == false)
                            {
                                //실패
                                WinUIMessageBox.Show(result, title, MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }

                            //성공
                            //WinUIMessageBox.Show("완료 되었습니다", title, MessageBoxButton.OK, MessageBoxImage.Information);
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

        //public ICommand CancelDialogCommand
        //{
        //    get
        //    {
        //        if (_CancelDialogCommand == null)
        //            _CancelDialogCommand = new DelegateCommand(CancelContact);
        //        return _CancelDialogCommand;
        //    }
        //}

        [Command]
        public void CancelContact()
        {
            if (SelectedMstItem == null)
            {
                return;
            }

            try
            {
                MessageBoxResult result = WinUIMessageBox.Show("[요청 번호 명 : " + SelectedMstItem.SL_RLSE_NO + "] 정말로 마감해제 하시겠습니까? ", "[마감해제]" + this.title, MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                //    JobVo resultVo = saleOrderClient.ProcS2211Delete(new JobVo() { SL_RLSE_NO = SelectedMstItem.SL_RLSE_NO, CRE_USR_ID = SystemProperties.USER});
                //    if (!resultVo.isSuccess)
                //    {
                //        //실패
                //        WinUIMessageBox.Show(resultVo.Message, "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
                //        return;
                //    }
                //    WinUIMessageBox.Show("[요청 번호 명 : " + SelectedMstItem.SL_RLSE_NO + "] 마감해제 하였습니다.", "[마감해제]" + title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                //    Refresh();
                }
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }


        }

        public async void SYSTEM_CODE_VO()
        {
            //SL_AREA_LIST = SystemProperties.SYSTEM_CODE_VO("L-002");
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "L-002"))
            {
                if (response.IsSuccessStatusCode)
                {
                    SL_AREA_LIST = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                    if (SL_AREA_LIST.Count > 0)
                    {
                        M_SL_AREA_VO = SL_AREA_LIST[0];
                    }
                }
            }
            Refresh();
        }
    }
}
