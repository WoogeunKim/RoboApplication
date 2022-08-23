using AquilaErpWpfApp3.Util;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Auth;
using ModelsLibrary.Man;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace AquilaErpWpfApp3.View.M.Dialog
{
    /// <summary>
    /// Interaction logic for ConfigView1Dialog.xaml
    /// </summary>
    public partial class M6632WeighingUserDialog : DXWindow
    {
        private KeyPadDialog keyPadDialog;

        private bool isEdit = false;
        public ManVo updateDao;


        public M6632WeighingUserDialog(ManVo _manVo)
        {
            InitializeComponent();


            this.updateDao = _manVo;

            this.configCode.DataContext = _manVo;
            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);

            this.btn_id.Click += Btn_id_Click;
            this.btn_pass.Click += Btn_pass_Click;


            this.btn_login.Click += new RoutedEventHandler(OKButton_Click);
            //this.CancelButton.Click += new RoutedEventHandler(CancelButton_Click);
        }

        private void Btn_id_Click(object sender, RoutedEventArgs e)
        {
            keyPadDialog = new KeyPadDialog((string.IsNullOrEmpty(this.txtId.Text) ? "" : this.txtId.Text), true, false, 70);
            //keyPadDialog.SetLeft()
            //keyPadDialog.Owner = Application.Current.MainWindow;
            keyPadDialog.BorderEffect = BorderEffect.Default;
            keyPadDialog.BorderEffectActiveColor = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 128, 0));
            keyPadDialog.BorderEffectInactiveColor = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 170, 170));
            bool isDialog = (bool)keyPadDialog.ShowDialog();
            if (!isDialog)
            {
                this.txtId.Text = keyPadDialog.editContent;
            }
            else
            {
                this.txtId.Text = keyPadDialog.orgContent;
            }
        }

        private void Btn_pass_Click(object sender, RoutedEventArgs e)
        {
            keyPadDialog = new KeyPadDialog((string.IsNullOrEmpty(this.txtPass.Text) ? "" : this.txtPass.Text), true, false, 70);
            //keyPadDialog.SetLeft()
            //keyPadDialog.Owner = Application.Current.MainWindow;
            keyPadDialog.BorderEffect = BorderEffect.Default;
            keyPadDialog.BorderEffectActiveColor = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 128, 0));
            keyPadDialog.BorderEffectInactiveColor = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 170, 170));
            bool isDialog = (bool)keyPadDialog.ShowDialog();
            if (!isDialog)
            {
                this.txtPass.Text = keyPadDialog.editContent;
            }
            else
            {
                this.txtPass.Text = keyPadDialog.orgContent;
            }
        }

        #region Functon (OKButton_Click, CancelButton_Click)
        private async void OKButton_Click(object sender, RoutedEventArgs e)
        {

            if (ValueCheckd())
            {
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s136/login/" + Properties.Settings.Default.SettingChnl + "/" + this.txtId.Text))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        System.Security.Cryptography.SHA256Managed sha256Managed = new System.Security.Cryptography.SHA256Managed();
                        GroupUserVo usrVo = JsonConvert.DeserializeObject<GroupUserVo>(await response.Content.ReadAsStringAsync());
                        if (usrVo == null)
                        {
                            this.btn_login.IsEnabled = true;
                            this.txtId.IsEnabled = true;
                            this.txtPass.IsEnabled = true;

                            WinUIMessageBox.Show("아이디가 존재 하지 않습니다.", this.Title, MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }
                        //암호화 비교 Convert.ToBase64String(sha256Managed.ComputeHash(System.Text.Encoding.UTF8.GetBytes(   )))
                        if (Convert.ToBase64String(sha256Managed.ComputeHash(System.Text.Encoding.UTF8.GetBytes(this.txtPass.Password))).Equals(usrVo.USR_PWD))
                        {
                            //
                            MessageBoxResult result = WinUIMessageBox.Show("[" + usrVo.USR_N1ST_NM + " / " + usrVo.USR_ID + "] 칭량 하시겠습니까?", this.Title, MessageBoxButton.YesNo, MessageBoxImage.Question);
                            if (result == MessageBoxResult.Yes)
                            {
                                //
                                //
                                int _Num = 0;
                                ////
                                this.updateDao.UPD_USR_ID = SystemProperties.USER;
                                this.updateDao.WRK_MAN_ID = this.txtId.Text;
                                this.updateDao.WRK_END_FLG = (this.updateDao.WRK_END_FLG.Equals("예") ? "Y" : "N");

                                using (HttpResponseMessage response_X = await SystemProperties.PROGRAM_HTTP.PostAsync("m6631/mst/u", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
                                {
                                    if (response.IsSuccessStatusCode)
                                    {
                                        //string resultMsg = await response.Content.ReadAsStringAsync();
                                        //if (int.TryParse(resultMsg, out _Num) == false)
                                        //{
                                        //실패
                                        //WinUIMessageBox.Show(resultMsg, this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                                        //return;
                                        //}

                                        this.DialogResult = true;
                                        this.Close();
                                    }
                                }

                            }

                        }
                    }
                }
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
            if (string.IsNullOrEmpty(this.txtId.Text))
            {
                WinUIMessageBox.Show("[ID] 입력 값이 맞지 않습니다.", "[유효검사] " + this.Title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.txtId.IsTabStop = true;
                this.txtId.Focus();
                return false;
            }
            else if (Convert.ToInt32(this.txtPass.Text) <= 0)
            {
                WinUIMessageBox.Show("[Password] 입력 값이 맞지 않습니다.", "[유효검사] " + this.Title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.txtPass.IsTabStop = true;
                this.txtPass.Focus();
                return false;
            }
            //else if (Convert.ToInt32(this.text_MIX_WEIH_VAL.Text) <= 0)
            //{
            //    WinUIMessageBox.Show("[Batch 중량(Kg)] 입력 값이 맞지 않습니다.", "[유효검사] 칭량 작업 계획", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.text_MIX_WEIH_VAL.IsTabStop = true;
            //    this.text_MIX_WEIH_VAL.Focus();
            //    return false;
            //}
            ////else if (string.IsNullOrEmpty(this.combo_MIX_NM.Text))
            ////{
            ////    WinUIMessageBox.Show("[내리 구분] 입력 값이 맞지 않습니다.", "[유효검사] 칭량 작업 계획 / 지시", MessageBoxButton.OK, MessageBoxImage.Warning);
            ////    this.combo_MIX_NM.IsTabStop = true;
            ////    this.combo_MIX_NM.Focus();
            ////    return false;
            ////}
            //else if (string.IsNullOrEmpty(this.text_WRK_DT.Text))
            //{
            //    WinUIMessageBox.Show("[작업 일자] 입력 값이 맞지 않습니다.", "[유효검사] 칭량 작업 계획", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.text_WRK_DT.IsTabStop = true;
            //    this.text_WRK_DT.Focus();
            //    return false;
            //}
            //else if (string.IsNullOrEmpty(this.combo_CRE_USR_NM.Text))
            //{
            //    WinUIMessageBox.Show("[지시자] 입력 값이 맞지 않습니다.", "[유효검사] 칭량 작업 계획", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.combo_CRE_USR_NM.IsTabStop = true;
            //    this.combo_CRE_USR_NM.Focus();
            //    return false;
            //}
            //else if (string.IsNullOrEmpty(this.text_INP_LOT_NO.Text))
            //{
            //    WinUIMessageBox.Show("[LOT-NO] 입력 값이 맞지 않습니다.", "[유효검사] 칭량 작업 계획", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.text_INP_LOT_NO.IsTabStop = true;
            //    this.text_INP_LOT_NO.Focus();
            //    return false;
            //}
            ////else
            ////{
            ////    if (this.isEdit == false)
            ////    {
            ////        ManVo dao = new ManVo()
            ////        {
            ////            ASSY_ITM_CD = this.orgDao.ASSY_ITM_CD,
            ////            BSE_WEIH_VAL = int.Parse(this.text_BSE_WEIH_VAL.Text),
            ////            ASSY_ITM_SEQ = int.Parse(this.text_ASSY_ITM_SEQ.Text)

            ////        };
            ////        IList<ManVo> daoList = (IList<ManVo>)manClient.SelectProdWeihTbl(dao);
            ////        if (daoList.Count != 0)
            ////        {
            ////            WinUIMessageBox.Show("[배합표 No. - 중복] 코드를 다시 입력 하십시오.", "[중복검사] 배합표", MessageBoxButton.OK, MessageBoxImage.Warning);
            ////            this.text_BSE_WEIH_VAL.IsTabStop = true;
            ////            this.text_BSE_WEIH_VAL.Focus();
            ////            return false;
            ////        }
            ////    }
            ////}
            return true;
        }
        #endregion

        #region Functon (getDomain - ConfigView1Dao)
        private ManVo getDomain()
        {
            ManVo Dao = new ManVo();
            //Dao.LOT_NO = this.text_LOT_NO.Text;

            //ManVo assyItmCd = this.combo_ASSY_ITM_CD.SelectedItem as ManVo;
            //if (assyItmCd != null)
            //{
            //    Dao.ASSY_ITM_CD = assyItmCd.ASSY_ITM_CD;
            //    Dao.ASSY_ITM_NM = assyItmCd.ITM_NM;
            //    Dao.BSE_WEIH_VAL = assyItmCd.BSE_WEIH_VAL;
            //}

            //ManVo slOrdNo = this.combo_SL_ORD_NO.SelectedItem as ManVo;
            //if (slOrdNo != null)
            //{
            //    Dao.SL_ORD_NO = slOrdNo.PROD_PLN_NO;
            //    Dao.SL_ORD_SEQ = 1;
            //    //Dao.SL_ORD_SEQ = slOrdNo.SL_ORD_SEQ;
            //}

            //Dao.MIX_WEIH_VAL = int.Parse(this.text_MIX_WEIH_VAL.Text);
            //Dao.TOT_WEIH_VAL = int.Parse(this.text_TOT_WEIH_VAL.Text);

            ////Dao.MIX_CD = (this.combo_MIX_NM.Text.Equals("신내리") ? "A" : "B");
            ////Dao.MIX_NM = this.combo_MIX_NM.Text;
            //Dao.MIX_CD = "A";
            //Dao.WRK_DT = Convert.ToDateTime(this.text_WRK_DT.Text).ToString("yyyy-MM-dd");

            //GroupUserVo WrkManVo = this.combo_WRK_MAN_NM.SelectedItem as GroupUserVo;
            //if (WrkManVo != null)
            //{
            //    Dao.WRK_MAN_ID = WrkManVo.USR_ID;
            //    Dao.WRK_MAN_NM = WrkManVo.USR_N1ST_NM;
            //}

            //Dao.WRK_END_FLG = (this.combo_WRK_END_FLG.Text.Equals("예") ? "Y" : "N");

            //Dao.WRK_DESC = this.text_WRK_DESC.Text;

            //Dao.INP_LOT_NO = this.text_INP_LOT_NO.Text;

            //Dao.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;

            //GroupUserVo CreUsrVo = this.combo_CRE_USR_NM.SelectedItem as GroupUserVo;
            //if (CreUsrVo != null)
            //{
            //    Dao.CRE_USR_ID = CreUsrVo.USR_ID;
            //    Dao.CRE_USR_NM = CreUsrVo.USR_N1ST_NM;
            //}

            ////Dao.CRE_USR_ID = SystemProperties.USER;
            //Dao.UPD_USR_ID = SystemProperties.USER;
            return Dao;
        }
        #endregion


        //public async void SYSTEM_CODE_VO()
        //{
        //    //try
        //    //{
        //    //    DXSplashScreen.Show<ProgressWindow>();

        //    //    if (isEdit == false)
        //    //    {
        //    //        //계획 번호
        //    //        //
        //    //        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6630/mst", new StringContent(JsonConvert.SerializeObject(new ManVo() { FM_DT = System.DateTime.Now.AddMonths(-1).ToString("yyyyMMdd"), TO_DT = System.DateTime.Now.ToString("yyyyMMdd"), DELT_FLG = "N", UPD_USR_ID = SystemProperties.USER, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
        //    //        {
        //    //            if (response.IsSuccessStatusCode)
        //    //            {
        //    //                this.combo_SL_ORD_NO.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
        //    //                //this.combo_SL_ORD_NO.SelectedItem = (this.combo_SL_ORD_NO.ItemsSource as List<ManVo>).Where(x => x.PROD_PLN_NO.Equals(this.orgDao.PROD_PLN_NO)).FirstOrDefault<ManVo>();
        //    //            }
        //    //        }
        //    //    }
        //    //    else
        //    //    {
        //    //        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6630/mst", new StringContent(JsonConvert.SerializeObject(new ManVo() { PROD_PLN_NO = this.orgDao.SL_ORD_NO, UPD_USR_ID = SystemProperties.USER, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
        //    //        {
        //    //            if (response.IsSuccessStatusCode)
        //    //            {
        //    //                this.combo_SL_ORD_NO.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
        //    //                this.combo_SL_ORD_NO.SelectedItem = (this.combo_SL_ORD_NO.ItemsSource as List<ManVo>).Where(x => x.PROD_PLN_NO.Equals(this.orgDao.SL_ORD_NO)).FirstOrDefault<ManVo>();
        //    //            }
        //    //        }

        //    //    }

        //    //    //this.combo_ITM_GRP_CLSS_CD_1.ItemsSource = SystemProperties.SYSTEM_CODE_VO("L-001");
        //    //    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "L-001"))
        //    //    {
        //    //        if (response.IsSuccessStatusCode)
        //    //        {
        //    //            this.combo_ITM_GRP_CLSS_CD.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
        //    //            this.combo_ITM_GRP_CLSS_CD.SelectedItem = (this.combo_ITM_GRP_CLSS_CD.ItemsSource as List<SystemCodeVo>).Where(x => x.CLSS_CD.Equals("W")).FirstOrDefault<SystemCodeVo>();
        //    //        }

        //    //        //
        //    //        using (HttpResponseMessage responseX = await SystemProperties.PROGRAM_HTTP.PostAsync("s133", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { DELT_FLG = "N", ITM_GRP_CLSS_CD = this.orgDao.ITM_GRP_CLSS_CD, CRE_USR_ID = "", CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
        //    //        {
        //    //            if (responseX.IsSuccessStatusCode)
        //    //            {
        //    //                this.combo_N1ST_ITM_GRP_CD.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await responseX.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
        //    //                if ((this.combo_N1ST_ITM_GRP_CD.ItemsSource as List<SystemCodeVo>).Count > 0)
        //    //                {
        //    //                    this.combo_N1ST_ITM_GRP_CD.SelectedItem = (this.combo_N1ST_ITM_GRP_CD.ItemsSource as List<SystemCodeVo>)[0];
        //    //                }
        //    //                //this.combo_N1ST_ITM_GRP_CD.SelectedItem = (this.combo_N1ST_ITM_GRP_CD.ItemsSource as List<SystemCodeVo>).Where(x => x.ITM_GRP_CD.Equals(this.orgDao.N1ST_ITM_GRP_CD)).LastOrDefault<SystemCodeVo>();
        //    //            }
        //    //        }

        //    //        //
        //    //        using (HttpResponseMessage responseY = await SystemProperties.PROGRAM_HTTP.PostAsync("m6623/mst", new StringContent(JsonConvert.SerializeObject(new ManVo() { DELT_FLG = "N", ITM_GRP_CLSS_CD = this.orgDao.ITM_GRP_CLSS_CD, N1ST_ITM_GRP_CD = (this.combo_N1ST_ITM_GRP_CD.SelectedItem as SystemCodeVo)?.CLSS_CD, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
        //    //        {
        //    //            if (responseY.IsSuccessStatusCode)
        //    //            {
        //    //                this.combo_ASSY_ITM_CD.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await responseY.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
        //    //            }
        //    //        }

        //    //    }

        //    //    //
        //    //    //작업자 = 지시자
        //    //    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s136/usr", new StringContent(JsonConvert.SerializeObject(new GroupUserVo() { DELT_FLG = "N", CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
        //    //    {
        //    //        if (response.IsSuccessStatusCode)
        //    //        {
        //    //            this.combo_WRK_MAN_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<GroupUserVo>>(await response.Content.ReadAsStringAsync()).Cast<GroupUserVo>().ToList();
        //    //            this.combo_CRE_USR_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<GroupUserVo>>(await response.Content.ReadAsStringAsync()).Cast<GroupUserVo>().ToList();
        //    //        }
        //    //    }

             


        //    //    DXSplashScreen.Close();
        //    //}
        //    //catch (Exception)
        //    //{
        //    //    if (DXSplashScreen.IsActive == true)
        //    //    {
        //    //        DXSplashScreen.Close();
        //    //    }
        //    //    return;
        //    //}

        //}


        //IsEdit
        public bool IsEdit
        {
            get
            {
                return this.isEdit;
            }
        }
    }
}
