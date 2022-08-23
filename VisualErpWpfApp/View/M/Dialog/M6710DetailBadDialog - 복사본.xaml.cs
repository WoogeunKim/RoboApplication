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
using AquilaErpWpfApp.Util;

namespace AquilaErpWpfApp.View.M.Dialog
{
    /// <summary>
    /// Interaction logic for ConfigView1Dialog.xaml
    /// </summary>
    public partial class M6710DetailBadDialog : DXWindow
    {
        //private static CodeServiceClient codeClient = SystemProperties.CodeClient;
        //private Dictionary<string, string> coTpCdMap = new Dictionary<string, string>();
        private IList<SystemCodeVo> totalItem;
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
            //SystemCodeVo delVo = (SystemCodeVo)ConfigViewPage1Edit_Master.SelectedItem;
            //if (delVo == null)
            //{
            //    return;
            //}

            //MessageBoxResult result = WinUIMessageBox.Show("[" + delVo.LOC_NM + "]" + " 정말로 삭제 하시겠습니까?", "[삭제]" + title, MessageBoxButton.YesNo, MessageBoxImage.Question);
            //if (result == MessageBoxResult.Yes)
            //{
            //    try
            //    {
            //        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s1144/mst/d", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { LOC_SEQ = delVo.LOC_SEQ, ITM_CD = this.orgDao.MOLD_NO, AREA_CD = SystemProperties.USER_VO.EMPE_PLC_NM, CRE_USR_ID = SystemProperties.USER, UPD_USR_ID = SystemProperties.USER, CHNL_CD = SystemProperties.USER_VO.CHNL_CD}), System.Text.Encoding.UTF8, "application/json")))
            //        {
            //            if (response.IsSuccessStatusCode)
            //            {
            //                int _Num = 0;
            //                string resultMsg = await response.Content.ReadAsStringAsync();
            //                if (int.TryParse(resultMsg, out _Num) == false)
            //                {
            //                    //실패
            //                    WinUIMessageBox.Show(resultMsg, title, MessageBoxButton.OK, MessageBoxImage.Error);
            //                    return;
            //                }
            //            }
            //        }
            //        //SystemCodeVo resultVo = codeClient.S1144DeleteMstHistory(new SystemCodeVo() { ITM_CD = this.orgDao.ITM_CD, AREA_CD = this.orgDao.AREA_CD, CRE_USR_ID = SystemProperties.USER, UPD_USR_ID = SystemProperties.USER });
            //        //resultVo = codeClient.S1144DeleteMst(new SystemCodeVo() { LOC_SEQ = delVo.LOC_SEQ, ITM_CD = this.orgDao.ITM_CD, AREA_CD = this.orgDao.AREA_CD });
            //        //if (!resultVo.isSuccess)
            //        //{
            //        //    //실패
            //        //    WinUIMessageBox.Show(resultVo.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
            //        //    return;
            //        //}
            //        WinUIMessageBox.Show("삭제가 완료되었습니다.", "[삭제]" + title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
            //        searchClass();
            //    }
            //    catch (System.Exception eLog)
            //    {
            //        WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
            //        return;
            //    }
            //}

        //    //this.totalItem = (ObservableCollection<CustomerCodeVo>)this.ConfigViewPage1Edit_Master.ItemsSource;
        //    this.totalItem.Remove(delVo);
        //    //
        //    //this.ConfigViewPage1Edit_Master.ItemsSource = this.totalItem;
        }

