using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Code;
using ModelsLibrary.Sale;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using AquilaErpWpfApp3.Util;

namespace AquilaErpWpfApp3.View.SAL.Dialog
{
    public partial class S22111DetailDialog : DXWindow
    {
        //private static SaleOrderServiceClient saleOrderClient = SystemProperties.SaleOrderClient;
        //private static ItemCodeServiceClient itemClient = SystemProperties.ItemClient;

        private SaleVo orgDao;
        //private bool isEdit = false;
        //private JobVo updateDao;

        private string title = "출하 의뢰 등록";

        //private ObservableCollection<SaleVo> right2List = new ObservableCollection<SaleVo>();

        public S22111DetailDialog(SaleVo Dao)
        {
            InitializeComponent();

            SYSTEM_CODE_VO();
            //- this.ViewGridRight2


            this.combo_ITM_GRP_CLSS_NM.SelectedIndexChanged += combo_ITM_GRP_CLSS_NM_SelectedIndexChanged;
            //this.combo_ITM_GRP_CLSS_NM.Text = "볼트";
            //this.combo_HD_LEN_NM.Text = "전체";
            //this.ViewGridLeft.IsEnabled = false;


            //this.ViewGridLeft.SelectedItemChanged += ViewGridLeft_SelectedItemChanged;
            this.ViewGridLeft.MouseDoubleClick += ViewGridLeft_MouseDoubleClick;


            this.orgDao = Dao;


            //if (SystemProperties.USER_VO.EMPE_PLC_NM.Equals("001"))
            //{
            //    ////본사
            //    //this.gridColumn_B.VisibleIndex = 2;
            //    //this.gridColumn_C.VisibleIndex = 3;
            //    this.gridColumn_A.VisibleIndex = 1;
            //}
            //else if (SystemProperties.USER_VO.EMPE_PLC_NM.Equals("200"))
            //{
            //    ////부산
            //    this.gridColumn_A.VisibleIndex = 2;
            //    this.gridColumn_C.VisibleIndex = 3;
            //    this.gridColumn_B.VisibleIndex = 1;
            //}
            //else if (SystemProperties.USER_VO.EMPE_PLC_NM.Equals("300"))
            //{
            //    ////대구
            //    this.gridColumn_A.VisibleIndex = 2;
            //    this.gridColumn_B.VisibleIndex = 3;
            //    this.gridColumn_C.VisibleIndex = 1;
            //}

            combo_ITM_GRP_CLSS_NM_SelectedIndexChanged(null, null);

            this.ViewGridRight1.MouseDoubleClick += ViewGridRight1_MouseDoubleClick;

            this.btn_Search.Click += btn_Search_Click;

            this.OKButton.Click += new RoutedEventHandler(OKButton_Click);
            this.CancelButton.Click += new RoutedEventHandler(CancelButton_Click);


            this.text_ITM_CD.Focus();
        }

        void ViewGridLeft_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ViewGridLeft_SelectedItemChanged(sender, null);
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

        void btn_Search_Click(object sender, RoutedEventArgs e)
        {
            ViewGridLeft_SelectedItemChanged(sender, null);
        }


        IList<SaleVo> tmpDao = new List<SaleVo>();
        void ViewGridRight1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                //SaleVo resultVo;
                SaleVo selVo = (SaleVo)this.ViewGridRight1.SelectedItem;
                if (selVo == null)
                {
                    return;
                }
                ////else  if (Convert.ToInt32(selVo.STK_A_QTY) <= 0)
                ////{
                ////    //WinUIMessageBox.Show("[본사]" + Convert.ToInt32(selVo.STK_A_QTY) + " 재고가 부족 합니다", "[재고 부족]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                ////    this.MSG.Text = "[본사]" + selVo.ITM_CD + " / " + Convert.ToInt32(selVo.STK_A_QTY) + " 재고가 부족 합니다";
                ////    WinUIMessageBox.Show(this.MSG.Text, title, MessageBoxButton.OK, MessageBoxImage.Warning);
                ////    //return;
                ////    //return;
                ////}
                ////else if (saleOrderClient.S2211SelectLmtCheck(new JobVo() { CO_CD = this.orgDao.SL_CO_CD }).Equals("N"))
                ////{
                ////    this.MSG.Text = "출고 한도 초과되었습니다";
                ////    WinUIMessageBox.Show(this.MSG.Text, title, MessageBoxButton.OK, MessageBoxImage.Warning);
                ////    return;
                ////}
                else
                {
                    this.MSG.Text = "";
                }

                //if (string.IsNullOrWhiteSpace(Convert.ToString(selVo.D_GRD_PRC)))
                //{
                //    WinUIMessageBox.Show("단가가 없는 경우 판매할 수 없습니다.", "[단가 없음]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                //    return;
                //}
                //else if (Convert.ToDecimal(selVo.D_GRD_PRC) == 0)
                //{
                //    WinUIMessageBox.Show("단가가 없는 경우 판매할 수 없습니다.", "[단가 없음]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                //    return;
                //}


