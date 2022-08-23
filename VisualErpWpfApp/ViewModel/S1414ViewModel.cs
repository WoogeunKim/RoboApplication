using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Code;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Media;
using AquilaErpWpfApp3.Util;
using AquilaErpWpfApp3.View.S.Dialog;

namespace AquilaErpWpfApp3.ViewModel
{
    public sealed class S1414ViewModel : ViewModelBase, INotifyPropertyChanged {

        //private static CodeServiceClient itemClient = SystemProperties.CodeClient;

        private string title = "품목 그룹 관리";

        private IList<SystemCodeVo> selectedMasterViewList;
        private IList<SystemCodeVo> selectedDetailViewList;
       
        //Master Dialog
        //private ICommand masterSearchDialogCommand;
        //private ICommand masterNewDialogCommand;
        //private ICommand masterEditDialogCommand;
        //private ICommand masterDelDialogCommand;
        //
        private S1414MasterDialog masterDialog;

        //Detail Dialog
        //private ICommand detailSearchDialogCommand;
        //private ICommand detailNewDialogCommand;
        //private ICommand detailEditDialogCommand;
        //private ICommand detailDelDialogCommand;
        ////
        private S1414DetailDialog detailDialog;

        public S1414ViewModel() 
        {
            Refresh();
        }

