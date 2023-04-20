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
                bool prodDirQty = (e.Column.FieldName.ToString().Equals("PROD_DIR_QTY") ? true : false);     // 수량
                bool n1st_eq_no = (e.Column.FieldName.ToString().Equals("N1ST_EQ_NM") ? true : false);       // 절단설비
                bool n2nd_eq_no = (e.Column.FieldName.ToString().Equals("N2ND_EQ_NM") ? true : false);       // 가공설비

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

                            if (string.IsNullOrEmpty(detailDomain.N1ST_EQ_NM) || string.IsNullOrEmpty(detailDomain.N1ST_EQ_NM))
                            {

                            }

                            detailDomain.PROD_TK_TM = tmpInt;

                            if(detailDomain.PROD_DIR_QTY != null)
                            {
                                if(Convert.ToDouble(detailDomain.PROD_DIR_QTY.ToString()) > 0)
                                {
                                    // 생산소요시간 = 렉타임 / 지시수량
                                    detailDomain.PROD_WRK_TM = Convert.ToDouble(detailDomain.PROD_TK_TM.ToString()) * Convert.ToDouble(detailDomain.PROD_DIR_QTY.ToString());
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
                                    // 생산소요시간 = 택타임(제품 한개 생산시간) * 지시수량
                                    detailDomain.PROD_WRK_TM = Convert.ToDouble(detailDomain.PROD_TK_TM.ToString()) * Convert.ToDouble(detailDomain.PROD_DIR_QTY.ToString());
                                }
                            }

                        }
                    }
                }
                else if(n1st_eq_no)
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
                else
                {
                    // 적용 안함.
                    return;
                }


                // 서버의 정보를 수정합니다.
                if (prodDieDt || prodTkTm || prodDirQty)
                {
                    // 지시일자, 택타임, 지시수량 변경
                    await PostJsonUpdate("m661010/dtl/u", detailDomain);
                }
                else if (n1st_eq_no || n2nd_eq_no)
                {
                    // 절단설비, 가공설비 변경
                    await PostJsonUpdate("m66311/eq/u", detailDomain);
                }


                //성공
                this.ViewGridDtl.RefreshData();
            }
            catch (Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, _title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


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
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync(Path
                                                                                                  , new StringContent(JsonConvert.SerializeObject(obj), System.Text.Encoding.UTF8, "application/json")))
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

    }
}
