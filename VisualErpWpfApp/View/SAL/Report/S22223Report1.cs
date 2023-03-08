using DevExpress.XtraReports.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using ModelsLibrary.Sale;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AquilaErpWpfApp3.View.SAL.Report
{
    public partial class S22223Report1 : DevExpress.XtraReports.UI.XtraReport
    {
        public S22223Report1(IList<SaleVo> allItems)
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
