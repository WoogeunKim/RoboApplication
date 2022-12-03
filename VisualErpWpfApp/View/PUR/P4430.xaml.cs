using DevExpress.Xpf.Editors;
using ModelsLibrary.Pur;
using System.Windows;
using System.Windows.Controls;
using AquilaErpWpfApp3.ViewModel;

namespace AquilaErpWpfApp3.View.PUR
{
    public partial class P4430 : UserControl
    {
        private string _title = "발주 등록 관리";

        public P4430()
        {
            DataContext = new P4430ViewModel();
            InitializeComponent();

        }
        private void CheckEdit_Checked(object sender, RoutedEventArgs e)
        {
            CheckEdit checkEdit = sender as CheckEdit;
            PurVo tmpImsi;
            for (int x = 0; x < this.ViewGridDtl.VisibleRowCount; x++)
            {
                int rowHandle = this.ViewGridDtl.GetRowHandleByVisibleIndex(x);
                if (rowHandle > -1)
                {
                    tmpImsi = this.ViewGridDtl.GetRow(rowHandle) as PurVo;
                    if (checkEdit.IsChecked == true)
                    {
                        tmpImsi.isCheckd = true;
                    }
                    else
                    {
                        tmpImsi.isCheckd = false;
                    }
                }
            }
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}