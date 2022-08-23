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
    public sealed class S2228ViewModel : ViewModelBase, INotifyPropertyChanged
    {
        //private static SaleOrderServiceClient saleOrderClient = SystemProperties.SaleOrderClient;
        private IList<SaleVo> selectedMstList = new List<SaleVo>();
        //private ICommand _searchDialogCommand;

        public S2228ViewModel() 
        {
          StartDt = System.DateTime.Now;
          EndDt = System.DateTime.Now;

          M_PUR_CLZ_FLG = "";


            SYSTEM_CODE_VO();

            //_DeptMap = SystemProperties.CUSTOMER_CODE_MAP(null, SystemProperties.USER_VO.EMPE_PLC_NM);
            //DeptList = SystemProperties.CUSTOMER_CODE_VO(null, SystemProperties.USER_VO.EMPE_PLC_NM);
            //DeptList.Insert(0, new CustomerCodeDao() { CO_NO = "", CO_NM = "" });
            ////M_DEPT_DESC = DeptList[0];
            //TXT_DEPT_DESC = DeptList[0].CO_NO;

            //AreaList = SystemProperties.SYSTEM_CODE_VO("L-002");
            //_AreaMap = SystemProperties.SYSTEM_CODE_MAP("L-002");
            //AreaList.Insert(0, new CodeDao() { CLSS_CD = "", CLSS_DESC = "" });
            ////M_SL_AREA_NM = AreaList[0];
            //TXT_SL_AREA_NM = AreaList[0].CLSS_DESC;
            //if (this.AreaList.Count > 0)
            //{
            //    this.TXT_SL_AREA_NM = SystemProperties.USER_VO.EMPE_PLC_CD;
            //    //for (int x = 0; x < this.AreaList.Count; x++)
            //    //{
            //    //    if (this.TXT_SL_AREA_NM.Equals(this.AreaList[x].CLSS_DESC))
            //    //    {
            //    //        this.M_SL_AREA_NM = this.AreaList[x];
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
                    //DXSplashScreen.Show<ProgressWindow>();
                    //SearchDetail = null;
                    //SelectDtlList = null;
                    //    SaleVo _param = new SaleVo();
                    //    _param.SL_AREA_CD = (M_SL_AREA_NM == null ? null : M_SL_AREA_NM.CLSS_CD);
                    //    _param.FM_DT = (StartDt).ToString("yyyy-MM-dd");
                    //    _param.TO_DT = (EndDt).ToString("yyyy-MM-dd");
                    //    _param.SL_CO_CD = (M_SL_CO_NM == null ? null : M_SL_CO_NM.CO_NO);
                    //    _param.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
                    ////_param.CLZ_FLG = (string.IsNullOrEmpty(M_PUR_CLZ_FLG) ? null : M_PUR_CLZ_FLG);

                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s2228/mst", new StringContent(JsonConvert.SerializeObject(new SaleVo() { FM_DT = (StartDt).ToString("yyyy-MM-dd"), TO_DT = (EndDt).ToString("yyyy-MM-dd"), SL_AREA_CD = M_SL_AREA_NM.CLSS_CD, SL_CO_CD = M_SL_CO_NM.CO_NO, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            this.SelectMstList = JsonConvert.DeserializeObject<IEnumerable<SaleVo>>(await response.Content.ReadAsStringAsync()).Cast<SaleVo>().ToList();
                        }

                        //SelectMstList = saleOrderClient.S2228SelectMstList(_param);
                        ////{ AREA_CD = (string.IsNullOrEmpty(M_SL_AREA_NM.CLSS_CD) ? null : M_SL_AREA_NM.CLSS_CD), FM_DT = (StartDt).ToString("yyyy-MM-dd"), TO_DT = (EndDt).ToString("yyyy-MM-dd"), PUR_CO_CD = (string.IsNullOrEmpty(M_DEPT_DESC.CO_NO) ? null : M_DEPT_DESC.CO_NO), PUR_CLZ_FLG = (string.IsNullOrEmpty(M_PUR_CLZ_FLG) ? null : M_PUR_CLZ_FLG) }
                        ////+ ",       [마감]" + (string.IsNullOrEmpty(M_PUR_CLZ_FLG) ? "전체" : M_PUR_CLZ_FLG)
                        Title = "[주문일자]" + (StartDt).ToString("yyyy-MM-dd") + "~" + (EndDt).ToString("yyyy-MM-dd") + ",     [사업장]" + M_SL_AREA_NM.CLSS_DESC + ",     [거래처]" + M_SL_CO_NM.CO_NM;

                        if (SelectMstList.Count >= 1)
                        {
                            isM_UPDATE = true;
                            isM_DELETE = true;

                            SelectedMstItem = SelectMstList[0];
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
                  //DXSplashScreen.Close();
                  WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]수주현황", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                  return;
              }
          }

          #region 발주기간 From To

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

        #endregion

        #region 거래처
        //private Dictionary<string, string> _DeptMap = new Dictionary<string, string>();
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

        #endregion

        #region 사업장
        //private Dictionary<string, string> _AreaMap = new Dictionary<string, string>();
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
            if (M_SL_AREA_NM != null)
            {
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s143", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { DELT_FLG = "N", /*CO_TP_CD = "AR", */ AREA_CD = M_SL_AREA_NM.CLSS_CD, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        CoNmList = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                        if (CoNmList.Count > 0)
                        {
                            CoNmList.Insert(0, new SystemCodeVo() { CO_NM = "전체", CO_NO = null });
                            M_SL_CO_NM = CoNmList[0];
                        }
                    }
                }
                //if (_AreaMap[TXT_SL_AREA_NM] != null)
                //{
                //    _DeptMap = SystemProperties.CUSTOMER_CODE_MAP(null, _AreaMap[TXT_SL_AREA_NM]);
                //    _DeptCd = SystemProperties.CUSTOMER_CODE_VO(null, _AreaMap[TXT_SL_AREA_NM]);
                //    _DeptCd.Insert(0, new CustomerCodeDao() { CO_NO = "", CO_NM = "" });
                //    DeptList = _DeptCd;
                //    //M_DEPT_DESC = DeptList[0];
                //    TXT_DEPT_DESC = DeptList[0].CO_NO;
                //}
            }
        }
          #endregion

          #region 마감
          private string _M_PUR_CLZ_FLG = string.Empty;
          public string M_PUR_CLZ_FLG
          {
              get { return _M_PUR_CLZ_FLG; }
              set { SetProperty(ref _M_PUR_CLZ_FLG, value, () => M_PUR_CLZ_FLG); }
          } 
          #endregion



          #region 기타
          //private string _M_SEARCH_TEXT = string.Empty;
          //public string M_SEARCH_TEXT
          //{
          //    get { return _M_SEARCH_TEXT; }
          //    set { SetProperty(ref _M_SEARCH_TEXT, value, () => M_SEARCH_TEXT); }
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

          string _Title = string.Empty;
          public string Title
          {
              get { return _Title; }
              set { SetProperty(ref _Title, value, () => Title); }
          }
          #endregion

          #region 마스터 그리드
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
                      SetProperty(ref _selectedMstItem, value, () => SelectedMstItem);
                  }
              }
         }
        #endregion



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

            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s143", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { DELT_FLG = "N", /*CO_TP_CD = "AR", */ AREA_CD = SystemProperties.USER_VO.EMPE_PLC_NM, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    CoNmList = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                    if (CoNmList.Count > 0)
                    {
                        CoNmList.Insert(0, new SystemCodeVo() { CO_NM = "전체", CO_NO = null });
                        M_SL_CO_NM = CoNmList[0];
                    }
                }
            }

            Refresh();
        }

            //public ICommand SearchDialogCommand
            //{
            //    get
            //    {
            //        if (_searchDialogCommand == null)
            //            _searchDialogCommand = new DelegateCommand(Refresh);
            //        return _searchDialogCommand;
            //    }
            //}
    }
}
