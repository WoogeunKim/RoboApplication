using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Binding = System.Windows.Data.Binding;
using Color = System.Drawing.Color;

namespace ShapeImageLib
{
    /// <summary>
    /// UserImageTools.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class UserImageTools : System.Windows.Controls.UserControl
    {
        System.Collections.ObjectModel.ObservableCollection<ShapeImagePointInfo> Datas = null;

        public ShapeImageInfo GetData
        {
            get
            {
                return ShapeData;
            }
        }

        PanelD DrawEdit = null;

        Bitmap ShpaeImage = null;

        // 데이터
        ShapeImageInfo ShapeData;

        public UserImageTools()
        {
            InitializeComponent();


            // 패널 에디터
            DrawEdit = new PanelD();
            DrawEdit.Paint += DrawEdit_Paint;
            DrawEdit.MouseMove += DrawEdit_MouseMove;
            DrawEdit.MouseDown += DrawEdit_MouseDown;
            wfEdit.SizeChanged += WfEdit_SizeChanged;
            wfEdit.Child = DrawEdit;

            // 패널 설정
            cbailgn.Items.Add("왼쪽"); 
            cbailgn.Items.Add("가운데");
            cbailgn.Items.Add("오른쪽");
            cbailgn.SelectionChanged += Cbailgn_SelectionChanged;
            cbailgn.SelectedIndex = 1;


            // 데이터 추가 컬럼 바인딩
            GridView gv = new GridView();
            
            GridViewColumn Col1 = new GridViewColumn();
            Col1.Header = "좌표명";
            Col1.Width = 100;
            Col1.DisplayMemberBinding = new Binding("Display");
            gv.Columns.Add(Col1);

            GridViewColumn Col2 = new GridViewColumn();
            Col2.Header = "값";
            Col2.Width = 100;
            Col2.DisplayMemberBinding = new Binding("Value");
            gv.Columns.Add(Col2);

            GridViewColumn Col3 = new GridViewColumn();
            Col3.Header = "좌표정보";
            Col3.Width = 100;
            Col3.DisplayMemberBinding = new Binding("Point");
            gv.Columns.Add(Col3);

            lstViee.View = gv;
            lstViee.SelectionMode = SelectionMode.Single;
        }

 

        /// <summary>
        /// 사이즈 변경에 따른 좌표 수정
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WfEdit_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            try
            {
                // 이미지 초기화 및 갱신
                if (ShpaeImage != null)
                {
                    DrawEdit.Location = new System.Drawing.Point(0, 0);
                    DrawEdit.Width = ShpaeImage.Width;
                    DrawEdit.Height = ShpaeImage.Height;

                    // 이미지 사이즈 표시
                    imagesize.Text = $"사이즈 : {DrawEdit.Width.ToString("D0")} x {DrawEdit.Height.ToString("D0")}";
                }
            }
            catch(Exception ex)
            {

            }
        }



        public void DataLoad(ShapeImageInfo _ShapeData)
        {
            ShapeData = _ShapeData;

            if (ShpaeImage != null)
            {
                ShpaeImage.Dispose();
                ShpaeImage = null;
            }

            if (ShapeData.Image != null)
            {
                using (System.IO.MemoryStream ss = new System.IO.MemoryStream(ShapeData.Image))
                {
                    ShpaeImage = new Bitmap(ss);
                }
            }
                

            Datas = new System.Collections.ObjectModel.ObservableCollection<ShapeImagePointInfo>();
            foreach (var item in ShapeData.Datas)
            {
                // 값이 없는 경우에는 보여지지 않는다.
                if (String.IsNullOrEmpty(item.Value))
                    continue;

                Datas.Add(item);
            }

            lstViee.ItemsSource = Datas;


            WfEdit_SizeChanged(null, null);
            ImgRefresh();

        }


        #region 좌표 관리 항목 선택

