using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.Core;
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
using System.Windows.Input;
using System.Windows.Media;
using AquilaErpWpfApp3.Util;
using AquilaErpWpfApp3.View.SAL.Dialog;

namespace AquilaErpWpfApp3.ViewModel
{
    public sealed class S221111ViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private string title = "출하등록";


        private IList<SaleVo> selectedMstList = new List<SaleVo>();

        private IList<SaleVo> selectedDtlList = new List<SaleVo>();

        private S22111MasterDialog masterDialog;
        private S22111DetailDialog detailDialog;



        public S221111ViewModel()
        {
            EndDt = System.DateTime.Now;
            StartDt = (EndDt).AddMonths(-1);

            //사업장
            //SYSTEM_CODE_VO();
            Refresh();
        }

        [Command]
        public async void Refresh(string _SL_BIL_NO = null)
        {
            try
            {
                //DXSplashScreen.Show<ProgressWindow>();
                //SearchDetail = null;
                //SelectDtlList = null;

                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s221111/mst", new StringContent(JsonConvert.SerializeObject(new SaleVo() { FM_DT = (StartDt).ToString("yyyy-MM-dd"), TO_DT = (EndDt).ToString("yyyy-MM-dd"), CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectMstList = JsonConvert.DeserializeObject<IEnumerable<SaleVo>>(await response.Content.ReadAsStringAsync()).Cast<SaleVo>().ToList();
                    }

                    Title = "[기간]" + (StartDt).ToString("yyyy-MM-dd") + "~" + (EndDt).ToString("yyyy-MM-dd") + ",    [사업장]";

                    if (SelectMstList.Count >= 1)
                    {
                        isM_UPDATE = true;
                        isM_DELETE = true;

                        if (string.IsNullOrEmpty(_SL_BIL_NO))
                        {
                            SelectedMstItem = SelectMstList[0];
                        }
                        else
                        {
                            SelectedMstItem = SelectMstList.Where(x => x.SL_BIL_NO.Equals(_SL_BIL_NO)).LastOrDefault<SaleVo>();
                        }
                    }
                    else
                    {
                        SearchDetail = null;
                        SelectDtlList = null;

                        isM_UPDATE = false;
                        isM_DELETE = false;
                        //
                        isD_UPDATE = false;
                        isD_DELETE = false;
                    }
                }
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }

        [Command]
        public async void SelectMstDetail()
        {
            try
            {
                if (this._selectedMstItem == null)
                {
                    return;
                }

                // 마감후 이월버튼 비활성화 마감전 이월버튼 활성화 2017-05-02
                //if (SelectedMstItem.CLZ_FLG == "Y")
                //{
                //    IsEnabledNextMonth = false;
                //}
                //else if (SelectedMstItem.CLZ_FLG == "N")
                //{
                //    IsEnabledNextMonth = true;
                //}

                //SelectedMstItem.AREA_CD = M_SL_AREA_VO.CLSS_CD;
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s221111/dtl", new StringContent(JsonConvert.SerializeObject(SelectedMstItem), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectDtlList = JsonConvert.DeserializeObject<IEnumerable<SaleVo>>(await response.Content.ReadAsStringAsync()).Cast<SaleVo>().ToList();
                    }

                    if (SelectDtlList.Count >= 1)
                    {
                        isD_UPDATE = true;
                        isD_DELETE = true;
                        isM_DELETE = true;

                        SearchDetail = SelectDtlList[0];
                    }
                    else
                    {
                        isD_UPDATE = false;
                        isD_DELETE = false;
                        isM_DELETE = false;
                    }
                }
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }

        #region MST_Dialog(New, Add, Del)

        [Command]
        public void NewContact()
        {
            masterDialog = new S22111MasterDialog(new SaleVo());
            masterDialog.Title = title + " 마스터 관리 - 추가";
            masterDialog.Owner = Application.Current.MainWindow;
            masterDialog.BorderEffect = BorderEffect.Default;
            masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            bool isDialog = (bool)masterDialog.ShowDialog();
            if (isDialog)
            {
                Refresh(masterDialog.SL_BIL_NO);
            }
        }

        [Command]
        public void EditContact()
        {

            try
            {
                if (SelectedMstItem == null)
                { return; }
                
                if (SelectMstList.Count <= 0)
                { return; }
                SelectedMstItem.CHNL_CD = SystemProperties.USER;
                masterDialog = new S22111MasterDialog(SelectedMstItem);
                masterDialog.Title = title + " 마스터 관리 - 수정 - [요청 번호 명 : " + SelectedMstItem.SL_BIL_NO + "]";
                masterDialog.Owner = Application.Current.MainWindow;
                masterDialog.BorderEffect = BorderEffect.Default;
                masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
                masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
                bool isDialog = (bool)masterDialog.ShowDialog();
                if (isDialog)
                {
                    Refresh(masterDialog.SL_BIL_NO);
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
        public async void MstContact()
        {
            try
            {
                if (SelectedMstItem == null)
                {
                    return;
                }
                if (SelectDtlList.Count > 0)
                {
                    WinUIMessageBox.Show("출고 등록 물품 내역이 존재하여 삭제할 수 없습니다.", "[경고]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }


                //else if (SelectedMstItem.CLZ_FLG.Equals("Y"))
                //{
                //    WinUIMessageBox.Show("[요청 번호 명 : " + SelectedMstItem.SL_RLSE_NM + "] 마감 처리 되었습니다", "[삭제]" + title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                //    return;
                //}

                MessageBoxResult result = WinUIMessageBox.Show("[요청 번호 명 : " + SelectedMstItem.SL_BIL_NO + "]" + " 정말로 삭제 하시겠습니까?", "[삭제]" + title, MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    SelectedMstItem.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s221111/mst/d", new StringContent(JsonConvert.SerializeObject(SelectedMstItem), System.Text.Encoding.UTF8, "application/json")))
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

                            WinUIMessageBox.Show("삭제가 완료되었습니다.", "[삭제]" + title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                            Refresh();
                        }
                    }

                }
            }
            catch (System.Exception eLog)
            {
                //DXSplashScreen.Close();
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }
        #endregion

        [Command]
        public void NewDtlContact()
        {
            if (SelectedMstItem == null)
            {
                return;
            }
            //else if (SelectedMstItem.CLZ_FLG.Equals("Y"))
            //{
            //    WinUIMessageBox.Show("[요청 번호 명 : " + SelectedMstItem.SL_RLSE_NM + "] 마감 처리 되었습니다", "[수주 등록 물품 관리]" + title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
            //    return;
            //}
            SelectedMstItem.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
            detailDialog = new S22111DetailDialog(SelectedMstItem);
            detailDialog.Title = "출하 등록 물품 관리 - [요청 번호 명 : " + SelectedMstItem.SL_BIL_NO + "]";
            detailDialog.Owner = Application.Current.MainWindow;
            detailDialog.BorderEffect = BorderEffect.Default;
            detailDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            detailDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            bool isDialog = (bool)detailDialog.ShowDialog();
            if (isDialog)
            {
                Refresh();
            }
        }

        [Command]
        public async void DelContact()
        {
            try
            {

                if (SelectDtlList.Count < 0)
                {
                    WinUIMessageBox.Show("출고 등록 물품 내역이 존재하지 않습니다.", "[경고]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                IList<SaleVo> DelDtlList = new List<SaleVo>();
                DelDtlList = SelectDtlList.Where<SaleVo>(x => x.isCheckd == true).ToList();
                for(int i =0; i < DelDtlList.Count; i++)
                {
                    DelDtlList[i].CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
                }

                //else if (SelectedMstItem.CLZ_FLG.Equals("Y"))
                //{
                //    WinUIMessageBox.Show("[요청 번호 명 : " + SelectedMstItem.SL_RLSE_NM + "] 마감 처리 되었습니다", "[삭제]" + title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                //    return;
                //}

                MessageBoxResult result = WinUIMessageBox.Show("[ " + DelDtlList.Count + " ] 건을" + " 정말로 삭제 하시겠습니까?", "[삭제]" + title, MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s221111/dtl/d", new StringContent(JsonConvert.SerializeObject(DelDtlList), System.Text.Encoding.UTF8, "application/json")))
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

                            WinUIMessageBox.Show("삭제가 완료되었습니다.", "[삭제]" + title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                            SelectMstDetail();
                        }
                    }

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
        public void CancelContact()
        {
            if (SelectedMstItem == null)
            {
                return;
            }

            try
            {
                MessageBoxResult result = WinUIMessageBox.Show("[요청 번호 명 : " + SelectedMstItem.SL_RLSE_NO + "] 정말로 마감해제 하시겠습니까? ", "[마감해제]" + this.title, MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                //    JobVo resultVo = saleOrderClient.ProcS2211Delete(new JobVo() { SL_RLSE_NO = SelectedMstItem.SL_RLSE_NO, CRE_USR_ID = SystemProperties.USER});
                //    if (!resultVo.isSuccess)
                //    {
                //        //실패
                //        WinUIMessageBox.Show(resultVo.Message, "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
                //        return;
                //    }
                //    WinUIMessageBox.Show("[요청 번호 명 : " + SelectedMstItem.SL_RLSE_NO + "] 마감해제 하였습니다.", "[마감해제]" + title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                //    Refresh();
                }
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }



        [Command]
        public async void SelectDetailCheckd()
        {

            if (SearchDetail == null)
            {
                return;
            }

            //
            if (SearchDetail.isCheckd)
            {
                SearchDetail.isCheckd = false;
            }
            else
            {
                SearchDetail.isCheckd = true;
            }
        }

        //public async void SYSTEM_CODE_VO()
        //{
        //    //SL_AREA_LIST = SystemProperties.SYSTEM_CODE_VO("L-002");
        //    //using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "L-002"))
        //    //{
        //    //    if (response.IsSuccessStatusCode)
        //    //    {
        //    //        SL_AREA_LIST = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
        //    //        if (SL_AREA_LIST.Count > 0)
        //    //        {
        //    //            M_SL_AREA_VO = SL_AREA_LIST[0];
        //    //        }
        //    //    }
        //    //}
        //    Refresh();
        //}





        #region Functon <Binding>
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

        private bool? _isM_Ok = false;
        public bool? isM_Ok
        {
            get { return _isM_Ok; }
            set { SetProperty(ref _isM_Ok, value, () => isM_Ok); }
        }
        private bool? _isM_Cancel = false;
        public bool? isM_Cancel
        {
            get { return _isM_Cancel; }
            set { SetProperty(ref _isM_Cancel, value, () => isM_Cancel); }
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

        private bool? _IsEnabledNextMonth = true;
        public bool? IsEnabledNextMonth
        {
            get { return _IsEnabledNextMonth; }
            set { SetProperty(ref _IsEnabledNextMonth, value, () => IsEnabledNextMonth); }
        }

        //#region Functon <Detail List>
        public IList<SaleVo> SelectDtlList
        {
            get { return selectedDtlList; }
            set { SetProperty(ref selectedDtlList, value, () => SelectDtlList); }
        }

        SaleVo _searchDetail;
        public SaleVo SearchDetail
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

        ////사업장
        //private Dictionary<string, string> _AreaMap = new Dictionary<string, string>();
        //private IList<SystemCodeVo> _AreaCd = new List<SystemCodeVo>();
        //public IList<SystemCodeVo> SL_AREA_LIST
        //{
        //    get { return _AreaCd; }
        //    set { SetProperty(ref _AreaCd, value, () => SL_AREA_LIST); }
        //}

        ////사업장
        //private SystemCodeVo _M_SL_AREA_NM;
        //public SystemCodeVo M_SL_AREA_VO
        //{
        //    get { return _M_SL_AREA_NM; }
        //    set { SetProperty(ref _M_SL_AREA_NM, value, () => M_SL_AREA_VO); }
        //}
        #endregion
    }
}
