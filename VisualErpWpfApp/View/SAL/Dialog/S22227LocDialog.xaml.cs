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
using AquilaErpWpfApp3.Util;

namespace AquilaErpWpfApp3.View.SAL.Dialog
{
    public partial class S22227LocDialog : DXWindow
    {
        private SaleVo orgDao;
        public SaleVo updateDao;

        private string title = "GR번호/LOC관리";

        public S22227LocDialog(SaleVo Dao)
        {
            InitializeComponent();

            this.orgDao = Dao;

            SYSTEM_CODE_VO();


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

                this.updateDao = getDomain();

                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("S22227/pop/loc/u", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
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
            if (string.IsNullOrEmpty(this.combo_LOC_NM.Text))
            {
                WinUIMessageBox.Show("[창고명] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.combo_LOC_NM.IsTabStop = true;
                this.combo_LOC_NM.Focus();
                return false;
            }
            return true;
        }
        #endregion

        #region Functon (getDomain)
        private SaleVo getDomain()
        {
            SaleVo Dao = new SaleVo();
            Dao.SL_RLSE_NO = this.orgDao.SL_RLSE_NO;
            Dao.RLSE_CMD_NO = this.orgDao.RLSE_CMD_NO;
            Dao.DELT_FLG = "N";

            SaleVo coNmVo = this.combo_LOC_NM.SelectedItem as SaleVo;
            if (coNmVo != null)
            {
                Dao.LOC_CD = coNmVo.LOC_CD;
                Dao.LOC_NM = coNmVo.LOC_NM;
            }
            Dao.LOC_RMK = this.orgDao.LOC_CD;

            Dao.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;

            return Dao;
        }
        #endregion


        public async void SYSTEM_CODE_VO()
        {
            try
            {
                // 창고
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("S22227/pop/loc", new StringContent(JsonConvert.SerializeObject(this.orgDao), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.combo_LOC_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SaleVo>>(await response.Content.ReadAsStringAsync()).Cast<SaleVo>().ToList();
                    }
                }
            }
            catch (Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, title, MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

    }
}
