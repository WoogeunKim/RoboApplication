using AquilaErpWpfApp3.Util;
using AquilaErpWpfApp3.View.S.Dialog;
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


namespace AquilaErpWpfApp3.ViewModel
{
    public sealed class S1148ViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private string title = "도면기준관리";

        private IList<SystemCodeVo> selectedMstList = new List<SystemCodeVo>();
        private S1148MasterDialog masterDialog;

        public S1148ViewModel()
        {
            Refresh();
        }

        [Command]
        public async void Refresh(string _SHP_CO_CD = null)
        {
            try
            {
                // CHNL_CD = SystemProperties.USER_VO.CHNL_CD
                // SystemCodeVo _param = new SystemCodeVo();

                // using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s1148", new StringContent(JsonConvert.SerializeObject(_param), System.Text.Encoding.UTF8, "application/json")))
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s1148", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectMasterList = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
              

                        if(selectedMstList.Count >= 1)
                        {
                            isM_UPDATE = true;
                            isM_DELETE = true;

                            if (_SHP_CO_CD == null)
                            {
                                SelectedMaster = selectedMstList[0];
                            }
                            else
                            {
                                SelectedMaster = selectedMstList.Where<SystemCodeVo>(x => x.SHP_CO_CD.Equals(_SHP_CO_CD)).FirstOrDefault<SystemCodeVo>();
                            }
                        }
                        else
                        {
                            isM_UPDATE = false;
                            isM_DELETE = false;
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

        // 도면업체 조회조건
        private string _M_SHP_CLSS_COL_NM = string.Empty;
        public string M_SHP_CLSS_COL_NM
        {
            get { return _M_SHP_CLSS_COL_NM; }
            set { SetProperty(ref _M_SHP_CLSS_COL_NM, value, () => M_SHP_CLSS_COL_NM); }
        }

        // 조회 리스트
        public IList<SystemCodeVo> SelectMasterList
        {
            get { return selectedMstList; }
            set { SetProperty(ref selectedMstList, value, () => SelectMasterList); }
        }

        SystemCodeVo _SelectedMaster; 
        public SystemCodeVo SelectedMaster
        {
            get { return _SelectedMaster; }
            set { SetProperty(ref _SelectedMaster, value, () => SelectedMaster); }
        }

        //public void RefreshCommand()
        //{
        //    Refresh();
        //}

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

        [Command]
        public async void DelContact()
        {
            SystemCodeVo delShp = this._SelectedMaster;
            if(delShp != null)
            {
                MessageBoxResult result = WinUIMessageBox.Show("[" + delShp.SHP_CO_CD + "]" + delShp.SHP_CLSS_COL_NM + " 정말로 삭제 하시겠습니까?", title, MessageBoxButton.YesNo, MessageBoxImage.Question);
                if(result == MessageBoxResult.Yes)
                {
                    try
                    {
                        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s1148/d", new StringContent(JsonConvert.SerializeObject(delShp), System.Text.Encoding.UTF8, "application/json")))
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

                                WinUIMessageBox.Show("삭제가 완료되었습니다.", title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                                this._SelectedMaster = null;
                                Refresh();
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

        [Command]
        public void EditContact()
        {
            if (this._SelectedMaster == null) return;
            SystemCodeVo editShp = this._SelectedMaster;
            ShowMasterDialog(editShp);
        }

        // 추가
        [Command]
        public void NewContact()
        {
            ShowMasterDialog(new SystemCodeVo());
        }

        public void ShowMasterDialog(SystemCodeVo Shp)
        {
            this.masterDialog = new S1148MasterDialog(Shp);
            this.masterDialog.Title = "도면기준";
            this.masterDialog.Owner = Application.Current.MainWindow;
            this.masterDialog.BorderEffect = BorderEffect.Default;
            this.masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            this.masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            bool isDialog = (bool)this.masterDialog.ShowDialog();
            if (isDialog)
            {
                Refresh(masterDialog.resultDao.SHP_CO_CD);



            }
        }


    }
}
