using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using DevExpress.Xpf;
using DevExpress.Xpf.Core.Native;

namespace AquilaErpWpfApp3.Utillity
{
    public partial class IEG_InfoView : UserControl {
        public IEG_InfoView() {
            InitializeComponent();
            this.DataContext = new InfoViewModel();
        }
    }
    public class AboutInfoToControlAboutConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            ControlAbout control = new ControlAbout((AboutInfo)value);
            control.Loaded += OnControlLoaded;
            return control;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotSupportedException();
        }
        static void OnControlLoaded(object sender, RoutedEventArgs e) {
            ControlAbout control = (ControlAbout)sender;
            LayoutHelper.FindElementByName(control, "CloseButton").Visibility = Visibility.Collapsed;
        }
    }
}
