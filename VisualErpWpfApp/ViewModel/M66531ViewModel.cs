using AquilaErpWpfApp3.Util;
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

namespace AquilaErpWpfApp3.ViewModel
{
    public sealed class M66531ViewModel : ViewModelBase, INotifyPropertyChanged
    {

        private string title = "계획대비 실적 리포트(포장)";

        private IList<ManVo> selectedMstList = new List<ManVo>();

        public M66531ViewModel()
        {
            StartDt = System.DateTime.Now.AddDays(0);
            EndDt = System.DateTime.Now;
        }




        [Command]
        public async void Refresh()
        {
            try
            {
                //DXSplashScreen.Show<ProgressWindow>();

                ManVo _param = new ManVo();
                _param.FM_DT = (StartDt).ToString("yyyy-MM-dd");
                _param.TO_DT = (EndDt).ToString("yyyy-MM-dd");
                _param.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;

                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m66531/mst", new StringContent(JsonConvert.SerializeObject(_param), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectMstList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
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




        #region  그리드 선택값, 그리드 전체값 프로퍼티 선언

        // 그리드 선택값, 그리드 전체값 프로퍼티 선언
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
                if (value != null)
                {
                    SetProperty(ref _selectedMstItem, value, () => SelectedMstItem);
                }
            }
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
        #endregion


    }
}
