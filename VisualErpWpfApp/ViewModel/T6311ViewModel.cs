using AquilaErpWpfApp3.Util;
using AquilaErpWpfApp3.View.TEC.Dialog;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Tec;
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
    public sealed class T6311ViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private string _title = "출하검사";

        private T6311MasterDialog masterDialog;

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

        public T6311ViewModel()
        {
            StartDt = System.DateTime.Now;
            EndDt = System.DateTime.Now;


            //SYSTEM_CODE_VO();
        }



        [Command]
        public async void Refresh(string tempKey = null)
        {
            try
            {


                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("t6311/mst", new StringContent(JsonConvert.SerializeObject(new TecVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD, FM_DT = (StartDt).ToString("yyyy-MM-dd"), TO_DT = (EndDt).ToString("yyyy-MM-dd")  }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        Title = "[일자]" + (StartDt).ToString("yyyy-MM-dd") + "~" + (EndDt).ToString("yyyy-MM-dd");

                        this.SelectMstList = JsonConvert.DeserializeObject<IEnumerable<TecVo>>(await response.Content.ReadAsStringAsync()).Cast<TecVo>().ToList();


                        if(SelectMstList.Count >= 1)
                        {
                            isM_UPDATE = true;

                            if (string.IsNullOrEmpty(tempKey))
                            {
                                SelectedMstItem = SelectMstList[0];
                            }
                            else
                            {
                                SelectedMstItem = SelectMstList.Where(x => (x.SL_RLSE_NO + x.SL_RLSE_SEQ).Equals(tempKey)).LastOrDefault<TecVo>();
                            }

                        }
                        else
                        {
                            isM_UPDATE = false;
                        }
                    }
                }
            }
            catch (Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }


        [Command]
        public async void EditContact()
        {
            if (SelectedMstItem == null)
            {
                return;
            }

            // 포커스 작업을위해 tempKey 생성
            string tempKey = SelectedMstItem.SL_RLSE_NO + SelectedMstItem.SL_RLSE_SEQ;


            masterDialog = new T6311MasterDialog(SelectedMstItem);
            masterDialog.Owner = Application.Current.MainWindow;
            masterDialog.BorderEffect = BorderEffect.Default;
            masterDialog.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            bool isDialog = (bool)masterDialog.ShowDialog();
            
            if (isDialog)
            {
                Refresh(tempKey);  // tempKey 위치에 포커스작업가도록 인자를 넣은채로 Refresh호출
            }
        }


        private IList<TecVo> _selectMstList = new List<TecVo>();
        public IList<TecVo> SelectMstList
        {
            get { return _selectMstList; }
            set { SetProperty(ref _selectMstList, value, () => SelectMstList); }
        }


        private TecVo _selectedMstItem;
        public TecVo SelectedMstItem
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


        private string _M_SEARCH_TEXT = string.Empty;
        public string M_SEARCH_TEXT
        {
            get { return _M_SEARCH_TEXT; }
            set { SetProperty(ref _M_SEARCH_TEXT, value, () => M_SEARCH_TEXT); }
        }



        private bool? _isM_UPDATE = false;
        public bool? isM_UPDATE
        {
            get { return _isM_UPDATE; }
            set { SetProperty(ref _isM_UPDATE, value, () => isM_UPDATE); }
        }


        private string _Title = string.Empty;
        public string Title
        {
            get { return _Title; }
            set { SetProperty(ref _Title, value, () => Title); }
        }




    }

}

