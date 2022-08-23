using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Code;
using ModelsLibrary.Sale;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Media;
using AquilaErpWpfApp3.Util;
using AquilaErpWpfApp3.View.SAL.Dialog;

namespace AquilaErpWpfApp3.ViewModel
{
    public sealed class S22313ViewModel : ViewModelBase
    {
        private string title = "세금계산서발행 - 건별(Home Tax)";

        //private static SaleOrderServiceClient saleOrderClient = SystemProperties.SaleOrderClient;
        private IList<SaleVo> selectedMstList = new List<SaleVo>();

        
        private S22313ExcelDialog excelDialog;
        private S22313BillDialog billDialog;

        ////Menu Dialog
        //private ICommand _searchDialogCommand;
        //private ICommand _newDialogCommand;
        //private ICommand _editDetailDialogCommand;
        //private ICommand _delDialogCommand;
        //private ICommand _billCommand;
        //private ICommand _billHistoryCommand;
        //private ICommand _delDialogCommand;
        //private string _areaCd = string.Empty;

        //private S22313BillDialog s22313BillDialog;
        public S22313ViewModel()
        {
            StartDt = System.DateTime.Now;
            //EndDt = System.DateTime.Now;

            SYSTEM_CODE_VO();
                // - Refresh
        }

        [Command]
        public async void Refresh()
        {
            try
            {
                SaleVo _param = new SaleVo();
                _param.SL_DC_YRMON = (StartDt).ToString("yyyyMM");
                //_param.TO_DT = (EndDt).ToString("yyyyMM");
                //사업장
                _param.AREA_CD = M_SL_AREA_NM.CLSS_CD;
                _param.AREA_NM = M_SL_AREA_NM.CLSS_DESC;
                _param.BIL_CD = "B";
                _param.CLZ_FLG = (this.M_RLSE_PRT_FLG.Equals("전체") ? null : (this.M_RLSE_PRT_FLG.Equals("발행") ? "Y" : "N"));
                _param.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
                //
                ////
                // SelectMstList = saleOrderClient.S22313SelectMstList(_param);
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s22313/mst", new StringContent(JsonConvert.SerializeObject(_param), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectMstList = JsonConvert.DeserializeObject<IEnumerable<SaleVo>>(await response.Content.ReadAsStringAsync()).Cast<SaleVo>().ToList();
                    }

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
            }
            catch (System.Exception eLog)
            {
                //DXSplashScreen.Close();
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }


