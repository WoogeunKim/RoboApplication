using AquilaErpWpfApp3.Util;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Inv;
using ModelsLibrary.Man;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;

namespace AquilaErpWpfApp3.View.M.Dialog
{
    public partial class M6710MasterDialog : DXWindow
    {
        //가입고 에서 정입고
        //private static InvServiceClient invClient = SystemProperties.InvClient;
        private ManVo orgVo;
        private string _title = "작업 지시서";

        public M6710MasterDialog(ManVo vo)
        {
            InitializeComponent();

            this.orgVo = vo;
            //.AddYears(-1)
            this.txt_stDate.Text = System.DateTime.Now.ToString("yyyy-MM-dd");
            this.txt_enDate.Text = System.DateTime.Now.ToString("yyyy-MM-dd");

            SYSTEM_CODE_VO();
            
            searchItem();

            this.btn_reset.Click += btn_reset_Click;
            //this.btn_apply.Click += btn_apply_Click;

            //this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
            this.OKButton.Click += new RoutedEventHandler(OKButton_Click);
            this.CancelButton.Click += new RoutedEventHandler(CancelButton_Click);


            this.OKButton.IsEnabled = false;
        }

       


        void btn_reset_Click(object sender, RoutedEventArgs e)
        {
            searchItem();

        }

