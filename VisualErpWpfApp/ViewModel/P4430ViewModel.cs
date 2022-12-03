using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Printing;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Code;
using ModelsLibrary.Pur;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Media;
using AquilaErpWpfApp3.Util;
using AquilaErpWpfApp3.View.PUR.Dialog;
using AquilaErpWpfApp3.View.PUR.Report;

namespace AquilaErpWpfApp3.ViewModel
{
    public sealed class P4430ViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private string _title = "발주서 관리";

        private IList<PurVo> selectedMstList = new List<PurVo>();

        private IList<PurVo> selectedDtlList = new List<PurVo>();


        private IList<SystemCodeVo> selectCustomerList = new List<SystemCodeVo>();

        private P4411MasterDialog masterDialog;
        private P4411Detail_1Dialog detailDialog_1;
        private P4430InsertDtlDialog detailDialog_2;
        private P4411Detail_3Dialog detailDialog_3;
        private P4411EmailDialog emailDialog;

        public P4430ViewModel()
        {

            WeekDt = System.DateTime.Now;

            StartDt = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy-MM-01"));
            EndDt = System.DateTime.Now;

            SYSTEM_CODE_VO();
        }
        [Command]
        public async void Refresh(string _PUR_ORD_NO = null)
        {
            try
            {
                SearchDetail = null;
                SelectDtlList = null;
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("p4411/mst", new StringContent(JsonConvert.SerializeObject(new PurVo() { FM_DT = (StartDt).ToString("yyyy-MM-dd"), TO_DT = (EndDt).ToString("yyyy-MM-dd"), AREA_CD = M_SL_AREA_NM.CLSS_CD, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectMstList = JsonConvert.DeserializeObject<IEnumerable<PurVo>>(await response.Content.ReadAsStringAsync()).Cast<PurVo>().ToList();
                    }

                    Title = "[기간]" + (StartDt).ToString("yyyy-MM-dd") + "~" + (EndDt).ToString("yyyy-MM-dd") + ",   [사업장]" + M_SL_AREA_NM.CLSS_DESC;

                    if (SelectMstList.Count >= 1)
                    {
                        isM_UPDATE = true;
                        isM_DELETE = true;

                        if (string.IsNullOrEmpty(_PUR_ORD_NO))
                        {
                            SelectedMstItem = SelectMstList[0];
                        }
                        else
                        {
                            SelectedMstItem = SelectMstList.Where(x => x.PUR_ORD_NO.Equals(_PUR_ORD_NO)).LastOrDefault<PurVo>();
                        }
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
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
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

        DateTime _weekDt;
        public DateTime WeekDt
        {
            get { return _weekDt; }
            set { SetProperty(ref _weekDt, value, () => WeekDt); }
        }

        private string _M_SEARCH_TEXT = string.Empty;
        public string M_SEARCH_TEXT
        {
            get { return _M_SEARCH_TEXT; }
            set { SetProperty(ref _M_SEARCH_TEXT, value, () => M_SEARCH_TEXT); }
        }

        private Dictionary<string, string> _AreaMap = new Dictionary<string, string>();

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
        public IList<PurVo> SelectMstList
        {
            get { return selectedMstList; }
            set { SetProperty(ref selectedMstList, value, () => SelectMstList); }
        }

        PurVo _selectedMstItem;
        public PurVo SelectedMstItem
        {
            get
            {
                return _selectedMstItem;
            }
            set
            {
                if (value != null)
                {
                    SetProperty(ref _selectedMstItem, value, () => SelectedMstItem, SelectMstDetail);
                }
            }
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

        [Command]
        public async void SelectMstDetail()
        {
            try
            {

                if (this._selectedMstItem == null)
                {
                    return;
                }

                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("p4430/dtl", new StringContent(JsonConvert.SerializeObject(SelectedMstItem), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectDtlList = JsonConvert.DeserializeObject<IEnumerable<PurVo>>(await response.Content.ReadAsStringAsync()).Cast<PurVo>().ToList();
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
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }
        public IList<PurVo> SelectDtlList
        {
            get { return selectedDtlList; }
            set { SetProperty(ref selectedDtlList, value, () => SelectDtlList); }
        }

        PurVo _searchDetail;
        public PurVo SearchDetail
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

        string _Title = string.Empty;
        public string Title
        {
            get { return _Title; }
            set { SetProperty(ref _Title, value, () => Title); }
        }

        [Command]
        public void NewContact()
        {
            masterDialog = new P4411MasterDialog(new PurVo() { AREA_CD = M_SL_AREA_NM.CLSS_CD, AREA_NM = M_SL_AREA_NM.CLSS_DESC, PUR_DT = System.DateTime.Now.ToString("yyyy-MM-dd"), PUR_CLZ_FLG = "N", PUR_EMPE_ID = SystemProperties.USER_VO.USR_ID });
            masterDialog.Title = _title + " - 추가";
            masterDialog.Owner = Application.Current.MainWindow;
            masterDialog.BorderEffect = BorderEffect.Default;
            masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            bool isDialog = (bool)masterDialog.ShowDialog();
            if (isDialog)
            {
                Refresh(masterDialog.PUR_ORD_NO);
            }
        }

        [Command]
        public void EditContact()
        {
            if (SelectedMstItem == null)
            {
                return;
            }

            masterDialog = new P4411MasterDialog(SelectedMstItem);
            masterDialog.Title = _title + " - 수정";
            masterDialog.Owner = Application.Current.MainWindow;
            masterDialog.BorderEffect = BorderEffect.Default;
            masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            bool isDialog = (bool)masterDialog.ShowDialog();
            if (isDialog)
            {
                Refresh(masterDialog.PUR_ORD_NO);
            }
        }

        [Command]
        public async void DelContact()
        {
            if (SelectedMstItem == null)
            {
                return;
            }
            PurVo delDao = SelectedMstItem;
            if (delDao != null)
            {
                MessageBoxResult result = WinUIMessageBox.Show("[" + delDao.PUR_ORD_NO + "]" + " 정말로 삭제 하시겠습니까?", "[삭제]" + _title, MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    SelectedMstItem.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("p4411/mst/d", new StringContent(JsonConvert.SerializeObject(SelectedMstItem), System.Text.Encoding.UTF8, "application/json")))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            int _Num = 0;
                            string resultMsg = await response.Content.ReadAsStringAsync();
                            if (int.TryParse(resultMsg, out _Num) == false)
                            {

                                WinUIMessageBox.Show(resultMsg, _title, MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                            Refresh();
                            WinUIMessageBox.Show("삭제가 완료되었습니다.", _title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                        }
                    }
                }
            }
        }



        [Command]
        public async void PrintWeekContact()
        {
            detailDialog_3 = new P4411Detail_3Dialog(new PurVo() { });
            detailDialog_3.Title = "발주 자재 관리 → " + (WeekDt).ToString("yyyy년 MM월") + " 예 상 발 주 서";
            detailDialog_3.Owner = Application.Current.MainWindow;
            detailDialog_3.BorderEffect = BorderEffect.Default;
            detailDialog_3.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            detailDialog_3.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            bool isDialog = (bool)detailDialog_3.ShowDialog();
            if (isDialog)
            {
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("p4411/report/weekly", new StringContent(JsonConvert.SerializeObject(new PurVo() { FM_DT = (WeekDt).ToString("yyyyMM"), TO_DT = (WeekDt).ToString("yyyyMM"), A_PUR_CO_CD = detailDialog_3.aCoCdItems, AREA_CD = M_SL_AREA_NM.CLSS_CD, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        IList<PurVo> allItems = JsonConvert.DeserializeObject<IEnumerable<PurVo>>(await response.Content.ReadAsStringAsync()).Cast<PurVo>().ToList();
                        if (allItems == null)
                        {
                            return;
                        }
                        else if (allItems.Count <= 0)
                        {
                            WinUIMessageBox.Show("데이터가 존재 하지 않습니다.", "[에러]" + _title, MessageBoxButton.OK, MessageBoxImage.Information);
                            return;
                        }
                        else if (allItems.Count <= 15)
                        {
                            int nCnt = (15 - allItems.Count);
                            for (int x = 1; x <= nCnt; x++)
                            {
                                allItems.Insert(allItems.Count, new PurVo());
                            }
                        }
                        else
                        {
                            int nCnt = (15 - (allItems.Count % 15));
                            for (int x = 1; x <= nCnt; x++)
                            {
                                allItems.Insert(allItems.Count, new PurVo());
                            }
                        }

                        allItems[0].GBN = (WeekDt).ToString("yyyy년 MM월") + " 발 주 서";
                        allItems[0].Message = string.Join(" & ", detailDialog_3.aCoNmItems);
                        allItems[allItems.Count - 1].PRN_DT = allItems[0].PRN_DT;
                        allItems[allItems.Count - 1].CHNL_NM = Properties.Settings.Default.SettingCompany;

                        P4411WeekReport report = new P4411WeekReport(allItems);
                        report.Margins.Top = 0;
                        report.Margins.Bottom = 0;
                        report.Margins.Left = 40;
                        report.Margins.Right = 0;
                        report.Landscape = true;
                        report.PrintingSystem.ShowPrintStatusDialog = true;
                        report.PaperKind = System.Drawing.Printing.PaperKind.A4;

                        var window = new DocumentPreviewWindow();
                        window.PreviewControl.DocumentSource = report;
                        report.CreateDocument(true);
                        window.Title = "발주서 [" + (WeekDt).ToString("yyyy년 MM월") + "] ";
                        window.Owner = Application.Current.MainWindow;
                        window.ShowDialog();

                    }

                }

            }

        }

        [Command]
        public async void PrintContact()
        {

            try
            {

                if (SearchDetail == null)
                {
                    return;
                }
                SystemCodeVo coNm = new SystemCodeVo();
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s143/dtl", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { DELT_FLG = "N", CO_NO = this.SelectedMstItem.PUR_CO_CD, AREA_CD = SystemProperties.USER_VO.EMPE_PLC_NM, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        coNm = JsonConvert.DeserializeObject<SystemCodeVo>(await response.Content.ReadAsStringAsync());

                        SelectDtlList[0].R_MM_04 = coNm.CO_NM;
                        SelectDtlList[0].R_MM_05 = "( 전화번호 : " + coNm.HDQTR_PHN_NO + "  /   " + "팩스번호 : " + coNm.HDQTR_FAX_NO + " )";
                        SelectDtlList[0].R_MM_06 = coNm.PRSD_NM;
                    }
                }

                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s143/dtl", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { DELT_FLG = "N", CO_NO = "999", AREA_CD = SystemProperties.USER_VO.EMPE_PLC_NM, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        coNm = JsonConvert.DeserializeObject<SystemCodeVo>(await response.Content.ReadAsStringAsync());

                        if (coNm != null)
                        {
                            SelectDtlList[0].R_MM_07 = coNm.CO_NM;
                            SelectDtlList[0].R_MM_08 = coNm.PRSD_NM;
                            SelectDtlList[0].R_MM_09 = "(" + coNm.HDQTR_PST_NO + ")  " + coNm.HDQTR_ADDR;
                            SelectDtlList[0].R_MM_10 = coNm.HDQTR_PHN_NO;
                            SelectDtlList[0].R_MM_11 = coNm.HDQTR_FAX_NO;
                        }
                    }
                }


                SelectDtlList[SelectDtlList.Count - 1].GBN = "합계금액 : " + String.Format("{0:#,#}", SelectDtlList.Sum<PurVo>(s => Convert.ToDouble(s.PUR_AMT)));
                SelectDtlList[SelectDtlList.Count - 1].R_MM_02 = "발주담당자명 : " + this.SelectedMstItem.CRE_USR_NM;
                SelectDtlList[SelectDtlList.Count - 1].R_MM_01 = System.DateTime.Now.ToString("yyyy년   MM월   dd일");
                SelectDtlList[SelectDtlList.Count - 1].PUR_RMK = this.SelectedMstItem.PUR_RMK;


                P4411Report report = new P4411Report(SelectDtlList);
                report.Margins.Top = 30;
                report.Margins.Bottom = 10;
                report.Margins.Left = 120;
                report.Margins.Right = 10;
                report.Landscape = true;
                report.PrintingSystem.ShowPrintStatusDialog = true;
                report.PaperKind = System.Drawing.Printing.PaperKind.A4;

                report.Watermark.Text = Properties.Settings.Default.SettingCompany;
                report.Watermark.TextDirection = DevExpress.XtraPrinting.Drawing.DirectionMode.ForwardDiagonal;
                report.Watermark.Font = new System.Drawing.Font(report.PrintingSystem.Watermark.Font.FontFamily, 40);
                report.Watermark.ForeColor = System.Drawing.Color.PaleTurquoise;
                report.Watermark.TextTransparency = 150;

                var window = new DocumentPreviewWindow();
                window.PreviewControl.DocumentSource = report;
                report.CreateDocument(true);
                window.Title = "발주서 [" + SelectedMstItem.PUR_ORD_NO + "] ";
                window.Owner = Application.Current.MainWindow;
                window.ShowDialog();


            }
            catch
            {
                return;
            }

        }

        [Command]
        public async void PrintTagLabel()
        {

            try
            {
                if (SearchDetail == null)
                {
                    return;
                }

                P4411TagReport report = new P4411TagReport(SelectDtlList);
                report.Margins.Top = 0;
                report.Margins.Bottom = 0;
                report.Margins.Left = 40;
                report.Margins.Right = 0;
                report.Landscape = true;
                report.PrintingSystem.ShowPrintStatusDialog = true;
                report.PaperKind = System.Drawing.Printing.PaperKind.A5;

                report.Watermark.Text = Properties.Settings.Default.SettingCompany;
                report.Watermark.TextDirection = DevExpress.XtraPrinting.Drawing.DirectionMode.ForwardDiagonal;
                report.Watermark.Font = new System.Drawing.Font(report.PrintingSystem.Watermark.Font.FontFamily, 40);
                report.Watermark.ForeColor = System.Drawing.Color.PaleTurquoise;
                report.Watermark.TextTransparency = 150;

                var window = new DocumentPreviewWindow();
                window.PreviewControl.DocumentSource = report;
                report.CreateDocument(true);
                window.Owner = Application.Current.MainWindow;
                window.ShowDialog();
            }
            catch (System.Exception eLog)
            {

                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }


        [Command]
        public void NewDtlContact()
        {
            if (SelectedMstItem == null)
            {
                return;
            }

            detailDialog_1 = new P4411Detail_1Dialog(new PurVo() { PUR_ORD_NO = SelectedMstItem.PUR_ORD_NO, CHNL_CD = SystemProperties.USER_VO.CHNL_CD, CRE_USR_ID = SystemProperties.USER_VO.USR_ID, UPD_USR_ID = SystemProperties.USER_VO.USR_ID });
            detailDialog_1.Title = "발주 자재 관리 - 품목 등록[" + SelectedMstItem.PUR_ORD_NO + "]";
            detailDialog_1.Owner = Application.Current.MainWindow;
            detailDialog_1.BorderEffect = BorderEffect.Default;
            detailDialog_1.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            detailDialog_1.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            bool isDialog = (bool)detailDialog_1.ShowDialog();
            if (isDialog)
            {
                SelectMstDetail();
            }
        }
        [Command]
        public void EditDtlContact()
        {
            if (SelectedMstItem == null)
            {
                return;
            }

            detailDialog_2 = new P4430InsertDtlDialog(new PurVo() { PUR_ORD_NO = SelectedMstItem.PUR_ORD_NO, CHNL_CD = SystemProperties.USER_VO.CHNL_CD, CRE_USR_ID = SystemProperties.USER_VO.USR_ID, UPD_USR_ID = SystemProperties.USER_VO.USR_ID });
            detailDialog_2.Title = "발주 자재 관리 - 자재 품목 등록[" + SelectedMstItem.PUR_ORD_NO + "]";
            detailDialog_2.Owner = Application.Current.MainWindow;
            detailDialog_2.BorderEffect = BorderEffect.Default;
            detailDialog_2.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            detailDialog_2.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            bool isDialog = (bool)detailDialog_2.ShowDialog();
            if (isDialog)
            {
                SelectMstDetail();
            }
        }

        [Command]
        public async void DelDtlContact()
        {
            try
            {
                //수정 삭제
                if (SearchDetail == null)
                {
                    return;
                }

                MessageBoxResult result = WinUIMessageBox.Show("[" + SearchDetail.PUR_ORD_NO + "/" + SearchDetail.RN + "]" + " 정말로 삭제 하시겠습니까?", "[삭제]" + _title, MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    SelectedMstItem.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("p4430/dtl/d", new StringContent(JsonConvert.SerializeObject(SearchDetail), System.Text.Encoding.UTF8, "application/json")))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            int _Num = 0;
                            string resultMsg = await response.Content.ReadAsStringAsync();
                            if (int.TryParse(resultMsg, out _Num) == false)
                            {

                                WinUIMessageBox.Show(resultMsg, _title, MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                            SelectMstDetail();

                            WinUIMessageBox.Show("삭제가 완료되었습니다.", _title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                        }
                    }


                }
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }




        //12/03 E-Mail 메뉴 제외
        //[Command]
        //public void EmailContact()
        //{
        //    if (SelectedMstItem == null)
        //    {
        //        return;
        //    }

        //    emailDialog = new P4411EmailDialog(SelectedMstItem);
        //    emailDialog.Title = "E-MAIL 관리 [" + SelectedMstItem.PUR_ORD_NO + "]";
        //    emailDialog.Owner = Application.Current.MainWindow;
        //    emailDialog.BorderEffect = BorderEffect.Default;
        //    emailDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
        //    emailDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));

        //    bool isDialog = (bool)emailDialog.ShowDialog();
        //}


        public async void SYSTEM_CODE_VO()
        {

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
            Refresh();
        }
    }
}
