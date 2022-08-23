using AquilaErpWpfApp3.ViewModel;
using DevExpress.Xpf.Grid;
using System.Windows.Controls;
using ModelsLibrary.Inv;
using DevExpress.Xpf.WindowsUI;
using AquilaErpWpfApp3.Util;
using System.Windows;

namespace AquilaErpWpfApp3.View.INV
{
    /// <summary>
    /// I55221.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class I55221 : UserControl
    {
        private string _title = "AL 자재수불장";

        public I55221()
        {
            InitializeComponent();

            DataContext = new I55221ViewModel();
        }

        private void TableView_CellMerge(object sender, DevExpress.Xpf.Grid.CellMergeEventArgs e)
        {
            TableView view = sender as TableView;

            try
            {
                InvVo item1 = view.Grid.GetRow(e.RowHandle1) as InvVo;
                InvVo item2 = view.Grid.GetRow(e.RowHandle2) as InvVo;

                if (item1.ITM_CD == item2.ITM_CD)
                {
                    if (e.Column.FieldName == "ITM_CD") e.Merge = true;
                    else if (e.Column.FieldName == "ITM_NM") e.Merge = true;

                    else if (item1.CO_CD == item2.CO_CD)
                    {
                        if (e.Column.FieldName == "CO_CD") e.Merge = true;
                        else if (e.Column.FieldName == "INAUD_DT") e.Merge = item1.INAUD_DT == item2.INAUD_DT ? true : false;

                        else e.Merge = false;
                    }
                    else
                    {
                        e.Merge = false;
                    }
                }
                else
                {
                    e.Merge = false;
                }
                e.Handled = true;
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }

        }
    }
}
