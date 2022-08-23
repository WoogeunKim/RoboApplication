using AquilaErpWpfApp3.Util;
using AquilaErpWpfApp3.View.TEC.Report;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using DevExpress.XtraReports.UI;
using ModelsLibrary.Tec;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;

namespace AquilaErpWpfApp3.View.TEC.Dialog
{
    /// <summary>
    /// Interaction logic for ConfigView1Dialog.xaml
    /// </summary>
    public partial class T8813DetailDialog : DXWindow
    {
        //private static CodeServiceClient codeClient = SystemProperties.CodeClient;
        //private TecVo orgDao;
        //private bool isEdit = false;
        //private SystemCodeVo updateDao;
        //private IList<TecVo> ChekList;
        private string _title = "부자재 품질검사";

        private T8813LabelReport reportLabel;

        public T8813DetailDialog()
        {
            InitializeComponent();
            //

            this.text_M_FM_DT.Text = System.DateTime.Now.ToString("yyyy-MM-dd");
            this.text_M_TO_DT.Text = System.DateTime.Now.ToString("yyyy-MM-dd");


            //Refresh();


            this.OKButton.IsEnabled = false;
            this.LabelButton.IsEnabled = false;


            this.SearchButton.Click += SearchButton_Click;
            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
            this.OKButton.Click += new RoutedEventHandler(OKButton_Click);
            this.CancelButton.Click += new RoutedEventHandler(CancelButton_Click);

            this.LabelButton.Click += LabelButton_Click;


            this.ViewGridMst.SelectedItemChanged += ViewGridMst_SelectedItemChanged;
        }

        private void ViewGridMst_SelectedItemChanged(object sender, DevExpress.Xpf.Grid.SelectedItemChangedEventArgs e)
        {
            if (this.ViewGridMst.SelectedItems.Count > 0)
            {

                this.OKButton.IsEnabled = true;
                this.LabelButton.IsEnabled = true;
            }
            else
            {

                this.OKButton.IsEnabled = false;
                this.LabelButton.IsEnabled = false;
            }
        }

        private void LabelButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<TecVo> _tmpItems = this.ViewGridMst.SelectedItems as List<TecVo>;

                if (_tmpItems.Count == 0)
                {
                    return;
                }


                if( Convert.ToInt32(this.text_Label_Cnt.Text) > 0)
                {
                    List<TecVo> _reportItems = new List<TecVo>();


                    foreach (TecVo _item in _tmpItems)
                    {
                        for (int x = 0; x < Convert.ToInt32(this.text_Label_Cnt.Text); x++)
                        {
                            _reportItems.Add(_item);
                        }
                    }

                    reportLabel = new T8813LabelReport(_reportItems);

                    var window = new DevExpress.Xpf.Printing.DocumentPreviewWindow();

                    window.PreviewControl.DocumentSource = reportLabel;
                    reportLabel.CreateDocument(true);
                    window.Owner = Application.Current.MainWindow;
                    window.ShowDialog();
                }

            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }


        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {

            Refresh();


        }


        //private void Text_MTRL_EXP_DT_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        //{
        //    try
        //    {
        //        DateTime T1 = DateTime.Parse(this.text_MTRL_MAKE_DT.Text);
        //        DateTime T2 = DateTime.Parse(this.text_MTRL_EXP_DT.Text);

        //        //
        //        TimeSpan TS = T2 - T1;
        //        this.text_MTRL_EXP_DY.Text = ("" + TS.Days);
        //    }
        //    catch
        //    {
        //        return;
        //    }
        //}

