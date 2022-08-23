using AquilaErpWpfApp3.Util;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Core.Native;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Code;
using ModelsLibrary.Man;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AquilaErpWpfApp3.View.M.Dialog
{
    public partial class M6622MasterDialog : DXWindow
    {
        //private static ManServiceClient manClient = SystemProperties.ManClient;
        private ManVo orgDao;
        private bool isEdit = false;
        private ManVo updateDao;

        private string _title = "설비 관리";

        private M6622ExcelDialog excelDialog;

        private byte[] EQ_IMG = new byte[0];

        public M6622MasterDialog(ManVo Dao)
        {
            InitializeComponent();

            SYSTEM_CODE_VO();
            //
            //
            this.orgDao = Dao;
            ManVo copyDao = new ManVo()
            {
                PROD_EQ_NO = Dao.PROD_EQ_NO,
                EQ_MDL_NM = Dao.EQ_MDL_NM,
                EQ_NM = Dao.EQ_NM,
                EQ_SZ_NM = Dao.EQ_SZ_NM,
                MN_MTR_CSM_CTNT = Dao.MN_MTR_CSM_CTNT,
                EQ_QTY = Dao.EQ_QTY,
                EQ_MKR_NM = Dao.EQ_MKR_NM,
                EQ_PUR_AMT = Dao.EQ_PUR_AMT,
                EQ_PUR_YRMON = Dao.EQ_PUR_YRMON,
                EQ_GRD_CD = Dao.EQ_GRD_CD,
                BALL_STAR_AREA_CTNT = Dao.BALL_STAR_AREA_CTNT,
                USG_DESC = Dao.USG_DESC,
                EQ_LOC_CD = Dao.EQ_LOC_CD,
                EQ_LOC_NM = Dao.EQ_LOC_NM,
                EQ_DESC = Dao.EQ_DESC,
                CHNL_CD = Dao.CHNL_CD,
                CRE_USR_ID = Dao.CRE_USR_ID,
                UPD_USR_ID = Dao.UPD_USR_ID,
                INSP_DT = Dao.INSP_DT,
                EQ_IMG = Dao.EQ_IMG,
                EQ_SNOR_NO = Dao.EQ_SNOR_NO,
                EQ_TP_CD = Dao.EQ_TP_CD,
                EQ_TP_NM = Dao.EQ_TP_NM,
                EQ_SNOR_IMG = Dao.EQ_SNOR_IMG
            };

            //수정
            if (copyDao.PROD_EQ_NO != null)
            {
                this.text_PROD_EQ_NO.IsReadOnly = true;
                this.isEdit = true;
                //
                if ( string.IsNullOrEmpty(Dao.EQ_PUR_YRMON ?? "" ) == false)
                {
                    copyDao.EQ_PUR_YRMON = DateTime.ParseExact(Dao.EQ_PUR_YRMON + "01", "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None).ToString("yyyy-MM-dd");
                }

                //this.EQ_IMG = copyDao.EQ_IMG;
                this.EQ_IMG = (copyDao.EQ_IMG == null ? new byte[0] : copyDao.EQ_IMG);

                if (Dao.EQ_SNOR_IMG != null)
                {
                    BitmapImage biImg = new BitmapImage();
                    MemoryStream ms = new MemoryStream(Dao.EQ_SNOR_IMG);
                    biImg.BeginInit();
                    biImg.StreamSource = ms;
                    biImg.EndInit();
                    this.text_EQ_SNOR_IMG.Source = biImg;
                }
            }
            else
            {
                //추가
                this.EQ_IMG = new byte[0];

                //this.text_ClssTpCd.IsReadOnly = false;
                //this.isEdit = false;
                copyDao.EQ_PUR_YRMON = System.DateTime.Now.ToString("yyyy-MM-dd");
                copyDao.INSP_DT = System.DateTime.Now.ToString("yyyy-MM-dd");
                copyDao.EQ_QTY = 0;
                copyDao.EQ_PUR_AMT = 0;
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

                File.WriteAllBytes(makeDir, (this.EQ_IMG == null ? new byte[0] : this.EQ_IMG));

                //int ArraySize = (this.orgDao.CERTI_IMG == null ? 0 : this.orgDao.CERTI_IMG.Length);
                //FileStream fs = new FileStream(makeDir, FileMode.OpenOrCreate, FileAccess.Write);
                //fs.Write(this.orgDao.CERTI_IMG, 0, ArraySize);
                //fs.Close();

                excelDialog = new M6622ExcelDialog(makeDir);
                excelDialog.Title = "설비 관리" + " - 엑셀";
                excelDialog.Owner = Application.Current.MainWindow;
                excelDialog.BorderEffect = BorderEffect.Default;
                excelDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
                excelDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
                bool isDialog = (bool)excelDialog.ShowDialog();
                if (isDialog)
                {
                    this.EQ_IMG = excelDialog.IMAGE;
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
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6622/i", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
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

                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6622/u", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
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
                //    resultVo = manClient.M6622InsertMaster(this.updateDao);
                //    if (!resultVo.isSuccess)
                //    {
                //        //실패
                //        WinUIMessageBox.Show(resultVo.Message, "[" + SystemProperties.PROGRAM_TITLE + "]설비 관리", MessageBoxButton.OK, MessageBoxImage.Error);
                //        return;
                //    }
                //    //성공
                //    WinUIMessageBox.Show("완료 되었습니다", "[추가]설비 관리", MessageBoxButton.OK, MessageBoxImage.Information);
                //}
                //else
                //{
                //    this.updateDao = getDomain();
                //    resultVo = manClient.M6622UpdateMaster(this.updateDao);
                //    if (!resultVo.isSuccess)
                //    {
                //        //실패
                //        WinUIMessageBox.Show(resultVo.Message, "[" + SystemProperties.PROGRAM_TITLE + "]설비 관리", MessageBoxButton.OK, MessageBoxImage.Error);
                //        return;
                //    }
                //    //성공
                //    WinUIMessageBox.Show("완료 되었습니다", "[수정]설비 관리", MessageBoxButton.OK, MessageBoxImage.Information);
                //    //
                //    this.orgDao.PROD_EQ_NO = this.updateDao.PROD_EQ_NO;
                //    this.orgDao.EQ_MDL_NM = this.updateDao.EQ_MDL_NM;
                //    this.orgDao.EQ_NM = this.updateDao.EQ_NM;
                //    this.orgDao.EQ_SZ_NM = this.updateDao.EQ_SZ_NM;
                //    this.orgDao.MN_MTR_CSM_CTNT = this.updateDao.MN_MTR_CSM_CTNT;
                //    this.orgDao.EQ_QTY = this.updateDao.EQ_QTY;
                //    this.orgDao.EQ_MKR_NM = this.updateDao.EQ_MKR_NM;
                //    this.orgDao.EQ_PUR_AMT = this.updateDao.EQ_PUR_AMT;
                //    this.orgDao.EQ_PUR_YRMON = this.updateDao.EQ_PUR_YRMON;
                //    this.orgDao.EQ_GRD_CD = this.updateDao.EQ_GRD_CD;
                //    this.orgDao.BALL_STAR_AREA_CTNT = this.updateDao.BALL_STAR_AREA_CTNT;
                //    this.orgDao.USG_DESC = this.updateDao.USG_DESC;
                //    this.orgDao.EQ_LOC_CD = this.updateDao.EQ_LOC_CD;
                //    this.orgDao.EQ_LOC_NM = this.updateDao.EQ_LOC_NM;
                //    this.orgDao.EQ_DESC = this.updateDao.EQ_DESC;
                //    this.orgDao.CRE_USR_ID = this.updateDao.CRE_USR_ID;
                //    this.orgDao.UPD_USR_ID = this.updateDao.UPD_USR_ID;
                //    this.orgDao.CHNL_CD = this.updateDao.CHNL_CD;
                //    this.orgDao.INSP_DT = this.updateDao.INSP_DT;

                //    this.orgDao.EQ_IMG = this.updateDao.EQ_IMG;
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
            if (string.IsNullOrEmpty(this.text_PROD_EQ_NO.Text))
            {
                WinUIMessageBox.Show("[코드] 입력 값이 맞지 않습니다.", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.text_PROD_EQ_NO.IsTabStop = true;
                this.text_PROD_EQ_NO.Focus();
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
                //        PROD_EQ_NO = this.text_PROD_EQ_NO.Text,
                //        CHNL_CD = SystemProperties.USER_VO.CHNL_CD,
                //    };
                //    IList<ManVo> daoList = (IList<ManVo>)manClient.M6622SelectMaster(dao);
                //    if (daoList.Count != 0)
                //    {
                //        WinUIMessageBox.Show("[코드 - 중복] 코드를 다시 입력 하십시오.", "[중복검사]설비 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
                //        this.text_PROD_EQ_NO.IsTabStop = true;
                //        this.text_PROD_EQ_NO.Focus();
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
            Dao.PROD_EQ_NO = this.text_PROD_EQ_NO.Text;
            //Dao.EQ_MDL_NM = this.text_EQ_MDL_NM.Text;
            Dao.EQ_NM = this.text_EQ_NM.Text;
            Dao.EQ_SZ_NM = this.text_EQ_SZ_NM.Text;
            Dao.MN_MTR_CSM_CTNT = this.text_MN_MTR_CSM_CTNT.Text;
            Dao.EQ_MKR_NM = this.text_EQ_MKR_NM.Text;
            Dao.EQ_GRD_CD = this.text_EQ_GRD_CD.Text;
            Dao.BALL_STAR_AREA_CTNT = this.text_BALL_STAR_AREA_CTNT.Text;
            Dao.USG_DESC = this.text_USG_DESC.Text;
            //Dao.EQ_LOC_CD = this.text_EQ_LOC_CD.Text;
            SystemCodeVo EQ_LOC_CD_VO = this.combo_EQ_LOC_CD.SelectedItem as SystemCodeVo;
            if (EQ_LOC_CD_VO != null)
            {
                Dao.EQ_LOC_NM = EQ_LOC_CD_VO.CLSS_DESC;
                Dao.EQ_LOC_CD = EQ_LOC_CD_VO.CLSS_CD;
            }


            Dao.EQ_DESC = this.text_EQ_DESC.Text;

            Dao.EQ_PUR_YRMON = Convert.ToDateTime(this.text_EQ_PUR_YRMON.Text).ToString("yyyyMM");

            Dao.EQ_QTY = int.Parse((string.IsNullOrEmpty(this.text_EQ_QTY.Text) ? "0" : this.text_EQ_QTY.Text));
            Dao.EQ_PUR_AMT = long.Parse((string.IsNullOrEmpty(this.text_EQ_PUR_AMT.Text) ? "0" : this.text_EQ_PUR_AMT.Text));

            //
            Dao.CRE_USR_ID = SystemProperties.USER;
            Dao.UPD_USR_ID = SystemProperties.USER;
            Dao.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
            Dao.INSP_DT = Convert.ToDateTime(this.text_INSP_DT.Text).ToString("yyyy-MM-dd");

            Dao.EQ_IMG = this.EQ_IMG;//new byte[0];

            //Dao.EQ_SNOR_NO = this.combo_EQ_SNOR_NO.Text;
            Dao.EQ_SNOR_IMG = ImageToByte(this.text_EQ_SNOR_IMG.Source);

            SystemCodeVo EQ_TP_CD_VO = this.combo_EQ_TP_NM.SelectedItem as SystemCodeVo;
            if (EQ_TP_CD_VO != null)
            {
                Dao.EQ_TP_NM = EQ_TP_CD_VO.CLSS_DESC;
                Dao.EQ_TP_CD = EQ_TP_CD_VO.CLSS_CD;
            }

            return Dao;
        }
        #endregion


        public async void SYSTEM_CODE_VO()
        {
            //this.combo_EQ_LOC_CD.ItemsSource = SystemProperties.SYSTEM_CODE_VO("L-002");
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "L-002"))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.combo_EQ_LOC_CD.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                }
            }

            ////센서
            //using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m66211", new StringContent(JsonConvert.SerializeObject(new ManVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD, DELT_FLG = "N" }), System.Text.Encoding.UTF8, "application/json")))
            //{

            //    if (response.IsSuccessStatusCode)
            //    {
            //        this.combo_EQ_SNOR_NO.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
            //    }
            //}

            //설비 유형
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "L-700"))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.combo_EQ_TP_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
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

        private byte[] ImageToByte(ImageSource img)
        {
            if (img == null)
            {
                //WinUIMessageBox.Show("이미지 등록을 권장 합니다.", "[유효검사]장비 등록", MessageBoxButton.OK, MessageBoxImage.Information);
                return new byte[0];
            }
            BitmapImage biImg = img as BitmapImage;
            Stream stream = biImg.StreamSource;
            return stream.GetDataFromStream();
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
