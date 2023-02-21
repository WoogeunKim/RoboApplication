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
        private string _title = "거래처별 판가기준표";

        public M66107()
        {
            DataContext = new M66107ViewModel();

            InitializeComponent();
        }
    }
}
