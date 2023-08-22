using AquilaErpWpfApp3.Util;
using AquilaErpWpfApp3.ViewModel;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Man;
using ModelsLibrary.Sale;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AquilaErpWpfApp3.View.M
{
    public partial class M6740 : UserControl
    {
        private string _title = "실적등록";

        public M6740()
        {
            DataContext = new M6740ViewModel();
            //
            InitializeComponent();

            //this.ViewGridMst.MouseLeftButtonUp += ViewGridMst_MouseLeftButtonUp;
            //this.ViewGridDtl.MouseLeftButtonUp += VeiwGridDtl_MouseLeftButtonUp;

        }

        //private void ViewGridMst_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        //{
        //    this.ViewGridMst.RefreshData();
        //}



        private void PART_Editor_Checked(object sender, RoutedEventArgs e)
        {
        }


        private void CheckEdit_Checked(object sender, RoutedEventArgs e)
        {
            //CheckEdit checkEdit = sender as CheckEdit;
            //SaleVo tmpImsi;
            //for (int x = 0; x < this.ViewGridDtl.VisibleRowCount; x++)
            //{
            //    int rowHandle = this.ViewGridDtl.GetRowHandleByVisibleIndex(x);
            //    if (rowHandle > -1)
            //    {
            //        tmpImsi = this.ViewGridDtl.GetRow(rowHandle) as SaleVo;
            //        if (checkEdit.IsChecked == true)
            //        {
            //            tmpImsi.isCheckd = true;

            //        }
            //        else
            //        {
            //            tmpImsi.isCheckd  
            //        }

            //    }
            //}
        }


        //private void VeiwGridDtl_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        //{
        //    this.ViewGridDtl.RefreshData();
        //}


        private async void GridColumn_Validate(object sender, DevExpress.Xpf.Grid.GridCellValidationEventArgs e)
        {
            try
            {
                ManVo detailDomain = (ManVo)ViewGridDtl.GetFocusedRow();
                bool prodDirQty = (e.Column.FieldName.ToString().Equals("PROD_GD_QTY") ? true : false);     // 양품수량


                if (prodDirQty)
                {
                    if (e.IsValid)
                    {
                        if (string.IsNullOrEmpty(detailDomain.PROD_DIR_QTY + ""))
                        {
                            detailDomain.PROD_GD_QTY = detailDomain.PROD_QTY;
                        }
                        //
                        if (!detailDomain.PROD_GD_QTY.Equals((e.Value == null ? "" : e.Value.ToString())))
                        {
                            double? tmpInt = Convert.ToDouble(e.Value.ToString());

                            // 잘못된 값
                            if (tmpInt <= 0)
                            {
                                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                                e.ErrorContent = "값이 0보다 커야 합니다.";
                                e.SetError(e.ErrorContent, e.ErrorType);
                                M_DTL_PROD.IsEnabled = false;
                                return;
                            }
                            else if (Convert.ToDouble(detailDomain.PROD_QTY) < tmpInt)
                            {
                                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                                e.ErrorContent = "양품수량 값이 수량 값 보다 작아야 합니다.";
                                e.SetError(e.ErrorContent, e.ErrorType);
                                M_DTL_PROD.IsEnabled = false;
                                return;
                            }
                            else
                            {
                                M_DTL_PROD.IsEnabled = true;
                            }

                            detailDomain.PROD_GD_QTY = tmpInt;

                            
                        }
                    }
                }
                else
                {
                    // 적용 안함.
                    return;
                }


                //// 서버의 정보를 수정합니다.
                //if (prodDirQty)
                //{
                //    // 지시일자, 택타임, 지시수량 변경
                //    await PostJsonUpdate("m661010/dtl/u", detailDomain);
                //}


                //성공
                this.ViewGridDtl.RefreshData();
            }
            catch (Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, _title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ViewTableDtl_HiddenEditor(object sender, DevExpress.Xpf.Grid.EditorEventArgs e)
        {
            this.ViewTableDtl.CommitEditing();
        }




        ///// <summary>
        ///// 변경할 정보를 서버에게 전달합니다.
        ///// </summary>
        ///// <param name="Path"></param>
        ///// <param name="obj"></param>
        ///// <returns></returns>
        //private async Task PostJsonUpdate(string Path, object obj)
        //{
        //    try
        //    {
        //        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync(Path
        //                                                                                          , new StringContent(JsonConvert.SerializeObject(obj), System.Text.Encoding.UTF8, "application/json")))
        //        {
        //            if (response.IsSuccessStatusCode)
        //            {
        //                int _Num = 0;
        //                string resultMsg = await response.Content.ReadAsStringAsync();
        //                if (int.TryParse(resultMsg, out _Num) == false)
        //                {
        //                    //실패
        //                    WinUIMessageBox.Show(resultMsg, _title + "- 수정 오류", MessageBoxButton.OK, MessageBoxImage.Error);
        //                }
        //                else
        //                {
        //                    // 성공
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception eLog)
        //    {
        //        WinUIMessageBox.Show(eLog.Message, _title + "- 수정 오류", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
        //    }
        //}
    }
}