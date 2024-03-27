using DevExpress.Xpf.Core;
using DevExpress.Xpf.Core.Native;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Auth;
using ModelsLibrary.Code;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using AquilaErpWpfApp3.Util;

namespace AquilaErpWpfApp3.View.S.Dialog
{
    /// <summary>
    /// Interaction logic for ConfigView1Dialog.xaml
    /// </summary>
    public partial class S136UserDialog : DXWindow
    {
        private string title = "사용자 권한 관리";
        //private static AuthorServiceClient authClient = SystemProperties.AuthClient;
        private bool isEdit = false;

        private GroupUserVo updateDao;

        public S136UserDialog(GroupUserVo Dao)
        {
            InitializeComponent();

            SYSTEM_CODE_VO("L-002", "G-001", "S-051");
            GROUP_VO();
            //this.combo_EMPE_PLC_NM.ItemsSource = SystemProperties.SYSTEM_CODE_VO("L-002");
            //this.combo_GRP_NM.ItemsSource = GROUP_VO();
            //this.combo_OFC_PSN_NM.ItemsSource = SystemProperties.SYSTEM_CODE_VO("G-001");

            ////수정
            if (Dao.USR_ID != null)
            {
                this.isEdit = true;
                if (Dao.DELT_FLG.Equals("Y"))
                {
                    this.check_RESGN_CO_DT.IsChecked = true;
                }
                if (Dao.USR_IMG != null)
                {
                    BitmapImage biImg = new BitmapImage();
                    MemoryStream ms = new MemoryStream(Dao.USR_IMG);
                    biImg.BeginInit();
                    biImg.StreamSource = ms;
                    biImg.EndInit();
                    this.text_Image.Source = biImg;
                }
                //this.text_USR_PWD_RE.Text = Dao.USR_PWD;

                this.text_USR_ID.IsReadOnly = true;
                //
                this.text_USR_PWD.IsReadOnly = true;
                //this.text_USR_PWD_RE.IsReadOnly = true;

                if (Dao.TM_PAY_FLG.ToString().Equals("1"))
                {
                    this.check_TM_PAY_FLG.IsChecked = true;
                }
                else
                {
                    this.check_TM_PAY_FLG.IsChecked = false;
                }
            }
            else
            {
                //추가
                this.isEdit = false;
                Dao.JOIN_CO_DT = System.DateTime.Now.ToString("yyyy-MM-dd");

                this.InitPwdButton.IsEnabled = false;
                //초기 패스워드
                //this.text_USR_PWD.Text = "1";
                //this.text_USR_PWD_RE.Text = "1";
                Dao.USR_PWD = "1";
                //
                this.text_USR_PWD.IsReadOnly = true;
                //this.text_USR_PWD_RE.IsReadOnly = true;
            }
            this.configCode.DataContext = Dao;
            this.check_RESGN_CO_DT.Click += check_RESGN_CO_DT_Click;

            this.OkPwdButton.Click += new RoutedEventHandler(InitPwdButton_Click);

            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
            this.OKButton.Click += new RoutedEventHandler(OKButton_Click);
            this.CancelButton.Click += new RoutedEventHandler(CancelButton_Click);
        }

