using System;
using System.Globalization;
using System.Net.Http;
using System.Windows.Controls;
using AquilaErpWpfApp3.Util;
using AquilaErpWpfApp3.ViewModel;
using ModelsLibrary.Man;
using Newtonsoft.Json;

namespace AquilaErpWpfApp3.View.M
{
    public partial class M6730 : UserControl
    {
        private string title = "조립작업실적입력";
        public M6730()
        {
            DataContext = new M6730ViewModel();
            //
            InitializeComponent();
            //
            //this.txt_Master_Search.KeyDown += new KeyEventHandler(txt_Master_Search_KeyDown);
            //this.btn_ConfigViewPage_Master_search.Click += new RoutedEventHandler(btn_Master_search_Click);
            //
        }
        #region Functon (Master Search)
        //private void M_REFRESH_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        //{
        //    try
        //    {
        //        this.ConfigViewPage1Edit_Master.ShowLoadingPanel = true;
        //        this.configViewPage1EditView_Master.SearchString = (this.M_SEARCH_TEXT.EditValue == null ? "" : this.M_SEARCH_TEXT.EditValue.ToString());
        //        //this.txt_Search.SelectAll();
        //        this.M_SEARCH_TEXT.Focus();
        //        this.ConfigViewPage1Edit_Master.ShowLoadingPanel = false;

        //        //((S131ViewModel)this.DataContext).setTitle();
        //    }
        //    catch (Exception eLog)
        //    {
        //        this.ConfigViewPage1Edit_Master.ShowLoadingPanel = false;
        //        WinUIMessageBox.Show(eLog.Message, "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
        //        //this.M_SEARCH_TEXT.SelectAll();
        //        this.M_SEARCH_TEXT.Focus();
        //        return;
        //    }
        //}

        //private void M_SEARCH_TEXT_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        //{
        //    if (e.Key == Key.Enter)
        //    {
        //        M_REFRESH_ItemClick(sender, null);
        //    }
        //}
        //void txt_Master_Search_KeyDown(Object sender, KeyEventArgs e)
        //{
        //    if (e.Key == Key.Enter)
        //    {
        //        Master_Search(this.txt_Master_Search.Text, true);
        //    }
        //}

        //void btn_Master_search_Click(object sender, RoutedEventArgs e)
        //{
        //    Master_Search(this.txt_Master_Search.Text, true);
        //}

        //void Master_Search(string scarch, bool isSearch)
        //{
        //    try
        //    {
        //        this.ConfigViewPage1Edit_Master.ShowLoadingPanel = true;
        //        if (isSearch)
        //        {
        //            this.configViewPage1EditView_Master.SearchString = scarch;
        //            this.txt_Master_Search.SelectAll();
        //            this.txt_Master_Search.Focus();
        //        }
        //        this.ConfigViewPage1Edit_Master.ShowLoadingPanel = false;
        //    }
        //    catch (Exception eLog)
        //    {
        //        this.ConfigViewPage1Edit_Master.ShowLoadingPanel = false;
        //        WinUIMessageBox.Show(eLog.Message, "[에러]표준 공정 관리", MessageBoxButton.OK, MessageBoxImage.Error);
        //        this.txt_Master_Search.SelectAll();
        //        this.txt_Master_Search.Focus();
        //        return;
        //    }
        //}
        #endregion



