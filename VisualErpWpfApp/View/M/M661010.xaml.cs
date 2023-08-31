using AquilaErpWpfApp3.Util;
using AquilaErpWpfApp3.ViewModel;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Man;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using ModelsLibrary.Code;
using AquilaErpWpfApp3.View.M.Dialog;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Data;

namespace AquilaErpWpfApp3.View.M
{
    /// <summary>
    /// M661010.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class M661010 : UserControl
    {

        string _title = "오더매니저 설비";


        public M661010()
        {

            DataContext = new M661010ViewModel();

            InitializeComponent();



            this.ViewGridMst.MouseLeftButtonUp += ViewGridMst_MouseLeftButtonUp;
            this.ViewGridSlList.MouseLeftButtonUp += ViewGridSlList_MouseLeftButtonUp;
            //this.ViewGridLotList.MouseLeftButtonUp += ViewGridLotList_MouseLeftButtonUp;

        }

        private void View_SL_CellValueChanging(object sender, CellValueChangedEventArgs e)
        {
            if (e.Column.FieldName == "isCheckd")
            {

                var viewModel = DataContext as M661010ViewModel;
                if (viewModel != null)
                {
                    viewModel.SelectDetailCheckd();
                }

                //TableView view = (TableView)sender;
                //view.CloseEditor();
                //view.FocusedRowHandle = GridControl.InvalidRowHandle;
                //view.FocusedRowHandle = e.RowHandle;
            }
        }

        //private void View_LOV_CellValueChanging(object sender, CellValueChangedEventArgs e)
        //{
        //    var viewModel = DataContext as M661010ViewModel;

        //    if (e.Column.FieldName == "isCheckd")
        //    {
        //        if (viewModel != null)
        //        {
        //            viewModel.SelectLovCheckd();
        //        }
        //    }

        //    //if (e.Column.FieldName == "PROD_DIR_DT" || e.Column.FieldName == "PROD_DIR_QTY")
        //    //{
        //    //    if (viewModel != null)
        //    //    {
        //    //        viewModel.SelectLovListDetail();
        //    //    }
        //    //}
        //}

        private async void GridColumn_Validate(object sender, DevExpress.Xpf.Grid.GridCellValidationEventArgs e)
        {
            try
            {
                ManVo detailDomain = (ManVo)ViewGridSlList.GetFocusedRow(); // 수주 리스트
                ManVo lovDomain = (ManVo)ViewGridLotList.GetFocusedRow(); // 작업지시 리스트


                bool n1st_eq_no = (e.Column.FieldName.ToString().Equals("N1ST_EQ_NO") ? true : false);       // 절단설비
                bool n2nd_eq_no = (e.Column.FieldName.ToString().Equals("N2ND_EQ_NO") ? true : false);       // 가공설비

                bool prod_dir_dt = (e.Column.FieldName.ToString().Equals("PROD_DIR_DT") ? true : false);       // 생산계획일자
                bool prod_dir_qty = (e.Column.FieldName.ToString().Equals("PROD_DIR_QTY") ? true : false);       // 지시수량

                // 절단설비
                if (n1st_eq_no)
                {
                    if (e.IsValid)
                    {
                        if (string.IsNullOrEmpty(detailDomain.N1ST_EQ_NM + ""))
                        {
                            detailDomain.N1ST_EQ_NO = "";
                            detailDomain.N1ST_EQ_NM = "";
                        }
                        if (!detailDomain.N1ST_EQ_NM.Equals((e.Value == null ? "" : e.Value.ToString())))
                        {
                            ManVo eqDao = this.lue_N1ST_EQ_NM.GetItemFromValue(e.Value) as ManVo;

                            if (eqDao != null)
                            {
                                detailDomain.N1ST_EQ_NO = eqDao.PROD_EQ_NO;
                                detailDomain.N1ST_EQ_NM = eqDao.EQ_NM;
                            }
                        }
                    }
                }
                // 가공설비
                else if (n2nd_eq_no)
                {
                    if (e.IsValid)
                    {
                        if (string.IsNullOrEmpty(detailDomain.N2ND_EQ_NM + ""))
                        {
                            detailDomain.N2ND_EQ_NO = "";
                            detailDomain.N2ND_EQ_NM = "";
                        }
                        if (!detailDomain.N2ND_EQ_NM.Equals((e.Value == null ? "" : e.Value.ToString())))
                        {
                            ManVo eqDao = this.lue_N2ND_EQ_NM.GetItemFromValue(e.Value) as ManVo;

                            if (eqDao != null)
                            {
                                detailDomain.N2ND_EQ_NO = eqDao.PROD_EQ_NO;
                                detailDomain.N2ND_EQ_NM = eqDao.EQ_NM;
                            }
                        }
                    }
                }
                else if (prod_dir_qty)
                {
                    if (e.IsValid)
                    {
                        if (string.IsNullOrEmpty(lovDomain.PROD_DIR_QTY + ""))
                        {
                            lovDomain.PROD_DIR_QTY = 0;
                        }
                        //
                        if (!lovDomain.PROD_DIR_QTY.Equals((e.Value == null ? "" : e.Value.ToString())))
                        {
                            double? tmpInt = Convert.ToDouble(e.Value.ToString());

                            // 잘못된 값
                            if (tmpInt <= 0) return;

                            lovDomain.PROD_DIR_QTY = tmpInt;

                            if (lovDomain.PROD_TK_TM != null)
                            {
                                if (Convert.ToDouble(lovDomain.PROD_TK_TM.ToString()) >= 0)
                                {

                                    // 생산소요시간 = 택타임(제품 한개 생산시간) * 지시수량
                                    lovDomain.PROD_WRK_TM = Convert.ToDouble(lovDomain.PROD_TK_TM.ToString()) * Convert.ToDouble(lovDomain.PROD_DIR_QTY.ToString());

                                }
                            }

                        }
                    }
                }
                else if (prod_dir_dt)
                {
                    if (e.IsValid)
                    {
                        if (string.IsNullOrEmpty(lovDomain.PROD_DIR_DT + ""))
                        {
                            lovDomain.PROD_DIR_DT = "";
                        }
                        if (!lovDomain.PROD_DIR_DT.Equals((e.Value == null ? "" : e.Value.ToString())))
                        {
                            DateTime date = DateTime.Now;

                            if (DateTime.TryParse(e.Value.ToString(), out date) == false)
                            {
                                //e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                                //e.ErrorContent = "날짜를 다시 입력하세요.";
                                //e.SetError(e.ErrorContent, e.ErrorType);
                                return;
                            }
                            lovDomain.PROD_DIR_DT = Convert.ToDateTime(e.Value.ToString());
                        }
                    }
                }
                else
                {
                    return;
                }

                // 서버 정보 수정
                if (n1st_eq_no || n2nd_eq_no)
                {
                    // 절단설비, 가공설비 변경
                    await PostJsonUpdate("m66311/eq/u", detailDomain);
                    this.ViewGridSlList.RefreshData();
                }

                // 작업지시 정보 수정
                else if (prod_dir_dt)
                {
                    // 생산계획 일자 변경
                    await PostJsonUpdate("m661010/lov/u", lovDomain);
                    this.ViewGridLotList.RefreshData();
                }
                else if (prod_dir_qty)
                {
                    await PostJsonUpdate("m661010/lov/wgt/u", lovDomain);

                    // 중량 정보 가져오기
                    ManVo ret = null; // ret 변수 선언 및 초기화
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m661010/lov/wgt", new StringContent(JsonConvert.SerializeObject(lovDomain), System.Text.Encoding.UTF8, "application/json")))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            ret = JsonConvert.DeserializeObject<ManVo>(await response.Content.ReadAsStringAsync());
                        }
                    }

                    // 중량 정보를 가져온 후에 사용


                    if (ret != null)
                    {
                        lovDomain.PROD_WGT = ret.PROD_WGT;

                        await PostJsonUpdate("m661010/lov/u", lovDomain);
                        this.ViewGridLotList.RefreshData();




                        this.ViewGridSlList.RefreshData();
                    }
                }

            }
            catch (Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, _title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        // GRID 선택할때마다 초기화

        private void ViewGridMst_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.ViewGridMst.RefreshData();
        }

