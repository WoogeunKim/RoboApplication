using AquilaErpWpfApp3.Util;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Man;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;

namespace AquilaErpWpfApp3.View.M.Dialog
{
    /// <summary>
    /// Interaction logic for ConfigView1Dialog.xaml
    /// </summary>
    public partial class M6710DetailInItmDialog : DXWindow
    {
        //private static CodeServiceClient codeClient = SystemProperties.CodeClient;
        //private Dictionary<string, string> coTpCdMap = new Dictionary<string, string>();
        private IList<ManVo> totalItemM;
        private IList<ManVo> totalItemD;
        private ManVo orgDao;

        private string title = "투입수량 현황";

        public M6710DetailInItmDialog(ManVo Dao)
        {
            InitializeComponent();

            this.orgDao = Dao;

            this.text_LOT_DIV_NO.Text = this.orgDao.LOT_DIV_NO;

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

        //async void btn_del_Click(object sender, RoutedEventArgs e)
        //{
        //    ManVo delVo = (ManVo)ConfigViewPage1Edit_Master.SelectedItem;
        //    if (delVo == null)
        //    {
        //        return;
        //    }

        //    MessageBoxResult result = WinUIMessageBox.Show("[" + delVo.BAD_NM + " / " + delVo.BAD_QTY + "]" + " 정말로 삭제 하시겠습니까?", "[삭제]" + title, MessageBoxButton.YesNo, MessageBoxImage.Question);
        //    if (result == MessageBoxResult.Yes)
        //    {
        //        this.totalItem.Remove(delVo);

        //        this.btn_save.IsEnabled = true;
        //    }
        //    this.ConfigViewPage1Edit_Master.RefreshData();

        //    //    //this.totalItem = (ObservableCollection<CustomerCodeVo>)this.ConfigViewPage1Edit_Master.ItemsSource;
        //    //    this.totalItem.Remove(delVo);
        //    //    //
        //    //    //this.ConfigViewPage1Edit_Master.ItemsSource = this.totalItem;
        //}

        //async void btn_add_Click(object sender, RoutedEventArgs e)
        //{
        //    this.totalItem.Insert(this.totalItem.Count, new ManVo() { CRE_USR_ID = SystemProperties.USER, UPD_USR_ID = SystemProperties.USER, CHNL_CD = SystemProperties.USER_VO.CHNL_CD, LOT_DIV_NO = this.orgDao.LOT_DIV_NO, ROUT_CD = this.orgDao.ROUT_CD, ROUT_ITM_CD = this.orgDao.ROUT_ITM_CD });
        //    this.configViewPage1EditView_Master.FocusedRowHandle = this.totalItem.Count - 1;
        //
        //  this.ConfigViewPage1Edit_Master.RefreshData();
        //}

        #region Functon (OKButton_Click, CancelButton_Click)
        private async void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int nCnt = this.totalItemM.Count;
                 


                ////삭제
                //using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6710/dtl4/d", new StringContent(JsonConvert.SerializeObject(this.orgDao), System.Text.Encoding.UTF8, "application/json")))
                //{
                //    if (response.IsSuccessStatusCode)
                //    {
                //        int _Num = 0;
                //        string resultMsg = await response.Content.ReadAsStringAsync();
                //        if (int.TryParse(resultMsg, out _Num) == false)
                //        {
                //            //실패
                //            WinUIMessageBox.Show(resultMsg, title, MessageBoxButton.OK, MessageBoxImage.Error);
                //            return;
                //        }
                //    }
                //}

                if (nCnt > 0)
                {
                    //저장
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6710/dtl4/m", new StringContent(JsonConvert.SerializeObject(this.totalItemM), System.Text.Encoding.UTF8, "application/json")))
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

                searchClass();
                //this.DialogResult = true;
                //this.Close();
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void HandleEsc(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.DialogResult = true;
                Close();
            }
        }
        #endregion

        private async void searchClass()
        {
            //투입자재 불량 현황
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6710/dtl4/dtl", new StringContent(JsonConvert.SerializeObject(this.orgDao), System.Text.Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.totalItemM = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
                    if (this.totalItemM == null)
                    {
                        this.totalItemM = new List<ManVo>();
                    }
                    this.ConfigViewPage1Edit_Master.ItemsSource = this.totalItemM;
                }
            }


