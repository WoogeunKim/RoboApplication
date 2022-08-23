using DevExpress.Spreadsheet;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Code;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using AquilaErpWpfApp3.Util;

namespace AquilaErpWpfApp3.S.View.Dialog
{
    /// <summary>
    /// Interaction logic for S1147MasterDialog.xaml
    /// </summary>
    public partial class S1162MasterDialog : DXWindow
    {
        //private static SaleOrderServiceClient saleOrderClient = SystemProperties.SaleOrderClient;

        private IList<SystemCodeVo> _typeRow;
        private SystemCodeVo _a001;
        private SystemCodeVo _a002;
        private SystemCodeVo _a003;
        private SystemCodeVo _a004;
        private SystemCodeVo _a005;
        private SystemCodeVo _a006;



        private IList<SystemCodeVo> _saveRow;
        private SystemCodeVo orgDao;
        private bool isEdit = false;
        private SystemCodeVo updateDao;
        private string title = "근무보고서";

        public S1162MasterDialog(SystemCodeVo Dao)
        {
            InitializeComponent();

            //this.combo_AREA_NM.ItemsSource = SystemProperties.SYSTEM_CODE_VO("L-002");
            SYSTEM_CODE_VO();

            this.orgDao = Dao;

            //SystemCodeVo copyDao = new SystemCodeVo()
            //{
            //    WRK_TP_CD = Dao.WRK_TP_CD,
            //    WRK_TP_NM = Dao.WRK_TP_NM,
            //    ST_HR_MNT = Dao.ST_HR_MNT,
            //    END_HR_MNT = Dao.END_HR_MNT,
            //    WRK_TP_VAL = Dao.WRK_TP_VAL,
            //    CHNL_CD = Dao.CHNL_CD
            //};

            //if (!string.IsNullOrEmpty(copyDao.WRK_TP_CD))
            //{
            //    //this.combo_AREA_NM.IsEnabled = false;
            //    //this.text_loc_cd.IsEnabled = false;
            //    //this.text_WRK_TP_CD.IsEnabled = false;
            //    this.isEdit = true;
            //}

            this.btn_Apply.Click += Btn_Apply_Click;

            //this.configCode.DataContext = copyDao;
            //this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
            this.OKButton.Click += new RoutedEventHandler(OKButton_Click);
            this.CancelButton.Click += new RoutedEventHandler(CancelButton_Click);
        }

