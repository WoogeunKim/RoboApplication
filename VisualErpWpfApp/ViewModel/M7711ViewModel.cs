using AquilaErpWpfApp3.Util;
using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Man;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;

namespace AquilaErpWpfApp3.ViewModel
{
    public sealed class M7711ViewModel : ViewModelBase, INotifyPropertyChanged {


        private string _title = "PC 생산 계획 관리";

        //private static ManServiceClient manClient = SystemProperties.ManClient;
        //private static CodeServiceClient codeClient = SystemProperties.CodeClient;

       // private IList<CodeDao> useList;

        private IList<ManVo> selectedMenuViewList;
        private IList<ManVo> selectDtlItmList;
        //
        //private IList<ManVo> selectDtPopupList;

        //Menu Dialog
        private ICommand searchDialogCommand;
        //private ICommand newDialog1Command;
        //private ICommand newDialog2Command;
        //private ICommand editDialogCommand;
        //private ICommand delDialogCommand;


        //private ICommand findDialogCommand;

        //private ICommand fileDownloadCommand;

        //private ICommand classDialogCommand;

        //private M6631Master1Dialog master1Dialog;
        //private M6631Master2Dialog master2Dialog;

        //private M6631DetailDialog detailDialog;


        public M7711ViewModel() 
        {
            StartDt = System.DateTime.Now;

            Refresh();
        }

