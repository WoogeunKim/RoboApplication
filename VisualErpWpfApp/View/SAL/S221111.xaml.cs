using System.Windows;
using System.Windows.Controls;
using AquilaErpWpfApp3.ViewModel;
using ModelsLibrary.Sale;
using DevExpress.Xpf.Editors;

namespace AquilaErpWpfApp3.View.SAL
{
    public partial class S221111 : UserControl
    {
        ///private static SaleOrderServiceClient saleOrderClient = SystemProperties.SaleOrderClient;
        private string title = "출하등록";
        //private IList<CodeDao> UserList;

        public S221111()
        {
            DataContext = new S221111ViewModel();
            //
            InitializeComponent();
            this.M_DTL_INSERT.IsEnabled = false;
            this.ViewGridDtl.MouseLeftButtonUp += VeiwGridDtl_MouseLeftButtonUp;
        }

        private void VeiwGridDtl_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.ViewGridDtl.RefreshData();
        }

        private void CheckEdit_Checked(object sender, RoutedEventArgs e)
        {
            CheckEdit checkEdit = sender as CheckEdit;
            SaleVo tmpImsi;
            for (int x = 0; x < this.ViewGridDtl.VisibleRowCount; x++)
            {
                int rowHandle = this.ViewGridDtl.GetRowHandleByVisibleIndex(x);
                if (rowHandle > -1)
                {
                    tmpImsi = this.ViewGridDtl.GetRow(rowHandle) as SaleVo;
                    if (checkEdit.IsChecked == true)
                    {
                        tmpImsi.isCheckd = true;
                        //
                        this.M_DTL_INSERT.IsEnabled = true;
                    }
                    else
                    {
                        tmpImsi.isCheckd = false;
                    }

                }
            }
        }

        private void M_REFRESH_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            //try
            //{
            //    this.ViewGridMst.ShowLoadingPanel = true;
            //    this.ViewTableMst.SearchString = (this.M_SEARCH_TEXT.EditValue == null ? "" : this.M_SEARCH_TEXT.EditValue.ToString());
            //    //this.txt_Search.SelectAll();
            //    this.M_SEARCH_TEXT.Focus();
            //    this.ViewGridMst.ShowLoadingPanel = false;
            //}
            //catch (Exception eLog)
            //{
            //    this.ViewGridMst.ShowLoadingPanel = false;
            //    WinUIMessageBox.Show(eLog.Message, "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
            //    //this.M_SEARCH_TEXT.SelectAll();
            //    this.M_SEARCH_TEXT.Focus();
            //    return;
            //}
        }


        private void isCheckEdit_DtlClzFlg(object sender, RoutedEventArgs e)
        {
            //CheckEdit checkEdit = sender as CheckEdit;
            //JobVo masterDomain = (JobVo)ViewGridDtl.GetFocusedRow();
            //JobVo resultVo;
            ////
            //if (checkEdit.IsChecked == true)
            //{
            //    //Y
            //    masterDomain.CLZ_FLG = "Y";
            //    masterDomain.isCheckd = true;
            //}
            //else
            //{
            //    //N
            //    masterDomain.CLZ_FLG = "N";
            //    masterDomain.isCheckd = false;
            //}

            ////
            //masterDomain.CRE_USR_ID = SystemProperties.USER;
            //masterDomain.UPD_USR_ID = SystemProperties.USER;
            ////
            ////
            //resultVo = saleOrderClient.S2211UpdateDtl(masterDomain);
            //if (!resultVo.isSuccess)
            //{
            //    //실패
            //    WinUIMessageBox.Show(resultVo.Message, "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
            //    return;
            //}
        }

        private void PART_Editor_Checked(object sender, RoutedEventArgs e)
        {
            this.M_DTL_INSERT.IsEnabled = true;
        }

    }
}
