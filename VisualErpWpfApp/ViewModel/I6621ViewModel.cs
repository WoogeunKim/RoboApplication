using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Inv;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Media;
using AquilaErpWpfApp3.Util;

namespace AquilaErpWpfApp3.ViewModel
{
    public sealed class I6621ViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private string title = "품목그룹별 재고장";

        private IList<InvVo> selectedMasterViewList;
        private IList<InvVo> selectedDetailViewList;

        public I6621ViewModel()
        {
            Refresh();
        }

        [Command]
        public async void Refresh()
        {
            try
            {
                SelectedMasterItem = null;
                SelectedMasterViewList = null;

                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("i6621/mst", new StringContent(JsonConvert.SerializeObject(new InvVo() { CRE_USR_ID = "", CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectedMasterViewList = JsonConvert.DeserializeObject<IEnumerable<InvVo>>(await response.Content.ReadAsStringAsync()).Cast<InvVo>().ToList();
                    }
                    //SelectedMasterViewList = itemClient.SelectCodeItemGroupList(new SystemCodeVo() { CRE_USR_ID = "" , CHNL_CD = SystemProperties.USER_VO.CHNL_CD});
                    if (SelectedMasterViewList.Count > 0)
                    {
                        SelectedMasterItem = SelectedMasterViewList[0];
                        SelectedMasterItem = null;
                    }
                    else
                    {
                        SelectedMasterItem = null;
                        SelectedMasterViewList = null;
                    }
                    SelectedDetailItem = null;
                    SelectedDetailViewList = null;
                }


                //using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("i6621/dtl", new StringContent(JsonConvert.SerializeObject(new InvVo() { CRE_USR_ID = "", CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                //{
                //    if (response.IsSuccessStatusCode)
                //    {
                //        this.SelectedDetailViewList = JsonConvert.DeserializeObject<IEnumerable<InvVo>>(await response.Content.ReadAsStringAsync()).Cast<InvVo>().ToList();

                //        //SelectedMasterViewList = itemClient.SelectCodeItemGroupList(new SystemCodeVo() { CRE_USR_ID = "" , CHNL_CD = SystemProperties.USER_VO.CHNL_CD});
                //        if (SelectedDetailViewList.Count > 0)
                //        {
                //            //SelectedDetailItem = SelectedDetailViewList[0];
                //            SelectedDetailItem = null;
                //        }
                //        else
                //        {
                //            SelectedDetailItem = null;
                //            SelectedDetailViewList = null;
                //        }
                //    }
                //}


            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }

            


        }
        public async void DtListRefresh()
        {
            try
            {
                if (SelectedMasterItem == null) return;

                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("i6621/dtl", new StringContent(JsonConvert.SerializeObject(SelectedMasterItem), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectedDetailViewList = JsonConvert.DeserializeObject<IEnumerable<InvVo>>(await response.Content.ReadAsStringAsync()).Cast<InvVo>().ToList();


                        if (SelectedDetailViewList.Count > 0)
                        {
                            //SelectedDetailItem = SelectedDetailViewList[0];
                            SelectedDetailItem = null;
                        }
                        else
                        {
                            SelectedDetailItem = null;
                            SelectedDetailViewList = null;
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

        #region Function (Master)

        public IList<InvVo> SelectedMasterViewList
        {
            get { return selectedMasterViewList; }
            private set { SetProperty(ref selectedMasterViewList, value, () => SelectedMasterViewList); }
        }

        InvVo _selectedMasterItem;
        public InvVo SelectedMasterItem
        {
            get
            {
                return _selectedMasterItem;
            }
            set
            {
                if (value != null)
                {
                    SetProperty(ref _selectedMasterItem, value, () => SelectedMasterItem); DtListRefresh();
                }
            }
        }
        #endregion

        #region Function (Detail)
        public IList<InvVo> SelectedDetailViewList
        {
            get { return selectedDetailViewList; }
            private set { SetProperty(ref selectedDetailViewList, value, () => SelectedDetailViewList); }
        }

        InvVo _selectedDetailItem;
        public InvVo SelectedDetailItem
        {
            get
            {
                return _selectedDetailItem;
            }
            set
            {
                if (value != null)
                {
                    SetProperty(ref _selectedDetailItem, value, () => SelectedDetailItem);
                }
            }
        }

        #endregion

        private string _M_SEARCH_TEXT = string.Empty;
        public string M_SEARCH_TEXT
        {
            get { return _M_SEARCH_TEXT; }
            set { SetProperty(ref _M_SEARCH_TEXT, value, () => M_SEARCH_TEXT); }
        }

    }


}
