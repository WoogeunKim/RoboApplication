using AquilaErpWpfApp3.Util;
using AquilaErpWpfApp3.View.M.Dialog;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.Core;
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

namespace AquilaErpWpfApp3.ViewModel
{
    //���� ����
    public sealed class M6720ViewModel : ViewModelBase, INotifyPropertyChanged
    {

        private string _title = "������Ȳ";

        //private static ManServiceClient manClient = SystemProperties.ManClient;

        private IList<ManVo> selectedMasterViewList;
        //private IList<ManVo> selectedPopup1ViewList;
        //private IList<ManVo> selectedPopup2ViewList;

        ////Master Dialog
        //private ICommand masterSearchDialogCommand;
        //private ICommand masterNewDialogCommand;
        //private ICommand masterEditDialogCommand;
        //private ICommand masterDelDialogCommand;
        ////
        //private M6710MasterDialog masterDialog;
        //private M6710DetailBadDialog badDialog;
        //private M6710DetailIdleDialog idleDialog;

        public M6720ViewModel() 
        {
            StartDt = System.DateTime.Now;
            EndDt = System.DateTime.Now;

            SYSTEM_CODE_VO();
            //Refresh();
        }

        [Command]
       public async void Refresh()
        {
            try
            {

                if (DXSplashScreen.IsActive == false)
                {
                    DXSplashScreen.Show<ProgressWindow>();
                }

                ManVo vo = new ManVo();
                vo.FM_DT = (StartDt).ToString("yyyy-MM-dd");
                vo.TO_DT = (EndDt).ToString("yyyy-MM-dd");
                vo.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
                if (M_PROD_ROUT_NM != null) vo.ROUT_CD = M_PROD_ROUT_NM.ROUT_CD;

                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6720/mst", new StringContent(JsonConvert.SerializeObject(vo), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectedMasterViewList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();

                        //SelectedMasterViewList = manClient.M6625SelectMaster(new ManVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD });
                        if (SelectedMasterViewList.Count > 0)
                        {
                            isM_UPDATE = true;
                            isM_DELETE = true;

                            //isM_REPORT = true;
                            //if (string.IsNullOrEmpty(_PCK_PLST_CLSS_CD_PCK_PLST_CD))
                            //{
                            SelectedMasterItem = SelectedMasterViewList[0];
                            //}
                            //else
                            //{
                            //    SelectedMasterItem = SelectedMasterViewList.Where(x => (x.PCK_PLST_CLSS_CD + "_" + x.PCK_PLST_CD).Equals(_PCK_PLST_CLSS_CD_PCK_PLST_CD)).LastOrDefault<ManVo>();
                            //}
                        }
                        else
                        {
                            isM_UPDATE = false;
                            isM_DELETE = false;

                            // isM_REPORT = false;
                        }


                        if (DXSplashScreen.IsActive == true)
                        {
                            DXSplashScreen.Close();
                        }
                    }
                }
            }
            catch (System.Exception eLog)
            {
                if (DXSplashScreen.IsActive == true)
                {
                    DXSplashScreen.Close();
                }
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }

        #region Functon (Master Add, Edit, Del)
        public IList<ManVo> SelectedMasterViewList
        {
            get { return selectedMasterViewList; }
            private set { SetProperty(ref selectedMasterViewList, value, () => SelectedMasterViewList); }
        }

        ManVo _selectMasterItem;
        public ManVo SelectedMasterItem
        {
            get
            {
                return _selectMasterItem;
            }
            set
            {
                if (value != null)
                {
                    SetProperty(ref _selectMasterItem, value, () => SelectedMasterItem, OnSelectedMasterItemChanged);
                }
            }
        }


        ////�ҷ���Ȳ
        //public IList<ManVo> SelectedPopup1ViewList
        //{
        //    get { return selectedPopup1ViewList; }
        //    private set { SetProperty(ref selectedPopup1ViewList, value, () => SelectedPopup1ViewList); }
        //}

        ////�񰡵� ��Ȳ
        //public IList<ManVo> SelectedPopup2ViewList
        //{
        //    get { return selectedPopup2ViewList; }
        //    private set { SetProperty(ref selectedPopup2ViewList, value, () => SelectedPopup2ViewList); }
        //}