                SaleVo insertDao = new SaleVo();
                insertDao.SL_RLSE_NO = this.orgDao.SL_RLSE_NO;
                insertDao.SL_ITM_CD = selVo.ITM_CD;
                insertDao.ITM_CD = selVo.ITM_CD;
                insertDao.ITM_NM = selVo.ITM_NM;
                insertDao.ITM_SZ_NM = selVo.ITM_SZ_NM;
                insertDao.CRE_USR_ID = SystemProperties.USER;
                insertDao.UPD_USR_ID = SystemProperties.USER;
                insertDao.AREA_CD = SystemProperties.USER_VO.EMPE_PLC_NM;
                insertDao.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
                insertDao.SL_ITM_QTY = selVo.SL_ITM_QTY;
                insertDao.ITM_QTY = 0;
                //S2211SelectQuickDtl
                //insertDao.RN = 3;


                //resultVo = saleOrderClient.S2211SelectQuickDtl(insertDao);
                //using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s2211/popup/r", new StringContent(JsonConvert.SerializeObject(insertDao), System.Text.Encoding.UTF8, "application/json")))
                //{
                //    if (response.IsSuccessStatusCode)
                //    {
                //        resultVo = JsonConvert.DeserializeObject<SaleVo>(await response.Content.ReadAsStringAsync());

                        tmpDao.Add(insertDao);

                        //한 줄출력
                        this.ViewGridRight2.ItemsSource = tmpDao;

                        //insertDao.RN = (this.right2List.Count == 0 ? 1 : this.right2List.Count+1);
                        //insertDao.MD_PER_QTY = resultVo.MD_PER_QTY;
                        //insertDao.PK_PER_QTY = resultVo.PK_PER_QTY;

                        //insertDao.SL_ITM_QTY = resultVo.SL_ITM_QTY;
                        //insertDao.MD_QTY = resultVo.MD_QTY;
                        //insertDao.PK_QTY = resultVo.PK_QTY;
                        //insertDao.SL_ITM_RMN_QTY = resultVo.SL_ITM_RMN_QTY;
                        //insertDao.SL_ITM_WGT = resultVo.SL_ITM_WGT;
                        //insertDao.SL_ITM_PRC_VAL = resultVo.SL_ITM_PRC_VAL;

                        //this.right2List.Add(insertDao);

                        //for (int x = 0; x < this.right2List.Count; x++)
                        //{
                        //    this.right2List[x].RN = (x + 1);
                        //}

                        ////this.ViewGridRight2.ItemsSource = new ObservableCollection<JobVo>(saleOrderClient.S2211SelectSubDtlList(this.orgDao));
                        this.ViewTableRight2.FocusedRowHandle = this.ViewGridRight2.VisibleRowCount - 1;
                        this.ViewGridRight2.RefreshData();
                    //}
                //}
            }
            catch (Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

        }

