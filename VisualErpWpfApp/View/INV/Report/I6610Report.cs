using ModelsLibrary.Inv;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AquilaErpWpfApp3.View.INV.Report
{
    public partial class I6610Report : DevExpress.XtraReports.UI.XtraReport
    {
        public I6610Report(IList<InvVo> allItems)
        {
            InitializeComponent();
            //
            NameList list = new NameList();
            int size = allItems.Count;
            for (int x = 0; x < size; x++)
            {
                list.Add(allItems[x]);
            }
            this.DataSource = list;
        }

        private class NameList : ObservableCollection<InvVo>
        {
            public NameList() : base() { }
        }

    }
}
