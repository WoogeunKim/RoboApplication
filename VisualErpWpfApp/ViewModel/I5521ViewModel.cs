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
using System.Windows.Input;
using AquilaErpWpfApp3.Util;
using AquilaErpWpfApp3.View.INV.Report;

namespace AquilaErpWpfApp3.ViewModel
{
    public sealed class I5521ViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private string _title = "자재수불명세서";

        //private static InvServiceClient invClient = SystemProperties.InvClient;
        private IList<InvVo> selectedMstList = new List<InvVo>();

        //private ObservableCollection<PurVo> selectedDtlList = new ObservableCollection<PurVo>();
        //private IList<InvVo> selectedDtlList = new List<InvVo>();

        //private I5514MasterDialog masterDialog;
        //private I5513DetailDialog detailDialog;

        ////Menu Dialog
        //private ICommand _searchDialogCommand;
        //private ICommand _newDialogCommand;
        //private ICommand _editDialogCommand;
        //private ICommand _delDialogCommand;

        //private ICommand _searchDetailDialogCommand;
        //private ICommand _newDetailDialogCommand;
        //private ICommand _editDetailDialogCommand;
        //private ICommand _delDetailDialogCommand;
        //private ICommand reportDialogCommand;

        //private ICommand reportDialogCommand;

        ////private ICommand _revListDetailDialogCommand;
        ////private ICommand _revNewDetailDialogCommand;

        //private P41MasterDialog masterDialog;
        //private P41DetailDialog detailDialog;
        ////private A21JobItemRevDialog jobItemRevDialog;

        //private P41ReportDialog reportDialog;

        public I5521ViewModel()
        {
            StartDt = System.DateTime.Now;
            //EndDt = System.DateTime.Now;

            SYSTEM_CODE_VO();

            //품목 구분
            //_ItmGrpMap = SystemProperties.SYSTEM_CODE_MAP("L-001");
            //ItmGrpList = SystemProperties.SYSTEM_CODE_VO("L-001");
            //if (ItmGrpList.Count > 0)
            //{
            //    //M_ITM_GRP_DESC = ItmGrpList[0];
            //    TXT_ITM_GRP_CLSS_NM = ItmGrpList[0].CLSS_DESC;
            //}
            //ItmGrpList = SystemProperties.SYSTEM_CODE_VO("L-001");
            //ItmGrpList.Insert(0, new CodeDao() { CLSS_CD = null, CLSS_DESC = "전체" });
            //_ItmGrpMap = SystemProperties.SYSTEM_CODE_MAP("L-001");
            ////GrpClssList.Insert(0, new CodeDao() { CLSS_CD = "", CLSS_DESC = "" });
            //if (ItmGrpList.Count > 0)
            //{
            //    //M_ITM_GRP_CLSS_CD = GrpClssList[0];
            //    TXT_ITM_GRP_CLSS_NM = ItmGrpList[0].CLSS_DESC;
            //}


            ////사업장
            //AreaList = SystemProperties.SYSTEM_CODE_VO("L-002");
            //_AreaMap = SystemProperties.SYSTEM_CODE_MAP("L-002");
            //if (AreaList.Count > 0)
            //{
            //    TXT_SL_AREA_NM = SystemProperties.USER_VO.EMPE_PLC_CD;
            //    //for (int x = 0; x < AreaList.Count; x++)
            //    //{
            //    //    if (TXT_SL_AREA_NM.Equals(AreaList[x].CLSS_DESC))
            //    //    {
            //    //        M_SL_AREA_NM = AreaList[x];
            //    //        break;
            //    //    }
            //    //}
            //}




            //Refresh();
        }

