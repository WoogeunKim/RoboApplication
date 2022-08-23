using System.Threading.Tasks;
using AquilaErpWpfApp3.ViewModel;

namespace AquilaErpWpfApp3.View.M
{
    public partial class M6623 : System.Windows.Controls.UserControl
    {
        private string title = "레시피자료관리";

        public M6623()
        {
            DataContext = new M6623ViewModel();
            //
            InitializeComponent();
        }

        //private void M_up_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        //{
        //    ((M6623ViewModel)this.DataContext).Up();
        //    this.ConfigViewPage1Edit_Detail.RefreshData();
        //    //this.ConfigViewPage1Edit_Detail.SelectedItem = ((M6623ViewModel)this.DataContext).SearchDetailJob;
        //}

        //private void M_down_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        //{
        //    ((M6623ViewModel)this.DataContext).Down();
        //    this.ConfigViewPage1Edit_Detail.RefreshData();
        //    //this.ConfigViewPage1Edit_Detail.SelectedItem = ((M6623ViewModel)this.DataContext).SearchDetailJob;
        //}


        #region Functon (Master Search)
        //private void M_REFRESH_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        //{
        //    try
        //    {
        //        this.ConfigViewPage1Edit_Master.ShowLoadingPanel = true;
        //        this.configViewPage1EditView_Master.SearchString = (this.M_SEARCH_TEXT.EditValue == null ? "" : this.M_SEARCH_TEXT.EditValue.ToString());
        //        //this.txt_Search.SelectAll();
        //        this.M_SEARCH_TEXT.Focus();
        //        this.ConfigViewPage1Edit_Master.ShowLoadingPanel = false;

        //        //((S131ViewModel)this.DataContext).setTitle();
        //    }
        //    catch (Exception eLog)
        //    {
        //        this.ConfigViewPage1Edit_Master.ShowLoadingPanel = false;
        //        WinUIMessageBox.Show(eLog.Message, "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
        //        //this.M_SEARCH_TEXT.SelectAll();
        //        this.M_SEARCH_TEXT.Focus();
        //        return;
        //    }
        //}

        //private void M_SEARCH_TEXT_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        //{
        //    if (e.Key == Key.Enter)
        //    {
        //        M_REFRESH_ItemClick(sender, null);
        //    }
        //}
        #endregion

        //private void M_Refresh_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        //{
        //    this.ConfigViewPage1Edit_Detail.RefreshData();
        //}
    }
}