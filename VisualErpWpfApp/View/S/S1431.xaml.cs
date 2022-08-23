using AquilaErpWpfApp3.ViewModel;
using System.Windows.Controls;

namespace AquilaErpWpfApp3.View.S
{
    public partial class S1431 : UserControl
    {

        private string title = "차량 마스터 등록";

        public S1431()
        {
            DataContext = new S1431ViewModel();
            InitializeComponent();

            
        }
    }
}