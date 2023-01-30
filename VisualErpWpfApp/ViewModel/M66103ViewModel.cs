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

namespace AquilaErpWpfApp3.ViewModel
{
    public sealed class M66103ViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private string _title = "도면바리스트형상추출";


        public M66103ViewModel()
        {
            StartDt = System.DateTime.Now;
            EndDt = System.DateTime.Now;


            SYSTEM_CODE_VO();
        }

        [Command]
        public async void Refresh()
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
                            SelectedMstItem = SelectMstList[0];
                        }
                        else
                        {
                            this.SelectDtlList = new List<ManVo>();
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
        public async void DtlRefresh()
        {
            try
            {
                if (SelectedMstItem == null) return;

                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m66103/dtl", new StringContent(JsonConvert.SerializeObject(SelectedMstItem), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectDtlList = new List<ManVo>();
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
    }
}
