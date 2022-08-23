using AquilaErpWpfApp3.Util;
using AquilaErpWpfApp3.View.M.Dialog;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Code;
using ModelsLibrary.Man;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Windows;
using AquilaErpWpfApp3.View.M.Report;

namespace AquilaErpWpfApp3.ViewModel
{
    public sealed class M66540ViewModel : ViewModelBase, INotifyPropertyChanged
    {
        //private static PurServiceClient purClient = SystemProperties.PurClient;
        private IList<ManVo> selectedMstList = new List<ManVo>();
        private IList<ManVo> selectedItems = new List<ManVo>();

        private string _title = "작업지지서 발행(사출)";

        private M66540MasterDialog masterDialog;

        //private ICommand _searchDialogCommand;

        public M66540ViewModel() 
        {
            StartDt = System.DateTime.Now;
            EndDt = System.DateTime.Now;

            //M_PUR_CLZ_FLG = "전체";

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
          public async void Refresh()
          {

              try
              {
                DXSplashScreen.Show<ProgressWindow>();
                //SearchDetail = null;
                //SelectDtlList = null;

                ManVo _param = new ManVo();
                    //_param.AREA_CD = (M_SL_AREA_NM == null ? null : M_SL_AREA_NM.CLSS_CD);
                    _param.FM_DT = (StartDt).ToString("yyyyMMdd");
                    _param.TO_DT = (EndDt).ToString("yyyyMMdd");
                    //_param.CO_NO = (M_SL_CO_NM == null ? null : M_SL_CO_NM.CO_NO);
                    //_param.CLZ_FLG = (M_PUR_CLZ_FLG.Equals("전체") ? null : M_PUR_CLZ_FLG);
                    _param.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
                    _param.UPD_USR_ID = SystemProperties.USER;
                    //_param.PCK_FLG = "C";

                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m66540/mst", new StringContent(JsonConvert.SerializeObject(_param), System.Text.Encoding.UTF8, "application/json")))
                    {

                        SelectMstList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
                        Title = "[기간]" + (StartDt).ToString("yyyy-MM-dd") + "~" + (EndDt).ToString("yyyy-MM-dd") + ",      [사업장]" + M_SL_AREA_NM.CLSS_DESC + ",      [구분]충전";

                        if (SelectMstList.Count >= 1)
                        {
                            isM_UPDATE = true;
                            isM_DELETE = false;

                           SelectedMstItem = SelectMstList[0];
                        }
                        else
                        {
                            isM_UPDATE = false;
                            isM_DELETE = false;

                        }
                        DXSplashScreen.Close();
                    }
              }
              catch (System.Exception eLog)
              {
                if (DXSplashScreen.IsActive == true)
                {
                    DXSplashScreen.Close();
                }
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + this._title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                  return;
              }


          }




        [Command]
        public async void NewContact()
        {
            try
            {

                if (this.SelectedMstItem == null) { return; }

                masterDialog = new M66540MasterDialog(SelectedMstItem);
                masterDialog.Title = SelectedMstItem.PROD_PLN_NO + " / " + SelectedMstItem.ITM_NM;
                masterDialog.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                masterDialog.Owner = Application.Current.MainWindow;
                masterDialog.BorderEffect = BorderEffect.Default;
                //masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
                //masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
                bool isDialog = (bool)masterDialog.ShowDialog();
                if (isDialog)
                {
                    SearchDetailItem();
                    //Refresh(SelectedMasterItem.ITM_CD);
                    //if (masterDialog.IsEdit == false)
                    //{
                    //    Refresh();
                    //}
                }
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + this._title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }


        [Command]
        public async void DelContact()
        {
            try
            {
                //ManVo delDao = this._selectMasterItem;
                if (SearchDetailJob != null)
                {
                    MessageBoxResult result = WinUIMessageBox.Show("[" + SearchDetailJob.LOT_DIV_NO + "/" + SearchDetailJob.GBN + "] 정말로 삭제 하시겠습니까?", _title, MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m66540/mst/d", new StringContent(JsonConvert.SerializeObject(SearchDetailJob), System.Text.Encoding.UTF8, "application/json")))
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

                                //성공
                                //WinUIMessageBox.Show("삭제가 완료되었습니다.", _title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);


                                using (HttpResponseMessage responseX = await SystemProperties.PROGRAM_HTTP.PostAsync("m66540/dtl/d", new StringContent(JsonConvert.SerializeObject(SearchDetailJob), System.Text.Encoding.UTF8, "application/json")))
                                {
                                    if (responseX.IsSuccessStatusCode)
                                    {
                                        _Num = 0;
                                        resultMsg = await response.Content.ReadAsStringAsync();
                                        if (int.TryParse(resultMsg, out _Num) == false)
                                        {
                                            //실패
                                            WinUIMessageBox.Show(resultMsg, _title, MessageBoxButton.OK, MessageBoxImage.Error);
                                            return;
                                        }
                                        SearchDetailItem();

                                        //성공
                                        WinUIMessageBox.Show("삭제가 완료되었습니다.", _title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                                    }
                                }
                            }
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
        public async void PrintContact()
        {
            try
            {

                IList<ManVo> _ItmList;
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m66540/report", new StringContent(JsonConvert.SerializeObject(SearchDetailJob), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        _ItmList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();

                        if (_ItmList.Count > 0)
                        {

                            _ItmList[0].LOT_DIV_NO = SearchDetailJob.LOT_DIV_NO;
                            _ItmList[0].FM_DT = System.DateTime.Now.ToString("yyyy-MM-dd");
                            _ItmList[0].MM_01 = "사출";

                            //
                            M665101Report report = new M665101Report(_ItmList);
                            report.Margins.Top = 20;
                            report.Margins.Bottom = 20;
                            report.Margins.Left = 50;
                            report.Margins.Right = 20;
                            report.Landscape = false;

                            report.PrintingSystem.ShowPrintStatusDialog = true;
                            report.PaperKind = System.Drawing.Printing.PaperKind.A4;
                            report.Watermark.Text = Properties.Settings.Default.SettingCompany;
                            report.Watermark.TextDirection = DevExpress.XtraPrinting.Drawing.DirectionMode.ForwardDiagonal;
                            report.Watermark.Font = new System.Drawing.Font(report.PrintingSystem.Watermark.Font.FontFamily, 40);
                            report.Watermark.ForeColor = System.Drawing.Color.PaleTurquoise;
                            report.Watermark.TextTransparency = 150;


                            var window = new DevExpress.Xpf.Printing.DocumentPreviewWindow();
                            window.PreviewControl.DocumentSource = report;
                            report.CreateDocument(true);
                            window.Title = _title;
                            window.Owner = Application.Current.MainWindow;
                            window.ShowDialog();

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
          public IList<ManVo> SelectMstList
          {
              get { return selectedMstList; }
              set { SetProperty(ref selectedMstList, value, () => SelectMstList); }
          }

        ManVo _selectedMstItem;
          public ManVo SelectedMstItem
          {
              get
              {
                  return _selectedMstItem;
              }
              set
              {
                  if (value != null)
                  {
                      SetProperty(ref _selectedMstItem, value, () => SelectedMstItem, SearchDetailItem);
                  }
              }
          }

            public IList<ManVo> SelectedItems
        {
                get { return selectedItems; }
                set { SetProperty(ref selectedItems, value, () => SelectedItems); }
            }

        #endregion


        private async void SearchDetailItem()
        {
            if (SelectedMstItem == null)
            {
                return;
            }

            SelectItmList = null;

            SelectedMstItem.PCK_FLG = "C";
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m66540/dtl", new StringContent(JsonConvert.SerializeObject(SelectedMstItem), System.Text.Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.SelectDtlItmList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
                }
                //SelectDtlItmList = manClient.M6623SelectDetail(SelectedMenuItem);
                if (SelectDtlItmList.Count > 0)
                {
                    //isM_UPDATE = true;
                    isM_DELETE = true;

                    SearchDetailJob = SelectDtlItmList[0];
                }
                else
                {
                    
                    //isM_UPDATE = false;
                    isM_DELETE = false;

                }
            }
        }


        private IList<ManVo> selectDtlItmList;
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
                    SetProperty(ref _searchDetailJob, value, () => SearchDetailJob, SearchItems);
                }
            }
        }


        private IList<ManVo> selectItmList;
        public IList<ManVo> SelectItmList
        {
            get { return selectItmList; }
            set { SetProperty(ref selectItmList, value, () => SelectItmList); }
        }



        private async void SearchItems()
        {
            if (SearchDetailJob == null)
            {
                return;
            }

            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m66540/dtl/two", new StringContent(JsonConvert.SerializeObject(SearchDetailJob), System.Text.Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.SelectItmList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
                }
                //SelectDtlItmList = manClient.M6623SelectDetail(SelectedMenuItem);
                //if (SelectDtlItmList.Count > 0)
                //{
                    //isM_UPDATE = true;
                    //isM_DELETE = true;

                    //SearchDetailJob = SelectDtlItmList[0];
                //}
                //else
               //{

                    //isM_UPDATE = false;
                    //isM_DELETE = false;

                //}
            }
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

        //public ICommand SearchDialogCommand
        //{
        //    get
        //    {
        //        if (_searchDialogCommand == null)
        //            _searchDialogCommand = new DelegateCommand(Refresh);
        //        return _searchDialogCommand;
        //    }
        //}


        //[Command]
        //public async void Ok()
        //{
        //    try
        //    {
        //        //WinUIMessageBox.Show(selectedItems.Count + "   삭제가 완료되었습니다.", "수주마감", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
        //        if (SelectedItems.Count > 0)
        //        {
        //            MessageBoxResult result = WinUIMessageBox.Show("[총 : " + SelectedItems.Count + "] 정말로 마감 하시겠습니까?", "칭량현황", MessageBoxButton.YesNo, MessageBoxImage.Question);
        //            if (result == MessageBoxResult.Yes)
        //            {
        //                int _Num = 0;
        //                string resultMsg = "";
        //                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("p44011/mst/ok", new StringContent(JsonConvert.SerializeObject(SelectedItems), System.Text.Encoding.UTF8, "application/json")))
        //                {
        //                    if (response.IsSuccessStatusCode)
        //                    {
        //                        resultMsg = await response.Content.ReadAsStringAsync();
        //                        if (int.TryParse(resultMsg, out _Num) == false)
        //                        {
        //                            //실패
        //                            WinUIMessageBox.Show(resultMsg, "칭량현황", MessageBoxButton.OK, MessageBoxImage.Error);
        //                            return;
        //                        }
        //                        Refresh();
        //                        //성공
        //                        WinUIMessageBox.Show("마감 완료되었습니다.", "칭량현황", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
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