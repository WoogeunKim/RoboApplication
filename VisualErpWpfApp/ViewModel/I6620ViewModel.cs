using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.Printing;
using DevExpress.Xpf.WindowsUI;
using DevExpress.XtraPrinting.Drawing;
using ModelsLibrary.Code;
using ModelsLibrary.Inv;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Windows;
using AquilaErpWpfApp3.Util;
using AquilaErpWpfApp3.View.INV.Report;
using DevExpress.Xpf.Core;

namespace AquilaErpWpfApp3.ViewModel
{
    public sealed class I6620ViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private string title = "재고장";

        private IList<InvVo> selectedMstList = new List<InvVo>();

        private IList<InvVo> selectedDtlList = new List<InvVo>();

        public I6620ViewModel()
        {
            SYSTEM_CODE_VO();
        }

        [Command]
        public async void Refresh()
        {
            try
            {
                SearchDetail = null;
                SelectDtlList = null;

                DXSplashScreen.Show<ProgressWindow>();

                Title = "[구분]" + TXT_ITM_GRP_CLSS_NM.CLSS_DESC;

                InvVo _vo = new InvVo()
                {
                    ITM_GRP_CLSS_CD = TXT_ITM_GRP_CLSS_NM.CLSS_CD,
                    CHNL_CD = SystemProperties.USER_VO.CHNL_CD
                };

                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("i6620/dtl", new StringContent(JsonConvert.SerializeObject(_vo), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        SelectDtlList = JsonConvert.DeserializeObject<IEnumerable<InvVo>>(await response.Content.ReadAsStringAsync()).Cast<InvVo>().ToList();
                    }
                    //// //
                    if (SelectDtlList.Count >= 1)
                    {
                        isD_UPDATE = true;
                        isD_DELETE = true;

                        SearchDetail = SelectDtlList[0];
                    }
                    else
                    {
                        isD_UPDATE = false;
                        isD_DELETE = false;
                    }

                    DXSplashScreen.Close();
                }
            }
            catch (System.Exception eLog)
            {
                if (DXSplashScreen.IsActive == true)
                {
                    DXSplashScreen.Close();
                }
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

        private string _M_DEPT_DESC;
        public string M_DEPT_DESC
        {
            get { return _M_DEPT_DESC; }
            set { SetProperty(ref _M_DEPT_DESC, value, () => M_DEPT_DESC); }
        }


        private string _M_SEARCH_TEXT = string.Empty;
        public string M_SEARCH_TEXT
        {
            get { return _M_SEARCH_TEXT; }
            set { SetProperty(ref _M_SEARCH_TEXT, value, () => M_SEARCH_TEXT); }
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

        //
        private bool? _isD_UPDATE = false;
        public bool? isD_UPDATE
        {
            get { return _isD_UPDATE; }
            set { SetProperty(ref _isD_UPDATE, value, () => isD_UPDATE); }
        }

        private bool? _isD_DELETE = false;
        public bool? isD_DELETE
        {
            get { return _isD_DELETE; }
            set { SetProperty(ref _isD_DELETE, value, () => isD_DELETE); }
        }

        //구분
        private IList<SystemCodeVo> _ItmGrpClssList = new List<SystemCodeVo>();
        public IList<SystemCodeVo> ItmGrpClssList
        {
            get { return _ItmGrpClssList; }
            set { SetProperty(ref _ItmGrpClssList, value, () => ItmGrpClssList); }
        }

        //구분
        private SystemCodeVo _TXT_ITM_GRP_CLSS_NM;
        public SystemCodeVo TXT_ITM_GRP_CLSS_NM
        {
            get { return _TXT_ITM_GRP_CLSS_NM; }
            set { SetProperty(ref _TXT_ITM_GRP_CLSS_NM, value, () => TXT_ITM_GRP_CLSS_NM); }
        }

        //호칭
        private IList<SystemCodeVo> _HdList = new List<SystemCodeVo>();
        public IList<SystemCodeVo> HdList
        {
            get { return _HdList; }
            set { SetProperty(ref _HdList, value, () => HdList); }
        }

        //호칭
        private SystemCodeVo _TXT_HD_LEN_NM;
        public SystemCodeVo TXT_HD_LEN_NM
        {
            get { return _TXT_HD_LEN_NM; }
            set { SetProperty(ref _TXT_HD_LEN_NM, value, () => TXT_HD_LEN_NM); }
        }

        public void EditContact()
        {

        }



        private async void SelectMstDetail()
        {
            try
            {
                //if (SelectedMstItem == null)
                //{
                //    return;
                //}

                InvVo _vo = new InvVo()
                {
                    ITM_GRP_CLSS_CD = TXT_ITM_GRP_CLSS_NM.CLSS_CD,
                    //N1ST_ITM_GRP_CD = SelectedMstItem.N1ST_ITM_GRP_CD,
                    //N2ND_ITM_GRP_CD = SelectedMstItem.N2ND_ITM_GRP_CD,
                    //HD_LEN_CD = TXT_HD_LEN_NM.CLSS_CD,
                    AREA_CD = SystemProperties.USER_VO.EMPE_PLC_NM,
                    CHNL_CD = SystemProperties.USER_VO.CHNL_CD
                    //SL_RLSE_DT = (StartDt).ToString("yyyy-MM-dd")
                };

                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("i6620/dtl", new StringContent(JsonConvert.SerializeObject(_vo), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        SelectDtlList = JsonConvert.DeserializeObject<IEnumerable<InvVo>>(await response.Content.ReadAsStringAsync()).Cast<InvVo>().ToList();
                    }

                    if (SelectDtlList.Count >= 1)
                    {
                        isD_UPDATE = true;
                        isD_DELETE = true;

                        SearchDetail = SelectDtlList[0];
                    }
                    else
                    {
                        isD_UPDATE = false;
                        isD_DELETE = false;
                    }

                }
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }
        //#endregion


        //#region Functon <Detail List>
        public IList<InvVo> SelectDtlList
        {
            get { return selectedDtlList; }
            set { SetProperty(ref selectedDtlList, value, () => SelectDtlList); }
        }

        InvVo _searchDetail;
        public InvVo SearchDetail
        {
            get
            {
                return _searchDetail;
            }
            set
            {
                if (value != null)
                {
                    SetProperty(ref _searchDetail, value, () => SearchDetail);
                }
            }
        }
        //#endregion


        string _Title = string.Empty;
        public string Title
        {
            get { return _Title; }
            set { SetProperty(ref _Title, value, () => Title); }
        }

        //[Command]
        //public async void Report()
        //{
        //    IList<InvVo> _reportList = new List<InvVo>();

        //    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("i5521/report", new StringContent(JsonConvert.SerializeObject(new InvVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD, AREA_CD = SystemProperties.USER_VO.EMPE_PLC_NM, ITM_GRP_CLSS_CD = TXT_ITM_GRP_CLSS_NM.CLSS_CD, /*N1ST_ITM_GRP_CD = SelectedMstItem.N1ST_ITM_GRP_CD, N2ND_ITM_GRP_CD = SelectedMstItem.N2ND_ITM_GRP_CD*/ }), System.Text.Encoding.UTF8, "application/json")))
        //    {
        //        if (response.IsSuccessStatusCode)
        //        {
        //            _reportList = JsonConvert.DeserializeObject<IEnumerable<InvVo>>(await response.Content.ReadAsStringAsync()).Cast<InvVo>().ToList();
        //        }

        //        // 재고조사용
        //        I5521Report report = new I5521Report(_reportList);
        //        report.Margins.Top = 2;
        //        report.Margins.Bottom = 0;
        //        report.Margins.Left = 1;
        //        report.Margins.Right = 1;
        //        report.Landscape = true;
        //        report.PrintingSystem.ShowPrintStatusDialog = true;
        //        report.PaperKind = System.Drawing.Printing.PaperKind.A4;
        //        //데모 시연 문서 표시 가능
        //        //report.Watermark.Text = "한양화스너공업(주)";
        //        report.Watermark.TextDirection = DirectionMode.ForwardDiagonal;
        //        report.Watermark.Font = new Font(report.Watermark.Font.FontFamily, 40);
        //        ////report.Watermark.ForeColor = Color.DodgerBlue;
        //        report.Watermark.ForeColor = System.Drawing.Color.PaleTurquoise;
        //        report.Watermark.TextTransparency = 180;
        //        report.Watermark.ShowBehind = false;
        //        //report.Watermark.PageRange = "1,3-5";

        //        var window = new DocumentPreviewWindow();
        //        window.PreviewControl.DocumentSource = report;
        //        report.CreateDocument(true);
        //        window.Title = "연말 재고조사용";
        //        window.Owner = Application.Current.MainWindow;
        //        window.ShowDialog();
        //    }

        //}


        public async void SYSTEM_CODE_VO()
        {
            //구분
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "L-001"))
            {
                if (response.IsSuccessStatusCode)
                {
                    ItmGrpClssList = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                    if (ItmGrpClssList.Count > 0)
                    {
                        ItmGrpClssList.Insert(0, new SystemCodeVo() { CLSS_CD = null, CLSS_DESC = "전체" });
                        TXT_ITM_GRP_CLSS_NM = ItmGrpClssList[1];
                    }
                }
            }

            //호칭
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "C-001"))
            {
                if (response.IsSuccessStatusCode)
                {
                    HdList = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                    if (HdList.Count > 0)
                    {
                        HdList.Insert(0, new SystemCodeVo() { CLSS_CD = null, CLSS_DESC = "전체" });
                        TXT_HD_LEN_NM = HdList[0];
                    }
                }
            }

        }
    }
}