        [Command]
        public void Bill()
        {
            try
            {
                if (string.IsNullOrEmpty(M_SL_CO_NM.CO_NO))
                {
                    WinUIMessageBox.Show("선택한 거래처가 없습니다.", "[경고]" + this.title, MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.None, MessageBoxOptions.None);
                    return;
                }

                //S22313BillDialogViewModel s22313BillDialogViewModel = new S22313BillDialogViewModel();
                //s22313BillDialogViewModel.CoCd = this.TXT_SL_CO_NM;
                //s22313BillDialogViewModel.SlDcYrMon = (this.StartDt).ToString("yyyyMM");
                //s22313BillDialogViewModel.AreaCd = this._areaCd;

                //s22313BillDialogViewModel.Refresh();



                billDialog = new S22313BillDialog(new SaleVo() { CO_CD = M_SL_CO_NM.CO_NO, SL_CO_CD = M_SL_CO_NM.CO_NO, SL_DC_YRMON = (this.StartDt).ToString("yyyyMM"), AREA_CD = M_SL_AREA_NM.CLSS_CD, CHNL_CD = SystemProperties.USER_VO.CHNL_CD, CRE_USR_ID = SystemProperties.USER } );
                billDialog.Title = this.title;
                billDialog.Owner = Application.Current.MainWindow;
                billDialog.BorderEffect = BorderEffect.Default;
                bool isDialog = (bool)billDialog.ShowDialog();
                if (isDialog)
                {




                }
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }

        [Command]
        public async void DelContact()
        {
            try
            {
                if (SelectedMstItem == null)
                {
                    return;
                }
                
                if (SelectedMstItem != null)
                {
                    MessageBoxResult result = WinUIMessageBox.Show(" 정말로 삭제 하시겠습니까?", "[삭제]", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        try
                        {
                            SelectedMstItem.SL_CLSS_CD = (SelectedMstItem.GBN.Equals("매출") ? "A" : (SelectedMstItem.GBN.Equals("수금할인") ? "B" : null));
                            SelectedMstItem.BIL_CD = "B";
                            SelectedMstItem.AREA_CD = SystemProperties.USER_VO.EMPE_PLC_NM;

                            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s22313/mst/d", new StringContent(JsonConvert.SerializeObject(SelectedMstItem), System.Text.Encoding.UTF8, "application/json")))
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
                                    Refresh();

                                    //성공
                                    WinUIMessageBox.Show("삭제가 완료되었습니다.", title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                                }
                            }

                            //Refresh();
                            //WinUIMessageBox.Show("삭제가 완료되었습니다.", "[삭제]", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                        }
                        catch (System.Exception eLog)
                        {
                            WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                            return;
                        }
                    }
                }

            }
            catch(Exception eLog)
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

        //private Dictionary<string, string> _CoNmMap = new Dictionary<string, string>();
        private IList<SystemCodeVo> _CoNmList = new List<SystemCodeVo>();
        public IList<SystemCodeVo> CoNmList
        {
            get { return _CoNmList; }
            set { SetProperty(ref _CoNmList, value, () => CoNmList); }
        }

        private SystemCodeVo _M_SL_CO_NM;
        public SystemCodeVo M_SL_CO_NM
        {
            get { return _M_SL_CO_NM; }
            set { SetProperty(ref _M_SL_CO_NM, value, () => M_SL_CO_NM); }
        }
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

        //private string _M_DC_FLG = "사용";
        //public string M_DC_FLG
        //{
        //    get { return _M_DC_FLG; }
        //    set { SetProperty(ref _M_DC_FLG, value, () => M_DC_FLG); }
        //}

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

        //출력유무
        private IList<SystemCodeVo> _RlsePrtFlgList = new List<SystemCodeVo>();
        public IList<SystemCodeVo> RlsePrtFlgList
        {
            get { return _RlsePrtFlgList; }
            set { SetProperty(ref _RlsePrtFlgList, value, () => RlsePrtFlgList); }
        }

        private SystemCodeVo _M_RLSE_PRT_FLG;
        public SystemCodeVo M_RLSE_PRT_FLG
        {
            get { return _M_RLSE_PRT_FLG; }
            set { SetProperty(ref _M_RLSE_PRT_FLG, value, () => M_RLSE_PRT_FLG); }
        }


        private void SelectMstDetail()
        {

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

        //public ICommand BillCommand
        //{
        //    get
        //    {
        //        if (_billCommand == null)
        //            _billCommand = new DelegateCommand(Bill);
        //        return _billCommand;
        //    }
        //}

        //public ICommand BillHistoryCommand
        //{
        //    get
        //    {
        //        if (_billHistoryCommand == null)
        //            _billHistoryCommand = new DelegateCommand(BillHistory);
        //        return _billHistoryCommand;
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
        public void NewContact()
        {
            try
            {
                if (SelectedMstItem == null)
                {
                    return;
                }

                //if (string.IsNullOrEmpty(SelectedMstItem.GRP_BIL_NO))
                //{
                //    WinUIMessageBox.Show("선택한 항목이 없습니다.", "[경고]" + this.title, MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.None, MessageBoxOptions.None);
                //    return;
                //}

                string _path = System.Windows.Forms.Application.StartupPath + @"\cpcn.xls";
                DateTime _slYrMon = StartDt;
                string _bilCd = "B";
                string _grpBilNo = SelectedMstItem.GRP_BIL_NO;
                string _clzFlg = (this.M_RLSE_PRT_FLG.Equals("전체") ? null : (this.M_RLSE_PRT_FLG.Equals("출력") ? "Y" : "N"));


                if (System.IO.File.Exists(_path))
                {
                    //파일 삭제
                    System.IO.File.Delete(_path);
                }

                System.IO.File.Create(_path).Close();

                excelDialog = new S22313ExcelDialog(_path, SystemProperties.USER_VO.EMPE_PLC_NM, _slYrMon, _bilCd, _grpBilNo, _clzFlg);
                excelDialog.Title = "[엑셀]" + this.title;
                excelDialog.Owner = Application.Current.MainWindow;
                excelDialog.BorderEffect = BorderEffect.Default;
                excelDialog.BorderEffectActiveColor = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 128, 0));
                excelDialog.BorderEffectInactiveColor = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 170, 170));
                bool isDialog = (bool)excelDialog.ShowDialog();
                //if (isDialog)
                {
                    if (System.IO.File.Exists(_path))
                    {
                        //파일 삭제
                        System.IO.File.Delete(_path);
                    }
                    Refresh();
                }

            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }


