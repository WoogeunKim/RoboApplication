using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.WindowsUI;
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
    public sealed class I6617ViewModel : ViewModelBase, INotifyPropertyChanged
    {
        //private static InvServiceClient invClient = SystemProperties.InvClient;
        private IList<InvVo> selectedMstList = new List<InvVo>();

        public I6617ViewModel()
        {
            StartDt = System.DateTime.Now;
            EndDt = System.DateTime.Now;

            M_PUR_CLZ_FLG = "진행";

           
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
                _param.CLZ_FLG = (M_PUR_CLZ_FLG.Equals("전체") ? null : (M_PUR_CLZ_FLG.Equals("진행") ? "N" : "Y"));
                _param.FM_DT = (StartDt).ToString("yyyy-MM-dd");
                _param.TO_DT = (EndDt).ToString("yyyy-MM-dd");
                _param.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
                //_param.CO_CD = (string.IsNullOrEmpty(TXT_DEPT_DESC) ? null : TXT_DEPT_DESC);
                //_param.INAUD_CD = (string.IsNullOrEmpty(TXT_INAUD_NM) ? null : _InaudNmMap[TXT_INAUD_NM]);
                //_param.PUR_CLZ_FLG = (string.IsNullOrEmpty(M_PUR_CLZ_FLG) ? null : M_PUR_CLZ_FLG);


                //SelectMstList = invClient.I6617SelectMstList(_param);
                //{ AREA_CD = (string.IsNullOrEmpty(M_SL_AREA_NM.CLSS_CD) ? null : M_SL_AREA_NM.CLSS_CD), FM_DT = (StartDt).ToString("yyyy-MM-dd"), TO_DT = (EndDt).ToString("yyyy-MM-dd"), PUR_CO_CD = (string.IsNullOrEmpty(M_DEPT_DESC.CO_NO) ? null : M_DEPT_DESC.CO_NO), PUR_CLZ_FLG = (string.IsNullOrEmpty(M_PUR_CLZ_FLG) ? null : M_PUR_CLZ_FLG) }
                ////

                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("i6617/mst", new StringContent(JsonConvert.SerializeObject(_param), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectMstList = JsonConvert.DeserializeObject<IEnumerable<InvVo>>(await response.Content.ReadAsStringAsync()).Cast<InvVo>().ToList();
                    }

                    Title = "[일자]" + (StartDt).ToString("yyyy-MM-dd") + "~" + (EndDt).ToString("yyyy-MM-dd") + ",     [구분]" + (M_PUR_CLZ_FLG);

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
                //DXSplashScreen.Close();
            }
            catch (System.Exception eLog)
            {
                //DXSplashScreen.Close();
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]외주현황", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }

        [Command]
        public async void Apply()
        {
            try
            {
                MessageBoxResult result = WinUIMessageBox.Show("선택한 값을 마감 하시겠습니까?", "[마감]", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    int _Num = 0;
                    string resultMsg;
                    for (int x = 0; x < SelectedMstItems.Count; x++)
                    {
                        SelectedMstItems[x].UPD_USR_ID = SystemProperties.USER;
                        SelectedMstItems[x].CLZ_FLG = "Y";
                        SelectedMstItems[x].CHNL_CD = SystemProperties.USER_VO.CHNL_CD;

                        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("i6617/mst/u", new StringContent(JsonConvert.SerializeObject(SelectedMstItems[x]), System.Text.Encoding.UTF8, "application/json")))
                        {
                            resultMsg = await response.Content.ReadAsStringAsync();
                            if (int.TryParse(resultMsg, out _Num) == false)
                            {
                                //실패
                                WinUIMessageBox.Show(resultMsg, "외주현황", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                        }
                        //invClient.UpdateInudDtl(SelectedMstItems[x]);
                    }
                    WinUIMessageBox.Show("마감하였습니다.", "[마감]", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                    //Refresh();
                }
            }
            catch (System.Exception eLog)
            {
                //DXSplashScreen.Close();
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]외주현황", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }

        [Command]
        public async void Cancel()
        {
            try
            {
                MessageBoxResult result = WinUIMessageBox.Show("선택한 값을 마감취소 하시겠습니까?", "[마감취소]", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    int _Num = 0;
                    string resultMsg;
                    for (int x = 0; x < SelectedMstItems.Count; x++)
                    {
                        SelectedMstItems[x].UPD_USR_ID = SystemProperties.USER;
                        SelectedMstItems[x].CLZ_FLG = "N";
                        SelectedMstItems[x].CHNL_CD = SystemProperties.USER_VO.CHNL_CD;

                        //invClient.UpdateInudDtl(SelectedMstItems[x]);
                        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("i6617/mst/u", new StringContent(JsonConvert.SerializeObject(SelectedMstItems[x]), System.Text.Encoding.UTF8, "application/json")))
                        {
                            resultMsg = await response.Content.ReadAsStringAsync();
                            if (int.TryParse(resultMsg, out _Num) == false)
                            {
                                //실패
                                WinUIMessageBox.Show(resultMsg, "외주현황", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                        }
                    }
                    WinUIMessageBox.Show("마감취소하였습니다.", "[마감취소]", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                    Refresh();
                }
            }
            catch (System.Exception eLog)
            {
                //DXSplashScreen.Close();
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]외주현황", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
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

     
        #region 사업장
        //private Dictionary<string, string> _AreaMap = new Dictionary<string, string>();
        //private IList<CodeDao> _AreaList = new List<CodeDao>();
        //public IList<CodeDao> AreaList
        //{
        //    get { return _AreaList; }
        //    set { SetProperty(ref _AreaList, value, () => AreaList); }
        //}

        //private CodeDao _M_SL_AREA_NM;
        //public CodeDao M_SL_AREA_NM
        //{
        //    get { return _M_SL_AREA_NM; }
        //    set { SetProperty(ref _M_SL_AREA_NM, value, () => M_SL_AREA_NM, RefreshCoNm); }
        //}

        //private string _TXT_SL_AREA_NM = string.Empty;
        //public string TXT_SL_AREA_NM
        //{
        //    get { return _TXT_SL_AREA_NM; }
        //    set { SetProperty(ref _TXT_SL_AREA_NM, value, () => TXT_SL_AREA_NM, RefreshCoNm); }
        //}

        //private void RefreshCoNm()
        //{
        //    //if (TXT_SL_AREA_NM != null)
        //    //{
        //    //    _DeptMap = SystemProperties.CUSTOMER_CODE_MAP(null, _AreaMap[TXT_SL_AREA_NM]);
        //    //    _DeptCd = SystemProperties.CUSTOMER_CODE_VO(null, _AreaMap[TXT_SL_AREA_NM]);
        //    //    _DeptCd.Insert(0, new CustomerCodeDao() { CO_NO = "", CO_NM = "" });

        //    //    DeptList = _DeptCd;
        //    //    //M_DEPT_DESC = DeptList[0];
        //    //    TXT_DEPT_DESC = DeptList[0].CO_NO;
        //    //}
        //}
        #endregion


        //#region 수불유형
        //private Dictionary<string, string> _InaudNmMap = new Dictionary<string, string>();
        //private IList<CodeDao> _INAUD_NM_LIST = new List<CodeDao>();
        //public IList<CodeDao> INAUD_NM_LIST
        //{
        //    get { return _INAUD_NM_LIST; }
        //    set { SetProperty(ref _INAUD_NM_LIST, value, () => INAUD_NM_LIST); }
        //}

        ////private CodeDao _M_SL_AREA_NM;
        ////public CodeDao M_SL_AREA_NM
        ////{
        ////    get { return _M_SL_AREA_NM; }
        ////    set { SetProperty(ref _M_SL_AREA_NM, value, () => M_SL_AREA_NM, RefreshCoNm); }
        ////}

        //private string _TXT_INAUD_NM = string.Empty;
        //public string TXT_INAUD_NM
        //{
        //    get { return _TXT_INAUD_NM; }
        //    set { SetProperty(ref _TXT_INAUD_NM, value, () => TXT_INAUD_NM); }
        //}
        //#endregion


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

        private IList<InvVo> selectedMstItems = new List<InvVo>();
        public IList<InvVo> SelectedMstItems
        {
            get { return selectedMstItems; }
            set { SetProperty(ref selectedMstItems, value, () => SelectedMstItems); }
        }
        #endregion

    }
}
