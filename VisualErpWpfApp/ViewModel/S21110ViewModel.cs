using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Code;
using ModelsLibrary.Pur;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using AquilaErpWpfApp3.Util;
using AquilaErpWpfApp3.View.S.Dialog;

namespace AquilaErpWpfApp3.ViewModel
{
    public sealed class S21110ViewModel : ViewModelBase, INotifyPropertyChanged
    {
        //private static PurServiceClient purClient = SystemProperties.PurClient;
        private IList<PurVo> selectedMstList = new List<PurVo>();
        private ObservableCollection<PurVo> selectedDtlList = new ObservableCollection<PurVo>();

        private S21110DetailDialog masterDialog;

        ////Menu Dialog
        //private ICommand _searchDialogCommand;
        private ICommand _newDialogCommand;
        private ICommand _editDialogCommand;
        private ICommand _delDialogCommand;

        //private ICommand _searchDetailDialogCommand;
        //private ICommand _newDetailDialogCommand;
        //private ICommand _editDetailDialogCommand;
        //private ICommand _delDetailDialogCommand;

        //private ICommand reportDialogCommand;

        ////private ICommand _revListDetailDialogCommand;
        ////private ICommand _revNewDetailDialogCommand;

        //private P41MasterDialog masterDialog;
        //private P41DetailDialog detailDialog;
        ////private A21JobItemRevDialog jobItemRevDialog;

        //private P41ReportDialog reportDialog;

        public S21110ViewModel() 
        {

            SYSTEM_CODE_VO();

            //StartDt = System.DateTime.Now;
            //EndDt = System.DateTime.Now;

            //ItemGrpClssCdList = SystemProperties.SYSTEM_CODE_VO("L-001");
            //ItemGrpClssCdList.Insert(0, new CodeDao() { CLSS_CD = "", CLSS_DESC = "" });
            //_ItemGrpClssCdMap = SystemProperties.SYSTEM_CODE_MAP("L-001");
            ////
            ////
            //_ItemCurrCd = SystemProperties.SYSTEM_CODE_VO("S-007");

            //Refresh();
        }

        [Command]
        public async void Refresh()
        {
            try
            {
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("p4401/mst", new StringContent(JsonConvert.SerializeObject(new PurVo() { ITM_GRP_CLSS_CD = (string.IsNullOrEmpty(M_ITM_GRP_CLSS_DESC.CLSS_CD) ? null : M_ITM_GRP_CLSS_DESC.CLSS_CD), CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectMstList = JsonConvert.DeserializeObject<IEnumerable<PurVo>>(await response.Content.ReadAsStringAsync()).Cast<PurVo>().ToList();
                    }
                    //DXSplashScreen.Show<ProgressWindow>();

                    //SelectMstList = purClient.P4401SelectMstList(new PurVo() { ITM_GRP_CLSS_CD = (string.IsNullOrEmpty(M_ITM_GRP_CLSS_DESC) ? null : _ItemGrpClssCdMap[M_ITM_GRP_CLSS_DESC]) });
                    //
                    Title = "  품목 구분 : " + M_ITM_GRP_CLSS_DESC.CLSS_DESC + ",  " + (M_SEARCH_CHECKD == true ? "구단가 포함" : "");


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
                        //
                        //isD_UPDATE = false;
                        //isD_DELETE = false;
                    }

                    //DXSplashScreen.Close();
                }
            }
            catch (System.Exception eLog)
            {
                //DXSplashScreen.Close();
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]제품 단가 관리", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }

        //#region Functon <Master List>
        //private Dictionary<string, string> _ItemGrpClssCdMap = new Dictionary<string, string>();
        private IList<SystemCodeVo> _ItemGrpClssCd = new List<SystemCodeVo>();
        public IList<SystemCodeVo> ItemGrpClssCdList
        {
            get { return _ItemGrpClssCd; }
            set { SetProperty(ref _ItemGrpClssCd, value, () => ItemGrpClssCdList); }
        }

        private IList<SystemCodeVo> _ItemCurrCd = new List<SystemCodeVo>();
        public IList<SystemCodeVo> ItemCurrCdList
        {
            get { return _ItemCurrCd; }
            set { SetProperty(ref _ItemCurrCd, value, () => ItemCurrCdList); }
        }


        private SystemCodeVo _M_ITM_GRP_CLSS_DESC;
        public SystemCodeVo M_ITM_GRP_CLSS_DESC
        {
            get { return _M_ITM_GRP_CLSS_DESC; }
            set { SetProperty(ref _M_ITM_GRP_CLSS_DESC, value, () => M_ITM_GRP_CLSS_DESC); }
        }


        private string _M_SEARCH_TEXT = string.Empty;
        public string M_SEARCH_TEXT
        {
            get { return _M_SEARCH_TEXT; }
            set { SetProperty(ref _M_SEARCH_TEXT, value, () => M_SEARCH_TEXT); }
        }

        private bool? _M_SEARCH_CHECKD = false;
        public bool? M_SEARCH_CHECKD
        {
            get { return _M_SEARCH_CHECKD; }
            set { SetProperty(ref _M_SEARCH_CHECKD, value, () => M_SEARCH_CHECKD); }
        }


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
                    SetProperty(ref _selectedMstItem, value, () => SelectedMstItem, SelectMstDetail);
                }
            }
        }

