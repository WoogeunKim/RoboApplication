using DevExpress.Xpf.Core;
using System.Windows;
using System.Windows.Input;


namespace AquilaErpWpfApp3.Util
{
    public partial class NumberPadDialog : DXWindow
    {
        private bool isKor = false;
        private bool isEng = true;
        private bool isShift = true;

        private string ContentTotalKeyPad = string.Empty;
        //
        private string ContentEngKeyPad = string.Empty;
        private string ContentKorKeyPad = string.Empty;
        private char _ContentKorPad;

        private string _Input = string.Empty;

        private Hangul _hangul = new Hangul();

        public virtual string editContent { get; set; }
        public virtual string orgContent { get; set; }

        public virtual bool isResult { get; set; }

        public NumberPadDialog(string _input, bool _isKor, bool _isNum, int _maxLen)
        {
            InitializeComponent();

            this._Input = new string(_input.ToCharArray());
            this.txtKeyPad.Text = (_input.Equals("0.00") ? "" : _input);

            //this.EngAndKor.Click += EngAndKor_Click;
            //this.shift.Click += shift_Click;

            #region Functon (첫줄 [숫자])
            this.K1.Click += K1_Click;
            this.K2.Click += K2_Click;
            this.K3.Click += K3_Click;
            this.K4.Click += K4_Click;
            this.K5.Click += K5_Click;
            this.K6.Click += K6_Click;
            this.K7.Click += K7_Click;
            this.K8.Click += K8_Click;
            this.K9.Click += K9_Click;
            this.K0.Click += K0_Click;
            //this.Km_.Click += KM__Click;
            //this.K_.Click += K__Click;
            //
            this.K_Del.Click += K_Del_Click;
            //this.space.Click += space_Click;
            this.enter.Click += enter_Click;
            #endregion

            //#region Functon (첫줄 [영어])
            //this.q.Click += q_Click;
            //this.w.Click += w_Click;
            //this.e.Click += e_Click;
            //this.r.Click += r_Click;
            //this.t.Click += t_Click;
            //this.y.Click += y_Click;
            //this.u.Click += u_Click;
            //this.i.Click += i_Click;
            //this.o.Click += o_Click;
            //this.p.Click += p_Click;
            //#endregion

            //#region Functon (두줄 [영어])
            //this.a.Click += a_Click;
            //this.s.Click += s_Click;
            //this.d.Click += d_Click;
            //this.f.Click += f_Click;
            //this.g.Click += g_Click;
            //this.h.Click += h_Click;
            //this.j.Click += j_Click;
            //this.k.Click += k_Click;
            //this.l.Click += l_Click;
            //#endregion

            //#region Functon (셋줄 [영어])
            //this.z.Click += z_Click;
            //this.x.Click += x_Click;
            //this.c.Click += c_Click;
            //this.v.Click += v_Click;
            //this.b.Click += b_Click;
            //this.n.Click += n_Click;
            //this.m.Click += m_Click;
            //#endregion

            #region Functon (넷줄 [.])
            this.point.Click += point_Click;
            #endregion


            isKeyEvent(_isNum);

            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
            this.Closed += Window_Closed;

            this.txtKeyPad.MaxLength = _maxLen;
            //this.EngAndKor.IsEnabled = _isKor;

            //
            isKor = false;
            isEng = true;
            isShift = true;
            shift_Click(null,null);
        }

        void Window_Closed(object sender, System.EventArgs e)
        {
            if (this.isResult == false)
            {
                this.orgContent = this._Input;
                this.isResult = false;
            }
            //Close();
        }

