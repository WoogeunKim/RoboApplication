using AquilaErpWpfApp3.ViewModel;
using System.Windows.Controls;

namespace AquilaErpWpfApp3.View.TEC
{
    /// <summary>
    /// T6311.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class T6311 : UserControl
    {
        public T6311()
        {
            InitializeComponent();
            DataContext = new T6311ViewModel();
        }
    }
}
