using ModelsLibrary.Pur;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AquilaErpWpfApp3.View.PUR.Report
{
    public partial class P441102TagReport : DevExpress.XtraReports.UI.XtraReport
    {
        public P441102TagReport(IList<PurVo> allItems)
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

        private class NameList : ObservableCollection<PurVo>
        {
            public NameList() : base() { }
        }
    }
}
