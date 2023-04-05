using ModelsLibrary.Inv;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;

namespace AquilaErpWpfApp3.View.INV.Report
{
    public partial class I6611BarCodeReport : DevExpress.XtraReports.UI.XtraReport
    {
        public I6611BarCodeReport(InvVo allItem)
        {
            InitializeComponent();

            //
            NameList list = new NameList();
            list.Add(allItem);

            this.DataSource = list;

            //if (Properties.Settings.Default.SettingLogImg != null)
            //{
            //    xrPictureBox1.ImageSource = new DevExpress.XtraPrinting.Drawing.ImageSource(ByteToImage(Convert.FromBase64String(Properties.Settings.Default.SettingLogImg)));
            //}

        }

        private class NameList : ObservableCollection<InvVo>
        {
            public NameList() : base() { }

        }



        //이미지 변환
        public Bitmap ByteToImage(byte[] imageData)
        {
            Bitmap biImg;
            using (var ms = new MemoryStream(imageData))
            {
                biImg = new Bitmap(ms);
                Size size = new Size((int)(biImg.Width / 1.2), (int)(biImg.Height / 1.2));
                biImg = new Bitmap(biImg, size);
            }

            return biImg;
        }
    }
}