        private void Btn_Apply_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //비동기를 쓰레드 작업 후 동기 작업 하는 방법
                _typeRow = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(Task.Run(() => SystemProperties.PROGRAM_HTTP.PostAsync("s1161", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json"))).Result.Content.ReadAsStringAsync().Result).Cast<SystemCodeVo>().ToList();

                // 주간
                _a001 = _typeRow.Where<SystemCodeVo>(x => x.WRK_TP_CD.Equals("A001")).First();
                // 연장
                _a002 = _typeRow.Where<SystemCodeVo>(x => x.WRK_TP_CD.Equals("A002")).First();
                // 조출
                _a003 = _typeRow.Where<SystemCodeVo>(x => x.WRK_TP_CD.Equals("A003")).First();
                //
                //
                // 심야(기본 시간)
                _a004 = _typeRow.Where<SystemCodeVo>(x => x.WRK_TP_CD.Equals("A004")).First();
                // 심야
                _a005 = _typeRow.Where<SystemCodeVo>(x => x.WRK_TP_CD.Equals("A005")).First();
                // (심야, 기본 시간) 휴식 시간
                _a006 = _typeRow.Where<SystemCodeVo>(x => x.WRK_TP_CD.Equals("A006")).First();
            }
            catch
            {
                //
                WinUIMessageBox.Show("[근태기준정보관리 → 주간/야간/조출] 코드를 다시 입력 하십시오.", "[근태기준정보관리]" + this.title, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }


            //
            //사원번호, 근무 일자, 조출, 연장, 출근 시간, 퇴근 시간
            this._saveRow = new List<SystemCodeVo>();

            // Access the active worksheet.
            Worksheet worksheet = spreadsheetControl.ActiveWorksheet;
            // Get the range containing non-empty cells.
            CellRange usedRange = worksheet.GetUsedRange();

            int nMaxRow = usedRange.RowCount;
            SystemCodeVo _imsiVo;
            DateTime chkDate;
            int chknum = 0;
            //float chkfnum = 0;

            //int chkstartTime = 0;
            //int chkendTime = 0;
            //int startTime = 0;
            //int endTime = 0;

            //int imsiStartTime = 0;
            //int imsiEndTime = 0;

            for (int x = 0; x <= nMaxRow; x++)
            {
                //chkstartTime = 0;
                //chkendTime = 0;
                //startTime = 0;
                //endTime = 0;

                //imsiStartTime = 0;
                //imsiEndTime = 0;

                if (string.IsNullOrEmpty(worksheet.Columns["A"][x].DisplayText))
                {
                    continue;
                }

                _imsiVo = new SystemCodeVo()
                {
                    EMPE_ID = worksheet.Columns["A"][x].DisplayText
                     //, WRK_DT = worksheet.Columns["B"][x].DisplayText
                     //, ERY_WRK_FLG = Convert.ToInt16(worksheet.Columns["C"][x].DisplayText)
                     //, XTD_WRK_FLG = Convert.ToInt16(worksheet.Columns["D"][x].DisplayText)
                     //, NGT_WRK_FLG = Convert.ToInt16(worksheet.Columns["E"][x].DisplayText)
                     , ST_TM_NO = worksheet.Columns["F"][x].DisplayText
                     , END_TM_NO = worksheet.Columns["G"][x].DisplayText
                     , WRK_GRP_CD = "A001"
                     , WRK_GRP_NM = "주간"
                     , WRK_RSN_CD = "B001"
                     , WRK_RSN_NM = "출근"
                     , N1ST_TM_VAL = 0
                     , N2ND_TM_VAL = 0
                     , N3RD_TM_VAL = 0
                     , N4TH_TM_VAL = 0
                     , TTL_TM_VAL = 0
                     //, TTL_TM_VAL = Convert.ToDecimal(N1ST_TM_VAL) + Convert.ToDecimal(N2ND_TM_VAL) + Convert.ToDecimal(N3RD_TM_VAL)
                     , CRE_USR_ID = SystemProperties.USER
                     , UPD_USR_ID = SystemProperties.USER
                     , CHNL_CD = SystemProperties.USER_VO.CHNL_CD
                };

                //근무 일자
                _imsiVo.WRK_DT = (DateTime.TryParse(worksheet.Columns["B"][x].DisplayText, out chkDate) ? chkDate.ToString("yyyyMMdd") : "");

                //조출 에러 확인
                _imsiVo.ERY_WRK_FLG= (int.TryParse(worksheet.Columns["C"][x].DisplayText, out chknum) ? chknum : 0);
                //연장 에러 확인
                _imsiVo.XTD_WRK_FLG = (int.TryParse(worksheet.Columns["D"][x].DisplayText, out chknum) ? chknum : 0);
                //심야 에러 확인
                _imsiVo.NGT_WRK_FLG = (int.TryParse(worksheet.Columns["E"][x].DisplayText, out chknum) ? chknum : 0);



                //::계산
                //심야 계산
                //ST_HR_MNT, END_HR_MNT, WRK_TP_VAL

                getNGT_WRK(ref _imsiVo);
                #region 주간/야간 계산
                //if (Convert.ToBoolean(_imsiVo.NGT_WRK_FLG))
                //{
                //    //심야
                //    _imsiVo.WRK_GRP_CD = "A002";
                //    _imsiVo.WRK_GRP_NM = "야간";

                //    //
                //    //비교 범위
                //    chkstartTime = (int.TryParse(_a004.ST_HR_MNT.Replace(":", ""), out chknum) ? chknum : 0);
                //    chkendTime = (int.TryParse(_a004.END_HR_MNT.Replace(":", ""), out chknum) ? chknum : 0);
                //    if (chkstartTime > chkendTime)
                //    {
                //        //24시 기준
                //        chkendTime = chkendTime + 2400;
                //    }

                //    //비교 값
                //    startTime = (int.TryParse(_imsiVo.ST_TM_NO.Replace(":", ""), out chknum) ? chknum : 0);
                //    endTime = (int.TryParse(_imsiVo.END_TM_NO.Replace(":", ""), out chknum) ? chknum : 0);
                //    if (startTime > endTime)
                //    {
                //        //24시 기준
                //        endTime = endTime + 2400;
                //    }

                //    //심야(기본 시간)
                //    if (startTime <= chkstartTime && endTime >= chkendTime)
                //    {
                //        _imsiVo.N1ST_TM_VAL = ((chkendTime - chkstartTime) / 100) * (float.TryParse(_a004.WRK_TP_VAL.ToString(), out chkfnum) ? chkfnum : 0);
                //    }
                //    else
                //    {
                //        imsiStartTime = ((chkendTime - chkstartTime) / 100);
                //        imsiEndTime = 0;
                //        if (startTime > chkstartTime)
                //        {
                //            imsiStartTime = ((chkendTime - startTime) / 100);
                //        }

                //        if (endTime < chkendTime)
                //        {
                //            imsiEndTime = ((chkendTime / 100) - (endTime / 100));
                //        }
                //        _imsiVo.N1ST_TM_VAL = (imsiStartTime - imsiEndTime) * (float.TryParse(_a004.WRK_TP_VAL.ToString(), out chkfnum) ? chkfnum : 0);
                //    }

                //    //
                //    //비교 범위
                //    chkstartTime = (int.TryParse(_a005.ST_HR_MNT.Replace(":", ""), out chknum) ? chknum : 0);
                //    chkendTime = (int.TryParse(_a005.END_HR_MNT.Replace(":", ""), out chknum) ? chknum : 0);
                //    if (chkstartTime > chkendTime)
                //    {
                //        //24시 기준
                //        chkendTime = chkendTime + 2400;
                //    }

                //    //심야
                //    if (startTime <= chkstartTime && endTime >= chkendTime)
                //    {
                //        _imsiVo.N4TH_TM_VAL = ((chkendTime - chkstartTime) / 100) * (float.TryParse(_a005.WRK_TP_VAL.ToString(), out chkfnum) ? chkfnum : 0);

                //        //30분 계산 : 소수점 0.5 더하기
                //        string result = (chkendTime - chkstartTime).ToString();
                //        if (result.Length > 2)
                //        {
                //            result = result.ToString().Substring(result.Length - 2, result.Length - 1);
                //            if (Convert.ToInt32(result) >= 30)
                //            {
                //                _imsiVo.N4TH_TM_VAL = Convert.ToDouble(_imsiVo.N4TH_TM_VAL) + 0.5;
                //            }
                //        }
                //    }
                //    else
                //    {
                //        imsiStartTime = ((chkendTime - chkstartTime) / 100);
                //        imsiEndTime = 0;
                //        if (startTime > chkstartTime)
                //        {
                //            imsiStartTime = ((chkendTime - startTime) / 100);
                //        }

                //        if (endTime < chkendTime)
                //        {
                //            imsiEndTime = ((chkendTime / 100) - (endTime / 100));
                //        }
                //        _imsiVo.N4TH_TM_VAL = (imsiStartTime - imsiEndTime) * (float.TryParse(_a005.WRK_TP_VAL.ToString(), out chkfnum) ? chkfnum : 0);

                //        //30분 계산 : 소수점 0.5 더하기
                //        string result = (imsiStartTime - imsiEndTime).ToString();
                //        if (result.Length > 2)
                //        {
                //            result = result.ToString().Substring(result.Length - 2, result.Length - 1);
                //            if (Convert.ToInt32(result) >= 30)
                //            {
                //                _imsiVo.N4TH_TM_VAL = Convert.ToDouble(_imsiVo.N4TH_TM_VAL) + 0.5;
                //            }
                //        }
                //    }

                //    //
                //    //비교 범위 (휴식 시간 체크 -1)
                //    chkstartTime = (int.TryParse(_a006.ST_HR_MNT.Replace(":", ""), out chknum) ? chknum : 0);
                //    chkendTime = (int.TryParse(_a006.END_HR_MNT.Replace(":", ""), out chknum) ? chknum : 0);

                //    //24시 기준
                //    chkstartTime = chkstartTime + 2400;
                //    chkendTime = chkendTime + 2400;

                //    if (startTime <= chkstartTime && endTime >= chkendTime)
                //    {
                //        //심야(기본 시간) - (휴식 시간 체크 -1)
                //        _imsiVo.N1ST_TM_VAL = Convert.ToInt16(_imsiVo.N1ST_TM_VAL) + (float.TryParse(_a006.WRK_TP_VAL.ToString(), out chkfnum) ? chkfnum : 0);

                //        //심야시간 - (휴식 시간 체크 -1)
                //        _imsiVo.N4TH_TM_VAL = Convert.ToDouble(_imsiVo.N4TH_TM_VAL) + (float.TryParse(_a006.WRK_TP_VAL.ToString(), out chkfnum) ? chkfnum : 0);
                //    }

                //    // 마이너스 이면 0으로 변경
                //    _imsiVo.N1ST_TM_VAL = ((Convert.ToInt16(_imsiVo.N1ST_TM_VAL) < -1) ? 0 : _imsiVo.N1ST_TM_VAL);
                //    // 마이너스 이면 0으로 변경
                //    _imsiVo.N4TH_TM_VAL = ((Convert.ToDouble(_imsiVo.N4TH_TM_VAL) < -1) ? 0 : _imsiVo.N4TH_TM_VAL);
                //}
                //else
                //{
                //    _imsiVo.WRK_GRP_CD = "A001";
                //    _imsiVo.WRK_GRP_NM = "주간";

                //    //주간 계산
                //    //ST_HR_MNT, END_HR_MNT, WRK_TP_VAL
                //    #region 주간 계산
                //    //비교 범위
                //    chkstartTime = (int.TryParse(_a001.ST_HR_MNT.Replace(":", ""), out chknum) ? chknum : 0);
                //    chkendTime = (int.TryParse(_a001.END_HR_MNT.Replace(":", ""), out chknum) ? chknum : 0);
                //    //비교 값
                //    startTime = (int.TryParse(_imsiVo.ST_TM_NO.Replace(":", ""), out chknum) ? chknum : 0);
                //    endTime = (int.TryParse(_imsiVo.END_TM_NO.Replace(":", ""), out chknum) ? chknum : 0);

                //    if (startTime <= chkstartTime && endTime >= chkendTime)
                //    {
                //        _imsiVo.N1ST_TM_VAL = ((chkendTime - chkstartTime) / 100) * (float.TryParse(_a001.WRK_TP_VAL.ToString(), out chkfnum) ? chkfnum : 0);
                //    }
                //    else
                //    {
                //        imsiStartTime = ((chkendTime - chkstartTime) / 100);
                //        imsiEndTime = 0;
                //        if (startTime > chkstartTime)
                //        {
                //            imsiStartTime = ((chkendTime - startTime) / 100);
                //        }

                //        if (endTime < chkendTime)
                //        {
                //            imsiEndTime = ((chkendTime / 100) - (endTime / 100));
                //        }
                //        _imsiVo.N1ST_TM_VAL = (imsiStartTime - imsiEndTime) * (float.TryParse(_a001.WRK_TP_VAL.ToString(), out chkfnum) ? chkfnum : 0);

                //        // 마이너스 이면 0으로 변경
                //        _imsiVo.N1ST_TM_VAL = ((Convert.ToInt16(_imsiVo.N1ST_TM_VAL) < -1) ? 0 : _imsiVo.N1ST_TM_VAL);
                //    }
                //    #endregion
                //} 
                #endregion

                //조출 계산
                if (Convert.ToBoolean(_imsiVo.ERY_WRK_FLG))
                {
                    getERY_WRK(ref _imsiVo);
                    #region 조출 계산
                    ////비교 범위
                    //chkstartTime = (int.TryParse(_a003.ST_HR_MNT.Replace(":", ""), out chknum) ? chknum : 0);
                    //chkendTime = (int.TryParse(_a003.END_HR_MNT.Replace(":", ""), out chknum) ? chknum : 0);
                    ////비교 값
                    //startTime = (int.TryParse(_imsiVo.ST_TM_NO.Replace(":", ""), out chknum) ? chknum : 0);
                    //endTime = (int.TryParse(_imsiVo.END_TM_NO.Replace(":", ""), out chknum) ? chknum : 0);

                    //if (startTime <= chkstartTime && endTime >= chkendTime)
                    //{
                    //    _imsiVo.N2ND_TM_VAL = ((chkendTime - chkstartTime) / 100) * (float.TryParse(_a003.WRK_TP_VAL.ToString(), out chkfnum) ? chkfnum : 0);
                    //}
                    //else
                    //{
                    //    imsiStartTime = ((chkendTime - chkstartTime) / 100);
                    //    imsiEndTime = 0;
                    //    if (startTime > chkstartTime)
                    //    {
                    //        imsiStartTime = ((chkendTime - startTime) / 100);
                    //    }

                    //    if (endTime < chkendTime)
                    //    {
                    //        imsiEndTime = ((chkendTime / 100) - (endTime / 100));
                    //    }
                    //    _imsiVo.N2ND_TM_VAL = (imsiStartTime - imsiEndTime) * (float.TryParse(_a003.WRK_TP_VAL.ToString(), out chkfnum) ? chkfnum : 0);

                    //    // 마이너스 이면 0으로 변경
                    //    _imsiVo.N2ND_TM_VAL = ((Convert.ToInt16(_imsiVo.N2ND_TM_VAL) < -1) ? 0 : _imsiVo.N2ND_TM_VAL);

                    //}
                    #endregion
                }

                //연장 계산
                if (Convert.ToBoolean(_imsiVo.XTD_WRK_FLG))
                {
                    getXTD_WRK(ref _imsiVo);
                    #region 연장 계산
                    ////비교 범위
                    //chkstartTime = (int.TryParse(_a002.ST_HR_MNT.Replace(":", ""), out chknum) ? chknum : 0);
                    //chkendTime = (int.TryParse(_a002.END_HR_MNT.Replace(":", ""), out chknum) ? chknum : 0);
                    ////비교 값
                    //startTime = (int.TryParse(_imsiVo.ST_TM_NO.Replace(":", ""), out chknum) ? chknum : 0);
                    //endTime = (int.TryParse(_imsiVo.END_TM_NO.Replace(":", ""), out chknum) ? chknum : 0);

                    //if (startTime <= chkstartTime && endTime >= chkendTime)
                    //{
                    //    _imsiVo.N3RD_TM_VAL = ((chkendTime - chkstartTime) / 100) * (float.TryParse(_a002.WRK_TP_VAL.ToString(), out chkfnum) ? chkfnum : 0);
                    //}
                    //else
                    //{
                    //    imsiStartTime = ((chkendTime - chkstartTime) / 100);
                    //    imsiEndTime = 0;
                    //    if (startTime > chkstartTime)
                    //    {
                    //        imsiStartTime = ((chkendTime - startTime) / 100);
                    //    }

                    //    if (endTime < chkendTime)
                    //    {
                    //        imsiEndTime = ((chkendTime / 100) - (endTime / 100));
                    //    }
                    //    _imsiVo.N3RD_TM_VAL = (imsiStartTime - imsiEndTime) * (float.TryParse(_a002.WRK_TP_VAL.ToString(), out chkfnum) ? chkfnum : 0);

                    //    // 마이너스 이면 0으로 변경
                    //    _imsiVo.N3RD_TM_VAL = ((Convert.ToInt16(_imsiVo.N3RD_TM_VAL) < -1) ? 0 : _imsiVo.N3RD_TM_VAL);
                    //} 
                    #endregion
                }

                //
                _imsiVo.TTL_TM_VAL = Convert.ToInt16(_imsiVo.N1ST_TM_VAL) + Convert.ToInt16(_imsiVo.N2ND_TM_VAL) + Convert.ToInt16(_imsiVo.N3RD_TM_VAL) + Convert.ToDouble(_imsiVo.N4TH_TM_VAL);
                _saveRow.Add(_imsiVo);
            }

            //
            if (_saveRow.Count > 0)
            {
                this.ViewGridMst.ItemsSource = _saveRow;
                this.ViewGridMst.RefreshData();
            //
                this.OKButton.IsEnabled = true;
            }
            //WinUIMessageBox.Show(worksheet.Columns["A"][0].DisplayText);
            //WinUIMessageBox.Show(worksheet.Columns["A"][1].DisplayText);
            //WinUIMessageBox.Show( "indes : " + worksheet.Index + " /  column : " + usedRange.ColumnCount + " / row" + usedRange.RowCount, title, MessageBoxButton.OK, MessageBoxImage.Information);
            //// Restrict the worksheet's visible area to the used range.
            //spreadsheetControl.WorksheetDisplayArea.SetSize(worksheet.Index, usedRange.ColumnCount, usedRange.RowCount);
        }


        #region Functon (ValueCheckd)
        public Boolean ValueCheckd()
        {
            //if (string.IsNullOrEmpty(this.text_WRK_TP_CD.Text))
            //{
            //    WinUIMessageBox.Show("[코드] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.text_WRK_TP_CD.IsTabStop = true;
            //    this.text_WRK_TP_CD.Focus();
            //    return false;
            //}
            //else
            //{
            //    //if (this.isEdit == false)
            //    //{
            //    //    SaleVo dao = new SaleVo();
            //    //    //CustomerCodeDao coNmVo = this.combo_CLT_CO_NM.SelectedItem as CustomerCodeDao;
            //    //    //if (coNmVo != null)
            //    //    //{
            //    //    //    dao.CO_CD = coNmVo.CO_NO;
            //    //    //    dao.CO_NM = coNmVo.CO_NM;
            //    //    //}

            //    //    CodeDao areaNmVo = this.combo_AREA_NM.SelectedItem as CodeDao;
            //    //    if (areaNmVo != null)
            //    //    {
            //    //        dao.AREA_CD = areaNmVo.CLSS_CD;
            //    //        dao.AREA_NM = areaNmVo.CLSS_DESC;
            //    //    }

            //    //    //JobVo daoList = (JobVo)saleOrderClient.S2219SelectCheck(dao);
            //    //    //if (daoList != null)
            //    //    //{
            //    //    //    WinUIMessageBox.Show("[코드 - 중복] 코드를 다시 입력 하십시오.", "[중복검사]" + this.title, MessageBoxButton.OK, MessageBoxImage.Warning);
            //    //    //    this.combo_AREA_NM.IsTabStop = true;
            //    //    //    this.combo_AREA_NM.Focus();
            //    //    //    return false;
            //    //    //}
            //    //}
            //}
            return true;
        }
        #endregion

        #region Functon (OKButton_Click, CancelButton_Click)
        private async void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValueCheckd())
            {
                int _Num = 0;
                //SystemCodeVo resultVo;
                //if (isEdit == false)
                //{
                //this.updateDao = getDomain();

                //this.ViewGridMst.ItemsSource = _saveRow;

                if (_saveRow.Count > 0)
                {
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s1162/m", new StringContent(JsonConvert.SerializeObject(this.ViewGridMst.ItemsSource), System.Text.Encoding.UTF8, "application/json")))
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
                //}
                //else
                //{
                //    this.updateDao = getDomain();

                //    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s1161/u", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
                //    {
                //        if (response.IsSuccessStatusCode)
                //        {
                //            string result = await response.Content.ReadAsStringAsync();
                //            if (int.TryParse(result, out _Num) == false)
                //            {
                //                //실패
                //                WinUIMessageBox.Show(result, title, MessageBoxButton.OK, MessageBoxImage.Error);
                //                return;
                //            }

                //            //성공
                //            WinUIMessageBox.Show("완료 되었습니다", title, MessageBoxButton.OK, MessageBoxImage.Information);
                //        }
                //    }
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

        #region Functon (getDomain - ConfigView1Dao)
        private SystemCodeVo getDomain()
        {
            SystemCodeVo Dao = new SystemCodeVo();

            //SystemCodeVo areaNmVo = this.combo_AREA_NM.SelectedItem as SystemCodeVo;
            //if (areaNmVo != null)
            //{
            //    Dao.AREA_CD = areaNmVo.CLSS_CD;
            //    Dao.AREA_NM = areaNmVo.CLSS_DESC;
            //}
            //Dao.WRK_TP_CD = this.text_WRK_TP_CD.Text;
            //Dao.WRK_TP_NM = this.text_WRK_TP_NM.Text;
            //Dao.ST_HR_MNT = this.text_ST_HR_MNT.Text;
            //Dao.END_HR_MNT = this.text_END_HR_MNT.Text;
            //Dao.WRK_TP_VAL = this.text_WRK_TP_VAL.Text;

            //Dao.CRE_USR_ID = SystemProperties.USER;
            //Dao.UPD_USR_ID = SystemProperties.USER;
            //Dao.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;

            return Dao;
        }
        #endregion


        public async void SYSTEM_CODE_VO()
        {
            //구분
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "HR-002"))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.lue_WRK_RSN_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                }
            }
            //근무조
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "HR-001"))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.lue_WRK_GRP_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
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
        public SystemCodeVo resultDao
        {
            get
            {
                return this.updateDao;
            }
        }



        private void GridColumn_Validate(object sender, DevExpress.Xpf.Grid.GridCellValidationEventArgs e)
        {
            try
            {
                SystemCodeVo masterDomain = (SystemCodeVo)ViewGridMst.GetFocusedRow();
                bool wrkRsnNm = (e.Column.FieldName.ToString().Equals("WRK_RSN_NM") ? true : false);
                bool wrkGrpNm = (e.Column.FieldName.ToString().Equals("WRK_GRP_NM") ? true : false);

                bool eryWrkFlg = (e.Column.FieldName.ToString().Equals("ERY_WRK_FLG") ? true : false);
                bool xtdWrkFlg = (e.Column.FieldName.ToString().Equals("XTD_WRK_FLG") ? true : false);
                bool ngtWrkFlg = (e.Column.FieldName.ToString().Equals("NGT_WRK_FLG") ? true : false);


                if (wrkRsnNm)
                {
                    if (e.IsValid)
                    {
                        if (string.IsNullOrEmpty(masterDomain.WRK_RSN_NM + ""))
                        {
                            masterDomain.WRK_RSN_NM = "";
                            masterDomain.WRK_RSN_CD = "";
                        }
                        //
                        if (!masterDomain.WRK_RSN_NM.Equals((e.Value == null ? "" : e.Value.ToString())))
                        {
                            SystemCodeVo bankIoDao = this.lue_WRK_RSN_NM.GetItemFromValue(e.Value) as SystemCodeVo;
                            //
                            if (bankIoDao != null)
                            {
                                masterDomain.WRK_RSN_CD = bankIoDao.CLSS_CD;
                                masterDomain.WRK_RSN_NM = bankIoDao.CLSS_DESC;
                            }

                            //masterDomain.isCheckd = true;
                            //this.OKButton.IsEnabled = true;
                        }
                    }
                }
                else if (wrkGrpNm)
                {
                    if (e.IsValid)
                    {
                        if (string.IsNullOrEmpty(masterDomain.WRK_GRP_NM + ""))
                        {
                            masterDomain.WRK_GRP_NM = "";
                            masterDomain.WRK_GRP_CD = "";
                        }
                        //
                        if (!masterDomain.WRK_GRP_NM.Equals((e.Value == null ? "" : e.Value.ToString())))
                        {
                            SystemCodeVo bankIoDao = this.lue_WRK_GRP_NM.GetItemFromValue(e.Value) as SystemCodeVo;
                            //
                            if (bankIoDao != null)
                            {
                                masterDomain.WRK_GRP_CD = bankIoDao.CLSS_CD;
                                masterDomain.WRK_GRP_NM = bankIoDao.CLSS_DESC;
                            }

                            //masterDomain.isCheckd = true;
                            //this.OKButton.IsEnabled = true;
                        }
                    }
                }
                else if (eryWrkFlg)
                {
                    if (e.IsValid)
                    {
                        //조출
                        //if (string.IsNullOrEmpty(masterDomain.ERY_WRK_FLG + ""))
                        //{
                        //    masterDomain.ERY_WRK_FLG = 0;
                        //}
                        //
                        //if (!masterDomain.ERY_WRK_FLG.ToString().Equals((e.Value == null ? "0" : e.Value.ToString())))
                        //{
                        masterDomain.ERY_WRK_FLG = Convert.ToInt16(e.Value);

                        masterDomain.N1ST_TM_VAL = 0;
                        masterDomain.N2ND_TM_VAL = 0;
                        //masterDomain.N3RD_TM_VAL = 0;
                        masterDomain.N4TH_TM_VAL = 0;
                        masterDomain.TTL_TM_VAL = 0;

                        getNGT_WRK(ref masterDomain);
                        getERY_WRK(ref masterDomain);

                        //}
                    }
                }
                else if (xtdWrkFlg)
                {
                    if (e.IsValid)
                    {
                        //연장
                        //if (!masterDomain.XTD_WRK_FLG.ToString().Equals((e.Value == null ? "0" : e.Value.ToString())))
                        //{
                        masterDomain.XTD_WRK_FLG = Convert.ToInt16(e.Value);

                        masterDomain.N1ST_TM_VAL = 0;
                        //masterDomain.N2ND_TM_VAL = 0;
                        masterDomain.N3RD_TM_VAL = 0;
                        masterDomain.N4TH_TM_VAL = 0;
                        masterDomain.TTL_TM_VAL = 0;

                        getNGT_WRK(ref masterDomain);
                        getXTD_WRK(ref masterDomain);

                        //}
                    }
                }
                else if (ngtWrkFlg)
                {
                    if (e.IsValid)
                    {
                        //심야
                        //if (!masterDomain.NGT_WRK_FLG.ToString().Equals((e.Value == null ? "0" : e.Value.ToString())))
                        //{
                        masterDomain.NGT_WRK_FLG = Convert.ToInt16(e.Value);

                        masterDomain.N1ST_TM_VAL = 0;
                        //masterDomain.N2ND_TM_VAL = 0;
                        //masterDomain.N3RD_TM_VAL = 0;
                        masterDomain.N4TH_TM_VAL = 0;
                        masterDomain.TTL_TM_VAL = 0;
                        getNGT_WRK(ref masterDomain);
                        //getXTD_WRK(ref masterDomain);
                        //}
                    }
                }

                this.ViewGridMst.RefreshData();
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
        private void viewPage1EditView_HiddenEditor(object sender, DevExpress.Xpf.Grid.EditorEventArgs e)
        {
            this.ViewTableMst.CommitEditing();

        }



        //연장 계산
        private void getXTD_WRK(ref SystemCodeVo _imsiVo)
        {
            //DateTime chkDate;
            int chknum = 0;
            float chkfnum = 0;

            int chkstartTime = 0;
            int chkendTime = 0;
            int startTime = 0;
            int endTime = 0;

            int imsiStartTime = 0;
            int imsiEndTime = 0;

            //연장 계산
            if (Convert.ToBoolean(_imsiVo.XTD_WRK_FLG))
            {
                #region 연장 계산
                //비교 범위
                chkstartTime = (int.TryParse(_a002.ST_HR_MNT.Replace(":", ""), out chknum) ? chknum : 0);
                chkendTime = (int.TryParse(_a002.END_HR_MNT.Replace(":", ""), out chknum) ? chknum : 0);
                //비교 값
                startTime = (int.TryParse(_imsiVo.ST_TM_NO.Replace(":", ""), out chknum) ? chknum : 0);
                endTime = (int.TryParse(_imsiVo.END_TM_NO.Replace(":", ""), out chknum) ? chknum : 0);

                if (startTime <= chkstartTime && endTime >= chkendTime)
                {
                    _imsiVo.N3RD_TM_VAL = ((chkendTime - chkstartTime) / 100) * (float.TryParse(_a002.WRK_TP_VAL.ToString(), out chkfnum) ? chkfnum : 0);
                }
                else
                {
                    imsiStartTime = ((chkendTime - chkstartTime) / 100);
                    imsiEndTime = 0;
                    if (startTime > chkstartTime)
                    {
                        imsiStartTime = ((chkendTime - startTime) / 100);
                    }

                    if (endTime < chkendTime)
                    {
                        imsiEndTime = ((chkendTime / 100) - (endTime / 100));
                    }
                    _imsiVo.N3RD_TM_VAL = (imsiStartTime - imsiEndTime) * (float.TryParse(_a002.WRK_TP_VAL.ToString(), out chkfnum) ? chkfnum : 0);

                    // 마이너스 이면 0으로 변경
                    _imsiVo.N3RD_TM_VAL = ((Convert.ToInt16(_imsiVo.N3RD_TM_VAL) <= -1) ? 0 : _imsiVo.N3RD_TM_VAL);
                }
                #endregion
            }


        }

        //조출 계산
        private void getERY_WRK(ref SystemCodeVo _imsiVo)
        {
            //DateTime chkDate;
            int chknum = 0;
            float chkfnum = 0;

            int chkstartTime = 0;
            int chkendTime = 0;
            int startTime = 0;
            int endTime = 0;

            int imsiStartTime = 0;
            int imsiEndTime = 0;

            //조출 계산
            if (Convert.ToBoolean(_imsiVo.ERY_WRK_FLG))
            {
                #region 조출 계산
                //비교 범위
                chkstartTime = (int.TryParse(_a003.ST_HR_MNT.Replace(":", ""), out chknum) ? chknum : 0);
                chkendTime = (int.TryParse(_a003.END_HR_MNT.Replace(":", ""), out chknum) ? chknum : 0);
                //비교 값
                startTime = (int.TryParse(_imsiVo.ST_TM_NO.Replace(":", ""), out chknum) ? chknum : 0);
                endTime = (int.TryParse(_imsiVo.END_TM_NO.Replace(":", ""), out chknum) ? chknum : 0);

                if (startTime <= chkstartTime && endTime >= chkendTime)
                {
                    _imsiVo.N2ND_TM_VAL = ((chkendTime - chkstartTime) / 100) * (float.TryParse(_a003.WRK_TP_VAL.ToString(), out chkfnum) ? chkfnum : 0);
                }
                else
                {
                    imsiStartTime = ((chkendTime - chkstartTime) / 100);
                    imsiEndTime = 0;
                    if (startTime > chkstartTime)
                    {
                        imsiStartTime = ((chkendTime - startTime) / 100);
                    }

                    if (endTime < chkendTime)
                    {
                        imsiEndTime = ((chkendTime / 100) - (endTime / 100));
                    }
                    _imsiVo.N2ND_TM_VAL = (imsiStartTime - imsiEndTime) * (float.TryParse(_a003.WRK_TP_VAL.ToString(), out chkfnum) ? chkfnum : 0);

                    // 마이너스 이면 0으로 변경
                    _imsiVo.N2ND_TM_VAL = ((Convert.ToInt16(_imsiVo.N2ND_TM_VAL) <= -1) ? 0 : _imsiVo.N2ND_TM_VAL);

                }
                #endregion
            }


        }

        //주/야간 계산
        private void getNGT_WRK(ref SystemCodeVo _imsiVo)
        {
            //DateTime chkDate;
            int chknum = 0;
            float chkfnum = 0;

            int chkstartTime = 0;
            int chkendTime = 0;
            int startTime = 0;
            int endTime = 0;

            int imsiStartTime = 0;
            int imsiEndTime = 0;

            //심야 계산
            //ST_HR_MNT, END_HR_MNT, WRK_TP_VAL
            if (Convert.ToBoolean(_imsiVo.NGT_WRK_FLG))
            {
                //심야
                _imsiVo.WRK_GRP_CD = "A002";
                _imsiVo.WRK_GRP_NM = "야간";

                //
                //비교 범위
                chkstartTime = (int.TryParse(_a004.ST_HR_MNT.Replace(":", ""), out chknum) ? chknum : 0);
                chkendTime = (int.TryParse(_a004.END_HR_MNT.Replace(":", ""), out chknum) ? chknum : 0);
                if (chkstartTime > chkendTime)
                {
                    //24시 기준
                    chkendTime = chkendTime + 2400;
                }

                //비교 값
                startTime = (int.TryParse(_imsiVo.ST_TM_NO.Replace(":", ""), out chknum) ? chknum : 0);
                endTime = (int.TryParse(_imsiVo.END_TM_NO.Replace(":", ""), out chknum) ? chknum : 0);
                if (startTime > endTime)
                {
                    //24시 기준
                    endTime = endTime + 2400;
                }

                //심야(기본 시간)
                if (startTime <= chkstartTime && endTime >= chkendTime)
                {
                    _imsiVo.N1ST_TM_VAL = ((chkendTime - chkstartTime) / 100) * (float.TryParse(_a004.WRK_TP_VAL.ToString(), out chkfnum) ? chkfnum : 0);
                }
                else
                {
                    imsiStartTime = ((chkendTime - chkstartTime) / 100);
                    imsiEndTime = 0;
                    if (startTime > chkstartTime)
                    {
                        imsiStartTime = ((chkendTime - startTime) / 100);
                    }

                    if (endTime < chkendTime)
                    {
                        imsiEndTime = ((chkendTime / 100) - (endTime / 100));
                    }
                    _imsiVo.N1ST_TM_VAL = (imsiStartTime - imsiEndTime) * (float.TryParse(_a004.WRK_TP_VAL.ToString(), out chkfnum) ? chkfnum : 0);
                }

                //
                //비교 범위
                chkstartTime = (int.TryParse(_a005.ST_HR_MNT.Replace(":", ""), out chknum) ? chknum : 0);
                chkendTime = (int.TryParse(_a005.END_HR_MNT.Replace(":", ""), out chknum) ? chknum : 0);
                if (chkstartTime > chkendTime)
                {
                    //24시 기준
                    chkendTime = chkendTime + 2400;
                }

                //심야
                if (startTime <= chkstartTime && endTime >= chkendTime)
                {
                    _imsiVo.N4TH_TM_VAL = ((chkendTime - chkstartTime) / 100) * (float.TryParse(_a005.WRK_TP_VAL.ToString(), out chkfnum) ? chkfnum : 0);

                    //30분 계산 : 소수점 0.5 더하기
                    string result = (chkendTime - chkstartTime).ToString();
                    if (result.Length > 2)
                    {
                        result = result.ToString().Substring(result.Length - 2, result.Length - 1);
                        if (Convert.ToInt32(result) >= 30)
                        {
                            _imsiVo.N4TH_TM_VAL = Convert.ToDouble(_imsiVo.N4TH_TM_VAL) + 0.5;
                        }
                    }
                }
                else
                {
                    imsiStartTime = ((chkendTime - chkstartTime) / 100);
                    imsiEndTime = 0;
                    if (startTime > chkstartTime)
                    {
                        imsiStartTime = ((chkendTime - startTime) / 100);
                    }

                    if (endTime < chkendTime)
                    {
                        imsiEndTime = ((chkendTime / 100) - (endTime / 100));
                    }
                    _imsiVo.N4TH_TM_VAL = (imsiStartTime - imsiEndTime) * (float.TryParse(_a005.WRK_TP_VAL.ToString(), out chkfnum) ? chkfnum : 0);

                    //30분 계산 : 소수점 0.5 더하기
                    string result = (imsiStartTime - imsiEndTime).ToString();
                    if (result.Length > 2)
                    {
                        result = result.ToString().Substring(result.Length - 2, result.Length - 1);
                        if (Convert.ToInt32(result) >= 30)
                        {
                            _imsiVo.N4TH_TM_VAL = Convert.ToDouble(_imsiVo.N4TH_TM_VAL) + 0.5;
                        }
                    }
                }

                //
                //비교 범위 (휴식 시간 체크 -1)
                chkstartTime = (int.TryParse(_a006.ST_HR_MNT.Replace(":", ""), out chknum) ? chknum : 0);
                chkendTime = (int.TryParse(_a006.END_HR_MNT.Replace(":", ""), out chknum) ? chknum : 0);

                //24시 기준
                chkstartTime = chkstartTime + 2400;
                chkendTime = chkendTime + 2400;

                if (startTime <= chkstartTime && endTime >= chkendTime)
                {
                    //심야(기본 시간) - (휴식 시간 체크 -1)
                    _imsiVo.N1ST_TM_VAL = Convert.ToInt16(_imsiVo.N1ST_TM_VAL) + (float.TryParse(_a006.WRK_TP_VAL.ToString(), out chkfnum) ? chkfnum : 0);

                    //심야시간 - (휴식 시간 체크 -1)
                    _imsiVo.N4TH_TM_VAL = Convert.ToDouble(_imsiVo.N4TH_TM_VAL) + (float.TryParse(_a006.WRK_TP_VAL.ToString(), out chkfnum) ? chkfnum : 0);
                }

                // 마이너스 이면 0으로 변경
                _imsiVo.N1ST_TM_VAL = ((Convert.ToInt16(_imsiVo.N1ST_TM_VAL) <= -1) ? 0 : _imsiVo.N1ST_TM_VAL);
                // 마이너스 이면 0으로 변경
                _imsiVo.N4TH_TM_VAL = ((Convert.ToDouble(_imsiVo.N4TH_TM_VAL) <= -1) ? 0 : _imsiVo.N4TH_TM_VAL);
            }
            else
            {
                _imsiVo.WRK_GRP_CD = "A001";
                _imsiVo.WRK_GRP_NM = "주간";

                //주간 계산
                //ST_HR_MNT, END_HR_MNT, WRK_TP_VAL
                #region 주간 계산
                //비교 범위
                chkstartTime = (int.TryParse(_a001.ST_HR_MNT.Replace(":", ""), out chknum) ? chknum : 0);
                chkendTime = (int.TryParse(_a001.END_HR_MNT.Replace(":", ""), out chknum) ? chknum : 0);
                //비교 값
                startTime = (int.TryParse(_imsiVo.ST_TM_NO.Replace(":", ""), out chknum) ? chknum : 0);
                endTime = (int.TryParse(_imsiVo.END_TM_NO.Replace(":", ""), out chknum) ? chknum : 0);

                if (startTime <= chkstartTime && endTime >= chkendTime)
                {
                    _imsiVo.N1ST_TM_VAL = ((chkendTime - chkstartTime) / 100) * (float.TryParse(_a001.WRK_TP_VAL.ToString(), out chkfnum) ? chkfnum : 0);
                }
                else
                {
                    imsiStartTime = ((chkendTime - chkstartTime) / 100);
                    imsiEndTime = 0;
                    if (startTime > chkstartTime)
                    {
                        imsiStartTime = ((chkendTime - startTime) / 100);
                    }

                    if (endTime < chkendTime)
                    {
                        imsiEndTime = ((chkendTime / 100) - (endTime / 100));
                    }
                    _imsiVo.N1ST_TM_VAL = (imsiStartTime - imsiEndTime) * (float.TryParse(_a001.WRK_TP_VAL.ToString(), out chkfnum) ? chkfnum : 0);

                    // 마이너스 이면 0으로 변경
                    _imsiVo.N1ST_TM_VAL = ((Convert.ToInt16(_imsiVo.N1ST_TM_VAL) <= -1) ? 0 : _imsiVo.N1ST_TM_VAL);
                }
                #endregion
            }


        }

    }
}
