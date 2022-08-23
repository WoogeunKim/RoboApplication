using AquilaErpWpfApp3.Util;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Code;
using ModelsLibrary.Man;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;

namespace AquilaErpWpfApp3.View.M.Dialog
{
    /// <summary>
    /// Interaction logic for ConfigView1Dialog.xaml
    /// </summary>
    public partial class M6623MasterDialog : DXWindow
    {
        private string _title = "레시피자료관리";

        //private static CodeServiceClient codeClient = SystemProperties.CodeClient;
        //private static ManServiceClient manClient = SystemProperties.ManClient;

        //private ObservableCollection<SystemCodeVo> N1ST_ITM_GRP_LIST_1;
        //private ObservableCollection<SystemCodeVo> ASSY_ITM_LIST;

        //private ObservableCollection<SystemCodeVo> N1ST_ITM_GRP_LIST;
        //private ObservableCollection<SystemCodeVo> CMPO_LIST;

        private ManVo orgDao;
        private bool isEdit = false;
        private ManVo updateDao;

        public M6623MasterDialog(ManVo Dao, bool IsEdit = false)
        {
            InitializeComponent();

            
            // - this.combo_ITM_GRP_CLSS_CD.ItemsSource = SystemProperties.SYSTEM_CODE_VO("L-001");
            this.combo_ITM_GRP_CLSS_CD_1.SelectedIndexChanged += combo_ITM_GRP_CLSS_CD_1_SelectedIndexChanged;
            this.combo_N1ST_ITM_GRP_CD_1.SelectedIndexChanged += combo_N1ST_ITM_GRP_CD_1_SelectedIndexChanged;

            //
            this.combo_ITM_GRP_CLSS_CD.SelectedIndexChanged += combo_ITM_GRP_CLSS_CD_SelectedIndexChanged;
            //this.combo_N1ST_ITM_GRP_CD.SelectedIndexChanged += combo_N1ST_ITM_GRP_CD_SelectedIndexChanged;


            this.combo_CMPO_CD.SelectedIndexChanged += Combo_CMPO_CD_SelectedIndexChanged;


            // - this.combo_ITM_GRP_CLSS_CD_1.ItemsSource = SystemProperties.SYSTEM_CODE_VO("L-001");
            // -- this.combo_ITM_GRP_CLSS_CD.SelectedIndexChanged += combo_ITM_GRP_CLSS_CD_SelectedIndexChanged;
            // -- this.combo_N1ST_ITM_GRP_CD.SelectedIndexChanged += combo_N1ST_ITM_GRP_CD_SelectedIndexChanged;


            this.orgDao = Dao;
            ManVo copyDao = new ManVo()
            {
                ITM_GRP_CLSS_NM = Dao.ITM_GRP_CLSS_NM,
                ITM_GRP_CLSS_CD = Dao.ITM_GRP_CLSS_CD,

                N1ST_ITM_GRP_CD = Dao.N1ST_ITM_GRP_CD,
                N1ST_ITM_GRP_NM = Dao.N1ST_ITM_GRP_NM,

                ASSY_ITM_CD = Dao.ASSY_ITM_CD,
                ITM_NM = Dao.ITM_NM,
                ASSY_ITM_SEQ = Dao.ASSY_ITM_SEQ,
                BSE_WEIH_VAL = Dao.BSE_WEIH_VAL,
                CMPO_CD = Dao.CMPO_CD,
                CMPO_NM = Dao.CMPO_NM,
                MM_01 = Dao.CHNL_CD,
                WEIH_VAL = Dao.WEIH_VAL,
                CMPO_RMK = Dao.CMPO_RMK,
                MIN_TOR_VAL = Dao.MIN_TOR_VAL,
                MAX_TOR_VAL = Dao.MAX_TOR_VAL,
                CHNL_CD = Dao.CHNL_CD,
                ORD_CLS_CD = Dao.ORD_CLS_CD,
                CMPO_TOR_VAL = Dao.CMPO_TOR_VAL
            };

            //수정
            if (Dao.CMPO_CD != null)
            {
                //this.text_ASSY_ITM_SEQ.IsReadOnly = true;
                this.text_BSE_WEIH_VAL.IsReadOnly = true;

                //this.combo_ITM_GRP_CLSS_CD_1.IsReadOnly = true;
                //this.combo_N1ST_ITM_GRP_CD_1.IsReadOnly = true;
                this.combo_ASSY_ITM_CD.IsReadOnly = true;

                //this.combo_ITM_GRP_CLSS_CD.IsReadOnly = true;
                //this.combo_ITM_GRP_CLSS_CD.IsEnabled = false;
                //this.combo_N1ST_ITM_GRP_CD.IsReadOnly = true;
                //this.combo_N1ST_ITM_GRP_CD.IsEnabled = false;
                //this.combo_CMPO_CD.IsReadOnly = true;

               // this.isEdit = true;
                ////this.text_ASSY_ITM_SEQ.Foreground = Brushes.DarkGray;
                //this.text_ASSY_ITM_SEQ.Background = Brushes.DarkGray;
                //this.text_BSE_WEIH_VAL.Background = Brushes.DarkGray;

                //this.combo_ITM_GRP_CLSS_CD_1.Background = Brushes.DarkGray;
                //this.combo_N1ST_ITM_GRP_CD_1.Background = Brushes.DarkGray;
                //this.combo_ASSY_ITM_CD.Background = Brushes.DarkGray;


                //this.combo_ITM_GRP_CLSS_CD.Background = Brushes.DarkGray;
                //this.combo_N1ST_ITM_GRP_CD.Background = Brushes.DarkGray;
                //this.combo_CMPO_CD.Background = Brushes.DarkGray;


                //combo_N1ST_ITM_GRP_CD_2_SelectedIndexChanged();


                //수정 -> 분류, 대분류, 물품 코드
                //SYSTEM_CODE_VO_EDIT();

            }
            else
            {
                //추가
                //this.text_ClssTpCd.IsReadOnly = false;
                //this.isEdit = false;
                copyDao.WEIH_VAL = 0;
               // copyDao.INAUD_DT = System.DateTime.Now.ToString("yyyy-MM-dd");
               // copyDao.INAUD_PLC_NM = "본사";

                //this.combo_ITM_GRP_CLSS_CD.ItemsSource = SystemProperties.SYSTEM_CODE_VO("L-001");
                //this.combo_ITM_GRP_CLSS_CD.SelectedIndexChanged += combo_ITM_GRP_CLSS_CD_SelectedIndexChanged;
                //this.combo_N1ST_ITM_GRP_CD.SelectedIndexChanged += combo_N1ST_ITM_GRP_CD_SelectedIndexChanged;

                //this.combo_ITM_GRP_CLSS_CD.Text = "완제품";
                //this.combo_N1ST_ITM_GRP_CD.Text = "포장";
            }

            this.isEdit = IsEdit;



            //추가 -> 분류, 대분류, 물품 코드
            SYSTEM_CODE_VO();


            this.configCode.DataContext = copyDao;
            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
            this.OKButton.Click += new RoutedEventHandler(OKButton_Click);
            this.CancelButton.Click += new RoutedEventHandler(CancelButton_Click);
        }

