using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Code;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;
using AquilaErpWpfApp3.Util;

namespace AquilaErpWpfApp3.S.View.Dialog
{
    /// <summary>
    /// Interaction logic for S1147MasterDialog.xaml
    /// </summary>
    public partial class S1147MasterDialog : DXWindow
    {
        //private static SaleOrderServiceClient saleOrderClient = SystemProperties.SaleOrderClient;
        private SystemCodeVo orgDao;
        private bool isEdit = false;
        private SystemCodeVo updateDao;
        private string title = "사업장별창고관리";

        public S1147MasterDialog(SystemCodeVo Dao)
        {
            InitializeComponent();

            //this.combo_AREA_NM.ItemsSource = SystemProperties.SYSTEM_CODE_VO("L-002");
            SYSTEM_CODE_VO();

            this.orgDao = Dao;

            SystemCodeVo copyDao = new SystemCodeVo()
            {
                AREA_CD = Dao.AREA_CD,
                AREA_NM = Dao.AREA_NM,
                LOC_CD = Dao.LOC_CD,
                LOC_NM = Dao.LOC_NM,
                LOC_RMK = Dao.LOC_RMK,
                CHNL_CD = Dao.CHNL_CD,

                CRE_USR_ID = Dao.CRE_USR_ID,
                UPD_USR_ID = Dao.UPD_USR_ID
            };

            if (!string.IsNullOrEmpty(copyDao.LOC_CD + copyDao.LOC_NM))
            {
                this.combo_AREA_NM.IsEnabled = false;
                this.text_loc_cd.IsEnabled = false;
                this.text_loc_nm.IsEnabled = false;
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
            if (string.IsNullOrEmpty(this.combo_AREA_NM.Text))
            {
                WinUIMessageBox.Show("[사업장] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.combo_AREA_NM.IsTabStop = true;
                this.combo_AREA_NM.Focus();
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

                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s1147/i", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
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

                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s1147/u", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
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

            SystemCodeVo areaNmVo = this.combo_AREA_NM.SelectedItem as SystemCodeVo;
            if (areaNmVo != null)
            {
                Dao.AREA_CD = areaNmVo.CLSS_CD;
                Dao.AREA_NM = areaNmVo.CLSS_DESC;
            }
            Dao.LOC_CD = this.text_loc_cd.Text;
            Dao.LOC_NM = this.text_loc_nm.Text;
            Dao.LOC_RMK = this.text_loc_rmk.Text;

            Dao.CRE_USR_ID = SystemProperties.USER;
            Dao.UPD_USR_ID = SystemProperties.USER;
            Dao.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;

            return Dao;
        }
        #endregion


        public async void SYSTEM_CODE_VO()
        {
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "L-002"))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.combo_AREA_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                }
            }
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
