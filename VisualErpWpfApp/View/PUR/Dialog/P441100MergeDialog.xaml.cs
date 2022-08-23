using AquilaErpWpfApp3.Util;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Pur;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Windows;

namespace AquilaErpWpfApp3.View.PUR.Dialog
{
    public partial class P441100MergeDialog : DXWindow
    {
        //private static PurServiceClient purClient = SystemProperties.PurClient;
        //private PurVo orgDao;
        //private bool isEdit = false;
        //private PurVo updateDao;

        private string _title = "원자재 소요량 전개";

        public P441100MergeDialog(IList<PurVo> _selectMstList)
        {
            InitializeComponent();


            this.ViewGridMst.ItemsSource = _selectMstList.Where<PurVo>(x => string.IsNullOrEmpty(x.UN_FOL_NO) == false).ToList();
            //
            this.ViewGridMst.SelectionChanged += ViewGridMst_SelectionChanged;


            this.text_PUR_DT.Text = System.DateTime.Now.ToString("yyyy-MM-dd");
            this.text_PUR_DUE_DT.Text = System.DateTime.Now.ToString("yyyy-MM-dd");

            //this.configCode.DataContext = orgDao;
            //this.PreviewKeyDown += new KeyEventHandler(HandleEsc);

            this.ApplyButton.Click += ApplyButton_Click;

            this.OKButton.Click += new RoutedEventHandler(OKButton_Click);
            this.CancelButton.Click += new RoutedEventHandler(CancelButton_Click);
        }

