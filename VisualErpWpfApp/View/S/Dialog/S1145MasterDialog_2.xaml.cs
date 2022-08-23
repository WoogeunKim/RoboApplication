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

namespace AquilaErpWpfApp3.S.View.Dialog
{
    public partial class S1145MasterDialog_2 : DXWindow
    {
        //private static CodeServiceClient codeClient = SystemProperties.CodeClient;
        //private static ItemCodeServiceClient itemClient = SystemProperties.ItemClient;
        private SystemCodeVo orgDao;

        private SystemCodeVo updateVo;
        //private SystemCodeVo itemDao;

        //private S1145MasterExcelDialog excelDialog;

        private string title = "제품 단가 등록 (사업장)";
        public bool isEdit = false;

        public S1145MasterDialog_2(SystemCodeVo Dao)
        {
            InitializeComponent();
            //

            SYSTEM_CODE_VO();
            //this.combo_AREA_NM.ItemsSource = SystemProperties.SYSTEM_CODE_VO("L-002");
            //this.combo_ITM_GRP_CLSS_CD.ItemsSource = SystemProperties.SYSTEM_CODE_VO("L-001");
            //this.combo_ITM_CD.ItemsSource = SystemProperties.ITM_CODE_VO(Dao.ITM_GRP_CLSS_CD);

            //this.combo_ITM_GRP_CLSS_CD.SelectedIndexChanged += combo_ITM_GRP_CLSS_CD_SelectedIndexChanged;

            this.orgDao = Dao;
            SystemCodeVo copyDao = new SystemCodeVo()
            {
                AREA_CD = Dao.AREA_CD,
                AREA_NM = Dao.AREA_NM,
                ITM_CD = Dao.ITM_CD,
                ITM_NM = Dao.ITM_NM,
                ITM_SZ_NM = Dao.ITM_SZ_NM,
                ST_APLY_DT = Dao.ST_APLY_DT,
                END_APLY_DT = Dao.END_APLY_DT,
                ITM_PRC = Dao.ITM_PRC,
                CRE_USR_ID = Dao.CRE_USR_ID,
                UPD_USR_ID = Dao.UPD_USR_ID,
                ITM_GRP_CLSS_NM = Dao.ITM_GRP_CLSS_NM,
                ITM_GRP_CLSS_CD = Dao.ITM_GRP_CLSS_CD
            };

            this.combo_AREA_NM.Text = Dao.AREA_NM;
            this.text_ITM_PRC.Text = "0";
            this.text_FM_DT.Text = System.DateTime.Now.ToString("yyyy-MM-dd");
           
            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);

           
            //this.FindButton.Click += new RoutedEventHandler(FindButton_Click);

            this.OKButton.Click += new RoutedEventHandler(OKButton_Click);
            this.CancelButton.Click += new RoutedEventHandler(CancelButton_Click);

            //this.text_ITM_CD.KeyDown += new KeyEventHandler(_KeyDown);
            //this.EXCELButton.IsEnabled = false;
            //this.OKButton.IsEnabled = false;
        }

        

        //private void combo_ITM_GRP_CLSS_CD_SelectedIndexChanged(object sender, RoutedEventArgs e)
        //{
        //    CodeDao ItmGrpVo = this.combo_ITM_GRP_CLSS_CD.SelectedItem as CodeDao;
        //    if (ItmGrpVo != null)
        //    {
        //        this.combo_ITM_CD.ItemsSource = SystemProperties.ITM_CODE_VO(ItmGrpVo.CLSS_CD);
        //    }
        //}