            //투입자재 불량 현황
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6710/dtl5/dtl", new StringContent(JsonConvert.SerializeObject(this.orgDao), System.Text.Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.totalItemD = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
                    if (this.totalItemD == null)
                    {
                        this.totalItemD = new List<ManVo>();
                    }
                    this.ConfigViewPage1Edit_Detail.ItemsSource = this.totalItemD;
                }
            }
        }

        private async void GridColumn_Validate(object sender, DevExpress.Xpf.Grid.GridCellValidationEventArgs e)
        {
            try
            {
                ManVo masterDomain = (ManVo)ConfigViewPage1Edit_Master.GetFocusedRow();


                    bool useQty = (e.Column.FieldName.ToString().Equals("USE_QTY") ? true : false);
                //    bool badNm = (e.Column.FieldName.ToString().Equals("BAD_NM") ? true : false);
                //    bool badRsn = (e.Column.FieldName.ToString().Equals("BAD_RSN") ? true : false);

                //    if (badNm)
                //    {
                //        if (e.IsValid)
                //        {
                //            if (string.IsNullOrEmpty(masterDomain.BAD_CD))
                //            {
                //                masterDomain.BAD_CD = "";
                //                masterDomain.BAD_NM = "";
                //            }
                //            //
                //            if (!masterDomain.BAD_NM.Equals((e.Value == null ? "" : e.Value.ToString())))
                //            {
                //                TecVo bankIoDao = this.lue_BAD_NM.GetItemFromValue(e.Value) as TecVo;
                //                //
                //                masterDomain.BAD_CD = bankIoDao.BAD_CD;
                //                masterDomain.BAD_NM = bankIoDao.BAD_NM;

                //                //
                //                masterDomain.ITM_CD = this.orgDao.ITM_CD;
                //                masterDomain.CRE_USR_ID = SystemProperties.USER;
                //                masterDomain.UPD_USR_ID = SystemProperties.USER;
                //                masterDomain.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;

                //                this.btn_save.IsEnabled = true;
                //            }
                //        }
                //    }
                //    else
                if (useQty)
                {
                    if (e.IsValid)
                    {
                        if (string.IsNullOrEmpty(masterDomain.USE_QTY?.ToString()))
                        {
                            masterDomain.USE_QTY = "0";
                        }
                        //
                        if (!masterDomain.USE_QTY.Equals((e.Value == null ? "" : e.Value.ToString())))
                        {
                            //사용 수량
                            masterDomain.USE_QTY = e.Value;

                            masterDomain.LOT_DIV_NO = this.orgDao.LOT_DIV_NO;
                            masterDomain.LOT_DIV_SEQ = this.orgDao.LOT_DIV_SEQ;
                            masterDomain.ROUT_CD = this.orgDao.ROUT_CD;

                            //잔량
                            masterDomain.RMN_QTY = Convert.ToInt32(masterDomain.SL_ITM_QTY) - Convert.ToInt32(masterDomain.USE_QTY);

                            //masterDomain.ITM_CD = this.orgDao.ITM_CD;
                            masterDomain.CRE_USR_ID = SystemProperties.USER;
                            masterDomain.UPD_USR_ID = SystemProperties.USER;
                            masterDomain.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
                            masterDomain.isCheckd = true;

                            this.btn_save.IsEnabled = true;
                        }
                    }
                }
                //    else if (badQty)
                //    {
                //        if (e.IsValid)
                //        {
                //            if (string.IsNullOrEmpty(masterDomain.BAD_RSN))
                //            {
                //                masterDomain.BAD_RSN = "";
                //            }
                //            //
                //            if (!masterDomain.BAD_RSN.Equals((e.Value == null ? "" : e.Value.ToString())))
                //            {
                //                masterDomain.BAD_RSN = e.Value.ToString();

                //                masterDomain.ITM_CD = this.orgDao.ITM_CD;
                //                masterDomain.CRE_USR_ID = SystemProperties.USER;
                //                masterDomain.UPD_USR_ID = SystemProperties.USER;
                //                masterDomain.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;

                //                this.btn_save.IsEnabled = true;
                //            }
                //        }
                //    }
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

            //using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("t91103/", new StringContent(JsonConvert.SerializeObject(new TecVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD, ROUT_CD = this.orgDao.ROUT_CD, DELT_FLG = "N" }), System.Text.Encoding.UTF8, "application/json")))
            //{
            //    if (response.IsSuccessStatusCode)
            //    {
            //        this.lue_BAD_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<TecVo>>(await response.Content.ReadAsStringAsync()).Cast<TecVo>().ToList();
            //    }
            //}
        }

        private async void btn_Delete_Click(object sender, RoutedEventArgs e)
        {

            ManVo delDao = (ManVo)ConfigViewPage1Edit_Detail.GetFocusedRow();

            MessageBoxResult result = WinUIMessageBox.Show(delDao.GBN + " / " + delDao.CRE_DT + " / " + delDao.LOT_NO + " 정말로 삭제 하시겠습니까?", title, MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {

                //삭제
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6710/dtl5/d", new StringContent(JsonConvert.SerializeObject(delDao), System.Text.Encoding.UTF8, "application/json")))
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


                        //투입자재 불량 현황
                        using (HttpResponseMessage response_X = await SystemProperties.PROGRAM_HTTP.PostAsync("m6710/dtl5/dtl", new StringContent(JsonConvert.SerializeObject(this.orgDao), System.Text.Encoding.UTF8, "application/json")))
                        {
                            if (response_X.IsSuccessStatusCode)
                            {
                                this.totalItemD = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response_X.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
                                if (this.totalItemD == null)
                                {
                                    this.totalItemD = new List<ManVo>();
                                }
                                this.ConfigViewPage1Edit_Detail.ItemsSource = this.totalItemD;
                            }
                        }
                    }
                }
            }


        }
    }
}
