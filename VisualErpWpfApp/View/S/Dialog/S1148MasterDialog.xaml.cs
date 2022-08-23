using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Code;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;
using AquilaErpWpfApp3.Util;

namespace AquilaErpWpfApp3.View.S.Dialog
{
    /// <summary>
    /// S1148MasterDialog.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class S1148MasterDialog : DXWindow
    {
        private string title = "도면기준관리";


        private SystemCodeVo updateDao;

        public S1148MasterDialog(SystemCodeVo Dao)
        {
            InitializeComponent();

            SystemCodeVo MstDialog = new SystemCodeVo()
            {
                SHP_CO_CD = Dao.SHP_CO_CD,
                SHP_CLSS_COL_NM = Dao.SHP_CLSS_COL_NM,
                SHP_SEQ = Dao.SHP_SEQ,
                SHP_NO = Dao.SHP_NO,
                SHP_TP_IMG = Dao.SHP_TP_IMG,
                SHP_SZ = Dao.SHP_SZ,
                SHP_ITM_SZ = Dao.SHP_ITM_SZ,
                SHP_ITM_QTY = Dao.SHP_ITM_QTY,
                SHP_ITM_LEN = Dao.SHP_ITM_LEN,
                SHP_ITM_WGT = Dao.SHP_ITM_WGT,
                CHNL_CD = Dao.CHNL_CD
            };

            // Update
            if (MstDialog.SHP_CO_CD != null)
            {
                this.text_SHP_CO.IsReadOnly = true;
                this.isEdit = true;
            }
            else
            {
                // SHP_CO_CD 지정된 값을 넣어주는 곳이이이이이이이이이이이이이이이이이이다 
                this.text_SHP_CO.IsReadOnly = false;
                this.isEdit = false;
            }

            this.configCode.DataContext = MstDialog;
            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
            this.OKButton.Click += new RoutedEventHandler(OKButton_Click);
            this.CancelButton.Click += new RoutedEventHandler(CancelButton_Click);

        }

        private async void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValueCheckd())
            {
                int _Num = 0;
                //SystemCodeVo resultVo;

                // Insert
                if (isEdit == false)
                {
                    this.updateDao = getDomain();

                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s1148/i", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string result = await response.Content.ReadAsStringAsync();
                            if (int.TryParse(result, out _Num) == false)
                            {
                                //실패
                                WinUIMessageBox.Show(result, title, MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }

                            //성공
                            WinUIMessageBox.Show("완료 되었습니다", title, MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }
                else
                {
                    this.updateDao = getDomain();

                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s1148/u", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string result = await response.Content.ReadAsStringAsync();
                            if (int.TryParse(result, out _Num) == false)
                            {
                                //실패
                                WinUIMessageBox.Show(result, title, MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }

                            //성공
                            WinUIMessageBox.Show("완료 되었습니다", title, MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }
                this.DialogResult = true;
                this.Close();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }


        private void HandleEsc(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.DialogResult = false;
                Close();
            }
        }

        // CHNL_CD 필요시 추가 , 현재 NULL 조건만 있음 글자수의 조건은 없는지...
        public Boolean ValueCheckd()
        {
            /*if (this.text_SHP_CO.Text == null || this.text_SHP_CO.Text.Trim().Length == 0) // this.text_SHP_CO.Text.Trim().Length == 0
            {
                WinUIMessageBox.Show("[도면업체코드] 값이 맞지 않습니다.", "[유효검사] " + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.text_SHP_CO.IsTabStop = true;
                this.text_SHP_CO.Focus();
                return false;
            }*/
            if (this.text_SHP_CLSS.Text == null || this.text_SHP_CLSS.Text.Trim().Length == 0)
            {
                WinUIMessageBox.Show("[도면업체] 입력 값이 없습니다.", "[유효검사] " + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.text_SHP_CO.IsTabStop = true;
                this.text_SHP_CO.Focus();
                return false;
            }

            return true;
        }

        // 내보낼때 밥그릇
        private SystemCodeVo getDomain()
        {
            SystemCodeVo Dao = new SystemCodeVo();
            Dao.SHP_CO_CD = this.text_SHP_CO.Text;
            Dao.SHP_CLSS_COL_NM = this.text_SHP_CLSS.Text;
            Dao.SHP_SEQ = this.text_SHP_SEQ.Text;
            Dao.SHP_NO = this.text_SHP_NO.Text;
            Dao.SHP_TP_IMG = this.text_SHP_TP_IMG.Text;
            Dao.SHP_SZ = this.text_SHP_SZ.Text;
            Dao.SHP_ITM_SZ = this.text_SHP_ITM_SZ.Text;
            Dao.SHP_ITM_QTY = this.text_SHP_ITM_QTY.Text;
            Dao.SHP_ITM_LEN = this.text_SHP_ITM_LEN.Text;
            Dao.SHP_ITM_WGT = this.text_SHP_ITM_WGT.Text;
            Dao.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
            return Dao;
        }

        // 추가인지 업데이트인지 확인하기 위한 변수 - 처음에 PK값이 있으면 true, 없으면 flase 로 반환할 때 확인해서 반환
        private bool isEdit = false;
        public bool IsEdit
        {
            get { return this.isEdit; }
        }

        public SystemCodeVo resultDao
        {
            get
            {
                return this.updateDao;
            }
        }
    }
}
