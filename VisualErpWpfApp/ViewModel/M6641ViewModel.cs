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
    //설비 관리
    public sealed class M6641ViewModel : ViewModelBase, INotifyPropertyChanged
    {

        private string _title = "설비 관리";
        //private static ManServiceClient manClient = SystemProperties.ManClient;

        private IList<ManVo> selectedMasterViewList;
       
        //Master Dialog
        //private ICommand masterSearchDialogCommand;
        //private ICommand masterNewDialogCommand;
        //private ICommand masterEditDialogCommand;
        //private ICommand masterDelDialogCommand;
        //
        private M6641MasterDialog masterDialog;


        public M6641ViewModel() 
        {
            Refresh();
        }


        [Command]
        public async void Refresh(string _PROD_EQ_NO = null)
        {
            try
            {
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6641", new StringContent(JsonConvert.SerializeObject(new ManVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectedMasterViewList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
                    }

                    //SelectedMasterViewList = manClient.M6622SelectMaster(new ManVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD });
                    if (SelectedMasterViewList.Count > 0)
                    {
                        isM_UPDATE = true;
                        isM_DELETE = true;

                        if (string.IsNullOrEmpty(_PROD_EQ_NO))
                        {
                            SelectedMasterItem = SelectedMasterViewList[0];
                        }
                        else
                        {
                            SelectedMasterItem = SelectedMasterViewList.Where(x => x.PROD_EQ_NO.Equals(_PROD_EQ_NO)).LastOrDefault<ManVo>();
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

        [Command]
        public void NewMasterContact()
        {
            masterDialog = new M6641MasterDialog(new ManVo() { EQ_LOC_CD = "100", EQ_LOC_NM = "본사" });
            masterDialog.Title = _title + " - 추가";
            masterDialog.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            masterDialog.Owner = Application.Current.MainWindow;
            masterDialog.BorderEffect = BorderEffect.Default;
            masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            bool isDialog = (bool)masterDialog.ShowDialog();
            if (isDialog)
            {
                Refresh(masterDialog.resultDomain.PROD_EQ_NO);
                //if (masterDialog.IsEdit == false)
                //{
                //    Refresh();

                //    for (int x = 0; x < SelectedMasterViewList.Count; x++)
                //    {
                //        if ((masterDialog.resultDomain.CHNL_CD + "_" + masterDialog.resultDomain.PROD_EQ_NO).Equals(SelectedMasterViewList[x].CHNL_CD + "_" + SelectedMasterViewList[x].PROD_EQ_NO))
                //        {
                //            SelectedMasterItem = SelectedMasterViewList[x];
                //            break;
                //        }
                //    }
                //}
            }


        }

        [Command]
        public void EditMasterContact()
        {
            if (this._selectMasterItem == null) { return; }
            ManVo editDao = this._selectMasterItem;
            if (editDao != null)
            {
                masterDialog = new M6641MasterDialog(editDao);
                masterDialog.Title = _title + " - 수정";
                masterDialog.Owner = Application.Current.MainWindow;
                masterDialog.BorderEffect = BorderEffect.Default;
                masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
                masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
                bool isDialog = (bool)masterDialog.ShowDialog();
                if (isDialog)
                {
                    Refresh(masterDialog.resultDomain.PROD_EQ_NO);
                    //if (masterDialog.IsEdit == false)
                    //{
                    //    Refresh();
                    //}
                }
            }
        }

        [Command]
        public async void DelMasterContact()
        {
            try
            {
                ManVo delDao = this._selectMasterItem;
                if (delDao != null)
                {
                    MessageBoxResult result = WinUIMessageBox.Show(delDao.PROD_EQ_NO + " 정말로 삭제 하시겠습니까?", "[삭제]설비 관리", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6641/d", new StringContent(JsonConvert.SerializeObject(delDao), System.Text.Encoding.UTF8, "application/json")))
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
                        //        delDao.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
                        //        manClient.M6622DeleteMaster(delDao);
                        //        WinUIMessageBox.Show("삭제가 완료되었습니다.", "[삭제]설비 관리", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                        //        Refresh();
                    }
                }
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
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
