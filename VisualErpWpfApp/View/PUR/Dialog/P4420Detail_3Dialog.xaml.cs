using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Code;
using ModelsLibrary.Pur;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;
using AquilaErpWpfApp3.Util;

namespace AquilaErpWpfApp3.View.PUR.Dialog
{
    public partial class P4420Detail_3Dialog : DXWindow
	{
		//private static PurServiceClient purClient = SystemProperties.PurClient;
		//private static ItemCodeServiceClient itemClient = SystemProperties.ItemClient;

        private string _title = "발주 등록 관리";

		public bool isEdit = false;
		private PurVo orgDao;
		//private PurVo updateDao;

		//private P4411DetailViewDialog itemDialog;

        public string[] aCoCdItems { get; set; }
        public string[] aCoNmItems { get; set; }


        private List<SystemCodeVo> SelectItems;

        public P4420Detail_3Dialog(PurVo Dao)
		{
			InitializeComponent();

            SYSTEM_CODE_VO();
            //this.combo_ITM_GRP_CLSS_CD.ItemsSource = SystemProperties.SYSTEM_CODE_VO("L-001");

            //this.combo_ITM_GRP_CLSS_CD.SelectedIndexChanged += combo_ITM_GRP_CLSS_CD_SelectedIndexChanged;
            //this.combo_N1ST_ITM_GRP_CD.SelectedIndexChanged += combo_N1ST_ITM_GRP_CD_SelectedIndexChanged;


            this.orgDao = Dao;
            //this.btn_ITEMS.Click += btn_ITEMS_Click;

                //if (string.IsNullOrEmpty(this.orgDao.GBN))  
                //{
                    //수정
                    //this.SelectItems = new ObservableCollection<PurVo>(purClient.P4411SelectDtlList(this.orgDao));
                   
                //}
                //else if (this.orgDao.GBN.Equals("Add"))
                //{
                //    //추가
                //    this.SelectItems = new ObservableCollection<PurVo>(purClient.P5511SelectDtlList(this.orgDao));

                    //if (this.SelectItems.Count <= 0)
                    //{
                        this.SelectItems = new List<SystemCodeVo>();
                    //}

      
                //}

                //this.ViewGridDtl.ItemsSource = this.SelectItems;
                //this.configCode.DataContext = copyDao;
                //this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
                this.OKButton.Click += new RoutedEventHandler(OKButton_Click);
                this.CancelButton.Click += new RoutedEventHandler(CancelButton_Click);

                //this.DELButton.Click += new RoutedEventHandler(DELButton_Click);

                //this.combo_ITM_GRP_CLSS_CD.Text = ((IList<CodeDao>)this.combo_ITM_GRP_CLSS_CD.ItemsSource)[0].CLSS_DESC;
                //this.ViewTableDtl.MouseDoubleClick += ViewTableDtl_MouseDoubleClick;

                //this.isEdit = IsPreview;
                //if (IsPreview)
                //{
                //    //this.SelectItems = new List<PurVo>(purClient.P4411SelectDtlList(this.orgDao));
                    
                //    //this.OKButton.IsEnabled = false;
                //    //this.DELButton.IsEnabled = false;
                //    this.btn_ITEMS.IsEnabled = false;
                //    this.combo_ITM_GRP_CLSS_CD.IsEnabled = false;
                //    //this.combo_N1ST_ITM_GRP_CD.IsEnabled = false;
                //    //this.combo_N2ND_ITM_GRP_CD.IsEnabled = false;

                //    //this.ViewTableDtl.NavigationStyle = DevExpress.Xpf.Grid.GridViewNavigationStyle.Row;

                //    this.ViewGridDtl.ItemsSource = this.SelectItems;
                //    this.ViewGridDtl.RefreshData();
                //}
                //else
                //{
                //    btn_ITEMS_Click(null, null);
                //}
		}

