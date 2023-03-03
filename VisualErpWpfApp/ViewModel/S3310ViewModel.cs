using AquilaErpWpfApp3.Util;
using AquilaErpWpfApp3.View.SAL.Dialog;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.Core;
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
using System.Windows.Media;

namespace AquilaErpWpfApp3.ViewModel
{
    public sealed class S3310ViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private string _title = "���Դܰ�����";
        private string _gbmCd = "P";

        private S3310MasterDialog masterDialog;

        public S3310ViewModel()
        {
            SYSTEM_CODE_VO();
        }

        #region Functon <Master List>

        [Command]
        public async void Refresh(string _CO_NM = null)
        {
            try
            {
                // ���÷��� ���� 
                if (DXSplashScreen.IsActive == false) DXSplashScreen.Show<ProgressWindow>();

                // ���� DTL ������ ����
                SearchDetail = null;
                SelectDtlList = null;

                // MST ������ ��ȸ (�ŷ�����, �����, ä���ڵ�)
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s3311/mst", new StringContent(JsonConvert.SerializeObject(new SaleVo() { GBN = _gbmCd, AREA_CD = M_SL_AREA_NM.CLSS_CD, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectMstList = JsonConvert.DeserializeObject<IEnumerable<SaleVo>>(await response.Content.ReadAsStringAsync()).Cast<SaleVo>().ToList();

                        // MST ������ ���� ��� ?  isM_UPDATE(DTL ��ȸ ��ư), isM_DELETE(����) Ȱ��ȭ 
                        if (SelectMstList.Count >= 1)
                        {
                            isM_UPDATE = true;
                            isM_DELETE = true;

                            if (string.IsNullOrEmpty(_CO_NM))
                            {
                                SelectedMstItem = SelectMstList[0];
                            }
                            else
                            {
                                // �߰��� �ŷ�ó�� �ٷ� ������.
                                SelectedMstItem = SelectMstList.Where(x => x.CO_NM.Equals(_CO_NM)).LastOrDefault<SaleVo>();
                            }
                        }
                        else
                        {

                            isM_DELETE = false;
                        }

                        // ��ȸ ����
                        Title = "[�����] " + M_SL_AREA_NM.CLSS_DESC;
                    }

                    // ���÷��� ����
                    if (DXSplashScreen.IsActive == true) DXSplashScreen.Close();
                }
            }
            catch (System.Exception eLog)
            {
                // ������ ���� ���÷��� ����
                if (DXSplashScreen.IsActive == true) DXSplashScreen.Close();

                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }
        #endregion

        #region Functon <Detail List>
        [Command]
        public async void RefreshDtl()
        {
            try
            {
                if (this.SelectedMstItem == null) return;

                // ���÷��� ���� 
                if (DXSplashScreen.IsActive == false) DXSplashScreen.Show<ProgressWindow>();

                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s3311/dtl", new StringContent(JsonConvert.SerializeObject(SelectedMstItem), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectDtlList = JsonConvert.DeserializeObject<IEnumerable<SaleVo>>(await response.Content.ReadAsStringAsync()).Cast<SaleVo>().ToList();
                    }
                }

                // ���÷��� ����
                if (DXSplashScreen.IsActive == true) DXSplashScreen.Close();
            }
            catch (System.Exception eLog)
            {
                // ������ ���� ���÷��� ����
                if (DXSplashScreen.IsActive == true) DXSplashScreen.Close();

                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }
        #endregion

        #region Functon <Master Delete>
        [Command]
        public async void DelContact()
        {

            try
            {
                if (SelectedMstItem == null) return;

                MessageBoxResult result = WinUIMessageBox.Show("[" + SelectedMstItem.FM_DT + " / " + SelectedMstItem.CO_NM + "]" + " ������ ���� �Ͻðڽ��ϱ�?", "[����]" + _title, MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("S3311/mst/d", new StringContent(JsonConvert.SerializeObject(SelectedMstItem), System.Text.Encoding.UTF8, "application/json")))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            int _Num = 0;
                            string resultMsg = await response.Content.ReadAsStringAsync();
                            if (int.TryParse(resultMsg, out _Num) == false)
                            {
                                //����
                                WinUIMessageBox.Show(resultMsg, _title, MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }

                            Refresh();

                            //����
                            WinUIMessageBox.Show("������ �Ϸ�Ǿ����ϴ�.", _title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
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
        #endregion

        #region Functon <New Dialog>
        [Command]
        public void NewContact()
        {
            masterDialog = new S3310MasterDialog(new SaleVo() { GBN = _gbmCd, CHNL_CD = SystemProperties.USER_VO.CHNL_CD, DELT_FLG = "N" });
            masterDialog.Title = _title + " - �߰�";
            masterDialog.Owner = Application.Current.MainWindow;
            masterDialog.BorderEffect = BorderEffect.Default;
            masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            bool isDialog = (bool)masterDialog.ShowDialog();
            if (isDialog)
            {
                Refresh(masterDialog.updateDao.CO_NM);
                //Refresh(masterDialog.updateDao.FM_DT + masterDialog.updateDao.CO_NM);
            }
        } 
        #endregion


        #region Binding Empty

        private string _M_SEARCH_TEXT = string.Empty;
        public string M_SEARCH_TEXT
        {
            get { return _M_SEARCH_TEXT; }
            set { SetProperty(ref _M_SEARCH_TEXT, value, () => M_SEARCH_TEXT); }
        }


        ////�����
        private IList<SystemCodeVo> _AreaCd = new List<SystemCodeVo>();
        public IList<SystemCodeVo> AreaList
        {
            get { return _AreaCd; }
            set { SetProperty(ref _AreaCd, value, () => AreaList); }
        }
        //�����
        private SystemCodeVo _M_SL_AREA_NM;
        public SystemCodeVo M_SL_AREA_NM
        {
            get { return _M_SL_AREA_NM; }
            set { SetProperty(ref _M_SL_AREA_NM, value, () => M_SL_AREA_NM); }
        }

        // MST 
        private IList<SaleVo> selectedMstList = new List<SaleVo>();
        public IList<SaleVo> SelectMstList
        {
            get { return selectedMstList; }
            set { SetProperty(ref selectedMstList, value, () => SelectMstList); }
        }
        SaleVo _selectedMstItem;
        public SaleVo SelectedMstItem
        {
            get { return _selectedMstItem; }
            set { SetProperty(ref _selectedMstItem, value, () => SelectedMstItem, RefreshDtl); }
        }


        // DTL
        private IList<SaleVo> selectedDtlList = new List<SaleVo>();
        public IList<SaleVo> SelectDtlList
        {
            get { return selectedDtlList; }
            set { SetProperty(ref selectedDtlList, value, () => SelectDtlList); }
        }

        SaleVo _searchDetail;
        public SaleVo SearchDetail
        {
            get { return _searchDetail; }
            set { SetProperty(ref _searchDetail, value, () => SearchDetail); }
        }


        // DTL ��ȸ :  CTRL+F5 ���
        private bool? _isM_UPDATE = false;
        public bool? isM_UPDATE
        {
            get { return _isM_UPDATE; }
            set { SetProperty(ref _isM_UPDATE, value, () => isM_UPDATE); }
        }


        // MST ����
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
            //SL_AREA_LIST = SystemProperties.SYSTEM_CODE_VO("L-002");
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
