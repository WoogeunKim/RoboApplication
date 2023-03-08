using DevExpress.XtraReports.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using ModelsLibrary.Sale;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
//using Tesseract;
//using IronOcr;
using System.Linq;
using DevExpress.Xpf.WindowsUI;
using System.Windows;

namespace AquilaErpWpfApp3.View.SAL.Report
{
    public partial class S22223Report2 : DevExpress.XtraReports.UI.XtraReport
    {
        public S22223Report2(IList<SaleVo> allItems)
        {
            InitializeComponent();
            //
            NameList list = new NameList();

            int size = allItems.Count;
            for (int x = 0; x < size; x++)
            {
                //var aaaaaa = GetImageByte(allItems[x].ITM_IMG);

                //var aaaaaa = GetImageOcr(allItems[x].ITM_IMG);



                list.Add(allItems[x]);
            }
            this.DataSource = list;
        }

        //public object GetImageOcr(byte[] array)
        //{
        //    var ocr = new IronTesseract();

        //    using (var ms = new MemoryStream(array))
        //    using (var image = new Bitmap(ms))
        //    {
        //        var result = ocr.Read(image);

        //        var targetChar = "A"; // 찾을 문자
        //        var targetRect = new Rectangle(0, 0, 5000, 5000); // 찾을 문자가 있을 범위
        //        var targetCharInfo = result
        //            .Characters
        //            .Where(c => c.Text == targetChar.ToString())
        //            .FirstOrDefault()?.Text;

        //        if (targetCharInfo != null)
        //        {

        //        }

        //        return targetCharInfo;

        //    }

        //}

        //public object GetImageByte(byte[] array)
        //{
        //    // OCR 엔진 생성
        //    using (var engine = new TesseractEngine("./tessdata", "eng", EngineMode.Default))
        //    {
        //        // byte[] 형식의 이미지 로드
        //        using (var ms = new MemoryStream(array))
        //        using (var image = new Bitmap(ms))
        //        {
        //            // OCR 수행         
        //            using (var page = engine.Process(image))
        //            {
        //                // 문자열 추출
        //                var text = page.GetText();

        //                // 문자열에서 특정 문자 위치 찾기
        //                var index = text.IndexOf("A");

        //                if (index >= 0)
        //                {
        //                    //문자열에서 특정 문자가 있는 위치 찾기
        //                    //var location = page.GetWordBoxes(page.Words[index]).FirstOrDefault();

        //                    //if (location != null)
        //                    //{
        //                    //    // Graphics 클래스를 사용하여 이미지에 숫자 그리기
        //                    //    using (var graphics = Graphics.FromImage(image))
        //                    //    {
        //                    //        // 그리기 폰트와 크기 설정
        //                    //        var font = new Font("Arial", 12);

        //                    //        // 숫자 그리기
        //                    //        graphics.DrawString("1", font, Brushes.Black, location.Left, location.Top);
        //                    //    }

        //                    //    // 수정된 이미지를 byte[] 형식으로 저장
        //                    //    byte[] outputImageBytes;
        //                    //    using (var outputMs = new MemoryStream())
        //                    //    {
        //                    //        image.Save(outputMs, image.RawFormat);
        //                    //        outputImageBytes = outputMs.ToArray();
        //                    //    }
        //                    //}
        //                }
        //            }
        //        }

        //        return true;
        //    }

        //}



        private class NameList : ObservableCollection<SaleVo>
        {
            public NameList() : base() { }
        }
    }
}
