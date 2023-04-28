using AquilaErpWpfApp3.Util;
using AquilaErpWpfApp3.ViewModel;
using ModelsLibrary.Man;
using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Net.Http;
using System.Windows.Controls;

namespace AquilaErpWpfApp3.View.M
{
    /// <summary>
    /// M66107.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class M66107 : UserControl
    {
        private string _title = "Loss 최적화 수행";

        public M66107()
        {
            DataContext = new M66107ViewModel();

            InitializeComponent();
        }
    }
}
