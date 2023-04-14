using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Printing;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Code;
using ModelsLibrary.Inv;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Media;
using AquilaErpWpfApp3.Util;
using AquilaErpWpfApp3.View.INV.Dialog;
using AquilaErpWpfApp3.View.INV.Report;
using DevExpress.XtraReports.UI;

namespace AquilaErpWpfApp3.ViewModel
{
    public sealed class I6611ViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private string _title = "품목 입고";

        //private static InvServiceClient invClient = SystemProperties.InvClient;
        private IList<InvVo> selectedMstList = new List<InvVo>();
        private IList<InvVo> selectedMstItemsList = new List<InvVo>();

        //private ObservableCollection<PurVo> selectedDtlList = new ObservableCollection<PurVo>();
        //private IList<InvVo> selectedDtlList = new List<InvVo>();

        private I6611DetailPurDialog detailPurDialog;
        private I6611DetailOtherCaseDialog detailOtherCaseDialog;


        //private ICommand _newDetailPurDialogCommand;
        //private ICommand _newDetailImpDialogCommand;

        ////Menu Dialog
        //private ICommand _searchDialogCommand;
        //private ICommand _newDialogCommand;
        //private ICommand _editDialogCommand;
        //private ICommand _delDialogCommand;
        //private ICommand reportDialogCommand;

        //private ICommand _searchDetailDialogCommand;
        //private ICommand _newDetailDialogCommand;
        //private ICommand _editDetailDialogCommand;
        //private ICommand _delDetailDialogCommand;

        //private ICommand reportDialogCommand;

        ////private ICommand _revListDetailDialogCommand;
        ////private ICommand _revNewDetailDialogCommand;

        //private P41MasterDialog masterDialog;
        //private P41DetailDialog detailDialog;
        ////private A21JobItemRevDialog jobItemRevDialog;

        //private P41ReportDialog reportDialog;

        public I6611ViewModel() 
        {
            StartDt = System.DateTime.Now;
            EndDt = System.DateTime.Now;

            IN_FLG = false;

            SYSTEM_CODE_VO();
           
        }

