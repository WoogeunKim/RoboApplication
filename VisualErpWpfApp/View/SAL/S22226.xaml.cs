using System.Windows.Controls;
using AquilaErpWpfApp3.ViewModel;

namespace AquilaErpWpfApp3.View.SAL
{
    /// <summary>
    /// S22225.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class S22226 : UserControl
    {
        private string title = "생산최적화";
        public S22226()
        {
            InitializeComponent();

            DataContext = new S22226ViewModel();

            //DevExpress.Data.ShellHelper.TryCreateShortcut("sample_notification_app", "DXSampleNotificationSevice");
        }

    }
}
