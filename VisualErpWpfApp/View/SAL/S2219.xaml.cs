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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AquilaErpWpfApp3.View.SAL
{
    /// <summary>
    /// S2219.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class S2219 : UserControl
    {
        public S2219()
        {
            DataContext = new S2219ViewModel();


            InitializeComponent();
        }
    }
}
