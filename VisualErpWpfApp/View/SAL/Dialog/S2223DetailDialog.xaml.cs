using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Code;
using ModelsLibrary.Sale;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using AquilaErpWpfApp3.Util;

namespace AquilaErpWpfApp3.View.SAL.Dialog
{
    public partial class S2223DetailDialog : DXWindow
    {
        private SaleVo orgDao;
        IList<SaleVo> saveItems = new List<SaleVo>();

        private string title = "수주 등록 물품 관리";

        public bool IsChecked { get; set; }

        public S2223DetailDialog(SaleVo Dao)
        {
            InitializeComponent();

            this.orgDao = Dao;

            SYSTEM_CODE_VO();

            this.ViewGridRight1.MouseDoubleClick += ViewGridRight1_MouseDoubleClick;

            this.btn_Search.Click += new RoutedEventHandler(btn_Search_Click);
            this.OKButton.Click += new RoutedEventHandler(OKButton_Click);
            this.CancelButton.Click += new RoutedEventHandler(CancelButton_Click);


            this.text_ITM_CD.Focus();
        }


        private async void ViewGridRight1_Refresh()
        {
            try
            {
                DXSplashScreen.Show<ProgressWindow>();

                if (this.combo_ITM_GRP_CLSS_NM.SelectedItem == null) return;

                SaleVo PopupVo = new SaleVo();
                PopupVo.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
                PopupVo.ITM_GRP_CLSS_CD = (this.combo_ITM_GRP_CLSS_NM.SelectedItem as SystemCodeVo).CLSS_CD;

                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s2223/popupitems", new StringContent(JsonConvert.SerializeObject(PopupVo), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.ViewGridRight1.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SaleVo>>(await response.Content.ReadAsStringAsync()).Cast<SaleVo>().ToList();
                    }
                }

