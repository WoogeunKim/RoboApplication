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
using AquilaErpWpfApp3.View.M.Dialog;
using AquilaErpWpfApp3.M.View.Dialog;
using DevExpress.Xpf.Core;
using System.Windows.Media;

namespace AquilaErpWpfApp3.ViewModel
{
    public sealed class M66321ViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private string _title = "가공지시";
        private IList<ManVo> selectedMstList = new List<ManVo>();

        public M66321ViewModel()
        {
            StartDt = System.DateTime.Now;
            EndDt = System.DateTime.Now;

            SYSTEM_CODE_VO();



            BL_CLZ_FLG = false;
        }

        // Master
        [Command]
        public async void Refresh()
        {
            try
            {
                if (M_EQ_NO == null) return;

                if (DXSplashScreen.IsActive == false) DXSplashScreen.Show<ProgressWindow>();

                ManVo _param = new ManVo();
                _param.N1ST_EQ_NO = M_EQ_NO.PROD_EQ_NO;
                _param.FM_DT = (StartDt).ToString("yyyy-MM-dd");
                _param.TO_DT = (EndDt).ToString("yyyy-MM-dd");
                _param.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
                _param.CLZ_FLG = BL_CLZ_FLG == true ? "Y" : "N";

                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("M66321/mst", new StringContent(JsonConvert.SerializeObject(_param), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectMstList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();

                    }
                }

                if (DXSplashScreen.IsActive == true) DXSplashScreen.Close();
            }
            catch (System.Exception eLog)
            {
                if (DXSplashScreen.IsActive == true) DXSplashScreen.Close();

                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }




        private async void SYSTEM_CODE_VO()
        {
            try
            {
                // 절단설비
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("M66311/n1st/eq"
                                                                                                   , new StringContent(JsonConvert.SerializeObject(new ManVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectN1stList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
                        M_EQ_NO = SelectN1stList[0];
                    }
                }

                //// 가공설비
                //using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("M66311/n2nd/eq"
                //                                                                                   , new StringContent(JsonConvert.SerializeObject(new ManVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                //{
                //    if (response.IsSuccessStatusCode)
                //    {
                //        this.SelectN2ndList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();

                //    }

                //}
            }
            catch (Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }









        // MVVM 방식으로 화면과 바이딩을 합니다.
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
            get { return _selectedMstItem; }
            set { SetProperty(ref _selectedMstItem, value, () => SelectedMstItem); }
        }

        private IList<ManVo> _selectN1stList = new List<ManVo>();
        public IList<ManVo> SelectN1stList
        {
            get { return _selectN1stList; }
            set { SetProperty(ref _selectN1stList, value, () => SelectN1stList); }
        }

        private ManVo _M_EQ_NO;
        public ManVo M_EQ_NO
        {
            get { return _M_EQ_NO; }
            set { SetProperty(ref _M_EQ_NO, value, () => M_EQ_NO); }
        }


        string _txtRefreshOptmNm = string.Empty;
        public string TxtRefreshOptmNm
        {
            get { return _txtRefreshOptmNm; }
            set { SetProperty(ref _txtRefreshOptmNm, value, () => TxtRefreshOptmNm); }
        }

        string _Title = string.Empty;
        public string Title
        {
            get { return _Title; }
            set { SetProperty(ref _Title, value, () => Title); }
        }


        ManVo _optiVo;
        public ManVo OptiVo
        {
            get { return _optiVo; }
            set { SetProperty(ref _optiVo, value, () => OptiVo); }
        }

        bool blClzFlg = false;
        public bool BL_CLZ_FLG
        {
            get { return blClzFlg; }
            set { SetProperty(ref blClzFlg, value, () => BL_CLZ_FLG, Refresh); }
        }
        String eqno = string.Empty;
        public String EQ_NO
        {
            get { return eqno; }
            set { SetProperty(ref eqno, value, () => EQ_NO); }
        }
    }
}
