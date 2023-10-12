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
using ModelsLibrary.Auth;
using System.Threading.Tasks;

namespace AquilaErpWpfApp3.View.SAL.Dialog
{
    public partial class S22111MasterDialog : DXWindow
    {
        private SaleVo orgDao;
        private bool isEdit = false;
        private SaleVo updateDao;

        private string title = "출고 등록";

        public S22111MasterDialog(SaleVo Dao)
        {
            InitializeComponent();


            SYSTEM_CODE_VO();

            this.orgDao = Dao;

            SaleVo copyDao = new SaleVo()
            {
                SL_BIL_NO = Dao.SL_BIL_NO,
                SL_AREA_NM = Dao.SL_AREA_NM,
                SL_AREA_CD = Dao.SL_AREA_CD,
                SL_CO_NM = Dao.SL_CO_NM,
                SL_CO_CD = Dao.SL_CO_CD,
                CAR_NO = Dao.CAR_NO,
                //CAR_NM = Dao.CAR_NM,
                SL_BIL_DT = Dao.SL_BIL_DT,
                //MV_CAR_AMT = Dao.MV_CAR_AMT,
                CLR_TXT = Dao.CLR_TXT,
                SL_BIL_RMK = Dao.SL_BIL_RMK,
                USR_NM = SystemProperties.USER_VO.USR_N1ST_NM
            };

            //수정
            if (Dao.SL_BIL_NO != null)
            {

                this.text_SL_BIL_NO.IsReadOnly = true;
                this.isEdit = true;
                //
                ////마감 처리 후 수정 불가능
                //if (Dao.MODI_FLG.Equals("N"))
                //{
                //    this.OKButton.IsEnabled = false;
                //}

            }
            else
            {
                //추가
                this.text_SL_BIL_NO.IsReadOnly = true;
                this.isEdit = false;
                copyDao.SL_BIL_DT = System.DateTime.Now.ToString("yyyy-MM-dd");
            }
            this.configCode.DataContext = copyDao;
            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
            this.OKButton.Click += new RoutedEventHandler(OKButton_Click);
            this.CancelButton.Click += new RoutedEventHandler(CancelButton_Click);


            this.combo_SL_AREA_NM.SelectedIndexChanged += combo_SL_AREA_NM_SelectedIndexChanged;

            this.combo_SL_CO_NM.Focus();
        }


