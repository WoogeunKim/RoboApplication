using AquilaErpWpfApp3.Util;
using AquilaErpWpfApp3.View.M.Dialog;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.WindowsUI;
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
    public sealed class M66521ViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private IList<ManVo> selectedMstList = new List<ManVo>();

        private string title = "계획대비 실적 리포트(충전)";
        
        #region 변수 선언 부
        // 날짜기능
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


        //검색기능

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



        ManVo _selectMasterItem;
        public ManVo SelectedMasterItem
        {
            get
            {
                return _selectMasterItem;
            }
            set
            {
                SetProperty(ref _selectMasterItem, value, () => SelectedMasterItem);
            }
        }



        string _Title = string.Empty;
        public string Title
        {
            get { return _Title; }
            set { SetProperty(ref _Title, value, () => Title); }
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
        #endregion

        public M66521ViewModel()
        {
            StartDt = System.DateTime.Now;
            EndDt = System.DateTime.Now;
        }


        #region 기능 선언 부
        [Command]
        public async void Refresh()
        {
            try
            {
                //DXSplashScreen.Show<ProgressWindow>();

                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m66521/mst", new StringContent(JsonConvert.SerializeObject(new ManVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD, FM_DT = StartDt.ToString("yyyy-MM-dd"), TO_DT = EndDt.ToString("yyyy-MM-dd") }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectMstList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
                    }
                    //Title = "[기간]" + (StartDt).ToString("yyyy-MM-dd") + "~" + (EndDt).ToString("yyyy-MM-dd");
                    if (SelectMstList.Count > 0)
                    {
                        isM_UPDATE = true;
                        isM_DELETE = true;

                        SelectedMasterItem = SelectMstList[0];
                    }

                    else
                    {
                        isM_UPDATE = false;
                        isM_DELETE = false;

                    }

                }
                //DXSplashScreen.Close();
            }
            catch (System.Exception eLog)
            {
                //if (DXSplashScreen.IsActive == true)
                //{
                //    DXSplashScreen.Close();
                //}
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }









        #endregion


    }
}
