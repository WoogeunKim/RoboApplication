﻿using DevExpress.Mvvm;
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
using AquilaErpWpfApp3.S.View.Dialog;
using AquilaErpWpfApp3.Util;

namespace AquilaErpWpfApp3.ViewModel
{
    public sealed class S1147ViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private string title = "사업장별창고관리";

        private S1147MasterDialog s1147MasterDialog;

        //private ICommand _searchDialogCommand;
        //private ICommand _newDialogCommand;
        //private ICommand _editDialogCommand;
        public S1147ViewModel() 
        {
            //사업장
            SYSTEM_CODE_VO();
            // - Refresh();
        }

        #region Refresh()
        //public ICommand SearchDialogCommand
        //{
        //    get
        //    {
        //        if (_searchDialogCommand == null)
        //            _searchDialogCommand = new DelegateCommand(Refresh);
        //        return _searchDialogCommand;
        //    }
        //}

        [Command]
        public async void Refresh(string _AREA_CD_LOC_CD = null)
        {
            try
            {
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s1147", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { AREA_CD = M_SL_AREA_VO.CLSS_CD, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectMstList = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                    }
                    //SelectMstList = saleOrderClient.S2211SelectMstList(new SaleVo() { FM_DT = (StartDt).ToString("yyyy-MM-dd"), TO_DT = (EndDt).ToString("yyyy-MM-dd"), AREA_CD = _AreaMap[TXT_SL_AREA_NM] });
                    ////
                    Title = "[사업장]" + M_SL_AREA_VO.CLSS_DESC;

                    if (SelectMstList.Count >= 1)
                    {
                        isM_UPDATE = true;
                        isM_DELETE = true;

                        if (string.IsNullOrEmpty(_AREA_CD_LOC_CD))
                        {
                            SelectedMstItem = SelectMstList[0];
                        }
                        else
                        {
                            SelectedMstItem = SelectMstList.Where(x => (x.AREA_CD + "" + x.LOC_CD).Equals(_AREA_CD_LOC_CD)).LastOrDefault<SystemCodeVo>();
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

                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        } 
        #endregion


        #region Add()
        //public ICommand NewDialogCommand
        //{
        //    get
        //    {
        //        if (_newDialogCommand == null)
        //            _newDialogCommand = new DelegateCommand(NewContact);
        //        return _newDialogCommand;
        //    }
        //}
        [Command]
        public void NewContact()
        {

            try
            {

                s1147MasterDialog = new S1147MasterDialog(new SystemCodeVo() { AREA_CD = M_SL_AREA_VO.CLSS_CD, AREA_NM = M_SL_AREA_VO.CLSS_DESC });
                s1147MasterDialog.Title = this.title + " - 추가";
                s1147MasterDialog.Owner = Application.Current.MainWindow;
                s1147MasterDialog.BorderEffect = BorderEffect.Default;
                s1147MasterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
                s1147MasterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
                if (s1147MasterDialog.ShowDialog() == true)
                {
                    Refresh(s1147MasterDialog.resultDao.AREA_CD + s1147MasterDialog.resultDao.LOC_CD );
                }
            }
            catch (System.Exception eLog)
            {

                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }

        }
        #endregion

        #region Edit()
        //public ICommand EditDialogCommand
        //{
        //    get
        //    {
        //        if (_editDialogCommand == null)
        //            _editDialogCommand = new DelegateCommand(EditContact);
        //        return _editDialogCommand;
        //    }
        //}

        [Command]
        public void EditContact()
        {
            if (SelectedMstItem == null)
            {
                return;
            }

            try
            {

                s1147MasterDialog = new S1147MasterDialog(SelectedMstItem);
                s1147MasterDialog.Title = this.title + " - 수정";
                s1147MasterDialog.Owner = Application.Current.MainWindow;
                s1147MasterDialog.BorderEffect = BorderEffect.Default;
                s1147MasterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
                s1147MasterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
                if (s1147MasterDialog.ShowDialog() == true)
                {
                    Refresh(s1147MasterDialog.resultDao.AREA_CD + s1147MasterDialog.resultDao.LOC_CD);
                }

            }
            catch (System.Exception eLog)
            {

                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }

        }

        [Command]
        public async void DelContact()
        {
            if (SelectedMstItem == null)
            {
                return;
            }
            SystemCodeVo delDao = SelectedMstItem;
            if (delDao != null)
            {
                MessageBoxResult result = WinUIMessageBox.Show("[" + delDao.AREA_NM + "/" + delDao.LOC_NM + "]" + " 정말로 삭제 하시겠습니까?", title, MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        delDao.CRE_USR_ID = SystemProperties.USER_VO.CRE_USR_ID;
                        delDao.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;

                        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s1147/d", new StringContent(JsonConvert.SerializeObject(delDao), System.Text.Encoding.UTF8, "application/json")))
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
                }
            }
        }
        #endregion

        #region Variable
        private IList<SystemCodeVo> selectedMstList = new List<SystemCodeVo>();
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
                    SetProperty(ref _selectedMstItem, value, () => SelectedMstItem);
                }
            }
        } 

        //private Dictionary<string, string> _AreaMap = new Dictionary<string, string>();
        private IList<SystemCodeVo> _AreaCd = new List<SystemCodeVo>();
        public IList<SystemCodeVo> AreaList
        {
            get { return _AreaCd; }
            set { SetProperty(ref _AreaCd, value, () => AreaList); }
        }

        //사업장 
        private SystemCodeVo _M_SL_AREA_VO;
        public SystemCodeVo M_SL_AREA_VO
        {
            get { return _M_SL_AREA_VO; }
            set { SetProperty(ref _M_SL_AREA_VO, value, () => M_SL_AREA_VO); }
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

        string _Title = string.Empty;
        public string Title
        {
            get { return _Title; }
            set { SetProperty(ref _Title, value, () => Title); }
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
                        M_SL_AREA_VO = AreaList[0];
                    }
                }
            }
            Refresh();
        }
        #endregion
    }
}
