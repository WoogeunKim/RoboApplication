using DevExpress.Data;
using DevExpress.Mvvm.Native;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.WindowsUI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;
using AquilaErpWpfApp3.Util;
using System.Threading.Tasks;
using ModelsLibrary.Pur;
//using System.Linq;

namespace AquilaErpWpfApp3.View.PUR.Dialog
{
    /// <summary>
    /// M66107DetailDialog.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class P4411PurOpmzDialog : DXWindow
    {
        private PurVo orgVo;
        private string _title = "최적화 발주 의뢰내역";
        private PurVo updateDao;


        public P4411PurOpmzDialog(PurVo vo)
        {
            InitializeComponent();

            this.orgVo = vo;

            this.OKButton.Click += new RoutedEventHandler(OKButton_Click);
            this.CancelButton.Click += new RoutedEventHandler(CancelButton_Click);

            this.OKButton.IsEnabled = false;

            Refresh();
        }

        // 최적화 대상 필요 원자재 리스트를 조회
        private async void Refresh()
        {
            try
            {
                if (DXSplashScreen.IsActive == false) DXSplashScreen.Show<ProgressWindow>();

                // 최적화 대상 필요 원자재 리스트를 조회합니다. 
                this.ViewJOB_ITEMEdit.ItemsSource = await PostMstItems<PurVo>("p4411/opmz/mtrl", this.orgVo);

                if (DXSplashScreen.IsActive == true) DXSplashScreen.Close();
            }
            catch (System.Exception eLog)
            {
                if (DXSplashScreen.IsActive == true) DXSplashScreen.Close();
                WinUIMessageBox.Show(eLog.Message, _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }

        /// <summary>
        /// JSON 을 통해 서버로부터 정보를 가져옵니다.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Path"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        private async Task<List<T>> PostMstItems<T>(string Path, object obj)
        {
            List<T> ret = new List<T>();

            try
            {
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync(Path
                                                                                                  , new StringContent(JsonConvert.SerializeObject(obj), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        ret = JsonConvert.DeserializeObject<IEnumerable<T>>(await response.Content.ReadAsStringAsync()).Cast<T>().ToList();
                    }
                }
            }
            catch (Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
            }

            return ret;
        }


        #region Functon (OKButton_Click, CancelButton_Click)

        private async void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //List<ManVo> checkList = (this.ViewJOB_ITEMEdit.ItemsSource as List<PurVo>).Where(x=>x.isCheckd == true).ToList<PurVo>();

                //if (checkList.Count <= 0)
                //{
                //    WinUIMessageBox.Show("데이터가 존재 하지 않습니다.", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                //    return;
                //}

                //foreach (ManVo vo in checkList)
                //{
                //    int _Num = 0;

                //    if(int.TryParse(vo.ITM_PUR_QTY.ToString(), out _Num) == false)
                //    {
                //        WinUIMessageBox.Show(vo.OPMZ_SEQ.ToString() + "번 발주수량이 입력되지 않았습니다.", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                //        return;
                //    }
                //    if (int.Parse(vo.ITM_PUR_QTY.ToString()) <= 0)
                //    {
                //        WinUIMessageBox.Show(vo.OPMZ_SEQ.ToString() + "번 발주수량울 다시 입력하세요.", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                //        return;
                //    }
                //}

                //MessageBoxResult result = WinUIMessageBox.Show("정말로 저장 하시겠습니까?", "[저장]" + _title, MessageBoxButton.YesNo, MessageBoxImage.Question);

                //if (result == MessageBoxResult.Yes)
                //{
                //    int _Num = 0;
                //    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m66107/pur/mtrl/u", new StringContent(JsonConvert.SerializeObject(checkList), System.Text.Encoding.UTF8, "application/json")))
                //    {
                //        if (response.IsSuccessStatusCode)
                //        {
                //            string resultMsg = await response.Content.ReadAsStringAsync();
                //            if (int.TryParse(resultMsg, out _Num) == false)
                //            {
                //                //실패
                //                WinUIMessageBox.Show(resultMsg, _title, MessageBoxButton.OK, MessageBoxImage.Error);
                //                return;
                //            }

                //            this.DialogResult = true;
                //            this.Close();
                //        }
                //    }
                //}
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
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
        #endregion

        private void PART_Editor_Checked(object sender, RoutedEventArgs e)
        {
            this.OKButton.IsEnabled = true;
        }

        private void CheckEdit_Checked(object sender, RoutedEventArgs e)
        {
            CheckEdit checkEdit = sender as CheckEdit;
            PurVo tmpImsi;
            for (int x = 0; x < this.ViewJOB_ITEMEdit.VisibleRowCount; x++)
            {
                int rowHandle = this.ViewJOB_ITEMEdit.GetRowHandleByVisibleIndex(x);
                if (rowHandle > -1)
                {
                    tmpImsi = this.ViewJOB_ITEMEdit.GetRow(rowHandle) as PurVo;
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

        private void viewPage1EditView_HiddenEditor(object sender, EditorEventArgs e)
        {
            this.viewJOB_ITEMView.CommitEditing();
        }



        private void GridColumn_Validate(object sender, DevExpress.Xpf.Grid.GridCellValidationEventArgs e)
        {
            try
            {
                //PurVo masterDomain = (PurVo)ViewJOB_ITEMEdit.GetFocusedRow();

                //bool itmPurQty = (e.Column.FieldName.ToString().Equals("ITM_PUR_QTY") ? true : false);
                //bool mrpDesc = (e.Column.FieldName.ToString().Equals("MRP_DESC") ? true : false);

                //if (itmPurQty)
                //{
                //    if (e.IsValid)
                //    {
                //        if (string.IsNullOrEmpty(masterDomain.ITM_PUR_QTY + ""))
                //        {
                //            masterDomain.ITM_PUR_QTY = 0;
                //        }
                //        //
                //        if (!masterDomain.ITM_PUR_QTY.Equals((e.Value == null ? "" : e.Value.ToString())))
                //        {
                //            int? tmpInt = Convert.ToInt32(e.Value.ToString());

                //            masterDomain.ITM_PUR_QTY = tmpInt;

                //            masterDomain.isCheckd = true;
                //            this.OKButton.IsEnabled = true;
                //        }
                //    }
                //}
                //else if (mrpDesc)
                //{
                //    if (e.IsValid)
                //    {
                //        if (string.IsNullOrEmpty(masterDomain.MRP_DESC))
                //        {
                //            masterDomain.MRP_DESC = "";
                //        }
                //        //
                //        if (!masterDomain.MRP_DESC.Equals((e.Value == null ? "" : e.Value.ToString())))
                //        {
                //            masterDomain.MRP_DESC = e.Value.ToString();
                //            masterDomain.isCheckd = true;
                //            this.OKButton.IsEnabled = true;
                //        }
                //    }
                //}

                //this.ViewJOB_ITEMEdit.RefreshData();
            }
            catch (Exception eLog)
            {
                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                e.ErrorContent = eLog.Message;
                e.SetError(e.ErrorContent, e.ErrorType);
                return;
            }
        }
    }
}
