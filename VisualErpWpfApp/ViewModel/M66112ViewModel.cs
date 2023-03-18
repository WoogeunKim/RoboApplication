using AquilaErpWpfApp3.Util;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
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
    public sealed class M66112ViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private string _title = "고객사 발주 모니터링";
        
        public M66112ViewModel()
        {
            StartDt = System.DateTime.Now;
            EndDt = System.DateTime.Now;
        }


        /// <summary>
        /// 조회 버튼 BarButtonItem - Command 바이딩 
        /// </summary>     
        #region 조회 버튼 RefreshCommand

        [Command]
        public async void Refresh()
        {
            try
            {
                // 조회조건 param
                var _param = GetRefParam();

                if (_param != null)
                {
                    // MST 정보를 서버에 조회
                    this.SelectMstList = await PostJsonData<ManVo>("M66112/mst", _param);
                }
            }
            catch(Exception ex)
            {

            }
        }


        /// <summary>
        /// 조회조건 데이터를 MavVo로 반환합니다.
        /// </summary>       
        private ManVo GetRefParam()
        {
            var ret = new ManVo();

            try
            {
                ret.FM_DT = (StartDt).ToString("yyyy-MM-dd");
                ret.TO_DT = (EndDt).ToString("yyyy-MM-dd");
                ret.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
            }
            catch (Exception eLog)
            {
                // 에러메시지
                ShowWinUIMessageOK(eLog.Message, MessageBoxImage.Error);
                // default - > 기본값 NULL
                ret = default(ManVo);
            }

            return ret;
        }


        /// <summary>
        /// Json를 통하여 서버 정보를 가져옵니다.
        /// </summary>        
        /// <param name="Path"></param>
        /// <param name="Obj"></param>
        private async Task<List<T>> PostJsonData<T>(string Path, object Obj)
        {
            // default - > 기본값 NULL
            var ret = default(List<T>);

            try
            {
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync(Path
                                                                                                  , new StringContent(JsonConvert.SerializeObject(Obj), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        ret = JsonConvert.DeserializeObject<IEnumerable<T>>(await response.Content.ReadAsStringAsync()).Cast<T>().ToList();
                    }
                }

            }
            catch(Exception eLog)
            {
                // 에러메시지
                ShowWinUIMessageOK(eLog.Message, MessageBoxImage.Error);
            }

            return ret;
        }


        /// <summary>
        /// OK 버튼 메시지를 표시합니다.
        /// </summary>        
        /// <param name="Msg"></param>
        /// <param name="Img"></param>
        private void ShowWinUIMessageOK(string Msg, MessageBoxImage Img)
        {
            WinUIMessageBox.Show(Msg, _title, MessageBoxButton.OK, Img);
        }


        #endregion



        #region MVVM 데이터 바이딩

        /// <summary>
        /// 데이터 바이딩
        /// </summary>        
        private IList<ManVo> _selectMstList = new List<ManVo>();

        public IList<ManVo> SelectMstList
        {
            get { return _selectMstList; }
            set { SetProperty(ref _selectMstList, value, () => SelectMstList); }
        }

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

        private string _M_SEARCH_TEXT = string.Empty;
        public string M_SEARCH_TEXT
        {
            get { return _M_SEARCH_TEXT; }
            set { SetProperty(ref _M_SEARCH_TEXT, value, () => M_SEARCH_TEXT); }
        } 
        #endregion

    }
}