        async void combo_SL_AREA_NM_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            SystemCodeVo areaNmVo = this.combo_SL_AREA_NM.SelectedItem as SystemCodeVo;
            if (areaNmVo != null)
            {
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s143", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { DELT_FLG = "N", CO_TP_CD = "AR", AREA_CD = SystemProperties.USER_VO.EMPE_PLC_NM, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.combo_SL_CO_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                    }
                }

            }
        }


        #region Functon (OKButton_Click, CancelButton_Click)
        private async void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValueCheckd())
            {
                int _Num = 0;
                if (isEdit == false)
                {
                    this.updateDao = getDomain();
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s221111/mst/i", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
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

                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s221111/mst/u", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
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
                SL_BIL_NO = this.updateDao.SL_BIL_NO;
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
            if (string.IsNullOrEmpty(this.combo_SL_AREA_NM.Text))
            {
                WinUIMessageBox.Show("[사업장] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.combo_SL_AREA_NM.IsTabStop = true;
                this.combo_SL_AREA_NM.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(this.combo_SL_CO_NM.Text))
            {
                WinUIMessageBox.Show("[거래처] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.combo_SL_CO_NM.IsTabStop = true;
                this.combo_SL_CO_NM.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(this.combo_CAR_NM.Text))
            {
                WinUIMessageBox.Show("[배차번호] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.combo_CAR_NM.IsTabStop = true;
                this.combo_CAR_NM.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(this.text_SL_BIL_DT.Text))
            {
                WinUIMessageBox.Show("[출하일자] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.text_SL_BIL_DT.IsTabStop = true;
                this.text_SL_BIL_DT.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(this.text_USR_NM.Text))
            {
                WinUIMessageBox.Show("[작성자] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.text_USR_NM.IsTabStop = true;
                this.text_USR_NM.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(this.text_CLR_TXT.Text))
            {
                WinUIMessageBox.Show("[색상] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.text_CLR_TXT.IsTabStop = true;
                this.text_CLR_TXT.Focus();
                return false;
            }
            //else if (string.IsNullOrEmpty(this.text_MV_CAR_AMT.Text))
            //{
            //    WinUIMessageBox.Show("[운임료] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.text_MV_CAR_AMT.IsTabStop = true;
            //    this.text_MV_CAR_AMT.Focus();
            //    return false;
            //}
            return true;
        }
        #endregion

        #region Functon (getDomain - ConfigView1Dao)
        private SaleVo getDomain()
        {
            SaleVo Dao = new SaleVo();
            //운송장번호
            Dao.SL_BIL_NO = this.text_SL_BIL_NO.Text;

            //사업장
            SystemCodeVo areaNmVo = this.combo_SL_AREA_NM.SelectedItem as SystemCodeVo;
            if (areaNmVo != null)
            {
                Dao.SL_AREA_CD = areaNmVo.CLSS_CD;
                Dao.SL_AREA_NM = areaNmVo.CLSS_DESC;
            }

            //거래처
            SystemCodeVo coNmVo = this.combo_SL_CO_NM.SelectedItem as SystemCodeVo;
            if (coNmVo != null)
            {
                Dao.SL_CO_CD = coNmVo.CO_NO;
                Dao.SL_CO_NM = coNmVo.CO_NM;
            }

            //배차번호
            SaleVo carNo = this.combo_CAR_NM.SelectedItem as SaleVo;
            if (carNo != null)
            {
                Dao.CAR_NO = carNo.CAR_NO;
                Dao.CAR_NM = carNo.CAR_NM;
            }
            //출하일자
            Dao.SL_BIL_DT = this.text_SL_BIL_DT.Text;

            //운임료
            //Dao.MV_CAR_AMT = double.Parse(this.text_MV_CAR_AMT.Text);

            //색상
            Dao.CLR_TXT = this.text_CLR_TXT.Text;

            //비고
            Dao.SL_BIL_RMK = this.text_SL_BIL_RMK.Text;

            Dao.CRE_USR_ID = SystemProperties.USER;
            Dao.UPD_USR_ID = SystemProperties.USER;
            Dao.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;

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

        public string SL_BIL_NO
        {
            get;
            set;
        }



        public async void SYSTEM_CODE_VO()
        {

            try
            {
                //this.combo_SL_AREA_NM.ItemsSource = SystemProperties.SYSTEM_CODE_VO("L-002");
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "L-002"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.combo_SL_AREA_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                    }
                }

                //당진공장 default
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s143", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { DELT_FLG = "N", SEEK = "", CO_TP_CD = "AR", SEEK_AR = "AR", CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                {
                    //IList<SystemCodeVo> Covo = new List<SystemCodeVo>();

                    if (response.IsSuccessStatusCode)
                    {
                        this.combo_SL_CO_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();

                        //this.combo_SL_CO_NM.ItemsSource = Covo;
                        //this.combo_SL_CO_NM.SelectedItem = Covo.Where<SystemCodeVo>(x => x.CO_NO.Equals("002")).LastOrDefault<SystemCodeVo>();
                    }
                }

                //배차번호
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s1150", new StringContent(JsonConvert.SerializeObject(new SaleVo()), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.combo_CAR_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SaleVo>>(await response.Content.ReadAsStringAsync()).Cast<SaleVo>().ToList();
                    }
                }
            }
            catch (System.Exception eLog)
            {
                //실패
                WinUIMessageBox.Show(eLog.ToString(), title, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

    }
}