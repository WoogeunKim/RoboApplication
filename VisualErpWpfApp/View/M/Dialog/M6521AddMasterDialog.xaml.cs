using AquilaErpWpfApp3.Util;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using DevExpress.Xpf.Editors;
using ModelsLibrary.Code;
using ModelsLibrary.Man;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;

namespace AquilaErpWpfApp3.M.View.Dialog
{
    public partial class M6521AddMasterDialog : DXWindow
    {
        private ManVo orgDao;
        private bool isEdit = false;
        private ManVo updateDao;

        private string title = "생산계획/작업지시";

        public M6521AddMasterDialog(ManVo Dao)
        {
            InitializeComponent();

            //
            this.orgDao = Dao;
            ManVo copyDao = new ManVo()
            {
                MOLD_NO = Dao.MOLD_NO,
                MOLD_NM = Dao.MOLD_NM,
                MOLD_SZ = Dao.MOLD_SZ,
                ITM_CD = Dao.ITM_CD,
                ITM_NM = Dao.ITM_NM,
                //MOLD_MAKE_DT = Dao.MOLD_MAKE_DT,
                SL_ORD_NO = Dao.SL_ORD_NO,
                SL_ORD_SEQ = Dao.SL_ORD_SEQ,
                PCK_FLG = Dao.PCK_FLG,
                PROD_QTY = Dao.PROD_QTY,
                MM_RMK = Dao.MM_RMK,
                DY_NGT_FLG = Dao.DY_NGT_FLG,
                MOLD_CD = Dao.MOLD_CD,
                EQ_NO = Dao.EQ_NO,
                ROUT_CD = Dao.ROUT_CD,
                ROUT_NM = Dao.ROUT_NM,
                PRSURE_VAL = Dao.PRSURE_VAL,
                LOT_DIV_QTY = Dao.LOT_DIV_QTY,
                LOT_DIV_NO = Dao.LOT_DIV_NO,
                LOT_DIV_SEQ = Dao.LOT_DIV_SEQ,
                MAKE_ST_DT = Dao.MAKE_ST_DT,
                MAKE_END_DT = Dao.MAKE_END_DT,
                DY_NGT_NM = Dao.DY_NGT_NM,
                PROD_ORD_NO = Dao.PROD_ORD_NO,
                PROD_PLN_DT = Dao.PROD_PLN_DT,
                CRE_USR_ID = Dao.CRE_USR_ID,
                UPD_USR_ID = Dao.UPD_USR_ID,
                SL_RLSE_NO = Dao.SL_RLSE_NO,
                SL_RLSE_SEQ = Dao.SL_RLSE_SEQ,
                ITM_SZ_NM = Dao.ITM_SZ_NM,
                LOSS_VAL = Dao.LOSS_VAL,
                GBN = Dao.GBN,
                MOLD_CAPA_QTY = Dao.MOLD_CAPA_QTY,
                CUT_SZ_NM = Dao.CUT_SZ_NM,
                PRT_VAL = Dao.PRT_VAL
            };

            if (Dao.LOT_DIV_NO != null)
            {
                this.isEdit = true;
                this.Title = title + " - 수정";
            }
            else
            {
                this.isEdit = false;
                this.Title = title + " - 추가";
                copyDao.DELT_FLG = "사용";
                copyDao.LOSS_VAL = 0.0;   // 로스율
            }

            this.configCode.DataContext = copyDao;
            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
            this.OKButton.Click += new RoutedEventHandler(OKButton_Click);
            this.CancelButton.Click += new RoutedEventHandler(CancelButton_Click);
            this.combo_MOLD_CD.SelectedIndexChanged += new RoutedEventHandler(Combo_MOLD_CD_SelectedIndexChanged);
            this.txt_LOSS_VAL.EditValueChanged += new EditValueChangedEventHandler(Txt_LOSS_VAL_EditValueChanged);


            SYSTEM_CODE_VO();
        }

        // 로스 입력시 인쉐통수 자동 계산
        private void Txt_LOSS_VAL_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            try
            {
                if (this.txt_LOSS_VAL.Text == null || orgDao.MOLD_CAPA_QTY == null || orgDao.LOT_DIV_QTY == null) return;

                double moldCapaQty = double.Parse(this.txt_LOSS_VAL.Text);
                this.orgDao.LOSS_VAL = moldCapaQty;
                this.orgDao.PRT_VAL = (double.Parse(orgDao.LOT_DIV_QTY.ToString()) / double.Parse(orgDao.MOLD_CAPA_QTY.ToString())) + moldCapaQty;
                this.txt_PRT_VAL.Text = this.orgDao.PRT_VAL.ToString();

            }
            catch (Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }

        // 목형 선택시 칼사이즈, 판수 자동 입력
        private void Combo_MOLD_CD_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.combo_MOLD_CD.SelectedItem == null) return;

                ManVo moldVo = this.combo_MOLD_CD.SelectedItem as ManVo;

