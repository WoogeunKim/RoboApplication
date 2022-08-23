using ModelsLibrary.Tec;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AquilaErpWpfApp3.View.TEC.Report
{
    public partial class T8813LabelReport : DevExpress.XtraReports.UI.XtraReport
    {
        public T8813LabelReport(IList<TecVo> _items)
        {
            InitializeComponent();
            NameList list = new NameList();
            foreach (TecVo item in _items)
            {
                list.Add(item);
            }
            this.DataSource = list;
        }
        private class NameList : ObservableCollection<TecVo>
        {
            public NameList() : base() { }

        }
    }
}
