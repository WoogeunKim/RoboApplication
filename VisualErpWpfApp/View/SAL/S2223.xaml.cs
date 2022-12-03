using System;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using AquilaErpWpfApp3.Util;
using AquilaErpWpfApp3.ViewModel;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Sale;
using Newtonsoft.Json;

namespace AquilaErpWpfApp3.View.SAL
{
    public partial class S2223 : UserControl
    {
        private string title = "수주 등록";

        public S2223()
        {
            DataContext = new S2223ViewModel();
            //
            InitializeComponent();

        }

        private async void GridColumn_Validate(object sender, DevExpress.Xpf.Grid.GridCellValidationEventArgs e)
        {
            SaleVo masterDomain = (SaleVo)ViewGridDtl.GetFocusedRow();

            string number = string.Empty;

            number = e.Value.ToString();

            try
            {
                bool slItmPrcDcRt = (e.Column.FieldName.ToString().Equals("SL_ITM_PRC_DC_RT") ? true : false);
                bool dcPrcVal = (e.Column.FieldName.ToString().Equals("SL_ITM_PRC_VAL") ? true : false);

                if (slItmPrcDcRt)
                {
                    if (e.IsValid)
                    {
                        masterDomain.SL_ITM_PRC_DC_RT = Math.Round(double.Parse(e.Value.ToString()), 2);
                    }
                }

                else if (dcPrcVal)
                {
                    if (e.IsValid)
                    {
                        masterDomain.SL_ITM_PRC_VAL = Math.Round(double.Parse(e.Value.ToString()), 2);
                    }
                }


                if (slItmPrcDcRt || dcPrcVal)
                {
                    if (double.Parse(masterDomain.SL_ITM_PRC_VAL.ToString()) > 0)
                    {
                        masterDomain.SL_ITM_AMT = Math.Round((double.Parse(masterDomain.SL_ITM_PRC_VAL.ToString()) * ((100 - double.Parse(masterDomain.SL_ITM_PRC_DC_RT.ToString()))/100) * double.Parse(masterDomain.SL_ITM_QTY.ToString())), 3);
                        
                       
                    }

                    SaleVo dtlVo = new SaleVo();
                    dtlVo = masterDomain;
                    dtlVo.UPD_USR_ID = SystemProperties.USER;

                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s2223/dtl/u", new StringContent(JsonConvert.SerializeObject(dtlVo), System.Text.Encoding.UTF8, "application/json")))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            int _Num = 0;
                            string resultMsg = await response.Content.ReadAsStringAsync();
                            if (int.TryParse(resultMsg, out _Num) == false)
                            {
                                //실패
                                WinUIMessageBox.Show(resultMsg, "수주등록 물품 내역 수정", MessageBoxButton.OK, MessageBoxImage.Error);
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

    }
}