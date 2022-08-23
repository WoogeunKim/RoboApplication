using AquilaErpWpfApp3.Util;
using DevExpress.Spreadsheet;
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

namespace AquilaErpWpfApp3.View.M.Dialog
{
    /// <summary>
    /// Interaction logic for ConfigView1Dialog.xaml
    /// </summary>
    public partial class M6623DetailDialog : DXWindow
    {
        private string _title = "BULK/BOM";

        private List<ManVo> _saveVo = new List<ManVo>();
        public string _ASSY_ITM_CD = string.Empty;
        public int? _BSE_WEIH_VAL = 0;

        public IList<SystemCodeVo> _ITEMS;
        public IList<SystemCodeVo> _BULK;

        //private static CodeServiceClient codeClient = SystemProperties.CodeClient;
        //private static ManServiceClient manClient = SystemProperties.ManClient;

        //private ObservableCollection<SystemCodeVo> N1ST_ITM_GRP_LIST_1;
        //private ObservableCollection<SystemCodeVo> ASSY_ITM_LIST;

        //private ObservableCollection<SystemCodeVo> N1ST_ITM_GRP_LIST;
        //private ObservableCollection<SystemCodeVo> CMPO_LIST;

        //private ManVo orgDao;
        //private bool isEdit = false;
        // private ManVo updateDao;

