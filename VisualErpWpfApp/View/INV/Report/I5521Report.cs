using ModelsLibrary.Inv;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AquilaErpWpfApp3.View.INV.Report
{
    public partial class I5521Report : DevExpress.XtraReports.UI.XtraReport
    {
        public I5521Report(IList<InvVo> allItems)
        {
            InitializeComponent();

            NameList list = new NameList();
            int size = allItems.Count;
            for (int x = 0; x < size; x++)
            {
                allItems[x].CRE_DT = System.DateTime.Now;
                allItems[x].ITM_QTY = string.Format("{0:#,##0}", allItems[x].ITM_QTY);
                allItems[x].MD_PER_QTY = string.Format("{0:#,##0}", allItems[x].MD_PER_QTY);
                allItems[x].PK_PER_QTY = string.Format("{0:#,##0}", allItems[x].PK_PER_QTY);
                allItems[x].ITM_MD_QTY = string.Format("{0:#,##0}", allItems[x].ITM_MD_QTY);

                


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
