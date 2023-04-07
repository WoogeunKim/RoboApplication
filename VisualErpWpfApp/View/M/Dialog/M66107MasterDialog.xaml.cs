using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Auth;
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
    /// <summary>
    /// M66107MasterDialog.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class M66107MasterDialog : DXWindow
    {
        private string _title = "Loss 최적화 수행";
        private ManVo orgDao;
        private bool isEdit = false;
        private ManVo updateDao;

        public M66107MasterDialog(ManVo Dao)
        {
            InitializeComponent();

            orgDao = new ManVo()
            {
                OPMZ_NO = Dao.OPMZ_NO,
                OPMZ_NM = Dao.OPMZ_NM,
                OPMZ_RMK = Dao.OPMZ_RMK,
                RUN_CLSS_CD = Dao.RUN_CLSS_CD
            };

            //수정
            if (Dao.OPMZ_NO != null)
            {
                this.isEdit = true;
                this.combo_RUN_CLSS_CD.IsReadOnly = true;
            }
            else
            {
                this.isEdit = false;
            }

            this.configCode.DataContext = orgDao;
            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
            this.OKButton.Click += new RoutedEventHandler(OKButton_Click);
            this.CancelButton.Click += new RoutedEventHandler(CancelButton_Click);
        }

        #region Functon (ValueCheckd)
        public Boolean ValueCheckd()
        {
            try
            {
                if (string.IsNullOrEmpty(this.combo_RUN_CLSS_CD.Text))
                {
                    WinUIMessageBox.Show("[분류]의 값을 선택하세요.", "[유효검사]", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }
                return true;
                
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return false;
            }
        }
        #endregion

        #region Functon (getDomain - ConfigView1Dao)
        private ManVo getDomain()
        {
            try
            {
                ManVo Dao = new ManVo();
                Dao = orgDao;

                //Dao.OPMZ_NO = isEdit == true ? orgDao.OPMZ_NO : text_OPMZ_NO.Text;
                this.OPMZ_NO = Dao.OPMZ_NO;

                Dao.OPMZ_RMK = orgDao.OPMZ_RMK;
                Dao.OPMZ_NM = orgDao.OPMZ_NM;
                Dao.RUN_CLSS_CD = (this.combo_RUN_CLSS_CD.Text.Equals("생산") ? "A" : "B");

                Dao.CRE_USR_ID = SystemProperties.USER;
                Dao.UPD_USR_ID = SystemProperties.USER;
                Dao.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;

                return Dao;

            }
            catch (Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return new ManVo();
            }
        }
            #endregion

            #region Functon (OKButton_Click, CancelButton_Click)
            private async void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ValueCheckd())
                {
                    int _Num = 0;
                    //ProgramVo resultVo;
                    if (isEdit == false)
                    {
                        this.updateDao = getDomain();//this.updateDao
                        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m66107/mst/i", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
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
                    }
                    else
                    {
                        this.updateDao = getDomain();

                        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m66107/mst/u", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
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
                    }
                    this.DialogResult = true;
                    this.Close();
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

        //private async void KeyMake_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        int _Num = 0;
        //        //ProgramVo resultVo;
        //        if (isEdit == false)
        //        {
        //            this.updateDao = getDomain();//this.updateDao
        //            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m66107/mst/key", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
        //            {
        //                if (response.IsSuccessStatusCode)
        //                {
        //                    this.text_OPMZ_NO.Text =  JsonConvert.DeserializeObject<string>(await response.Content.ReadAsStringAsync());
        //                }
        //            }
        //        }
        //    }
        //    catch (System.Exception eLog)
        //    {
        //        WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
        //        return;
        //    }
        //}
        #endregion

        private void HandleEsc(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.DialogResult = false;
                Close();
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

        public string OPMZ_NO
        {
            get;
            set;
        }

        
    }
}
