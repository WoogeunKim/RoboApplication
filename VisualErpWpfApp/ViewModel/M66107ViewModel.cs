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
using System.Windows.Input;
using AquilaErpWpfApp3.Util;
using DevExpress.Xpf.Core;
using System.Windows.Media;
using AquilaErpWpfApp3.View.M.Dialog;


namespace AquilaErpWpfApp3.ViewModel
{
    public sealed class M66107ViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private string title = "Loss 최적화 수행";

        private IList<ManVo> selectedMstList = new List<ManVo>();

        private M66107MasterDialog masterDialog;
        private M66107DetailDialog detailDialog;
        private M66107OptiDialog optiRunDialog;
        private M66107PurOpmzDialog purOpmzDialog;

        public M66107ViewModel()
        {
            StartDt = System.DateTime.Now;
            EndDt = System.DateTime.Now;

            isM_DELETE = false;
            isM_UPDATE = false;

            Refresh();
        }

        [Command]
        public async void Refresh(string _OPMZ_NO = null)
        {
            try
            {
                ManVo _param = new ManVo();
                _param.FM_DT = (StartDt).ToString("yyyy-MM-dd");
                _param.TO_DT = (EndDt).ToString("yyyy-MM-dd");

                _param.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;

                if (EndDt < StartDt)
                {
                    WinUIMessageBox.Show("조회일자가 올바르지 않습니다.", title, MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                SelectDtlList = null;

                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("M66107/mst", new StringContent(JsonConvert.SerializeObject(_param), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectMstList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();

                        Title = "[기간]" + (StartDt).ToString("yyyy-MM-dd") + "~" + (EndDt).ToString("yyyy-MM-dd");

                        if (SelectMstList.Count > 0)
                        {
                            //isM_UPDATE = true;

                            if (string.IsNullOrEmpty(_OPMZ_NO))
                            {
                                SelectedMstItem = null;
                                SelectDtlList = null;

                                isM_UPDATE = false;
                            }
                            else
                            {
                                SelectedMstItem = SelectMstList.Where(x => x.OPMZ_NO.Equals(_OPMZ_NO)).LastOrDefault<ManVo>();
                            }
                        }
                        else
                        {
                            isM_UPDATE = false;
                            SelectDtlList = null;
                        }

                    }
                }
            }
            catch (System.Exception eLog)
            {
                //DXSplashScreen.Close();
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _Title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }

        [Command]
        public async void DtListRefresh()
        {
            try
            {
                if (SelectedMstItem == null) return;
                if (DXSplashScreen.IsActive == false) DXSplashScreen.Show<ProgressWindow>();

                // 완료된 건은 수정,자재추가, 최적화 불가능함. 
                //if (SelectedMstItem.CLZ_FLG.Equals("N"))
                //{
                //    isM_DELETE = false;
                //    isM_UPDATE = true;
                //}
                //else
                //{
                //    isM_DELETE = true;
                //    isM_UPDATE = false;
                //}
                isM_UPDATE = true;

                // 해당 최적화 건에 로그상태
                using (HttpResponseMessage responseLog = await SystemProperties.PROGRAM_HTTP.PostAsync("m66107/mst/log", new StringContent(JsonConvert.SerializeObject(SelectedMstItem), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (responseLog.IsSuccessStatusCode)
                    {
                        this.SelectMstLogList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await responseLog.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
                    }
                }

                // 최적화 해당 자재 리스트
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m66107/dtl", new StringContent(JsonConvert.SerializeObject(SelectedMstItem), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectDtlList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
                    }
                }

                if (DXSplashScreen.IsActive == true) DXSplashScreen.Close();
            }
            catch (System.Exception eLog)
            {
                if (DXSplashScreen.IsActive == true) DXSplashScreen.Close();
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }

        // MST 추가
        [Command]
        public void NewContact()
        {
            try
            {
                masterDialog = new M66107MasterDialog(new ManVo() { });
                masterDialog.Title = title + " - 추가";
                masterDialog.Owner = Application.Current.MainWindow;
                masterDialog.BorderEffect = BorderEffect.Default;
                masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
                masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
                bool isDialog = (bool)masterDialog.ShowDialog();
                if (isDialog)
                {
                    Refresh(masterDialog.OPMZ_NO);
                }
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }

        // MST 수정
        [Command]
        public void EditContact()
        {
            try
            {
                if (SelectedMstItem == null) return;

                masterDialog = new M66107MasterDialog(SelectedMstItem);
                masterDialog.Title = title + " - 수정";
                masterDialog.Owner = Application.Current.MainWindow;
                masterDialog.BorderEffect = BorderEffect.Default;
                masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
                masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
                bool isDialog = (bool)masterDialog.ShowDialog();
                if (isDialog)
                {
                    Refresh(SelectedMstItem.OPMZ_NO);
                }
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }

        // dtl 추가
        [Command]
        public void New2Contact()
        {
            try
            {
                if (SelectedMstItem == null) return;

                detailDialog = new M66107DetailDialog(SelectedMstItem);
                detailDialog.Title = title + " - 추가";
                detailDialog.Owner = Application.Current.MainWindow;
                detailDialog.BorderEffect = BorderEffect.Default;
                detailDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
                detailDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
                bool isDialog = (bool)detailDialog.ShowDialog();
                if (isDialog)
                {
                    Refresh(SelectedMstItem.OPMZ_NO);
                }
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }

        // dtl 추가
        [Command]
        public void OptiRun()
        {
            try
            {
                if (SelectedMstItem == null || SelectDtlList.Count == 0)
                {
                    WinUIMessageBox.Show("조회된 데이터가 없습니다.", "[유효검사]", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.None, MessageBoxOptions.None);
                    return;
                }


                optiRunDialog = new M66107OptiDialog(SelectedMstItem);
                optiRunDialog.Title = title + " - Opti Run";
                optiRunDialog.Owner = Application.Current.MainWindow;
                optiRunDialog.BorderEffect = BorderEffect.Default;
                optiRunDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
                optiRunDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
                bool isDialog = (bool)optiRunDialog.ShowDialog();
                if (isDialog)
                {
                    Refresh(SelectedMstItem.OPMZ_NO);
                }

                //if (string.IsNullOrEmpty(SelectedMstItem.RUN_CLSS_NM) == true)
                //{
                //    WinUIMessageBox.Show("최적화 구분이 없습니다.", "[유효검사]", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.None, MessageBoxOptions.None);
                //    return;
                //}
                //MessageBoxResult isResult = WinUIMessageBox.Show(SelectedMstItem.RUN_CLSS_NM + " [ Opti.NO: " + SelectedMstItem.OPMZ_NO + " ] 를 확정하겠습니까? ", title + "[완료]", MessageBoxButton.YesNo, MessageBoxImage.Question);
                //if (isResult == MessageBoxResult.Yes)
                //{
                //    // A:생산최적화 = N  && B:발주최적화 = Y
                //    string _isChecked = SelectedMstItem.RUN_CLSS_CD.Equals("A") ? "N" : "Y";

                //    HttpClient httpClient = new HttpClient();
                //    httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                //    //string url = "http://210.217.42.139:8880/robocon/api/optimize_barlist/v2?"; 
                //    //string value = "OPMZ_NO=" + orgDao.OPMZ_NO + "&" + "APPLY_ELON=" + _isChecked;
                //    string url = "http://aiblue.ddns.net:8880/robocon/api/optimize_barlist/v3?"; // 2023-04-04 호출주소 변경

                //    string value = "OPMZ_NO=" + SelectedMstItem.OPMZ_NO + "&" + "PLANNING_MODE=" + _isChecked;

                //    httpClient.GetAsync(url + value);

                //    Refresh(SelectedMstItem.OPMZ_NO);
                //}
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }

        [Command]
        public async void OptiPurMtrl()
        {
            if (SelectedMstItem == null) return;

            // 발주최적화가 아닐 경우
            if (!SelectedMstItem.RUN_CLSS_CD.Equals("B"))
            {
                WinUIMessageBox.Show("발주최적화 대상이 아닙니다.", "[유효검사]", MessageBoxButton.OK, MessageBoxImage.Warning);
                //return;
            }

            // 발주일 경우 원자재 조회
            purOpmzDialog = new M66107PurOpmzDialog(SelectedMstItem);
            purOpmzDialog.Owner = Application.Current.MainWindow;
            purOpmzDialog.BorderEffect = BorderEffect.Default;
            purOpmzDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            purOpmzDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            bool isDialog = (bool)purOpmzDialog.ShowDialog();
            if (isDialog)
            {
                //Refresh(SelectedMstItem.OPMZ_NO);
            }

        }


        [Command]
        public async void OptiOk()
        {
            try
            {
                if (SelectedMstItem == null) return;

                MessageBoxResult isResult = WinUIMessageBox.Show("[ Opti.NO: " + SelectedMstItem.OPMZ_NO + " ] 를 확정하겠습니까? ", title + "[완료]", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (isResult == MessageBoxResult.Yes)
                {
                    int _Num = 0;

                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m66107/mst/u", new StringContent(JsonConvert.SerializeObject(new ManVo() { OPMZ_NO = SelectedMstItem.OPMZ_NO, OPMZ_NM = SelectedMstItem.OPMZ_NM, OPMZ_RMK = SelectedMstItem.OPMZ_RMK, CLZ_FLG = "Y", UPD_USR_ID = SystemProperties.USER }), System.Text.Encoding.UTF8, "application/json")))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string result = await response.Content.ReadAsStringAsync();
                            if (int.TryParse(result, out _Num) == false)
                            {
                                //실패
                                WinUIMessageBox.Show(result, title + "[완료]", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }

                            //성공
                            WinUIMessageBox.Show("완료 되었습니다", title + "[완료]", MessageBoxButton.OK, MessageBoxImage.Information);


                            Refresh(SelectedMstItem.OPMZ_NO);
                        }
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

        public IList<ManVo> SelectMstList
        {
            get { return selectedMstList; }
            set { SetProperty(ref selectedMstList, value, () => SelectMstList); }
        }

        ManVo _selectedMstItem;
        public ManVo SelectedMstItem
        {
            get
            {
                return _selectedMstItem;
            }
            set
            {
                SetProperty(ref _selectedMstItem, value, () => SelectedMstItem); DtListRefresh();

            }
        }

        IList<ManVo> _selectMstLogList = new List<ManVo>();
        public IList<ManVo> SelectMstLogList
        {
            get { return _selectMstLogList; }
            set { SetProperty(ref _selectMstLogList, value, () => SelectMstLogList); }
        }

        private IList<ManVo> selectedDtlList = new List<ManVo>();

        public IList<ManVo> SelectDtlList
        {
            get { return selectedDtlList; }
            set { SetProperty(ref selectedDtlList, value, () => SelectDtlList); }
        }

        string _Title = string.Empty;
        public string Title
        {
            get { return _Title; }
            set { SetProperty(ref _Title, value, () => Title); }
        }

        bool _isM_UPDATE; 
        public bool isM_UPDATE
        {
            get { return _isM_UPDATE; }
            set { SetProperty(ref _isM_UPDATE, value, () => isM_UPDATE);}
        }

        bool _isM_DELETE;
        public bool isM_DELETE
        {
            get { return _isM_DELETE; }
            set { SetProperty(ref _isM_DELETE, value, () => isM_DELETE); }
        }
    }
    
}