                this.orgDao.MOLD_CD = moldVo.MOLD_NO;
                this.orgDao.CUT_SZ_NM = moldVo.CUT_SZ_NM;
                this.orgDao.MOLD_CAPA_QTY = moldVo.MOLD_CAPA_QTY;
                this.label_CUT_SZ_NM.Content = "칼사이즈 : " + moldVo.CUT_SZ_NM.ToString();
                this.txt_MOLD_CAPA_QTY.Text = moldVo.MOLD_CAPA_QTY.ToString();

            }
            catch (Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }

        }



        #region Functon (OKButton_Click, CancelButton_Click)
        private async void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ValueCheckd())
                {
                    int _Num = 0;
                    if (isEdit == false)
                    {
                        this.updateDao = getDomain();
                        ResultDao = this.updateDao;
                        if (Convert.ToDateTime(updateDao.MAKE_ST_DT) < DateTime.Today)
                        {
                            if (WinUIMessageBox.Show("[생산계획일자] 생산계획일자가 현재보다 이전입니다. " + Environment.NewLine + "추가하시겠습니까?", "[유효검사]" + title, MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                            {
                                return;
                            }
                        }
                        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6521/dtl/i", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                string result = await response.Content.ReadAsStringAsync();
                                if (int.TryParse(result, out _Num) == false)
                                {
                                    //실패
                                    WinUIMessageBox.Show(result, title, MessageBoxButton.OK, MessageBoxImage.Error);
                                    return;
                                }

                                //성공
                                WinUIMessageBox.Show("완료 되었습니다", title, MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                        }
                    }
                    else
                    {
                        this.updateDao = getDomain();
                        ResultDao = this.updateDao;
                        if (Convert.ToDateTime(updateDao.MAKE_ST_DT) < DateTime.Today)
                        {
                            if (WinUIMessageBox.Show("[생산계획일자] 생산계획일자가 현재보다 이전입니다. " + Environment.NewLine + "수정하시겠습니까?", "[유효검사]" + title, MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                            {
                                return;
                            }
                        }

                        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6521/dtl/u", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                string result = await response.Content.ReadAsStringAsync();
                                if (int.TryParse(result, out _Num) == false)
                                {
                                    //실패
                                    WinUIMessageBox.Show(result, title, MessageBoxButton.OK, MessageBoxImage.Error);
                                    return;
                                }

                                //성공
                                WinUIMessageBox.Show("완료 되었습니다", title, MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                        }
                    }
                    this.DialogResult = true;
                    this.Close();
                }
            }
            catch (Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
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

        #region Functon (ValueCheckd)
        public Boolean ValueCheckd()
        {
            try
            {
                if (this.combo_MOLD_CD.SelectedItem == null)
                {
                    WinUIMessageBox.Show("[목형] 선택하지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                    this.combo_MOLD_CD.IsTabStop = true;
                    this.combo_MOLD_CD.Focus();
                    return false;
                }
                else if (this.orgDao.ROUT_CD == null)
                {
                    WinUIMessageBox.Show("[공정] 설정이 필요합니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }
                else if (this.orgDao.PRT_VAL == null)
                {
                    WinUIMessageBox.Show("[인쇄통수] 설정이 필요합니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }
                else if (double.Parse(this.orgDao.PRT_VAL.ToString()) <= 0)
                {
                    WinUIMessageBox.Show("[인쇄통수] 값을 확인하세요.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }

                return true;
            }
            catch (Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return false;
            }
        }
        #endregion

        #region Functon (getDomain - ConfigView1Dao)
        public ManVo getDomain()
        {
            this.orgDao.PROD_PLN_DT = Convert.ToDateTime(this.text_MAKE_ST_DT.Text).ToString("yyyy-MM-dd");
            this.orgDao.MAKE_ST_DT = Convert.ToDateTime(this.text_MAKE_ST_DT.Text).ToString("yyyy-MM-dd");
            this.orgDao.LOT_DIV_NO = 
            this.orgDao.CRE_USR_ID = SystemProperties.USER;
            this.orgDao.UPD_USR_ID = SystemProperties.USER;
            this.orgDao.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
            this.orgDao.DY_NGT_FLG = "D";
            return this.orgDao;
        }
        #endregion

        public async void SYSTEM_CODE_VO()
        {
            try
            {
                // 공정
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6521/rout", new StringContent(JsonConvert.SerializeObject(new ManVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD, ITM_CD = orgDao.ITM_CD }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        ManVo routVo = JsonConvert.DeserializeObject<ManVo>(await response.Content.ReadAsStringAsync());

                        if (routVo == null)
                        {
                            WinUIMessageBox.Show(orgDao.ITM_NM + "의 공정 존재 하지 않습니다", title, MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.None, MessageBoxOptions.None);

                            this.DialogResult = false;
                            Close();
                            return;
                        }

                        this.orgDao.ROUT_CD = JsonConvert.DeserializeObject<ManVo>(await response.Content.ReadAsStringAsync()).ROUT_CD;
                    }
                }

                //// 목형
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6521/mold", new StringContent(JsonConvert.SerializeObject(new ManVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.combo_MOLD_CD.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
                    }
                }

                // 자재 Size 정보 
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6521/pur/itm/sz", new StringContent(JsonConvert.SerializeObject(new ManVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD, SL_RLSE_NO = orgDao.SL_RLSE_NO }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.PUT_ITM_SZ_DTL.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
                    }
                }
            }
            catch(Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
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

        public ManVo ResultDao
        {
            get;
            set;
        }

    }
}