        private void ViewGridSlList_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.ViewGridSlList.RefreshData();

        }

        //private void ViewGridLotList_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        //{
        //    this.ViewGridLotList.RefreshData();
        //}








        /// <summary>
        /// 변경할 정보를 서버에게 전달합니다.
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        private async Task PostJsonUpdate(string Path, object obj)
        {
            try
            {
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync(Path, new StringContent(JsonConvert.SerializeObject(obj), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        int _Num = 0;
                        string resultMsg = await response.Content.ReadAsStringAsync();
                        if (int.TryParse(resultMsg, out _Num) == false)
                        {
                            //실패
                            WinUIMessageBox.Show(resultMsg, _title + "- 수정 오류", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                        {
                            // 성공
                        }
                    }
                }
            }
            catch (Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, _title + "- 수정 오류", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
            }
        }

        private void CheckEdit_Checked(object sender, RoutedEventArgs e)
        {
            CheckEdit checkEdit = sender as CheckEdit;

            SetCheckState(this.ViewGridSlList, checkEdit.IsChecked == true);
            SetCheckState(this.ViewGridLotList, checkEdit.IsChecked == true);

        }

        private void SetCheckState(GridControl gridView, bool isChecked)
        {
            for (int x = 0; x < gridView.VisibleRowCount; x++)
            {
                int rowHandle = gridView.GetRowHandleByVisibleIndex(x);
                if (rowHandle > -1)
                {
                    ManVo tmpImsi = gridView.GetRow(rowHandle) as ManVo;
                    if (tmpImsi != null)
                    {
                        tmpImsi.isCheckd = isChecked;
                    }
                }
            }
        }

        private void AllCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (this.DataContext is M661010ViewModel viewModel)
            {
                viewModel.SelectLovListDetail();
            }
        }





    }
}