using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.Docking;
using DevExpress.Xpf.Printing;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Code;
using ModelsLibrary.Sale;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;
using AquilaErpWpfApp3.Util;
using AquilaErpWpfApp3.View.SAL.Report;

namespace AquilaErpWpfApp3.ViewModel
{
    public sealed class S22122ViewModel : ViewModelBase, INotifyPropertyChanged
    {
        //private static SaleOrderServiceClient saleOrderClient = SystemProperties.SaleOrderClient;

        private IList<SaleVo> selectedMstList = new List<SaleVo>();
        private IList<SaleVo> selectedDtlList = new List<SaleVo>();
        
        //private ICommand _searchDialogCommand;

        //private ICommand _S22111DialogCommand;
        //private ICommand _S22122DialogCommand;
        //private ICommand _S2229DialogCommand;
        private ICommand reportDialogCommand;

        //private DocumentPanel panel;

        public S22122ViewModel() 
        {
          StartDt = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy-MM-01"));
          EndDt = System.DateTime.Now;

          M_PUR_CLZ_FLG = "";

          SYSTEM_CODE_VO();

            //_DeptMap = SystemProperties.CUSTOMER_CODE_MAP(null, SystemProperties.USER_VO.EMPE_PLC_NM);
            //DeptList = SystemProperties.CUSTOMER_CODE_VO(null, SystemProperties.USER_VO.EMPE_PLC_NM);
            ////DeptList.Insert(0, new CustomerCodeDao() { CO_NO = "", CO_NM = "" });
            ////M_DEPT_DESC = DeptList[0];
            //TXT_DEPT_DESC = DeptList[0].CO_NO;

            //_AreaMap = SystemProperties.SYSTEM_CODE_MAP("L-002");
            //AreaList = SystemProperties.SYSTEM_CODE_VO("L-002");
            //AreaList.Insert(0, new CodeDao() { CLSS_CD = "", CLSS_DESC = "" });
            ////M_SL_AREA_NM = AreaList[0];
            //TXT_SL_AREA_NM = AreaList[0].CLSS_DESC;
            //if (this.AreaList.Count > 0)
            //{
            //    this.TXT_SL_AREA_NM = SystemProperties.USER_VO.EMPE_PLC_CD;
            //    //for (int x = 0; x < this.AreaList.Count; x++)
            //    //{
            //    //    if (this.TXT_SL_AREA_NM.Equals(this.AreaList[x].CLSS_DESC))
            //    //    {
            //    //        this.M_SL_AREA_NM = this.AreaList[x];
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
                //DXSplashScreen.Show<ProgressWindow>();
                //SearchDetail = null;
                //SelectDtlList = null;

                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s22122/mst", new StringContent(JsonConvert.SerializeObject(new SaleVo() { FM_DT = (StartDt).ToString("yyyyMMdd"), TO_DT = (EndDt).ToString("yyyyMMdd"), AREA_CD = M_SL_AREA_NM.CLSS_CD, SL_CO_CD = (M_SL_CO_NM == null ? null : M_SL_CO_NM.CO_NO), CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectMstList = JsonConvert.DeserializeObject<IEnumerable<SaleVo>>(await response.Content.ReadAsStringAsync()).Cast<SaleVo>().ToList();
                    }