        //void ViewTableDtl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        //{
        //    PurVo tmpVo = (PurVo)this.ViewGridDtl.GetFocusedRow();
        //    if (tmpVo != null)
        //    {
        //        if (tmpVo.isCheckd)
        //        {
        //            tmpVo.isCheckd = false;
        //        }
        //        else
        //        {
        //            tmpVo.isCheckd = true;
        //            this.OKButton.IsEnabled = true;
        //        }
        //    }
        //}

        //void DELButton_Click(object sender, RoutedEventArgs e)
        //{
        //    PurVo tmpVo = (PurVo)this.ViewGridDtl.GetFocusedRow();
        //    if (tmpVo != null)
        //    {
        //        this.SelectItems.Remove(tmpVo);

        //        //this.ViewGridDtl.ItemsSource = this.SelectItems;
        //        this.ViewGridDtl.RefreshData();

        //    }
        //}

		async void btn_ITEMS_Click(object sender, RoutedEventArgs e)
		{
            try
            {
       //         SystemCodeVo dao = new SystemCodeVo();

       //         SystemCodeVo ITM_GRP_CLSS_CDVo = this.combo_ITM_GRP_CLSS_CD.SelectedItem as SystemCodeVo;
			    //if (ITM_GRP_CLSS_CDVo != null)
			    //{
				   // dao.ITM_GRP_CLSS_CD = ITM_GRP_CLSS_CDVo.CLSS_CD;
				   // dao.ITM_GRP_CLSS_NM = ITM_GRP_CLSS_CDVo.CLSS_DESC;
       //             dao.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
       //         }
                //
                //CodeDao N1ST_ITM_GRP_CDVo = this.combo_N1ST_ITM_GRP_CD.SelectedItem as CodeDao;
                //if (N1ST_ITM_GRP_CDVo != null)
                //{
                // dao.N1ST_ITM_GRP_CD = N1ST_ITM_GRP_CDVo.CLSS_CD;
                // dao.N1ST_ITM_GRP_NM = N1ST_ITM_GRP_CDVo.CLSS_DESC;
                //}
                //         //
                //         CodeDao N2ND_ITM_GRP_NMVo = this.combo_N2ND_ITM_GRP_CD.SelectedItem as CodeDao;
                //         if (N2ND_ITM_GRP_NMVo != null)
                //         {
                //             dao.N2ND_ITM_GRP_CD = N2ND_ITM_GRP_NMVo.CLSS_CD;
                //             dao.N2ND_ITM_GRP_NM = N2ND_ITM_GRP_NMVo.CLSS_DESC;
                //         }

                //IList<ItemCodeVo> resultItems = (IList<ItemCodeVo>)itemClient.SelectItemList2(dao);

                //using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s141", new StringContent(JsonConvert.SerializeObject(dao), System.Text.Encoding.UTF8, "application/json")))
                //{
                //    if (response.IsSuccessStatusCode)
                //    {
                //        IList<SystemCodeVo> resultItems = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();

                //        this.SelectItems.Clear();
                //        for (int x = 0; x < resultItems.Count; x++)
                //        {
                //             this.SelectItems.Add(new PurVo() {
                //                              PUR_ORD_NO = this.orgDao.PUR_ORD_NO
                //                            , ITM_CD = resultItems[x].ITM_CD
                //                            , ITM_NM = resultItems[x].ITM_NM
                //                            , ITM_SZ_NM = resultItems[x].ITM_SZ_NM
                //                            , UOM_NM = resultItems[x].UOM_NM
                //                            , UOM_CD = resultItems[x].UOM_CD 
                //                            , CRE_USR_ID = SystemProperties.USER
                //                            , UPD_USR_ID = SystemProperties.USER
                //                            , CHNL_CD = SystemProperties.USER_VO.CHNL_CD
                //                        });
                //        }
                //        this.ViewGridDtl.ItemsSource = this.SelectItems;
                //        this.ViewGridDtl.RefreshData();
                //    }
                //}

            }
            catch (Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + this._title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            //DXSplashScreen.Close();
            //itemDialog = new P4411DetailViewDialog(dao);
            //itemDialog.Title = "자재 명 조회";
            //itemDialog.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            ////masterDialog.Owner = Application.Current.MainWindow;
            //itemDialog.BorderEffect = BorderEffect.Default;
            ////itemDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            ////itemDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            //bool isDialog = (bool)itemDialog.ShowDialog();
            //if (isDialog)
            //{
            //    IList<ItemCodeVo> resultItems = itemDialog.SelectItems;
            //    if (resultItems != null)
            //    {
            //        for (int x = 0; x < resultItems.Count; x++)
            //        {
            //            this.SelectItems.Add(new PurVo() {
            //                      PUR_ORD_NO = this.orgDao.PUR_ORD_NO
            //                    , ITM_CD = resultItems[x].ITM_CD
            //                    , ITM_NM = resultItems[x].ITM_NM
            //                    , ITM_SZ_NM = resultItems[x].ITM_SZ_NM
            //                    , UOM_NM = resultItems[x].UOM_NM
            //                    , UOM_CD = resultItems[x].UOM_CD 
            //                    , CRE_USR_ID = SystemProperties.USER
            //                    , UPD_USR_ID = SystemProperties.USER
            //                        });
            //        }
            //        this.ViewGridDtl.ItemsSource = this.SelectItems;
            //        this.ViewGridDtl.RefreshData();
            //    }
            //}
		}

		private async void OKButton_Click(object sender, RoutedEventArgs e)
		{
            try
            {
                List<SystemCodeVo> saveItems = (List<SystemCodeVo>)this.ViewGridDtl.ItemsSource;
                if (saveItems.FindAll(x => x.isCheckd == true).Count < 0)
                {
                    WinUIMessageBox.Show("매입처를 선택해 주세요", this._title, MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                this.aCoCdItems = (saveItems.FindAll(x => x.isCheckd == true)).ToArray().Select(s => s.CO_NO).ToArray();
                this.aCoNmItems = (saveItems.FindAll(x => x.isCheckd == true)).ToArray().Select(s => s.CO_NM).ToArray();

                //if (saveItems.FindAll(x => x.isCheckd == true).Count > 0)
                //{
                //    int _Num = 0;
                //    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("p4411/dtl/i", new StringContent(JsonConvert.SerializeObject((saveItems.FindAll(x => x.isCheckd == true)).ToArray()), System.Text.Encoding.UTF8, "application/json")))
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
                //            WinUIMessageBox.Show("완료 되었습니다", this._title, MessageBoxButton.OK, MessageBoxImage.Information);
                //            this.DialogResult = true;
                //            this.Close();
                //        }
                //    }
                //}


                ////삭제
                ////PurVo resultVo = purClient.P4411DeleteDtl(new PurVo() { PUR_ORD_NO = orgDao.PUR_ORD_NO });
                ////if (!resultVo.isSuccess)
                ////{
                ////    //실패
                ////    WinUIMessageBox.Show(resultVo.Message, "[에러]" + this._title, MessageBoxButton.OK, MessageBoxImage.Error);
                ////    return;
                ////}
                //// 전체 저장
                //List<PurVo> saveItems = (List<PurVo>)this.ViewGridDtl.ItemsSource;
                //if (this.isEdit)
                //{
                //    PurVo resultVo = purClient.P4411UpdateDtl_Transaction((saveItems.FindAll(x => x.isCheckd == true)).ToArray());
                //    if (!resultVo.isSuccess)
                //    {
                //        //실패
                //        WinUIMessageBox.Show(resultVo.Message, "[에러]" + this._title, MessageBoxButton.OK, MessageBoxImage.Error);
                //        return;
                //    }
                //    //성공
                //    WinUIMessageBox.Show("완료 되었습니다", "[수정]" + this._title, MessageBoxButton.OK, MessageBoxImage.Information);
                //}
                //else
                //{ 
                //    //if (saveItems.FindAll(x => x.isCheckd == true).Count > 0)
                //    //{
                //        PurVo resultVo = purClient.P4411InsertDtl_Transaction((saveItems.FindAll(x => x.isCheckd == true)).ToArray());
                //        if (!resultVo.isSuccess)
                //        {
                //            //실패
                //            WinUIMessageBox.Show(resultVo.Message, "[에러]" + this._title, MessageBoxButton.OK, MessageBoxImage.Error);
                //            return;
                //        }
                //        //성공
                //        WinUIMessageBox.Show("완료 되었습니다", "[추가]" + this._title, MessageBoxButton.OK, MessageBoxImage.Information);
                //    //}
                //}

                this.DialogResult = true;
                this.Close();
            }
            catch (Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + this._title, MessageBoxButton.OK, MessageBoxImage.Error);
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

		//void combo_ITM_GRP_CLSS_CD_SelectedIndexChanged(object sender, RoutedEventArgs e)
		//{
		//	try
		//	{
		//		CodeDao ITM_GRP_CLSS_NM_VO = this.combo_ITM_GRP_CLSS_CD.SelectedItem as CodeDao;
		//		if (ITM_GRP_CLSS_NM_VO != null)
		//		{
		//			if (string.IsNullOrEmpty(ITM_GRP_CLSS_NM_VO.CLSS_CD))
		//			{
		//				return;
		//			}

		//			IList<ItemGroupCodeVo> ItemGroupVo = itemClient.SelectCodeItemGroupList(new ItemGroupCodeVo() { DELT_FLG = "N", ITM_GRP_CLSS_CD = ITM_GRP_CLSS_NM_VO.CLSS_CD, CRE_USR_ID = "" });
		//			IList<CodeDao> ItemList = new List<CodeDao>();
		//			int nCnt = ItemGroupVo.Count;
		//			ItemGroupCodeVo tmpVo;

		//			this.combo_N1ST_ITM_GRP_CD.Clear();

		//			for (int x = 0; x < nCnt; x++)
		//			{
		//				tmpVo = ItemGroupVo[x];
		//				ItemList.Add(new CodeDao() { CLSS_DESC = tmpVo.ITM_GRP_NM, CLSS_CD = tmpVo.ITM_GRP_CD });
		//			}
		//			this.combo_N1ST_ITM_GRP_CD.ItemsSource = ItemList;

  //                  if (ItemList.Count > 0)
  //                  {
  //                      this.combo_N1ST_ITM_GRP_CD.Text = ItemList[0].CLSS_DESC;
  //                  }
		//		}

		//	}
		//	catch (Exception eLog)
		//	{
		//		WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + this._title, MessageBoxButton.OK, MessageBoxImage.Error);
		//		return;
		//	}
		//}

        //void combo_N1ST_ITM_GRP_CD_SelectedIndexChanged(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        CodeDao ITM_GRP_CLSS_NM_VO = this.combo_ITM_GRP_CLSS_CD.SelectedItem as CodeDao;
        //        if (ITM_GRP_CLSS_NM_VO != null)
        //        {
        //            CodeDao ITM_N1ST_TP_NM_VO = this.combo_N1ST_ITM_GRP_CD.SelectedItem as CodeDao;
        //            if (ITM_N1ST_TP_NM_VO != null)
        //            {
        //                if (string.IsNullOrEmpty(ITM_N1ST_TP_NM_VO.CLSS_CD))
        //                {
        //                    return;
        //                }

        //                IList<ItemGroupCodeVo> ItemGroupVo = itemClient.SelectCodeItemGroupList(new ItemGroupCodeVo() { PRNT_ITM_GRP_CD = "X", N1ST_ITM_GRP_CD = ITM_N1ST_TP_NM_VO.CLSS_CD, DELT_FLG = "N", ITM_GRP_CLSS_CD = ITM_GRP_CLSS_NM_VO.CLSS_CD, CRE_USR_ID = "" });
        //                IList<CodeDao> ItemList = new List<CodeDao>();
        //                int nCnt = ItemGroupVo.Count;
        //                ItemGroupCodeVo tmpVo;

        //                this.combo_N2ND_ITM_GRP_CD.Clear();
        //                for (int x = 0; x < nCnt; x++)
        //                {
        //                    tmpVo = ItemGroupVo[x];
        //                    ItemList.Add(new CodeDao() { CLSS_DESC = tmpVo.ITM_GRP_NM, CLSS_CD = tmpVo.ITM_GRP_CD });
        //                }
        //                this.combo_N2ND_ITM_GRP_CD.ItemsSource = ItemList;
        //                this.combo_N2ND_ITM_GRP_CD.SelectedIndex = 0;
        //            }
        //        }
        //    }
        //    catch (Exception eLog)
        //    {
        //        WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error);
        //        return;
        //    }
        //}

        private void GridColumn_Validate(object sender, DevExpress.Xpf.Grid.GridCellValidationEventArgs e)
        {
            try
            {
                PurVo masterDomain = (PurVo)ViewGridDtl.GetFocusedRow();
                bool impItmQty = (e.Column.FieldName.ToString().Equals("PUR_QTY") ? true : false);
                bool impItmPrc = (e.Column.FieldName.ToString().Equals("CO_UT_PRC") ? true : false);
                //bool impItmAmt = (e.Column.FieldName.ToString().Equals("IMP_ITM_AMT") ? true : false);
                bool impItmRmk = (e.Column.FieldName.ToString().Equals("PUR_ITM_RMK") ? true : false);


                if (impItmQty)
                {
                    if (e.IsValid)
                    {
                        if (string.IsNullOrEmpty(masterDomain.PUR_QTY + ""))
                        {
                            masterDomain.PUR_QTY = 0;
                        }
                        //
                        if (!masterDomain.PUR_QTY.Equals((e.Value == null ? "" : e.Value.ToString())))
                        {
                            int? tmpInt = Convert.ToInt32(e.Value.ToString());
                            //if (tmpInt <= -1)
                            //{
                            //    e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                            //    e.ErrorContent = "[수량]" + tmpInt + " -값은 입력 하실수 없습니다";
                            //    e.SetError(e.ErrorContent, e.ErrorType);
                            //    return;
                            //}

                            masterDomain.PUR_QTY = tmpInt;
                            masterDomain.isCheckd = true;

                            //에러 체크
                            try
                            {
                                masterDomain.PUR_AMT = tmpInt * Convert.ToDouble(masterDomain.CO_UT_PRC);
                            }
                            catch (Exception)
                            {
                                masterDomain.PUR_AMT = 0;
                            }

                            this.OKButton.IsEnabled = true;
                        }
                    }
                }
                else if (impItmPrc)
                {
                    if (e.IsValid)
                    {
                        if (string.IsNullOrEmpty(masterDomain.CO_UT_PRC + ""))
                        {
                            masterDomain.CO_UT_PRC = 0;
                        }
                        //
                        if (!masterDomain.CO_UT_PRC.Equals((e.Value == null ? "" : e.Value.ToString())))
                        {
                            double? tmpInt = Convert.ToDouble(e.Value.ToString());
                            //if (tmpInt <= -1)
                            //{
                            //    e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                            //    e.ErrorContent = "[단가]" + tmpInt + " -값은 입력 하실수 없습니다";
                            //    e.SetError(e.ErrorContent, e.ErrorType);
                            //    return;
                            //}

                            masterDomain.CO_UT_PRC = tmpInt;
                            masterDomain.isCheckd = true;
                            //에러 체크
                            try
                            {
                                masterDomain.PUR_AMT = Convert.ToInt32(masterDomain.PUR_QTY) * tmpInt;
                            }
                            catch (Exception)
                            {
                                masterDomain.PUR_AMT = 0;
                            }
                            this.OKButton.IsEnabled = true;
                        }
                    }
                }
                else if (impItmRmk)
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
            }
            catch (Exception eLog)
            {
                e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
                e.ErrorContent = eLog.Message;
                e.SetError(e.ErrorContent, e.ErrorType);
                return;
            }
        }



        private void viewPage1EditView_HiddenEditor(object sender, DevExpress.Xpf.Grid.EditorEventArgs e)
        {
            this.ViewTableDtl.CommitEditing();

            bool impItmQty = (e.Column.FieldName.ToString().Equals("PUR_QTY") ? true : false);
            bool impItmPrc = (e.Column.FieldName.ToString().Equals("CO_UT_PRC") ? true : false);
            //bool impItmAmt = (e.Column.FieldName.ToString().Equals("IMP_ITM_AMT") ? true : false);
            bool impItmRmk = (e.Column.FieldName.ToString().Equals("PUR_ITM_RMK") ? true : false);

            int rowHandle = this.ViewTableDtl.FocusedRowHandle + 1;

            //if (impItmQty)
            //{
            //    this.ViewGridDtl.CurrentColumn = this.ViewGridDtl.Columns["PUR_QTY"];
            //}
            //else if (impItmPrc)
            //{
            //    this.ViewGridDtl.CurrentColumn = this.ViewGridDtl.Columns["CO_UT_PRC"];
            //}
            //else if (impItmRmk)
            //{
            //    this.ViewGridDtl.CurrentColumn = this.ViewGridDtl.Columns["PUR_ITM_RMK"];
            //}

            this.ViewGridDtl.RefreshRow(rowHandle - 1);
            //this.ViewTableDtl.FocusedRowHandle = rowHandle;
        }


        private void CheckEdit_Checked(object sender, RoutedEventArgs e)
        {
            CheckEdit checkEdit = sender as CheckEdit;
            PurVo tmpImsi;
            for (int x = 0; x < this.ViewGridDtl.VisibleRowCount; x++)
            {
                int rowHandle = this.ViewGridDtl.GetRowHandleByVisibleIndex(x);
                if (rowHandle > -1)
                {
                    tmpImsi = this.ViewGridDtl.GetRow(rowHandle) as PurVo;
                    if (checkEdit.IsChecked == true)
                    {
                        tmpImsi.isCheckd = true;
                        //
                        this.OKButton.IsEnabled = true;
                    }
                    else
                    {
                        tmpImsi.isCheckd = false;
                    }

                }
            }
        }

        private void view_CellValueChanging(object sender, CellValueChangedEventArgs e)
        {
            if (e.Column.FieldName == "isCheckd")
            {
                TableView view = (TableView)sender;
                view.CloseEditor();
                view.FocusedRowHandle = GridControl.InvalidRowHandle;
                view.FocusedRowHandle = e.RowHandle;
            }
        }

        public async void SYSTEM_CODE_VO()
        {

            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s143", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { DELT_FLG = "N", SEEK_AP = "AP", SEEK = "AP", CO_TP_CD = "AP", AREA_CD = SystemProperties.USER_VO.EMPE_PLC_NM, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.ViewGridDtl.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                }
            }

            //this.combo_ITM_GRP_CLSS_CD.ItemsSource = SystemProperties.SYSTEM_CODE_VO("L-001");
            //using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "L-001"))
            //{
            //    if (response.IsSuccessStatusCode)
            //    {
            //        this.combo_ITM_GRP_CLSS_CD.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
            //        this.combo_ITM_GRP_CLSS_CD.SelectedIndex = 0;
            //    }
            //}
            //btn_ITEMS_Click(null, null);
        }

    }
}
