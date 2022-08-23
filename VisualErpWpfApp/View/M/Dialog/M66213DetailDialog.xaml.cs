using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Code;
using ModelsLibrary.Man;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;
using AquilaErpWpfApp3.Util;

namespace AquilaErpWpfApp3.View.M.Dialog
{
    public partial class M66213DetailDialog : DXWindow
    {
        private string _title = "(PDM)재공품번생성 및 제품데이터관리";

        //private static ManServiceClient manClient = SystemProperties.ManClient;
        private ManVo orgDao;
        //private bool isEdit = false;
        //private ManVo updateDao;
        private IList<ManVo> totalItem;
        //private M6627ExcelDialog excelDialog;

        //private byte[] CERTI_IMG = new byte[0];


        public M66213DetailDialog(ManVo Dao)
        {
            InitializeComponent();

            SYSTEM_CODE_VO();
            //this.combo_CERTI_USR_ID.ItemsSource = SystemProperties.USER_CODE_VO();
            //
            //
            this.orgDao = Dao;
            ManVo copyDao = new ManVo()
            {
                //CERTI_USR_ID = Dao.CERTI_USR_ID,
                //CERTI_USR_NM = Dao.CERTI_USR_NM,
                //CERTI_PSN_RMK = Dao.CERTI_PSN_RMK,
                //CERTI_NO = Dao.CERTI_NO,
                //CERTI_GRD_CD = Dao.CERTI_GRD_CD,
                //CERTI_DT = Dao.CERTI_DT,
                //CERTI_NXT_DT = Dao.CERTI_NXT_DT,
                //CERTI_YR = Dao.CERTI_YR,
                //CERTI_RMK = Dao.CERTI_RMK,
                ITM_CD = Dao.ITM_CD,
                ITM_NM = Dao.ITM_NM,
                CHNL_CD = Dao.CHNL_CD,
                CRE_USR_ID = Dao.CRE_USR_ID,
                UPD_USR_ID = Dao.UPD_USR_ID
            };

            this.btn_add.Click += btn_add_Click;
            this.btn_del.Click += btn_del_Click;
            this.btn_reset.Click += btn_reset_Click;


            //////수정
            ////if (copyDao.CERTI_NO != null)
            ////{
            ////    //this.text_CERTI_NO.IsReadOnly = true;
            ////    //성명

            ////    this.isEdit = true;
            ////    //this.CERTI_IMG = (copyDao.CERTI_IMG == null ? new byte[0] : copyDao.CERTI_IMG);
            ////    //
            ////    //copyDao.EQ_PUR_YRMON = DateTime.ParseExact(Dao.EQ_PUR_YRMON + "01", "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None).ToString("yyyy-MM-dd");
            ////}
            ////else
            ////{

            ////    //this.CERTI_IMG = new byte[0];
            ////    //copyDao.CERTI_YR = "0";
            ////    ////추가
            ////    ////this.text_ClssTpCd.IsReadOnly = false;
            ////    ////this.isEdit = false;
            ////    //copyDao.CERTI_DT = System.DateTime.Now.ToString("yyyy-MM-dd");
            ////    //copyDao.CERTI_NXT_DT = System.DateTime.Now.ToString("yyyy-MM-dd");
            ////    //copyDao.EQ_QTY = 0;
            ////    //copyDao.EQ_PUR_AMT = 0;
            ////}
            ///
            Search();

            this.configCode.DataContext = copyDao;
            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);

            //this.EXCELButton.Click += new RoutedEventHandler(EXCELButton_Click);

            this.OKButton.Click += new RoutedEventHandler(OKButton_Click);
            this.CancelButton.Click += new RoutedEventHandler(CancelButton_Click);
        }


