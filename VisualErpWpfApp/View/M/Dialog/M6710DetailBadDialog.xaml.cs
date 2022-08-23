using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Code;
using ModelsLibrary.Man;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;
using AquilaErpWpfApp3.Util;
using ModelsLibrary.Tec;

namespace AquilaErpWpfApp3.View.M.Dialog
{
    /// <summary>
    /// Interaction logic for ConfigView1Dialog.xaml
    /// </summary>
    public partial class M6710DetailBadDialog : DXWindow
    {
        //private static CodeServiceClient codeClient = SystemProperties.CodeClient;
        //private Dictionary<string, string> coTpCdMap = new Dictionary<string, string>();
        private IList<ManVo> totalItem;
        private ManVo orgDao;

        private string title = "불량 현황";

        public M6710DetailBadDialog(ManVo Dao)
        {
            InitializeComponent();

            this.orgDao = Dao;

            SYSTEM_CODE_VO();

            searchClass();

            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
            //this.OKButton.Click += new RoutedEventHandler(OKButton_Click);
            //this.CancelButton.Click += new RoutedEventHandler(CancelButton_Click);
        }

        void btn_reset_Click(object sender, RoutedEventArgs e)
        {
            searchClass();
        }

        async void btn_del_Click(object sender, RoutedEventArgs e)
        {
            ManVo delVo = (ManVo)ConfigViewPage1Edit_Master.SelectedItem;
            if (delVo == null)
            {
                return;
            }

            MessageBoxResult result = WinUIMessageBox.Show("[" + delVo.BAD_NM + " / " + delVo.BAD_QTY + "]" + " 정말로 삭제 하시겠습니까?", "[삭제]" + title, MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                this.totalItem.Remove(delVo);

                this.btn_save.IsEnabled = true;
            }
            this.ConfigViewPage1Edit_Master.RefreshData();

            //    //this.totalItem = (ObservableCollection<CustomerCodeVo>)this.ConfigViewPage1Edit_Master.ItemsSource;
            //    this.totalItem.Remove(delVo);
            //    //
            //    //this.ConfigViewPage1Edit_Master.ItemsSource = this.totalItem;
        }

        async void btn_add_Click(object sender, RoutedEventArgs e)
        {
            this.totalItem.Insert(this.totalItem.Count, new ManVo() { CRE_USR_ID = SystemProperties.USER, UPD_USR_ID = SystemProperties.USER, CHNL_CD = SystemProperties.USER_VO.CHNL_CD, LOT_DIV_NO = this.orgDao.LOT_DIV_NO, LOT_DIV_SEQ = this.orgDao.LOT_DIV_SEQ, ROUT_CD = this.orgDao.ROUT_CD, ROUT_ITM_CD = this.orgDao.ROUT_ITM_CD });
            this.configViewPage1EditView_Master.FocusedRowHandle = this.totalItem.Count - 1;

            this.ConfigViewPage1Edit_Master.RefreshData();
        }

        #region Functon (OKButton_Click, CancelButton_Click)
        private async void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int nCnt = this.totalItem.Count;
                 
                //삭제
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6710/dtl2/d", new StringContent(JsonConvert.SerializeObject(this.orgDao), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        int _Num = 0;
                        string resultMsg = await response.Content.ReadAsStringAsync();
                        if (int.TryParse(resultMsg, out _Num) == false)
                        {
                            //실패
                            WinUIMessageBox.Show(resultMsg, title, MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    }
                }

                if (nCnt > 0)
                {
                    //저장
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6710/dtl2/m", new StringContent(JsonConvert.SerializeObject(this.totalItem), System.Text.Encoding.UTF8, "application/json")))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            int _Num = 0;
                            string resultMsg = await response.Content.ReadAsStringAsync();
                            if (int.TryParse(resultMsg, out _Num) == false)
                            {
                                //실패
                                WinUIMessageBox.Show(resultMsg, title, MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                        }
                    }
                }
                WinUIMessageBox.Show("완료 되었습니다", "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Information);
                this.DialogResult = true;
                this.Close();
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
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
            //불량 현황
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6710/dtl2", new StringContent(JsonConvert.SerializeObject(this.orgDao), System.Text.Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.totalItem = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
                    if (this.totalItem == null)
                    {
                        this.totalItem = new List<ManVo>();
                    }
                    this.ConfigViewPage1Edit_Master.ItemsSource = this.totalItem;
                }
            }
        }

        private async void GridColumn_Validate(object sender, DevExpress.Xpf.Grid.GridCellValidationEventArgs e)
        {
            try
            {
                ManVo masterDomain = (ManVo)ConfigViewPage1Edit_Master.GetFocusedRow();


                bool badNm = (e.Column.FieldName.ToString().Equals("BAD_NM") ? true : false);
                bool badQty = (e.Column.FieldName.ToString().Equals("BAD_QTY") ? true : false);
                bool badRsn = (e.Column.FieldName.ToString().Equals("BAD_RSN") ? true : false);

                if (badNm)
                {
                    if (e.IsValid)
                    {
                        if (string.IsNullOrEmpty(masterDomain.BAD_CD))
                        {
                            masterDomain.BAD_CD = "";
                            masterDomain.BAD_NM = "";
                        }
                        //
                        if (!masterDomain.BAD_NM.Equals((e.Value == null ? "" : e.Value.ToString())))
                        {
                            TecVo bankIoDao = this.lue_BAD_NM.GetItemFromValue(e.Value) as TecVo;
                            //
                            masterDomain.BAD_CD = bankIoDao.BAD_CD;
                            masterDomain.BAD_NM = bankIoDao.BAD_NM;

                            //
                            masterDomain.CRE_USR_ID = SystemProperties.USER;
                            masterDomain.UPD_USR_ID = SystemProperties.USER;
                            masterDomain.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;

                            this.btn_save.IsEnabled = true;
                        }
                    }
                }
                else if (badQty)
                {
                    if (e.IsValid)
                    {
                        if (string.IsNullOrEmpty(masterDomain.BAD_QTY?.ToString()))
                        {
                            masterDomain.BAD_QTY = "0";
                        }
                        //
                        if (!masterDomain.BAD_QTY.Equals((e.Value == null ? "" : e.Value.ToString())))
                        {
                            masterDomain.BAD_QTY = e.Value;

                            masterDomain.CRE_USR_ID = SystemProperties.USER;
                            masterDomain.UPD_USR_ID = SystemProperties.USER;
                            masterDomain.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;

                            this.btn_save.IsEnabled = true;
                        }
                    }
                }
                else if (badRsn)
                {
                    if (e.IsValid)
                    {
                        if (string.IsNullOrEmpty(masterDomain.BAD_RSN))
                        {
                            masterDomain.BAD_RSN = "";
                        }
                        //
                        if (!masterDomain.BAD_RSN.Equals((e.Value == null ? "" : e.Value.ToString())))
                        {
                            masterDomain.BAD_RSN = e.Value.ToString();

                            masterDomain.CRE_USR_ID = SystemProperties.USER;
                            masterDomain.UPD_USR_ID = SystemProperties.USER;
                            masterDomain.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;

                            this.btn_save.IsEnabled = true;
                        }
                    }
                }
            }
            catch (Exception eLog)
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

        public async void SYSTEM_CODE_VO()
        {

            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("t91103/", new StringContent(JsonConvert.SerializeObject(new TecVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD, ROUT_CD = this.orgDao.ROUT_CD, DELT_FLG = "N" }), System.Text.Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.lue_BAD_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<TecVo>>(await response.Content.ReadAsStringAsync()).Cast<TecVo>().ToList();
                }
            }
        }
    }
}
