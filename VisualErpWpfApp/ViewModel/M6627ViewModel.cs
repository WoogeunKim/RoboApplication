using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Man;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Media;
using AquilaErpWpfApp3.Util;
using AquilaErpWpfApp3.View.M.Dialog;

namespace AquilaErpWpfApp3.ViewModel
{
    //인원 자격 관리
    public sealed class M6627ViewModel : ViewModelBase, INotifyPropertyChanged
    {

        private string _title = "인원 자격 관리";
        //private static ManServiceClient manClient = SystemProperties.ManClient;

        private IList<ManVo> selectedMasterViewList;
       
        //Master Dialog
        //private ICommand masterSearchDialogCommand;
        //private ICommand masterNewDialogCommand;
        //private ICommand masterEditDialogCommand;
        //private ICommand masterDelDialogCommand;
        //
        private M6627MasterDialog masterDialog;


        public M6627ViewModel() 
        {
            Refresh();
        }

        [Command]
        public async void Refresh(string _CERTI_NO = null)
        {
            try
            {
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6627", new StringContent(JsonConvert.SerializeObject(new ManVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectedMasterViewList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
                    }
                    //SelectedMasterViewList = manClient.M6627SelectMaster(new ManVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD });
                    if (SelectedMasterViewList.Count > 0)
                    {
                        isM_UPDATE = true;
                        isM_DELETE = true;

                        if (string.IsNullOrEmpty(_CERTI_NO))
                        {
                            SelectedMasterItem = SelectedMasterViewList[0];
                        }
                        else
                        {
                            SelectedMasterItem = SelectedMasterViewList.Where(x => x.CERTI_NO.Equals(_CERTI_NO)).LastOrDefault<ManVo>();
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
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }

        #region Functon (Master Add, Edit, Del)
        public IList<ManVo> SelectedMasterViewList
        {
            get { return selectedMasterViewList; }
            private set { SetProperty(ref selectedMasterViewList, value, () => SelectedMasterViewList); }
        }

        ManVo _selectMasterItem;
        public ManVo SelectedMasterItem
        {
            get
            {
                return _selectMasterItem;
            }
            set
            {
                if (value != null)
                {
                    SetProperty(ref _selectMasterItem, value, () => SelectedMasterItem, OnSelectedMasterItemChanged);
                }
            }
        }

        void OnSelectedMasterItemChanged()
        {
            //try
            //{
            //    if (this._selectMasterItem == null) { return; }
            //     //
            //    string deltFlg = this._selectMasterItem.DELT_FLG;
            //    this._selectMasterItem.DELT_FLG = null;
            //    this._selectMasterItem.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
            //    SelectedDetailViewList = codeClient.SelectDetailCode(this._selectMasterItem);
            //    this._selectMasterItem.DELT_FLG = deltFlg;
            //}
            //catch (System.Exception eLog)
            //{
            //    WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]시스템 분류 코드", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
            //    return;
            //}
        }

        //public ICommand MasterSearchDialogCommand
        //{
        //    get
        //    {
        //        if (masterSearchDialogCommand == null)
        //            masterSearchDialogCommand = new DelegateCommand(Refresh);
        //        return masterSearchDialogCommand;
        //    }
        //}

        //public void SearchMasterContact()
        //{
        //    ProgressWindow program = new ProgressWindow();
        //    try
        //    {
        //        ThreadStart start = delegate()
        //        {
        //            program.Dispatcher.Invoke(DispatcherPriority.Normal,(Action)(() => 
        //            {
        //                program.Show();
        //                Thread.Sleep(100);
        //                SelectedMasterViewList = codeClient.SelectMasterCode(new SystemCodeVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD });
        //                OnSelectedMasterItemChanged();
        //                program.Close();
        //            }));
        //        };
        //        new Thread(start).Start();
        //    }
        //    catch (System.Exception eLog)
        //    {
        //    //    program.Close();
        //        WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]표준 공정 관리", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
        //        return;
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
            masterDialog = new M6627MasterDialog(new ManVo());
            masterDialog.Title = _title + " - 추가";
            masterDialog.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            masterDialog.Owner = Application.Current.MainWindow;
            masterDialog.BorderEffect = BorderEffect.Default;
            masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            bool isDialog = (bool)masterDialog.ShowDialog();
            if (isDialog)
            {
                Refresh(masterDialog.resultDomain.CERTI_NO);
            }
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
            ManVo editDao = this._selectMasterItem;
            if (editDao != null)
            {
                masterDialog = new M6627MasterDialog(editDao);
                masterDialog.Title = _title + " - 수정";
                masterDialog.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                masterDialog.Owner = Application.Current.MainWindow;
                masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
                masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
                masterDialog.BorderEffect = BorderEffect.Default;
                bool isDialog = (bool)masterDialog.ShowDialog();
                if (isDialog)
                {
                    Refresh(masterDialog.resultDomain.CERTI_NO);
                }
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
            try
            {
                ManVo delDao = this._selectMasterItem;
                if (delDao != null)
                {
                    MessageBoxResult result = WinUIMessageBox.Show(delDao.CERTI_NO + "/" + delDao.CERTI_USR_NM + " 정말로 삭제 하시겠습니까?", _title, MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6627/d", new StringContent(JsonConvert.SerializeObject(delDao), System.Text.Encoding.UTF8, "application/json")))
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
                        //manClient.M6627DeleteMaster(delDao);
                        //WinUIMessageBox.Show("삭제가 완료되었습니다.", "[삭제]인원 자격 관리", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                        //Refresh();
                    }
                }
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title , MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
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

        private string _M_SEARCH_TEXT = string.Empty;
        public string M_SEARCH_TEXT
        {
            get { return _M_SEARCH_TEXT; }
            set { SetProperty(ref _M_SEARCH_TEXT, value, () => M_SEARCH_TEXT); }
        }
    }
}