                if (DXSplashScreen.IsActive == true)
                {
                    DXSplashScreen.Close();
                }
            }
            catch (Exception eLog)
            {
                if (DXSplashScreen.IsActive == true)
                {
                    DXSplashScreen.Close();
                }

                WinUIMessageBox.Show(eLog.Message, "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        void btn_Search_Click(object sender, RoutedEventArgs e)
        {
            ViewGridRight1_Refresh();
        }


        IList<SaleVo> tmpDao = new List<SaleVo>();
        void ViewGridRight1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                tmpDao = this.ViewGridRight2.ItemsSource as List<SaleVo>;

                SaleVo selVo = (SaleVo)this.ViewGridRight1.SelectedItem;
                if (selVo == null)
                {
                    return;
                }
                else
                {
                    this.MSG.Text = "";
                }

                SaleVo insertDao = new SaleVo();
                insertDao.SL_RLSE_NO = this.orgDao.SL_RLSE_NO;
                insertDao.SL_ITM_CD = selVo.ITM_CD;
                insertDao.ITM_CD = selVo.ITM_CD;
                insertDao.ITM_NM = selVo.ITM_NM;
                insertDao.CRE_USR_ID = SystemProperties.USER;
                insertDao.UPD_USR_ID = SystemProperties.USER;
                insertDao.AREA_CD = SystemProperties.USER_VO.EMPE_PLC_NM;
                insertDao.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
                insertDao.SL_ITM_QTY = selVo.SL_ITM_QTY;
                insertDao.ITM_QTY = 0;

                tmpDao.Add(insertDao);

                //한 줄출력
                this.ViewGridRight2.ItemsSource = tmpDao;
                this.ViewTableRight2.FocusedRowHandle = this.ViewGridRight2.VisibleRowCount - 1;
                this.ViewGridRight2.RefreshData();
                    //}
                //}
            }
            catch (Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

        }

        private Boolean GetList()
        {
            /// try
            saveItems = this.ViewGridRight2.ItemsSource as List<SaleVo>;

            int _Num = 0;

            if (saveItems.Any<SaleVo>(s => (s.SL_ITM_QTY <= 0)))
            {
                WinUIMessageBox.Show("수량값이 잘못되었습니다.", "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (saveItems.Any<SaleVo>(s => (int.TryParse(s.N1ST_ITM_SZ_NM, out _Num) == false)))
            {
                WinUIMessageBox.Show("가로 값을 입력해야합니다.", "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (saveItems.Any<SaleVo>(s => (int.TryParse(s.N2ND_ITM_SZ_NM, out _Num) == false)))
            {
                WinUIMessageBox.Show("세로 값을 입력해야합니다", "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (saveItems.Any<SaleVo>(s => (int.TryParse(s.N3RD_ITM_SZ_NM, out _Num) == false)))
            {
                WinUIMessageBox.Show("높이 값을 입력해야합니다", "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }


            foreach(SaleVo item in saveItems)
            {
                item.CRE_USR_ID = SystemProperties.USER;
                item.UPD_USR_ID = SystemProperties.USER;
                item.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
            }

            return true;
        }


        #region Functon (OKButton_Click, CancelButton_Click)
        private async void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int _Num = 0;
                
                if (GetList())
                {
                    if (saveItems.Count == 0)
                    {
                        //전체 삭제
                        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s2223/dtl/d", new StringContent(JsonConvert.SerializeObject(this.orgDao), System.Text.Encoding.UTF8, "application/json")))
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
                                WinUIMessageBox.Show("[요청 번호 : " + this.orgDao.SL_RLSE_NO + "] 완료 되었습니다", title, MessageBoxButton.OK, MessageBoxImage.Information);
                                this.DialogResult = true;
                                this.Close();
                            }
                        }
                    }

                    //건 바이 입력
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s2223/dtl/i", new StringContent(JsonConvert.SerializeObject(saveItems), System.Text.Encoding.UTF8, "application/json")))
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
                            WinUIMessageBox.Show("[요청 번호 : " + this.orgDao.SL_RLSE_NO + "] 완료 되었습니다", title, MessageBoxButton.OK, MessageBoxImage.Information);
                            this.DialogResult = true;
                            this.Close();


                        }
                    }
                }
            }
            catch (Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        #endregion


        /// <summary>
        /// 검색창 KeyUp
        /// </summary>
        private void HandleKey(object sender, KeyEventArgs e)
        {
            try
            {
                this.ViewTableRight1.SearchString = text_ITM_CD.Text;
            }
            catch (Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        //IsEdit
        public bool IsEdit
        {
            get
            {
                return this.IsEdit;
            }
        }

        private void GridColumn_Validate(object sender, DevExpress.Xpf.Grid.GridCellValidationEventArgs e)
        {
            SaleVo masterDomain = (SaleVo)ViewGridRight2.GetFocusedRow();

            string number = string.Empty;

            number = e.Value.ToString();

            try
            {
                bool slItmQty = (e.Column.FieldName.ToString().Equals("SL_ITM_QTY") ? true : false);
                bool slItmRmk = (e.Column.FieldName.ToString().Equals("SL_ITM_RMK") ? true : false);
                bool n1stItmSzNm = (e.Column.FieldName.ToString().Equals("N1ST_ITM_SZ_NM") ? true : false);
                bool n2ndItmSzNm = (e.Column.FieldName.ToString().Equals("N2ND_ITM_SZ_NM") ? true : false);
                bool n3rdItmSzNm = (e.Column.FieldName.ToString().Equals("N3RD_ITM_SZ_NM") ? true : false);          

                if (slItmRmk)
                {
                    if (e.IsValid)
                    {
                        if (string.IsNullOrEmpty(masterDomain.SL_ITM_RMK))
                        {
                            masterDomain.SL_ITM_RMK = "";
                        }
                        
                        if (!masterDomain.SL_ITM_RMK.Equals((e.Value == null ? "" : e.Value.ToString())))
                        {
                            masterDomain.SL_ITM_RMK = e.Value.ToString();
                        }
                    }
                }

                else if (slItmQty)
                {
                    if (e.IsValid)
                    {
                        if (string.IsNullOrEmpty(masterDomain.SL_ITM_QTY + ""))
                        {
                            masterDomain.SL_ITM_QTY = 0;
                        }
                        
                        if (!masterDomain.SL_ITM_QTY.Equals((e.Value == null ? "0" : e.Value.ToString())))
                        {
                            masterDomain.SL_ITM_QTY = int.Parse(e.Value.ToString());
                        }
                    }
                }

                // 가로, 세로, 높이
                else if (n1stItmSzNm)
                {
                    if (e.IsValid)
                    {
                        if(string.IsNullOrEmpty(masterDomain.N1ST_ITM_SZ_NM))
                        {
                            masterDomain.N1ST_ITM_SZ_NM = "";
                        }
                        if(!masterDomain.N1ST_ITM_SZ_NM.Equals((e.Value == null ? "" : e.Value.ToString())))
                        {
                            if (!IsNumeric(number) == true)
                            {
                                WinUIMessageBox.Show("가로는 숫자로 입력해야 합니다", "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                                return; 
                            }
                            masterDomain.N1ST_ITM_SZ_NM = e.Value.ToString();
                        }
                    }
                }
                
                else if (n2ndItmSzNm)
                {
                    if (e.IsValid)
                    {
                        if(string.IsNullOrEmpty(masterDomain.N2ND_ITM_SZ_NM))
                        {
                            masterDomain.N2ND_ITM_SZ_NM = "";
                        }
                        if(!masterDomain.N2ND_ITM_SZ_NM.Equals((e.Value == null ? "" : e.Value.ToString())))
                        {
                            if (IsNumeric(number) == true)
                            {
                                masterDomain.N2ND_ITM_SZ_NM = e.Value.ToString();
                            }
                            else
                            {
                                WinUIMessageBox.Show("세로는 숫자로 입력해야합니다", "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                                return;
                            }
                        }
                    }
                }
                
                else if (n3rdItmSzNm)
                {
                    if (e.IsValid)
                    {
                        if(string.IsNullOrEmpty(masterDomain.N3RD_ITM_SZ_NM))
                        {
                            masterDomain.N3RD_ITM_SZ_NM = "";
                        }
                        if(!masterDomain.N3RD_ITM_SZ_NM.Equals((e.Value == null ? "" : e.Value.ToString())))
                        {
                            if (IsNumeric(number) == true)
                            {
                                masterDomain.N3RD_ITM_SZ_NM = e.Value.ToString();
                            }
                            else
                            {
                                WinUIMessageBox.Show("높이는 숫자로 입력해야합니다", "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                                return;
                            }
                        }
                    }
                }
            }
            catch (Exception eLog)
            {
                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                e.ErrorContent = eLog.Message;
                e.SetError(e.ErrorContent, e.ErrorType);
                this.MSG.Text = e.ErrorContent.ToString();
                return;
            }
        }

        private static bool IsNumeric(string input)
        {
            int number = 0;
            return int.TryParse(input, out number);
        }

        private void ViewGridRight2_HiddenEditor(object sender, DevExpress.Xpf.Grid.EditorEventArgs e)
        {
            this.ViewTableRight2.CommitEditing();
        }


        private void btn_del_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaleVo selVo = (SaleVo)ViewGridRight2.GetFocusedRow();
                if (selVo == null)
                {
                    return;
                }

                MessageBoxResult result = WinUIMessageBox.Show("[" + selVo.SL_ITM_CD + "] 정말로 삭제 하시겠습니까?", title, MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    this.tmpDao.Remove(selVo);
                    this.ViewGridRight2.ItemsSource = this.tmpDao;
                    this.ViewGridRight2.RefreshData();
                }
            }
            catch (Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }

        public async void SYSTEM_CODE_VO()
        {
            try
            {
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "L-001"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.combo_ITM_GRP_CLSS_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                        this.combo_ITM_GRP_CLSS_NM.SelectedIndex = 2;
                    }
                }

                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s2223/popup", new StringContent(JsonConvert.SerializeObject(this.orgDao), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        tmpDao = JsonConvert.DeserializeObject<IEnumerable<SaleVo>>(await response.Content.ReadAsStringAsync()).Cast<SaleVo>().ToList();
                        this.ViewGridRight2.ItemsSource = tmpDao;
                    }
                }


                ViewGridRight1_Refresh();
            }
            catch (Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
            
        }

        
    }
}
