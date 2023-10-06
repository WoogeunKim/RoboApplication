using AquilaErpWpfApp3.Util;
using AquilaErpWpfApp3.View.M.Dialog;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Man;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace AquilaErpWpfApp3.ViewModel
{
    class M661010ViewModel : ViewModelBase, INotifyPropertyChanged
    {

        string _title = "오더매니저 설비";

        private M661010DetailDialog lovDialog;
        public M661010ViewModel()
        {
            StartDt = DateTime.Now;
            EndDt = DateTime.Now;

            SYSTEM_CODE_VO();
        }

        /// <summary>
        /// DTL 설비 : 설비 ComboBox 정보들을 가져옵니다.
        /// </summary>
        private async void SYSTEM_CODE_VO()
        {
            try
            {
                var eqObj = new ManVo()
                {
                    CHNL_CD = SystemProperties.USER_VO.CHNL_CD
                };

                // 서버로부터 절단설비 정보를 가져옵니다.
                SelectN1stList = await PostJsonList<ManVo>("M66311/n1st/eq", eqObj);

                // 서버로부터 가공설비 정보를 가져옵니다.
                SelectN2ndList = await PostJsonList<ManVo>("M66311/n2nd/eq", eqObj);
            }
            catch (Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, _title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        /// <summary>
        /// MST : Loss최적화 배정된 설비 조회
        /// </summary>
        [Command]
        public async void Refresh()
        {
            try
            {
                var mstObj = new ManVo()
                {
                    CHNL_CD = SystemProperties.USER_VO.CHNL_CD,
                    FM_DT = StartDt.ToString("yyyy-MM-dd"),
                    TO_DT = EndDt.ToString("yyyy-MM-dd")
                };

                // 서버로부터 MST 정보를 가져옵니다.
                SelectMstList = await PostJsonList<ManVo>("m661010/mst", mstObj);
            }
            catch (Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, _title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// DTL : 설비 배정 최적화 지시 정보 조회
        /// </summary>
        public async void DetailRefresh()
        {
            try
            {
                if (SelectedMstItem == null) return;

                var dtlObj = new ManVo()
                {
                    CHNL_CD = SystemProperties.USER_VO.CHNL_CD,
                    FM_DT = StartDt.ToString("yyyy-MM-dd"),
                    TO_DT = EndDt.ToString("yyyy-MM-dd"),
                    EQ_NO = SelectedMstItem.EQ_NO
                };


                // 조회 스플레쉬 기능
                if (DXSplashScreen.IsActive == false) DXSplashScreen.Show<ProgressWindow>();

                IList<ManVo> voList = new List<ManVo>();

                // 서버로부터 DTL 정보를 가져옵니다.
                SelectDtlList = await PostJsonList<ManVo>("m661010/dtl", dtlObj);

                if (SelectDtlList.Count > 0)
                {
                    SelectedDtlItem = SelectDtlList[0];
                }
                else
                {
                    SelectedDtlItem = null;
                }


                // 성공
                if (DXSplashScreen.IsActive == true) DXSplashScreen.Close();

                SelectLovListDetail();

            }
            catch (Exception eLog)
            {
                // 실패
                if (DXSplashScreen.IsActive == true) DXSplashScreen.Close();

                WinUIMessageBox.Show(eLog.Message, _title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //SL리스트 선택 시 
        [Command]
        public async void SelectDetailCheckd()
        {

            try
            {
                if (SelectedDtlItem == null)
                {
                    return;
                }


                if (SelectedDtlItem.isCheckd)
                {
                    SelectedDtlItem.isCheckd = false;
                }
                else
                {
                    SelectedDtlItem.isCheckd = true;
                }



                //작업지시리스트 조회
                SelectLovListDetail();
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
            }
        }


        //작업지시리스트 선택 시 체크 박스 자동 선택
        [Command]
        public async void SelectLovCheckd()
        {

            try
            {
                if (SelectLovItem == null)
                {
                    return;
                }


                if (SelectLovItem.isCheckd)
                {
                    SelectLovItem.isCheckd = false;
                }
                else
                {
                    SelectLovItem.isCheckd = true;
                }

            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
            }
        }



        //작업지시리스트
        public async void SelectLovListDetail(string _LOT_DIV_NO = null)
        {
            try
            {
                if (this.SelectDtlList.Any<ManVo>(x => x.isCheckd == true))
                {
                    SelectedDtlItem.A_LOT_DIV_NO = this.SelectDtlList.Where<ManVo>(w => w.isCheckd == true).Select(x => (x.SL_RLSE_NO + "_" + x.SL_RLSE_SEQ)).ToArray<string>();

                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m661010/dtl/lov", new StringContent(JsonConvert.SerializeObject(SelectedDtlItem), System.Text.Encoding.UTF8, "application/json")))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            this.SelectLovList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();

                            // //
                            if (SelectLovList.Count >= 1)
                            {
                                if (!string.IsNullOrEmpty(_LOT_DIV_NO))
                                {
                                    this.SelectLovItem = this.SelectLovList.Where(x => x.LOT_DIV_NO.Equals(_LOT_DIV_NO)).FirstOrDefault<ManVo>();
                                }

                            }
                            else
                            {
                                SelectLovItem = null;
                            }
                        }
                    }
                    ////// 조회 스플레쉬 기능
                    ////if (DXSplashScreen.IsActive == false) DXSplashScreen.Show<ProgressWindow>();

                    //// 서버로부터 DTL 정보를 가져옵니다.
                    //SelectLovList = await PostJsonList<ManVo>("m661010/dtl/lov", lovObj);

                    //if (SelectLovList != null)
                    //{
                    //    if (SelectLovList.Count >= 1)
                    //    {
                    //        if (!string.IsNullOrEmpty(_LOT_DIV_NO))
                    //        {
                    //            this.SelectLovItem = this.SelectLovList.Where(x => x.LOT_DIV_NO.Equals(_LOT_DIV_NO)).FirstOrDefault<ManVo>();
                    //        }

                    //    }
                    //    else
                    //    {
                    //        SelectLovItem = null;
                    //    }

                    //}
                }
                else
                {
                    this.SelectLovList = null;
                }

                // 성공
                if (DXSplashScreen.IsActive == true) DXSplashScreen.Close();


            }
            catch (Exception eLog)
            {
                // 실패
                if (DXSplashScreen.IsActive == true) DXSplashScreen.Close();

                WinUIMessageBox.Show(eLog.Message, _title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        /// <summary>
        /// IMG : 형상 이미지 정보를 가져옵니다.
        /// </summary>
        [Command]
        public async void DtlImgRefresh()
        {
            try
            {
                if (SelectedDtlItem == null)
                {
                    ImgArray = new byte[0];
                    return;
                }

                var imgObj = new ManVo()
                {
                    CHNL_CD = SystemProperties.USER_VO.CHNL_CD,
                    ITM_CD = SelectedDtlItem.ITM_SHP_CD
                };

                // 서버로부터 형상 이미지 정보를 가져옵니다.
                var ImgList = await PostJsonList<ManVo>("m661010/dtl/img", imgObj);

                if (ImgList != null)
                {
                    if (ImgList.Count >= 1)
                    {
                        if (ImgList[0].IMG != null)
                        {
                            // 이미지 있음.
                            ImgArray = ImgList[0].IMG;
                            return;
                        }
                    }
                }
                // 이미지 없음.
                ImgArray = new byte[0];

                SelectLovListDetail();
            }
            catch (Exception eLog)
            {
                // 실패
                ImgArray = new byte[0];
                WinUIMessageBox.Show(eLog.Message, _title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //생산계획 관리
        [Command]
        public async void LovPlan()
        {
            try
            {
                IList<ManVo> CheckDtlList = SelectDtlList.Where<ManVo>(x => x.isCheckd == true).ToList();

                for (int i = 0; i < CheckDtlList.Count; i++)
                {
                    if (Convert.ToDouble(CheckDtlList[i].RMN_QTY) <= 0)
                    {
                        WinUIMessageBox.Show("오더잔여수량이 0 입니다.", _title, MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }

                if (CheckDtlList.Count > 0)
                {
                    lovDialog = new M661010DetailDialog(CheckDtlList);
                    lovDialog.Title = " 생산 게획 관리 ";
                    lovDialog.Owner = Application.Current.MainWindow;
                    lovDialog.BorderEffect = BorderEffect.Default;
                    lovDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
                    lovDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
                    bool isDialog = (bool)lovDialog.ShowDialog();
                    if (isDialog)
                    {
                        // 서버로부터 DTL 정보를 초기화합니다.
                        IList<ManVo> voList = new List<ManVo>();
                        voList = await PostJsonList<ManVo>("m661010/dtl", new ManVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD, FM_DT = StartDt.ToString("yyyy-MM-dd"), TO_DT = EndDt.ToString("yyyy-MM-dd"), EQ_NO = SelectedMstItem.EQ_NO });

                        foreach(ManVo vo in SelectDtlList.Where<ManVo>(x => x.isCheckd == true).ToList())
                        {
                            voList[(int)vo.RN - 1].isCheckd = true;
                        }

                        this.SelectDtlList = voList;

                        SelectLovListDetail();
                    }
                }
                else
                {
                    WinUIMessageBox.Show("[생산계획]을 선택하지 않았습니다.", "[유효검사]", MessageBoxButton.OK, MessageBoxImage.Warning);
                }

            }
            catch (Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, _title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [Command]
        public async void LovDelete()
        {
            try
            {
                if (this.SelectLovList.Any<ManVo>(x => x.isCheckd == true))
                {
                    IList<ManVo> checkedList = this.SelectLovList.Where(x => x.isCheckd == true).ToList();
                    MessageBoxResult result = WinUIMessageBox.Show(checkedList.Count + "개를 삭제하시겠습니까?", _title, MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("M661010/lov/d", new StringContent(JsonConvert.SerializeObject(checkedList), System.Text.Encoding.UTF8, "application/json")))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                int _Num = 0;
                                string resultMsg = await response.Content.ReadAsStringAsync();
                                if (int.TryParse(resultMsg, out _Num) == false)
                                {
                                    //실패
                                    WinUIMessageBox.Show(resultMsg, _title, MessageBoxButton.OK, MessageBoxImage.Error);
                                    return;
                                }

                                //성공
                                // 서버로부터 DTL 정보를 초기화합니다.
                                IList<ManVo> voList = new List<ManVo>();
                                voList = await PostJsonList<ManVo>("m661010/dtl", new ManVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD, FM_DT = StartDt.ToString("yyyy-MM-dd"), TO_DT = EndDt.ToString("yyyy-MM-dd"), EQ_NO = SelectedMstItem.EQ_NO });

                                foreach (ManVo vo in SelectDtlList.Where<ManVo>(x => x.isCheckd == true).ToList())
                                {
                                    voList[(int)vo.RN - 1].isCheckd = true;
                                }

                                this.SelectDtlList = voList;

                                SelectLovListDetail();
                                WinUIMessageBox.Show("삭제되었습니다.", _title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                            }
                        }
                    }
                }
                else
                {
                    return;
                }

            }
            catch (Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, _title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }








        /// <summary>
        /// Post JSON 을 통해 서버로부터 정보를 가져옵니다.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Path"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        private async Task<List<T>> PostJsonList<T>(string Path, Object obj)
        {
            var ret = default(List<T>);

            try
            {
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync(Path, new StringContent(JsonConvert.SerializeObject(obj), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        ret = JsonConvert.DeserializeObject<IEnumerable<T>>(await response.Content.ReadAsStringAsync()).Cast<T>().ToList();
                    }
                }
            }
            catch (Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, _title, MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return ret;
        }


        #region MVVM 패턴 각 객체를 바이딩 합니다.

        private IList<ManVo> _selectMstList = new List<ManVo>();
        public IList<ManVo> SelectMstList
        {
            get { return _selectMstList; }
            set { SetProperty(ref _selectMstList, value, () => SelectMstList); }
        }
        private ManVo _selectedMstItem;
        public ManVo SelectedMstItem
        {
            get { return _selectedMstItem; }
            set { SetProperty(ref _selectedMstItem, value, () => SelectedMstItem, DetailRefresh); }
        }

        private IList<ManVo> _selectDtlList = new List<ManVo>();
        public IList<ManVo> SelectDtlList
        {
            get { return _selectDtlList; }
            set { SetProperty(ref _selectDtlList, value, () => SelectDtlList); }
        }
        private ManVo _selectedDtlItem;
        public ManVo SelectedDtlItem
        {
            get { return _selectedDtlItem; }
            set { SetProperty(ref _selectedDtlItem, value, () => SelectedDtlItem, DtlImgRefresh); }
        }

        private IList<ManVo> _selectLovList = new List<ManVo>();
        public IList<ManVo> SelectLovList
        {
            get { return _selectLovList; }
            set { SetProperty(ref _selectLovList, value, () => SelectLovList); }
        }

        private ManVo _selectLovItem;
        public ManVo SelectLovItem
        {
            get { return _selectLovItem; }
            set { SetProperty(ref _selectLovItem, value, () => SelectLovItem); }
        }


        private byte[] _imgArray = new byte[0];
        public byte[] ImgArray
        {
            get { return _imgArray; }
            set { SetProperty(ref _imgArray, value, () => ImgArray); }
        }

        private IList<ManVo> _selectN1stList = new List<ManVo>();
        public IList<ManVo> SelectN1stList
        {
            get { return _selectN1stList; }
            set { SetProperty(ref _selectN1stList, value, () => SelectN1stList); }
        }
        private IList<ManVo> _selectN2ndList = new List<ManVo>();
        public IList<ManVo> SelectN2ndList
        {
            get { return _selectN2ndList; }
            set { SetProperty(ref _selectN2ndList, value, () => SelectN2ndList); }
        }


        private DateTime _startDt;
        public DateTime StartDt
        {
            get { return _startDt; }
            set { SetProperty(ref _startDt, value, () => StartDt); }
        }
        private DateTime _endDt;
        public DateTime EndDt
        {
            get { return _endDt; }
            set { SetProperty(ref _endDt, value, () => EndDt); }
        }

        private string _m_SEARCH_TEXT;
        public string M_SEARCH_TEXT
        {
            get { return _m_SEARCH_TEXT; }
            set { SetProperty(ref _m_SEARCH_TEXT, value, () => M_SEARCH_TEXT); }
        }
        #endregion


    }
}
