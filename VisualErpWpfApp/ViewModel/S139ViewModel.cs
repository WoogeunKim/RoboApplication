using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Auth;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Windows;
using System.Linq;
using AquilaErpWpfApp3.Util;
using AquilaErpWpfApp3.View.S.Dialog;
using DevExpress.Xpf.Core;
using System.Windows.Media;

namespace AquilaErpWpfApp3.ViewModel
{
    public sealed class S139ViewModel  : ViewModelBase, INotifyPropertyChanged {

        private string _title = "프로그램 메뉴 관리";

        //private static AuthorServiceClient authClient = SystemProperties.AuthClient;
        private IList<ProgramVo> selectedMenuViewList;
        //Menu Dialog
        //private ICommand masterSearchDialogCommand;
        //private ICommand masterNewDialogCommand;
        //private ICommand masterEditDialogCommand;
        //private ICommand masterDelDialogCommand;

        private S139MenuDialog masterDialog;

        public S139ViewModel() 
        {
            Refresh();
        }

        [Command]
        public async void Refresh(string _MDL_ID = null)
        {
            try
            {
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s139/" + Properties.Settings.Default.SettingChnl))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectedMenuViewList = JsonConvert.DeserializeObject<IEnumerable<ProgramVo>>(await response.Content.ReadAsStringAsync()).Cast<ProgramVo>().ToList();
                    }

                    if (SelectedMenuViewList.Count > 0)
                    {
                        isM_UPDATE = true;
                        isM_DELETE = true;
                        if (string.IsNullOrEmpty(_MDL_ID))
                        {
                            SelectedMenuItem = SelectedMenuViewList[0];
                        }
                        else
                        {
                            SelectedMenuItem = SelectedMenuViewList.Where(x => x.MDL_ID.Equals(_MDL_ID)).LastOrDefault<ProgramVo>();
                        }
                    }
                    else
                    {
                        isM_UPDATE = false;
                        isM_DELETE = false;
                    }
                }
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, _title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }

        #region Functon (Menu Add, Edit, Del)
        public IList<ProgramVo> SelectedMenuViewList
        {
            get { return selectedMenuViewList; }
            private set { SetProperty(ref selectedMenuViewList, value, () => SelectedMenuViewList); }
        }

        ProgramVo _selectMenuItem;
        public ProgramVo SelectedMenuItem
        {
            get
            {
                return _selectMenuItem;
            }
            set
            {
                if (value != null)
                {
                    SetProperty(ref _selectMenuItem, value, () => SelectedMenuItem);
                }
            }
        }

        //public ICommand MasterSearchDialogCommand
        //{
        //    get
        //    {
        //        if (masterSearchDialogCommand == null)
        //            masterSearchDialogCommand = new DelegateCommand(SearchMenuContact);
        //        return masterSearchDialogCommand;
        //    }
        //}

        //public void SearchMenuContact()
        //{
        //    SelectedMenuViewList = authClient.SelectProgramMenuTotalList(new ProgramVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD });
        //    //SystemProperties.MENU_IMG_SET(SelectedMenuViewList);
        //    if (SelectedMenuViewList.Count > 0)
        //    {
        //        isM_UPDATE = true;
        //        isM_DELETE = true;

        //        SelectedMenuItem = SelectedMenuViewList[0];
        //    }
        //    else
        //    {

        //        isM_UPDATE = false;
        //        isM_DELETE = false;

        //    }
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
            if (this._selectMenuItem == null) { this._selectMenuItem = new ProgramVo() { PRNT_MDL_ID = "", PRNT_MDL_NM = "", SYS_AREA_CD = "", SYS_AREA_NM = "" }; };
            ShowMasterDialog(new ProgramVo() { PRNT_MDL_ID = this._selectMenuItem.PRNT_MDL_ID, PRNT_MDL_NM = this._selectMenuItem.PRNT_MDL_NM, SYS_AREA_CD = this._selectMenuItem.SYS_AREA_CD, SYS_AREA_NM = this._selectMenuItem.SYS_AREA_NM, CHNL_CD = SystemProperties.USER_VO.CHNL_CD });
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
            if (this._selectMenuItem == null) { return; }
            ProgramVo editDao = this._selectMenuItem;
            if (editDao != null)
            {
                ShowMasterDialog(editDao);
            }
        }

        public void ShowMasterDialog(ProgramVo dao)
        {
            masterDialog = new S139MenuDialog(dao);
            masterDialog.Title = "프로그램 메뉴 관리";
            masterDialog.Owner = Application.Current.MainWindow;
            masterDialog.BorderEffect = BorderEffect.Default;
            masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            bool isDialog = (bool)masterDialog.ShowDialog();
            if (isDialog)
            {
                Refresh(masterDialog.resultDao.MDL_ID);

                //for (int x = 0; x < SelectedMenuViewList.Count; x++)
                //{
                //    if ((SelectedMenuViewList[x].CHNL_CD + "_" + SelectedMenuViewList[x].MDL_ID).Equals(masterDialog.resultDao.CHNL_CD + "_" + masterDialog.resultDao.MDL_ID))
                //    {
                //        SelectedMenuItem = SelectedMenuViewList[x];
                //        break;
                //    }
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
        public void DelMasterContact()
        {
            ProgramVo delDao = this._selectMenuItem;
            if (delDao != null)
            {
                //        //
                //        //IList<ItemGroupCodeVo> checkDel = itemClient.SelectCodeItemGroupList(this._selectMasterItem);
                //        //if (checkDel.Count != 0)
                //        //{
                //        //    WinUIMessageBox.Show("중분류 코드 부분을 먼저 삭제 해주세요.", "[삭제 - 대분류]장비 분류 코드", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                //        //    return;
                //        //}

                MessageBoxResult result = WinUIMessageBox.Show("[" + delDao.MDL_ID + "]" + delDao.MDL_NM + " 정말로 삭제 하시겠습니까?", _title, MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    //authClient.DeleteGroupProgram(delDao);
                    //authClient.DeleteUserProgram(delDao);
                    //authClient.DeleteProgram(delDao);
                    WinUIMessageBox.Show("삭제가 완료되었습니다.", _title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                    this._selectMenuItem = null;
                    Refresh();
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
    }
}
