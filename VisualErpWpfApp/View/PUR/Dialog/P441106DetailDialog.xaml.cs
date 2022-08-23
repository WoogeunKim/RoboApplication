using AquilaErpWpfApp3.Util;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Dialogs;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Code;
using ModelsLibrary.Pur;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;

namespace AquilaErpWpfApp3.View.PUR.Dialog
{
    public partial class P441106DetailDialog : DXWindow
    {
        //private static PurServiceClient purClient = SystemProperties.PurClient;
        private PurVo orgDao;
        private bool isEdit = false;
        private PurVo OrgDao;

        private List<PurVo> FileList = new List<PurVo>();
        private List<PurVo> UpdateDaoList = new List<PurVo>();
        //private byte[] bfiledata;          // 바이트[] 형식의 변수 선언(파일 넣기용)

        private string _title = "고객사발주등록";

        public P441106DetailDialog(PurVo _Dao)
        { 
            InitializeComponent();

            this.OrgDao = _Dao;
            //OrgDao.PUR_NO = _Dao.PUR_NO;

            SYSTEM_CODE_VO();


            this.grid_File.ItemsSource = FileList;   // List랑 그리드랑 연결

            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
            this.OKButton.Click += new RoutedEventHandler(OKButton_Click);
            this.CancelButton.Click += new RoutedEventHandler(CancelButton_Click);
            this.btn_import.Click += new RoutedEventHandler(btn_import_Click);

            this.ApplyButton.Click += new RoutedEventHandler(ApplyButton_Click);
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PurVo masterDomain = (PurVo)grid_File.GetFocusedRow();

                MessageBoxResult result = WinUIMessageBox.Show("[ 납품처 :" + masterDomain.DE_CO_NM + " / 공사명 : " + masterDomain.CNTR_NM + " ] 일괄 적용 하시겠습니까?", _title, MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    List <PurVo> _applyList = this.grid_File.ItemsSource as List<PurVo>;
                    foreach (PurVo _item in _applyList)
                    {
                        _item.DE_CO_NM = masterDomain.DE_CO_NM;
                        _item.CNTR_NM = masterDomain.CNTR_NM;

                    }
                    this.grid_File.ItemsSource = _applyList;
                    this.grid_File.RefreshData();
                }
            }
            catch
            {
                return;
            }
        }


        /// <summary>
        /// FileDialog 호출
        /// </summary>
        private async void btn_import_Click(object sender, RoutedEventArgs e)
        {
            var fileDialog = new DXOpenFileDialog();
                fileDialog.Multiselect = true;
                fileDialog.Filter = "drawing Files|*.dwg;*.xls;*.xlsx;*.dxf|drawing File|*.dwg|Excel File|*.xls;*.xlsx|All files|*.*";
            if (fileDialog.ShowDialog() == true)
            {
                this.text_FLR_NM.Text = fileDialog.FileName;

                //
                string[] _fileList = fileDialog.FileNames;
                byte[] _fileSize;
                foreach (string item in _fileList)
                {
                    _fileSize = System.IO.File.ReadAllBytes(item);
                    if (_fileSize.Length > 370000000)
                    {
                        WinUIMessageBox.Show("350MB를 초과 하였습니다 ", _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    //
                    UpdateDaoList.Add(new PurVo()                       // 파일선택했을때 UpdateDao에 디폴트값 추가
                    { 
                          PUR_NO = this.OrgDao.PUR_NO                   // 생성자에서 받아온 값을 대입
                        , CRE_DT = DateTime.Now.ToString("yyyy-MM-dd")  // 현재 날짜 대입
                        , FLR_FILE = _fileSize                          // 파일을 바이너리로 변환해서 대입
                        , FLR_NM = Path.GetFileName(item)               // 파일 경로의 파일명만 추출해서 대입
                        , IN_REQ_DT = DateTime.Now.ToString("yyyy-MM-dd")
                        , CHNL_CD = SystemProperties.USER_VO.CHNL_CD
                        , CRE_USR_ID = SystemProperties.USER_VO.CRE_USR_ID
                        , UPD_USR_ID = SystemProperties.USER_VO.UPD_USR_ID
                    });
                }

               this.grid_File.ItemsSource = UpdateDaoList;              // UpdateDao를 그리드에 출력
            }

        }


        #region Functon (OKButton_Click, CancelButton_Click)
        private async void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValueCheckd())
            {
                int _Num = 0;
                if (isEdit == false)
                {
                    if (DXSplashScreen.IsActive == false)
                    {
                        DXSplashScreen.Show<ProgressWindow>();
                    }

                    //
                    foreach (PurVo _item in UpdateDaoList)
                    {
                        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("p441106/dtl/i", new StringContent(JsonConvert.SerializeObject(_item), System.Text.Encoding.UTF8, "application/json")))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                string result = await response.Content.ReadAsStringAsync();
                                if (int.TryParse(result, out _Num) == false)
                                {
                                    if (DXSplashScreen.IsActive == true)
                                    {
                                        DXSplashScreen.Close();
                                    }
                                    //실패
                                    WinUIMessageBox.Show(result, _title, MessageBoxButton.OK, MessageBoxImage.Error);
                                    return;
                                }

                            }
                        }
                    }

