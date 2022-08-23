using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
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
    /// <summary>
    /// Interaction logic for ConfigView1Dialog.xaml
    /// </summary>
    public partial class M6646MasterDialog : DXWindow
    {
        private string _title = "예측보전계획등록";
        //private static ManServiceClient manClient = SystemProperties.ManClient;
        private ManVo orgDao;
        private bool isEdit = false;
        private ManVo updateDao;

        public M6646MasterDialog(ManVo Dao)
        {
            InitializeComponent();
            //
            SYSTEM_CODE_VO();

            this.orgDao = Dao;
            ManVo copyDao = new ManVo()
            {
                EQ_NO = Dao.EQ_NO,
                EQ_NM = Dao.EQ_NM,
                PLN_MON = Dao.PLN_MON,
                PLN_DY = Dao.PLN_DY,
                PLN_CD = Dao.PLN_CD,
                EQ_DESC = Dao.EQ_DESC,
                CRE_USR_ID = Dao.CRE_USR_ID,
                UPD_USR_ID = Dao.UPD_USR_ID,
                CHNL_CD = Dao.CHNL_CD
            };

            //수정
            if (copyDao.EQ_NO != null)
            {
                this.combo_EQ_NO.IsReadOnly = true;
                this.isEdit = true;
            }
            else
            {
                //추가
                //this.text_ClssTpCd.IsReadOnly = false;
                //this.isEdit = false;
                copyDao.PLN_DY = "0";
                copyDao.PLN_MON = "0";
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
                //ProgramVo resultVo;
                if (isEdit == false)
                {
                    this.updateDao = getDomain();//this.updateDao
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6646/i", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
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

                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6646/u", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
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





                //ManVo resultVo;
                //if (isEdit == false)
                //{
                //    this.updateDao = getDomain();//this.updateDao
                //    resultVo = manClient.M6611InsertMaster(this.updateDao);
                //    if (!resultVo.isSuccess)
                //    {
                //        //실패
                //        WinUIMessageBox.Show(resultVo.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error);
                //        return;
                //    }
                //    //성공
                //    WinUIMessageBox.Show("완료 되었습니다", _title, MessageBoxButton.OK, MessageBoxImage.Information);
                //}
                //else
                //{
                //    this.updateDao = getDomain();
                //    resultVo = manClient.M6611UpdateMaster(this.updateDao);
                //    if (!resultVo.isSuccess)
                //    {
                //        //실패
                //        WinUIMessageBox.Show(resultVo.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error);
                //        return;
                //    }
                //    //성공
                //    WinUIMessageBox.Show("완료 되었습니다", _title, MessageBoxButton.OK, MessageBoxImage.Information);
                //    //
                //    this.orgDao.ROUT_ORD_SEQ = this.updateDao.ROUT_ORD_SEQ;
                //    this.orgDao.ROUT_CD = this.updateDao.ROUT_CD;
                //    this.orgDao.SUB_CD = this.updateDao.SUB_CD;
                //    this.orgDao.ROUT_NM = this.updateDao.ROUT_NM;
                //    this.orgDao.ROUT_MV_CTNT = this.updateDao.ROUT_MV_CTNT;
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
            if (string.IsNullOrEmpty(this.combo_EQ_NO.Text))
            {
                WinUIMessageBox.Show("[설비명] 입력 값이 맞지 않습니다.", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.combo_EQ_NO.IsTabStop = true;
                this.combo_EQ_NO.Focus();
                return false;
            }
            //else if (this.text_ClssTpNm.Text == null || this.text_ClssTpNm.Text.Trim().Length == 0)
            //{
            //    WinUIMessageBox.Show("[코드 명] 입력 값이 맞지 않습니다.", "[유효검사]시스템 분류 코드", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.text_ClssTpNm.IsTabStop = true;
            //    this.text_ClssTpNm.Focus();
            //    return false;
            //}
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
            else
            {
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
            }
            return true;
        }
        #endregion

        #region Functon (getDomain - ConfigView1Dao)
        private ManVo getDomain()
        {
            ManVo Dao = new ManVo();

            ManVo eqNoVo = this.combo_EQ_NO.SelectedItem as ManVo;
            if (eqNoVo != null)
            {
                Dao.EQ_NO = eqNoVo.PROD_EQ_NO;
                Dao.EQ_NM = eqNoVo.EQ_NM;
            }
            //
            Dao.EQ_DESC = this.text_EQ_DESC.Text;

            Dao.PLN_MON = this.orgDao.PLN_MON;
            Dao.TMP_PLN_MON = this.text_PLN_MON.Text;

            Dao.PLN_DY = this.orgDao.PLN_DY;
            Dao.TMP_PLN_DY = this.text_PLN_DY.Text;

            Dao.PLN_CD = "A";
            //
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

        //Vo
        public ManVo resultDomain
        {
            get
            {
                return this.updateDao;
            }
        }


        public async void SYSTEM_CODE_VO()
        {
            ////설비
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6622", new StringContent(JsonConvert.SerializeObject(new ManVo() {CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.combo_EQ_NO.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
                }
            }
        }



    }
}