        private async void searchItem()
        {
            try
            {
                ManVo vo = new ManVo() { UPD_USR_ID = SystemProperties.USER, CRE_USR_ID = SystemProperties.USER, CHNL_CD = SystemProperties.USER_VO.CHNL_CD, FM_DT = Convert.ToDateTime(this.txt_stDate.Text).ToString("yyyy-MM-dd"), TO_DT = Convert.ToDateTime(this.txt_enDate.Text).ToString("yyyy-MM-dd") };

                this.search_title.Text = "[조회 조건]   " + "기간 : " + Convert.ToDateTime(this.txt_stDate.Text).ToString("yyyy-MM-dd") + "~" + Convert.ToDateTime(this.txt_enDate.Text).ToString("yyyy-MM-dd");
              
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6710/popup", new StringContent(JsonConvert.SerializeObject(vo), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.ViewJOB_ITEMEdit.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
                    }
                }
                    //this.ViewJOB_ITEMEdit.ItemsSource = invClient.I5511SelectDtlInList(vo);
            }
            catch (Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[에러]" + _title, MessageBoxButton.OK, MessageBoxImage.Error);
                this.MSG.Text = eLog.Message;
                return;
            }
        }

        #region Functon (OKButton_Click, CancelButton_Click)
        private async void OKButton_Click(object sender, RoutedEventArgs e)
        {
            //bool isMsg = false;
            //IList<InvVo> checkList = (IList<InvVo>)this.ViewJOB_ITEMEdit.ItemsSource;
            //List<InvVo> saveList = new List<InvVo>();
            //int nSize = checkList.Count;
            //if (nSize <= 0)
            //{
            //    WinUIMessageBox.Show("데이터가 존재 하지 않습니다.", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.MSG.Text = "데이터가 존재 하지 않습니다.";
            //    return;
            //}
            if(((IList<ManVo>)this.ViewJOB_ITEMEdit.ItemsSource).Any<ManVo>(x => x.isCheckd == true) == false)
            {
                WinUIMessageBox.Show("데이터가 존재 하지 않습니다.", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.MSG.Text = "데이터가 존재 하지 않습니다.";
                return;
            }

            List<ManVo> saveList = ((IList<ManVo>)this.ViewJOB_ITEMEdit.ItemsSource).Where<ManVo>(x => x.isCheckd == true).ToList<ManVo>();
            MessageBoxResult result = WinUIMessageBox.Show("정말로 저장 하시겠습니까?", "[저장]" + _title, MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                int _Num = 0;
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6710/mst/m", new StringContent(JsonConvert.SerializeObject(saveList), System.Text.Encoding.UTF8, "application/json")))
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
                    }

                    WinUIMessageBox.Show("저장 완료 되었습니다", _title, MessageBoxButton.OK, MessageBoxImage.Information);
                    this.DialogResult = true;
                    this.Close();
                }
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


        private void GridColumn_Validate(object sender, DevExpress.Xpf.Grid.GridCellValidationEventArgs e)
        {
            try
            {
                ManVo masterDomain = (ManVo)ViewJOB_ITEMEdit.GetFocusedRow();

                bool slItmQty = (e.Column.FieldName.ToString().Equals("SL_ITM_QTY") ? true : false);
                bool inpStaffVal = (e.Column.FieldName.ToString().Equals("INP_STAFF_VAL") ? true : false);
                bool wrkStDt = (e.Column.FieldName.ToString().Equals("WRK_ST_DT") ? true : false);
                bool wrkEndDt = (e.Column.FieldName.ToString().Equals("WRK_END_DT") ? true : false);
                bool moldCd = (e.Column.FieldName.ToString().Equals("MOLD_CD") ? true : false);
                bool dyNgtFlg= (e.Column.FieldName.ToString().Equals("DY_NGT_FLG") ? true : false);
     
                if (slItmQty)
                {
                    if (e.IsValid)
                    {
                        if (string.IsNullOrEmpty(masterDomain.SL_ITM_QTY + ""))
                        {
                            masterDomain.SL_ITM_QTY = 0;
                        }
                        //
                        if (!masterDomain.SL_ITM_QTY.Equals((e.Value == null ? "" : e.Value.ToString())))
                        {
                            masterDomain.SL_ITM_QTY = e.Value.ToString();

                            masterDomain.isCheckd = true;
                            this.OKButton.IsEnabled = true;
                        }
                    }
                }
                else if (inpStaffVal)
                {
                    if (e.IsValid)
                    {
                        if (masterDomain.INP_STAFF_VAL == null)
                        {
                            masterDomain.INP_STAFF_VAL = "";
                        }
                        //
                        if (!masterDomain.INP_STAFF_VAL.Equals((e.Value == null ? "" : e.Value.ToString())))
                        {
                            masterDomain.INP_STAFF_VAL = e.Value.ToString();
                            masterDomain.isCheckd = true;
                            this.OKButton.IsEnabled = true;
                        }
                    }
                }
                else if (wrkStDt)
                {
                    if (e.IsValid)
                    {
                        if (masterDomain.WRK_ST_DT == null)
                        {
                            masterDomain.WRK_ST_DT = "00:00";
                        }
                        //
                        if (!masterDomain.WRK_ST_DT.Equals((e.Value == null ? "" : e.Value.ToString())))
                        {
                            DateTime date;
                            if (DateTime.TryParseExact(masterDomain.PROD_PLN_DT + " " + e.Value.ToString() + ":00", "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out date) == false)
                            {
                                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                                e.ErrorContent = "[" + e.Value.ToString() + "] (HH:MM) 입력 값이 맞지 않습니다";
                                e.SetError(e.ErrorContent, e.ErrorType);
                                return;
                            }

                            //masterDomain.WRK_ST_DT = Convert.ToDateTime(e.Value.ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                            masterDomain.WRK_ST_DT = e.Value.ToString();
                            masterDomain.MAKE_ST_DT = Convert.ToDateTime(masterDomain.PROD_PLN_DT + " " + masterDomain.WRK_ST_DT + ":00").ToString("yyyy-MM-dd HH:mm:ss");
                            //
                            if (masterDomain.WRK_ST_DT == null)
                            {
                                masterDomain.WRK_ST_DT = "00:00";
                            }
                            else if (masterDomain.WRK_END_DT == null)
                            {
                                masterDomain.WRK_END_DT = "00:00";
                            }
                            else
                            {
                                DateTime StartDate = Convert.ToDateTime(masterDomain.PROD_PLN_DT + " " + masterDomain.WRK_ST_DT + ":00");
                                DateTime EndDate = Convert.ToDateTime(masterDomain.PROD_PLN_DT + " " + masterDomain.WRK_END_DT + ":00");
                                TimeSpan dateDiff = EndDate - StartDate;
                                masterDomain.WRK_HRS = dateDiff.Hours + "." + dateDiff.Minutes;
                            }

                            masterDomain.isCheckd = true;
                            this.OKButton.IsEnabled = true;
                        }
                    }
                }
                else if (wrkEndDt)
                {
                    if (e.IsValid)
                    {
                        if (masterDomain.WRK_END_DT == null)
                        {
                            masterDomain.WRK_END_DT = "00:00";
                        }
                        //
                        if (!masterDomain.WRK_END_DT.Equals((e.Value == null ? "" : e.Value.ToString())))
                        {
                            DateTime date;
                            if(DateTime.TryParseExact(masterDomain.PROD_PLN_DT + " " + e.Value.ToString() + ":00", "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out date) == false)
                            {
                                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                                e.ErrorContent = "[" + e.Value.ToString() + "] (HH:MM) 입력 값이 맞지 않습니다";
                                e.SetError(e.ErrorContent, e.ErrorType);
                                return;
                            }

                            //masterDomain.WRK_END_DT = Convert.ToDateTime(e.Value.ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                            masterDomain.WRK_END_DT = e.Value.ToString();
                            masterDomain.MAKE_END_DT = Convert.ToDateTime(masterDomain.PROD_PLN_DT + " " + masterDomain.WRK_END_DT + ":00").ToString("yyyy-MM-dd HH:mm:ss");
                            //
                            if (masterDomain.WRK_ST_DT == null)
                            {
                                masterDomain.WRK_ST_DT = "00:00";
                            }
                            else if (masterDomain.WRK_END_DT == null)
                            {
                                masterDomain.WRK_END_DT = "00:00";
                            }
                            else
                            {
                                DateTime StartDate = Convert.ToDateTime(masterDomain.PROD_PLN_DT + " " + masterDomain.WRK_ST_DT + ":00");
                                DateTime EndDate = Convert.ToDateTime(masterDomain.PROD_PLN_DT + " " + masterDomain.WRK_END_DT + ":00");
                                TimeSpan dateDiff = EndDate - StartDate;
                                masterDomain.WRK_HRS = dateDiff.Hours + "." + dateDiff.Minutes;
                            }

                            masterDomain.isCheckd = true;
                            this.OKButton.IsEnabled = true;
                        }
                    }
                }
                else if (moldCd)
                {
                    if (e.IsValid)
                    {
                        if (string.IsNullOrEmpty(masterDomain.MOLD_CD + ""))
                        {
                            masterDomain.MOLD_CD = "";
                        }
                        //
                        if (!masterDomain.MOLD_CD.Equals((e.Value == null ? "" : e.Value.ToString())))
                        {
                            ManVo bankIoDao = this.lue_MOLD_NO.GetItemFromValue(e.Value) as ManVo;
                            //
                            if (bankIoDao != null)
                            {
                                masterDomain.MOLD_CD = bankIoDao.MOLD_NO;
                            }

                            masterDomain.isCheckd = true;
                            this.OKButton.IsEnabled = true;
                        }
                    }
                }
                else if (dyNgtFlg)
                {
                    if (e.IsValid)
                    {
                        if (masterDomain.DY_NGT_FLG == null)
                        {
                            masterDomain.DY_NGT_FLG = "";
                        }
                        //
                        if (!masterDomain.DY_NGT_FLG.Equals((e.Value == null ? "" : e.Value.ToString())))
                        {
                            masterDomain.DY_NGT_FLG = e.Value.ToString();
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
            this.viewJOB_ITEMView.CommitEditing();

            //bool inaudQty = (e.Column.FieldName.ToString().Equals("IN_QTY") ? true : false);
            //bool inaudOrgNm = (e.Column.FieldName.ToString().Equals("INAUD_ORG_NM") ? true : false);
            //bool inaudRmk = (e.Column.FieldName.ToString().Equals("ITM_RMK") ? true : false);

            //int rowHandle = this.viewJOB_ITEMView.FocusedRowHandle + 1;

            //if (inaudQty)
            //{
            //    this.ViewJOB_ITEMEdit.CurrentColumn = this.ViewJOB_ITEMEdit.Columns["IN_QTY"];
            //}
            //else if (inaudOrgNm)
            //{
            //    this.ViewJOB_ITEMEdit.CurrentColumn = this.ViewJOB_ITEMEdit.Columns["INAUD_ORG_NM"];
            //}
            //else if (inaudRmk)
            //{
            //    this.ViewJOB_ITEMEdit.CurrentColumn = this.ViewJOB_ITEMEdit.Columns["ITM_RMK"];
            //}

            //this.ViewJOB_ITEMEdit.RefreshRow(rowHandle - 1);
            //this.viewJOB_ITEMView.FocusedRowHandle = rowHandle;
        }


        private void CheckEdit_Checked(object sender, RoutedEventArgs e)
        {
            CheckEdit checkEdit = sender as CheckEdit;
            InvVo tmpImsi;
            for (int x = 0; x < this.ViewJOB_ITEMEdit.VisibleRowCount; x++)
            {
                int rowHandle = this.ViewJOB_ITEMEdit.GetRowHandleByVisibleIndex(x);
                if (rowHandle > -1)
                {
                    tmpImsi = this.ViewJOB_ITEMEdit.GetRow(rowHandle) as InvVo;
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

        //private void PART_Editor_Checked(object sender, RoutedEventArgs e)
        //{
        //    this.OKButton.IsEnabled = true;
        //}

        //private void ButtonInfo_Click(object sender, RoutedEventArgs e)
        //{
        //    InvVo selVo = (InvVo)ViewJOB_ITEMEdit.GetFocusedRow();
        //    if (selVo != null)
        //    {
        //        selVo.isCheckd = true;

        //        if (Convert.ToInt32(selVo.IN_QTY) < 0)
        //        {
        //            selVo.IN_QTY = 0;
        //        }
        //        else
        //        {
        //            selVo.IN_QTY = selVo.RMN_QTY;
        //            //selVo.CTRT_RMN_QTY = 0;
        //        }
        //        //selVo.IMP_ITM_AMT = Convert.ToInt32(selVo.IMP_INV_QTY) * Convert.ToDouble(selVo.IMP_ITM_PRC) / 1000;

        //        selVo.RMN_QTY = Convert.ToDecimal(selVo.TMP_RMK_QTY) - Convert.ToDecimal(selVo.IN_QTY);
        //        this.OKButton.IsEnabled = true;

        //        this.viewJOB_ITEMView.CommitEditing();
        //        this.ViewJOB_ITEMEdit.RefreshData();
        //        this.viewJOB_ITEMView.FocusedRowHandle = this.viewJOB_ITEMView.FocusedRowHandle;
        //    }
        //}


        public async void SYSTEM_CODE_VO()
        {


            //금형
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6661", new StringContent(JsonConvert.SerializeObject(new ManVo() { DELT_FLG = "N", CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.lue_MOLD_NO.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
                }
            }

            //this.lue_INAUD_ORG_NM.ItemsSource = SystemProperties.SYSTEM_CODE_VO("L-100");
            //this.combo_INAUD_ORG_NM.ItemsSource = SystemProperties.SYSTEM_CODE_VO("L-100");
            //using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "L-100"))
            //{
            //    if (response.IsSuccessStatusCode)
            //    {
            //        this.lue_INAUD_ORG_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
            //        this.combo_INAUD_ORG_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
            //    }
            //}
        }

    }
}