                    //
                    if (DXSplashScreen.IsActive == true)
                    {
                        DXSplashScreen.Close();
                    }
                    //
                    //성공
                    WinUIMessageBox.Show("완료 되었습니다", _title, MessageBoxButton.OK, MessageBoxImage.Information);
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
            //if (string.IsNullOrEmpty(this.text_File_NM.Text))
            //{
            //    WinUIMessageBox.Show("파일을 넣으셔야 저장가능합니다", "[고객발주관리]도면등록", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.text_DO_RQST_NO.IsTabStop = true;
            //    this.text_DO_RQST_NO.Focus();
            //    return false;
            //}
            //else
         
            return true;
        }
        #endregion

        #region Functon (getDomain - ConfigView1Dao)
        //private PurVo getDomain()
        //{
        //    PurVo Dao = new PurVo();

        //    Dao.PUR_ORD_NO = this.text_PUR_ORD_NO.Text;

        //    SystemCodeVo areaVo = this.combo_AREA_CD.SelectedItem as SystemCodeVo;
        //    Dao.AREA_CD = areaVo.CLSS_CD;
        //    Dao.AREA_NM = areaVo.CLSS_DESC;

        //    Dao.PUR_DT = Convert.ToDateTime(this.text_PUR_DT.Text).ToString("yyyy-MM-dd");
        //    Dao.PUR_DUE_DT = Convert.ToDateTime(this.text_PUR_DUE_DT.Text).ToString("yyyy-MM-dd");

        //    SystemCodeVo coNmVo = this.combo_CO_NO.SelectedItem as SystemCodeVo;
        //    Dao.PUR_CO_CD = coNmVo.CO_NO;
        //    Dao.CO_NO = coNmVo.CO_NO;
        //    Dao.CO_NM = coNmVo.CO_NM;

        //    SystemCodeVo itmVo = this.combo_PUR_ITM_NM.SelectedItem as SystemCodeVo;
        //    Dao.PUR_ITM_CD = itmVo.CLSS_CD;
        //    Dao.PUR_ITM_NM = itmVo.CLSS_DESC;


        //    //UserCodeDao purEmpeIdVo = this.combo_PUR_EMPE_ID.SelectedItem as UserCodeDao;
        //    //Dao.PUR_EMPE_ID = purEmpeIdVo.USR_ID;
        //    //Dao.USR_NM = purEmpeIdVo.USR_N1ST_NM;

        //    Dao.PUR_RMK = this.text_ORD_RMK.Text;

        //    Dao.PUR_CLZ_FLG = this.text_PUR_CLZ_FLG.Text;

        //    Dao.CRE_USR_ID = SystemProperties.USER;
        //    Dao.UPD_USR_ID = SystemProperties.USER;
        //    Dao.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;

        //    return Dao;
        //}
        #endregion

        //IsEdit
        public bool IsEdit
        {
            get
            {
                return this.isEdit;
            }
        }


        public string PUR_NO
        {
            get;
            set;
        }


        private void GridColumn_Validate(object sender, DevExpress.Xpf.Grid.GridCellValidationEventArgs e)
        {
            try
            {
                PurVo masterDomain = (PurVo)grid_File.GetFocusedRow();

                bool in_req_dt = e.Column.FieldName.ToString().Equals("IN_REQ_DT") ? true : false;       // 납품요청일
                bool cntr_nm = e.Column.FieldName.ToString().Equals("CNTR_NM") ? true : false;          // 공사명
                bool cntr_psn_nm = e.Column.FieldName.ToString().Equals("CNTR_PSN_NM") ? true : false;  // 공사부위
                bool de_co_nm = e.Column.FieldName.ToString().Equals("DE_CO_NM") ? true : false;        // 납품처
                bool qur_qty = e.Column.FieldName.ToString().Equals("PUR_QTY") ? true : false;          // 도면수량
                bool pur_wgt = e.Column.FieldName.ToString().Equals("PUR_WGT") ? true : false;          // 도면중량

                if(in_req_dt)
                {
                    if(e.IsValid)
                    {
                        DateTime date;
                        if (DateTime.TryParseExact(e.Value.ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out date) == false)
                        {
                            e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                            e.ErrorContent = "[" + e.Value.ToString() + "] (yyyy-MM-dd) 입력 값이 맞지 않습니다";
                            e.SetError(e.ErrorContent, e.ErrorType);
                            return;
                        }
                        if (string.IsNullOrEmpty(masterDomain.IN_REQ_DT))
                        {
                            masterDomain.IN_REQ_DT = "";
                        }
                        if (!masterDomain.IN_REQ_DT.Equals(e.Value == null ? "" : e.Value.ToString()))
                        {
                            masterDomain.IN_REQ_DT = e.Value.ToString();
                            masterDomain.isCheckd = true;
                        }
                    }
                }
                else if(cntr_nm)
                {
                    if(e.IsValid)
                    {
                        if (string.IsNullOrEmpty(masterDomain.CNTR_NM))
                        {
                            masterDomain.CNTR_NM = "";
                        }
                        if (!masterDomain.CNTR_NM.Equals(e.Value == null ? "" : e.Value.ToString()))
                        {
                            masterDomain.CNTR_NM = e.Value.ToString();
                            masterDomain.isCheckd = true;
                        }
                    }
                }
                else if (cntr_psn_nm)
                {
                    if (e.IsValid)
                    {
                        if (string.IsNullOrEmpty(masterDomain.CNTR_PSN_NM))
                        {
                            masterDomain.CNTR_PSN_NM = "";
                        }
                        if (!masterDomain.CNTR_PSN_NM.Equals(e.Value == null ? "" : e.Value.ToString()))
                        {
                            masterDomain.CNTR_PSN_NM = e.Value.ToString();
                            masterDomain.isCheckd = true;
                        }
                    }
                }
                else if (de_co_nm)
                {
                    if (e.IsValid)
                    {
                        if (string.IsNullOrEmpty(masterDomain.DE_CO_NM))
                        {
                            masterDomain.DE_CO_NM = "";
                        }
                        if (!masterDomain.DE_CO_NM.Equals(e.Value == null ? "" : e.Value.ToString()))
                        {
                            masterDomain.DE_CO_NM = e.Value.ToString();
                            masterDomain.isCheckd = true;
                        }
                    }
                }
                else if (qur_qty)
                {
                    if (e.IsValid)
                    {
                        if (masterDomain.PUR_QTY == null)
                        {
                            masterDomain.PUR_QTY = "";
                        }
                        if (!masterDomain.PUR_QTY.Equals(e.Value == null ? "" : e.Value.ToString()))
                        {
                            masterDomain.PUR_QTY = e.Value.ToString();
                            masterDomain.isCheckd = true;
                        }
                    }
                }
                else if (pur_wgt)
                {
                    if (e.IsValid)
                    {
                        if (masterDomain.PUR_WGT == null)
                        {
                            masterDomain.PUR_WGT = "";
                        }
                        if (!masterDomain.PUR_WGT.Equals(e.Value == null ? "" : e.Value.ToString()))
                        {
                            masterDomain.PUR_WGT = e.Value.ToString();
                            masterDomain.isCheckd = true;
                        }
                    }
                }


                this.grid_File.RefreshData();
            }
            catch (Exception eLog)
            {
                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                e.ErrorContent = eLog.Message;
                e.SetError(e.ErrorContent, e.ErrorType);
                return;
            }
        }


        public async void SYSTEM_CODE_VO()
        {

            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s11479/mst", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD, DELT_FLG = "N"}), System.Text.Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.lue_DE_CO_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().Where<SystemCodeVo>(x => Convert.ToInt32(x.DE_PRNT_NO) == 0).ToList();
                    this.lue_CNTR_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().Where<SystemCodeVo>(x => Convert.ToInt32(x.DE_PRNT_NO) != 0).ToList();
                }
            }
         
        }

    }
}
