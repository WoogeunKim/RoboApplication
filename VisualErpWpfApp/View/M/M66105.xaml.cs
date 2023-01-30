using AquilaErpWpfApp3.ViewModel;
using System.Windows.Controls;

namespace AquilaErpWpfApp3.View.M
{
    /// <summary>
    /// M66105.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class M66105 : UserControl
    {
        private string title = "BarList 추출검증";

        public M66105()
        {
            InitializeComponent();

            DataContext = new M66105ViewModel();
        }
    }
}