        private void Combo_CMPO_CD_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            SystemCodeVo CMPO_NM = this.combo_CMPO_CD.SelectedItem as SystemCodeVo;
            if (CMPO_NM != null)
            {
                if (CMPO_NM.N1ST_ITM_GRP_CD.Equals("002"))
                {
                    //색소
                    this.text_CMPO_TOR_VAL.Text = "30";
                }
                else if (CMPO_NM.N1ST_ITM_GRP_CD.Equals("003"))
                {
                    //단일원료
                    this.text_CMPO_TOR_VAL.Text = "0.2";
                }
                else
                {
                    //기타
                    this.text_CMPO_TOR_VAL.Text = "0.5";
                }
                //Dao.CMPO_CD = CMPO_NM.ITM_CD;
                //Dao.CMPO_NM = CMPO_NM.ITM_NM;
            }
        }

        async void combo_N1ST_ITM_GRP_CD_1_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            SystemCodeVo ITM_GRP_CLSS_NM = this.combo_ITM_GRP_CLSS_CD_1.SelectedItem as SystemCodeVo;
             if (ITM_GRP_CLSS_NM == null)
             {
                 return;
             }

            SystemCodeVo N1ST_ITM_GRP_NM = this.combo_N1ST_ITM_GRP_CD_1.SelectedItem as SystemCodeVo;
             if (N1ST_ITM_GRP_NM == null)
             {
                 return;
             }

            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s141", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { DELT_FLG = "N", ITM_GRP_CLSS_CD = ITM_GRP_CLSS_NM.CLSS_CD, N1ST_ITM_GRP_CD = N1ST_ITM_GRP_NM.ITM_GRP_CD, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.combo_ASSY_ITM_CD.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                }
                //IList<SystemCodeVo> N1stVoList = codeClient.SelectItemList(new SystemCodeVo() { DELT_FLG = "N", ITM_GRP_CLSS_CD = ITM_GRP_CLSS_NM.CLSS_CD, N1ST_ITM_GRP_CD = N1ST_ITM_GRP_NM.CLSS_CD, CHNL_CD = SystemProperties.USER_VO.CHNL_CD });
                //int nCnt = N1stVoList.Count;
                //SystemCodeVo tmpVo;
                //ASSY_ITM_LIST = new List<SystemCodeVo>();
                //for (int x = 0; x < nCnt; x++)
                //{
                //    tmpVo = N1stVoList[x];
                //    ASSY_ITM_LIST.Add(new ItmCodeDao() { ITM_CD = tmpVo.ITM_CD, ITM_NM = tmpVo.ITM_NM, ITM_SZ_NM = tmpVo.ITM_SZ_NM, ITM_GRP_CLSS_NM = tmpVo.ITM_GRP_CLSS_NM, N1ST_ITM_GRP_NM = tmpVo.N1ST_ITM_GRP_NM, N2ND_ITM_GRP_NM = tmpVo.N2ND_ITM_GRP_NM });
                //}
                // = ASSY_ITM_LIST;
            }
        }

        async void combo_ITM_GRP_CLSS_CD_1_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            SystemCodeVo ITM_GRP_CLSS_NM = this.combo_ITM_GRP_CLSS_CD_1.SelectedItem as SystemCodeVo;
            if (ITM_GRP_CLSS_NM != null)
            {
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s133", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { DELT_FLG = "N", ITM_GRP_CLSS_CD = ITM_GRP_CLSS_NM.CLSS_CD, CRE_USR_ID = "", CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.combo_N1ST_ITM_GRP_CD_1.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                    }
                }
            }

            //IList<SystemCodeVo> N1stVoList = codeClient.SelectCodeItemGroupList(new SystemCodeVo() { DELT_FLG = "N", ITM_GRP_CLSS_CD = ITM_GRP_CLSS_NM.CLSS_CD, CRE_USR_ID = "", CHNL_CD = SystemProperties.USER_VO.CHNL_CD });
            //int nCnt = N1stVoList.Count;
            // SystemCodeVo tmpVo;
            // N1ST_ITM_GRP_LIST_1 = new List<CodeDao>();
            // for (int x = 0; x < nCnt; x++)
            // {
            //     tmpVo = N1stVoList[x];
            //     N1ST_ITM_GRP_LIST_1.Add(new CodeDao() { CLSS_DESC = tmpVo.ITM_GRP_NM, CLSS_CD = tmpVo.ITM_GRP_CD });
            // }
            // this.combo_N1ST_ITM_GRP_CD_1.ItemsSource = N1ST_ITM_GRP_LIST_1;
            //}
        }

        //
        async void combo_N1ST_ITM_GRP_CD_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            SystemCodeVo ITM_GRP_CLSS_NM = this.combo_ITM_GRP_CLSS_CD.SelectedItem as SystemCodeVo;
            if (ITM_GRP_CLSS_NM == null)
            {
                return;
            }

            //SystemCodeVo N1ST_ITM_GRP_NM = this.combo_N1ST_ITM_GRP_CD.SelectedItem as SystemCodeVo;
            //if (N1ST_ITM_GRP_NM == null)
            //{
            //    return;
            //}

            //using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s141", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { DELT_FLG = "N", ITM_GRP_CLSS_CD = ITM_GRP_CLSS_NM.CLSS_CD, N1ST_ITM_GRP_CD = N1ST_ITM_GRP_NM.ITM_GRP_CD, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
            //{
            //    if (response.IsSuccessStatusCode)
            //    {
            //        this.combo_CMPO_CD.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
            //    }


            //    //IList<SystemCodeVo> N1stVoList = codeClient.SelectItemList(new SystemCodeVo() { DELT_FLG = "N", ITM_GRP_CLSS_CD = ITM_GRP_CLSS_NM.CLSS_CD, N1ST_ITM_GRP_CD = N1ST_ITM_GRP_NM.CLSS_CD, CHNL_CD = SystemProperties.USER_VO.CHNL_CD });
            //    //int nCnt = N1stVoList.Count;
            //    //SystemCodeVo tmpVo;

            //    //CMPO_LIST = new List<ItmCodeDao>();
            //    //for (int x = 0; x < nCnt; x++)
            //    //{
            //    //    tmpVo = N1stVoList[x];
            //    //    CMPO_LIST.Add(new ItmCodeDao() { ITM_CD = tmpVo.ITM_CD, ITM_NM = tmpVo.ITM_NM, ITM_SZ_NM = tmpVo.ITM_SZ_NM, ITM_GRP_CLSS_NM = tmpVo.ITM_GRP_CLSS_NM, N1ST_ITM_GRP_NM = tmpVo.N1ST_ITM_GRP_NM, N2ND_ITM_GRP_NM = tmpVo.N2ND_ITM_GRP_NM });
            //    //}
            //    //this.combo_CMPO_CD.ItemsSource = CMPO_LIST;
            //}
        }
        async void combo_ITM_GRP_CLSS_CD_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            SystemCodeVo ITM_GRP_CLSS_NM = this.combo_ITM_GRP_CLSS_CD.SelectedItem as SystemCodeVo;
            if (ITM_GRP_CLSS_NM != null)
            {

                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s141", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { DELT_FLG = "N", ITM_GRP_CLSS_CD = ITM_GRP_CLSS_NM.CLSS_CD, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.combo_CMPO_CD.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                    }
                }

                //using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s133", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { DELT_FLG = "N", ITM_GRP_CLSS_CD = ITM_GRP_CLSS_NM.CLSS_CD, CRE_USR_ID = "", CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                //{
                //    if (response.IsSuccessStatusCode)
                //    {
                //        this.combo_N1ST_ITM_GRP_CD.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                //    }
                //}
                //IList<SystemCodeVo> N1stVoList = codeClient.SelectCodeItemGroupList(new SystemCodeVo() { DELT_FLG = "N", ITM_GRP_CLSS_CD = ITM_GRP_CLSS_NM.CLSS_CD, CRE_USR_ID = "", CHNL_CD = SystemProperties.USER_VO.CHNL_CD });
                //int nCnt = N1stVoList.Count;
                //SystemCodeVo tmpVo;

                //N1ST_ITM_GRP_LIST = new List<SystemCodeVo>();
                //for (int x = 0; x < nCnt; x++)
                //{
                //    tmpVo = N1stVoList[x];
                //    N1ST_ITM_GRP_LIST.Add(new CodeDao() { CLSS_DESC = tmpVo.ITM_GRP_NM, CLSS_CD = tmpVo.ITM_GRP_CD });
                //}
                //this.combo_N1ST_ITM_GRP_CD.ItemsSource = N1ST_ITM_GRP_LIST;
            }
        }

        //
        //
        //
        //async void combo_N1ST_ITM_GRP_CD_2_SelectedIndexChanged()
        //{
        //    //CodeDao ITM_GRP_CLSS_NM = this.combo_ITM_GRP_CLSS_CD.SelectedItem as CodeDao;
        //    //if (ITM_GRP_CLSS_NM == null)
        //    //{
        //    //    return;
        //    //}

        //    //CodeDao N1ST_ITM_GRP_NM = this.combo_N1ST_ITM_GRP_CD.SelectedItem as CodeDao;
        //    //if (N1ST_ITM_GRP_NM == null)
        //    //{
        //    //    return;
        //    //}


        //    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s141", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { DELT_FLG = "N", CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
        //    {
        //        if (response.IsSuccessStatusCode)
        //        {
        //            this.combo_CMPO_CD.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
        //        }
        //    }

        //    //IList<SystemCodeVo> N1stVoList = codeClient.SelectItemList(new SystemCodeVo() { DELT_FLG = "N", CHNL_CD = SystemProperties.USER_VO.CHNL_CD });
        //    //int nCnt = N1stVoList.Count;
        //    //SystemCodeVo tmpVo;

        //    //CMPO_LIST = new List<SystemCodeVo>();
        //    //for (int x = 0; x < nCnt; x++)
        //    //{
        //    //    tmpVo = N1stVoList[x];
        //    //    CMPO_LIST.Add(new ItmCodeDao() { ITM_CD = tmpVo.ITM_CD, ITM_NM = tmpVo.ITM_NM, ITM_SZ_NM = tmpVo.ITM_SZ_NM, ITM_GRP_CLSS_NM = tmpVo.ITM_GRP_CLSS_NM, N1ST_ITM_GRP_NM = tmpVo.N1ST_ITM_GRP_NM, N2ND_ITM_GRP_NM = tmpVo.N2ND_ITM_GRP_NM });
        //    //}
        //    //this.combo_CMPO_CD.ItemsSource = CMPO_LIST;
        //}
        //


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
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6623/mst/i", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
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

                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6623/mst/u", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
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


                //ManVo resultVo;
                //if (isEdit == false)
                //{
                //    this.updateDao = getDomain();
                //    resultVo = manClient.M6623InsertMaster(this.updateDao);
                //    if (!resultVo.isSuccess)
                //    {
                //        //실패
                //        WinUIMessageBox.Show(resultVo.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error);
                //        return;
                //    }

                //    //성공
                //    WinUIMessageBox.Show("완료 되었습니다", "[추가]" + _title, MessageBoxButton.OK, MessageBoxImage.Information);
                //}
                //else
                //{
                //    this.updateDao = getDomain();
                //    resultVo = manClient.M6623UpdateMaster(this.updateDao);
                //    if (!resultVo.isSuccess)
                //    {
                //        //실패
                //        WinUIMessageBox.Show(resultVo.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error);
                //        return;
                //    }
                //    //성공
                //    WinUIMessageBox.Show("완료 되었습니다", "[수정]" + _title, MessageBoxButton.OK, MessageBoxImage.Information);

                //    this.orgDao.ASSY_ITM_CD = this.updateDao.ASSY_ITM_CD;
                //    this.orgDao.ITM_NM = this.updateDao.ITM_NM;
                //    this.orgDao.BSE_WEIH_VAL = this.updateDao.BSE_WEIH_VAL;
                //    this.orgDao.ASSY_ITM_SEQ = this.updateDao.ASSY_ITM_SEQ;
                //    this.orgDao.CMPO_CD = this.updateDao.CMPO_CD;
                //    this.orgDao.CMPO_NM = this.updateDao.CMPO_NM;
                //    this.orgDao.WEIH_VAL = this.updateDao.WEIH_VAL;
                //    this.orgDao.MIN_TOR_VAL = this.updateDao.MIN_TOR_VAL;
                //    this.orgDao.MAX_TOR_VAL = this.updateDao.MAX_TOR_VAL;
                //    //this.orgDao.CMPO_TOR_VAL = this.updateDao.CMPO_TOR_VAL;
                //    this.orgDao.CMPO_RMK = this.updateDao.CMPO_RMK;
                //    this.orgDao.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
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
            if (string.IsNullOrEmpty(this.text_BSE_WEIH_VAL.Text))
            {
                WinUIMessageBox.Show("[기준 중량(Kg)] 입력 값이 맞지 않습니다.", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.text_BSE_WEIH_VAL.IsTabStop = true;
                this.text_BSE_WEIH_VAL.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(this.text_ASSY_ITM_SEQ.Text))
            {
                WinUIMessageBox.Show("[순번] 입력 값이 맞지 않습니다.", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.text_ASSY_ITM_SEQ.IsTabStop = true;
                this.text_ASSY_ITM_SEQ.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(this.combo_ASSY_ITM_CD.Text))
            {
                WinUIMessageBox.Show("[벌크 코드] 입력 값이 맞지 않습니다.", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.combo_ASSY_ITM_CD.IsTabStop = true;
                this.combo_ASSY_ITM_CD.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(this.combo_CMPO_CD.Text))
            {
                WinUIMessageBox.Show("[원료] 입력 값이 맞지 않습니다.", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.combo_CMPO_CD.IsTabStop = true;
                this.combo_CMPO_CD.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(this.text_ORD_CLS_CD.Text))
            {
                WinUIMessageBox.Show("[상] 입력 값이 맞지 않습니다.", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.text_ORD_CLS_CD.IsTabStop = true;
                this.text_ORD_CLS_CD.Focus();
                return false;
            }
            //else if (string.IsNullOrEmpty(this.combo_INAUD_PLC_NM.Text))
            //{
            //    WinUIMessageBox.Show("[입고처] 입력 값이 맞지 않습니다.", "[유효검사]자재 입고 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.combo_INAUD_PLC_NM.IsTabStop = true;
            //    this.combo_INAUD_PLC_NM.Focus();
            //    return false;
            //}
            //else if (string.IsNullOrEmpty(this.combo_RQST_EMPE_NM.Text))
            //{
            //    WinUIMessageBox.Show("[요청자] 입력 값이 맞지 않습니다.", "[유효검사]자재 입고 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.combo_RQST_EMPE_NM.IsTabStop = true;
            //    this.combo_RQST_EMPE_NM.Focus();
            //    return false;
            //}
            //else if (this.combo_deltFlg.Text == null || this.combo_deltFlg.Text.Trim().Length == 0)
            //{
            //    WinUIMessageBox.Show("[사용 여부] 입력 값이 맞지 않습니다.", "[유효검사]자재 입고 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.combo_deltFlg.IsTabStop = true;
            //    this.combo_deltFlg.Focus();
            //    return false;
            //}
            else
            {
                //if (this.isEdit == false)
                //{
                //    ItmCodeDao ASSY_ITM_CD = this.combo_ASSY_ITM_CD.SelectedItem as ItmCodeDao;
                //    ManVo dao = new ManVo()
                //    {
                //        ASSY_ITM_CD = ASSY_ITM_CD.ITM_CD,
                //        BSE_WEIH_VAL = int.Parse(this.text_BSE_WEIH_VAL.Text),
                //        CHNL_CD = SystemProperties.USER_VO.CHNL_CD,
                //        ASSY_ITM_SEQ = int.Parse(this.text_ASSY_ITM_SEQ.Text)

                //    };
                //    IList<ManVo> daoList = (IList<ManVo>)manClient.M6623SelectDetail(dao);
                //    if (daoList.Count != 0)
                //    {
                //        WinUIMessageBox.Show("[배합표 No. - 중복] 코드를 다시 입력 하십시오.", "[중복검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                //        this.text_BSE_WEIH_VAL.IsTabStop = true;
                //        this.text_BSE_WEIH_VAL.Focus();
                //        return false;
                //    }
                //}
            }
            return true;
        }
        #endregion

        #region Functon (getDomain - ConfigView1Dao)
        private ManVo getDomain()
        {
            ManVo Dao = new ManVo();

            SystemCodeVo ASSY_ITM_CD = this.combo_ASSY_ITM_CD.SelectedItem as SystemCodeVo;
            if (ASSY_ITM_CD != null)
            {
                Dao.ASSY_ITM_CD = ASSY_ITM_CD.ITM_CD;
                Dao.ITM_NM = ASSY_ITM_CD.ITM_NM;
            }

            Dao.BSE_WEIH_VAL = int.Parse(this.text_BSE_WEIH_VAL.Text);
            Dao.ASSY_ITM_SEQ = int.Parse(this.text_ASSY_ITM_SEQ.Text);
            //

            SystemCodeVo CMPO_NM = this.combo_CMPO_CD.SelectedItem as SystemCodeVo;
            if (CMPO_NM != null)
            {
                Dao.CMPO_CD = CMPO_NM.ITM_CD;
                Dao.CMPO_NM = CMPO_NM.ITM_NM;
            }
            
            Dao.ORD_CLS_CD = this.text_ORD_CLS_CD.Text;

            Dao.WEIH_VAL = this.text_WEIH_VAL.Text;
            Dao.CMPO_TOR_VAL = this.text_CMPO_TOR_VAL.Text;

            Dao.CMPO_RMK = this.text_CMPO_RMK.Text;

            Dao.CRE_USR_ID = SystemProperties.USER;
            Dao.UPD_USR_ID = SystemProperties.USER;
            Dao.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
            return Dao;
        }
        #endregion

        public async void SYSTEM_CODE_VO()
        {
            try
            {
                DXSplashScreen.Show<ProgressWindow>();

                //this.combo_ITM_GRP_CLSS_CD_1.ItemsSource = SystemProperties.SYSTEM_CODE_VO("L-001");
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "L-001"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.combo_ITM_GRP_CLSS_CD.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                        this.combo_ITM_GRP_CLSS_CD.SelectedItem = (this.combo_ITM_GRP_CLSS_CD.ItemsSource as List<SystemCodeVo>).Where(x => x.CLSS_CD.Equals("M")).FirstOrDefault<SystemCodeVo>();



                        //
                        this.combo_ITM_GRP_CLSS_CD_1.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                        this.combo_ITM_GRP_CLSS_CD_1.SelectedItem = (this.combo_ITM_GRP_CLSS_CD_1.ItemsSource as List<SystemCodeVo>).Where(x => x.CLSS_CD.Equals("G")).LastOrDefault<SystemCodeVo>();
                    }
                }

                //
                using (HttpResponseMessage responseX = await SystemProperties.PROGRAM_HTTP.PostAsync("s133", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { DELT_FLG = "N", ITM_GRP_CLSS_CD = "G", CRE_USR_ID = "", CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (responseX.IsSuccessStatusCode)
                    {
                        this.combo_N1ST_ITM_GRP_CD_1.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await responseX.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                        this.combo_N1ST_ITM_GRP_CD_1.SelectedItem = (this.combo_N1ST_ITM_GRP_CD_1.ItemsSource as List<SystemCodeVo>).Where(x => x.ITM_GRP_CD.Equals("G13")).LastOrDefault<SystemCodeVo>();
                    }
                }



                //수정
                if (this.orgDao.CMPO_CD != null)
                {
                    using (HttpResponseMessage responseY = await SystemProperties.PROGRAM_HTTP.PostAsync("s141", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { DELT_FLG = "N", ITM_GRP_CLSS_CD = "G", N1ST_ITM_GRP_CD = "G13", CHNL_CD = SystemProperties.USER_VO.CHNL_CD, ITM_CD = this.orgDao.ASSY_ITM_CD }), System.Text.Encoding.UTF8, "application/json")))
                    {
                        if (responseY.IsSuccessStatusCode)
                        {
                            this.combo_ASSY_ITM_CD.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await responseY.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                            if (string.IsNullOrEmpty(this.orgDao.ASSY_ITM_CD))
                            {
                                this.combo_ASSY_ITM_CD.SelectedItem = (this.combo_ASSY_ITM_CD.ItemsSource as List<SystemCodeVo>)[0];
                                //this.combo_ASSY_ITM_CD.SelectedItem = (this.combo_ASSY_ITM_CD.ItemsSource as List<SystemCodeVo>).Where(x => x.ITM_CD.Equals(this.orgDao.ASSY_ITM_CD)).LastOrDefault<SystemCodeVo>();
                            }
                        }
                    }
                }
                else
                {
                    using (HttpResponseMessage responseY = await SystemProperties.PROGRAM_HTTP.PostAsync("s141", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { DELT_FLG = "N", ITM_GRP_CLSS_CD = "G", N1ST_ITM_GRP_CD = "G13", CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                    {
                        if (responseY.IsSuccessStatusCode)
                        {
                            this.combo_ASSY_ITM_CD.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await responseY.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                            if (string.IsNullOrEmpty(this.orgDao.ASSY_ITM_CD))
                            {
                                this.combo_ASSY_ITM_CD.SelectedItem = (this.combo_ASSY_ITM_CD.ItemsSource as List<SystemCodeVo>)[0];
                                //this.combo_ASSY_ITM_CD.SelectedItem = (this.combo_ASSY_ITM_CD.ItemsSource as List<SystemCodeVo>).Where(x => x.ITM_CD.Equals(this.orgDao.ASSY_ITM_CD)).LastOrDefault<SystemCodeVo>();
                            }
                        }
                    }
                }

                DXSplashScreen.Close();
            }
            catch (Exception)
            {
                if (DXSplashScreen.IsActive == true)
                {
                    DXSplashScreen.Close();
                }
                return;
            }
        }


        //public async void SYSTEM_CODE_VO_EDIT()
        //{

        //    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "L-001"))
        //    {
        //        if (response.IsSuccessStatusCode)
        //        {
        //            this.combo_ITM_GRP_CLSS_CD.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
        //            this.combo_ITM_GRP_CLSS_CD.SelectedItem = (this.combo_ITM_GRP_CLSS_CD.ItemsSource as List<SystemCodeVo>).Where(x => x.CLSS_DESC.Equals("원자재")).FirstOrDefault<SystemCodeVo>();
        //        }


        //        using (HttpResponseMessage responseX = await SystemProperties.PROGRAM_HTTP.PostAsync("s133", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { DELT_FLG = "N", ITM_GRP_CLSS_CD = ITM_GRP_CLSS_NM.CLSS_CD, CRE_USR_ID = "", CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
        //        {
        //            if (responseX.IsSuccessStatusCode)
        //            {
        //                this.combo_N1ST_ITM_GRP_CD.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await responseX.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
        //                this.combo_N1ST_ITM_GRP_CD.SelectedItem = (this.combo_N1ST_ITM_GRP_CD.ItemsSource as List<SystemCodeVo>).Where(x => x.ITM_GRP_CD.Equals(this.orgDao.MM_01)).LastOrDefault<SystemCodeVo>();
        //            }
        //        }
        //    }


        //    //using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s141", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { DELT_FLG = "N", ITM_GRP_CLSS_CD = ITM_GRP_CLSS_NM.CLSS_CD, N1ST_ITM_GRP_CD = N1ST_ITM_GRP_NM.CLSS_CD, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
        //    //{
        //    //    if (response.IsSuccessStatusCode)
        //    //    {
        //    //        this.combo_CMPO_CD.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
        //    //    }

        //    //}

        //}


           


        //IsEdit
        public bool IsEdit
        {
            get
            {
                return this.isEdit;
            }
        }


        public ManVo resultVo
        {
            get
            {
                return this.updateDao;
            }
        }
    }
}
