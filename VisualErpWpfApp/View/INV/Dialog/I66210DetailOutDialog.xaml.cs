using AquilaErpWpfApp3.Util;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Code;
using ModelsLibrary.Inv;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;

namespace AquilaErpWpfApp3.View.INV.Dialog
{
    public partial class I66210DetailOutDialog : DXWindow
    {

        private string _title = "품목 출고";
        //private static InvServiceClient invClient = SystemProperties.InvClient;
        private InvVo orgDao;
        private bool isEdit = false;
        private InvVo updateDao;

        public I66210DetailOutDialog(InvVo Dao)
        {
            InitializeComponent();

            //this.combo_RQST_EMPE_NM.ItemsSource = SystemProperties.USER_CODE_VO();
            //this.combo_AREA_CD.ItemsSource = SystemProperties.SYSTEM_CODE_VO("L-002");
            //this.combo_CO_NO.ItemsSource = SystemProperties.CUSTOMER_CODE_VO("AP");
            //this.combo_DO_RQST_TP_NM.ItemsSource = SystemProperties.SYSTEM_CODE_VO("INV07"); 
            //this.combo_RQST_EMPE_ID.ItemsSource = SystemProperties.SYSTEM_CODE_VO("HR04");

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

                this.INSRL_NO = this.text_INSRL_NO.Text;
            }
            else
            {
                //추가
                this.text_INSRL_NO.IsReadOnly = true;
                this.isEdit = false;
                copyDao.INAUD_DT = System.DateTime.Now.ToString("yyyy-MM-dd");
   
                //
                //Dao.DO_RQST_GRP_NM = SystemProperties.USERVO.GRP_NM;
                //Dao.DO_RQST_USR_NM = SystemProperties.USERVO.USR_N1ST_NM;
                //this.combo_DO_RQST_USR_NM.SelectedItem = ((IList<CodeDao>)combo_DO_RQST_USR_NM.ItemsSource)[2];
            }

            SYSTEM_CODE_VO();
            //this.combo_CO_NM.ItemsSource = SystemProperties.CUSTOMER_CODE_VO(null, copyDao.AREA_CD);


            this.text_INSRL_NO.Focus();

            this.configCode.DataContext = copyDao;
            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
            this.OKButton.Click += new RoutedEventHandler(OKButton_Click);
            this.CancelButton.Click += new RoutedEventHandler(CancelButton_Click);
        }


