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
    public sealed class I66201ViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private string title = "재고장";


        private IList<InvVo> selectedMstList = new List<InvVo>();

        public I66201ViewModel() 
        {
            StartDt = System.DateTime.Now;

            SYSTEM_CODE_VO();

        }

        [Command]
        public async void Refresh()
        {
            try
            {
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("i66201/mst", new StringContent(JsonConvert.SerializeObject(new InvVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD, YRMON = (StartDt).ToString("yyyyMM"), AREA_CD = M_SL_AREA_NM.CLSS_CD}), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectMstList = JsonConvert.DeserializeObject<IEnumerable<InvVo>>(await response.Content.ReadAsStringAsync()).Cast<InvVo>().ToList();

                        Title = "[기간]" + (StartDt).ToString("yyyy-MM-dd") + ",   [사업장]" + M_SL_AREA_NM.CLSS_DESC;

                        if (SelectMstList.Count >= 1)
                        {
                            isM_UPDATE = true;
                            isM_DELETE = true;

                            SelectedMstItem = SelectMstList[0];
                        }
                        else
                        {
                            isM_UPDATE = false;
                            isM_DELETE = false;
                            isD_UPDATE = false;
                            isD_DELETE = false;
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



        //[Command]
        //public async void Report()
        //{

        //    IList<InvVo> _reportList = new List<InvVo>();

        //    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("i66201/mst", new StringContent(JsonConvert.SerializeObject(new InvVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD, AREA_CD = SystemProperties.USER_VO.EMPE_PLC_NM }), System.Text.Encoding.UTF8, "application/json")))
        //    {
        //        if (response.IsSuccessStatusCode)
        //        {
        //            _reportList = JsonConvert.DeserializeObject<IEnumerable<InvVo>>(await response.Content.ReadAsStringAsync()).Cast<InvVo>().ToList();
        //        }


        //        // 재고조사용
        //        I66201Report report = new I66201Report(_reportList);
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



        DateTime _startDt;
        public DateTime StartDt
        {
            get { return _startDt; }
            set { SetProperty(ref _startDt, value, () => StartDt); }
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




        string _Title = string.Empty;
        public string Title
        {
            get { return _Title; }
            set { SetProperty(ref _Title, value, () => Title); }
        }


        


        public async void SYSTEM_CODE_VO()
        {
            //사업장
            //AreaList = SystemProperties.SYSTEM_CODE_VO("L-002");
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "L-002"))
            {
                if (response.IsSuccessStatusCode)
                {
                    AreaList = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                    if (AreaList.Count > 0)
                    {
                        M_SL_AREA_NM = AreaList[0];
                    }
                }
            }


        }


        private IList<SystemCodeVo> _AreaCd = new List<SystemCodeVo>();
        public IList<SystemCodeVo> AreaList
        {
            get { return _AreaCd; }
            set { SetProperty(ref _AreaCd, value, () => AreaList); }
        }

        private SystemCodeVo _M_SL_AREA_NM;
        public SystemCodeVo M_SL_AREA_NM
        {
            get { return _M_SL_AREA_NM; }
            set { SetProperty(ref _M_SL_AREA_NM, value, () => M_SL_AREA_NM); }
        }
        public IList<InvVo> SelectMstList
        {
            get { return selectedMstList; }
            set { SetProperty(ref selectedMstList, value, () => SelectMstList); }
        }

        InvVo _selectedMstItem;
        public InvVo SelectedMstItem
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
    }
}
