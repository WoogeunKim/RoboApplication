using AquilaErpWpfApp3.Util;
using AquilaErpWpfApp3.View.TEC.Dialog;
using AquilaErpWpfApp3.View.TEC.Report;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using DevExpress.XtraReports.UI;
using ModelsLibrary.Code;
using ModelsLibrary.Tec;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Windows;

namespace AquilaErpWpfApp3.ViewModel
{
    public sealed class M6111ViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private string _title = "�������";

        //private static InvServiceClient invClient = SystemProperties.InvClient;
        private IList<TecVo> selectedMstList = new List<TecVo>();
        private IList<TecVo> selectedMstItemsList = new List<TecVo>();

        //private ObservableCollection<PurVo> selectedDtlList = new ObservableCollection<PurVo>();
        //private IList<InvVo> selectedDtlList = new List<InvVo>();

        //private T8812MasterDialog detailDialog;
        //private I6610DetailImpDialog detailImpDialog;


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

        public M6111ViewModel() 
        {
            StartDt = System.DateTime.Now;
            EndDt = System.DateTime.Now;

            //IN_FLG = false;

            SYSTEM_CODE_VO();
           
        }

       [Command]
       public async void Refresh()
        {
            try
            {

                //using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("t8812/", new StringContent(JsonConvert.SerializeObject(new TecVo() {CHNL_CD = SystemProperties.USER_VO.CHNL_CD, FM_DT = (StartDt).ToString("yyyy-MM-dd"), TO_DT = (EndDt).ToString("yyyy-MM-dd"), AREA_CD = M_SL_AREA_NM.CLSS_CD, LOC_CD = M_SL_LOC_NM.CLSS_CD }), System.Text.Encoding.UTF8, "application/json")))
                //{
                //    if (response.IsSuccessStatusCode)
                //    {
                //        this.SelectMstList = JsonConvert.DeserializeObject<IEnumerable<TecVo>>(await response.Content.ReadAsStringAsync()).Cast<TecVo>().ToList();
                //    }

                //    //SelectMstList = invClient.I6610SelectMstList(new InvVo() { FM_DT = (StartDt).ToString("yyyy-MM-dd"), TO_DT = (EndDt).ToString("yyyy-MM-dd"), AREA_CD = (string.IsNullOrEmpty(TXT_SL_AREA_NM) ? null : _AreaMap[TXT_SL_AREA_NM]), GBN = (string.IsNullOrEmpty(M_CHECKD) ? "%" : M_CHECKD), LOC_CD = (string.IsNullOrEmpty(TXT_SL_LOC_NM) ? null : _LocMap[TXT_SL_LOC_NM]), IN_FLG = (IN_FLG == true ? "Y" : "N") });
                //    ////+ ",   [�԰���]" + (IN_FLG == true ? "Y" : "N") 
                //    Title = "[�Ⱓ]" + (StartDt).ToString("yyyy-MM-dd") + "~" + (EndDt).ToString("yyyy-MM-dd") + ",   [�����]" + M_SL_AREA_NM.CLSS_DESC + ",   [â��]" + M_SL_LOC_NM.CLSS_DESC;

                //    if (SelectMstList.Count >= 1)
                //    {
                //        isM_UPDATE = true;
                //        isM_DELETE = true;
                //        //SelectedMstItem = SelectMstList[0];
                //        //for (int x = 0; x < SelectMstList.Count; x++)
                //        //{
                //        //    SelectMstList[x].DTL_DATA = invClient.I6610SelectDtlList(SelectedMstItem);
                //        //}
                //    }
                //    else
                //    {
                //        isM_UPDATE = false;
                //        isM_DELETE = false;
                //    }
                //    //DXSplashScreen.Close();
                //}
            }
            catch (System.Exception eLog)
            {
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


        //�����
        //private Dictionary<string, string> _AreaMap = new Dictionary<string, string>();
        private IList<SystemCodeVo> _AreaCd = new List<SystemCodeVo>();
        public IList<SystemCodeVo> AreaList
        {
            get { return _AreaCd; }
            set { SetProperty(ref _AreaCd, value, () => AreaList); }
        }
        //�����
        //private CodeDao _M_SL_AREA_NM;
        //public CodeDao M_SL_AREA_NM
        //{
        //    get { return _M_SL_AREA_NM; }
        //    set { SetProperty(ref _M_SL_AREA_NM, value, () => M_SL_AREA_NM); }
        //}
        //����� 
        private SystemCodeVo _M_SL_AREA_NM;
        public SystemCodeVo M_SL_AREA_NM
        {
            get { return _M_SL_AREA_NM; }
            set { SetProperty(ref _M_SL_AREA_NM, value, () => M_SL_AREA_NM); }
        }

        //â��
        //private Dictionary<string, string> _LocMap = new Dictionary<string, string>();
        private IList<SystemCodeVo> _LocCd = new List<SystemCodeVo>();
        public IList<SystemCodeVo> LocList
        {
            get { return _LocCd; }
            set { SetProperty(ref _LocCd, value, () => LocList); }
        }
        //â��
        //private CodeDao _M_SL_AREA_NM;
        //public CodeDao M_SL_AREA_NM
        //{
        //    get { return _M_SL_AREA_NM; }
        //    set { SetProperty(ref _M_SL_AREA_NM, value, () => M_SL_AREA_NM); }
        //}
        //â�� 
        private SystemCodeVo _M_SL_LOC_NM;
        public SystemCodeVo M_SL_LOC_NM
        {
            get { return _M_SL_LOC_NM; }
            set { SetProperty(ref _M_SL_LOC_NM, value, () => M_SL_LOC_NM); }
        }

        ////����
        //private bool? _M_CHECKD_ALL = true;
        //public bool? M_CHECKD_ALL
        //{
        //    get { return _M_CHECKD_ALL; }
        //    set { SetProperty(ref _M_CHECKD_ALL, value, () => M_CHECKD_ALL, SelectCheckdAll); }
        //}
        //private bool? _M_CHECKD_PO = false;
        //public bool? M_CHECKD_PO
        //{
        //    get { return _M_CHECKD_PO; }
        //    set { SetProperty(ref _M_CHECKD_PO, value, () => M_CHECKD_PO, SelectCheckdPo); }
        //}
        //private bool? _M_CHECKD_IV = false;
        //public bool? M_CHECKD_IV
        //{
        //    get { return _M_CHECKD_IV; }
        //    set { SetProperty(ref _M_CHECKD_IV, value, () => M_CHECKD_IV, SelectCheckdIv); }
        //}


        //private String M_CHECKD = string.Empty;
        //private String M_CHECKD_NAME = string.Empty;
        //private void SelectCheckdAll()
        //{
        //    if (M_CHECKD_ALL == true)
        //    {
        //        //M_CHECKD_ALL = true;
        //        M_CHECKD_PO = false;
        //        M_CHECKD_IV = false;
        //        //
        //        M_CHECKD = "";
        //        M_CHECKD_NAME = "��ü";
        //    }

        //    if (M_CHECKD_ALL == false && M_CHECKD_PO == false && M_CHECKD_IV == false)
        //    {
        //        M_CHECKD_ALL = true;
        //        M_CHECKD_NAME = "��ü";
        //    }
            
        //    //else
        //    //{
        //    //    M_CHECKD_PO = true;
        //    //    M_CHECKD = "PO";
        //    //    M_CHECKD_NAME = "����";
        //    //}
        //}
        //private void SelectCheckdPo()
        //{
        //    if (M_CHECKD_PO == true)
        //    {
        //        //M_CHECKD_PO = true;
        //        M_CHECKD_ALL = false;
        //        M_CHECKD_IV = false;
        //        //
        //        M_CHECKD = "PO";
        //        M_CHECKD_NAME = "����";
        //    }

        //    if (M_CHECKD_ALL == false && M_CHECKD_PO == false && M_CHECKD_IV == false)
        //    {
        //        M_CHECKD_ALL = true;
        //        M_CHECKD_NAME = "��ü";
        //    }
        //    //else
        //    //{
        //    //    M_CHECKD_ALL = true;
        //    //    M_CHECKD = "";
        //    //    M_CHECKD_NAME = "��ü";
        //    //}
        //}
        //private void SelectCheckdIv()
        //{
        //    if (M_CHECKD_IV == true)
        //    {
        //        //M_CHECKD_IV = true;
        //        M_CHECKD_PO = false;
        //        M_CHECKD_ALL = false;
        //        //
        //        M_CHECKD = "IV";
        //        M_CHECKD_NAME = "����";
        //    }

        //    if (M_CHECKD_ALL == false && M_CHECKD_PO == false && M_CHECKD_IV == false)
        //    {
        //        M_CHECKD_ALL = true;
        //        M_CHECKD_NAME = "��ü";
        //    }
        //    //else
        //    //{
        //    //    M_CHECKD_ALL = true;
        //    //    M_CHECKD = "";
        //    //    M_CHECKD_NAME = "��ü";
        //    //}
        //}



        public IList<TecVo> SelectMstList
        {
            get { return selectedMstList; }
            set { SetProperty(ref selectedMstList, value, () => SelectMstList); }
        }

        TecVo _selectedMstItem;
        public TecVo SelectedMstItem
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

        public IList<TecVo> SelectedMstItems
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


        //��� ��
        private string _M_PRINT_CNT = "1";
        public string M_PRINT_CNT
        {
            get { return _M_PRINT_CNT; }
            set { SetProperty(ref _M_PRINT_CNT, value, () => M_PRINT_CNT); }
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
             //    WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]����Ƿڳ������", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
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

        //[Command]
        //public void NewContact()
        //{
        //    if (SelectedMstItems.Count > 0)
        //    {
        //        string _coNm = this.SelectedMstItems[0].CO_NM;


        //        detailDialog = new T8812MasterDialog(this.SelectedMstItems);
        //        detailDialog.Title = this._title + " - [ �� : " + SelectedMstItems.Count + " ]";
        //        detailDialog.Owner = Application.Current.MainWindow;
        //        detailDialog.BorderEffect = BorderEffect.Default;
        //        bool isDialog = (bool)detailDialog.ShowDialog();
        //        if (isDialog)
        //        {
        //            Refresh();

        //           //this.SelectedMstItem = this.SelectMstList.Where<TecVo>(w => w.CO_NM.Equals(_coNm)).FirstOrDefault<TecVo>();
        //        }
        //    }
        //    else
        //    {
        //        WinUIMessageBox.Show("ǰ�� �˻� ����� ���� ���� �ʽ��ϴ�", _title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
        //        return;
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
        //    masterDialog.Title = this._title + " - ����";
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
        public async void BarPrintConfig()
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
                WinUIMessageBox.Show(eLog.Message, "[����]" + _Title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }

        #region [2021-09-09]  ���ڵ� �ӵ� ���� �۾� ��
        //[Command]
        //public async void BarPrint()
        //{
        //    try
        //    {

        //        if (string.IsNullOrEmpty(Properties.Settings.Default.str_PrnNm))
        //        {
        //            System.Windows.Controls.PrintDialog dialogue = new System.Windows.Controls.PrintDialog();
        //            if (dialogue.ShowDialog() == true)
        //            {
        //                Properties.Settings.Default.str_PrnNm = dialogue.PrintQueue.FullName;
        //                Properties.Settings.Default.Save();
        //            }
        //        }

        //        if (SelectedMstItems.Count > 0)
        //        {
        //            BarPrint _bar = new BarPrint();

        //            MessageBoxResult result = WinUIMessageBox.Show( "[ �� :" + SelectedMstItems.Count + "�� / (" + M_PRINT_CNT + "��) ] ������ ���ڵ� �Ͻðڽ��ϱ�?", "[���ڵ� - ���]" + _title, MessageBoxButton.YesNo, MessageBoxImage.Question);
        //            if (result == MessageBoxResult.Yes)
        //            {
        //                DXSplashScreen.Show<ProgressWindow>();

        //                LabelDao labelDao = new LabelDao();
        //                foreach (TecVo _item in SelectedMstItems)
        //                {

        //                    labelDao.A01 = _item.ITM_NM ?? " ";
        //                    labelDao.A02 = _item.ITM_CD ?? " ";
        //                    //����
        //                    labelDao.A03 = _item.MTRL_MAKE_DT;
        //                    //����
        //                    labelDao.A04 = _item.ITM_QTY;
        //                    //������ȣ
        //                    labelDao.A05 = _item.CO_MAKE_NO ?? " ";
        //                    //�����ȣ
        //                    labelDao.A06 = _item.INSP_NO ?? " ";
        //                    //������
        //                    labelDao.A07 = _item.CO_NM ?? " ";
        //                    //������
        //                    labelDao.A08 = _item.MTRL_EXP_DT ?? " ";
        //                    //������ġ
        //                    labelDao.A09 = _item.TMP_LOC_NM ?? " ";
        //                    //��������
        //                    labelDao.A10 = _item.TMP_CONDI_NM ?? " ";
        //                    //�������
        //                    labelDao.A11 = _item.ITM_RMK ?? " ";
        //                    //������
        //                    labelDao.A12 = _item.INSP_DT ?? " ";
        //                    //labelDao.A10 = "������";//_item.ITM_GRP_NM ?? " ";
        //                    //labelDao.A11 = " ";//_item.ITM_IN_DT ?? " ";

        //                    //�� �հ� - [���� ��ȣ - �� �հ�]
        //                    //labelDao.A13 = SelectedMstItems.Sum<TecVo>(x => Convert.ToInt32(x.ITM_QTY));
        //                    //labelDao.A13 = this.SelectMstList.Where<TecVo>(x => (x.INSP_NO ?? "").Equals(_item.INSP_NO)).Sum<TecVo>(x => Convert.ToInt32(x.ITM_QTY));

        //                    //
        //                    //labelDao.A13 = _item.BATCH_CD ?? " ";
        //                    //���ڵ�
        //                    //labelDao.A14 = (SelectedMasterItem.ITM_GRP_NM.Equals("������") ? SelectedMasterItem.MTRL_LOT_NO : SelectedMasterItem.CO_MAKE_NO);
        //                    labelDao.A14 = (_item.LOT_NO);

        //                    labelDao.A15 = _item.GBN ?? " ";

        //                    //������ ���
        //                    _bar.M_Godex(labelDao, Convert.ToInt16(M_PRINT_CNT));
        //                }


        //                DXSplashScreen.Close();
        //            }
        //        }

        //        //if (SelectedMstItem == null)
        //        //{
        //        //    return;
        //        //}

        //        //MessageBoxResult result = WinUIMessageBox.Show("������ ���� �Ͻðڽ��ϱ�?", "[����]" + _title, MessageBoxButton.YesNo, MessageBoxImage.Question);
        //        //if (result == MessageBoxResult.Yes)
        //        //{
        //        //    int _Num = 0;
        //        //    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("i6610/mst/d", new StringContent(JsonConvert.SerializeObject(SelectedMstItems), System.Text.Encoding.UTF8, "application/json")))
        //        //    {
        //        //        if (response.IsSuccessStatusCode)
        //        //        {
        //        //            string resultMsg = await response.Content.ReadAsStringAsync();
        //        //            if (int.TryParse(resultMsg, out _Num) == false)
        //        //            {
        //        //                //����
        //        //                WinUIMessageBox.Show(resultMsg, _title, MessageBoxButton.OK, MessageBoxImage.Error);
        //        //                return;
        //        //            }
        //        //        }
        //        //    }
        //        //        // InvVo resultVo;
        //        //        //// DXSplashScreen.Show<ProgressWindow>();
        //        //        // for (int x = 0; x < SelectedMstItems.Count; x++)
        //        //        // {
        //        //        //    resultVo = invClient.I6610DeleteMst(new InvVo() { INAUD_TMP_NO = SelectedMstItems[x].INAUD_TMP_NO, INAUD_TMP_SEQ = SelectedMstItems[x].INAUD_TMP_SEQ });
        //        //        //    if (!resultVo.isSuccess)
        //        //        //    {
        //        //        //        //����
        //        //        //        WinUIMessageBox.Show(resultVo.Message, this._title, MessageBoxButton.OK, MessageBoxImage.Error);
        //        //        //        return;
        //        //        //    }
        //        //        // }
        //        //        // //DXSplashScreen.Close();

        //        //        Refresh();
        //        //        WinUIMessageBox.Show("������ �Ϸ�Ǿ����ϴ�.", "[����]" + _title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
        //        //    }
        //    }
        //    catch (System.Exception)
        //    {
        //        if (DXSplashScreen.IsActive)
        //        {
        //            DXSplashScreen.Close();
        //        }
        //        return;
        //    }
        //} 
        #endregion

        [Command]
        public async void BarPrint()
        {
            try
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


                if (SelectedMstItems.Count > 0)
                {
                    //BarPrint _bar = new BarPrint();

                    MessageBoxResult result = WinUIMessageBox.Show("[ �� :" + SelectedMstItems.Count + "�� / (" + M_PRINT_CNT + "��) ] ������ ���ڵ� �Ͻðڽ��ϱ�?", "[���ڵ� - ���]" + _title, MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        // DXSplashScreen.Show<ProgressWindow>();

                        //for (int x = 0; x < Convert.ToInt32(M_PRINT_CNT); x++)
                        //{
                        T8812BarCodeReport BarCodeReport = new T8812BarCodeReport(SelectedMstItems);

                        //BarCodeReport.Margins.Top = 1;
                        //BarCodeReport.Margins.Bottom = 1;
                        //BarCodeReport.Margins.Left = 1;
                        //BarCodeReport.Margins.Right = 1;
                        //BarCodeReport.Landscape = false;
                        //BarCodeReport.PrintingSystem.ShowPrintStatusDialog = true;
                        //BarCodeReport.PaperKind = System.Drawing.Printing.PaperKind.Custom;

                        //BarCodeReport.PrintingSystem.ShowPrintStatusDialog = false;
                        //BarCodeReport.PrintingSystem.ShowMarginsWarning = false;
                        //BarCodeReport.Padding = 0;
                        //BarCodeReport.Margins.Top = 0;
                        //BarCodeReport.Margins.Bottom = 0;
                        //BarCodeReport.Margins.Left = 0;
                        //BarCodeReport.Margins.Right = 0;

                        //BarCodeReport.CreateDocument(true);
                        //BarCodeReport.PrinterName = Properties.Settings.Default.str_PrnNm;
                        //BarCodeReport.Print();

                        ReportPrintTool printTool = new ReportPrintTool(BarCodeReport);
                        printTool.PrinterSettings.Copies = Convert.ToInt16(M_PRINT_CNT);
                        printTool.PrintingSystem.ShowPrintStatusDialog = false;
                        printTool.PrintingSystem.ShowMarginsWarning = false;
                        

                        printTool.Print(Properties.Settings.Default.str_PrnNm);
                        //}


                        //DXSplashScreen.Close();
                    }
                }
            }
            catch (System.Exception)
            {
                //if (DXSplashScreen.IsActive)
                //{
                //    DXSplashScreen.Close();
                //}
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
                //detailPurDialog = new I6610DetailPurDialog(new InvVo() {CHNL_CD = SystemProperties.USER_VO.CHNL_CD, FM_DT = (StartDt).ToString("yyyy-MM-dd"), TO_DT = (EndDt).ToString("yyyy-MM-dd") });
                //detailPurDialog.Title = "���� �԰� ���� ���� ";
                //detailPurDialog.Owner = Application.Current.MainWindow;
                //detailPurDialog.BorderEffect = BorderEffect.Default;
                //detailPurDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
                //detailPurDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
                //bool isDialog = (bool)detailPurDialog.ShowDialog();
                ////if (isDialog)
                //{
                //    Refresh();
                //}
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

        //public void NewDtlImpContact()
        //{
        //    ////if (SelectedMstItem == null)
        //    ////{
        //    ////    return;
        //    ////}

            
        //    ////SelectedMstItem.FM_DT = (StartDt).ToString("yyyy-MM-dd");
        //    ////SelectedMstItem.TO_DT = (EndDt).ToString("yyyy-MM-dd");
        //    ////SelectedMstItem.GRP_ID = (string.IsNullOrEmpty(M_DEPT_DESC) ? null : _DeptMap[M_DEPT_DESC]);
        //    ////SelectedMstItem.GRP_NM = M_DEPT_DESC;


        //    ////if (string.IsNullOrEmpty(SelectedMstItem.GRP_ID))
        //    ////{
        //    ////    WinUIMessageBox.Show("[�μ�]��ü�� ���� �ϽǼ� �����ϴ�", "[��ȸ ����]ǰ�� �԰� ����", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
        //    ////    return;
        //    ////}
        //    //try
        //    //{
        //    //    detailImpDialog = new I6610DetailImpDialog(new InvVo() { FM_DT = (StartDt).ToString("yyyy-MM-dd"), TO_DT = (EndDt).ToString("yyyy-MM-dd") });
        //    //    detailImpDialog.Title = "���� �԰� ���� ���� ";
        //    //    detailImpDialog.Owner = Application.Current.MainWindow;
        //    //    detailImpDialog.BorderEffect = BorderEffect.Default;
        //    //    ////masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
        //    //    ////masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
        //    //    bool isDialog = (bool)detailImpDialog.ShowDialog();
        //    //    //if (isDialog)
        //    //    {
              
        //    //            DXSplashScreen.Show<ProgressWindow>();
        //    //            Refresh();
        //    //            DXSplashScreen.Close();
               
        //    //    }
        //    //}
        //    //catch (System.Exception)
        //    //{
        //    //      DXSplashScreen.Close();
        //    //      //WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
        //    //      return;
        //    //}
        //}

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
                //int tmpIMP_ITM_AMT = 0;
                //int tmpITM_QTY = 0;
                //if (SelectedMstItem == null)
                //{
                //    return;
                //}

                //IList<InvVo> printDao = new List<InvVo>();
                //if (SelectedMstItem != null)
                //{
                //    if (SelectedMstItems.Count > 0)
                //    {
                //        for (int x = 0; x < SelectedMstItems.Count; x++)
                //        {
                //            SelectedMstItems[x].GRP_NM = "[���԰� ���� (From) " + (StartDt).ToString("yyyy-MM-dd HH:mm") + " ~ (To) " + (EndDt).ToString("yyyy-MM-dd HH:mm") + ", �����: " + SelectedMstItems[x].AREA_NM + "]";
                //            tmpIMP_ITM_AMT += Convert.ToInt32(SelectedMstItems[x].IMP_ITM_AMT);
                //            tmpITM_QTY += Convert.ToInt32(SelectedMstItems[x].ITM_QTY);

                //            SelectedMstItems[x].TMP_A_QTY = tmpIMP_ITM_AMT;
                //            SelectedMstItems[x].TMP_B_QTY = tmpITM_QTY;
                //        }

                //        I6610Report report = new I6610Report(SelectedMstItems);
                //        report.Margins.Top = 0;
                //        report.Margins.Bottom = 0;
                //        report.Margins.Left = 30;
                //        report.Margins.Right = 0;
                //        report.Landscape = true;
                //        report.PrintingSystem.ShowPrintStatusDialog = true;
                //        report.PaperKind = System.Drawing.Printing.PaperKind.A4;

                //        var window = new DocumentPreviewWindow();
                //        window.PreviewControl.DocumentSource = report;
                //        report.CreateDocument(true);
                //        window.Title = "ǰ���԰� ���";
                //        window.Owner = Application.Current.MainWindow;
                //        window.ShowDialog();
                //        //XtraReportPreviewModel model = new XtraReportPreviewModel(report);
                //        //DocumentPreviewWindow window = new DocumentPreviewWindow() { Model = model };
                //        //report.CreateDocument(true);
                //        //window.Owner = Application.Current.MainWindow;
                //        //window.Title = "ǰ���԰� ���";
                //        //window.ShowDialog();
                //    }
                //}
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
