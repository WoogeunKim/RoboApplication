using AquilaErpWpfApp3.Util;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Man;
using ModelsLibrary.Pur;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;

namespace AquilaErpWpfApp3.View.M.Dialog
{
    public partial class M6630MasterDialog : DXWindow
    {

        private string _title = "칭량 작업 계획";

        //private static ManServiceClient manClient = SystemProperties.ManClient;
        private ManVo orgDao;
        private bool isEdit = false;
        //private ManVo updateDao;
        private List<ManVo> _saveList = new List<ManVo>();


        public M6630MasterDialog(ManVo Dao)
        {
            InitializeComponent();

            //
            this.orgDao = Dao;
            ManVo copyDao = new ManVo()
            {
                PROD_PLN_NO = Dao.PROD_PLN_NO,
                PROD_PLN_RMK = Dao.PROD_PLN_RMK,
                CRE_DT = Dao.CRE_DT,
                CHNL_CD = Dao.CHNL_CD,
                CRE_USR_ID = Dao.CRE_USR_ID,
                UPD_USR_ID = Dao.UPD_USR_ID,
                DELT_FLG = Dao.DELT_FLG
            };

            //
            this.text_FM_DT.Text = System.DateTime.Now.AddDays(-6).ToString("yyyy-MM-dd");
            this.text_TO_DT.Text = System.DateTime.Now.ToString("yyyy-MM-dd");

            //
            SYSTEM_CODE_VO();

            //
            //수정
            if (copyDao.PROD_PLN_NO != null)
            {
                this.isEdit = true;
                //this.text_PROD_PLN_RMK.Text = this.orgDao.PROD_PLN_RMK;
            }
            else
            {
                //추가
                this.isEdit = false;
            }
            this.configCode.DataContext = copyDao;
            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
            this.OKButton.Click += new RoutedEventHandler(OKButton_Click);
            this.CancelButton.Click += new RoutedEventHandler(CancelButton_Click);



            this.ViewGridMst.SelectedItemChanged += ViewGridMst_SelectedItemChanged;
            this.ViewGridDtl.SelectedItemChanged += ViewGridDtl_SelectedItemChanged;

            //추가 = 삭제
            this.AddButton.Click += AddButton_Click;
            this.DelButton.Click += DelButton_Click;

            //조회
            this.FindButton.Click += FindButton_Click;
        }

        //
        //수주 조회 있을시 => 삭제
        private void ViewGridDtl_SelectedItemChanged(object sender, DevExpress.Xpf.Grid.SelectedItemChangedEventArgs e) 
        {
            if (this.ViewGridDtl.SelectedItems.Count > 0)
            {
                this.DelButton.IsEnabled = true;
            }
        }


        //선택 시 => 추가
        private void ViewGridMst_SelectedItemChanged(object sender, DevExpress.Xpf.Grid.SelectedItemChangedEventArgs e)
        {
            if (this.ViewGridMst.SelectedItems.Count > 0)
            {
                this.AddButton.IsEnabled = true;
            }
        }


        //조회
        private async void FindButton_Click(object sender, RoutedEventArgs e)
        {
            PurVo _param = new PurVo();
            _param.AREA_CD = "001";
            _param.FM_DT = Convert.ToDateTime(this.text_FM_DT.Text).ToString("yyyyMMdd");
            _param.TO_DT = Convert.ToDateTime(this.text_TO_DT.Text).ToString("yyyyMMdd");
            _param.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("p44011/mst", new StringContent(JsonConvert.SerializeObject(_param), System.Text.Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.ViewGridMst.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<PurVo>>(await response.Content.ReadAsStringAsync()).Cast<PurVo>().ToList();
                }
                //
                this.ViewGridMst.SelectedItems = new List<PurVo>();
            }

        }