        //소요량 취합
        private async void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.ViewGridMst.SelectedItems.Count > 0)
                {

                    DXSplashScreen.Show<ProgressWindow>();

                    PurVo _param = new PurVo();
                    _param.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
                    //_param.A_UN_FOL_NO = this.ViewGridMst.SelectedItems as 

                    IList<string> _A_UN_FOL_NO = new List<string>();
                    foreach (PurVo _item in this.ViewGridMst.SelectedItems)
                    {
                        _A_UN_FOL_NO.Add(_item.UN_FOL_NO);
                    }
                    _param.A_UN_FOL_NO = _A_UN_FOL_NO.ToArray();

                    //
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("p441100/marge", new StringContent(JsonConvert.SerializeObject(_param), System.Text.Encoding.UTF8, "application/json")))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            this.ViewGridDtl.ItemsSource = JsonConvert.DeserializeObject<List<PurVo>>(await response.Content.ReadAsStringAsync()).Cast<PurVo>().ToList();
                            this.ViewGridDtl.RefreshData();
                        }
                    }
                    DXSplashScreen.Close();


                    this.ViewGridDtl.SelectedItems = new List<PurVo>();
                }
            }
            catch (System.Exception eLog)
            {
                if (DXSplashScreen.IsActive)
                {
                    DXSplashScreen.Close();
                }
                //
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }

        private void ViewGridMst_SelectionChanged(object sender, DevExpress.Xpf.Grid.GridSelectionChangedEventArgs e)
        {
            //Grid  => Checkd   
            if (this.ViewGridMst.SelectedItems.Count > 0)
            {
                this.ApplyButton.IsEnabled = true;
                this.panel_dtl.IsEnabled = true;
                //
                this.OKButton.IsEnabled = true;
                this.text_PUR_DUE_DT.IsEnabled = true;
                this.text_PUR_DT.IsEnabled = true;
            }
            else
            {
                this.ApplyButton.IsEnabled = false;
                this.panel_dtl.IsEnabled = false;
                //
                this.OKButton.IsEnabled = false;
                this.text_PUR_DUE_DT.IsEnabled = false;
                this.text_PUR_DT.IsEnabled = false;
            }

            //
            this.lab_select.Text = "『총 전개번호 : " + this.ViewGridMst.SelectedItems.Count + "』";

        }

        //private async void SelectMstDetail()
        //{
        //    try
        //    {
        //        //DXSplashScreen.Show<ProgressWindow>();

        //        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("p4401/dtl", new StringContent(JsonConvert.SerializeObject(this.orgDao), System.Text.Encoding.UTF8, "application/json")))
        //        {
        //            if (response.IsSuccessStatusCode)
        //            {
        //                this.ViewGridDtl.ItemsSource = JsonConvert.DeserializeObject<List<PurVo>>(await response.Content.ReadAsStringAsync()).Cast<PurVo>().ToList();
        //                this.ViewGridDtl.RefreshData();
        //            }

        //        }
        //        //DXSplashScreen.Close();
        //    }
        //    catch (System.Exception eLog)
        //    {
        //        //DXSplashScreen.Close();
        //        //
        //        WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
        //        return;
        //    }
        //}


        #region Functon (OKButton_Click, CancelButton_Click)
        private async void OKButton_Click(object sender, RoutedEventArgs e)
        {
            //
            if(this.ViewGridDtl.SelectedItems.Count > 0)
            {
                if (string.IsNullOrEmpty(this.text_PUR_DT.Text))
                {
                    WinUIMessageBox.Show("[발주 일자] 입력 값이 맞지 않습니다.", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                else if (string.IsNullOrEmpty(this.text_PUR_DUE_DT.Text))
                {
                    WinUIMessageBox.Show("[납기일] 입력 값이 맞지 않습니다.", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }


                //
                string _gbn = "[자동 발주 : " + System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + " - " + SystemProperties.USER_VO.USR_N1ST_NM + "(" + SystemProperties.USER_VO.USR_ID + ")" + "]";
                if (WinUIMessageBox.Show("(" + _gbn + ")  자동 발주 하시겠습니까?", _title, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    //
                    List<PurVo> _purList = this.ViewGridDtl.SelectedItems as List<PurVo>;
                    _purList[0].PUR_DT = Convert.ToDateTime(this.text_PUR_DT.Text).ToString("yyyy-MM-dd");
                    _purList[0].PUR_DUE_DT = Convert.ToDateTime(this.text_PUR_DUE_DT.Text).ToString("yyyy-MM-dd");
                    _purList[0].GBN = _gbn;
                    _purList[0].UPD_USR_ID = SystemProperties.USER_VO.USR_ID;
                    _purList[0].CRE_USR_ID = SystemProperties.USER_VO.USR_ID;


                    //
                    //
                    int _Num = 0;
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("p441100/marge/m", new StringContent(JsonConvert.SerializeObject(_purList), System.Text.Encoding.UTF8, "application/json")))
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
                            WinUIMessageBox.Show("[" + _gbn + "] 완료 되었습니다", _title, MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }
            }
            else
            {
                WinUIMessageBox.Show("체크 사항이 존재 하지 않습니다", _title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }

            this.DialogResult = true;
            this.Close();


            //if (ValueCheckd())
            //{
            //    PurVo _selVo =this.ViewGridDtl.SelectedItem as PurVo;
            //    if (_selVo != null)
            //    {
            //        this.orgDao.CO_CD = _selVo.CO_NO;
            //        this.orgDao.CO_NM = _selVo.CO_NM;

            //        this.orgDao.ITM_PRC = _selVo.CO_UT_PRC;

            //        this.DialogResult = true;
            //        this.Close();
            //    }
            //}
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
            return true;
        }
        #endregion

        #region Functon (getDomain - ConfigView1Dao)
        private PurVo getDomain()
        {
            PurVo Dao = new PurVo();

            //Dao.PUR_ORD_NO = this.text_PUR_ORD_NO.Text;

            //SystemCodeVo areaVo = this.combo_AREA_CD.SelectedItem as SystemCodeVo;
            //Dao.AREA_CD = areaVo.CLSS_CD;
            //Dao.AREA_NM = areaVo.CLSS_DESC;

            //Dao.PUR_DT = Convert.ToDateTime(this.text_PUR_DT.Text).ToString("yyyy-MM-dd");
            //Dao.PUR_DUE_DT = Convert.ToDateTime(this.text_PUR_DUE_DT.Text).ToString("yyyy-MM-dd");

            //SystemCodeVo coNmVo = this.combo_CO_NO.SelectedItem as SystemCodeVo;
            //Dao.PUR_CO_CD = coNmVo.CO_NO;
            //Dao.CO_NO = coNmVo.CO_NO;
            //Dao.CO_NM = coNmVo.CO_NM;

            //SystemCodeVo itmVo = this.combo_PUR_ITM_NM.SelectedItem as SystemCodeVo;
            //Dao.PUR_ITM_CD = itmVo.CLSS_CD;
            //Dao.PUR_ITM_NM = itmVo.CLSS_DESC;


            ////UserCodeDao purEmpeIdVo = this.combo_PUR_EMPE_ID.SelectedItem as UserCodeDao;
            ////Dao.PUR_EMPE_ID = purEmpeIdVo.USR_ID;
            ////Dao.USR_NM = purEmpeIdVo.USR_N1ST_NM;

            //Dao.PUR_RMK = this.text_ORD_RMK.Text;

            //Dao.PUR_WK_CD = Convert.ToInt16(this.combo_PUR_WK_CD.Text);

            //Dao.PUR_CLZ_FLG = this.text_PUR_CLZ_FLG.Text;

            //Dao.CRE_USR_ID = SystemProperties.USER;
            //Dao.UPD_USR_ID = SystemProperties.USER;
            //Dao.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;

            return Dao;
        }
        #endregion

        //IsEdit
        //public bool IsEdit
        //{
        //    get
        //    {
        //        return this.isEdit;
        //    }
        //}


        //public async void SYSTEM_CODE_VO()
        //{
        //    //this.combo_AREA_CD.ItemsSource = SystemProperties.SYSTEM_CODE_VO("L-002");
        //    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "L-002"))
        //    {
        //        if (response.IsSuccessStatusCode)
        //        {
        //            this.combo_AREA_CD.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
        //        }
        //    }

        //    //this.combo_PUR_ITM_NM.ItemsSource = SystemProperties.SYSTEM_CODE_VO("L-111");
        //    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "L-111"))
        //    {
        //        if (response.IsSuccessStatusCode)
        //        {
        //            this.combo_PUR_ITM_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
        //        }
        //    }

        //    //this.combo_CO_NO.ItemsSource = SystemProperties.CUSTOMER_CODE_VO("AP", SystemProperties.USER_VO.EMPE_PLC_NM);
        //    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s143", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { DELT_FLG = "N", SEEK_AP = "AP", SEEK = "AP", CO_TP_CD = "AP", AREA_CD = SystemProperties.USER_VO.EMPE_PLC_NM, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
        //    {
        //        if (response.IsSuccessStatusCode)
        //        {
        //            this.combo_CO_NO.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
        //        }
        //    }
        //}

    }
}
