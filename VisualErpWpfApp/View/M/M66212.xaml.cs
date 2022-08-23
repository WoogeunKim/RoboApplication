using ModelsLibrary.Code;
using ModelsLibrary.Man;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Windows.Controls;
using AquilaErpWpfApp3.Util;
using AquilaErpWpfApp3.ViewModel;

namespace AquilaErpWpfApp3.View.M
{
    public partial class M66212 : UserControl
    {
        private string title = "(BOM)공정제품연결";
        public M66212()
        {
            DataContext = new M66212ViewModel();
            //
            InitializeComponent();
            //
            //this.txt_Master_Search.KeyDown += new KeyEventHandler(txt_Master_Search_KeyDown);
            //this.btn_ConfigViewPage_Master_search.Click += new RoutedEventHandler(btn_Master_search_Click);
            //
        }
        #region Functon (Master Search)

        private void viewPage1EditView_HiddenEditor(object sender, DevExpress.Xpf.Grid.EditorEventArgs e)
        {
            this.configViewPage1EditView_Master.CommitEditing();
        }

        private async void GridColumn_Validate(object sender, DevExpress.Xpf.Grid.GridCellValidationEventArgs e)
        {
            try
            {
                ManVo masterDomain = (ManVo)ConfigViewPage1Edit_Master.GetFocusedRow();

                bool mtrltmNm = (e.Column.FieldName.ToString().Equals("MTRL_ITM_NM") ? true : false);
                bool useQty = (e.Column.FieldName.ToString().Equals("USE_QTY") ? true : false);
                bool tolQty = (e.Column.FieldName.ToString().Equals("TOL_QTY") ? true : false);

                int _Num;
                if (mtrltmNm)
                {
                    if (e.IsValid)
                    {
                        if (string.IsNullOrEmpty(masterDomain.MTRL_ITM_NM + ""))
                        {
                            masterDomain.MTRL_ITM_NM = "";
                            masterDomain.MTRL_ITM_CD = "";
                        }
                        //
                        if (!masterDomain.MTRL_ITM_NM.Equals((e.Value == null ? "" : e.Value.ToString())))
                        {
                            SystemCodeVo bankIoDao = this.lue_MTRL_ITM_NM.GetItemFromValue(e.Value) as SystemCodeVo;
                            //
                            if (bankIoDao != null)
                            {
                                masterDomain.MTRL_ITM_CD = bankIoDao.ITM_CD;
                                masterDomain.MTRL_ITM_NM = bankIoDao.ITM_NM;
                            }
                            //
                            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m66212/mst/i", new StringContent(JsonConvert.SerializeObject(masterDomain), System.Text.Encoding.UTF8, "application/json")))
                            {
                                if (response.IsSuccessStatusCode)
                                {
                                    string result = await response.Content.ReadAsStringAsync();
                                    if (int.TryParse(result, out _Num) == false)
                                    {
                                        //실패
                                        //WinUIMessageBox.Show(result, _title, MessageBoxButton.OK, MessageBoxImage.Error);
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
                else if (useQty)
                {
                    if (e.IsValid)
                    {
                        if (string.IsNullOrEmpty(masterDomain.USE_QTY + ""))
                        {
                            masterDomain.USE_QTY = 0;
                        }
                        //
                        if (!masterDomain.USE_QTY.Equals((e.Value == null ? "" : e.Value.ToString())))
                        {
                            masterDomain.USE_QTY = e.Value;

                        }
                        //
                        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m66212/mst/i", new StringContent(JsonConvert.SerializeObject(masterDomain), System.Text.Encoding.UTF8, "application/json")))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                string result = await response.Content.ReadAsStringAsync();
                                if (int.TryParse(result, out _Num) == false)
                                {
                                    //실패
                                    //WinUIMessageBox.Show(result, _title, MessageBoxButton.OK, MessageBoxImage.Error);
                                    return;
                                }
                            }
                        }
                    }
                }
                else if (tolQty)
                {
                    if (e.IsValid)
                    {
                        if (string.IsNullOrEmpty(masterDomain.TOL_QTY + ""))
                        {
                            masterDomain.TOL_QTY = 0;
                        }
                        //
                        if (!masterDomain.TOL_QTY.Equals((e.Value == null ? "" : e.Value.ToString())))
                        {
                            masterDomain.TOL_QTY = e.Value;

                        }
                        //
                        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m66212/mst/i", new StringContent(JsonConvert.SerializeObject(masterDomain), System.Text.Encoding.UTF8, "application/json")))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                string result = await response.Content.ReadAsStringAsync();
                                if (int.TryParse(result, out _Num) == false)
                                {
                                    //실패
                                    //WinUIMessageBox.Show(result, _title, MessageBoxButton.OK, MessageBoxImage.Error);
                                    return;
                                }
                            }
                        }
                    }
                }
                this.ConfigViewPage1Edit_Master.RefreshData();
            }
            catch (Exception eLog)
            {
                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                e.ErrorContent = eLog.Message;
                //this.MSG.Text = eLog.Message;
                e.SetError(e.ErrorContent, e.ErrorType);
                return;
            }
        }
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

    }
}