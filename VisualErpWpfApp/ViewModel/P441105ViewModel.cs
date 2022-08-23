using AquilaErpWpfApp3.Util;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Code;
using ModelsLibrary.Man;
using ModelsLibrary.Pur;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Windows;

namespace AquilaErpWpfApp3.ViewModel
{
    public sealed class P441105ViewModel : ViewModelBase {


        private string _title = "역전개";

        //private static ManServiceClient manClient = SystemProperties.ManClient;
        //private static CodeServiceClient codeClient = SystemProperties.CodeClient;


        private IList<SystemCodeVo> itemN1st;


        private IList<ManVo> selectedMenuViewList;
        private IList<PurVo> selectDtlItmList;

        ////Menu Dialog
        //private ICommand searchDialogCommand;
        //private ICommand newDialogCommand;
        //private ICommand editDialogCommand;
        //private ICommand delDialogCommand;

        //private ICommand fileDownloadCommand;
        //private ICommand classDialogCommand;



        //private M6623MasterDialog masterDialog;
        //private M6623DetailDialog detailDialog;


        public P441105ViewModel()
        {
            //SYSTEM_CODE_VO();
            // - Refresh

        }

        [Command]
        public async void Refresh()
        {
            try
            {
                SelectDtlItmList = null;
                SearchDetailJob = null;

                if (string.IsNullOrEmpty(M_SEARCH_TEXT))
                {
                    WinUIMessageBox.Show("[검색명] 입력 값이 맞지 않습니다", this._title, MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.None, MessageBoxOptions.None);
                    return;
                }


                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("p441105/mst", new StringContent(JsonConvert.SerializeObject(new PurVo() {ITM_CD = M_SEARCH_TEXT, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectDtlItmList = JsonConvert.DeserializeObject<IEnumerable<PurVo>>(await response.Content.ReadAsStringAsync()).Cast<PurVo>().ToList();
                    }
                    //SelectDtlItmList = manClient.M6623SelectDetail(SelectedMenuItem);
                    if (SelectDtlItmList.Count > 0)
                    {
                        isM_UPDATE = true;
                        isM_DELETE = true;

                        SearchDetailJob = SelectDtlItmList[0];
                    }
                    else
                    {

                        isM_UPDATE = false;
                        isM_DELETE = false;

                    }
                }

                //using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6623/mst", new StringContent(JsonConvert.SerializeObject(new ManVo() { ITM_GRP_CLSS_CD = M_AREA_ITEM.CLSS_CD, N1ST_ITM_GRP_CD = M_N1ST_ITM_GRP_ITEM.ITM_GRP_CD, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                //{
                //    if (response.IsSuccessStatusCode)
                //    {
                //        this.SelectedMenuViewList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
                //    }
                //    //SelectedMenuViewList = manClient.M6623SelectMaster(new ManVo() { ITM_GRP_CLSS_CD = M_AREA_ITEM.CLSS_CD, N1ST_ITM_GRP_CD = (M_N1ST_ITM_GRP_ITEM == null ? null : M_N1ST_ITM_GRP_ITEM.CLSS_CD), CHNL_CD = SystemProperties.USER_VO.CHNL_CD });
                //    //SearchMenuContact();
                //    if (SelectedMenuViewList.Count > 0)
                //    {
                //        //isM_UPDATE = true;
                //        //isM_DELETE = true;
                //        if (string.IsNullOrEmpty(_ASSY_ITM_CD_BSE_WEIH_VAL))
                //        {
                //            SelectedMenuItem = SelectedMenuViewList[0];
                //        }
                //        else
                //        {
                //            SelectedMenuItem = SelectedMenuViewList.Where(x => (x.ASSY_ITM_CD + "_" + x.BSE_WEIH_VAL).Equals(_ASSY_ITM_CD_BSE_WEIH_VAL)).LastOrDefault<ManVo>();
                //        }
                //    }
                //    else
                //    {
                //        SelectDtlItmList = null;
                //        SearchDetailJob = null;

                //        isM_UPDATE = false;
                //        isM_DELETE = false;
                //    }
                //}
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }

      
        private IList<SystemCodeVo> _AreaCd = new List<SystemCodeVo>();
        public IList<SystemCodeVo> AreaList
        {
            get { return _AreaCd; }
            set { SetProperty(ref _AreaCd, value, () => AreaList); }
        }

        private SystemCodeVo _M_AREA_ITEM;
        public SystemCodeVo M_AREA_ITEM
        {
            get { return _M_AREA_ITEM; }
            set { SetProperty(ref _M_AREA_ITEM, value, () => M_AREA_ITEM, N1stList); }
        }


        async void N1stList()
        {
            //if (M_AREA_ITEM == null)
            //{
            //    return;
            //}
            ////
            //using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s133", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { DELT_FLG = "N", ITM_GRP_CLSS_CD = M_AREA_ITEM.CLSS_CD, CRE_USR_ID = "", CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
            //{
            //    if (response.IsSuccessStatusCode)
            //    {
            //        N1ST_ITM_GRP_LIST = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
            //        if (N1ST_ITM_GRP_LIST.Count > 0)
            //        {
            //            M_N1ST_ITM_GRP_ITEM = N1ST_ITM_GRP_LIST[0];
            //        }
            //    }
            //}
        }

        public IList<SystemCodeVo> N1ST_ITM_GRP_LIST
        {
            get { return itemN1st; }
            set { SetProperty(ref itemN1st, value, () => N1ST_ITM_GRP_LIST); }
        }

        private SystemCodeVo _M_N1ST_ITM_GRP_ITEM;
        public SystemCodeVo M_N1ST_ITM_GRP_ITEM
        {
            get { return _M_N1ST_ITM_GRP_ITEM; }
            set { SetProperty(ref _M_N1ST_ITM_GRP_ITEM, value, () => M_N1ST_ITM_GRP_ITEM); }
        }



        ////
        //public IList<ManVo> SelectedMenuViewList
        //{
        //    get { return selectedMenuViewList; }
        //    set { SetProperty(ref selectedMenuViewList, value, () => SelectedMenuViewList); }
        //}

        //ManVo _selectMenuItem;
        //public ManVo SelectedMenuItem
        //{
        //    get
        //    {
        //        return _selectMenuItem;
        //    }
        //    set
        //    {
        //        if (value != null)
        //        {
        //            SetProperty(ref _selectMenuItem, value, () => SelectedMenuItem, SearchDetailItem);
        //            //SearchDetailItem();
        //        }
        //    }
        //}

        private async void SearchDetailItem()
        {
            //if (SelectedMenuItem == null)
            //{
            //    return;
            //}

            //using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("p441105/dtl", new StringContent(JsonConvert.SerializeObject(new PurVo() { GBN  = SelectedMenuItem.ASSY_ITM_CD, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
            //{
            //    if (response.IsSuccessStatusCode)
            //    {
            //        this.SelectDtlItmList = JsonConvert.DeserializeObject<IEnumerable<PurVo>>(await response.Content.ReadAsStringAsync()).Cast<PurVo>().ToList();
            //    }
            //    //SelectDtlItmList = manClient.M6623SelectDetail(SelectedMenuItem);
            //    if (SelectDtlItmList.Count > 0)
            //    {
            //        isM_UPDATE = true;
            //        isM_DELETE = true;

            //        SearchDetailJob = SelectDtlItmList[0];
            //    }
            //    else
            //    {

            //        isM_UPDATE = false;
            //        isM_DELETE = false;

            //    }
            //}
        }

        public IList<PurVo> SelectDtlItmList
        {
            get { return selectDtlItmList; }
            set { SetProperty(ref selectDtlItmList, value, () => SelectDtlItmList); }
        }


        PurVo _searchDetailJob;
        public PurVo SearchDetailJob
        {
            get
            {
                return _searchDetailJob;
            }
            set
            {
                if (value != null)
                {
                    SetProperty(ref _searchDetailJob, value, () => SearchDetailJob);
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




        private string _M_SEARCH_TEXT = string.Empty;
        public string M_SEARCH_TEXT
        {
            get { return _M_SEARCH_TEXT; }
            set { SetProperty(ref _M_SEARCH_TEXT, value, () => M_SEARCH_TEXT); }
        }




        public async void SYSTEM_CODE_VO()
        {
            //AreaList = SystemProperties.SYSTEM_CODE_VO("L-001");
            //if (AreaList.Count > 0)
            //{
            //    M_AREA_ITEM = AreaList[3];
            //    TXT_AREA_ITEM = AreaList[3].CLSS_DESC;
            //}
            //using (HttpResponseMessage responseX = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/L-001"))
            //{
            //    if (responseX.IsSuccessStatusCode)
            //    {
            //        AreaList = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await responseX.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
            //        if (AreaList.Count > 0)
            //        {
            //            //M_AREA_ITEM = AreaList[0];
            //            //벌크
            //            M_AREA_ITEM = AreaList[3];
            //            //N1stList();
            //            using (HttpResponseMessage responseY = await SystemProperties.PROGRAM_HTTP.PostAsync("s133", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { DELT_FLG = "N", ITM_GRP_CLSS_CD = M_AREA_ITEM.CLSS_CD, CRE_USR_ID = "", CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
            //            {
            //                if (responseY.IsSuccessStatusCode)
            //                {
            //                    N1ST_ITM_GRP_LIST = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await responseY.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
            //                    if (N1ST_ITM_GRP_LIST.Count > 0)
            //                    {
            //                        M_N1ST_ITM_GRP_ITEM = N1ST_ITM_GRP_LIST[0];
            //                    }
            //                }
            //            }



            //            //비동기 
            //            Refresh();
            //        }

               // }
            //}
        }
    

    }
}