        [Command]
        public async void Refresh(string _ITM_GRP_CD = null)
        {
            try
            {
                SelectedDetailItem = null;
                SelectedDetailViewList = null;

                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s133", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { CRE_USR_ID = "", CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectedMasterViewList = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                    }
                    //SelectedMasterViewList = itemClient.SelectCodeItemGroupList(new SystemCodeVo() { CRE_USR_ID = "" , CHNL_CD = SystemProperties.USER_VO.CHNL_CD});
                    if (SelectedMasterViewList.Count > 0)
                    {
                        isM_UPDATE = true;
                        isM_DELETE = true;

                        if (string.IsNullOrEmpty(_ITM_GRP_CD))
                        {
                            SelectedMasterItem = SelectedMasterViewList[0];
                        }
                        else
                        {
                            SelectedMasterItem = SelectedMasterViewList.Where(x => x.ITM_GRP_CD.Equals(_ITM_GRP_CD)).LastOrDefault<SystemCodeVo>();
                        }
                    }
                    else
                    {
                        SelectedDetailItem = null;
                        SelectedDetailViewList = null;

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

        #region Functon (Master Add, Edit, Del)
        public IList<SystemCodeVo> SelectedMasterViewList
        {
            get { return selectedMasterViewList; }
            private set { SetProperty(ref selectedMasterViewList, value, () => SelectedMasterViewList); }
        }

        SystemCodeVo _selectMasterItem;
        public SystemCodeVo SelectedMasterItem
        {
            get
            {
                return _selectMasterItem;
            }
            set
            {
                if (value != null)
                {
                    SetProperty(ref _selectMasterItem, value, () => SelectedMasterItem, SearchDetailContact);
                }
            }
        }

        async void OnSelectedMasterItemChanged(string _N1ST_ITM_GRP_CD = null, string _ITM_GRP_CLSS_CD = null)
        {
            if (this._selectMasterItem == null) { return; }
            //
            //this._selectMasterItem.ITM_GRP_CLSS_CD = null;
            //this._selectMasterItem.PRNT_ITM_GRP_CD = "";
            //this._selectMasterItem.DELT_FLG = "N";
            //this._selectMasterItem.N1ST_ITM_GRP_CD = this._selectMasterItem.ITM_GRP_CD;
            //SelectedDetailViewList = itemClient.SelectCodeItemGroupList(this._selectMasterItem);

            SystemCodeVo _param = new SystemCodeVo();
            _param.PRNT_ITM_GRP_CD = "X";
            //_param.DELT_FLG = "N";
            _param.N1ST_ITM_GRP_CD = this._selectMasterItem.ITM_GRP_CD;
            _param.ITM_GRP_CLSS_CD = this._selectMasterItem.ITM_GRP_CLSS_CD;
            _param.CRE_USR_ID = "";
            _param.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;

            //SelectedDetailViewList = itemClient.SelectCodeItemGroupList(_param);

            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s133", new StringContent(JsonConvert.SerializeObject(_param), System.Text.Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.SelectedDetailViewList = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                }
            }


            if (SelectedDetailViewList.Count > 0)
            {
                isD_UPDATE = true;
                isD_DELETE = true;


                SelectedDetailItem = SelectedDetailViewList[0];
            }
            else
            {
                isD_UPDATE = false;
                isD_DELETE = false;
            }
        }

        //public ICommand MasterSearchDialogCommand
        //{
        //    get
        //    {
        //        if (masterSearchDialogCommand == null)
        //            masterSearchDialogCommand = new DelegateCommand(SearchMasterContact);
        //        return masterSearchDialogCommand;
        //    }
        //}

        //public void SearchMasterContact()
        //{
        //    //electedMasterViewList = itemClient.SelectCodeItemGroupList(new SystemCodeVo() { CRE_USR_ID = "", CHNL_CD = SystemProperties.USER_VO.CHNL_CD });
        //    OnSelectedMasterItemChanged();
        //}

        //public ICommand MasterNewDialogCommand
        //{
        //    get
        //    {
        //        if (masterNewDialogCommand == null)
        //            masterNewDialogCommand = new DelegateCommand(NewMasterContact);
        //        return masterNewDialogCommand;
        //    }
        //}

        [Command]
        public void NewMasterContact()
        {
            ShowMasterDialog(new SystemCodeVo());
        }

        //public ICommand MasterEditDialogCommand
        //{
        //    get
        //    {
        //        if (masterEditDialogCommand == null)
        //            masterEditDialogCommand = new DelegateCommand(EditMasterContact);
        //        return masterEditDialogCommand;
        //    }
        //}

        [Command]
        public void EditMasterContact()
        {
            if (this._selectMasterItem == null) { return; }
            SystemCodeVo editDao = this._selectMasterItem;
            if (editDao != null)
            {
                ShowMasterDialog(editDao);
            }
        }

        public void ShowMasterDialog(SystemCodeVo dao)
        {
            masterDialog = new S1414MasterDialog(dao);
            masterDialog.Title = "대분류 코드";
            masterDialog.Owner = Application.Current.MainWindow;
            masterDialog.BorderEffect = BorderEffect.Default;
            masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            bool isDialog = (bool)masterDialog.ShowDialog();
            if (isDialog)
            {
                Refresh(masterDialog.resultDomain.ITM_GRP_CD);
                //if (masterDialog.IsEdit == false)
                //{
                //    SelectedMasterViewList = itemClient.SelectCodeItemGroupList(new SystemCodeVo() { CRE_USR_ID = "" });

                //    for (int x = 0; x < SelectedMasterViewList.Count; x++)
                //    {
                //        if ((masterDialog.resultDomain.CHNL_CD + "_" + masterDialog.resultDomain.PRNT_ITM_GRP_CD).Equals(SelectedMasterViewList[x].CHNL_CD + "_" + SelectedMasterViewList[x].PRNT_ITM_GRP_CD))
                //        {
                //            SelectedMasterItem = SelectedMasterViewList[x];
                //            break;
                //        }
                //    }

                //    //OnSelectedMasterItemChanged();
                //}
            }
        }

        //public ICommand MasterDelDialogCommand
        //{
        //    get
        //    {
        //        if (masterDelDialogCommand == null)
        //            masterDelDialogCommand = new DelegateCommand(DelMasterContact);
        //        return masterDelDialogCommand;
        //    }
        //}


        [Command]
        public async void DelMasterContact()
        {
            SystemCodeVo delDao = this._selectMasterItem;
            if (delDao != null)
            {
                //
                //this._selectMasterItem.ITM_GRP_CLSS_CD = null;
                //this._selectMasterItem.PRNT_ITM_GRP_CD = "";
                //this._selectMasterItem.DELT_FLG = "N";
                //this._selectMasterItem.N1ST_ITM_GRP_CD = this._selectMasterItem.ITM_GRP_CD;
                //IList<SystemCodeVo> checkDel = itemClient.SelectCodeItemGroupList(new SystemCodeVo() { CRE_USR_ID = "", ITM_GRP_CLSS_CD = null, PRNT_ITM_GRP_CD = "X", N1ST_ITM_GRP_CD = this._selectMasterItem.ITM_GRP_CD, CHNL_CD = SystemProperties.USER_VO.CHNL_CD });
                OnSelectedMasterItemChanged();

                if (this.SelectedDetailViewList.Count != 0)
                {
                    WinUIMessageBox.Show("중분류 코드 부분을 먼저 삭제 해주세요.", title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                    return;
                }

                MessageBoxResult result = WinUIMessageBox.Show(delDao.ITM_GRP_CD + " 정말로 삭제 하시겠습니까?", title, MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    delDao.CRE_USR_ID = SystemProperties.USER;
                    delDao.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s133/d", new StringContent(JsonConvert.SerializeObject(delDao), System.Text.Encoding.UTF8, "application/json")))
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
                            Refresh();

                            //성공
                            WinUIMessageBox.Show("삭제가 완료되었습니다.", title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                        }
                    }
                    //itemClient.DeleteItemGroupCode(delDao);
                    //this._selectMasterItem = null;
                    //Refresh();
                    //SelectedMasterViewList = itemClient.SelectCodeItemGroupList(new SystemCodeVo() { CRE_USR_ID = "", CHNL_CD = SystemProperties.USER_VO.CHNL_CD });
                }
            }
        }
        #endregion


        #region Functon (Detail Add, Edit, Del)
        public IList<SystemCodeVo> SelectedDetailViewList
        {
            get { return selectedDetailViewList; }
            private set { SetProperty(ref selectedDetailViewList, value, () => SelectedDetailViewList); }
        }

        SystemCodeVo _selectedDetailItem;
        public SystemCodeVo SelectedDetailItem
        {
            get
            {
                return _selectedDetailItem;
            }
            set
            {
                if (value != null)
                {
                    SetProperty(ref _selectedDetailItem, value, () => SelectedDetailItem);
                }
            }
        }



        //public ICommand DetailSearchDialogCommand
        //{
        //    get
        //    {
        //        if (detailSearchDialogCommand == null)
        //            detailSearchDialogCommand = new DelegateCommand(SearchDetailContact);
        //        return detailSearchDialogCommand;
        //    }
        //}

        [Command]
        public void SearchDetailContact()
        {
            OnSelectedMasterItemChanged();
        }

        //public ICommand DetailNewDialogCommand
        //{
        //    get
        //    {
        //        if (detailNewDialogCommand == null)
        //            detailNewDialogCommand = new DelegateCommand(NewDetailContact);
        //        return detailNewDialogCommand;
        //    }
        //}
        [Command]
        public void NewDetailContact()
        {
            if (this._selectMasterItem == null) { return; }
            ShowDetailDialog(new SystemCodeVo() { PRNT_ITM_GRP_CD = _selectMasterItem.ITM_GRP_CD });
        }

        //public ICommand DetailEditDialogCommand
        //{
        //    get
        //    {
        //        if (detailEditDialogCommand == null)
        //            detailEditDialogCommand = new DelegateCommand(EditDetailContact, (SelectedDetailItem == null ? false : true));
        //        return detailEditDialogCommand;
        //    }
        //}

        [Command]
        public void EditDetailContact()
        {
            SystemCodeVo editDao = SelectedDetailItem;
            if (editDao != null)
            {
                ShowDetailDialog(editDao);
            }
        }

        public void ShowDetailDialog(SystemCodeVo dao)
        {
            dao.ITM_GRP_CLSS_CD = _selectMasterItem.ITM_GRP_CLSS_CD;
            dao.ITM_GRP_CLSS_NM = _selectMasterItem.ITM_GRP_CLSS_NM;
            //
            detailDialog = new S1414DetailDialog(dao);
            detailDialog.Title = "중분류 코드(분류 코드 : " + _selectMasterItem.ITM_GRP_CD + ")";
            detailDialog.Owner = Application.Current.MainWindow;
            detailDialog.BorderEffect = BorderEffect.Default;
            detailDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            detailDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            bool isDialog = (bool)detailDialog.ShowDialog();
            if (isDialog)
            {
                OnSelectedMasterItemChanged(detailDialog.resultDomain.N1ST_ITM_GRP_CD, detailDialog.resultDomain.ITM_GRP_CLSS_CD);

                //if (detailDialog.IsEdit == false)
                //{
                //    OnSelectedMasterItemChanged();

                //    for (int x = 0; x < SelectedDetailViewList.Count; x++)
                //    {
                //        if ((detailDialog.resultDomain.CHNL_CD + "_" + detailDialog.resultDomain.N1ST_ITM_GRP_CD + "_" + detailDialog.resultDomain.ITM_GRP_CLSS_CD).Equals(SelectedDetailViewList[x].CHNL_CD + "_" + SelectedDetailViewList[x].N1ST_ITM_GRP_CD + "_" + SelectedDetailViewList[x].ITM_GRP_CLSS_CD))
                //        {
                //            SelectedDetailItem = SelectedDetailViewList[x];
                //            break;
                //        }
                //    }
                //}
            }
        }

        //public ICommand DetailDelDialogCommand
        //{
        //    get
        //    {
        //        if (detailDelDialogCommand == null)
        //            detailDelDialogCommand = new DelegateCommand(DelDetailContact);
        //        return detailDelDialogCommand;
        //    }
        //}

        [Command]
        public async void DelDetailContact()
        {
            SystemCodeVo delDao = this.SelectedDetailItem;
            if (delDao != null)
            {

                MessageBoxResult result = WinUIMessageBox.Show(delDao.ITM_GRP_CD + " 정말로 삭제 하시겠습니까?", title, MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    delDao.CRE_USR_ID = SystemProperties.USER;
                    delDao.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s133/d", new StringContent(JsonConvert.SerializeObject(delDao), System.Text.Encoding.UTF8, "application/json")))
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
                            OnSelectedMasterItemChanged();

                            //성공
                            WinUIMessageBox.Show("삭제가 완료되었습니다.", title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                        }
                    }
                }
            }
        }
        #endregion


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

        private string _M_SEARCH_TEXT = string.Empty;
        public string M_SEARCH_TEXT
        {
            get { return _M_SEARCH_TEXT; }
            set { SetProperty(ref _M_SEARCH_TEXT, value, () => M_SEARCH_TEXT); }
        }
        private string _D_SEARCH_TEXT = string.Empty;
        public string D_SEARCH_TEXT
        {
            get { return _D_SEARCH_TEXT; }
            set { SetProperty(ref _D_SEARCH_TEXT, value, () => D_SEARCH_TEXT); }
        }
    }
}
