using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Code;
using ModelsLibrary.Pur;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;
using AquilaErpWpfApp3.Util;

namespace AquilaErpWpfApp3.View.PUR.Dialog
{
    public partial class P4411Detail_2Dialog : DXWindow
    {
        private string _title = "발주 등록 관리";

        public bool isEdit = false;
        private PurVo orgDao;


        private IList<PurVo> SelectItems = new List<PurVo>();

        public P4411Detail_2Dialog(PurVo Dao)
        {
            InitializeComponent();

            SYSTEM_CODE_VO();

            this.orgDao = Dao;

            this.btn_ITEMS.Click += btn_ITEMS_Click;
            this.OKButton.Click += new RoutedEventHandler(OKButton_Click);
            this.CancelButton.Click += new RoutedEventHandler(CancelButton_Click);
        }

        void ViewTableDtl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            PurVo tmpVo = (PurVo)this.ViewGridDtl.GetFocusedRow();
            if (tmpVo != null)
            {
                if (tmpVo.isCheckd)
                {
                    tmpVo.isCheckd = false;
                }
                else
                {
                    tmpVo.isCheckd = true;
                    this.OKButton.IsEnabled = true;
                }
            }
        }

        void DELButton_Click(object sender, RoutedEventArgs e)
        {
            PurVo tmpVo = (PurVo)this.ViewGridDtl.GetFocusedRow();
            if (tmpVo != null)
            {
                this.SelectItems.Remove(tmpVo);

                //this.ViewGridDtl.ItemsSource = this.SelectItems;
                this.ViewGridDtl.RefreshData();

            }
        }

        async void btn_ITEMS_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (DXSplashScreen.IsActive == false)  DXSplashScreen.Show<ProgressWindow>();

                PurVo dao = new PurVo();

                SystemCodeVo ITM_GRP_CLSS_CDVo = this.combo_ITM_GRP_CLSS_CD.SelectedItem as SystemCodeVo;
                if (ITM_GRP_CLSS_CDVo != null)
                {
                    dao.ITM_GRP_CLSS_CD = ITM_GRP_CLSS_CDVo.CLSS_CD;
                    dao.CHNL_CD = SystemProperties.USER_VO.CHNL_CD; 
                    dao.CO_NO = orgDao.CO_NO;
                    dao.PUR_ORD_NO = orgDao.PUR_ORD_NO;
                    dao.CRE_USR_ID = SystemProperties.USER;
                    dao.UPD_USR_ID = SystemProperties.USER;
                }


                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("p4411/dlg2", new StringContent(JsonConvert.SerializeObject(dao), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        IList<PurVo> resultItems = JsonConvert.DeserializeObject<IEnumerable<PurVo>>(await response.Content.ReadAsStringAsync()).Cast<PurVo>().ToList();

                        this.SelectItems.Clear();

                        this.ViewGridDtl.ItemsSource = resultItems;
                        this.ViewGridDtl.SelectedItems = new List<PurVo>();
                        this.ViewGridDtl.RefreshData();
                    }
                }