        public M6623DetailDialog()
        {
            InitializeComponent();

            //this.configCode.DataContext = copyDao;
            //this.PreviewKeyDown += new KeyEventHandler(HandleEsc);

            //this.PasteButton.Click += new RoutedEventHandler(PasteButton_Click);

            this.text_BSE_WEIH_VAL.Text = "1";

            this.OKButton.Click += new RoutedEventHandler(OKButton_Click);
            this.CancelButton.Click += new RoutedEventHandler(CancelButton_Click);
        }

      
        #region Functon (OKButton_Click, CancelButton_Click)
        private async void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValueCheckd())
            {
                try
                {
                    DXSplashScreen.Show<ProgressWindow>();

                    SystemCodeVo _chkItem = new SystemCodeVo();
                    List<ManVo> _saveVo = new List<ManVo>();

                    //Excel 세팅
                    IWorkbook workbook = this.spreadsheetControl1.Document;
                    Worksheet worksheet = workbook.Worksheets.ActiveWorksheet;

                    int _nCnt = 8;
                    string _CMPO_RMK = string.Empty;

                    //벌크 코드
                    this._ASSY_ITM_CD = worksheet.Cells["E3"].Value.ToString();
                    //기준 중량
                    this._BSE_WEIH_VAL = int.Parse(this.text_BSE_WEIH_VAL.Text);


                    //
                    //원재료 구분
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s141", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { ITM_GRP_CLSS_CD = "M", CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            this._ITEMS = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                        }
                    }

                    //4번때 벌크 체크
                    if (this._ASSY_ITM_CD.StartsWith("4-"))
                    {
                        //Bulk 코드 체크
                        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s141", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { ITM_GRP_CLSS_CD = "W", CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                this._BULK = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                            }
                        }
                    }
                    else if (this._ASSY_ITM_CD.StartsWith("5-"))
                    {
                        //Bulk 코드 체크
                        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s141", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { ITM_GRP_CLSS_CD = "S", CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                this._BULK = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                            }
                        }
                    }

                  

                    //벌크 체크
                    if (this._BULK.Any<SystemCodeVo>(a => a.ITM_CD.Equals(this._ASSY_ITM_CD)) == false)
                    {
                        WinUIMessageBox.Show("[반 제 품 코 드 / 가 공 원 료] 입력 값이 맞지 않습니다.", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }


                    for (int x = 0; x < 50; x++)
                    {
                        ManVo manVo = new ManVo();
                        //
                        if (string.IsNullOrEmpty(worksheet.Cells["A" + (_nCnt + x)].Value.ToString()))
                        {
                            //if (string.IsNullOrEmpty(Environment.NewLine + workbook.Worksheets.ActiveWorksheet.Range["AC" + (_nCnt + x) + ":AT" + (_nCnt + x)].Value.ToString().Trim()))
                            if (string.IsNullOrEmpty(Environment.NewLine + workbook.Worksheets.ActiveWorksheet.Range["AC" + (_nCnt + x)].Value.ToString().Trim()))
                            {
                                continue;
                            }

                            //_CMPO_RMK += Environment.NewLine + workbook.Worksheets.ActiveWorksheet.Range["AC" + (_nCnt + x) + ":AT" + (_nCnt + x)].Value.ToString();
                            _CMPO_RMK += Environment.NewLine + workbook.Worksheets.ActiveWorksheet.Range["AC" + (_nCnt + x)].Value.ToString();
                            continue;
                        }

                        //순번
                        manVo.ASSY_ITM_SEQ = (x + 1);
                        //벌크 코드
                        manVo.ASSY_ITM_CD = worksheet.Cells["E3"].Value.ToString();
                        //기준 중량
                        manVo.BSE_WEIH_VAL = int.Parse(this.text_BSE_WEIH_VAL.Text);
                        //상 : ORD_CLS_CD
                        manVo.ORD_CLS_CD = worksheet.Cells["A" + (_nCnt + x)].Value.ToString();

                        //원료 코드 : CMPO_CD
                        manVo.CMPO_CD = worksheet.Cells["B" + (_nCnt + x)].Value.ToString();

                        //칭량 값 : WEIH_VAL * 1Kg
                        manVo.WEIH_VAL =  Convert.ToDecimal( worksheet.Cells["Z" + (_nCnt + x)].Value.ToString()) * 10;

                        if (this._ITEMS.Any<SystemCodeVo>(a => a.ITM_CD.Equals(manVo.CMPO_CD)))
                        {
                            _chkItem = this._ITEMS.Where<SystemCodeVo>(b => b.ITM_CD.Equals(manVo.CMPO_CD)).FirstOrDefault<SystemCodeVo>();
                            //
                            if (_chkItem.N1ST_ITM_GRP_CD.Equals("002"))
                            {
                                //색소
                                manVo.CMPO_TOR_VAL = "30";
                            }
                            else if (_chkItem.N1ST_ITM_GRP_CD.Equals("003"))
                            {
                                //단일원료
                                manVo.CMPO_TOR_VAL = "0.2";
                            }
                            else
                            {
                                //기타
                                manVo.CMPO_TOR_VAL = "0.5";
                            }
                        }

                        //제조 공정도 : CMPO_RMK
                        //manVo.CMPO_RMK = worksheet.Cells["AC" + (_nCnt + x)].Value.ToString();
                        manVo.CMPO_RMK = workbook.Worksheets.ActiveWorksheet.Range["AC" + (_nCnt + x)].Value.ToString();

                        manVo.CRE_USR_ID = SystemProperties.USER;
                        manVo.UPD_USR_ID = SystemProperties.USER;
                        manVo.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;

                        _saveVo.Add(manVo);
                    }

                    _saveVo[_saveVo.Count - 1].CMPO_RMK += _CMPO_RMK;


                    //WinUIMessageBox.Show(temp, _title, MessageBoxButton.OK, MessageBoxImage.Information);
                    //Dao.CRE_USR_ID = SystemProperties.USER;
                    //Dao.UPD_USR_ID = SystemProperties.USER;
                    //Dao.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
                    //_saveVo
                    int _Num = 0;
                    //
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6623/mst/m", new StringContent(JsonConvert.SerializeObject(_saveVo), System.Text.Encoding.UTF8, "application/json")))
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

                    //
                    DXSplashScreen.Close();
                    //성공
                    WinUIMessageBox.Show("완료 되었습니다", _title, MessageBoxButton.OK, MessageBoxImage.Information);
                    this.DialogResult = true;
                    this.Close();
                }
                catch(Exception eLog)
                {
                    if (DXSplashScreen.IsActive == true)
                    {
                        DXSplashScreen.Close();
                    }
                    WinUIMessageBox.Show(eLog.Message, _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        //private void HandleEsc(object sender, KeyEventArgs e)
        //{
        //    if (e.Key == Key.Escape)
        //    {
        //        this.DialogResult = false;
        //        Close();
        //    }
        //}
        #endregion

        #region Functon (ValueCheckd)
        public Boolean ValueCheckd()
        {
            if (string.IsNullOrEmpty(this.text_BSE_WEIH_VAL.Text))
            {
                WinUIMessageBox.Show("[기준 중량(Kg)] 입력 값이 맞지 않습니다.", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.text_BSE_WEIH_VAL.IsTabStop = true;
                this.text_BSE_WEIH_VAL.Focus();
                return false;
            }
            //else if (string.IsNullOrEmpty(this.text_ASSY_ITM_SEQ.Text))
            //{
            //    WinUIMessageBox.Show("[순번] 입력 값이 맞지 않습니다.", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.text_ASSY_ITM_SEQ.IsTabStop = true;
            //    this.text_ASSY_ITM_SEQ.Focus();
            //    return false;
            //}
            //else if (string.IsNullOrEmpty(this.combo_ASSY_ITM_CD.Text))
            //{
            //    WinUIMessageBox.Show("[물품 코드] 입력 값이 맞지 않습니다.", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.combo_ASSY_ITM_CD.IsTabStop = true;
            //    this.combo_ASSY_ITM_CD.Focus();
            //    return false;
            //}
            //else if (string.IsNullOrEmpty(this.combo_CMPO_CD.Text))
            //{
            //    WinUIMessageBox.Show("[원료] 입력 값이 맞지 않습니다.", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.combo_CMPO_CD.IsTabStop = true;
            //    this.combo_CMPO_CD.Focus();
            //    return false;
            //}
            //else if (string.IsNullOrEmpty(this.text_ORD_CLS_CD.Text))
            //{
            //    WinUIMessageBox.Show("[상] 입력 값이 맞지 않습니다.", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.text_ORD_CLS_CD.IsTabStop = true;
            //    this.text_ORD_CLS_CD.Focus();
            //    return false;
            //}
            ////else if (string.IsNullOrEmpty(this.combo_INAUD_PLC_NM.Text))
            ////{
            ////    WinUIMessageBox.Show("[입고처] 입력 값이 맞지 않습니다.", "[유효검사]자재 입고 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
            ////    this.combo_INAUD_PLC_NM.IsTabStop = true;
            ////    this.combo_INAUD_PLC_NM.Focus();
            ////    return false;
            ////}
            ////else if (string.IsNullOrEmpty(this.combo_RQST_EMPE_NM.Text))
            ////{
            ////    WinUIMessageBox.Show("[요청자] 입력 값이 맞지 않습니다.", "[유효검사]자재 입고 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
            ////    this.combo_RQST_EMPE_NM.IsTabStop = true;
            ////    this.combo_RQST_EMPE_NM.Focus();
            ////    return false;
            ////}
            ////else if (this.combo_deltFlg.Text == null || this.combo_deltFlg.Text.Trim().Length == 0)
            ////{
            ////    WinUIMessageBox.Show("[사용 여부] 입력 값이 맞지 않습니다.", "[유효검사]자재 입고 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
            ////    this.combo_deltFlg.IsTabStop = true;
            ////    this.combo_deltFlg.Focus();
            ////    return false;
            ////}
            //else
            //{
            //    //if (this.isEdit == false)
            //    //{
            //    //    ItmCodeDao ASSY_ITM_CD = this.combo_ASSY_ITM_CD.SelectedItem as ItmCodeDao;
            //    //    ManVo dao = new ManVo()
            //    //    {
            //    //        ASSY_ITM_CD = ASSY_ITM_CD.ITM_CD,
            //    //        BSE_WEIH_VAL = int.Parse(this.text_BSE_WEIH_VAL.Text),
            //    //        CHNL_CD = SystemProperties.USER_VO.CHNL_CD,
            //    //        ASSY_ITM_SEQ = int.Parse(this.text_ASSY_ITM_SEQ.Text)

            //    //    };
            //    //    IList<ManVo> daoList = (IList<ManVo>)manClient.M6623SelectDetail(dao);
            //    //    if (daoList.Count != 0)
            //    //    {
            //    //        WinUIMessageBox.Show("[배합표 No. - 중복] 코드를 다시 입력 하십시오.", "[중복검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
            //    //        this.text_BSE_WEIH_VAL.IsTabStop = true;
            //    //        this.text_BSE_WEIH_VAL.Focus();
            //    //        return false;
            //    //    }
            //    //}
            //}
            return true;
        }
        #endregion

    }
}