        private async void InitPwdButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.text_USR_PWD_RE.Text))
            {
                WinUIMessageBox.Show("[패스워드] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.text_USR_PWD_RE.IsTabStop = true;
                this.text_USR_PWD_RE.Focus();
                return;
            }
            else if (string.IsNullOrEmpty(this.text_USR_PWD_RE_OK.Text))
            {
                WinUIMessageBox.Show("[확인 패스워드] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.text_USR_PWD_RE_OK.IsTabStop = true;
                this.text_USR_PWD_RE_OK.Focus();
                return;
            }
            else if (!this.text_USR_PWD_RE.Text.Equals(this.text_USR_PWD_RE_OK.Text))
            {
                WinUIMessageBox.Show("[패스워드 일지 하지 않습니다.] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.text_USR_PWD_RE.IsTabStop = true;
                this.text_USR_PWD_RE.Focus();
                return;
            }
            else if (!(this.text_USR_PWD_RE.Text.Length >= 4))
            {
                WinUIMessageBox.Show("[패스워드 4글자 이상 입력] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.text_USR_PWD_RE.IsTabStop = true;
                this.text_USR_PWD_RE.Focus();
                return;
            }


            //
            //
            GroupUserVo Grp_nm_vo = Create_Selected_GrpNm_Value();
            if(Grp_nm_vo != null && Grp_nm_vo.OSTR_FLG.Equals("Y"))
            {
                if (ValueOstrCheckd())
                {
                    int _Num = 0;
                    this.updateDao = getDomain();
                    //초기 값 패스워드 
                    System.Security.Cryptography.SHA256Managed sha256Managed = new System.Security.Cryptography.SHA256Managed();
                    this.updateDao.USR_PWD = Convert.ToBase64String(sha256Managed.ComputeHash(System.Text.Encoding.UTF8.GetBytes(this.text_USR_PWD_RE.Text)));

                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s136/u/u", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string result = await response.Content.ReadAsStringAsync();
                            if (int.TryParse(result, out _Num) == false)
                            {
                                //실패
                                WinUIMessageBox.Show(result, title, MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }

                            //성공
                            WinUIMessageBox.Show("패스워드 변경 되었습니다", title, MessageBoxButton.OK, MessageBoxImage.Information);

                            this.text_USR_PWD_RE.Text = string.Empty;
                            this.text_USR_PWD_RE_OK.Text = string.Empty;
                        }
                    }
                }
            }
            else
            {
                if (ValueCheckd())
                {
                    int _Num = 0;
                    this.updateDao = getDomain();
                    //초기 값 패스워드 
                    System.Security.Cryptography.SHA256Managed sha256Managed = new System.Security.Cryptography.SHA256Managed();
                    this.updateDao.USR_PWD = Convert.ToBase64String(sha256Managed.ComputeHash(System.Text.Encoding.UTF8.GetBytes(this.text_USR_PWD_RE.Text)));

                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s136/u/u", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string result = await response.Content.ReadAsStringAsync();
                            if (int.TryParse(result, out _Num) == false)
                            {
                                //실패
                                WinUIMessageBox.Show(result, title, MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }

                            //성공
                            WinUIMessageBox.Show("패스워드 변경 되었습니다", title, MessageBoxButton.OK, MessageBoxImage.Information);

                            this.text_USR_PWD_RE.Text = string.Empty;
                            this.text_USR_PWD_RE_OK.Text = string.Empty;
                        }
                    }
                }
            }
        }

        void check_RESGN_CO_DT_Click(object sender, RoutedEventArgs e)
        {
            if (this.check_RESGN_CO_DT.IsChecked == true)
            {
                this.text_RESGN_CO_DT.IsReadOnly = false;
                this.text_RESGN_CO_DT.Background = null;
                this.text_RESGN_CO_DT.Text = System.DateTime.Now.ToString("yyyy-MM-dd");
            }
            else
            {
                this.text_RESGN_CO_DT.IsReadOnly = true;
                this.text_RESGN_CO_DT.Background = Brushes.DarkGray;
            }
        }

        #region Functon (OKButton_Click, CancelButton_Click)
        private async void OKButton_Click(object sender, RoutedEventArgs e)
        {
            GroupUserVo Grp_nm_vo = Create_Selected_GrpNm_Value();
            if (Grp_nm_vo != null && Grp_nm_vo.OSTR_FLG.Equals("Y"))
            {
                //고객사 체크
                if (ValueOstrCheckd())
                {
                    int _Num = 0;
                    //SystemCodeVo resultVo;
                    if (isEdit == false)
                    {
                        this.updateDao = getDomain();

                        //초기 값 패스워드 
                        System.Security.Cryptography.SHA256Managed sha256Managed = new System.Security.Cryptography.SHA256Managed();
                        this.updateDao.USR_PWD = Convert.ToBase64String(sha256Managed.ComputeHash(System.Text.Encoding.UTF8.GetBytes(this.text_USR_PWD.Text)));

                        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s136/u/i", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                string result = await response.Content.ReadAsStringAsync();
                                if (int.TryParse(result, out _Num) == false)
                                {
                                    //실패
                                    WinUIMessageBox.Show(result, title, MessageBoxButton.OK, MessageBoxImage.Error);
                                    return;
                                }

                                //성공
                                WinUIMessageBox.Show(this.updateDao.USR_ID + "/(패스워드 : " + 1 + ") 완료 되었습니다", title, MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                        }
                    }
                    else
                    {
                        this.updateDao = getDomain();

                        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s136/u/u", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                string result = await response.Content.ReadAsStringAsync();
                                if (int.TryParse(result, out _Num) == false)
                                {
                                    //실패
                                    WinUIMessageBox.Show(result, title, MessageBoxButton.OK, MessageBoxImage.Error);
                                    return;
                                }

                                //성공
                                WinUIMessageBox.Show("완료 되었습니다", title, MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                        }
                    }
                    this.DialogResult = true;
                    this.Close();
                }

            }
            else
            {
                if (ValueCheckd())
                {
                    int _Num = 0;
                    //SystemCodeVo resultVo;
                    if (isEdit == false)
                    {
                        this.updateDao = getDomain();

                        //초기 값 패스워드 
                        System.Security.Cryptography.SHA256Managed sha256Managed = new System.Security.Cryptography.SHA256Managed();
                        this.updateDao.USR_PWD = Convert.ToBase64String(sha256Managed.ComputeHash(System.Text.Encoding.UTF8.GetBytes(this.text_USR_PWD.Text)));

                        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s136/u/i", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                string result = await response.Content.ReadAsStringAsync();
                                if (int.TryParse(result, out _Num) == false)
                                {
                                    //실패
                                    WinUIMessageBox.Show(result, title, MessageBoxButton.OK, MessageBoxImage.Error);
                                    return;
                                }

                                //성공
                                WinUIMessageBox.Show(this.updateDao.USR_ID + "/(패스워드 : " + 1 + ") 완료 되었습니다", title, MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                        }
                    }
                    else
                    {
                        this.updateDao = getDomain();

                        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s136/u/u", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                string result = await response.Content.ReadAsStringAsync();
                                if (int.TryParse(result, out _Num) == false)
                                {
                                    //실패
                                    WinUIMessageBox.Show(result, title, MessageBoxButton.OK, MessageBoxImage.Error);
                                    return;
                                }

                                //성공
                                WinUIMessageBox.Show("완료 되었습니다", title, MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                        }
                    }


                    //GroupUserVo resultVo;
                    //if (isEdit == false)
                    //{
                    //    resultVo = authClient.InsertUserInfo(getDomain());
                    //    if (!resultVo.isSuccess)
                    //    {
                    //        //실패
                    //        WinUIMessageBox.Show(resultVo.Message, "[" + SystemProperties.PROGRAM_TITLE + "]사용자 권한 관리", MessageBoxButton.OK, MessageBoxImage.Error);
                    //        return;
                    //    }
                    //    //성공
                    //    WinUIMessageBox.Show("완료 되었습니다", "[추가]사용자 권한 관리", MessageBoxButton.OK, MessageBoxImage.Information);
                    //}
                    //else
                    //{
                    //    resultVo = authClient.UpdateUserInfo(getDomain());
                    //    if (!resultVo.isSuccess)
                    //    {
                    //        //실패
                    //        WinUIMessageBox.Show(resultVo.Message, "[" + SystemProperties.PROGRAM_TITLE + "]사용자 권한 관리", MessageBoxButton.OK, MessageBoxImage.Error);
                    //        return;
                    //    }
                    //    //성공
                    //    WinUIMessageBox.Show("완료 되었습니다", "[수정]사용자 권한 관리", MessageBoxButton.OK, MessageBoxImage.Information);
                    //}
                    this.DialogResult = true;
                    this.Close();
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
            if (string.IsNullOrEmpty(this.text_USR_ID.Text))
            {
                WinUIMessageBox.Show("[아이디] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.text_USR_ID.IsTabStop = true;
                this.text_USR_ID.Focus();
                return false;
            }
            //else if (string.IsNullOrEmpty(this.text_USR_PWD.Text))
            //{
            //    WinUIMessageBox.Show("[패스워드] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.text_USR_PWD.IsTabStop = true;
            //    this.text_USR_PWD.Focus();
            //    return false;
            //}
            //else if (string.IsNullOrEmpty(this.text_USR_PWD_RE.Text))
            //{
            //    WinUIMessageBox.Show("[확인 패스워드] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.text_USR_PWD_RE.IsTabStop = true;
            //    this.text_USR_PWD_RE.Focus();
            //    return false;
            //}
            //else if (!this.text_USR_PWD_RE.Text.Equals(this.text_USR_PWD.Text))
            //{
            //    WinUIMessageBox.Show("[패스워드 일지 하지 않습니다.] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.text_USR_PWD.IsTabStop = true;
            //    this.text_USR_PWD.Focus();
            //    return false;
            //}
            else if (string.IsNullOrEmpty(this.combo_EMPE_PLC_NM.Text))
            {
                WinUIMessageBox.Show("[사업장] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.combo_EMPE_PLC_NM.IsTabStop = true;
                this.combo_EMPE_PLC_NM.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(this.combo_GRP_NM.Text))
            {
                WinUIMessageBox.Show("[부서] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.combo_GRP_NM.IsTabStop = true;
                this.combo_GRP_NM.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(this.text_JOIN_CO_DT.Text))
            {
                WinUIMessageBox.Show("[입사일] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.text_JOIN_CO_DT.IsTabStop = true;
                this.text_JOIN_CO_DT.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(this.combo_OFC_PSN_NM.Text))
            {
                WinUIMessageBox.Show("[직책] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.combo_OFC_PSN_NM.IsTabStop = true;
                this.combo_OFC_PSN_NM.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(this.text_USR_N1ST_NM.Text))
            {
                WinUIMessageBox.Show("[이름] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.text_USR_N1ST_NM.IsTabStop = true;
                this.text_USR_N1ST_NM.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(this.text_EMPE_NO.Text))
            {
                WinUIMessageBox.Show("[사번] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.text_EMPE_NO.IsTabStop = true;
                this.text_EMPE_NO.Focus();
                return false;
            }
            else if (((this.text_Image.EditValue ?? new byte[0]) as byte[]).Length > 2197152)
            {
                WinUIMessageBox.Show("이미지 파일 크기가 2Mbyte 초과 하였습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }
            //else if (string.IsNullOrEmpty(this.text_PHN_NO.Text))
            //{
            //    WinUIMessageBox.Show("[전화번호] 입력 값이 맞지 않습니다.", "[유효검사]사용자 권한 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.text_PHN_NO.IsTabStop = true;
            //    this.text_PHN_NO.Focus();
            //    return false;
            //}
            //else if (string.IsNullOrEmpty(this.text_CELL_PHN_NO.Text))
            //{
            //    WinUIMessageBox.Show("[핸드폰] 입력 값이 맞지 않습니다.", "[유효검사]사용자 권한 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.text_CELL_PHN_NO.IsTabStop = true;
            //    this.text_CELL_PHN_NO.Focus();
            //    return false;
            //}
            //else if (string.IsNullOrEmpty(this.text_EML_ID.Text))
            //{
            //    WinUIMessageBox.Show("[이메일] 입력 값이 맞지 않습니다.", "[유효검사]사용자 권한 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.text_EML_ID.IsTabStop = true;
            //    this.text_EML_ID.Focus();
            //    return false;
            //}
            //else if (string.IsNullOrEmpty(this.text_SGN_NM.Text))
            //{
            //    WinUIMessageBox.Show("[서명] 입력 값이 맞지 않습니다.", "[유효검사]시스템 권한 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.text_SGN_NM.IsTabStop = true;
            //    this.text_SGN_NM.Focus();
            //    return false;
            //}
            else
            {
                if (this.isEdit == false)
                {
                    //GroupUserVo dao = new GroupUserVo()
                    //{
                    //    USR_ID = this.text_USR_ID.Text,
                    //};
                    //IList<GroupUserVo> daoList = (IList<GroupUserVo>)authClient.SelectUserList(dao);
                    //if (daoList.Count != 0)
                    //{
                    //    WinUIMessageBox.Show("[코드 - 중복] 코드를 다시 입력 하십시오.", "[중복검사]사용자 권한 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
                    //    this.text_USR_ID.IsTabStop = true;
                    //    this.text_USR_ID.Focus();
                    //    return false;
                    //}
                }
            }
            return true;
        }
        #endregion


        #region Functon (ValueOstrCheckd)
        public Boolean ValueOstrCheckd()
        {
            if (string.IsNullOrEmpty(this.text_USR_ID.Text))
            {
                WinUIMessageBox.Show("[아이디] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.text_USR_ID.IsTabStop = true;
                this.text_USR_ID.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(this.combo_GRP_NM.Text))
            {
                WinUIMessageBox.Show("[고객사] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.combo_GRP_NM.IsTabStop = true;
                this.combo_GRP_NM.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(this.text_USR_N1ST_NM.Text))
            {
                WinUIMessageBox.Show("[업체명] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.text_USR_N1ST_NM.IsTabStop = true;
                this.text_USR_N1ST_NM.Focus();
                return false;
            }
            else if (((this.text_Image.EditValue ?? new byte[0]) as byte[]).Length > 2197152)
            {
                WinUIMessageBox.Show("이미지 파일 크기가 2Mbyte 초과 하였습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }

            return true;
        }
        #endregion

        #region Functon (getDomain - ConfigView1Dao)
        private GroupUserVo getDomain()
        {
            GroupUserVo Dao = new GroupUserVo();
            SystemCodeVo EMPE_PLC_NMVo = this.combo_EMPE_PLC_NM.SelectedItem as SystemCodeVo;
            GroupUserVo GRP_NMVo = this.combo_GRP_NM.SelectedItem as GroupUserVo;
            SystemCodeVo OFC_PSN_NMVo = this.combo_OFC_PSN_NM.SelectedItem as SystemCodeVo;
            SystemCodeVo EMP_LOC_NMVo = this.combo_EMP_LOC_NM.SelectedItem as SystemCodeVo;

            Dao.USR_ID = this.text_USR_ID.Text;

            //System.Security.Cryptography.SHA256Managed sha256Managed = new System.Security.Cryptography.SHA256Managed();
            //Dao.USR_PWD = Convert.ToBase64String(sha256Managed.ComputeHash(System.Text.Encoding.UTF8.GetBytes(this.text_USR_PWD.Text)));

            //Dao.USR_PWD = this.text_USR_PWD.Text;
            if (EMPE_PLC_NMVo != null)
            {
                Dao.EMPE_PLC_NM = EMPE_PLC_NMVo.CLSS_CD;
                Dao.EMPE_PLC_CD = EMPE_PLC_NMVo.CLSS_DESC;
            }
            else
            {
                Dao.EMPE_PLC_NM = "001"; //본사
            } 

            if (GRP_NMVo != null)
            {
                Dao.GRP_ID = GRP_NMVo.GRP_ID;
                Dao.GRP_NM = GRP_NMVo.GRP_NM;
            }

            Dao.JOIN_CO_DT = this.text_JOIN_CO_DT.Text;
            if (this.check_RESGN_CO_DT.IsChecked == true)
            {
                Dao.DELT_FLG = "Y";
                Dao.RESGN_CO_DT = this.text_RESGN_CO_DT.Text;
            }
            else
            {
                Dao.DELT_FLG = "N";
            }

            if(OFC_PSN_NMVo != null)
            {
                Dao.OFC_PSN_CD = OFC_PSN_NMVo.CLSS_CD;
                Dao.OFC_PSN_NM = OFC_PSN_NMVo.CLSS_DESC;
            }
            Dao.USR_N1ST_NM = this.text_USR_N1ST_NM.Text;
            Dao.USR_LST_NM = "";
            Dao.EMPE_NO = this.text_EMPE_NO.Text;
            Dao.PHN_NO = this.text_PHN_NO.Text;
            Dao.CELL_PHN_NO = this.text_CELL_PHN_NO.Text;
            Dao.EML_ID = this.text_EML_ID.Text;
            Dao.ADDR = this.text_ADDR.Text;
            Dao.DTL_ADDR = "";
            Dao.LST_LGN_IP = "";
            //Dao.LST_LGN_DT = "";
            Dao.USR_IMG = ((this.text_Image.EditValue ?? new byte[0]) as byte[]);

            Dao.TM_PAY_FLG = (this.check_TM_PAY_FLG.IsChecked == true ? "Y" : "N");
            if(this.check_TM_PAY_FLG.IsChecked == true)
            {
                Dao.TM_PAY_AMT = (string.IsNullOrEmpty(this.text_TM_PAY_AMT.Text) ? 0 :Convert.ToInt32(this.text_TM_PAY_AMT.Text));
            }
            else
            {
                Dao.TM_PAY_AMT = 0;
            }

            if (EMP_LOC_NMVo != null) 
            {
                Dao.EMP_LOC_CD = EMP_LOC_NMVo.CLSS_CD;
                Dao.EMP_LOC_NM = EMP_LOC_NMVo.CLSS_DESC;
            }

            Dao.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
            Dao.CRE_USR_ID = SystemProperties.USER;
            Dao.UPD_USR_ID = SystemProperties.USER;
            Dao.SGN_NM = "";//this.text_SGN_NM.Text;
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

        //private byte[] ImageToByte(ImageSource img)
        //{
        //    if (img == null)
        //    {
        //        //WinUIMessageBox.Show("이미지 등록을 권장 합니다.", "[유효검사]장비 등록", MessageBoxButton.OK, MessageBoxImage.Information);
        //        return new byte[0];
        //    }
        //    BitmapImage biImg = img as BitmapImage;
        //    Stream stream = biImg.StreamSource;
        //    return stream.GetDataFromStream();
        //}

        //public static IList<CodeDao> GROUP_VO()
        //{
        //    IList<GroupUserVo> GroupVo = authClient.SelectGroupList(new GroupUserVo() { GRP_TP_CD = "G", CHNL_CD = SystemProperties.USER_VO.CHNL_CD });
        //    IList<CodeDao> resultMap = new List<CodeDao>();
        //    int nCnt = GroupVo.Count;
        //    GroupUserVo tmpVo;
        //    resultMap.Clear();
        //    for (int x = 0; x < nCnt; x++)
        //    {
        //        tmpVo = GroupVo[x];
        //        resultMap.Add(new CodeDao() { CLSS_CD = tmpVo.GRP_ID, CLSS_DESC = tmpVo.GRP_NM });
        //    }
        //    //
        //    //resultMap.Insert(0, new CodeDao() { CLSS_CD = "", CLSS_DESC = "" });
        //    return resultMap;
        //}

        public async void GROUP_VO()
        {
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s136/g", new StringContent(JsonConvert.SerializeObject(new GroupUserVo() { GRP_TP_CD = "G", CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.combo_GRP_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<GroupUserVo>>(await response.Content.ReadAsStringAsync()).Cast<GroupUserVo>().ToList();
                }
            }
            User_add_setting();
        }

        public async void SYSTEM_CODE_VO(string _CLSS_TP_CD1, string _CLSS_TP_CD2, string _CLSS_TP_CD3 )
        {
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + _CLSS_TP_CD1))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.combo_EMPE_PLC_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                }
            }

            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + _CLSS_TP_CD2))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.combo_OFC_PSN_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                }
            }


            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + _CLSS_TP_CD3))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.combo_EMP_LOC_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                }
            }

        }

        private void combo_GRP_NM_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            User_add_setting();
        }

        private void User_add_setting()
        {
            GroupUserVo Grp_nm_vo = Create_Selected_GrpNm_Value();

            if (Grp_nm_vo != null)
            {
                if (Grp_nm_vo.OSTR_FLG != null && Grp_nm_vo.OSTR_FLG.Equals("Y"))
                {
                    //부서, 고객사
                    this.text_GRP_NM.Text = "고객사* : ";
                    //이름, 업체명
                    this.text_USR_NM.Text = "업체명* : ";
                    this.text_EMPE_PLC_NM.Visibility = Visibility.Hidden;
                    this.combo_EMPE_PLC_NM.Visibility = Visibility.Hidden;
                    //입사일
                    this.text_CO_DT.Visibility = Visibility.Hidden;
                    this.text_JOIN_CO_DT.Visibility = Visibility.Hidden;
                    //퇴사일
                    this.check_RESGN_CO_DT.Visibility = Visibility.Hidden;
                    this.text_RESGN_CO_DT.Visibility = Visibility.Hidden;
                    //사번
                    this.text_EMPE_NO_NM.Visibility = Visibility.Hidden;
                    this.text_EMPE_NO.Visibility = Visibility.Hidden;
                    //직책
                    this.text_OFC_PSN_NM.Visibility = Visibility.Hidden;
                    this.combo_OFC_PSN_NM.Visibility = Visibility.Hidden;
                    //근무구역
                    this.text_EMP_LOC_NM.Visibility = Visibility.Hidden;
                    this.combo_EMP_LOC_NM.Visibility = Visibility.Hidden;
                }
                else
                {
                    //부서, 고객사
                    this.text_GRP_NM.Text = "부서* : ";
                    //이름, 업체명
                    this.text_USR_NM.Text = "이름* : ";
                    this.text_EMPE_PLC_NM.Visibility = Visibility.Visible;
                    this.combo_EMPE_PLC_NM.Visibility = Visibility.Visible;
                    //입사일
                    this.text_CO_DT.Visibility = Visibility.Visible;
                    this.text_JOIN_CO_DT.Visibility = Visibility.Visible;
                    //퇴사일
                    this.check_RESGN_CO_DT.Visibility = Visibility.Visible;
                    this.text_RESGN_CO_DT.Visibility = Visibility.Visible;
                    //사번
                    this.text_EMPE_NO_NM.Visibility = Visibility.Visible;
                    this.text_EMPE_NO.Visibility = Visibility.Visible;
                    //직책
                    this.text_OFC_PSN_NM.Visibility = Visibility.Visible;
                    this.combo_OFC_PSN_NM.Visibility = Visibility.Visible;
                    //근무구역
                    this.text_EMP_LOC_NM.Visibility = Visibility.Visible;
                    this.combo_EMP_LOC_NM.Visibility = Visibility.Visible;
                }
            }
        }

        private GroupUserVo Create_Selected_GrpNm_Value()
        {
            GroupUserVo Grp_nm_vo = this.combo_GRP_NM.SelectedItem as GroupUserVo;

            return Grp_nm_vo;
        }

    }

}
