using System.Windows.Controls;
using AquilaErpWpfApp3.ViewModel;

namespace AquilaErpWpfApp3.View.S
{
    public partial class S1414 : UserControl
    {
        private string title = "품목 그룹 관리";
        public S1414()
        {
            DataContext = new S1414ViewModel();
            //
            InitializeComponent();

            //this.txt_Master_Search.KeyDown += new KeyEventHandler(txt_Master_Search_KeyDown);
            //this.btn_ConfigViewPage_Master_search.Click += new RoutedEventHandler(btn_Master_search_Click);
            //
            //this.txt_Detail_Search.KeyDown += new KeyEventHandler(txt_Detail_Search_KeyDown);
            //this.btn_ConfigViewPage_Detail_search.Click += new RoutedEventHandler(btn_Detail_search_Click);
        }
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
        //void txt_Master_Search_KeyDown(Object sender, KeyEventArgs e)
        //{
        //    if (e.Key == Key.Enter)
        //    {
        //        Master_Search(this.txt_Master_Search.Text, true);
        //    }
        //}

        //void btn_Master_search_Click(object sender, RoutedEventArgs e)
        //{
        //    Master_Search(this.txt_Master_Search.Text, true);
        //}

        //void Master_Search(string scarch, bool isSearch)
        //{
        //    try
        //    {
        //        this.ConfigViewPage1Edit_Master.ShowLoadingPanel = true;
        //        if (isSearch)
        //        {
        //            this.configViewPage1EditView_Master.SearchString = scarch;
        //            this.txt_Master_Search.SelectAll();
        //            this.txt_Master_Search.Focus();
        //        }
        //        this.ConfigViewPage1Edit_Master.ShowLoadingPanel = false;
        //    }
        //    catch (Exception eLog)
        //    {
        //        this.ConfigViewPage1Edit_Master.ShowLoadingPanel = false;
        //        WinUIMessageBox.Show(eLog.Message, "[에러]품목 그룹 관리", MessageBoxButton.OK, MessageBoxImage.Error);
        //        this.txt_Master_Search.SelectAll();
        //        this.txt_Master_Search.Focus();
        //        return;
        //    }
        //}
        #endregion

        #region Functon (Detail Search)
        //private void D_REFRESH_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        //{
        //    try
        //    {
        //        this.ConfigViewPage1Edit_Detail.ShowLoadingPanel = true;
        //        this.configViewPage1EditView_Detail.SearchString = (this.D_SEARCH_TEXT.EditValue == null ? "" : this.D_SEARCH_TEXT.EditValue.ToString());
        //        //this.txt_Search.SelectAll();
        //        this.D_SEARCH_TEXT.Focus();
        //        this.ConfigViewPage1Edit_Detail.ShowLoadingPanel = false;

        //        //((S131ViewModel)this.DataContext).setTitle();
        //    }
        //    catch (Exception eLog)
        //    {
        //        this.ConfigViewPage1Edit_Detail.ShowLoadingPanel = false;
        //        WinUIMessageBox.Show(eLog.Message, "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
        //        //this.M_SEARCH_TEXT.SelectAll();
        //        this.D_SEARCH_TEXT.Focus();
        //        return;
        //    }
        //}

        //private void D_SEARCH_TEXT_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        //{
        //    if (e.Key == Key.Enter)
        //    {
        //        D_REFRESH_ItemClick(sender, null);
        //    }
        //}

        //void txt_Detail_Search_KeyDown(Object sender, KeyEventArgs e)
        //{
        //    if (e.Key == Key.Enter)
        //    {
        //        Detail_Search(this.txt_Detail_Search.Text, true);
        //    }
        //}

        //void btn_Detail_search_Click(object sender, RoutedEventArgs e)
        //{
        //    Detail_Search(this.txt_Detail_Search.Text, true);
        //}

        //void Detail_Search(string scarch, bool isSearch)
        //{
        //    try
        //    {
        //        this.ConfigViewPage1Edit_Detail.ShowLoadingPanel = true;
        //        if (isSearch)
        //        {
        //            this.configViewPage1EditView_Detail.SearchString = scarch;
        //            this.txt_Detail_Search.SelectAll();
        //            this.txt_Detail_Search.Focus();
        //        }
        //        this.ConfigViewPage1Edit_Detail.ShowLoadingPanel = false;
        //    }
        //    catch (Exception eLog)
        //    {
        //        this.ConfigViewPage1Edit_Detail.ShowLoadingPanel = false;
        //        WinUIMessageBox.Show(eLog.Message, "[에러]품목 그룹 관리", MessageBoxButton.OK, MessageBoxImage.Error);
        //        this.txt_Detail_Search.SelectAll();
        //        this.txt_Detail_Search.Focus();
        //        return;
        //    }
        //}
        #endregion
    }
}