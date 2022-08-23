using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Sale;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using AquilaErpWpfApp3.Util;

namespace AquilaErpWpfApp3.SAL.View.Dialog
{
    public partial class S2231MasterDialog : DXWindow
    {
        //private static SaleOrderServiceClient saleClient = SystemProperties.SaleOrderClient;
        private SaleVo orgDao;
        private bool isEdit = false;
        private SaleVo updateDao;

        private string title = "통장관리";

        public S2231MasterDialog(SaleVo Dao)
        {
            InitializeComponent();
            //
            this.orgDao = Dao;
            SaleVo copyDao = new SaleVo()
            {
                BANK_ACCT_CD = Dao.BANK_ACCT_CD,
                BANK_ACCT_NM = Dao.BANK_ACCT_NM,
                BANK_TP_NM = Dao.BANK_TP_NM,
                BANK_RMK = Dao.BANK_RMK,
                BANK_NM = Dao.BANK_NM,
                CRE_USR_ID = Dao.CRE_USR_ID,
                UPD_USR_ID = Dao.UPD_USR_ID
            };

            if (Dao.BANK_ACCT_CD != null)
            {
                this.isEdit = true;

                this.text_BANK_ACCT_CD.IsEnabled = false;
                this.text_BANK_ACCT_CD.Background = Brushes.DarkGray;
            }
            else
            {
                this.isEdit = false;
            }
            this.configCode.DataContext = copyDao;
            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
            this.OKButton.Click += new RoutedEventHandler(OKButton_Click);
            this.CancelButton.Click += new RoutedEventHandler(CancelButton_Click);

        }

      

        #region Functon (OKButton_Click, CancelButton_Click)
        private async void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValueCheckd())
            {
                int _Num = 0;
                //SaleVo resultVo;
                if (isEdit == false)
                {
                    this.updateDao = getDomain();//this.updateDao
                    this.ResultDao = this.updateDao;
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s2231/i", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
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
                    this.ResultDao = this.updateDao;
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s2231/u", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
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
        #endregion

        #region Functon (ValueCheckd)
        public Boolean ValueCheckd()
        {
            if (string.IsNullOrEmpty(this.text_BANK_ACCT_CD.Text))
            {
                WinUIMessageBox.Show("[코드] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.text_BANK_ACCT_CD.IsTabStop = true;
                this.text_BANK_ACCT_CD.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(this.text_BANK_ACCT_NM.Text))
            {
                WinUIMessageBox.Show("[통장 계좌] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.text_BANK_ACCT_NM.IsTabStop = true;
                this.text_BANK_ACCT_NM.Focus();
                return false;
            }
            //else if (string.IsNullOrEmpty(this.combo_CO_NM.Text))
            //{
            //    WinUIMessageBox.Show("[거래처] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.combo_CO_NM.IsTabStop = true;
            //    this.combo_CO_NM.Focus();
            //    return false;
            //}
            //else if (string.IsNullOrEmpty(this.text_ST_APLY_DT.Text))
            //{
            //    WinUIMessageBox.Show("[사용 시작] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.text_ST_APLY_DT.IsTabStop = true;
            //    this.text_ST_APLY_DT.Focus();
            //    return false;
            //}
            //else if (string.IsNullOrEmpty(this.text_END_APLY_DT.Text))
            //{
            //    WinUIMessageBox.Show("[사용 종료] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.text_END_APLY_DT.IsTabStop = true;
            //    this.text_END_APLY_DT.Focus();
            //    return false;
            //}
            else
            {
                //if (this.isEdit == false)
                //{
                //    SaleVo dao = new SaleVo()
                //    {
                //        BANK_ACCT_CD = this.text_BANK_ACCT_CD.Text
                //    };
                //    //ObservableCollection<SystemCodeVo> daoList = service.SearchDetailConfigView1(dao);
                //    //IList<SaleVo> daoList = (IList<SaleVo>)saleClient.S2231SelectMstList(dao);
                //    //if (daoList.Count != 0)
                //    //{
                //    //    WinUIMessageBox.Show("[코드 - 중복] 코드를 다시 입력 하십시오.", "[중복검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                //    //    this.text_BANK_ACCT_CD.IsTabStop = true;
                //    //    this.text_BANK_ACCT_CD.Focus();
                //    //    return false;
                //    //}
                //}
            }
            return true;
        }
        #endregion

        #region Functon (getDomain - ConfigView1Dao)
        public SaleVo getDomain()
        {
            SaleVo Dao = new SaleVo();

            Dao.BANK_ACCT_CD = this.text_BANK_ACCT_CD.Text;
            Dao.BANK_ACCT_NM = this.text_BANK_ACCT_NM.Text;

            Dao.BANK_TP_NM = this.text_BANK_TP_NM.Text;
            Dao.BANK_NM = this.text_BANK_NM.Text;

            Dao.BANK_RMK = this.text_BANK_RMK.Text;

            Dao.CRE_USR_ID = SystemProperties.USER;
            Dao.UPD_USR_ID = SystemProperties.USER;
            Dao.CHNL_CD= SystemProperties.USER_VO.CHNL_CD;
            return Dao;
        }
        #endregion

        //IsEdit
        public bool IsEdit
        {
            get
            {
                return this.isEdit;
            }
        }

        public SaleVo ResultDao
        {
            get;
            set;
        }
        
    }
}
