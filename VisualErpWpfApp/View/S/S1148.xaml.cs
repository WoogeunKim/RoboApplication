using System.Windows;
using System.Windows.Controls;
using AquilaErpWpfApp3.ViewModel;


namespace AquilaErpWpfApp3.View.S
{
    /// <summary>
    /// S1148.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class S1148 : UserControl
    {
        private string title = "도면기준관리";

        public S1148()
        {
            InitializeComponent();

            DataContext = new S1148ViewModel();
        }
    }
}
