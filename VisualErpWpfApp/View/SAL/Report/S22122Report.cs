using ModelsLibrary.Sale;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AquilaErpWpfApp3.View.SAL.Report
{
    public partial class S22122Report : DevExpress.XtraReports.UI.XtraReport
    {
        public S22122Report(IList<SaleVo> allItems)
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

        private class NameList : ObservableCollection<SaleVo>
        {
            public NameList() : base() { }
        }

    }
}