        [Command]
        public async void Refresh()
        {
            try
            {
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("i5521/mst", new StringContent(JsonConvert.SerializeObject(new InvVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD, YRMON = (StartDt).ToString("yyyyMM"), AREA_CD = M_SL_AREA_NM.CLSS_CD, ITM_GRP_CLSS_CD = M_ITM_GRP_CLSS_NM.CLSS_CD, N1ST_ITM_GRP_CD = M_ITM_N1ST_NM.ITM_GRP_CD }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectMstList = JsonConvert.DeserializeObject<IEnumerable<InvVo>>(await response.Content.ReadAsStringAsync()).Cast<InvVo>().ToList();
                    }   //////

                    //DXSplashScreen.Show<ProgressWindow>();
                    //SearchDetail = null;
                    //SelectDtlList = null;

                    //SelectMstList = invClient.I5521SelectMstList(new InvVo() { YRMON = (StartDt).ToString("yyyyMM")
                    //                                                         , AREA_CD = (string.IsNullOrEmpty(TXT_SL_AREA_NM) ? null : _AreaMap[TXT_SL_AREA_NM] )
                    //                                                         , ITM_GRP_CLSS_CD = ((TXT_ITM_GRP_CLSS_NM.Equals("전체")) ? null : _ItmGrpMap[TXT_ITM_GRP_CLSS_NM])
                    //                                                         , N1ST_ITM_GRP_CD = (string.IsNullOrEmpty(TXT_ITM_N1ST_DESC) ? null : _ItmN1stMap[TXT_ITM_N1ST_DESC]) });
                    //////
                    Title = "[기간]" + (StartDt).ToString("yyyy-MM") + ",   [사업장]" + M_SL_AREA_NM.CLSS_DESC + ",   [품목구분]" + M_ITM_GRP_CLSS_NM.CLSS_DESC + ",   [대 분류]" + M_ITM_N1ST_NM.ITM_GRP_NM;

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
                        //
                        isD_UPDATE = false;
                        isD_DELETE = false;
                    }
                    //DXSplashScreen.Close();
                }
            }
            catch (System.Exception eLog)
            {
                //DXSplashScreen.Close();
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }


