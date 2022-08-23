using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Auth;
using ModelsLibrary.Code;
using ModelsLibrary.Inv;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;
using AquilaErpWpfApp3.Util;

namespace AquilaErpWpfApp3.View.INV.Dialog
{
    public partial class I5511MasterDialog : DXWindow
    {
        private string _title = "품목 입고 관리";
        //private static InvServiceClient invClient = SystemProperties.InvClient;
        private InvVo orgDao;
        private bool isEdit = false;
        private InvVo updateDao;

        public I5511MasterDialog(InvVo Dao)
        {
            InitializeComponent();

            this.orgDao = Dao;

            InvVo copyDao = new InvVo()
            {
                INSRL_NO = Dao.INSRL_NO,
                INAUD_DT = Dao.INAUD_DT,
                RQST_EMPE_ID = Dao.RQST_EMPE_ID,
                RQST_EMPE_NM = Dao.RQST_EMPE_NM,
                IO_CD = Dao.IO_CD,
                INAUD_RMK = Dao.INAUD_RMK,
                AREA_CD = Dao.AREA_CD,
                AREA_NM = Dao.AREA_NM,
                CO_CD = Dao.CO_CD,
                CO_NM = Dao.CO_NM
            };

            //수정
            if (Dao.INSRL_NO != null)
            {
                this.text_INSRL_NO.IsReadOnly = true;
                this.isEdit = true;
                //
                ////마감 처리 후 수정 불가능
                //if (Dao.MODI_FLG.Equals("N"))
                //{
                //    this.OKButton.IsEnabled = false;
                //}
                this.INSRL_NO = this.text_INSRL_NO.Text;
            }
            else
            {
                //추가
                this.text_INSRL_NO.IsReadOnly = true;
                this.isEdit = false;


                //copyDao.INAUD_DT = System.DateTime.Now.ToString("yyyy-MM-dd");
   
                //
                //Dao.DO_RQST_GRP_NM = SystemProperties.USERVO.GRP_NM;
                //Dao.DO_RQST_USR_NM = SystemProperties.USERVO.USR_N1ST_NM;
                //this.combo_DO_RQST_USR_NM.SelectedItem = ((IList<CodeDao>)combo_DO_RQST_USR_NM.ItemsSource)[2];
            }

            SYSTEM_CODE_VO();

            this.configCode.DataContext = copyDao;
            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
            this.OKButton.Click += new RoutedEventHandler(OKButton_Click);
            this.CancelButton.Click += new RoutedEventHandler(CancelButton_Click);
        }

