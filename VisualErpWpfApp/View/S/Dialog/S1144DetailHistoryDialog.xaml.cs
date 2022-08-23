using DevExpress.Xpf.Core;
using ModelsLibrary.Code;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;
using AquilaErpWpfApp3.Util;

namespace AquilaErpWpfApp3.View.S.Dialog
{
    /// <summary>
    /// Interaction logic for ConfigView1Dialog.xaml
    /// </summary>
    public partial class S1144DetailHistoryDialog : DXWindow
    {
        //private static CodeServiceClient codeClient = SystemProperties.CodeClient;
        //private Dictionary<string, string> coTpCdMap = new Dictionary<string, string>();
        private IList<SystemCodeVo> totalItem;
        private SystemCodeVo orgDao;

        private string title = "이력관리";

        public S1144DetailHistoryDialog(SystemCodeVo Dao)
        {
            InitializeComponent();

            this.orgDao = Dao;
            this.txt_stDate.Text = System.DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd");
            this.txt_enDate.Text = System.DateTime.Now.ToString("yyyy-MM-dd");
            //this.txt_Area.Text = Dao.AREA_NM;
            //this.coTpCdMap = SystemProperties.SYSTEM_CODE_MAP("S-004");
            //
            searchClass();
            //
            //this.btn_add.Click += btn_add_Click;
            //this.btn_del.Click += btn_del_Click;
            //this.btn_reset.Click += btn_reset_Click;
            //
            //this.text_ITM_NM.Text = Dao.ITM_NM;

            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
            //this.OKButton.Click += new RoutedEventHandler(OKButton_Click);
            //this.CancelButton.Click += new RoutedEventHandler(CancelButton_Click);
        }

        void btn_reset_Click(object sender, RoutedEventArgs e)
        {
            searchClass();
        }

     
        #region Functon (OKButton_Click, CancelButton_Click)
        //private void OKButton_Click(object sender, RoutedEventArgs e)
        //{
        //    SystemCodeVo resultVo;
        //    int nCnt = this.totalItem.Count;
        //    if (nCnt <= 0)
        //    {
        //        WinUIMessageBox.Show("데이터가 존재 하지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
        //        return;
        //    }

        //    //삭제
        //    resultVo = codeClient.S1144SelectHistList(new SystemCodeVo() { ITM_CD = this.orgDao.ITM_CD, AREA_CD = this.orgDao.AREA_CD });

        //    //추가
        //    for (int x = 0; x < nCnt; x++)
        //    {
        //        this.totalItem[x].ITM_CD = this.orgDao.ITM_CD;
        //        this.totalItem[x].AREA_CD = this.orgDao.AREA_CD;
        //        this.totalItem[x].CRE_USR_ID = SystemProperties.USER;
        //        this.totalItem[x].UPD_USR_ID = SystemProperties.USER;
        //        resultVo = codeClient.S1144InsertMst(this.totalItem[x]);
        //        if (!resultVo.isSuccess)
        //        {
        //            //실패
        //            WinUIMessageBox.Show(resultVo.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
        //            return;
        //        }
        //    }
        //    WinUIMessageBox.Show("완료 되었습니다", "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Information);
        //    this.DialogResult = true;
        //    this.Close();
        //}

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

        private async void searchClass()
        {
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s1144/his", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { AREA_CD = orgDao.AREA_CD, FM_DT = Convert.ToDateTime(this.txt_stDate.Text).ToString("yyyy-MM-dd"), TO_DT = Convert.ToDateTime(this.txt_enDate.Text).ToString("yyyy-MM-dd"), ITM_CD = orgDao.ITM_CD, ITM_NM = orgDao.ITM_NM, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.totalItem = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                    this.ConfigViewPage1Edit_Master.ItemsSource = this.totalItem;
                }
            }
        }
    }
}
