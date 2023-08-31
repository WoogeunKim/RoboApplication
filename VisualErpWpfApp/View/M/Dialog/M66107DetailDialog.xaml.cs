﻿using DevExpress.Data;
using DevExpress.Mvvm.Native;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
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
using System.Threading.Tasks;
//using System.Linq;

namespace AquilaErpWpfApp3.View.M.Dialog
{
    /// <summary>
    /// M66107DetailDialog.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class M66107DetailDialog : DXWindow
    {
        private ManVo orgVo;
        private string _title = "Loss 최적화 수행";
        private ManVo updateDao;


        public M66107DetailDialog(ManVo vo)
        {
            InitializeComponent();

            this.orgVo = vo;

            SYSTEM_CODE_VO();

            this.Refresh.Click += new RoutedEventHandler(Refresh_Click);
            this.OKButton.Click += new RoutedEventHandler(OKButton_Click);
            this.CancelButton.Click += new RoutedEventHandler(CancelButton_Click);

            this.OKButton.IsEnabled = false;

        }

        #region Functon (getDomain - ConfigView1Dao)
        private ManVo getDomain()
        {
            ManVo vo = new ManVo();

            //vo.CNTR_PSN_NM = getRemoveWhiteSpaces(this.text_CNTR_PSN_NM.Text);
            //vo.CNTR_PSN_NM = (this.text_CNTR_PSN_NM.Text).Trim();
            if (vo.CNTR_PSN_NM == "") vo.CNTR_PSN_NM = null;

            SystemCodeVo CO_NM = this.combo_CO_NM.SelectedItem as SystemCodeVo;
            vo.CO_CD = CO_NM.CO_NO;
            vo.CO_NM = CO_NM.CO_NM;

            vo.OPMZ_NO = orgVo.OPMZ_NO;
            vo.UPD_USR_ID = SystemProperties.USER;
            vo.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;

            return vo;
        }

        //public static string getRemoveWhiteSpaces(string str)
        //{
        //    return string.Concat(str.Where(c => !Char.IsWhiteSpace(c)));
        //}
        #endregion

        #region Functon (ValueCheckd)
        public Boolean ValueCheckd()
        {
            if (string.IsNullOrEmpty(this.combo_CO_NM.Text))
            {
                WinUIMessageBox.Show("고객사가 선택되지 않았습니다.", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.combo_CO_NM.IsTabStop = true;
                this.combo_CO_NM.Focus();
                return false;
            }

            return true;
        }
        #endregion

        #region Functon (Refresh_Click, OKButton_Click, CancelButton_Click)
        private async void Refresh_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ValueCheckd())
                {
                    if (DXSplashScreen.IsActive == false)
                    {
                        DXSplashScreen.Show<ProgressWindow>();
                    }

                    this.updateDao = getDomain();

                    this.search_title.Text = "[조회 조건]   " + "거래처 : " + updateDao.CO_NM + "  공사부위 : " + updateDao.CNTR_PSN_NM;

                    this.ViewJOB_ITEMEdit.ItemsSource = await PostMstItems("m66107/dtl/dia", this.updateDao);
                }

                if (DXSplashScreen.IsActive == true)
                {
                    DXSplashScreen.Close();
                }
            }
            catch (System.Exception eLog)
            {
                if (DXSplashScreen.IsActive == true)
                {
                    DXSplashScreen.Close();
                }

                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }

        private async Task<List<ManVo>> PostMstItems(string Path, object obj)
        {
            List<ManVo> ret = new List<ManVo>();

            try
            {
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m66107/dtl/dia", new StringContent(JsonConvert.SerializeObject(obj), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        ret = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
                        if (this.orgVo.RUN_CLSS_CD != null)
                        {
                            if (this.orgVo.RUN_CLSS_CD.Equals("B"))
                            {
                                ret.Where(x => x.COLR_FLG.Equals("B")).ToList().ForEach(x => x.COLR_FLG = "A");
                            }
                        }
                    }
                }
            }
            catch (Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
            }

            return ret;


        }

        private async void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<ManVo> checkList = (this.ViewJOB_ITEMEdit.ItemsSource as IList<ManVo>).Where(x => x.isCheckd == true).ToList<ManVo>();


                if (checkList.Count <= 0)
                {
                    WinUIMessageBox.Show("데이터가 존재 하지 않습니다.", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                for(int i = 0; i< checkList.Count - 1; i++)
                {
                    for(int j= i + 1; j < checkList.Count; j++)
                    {
                        if(checkList[i].PROD_DIR_DT.ToString() != checkList[j].PROD_DIR_DT.ToString())
                        {
                            WinUIMessageBox.Show("생산계획일자는 같은 일자끼리만 저장할 수 있습니다.", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }
                    }
                }

                MessageBoxResult result = WinUIMessageBox.Show("정말로 저장 하시겠습니까?", "[저장]" + _title, MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    int _Num = 0;
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m66107/dtl/dia/i", new StringContent(JsonConvert.SerializeObject(checkList), System.Text.Encoding.UTF8, "application/json")))
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

                            this.DialogResult = true;
                            this.Close();
                        }
                    }
                }
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
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

        private void PART_Editor_Checked(object sender, RoutedEventArgs e)
        {
            this.OKButton.IsEnabled = true;
        }

        private void CheckEdit_Checked(object sender, RoutedEventArgs e)
        {
            CheckEdit checkEdit = sender as CheckEdit;
            ManVo tmpImsi;
            for (int x = 0; x < this.ViewJOB_ITEMEdit.VisibleRowCount; x++)
            {
                int rowHandle = this.ViewJOB_ITEMEdit.GetRowHandleByVisibleIndex(x);
                if (rowHandle > -1)
                {
                    tmpImsi = this.ViewJOB_ITEMEdit.GetRow(rowHandle) as ManVo;
                    if (checkEdit.IsChecked == true)
                    {
                        tmpImsi.isCheckd = true;
                        //
                        this.OKButton.IsEnabled = true;
                    }
                    else
                    {
                        tmpImsi.isCheckd = false;
                    }

                }
            }
        }

        private void viewPage1EditView_HiddenEditor(object sender, EditorEventArgs e)
        {
            this.viewJOB_ITEMView.CommitEditing();
        }

        public async void SYSTEM_CODE_VO()
        {
            try
            {
                if (DXSplashScreen.IsActive == false) DXSplashScreen.Show<ProgressWindow>();

                // 고객사 조회
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s143", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { DELT_FLG = "N", CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.combo_CO_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                    }
                }

                //// 메인화면 조회
                //using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m66107/dtl/dia", new StringContent(JsonConvert.SerializeObject(new ManVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD, OPMZ_NO = orgVo.OPMZ_NO, UPD_USR_ID = SystemProperties.USER }), System.Text.Encoding.UTF8, "application/json")))
                //{
                //    if (response.IsSuccessStatusCode)
                //    {
                //        this.ViewJOB_ITEMEdit.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
                //    }
                //}

                if (DXSplashScreen.IsActive == true) DXSplashScreen.Close();
            }
            catch (System.Exception eLog)
            {
                if (DXSplashScreen.IsActive == true) DXSplashScreen.Close();
                //DXSplashScreen.Close();
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }
    }
}
