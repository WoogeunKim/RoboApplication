using AquilaErpWpfApp3.ViewModel;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using ModelsLibrary.Sale;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;


namespace AquilaErpWpfApp3.View.SAL
{
    public partial class S22227 : UserControl
    {
        private string title = "GR번호/LOC관리";

        public S22227()
        {
            DataContext = new S22227ViewModel();
            //
            InitializeComponent();

            this.ViewGridMst.MouseLeftButtonUp += ViewGridMst_MouseLeftButtonUp;
            this.ViewGridDtl.MouseLeftButtonUp += VeiwGridDtl_MouseLeftButtonUp;
        }

        private void ViewGridMst_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.ViewGridMst.RefreshData();
        }


        private void VeiwGridDtl_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.ViewGridDtl.RefreshData();
        }



        private void CheckEdit_Checked(object sender, RoutedEventArgs e)
        {
            CheckEdit checkEdit = sender as CheckEdit;

            SetCheckState(this.ViewGridMst, checkEdit.IsChecked == true);
            SetCheckState(this.ViewGridDtl, checkEdit.IsChecked == true);


        }

        private void SetCheckState(GridControl gridView, bool isChecked)
        {
            for (int x = 0; x < gridView.VisibleRowCount; x++)
            {
                int rowHandle = gridView.GetRowHandleByVisibleIndex(x);
                if (rowHandle > -1)
                {
                    SaleVo tmpImsi = gridView.GetRow(rowHandle) as SaleVo;
                    if (tmpImsi != null)
                    {
                        tmpImsi.isCheckd = isChecked;
                    }
                }
            }
        }

        private void AllCheckBox_Mst_Checked(object sender, RoutedEventArgs e)
        {
            if (this.DataContext is S22227ViewModel viewModel)
            {
                viewModel.SelectDetailRefresh();
            }
        }

        private void AllCheckBox_DTL_Checked(object sender, RoutedEventArgs e)
        {
            if (this.DataContext is S22227ViewModel viewModel)
            {
                viewModel.DtlList_Activity();
            }
        }
    }
}