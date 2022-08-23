using AquilaErpWpfApp3.Util;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Code;
using ModelsLibrary.Man;
using ModelsLibrary.Sale;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Windows;


namespace AquilaErpWpfApp3.ViewModel
{
    public sealed class S22225ViewModel : ViewModelBase, INotifyPropertyChanged 
    {
        private string title = "GR번호 확정";

        private IList<SaleVo> selectMstList = new List<SaleVo>();   
        private IList<SaleVo> selectDtlList = new List<SaleVo>();
        private IList<SaleVo> selectItemsList = new List<SaleVo>();

        private SaleVo selectMstGr = new SaleVo();

        private DateTime _startDT;
        private DateTime _endDT;
        private DateTime _inputDT;
        // Mst와 Dtl 검색조건
        private string _mstSearch = string.Empty;   
        private string _dtlSearch = string.Empty;


        public S22225ViewModel()
        {
            this.StartDT = System.DateTime.Now;
            this.EndDT = System.DateTime.Now;


            this.InputDT = System.DateTime.Now;

            SYSTEM_CODE_VO();

        }

        #region MyRegion
        [Command]
        public async void Refresh(string _RLSE_CMD_NO = null)
        {
            try
            {
                SaleVo _param = new SaleVo();
                _param.FM_DT = (StartDT).ToString("yyyy-MM-dd");
                _param.TO_DT = (EndDT).ToString("yyyy-MM-dd");
                _param.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;


                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s22225/mst", new StringContent(JsonConvert.SerializeObject(_param), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.GrItemsSelectList = new List<SaleVo>();

                        this.GrMstSelectList = JsonConvert.DeserializeObject<IEnumerable<SaleVo>>(await response.Content.ReadAsStringAsync()).Cast<SaleVo>().ToList();
                        if (this.GrMstSelectList.Count >= 1)
                        {

                            isM_UPDATE = true;
                            isM_DELETE = true;


                            if (string.IsNullOrEmpty(_RLSE_CMD_NO))
                            {
                                this.selectMstGr = this.GrMstSelectList[0];
                            }
                            else
                            {
                                this.selectMstGr = this.GrMstSelectList.Where(x => x.RLSE_CMD_NO.Equals(_RLSE_CMD_NO)).LastOrDefault<SaleVo>();
                            }
                        }
                        else
                        {
                            isM_UPDATE = false;
                            isM_DELETE = false;
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

        // Detail 검색 
        public async void DtlRefresh()
        {
            if (selectMstGr == null) return;

            try
            {
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s22225/dtl", new StringContent(JsonConvert.SerializeObject(selectMstGr), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.GrDtlSelectList = JsonConvert.DeserializeObject<IEnumerable<SaleVo>>(await response.Content.ReadAsStringAsync()).Cast<SaleVo>().ToList();
                        DtlSearch = selectMstGr.RLSE_CMD_NO.ToString();

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
        public async void Apply()
        {
            try
            {

                MessageBoxResult result = WinUIMessageBox.Show("[ 총 :" + GrItemsSelectList.Count + " ] 정말로 생산분배 하시겠습니까?", title, MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    string _RLSE_CMD_NO = this.GrItemsSelectList[GrItemsSelectList.Count - 1].RLSE_CMD_NO;

                    List<SaleVo> _saveList = new List<SaleVo>();
                    foreach (SaleVo _item in GrItemsSelectList)
                    {
                        _item.RLSE_DIV_DT = (InputDT).ToString("yyyy-MM-dd");
                        _item.PROD_EQ_NO = M_EQ_NM.PROD_EQ_NO;
                        _item.CRE_USR_ID = SystemProperties.USER;
                        _item.UPD_USR_ID = SystemProperties.USER;
                        _item.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;

                        _saveList.Add(_item);
                    }


                    int _Num = 0;
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s22225/mst/i", new StringContent(JsonConvert.SerializeObject(_saveList), System.Text.Encoding.UTF8, "application/json")))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string resultMsg = await response.Content.ReadAsStringAsync();
                            if (int.TryParse(resultMsg, out _Num) == false)
                            {
                                //실패
                                WinUIMessageBox.Show(resultMsg, title, MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }

                            Refresh(_RLSE_CMD_NO);

                            //성공
                            WinUIMessageBox.Show("완료 되었습니다", title, MessageBoxButton.OK, MessageBoxImage.Information);
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

        #endregion



            #region MyRegion
        public IList<SaleVo> GrMstSelectList
        {
            get { return selectMstList; }
            set { SetProperty(ref selectMstList, value, () => GrMstSelectList); }
        }

        public IList<SaleVo> GrDtlSelectList
        {
            get { return selectDtlList; }
            set { SetProperty(ref selectDtlList, value, () => GrDtlSelectList); }
        }

        public IList<SaleVo> GrItemsSelectList
        {
            get { return selectItemsList; }
            set { SetProperty(ref selectItemsList, value, () => GrItemsSelectList); }
        }




        public SaleVo GrSelectedItem
        {
            get { return selectMstGr; }
            set { SetProperty(ref selectMstGr, value, () => GrSelectedItem, DtlRefresh); }         // DtlRefresh 검색 
        }

        public DateTime StartDT
        {
            get { return _startDT; }
            set { SetProperty(ref _startDT, value, () => StartDT); }
        }

        public DateTime EndDT
        {
            get { return _endDT; }
            set { SetProperty(ref _endDT, value, () => EndDT); }
        }






        //분배일
        public DateTime InputDT
        {
            get { return _inputDT; }
            set { SetProperty(ref _inputDT, value, () => InputDT); }
        }

        //설비
        private IList<ManVo> _EqCd = new List<ManVo>();
        public IList<ManVo> EqList
        {
            get { return _EqCd; }
            set { SetProperty(ref _EqCd, value, () => EqList); }
        }
        //설비
        private ManVo _M_EQ_NM;
        public ManVo M_EQ_NM
        {
            get { return _M_EQ_NM; }
            set { SetProperty(ref _M_EQ_NM, value, () => M_EQ_NM); }
        }




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



        public string MstSearch
        {
            get { return _mstSearch; }
            set { SetProperty(ref _mstSearch, value, () => MstSearch); }
        }

        public string DtlSearch
        {
            get { return _dtlSearch; }
            set { SetProperty(ref _dtlSearch, value, () => DtlSearch); }
        }


        #endregion





        public async void SYSTEM_CODE_VO()
        {
            //설비
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6641", new StringContent(JsonConvert.SerializeObject(new ManVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD, DELT_FLG = "N" }), System.Text.Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    EqList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
                    if (EqList.Count > 0)
                    {
                        M_EQ_NM = EqList[0];
                    }
                }
            }


        }


    }
}
