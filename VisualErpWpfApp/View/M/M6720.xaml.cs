﻿using AquilaErpWpfApp3.ViewModel;
using DevExpress.Xpf.Printing;
using ModelsLibrary.Man;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace AquilaErpWpfApp3.View.M
{
    public partial class M6720 : UserControl
    {
        private string title = "실적현황";
        public M6720()
        {
            DataContext = new M6720ViewModel();
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
        }

        private void viewPage1EditView_HiddenEditor(object sender, DevExpress.Xpf.Grid.EditorEventArgs e)
        {
            this.configViewPage1EditView_Master.CommitEditing();

            //bool lssVal = (e.Column.FieldName.ToString().Equals("LSS_VAL") ? true : false);
            //int rowHandle = this.ViewTableDtl.FocusedRowHandle + 1;
            //this.ViewGridDtl.RefreshRow(rowHandle - 1);
            //this.ViewTableDtl.FocusedRowHandle = rowHandle;
        }



        private void M_MST_PRINT_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            //this.ViewTableMst.PrintAutoWidth = true;
            ////this.tableView.AutoWidth = true;
            //this.ViewTableMst.BestFitColumns();
            ////IList<GridColumn> columns = this.tableView.VisibleColumns;
            ////columns[0].Visible = false;
            ////columns[0].AllowEditing = DevExpress.Utils.DefaultBoolean.True;
            ManVo selVo = (ManVo)this.ConfigViewPage1Edit_Master.SelectedItem;
            if (selVo != null)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<DataTemplate xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" xmlns:dxe=\"http://schemas.devexpress.com/winfx/2008/xaml/editors\">");
                sb.Append("<dxe:TextEdit Text=\"" + this.title + "\" FontWeight=\"Bold\"  FontSize=\"10\"  />");
                sb.Append("</DataTemplate>");
                DataTemplate templateHeader = (DataTemplate)System.Windows.Markup.XamlReader.Parse(sb.ToString());

                sb = new StringBuilder();
                sb.Append("<DataTemplate xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" xmlns:dxe=\"http://schemas.devexpress.com/winfx/2008/xaml/editors\">");
                sb.Append("<dxe:TextEdit Text=\"" + System.DateTime.Now.ToString("yyyy-MM-dd hh:mm") + "\"/>");
                sb.Append("</DataTemplate>");
                DataTemplate templateFooter = (DataTemplate)System.Windows.Markup.XamlReader.Parse(sb.ToString());


                using (PrintableControlLink prtLink = new PrintableControlLink(this.configViewPage1EditView_Master))
                {
                    //  prtLink.PageHeaderTemplate = templateHeader;
                    //prtLink.PageFooterData = "" + System.DateTime.Now.ToString("yyyy-MM-dd hh:mm");
                    //prtLink.PageFooterTemplate = Resources["PageFooter"] as DataTemplate;
                    prtLink.PageHeaderTemplate = templateHeader;
                    prtLink.PageFooterTemplate = templateFooter;
                    prtLink.PageHeaderData = null;
                    prtLink.PageFooterData = null;

                    prtLink.Margins.Top = 8;
                    prtLink.Margins.Bottom = 8;
                    prtLink.Margins.Left = 5;
                    prtLink.Margins.Right = 5;

                    prtLink.DocumentName = this.title + " - Print";
                    //prtLink.PrintingSystem.Watermark.Text = "한양화스너공업(주)";
                    //prtLink.PrintingSystem.Watermark.TextDirection = DirectionMode.ForwardDiagonal;
                    //prtLink.PrintingSystem.Watermark.Font = new Font(prtLink.PrintingSystem.Watermark.Font.FontFamily, 40);
                    //prtLink.PrintingSystem.Watermark.ForeColor = System.Drawing.Color.PaleTurquoise;
                    //prtLink.PrintingSystem.Watermark.TextTransparency = 150;

                    prtLink.Landscape = true;
                    prtLink.PrintingSystem.ShowPrintStatusDialog = true;

                    //System.Drawing.Printing.PaperSize p = new System.Drawing.Printing.PaperSize("Custom Paper Size", 400, 900); //hundredths of an inch
                    //prtLink.PaperKind = System.Drawing.Printing.PaperKind.Custom;
                    //prtLink.CustomPaperSize = new System.Drawing.Size(800,1000);


                    prtLink.PaperKind = System.Drawing.Printing.PaperKind.A4;
                    prtLink.CreateDocument(true);
                    prtLink.ShowPrintPreviewDialog(Application.Current.MainWindow, this.title);
                }
            }
        }

    }
}