                if (DXSplashScreen.IsActive == true) DXSplashScreen.Close();
            }
            catch (Exception eLog)
            {
                if (DXSplashScreen.IsActive == true) DXSplashScreen.Close();

                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + this._title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<PurVo> saveItems = (List<PurVo>)this.ViewGridDtl.ItemsSource;
                if (saveItems.FindAll(x => x.isCheckd == true).Count > 0)
                {
                    int _Num = 0;
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("p4411/dtl/i", new StringContent(JsonConvert.SerializeObject((saveItems.FindAll(x => x.isCheckd == true)).ToArray()), System.Text.Encoding.UTF8, "application/json")))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string result = await response.Content.ReadAsStringAsync();
                            if (int.TryParse(result, out _Num) == false)
                            {
                                //실패
                                WinUIMessageBox.Show(result, _title, MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                            //성공
                            WinUIMessageBox.Show("완료 되었습니다", this._title, MessageBoxButton.OK, MessageBoxImage.Information);
                            this.DialogResult = true;
                            this.Close();
                        }
                    }
                }
            }
            catch (Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + this._title, MessageBoxButton.OK, MessageBoxImage.Error);
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

        private void GridColumn_Validate(object sender, DevExpress.Xpf.Grid.GridCellValidationEventArgs e)
        {
            try
            {
                PurVo masterDomain = (PurVo)ViewGridDtl.GetFocusedRow();

                bool impItmQty = (e.Column.FieldName.ToString().Equals("PUR_QTY") ? true : false);
                bool impItmPrc = (e.Column.FieldName.ToString().Equals("CO_UT_PRC") ? true : false);
                //bool impItmAmt = (e.Column.FieldName.ToString().Equals("IMP_ITM_AMT") ? true : false);
                bool impItmRmk = (e.Column.FieldName.ToString().Equals("PUR_ITM_RMK") ? true : false);

                if (impItmQty)
                {
                    if (e.IsValid)
                    {
                        if (string.IsNullOrEmpty(masterDomain.PUR_QTY + ""))
                        {
                            masterDomain.PUR_QTY = 0;
                        }
                        //
                        if (!masterDomain.PUR_QTY.Equals((e.Value == null ? "" : e.Value.ToString())))
                        {
                            int? tmpInt = Convert.ToInt32(e.Value.ToString());
                            //if (tmpInt <= -1)
                            //{
                            //    e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                            //    e.ErrorContent = "[수량]" + tmpInt + " -값은 입력 하실수 없습니다";
                            //    e.SetError(e.ErrorContent, e.ErrorType);
                            //    return;
                            //}

                            masterDomain.PUR_QTY = tmpInt;
                            masterDomain.isCheckd = true;

                            //에러 체크
                            try
                            {
                                masterDomain.TOT_USE_QTY = tmpInt * Convert.ToDouble(masterDomain.BUN_WGT);
                                masterDomain.PUR_AMT = tmpInt * Convert.ToDouble(masterDomain.CO_UT_PRC);
                            }
                            catch (Exception)
                            {
                                masterDomain.TOT_USE_QTY = 0;
                                masterDomain.PUR_AMT = 0;
                            }

                            this.OKButton.IsEnabled = true;
                        }
                    }
                }
                else if (impItmPrc)
                {
                    if (e.IsValid)
                    {
                        if (string.IsNullOrEmpty(masterDomain.CO_UT_PRC + ""))
                        {
                            masterDomain.CO_UT_PRC = 0;
                        }
                        //
                        if (!masterDomain.CO_UT_PRC.Equals((e.Value == null ? "" : e.Value.ToString())))
                        {
                            double? tmpInt = Convert.ToDouble(e.Value.ToString());
                            //if (tmpInt <= -1)
                            //{
                            //    e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                            //    e.ErrorContent = "[단가]" + tmpInt + " -값은 입력 하실수 없습니다";
                            //    e.SetError(e.ErrorContent, e.ErrorType);
                            //    return;
                            //}

                            masterDomain.CO_UT_PRC = tmpInt;
                            masterDomain.isCheckd = true;
                            //에러 체크
                            try
                            {
                                masterDomain.PUR_AMT = Convert.ToInt32(masterDomain.PUR_QTY) * tmpInt;
                            }
                            catch (Exception)
                            {
                                masterDomain.PUR_AMT = 0;
                            }
                            this.OKButton.IsEnabled = true;
                        }
                    }
                }
                else if (impItmRmk)
                {
                    if (e.IsValid)
                    {
                        if (string.IsNullOrEmpty(masterDomain.PUR_ITM_RMK))
                        {
                            masterDomain.PUR_ITM_RMK = "";
                        }

                        if (!masterDomain.PUR_ITM_RMK.Equals((e.Value == null ? "" : e.Value.ToString())))
                        {
                            masterDomain.PUR_ITM_RMK = e.Value.ToString();
                            masterDomain.isCheckd = true;
                            this.OKButton.IsEnabled = true;
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
            this.ViewTableDtl.CommitEditing();

            bool impItmQty = (e.Column.FieldName.ToString().Equals("PUR_QTY") ? true : false);
            bool impItmPrc = (e.Column.FieldName.ToString().Equals("CO_UT_PRC") ? true : false);
            //bool impItmAmt = (e.Column.FieldName.ToString().Equals("IMP_ITM_AMT") ? true : false);
            bool impItmRmk = (e.Column.FieldName.ToString().Equals("PUR_ITM_RMK") ? true : false);

            int rowHandle = this.ViewTableDtl.FocusedRowHandle + 1;

            //if (impItmQty)
            //{
            //    this.ViewGridDtl.CurrentColumn = this.ViewGridDtl.Columns["PUR_QTY"];
            //}
            //else if (impItmPrc)
            //{
            //    this.ViewGridDtl.CurrentColumn = this.ViewGridDtl.Columns["CO_UT_PRC"];
            //}
            //else if (impItmRmk)
            //{
            //    this.ViewGridDtl.CurrentColumn = this.ViewGridDtl.Columns["PUR_ITM_RMK"];
            //}

            this.ViewGridDtl.RefreshRow(rowHandle - 1);
            //this.ViewTableDtl.FocusedRowHandle = rowHandle;
        }


        private void CheckEdit_Checked(object sender, RoutedEventArgs e)
        {
            CheckEdit checkEdit = sender as CheckEdit;
            PurVo tmpImsi;
            for (int x = 0; x < this.ViewGridDtl.VisibleRowCount; x++)
            {
                int rowHandle = this.ViewGridDtl.GetRowHandleByVisibleIndex(x);
                if (rowHandle > -1)
                {
                    tmpImsi = this.ViewGridDtl.GetRow(rowHandle) as PurVo;
                    if (checkEdit.IsChecked == true)
                    {
                        tmpImsi.isCheckd = true;
                        //
                        this.OKButton.IsEnabled = true;
                    }
                    else
                    {
                        tmpImsi.isCheckd = false;
                    }

                }
            }
        }

        private void view_CellValueChanging(object sender, CellValueChangedEventArgs e)
        {
            if (e.Column.FieldName == "isCheckd")
            {
                TableView view = (TableView)sender;
                view.CloseEditor();
                view.FocusedRowHandle = GridControl.InvalidRowHandle;
                view.FocusedRowHandle = e.RowHandle;
            }
        }

        public async void SYSTEM_CODE_VO()
        {
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "L-001"))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.combo_ITM_GRP_CLSS_CD.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                    this.combo_ITM_GRP_CLSS_CD.SelectedIndex = 0;
                }
            }

            btn_ITEMS_Click(null, null);
        }

    }
}