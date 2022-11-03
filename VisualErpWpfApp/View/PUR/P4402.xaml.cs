using DevExpress.Xpf.Editors;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Pur;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using AquilaErpWpfApp3.ViewModel;

namespace AquilaErpWpfApp3.View.PUR
{
    /// <summary>
    /// P4402.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class P4402 : UserControl
    {
        public P4402()
        {
            DataContext = new P4402ViewModel();

            InitializeComponent();
        }
    }
}
