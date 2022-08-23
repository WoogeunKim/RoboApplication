using AquilaErpWpfApp3.ViewModel;
using System.Windows.Controls;

namespace AquilaErpWpfApp3.View.TEC
{
    /// <summary>
    /// T6311.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class T6325 : UserControl
    {
        public T6325()
        {
            InitializeComponent();
            DataContext = new T6325ViewModel();
        }
    }
}
