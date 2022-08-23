using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Code;
using ModelsLibrary.Pur;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Windows;
using AquilaErpWpfApp3.Util;
using DevExpress.Xpf.Core;
using AquilaErpWpfApp3.View.PUR.Dialog;
using System.Threading.Tasks;

namespace AquilaErpWpfApp3.ViewModel
{
    public sealed class P441101ViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private P441101MasterDialog masterDialog;

        private P441101MergeDialog mergeDialog;
        //private static PurServiceClient purClient = SystemProperties.PurClient;
        private IList<PurVo> selectedMstList = new List<PurVo>();
        private IList<PurVo> selectedDtlList = new List<PurVo>();
        //private IList<PurVo> selectedItems = new List<PurVo>();
        
        //private ICommand _searchDialogCommand;

        public P441101ViewModel() 
        {
            StartDt = System.DateTime.Now;
            EndDt = System.DateTime.Now;

            M_PUR_CLZ_FLG = "전체";

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


            //  Refresh();
        }

          [Command]
          public async void Refresh(string _SL_ORD_NO = null)
          {

              try
              {
                  DXSplashScreen.Show<ProgressWindow>();
                  //SearchDetail = null;
                  //SelectDtlList = null;

                    PurVo _param = new PurVo();
                    _param.AREA_CD = (M_SL_AREA_NM == null ? null : M_SL_AREA_NM.CLSS_CD);
                    _param.FM_DT = (StartDt).ToString("yyyyMMdd");
                    _param.TO_DT = (EndDt).ToString("yyyyMMdd");
                    _param.CO_NO = (M_SL_CO_NM == null ? null : M_SL_CO_NM.CO_NO);
                    _param.CLZ_FLG = (M_PUR_CLZ_FLG.Equals("전체") ? null : M_PUR_CLZ_FLG);
                    _param.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
                    _param.UPD_USR_ID = SystemProperties.USER;

                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("p441101/mst", new StringContent(JsonConvert.SerializeObject(_param), System.Text.Encoding.UTF8, "application/json")))
                    {

                        if (response.IsSuccessStatusCode)
                        {
                            SelectMstList = JsonConvert.DeserializeObject<IEnumerable<PurVo>>(await response.Content.ReadAsStringAsync()).Cast<PurVo>().ToList();
                        }
                        Title = "[수주기간]" + (StartDt).ToString("yyyy-MM-dd") + "~" + (EndDt).ToString("yyyy-MM-dd") + ",      [사업장]" + M_SL_AREA_NM.CLSS_DESC ;

                        if (SelectMstList.Count >= 1)
                        {
                            isM_UPDATE = true;
                            isM_DELETE = true;

                            if (string.IsNullOrEmpty(_SL_ORD_NO))
                            {
                               SelectedMstItem = SelectMstList[0];
                            }
                            else
                            {
                            SelectedMstItem = SelectMstList.Where(x => (x.SL_ORD_NO + "_" + x.SL_ORD_SEQ).Equals(_SL_ORD_NO)).LastOrDefault<PurVo>();
                            }
                        }
                        else
                        {
                            isM_UPDATE = false;
                            isM_DELETE = false;

                        }
                    }
                    DXSplashScreen.Close();
              }
              catch (System.Exception eLog)
              {
                if (DXSplashScreen.IsActive == true)
                {
                    DXSplashScreen.Close();
                }
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]부자재 소요량 전개", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                  return;
              }


          }

          #region 수주기간 From To

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
                //using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s143", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { DELT_FLG = "N", /*CO_TP_CD = "AR", */ AREA_CD = M_SL_AREA_NM.CLSS_CD, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
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
            }
        }
        #endregion

        #region 마감
        private string _M_PUR_CLZ_FLG = "Y";
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

          //Mst
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
                  if (value != null)
                  {
                      SetProperty(ref _selectedMstItem, value, () => SelectedMstItem, onSelDtl);
                  }
              }
          }

        private async void onSelDtl()
        {
            try
            {
                if (SelectedMstItem == null)
                {
                    return;
                }

                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("p441101/dtl", new StringContent(JsonConvert.SerializeObject(SelectedMstItem), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        SelectDtlList = JsonConvert.DeserializeObject<IEnumerable<PurVo>>(await response.Content.ReadAsStringAsync()).Cast<PurVo>().ToList();
                    }
                }
            }
            catch (System.Exception eLog)
            {
                //DXSplashScreen.Close();
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]부자재 소요량 전개", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }

        //Dtl
        public IList<PurVo> SelectDtlList
         {
            get { return selectedDtlList; }
            set { SetProperty(ref selectedDtlList, value, () => SelectDtlList); }
         }

         PurVo _selectedDtlItem;
         public PurVo SelectedDtlItem
         {
            get
            {
                return _selectedDtlItem;
            }
            set
            {
                if (value != null)
                {
                    SetProperty(ref _selectedDtlItem, value, () => SelectedDtlItem);
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

            //Refresh();
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


        [Command]
        public async void Ok()
        {
            try
            {
                if (SelectedMstItem == null)
                {
                    return;
                }

                //
                MessageBoxResult result = WinUIMessageBox.Show("[" +SelectedMstItem.SL_ORD_NO + "/" + SelectedMstItem.SL_ORD_SEQ + "] " + SelectedMstItem.ITM_NM + " 소요량 전개 하시겠습니까?", "부자재 소요량 전개", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    int _Num = 0;
                    string resultMsg = "";
                    string _SL_ORD_NO = SelectedMstItem.SL_ORD_NO + "_" + SelectedMstItem.SL_ORD_SEQ;
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("p441101/proc", new StringContent(JsonConvert.SerializeObject(SelectedMstItem), System.Text.Encoding.UTF8, "application/json")))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            resultMsg = await response.Content.ReadAsStringAsync();
                            if (int.TryParse(resultMsg, out _Num) == false)
                            {
                                //실패
                                WinUIMessageBox.Show(resultMsg, "부자재 소요량 전개", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }


                            Task.Delay(4500).Wait();
                            Refresh(_SL_ORD_NO);

                            //성공
                            WinUIMessageBox.Show("완료되었습니다.", "부자재 소요량 전개", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                        }
                    }
                }
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]부자재 소요량 전개", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }




        [Command]
        public async void Custemer()
        {
            try
            {
                if (SelectedDtlItem == null)
                {
                    return;
                }
                //else if (SelectedDtlItem.ITM_GRP_CLSS_NM.Equals("벌크(반제품)"))
                //{
                //    WinUIMessageBox.Show("벌크(반제품) - 거래처를 수정 하실수 없습니다", "[" + SystemProperties.PROGRAM_TITLE + "]원자재 소요량 전개", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                //    return;
                //}


                this.masterDialog = new P441101MasterDialog(SelectedDtlItem);
                //조회
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("p4401/dtl", new StringContent(JsonConvert.SerializeObject(SelectedDtlItem), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.masterDialog.ViewGridDtl.ItemsSource = JsonConvert.DeserializeObject<List<PurVo>>(await response.Content.ReadAsStringAsync()).Cast<PurVo>().ToList();
                        this.masterDialog.ViewGridDtl.RefreshData();
                    }

                }

                this.masterDialog.Title = "부자재 소요량 전개 - 거래처";
                this.masterDialog.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                this.masterDialog.Owner = Application.Current.MainWindow;
                this.masterDialog.BorderEffect = BorderEffect.Default;
                //masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
                //masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
                bool isDialog = (bool)this.masterDialog.ShowDialog();
                if (isDialog)
                {
                    try
                    {
                        //
                        int _Num = 0;
                        string resultMsg = "";
                        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("p441101/dtl/u", new StringContent(JsonConvert.SerializeObject(SelectedDtlItem), System.Text.Encoding.UTF8, "application/json")))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                resultMsg = await response.Content.ReadAsStringAsync();
                                if (int.TryParse(resultMsg, out _Num) == false)
                                {
                                    //실패
                                    WinUIMessageBox.Show(resultMsg, "[" + SystemProperties.PROGRAM_TITLE + "]부자재 소요량 전개", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                                    return;
                                }
                            }
                        }
                    }
                    catch (System.Exception eLog)
                    {
                        WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]부자재 소요량 전개", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                        return;
                    }
                }
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]부자재 소요량 전개", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }

        [Command]
        public async void Merge()
        {
            try
            {
                if (SelectedMstItem == null)
                {
                    return;
                }

                this.mergeDialog = new P441101MergeDialog(SelectMstList);
                this.mergeDialog.Title = "부자재 소요량 전개 - 소요량 취합등록";
                this.mergeDialog.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                this.mergeDialog.Owner = Application.Current.MainWindow;
                this.mergeDialog.BorderEffect = BorderEffect.Default;
                //masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
                //masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
                bool isDialog = (bool)this.mergeDialog.ShowDialog();
                if (isDialog)
                {





                }
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]부자재 소요량 전개", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }


        //[Command]
        //public async void Cancel()
        //{
        //    try
        //    {
        //        if (SelectedItems.Count > 0)
        //        {
        //            MessageBoxResult result = WinUIMessageBox.Show("[총 : " + SelectedItems.Count + "] 정말로 취소 하시겠습니까?", "수주마감", MessageBoxButton.YesNo, MessageBoxImage.Question);
        //            if (result == MessageBoxResult.Yes)
        //            {
        //                int _Num = 0;
        //                string resultMsg = "";
        //                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("p44011/mst/cancel", new StringContent(JsonConvert.SerializeObject(SelectedItems), System.Text.Encoding.UTF8, "application/json")))
        //                {
        //                    if (response.IsSuccessStatusCode)
        //                    {
        //                        resultMsg = await response.Content.ReadAsStringAsync();
        //                        if (int.TryParse(resultMsg, out _Num) == false)
        //                        {
        //                            //실패
        //                            WinUIMessageBox.Show(resultMsg, "수주마감", MessageBoxButton.OK, MessageBoxImage.Error);
        //                            return;
        //                        }
        //                        Refresh();
        //                        //성공
        //                        WinUIMessageBox.Show("취소 완료되었습니다.", "수주마감", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
        //                    }
        //                }
        //                //Refresh();
        //            }
        //        }
        //    }
        //    catch (System.Exception eLog)
        //    {
        //        WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]수주마감", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
        //        return;
        //    }
        //}



    }
}