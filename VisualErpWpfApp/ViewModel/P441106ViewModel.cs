using AquilaErpWpfApp3.Util;
using AquilaErpWpfApp3.View.PUR.Dialog;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Dialogs;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Auth;
using ModelsLibrary.Code;
using ModelsLibrary.Pur;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Media;

namespace AquilaErpWpfApp3.ViewModel
{
    public sealed class P441106ViewModel : ViewModelBase, INotifyPropertyChanged
    {

        //private string SMTP_SERVER = "mail.iegkr.com";
        private string _title = "고객발주관리";

        //private static PurServiceClient purClient = SystemProperties.PurClient;
        private IList<PurVo> selectedMstList = new List<PurVo>();

        //private ObservableCollection<PurVo> selectedDtlList = new ObservableCollection<PurVo>();
        private IList<PurVo> selectedDtlList = new List<PurVo>();


        //private IList<SystemCodeVo> customerList = new List<SystemCodeVo>();
        //private IList<SystemCodeVo> selectCustomerList = new List<SystemCodeVo>();

        private P441106MasterDialog masterDialog;
        private P441106DetailDialog detailDialog;
        private GroupUserVo UserVo;
        //private P441102EmailDialog emailDialog;


        public P441106ViewModel() 
        {

            //WeekDt = System.DateTime.Now;

            StartDt = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy-MM-01"));
            EndDt = System.DateTime.Now;

            //사업장
            SYSTEM_CODE_VO();
            // - Refresh();

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


            ////매입처
            //_DeptMap = SystemProperties.CUSTOMER_CODE_MAP("AP", SystemProperties.USER_VO.EMPE_PLC_NM);
            //DeptList = SystemProperties.CUSTOMER_CODE_VO("AP", SystemProperties.USER_VO.EMPE_PLC_NM);
            //DeptList.Insert(0, new CustomerCodeDao() { CO_NO = "", CO_NM = "" });
            //if (DeptList.Count > 0)
            //{
            //    //M_DEPT_DESC = DeptList[0];
            //    TXT_DEPT_DESC = DeptList[0].CO_NO;

            //    //TXT_SL_AREA_NM = SystemProperties.USER_VO.EMPE_PLC_CD;
            //    //for (int x = 0; x < DeptList.Count; x++)
            //    //{
            //    //    if (TXT_DEPT_DESC.Equals(DeptList[x].CO_NO))
            //    //    {
            //    //        M_SL_AREA_NM = DeptList[x];
            //    //        break;
            //    //    }
            //    //}
            //}


            //Refresh();
        }

