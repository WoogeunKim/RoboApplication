using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Code;
using ModelsLibrary.Inv;
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
    public sealed class I6613ViewModel : ViewModelBase, INotifyPropertyChanged
    {
        //private static InvServiceClient invClient = SystemProperties.InvClient;
        private IList<InvVo> selectedMstList = new List<InvVo>();
        //private ICommand _searchDialogCommand;

        public I6613ViewModel() 
        {
              StartDt = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy-MM-01"));
              EndDt = System.DateTime.Now;

            //M_PUR_CLZ_FLG = "";


            SYSTEM_CODE_VO();

            //_DeptMap = SystemProperties.CUSTOMER_CODE_MAP(null, SystemProperties.USER_VO.EMPE_PLC_NM);
            //  DeptList = SystemProperties.CUSTOMER_CODE_VO(null, SystemProperties.USER_VO.EMPE_PLC_NM);
            //  DeptList.Insert(0, new CustomerCodeDao() { CO_NO = "", CO_NM = "" });
            //  //M_DEPT_DESC = DeptList[0];
            //  TXT_DEPT_DESC = DeptList[0].CO_NO;


            //  _InaudNmMap = SystemProperties.SYSTEM_CODE_MAP("L-007");
            //  INAUD_NM_LIST = SystemProperties.SYSTEM_CODE_VO("L-007");
            //  for (int x = (INAUD_NM_LIST.Count-1); x >= 0 ; x--)
            //  {
            //      if (!INAUD_NM_LIST[x].CLSS_CD.StartsWith("R"))
            //      {
            //          INAUD_NM_LIST.Remove(INAUD_NM_LIST[x]);
            //      }
            //  }


            //  INAUD_NM_LIST.Insert(0, new CodeDao() { CLSS_CD = "", CLSS_DESC = "" });
            //  //M_SL_AREA_NM = AreaList[0];
            //  TXT_INAUD_NM = INAUD_NM_LIST[0].CLSS_DESC;


            //  _AreaMap = SystemProperties.SYSTEM_CODE_MAP("L-002");
            //  AreaList = SystemProperties.SYSTEM_CODE_VO("L-002");
            //  AreaList.Insert(0, new CodeDao() { CLSS_CD = "", CLSS_DESC = "" });
            //  //M_SL_AREA_NM = AreaList[0];
            //  TXT_SL_AREA_NM = AreaList[0].CLSS_DESC;
            //  if (this.AreaList.Count > 0)
            //  {
            //      this.TXT_SL_AREA_NM = SystemProperties.USER_VO.EMPE_PLC_CD;
            //      //for (int x = 0; x < this.AreaList.Count; x++)
            //      //{
            //      //    if (this.TXT_SL_AREA_NM.Equals(this.AreaList[x].CLSS_DESC))
            //      //    {
            //      //        this.M_SL_AREA_NM = this.AreaList[x];
            //      //        break;
            //      //    }
            //      //}
            //  }

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

                    InvVo _param = new InvVo();
                    _param.AREA_CD = M_SL_AREA_NM.CLSS_CD;
                    _param.FM_DT = (StartDt).ToString("yyyy-MM-dd");
                    _param.TO_DT = (EndDt).ToString("yyyy-MM-dd");
                    _param.CO_CD = (M_SL_CO_CD.CO_NO.Equals("전체") ? null : M_SL_CO_CD.CO_NO);
                    _param.INAUD_CD = M_INAUD_NM.CLSS_CD;
                    _param.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;

                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("i6613/mst", new StringContent(JsonConvert.SerializeObject(_param), System.Text.Encoding.UTF8, "application/json")))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            this.SelectMstList = JsonConvert.DeserializeObject<IEnumerable<InvVo>>(await response.Content.ReadAsStringAsync()).Cast<InvVo>().ToList();
                        }

                        //SelectMstList = invClient.I6613SelectMstList(_param);
                        //{ AREA_CD = (string.IsNullOrEmpty(M_SL_AREA_NM.CLSS_CD) ? null : M_SL_AREA_NM.CLSS_CD), FM_DT = (StartDt).ToString("yyyy-MM-dd"), TO_DT = (EndDt).ToString("yyyy-MM-dd"), PUR_CO_CD = (string.IsNullOrEmpty(M_DEPT_DESC.CO_NO) ? null : M_DEPT_DESC.CO_NO), PUR_CLZ_FLG = (string.IsNullOrEmpty(M_PUR_CLZ_FLG) ? null : M_PUR_CLZ_FLG) }
                        ////
                        Title = "[입고기간]" + (StartDt).ToString("yyyy-MM-dd") + "~" + (EndDt).ToString("yyyy-MM-dd") + ",   [사업장]" + M_SL_AREA_NM.CLSS_DESC + ",   [거래처]" + (M_SL_CO_CD.CO_NO.Equals("전체") ? "전체" : M_SL_CO_CD.CO_NM) + ",   [수불유형]" + M_INAUD_NM.CLSS_DESC;

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
                      WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]입고현황", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
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
        //거래처 
        //private Dictionary<string, string> _CustomerMap = new Dictionary<string, string>();
        private IList<SystemCodeVo> _CustomerList = new List<SystemCodeVo>();
        public IList<SystemCodeVo> CustomerList
        {
            get { return _CustomerList; }
            set { SetProperty(ref _CustomerList, value, () => CustomerList); }
        }

        private SystemCodeVo _M_SL_CO_CD;
        public SystemCodeVo M_SL_CO_CD
        {
            get { return _M_SL_CO_CD; }
            set { SetProperty(ref _M_SL_CO_CD, value, () => M_SL_CO_CD); }
        }
        #endregion

        #region 사업장
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
        //    set { SetProperty(ref _M_SL_AREA_NM, value, () => M_SL_AREA_NM); }
        //}
        //사업장 
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
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s143", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { DELT_FLG = "N", CO_TP_CD = null, AREA_CD = SystemProperties.USER_VO.EMPE_PLC_NM, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        CustomerList = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                        CustomerList.Insert(0, new SystemCodeVo() { CO_NO = "전체", CO_NM = "" });
                        //
                        if (CustomerList.Count > 0)
                        {
                            M_SL_CO_CD = CustomerList[0];
                        }
                    }
                }


            }
        }
        #endregion


        #region 수불유형
        //private Dictionary<string, string> _InaudNmMap = new Dictionary<string, string>();
          private IList<SystemCodeVo> _INAUD_NM_LIST = new List<SystemCodeVo>();
          public IList<SystemCodeVo> INAUD_NM_LIST
          {
              get { return _INAUD_NM_LIST; }
              set { SetProperty(ref _INAUD_NM_LIST, value, () => INAUD_NM_LIST); }
          }

          //private CodeDao _M_SL_AREA_NM;
          //public CodeDao M_SL_AREA_NM
          //{
          //    get { return _M_SL_AREA_NM; }
          //    set { SetProperty(ref _M_SL_AREA_NM, value, () => M_SL_AREA_NM, RefreshCoNm); }
          //}

        private SystemCodeVo _M_INAUD_NM;
        public SystemCodeVo M_INAUD_NM
        {
                get { return _M_INAUD_NM; }
                set { SetProperty(ref _M_INAUD_NM, value, () => M_INAUD_NM); }
        }
        #endregion


          //#region 마감
          //private string _M_PUR_CLZ_FLG = string.Empty;
          //public string M_PUR_CLZ_FLG
          //{
          //    get { return _M_PUR_CLZ_FLG; }
          //    set { SetProperty(ref _M_PUR_CLZ_FLG, value, () => M_PUR_CLZ_FLG); }
          //} 
          //#endregion



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
          public IList<InvVo> SelectMstList
          {
              get { return selectedMstList; }
              set { SetProperty(ref selectedMstList, value, () => SelectMstList); }
          }

          InvVo _selectedMstItem;
          public InvVo SelectedMstItem
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
            //거래처
            //CustomerList = SystemProperties.CUSTOMER_CODE_VO(null, null);
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s143", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { DELT_FLG = "N", CO_TP_CD = "AR", AREA_CD = SystemProperties.USER_VO.EMPE_PLC_NM, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.CustomerList = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                    CustomerList.Insert(0, new SystemCodeVo() { CO_NO = "전체", CO_NM = "전체" });
                    if (CustomerList.Count > 0)
                    {
                        M_SL_CO_CD = CustomerList[0];
                    }
                }
            }

            //사업장
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

            //  _InaudNmMap = SystemProperties.SYSTEM_CODE_MAP("L-007");
            //  INAUD_NM_LIST = SystemProperties.SYSTEM_CODE_VO("L-007");
            //  for (int x = (INAUD_NM_LIST.Count-1); x >= 0 ; x--)
            //  {
            //      if (!INAUD_NM_LIST[x].CLSS_CD.StartsWith("R"))
            //      {
            //          INAUD_NM_LIST.Remove(INAUD_NM_LIST[x]);
            //      }
            //  }

            //수불유형
            //AreaList = SystemProperties.SYSTEM_CODE_VO("L-007");
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "L-007"))
            {
                if (response.IsSuccessStatusCode)
                {
                    INAUD_NM_LIST = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList().Where<SystemCodeVo>(x=> x.CLSS_CD.StartsWith("R")).ToList();
                    if (INAUD_NM_LIST.Count > 0)
                    {
                        ////삭제
                        //for (int x = (INAUD_NM_LIST.Count - 1); x >= 0; x--)
                        //{
                        //    if (!INAUD_NM_LIST[x].CLSS_CD.StartsWith("R"))
                        //    {
                        //        INAUD_NM_LIST.Remove(INAUD_NM_LIST[x]);
                        //    }
                        //}

                        M_INAUD_NM = INAUD_NM_LIST[0];
                    }
                }
            }

            //Refresh();
        }

    }
}