        public I66210DetailOutDialog()
        {
            InitializeComponent();

            SYSTEM_CODE_VO();


            this.text_INSRL_NO.Focus();

            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
            this.OKButton.Click += new RoutedEventHandler(OKButton_Click);
            this.CancelButton.Click += new RoutedEventHandler(CancelButton_Click);
        }



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
                        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("i66210/mst/i", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
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
                }
            }
            catch (Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }

            text_INSRL_NO.Clear();
            text_ITM_CD.Clear();
            text_BAT_CD.Clear();
            text_MST_LOT_NO.Clear();
            text_SER_NO.Clear();
            text_EXP_NO.Clear();
            text_ITM_QTY.Clear();
            text_INAUD_DT.Clear();
            combo_CO_NM.Clear();
            text_O_QTY.Clear();
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
            if (string.IsNullOrEmpty(this.text_INSRL_NO.Text))
            {
                WinUIMessageBox.Show("[바코드] 입력 값이 맞지 않습니다.", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.text_INSRL_NO.IsTabStop = true;
                this.text_INSRL_NO.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(this.text_INAUD_DT.Text))
            {
                WinUIMessageBox.Show("[출고 일자] 입력 값이 맞지 않습니다.", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.text_INAUD_DT.IsTabStop = true;
                this.text_INAUD_DT.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(this.combo_CO_NM.Text))
            {
                WinUIMessageBox.Show("[거래처] 입력 값이 맞지 않습니다.", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.combo_CO_NM.IsTabStop = true;
                this.combo_CO_NM.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(this.text_ITM_QTY.Text))
            {
                WinUIMessageBox.Show("[출고 수량] 입력 값이 맞지 않습니다.", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.text_ITM_QTY.IsTabStop = true;
                this.text_ITM_QTY.Focus();
                return false;
            }

            //else if (string.IsNullOrEmpty(this.text_PUR_CLZ_FLG.Text))
            //{
            //    WinUIMessageBox.Show("[마감 유무] 입력 값이 맞지 않습니다.", "[유효검사]품목 입고 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.text_PUR_CLZ_FLG.IsTabStop = true;
            //    this.text_PUR_CLZ_FLG.Focus();
            //    return false;
            //}

            return true;
        }
        #endregion

        #region Functon (getDomain - ConfigView1Dao)
        private InvVo getDomain()
        {
            InvVo Dao = new InvVo();

            Dao.INSRL_NO = this.text_INSRL_NO.Text;

     
            Dao.ITM_CD = this.text_ITM_CD.Text;
            Dao.BATCH_CD = this.text_BAT_CD.Text;
            Dao.MST_LOT_NO = this.text_MST_LOT_NO.Text;
            Dao.SER_NO = this.text_SER_NO.Text;
            Dao.EXP_NO = this.text_EXP_NO.Text;

            SystemCodeVo coNmVo = this.combo_CO_NM.SelectedItem as SystemCodeVo;
            Dao.CO_NO = coNmVo.CO_NO;
            Dao.CO_NM = coNmVo.CO_NM;


            Dao.INAUD_DT = Convert.ToDateTime(this.text_INAUD_DT.Text).ToString("yyyy-MM-dd");
            Dao.ITM_QTY = this.text_ITM_QTY.Text;
            Dao.MM_01 = this.text_O_QTY.Text;


            Dao.IO_CD = "O";


            //SystemCodeVo areaVo = this.combo_AREA_CD.SelectedItem as SystemCodeVo;
            //Dao.AREA_CD = areaVo.CLSS_CD;
            //Dao.AREA_NM = areaVo.CLSS_DESC;

            //GroupUserVo purEmpeIdVo = this.combo_RQST_EMPE_NM.SelectedItem as GroupUserVo;
            //Dao.RQST_EMPE_ID = purEmpeIdVo.USR_ID;
            //Dao.RQST_EMPE_NM = purEmpeIdVo.USR_N1ST_NM;

            //Dao.INAUD_RMK = this.text_INAUD_RMK.Text;

            Dao.CRE_USR_ID = SystemProperties.USER;
            Dao.UPD_USR_ID = SystemProperties.USER;
            Dao.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;

            return Dao;
        }
        #endregion


        public async void SYSTEM_CODE_VO()
        {
            //this.combo_RQST_EMPE_NM.ItemsSource = SystemProperties.USER_CODE_VO();
            //using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s136/usr", new StringContent(JsonConvert.SerializeObject(new GroupUserVo() { DELT_FLG = "N", CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
            //{
            //    if (response.IsSuccessStatusCode)
            //    {
            //        this.combo_RQST_EMPE_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<GroupUserVo>>(await response.Content.ReadAsStringAsync()).Cast<GroupUserVo>().ToList();
            //    }
            //}

            //this.combo_AREA_CD.ItemsSource = SystemProperties.SYSTEM_CODE_VO("L-002");
            //using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "L-002"))
            //{
            //    if (response.IsSuccessStatusCode)
            //    {
            //        this.combo_AREA_CD.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
            //    }
            //}

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

        private async void text_INSRL_NO_KeyDown(object sender, KeyEventArgs e)
        {
          
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                if (string.IsNullOrEmpty(text_INSRL_NO.Text))
                {
                    return;
                }

                try
                {
                    List<InvVo> tmpList = new List<InvVo>();
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("i66110/mst", new StringContent(JsonConvert.SerializeObject(new InvVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD, AREA_CD = "001", INSRL_NO = text_INSRL_NO.Text, IO_CD = "I" }), System.Text.Encoding.UTF8, "application/json")))
                    {
                        tmpList = JsonConvert.DeserializeObject<IEnumerable<InvVo>>(await response.Content.ReadAsStringAsync()).Cast<InvVo>().ToList();
                    }

                    if (tmpList.Count == 0)
                    {
                        WinUIMessageBox.Show("[ 바코드 : " + this.text_INSRL_NO.Text + "] 존재 하지 않습니다.", "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.None, MessageBoxOptions.None);
                        return;
                    }

                    text_INSRL_NO.Text = tmpList[0].INSRL_NO;
                    text_ITM_CD.Text = tmpList[0].ITM_CD;
                    text_BAT_CD.Text = tmpList[0].BATCH_CD;
                    text_MST_LOT_NO.Text = tmpList[0].MST_LOT_NO;
                    text_SER_NO.Text = tmpList[0].SER_NO;
                    text_EXP_NO.Text = tmpList[0].EXP_NO;
                    text_ITM_QTY.Text = tmpList[0].ITM_QTY.ToString();

                    //MessageBox.Show("엔터를 누르셨습니다");
                }
                catch (Exception eLog)
                {
                    WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                    return;
                }
            }
            else
            {
                return;
            }


        
           
            
        }

    }
}
