using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Code;
using ModelsLibrary.Sale;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using AquilaErpWpfApp3.Util;

namespace AquilaErpWpfApp3.SAL.View.Dialog
{
    public partial class S11410MasterDialog : DXWindow
    {
        //private static SaleOrderServiceClient saleClient = SystemProperties.SaleOrderClient;
        private SaleVo orgDao;
        private bool isEdit = false;
        private SaleVo updateDao;

        private string title = "품목군등록";

        public S11410MasterDialog(SaleVo Dao)
        {
            InitializeComponent();
            //
            SYSTEM_CODE_VO();


            this.orgDao = Dao;
            SaleVo copyDao = new SaleVo()
            {
                CAR_TP_CD = Dao.CAR_TP_CD,
                CAR_TP_NM = Dao.CAR_TP_NM,
                CAR_TP_SUB_NM = Dao.CAR_TP_SUB_NM,
                CO_CD = Dao.CO_CD,
                CO_NM = Dao.CO_NM,
                DELT_FLG = Dao.DELT_FLG,
                CRE_USR_ID = Dao.CRE_USR_ID,
                UPD_USR_ID = Dao.UPD_USR_ID
            };

            if (Dao.CAR_TP_CD != null)
            {
                this.isEdit = true;

                this.text_CAR_TP_CD.IsEnabled = false;
                this.text_CAR_TP_CD.Background = Brushes.DarkGray;
            }
            else
            {
                this.isEdit = false;
                copyDao.DELT_FLG = "사용";
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
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("S11410/i", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
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
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("S11410/u", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
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
            if (string.IsNullOrEmpty(this.text_CAR_TP_CD.Text))
            {
                WinUIMessageBox.Show("[품목코드] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.text_CAR_TP_CD.IsTabStop = true;
                this.text_CAR_TP_CD.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(this.text_CAR_TP_NM.Text))
            {
                WinUIMessageBox.Show("[품목명] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.text_CAR_TP_NM.IsTabStop = true;
                this.text_CAR_TP_NM.Focus();
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

            Dao.CAR_TP_CD = this.text_CAR_TP_CD.Text;
            Dao.CAR_TP_NM = this.text_CAR_TP_NM.Text;

            Dao.CAR_TP_SUB_NM = this.text_CAR_TP_SUB_NM.Text;

            //SystemCodeVo coNmVo = this.combo_CO_NM.SelectedItem as SystemCodeVo;
            //if (coNmVo != null)
            //{
            //    Dao.CO_CD = coNmVo.CO_NO;
            //    Dao.CO_NM = coNmVo.CO_NM;
            //}

            Dao.DELT_FLG = (this.combo_DELT_FLG.Text.Equals("사용") ? "N" : "Y");
            Dao.CRE_USR_ID = SystemProperties.USER;
            Dao.UPD_USR_ID = SystemProperties.USER;
            Dao.CHNL_CD= SystemProperties.USER_VO.CHNL_CD;
            return Dao;
        }
        #endregion

        public async void SYSTEM_CODE_VO()
        {
            //this.combo_SL_CO_NM.ItemsSource = SystemProperties.CUSTOMER_CODE_VO("AR", "100");
            //using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s143", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { DELT_FLG = "N", SEEK = "", CO_TP_CD = "AR", SEEK_AR = "AR", AREA_CD = SystemProperties.USER_VO.EMPE_PLC_NM, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
            //{
            //    if (response.IsSuccessStatusCode)
            //    {
            //        this.combo_CO_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
            //    }
            //}
        }

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
