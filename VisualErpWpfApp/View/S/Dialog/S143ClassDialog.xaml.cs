using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Code;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;
using AquilaErpWpfApp3.Util;


namespace AquilaErpWpfApp3.View.S.Dialog
{
    /// <summary>
    /// Interaction logic for ConfigView1Dialog.xaml
    /// </summary>
    public partial class S143ClassDialog : DXWindow
    {
        private string title = "거래처 관리";
        //private static CodeServiceClient customerClient = SystemProperties.CodeClient;
        //private Dictionary<string, string> coTpCdMap = new Dictionary<string, string>();
        private IList<SystemCodeVo> totalItem;
        private SystemCodeVo orgDao;

        public S143ClassDialog(SystemCodeVo Dao)
        {
            InitializeComponent();

            this.orgDao = Dao;

            SYSTEM_CODE_VO();
            //this.lue_CO_TP_CD.ItemsSource = SystemProperties.SYSTEM_CODE_VO("S-004");
            //this.coTpCdMap = SystemProperties.SYSTEM_CODE_MAP("S-004");
            //
            searchClass();
            //
            this.btn_add.Click += btn_add_Click;
            this.btn_del.Click += btn_del_Click;
            this.btn_reset.Click += btn_reset_Click;
            //
            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
            this.OKButton.Click += new RoutedEventHandler(OKButton_Click);
            this.CancelButton.Click += new RoutedEventHandler(CancelButton_Click);
        }

        void btn_reset_Click(object sender, RoutedEventArgs e)
        {
            searchClass();
        }

        void btn_del_Click(object sender, RoutedEventArgs e)
        {
            SystemCodeVo masterDomain = (SystemCodeVo)ConfigViewPage1Edit_Master.SelectedItem;
            if (masterDomain == null)
            {
                return;
            }
            //this.totalItem = (ObservableCollection<CustomerCodeVo>)this.ConfigViewPage1Edit_Master.ItemsSource;
            this.totalItem.Remove(masterDomain);
            //
            this.ConfigViewPage1Edit_Master.ItemsSource = this.totalItem;
            this.ConfigViewPage1Edit_Master.RefreshData();
        }

        void btn_add_Click(object sender, RoutedEventArgs e)
        {
            //this.totalItem = (ObservableCollection<CustomerCodeVo>)this.ConfigViewPage1Edit_Master.ItemsSource;
            this.totalItem.Insert(this.totalItem.Count, new SystemCodeVo() { CO_NO = orgDao.CO_NO, CO_NM = orgDao.CO_NM, CO_GRD_CD = "C" });
            this.configViewPage1EditView_Master.FocusedRowHandle = this.totalItem.Count - 1;
        }