        private static void ReleaseExcelObject(object obj)
        {
            try
            {
                if (obj != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                    obj = null;
                }
            }
            catch (Exception ex)
            {
                obj = null;
                throw ex;
            }
            finally
            {
                GC.Collect();
            }
        }

        private SystemCodeVo _SelectedItemAreaCd;
        public SystemCodeVo SelectedItemAreaCd
        {
            get { return _SelectedItemAreaCd; }
            set
            {
                if (value != null)
                {
                    SetProperty(ref _SelectedItemAreaCd, value, () => SelectedItemAreaCd);
                }
            }
        }

        public void setTitle()
        {
            Title = "[기간]" + (StartDt).ToString("yyyy-MM") + ", [사업장]" + M_SL_AREA_NM.CLSS_DESC;
        }


        public async void SYSTEM_CODE_VO()
        {
            ////사업장
            //AreaList = SystemProperties.SYSTEM_CODE_VO("L-002");
            //_AreaMap = SystemProperties.SYSTEM_CODE_MAP("L-002");
            //if (AreaList.Count > 0)
            //{
            //    M_SL_AREA_NM = SystemProperties.USER_VO.EMPE_PLC_CD;
            //    this._areaCd = SystemProperties.USER_VO.EMPE_PLC_NM;
            //}
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

            //this.CoNmList = SystemProperties.CUSTOMER_CODE_VO(null, SystemProperties.USER_VO.EMPE_PLC_NM);
            //this.CoNmList.Insert(0, new CustomerCodeDao() { CO_NO = "" });
            ////this.M_SL_CO_NM = this.CoNmList[0];
            //TXT_SL_CO_NM = this.CoNmList[0].CO_NO;
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s143", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { DELT_FLG = "N", CO_TP_CD = "AR", AREA_CD = SystemProperties.USER_VO.EMPE_PLC_NM, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    CoNmList = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                    if (CoNmList.Count > 0)
                    {
                        //CoNmList.Insert(0, new SystemCodeVo() { CO_NM = "전체", CO_NO = null });
                        M_SL_CO_NM = CoNmList[0];
                    }
                }
            }

            RlsePrtFlgList = new List<SystemCodeVo>()
            {   
                    new SystemCodeVo(){ CLSS_CD = null, CLSS_DESC = "전체"}
                ,   new SystemCodeVo(){ CLSS_CD = "Y", CLSS_DESC = "발행"}
                ,   new SystemCodeVo(){CLSS_CD = "N", CLSS_DESC = "미발행"}
            };
            this.M_RLSE_PRT_FLG = RlsePrtFlgList[2];

            Refresh();
        }

    }
}