        void isKeyEvent(bool isNum)
        {
            if (isNum)
            {
                this.K1.IsEnabled = true;
                this.K2.IsEnabled = true;
                this.K3.IsEnabled = true;
                this.K4.IsEnabled = true;
                this.K5.IsEnabled = true;
                this.K6.IsEnabled = true;
                this.K7.IsEnabled = true;
                this.K8.IsEnabled = true;
                this.K9.IsEnabled = true;
                this.K0.IsEnabled = true;

                this.K_Del.IsEnabled = true;
                //
                //this.K_.IsEnabled = false;
                //this.Km_.IsEnabled = false;

                //this.EngAndKor.IsEnabled = false;
                //this.shift.IsEnabled = false;
                ////
                //this.q.IsEnabled = false;
                //this.w.IsEnabled = false;
                //this.e.IsEnabled = false;
                //this.r.IsEnabled = false;
                //this.t.IsEnabled = false;
                //this.y.IsEnabled = false;
                //this.u.IsEnabled = false;
                //this.i.IsEnabled = false;
                //this.o.IsEnabled = false;
                //this.p.IsEnabled = false;

                //this.a.IsEnabled = false;
                //this.s.IsEnabled = false;
                //this.d.IsEnabled = false;
                //this.f.IsEnabled = false;
                //this.g.IsEnabled = false;
                //this.h.IsEnabled = false;
                //this.j.IsEnabled = false;
                //this.k.IsEnabled = false;
                //this.l.IsEnabled = false;

                //this.z.IsEnabled = false;
                //this.x.IsEnabled = false;
                //this.c.IsEnabled = false;
                //this.v.IsEnabled = false;
                //this.b.IsEnabled = false;
                //this.n.IsEnabled = false;
                //this.m.IsEnabled = false;
            }
        }


        void enter_Click(object sender, RoutedEventArgs e)
        {
            this.editContent = this.txtKeyPad.Text;
            this.isResult = true;
            Close();
        }

        #region Functon (첫줄 [숫자])
        void K1_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.ContentTotalKeyPad = this.txtKeyPad.Text;
            this.ContentTotalKeyPad += "1";
            this.txtKeyPad.Text = this.ContentTotalKeyPad;
        }