        //public IList<ManVo> SelectedPopupViewList
        //{
        //    get { return selectedPopupViewList; }
        //    private set { SetProperty(ref selectedPopupViewList, value, () => SelectedPopupViewList); }
        //}

        //ManVo _selectPopupItem;
        //public ManVo SelectedPopupItem
        //{
        //    get
        //    {
        //        return _selectPopupItem;
        //    }
        //    set
        //    {
        //        if (value != null)
        //        {
        //            SetProperty(ref _selectPopupItem, value, () => SelectedPopupItem);
        //        }
        //    }
        //}

        async void OnSelectedMasterItemChanged()
        {
            try
            {
                //if (this.SelectedMasterItem == null) { return; }

                ////�ҷ� ��Ȳ
                //using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6710/dtl2", new StringContent(JsonConvert.SerializeObject(this.SelectedMasterItem), System.Text.Encoding.UTF8, "application/json")))
                //{
                //    if (response.IsSuccessStatusCode)
                //    {
                //        this.SelectedPopup1ViewList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
                //    }

                //}

                ////�񰡵� ��Ȳ
                //using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6710/dtl1", new StringContent(JsonConvert.SerializeObject(this.SelectedMasterItem), System.Text.Encoding.UTF8, "application/json")))
                //{
                //    if (response.IsSuccessStatusCode)
                //    {
                //        this.SelectedPopup2ViewList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
                //    }

                //}
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }

        //public ICommand MasterSearchDialogCommand
        //{
        //    get
        //    {
        //        if (masterSearchDialogCommand == null)
        //            masterSearchDialogCommand = new DelegateCommand(Refresh);
        //        return masterSearchDialogCommand;
        //    }
        //}

        //public void SearchMasterContact()
        //{
        //    ProgressWindow program = new ProgressWindow();
        //    try
        //    {
        //        ThreadStart start = delegate()
        //        {
        //            program.Dispatcher.Invoke(DispatcherPriority.Normal,(Action)(() => 
        //            {
        //                program.Show();
        //                Thread.Sleep(100);
        //                SelectedMasterViewList = codeClient.SelectMasterCode(new SystemCodeVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD });
        //                OnSelectedMasterItemChanged();
        //                program.Close();
        //            }));
        //        };
        //        new Thread(start).Start();
        //    }
        //    catch (System.Exception eLog)
        //    {
        //    //    program.Close();
        //        WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]ǥ�� ���� ����", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
        //        return;
        //    }
        //}

        //public ICommand MasterNewDialogCommand
        //{
        //    get
        //    {
        //        if (masterNewDialogCommand == null)
        //            masterNewDialogCommand = new DelegateCommand(NewMasterContact);
        //        return masterNewDialogCommand;
        //    }
        //}
        [Command]
        public void NewMasterContact()
        {
            //masterDialog = new M6710MasterDialog(new ManVo());
            //masterDialog.Title = _title + " - �߰�";
            //masterDialog.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            //masterDialog.Owner = Application.Current.MainWindow;
            //masterDialog.BorderEffect = BorderEffect.Default;
            ////masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            ////masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            //bool isDialog = (bool)masterDialog.ShowDialog();
            //if (isDialog)
            //{
            //    Refresh();



            //    //    // Refresh(masterDialog.resultDomain.PCK_PLST_CLSS_CD + "_" + masterDialog.resultDomain.PCK_PLST_CD);

            //    //    //if (masterDialog.IsEdit == false)
            //    //    //{
            //    //    //    Refresh();

            //    //    //    for (int x = 0; x < SelectedMasterViewList.Count; x++)
            //    //    //    {
            //    //    //        if ((masterDialog.resultDomain.PCK_PLST_CLSS_CD + "_" + masterDialog.resultDomain.PCK_PLST_CD).Equals(SelectedMasterViewList[x].PCK_PLST_CLSS_CD + "_" + SelectedMasterViewList[x].PCK_PLST_CD))
            //    //    //        {
            //    //    //            SelectedMasterItem = SelectedMasterViewList[x];
            //    //    //            break;
            //    //    //        }
            //    //    //    }
            //    //    //}
            //}


        }

        //public ICommand MasterEditDialogCommand
        //{
        //    get
        //    {
        //        if (masterEditDialogCommand == null)
        //            masterEditDialogCommand = new DelegateCommand(EditMasterContact);
        //        return masterEditDialogCommand;
        //    }
        //}
        [Command]
        public void EditMasterContact()
        {
            //if (this._selectMasterItem == null) { return; }
            //ManVo editDao = this._selectMasterItem;
            //if (editDao != null)
            //{
            //    masterDialog = new M665101MasterDialog(editDao);
            //    masterDialog.Title = _title + " - ����";
            //    masterDialog.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            //    masterDialog.BorderEffect = BorderEffect.Default;
            //    masterDialog.Owner = Application.Current.MainWindow;
            //    masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            //    masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            //    bool isDialog = (bool)masterDialog.ShowDialog();
            //    if (isDialog)
            //    {
            //        //Refresh(masterDialog.resultDomain.PCK_PLST_CLSS_CD + "_" + masterDialog.resultDomain.PCK_PLST_CD);
            //        //if (masterDialog.IsEdit == false)
            //        //{
            //        //    Refresh();
            //        //}
            //    }
            //}
        }

        //public ICommand MasterDelDialogCommand
        //{
        //    get
        //    {
        //        if (masterDelDialogCommand == null)
        //            masterDelDialogCommand = new DelegateCommand(DelMasterContact);
        //        return masterDelDialogCommand;
        //    }
        //}

        [Command]
        public async void DelMasterContact()
        {
            //try
            //{
            //    ManVo delDao = this._selectMasterItem;
            //    if (delDao != null)
            //    {
            //        MessageBoxResult result = WinUIMessageBox.Show("(" + delDao.PROD_PLN_DT + " / " + delDao.LOT_DIV_NO + " / " + delDao.ROUT_NM + ")  ������ ���� �Ͻðڽ��ϱ�?", _title, MessageBoxButton.YesNo, MessageBoxImage.Question);
            //        if (result == MessageBoxResult.Yes)
            //        {

            //            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6710/mst/d", new StringContent(JsonConvert.SerializeObject(delDao), System.Text.Encoding.UTF8, "application/json")))
            //            {
            //                if (response.IsSuccessStatusCode)
            //                {
            //                    int _Num = 0;
            //                    string resultMsg = await response.Content.ReadAsStringAsync();
            //                    if (int.TryParse(resultMsg, out _Num) == false)
            //                    {
            //                        //����
            //                        WinUIMessageBox.Show(resultMsg, _title, MessageBoxButton.OK, MessageBoxImage.Error);
            //                        return;
            //                    }
            //                }
            //            }
            //            Refresh();

            //            //����
            //            WinUIMessageBox.Show("������ �Ϸ�Ǿ����ϴ�.", _title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
            //        }
            //    }
            //}
            //catch (System.Exception eLog)
            //{
            //    WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
            //    return;
            //}
        }



        ////�ҷ� ��Ȳ
        //[Command]
        //public async void Bad()
        //{
        //    try
        //    {
        //        ManVo badDao = this._selectMasterItem;
        //        if (badDao != null)
        //        {
        //            //
        //            badDialog = new M6710DetailBadDialog(badDao);
        //            badDialog.Title = _title + " - �ҷ� ��Ȳ";
        //            badDialog.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        //            badDialog.BorderEffect = BorderEffect.Default;
        //            badDialog.Owner = Application.Current.MainWindow;
        //            //badDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
        //            //badDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
        //            bool isDialog = (bool)badDialog.ShowDialog();
        //            if (isDialog)
        //            {
        //                OnSelectedMasterItemChanged();
        //                //        //Refresh(masterDialog.resultDomain.PCK_PLST_CLSS_CD + "_" + masterDialog.resultDomain.PCK_PLST_CD);
        //                //        //if (masterDialog.IsEdit == false)
        //                //        //{
        //                //        //    Refresh();
        //                //        //}
        //            }
        //        }
        //    }
        //    catch (System.Exception eLog)
        //    {
        //        WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
        //        return;
        //    }
        //}

        ////�񰡵� ��Ȳ
        //[Command]
        //public async void Idle()
        //{
        //    try
        //    {
        //        ManVo badDao = this._selectMasterItem;
        //        if (badDao != null)
        //        {
        //            //
        //            idleDialog = new M6710DetailIdleDialog(badDao);
        //            idleDialog.Title = _title + " - �񰡵� ��Ȳ";
        //            idleDialog.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        //            idleDialog.BorderEffect = BorderEffect.Default;
        //            idleDialog.Owner = Application.Current.MainWindow;
        //            //badDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
        //            //badDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
        //            bool isDialog = (bool)idleDialog.ShowDialog();
        //            if (isDialog)
        //            {
        //                OnSelectedMasterItemChanged();
        //                //        //Refresh(masterDialog.resultDomain.PCK_PLST_CLSS_CD + "_" + masterDialog.resultDomain.PCK_PLST_CD);
        //                //        //if (masterDialog.IsEdit == false)
        //                //        //{
        //                //        //    Refresh();
        //                //        //}
        //            }
        //        }
        //    }
        //    catch (System.Exception eLog)
        //    {
        //        WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
        //        return;
        //    }
        //}
        #endregion


        //[Command]
        //public async void OpenReportViewContact()
        //{
        //    isPRNT_PLN = true;


        //    OnSelectedMasterItemChanged();
        //}

        //[Command]
        //public async void ReportContact()
        //{
        //    try
        //    {
        //        if (this.SelectedMasterItem == null) { return; }

        //        MessageBoxResult result = WinUIMessageBox.Show("[" + this.SelectedMasterItem.PROD_EQ_NO + " / " + this.SelectedPopupItem.PROD_PLN_DT + " / " + this.SelectedPopupItem.PROD_PLN_QTY + "] ���� ���� �Ͻðڽ��ϱ�?", _title, MessageBoxButton.YesNo, MessageBoxImage.Question);
        //        if (result == MessageBoxResult.Yes)
        //        {

        //            List<ManVo> _paramList = new List<ManVo>();

        //            ManVo _param = new ManVo();
        //            _param.PROD_LOC_CD = this.SelectedMasterItem.PROD_LOC_CD;
        //            _param.WKY_YRMON = this.SelectedMasterItem.WKY_YRMON;
        //            _param.WK = this.SelectedMasterItem.WK;
        //            _param.CMPO_CD = this.SelectedMasterItem.CMPO_CD;
        //            _param.PROD_EQ_NO = this.SelectedMasterItem.PROD_EQ_NO;
        //            _param.CRE_USR_ID = SystemProperties.USER_VO.USR_ID;
        //            _param.UPD_USR_ID = SystemProperties.USER_VO.USR_ID;
        //            _param.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
        //            _param.RN = this.SelectedPopupItem.RN;
        //            _param.PROD_PLN_QTY = this.SelectedPopupItem.PROD_PLN_QTY;
        //            _param.INP_STAFF_VAL = 0;
        //            _param.MM_RMK = this.SelectedPopupItem.MM_RMK;
        //            _param.PCK_FLG = "IN";
        //            _param.A_ROUT_CD = new string[] { "IN" };

        //            //
        //            _paramList.Add(_param);


        //            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m665101/wrk", new StringContent(JsonConvert.SerializeObject(_paramList), System.Text.Encoding.UTF8, "application/json")))
        //            {
        //                if (response.IsSuccessStatusCode)
        //                {
        //                    //��¹�
        //                    List<ManVo>  _reportList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();

        //                    //_reportList[0].FM_DT = System.DateTime.Now.ToString("yyyy-MM-dd");
        //                    _reportList[0].FM_DT = this.SelectedPopupItem.PROD_PLN_DT;
        //                    _reportList[0].MM_01 = "����";

        //                    //
        //                    M665101Report report = new M665101Report(_reportList);
        //                    report.Margins.Top = 20;
        //                    report.Margins.Bottom = 20;
        //                    report.Margins.Left = 50;
        //                    report.Margins.Right = 20;
        //                    report.Landscape = false;

        //                    report.PrintingSystem.ShowPrintStatusDialog = true;
        //                    report.PaperKind = System.Drawing.Printing.PaperKind.A4;
        //                    report.Watermark.Text = Properties.Settings.Default.SettingCompany;
        //                    report.Watermark.TextDirection = DevExpress.XtraPrinting.Drawing.DirectionMode.ForwardDiagonal;
        //                    report.Watermark.Font = new System.Drawing.Font(report.PrintingSystem.Watermark.Font.FontFamily, 40);
        //                    report.Watermark.ForeColor = System.Drawing.Color.PaleTurquoise;
        //                    report.Watermark.TextTransparency = 150;


        //                    var window = new DevExpress.Xpf.Printing.DocumentPreviewWindow();
        //                    window.PreviewControl.DocumentSource = report;
        //                    report.CreateDocument(true);
        //                    window.Title = _title;
        //                    window.Owner = Application.Current.MainWindow;
        //                    window.ShowDialog();

        //                    //��� ���� ������Ʈ
        //                    //SearchDetailJob.UPD_USR_ID = SystemProperties.USER;
        //                    //using (HttpResponseMessage responseY = await SystemProperties.PROGRAM_HTTP.PostAsync("m66520/mst/u", new StringContent(JsonConvert.SerializeObject(SearchDetailJob), System.Text.Encoding.UTF8, "application/json")))
        //                    //{
        //                    //}
        //                }

        //                    //if (response.IsSuccessStatusCode)
        //                    //{
        //                    //    string resultMsg = await response.Content.ReadAsStringAsync();
        //                    //    if (int.TryParse(resultMsg, out _Num) == false)
        //                    //    {
        //                    //        //����
        //                    //        WinUIMessageBox.Show(resultMsg, _title, MessageBoxButton.OK, MessageBoxImage.Error);
        //                    //        return;
        //                    //    }

        //                    //    //����
        //                    //    WinUIMessageBox.Show("�Ϸ� �Ǿ����ϴ�", _title, MessageBoxButton.OK, MessageBoxImage.Information);
        //                    //}
        //                }
        //        }

        //    }
        //    catch (System.Exception eLog)
        //    {
        //        WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
        //        return;
        //    }
        //}

        //[Command]
        //public async void CloseReportViewContact()
        //{
        //    isPRNT_PLN = false;
        //}


        public async void SYSTEM_CODE_VO()
        {
            ////�����
            //using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "A-001"))
            //{
            //    if (response.IsSuccessStatusCode)
            //    {
            //        ProdLocList = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
            //        if (ProdLocList.Count > 0)
            //        {
            //            //ProdLocList.Insert(0, new SystemCodeVo() { CLSS_CD = null, CLSS_DESC = "��ü" });
            //            M_PROD_LOC_NM = ProdLocList[0];
            //        }
            //    }
            //}



            //SL_AREA_LIST = SystemProperties.SYSTEM_CODE_VO("L-002");
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "L-002"))
            {
                if (response.IsSuccessStatusCode)
                {
                    SL_AREA_LIST = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                    if (SL_AREA_LIST.Count > 0)
                    {
                        M_SL_AREA_NM = SL_AREA_LIST[0];
                    }
                }
            }