                    //SelectMstList = saleOrderClient.S22122SelectMstList(_param);
                    //{ AREA_CD = (string.IsNullOrEmpty(M_SL_AREA_NM.CLSS_CD) ? null : M_SL_AREA_NM.CLSS_CD), FM_DT = (StartDt).ToString("yyyy-MM-dd"), TO_DT = (EndDt).ToString("yyyy-MM-dd"), PUR_CO_CD = (string.IsNullOrEmpty(M_DEPT_DESC.CO_NO) ? null : M_DEPT_DESC.CO_NO), PUR_CLZ_FLG = (string.IsNullOrEmpty(M_PUR_CLZ_FLG) ? null : M_PUR_CLZ_FLG) }
                    ////+ ",       [마감]" + (string.IsNullOrEmpty(M_PUR_CLZ_FLG) ? "전체" : M_PUR_CLZ_FLG)
                    Title = "[기간]" + (StartDt).ToString("yyyy-MM-dd") + "~" + (EndDt).ToString("yyyy-MM-dd") + ",     [사업장]" + M_SL_AREA_NM.CLSS_DESC + ",     [거래처]" + M_SL_CO_NM.CO_NM;

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

                    }
                    //DXSplashScreen.Close();
                }
              }
              catch (System.Exception eLog)
              {
                  //DXSplashScreen.Close();
                  WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]외상매출원장", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                  return;
              }


          }

          #region 발주기간 From To

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

        #region 거래처
        //private Dictionary<string, string> _DeptMap = new Dictionary<string, string>();
        private IList<SystemCodeVo> _CoNmList = new List<SystemCodeVo>();
        public IList<SystemCodeVo> CoNmList
        {
            get { return _CoNmList; }
            set { SetProperty(ref _CoNmList, value, () => CoNmList); }
        }

        //private CustomerCodeDao _M_DEPT_DESC;
        //public CustomerCodeDao M_DEPT_DESC
        //{
        //    get { return _M_DEPT_DESC; }
        //    set { SetProperty(ref _M_DEPT_DESC, value, () => M_DEPT_DESC); }
        //}

        private SystemCodeVo _M_SL_CO_NM;
        public SystemCodeVo M_SL_CO_NM
        {
            get { return _M_SL_CO_NM; }
            set { SetProperty(ref _M_SL_CO_NM, value, () => _M_SL_CO_NM); }
        }
        #endregion

        #region 사업장
        //private Dictionary<string, string> _AreaMap = new Dictionary<string, string>();
        private IList<SystemCodeVo> _AreaCd = new List<SystemCodeVo>();
        public IList<SystemCodeVo> AreaList
        {
            get { return _AreaCd; }
            set { SetProperty(ref _AreaCd, value, () => AreaList); }
        }

        //private CodeDao _M_SL_AREA_NM;
        //public CodeDao M_SL_AREA_NM
        //{
        //    get { return _M_SL_AREA_NM; }
        //    set { SetProperty(ref _M_SL_AREA_NM, value, () => M_SL_AREA_NM, RefreshCoNm); }
        //}

        private SystemCodeVo _M_SL_AREA_NM;
        public SystemCodeVo M_SL_AREA_NM
        {
            get { return _M_SL_AREA_NM; }
            set { SetProperty(ref _M_SL_AREA_NM, value, () => M_SL_AREA_NM, RefreshCoNm); }
        }
        private async void RefreshCoNm()
        {
            if (M_SL_AREA_NM != null)
            {
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s143", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { DELT_FLG = "N", /*CO_TP_CD = "AR", */ AREA_CD = M_SL_AREA_NM.CLSS_CD, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        CoNmList = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                        if (CoNmList.Count > 0)
                        {
                            CoNmList.Insert(0, new SystemCodeVo() { CO_NM = "전체", CO_NO = null });
                            M_SL_CO_NM = CoNmList[0];
                        }
                    }
                }
                //if (_AreaMap[TXT_SL_AREA_NM] != null)
                //{
                //    _DeptMap = SystemProperties.CUSTOMER_CODE_MAP(null, _AreaMap[TXT_SL_AREA_NM]);
                //    _DeptCd = SystemProperties.CUSTOMER_CODE_VO(null, _AreaMap[TXT_SL_AREA_NM]);
                //    _DeptCd.Insert(0, new CustomerCodeDao() { CO_NO = "", CO_NM = "" });
                //    DeptList = _DeptCd;
                //    //M_DEPT_DESC = DeptList[0];
                //    TXT_DEPT_DESC = DeptList[0].CO_NO;
                //}
            }
        }
        #endregion

        #region 마감
        private string _M_PUR_CLZ_FLG = string.Empty;
          public string M_PUR_CLZ_FLG
          {
              get { return _M_PUR_CLZ_FLG; }
              set { SetProperty(ref _M_PUR_CLZ_FLG, value, () => M_PUR_CLZ_FLG); }
          } 
          #endregion



          #region 기타
          //private string _M_SEARCH_TEXT = string.Empty;
          //public string M_SEARCH_TEXT
          //{
          //    get { return _M_SEARCH_TEXT; }
          //    set { SetProperty(ref _M_SEARCH_TEXT, value, () => M_SEARCH_TEXT); }
          //}

          private async void SelectMstDetail()
          {
              try
              {
                    if (SelectedMstItem == null)
                    {
                        return;
                    }

                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s22122/dtl", new StringContent(JsonConvert.SerializeObject(SelectedMstItem), System.Text.Encoding.UTF8, "application/json")))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            this.SelectDtlList = JsonConvert.DeserializeObject<IEnumerable<SaleVo>>(await response.Content.ReadAsStringAsync()).Cast<SaleVo>().ToList();
                        }
                        //else if (SelectedMstItem.SL_YRMON.Equals("(이월)"))
                        //{
                        //    SelectDtlList = null;
                        //    SelectedDtlItem = null;
                        //    return;
                        //}
                        //else if (SelectedMstItem.SL_YRMON.Equals("(일계)"))
                        //{
                        //    SelectDtlList = null;
                        //    SelectedDtlItem = null;
                        //    return;
                        //}
                        //else if (SelectedMstItem.SL_YRMON.Equals("(월계)"))
                        //{
                        //    SelectDtlList = null;
                        //    SelectedDtlItem = null;
                        //    return;
                        //}
                        //else if (SelectedMstItem.SL_YRMON.Equals("(합계)"))
                        //{
                        //    SelectDtlList = null;
                        //    SelectedDtlItem = null;
                        //    return;
                        //}
                        //else if (SelectedMstItem.SL_YRMON.Equals("(부가세)"))
                        //{
                        //    SelectDtlList = null;
                        //    SelectedDtlItem = null;
                        //    return;
                        //}

                        //SelectDtlList = saleOrderClient.S22122SelectDtlList(SelectedMstItem);
                        //{ AREA_CD = (string.IsNullOrEmpty(M_SL_AREA_NM.CLSS_CD) ? null : M_SL_AREA_NM.CLSS_CD), FM_DT = (StartDt).ToString("yyyy-MM-dd"), TO_DT = (EndDt).ToString("yyyy-MM-dd"), PUR_CO_CD = (string.IsNullOrEmpty(M_DEPT_DESC.CO_NO) ? null : M_DEPT_DESC.CO_NO), PUR_CLZ_FLG = (string.IsNullOrEmpty(M_PUR_CLZ_FLG) ? null : M_PUR_CLZ_FLG) }
                        ////+ ",       [마감]" + (string.IsNullOrEmpty(M_PUR_CLZ_FLG) ? "전체" : M_PUR_CLZ_FLG)
                        //Title = "[기간]" + (StartDt).ToString("yyyy-MM-dd") + "~" + (EndDt).ToString("yyyy-MM-dd") + ",     [사업장]" + (string.IsNullOrEmpty(TXT_SL_AREA_NM) ? "전체" : TXT_SL_AREA_NM) + ",    [거래처]" + (string.IsNullOrEmpty(TXT_DEPT_DESC.Trim()) ? "전체" : _DeptMap[TXT_DEPT_DESC]);

                        if (SelectDtlList.Count >= 1)
                        {
                            SelectedDtlItem = SelectDtlList[0];
                        }
                    }
              }
              catch (System.Exception eLog)
              {
                   //DXSplashScreen.Close();
                   WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]외상매출원장", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                   return;
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

          string _Title = string.Empty;
          public string Title
          {
              get { return _Title; }
              set { SetProperty(ref _Title, value, () => Title); }
          }
          #endregion

          #region 마스터 그리드
          public IList<SaleVo> SelectMstList
          {
              get { return selectedMstList; }
              set { SetProperty(ref selectedMstList, value, () => SelectMstList); }
          }

        SaleVo _selectedMstItem;
          public SaleVo SelectedMstItem
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

          //
          public IList<SaleVo> SelectDtlList
          {
              get { return selectedDtlList; }
              set { SetProperty(ref selectedDtlList, value, () => SelectDtlList); }
          }

        SaleVo _selectedDtlItem;
          public SaleVo SelectedDtlItem
          {
              get
              {
                  return _selectedDtlItem;
              }
              set
              {
                  if (value != null)
                  {
                      SetProperty(ref _selectedDtlItem, value, () => SelectedDtlItem);
                  }
              }
          } 
          #endregion


        
          ////"명세서대사용";
          //public ICommand S22111DialogCommand
          //{
          //    get
          //    {
          //        if (_S22111DialogCommand == null)
          //            _S22111DialogCommand = new DelegateCommand(CALL_S22111);
          //        return _S22111DialogCommand;
          //    }
          //}


          //private void CALL_S22111()
          //{

          //    SystemProperties.DOCK_MANGER.ClosedPanels.Clear();
          //    BaseLayoutItem item = SystemProperties.DOCK_MANGER.GetItem("S22111");
          //    if (item != null)
          //    {
          //        //SystemProperties.DOCK_MANGER.ActiveMDIItem = item;
          //        //item.AllowSelection = true;
          //        //item.IsActive = true;
          //        //return;
          //        item.Closed = true;
          //    }


          //    this.panel = SystemProperties.DOCK_MANGER.DockController.AddDocumentPanel(SystemProperties.DOCK_GROUP, new Uri(SystemProperties.PROGRAM_NAME + "View/SAL/S22111.xaml", UriKind.Relative));
          //    this.panel.Name = "S22111";
          //    this.panel.Caption = "명세서대사용";
          //    //this.panel.ToolTip = "명세서대사용 - [" + SearchDetail.ITM_CD + "]" + SearchDetail.ITM_NM;
          //    //this.panel.Caption = "명세서대사용 - [" + SearchDetail.ITM_CD + "]" + SearchDetail.ITM_NM;
          //    this.panel.AllowContextMenu = false;
          //    //panel.CaptionImage = DecodePhoto(dao.IMAGE);
          //    //panel.CaptionImage = new BitmapImage(new Uri(SystemProperties.PROGRAM_NAME + "Images/Menu/" + dao.PGM_IMG_NM + ".png", UriKind.Relative));

          //    SystemProperties.DOCK_MANGER.Activate(panel);


          //}


          ////"외상매출명세서";
          //public ICommand S22122DialogCommand
          //{
          //    get
          //    {
          //        if (_S22122DialogCommand == null)
          //            _S22122DialogCommand = new DelegateCommand(CALL_S22122);
          //        return _S22122DialogCommand;
          //    }
          //}


          //private void CALL_S22122()
          //{

          //    SystemProperties.DOCK_MANGER.ClosedPanels.Clear();
          //    BaseLayoutItem item = SystemProperties.DOCK_MANGER.GetItem("S22122");
          //    if (item != null)
          //    {
          //        //SystemProperties.DOCK_MANGER.ActiveMDIItem = item;
          //        //item.AllowSelection = true;
          //        //item.IsActive = true;
          //        //return;
          //        item.Closed = true;
          //    }


          //    this.panel = SystemProperties.DOCK_MANGER.DockController.AddDocumentPanel(SystemProperties.DOCK_GROUP, new Uri(SystemProperties.PROGRAM_NAME + "View/SAL/S22122.xaml", UriKind.Relative));
          //    this.panel.Name = "S22122";
          //    this.panel.Caption = "외상매출명세서";
          //    //this.panel.ToolTip = "명세서대사용 - [" + SearchDetail.ITM_CD + "]" + SearchDetail.ITM_NM;
          //    //this.panel.Caption = "명세서대사용 - [" + SearchDetail.ITM_CD + "]" + SearchDetail.ITM_NM;
          //    this.panel.AllowContextMenu = false;
          //    //panel.CaptionImage = DecodePhoto(dao.IMAGE);
          //    //panel.CaptionImage = new BitmapImage(new Uri(SystemProperties.PROGRAM_NAME + "Images/Menu/" + dao.PGM_IMG_NM + ".png", UriKind.Relative));

          //    SystemProperties.DOCK_MANGER.Activate(panel);
          //}


          ////"매출-일자별원장";
          //public ICommand S2229DialogCommand
          //{
          //    get
          //    {
          //        if (_S2229DialogCommand == null)
          //            _S2229DialogCommand = new DelegateCommand(CALL_S2229);
          //        return _S2229DialogCommand;
          //    }
          //}


          //private void CALL_S2229()
          //{

          //    SystemProperties.DOCK_MANGER.ClosedPanels.Clear();
          //    BaseLayoutItem item = SystemProperties.DOCK_MANGER.GetItem("S2229");
          //    if (item != null)
          //    {
          //        //SystemProperties.DOCK_MANGER.ActiveMDIItem = item;
          //        //item.AllowSelection = true;
          //        //item.IsActive = true;
          //        //return;
          //        item.Closed = true;
          //    }


          //    this.panel = SystemProperties.DOCK_MANGER.DockController.AddDocumentPanel(SystemProperties.DOCK_GROUP, new Uri(SystemProperties.PROGRAM_NAME + "View/SAL/S2229.xaml", UriKind.Relative));
          //    this.panel.Name = "S2229";
          //    this.panel.Caption = "매출-일자별원장 ";
          //    //this.panel.ToolTip = "명세서대사용 - [" + SearchDetail.ITM_CD + "]" + SearchDetail.ITM_NM;
          //    //this.panel.Caption = "명세서대사용 - [" + SearchDetail.ITM_CD + "]" + SearchDetail.ITM_NM;
          //    this.panel.AllowContextMenu = false;
          //    //panel.CaptionImage = DecodePhoto(dao.IMAGE);
          //    //panel.CaptionImage = new BitmapImage(new Uri(SystemProperties.PROGRAM_NAME + "Images/Menu/" + dao.PGM_IMG_NM + ".png", UriKind.Relative));

          //    SystemProperties.DOCK_MANGER.Activate(panel);
          //}
          public ICommand ReportDialogCommand
          {
              get
              {
                  if (reportDialogCommand == null)
                      reportDialogCommand = new DelegateCommand(ReportContact);
                  return reportDialogCommand;
              }
          }

          public async void ReportContact()
          {
              try
              {
                  if (SelectedMstItem == null)
                  {
                      return;
                  }
                IList<SaleVo> vo = new List<SaleVo>();
                vo = SelectMstList;

                SaleVo printDao = SelectedMstItem;
                if (printDao != null)
                {
                    if (SelectMstList.Count > 0)
                    {
                        string SL_CO_CD = (M_SL_CO_NM == null ? null : M_SL_CO_NM.CO_NO);
                        string tmpDT = (StartDt).ToString("yyyy-MM-dd") + " ~ " + (EndDt).ToString("yyyy-MM-dd");

                        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s22122/report", new StringContent(JsonConvert.SerializeObject(SelectedMstItem), System.Text.Encoding.UTF8, "application/json")))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                printDao = JsonConvert.DeserializeObject<SaleVo>(await response.Content.ReadAsStringAsync());
                            }

                            //printDao = saleOrderClient.S22111SelectCOList(new SaleVo() { SL_CO_CD = SL_CO_CD, CHNL_CD = SystemProperties.USER_VO.CHNL_CD });
                            for (int x = 0; x < vo.Count; x++)
                            {
                                vo[x].APLY_DT = tmpDT;
                                vo[x].CO_NO = SL_CO_CD;
                                vo[x].CO_NM = printDao.CO_NM;
                                vo[x].HDQTR_PHN_NO = printDao.HDQTR_PHN_NO;
                                vo[x].HDQTR_FAX_NO = printDao.HDQTR_FAX_NO;
                                if (vo[x].GBN != null)
                                {
                                    if (vo[x].GBN.Equals("이월"))
                                    {
                                        vo[x].GBN = "-- " + vo[x].GBN + " --";
                                    }
                                    else if (vo[x].GBN.Equals("일계"))
                                    {
                                        vo[x].GBN = "> " + vo[x].GBN + " <";
                                    }
                                    else if (vo[x].GBN.Equals("월계"))
                                    {
                                        vo[x].GBN = ">> " + vo[x].GBN + " <<";
                                    }
                                    else if (vo[x].GBN.Equals("합계"))
                                    {
                                        vo[x].GBN = ">>> " + vo[x].GBN + " <<<";
                                    }
                                }
                            }
                        }

                        S22122Report report = new S22122Report(vo);
                        report.Margins.Top = 0;
                        report.Margins.Bottom = 0;
                        report.Margins.Left = 30;
                        report.Margins.Right = 0;
                        report.Landscape = false;
                        report.PrintingSystem.ShowPrintStatusDialog = true;
                        report.PaperKind = System.Drawing.Printing.PaperKind.A4;

                        var window = new DocumentPreviewWindow();
                        window.PreviewControl.DocumentSource = report;
                        report.CreateDocument(true);
                        window.Title = "외상매출원장 출력";
                        window.Owner = Application.Current.MainWindow;
                        window.ShowDialog();

                        //S22122Report report = new S22122Report(vo);
                        //report.Margins.Top = 0;
                        //report.Margins.Bottom = 0;
                        //report.Margins.Left = 30;
                        //report.Margins.Right = 0;
                        //report.Landscape = false;
                        //report.PrintingSystem.ShowPrintStatusDialog = true;
                        //report.PaperKind = System.Drawing.Printing.PaperKind.A4;
                        //XtraReportPreviewModel model = new XtraReportPreviewModel(report);
                        //DocumentPreviewWindow window = new DocumentPreviewWindow() { Model = model };
                        //report.CreateDocument(true);
                        //window.Owner = Application.Current.MainWindow;
                        //window.Title = "외상매출원장 출력 ";
                        //window.ShowDialog();
                    }

                }
                Refresh();
              }
              catch (System.Exception eLog)
              {
                  //DXSplashScreen.Close();
                  WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]외상매출원장", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                  return;
              }
          }



        public async void SYSTEM_CODE_VO()
        {
            //사업장
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

            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s143", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { DELT_FLG = "N", /*CO_TP_CD = "AR", */ AREA_CD = SystemProperties.USER_VO.EMPE_PLC_NM, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    CoNmList = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                    if (CoNmList.Count > 0)
                    {
                        CoNmList.Insert(0, new SystemCodeVo() { CO_NM = "전체", CO_NO = null });
                        M_SL_CO_NM = CoNmList[0];
                    }
                }
            }

            //Refresh();
        }


    }
}
