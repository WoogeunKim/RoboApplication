using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
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
using AquilaErpWpfApp3.Util;

namespace AquilaErpWpfApp3.ViewModel
{
    public sealed class S2239ViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private string title = "매출계산서현황";

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

        public S2239ViewModel() 
        {
            StartDt = System.DateTime.Now;
            EndDt = System.DateTime.Now;

            SYSTEM_CODE_VO();

            ////사업장
            //AreaList = SystemProperties.SYSTEM_CODE_VO("L-002");
            //this.AreaList.Insert(0, new CodeDao() { CLSS_CD = "000", CLSS_DESC = "전체" });
            //_AreaMap = SystemProperties.SYSTEM_CODE_MAP("L-002");
            //this._AreaMap.Add( "전체", "000");

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

            ////거래처

            ////_CoNmMap = SystemProperties.CUSTOMER_CODE_MAP(null, SystemProperties.USER_VO.EMPE_PLC_NM);
            //_CoNmMap = SystemProperties.CUSTOMER_CODE_MAP("AR", null);
            //this.CoNmList = SystemProperties.CUSTOMER_CODE_VO(null, SystemProperties.USER_VO.EMPE_PLC_NM);
            //this.CoNmList.Insert(0, new CustomerCodeDao() { CO_NO = "", CO_NM = "" });
            ////this.M_SL_CO_NM = this.CoNmList[0];
            //TXT_SL_CO_NM = this.CoNmList[0].CO_NO;


            ////매출/수금할인
            //GbnList = new List<CodeDao>()
            //                    {   
            //                            new CodeDao(){ CLSS_CD = "", CLSS_DESC = "전체"}
            //                        ,   new CodeDao(){ CLSS_CD = "매출", CLSS_DESC = "매출"}
            //                        ,   new CodeDao(){CLSS_CD = "수금할인", CLSS_DESC = "수금할인"}
            //                    };
            //this.M_GBN_FLG = "전체";



           // Refresh();
        }

