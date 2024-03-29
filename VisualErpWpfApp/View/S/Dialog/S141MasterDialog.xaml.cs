﻿using DevExpress.Xpf.Core;
using DevExpress.Xpf.Core.Native;
using DevExpress.Xpf.WindowsUI;
using Microsoft.Win32;
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
using ModelsLibrary.Sale;

namespace AquilaErpWpfApp3.View.S.Dialog
{
    /// <summary>
    /// Interaction logic for ConfigView1Dialog.xaml
    /// </summary>
    public partial class S141MasterDialog : DXWindow
    {
        //private static CodeServiceClient codeClient = SystemProperties.CodeClient;
        private string title = "품목 마스터 등록";

        private SystemCodeVo orgDao;
        private bool isEdit = false;
        private byte[] fileByte = null;

        private byte[] fileByte1 = null;

        private SystemCodeVo updateVo;

        public S141MasterDialog(SystemCodeVo Dao)
        {
            InitializeComponent();


            SYSTEM_CODE_VO();
            //중분류
            //this.combo_ITM_GRP_CLSS_CD.SelectedIndexChanged += combo_ITM_GRP_CLSS_CD_SelectedIndexChanged;
            //대분류
            //this.combo_N1ST_ITM_GRP_CD.SelectedIndexChanged += combo_N1ST_ITM_GRP_CD_SelectedIndexChanged;



            //this.combo_PROD_LOC_CD.ItemsSource = SystemProperties.SYSTEM_CODE_VO("L-100");
            //this.combo_ITM_GRP_CLSS_CD.ItemsSource = SystemProperties.SYSTEM_CODE_VO("L-001");
            //this.combo_UOM_CD.ItemsSource = SystemProperties.SYSTEM_CODE_VO("L-003");
            //this.combo_QLTY_INSP_CD.ItemsSource = SystemProperties.SYSTEM_CODE_VO("L-004");
            //this.combo_ABC_CD.ItemsSource = SystemProperties.SYSTEM_CODE_VO("L-200");
            //this.combo_INSP_ITM_GRP_CD.ItemsSource = SystemProperties.SYSTEM_CODE_VO("Q-005");

            //this.combo_ITM_GRP_CLSS_CD.SelectedIndexChanged += combo_ITM_GRP_CLSS_CD_SelectedIndexChanged;

            //this.combo_N1ST_ITM_GRP_CD.SelectedIndexChanged += combo_N1ST_ITM_GRP_CD_SelectedIndexChanged;


            this.btn_file.Click += btn_file_Click;
            this.btn_delete.Click += btn_delete_Click;

            this.btn_file1.Click += btn_file1_Click;
            this.btn_delete1.Click += btn_delete1_Click;



            this.orgDao = Dao;
            //
            SystemCodeVo copyDao = new SystemCodeVo()
            {
                ITM_CD = Dao.ITM_CD,
                ITM_NM = Dao.ITM_NM,
                PROD_LOC_CD = Dao.PROD_LOC_CD,
                PROD_LOC_NM = Dao.PROD_LOC_NM,
                PROD_PUR_DT = Dao.PROD_PUR_DT,
                N1ST_ITM_GRP_CD = Dao.N1ST_ITM_GRP_CD,
                N1ST_ITM_GRP_NM = Dao.N1ST_ITM_GRP_NM,
                N2ND_ITM_GRP_CD = Dao.N2ND_ITM_GRP_CD,
                N2ND_ITM_GRP_NM = Dao.N2ND_ITM_GRP_NM,
                ITM_GRP_CLSS_CD = Dao.ITM_GRP_CLSS_CD,
                ITM_GRP_CLSS_NM = Dao.ITM_GRP_CLSS_NM,
                DELT_FLG = Dao.DELT_FLG,
                IMG = Dao.IMG,
                ITM_SZ_NM = Dao.ITM_SZ_NM,
                UOM_CD = Dao.UOM_CD,
                UOM_NM = Dao.UOM_NM,
                UOM_WGT = Dao.UOM_WGT,
                LD_TM_VAL = Dao.LD_TM_VAL,
                LOT_VAL = Dao.LOT_VAL,
                ABC_CD = Dao.ABC_CD,
                ABC_NM = Dao.ABC_NM,
                ITM_INFO_A = Dao.ITM_INFO_A,
                ITM_INFO_B = Dao.ITM_INFO_B,
                ITM_INFO_C = Dao.ITM_INFO_C,
                ITM_INFO_D = Dao.ITM_INFO_D,
                ITM_INFO_E = Dao.ITM_INFO_E,
                ITM_INFO_F = Dao.ITM_INFO_F,
                ACT_INSP_CD = Dao.ACT_INSP_CD,
                QLTY_INSP_CD = Dao.QLTY_INSP_CD,
                QLTY_INSP_NM = Dao.QLTY_INSP_NM,
                ITM_ORD_CD = Dao.ITM_ORD_CD,
                SFSTK_QTY = Dao.SFSTK_QTY,
                CAR_ITM_NM = Dao.CAR_ITM_NM,
                ITM_PLSTE_NO = Dao.ITM_PLSTE_NO,
                CRE_USR_ID = Dao.CRE_USR_ID,
                UPD_USR_ID = Dao.UPD_USR_ID,
                CHNL_CD = Dao.CHNL_CD,
                //
                FMB_ITM_CD = Dao.FMB_ITM_CD,
                FMB_ITM_NM = Dao.FMB_ITM_NM,
                //
                ITM_PLSTE_FILE_NM = Dao.ITM_PLSTE_FILE_NM,
                ITM_PLSTE_FILE = Dao.ITM_PLSTE_FILE,
                //
                MIN_TOR_VAL = Dao.MIN_TOR_VAL,
                MAX_TOR_VAL = Dao.MAX_TOR_VAL,
                //
                INSP_ITM_GRP_CD = Dao.INSP_ITM_GRP_CD,
                INSP_ITM_GRP_NM = Dao.INSP_ITM_GRP_NM,

                ITM_STD_FILE = Dao.ITM_STD_FILE,
                ITM_STD_FILE_NM = Dao.ITM_STD_FILE_NM,
                STO_TM_VAL = Dao.STO_TM_VAL,
                CYC_TM_VAL = Dao.CYC_TM_VAL,
                TACT_TM_VAL = Dao.TACT_TM_VAL,
                PROD_RGST_DT = Dao.PROD_RGST_DT,
                MOQ_QTY = Dao.MOQ_QTY,
                MTRL_NM_NM = Dao.MTRL_NM_NM,
                MTRL_NM_CD = Dao.MTRL_NM_CD,
                N2ND_SUB_ITM_CD = Dao.N2ND_SUB_ITM_CD,
                N2ND_SUB_ITM_NM = Dao.N2ND_SUB_ITM_NM,
                ACT_INSP_FLG = Dao.ACT_INSP_FLG,
                UOM_RUN_WGT = Dao.UOM_RUN_WGT,
                PK_PER_QTY = Dao.PK_PER_QTY
                //
                //isCheckd = false

            };

            ////수정
            if (Dao.ITM_CD != null)
            {
                this.isEdit = true;
                this.text_ITM_CD.IsReadOnly = true;
                this.text_ITM_CD.Background = Brushes.DarkGray;
                
                if (Dao.IMG != null)
                {
                    BitmapImage biImg = new BitmapImage();
                    MemoryStream ms = new MemoryStream(Dao.IMG);
                    biImg.BeginInit();
                    biImg.StreamSource = ms;
                    biImg.EndInit();
                    this.text_Image.Source = biImg;
                }

                this.fileByte = copyDao.ITM_PLSTE_FILE;
                this.fileByte1 = copyDao.ITM_STD_FILE;
            }
            else
            {
                //추가
                this.isEdit = false;
                copyDao.DELT_FLG = "사용";
                //Dao.JOIN_CO_DT = System.DateTime.Now.ToString("yyyy-MM-dd");
            }
            this.configCode.DataContext = copyDao;
            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
            this.OKButton.Click += new RoutedEventHandler(OKButton_Click);
            this.CancelButton.Click += new RoutedEventHandler(CancelButton_Click);
        }

