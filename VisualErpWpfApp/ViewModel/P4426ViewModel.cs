using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Code;
using ModelsLibrary.Pur;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Windows;
using AquilaErpWpfApp3.Util;

namespace AquilaErpWpfApp3.ViewModel
{
    public sealed class P4426ViewModel : ViewModelBase, INotifyPropertyChanged
    {
        //private static PurServiceClient purClient = SystemProperties.PurClient;
        private IList<PurVo> selectedMstList = new List<PurVo>();
        private IList<PurVo> selectedTmpList = new List<PurVo>();

        private int nDiffMonth;
        private List<string> diffMonth = new List<string>();
        //private ObservableCollection<PurVo> selectedDtlList = new ObservableCollection<PurVo>();
       
        public P4426ViewModel() 
        {
            StartDt = System.DateTime.Now;
            EndDt = System.DateTime.Now;

            SYSTEM_CODE_VO();

            //DeptList = SystemProperties.CUSTOMER_CODE_VO(null, null);
            //DeptList.Insert(0, new CustomerCodeDao() { CO_NO = "", CO_NM = "" });
            //_DeptMap = SystemProperties.CUSTOMER_CODE_MAP(null, null);
            //M_DEPT_DESC = "";

            //DeptList = SystemProperties.SYSTEM_DEPT_VO();
            //DeptList.Insert(0, new CodeDao() { CLSS_CD = "", CLSS_DESC = "" });
            //_DeptMap = SystemProperties.SYSTEM_DEPT_MAP();

            //M_DEPT_DESC = SystemProperties.USER_VO.GRP_NM;
            //Refresh();
        }

