using AquilaErpWpfApp3.Util;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Tec;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;

namespace AquilaErpWpfApp3.View.TEC.Dialog
{
    /// <summary>
    /// Interaction logic for ConfigView1Dialog.xaml
    /// </summary>
    public partial class T8815MasterDialog : DXWindow
    {
        //private static CodeServiceClient codeClient = SystemProperties.CodeClient;
        //private SystemCodeVo orgDao;
        //private bool isEdit = false;
        //private SystemCodeVo updateDao;
        private IList<TecVo> ChekList;
        private string _title = "반제품 품질검사";

        public T8815MasterDialog(IList<TecVo> _chekList)
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
            this.configCode.DataContext = _chekList[0];
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
                    //item.MTRL_MAKE_DT = Convert.ToDateTime(this.text_MTRL_MAKE_DT.Text).ToString("yyyy-MM-dd");
                    //item.MTRL_EXP_DT = Convert.ToDateTime(this.text_MTRL_EXP_DT.).ToString("yyyy-MM-dd");
                    //Dao.ITM_IN_DT = Convert.ToDateTime(ITM_IN_DT).ToString("yyyy-MM-dd");

                    item.INSP_NO = this.text_INSP_NO.Text;

                    //Dao.INSP_NO = INSP_NO;
                    //item.MTRL_EXP_DY = this.text_MTRL_EXP_DY;

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

                    //item.INSP_QTY = this.text_INSP_QTY.Text;
                    item.ITM_QTY = this.text_ITM_QTY.Text;

                    //item.INAUD_TMP_NO = INAUD_TMP_NO;
                    //item.INAUD_TMP_SEQ = INAUD_TMP_SEQ;
                    item.CRE_USR_ID = SystemProperties.USER_VO.USR_ID;
                    item.UPD_USR_ID = SystemProperties.USER_VO.USR_ID;
                    item.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
                }

                //
                //
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("t8815/m", new StringContent(JsonConvert.SerializeObject(this.ChekList), System.Text.Encoding.UTF8, "application/json")))
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
            if (string.IsNullOrEmpty(this.text_INSP_NO.Text))
            {
                //UOM_CD = new CodeDao() { CLSS_DESC = ITM_NM.UOM_NM };
                //MTRL_MAKE_DT = Convert.ToDateTime(MTRL_MAKE_DT).AddDays(MTRL_EXP_DY).ToString("yyyy-MM-dd");
                WinUIMessageBox.Show("[시험번호] 입력 값이 맞지 않습니다", "[유효검사] " + this._title, MessageBoxButton.OK, MessageBoxImage.Warning);
                //MSG = "[배치코드] 입력 값이 맞지 않습니다";
                return false;
            }

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
