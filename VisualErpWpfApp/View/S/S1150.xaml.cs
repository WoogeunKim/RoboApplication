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
    /// Interaction logic for S1150.xaml
    /// </summary>
    public partial class S1150 : UserControl
    {
        public S1150()
        {
            InitializeComponent();
            DataContext = new S1150ViewModel();

        }
    }
}