        async void ViewGridLeft_SelectedItemChanged(object sender, DevExpress.Xpf.Grid.SelectedItemChangedEventArgs e)
        {
            try
            {
                //CodeDao hdLenNmVo = this.combo_HD_LEN_NM.SelectedItem as CodeDao;
                SystemCodeVo itmGrpClssNmVo = this.combo_ITM_GRP_CLSS_NM.SelectedItem as SystemCodeVo;

                SystemCodeVo selVo = (SystemCodeVo)this.ViewGridLeft.SelectedItem;
                if (selVo == null)
                {
                    this.ViewGridRight1.ItemsSource = null;
                    return;
                }

                if (itmGrpClssNmVo != null)
                {
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s22111/popupitems", new StringContent(JsonConvert.SerializeObject(new SaleVo()
                    {
                        ITM_CD = (string.IsNullOrEmpty(this.text_ITM_CD.Text) ? null : this.text_ITM_CD.Text),
                        ITM_GRP_CLSS_CD = (string.IsNullOrEmpty(itmGrpClssNmVo.CLSS_CD) ? null : itmGrpClssNmVo.CLSS_CD),
                        N1ST_ITM_GRP_CD = (string.IsNullOrEmpty(itmGrpClssNmVo.CLSS_CD) ? null : selVo.N1ST_ITM_GRP_CD),
                        N2ND_ITM_GRP_CD = (string.IsNullOrEmpty(itmGrpClssNmVo.CLSS_CD) ? null : selVo.N2ND_ITM_GRP_CD),
                        //HD_LEN_CD = (string.IsNullOrEmpty(hdLenNmVo.CLSS_CD) ? null : hdLenNmVo.CLSS_CD),
                        AREA_CD = SystemProperties.USER_VO.EMPE_PLC_NM,
                        SL_RLSE_DT = this.orgDao.SL_RLSE_DT,
                        CHNL_CD = SystemProperties.USER_VO.CHNL_CD
                    }), System.Text.Encoding.UTF8, "application/json")))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            this.ViewGridRight1.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SaleVo>>(await response.Content.ReadAsStringAsync()).Cast<SaleVo>().ToList();
                        }
                    }
                    //   // this.search_title.Text = "[조회 조건]   사업장 : " + SystemProperties.USER_VO.EMPE_PLC_CD + ", 구분 : " + itmGrpClssNmVo.CLSS_DESC + (hdLenNmVo == null ? "" : ", 호칭 : " + hdLenNmVo.CLSS_DESC) + ", 품목 명 : " + selVo.N1ST_ITM_GRP_NM + ", 분류 명 : " + selVo.N2ND_ITM_GRP_NM + (itmGrpClssNmVo.CLSS_DESC.Equals("전체") ? ", 품목 코드 : " + this.text_ITM_CD.Text : "");
                    //    this.ViewGridRight1.ItemsSource = itemClient.S2211SelectCodeItemDtlGroupList(new ItemCodeVo() {ITM_CD = (string.IsNullOrEmpty(this.text_ITM_CD.Text) ? null : this.text_ITM_CD.Text),
                    //                                                                                                   ITM_GRP_CLSS_CD = (string.IsNullOrEmpty(itmGrpClssNmVo.CLSS_CD) ? null : itmGrpClssNmVo.CLSS_CD),
                    //                                                                                                   N1ST_ITM_GRP_CD = (string.IsNullOrEmpty(itmGrpClssNmVo.CLSS_CD) ? null : selVo.N1ST_ITM_GRP_CD),
                    //                                                                                                   N2ND_ITM_GRP_CD = (string.IsNullOrEmpty(itmGrpClssNmVo.CLSS_CD) ? null : selVo.N2ND_ITM_GRP_CD),
                    //                                                                                                   HD_LEN_CD = (string.IsNullOrEmpty(hdLenNmVo.CLSS_CD) ? null :  hdLenNmVo.CLSS_CD),
                    //                                                                                                   AREA_CD = SystemProperties.USER_VO.EMPE_PLC_NM,
                    //                                                                                                   SL_RLSE_DT = this.orgDao.SL_RLSE_DT
                    //                                                                                                   });
                }
            }
            catch (Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        async void combo_ITM_GRP_CLSS_NM_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.combo_ITM_GRP_CLSS_NM.Text))
            {
                this.ViewGridLeft.IsEnabled = true;
                this.text_ITM_CD.IsEnabled = true;

                SystemCodeVo itmGrpClssNmVo = this.combo_ITM_GRP_CLSS_NM.SelectedItem as SystemCodeVo;
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s22111/popupleft", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { ITM_GRP_CLSS_CD = itmGrpClssNmVo.CLSS_CD, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.ViewGridLeft.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList<SystemCodeVo>();
                    }
                    this.text_ITM_CD.SelectAll();
                    this.text_ITM_CD.Focus();
                }
            }
        }

        #region Functon (OKButton_Click, CancelButton_Click)
        private async void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                //if (saleOrderClient.S2211SelectLmtCheck(new JobVo() { CO_CD = this.orgDao.SL_CO_CD }).Equals("N"))
                //{
                //this.MSG.Text = "출고 한도 초과되었습니다";
                //return;
                //}



                //JobVo resultVo = saleOrderClient.S2211DeleteDtl(this.orgDao);
                //if (!resultVo.isSuccess)
                //{
                //    //실패
                //    WinUIMessageBox.Show(resultVo.Message, "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
                //    return;
                //}


                //JobVo resultVo = saleOrderClient.S2211TransactionInsertQuickDtl(tmpDao.ToArray());
                //if (!resultVo.isSuccess)
                //{
                //    //실패
                //    WinUIMessageBox.Show(resultVo.Message, "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
                //    return;
                //}

                int _Num = 0;
                if (tmpDao.Count == 0)
                {
                    //전체 삭제
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s22111/dtl/d", new StringContent(JsonConvert.SerializeObject(this.orgDao), System.Text.Encoding.UTF8, "application/json")))
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
                            WinUIMessageBox.Show("[요청 번호 : " + this.orgDao.SL_RLSE_NO + "] 완료 되었습니다", title, MessageBoxButton.OK, MessageBoxImage.Information);
                            this.DialogResult = true;
                            this.Close();
                        }
                    }
                }

                //건 바이 입력
                //foreach (SaleVo item in tmpDao)
                //{
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s22111/dtl/i", new StringContent(JsonConvert.SerializeObject(tmpDao), System.Text.Encoding.UTF8, "application/json")))
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
                        WinUIMessageBox.Show("[요청 번호 : " + this.orgDao.SL_RLSE_NO + "] 완료 되었습니다", title, MessageBoxButton.OK, MessageBoxImage.Information);
                        this.DialogResult = true;
                        this.Close();
                    }
                }
                //}
                //}
            }
            catch (Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
    }

