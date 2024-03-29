﻿using ModelsLibrary.Man;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AquilaErpWpfApp3.View.M.Report
{
    public partial class M665101Report : DevExpress.XtraReports.UI.XtraReport
    {
        public M665101Report(IList<ManVo> allItems)
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
        private class NameList : ObservableCollection<ManVo>
        {
            public NameList() : base() { }

        }

    }

}
