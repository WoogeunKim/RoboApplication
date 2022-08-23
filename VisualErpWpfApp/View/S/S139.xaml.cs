using System.Windows.Controls;
using AquilaErpWpfApp3.ViewModel;

namespace AquilaErpWpfApp3.View.S
{
    public partial class S139 : UserControl
    {
        private string _title = "프로그램 메뉴 관리";

        public S139()
        {
            DataContext = new S139ViewModel();
            InitializeComponent();

            //this.ConfigViewPage1Edit_Master.MouseDoubleClick += ConfigViewPage1Edit_Master_MouseDoubleClick;
            //this.GridEditView_menu.ExpandAllNodes();
        }

        //void ConfigViewPage1Edit_Master_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        //{
            //TreeListNode node = this.GridEditView_menu.FocusedNode;
            //try
            //{
            //    if (node.IsExpanded)
            //    {
            //        node.CollapseAll();
            //    }
            //    else
            //    {
            //        node.ExpandAll();
            //    }
            //}
            //catch (Exception eLog)
            //{
            //    WinUIMessageBox.Show(eLog.Message, "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
            //    return;
            //}
        //}

        //private void Menu_ref_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        //{
        //    try
        //    {
        //        GridEditView_menu.ExpandAllNodes();
        //    }
        //    catch (Exception eLog)
        //    {
        //        WinUIMessageBox.Show(eLog.Message, "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
        //        return;
        //    }

        //}
    }
}