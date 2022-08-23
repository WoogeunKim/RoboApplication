using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Code;
using ModelsLibrary.Sale;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;
using AquilaErpWpfApp3.Util;

namespace AquilaErpWpfApp3.View.SAL.Dialog
{
    public partial class S2217MasterDialog : DXWindow
    {
        //private static SaleOrderServiceClient saleOrderClient = SystemProperties.SaleOrderClient;
        private SaleVo orgDao;
        private bool isEdit = false;
        private SaleVo updateDao;

        private string title = "반품 등록 관리";

        public S2217MasterDialog(SaleVo Dao)
        {
            InitializeComponent();

            ////this.combo_RGST_USR_ID.ItemsSource = SystemProperties.USER_CODE_VO();
            //this.combo_SL_CO_NM.ItemsSource = SystemProperties.CUSTOMER_CODE_VO("AR", SystemProperties.USER_VO.EMPE_PLC_NM);
            //this.combo_AREA_NM.ItemsSource = SystemProperties.SYSTEM_CODE_VO("L-002");
            SYSTEM_CODE_VO();


            this.orgDao = Dao;

            SaleVo copyDao = new SaleVo()
            {
                AREA_CD = Dao.AREA_CD,
                AREA_NM = Dao.AREA_NM,
                SL_BIL_RTN_NO = Dao.SL_BIL_RTN_NO,
                SL_BIL_RTN_DT = Dao.SL_BIL_RTN_DT,
                SL_CO_CD = Dao.SL_CO_CD,
                SL_CO_NM = Dao.SL_CO_NM,
                SL_ITM_RMK = Dao.SL_ITM_RMK,
                RTN_AFT_DESC = Dao.RTN_AFT_DESC,
                RTN_AFT_A_DESC = Dao.RTN_AFT_A_DESC,
                RTN_AFT_B_DESC = Dao.RTN_AFT_B_DESC,
                RTN_AFT_C_DESC = Dao.RTN_AFT_C_DESC,

                RTN_APRO_FLG = Dao.RTN_APRO_FLG,

                CRE_USR_ID = Dao.CRE_USR_ID,
                UPD_USR_ID = Dao.UPD_USR_ID,
                CHNL_CD = SystemProperties.USER_VO.CHNL_CD
            };

            //수정
            if (copyDao.SL_BIL_RTN_NO != null)
            {
                this.text_SL_BIL_RTN_NO.IsReadOnly = true;
                this.isEdit = true;
               
            }
            else
            {
                //추가
                this.text_SL_BIL_RTN_NO.IsReadOnly = true;
                this.isEdit = false;
                copyDao.SL_BIL_RTN_DT = System.DateTime.Now.ToString("yyyy-MM-dd");
                copyDao.RTN_APRO_FLG = "N";
            }
            this.configCode.DataContext = copyDao;
            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
            this.OKButton.Click += new RoutedEventHandler(OKButton_Click);
            this.CancelButton.Click += new RoutedEventHandler(CancelButton_Click);

            this.combo_AREA_NM.SelectedIndexChanged += combo_AREA_NM_SelectedIndexChanged;


            this.combo_SL_CO_NM.Focus();
        }

