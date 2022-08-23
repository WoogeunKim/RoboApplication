using AquilaErpWpfApp3.Util;
using AquilaErpWpfApp3.View.M.Dialog;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Code;
using ModelsLibrary.Man;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace AquilaErpWpfApp3.ViewModel
{
    public class M6684ViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private IList<ManVo> selectedMenuViewList;
        private IList<ManVo> selectedDocViewList;
        private M6684DocDialog docDialog;


        private System.Windows.Forms.FolderBrowserDialog downloaddialog;
        private FileStream fs;

        public M6684ViewModel()
        {

            //Refresh();
            SYSTEM_CODE_VO("L-001");
        }


      

        [Command]
        public async void Refresh(string _ITM_CD = null)
        {
            try
            {
                //if (SelectedN1stItem == null)
                //{
                //    return;
                //}

                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6684", new StringContent(JsonConvert.SerializeObject(new ManVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD, ITM_GRP_CLSS_CD = M_SL_AREA_NM.CLSS_CD }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        SelectedMenuViewList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
                        if (SelectedMenuViewList.Count > 0)
                        {
                            //SelectedMenuItem = SelectedMenuViewList[0];
                            this.IS_MENU = true;
                            if (string.IsNullOrEmpty(_ITM_CD))
                            {
                                SelectedMenuItem = SelectedMenuViewList[0];
                            }
                            else
                            {
                                SelectedMenuItem = SelectedMenuViewList.Where(x => (x.ITM_CD).Equals(_ITM_CD)).LastOrDefault<ManVo>();
                            }
                        }
                        else
                        {
                            SelectedMenuItem = null;
                            this.IS_MENU = false;
                        }
                    }
                }
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[에러]문서관리", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }




        bool _isMenu;
        public bool IS_MENU
        {
            get
            {
                return _isMenu;
            }
            set
            {
                if (value != null)
                {
                    SetProperty(ref _isMenu, value, () => IS_MENU);
                }
            }
        }

        bool _isDocMenu;
        public bool IS_DOC_MENU
        {
            get
            {
                return _isDocMenu;
            }
            set
            {
                if (value != null)
                {
                    SetProperty(ref _isDocMenu, value, () => IS_DOC_MENU);
                }
            }
        }


        // selectedDocViewList
        public IList<ManVo> SelectedMenuViewList
        {
            get { return selectedMenuViewList; }
            set { SetProperty(ref selectedMenuViewList, value, () => SelectedMenuViewList); }
        }

        ManVo _selectMenuItem;
        public ManVo SelectedMenuItem
        {
            get
            {
                return _selectMenuItem;
            }
            set
            {
                if (value != null)
                {
                    SetProperty(ref _selectMenuItem, value, () => SelectedMenuItem, SearchDetailItem);
                    //SearchDetailItem();
                }
            }
        }

        public void SearchDetailItem()
        {
            DocRefresh();
        }


        [Command]
        public async void DocRefresh(string _DWG_NM = null)
        {
            if (SelectedMenuItem == null)
            {
                return;
            }

            //
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6684/doc", new StringContent(JsonConvert.SerializeObject(new ManVo() { ITM_CD = SelectedMenuItem.ITM_CD, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    SelectedDocViewList = JsonConvert.DeserializeObject<IEnumerable<ManVo>>(await response.Content.ReadAsStringAsync()).Cast<ManVo>().ToList();
                    if (SelectedDocViewList.Count > 0)
                    {
                        this.IS_DOC_MENU = true;

                        if (string.IsNullOrEmpty(_DWG_NM))
                        {
                            SelectedDocItem = SelectedDocViewList[0];
                        }
                        else
                        {
                            SelectedDocItem = SelectedDocViewList.Where(x => (x.DWG_NM).Equals(_DWG_NM)).LastOrDefault<ManVo>();
                        }
                    }
                    else
                    {
                        SelectedDocItem = null;
                        this.IS_DOC_MENU = false;
                    }
                }
            }
        }



        //도면 이력관리
        public IList<ManVo> SelectedDocViewList
        {
            get { return selectedDocViewList; }
            set { SetProperty(ref selectedDocViewList, value, () => SelectedDocViewList); }
        }

        ManVo _selectedDocItem;
        public ManVo SelectedDocItem
        {
            get
            {
                return _selectedDocItem;
            }
            set
            {
                if (value != null)
                {
                    SetProperty(ref _selectedDocItem, value, () => SelectedDocItem);
                    //SearchDetailItem();
                }
            }
        }



        // 문서 추가
        [Command]
        public void NewDocContact()
        {
            if (SelectedMenuItem == null)
            {
                return;
            }

            docDialog = new M6684DocDialog(new ManVo() { ITM_CD = SelectedMenuItem.ITM_CD, ITM_NM = SelectedMenuItem.ITM_NM });
            docDialog.Title = "문서관리 - 추가 [" + SelectedMenuItem.ITM_CD + " / " + SelectedMenuItem.ITM_NM + "]";
            docDialog.Owner = Application.Current.MainWindow;
            docDialog.BorderEffect = BorderEffect.Default;
            docDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            docDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            bool isDialog = (bool)docDialog.ShowDialog();
            if (isDialog)
            {
                Refresh(docDialog.updateDao.ITM_CD);
              
            }
        }




        // 문서 수정
        [Command]
        public async void EditDocContact()
        {
            if (SelectedDocItem != null)
            {
                MessageBoxResult result = WinUIMessageBox.Show("[" + SelectedDocItem.RN + "/" + SelectedDocItem.DWG_NM + "]" + " 정말로 삭제 하시겠습니까?", "문서관리 - 수정", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    SelectedDocItem.DELT_FLG = "Y";
                    SelectedDocItem.UPD_USR_ID = SystemProperties.USER;
                    SelectedDocItem.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;

                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6684/doc/u", new StringContent(JsonConvert.SerializeObject(SelectedDocItem), System.Text.Encoding.UTF8, "application/json")))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            int _Num = 0;
                            string resultMsg = await response.Content.ReadAsStringAsync();
                            if (int.TryParse(resultMsg, out _Num) == false)
                            {
                                //실패
                                WinUIMessageBox.Show(resultMsg, "문서관리 - 수정", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                            DocRefresh();

                            //성공
                            WinUIMessageBox.Show("삭제가 완료되었습니다.", "문서관리 - 수정", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                        }
                    }

                }
            }

        }





        // 저장된 파일을 내 컴에 불러오기
        [Command]
        public void DownloadRvisContact(string parameter)
        {
            if (parameter.Equals("D"))
            {
                ManVo dao = SelectedDocItem;
                if (dao != null)
                {
                    downloaddialog = new System.Windows.Forms.FolderBrowserDialog();
                    downloaddialog.ShowNewFolderButton = true;
                    downloaddialog.Description = "[파일 : " + dao.DWG_FILE_NM + "] 저장 폴더를 선택 해 주세요.";
                    downloaddialog.RootFolder = System.Environment.SpecialFolder.Desktop;
                    downloaddialog.ShowDialog();
                    if (!string.IsNullOrEmpty(downloaddialog.SelectedPath))
                    {
                        if (!string.IsNullOrEmpty(dao.DWG_FILE_NM))
                        {
                            int ArraySize = dao.DWG_FILE.Length;
                            this.fs = new FileStream(downloaddialog.SelectedPath + "/" + dao.DWG_FILE_NM, FileMode.OpenOrCreate, FileAccess.Write);
                            this.fs.Write(dao.DWG_FILE, 0, ArraySize);
                            this.fs.Close();
                        }
                        System.Diagnostics.Process.Start(downloaddialog.SelectedPath);
                    }
                }
                return;
            }
            //else if (parameter.Equals("W"))
            //{
            //    ManVo dao = SelectedWrkItem;
            //    if (dao != null)
            //    {
            //        downloaddialog = new System.Windows.Forms.FolderBrowserDialog();
            //        downloaddialog.ShowNewFolderButton = true;
            //        downloaddialog.Description = "[파일 : " + dao.WRK_DOC_FILE_NM + "] 저장 폴더를 선택 해 주세요.";
            //        downloaddialog.RootFolder = System.Environment.SpecialFolder.Desktop;
            //        downloaddialog.ShowDialog();
            //        if (!string.IsNullOrEmpty(downloaddialog.SelectedPath))
            //        {
            //            if (!string.IsNullOrEmpty(dao.WRK_DOC_FILE_NM))
            //            {
            //                int ArraySize = dao.WRK_DOC_FILE.Length;
            //                this.fs = new FileStream(downloaddialog.SelectedPath + "/" + dao.WRK_DOC_FILE_NM, FileMode.OpenOrCreate, FileAccess.Write);
            //                this.fs.Write(dao.WRK_DOC_FILE, 0, ArraySize);
            //                this.fs.Close();
            //            }
            //            System.Diagnostics.Process.Start(downloaddialog.SelectedPath);
            //        }
            //    }
            //    return;
            //}
        }


        // ??
        public void ImageRvisContact(string parameter)
        {
            //if (parameter.Equals("D"))
            //{
            //MakeVo dao = SelectedDwgRvisItem;
            //if (dao != null)
            //{
            //    imgDetailDialog = new P4413ImageDialog(dao);
            //    imgDetailDialog.Title = "파일 - 이미지 [" + dao.EQ_FILE_NM + "]";
            //    imgDetailDialog.Owner = Application.Current.MainWindow;
            //    imgDetailDialog.BorderEffect = BorderEffect.Default;
            //    //masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            //    //masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            //    bool isDialog = (bool)imgDetailDialog.ShowDialog();
            //    if (isDialog)
            //    {

            //    }
            //}
            //return;
            //}
            //else if (parameter.Equals("W"))
            //{
            //    MakeVo dao = SelectedWrkRvisItem;
            //    if (dao != null)
            //    {
            //        dao.DWG_FILE = dao.WRK_DOC_FILE;
            //        imgDetailDialog = new P4411ImageDialog(dao);
            //        imgDetailDialog.Title = "파일 - 이미지 [" + dao.WRK_DOC_NM + "]";
            //        imgDetailDialog.Owner = Application.Current.MainWindow;
            //        imgDetailDialog.BorderEffect = BorderEffect.Default;
            //        //masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            //        //masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            //        bool isDialog = (bool)imgDetailDialog.ShowDialog();
            //        if (isDialog)
            //        {

            //        }
            //    }
            //    return;
            //}
        }



        // 품목마스터 코드 불러오기

        private IList<SystemCodeVo> _AreaCd = new List<SystemCodeVo>();
        public IList<SystemCodeVo> AreaList
        {
            get { return _AreaCd; }
            set { SetProperty(ref _AreaCd, value, () => AreaList); }
        }

        //사업장
        private SystemCodeVo _M_SL_AREA_NM;
        public SystemCodeVo M_SL_AREA_NM
        {
            get { return _M_SL_AREA_NM; }
            set { SetProperty(ref _M_SL_AREA_NM, value, () => M_SL_AREA_NM); }
        }


        private async void SYSTEM_CODE_VO(string _CLSS_TP_CD)
        {

            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + _CLSS_TP_CD))
            {
                if (response.IsSuccessStatusCode)
                {
                    AreaList = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                    if (AreaList.Count > 0)
                    {
                        M_SL_AREA_NM = AreaList[0];
                    }

                    //비동기 
                    //Refresh();
                }
            }

        }


    }
}
