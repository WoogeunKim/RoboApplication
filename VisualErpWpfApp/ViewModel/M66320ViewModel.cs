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
using AquilaErpWpfApp3.View.M.Dialog;
using AquilaErpWpfApp3.M.View.Dialog;

namespace AquilaErpWpfApp3.ViewModel
{
    public sealed class M66320ViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private string _title = "투입자재지시";



        public M66320ViewModel()
        {

            StartDt = System.DateTime.Now;
            EndDt = System.DateTime.Now;
            B_UPDATE = false;
            SYSTEM_CODE_VO();

            BL_CLZ_FLG = false;
        }



        [Command]
        public async void Refresh()
        {
            try
            {


                if (DXSplashScreen.IsActive == false) DXSplashScreen.Show<ProgressWindow>();

                this.SelectMstList = null;
                this.SelectedMstItem = null;
                this.SelectDtlList = null;
                this.SelectedDtlItem = null;
                this.SummaryTableList = null;



                IList<ManVo> MstList = new List<ManVo>();
                IList<ManVo> SummaryList = new List<ManVo>();


                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m66320", new StringContent(JsonConvert.SerializeObject(new ManVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD, N1ST_EQ_NO = M_EQ_NO.PROD_EQ_NO, FM_DT = (StartDt).ToString("yyyy-MM-dd"), TO_DT = (EndDt).ToString("yyyy-MM-dd"), CLZ_FLG = BL_CLZ_FLG == true ? "Y" : "N" }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        MstList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
                    }
                }
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m66320/dtl/summary", new StringContent(JsonConvert.SerializeObject(new ManVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD, N1ST_EQ_NO = M_EQ_NO.PROD_EQ_NO, FM_DT = (StartDt).ToString("yyyy-MM-dd"), TO_DT = (EndDt).ToString("yyyy-MM-dd"), CLZ_FLG = BL_CLZ_FLG == true ? "Y" : "N" }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        SummaryList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
                    }
                }
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m66320/dtl", new StringContent(JsonConvert.SerializeObject(new ManVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD, N1ST_EQ_NO = M_EQ_NO.PROD_EQ_NO, FM_DT = (StartDt).ToString("yyyy-MM-dd"), TO_DT = (EndDt).ToString("yyyy-MM-dd"), CLZ_FLG = BL_CLZ_FLG == true ? "Y" : "N" }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        // DTL 조회하는데 오래걸려서 같이 보이도록 조절하였음.
                        this.SelectMstList = MstList;
                        this.SummaryTableList = SummaryList;

                        this.SelectDtlList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();

                        B_UPDATE = true;
                    }
                }

                if (DXSplashScreen.IsActive == true) DXSplashScreen.Close();
            }
            catch (System.Exception eLog)
            {
                if (DXSplashScreen.IsActive == true) DXSplashScreen.Close();

                WinUIMessageBox.Show(eLog.Message, _title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }



        private async void SYSTEM_CODE_VO()
        {
            try
            {
                // 절단설비
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("M66311/n1st/eq", new StringContent(JsonConvert.SerializeObject(new ManVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectN1stList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();

                        M_EQ_NO = SelectN1stList[0];
                    }
                }
                Refresh();


            }
            catch (Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
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

        private string _opmzNo = default(string);
        public string OpmzNo
        {
            get { return _opmzNo; }
            set { SetProperty(ref _opmzNo, value, () => OpmzNo); }
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
            set { SetProperty(ref _selectedMstItem, value, () => SelectedMstItem); }
        }

        private IList<ManVo> _selectDtlList;
        public IList<ManVo> SelectDtlList
        {
            get { return _selectDtlList; }
            set { SetProperty(ref _selectDtlList, value, () => SelectDtlList); }
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

        private ManVo _selectedDtlItem;
        public ManVo SelectedDtlItem
        {
            get { return _selectedDtlItem; }
            set { SetProperty(ref _selectedDtlItem, value, () => SelectedDtlItem, InputValue); }
        }


        private IList<ManVo> _summaryTableList;
        public IList<ManVo> SummaryTableList
        {
            get { return _summaryTableList; }
            set { SetProperty(ref _summaryTableList, value, () => SummaryTableList); }
        }

        //private IList<SystemCodeVo> _extrStsList;
        //public IList<SystemCodeVo> ExtrStsList
        //{
        //    get { return _extrStsList; }
        //    set { SetProperty(ref _extrStsList, value, () => ExtrStsList); }
        //}

        //private SystemCodeVo _M_EXTR_STS_NM;
        //public SystemCodeVo M_EXTR_STS_NM
        //{
        //    get { return _M_EXTR_STS_NM; }
        //    set { SetProperty(ref _M_EXTR_STS_NM, value, () => M_EXTR_STS_NM); }
        //}

        bool _B_UPDATE;
        public bool B_UPDATE
        {
            get { return _B_UPDATE; }
            set { SetProperty(ref _B_UPDATE, value, () => B_UPDATE); }
        }

        bool _B_INSERT;
        public bool B_INSERT
        {
            get { return _B_INSERT; }
            set { SetProperty(ref _B_INSERT, value, () => B_INSERT); }
        }

        private void InputValue()
        {
            B_INSERT = SelectedDtlItem != null ? true : false;
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