        [Command]
        public async void Refresh()
        {
            try
            {
                SaleVo _param = new SaleVo();
                _param.FM_DT = (StartDt).ToString("yyyyMMdd");
                _param.TO_DT = (EndDt).ToString("yyyyMMdd");
                //사업장
                _param.AREA_CD = M_SL_AREA_NM.CLSS_CD;

                if (_param.AREA_CD == "000")
                {
                    _param.AREA_CD = null;
                }

                _param.AREA_NM = M_SL_AREA_NM.CLSS_DESC;
                //거래처
                //_param.CO_CD = (string.IsNullOrEmpty(TXT_SL_CO_NM) ? null : TXT_SL_CO_NM);
                //_param.SL_CO_CD = (M_SL_CO_NM == null ? null : M_SL_CO_NM.CO_NO);
                //채널
                _param.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
                //매출/수금할인/전체
                _param.GBN = (M_GBN_FLG.CLSS_DESC == "전체" ? null : M_GBN_FLG.CLSS_DESC);

                ////
                //SelectMstList = saleOrderClient.S2239SelectMstList(_param);
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s2239/mst", new StringContent(JsonConvert.SerializeObject(_param), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        SelectMstList = JsonConvert.DeserializeObject<IEnumerable<SaleVo>>(await response.Content.ReadAsStringAsync()).Cast<SaleVo>().ToList();
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


        private IList<SystemCodeVo> _GbnList = new List<SystemCodeVo>();
        public IList<SystemCodeVo> GbnList
        {
            get { return _GbnList; }
            set { SetProperty(ref _GbnList, value, () => GbnList); }
        }

        private SystemCodeVo _M_GBN_FLG;
        public SystemCodeVo M_GBN_FLG
        {
            get { return _M_GBN_FLG; }
            set { SetProperty(ref _M_GBN_FLG, value, () => M_GBN_FLG); }
        }




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
        private IList<SystemCodeVo> _AreaCd = new List<SystemCodeVo>();
        public IList<SystemCodeVo> AreaList
        {
            get { return _AreaCd; }
            set { SetProperty(ref _AreaCd, value, () => AreaList); }
        }

        //private CodeDao _M_SL_AREA_NM;
        //public CodeDao M_SL_AREA_NM
        //{
        //    get { return _M_SL_AREA_NM; }
        //    set { SetProperty(ref _M_SL_AREA_NM, value, () => M_SL_AREA_NM, RefreshCoNm); }
        //}

        private SystemCodeVo _M_SL_AREA_NM;
        public SystemCodeVo M_SL_AREA_NM
        {
            get { return _M_SL_AREA_NM; }
            set { SetProperty(ref _M_SL_AREA_NM, value, () => M_SL_AREA_NM, RefreshCoNm); }
        }

        private async void RefreshCoNm()
        {
            //if (M_SL_AREA_NM != null)
            //{
            //    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s143", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { DELT_FLG = "N", /*CO_TP_CD = "AR", */ AREA_CD = M_SL_AREA_NM.CLSS_CD, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
            //    {
            //        if (response.IsSuccessStatusCode)
            //        {
            //            CoNmList = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
            //            if (CoNmList.Count > 0)
            //            {
            //                CoNmList.Insert(0, new SystemCodeVo() { CO_NM = "전체", CO_NO = null });
            //                M_SL_CO_NM = CoNmList[0];
            //            }
            //        }
            //    }
            //}
        }

        //거래처
        private IList<SystemCodeVo> _CoNmList = new List<SystemCodeVo>();
        public IList<SystemCodeVo> CoNmList
        {
            get { return _CoNmList; }
            set { SetProperty(ref _CoNmList, value, () => CoNmList); }
        }
        //private CustomerCodeDao _M_DEPT_DESC;
        //public CustomerCodeDao M_DEPT_DESC
        //{
        //    get { return _M_DEPT_DESC; }
        //    set { SetProperty(ref _M_DEPT_DESC, value, () => M_DEPT_DESC); }
        //}

        private SystemCodeVo _M_SL_CO_NM;
        public SystemCodeVo M_SL_CO_NM
        {
            get { return _M_SL_CO_NM; }
            set { SetProperty(ref _M_SL_CO_NM, value, () => _M_SL_CO_NM); }
        }

        private void SelectMstDetail()
        {
            
        }
      

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

        public void setTitle()
        {
            Title = "[기간]" + (StartDt).ToString("yyyy-MM-dd") + "~" + (EndDt).ToString("yyyy-MM-dd") + ", [사업장]" + M_SL_AREA_NM.CLSS_DESC /*+ ", [거래처]" + M_SL_CO_NM.CO_NM */+ ", [매출/수금할인]" + M_GBN_FLG.CLSS_DESC;
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

            //using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s143", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { DELT_FLG = "N", /*CO_TP_CD = "AR", */ AREA_CD = SystemProperties.USER_VO.EMPE_PLC_NM, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
            //{
            //    if (response.IsSuccessStatusCode)
            //    {
            //        CoNmList = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
            //        if (CoNmList.Count > 0)
            //        {
            //            CoNmList.Insert(0, new SystemCodeVo() { CO_NM = "전체", CO_NO = null });
            //            M_SL_CO_NM = CoNmList[0];
            //        }
            //    }
            //}

            ////매출/수금할인
            GbnList = new List<SystemCodeVo>()
                                {
                                        new SystemCodeVo(){ CLSS_CD = "", CLSS_DESC = "전체"}
                                    ,   new SystemCodeVo(){ CLSS_CD = "매출", CLSS_DESC = "매출"}
                                    ,   new SystemCodeVo(){CLSS_CD = "수금할인", CLSS_DESC = "수금할인"}
                                };
            this.M_GBN_FLG = GbnList[0];

        }

    }
}
