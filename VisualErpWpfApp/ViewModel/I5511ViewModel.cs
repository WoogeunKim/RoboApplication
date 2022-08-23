using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.Core;
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
using System.Windows.Media;
using AquilaErpWpfApp3.Util;
using AquilaErpWpfApp3.View.INV.Dialog;

namespace AquilaErpWpfApp3.ViewModel
{
    public sealed class I5511ViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private string _title = "품목 입고 관리";

        private IList<InvVo> selectedMstList = new List<InvVo>();
        private IList<InvVo> selectedDtlList = new List<InvVo>();

        private I5511MasterDialog masterDialog;
        //private I5511DetailDialog detailDialog;

        //private I5511DetailEditDialog detailEditDialog;
        private I5511NextMonthDialog masterNextMonthDialog;


        private I5511DetailEtcDialog detailEtcDialog;
        private I5511DetailImpDialog detailImpDialog;
        private I5511DetailOutDialog detailOutDialog;

        //private ICommand _nextMonthDialogCommand;


        public I5511ViewModel() 
        {
            StartDt = System.DateTime.Now;
            EndDt = System.DateTime.Now;

            SYSTEM_CODE_VO();
        }

       [Command]
       public async void Refresh(string _INSRL_NO = null)
        {
            try
            {
                // 김경식
                //DXSplashScreen.Show<ProgressWindow>();
                SearchDetail = null;
                SelectDtlList = null;

                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("i5511/mst", new StringContent(JsonConvert.SerializeObject(new InvVo() {CHNL_CD = SystemProperties.USER_VO.CHNL_CD, FM_DT = (StartDt).ToString("yyyy-MM-dd"), TO_DT = (EndDt).ToString("yyyy-MM-dd"), AREA_CD = M_SL_AREA_NM.CLSS_CD, CO_CD = (M_SL_CO_CD.CO_NO.Equals("전체") ? null : M_SL_CO_CD.CO_NO), CO_TP_CD = (string.IsNullOrEmpty(M_CHECKD) ? null : M_CHECKD) }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectMstList = JsonConvert.DeserializeObject<IEnumerable<InvVo>>(await response.Content.ReadAsStringAsync()).Cast<InvVo>().ToList();
                    }   //////
                    Title = "[기간]" + (StartDt).ToString("yyyy-MM-dd") + "~" + (EndDt).ToString("yyyy-MM-dd") + ",   [사업장]" + M_SL_AREA_NM.CLSS_DESC + ",   [거래처]" + (M_SL_CO_CD.Equals("전체") ? "전체" : M_SL_CO_CD.CO_NM);

                    if (SelectMstList.Count >= 1)
                    {
                        isM_UPDATE = true;
                        isM_DELETE = true;

                        if (string.IsNullOrEmpty(_INSRL_NO))
                        {
                            SelectedMstItem = SelectMstList[0];
                        }
                        else
                        {
                            SelectedMstItem = SelectMstList.Where(x => x.INSRL_NO.Equals(_INSRL_NO)).LastOrDefault<InvVo>();
                        }
                    }
                    else
                    {
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



        //사업장
        //private Dictionary<string, string> _AreaMap = new Dictionary<string, string>();
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

        //구분
        private bool? _M_CHECKD_ALL = true;
        public bool? M_CHECKD_ALL
        {
            get { return _M_CHECKD_ALL; }
            set { SetProperty(ref _M_CHECKD_ALL, value, () => M_CHECKD_ALL, SelectCheckdAll); }
        }
        private bool? _M_CHECKD_PO = false;
        public bool? M_CHECKD_PO
        {
            get { return _M_CHECKD_PO; }
            set { SetProperty(ref _M_CHECKD_PO, value, () => M_CHECKD_PO, SelectCheckdPo); }
        }
        private bool? _M_CHECKD_IV = false;
        public bool? M_CHECKD_IV
        {
            get { return _M_CHECKD_IV; }
            set { SetProperty(ref _M_CHECKD_IV, value, () => M_CHECKD_IV, SelectCheckdIv); }
        }

        private String M_CHECKD = string.Empty;
        private String M_CHECKD_NAME = string.Empty;
        private void SelectCheckdAll()
        {
            if (M_CHECKD_ALL == true)
            {
                //M_CHECKD_ALL = true;
                M_CHECKD_PO = false;
                M_CHECKD_IV = false;
                //
                M_CHECKD = "";
                M_CHECKD_NAME = "전체";
            }

            if (M_CHECKD_ALL == false && M_CHECKD_PO == false && M_CHECKD_IV == false)
            {
                M_CHECKD_ALL = true;
                M_CHECKD_NAME = "전체";
            }

            //else
            //{
            //    M_CHECKD_PO = true;
            //    M_CHECKD = "PO";
            //    M_CHECKD_NAME = "국내";
            //}
        }
        private void SelectCheckdPo()
        {
            if (M_CHECKD_PO == true)
            {
                //M_CHECKD_PO = true;
                M_CHECKD_ALL = false;
                M_CHECKD_IV = false;
                //
                M_CHECKD = "AP";
                M_CHECKD_NAME = "국내";
            }

            if (M_CHECKD_ALL == false && M_CHECKD_PO == false && M_CHECKD_IV == false)
            {
                M_CHECKD_ALL = true;
                M_CHECKD_NAME = "전체";
            }
            //else
            //{
            //    M_CHECKD_ALL = true;
            //    M_CHECKD = "";
            //    M_CHECKD_NAME = "전체";
            //}
        }
        private void SelectCheckdIv()
        {
            if (M_CHECKD_IV == true)
            {
                //M_CHECKD_IV = true;
                M_CHECKD_PO = false;
                M_CHECKD_ALL = false;
                //
                M_CHECKD = "OR";
                M_CHECKD_NAME = "수입";
            }

            if (M_CHECKD_ALL == false && M_CHECKD_PO == false && M_CHECKD_IV == false)
            {
                M_CHECKD_ALL = true;
                M_CHECKD_NAME = "전체";
            }
            //else
            //{
            //    M_CHECKD_ALL = true;
            //    M_CHECKD = "";
            //    M_CHECKD_NAME = "전체";
            //}
        }

        private async void RefreshCoNm()
        {
            if (M_SL_AREA_NM != null)
            {
                //CustomerList = SystemProperties.CUSTOMER_CODE_VO(null, _AreaMap[TXT_SL_AREA_NM]);
                //CustomerList.Insert(0, new CustomerCodeDao() { CO_NO = "전체" });
                //_CustomerMap = SystemProperties.CUSTOMER_CODE_MAP(null, _AreaMap[TXT_SL_AREA_NM]);
                //if (CustomerList.Count > 0)
                //{
                //    M_SL_CO_CD = CustomerList[0].CO_NO;
                //}

                //using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s143", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { DELT_FLG = "N", CO_TP_CD = null, AREA_CD = SystemProperties.USER_VO.EMPE_PLC_NM, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                //{
                //    if (response.IsSuccessStatusCode)
                //    {
                //        CustomerList = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                //        CustomerList.Insert(0, new SystemCodeVo() { CO_NO = "전체", CO_NM = "" });
                //        //
                //        if (CustomerList.Count > 0)
                //        {
                //            M_SL_CO_CD = CustomerList[0];
                //        }
                //    }
                //}


            }
        }

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



        [Command]
        public async void SelectMstDetail()
        {
             try
             {
                 //DXSplashScreen.Show<ProgressWindow>();

                if (this._selectedMstItem == null)
                {
                    return;
                }
                else
                {
                    if (this.SelectedMstItem.CLZ_FLG.Equals("Y"))
                    {
                        isM_UPDATE = false;
                        isM_DELETE = false;
                    }
                    else
                    {
                        isM_UPDATE = true;
                        isM_DELETE = true;
                    }
                }


                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("i5511/dtl", new StringContent(JsonConvert.SerializeObject(SelectedMstItem), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectDtlList = JsonConvert.DeserializeObject<IEnumerable<InvVo>>(await response.Content.ReadAsStringAsync()).Cast<InvVo>().ToList();
                    }
                    //SelectDtlList = new ObservableCollection<PurVo>(purClient.P4401SelectDtlList(new PurVo() { ITM_CD = this._selectedMstItem.}));
                    //SelectDtlList = invClient.I5511SelectDtlList(SelectedMstItem);
                    // //
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
        public IList<InvVo> SelectDtlList
        {
            get { return selectedDtlList; }
            set { SetProperty(ref selectedDtlList, value, () => SelectDtlList); }
        }

        InvVo _searchDetail;
        public InvVo SearchDetail
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
        //#endregion


        string _Title = string.Empty;
        public string Title
        {
            get { return _Title; }
            set { SetProperty(ref _Title, value, () => Title); }
        }



        //#region Functon Command <add, Edit, Del>
       
        [Command]
        public void NewContact()
        {
            masterDialog = new I5511MasterDialog(new InvVo() { INAUD_DT = this.StartDt.ToString("yyyy-MM-dd"), RQST_EMPE_ID = SystemProperties.USER_VO.USR_ID, AREA_NM = SystemProperties.USER_VO.EMPE_PLC_CD, AREA_CD = SystemProperties.USER_VO.EMPE_PLC_NM });
            masterDialog.Title = _title + " - 추가";
            masterDialog.Owner = Application.Current.MainWindow;
            masterDialog.BorderEffect = BorderEffect.Default;
            masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            bool isDialog = (bool)masterDialog.ShowDialog();
            if (isDialog)
            {
                Refresh(masterDialog.INSRL_NO);

                //if (masterDialog.IsEdit == false)
                //{
                //    try
                //    {
                //        DXSplashScreen.Show<ProgressWindow>();
                //        Refresh();
                //        DXSplashScreen.Close();


                //        for (int x = 0; x < SelectMstList.Count; x++)
                //        {
                //            if (masterDialog.INSRL_NO.Equals(SelectMstList[x].INSRL_NO))
                //            {
                //                SelectedMstItem = SelectMstList[x];
                //                return;
                //            }
                //        }

                //    }
                //    catch (System.Exception eLog)
                //    {
                //        DXSplashScreen.Close();
                //        WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                //        return;
                //    }
                //}
            }
        }

        //public ICommand EditDialogCommand
        //{
        //    get
        //    {
        //        if (_editDialogCommand == null)
        //            _editDialogCommand = new DelegateCommand(EditContact);
        //        return _editDialogCommand;
        //    }
        //}
        [Command]
        public void EditContact()
        {
            if (SelectedMstItem == null)
            {
                return;
            }
           
            masterDialog = new I5511MasterDialog(SelectedMstItem);
            masterDialog.Title = _title + " - 수정";
            masterDialog.Owner = Application.Current.MainWindow;
            masterDialog.BorderEffect = BorderEffect.Default;
            masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            bool isDialog = (bool)masterDialog.ShowDialog();
            if (isDialog)
            {
                Refresh(masterDialog.INSRL_NO);
                //if (masterDialog.IsEdit == false)
                //{
                //    try
                //    {
                //        DXSplashScreen.Show<ProgressWindow>();
                //        Refresh();
                //        DXSplashScreen.Close();
                //    }
                //    catch (System.Exception eLog)
                //    {
                //        DXSplashScreen.Close();
                //        WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                //        return;
                //    }
                //}
            }
        }



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
        public async void DelContact()
        {
            if (SelectedMstItem == null) 
            {
                return; 
            }

            // 리스트가 1개 이상이면 삭제 안됨
            //if(SelectDtlList.Count <= 0)
            //{
            //    InvVo delDao = SelectedMstItem;
            //    if (delDao != null)
            //    {
            MessageBoxResult result = WinUIMessageBox.Show("[" + SelectedMstItem.INSRL_NO + "]" + " 정말로 삭제 하시겠습니까?", "[삭제]" + _title, MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("i5511/mst/d", new StringContent(JsonConvert.SerializeObject(SelectedMstItem), System.Text.Encoding.UTF8, "application/json")))
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
                //            try
                //            {
                //                //InvVo resultVo = invClient.I5511DeleteDtl(new InvVo() { INSRL_NO = delDao.INSRL_NO });
                //                //if (!resultVo.isSuccess)
                //                //{
                //                //    //실패
                //                //    WinUIMessageBox.Show(resultVo.Message, "[에러]" + _title, MessageBoxButton.OK, MessageBoxImage.Error);
                //                //    return;
                //                //}


                //                InvVo resultVo = invClient.I5511DeleteMst(new InvVo() { INSRL_NO = delDao.INSRL_NO });
                //                if (!resultVo.isSuccess)
                //                {
                //                    //실패
                //                    WinUIMessageBox.Show(resultVo.Message, "[에러]" + _title, MessageBoxButton.OK, MessageBoxImage.Error);
                //                    return;
                //                }

                //                DXSplashScreen.Show<ProgressWindow>();
                //                SelectMstDetail();
                //                Refresh();
                //                DXSplashScreen.Close();
                //                WinUIMessageBox.Show("삭제가 완료되었습니다.", "[삭제]" + _title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                //            }
                //            catch (System.Exception eLog)
                //            {
                //                DXSplashScreen.Close();
                //                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                //                return;
                //            }
                //        }
                //    }
            }


        }

        [Command]
        public void NewDtlEtcContact()
        {
            try
            {
                if (SelectedMstItem == null)
                {
                    return;
                }

                SelectedMstItem.FM_DT = (StartDt).ToString("yyyy-MM-dd");
                SelectedMstItem.TO_DT = (EndDt).ToString("yyyy-MM-dd");
                //SelectedMstItem.GRP_ID = (string.IsNullOrEmpty(M_DEPT_DESC) ? null : _DeptMap[M_DEPT_DESC]);
                //SelectedMstItem.GRP_NM = M_DEPT_DESC;


                //if (string.IsNullOrEmpty(SelectedMstItem.GRP_ID))
                //{
                //    WinUIMessageBox.Show("[부서]전체를 선택 하실수 없습니다", "[조회 조건]품목 입고 관리", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                //    return;
                //}

                detailEtcDialog = new I5511DetailEtcDialog(SelectedMstItem);
                detailEtcDialog.Title = "기타 입고 자재 관리 - " + SelectedMstItem.INSRL_NO;
                detailEtcDialog.Owner = Application.Current.MainWindow;
                detailEtcDialog.BorderEffect = BorderEffect.Default;
                detailEtcDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
                detailEtcDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
                bool isDialog = (bool)detailEtcDialog.ShowDialog();
                //if (isDialog)
                {
                    SelectMstDetail();
                }
            }
            catch (System.Exception)
            {
                return;
            }
        }

        [Command]
        public void NewDtlImpContact()
        {
            try
            {

                if (SelectedMstItem == null)
                {
                    return;
                }

                SelectedMstItem.FM_DT = (StartDt).ToString("yyyy-MM-dd");
                SelectedMstItem.TO_DT = (EndDt).ToString("yyyy-MM-dd");
                //SelectedMstItem.GRP_ID = (string.IsNullOrEmpty(M_DEPT_DESC) ? null : _DeptMap[M_DEPT_DESC]);
                //SelectedMstItem.GRP_NM = M_DEPT_DESC;


                //if (string.IsNullOrEmpty(SelectedMstItem.GRP_ID))
                //{
                //    WinUIMessageBox.Show("[부서]전체를 선택 하실수 없습니다", "[조회 조건]품목 입고 관리", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                //    return;
                //}

                detailImpDialog = new I5511DetailImpDialog(SelectedMstItem);
                detailImpDialog.Title = "가입고에서 정입고 자재 관리 - " + SelectedMstItem.INSRL_NO;
                detailImpDialog.Owner = Application.Current.MainWindow;
                detailImpDialog.BorderEffect = BorderEffect.Default;
                detailImpDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
                detailImpDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
                bool isDialog = (bool)detailImpDialog.ShowDialog();
                //if (isDialog)
                {
                    SelectMstDetail();
                }
            }
            catch (System.Exception)
            {
                return;
            }
        }


        [Command]
        public void NewDtlOutContact()
        {
            try
            {

                if (SelectedMstItem == null)
                {
                    return;
                }

                SelectedMstItem.FM_DT = (StartDt).ToString("yyyy-MM-dd");
                SelectedMstItem.TO_DT = (EndDt).ToString("yyyy-MM-dd");
                //SelectedMstItem.GRP_ID = (string.IsNullOrEmpty(M_DEPT_DESC) ? null : _DeptMap[M_DEPT_DESC]);
                //SelectedMstItem.GRP_NM = M_DEPT_DESC;


                //if (string.IsNullOrEmpty(SelectedMstItem.GRP_ID))
                //{
                //    WinUIMessageBox.Show("[부서]전체를 선택 하실수 없습니다", "[조회 조건]품목 입고 관리", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                //    return;
                //}

                detailOutDialog = new I5511DetailOutDialog(SelectedMstItem);
                detailOutDialog.Title = "외주 입고 - " + SelectedMstItem.INSRL_NO;
                detailOutDialog.Owner = Application.Current.MainWindow;
                detailOutDialog.BorderEffect = BorderEffect.Default;
                detailOutDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
                detailOutDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
                bool isDialog = (bool)detailOutDialog.ShowDialog();
                //if (isDialog)
                {
                    SelectMstDetail();
                }
            }
            catch (System.Exception)
            {
                return;
            }
        }



        [Command]
        public async void NextMonthContact()
        {
            if (this.SelectedMstItem == null)
            {
                return;
            }
            else if (this.SelectedMstItem.CLZ_FLG.Equals("Y"))
            {
                return;
            }

            masterNextMonthDialog = new I5511NextMonthDialog(SelectedMstItem);
            masterNextMonthDialog.Title = "이월 등록";
            masterNextMonthDialog.Owner = Application.Current.MainWindow;
            masterNextMonthDialog.BorderEffect = BorderEffect.Default;
            masterNextMonthDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            masterNextMonthDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            //detailDialog.IsEdit = false;
            bool isDialog = (bool)masterNextMonthDialog.ShowDialog();
            if (isDialog)
            {
                //if (detailDialog.IsEdit == false)
                //{
                try
                {
                    int _Num = 0;
                    string _INSRL_NO = SelectedMstItem.INSRL_NO;

                    SelectedMstItem.INAUD_RMK = masterNextMonthDialog.NXT_MON_DT + " 일자 이월건";
                    SelectedMstItem.INAUD_DT = masterNextMonthDialog.NXT_MON_DT;
                    SelectedMstItem.CRE_USR_ID = SystemProperties.USER;
                    SelectedMstItem.UPD_USR_ID = SystemProperties.USER;
                    SelectedMstItem.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;

                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("i5511/mst/u", new StringContent(JsonConvert.SerializeObject(SelectedMstItem), System.Text.Encoding.UTF8, "application/json")))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string result = await response.Content.ReadAsStringAsync();
                            if (int.TryParse(result, out _Num) == false)
                            {
                                //실패
                                WinUIMessageBox.Show(result, _title, MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                        }
                        Refresh(_INSRL_NO);
                    }

                    //InvVo resultVo = invClient.I5511UpdateMst(SelectedMstItem);
                    //if (!resultVo.isSuccess)
                    //{
                    //    실패
                    //    WinUIMessageBox.Show(resultVo.Message, "[에러]" + _title, MessageBoxButton.OK, MessageBoxImage.Error);
                    //    //DXSplashScreen.Close();
                    //    return;
                    //}

                }
                catch (System.Exception eLog)
                {
                    WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                    return;
                }
            }

        }

        [Command]
        public async void DelDtlContact()
        {
            try
            {
                if (SearchDetail == null)
                {
                    return;
                }

                MessageBoxResult result = WinUIMessageBox.Show("[" + SearchDetail.INSRL_NO + "/" + SearchDetail.INSRL_SEQ + "]" + " 정말로 삭제 하시겠습니까?", "[삭제]" + _title, MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("i5511/dtl/d", new StringContent(JsonConvert.SerializeObject(SearchDetail), System.Text.Encoding.UTF8, "application/json")))
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
                        }

                        SelectMstDetail();
                        WinUIMessageBox.Show("삭제가 완료되었습니다.", "[삭제]" + _title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                    }
                }
            }
            catch (System.Exception)
            {
                return;
            }
        }

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


            Refresh();

        }
    }
}
