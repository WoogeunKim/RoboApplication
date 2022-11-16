using AquilaErpWpfApp3.Util;
using AquilaErpWpfApp3.View.PUR.Dialog;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.Charts;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Spreadsheet;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Code;
using ModelsLibrary.Pur;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace AquilaErpWpfApp3.ViewModel
{
    public sealed class P4402ViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private P4402ExcelDialog excelDialog;
        private System.IO.Stream streamFile;

        PurVo fileVo = new PurVo();

        public P4402ViewModel()
        {
            //SYSTEM_CODE_VO();
            //Refresh();
        }

        [Command]
        public async void Refresh(int ordNo = 0)
        {
            try
            {
                DXSplashScreen.Show<ProgressWindow>();

                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("p4402/mst", new StringContent(JsonConvert.SerializeObject(new PurVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectMstList = JsonConvert.DeserializeObject<IEnumerable<PurVo>>(await response.Content.ReadAsStringAsync()).Cast<PurVo>().ToList();
                    }

                    if (ordNo.Equals(0))
                    {
                        if (SelectMstList.Count >= 1)
                        {
                            SelectedMstItem = SelectMstList[0];
                        }
                    }
                    else
                    {
                        if (SelectMstList.Count >= 1)
                        {
                            SelectedMstItem = SelectMstList.Where(x => x.RN.Equals(ordNo)).LastOrDefault<PurVo>();
                        }
                    }

                    DXSplashScreen.Close();
                }
            }
            catch (System.Exception eLog)
            {
                if (DXSplashScreen.IsActive == true)
                {
                    DXSplashScreen.Close();
                }
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]원자재 원가표", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }

        [Command]
        public async void NewContact()
        {
            try
            {
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("p4402/file", new StringContent(JsonConvert.SerializeObject(new PurVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        PurVo vo = new PurVo();
                        vo = JsonConvert.DeserializeObject<PurVo>(await response.Content.ReadAsStringAsync());

                        if (vo != null)
                        {
                            excelDialog = new P4402ExcelDialog(vo);
                            excelDialog.Title = "원자재 원가표 - 추가";
                            excelDialog.Owner = Application.Current.MainWindow;
                            excelDialog.BorderEffect = BorderEffect.Default;
                            excelDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
                            excelDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
                            bool isDialog = (bool)excelDialog.ShowDialog();
                            if (isDialog)
                            {
                                Refresh();
                            }
                        }

                    }
                }

            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]원자재 원가표", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }

        [Command]
        public async void UpdateContact()
        {
            try
            {
                excelDialog = new P4402ExcelDialog(fileVo);
                excelDialog.Title = "원자재 원가표 - 수정";
                excelDialog.Owner = Application.Current.MainWindow;
                excelDialog.BorderEffect = BorderEffect.Default;
                excelDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
                excelDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
                bool isDialog = (bool)excelDialog.ShowDialog();
                if (isDialog)
                {
                    Refresh((int)SelectedMstItem.RN);
                }
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]원자재 원가표", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }

        //public async void SYSTEM_CODE_VO()
        //{
        //    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("S131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "L-011"))
        //    {
        //        if (response.IsSuccessStatusCode)
        //        {
        //            this.CO_TP_List = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
        //            // 업체 구분 리스트 첫 번째 항목 기본값 설정
        //            this.CO_TP_OBJ = this.CO_TP_List[0];
        //        }
        //    }
        //}

        public async void DetailView()
        {
            try
            {
                if(this.SelectedMstItem == null)
                {
                    isM_UPDATE = false;
                    return;
                }

                isM_UPDATE = true;

                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("p4402/dtl", new StringContent(JsonConvert.SerializeObject(this.SelectedMstItem), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        fileVo = this.selectmstItm;
                        fileVo.PRC_FILE = JsonConvert.DeserializeObject<PurVo>(await response.Content.ReadAsStringAsync()).PRC_FILE;

                        if (fileVo != null)
                        {
                            this.streamFile = new System.IO.MemoryStream(fileVo.PRC_FILE);
                            SourceStream = new SpreadsheetDocumentSource(streamFile, DevExpress.Spreadsheet.DocumentFormat.Xlsx);
                        }

                        //this.spreadsheetControl1.ActiveViewZoom = 85;
                    }
                }
            }
            catch(System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + "원자재 원가표", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }

        }
        // 업체 구분 리스트
        private IList<SystemCodeVo> cotplst = new List<SystemCodeVo>();
        public IList<SystemCodeVo> CO_TP_List
        {
            get { return cotplst; }
            set { SetProperty(ref cotplst, value, () => CO_TP_List); }
        }
        // 업체 구분 하나 선택
        private SystemCodeVo cotpobj = new SystemCodeVo();
        public SystemCodeVo CO_TP_OBJ
        {
            get { return cotpobj; }
            set { SetProperty(ref cotpobj, value, () => CO_TP_OBJ); }
        }


        //마스터 리스트 조회
        private IList<PurVo> selectmstlist = new List<PurVo>();
        public IList<PurVo> SelectMstList
        {
            get { return selectmstlist; }
            set { SetProperty(ref selectmstlist, value, () => SelectMstList); }
        }
        // 마스터 하나 선택
        private PurVo selectmstItm = new PurVo();
        public PurVo SelectedMstItem
        {
            get { return selectmstItm; }
            set { SetProperty(ref selectmstItm, value, () => SelectedMstItem); DetailView(); }
        }

        // 
        private bool ismupdate = false;
        public bool isM_UPDATE
        {
            get { return ismupdate; }
            set { SetProperty(ref ismupdate, value, () => isM_UPDATE); }
        }

        SpreadsheetDocumentSource _sourceStream;
        public SpreadsheetDocumentSource SourceStream
        {
            get
            {
                return _sourceStream;
            }
            set
            {
                if (value != null)
                {
                    SetProperty(ref _sourceStream, value, () => SourceStream);
                }
            }
        }




    }
}