        private async void GridColumn_Validate(object sender, DevExpress.Xpf.Grid.GridCellValidationEventArgs e)
        {
            #region MyRegion
            //try
            //{
            //    ManVo masterDomain = (ManVo)ConfigViewPage1Edit_Master.GetFocusedRow();
            //    bool n1stPlnQty = (e.Column.FieldName.ToString().Equals("N1ST_PLN_QTY") ? true : false);
            //    bool n2ndPlnQty = (e.Column.FieldName.ToString().Equals("N2ND_PLN_QTY") ? true : false);
            //    bool n3rdPlnQty = (e.Column.FieldName.ToString().Equals("N3RD_PLN_QTY") ? true : false);
            //    bool n4thPlnQty = (e.Column.FieldName.ToString().Equals("N4TH_PLN_QTY") ? true : false);
            //    bool n5thPlnQty = (e.Column.FieldName.ToString().Equals("N5TH_PLN_QTY") ? true : false);
            //    bool n6thPlnQty = (e.Column.FieldName.ToString().Equals("N6TH_PLN_QTY") ? true : false);
            //    bool n7thPlnQty = (e.Column.FieldName.ToString().Equals("N7TH_PLN_QTY") ? true : false);

            //    if (n1stPlnQty)
            //    {
            //        if (e.IsValid)
            //        {
            //            if (string.IsNullOrEmpty(masterDomain.N1ST_PLN_QTY + ""))
            //            {
            //                masterDomain.N1ST_PLN_QTY = 0;
            //            }
            //            //
            //            if (!masterDomain.N1ST_PLN_QTY.Equals((e.Value == null ? "" : e.Value.ToString())))
            //            {
            //                //double? tmpInt = System.Convert.ToDouble(e.Value.ToString());
            //                //if (tmpInt <= -1)
            //                //{
            //                //    e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
            //                //    e.ErrorContent = "[단가]" + tmpInt + " -값은 입력 하실수 없습니다";
            //                //    e.SetError(e.ErrorContent, e.ErrorType);
            //                //    return;
            //                //}
            //                //masterDomain.CO_UT_PRC = tmpInt;
            //                //masterDomain.isCheckd = true;
            //                //에러 체크
            //                try
            //                {
            //                    masterDomain.N1ST_PLN_QTY = e.Value;
            //                    masterDomain.PROD_QTY = e.Value;
            //                    masterDomain.UPD_USR_ID = SystemProperties.USER_VO.USR_ID;
            //                    masterDomain.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
            //                    masterDomain.RN = 1;
            //                    //합계
            //                    masterDomain.PLN_SUM_QTY = Convert.ToInt32(masterDomain.N1ST_PLN_QTY) + Convert.ToInt32(masterDomain.N2ND_PLN_QTY) + Convert.ToInt32(masterDomain.N3RD_PLN_QTY) + Convert.ToInt32(masterDomain.N4TH_PLN_QTY) + Convert.ToInt32(masterDomain.N5TH_PLN_QTY) + Convert.ToInt32(masterDomain.N6TH_PLN_QTY) + Convert.ToInt32(masterDomain.N7TH_PLN_QTY);

            //                    //
            //                    int _Num = 0;
            //                    string resultMsg = "";
            //                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m665101/u", new StringContent(JsonConvert.SerializeObject(masterDomain), System.Text.Encoding.UTF8, "application/json")))
            //                    {
            //                        if (response.IsSuccessStatusCode)
            //                        {
            //                            resultMsg = await response.Content.ReadAsStringAsync();
            //                            if (int.TryParse(resultMsg, out _Num) == false)
            //                            {
            //                                //실패
            //                                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
            //                                e.ErrorContent = resultMsg;
            //                                e.SetError(e.ErrorContent, e.ErrorType);
            //                                return;
            //                            }

            //                            this.ConfigViewPage1Edit_Master.RefreshData();
            //                        }
            //                    }
            //                }
            //                catch (System.Exception eLog)
            //                {
            //                    //masterDomain.LSS_VAL = 0;
            //                    e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
            //                    e.ErrorContent = eLog.Message;
            //                    e.SetError(e.ErrorContent, e.ErrorType);
            //                    return;
            //                }
            //            }
            //        }
            //    }
            //    else if (n2ndPlnQty)
            //    {
            //        if (e.IsValid)
            //        {
            //            if (string.IsNullOrEmpty(masterDomain.N2ND_PLN_QTY + ""))
            //            {
            //                masterDomain.N2ND_PLN_QTY = 0;
            //            }
            //            //
            //            if (!masterDomain.N2ND_PLN_QTY.Equals((e.Value == null ? "" : e.Value.ToString())))
            //            {
            //                //double? tmpInt = System.Convert.ToDouble(e.Value.ToString());
            //                //if (tmpInt <= -1)
            //                //{
            //                //    e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
            //                //    e.ErrorContent = "[단가]" + tmpInt + " -값은 입력 하실수 없습니다";
            //                //    e.SetError(e.ErrorContent, e.ErrorType);
            //                //    return;
            //                //}
            //                //masterDomain.CO_UT_PRC = tmpInt;
            //                //masterDomain.isCheckd = true;
            //                //에러 체크
            //                try
            //                {
            //                    masterDomain.N2ND_PLN_QTY = e.Value;
            //                    masterDomain.PROD_QTY = e.Value;
            //                    masterDomain.UPD_USR_ID = SystemProperties.USER_VO.USR_ID;
            //                    masterDomain.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
            //                    masterDomain.RN = 2;
            //                    //합계
            //                    masterDomain.PLN_SUM_QTY = Convert.ToInt32(masterDomain.N1ST_PLN_QTY) + Convert.ToInt32(masterDomain.N2ND_PLN_QTY) + Convert.ToInt32(masterDomain.N3RD_PLN_QTY) + Convert.ToInt32(masterDomain.N4TH_PLN_QTY) + Convert.ToInt32(masterDomain.N5TH_PLN_QTY) + Convert.ToInt32(masterDomain.N6TH_PLN_QTY) + Convert.ToInt32(masterDomain.N7TH_PLN_QTY);

            //                    //
            //                    int _Num = 0;
            //                    string resultMsg = "";
            //                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m665101/u", new StringContent(JsonConvert.SerializeObject(masterDomain), System.Text.Encoding.UTF8, "application/json")))
            //                    {
            //                        if (response.IsSuccessStatusCode)
            //                        {
            //                            resultMsg = await response.Content.ReadAsStringAsync();
            //                            if (int.TryParse(resultMsg, out _Num) == false)
            //                            {
            //                                //실패
            //                                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
            //                                e.ErrorContent = resultMsg;
            //                                e.SetError(e.ErrorContent, e.ErrorType);
            //                                return;
            //                            }

            //                            this.ConfigViewPage1Edit_Master.RefreshData();
            //                        }
            //                    }
            //                }
            //                catch (System.Exception eLog)
            //                {
            //                    //masterDomain.LSS_VAL = 0;
            //                    e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
            //                    e.ErrorContent = eLog.Message;
            //                    e.SetError(e.ErrorContent, e.ErrorType);
            //                    return;
            //                }
            //            }
            //        }
            //    }
            //    else if (n3rdPlnQty)
            //    {
            //        if (e.IsValid)
            //        {
            //            if (string.IsNullOrEmpty(masterDomain.N3RD_PLN_QTY + ""))
            //            {
            //                masterDomain.N3RD_PLN_QTY = 0;
            //            }
            //            //
            //            if (!masterDomain.N3RD_PLN_QTY.Equals((e.Value == null ? "" : e.Value.ToString())))
            //            {
            //                //double? tmpInt = System.Convert.ToDouble(e.Value.ToString());
            //                //if (tmpInt <= -1)
            //                //{
            //                //    e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
            //                //    e.ErrorContent = "[단가]" + tmpInt + " -값은 입력 하실수 없습니다";
            //                //    e.SetError(e.ErrorContent, e.ErrorType);
            //                //    return;
            //                //}
            //                //masterDomain.CO_UT_PRC = tmpInt;
            //                //masterDomain.isCheckd = true;
            //                //에러 체크
            //                try
            //                {
            //                    masterDomain.N3RD_PLN_QTY = e.Value;
            //                    masterDomain.PROD_QTY = e.Value;
            //                    masterDomain.UPD_USR_ID = SystemProperties.USER_VO.USR_ID;
            //                    masterDomain.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
            //                    masterDomain.RN = 3;
            //                    //합계
            //                    masterDomain.PLN_SUM_QTY = Convert.ToInt32(masterDomain.N1ST_PLN_QTY) + Convert.ToInt32(masterDomain.N2ND_PLN_QTY) + Convert.ToInt32(masterDomain.N3RD_PLN_QTY) + Convert.ToInt32(masterDomain.N4TH_PLN_QTY) + Convert.ToInt32(masterDomain.N5TH_PLN_QTY) + Convert.ToInt32(masterDomain.N6TH_PLN_QTY) + Convert.ToInt32(masterDomain.N7TH_PLN_QTY);

            //                    //
            //                    int _Num = 0;
            //                    string resultMsg = "";
            //                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m665101/u", new StringContent(JsonConvert.SerializeObject(masterDomain), System.Text.Encoding.UTF8, "application/json")))
            //                    {
            //                        if (response.IsSuccessStatusCode)
            //                        {
            //                            resultMsg = await response.Content.ReadAsStringAsync();
            //                            if (int.TryParse(resultMsg, out _Num) == false)
            //                            {
            //                                //실패
            //                                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
            //                                e.ErrorContent = resultMsg;
            //                                e.SetError(e.ErrorContent, e.ErrorType);
            //                                return;
            //                            }

            //                            this.ConfigViewPage1Edit_Master.RefreshData();
            //                        }
            //                    }
            //                }
            //                catch (System.Exception eLog)
            //                {
            //                    //masterDomain.LSS_VAL = 0;
            //                    e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
            //                    e.ErrorContent = eLog.Message;
            //                    e.SetError(e.ErrorContent, e.ErrorType);
            //                    return;
            //                }
            //            }
            //        }
            //    }
            //    else if (n4thPlnQty)
            //    {
            //        if (e.IsValid)
            //        {
            //            if (string.IsNullOrEmpty(masterDomain.N4TH_PLN_QTY + ""))
            //            {
            //                masterDomain.N4TH_PLN_QTY = 0;
            //            }
            //            //
            //            if (!masterDomain.N4TH_PLN_QTY.Equals((e.Value == null ? "" : e.Value.ToString())))
            //            {
            //                //double? tmpInt = System.Convert.ToDouble(e.Value.ToString());
            //                //if (tmpInt <= -1)
            //                //{
            //                //    e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
            //                //    e.ErrorContent = "[단가]" + tmpInt + " -값은 입력 하실수 없습니다";
            //                //    e.SetError(e.ErrorContent, e.ErrorType);
            //                //    return;
            //                //}
            //                //masterDomain.CO_UT_PRC = tmpInt;
            //                //masterDomain.isCheckd = true;
            //                //에러 체크
            //                try
            //                {
            //                    masterDomain.N4TH_PLN_QTY = e.Value;
            //                    masterDomain.PROD_QTY = e.Value;
            //                    masterDomain.UPD_USR_ID = SystemProperties.USER_VO.USR_ID;
            //                    masterDomain.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
            //                    masterDomain.RN = 4;
            //                    //합계
            //                    masterDomain.PLN_SUM_QTY = Convert.ToInt32(masterDomain.N1ST_PLN_QTY) + Convert.ToInt32(masterDomain.N2ND_PLN_QTY) + Convert.ToInt32(masterDomain.N3RD_PLN_QTY) + Convert.ToInt32(masterDomain.N4TH_PLN_QTY) + Convert.ToInt32(masterDomain.N5TH_PLN_QTY) + Convert.ToInt32(masterDomain.N6TH_PLN_QTY) + Convert.ToInt32(masterDomain.N7TH_PLN_QTY);

            //                    //
            //                    int _Num = 0;
            //                    string resultMsg = "";
            //                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m665101/u", new StringContent(JsonConvert.SerializeObject(masterDomain), System.Text.Encoding.UTF8, "application/json")))
            //                    {
            //                        if (response.IsSuccessStatusCode)
            //                        {
            //                            resultMsg = await response.Content.ReadAsStringAsync();
            //                            if (int.TryParse(resultMsg, out _Num) == false)
            //                            {
            //                                //실패
            //                                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
            //                                e.ErrorContent = resultMsg;
            //                                e.SetError(e.ErrorContent, e.ErrorType);
            //                                return;
            //                            }

            //                            this.ConfigViewPage1Edit_Master.RefreshData();
            //                        }
            //                    }
            //                }
            //                catch (System.Exception eLog)
            //                {
            //                    //masterDomain.LSS_VAL = 0;
            //                    e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
            //                    e.ErrorContent = eLog.Message;
            //                    e.SetError(e.ErrorContent, e.ErrorType);
            //                    return;
            //                }
            //            }
            //        }
            //    }
            //    else if (n5thPlnQty)
            //    {
            //        if (e.IsValid)
            //        {
            //            if (string.IsNullOrEmpty(masterDomain.N5TH_PLN_QTY + ""))
            //            {
            //                masterDomain.N5TH_PLN_QTY = 0;
            //            }
            //            //
            //            if (!masterDomain.N5TH_PLN_QTY.Equals((e.Value == null ? "" : e.Value.ToString())))
            //            {
            //                //double? tmpInt = System.Convert.ToDouble(e.Value.ToString());
            //                //if (tmpInt <= -1)
            //                //{
            //                //    e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
            //                //    e.ErrorContent = "[단가]" + tmpInt + " -값은 입력 하실수 없습니다";
            //                //    e.SetError(e.ErrorContent, e.ErrorType);
            //                //    return;
            //                //}
            //                //masterDomain.CO_UT_PRC = tmpInt;
            //                //masterDomain.isCheckd = true;
            //                //에러 체크
            //                try
            //                {
            //                    masterDomain.N5TH_PLN_QTY = e.Value;
            //                    masterDomain.PROD_QTY = e.Value;
            //                    masterDomain.UPD_USR_ID = SystemProperties.USER_VO.USR_ID;
            //                    masterDomain.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
            //                    masterDomain.RN = 5;
            //                    //합계
            //                    masterDomain.PLN_SUM_QTY = Convert.ToInt32(masterDomain.N1ST_PLN_QTY) + Convert.ToInt32(masterDomain.N2ND_PLN_QTY) + Convert.ToInt32(masterDomain.N3RD_PLN_QTY) + Convert.ToInt32(masterDomain.N4TH_PLN_QTY) + Convert.ToInt32(masterDomain.N5TH_PLN_QTY) + Convert.ToInt32(masterDomain.N6TH_PLN_QTY) + Convert.ToInt32(masterDomain.N7TH_PLN_QTY);

            //                    //
            //                    int _Num = 0;
            //                    string resultMsg = "";
            //                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m665101/u", new StringContent(JsonConvert.SerializeObject(masterDomain), System.Text.Encoding.UTF8, "application/json")))
            //                    {
            //                        if (response.IsSuccessStatusCode)
            //                        {
            //                            resultMsg = await response.Content.ReadAsStringAsync();
            //                            if (int.TryParse(resultMsg, out _Num) == false)
            //                            {
            //                                //실패
            //                                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
            //                                e.ErrorContent = resultMsg;
            //                                e.SetError(e.ErrorContent, e.ErrorType);
            //                                return;
            //                            }

            //                            this.ConfigViewPage1Edit_Master.RefreshData();
            //                        }
            //                    }
            //                }
            //                catch (System.Exception eLog)
            //                {
            //                    //masterDomain.LSS_VAL = 0;
            //                    e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
            //                    e.ErrorContent = eLog.Message;
            //                    e.SetError(e.ErrorContent, e.ErrorType);
            //                    return;
            //                }
            //            }
            //        }
            //    }
            //    else if (n6thPlnQty)
            //    {
            //        if (e.IsValid)
            //        {
            //            if (string.IsNullOrEmpty(masterDomain.N6TH_PLN_QTY + ""))
            //            {
            //                masterDomain.N6TH_PLN_QTY = 0;
            //            }
            //            //
            //            if (!masterDomain.N6TH_PLN_QTY.Equals((e.Value == null ? "" : e.Value.ToString())))
            //            {
            //                //double? tmpInt = System.Convert.ToDouble(e.Value.ToString());
            //                //if (tmpInt <= -1)
            //                //{
            //                //    e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
            //                //    e.ErrorContent = "[단가]" + tmpInt + " -값은 입력 하실수 없습니다";
            //                //    e.SetError(e.ErrorContent, e.ErrorType);
            //                //    return;
            //                //}
            //                //masterDomain.CO_UT_PRC = tmpInt;
            //                //masterDomain.isCheckd = true;
            //                //에러 체크
            //                try
            //                {
            //                    masterDomain.N6TH_PLN_QTY = e.Value;
            //                    masterDomain.PROD_QTY = e.Value;
            //                    masterDomain.UPD_USR_ID = SystemProperties.USER_VO.USR_ID;
            //                    masterDomain.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
            //                    masterDomain.RN = 6;
            //                    //합계
            //                    masterDomain.PLN_SUM_QTY = Convert.ToInt32(masterDomain.N1ST_PLN_QTY) + Convert.ToInt32(masterDomain.N2ND_PLN_QTY) + Convert.ToInt32(masterDomain.N3RD_PLN_QTY) + Convert.ToInt32(masterDomain.N4TH_PLN_QTY) + Convert.ToInt32(masterDomain.N5TH_PLN_QTY) + Convert.ToInt32(masterDomain.N6TH_PLN_QTY) + Convert.ToInt32(masterDomain.N7TH_PLN_QTY);

            //                    //
            //                    int _Num = 0;
            //                    string resultMsg = "";
            //                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m665101/u", new StringContent(JsonConvert.SerializeObject(masterDomain), System.Text.Encoding.UTF8, "application/json")))
            //                    {
            //                        if (response.IsSuccessStatusCode)
            //                        {
            //                            resultMsg = await response.Content.ReadAsStringAsync();
            //                            if (int.TryParse(resultMsg, out _Num) == false)
            //                            {
            //                                //실패
            //                                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
            //                                e.ErrorContent = resultMsg;
            //                                e.SetError(e.ErrorContent, e.ErrorType);
            //                                return;
            //                            }

            //                            this.ConfigViewPage1Edit_Master.RefreshData();
            //                        }
            //                    }
            //                }
            //                catch (System.Exception eLog)
            //                {
            //                    //masterDomain.LSS_VAL = 0;
            //                    e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
            //                    e.ErrorContent = eLog.Message;
            //                    e.SetError(e.ErrorContent, e.ErrorType);
            //                    return;
            //                }
            //            }
            //        }
            //    }
            //    else if (n1stPlnQty)
            //    {
            //        if (e.IsValid)
            //        {
            //            if (string.IsNullOrEmpty(masterDomain.N7TH_PLN_QTY + ""))
            //            {
            //                masterDomain.N7TH_PLN_QTY = 0;
            //            }
            //            //
            //            if (!masterDomain.N7TH_PLN_QTY.Equals((e.Value == null ? "" : e.Value.ToString())))
            //            {
            //                //double? tmpInt = System.Convert.ToDouble(e.Value.ToString());
            //                //if (tmpInt <= -1)
            //                //{
            //                //    e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
            //                //    e.ErrorContent = "[단가]" + tmpInt + " -값은 입력 하실수 없습니다";
            //                //    e.SetError(e.ErrorContent, e.ErrorType);
            //                //    return;
            //                //}
            //                //masterDomain.CO_UT_PRC = tmpInt;
            //                //masterDomain.isCheckd = true;
            //                //에러 체크
            //                try
            //                {
            //                    masterDomain.N7TH_PLN_QTY = e.Value;
            //                    masterDomain.PROD_QTY = e.Value;
            //                    masterDomain.UPD_USR_ID = SystemProperties.USER_VO.USR_ID;
            //                    masterDomain.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
            //                    masterDomain.RN = 7;
            //                    //합계
            //                    masterDomain.PLN_SUM_QTY = Convert.ToInt32(masterDomain.N1ST_PLN_QTY) + Convert.ToInt32(masterDomain.N2ND_PLN_QTY) + Convert.ToInt32(masterDomain.N3RD_PLN_QTY) + Convert.ToInt32(masterDomain.N4TH_PLN_QTY) + Convert.ToInt32(masterDomain.N5TH_PLN_QTY) + Convert.ToInt32(masterDomain.N6TH_PLN_QTY) + Convert.ToInt32(masterDomain.N7TH_PLN_QTY);

            //                    //
            //                    int _Num = 0;
            //                    string resultMsg = "";
            //                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m665101/u", new StringContent(JsonConvert.SerializeObject(masterDomain), System.Text.Encoding.UTF8, "application/json")))
            //                    {
            //                        if (response.IsSuccessStatusCode)
            //                        {
            //                            resultMsg = await response.Content.ReadAsStringAsync();
            //                            if (int.TryParse(resultMsg, out _Num) == false)
            //                            {
            //                                //실패
            //                                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
            //                                e.ErrorContent = resultMsg;
            //                                e.SetError(e.ErrorContent, e.ErrorType);
            //                                return;
            //                            }

            //                            this.ConfigViewPage1Edit_Master.RefreshData();
            //                        }
            //                    }
            //                }
            //                catch (System.Exception eLog)
            //                {
            //                    //masterDomain.LSS_VAL = 0;
            //                    e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
            //                    e.ErrorContent = eLog.Message;
            //                    e.SetError(e.ErrorContent, e.ErrorType);
            //                    return;
            //                }
            //            }
            //        }
            //    }
            //}
            //catch (System.Exception eLog)
            //{
            //    e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
            //    e.ErrorContent = eLog.Message;
            //    e.SetError(e.ErrorContent, e.ErrorType);
            //    return;
            //} 
            #endregion

            try
            {
                ManVo masterDomain = (ManVo)ConfigViewPage1Edit_Master.GetFocusedRow();

                bool slItmQty = (e.Column.FieldName.ToString().Equals("SL_ITM_QTY") ? true : false);
                bool inpStaffVal = (e.Column.FieldName.ToString().Equals("INP_STAFF_VAL") ? true : false);
                bool wrkStDt = (e.Column.FieldName.ToString().Equals("WRK_ST_DT") ? true : false);
                bool wrkEndDt = (e.Column.FieldName.ToString().Equals("WRK_END_DT") ? true : false);
                bool moldCd = (e.Column.FieldName.ToString().Equals("GBN") ? true : false);
                bool dyNgtFlg = (e.Column.FieldName.ToString().Equals("DY_NGT_FLG") ? true : false);

                bool updDt = (e.Column.FieldName.ToString().Equals("UPD_DT") ? true : false);


                bool torVal = (e.Column.FieldName.ToString().Equals("TOR_VAL") ? true : false);


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
                            //this.OKButton.IsEnabled = true;

                            try
                            {
                                
                                masterDomain.UPD_USR_ID = SystemProperties.USER_VO.USR_ID;
                                masterDomain.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
                        
                                //
                                int _Num = 0;
                                string resultMsg = "";
                                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6710/mst/u", new StringContent(JsonConvert.SerializeObject(masterDomain), System.Text.Encoding.UTF8, "application/json")))
                                {
                                    if (response.IsSuccessStatusCode)
                                    {
                                        resultMsg = await response.Content.ReadAsStringAsync();
                                        if (int.TryParse(resultMsg, out _Num) == false)
                                        {
                                            //실패
                                            e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                                            e.ErrorContent = resultMsg;
                                            e.SetError(e.ErrorContent, e.ErrorType);
                                            return;
                                        }

                                        this.ConfigViewPage1Edit_Master.RefreshData();
                                    }
                                }
                            }
                            catch (System.Exception eLog)
                            {
                                //masterDomain.LSS_VAL = 0;
                                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                                e.ErrorContent = eLog.Message;
                                e.SetError(e.ErrorContent, e.ErrorType);
                                return;
                            }

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
                            try
                            {

                                masterDomain.UPD_USR_ID = SystemProperties.USER_VO.USR_ID;
                                masterDomain.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;

                                //
                                int _Num = 0;
                                string resultMsg = "";
                                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6710/mst/u", new StringContent(JsonConvert.SerializeObject(masterDomain), System.Text.Encoding.UTF8, "application/json")))
                                {
                                    if (response.IsSuccessStatusCode)
                                    {
                                        resultMsg = await response.Content.ReadAsStringAsync();
                                        if (int.TryParse(resultMsg, out _Num) == false)
                                        {
                                            //실패
                                            e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                                            e.ErrorContent = resultMsg;
                                            e.SetError(e.ErrorContent, e.ErrorType);
                                            return;
                                        }

                                        this.ConfigViewPage1Edit_Master.RefreshData();
                                    }
                                }
                            }
                            catch (System.Exception eLog)
                            {
                                //masterDomain.LSS_VAL = 0;
                                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                                e.ErrorContent = eLog.Message;
                                e.SetError(e.ErrorContent, e.ErrorType);
                                return;
                            }
                        }
                    }
                }
                else if (updDt)
                {
                    if (e.IsValid)
                    {
                        if (masterDomain.UPD_DT == null)
                        {
                            masterDomain.UPD_DT = "";
                        }
                        //
                        if (!masterDomain.UPD_DT.Equals((e.Value == null ? "" : e.Value.ToString())))
                        {

                            try
                            {
                                DateTime date;
                                if (DateTime.TryParseExact(e.Value.ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out date) == false)
                                {
                                    e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                                    e.ErrorContent = "[" + e.Value.ToString() + "] (yyyy-MM-dd) 입력 값이 맞지 않습니다";
                                    e.SetError(e.ErrorContent, e.ErrorType);
                                    return;
                                }

                                masterDomain.UPD_DT = e.Value.ToString();

                                masterDomain.MAKE_ST_DT = Convert.ToDateTime(masterDomain.UPD_DT + " " + masterDomain.WRK_ST_DT).ToString("yyyy-MM-dd HH:mm:00");
                                masterDomain.MAKE_END_DT = Convert.ToDateTime(masterDomain.UPD_DT + " " + masterDomain.WRK_END_DT).ToString("yyyy-MM-dd HH:mm:00");

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
                                    DateTime StartDate = Convert.ToDateTime(masterDomain.UPD_DT + " " + masterDomain.WRK_ST_DT);
                                    DateTime EndDate = Convert.ToDateTime(masterDomain.UPD_DT + " " + masterDomain.WRK_END_DT);
                                    TimeSpan dateDiff = EndDate - StartDate;
                                    masterDomain.WRK_HRS = dateDiff.Hours + "." + dateDiff.Minutes;
                                }


                                masterDomain.isCheckd = true;

                                masterDomain.UPD_USR_ID = SystemProperties.USER_VO.USR_ID;
                                masterDomain.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;

                                //
                                int _Num = 0;
                                string resultMsg = "";
                                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6710/mst/u", new StringContent(JsonConvert.SerializeObject(masterDomain), System.Text.Encoding.UTF8, "application/json")))
                                {
                                    if (response.IsSuccessStatusCode)
                                    {
                                        resultMsg = await response.Content.ReadAsStringAsync();
                                        if (int.TryParse(resultMsg, out _Num) == false)
                                        {
                                            //실패
                                            e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                                            e.ErrorContent = resultMsg;
                                            e.SetError(e.ErrorContent, e.ErrorType);
                                            return;
                                        }

                                        this.ConfigViewPage1Edit_Master.RefreshData();
                                    }
                                }
                            }
                            catch (System.Exception eLog)
                            {
                                //masterDomain.LSS_VAL = 0;
                                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                                e.ErrorContent = eLog.Message;
                                e.SetError(e.ErrorContent, e.ErrorType);
                                return;
                            }
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
                            if (DateTime.TryParseExact(masterDomain.PROD_PLN_DT + " " + e.Value.ToString(), "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out date) == false)
                            {
                                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                                e.ErrorContent = "[" + e.Value.ToString() + "] (HH:MM) 입력 값이 맞지 않습니다";
                                e.SetError(e.ErrorContent, e.ErrorType);
                                return;
                            }

                            //masterDomain.WRK_ST_DT = Convert.ToDateTime(e.Value.ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                            masterDomain.WRK_ST_DT = e.Value.ToString();
                            masterDomain.MAKE_ST_DT = Convert.ToDateTime(masterDomain.PROD_PLN_DT + " " + masterDomain.WRK_ST_DT).ToString("yyyy-MM-dd HH:mm:00");
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
                                DateTime StartDate = Convert.ToDateTime(masterDomain.PROD_PLN_DT + " " + masterDomain.WRK_ST_DT);
                                DateTime EndDate = Convert.ToDateTime(masterDomain.PROD_PLN_DT + " " + masterDomain.WRK_END_DT);
                                TimeSpan dateDiff = EndDate - StartDate;
                                masterDomain.WRK_HRS = dateDiff.Hours + "." + dateDiff.Minutes;
                            }


                            try
                            {

                                masterDomain.UPD_USR_ID = SystemProperties.USER_VO.USR_ID;
                                masterDomain.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;

                                //
                                int _Num = 0;
                                string resultMsg = "";
                                //ManVo _tmpVo;
                                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6710/mst/u", new StringContent(JsonConvert.SerializeObject(masterDomain), System.Text.Encoding.UTF8, "application/json")))
                                {
                                    if (response.IsSuccessStatusCode)
                                    {
                                        resultMsg = await response.Content.ReadAsStringAsync();
                                        if (int.TryParse(resultMsg, out _Num) == false)
                                        {
                                            //실패
                                            e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                                            e.ErrorContent = resultMsg;
                                            e.SetError(e.ErrorContent, e.ErrorType);
                                            return;
                                        }

                                        //업데이트
                                        using (HttpResponseMessage response_x = await SystemProperties.PROGRAM_HTTP.PostAsync("m6710/mst/r/u", new StringContent(JsonConvert.SerializeObject(masterDomain), System.Text.Encoding.UTF8, "application/json")))
                                        {
                                            if (response_x.IsSuccessStatusCode)
                                            {
                                                masterDomain.WRK_HRS = JsonConvert.DeserializeObject<string>(await response_x.Content.ReadAsStringAsync());
                                            }
                                        }


                                        this.ConfigViewPage1Edit_Master.RefreshData();
                                    }
                                }
                            }
                            catch (System.Exception eLog)
                            {
                                //masterDomain.LSS_VAL = 0;
                                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                                e.ErrorContent = eLog.Message;
                                e.SetError(e.ErrorContent, e.ErrorType);
                                return;
                            }

                            masterDomain.isCheckd = true;
                            //this.OKButton.IsEnabled = true;
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
                            if (DateTime.TryParseExact(masterDomain.PROD_PLN_DT + " " + e.Value.ToString(), "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out date) == false)
                            {
                                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                                e.ErrorContent = "[" + e.Value.ToString() + "] (HH:MM) 입력 값이 맞지 않습니다";
                                e.SetError(e.ErrorContent, e.ErrorType);
                                return;
                            }

                            //masterDomain.WRK_END_DT = Convert.ToDateTime(e.Value.ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                            masterDomain.WRK_END_DT = e.Value.ToString();
                            masterDomain.MAKE_END_DT = Convert.ToDateTime(masterDomain.PROD_PLN_DT + " " + masterDomain.WRK_END_DT).ToString("yyyy-MM-dd HH:mm:00");
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
                                DateTime StartDate = Convert.ToDateTime(masterDomain.PROD_PLN_DT + " " + masterDomain.WRK_ST_DT);
                                DateTime EndDate = Convert.ToDateTime(masterDomain.PROD_PLN_DT + " " + masterDomain.WRK_END_DT);
                                TimeSpan dateDiff = EndDate - StartDate;
                                masterDomain.WRK_HRS = dateDiff.Hours + "." + dateDiff.Minutes;
                            }

                            try
                            {

                                masterDomain.UPD_USR_ID = SystemProperties.USER_VO.USR_ID;
                                masterDomain.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;

                                //
                                int _Num = 0;
                                string resultMsg = "";
                               // ManVo _tmpVo;
                                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6710/mst/u", new StringContent(JsonConvert.SerializeObject(masterDomain), System.Text.Encoding.UTF8, "application/json")))
                                {
                                    if (response.IsSuccessStatusCode)
                                    {
                                        resultMsg = await response.Content.ReadAsStringAsync();
                                        if (int.TryParse(resultMsg, out _Num) == false)
                                        {
                                            //실패
                                            e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                                            e.ErrorContent = resultMsg;
                                            e.SetError(e.ErrorContent, e.ErrorType);
                                            return;
                                        }

                                        //업데이트
                                        using (HttpResponseMessage response_x = await SystemProperties.PROGRAM_HTTP.PostAsync("m6710/mst/r/u", new StringContent(JsonConvert.SerializeObject(masterDomain), System.Text.Encoding.UTF8, "application/json")))
                                        {
                                            if (response_x.IsSuccessStatusCode)
                                            {
                                                masterDomain.WRK_HRS = JsonConvert.DeserializeObject<string>(await response_x.Content.ReadAsStringAsync());
                                            }
                                        }



                                        this.ConfigViewPage1Edit_Master.RefreshData();
                                    }
                                }
                            }
                            catch (System.Exception eLog)
                            {
                                //masterDomain.LSS_VAL = 0;
                                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                                e.ErrorContent = eLog.Message;
                                e.SetError(e.ErrorContent, e.ErrorType);
                                return;
                            }

                            masterDomain.isCheckd = true;
                            //this.OKButton.IsEnabled = true;
                        }
                    }
                }
                //else if (moldCd)
                //{
                //    if (e.IsValid)
                //    {
                //        if (string.IsNullOrEmpty(masterDomain.MOLD_CD + ""))
                //        {
                //            masterDomain.MOLD_CD = "";
                //        }
                //        //
                //        if (!masterDomain.MOLD_CD.Equals((e.Value == null ? "" : e.Value.ToString())))
                //        {
                //            ManVo bankIoDao = this.lue_MOLD_NO.GetItemFromValue(e.Value) as ManVo;
                //            //
                //            if (bankIoDao != null)
                //            {
                //                masterDomain.MOLD_CD = bankIoDao.MOLD_NO;
                //                masterDomain.WEIH_SEQ = bankIoDao.WEIH_SEQ;

                //                masterDomain.GBN = bankIoDao.MOLD_NO + "-" + bankIoDao.WEIH_SEQ;
                //            }


                //            try
                //            {

                //                masterDomain.UPD_USR_ID = SystemProperties.USER_VO.USR_ID;
                //                masterDomain.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;

                //                //
                //                int _Num = 0;
                //                string resultMsg = "";
                //                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6710/mst/u", new StringContent(JsonConvert.SerializeObject(masterDomain), System.Text.Encoding.UTF8, "application/json")))
                //                {
                //                    if (response.IsSuccessStatusCode)
                //                    {
                //                        resultMsg = await response.Content.ReadAsStringAsync();
                //                        if (int.TryParse(resultMsg, out _Num) == false)
                //                        {
                //                            //실패
                //                            e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                //                            e.ErrorContent = resultMsg;
                //                            e.SetError(e.ErrorContent, e.ErrorType);
                //                            return;
                //                        }

                //                        this.ConfigViewPage1Edit_Master.RefreshData();
                //                    }
                //                }
                //            }
                //            catch (System.Exception eLog)
                //            {
                //                //masterDomain.LSS_VAL = 0;
                //                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                //                e.ErrorContent = eLog.Message;
                //                e.SetError(e.ErrorContent, e.ErrorType);
                //                return;
                //            }

                //            masterDomain.isCheckd = true;
                //            //this.OKButton.IsEnabled = true;
                //        }
                //    }
                //}
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

                            try
                            {

                                masterDomain.UPD_USR_ID = SystemProperties.USER_VO.USR_ID;
                                masterDomain.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;

                                //
                                int _Num = 0;
                                string resultMsg = "";
                                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6710/mst/u", new StringContent(JsonConvert.SerializeObject(masterDomain), System.Text.Encoding.UTF8, "application/json")))
                                {
                                    if (response.IsSuccessStatusCode)
                                    {
                                        resultMsg = await response.Content.ReadAsStringAsync();
                                        if (int.TryParse(resultMsg, out _Num) == false)
                                        {
                                            //실패
                                            e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                                            e.ErrorContent = resultMsg;
                                            e.SetError(e.ErrorContent, e.ErrorType);
                                            return;
                                        }

                                        this.ConfigViewPage1Edit_Master.RefreshData();
                                    }
                                }
                            }
                            catch (System.Exception eLog)
                            {
                                //masterDomain.LSS_VAL = 0;
                                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                                e.ErrorContent = eLog.Message;
                                e.SetError(e.ErrorContent, e.ErrorType);
                                return;
                            }
                        }
                    }
                }
                //투입 수량
                else if (torVal)
                {
                    ManVo itemDomain = (ManVo)ConfigViewPage3Edit_Popup.GetFocusedRow();

                    if (e.IsValid)
                    {
                        if (string.IsNullOrEmpty(itemDomain.TOR_VAL + ""))
                        {
                            itemDomain.TOR_VAL = 0;
                        }
                        //
                        if (!itemDomain.TOR_VAL.Equals((e.Value == null ? "" : e.Value.ToString())))
                        {
                            itemDomain.TOR_VAL = e.Value.ToString();

                            itemDomain.isCheckd = true;
                            //this.OKButton.IsEnabled = true;

                            try
                            {

                                itemDomain.UPD_USR_ID = SystemProperties.USER_VO.USR_ID;
                                itemDomain.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;

                                //
                                int _Num = 0;
                                string resultMsg = "";
                                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6710/dtl3/u", new StringContent(JsonConvert.SerializeObject(itemDomain), System.Text.Encoding.UTF8, "application/json")))
                                {
                                    if (response.IsSuccessStatusCode)
                                    {
                                        resultMsg = await response.Content.ReadAsStringAsync();
                                        if (int.TryParse(resultMsg, out _Num) == false)
                                        {
                                            //실패
                                            e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                                            e.ErrorContent = resultMsg;
                                            e.SetError(e.ErrorContent, e.ErrorType);
                                            return;
                                        }

                                        this.ConfigViewPage3Edit_Popup.RefreshData();
                                    }
                                }
                            }
                            catch (System.Exception eLog)
                            {
                                //masterDomain.LSS_VAL = 0;
                                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                                e.ErrorContent = eLog.Message;
                                e.SetError(e.ErrorContent, e.ErrorType);
                                return;
                            }

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

            //bool lssVal = (e.Column.FieldName.ToString().Equals("LSS_VAL") ? true : false);
            //int rowHandle = this.ViewTableDtl.FocusedRowHandle + 1;
            //this.ViewGridDtl.RefreshRow(rowHandle - 1);
            //this.ViewTableDtl.FocusedRowHandle = rowHandle;
        }


    }
}