using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Man;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using AquilaErpWpfApp3.Util;

namespace AquilaErpWpfApp3.View.M.Dialog
{
    public partial class M6626MasterDialog : DXWindow
    {
        private string _title = "계측기 관리";
        //private static ManServiceClient manClient = SystemProperties.ManClient;
        private ManVo orgDao;
        private bool isEdit = false;
        private ManVo updateDao;

        private M6626ExcelDialog excelDialog;

        private byte[] INSRT_IMG = new byte[0];


        public M6626MasterDialog(ManVo Dao)
        {
            InitializeComponent();

            //this.combo_EQ_LOC_CD.ItemsSource = SystemProperties.SYSTEM_CODE_VO("L-002");
            //
            //
            this.orgDao = Dao;
            ManVo copyDao = new ManVo()
            {
                INSRT_MGMT_NO = Dao.INSRT_MGMT_NO,
                INSRT_NM = Dao.INSRT_NM,
                INSRT_SZ = Dao.INSRT_SZ,
                SER_NO = Dao.SER_NO,
                MAKE_CO_NM = Dao.MAKE_CO_NM,
                PUR_CO_NM = Dao.PUR_CO_NM,
                PUR_DT = Dao.PUR_DT,
                INSRT_FX_RMK = Dao.INSRT_FX_RMK,
                INSRT_FX_DT = Dao.INSRT_FX_DT,
                INSRT_NXT_FX_DT = Dao.INSRT_NXT_FX_DT,
                INSRT_USE_LOC_RMK = Dao.INSRT_USE_LOC_RMK,
                INSRT_RMK = Dao.INSRT_RMK,
                CHNL_CD = Dao.CHNL_CD,
                CRE_USR_ID = Dao.CRE_USR_ID,
                UPD_USR_ID = Dao.UPD_USR_ID,
                INSRT_IMG = Dao.INSRT_IMG
            };

            //수정
            if (copyDao.INSRT_MGMT_NO != null)
            {
                this.INSRT_IMG = new byte[0];

                this.text_INSRT_MGMT_NO.IsReadOnly = true;
                this.isEdit = true;

                this.INSRT_IMG = (copyDao.INSRT_IMG == null ? new byte[0] : copyDao.INSRT_IMG);
                //this.INSRT_IMG = copyDao.INSRT_IMG;
                //
                //copyDao.EQ_PUR_YRMON = DateTime.ParseExact(Dao.EQ_PUR_YRMON + "01", "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None).ToString("yyyy-MM-dd");
            }
            else
            {
                //추가
                //this.text_ClssTpCd.IsReadOnly = false;
                //this.isEdit = false;
                copyDao.PUR_DT = System.DateTime.Now.ToString("yyyy-MM-dd");
                copyDao.INSRT_FX_DT = System.DateTime.Now.ToString("yyyy-MM-dd");
                copyDao.INSRT_NXT_FX_DT = System.DateTime.Now.ToString("yyyy-MM-dd");
                //copyDao.EQ_QTY = 0;
                //copyDao.EQ_PUR_AMT = 0;
                this.INSRT_IMG = new byte[0];
            }
            this.configCode.DataContext = copyDao;
            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);

            this.EXCELButton.Click += new RoutedEventHandler(EXCELButton_Click);

            this.OKButton.Click += new RoutedEventHandler(OKButton_Click);
            this.CancelButton.Click += new RoutedEventHandler(CancelButton_Click);
        }

        void EXCELButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //임시 폴더
                string tempFolderPath = System.IO.Path.GetTempPath();
                //
                string makeDir = tempFolderPath + System.DateTime.Now.ToString("yyyyMMddHHmmss.xlsx");

                File.WriteAllBytes(makeDir, (this.INSRT_IMG == null ? new byte[0] : this.INSRT_IMG));

                //int ArraySize = (this.orgDao.CERTI_IMG == null ? 0 : this.orgDao.CERTI_IMG.Length);
                //FileStream fs = new FileStream(makeDir, FileMode.OpenOrCreate, FileAccess.Write);
                //fs.Write(this.orgDao.CERTI_IMG, 0, ArraySize);
                //fs.Close();