        void _KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                FindButton_Click(sender, null);
            }
        }


        #region Functon (FindButton_Click, OKButton_Click, CancelButton_Click)
        private async void FindButton_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    if (string.IsNullOrEmpty(this.text_ITM_CD.Text))
            //    {
            //        WinUIMessageBox.Show("[품번] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
            //        this.text_ITM_CD.IsTabStop = true;
            //        this.text_ITM_CD.Focus();
            //        return;
            //    }

            //    //
            //    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s141", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { ITM_CD = this.text_ITM_CD.Text, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
            //    {
            //        if (response.IsSuccessStatusCode)
            //        {
            //            IList<SystemCodeVo> itmList = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
            //            if (itmList.Count > 0)
            //            {
            //                itemDao = itmList[0];

            //                this.text_ITM_CD.Text = itmList[0].ITM_CD;
            //                this.text_ITM_NM.Text = itmList[0].ITM_NM;
            //                this.text_ITM_SZ.Text = itmList[0].ITM_SZ_NM;
            //            }
            //            else
            //            {
            //                WinUIMessageBox.Show("[품번] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
            //                this.text_ITM_CD.Focus();

            //                this.text_ITM_CD.Text = string.Empty;
            //                this.text_ITM_NM.Text = string.Empty;
            //                this.text_ITM_SZ.Text = string.Empty;

            //                itemDao = null;

            //                return;
            //            }
            //        }
            //    }
            //}
            //catch (System.Exception eLog)
            //{
            //    WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
            //    return;
            //}
        }

        private async void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValueCheckd() == true)
            {
                this.updateVo = getDomain();
                if (Convert.ToDouble(this.updateVo.ITM_PRC) < 0)
                {
                    WinUIMessageBox.Show("[단가(-값)] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                    this.text_ITM_PRC.IsTabStop = true;
                    this.text_ITM_PRC.Focus();
                    return;
                }


                MessageBoxResult result = WinUIMessageBox.Show("[" + Convert.ToDateTime(this.text_FM_DT.Text).ToString("yyyy-MM-dd") + " / " + this.combo_AREA_NM.Text + " / " + this.combo_ITM_NM.Text + " 정말로 저장 하시겠습니까?", "[저장]" + title, MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    int _Num = 0;
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("S1145/i", new StringContent(JsonConvert.SerializeObject(this.updateVo), System.Text.Encoding.UTF8, "application/json")))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string resultMsg = await response.Content.ReadAsStringAsync();
                            if (int.TryParse(resultMsg, out _Num) == false)
                            {
                                //실패
                                WinUIMessageBox.Show(resultMsg, title, MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }

                            //성공
                            //WinUIMessageBox.Show("완료 되었습니다", title, MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }

                    //SystemCodeVo resultVo = codeClient.S1145InsertMst_2(getDomain());
                    //if (!resultVo.isSuccess)
                    //{
                    //    //실패
                    //    WinUIMessageBox.Show(resultVo.Message, "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
                    //    return;
                    //}

                    //resultVo = codeClient.ProcS1145_2(getDomain());
                    //if (!resultVo.isSuccess)
                    //{
                    //    //실패
                    //    WinUIMessageBox.Show(resultVo.Message, "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
                    //    return;
                    //}

                    //SystemCodeVo resultVo;
                    //IList<SystemCodeVo> itemsList = (IList<SystemCodeVo>)this.ViewGridDtl.ItemsSource;

                    ////저장
                    //for (int x = 0; x < itemsList.Count; x++)
                    //{
                    //    if (itemsList[x].isCheckd)
                    //    {
                    //        //선분 이력 관리
                    //        resultVo = codeClient.S1145UpdateMst(itemsList[x]);
                    //        if (!resultVo.isSuccess)
                    //        {
                    //            //실패
                    //            WinUIMessageBox.Show(resultVo.Message, "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
                    //            return;
                    //        }
                    //    }
                    //}

                    //성공
                    WinUIMessageBox.Show("완료 되었습니다", this.title, MessageBoxButton.OK, MessageBoxImage.Information);

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

        //private void EXCELButton_Click(object sender, RoutedEventArgs e)
        //{
        //    //try
        //    //{
        //    //    SystemCodeVo _param = new SystemCodeVo();

        //    //    string sDirPath = AppDomain.CurrentDomain.BaseDirectory;
        //    //    DirectoryInfo di = new DirectoryInfo(sDirPath);
        //    //    if (di.Exists == false)
        //    //    {
        //    //        di.Create();
        //    //    }

        //    //    string sFilePath = sDirPath + "\\" + Convert.ToDateTime(this.text_ST_APLY_DT.Text).ToString("yyyyMMdd") + "_" + Convert.ToDateTime(this.text_END_APLY_DT.Text).ToString("yyyyMMdd") + "_" + this.combo_AREA_NM.Text + ".xlsx";
        //    //    this.ViewTableDtl.ExportToXlsx(sFilePath);

        //    //    CodeDao AREA_NMVo = this.combo_AREA_NM.SelectedItem as CodeDao;
        //    //    if (AREA_NMVo != null)
        //    //    {
        //    //        _param.AREA_CD = AREA_NMVo.CLSS_CD;
        //    //        _param.AREA_NM = AREA_NMVo.CLSS_DESC;
        //    //    }

        //    //    _param.CRE_USR_ID = SystemProperties.USER;
        //    //    _param.UPD_USR_ID = SystemProperties.USER;
        //    //    _param.ST_APLY_DT = Convert.ToDateTime(this.text_ST_APLY_DT.Text).ToString("yyyy-MM-dd");
        //    //    _param.FM_DT = Convert.ToDateTime(this.text_ST_APLY_DT.Text).ToString("yyyyMMdd");

        //    //    _param.END_APLY_DT = Convert.ToDateTime(this.text_END_APLY_DT.Text).ToString("yyyy-MM-dd");


        //    //    excelDialog = new S1145MasterExcelDialog(_param, sFilePath, ((IList<SystemCodeVo>)this.ViewGridDtl.ItemsSource).Count);
        //    //    excelDialog.Title = title + " - 엑셀 수정";
        //    //    excelDialog.Owner = this;
        //    //    excelDialog.BorderEffect = BorderEffect.Default;
        //    //    //masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
        //    //    //masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
        //    //    bool isDialog = (bool)excelDialog.ShowDialog();
        //    //    if (isDialog)
        //    //    {
        //    //        _param = new SystemCodeVo();

        //    //        _param.AREA_CD = this.orgDao.AREA_CD;
        //    //        _param.ST_APLY_DT = Convert.ToDateTime(this.orgDao.ST_APLY_DT).ToString("yyyy-MM-dd");
        //    //        _param.END_APLY_DT = Convert.ToDateTime(this.orgDao.END_APLY_DT).ToString("yyyy-MM-dd");
        //    //        //
        //    //        this.ViewGridDtl.ItemsSource = codeClient.S1145SelectDtlList(_param);
        //    //        this.ViewGridDtl.RefreshData();
        //    //    }
        //    //}
        //    //catch (System.Exception eLog)
        //    //{
        //    //     WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
        //    //     return;
        //    //}
        //}


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
            if (string.IsNullOrEmpty(this.combo_AREA_NM.Text))
            {
                WinUIMessageBox.Show("[사업장] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.combo_AREA_NM.IsTabStop = true;
                this.combo_AREA_NM.Focus();
                return false;
            }
            //else if (itemDao == null)
            //{
            //    WinUIMessageBox.Show("[품번] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.text_ITM_CD.IsTabStop = true;
            //    this.text_ITM_CD.Focus();
            //    return false;
            //}
            //else if (string.IsNullOrEmpty(this.combo_ITM_CD.Text))
            //{
            //    WinUIMessageBox.Show("[품번] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.combo_ITM_CD.IsTabStop = true;
            //    this.combo_ITM_CD.Focus();
            //    return false;
            //}
            //else if (string.IsNullOrEmpty(this.text_ST_APLY_DT.Text))
            //{
            //    WinUIMessageBox.Show("[사용 시작] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.text_ST_APLY_DT.IsTabStop = true;
            //    this.text_ST_APLY_DT.Focus();
            //    return false;
            //}
            //else if (string.IsNullOrEmpty(this.text_END_APLY_DT.Text))
            //{
            //    WinUIMessageBox.Show("[사용 종료] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.text_END_APLY_DT.IsTabStop = true;
            //    this.text_END_APLY_DT.Focus();
            //    return false;
            //}
            //else
            //{
            //    //if (this.isEdit == false)
            //    //{
            //    //    CodeDao AreaVo = this.combo_AREA_NM.SelectedItem as CodeDao;
            //    //    ItmCodeDao ItmVo = this.combo_ITM_CD.SelectedItem as ItmCodeDao;

            //    //    SystemCodeVo dao = new SystemCodeVo()
            //    //    {
            //    //        AREA_CD = AreaVo.CLSS_CD
            //    //        ,
            //    //        ITM_CD = ItmVo.ITM_CD
            //    //        ,
            //    //        ST_APLY_DT = Convert.ToDateTime(this.text_ST_APLY_DT.Text).ToString("yyyy-MM-dd")
            //    //        ,
            //    //        END_APLY_DT = Convert.ToDateTime(this.text_END_APLY_DT.Text).ToString("yyyy-MM-dd")
            //    //    };
            //    //    //ObservableCollection<SystemCodeVo> daoList = service.SearchDetailConfigView1(dao);
            //    //    IList<SystemCodeVo> daoList = (IList<SystemCodeVo>)codeClient.S1145SelectMstList(dao);
            //    //    if (daoList.Count != 0)
            //    //    {
            //    //        WinUIMessageBox.Show("[코드 - 중복] 코드를 다시 입력 하십시오.", "[중복검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
            //    //        this.combo_ITM_CD.IsTabStop = true;
            //    //        this.combo_ITM_CD.Focus();
            //    //        return false;
            //    //    }
            //    //}
            //}
            return true;
        }
        #endregion

        #region Functon (getDomain - ConfigView1Dao)
        public SystemCodeVo getDomain()
        {
            SystemCodeVo Dao = new SystemCodeVo();

            SystemCodeVo AREA_NMVo = this.combo_AREA_NM.SelectedItem as SystemCodeVo;
            if (AREA_NMVo != null)
            {
                Dao.AREA_CD = AREA_NMVo.CLSS_CD;
                Dao.AREA_NM = AREA_NMVo.CLSS_DESC;
            }

            SystemCodeVo itemDao = this.combo_ITM_NM.SelectedItem as SystemCodeVo;
            if (itemDao != null)
            {
                Dao.ITM_CD = itemDao.ITM_CD;
                Dao.ITM_NM = itemDao.ITM_NM;
                Dao.ITM_SZ_NM = itemDao.ITM_SZ_NM;
            }


            Dao.FM_DT = Convert.ToDateTime(this.text_FM_DT.Text).ToString("yyyyMMdd");

            Dao.ITM_PRC = float.Parse((this.text_ITM_PRC.Text == null ? "0" : this.text_ITM_PRC.Text));
            Dao.CRE_USR_ID = SystemProperties.USER;
            Dao.UPD_USR_ID = SystemProperties.USER;
            Dao.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
            Dao.RN = 2;
            return Dao;
        }
        #endregion

        //private void GridColumn_Validate(object sender, DevExpress.Xpf.Grid.GridCellValidationEventArgs e)
        //{
        //    try
        //    {
        //        //SystemCodeVo resultVo;
        //        SystemCodeVo masterDomain = (SystemCodeVo)ViewGridDtl.GetFocusedRow();

        //        bool dcRto = (e.Column.FieldName.ToString().Equals("ITM_PRC") ? true : false);

        //        if (dcRto)
        //        {
        //            if (e.IsValid)
        //            {
        //                if (string.IsNullOrEmpty(masterDomain.ITM_PRC + ""))
        //                {
        //                    masterDomain.ITM_PRC = 0;
        //                }
        //                //
        //                //
        //                if (!masterDomain.ITM_PRC.Equals((e.Value == null ? "0" : e.Value.ToString())))
        //                {

        //                    if (e.Value.ToString().Equals("0"))
        //                    {
        //                        e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
        //                        e.ErrorContent = "단가 0을 입력할 수 없습니다";
        //                        e.SetError(e.ErrorContent, e.ErrorType);
        //                        return;
        //                    }

        //                    CodeDao AREA_NMVo = this.combo_AREA_NM.SelectedItem as CodeDao;

        //                    this.OKButton.IsEnabled = true;
        //                    masterDomain.isCheckd = true;
        //                    masterDomain.AREA_CD = AREA_NMVo.CLSS_CD;
        //                    masterDomain.AREA_NM = AREA_NMVo.CLSS_DESC;

        //                    masterDomain.CRE_USR_ID = SystemProperties.USER;
        //                    masterDomain.UPD_USR_ID = SystemProperties.USER;
        //                    masterDomain.ST_APLY_DT = Convert.ToDateTime(this.text_ST_APLY_DT.Text).ToString("yyyy-MM-dd");
        //                    masterDomain.FM_DT = Convert.ToDateTime(this.text_ST_APLY_DT.Text).ToString("yyyyMMdd");

        //                    masterDomain.END_APLY_DT = Convert.ToDateTime(this.text_END_APLY_DT.Text).ToString("yyyy-MM-dd");
        //                    masterDomain.ITM_PRC = float.Parse(e.Value.ToString());


        //                    //if (masterDomain.SYS_FLG.Equals("Y"))
        //                    //{
        //                    //    resultVo = codeClient.S1145UpdateMst(masterDomain);
        //                    //    if (!resultVo.isSuccess)
        //                    //    {
        //                    //        //실패
        //                    //        WinUIMessageBox.Show(resultVo.Message, "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
        //                    //        return;
        //                    //    }
        //                    //}
        //                    //else
        //                    //{
        //                    //    IList<SystemCodeVo> daoList = (IList<SystemCodeVo>)codeClient.S1145SelectMstList(masterDomain);
        //                    //    if (daoList.Count != 0)
        //                    //    {
        //                    //        WinUIMessageBox.Show("[코드 - 중복] 코드를 다시 입력 하십시오.", "[중복검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
        //                    //        //this.ViewTableDtl.FocusedRowHandle = x;
        //                    //        return;
        //                    //    }
        //                    //    else
        //                    //    {
        //                    //        resultVo = codeClient.S1145InsertMst(masterDomain);
        //                    //        if (!resultVo.isSuccess)
        //                    //        {
        //                    //            //실패
        //                    //            WinUIMessageBox.Show(resultVo.Message, "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
        //                    //            return;
        //                    //        }
        //                    //    }
        //                    //}

        //                    ////선분 이력 관리
        //                    //resultVo = codeClient.S1145UpdateMst(masterDomain);
        //                    //if (!resultVo.isSuccess)
        //                    //{
        //                    //    //실패
        //                    //    WinUIMessageBox.Show(resultVo.Message, "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
        //                    //    return;
        //                    //}

        //                }
        //            }
        //        }
        //    }
        //    catch (Exception eLog)
        //    {
        //        e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
        //        e.ErrorContent = eLog.Message;
        //        e.SetError(e.ErrorContent, e.ErrorType);
        //        return;
        //    }
        //}

        //private void ViewGrid_HiddenEditor(object sender, DevExpress.Xpf.Grid.EditorEventArgs e)
        //{
        //    this.ViewTableDtl.CommitEditing();

        //    bool dcRto = (e.Column.FieldName.ToString().Equals("ITM_PRC") ? true : false);

        //    int rowHandle = this.ViewTableDtl.FocusedRowHandle;
        //    if (dcRto)
        //    {
        //        this.ViewGridDtl.CurrentColumn = this.ViewGridDtl.Columns["ITM_PRC"];
        //    }

        //    this.ViewTableDtl.FocusedRowHandle = rowHandle + 1;
        //}


        //private void ButtonInfo_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        MessageBoxResult result = WinUIMessageBox.Show("정말로 단가(B등급) 0 으로 하시겠습니까?", "[삭제]" + title, MessageBoxButton.YesNo, MessageBoxImage.Question);
        //        if (result == MessageBoxResult.Yes)
        //        {
        //            SystemCodeVo masterDomain = (SystemCodeVo)ViewGridDtl.GetFocusedRow();
        //            if (masterDomain == null)
        //            {
        //                return;
        //            }

        //            masterDomain.ITM_PRC = 0;
        //            masterDomain.SYS_FLG = "N";

        //            SystemCodeVo resultVo = codeClient.S1145DeleteMst(masterDomain);
        //            if (!resultVo.isSuccess)
        //            {
        //                //실패
        //                WinUIMessageBox.Show(resultVo.Message, "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
        //                return;
        //            }
        //        }
        //    }
        //    catch (Exception eLog)
        //    {
        //        WinUIMessageBox.Show(eLog.Message, "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
        //        return;
        //    }
        //}

        //IsEdit
        //public bool IsEdit
        //{
        //    get
        //    {
        //        return this.isEdit;
        //    }
        //}

        //public SystemCodeVo ResultDao
        //{
        //    get;
        //    set;
        //}


        public async void SYSTEM_CODE_VO()
        {
            //this.combo_AREA_NM.ItemsSource = SystemProperties.SYSTEM_CODE_VO("L-002");
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "L-002"))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.combo_AREA_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                }
            }

            //품목 마스터
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s141/mini", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD, DELT_FLG = "N", ITM_GRP_CLSS_CD = "G" /*N1ST_ITM_GRP_CD = SelectedMasterItem.N1ST_ITM_GRP_CD*/ }), System.Text.Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.combo_ITM_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                }
            }
        }

    }
}
