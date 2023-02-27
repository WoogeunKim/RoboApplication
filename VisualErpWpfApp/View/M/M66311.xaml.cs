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
    /// M66311.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class M66311 : UserControl
    {
        private string _title = "가공생산지시관리";
        public M66311()
        {
            InitializeComponent();

            DataContext = new M66311ViewModel();
        }
    }
}