        #region Functon (OKButton_Click, CancelButton_Click)
        private async  void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValueCheckd())
            {
                int _Num = 0;
                //ProgramVo resultVo;
                if (isEdit == false)
                {
                    this.updateDao = getDomain();//this.updateDao
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("i5511/mst/i", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
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

                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("i5511/mst/u", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
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

                //InvVo resultVo;
                ////PurVo updateDao;
                //if (isEdit == false)
                //{
                //    updateDao = getDomain();//this.updateDao

                //    // 자동 번호 할당
                //    resultVo = invClient.I5511SelectInsrlNo(updateDao);
                //    if (!resultVo.isSuccess)
                //    {
                //        //실패
                //        WinUIMessageBox.Show("[입고 번호]" + resultVo.Message, "[에러]품목 입고 관리", MessageBoxButton.OK, MessageBoxImage.Error);
                //        return;
                //    }
                //    updateDao.INSRL_NO = resultVo.LST_FMT_NO;
                //    this.INSRL_NO = updateDao.INSRL_NO;
                //    this.text_INSRL_NO.Text = resultVo.LST_FMT_NO;
                //    //MessageBoxResult result = WinUIMessageBox.Show("[" + updateDao.INSRL_NO + "] 저장 하시겠습니까?", "[전표 번호]창고간 이동", MessageBoxButton.YesNo, MessageBoxImage.Question);
                //    //if (result == MessageBoxResult.Yes)
                //    //{
                //    resultVo = invClient.I5511InsertMst(updateDao);
                //    if (!resultVo.isSuccess)
                //    {
                //        //실패
                //        WinUIMessageBox.Show(resultVo.Message, "[에러]품목 입고 관리", MessageBoxButton.OK, MessageBoxImage.Error);
                //        return;
                //    }
                //    //성공
                //    WinUIMessageBox.Show("[입고 번호 : " + updateDao.INSRL_NO + "] 완료 되었습니다", "[추가]품목 입고 관리", MessageBoxButton.OK, MessageBoxImage.Information);
                //    //}
                //}
                //else
                //{
                //    updateDao = getDomain();
                //    resultVo = invClient.I5511UpdateMst(updateDao);
                //    if (!resultVo.isSuccess)
                //    {
                //        //실패
                //        WinUIMessageBox.Show(resultVo.Message, "[에러]품목 입고 관리", MessageBoxButton.OK, MessageBoxImage.Error);
                //        return;
                //    }
                //    //성공
                //    WinUIMessageBox.Show("완료 되었습니다", "[수정]품목 입고 관리", MessageBoxButton.OK, MessageBoxImage.Information);


                //    this.orgDao.INSRL_NO = this.updateDao.INSRL_NO;
                //    this.orgDao.INAUD_DT = this.updateDao.INAUD_DT;
                //    this.orgDao.AREA_CD = this.updateDao.AREA_CD;
                //    this.orgDao.AREA_NM = this.updateDao.AREA_NM;
                //    this.orgDao.CO_NO = this.updateDao.CO_NO;
                //    this.orgDao.CO_NM = this.updateDao.CO_NM;
                //    this.orgDao.RQST_EMPE_ID = this.updateDao.RQST_EMPE_ID;
                //    this.orgDao.RQST_EMPE_NM = this.updateDao.RQST_EMPE_NM;
                //    this.orgDao.IO_CD = this.updateDao.IO_CD;
                //    this.orgDao.INAUD_RMK = this.updateDao.INAUD_RMK;
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
            //if (string.IsNullOrEmpty(this.text_DO_RQST_NO.Text))
            //{
            //    WinUIMessageBox.Show("[전표 번호] 입력 값이 맞지 않습니다.", "[유효검사]창고간 이동", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.text_DO_RQST_NO.IsTabStop = true;
            //    this.text_DO_RQST_NO.Focus();
            //    return false;
            //}
            //else
            if (string.IsNullOrEmpty(this.text_INAUD_DT.Text))
            {
                WinUIMessageBox.Show("[입고 일자] 입력 값이 맞지 않습니다.", "[유효검사]품목 입고 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
                this.text_INAUD_DT.IsTabStop = true;
                this.text_INAUD_DT.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(this.combo_CO_NM.Text))
            {
                WinUIMessageBox.Show("[거래처] 입력 값이 맞지 않습니다.", "[유효검사]품목 입고 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
                this.combo_CO_NM.IsTabStop = true;
                this.combo_CO_NM.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(this.combo_RQST_EMPE_NM.Text))
            {
                WinUIMessageBox.Show("[작성자] 입력 값이 맞지 않습니다.", "[유효검사]품목 입고 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
                this.combo_RQST_EMPE_NM.IsTabStop = true;
                this.combo_RQST_EMPE_NM.Focus();
                return false;
            }
            //else if (string.IsNullOrEmpty(this.text_PUR_CLZ_FLG.Text))
            //{
            //    WinUIMessageBox.Show("[마감 유무] 입력 값이 맞지 않습니다.", "[유효검사]품목 입고 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.text_PUR_CLZ_FLG.IsTabStop = true;
            //    this.text_PUR_CLZ_FLG.Focus();
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
        private InvVo getDomain()
        {
            InvVo Dao = new InvVo();

            Dao.INSRL_NO = this.text_INSRL_NO.Text;
            Dao.INAUD_DT = Convert.ToDateTime(this.text_INAUD_DT.Text).ToString("yyyy-MM-dd");


            SystemCodeVo areaVo = this.combo_AREA_CD.SelectedItem as SystemCodeVo;
            Dao.AREA_CD = areaVo.CLSS_CD;
            Dao.AREA_NM = areaVo.CLSS_DESC;


            SystemCodeVo coNmVo = this.combo_CO_NM.SelectedItem as SystemCodeVo;
            Dao.CO_NO = coNmVo.CO_NO;
            Dao.CO_NM = coNmVo.CO_NM;


            GroupUserVo purEmpeIdVo = this.combo_RQST_EMPE_NM.SelectedItem as GroupUserVo;
            Dao.RQST_EMPE_ID = purEmpeIdVo.USR_ID;
            Dao.RQST_EMPE_NM = purEmpeIdVo.USR_N1ST_NM;


            Dao.INAUD_RMK = this.text_INAUD_RMK.Text;

            Dao.IO_CD = "I";

            Dao.CRE_USR_ID = SystemProperties.USER;
            Dao.UPD_USR_ID = SystemProperties.USER;
            Dao.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;

            return Dao;
        }
        #endregion


        public async void SYSTEM_CODE_VO()
        {
            //this.combo_RQST_EMPE_NM.ItemsSource = SystemProperties.USER_CODE_VO();
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s136/usr", new StringContent(JsonConvert.SerializeObject(new GroupUserVo() { DELT_FLG = "N", CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.combo_RQST_EMPE_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<GroupUserVo>>(await response.Content.ReadAsStringAsync()).Cast<GroupUserVo>().ToList();
                }
            }

            //this.combo_AREA_CD.ItemsSource = SystemProperties.SYSTEM_CODE_VO("L-002");
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "L-002"))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.combo_AREA_CD.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                }
            }

            //this.combo_CO_NM.ItemsSource = SystemProperties.CUSTOMER_CODE_VO(null, null);
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s143", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { DELT_FLG = "N", CO_TP_CD = null, AREA_CD = SystemProperties.USER_VO.EMPE_PLC_NM, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.combo_CO_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
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

        public string INSRL_NO
        { 
            get; 
            set;
        }

    }
}