                excelDialog = new M6626ExcelDialog(makeDir);
                excelDialog.Title = _title + " - 엑셀";
                excelDialog.Owner = Application.Current.MainWindow;
                excelDialog.BorderEffect = BorderEffect.Default;
                excelDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
                excelDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
                bool isDialog = (bool)excelDialog.ShowDialog();
                if (isDialog)
                {
                    this.INSRT_IMG = excelDialog.IMAGE;
                    //파일 삭제
                    File.Delete(makeDir);
                }
            }
            catch (Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
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
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6626/i", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
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

                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6626/u", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
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
                //    resultVo = manClient.M6626InsertMaster(this.updateDao);
                //    if (!resultVo.isSuccess)
                //    {
                //        //실패
                //        WinUIMessageBox.Show(resultVo.Message, "[" + SystemProperties.PROGRAM_TITLE + "]계측기 관리", MessageBoxButton.OK, MessageBoxImage.Error);
                //        return;
                //    }
                //    //성공
                //    WinUIMessageBox.Show("완료 되었습니다", "[추가]계측기 관리", MessageBoxButton.OK, MessageBoxImage.Information);
                //}
                //else
                //{
                //    this.updateDao = getDomain();
                //    resultVo = manClient.M6626UpdateMaster(this.updateDao);
                //    if (!resultVo.isSuccess)
                //    {
                //        //실패
                //        WinUIMessageBox.Show(resultVo.Message, "[" + SystemProperties.PROGRAM_TITLE + "]계측기 관리", MessageBoxButton.OK, MessageBoxImage.Error);
                //        return;
                //    }
                //    //성공
                //    WinUIMessageBox.Show("완료 되었습니다", "[수정]계측기 관리", MessageBoxButton.OK, MessageBoxImage.Information);
                //    //
                //    this.orgDao.INSRT_MGMT_NO = this.updateDao.INSRT_MGMT_NO;
                //    this.orgDao.INSRT_NM = this.updateDao.INSRT_NM;
                //    this.orgDao.INSRT_SZ = this.updateDao.INSRT_SZ;
                //    this.orgDao.SER_NO = this.updateDao.SER_NO;
                //    this.orgDao.MAKE_CO_NM = this.updateDao.MAKE_CO_NM;
                //    this.orgDao.PUR_CO_NM = this.updateDao.PUR_CO_NM;
                //    this.orgDao.PUR_DT = this.updateDao.PUR_DT;
                //    this.orgDao.INSRT_FX_RMK = this.updateDao.INSRT_FX_RMK;
                //    this.orgDao.INSRT_FX_DT = this.updateDao.INSRT_FX_DT;
                //    this.orgDao.INSRT_NXT_FX_DT = this.updateDao.INSRT_NXT_FX_DT;
                //    this.orgDao.INSRT_USE_LOC_RMK = this.updateDao.INSRT_USE_LOC_RMK;
                //    this.orgDao.INSRT_RMK = this.updateDao.INSRT_RMK;
                //    this.orgDao.CRE_USR_ID = this.updateDao.CRE_USR_ID;
                //    this.orgDao.UPD_USR_ID = this.updateDao.UPD_USR_ID;
                //    this.orgDao.CHNL_CD = this.updateDao.CHNL_CD;

                //    this.orgDao.INSRT_IMG = this.updateDao.INSRT_IMG;
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
            if (string.IsNullOrEmpty(this.text_INSRT_MGMT_NO.Text))
            {
                WinUIMessageBox.Show("[관리번호] 입력 값이 맞지 않습니다.", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.text_INSRT_MGMT_NO.IsTabStop = true;
                this.text_INSRT_MGMT_NO.Focus();
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
                //        INSRT_MGMT_NO = this.text_INSRT_MGMT_NO.Text,
                //        CHNL_CD = SystemProperties.USER_VO.CHNL_CD,
                //    };
                //    IList<ManVo> daoList = (IList<ManVo>)manClient.M6626SelectMaster(dao);
                //    if (daoList.Count != 0)
                //    {
                //        WinUIMessageBox.Show("[코드 - 중복] 코드를 다시 입력 하십시오.", "[중복검사]계측기 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
                //        this.text_INSRT_MGMT_NO.IsTabStop = true;
                //        this.text_INSRT_MGMT_NO.Focus();
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

            Dao.INSRT_MGMT_NO = this.text_INSRT_MGMT_NO.Text;
            Dao.INSRT_NM = this.text_INSRT_NM.Text;
            Dao.INSRT_SZ = this.text_INSRT_SZ.Text;
            Dao.SER_NO = this.text_SER_NO.Text;
            Dao.MAKE_CO_NM = this.text_MAKE_CO_NM.Text;
            Dao.PUR_CO_NM = this.text_PUR_CO_NM.Text;

            Dao.INSRT_FX_RMK = this.text_INSRT_FX_RMK.Text;

            Dao.INSRT_USE_LOC_RMK = this.text_INSRT_USE_LOC_RMK.Text;
            Dao.INSRT_RMK = this.text_INSRT_RMK.Text;

            Dao.CRE_USR_ID = SystemProperties.USER;
            Dao.UPD_USR_ID = SystemProperties.USER;
            Dao.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;

            Dao.PUR_DT = Convert.ToDateTime(this.text_PUR_DT.Text).ToString("yyyy-MM-dd");
            Dao.INSRT_FX_DT = Convert.ToDateTime(this.text_INSRT_FX_DT.Text).ToString("yyyy-MM-dd");
            Dao.INSRT_NXT_FX_DT = Convert.ToDateTime(this.text_INSRT_NXT_FX_DT.Text).ToString("yyyy-MM-dd");

            Dao.INSRT_IMG = this.INSRT_IMG;

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

    }
}