        async void btn_add_Click(object sender, RoutedEventArgs e)
        {
            //if (string.IsNullOrEmpty(this.combo_N1ST_LOC_NM.Text))
            //{
            //    WinUIMessageBox.Show("[동/면] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.combo_N1ST_LOC_NM.IsTabStop = true;
            //    this.combo_N1ST_LOC_NM.Focus();
            //    return;
            //}
            //else if (string.IsNullOrEmpty(this.combo_N2ND_LOC_NM.Text))
            //{
            //    WinUIMessageBox.Show("[층] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.combo_N2ND_LOC_NM.IsTabStop = true;
            //    this.combo_N2ND_LOC_NM.Focus();
            //    return;
            //}
            //else if (string.IsNullOrEmpty(this.combo_N3RD_LOC_NM.Text))
            //{
            //    WinUIMessageBox.Show("[열] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.combo_N3RD_LOC_NM.IsTabStop = true;
            //    this.combo_N3RD_LOC_NM.Focus();
            //    return;
            //}

            //searchClass();

            //SystemCodeVo N1ST_LOC_NM = this.combo_N1ST_LOC_NM.SelectedItem as SystemCodeVo;
            //SystemCodeVo N2ND_LOC_NM = this.combo_N2ND_LOC_NM.SelectedItem as SystemCodeVo;
            //SystemCodeVo N3RD_LOC_NM = this.combo_N3RD_LOC_NM.SelectedItem as SystemCodeVo;

            //using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s1144/mst/i", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { LOC_SEQ = this.totalItem.Count + 1, ITM_CD = this.orgDao.MOLD_NO, AREA_CD = SystemProperties.USER_VO.EMPE_PLC_NM, CRE_USR_ID = SystemProperties.USER, UPD_USR_ID = SystemProperties.USER, N1ST_LOC_ID = N1ST_LOC_NM.CLSS_CD, N1ST_LOC_NM = N1ST_LOC_NM.CLSS_DESC, N2ND_LOC_ID = N2ND_LOC_NM.CLSS_CD, N2ND_LOC_NM = N2ND_LOC_NM.CLSS_DESC, N3RD_LOC_ID = N3RD_LOC_NM.CLSS_CD, N3RD_LOC_NM = N3RD_LOC_NM.CLSS_DESC, LOC_NM = N1ST_LOC_NM.CLSS_DESC + N2ND_LOC_NM.CLSS_DESC + "-" + N3RD_LOC_NM.CLSS_DESC, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
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


            ////SystemCodeVo resultVo = codeClient.S1144InsertMst(new SystemCodeVo() { LOC_SEQ = this.totalItem.Count + 1, ITM_CD = this.orgDao.ITM_CD, AREA_CD = this.orgDao.AREA_CD, CRE_USR_ID = SystemProperties.USER, UPD_USR_ID = SystemProperties.USER, N1ST_LOC_ID = N1ST_LOC_NM.CLSS_CD, N1ST_LOC_NM = N1ST_LOC_NM.CLSS_DESC, N2ND_LOC_ID = N2ND_LOC_NM.CLSS_CD, N2ND_LOC_NM = N2ND_LOC_NM.CLSS_DESC, N3RD_LOC_ID = N3RD_LOC_NM.CLSS_CD, N3RD_LOC_NM = N3RD_LOC_NM.CLSS_DESC, LOC_NM = N1ST_LOC_NM.CLSS_DESC + N2ND_LOC_NM.CLSS_DESC + "-" + N3RD_LOC_NM.CLSS_DESC});
            ////if (!resultVo.isSuccess)
            ////{
            ////    //실패
            ////    WinUIMessageBox.Show(resultVo.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
            ////    return;
            ////}
            ////else
            ////{
            ////    resultVo = codeClient.S1144UpdateMstHistory(new SystemCodeVo() { ITM_CD = this.orgDao.ITM_CD, AREA_CD = this.orgDao.AREA_CD, CRE_USR_ID = SystemProperties.USER, UPD_USR_ID = SystemProperties.USER});
            ////}

            //////this.totalItem = (ObservableCollection<CustomerCodeVo>)this.ConfigViewPage1Edit_Master.ItemsSource;
            ////for (int x = 0; x < this.totalItem.Count; x++)
            ////{
            ////    this.totalItem[x].LOC_SEQ = x+1;
            ////}

            ////this.totalItem.Insert(this.totalItem.Count, new SystemCodeVo() { LOC_SEQ = this.totalItem.Count+1 });
            ////this.configViewPage1EditView_Master.FocusedRowHandle = this.totalItem.Count - 1;

            //searchClass();
        }

        #region Functon (OKButton_Click, CancelButton_Click)
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            //SystemCodeVo resultVo;
            //int nCnt = this.totalItem.Count;
            //if (nCnt <= 0)
            //{
            //    //삭제
            //    resultVo = codeClient.S1144DeleteMst(new SystemCodeVo() { ITM_CD = this.orgDao.ITM_CD, AREA_CD = this.orgDao.AREA_CD });
            //    //WinUIMessageBox.Show("데이터가 존재 하지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
            //    //return;
            //}
            //else
            //{
            //    //삭제
            //    resultVo = codeClient.S1144DeleteMst(new SystemCodeVo() { ITM_CD = this.orgDao.ITM_CD, AREA_CD = this.orgDao.AREA_CD });

