using DevExpress.XtraReports.UI;
using ModelsLibrary.Man;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;

namespace AquilaErpWpfApp3.View.M.Report
{
    public partial class M6633BarCodeReport : DevExpress.XtraReports.UI.XtraReport
    {
        public M6633BarCodeReport(ManVo allItems)
        {
            InitializeComponent();

            //
            NameList list = new NameList();
            //int size = allItems.Count;
            //for (int x = 0; x < size; x++)
            //{
            //    list.Add(allItems[x]);
            //}
            list.Add(allItems);
            this.DataSource = list;

        }
        private class NameList : ObservableCollection<ManVo>
        {
            public NameList() : base() { }

        }

    }
}