        #region Functon (OKButton_Click, CancelButton_Click)
        private async void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValueCheckd())
            {
                MessageBoxResult result = WinUIMessageBox.Show("[ 총 :" + this.ViewGridMst.SelectedItems.Count + "개 / (" + this.text_Barcode_Cnt.Text + "장) ] 정말로 바코드 하시겠습니까?", "[바코드 - 출력]" + _title, MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    //WinUIMessageBox.Show(this.ViewGridMst.SelectedItems.Count + "", _title, MessageBoxButton.OK, MessageBoxImage.Information);
                    T8813BarCodeReport BarCodeReport = new T8813BarCodeReport(this.ViewGridMst.SelectedItems as List<TecVo>);
                    //var window = new DevExpress.Xpf.Printing.DocumentPreviewWindow();
                    //window.PreviewControl.DocumentSource = BarCodeReport;
                    ReportPrintTool printTool = new ReportPrintTool(BarCodeReport);
                    printTool.PrinterSettings.Copies = Convert.ToInt16(this.text_Barcode_Cnt.Text);
                    printTool.PrintingSystem.ShowPrintStatusDialog = false;
                    printTool.PrintingSystem.ShowMarginsWarning = false;

                    printTool.Print(Properties.Settings.Default.str_PrnNm);


                    //성공
                    WinUIMessageBox.Show("완료 되었습니다", _title, MessageBoxButton.OK, MessageBoxImage.Information);
                }

                //int _Num = 0;
                ////SystemCodeVo resultVo;
                //if (isEdit == false)
                //{
                //    this.updateDao = getDomain();

                //    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s131/mst/i", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
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
                //else
                //{
                //this.updateDao = getDomain();

                //    foreach (TecVo item in this.ChekList)
                //    {
                //        //item.MTRL_MAKE_DT = Convert.ToDateTime(this.text_MTRL_MAKE_DT.Text).ToString("yyyy-MM-dd");
                //        //item.MTRL_EXP_DT = Convert.ToDateTime(this.text_MTRL_EXP_DT.).ToString("yyyy-MM-dd");
                //        //Dao.ITM_IN_DT = Convert.ToDateTime(ITM_IN_DT).ToString("yyyy-MM-dd");

                //        item.BATCH_CD = this.text_BATCH_CD.Text;

                //        //Dao.INSP_NO = INSP_NO;
                //        //item.MTRL_EXP_DY = this.text_MTRL_EXP_DY;

                //        item.MTRL_MAKE_DT = Convert.ToDateTime(this.text_MTRL_MAKE_DT.Text).ToString("yyyy-MM-dd");
                //        item.MTRL_EXP_DY = 1095;

                //        if (string.IsNullOrEmpty(this.text_INSP_DT.Text))
                //        {
                //            item.INSP_DT = null;
                //        }
                //        else
                //        {
                //            item.INSP_DT = Convert.ToDateTime(this.text_INSP_DT.Text).ToString("yyyy-MM-dd");
                //        }

                //        item.INSP_FLG = (this.combo_INSP_FLG.Text.Equals("시험중") ? "Z" : (this.combo_INSP_FLG.Text.Equals("적합") ? "Y" : "N"));
                //        item.ITM_RMK = this.text_ITM_RMK.Text;

                //        //item.INSP_QTY = this.text_INSP_QTY.Text;
                //        //item.BAD_QTY = this.text_BAD_QTY.Text;

                //        //item.INAUD_TMP_NO = INAUD_TMP_NO;
                //        //item.INAUD_TMP_SEQ = INAUD_TMP_SEQ;
                //        item.CRE_USR_ID = SystemProperties.USER_VO.USR_ID;
                //        item.UPD_USR_ID = SystemProperties.USER_VO.USR_ID;
                //        item.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
                //    }

                //    //
                //    //
                //    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("t8813/m", new StringContent(JsonConvert.SerializeObject(this.ChekList), System.Text.Encoding.UTF8, "application/json")))
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
                //            this.DialogResult = true;
                //            this.Close();
                //        }
                //    }
                //    //
                //    //


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
            //if (string.IsNullOrEmpty(this.text_BATCH_CD.Text))
            //{
            //    //UOM_CD = new CodeDao() { CLSS_DESC = ITM_NM.UOM_NM };
            //    //MTRL_MAKE_DT = Convert.ToDateTime(MTRL_MAKE_DT).AddDays(MTRL_EXP_DY).ToString("yyyy-MM-dd");
            //    WinUIMessageBox.Show("[배치코드] 입력 값이 맞지 않습니다", "[유효검사] " + this._title, MessageBoxButton.OK, MessageBoxImage.Warning);
            //    //MSG = "[배치코드] 입력 값이 맞지 않습니다";
            //    return false;
            //}

            //if (string.IsNullOrEmpty(MTRL_MAKE_DT))
            //{
            //    //UOM_CD = new CodeDao() { CLSS_DESC = ITM_NM.UOM_NM };
            //    //MTRL_MAKE_DT = Convert.ToDateTime(MTRL_MAKE_DT).AddDays(MTRL_EXP_DY).ToString("yyyy-MM-dd");
            //    //WinUIMessageBox.Show("[구분] 입력 값이 맞지 않습니다.", "[유효검사] 시스템 분류 코드", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    MSG = "[제조 일자] 입력 값이 맞지 않습니다";
            //    return false;
            //}


            //if (string.IsNullOrEmpty(MTRL_EXP_DY + ""))
            //{
            //    //UOM_CD = new CodeDao() { CLSS_DESC = ITM_NM.UOM_NM };
            //    //MTRL_MAKE_DT = Convert.ToDateTime(MTRL_MAKE_DT).AddDays(MTRL_EXP_DY).ToString("yyyy-MM-dd");
            //    //WinUIMessageBox.Show("[구분] 입력 값이 맞지 않습니다.", "[유효검사] 시스템 분류 코드", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    MSG = "[유효기간(일수)] 입력 값이 맞지 않습니다";
            //    return false;
            //}
            //else
            //{
            //    MTRL_EXP_DT = Convert.ToDateTime(MTRL_MAKE_DT).AddDays(MTRL_EXP_DY).ToString("yyyy-MM-dd");
            //}



            //if (string.IsNullOrEmpty(INSP_NO))
            //{
            //    //UOM_CD = new CodeDao() { CLSS_DESC = ITM_NM.UOM_NM };
            //    //MTRL_MAKE_DT = Convert.ToDateTime(MTRL_MAKE_DT).AddDays(MTRL_EXP_DY).ToString("yyyy-MM-dd");
            //    //WinUIMessageBox.Show("[구분] 입력 값이 맞지 않습니다.", "[유효검사] 시스템 분류 코드", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    MSG = "[시험 번호] 입력 값이 맞지 않습니다";
            //    return false;
            //}

            //if (string.IsNullOrEmpty(this.text_INSP_DT.Text))
            //{
            //    //UOM_CD = new CodeDao() { CLSS_DESC = ITM_NM.UOM_NM };
            //    //MTRL_MAKE_DT = Convert.ToDateTime(MTRL_MAKE_DT).AddDays(MTRL_EXP_DY).ToString("yyyy-MM-dd");
            //    WinUIMessageBox.Show("[판정 일자] 입력 값이 맞지 않습니다", "[유효검사] " + this._title, MessageBoxButton.OK, MessageBoxImage.Warning);
            //    //MSG = "[판정 일자] 입력 값이 맞지 않습니다";
            //    return false;
            //}



            //if (string.IsNullOrEmpty(this.combo_INSP_FLG.Text))
            //{
            //    //UOM_CD = new CodeDao() { CLSS_DESC = ITM_NM.UOM_NM };
            //    //MTRL_MAKE_DT = Convert.ToDateTime(MTRL_MAKE_DT).AddDays(MTRL_EXP_DY).ToString("yyyy-MM-dd");
            //    WinUIMessageBox.Show("[판정] 입력 값이 맞지 않습니다", "[유효검사] " + this._title, MessageBoxButton.OK, MessageBoxImage.Warning);
            //    //MSG = "[판정] 입력 값이 맞지 않습니다";
            //    return false;
            //}


            return true;
        }
        #endregion

        //#region Functon (getDomain - ConfigView1Dao)
        //private TecVo getDomain()
        //{
        //    TecVo Dao = new TecVo();
        //    //Dao.CLSS_TP_CD = this.text_ClssTpCd.Text;
        //    //Dao.CLSS_TP_NM = this.text_ClssTpNm.Text;
        //    //Dao.SYS_FLG = this.text_SysFlg.Text;
        //    //Dao.SYS_AREA_CD = this.text_SysAreaCd.Text;
        //    //Dao.CLSS_CD_DESC = this.text_ClssCdDesc.Text;
        //    //Dao.DELT_FLG = (this.combo_deltFlg.Text.Equals("사용") ? "N" : "Y");
        //    //Dao.USR_ID = SystemProperties.USER;
        //    Dao.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
        //    return Dao;
        //}
        //#endregion

        //Vo
        //public SystemCodeVo resultDomain
        //{
        //    get
        //    {
        //        return this.updateDao;
        //    }
        //}


        public async void Refresh()
        {
            try
            {

                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("t8813/dtl", new StringContent(JsonConvert.SerializeObject(new TecVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD, FM_DT = Convert.ToDateTime(this.text_M_FM_DT.Text).ToString("yyyy-MM-dd"), TO_DT = Convert.ToDateTime(this.text_M_TO_DT.Text).ToString("yyyy-MM-dd")}), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.ViewGridMst.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<TecVo>>(await response.Content.ReadAsStringAsync()).Cast<TecVo>().ToList();
                    }

                    //
                    this.ViewGridMst.SelectedItems = new List<TecVo>();
                }
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }



    }
}
