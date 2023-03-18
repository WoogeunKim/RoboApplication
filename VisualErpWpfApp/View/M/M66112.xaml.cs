using AquilaErpWpfApp3.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AquilaErpWpfApp3.View.M
{
    /// <summary>
    /// M66112.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class M66112 : UserControl
    {
        public M66112()
        {
            InitializeComponent();

            DataContext = new M66112ViewModel();
        }
    }
}