        [Command]
        public async void Refresh()
        {
                //DXSplashScreen.Show<ProgressWindow>();
                //SearchDetail = null;
                //SelectDtlList = null;

                //SelectMstList = purClient.P4412SelectMstList(new PurVo() { FM_DT = (StartDt).ToString("yyyy-MM-dd"), TO_DT = (EndDt).ToString("yyyy-MM-dd"), CO_NO = (string.IsNullOrEmpty(M_DEPT_DESC) ? null : M_DEPT_DESC) });
                //////
                //Title = "[기간]" + (StartDt).ToString("yyyy-MM-dd") + "~" + (EndDt).ToString("yyyy-MM-dd") + ",   [거래처]" + (string.IsNullOrEmpty(M_DEPT_DESC) ? "전체" : _DeptMap[M_DEPT_DESC]) + (string.IsNullOrEmpty(M_SEARCH_TEXT) ? "" : (",   [검 색]" + M_SEARCH_TEXT));

                //if (SelectMstList.Count >= 1)
                //{
                //    isM_UPDATE = true;
                //    isM_DELETE = true;

                //    SelectedMstItem = SelectMstList[0];
                //}
                //else
                //{
                //    isM_UPDATE = false;
                //    isM_DELETE = false;
                //    //
                //    isD_UPDATE = false;
                //    isD_DELETE = false;
                //}
                ////DXSplashScreen.Close();
            try
            {
                selectedTmpList = new List<PurVo>();
                SelectMstList = new List<PurVo>();

                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("p4426/mst", new StringContent(JsonConvert.SerializeObject(new PurVo() { FM_DT = (StartDt).ToString("yyyyMMdd"), TO_DT = (EndDt).ToString("yyyyMMdd"), CO_NO = M_SL_CO_NM.CO_NO, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        selectedTmpList = JsonConvert.DeserializeObject<IEnumerable<PurVo>>(await response.Content.ReadAsStringAsync()).Cast<PurVo>().ToList();
                    }

                    //selectedTmpList = purClient.P4426SelectMstList(new PurVo()
                    //{
                    //     FM_DT = (StartDt).ToString("yyyyMMdd")
                    //   , TO_DT = (EndDt).ToString("yyyyMMdd")
                    //   , CO_NO = (string.IsNullOrEmpty(M_DEPT_DESC.Trim()) ? null : M_DEPT_DESC)
                    //});

                    Title = "[기간]" + (StartDt).ToString("yyyy-MM-dd") + "~" + (EndDt).ToString("yyyy-MM-dd") + ",   [거래처]" + M_SL_CO_NM.CO_NM;

                    if (selectedTmpList.Count > 0)
                    {
                        //이월
                        //if (selectedTmpList[0].INAUD_RMK.Equals("이월"))
                        //{
                        //    selectedTmpList[0].YRMON = "0";
                        //    SelectMstList.Add(selectedTmpList[0]);
                        //}

                        //월수
                        this.diffMonth.Clear();
                        this.nDiffMonth = (12 * (EndDt.Year - StartDt.Year)) + (EndDt.Month - StartDt.Month);
                        for (int x = 0; x <= this.nDiffMonth; x++)
                        {
                            this.diffMonth.Insert(x, StartDt.AddMonths(x).ToString("yyyyMM"));
                        }

                        //계산
                        IList<PurVo> tmpList = new List<PurVo>();
                        object TTL_IMP_ITM_AMT = 0;
                        object TTL_ITM_SUM_AMT = 0;
                        //object TTL_OUT_QTY = 0;
                        //object TTL_OUT_AMT = 0;
                        //object TTL_STK_QTY = 0;
                        //object TTL_STK_AMT = 0;

                        //if (selectedTmpList[0].INAUD_RMK.Equals("이월"))
                        //{
                        //    TTL_IMP_ITM_AMT = SelectMstList[0].IMP_ITM_AMT;
                        //    TTL_ITM_SUM_AMT = SelectMstList[0].ITM_SUM_AMT;
                        //    //TTL_OUT_QTY = SelectMstList[0].OUT_QTY;
                        //    //TTL_OUT_AMT = SelectMstList[0].OUT_AMT;
                        //    //TTL_STK_QTY = SelectMstList[0].STK_QTY;
                        //    //TTL_STK_AMT = SelectMstList[0].STK_AMT;
                        //}


                        object MON_IMP_ITM_AMT = 0;
                        object MON_ITM_SUM_AMT = 0;
                        //object MON_OUT_QTY = 0;
                        //object MON_OUT_AMT = 0;
                        //object MON_STK_QTY = 0;
                        //object MON_STK_AMT = 0;
                        for (int x = 0; x < this.diffMonth.Count; x++)
                        {
                            MON_IMP_ITM_AMT = 0;
                            MON_ITM_SUM_AMT = 0;
                            //MON_OUT_QTY = 0;
                            //MON_OUT_AMT = 0;
                            //MON_STK_QTY = 0;
                            //MON_STK_AMT = 0;
                            tmpList = selectedTmpList.Where(z => z.YRMON == this.diffMonth[x]).OrderBy(z => z.PUR_CLZ_YRMON).ToList<PurVo>();
                            for (int y = 0; y < tmpList.Count; y++)
                            {
                                SelectMstList.Add(tmpList[y]);
                                //(월계)
                                MON_IMP_ITM_AMT = Convert.ToInt32(MON_IMP_ITM_AMT) + Convert.ToInt32(tmpList[y].IMP_ITM_AMT);
                                MON_ITM_SUM_AMT = Convert.ToDouble(MON_ITM_SUM_AMT) + Convert.ToDouble(tmpList[y].ITM_SUM_AMT);
                                //MON_OUT_QTY = Convert.ToInt32(MON_OUT_QTY) + Convert.ToInt32(tmpList[y].OUT_QTY);
                                //MON_OUT_AMT = Convert.ToDouble(MON_OUT_AMT) + Convert.ToDouble(tmpList[y].OUT_AMT);
                                //MON_STK_QTY = Convert.ToInt32(MON_STK_QTY) + Convert.ToInt32(tmpList[y].STK_QTY);
                                //MON_STK_AMT = Convert.ToDouble(MON_STK_AMT) + Convert.ToDouble(tmpList[y].STK_AMT);
                                //(누계)
                                TTL_IMP_ITM_AMT = Convert.ToInt32(TTL_IMP_ITM_AMT) + Convert.ToInt32(tmpList[y].IMP_ITM_AMT);
                                TTL_ITM_SUM_AMT = Convert.ToDouble(TTL_ITM_SUM_AMT) + Convert.ToDouble(tmpList[y].ITM_SUM_AMT);
                                //TTL_OUT_QTY = Convert.ToInt32(TTL_OUT_QTY) + Convert.ToInt32(tmpList[y].OUT_QTY);
                                //TTL_OUT_AMT = Convert.ToDouble(TTL_OUT_AMT) + Convert.ToDouble(tmpList[y].OUT_AMT);
                                //TTL_STK_QTY = Convert.ToInt32(TTL_STK_QTY) + Convert.ToInt32(tmpList[y].STK_QTY);
                                //TTL_STK_AMT = Convert.ToDouble(TTL_STK_AMT) + Convert.ToDouble(tmpList[y].STK_AMT);
                                //(월계) && (합계)
                                if ((tmpList.Count - 1) == y)
                                {
                                    SelectMstList.Add(new PurVo() { PUR_CLZ_YRMON = this.diffMonth[x] + " 월계", YRMON = "1", INAUD_RMK = this.diffMonth[x] + " 월계", IMP_ITM_AMT = MON_IMP_ITM_AMT, ITM_SUM_AMT = MON_ITM_SUM_AMT });
                                    SelectMstList.Add(new PurVo() { PUR_CLZ_YRMON = " 누계", YRMON = "2", INAUD_RMK = " 누계", IMP_ITM_AMT = TTL_IMP_ITM_AMT, ITM_SUM_AMT = TTL_ITM_SUM_AMT });
                                }
                            }

                        }

                        if (SelectMstList.Count >= 1)
                        {

                            this.nDiffMonth = (12 * (EndDt.Year - StartDt.Year)) + (EndDt.Month - StartDt.Month);
                            //this.diffMonth
                            for (int x = 0; x <= this.nDiffMonth; x++)
                            {
                                this.diffMonth.Insert(x, StartDt.AddMonths(x).ToString("yyyyMM"));
                            }

                            isM_UPDATE = true;
                            isM_DELETE = true;

                            SelectedMstItem = SelectMstList[0];
                        }
                        else
                        {
                            isM_UPDATE = false;
                            isM_DELETE = false;
                            //
                            //isD_UPDATE = false;
                            //isD_DELETE = false;
                        }
                    }

                    //매입원장 날짜변경 시 Refresh안되는 현상 
                    TmpList.Clear();
                    for (int x = 0; x < SelectMstList.Count; x++)
                    {
                        TmpList.Add(SelectMstList[x]);
                    }

                    this.SelectMstList = TmpList;
                }
            }
            catch (System.Exception eLog)
            {
                //DXSplashScreen.Close();
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]매입원장[일자]", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }

        //#region Functon <Master List>
        DateTime _startDt;
        public DateTime StartDt
        {
            get { return _startDt; }
            set { SetProperty(ref _startDt, value, () => StartDt); }
        }

        DateTime _endDt;
        public DateTime EndDt
        {
            get { return _endDt; }
            set { SetProperty(ref _endDt, value, () => EndDt); }
        }

        private IList<SystemCodeVo> _CoNmList = new List<SystemCodeVo>();
        public IList<SystemCodeVo> CoNmList
        {
            get { return _CoNmList; }
            set { SetProperty(ref _CoNmList, value, () => CoNmList); }
        }
        //private CustomerCodeDao _M_DEPT_DESC;
        //public CustomerCodeDao M_DEPT_DESC
        //{
        //    get { return _M_DEPT_DESC; }
        //    set { SetProperty(ref _M_DEPT_DESC, value, () => M_DEPT_DESC); }
        //}

        private SystemCodeVo _M_SL_CO_NM;
        public SystemCodeVo M_SL_CO_NM
        {
            get { return _M_SL_CO_NM; }
            set { SetProperty(ref _M_SL_CO_NM, value, () => _M_SL_CO_NM); }
        }


        private string _M_SEARCH_TEXT = string.Empty;
        public string M_SEARCH_TEXT
        {
            get { return _M_SEARCH_TEXT; }
            set { SetProperty(ref _M_SEARCH_TEXT, value, () => M_SEARCH_TEXT); }
        }


        //private string _M_PUR_CLZ_FLG = string.Empty;
        //public string M_PUR_CLZ_FLG
        //{
        //    get { return _M_PUR_CLZ_FLG; }
        //    set { SetProperty(ref _M_PUR_CLZ_FLG, value, () => M_PUR_CLZ_FLG); }
        //}


        //private bool? _M_SEARCH_CHECKD = false;
        //public bool? M_SEARCH_CHECKD
        //{
        //    get { return _M_SEARCH_CHECKD; }
        //    set { SetProperty(ref _M_SEARCH_CHECKD, value, () => M_SEARCH_CHECKD); }
        //}


        public IList<PurVo> SelectMstList
        {
            get { return selectedMstList; }
            set { SetProperty(ref selectedMstList, value, () => SelectMstList); }
        }

        PurVo _selectedMstItem;
        public PurVo SelectedMstItem
        {
            get
            {
                return _selectedMstItem;
            }
            set
            {
                if (value != null)
                {
                    SetProperty(ref _selectedMstItem, value, () => SelectedMstItem, SelectMstDetail);
                }
            }
        }

        private IList<PurVo> _TmpList = new List<PurVo>();

        public IList<PurVo> TmpList
        {
            get { return _TmpList; }
            set { SetProperty(ref _TmpList, value, () => TmpList); }
        }

        //private bool? _isM_SAVE = false;
        //public bool? isM_SAVE
        //{
        //    get { return _isM_SAVE; }
        //    set { SetProperty(ref _isM_SAVE, value, () => isM_SAVE); }
        //}

        private bool? _isM_UPDATE = false;
        public bool? isM_UPDATE
        {
            get { return _isM_UPDATE; }
            set { SetProperty(ref _isM_UPDATE, value, () => isM_UPDATE); }
        }

        private bool? _isM_DELETE = false;
        public bool? isM_DELETE
        {
            get { return _isM_DELETE; }
            set { SetProperty(ref _isM_DELETE, value, () => isM_DELETE); }
        }

        //
        private bool? _isD_UPDATE = false;
        public bool? isD_UPDATE
        {
            get { return _isD_UPDATE; }
            set { SetProperty(ref _isD_UPDATE, value, () => isD_UPDATE); }
        }

        private bool? _isD_DELETE = false;
        public bool? isD_DELETE
        {
            get { return _isD_DELETE; }
            set { SetProperty(ref _isD_DELETE, value, () => isD_DELETE); }
        }


        private void SelectMstDetail()
        {
             //try
             //{
             //    //DXSplashScreen.Show<ProgressWindow>();

             //   if (this._selectedMstItem == null)
             //   {
             //       return;
             //   }

             //   //SelectDtlList = new ObservableCollection<PurVo>(purClient.P4401SelectDtlList(new PurVo() { ITM_CD = this._selectedMstItem.}));
             //   SelectDtlList = purClient.P4411SelectDtlList(SelectedMstItem);
             //   // //
             //   if (SelectDtlList.Count >= 1)
             //   {
             //       isD_UPDATE = true;
             //       isD_DELETE = true;

             //       SearchDetail = SelectDtlList[0];
             //   }
             //   else
             //   {
             //       isD_UPDATE = false;
             //       isD_DELETE = false;
             //   }

             //   //DXSplashScreen.Close();
             //}
             //catch (System.Exception eLog)
             //{
             //    //DXSplashScreen.Close();
             //    //
             //    WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]발주 등록 관리", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
             //    return;
             //}
        }
        //#endregion


        ////#region Functon <Detail List>
        //public IList<PurVo> SelectDtlList
        //{
        //    get { return selectedDtlList; }
        //    set { SetProperty(ref selectedDtlList, value, () => SelectDtlList); }
        //}

        //PurVo _searchDetail;
        //public PurVo SearchDetail
        //{
        //    get
        //    {
        //        return _searchDetail;
        //    }
        //    set
        //    {
        //        if (value != null)
        //        {
        //            SetProperty(ref _searchDetail, value, () => SearchDetail);
        //        }
        //    }
        //}
        ////#endregion


        string _Title = string.Empty;
        public string Title
        {
            get { return _Title; }
            set { SetProperty(ref _Title, value, () => Title); }
        }


        public async void SYSTEM_CODE_VO()
        {
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s143", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { DELT_FLG = "N", /*CO_TP_CD = "AR", */ AREA_CD = SystemProperties.USER_VO.EMPE_PLC_NM, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    CoNmList = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                    if (CoNmList.Count > 0)
                    {
                        CoNmList.Insert(0, new SystemCodeVo() { CO_NM = "전체", CO_NO = null });
                        M_SL_CO_NM = CoNmList[0];
                    }
                }
            }

            Refresh();
        }

    }
}