        private async void Search()
        {
            using (HttpResponseMessage responseX = await SystemProperties.PROGRAM_HTTP.PostAsync("m66213/popup/list", new StringContent(JsonConvert.SerializeObject(new ManVo() { ITM_CD = orgDao.ITM_CD, CHNL_CD = SystemProperties.USER_VO.CHNL_CD, CRE_USR_ID = SystemProperties.USER, UPD_USR_ID = SystemProperties.USER }), System.Text.Encoding.UTF8, "application/json")))
            {
                if (responseX.IsSuccessStatusCode)
                {
                    this.totalItem = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await responseX.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
                    this.ConfigViewPage1Edit_Master.ItemsSource = this.totalItem;
                }
                //SelectedMasterViewList = manClient.M6611SelectMaster(new ManVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD });
            }
        }

        void btn_reset_Click(object sender, RoutedEventArgs e)
        {
            Search();
        }

        void btn_del_Click(object sender, RoutedEventArgs e)
        {
            ManVo masterDomain = (ManVo)ConfigViewPage1Edit_Master.SelectedItem;
            if (masterDomain == null)
            {
                return;
            }
            //this.totalItem = (ObservableCollection<CustomerCodeVo>)this.ConfigViewPage1Edit_Master.ItemsSource;
            this.totalItem.Remove(masterDomain);
            //
            this.ConfigViewPage1Edit_Master.ItemsSource = this.totalItem;
            this.ConfigViewPage1Edit_Master.RefreshData();
        }

        void btn_add_Click(object sender, RoutedEventArgs e)
        {
            //this.totalItem = (ObservableCollection<CustomerCodeVo>)this.ConfigViewPage1Edit_Master.ItemsSource;
            this.totalItem.Insert(this.totalItem.Count, new ManVo() { MTRL_ITM_SEQ = this.totalItem.Count +1, ITM_CD = orgDao.ITM_CD, ITM_NM = orgDao.ITM_NM, MTRL_ITM_CD = "", MTRL_ITM_NM = "", USE_QTY = 0.00, TOL_QTY=0.00, CRE_USR_ID = SystemProperties.USER, UPD_USR_ID = SystemProperties.USER, CHNL_CD = SystemProperties.USER_VO.CHNL_CD });            
            this.configViewPage1EditView_Master.FocusedRowHandle = this.totalItem.Count - 1;
            this.ConfigViewPage1Edit_Master.RefreshData();
        }

        //void EXCELButton_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        //임시 폴더
        //        string tempFolderPath = System.IO.Path.GetTempPath();
        //        //
        //        string makeDir = tempFolderPath + System.DateTime.Now.ToString("yyyyMMddHHmmss.xlsx");

        //        File.WriteAllBytes(makeDir, (this.CERTI_IMG == null ? new byte[0] : this.CERTI_IMG));

        //        //int ArraySize = (this.orgDao.CERTI_IMG == null ? 0 : this.orgDao.CERTI_IMG.Length);
        //        //FileStream fs = new FileStream(makeDir, FileMode.OpenOrCreate, FileAccess.Write);
        //        //fs.Write(this.orgDao.CERTI_IMG, 0, ArraySize);
        //        //fs.Close();

        //        excelDialog = new M6627ExcelDialog(makeDir);
        //        excelDialog.Title = _title + " - 엑셀";
        //        excelDialog.Owner = Application.Current.MainWindow;
        //        excelDialog.BorderEffect = BorderEffect.Default;
        //        excelDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
        //        excelDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
        //        bool isDialog = (bool)excelDialog.ShowDialog();
        //        if (isDialog)
        //        {
        //            this.CERTI_IMG = excelDialog.IMAGE;
        //            //파일 삭제
        //            File.Delete(makeDir);
        //        }
        //    }
        //    catch (Exception eLog)
        //    {
        //        WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error);
        //        return;
        //    }
        //}

