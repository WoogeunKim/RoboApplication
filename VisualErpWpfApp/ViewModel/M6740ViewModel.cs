using AquilaErpWpfApp3.Util;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Man;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Windows;

namespace AquilaErpWpfApp3.ViewModel
{
    public sealed class M6740ViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private string title = "실적등록";

        private IList<ManVo> selectedMstList = new List<ManVo>();
        private IList<ManVo> selectedDtlList = new List<ManVo>();
        private IList<ManVo> selectedDtlItemsList = new List<ManVo>();

        public M6740ViewModel()
        {
            EndInReqDt = System.DateTime.Now;
            StartInReqDt = (EndInReqDt).AddMonths(-1);
            Refresh();
        }

        [Command]
        public async void Refresh()
        {
            try
            {
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6740/mst", new StringContent(JsonConvert.SerializeObject(new ManVo() { FM_DT = (StartInReqDt).ToString("yyyy-MM-dd"), TO_DT = (EndInReqDt).ToString("yyyy-MM-dd"), CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectMstList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();


                        Title = "[납품요청일]" + (StartInReqDt).ToString("yyyy-MM-dd") + "~" + (EndInReqDt).ToString("yyyy-MM-dd");

                        if (SelectMstList.Count < 1)
                        {
                            SelectedDtlItem = null;
                            SelectDtlList = null;
                        }
                    }
                }
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }
      
        //#region mst,dtl 체크관리
        //[Command]
        //public async void SelectMasterCheckd()
        //{
        //    if (SelectedMstItem == null)
        //    {
        //        return;
        //    }

        //    //
        //    if (SelectedMstItem.isCheckd)
        //    {
        //        SelectedMstItem.isCheckd = false;
        //    }
        //    else
        //    {
        //        SelectedMstItem.isCheckd = true;
        //    }

        //}

        //[Command]
        //public async void SelectDetailCheckd()
        //{

        //    if (SelectedDtlItem == null)
        //    {
        //        return;
        //    }

        //    //
        //    if (SelectedDtlItem.isCheckd)
        //    {
        //        SelectedDtlItem.isCheckd = false;
        //    }
        //    else
        //    {
        //        SelectedDtlItem.isCheckd = true;
        //    }
        //}
        //#endregion

        public async void SelectDetailRefresh()
        {
            try
            {
                if (SelectedMstItem != null)
                {
                    SelectedMstItem.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6740/dtl", new StringContent(JsonConvert.SerializeObject(SelectedMstItem), System.Text.Encoding.UTF8, "application/json")))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            this.SelectDtlList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();

                        }
                    }

                    for(int i = 0; i < SelectDtlList.Count; i++)
                    {
                        if (SelectDtlList[i].PROD_GD_QTY == null)
                        {
                            SelectDtlList[i].PROD_GD_QTY = SelectDtlList[i].PROD_QTY;
                        }
                    }

                }
                else
                {
                    this.SelectDtlList = null;
                    this.SelectedDtlItem = null;
                    return;
                }
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }


        [Command]
        public async void ProdWareHousing()
        {
            try
            {
                IList<ManVo> ProdList = new List<ManVo>();
                ProdList = this.SelectDtlList.Where<ManVo>(w => w.isCheckd == true).ToList<ManVo>();


                for (int i = 0; i < ProdList.Count; i++)
                {
                    ProdList[i].UPD_USR_ID = SystemProperties.USER;
                    ProdList[i].CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
                }

                MessageBoxResult result = WinUIMessageBox.Show(ProdList.Count + "건에 대해 생산입고 하시겠습니까?", title, MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6740/dtl/u", new StringContent(JsonConvert.SerializeObject(ProdList), System.Text.Encoding.UTF8, "application/json")))
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

                            //성공
                            WinUIMessageBox.Show(ProdList.Count + "건이 생산입고 되었습니다.", title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                        }
                    }
                }
            }
            catch(System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }


        //Binding
        #region binding
        public IList<ManVo> SelectDtlList
        {
            get { return selectedDtlList; }
            set { SetProperty(ref selectedDtlList, value, () => SelectDtlList); }
        }

        ManVo _selectedDtlItem;
        public ManVo SelectedDtlItem
        {
            get
            {
                return _selectedDtlItem;
            }
            set
            {
                SetProperty(ref _selectedDtlItem, value, () => SelectedDtlItem);
            }
        }

        public IList<ManVo> SelectedDtlItems
        {
            get { return selectedDtlItemsList; }
            set { SetProperty(ref selectedDtlItemsList, value, () => SelectedDtlItems); }
        }


        string _Title = string.Empty;
        public string Title
        {
            get { return _Title; }
            set { SetProperty(ref _Title, value, () => Title); }
        }

        //납품 요청일
        DateTime _startInReqDt;
        public DateTime StartInReqDt
        {
            get { return _startInReqDt; }
            set { SetProperty(ref _startInReqDt, value, () => StartInReqDt); }
        }

        DateTime _endInReqDt;
        public DateTime EndInReqDt
        {
            get { return _endInReqDt; }
            set { SetProperty(ref _endInReqDt, value, () => EndInReqDt); }
        }

        private string _M_SEARCH_TEXT = string.Empty;
        public string M_SEARCH_TEXT
        {
            get { return _M_SEARCH_TEXT; }
            set { SetProperty(ref _M_SEARCH_TEXT, value, () => M_SEARCH_TEXT); }
        }

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
                    SetProperty(ref _selectedMstItem, value, () => SelectedMstItem, SelectDetailRefresh);
                }
            }
        }

        //private bool? _isGr_DELETE = false;
        //public bool? isGr_DELETE
        //{
        //    get { return _isGr_DELETE; }
        //    set { SetProperty(ref _isGr_DELETE, value, () => isGr_DELETE); }
        //}



        ////사업장
        //private IList<SystemCodeVo> _AreaCd = new List<SystemCodeVo>();
        //public IList<SystemCodeVo> AreaList
        //{
        //    get { return _AreaCd; }
        //    set { SetProperty(ref _AreaCd, value, () => AreaList); }
        //}

        ////사업장
        //private SystemCodeVo _M_SL_AREA_NM;
        //public SystemCodeVo M_SL_AREA_NM
        //{
        //    get { return _M_SL_AREA_NM; }
        //    set { SetProperty(ref _M_SL_AREA_NM, value, () => M_SL_AREA_NM); }
        //}

        #endregion

        //public async void SYSTEM_CODE_VO()
        //{
        //    //    //사업장
        //    //    //AreaList = SystemProperties.SYSTEM_CODE_VO("L-002");
        //    //    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + AquilaErpWpfApp3.Properties.Settings.Default.SettingChnl + "/" + "L-002"))
        //    //    {
        //    //        if (response.IsSuccessStatusCode)
        //    //        {
        //    //            AreaList = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
        //    //            if (AreaList.Count > 0)
        //    //            {
        //    //                M_SL_AREA_NM = AreaList[0];
        //    //            }
        //    //        }
        //    //    }
        //    //    Refresh();
        //}
    }
}