        async void Refresh( string _LOT_NO = null)
        {
            try
            {
                SelectDtlItmList = null;
                SearchDetailJob = null;

                //제품 목록
                using (HttpResponseMessage responseX = await SystemProperties.PROGRAM_HTTP.PostAsync("m7711/mst", new StringContent(JsonConvert.SerializeObject(new ManVo() { FM_DT = (StartDt).ToString("yyyy-MM-dd"), TO_DT = (StartDt).ToString("yyyy-MM-dd"), CHNL_CD = SystemProperties.USER_VO.CHNL_CD, ITM_CLSS_CD = "100" }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (responseX.IsSuccessStatusCode)
                    {
                        this.SelectedMenuViewList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await responseX.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();

                        //SelectedMenuViewList = manClient.M6631SelectMaster(new ManVo() {FM_DT =   (StartDt).ToString("yyyy-MM-dd"), TO_DT =  (StartDt).ToString("yyyy-MM-dd"), CHNL_CD = SystemProperties.USER_VO.CHNL_CD , ITM_CLSS_CD = "100" });
                        //SearchMenuContact();
                        if (SelectedMenuViewList.Count > 0)
                        {
                            isM_UPDATE = true;
                            isM_DELETE = true;

                            if (string.IsNullOrEmpty(_LOT_NO))
                            {
                                SelectedMenuItem = SelectedMenuViewList[SelectedMenuViewList.Count -1];
                            }
                            else
                            {
                                SelectedMenuItem = SelectedMenuViewList.Where(x => x.LOT_NO.Equals(_LOT_NO)).LastOrDefault<ManVo>();
                            }
                        }
                        else
                        {
                            SelectDtlItmList = null;
                            SearchDetailJob = null;

                            isM_UPDATE = false;
                            isM_DELETE = false;
                        }
                    }
                }
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }

        #region Functon (Menu Add, Edit, Del)
        #region MyRegion

        //private IList<CodeDao> _AreaCd = new List<CodeDao>();
        //public IList<CodeDao> AreaList
        //{
        //    get { return _AreaCd; }
        //    set { SetProperty(ref _AreaCd, value, () => AreaList); }
        //}

        //private CodeDao _M_AREA_ITEM;
        //public CodeDao M_AREA_ITEM
        //{
        //    get { return _M_AREA_ITEM; }
        //    set { SetProperty(ref _M_AREA_ITEM, value, () => M_AREA_ITEM, N1stList); }
        //}

        //void N1stList()
        //{
        //    if (M_AREA_ITEM == null)
        //    {
        //        return;
        //    }

        //    IList<SystemCodeVo> N1stVoList = codeClient.SelectCodeItemGroupList(new SystemCodeVo() { DELT_FLG = "N", ITM_GRP_CLSS_CD = M_AREA_ITEM.CLSS_CD, CRE_USR_ID = "", CHNL_CD = SystemProperties.USER_VO.CHNL_CD });
        //    int nCnt = N1stVoList.Count;
        //    SystemCodeVo tmpVo;

        //    N1ST_ITM_GRP_LIST = new ObservableCollection<CodeDao>();
        //    for (int x = 0; x < nCnt; x++)
        //    {
        //        tmpVo = N1stVoList[x];
        //        N1ST_ITM_GRP_LIST.Add(new CodeDao() { CLSS_DESC = tmpVo.ITM_GRP_NM, CLSS_CD = tmpVo.ITM_GRP_CD });
        //    }
        //    //
        //    if (nCnt > 0)
        //    {
        //        M_N1ST_ITM_GRP_ITEM = N1ST_ITM_GRP_LIST[0];
        //    }
        //}

        //public ObservableCollection<CodeDao> N1ST_ITM_GRP_LIST
        //{
        //    get { return itemN1st; }
        //    set { SetProperty(ref itemN1st, value, () => N1ST_ITM_GRP_LIST); }
        //}

        //private CodeDao _M_N1ST_ITM_GRP_ITEM;
        //public CodeDao M_N1ST_ITM_GRP_ITEM
        //{
        //    get { return _M_N1ST_ITM_GRP_ITEM; }
        //    set { SetProperty(ref _M_N1ST_ITM_GRP_ITEM, value, () => M_N1ST_ITM_GRP_ITEM); }
        //}

        #endregion

        DateTime _startDt;
        public DateTime StartDt
        {
            get { return _startDt; }
            set { SetProperty(ref _startDt, value, () => StartDt); }
        }

        //
        public IList<ManVo> SelectedMenuViewList
        {
            get { return selectedMenuViewList; }
            set { SetProperty(ref selectedMenuViewList, value, () => SelectedMenuViewList); }
        }

        ManVo _selectMenuItem;
        public ManVo SelectedMenuItem
        {
            get
            {
                return _selectMenuItem;
            }
            set
            {
                if (value != null)
                {
                    SetProperty(ref _selectMenuItem, value, () => SelectedMenuItem, SearchDetailItem);
                    //SearchDetailItem();
                }
            }
        }

        private async void SearchDetailItem()
        {
            if (SelectedMenuItem == null)
            {
                return;
            }

            //
            using (HttpResponseMessage responseX = await SystemProperties.PROGRAM_HTTP.PostAsync("m7711/dtl", new StringContent(JsonConvert.SerializeObject(SelectedMenuItem), System.Text.Encoding.UTF8, "application/json")))
            {
                if (responseX.IsSuccessStatusCode)
                {
                    this.SelectDtlItmList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await responseX.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();

                    //SelectDtlItmList = manClient.M6631SelectDetail(SelectedMenuItem);
                    if (SelectDtlItmList.Count > 0)
                    {
                        //isM_UPDATE = true;
                        //isM_DELETE = true;

                        SearchDetailJob = SelectDtlItmList[0];
                    }
                    else
                    {

                        //isM_UPDATE = false;
                        //isM_DELETE = false;

                    }
                }
            }
        }

        ////계획 번호 조회
        //public IList<ManVo> SelectDtlPopupList
        //{
        //    get { return selectDtPopupList; }
        //    set { SetProperty(ref selectDtPopupList, value, () => SelectDtlPopupList); }
        //}


        public IList<ManVo> SelectDtlItmList
        {
            get { return selectDtlItmList; }
            set { SetProperty(ref selectDtlItmList, value, () => SelectDtlItmList); }
        }


        ManVo _searchDetailJob;
        public ManVo SearchDetailJob
        {
            get
            {
                return _searchDetailJob;
            }
            set
            {
                if (value != null)
                {
                    SetProperty(ref _searchDetailJob, value, () => SearchDetailJob);
                }
            }
        }

        public ICommand SearchDialogCommand
        {
            get
            {
                if (searchDialogCommand == null)
                    searchDialogCommand = new DelegateCommand(SearchMenuContact);
                return searchDialogCommand;
            }
        }

        public void SearchMenuContact()
        {
            try
            {
                DXSplashScreen.Show<ProgressWindow>();
                Refresh();
                DXSplashScreen.Close();
            }
            catch
            {
                if (DXSplashScreen.IsActive == true)
                {
                    DXSplashScreen.Close();
                }
                return;
            }
        }

        #region MyRegion
        //public ICommand NewDialog1Command
        //{
        //    get
        //    {
        //        if (newDialog1Command == null)
        //            newDialog1Command = new DelegateCommand(NewContact1);
        //        return newDialog1Command;
        //    }
        //}

        //public void NewContact1()
        //{
        //    //if (M_AREA_ITEM == null)
        //    //{
        //    //    return;
        //    //}

        //    master1Dialog = new M6631Master1Dialog(new ManVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD });
        //    master1Dialog.Title = _title + " - (벌크) 추가";
        //    master1Dialog.Owner = Application.Current.MainWindow;
        //    master1Dialog.BorderEffect = BorderEffect.Default;
        //    //masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
        //    //masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
        //    bool isDialog = (bool)master1Dialog.ShowDialog();
        //    if (isDialog)
        //    {
        //        //if (masterDialog.IsEdit == false)
        //        //{
        //        Refresh();

        //            //for (int x = 0; x < SelectedMenuViewList.Count; x++)
        //            //{
        //            //    if ((SelectedMenuViewList[x].ASSY_ITM_CD + "_" + SelectedMenuViewList[x].BSE_WEIH_VAL).Equals(masterDialog.resultVo.ASSY_ITM_CD + "_" + masterDialog.resultVo.BSE_WEIH_VAL))
        //            //    {
        //            //        SelectedMenuItem = SelectedMenuViewList[x];
        //            //        break;
        //            //    }
        //            //}
        //        //}
        //    }
        //}

        //public ICommand NewDialog2Command
        //{
        //    get
        //    {
        //        if (newDialog2Command == null)
        //            newDialog2Command = new DelegateCommand(NewContact2);
        //        return newDialog2Command;
        //    }
        //}

        //public void NewContact2()
        //{
        //    //if (M_AREA_ITEM == null)
        //    //{
        //    //    return;
        //    //}

        //    master2Dialog = new M6631Master2Dialog(new ManVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD });
        //    master2Dialog.Title = _title + " - (가공원료) 추가";
        //    master2Dialog.Owner = Application.Current.MainWindow;
        //    master2Dialog.BorderEffect = BorderEffect.Default;
        //    //masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
        //    //masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
        //    bool isDialog = (bool)master2Dialog.ShowDialog();
        //    if (isDialog)
        //    {
        //        //if (masterDialog.IsEdit == false)
        //        //{
        //        Refresh();

        //        //for (int x = 0; x < SelectedMenuViewList.Count; x++)
        //        //{
        //        //    if ((SelectedMenuViewList[x].ASSY_ITM_CD + "_" + SelectedMenuViewList[x].BSE_WEIH_VAL).Equals(masterDialog.resultVo.ASSY_ITM_CD + "_" + masterDialog.resultVo.BSE_WEIH_VAL))
        //        //    {
        //        //        SelectedMenuItem = SelectedMenuViewList[x];
        //        //        break;
        //        //    }
        //        //}
        //        //}
        //    }
        //}


        //public ICommand EditDialogCommand
        //{
        //    get
        //    {
        //        if (editDialogCommand == null)
        //            editDialogCommand = new DelegateCommand(EditMasterContact);
        //        return editDialogCommand;
        //    }
        //}

        //public void EditMasterContact()
        //{
        //    if (SelectedMenuItem == null) { return; }
        //    master1Dialog = new M6631Master1Dialog(SelectedMenuItem);
        //    master1Dialog.Title = _title + " - 수정";
        //    master1Dialog.Owner = Application.Current.MainWindow;
        //    master1Dialog.BorderEffect = BorderEffect.Default;
        //    //masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
        //    //masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
        //    bool isDialog = (bool)master1Dialog.ShowDialog();
        //    if (isDialog)
        //    {

        //        Refresh( master1Dialog.updateDao.LOT_NO);

        //        //if (masterDialog.IsEdit == true)
        //        //{
        //        //  SelectedMenuItem.DELT_FLG = SelectedMenuItem.DELT_FLG;
        //        //}
        //    }

        //}

        //public ICommand DelDialogCommand
        //{
        //    get
        //    {
        //        if (delDialogCommand == null)
        //            delDialogCommand = new DelegateCommand(DelMasterContact);
        //        return delDialogCommand;
        //    }
        //}


        //public async void DelMasterContact()
        //{
        //    ManVo delDao = SelectedMenuItem;
        //    if (delDao != null)
        //    {
        //        //this.useList = SystemProperties.SYSTEM_CODE_VO("A-002");
        //        //if (!(this.useList.FindIndex(x => x.CLSS_CD.ToLower().Equals(SystemProperties.USER.ToLower())) >= 0))
        //        //{
        //            SearchDetailItem();
        //            if (SelectDtlItmList.Count > 0)
        //            {
        //                if(SelectDtlItmList.Any<ManVo>(x => Convert.ToDecimal(x.WEIH_VAL) > 0))
        //                {
        //                    WinUIMessageBox.Show("칭량이 완료 되었습니다.", "[삭제X] " + _title, MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.None, MessageBoxOptions.None);
        //                    return;
        //                }

        //                //for (int x = 0; x < SelectDtlItmList.Count; x++)
        //                //{
        //                //    if (Convert.ToDecimal(SelectDtlItmList[x].WEIH_VAL) > 0 )
        //                //    {
        //                //        WinUIMessageBox.Show("칭량이 완료 되었습니다.", "[삭제X] " + _title, MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.None, MessageBoxOptions.None);
        //                //        return;
        //                //    }
        //                //}
        //            }
        //        //}

        //        MessageBoxResult result = WinUIMessageBox.Show(delDao.LOT_NO + " 정말로 삭제 하시겠습니까?", "[삭제]" + _title, MessageBoxButton.YesNo, MessageBoxImage.Question);
        //        if (result == MessageBoxResult.Yes)
        //        {
        //            //    //ManVo resultVo = manClient.M6631DeleteDetail(delDao);
        //            //    if (!resultVo.isSuccess)
        //            //    {
        //            //        //실패
        //            //        WinUIMessageBox.Show(resultVo.Message, "[" + SystemProperties.PROGRAM_TITLE + "] 칭량 작업 계획 / 지시", MessageBoxButton.OK, MessageBoxImage.Error);
        //            //        return;
        //            //    }


        //            //    resultVo = manClient.M6631DeleteDetail2(delDao);
        //            //    if (!resultVo.isSuccess)
        //            //    {
        //            //        //실패
        //            //        WinUIMessageBox.Show(resultVo.Message, "[" + SystemProperties.PROGRAM_TITLE + "] 칭량 작업 계획 / 지시", MessageBoxButton.OK, MessageBoxImage.Error);
        //            //        return;
        //            //    }

        //            //    SearchDetailItem();
        //            //    resultVo = manClient.M6631DeleteMaster(delDao);
        //            //    if (!resultVo.isSuccess)
        //            //    {
        //            //        //실패
        //            //        WinUIMessageBox.Show(resultVo.Message, "[" + SystemProperties.PROGRAM_TITLE + "] 칭량 작업 계획 / 지시", MessageBoxButton.OK, MessageBoxImage.Error);
        //            //        return;
        //            //    }
        //            //    SearchMenuContact();


        //            using (HttpResponseMessage responseX = await SystemProperties.PROGRAM_HTTP.PostAsync("m6631/dtl/d", new StringContent(JsonConvert.SerializeObject(delDao), System.Text.Encoding.UTF8, "application/json")))
        //            {
        //                if (responseX.IsSuccessStatusCode)
        //                {
        //                    int _Num = 0;
        //                    string resultMsg = await responseX.Content.ReadAsStringAsync();
        //                    if (int.TryParse(resultMsg, out _Num) == false)
        //                    {
        //                        //실패
        //                        WinUIMessageBox.Show(resultMsg, _title, MessageBoxButton.OK, MessageBoxImage.Error);
        //                        return;
        //                    }

        //                    //MST
        //                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6631/mst/d", new StringContent(JsonConvert.SerializeObject(delDao), System.Text.Encoding.UTF8, "application/json")))
        //                    {
        //                        if (response.IsSuccessStatusCode)
        //                        {
        //                            _Num = 0;
        //                            resultMsg = await response.Content.ReadAsStringAsync();
        //                            if (int.TryParse(resultMsg, out _Num) == false)
        //                            {
        //                                //실패
        //                                WinUIMessageBox.Show(resultMsg, _title, MessageBoxButton.OK, MessageBoxImage.Error);
        //                                return;
        //                            }
        //                            Refresh();

        //                            //성공
        //                            WinUIMessageBox.Show("삭제가 완료되었습니다.", _title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}



        //public ICommand FindDialogCommand
        //{
        //    get
        //    {
        //        if (findDialogCommand == null)
        //            findDialogCommand = new DelegateCommand(FindContact);
        //        return findDialogCommand;
        //    }
        //}

        //public void FindContact()
        //{
        //    //////제품명 / 수량 / 조회
        //    //using (HttpResponseMessage responseX = await SystemProperties.PROGRAM_HTTP.PostAsync("m6630/dtl", new StringContent(JsonConvert.SerializeObject(new ManVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD, PROD_PLN_NO = SelectedMenuItem.SL_ORD_NO }), System.Text.Encoding.UTF8, "application/json")))
        //    //{
        //    //    if (responseX.IsSuccessStatusCode)
        //    //    {
        //    //      this.SelectDtlPopupList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await responseX.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
        //    //    }
        //    //}
        //    if (SelectedMenuItem == null) { return; }

        //    detailDialog = new M6631DetailDialog(new ManVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD, PROD_PLN_NO = SelectedMenuItem.SL_ORD_NO });
        //    detailDialog.Title = _title + " - " + SelectedMenuItem.SL_ORD_NO;
        //    detailDialog.Owner = Application.Current.MainWindow;
        //    detailDialog.BorderEffect = BorderEffect.Default;
        //    //masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
        //    //masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
        //    bool isDialog = (bool)detailDialog.ShowDialog();
        //    if (isDialog)
        //    {
        //    }

        //}

        #endregion

        #endregion


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
    }
}
