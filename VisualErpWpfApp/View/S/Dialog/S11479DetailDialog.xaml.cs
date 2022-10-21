using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Code;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;
using AquilaErpWpfApp3.Util;
using System.Collections.Generic;
using System.Linq;

namespace AquilaErpWpfApp3.View.S.Dialog
{
    /// <summary>
    /// Interaction logic for ConfigView1Dialog.xaml
    /// </summary>
    public partial class S11479DetailDialog : DXWindow
    {
        //private static CodeServiceClient codeClient = SystemProperties.CodeClient;
        private SystemCodeVo orgDao;
        private bool isEdit = false;
        private SystemCodeVo updateDao;
        private string _title = "공사명 관리";

        public S11479DetailDialog(SystemCodeVo Dao)
        {
            InitializeComponent();

            SYSTEM_CODE_VO();
            
            this.orgDao = Dao;
            SystemCodeVo copyDao = new SystemCodeVo()
            {
                DE_PRNT_NO = Dao.DE_PRNT_NO,
                DE_CHD_NO  = Dao.DE_CHD_NO,
                CO_NO      = Dao.CO_NO,
                CO_NM      = Dao.CO_NM,
                DE_CO_NM   = Dao.DE_CO_NM,
                DUE_DT     = Dao.DUE_DT,
                S_DUE_DT   = Dao.S_DUE_DT,
                CNTR_NM    = Dao.CNTR_NM,
                CNTR_RMK   = Dao.CNTR_RMK,

                CRE_DT     = Dao.CRE_DT,
                S_CRE_DT   = Dao.S_CRE_DT,
                DELT_FLG   = Dao.DELT_FLG,
                CRE_USR_ID = Dao.CRE_USR_ID,
                UPD_USR_ID = Dao.UPD_USR_ID,
                CHNL_CD    = Dao.CHNL_CD
            };

            //수정
            //if (copyDao.DE_PRNT_NO != null)
            if (copyDao.DE_CHD_NO != null)
            {
                //this.combo_DE_PRNT_NO.IsReadOnly = true;
                this.isEdit = true;
            }
            else
            {
                //추가
                //this.text_DE_PRNT_NO.IsReadOnly = false;
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
            try
            {
                if (ValueCheckd())
                {
                    int _Num = 0;
                    //SystemCodeVo resultVo;
                    if (isEdit == false)
                    {
                        this.updateDao = getDomain();

                        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s11479/dtl/i", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
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

                        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s11479/dtl/u", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
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
            catch (Exception eLog)
            {
                //DXSplashScreen.Close();
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
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
            //// 콤보박스로 바뀔예정
            //if (this.text_DE_PRNT_NO.Text == null || this.text_DE_PRNT_NO.Text.Trim().Length == 0)
            //{
            //    WinUIMessageBox.Show("[상위 ID] 입력 값이 맞지 않습니다.", "[유효검사] " + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.text_DE_PRNT_NO.IsTabStop = true;
            //    this.text_DE_PRNT_NO.Focus();
            //    return false;
            //}
            //// 콤보박스로 바뀔예정
            //else if (this.text_CO_NO.Text == null || this.text_CO_NO.Text.Trim().Length == 0)
            //{
            //    WinUIMessageBox.Show("[납품처] 입력 값이 맞지 않습니다.", "[유효검사] " + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.text_CO_NO.IsTabStop = true;
            //    this.text_CO_NO.Focus();
            //    return false;
            //}
            //else if (this.text_DE_CO_NM.Text == null || this.text_DE_CO_NM.Text.Trim().Length == 0)
            //{
            //    WinUIMessageBox.Show("[납품처] 입력 값이 맞지 않습니다.", "[유효검사] " + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.text_DE_CO_NM.IsTabStop = true;
            //    this.text_DE_CO_NM.Focus();
            //    return false;
            //}
            //else if (this.text_DUE_DT.Text == null || this.text_DUE_DT.Text.Trim().Length == 0)
            //{
            //    WinUIMessageBox.Show("[납기일] 입력 값이 맞지 않습니다.", "[유효검사] " + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.text_DUE_DT.IsTabStop = true;
            //    this.text_DUE_DT.Focus();
            //    return false;
            //}
            //else if (this.text_CNTR_NM.Text == null || this.text_CNTR_NM.Text.Trim().Length == 0)
            //{
            //    WinUIMessageBox.Show("[공사명] 입력 값이 맞지 않습니다.", "[유효검사] " + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.text_CNTR_NM.IsTabStop = true;
            //    this.text_CNTR_NM.Focus();
            //    return false;
            //}
            //else if (this.combo_DELT_FLG.Text == null || this.combo_DELT_FLG.Text.Trim().Length == 0)
            //{
            //    WinUIMessageBox.Show("[사용 여부] 입력 값이 맞지 않습니다.", "[유효검사] " + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.combo_DELT_FLG.IsTabStop = true;
            //    this.combo_DELT_FLG.Focus();
            //    return false;
            //}
            //else
            //{
            //    //if (this.isEdit == false)
            //    //{
            //    //    SystemCodeVo dao = new SystemCodeVo()
            //    //    {
            //    //        CLSS_TP_CD = this.text_ClssTpCd.Text,
            //    //    };
            //    //    //IList<SystemCodeVo> daoList = (IList<SystemCodeVo>)codeClient.SelectMasterCode(dao);
            //    //    //if (daoList.Count != 0)
            //    //    //{
            //    //    //    WinUIMessageBox.Show("[코드 - 중복] 코드를 다시 입력 하십시오.", "[중복검사] 시스템 분류 코드", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    //    //    this.text_ClssTpCd.IsTabStop = true;
            //    //    //    this.text_ClssTpCd.Focus();
            //    //    //    return false;
            //    //    //}
            //    //}
            //}
            return true;
        }
        #endregion

        #region Functon (getDomain - ConfigView1Dao)
        private SystemCodeVo getDomain()
        {
            SystemCodeVo Dao = new SystemCodeVo();

            Dao.DE_CHD_NO = this.orgDao.DE_CHD_NO;
            Dao.DE_CO_NM = this.text_DE_CO_NM.Text;
            Dao.CRE_DT = this.text_CRE_DT.Text;
            Dao.DUE_DT = this.text_DUE_DT.Text;
            Dao.CNTR_NM = this.text_CNTR_NM.Text;
            Dao.CNTR_RMK = this.text_CNTR_RMK.Text;

            SystemCodeVo coNo = this.combo_CO_NM.SelectedItem as SystemCodeVo;
            if (coNo != null)
            {
                Dao.CO_NO = coNo.CO_NO;
                Dao.CO_NM = coNo.CO_NM;
            }

            SystemCodeVo dePrntNo = this.combo_DE_PRNT_NO.SelectedItem as SystemCodeVo;
            if(dePrntNo != null)
            {
                Dao.DE_PRNT_NO = dePrntNo.DE_CHD_NO;

            }

            Dao.DELT_FLG = (this.combo_DELT_FLG.Text.Equals("사용") ? "N" : "Y");

            Dao.CRE_USR_ID = SystemProperties.USER;
            Dao.UPD_USR_ID = SystemProperties.USER;
            Dao.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
            return Dao;
        }
        #endregion


        public async void SYSTEM_CODE_VO()
        {

            // 부모만 불러오기
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s11479/mst", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { DELT_FLG = "N", CHNL_CD = SystemProperties.USER_VO.CHNL_CD, DE_PRNT_NO = "0" }), System.Text.Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.combo_DE_PRNT_NO.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                    this.combo_DE_PRNT_NO.SelectedItem = (this.combo_DE_PRNT_NO.ItemsSource as IEnumerable<SystemCodeVo>).Where(x => Convert.ToInt32(x.DE_CHD_NO) == Convert.ToInt32(this.orgDao.DE_PRNT_NO)).FirstOrDefault<SystemCodeVo>();
                }
            }

            // 거래처
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s143", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { DELT_FLG = "N", CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.combo_CO_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
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
