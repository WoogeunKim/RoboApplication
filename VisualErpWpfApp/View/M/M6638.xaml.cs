using System.Windows.Controls;
using AquilaErpWpfApp3.ViewModel;

namespace AquilaErpWpfApp3.View.M
{
    /// <summary>
    /// S22225.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class M6638 : UserControl
    {
        private string title = "LOC관리";
        public M6638()
        {
            InitializeComponent();

            DataContext = new M6638ViewModel();

            //DevExpress.Data.ShellHelper.TryCreateShortcut("sample_notification_app", "DXSampleNotificationSevice");
        }

    }
}
