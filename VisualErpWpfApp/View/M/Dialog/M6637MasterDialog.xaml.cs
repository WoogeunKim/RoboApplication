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
using AquilaErpWpfApp3.Util;

namespace AquilaErpWpfApp3.View.M.Dialog
{
    public partial class M6637MasterDialog : DXWindow
    {

        private string _title = "칭량 작업(추가/폐기)";

        //private static ManServiceClient manClient = SystemProperties.ManClient;
        private ManVo orgDao;
        private bool isEdit = false;
        private ManVo updateDao;

        public M6637MasterDialog(ManVo Dao)
        {
            InitializeComponent();

            
            //
            //
            this.orgDao = Dao;
            ManVo copyDao = new ManVo()
            {
                LOT_NO = Dao.LOT_NO,
                ASSY_ITM_SEQ = Dao.ASSY_ITM_SEQ,
                WEIH_SEQ = Dao.WEIH_SEQ,
                ROUT_CD = Dao.ROUT_CD,
                ROUT_NM = Dao.ROUT_NM,
                MTRL_LOT_NO = Dao.MTRL_LOT_NO,
                WEIH_VAL = Dao.WEIH_VAL,
                CHNL_CD = Dao.CHNL_CD,
                CRE_USR_ID = Dao.CRE_USR_ID,
                UPD_USR_ID = Dao.UPD_USR_ID,
                GBN = Dao.GBN,
            };


            SYSTEM_CODE_VO();

            //수정
            //if (copyDao.PCK_PLST_CLSS_CD != null)
            //{
            //    //this.combo_PCK_PLST_CLSS_CD.IsReadOnly = true;
            //    //this.text_PCK_PLST_CD.IsReadOnly = true;
            //    //this.isEdit = true;
            //    //
            //    //copyDao.EQ_PUR_YRMON = DateTime.ParseExact(Dao.EQ_PUR_YRMON + "01", "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None).ToString("yyyy-MM-dd");
            //}
            //else
            //{
            //    //추가
            //    //this.text_ClssTpCd.IsReadOnly = false;
            //    //this.isEdit = false;
            //    //copyDao.EQ_PUR_YRMON = System.DateTime.Now.ToString("yyyy-MM-dd");
            //    copyDao.PCK_PLST_VAL = 0;
            //    //copyDao.EQ_PUR_AMT = 0;
            //}

            this.configCode.DataContext = copyDao;
            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
            this.OKButton.Click += new RoutedEventHandler(OKButton_Click);
            this.CancelButton.Click += new RoutedEventHandler(CancelButton_Click);

            this.RefButton.Click += new RoutedEventHandler(RefButton_Click);


            this.text_LOT_NO.KeyDown += Text_LOT_NO_KeyDown;
            this.text_LOT_NO.Focus();
        }