        private bool? _isM_SAVE = false;
        public bool? isM_SAVE
        {
            get { return _isM_SAVE; }
            set { SetProperty(ref _isM_SAVE, value, () => isM_SAVE); }
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

        private async void SelectMstDetail()
        {
             try
             {
                 //DXSplashScreen.Show<ProgressWindow>();

                if (this._selectedMstItem == null)
                {
                    return;
                }

                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("p4401/dtl", new StringContent(JsonConvert.SerializeObject(new PurVo() { ITM_CD = this._selectedMstItem.ITM_CD, RN = (M_SEARCH_CHECKD == true ? 1 : 2), CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectDtlList = new ObservableCollection<PurVo>(JsonConvert.DeserializeObject<IEnumerable<PurVo>>(await response.Content.ReadAsStringAsync()).Cast<PurVo>().ToList());
                    }
                    //SelectDtlList = new ObservableCollection<PurVo>(purClient.P4401SelectDtlList(new PurVo() { ITM_CD = this._selectedMstItem.ITM_CD, RN = (M_SEARCH_CHECKD == true ? 1 : 2) }));
                    //
                    if (SelectDtlList.Count >= 1)
                    {
                        isM_UPDATE = true;
                        isM_DELETE = true;

                        SearchDetail = SelectDtlList[0];
                    }
                    else
                    {
                        isM_UPDATE = false;
                        isM_DELETE = false;
                    }
                }

                //DXSplashScreen.Close();
             }
             catch (System.Exception eLog)
             {
                 //DXSplashScreen.Close();
                 //
                 WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]제품 단가 관리", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                 return;
             }
        }
        //#endregion


        //#region Functon <Detail List>
        public ObservableCollection<PurVo> SelectDtlList
        {
            get { return selectedDtlList; }
            set { SetProperty(ref selectedDtlList, value, () => SelectDtlList); }
        }

        PurVo _searchDetail;
        public PurVo SearchDetail
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
        //public ICommand SearchDialogCommand
        //{
        //    get
        //    {
        //        if (_searchDialogCommand == null)
        //            _searchDialogCommand = new DelegateCommand(Refresh);
        //        return _searchDialogCommand;
        //    }
        //}

        public ICommand NewDialogCommand
        {
            get
            {
                if (_newDialogCommand == null)
                    _newDialogCommand = new DelegateCommand(NewDtlContact);
                return _newDialogCommand;
            }
        }

        public void NewDtlContact()
        {
            if (SelectedMstItem == null)
            {
                return;
            }

            masterDialog = new S21110DetailDialog(new PurVo() { ITM_CD = SelectedMstItem.ITM_CD, CNG_APLY_DT = System.DateTime.Now.ToString("yyyy-MM-dd"), CO_UT_PRC = 0, CURR_CD = "KRW", MN_CO_FLG = 0, CRNT_PRC_FLG = 1, CO_ITM_CD = SelectedMstItem.ITM_CD, CO_ITM_NM = SelectedMstItem.ITM_NM });
            masterDialog.Title = "제품 단가 관리 - 추가[" + SelectedMstItem.ITM_CD + "]";
            masterDialog.Owner = Application.Current.MainWindow;
            masterDialog.BorderEffect = BorderEffect.Default;
            masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            bool isDialog = (bool)masterDialog.ShowDialog();
            if (isDialog)
            {
                SelectMstDetail();
                //if (masterDialog.IsEdit == false)
                //{
                //Refresh();
                //}
            }
        }

        public ICommand EditDtlDialogCommand
        {
            get
            {
                if (_editDialogCommand == null)
                    _editDialogCommand = new DelegateCommand(EditDtlContact);
                return _editDialogCommand;
            }
        }

        public void EditDtlContact()
        {
            if (SelectedMstItem == null)
            {
                return;
            }

            if (SearchDetail == null)
            {
                return;
            }
            //else if (SelectDtlList == null)
            //{
            //    return;
            //}
            //else if (SelectDtlList.Count <= 0)
            //{
            //    return;
            //}


            masterDialog = new S21110DetailDialog(SearchDetail);
            masterDialog.Title = "제품 단가 관리 - 수정[" + SelectedMstItem.ITM_CD + "]";
            masterDialog.Owner = Application.Current.MainWindow;
            masterDialog.BorderEffect = BorderEffect.Default;
            masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            bool isDialog = (bool)masterDialog.ShowDialog();
            if (isDialog)
            {
                SelectMstDetail();
                //if (masterDialog.IsEdit == false)
                //{
                //    Refresh();
                //}
            }
        }



        public ICommand DelDtlDialogCommand
        {
            get
            {
                if (_delDialogCommand == null)
                    _delDialogCommand = new DelegateCommand(DelDtlContact);
                return _delDialogCommand;
            }
        }

        public async void DelDtlContact()
        {
            if (SelectedMstItem == null)
            {
                return;
            }

            PurVo delDao = SearchDetail;
            if (delDao != null)
            {
                    MessageBoxResult result = WinUIMessageBox.Show("[" + delDao.CO_NO + "/" + delDao.CO_NM + "]" + " 정말로 삭제 하시겠습니까?", "제품 단가 관리", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("p4401/dtl/d", new StringContent(JsonConvert.SerializeObject(SearchDetail), System.Text.Encoding.UTF8, "application/json")))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            int _Num = 0;
                            string resultMsg = await response.Content.ReadAsStringAsync();
                            if (int.TryParse(resultMsg, out _Num) == false)
                            {
                                //실패
                                WinUIMessageBox.Show(resultMsg, "제품 단가 관리", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                            Refresh();

                            //성공
                            WinUIMessageBox.Show("삭제가 완료되었습니다.", "제품 단가 관리", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                        }
                    }

                    //        purClient.P4401DeleteDtl(delDao);
                    //        SelectMstDetail();
                    //WinUIMessageBox.Show("삭제가 완료되었습니다.", "[삭제]매입 단가 관리", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                    }
            }
        }

        public async void SYSTEM_CODE_VO()
        {
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "L-001"))
            {
                if (response.IsSuccessStatusCode)
                {
                    ItemGrpClssCdList = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                    if (ItemGrpClssCdList.Count > 0)
                    {
                        //ItemGrpClssCdList.Insert(0, new SystemCodeVo() { CLSS_CD = "", CLSS_DESC = "전체" });
                        M_ITM_GRP_CLSS_DESC = ItemGrpClssCdList[4];
                    }
                }
            }

            //
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "S-007"))
            {
                if (response.IsSuccessStatusCode)
                {
                    _ItemCurrCd = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                }
            }

            Refresh();
        }

    }
}