        async void combo_AREA_NM_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            SystemCodeVo areaNmVo = this.combo_AREA_NM.SelectedItem as SystemCodeVo;
            if (areaNmVo != null)
            {
                //this.combo_SL_CO_NM.ItemsSource = SystemProperties.CUSTOMER_CODE_VO("AR", areaNmVo.CLSS_CD);
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s143", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { DELT_FLG = "N", CO_TP_CD = "AR", AREA_CD = areaNmVo.CLSS_CD, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.combo_SL_CO_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                    }
                }
            }
        }

        #region Functon (OKButton_Click, CancelButton_Click)
        private async void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValueCheckd())
            {
                int _Num = 0;
                //ProgramVo resultVo;
                if (isEdit == false)
                {
                    this.updateDao = getDomain();//this.updateDao
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s2217/mst/i", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
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

                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s2217/mst/u", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
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

                //JobVo resultVo;
                ////PurVo updateDao;
                //if (isEdit == false)
                //{
                //    updateDao = getDomain();//this.updateDao

                //    // 자동 번호 할당
                //    resultVo = saleOrderClient.S2217SelectSlBilRtnNo(updateDao);
                //    if (!resultVo.isSuccess)
                //    {
                //        //실패
                //        WinUIMessageBox.Show(resultVo.Message, "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
                //        return;
                //    }
                //    SL_BIL_RTN_NO = resultVo.LST_FMT_NO;
                //    updateDao.SL_BIL_RTN_NO = resultVo.LST_FMT_NO;
                //    this.text_SL_BIL_RTN_NO.Text = resultVo.LST_FMT_NO;
                //    //MessageBoxResult result = WinUIMessageBox.Show("[" + updateDao.INSRL_NO + "] 저장 하시겠습니까?", "[전표 번호]창고간 이동", MessageBoxButton.YesNo, MessageBoxImage.Question);
                //    //if (result == MessageBoxResult.Yes)
                //    //{
                //    resultVo = saleOrderClient.S2217InsertMst(updateDao);
                //    if (!resultVo.isSuccess)
                //    {
                //        //실패
                //        WinUIMessageBox.Show(resultVo.Message, "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
                //        return;
                //    }
                //    //성공
                //    WinUIMessageBox.Show("[반품 번호 : " + updateDao.SL_BIL_RTN_NO + "] 완료 되었습니다", "[추가]" + title, MessageBoxButton.OK, MessageBoxImage.Information);
                //    //}
                //}
                //else
                //{
                //    updateDao = getDomain();
                //    resultVo = saleOrderClient.S2217UpdateMst(updateDao);
                //    if (!resultVo.isSuccess)
                //    {
                //        //실패
                //        WinUIMessageBox.Show(resultVo.Message, "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
                //        return;
                //    }
                //    //성공
                //    WinUIMessageBox.Show("완료 되었습니다", "[수정]" + title, MessageBoxButton.OK, MessageBoxImage.Information);


                //    this.orgDao.AREA_CD = this.updateDao.AREA_CD;
                //    this.orgDao.AREA_NM = this.updateDao.AREA_NM;
                //    this.orgDao.SL_BIL_RTN_NO = this.updateDao.SL_BIL_RTN_NO;
                //    this.orgDao.SL_BIL_RTN_DT = Convert.ToDateTime(this.updateDao.SL_BIL_RTN_DT).ToString("yyyy-MM-dd");
                //    this.orgDao.SL_CO_CD = this.updateDao.SL_CO_CD;
                //    this.orgDao.SL_CO_NM = this.updateDao.SL_CO_NM;
                //    this.orgDao.SL_ITM_RMK = this.updateDao.SL_ITM_RMK;
                //    this.orgDao.RTN_APRO_FLG = this.updateDao.RTN_APRO_FLG;
                //    this.orgDao.CRE_USR_ID = this.updateDao.CRE_USR_ID;
                //    this.orgDao.UPD_USR_ID = this.updateDao.UPD_USR_ID;
                //    this.orgDao.RTN_AFT_DESC = this.updateDao.RTN_AFT_DESC;
                //    this.orgDao.RTN_AFT_A_DESC = this.updateDao.RTN_AFT_A_DESC;
                //    this.orgDao.RTN_AFT_B_DESC = this.updateDao.RTN_AFT_B_DESC;
                //    this.orgDao.RTN_AFT_C_DESC = this.updateDao.RTN_AFT_C_DESC; 
                //}

                ////
                //if (updateDao.CLZ_FLG.Equals("Y"))
                //{
                //    //수주 자재 리스트  (자동 마감)
                //    resultVo = saleOrderClient.S2211UpdateDtl(new JobVo() { JB_NO = this.orgDao.JB_NO, CLZ_FLG = "Y", CRE_USR_ID = SystemProperties.USER, UPD_USR_ID = SystemProperties.USER });
                //    if (!resultVo.isSuccess)
                //    {
                //        //실패
                //        WinUIMessageBox.Show(resultVo.Message, "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
                //        return;
                //    }
                //}

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
            //if (string.IsNullOrEmpty(this.text_SL_BIL_RTN_NO.Text))
            //{
            //    WinUIMessageBox.Show("[반품 번호] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.text_SL_BIL_RTN_NO.IsTabStop = true;
            //    this.text_SL_BIL_RTN_NO.Focus();
            //    return false;
            //}
            //else 
            if (string.IsNullOrEmpty(this.text_SL_BIL_RTN_DT.Text))
            {
                WinUIMessageBox.Show("[반품 접수 일자] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.text_SL_BIL_RTN_DT.IsTabStop = true;
                this.text_SL_BIL_RTN_DT.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(this.combo_SL_CO_NM.Text))
            {
                WinUIMessageBox.Show("[거래처] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.combo_SL_CO_NM.IsTabStop = true;
                this.combo_SL_CO_NM.Focus();
                return false;
            }
            //else if (string.IsNullOrEmpty(this.text_SL_DUE_DT.Text))
            //{
            //    WinUIMessageBox.Show("[납기일] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.text_SL_DUE_DT.IsTabStop = true;
            //    this.text_SL_DUE_DT.Focus();
            //    return false;
            //}
            //else if (string.IsNullOrEmpty(this.combo_RGST_USR_ID.Text))
            //{
            //    WinUIMessageBox.Show("[작성자] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.combo_RGST_USR_ID.IsTabStop = true;
            //    this.combo_RGST_USR_ID.Focus();
            //    return false;
            //}
            //else if (string.IsNullOrEmpty(this.text_CLZ_FLG.Text))
            //{
            //    WinUIMessageBox.Show("[마감 유무] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.text_CLZ_FLG.IsTabStop = true;
            //    this.text_CLZ_FLG.Focus();
            //    return false;
            //}
                //else if (string.IsNullOrEmpty(this.combo_RQST_EMPE_ID.Text))
                //{
                //    WinUIMessageBox.Show("[요청자] 입력 값이 맞지 않습니다.", "[유효검사]창고간 이동", MessageBoxButton.OK, MessageBoxImage.Warning);
                //    this.combo_RQST_EMPE_ID.IsTabStop = true;
                //    this.combo_RQST_EMPE_ID.Focus();
                //    return false;
                //}
                //else if (string.IsNullOrEmpty(this.text_CAR_NO.Text))
                //{
                //    WinUIMessageBox.Show("[차량 넘버] 입력 값이 맞지 않습니다.", "[유효검사]창고간 이동", MessageBoxButton.OK, MessageBoxImage.Warning);
                //    this.text_CAR_NO.IsTabStop = true;
                //    this.text_CAR_NO.Focus();
                //    return false;
                //}
            //else
            //{
            //    //서버와 날짜 체크

            //    if (this.isEdit == false)
            //    {
            //        InauditVo dao = new InauditVo()
            //        {
            //            INAUD_DT = Convert.ToDateTime(this.text_INAUD_DT.Text)
            //        };

            //        InauditVo daoList = inauditclient.SelectInvtInaudCheckTime(dao);
            //        if (daoList.MODI_FLG.Equals("N"))
            //        {
            //            WinUIMessageBox.Show("[이동 일자] 일자를 다시 입력 하십시오.", "[날짜검사]창고간 이동", MessageBoxButton.OK, MessageBoxImage.Warning);
            //            this.text_INAUD_DT.IsTabStop = true;
            //            this.text_INAUD_DT.Focus();
            //            return false;
            //        }
            //    }
            //    //if (this.isEdit == false)
            //    //{
            //    //    InauditVo dao = new InauditVo()
            //    //    {
            //    //        INSRL_NO = this.text_INSRL_NO.Text,
            //    //    };
            //    //    IList<InauditVo> daoList = (IList<InauditVo>)inauditclient.SelectInvtInaudMastList(dao);
            //    //    if (daoList.Count != 0)
            //    //    {
            //    //        WinUIMessageBox.Show("[코드 - 중복] 코드를 다시 입력 하십시오.", "[중복검사]창고간 이동", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    //        this.text_INSRL_NO.IsTabStop = true;
            //    //        this.text_INSRL_NO.Focus();
            //    //        return false;
            //    //    }
            //    //}
            //}
            return true;
        }
        #endregion

        #region Functon (getDomain - ConfigView1Dao)
        private SaleVo getDomain()
        {
            SaleVo Dao = new SaleVo();


            Dao.SL_BIL_RTN_NO = this.text_SL_BIL_RTN_NO.Text;
            Dao.SL_BIL_RTN_DT = Convert.ToDateTime(this.text_SL_BIL_RTN_DT.Text).ToString("yyyy-MM-dd");
            Dao.RTN_AFT_DESC = this.text_RTN_AFT_DESC.Text;
            Dao.RTN_AFT_A_DESC = this.text_RTN_AFT_A_DESC.Text;
            Dao.RTN_AFT_B_DESC = this.text_RTN_AFT_B_DESC.Text;
            Dao.RTN_AFT_C_DESC = this.text_RTN_AFT_C_DESC.Text;

            SystemCodeVo coNmVo = this.combo_SL_CO_NM.SelectedItem as SystemCodeVo;
            if (coNmVo != null)
            {
                Dao.SL_CO_NO = coNmVo.CO_NO;
                Dao.SL_CO_CD = coNmVo.CO_NO;
                Dao.SL_CO_NM = coNmVo.CO_NM;
            }

            SystemCodeVo areaNmVo = this.combo_AREA_NM.SelectedItem as SystemCodeVo;
            if (areaNmVo != null)
            {
                Dao.AREA_CD = areaNmVo.CLSS_CD;
                Dao.AREA_NM = areaNmVo.CLSS_DESC;
            }

            Dao.SL_ITM_RMK = this.text_SL_ITM_RMK.Text;

            Dao.RTN_APRO_FLG = this.combo_RTN_APRO_FLG.Text;

            Dao.CRE_USR_ID = SystemProperties.USER;
            Dao.UPD_USR_ID = SystemProperties.USER;
            Dao.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
            Dao.GBN = "QC";

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

        public string SL_BIL_RTN_NO
        {
            get;
            set;
        }

        public async void SYSTEM_CODE_VO()
        {
           
            //this.combo_AREA_NM.ItemsSource = SystemProperties.SYSTEM_CODE_VO("L-002");
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "L-002"))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.combo_AREA_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                }
            }

            //this.combo_SL_CO_NM.ItemsSource = SystemProperties.CUSTOMER_CODE_VO("AR", SystemProperties.USER_VO.EMPE_PLC_NM);
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s143", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { DELT_FLG = "N", CO_TP_CD = "AR", AREA_CD = SystemProperties.USER_VO.EMPE_PLC_NM, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.combo_SL_CO_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                }
            }
        }
    }
}
