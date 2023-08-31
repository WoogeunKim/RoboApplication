using AquilaErpWpfApp3.Util;
using AquilaErpWpfApp3.View.SAL.Dialog;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Sale;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Media;

namespace AquilaErpWpfApp3.ViewModel
{
    public sealed class S22227ViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private string title = "GR번호/LOC관리";

        private S22227LocDialog locDialog;

        private IList<SaleVo> selectedMstList = new List<SaleVo>();

        private IList<SaleVo> selectedDtlList = new List<SaleVo>();
        private IList<SaleVo> selectedDtlItemsList = new List<SaleVo>();

        private IList<SaleVo> selectedCoMstList = new List<SaleVo>();
        private IList<SaleVo> selectedGRList = new List<SaleVo>();

        public S22227ViewModel()
        {
            StartInReqDt = System.DateTime.Now;
            EndInReqDt = System.DateTime.Now;
            //SYSTEM_CODE_VO();
        }



        [Command]
        public async void Refresh()
        {
            try
            {
                SelectedDtlItem = null;
                SelectDtlList = null;
                SelectGRList = null;

                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s22227/mst/co", new StringContent(JsonConvert.SerializeObject(new SaleVo() { FM_DT = (StartInReqDt).ToString("yyyy-MM-dd"), TO_DT = (EndInReqDt).ToString("yyyy-MM-dd"), CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectCoMstList = JsonConvert.DeserializeObject<IEnumerable<SaleVo>>(await response.Content.ReadAsStringAsync()).Cast<SaleVo>().ToList();


                        Title = "[납품요청일]" + (StartInReqDt).ToString("yyyy-MM-dd") + "~" + (EndInReqDt).ToString("yyyy-MM-dd");

                        if (SelectCoMstList.Count < 1)
                        {
                            SelectedDtlItem = null;
                            SelectDtlList = null;
                            SelectGRList = null;
                        }
                    }
                }
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }

        [Command]
        public async void SelectMstDetail()
        {
            try
            {
                if (SelectedCoMstItem == null)
                {
                    return;
                }
                else if (selectedCoMstList.Count <= 0)
                {
                    return;
                }

                SelectDtlList = null;
                isN__GR_UPDATE = false;
                isO_GR_UPDATE = false;

                SelectedCoMstItem.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("S22227/dtl/right/top", new StringContent(JsonConvert.SerializeObject(SelectedCoMstItem), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectMstList = JsonConvert.DeserializeObject<IEnumerable<SaleVo>>(await response.Content.ReadAsStringAsync()).Cast<SaleVo>().ToList();
                    }
                }

                SelectGrListDetail();
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }

        public async void SelectGrListDetail(string _RLSE_CMD_NO = null, string _LOC_CD = null)
        {

            try
            {
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("S22227/dtl/left/bottom", new StringContent(JsonConvert.SerializeObject(SelectedCoMstItem), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectGRList = JsonConvert.DeserializeObject<IEnumerable<SaleVo>>(await response.Content.ReadAsStringAsync()).Cast<SaleVo>().ToList();

                        if (SelectGRList.Count >= 1)
                        {
                            if (!string.IsNullOrEmpty(_RLSE_CMD_NO))
                            {
                                this.SelectedGRItem = this.SelectGRList.Where(x => x.RLSE_CMD_NO .Equals(_RLSE_CMD_NO)).FirstOrDefault<SaleVo>();
                            }

                            if (!string.IsNullOrEmpty(_RLSE_CMD_NO) && !string.IsNullOrEmpty(_LOC_CD))
                            {
                                this.SelectedGRItem = this.SelectGRList.Where(x => (x.RLSE_CMD_NO + "_" + x.LOC_CD).Equals(_RLSE_CMD_NO + "_" + _LOC_CD)).FirstOrDefault<SaleVo>();
                            }
                        }
                    }
                }

                if (this.SelectGRList.Count >= 1)
                {
                    isLoc_DELETE = true;

                    if (string.IsNullOrEmpty(_RLSE_CMD_NO))
                    {
                        this.SelectedGRItem = this.SelectGRList[SelectGRList.Count - 1];
                    }

                    if (this.SelectedGRItem != null)
                    {
                        isLoc_UPDATE = true;
                        isGr_DELETE = true;
                    }
                }
                else
                {
                    isLoc_UPDATE = false;
                    isLoc_DELETE = false;
                    isGr_DELETE = false;
                }


                GrList_Activity();
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }


        public void Mst_ValueCheck()
        {
            //유의성 체크
            IList<SaleVo> MstVo = this.SelectMstList.Where<SaleVo>(w => w.isCheckd == true).ToList();

            for (int i = 0; i < MstVo.Count - 1; i++)
            {
                for (int j = i + 1; j < MstVo.Count; j++)
                {
                    if (MstVo[i].CNTR_NM != MstVo[j].CNTR_NM)
                    {
                        WinUIMessageBox.Show("선택한 항목들의 공사명이 일치하지 않습니다.", "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                        SelectedMstItem.isCheckd = false;
                        return;
                    }

                    if (MstVo[i].IN_REQ_DT != MstVo[j].IN_REQ_DT)
                    {
                        WinUIMessageBox.Show("선택한 항목들의 납품 요청일자가 일치하지 않습니다.", "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                        SelectedMstItem.isCheckd = false;
                        return;
                    }
                }
            }
        }


        #region mst,dtl 체크관리
        [Command]
        public async void SelectMasterCheckd()
        {
            try
            {
                if (SelectedMstItem == null)
                {
                    return;
                }

                //
                if (SelectedMstItem.isCheckd)
                {
                    SelectedMstItem.isCheckd = false;
                }
                else
                {
                    SelectedMstItem.isCheckd = true;
                }

                ////유의성 체크
                Mst_ValueCheck();

                //공사별 리스트 다시 조회
                SelectDetailRefresh();
            }
            catch(System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
            }
        }

        [Command]
        public async void SelectDetailCheckd()
        {

            if (SelectedDtlItem == null)
            {
                return;
            }

            //
            if (SelectedDtlItem.isCheckd)
            {
                SelectedDtlItem.isCheckd = false;
            }
            else
            {
                SelectedDtlItem.isCheckd = true;
            }

            DtlList_Activity();
        }
        #endregion

        [Command]
        public async void SelectDetailRefresh(string _RLSE_CMD_NO = null)
        {
            try
            {
                if (this.SelectMstList.Any<SaleVo>(x => x.isCheckd == true))
                {
                    SelectedMstItem.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
                    SelectedMstItem.A_PUR_NO = this.SelectMstList.Where<SaleVo>(w => w.isCheckd == true).Select(x => (x.PUR_NO + "_" + x.CNTR_NM + "_" + x.CNTR_PSN_NM)).ToArray<string>();
                    SelectedMstItem.OK_FLG = "N";
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("S22227/dtl/right/bottom", new StringContent(JsonConvert.SerializeObject(SelectedMstItem), System.Text.Encoding.UTF8, "application/json")))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            this.SelectDtlList = JsonConvert.DeserializeObject<IEnumerable<SaleVo>>(await response.Content.ReadAsStringAsync()).Cast<SaleVo>().ToList();

                            if (SelectDtlList.Count >= 1)
                            {
                                //if (!string.IsNullOrEmpty(_RLSE_CMD_NO))
                                //{
                                //    this.SelectedDtlItem = this.selectedDtlList.Where(x => x.RLSE_CMD_NO.Equals(_RLSE_CMD_NO)).FirstOrDefault<SaleVo>();
                                //}
                            }
                            else
                            {
                            }
                        }
                    }
                    DtlList_Activity();

                }
                else
                {
                    this.SelectDtlList = null;
                    this.SelectedDtlItem = null;
                    return;
                }
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }

        [Command] //GR리스트 더블클릭 조회
        public async void GRListDoubleClick(string _RLSE_CMD_NO = null)
        {
            try
            {
                if (!string.IsNullOrEmpty(_RLSE_CMD_NO))
                {
                    SelectedGRItem.RLSE_CMD_NO = _RLSE_CMD_NO;
                }

                SelectedGRItem.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
                SelectedGRItem.OK_FLG = "Y";
                SelectedGRItem.SL_CO_GRD_NM = SelectedGRItem.RLSE_CMD_NO + "_" + SelectedGRItem.LOC_CD;

                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("S22227/dtl/grlist", new StringContent(JsonConvert.SerializeObject(SelectedGRItem), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectDtlList = JsonConvert.DeserializeObject<IEnumerable<SaleVo>>(await response.Content.ReadAsStringAsync()).Cast<SaleVo>().ToList();            
                    }

                }

                if (this.SelectDtlList.Count >= 1)
                {
                    isN__GR_UPDATE = false;
                    isO_GR_UPDATE = true;
                }
                else
                {
                    isO_GR_UPDATE = false;
                }

                GrList_Activity();
            }
            catch(System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
            }
        }


        // GR번호 신규부여 
        [Command]
        public async void NewApply()
        {
            try
            {
                IList<SaleVo> NewGrVoList = new List<SaleVo>();
                NewGrVoList = this.SelectDtlList.Where<SaleVo>(w => w.isCheckd == true).ToList<SaleVo>();

                //정합성 체크
                if (NewGrVoList.Count < 1) return;
                if (NewGrVoList[0].RLSE_CMD_NO != null)
                {
                    isN__GR_UPDATE = false;
                    return;
                }

                double _sum_itm_wgt = 0;

                for (int i = 0; i < NewGrVoList.Count; i++)
                {
                    //NewGrVoList[i].RLSE_CMD_NO = _NEW_GR_NO.Replace("\"" , "");
                    NewGrVoList[i].CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
                    _sum_itm_wgt += double.Parse(NewGrVoList[i].SL_ITM_WGT.ToString());
                }

                MessageBoxResult result = WinUIMessageBox.Show("신규 GR번호를 부여하시겠습니까?" + " 선택된 중량(kg) : " + _sum_itm_wgt, title, MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("S22227/dtl/gr/new/u", new StringContent(JsonConvert.SerializeObject(NewGrVoList), System.Text.Encoding.UTF8, "application/json")))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            int _Num = 0;
                            string resultMsg = await response.Content.ReadAsStringAsync();
                            if (int.TryParse(resultMsg, out _Num) == false)
                            {
                                //실패
                                WinUIMessageBox.Show(resultMsg, title, MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }

                            //성공
                            WinUIMessageBox.Show("신규 GR번호가 부여되었습니다.", title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);

                            //SelectGrListDetail(NewGrVoList[0].RLSE_CMD_NO);
                            SelectGrListDetail();
                            SelectDetailRefresh();
                        }
                    }
                }
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }

        // 기존 GR번호 부여 
        [Command]
        public async void OriginApply()
        {
            try
            {
                IList<SaleVo> OriginGrVoList = new List<SaleVo>();
                OriginGrVoList = this.SelectDtlList.Where<SaleVo>(w => w.isCheckd == true).ToList<SaleVo>();
                if (OriginGrVoList.Count < 1) return;
                if (SelectedGRItem.RLSE_CMD_NO == null) return;

                double _sum_itm_wgt = 0;

                for (int i = 0; i < OriginGrVoList.Count; i++)
                {
                    OriginGrVoList[i].RLSE_CMD_NO = SelectedGRItem.RLSE_CMD_NO;
                    OriginGrVoList[i].LOC_CD = SelectedGRItem.LOC_CD;
                    OriginGrVoList[i].CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
                    _sum_itm_wgt += double.Parse(OriginGrVoList[i].SL_ITM_WGT.ToString());
                }

                MessageBoxResult result = WinUIMessageBox.Show(SelectedGRItem.RLSE_CMD_NO + "를 부여하시겠습니까?" + " 선택된 중량(kg) : " + _sum_itm_wgt, title, MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("S22227/dtl/gr/orgin/u", new StringContent(JsonConvert.SerializeObject(OriginGrVoList), System.Text.Encoding.UTF8, "application/json")))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            int _Num = 0;
                            string resultMsg = await response.Content.ReadAsStringAsync();
                            if (int.TryParse(resultMsg, out _Num) == false)
                            {
                                //실패
                                WinUIMessageBox.Show(resultMsg, title, MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                            //성공
                            WinUIMessageBox.Show("기존 GR번호가 부여되었습니다.", title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);

                            SelectGrListDetail(SelectedGRItem.RLSE_CMD_NO);
                            SelectDtlList = null;
                        }
                    }
                }
                else
                {
                    SelectDetailRefresh();
                }
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }


        // LOC번호 관리 
        [Command]
        public async void LocApply()
        {
            SelectedGRItem.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
            locDialog = new S22227LocDialog(SelectedGRItem);
            locDialog.Title = title + " - 추가";
            locDialog.Owner = Application.Current.MainWindow;
            locDialog.BorderEffect = BorderEffect.Default;
            locDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            locDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            bool isDialog = (bool)locDialog.ShowDialog();
            if (isDialog)
            {
                SelectGrListDetail(locDialog.updateDao.RLSE_CMD_NO, locDialog.updateDao.LOC_CD);
            }
        }

        // GR번호 삭제 
        [Command]
        public async void GrDelete()
        {
            try
            {
                if (this.SelectedGRItem.RLSE_CMD_NO == null) return;

                IList<SaleVo> delVoList = new List<SaleVo>();
                delVoList = this.SelectDtlList.Where<SaleVo>(x => x.isCheckd == true).ToList();

                if (delVoList == null) return;
                if (delVoList.Count < 1) return;

                MessageBoxResult result = WinUIMessageBox.Show(delVoList.Count + "건을 삭제하겠습니까?", title, MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    for(int i=0; i < delVoList.Count; i++)
                    {
                        delVoList[i].CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
                    }

                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("S22227/dtl/gr/d", new StringContent(JsonConvert.SerializeObject(delVoList), System.Text.Encoding.UTF8, "application/json")))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            int _Num = 0;
                            string resultMsg = await response.Content.ReadAsStringAsync();
                            if (int.TryParse(resultMsg, out _Num) == false)
                            {
                                //실패
                                WinUIMessageBox.Show(resultMsg, title, MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                            //성공
                            SelectGrListDetail(delVoList[0].RLSE_CMD_NO, delVoList[0].LOC_CD) ;
                            
                            
                            this.SelectDtlList = null;
                            WinUIMessageBox.Show(delVoList.Count + "건이 삭제되었습니다.", title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                        }
                    }
                }
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }

        // Loc번호 삭제 
        [Command]
        public async void LocDelete()
        {
            try
            {
                if (this.SelectedGRItem.RLSE_CMD_NO == null) return;
                if (this.SelectedGRItem.LOC_CD == null) return;

                SelectedGRItem.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
                MessageBoxResult result = WinUIMessageBox.Show(this.SelectedGRItem.RLSE_CMD_NO + "의 Loc번호를 삭제하겠습니까?", title, MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("S22227/dtl/loc/d", new StringContent(JsonConvert.SerializeObject(SelectedGRItem), System.Text.Encoding.UTF8, "application/json")))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            int _Num = 0;
                            string resultMsg = await response.Content.ReadAsStringAsync();
                            if (int.TryParse(resultMsg, out _Num) == false)
                            {
                                if (DXSplashScreen.IsActive == true)
                                {
                                    DXSplashScreen.Close();
                                }
                                //실패
                                WinUIMessageBox.Show(resultMsg, title, MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                            //성공
                            SelectGrListDetail();
                            WinUIMessageBox.Show("삭제되었습니다.", title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                        }
                    }
                }
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }
        
        public void GrList_Activity()
        {
            if (this.SelectGRList.Count >= 1)
            {

                if (this.SelectedGRItem.LOC_CD != null)
                {
                    isLoc_DELETE = true;
                }

                if (this.SelectedGRItem != null)
                {
                    isLoc_UPDATE = true;
                    isGr_DELETE = true;
                }
            }
            else
            {
                isLoc_UPDATE = false;
                isLoc_DELETE = false;
                isGr_DELETE = false;
            }
        }

        public void DtlList_Activity()
        {
            if (this.SelectDtlList.Count >= 1)
            {
                if (this.SelectDtlList.Where<SaleVo>(w => w.isCheckd == true).ToList<SaleVo>().Count >= 1)
                {
                    isN__GR_UPDATE = true;
                    isO_GR_UPDATE = true;
                }

                if (SelectDtlList[0].RLSE_CMD_NO != null)
                {
                    isN__GR_UPDATE = false;
                }
            }
            else
            {
                isN__GR_UPDATE = false;
                isO_GR_UPDATE = false;
            }
        }


        //Binding
        #region binding
        public IList<SaleVo> SelectDtlList
        {
            get { return selectedDtlList; }
            set { SetProperty(ref selectedDtlList, value, () => SelectDtlList); }
        }


        SaleVo _selectedDtlItem;
        public SaleVo SelectedDtlItem
        {
            get
            {
                return _selectedDtlItem;
            }
            set
            {
                SetProperty(ref _selectedDtlItem, value, () => SelectedDtlItem);
            }
        }


        public IList<SaleVo> SelectedDtlItems
        {
            get { return selectedDtlItemsList; }
            set { SetProperty(ref selectedDtlItemsList, value, () => SelectedDtlItems); }
        }


        string _Title = string.Empty;
        public string Title
        {
            get { return _Title; }
            set { SetProperty(ref _Title, value, () => Title); }
        }

        //납품 요청일
        DateTime _startInReqDt;
        public DateTime StartInReqDt
        {
            get { return _startInReqDt; }
            set { SetProperty(ref _startInReqDt, value, () => StartInReqDt); }
        }

        DateTime _endInReqDt;
        public DateTime EndInReqDt
        {
            get { return _endInReqDt; }
            set { SetProperty(ref _endInReqDt, value, () => EndInReqDt); }
        }

        private string _M_SEARCH_TEXT = string.Empty;
        public string M_SEARCH_TEXT
        {
            get { return _M_SEARCH_TEXT; }
            set { SetProperty(ref _M_SEARCH_TEXT, value, () => M_SEARCH_TEXT); }
        }

        public IList<SaleVo> SelectMstList
        {
            get { return selectedMstList; }
            set { SetProperty(ref selectedMstList, value, () => SelectMstList); }
        }

        SaleVo _selectedMstItem;
        public SaleVo SelectedMstItem
        {
            get
            {
                return _selectedMstItem;
            }
            set
            {
                if (value != null)
                {
                    SetProperty(ref _selectedMstItem, value, () => SelectedMstItem);
                }
            }
        }

        public IList<SaleVo> SelectCoMstList
        {
            get { return selectedCoMstList; }
            set { SetProperty(ref selectedCoMstList, value, () => SelectCoMstList); }
        }

        SaleVo _selectedCoMstItem;
        public SaleVo SelectedCoMstItem
        {
            get
            {
                return _selectedCoMstItem;
            }
            set
            {
                if (value != null)
                {
                    SetProperty(ref _selectedCoMstItem, value, () => SelectedCoMstItem, SelectMstDetail);
                }
            }
        }

        public IList<SaleVo> SelectGRList
        {
            get { return selectedGRList; }
            set { SetProperty(ref selectedGRList, value, () => SelectGRList); }
        }

        SaleVo _selectedGRItem;
        public SaleVo SelectedGRItem
        {
            get
            {
                return _selectedGRItem;
            }
            set
            {
                if (value != null)
                {
                    SetProperty(ref _selectedGRItem, value, () => SelectedGRItem);
                    //SetProperty(ref _selectedMstItem, value, () => SelectedMstItem, SelectDetailRefresh);
                }
            }
        }


        private bool? _isGr_DELETE = false;
        public bool? isGr_DELETE
        {
            get { return _isGr_DELETE; }
            set { SetProperty(ref _isGr_DELETE, value, () => isGr_DELETE); }
        }

        private bool? _isLoc_DELETE = false;
        public bool? isLoc_DELETE
        {
            get { return _isLoc_DELETE; }
            set { SetProperty(ref _isLoc_DELETE, value, () => isLoc_DELETE); }
        }

        private bool? _isLoc_UPDATE = false;
        public bool? isLoc_UPDATE
        {
            get { return _isLoc_UPDATE; }
            set { SetProperty(ref _isLoc_UPDATE, value, () => isLoc_UPDATE); }
        }

        private bool? _isO_GR_UPDATE = false;
        public bool? isO_GR_UPDATE
        {
            get { return _isO_GR_UPDATE; }
            set { SetProperty(ref _isO_GR_UPDATE, value, () => isO_GR_UPDATE); }
        }

        private bool? _isN__GR_UPDATE = false;
        public bool? isN__GR_UPDATE
        {
            get { return _isN__GR_UPDATE; }
            set { SetProperty(ref _isN__GR_UPDATE, value, () => isN__GR_UPDATE); }
        }
        ////사업장
        //private IList<SystemCodeVo> _AreaCd = new List<SystemCodeVo>();
        //public IList<SystemCodeVo> AreaList
        //{
        //    get { return _AreaCd; }
        //    set { SetProperty(ref _AreaCd, value, () => AreaList); }
        //}

        ////사업장
        //private SystemCodeVo _M_SL_AREA_NM;
        //public SystemCodeVo M_SL_AREA_NM
        //{
        //    get { return _M_SL_AREA_NM; }
        //    set { SetProperty(ref _M_SL_AREA_NM, value, () => M_SL_AREA_NM); }
        //}

        #endregion

        public async void SYSTEM_CODE_VO()
        {
        //    //사업장
        //    //AreaList = SystemProperties.SYSTEM_CODE_VO("L-002");
        //    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + AquilaErpWpfApp3.Properties.Settings.Default.SettingChnl + "/" + "L-002"))
        //    {
        //        if (response.IsSuccessStatusCode)
        //        {
        //            AreaList = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
        //            if (AreaList.Count > 0)
        //            {
        //                M_SL_AREA_NM = AreaList[0];
        //            }
        //        }
        //    }
        //    Refresh();
        }
    }
}