        void K2_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.ContentTotalKeyPad = this.txtKeyPad.Text;
            this.ContentTotalKeyPad += "2";
            this.txtKeyPad.Text = this.ContentTotalKeyPad;
        }

        void K3_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.ContentTotalKeyPad = this.txtKeyPad.Text;
            this.ContentTotalKeyPad += "3";
            this.txtKeyPad.Text = this.ContentTotalKeyPad;
        }

        void K4_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.ContentTotalKeyPad = this.txtKeyPad.Text;
            this.ContentTotalKeyPad += "4";
            this.txtKeyPad.Text = this.ContentTotalKeyPad;
        }

        void K5_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.ContentTotalKeyPad = this.txtKeyPad.Text;
            this.ContentTotalKeyPad += "5";
            this.txtKeyPad.Text = this.ContentTotalKeyPad;
        }

        void K6_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.ContentTotalKeyPad = this.txtKeyPad.Text;
            this.ContentTotalKeyPad += "6";
            this.txtKeyPad.Text = this.ContentTotalKeyPad;
        }

        void K7_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.ContentTotalKeyPad = this.txtKeyPad.Text;
            this.ContentTotalKeyPad += "7";
            this.txtKeyPad.Text = this.ContentTotalKeyPad;
        }

        void K8_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.ContentTotalKeyPad = this.txtKeyPad.Text;
            this.ContentTotalKeyPad += "8";
            this.txtKeyPad.Text = this.ContentTotalKeyPad;
        }

        void K9_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.ContentTotalKeyPad = this.txtKeyPad.Text;
            this.ContentTotalKeyPad += "9";
            this.txtKeyPad.Text = this.ContentTotalKeyPad;
        }

        void K0_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.ContentTotalKeyPad = this.txtKeyPad.Text;
            this.ContentTotalKeyPad += "0";
            this.txtKeyPad.Text = this.ContentTotalKeyPad;
        }

        void KM__Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.ContentTotalKeyPad = this.txtKeyPad.Text;
            this.ContentTotalKeyPad += "-";
            this.txtKeyPad.Text = this.ContentTotalKeyPad;
        }

        void K__Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.ContentTotalKeyPad = this.txtKeyPad.Text;
            this.ContentTotalKeyPad += "_";
            this.txtKeyPad.Text = this.ContentTotalKeyPad;
        }

        void K_Del_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //this.ContentKorKeyPad = string.Empty;
                //this.ContentEngKeyPad = string.Empty;
                this.ContentTotalKeyPad = this.txtKeyPad.Text;
                this.txtKeyPad.Text = this.ContentTotalKeyPad.Substring(0, this.ContentTotalKeyPad.Length - 1);
            }
            catch { }
        }
        #endregion

        #region Functon (첫줄 [영어])
        void q_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            CallKeyPad("q");
        }

        void w_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            CallKeyPad("w");
        }

        void e_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            CallKeyPad("e");
        }

        void r_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            CallKeyPad("r");
        }

        void t_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            CallKeyPad("t");
        }

        void y_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            CallKeyPad("y");
        }

        void u_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            CallKeyPad("u");
        }

        void i_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            CallKeyPad("i");
        }

        void o_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            CallKeyPad("o");
        }


        void p_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            CallKeyPad("p");
        }
        #endregion

        #region Functon (두줄 [영어])
        void a_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            CallKeyPad("a");
        }

        void s_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            CallKeyPad("s");
        }

        void d_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            CallKeyPad("d");
        }

        void f_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            CallKeyPad("f");
        }

        void g_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            CallKeyPad("g");
        }

        void h_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            CallKeyPad("h");
        }

        void j_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            CallKeyPad("j");
        }

        void k_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            CallKeyPad("k");
        }

        void l_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            CallKeyPad("l");
        }
        #endregion

        #region Functon (셋줄 [영어])
        void z_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            CallKeyPad("z");
        }

        void x_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            CallKeyPad("x");
        }

        void c_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            CallKeyPad("c");
        }

        void v_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            CallKeyPad("v");
        }

        void b_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            CallKeyPad("b");
        }

        void n_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            CallKeyPad("n");
        }

        void m_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            CallKeyPad("m");
        }

        //void space_Click(object sender, RoutedEventArgs e)
        //{
        //    this.ContentTotalKeyPad = this.txtKeyPad.Text;
        //    this.ContentTotalKeyPad += " ";
        //    this.txtKeyPad.Text = this.ContentTotalKeyPad;
        //}
        #endregion

        #region Functon (넷줄 [.])
        void point_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            CallKeyPad(".");
        }
        #endregion

        void shift_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            //if (this.isShift == true)
            //{
            //    isShift = false;
            //    this.shift.FontStyle = FontStyles.Normal;
            //}
            //else
            //{
            //    isShift = true;
            //    this.shift.FontStyle = FontStyles.Italic;
            //}

            //
            if (this.isKor)
            {
                setKor();
            }
            else if (this.isEng)
            {
                setEng();
            }
        }

        void EngAndKor_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.ContentKorKeyPad = string.Empty;
            this.ContentEngKeyPad = string.Empty;

            this._hangul.InitState();

            if (this.isKor == true)
            {
                this.Title = "[영어]가상 키보드";
                this.isKor = false;
                this.isEng = true;
                //영어
                setEng();
                //this.EngAndKor.Content = "한글";




            }
            else if (this.isEng == true)
            {
                this.Title = "[한글]가상 키보드";
                this.isEng = false;
                this.isKor = true;
                //한글
                setKor();
                //this.EngAndKor.Content = "영어";



            }
        }

        void setEng()
        {
            //if (this.isShift == true)
            //{
            //    //첫줄
            //    this.q.Content = "Q";
            //    this.w.Content = "W";
            //    this.e.Content = "E";
            //    this.r.Content = "R";
            //    this.t.Content = "T";
            //    this.y.Content = "Y";
            //    this.u.Content = "U";
            //    this.i.Content = "I";
            //    this.o.Content = "O";
            //    this.p.Content = "P";
            //    //두줄
            //    this.a.Content = "A";
            //    this.s.Content = "S";
            //    this.d.Content = "D";
            //    this.f.Content = "F";
            //    this.g.Content = "G";
            //    this.h.Content = "H";
            //    this.j.Content = "J";
            //    this.k.Content = "K";
            //    this.l.Content = "L";
            //    //세줄
            //    this.z.Content = "Z";
            //    this.x.Content = "X";
            //    this.c.Content = "C";
            //    this.v.Content = "V";
            //    this.b.Content = "B";
            //    this.n.Content = "N";
            //    this.m.Content = "M";
            //}
            //else
            //{
            //    //첫줄
            //    this.q.Content = "q";
            //    this.w.Content = "w";
            //    this.e.Content = "e";
            //    this.r.Content = "r";
            //    this.t.Content = "t";
            //    this.y.Content = "y";
            //    this.u.Content = "u";
            //    this.i.Content = "i";
            //    this.o.Content = "o";
            //    this.p.Content = "p";
            //    //두줄
            //    this.a.Content = "a";
            //    this.s.Content = "s";
            //    this.d.Content = "d";
            //    this.f.Content = "f";
            //    this.g.Content = "g";
            //    this.h.Content = "h";
            //    this.j.Content = "j";
            //    this.k.Content = "k";
            //    this.l.Content = "l";
            //    //세줄
            //    this.z.Content = "z";
            //    this.x.Content = "x";
            //    this.c.Content = "c";
            //    this.v.Content = "v";
            //    this.b.Content = "b";
            //    this.n.Content = "n";
            //    this.m.Content = "m";
            //}
        }
        void setKor()
        {
            ////첫줄
            //this.q.Content = "ㅂ";
            //this.w.Content = "ㅈ";
            //this.e.Content = "ㄷ";
            //this.r.Content = "ㄱ";
            //this.t.Content = "ㅅ";
            //this.y.Content = "ㅛ";
            //this.u.Content = "ㅕ";
            //this.i.Content = "ㅑ";
            //this.o.Content = "ㅐ";
            //this.p.Content = "ㅔ";
            ////두줄
            //this.a.Content = "ㅁ";
            //this.s.Content = "ㄴ";
            //this.d.Content = "ㅇ";
            //this.f.Content = "ㄹ";
            //this.g.Content = "ㅎ";
            //this.h.Content = "ㅗ";
            //this.j.Content = "ㅓ";
            //this.k.Content = "ㅏ";
            //this.l.Content = "ㅣ";
            ////세줄
            //this.z.Content = "ㅋ";
            //this.x.Content = "ㅌ";
            //this.c.Content = "ㅊ";
            //this.v.Content = "ㅍ";
            //this.b.Content = "ㅠ";
            //this.n.Content = "ㅜ";
            //this.m.Content = "ㅡ";

            //if (this.isShift == true)
            //{
            //    //첫줄
            //    this.q.Content = "ㅃ";
            //    this.w.Content = "ㅉ";
            //    this.e.Content = "ㄸ";
            //    this.r.Content = "ㄲ";
            //    this.t.Content = "ㅆ";
            //    this.y.Content = "ㅛ";
            //    this.u.Content = "ㅕ";
            //    this.i.Content = "ㅑ";
            //    this.o.Content = "ㅒ";
            //    this.p.Content = "ㅖ";
            //}
        }

        private void HandleEsc(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.orgContent = this._Input;
                this.isResult = false;
                Close();
            }
        }


        private void CallKeyPad(string _smallKey)
        {
            this.ContentTotalKeyPad = this.txtKeyPad.Text;
            if (isEng)
            {
                if (isShift)
                {
                    this.ContentEngKeyPad = _smallKey.ToUpper();
                }
                else
                {
                    this.ContentEngKeyPad = _smallKey.ToLower();
                }
                this.ContentTotalKeyPad += this.ContentEngKeyPad;
            }
            else if (isKor)
            {
                if (!string.IsNullOrEmpty(this.ContentKorKeyPad))
                {
                    if (this.ContentKorKeyPad.Length > 0)
                    {
                        this.ContentTotalKeyPad = this.ContentTotalKeyPad.Substring(0, ((this.ContentTotalKeyPad.Length - 1) - (this.ContentKorKeyPad.Length - 1)));
                    }
                }

                if (isShift)
                {
                    this._ContentKorPad = _smallKey.ToUpper().ToCharArray()[0];
                }
                else
                {
                    this._ContentKorPad = _smallKey.ToLower().ToCharArray()[0];
                }
                this._hangul.Input(ref ContentKorKeyPad, this._ContentKorPad);
                this.ContentTotalKeyPad += this.ContentKorKeyPad;
            }
            this.txtKeyPad.Text = this.ContentTotalKeyPad;
        }

    }
}