        #region Functon (OKButton_Click, CancelButton_Click)
        private async void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValueCheckd())
            {
                int _Num = 0;
                ////ProgramVo resultVo;
                //if (isEdit == false)
                //{
                //    this.updateDao = getDomain();//this.updateDao
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m66213/dtl/d", new StringContent(JsonConvert.SerializeObject(new ManVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD, ITM_CD = this.orgDao.ITM_CD }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string resultMsg = await response.Content.ReadAsStringAsync();
                        if (int.TryParse(resultMsg, out _Num) == false)
                        {
                            //실패
                            WinUIMessageBox.Show(resultMsg, _title, MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                    }
                }

                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m66213/dtl/i", new StringContent(JsonConvert.SerializeObject(this.totalItem), System.Text.Encoding.UTF8, "application/json")))
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
                //}
                //else
                //{
                //    this.updateDao = getDomain();

                //    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6627/u", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
                //    {
                //        if (response.IsSuccessStatusCode)
                //        {
                //            string result = await response.Content.ReadAsStringAsync();
                //            if (int.TryParse(result, out _Num) == false)
                //            {
                //                //실패
                //                WinUIMessageBox.Show(result, _title, MessageBoxButton.OK, MessageBoxImage.Error);
                //                return;
                //            }

                //            //성공
                //            WinUIMessageBox.Show("완료 되었습니다", _title, MessageBoxButton.OK, MessageBoxImage.Information);
                //        }
                //    }
                //}
                //ManVo resultVo;
                //if (isEdit == false)
                //{
                //    this.updateDao = getDomain();//this.updateDao
                //    resultVo = manClient.M6627InsertMaster(this.updateDao);
                //    if (!resultVo.isSuccess)
                //    {
                //        //실패
                //        WinUIMessageBox.Show(resultVo.Message, "[" + SystemProperties.PROGRAM_TITLE + "]인원 자격 관리", MessageBoxButton.OK, MessageBoxImage.Error);
                //        return;
                //    }
                //    //성공
                //    WinUIMessageBox.Show("완료 되었습니다", "[추가]인원 자격 관리", MessageBoxButton.OK, MessageBoxImage.Information);
                //}
                //else
                //{
                //    this.updateDao = getDomain();
                //    resultVo = manClient.M6627UpdateMaster(this.updateDao);
                //    if (!resultVo.isSuccess)
                //    {
                //        //실패
                //        WinUIMessageBox.Show(resultVo.Message, "[" + SystemProperties.PROGRAM_TITLE + "]인원 자격 관리", MessageBoxButton.OK, MessageBoxImage.Error);
                //        return;
                //    }
                //    //성공
                //    WinUIMessageBox.Show("완료 되었습니다", "[수정]인원 자격 관리", MessageBoxButton.OK, MessageBoxImage.Information);
                //    //
                //    this.orgDao.CERTI_USR_ID = this.updateDao.CERTI_USR_ID;
                //    this.orgDao.CERTI_USR_NM = this.updateDao.CERTI_USR_NM;
                //    this.orgDao.CERTI_PSN_RMK = this.updateDao.CERTI_PSN_RMK;
                //    this.orgDao.CERTI_NO = this.updateDao.CERTI_NO;
                //    this.orgDao.CERTI_GRD_CD = this.updateDao.CERTI_GRD_CD;
                //    this.orgDao.CERTI_DT = this.updateDao.CERTI_DT;
                //    this.orgDao.CERTI_NXT_DT = this.updateDao.CERTI_NXT_DT;
                //    this.orgDao.CERTI_YR = this.updateDao.CERTI_YR;
                //    this.orgDao.CERTI_RMK = this.updateDao.CERTI_RMK;
                //    this.orgDao.CRE_USR_ID = this.updateDao.CRE_USR_ID;
                //    this.orgDao.UPD_USR_ID = this.updateDao.UPD_USR_ID;
                //    this.orgDao.CHNL_CD = this.updateDao.CHNL_CD;

                //    this.orgDao.CERTI_IMG = this.updateDao.CERTI_IMG;
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
            //if (string.IsNullOrEmpty(this.text_CERTI_NO.Text))
            //{
            //    WinUIMessageBox.Show("[인증번호] 입력 값이 맞지 않습니다.", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.text_CERTI_NO.IsTabStop = true;
            //    this.text_CERTI_NO.Focus();
            //    return false;
            //}
            //else if (string.IsNullOrEmpty(this.combo_CERTI_USR_ID.Text))
            //{
            //    WinUIMessageBox.Show("[사번] 입력 값이 맞지 않습니다.", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.combo_CERTI_USR_ID.IsTabStop = true;
            //    this.combo_CERTI_USR_ID.Focus();
            //    return false;
            //}
            //else if (string.IsNullOrEmpty(this.text_CERTI_PSN_RMK.Text))
            //{
            //    WinUIMessageBox.Show("[해당분야] 입력 값이 맞지 않습니다.", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.text_CERTI_PSN_RMK.IsTabStop = true;
            //    this.text_CERTI_PSN_RMK.Focus();
            //    return false;
            //}

            
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
                //if (this.isEdit == false)
                //{
                //    UserCodeDao CERTI_USR_ID_VO = this.combo_CERTI_USR_ID.SelectedItem as UserCodeDao;
                //    ManVo dao = new ManVo()
                //    {
                //        CERTI_NO = this.text_CERTI_NO.Text,
                //        CHNL_CD = SystemProperties.USER_VO.CHNL_CD,
                //        CERTI_USR_NM = CERTI_USR_ID_VO.USR_N1ST_NM,
                //        CERTI_USR_ID = CERTI_USR_ID_VO.USR_ID
                //    };
                //    IList<ManVo> daoList = (IList<ManVo>)manClient.M6627SelectMaster(dao);
                //    if (daoList.Count != 0)
                //    {
                //        WinUIMessageBox.Show("[코드 - 중복] 코드를 다시 입력 하십시오.", "[중복검사]인원 자격 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
                //        this.text_CERTI_NO.IsTabStop = true;
                //        this.text_CERTI_NO.Focus();
                //        return false;
                //    }
                //}
            //}
            return true;
        }
        #endregion

        #region Functon (getDomain - ConfigView1Dao)
        private ManVo getDomain()
        {
            ManVo Dao = new ManVo();

            ////Dao.CERTI_USR_ID = this.text_CERTI_USR_ID.Text;
            //GroupUserVo CERTI_USR_ID_VO = this.combo_CERTI_USR_ID.SelectedItem as GroupUserVo;
            //if (CERTI_USR_ID_VO != null)
            //{
            //    Dao.CERTI_USR_NM = CERTI_USR_ID_VO.USR_N1ST_NM;
            //    Dao.CERTI_USR_ID = CERTI_USR_ID_VO.USR_ID;
            //}

            ////Dao.INSRT_SZ = this.text_INSRT_SZ.Text;
            ////Dao.CERTI_USR_NM = this.text_CERTI_USR_NM.Text;
            //Dao.CERTI_PSN_RMK = this.text_CERTI_PSN_RMK.Text;
            //Dao.CERTI_NO = this.text_CERTI_NO.Text;

            //Dao.CERTI_GRD_CD = this.text_CERTI_GRD_CD.Text;

            //Dao.CERTI_YR = this.text_CERTI_YR.Text;
            //Dao.CERTI_RMK = this.text_CERTI_RMK.Text;

            //Dao.CRE_USR_ID = SystemProperties.USER;
            //Dao.UPD_USR_ID = SystemProperties.USER;
            //Dao.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;

            //Dao.CERTI_DT = Convert.ToDateTime(this.text_CERTI_DT.Text).ToString("yyyy-MM-dd");
            //Dao.CERTI_NXT_DT = Convert.ToDateTime(this.text_CERTI_NXT_DT.Text).ToString("yyyy-MM-dd");

            //Dao.CERTI_IMG = this.CERTI_IMG;//new byte[0];

            return Dao;
        }
        #endregion

        public async void SYSTEM_CODE_VO()
        {

            using (HttpResponseMessage responseX = await SystemProperties.PROGRAM_HTTP.PostAsync("s141", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD, ITM_GRP_CLSS_CD = "M" /*N1ST_ITM_GRP_CD = SelectedMasterItem.N1ST_ITM_GRP_CD*/ }), System.Text.Encoding.UTF8, "application/json")))
            {
                if (responseX.IsSuccessStatusCode)
                {
                    this.lue_MTRL_ITM_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await responseX.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                }
            }

            //this.combo_CERTI_USR_ID.ItemsSource = SystemProperties.USER_CODE_VO();
            //using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s136/usr", new StringContent(JsonConvert.SerializeObject(new GroupUserVo() { DELT_FLG = "N", CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
            //{
            //    if (response.IsSuccessStatusCode)
            //    {
            //        this.lue_MTRL_ITM_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<GroupUserVo>>(await response.Content.ReadAsStringAsync()).Cast<GroupUserVo>().ToList();
            //    }
            //}
        }


        private void viewPage1EditView_HiddenEditor(object sender, DevExpress.Xpf.Grid.EditorEventArgs e)
        {
            this.configViewPage1EditView_Master.CommitEditing();
        }

        private async void GridColumn_Validate(object sender, DevExpress.Xpf.Grid.GridCellValidationEventArgs e)
        {
            try
            {
                ManVo masterDomain = (ManVo)ConfigViewPage1Edit_Master.GetFocusedRow();

                bool mtrlItmSeq = (e.Column.FieldName.ToString().Equals("MTRL_ITM_SEQ") ? true : false);

                bool mtrltmNm = (e.Column.FieldName.ToString().Equals("MTRL_ITM_NM") ? true : false);
                bool useQty = (e.Column.FieldName.ToString().Equals("USE_QTY") ? true : false);
                bool tolQty = (e.Column.FieldName.ToString().Equals("TOL_QTY") ? true : false);

                int _Num;
                if (mtrltmNm)
                {
                    if (e.IsValid)
                    {
                        if (string.IsNullOrEmpty(masterDomain.MTRL_ITM_NM + ""))
                        {
                            masterDomain.MTRL_ITM_NM = "";
                            masterDomain.MTRL_ITM_CD = "";
                        }
                        //
                        if (!masterDomain.MTRL_ITM_NM.Equals((e.Value == null ? "" : e.Value.ToString())))
                        {
                            SystemCodeVo bankIoDao = this.lue_MTRL_ITM_NM.GetItemFromValue(e.Value) as SystemCodeVo;
                            //
                            if (bankIoDao != null)
                            {
                                masterDomain.MTRL_ITM_CD = bankIoDao.ITM_CD;
                                masterDomain.MTRL_ITM_NM = bankIoDao.ITM_NM;
                            }

                            masterDomain.CRE_USR_ID = SystemProperties.USER;
                            masterDomain.UPD_USR_ID = SystemProperties.USER;
                            //
                            //using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m66212/mst/i", new StringContent(JsonConvert.SerializeObject(masterDomain), System.Text.Encoding.UTF8, "application/json")))
                            //{
                            //    if (response.IsSuccessStatusCode)
                            //    {
                            //        string result = await response.Content.ReadAsStringAsync();
                            //        if (int.TryParse(result, out _Num) == false)
                            //        {
                            //            //실패
                            //            //WinUIMessageBox.Show(result, _title, MessageBoxButton.OK, MessageBoxImage.Error);
                            //            return;
                            //        }
                            //    }
                            //}
                        }
                    }
                }
                else if (useQty)
                {
                    if (e.IsValid)
                    {
                        if (string.IsNullOrEmpty(masterDomain.USE_QTY + ""))
                        {
                            masterDomain.USE_QTY = 0;
                        }
                        //
                        if (!masterDomain.USE_QTY.Equals((e.Value == null ? "" : e.Value.ToString())))
                        {
                            masterDomain.USE_QTY = e.Value;

                        }

                        masterDomain.CRE_USR_ID = SystemProperties.USER;
                        masterDomain.UPD_USR_ID = SystemProperties.USER;
                        //
                        //using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m66212/mst/i", new StringContent(JsonConvert.SerializeObject(masterDomain), System.Text.Encoding.UTF8, "application/json")))
                        //{
                        //    if (response.IsSuccessStatusCode)
                        //    {
                        //        string result = await response.Content.ReadAsStringAsync();
                        //        if (int.TryParse(result, out _Num) == false)
                        //        {
                        //            //실패
                        //            //WinUIMessageBox.Show(result, _title, MessageBoxButton.OK, MessageBoxImage.Error);
                        //            return;
                        //        }
                        //    }
                        //}
                    }
                }
                else if (tolQty)
                {
                    if (e.IsValid)
                    {
                        if (string.IsNullOrEmpty(masterDomain.TOL_QTY + ""))
                        {
                            masterDomain.TOL_QTY = 0;
                        }
                        //
                        if (!masterDomain.TOL_QTY.Equals((e.Value == null ? "" : e.Value.ToString())))
                        {
                            masterDomain.TOL_QTY = e.Value;

                        }

                        masterDomain.CRE_USR_ID = SystemProperties.USER;
                        masterDomain.UPD_USR_ID = SystemProperties.USER;
                        //
                        //using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m66212/mst/i", new StringContent(JsonConvert.SerializeObject(masterDomain), System.Text.Encoding.UTF8, "application/json")))
                        //{
                        //    if (response.IsSuccessStatusCode)
                        //    {
                        //        string result = await response.Content.ReadAsStringAsync();
                        //        if (int.TryParse(result, out _Num) == false)
                        //        {
                        //            //실패
                        //            //WinUIMessageBox.Show(result, _title, MessageBoxButton.OK, MessageBoxImage.Error);
                        //            return;
                        //        }
                        //    }
                        //}
                    }
                }
                else if (mtrlItmSeq)
                {
                    if (e.IsValid)
                    {
                        if (string.IsNullOrEmpty(masterDomain.MTRL_ITM_SEQ + ""))
                        {
                            masterDomain.MTRL_ITM_SEQ = 1;
                        }
                        //
                        if (!masterDomain.MTRL_ITM_SEQ.Equals((e.Value == null ? "" : e.Value.ToString())))
                        {
                            masterDomain.MTRL_ITM_SEQ = Convert.ToInt16(e.Value);
                        }

                        masterDomain.CRE_USR_ID = SystemProperties.USER;
                        masterDomain.UPD_USR_ID = SystemProperties.USER;
                        //
                        //using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m66212/mst/i", new StringContent(JsonConvert.SerializeObject(masterDomain), System.Text.Encoding.UTF8, "application/json")))
                        //{
                        //    if (response.IsSuccessStatusCode)
                        //    {
                        //        string result = await response.Content.ReadAsStringAsync();
                        //        if (int.TryParse(result, out _Num) == false)
                        //        {
                        //            //실패
                        //            //WinUIMessageBox.Show(result, _title, MessageBoxButton.OK, MessageBoxImage.Error);
                        //            return;
                        //        }
                        //    }
                        //}
                    }
                }
                this.ConfigViewPage1Edit_Master.RefreshData();
            }
            catch (Exception eLog)
            {
                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                e.ErrorContent = eLog.Message;
                //this.MSG.Text = eLog.Message;
                e.SetError(e.ErrorContent, e.ErrorType);
                return;
            }
        }

        //IsEdit
        //public bool IsEdit
        //{
        //    get
        //    {
        //        return this.isEdit;
        //    }
        //}

        //Vo
        //public ManVo resultDomain
        //{
        //    get
        //    {
        //        return this.updateDao;
        //    }
        //}

    }
}
