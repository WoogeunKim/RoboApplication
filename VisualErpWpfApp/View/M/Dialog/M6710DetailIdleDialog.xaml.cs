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

namespace AquilaErpWpfApp3.View.M.Dialog
{
    /// <summary>
    /// Interaction logic for ConfigView1Dialog.xaml
    /// </summary>
    public partial class M6710DetailIdleDialog : DXWindow
    {
        //private static CodeServiceClient codeClient = SystemProperties.CodeClient;
        //private Dictionary<string, string> coTpCdMap = new Dictionary<string, string>();
        private IList<ManVo> totalItem;
        private ManVo orgDao;

        private string title = "비가동 현황";

        public M6710DetailIdleDialog(ManVo Dao)
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

            MessageBoxResult result = WinUIMessageBox.Show("[" + delVo.IDLE_NM + " / " + delVo.IDLE_MNT + "]" + " 정말로 삭제 하시겠습니까?", "[삭제]" + title, MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                this.totalItem.Remove(delVo);

                this.btn_save.IsEnabled = true;
            }
            this.ConfigViewPage1Edit_Master.RefreshData();
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
            int nCnt = this.totalItem.Count;

            //삭제
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6710/dtl1/d", new StringContent(JsonConvert.SerializeObject(this.orgDao), System.Text.Encoding.UTF8, "application/json")))
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
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6710/dtl1/m", new StringContent(JsonConvert.SerializeObject(this.totalItem), System.Text.Encoding.UTF8, "application/json")))
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
            //비가동 현황
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6710/dtl1", new StringContent(JsonConvert.SerializeObject(this.orgDao), System.Text.Encoding.UTF8, "application/json")))
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

                bool idleNm = (e.Column.FieldName.ToString().Equals("IDLE_NM") ? true : false);
                bool idleMnt = (e.Column.FieldName.ToString().Equals("IDLE_MNT") ? true : false);

                if (idleNm)
                {
                    if (e.IsValid)
                    {
                        if (string.IsNullOrEmpty(masterDomain.IDLE_NM))
                        {
                            masterDomain.IDLE_TM_CD = "";
                            masterDomain.IDLE_NM = "";
                        }
                        //
                        if (!masterDomain.IDLE_NM.Equals((e.Value == null ? "" : e.Value.ToString())))
                        {
                            SystemCodeVo bankIoDao = this.lue_IDLE_NM.GetItemFromValue(e.Value) as SystemCodeVo;
                            //
                            masterDomain.IDLE_TM_CD = bankIoDao.CLSS_CD;
                            masterDomain.IDLE_NM = bankIoDao.CLSS_DESC;

                            //
                            masterDomain.ROUT_CD = this.orgDao.ROUT_CD;
                            masterDomain.CRE_USR_ID = SystemProperties.USER;
                            masterDomain.UPD_USR_ID = SystemProperties.USER;
                            masterDomain.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;

                            this.btn_save.IsEnabled = true;
                        }
                    }
                }
                else if (idleMnt)
                {
                    if (e.IsValid)
                    {
                        if (string.IsNullOrEmpty(masterDomain.IDLE_MNT?.ToString()))
                        {
                            masterDomain.IDLE_MNT = "0";
                        }
                        //
                        if (!masterDomain.IDLE_MNT.Equals((e.Value == null ? "" : e.Value.ToString())))
                        {
                            masterDomain.IDLE_MNT = e.Value;

                            //
                            masterDomain.ROUT_CD = this.orgDao.ROUT_CD;
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
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "Q-007"))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.lue_IDLE_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                }
            }

        }
    }
}
