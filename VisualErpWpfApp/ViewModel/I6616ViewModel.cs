using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Inv;
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
    public sealed class I6616ViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private IList<InvVo> selectedMstList = new List<InvVo>();
        private IList<InvVo> selectedDtlList = new List<InvVo>();


        public I6616ViewModel() 
        {
            
          StartDt = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy-MM-01")).AddMonths(-12);
          EndDt = System.DateTime.Now;

          this._M_GBN_FLG = "진행";
       
          }

          [Command]
          public async void Refresh()
          {

              try
              {
                InvVo _param = new InvVo();
                //_param.AREA_CD = (string.IsNullOrEmpty(TXT_SL_AREA_NM) ? null : _AreaMap[TXT_SL_AREA_NM]);
                _param.FM_DT = (StartDt).ToString("yyyyMMdd");
                _param.TO_DT = (EndDt).ToString("yyyyMMdd");
                _param.CLZ_FLG = (M_GBN_FLG.Equals("완료") ? "Y" : M_GBN_FLG.Equals("진행") ? "N" : "");
                _param.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;

                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("i6616/mst", new StringContent(JsonConvert.SerializeObject(_param), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectMstList = JsonConvert.DeserializeObject<IEnumerable<InvVo>>(await response.Content.ReadAsStringAsync()).Cast<InvVo>().ToList();
                    }

                    Title = "[일자]" + (StartDt).ToString("yyyy-MM-dd") + "~" + (EndDt).ToString("yyyy-MM-dd") + ", [구분]" + M_GBN_FLG;

                    if (SelectMstList.Count >= 1)
                    {
                        isM_UPDATE = true;
                        isM_DELETE = true;

                        SelectedMstItem = SelectMstList[0];
                    }
                    else
                    {
                        isM_UPDATE = false;
                        isM_DELETE = false;

                    }
                    //DXSplashScreen.Close();
                }
              }
              catch (System.Exception eLog)
              {
                  //DXSplashScreen.Close();
                  WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]발주잔량", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                  return;
              }


          }

          #region 계약기간 From To

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
          
          #endregion


          #region 마감
          private string _M_GBN_FLG = string.Empty;
          public string M_GBN_FLG
          {
              get { return _M_GBN_FLG; }
              set { SetProperty(ref _M_GBN_FLG, value, () => M_GBN_FLG); }
          } 
          #endregion


          #region 기타
          //private string _M_SEARCH_TEXT = string.Empty;
          //public string M_SEARCH_TEXT
          //{
          //    get { return _M_SEARCH_TEXT; }
          //    set { SetProperty(ref _M_SEARCH_TEXT, value, () => M_SEARCH_TEXT); }
          //}

          private string _IO_CD;
          public string IO_CD
          {
              get { return _IO_CD; }
              set { SetProperty(ref _IO_CD, value, () => IO_CD); }
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

          string _Title = string.Empty;
          public string Title
          {
              get { return _Title; }
              set { SetProperty(ref _Title, value, () => Title); }
          }
          #endregion


          #region 마스터 그리드
          public IList<InvVo> SelectMstList
          {
              get { return selectedMstList; }
              set { SetProperty(ref selectedMstList, value, () => SelectMstList); }
          }

          InvVo _selectedMstItem;
          public InvVo SelectedMstItem
          {
              get
              {
                  return _selectedMstItem;
              }
              set
              {
                  if (value != null)
                  {
                      SetProperty(ref _selectedMstItem, value, () => SelectedMstItem, OnSelectedMasterItemChanged);
                  }
              }
          }
        #endregion

        async void OnSelectedMasterItemChanged()
        {
              try
              {
                    if (this.SelectedMstItem == null) { return; }
                    if (this.SelectedMstItem.CLZ_FLG == "")
                    {
                        this.SelectedMstItem.CLZ_FLG = null;
                    }


                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("i6616/dtl", new StringContent(JsonConvert.SerializeObject(this.SelectedMstItem), System.Text.Encoding.UTF8, "application/json")))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            this.SelectDtlList = JsonConvert.DeserializeObject<IEnumerable<InvVo>>(await response.Content.ReadAsStringAsync()).Cast<InvVo>().ToList();
                        }

                        //
                        if (SelectDtlList.Count >= 1)
                        {
                            //isD_DELETE = true;
                            //isD_UPDATE = true;

                            SelectedDtlItem = SelectDtlList[0];
                        }
                        else
                        {
                            //isD_UPDATE = false;
                            //isD_DELETE = false;
                        }
                    }
              }
              catch (System.Exception eLog)
              {
                  WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]발주잔량", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.None, MessageBoxOptions.None);
                  return;
              }
          }


          #region 디테일 그리드
          public IList<InvVo> SelectDtlList
          {
              get { return selectedDtlList; }
              set { SetProperty(ref selectedDtlList, value, () => SelectDtlList); }
          }

          InvVo _selectedDtlItem;
          public InvVo SelectedDtlItem
          {
              get
              {
                  return _selectedDtlItem;
              }
              set
              {
                  if (value != null)
                  {
                      SetProperty(ref _selectedDtlItem, value, () => SelectedDtlItem);
                  }
              }
          }
          #endregion

    }

}
