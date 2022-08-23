using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Printing;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Man;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Media;
using AquilaErpWpfApp3.Util;
using AquilaErpWpfApp3.View.M.Dialog;
using AquilaErpWpfApp3.View.M.Report;

namespace AquilaErpWpfApp3.ViewModel
{
    //������ �˱��� ��ȹ�� ����
    public sealed class M6628ViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private string _title = "������ �˱��� ��ȹ�� ����";
        //private static ManServiceClient manClient = SystemProperties.ManClient;

        private IList<ManVo> selectedMasterViewList;
       
        //Master Dialog
        //private ICommand masterSearchDialogCommand;
        //private ICommand masterNewDialogCommand;
        //private ICommand masterReportDialogCommand;
        //private ICommand masterDelDialogCommand;
        //
        private M6628MasterDialog masterDialog;


        public M6628ViewModel() 
        {
            StartDt = System.DateTime.Now;

            Refresh();
        }

        [Command]
        public async void Refresh(string _INSRT_MGMT_NO =null)
        {
            try
            {
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6628", new StringContent(JsonConvert.SerializeObject(new ManVo() { FM_DT = (StartDt).ToString("yyyy"), CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectedMasterViewList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
                    }
                    //SelectedMasterViewList = manClient.M6628SelectMaster(new ManVo() { FM_DT = (StartDt).ToString("yyyy"), CHNL_CD = SystemProperties.USER_VO.CHNL_CD });

                    M_TITLE_TEXT = (StartDt).ToString("yyyy") + "�� �˱��� ��ȹ��";

                    if (SelectedMasterViewList.Count > 0)
                    {
                        isM_UPDATE = true;
                        isM_DELETE = true;

                        if (string.IsNullOrEmpty(_INSRT_MGMT_NO))
                        {
                            SelectedMasterItem = SelectedMasterViewList[0];
                        }
                        else
                        {
                            SelectedMasterItem = SelectedMasterViewList.Where(x => x.INSRT_MGMT_NO.Equals(_INSRT_MGMT_NO)).LastOrDefault<ManVo>();
                        }
                    }
                    else
                    {
                        isM_UPDATE = false;
                        isM_DELETE = false;

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

        void OnSelectedMasterItemChanged()
        {
            //try
            //{
            //    if (this._selectMasterItem == null) { return; }
            //     //
            //    string deltFlg = this._selectMasterItem.DELT_FLG;
            //    this._selectMasterItem.DELT_FLG = null;
            //    this._selectMasterItem.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
            //    SelectedDetailViewList = codeClient.SelectDetailCode(this._selectMasterItem);
            //    this._selectMasterItem.DELT_FLG = deltFlg;
            //}
            //catch (System.Exception eLog)
            //{
            //    WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]�ý��� �з� �ڵ�", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
            //    return;
            //}
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
             ManVo editDao = this._selectMasterItem;
             if (editDao != null)
             {
                 editDao.INSRT_MGMT_YRMON = (StartDt).ToString("yyyy");

                 masterDialog = new M6628MasterDialog(editDao);
                 masterDialog.Title = "[����]" + M_TITLE_TEXT;
                 masterDialog.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                 masterDialog.Owner = Application.Current.MainWindow;
                 masterDialog.BorderEffect = BorderEffect.Default;
                 masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
                 masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
                 bool isDialog = (bool)masterDialog.ShowDialog();
                 if (isDialog)
                 {
                     Refresh(masterDialog.resultDomain.INSRT_MGMT_NO);
                     //for (int x = 0; x < SelectedMasterViewList.Count; x++)
                     //{
                     //    if ((masterDialog.resultDomain.INSRT_MGMT_NO).Equals(SelectedMasterViewList[x].INSRT_MGMT_NO))
                     //    {
                     //        SelectedMasterItem = SelectedMasterViewList[x];
                     //        break;
                     //    }
                     //}

                     //if (masterDialog.IsEdit == false)
                     //{
                     //    Refresh();

                     //    for (int x = 0; x < SelectedMasterViewList.Count; x++)
                     //    {
                     //        if ((masterDialog.resultDomain.CERTI_NO).Equals(SelectedMasterViewList[x].CERTI_NO))
                     //        {
                     //            SelectedMasterItem = SelectedMasterViewList[x];
                     //            break;
                     //        }
                     //    }
                     //}
                 }
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

        //public void EditMasterContact()
        //{
        //    ManVo editDao = this._selectMasterItem;
        //    if (editDao != null)
        //    {
        //        masterDialog = new M6628MasterDialog(editDao);
        //        masterDialog.Title = "������ �˱��� ��ȹ�� ���� - ����";
        //        masterDialog.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        //        masterDialog.BorderEffect = BorderEffect.Default;
        //        bool isDialog = (bool)masterDialog.ShowDialog();
        //        if (isDialog)
        //        {
        //            //if (masterDialog.IsEdit == false)
        //            //{
        //            //    Refresh();
        //            //}
        //        }
        //    }
        //}

        //public ICommand MasterDelDialogCommand
        //{
        //    get
        //    {
        //        if (masterDelDialogCommand == null)
        //            masterDelDialogCommand = new DelegateCommand(DelMasterContact);
        //        return masterDelDialogCommand;
        //    }
        //}


        //public void DelMasterContact()
        //{
        //    try
        //    {
        //        ManVo delDao = this._selectMasterItem;
        //        if (delDao != null)
        //        {
        //            MessageBoxResult result = WinUIMessageBox.Show(delDao.CERTI_NO + "/" + delDao.CERTI_USR_NM + " ������ ���� �Ͻðڽ��ϱ�?", "[����]������ ����", MessageBoxButton.YesNo, MessageBoxImage.Question);
        //            if (result == MessageBoxResult.Yes)
        //            {
        //                manClient.M6627DeleteMaster(delDao);
        //                WinUIMessageBox.Show("������ �Ϸ�Ǿ����ϴ�.", "[����]�ο� �ڰ� ����", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
        //                Refresh();
        //            }
        //        }
        //    }
        //    catch (System.Exception eLog)
        //    {
        //        WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]������ ����", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
        //        return;
        //    }
        //}
        #endregion



        //public ICommand MasterReportDialogCommand
        //{
        //    get
        //    {
        //        if (masterReportDialogCommand == null)
        //            masterReportDialogCommand = new DelegateCommand(ReportMasterContact);
        //        return masterReportDialogCommand;
        //    }
        //}

        [Command]
        public void ReportMasterContact()
        {
            ManVo editDao = this._selectMasterItem;
            if (editDao != null)
            {
                ObservableCollection<ManVo> ReportMasterViewList = new ObservableCollection<ManVo>();
                for (int x = 0; x < SelectedMasterViewList.Count; x++)
                {
                    ReportMasterViewList.Add(new ManVo());


                    ReportMasterViewList[x].RN = SelectedMasterViewList[x].RN;
                    ReportMasterViewList[x].INSRT_MGMT_NO = SelectedMasterViewList[x].INSRT_MGMT_NO;
                    ReportMasterViewList[x].INSRT_NM = SelectedMasterViewList[x].INSRT_NM;
                    ReportMasterViewList[x].MAKE_CO_NM = SelectedMasterViewList[x].MAKE_CO_NM;
                    ReportMasterViewList[x].PUR_DT = SelectedMasterViewList[x].PUR_DT;
                    ReportMasterViewList[x].INSRT_NXT_FX_DT = SelectedMasterViewList[x].INSRT_NXT_FX_DT;

                    //ReportMasterViewList[x].MM_01 = SelectedMasterViewList[x].MM_01;
                    SelectedMasterViewList[x].MM_01 = (SelectedMasterViewList[x].MM_01 == null ? "" : SelectedMasterViewList[x].MM_01);
                    if (SelectedMasterViewList[x].MM_01.Equals("��ȹ"))
                    {
                        ReportMasterViewList[x].MM_01 = "��";
                    }
                    else if (SelectedMasterViewList[x].MM_01.Equals("����"))
                    {
                        ReportMasterViewList[x].MM_01 = "��";
                    }
                    else if (SelectedMasterViewList[x].MM_01.Equals("�Ϸ�"))
                    {
                        ReportMasterViewList[x].MM_01 = "��";
                    }
                        
                    //ReportMasterViewList[x].MM_02 = SelectedMasterViewList[x].MM_02;
                    SelectedMasterViewList[x].MM_02 = (SelectedMasterViewList[x].MM_02 == null ? "" : SelectedMasterViewList[x].MM_02);
                    if (SelectedMasterViewList[x].MM_02.Equals("��ȹ"))
                    {
                        ReportMasterViewList[x].MM_02 = "��";
                    }
                    else if (SelectedMasterViewList[x].MM_02.Equals("����"))
                    {
                        ReportMasterViewList[x].MM_02 = "��";
                    }
                    else if (SelectedMasterViewList[x].MM_02.Equals("�Ϸ�"))
                    {
                        ReportMasterViewList[x].MM_02 = "��";
                    }

                    //ReportMasterViewList[x].MM_03 = SelectedMasterViewList[x].MM_03;
                    SelectedMasterViewList[x].MM_03 = (SelectedMasterViewList[x].MM_03 == null ? "" : SelectedMasterViewList[x].MM_03);
                    if (SelectedMasterViewList[x].MM_03.Equals("��ȹ"))
                    {
                        ReportMasterViewList[x].MM_03 = "��";
                    }
                    else if (SelectedMasterViewList[x].MM_03.Equals("����"))
                    {
                        ReportMasterViewList[x].MM_03 = "��";
                    }
                    else if (SelectedMasterViewList[x].MM_03.Equals("�Ϸ�"))
                    {
                        ReportMasterViewList[x].MM_03 = "��";
                    }

                    //ReportMasterViewList[x].MM_04 = SelectedMasterViewList[x].MM_04;
                    SelectedMasterViewList[x].MM_04 = (SelectedMasterViewList[x].MM_04 == null ? "" : SelectedMasterViewList[x].MM_04);
                    if (SelectedMasterViewList[x].MM_04.Equals("��ȹ"))
                    {
                        ReportMasterViewList[x].MM_04 = "��";
                    }
                    else if (SelectedMasterViewList[x].MM_04.Equals("����"))
                    {
                        ReportMasterViewList[x].MM_04 = "��";
                    }
                    else if (SelectedMasterViewList[x].MM_04.Equals("�Ϸ�"))
                    {
                        ReportMasterViewList[x].MM_04 = "��";
                    }

                    //ReportMasterViewList[x].MM_05 = SelectedMasterViewList[x].MM_05;
                    SelectedMasterViewList[x].MM_05 = (SelectedMasterViewList[x].MM_05 == null ? "" : SelectedMasterViewList[x].MM_05);
                    if (SelectedMasterViewList[x].MM_05.Equals("��ȹ"))
                    {
                        ReportMasterViewList[x].MM_05 = "��";
                    }
                    else if (SelectedMasterViewList[x].MM_05.Equals("����"))
                    {
                        ReportMasterViewList[x].MM_05 = "��";
                    }
                    else if (SelectedMasterViewList[x].MM_05.Equals("�Ϸ�"))
                    {
                        ReportMasterViewList[x].MM_05 = "��";
                    }

                    //ReportMasterViewList[x].MM_06 = SelectedMasterViewList[x].MM_06;
                    SelectedMasterViewList[x].MM_06 = (SelectedMasterViewList[x].MM_06 == null ? "" : SelectedMasterViewList[x].MM_06);
                    if (SelectedMasterViewList[x].MM_06.Equals("��ȹ"))
                    {
                        ReportMasterViewList[x].MM_06 = "��";
                    }
                    else if (SelectedMasterViewList[x].MM_06.Equals("����"))
                    {
                        ReportMasterViewList[x].MM_06 = "��";
                    }
                    else if (SelectedMasterViewList[x].MM_06.Equals("�Ϸ�"))
                    {
                        ReportMasterViewList[x].MM_06 = "��";
                    }

                    //ReportMasterViewList[x].MM_07 = SelectedMasterViewList[x].MM_07;
                    SelectedMasterViewList[x].MM_07 = (SelectedMasterViewList[x].MM_07 == null ? "" : SelectedMasterViewList[x].MM_07);
                    if (SelectedMasterViewList[x].MM_07.Equals("��ȹ"))
                    {
                        ReportMasterViewList[x].MM_07 = "��";
                    }
                    else if (SelectedMasterViewList[x].MM_07.Equals("����"))
                    {
                        ReportMasterViewList[x].MM_07 = "��";
                    }
                    else if (SelectedMasterViewList[x].MM_07.Equals("�Ϸ�"))
                    {
                        ReportMasterViewList[x].MM_07 = "��";
                    }

                    //ReportMasterViewList[x].MM_08 = SelectedMasterViewList[x].MM_08;
                    SelectedMasterViewList[x].MM_08 = (SelectedMasterViewList[x].MM_08 == null ? "" : SelectedMasterViewList[x].MM_08);
                    if (SelectedMasterViewList[x].MM_08.Equals("��ȹ"))
                    {
                        ReportMasterViewList[x].MM_08 = "��";
                    }
                    else if (SelectedMasterViewList[x].MM_08.Equals("����"))
                    {
                        ReportMasterViewList[x].MM_08 = "��";
                    }
                    else if (SelectedMasterViewList[x].MM_08.Equals("�Ϸ�"))
                    {
                        ReportMasterViewList[x].MM_08 = "��";
                    }

                    //ReportMasterViewList[x].MM_09 = SelectedMasterViewList[x].MM_09;
                    SelectedMasterViewList[x].MM_09 = (SelectedMasterViewList[x].MM_09 == null ? "" : SelectedMasterViewList[x].MM_09);
                    if (SelectedMasterViewList[x].MM_09.Equals("��ȹ"))
                    {
                        ReportMasterViewList[x].MM_09 = "��";
                    }
                    else if (SelectedMasterViewList[x].MM_09.Equals("����"))
                    {
                        ReportMasterViewList[x].MM_09 = "��";
                    }
                    else if (SelectedMasterViewList[x].MM_09.Equals("�Ϸ�"))
                    {
                        ReportMasterViewList[x].MM_09 = "��";
                    }

                    //ReportMasterViewList[x].MM_10 = SelectedMasterViewList[x].MM_10;
                    SelectedMasterViewList[x].MM_10 = (SelectedMasterViewList[x].MM_10 == null ? "" : SelectedMasterViewList[x].MM_10);
                    if (SelectedMasterViewList[x].MM_10.Equals("��ȹ"))
                    {
                        ReportMasterViewList[x].MM_10 = "��";
                    }
                    else if (SelectedMasterViewList[x].MM_10.Equals("����"))
                    {
                        ReportMasterViewList[x].MM_10 = "��";
                    }
                    else if (SelectedMasterViewList[x].MM_10.Equals("�Ϸ�"))
                    {
                        ReportMasterViewList[x].MM_10 = "��";
                    }

                    //ReportMasterViewList[x].MM_11 = SelectedMasterViewList[x].MM_11;
                    SelectedMasterViewList[x].MM_11 = (SelectedMasterViewList[x].MM_11 == null ? "" : SelectedMasterViewList[x].MM_11);
                    if (SelectedMasterViewList[x].MM_11.Equals("��ȹ"))
                    {
                        ReportMasterViewList[x].MM_11 = "��";
                    }
                    else if (SelectedMasterViewList[x].MM_11.Equals("����"))
                    {
                        ReportMasterViewList[x].MM_11 = "��";
                    }
                    else if (SelectedMasterViewList[x].MM_11.Equals("�Ϸ�"))
                    {
                        ReportMasterViewList[x].MM_11 = "��";
                    }

                    //ReportMasterViewList[x].MM_12 = SelectedMasterViewList[x].MM_12;
                    SelectedMasterViewList[x].MM_12 = (SelectedMasterViewList[x].MM_12 == null ? "" : SelectedMasterViewList[x].MM_12);
                    if (SelectedMasterViewList[x].MM_12.Equals("��ȹ"))
                    {
                        ReportMasterViewList[x].MM_12 = "��";
                    }
                    else if (SelectedMasterViewList[x].MM_12.Equals("����"))
                    {
                        ReportMasterViewList[x].MM_12 = "��";
                    }
                    else if (SelectedMasterViewList[x].MM_12.Equals("�Ϸ�"))
                    {
                        ReportMasterViewList[x].MM_12 = "��";
                    }

                    ReportMasterViewList[x].RN = SelectedMasterViewList[x].RN;
                    ReportMasterViewList[x].MM_RMK = SelectedMasterViewList[x].MM_RMK;
                }

                ReportMasterViewList[0].Message = M_TITLE_TEXT;
                ReportMasterViewList[0].FM_DT = (StartDt).ToString("yyyy") + " �⵵";



                //
                M6628Report report = new M6628Report(ReportMasterViewList);
                report.Margins.Top = 20;
                report.Margins.Bottom = 20;
                report.Margins.Left = 50;
                report.Margins.Right = 20;
                report.Landscape = true;
                report.PrintingSystem.ShowPrintStatusDialog = true;
                report.PaperKind = System.Drawing.Printing.PaperKind.A4;

                var window = new DocumentPreviewWindow();
                window.PreviewControl.DocumentSource = report;
                report.CreateDocument(true);
                window.Title = M_TITLE_TEXT;
                window.Owner = Application.Current.MainWindow;
                window.ShowDialog();
                //
                //
                //XtraReportPreviewModel model = new XtraReportPreviewModel(report);
                //DocumentPreviewWindow window = new DocumentPreviewWindow() { Model = model };
                //report.CreateDocument(true);
                //window.Owner = Application.Current.MainWindow;
                //window.Title = M_TITLE_TEXT;
                //window.ShowDialog();

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

        private string _M_SEARCH_TEXT = string.Empty;
        public string M_SEARCH_TEXT
        {
            get { return _M_SEARCH_TEXT; }
            set { SetProperty(ref _M_SEARCH_TEXT, value, () => M_SEARCH_TEXT); }
        }

        private DateTime _startDt;
        public DateTime StartDt
        {
            get { return _startDt; }
            set { SetProperty(ref _startDt, value, () => StartDt); }
        }

        private string _M_TITLE_TEXT = string.Empty;
        public string M_TITLE_TEXT
        {
            get { return _M_TITLE_TEXT; }
            set { SetProperty(ref _M_TITLE_TEXT, value, () => M_TITLE_TEXT); }
        }
    }
}
