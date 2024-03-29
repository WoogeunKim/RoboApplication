﻿using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Man;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using AquilaErpWpfApp3.Util;
using System;
using ModelsLibrary.Code;
using System.Web;
using System.Net;
using System.IO;
using System.Text;
using System.Net.Http.Headers;
using AquilaErpWpfApp3.View.M.Dialog;
using AquilaErpWpfApp3.M.View.Dialog;
using System.Threading.Tasks;

namespace AquilaErpWpfApp3.ViewModel
{
    public sealed class M66310ViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private string _title = "투입자재지시관리";

        private M66311MasterDialog masterDialog;


        public M66310ViewModel()
        {
            B_UPDATE = false;
            B_INSERT = false;
        }

        [Command]
        public async void BarlistInput()
        {
            try
            {
                if (SelectedDtlItem == null) return;

                MessageBoxResult result = WinUIMessageBox.Show("순번 " + SelectedDtlItem.RN.ToString() + " 을(를) 정말로 투입 하시겠습니까?", "투입자재", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m66310/dtl/i", new StringContent(JsonConvert.SerializeObject(this.SelectedDtlItem), System.Text.Encoding.UTF8, "application/json")))
                    {
                        int _Num = 0;
                        string resultMsg = await response.Content.ReadAsStringAsync();
                        if (int.TryParse(resultMsg, out _Num) == false)
                        {
                            //실패
                            WinUIMessageBox.Show(resultMsg, _title, MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        WinUIMessageBox.Show("완료 되었습니다", _title, MessageBoxButton.OK, MessageBoxImage.Information);
                        DtlRefresh(OpmzNo);
                    }
                }
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, _title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }



        [Command]
        public void Refresh()
        {
            try
            {
                masterDialog = new M66311MasterDialog();
                masterDialog.Title = _title + " - 선택";
                masterDialog.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                masterDialog.Owner = Application.Current.MainWindow;
                masterDialog.BorderEffect = BorderEffect.Default;
                masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
                masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
                bool isDialog = (bool)masterDialog.ShowDialog();
                if (isDialog)
                {
                    OpmzNo = masterDialog.resultDomain.OPMZ_NO;
                    DtlRefresh(OpmzNo);
                }
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, _title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }


        [Command]
        public void DtlRefresh()
        {
            if (string.IsNullOrEmpty(OpmzNo + "")) return;

            DtlRefresh(OpmzNo);
        }


        private async void DtlRefresh(string opmgno)
        {
            try
            {
                if (opmgno == null) return;

                MstListRefresh(opmgno);
                SummaryListRefresh(opmgno);
                DtlListRefresh(opmgno);
                B_UPDATE = true;
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }


        private async void MstListRefresh(string opmgno)
        {
            try
            {
                this.SelectMstList = await PostHttpSelect<ManVo>("m66310", new ManVo() { OPMZ_NO = opmgno, CHNL_CD = SystemProperties.USER_VO.CHNL_CD, CLZ_FLG = BL_CLZ_FLG == true ? "Y" : "N" });
                this.SelectedMstItem = null;
            }
            catch
            {

            }
        }
        private async void SummaryListRefresh(string opmgno)
        {
            try
            {
                this.SummaryTableList = await PostHttpSelect<ManVo>("m66310/dtl/summary", new ManVo() { OPMZ_NO = opmgno, CLZ_FLG = BL_CLZ_FLG == true ? "Y" : "N" });
            }
            catch
            {

            }
        }

        private async void DtlListRefresh(string opmgno)
        {
            try
            {
                if (DXSplashScreen.IsActive == false) DXSplashScreen.Show<ProgressWindow>();

                this.SelectDtlList = await PostHttpSelect<ManVo>("m66310/dtl", new ManVo() { OPMZ_NO = opmgno, CHNL_CD = SystemProperties.USER_VO.CHNL_CD, CLZ_FLG = BL_CLZ_FLG == true ? "Y" : "N" });
                this.SelectedDtlItem = null;

                if (DXSplashScreen.IsActive == true) DXSplashScreen.Close();
            }
            catch
            {
                if (DXSplashScreen.IsActive == true) DXSplashScreen.Close();
            }
        }

        private async Task<List<T>> PostHttpSelect<T>(string Path, object Obj)
        {
            var ret = new List<T>();

            try
            {
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync( Path
                                                                                                   , new StringContent(JsonConvert.SerializeObject(Obj), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        ret = JsonConvert.DeserializeObject<IEnumerable<T>>(await response.Content.ReadAsStringAsync()).Cast<T>().ToList();
                    }
                }
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
            }

            return ret;
        } 



        private string _opmzNo = default(string);
        public string OpmzNo
        {
            get { return _opmzNo; }
            set { SetProperty(ref _opmzNo, value, () => OpmzNo); }
        }

        private IList<ManVo> _selectMstList;
        public IList<ManVo> SelectMstList
        {
            get { return _selectMstList; }
            set { SetProperty(ref _selectMstList, value, () => SelectMstList); }
        }
        private ManVo _selectedMstItem;
        public ManVo SelectedMstItem
        {
            get { return _selectedMstItem; }
            set { SetProperty(ref _selectedMstItem, value, () => SelectedMstItem); }
        }

        private IList<ManVo> _selectDtlList;
        public IList<ManVo> SelectDtlList
        {
            get { return _selectDtlList; }
            set { SetProperty(ref _selectDtlList, value, () => SelectDtlList); }
        }

        private ManVo _selectedDtlItem;
        public ManVo SelectedDtlItem
        {
            get { return _selectedDtlItem; }
            set { SetProperty(ref _selectedDtlItem, value, () => SelectedDtlItem, InputValue); }
        }


        private IList<ManVo> _summaryTableList;
        public IList<ManVo> SummaryTableList
        {
            get { return _summaryTableList; }
            set { SetProperty(ref _summaryTableList, value, () => SummaryTableList); }
        }

        //private IList<SystemCodeVo> _extrStsList;
        //public IList<SystemCodeVo> ExtrStsList
        //{
        //    get { return _extrStsList; }
        //    set { SetProperty(ref _extrStsList, value, () => ExtrStsList); }
        //}

        //private SystemCodeVo _M_EXTR_STS_NM;
        //public SystemCodeVo M_EXTR_STS_NM
        //{
        //    get { return _M_EXTR_STS_NM; }
        //    set { SetProperty(ref _M_EXTR_STS_NM, value, () => M_EXTR_STS_NM); }
        //}

        bool _B_UPDATE;
        public bool B_UPDATE
        {
            get { return _B_UPDATE; }
            set { SetProperty(ref _B_UPDATE, value, () => B_UPDATE); }
        }

        bool _B_INSERT;
        public bool B_INSERT
        {
            get { return _B_INSERT; }
            set { SetProperty(ref _B_INSERT, value, () => B_INSERT); }
        }

        private void InputValue()
        {
            B_INSERT = SelectedDtlItem != null ? true : false;
        }

        bool blClzFlg = false;
        public bool BL_CLZ_FLG
        {
            get { return blClzFlg; }
            set { SetProperty(ref blClzFlg, value, () => BL_CLZ_FLG, DtlRefresh); }
        }

    }
}