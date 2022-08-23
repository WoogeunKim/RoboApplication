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

namespace AquilaErpWpfApp3.View.S.Dialog
{
    /// <summary>
    /// Interaction logic for ConfigView1Dialog.xaml
    /// </summary>
    public partial class S1414MasterDialog : DXWindow
    {
        private string _title = "품목 그룹 관리";
        //private static CodeServiceClient codeClient = SystemProperties.CodeClient;

        //private Dictionary<string, string> _ItemGrpClssCd = new Dictionary<string, string>();
        //private IList<CodeDao> _ItemGrpClssCd = new List<CodeDao>();
        private SystemCodeVo orgDao;
        private bool isEdit = false;
        private SystemCodeVo updateDao;

        public S1414MasterDialog(SystemCodeVo Dao)
        {
            InitializeComponent();
            //
            this.orgDao = Dao;
            SystemCodeVo copyDao = new SystemCodeVo()
            {
                ITM_GRP_CD = Dao.ITM_GRP_CD,
                ITM_GRP_NM = Dao.ITM_GRP_NM,
                ITM_GRP_CLSS_NM = Dao.ITM_GRP_CLSS_NM,
                PRNT_ITM_GRP_CD = Dao.PRNT_ITM_GRP_CD,
                DELT_FLG = Dao.DELT_FLG,
                CHNL_CD = Dao.CHNL_CD,
                CRE_USR_ID = Dao.CRE_USR_ID,
                UPD_USR_ID = Dao.UPD_USR_ID
            };

            //수정
            if (copyDao.ITM_GRP_CD != null)
            {
                this.text_ItmGrpCd.IsReadOnly = true;
                this.isEdit = true;
            }
            else
            {
                //추가
                this.text_ItmGrpCd.IsReadOnly = true;
                //this.isEdit = false;
                copyDao.DELT_FLG = "사용";
            }

            SYSTEM_CODE_VO();
            //this._ItemGrpClssCd = SystemProperties.SYSTEM_CODE_VO("L-001");
            //this.combo_ItmGrpClssNm.ItemsSource = this._ItemGrpClssCd;
            //this.combo_ItmGrpClssNm.ItemsSource = new List<string>(this._ItemGrpClssCd.Keys);

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

                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s133/i/auto", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
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

                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s133/u", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
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
            //if (string.IsNullOrEmpty(this.text_ItmGrpCd.Text))
            //{
            //    WinUIMessageBox.Show("[대분류 코드] 입력 값이 맞지 않습니다.", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.text_ItmGrpCd.IsTabStop = true;
            //    this.text_ItmGrpCd.Focus();
            //    return false;
            //}
            //else
            if (string.IsNullOrEmpty(this.combo_ItmGrpClssNm.Text))
            {
                WinUIMessageBox.Show("[그룹구분] 입력 값이 맞지 않습니다.", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.combo_ItmGrpClssNm.IsTabStop = true;
                this.combo_ItmGrpClssNm.Focus();
                return false;
            }
            else
            {
                //if (this.isEdit == false)
                //{
                //    SystemCodeVo dao = new SystemCodeVo()
                //    {
                //        ITM_GRP_CD = this.text_ItmGrpCd.Text,
                //        PRNT_ITM_GRP_CD = "T",
                //        CRE_USR_ID = "dup"
                //    };
                //    IList<SystemCodeVo> daoList = (IList<SystemCodeVo>)codeClient.SelectCodeItemGroupList(dao);
                //    if (daoList.Count != 0)
                //    {
                //        WinUIMessageBox.Show("[코드 - 중복] 코드를 다시 입력 하십시오.", "[중복검사]품목 그룹 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
                //        this.text_ItmGrpCd.IsTabStop = true;
                //        this.text_ItmGrpCd.Focus();
                //        return false;
                //    }
                //}
            }
            return true;
        }
        #endregion

        #region Functon (getDomain - ConfigView1Dao)
        private SystemCodeVo getDomain()
        {
            SystemCodeVo Dao = new SystemCodeVo();
            Dao.ITM_GRP_CD = this.text_ItmGrpCd.Text;
            Dao.ITM_GRP_NM = this.text_ItmGrpNm.Text;

            SystemCodeVo itmGrpClssVo = this.combo_ItmGrpClssNm.SelectedItem as SystemCodeVo;
            Dao.ITM_GRP_CLSS_NM = itmGrpClssVo.CLSS_DESC;
            Dao.ITM_GRP_CLSS_CD = itmGrpClssVo.CLSS_CD;

            Dao.PRNT_ITM_GRP_CD = "T";

            Dao.CRE_USR_ID = SystemProperties.USER;
            Dao.UPD_USR_ID = SystemProperties.USER;

            Dao.DELT_FLG = (this.combo_deltFlg.Text.Equals("사용") ? "N" : "Y");

            Dao.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;

            return Dao;
        }
        #endregion

        public async void SYSTEM_CODE_VO()
        {
            //this._ItemGrpClssCd = SystemProperties.SYSTEM_CODE_VO("L-001");
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "L-001"))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.combo_ItmGrpClssNm.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
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
