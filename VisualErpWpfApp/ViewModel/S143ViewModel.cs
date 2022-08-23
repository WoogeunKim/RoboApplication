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
    public sealed class S143ViewModel  : ViewModelBase, INotifyPropertyChanged {

        private string title = "거래처 관리";

        //private static CodeServiceClient customerClient = SystemProperties.CodeClient;
        private IList<SystemCodeVo> selectedMenuViewList;
        //Menu Dialog
        //private ICommand searchDialogCommand;
        //private ICommand newDialogCommand;
        //private ICommand editDialogCommand;
        //private ICommand delDialogCommand;

        //private ICommand classDialogCommand;

        private S143CustomerDialog masterDialog;
        private S143ClassDialog classDialog;


        private S143FaxDialog faxDialog;

        private bool? seek_ap = true;
        private bool? seek_ar = true;
        private bool? seek_or = true;
        private bool? seek_su = true;

        public S143ViewModel() 
        {
            Refresh();
        }

      [Command]
      public async void Refresh(string _CO_NO = null)
        {
            try
            {
                //SearchMenuContact();
                string _ap = (seek_ap == true ? "AP" : "X");
                string _ar = (seek_ar == true ? "AR" : "X");
                string _or = (seek_or == true ? "OR" : "X");
                string _su = (seek_su == true ? "SU" : "X");

                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s143", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { SEEK = "", SEEK_AP = _ap, SEEK_AR = _ar, SEEK_OR = _or, SEEK_SU = _su, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectedMenuViewList = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                    }

                    //SelectedMenuViewList = customerClient.SelectCodeList(new SystemCodeVo() { SEEK = "", SEEK_AP = _ap, SEEK_AR = _ar, SEEK_OR = _or, SEEK_SU = _su , CHNL_CD = SystemProperties.USER_VO.CHNL_CD});
                    if (SelectedMenuViewList.Count > 0)
                    {
                        isM_UPDATE = true;
                        isM_DELETE = true;

                        if (string.IsNullOrEmpty(_CO_NO))
                        {
                            SelectedMenuItem = SelectedMenuViewList[0];
                        }
                        else
                        {
                            SelectedMenuItem = SelectedMenuViewList.Where(x => x.CO_NO.Equals(_CO_NO)).LastOrDefault<SystemCodeVo>();
                        }
                    }
                    else
                    {
                        isM_UPDATE = false;
                        isM_DELETE = false;

                        SelectedMenuItem = null;
                    }
                }
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }


        [Command]
        public void Fax()
        {
            if (SearchItem == null) { return; }
            ShowFaxDialog(SearchItem);

        }
        public void ShowFaxDialog(SystemCodeVo dao)
        {
            faxDialog = new S143FaxDialog(dao);
            faxDialog.Title = "팩스";
            faxDialog.Owner = Application.Current.MainWindow;
            faxDialog.BorderEffect = BorderEffect.Default;
            faxDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            faxDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            faxDialog.ShowDialog();

        }

        #region Functon (Menu Add, Edit, Del)
        public IList<SystemCodeVo> SelectedMenuViewList
        {
            get { return selectedMenuViewList; }
            private set { SetProperty(ref selectedMenuViewList, value, () => SelectedMenuViewList); }
        }

        SystemCodeVo _selectMenuItem;
        public SystemCodeVo SelectedMenuItem
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
            if (this._selectMenuItem == null)
            {
                return;
            }
            // SearchItem = customerClient.SelectCustomerDetailCode(this._selectMenuItem);
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s143/dtl", new StringContent(JsonConvert.SerializeObject(_selectMenuItem), System.Text.Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.SearchItem = JsonConvert.DeserializeObject<SystemCodeVo>(await response.Content.ReadAsStringAsync());
                }
            }
        }

        SystemCodeVo _searchItem;
        public SystemCodeVo SearchItem
        {
            get
            {
                return _searchItem;
            }
            set
            {
                if (value != null)
                {
                    SetProperty(ref _searchItem, value, () => SearchItem);
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
        //    //WinUIMessageBox.Show("매입처 : " + seek_M + ", 매출처 : " + seek_C + ", 구입처 : " + seek_W, "검색 조건", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);

        //    string _ap = (seek_ap == true ? "AP" : "X");
        //    string _ar = (seek_ar == true ? "AR" : "X");
        //    string _or = (seek_or == true ? "OR" : "X");
        //    string _su = (seek_su == true ? "SU" : "X");

        //    //SelectedMenuViewList = customerClient.SelectCodeList(new SystemCodeVo() { SEEK = "", SEEK_AP = _ap, SEEK_AR = _ar, SEEK_OR = _or, SEEK_SU = _su , CHNL_CD = SystemProperties.USER_VO.CHNL_CD});

        //    if (SelectedMenuViewList.Count > 0)
        //    {
        //        SelectedMenuItem = SelectedMenuViewList[0];
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
        public void NewContact()
        {
            //if (this._selectMenuItem == null) { return; }
            ShowMasterDialog(new SystemCodeVo());
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
        public void EditContact()
        {
            if (SearchItem == null) { return; }
            ShowMasterDialog(SearchItem);
            
        }

        public void ShowMasterDialog(SystemCodeVo dao)
        {
            masterDialog = new S143CustomerDialog(dao);
            masterDialog.Title = "거래처 관리";
            masterDialog.Owner = Application.Current.MainWindow;
            masterDialog.BorderEffect = BorderEffect.Default;
            masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            bool isDialog = (bool)masterDialog.ShowDialog();
            if (isDialog)
            {
                Refresh(masterDialog.resultDao.CO_NO);
                //if (masterDialog.IsEdit == false)
                //{
                //    SearchMenuContact();

                //    for (int x = 0; x < SelectedMenuViewList.Count; x++)
                //    {
                //        if (masterDialog.ResultDao.CO_NO.Equals(SelectedMenuViewList[x].CO_NO))
                //        {
                //            SelectedMenuItem = SelectedMenuViewList[x];
                //            return;
                //        }
                //    }
                //}
                //else
                //{
                //    SelectedMenuItem.DELT_FLG = dao.DELT_FLG;
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
        public async void DelContact()
        {
            SystemCodeVo delDao = SelectedMenuItem;
            if (delDao != null)
            {
                MessageBoxResult result = WinUIMessageBox.Show("[" + delDao.CO_NO + "]" + delDao.CO_NM + " 정말로 삭제 하시겠습니까?", title, MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    int _Num = 0;
                    string resultMsg = "";
                    //customerClient.DeleteCustomerClassCode(delDao);
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s143/class/d", new StringContent(JsonConvert.SerializeObject(delDao), System.Text.Encoding.UTF8, "application/json")))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            resultMsg = await response.Content.ReadAsStringAsync();
                            if (int.TryParse(resultMsg, out _Num) == false)
                            {
                                //실패
                                WinUIMessageBox.Show(resultMsg, this.title, MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                            //Refresh();
                            //성공
                            //WinUIMessageBox.Show("삭제가 완료되었습니다.", this.title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                        }
                    }

                    //customerClient.DeleteCustomerDetailCode(delDao);
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s143/d", new StringContent(JsonConvert.SerializeObject(delDao), System.Text.Encoding.UTF8, "application/json")))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            resultMsg = await response.Content.ReadAsStringAsync();
                            if (int.TryParse(resultMsg, out _Num) == false)
                            {
                                //실패
                                WinUIMessageBox.Show(resultMsg, this.title, MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                            //Refresh();
                            //성공
                            //WinUIMessageBox.Show("삭제가 완료되었습니다.", this.title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                        }
                    }

                    Refresh();
                    WinUIMessageBox.Show("삭제가 완료되었습니다.", title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                    //this._selectMenuItem = null;
                   // SearchMenuContact();
                }
            }
        }
        #endregion

        //public ICommand ClassDialogCommand
        //{
        //    get
        //    {
        //        if (classDialogCommand == null)
        //            classDialogCommand = new DelegateCommand(EditClassContact);
        //        return classDialogCommand;
        //    }
        //}

        [Command]
        public void EditClassContact()
        {
            if (SearchItem == null) { return; }
            ShowClassDialog(SearchItem);

        }

        public void ShowClassDialog(SystemCodeVo dao)
        {
            classDialog = new S143ClassDialog(dao);
            classDialog.Title = "거래처 등급 관리 - " + dao.CO_NM;
            classDialog.Owner = Application.Current.MainWindow;
            classDialog.BorderEffect = BorderEffect.Default;
            classDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            classDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            bool isDialog = (bool)classDialog.ShowDialog();
            //if (isDialog)
            //{
            //    if (masterDialog.IsEdit == false)
            //    {
            //        SearchMenuContact();
            //    }
            //    else
            //    {
            //        SelectedMenuItem.DELT_FLG = dao.DELT_FLG;
            //    }
            //}
        }




        public bool? SEEK_AP
        {
            get
            {
                return seek_ap;
            }
            set
            {
                if (value != null)
                {
                    SetProperty(ref seek_ap, value, () => SEEK_AP);
                }
            }
        }

        public bool? SEEK_AR
        {
            get
            {
                return seek_ar;
            }
            set
            {
                if (value != null)
                {
                    SetProperty(ref seek_ar, value, () => SEEK_AR);
                }
            }
        }

        public bool? SEEK_OR
        {
            get
            {
                return seek_or;
            }
            set
            {
                if (value != null)
                {
                    SetProperty(ref seek_or, value, () => SEEK_OR);
                }
            }
        }

        public bool? SEEK_SU
        {
            get
            {
                return seek_su;
            }
            set
            {
                if (value != null)
                {
                    SetProperty(ref seek_su, value, () => SEEK_SU);
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
    }
}
