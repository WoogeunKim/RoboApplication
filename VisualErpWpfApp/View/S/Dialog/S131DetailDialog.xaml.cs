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
    /// Interaction logic for ConfigView1Dialog.xaml
    /// </summary>
    public partial class S131DetailDialog : DXWindow
    {
        //private static CodeServiceClient codeClient = SystemProperties.CodeClient;
        private SystemCodeVo orgDao;
        private bool isEdit = false;
        private SystemCodeVo updateDao;
        private string _title = "시스템 분류 코드";

        private string clssTpCd = "";

        public S131DetailDialog(SystemCodeVo Dao)
        {
            InitializeComponent();
            //
            this.orgDao = Dao;
            SystemCodeVo copyDao = new SystemCodeVo()
            {
                USE_ST_DT = Dao.USE_ST_DT,
                USE_END_DT = Dao.USE_END_DT,
                CLSS_TP_CD = Dao.CLSS_TP_CD,
                //CLSS_TP_NM = Dao.CLSS_TP_NM,
                CLSS_CD = Dao.CLSS_CD,
                CLSS_DESC = Dao.CLSS_DESC,
                CLSS_ORD_SEQ = Dao.CLSS_ORD_SEQ,
                //SYS_FLG = Dao.SYS_FLG,
                //SYS_AREA_CD = Dao.SYS_AREA_CD,
                //CLSS_CD_DESC = Dao.CLSS_CD_DESC,
                DELT_FLG = Dao.DELT_FLG,
                CHNL_CD = Dao.CHNL_CD,
                CRE_USR_ID = Dao.CRE_USR_ID,
                UPD_USR_ID = Dao.UPD_USR_ID
            };

            this.clssTpCd = Dao.CLSS_TP_CD;
            if (Dao.CLSS_CD != null)
            {
                this.isEdit = true;
                this.text_ClssCd.IsReadOnly = true;
                this.text_useStDt.IsReadOnly = true;
                this.text_useEndDt.IsReadOnly = false;
            }
            else
            {
                this.isEdit = false;
                copyDao.USE_ST_DT = System.DateTime.Now.ToString("yyyy-MM-dd");
                copyDao.USE_END_DT = "2999-12-31";
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
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s131/dtl/i", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
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
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s131/dtl/u", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
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
            if (this.text_ClssCd.Text == null || this.text_ClssCd.Text.Trim().Length == 0)
            {
                WinUIMessageBox.Show("[코드] 입력 값이 맞지 않습니다.", "[유효검사] " + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.text_ClssCd.IsTabStop = true;
                this.text_ClssCd.Focus();
                return false;
            }
            //else if (this.text_ClssTpNm.Text == null || this.text_ClssTpNm.Text.Trim().Length == 0)
            //{
            //    WinUIMessageBox.Show("[코드 명] 입력 값이 맞지 않습니다.", "[유효검사] 시스템 분류 코드", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.text_ClssTpNm.IsTabStop = true;
            //    this.text_ClssTpNm.Focus();
            //    return false;
            //}
            else if (this.text_useStDt.Text == null || this.text_useStDt.Text.Trim().Length == 0)
            {
                WinUIMessageBox.Show("[사용 시작] 입력 값이 맞지 않습니다.", "[유효검사] " + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.text_useStDt.IsTabStop = true;
                this.text_useStDt.Focus();
                return false;
            }
            else if (this.text_useEndDt.Text == null || this.text_useEndDt.Text.Trim().Length == 0)
            {
                WinUIMessageBox.Show("[사용 종료] 입력 값이 맞지 않습니다.", "[유효검사] " + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.text_useEndDt.IsTabStop = true;
                this.text_useEndDt.Focus();
                return false;
            }
            else if (this.text_clssOrdSeq.Text == null || this.text_clssOrdSeq.Text.Trim().Length == 0)
            {
                WinUIMessageBox.Show("[정렬 순서] 입력 값이 맞지 않습니다.", "[유효검사] " + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.text_clssOrdSeq.IsTabStop = true;
                this.text_clssOrdSeq.Focus();
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
                //        CLSS_TP_CD = this.clssTpCd
                //        ,
                //        CLSS_CD = this.text_ClssCd.Text
                //    };
                //    //ObservableCollection<SystemCodeVo> daoList = service.SearchDetailConfigView1(dao);
                //    //IList<SystemCodeVo> daoList = (IList<SystemCodeVo>)codeClient.SelectDetailCode(dao);
                //    //if (daoList.Count != 0)
                //    //{
                //    //    WinUIMessageBox.Show("[코드 - 중복] 코드를 다시 입력 하십시오.", "[중복검사] " + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                //    //    this.text_ClssCd.IsTabStop = true;
                //    //    this.text_ClssCd.Focus();
                //    //    return false;
                //    //}
                //}
            }
            return true;
        }
        #endregion

        #region Functon (getDomain - ConfigView1Dao)
        public SystemCodeVo getDomain()
        {
            SystemCodeVo Dao = new SystemCodeVo();
            Dao.CLSS_TP_CD = this.clssTpCd;
            Dao.CLSS_CD = this.text_ClssCd.Text;
            Dao.CLSS_DESC = this.text_ClssCdDesc.Text;
            Dao.USE_ST_DT = Convert.ToDateTime(this.text_useStDt.Text);
            Dao.USE_END_DT = Convert.ToDateTime(this.text_useEndDt.Text).ToString("yyyy-MM-dd");
            Dao.CLSS_ORD_SEQ = int.Parse((this.text_clssOrdSeq.Text == null ? "0" : this.text_clssOrdSeq.Text));
            Dao.DELT_FLG = (this.combo_deltFlg.Text.Equals("사용") ? "N" : "Y");
            //
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


        public SystemCodeVo resultDomain
        {
            get
            {
                return this.updateDao;
            }
        }
    }
}
