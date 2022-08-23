using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Code;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;
using AquilaErpWpfApp3.Util;

namespace AquilaErpWpfApp3.View.S.Dialog
{
    /// <summary>
    /// Interaction logic for ConfigView1Dialog.xaml
    /// </summary>
    public partial class S131MasterDialog : DXWindow
    {
        //private static CodeServiceClient codeClient = SystemProperties.CodeClient;
        private SystemCodeVo orgDao;
        private bool isEdit = false;
        private SystemCodeVo updateDao;
        private string _title = "시스템 분류 코드";

        public S131MasterDialog(SystemCodeVo Dao)
        {
            InitializeComponent();
            //
            this.orgDao = Dao;
            SystemCodeVo copyDao = new SystemCodeVo()
            {
                CLSS_TP_CD = Dao.CLSS_TP_CD,
                CLSS_TP_NM = Dao.CLSS_TP_NM,
                SYS_FLG = Dao.SYS_FLG,
                SYS_AREA_CD = Dao.SYS_AREA_CD,
                CLSS_CD_DESC = Dao.CLSS_CD_DESC,
                DELT_FLG = Dao.DELT_FLG,
                CRE_USR_ID = Dao.CRE_USR_ID,
                UPD_USR_ID = Dao.UPD_USR_ID,
                CHNL_CD = Dao.CHNL_CD
            };

            //수정
            if (copyDao.CLSS_TP_CD != null)
            {
                this.text_ClssTpCd.IsReadOnly = true;
                this.isEdit = true;
            }
            else
            {
                //추가
                //this.text_ClssTpCd.IsReadOnly = false;
                //this.isEdit = false;
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

                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s131/mst/i", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
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

                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s131/mst/u", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
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
            if (this.text_ClssTpCd.Text == null || this.text_ClssTpCd.Text.Trim().Length == 0)
            {
                WinUIMessageBox.Show("[분류 코드] 입력 값이 맞지 않습니다.", "[유효검사] " + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.text_ClssTpCd.IsTabStop = true;
                this.text_ClssTpCd.Focus();
                return false;
            }
            else if (this.text_ClssTpNm.Text == null || this.text_ClssTpNm.Text.Trim().Length == 0)
            {
                WinUIMessageBox.Show("[코드 명] 입력 값이 맞지 않습니다.", "[유효검사] " + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.text_ClssTpNm.IsTabStop = true;
                this.text_ClssTpNm.Focus();
                return false;
            }
            else if (this.text_SysFlg.Text == null || this.text_SysFlg.Text.Trim().Length == 0)
            {
                WinUIMessageBox.Show("[구분] 입력 값이 맞지 않습니다.", "[유효검사] " + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.text_SysFlg.IsTabStop = true;
                this.text_SysFlg.Focus();
                return false;
            }
            else if (this.text_SysAreaCd.Text == null || this.text_SysAreaCd.Text.Trim().Length == 0)
            {
                WinUIMessageBox.Show("[업무 분야] 입력 값이 맞지 않습니다.", "[유효검사] " + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.text_SysAreaCd.IsTabStop = true;
                this.text_SysAreaCd.Focus();
                return false;
            }
            else if (this.combo_deltFlg.Text == null || this.combo_deltFlg.Text.Trim().Length == 0)
            {
                WinUIMessageBox.Show("[사용 여부] 입력 값이 맞지 않습니다.", "[유효검사] " + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.combo_deltFlg.IsTabStop = true;
                this.combo_deltFlg.Focus();
                return false;
            }
            else
            {
                //if (this.isEdit == false)
                //{
                //    SystemCodeVo dao = new SystemCodeVo()
                //    {
                //        CLSS_TP_CD = this.text_ClssTpCd.Text,
                //    };
                //    //IList<SystemCodeVo> daoList = (IList<SystemCodeVo>)codeClient.SelectMasterCode(dao);
                //    //if (daoList.Count != 0)
                //    //{
                //    //    WinUIMessageBox.Show("[코드 - 중복] 코드를 다시 입력 하십시오.", "[중복검사] 시스템 분류 코드", MessageBoxButton.OK, MessageBoxImage.Warning);
                //    //    this.text_ClssTpCd.IsTabStop = true;
                //    //    this.text_ClssTpCd.Focus();
                //    //    return false;
                //    //}
                //}
            }
            return true;
        }
        #endregion

        #region Functon (getDomain - ConfigView1Dao)
        private SystemCodeVo getDomain()
        {
            SystemCodeVo Dao = new SystemCodeVo();
            Dao.CLSS_TP_CD = this.text_ClssTpCd.Text;
            Dao.CLSS_TP_NM = this.text_ClssTpNm.Text;
            Dao.SYS_FLG = this.text_SysFlg.Text;
            Dao.SYS_AREA_CD = this.text_SysAreaCd.Text;
            Dao.CLSS_CD_DESC = this.text_ClssCdDesc.Text;
            Dao.DELT_FLG = (this.combo_deltFlg.Text.Equals("사용") ? "N" : "Y");
            Dao.USR_ID = SystemProperties.USER;
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
        public SystemCodeVo resultDomain
        {
            get
            {
                return this.updateDao;
            }
        }

    }
}
