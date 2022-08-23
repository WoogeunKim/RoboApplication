using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
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
using AquilaErpWpfApp3.Util;

namespace AquilaErpWpfApp3.View.INV.Dialog
{
    //기타 입고
    public partial class I5511DetailEtcDialog : DXWindow
    {
        //private static InvServiceClient invClient = SystemProperties.InvClient;
        private InvVo orgVo;
        private string _title = "기타 입고";

        //private static ItemCodeServiceClient itemClient = SystemProperties.ItemClient;

        public I5511DetailEtcDialog(InvVo vo)
        {
            InitializeComponent();

            this.orgVo = vo;


            SYSTEM_CODE_VO();

            //searchItem();

            

            this.btn_reset.Click += btn_reset_Click;
            this.btn_apply.Click += btn_apply_Click;

            //this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
            this.OKButton.Click += new RoutedEventHandler(OKButton_Click);
            this.CancelButton.Click += new RoutedEventHandler(CancelButton_Click);


            this.OKButton.IsEnabled = false;
        }

        void btn_reset_Click(object sender, RoutedEventArgs e)
        {
            searchItem();

        }

        private void btn_apply_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //if (this.check_CO_NM.IsChecked == true)
                //{
                //    if (string.IsNullOrEmpty(this.combo_CO_NM.Text))
                //    {
                //        WinUIMessageBox.Show("[거래처]입력 값이 맞지 안습니다", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                //        this.MSG.Text = "[거래처]입력 값이 맞지 안습니다";
                //        return;
                //    }
                //}


