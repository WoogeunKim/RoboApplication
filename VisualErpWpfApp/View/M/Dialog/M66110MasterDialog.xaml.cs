using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Code;
using ModelsLibrary.Man;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;
using AquilaErpWpfApp3.Util;

namespace AquilaErpWpfApp3.M.View.Dialog
{
    /// <summary>
    /// Interaction logic for ConfigView1Dialog.xaml
    /// </summary>
    public partial class M66110MasterDialog : DXWindow
    {
        //private static CodeServiceClient codeClient = SystemProperties.CodeClient;
        private ManVo orgDao;
        private bool isEdit = false;
        private ManVo updateDao;
        private string _title = "휴무일관리";

        //private string clssTpCd = "";


        public M66110MasterDialog(IList<ManVo> _Dates)
        {
            InitializeComponent();
            //
            //ManVo copyDao = new ManVo()
            //{
            //    EXPT_DT = Dao.EXPT_DT,
            //    EXPT_RMK = Dao.EXPT_RMK,
            //    CHNL_CD = Dao.CHNL_CD,
            //    CRE_USR_ID = Dao.CRE_USR_ID,
            //    UPD_USR_ID = Dao.UPD_USR_ID
            //};

            //if (Dao.EXPT_DT != null)
            //{
            //    this.isEdit = true;
                //this.text_ROUT_TP_CD.IsReadOnly = true;
            //}
            //else
            //{
                this.isEdit = false;
            //}
            //this.configCode.DataContext = copyDao;

            this.ViewGridMst.ItemsSource = _Dates;
            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
            this.OKButton.Click += new RoutedEventHandler(OKButton_Click);
            this.CancelButton.Click += new RoutedEventHandler(CancelButton_Click);
        }

        public M66110MasterDialog(ManVo Dao)
        {
            InitializeComponent();
            //
            this.orgDao = Dao;
            ManVo copyDao = new ManVo()
            {
                EXPT_DT = Dao.EXPT_DT,
                EXPT_RMK = Dao.EXPT_RMK,
                CHNL_CD = Dao.CHNL_CD,
                CRE_USR_ID = Dao.CRE_USR_ID,
                UPD_USR_ID = Dao.UPD_USR_ID
            };


            IList<ManVo> _Dates = new List<ManVo>();
            _Dates.Add(copyDao);

            //if (Dao.EXPT_DT != null)
            //{
                this.isEdit = true;
            //this.text_ROUT_TP_CD.IsReadOnly = true;
            //}
            //else
            //{
            //    this.isEdit = false;
            //}
            //this.configCode.DataContext = copyDao;
            this.ViewGridMst.ItemsSource = _Dates;
            this.text_EXPT_RMK.Text = copyDao.EXPT_RMK;

            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
            this.OKButton.Click += new RoutedEventHandler(OKButton_Click);
            this.CancelButton.Click += new RoutedEventHandler(CancelButton_Click);
        }

        #region Functon (OKButton_Click, CancelButton_Click)
        private async void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValueCheckd())
            {
                int _Num = 0;
                //SystemCodeVo resultVo;
                if (isEdit == false)
                {
                    IList<ManVo> _Dates =  this.ViewGridMst.ItemsSource as List<ManVo>;
                    foreach (ManVo item in _Dates)
                    {
                        item.EXPT_RMK = this.text_EXPT_RMK.Text;

                        item.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
                        item.CRE_USR_ID = SystemProperties.USER;
                        item.UPD_USR_ID = SystemProperties.USER;
                    }

                    //this.updateDao = getDomain();
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m66110/i", new StringContent(JsonConvert.SerializeObject(_Dates), System.Text.Encoding.UTF8, "application/json")))
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
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m66110/u", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
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
            ////if (string.IsNullOrEmpty(this.text_ROUT_TP_CD.Text))
            //{
            //    WinUIMessageBox.Show("[코드] 입력 값이 맞지 않습니다.", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
            //    //this.text_ROUT_TP_CD.IsTabStop = true;
            //    //this.text_ROUT_TP_CD.Focus();
            //    //return false;
            //}
            ////else if (string.IsNullOrEmpty(this.text_ROUT_TP_NM.Text))
            ////{
            //    WinUIMessageBox.Show("[명칭] 입력 값이 맞지 않습니다.", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
            //    //this.text_ROUT_TP_CD.IsTabStop = true;
            //    //this.text_ROUT_TP_CD.Focus();
            //    //return false;
            ////}
            ////else
            //{
            //    //if (this.isEdit == false)
            //    //{
            //    //    SystemCodeVo dao = new SystemCodeVo()
            //    //    {
            //    //        CLSS_TP_CD = this.clssTpCd
            //    //        ,
            //    //        CLSS_CD = this.text_ClssCd.Text
            //    //    };
            //    //    //ObservableCollection<SystemCodeVo> daoList = service.SearchDetailConfigView1(dao);
            //    //    //IList<SystemCodeVo> daoList = (IList<SystemCodeVo>)codeClient.SelectDetailCode(dao);
            //    //    //if (daoList.Count != 0)
            //    //    //{
            //    //    //    WinUIMessageBox.Show("[코드 - 중복] 코드를 다시 입력 하십시오.", "[중복검사] " + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
            //    //    //    this.text_ClssCd.IsTabStop = true;
            //    //    //    this.text_ClssCd.Focus();
            //    //    //    return false;
            //    //    //}
            //    //}
            //}
            return true;
        }
        #endregion

        #region Functon (getDomain - ConfigView1Dao)
        public ManVo getDomain()
        {
            ManVo Dao = new ManVo();
            Dao.EXPT_DT = (this.ViewGridMst.SelectedItem as ManVo).EXPT_DT;
            Dao.EXPT_RMK = this.text_EXPT_RMK.Text;

            //
            Dao.CRE_USR_ID = SystemProperties.USER;
            Dao.UPD_USR_ID = SystemProperties.USER;
            Dao.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
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


        public ManVo resultDomain
        {
            get
            {
                return this.updateDao;
            }
}
    }
}
