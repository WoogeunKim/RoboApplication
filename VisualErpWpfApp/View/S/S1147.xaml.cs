using System.Windows.Controls;
using AquilaErpWpfApp3.ViewModel;

namespace AquilaErpWpfApp3.View.S
{
    /// <summary>
    /// S1147.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class S1147 : UserControl
    {
        public S1147()
        {
            DataContext = new S1147ViewModel();
            InitializeComponent();
        }
    }
}
