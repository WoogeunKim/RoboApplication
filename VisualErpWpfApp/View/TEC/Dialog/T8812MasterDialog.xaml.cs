using AquilaErpWpfApp3.Util;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Code;
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
    public partial class T8812MasterDialog : DXWindow
    {
        //private static CodeServiceClient codeClient = SystemProperties.CodeClient;
        //private SystemCodeVo orgDao;
        //private bool isEdit = false;
        //private SystemCodeVo updateDao;
        private IList<TecVo> ChekList;
        private string _title = "수입검사";

        public T8812MasterDialog(IList<TecVo> _chekList)
        {
            InitializeComponent();
            //
            //this.orgDao = Dao;
            //SystemCodeVo copyDao = new SystemCodeVo()
            //{
            //    CLSS_TP_CD = Dao.CLSS_TP_CD,
            //    CLSS_TP_NM = Dao.CLSS_TP_NM,
            //    SYS_FLG = Dao.SYS_FLG,
            //    SYS_AREA_CD = Dao.SYS_AREA_CD,
            //    CLSS_CD_DESC = Dao.CLSS_CD_DESC,
            //    DELT_FLG = Dao.DELT_FLG,
            //    CRE_USR_ID = Dao.CRE_USR_ID,
            //    UPD_USR_ID = Dao.UPD_USR_ID,
            //    CHNL_CD = Dao.CHNL_CD
            //};


            this.ChekList = _chekList;

            SYSTEM_CODE_VO();
            ////수정
            //if (copyDao.CLSS_TP_CD != null)
            //{
            //    this.text_ClssTpCd.IsReadOnly = true;
            //    this.isEdit = true;
            //}
            //else
            //{
            //    //추가
            //    //this.text_ClssTpCd.IsReadOnly = false;
            //    //this.isEdit = false;
            //    copyDao.DELT_FLG = "사용";
            //}
            //this.text_INSP_DT.Text = System.DateTime.Now.ToString("yyyy-MM-dd");

            this.text_MTRL_EXP_DT.EditValueChanged += Text_MTRL_EXP_DT_EditValueChanged;

            this.text_MTRL_EXP_DY.KeyUp += Text_MTRL_EXP_DY_KeyUp;



            this.configCode.DataContext = _chekList[0];
            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
            this.OKButton.Click += new RoutedEventHandler(OKButton_Click);
            this.CancelButton.Click += new RoutedEventHandler(CancelButton_Click);
        }

        private void Text_MTRL_EXP_DY_KeyUp(object sender, KeyEventArgs e)
        {

            this.text_MTRL_EXP_DT.Text = Convert.ToDateTime(this.text_MTRL_MAKE_DT.Text).AddDays(Convert.ToInt32(this.text_MTRL_EXP_DY.Text)).ToString("yyyy-MM-dd");

        }

        private void Text_MTRL_EXP_DT_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            try
            {
                DateTime T1 = DateTime.Parse(this.text_MTRL_MAKE_DT.Text);
                DateTime T2 = DateTime.Parse(this.text_MTRL_EXP_DT.Text);

                //
                TimeSpan TS = T2 - T1;
                this.text_MTRL_EXP_DY.Text = ("" + TS.Days);
            }
            catch
            {
                return;
            }
        }

        #region Functon (OKButton_Click, CancelButton_Click)
        private async void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValueCheckd())
            {
                int _Num = 0;
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

                foreach (TecVo item in this.ChekList)
                {
                    item.MTRL_MAKE_DT = Convert.ToDateTime(this.text_MTRL_MAKE_DT.Text).ToString("yyyy-MM-dd");
                    item.MTRL_EXP_DT = Convert.ToDateTime(this.text_MTRL_EXP_DT.Text).ToString("yyyy-MM-dd");

                    item.CO_MAKE_NO = this.text_CO_MAKE_NO.Text;

                    item.INSP_NO = this.text_INSP_NO.Text;
                    item.MTRL_EXP_DY = Convert.ToInt32(this.text_MTRL_EXP_DY.Text);

                    if (string.IsNullOrEmpty(this.text_INSP_DT.Text))
                    {
                        item.INSP_DT = null;
                    }
                    else
                    {
                        item.INSP_DT = Convert.ToDateTime(this.text_INSP_DT.Text).ToString("yyyy-MM-dd");
                    }

                    item.INSP_FLG = (this.combo_INSP_FLG.Text.Equals("시험중") ? "Z" : (this.combo_INSP_FLG.Text.Equals("적합") ? "Y" : "N"));
                    item.ITM_RMK = this.text_ITM_RMK.Text;

                    item.TMP_LOC_NM = this.combo_TMP_LOC_NM.Text;
                    item.TMP_CONDI_NM = this.text_TMP_CONDI_NM.Text;
                    //item.INAUD_TMP_NO = INAUD_TMP_NO;
                    //item.INAUD_TMP_SEQ = INAUD_TMP_SEQ;
                    item.CO_CD = this.combo_CO_NM.Text;

                    item.CRE_USR_ID = SystemProperties.USER_VO.USR_ID;
                    item.UPD_USR_ID = SystemProperties.USER_VO.USR_ID;
                    item.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
                }

                //
                //
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("t8812/m", new StringContent(JsonConvert.SerializeObject(this.ChekList), System.Text.Encoding.UTF8, "application/json")))
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
                        this.DialogResult = true;
                        this.Close();
                    }
                }
                //
                //


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
            if (string.IsNullOrEmpty(this.combo_CO_NM.Text))
            {
                //UOM_CD = new CodeDao() { CLSS_DESC = ITM_NM.UOM_NM };
                //MTRL_MAKE_DT = Convert.ToDateTime(MTRL_MAKE_DT).AddDays(MTRL_EXP_DY).ToString("yyyy-MM-dd");
                WinUIMessageBox.Show("[매입처] 입력 값이 맞지 않습니다", "[유효검사] " + this._title, MessageBoxButton.OK, MessageBoxImage.Warning);
                //MSG = "[제조 번호] 입력 값이 맞지 않습니다";
                return false;
            }

            if (string.IsNullOrEmpty(this.text_CO_MAKE_NO.Text))
            {
                //UOM_CD = new CodeDao() { CLSS_DESC = ITM_NM.UOM_NM };
                //MTRL_MAKE_DT = Convert.ToDateTime(MTRL_MAKE_DT).AddDays(MTRL_EXP_DY).ToString("yyyy-MM-dd");
                WinUIMessageBox.Show("[제조 번호] 입력 값이 맞지 않습니다", "[유효검사] " + this._title, MessageBoxButton.OK, MessageBoxImage.Warning);
                //MSG = "[제조 번호] 입력 값이 맞지 않습니다";
                return false;
            }

            if (string.IsNullOrEmpty(this.text_MTRL_MAKE_DT.Text))
            {
                //UOM_CD = new CodeDao() { CLSS_DESC = ITM_NM.UOM_NM };
                //MTRL_MAKE_DT = Convert.ToDateTime(MTRL_MAKE_DT).AddDays(MTRL_EXP_DY).ToString("yyyy-MM-dd");
                WinUIMessageBox.Show("[제조 일자] 입력 값이 맞지 않습니다", "[유효검사] " + this._title, MessageBoxButton.OK, MessageBoxImage.Warning);
                //MSG = "[제조 일자] 입력 값이 맞지 않습니다";
                return false;
            }


            if (string.IsNullOrEmpty(this.text_MTRL_EXP_DY.Text + ""))
            {
                //UOM_CD = new CodeDao() { CLSS_DESC = ITM_NM.UOM_NM };
                //MTRL_MAKE_DT = Convert.ToDateTime(MTRL_MAKE_DT).AddDays(MTRL_EXP_DY).ToString("yyyy-MM-dd");
                WinUIMessageBox.Show("[유효기간(일수)] 입력 값이 맞지 않습니다", "[유효검사] " + this._title, MessageBoxButton.OK, MessageBoxImage.Warning);
                //MSG = "[유효기간(일수)] 입력 값이 맞지 않습니다";
                return false;
            }
            else
            {
                this.text_MTRL_EXP_DT.Text = Convert.ToDateTime(this.text_MTRL_MAKE_DT.Text).AddDays(Convert.ToInt32(this.text_MTRL_EXP_DY.Text)).ToString("yyyy-MM-dd");
            }



            if (string.IsNullOrEmpty(this.text_INSP_NO.Text))
            {
                //UOM_CD = new CodeDao() { CLSS_DESC = ITM_NM.UOM_NM };
                //MTRL_MAKE_DT = Convert.ToDateTime(MTRL_MAKE_DT).AddDays(MTRL_EXP_DY).ToString("yyyy-MM-dd");
                WinUIMessageBox.Show("[시험 번호] 입력 값이 맞지 않습니다", "[유효검사] " + this._title, MessageBoxButton.OK, MessageBoxImage.Warning);
                //MSG = "[시험 번호] 입력 값이 맞지 않습니다";
                return false;
            }

            //if (string.IsNullOrEmpty(this.text_INSP_DT.Text))
            //{
            //    //UOM_CD = new CodeDao() { CLSS_DESC = ITM_NM.UOM_NM };
            //    //MTRL_MAKE_DT = Convert.ToDateTime(MTRL_MAKE_DT).AddDays(MTRL_EXP_DY).ToString("yyyy-MM-dd");
            //    WinUIMessageBox.Show("[판정 일자] 입력 값이 맞지 않습니다", "[유효검사] " + this._title, MessageBoxButton.OK, MessageBoxImage.Warning);
            //    //MSG = "[판정 일자] 입력 값이 맞지 않습니다";
            //    return false;
            //}



            if (string.IsNullOrEmpty(this.combo_INSP_FLG.Text))
            {
                //UOM_CD = new CodeDao() { CLSS_DESC = ITM_NM.UOM_NM };
                //MTRL_MAKE_DT = Convert.ToDateTime(MTRL_MAKE_DT).AddDays(MTRL_EXP_DY).ToString("yyyy-MM-dd");
                WinUIMessageBox.Show("[판정] 입력 값이 맞지 않습니다", "[유효검사] " + this._title, MessageBoxButton.OK, MessageBoxImage.Warning);
                //MSG = "[판정] 입력 값이 맞지 않습니다";
                return false;
            }

            return true;
        }
        #endregion


        public async void SYSTEM_CODE_VO()
        {

            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s143", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { DELT_FLG = "N", CO_TP_CD = "AR", AREA_CD = SystemProperties.USER_VO.EMPE_PLC_NM, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    IList<SystemCodeVo> tmpList = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                    tmpList.Insert(0, new SystemCodeVo() { CO_NO = "", CO_NM = "" });
                    //
                    this.combo_CO_NM.ItemsSource = tmpList;
                }
            }

        }
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

    }
}
