using AquilaErpWpfApp3.Util;
using AquilaErpWpfApp3.View.M.Dialog;
using AquilaErpWpfApp3.View.M.Report;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using DevExpress.XtraReports.UI;
using ModelsLibrary.Man;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace AquilaErpWpfApp3.ViewModel
{
    public sealed class M6633ViewModel : ViewModelBase, INotifyPropertyChanged
    {

        //private BarPrint _barCode = new BarPrint();
        private string _title = "칭량 작업";

        //private static ManServiceClient manClient = SystemProperties.ManClient;
        //private static CodeServiceClient codeClient = SystemProperties.CodeClient;

        private BarPrint _barPrint = new BarPrint();

        private IList<ManVo> selectedMenuViewList;
        private IList<ManVo> selectDtlItmList;

        //Menu Dialog
        private ICommand searchDialogCommand;
        //private ICommand newDialogCommand;
        //private ICommand editDialogCommand;
        //private ICommand delDialogCommand;

        private ICommand _configDialogCommand;
        private ICommand _weighingDialogCommand;

        private ICommand _barCodeDialogCommand;
        private ICommand _barCodeConfigDialogCommand;

        //private ICommand classDialogCommand;

      //  private M6631MasterDialog masterDialog;
        private M6632WeighingConfigDialog configWeighingDialog;
        private M6632WeighingDialog weighingDialog;

        private M6631DetailDialog detailDialog;

        private M6632WeighingUserDialog weighingUserDialog;

        //Print
        private M6632WeighingPrintDialog weighingPrintDialog;
        private M6632ProducePrintDialog producePrintDialog;
        // private M6632BarCodeDialog barCodeDialog;

        public M6633ViewModel()
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
                using (HttpResponseMessage responseX = await SystemProperties.PROGRAM_HTTP.PostAsync("m6631/mst", new StringContent(JsonConvert.SerializeObject(new ManVo() { FM_DT = (StartDt).ToString("yyyy-MM-dd"), TO_DT = (StartDt).ToString("yyyy-MM-dd"), CHNL_CD = SystemProperties.USER_VO.CHNL_CD, ITM_CLSS_CD = "200" , INP_LOT_NO = "A" }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (responseX.IsSuccessStatusCode)
                    {
                        this.SelectedMenuViewList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await responseX.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
                    }

                    //SelectedMenuViewList = manClient.M6631SelectMaster(new ManVo() {FM_DT =   (StartDt).ToString("yyyy-MM-dd"), TO_DT =  (StartDt).ToString("yyyy-MM-dd"), CHNL_CD = SystemProperties.USER_VO.CHNL_CD , ITM_CLSS_CD = "100" });
                    //SearchMenuContact();
                    if (SelectedMenuViewList.Count > 0)
                    {
                        isM_UPDATE = true;
                        isM_DELETE = true;

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

                        isM_UPDATE = false;
                        isM_DELETE = false;
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


            //프로그래스 바 시작
            if (DXSplashScreen.IsActive == false)
            {
                DXSplashScreen.Show<ProgressWindow>();
            }

            if (SelectedMenuItem.WRK_END_FLG.Equals("예"))
            {
                isM_OK = false;
                isM_CANCEL = true;
            }
            else
            {
                isM_OK = true;
                isM_CANCEL = false;
            }

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


            //프로그래스 바 종료
            if (DXSplashScreen.IsActive == true)
            {
                DXSplashScreen.Close();
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

        //public ICommand NewDialogCommand
        //{
        //    get
        //    {
        //        if (newDialogCommand == null)
        //            newDialogCommand = new DelegateCommand(NewContact);
        //        return newDialogCommand;
        //    }
        //}

        //public void NewContact()
        //{
        //    //if (M_AREA_ITEM == null)
        //    //{
        //    //    return;
        //    //}

        //    masterDialog = new M6631MasterDialog(new ManVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD });
        //    masterDialog.Title = _title + " - 추가";
        //    masterDialog.Owner = Application.Current.MainWindow;
        //    masterDialog.BorderEffect = BorderEffect.Default;
        //    //masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
        //    //masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
        //    bool isDialog = (bool)masterDialog.ShowDialog();
        //    if (isDialog)
        //    {
        //        if (masterDialog.IsEdit == false)
        //        {
        //            SearchMenuContact();

        //            //for (int x = 0; x < SelectedMenuViewList.Count; x++)
        //            //{
        //            //    if ((SelectedMenuViewList[x].ASSY_ITM_CD + "_" + SelectedMenuViewList[x].BSE_WEIH_VAL).Equals(masterDialog.resultVo.ASSY_ITM_CD + "_" + masterDialog.resultVo.BSE_WEIH_VAL))
        //            //    {
        //            //        SelectedMenuItem = SelectedMenuViewList[x];
        //            //        break;
        //            //    }
        //            //}
        //        }
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
        //    masterDialog = new M6631MasterDialog(SelectedMenuItem);
        //    masterDialog.Title = _title + " - 수정";
        //    masterDialog.Owner = Application.Current.MainWindow;
        //    masterDialog.BorderEffect = BorderEffect.Default;
        //    //masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
        //    //masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
        //    bool isDialog = (bool)masterDialog.ShowDialog();
        //    if (isDialog)
        //    {
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


        //public void DelMasterContact()
        //{
        //    ManVo delDao = SearchDetailJob;
        //    if (delDao != null)
        //    {
        //        MessageBoxResult result = WinUIMessageBox.Show(delDao.LOT_NO + " 정말로 삭제 하시겠습니까?", "[삭제]" + _title, MessageBoxButton.YesNo, MessageBoxImage.Question);
        //        if (result == MessageBoxResult.Yes)
        //        {
        //            manClient.M6631DeleteDetail(delDao);
        //            SearchDetailItem();
        //            manClient.M6631DeleteMaster(delDao);

        //            WinUIMessageBox.Show("삭제가 완료되었습니다.", "[삭제]" + _title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
        //            SearchMenuContact();
        //        }
        //    }
        //}



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


        public ICommand BarCodeDialogCommand
        {
            get
            {
                if (_barCodeDialogCommand == null)
                    _barCodeDialogCommand = new DelegateCommand(BarCodeContact);
                return _barCodeDialogCommand;
            }
        }

        public void BarCodeContact()
        {
            try
            {
                if (SearchDetailJob != null)
                {
                    if(string.IsNullOrEmpty(SearchDetailJob.MTRL_LOT_NO))
                    {
                        WinUIMessageBox.Show("해당 원재료는 칭량이 완료 되지 않았습니다.", _title, MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.None, MessageBoxOptions.None);
                        return;
                    }

                    MessageBoxResult result = WinUIMessageBox.Show("[" + SearchDetailJob.PRN_LOT_NO + " / " + SearchDetailJob.ITM_NM + "] 바코드 출력 하시겠습니까?", "[바코드 - " + Properties.Settings.Default.str_PrnNm + "]" + _title, MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        //벌크 코드
                        //SearchDetailJob.MM_01 = SelectedMenuItem.ASSY_ITM_CD;
                        //제조 번호
                        //SearchDetailJob.MM_02 = SelectedMenuItem.INP_LOT_NO;

                        #region 2021-09-09 바코드 속도 개선
                        //강제 출력
                        //if (_barPrint.SmallPackingPrint_Godex(SearchDetailJob) == true)
                        //{
                        //    WinUIMessageBox.Show("완료되었습니다.", _title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                        //    return;
                        //} 
                        #endregion


                        M6633BarCodeReport barcodeReport = new M6633BarCodeReport( SearchDetailJob );
                        barcodeReport.CreateDocument(true);
                        barcodeReport.PrinterName = Properties.Settings.Default.str_PrnNm;
                        //barcodeReport.PrintingSystem.ShowMarginsWarning = false;
                        barcodeReport.Print();

                        //WinUIMessageBox.Show("완료되었습니다.", _title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                        //    return;
                        //    //    _barCode.M6632WeightPrint(SearchDetailJob);
                    }
                }
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }



        public ICommand BarCodeConfigDialogCommand
        {
            get
            {
                if (_barCodeConfigDialogCommand == null)
                    _barCodeConfigDialogCommand = new DelegateCommand(BarCodeConfig);
                return _barCodeConfigDialogCommand;
            }
        }

        public void BarCodeConfig()
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
                WinUIMessageBox.Show(eLog.Message, "[에러]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }




        public ICommand WeighingDialogCommand
        {
            get
            {
                if (_weighingDialogCommand == null)
                    _weighingDialogCommand = new DelegateCommand(WeighingContact);
                return _weighingDialogCommand;
            }
        }

        public void WeighingContact()
        {

            try
            {
                if (SelectDtlItmList == null)
                {
                    return;
                }

                if (SelectDtlItmList.Count > 0)
                {

                    //로그인 인증
                    weighingUserDialog = new M6632WeighingUserDialog(SelectedMenuItem);
                    weighingUserDialog.Title = "칭량 - " + SelectedMenuItem.LOT_NO;
                    weighingUserDialog.Owner = Application.Current.MainWindow;
                    weighingUserDialog.BorderEffect = BorderEffect.Default;
                    weighingUserDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
                    weighingUserDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
                    if (weighingUserDialog.ShowDialog() == true)
                    {
                        //사용자 확인 후 칭량
                        //
                        //칭량 작업

                        weighingDialog = new M6632WeighingDialog(SelectDtlItmList);
                        weighingDialog.Title = "칭량 [ " + SelectedMenuItem.LOT_NO + " ] - " + SelectedMenuItem.ASSY_ITM_CD + " / " + SelectedMenuItem.ASSY_ITM_NM;
                        weighingDialog.Owner = Application.Current.MainWindow;
                        weighingDialog.BorderEffect = BorderEffect.Default;
                        ////masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
                        ////masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
                        bool isDialog = (bool)weighingDialog.ShowDialog();
                        if (isDialog)
                        {
                            SearchDetailItem();
                            //string _LOT_NO = SelectedMenuItem.LOT_NO;

                            //SearchMenuContact();

                            ////x => x.LOT_NO.Equals(_LOT_NO)
                            //int nCnt = SelectedMenuViewList.IndexOf(SelectedMenuViewList.Where(x => x.LOT_NO.Equals(_LOT_NO)).LastOrDefault<ManVo>());
                            //if (nCnt >= 0)
                            //{
                            //    SelectedMenuItem = SelectedMenuViewList[nCnt];
                            //}

                            //for (int x = 0; x < SelectedMenuViewList.Count; x++)
                            //{
                            //    if ((SelectedMenuViewList[x].LOT_NO).Equals(_LOT_NO))
                            //    {
                            //        SelectedMenuItem = SelectedMenuViewList[x];
                            //        break;
                            //    }
                            //}

                            //SearchDetailItem();
                            //
                            //if (masterDialog.IsEdit == false)
                            //{
                            //}
                        }
                    }
                }
                else
                {
                }

            }
            catch (Exception)
            {
                return;
            }
        }



        [Command]
        public void WeighingPrintContact()
        {
            try
            {
                if (SelectDtlItmList == null)
                {
                    return;
                }

                if (SelectDtlItmList.Count > 0)
                {
                    SelectDtlItmList[0].LOT_NO = SelectedMenuItem.LOT_NO;
                    SelectDtlItmList[0].WRK_MAN_NM = SelectedMenuItem.WRK_MAN_NM;
                    SelectDtlItmList[0].ASSY_ITM_CD = SelectedMenuItem.ASSY_ITM_CD;
                    SelectDtlItmList[0].ASSY_ITM_NM = SelectedMenuItem.ASSY_ITM_NM;
                    SelectDtlItmList[0].WRK_DT = SelectedMenuItem.WRK_DT;
                    SelectDtlItmList[0].MIX_WEIH_VAL = SelectedMenuItem.MIX_WEIH_VAL;


                    weighingPrintDialog = new M6632WeighingPrintDialog(SelectDtlItmList);
                    weighingPrintDialog.Title = "칭량 지시 및 기록서 - [" + SelectedMenuItem.LOT_NO + "]";
                    weighingPrintDialog.Owner = Application.Current.MainWindow;
                    weighingPrintDialog.BorderEffect = BorderEffect.Default;
                    ////masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
                    ////masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
                    bool isDialog = (bool)weighingPrintDialog.ShowDialog();
                    if (isDialog)
                    {
                        //string _LOT_NO = SelectedMenuItem.LOT_NO;

                        //SearchMenuContact();

                        ////x => x.LOT_NO.Equals(_LOT_NO)
                        //int nCnt = SelectedMenuViewList.IndexOf(SelectedMenuViewList.Where(x => x.LOT_NO.Equals(_LOT_NO)).LastOrDefault<ManVo>());
                        //if (nCnt >= 0)
                        //{
                        //    SelectedMenuItem = SelectedMenuViewList[nCnt];
                        //}
                    }
                }

            }
            catch (Exception)
            {
                return;
            }
        }

        [Command]
        public void ProducePrintContact()
        {
            try
            {
                if (SelectDtlItmList == null)
                {
                    return;
                }

                if (SelectDtlItmList.Count > 0)
                {
                    SelectDtlItmList[0].LOT_NO = SelectedMenuItem.LOT_NO;
                    SelectDtlItmList[0].WRK_MAN_NM = SelectedMenuItem.WRK_MAN_NM;
                    SelectDtlItmList[0].ASSY_ITM_CD = SelectedMenuItem.ASSY_ITM_CD;
                    SelectDtlItmList[0].ASSY_ITM_NM = SelectedMenuItem.ASSY_ITM_NM;
                    SelectDtlItmList[0].WRK_DT = SelectedMenuItem.WRK_DT;
                    SelectDtlItmList[0].MIX_WEIH_VAL = SelectedMenuItem.MIX_WEIH_VAL;


                    producePrintDialog = new M6632ProducePrintDialog(SelectDtlItmList);
                    producePrintDialog.Title = "제조 지시 및 기록서 - [" + SelectedMenuItem.LOT_NO + "]";
                    producePrintDialog.Owner = Application.Current.MainWindow;
                    producePrintDialog.BorderEffect = BorderEffect.Default;
                    ////masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
                    ////masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
                    bool isDialog = (bool)producePrintDialog.ShowDialog();
                    if (isDialog)
                    {
                        //string _LOT_NO = SelectedMenuItem.LOT_NO;

                        //SearchMenuContact();

                        ////x => x.LOT_NO.Equals(_LOT_NO)
                        //int nCnt = SelectedMenuViewList.IndexOf(SelectedMenuViewList.Where(x => x.LOT_NO.Equals(_LOT_NO)).LastOrDefault<ManVo>());
                        //if (nCnt >= 0)
                        //{
                        //    SelectedMenuItem = SelectedMenuViewList[nCnt];
                        //}
                    }
                }

            }
            catch (Exception)
            {
                return;
            }
        }


        [Command]
        public async void Apply()
        {
            try
            {
                if(SelectedMenuItem == null)
                {
                    return;
                }

                MessageBoxResult result = WinUIMessageBox.Show(SelectedMenuItem.INP_LOT_NO + " 정말로 칭량 완료 하시겠습니까?", _title, MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    int _Num = 0;

                    this.SelectedMenuItem.UPD_USR_ID = SystemProperties.USER;
                    this.SelectedMenuItem.WRK_END_FLG = "Y";
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6631/mst/u", new StringContent(JsonConvert.SerializeObject(this.SelectedMenuItem), System.Text.Encoding.UTF8, "application/json")))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string resultX = await response.Content.ReadAsStringAsync();
                            if (int.TryParse(resultX, out _Num) == false)
                            {
                                //실패
                                WinUIMessageBox.Show(resultX, "[" + SystemProperties.PROGRAM_TITLE + "] " + _title, MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }


                                isM_OK = false;
                                isM_CANCEL = true;
                           
                            this.SelectedMenuItem.WRK_END_FLG = "예";
                            //성공
                            WinUIMessageBox.Show("칭량 완료 되었습니다", _title, MessageBoxButton.OK, MessageBoxImage.Information);
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
        public async void Cancel()
        {
            try
            {
                if (SelectedMenuItem == null)
                {
                    return;
                }

                MessageBoxResult result = WinUIMessageBox.Show(SelectedMenuItem.INP_LOT_NO + " 정말로 칭량 취소 하시겠습니까?", _title, MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    int _Num = 0;

                    this.SelectedMenuItem.UPD_USR_ID = SystemProperties.USER;
                    this.SelectedMenuItem.WRK_END_FLG = "N";
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6631/mst/u", new StringContent(JsonConvert.SerializeObject(this.SelectedMenuItem), System.Text.Encoding.UTF8, "application/json")))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string resultX = await response.Content.ReadAsStringAsync();
                            if (int.TryParse(resultX, out _Num) == false)
                            {
                                //실패
                                WinUIMessageBox.Show(resultX, "[" + SystemProperties.PROGRAM_TITLE + "] " + _title, MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }


                                isM_OK = true;
                                isM_CANCEL = false;
                          
                            this.SelectedMenuItem.WRK_END_FLG = "아니요";
                            //성공
                            WinUIMessageBox.Show("칭량 취소 되었습니다", _title, MessageBoxButton.OK, MessageBoxImage.Information);
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




        public void FindContact()
        {
            //////제품명 / 수량 / 조회
            //using (HttpResponseMessage responseX = await SystemProperties.PROGRAM_HTTP.PostAsync("m6630/dtl", new StringContent(JsonConvert.SerializeObject(new ManVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD, PROD_PLN_NO = SelectedMenuItem.SL_ORD_NO }), System.Text.Encoding.UTF8, "application/json")))
            //{
            //    if (responseX.IsSuccessStatusCode)
            //    {
            //      this.SelectDtlPopupList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await responseX.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
            //    }
            //}
            if (SelectedMenuItem == null) { return; }

            detailDialog = new M6631DetailDialog(new ManVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD, PROD_PLN_NO = SelectedMenuItem.SL_ORD_NO });
            detailDialog.Title = _title + " - " + SelectedMenuItem.SL_ORD_NO;
            detailDialog.Owner = Application.Current.MainWindow;
            detailDialog.BorderEffect = BorderEffect.Default;
            //masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            //masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            bool isDialog = (bool)detailDialog.ShowDialog();
            if (isDialog)
            {
            }

        }




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


        private bool? _isD_UPDATE = false;
        public bool? isD_UPDATE
        {
            get { return _isD_UPDATE; }
            set { SetProperty(ref _isD_UPDATE, value, () => isD_UPDATE); }
        }




        private bool? _isM_OK = false;
        public bool? isM_OK
        {
            get { return _isM_OK; }
            set { SetProperty(ref _isM_OK, value, () => isM_OK); }
        }


        private bool? _isM_CANCEL = false;
        public bool? isM_CANCEL
        {
            get { return _isM_CANCEL; }
            set { SetProperty(ref _isM_CANCEL, value, () => isM_CANCEL); }
        }






        private string _M_SEARCH_TEXT = string.Empty;
        public string M_SEARCH_TEXT
        {
            get { return _M_SEARCH_TEXT; }
            set { SetProperty(ref _M_SEARCH_TEXT, value, () => M_SEARCH_TEXT); }
        }
    }
}
