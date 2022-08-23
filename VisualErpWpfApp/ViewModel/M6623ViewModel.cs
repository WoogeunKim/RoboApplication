using AquilaErpWpfApp3.Util;
using AquilaErpWpfApp3.View.M.Dialog;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Code;
using ModelsLibrary.Man;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Media;

namespace AquilaErpWpfApp3.ViewModel
{
    public sealed class M6623ViewModel  : ViewModelBase, INotifyPropertyChanged {


        private string _title = "레시피자료관리";

        //private static ManServiceClient manClient = SystemProperties.ManClient;
        //private static CodeServiceClient codeClient = SystemProperties.CodeClient;


        private IList<SystemCodeVo> itemN1st;


        private IList<ManVo> selectedMenuViewList;
        private IList<ManVo> selectDtlItmList;

        ////Menu Dialog
        //private ICommand searchDialogCommand;
        //private ICommand newDialogCommand;
        //private ICommand editDialogCommand;
        //private ICommand delDialogCommand;

        //private ICommand fileDownloadCommand;
        //private ICommand classDialogCommand;



        private M6623MasterDialog masterDialog;
        private M6623DetailDialog detailDialog;


        public M6623ViewModel() 
        {
            SYSTEM_CODE_VO();
            // - Refresh

        }

        [Command]
        public async void Refresh(string _ASSY_ITM_CD_BSE_WEIH_VAL = null)
        {
            try
            {
                SelectDtlItmList = null;
                SearchDetailJob = null;
                
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6623/mst", new StringContent(JsonConvert.SerializeObject(new ManVo() { ITM_GRP_CLSS_CD = M_AREA_ITEM.CLSS_CD, N1ST_ITM_GRP_CD = M_N1ST_ITM_GRP_ITEM?.ITM_GRP_CD, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectedMenuViewList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
                    }
                    //SelectedMenuViewList = manClient.M6623SelectMaster(new ManVo() { ITM_GRP_CLSS_CD = M_AREA_ITEM.CLSS_CD, N1ST_ITM_GRP_CD = (M_N1ST_ITM_GRP_ITEM == null ? null : M_N1ST_ITM_GRP_ITEM.CLSS_CD), CHNL_CD = SystemProperties.USER_VO.CHNL_CD });
                    //SearchMenuContact();
                    if (SelectedMenuViewList.Count > 0)
                    {
                        //isM_UPDATE = true;
                        //isM_DELETE = true;
                        if (string.IsNullOrEmpty(_ASSY_ITM_CD_BSE_WEIH_VAL))
                        {
                            SelectedMenuItem = SelectedMenuViewList[0];
                        }
                        else
                        {
                            SelectedMenuItem = SelectedMenuViewList.Where(x => (x.ASSY_ITM_CD+"_"+x.BSE_WEIH_VAL).Equals(_ASSY_ITM_CD_BSE_WEIH_VAL)).LastOrDefault<ManVo>();
                        }
                    }
                    else
                    {
                        SelectDtlItmList = null;
                        SearchDetailJob = null;

                        isM_UPDATE = false;
                        isM_DELETE = false;
                    }
                }
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }

        #region Functon (Menu Add, Edit, Del)
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
            if (M_AREA_ITEM == null)
            {
                return;
            }
            //
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s133", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { DELT_FLG = "N", ITM_GRP_CLSS_CD = M_AREA_ITEM.CLSS_CD, CRE_USR_ID = "", CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    N1ST_ITM_GRP_LIST = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                    if (N1ST_ITM_GRP_LIST.Count > 0)
                    {
                        M_N1ST_ITM_GRP_ITEM = N1ST_ITM_GRP_LIST[0];
                    }
                }
            }
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



        //
        public IList<ManVo> SelectedMenuViewList
        {
            get { return selectedMenuViewList; }
            set { SetProperty(ref selectedMenuViewList, value, () => SelectedMenuViewList); }
        }

        ManVo _selectMenuItem;
        public ManVo SelectedMenuItem
        {
            get
            {
                return _selectMenuItem;
            }
            set
            {
                if (value != null)
                {
                    SetProperty(ref _selectMenuItem, value, () => SelectedMenuItem, SearchDetailItem);
                    //SearchDetailItem();
                }
            }
        }

        private async void SearchDetailItem()
        {
            if (SelectedMenuItem == null)
            {
                return;
            }

            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6623/dtl", new StringContent(JsonConvert.SerializeObject(SelectedMenuItem), System.Text.Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.SelectDtlItmList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
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
        }

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
                    SetProperty(ref _searchDetailJob, value, () => SearchDetailJob);
                }
            }
        }

        //public ICommand SearchDialogCommand
        //{
        //    get
        //    {
        //        if (searchDialogCommand == null)
        //            searchDialogCommand = new DelegateCommand(SearchMenuContact);
        //        return searchDialogCommand;
        //    }
        //}

        //public void SearchMenuContact()
        //{
        //    try
        //    {
        //        Refresh();
        //    }
        //    catch
        //    {
        //        return;
        //    }
        //}

        //public ICommand NewDialogCommand
        //{
        //    get
        //    {
        //        if (newDialogCommand == null)
        //            newDialogCommand = new DelegateCommand(NewContact);
        //        return newDialogCommand;
        //    }
        //}

        [Command]
        public void NewContact(string _parameter)
        {
            if (M_AREA_ITEM == null)
            {
                return;
            }

            if (_parameter.Equals("A"))
            {
                masterDialog = new M6623MasterDialog(new ManVo() { ITM_GRP_CLSS_NM = M_AREA_ITEM.CLSS_DESC, ITM_GRP_CLSS_CD = M_AREA_ITEM.CLSS_CD, N1ST_ITM_GRP_CD = M_N1ST_ITM_GRP_ITEM?.ITM_GRP_CD, N1ST_ITM_GRP_NM = M_N1ST_ITM_GRP_ITEM?.ITM_GRP_NM, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }, false);
                masterDialog.Title = _title + " - 추가";
                masterDialog.Owner = Application.Current.MainWindow;
                masterDialog.BorderEffect = BorderEffect.Default;
                masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
                masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
                bool isDialog = (bool)masterDialog.ShowDialog();
                if (isDialog)
                {
                    Refresh(masterDialog.resultVo.ASSY_ITM_CD + "_" + masterDialog.resultVo.BSE_WEIH_VAL);
                    //if (masterDialog.IsEdit == false)
                    //{
                    //    SearchMenuContact();

                    //    for (int x = 0; x < SelectedMenuViewList.Count; x++)
                    //    {
                    //        if ((SelectedMenuViewList[x].ASSY_ITM_CD + "_" + SelectedMenuViewList[x].BSE_WEIH_VAL).Equals(masterDialog.resultVo.ASSY_ITM_CD + "_" + masterDialog.resultVo.BSE_WEIH_VAL))
                    //        {
                    //            SelectedMenuItem = SelectedMenuViewList[x];
                    //            break;
                    //        }
                    //    }
                    //}
                }
            } 
            else if (_parameter.Equals("B"))
            {
                masterDialog = new M6623MasterDialog(SearchDetailJob, false);
                masterDialog.Title = _title + " - 추가";
                masterDialog.Owner = Application.Current.MainWindow;
                masterDialog.BorderEffect = BorderEffect.Default;
                masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
                masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
                bool isDialog = (bool)masterDialog.ShowDialog();
                if (isDialog)
                {
                    SearchDetailItem();
                    //Refresh(masterDialog.resultVo.ASSY_ITM_CD + "_" + masterDialog.resultVo.BSE_WEIH_VAL);
                    //if (masterDialog.IsEdit == false)
                    //{
                    //    SearchMenuContact();

                    //    for (int x = 0; x < SelectedMenuViewList.Count; x++)
                    //    {
                    //        if ((SelectedMenuViewList[x].ASSY_ITM_CD + "_" + SelectedMenuViewList[x].BSE_WEIH_VAL).Equals(masterDialog.resultVo.ASSY_ITM_CD + "_" + masterDialog.resultVo.BSE_WEIH_VAL))
                    //        {
                    //            SelectedMenuItem = SelectedMenuViewList[x];
                    //            break;
                    //        }
                    //    }
                    //}
                }
            }
        }

        //public ICommand EditDialogCommand
        //{
        //    get
        //    {
        //        if (editDialogCommand == null)
        //            editDialogCommand = new DelegateCommand(EditMasterContact);
        //        return editDialogCommand;
        //    }
        //}
        [Command]
        public void EditMasterContact()
        {
            if (SearchDetailJob == null) { return; }
            masterDialog = new M6623MasterDialog(SearchDetailJob, true);
            masterDialog.Title = _title + " - 수정";
            masterDialog.Owner = Application.Current.MainWindow;
            masterDialog.BorderEffect = BorderEffect.Default;
            masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            bool isDialog = (bool)masterDialog.ShowDialog();
            if (isDialog)
            {
                SearchDetailItem();
                //Refresh(masterDialog.resultVo.ASSY_ITM_CD + "_" + masterDialog.resultVo.BSE_WEIH_VAL);
                //if (masterDialog.IsEdit == true)
                //{
                //  SelectedMenuItem.DELT_FLG = SelectedMenuItem.DELT_FLG;
                //}
            }

        }

        //public ICommand DelDialogCommand
        //{
        //    get
        //    {
        //        if (delDialogCommand == null)
        //            delDialogCommand = new DelegateCommand(DelMasterContact);
        //        return delDialogCommand;
        //    }
        //}

        [Command]
        public async void DelMasterContact(string _parameter)
        {


            ManVo delDao = SearchDetailJob;
            if (delDao != null)
            {

                if (_parameter.Equals("A"))
                {
                    MessageBoxResult result = WinUIMessageBox.Show(" [순번 : " + delDao.ASSY_ITM_SEQ + "]" + delDao.CMPO_NM + " 정말로 삭제 하시겠습니까?", "[삭제]" + _title, MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {

                        string _ASSY_ITM_CD = SelectedMenuItem.ASSY_ITM_CD + "_" + SelectedMenuItem.BSE_WEIH_VAL;
                        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6623/mst/d", new StringContent(JsonConvert.SerializeObject(delDao), System.Text.Encoding.UTF8, "application/json")))
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
                                SearchDetailItem();
                                // Refresh(_ASSY_ITM_CD);
                                //Refresh();
                                ////
                                ////비동기 => 동기화 작업
                                ////Task.Run(() => Refresh()).Wait();
                                //if (SelectedMenuViewList.Count > 0)
                                //{
                                //    SelectedMenuItem = SelectedMenuViewList.Where<ManVo>(x => x.ASSY_ITM_CD.Equals(_ASSY_ITM_CD)).FirstOrDefault<ManVo>();
                                //}

                                //SearchDetailItem();
                                //성공
                                WinUIMessageBox.Show("삭제가 완료되었습니다.", _title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                            }
                        }
                    }

                }
                else if (_parameter.Equals("B"))
                {
                    MessageBoxResult result = WinUIMessageBox.Show("[기준 중량 : " + delDao.BSE_WEIH_VAL + "] " + delDao.ASSY_ITM_CD + " 정말로 삭제 하시겠습니까?", "[벌크 삭제]" + _title, MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        //string _ASSY_ITM_CD = SelectedMenuItem.ASSY_ITM_CD + "_" + SelectedMenuItem.BSE_WEIH_VAL;
                        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6623/mst/d", new StringContent(JsonConvert.SerializeObject(new ManVo() { ASSY_ITM_CD = delDao.ASSY_ITM_CD, BSE_WEIH_VAL = delDao.BSE_WEIH_VAL, CHNL_CD = delDao.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
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
                                //SearchDetailItem();
                                // Refresh(_ASSY_ITM_CD);
                                Refresh();
                                ////
                                ////비동기 => 동기화 작업
                                ////Task.Run(() => Refresh()).Wait();
                                //if (SelectedMenuViewList.Count > 0)
                                //{
                                //    SelectedMenuItem = SelectedMenuViewList.Where<ManVo>(x => x.ASSY_ITM_CD.Equals(_ASSY_ITM_CD)).FirstOrDefault<ManVo>();
                                //}

                                //SearchDetailItem();
                                //성공
                                WinUIMessageBox.Show("삭제가 완료되었습니다.", _title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                            }
                        }
                    }
                }
            }
        }
        #endregion


        [Command]
        public async void UpSeq()
        {
            try
            {
                if (SearchDetailJob == null) { return; }

                string _CMPO_CD = SearchDetailJob.CMPO_CD;

                int _Num = 0;
                int _FindIdx = SelectDtlItmList.IndexOf(SearchDetailJob);
                if (_FindIdx <= 0)
                {
                    SearchDetailJob.ASSY_ITM_SEQ = 1;
                }
                else
                {
                    int? _Temp = SelectDtlItmList[_FindIdx].ASSY_ITM_SEQ;

                    SelectDtlItmList[_FindIdx].ASSY_ITM_SEQ = SelectDtlItmList[_FindIdx - 1].ASSY_ITM_SEQ;
                    SelectDtlItmList[_FindIdx].CRE_USR_ID = SystemProperties.USER;
                    SelectDtlItmList[_FindIdx].UPD_USR_ID = SystemProperties.USER;
                    //삭제
                    using (HttpResponseMessage response_X = await SystemProperties.PROGRAM_HTTP.PostAsync("m6623/mst/d", new StringContent(JsonConvert.SerializeObject(SelectDtlItmList[_FindIdx]), System.Text.Encoding.UTF8, "application/json")))
                    {
                        if (response_X.IsSuccessStatusCode)
                        {
                            //저장
                            using (HttpResponseMessage response_Y = await SystemProperties.PROGRAM_HTTP.PostAsync("m6623/mst/i", new StringContent(JsonConvert.SerializeObject(SelectDtlItmList[_FindIdx]), System.Text.Encoding.UTF8, "application/json")))
                            {
                                if (response_Y.IsSuccessStatusCode)
                                {
                                    string result = await response_Y.Content.ReadAsStringAsync();
                                    if (int.TryParse(result, out _Num) == false)
                                    {
                                        //실패
                                        WinUIMessageBox.Show(result, _title, MessageBoxButton.OK, MessageBoxImage.Error);
                                        return;
                                    }
                                }
                            }
                        }
                    }

                    SelectDtlItmList[_FindIdx - 1].ASSY_ITM_SEQ = _Temp;
                    SelectDtlItmList[_FindIdx - 1].CRE_USR_ID = SystemProperties.USER;
                    SelectDtlItmList[_FindIdx - 1].UPD_USR_ID = SystemProperties.USER;
                    //삭제
                    using (HttpResponseMessage response_X = await SystemProperties.PROGRAM_HTTP.PostAsync("m6623/mst/d", new StringContent(JsonConvert.SerializeObject(SelectDtlItmList[_FindIdx - 1]), System.Text.Encoding.UTF8, "application/json")))
                    {
                        if (response_X.IsSuccessStatusCode)
                        {
                            //저장
                            using (HttpResponseMessage response_Y = await SystemProperties.PROGRAM_HTTP.PostAsync("m6623/mst/i", new StringContent(JsonConvert.SerializeObject(SelectDtlItmList[_FindIdx - 1]), System.Text.Encoding.UTF8, "application/json")))
                            {
                                if (response_Y.IsSuccessStatusCode)
                                {
                                    string result = await response_Y.Content.ReadAsStringAsync();
                                    if (int.TryParse(result, out _Num) == false)
                                    {
                                        //실패
                                        WinUIMessageBox.Show(result, _title, MessageBoxButton.OK, MessageBoxImage.Error);
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }


                //조회
                if (SelectedMenuItem == null)
                {
                    return;
                }

                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6623/dtl", new StringContent(JsonConvert.SerializeObject(SelectedMenuItem), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectDtlItmList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
                    }
                    //
                    if (SelectDtlItmList.Count > 0)
                    {
                        //isM_UPDATE = true;
                        //isM_DELETE = true;

                        SearchDetailJob = SelectDtlItmList.Where<ManVo>(x => x.CMPO_CD.Equals(_CMPO_CD)).FirstOrDefault<ManVo>();
                    }
                    //else
                    //{

                        //isM_UPDATE = false;
                        //isM_DELETE = false;

                    //}
                }




                ////비동기 => 동기화 작업
                //var task1 =  Task.Run(() => SearchDetailItem() );
                //await task1;
                ////SearchDetailItem();
                ////Task.Run(() =>
                ////    {
                //if (SelectDtlItmList.Count > 0)
                //{
                //    SearchDetailJob = SelectDtlItmList.Where<ManVo>(x => x.CMPO_CD.Equals(_CMPO_CD)).FirstOrDefault<ManVo>();
                //}
                //    }
                //).Wait();
            }
            catch
            {
                return;
            }
        }


        [Command]
        public async void DownSeq()
        {
            try
            {
                if (SearchDetailJob == null) { return; }

                string _CMPO_CD = SearchDetailJob.CMPO_CD;

                int _Num = 0;
                int _FindIdx = SelectDtlItmList.IndexOf(SearchDetailJob);
                if (_FindIdx >= SelectDtlItmList.Count - 1)
                {
                    SearchDetailJob.ROUT_SEQ = SelectDtlItmList.Count;
                }
                else
                {
                    int? _Temp = SelectDtlItmList[_FindIdx].ASSY_ITM_SEQ;

                    //SelectDtlItmList[_FindIdx].RN = SelectDtlItmList[_FindIdx + 1].ASSY_ITM_SEQ;
                    SelectDtlItmList[_FindIdx].ASSY_ITM_SEQ = SelectDtlItmList[_FindIdx + 1].ASSY_ITM_SEQ;
                    SelectDtlItmList[_FindIdx].CRE_USR_ID = SystemProperties.USER;
                    SelectDtlItmList[_FindIdx].UPD_USR_ID = SystemProperties.USER;

                    //삭제
                    using (HttpResponseMessage response_X = await SystemProperties.PROGRAM_HTTP.PostAsync("m6623/mst/d", new StringContent(JsonConvert.SerializeObject(SelectDtlItmList[_FindIdx]), System.Text.Encoding.UTF8, "application/json")))
                    {
                        if (response_X.IsSuccessStatusCode)
                        {
                            //저장
                            using (HttpResponseMessage response_Y = await SystemProperties.PROGRAM_HTTP.PostAsync("m6623/mst/i", new StringContent(JsonConvert.SerializeObject(SelectDtlItmList[_FindIdx]), System.Text.Encoding.UTF8, "application/json")))
                            {
                                if (response_Y.IsSuccessStatusCode)
                                {
                                    string result = await response_Y.Content.ReadAsStringAsync();
                                    if (int.TryParse(result, out _Num) == false)
                                    {
                                        //실패
                                        WinUIMessageBox.Show(result, _title, MessageBoxButton.OK, MessageBoxImage.Error);
                                        return;
                                    }
                                }
                            }
                        }
                    }


                    SelectDtlItmList[_FindIdx + 1].ASSY_ITM_SEQ = _Temp;
                    SelectDtlItmList[_FindIdx + 1].CRE_USR_ID = SystemProperties.USER;
                    SelectDtlItmList[_FindIdx + 1].UPD_USR_ID = SystemProperties.USER;
                    //삭제
                    using (HttpResponseMessage response_X = await SystemProperties.PROGRAM_HTTP.PostAsync("m6623/mst/d", new StringContent(JsonConvert.SerializeObject(SelectDtlItmList[_FindIdx + 1]), System.Text.Encoding.UTF8, "application/json")))
                    {
                        if (response_X.IsSuccessStatusCode)
                        {
                            //저장
                            using (HttpResponseMessage response_Y = await SystemProperties.PROGRAM_HTTP.PostAsync("m6623/mst/i", new StringContent(JsonConvert.SerializeObject(SelectDtlItmList[_FindIdx + 1]), System.Text.Encoding.UTF8, "application/json")))
                            {
                                if (response_Y.IsSuccessStatusCode)
                                {
                                    string result = await response_Y.Content.ReadAsStringAsync();
                                    if (int.TryParse(result, out _Num) == false)
                                    {
                                        //실패
                                        WinUIMessageBox.Show(result, _title, MessageBoxButton.OK, MessageBoxImage.Error);
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }


                //조회
                if (SelectedMenuItem == null)
                {
                    return;
                }

                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6623/dtl", new StringContent(JsonConvert.SerializeObject(SelectedMenuItem), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectDtlItmList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
                    }
                    //
                    if (SelectDtlItmList.Count > 0)
                    {
                        //isM_UPDATE = true;
                        //isM_DELETE = true;

                        SearchDetailJob = SelectDtlItmList.Where<ManVo>(x => x.CMPO_CD.Equals(_CMPO_CD)).FirstOrDefault<ManVo>();
                    }
                    //else
                    //{

                    //isM_UPDATE = false;
                    //isM_DELETE = false;

                    //}
                }

                ////비동기 => 동기화 작업
                ////SearchDetailItem();
                //Task.Run(() => SearchDetailItem()).Wait();
                ////Task.Run(() =>
                ////{
                //if (SelectDtlItmList.Count > 0)
                //{
                //    SearchDetailJob = SelectDtlItmList.Where<ManVo>(x => x.CMPO_CD.Equals(_CMPO_CD)).FirstOrDefault<ManVo>();
                //}
                ////}
                ////).Wait();
            }
            catch
            {
                return;
            }
        }



        [Command]
        public void TreeContact()
        {
            try
            {
                //SelectDtlItmList
                if (SearchDetailJob == null) { return; }
                detailDialog = new M6623DetailDialog();
                detailDialog.Title = _title + " - 제조공정도";
                detailDialog.Owner = Application.Current.MainWindow;
                detailDialog.BorderEffect = BorderEffect.Default;
                detailDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
                detailDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
                bool isDialog = (bool)detailDialog.ShowDialog();
                if (isDialog)
                {
                    Refresh(detailDialog._ASSY_ITM_CD + "_" + detailDialog._BSE_WEIH_VAL);

                    //Refresh(masterDialog.resultVo.ASSY_ITM_CD + "_" + masterDialog.resultVo.BSE_WEIH_VAL);
                    //if (masterDialog.IsEdit == true)
                    //{
                    //  SelectedMenuItem.DELT_FLG = SelectedMenuItem.DELT_FLG;
                    //}
                }

            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                return;
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
            using (HttpResponseMessage responseX = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/L-001"))
            {
                if (responseX.IsSuccessStatusCode)
                {
                    AreaList = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await responseX.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                    if (AreaList.Count > 0)
                    {
                        M_AREA_ITEM = AreaList[2];
                        //벌크
                        //M_AREA_ITEM = AreaList[3];
                        //N1stList();
                        using (HttpResponseMessage responseY = await SystemProperties.PROGRAM_HTTP.PostAsync("s133", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { DELT_FLG = "N", ITM_GRP_CLSS_CD = M_AREA_ITEM.CLSS_CD, CRE_USR_ID = "", CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                        {
                            if (responseY.IsSuccessStatusCode)
                            {
                                N1ST_ITM_GRP_LIST = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await responseY.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                                if (N1ST_ITM_GRP_LIST.Count > 0)
                                {
                                    M_N1ST_ITM_GRP_ITEM = N1ST_ITM_GRP_LIST[0];
                                }
                                else
                                {
                                    M_N1ST_ITM_GRP_ITEM = null;
                                }
                            }
                        }



                        //비동기 
                        Refresh();
                    }
                    
                }
            }
        }


    }
}
