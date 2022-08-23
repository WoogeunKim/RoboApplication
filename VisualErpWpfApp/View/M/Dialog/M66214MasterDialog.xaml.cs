using AquilaErpWpfApp3.Util;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Code;
using ModelsLibrary.Man;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;

namespace AquilaErpWpfApp3.View.M.Dialog
{
    /// <summary>
    /// Interaction logic for ConfigView1Dialog.xaml
    /// </summary>
    public partial class M66214MasterDialog : DXWindow
    {
        private string _title = "(PDM)재공품번생성 및 제품데이터관리";
        //private static ManServiceClient manClient = SystemProperties.ManClient;
        private ManVo orgDao;
        private bool isEdit = false;
        private ManVo updateDao;

        public M66214MasterDialog(ManVo Dao, bool IsEdit)
        {
            InitializeComponent();
            //
            this.orgDao = Dao;


            SYSTEM_CODE_VO();

            ManVo copyDao = new ManVo()
            {
                MDL_CD = Dao.MDL_CD,
                MDL_NM = Dao.MDL_NM,
                ASSY_CD = Dao.ASSY_CD,
                ASSY_SEQ = Dao.ASSY_SEQ,
                CMPO_CD = Dao.CMPO_CD,
                CMPO_NM = Dao.CMPO_NM,
                INP_QTY = Dao.INP_QTY,
                BOM_DESC = Dao.BOM_DESC,
                ROUT_ITM_CD = Dao.ROUT_ITM_CD,
                ROUT_CD = Dao.ROUT_CD,
                ROUT_NM = Dao.ROUT_NM,
                ITM_CLSS_CD = Dao.ITM_CLSS_CD,
                ITM_CLSS_NM = Dao.ITM_CLSS_NM,
                SERS_COLR_CD = Dao.SERS_COLR_CD,
                SERS_COLR_NM = Dao.SERS_COLR_NM,
                ROUT_MZD_CD = Dao.ROUT_MZD_CD,
                ROUT_MZD_NM = Dao.ROUT_MZD_NM,
                COLR_DESC = Dao.COLR_DESC,
                CMPO_SZ_DESC = Dao.CMPO_SZ_DESC,
                CMPO_BRT_DESC = Dao.CMPO_BRT_DESC,
                CMPO_SUB_NM = Dao.CMPO_SUB_NM,
                BOM_DGR_SEQ = Dao.BOM_DGR_SEQ,
                CMPO_MTRL_DESC = Dao.CMPO_MTRL_DESC,
                DELT_FLG = Dao.DELT_FLG,
                CRE_USR_ID = Dao.CRE_USR_ID,
                UPD_USR_ID = Dao.UPD_USR_ID,
                CHNL_CD = Dao.CHNL_CD,
                UOM_NM = Dao.UOM_NM
            };

            if (copyDao.CMPO_CD != null)
            {
                //수정
                //this.isEdit = true;
                this.combo_ROUT_NM.IsReadOnly = true;
                this.combo_ITM_CLSS_NM.IsReadOnly = true;
                this.combo_SERS_COLR_NM.IsReadOnly = true;
            }
            else
            {
                //추가
                //this.text_ClssTpCd.IsReadOnly = false;
                //this.isEdit = false;
                copyDao.ASSY_SEQ = 1;
                copyDao.INP_QTY = 1;
                copyDao.DELT_FLG = "사용";
            }

            this.isEdit = IsEdit;

            this.configCode.DataContext = Dao;

            this.combo_ROUT_NM.SelectedIndexChanged += Combo_ROUT_NM_SelectedIndexChanged;

            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
            this.OKButton.Click += new RoutedEventHandler(OKButton_Click);
            this.CancelButton.Click += new RoutedEventHandler(CancelButton_Click);
        }

        private async void Combo_ROUT_NM_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                ManVo _selVo = this.combo_ROUT_NM.SelectedItem as ManVo;

