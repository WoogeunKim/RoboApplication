using AquilaErpWpfApp3.Util;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Code;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AquilaErpWpfApp3.View.S.Dialog
{
    /// <summary>
    /// S11472MasterDialog.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class S11472MasterDialog : DXWindow
    {
        private string _title = "창고위치관리";
        //private static ManServiceClient manClient = SystemProperties.ManClient;
        private SystemCodeVo orgDao;
        private bool isEdit = false;
        private SystemCodeVo updateDao;
        public S11472MasterDialog(SystemCodeVo Dao)
        {
            InitializeComponent();

            SYSTEM_CODE_VO();

            this.orgDao = Dao;

            SystemCodeVo copyDao = new SystemCodeVo()
            {
                AREA_CD     = Dao.AREA_CD,
                AREA_NM     = Dao.AREA_NM,
                N1ST_LOC_ID = Dao.N1ST_LOC_ID,
                N1ST_LOC_NM = Dao.N1ST_LOC_NM,
                N2ND_LOC_ID = Dao.N2ND_LOC_ID,
                N2ND_LOC_NM = Dao.N2ND_LOC_NM,
                N3RD_LOC_ID = Dao.N3RD_LOC_ID,
                LOC_NM      = Dao.LOC_NM,
                INV_CAPA_WGT = Dao.INV_CAPA_WGT,
                LOC_DESC    = Dao.LOC_DESC,
                CRE_USR_ID  = Dao.CRE_USR_ID,
                UPD_USR_ID  = Dao.UPD_USR_ID,
                CHNL_CD     = Dao.CHNL_CD
            };


            if (!string.IsNullOrEmpty(Dao.N1ST_LOC_ID) && !string.IsNullOrEmpty(Dao.N2ND_LOC_ID))
            {
                this.isEdit = true;
            }


            this.configCode.DataContext = copyDao;
            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
            this.OKButton.Click += new RoutedEventHandler(OKButton_Click);
            this.CancelButton.Click += new RoutedEventHandler(CancelButton_Click);
        }

        #region 이벤트

        private async void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValueCheckd())
            {

                int _Num = 0;
                //ProgramVo resultVo;
                if (isEdit == false)
                {
                    this.updateDao = getDomain();//this.updateDao
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("S11472/i", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string result = await response.Content.ReadAsStringAsync();
                            if (int.TryParse(result, out _Num) == false)
                            {
                                //실패
                                WinUIMessageBox.Show(result, _title, MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }

                            //성공
                            WinUIMessageBox.Show("완료 되었습니다", _title, MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }

                else
                {
                    this.updateDao = getDomain();

                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("S11472/u", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string result = await response.Content.ReadAsStringAsync();
                            if (int.TryParse(result, out _Num) == false)
                            {
                                //실패
                                WinUIMessageBox.Show(result, _title, MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }

                            //성공
                            WinUIMessageBox.Show("완료 되었습니다", _title, MessageBoxButton.OK, MessageBoxImage.Information);
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

        #region 유효성검사

        public Boolean ValueCheckd()
        {
            if (string.IsNullOrEmpty(this.combo_AREA_CD.Text))
            {
                WinUIMessageBox.Show("[사업장] 입력 값이 맞지 않습니다.", "[유효검사]창고위치관리(완제품)", MessageBoxButton.OK, MessageBoxImage.Warning);
                this.combo_AREA_CD.IsTabStop = true;
                this.combo_AREA_CD.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(this.combo_N1ST_LOC_NM.Text))
            {
                WinUIMessageBox.Show("[위치(동)] 입력 값이 맞지 않습니다.", "[유효검사]창고위치관리(완제품)", MessageBoxButton.OK, MessageBoxImage.Warning);
                this.combo_N1ST_LOC_NM.IsTabStop = true;
                this.combo_N1ST_LOC_NM.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(this.combo_N2ND_LOC_NM.Text))
            {
                WinUIMessageBox.Show("[열] 입력 값이 맞지 않습니다.", "[유효검사]창고위치관리(완제품)", MessageBoxButton.OK, MessageBoxImage.Warning);
                this.combo_N2ND_LOC_NM.IsTabStop = true;
                this.combo_N2ND_LOC_NM.Focus();
                return false;
            }

            return true;
        }
        #endregion



        #region getDomain(), SYSTEM_CODE_VO()

        private SystemCodeVo getDomain()
        {
            SystemCodeVo Dao = new SystemCodeVo();

            SystemCodeVo areaVo = this.combo_AREA_CD.SelectedItem as SystemCodeVo;
            Dao.AREA_CD = areaVo.CLSS_CD;
            Dao.AREA_NM = areaVo.CLSS_DESC;

            SystemCodeVo n1stVo = this.combo_N1ST_LOC_NM.SelectedItem as SystemCodeVo;
            Dao.N1ST_LOC_ID      = n1stVo.CLSS_CD;
            Dao.N1ST_LOC_NM      = n1stVo.CLSS_DESC;
            Dao.N1ST_LOC_ID_TEMP = this.orgDao.N1ST_LOC_ID;

            SystemCodeVo n2ndVo = this.combo_N2ND_LOC_NM.SelectedItem as SystemCodeVo;
            Dao.N2ND_LOC_ID      = n2ndVo.CLSS_CD;
            Dao.N2ND_LOC_NM      = n2ndVo.CLSS_DESC;
            Dao.N2ND_LOC_ID_TEMP = this.orgDao.N2ND_LOC_ID;

            Dao.INV_CAPA_WGT = this.text_INV_CAPA_WGT.Text;

            Dao.LOC_DESC = this.text_LOC_DESC.Text;

            Dao.CRE_USR_ID = SystemProperties.USER;
            Dao.UPD_USR_ID = SystemProperties.USER;
            Dao.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;

            return Dao;
        }


        public async void SYSTEM_CODE_VO()
        {
            //this.combo_AREA_CD.ItemsSource = SystemProperties.SYSTEM_CODE_VO("L-002");
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "L-002"))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.combo_AREA_CD.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                }
            }

            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "C-008"))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.combo_N1ST_LOC_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                }
            }

            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "C-009"))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.combo_N2ND_LOC_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                }
            }
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

        //Vo
        public SystemCodeVo resultDomain
        {
            get
            {
                return this.updateDao;
            }
        }

    }
}
