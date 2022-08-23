using DevExpress.XtraReports.UI;
using ModelsLibrary.Pur;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;

namespace AquilaErpWpfApp3.View.PUR.Report
{
    public partial class P4411TagReport : DevExpress.XtraReports.UI.XtraReport
    {
        public P4411TagReport(IList<PurVo> allItems)
        {
            InitializeComponent();
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

        private void PageHeader_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

        }

        private void P4411TagReport_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {

        }
    }
}
