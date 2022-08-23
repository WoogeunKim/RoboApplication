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
    public sealed class S2247ViewModel : ViewModelBase, INotifyPropertyChanged
    {
        //private static SaleOrderServiceClient saleOrderClient = SystemProperties.SaleOrderClient;
        private IList<SaleVo> selectedMstList = new List<SaleVo>();
        private IList<SaleVo> selectedTmpList = new List<SaleVo>();

        private int nDiffDay;
        private List<string> diffDay = new List<string>();

       // private ICommand _searchDialogCommand;

        public S2247ViewModel() 
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

            //_AreaMap = SystemProperties.SYSTEM_CODE_MAP("L-002");
            //AreaList = SystemProperties.SYSTEM_CODE_VO("L-002");
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
                  selectedTmpList = new List<SaleVo>();
                  SelectMstList = new List<SaleVo>();

                //DXSplashScreen.Show<ProgressWindow>();
                //SearchDetail = null;
                //SelectDtlList = null;

                //    SaleVo _param = new SaleVo();
                //    _param.AREA_CD = (M_SL_AREA_NM == null ? null : M_SL_AREA_NM.CLSS_CD);
                //    _param.FM_DT = (StartDt).ToString("yyyy-MM-dd");
                //    _param.TO_DT = (EndDt).ToString("yyyy-MM-dd");
                //    _param.CO_CD = (M_SL_CO_NM == null ? null : M_SL_CO_NM.CO_NO);
                ////_param.CLZ_FLG = (string.IsNullOrEmpty(M_PUR_CLZ_FLG) ? null : M_PUR_CLZ_FLG);


                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s2247/mst", new StringContent(JsonConvert.SerializeObject(new SaleVo() { FM_DT = (StartDt).ToString("yyyy-MM-dd"), TO_DT = (EndDt).ToString("yyyy-MM-dd"), AREA_CD = M_SL_AREA_NM.CLSS_CD, CO_CD = (M_SL_CO_NM == null ? null : M_SL_CO_NM.CO_NO), CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectMstList = JsonConvert.DeserializeObject<IEnumerable<SaleVo>>(await response.Content.ReadAsStringAsync()).Cast<SaleVo>().ToList();
                    }
                    //selectedTmpList = saleOrderClient.S2247SelectMstList(_param);
                    //{ AREA_CD = (string.IsNullOrEmpty(M_SL_AREA_NM.CLSS_CD) ? null : M_SL_AREA_NM.CLSS_CD), FM_DT = (StartDt).ToString("yyyy-MM-dd"), TO_DT = (EndDt).ToString("yyyy-MM-dd"), PUR_CO_CD = (string.IsNullOrEmpty(M_DEPT_DESC.CO_NO) ? null : M_DEPT_DESC.CO_NO), PUR_CLZ_FLG = (string.IsNullOrEmpty(M_PUR_CLZ_FLG) ? null : M_PUR_CLZ_FLG) }
                    ////+ ",       [마감]" + (string.IsNullOrEmpty(M_PUR_CLZ_FLG) ? "전체" : M_PUR_CLZ_FLG)
                    Title = "[일자]" + (StartDt).ToString("yyyy-MM-dd") + "~" + (EndDt).ToString("yyyy-MM-dd") + ",     [사업장]" + M_SL_AREA_NM.CLSS_DESC + ",     [거래처]" + M_SL_CO_NM.CO_NM;

                    if (selectedTmpList.Count > 0)
                    {
                        this.diffDay.Clear();

                        TimeSpan ts = EndDt - StartDt;
                        this.nDiffDay = ts.Days;

                        //일자
                        for (int x = 0; x <= this.nDiffDay; x++)
                        {
                            this.diffDay.Insert(x, StartDt.AddDays(x).ToString("yyyy-MM-dd"));
                        }

                        //계산
                        IList<SaleVo> tmpList = new List<SaleVo>();
                        object TTL_N1ST_CLT_AMT = 0;
                        object TTL_N2ND_CLT_AMT = 0;
                        object TTL_N3RD_CLT_AMT = 0;
                        object TTL_N4TH_CLT_AMT = 0;
                        object TTL_CLT_RMN_AMT = 0;
                        object TTL_CLT_AMT = 0;
                        //if (selectedTmpList[0].INAUD_NM.Equals("( 일 계 )"))
                        //{
                        //    TTL_N1ST_CLT_AMT = SelectMstList[0].N1ST_CLT_AMT;
                        //    TTL_N2ND_CLT_AMT = SelectMstList[0].N2ND_CLT_AMT;
                        //    TTL_N3RD_CLT_AMT = SelectMstList[0].N3RD_CLT_AMT;
                        //    TTL_N4TH_CLT_AMT = SelectMstList[0].N4TH_CLT_AMT;
                        //    TTL_CLT_RMN_AMT = SelectMstList[0].CLT_RMN_AMT;
                        //    TTL_CLT_AMT = SelectMstList[0].CLT_AMT;
                        //}
                        //
                        object MON_N1ST_CLT_AMT = 0;
                        object MON_N2ND_CLT_AMT = 0;
                        object MON_N3RD_CLT_AMT = 0;
                        object MON_N4TH_CLT_AMT = 0;
                        object MON_CLT_RMN_AMT = 0;
                        object MON_CLT_AMT = 0;
                        for (int x = 0; x < this.diffDay.Count; x++)
                        {
                            MON_N1ST_CLT_AMT = 0;
                            MON_N2ND_CLT_AMT = 0;
                            MON_N3RD_CLT_AMT = 0;
                            MON_N4TH_CLT_AMT = 0;
                            MON_CLT_RMN_AMT = 0;
                            MON_CLT_AMT = 0;
                            tmpList = selectedTmpList.Where(z => z.CLT_BIL_DT == this.diffDay[x]).ToList<SaleVo>();
                            for (int y = 0; y < tmpList.Count; y++)
                            {
                                SelectMstList.Add(tmpList[y]);
                                //(월계)
                                MON_N1ST_CLT_AMT = Convert.ToInt64(MON_N1ST_CLT_AMT) + Convert.ToInt64(tmpList[y].N1ST_CLT_AMT);
                                MON_N2ND_CLT_AMT = Convert.ToDouble(MON_N2ND_CLT_AMT) + Convert.ToDouble(tmpList[y].N2ND_CLT_AMT);
                                MON_N3RD_CLT_AMT = Convert.ToInt64(MON_N3RD_CLT_AMT) + Convert.ToInt64(tmpList[y].N3RD_CLT_AMT);
                                MON_N4TH_CLT_AMT = Convert.ToDouble(MON_N4TH_CLT_AMT) + Convert.ToDouble(tmpList[y].N4TH_CLT_AMT);
                                MON_CLT_RMN_AMT = Convert.ToInt64(MON_CLT_RMN_AMT) + Convert.ToInt64(tmpList[y].CLT_RMN_AMT);
                                MON_CLT_AMT = Convert.ToDouble(MON_CLT_AMT) + Convert.ToDouble(tmpList[y].CLT_AMT);
                                //(누계)
                                TTL_N1ST_CLT_AMT = Convert.ToInt64(TTL_N1ST_CLT_AMT) + Convert.ToInt64(tmpList[y].N1ST_CLT_AMT);
                                TTL_N2ND_CLT_AMT = Convert.ToDouble(TTL_N2ND_CLT_AMT) + Convert.ToDouble(tmpList[y].N2ND_CLT_AMT);
                                TTL_N3RD_CLT_AMT = Convert.ToInt64(TTL_N3RD_CLT_AMT) + Convert.ToInt64(tmpList[y].N3RD_CLT_AMT);
                                TTL_N4TH_CLT_AMT = Convert.ToDouble(TTL_N4TH_CLT_AMT) + Convert.ToDouble(tmpList[y].N4TH_CLT_AMT);
                                TTL_CLT_RMN_AMT = Convert.ToInt64(TTL_CLT_RMN_AMT) + Convert.ToInt64(tmpList[y].CLT_RMN_AMT);
                                TTL_CLT_AMT = Convert.ToDouble(TTL_CLT_AMT) + Convert.ToDouble(tmpList[y].CLT_AMT);
                                //(월계) && (합계)
                                if ((tmpList.Count - 1) == y)
                                {
                                    SelectMstList.Add(new SaleVo() { A1 = "1", CO_NM = this.diffDay[x] + " (일계)", N1ST_CLT_AMT = MON_N1ST_CLT_AMT, N2ND_CLT_AMT = MON_N2ND_CLT_AMT, N3RD_CLT_AMT = Convert.ToInt64(MON_N3RD_CLT_AMT), N4TH_CLT_AMT = MON_N4TH_CLT_AMT, CLT_RMN_AMT = Convert.ToInt64(MON_CLT_RMN_AMT), CLT_AMT = Convert.ToInt64(MON_CLT_AMT) });
                                    SelectMstList.Add(new SaleVo() { A1 = "2", CO_NM = " (누계)", N1ST_CLT_AMT = TTL_N1ST_CLT_AMT, N2ND_CLT_AMT = TTL_N2ND_CLT_AMT, N3RD_CLT_AMT = Convert.ToInt64(TTL_N3RD_CLT_AMT), N4TH_CLT_AMT = TTL_N4TH_CLT_AMT, CLT_RMN_AMT = Convert.ToInt64(TTL_CLT_RMN_AMT), CLT_AMT = Convert.ToInt64(TTL_CLT_AMT) });
                                }
                            }
                        }

                        if (SelectMstList.Count >= 1)
                        {
                            //ts = StartDt - EndDt;
                            //this.nDiffDay = ts.Days;

                            ////일자
                            //for (int x = 0; x <= this.nDiffDay; x++)
                            //{
                            //    this.diffDay.Insert(x, StartDt.AddMonths(x).ToString("yyyy-MM-dd"));
                            //}

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
              }
              catch (System.Exception eLog)
              {
                  //DXSplashScreen.Close();
                  WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]일자별 수금현황", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
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


        //public ICommand SearchDialogCommand
        //{
        //    get
        //    {
        //        if (_searchDialogCommand == null)
        //            _searchDialogCommand = new DelegateCommand(Refresh);
        //        return _searchDialogCommand;
        //    }
        //}

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

            //Refresh();
        }

    }





}
