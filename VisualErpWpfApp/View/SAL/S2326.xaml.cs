using DevExpress.Xpf.Editors;
using ModelsLibrary.Sale;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using AquilaErpWpfApp3.ViewModel;

namespace AquilaErpWpfApp3.View.SAL
{
    public partial class S2326 : UserControl
    {
        //private static SaleOrderServiceClient saleOrderClient = SystemProperties.SaleOrderClient;

        public S2326()
        {
            DataContext = new S2326ViewModel();
            //
            InitializeComponent();


            this.ViewGridMst.MouseDoubleClick += ViewGridMst_MouseDoubleClick;
        }

        void ViewGridMst_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SaleVo masterDomain = (SaleVo)this.ViewGridMst.GetFocusedRow();
            if (masterDomain != null)
            {
                if (masterDomain.isCheckd == false)
                {
                    masterDomain.isCheckd = true;
                }
                else
                {
                    masterDomain.isCheckd = false;
                }
            }
        }

        private void CheckEdit_Checked(object sender, RoutedEventArgs e)
        {
            CheckEdit checkEdit = sender as CheckEdit;
            SaleVo tmpImsi;
            //JobVo resultVo;
            for (int x = 0; x < this.ViewGridMst.VisibleRowCount; x++)
            {
                int rowHandle = this.ViewGridMst.GetRowHandleByVisibleIndex(x);
                if (rowHandle > -1)
                {
                    tmpImsi = this.ViewGridMst.GetRow(rowHandle) as SaleVo;
                    if (checkEdit.IsChecked == true)
                    {
                        //tmpImsi.CLZ_FLG = "Y";
                        tmpImsi.isCheckd = true;
                    }
                    else
                    {
                        //tmpImsi.CLZ_FLG = "N";
                        tmpImsi.isCheckd = false;
                    }
                    //
                    //tmpImsi.CRE_USR_ID = SystemProperties.USER;
                    //tmpImsi.UPD_USR_ID = SystemProperties.USER;
                    //
                    //
                    //resultVo = saleOrderClient.S2211UpdateDtl(tmpImsi);
                    //if (!resultVo.isSuccess)
                    //{
                    //    //실패
                    //    WinUIMessageBox.Show(resultVo.Message, "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
                    //    return;
                    //}
                }
            }
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

    }
}