        private async void RefButton_Click(object sender, RoutedEventArgs e)
        {
            using (HttpResponseMessage responseX = await SystemProperties.PROGRAM_HTTP.PostAsync("m6631/dtl", new StringContent(JsonConvert.SerializeObject(new ManVo() { LOT_NO = this.text_LOT_NO.Text, ASSY_ITM_CD = string.Empty, INP_LOT_NO = string.Empty, SL_ORD_NO = string.Empty, SL_ORD_SEQ = 0, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
            {
                if (responseX.IsSuccessStatusCode)
                {
                    this.combo_MTRL_LOT_NO.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await responseX.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
                }
            }
        }

        private async void Text_LOT_NO_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                RefButton_Click(sender, null);
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
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6637/mst/i", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
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
                //else
                //{
                //    this.updateDao = getDomain();

                //    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6625/u", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
                //    {
                //        if (response.IsSuccessStatusCode)
                //        {
                //            string result = await response.Content.ReadAsStringAsync();
                //            if (int.TryParse(result, out _Num) == false)
                //            {
                //                //실패
                //                WinUIMessageBox.Show(result, _title, MessageBoxButton.OK, MessageBoxImage.Error);
                //                return;
                //            }

                //            //성공
                //            WinUIMessageBox.Show("완료 되었습니다", _title, MessageBoxButton.OK, MessageBoxImage.Information);
                //        }
                //    }
                //}

                //ManVo resultVo;
                //if (isEdit == false)
                //{
                //    this.updateDao = getDomain();//this.updateDao
                //    resultVo = manClient.M6625InsertMaster(this.updateDao);
                //    if (!resultVo.isSuccess)
                //    {
                //        //실패
                //        WinUIMessageBox.Show(resultVo.Message, "[" + SystemProperties.PROGRAM_TITLE + "]포장 코드 관리", MessageBoxButton.OK, MessageBoxImage.Error);
                //        return;
                //    }
                //    //성공
                //    WinUIMessageBox.Show("완료 되었습니다", "[추가]포장 코드 관리", MessageBoxButton.OK, MessageBoxImage.Information);
                //}
                //else
                //{
                //    this.updateDao = getDomain();
                //    resultVo = manClient.M6625UpdateMaster(this.updateDao);
                //    if (!resultVo.isSuccess)
                //    {
                //        //실패
                //        WinUIMessageBox.Show(resultVo.Message, "[" + SystemProperties.PROGRAM_TITLE + "]포장 코드 관리", MessageBoxButton.OK, MessageBoxImage.Error);
                //        return;
                //    }
                //    //성공
                //    WinUIMessageBox.Show("완료 되었습니다", "[수정]포장 코드 관리", MessageBoxButton.OK, MessageBoxImage.Information);
                //    //
                //    this.orgDao.PCK_PLST_CLSS_CD = this.updateDao.PCK_PLST_CLSS_CD;
                //    this.orgDao.PCK_PLST_CLSS_NM = this.updateDao.PCK_PLST_CLSS_NM;
                //    this.orgDao.PCK_PLST_CD = this.updateDao.PCK_PLST_CD;
                //    this.orgDao.PCK_PLST_NM = this.updateDao.PCK_PLST_NM;
                //    this.orgDao.PCK_PLST_VAL = this.updateDao.PCK_PLST_VAL;
                //    this.orgDao.CRE_DT = this.updateDao.CRE_DT;
                //    this.orgDao.CRE_USR_ID = this.updateDao.CRE_USR_ID;
                //    this.orgDao.UPD_USR_ID = this.updateDao.UPD_USR_ID;
                //    this.orgDao.CHNL_CD = this.updateDao.CHNL_CD;
                //}
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
            if (string.IsNullOrEmpty(this.text_LOT_NO.Text))
            {
                WinUIMessageBox.Show("[칭량 지시 번호] 입력 값이 맞지 않습니다.", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.text_LOT_NO.IsTabStop = true;
                this.text_LOT_NO.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(this.combo_MTRL_LOT_NO.Text))
            {
                WinUIMessageBox.Show("[(원료) 투입 순번] 입력 값이 맞지 않습니다.", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.combo_MTRL_LOT_NO.IsTabStop = true;
                this.combo_MTRL_LOT_NO.Focus();
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
            {
                if (this.isEdit == false)
                {
                    //CodeDao PCK_PLST_CLSS_CD = this.combo_PCK_PLST_CLSS_CD.SelectedItem as CodeDao;

                    //ManVo dao = new ManVo()
                    //{
                    //    PCK_PLST_CLSS_CD = PCK_PLST_CLSS_CD.CLSS_CD,
                    //    PCK_PLST_CD = this.text_PCK_PLST_CD.Text,
                    //    CHNL_CD = SystemProperties.USER_VO.CHNL_CD
                    //};
                    //IList<ManVo> daoList = (IList<ManVo>)manClient.M6625SelectMaster(dao);
                    //if (daoList.Count != 0)
                    //{
                    //    WinUIMessageBox.Show("[코드 - 중복] 코드를 다시 입력 하십시오.", "[중복검사]포장 코드 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
                    //    this.text_PCK_PLST_CD.IsTabStop = true;
                    //    this.text_PCK_PLST_CD.Focus();
                    //    return false;
                    //}
                }
            }
            return true;
        }
        #endregion

        #region Functon (getDomain - ConfigView1Dao)
        private ManVo getDomain()
        {
            ManVo Dao = new ManVo();
            Dao.ROUT_CD = this.orgDao.ROUT_CD;
            Dao.ROUT_NM = this.orgDao.ROUT_NM;
            //Dao.ASSY_ITM_SEQ = this.orgDao.ASSY_ITM_SEQ;

            Dao.LOT_NO = this.text_LOT_NO.Text;

            ManVo _mtrl_lot_no = this.combo_MTRL_LOT_NO.SelectedItem as ManVo;
            //Dao.MTRL_LOT_NO = this.text_MTRL_LOT_NO.Text;
            Dao.MTRL_LOT_NO = _mtrl_lot_no.MTRL_LOT_NO;
            Dao.ASSY_ITM_SEQ = _mtrl_lot_no.ASSY_ITM_SEQ;


            Dao.WEIH_VAL = this.text_WEIH_VAL.Text;

            //
            Dao.CRE_USR_ID = SystemProperties.USER;
            Dao.UPD_USR_ID = SystemProperties.USER;
            Dao.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;

            return Dao;
        }
        #endregion


        public async void SYSTEM_CODE_VO()
        {
            //this.combo_PCK_PLST_CLSS_CD.ItemsSource = SystemProperties.SYSTEM_CODE_VO("M-009");
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "W-001"))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.combo_ROUT_CD.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                   // this.combo_ROUT_CD.Text = this.orgDao.ROUT_NM;

                }
            }

            //
            using (HttpResponseMessage responseX = await SystemProperties.PROGRAM_HTTP.PostAsync("m6631/dtl", new StringContent(JsonConvert.SerializeObject(new ManVo() { LOT_NO = this.text_LOT_NO.Text, ASSY_ITM_CD = string.Empty, INP_LOT_NO = string.Empty, SL_ORD_NO = string.Empty, SL_ORD_SEQ = 0, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
            {
                if (responseX.IsSuccessStatusCode)
                {
                    this.combo_MTRL_LOT_NO.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await responseX.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
                }
            }

        }


        //IsEdit
        //public bool IsEdit
        //{
        //    get
        //    {
        //        return this.isEdit;
        //    }
        //}

        //Vo
        //public ManVo resultDomain
        //{
        //    get
        //    {
        //        return this.updateDao;
        //    }
        //}

    }
}