        //Add
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.ViewGridMst.SelectedItems.Count > 0)
            {
                //
                foreach (PurVo _item in this.ViewGridMst.SelectedItems)
                {
                    if (this._saveList.Any<ManVo>(x=> x.SL_ORD_NO.Equals(_item.SL_ORD_NO) && x.SL_ORD_SEQ == _item.SL_ORD_SEQ) == false)
                    {
                        this._saveList.Add(new ManVo() {GBN = string.Empty, SL_ORD_NO = _item.SL_ORD_NO, SL_ORD_SEQ = _item.SL_ORD_SEQ, CHNL_CD = SystemProperties.USER_VO.CHNL_CD, CRE_USR_ID = SystemProperties.USER, UPD_USR_ID = SystemProperties.USER, PROD_PLN_NO = this.orgDao.PROD_PLN_NO });
                    }
                }
                this.ViewGridDtl.RefreshData();
            }
        }

        //Del
        private void DelButton_Click(object sender, RoutedEventArgs e)
        {
            ManVo _vo = this.ViewGridDtl.SelectedItem as ManVo;
            this._saveList.Remove(_vo);

            //
            this.ViewGridDtl.RefreshData();
        }


        #region Functon (OKButton_Click, CancelButton_Click)
        private async void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValueCheckd())
            {
                if (this._saveList.Count == 0)
                {
                    this._saveList = new List<ManVo>() { new ManVo() { PROD_PLN_NO = this.orgDao.PROD_PLN_NO, GBN = "Y", CHNL_CD = SystemProperties.USER_VO.CHNL_CD, CRE_USR_ID = SystemProperties.USER, UPD_USR_ID = SystemProperties.USER } };
                }


                int _Num = 0;
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6630/dtl/m", new StringContent(JsonConvert.SerializeObject(this._saveList), System.Text.Encoding.UTF8, "application/json")))
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
                        WinUIMessageBox.Show("저장 되었습니다", _title, MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                //this.DialogResult = true;
                //this.Close();
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
            //if (string.IsNullOrEmpty(this.combo_PCK_PLST_CLSS_CD.Text))
            //{
            //    WinUIMessageBox.Show("[구분] 입력 값이 맞지 않습니다.", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.combo_PCK_PLST_CLSS_CD.IsTabStop = true;
            //    this.combo_PCK_PLST_CLSS_CD.Focus();
            //    return false;
            //}
            //else if (string.IsNullOrEmpty(this.text_PCK_PLST_CD.Text))
            //{
            //    WinUIMessageBox.Show("[포장 코드] 입력 값이 맞지 않습니다.", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.text_PCK_PLST_CD.IsTabStop = true;
            //    this.text_PCK_PLST_CD.Focus();
            //    return false;
            //}
            //else if (this.text_SysFlg.Text == null || this.text_SysFlg.Text.Trim().Length == 0)
            //{
            //    WinUIMessageBox.Show("[구분] 입력 값이 맞지 않습니다.", "[유효검사]시스템 분류 코드", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.text_SysFlg.IsTabStop = true;
            //    this.text_SysFlg.Focus();
            //    return false;
            //}
            //else if (this.text_SysAreaCd.Text == null || this.text_SysAreaCd.Text.Trim().Length == 0)
            //{
            //    WinUIMessageBox.Show("[업무 분야] 입력 값이 맞지 않습니다.", "[유효검사]시스템 분류 코드", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.text_SysAreaCd.IsTabStop = true;
            //    this.text_SysAreaCd.Focus();
            //    return false;
            //}
            //else if (this.combo_deltFlg.Text == null || this.combo_deltFlg.Text.Trim().Length == 0)
            //{
            //    WinUIMessageBox.Show("[사용 여부] 입력 값이 맞지 않습니다.", "[유효검사]시스템 분류 코드", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.combo_deltFlg.IsTabStop = true;
            //    this.combo_deltFlg.Focus();
            //    return false;
            //}
            //else
            //{
            //    if (this.isEdit == false)
            //    {
            //        //CodeDao PCK_PLST_CLSS_CD = this.combo_PCK_PLST_CLSS_CD.SelectedItem as CodeDao;

            //        //ManVo dao = new ManVo()
            //        //{
            //        //    PCK_PLST_CLSS_CD = PCK_PLST_CLSS_CD.CLSS_CD,
            //        //    PCK_PLST_CD = this.text_PCK_PLST_CD.Text,
            //        //    CHNL_CD = SystemProperties.USER_VO.CHNL_CD
            //        //};
            //        //IList<ManVo> daoList = (IList<ManVo>)manClient.M6625SelectMaster(dao);
            //        //if (daoList.Count != 0)
            //        //{
            //        //    WinUIMessageBox.Show("[코드 - 중복] 코드를 다시 입력 하십시오.", "[중복검사]포장 코드 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
            //        //    this.text_PCK_PLST_CD.IsTabStop = true;
            //        //    this.text_PCK_PLST_CD.Focus();
            //        //    return false;
            //        //}
            //    }
            //}
            return true;
        }
        #endregion

        //#region Functon (getDomain - ConfigView1Dao)
        //private ManVo getDomain()
        //{
        //    ManVo Dao = new ManVo();

        //    //Dao.PCK_PLST_CD = this.text_PCK_PLST_CD.Text;
        //    //Dao.PCK_PLST_NM = this.text_PCK_PLST_NM.Text;
        //    ////Dao.EQ_LOC_CD = this.text_EQ_LOC_CD.Text;
        //    //SystemCodeVo PCK_PLST_CLSS_CD = this.combo_PCK_PLST_CLSS_CD.SelectedItem as SystemCodeVo;
        //    //if (PCK_PLST_CLSS_CD != null)
        //    //{
        //    //    Dao.PCK_PLST_CLSS_NM = PCK_PLST_CLSS_CD.CLSS_DESC;
        //    //    Dao.PCK_PLST_CLSS_CD = PCK_PLST_CLSS_CD.CLSS_CD;
        //    //}

        //    //Dao.PCK_PLST_VAL = int.Parse((string.IsNullOrEmpty(this.text_PCK_PLST_VAL.Text) ? "0" : this.text_PCK_PLST_VAL.Text));

        //    //
        //    Dao.CRE_USR_ID = SystemProperties.USER;
        //    Dao.UPD_USR_ID = SystemProperties.USER;
        //    Dao.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;

        //    return Dao;
        //}
        //#endregion


        public async void SYSTEM_CODE_VO()
        {
            //수주 조회
            FindButton_Click(null, null);

            //
            ManVo _param2 = new ManVo();
            _param2.UPD_USR_ID = SystemProperties.USER;
            _param2.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
            _param2.PROD_PLN_NO = this.orgDao.PROD_PLN_NO;
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6630/dtl", new StringContent(JsonConvert.SerializeObject(_param2), System.Text.Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    this._saveList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
                    this.ViewGridDtl.ItemsSource = this._saveList;
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
       

    }
}