                if (this.check_IN_QTY.IsChecked == true)
                {
                    if (string.IsNullOrEmpty(this.text_IN_QTY.Text))
                    {
                        WinUIMessageBox.Show("[입고수량]입력 값이 맞지 안습니다", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                        this.MSG.Text = "[입고수량]입력 값이 맞지 안습니다";
                        return;
                    }
                }

                if (this.check_IMP_ITM_PRC.IsChecked == true)
                {
                    if (string.IsNullOrEmpty(this.text_IMP_ITM_PRC.Text))
                    {
                        WinUIMessageBox.Show("[단가]입력 값이 맞지 안습니다", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                        this.MSG.Text = "[단가]입력 값이 맞지 안습니다";
                        return;
                    }
                }


                if (this.check_INAUD_ORG_NM.IsChecked == true)
                {
                    if (string.IsNullOrEmpty(this.combo_INAUD_ORG_NM.Text))
                    {
                        WinUIMessageBox.Show("[입고창고]입력 값이 맞지 안습니다", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                        this.MSG.Text = "[입고창고]입력 값이 맞지 안습니다";
                        return;
                    }


                }



                IList<InvVo> _checkList = ((IList<InvVo>)this.ViewJOB_ITEMEdit.ItemsSource).Where(w => w.isCheckd == true).ToList<InvVo>();
                InvVo _tmpVo;
                for (int x = 0; x < _checkList.Count; x++)
                {
                    _tmpVo = _checkList[x];
                    if (this.check_IN_QTY.IsChecked == true)
                    {
                        _tmpVo.IN_QTY = Convert.ToInt32(string.IsNullOrEmpty(this.text_IN_QTY.Text) ? "0" : this.text_IN_QTY.Text);

                        _tmpVo.isCheckd = true;
                        this.OKButton.IsEnabled = true;
                    }

                    if (this.check_IMP_ITM_PRC.IsChecked == true)
                    {
                        _tmpVo.IMP_ITM_PRC = Convert.ToInt32(string.IsNullOrEmpty(this.text_IMP_ITM_PRC.Text) ? "0" : this.text_IMP_ITM_PRC.Text);

                        _tmpVo.isCheckd = true;
                        this.OKButton.IsEnabled = true;
                    }

                    //if (this.check_CO_NM.IsChecked == true)
                    //{
                    //    CustomerCodeDao inaudOrgVo = this.combo_CO_NM.SelectedItem as CustomerCodeDao;
                    //    if (inaudOrgVo != null)
                    //    {
                    //        _tmpVo.CO_NM = inaudOrgVo.CO_NM;
                    //        _tmpVo.CO_CD = inaudOrgVo.CO_NO;

                    //        _tmpVo.isCheckd = true;
                    //        this.OKButton.IsEnabled = true;
                    //    }
                    //}

                    if (this.check_INAUD_ORG_NM.IsChecked == true)
                    {
                        SystemCodeVo inaudOrgVo = this.combo_INAUD_ORG_NM.SelectedItem as SystemCodeVo;
                        if (inaudOrgVo != null)
                        {
                            _tmpVo.INAUD_ORG_NM = inaudOrgVo.CLSS_DESC;
                            _tmpVo.INAUD_ORG_NO = inaudOrgVo.CLSS_CD;

                            _tmpVo.isCheckd = true;
                            this.OKButton.IsEnabled = true;
                        }
                    }

                    if (this.check_PUR_RMK.IsChecked == true)
                    {
                        _tmpVo.PUR_RMK = this.text_PUR_RMK.Text;

                        _tmpVo.isCheckd = true;
                        this.OKButton.IsEnabled = true;
                    }
                }
                this.ViewJOB_ITEMEdit.RefreshData();
            }
            catch (Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[에러]" + _title, MessageBoxButton.OK, MessageBoxImage.Error);
                this.MSG.Text = eLog.Message;
                return;
            }
        }



        private async void searchItem()
        {
            try
            {


                if (DXSplashScreen.IsActive == false)
                {
                    DXSplashScreen.Show<ProgressWindow>();
                }


                SystemCodeVo ITM_GRP_CLSS_NM_VO = this.combo_ITM_GRP_CLSS_CD.SelectedItem as SystemCodeVo;
                //SystemCodeVo ITM_N1ST_TP_NM_VO = this.combo_N1ST_ITM_GRP_CD.SelectedItem as SystemCodeVo;
                //SystemCodeVo N2ND_ITM_GRP_NM_VO = this.combo_N2ND_ITM_GRP_CD.SelectedItem as SystemCodeVo;

                //if (ITM_N1ST_TP_NM_VO == null)
                //{
                //    WinUIMessageBox.Show("[대그룹]을 다시 선택 해주세요", "[조회 조건]" + _title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                //    return;
                //}

                //if (N2ND_ITM_GRP_NM_VO == null)
                //{
                //    WinUIMessageBox.Show("[중그룹]을 다시 선택 해주세요", "[조회 조건]" + _title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                //    return;
                //}
                //, N1ST_ITM_GRP_CD = ITM_N1ST_TP_NM_VO.ITM_GRP_CD, N2ND_ITM_GRP_CD = N2ND_ITM_GRP_NM_VO?.ITM_GRP_CD
                InvVo vo = new InvVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD, ITM_GRP_CLSS_CD = ITM_GRP_CLSS_NM_VO.CLSS_CD, AREA_CD = SystemProperties.USER_VO.EMPE_PLC_NM, CO_CD = this.orgVo.CO_CD, CO_NM = this.orgVo.CO_NM };

                this.search_title.Text = "[조회 조건]   " + "분류 : " + ITM_GRP_CLSS_NM_VO.CLSS_DESC; /*+ ", 대 그룹 : " + ITM_N1ST_TP_NM_VO.ITM_GRP_NM + ", 중 그룹 : " + N2ND_ITM_GRP_NM_VO?.ITM_GRP_NM*/;
                //CustomerCodeDao grpCdVo = this.combo_GRP_NM.SelectedItem as CustomerCodeDao;
                //if (grpCdVo != null)
                //{
                //    vo.CO_NO = (string.IsNullOrEmpty(grpCdVo.CO_NO) ? null : grpCdVo.CO_NO);
                //    vo.CO_CD = (string.IsNullOrEmpty(grpCdVo.CO_NO) ? null : grpCdVo.CO_NO);
                //    vo.CO_NM = (string.IsNullOrEmpty(grpCdVo.CO_NM) ? null : grpCdVo.CO_NM);
                //}
                //else
                //{
                //    WinUIMessageBox.Show("[부서 명]을 다시 선택 해주세요", "[조회 조건]품목 입고 관리", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                //    return;
                //}
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("i5511/popup/etc", new StringContent(JsonConvert.SerializeObject(vo), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.ViewJOB_ITEMEdit.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<InvVo>>(await response.Content.ReadAsStringAsync()).Cast<InvVo>().ToList();
                    }
                }
                //this.ViewJOB_ITEMEdit.ItemsSource = invClient.I5511SelectDtlEtcInList(vo);




                if (DXSplashScreen.IsActive == true)
                {
                    DXSplashScreen.Close();
                }
            }
            catch (Exception eLog)
            {

                if (DXSplashScreen.IsActive == true)
                {
                    DXSplashScreen.Close();
                }


                WinUIMessageBox.Show(eLog.Message, "[에러]" + _title, MessageBoxButton.OK, MessageBoxImage.Error);
                this.MSG.Text = eLog.Message;
                return;
            }
        }

