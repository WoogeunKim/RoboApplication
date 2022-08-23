using AquilaErpWpfApp3.M.View.Dialog;
using AquilaErpWpfApp3.Util;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Code;
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
    //시스템 분류 코드
    public sealed class M5210ViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private string title = "생산계획";

        private IList<ManVo> selectedMasterViewList;
        private IList<ManVo> selectedDetailViewList;
       
        //Master Dialog
        private M5210MasterDialog masterDialog;

        //Detail Dialog

        public M5210ViewModel() 
        {

            StartDt = System.DateTime.Now.AddMonths(-1);
            EndDt = System.DateTime.Now;


            SYSTEM_CODE_VO();

            MstRefresh();
        }

        [Command]
        public async void MstRefresh(string _PROD_PLN_NO = null)
        {
            try
            {
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m5210/mst", new StringContent(JsonConvert.SerializeObject(new ManVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD, FM_DT = (StartDt).ToString("yyyy-MM-dd"), TO_DT = (EndDt).ToString("yyyy-MM-dd") }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectedMasterViewList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();


                        //
                        if (SelectedMasterViewList.Count > 0)
                        {
                            isM_UPDATE = true;
                            isM_DELETE = true;

                            if (string.IsNullOrEmpty(_PROD_PLN_NO))
                            {
                                SelectedMasterItem = SelectedMasterViewList[0];
                            }
                            else
                            {
                                SelectedMasterItem = SelectedMasterViewList.Where(x => x.PROD_PLN_NO.StartsWith(_PROD_PLN_NO)).LastOrDefault<ManVo>();
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
            }
            catch (System.Exception eLog)
            {
                //program.Close();
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
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
            DtlRefresh();
        }

       [Command]
       public async void DtlRefresh(string _CLSS_CD = null)
       {
            try
            {
                if (this.SelectedMasterItem == null) { return; }
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m5210/dtl", new StringContent(JsonConvert.SerializeObject(SelectedMasterItem), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectedDetailViewList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();


                        //
                        if (SelectedDetailViewList.Count > 0)
                        {
                            isD_UPDATE = true;
                            isD_DELETE = true;

                            //if (string.IsNullOrEmpty(_CLSS_CD))
                            //{
                            SelectedDetailItem = SelectedDetailViewList[0];
                            //}
                            //else
                            //{
                            //    SelectedDetailItem = SelectedDetailViewList.Where(x => x.CLSS_CD.Equals(_CLSS_CD)).FirstOrDefault<SystemCodeVo>();
                            //}
                        }
                        else
                        {
                            isD_UPDATE = false;
                            isD_DELETE = false;
                        }
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
        public void NewMasterContact()
        {
            masterDialog = new M5210MasterDialog(new ManVo());
            masterDialog.Title = title + " - 추가";
            masterDialog.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            masterDialog.Owner = Application.Current.MainWindow;
            //masterDialog.BorderEffect = BorderEffect.Default;
            //masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            //masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            bool isDialog = (bool)masterDialog.ShowDialog();
            if (isDialog)
            {
                MstRefresh( "UF" + Convert.ToDateTime(masterDialog.ResultDao.PROD_PLN_DT).ToString("yyMMdd"));

                //if (masterDialog.IsEdit == false)
                //{
                //    Refresh();

                //    for (int x = 0; x < SelectedMasterViewList.Count; x++)
                //    {
                //        if ((masterDialog.resultDomain.CHNL_CD + "_" + masterDialog.resultDomain.ROUT_CD).Equals(SelectedMasterViewList[x].CHNL_CD + "_" + SelectedMasterViewList[x].ROUT_CD))
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
                masterDialog = new M5210MasterDialog(editDao);
                masterDialog.Title = title + " - 수정";
                masterDialog.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                masterDialog.Owner = Application.Current.MainWindow;
                //masterDialog.BorderEffect = BorderEffect.Default;
                //masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
                //masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
                bool isDialog = (bool)masterDialog.ShowDialog();
                if (isDialog)
                {
                    DtlRefresh();

                    //if (masterDialog.IsEdit == false)
                    //{
                    //    Refresh();
                    //}
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
                    if (this.SelectedDetailItem == null) { return; }
                    ManVo delDao = this.SelectedDetailItem;
                if (delDao != null)
                {

                    MessageBoxResult result = WinUIMessageBox.Show(delDao.PROD_PLN_NO + " 정말로 삭제 하시겠습니까?", title, MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {

                        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m5210/mst/d", new StringContent(JsonConvert.SerializeObject(delDao), System.Text.Encoding.UTF8, "application/json")))
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
                                DtlRefresh();

                                //성공
                                WinUIMessageBox.Show("삭제가 완료되었습니다.", title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                            }
                        }
                    }
                }
            }
             catch (System.Exception eLog)
             {
                 WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                 return;
             }
        }
        #endregion



        #region Functon (Detail Add, Edit, Del)
        public IList<ManVo> SelectedDetailViewList
        {
            get { return selectedDetailViewList; }
            private set { SetProperty(ref selectedDetailViewList, value, () => SelectedDetailViewList); }
        }

        ManVo _selectedDetailItem;
        public ManVo SelectedDetailItem
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

        //public void SearchDetailContact()
        //{
        //    OnSelectedMasterItemChanged();
        //}

        //public ICommand DetailNewDialogCommand
        //{
        //    get
        //    {
        //        if (detailNewDialogCommand == null)
        //            detailNewDialogCommand = new DelegateCommand(NewDetailContact);
        //        return detailNewDialogCommand;
        //    }
        //}

        //[Command]
        //public void NewDetailContact()
        //{
        //    if (this._selectMasterItem == null) { return; }
        //    ShowDetailDialog(new SystemCodeVo() { CLSS_TP_CD = _selectMasterItem.CLSS_TP_CD });
        //}

        ////public ICommand DetailEditDialogCommand
        ////{
        ////    get
        ////    {
        ////        if (detailEditDialogCommand == null)
        ////            detailEditDialogCommand = new DelegateCommand(EditDetailContact, (SelectedDetailItem == null ? false : true));
        ////        return detailEditDialogCommand;
        ////    }
        ////}

        //[Command]
        //public void EditDetailContact()
        //{
        //    SystemCodeVo editDao = SelectedDetailItem;
        //    if (editDao != null)
        //    {
        //        ShowDetailDialog(editDao);
        //    }
        //}

        //public void ShowDetailDialog(SystemCodeVo dao)
        //{
        //    detailDialog = new S131DetailDialog(dao);
        //    detailDialog.Title = "Detail Code (분류 코드 : " + _selectMasterItem.CLSS_TP_CD + ")";
        //    detailDialog.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        //    detailDialog.Owner = Application.Current.MainWindow;
        //    detailDialog.BorderEffect = BorderEffect.Default;
        //    detailDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
        //    detailDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
        //    bool isDialog = (bool)detailDialog.ShowDialog();
        //    if (isDialog)
        //    {
        //        DtlRefresh(detailDialog.resultDomain.CLSS_CD);
        //        //SelectedDetailItem = SelectedDetailViewList.Where(x => x.CLSS_CD.Equals(detailDialog.resultDomain.CLSS_CD)).FirstOrDefault<SystemCodeVo>();
        //    }
        //}

        //public ICommand DetailDelDialogCommand
        //{
        //    get
        //    {
        //        if (detailDelDialogCommand == null)
        //            detailDelDialogCommand = new DelegateCommand(DelDetailContact);
        //        return detailDelDialogCommand;
        //    }
        //}

        //public void DelDetailContact()
        //{
        //    try
        //    {
        //        SystemCodeVo delDao = this.SelectedDetailItem;
        //        if (delDao != null)
        //        {
        //            if (delDao.DELT_FLG.Equals("미사용"))
        //            {
        //                WinUIMessageBox.Show("현재 미사용 코드 입니다.", "[삭제 - Detail]" + title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
        //                return;
        //            }
        //            //
        //            MessageBoxResult result = WinUIMessageBox.Show(delDao.CLSS_CD + " 정말로 삭제 하시겠습니까?", "[삭제 - Detail]" + title, MessageBoxButton.YesNo, MessageBoxImage.Question);
        //            if (result == MessageBoxResult.Yes)
        //            {
        //                delDao.DELT_FLG = "Y";
        //                delDao.USR_ID = SystemProperties.USER;
        //                //codeClient.UpdateDetailCode(delDao);
        //                WinUIMessageBox.Show("삭제가 완료되었습니다.", "[삭제 - Detail]" + title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
        //                //
        //                delDao.DELT_FLG = "미사용";
        //            }
        //        }
        //    }
        //    catch (System.Exception eLog)
        //    {
        //        WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
        //         return;
        //    }
        //}
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

        //private string _D_SEARCH_TEXT = string.Empty;
        //public string D_SEARCH_TEXT
        //{
        //    get { return _D_SEARCH_TEXT; }
        //    set { SetProperty(ref _D_SEARCH_TEXT, value, () => D_SEARCH_TEXT); }
        //}




        //
        //사업장
        //private Dictionary<string, string> _AreaMap = new Dictionary<string, string>();
        private IList<SystemCodeVo> _AreaCd = new List<SystemCodeVo>();
        public IList<SystemCodeVo> AreaList
        {
            get { return _AreaCd; }
            set { SetProperty(ref _AreaCd, value, () => AreaList); }
        }

        //사업장
        private SystemCodeVo _M_SL_AREA_NMT;
        public SystemCodeVo M_SL_AREA_NM
        {
            get { return _M_SL_AREA_NMT; }
            set { SetProperty(ref _M_SL_AREA_NMT, value, () => M_SL_AREA_NM); }
        }



        public async void SYSTEM_CODE_VO()
        {
            //SL_AREA_LIST = SystemProperties.SYSTEM_CODE_VO("L-002");
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "L-002"))
            {
                if (response.IsSuccessStatusCode)
                {
                    AreaList = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                    if (AreaList.Count > 0)
                    {
                        M_SL_AREA_NM = AreaList[0];
                    }
                }
            }
            //Refresh();
        }

    }
}
