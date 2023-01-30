using AquilaErpWpfApp3.ViewModel;
using System.Windows.Controls;

namespace AquilaErpWpfApp3.View.SAL
{
    /// <summary>
    /// S32210.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class S32210 : UserControl
    {
        private string title = "고객발주현황";

        public S32210()
        {
            InitializeComponent();

            DataContext = new S32210ViewModel();
        }
    }
}
