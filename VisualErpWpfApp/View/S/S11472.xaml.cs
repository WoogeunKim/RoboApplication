using AquilaErpWpfApp3.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AquilaErpWpfApp3.View.S
{
    /// <summary>
    /// Interaction logic for S11472.xaml
    /// </summary>
    public partial class S11472 : UserControl
    {
        public S11472()
        {
            InitializeComponent();
            DataContext = new S11472ViewModel();

        }
    }
}
