using AquilaErpWpfApp3.Util;
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
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AquilaErpWpfApp3.ViewModel
{
    class M661010ViewModel : ViewModelBase, INotifyPropertyChanged
    {

        string _title = "오더매니저 설비";

        public M661010ViewModel()
        {
            StartDt = DateTime.Now;
            EndDt = DateTime.Now;
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
            catch(Exception eLog)
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
                    // Z1 : 전체설비 기준 최적화 지시 정보 조회
                    PROD_EQ_NO = SelectedMstItem.PROD_EQ_NO.Equals("Z1") ? null : SelectedMstItem.PROD_EQ_NO
                };

                // 조회 스플레쉬 기능
                if (DXSplashScreen.IsActive == false) DXSplashScreen.Show<ProgressWindow>();

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
                        if(ImgList[0].IMG != null)
                        {
                            // 이미지 있음.
                            ImgArray = ImgList[0].IMG;
                            return;
                        }
                    }
                }

                // 이미지 없음.
                ImgArray = new byte[0];
            }
            catch (Exception eLog)
            {
                // 실패
                ImgArray = new byte[0];
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
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync(Path
                                                                                                  , new StringContent(JsonConvert.SerializeObject(obj), System.Text.Encoding.UTF8, "application/json")))
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
        private byte[] _imgArray = new byte[0];
        public byte[] ImgArray
        {
            get { return _imgArray; }
            set { SetProperty(ref _imgArray, value, () => ImgArray); }
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
