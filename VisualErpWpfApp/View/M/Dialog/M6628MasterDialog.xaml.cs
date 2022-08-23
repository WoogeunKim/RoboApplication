using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Man;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;
using AquilaErpWpfApp3.Util;

namespace AquilaErpWpfApp3.View.M.Dialog
{
    public partial class M6628MasterDialog : DXWindow
    {
        private string _title = "계측기 검교정 계획서 관리";

        //private static ManServiceClient manClient = SystemProperties.ManClient;
        private ManVo orgDao;
        private bool isEdit = false;
        private ManVo updateDao;

        //private ExcelEditDialog excelDialog;

        //private byte[] INSRT_IMG = new byte[0];


        public M6628MasterDialog(ManVo Dao)
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
                INSRT_MGMT_YRMON = Dao.INSRT_MGMT_YRMON,
                MM_01 = Dao.MM_01,
                MM_02 = Dao.MM_02,
                MM_03 = Dao.MM_03,
                MM_04 = Dao.MM_04,
                MM_05 = Dao.MM_05,
                MM_06 = Dao.MM_06,
                MM_07 = Dao.MM_07,
                MM_08 = Dao.MM_08,
                MM_09 = Dao.MM_09,
                MM_10 = Dao.MM_10,
                MM_11 = Dao.MM_11,
                MM_12 = Dao.MM_12,
                MM_RMK = Dao.MM_RMK
            };