        [Command]
        public async void Report()
        {
            IList<InvVo> _reportList = new List<InvVo>();
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("i5521/report", new StringContent(JsonConvert.SerializeObject(new InvVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD, AREA_CD = M_SL_AREA_NM.CLSS_CD, ITM_GRP_CLSS_CD = M_ITM_GRP_CLSS_NM.CLSS_CD, N1ST_ITM_GRP_CD = M_ITM_N1ST_NM.ITM_GRP_CD }), System.Text.Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    _reportList = JsonConvert.DeserializeObject<IEnumerable<InvVo>>(await response.Content.ReadAsStringAsync()).Cast<InvVo>().ToList();
                }
                //_reportList = invClient.I5521SelectStkSilsa(new InvVo() { AREA_CD = SystemProperties.USER_VO.EMPE_PLC_CD, ITM_GRP_CLSS_CD = M_ITM_GRP_CLSS_CD.EditValue.ToString(), N1ST_ITM_GRP_CD =M_N1ST_ITM_GRP_CD.EditValue.ToString() });

                // 재고조사용
                I5521Report report = new I5521Report(_reportList);
                report.Margins.Top = 2;
                report.Margins.Bottom = 0;
                report.Margins.Left = 1;
                report.Margins.Right = 1;
                report.Landscape = true;
                report.PrintingSystem.ShowPrintStatusDialog = true;
                report.PaperKind = System.Drawing.Printing.PaperKind.A4;
                //데모 시연 문서 표시 가능
                //report.Watermark.Text = "한양화스너공업(주)";
                report.Watermark.TextDirection = DirectionMode.ForwardDiagonal;
                report.Watermark.Font = new Font(report.Watermark.Font.FontFamily, 40);
                ////report.Watermark.ForeColor = Color.DodgerBlue;
                report.Watermark.ForeColor = System.Drawing.Color.PaleTurquoise;
                report.Watermark.TextTransparency = 180;
                report.Watermark.ShowBehind = false;
                //report.Watermark.PageRange = "1,3-5";

                var window = new DocumentPreviewWindow();
                window.PreviewControl.DocumentSource = report;
                report.CreateDocument(true);
                window.Title = "연말 재고조사용";
                window.Owner = Application.Current.MainWindow;
                window.ShowDialog();
            }
        }


        //#region Functon <Master List>
        DateTime _startDt;
        public DateTime StartDt
        {
            get { return _startDt; }
            set { SetProperty(ref _startDt, value, () => StartDt); }
        }

        //DateTime _endDt;
        //public DateTime EndDt
        //{
        //    get { return _endDt; }
        //    set { SetProperty(ref _endDt, value, () => EndDt); }
        //}

        //사업장
        //private Dictionary<string, string> _AreaMap = new Dictionary<string, string>();
        private IList<SystemCodeVo> _AreaCd = new List<SystemCodeVo>();
        public IList<SystemCodeVo> AreaList
        {
            get { return _AreaCd; }
            set { SetProperty(ref _AreaCd, value, () => AreaList); }
        }
        ////사업장
        //private CodeDao _M_SL_AREA_NM;
        //public CodeDao M_SL_AREA_NM
        //{
        //    get { return _M_SL_AREA_NM; }
        //    set { SetProperty(ref _M_SL_AREA_NM, value, () => M_SL_AREA_NM); }
        //}
        //사업장 
        private SystemCodeVo _M_SL_AREA_NM;
        public SystemCodeVo M_SL_AREA_NM
        {
            get { return _M_SL_AREA_NM; }
            set { SetProperty(ref _M_SL_AREA_NM, value, () => M_SL_AREA_NM); }
        }


        //품목 구분
        //private Dictionary<string, string> _ItmGrpMap = new Dictionary<string, string>();
        private IList<SystemCodeVo> _ItmGrpCd = new List<SystemCodeVo>();
        public IList<SystemCodeVo> ItmGrpList
        {
            get { return _ItmGrpCd; }
            set { SetProperty(ref _ItmGrpCd, value, () => ItmGrpList); }
        }
        ////품목 구분
        //private CodeDao _M_ITM_GRP_DESC;
        //public CodeDao M_ITM_GRP_DESC
        //{
        //    get { return _M_ITM_GRP_DESC; }
        //    set { SetProperty(ref _M_ITM_GRP_DESC, value, () => M_ITM_GRP_DESC, n1stItmGrpCd); }
        //}
        //품목 구분
        private SystemCodeVo _M_ITM_GRP_CLSS_NM;
        public SystemCodeVo M_ITM_GRP_CLSS_NM
        {
            get { return _M_ITM_GRP_CLSS_NM; }
            set { SetProperty(ref _M_ITM_GRP_CLSS_NM, value, () => M_ITM_GRP_CLSS_NM, n1stItmGrpCd); }
        }
        //
        public async void n1stItmGrpCd()
        {
            try
            {
                //if (M_ITM_GRP_CLSS_NM == null)
                //{
                //    return;
                //}

                //using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s133", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { CRE_USR_ID = "", CHNL_CD = SystemProperties.USER_VO.CHNL_CD, ITM_GRP_CLSS_CD = M_ITM_GRP_CLSS_NM.CLSS_CD, DELT_FLG = "N" }), System.Text.Encoding.UTF8, "application/json")))
                //{
                //    if (response.IsSuccessStatusCode)
                //    {
                //        this.ItmN1stList = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                //        if (ItmN1stList.Count > 0)
                //        {
                //            M_ITM_N1ST_NM = ItmN1stList[0];
                //        }
                //    }


                // ItmN1stList.Clear();
                // _ItmN1stMap.Clear();

                // if (TXT_ITM_GRP_CLSS_NM.Equals("전체"))
                // {
                //     ItmN1stList.Insert(0, new CodeDao() { CLSS_CD = "", CLSS_DESC = "" });
                //     TXT_ITM_N1ST_DESC = ItmN1stList[0].CLSS_DESC;
                //     return;
                // }


                // ItmN1stList = SystemProperties.ITM_N1ST_CODE_VO((string.IsNullOrEmpty(TXT_ITM_GRP_CLSS_NM) ? null : _ItmGrpMap[TXT_ITM_GRP_CLSS_NM]));
                // ItmN1stList.Insert(0, new CodeDao() { CLSS_CD = "", CLSS_DESC = "" });
                // _ItmN1stMap = SystemProperties.ITM_N1ST_CODE_MAP((string.IsNullOrEmpty(TXT_ITM_GRP_CLSS_NM) ? null : _ItmGrpMap[TXT_ITM_GRP_CLSS_NM]));


                //// M_ITM_N1ST_DESC = ItmN1stList[0];
                // TXT_ITM_N1ST_DESC = ItmN1stList[0].CLSS_DESC;
                // //_ItmN1stMap = SystemProperties.ITM_N1ST_CODE_MAP((string.IsNullOrEmpty(M_ITM_GRP_DESC) ? null : _ItmGrpMap[M_ITM_GRP_DESC]));
                //}
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }



        //대분류
        //private Dictionary<string, string> _ItmN1stMap = new Dictionary<string, string>();
        private IList<SystemCodeVo> _ItmN1stCd = new List<SystemCodeVo>();
        public IList<SystemCodeVo> ItmN1stList
        {
            get { return _ItmN1stCd; }
            set { SetProperty(ref _ItmN1stCd, value, () => ItmN1stList); }
        }
        ////대분류
        //private CodeDao _M_ITM_N1ST_DESC;
        //public CodeDao M_ITM_N1ST_DESC
        //{
        //    get { return _M_ITM_N1ST_DESC; }
        //    set { SetProperty(ref _M_ITM_N1ST_DESC, value, () => M_ITM_N1ST_DESC); }
        //}
        //대분류
        private SystemCodeVo _M_ITM_N1ST_NM;
        public SystemCodeVo M_ITM_N1ST_NM
        {
            get { return _M_ITM_N1ST_NM; }
            set { SetProperty(ref _M_ITM_N1ST_NM, value, () => M_ITM_N1ST_NM); }
        }

        private string _M_SEARCH_TEXT = string.Empty;
        public string M_SEARCH_TEXT
        {
            get { return _M_SEARCH_TEXT; }
            set { SetProperty(ref _M_SEARCH_TEXT, value, () => M_SEARCH_TEXT); }
        }

        //private bool? _M_SEARCH_CHECKD = false;
        //public bool? M_SEARCH_CHECKD
        //{
        //    get { return _M_SEARCH_CHECKD; }
        //    set { SetProperty(ref _M_SEARCH_CHECKD, value, () => M_SEARCH_CHECKD); }
        //}


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
                    SetProperty(ref _selectedMstItem, value, () => SelectedMstItem, SelectMstDetail);
                }
            }
        }

        //private bool? _isM_SAVE = false;
        //public bool? isM_SAVE
        //{
        //    get { return _isM_SAVE; }
        //    set { SetProperty(ref _isM_SAVE, value, () => isM_SAVE); }
        //}

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


        private void SelectMstDetail()
        {
            //try
            //{
            //    //DXSplashScreen.Show<ProgressWindow>();

            //   if (this._selectedMstItem == null)
            //   {
            //       return;
            //   }

            //   //SelectDtlList = new ObservableCollection<PurVo>(purClient.P4401SelectDtlList(new PurVo() { ITM_CD = this._selectedMstItem.}));
            //   SelectDtlList = invClient.I5513SelectDtlList(SelectedMstItem);
            //   // //
            //   if (SelectDtlList.Count >= 1)
            //   {
            //       isD_UPDATE = true;
            //       isD_DELETE = true;
            //   }
            //   else
            //   {
            //       isD_UPDATE = false;
            //       isD_DELETE = false;
            //   }

            //   //DXSplashScreen.Close();
            //}
            //catch (System.Exception eLog)
            //{
            //    //DXSplashScreen.Close();
            //    //
            //    WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]출고의뢰내역등록", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
            //    return;
            //}
        }
        //#endregion


        ////#region Functon <Detail List>
        //public IList<InvVo> SelectDtlList
        //{
        //    get { return selectedDtlList; }
        //    set { SetProperty(ref selectedDtlList, value, () => SelectDtlList); }
        //}

        //InvVo _searchDetail;
        //public InvVo SearchDetail
        //{
        //    get
        //    {
        //        return _searchDetail;
        //    }
        //    set
        //    {
        //        if (value != null)
        //        {
        //            SetProperty(ref _searchDetail, value, () => SearchDetail);
        //        }
        //    }
        //}
        ////#endregion


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

            //구분
            //ItmGrpList = SystemProperties.SYSTEM_CODE_VO("L-001");
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "L-001"))
            {
                if (response.IsSuccessStatusCode)
                {
                    ItmGrpList = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                    if (ItmGrpList.Count > 0)
                    {
                        M_ITM_GRP_CLSS_NM = ItmGrpList[0];
                    }
                }
            }


            if (M_ITM_GRP_CLSS_NM == null)
            {
                return;
            }

            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s133", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { CRE_USR_ID = "", CHNL_CD = SystemProperties.USER_VO.CHNL_CD, ITM_GRP_CLSS_CD = M_ITM_GRP_CLSS_NM.CLSS_CD, DELT_FLG = "N" }), System.Text.Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.ItmN1stList = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                    this.ItmN1stList.Insert(0, new SystemCodeVo() { ITM_GRP_CD = null, ITM_GRP_NM = "전체" });

                    if (ItmN1stList.Count > 0)
                    {
                        M_ITM_N1ST_NM = ItmN1stList[0];
                    }
                }


            }
        }

    }
}
