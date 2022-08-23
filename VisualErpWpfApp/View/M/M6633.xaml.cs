using AquilaErpWpfApp3.ViewModel;
using DevExpress.Xpf.WindowsUI;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AquilaErpWpfApp3.View.M
{
    public partial class M6633 : UserControl
    {
        private string title = "칭량 작업";

        public M6633()
        {
            DataContext = new M6633ViewModel();
            //
            InitializeComponent();

            //this.txt_Search.KeyDown += new KeyEventHandler(txt_Search_KeyDown);
            //this.btn_ViewPage_search.Click += new RoutedEventHandler(btn_search_Click);
        }

        private void FindButton_Click(object sender, RoutedEventArgs e)
        {
            (this.DataContext as M6632ViewModel).FindContact();
        }

        #region Functon (Search)
        private void M_REFRESH_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            try
            {
                this.ViewJOBEdit.ShowLoadingPanel = true;
                this.viewJOBEditView.SearchString = (this.M_SEARCH_TEXT.EditValue == null ? "" : this.M_SEARCH_TEXT.EditValue.ToString());
                //this.txt_Search.SelectAll();
                this.M_SEARCH_TEXT.Focus();
                this.ViewJOBEdit.ShowLoadingPanel = false;

                //((S131ViewModel)this.DataContext).setTitle();
            }
            catch (Exception eLog)
            {
                this.ViewJOBEdit.ShowLoadingPanel = false;
                WinUIMessageBox.Show(eLog.Message, "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
                //this.M_SEARCH_TEXT.SelectAll();
                this.M_SEARCH_TEXT.Focus();
                return;
            }
        }

        private void M_SEARCH_TEXT_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                M_REFRESH_ItemClick(sender, null);
            }
        }
        //void txt_Search_KeyDown(Object sender, KeyEventArgs e)
        //{
        //    if (e.Key == Key.Enter)
        //    {
        //        Search(this.txt_Search.Text);
        //    }
        //}

        //void btn_search_Click(object sender, RoutedEventArgs e)
        //{
        //    Search(this.txt_Search.Text);
        //}

        //void Search(string scarch)
        //{
        //    try
        //    {
        //        this.ViewJOBEdit.ShowLoadingPanel = true;
        //        this.viewJOBEditView.SearchString = scarch;
        //        this.txt_Search.SelectAll();
        //        this.txt_Search.Focus();
        //        this.ViewJOBEdit.ShowLoadingPanel = false;
        //    }
        //    catch (Exception eLog)
        //    {
        //        this.ViewJOBEdit.ShowLoadingPanel = false;
        //        WinUIMessageBox.Show(eLog.Message, "[에러] 칭량 작업 계획 / 지시", MessageBoxButton.OK, MessageBoxImage.Error);
        //        this.txt_Search.SelectAll();
        //        this.txt_Search.Focus();
        //        return;
        //    }
        //}
        #endregion
    }
}