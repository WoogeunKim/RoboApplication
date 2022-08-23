using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Sale;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Windows;
using AquilaErpWpfApp3.Util;

namespace AquilaErpWpfApp3.View.SAL.Dialog
{
    /// <summary>
    /// Interaction logic for S2225BillDialog.xaml
    /// </summary>
    public partial class S22313BillDialog : DXWindow
    {
        private string title = "계산서 발행 (건별)";

        private SaleVo orgVo;
        private IList<SaleVo> MstList = new List<SaleVo>();
        //private IList<SaleVo> ChkItemsList = new List<SaleVo>();

        public S22313BillDialog(SaleVo _vo)
        {
            InitializeComponent();

            this.orgVo = _vo;

            Refresh();
        }

        private void CheckEdit_Checked(object sender, RoutedEventArgs e)
        {
            CheckEdit checkEdit = sender as CheckEdit;
            SaleVo tmpImsi;
            int rowHandle;
            for (int x = 0; x < this.MasterGrid.VisibleRowCount; x++)
            {
                rowHandle = this.MasterGrid.GetRowHandleByVisibleIndex(x);
                if (rowHandle > -1)
                {
                    tmpImsi = this.MasterGrid.GetRow(rowHandle) as SaleVo;
                    if (checkEdit.IsChecked == true)
                    {
                        tmpImsi.isCheckd = true;
                    }
                    else
                    {
                        tmpImsi.isCheckd = false;
                    }
                    this.MasterGrid.RefreshRow(rowHandle);
                }
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }



        public async void Refresh()
        {
            try
            {
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s22313/mst/bill", new StringContent(JsonConvert.SerializeObject(this.orgVo), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        MstList = JsonConvert.DeserializeObject<IEnumerable<SaleVo>>(await response.Content.ReadAsStringAsync()).Cast<SaleVo>().ToList();
                    }
                    //this.MstList = saleOrderClient.S2225SelectDialogAllList(new JobVo() { SL_DC_YRMON = this.SlDcYrMon, AREA_CD = this.AreaCd });

                    //for (int x = 0; x < MstList.Count; x++)
                    //{
                    //    if (MstList[x].CHK_FLG == "Y")
                    //    {
                    //        MstList[x].CHK_FLG = "True";


                    //    }
                    //    else if (MstList[x].CHK_FLG == "N")
                    //    {
                    //        MstList[x].CHK_FLG = "False";
                    //    }
                    //}

                    this.MasterGrid.ItemsSource = MstList;
                    //this.MasterGrid.SelectedItems = ChkItemsList;
                }
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[에러]" + this.title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }

        private async void btn_search_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                IList<SaleVo> selectItms = MstList.Where<SaleVo>(x => x.isCheckd == true).ToList<SaleVo>();
                MessageBoxResult result = WinUIMessageBox.Show("정말로 계산서 발행을 하시겠습니까?", this.title, MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s22313/mst/bill/crt", new StringContent(JsonConvert.SerializeObject(selectItms), System.Text.Encoding.UTF8, "application/json")))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            JsonConvert.DeserializeObject<int?>(await response.Content.ReadAsStringAsync());
                        }


                        //DXSplashScreen.Show<ProgressWindow>();

                        //JobVo resultVo;

                        //for (int x = 0; x < SelectedItemsMst.Count; x++)
                        //{
                        //    resultVo = saleOrderClient.ProcS2225(new JobVo() { AREA_CD = this.AreaCd, SL_DC_YRMON = SlDcYrMon, CO_CD = SelectedItemsMst[x].CO_NO, CRE_USR_ID = SystemProperties.USER });
                        //    if (resultVo.isSuccess == false)
                        //    {
                        //        WinUIMessageBox.Show(resultVo.Message, "[계산서생성]", MessageBoxButton.OK, MessageBoxImage.Warning);
                        //    }
                        //}

                        //DXSplashScreen.Close();
                        WinUIMessageBox.Show("전자계산서 집계완료했습니다", this.title, MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                Refresh();

            }
            catch (System.Exception eLog)
            {
                //DXSplashScreen.Close();
                WinUIMessageBox.Show(eLog.Message, "[에러]" + this.title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }
    }
}