            // ����
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6611", new StringContent(JsonConvert.SerializeObject(new ManVo() { DELT_FLG = "N", CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    IList<ManVo> _tempRoutList = new List<ManVo>();
                    _tempRoutList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
                    _tempRoutList.Insert(0, new ManVo() { ROUT_NM = "��ü" });

                    this.PROD_ROUT_LIST = _tempRoutList;
                    this.M_PROD_ROUT_NM = _tempRoutList[0];
                }
            }


            //using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("M665100", new StringContent(JsonConvert.SerializeObject(new ManVo() { DELT_FLG = "N", CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
            //{
            //    if (response.IsSuccessStatusCode)
            //    {
            //        PlnYrmonList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
            //        if (PlnYrmonList.Count > 0)
            //        {
            //            //CoNmList.Insert(0, new SystemCodeVo() { CO_NM = "��ü", CO_NO = null });
            //            //M_PLN_YRMON = PlnYrmonList[PlnYrmonList.Count - 1];
            //            M_PLN_YRMON = PlnYrmonList.Where<ManVo>(w => w.WKY_YRMON.Equals(System.DateTime.Now.ToString("yyyyMM"))).FirstOrDefault<ManVo>();
            //        }
            //    }
            //}

            //Refresh();
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

        private IList<SystemCodeVo> _AreaCd = new List<SystemCodeVo>();
        public IList<SystemCodeVo> SL_AREA_LIST
        {
            get { return _AreaCd; }
            set { SetProperty(ref _AreaCd, value, () => SL_AREA_LIST); }
        }

        private SystemCodeVo _M_SL_AREA_NM;
        public SystemCodeVo M_SL_AREA_NM
        {
            get { return _M_SL_AREA_NM; }
            set { SetProperty(ref _M_SL_AREA_NM, value, () => M_SL_AREA_NM); }
        }

        private IList<ManVo> _RoutCdList = new List<ManVo>();
        public IList<ManVo> PROD_ROUT_LIST
        {
            get { return _RoutCdList; }
            set { SetProperty(ref _RoutCdList, value, () => PROD_ROUT_LIST); }
        }
        private ManVo _M_PROD_ROUT_NM;
        public ManVo M_PROD_ROUT_NM
        {
            get { return _M_PROD_ROUT_NM; }
            set { SetProperty(ref _M_PROD_ROUT_NM, value, () => M_PROD_ROUT_NM); }
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

        private string _M_SEARCH_TEXT = string.Empty;
        public string M_SEARCH_TEXT
        {
            get { return _M_SEARCH_TEXT; }
            set { SetProperty(ref _M_SEARCH_TEXT, value, () => M_SEARCH_TEXT); }
        }


        ////TITLE
        //private string _N1ST_PLN_TITLE = string.Empty;
        //public string N1ST_PLN_TITLE
        //{
        //    get { return _N1ST_PLN_TITLE; }
        //    set { SetProperty(ref _N1ST_PLN_TITLE, value, () => N1ST_PLN_TITLE); }
        //}
        //private string _N2ND_PLN_TITLE = string.Empty;
        //public string N2ND_PLN_TITLE
        //{
        //    get { return _N2ND_PLN_TITLE; }
        //    set { SetProperty(ref _N2ND_PLN_TITLE, value, () => N2ND_PLN_TITLE); }
        //}
        //private string _N3RD_PLN_TITLE = string.Empty;
        //public string N3RD_PLN_TITLE
        //{
        //    get { return _N3RD_PLN_TITLE; }
        //    set { SetProperty(ref _N3RD_PLN_TITLE, value, () => N3RD_PLN_TITLE); }
        //}
        //private string _N4TH_PLN_TITLE = string.Empty;
        //public string N4TH_PLN_TITLE
        //{
        //    get { return _N4TH_PLN_TITLE; }
        //    set { SetProperty(ref _N4TH_PLN_TITLE, value, () => N4TH_PLN_TITLE); }
        //}
        //private string _N5TH_PLN_TITLE = string.Empty;
        //public string N5TH_PLN_TITLE
        //{
        //    get { return _N5TH_PLN_TITLE; }
        //    set { SetProperty(ref _N5TH_PLN_TITLE, value, () => N5TH_PLN_TITLE); }
        //}
        //private string _N6TH_PLN_TITLE = string.Empty;
        //public string N6TH_PLN_TITLE
        //{
        //    get { return _N6TH_PLN_TITLE; }
        //    set { SetProperty(ref _N6TH_PLN_TITLE, value, () => N6TH_PLN_TITLE); }
        //}
        //private string _N7TH_PLN_TITLE = string.Empty;
        //public string N7TH_PLN_TITLE
        //{
        //    get { return _N7TH_PLN_TITLE; }
        //    set { SetProperty(ref _N7TH_PLN_TITLE, value, () => N7TH_PLN_TITLE); }
        //}
        //
        ////�۾����ü� â - Ȱ��
        //private bool? _isPRNT_PLN = false;
        //public bool? isPRNT_PLN
        //{
        //    get { return _isPRNT_PLN; }
        //    set { SetProperty(ref _isPRNT_PLN, value, () => isPRNT_PLN); }
        //}

        //private bool? _isM_REPORT = false;
        //public bool? isM_REPORT
        //{
        //    get { return _isM_REPORT; }
        //    set { SetProperty(ref _isM_REPORT, value, () => isM_REPORT); }
        //}


    }
}
