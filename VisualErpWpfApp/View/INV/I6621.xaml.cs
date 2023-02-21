using System.Windows.Controls;
using AquilaErpWpfApp3.ViewModel;

namespace AquilaErpWpfApp3.View.INV
{
    public partial class I6621 : UserControl
    {
        private string title = "품목그룹별 재고장";
        public I6621()
        {
            DataContext = new I6621ViewModel();
            InitializeComponent();
        }
    }
}