            //    //추가
            //    for (int x = 0; x < nCnt; x++)
            //    {
            //        this.totalItem[x].ITM_CD = this.orgDao.ITM_CD;
            //        this.totalItem[x].AREA_CD = this.orgDao.AREA_CD;
            //        this.totalItem[x].CRE_USR_ID = SystemProperties.USER;
            //        this.totalItem[x].UPD_USR_ID = SystemProperties.USER;
            //        resultVo = codeClient.S1144InsertMst(this.totalItem[x]);
            //        if (!resultVo.isSuccess)
            //        {
            //            //실패
            //            WinUIMessageBox.Show(resultVo.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
            //            return;
            //        }
            //    }
            //}
            //WinUIMessageBox.Show("완료 되었습니다", "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Information);
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
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s1144/dtl", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD, ITM_CD = this.orgDao.MOLD_NO }), System.Text.Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.totalItem = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                    this.ConfigViewPage1Edit_Master.ItemsSource = this.totalItem;
                }
                //this.totalItem = new ObservableCollection<SystemCodeVo>((IList<SystemCodeVo>)codeClient.S1144SelectDtlList(orgDao));
                //this.ConfigViewPage1Edit_Master.ItemsSource = this.totalItem;
            }
        }

        private async void GridColumn_Validate(object sender, DevExpress.Xpf.Grid.GridCellValidationEventArgs e)
        {
            try
            {
                SystemCodeVo masterDomain = (SystemCodeVo)ConfigViewPage1Edit_Master.GetFocusedRow();

                //bool n1stLocNm = (e.Column.FieldName.ToString().Equals("N1ST_LOC_NM") ? true : false);
                //bool n2ndLocNm = (e.Column.FieldName.ToString().Equals("N2ND_LOC_NM") ? true : false);
                //bool n3rdLocNm = (e.Column.FieldName.ToString().Equals("N3RD_LOC_NM") ? true : false);
                //
                //if (n1stLocNm)
                //{
                //    if (e.IsValid)
                //    {
                //        if (string.IsNullOrEmpty(masterDomain.N1ST_LOC_NM))
                //        {
                //            masterDomain.N1ST_LOC_ID = "";
                //            masterDomain.N1ST_LOC_NM = "";
                //        }
                //        //
                //        if (!masterDomain.N1ST_LOC_NM.Equals((e.Value == null ? "" : e.Value.ToString())))
                //        {
                //            SystemCodeVo bankIoDao = this.lue_N1ST_LOC_ID.GetItemFromValue(e.Value) as SystemCodeVo;
                //            //
                //            masterDomain.N1ST_LOC_ID = bankIoDao.CLSS_CD;
                //            masterDomain.N1ST_LOC_NM = bankIoDao.CLSS_DESC;
                //            //
                //            masterDomain.LOC_NM = masterDomain.N1ST_LOC_NM + masterDomain.N2ND_LOC_NM + "-" + masterDomain.N3RD_LOC_NM;
                //            masterDomain.ITM_CD = this.orgDao.ITM_CD;
                //            masterDomain.AREA_CD = SystemProperties.USER_VO.EMPE_PLC_NM;/*this.orgDao.AREA_CD;*/
                //            masterDomain.CRE_USR_ID = SystemProperties.USER;
                //            masterDomain.UPD_USR_ID = SystemProperties.USER;
                //            masterDomain.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;

                //            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s1144/mst/u", new StringContent(JsonConvert.SerializeObject(masterDomain), System.Text.Encoding.UTF8, "application/json")))
                //            {
                //                if (response.IsSuccessStatusCode)
                //                {
                //                    int _Num = 0;
                //                    string resultMsg = await response.Content.ReadAsStringAsync();
                //                    if (int.TryParse(resultMsg, out _Num) == false)
                //                    {
                //                        //실패
                //                        WinUIMessageBox.Show(resultMsg, title, MessageBoxButton.OK, MessageBoxImage.Error);
                //                        return;
                //                    }
                //                }
                //            }


                //            //SystemCodeVo resultVo = codeClient.S1144UpdateMst(masterDomain);
                //            //if (!resultVo.isSuccess)
                //            //{
                //            //    //실패
                //            //    WinUIMessageBox.Show(resultVo.Message, "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
                //            //    return;
                //            //}
                //            //else
                //            //{
                //            //    resultVo = codeClient.S1144UpdateMstHistory(masterDomain);
                //            //}
                //            //masterDomain.IS_CHECK = true;
                //            //this.btn_save.IsEnabled = true;
                //        }
                //    }
                //}
                //else if (n2ndLocNm)
                //{
                //    if (e.IsValid)
                //    {
                //        if (string.IsNullOrEmpty(masterDomain.N2ND_LOC_NM))
                //        {
                //            masterDomain.N2ND_LOC_ID = "";
                //            masterDomain.N2ND_LOC_NM = "";
                //        }
                //        //
                //        if (!masterDomain.N2ND_LOC_NM.Equals((e.Value == null ? "" : e.Value.ToString())))
                //        {
                //            SystemCodeVo bankIoDao = this.lue_N2ND_LOC_ID.GetItemFromValue(e.Value) as SystemCodeVo;
                //            //
                //            masterDomain.N2ND_LOC_ID = bankIoDao.CLSS_CD;
                //            masterDomain.N2ND_LOC_NM = bankIoDao.CLSS_DESC;
                //            //
                //            masterDomain.LOC_NM = masterDomain.N1ST_LOC_NM + masterDomain.N2ND_LOC_NM + "-" + masterDomain.N3RD_LOC_NM;
                //            masterDomain.ITM_CD = this.orgDao.ITM_CD;
                //            masterDomain.AREA_CD = SystemProperties.USER_VO.EMPE_PLC_NM;/*this.orgDao.AREA_CD;*/
                //            masterDomain.CRE_USR_ID = SystemProperties.USER;
                //            masterDomain.UPD_USR_ID = SystemProperties.USER;
                //            masterDomain.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;

                //            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s1144/mst/u", new StringContent(JsonConvert.SerializeObject(masterDomain), System.Text.Encoding.UTF8, "application/json")))
                //            {
                //                if (response.IsSuccessStatusCode)
                //                {
                //                    int _Num = 0;
                //                    string resultMsg = await response.Content.ReadAsStringAsync();
                //                    if (int.TryParse(resultMsg, out _Num) == false)
                //                    {
                //                        //실패
                //                        WinUIMessageBox.Show(resultMsg, title, MessageBoxButton.OK, MessageBoxImage.Error);
                //                        return;
                //                    }
                //                }
                //            }
                //            //SystemCodeVo resultVo = codeClient.S1144UpdateMst(masterDomain);
                //            //if (!resultVo.isSuccess)
                //            //{
                //            //    //실패
                //            //    WinUIMessageBox.Show(resultVo.Message, "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
                //            //    return;
                //            //}
                //            //else
                //            //{
                //            //    resultVo = codeClient.S1144UpdateMstHistory(masterDomain);
                //            //}
                //            //masterDomain.IS_CHECK = true;
                //            //this.btn_save.IsEnabled = true;
                //        }
                //    }
                //}
                //else if (n3rdLocNm)
                //{
                //    if (e.IsValid)
                //    {
                //        if (string.IsNullOrEmpty(masterDomain.N3RD_LOC_NM))
                //        {
                //            masterDomain.N3RD_LOC_ID = "";
                //            masterDomain.N3RD_LOC_NM = "";
                //        }
                //        //
                //        if (!masterDomain.N3RD_LOC_NM.Equals((e.Value == null ? "" : e.Value.ToString())))
                //        {
                //            SystemCodeVo bankIoDao = this.lue_N3RD_LOC_ID.GetItemFromValue(e.Value) as SystemCodeVo;
                //            //
                //            masterDomain.N3RD_LOC_ID = bankIoDao.CLSS_CD;
                //            masterDomain.N3RD_LOC_NM = bankIoDao.CLSS_DESC;
                //            //
                //            masterDomain.LOC_NM = masterDomain.N1ST_LOC_NM + masterDomain.N2ND_LOC_NM + "-" + masterDomain.N3RD_LOC_NM;
                //            masterDomain.ITM_CD = this.orgDao.ITM_CD;
                //            masterDomain.AREA_CD = SystemProperties.USER_VO.EMPE_PLC_NM;/*this.orgDao.AREA_CD;*/
                //            masterDomain.CRE_USR_ID = SystemProperties.USER;
                //            masterDomain.UPD_USR_ID = SystemProperties.USER;
                //            masterDomain.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;

                //            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s1144/mst/u", new StringContent(JsonConvert.SerializeObject(masterDomain), System.Text.Encoding.UTF8, "application/json")))
                //            {
                //                if (response.IsSuccessStatusCode)
                //                {
                //                    int _Num = 0;
                //                    string resultMsg = await response.Content.ReadAsStringAsync();
                //                    if (int.TryParse(resultMsg, out _Num) == false)
                //                    {
                //                        //실패
                //                        WinUIMessageBox.Show(resultMsg, title, MessageBoxButton.OK, MessageBoxImage.Error);
                //                        return;
                //                    }
                //                }
                //            }
                //            //SystemCodeVo resultVo = codeClient.S1144UpdateMst(masterDomain);
                //            //if (!resultVo.isSuccess)
                //            //{
                //            //    //실패
                //            //    WinUIMessageBox.Show(resultVo.Message, "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
                //            //    return;
                //            //}
                //            //else
                //            //{
                //            //    resultVo = codeClient.S1144UpdateMstHistory(masterDomain);
                //            //}
                //            //masterDomain.IS_CHECK = true;
                //            //this.btn_save.IsEnabled = true;
                //        }
                //    }
                //}
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

            //bool n1stLocNm = (e.Column.FieldName.ToString().Equals("N1ST_LOC_NM") ? true : false);
            //bool n2ndLocNm = (e.Column.FieldName.ToString().Equals("N2ND_LOC_NM") ? true : false);
            //bool n3rdLocNm = (e.Column.FieldName.ToString().Equals("N3RD_LOC_NM") ? true : false);

            //int rowHandle = this.configViewPage1EditView_Master.FocusedRowHandle;

            //if (n1stLocNm)
            //{
            //    this.ConfigViewPage1Edit_Master.CurrentColumn = this.ConfigViewPage1Edit_Master.Columns["N2ND_LOC_NM"];
            //}
            //else if (n2ndLocNm)
            //{
            //    this.ConfigViewPage1Edit_Master.CurrentColumn = this.ConfigViewPage1Edit_Master.Columns["N3RD_LOC_NM"];
            //}
            //else if (n3rdLocNm)
            //{
            //    this.ConfigViewPage1Edit_Master.CurrentColumn = this.ConfigViewPage1Edit_Master.Columns["N1ST_LOC_NM"];
            //    //rowHandle = this.configViewPage1EditView_Master.FocusedRowHandle + 1;
            //}

            //this.ConfigViewPage1Edit_Master.RefreshRow(rowHandle - 1);
            //this.configViewPage1EditView_Master.FocusedRowHandle = rowHandle;
        }

        public async void SYSTEM_CODE_VO()
        {
            //this.lue_N1ST_LOC_ID.ItemsSource = SystemProperties.SYSTEM_CODE_VO("C-006");
            //this.combo_N1ST_LOC_NM.ItemsSource = SystemProperties.SYSTEM_CODE_VO("C-006");
            //using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "C-006"))
            //{
            //    if (response.IsSuccessStatusCode)
            //    {
            //        this.lue_N1ST_LOC_ID.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
            //        this.combo_N1ST_LOC_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
            //    }
            //}

            //////this.lue_N2ND_LOC_ID.ItemsSource = SystemProperties.SYSTEM_CODE_VO("C-007");
            //////this.combo_N2ND_LOC_NM.ItemsSource = SystemProperties.SYSTEM_CODE_VO("C-007");
            //using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "C-007"))
            //{
            //    if (response.IsSuccessStatusCode)
            //    {
            //        this.lue_N2ND_LOC_ID.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
            //        this.combo_N2ND_LOC_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
            //    }
            //}

            //////this.lue_N3RD_LOC_ID.ItemsSource = SystemProperties.SYSTEM_CODE_VO("C-008");
            //////this.combo_N3RD_LOC_NM.ItemsSource = SystemProperties.SYSTEM_CODE_VO("C-008");
            //using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "C-008"))
            //{
            //    if (response.IsSuccessStatusCode)
            //    {
            //        this.lue_N3RD_LOC_ID.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
            //        this.combo_N3RD_LOC_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
            //    }
            //}

        }
    }
}