        [Command]
        public async void BarCodeContact()
        {
            try
            {
                if (this.SelectedMstItem != null)
                {
                    if (string.IsNullOrEmpty(Properties.Settings.Default.str_PrnNm))
                    {
                        System.Windows.Controls.PrintDialog dialogue = new System.Windows.Controls.PrintDialog();
                        if (dialogue.ShowDialog() == true)
                        {
                            Properties.Settings.Default.str_PrnNm = dialogue.PrintQueue.FullName;
                            Properties.Settings.Default.Save();
                        }
                    }

                    //InvVo BarcodeDao = new InvVo();

                    //using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("i6611/bar", new StringContent(JsonConvert.SerializeObject(SelectedMstItem), System.Text.Encoding.UTF8, "application/json")))
                    //{
                    //    if (response.IsSuccessStatusCode)
                    //    {
                    //        BarcodeDao = JsonConvert.DeserializeObject<InvVo>(await response.Content.ReadAsStringAsync());

                    //        if(BarcodeDao == null)
                    //        {
                    //            WinUIMessageBox.Show("해당 바코드가 존재하지 않습니다.", "[유효검사]", MessageBoxButton.OK, MessageBoxImage.Error);
                    //            return;
                    //        }
                    //        else if (BarcodeDao.LOT_NO == null)
                    //        {
                    //            WinUIMessageBox.Show("해당 바코드가 존재하지 않습니다.", "[유효검사]", MessageBoxButton.OK, MessageBoxImage.Error);
                    //            return;
                    //        }
                    //        else if (BarcodeDao.ITM_QTY == null)
                    //        {
                    //            WinUIMessageBox.Show("해당 바코드의 잔량이 존재하지 않습니다.", "[유효검사]", MessageBoxButton.OK, MessageBoxImage.Error);
                    //            return;
                    //        }
                    //        else if (double.Parse(BarcodeDao.ITM_QTY.ToString()) <= 0)
                    //        {
                    //            WinUIMessageBox.Show("해당 바코드의 잔량이 존재하지 않습니다.", "[유효검사]", MessageBoxButton.OK, MessageBoxImage.Error);
                    //            return;
                    //        }
                    //    }
                    //}SelectedMstItem

                    //MessageBoxResult result = WinUIMessageBox.Show("[" + BarcodeDao.LOT_NO + "] 정말로 바코드 하시겠습니까?", "[바코드 - 출력]" + _title, MessageBoxButton.YesNo, MessageBoxImage.Question);
                    MessageBoxResult result = WinUIMessageBox.Show("[" + SelectedMstItem.LOT_NO + "] 정말로 바코드 하시겠습니까?", "[바코드 - 출력]" + _title, MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {

                        I6611BarCodeReport BarCodeReport = new I6611BarCodeReport(SelectedMstItem);
                        BarCodeReport.ShowPrintMarginsWarning = false;
                        //// 페이지 크기 설정
                        //BarCodeReport.PageWidth = 1000;
                        //BarCodeReport.PageHeight = 1000;

                        //// 인쇄 가능한 영역 지정
                        //float margin = 10; // 여백 값을 조정합니다.
                        //BarCodeReport.Margins = new System.Drawing.Printing.Margins((int)(margin), (int)(margin), (int)(margin), (int)(margin));


                        //var margins = BarCodeReport.Margins;
                        //margins.Left = 10;
                        //margins.Right = 50;
                        //margins.Top = 30;
                        //margins.Bottom = 30;

                        ReportPrintTool printTool = new ReportPrintTool(BarCodeReport);
                        printTool.PrinterSettings.Copies = Convert.ToInt16(1);
                        printTool.PrintingSystem.ShowPrintStatusDialog = false;
                        printTool.PrintingSystem.ShowMarginsWarning = false;
                        printTool.Print(Properties.Settings.Default.str_PrnNm);
                    }
                }
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }

        [Command]
        public void BarPrintConfig()
        {

            try
            {
                System.Windows.Controls.PrintDialog dialogue = new System.Windows.Controls.PrintDialog();
                if (dialogue.ShowDialog() == true)
                {
                    Properties.Settings.Default.str_PrnNm = dialogue.PrintQueue.FullName;
                    Properties.Settings.Default.Save();
                }
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[에러]" + _Title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }



        [Command]
       public async void Refresh()
        {
            try
            {
                if (DXSplashScreen.IsActive == false) DXSplashScreen.Show<ProgressWindow>();

                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("i6611/mst", new StringContent(JsonConvert.SerializeObject(new InvVo() {CHNL_CD = SystemProperties.USER_VO.CHNL_CD, FM_DT = (StartDt).ToString("yyyy-MM-dd"), TO_DT = (EndDt).ToString("yyyy-MM-dd"), AREA_CD = M_SL_AREA_NM.CLSS_CD, GBN = (string.IsNullOrEmpty(M_CHECKD) ? null : M_CHECKD), LOC_CD = M_SL_LOC_NM.CLSS_CD, IN_FLG = (IN_FLG == true ? "Y" : "N") }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectMstList = JsonConvert.DeserializeObject<IEnumerable<InvVo>>(await response.Content.ReadAsStringAsync()).Cast<InvVo>().ToList();


                        //SelectMstList = invClient.I6610SelectMstList(new InvVo() { FM_DT = (StartDt).ToString("yyyy-MM-dd"), TO_DT = (EndDt).ToString("yyyy-MM-dd"), AREA_CD = (string.IsNullOrEmpty(TXT_SL_AREA_NM) ? null : _AreaMap[TXT_SL_AREA_NM]), GBN = (string.IsNullOrEmpty(M_CHECKD) ? "%" : M_CHECKD), LOC_CD = (string.IsNullOrEmpty(TXT_SL_LOC_NM) ? null : _LocMap[TXT_SL_LOC_NM]), IN_FLG = (IN_FLG == true ? "Y" : "N") });
                        ////+ ",   [입고여부]" + (IN_FLG == true ? "Y" : "N") 
                        Title = "[기간]" + (StartDt).ToString("yyyy-MM-dd") + "~" + (EndDt).ToString("yyyy-MM-dd") + ",   [사업장]" + M_SL_AREA_NM.CLSS_DESC + ",   [창고]" + M_SL_LOC_NM.CLSS_DESC;

                        if (SelectMstList.Count >= 1)
                        {
                            isM_UPDATE = true;
                            isM_DELETE = true;
                            //SelectedMstItem = SelectMstList[0];
                            //for (int x = 0; x < SelectMstList.Count; x++)
                            //{
                            //    SelectMstList[x].DTL_DATA = invClient.I6610SelectDtlList(SelectedMstItem);
                            //}
                        }
                        else
                        {
                            isM_UPDATE = false;
                            isM_DELETE = false;
                        }
                        //DXSplashScreen.Close();
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

        //private Dictionary<string, string> _DeptMap = new Dictionary<string, string>();
        //private IList<CodeDao> _DeptCd = new List<CodeDao>();
        //public IList<CodeDao> DeptList
        //{
        //    get { return _DeptCd; }
        //    set { SetProperty(ref _DeptCd, value, () => DeptList); }
        //}

        //private string _M_DEPT_DESC;
        //public string M_DEPT_DESC
        //{
        //    get { return _M_DEPT_DESC; }
        //    set { SetProperty(ref _M_DEPT_DESC, value, () => M_DEPT_DESC); }
        //}


        private string _M_SEARCH_TEXT = string.Empty;
        public string M_SEARCH_TEXT
        {
            get { return _M_SEARCH_TEXT; }
            set { SetProperty(ref _M_SEARCH_TEXT, value, () => M_SEARCH_TEXT); }
        }


        //사업장
        //private Dictionary<string, string> _AreaMap = new Dictionary<string, string>();
        private IList<SystemCodeVo> _AreaCd = new List<SystemCodeVo>();
        public IList<SystemCodeVo> AreaList
        {
            get { return _AreaCd; }
            set { SetProperty(ref _AreaCd, value, () => AreaList); }
        }
        //사업장
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

        //창고
        //private Dictionary<string, string> _LocMap = new Dictionary<string, string>();
        private IList<SystemCodeVo> _LocCd = new List<SystemCodeVo>();
        public IList<SystemCodeVo> LocList
        {
            get { return _LocCd; }
            set { SetProperty(ref _LocCd, value, () => LocList); }
        }
        //창고
        //private CodeDao _M_SL_AREA_NM;
        //public CodeDao M_SL_AREA_NM
        //{
        //    get { return _M_SL_AREA_NM; }
        //    set { SetProperty(ref _M_SL_AREA_NM, value, () => M_SL_AREA_NM); }
        //}
        //창고 
        private SystemCodeVo _M_SL_LOC_NM;
        public SystemCodeVo M_SL_LOC_NM
        {
            get { return _M_SL_LOC_NM; }
            set { SetProperty(ref _M_SL_LOC_NM, value, () => M_SL_LOC_NM); }
        }

        //구분
        private bool? _M_CHECKD_ALL = true;
        public bool? M_CHECKD_ALL
        {
            get { return _M_CHECKD_ALL; }
            set { SetProperty(ref _M_CHECKD_ALL, value, () => M_CHECKD_ALL, SelectCheckdAll); }
        }
        private bool? _M_CHECKD_PO = false;
        public bool? M_CHECKD_PO
        {
            get { return _M_CHECKD_PO; }
            set { SetProperty(ref _M_CHECKD_PO, value, () => M_CHECKD_PO, SelectCheckdPo); }
        }
        private bool? _M_CHECKD_IV = false;
        public bool? M_CHECKD_IV
        {
            get { return _M_CHECKD_IV; }
            set { SetProperty(ref _M_CHECKD_IV, value, () => M_CHECKD_IV, SelectCheckdIv); }
        }


        private String M_CHECKD = string.Empty;
        private String M_CHECKD_NAME = string.Empty;
        private void SelectCheckdAll()
        {
            if (M_CHECKD_ALL == true)
            {
                //M_CHECKD_ALL = true;
                M_CHECKD_PO = false;
                M_CHECKD_IV = false;
                //
                M_CHECKD = "";
                M_CHECKD_NAME = "전체";
            }

            if (M_CHECKD_ALL == false && M_CHECKD_PO == false && M_CHECKD_IV == false)
            {
                M_CHECKD_ALL = true;
                M_CHECKD_NAME = "전체";
            }
            
            //else
            //{
            //    M_CHECKD_PO = true;
            //    M_CHECKD = "PO";
            //    M_CHECKD_NAME = "국내";
            //}
        }
        private void SelectCheckdPo()
        {
            if (M_CHECKD_PO == true)
            {
                //M_CHECKD_PO = true;
                M_CHECKD_ALL = false;
                M_CHECKD_IV = false;
                //
                M_CHECKD = "PO";
                M_CHECKD_NAME = "국내";
            }

            if (M_CHECKD_ALL == false && M_CHECKD_PO == false && M_CHECKD_IV == false)
            {
                M_CHECKD_ALL = true;
                M_CHECKD_NAME = "전체";
            }
            //else
            //{
            //    M_CHECKD_ALL = true;
            //    M_CHECKD = "";
            //    M_CHECKD_NAME = "전체";
            //}
        }
        private void SelectCheckdIv()
        {
            if (M_CHECKD_IV == true)
            {
                //M_CHECKD_IV = true;
                M_CHECKD_PO = false;
                M_CHECKD_ALL = false;
                //
                M_CHECKD = "IV";
                M_CHECKD_NAME = "수입";
            }

            if (M_CHECKD_ALL == false && M_CHECKD_PO == false && M_CHECKD_IV == false)
            {
                M_CHECKD_ALL = true;
                M_CHECKD_NAME = "전체";
            }
            //else
            //{
            //    M_CHECKD_ALL = true;
            //    M_CHECKD = "";
            //    M_CHECKD_NAME = "전체";
            //}
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
                SetProperty(ref _selectedMstItem, value, () => SelectedMstItem, SelectMstDetail);

            }
        }

        public IList<InvVo> SelectedMstItems
        {
            get { return selectedMstItemsList; }
            set { SetProperty(ref selectedMstItemsList, value, () => SelectedMstItems); }
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


        //입고 여부
        private bool? _IN_FLG = false;
        public bool? IN_FLG
        {
            get { return _IN_FLG; }
            set { SetProperty(ref _IN_FLG, value, () => IN_FLG); }
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

             //       SearchDetail = SelectDtlList[0];
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

        string _Title = string.Empty;
        public string Title
        {
            get { return _Title; }
            set { SetProperty(ref _Title, value, () => Title); }
        }



        //#region Functon Command <add, Edit, Del>
        //public ICommand SearchDialogCommand
        //{
        //    get
        //    {
        //        if (_searchDialogCommand == null)
        //            _searchDialogCommand = new DelegateCommand(Refresh);
        //        return _searchDialogCommand;
        //    }
        //}

        //public ICommand NewDialogCommand
        //{
        //    get
        //    {
        //        if (_newDialogCommand == null)
        //            _newDialogCommand = new DelegateCommand(NewContact);
        //        return _newDialogCommand;
        //    }
        //}

        //public void NewContact()
        //{
        //    masterDialog = new I5513MasterDialog(new InvVo() { DO_RQST_DT = System.DateTime.Now.ToString("yyyy-MM-dd"), DO_RQST_USR_ID = SystemProperties.USER_VO.USR_ID, CLZ_FLG= "N" });
        //    masterDialog.Title = this._title + " - 추가";
        //    masterDialog.Owner = Application.Current.MainWindow;
        //    masterDialog.BorderEffect = BorderEffect.Default;
        //    ////masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
        //    ////masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
        //    bool isDialog = (bool)masterDialog.ShowDialog();
        //    if (isDialog)
        //    {
        //        if (masterDialog.IsEdit == false)
        //        {
        //            try
        //            {
        //                DXSplashScreen.Show<ProgressWindow>();
        //                Refresh();
        //                DXSplashScreen.Close();
        //            }
        //            catch (System.Exception eLog)
        //            {
        //                DXSplashScreen.Close();
        //                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
        //                return;
        //            }
        //        }
        //    }
        //}

        //public ICommand EditDialogCommand
        //{
        //    get
        //    {
        //        if (_editDialogCommand == null)
        //            _editDialogCommand = new DelegateCommand(EditContact);
        //        return _editDialogCommand;
        //    }
        //}

        //public void EditContact()
        //{
        //    if (SelectedMstItem == null)
        //    {
        //        return;
        //    }
        //    else if (SelectMstList.Count <= 0)
        //    {
        //        return;
        //    }

        //    masterDialog = new I5513MasterDialog(SelectedMstItem);
        //    masterDialog.Title = this._title + " - 수정";
        //    masterDialog.Owner = Application.Current.MainWindow;
        //    masterDialog.BorderEffect = BorderEffect.Default;
        //    ////masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
        //    ////masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
        //    bool isDialog = (bool)masterDialog.ShowDialog();
        //    if (isDialog)
        //    {
        //        if (masterDialog.IsEdit == false)
        //        {
        //            try
        //            {
        //                DXSplashScreen.Show<ProgressWindow>();
        //                Refresh();
        //                DXSplashScreen.Close();
        //            }
        //            catch (System.Exception eLog)
        //            {
        //                DXSplashScreen.Close();
        //                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
        //                return;
        //            }
        //        }
        //    }
        //}



        //public ICommand DelDialogCommand
        //{
        //    get
        //    {
        //        if (_delDialogCommand == null)
        //            _delDialogCommand = new DelegateCommand(DelContact);
        //        return _delDialogCommand;
        //    }
        //}

        [Command]
        public async void DelContact()
        {
            try
            {
                if (SelectedMstItem == null)
                {
                    return;
                }

                //MessageBoxResult result = WinUIMessageBox.Show("정말로 삭제 하시겠습니까?", "[삭제]" + _title, MessageBoxButton.YesNo, MessageBoxImage.Question);
                //if (result == MessageBoxResult.Yes)
                //{
                //    int _Num = 0;
                //    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("i6610/mst/d", new StringContent(JsonConvert.SerializeObject(SelectedMstItems), System.Text.Encoding.UTF8, "application/json")))
                //    {
                //        if (response.IsSuccessStatusCode)
                //        {
                //            string resultMsg = await response.Content.ReadAsStringAsync();
                //            if (int.TryParse(resultMsg, out _Num) == false)
                //            {
                //                //실패
                //                WinUIMessageBox.Show(resultMsg, _title, MessageBoxButton.OK, MessageBoxImage.Error);
                //                return;
                //            }
                //        }
                //    }
                //        // InvVo resultVo;
                //        //// DXSplashScreen.Show<ProgressWindow>();
                //        // for (int x = 0; x < SelectedMstItems.Count; x++)
                //        // {
                //        //    resultVo = invClient.I6610DeleteMst(new InvVo() { INAUD_TMP_NO = SelectedMstItems[x].INAUD_TMP_NO, INAUD_TMP_SEQ = SelectedMstItems[x].INAUD_TMP_SEQ });
                //        //    if (!resultVo.isSuccess)
                //        //    {
                //        //        //실패
                //        //        WinUIMessageBox.Show(resultVo.Message, this._title, MessageBoxButton.OK, MessageBoxImage.Error);
                //        //        return;
                //        //    }
                //        // }
                //        // //DXSplashScreen.Close();

                //        Refresh();
                //        WinUIMessageBox.Show("삭제가 완료되었습니다.", "[삭제]" + _title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                    //}
            }
            catch (System.Exception)
            {
                 //DXSplashScreen.Close();
                 return;
            }
        }




        //public ICommand NewDtlPurDialogCommand
        //{
        //    get
        //    {
        //        if (_newDetailPurDialogCommand == null)
        //            _newDetailPurDialogCommand = new DelegateCommand(NewDtlPurContact);
        //        return _newDetailPurDialogCommand;
        //    }
        //}
        [Command]
        public void NewDtlPurContact()
        {
            try
            {
                detailPurDialog = new I6611DetailPurDialog(new InvVo() {CHNL_CD = SystemProperties.USER_VO.CHNL_CD, FM_DT = (StartDt).ToString("yyyy-MM-dd"), TO_DT = (EndDt).ToString("yyyy-MM-dd") });
                detailPurDialog.Title = "구매 입고 자재 관리 ";
                detailPurDialog.Owner = Application.Current.MainWindow;
                detailPurDialog.BorderEffect = BorderEffect.Default;
                detailPurDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
                detailPurDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
                bool isDialog = (bool)detailPurDialog.ShowDialog();
                //if (isDialog)
                {
                    Refresh();
                }
            }
            catch (System.Exception)
            {
                return;
            }
        }


        //public ICommand NewDtlImpDialogCommand
        //{
        //    get
        //    {
        //        if (_newDetailImpDialogCommand == null)
        //            _newDetailImpDialogCommand = new DelegateCommand(NewDtlImpContact);
        //        return _newDetailImpDialogCommand;
        //    }
        //}

        [Command]
        public void NewDtlOtherCaseDialog()
        {
            detailOtherCaseDialog = new I6611DetailOtherCaseDialog();
            detailOtherCaseDialog.Title = "기타 입고 등록";
            detailOtherCaseDialog.Owner = Application.Current.MainWindow;
            detailOtherCaseDialog.BorderEffect = BorderEffect.Default;
            bool isDialog = (bool)detailOtherCaseDialog.ShowDialog();
            if (isDialog)
            {
                Refresh();
            }
        }

        //public ICommand ReportDialogCommand
        //{
        //    get
        //    {
        //        if (reportDialogCommand == null)
        //            reportDialogCommand = new DelegateCommand(ReportContact);
        //        return reportDialogCommand;
        //    }
        //}

        [Command]
        public void ReportContact()
        {
            try
            {
                int tmpIMP_ITM_AMT = 0;
                int tmpITM_QTY = 0;
                if (SelectMstList == null)
                {
                    return;
                }

                IList<InvVo> printDao = new List<InvVo>();
                if (SelectMstList != null)
                {
                    if (SelectMstList.Count > 0)
                    {
                        printDao = SelectMstList;

                        for (int x = 0; x < printDao.Count; x++)
                        {
                            printDao[x].GRP_NM = "[가입고 일자 (From) " + (StartDt).ToString("yyyy-MM-dd") + "~" + (EndDt).ToString("yyyy-MM-dd") + ", 사업장: " + M_SL_AREA_NM.CLSS_DESC + ", 창고: " + M_SL_LOC_NM.CLSS_DESC + "]";
                            tmpIMP_ITM_AMT += Convert.ToInt32(printDao[x].IMP_ITM_AMT);
                            tmpITM_QTY += Convert.ToInt32(printDao[x].ITM_QTY);

                            printDao[x].TMP_A_QTY = tmpIMP_ITM_AMT;
                            printDao[x].TMP_B_QTY = tmpITM_QTY;
                        }

                        I6610Report report = new I6610Report(printDao);
                        report.Margins.Top = 0;
                        report.Margins.Bottom = 0;
                        report.Margins.Left = 30;
                        report.Margins.Right = 0;
                        report.Landscape = true;
                        report.PrintingSystem.ShowPrintStatusDialog = true;
                        report.PaperKind = System.Drawing.Printing.PaperKind.A4;

                        var window = new DocumentPreviewWindow();
                        window.PreviewControl.DocumentSource = report;
                        report.CreateDocument(true);
                        window.Title = "품목입고 출력";
                        window.Owner = Application.Current.MainWindow;
                        window.ShowDialog();
                        //XtraReportPreviewModel model = new XtraReportPreviewModel(report);
                        //DocumentPreviewWindow window = new DocumentPreviewWindow() { Model = model };
                        //report.CreateDocument(true);
                        //window.Owner = Application.Current.MainWindow;
                        //window.Title = "품목가입고 출력";
                        //window.ShowDialog();
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public async void SYSTEM_CODE_VO()
        {
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

            //LocList = SystemProperties.SYSTEM_CODE_VO("P-008");
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "P-008"))
            {
                if (response.IsSuccessStatusCode)
                {
                    LocList = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                    if (LocList.Count > 0)
                    {
                        M_SL_LOC_NM = LocList[0];
                    }
                }
            }

            Refresh();
        }

    }
}
