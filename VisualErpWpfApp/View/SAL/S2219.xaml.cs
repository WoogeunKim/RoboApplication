using AquilaErpWpfApp3.Util;
using AquilaErpWpfApp3.ViewModel;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Sale;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AquilaErpWpfApp3.View.SAL
{
    /// <summary>
    /// S2219.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class S2219 : UserControl
    {
        public S2219()
        {
            DataContext = new S2219ViewModel();


            InitializeComponent();
        }

        private async void ViewTableDtl_CellValueChanged(object sender, DevExpress.Xpf.Grid.CellValueChangedEventArgs e)
        {
            try
            {
                bool isChange = false;
                SaleVo updEstm = new SaleVo();
                updEstm = ViewGridDtl.SelectedItem as SaleVo;
                updEstm.UPD_USR_ID = SystemProperties.USER;

                if (e.Column.FieldName == "ESTM_ITM_NM")  // 품목명
                {
                    updEstm.ESTM_ITM_NM = e.Value.ToString();
                    isChange = true;
                }

                if (e.Column.FieldName == "ESTM_ITM_SZ_NM") // 규격
                {
                    updEstm.ESTM_ITM_SZ_NM = e.Value.ToString();
                    isChange = true;
                }

                if (e.Column.FieldName == "ESTM_ITM_QTY") // 수량
                {
                    updEstm.ESTM_ITM_QTY = e.Value;
                    isChange = true;
                }

                if (e.Column.FieldName == "ITM_UT") // 단위
                {
                    updEstm.ITM_UT = e.Value.ToString();
                    isChange = true;
                }

                if (e.Column.FieldName == "ITM_PER_PRC") // 기준가
                {
                    updEstm.ITM_PER_PRC = e.Value;
                    isChange = true;
                }

                if (e.Column.FieldName == "ITM_DC_RT") // DC율
                {
                    updEstm.ITM_DC_RT = e.Value;
                    isChange = true;
                }

                if (e.Column.FieldName == "ITM_PRC") // 최종 단가
                {
                    updEstm.ITM_PRC = e.Value;
                    isChange = true;
                }

                if (e.Column.FieldName == "ITM_RMK") // 비고
                {
                    updEstm.ITM_RMK = e.Value.ToString();
                    isChange = true;
                }

                if (!isChange) return;

                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s2219/estm/u", new StringContent(JsonConvert.SerializeObject(updEstm), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        int _Num = 0;
                        string resultMsg = await response.Content.ReadAsStringAsync();
                        if (int.TryParse(resultMsg, out _Num) == false)
                        {
                            //실패
                            WinUIMessageBox.Show(resultMsg, "품목 수정", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        (DataContext as S2219ViewModel).Refresh(updEstm.ESTM_NO);

                        //ViewGridDtl.SelectedItem = (ViewGridDtl.ItemsSource as List<SaleVo>).Where(x => x.ESTM_SEQ.Equals(updEstm.ESTM_SEQ)).FirstOrDefault<SaleVo>();
                    }
                }
            }
            catch (Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "품목 수정 오류", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
    }
}