using AquilaErpWpfApp3.Util;
using DevExpress.Xpf.Core;
using ModelsLibrary.Code;
using ModelsLibrary.Pur;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.WindowsUI;

namespace AquilaErpWpfApp3.View.PUR.Dialog
{
    /// <summary>
    /// P4430InsertDtlDialog.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class P4430InsertDtlDialog : DXWindow
    {
        private string _title = "발주 자재 추가";

        private PurVo orgDao;

        private IList<PurVo> SelectItems;
        public P4430InsertDtlDialog(PurVo vo)
        {
            InitializeComponent();

            SYSTEM_CODE_VO();

            this.orgDao = vo;

            this.btn_ITEMS.Click += btn_Items_Click;

            this.OKButton.Click += new RoutedEventHandler(OKButton_Click);
            this.CancelButton.Click += new RoutedEventHandler(CancelButton_Click);


        }


        public async void SYSTEM_CODE_VO()
        {
            try
            {
                // dtl/i/sl
                // 수주 정보 가져오기
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("p4430/dtl/i/sl", new StringContent(JsonConvert.SerializeObject(new PurVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        IList<PurVo> SlRlseList = new List<PurVo>();
                        SlRlseList = JsonConvert.DeserializeObject<IEnumerable<PurVo>>(await response.Content.ReadAsStringAsync()).Cast<PurVo>().ToList();

                        SlRlseList.Insert(0, new PurVo() { SL_RLSE_NO = "수주없음" });
                        this.combo_SL_RLSE.ItemsSource = SlRlseList;
                        this.combo_SL_RLSE.SelectedItem = SlRlseList[0];
                    }
                }

                // 분류 리스트 가져오기
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "L-001"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        IList<SystemCodeVo> AreaList = new List<SystemCodeVo>();
                        AreaList = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();

                        this.combo_ITM_GRP_CLSS_CD.ItemsSource = AreaList;

                        if (AreaList.Count > 0)
                        {
                            this.combo_ITM_GRP_CLSS_CD.SelectedItem = AreaList[0];
                        }
                    }
                }

                //// 단가 object
                //using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("p4430/dtl/i/prc", new StringContent(JsonConvert.SerializeObject(new PurVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                //{
                //    if (response.IsSuccessStatusCode)
                //    {



                //    }
                //}
            }
            catch (Exception eLog)
            {
                MessageBox.Show(eLog.Message, _title, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

        }
        // 분류 조회 버튼을 클릭했을 때
        private async void btn_Items_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DXSplashScreen.Show<ProgressWindow>();

                SystemCodeVo vo = new SystemCodeVo();
                vo.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
                vo.DELT_FLG = "N";
                vo.ITM_GRP_CLSS_CD = (this.combo_ITM_GRP_CLSS_CD.SelectedItem as SystemCodeVo).CLSS_CD;


                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s141", new StringContent(JsonConvert.SerializeObject(vo), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        IList<SystemCodeVo> ItemList = new List<SystemCodeVo>();
                        ItemList = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();

                        IList<PurVo> PurvoChangeList = new List<PurVo>();

                        for (int x = 0; x < ItemList.Count; x++)
                        {
                            PurvoChangeList.Add(new PurVo()
                            {
                                PUR_ORD_NO = this.orgDao.PUR_ORD_NO
                              ,
                                ITM_CD = ItemList[x].ITM_CD
                              ,
                                ITM_NM = ItemList[x].ITM_NM
                              ,
                                PUR_QTY = 0.00
                              ,
                                UOM_NM = "R"
                              ,
                                UOM_CD = "001"
                              ,
                                N2ND_ITM_GRP_NM = ItemList[x].N2ND_ITM_GRP_NM
                              ,
                                CRE_USR_ID = SystemProperties.USER
                              ,
                                UPD_USR_ID = SystemProperties.USER
                              ,
                                CHNL_CD = SystemProperties.USER_VO.CHNL_CD
                            });
                        }

                        this.ViewGridDtl.ItemsSource = PurvoChangeList;
                        this.ViewGridDtl.SelectedItems = new List<PurVo>();
                        this.ViewGridDtl.RefreshData();
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
                MessageBox.Show(eLog.Message, _title, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }


        // 확인 버튼을 눌렀을 때 
        private async void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<PurVo> saveitems = new List<PurVo>();
                saveitems = ((List<PurVo>)this.ViewGridDtl.ItemsSource).FindAll(x => x.isCheckd == true);

                if (saveitems.Count > 0)
                {
                    int _Num = 0;
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("p4430/dtl/i", new StringContent(JsonConvert.SerializeObject(saveitems), System.Text.Encoding.UTF8, "application/json")))
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
                            WinUIMessageBox.Show("완료 되었습니다", this._title, MessageBoxButton.OK, MessageBoxImage.Information);
                            this.DialogResult = true;
                            this.Close();
                        }
                    }
                }
            }
            catch (Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + this._title, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }


        // 취소 버튼 눌렀을 때
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }


        //셀 값을 편집했을 때
        private void ViewTableDtl_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            if (e.Column.FieldName == "isCheckd")
            {
                TableView view = (TableView)sender;
                view.CloseEditor();
                view.FocusedRowHandle = GridControl.InvalidRowHandle;
                view.FocusedRowHandle = e.RowHandle;
            }
        }

        // 셀 편집기가 닫힌 후
        private void viewPage1EditView_HiddenEditor(object sender, EditorEventArgs e)
        {
            this.ViewTableDtl.CommitEditing();

            //bool impurqty = (e.Column.FieldName.ToString().Equals("PUR_QTY") ? true : false);
            //bool imbsswgt = (e.Column.FieldName.ToString().Equals("BSS_WGT") ? true : false);
            //bool n1stitmsznm = (e.Column.FieldName.ToString().Equals("N1ST_ITM_SZ_NM") ? true : false);
            //bool n2nditmsznm = (e.Column.FieldName.ToString().Equals("N2ND_ITM_SZ_NM") ? true : false);
            //bool imdcrt = (e.Column.FieldName.ToString().Equals("DC_RT") ? true : false);
            ////12-02 수정
            //bool imitmrmk = (e.Column.FieldName.ToString().Equals("PUR_ITM_RMK") ? true : false);
            int rowHandle = this.ViewTableDtl.FocusedRowHandle + 1;
            this.ViewGridDtl.RefreshRow(rowHandle - 1);
        }



        // 셀 값 입력
        private async void GridColumn_Validate(object sender, GridCellValidationEventArgs e)
        {
            try
            {

                PurVo masterDomain = (PurVo)ViewGridDtl.GetFocusedRow();

                bool impurqty = (e.Column.FieldName.ToString().Equals("PUR_QTY") ? true : false);
                bool imbsswgt = (e.Column.FieldName.ToString().Equals("BSS_WGT") ? true : false);
                bool n1stitmsznm = (e.Column.FieldName.ToString().Equals("N1ST_ITM_SZ_NM") ? true : false);
                bool n2nditmsznm = (e.Column.FieldName.ToString().Equals("N2ND_ITM_SZ_NM") ? true : false);
                bool imdcrt = (e.Column.FieldName.ToString().Equals("DC_RT") ? true : false);
                bool imutprc = (e.Column.FieldName.ToString().Equals("CO_UT_PRC") ? true : false);
                bool imitmrmk = (e.Column.FieldName.ToString().Equals("PUR_ITM_RMK") ? true : false);

                //비고
                if (imitmrmk)
                {
                    if (e.IsValid)
                    {
                        if (string.IsNullOrEmpty(masterDomain.PUR_ITM_RMK))
                        {
                            masterDomain.PUR_ITM_RMK = "";
                        }
                        if (!masterDomain.PUR_ITM_RMK.Equals((e.Value == null ? "" : e.Value.ToString())))
                        {
                            masterDomain.PUR_ITM_RMK = e.Value.ToString();
                            masterDomain.isCheckd = true;
                            this.OKButton.IsEnabled = true;
                        }
                    }
                }
                // 수량
                else if (impurqty)
                {
                    if (e.IsValid)
                    {   //빈 값이거나 없을 때 0
                        if (string.IsNullOrEmpty(masterDomain.PUR_QTY + ""))
                        {
                            masterDomain.PUR_QTY = 0;
                        }
                        if (!masterDomain.PUR_QTY.Equals((e.Value == null ? "" : e.Value.ToString())))
                        {
                            double? tmpInt = Convert.ToDouble(e.Value.ToString());
                            masterDomain.PUR_QTY = tmpInt;

                            try
                            {

                                masterDomain.TOT_QTY = (500 * tmpInt);
                            }
                            catch
                            {
                                masterDomain.TOT_QTY = 0;
                            }

                            masterDomain.isCheckd = true;
                            this.OKButton.IsEnabled = true;

                        }
                    }
                }
                //평량
                else if (imbsswgt)
                {
                    if (e.IsValid)
                    {
                        if (string.IsNullOrEmpty(masterDomain.BSS_WGT + ""))
                        {
                            masterDomain.BSS_WGT = 0;
                        }
                        if (!masterDomain.BSS_WGT.Equals((e.Value == null ? "" : e.Value.ToString())))
                        {
                            int? tmwgt = Convert.ToInt32(e.Value.ToString());

                            masterDomain.BSS_WGT = tmwgt;
                            masterDomain.isCheckd = true;
                        }
                        this.OKButton.IsEnabled = true;

                    }
                }
                //가로
                else if (n1stitmsznm)
                {
                    if (e.IsValid)
                    {
                        if (string.IsNullOrEmpty(masterDomain.N1ST_ITM_SZ_NM))
                        {
                            masterDomain.N1ST_ITM_SZ_NM = "";
                        }
                        if (!masterDomain.N1ST_ITM_SZ_NM.Equals((e.Value == null ? "" : e.Value.ToString())))
                        {
                            masterDomain.N1ST_ITM_SZ_NM = e.Value.ToString();
                            masterDomain.isCheckd = true;
                            this.OKButton.IsEnabled = true;
                        }
                    }
                    masterDomain.ITM_SZ_NM = masterDomain.N1ST_ITM_SZ_NM + '*' + masterDomain.N2ND_ITM_SZ_NM;
                }
                //세로
                else if (n2nditmsznm)
                {
                    if (e.IsValid)
                    {
                        if (string.IsNullOrEmpty(masterDomain.N2ND_ITM_SZ_NM))
                        {
                            masterDomain.N2ND_ITM_SZ_NM = "";
                        }

                        if (!masterDomain.N2ND_ITM_SZ_NM.Equals((e.Value == null ? "" : e.Value.ToString())))
                        {
                            masterDomain.N2ND_ITM_SZ_NM = e.Value.ToString();
                            masterDomain.isCheckd = true;
                            this.OKButton.IsEnabled = true;
                        }
                    }
                    masterDomain.ITM_SZ_NM = masterDomain.N1ST_ITM_SZ_NM + '*' + masterDomain.N2ND_ITM_SZ_NM;
                }
                //DC단가
                else if (imdcrt)
                {
                    if (string.IsNullOrEmpty(masterDomain.DC_RT + ""))
                    {
                        masterDomain.DC_RT = 0;
                    }

                    if (!masterDomain.DC_RT.Equals((e.Value == null ? "" : e.Value.ToString())))
                    {
                        int? tmdcrt = Convert.ToInt32(e.Value.ToString());

                        masterDomain.DC_RT = tmdcrt;
                        masterDomain.isCheckd = true;
                        this.OKButton.IsEnabled = true;
                    }
                }

                // 단가 계산
                if ((!string.IsNullOrEmpty(masterDomain.N1ST_ITM_SZ_NM)) && (!string.IsNullOrEmpty(masterDomain.N2ND_ITM_SZ_NM)) && (!string.IsNullOrEmpty(Convert.ToString(masterDomain.BSS_WGT))))
                {
                    try
                    {
                        if(masterDomain.N2ND_ITM_GRP_NM == null)
                        {
                            MessageBox.Show("원자재 [중분류]를 등록하세요.", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);

                            masterDomain.isCheckd = false;
                            masterDomain.BSS_WGT = null;
                            return;
                        }

                        PurVo vo = new PurVo();
                        vo.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
                        vo.BSS_WGT = masterDomain.BSS_WGT;
                        vo.N1ST_ITM_SZ_NM = masterDomain.N1ST_ITM_SZ_NM;
                        vo.N2ND_ITM_SZ_NM = masterDomain.N2ND_ITM_SZ_NM;
                        vo.N2ND_ITM_GRP_NM = masterDomain.N2ND_ITM_GRP_NM;

                        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("p4430/dtl/i/prc", new StringContent(JsonConvert.SerializeObject(vo), System.Text.Encoding.UTF8, "application/json")))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                PurVo coutprcVo = new PurVo();
                                coutprcVo = JsonConvert.DeserializeObject<PurVo>(await response.Content.ReadAsStringAsync());

                                if (coutprcVo == null)
                                {
                                    MessageBox.Show("평량당 가격을 원자재 원가표에 등록하세요.", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);

                                    masterDomain.isCheckd = false;
                                    masterDomain.BSS_WGT = null;
                                }
                                else
                                {
                                    masterDomain.CO_UT_PRC = JsonConvert.DeserializeObject<PurVo>(await response.Content.ReadAsStringAsync()).CO_UT_PRC;
                                    masterDomain.PUR_AMT = Convert.ToDouble(masterDomain.CO_UT_PRC) * ((100 - Convert.ToInt32(masterDomain.DC_RT)) * 0.01) * Convert.ToDouble(masterDomain.PUR_QTY);

                                }
                            }
                        }
                    }
                    catch (Exception eLog)
                    {
                        WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + this._title, MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }

                this.ViewGridDtl.RefreshData();


            }
            catch (Exception eLog)
            {
                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                e.ErrorContent = eLog.Message;
                e.SetError(e.ErrorContent, e.ErrorType);
                return;
            }
        }


    }
}
