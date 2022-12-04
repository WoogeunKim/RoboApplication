using System.Windows.Controls;
using AquilaErpWpfApp3.ViewModel;

namespace AquilaErpWpfApp3.View.INV
{
    public partial class I66101 : UserControl
    {
        private string _title = "발주입고";

        public I66101()
        {
            DataContext = new I66101ViewModel();
            //
            InitializeComponent();

        }
    }
}