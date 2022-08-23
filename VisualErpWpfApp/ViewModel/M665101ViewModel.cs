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
using System.Windows.Media;
using AquilaErpWpfApp3.Util;
using AquilaErpWpfApp3.View.M.Dialog;
using ModelsLibrary.Code;
using System;
using AquilaErpWpfApp3.View.M.Report;

namespace AquilaErpWpfApp3.ViewModel
{
    //설비 관리
    public sealed class M665101ViewModel : ViewModelBase, INotifyPropertyChanged
    {

        private string _title = "주간 생산계획";

        //private static ManServiceClient manClient = SystemProperties.ManClient;

        private IList<ManVo> selectedMasterViewList;
        private IList<ManVo> selectedPopupViewList;

        ////Master Dialog
        //private ICommand masterSearchDialogCommand;
        //private ICommand masterNewDialogCommand;
        //private ICommand masterEditDialogCommand;
        //private ICommand masterDelDialogCommand;
        ////
        private M665101MasterDialog masterDialog;


        public M665101ViewModel() 
        {
            SYSTEM_CODE_VO();
            //Refresh();
        }

        [Command]
       public async void Refresh()
        {
            try
            {
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m665101", new StringContent(JsonConvert.SerializeObject(new ManVo() { WK = M_PLN_YRMON?.WK, WKY_YRMON = M_PLN_YRMON?.WKY_YRMON, PROD_LOC_CD = M_PROD_LOC_NM?.CLSS_CD, CHNL_CD = SystemProperties.USER_VO.CHNL_CD, A_ROUT_CD = new string[] { "IN" } }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectedMasterViewList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
                    }
                    //SelectedMasterViewList = manClient.M6625SelectMaster(new ManVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD });
                    if (SelectedMasterViewList.Count > 0)
                    {
                        isM_UPDATE = true;
                        isM_DELETE = true;

                        isM_REPORT = true;


                        this.N1ST_PLN_TITLE = Convert.ToDateTime(M_PLN_YRMON.FM_DT).AddDays(0).ToString("yyyy-MM-dd");
                        this.N2ND_PLN_TITLE = Convert.ToDateTime(M_PLN_YRMON.FM_DT).AddDays(1).ToString("yyyy-MM-dd");
                        this.N3RD_PLN_TITLE = Convert.ToDateTime(M_PLN_YRMON.FM_DT).AddDays(2).ToString("yyyy-MM-dd");
                        this.N4TH_PLN_TITLE = Convert.ToDateTime(M_PLN_YRMON.FM_DT).AddDays(3).ToString("yyyy-MM-dd");
                        this.N5TH_PLN_TITLE = Convert.ToDateTime(M_PLN_YRMON.FM_DT).AddDays(4).ToString("yyyy-MM-dd");
                        this.N6TH_PLN_TITLE = Convert.ToDateTime(M_PLN_YRMON.FM_DT).AddDays(5).ToString("yyyy-MM-dd");
                        this.N7TH_PLN_TITLE = Convert.ToDateTime(M_PLN_YRMON.FM_DT).AddDays(6).ToString("yyyy-MM-dd");



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

                        isM_REPORT = false;
                    }
                }
            }
            catch (System.Exception eLog)
            {
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

        public IList<ManVo> SelectedPopupViewList
        {
            get { return selectedPopupViewList; }
            private set { SetProperty(ref selectedPopupViewList, value, () => SelectedPopupViewList); }
        }

        ManVo _selectPopupItem;
        public ManVo SelectedPopupItem
        {
            get
            {
                return _selectPopupItem;
            }
            set
            {
                if (value != null)
                {
                    SetProperty(ref _selectPopupItem, value, () => SelectedPopupItem);
                }
            }
        }

        void OnSelectedMasterItemChanged()
        {
            try
            {
                if (this.SelectedMasterItem == null) { return; }

                this.SelectedPopupViewList = new List<ManVo>();

                SelectedPopupViewList.Add(new ManVo() { MM_RMK = " ", RN = 1, PROD_EQ_NO = SelectedMasterItem.PROD_EQ_NO, MDL_CD = SelectedMasterItem.MDL_CD, MDL_NM = SelectedMasterItem.MDL_NM, CMPO_CD = SelectedMasterItem.CMPO_CD, CMPO_NM = SelectedMasterItem.CMPO_NM, ITM_SZ_NM = SelectedMasterItem.ITM_SZ_NM, N1ST_ITM_GRP_NM = SelectedMasterItem.N1ST_ITM_GRP_NM, N1ST_ITM_GRP_CD = SelectedMasterItem.N1ST_ITM_GRP_CD, CO_CD = SelectedMasterItem.CO_CD, CO_NM = SelectedMasterItem.CO_NM, PROD_PLN_DT = this.N1ST_PLN_TITLE, PROD_PLN_QTY = SelectedMasterItem.N1ST_PLN_QTY }); ;
                SelectedPopupViewList.Add(new ManVo() { MM_RMK = " ", RN = 2, PROD_EQ_NO = SelectedMasterItem.PROD_EQ_NO, MDL_CD = SelectedMasterItem.MDL_CD, MDL_NM = SelectedMasterItem.MDL_NM, CMPO_CD = SelectedMasterItem.CMPO_CD, CMPO_NM = SelectedMasterItem.CMPO_NM, ITM_SZ_NM = SelectedMasterItem.ITM_SZ_NM, N1ST_ITM_GRP_NM = SelectedMasterItem.N1ST_ITM_GRP_NM, N1ST_ITM_GRP_CD = SelectedMasterItem.N1ST_ITM_GRP_CD, CO_CD = SelectedMasterItem.CO_CD, CO_NM = SelectedMasterItem.CO_NM, PROD_PLN_DT = this.N2ND_PLN_TITLE, PROD_PLN_QTY = SelectedMasterItem.N2ND_PLN_QTY });
                SelectedPopupViewList.Add(new ManVo() { MM_RMK = " ", RN = 3, PROD_EQ_NO = SelectedMasterItem.PROD_EQ_NO, MDL_CD = SelectedMasterItem.MDL_CD, MDL_NM = SelectedMasterItem.MDL_NM, CMPO_CD = SelectedMasterItem.CMPO_CD, CMPO_NM = SelectedMasterItem.CMPO_NM, ITM_SZ_NM = SelectedMasterItem.ITM_SZ_NM, N1ST_ITM_GRP_NM = SelectedMasterItem.N1ST_ITM_GRP_NM, N1ST_ITM_GRP_CD = SelectedMasterItem.N1ST_ITM_GRP_CD, CO_CD = SelectedMasterItem.CO_CD, CO_NM = SelectedMasterItem.CO_NM, PROD_PLN_DT = this.N3RD_PLN_TITLE, PROD_PLN_QTY = SelectedMasterItem.N3RD_PLN_QTY });
                SelectedPopupViewList.Add(new ManVo() { MM_RMK = " ", RN = 4, PROD_EQ_NO = SelectedMasterItem.PROD_EQ_NO, MDL_CD = SelectedMasterItem.MDL_CD, MDL_NM = SelectedMasterItem.MDL_NM, CMPO_CD = SelectedMasterItem.CMPO_CD, CMPO_NM = SelectedMasterItem.CMPO_NM, ITM_SZ_NM = SelectedMasterItem.ITM_SZ_NM, N1ST_ITM_GRP_NM = SelectedMasterItem.N1ST_ITM_GRP_NM, N1ST_ITM_GRP_CD = SelectedMasterItem.N1ST_ITM_GRP_CD, CO_CD = SelectedMasterItem.CO_CD, CO_NM = SelectedMasterItem.CO_NM, PROD_PLN_DT = this.N4TH_PLN_TITLE, PROD_PLN_QTY = SelectedMasterItem.N4TH_PLN_QTY });
                SelectedPopupViewList.Add(new ManVo() { MM_RMK = " ", RN = 5, PROD_EQ_NO = SelectedMasterItem.PROD_EQ_NO, MDL_CD = SelectedMasterItem.MDL_CD, MDL_NM = SelectedMasterItem.MDL_NM, CMPO_CD = SelectedMasterItem.CMPO_CD, CMPO_NM = SelectedMasterItem.CMPO_NM, ITM_SZ_NM = SelectedMasterItem.ITM_SZ_NM, N1ST_ITM_GRP_NM = SelectedMasterItem.N1ST_ITM_GRP_NM, N1ST_ITM_GRP_CD = SelectedMasterItem.N1ST_ITM_GRP_CD, CO_CD = SelectedMasterItem.CO_CD, CO_NM = SelectedMasterItem.CO_NM, PROD_PLN_DT = this.N5TH_PLN_TITLE, PROD_PLN_QTY = SelectedMasterItem.N5TH_PLN_QTY });
                SelectedPopupViewList.Add(new ManVo() { MM_RMK = " ", RN = 6, PROD_EQ_NO = SelectedMasterItem.PROD_EQ_NO, MDL_CD = SelectedMasterItem.MDL_CD, MDL_NM = SelectedMasterItem.MDL_NM, CMPO_CD = SelectedMasterItem.CMPO_CD, CMPO_NM = SelectedMasterItem.CMPO_NM, ITM_SZ_NM = SelectedMasterItem.ITM_SZ_NM, N1ST_ITM_GRP_NM = SelectedMasterItem.N1ST_ITM_GRP_NM, N1ST_ITM_GRP_CD = SelectedMasterItem.N1ST_ITM_GRP_CD, CO_CD = SelectedMasterItem.CO_CD, CO_NM = SelectedMasterItem.CO_NM, PROD_PLN_DT = this.N6TH_PLN_TITLE, PROD_PLN_QTY = SelectedMasterItem.N6TH_PLN_QTY });
                SelectedPopupViewList.Add(new ManVo() { MM_RMK = " ", RN = 7, PROD_EQ_NO = SelectedMasterItem.PROD_EQ_NO, MDL_CD = SelectedMasterItem.MDL_CD, MDL_NM = SelectedMasterItem.MDL_NM, CMPO_CD = SelectedMasterItem.CMPO_CD, CMPO_NM = SelectedMasterItem.CMPO_NM, ITM_SZ_NM = SelectedMasterItem.ITM_SZ_NM, N1ST_ITM_GRP_NM = SelectedMasterItem.N1ST_ITM_GRP_NM, N1ST_ITM_GRP_CD = SelectedMasterItem.N1ST_ITM_GRP_CD, CO_CD = SelectedMasterItem.CO_CD, CO_NM = SelectedMasterItem.CO_NM, PROD_PLN_DT = this.N7TH_PLN_TITLE, PROD_PLN_QTY = SelectedMasterItem.N7TH_PLN_QTY });
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
        //        WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]표준 공정 관리", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
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
            masterDialog = new M665101MasterDialog(new ManVo() { GBN = M_PLN_YRMON?.GBN, WK = M_PLN_YRMON?.WK, WKY_YRMON = M_PLN_YRMON?.WKY_YRMON, PROD_LOC_CD = M_PROD_LOC_NM?.CLSS_CD });
            masterDialog.Title = _title + " - 추가";
            masterDialog.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            masterDialog.Owner = Application.Current.MainWindow;
            masterDialog.BorderEffect = BorderEffect.Default;
            masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            bool isDialog = (bool)masterDialog.ShowDialog();
            if (isDialog)
            {
                Refresh();



                // Refresh(masterDialog.resultDomain.PCK_PLST_CLSS_CD + "_" + masterDialog.resultDomain.PCK_PLST_CD);

                //if (masterDialog.IsEdit == false)
                //{
                //    Refresh();

                //    for (int x = 0; x < SelectedMasterViewList.Count; x++)
                //    {
                //        if ((masterDialog.resultDomain.PCK_PLST_CLSS_CD + "_" + masterDialog.resultDomain.PCK_PLST_CD).Equals(SelectedMasterViewList[x].PCK_PLST_CLSS_CD + "_" + SelectedMasterViewList[x].PCK_PLST_CD))
                //        {
                //            SelectedMasterItem = SelectedMasterViewList[x];
                //            break;
                //        }
                //    }
                //}
            }


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
            //    masterDialog.Title = _title + " - 수정";
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
            try
            {
                ManVo delDao = this._selectMasterItem;
                if (delDao != null)
                {
                    MessageBoxResult result = WinUIMessageBox.Show(delDao.PROD_EQ_NO + " / " + delDao.WKY_YRMON + " / " + delDao.WK + " 정말로 삭제 하시겠습니까?", _title, MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        for (int x = 1; x <= 7; x++)
                        {
                            delDao.RN = x;
                            delDao.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
                            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m665101/d", new StringContent(JsonConvert.SerializeObject(delDao), System.Text.Encoding.UTF8, "application/json")))
                            {
                                if (response.IsSuccessStatusCode)
                                {
                                    int _Num = 0;
                                    string resultMsg = await response.Content.ReadAsStringAsync();
                                    if (int.TryParse(resultMsg, out _Num) == false)
                                    {
                                        //실패
                                        WinUIMessageBox.Show(resultMsg, _title, MessageBoxButton.OK, MessageBoxImage.Error);
                                        return;
                                    }
                                }
                            }
                        }
                        Refresh();

                        //성공
                        WinUIMessageBox.Show("삭제가 완료되었습니다.", _title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                    }
                }
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }
        #endregion


        [Command]
        public async void OpenReportViewContact()
        {
            isPRNT_PLN = true;


            OnSelectedMasterItemChanged();
        }

        [Command]
        public async void ReportContact()
        {
            try
            {
                if (this.SelectedMasterItem == null) { return; }

                MessageBoxResult result = WinUIMessageBox.Show("[" + this.SelectedMasterItem.PROD_EQ_NO + " / " + this.SelectedPopupItem.PROD_PLN_DT + " / " + this.SelectedPopupItem.PROD_PLN_QTY + "] 작지 발행 하시겠습니까?", _title, MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {

                    List<ManVo> _paramList = new List<ManVo>();

                    ManVo _param = new ManVo();
                    _param.PROD_LOC_CD = this.SelectedMasterItem.PROD_LOC_CD;
                    _param.WKY_YRMON = this.SelectedMasterItem.WKY_YRMON;
                    _param.WK = this.SelectedMasterItem.WK;
                    _param.CMPO_CD = this.SelectedMasterItem.CMPO_CD;
                    _param.PROD_EQ_NO = this.SelectedMasterItem.PROD_EQ_NO;
                    _param.CRE_USR_ID = SystemProperties.USER_VO.USR_ID;
                    _param.UPD_USR_ID = SystemProperties.USER_VO.USR_ID;
                    _param.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
                    _param.RN = this.SelectedPopupItem.RN;
                    _param.PROD_PLN_QTY = this.SelectedPopupItem.PROD_PLN_QTY;
                    _param.INP_STAFF_VAL = 0;
                    _param.MM_RMK = this.SelectedPopupItem.MM_RMK;
                    _param.PCK_FLG = "IN";
                    _param.A_ROUT_CD = new string[] { "IN" };

                    //
                    _paramList.Add(_param);


                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m665101/wrk", new StringContent(JsonConvert.SerializeObject(_paramList), System.Text.Encoding.UTF8, "application/json")))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            //출력물
                            List<ManVo>  _reportList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();

                            //_reportList[0].FM_DT = System.DateTime.Now.ToString("yyyy-MM-dd");
                            _reportList[0].FM_DT = this.SelectedPopupItem.PROD_PLN_DT;
                            _reportList[0].MM_01 = "사출";

                            //
                            M665101Report report = new M665101Report(_reportList);
                            report.Margins.Top = 20;
                            report.Margins.Bottom = 20;
                            report.Margins.Left = 50;
                            report.Margins.Right = 20;
                            report.Landscape = false;

                            report.PrintingSystem.ShowPrintStatusDialog = true;
                            report.PaperKind = System.Drawing.Printing.PaperKind.A4;
                            report.Watermark.Text = Properties.Settings.Default.SettingCompany;
                            report.Watermark.TextDirection = DevExpress.XtraPrinting.Drawing.DirectionMode.ForwardDiagonal;
                            report.Watermark.Font = new System.Drawing.Font(report.PrintingSystem.Watermark.Font.FontFamily, 40);
                            report.Watermark.ForeColor = System.Drawing.Color.PaleTurquoise;
                            report.Watermark.TextTransparency = 150;


                            var window = new DevExpress.Xpf.Printing.DocumentPreviewWindow();
                            window.PreviewControl.DocumentSource = report;
                            report.CreateDocument(true);
                            window.Title = _title;
                            window.Owner = Application.Current.MainWindow;
                            window.ShowDialog();

                            //출력 유무 업데이트
                            //SearchDetailJob.UPD_USR_ID = SystemProperties.USER;
                            //using (HttpResponseMessage responseY = await SystemProperties.PROGRAM_HTTP.PostAsync("m66520/mst/u", new StringContent(JsonConvert.SerializeObject(SearchDetailJob), System.Text.Encoding.UTF8, "application/json")))
                            //{
                            //}
                        }

                            //if (response.IsSuccessStatusCode)
                            //{
                            //    string resultMsg = await response.Content.ReadAsStringAsync();
                            //    if (int.TryParse(resultMsg, out _Num) == false)
                            //    {
                            //        //실패
                            //        WinUIMessageBox.Show(resultMsg, _title, MessageBoxButton.OK, MessageBoxImage.Error);
                            //        return;
                            //    }

                            //    //성공
                            //    WinUIMessageBox.Show("완료 되었습니다", _title, MessageBoxButton.OK, MessageBoxImage.Information);
                            //}
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
        public async void CloseReportViewContact()
        {
            isPRNT_PLN = false;
        }


        public async void SYSTEM_CODE_VO()
        {
            //사업장
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "A-001"))
            {
                if (response.IsSuccessStatusCode)
                {
                    ProdLocList = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                    if (ProdLocList.Count > 0)
                    {
                        //ProdLocList.Insert(0, new SystemCodeVo() { CLSS_CD = null, CLSS_DESC = "전체" });
                        M_PROD_LOC_NM = ProdLocList[0];
                    }
                }
            }

            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("M665100", new StringContent(JsonConvert.SerializeObject(new ManVo() { DELT_FLG = "N", CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    PlnYrmonList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
                    if (PlnYrmonList.Count > 0)
                    {
                        //CoNmList.Insert(0, new SystemCodeVo() { CO_NM = "전체", CO_NO = null });
                        //M_PLN_YRMON = PlnYrmonList[PlnYrmonList.Count - 1];
                        M_PLN_YRMON = PlnYrmonList.Where<ManVo>(w => w.WKY_YRMON.Equals(System.DateTime.Now.ToString("yyyyMM"))).FirstOrDefault<ManVo>();
                    }
                }
            }

            //Refresh();
        }

        //공장
        private IList<SystemCodeVo> _ProdLocList = new List<SystemCodeVo>();
        public IList<SystemCodeVo> ProdLocList
        {
            get { return _ProdLocList; }
            set { SetProperty(ref _ProdLocList, value, () => ProdLocList); }
        }
        private SystemCodeVo _M_PROD_LOC_NM;
        public SystemCodeVo M_PROD_LOC_NM
        {
            get { return _M_PROD_LOC_NM; }
            set { SetProperty(ref _M_PROD_LOC_NM, value, () => M_PROD_LOC_NM); }
        }


        //년월 / 주차
        private IList<ManVo> _PlnYrmonList = new List<ManVo>();
        public IList<ManVo> PlnYrmonList
        {
            get { return _PlnYrmonList; }
            set { SetProperty(ref _PlnYrmonList, value, () => PlnYrmonList); }
        }
        private ManVo _M_PLN_YRMON;
        public ManVo M_PLN_YRMON
        {
            get { return _M_PLN_YRMON; }
            set { SetProperty(ref _M_PLN_YRMON, value, () => M_PLN_YRMON); }
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


        //TITLE
        private string _N1ST_PLN_TITLE = string.Empty;
        public string N1ST_PLN_TITLE
        {
            get { return _N1ST_PLN_TITLE; }
            set { SetProperty(ref _N1ST_PLN_TITLE, value, () => N1ST_PLN_TITLE); }
        }
        private string _N2ND_PLN_TITLE = string.Empty;
        public string N2ND_PLN_TITLE
        {
            get { return _N2ND_PLN_TITLE; }
            set { SetProperty(ref _N2ND_PLN_TITLE, value, () => N2ND_PLN_TITLE); }
        }
        private string _N3RD_PLN_TITLE = string.Empty;
        public string N3RD_PLN_TITLE
        {
            get { return _N3RD_PLN_TITLE; }
            set { SetProperty(ref _N3RD_PLN_TITLE, value, () => N3RD_PLN_TITLE); }
        }
        private string _N4TH_PLN_TITLE = string.Empty;
        public string N4TH_PLN_TITLE
        {
            get { return _N4TH_PLN_TITLE; }
            set { SetProperty(ref _N4TH_PLN_TITLE, value, () => N4TH_PLN_TITLE); }
        }
        private string _N5TH_PLN_TITLE = string.Empty;
        public string N5TH_PLN_TITLE
        {
            get { return _N5TH_PLN_TITLE; }
            set { SetProperty(ref _N5TH_PLN_TITLE, value, () => N5TH_PLN_TITLE); }
        }
        private string _N6TH_PLN_TITLE = string.Empty;
        public string N6TH_PLN_TITLE
        {
            get { return _N6TH_PLN_TITLE; }
            set { SetProperty(ref _N6TH_PLN_TITLE, value, () => N6TH_PLN_TITLE); }
        }
        private string _N7TH_PLN_TITLE = string.Empty;
        public string N7TH_PLN_TITLE
        {
            get { return _N7TH_PLN_TITLE; }
            set { SetProperty(ref _N7TH_PLN_TITLE, value, () => N7TH_PLN_TITLE); }
        }




        //작업지시서 창 - 활성
        private bool? _isPRNT_PLN = false;
        public bool? isPRNT_PLN
        {
            get { return _isPRNT_PLN; }
            set { SetProperty(ref _isPRNT_PLN, value, () => isPRNT_PLN); }
        }

        private bool? _isM_REPORT = false;
        public bool? isM_REPORT
        {
            get { return _isM_REPORT; }
            set { SetProperty(ref _isM_REPORT, value, () => isM_REPORT); }
        }


    }
}
