using DevExpress.XtraReports.UI;
using ModelsLibrary.Tec;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;

namespace AquilaErpWpfApp3.View.TEC.Report
{
    public partial class T8814BarCodeReport : DevExpress.XtraReports.UI.XtraReport
    {
        public T8814BarCodeReport(IList<TecVo> allItems)
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
        private class NameList : ObservableCollection<TecVo>
        {
            public NameList() : base() { }

        }
    }
}
