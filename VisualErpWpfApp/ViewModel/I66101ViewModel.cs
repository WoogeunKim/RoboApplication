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

namespace AquilaErpWpfApp3.ViewModel
{
    public sealed class I66101ViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private string _title = "발주입고";

        private I66101DetailPurDialog detailPurDialog;

        public I66101ViewModel() 
        {
            StartDt = System.DateTime.Now;
            EndDt = System.DateTime.Now;

            SYSTEM_CODE_VO();

            Refresh();
        }

       [Command]
       public async void Refresh()
        {
            try
            {
                // 사업장이 null 경우 
                if (M_SL_AREA_NM == null) return;

                if (DXSplashScreen.IsActive == false)
                {
                    DXSplashScreen.Show<ProgressWindow>();
                }

                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("i6610/mst", new StringContent(JsonConvert.SerializeObject(new InvVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD, FM_DT = (StartDt).ToString("yyyy-MM-dd"), TO_DT = (EndDt).ToString("yyyy-MM-dd"), AREA_CD = M_SL_AREA_NM.CLSS_CD, LOC_CD = "300", IN_FLG = "Y" }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectMstList = JsonConvert.DeserializeObject<IEnumerable<InvVo>>(await response.Content.ReadAsStringAsync()).Cast<InvVo>().ToList();

                        Title = "[기간]" + (StartDt).ToString("yyyy-MM-dd") + "~" + (EndDt).ToString("yyyy-MM-dd") + ",   [사업장]" + M_SL_AREA_NM.CLSS_DESC;


                        // 선택된 것에 대해서 초기화됨.
                        SelectedMstItem = new InvVo();
                        SelectedMstItems = new List<InvVo>();

                        isM_UPDATE = false; 
                        isM_DELETE = false;

                        //if (SelectMstList.Count >= 1)
                        //{
                        //    //isM_UPDATE = true;
                        //    //isM_DELETE = true;
                        //}
                        //else
                        //{
                        //    //isM_UPDATE = false;
                        //    //isM_DELETE = false;
                        //}

                    }
                }

                if (DXSplashScreen.IsActive == true)
                {
                    DXSplashScreen.Close();
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

        private void SelectMstDetail()
        {
            try
            {
                if (this.SelectedMstItem == null)  return;

                isM_DELETE = true;
                isM_UPDATE = true;
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }

        [Command]
        public async void DelContact()
        {
            try
            {
                if (this.SelectedMstItems.Count <= 0) return;

                if (DXSplashScreen.IsActive == false) DXSplashScreen.Show<ProgressWindow>();

                MessageBoxResult result = WinUIMessageBox.Show("[입고]" + SelectedMstItems.Count + "건 정말로 삭제 하시겠습니까?", "[삭제]" + _title, MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    int _Num = 0;
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("i66101/d", new StringContent(JsonConvert.SerializeObject(SelectedMstItems), System.Text.Encoding.UTF8, "application/json")))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string resultMsg = await response.Content.ReadAsStringAsync();
                            if (int.TryParse(resultMsg, out _Num) == false)
                            {
                                //실패
                                WinUIMessageBox.Show(resultMsg, _title, MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                            
                            Refresh();
                        }
                    }
                }

                if (DXSplashScreen.IsActive == true)
                {
                    DXSplashScreen.Close();
                }
            }
            catch (Exception eLog)
            {
                if (DXSplashScreen.IsActive == true)
                {
                    DXSplashScreen.Close();
                }
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }

        [Command]
        public void NewDtlPurContact()
        {
            try
            {
                detailPurDialog = new I66101DetailPurDialog();
                detailPurDialog.Title = "발주 입고 관리 ";
                detailPurDialog.Owner = Application.Current.MainWindow;
                detailPurDialog.BorderEffect = BorderEffect.Default;
                detailPurDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
                detailPurDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
                bool isDialog = (bool)detailPurDialog.ShowDialog();
                {
                    Refresh();
                }
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }

        [Command]
        public void ReportContact()
        {
            try
            {
                if (this.SelectedMstItem == null) return;

                int tmpIMP_ITM_AMT = 0;
                int tmpITM_QTY = 0;

                IList<InvVo> printDao = new List<InvVo>();

                if (SelectedMstItems.Count > 0)
                {
                    for (int x = 0; x < SelectedMstItems.Count; x++)
                    {
                        SelectedMstItems[x].GRP_NM = "[입고 일자 (From) " + (StartDt).ToString("yyyy-MM-dd HH:mm") + " ~ (To) " + (EndDt).ToString("yyyy-MM-dd HH:mm") + ", 사업장: " + SelectedMstItems[x].AREA_NM + "]";
                        tmpIMP_ITM_AMT += Convert.ToInt32(SelectedMstItems[x].IMP_ITM_AMT);
                        tmpITM_QTY += Convert.ToInt32(SelectedMstItems[x].ITM_QTY);

                        SelectedMstItems[x].TMP_A_QTY = tmpIMP_ITM_AMT;
                        SelectedMstItems[x].TMP_B_QTY = tmpITM_QTY;
                    }

                    I6610Report report = new I6610Report(SelectedMstItems);
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
                    window.Title = "입고내역 출력";
                    window.Owner = Application.Current.MainWindow;
                    window.ShowDialog();
                }
            }
            catch (Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }


        #region Data Binding List

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
        private string _M_SEARCH_TEXT = string.Empty;
        public string M_SEARCH_TEXT
        {
            get { return _M_SEARCH_TEXT; }
            set { SetProperty(ref _M_SEARCH_TEXT, value, () => M_SEARCH_TEXT); }
        }

        //사업장
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

        // MST 
        private IList<InvVo> selectedMstList = new List<InvVo>();
        public IList<InvVo> SelectMstList
        {
            get { return selectedMstList; }
            set { SetProperty(ref selectedMstList, value, () => SelectMstList); }
        }

        private InvVo _selectedMstItem;
        public InvVo SelectedMstItem
        {
            get { return _selectedMstItem; }
            set { if (value != null) SetProperty(ref _selectedMstItem, value, () => SelectedMstItem, SelectMstDetail); }
        }

        private IList<InvVo> selectedMstItemsList = new List<InvVo>();
        public IList<InvVo> SelectedMstItems
        {
            get { return selectedMstItemsList; }
            set { SetProperty(ref selectedMstItemsList, value, () => SelectedMstItems); }
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

        public async void SYSTEM_CODE_VO()
        {
            try
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
            }
            catch (Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }

    }
}
