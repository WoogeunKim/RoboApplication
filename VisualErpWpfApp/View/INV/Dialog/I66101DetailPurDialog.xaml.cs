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
    public partial class I66101DetailPurDialog : DXWindow
    {
        //private static InvServiceClient invClient = SystemProperties.InvClient;
        private InvVo orgVo;
        private string _title = "입고 관리";

        public I66101DetailPurDialog()
        {
            InitializeComponent();


            this.txt_stDate.Text = System.DateTime.Now.AddYears(-1).ToString("yyyy-MM-dd");
            this.txt_enDate.Text = System.DateTime.Now.ToString("yyyy-MM-dd");

            SYSTEM_CODE_VO();

            Refresh();


            this.btn_reset.Click += btn_reset_Click;
            this.OKButton.Click += new RoutedEventHandler(OKButton_Click);
            this.CancelButton.Click += new RoutedEventHandler(CancelButton_Click);
            this.OKButton.IsEnabled = false;
        }

        void btn_reset_Click(object sender, RoutedEventArgs e)
        {
            Refresh();
        }

        private async void Refresh()
        {
            try
            {
                // 발주내역 조회
                if (this.combo_GRP_NM.SelectedItem == null) return;

                if (DXSplashScreen.IsActive == false) DXSplashScreen.Show<ProgressWindow>();

                InvVo refVo = new InvVo();
                refVo.FM_DT = Convert.ToDateTime(this.txt_stDate.Text).ToString("yyyy-MM-dd");
                refVo.TO_DT = Convert.ToDateTime(this.txt_enDate.Text).ToString("yyyy-MM-dd");
                refVo.CO_NO = (this.combo_GRP_NM.SelectedItem as SystemCodeVo).CO_NO;
                refVo.AREA_CD = SystemProperties.USER_VO.EMPE_PLC_NM;
                refVo.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;

                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("i66101/pur", new StringContent(JsonConvert.SerializeObject(refVo), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.ViewJOB_ITEMEdit.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<InvVo>>(await response.Content.ReadAsStringAsync()).Cast<InvVo>().ToList();
                    }

                    if (DXSplashScreen.IsActive == true) DXSplashScreen.Close();
                }
            }
            catch (Exception eLog)
            {
                if (DXSplashScreen.IsActive == true) DXSplashScreen.Close();

                WinUIMessageBox.Show(eLog.Message, "[에러]" + _title, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }


        #region Functon (OKButton_Click, CancelButton_Click)
        private async void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.ViewJOB_ITEMEdit.SelectedItem == null)
                {
                    WinUIMessageBox.Show("데이터가 존재 하지 않습니다.", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                IList<InvVo> selectList  = (this.ViewJOB_ITEMEdit.ItemsSource as IList<InvVo>).Where(w => w.isCheckd == true).ToList<InvVo>();

                MessageBoxResult result = WinUIMessageBox.Show("구매입고 " + selectList.Count + "건 정말로 저장 하시겠습니까?", "[저장]" + _title, MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    // 저장값 설정
                    for (int x = 0; x < selectList.Count; x++)
                    {
                        selectList[x].ITM_RMK = selectList[x].PUR_RMK;        // 비고
                        selectList[x].LOC_CD = selectList[x].INAUD_ORG_NO;    // 보관창고
                        selectList[x].AREA_CD = SystemProperties.USER_VO.EMPE_PLC_NM;
                        selectList[x].CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
                        selectList[x].CRE_USR_ID = SystemProperties.USER;
                        selectList[x].UPD_USR_ID = SystemProperties.USER;
                    }

                    int _Num = 0;
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("i66101/i", new StringContent(JsonConvert.SerializeObject(selectList), System.Text.Encoding.UTF8, "application/json")))
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

                            this.DialogResult = true;
                            this.Close();
                            return;
                        }
                    }
                }
            }
            catch (Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[에러]" + _title, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
        #endregion


        private void GridColumn_Validate(object sender, DevExpress.Xpf.Grid.GridCellValidationEventArgs e)
        {
            try
            {
                if (!e.IsValid) return;

                InvVo masterDomain = (InvVo)ViewJOB_ITEMEdit.GetFocusedRow();

                bool n1stItmSzNm = (e.Column.FieldName.ToString().Equals("N1ST_ITM_SZ_NM") ? true : false);     // 가로
                bool n2ndItmSzNm = (e.Column.FieldName.ToString().Equals("N2ND_ITM_SZ_NM") ? true : false);     // 세로
                bool itmQty = (e.Column.FieldName.ToString().Equals("ITM_QTY") ? true : false);                 // 수량
                bool purRmk = (e.Column.FieldName.ToString().Equals("PUR_RMK") ? true : false);                 // 비고  

                if (n1stItmSzNm)
                {
                    if(e.Value != null)
                    {
                        masterDomain.N1ST_ITM_SZ_NM = e.Value.ToString();
                        masterDomain.isCheckd = true;
                    }
                }
                else if (n2ndItmSzNm)
                {
                    if (e.Value != null)
                    {
                        masterDomain.N2ND_ITM_SZ_NM = e.Value.ToString();
                        masterDomain.isCheckd = true;
                    }
                }
                else if (itmQty)
                {
                    if (e.Value != null)
                    {
                        if((double)e.Value > 0)
                        {
                            masterDomain.ITM_QTY = e.Value;
                            masterDomain.IN_PUR_ITM_QTY = ((double)e.Value * 500);
                            masterDomain.isCheckd = true;
                            this.OKButton.IsEnabled = true;
                        }
                        else
                        {
                            masterDomain.ITM_QTY = 0.0;
                            masterDomain.IN_PUR_ITM_QTY = 0.0;
                            masterDomain.isCheckd = false;
                        }
                    }
                }
                else if (purRmk)
                {
                    if (e.Value != null)
                    {
                        masterDomain.PUR_RMK = e.Value.ToString();
                        masterDomain.isCheckd = true;
                    }
                }

            }
            catch (Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[에러]" + _title, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        // 입력시 다음줄로 자동 이동
        //private void viewPage1EditView_HiddenEditor(object sender, DevExpress.Xpf.Grid.EditorEventArgs e)
        //{
        //    try
        //    {
        //        this.viewJOB_ITEMView.CommitEditing();

        //        bool n1stItmSzNm = (e.Column.FieldName.ToString().Equals("N1ST_ITM_SZ_NM") ? true : false);     // 가로
        //        bool n2ndItmSzNm = (e.Column.FieldName.ToString().Equals("N2ND_ITM_SZ_NM") ? true : false);     // 세로
        //        bool itmQty = (e.Column.FieldName.ToString().Equals("ITM_QTY") ? true : false);                 // 수량
        //        bool purRmk = (e.Column.FieldName.ToString().Equals("PUR_RMK") ? true : false);                 // 비고  

        //        int rowHandle = this.viewJOB_ITEMView.FocusedRowHandle + 1;

        //        if (n1stItmSzNm)
        //        {
        //            this.ViewJOB_ITEMEdit.CurrentColumn = this.ViewJOB_ITEMEdit.Columns["N1ST_ITM_SZ_NM"];
        //        }
        //        else if (n2ndItmSzNm)
        //        {
        //            this.ViewJOB_ITEMEdit.CurrentColumn = this.ViewJOB_ITEMEdit.Columns["N2ND_ITM_SZ_NM"];
        //        }
        //        else if (itmQty)
        //        {
        //            this.ViewJOB_ITEMEdit.CurrentColumn = this.ViewJOB_ITEMEdit.Columns["ITM_QTY"];
        //        }
        //        else if (purRmk)
        //        {
        //            this.ViewJOB_ITEMEdit.CurrentColumn = this.ViewJOB_ITEMEdit.Columns["PUR_RMK"];
        //        }

        //        this.ViewJOB_ITEMEdit.RefreshRow(rowHandle - 1);
        //        this.viewJOB_ITEMView.FocusedRowHandle = rowHandle;
        //    }
        //    catch (Exception eLog)
        //    {
        //        WinUIMessageBox.Show(eLog.Message, "[에러]" + _title, MessageBoxButton.OK, MessageBoxImage.Error);
        //        return;
        //    }
        //}


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

        public async void SYSTEM_CODE_VO()
        {
            // 창고 관리 P-008
            //using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "P-008"))
            //{
            //    if (response.IsSuccessStatusCode)
            //    {
            //        this.combo_INAUD_ORG_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
            //        this.lue_INAUD_ORG_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
            //    }
            //}


            // 거래처 AR 발주처 여부 
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s143", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { DELT_FLG = "N", CO_TP_CD = "AR", AREA_CD = SystemProperties.USER_VO.EMPE_PLC_NM, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    IList<SystemCodeVo> tmpList = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                    tmpList.Insert(0, new SystemCodeVo() { CO_NM = "전체" });

                    this.combo_GRP_NM.ItemsSource = tmpList;
                    this.combo_GRP_NM.SelectedItem = tmpList[0];
                }
            }
        }


    }
}