        void btn_delete_Click(object sender, RoutedEventArgs e)
        {
            this.text_ITM_PLSTE_FILE_NM.Text = "";
            this.fileByte = new byte[0];
        }

        void btn_file_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            //dlg.DefaultExt = ".xls";
            dlg.Filter = "Image Files (*.png, *.jpeg, *jpg)|*.png;*.jpeg;*.jpg";
            dlg.Title = "파일를 선택 해 주세요.";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                string filename = dlg.FileName;
                //this.text_Path.Text = filename;
                this.text_ITM_PLSTE_FILE_NM.Text = Path.GetFileName(filename);
                this.fileByte = FileToByteArray(filename);

                //5MB
                if (fileByte.Length > 5485760)
                {
                    WinUIMessageBox.Show("[파일 크기] 5MB를 초과 하였습니다", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                    this.text_ITM_PLSTE_FILE_NM.Text = "";
                    this.fileByte = new byte[0];
                    return;
                }
            }
        }


        void btn_delete1_Click(object sender, RoutedEventArgs e)
        {
            this.text_ITM_STD_FILE_NM.Text = "";
            this.fileByte1 = new byte[0];
        }

        void btn_file1_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            //dlg.DefaultExt = ".xls";
            dlg.Filter = "All Files|*.*";
            dlg.Title = "파일를 선택 해 주세요.";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                string filename = dlg.FileName;
                //this.text_Path.Text = filename;
                this.text_ITM_STD_FILE_NM.Text = Path.GetFileName(filename);
                this.fileByte1 = FileToByteArray(filename);

                //5MB
                if (fileByte1.Length > 5485760)
                {
                    WinUIMessageBox.Show("[파일 크기] 5MB를 초과 하였습니다", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                    this.text_ITM_STD_FILE_NM.Text = "";
                    this.fileByte1 = new byte[0];
                    return;
                }
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
                    //중복 검사
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s141", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { ITM_CD = this.text_ITM_CD.Text, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            IList<SystemCodeVo> daoList = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                            if (daoList.Count != 0)
                            {
                                WinUIMessageBox.Show("[코드 - 중복] 코드를 다시 입력 하십시오.", "[중복검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                                this.text_ITM_CD.IsTabStop = true;
                                this.text_ITM_CD.Focus();
                                return ;
                            }
                        }
                    }


                    //저장
                    this.updateVo = getDomain();//this.updateDao
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s141/i", new StringContent(JsonConvert.SerializeObject(this.updateVo), System.Text.Encoding.UTF8, "application/json")))
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
                            //WinUIMessageBox.Show("완료 되었습니다", title, MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }

