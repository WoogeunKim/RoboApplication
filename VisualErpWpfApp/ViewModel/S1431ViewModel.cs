using AquilaErpWpfApp3.Util;
using AquilaErpWpfApp3.View.S.Dialog;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Code;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Media;

namespace AquilaErpWpfApp3.ViewModel
{
    public class S1431ViewModel : ViewModelBase
    {
        private string _title = "차량 마스터 등록";
        private IList<SystemCodeVo> selectedMenuViewList;

        private S1431MasterDialog masterDialog;

        public S1431ViewModel()
        {
            Refresh();
        }

        #region 조회
        [Command]
        public async void Refresh(string _CAR_NO = null)
        {
            try
            {
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s1431/mst/s", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectedMenuViewList = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();

                        if (SelectedMenuViewList.Count > 0)
                        {
                            isM_UPDATE = true;
                            if (string.IsNullOrEmpty(_CAR_NO))
                            {
                                SelectedMenuItem = SelectedMenuViewList[0];
                            }
                            else
                            {
                                SelectedMenuItem = SelectedMenuViewList.Where(x => x.CAR_NO.Equals(_CAR_NO)).LastOrDefault<SystemCodeVo>(); // 확인
                            }
                        }
                        else
                        {
                            isM_UPDATE = false;
                            SelectedMenuItem = null;
                        }
                    }
                }
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
            }
        }
        #endregion


        #region 함수 메뉴
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
                }
            }
        }

        private async void SearchDetailItem()
        {
            if (this._selectMenuItem == null)
            {
                return;
            }
            _selectMenuItem.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s1431/dtl/s", new StringContent(JsonConvert.SerializeObject(_selectMenuItem), System.Text.Encoding.UTF8, "application/json")))
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

        private bool? _isM_UPDATE = false;
        public bool? isM_UPDATE
        {
            get { return _isM_UPDATE; }
            set { SetProperty(ref _isM_UPDATE, value, () => isM_UPDATE); }
        }

        private string _M_SEARCH_TEXT = string.Empty;
        public string M_SEARCH_TEXT
        {
            get { return _M_SEARCH_TEXT; }
            set { SetProperty(ref _M_SEARCH_TEXT, value, () => M_SEARCH_TEXT); }
        }
        #endregion

        public void ShowMasterDialog(SystemCodeVo vo)
        {
            masterDialog = new S1431MasterDialog(vo);
            masterDialog.Title = _title;
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
        
        [Command]
        public void NewContact()
        {
            ShowMasterDialog(new SystemCodeVo());
        }

        [Command]
        public void EditContact()
        {
            if(SearchItem == null) { return; }
            ShowMasterDialog(SearchItem);
        }




    }
}