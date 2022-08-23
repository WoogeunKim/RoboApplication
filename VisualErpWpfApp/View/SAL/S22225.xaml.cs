using System.Windows.Controls;
using AquilaErpWpfApp3.ViewModel;

namespace AquilaErpWpfApp3.View.SAL
{
    /// <summary>
    /// S22225.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class S22225 : UserControl
    {
        private string title = "GR번호 관리";
        public S22225()
        {
            InitializeComponent();

            DataContext = new S22225ViewModel();

            //DevExpress.Data.ShellHelper.TryCreateShortcut("sample_notification_app", "DXSampleNotificationSevice");
        }

    }
}