        #region Functon (OKButton_Click, CancelButton_Click)
        private async void OKButton_Click(object sender, RoutedEventArgs e)
        {
            bool isMsg = false;
            IList<InvVo> checkList = (IList<InvVo>)this.ViewJOB_ITEMEdit.ItemsSource;
            List<InvVo> saveList = new List<InvVo>();
            int nSize = checkList.Count;
            if (nSize <= 0)
            {
                WinUIMessageBox.Show("데이터가 존재 하지 않습니다.", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }


            MessageBoxResult result = WinUIMessageBox.Show("정말로 저장 하시겠습니까?", "[저장]" + _title, MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                InvVo tmpVo;
                for (int x = 0; x < nSize; x++)
                {
                    tmpVo = checkList[x];
                    if (tmpVo.isCheckd)
                    {
                        if (string.IsNullOrEmpty(tmpVo.INAUD_ORG_NO))
                        {
                            WinUIMessageBox.Show("[입고 창고] 입력 값이 맞지 않습니다", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                            this.MSG.Text = "[입고 창고] 입력 값이 맞지 않습니다";
                            return;
                        }


                        isMsg = true;
                        //
                        InvVo insertVo = new InvVo() {
                                  INSRL_NO = this.orgVo.INSRL_NO
                                //, INAUD_CD = "RGM"
                                , INAUD_CD = tmpVo.INAUD_CD
                                , LOC_CD = tmpVo.INAUD_ORG_NO
                                , CO_NO = orgVo.CO_CD
                                , CO_CD = orgVo.CO_CD
                                , ITM_CD = tmpVo.ITM_CD
                                , INAUD_QTY = tmpVo.IN_QTY
                                , IMP_ITM_PRC = tmpVo.IMP_ITM_PRC
                                , IMP_ITM_AMT = tmpVo.IMP_ITM_AMT
                                , ORD_NO = ""
                                , ORD_SEQ = null
                                , INAUD_RMK = tmpVo.PUR_RMK
                                , ROUT_CD = tmpVo.ROUT_CD
                                , CHNL_CD = SystemProperties.USER_VO.CHNL_CD
                                , CRE_USR_ID = SystemProperties.USER
                                , UPD_USR_ID = SystemProperties.USER };
    

                        //resultVo = invClient.I5511InsertPurDtl(insertVo);
                        //if (!resultVo.isSuccess)
                        //{
                        //    //실패
                        //    WinUIMessageBox.Show(resultVo.Message, "[에러]품목 입고 관리", MessageBoxButton.OK, MessageBoxImage.Error);
                        //    return;
                        //}
                        saveList.Add(insertVo);
                        tmpVo.isCheckd = false;
                    }
                }


                if (isMsg)
                {
                    int _Num = 0;
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("i5511/dtl/i", new StringContent(JsonConvert.SerializeObject(saveList.ToArray()), System.Text.Encoding.UTF8, "application/json")))
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
                        //InvVo resultVo = invClient.I6610InsertPurMst_Transaction(saveList.ToArray());
                        //if (!resultVo.isSuccess)
                        //{
                        //    //실패
                        //    WinUIMessageBox.Show(resultVo.Message, "[에러]" + _title, MessageBoxButton.OK, MessageBoxImage.Error);
                        //    return;
                        //}
                        WinUIMessageBox.Show("[총 계수 : " + saveList.Count + "] - 저장 완료 되었습니다", "[수정]" + _title, MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    //
                    this.OKButton.IsEnabled = false;
                }
            }

            // 종료 여부
            if (isMsg)
            {
                this.DialogResult = true;
                this.Close();
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
                InvVo masterDomain = (InvVo)ViewJOB_ITEMEdit.GetFocusedRow();
                bool inaudNm = (e.Column.FieldName.ToString().Equals("INAUD_NM") ? true : false);
                bool rmnQty = (e.Column.FieldName.ToString().Equals("IN_QTY") ? true : false);
                bool inaudOrgNm = (e.Column.FieldName.ToString().Equals("INAUD_ORG_NM") ? true : false);
                bool inaudRmk = (e.Column.FieldName.ToString().Equals("PUR_RMK") ? true : false);
                bool impItmPrc = (e.Column.FieldName.ToString().Equals("IMP_ITM_PRC") ? true : false);
                //bool isCheckd = (e.Column.FieldName.ToString().Equals("isCheckd") ? true : false);

                //InvVo resultVo;
                //InvVo insertVo;
                //
                if (rmnQty)
                {
                    if (e.IsValid)
                    {
                        if (string.IsNullOrEmpty(masterDomain.IN_QTY + ""))
                        {
                            masterDomain.IN_QTY = 0;
                        }
                        //
                        if (!masterDomain.IN_QTY.Equals((e.Value == null ? "" : e.Value.ToString())))
                        {
                            int? tmpInt = Convert.ToInt32(e.Value.ToString());
                            //if (int.Parse(e.Value.ToString()) > masterDomain.PUR_QTY)
                            //{
                            //    e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                            //    e.ErrorContent = "[발주수량]" + masterDomain.PUR_QTY + " 보다 큰 값은 입력 하실수 없습니다";
                            //    e.SetError(e.ErrorContent, e.ErrorType);
                            //    return;
                            //}
                            //else if(e.Value.ToString().Equals("0"))
                            //{
                            //    e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                            //    e.ErrorContent = "0 보다 작은 값은 입력 하실수 없습니다";
                            //    e.SetError(e.ErrorContent, e.ErrorType);
                            //    return;
                            //}

                            masterDomain.IN_QTY = tmpInt;

                            try
                            {
                                masterDomain.IMP_ITM_AMT = tmpInt * Convert.ToDouble(masterDomain.IMP_ITM_PRC);
                            }
                            catch (Exception)
                            {
                                masterDomain.IMP_ITM_AMT = 0;
                            }

                            masterDomain.isCheckd = true;
                            this.OKButton.IsEnabled = true;
                            //insertVo = new InvVo() {
                            //          INSRL_NO = this.orgVo.INSRL_NO
                            //        , INAUD_CD = masterDomain.INAUD_CD
                            //        , ITM_CD = masterDomain.ITM_CD
                            //        , CO_UT_PRC = masterDomain.CO_UT_PRC
                            //        , PUR_AMT = masterDomain.PUR_AMT
                            //        , PUR_ORD_NO = masterDomain.PUR_ORD_NO
                            //        , PUR_ORD_SEQ = masterDomain.PUR_ORD_SEQ
                            //        , RMN_QTY = int.Parse(e.Value.ToString())
                            //        , CRE_USR_ID = SystemProperties.USER
                            //        , UPD_USR_ID = SystemProperties.USER };

                            //resultVo = invClient.I5511InsertPurDtl(insertVo);
                            //if (!resultVo.isSuccess)
                            //{
                            //    //실패
                            //    WinUIMessageBox.Show(resultVo.Message, "[에러]품목 입고 관리", MessageBoxButton.OK, MessageBoxImage.Error);
                            //    return;
                            //}
                        }
                    }
                }
                else if (inaudNm)
                {
                    if (e.IsValid)
                    {
                        if (string.IsNullOrEmpty(masterDomain.INAUD_CD + ""))
                        {
                            masterDomain.INAUD_NM = "";
                            masterDomain.INAUD_CD = "";
                        }
                        //
                        if (!masterDomain.INAUD_CD.Equals((e.Value == null ? "" : e.Value.ToString())))
                        {
                            SystemCodeVo bankIoDao = this.lue_INAUD_NM.GetItemFromValue(e.Value) as SystemCodeVo;
                            //
                            if (bankIoDao != null)
                            {
                                masterDomain.INAUD_CD = bankIoDao.CLSS_CD;
                                masterDomain.INAUD_NM = bankIoDao.CLSS_DESC;
                            }

                            masterDomain.isCheckd = true;
                            this.OKButton.IsEnabled = true;
                        }
                    }
                }
                else if (inaudOrgNm)
                {
                    if (e.IsValid)
                    {
                        if (string.IsNullOrEmpty(masterDomain.INAUD_ORG_NO))
                        {
                            masterDomain.INAUD_ORG_NM = "";
                            masterDomain.INAUD_ORG_NO = "";
                        }
                        //
                        if (!masterDomain.INAUD_ORG_NO.Equals((e.Value == null ? "" : e.Value.ToString())))
                        {
                            SystemCodeVo bankIoDao = this.lue_INAUD_ORG_NM.GetItemFromValue(e.Value) as SystemCodeVo;
                            //
                            if (bankIoDao != null)
                            {
                                masterDomain.INAUD_ORG_NO = bankIoDao.CLSS_CD;
                                masterDomain.INAUD_ORG_NM = bankIoDao.CLSS_DESC;
                            }

                            masterDomain.isCheckd = true;
                            this.OKButton.IsEnabled = true;
                        }
                    }
                }
                else if (inaudRmk)
                {
                    if (e.IsValid)
                    {
                        if (string.IsNullOrEmpty(masterDomain.PUR_RMK))
                        {
                            masterDomain.PUR_RMK = "";
                        }
                        //
                        if (!masterDomain.PUR_RMK.Equals((e.Value == null ? "" : e.Value.ToString())))
                        {
                            masterDomain.PUR_RMK = e.Value.ToString();
                            masterDomain.isCheckd = true;
                            this.OKButton.IsEnabled = true;
                        }
                    }
                }
                else if (impItmPrc)
                {
                    if (e.IsValid)
                    {
                        if (string.IsNullOrEmpty(masterDomain.IMP_ITM_PRC + ""))
                        {
                            masterDomain.IMP_ITM_PRC = 0;
                        }
                        //
                        if (!masterDomain.IMP_ITM_PRC.Equals((e.Value == null ? "" : e.Value.ToString())))
                        {
                            double? tmpInt = Convert.ToDouble(e.Value.ToString());
                            //if (tmpInt <= -1)
                            //{
                            //    e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                            //    e.ErrorContent = "[단가]" + tmpInt + " -값은 입력 하실수 없습니다";
                            //    e.SetError(e.ErrorContent, e.ErrorType);
                            //    return;
                            //}

                            masterDomain.IMP_ITM_PRC = tmpInt;

                            try
                            {
                                masterDomain.IMP_ITM_AMT = Convert.ToInt32(masterDomain.IN_QTY) * tmpInt;
                            }
                            catch (Exception)
                            {
                                masterDomain.IMP_ITM_AMT = 0;
                            }

                            masterDomain.isCheckd = true;
                            this.OKButton.IsEnabled = true;
                        }
                    }
                }

                this.ViewJOB_ITEMEdit.RefreshData();
            }
            catch (Exception eLog)
            {
                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                e.ErrorContent = eLog.Message;
                this.MSG.Text = eLog.Message;
                e.SetError(e.ErrorContent, e.ErrorType);
                return;
            }
        }

        private void viewPage1EditView_HiddenEditor(object sender, DevExpress.Xpf.Grid.EditorEventArgs e)
        {
            this.viewJOB_ITEMView.CommitEditing();

            //bool coNm = (e.Column.FieldName.ToString().Equals("CO_NM") ? true : false);
            //bool rmnQty = (e.Column.FieldName.ToString().Equals("IN_QTY") ? true : false);
            //bool inaudOrgNm = (e.Column.FieldName.ToString().Equals("INAUD_ORG_NM") ? true : false);
            //bool inaudRmk = (e.Column.FieldName.ToString().Equals("PUR_RMK") ? true : false);
            //bool impItmPrc = (e.Column.FieldName.ToString().Equals("IMP_ITM_PRC") ? true : false);

            //int rowHandle = this.viewJOB_ITEMView.FocusedRowHandle + 1;

            ////if (rmnQty)
            ////{
            ////    this.ViewJOB_ITEMEdit.CurrentColumn = this.ViewJOB_ITEMEdit.Columns["RMN_QTY"];
            ////}

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

        private void PART_Editor_Checked(object sender, RoutedEventArgs e)
        {
            this.OKButton.IsEnabled = true;
        }



        async void combo_ITM_GRP_CLSS_CD_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                SystemCodeVo ITM_GRP_CLSS_NM_VO = this.combo_ITM_GRP_CLSS_CD.SelectedItem as SystemCodeVo;
                if (ITM_GRP_CLSS_NM_VO != null)
                {
                    //if (string.IsNullOrEmpty(ITM_GRP_CLSS_NM_VO.CLSS_CD))
                    //{
                    //    return;
                    //}

                    //using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s133", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { DELT_FLG = "N", ITM_GRP_CLSS_CD = ITM_GRP_CLSS_NM_VO.CLSS_CD, CRE_USR_ID = "", CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                    //{
                    //    if (response.IsSuccessStatusCode)
                    //    {
                    //        this.combo_N1ST_ITM_GRP_CD.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                    //    }
                    //}

                    //SystemCodeVo ITM_GRP_CLSS_NM_VO = this.combo_ITM_GRP_CLSS_CD.SelectedItem as SystemCodeVo;
                    //if (ITM_GRP_CLSS_NM_VO != null)
                    //{
                    //    if (string.IsNullOrEmpty(ITM_GRP_CLSS_NM_VO.CLSS_CD))
                    //    {
                    //        return;
                    //    }
                    //    this.combo_N1ST_ITM_GRP_CD.ItemsSource = SystemProperties.ITM_N1ST_CODE_VO(ITM_GRP_CLSS_NM_VO.CLSS_CD);
                    //}

                    //this.combo_N1ST_ITM_GRP_CD.SelectedIndex = 0;

                    //combo_N1ST_ITM_GRP_CD_SelectedIndexChanged(sender, e);
                    ////CodeDao ITM_GRP_CLSS_NM_VO = this.combo_ITM_GRP_CLSS_CD.SelectedItem as CodeDao;
                    ////if (ITM_GRP_CLSS_NM_VO != null)
                    ////{
                    ////    if (string.IsNullOrEmpty(ITM_GRP_CLSS_NM_VO.CLSS_CD))
                    ////    {
                    ////        return;
                    ////    }

                    ////    IList<ItemGroupCodeVo> ItemGroupVo = itemClient.SelectCodeItemGroupList(new ItemGroupCodeVo() { DELT_FLG = "N", ITM_GRP_CLSS_CD = ITM_GRP_CLSS_NM_VO.CLSS_CD, CRE_USR_ID = "" });
                    ////    IList<CodeDao> ItemList = new List<CodeDao>();
                    ////    int nCnt = ItemGroupVo.Count;
                    ////    ItemGroupCodeVo tmpVo;

                    ////    this.combo_N1ST_ITM_GRP_CD.Clear();

                    ////    for (int x = 0; x < nCnt; x++)
                    ////    {
                    ////        tmpVo = ItemGroupVo[x];
                    ////        ItemList.Add(new CodeDao() { CLSS_DESC = tmpVo.ITM_GRP_NM, CLSS_CD = tmpVo.ITM_GRP_CD });
                    ////    }
                    ////    this.combo_N1ST_ITM_GRP_CD.ItemsSource = ItemList;
                    ////}
                    ///}
                }
            }
            catch (Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error);
                this.MSG.Text = eLog.Message;
                return;
            }
        }

