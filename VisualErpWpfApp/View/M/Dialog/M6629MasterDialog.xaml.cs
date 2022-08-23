using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Code;
using ModelsLibrary.Man;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;
using AquilaErpWpfApp3.Util;

namespace AquilaErpWpfApp3.M.View.Dialog
{
    /// <summary>
    /// Interaction logic for ConfigView1Dialog.xaml
    /// </summary>
    public partial class M6629MasterDialog : DXWindow
    {
        //private static CodeServiceClient codeClient = SystemProperties.CodeClient;
        private ManVo orgDao;
        private bool isEdit = false;
        private ManVo updateDao;
        private string _title = "공정유형맵핑";

        private string clssTpCd = "";

        public M6629MasterDialog(ManVo Dao)
        {
            InitializeComponent();
            //
            this.orgDao = Dao;
            ManVo copyDao = new ManVo()
            {
                ROUT_TP_CD = Dao.ROUT_TP_CD,
                ROUT_TP_NM = Dao.ROUT_TP_NM,
                ROUT_TP_DESC = Dao.ROUT_TP_DESC,
                CHNL_CD = Dao.CHNL_CD,
                CRE_USR_ID = Dao.CRE_USR_ID,
                UPD_USR_ID = Dao.UPD_USR_ID
            };

            if (Dao.ROUT_TP_CD != null)
            {
                this.isEdit = true;
                this.text_ROUT_TP_CD.IsReadOnly = true;
            }
            else
            {
                this.isEdit = false;
            }
            this.configCode.DataContext = copyDao;
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
                    this.updateDao = getDomain();
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6629/mst/i", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
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
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6629/mst/u", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
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
            if (string.IsNullOrEmpty(this.text_ROUT_TP_CD.Text))
            {
                WinUIMessageBox.Show("[코드] 입력 값이 맞지 않습니다.", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.text_ROUT_TP_CD.IsTabStop = true;
                this.text_ROUT_TP_CD.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(this.text_ROUT_TP_NM.Text))
            {
                WinUIMessageBox.Show("[명칭] 입력 값이 맞지 않습니다.", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.text_ROUT_TP_CD.IsTabStop = true;
                this.text_ROUT_TP_CD.Focus();
                return false;
            }
            else
            {
                //if (this.isEdit == false)
                //{
                //    SystemCodeVo dao = new SystemCodeVo()
                //    {
                //        CLSS_TP_CD = this.clssTpCd
                //        ,
                //        CLSS_CD = this.text_ClssCd.Text
                //    };
                //    //ObservableCollection<SystemCodeVo> daoList = service.SearchDetailConfigView1(dao);
                //    //IList<SystemCodeVo> daoList = (IList<SystemCodeVo>)codeClient.SelectDetailCode(dao);
                //    //if (daoList.Count != 0)
                //    //{
                //    //    WinUIMessageBox.Show("[코드 - 중복] 코드를 다시 입력 하십시오.", "[중복검사] " + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                //    //    this.text_ClssCd.IsTabStop = true;
                //    //    this.text_ClssCd.Focus();
                //    //    return false;
                //    //}
                //}
            }
            return true;
        }
        #endregion

        #region Functon (getDomain - ConfigView1Dao)
        public ManVo getDomain()
        {
            ManVo Dao = new ManVo();
            Dao.ROUT_TP_CD = this.text_ROUT_TP_CD.Text;
            Dao.ROUT_TP_NM = this.text_ROUT_TP_NM.Text;
            Dao.ROUT_TP_DESC = this.text_ROUT_TP_DESC.Text;

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
