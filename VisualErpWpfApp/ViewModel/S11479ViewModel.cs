using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Man;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Media;
using ModelsLibrary.Code;
using AquilaErpWpfApp3.View.S.Dialog;
using AquilaErpWpfApp3.Util;

namespace AquilaErpWpfApp3.ViewModel
{
    public sealed class S11479ViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private string title = "납품처관리";
        private IList<SystemCodeVo> selectedMstList = new List<SystemCodeVo>();
        private S11479MasterDialog masterDialog;
        private S11479DetailDialog DetailDialog;

        public S11479ViewModel() 
        {
            StartDt = System.DateTime.Now;
            EndDt = System.DateTime.Now;
          
            //SYSTEM_CODE_VO();
            Refresh();
        }

        [Command]
        public async void Refresh(string _DE_CHD_NO = null)
        {
            try
            {
                SystemCodeVo _param = new SystemCodeVo();
                _param.FM_DT = (StartDt).ToString("yyyy-MM-01");
                _param.TO_DT = (EndDt).ToString("yyyy-MM-dd");
                _param.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;

                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("S11479/mst", new StringContent(JsonConvert.SerializeObject(_param), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectMstList = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                    }

                    if (SelectMstList.Count >= 1)
                    {
                        isM_UPDATE = true;
                        isM_DELETE = true;
                        isD_UPDATE = true;

                        if (string.IsNullOrEmpty(_DE_CHD_NO))
                        {
                            SelectedMstItem = SelectMstList[0];
                        }
                        else
                        {
                            SelectedMstItem = SelectMstList.Where(x => x.DE_CHD_NO.Equals(_DE_CHD_NO)).LastOrDefault<SystemCodeVo>();
                        }
                    }
                    else
                    {
                        //SelectDtlList = null;
                        //SearchDetail = null;

                        isM_UPDATE = false;
                        isM_DELETE = false;
                        isD_UPDATE = false;

                        //isD_UPDATE = false;
                        //isD_DELETE = false;
                    }
                }
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }

        //#region Functon <Master List>
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

        private string _M_SEARCH_TEXT = string.Empty;
        public string M_SEARCH_TEXT
        {
            get { return _M_SEARCH_TEXT; }
            set { SetProperty(ref _M_SEARCH_TEXT, value, () => M_SEARCH_TEXT); }
        }

        public IList<SystemCodeVo> SelectMstList
        {
            get { return selectedMstList; }
            set { SetProperty(ref selectedMstList, value, () => SelectMstList); }
        }

        SystemCodeVo _selectedMstItem;
        public SystemCodeVo SelectedMstItem
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

        private IList<DateTime> selectedDates = new List<DateTime>();
        public IList<DateTime> SelectedDates
        {
            get { return selectedDates; }
            set { SetProperty(ref selectedDates, value, () => SelectedDates); }
        }




        private bool? _isM_UPDATE = false;
        public bool? isM_UPDATE
        {
            get { return _isM_UPDATE; }
            set { SetProperty(ref _isM_UPDATE, value, () => isM_UPDATE); }
        }



        private bool? _isD_UPDATE = false;
        public bool? isD_UPDATE
        {
            get { return _isD_UPDATE; }
            set { SetProperty(ref _isD_UPDATE, value, () => isD_UPDATE); }
        }

        private bool? _isM_DELETE = false;
        public bool? isM_DELETE
        {
            get { return _isM_DELETE; }
            set { SetProperty(ref _isM_DELETE, value, () => isM_DELETE); }
        }

        //
        //private bool? _isD_UPDATE = false;
        //public bool? isD_UPDATE
        //{
        //    get { return _isD_UPDATE; }
        //    set { SetProperty(ref _isD_UPDATE, value, () => isD_UPDATE); }
        //}

        //private bool? _isD_DELETE = false;
        //public bool? isD_DELETE
        //{
        //    get { return _isD_DELETE; }
        //    set { SetProperty(ref _isD_DELETE, value, () => isD_DELETE); }
        //}


        
       

        private void SelectMstDetail()
        {
            try
            {
                if (SelectedMstItem == null)
                {
                    return;
                }

                isM_UPDATE = true;
                isD_UPDATE = true;

                if(Convert.ToInt32(SelectedMstItem.DE_PRNT_NO) == 0)
                {
                    isM_UPDATE = true;
                    isD_UPDATE = false;
                }
                else
                {
                    isM_UPDATE = false;
                    isD_UPDATE = true;

                }
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }
        //#endregion


        //#region Functon <Detail List>
        //public IList<JobVo> SelectDtlList
        //{
        //    get { return selectedDtlList; }
        //    set { SetProperty(ref selectedDtlList, value, () => SelectDtlList); }
        //}

        //JobVo _searchDetail;
        //public JobVo SearchDetail
        //{
        //    get
        //    {
        //        return _searchDetail;
        //    }
        //    set
        //    {
        //        if (value != null)
        //        {
        //            SetProperty(ref _searchDetail, value, () => SearchDetail);
        //        }
        //    }
        //}
        //#endregion


        string _Title = string.Empty;
        public string Title
        {
            get { return _Title; }
            set { SetProperty(ref _Title, value, () => Title); }
        }


        //string _isDetailView = string.Empty;
        //public string DetailView
        //{
        //    get { return _isDetailView; }
        //    set { SetProperty(ref _isDetailView, value, () => DetailView); }
        //}


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

        //public ICommand NewDialogCommand
        //{
        //    get
        //    {
        //        if (_newDialogCommand == null)
        //            _newDialogCommand = new DelegateCommand(NewContact);
        //        return _newDialogCommand;
        //    }
        //}


        // 마스터 추가
        [Command]
        public void NewMstContact()
        {
            masterDialog = new S11479MasterDialog(new SystemCodeVo());
            masterDialog.Title = title + " - 추가";
            masterDialog.Owner = Application.Current.MainWindow;
            masterDialog.BorderEffect = BorderEffect.Default;
            masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            bool isDialog = (bool)masterDialog.ShowDialog();
            if (isDialog)
            {
                Refresh();
            }
        }


        // 디테일 추가
        [Command]
        public void NewDtlContact()
        {
            SystemCodeVo _tmp = new SystemCodeVo();
            if(Convert.ToInt32(SelectedMstItem.DE_PRNT_NO) == 0)
            {
                // 부모를 클릭했을때는 부모의 고유 번호 투입
                _tmp.DE_PRNT_NO = SelectedMstItem.DE_CHD_NO;
                _tmp.CO_NM = SelectedMstItem.CO_NM;
                _tmp.DE_CO_NM = SelectedMstItem.DE_CO_NM;
                _tmp.S_CRE_DT = System.DateTime.Now.ToString("yyyy-MM-dd");
                _tmp.S_DUE_DT = System.DateTime.Now.ToString("yyyy-MM-dd");
                _tmp.DELT_FLG = "사용";
            }
            else
            {
                // 자식을 클릭했을때는 부모 번호 투입
                _tmp.DE_PRNT_NO = SelectedMstItem.DE_PRNT_NO;
                _tmp.CO_NM = SelectedMstItem.CO_NM;
                _tmp.DE_CO_NM = SelectedMstItem.DE_CO_NM;
                _tmp.S_CRE_DT = System.DateTime.Now.ToString("yyyy-MM-dd");
                _tmp.S_DUE_DT = System.DateTime.Now.ToString("yyyy-MM-dd");
                _tmp.DELT_FLG = "사용";
            }
            //DetailDialog = new S11479DetailDialog(new SystemCodeVo() { DE_CHD_NO = SelectedMstItem.DE_PRNT_NO, CHNL_CD = SystemProperties.USER_VO.CHNL_CD } );
            DetailDialog = new S11479DetailDialog(_tmp);
            DetailDialog.Title = title + " - 추가";
            DetailDialog.Owner = Application.Current.MainWindow;
            DetailDialog.BorderEffect = BorderEffect.Default;
            DetailDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            DetailDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            bool isDialog = (bool)DetailDialog.ShowDialog();
            if (isDialog)
            {
                //Refresh(DetailDialog.resultDomain.DE_CHD_NO.ToString());
                Refresh(DetailDialog.resultDomain.DE_PRNT_NO.ToString());
            }
        }

        //public ICommand EditDtlDialogCommand
        //{
        //    get
        //    {
        //        if (_editDetailDialogCommand == null)
        //            _editDetailDialogCommand = new DelegateCommand(EditDtlContact);
        //        return _editDetailDialogCommand;
        //    }
        //}


        // 마스터 수정
        [Command]
        public void EditMstContact()
        {
            if (SelectedMstItem == null)
            {
                return;
            }
            SystemCodeVo _tmp = new SystemCodeVo();
            if (Convert.ToInt32(SelectedMstItem.DE_PRNT_NO) != 0)
            {
                // 자식을 클릭했을때 부모 번호 투입
                SelectedMstItem.DE_CHD_NO = SelectedMstItem.DE_PRNT_NO;
            }
            else
            {
                // 부모를 클릭했을때 본인의 번호 투입(의미x)
                //SelectedMstItem.DE_CHD_NO = SelectedMstItem.DE_CHD_NO;
            }

            masterDialog = new S11479MasterDialog(SelectedMstItem);
            masterDialog.Title = title + " - 수정";
            masterDialog.Owner = Application.Current.MainWindow;
            masterDialog.BorderEffect = BorderEffect.Default;
            masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            bool isDialog = (bool)masterDialog.ShowDialog();
            if (isDialog)
            {
               Refresh(masterDialog.resultDomain.DE_CHD_NO.ToString());
            }
        }



        // 디테일 수정
        [Command]
        public void EditDtlContact()
        {
            if (SelectedMstItem == null)
            {
                return;
            }
          
            DetailDialog = new S11479DetailDialog(SelectedMstItem);
            DetailDialog.Title = title + " - 수정";
            DetailDialog.Owner = Application.Current.MainWindow;
            DetailDialog.BorderEffect = BorderEffect.Default;
            DetailDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            DetailDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            bool isDialog = (bool)DetailDialog.ShowDialog();
            if (isDialog)
            {
                //Refresh(DetailDialog.resultDomain.DE_CHD_NO.ToString());
                Refresh(DetailDialog.resultDomain.DE_PRNT_NO.ToString());
            }
        }


        //public ICommand DelDialogCommand
        //{
        //    get
        //    {
        //        if (_delDialogCommand == null)
        //            _delDialogCommand = new DelegateCommand(DelContact);
        //        return _delDialogCommand;
        //    }
        //}

        [Command]
        public async void DelContact()
        {
            if (SelectedMstItem != null)
            {
                MessageBoxResult result = WinUIMessageBox.Show("[" /*+ SelectedMstItem.EXPT_DT*/ + "] 정말로 삭제 하시겠습니까?", title, MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m66110/d", new StringContent(JsonConvert.SerializeObject(SelectedMstItem), System.Text.Encoding.UTF8, "application/json")))
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
                    }
                    catch (System.Exception eLog)
                    {
                        WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                        return;
                    }

                    //WinUIMessageBox.Show("삭제가 완료되었습니다.", title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                    //Refresh();
                }
            }
        }



        //public void setTitle()
        //{
        //    Title = "[기간]" + (StartDt).ToString("yyyy-MM-dd") + "~" + (EndDt).ToString("yyyy-MM-dd") + ", [사업장]" + M_SL_AREA_NM.CLSS_DESC ;
        //}

        //public async void SYSTEM_CODE_VO()
        //{
        //    //SL_AREA_LIST = SystemProperties.SYSTEM_CODE_VO("L-002");
        //    //using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "L-002"))
        //    //{
        //    //    if (response.IsSuccessStatusCode)
        //    //    {
        //    //        AreaList = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
        //    //        if (AreaList.Count > 0)
        //    //        {
        //    //            M_SL_AREA_NM = AreaList[0];
        //    //        }
        //    //    }
        //    //}
        //    //Refresh();
        //}

    }
}