        async void combo_N1ST_ITM_GRP_CD_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                SystemCodeVo ITM_GRP_CLSS_NM_VO = this.combo_ITM_GRP_CLSS_CD.SelectedItem as SystemCodeVo;
                if (ITM_GRP_CLSS_NM_VO != null)
                {
                    //SystemCodeVo ITM_N1ST_TP_NM_VO = this.combo_N1ST_ITM_GRP_CD.SelectedItem as SystemCodeVo;
                    //if (ITM_N1ST_TP_NM_VO != null)
                    //{
                    //    if (string.IsNullOrEmpty(ITM_N1ST_TP_NM_VO.ITM_GRP_CD))
                    //    {
                    //        return;
                    //    }

                    //    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s133", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { PRNT_ITM_GRP_CD = "X", N1ST_ITM_GRP_CD = ITM_N1ST_TP_NM_VO.ITM_GRP_CD, DELT_FLG = "N", ITM_GRP_CLSS_CD = ITM_GRP_CLSS_NM_VO.CLSS_CD, CRE_USR_ID = "", CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                    //    {
                    //        if (response.IsSuccessStatusCode)
                    //        {
                    //            this.combo_N2ND_ITM_GRP_CD.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                    //        }
                    //    }
                    //}
                }
                //SystemCodeVo ITM_GRP_CLSS_NM_VO = this.combo_ITM_GRP_CLSS_CD.SelectedItem as SystemCodeVo;
                //if (ITM_GRP_CLSS_NM_VO != null)
                //{
                //    SystemCodeVo ITM_N1ST_TP_NM_VO = this.combo_N1ST_ITM_GRP_CD.SelectedItem as SystemCodeVo;
                //    if (ITM_N1ST_TP_NM_VO != null)
                //    {
                //        if (string.IsNullOrEmpty(ITM_N1ST_TP_NM_VO.CLSS_CD))
                //        {
                //            return;
                //        }