            //수정
            if (copyDao.INSRT_MGMT_NO != null)
            {
                //this.INSRT_IMG = new byte[0];

                //this.text_INSRT_MGMT_NO.IsReadOnly = true;
                this.isEdit = true;

               // this.INSRT_IMG = (copyDao.INSRT_IMG == null ? new byte[0] : copyDao.INSRT_IMG);
                //this.INSRT_IMG = copyDao.INSRT_IMG;
                //
                //copyDao.EQ_PUR_YRMON = DateTime.ParseExact(Dao.EQ_PUR_YRMON + "01", "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None).ToString("yyyy-MM-dd");
            }
            else
            {
                //추가
                //this.text_ClssTpCd.IsReadOnly = false;
                //this.isEdit = false;
                //copyDao.PUR_DT = System.DateTime.Now.ToString("yyyy-MM-dd");
                //copyDao.INSRT_FX_DT = System.DateTime.Now.ToString("yyyy-MM-dd");
                //copyDao.INSRT_NXT_FX_DT = System.DateTime.Now.ToString("yyyy-MM-dd");
                //copyDao.EQ_QTY = 0;
                //copyDao.EQ_PUR_AMT = 0;
                //this.INSRT_IMG = new byte[0];
            }
            this.configCode.DataContext = copyDao;
            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);

            //this.EXCELButton.Click += new RoutedEventHandler(EXCELButton_Click);

            this.OKButton.Click += new RoutedEventHandler(OKButton_Click);
            this.CancelButton.Click += new RoutedEventHandler(CancelButton_Click);
        }

        //void EXCELButton_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        //임시 폴더
        //        string tempFolderPath = System.IO.Path.GetTempPath();
        //        //
        //        string makeDir = tempFolderPath + System.DateTime.Now.ToString("yyyyMMddHHmmss.xlsx");

        //        File.WriteAllBytes(makeDir, (this.INSRT_IMG == null ? new byte[0] : this.INSRT_IMG));

        //        //int ArraySize = (this.orgDao.CERTI_IMG == null ? 0 : this.orgDao.CERTI_IMG.Length);
        //        //FileStream fs = new FileStream(makeDir, FileMode.OpenOrCreate, FileAccess.Write);
        //        //fs.Write(this.orgDao.CERTI_IMG, 0, ArraySize);
        //        //fs.Close();

        //        excelDialog = new ExcelEditDialog(makeDir);
        //        excelDialog.Title = "계측기 관리" + " - 엑셀";
        //        excelDialog.Owner = Application.Current.MainWindow;
        //        excelDialog.BorderEffect = BorderEffect.Default;
        //        //masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
        //        //masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
        //        bool isDialog = (bool)excelDialog.ShowDialog();
        //        if (isDialog)
        //        {
        //            this.INSRT_IMG = excelDialog.IMAGE;
        //            //파일 삭제
        //            File.Delete(makeDir);
        //        }
        //    }
        //    catch (Exception eLog)
        //    {
        //        WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]계측기 관리", MessageBoxButton.OK, MessageBoxImage.Error);
        //        return;
        //    }
        //}



        #region Functon (OKButton_Click, CancelButton_Click)
        private async void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValueCheckd())
            {
                //ManVo resultVo;
                if (isEdit == false)
                {
                    //this.updateDao = getDomain();//this.updateDao
                    //resultVo = manClient.M6626InsertMaster(this.updateDao);
                    //if (!resultVo.isSuccess)
                    //{
                    //    //실패
                    //    WinUIMessageBox.Show(resultVo.Message, "[" + SystemProperties.PROGRAM_TITLE + "]계측기 관리", MessageBoxButton.OK, MessageBoxImage.Error);
                    //    return;
                    //}
                    ////성공
                    //WinUIMessageBox.Show("완료 되었습니다", "[추가]계측기 관리", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    //삭제
                    this.updateDao = getDomain();
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6628/d", new StringContent(JsonConvert.SerializeObject(updateDao), System.Text.Encoding.UTF8, "application/json")))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            int _Num = 0;
                            string resultMsg = await response.Content.ReadAsStringAsync();
                            if (int.TryParse(resultMsg, out _Num) == false)
                            {
                                //실패
                                WinUIMessageBox.Show(resultMsg, _title, MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                        }
                    }
                    //resultVo = manClient.M6628DeleteMaster(this.updateDao);
                    //if (!resultVo.isSuccess)
                    //{
                    //    //실패
                    //    WinUIMessageBox.Show(resultVo.Message, "[" + SystemProperties.PROGRAM_TITLE + "]계측기 검교정 계획서 관리", MessageBoxButton.OK, MessageBoxImage.Error);
                    //    return;
                    //}

                    //
                    //
                    if (!string.IsNullOrEmpty(this.text_MM_01.Text))
                    {
                        //1월
                        this.updateDao.INSRT_MGMT_YRMON = this.orgDao.INSRT_MGMT_YRMON + "01";
                        this.updateDao.INSRT_MGMT_CD = this.text_MM_01.Text;
                        this.updateDao.INSRT_MGMT_RMK = this.text_INSRT_MGMT_RMK.Text;

                        //resultVo = manClient.M6628InsertMaster(this.updateDao);
                        //if (!resultVo.isSuccess)
                        //{
                        //    //실패
                        //    WinUIMessageBox.Show(resultVo.Message, "[" + SystemProperties.PROGRAM_TITLE + "]계측기 검교정 계획서 관리", MessageBoxButton.OK, MessageBoxImage.Error);
                        //    return;
                        //}
                        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6628/i", new StringContent(JsonConvert.SerializeObject(updateDao), System.Text.Encoding.UTF8, "application/json")))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                int _Num = 0;
                                string resultMsg = await response.Content.ReadAsStringAsync();
                                if (int.TryParse(resultMsg, out _Num) == false)
                                {
                                    //실패
                                    WinUIMessageBox.Show(resultMsg, _title, MessageBoxButton.OK, MessageBoxImage.Error);
                                    return;
                                }
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(this.text_MM_02.Text))
                    {
                        //2월
                        this.updateDao.INSRT_MGMT_YRMON = this.orgDao.INSRT_MGMT_YRMON + "02";
                        this.updateDao.INSRT_MGMT_CD = this.text_MM_02.Text;
                        this.updateDao.INSRT_MGMT_RMK = this.text_INSRT_MGMT_RMK.Text;

                        //resultVo = manClient.M6628InsertMaster(this.updateDao);
                        //if (!resultVo.isSuccess)
                        //{
                        //    //실패
                        //    WinUIMessageBox.Show(resultVo.Message, "[" + SystemProperties.PROGRAM_TITLE + "]계측기 검교정 계획서 관리", MessageBoxButton.OK, MessageBoxImage.Error);
                        //    return;
                        //}
                        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6628/i", new StringContent(JsonConvert.SerializeObject(updateDao), System.Text.Encoding.UTF8, "application/json")))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                int _Num = 0;
                                string resultMsg = await response.Content.ReadAsStringAsync();
                                if (int.TryParse(resultMsg, out _Num) == false)
                                {
                                    //실패
                                    WinUIMessageBox.Show(resultMsg, _title, MessageBoxButton.OK, MessageBoxImage.Error);
                                    return;
                                }
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(this.text_MM_03.Text))
                    {
                        //3월
                        this.updateDao.INSRT_MGMT_YRMON = this.orgDao.INSRT_MGMT_YRMON + "03";
                        this.updateDao.INSRT_MGMT_CD = this.text_MM_03.Text;
                        this.updateDao.INSRT_MGMT_RMK = this.text_INSRT_MGMT_RMK.Text;

                        //resultVo = manClient.M6628InsertMaster(this.updateDao);
                        //if (!resultVo.isSuccess)
                        //{
                        //    //실패
                        //    WinUIMessageBox.Show(resultVo.Message, "[" + SystemProperties.PROGRAM_TITLE + "]계측기 검교정 계획서 관리", MessageBoxButton.OK, MessageBoxImage.Error);
                        //    return;
                        //}
                        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6628/i", new StringContent(JsonConvert.SerializeObject(updateDao), System.Text.Encoding.UTF8, "application/json")))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                int _Num = 0;
                                string resultMsg = await response.Content.ReadAsStringAsync();
                                if (int.TryParse(resultMsg, out _Num) == false)
                                {
                                    //실패
                                    WinUIMessageBox.Show(resultMsg, _title, MessageBoxButton.OK, MessageBoxImage.Error);
                                    return;
                                }
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(this.text_MM_04.Text))
                    {
                        //4월
                        this.updateDao.INSRT_MGMT_YRMON = this.orgDao.INSRT_MGMT_YRMON + "04";
                        this.updateDao.INSRT_MGMT_CD = this.text_MM_04.Text;
                        this.updateDao.INSRT_MGMT_RMK = this.text_INSRT_MGMT_RMK.Text;

                        //resultVo = manClient.M6628InsertMaster(this.updateDao);
                        //if (!resultVo.isSuccess)
                        //{
                        //    //실패
                        //    WinUIMessageBox.Show(resultVo.Message, "[" + SystemProperties.PROGRAM_TITLE + "]계측기 검교정 계획서 관리", MessageBoxButton.OK, MessageBoxImage.Error);
                        //    return;
                        //}
                        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6628/i", new StringContent(JsonConvert.SerializeObject(updateDao), System.Text.Encoding.UTF8, "application/json")))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                int _Num = 0;
                                string resultMsg = await response.Content.ReadAsStringAsync();
                                if (int.TryParse(resultMsg, out _Num) == false)
                                {
                                    //실패
                                    WinUIMessageBox.Show(resultMsg, _title, MessageBoxButton.OK, MessageBoxImage.Error);
                                    return;
                                }
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(this.text_MM_05.Text))
                    {
                        //5월
                        this.updateDao.INSRT_MGMT_YRMON = this.orgDao.INSRT_MGMT_YRMON + "05";
                        this.updateDao.INSRT_MGMT_CD = this.text_MM_05.Text;
                        this.updateDao.INSRT_MGMT_RMK = this.text_INSRT_MGMT_RMK.Text;

                        //resultVo = manClient.M6628InsertMaster(this.updateDao);
                        //if (!resultVo.isSuccess)
                        //{
                        //    //실패
                        //    WinUIMessageBox.Show(resultVo.Message, "[" + SystemProperties.PROGRAM_TITLE + "]계측기 검교정 계획서 관리", MessageBoxButton.OK, MessageBoxImage.Error);
                        //    return;
                        //}
                        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6628/i", new StringContent(JsonConvert.SerializeObject(updateDao), System.Text.Encoding.UTF8, "application/json")))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                int _Num = 0;
                                string resultMsg = await response.Content.ReadAsStringAsync();
                                if (int.TryParse(resultMsg, out _Num) == false)
                                {
                                    //실패
                                    WinUIMessageBox.Show(resultMsg, _title, MessageBoxButton.OK, MessageBoxImage.Error);
                                    return;
                                }
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(this.text_MM_06.Text))
                    {
                        //6월
                        this.updateDao.INSRT_MGMT_YRMON = this.orgDao.INSRT_MGMT_YRMON + "06";
                        this.updateDao.INSRT_MGMT_CD = this.text_MM_06.Text;
                        this.updateDao.INSRT_MGMT_RMK = this.text_INSRT_MGMT_RMK.Text;

                        //resultVo = manClient.M6628InsertMaster(this.updateDao);
                        //if (!resultVo.isSuccess)
                        //{
                        //    //실패
                        //    WinUIMessageBox.Show(resultVo.Message, "[" + SystemProperties.PROGRAM_TITLE + "]계측기 검교정 계획서 관리", MessageBoxButton.OK, MessageBoxImage.Error);
                        //    return;
                        //}
                        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6628/i", new StringContent(JsonConvert.SerializeObject(updateDao), System.Text.Encoding.UTF8, "application/json")))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                int _Num = 0;
                                string resultMsg = await response.Content.ReadAsStringAsync();
                                if (int.TryParse(resultMsg, out _Num) == false)
                                {
                                    //실패
                                    WinUIMessageBox.Show(resultMsg, _title, MessageBoxButton.OK, MessageBoxImage.Error);
                                    return;
                                }
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(this.text_MM_07.Text))
                    {
                        //7월
                        this.updateDao.INSRT_MGMT_YRMON = this.orgDao.INSRT_MGMT_YRMON + "07";
                        this.updateDao.INSRT_MGMT_CD = this.text_MM_07.Text;
                        this.updateDao.INSRT_MGMT_RMK = this.text_INSRT_MGMT_RMK.Text;

                        //resultVo = manClient.M6628InsertMaster(this.updateDao);
                        //if (!resultVo.isSuccess)
                        //{
                        //    //실패
                        //    WinUIMessageBox.Show(resultVo.Message, "[" + SystemProperties.PROGRAM_TITLE + "]계측기 검교정 계획서 관리", MessageBoxButton.OK, MessageBoxImage.Error);
                        //    return;
                        //}
                        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6628/i", new StringContent(JsonConvert.SerializeObject(updateDao), System.Text.Encoding.UTF8, "application/json")))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                int _Num = 0;
                                string resultMsg = await response.Content.ReadAsStringAsync();
                                if (int.TryParse(resultMsg, out _Num) == false)
                                {
                                    //실패
                                    WinUIMessageBox.Show(resultMsg, _title, MessageBoxButton.OK, MessageBoxImage.Error);
                                    return;
                                }
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(this.text_MM_08.Text))
                    {
                        //8월
                        this.updateDao.INSRT_MGMT_YRMON = this.orgDao.INSRT_MGMT_YRMON + "08";
                        this.updateDao.INSRT_MGMT_CD = this.text_MM_08.Text;
                        this.updateDao.INSRT_MGMT_RMK = this.text_INSRT_MGMT_RMK.Text;

                        //resultVo = manClient.M6628InsertMaster(this.updateDao);
                        //if (!resultVo.isSuccess)
                        //{
                        //    //실패
                        //    WinUIMessageBox.Show(resultVo.Message, "[" + SystemProperties.PROGRAM_TITLE + "]계측기 검교정 계획서 관리", MessageBoxButton.OK, MessageBoxImage.Error);
                        //    return;
                        //}
                        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6628/i", new StringContent(JsonConvert.SerializeObject(updateDao), System.Text.Encoding.UTF8, "application/json")))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                int _Num = 0;
                                string resultMsg = await response.Content.ReadAsStringAsync();
                                if (int.TryParse(resultMsg, out _Num) == false)
                                {
                                    //실패
                                    WinUIMessageBox.Show(resultMsg, _title, MessageBoxButton.OK, MessageBoxImage.Error);
                                    return;
                                }
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(this.text_MM_09.Text))
                    {
                        //9월
                        this.updateDao.INSRT_MGMT_YRMON = this.orgDao.INSRT_MGMT_YRMON + "09";
                        this.updateDao.INSRT_MGMT_CD = this.text_MM_09.Text;
                        this.updateDao.INSRT_MGMT_RMK = this.text_INSRT_MGMT_RMK.Text;

                        //resultVo = manClient.M6628InsertMaster(this.updateDao);
                        //if (!resultVo.isSuccess)
                        //{
                        //    //실패
                        //    WinUIMessageBox.Show(resultVo.Message, "[" + SystemProperties.PROGRAM_TITLE + "]계측기 검교정 계획서 관리", MessageBoxButton.OK, MessageBoxImage.Error);
                        //    return;
                        //}
                        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6628/i", new StringContent(JsonConvert.SerializeObject(updateDao), System.Text.Encoding.UTF8, "application/json")))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                int _Num = 0;
                                string resultMsg = await response.Content.ReadAsStringAsync();
                                if (int.TryParse(resultMsg, out _Num) == false)
                                {
                                    //실패
                                    WinUIMessageBox.Show(resultMsg, _title, MessageBoxButton.OK, MessageBoxImage.Error);
                                    return;
                                }
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(this.text_MM_10.Text))
                    {
                        //10월
                        this.updateDao.INSRT_MGMT_YRMON = this.orgDao.INSRT_MGMT_YRMON + "10";
                        this.updateDao.INSRT_MGMT_CD = this.text_MM_10.Text;
                        this.updateDao.INSRT_MGMT_RMK = this.text_INSRT_MGMT_RMK.Text;

                        //resultVo = manClient.M6628InsertMaster(this.updateDao);
                        //if (!resultVo.isSuccess)
                        //{
                        //    //실패
                        //    WinUIMessageBox.Show(resultVo.Message, "[" + SystemProperties.PROGRAM_TITLE + "]계측기 검교정 계획서 관리", MessageBoxButton.OK, MessageBoxImage.Error);
                        //    return;
                        //}
                        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6628/i", new StringContent(JsonConvert.SerializeObject(updateDao), System.Text.Encoding.UTF8, "application/json")))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                int _Num = 0;
                                string resultMsg = await response.Content.ReadAsStringAsync();
                                if (int.TryParse(resultMsg, out _Num) == false)
                                {
                                    //실패
                                    WinUIMessageBox.Show(resultMsg, _title, MessageBoxButton.OK, MessageBoxImage.Error);
                                    return;
                                }
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(this.text_MM_11.Text))
                    {
                        //11월
                        this.updateDao.INSRT_MGMT_YRMON = this.orgDao.INSRT_MGMT_YRMON + "11";
                        this.updateDao.INSRT_MGMT_CD = this.text_MM_11.Text;
                        this.updateDao.INSRT_MGMT_RMK = this.text_INSRT_MGMT_RMK.Text;

                        //resultVo = manClient.M6628InsertMaster(this.updateDao);
                        //if (!resultVo.isSuccess)
                        //{
                        //    //실패
                        //    WinUIMessageBox.Show(resultVo.Message, "[" + SystemProperties.PROGRAM_TITLE + "]계측기 검교정 계획서 관리", MessageBoxButton.OK, MessageBoxImage.Error);
                        //    return;
                        //}
                        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6628/i", new StringContent(JsonConvert.SerializeObject(updateDao), System.Text.Encoding.UTF8, "application/json")))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                int _Num = 0;
                                string resultMsg = await response.Content.ReadAsStringAsync();
                                if (int.TryParse(resultMsg, out _Num) == false)
                                {
                                    //실패
                                    WinUIMessageBox.Show(resultMsg, _title, MessageBoxButton.OK, MessageBoxImage.Error);
                                    return;
                                }
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(this.text_MM_12.Text))
                    {
                        //12월
                        this.updateDao.INSRT_MGMT_YRMON = this.orgDao.INSRT_MGMT_YRMON + "12";
                        this.updateDao.INSRT_MGMT_CD = this.text_MM_12.Text;
                        this.updateDao.INSRT_MGMT_RMK = this.text_INSRT_MGMT_RMK.Text;

                        //resultVo = manClient.M6628InsertMaster(this.updateDao);
                        //if (!resultVo.isSuccess)
                        //{
                        //    //실패
                        //    WinUIMessageBox.Show(resultVo.Message, "[" + SystemProperties.PROGRAM_TITLE + "]계측기 검교정 계획서 관리", MessageBoxButton.OK, MessageBoxImage.Error);
                        //    return;
                        //}
                        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6628/i", new StringContent(JsonConvert.SerializeObject(updateDao), System.Text.Encoding.UTF8, "application/json")))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                int _Num = 0;
                                string resultMsg = await response.Content.ReadAsStringAsync();
                                if (int.TryParse(resultMsg, out _Num) == false)
                                {
                                    //실패
                                    WinUIMessageBox.Show(resultMsg, _title, MessageBoxButton.OK, MessageBoxImage.Error);
                                    return;
                                }
                            }
                        }
                    }

                    //성공
                    WinUIMessageBox.Show("완료 되었습니다", "[수정]" + _title, MessageBoxButton.OK, MessageBoxImage.Information);
                    ////
                    //this.orgDao.INSRT_MGMT_NO = this.updateDao.INSRT_MGMT_NO;
                    //this.orgDao.INSRT_NM = this.updateDao.INSRT_NM;
                    //this.orgDao.INSRT_SZ = this.updateDao.INSRT_SZ;
                    //this.orgDao.SER_NO = this.updateDao.SER_NO;
                    //this.orgDao.MAKE_CO_NM = this.updateDao.MAKE_CO_NM;
                    //this.orgDao.PUR_CO_NM = this.updateDao.PUR_CO_NM;
                    //this.orgDao.PUR_DT = this.updateDao.PUR_DT;
                    //this.orgDao.INSRT_FX_RMK = this.updateDao.INSRT_FX_RMK;
                    //this.orgDao.INSRT_FX_DT = this.updateDao.INSRT_FX_DT;
                    //this.orgDao.INSRT_NXT_FX_DT = this.updateDao.INSRT_NXT_FX_DT;
                    //this.orgDao.INSRT_USE_LOC_RMK = this.updateDao.INSRT_USE_LOC_RMK;
                    //this.orgDao.INSRT_RMK = this.updateDao.INSRT_RMK;
                    //this.orgDao.CRE_USR_ID = this.updateDao.CRE_USR_ID;
                    //this.orgDao.UPD_USR_ID = this.updateDao.UPD_USR_ID;
                    //this.orgDao.CHNL_CD = this.updateDao.CHNL_CD;
                    //this.orgDao.MM_01 = this.updateDao.MM_01;
                    //this.orgDao.MM_02 = this.updateDao.MM_02;
                    //this.orgDao.MM_03 = this.updateDao.MM_03;
                    //this.orgDao.MM_04 = this.updateDao.MM_04;
                    //this.orgDao.MM_05 = this.updateDao.MM_05;
                    //this.orgDao.MM_06 = this.updateDao.MM_06;
                    //this.orgDao.MM_07 = this.updateDao.MM_07;
                    //this.orgDao.MM_08 = this.updateDao.MM_08;
                    //this.orgDao.MM_09 = this.updateDao.MM_09;
                    //this.orgDao.MM_10 = this.updateDao.MM_10;
                    //this.orgDao.MM_11 = this.updateDao.MM_11;
                    //this.orgDao.MM_12 = this.updateDao.MM_12;
                    //this.orgDao.MM_RMK = this.updateDao.MM_RMK;
                    //this.orgDao.INSRT_IMG = this.updateDao.INSRT_IMG;
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
            //else
            //{
            //    if (this.isEdit == false)
            //    {
            //        ManVo dao = new ManVo()
            //        {
            //            INSRT_MGMT_NO = this.text_INSRT_MGMT_NO.Text,
            //            CHNL_CD = SystemProperties.USER_VO.CHNL_CD,
            //        };
            //        IList<ManVo> daoList = (IList<ManVo>)manClient.M6626SelectMaster(dao);
            //        if (daoList.Count != 0)
            //        {
            //            WinUIMessageBox.Show("[코드 - 중복] 코드를 다시 입력 하십시오.", "[중복검사]계측기 검교정 계획서 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
            //            this.text_INSRT_MGMT_NO.IsTabStop = true;
            //            this.text_INSRT_MGMT_NO.Focus();
            //            return false;
            //        }
            //    }
            //}
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

            //Dao.INSRT_FX_RMK = this.text_INSRT_FX_RMK.Text;

            //Dao.INSRT_USE_LOC_RMK = this.text_INSRT_USE_LOC_RMK.Text;
            //Dao.INSRT_RMK = this.text_INSRT_RMK.Text;

            Dao.CRE_USR_ID = SystemProperties.USER;
            Dao.UPD_USR_ID = SystemProperties.USER;
            Dao.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;

            //Dao.PUR_DT = Convert.ToDateTime(this.text_PUR_DT.Text).ToString("yyyy-MM-dd");
            //Dao.INSRT_FX_DT = Convert.ToDateTime(this.text_INSRT_FX_DT.Text).ToString("yyyy-MM-dd");
            //Dao.INSRT_NXT_FX_DT = Convert.ToDateTime(this.text_INSRT_NXT_FX_DT.Text).ToString("yyyy-MM-dd");

            //Dao.INSRT_IMG = this.INSRT_IMG;

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
