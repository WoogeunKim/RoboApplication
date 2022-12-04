using System.Windows;
using System.Windows.Controls;
using AquilaErpWpfApp3.ViewModel;


namespace AquilaErpWpfApp3.View.M
{
    /// <summary>
    /// S2233.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class M6722 : UserControl
    {
        private string title = "실적현황";

        public M6722()
        {
            InitializeComponent();

            DataContext = new M6722ViewModel();

        }
    }
}