                //        IList<ItemGroupCodeVo> ItemGroupVo = itemClient.SelectCodeItemGroupList(new ItemGroupCodeVo() { PRNT_ITM_GRP_CD = "X", N1ST_ITM_GRP_CD = ITM_N1ST_TP_NM_VO.CLSS_CD, DELT_FLG = "N", ITM_GRP_CLSS_CD = ITM_GRP_CLSS_NM_VO.CLSS_CD, CRE_USR_ID = "" });
                //        IList<CodeDao> ItemList = new List<CodeDao>();
                //        int nCnt = ItemGroupVo.Count;
                //        ItemGroupCodeVo tmpVo;

                //        this.combo_N2ND_ITM_GRP_CD.Clear();
                //        for (int x = 0; x < nCnt; x++)
                //        {
                //            tmpVo = ItemGroupVo[x];
                //            ItemList.Add(new CodeDao() { CLSS_DESC = tmpVo.ITM_GRP_NM, CLSS_CD = tmpVo.ITM_GRP_CD });
                //        }
                //        this.combo_N2ND_ITM_GRP_CD.ItemsSource = ItemList;
                //        this.combo_N2ND_ITM_GRP_CD.SelectedIndex = 0;
                //    }
                //}
            }
            catch (Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error);
                this.MSG.Text = eLog.Message;
                return;
            }
        }


        public async void SYSTEM_CODE_VO()
        {
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "L-007"))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.lue_INAUD_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().Where<SystemCodeVo>(x => x.CLSS_CD.StartsWith("R")).ToList();
                    //this.combo_INAUD_ORG_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                }
            }

            //this.lue_INAUD_ORG_NM.ItemsSource = SystemProperties.SYSTEM_CODE_VO("L-100");
            //this.combo_INAUD_ORG_NM.ItemsSource = SystemProperties.SYSTEM_CODE_VO("L-100");
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "L-100"))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.lue_INAUD_ORG_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                    this.combo_INAUD_ORG_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                }
            }

            //this.combo_ITM_GRP_CLSS_CD.ItemsSource = SystemProperties.SYSTEM_CODE_VO("L-001");
            //this.combo_ITM_GRP_CLSS_CD.SelectedIndexChanged += combo_ITM_GRP_CLSS_CD_SelectedIndexChanged;
            //this.combo_ITM_GRP_CLSS_CD.SelectedIndex = 0;
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "L-001"))
            {
                if (response.IsSuccessStatusCode)
                {
                    //중분류
                    this.combo_ITM_GRP_CLSS_CD.SelectedIndexChanged += combo_ITM_GRP_CLSS_CD_SelectedIndexChanged;

                    //대분류
                    //this.combo_N1ST_ITM_GRP_CD.SelectedIndexChanged += combo_N1ST_ITM_GRP_CD_SelectedIndexChanged;

                    //분류
                    this.combo_ITM_GRP_CLSS_CD.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();

                    //N1ST_ITM_GRP_CD = this.orgDao.N1ST_ITM_GRP_CD;
                    //N1ST_ITM_GRP_NM = this.orgDao.N1ST_ITM_GRP_NM;
                    //N2ND_ITM_GRP_CD = this.orgDao.N2ND_ITM_GRP_CD;
                    //N2ND_ITM_GRP_NM = this.orgDao.N2ND_ITM_GRP_NM;
                    //ITM_GRP_CLSS_CD = this.orgDao.ITM_GRP_CLSS_CD;

                    //this.combo_ITM_GRP_CLSS_CD.Text = this.orgDao.ITM_GRP_CLSS_NM;
                    this.combo_ITM_GRP_CLSS_CD.SelectedIndex = 0;
                    //combo_ITM_GRP_CLSS_CD_SelectedIndexChanged(null, null);
                    SystemCodeVo ITM_GRP_CLSS_NM_VO = this.combo_ITM_GRP_CLSS_CD.SelectedItem as SystemCodeVo;
                    if (ITM_GRP_CLSS_NM_VO != null)
                    {
                        if (string.IsNullOrEmpty(ITM_GRP_CLSS_NM_VO.CLSS_CD))
                        {
                            return;
                        }

                        //using (HttpResponseMessage response_1 = await SystemProperties.PROGRAM_HTTP.PostAsync("s133", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { DELT_FLG = "N", ITM_GRP_CLSS_CD = ITM_GRP_CLSS_NM_VO.CLSS_CD, CRE_USR_ID = "", CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                        //{
                        //    if (response_1.IsSuccessStatusCode)
                        //    {
                        //        this.combo_N1ST_ITM_GRP_CD.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response_1.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                        //    }
                        //}
                    }

                    //combo_N1ST_ITM_GRP_CD_SelectedIndexChanged(null, null);
                    //SystemCodeVo ITM_N1ST_TP_NM_VO = this.combo_N1ST_ITM_GRP_CD.SelectedItem as SystemCodeVo;
                    //if (ITM_N1ST_TP_NM_VO != null)
                    //{
                    //    if (string.IsNullOrEmpty(ITM_N1ST_TP_NM_VO.ITM_GRP_CD))
                    //    {
                    //        return;
                    //    }

                    //    using (HttpResponseMessage response_2 = await SystemProperties.PROGRAM_HTTP.PostAsync("s133", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { PRNT_ITM_GRP_CD = "X", N1ST_ITM_GRP_CD = ITM_N1ST_TP_NM_VO.ITM_GRP_CD, DELT_FLG = "N", ITM_GRP_CLSS_CD = ITM_GRP_CLSS_NM_VO.CLSS_CD, CRE_USR_ID = "", CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                    //    {
                    //        if (response_2.IsSuccessStatusCode)
                    //        {
                    //            this.combo_N2ND_ITM_GRP_CD.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response_2.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                    //        }
                    //    }
                    //}
                   
                }
            }
        }


    }
}