        [Command]
        public async void Refresh(string _PUR_ORD_NO = null)
        {
            try
            {
                //DXSplashScreen.Show<ProgressWindow>();
                SearchDetail = null;
                SelectDtlList = null;
                SelectedMstItem = null;

                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("p441106/mst", new StringContent(JsonConvert.SerializeObject(new PurVo() { FM_DT = (StartDt).ToString("yyyy-MM-dd"), TO_DT = (EndDt).ToString("yyyy-MM-dd"), AREA_CD = M_SL_AREA_NM.CLSS_CD, CHNL_CD = SystemProperties.USER_VO.CHNL_CD, PUR_ITM_CD = "M" }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectMstList = JsonConvert.DeserializeObject<IEnumerable<PurVo>>(await response.Content.ReadAsStringAsync()).Cast<PurVo>().ToList();
                    }
                    //string tmpCO_NO = "";
                    //if (SelectedItemDept != null) { tmpCO_NO=SelectedItemDept.CO_NO; }
                    //SelectMstList = purClient.P4411SelectMstList(new PurVo() { FM_DT = (StartDt).ToString("yyyy-MM-dd"), TO_DT = (EndDt).ToString("yyyy-MM-dd"), AREA_CD = (string.IsNullOrEmpty(TXT_SL_AREA_NM) ? null : _AreaMap[TXT_SL_AREA_NM]), PUR_CO_CD = (string.IsNullOrEmpty(TXT_DEPT_DESC) ? null : TXT_DEPT_DESC) });
                    //Title = "[기간]" + (StartDt).ToString("yyyy-MM-dd") + "~" + (EndDt).ToString("yyyy-MM-dd") + ",   [사업장]" + (string.IsNullOrEmpty(TXT_SL_AREA_NM) ? "전체" : TXT_SL_AREA_NM) + ",   [매입처]" + (string.IsNullOrEmpty(TXT_DEPT_DESC) ? "전체" : _DeptMap[TXT_DEPT_DESC]) + (string.IsNullOrEmpty(M_SEARCH_TEXT) ? "" : (",   [검 색]" + M_SEARCH_TEXT));
                    //SelectMstList = purClient.P4411SelectMstList(new PurVo() { FM_DT = (StartDt).ToString("yyyy-MM-dd"), TO_DT = (EndDt).ToString("yyyy-MM-dd"), AREA_CD = (string.IsNullOrEmpty(TXT_SL_AREA_NM) ? null : _AreaMap[TXT_SL_AREA_NM]), PUR_CO_CD = (string.IsNullOrEmpty(tmpCO_NO) ? null : tmpCO_NO) });
                    Title = "[기간]" + (StartDt).ToString("yyyy-MM-dd") + "~" + (EndDt).ToString("yyyy-MM-dd") + ",   [사업장]" + M_SL_AREA_NM.CLSS_DESC;

                    if (SelectMstList.Count >= 1)
                    {
                        isM_UPDATE = true;
                        isM_DELETE = true;

                        if (string.IsNullOrEmpty(_PUR_ORD_NO))
                        {
                            SelectedMstItem = SelectMstList[0];
                        }
                        else
                        {
                            SelectedMstItem = SelectMstList.Where(x => x.PUR_NO.Equals(_PUR_ORD_NO)).LastOrDefault<PurVo>();
                        }
                    }
                    else
                    {

                        isM_UPDATE = false;
                        //isM_UPDATE = true;
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


        private string _M_SEARCH_TEXT = string.Empty;
        public string M_SEARCH_TEXT
        {
            get { return _M_SEARCH_TEXT; }
            set { SetProperty(ref _M_SEARCH_TEXT, value, () => M_SEARCH_TEXT); }
        }

        //출력 수
        private string _M_BAR_TEXT = "1";
        public string M_BAR_TEXT
        {
            get { return _M_BAR_TEXT; }
            set { SetProperty(ref _M_BAR_TEXT, value, () => M_BAR_TEXT); }
        }



        //사업장
        private Dictionary<string, string> _AreaMap = new Dictionary<string, string>();
        private IList<SystemCodeVo> _AreaCd = new List<SystemCodeVo>();
        public IList<SystemCodeVo> AreaList
        {
            get { return _AreaCd; }
            set { SetProperty(ref _AreaCd, value, () => AreaList); }
        }
        ////사업장
        //private CodeDao _M_SL_AREA_NM;
        //public CodeDao M_SL_AREA_NM
        //{
        //    get { return _M_SL_AREA_NM; }
        //    set { SetProperty(ref _M_SL_AREA_NM, value, () => M_SL_AREA_NM, RefreshCoNm); }
        //}
        //사업장 
        private SystemCodeVo _M_SL_AREA_NM;
        public SystemCodeVo M_SL_AREA_NM
        {
            get { return _M_SL_AREA_NM; }
            set { SetProperty(ref _M_SL_AREA_NM, value, () => M_SL_AREA_NM, RefreshCoNm); }
        }
        private void RefreshCoNm()
        {
            //if (TXT_SL_AREA_NM != null)
            //{
            //    _DeptMap = SystemProperties.CUSTOMER_CODE_MAP("AP", _AreaMap[TXT_SL_AREA_NM]);
            //    DeptList = SystemProperties.CUSTOMER_CODE_VO("AP", _AreaMap[TXT_SL_AREA_NM]);
            //    DeptList.Insert(0, new CustomerCodeDao() { CO_NO = "", CO_NM = "" });
            //    if (DeptList.Count > 0)
            //    {
            //        //M_DEPT_DESC = DeptList[0];
            //        //TXT_DEPT_DESC = DeptList[0].CO_NO;
            //    }
            //}
        }

        ////매입처
        //private Dictionary<string, string> _DeptMap = new Dictionary<string, string>();
        //private IList<CustomerCodeDao> _DeptCd = new List<CustomerCodeDao>();
        //public IList<CustomerCodeDao> DeptList
        //{
        //    get { return _DeptCd; }
        //    set { SetProperty(ref _DeptCd, value, () => DeptList); }
        //}

        //private CustomerCodeDao _SelectedItemDept = new CustomerCodeDao();
        //public CustomerCodeDao SelectedItemDept
        //{
        //    get { return _SelectedItemDept; }
        //    set { SetProperty(ref _SelectedItemDept, value, () => SelectedItemDept); }
        //}
        ////매입처
        //private CustomerCodeDao _M_DEPT_DESC;
        //public CustomerCodeDao M_DEPT_DESC
        //{
        //    get { return _M_DEPT_DESC; }
        //    set { SetProperty(ref _M_DEPT_DESC, value, () => M_DEPT_DESC); }
        //}
        //매입처
        //private string _TXT_DEPT_DESC = string.Empty;
        //public string TXT_DEPT_DESC
        //{
        //    get { return _TXT_DEPT_DESC; }
        //    set { SetProperty(ref _TXT_DEPT_DESC, value, () => TXT_DEPT_DESC); }
        //}


        //private bool? _M_SEARCH_CHECKD = false;
        //public bool? M_SEARCH_CHECKD
        //{
        //    get { return _M_SEARCH_CHECKD; }
        //    set { SetProperty(ref _M_SEARCH_CHECKD, value, () => M_SEARCH_CHECKD); }
        //}


        public IList<PurVo> SelectMstList
        {
            get { return selectedMstList; }
            set { SetProperty(ref selectedMstList, value, () => SelectMstList); }
        }

        PurVo _selectedMstItem;
        public PurVo SelectedMstItem
        {
            get
            {
                return _selectedMstItem;
            }
            set
            {
                SetProperty(ref _selectedMstItem, value, () => SelectedMstItem, SelectMstDetail);
            }
        }

        //private bool? _isM_SAVE = false;
        //public bool? isM_SAVE
        //{
        //    get { return _isM_SAVE; }
        //    set { SetProperty(ref _isM_SAVE, value, () => isM_SAVE); }
        //}

        //// 발주 클릭후 도면 등록 버튼 활성화
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

        private bool? _isD_DELETE = false;
        public bool? isD_DELETE
        {
            get { return _isD_DELETE; }
            set { SetProperty(ref _isD_DELETE, value, () => isD_DELETE); }
        }

        //


        // 마스터의 디테일 조회(발주 상세정보)
        [Command]
        public async void SelectMstDetail()
        {
            try
            {
                if (this._selectedMstItem == null)
                {
                    return;
                }

                PurVo Dtlvo = SelectedMstItem;

                //로그인한 유저가 고객사 일 때 (Y)
                if (this.UserVo.OSTR_FLG.Equals("Y"))
                {
                    Dtlvo.OSTR_FLG = "Y";
                }
                else
                {
                    //고객사가 아니라면 자기가 등록한 도면만 조회
                    Dtlvo.OSTR_FLG = "N";
                    Dtlvo.CRE_USR_ID = SystemProperties.USER;
                }


                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("p441106/dtl", new StringContent(JsonConvert.SerializeObject(Dtlvo), System.Text.Encoding.UTF8, "application/json")))
                {
                    IList<PurVo> resultVo = new List<PurVo>();

                    if (response.IsSuccessStatusCode)
                    {
                        resultVo = JsonConvert.DeserializeObject<IEnumerable<PurVo>>(await response.Content.ReadAsStringAsync()).Cast<PurVo>().ToList();
                    }


                    //N1ST는 도면등록자의 부모그룹아이디 (샵업체가 도면등록)
                    //사용자의 그룹아이디(고객사의 그룹아이디) 와 일치하는것만 보여주기
                    if (this.UserVo.OSTR_FLG.Equals("Y"))
                    {
                        resultVo = resultVo.Where<PurVo>(x => x.N1ST_RVW_USR_ID != null).ToList();
                        this.SelectDtlList = resultVo.Where<PurVo>(x => x.N1ST_RVW_USR_ID.Equals(this.UserVo.GRP_ID)).ToList();
                    }
                    else
                    {
                        //고객사가 아니면 자기가 등록한 도면만 모두조회
                        this.SelectDtlList = resultVo;
                    }


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

                //DXSplashScreen.Close();
            }
            catch (System.Exception eLog)
            {
                //DXSplashScreen.Close();
                //
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }
        //#endregion

        //#region Functon <Detail List>
        public IList<PurVo> SelectDtlList
        {
            get { return selectedDtlList; }
            set { SetProperty(ref selectedDtlList, value, () => SelectDtlList); }
        }

        PurVo _searchDetail;
        public PurVo SearchDetail
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


        string _Title = string.Empty;
        public string Title
        {
            get { return _Title; }
            set { SetProperty(ref _Title, value, () => Title); }
        }


        [Command]
        public void NewContact()
        {
            masterDialog = new P441106MasterDialog(new PurVo() { DELT_FLG = "N", AREA_CD = M_SL_AREA_NM.CLSS_CD, AREA_NM = M_SL_AREA_NM.CLSS_DESC, PUR_DT = System.DateTime.Now.ToString("yyyy-MM-dd"), PUR_CLZ_FLG = "N", PUR_EMPE_ID = SystemProperties.USER_VO.USR_ID });
            masterDialog.Title = _title + " - 추가";
            masterDialog.Owner = Application.Current.MainWindow;
            masterDialog.BorderEffect = BorderEffect.Default;
            masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            bool isDialog = (bool)masterDialog.ShowDialog();
            if (isDialog)
            {
                Refresh(masterDialog.PUR_NO);
            }
        }

      
        [Command]
        public void EditContact()
        {
            if (SelectedMstItem == null)
            {
                return;
            }
            //else if (SelectMstList.Count <= 0)
            //{
            //    return;
            //}

            masterDialog = new P441106MasterDialog(SelectedMstItem);
            masterDialog.Title = _title + " - 수정";
            masterDialog.Owner = Application.Current.MainWindow;
            masterDialog.BorderEffect = BorderEffect.Default;
            masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            bool isDialog = (bool)masterDialog.ShowDialog();
            if (isDialog)
            {
                Refresh(masterDialog.PUR_NO);
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
                PurVo delDao = SelectedMstItem;
                if (delDao != null)
                {
                    MessageBoxResult result = WinUIMessageBox.Show("[" + delDao.PUR_NO + "]" + " 정말로 삭제 하시겠습니까?", "[삭제]" + _title, MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        SelectedMstItem.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
                        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("p441106/mst/d", new StringContent(JsonConvert.SerializeObject(SelectedMstItem), System.Text.Encoding.UTF8, "application/json")))
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
                                Refresh();

                                //성공
                                WinUIMessageBox.Show("삭제가 완료되었습니다.", _title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                            }
                        }

                    }
                }
            }
            catch (Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }

        [Command]
        public async void FileDownloadContact()
        {
            try
            {
                //if (SearchDetail.FLR_FILE == null)
                //{
                //    WinUIMessageBox.Show("해당 파일이 존재하지 않습니다", _title, MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.None, MessageBoxOptions.None);
                //    return;
                //}

                DXFolderBrowserDialog _folderDiloag = new DXFolderBrowserDialog();
                _folderDiloag.ShowNewFolderButton = true;
                _folderDiloag.Description = "[파일명] 저장 폴더를 선택해주세요";
                _folderDiloag.RootFolder = Environment.SpecialFolder.Desktop;

                if (_folderDiloag.ShowDialog() == true)
                {
                    if (DXSplashScreen.IsActive == false)
                    {
                        DXSplashScreen.Show<ProgressWindow>();
                    }

                    //string _tmpPath = Path.Combine(_folderDiloag.SelectedPath, SearchDetail.FLR_NM);
                    //File.WriteAllBytes(_tmpPath, SearchDetail.FLR_FILE);
                    //System.Diagnostics.Process.Start(_folderDiloag.SelectedPath);
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("p441106/dtl/file", new StringContent(JsonConvert.SerializeObject(SearchDetail), System.Text.Encoding.UTF8, "application/json")))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            PurVo _downFileVo = JsonConvert.DeserializeObject<PurVo>(await response.Content.ReadAsStringAsync());
                            if (_downFileVo.isSuccess == true)
                            {
                                //byte[] resultMsg = await response.Content.ReadAsByteArrayAsync();

                                string _tmpPath = Path.Combine(_folderDiloag.SelectedPath, _downFileVo.FLR_NM);
                                File.WriteAllBytes(_tmpPath, _downFileVo.FLR_FILE);
                                System.Diagnostics.Process.Start(_folderDiloag.SelectedPath);
                            }
                            else
                            {
                                if (DXSplashScreen.IsActive == true)
                                {
                                    DXSplashScreen.Close();
                                }
                                //string result = await response.Content.ReadAsStringAsync();
                                //실패
                                WinUIMessageBox.Show(_downFileVo.Message, _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                                return;
                            }

                        }
                    }

                    //
                    if (DXSplashScreen.IsActive == true)
                    {
                        DXSplashScreen.Close();
                    }
                }
            }
            catch (System.Exception eLog)
            {
                if (DXSplashScreen.IsActive == true)
                {
                    DXSplashScreen.Close();
                }

                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }

        [Command]
        public async void FileConfirmContact()
        {
            try
            {
                //도면확정
                if (SearchDetail == null)
                {
                    return;
                }

                if (!this.UserVo.OSTR_FLG.Equals("Y"))
                {
                    WinUIMessageBox.Show("도면 확정 권한이 없습니다.", _title, MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                MessageBoxResult result = WinUIMessageBox.Show("도면번호" + "[" + SearchDetail.FLR_NO + "]" + " (을)를 정말로 확정 하시겠습니까?", "[확정]" + _title, MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    SearchDetail.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
                    SearchDetail.UPD_USR_ID = SystemProperties.USER;
                    
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("p441106/dtl/u", new StringContent(JsonConvert.SerializeObject(SearchDetail), System.Text.Encoding.UTF8, "application/json")))
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
                            //Refresh();
                            SelectMstDetail();

                            //성공
                            WinUIMessageBox.Show("도면확정이 완료되었습니다.", _title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
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

        [Command]
        public void NewDtlContact()
        {
            // 11.05 도면등록을 위해 잠시 주석
            if (SelectedMstItem == null)
            {
                return;
            }

            detailDialog = new P441106DetailDialog(new PurVo() { PUR_NO = SelectedMstItem.PUR_NO/*, DE_CO_NM = SelectedMstItem.DE_CO_NM*/, CHNL_CD = SystemProperties.USER_VO.CHNL_CD, CRE_USR_ID = SystemProperties.USER, UPD_USR_ID = SystemProperties.USER, DUE_DT = SelectedMstItem.DUE_DT });
            detailDialog.Title = "도면등록 - " + SelectedMstItem.PUR_NO + " / " + SelectedMstItem.CO_NM;
            detailDialog.Owner = Application.Current.MainWindow;
            detailDialog.BorderEffect = BorderEffect.Default;
            detailDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            detailDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            //Refresh();
            bool isDialog = (bool)detailDialog.ShowDialog();
            if (isDialog)
            {
                SelectMstDetail();
                //
                SelectedMstItem.PUR_SUM_AMT = this.SelectDtlList.Sum<PurVo>(s => Convert.ToDouble(s.PUR_WGT));
            }
        }



        [Command]
        public async void DelDtlContact()
        {
            try
            {
                //수정 삭제
                if (SearchDetail == null)
                {
                    return;
                }

                MessageBoxResult result = WinUIMessageBox.Show("[" + SearchDetail.PUR_NO + "/" + SearchDetail.PUR_SEQ + "]" + " 정말로 삭제 하시겠습니까?", "[삭제]" + _title, MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    SelectedMstItem.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("p441106/dtl/d", new StringContent(JsonConvert.SerializeObject(SearchDetail), System.Text.Encoding.UTF8, "application/json")))
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
                            //Refresh();
                            SelectMstDetail();
                            //
                            SelectedMstItem.PUR_SUM_AMT = this.SelectDtlList.Sum<PurVo>(s => Convert.ToDouble(s.PUR_WGT));

                            //성공
                            WinUIMessageBox.Show("삭제가 완료되었습니다.", _title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
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

        public async void SYSTEM_CODE_VO()
        {

            try
            {
                //SL_AREA_LIST = SystemProperties.SYSTEM_CODE_VO("L-002");
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

                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s136/u", new StringContent(JsonConvert.SerializeObject(new GroupUserVo{USR_ID = SystemProperties.USER_VO.USR_ID, CHNL_CD = SystemProperties.USER_VO.CHNL_CD, DELT_FLG = "N" }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.UserVo = JsonConvert.DeserializeObject<IEnumerable<GroupUserVo>>(await response.Content.ReadAsStringAsync()).Cast<GroupUserVo>().FirstOrDefault();
                    }
                }

            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }

            ////CustomerList = SystemProperties.SYSTEM_CODE_VO("L-002");
            //using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s143", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { DELT_FLG = "N", SEEK_AP = "AP", SEEK = "AP", CO_TP_CD = "AP", AREA_CD = SystemProperties.USER_VO.EMPE_PLC_NM, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
            //{
            //    if (response.IsSuccessStatusCode)
            //    {
            //        this.CustomerList = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
            //    }
            //}
            Refresh();
        }
    }
}
