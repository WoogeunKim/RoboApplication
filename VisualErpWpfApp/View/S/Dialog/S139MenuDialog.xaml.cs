using DevExpress.Xpf.Core;
using DevExpress.Xpf.Core.Native;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Auth;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Windows;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using AquilaErpWpfApp3.Util;
using ModelsLibrary.Code;

namespace AquilaErpWpfApp3.View.S.Dialog
{
    /// <summary>
    /// Interaction logic for ConfigView1Dialog.xaml
    /// </summary>
    public partial class S139MenuDialog : DXWindow
    {
        private string _title = "프로그램 메뉴 관리";
        //private static CodeServiceClient codeClient = SystemProperties.CodeClient;
        //private static AuthorServiceClient authClient = SystemProperties.AuthClient;

        private ProgramVo orgDao;
        private bool isEdit = false;
        private ProgramVo updateDao;

        public S139MenuDialog(ProgramVo Dao)
        {
            InitializeComponent();
            //
            this.orgDao = Dao;
            ProgramVo copyDao = new ProgramVo()
            {
                MDL_ID = Dao.MDL_ID,
                MDL_NM = Dao.MDL_NM,
                MDL_DESC = Dao.MDL_DESC,
                PRNT_MDL_ID = Dao.PRNT_MDL_ID,
                PRNT_MDL_NM = Dao.PRNT_MDL_NM,
                PGM_CD = Dao.PGM_CD,
                PGM_NM = Dao.PGM_NM,
                PGM_IMG_NM = Dao.PGM_IMG_NM,
                SRT_ORD_SEQ = Dao.SRT_ORD_SEQ,
                SYS_AREA_CD = Dao.SYS_AREA_CD,
                SYS_AREA_NM = Dao.SYS_AREA_NM,
                DELT_FLG = Dao.DELT_FLG,
                CHNL_CD = Dao.CHNL_CD,
                CRE_USR_ID = Dao.CRE_USR_ID,
                UPD_USR_ID = Dao.UPD_USR_ID,
                IMAGE = Dao.IMAGE
            };

            //수정
            if (copyDao.MDL_ID != null)
            {
                this.text_MDL_ID.IsReadOnly = true;
                this.isEdit = true;

                if (Dao.IMAGE != null)
                {
                    BitmapImage biImg = new BitmapImage();
                    MemoryStream ms = new MemoryStream(Dao.IMAGE);
                    biImg.BeginInit();
                    biImg.StreamSource = ms;
                    biImg.EndInit();
                    this.text_Image.Source = biImg;
                }
            }
            else
            {
                //추가
                this.text_MDL_ID.IsReadOnly = false;
                this.isEdit = false;
                copyDao.DELT_FLG = "사용";
            }


            PRNT_MENU_VO();
            SYSTEM_CODE_VO();
            //this.combo_PRNT_MDL_NM.ItemsSource = PRNT_MENU_VO();
            //this.combo_SYS_AREA_NM.ItemsSource = SystemProperties.SYSTEM_CODE_VO("S-009");

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
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s139/i", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
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

                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s139/u", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
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
            if (string.IsNullOrEmpty(this.text_MDL_ID.Text))
            {
                WinUIMessageBox.Show("[메뉴 ID] 입력 값이 맞지 않습니다.", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.text_MDL_ID.IsTabStop = true;
                this.text_MDL_ID.Focus();
                return false;
            }
            //else if (string.IsNullOrEmpty(this.combo_PRNT_MDL_NM.Text))
            //{
            //    WinUIMessageBox.Show("[상위 메뉴 ID] 입력 값이 맞지 않습니다.", "[유효검사]프로그램 메뉴 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.combo_PRNT_MDL_NM.IsTabStop = true;
            //    this.combo_PRNT_MDL_NM.Focus();
            //    return false;
            //}
            else if (string.IsNullOrEmpty(this.text_MDL_NM.Text))
            {
                WinUIMessageBox.Show("[메뉴 명] 입력 값이 맞지 않습니다.", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.text_MDL_NM.IsTabStop = true;
                this.text_MDL_NM.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(this.text_MDL_DESC.Text))
            {
                WinUIMessageBox.Show("[메뉴 설명] 입력 값이 맞지 않습니다.", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.text_MDL_DESC.IsTabStop = true;
                this.text_MDL_DESC.Focus();
                return false;
            }
            //else if (string.IsNullOrEmpty(this.combo_SYS_AREA_NM.Text))
            //{
            //    WinUIMessageBox.Show("[시스템 구분] 입력 값이 맞지 않습니다.", "[유효검사]프로그램 메뉴 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.combo_SYS_AREA_NM.IsTabStop = true;
            //    this.combo_SYS_AREA_NM.Focus();
            //    return false;
            //}
            //else if (string.IsNullOrEmpty(this.combo_PGM_CD.Text))
            //{
            //    WinUIMessageBox.Show("[메뉴 종류] 입력 값이 맞지 않습니다.", "[유효검사]프로그램 메뉴 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.combo_PGM_CD.IsTabStop = true;
            //    this.combo_PGM_CD.Focus();
            //    return false;
            //}
            //else if (string.IsNullOrEmpty(this.text_PGM_NM.Text))
            //{
            //    WinUIMessageBox.Show("[실행 파일 명] 입력 값이 맞지 않습니다.", "[유효검사]프로그램 메뉴 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.text_PGM_NM.IsTabStop = true;
            //    this.text_PGM_NM.Focus();
            //    return false;
            //}
            //else if (string.IsNullOrEmpty(this.text_PGM_IMG_NM.Text))
            //{
            //    WinUIMessageBox.Show("[이미지 파일 명] 입력 값이 맞지 않습니다.", "[유효검사]프로그램 메뉴 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.text_PGM_IMG_NM.IsTabStop = true;
            //    this.text_PGM_IMG_NM.Focus();
            //    return false;
            //}
            //else if (string.IsNullOrEmpty(this.text_SRT_ORD_SEQ.Text))
            //{
            //    WinUIMessageBox.Show("[순서] 입력 값이 맞지 않습니다.", "[유효검사]프로그램 메뉴 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.text_SRT_ORD_SEQ.IsTabStop = true;
            //    this.text_SRT_ORD_SEQ.Focus();
            //    return false;
            //}
            else if (string.IsNullOrEmpty(this.combo_DELT_FLG.Text))
            {
                WinUIMessageBox.Show("[사용 여부] 입력 값이 맞지 않습니다.", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.combo_DELT_FLG.IsTabStop = true;
                this.combo_DELT_FLG.Focus();
                return false;
            }
            else
            {
                //if (this.isEdit == false)
                //{
                //    ProgramVo dao = new ProgramVo()
                //    {
                //        MDL_ID = this.text_MDL_ID.Text,
                //    };
                //    IList<ProgramVo> daoList = (IList<ProgramVo>)authClient.SelectProgramMenuTotalList(dao);
                //    if (daoList.Count != 0)
                //    {
                //        WinUIMessageBox.Show("[코드 - 중복] 코드를 다시 입력 하십시오.", "[중복검사]프로그램 메뉴 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
                //        this.text_MDL_ID.IsTabStop = true;
                //        this.text_MDL_ID.Focus();
                //        return false;
                //    }
                //}
            }
            return true;
        }
        #endregion

        #region Functon (getDomain - ConfigView1Dao)
        private ProgramVo getDomain()
        {
            ProgramVo Dao = new ProgramVo();
            Dao.MDL_ID = this.text_MDL_ID.Text;

            ProgramVo prntMdlNmVo = this.combo_PRNT_MDL_NM.SelectedItem as ProgramVo;
            ////Edit
            if (prntMdlNmVo != null)
            {
                Dao.PRNT_MDL_NM = prntMdlNmVo.MDL_NM;
                Dao.PRNT_MDL_ID = prntMdlNmVo.MDL_ID;
            }

            Dao.MDL_NM = this.text_MDL_NM.Text;
            Dao.MDL_DESC = this.text_MDL_DESC.Text;

            SystemCodeVo sysAreaNmVo = this.combo_SYS_AREA_NM.SelectedItem as SystemCodeVo;
            if (sysAreaNmVo != null)
            {
                Dao.SYS_AREA_NM = sysAreaNmVo.CLSS_DESC;
                Dao.SYS_AREA_CD = sysAreaNmVo.CLSS_CD;
            }

            Dao.PGM_CD = this.combo_PGM_CD.Text;
            Dao.PGM_NM = this.text_PGM_NM.Text;
            Dao.PGM_IMG_NM = this.text_PGM_IMG_NM.Text;
            Dao.SRT_ORD_SEQ = Convert.ToInt32(this.text_SRT_ORD_SEQ.Text);
            Dao.DELT_FLG = (this.combo_DELT_FLG.Text.Equals("사용") ? "N" : "Y");

            Dao.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;

            Dao.CRE_USR_ID = SystemProperties.USER;
            Dao.UPD_USR_ID = SystemProperties.USER;

            //Dao.IMAGE = ImageToByte(this.text_Image.Source);
            Dao.IMAGE = (this.text_Image.EditValue ?? new byte[0]) as byte[];

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

        public ProgramVo resultDao
        {
            get
            {
                return this.updateDao;
            }
        }


        public async void SYSTEM_CODE_VO()
        {
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" +"S-009"))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.combo_SYS_AREA_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                }
            }
        }

        public async void PRNT_MENU_VO()
        {
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s139/prntmenu", new StringContent(JsonConvert.SerializeObject(new ProgramVo() { PRNT_MDL_ID = "PRNT_MDL_ID", CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.combo_PRNT_MDL_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<ProgramVo>>(await response.Content.ReadAsStringAsync()).Cast<ProgramVo>().ToList();
                }
            }
            //return new List<ProgramVo>();
            //IList<ProgramVo> MenuVo = authClient.SelectProgramMenuTotalList(new ProgramVo() { PRNT_MDL_ID = "PRNT_MDL_ID", CHNL_CD = SystemProperties.USER_VO.CHNL_CD });
            //IList<CodeDao> resultMap = new List<CodeDao>();
            //int nCnt = MenuVo.Count;
            //ProgramVo tmpVo;
            //resultMap.Clear();
            //for (int x = 0; x < nCnt; x++)
            //{
            //    tmpVo = MenuVo[x];
            //    resultMap.Add(new CodeDao() { CLSS_CD = tmpVo.MDL_ID, CLSS_DESC = tmpVo.MDL_NM });
            //}
            ////
            //resultMap.Insert(0, new CodeDao() { CLSS_CD = "T", CLSS_DESC = "최상위 메뉴" });
            //return resultMap;
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
    }
}
