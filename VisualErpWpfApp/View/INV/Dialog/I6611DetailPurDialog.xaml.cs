using AquilaErpWpfApp3.Util;
using DevExpress.Data;
using DevExpress.Mvvm.Native;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Code;
using ModelsLibrary.Inv;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;

namespace AquilaErpWpfApp3.View.INV.Dialog
{
    public partial class I6611DetailPurDialog : DXWindow
    {
        private InvVo orgVo;
        private string _title = "품목 입고 관리";

        public I6611DetailPurDialog(InvVo vo)
        {
            InitializeComponent();

            this.orgVo = vo;

            this.txt_stDate.Text = System.DateTime.Now.AddYears(-2).ToString("yyyy-MM-dd");
            this.txt_enDate.Text = System.DateTime.Now.ToString("yyyy-MM-dd");

            SYSTEM_CODE_VO();

            searchItem();


            this.btn_reset.Click += btn_reset_Click;

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
                InvVo vo = new InvVo() { FM_DT = Convert.ToDateTime(this.txt_stDate.Text).ToString("yyyy-MM-dd"), TO_DT = Convert.ToDateTime(this.txt_enDate.Text).ToString("yyyy-MM-dd") };

                SystemCodeVo grpCdVo = this.combo_GRP_NM.SelectedItem as SystemCodeVo;
                if (grpCdVo != null)
                {
                    vo.CO_NO = (string.IsNullOrEmpty(grpCdVo.CO_NO) ? null : grpCdVo.CO_NO);
                    vo.CO_CD = (string.IsNullOrEmpty(grpCdVo.CO_NO) ? null : grpCdVo.CO_NO);
                    vo.CO_NM = (string.IsNullOrEmpty(grpCdVo.CO_NM) ? null : grpCdVo.CO_NM);
                }

                //vo.AREA_CD = SystemProperties.USER_VO.EMPE_PLC_NM;
                vo.AREA_CD = "002";
                vo.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;

                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("i6611/pur", new StringContent(JsonConvert.SerializeObject(vo), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.ViewJOB_ITEMEdit.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<InvVo>>(await response.Content.ReadAsStringAsync()).Cast<InvVo>().ToList();
                    }
                }
            }
            catch (Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[에러]" + _title, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        #region Functon (OKButton_Click, CancelButton_Click)
        private async void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var checkList = ((List<InvVo>)this.ViewJOB_ITEMEdit.ItemsSource).Where(x=>x.isCheckd).ToList<InvVo>();
                if (checkList.Count <= 0)
                {
                    WinUIMessageBox.Show("데이터가 존재 하지 않습니다.", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // 유효검사
                if (ValueChanged(checkList))
                {

                    MessageBoxResult result = WinUIMessageBox.Show("정말로 저장 하시겠습니까?", "[저장]" + _title, MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        if (DXSplashScreen.IsActive == false) DXSplashScreen.Show<ProgressWindow>();

                        for (int i = 0; i < checkList.Count; i++)
                        {
                            checkList[i].AREA_CD = "002";
                            checkList[i].INAUD_CD = "RGU";
                            checkList[i].CRE_USR_ID = SystemProperties.USER_VO.CHNL_CD;
                        }


                        int _Num = 0;
                        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("i6611/mst/i", new StringContent(JsonConvert.SerializeObject(checkList), System.Text.Encoding.UTF8, "application/json")))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                string resultMsg = await response.Content.ReadAsStringAsync();
                                if (int.TryParse(resultMsg, out _Num) == false)
                                {
                                    if (DXSplashScreen.IsActive == true) DXSplashScreen.Close();
                                    //실패
                                    WinUIMessageBox.Show(resultMsg, _title, MessageBoxButton.OK, MessageBoxImage.Error);
                                    return;
                                }
                            }
                            if (DXSplashScreen.IsActive == true) DXSplashScreen.Close();

                            WinUIMessageBox.Show("저장 완료 되었습니다", "[저장]" + _title, MessageBoxButton.OK, MessageBoxImage.Information);
                        }

                        if (DXSplashScreen.IsActive == true) DXSplashScreen.Close();

                        this.OKButton.IsEnabled = false;
                    }

                    this.DialogResult = true;
                    this.Close();
                }
            }
            catch(Exception eLog)
            {
                if (DXSplashScreen.IsActive == true) DXSplashScreen.Close();
                WinUIMessageBox.Show(eLog.Message, _title, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private bool ValueChanged(List<InvVo> voList)
        {
            var ret = true;

            try
            {
                foreach(InvVo vo in voList)
                {
                    int _Num = 0;

                    if (int.TryParse(vo.ITM_QTY.ToString(), out _Num) == false)
                    {
                        WinUIMessageBox.Show("입고수량을 다시 입력하세요.", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                        ret = false;
                        break;
                    }
                    else if (Convert.ToDouble(vo.ITM_QTY.ToString()) > Convert.ToDouble(vo.PUR_QTY_RMN.ToString()))
                    {
                        WinUIMessageBox.Show("입고수량이 잔량보다 큰 값을 입력할 수 없습니다..", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                        ret = false;
                        break;
                    }
                    else if (string.IsNullOrEmpty(vo.LOC_CD))
                    {
                        WinUIMessageBox.Show("창고를 다시 입력하세요.", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                        ret = false;
                        break;
                    }
                }
            }
            catch (Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, _title, MessageBoxButton.OK, MessageBoxImage.Error);
            }


            return ret;
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
                InvVo masterDomain = (InvVo)ViewJOB_ITEMEdit.GetFocusedRow();
                bool itmQty = (e.Column.FieldName.ToString().Equals("ITM_QTY") ? true : false);
                bool locNm = (e.Column.FieldName.ToString().Equals("LOC_NM") ? true : false);

                if (itmQty)
                {
                    if (e.IsValid)
                    {
                        if (string.IsNullOrEmpty(masterDomain.ITM_QTY + ""))
                        {
                            masterDomain.ITM_QTY = 0;
                        }
                        //
                        if (!masterDomain.ITM_QTY.Equals((e.Value == null ? "" : e.Value.ToString())))
                        {
                            if (Convert.ToDecimal(e.Value.ToString()) > Convert.ToDecimal(masterDomain.PUR_QTY_RMN))
                            {
                                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                                e.ErrorContent = "[번들잔량] " + masterDomain.PUR_QTY_RMN + " 보다 큰 값은 입력 하실수 없습니다";
                                e.SetError(e.ErrorContent, e.ErrorType);
                                return;
                            }

                            masterDomain.ITM_QTY = e.Value.ToString();

                            masterDomain.IN_QTY = Convert.ToDecimal(masterDomain.ITM_QTY) * Convert.ToDecimal(masterDomain.BAR_QTY);
                            masterDomain.ITM_WGT = Convert.ToDecimal(masterDomain.ITM_QTY) * Convert.ToDecimal(masterDomain.BAR_WGT);

                            masterDomain.isCheckd = true;
                            this.OKButton.IsEnabled = true;
                        }
                    }
                }
                else if (locNm)
                {
                    if (e.IsValid)
                    {
                        if (string.IsNullOrEmpty(masterDomain.LOC_CD))
                        {
                            masterDomain.LOC_CD = "";
                            masterDomain.LOC_NM = "";
                        }
                        //
                        if (!masterDomain.LOC_CD.Equals((e.Value == null ? "" : e.Value.ToString())))
                        {
                            SystemCodeVo bankIoDao = this.lue_LOC_NM.GetItemFromValue(e.Value) as SystemCodeVo;
                            //
                            if (bankIoDao != null)
                            {
                                masterDomain.LOC_CD = bankIoDao.CLSS_CD;
                                masterDomain.LOC_NM = bankIoDao.CLSS_DESC;
                            }

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

            bool itmQty = (e.Column.FieldName.ToString().Equals("ITM_QTY") ? true : false);
            bool inaudOrgNm = (e.Column.FieldName.ToString().Equals("INAUD_ORG_NM") ? true : false);
            bool inaudRmk = (e.Column.FieldName.ToString().Equals("PUR_RMK") ? true : false);

            int rowHandle = this.viewJOB_ITEMView.FocusedRowHandle + 1;

            if (itmQty)
            {
                this.ViewJOB_ITEMEdit.CurrentColumn = this.ViewJOB_ITEMEdit.Columns["ITM_QTY"];
            }
            else if (inaudOrgNm)
            {
                this.ViewJOB_ITEMEdit.CurrentColumn = this.ViewJOB_ITEMEdit.Columns["INAUD_ORG_NM"];
            }

            this.ViewJOB_ITEMEdit.RefreshRow(rowHandle - 1);
            this.viewJOB_ITEMView.FocusedRowHandle = rowHandle;
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

        private void PART_Editor_Checked(object sender, RoutedEventArgs e)
        {
            this.OKButton.IsEnabled = true;
        }

        private decimal plnAmtTotal = 0;
        private void grid_CustomSummary(object sender, CustomSummaryEventArgs e)
        {
            if (((GridSummaryItem)e.Item).FieldName.Equals("ITM_QTY"))
            {
                if (e.IsTotalSummary)
                {
                    if (e.SummaryProcess == CustomSummaryProcess.Start)
                    {
                        plnAmtTotal = 0;
                        InvVo tmpImsi;
                        for (int x = 0; x < this.ViewJOB_ITEMEdit.VisibleRowCount; x++)
                        {
                            int rowHandle = this.ViewJOB_ITEMEdit.GetRowHandleByVisibleIndex(x);
                            if (rowHandle > -1)
                            {
                                tmpImsi = this.ViewJOB_ITEMEdit.GetRow(rowHandle) as InvVo;
                                if (tmpImsi.isCheckd == true)
                                {
                                    plnAmtTotal = plnAmtTotal + Convert.ToDecimal(tmpImsi.ITM_QTY);
                                }
                            }
                        }
                        if (plnAmtTotal > 0)
                        {
                            e.TotalValue = plnAmtTotal;
                        }
                        else
                        {
                            e.TotalValue = 0;
                        }
                    }
                    if (e.SummaryProcess == CustomSummaryProcess.Calculate)
                    {
                    }
                }
            }
        }



        public async void SYSTEM_CODE_VO()
        {
            //this.lue_INAUD_ORG_NM.ItemsSource = SystemProperties.SYSTEM_CODE_VO("P-008");
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "P-008"))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.lue_LOC_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                }
            }

            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s143", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { DELT_FLG = "N", CO_TP_CD = "AR", AREA_CD = SystemProperties.USER_VO.EMPE_PLC_NM, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    IList<SystemCodeVo> tmpList = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                    tmpList.Insert(0, new SystemCodeVo() { CO_NO = null, CO_NM = "전체" });
                    //
                    this.combo_GRP_NM.ItemsSource = tmpList;
                }
            }
        }


    }
}
