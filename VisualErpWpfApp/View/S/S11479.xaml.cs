using System.Windows;
using System.Windows.Controls;
using AquilaErpWpfApp3.ViewModel;
using ModelsLibrary.Code;

namespace AquilaErpWpfApp3.View.S
{
    public partial class S11479 : UserControl
    {
        private string title = "납품처관리";

        //private string isDetailView;


        public S11479()
        {
            InitializeComponent();

            DataContext = new S11479ViewModel();
        }

    }
}