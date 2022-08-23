using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Code;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;
using AquilaErpWpfApp3.Util;

namespace AquilaErpWpfApp3.S.View.Dialog
{
    /// <summary>
    /// Interaction logic for S1147MasterDialog.xaml
    /// </summary>
    public partial class S1161MasterDialog : DXWindow
    {
        //private static SaleOrderServiceClient saleOrderClient = SystemProperties.SaleOrderClient;
        private SystemCodeVo orgDao;
        private bool isEdit = false;
        private SystemCodeVo updateDao;
        private string title = "근태기준정보관리";

        public S1161MasterDialog(SystemCodeVo Dao)
        {
            InitializeComponent();

            //this.combo_AREA_NM.ItemsSource = SystemProperties.SYSTEM_CODE_VO("L-002");
            SYSTEM_CODE_VO();

            this.orgDao = Dao;

            SystemCodeVo copyDao = new SystemCodeVo()
            {
                WRK_TP_CD = Dao.WRK_TP_CD,
                WRK_TP_NM = Dao.WRK_TP_NM,
                ST_HR_MNT = Dao.ST_HR_MNT,
                END_HR_MNT = Dao.END_HR_MNT,
                WRK_TP_VAL = Dao.WRK_TP_VAL,
                CHNL_CD = Dao.CHNL_CD
            };

            if (!string.IsNullOrEmpty(copyDao.WRK_TP_CD))
            {
                //this.combo_AREA_NM.IsEnabled = false;
                //this.text_loc_cd.IsEnabled = false;
                this.text_WRK_TP_CD.IsEnabled = false;
                this.isEdit = true;
            }

            this.configCode.DataContext = copyDao;
            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
            this.OKButton.Click += new RoutedEventHandler(OKButton_Click);
            this.CancelButton.Click += new RoutedEventHandler(CancelButton_Click);
        }

        #region Functon (ValueCheckd)
        public Boolean ValueCheckd()
        {
            if (string.IsNullOrEmpty(this.text_WRK_TP_CD.Text))
            {
                WinUIMessageBox.Show("[코드] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.text_WRK_TP_CD.IsTabStop = true;
                this.text_WRK_TP_CD.Focus();
                return false;
            }
            else
            {
                //if (this.isEdit == false)
                //{
                //    SaleVo dao = new SaleVo();
                //    //CustomerCodeDao coNmVo = this.combo_CLT_CO_NM.SelectedItem as CustomerCodeDao;
                //    //if (coNmVo != null)
                //    //{
                //    //    dao.CO_CD = coNmVo.CO_NO;
                //    //    dao.CO_NM = coNmVo.CO_NM;
                //    //}

                //    CodeDao areaNmVo = this.combo_AREA_NM.SelectedItem as CodeDao;
                //    if (areaNmVo != null)
                //    {
                //        dao.AREA_CD = areaNmVo.CLSS_CD;
                //        dao.AREA_NM = areaNmVo.CLSS_DESC;
                //    }

                //    //JobVo daoList = (JobVo)saleOrderClient.S2219SelectCheck(dao);
                //    //if (daoList != null)
                //    //{
                //    //    WinUIMessageBox.Show("[코드 - 중복] 코드를 다시 입력 하십시오.", "[중복검사]" + this.title, MessageBoxButton.OK, MessageBoxImage.Warning);
                //    //    this.combo_AREA_NM.IsTabStop = true;
                //    //    this.combo_AREA_NM.Focus();
                //    //    return false;
                //    //}
                //}
            }
            return true;
        }
        #endregion

        #region Functon (OKButton_Click, CancelButton_Click)
        private async void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValueCheckd())
            {
                int _Num = 0;
                //SystemCodeVo resultVo;
                if (isEdit == false)
                {
                    this.updateDao = getDomain();

                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s1161/i", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
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

                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s1161/u", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
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

        #region Functon (getDomain - ConfigView1Dao)
        private SystemCodeVo getDomain()
        {
            SystemCodeVo Dao = new SystemCodeVo();

            //SystemCodeVo areaNmVo = this.combo_AREA_NM.SelectedItem as SystemCodeVo;
            //if (areaNmVo != null)
            //{
            //    Dao.AREA_CD = areaNmVo.CLSS_CD;
            //    Dao.AREA_NM = areaNmVo.CLSS_DESC;
            //}
            Dao.WRK_TP_CD = this.text_WRK_TP_CD.Text;
            Dao.WRK_TP_NM = this.text_WRK_TP_NM.Text;
            Dao.ST_HR_MNT = this.text_ST_HR_MNT.Text;
            Dao.END_HR_MNT = this.text_END_HR_MNT.Text;
            Dao.WRK_TP_VAL = this.text_WRK_TP_VAL.Text;

            Dao.CRE_USR_ID = SystemProperties.USER;
            Dao.UPD_USR_ID = SystemProperties.USER;
            Dao.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;

            return Dao;
        }
        #endregion


        public async void SYSTEM_CODE_VO()
        {
            //using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "L-002"))
            //{
            //    if (response.IsSuccessStatusCode)
            //    {
            //        this.combo_AREA_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
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
        public SystemCodeVo resultDao
        {
            get
            {
                return this.updateDao;
            }
        }
    }
}
