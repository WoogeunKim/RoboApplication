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
            InitializeComponent();

            DataContext = new M661010ViewModel();
        }


        private async void GridColumn_Validate(object sender, DevExpress.Xpf.Grid.GridCellValidationEventArgs e)
        {
            try
            {
                ManVo detailDomain = (ManVo)ViewGridDtl.GetFocusedRow();
                bool prodDieDt = (e.Column.FieldName.ToString().Equals("PROD_DIR_DT") ? true : false);
                bool prodTkTm = (e.Column.FieldName.ToString().Equals("PROD_TK_TM") ? true : false);
                bool prodDirQty = (e.Column.FieldName.ToString().Equals("PROD_DIR_QTY") ? true : false);

                if (prodDieDt)
                {
                    if (e.IsValid)
                    {
                        if (string.IsNullOrEmpty(detailDomain.PROD_DIR_DT + ""))
                        {
                            detailDomain.PROD_DIR_DT = "";
                        }
                        if (!detailDomain.PROD_DIR_DT.Equals((e.Value == null ? "" : e.Value.ToString())))
                        {
                            DateTime date = DateTime.Now;

                            if (DateTime.TryParse(e.Value.ToString(), out date) == false)
                            {
                                //e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                                //e.ErrorContent = "날짜를 다시 입력하세요.";
                                //e.SetError(e.ErrorContent, e.ErrorType);
                                return;
                            }


                            detailDomain.PROD_DIR_DT = Convert.ToDateTime(e.Value.ToString());
                        }
                    }
                }
                else if (prodTkTm)
                {
                    if (e.IsValid)
                    {
                        if (string.IsNullOrEmpty(detailDomain.PROD_TK_TM + ""))
                        {
                            detailDomain.PROD_TK_TM = 0;
                        }
                        //
                        if (!detailDomain.PROD_TK_TM.Equals((e.Value == null ? "" : e.Value.ToString())))
                        {
                            double? tmpInt = Convert.ToDouble(e.Value.ToString());

                            // 잘못된 값
                            if (tmpInt < 0) return;

                            detailDomain.PROD_TK_TM = tmpInt;

                            if(detailDomain.PROD_DIR_QTY != null)
                            {
                                if(Convert.ToDouble(detailDomain.PROD_DIR_QTY.ToString()) > 0)
                                {
                                    // 생산소요시간 = 렉타임 / 지시수량
                                    detailDomain.PROD_WRK_TM = Convert.ToDouble(detailDomain.PROD_TK_TM.ToString()) / Convert.ToDouble(detailDomain.PROD_DIR_QTY.ToString());
                                }
                            }

                        }
                    }
                }
                else if (prodDirQty)
                {
                    if (e.IsValid)
                    {
                        if (string.IsNullOrEmpty(detailDomain.PROD_DIR_QTY + ""))
                        {
                            detailDomain.PROD_DIR_QTY = 0;
                        }
                        //
                        if (!detailDomain.PROD_DIR_QTY.Equals((e.Value == null ? "" : e.Value.ToString())))
                        {
                            double? tmpInt = Convert.ToDouble(e.Value.ToString());

                            // 잘못된 값
                            if (tmpInt <= 0) return;

                            detailDomain.PROD_DIR_QTY = tmpInt;

                            if (detailDomain.PROD_TK_TM != null)
                            {
                                if (Convert.ToDouble(detailDomain.PROD_TK_TM.ToString()) >= 0)
                                {
                                    // 생산소요시간 = 렉타임 / 지시수량
                                    detailDomain.PROD_WRK_TM = Convert.ToDouble(detailDomain.PROD_TK_TM.ToString()) / Convert.ToDouble(detailDomain.PROD_DIR_QTY.ToString());
                                }
                            }

                        }
                    }
                }
                else
                {
                    // 적용 안함.
                    return;
                }


                // 변경 적용하기
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m661010/dtl/u"
                                                                                                  , new StringContent(JsonConvert.SerializeObject(detailDomain), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        int _Num = 0;

                        string result = await response.Content.ReadAsStringAsync();
                        if (int.TryParse(result, out _Num) == false)
                        {
                            //실패
                            WinUIMessageBox.Show(result, _title, MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    }
                }



                //성공
                this.ViewGridDtl.RefreshData();
            }
            catch (Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, _title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