        #region Functon (OKButton_Click, CancelButton_Click)
        private async void OKButton_Click(object sender, RoutedEventArgs e)
        {
            IList<string> dupList = new List<string>();
            int nCnt = this.totalItem.Count;

            if(nCnt <= 0){
                WinUIMessageBox.Show("[거래처 유형]데이터가 존재 하지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int x = 0;
            //SystemCodeVo resultVo;
            SystemCodeVo tmpVo;
            for (x = 0; x < nCnt; x++)
            {
                tmpVo = this.totalItem[x];
                //tmpVo.CO_TP_CD = (this.coTpCdMap.ContainsKey(tmpVo.CO_TP_NM) ? this.coTpCdMap[tmpVo.CO_TP_NM] : "");

                tmpVo.CRE_USR_ID = SystemProperties.USER;
                tmpVo.UPD_USR_ID = SystemProperties.USER;
                tmpVo.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;

                if (string.IsNullOrEmpty(tmpVo.CO_NO))
                {
                    WinUIMessageBox.Show("[거래처 코드] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                    this.configViewPage1EditView_Master.FocusedRowHandle = x;
                    return ;

                }
                else if (string.IsNullOrEmpty(tmpVo.CO_TP_NM) || string.IsNullOrEmpty(tmpVo.CO_TP_CD))
                {
                    WinUIMessageBox.Show("[거래처 유형] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                    this.configViewPage1EditView_Master.FocusedRowHandle = x;
                    return ;
                }
                else if (string.IsNullOrEmpty(tmpVo.CO_GRD_CD))
                {
                    WinUIMessageBox.Show("[등급] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                    this.configViewPage1EditView_Master.FocusedRowHandle = x;
                    return ;
                }
                else 
                {
                    if (dupList.Contains(tmpVo.CO_NO + "_" + tmpVo.CO_TP_CD))
                    {
                        WinUIMessageBox.Show("[코드 - 중복] 코드를 다시 입력 하십시오.", "[중복검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                        this.configViewPage1EditView_Master.FocusedRowHandle = x;
                        return;
                    }
                    dupList.Add(tmpVo.CO_NO + "_" + tmpVo.CO_TP_CD);
                }
                //else
                //{
                //    customerClient.DeleteCustomerClassCode(tmpVo);
                //    resultVo = customerClient.InsertCustomerClassCode(tmpVo);
                //    if (!resultVo.isSuccess)
                //    {
                //        //실패
                //        WinUIMessageBox.Show(resultVo.Message, "[자재 관리 시스템]거래처 관리", MessageBoxButton.OK, MessageBoxImage.Error);
                //        return;
                //    }
                //}
            }
            //
            //customerClient.DeleteCustomerClassCode(this.totalItem[0]);
            int _Num = 0;
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s143/class/d", new StringContent(JsonConvert.SerializeObject(this.totalItem[0]), System.Text.Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    if (int.TryParse(result, out _Num) == false)
                    {
                        //실패
                        WinUIMessageBox.Show(result, title, MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    //성공
                    // WinUIMessageBox.Show("완료 되었습니다", title, MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            for (x = 0; x < nCnt; x++)
            {
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s143/class/i", new StringContent(JsonConvert.SerializeObject(this.totalItem[x]), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();
                        if (int.TryParse(result, out _Num) == false)
                        {
                            //실패
                            WinUIMessageBox.Show(result, title, MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        //성공
                        // WinUIMessageBox.Show("완료 되었습니다", title, MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                //resultVo = customerClient.InsertCustomerClassCode(this.totalItem[x]);
                //if (!resultVo.isSuccess)
                //{
                //    //실패
                //    WinUIMessageBox.Show(resultVo.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
                //    return;
                //}
            }
            WinUIMessageBox.Show("[거래처 등급 관리]" + orgDao.CO_NM + " 완료 되었습니다", "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Information);
            this.DialogResult = true;
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void HandleEsc(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.DialogResult = false;
                Close();
            }
        }
        #endregion

        private async void searchClass()
        {
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s143/class", new StringContent(JsonConvert.SerializeObject(orgDao), System.Text.Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.totalItem = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                    this.ConfigViewPage1Edit_Master.ItemsSource = this.totalItem;
                }
            }
        }

        public async void SYSTEM_CODE_VO()
        {
            //this.lue_CO_TP_CD.ItemsSource = SystemProperties.SYSTEM_CODE_VO("S-004");
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "S-004"))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.lue_CO_TP_CD.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                }
            }
        }




        private void GridColumn_Validate(object sender, DevExpress.Xpf.Grid.GridCellValidationEventArgs e)
        {
            try
            {
                //
                SystemCodeVo masterDomain = (SystemCodeVo)ConfigViewPage1Edit_Master.GetFocusedRow();
                bool coTpNm = (e.Column.FieldName.ToString().Equals("CO_TP_NM") ? true : false);

                IList<GridColumn> columns = this.configViewPage1EditView_Master.VisibleColumns;
                //
                if (coTpNm)
                {
                    if (e.IsValid)
                    {
                        SystemCodeVo bankIoDao = null;
                        if (e.Value == null)
                        {
                            masterDomain.CO_TP_NM = "";
                            masterDomain.CO_TP_CD = "";
                        }
                        else
                        {
                            bankIoDao = this.lue_CO_TP_CD.GetItemFromValue(e.Value) as SystemCodeVo;
                            if (masterDomain.CO_TP_CD == null)
                            {
                                masterDomain.CO_TP_CD = string.Empty;
                            }
                            //
                            if (bankIoDao != null)
                            {
                                masterDomain.CO_TP_NM = bankIoDao.CLSS_DESC;
                                masterDomain.CO_TP_CD = bankIoDao.CLSS_CD;
                            }
                        }
                        //
                    }
                }
            }
            catch (System.Exception eLog)
            {
                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                e.ErrorContent = eLog.Message;
                e.SetError(e.ErrorContent, e.ErrorType);
                return;
            }
        }


        private void viewPage1EditView_HiddenEditor(object sender, DevExpress.Xpf.Grid.EditorEventArgs e)
        {
            this.configViewPage1EditView_Master.CommitEditing();
        }
    }
}
