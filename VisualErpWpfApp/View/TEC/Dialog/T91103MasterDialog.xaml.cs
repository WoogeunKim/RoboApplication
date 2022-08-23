using AquilaErpWpfApp3.Util;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Man;
using ModelsLibrary.Tec;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;

namespace AquilaErpWpfApp3.View.TEC.Dialog
{
    /// <summary>
    /// Interaction logic for ConfigView1Dialog.xaml
    /// </summary>
    public partial class T91103MasterDialog : DXWindow
    {
        //private static CodeServiceClient codeClient = SystemProperties.CodeClient;
        //private SystemCodeVo orgDao;
        private bool isEdit = false;
        public TecVo updateDao;
        private TecVo orgDao;
        private string _title = "공정별 불량코드 관리";

        public T91103MasterDialog(TecVo _vo)
        {
            InitializeComponent();
            //
            //this.orgDao = Dao;
            TecVo copyDao = new TecVo()
            {
                BAD_CD = _vo.BAD_CD,
                BAD_NM = _vo.BAD_NM,
                ROUT_CD = _vo.ROUT_CD,
                ROUT_NM = _vo.ROUT_NM,
                DELT_FLG = _vo.DELT_FLG,
                BAD_RMK = _vo.BAD_RMK,
                CHNL_CD = _vo.CHNL_CD
            };

            SYSTEM_CODE_VO();

            this.orgDao = _vo;

            //수정
            if (copyDao.BAD_CD != null)
            {
                this.text_BAD_CD.IsReadOnly = true;
                this.isEdit = true;
            }
            else
            {
                //추가
                //this.text_ClssTpCd.IsReadOnly = false;
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
                //SystemCodeVo resultVo;
                if (isEdit == false)
                {
                    this.updateDao = getDomain();

                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("t91103/i", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
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

                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("t91103/u", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
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
            if (string.IsNullOrEmpty(this.text_BAD_CD.Text))
            {
                //UOM_CD = new CodeDao() { CLSS_DESC = ITM_NM.UOM_NM };
                //MTRL_MAKE_DT = Convert.ToDateTime(MTRL_MAKE_DT).AddDays(MTRL_EXP_DY).ToString("yyyy-MM-dd");
                WinUIMessageBox.Show("[불량 코드] 입력 값이 맞지 않습니다", "[유효검사] " + this._title, MessageBoxButton.OK, MessageBoxImage.Warning);
                //MSG = "[배치코드] 입력 값이 맞지 않습니다";
                return false;
            }

            if (string.IsNullOrEmpty(this.combo_ROUT_NM.Text))
            {
                //UOM_CD = new CodeDao() { CLSS_DESC = ITM_NM.UOM_NM };
                //MTRL_MAKE_DT = Convert.ToDateTime(MTRL_MAKE_DT).AddDays(MTRL_EXP_DY).ToString("yyyy-MM-dd");
                //WinUIMessageBox.Show("[구분] 입력 값이 맞지 않습니다.", "[유효검사] 시스템 분류 코드", MessageBoxButton.OK, MessageBoxImage.Warning);
                WinUIMessageBox.Show("[공정명] 입력 값이 맞지 않습니다", "[유효검사] " + this._title, MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }


            //if (string.IsNullOrEmpty(MTRL_EXP_DY + ""))
            //{
            //    //UOM_CD = new CodeDao() { CLSS_DESC = ITM_NM.UOM_NM };
            //    //MTRL_MAKE_DT = Convert.ToDateTime(MTRL_MAKE_DT).AddDays(MTRL_EXP_DY).ToString("yyyy-MM-dd");
            //    //WinUIMessageBox.Show("[구분] 입력 값이 맞지 않습니다.", "[유효검사] 시스템 분류 코드", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    MSG = "[유효기간(일수)] 입력 값이 맞지 않습니다";
            //    return false;
            //}
            //else
            //{
            //    MTRL_EXP_DT = Convert.ToDateTime(MTRL_MAKE_DT).AddDays(MTRL_EXP_DY).ToString("yyyy-MM-dd");
            //}



            //if (string.IsNullOrEmpty(INSP_NO))
            //{
            //    //UOM_CD = new CodeDao() { CLSS_DESC = ITM_NM.UOM_NM };
            //    //MTRL_MAKE_DT = Convert.ToDateTime(MTRL_MAKE_DT).AddDays(MTRL_EXP_DY).ToString("yyyy-MM-dd");
            //    //WinUIMessageBox.Show("[구분] 입력 값이 맞지 않습니다.", "[유효검사] 시스템 분류 코드", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    MSG = "[시험 번호] 입력 값이 맞지 않습니다";
            //    return false;
            //}

            //if (string.IsNullOrEmpty(this.text_INSP_DT.Text))
            //{
            //    //UOM_CD = new CodeDao() { CLSS_DESC = ITM_NM.UOM_NM };
            //    //MTRL_MAKE_DT = Convert.ToDateTime(MTRL_MAKE_DT).AddDays(MTRL_EXP_DY).ToString("yyyy-MM-dd");
            //    WinUIMessageBox.Show("[판정 일자] 입력 값이 맞지 않습니다", "[유효검사] " + this._title, MessageBoxButton.OK, MessageBoxImage.Warning);
            //    //MSG = "[판정 일자] 입력 값이 맞지 않습니다";
            //    return false;
            //}



            //if (string.IsNullOrEmpty(this.combo_INSP_FLG.Text))
            //{
            //    //UOM_CD = new CodeDao() { CLSS_DESC = ITM_NM.UOM_NM };
            //    //MTRL_MAKE_DT = Convert.ToDateTime(MTRL_MAKE_DT).AddDays(MTRL_EXP_DY).ToString("yyyy-MM-dd");
            //    WinUIMessageBox.Show("[판정] 입력 값이 맞지 않습니다", "[유효검사] " + this._title, MessageBoxButton.OK, MessageBoxImage.Warning);
            //    //MSG = "[판정] 입력 값이 맞지 않습니다";
            //    return false;
            //}


            return true;
        }
        #endregion


        public async void SYSTEM_CODE_VO()
        {


            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6611", new StringContent(JsonConvert.SerializeObject(new ManVo() { DELT_FLG = "N", CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.combo_ROUT_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
                }
            }

            //this.combo_ITM_GRP_CLSS_CD.ItemsSource = SystemProperties.SYSTEM_CODE_VO("L-001");
            //using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "L-001"))
            //{
            //    if (response.IsSuccessStatusCode)
            //    {
            //        this.combo_ITM_GRP_CLSS_CD.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
            //        this.combo_ITM_GRP_CLSS_CD.SelectedIndex = 0;
            //    }
            //}
            //btn_ITEMS_Click(null, null);
        }

        //#region Functon (getDomain - ConfigView1Dao)
        private TecVo getDomain()
        {
            TecVo Dao = new TecVo();
            Dao.BAD_CD = this.text_BAD_CD.Text;
            Dao.BAD_NM = this.text_BAD_NM.Text;

            ManVo _routNm = this.combo_ROUT_NM.SelectedItem as ManVo;
            if (_routNm != null)
            {
                Dao.ROUT_CD = _routNm.ROUT_CD;
                Dao.ROUT_NM = _routNm.ROUT_NM;
            }
            
            Dao.BAD_RMK = this.text_BAD_RMK.Text;
            Dao.DELT_FLG = (this.combo_DELT_FLG.Text.Equals("사용") ? "N" : "Y");

            Dao.CRE_USR_ID = SystemProperties.USER_VO.USR_ID;
            Dao.UPD_USR_ID = SystemProperties.USER_VO.USR_ID;
            Dao.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;

            return Dao;
        }
        //#endregion

        //Vo
        //public SystemCodeVo resultDomain
        //{
        //    get
        //    {
        //        return this.updateDao;
        //    }
        //}

    }
}