                    //IMG
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s141/img/i", new StringContent(JsonConvert.SerializeObject(this.updateVo), System.Text.Encoding.UTF8, "application/json")))
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
                else
                {
                    this.updateVo = getDomain();

                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s141/u", new StringContent(JsonConvert.SerializeObject(this.updateVo), System.Text.Encoding.UTF8, "application/json")))
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
                            //WinUIMessageBox.Show("완료 되었습니다", title, MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }

                        //IMG 삭제
                        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s141/img/d", new StringContent(JsonConvert.SerializeObject(this.updateVo), System.Text.Encoding.UTF8, "application/json")))
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
                            //WinUIMessageBox.Show("완료 되었습니다", title, MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }

                    //IMG
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s141/img/i", new StringContent(JsonConvert.SerializeObject(this.updateVo), System.Text.Encoding.UTF8, "application/json")))
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


                //SystemCodeVo resultVo;
                //if (isEdit == false)
                //{

                //    updateVo = getDomain();
                //    resultVo = codeClient.InsertItemCode(getDomain());
                //    if (!resultVo.isSuccess)
                //    {
                //        //실패
                //        WinUIMessageBox.Show(resultVo.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
                //        return;
                //    }
                //    resultVo = codeClient.InsertItemImg(getDomain());
                //    if (!resultVo.isSuccess)
                //    {
                //        //실패
                //        WinUIMessageBox.Show(resultVo.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
                //        return;
                //    }
                //    //성공
                //    WinUIMessageBox.Show("완료 되었습니다", "[추가]품목 마스터 관리", MessageBoxButton.OK, MessageBoxImage.Information);
                //}
                //else
                //{
                //    updateVo = getDomain();
                //    resultVo = codeClient.UpdateItemCode(updateVo);
                //    if (!resultVo.isSuccess)
                //    {
                //        //실패
                //        WinUIMessageBox.Show(resultVo.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
                //        return;
                //    }
                //    codeClient.DeleteItemImg(updateVo);
                //    resultVo = codeClient.InsertItemImg(updateVo);
                //    if (!resultVo.isSuccess)
                //    {
                //        //실패
                //        WinUIMessageBox.Show(resultVo.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
                //        return;
                //    }
                //    //성공
                //    WinUIMessageBox.Show("완료 되었습니다", "[수정]품목 마스터 관리", MessageBoxButton.OK, MessageBoxImage.Information);

                //    this.orgDao.ITM_CD = updateVo.ITM_CD;
                //    this.orgDao.ITM_NM = updateVo.ITM_NM;
                //    this.orgDao.PROD_LOC_CD = updateVo.PROD_LOC_CD;
                //    this.orgDao.PROD_LOC_NM = updateVo.PROD_LOC_NM;
                //    this.orgDao.PROD_PUR_DT = updateVo.PROD_PUR_DT;
                //    this.orgDao.N1ST_ITM_GRP_CD = updateVo.N1ST_ITM_GRP_CD;
                //    this.orgDao.N1ST_ITM_GRP_NM = updateVo.N1ST_ITM_GRP_NM;
                //    this.orgDao.N2ND_ITM_GRP_CD = updateVo.N2ND_ITM_GRP_CD;
                //    this.orgDao.N2ND_ITM_GRP_NM = updateVo.N2ND_ITM_GRP_NM;
                //    this.orgDao.ITM_GRP_CLSS_CD = updateVo.ITM_GRP_CLSS_CD;
                //    this.orgDao.ITM_GRP_CLSS_NM = updateVo.ITM_GRP_CLSS_NM;
                //    //this.orgDao.DELT_FLG = updateVo.DELT_FLG;
                //    this.orgDao.DELT_FLG = (updateVo.DELT_FLG.Equals("N") ? "사용" : "미사용");
                //    if (updateVo.IMG.Length > 0)
                //    {
                //        this.orgDao.IMG = updateVo.IMG;
                //    }
                //    this.orgDao.ITM_SZ_NM = updateVo.ITM_SZ_NM;
                //    this.orgDao.UOM_CD = updateVo.UOM_CD;
                //    this.orgDao.UOM_NM = updateVo.UOM_NM;
                //    this.orgDao.UOM_WGT = updateVo.UOM_WGT;
                //    this.orgDao.LD_TM_VAL = updateVo.LD_TM_VAL;
                //    this.orgDao.LOT_VAL = updateVo.LOT_VAL;
                //    this.orgDao.ABC_CD = updateVo.ABC_CD;
                //    this.orgDao.ABC_NM = updateVo.ABC_NM;
                //    this.orgDao.ITM_INFO_A = updateVo.ITM_INFO_A;
                //    this.orgDao.ITM_INFO_B = updateVo.ITM_INFO_B;
                //    this.orgDao.ITM_INFO_C = updateVo.ITM_INFO_C;
                //    this.orgDao.ITM_INFO_D = updateVo.ITM_INFO_D;
                //    this.orgDao.ITM_INFO_E = updateVo.ITM_INFO_E;
                //    this.orgDao.ITM_INFO_F = updateVo.ITM_INFO_F;
                //    this.orgDao.ACT_INSP_CD = updateVo.ACT_INSP_CD;
                //    this.orgDao.QLTY_INSP_CD = updateVo.QLTY_INSP_CD;
                //    this.orgDao.QLTY_INSP_NM = updateVo.QLTY_INSP_NM;
                //    this.orgDao.ITM_ORD_CD = updateVo.ITM_ORD_CD;
                //    this.orgDao.SFSTK_QTY = updateVo.SFSTK_QTY;
                //    this.orgDao.CAR_ITM_NM = updateVo.CAR_ITM_NM;
                //    this.orgDao.ITM_PLSTE_NO = updateVo.ITM_PLSTE_NO;
                //    this.orgDao.CRE_USR_ID = updateVo.CRE_USR_ID;
                //    this.orgDao.UPD_USR_ID = updateVo.UPD_USR_ID;
                //    this.orgDao.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
                //    //
                //    this.orgDao.ITM_PLSTE_FILE_NM = updateVo.ITM_PLSTE_FILE_NM;
                //    if (updateVo.ITM_PLSTE_FILE.Length > 0)
                //    {
                //        this.orgDao.ITM_PLSTE_FILE = updateVo.ITM_PLSTE_FILE;
                //    }
                //    this.orgDao.MIN_TOR_VAL = updateVo.MIN_TOR_VAL;
                //    this.orgDao.MAX_TOR_VAL = updateVo.MAX_TOR_VAL;
                //    //
                //    this.orgDao.INSP_ITM_GRP_CD = updateVo.INSP_ITM_GRP_CD;
                //    this.orgDao.INSP_ITM_GRP_NM = updateVo.INSP_ITM_GRP_NM;
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
        public bool ValueCheckd()
        {
            if (string.IsNullOrEmpty(this.text_ITM_CD.Text))
            {
                WinUIMessageBox.Show("[물품 코드] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.text_ITM_CD.IsTabStop = true;
                this.text_ITM_CD.Focus();
                return false;
            }
            //else if (string.IsNullOrEmpty(this.text_ITM_NM.Text))
            //{
            //    WinUIMessageBox.Show("[물품 명] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.text_ITM_NM.IsTabStop = true;
            //    this.text_ITM_NM.Focus();
            //    return false;
            //}
            //else if (string.IsNullOrEmpty(this.combo_PROD_LOC_CD.Text))
            //{
            //    WinUIMessageBox.Show("[저장소 번호] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.combo_PROD_LOC_CD.IsTabStop = true;
            //    this.combo_PROD_LOC_CD.Focus();
            //    return false;
            //}
            else if (string.IsNullOrEmpty(this.combo_ITM_GRP_CLSS_CD.Text))
            {
                WinUIMessageBox.Show("[분류] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.combo_ITM_GRP_CLSS_CD.IsTabStop = true;
                this.combo_ITM_GRP_CLSS_CD.Focus();
                return false;
            }
            //else if (string.IsNullOrEmpty(this.combo_N1ST_ITM_GRP_CD.Text))
            //{
            //    WinUIMessageBox.Show("[대 그룹] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.combo_N1ST_ITM_GRP_CD.IsTabStop = true;
            //    this.combo_N1ST_ITM_GRP_CD.Focus();
            //    return false;
            //}
            //else if (string.IsNullOrEmpty(this.combo_N2ND_ITM_GRP_CD.Text))
            //{
            //    WinUIMessageBox.Show("[중 그룹] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.combo_N2ND_ITM_GRP_CD.IsTabStop = true;
            //    this.combo_N2ND_ITM_GRP_CD.Focus();
            //    return false;
            //}
            //else if (string.IsNullOrEmpty(this.combo_UOM_CD.Text))
            //{
            //    WinUIMessageBox.Show("[단위] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.combo_UOM_CD.IsTabStop = true;
            //    this.combo_UOM_CD.Focus();
            //    return false;
            //}
            //else if (string.IsNullOrEmpty(this.combo_ABC_CD.Text))
            //{
            //    WinUIMessageBox.Show("[ABC 등급] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.combo_ABC_CD.IsTabStop = true;
            //    this.combo_ABC_CD.Focus();
            //    return false;
            //}
            //else if (string.IsNullOrEmpty(this.combo_QLTY_INSP_CD.Text))
            //{
            //    WinUIMessageBox.Show("[품질 검사 유형] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.combo_QLTY_INSP_CD.IsTabStop = true;
            //    this.combo_QLTY_INSP_CD.Focus();
            //    return false;
            //}
            else if (((this.text_Image.EditValue ?? new byte[0]) as byte[]).Length > 2197152)
            {
                WinUIMessageBox.Show("이미지 파일 크기가 2Mbyte 초과 하였습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }
            else
            {
                //if (this.isEdit == false)
                //{
                //    //this.updateVo = getDomain();
                //    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s141", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { ITM_CD = this.text_ITM_CD.Text, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                //    {
                //        if (response.IsSuccessStatusCode)
                //        {
                //            IList<SystemCodeVo> daoList = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                //            if (daoList.Count != 0)
                //            {
                //                WinUIMessageBox.Show("[코드 - 중복] 코드를 다시 입력 하십시오.", "[중복검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                //                this.text_ITM_CD.IsTabStop = true;
                //                this.text_ITM_CD.Focus();
                //                return false;
                //            }
                //        }
                //    }
                //}
            }
            return true;
        }
        #endregion

        #region Functon (getDomain - CustomerCodeVo)
        private SystemCodeVo getDomain()
        {
            SystemCodeVo Dao = new SystemCodeVo();
            SystemCodeVo PROD_LOC_CDVo = this.combo_PROD_LOC_CD.SelectedItem as SystemCodeVo;
            SystemCodeVo ITM_GRP_CLSS_CDVo = this.combo_ITM_GRP_CLSS_CD.SelectedItem as SystemCodeVo;
            SystemCodeVo N1ST_ITM_GRP_CDVo = this.combo_N1ST_ITM_GRP_CD.SelectedItem as SystemCodeVo;
            SystemCodeVo N2ND_ITM_GRP_CDVo = this.combo_N2ND_ITM_GRP_CD.SelectedItem as SystemCodeVo;
            SystemCodeVo UOM_CDVo = this.combo_UOM_CD.SelectedItem as SystemCodeVo;
            SystemCodeVo ABC_CDVo = this.combo_ABC_CD.SelectedItem as SystemCodeVo;
            SystemCodeVo QLTY_INSP_CDVo = this.combo_QLTY_INSP_CD.SelectedItem as SystemCodeVo;
            SystemCodeVo INSP_ITM_GRP_CDVo = this.combo_INSP_ITM_GRP_CD.SelectedItem as SystemCodeVo;
            //SystemCodeVo MTRL_NM_CDVo = this.combo_MTRL_NM_CD.SelectedItem as SystemCodeVo;
            SaleVo MTRL_NM_CDVo = this.combo_MTRL_NM_CD.SelectedItem as SaleVo;
            SystemCodeVo N2ND_SUB_ITM_CDVo = this.combo_N2ND_SUB_ITM_CD.SelectedItem as SystemCodeVo;

            SystemCodeVo FMB_ITM_CDVo = this.combo_FMB_ITM_CD.SelectedItem as SystemCodeVo;
            //
            Dao.ITM_CD = this.text_ITM_CD.Text;
            Dao.ITM_NM = this.text_ITM_NM.Text;
            Dao.CAR_ITM_NM = this.text_CAR_ITM_NM.Text;
            if (PROD_LOC_CDVo != null)
            {
                Dao.PROD_LOC_CD = PROD_LOC_CDVo.CLSS_CD;
                Dao.PROD_LOC_NM = PROD_LOC_CDVo.CLSS_DESC;
            }
            Dao.ITM_SZ_NM = this.text_ITM_SZ_NM.Text;
            if (ITM_GRP_CLSS_CDVo != null)
            {
                Dao.ITM_GRP_CLSS_CD = ITM_GRP_CLSS_CDVo.CLSS_CD;
                Dao.ITM_GRP_CLSS_NM = ITM_GRP_CLSS_CDVo.CLSS_DESC;
            }
            if (N1ST_ITM_GRP_CDVo != null)
            {
                Dao.N1ST_ITM_GRP_CD = N1ST_ITM_GRP_CDVo.ITM_GRP_CD;
                Dao.N1ST_ITM_GRP_NM = N1ST_ITM_GRP_CDVo.ITM_GRP_NM;
            }
            if (N2ND_ITM_GRP_CDVo != null)
            {
                Dao.N2ND_ITM_GRP_CD = N2ND_ITM_GRP_CDVo.ITM_GRP_CD;
                Dao.N2ND_ITM_GRP_NM = N2ND_ITM_GRP_CDVo.ITM_GRP_NM;
            }
            if (UOM_CDVo != null)
            {
                Dao.UOM_CD = UOM_CDVo.CLSS_CD;
                Dao.UOM_NM = UOM_CDVo.CLSS_DESC;
            }
            Dao.UOM_WGT = this.text_UOM_WGT.Text;
            Dao.LD_TM_VAL = this.text_LD_TM_VAL.Text;
            Dao.LOT_VAL = this.text_LOT_VAL.Text;
            Dao.ITM_ORD_CD = this.text_ITM_ORD_CD.Text;
            Dao.SFSTK_QTY = this.text_SFSTK_QTY.Text;
            Dao.ITM_PLSTE_NO = this.text_ITM_PLSTE_NO.Text;
            if (ABC_CDVo != null)
            {
                Dao.ABC_CD = ABC_CDVo.CLSS_CD;
                Dao.ABC_NM = ABC_CDVo.CLSS_DESC;
            }
            Dao.ACT_INSP_CD = this.text_ACT_INSP_CD.Text;
            if (QLTY_INSP_CDVo != null)
            {
                Dao.QLTY_INSP_CD = QLTY_INSP_CDVo.CLSS_CD;
                Dao.QLTY_INSP_NM = QLTY_INSP_CDVo.CLSS_DESC;
            }

            Dao.IMG = (this.text_Image.EditValue ?? new byte[0]) as byte[];
            //Dao.IMG = ImageToByte(this.text_Image.Source);
            Dao.DELT_FLG = (this.combo_deltFlg.Text.Equals("사용") ? "N" : "Y");
            Dao.CRE_USR_ID = SystemProperties.USER;
            Dao.UPD_USR_ID = SystemProperties.USER;
            Dao.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;

            //
            Dao.ITM_PLSTE_FILE_NM = this.text_ITM_PLSTE_FILE_NM.Text;
            if (fileByte == null)
            {
                Dao.ITM_PLSTE_FILE = new byte[0];
            }
            else
            {
                Dao.ITM_PLSTE_FILE = this.fileByte;
            }

            Dao.ITM_STD_FILE_NM = this.text_ITM_STD_FILE_NM.Text;
            if (fileByte1 == null)
            {
                Dao.ITM_STD_FILE = new byte[0];
            }
            else
            {
                Dao.ITM_STD_FILE = this.fileByte1;
            }

            Dao.MIN_TOR_VAL = this.text_MIN_TOR_VAL.Text;
            Dao.MAX_TOR_VAL = this.text_MAX_TOR_VAL.Text;

            if (INSP_ITM_GRP_CDVo != null)
            {
                Dao.INSP_ITM_GRP_CD = INSP_ITM_GRP_CDVo.CLSS_CD;
                Dao.INSP_ITM_GRP_NM = INSP_ITM_GRP_CDVo.CLSS_DESC;
            }

            if (MTRL_NM_CDVo != null)
            {
                //Dao.MTRL_NM_CD = MTRL_NM_CDVo.CLSS_CD;
                Dao.MTRL_NM_CD = MTRL_NM_CDVo.CAR_TP_CD;
                //Dao.MTRL_NM_NM = MTRL_NM_CDVo.CLSS_DESC;
                Dao.MTRL_NM_NM = MTRL_NM_CDVo.CAR_TP_NM;
            }

            if (N2ND_SUB_ITM_CDVo != null)
            {
                Dao.N2ND_SUB_ITM_CD = N2ND_SUB_ITM_CDVo.CLSS_CD;
                Dao.N2ND_SUB_ITM_NM = N2ND_SUB_ITM_CDVo.CLSS_DESC;
            }

            Dao.STO_TM_VAL = this.text_STO_TM_VAL.Text;
            Dao.CYC_TM_VAL = this.text_CYC_TM_VAL.Text;
            Dao.TACT_TM_VAL = this.text_TACT_TM_VAL.Text;
            Dao.PROD_RGST_DT = this.text_PROD_RGST_DT.Text;
            Dao.MOQ_QTY = this.text_MOQ_QTY.Text;

            Dao.UOM_RUN_WGT = this.text_UOM_RUN_WGT.Text;
            Dao.PK_PER_QTY = this.text_PK_PER_QTY.Text;

            Dao.ACT_INSP_FLG = this.combo_ACT_INSP_FLG.Text;

            if (FMB_ITM_CDVo != null)
            {
                Dao.FMB_ITM_CD = FMB_ITM_CDVo.ITM_CD;
                Dao.FMB_ITM_NM = FMB_ITM_CDVo.ITM_NM;
            }

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


        async void combo_ITM_GRP_CLSS_CD_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                SystemCodeVo ITM_GRP_CLSS_NM_VO = this.combo_ITM_GRP_CLSS_CD.SelectedItem as SystemCodeVo;
                if (ITM_GRP_CLSS_NM_VO != null)
                {
                    if (string.IsNullOrEmpty(ITM_GRP_CLSS_NM_VO.CLSS_CD))
                    {
                        return;
                    }

                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s133", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { DELT_FLG = "N", ITM_GRP_CLSS_CD = ITM_GRP_CLSS_NM_VO.CLSS_CD, CRE_USR_ID = "", CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            this.combo_N1ST_ITM_GRP_CD.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                        }
                    }

                    //IList<SystemCodeVo> ItemGroupVo = codeClient.SelectCodeItemGroupList(new SystemCodeVo() { DELT_FLG = "N", ITM_GRP_CLSS_CD = ITM_GRP_CLSS_NM_VO.CLSS_CD, CRE_USR_ID = "", CHNL_CD = SystemProperties.USER_VO.CHNL_CD });
                    //IList<CodeDao> ItemList = new List<CodeDao>();
                    //int nCnt = ItemGroupVo.Count;
                    //SystemCodeVo tmpVo;

                    //this.combo_N1ST_ITM_GRP_CD.Clear();

                    //for (int x = 0; x < nCnt; x++)
                    //{
                    //    tmpVo = ItemGroupVo[x];
                    //    ItemList.Add(new CodeDao() { CLSS_DESC = tmpVo.ITM_GRP_NM, CLSS_CD = tmpVo.ITM_GRP_CD });
                    //}
                    //this.combo_N1ST_ITM_GRP_CD.ItemsSource = ItemList;
                }
            }
            catch (Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        async void combo_N1ST_ITM_GRP_CD_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                SystemCodeVo ITM_GRP_CLSS_NM_VO = this.combo_ITM_GRP_CLSS_CD.SelectedItem as SystemCodeVo;
                if (ITM_GRP_CLSS_NM_VO != null)
                {
                    SystemCodeVo ITM_N1ST_TP_NM_VO = this.combo_N1ST_ITM_GRP_CD.SelectedItem as SystemCodeVo;
                    if (ITM_N1ST_TP_NM_VO != null)
                    {
                        if (string.IsNullOrEmpty(ITM_N1ST_TP_NM_VO.ITM_GRP_CD))
                        {
                            return;
                        }

                        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s133", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { PRNT_ITM_GRP_CD = "X", N1ST_ITM_GRP_CD = ITM_N1ST_TP_NM_VO.ITM_GRP_CD, DELT_FLG = "N", ITM_GRP_CLSS_CD = ITM_GRP_CLSS_NM_VO.CLSS_CD, CRE_USR_ID = "", CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                this.combo_N2ND_ITM_GRP_CD.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                            }
                        }
                        //IList<SystemCodeVo> ItemGroupVo = codeClient.SelectCodeItemGroupList(new SystemCodeVo() { PRNT_ITM_GRP_CD = "X", N1ST_ITM_GRP_CD = ITM_N1ST_TP_NM_VO.CLSS_CD, DELT_FLG = "N", ITM_GRP_CLSS_CD = ITM_GRP_CLSS_NM_VO.CLSS_CD, CRE_USR_ID = "", CHNL_CD = SystemProperties.USER_VO.CHNL_CD });
                        //IList<CodeDao> ItemList = new List<CodeDao>();
                        //int nCnt = ItemGroupVo.Count;
                        //SystemCodeVo tmpVo;

                        //this.combo_N2ND_ITM_GRP_CD.Clear();
                        //for (int x = 0; x < nCnt; x++)
                        //{
                        //    tmpVo = ItemGroupVo[x];
                        //    ItemList.Add(new CodeDao() { CLSS_DESC = tmpVo.ITM_GRP_NM, CLSS_CD = tmpVo.ITM_GRP_CD });
                        //}
                        //this.combo_N2ND_ITM_GRP_CD.ItemsSource = ItemList;
                    }
                }
            }
            catch (Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        public SystemCodeVo resultDao
        {
            get
            {
                return this.updateVo;
            }
        }


        //private byte[] ImageToByte(ImageSource img)
        //{
        //    try
        //    {

        //        if (img == null)
        //        {
        //            //WinUIMessageBox.Show("이미지 등록을 권장 합니다.", "[유효검사]장비 등록", MessageBoxButton.OK, MessageBoxImage.Information);
        //            return new byte[0];
        //        }
        //        BitmapImage biImg = img as BitmapImage;
        //        Stream stream = biImg.StreamSource;
        //        return stream.GetDataFromStream();
        //    }
        //    catch
        //    {
        //        return new byte[0];
        //    }
        //}

        public byte[] FileToByteArray(string filePath)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                int length = Convert.ToInt32(fs.Length);
                BinaryReader br = new BinaryReader(fs);
                byte[] buff = br.ReadBytes(length);
                fs.Close();

                return buff;
            }
        }


        public async void SYSTEM_CODE_VO()
        {
            //this.combo_PROD_LOC_CD.ItemsSource = SystemProperties.SYSTEM_CODE_VO("L-100");
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "L-100"))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.combo_PROD_LOC_CD.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                }
            }

            //this.combo_PROD_LOC_CD.ItemsSource = SystemProperties.SYSTEM_CODE_VO("L-100");
            //using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "DH02"))
            //{
            //    if (response.IsSuccessStatusCode)
            //    {
            //        this.combo_MTRL_NM_CD.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
            //    }
            //}
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s11410", new StringContent(JsonConvert.SerializeObject(new SaleVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD, DELT_FLG = "N" }), System.Text.Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.combo_MTRL_NM_CD.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SaleVo>>(await response.Content.ReadAsStringAsync()).Cast<SaleVo>().ToList();
                }
            }

            //this.combo_ITM_GRP_CLSS_CD.ItemsSource = SystemProperties.SYSTEM_CODE_VO("L-001");
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "L-001"))
            {
                if (response.IsSuccessStatusCode)
                {
                    //중분류
                    this.combo_ITM_GRP_CLSS_CD.SelectedIndexChanged += combo_ITM_GRP_CLSS_CD_SelectedIndexChanged;

                    //대분류
                    this.combo_N1ST_ITM_GRP_CD.SelectedIndexChanged += combo_N1ST_ITM_GRP_CD_SelectedIndexChanged;

                    //분류
                    this.combo_ITM_GRP_CLSS_CD.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();

                    //N1ST_ITM_GRP_CD = this.orgDao.N1ST_ITM_GRP_CD;
                    //N1ST_ITM_GRP_NM = this.orgDao.N1ST_ITM_GRP_NM;
                    //N2ND_ITM_GRP_CD = this.orgDao.N2ND_ITM_GRP_CD;
                    //N2ND_ITM_GRP_NM = this.orgDao.N2ND_ITM_GRP_NM;
                    //ITM_GRP_CLSS_CD = this.orgDao.ITM_GRP_CLSS_CD;

                    this.combo_ITM_GRP_CLSS_CD.Text = this.orgDao.ITM_GRP_CLSS_NM;
                    //combo_ITM_GRP_CLSS_CD_SelectedIndexChanged(null, null);
                    SystemCodeVo ITM_GRP_CLSS_NM_VO = this.combo_ITM_GRP_CLSS_CD.SelectedItem as SystemCodeVo;
                    if (ITM_GRP_CLSS_NM_VO != null)
                    {
                        if (string.IsNullOrEmpty(ITM_GRP_CLSS_NM_VO.CLSS_CD))
                        {
                            return;
                        }

                        using (HttpResponseMessage response_1 = await SystemProperties.PROGRAM_HTTP.PostAsync("s133", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { DELT_FLG = "N", ITM_GRP_CLSS_CD = ITM_GRP_CLSS_NM_VO.CLSS_CD, CRE_USR_ID = "", CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                        {
                            if (response_1.IsSuccessStatusCode)
                            {
                                this.combo_N1ST_ITM_GRP_CD.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response_1.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                            }
                        }
                    }

                    if(this.combo_N1ST_ITM_GRP_CD.ItemsSource != null)
                    {
                        this.combo_N1ST_ITM_GRP_CD.Text = this.orgDao.N1ST_ITM_GRP_NM;
                        this.combo_N1ST_ITM_GRP_CD.SelectedItem = (this.combo_N1ST_ITM_GRP_CD.ItemsSource as IEnumerable<SystemCodeVo>).Where(x => x.ITM_GRP_CD.Equals(this.orgDao.N1ST_ITM_GRP_CD)).FirstOrDefault<SystemCodeVo>();
                    }


                    //combo_N1ST_ITM_GRP_CD_SelectedIndexChanged(null, null);
                    SystemCodeVo ITM_N1ST_TP_NM_VO = this.combo_N1ST_ITM_GRP_CD.SelectedItem as SystemCodeVo;
                    if (ITM_N1ST_TP_NM_VO != null)
                    {
                        if (string.IsNullOrEmpty(ITM_N1ST_TP_NM_VO.ITM_GRP_CD))
                        {
                            return;
                        }

                        using (HttpResponseMessage response_2 = await SystemProperties.PROGRAM_HTTP.PostAsync("s133", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { PRNT_ITM_GRP_CD = "X", N1ST_ITM_GRP_CD = ITM_N1ST_TP_NM_VO.ITM_GRP_CD, DELT_FLG = "N", ITM_GRP_CLSS_CD = ITM_GRP_CLSS_NM_VO.CLSS_CD, CRE_USR_ID = "", CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                        {
                            if (response_2.IsSuccessStatusCode)
                            {
                                this.combo_N2ND_ITM_GRP_CD.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response_2.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                            }
                        }
                    }

                    if (this.combo_N2ND_ITM_GRP_CD.ItemsSource != null)
                    {
                        this.combo_N2ND_ITM_GRP_CD.Text = this.orgDao.N2ND_ITM_GRP_NM;
                        this.combo_N2ND_ITM_GRP_CD.SelectedItem = (this.combo_N2ND_ITM_GRP_CD.ItemsSource as IEnumerable<SystemCodeVo>).Where(x => x.ITM_GRP_CD.Equals(this.orgDao.N2ND_ITM_GRP_CD)).FirstOrDefault<SystemCodeVo>();
                    }
                }
            }

            //this.combo_UOM_CD.ItemsSource = SystemProperties.SYSTEM_CODE_VO("L-003");
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "L-003"))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.combo_UOM_CD.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                }
            }

            //this.combo_QLTY_INSP_CD.ItemsSource = SystemProperties.SYSTEM_CODE_VO("L-004");
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "L-004"))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.combo_QLTY_INSP_CD.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                }
            }

            //this.combo_ABC_CD.ItemsSource = SystemProperties.SYSTEM_CODE_VO("L-200");
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "L-200"))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.combo_ABC_CD.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                }
            }

            //this.combo_INSP_ITM_GRP_CD.ItemsSource = SystemProperties.SYSTEM_CODE_VO("Q-005");
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "Q-005"))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.combo_INSP_ITM_GRP_CD.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                }
            }

            //this.combo_N2ND_SUB_ITM_CD.ItemsSource = SystemProperties.SYSTEM_CODE_VO("L-800");
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "L-800"))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.combo_N2ND_SUB_ITM_CD.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                }
            }

            ////품명
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s141/fmb", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { DELT_FLG = "N", CHNL_CD = SystemProperties.USER_VO.CHNL_CD, ITM_GRP_CLSS_CD = "M" }), System.Text.Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.combo_FMB_ITM_CD.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                }
            }

        }

        //private void OnApplyEffectButtonClick(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        OpenFileDialog dlg = new OpenFileDialog();
        //        dlg.Title = "이미지 파일";
        //        //dlg.InitialDirectory = "c:\\";
        //        //dlg.Filter = "Image files (*.jpg; *.jpeg; *.gif; *.bmp; *.png)|*.jpg;*.jpeg;*.gif;*.bmp;*.png";
        //        //dlg.RestoreDirectory = true;
        //        dlg.FileName = "";
        //        dlg.Filter = "Image File|*.png";
        //        //ofd.Filter = "Image File|*.jpg;*jpeg;*.png;*.gif;*.bmp";

        //        if (dlg.ShowDialog() == true)
        //        {
        //            byte[] fileBytes = null;
        //            string selectedFileName = dlg.FileName;
        //            //bitmap.BeginInit();
        //            using (FileStream fileStream = new FileStream(selectedFileName, FileMode.Open))
        //            {
        //                fileBytes = new byte[fileStream.Length];
        //                fileStream.Read(fileBytes, 0, fileBytes.Length);
                        
        //                BitmapImage bitmap = new BitmapImage();
        //                MemoryStream ms = new MemoryStream(fileBytes);
        //                bitmap.BeginInit();
        //                bitmap.StreamSource = ms;
        //                bitmap.EndInit();

        //                this.text_Image.Source = bitmap;
        //                //this.Image_Clear.
        //            }
        //            //bitmapSource = GetBitmapSource(fileStream);
        //            //bitmap.StreamSource = 
        //            //Stream stream = biImg.StreamSource;
        //            //bitmap.UriSource = new Uri(selectedFileName);
        //            //bitmap.EndInit();
        //        }
        //        //this.text_Image.
        //        //WinUIMessageBox.Show("1234", "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //    catch (Exception eLog)
        //    {
        //        WinUIMessageBox.Show("[이미지 파일만 가능합니다]" + Environment.NewLine + eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
        //        return;
        //    }
        //}

        //private void OnClearEffectButtonClick(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        this.text_Image.Source = null;
        //    }
        //    catch (Exception eLog)
        //    {
        //        WinUIMessageBox.Show("이미지 파일만 가능합니다" + Environment.NewLine + eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
        //        return;
        //    }
        //}
    }
}
