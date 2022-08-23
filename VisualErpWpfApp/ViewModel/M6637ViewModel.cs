using AquilaErpWpfApp3.Util;
using AquilaErpWpfApp3.View.M.Dialog;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
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
    public sealed class M6637ViewModel : ViewModelBase, INotifyPropertyChanged
    {

        //private BarPrint _barCode = new BarPrint();
        private string _title = "칭량 작업(추가/폐기)";

        //private static ManServiceClient manClient = SystemProperties.ManClient;
        //private static CodeServiceClient codeClient = SystemProperties.CodeClient;

        //private BarPrint _barPrint = new BarPrint();

        private IList<ManVo> selectedMenuViewList;
        private IList<ManVo> selectDtlItmList;
        private IList<ManVo> selectItmList;

        //Menu Dialog
        private ICommand searchDialogCommand;
        //private ICommand newDialogCommand;
        //private ICommand editDialogCommand;
        //private ICommand delDialogCommand;

        private ICommand _configDialogCommand;
        //private ICommand _weighingDialogCommand;

        //private ICommand _barCodeDialogCommand;
        //private ICommand _barCodeConfigDialogCommand;

        //private ICommand classDialogCommand;

        private M6637MasterDialog masterDialog;
        //  private M6631MasterDialog masterDialog;
        private M6632WeighingConfigDialog configWeighingDialog;
        //private M6632WeighingDialog weighingDialog;

        //Print
        //private M6632WeighingPrintDialog weighingPrintDialog;
        //private M6632ProducePrintDialog producePrintDialog;
        // private M6632BarCodeDialog barCodeDialog;

        public M6637ViewModel()
        {
            StartDt = System.DateTime.Now;

            Refresh();
        }

        async void Refresh(string _LOT_NO = null)
        {
            try
            {
                SelectDtlItmList = null;
                SearchDetailJob = null;

                //제품 목록
                using (HttpResponseMessage responseX = await SystemProperties.PROGRAM_HTTP.PostAsync("m6631/mst", new StringContent(JsonConvert.SerializeObject(new ManVo() { FM_DT = (StartDt).ToString("yyyy-MM-dd"), TO_DT = (StartDt).ToString("yyyy-MM-dd"), CHNL_CD = SystemProperties.USER_VO.CHNL_CD, ITM_CLSS_CD = "100" , INP_LOT_NO = "A" }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (responseX.IsSuccessStatusCode)
                    {
                        this.SelectedMenuViewList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await responseX.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
                    }

                    //SelectedMenuViewList = manClient.M6631SelectMaster(new ManVo() {FM_DT =   (StartDt).ToString("yyyy-MM-dd"), TO_DT =  (StartDt).ToString("yyyy-MM-dd"), CHNL_CD = SystemProperties.USER_VO.CHNL_CD , ITM_CLSS_CD = "100" });
                    //SearchMenuContact();
                    if (SelectedMenuViewList.Count > 0)
                    {
                        //isM_UPDATE = true;
                        //isM_DELETE = true;

                        if (string.IsNullOrEmpty(_LOT_NO))
                        {
                            SelectedMenuItem = SelectedMenuViewList[0];
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

                        //isM_UPDATE = false;
                        //isM_DELETE = false;
                    }
                }

                //SelectedMenuViewList = manClient.M6631SelectMaster(new ManVo() { FM_DT = (StartDt).ToString("yyyy-MM-dd"), TO_DT = (StartDt).ToString("yyyy-MM-dd"), CHNL_CD = SystemProperties.USER_VO.CHNL_CD, ITM_CLSS_CD = "100" });
                ////SearchMenuContact();
                //if (SelectedMenuViewList.Count > 0)
                //{
                //    isM_UPDATE = true;
                //    isM_DELETE = true;
                //    SelectedMenuItem = SelectedMenuViewList[0];
                //}
                //else
                //{
                //    SelectDtlItmList = null;
                //    SearchDetailJob = null;

                //    isM_UPDATE = false;
                //    isM_DELETE = false;
                //}
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }

        #region Functon (Menu Add, Edit, Del)
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

            //if (SelectedMenuItem.WRK_END_FLG.Equals("예"))
            //{
            //    isM_OK = false;
            //    isM_CANCEL = true;
            //}
            //else
            //{
            //    isM_OK = true;
            //    isM_CANCEL = false;
            //}

            using (HttpResponseMessage responseX = await SystemProperties.PROGRAM_HTTP.PostAsync("m6631/dtl", new StringContent(JsonConvert.SerializeObject(SelectedMenuItem), System.Text.Encoding.UTF8, "application/json")))
            {
                if (responseX.IsSuccessStatusCode)
                {
                    this.SelectDtlItmList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await responseX.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
                }

                //SelectDtlItmList = manClient.M6631SelectDetail(SelectedMenuItem);
                if (SelectDtlItmList.Count > 0)
                {
                    isD_UPDATE = true;
                    //isM_DELETE = true;

                    SearchDetailJob = SelectDtlItmList[0];
                }
                else
                {

                    isD_UPDATE = false;
                    //isM_DELETE = false;

                }
            }

            //SelectDtlItmList = manClient.M6631SelectDetail(SelectedMenuItem);
            //if (SelectDtlItmList.Count > 0)
            //{
            //    isD_UPDATE = true;
            //    //isM_DELETE = true;

            //    SearchDetailJob = SelectDtlItmList[0];
            //}
            //else
            //{

            //    isD_UPDATE = false;
            //    //isM_DELETE = false;

            //}
        }

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
                    SetProperty(ref _searchDetailJob, value, () => SearchDetailJob, SearchItem);
                }
            }
        }





        private async void SearchItem()
        {
            if (SearchDetailJob == null)
            {
                return;
            }

            //if (SelectedMenuItem.WRK_END_FLG.Equals("예"))
            //{
            //    isM_OK = false;
            //    isM_CANCEL = true;
            //}
            //else
            //{
            //    isM_OK = true;
            //    isM_CANCEL = false;
            //}

            using (HttpResponseMessage responseX = await SystemProperties.PROGRAM_HTTP.PostAsync("m6637/mst", new StringContent(JsonConvert.SerializeObject(SearchDetailJob), System.Text.Encoding.UTF8, "application/json")))
            {
                if (responseX.IsSuccessStatusCode)
                {
                    this.SelectItmList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await responseX.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
                }

                //SelectDtlItmList = manClient.M6631SelectDetail(SelectedMenuItem);
                if (SelectItmList.Count > 0)
                {
                    isD_DELETE = true;
                    //isM_DELETE = true;

                    SearchItm = SelectItmList[0];
                }
                else
                {

                    isD_DELETE = false;
                    //isM_DELETE = false;

                }
            }

            //SelectDtlItmList = manClient.M6631SelectDetail(SelectedMenuItem);
            //if (SelectDtlItmList.Count > 0)
            //{
            //    isD_UPDATE = true;
            //    //isM_DELETE = true;

            //    SearchDetailJob = SelectDtlItmList[0];
            //}
            //else
            //{

            //    isD_UPDATE = false;
            //    //isM_DELETE = false;

            //}
        }


        public IList<ManVo> SelectItmList
        {
            get { return selectItmList; }
            set { SetProperty(ref selectItmList, value, () => SelectItmList); }
        }


        ManVo _searchItm;
        public ManVo SearchItm
        {
            get
            {
                return _searchItm;
            }
            set
            {
                if (value != null)
                {
                    SetProperty(ref _searchItm, value, () => SearchItm);
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

      
        public ICommand ConfigDialogCommand
        {
            get
            {
                if (_configDialogCommand == null)
                    _configDialogCommand = new DelegateCommand(ConfigContact);
                return _configDialogCommand;
            }
        }

        public void ConfigContact()
        {
            configWeighingDialog = new M6632WeighingConfigDialog();
            configWeighingDialog.Title = "칭량 설정";
            configWeighingDialog.Owner = Application.Current.MainWindow;
            configWeighingDialog.BorderEffect = BorderEffect.Default;
            ////masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            ////masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            bool isDialog = (bool)configWeighingDialog.ShowDialog();
            if (isDialog)
            {

            }
        }

        
        [Command]
        public async void Apply(string obj)
        {
            try
            {
                //추가 칭량
                if(SearchDetailJob == null)
                {
                    return;
                }


                SearchDetailJob.ROUT_CD = "100";
                SearchDetailJob.ROUT_NM = "추가 칭량";

                if (obj.Equals("B"))
                {
                    masterDialog = new M6637MasterDialog(new ManVo() { ROUT_CD = SearchDetailJob.ROUT_CD, ROUT_NM = SearchDetailJob.ROUT_NM, CHNL_CD = SystemProperties.USER_VO.CHNL_CD });
                    masterDialog.Title = _title + " - 추가 칭량 (바코드)";
                    masterDialog.Owner = Application.Current.MainWindow;
                    masterDialog.BorderEffect = BorderEffect.Default;
                    //masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
                    //masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
                    bool isDialog = (bool)masterDialog.ShowDialog();
                }
                else
                {
                    int? _assy_itm_seq = SearchDetailJob.ASSY_ITM_SEQ;

                    masterDialog = new M6637MasterDialog(SearchDetailJob);
                    masterDialog.Title = _title + " - 추가 칭량";
                    masterDialog.Owner = Application.Current.MainWindow;
                    masterDialog.BorderEffect = BorderEffect.Default;
                    //masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
                    //masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
                    bool isDialog = (bool)masterDialog.ShowDialog();
                    if (isDialog)
                    {
                        //
                        //SearchDetailItem();
                        if (SelectedMenuItem == null)
                        {
                            return;
                        }

                        using (HttpResponseMessage responseX = await SystemProperties.PROGRAM_HTTP.PostAsync("m6631/dtl", new StringContent(JsonConvert.SerializeObject(SelectedMenuItem), System.Text.Encoding.UTF8, "application/json")))
                        {
                            if (responseX.IsSuccessStatusCode)
                            {
                                this.SelectDtlItmList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await responseX.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
                                //
                                if (SelectDtlItmList.Count > 0)
                                {
                                    isD_UPDATE = true;

                                    SearchDetailJob = SelectDtlItmList.Where(x => x.ASSY_ITM_SEQ == _assy_itm_seq).LastOrDefault<ManVo>();

                                    //SearchItm = SelectedMenuViewList.LastOrDefault<ManVo>();
                                }
                                else
                                {
                                    isD_UPDATE = false;
                                }
                            }
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


        [Command]
        public async void Cancel(string obj)
        {
            try
            {
                //폐기
                if (SearchDetailJob == null)
                {
                    return;
                }

                //
                SearchDetailJob.ROUT_CD = "200";
                SearchDetailJob.ROUT_NM = "폐기";

                if (obj.Equals("B"))
                {
                    masterDialog = new M6637MasterDialog(new ManVo() { ROUT_CD = SearchDetailJob.ROUT_CD, ROUT_NM = SearchDetailJob.ROUT_NM, CHNL_CD = SystemProperties.USER_VO.CHNL_CD });
                    masterDialog.Title = _title + " - 폐기 (바코드)";
                    masterDialog.Owner = Application.Current.MainWindow;
                    masterDialog.BorderEffect = BorderEffect.Default;
                    //masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
                    //masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
                    bool isDialog = (bool)masterDialog.ShowDialog();
                }
                else
                {
                    int? _assy_itm_seq = SearchDetailJob.ASSY_ITM_SEQ;

                    masterDialog = new M6637MasterDialog(SearchDetailJob);
                    masterDialog.Title = _title + " - 폐기";
                    masterDialog.Owner = Application.Current.MainWindow;
                    masterDialog.BorderEffect = BorderEffect.Default;
                    //masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
                    //masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
                    bool isDialog = (bool)masterDialog.ShowDialog();
                    if (isDialog)
                    {
                        //
                        //SearchDetailItem();
                        if (SelectedMenuItem == null)
                        {
                            return;
                        }
                    
                        using (HttpResponseMessage responseX = await SystemProperties.PROGRAM_HTTP.PostAsync("m6631/dtl", new StringContent(JsonConvert.SerializeObject(SelectedMenuItem), System.Text.Encoding.UTF8, "application/json")))
                        {
                            if (responseX.IsSuccessStatusCode)
                            {
                                this.SelectDtlItmList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await responseX.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
                                //
                                if (SelectDtlItmList.Count > 0)
                                {
                                    isD_UPDATE = true;

                                    SearchDetailJob = SelectDtlItmList.Where(x => x.ASSY_ITM_SEQ == _assy_itm_seq).LastOrDefault<ManVo>();

                                    //SearchItm = SelectedMenuViewList.LastOrDefault<ManVo>();
                                }
                                else
                                {
                                    isD_UPDATE = false;
                                }
                            }
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




        [Command]
        public async void Delete()
        {
            try
            {
                //삭제
                if (SearchItm == null)
                {
                    return;
                }

                MessageBoxResult result = WinUIMessageBox.Show("[" + SearchItm.ROUT_NM + "/" + SearchItm.WEIH_SEQ + "] 정말로 삭제 하시겠습니까?", _title, MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6637/mst/d", new StringContent(JsonConvert.SerializeObject(SearchItm), System.Text.Encoding.UTF8, "application/json")))
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

                            SearchItem();

                            //성공
                            WinUIMessageBox.Show("삭제가 완료되었습니다.", _title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
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


        #endregion


        //private bool? _isM_UPDATE = false;
        //public bool? isM_UPDATE
        //{
        //    get { return _isM_UPDATE; }
        //    set { SetProperty(ref _isM_UPDATE, value, () => isM_UPDATE); }
        //}

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


        //private bool? _isM_OK = false;
        //public bool? isM_OK
        //{
        //    get { return _isM_OK; }
        //    set { SetProperty(ref _isM_OK, value, () => isM_OK); }
        //}


        //private bool? _isM_CANCEL = false;
        //public bool? isM_CANCEL
        //{
        //    get { return _isM_CANCEL; }
        //    set { SetProperty(ref _isM_CANCEL, value, () => isM_CANCEL); }
        //}






        private string _M_SEARCH_TEXT = string.Empty;
        public string M_SEARCH_TEXT
        {
            get { return _M_SEARCH_TEXT; }
            set { SetProperty(ref _M_SEARCH_TEXT, value, () => M_SEARCH_TEXT); }
        }
    }
}