        private void lstViee_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstViee.SelectedItem == null)
                return;

            var item = lstViee.SelectedItem as ShapeImagePointInfo;
            if (item == null)
                return;


            // 설명
            var strMeg = $"[{item.Display}] ";
            if (item.isPoint())
            {
                strMeg += "마우스 클릭시 좌표가 수정 됩니다.";
            }
            else
            {
                strMeg += "마우스 클릭 하여 좌표를 추가 하세요.";
            }

            lblView.Content = strMeg;

            // 폰트 정렬 위치 설정
            cbailgn.SelectedIndex = Convert.ToInt32(item.Align);

            // 좌표 위치
             SetPos(item.pos.X.ToString(), item.pos.Y.ToString());

            ImgRefresh();
        }

        #endregion

        #region 이미지 에디터


        private void ImgRefresh()
        {
            if (DrawEdit != null)
            {
                DrawEdit.Invalidate();
            }


            // 이미지 생성 실시간

            if (ShapeData != null)
            {
                var image = new BitmapImage();
                using (var ms = new System.IO.MemoryStream(ShapeImageLibFunc.TransShapeImage(ShapeData)))
                {
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad; // here
                    image.StreamSource = ms;
                    image.EndInit();
                }
                
                imgPreview.Source = image;
                
            }

        }

        private void DrawEdit_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                SetPos(e.Location.X.ToString(), e.Location.Y.ToString() , true);
            }
        }

        private void DrawEdit_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            imagepos.Text = $"좌표 : {e.Location.X.ToString()} , {e.Location.Y.ToString()}";
        }


        private void DrawEdit_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            ShapeImageLibFunc.CoreCreate(e.Graphics , ShpaeImage , Datas , lstViee.SelectedItem as ShapeImagePointInfo);
        }
  

        /// <summary>
        /// 위치 변경
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void SetPos(string x , string y , bool isAdd = false)
        {
            var item = lstViee.SelectedItem as ShapeImagePointInfo;

            if (item == null)
                return;

            if (!item.isPoint())
            {
                if (!isAdd)
                {
                    btndelete.IsEnabled = tbX.IsEnabled = tbY.IsEnabled = cbailgn.IsEnabled = false;
                    
                    return;
                }
                else
                {

                }
            }
            else 
            {
               
            }

            btndelete.IsEnabled = tbX.IsEnabled = tbY.IsEnabled = cbailgn.IsEnabled = true;

            bool isnull = false;

            if (String.IsNullOrWhiteSpace(x))
                isnull = true;

            if (String.IsNullOrWhiteSpace(y))
                isnull = true;

            if (!isnull)
            {
                // 값 저장
                item.Point = $"{x},{y},{cbailgn.SelectedIndex.ToString()}";

                // ui 매핑
                tbX.Text = x;
                tbY.Text = y;
            }

            ImgRefresh();
        }


        #endregion

        #region 프로퍼티 

        // 이미지 에디터 정렬 순서
        private void Cbailgn_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = lstViee.SelectedItem as ShapeImagePointInfo;

            if (item == null)
                return;

            if (!item.isPoint())
                return;
            
            item.Point = $"{item.pos.X.ToString()},{item.pos.Y.ToString()},{cbailgn.SelectedIndex.ToString()}";

            ImgRefresh();
        }

        // X 변경
        private void tbX_TextChanged(object sender, TextChangedEventArgs e)
        {
            SetPos(tbX.Text , tbY.Text);
        }

        /// <summary>
        /// 숫자만 입력
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbX_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        // Y 변경
        private void tbY_TextChanged(object sender, TextChangedEventArgs e)
        {
            SetPos(tbX.Text, tbY.Text);
        }


        /// <summary>
        /// 숫자만 입력
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbY_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        /// <summary>
        /// 삭제
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var item = lstViee.SelectedItem as ShapeImagePointInfo;

            if (item == null)
                return;

            if (System.Windows.Forms.MessageBox.Show(item.Display + "삭제 하시겠습니까?" , "삭제",
                System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                // 값 삭제
                item.Point = null;
                SetPos("", "");
            }
        }

        #endregion

        #region 컨트롤

        /// <summary>
        /// 패널 버퍼링 전용
        /// </summary>
        public partial class PanelD : System.Windows.Forms.Panel
        {
            public PanelD()
            {
                DoubleBuffered = true;
                SetStyle(System.Windows.Forms.ControlStyles.DoubleBuffer, true);
                SetStyle(System.Windows.Forms.ControlStyles.AllPaintingInWmPaint, true);
                SetStyle(System.Windows.Forms.ControlStyles.UserPaint, true);
                SetStyle(System.Windows.Forms.ControlStyles.OptimizedDoubleBuffer, true);
                SetStyle(System.Windows.Forms.ControlStyles.ResizeRedraw, true);
                UpdateStyles();
            }

            /// <summary>
            /// DoubleBuffering 사용 여부
            /// </summary>
            public bool DoubleBuffering
            {
                get
                {
                    return DoubleBuffered;
                }

                set
                {
                    if (value)
                    {
                        DoubleBuffered = true;
                        return;
                    }

                    if (DoubleBuffered != value)
                    {
                        DoubleBuffered = value;
                        SetStyle(System.Windows.Forms.ControlStyles.DoubleBuffer, value);
                        SetStyle(System.Windows.Forms.ControlStyles.OptimizedDoubleBuffer, value);

                        if (value)
                        {
                            SetStyle(System.Windows.Forms.ControlStyles.AllPaintingInWmPaint, value);
                            SetStyle(System.Windows.Forms.ControlStyles.UserPaint, value);
                        }

                    }
                }
            }

            System.Drawing.Text.TextRenderingHint TextRenderingHint;
            System.Drawing.Drawing2D.CompositingQuality CompositingQuality;
            System.Drawing.Drawing2D.CompositingMode CompositingMode;
            System.Drawing.Drawing2D.InterpolationMode InterpolationMode;
            System.Drawing.Drawing2D.PixelOffsetMode PixelOffsetMode;
            System.Drawing.Drawing2D.SmoothingMode SmoothingMode;

            /// <summary>
            /// Graphics 품질 변경 합니다.
            /// </summary>
            /// <param name="gr"></param>
            /// <param name="Type"></param>
            public void SetDrawOption(Graphics gr)
            {
                if (gr == null)
                    return;

                TextRenderingHint = gr.TextRenderingHint;
                CompositingQuality = gr.CompositingQuality;
                CompositingMode = gr.CompositingMode;
                InterpolationMode = gr.InterpolationMode;
                PixelOffsetMode = gr.PixelOffsetMode;
                SmoothingMode = gr.SmoothingMode;
            }

            /// <summary>
            /// Graphics 품질 이전값으로 복구 합니다.
            /// </summary>
            /// <param name="gr"></param>
            /// <param name="Type"></param>
            public void ResetDrawOption(Graphics gr)
            {
                gr.TextRenderingHint = TextRenderingHint;
                gr.CompositingQuality = CompositingQuality;
                gr.CompositingMode = CompositingMode;
                gr.InterpolationMode = InterpolationMode;
                gr.PixelOffsetMode = PixelOffsetMode;
                gr.SmoothingMode = SmoothingMode;
            }

        }


        #endregion

     
    }
}
