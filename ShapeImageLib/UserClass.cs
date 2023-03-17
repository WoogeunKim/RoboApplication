using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeImageLib
{
    public static class ShapeImageLibFunc
    {
        public static void CoreCreate(
              Graphics gg,
              Bitmap _ShpaeImage,
              System.Collections.ObjectModel.ObservableCollection<ShapeImagePointInfo> Data, ShapeImagePointInfo Selelctitem)
        {
            // 그래프 화면 초기화 - 필수적으로 사용 해야함. 텍스트 일그러짐, 퍼짐 현상 방지
            //gg.Clear(Color.White);

            // 배경 이미지 그리기
            if (_ShpaeImage != null)
            {
                gg.DrawImage(_ShpaeImage, 0, 0);
            }

           
            // 실제 데이터 그리기
            if (Data != null)
            {
                for (int i = 0; i < Data.Count; i++)
                {
                    var DrawData = Data[i];

                    if (DrawData.isPoint())
                    {
                        var spData = DrawData.Point.Split(',');
                        if (spData.Length >= 3)
                        {
                            System.Drawing.PointF TextPoint = DrawData.pos;
                            var pos = DrawData.Align;
                            var Text = Convert.ToInt32(DrawData.Value).ToString("#,##0");
                            var Fontinfo = new Font("tahoma", 10);

                            // 사이즈 변경
                            SizeF SizeData = gg.MeasureString(Text, Fontinfo);

                            // 실제 그리기
                            if (pos == "1")
                            {
                                // 가운데
                                TextPoint.X = TextPoint.X - SizeData.Width / 2;
                            }
                            else if (pos == "2")
                            {
                                // 오른쪽

                            }
                            else
                            {
                                // 나머지는 왼쪽
                                TextPoint.X = TextPoint.X - SizeData.Width;
                            }

                            // Y축은 원래 가운데 정렬
                            TextPoint.Y = TextPoint.Y - SizeData.Height / 2;

                            var brrect = new SolidBrush(Color.White);
                            if (Selelctitem != null)
                            {
                                if (Selelctitem.Equals(DrawData))
                                {
                                    brrect = new SolidBrush(Color.Red);
                                }
                            }

                            // 뒤에 배경 그리기
                            gg.FillRectangle(brrect,
                                new RectangleF(TextPoint.X, TextPoint.Y, SizeData.Width - (Fontinfo.Size / 4),
                                SizeData.Height - (Fontinfo.Size / 4)));

                            // 글자 그리기
                            gg.DrawString(Text, Fontinfo,
                                new SolidBrush(Color.Black), new PointF(TextPoint.X, TextPoint.Y));


                        }
                    }
                }
            }


        }




        public static byte[] TransShapeImage(ShapeImageInfo Data)
        {
            byte[] ret = null;

            using (System.IO.MemoryStream ss = new System.IO.MemoryStream(Data.Image))
            {
                using (Bitmap ShpaeImage = new Bitmap(ss))
                {
                    using (var gg = Graphics.FromImage(ShpaeImage))
                    {
                        CoreCreate(gg , ShpaeImage , Data.Datas , null);
                    }

                    using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
                    {
                        ShpaeImage.Save(stream, ShpaeImage.RawFormat);
                        ret = stream.ToArray();
                    }
               
                }
            }

            return ret;
        }

    }




    public class ShapeImageInfo : INotifyPropertyChangedEx
    {
        public ShapeImageInfo()
        {

        }

        private string _Code;

        /// <summary>
        /// 형상코드
        /// </summary>
        public string Code
        {
            get { return _Code; }
            set { _Code = value; OnPropertyChanged(); }
        }

        private byte[] _Image;

        /// <summary>
        /// 이미지 정보
        /// </summary>
        public byte[] Image
        {
            get { return _Image; }
            set { _Image = value; OnPropertyChanged(); }
        }


        /// <summary>
        /// 값 정보
        /// </summary>
        public System.Collections.ObjectModel.ObservableCollection<ShapeImagePointInfo> Datas { get; set; } = new System.Collections.ObjectModel.ObservableCollection<ShapeImagePointInfo>();

    }

    /// <summary>
    ///  데이터 정보
    /// </summary>
    public class ShapeImagePointInfo : INotifyPropertyChangedEx
    {
        public ShapeImagePointInfo(string m_Display , object m_Value, object m_Point)
        {
            Display = m_Display;

            // 값 정보
            if (m_Value != null)
                Value = m_Value.ToString();

            // 좌표 정보
            if (m_Point != null)
                Point = m_Point.ToString();
        }


        private string _Display;

        /// <summary>
        /// 표시명
        /// </summary>
        public string Display
        {
            get { return _Display; }
            set { _Display = value; OnPropertyChanged(); }
        }

        private string _Value;

        /// <summary>
        /// 값
        /// </summary>
        public string Value
        {
            get { return _Value; }
            set { _Value = value; OnPropertyChanged(); }
        }

        private string _Point;

        /// <summary>
        /// 위치
        /// </summary>
        public string Point
        {
            get { return _Point; }
            set 
            {
                _Point = value;
                
                if (Point != null)
                {
                    var spData = Point.Split(',');
                    if (spData.Length >= 3)
                    {
                        pos = new System.Drawing.PointF(
                                   Convert.ToInt32(spData[0]),
                                   Convert.ToInt32(spData[1]));

                        Align = spData[2];
                    }
                }
                else
                {
                    pos = new PointF(0, 0);
                }

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 좌표값 존재 여부
        /// </summary>
        /// <returns></returns>
        public bool isPoint()
        {
            bool ret = false;

            if (string.IsNullOrWhiteSpace(_Point))
            {
                // 미존재
                ret = false;
            }
            else
            {
                // 존재 
                ret = true;
            }


            return ret;
        }



        public System.Drawing.PointF pos = new System.Drawing.PointF();
        public string Align = "1";


    }

    /// <summary>
    /// INotifyPropertyChanged 확장형
    /// </summary>
    public class INotifyPropertyChangedEx : INotifyPropertyChanged
    {
        public INotifyPropertyChangedEx()
        {


        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