//private void CancelButton_Click(object sender, RoutedEventArgs e)
//{
//    this.DialogResult = false;
//    this.Close();
//}

        private void HandleEnter(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {

                ViewGridLeft_SelectedItemChanged(sender, null);
                this.text_ITM_CD.SelectAll();
                this.text_ITM_CD.Focus();
                //this.DialogResult = false;
                //Close();
            }
        }
        #endregion


        //IsEdit
        public bool IsEdit
        {
            get
            {
                return this.IsEdit;
            }
        }

        private void GridColumn_Validate(object sender, DevExpress.Xpf.Grid.GridCellValidationEventArgs e)
        {
            //SaleVo resultVo;
            SaleVo masterDomain = (SaleVo)ViewGridRight2.GetFocusedRow();
            SaleVo checkDomain;

            try
            {
                    //using (HttpResponseMessage responseX = await SystemProperties.PROGRAM_HTTP.PostAsync("s2211/popupitems", new StringContent(JsonConvert.SerializeObject(new SaleVo() { ITM_CD = masterDomain.ITM_CD, AREA_CD = SystemProperties.USER_VO.EMPE_PLC_NM, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                    //{
                    //    if (responseX.IsSuccessStatusCode)
                    //    {
                    //        checkDomain = JsonConvert.DeserializeObject<IEnumerable<SaleVo>>(await responseX.Content.ReadAsStringAsync()).Cast<SaleVo>().ToList()[0];

              

                        bool slItmQty = (e.Column.FieldName.ToString().Equals("SL_ITM_QTY") ? true : false);
                        bool slItmRmk = (e.Column.FieldName.ToString().Equals("SL_ITM_RMK") ? true : false);

                        //bool slItmRmnQty = (e.Column.FieldName.ToString().Equals("SL_ITM_RMN_QTY") ? true : false);
                        //bool pkQty = (e.Column.FieldName.ToString().Equals("PK_QTY") ? true : false);
                        //bool mdQty = (e.Column.FieldName.ToString().Equals("MD_QTY") ? true : false);

                        if (slItmRmk)
                        {
                            if (e.IsValid)
                            {
                                if (string.IsNullOrEmpty(masterDomain.SL_ITM_RMK))
                                {
                                    masterDomain.SL_ITM_RMK = "";
                                }
                                //
                                if (!masterDomain.SL_ITM_RMK.Equals((e.Value == null ? "" : e.Value.ToString())))
                                {
                                    masterDomain.CRE_USR_ID = SystemProperties.USER;
                                    masterDomain.UPD_USR_ID = SystemProperties.USER;
                                    masterDomain.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
                                    masterDomain.SL_ITM_RMK = e.Value.ToString();
                                }
                            }
                        }
                        //else if (slItmRmnQty)
                        //{
                        //    if (e.IsValid)
                        //    {
                        //        if (string.IsNullOrEmpty(masterDomain.SL_ITM_RMN_QTY + ""))
                        //        {
                        //            masterDomain.SL_ITM_RMN_QTY = 0;
                        //        }
                        //        //
                        //        if (!masterDomain.SL_ITM_RMN_QTY.Equals((e.Value == null ? "0" : e.Value.ToString())))
                        //        {
                        //            if (int.Parse(e.Value.ToString()) > 1000000000)
                        //            {
                        //                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                        //                e.ErrorContent = "자리수 초과 되었습니다";
                        //                e.SetError(e.ErrorContent, e.ErrorType);
                        //                this.MSG.Text = e.ErrorContent.ToString();
                        //                return;
                        //            }


                        //            masterDomain.SL_ITM_RMN_QTY = int.Parse(e.Value.ToString());
                        //            //
                        //            masterDomain.CRE_USR_ID = SystemProperties.USER;
                        //            masterDomain.UPD_USR_ID = SystemProperties.USER;
                        //            masterDomain.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
                        //            masterDomain.RN = 2;
                        //            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s2211/popup/r", new StringContent(JsonConvert.SerializeObject(masterDomain), System.Text.Encoding.UTF8, "application/json")))
                        //            {
                        //                if (response.IsSuccessStatusCode)
                        //                {
                        //                    resultVo = JsonConvert.DeserializeObject<SaleVo>(await response.Content.ReadAsStringAsync());
                        //                    masterDomain.SL_ITM_QTY = resultVo.SL_ITM_QTY;
                        //                    masterDomain.SL_ITM_WGT = resultVo.SL_ITM_WGT;
                        //                }
                        //            }
                        //            //asterDomain.SL_ITM_QTY = (masterDomain.MD_PER_QTY * masterDomain.MD_QTY) + (masterDomain.PK_PER_QTY * masterDomain.PK_QTY) + masterDomain.SL_ITM_RMN_QTY;
                        //            //resultVo = saleOrderClient.S2211SelectSubDtlResult2(masterDomain);
                        //        }
                        //    }
                        //}
                        //else if (pkQty)
                        //{
                        //    if (e.IsValid)
                        //    {
                        //        if (string.IsNullOrEmpty(masterDomain.PK_QTY + ""))
                        //        {
                        //            masterDomain.PK_QTY = 0;
                        //        }
                        //        //
                        //        if (!masterDomain.PK_QTY.Equals((e.Value == null ? "0" : e.Value.ToString())))
                        //        {
                        //            if (int.Parse(e.Value.ToString()) > 1000000000)
                        //            {
                        //                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                        //                e.ErrorContent = "자리수 초과 되었습니다";
                        //                e.SetError(e.ErrorContent, e.ErrorType);
                        //                this.MSG.Text = e.ErrorContent.ToString();
                        //                return;
                        //            }

                        //            masterDomain.PK_QTY = int.Parse(e.Value.ToString());
                        //            //
                        //            masterDomain.CRE_USR_ID = SystemProperties.USER;
                        //            masterDomain.UPD_USR_ID = SystemProperties.USER;
                        //            masterDomain.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
                        //            masterDomain.RN = 2;
                        //            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s2211/popup/r", new StringContent(JsonConvert.SerializeObject(masterDomain), System.Text.Encoding.UTF8, "application/json")))
                        //            {
                        //                if (response.IsSuccessStatusCode)
                        //                {
                        //                    resultVo = JsonConvert.DeserializeObject<SaleVo>(await response.Content.ReadAsStringAsync());
                        //                    masterDomain.SL_ITM_QTY = resultVo.SL_ITM_QTY;
                        //                    masterDomain.SL_ITM_WGT = resultVo.SL_ITM_WGT;
                        //                }
                        //            }
                        //        }
                        //    }
                        //}
                        //else if (mdQty)
                        //{
                        //    if (e.IsValid)
                        //    {
                        //        if (string.IsNullOrEmpty(masterDomain.MD_QTY + ""))
                        //        {
                        //            masterDomain.MD_QTY = 0;
                        //        }
                        //        //
                        //        if (!masterDomain.MD_QTY.Equals((e.Value == null ? "0" : e.Value.ToString())))
                        //        {
                        //            if (int.Parse(e.Value.ToString()) > 1000000000)
                        //            {
                        //                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                        //                e.ErrorContent = "자리수 초과 되었습니다";
                        //                e.SetError(e.ErrorContent, e.ErrorType);
                        //                this.MSG.Text = e.ErrorContent.ToString();
                        //                return;
                        //            }

                        //            masterDomain.MD_QTY = int.Parse(e.Value.ToString());
                        //            //
                        //            masterDomain.CRE_USR_ID = SystemProperties.USER;
                        //            masterDomain.UPD_USR_ID = SystemProperties.USER;
                        //            masterDomain.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
                        //            //masterDomain.SL_ITM_QTY = (masterDomain.MD_PER_QTY * masterDomain.MD_QTY) + (masterDomain.PK_PER_QTY * masterDomain.PK_QTY) + masterDomain.SL_ITM_RMN_QTY;
                        //            masterDomain.RN = 2;
                        //            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s2211/popup/r", new StringContent(JsonConvert.SerializeObject(masterDomain), System.Text.Encoding.UTF8, "application/json")))
                        //            {
                        //                if (response.IsSuccessStatusCode)
                        //                {
                        //                    resultVo = JsonConvert.DeserializeObject<SaleVo>(await response.Content.ReadAsStringAsync());
                        //                    if (SystemProperties.USER_VO.EMPE_PLC_NM.Equals("001"))
                        //                    {
                        //                        if (Convert.ToInt32(resultVo.SL_ITM_QTY) > Convert.ToInt32(checkDomain.STK_A_QTY))
                        //                        {
                        //                            e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                        //                            e.ErrorContent = "[본사 재고 - " + Convert.ToInt32(checkDomain.STK_A_QTY) + "] 초과 하였습니다";
                        //                            e.SetError(e.ErrorContent, e.ErrorType);
                        //                            this.MSG.Text = e.ErrorContent.ToString();
                        //                            return;
                        //                        }
                        //                    }
                        //                    //else if (SystemProperties.USER_VO.EMPE_PLC_NM.Equals("002"))
                        //                    //{
                        //                    //    if (Convert.ToInt32(resultVo.SL_ITM_QTY) > Convert.ToInt32(checkDomain.STK_B_QTY))
                        //                    //    {
                        //                    //        e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                        //                    //        e.ErrorContent = "[부산 재고 - " + Convert.ToInt32(checkDomain.STK_B_QTY) + "] 초과 하였습니다";
                        //                    //        e.SetError(e.ErrorContent, e.ErrorType);
                        //                    //        this.MSG.Text = e.ErrorContent.ToString();
                        //                    //        return;
                        //                    //    }
                        //                    //}
                        //                    //else if (SystemProperties.USER_VO.EMPE_PLC_NM.Equals("003"))
                        //                    //{
                        //                    //    if (Convert.ToInt32(resultVo.SL_ITM_QTY) > Convert.ToInt32(checkDomain.STK_C_QTY))
                        //                    //    {
                        //                    //        e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                        //                    //        e.ErrorContent = "[대구 재고 - " + Convert.ToInt32(checkDomain.STK_C_QTY) + "] 초과 하였습니다";
                        //                    //        e.SetError(e.ErrorContent, e.ErrorType);
                        //                    //        this.MSG.Text = e.ErrorContent.ToString();
                        //                    //        return;
                        //                    //    }
                        //                    //}

                        //                    masterDomain.SL_ITM_QTY = resultVo.SL_ITM_QTY;
                        //                    masterDomain.SL_ITM_WGT = resultVo.SL_ITM_WGT;
                        //                }
                        //            }
                        //        }
                        //    }
                        //}

                       else if (slItmQty)
                       {
                            if (e.IsValid)
                            {
                                if (string.IsNullOrEmpty(masterDomain.SL_ITM_QTY + ""))
                                {
                                    masterDomain.SL_ITM_QTY = 0;
                                }
                        //
                            if (!masterDomain.SL_ITM_QTY.Equals((e.Value == null ? "0" : e.Value.ToString())))
                            {
                                //비동기를 쓰레드 작업 후 동기 작업 하는 방법
                                //string _checkDomainTmp = Task.Run(() => SystemProperties.PROGRAM_HTTP.PostAsync("s2211/popupitems", new StringContent(JsonConvert.SerializeObject(new SaleVo() { ITM_CD = masterDomain.ITM_CD, AREA_CD = SystemProperties.USER_VO.EMPE_PLC_NM, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json"))).Result.Content.ReadAsStringAsync().Result;

                                //checkDomain = JsonConvert.DeserializeObject<IEnumerable<SaleVo>>(_checkDomainTmp).Cast<SaleVo>().ToList()[0];
                                //if (Convert.ToInt32(e.Value.ToString()) > Convert.ToInt32(checkDomain.STK_A_QTY))
                                //{
                                //    e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                                //    e.ErrorContent = "[재고 - " + Convert.ToInt32(checkDomain.STK_A_QTY) + "] 초과 하였습니다";
                                //    e.SetError(e.ErrorContent, e.ErrorType);
                                //    this.MSG.Text = e.ErrorContent.ToString();
                                //    return;
                                //}
                                //        

                                //using (HttpResponseMessage responseX =  SystemProperties.PROGRAM_HTTP.PostAsync("s2211/popupitems", new StringContent(JsonConvert.SerializeObject(new SaleVo() { ITM_CD = masterDomain.ITM_CD, AREA_CD = SystemProperties.USER_VO.EMPE_PLC_NM, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")).ConfigureAwait(false))
                                //{
                                //    if (responseX.IsSuccessStatusCode)
                                //    {
                                //        checkDomain = JsonConvert.DeserializeObject<IEnumerable<SaleVo>>(responseX.Content.ReadAsStringAsync().Result).Cast<SaleVo>().ToList()[0];
                                //        //
                                //        if (Convert.ToInt32(e.Value.ToString()) > Convert.ToInt32(checkDomain.STK_A_QTY))
                                //        {
                                //        //    Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                                //        //    {
                                //                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                                //                e.ErrorContent = "[재고 - " + Convert.ToInt32(checkDomain.STK_A_QTY) + "] 초과 하였습니다";
                                //                e.SetError(e.ErrorContent, e.ErrorType);
                                //                this.MSG.Text = e.ErrorContent.ToString();
                                //                return;
                                //        //    }));
                                //        // }
                                //    }
                                //}
                                //
                                //if (int.Parse(e.Value.ToString()) > 1000000000)
                                //{
                                //    e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                                //    e.ErrorContent = "자리수 초과 되었습니다";
                                //    e.SetError(e.ErrorContent, e.ErrorType);
                                //    this.MSG.Text = e.ErrorContent.ToString();
                                //    return;
                                //}

                                masterDomain.SL_ITM_QTY = int.Parse(e.Value.ToString());
                                    //
                                    masterDomain.CRE_USR_ID = SystemProperties.USER;
                                    masterDomain.UPD_USR_ID = SystemProperties.USER;
                                    masterDomain.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;

                                    //resultVo = saleOrderClient.S2211SelectSubDtlResult(masterDomain);
                                    //masterDomain.RN = 1;
                                    //using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s2211/popup/r", new StringContent(JsonConvert.SerializeObject(masterDomain), System.Text.Encoding.UTF8, "application/json")))
                                    //{
                                    //if (response.IsSuccessStatusCode)
                                    //{
                                    //  resultVo = JsonConvert.DeserializeObject<SaleVo>(await response.Content.ReadAsStringAsync());
                                    //masterDomain.MD_QTY = resultVo.MD_QTY;
                                    //masterDomain.PK_QTY = resultVo.PK_QTY;
                                    //masterDomain.SL_ITM_RMN_QTY = resultVo.SL_ITM_RMN_QTY;
                                    //masterDomain.SL_ITM_WGT = resultVo.SL_ITM_WGT;
                                    //}
                                    //}
                                    //}
                                    //}
                                }
                            }
                        }
                //    }
                //}
            }
            catch (Exception eLog)
            {
                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                e.ErrorContent = eLog.Message;
                e.SetError(e.ErrorContent, e.ErrorType);
                this.MSG.Text = e.ErrorContent.ToString();
                return;
            }
        }

        ////비동기를 동기로 작업 하는 방법
        //public HttpResponseMessage getItems(string _ITM_CD)
        //{
        //    return Task.Run(() => SystemProperties.PROGRAM_HTTP.PostAsync("s2211/popupitems", new StringContent(JsonConvert.SerializeObject(new SaleVo() { ITM_CD = _ITM_CD, AREA_CD = SystemProperties.USER_VO.EMPE_PLC_NM, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json"))).Result;
        //}

        private void ViewGridRight2_HiddenEditor(object sender, DevExpress.Xpf.Grid.EditorEventArgs e)
        {
            this.ViewTableRight2.CommitEditing();

            //bool slItmQty = (e.Column.FieldName.ToString().Equals("SL_ITM_QTY") ? true : false);
            //
            //bool slItmRmk = (e.Column.FieldName.ToString().Equals("SL_ITM_RMK") ? true : false);
            //bool slItmRmnQty = (e.Column.FieldName.ToString().Equals("SL_ITM_RMN_QTY") ? true : false);
            //bool pkQty = (e.Column.FieldName.ToString().Equals("PK_QTY") ? true : false);
            //bool mdQty = (e.Column.FieldName.ToString().Equals("MD_QTY") ? true : false);
            //int rowHandle = this.ViewTableRight2.FocusedRowHandle;

            //if (slItmRmk)
            //{
            //    rowHandle = rowHandle + 1;
            //    this.ViewGridRight2.CurrentColumn = this.ViewGridRight2.Columns["MD_QTY"];
            //}
            //else if (slItmRmnQty)
            //{
            //    this.ViewGridRight2.CurrentColumn = this.ViewGridRight2.Columns["SL_ITM_RMK"];
            //}
            //else if (pkQty)
            //{
            //    this.ViewGridRight2.CurrentColumn = this.ViewGridRight2.Columns["SL_ITM_RMN_QTY"];
            //}
            //else if (mdQty)
            //{
            //    this.ViewGridRight2.CurrentColumn = this.ViewGridRight2.Columns["PK_QTY"];
            //}
            //if (slItmQty)
            //{
            //    this.ViewGridRight2.CurrentColumn = this.ViewGridRight2.Columns["MD_QTY"];
            //}
            //else
                //this.ViewGridRight2.RefreshRow(rowHandle - 1);
            //this.ViewTableRight2.FocusedRowHandle = rowHandle;
        }


        //void ViewGridRight2_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        //{
        //    try
        //    {
        //        JobVo masterDomain = (JobVo)ViewGridRight2.GetFocusedRow();
        //        if (masterDomain == null)
        //        {
        //            return;
        //        }

        //        MessageBoxResult result = WinUIMessageBox.Show("[" + masterDomain.SL_ITM_CD + "] 정말로 삭제 하시겠습니까?", "[삭제]" + title, MessageBoxButton.YesNo, MessageBoxImage.Question);
        //        if (result == MessageBoxResult.Yes)
        //        {
                    

        //            JobVo resultVo = saleOrderClient.S2211DeleteDtl(masterDomain);
        //            if (!resultVo.isSuccess)
        //            {
        //                //실패
        //                WinUIMessageBox.Show(resultVo.Message, "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
        //                return;
        //            }
        //            this.ViewGridRight2.ItemsSource = new ObservableCollection<JobVo>(saleOrderClient.S2211SelectSubDtlList(this.orgDao));
        //        }
        //    }
        //    catch (Exception eLog)
        //    {
        //        WinUIMessageBox.Show(eLog.Message, "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
        //        return;
        //    }
        //}

        //private void ButtonInfo_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //          MessageBoxResult result = WinUIMessageBox.Show("정말로 삭제 하시겠습니까?", "[삭제]" + title, MessageBoxButton.YesNo, MessageBoxImage.Question);
        //          if (result == MessageBoxResult.Yes)
        //          {
        //              JobVo masterDomain = (JobVo)ViewGridRight2.GetFocusedRow();
        //              if (masterDomain == null)
        //              {
        //                  return;
        //              }

        //              JobVo resultVo = saleOrderClient.S2211DeleteDtl(masterDomain);
        //              if (!resultVo.isSuccess)
        //              {
        //                  //실패
        //                  WinUIMessageBox.Show(resultVo.Message, "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
        //                  return;
        //              }
        //              this.ViewGridRight2.ItemsSource = new ObservableCollection<JobVo>(saleOrderClient.S2211SelectSubDtlList(this.orgDao));

        //              //ObservableCollection<JobVo> tmpList = new ObservableCollection<JobVo>((ObservableCollection<JobVo>)this.ViewGridRight2.ItemsSource);
        //              //for (int x = 0; x < tmpList.Count; x++)
        //              //{
        //              //    if (tmpList[x].SL_ITM_CD.Equals(masterDomain.SL_ITM_CD))
        //              //    {
        //              //        this.ViewTableRight2.DeleteRow(x);
        //              //        break;
        //              //    }
        //              //}

        //              //순번
        //              //tmpList = new ObservableCollection<JobVo>((ObservableCollection<JobVo>)this.ViewGridRight2.ItemsSource);
        //              //for (int x = 0; x < tmpList.Count; x++)
        //              //{
        //              //    tmpList[x].RN = x + 1;
        //              //}
        //          }
        //    }
        //    catch (Exception eLog)
        //    {
        //        WinUIMessageBox.Show(eLog.Message, "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
        //        return;
        //    }
        //}



        private void btn_del_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //DevExpress.Xpf.Editors.ButtonInfo bt  = (DevExpress.Xpf.Editors.ButtonInfo)sender;

                // //Button bt = (Button)sender;

                // SaleVo selVo = ((SaleVo)(((EditGridCellData)bt.DataContext).RowData.Row));
                // this.ViewTableRight2.CommitEditing();

                SaleVo selVo = (SaleVo)ViewGridRight2.GetFocusedRow();
                if (selVo == null)
                {
                    return;
                }

                MessageBoxResult result = WinUIMessageBox.Show("[" + selVo.SL_ITM_CD + "] 정말로 삭제 하시겠습니까?", title, MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    //JobVo resultVo = saleOrderClient.S2211DeleteDtl(selVo);
                    //if (!resultVo.isSuccess)
                    //{
                    //    //실패
                    //    WinUIMessageBox.Show(resultVo.Message, "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
                    //    return;
                    //}
                    this.tmpDao.Remove(selVo);
                    this.ViewGridRight2.ItemsSource = this.tmpDao;
                    this.ViewGridRight2.RefreshData();
                }
            }
            catch (Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }



        public async void SYSTEM_CODE_VO()
        {
            //IList<CodeDao> tmpItmGrpClss = SystemProperties.SYSTEM_CODE_VO("L-001");
            ////tmpItmGrpClss.Insert(0, new CodeDao() { CLSS_DESC = "전체" });
            //this.combo_ITM_GRP_CLSS_NM.ItemsSource = tmpItmGrpClss;
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "L-001"))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.combo_ITM_GRP_CLSS_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                    this.combo_ITM_GRP_CLSS_NM.SelectedIndex = 4;
                }
            }

            //IList<CodeDao> tmphdLenBm = SystemProperties.SYSTEM_CODE_VO("C-002");
            //tmphdLenBm.Insert(0, new CodeDao() { CLSS_DESC = "전체" });
            //this.combo_HD_LEN_NM.ItemsSource = tmphdLenBm;
            //using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "C-002"))
            //{
            //    if (response.IsSuccessStatusCode)
            //    {
            //        this.combo_HD_LEN_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
            //        this.combo_HD_LEN_NM.SelectedIndex = 0;
            //    }
            //}

            //Refresh() - this.ViewGridRight2
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s22111/popup", new StringContent(JsonConvert.SerializeObject(this.orgDao), System.Text.Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    tmpDao = JsonConvert.DeserializeObject<IEnumerable<SaleVo>>(await response.Content.ReadAsStringAsync()).Cast<SaleVo>().ToList();
                    this.ViewGridRight2.ItemsSource = tmpDao;
                }

            }
        }
    }
}
