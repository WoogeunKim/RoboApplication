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
using System.Windows.Input;
using System.Windows.Media;
using AquilaErpWpfApp3.Util;
using System;
using ModelsLibrary.Code;
using System.Web;
using System.Net;
using System.IO;
using System.Text;
using System.Net.Http.Headers;

namespace AquilaErpWpfApp3.ViewModel
{
    public sealed class M66103ViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private string _title = "도면바리스트형상추출";


        public M66103ViewModel()
        {
            StartDt = System.DateTime.Now;
            EndDt = System.DateTime.Now;
            B_UPDATE = false;

            SYSTEM_CODE_VO();


        }

        [Command]
        public async void Refresh(string _num = null)
        {
            try
            {
                if (M_EXTR_STS_NM == null) return;

                ManVo mstRefVo = new ManVo()
                {
                    CHNL_CD = SystemProperties.USER_VO.CHNL_CD,
                    FM_DT = (StartDt).ToString("yyyy-MM-dd"),
                    TO_DT = (EndDt).ToString("yyyy-MM-dd"),
                    EXTR_STS_CD = M_EXTR_STS_NM.CLSS_CD
                };

                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m66103/", new StringContent(JsonConvert.SerializeObject(mstRefVo), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    { 
                        this.SelectMstList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();

                        if (SelectMstList.Count > 0)
                        {
                            if(_num == null)
                            {
                                SelectedMstItem = SelectMstList[0];
                            }
                            else
                            {
                                SelectedMstItem = SelectMstList.Where(x => x.RN.ToString().Equals(_num)).FirstOrDefault<ManVo>();
                            }

                            B_UPDATE = true;
                        }
                        else
                        {
                            this.SelectDtlList = new List<ManVo>();
                            SelectedMstItem = null;
                            B_UPDATE = false;
                        }
                    }
                }
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, _title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }

        [Command]
        public async void BarListPlay()
        {
            try
            {
                if (SelectedMstItem == null) return;

                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m66103/bar/count", new StringContent(JsonConvert.SerializeObject(SelectedMstItem), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        int barCount = JsonConvert.DeserializeObject<int>(await response.Content.ReadAsStringAsync());

                        // 수정하는 것
                        if (barCount > 0)
                        {
                            MessageBoxResult result = WinUIMessageBox.Show("재시작 하시겠습니까?", "바리스트 재시작", MessageBoxButton.YesNo, MessageBoxImage.Question);
                            if (result == MessageBoxResult.No)
                            {
                                return;
                            }

                            using (HttpResponseMessage DelResponse = await SystemProperties.PROGRAM_HTTP.PostAsync("m66103/bar/d", new StringContent(JsonConvert.SerializeObject(SelectedMstItem), System.Text.Encoding.UTF8, "application/json")))
                            {
                                if (DelResponse.IsSuccessStatusCode)
                                {
                                    int _Num = 0;
                                    string resultMsg = await DelResponse.Content.ReadAsStringAsync();
                                    if (int.TryParse(resultMsg, out _Num) == false)
                                    {
                                        //실패
                                        WinUIMessageBox.Show(resultMsg, _title, MessageBoxButton.OK, MessageBoxImage.Error);
                                        return;
                                    }
                                }
                            }
                        }

                        //if (DXSplashScreen.IsActive == false) DXSplashScreen.Show<ProgressWindow>();

                        HttpClient httpClient = new HttpClient();
                        httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                        string url = "http://210.217.42.139:8880/robocon/api/extract_barlist/v1?";
                        string value = "PUR_NO=" + SelectedMstItem.PUR_NO + "&" + "PUR_SEQ=" + SelectedMstItem.PUR_SEQ.ToString();

                        httpClient.GetAsync(url + value);

                        //using (var playResponse = await httpClient.GetAsync(url+value))
                        //{
                        //    if (HttpStatusCode.OK != playResponse.StatusCode)
                        //    {
                        //        if (DXSplashScreen.IsActive == true) DXSplashScreen.Close();
                        //        WinUIMessageBox.Show(playResponse.ReasonPhrase, _title, MessageBoxButton.OK, MessageBoxImage.Error);
                        //        return;
                        //    }
                        //}

                        //if (DXSplashScreen.IsActive == true) DXSplashScreen.Close();

                        Refresh(SelectedMstItem.RN.ToString());
                    }

                }
            }
            catch (System.Exception eLog)
            {  
                //if (DXSplashScreen.IsActive == true) DXSplashScreen.Close();

                WinUIMessageBox.Show(eLog.Message, _title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }

        [Command]
        public async void DtlRefresh()
        {
            try
            {
                if (SelectedMstItem == null) return;

                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m66103/dtl", new StringContent(JsonConvert.SerializeObject(SelectedMstItem), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectDtlList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
                    }
                }
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, _title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }

        public async void SYSTEM_CODE_VO()
        {
            try
            {
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "L-912"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        ExtrStsList = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();

                        if (ExtrStsList.Count > 0)
                        {
                            ExtrStsList.Insert(0, new SystemCodeVo() { CLSS_DESC = "전체" });

                            M_EXTR_STS_NM = ExtrStsList[0];
                        }
                    }
                }

                // Refresh();
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, _title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }


        private IList<ManVo> _selectMstList;
        public IList<ManVo> SelectMstList
        {
            get { return _selectMstList; }
            set { SetProperty(ref _selectMstList, value, () => SelectMstList); }
        }
        private ManVo _selectedMstItem;
        public ManVo SelectedMstItem
        {
            get { return _selectedMstItem; }
            set { SetProperty(ref _selectedMstItem, value, () => SelectedMstItem, DtlRefresh); }
        }

        private IList<ManVo> _selectDtlList;
        public IList<ManVo> SelectDtlList
        {
            get { return _selectDtlList; }
            set { SetProperty(ref _selectDtlList, value, () => SelectDtlList); }
        }
        //private ManVo _selectedDtlItem;
        //public ManVo SelectedDtlItem
        //{
        //    get { return _selectedDtlItem; }
        //    private set { SetProperty(ref _selectedDtlItem, value, () => SelectedDtlItem); }
        //}

        private IList<SystemCodeVo> _extrStsList;
        public IList<SystemCodeVo> ExtrStsList
        {
            get { return _extrStsList; }
            set { SetProperty(ref _extrStsList, value, () => ExtrStsList); }
        }

        private SystemCodeVo _M_EXTR_STS_NM;
        public SystemCodeVo M_EXTR_STS_NM
        {
            get { return _M_EXTR_STS_NM; }
            set { SetProperty(ref _M_EXTR_STS_NM, value, () => M_EXTR_STS_NM); }
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

        bool _B_UPDATE;
        public bool B_UPDATE
        {
            get { return _B_UPDATE; }
            set { SetProperty(ref _B_UPDATE, value, () => B_UPDATE); }
        }

    }
}
