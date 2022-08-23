using System.Windows;
using System.Windows.Controls;
using AquilaErpWpfApp3.ViewModel;

namespace AquilaErpWpfApp3.View.S
{
    public partial class S1413 : UserControl
    {
        //private static SaleOrderServiceClient saleOrderClient = SystemProperties.SaleOrderClient;
        private string title = "이형봉강 조견표";

        //private string isDetailView;


        public S1413()
        {
            InitializeComponent();

            //this.ViewGridMst.SelectedItemChanged += ViewGridMst_SelectedItemChanged;
            //
            DataContext = new S1413ViewModel();
            
        }

        //void ViewGridMst_SelectedItemChanged(object sender, DevExpress.Xpf.Grid.SelectedItemChangedEventArgs e)
        //{
        //    this.isDetailView = ((S2221ViewModel)this.DataContext).DetailView;

        //    if (isDetailView.Equals("1"))
        //    {
        //        this.DetailView1.Visibility = System.Windows.Visibility.Visible;
        //        this.DetailView2.Visibility = System.Windows.Visibility.Hidden;
        //        this.DetailView1.Content = ((S2221ViewModel)this.DataContext).SelectedMstItem;
        //    }
        //    else if (isDetailView.Equals("2"))
        //    {
        //        this.DetailView1.Visibility = System.Windows.Visibility.Hidden;
        //        this.DetailView2.Visibility = System.Windows.Visibility.Visible;
        //        this.DetailView2.Content = ((S2221ViewModel)this.DataContext).SelectedMstItem;
        //    }
        //    else
        //    {
        //        this.DetailView1.Visibility = System.Windows.Visibility.Visible;
        //        this.DetailView2.Visibility = System.Windows.Visibility.Hidden;
        //        this.DetailView1.Content = ((S2221ViewModel)this.DataContext).SelectedMstItem;
        //    }
        //}

        private void CheckEdit_Checked(object sender, RoutedEventArgs e)
        {
            //CheckEdit checkEdit = sender as CheckEdit;
            //JobVo tmpImsi;
            //JobVo resultVo;
            //for (int x = 0; x < this.ViewGridDtl.VisibleRowCount; x++)
            //{
            //    int rowHandle = this.ViewGridDtl.GetRowHandleByVisibleIndex(x);
            //    if (rowHandle > -1)
            //    {
            //        tmpImsi = this.ViewGridDtl.GetRow(rowHandle) as JobVo;
            //        if (checkEdit.IsChecked == true)
            //        {
            //            tmpImsi.CLZ_FLG = "Y";
            //            tmpImsi.isCheckd = true;
            //        }
            //        else
            //        {
            //            tmpImsi.CLZ_FLG = "N";
            //            tmpImsi.isCheckd = false;
            //        }
            //        //
            //        tmpImsi.CRE_USR_ID = SystemProperties.USER;
            //        tmpImsi.UPD_USR_ID = SystemProperties.USER;
            //        //
            //        //
            //        resultVo = saleOrderClient.S2211UpdateDtl(tmpImsi);
            //        if (!resultVo.isSuccess)
            //        {
            //            //실패
            //            WinUIMessageBox.Show(resultVo.Message, "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
            //            return;
            //        }
            //    }
            //}
        }

        //private void M_REFRESH_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        //{
        //    try
        //    {
        //        this.ViewGridMst.ShowLoadingPanel = true;
        //        this.ViewTableMst.SearchString = (this.M_SEARCH_TEXT.EditValue == null ? "" : this.M_SEARCH_TEXT.EditValue.ToString());
        //        //this.txt_Search.SelectAll();
        //        this.M_SEARCH_TEXT.Focus();
        //        this.ViewGridMst.ShowLoadingPanel = false;

        //        ((S2222ViewModel)this.DataContext).setTitle();
        //    }
        //    catch (Exception eLog)
        //    {
        //        this.ViewGridMst.ShowLoadingPanel = false;
        //        WinUIMessageBox.Show(eLog.Message, "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
        //        //this.M_SEARCH_TEXT.SelectAll();
        //        this.M_SEARCH_TEXT.Focus();
        //        return;
        //    }
        //}


        private void isCheckEdit_DtlClzFlg(object sender, RoutedEventArgs e)
        {
            //CheckEdit checkEdit = sender as CheckEdit;
            //JobVo masterDomain = (JobVo)ViewGridDtl.GetFocusedRow();
            //JobVo resultVo;
            ////
            //if (checkEdit.IsChecked == true)
            //{
            //    //Y
            //    masterDomain.CLZ_FLG = "Y";
            //    masterDomain.isCheckd = true;
            //}
            //else
            //{
            //    //N
            //    masterDomain.CLZ_FLG = "N";
            //    masterDomain.isCheckd = false;
            //}

            ////
            //masterDomain.CRE_USR_ID = SystemProperties.USER;
            //masterDomain.UPD_USR_ID = SystemProperties.USER;
            ////
            ////
            //resultVo = saleOrderClient.S2211UpdateDtl(masterDomain);
            //if (!resultVo.isSuccess)
            //{
            //    //실패
            //    WinUIMessageBox.Show(resultVo.Message, "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
            //    return;
            //}
        }

        //private void CheckEdit_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        //{
        //    FrameworkElement editor = (FrameworkElement)sender;
        //    GridGroupValueData data = (GridGroupValueData)editor.DataContext;

        //    GridControl grid = ((TableView)data.View).Grid;
        //    int currentGroupRowHandle = data.RowData.RowHandle.Value;

        //    int childRowsCount = grid.GetChildRowCount(currentGroupRowHandle);
        //    for (int index = 0; index < childRowsCount; index++)
        //    {
        //        int childRowHandle = grid.GetChildRowHandle(currentGroupRowHandle, index);
        //        if (grid.IsGroupRowHandle(childRowHandle))
        //        {
        //            GroupRowSelectionHelper helper = (GroupRowSelectionHelper)grid.GetValue(GroupRowSelectionHelper.GroupRowSelectionHelperProperty);
        //            helper.SetNewIsSelectedValue(childRowHandle, (bool)e.NewValue);
        //        }
        //        else
        //        {
        //            grid.SetCellValue(childRowHandle, "IsSelected", e.NewValue);
        //        }
        //    }
        //}

        //private void M_SAVE_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        //{
        //    //this.ViewTableDtl.Focus();
        //}

        //private void M_SEARCH_TEXT_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        //{
        //    if (e.Key == Key.Enter)
        //    {
        //        M_REFRESH_ItemClick(sender, null);
        //    }
        //}

    }
}