                if (_selVo.ROUT_CD.Equals("CT"))
                {
                    //색상계열
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "L-023"))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            this.combo_ROUT_MZD_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                            if (string.IsNullOrEmpty(this.orgDao.ROUT_MZD_CD) == false)
                            {
                                this.combo_ROUT_MZD_NM.SelectedItem = (this.combo_ROUT_MZD_NM.ItemsSource as IEnumerable<SystemCodeVo>).Where<SystemCodeVo>(w => w.CLSS_CD.Equals(this.orgDao.ROUT_MZD_CD)).FirstOrDefault<SystemCodeVo>();
                            }
                        }
                    }
                }
                else if (_selVo.ROUT_CD.Equals("PR"))
                {
                    //색상계열
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "L-024"))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            this.combo_ROUT_MZD_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                            if (string.IsNullOrEmpty(this.orgDao.ROUT_MZD_CD) == false)
                            {
                                this.combo_ROUT_MZD_NM.SelectedItem = (this.combo_ROUT_MZD_NM.ItemsSource as IEnumerable<SystemCodeVo>).Where<SystemCodeVo>(w => w.CLSS_CD.Equals(this.orgDao.ROUT_MZD_CD)).FirstOrDefault<SystemCodeVo>();
                            }
                        }
                    }
                }
                else if (_selVo.ROUT_CD.Equals("IN"))
                {
                    //색상계열
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "L-025"))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            this.combo_ROUT_MZD_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                            if (string.IsNullOrEmpty(this.orgDao.ROUT_MZD_CD) == false)
                            {
                                this.combo_ROUT_MZD_NM.SelectedItem = (this.combo_ROUT_MZD_NM.ItemsSource as IEnumerable<SystemCodeVo>).Where<SystemCodeVo>(w => w.CLSS_CD.Equals(this.orgDao.ROUT_MZD_CD)).FirstOrDefault<SystemCodeVo>();
                            }
                        }
                    }
                }
                //else if (_selVo.ROUT_CD.Equals("CT"))
                //{

                //}
                //else if (_selVo.ROUT_CD.Equals("CT"))
                //{

                //}
                //
            } 
            catch (Exception eLog)
            {
                //실패
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        #region Functon (OKButton_Click, CancelButton_Click)
        private async void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValueCheckd())
            {

                int _Num = 0;
                //ProgramVo resultVo;
                if (isEdit == false)
                {
                    this.updateDao = getDomain();//this.updateDao
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m66213/mst/i", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
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

                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m66213/mst/u", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
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

        #region Functon (ValueCheckd)
        public Boolean ValueCheckd()
        {
            if (string.IsNullOrEmpty(this.combo_ROUT_NM.Text))
            {
                WinUIMessageBox.Show("[공정] 입력 값이 맞지 않습니다.", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.combo_ROUT_NM.IsTabStop = true;
                this.combo_ROUT_NM.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(this.combo_ITM_CLSS_NM.Text))
            {
                WinUIMessageBox.Show("[품목구분] 입력 값이 맞지 않습니다.", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.combo_ITM_CLSS_NM.IsTabStop = true;
                this.combo_ITM_CLSS_NM.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(this.combo_SERS_COLR_NM.Text))
            {
                WinUIMessageBox.Show("[색상계열] 입력 값이 맞지 않습니다.", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.combo_SERS_COLR_NM.IsTabStop = true;
                this.combo_SERS_COLR_NM.Focus();
                return false;
            }
            //else if (string.IsNullOrEmpty(this.text_CMPO_CD.Text))
            //{
            //    WinUIMessageBox.Show("[재공품번] 입력 값이 맞지 않습니다.", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.text_CMPO_CD.IsTabStop = true;
            //    this.text_CMPO_CD.Focus();
            //    return false;
            //}
            //else if (string.IsNullOrEmpty(this.text_CMPO_SUB_NM.Text))
            //{
            //    WinUIMessageBox.Show("[보조품명] 입력 값이 맞지 않습니다.", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.text_CMPO_SUB_NM.IsTabStop = true;
            //    this.text_CMPO_SUB_NM.Focus();
            //    return false;
            //}
            else if (string.IsNullOrEmpty(this.text_CMPO_NM.Text))
            {
                WinUIMessageBox.Show("[품명] 입력 값이 맞지 않습니다.", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.text_CMPO_NM.IsTabStop = true;
                this.text_CMPO_NM.Focus();
                return false;
            }
            //else if (this.text_SysFlg.Text == null || this.text_SysFlg.Text.Trim().Length == 0)
            //{
            //    WinUIMessageBox.Show("[구분] 입력 값이 맞지 않습니다.", "[유효검사]시스템 분류 코드", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.text_SysFlg.IsTabStop = true;
            //    this.text_SysFlg.Focus();
            //    return false;
            //}
            //else if (this.text_SysAreaCd.Text == null || this.text_SysAreaCd.Text.Trim().Length == 0)
            //{
            //    WinUIMessageBox.Show("[업무 분야] 입력 값이 맞지 않습니다.", "[유효검사]시스템 분류 코드", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.text_SysAreaCd.IsTabStop = true;
            //    this.text_SysAreaCd.Focus();
            //    return false;
            //}
            //else if (this.combo_deltFlg.Text == null || this.combo_deltFlg.Text.Trim().Length == 0)
            //{
            //    WinUIMessageBox.Show("[사용 여부] 입력 값이 맞지 않습니다.", "[유효검사]시스템 분류 코드", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.combo_deltFlg.IsTabStop = true;
            //    this.combo_deltFlg.Focus();
            //    return false;
            //}
            //else
            //{
            //if (this.isEdit == false)
            //{
            //    ManVo dao = new ManVo()
            //    {
            //        ROUT_CD = this.text_ROUT_CD.Text,
            //        CHNL_CD = SystemProperties.USER_VO.CHNL_CD,
            //    };
            //    IList<ManVo> daoList = (IList<ManVo>)manClient.M6611SelectMaster(dao);
            //    if (daoList.Count != 0)
            //    {
            //        WinUIMessageBox.Show("[코드 - 중복] 코드를 다시 입력 하십시오.", "[중복검사]표준 공정 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
            //        this.text_ROUT_CD.IsTabStop = true;
            //        this.text_ROUT_CD.Focus();
            //        return false;
            //    }
            //}
            //}
            return true;
        }
        #endregion

        #region Functon (getDomain - ConfigView1Dao)
        private ManVo getDomain()
        {
            ManVo Dao = new ManVo();
            Dao.MDL_CD = this.orgDao.MDL_CD;

            ManVo _rout_cd = this.combo_ROUT_NM.SelectedItem as ManVo;
            if (_rout_cd != null)
            {
                Dao.ROUT_CD = _rout_cd.ROUT_CD;
                Dao.ROUT_NM = _rout_cd.ROUT_NM;
            }

            SystemCodeVo _itm_clss_nm = this.combo_ITM_CLSS_NM.SelectedItem as SystemCodeVo;
            if (_itm_clss_nm != null)
            {
                Dao.ITM_CLSS_CD = _itm_clss_nm.CLSS_CD;
                Dao.ITM_CLSS_NM = _itm_clss_nm.CLSS_DESC;
            }

            SystemCodeVo _sers_colr_nm = this.combo_SERS_COLR_NM.SelectedItem as SystemCodeVo;
            if (_itm_clss_nm  != null)
            {
                Dao.SERS_COLR_CD = _sers_colr_nm.CLSS_CD;
                Dao.SERS_COLR_NM = _sers_colr_nm.CLSS_DESC;
            }

            Dao.COLR_DESC = this.text_COLR_DESC.Text;
            Dao.CMPO_SZ_DESC = this.text_CMPO_SZ_DESC.Text;
            Dao.CMPO_MTRL_DESC = this.text_CMPO_MTRL_DESC.Text;

            SystemCodeVo _rout_mzd_nm = this.combo_ROUT_MZD_NM.SelectedItem as SystemCodeVo;
            if (_rout_mzd_nm != null)
            {
                Dao.ROUT_MZD_CD = _rout_mzd_nm.CLSS_CD;
                Dao.ROUT_MZD_NM = _rout_mzd_nm.CLSS_DESC;
            }

            Dao.CMPO_BRT_DESC = this.text_CMPO_BRT_DESC.Text;

            SystemCodeVo _co_nm = this.combo_CO_NO.SelectedItem as SystemCodeVo;
            if (_co_nm != null)
            {
                Dao.CO_NO = _co_nm.CLSS_CD;
                Dao.CO_NM = _co_nm.CLSS_DESC;
            }

            Dao.BOM_DESC = this.text_BOM_DESC.Text;

            Dao.CMPO_CD = this.text_CMPO_CD.Text;


            Dao.CMPO_SUB_NM = this.text_CMPO_SUB_NM.Text;
            Dao.CMPO_NM = this.text_CMPO_NM.Text;

            Dao.ASSY_SEQ = Convert.ToInt32( this.text_ASSY_SEQ.Text);
            Dao.INP_QTY = this.text_INP_QTY.Text;
            //Dao.SENS_TOL_VAL = this.text_SENS_TOL_VAL.Text;
            ////
            //Dao.DELT_FLG = (this.combo_deltFlg.Text.Equals("사용") ? "N" : "Y");
            Dao.CRE_USR_ID = SystemProperties.USER;
            Dao.UPD_USR_ID = SystemProperties.USER;
            Dao.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;

            ManVo _assy_cd = this.combo_ASSY_NM.SelectedItem as ManVo;
            if (_assy_cd != null)
            {
                Dao.ASSY_CD = _assy_cd.CMPO_CD;
                Dao.ASSY_NM = _assy_cd.CMPO_NM;
            }

            Dao.ROUT_ITM_CD = this.orgDao.ROUT_ITM_CD;

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

        //Vo
        //public ManVo resultDomain
        //{
        //    get
        //    {
        //        return this.updateDao;
        //    }
        //}


        public async void SYSTEM_CODE_VO()
        {
            //품목구분
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "L-021"))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.combo_ITM_CLSS_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                    if (string.IsNullOrEmpty(this.orgDao.ITM_CLSS_CD) == false)
                    {
                        //품목구분
                        this.combo_ITM_CLSS_NM.SelectedItem = (this.combo_ITM_CLSS_NM.ItemsSource as IEnumerable<SystemCodeVo>).Where<SystemCodeVo>(w => w.CLSS_CD.Equals(this.orgDao.ITM_CLSS_CD)).FirstOrDefault<SystemCodeVo>();
                    }
                }
            }

            //색상계열
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "L-022"))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.combo_SERS_COLR_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                    if (string.IsNullOrEmpty(this.orgDao.SERS_COLR_CD) == false)
                    {
                        //품목구분
                        this.combo_SERS_COLR_NM.SelectedItem = (this.combo_SERS_COLR_NM.ItemsSource as IEnumerable<SystemCodeVo>).Where<SystemCodeVo>(w => w.CLSS_CD.Equals(this.orgDao.SERS_COLR_CD)).FirstOrDefault<SystemCodeVo>();
                    }
                }
            }

            //공정방식
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6611", new StringContent(JsonConvert.SerializeObject(new ManVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    //공정에 따라 변경
                    this.combo_ROUT_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
                    if (string.IsNullOrEmpty(this.orgDao.ROUT_CD) == false)
                    {
                        //공정
                        this.combo_ROUT_NM.SelectedItem = (this.combo_ROUT_NM.ItemsSource as IEnumerable<ManVo>).Where<ManVo>(w => w.ROUT_CD.Equals(this.orgDao.ROUT_CD)).FirstOrDefault<ManVo>();

                        //CT (도장)

                        //PR (인쇄)

                        //IN (사출)
                    }
                }
            }

            //매입처
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s143", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { DELT_FLG = "N", SEEK_AP = "AP", SEEK = "AP", CO_TP_CD = "AP", AREA_CD = SystemProperties.USER_VO.EMPE_PLC_NM, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.combo_CO_NO.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                    if (string.IsNullOrEmpty(this.orgDao.CO_NO) == false)
                    {
                        this.combo_CO_NO.SelectedItem = (this.combo_CO_NO.ItemsSource as IEnumerable<SystemCodeVo>).Where<SystemCodeVo>(w => w.CO_NO.Equals(this.orgDao.CO_NO)).FirstOrDefault<SystemCodeVo>();
                    }
                }
            }

            //BOM - Tree
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m66213/tree", new StringContent(JsonConvert.SerializeObject(this.orgDao), System.Text.Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    IList <ManVo> _tmpList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
                    _tmpList.Add(new ManVo() { CMPO_CD = this.orgDao.ROUT_ITM_CD, CMPO_NM = this.orgDao.ITM_NM });

                    this.combo_ASSY_NM.ItemsSource = _tmpList;
                    if (_tmpList.Count > 0)
                    {
                        this.combo_ASSY_NM.SelectedItem = (this.combo_ASSY_NM.ItemsSource as IEnumerable<ManVo>).FirstOrDefault<ManVo>();
                    }
                    else
                    {
                        //
                        if (string.IsNullOrEmpty(this.orgDao.ASSY_CD) == false)
                        {
                            this.combo_ASSY_NM.SelectedItem = (this.combo_ASSY_NM.ItemsSource as IEnumerable<ManVo>).Where<ManVo>(w => w.GBN.Equals(this.orgDao.ASSY_CD)).FirstOrDefault<ManVo>();
                        }
                    }
                }
            }


        }

    }
}
