using AquilaErpWpfApp3.Util;
using AquilaErpWpfApp3.View.PUR.Dialog;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Dialogs;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Auth;
using ModelsLibrary.Code;
using ModelsLibrary.Pur;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace AquilaErpWpfApp3.ViewModel
{
    public sealed class P441106ViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private string _title = "고객발주관리";

        private P441106MasterDialog masterDialog;
        private P441106DetailDialog detailDialog;



        public P441106ViewModel()
        {
            StartDt = Convert.ToDateTime(System.DateTime.Now.ToString("yyyy-MM-01"));
            EndDt = System.DateTime.Now;

            //사업장
            SYSTEM_CODE_VO();
        }



        [Command]
        public async void Refresh(string _PUR_ORD_NO = null)
        {
            try
            {
                if (this.M_SL_AREA_NM == null) return;

                IList<PurVo> resultList = await PostHttpSelect<PurVo>("p441106/mst", new PurVo() { FM_DT = (StartDt).ToString("yyyy-MM-dd"), TO_DT = (EndDt).ToString("yyyy-MM-dd"), AREA_CD = M_SL_AREA_NM.CLSS_CD, CHNL_CD = SystemProperties.USER_VO.CHNL_CD, PUR_ITM_CD = "M" });

                Title = "[기간]" + (StartDt).ToString("yyyy-MM-dd") + "~" + (EndDt).ToString("yyyy-MM-dd") + ",   [사업장]" + M_SL_AREA_NM.CLSS_DESC;

                switch (SystemProperties.USER_VO.OSTR_FLG)
                {
                    case "R": // 로보콘
                        this.SelectMstList = resultList;
                        break;
                    case "Y": // 고객사
                        //자기가 만든 마스터만 보여주기
                        this.SelectMstList = resultList.Where(x => x.CRE_USR_ID.Equals(SystemProperties.USER_VO.USR_ID)).ToList<PurVo>();
                        break;
                    case "N": // 샵업체
                        //자기 상위의 고객사가 만든 마스터만 보여주기
                        if (SystemProperties.USER_VO.PRNT_GRP_ID != null)
                            this.SelectMstList = resultList.Where(x => x.GRP_ID.Equals(SystemProperties.USER_VO.PRNT_GRP_ID)).ToList<PurVo>();
                        break;
                    default:
                        break;
                }

                if(this.SelectMstList.Count > 0)
                {
                    if (string.IsNullOrEmpty(_PUR_ORD_NO))
                    {
                        SelectedMstItem = SelectMstList[0];
                    }
                    else
                    {
                        SelectedMstItem = SelectMstList.Where(x => x.PUR_NO.Equals(_PUR_ORD_NO)).LastOrDefault<PurVo>();
                    }
                }
                else
                {
                    SelectedMstItem = null;
                    SelectDtlList = new List<PurVo>();
                    SearchDetail = null;
                }

                MstValCheck();

                Title = "[기간]" + (StartDt).ToString("yyyy-MM-dd") + "~" + (EndDt).ToString("yyyy-MM-dd") + ",   [사업장]" + M_SL_AREA_NM.CLSS_DESC;
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }


        // 마스터의 디테일 조회(발주 상세정보)
        [Command]
        public async void SelectMstDetail()
        {
            try
            {
                if (this.SelectedMstItem == null) return;

                IList<PurVo> resultList = await PostHttpSelect<PurVo>("p441106/dtl", SelectedMstItem);

                switch (SystemProperties.USER_VO.OSTR_FLG)
                {
                    case "R": // 로보콘
                        this.SelectDtlList = resultList;
                        break;
                    case "Y": // 고객사
                        resultList = resultList.Where<PurVo>(x => x.N1ST_RVW_USR_ID != null).ToList();
                        this.SelectDtlList = resultList.Where<PurVo>(x => x.N1ST_RVW_USR_ID.Equals(SystemProperties.USER_VO.GRP_ID)).ToList();
                        break;
                    case "N": // 샵업체
                        //자기가 등록한 도면만 모두조회
                        this.SelectDtlList = resultList.Where<PurVo>(x => x.CRE_USR_ID.Equals(SystemProperties.USER)).ToList();
                        break;
                    default:
                        break;
                }

                DtlValCheck();
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }

        private void MstValCheck()
        {
            switch (SystemProperties.USER_VO.OSTR_FLG)
            {
                case "Y": // 고객사
                    isM_INSERT = true;
                    if (SelectMstList.Count >= 1)
                    {
                        isM_UPDATE = true;
                        isM_DELETE = true;
                    }
                    else
                    {
                        isM_UPDATE = false;
                        isM_DELETE = false;
                    }
                    break;

                case "R": // 로보콘 (임시)
                    isM_INSERT = true;
                    if (SelectMstList.Count >= 1)
                    {
                        isM_UPDATE = true;
                        isM_DELETE = true;
                    }
                    else
                    {
                        isM_UPDATE = false;
                        isM_DELETE = false;
                    }
                    break;

                default: // 로보콘, 샵업체
                    isM_INSERT = false;
                    isM_UPDATE = false;
                    isM_DELETE = false;
                    break;
            }
        }

        private void DtlValCheck()
        {
            switch (SystemProperties.USER_VO.OSTR_FLG)
            {
                case "R": // 로보콘
                    //isD_INSERT = false;
                    //isD_UPDATE = SelectDtlList.Count >= 1 ? true : false;
                    //isD_DELETE = false;
                    //isD_CONFIRM = false;

                    isD_INSERT = true;
                    if (SelectDtlList.Count >= 1)
                    {
                        isD_UPDATE = true;  //도면저장 0
                        isD_CONFIRM = true; //도면확정권한 O
                        isD_DELETE = true;  //도면삭제 0
                    }
                    else
                    {
                        isD_UPDATE = false;
                        isD_CONFIRM = false;
                        isD_DELETE = false;  //도면삭제 X
                    }
                    break;

                case "Y": // 고객사
                    isD_INSERT = false;
                    isD_DELETE = false;

                    if (SelectDtlList.Count >= 1)
                    {
                        isD_UPDATE = true;  //도면저장 0
                        isD_CONFIRM = true; //도면확정권한 O
                    }
                    else
                    {
                        isD_UPDATE = false;
                        isD_CONFIRM = false;
                    }
                    break;

                case "N": // 삽업체
                    isD_CONFIRM = false;  //도면확정권한 X
                    isD_INSERT = true;
                    if (SelectDtlList.Count >= 1)
                    {
                        isD_UPDATE = true;  //도면저장 0
                        isD_DELETE = true;  //도면삭제 0
                    }
                    else
                    {
                        isD_UPDATE = false;  //도면저장 X
                        isD_DELETE = false;  //도면삭제 X
                    }
                    break;

                default: 
                    break;
            }
        }



        //private void ValCheck()
        //{
        //    try
        //    {
        //        if (this.UserVo.OSTR_FLG.Equals("R"))  //로보콘
        //        {
        //            isM_INSERT = false;
        //            isM_UPDATE = false;
        //            isM_DELETE = false;

        //            isD_INSERT = false;
        //            isD_UPDATE = false;
        //            isD_DELETE = false;

        //            if (SelectDtlList.Count >= 1)
        //            {
        //                isD_CONFIRM = true;
        //            }
        //            else
        //            {
        //                isD_CONFIRM = false;
        //            }
        //        }
        //        else if (this.UserVo.OSTR_FLG.Equals("Y"))  //고객사
        //        {
        //            isM_INSERT = true;

        //            if (SelectMstList.Count >= 1)
        //            {
        //                isM_UPDATE = true;
        //                isM_DELETE = true;
        //            }
        //            else
        //            {
        //                isM_UPDATE = false;
        //                isM_DELETE = false;
        //            }

        //            if (SelectDtlList.Count >= 1)
        //            {
        //                isD_UPDATE = true;  //도면저장 0
        //                isD_CONFIRM = true; //도면확정권한 O
        //            }
        //            else
        //            {
        //                isD_UPDATE = false;
        //                isD_CONFIRM = false;
        //            }

        //            isD_INSERT = false;
        //            isD_DELETE = false;
        //        }
        //        else if (this.UserVo.OSTR_FLG.Equals("N"))  //샵업체
        //        {
        //            isM_INSERT = false;
        //            isM_UPDATE = false;
        //            isM_DELETE = false;
        //            isD_CONFIRM = false;  //도면확정권한 X
        //            isD_INSERT = true;

        //            if (SelectDtlList.Count >= 1)
        //            {
        //                isD_UPDATE = true;  //도면저장 0
        //                isD_DELETE = true;  //도면삭제 0
        //            }
        //            else
        //            {
        //                isD_UPDATE = false;  //도면저장 X
        //                isD_DELETE = false;  //도면삭제 X
        //            }
        //        }
        //    }
        //    catch (System.Exception eLog)
        //    {
        //        WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
        //        return;
        //    }
        //}




        [Command]
        public void NewContact()
        {
            masterDialog = new P441106MasterDialog(new PurVo() { DELT_FLG = "N", AREA_CD = M_SL_AREA_NM.CLSS_CD, AREA_NM = M_SL_AREA_NM.CLSS_DESC, PUR_DT = System.DateTime.Now.ToString("yyyy-MM-dd"), PUR_CLZ_FLG = "N", PUR_EMPE_ID = SystemProperties.USER_VO.USR_ID });
            masterDialog.Title = _title + " - 추가";
            masterDialog.Owner = Application.Current.MainWindow;
            masterDialog.BorderEffect = BorderEffect.Default;
            masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            bool isDialog = (bool)masterDialog.ShowDialog();
            if (isDialog)
            {
                Refresh(masterDialog.PUR_NO);
            }
        }


        [Command]
        public void EditContact()
        {
            if (this.SelectedMstItem == null) return;

            masterDialog = new P441106MasterDialog(SelectedMstItem);
            masterDialog.Title = _title + " - 수정";
            masterDialog.Owner = Application.Current.MainWindow;
            masterDialog.BorderEffect = BorderEffect.Default;
            masterDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            masterDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            bool isDialog = (bool)masterDialog.ShowDialog();
            if (isDialog)
            {
                Refresh(masterDialog.PUR_NO);
            }
        }


        [Command]
        public async void DelContact()
        {
            try
            {
                if (this.SelectedMstItem == null) return;
                if(this.SelectDtlList.Count > 0)
                {
                    WinUIMessageBox.Show(" 등록된 도면이 있습니다.", "[삭제]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                MessageBoxResult result = WinUIMessageBox.Show("[" + SelectedMstItem.PUR_NO + "]" + " 정말로 삭제 하시겠습니까?", "[삭제]" + _title, MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    if(await PostHttpInsert("p441106/mst/d", SelectedMstItem))
                    {
                        //성공
                        WinUIMessageBox.Show("삭제가 완료되었습니다.", _title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);

                        Refresh();
                    }
                }
            }
            catch
            {
            }
        }

        [Command]
        public async void FileDownloadContact()
        {
            try
            {
                // 유효검사
                if (this.SearchDetail == null) return;

                DXFolderBrowserDialog _folderDiloag = new DXFolderBrowserDialog();
                _folderDiloag.ShowNewFolderButton = true;
                _folderDiloag.Description = "[파일명] 저장 폴더를 선택해주세요";
                _folderDiloag.RootFolder = Environment.SpecialFolder.Desktop;

                if (_folderDiloag.ShowDialog() == true)
                {
                    if (DXSplashScreen.IsActive == false) DXSplashScreen.Show<ProgressWindow>();

                    var _downFileVo = await PostHttpSelectItem<PurVo>("p441106/dtl/file", SearchDetail);
                    if(_downFileVo != null)
                    {
                        string _tmpPath = Path.Combine(_folderDiloag.SelectedPath, _downFileVo.FLR_NM);
                        File.WriteAllBytes(_tmpPath, _downFileVo.FLR_FILE);
                        System.Diagnostics.Process.Start(_folderDiloag.SelectedPath);
                    }


                    if (DXSplashScreen.IsActive == true) DXSplashScreen.Close();
                }
            }
            catch (System.Exception eLog)
            {
                if (DXSplashScreen.IsActive == true) DXSplashScreen.Close();

                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }


        [Command]
        public async void FileConfirmContact()
        {
            try
            {
                //도면확정
                if (SearchDetail == null) return;

                if (SystemProperties.USER_VO.OSTR_FLG != "Y")
                {
                    WinUIMessageBox.Show("도면 확정 권한이 없습니다.", _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                MessageBoxResult result = WinUIMessageBox.Show("도면번호" + "[" + SearchDetail.FLR_NO + "]" + " (을)를 정말로 확정 하시겠습니까?", "[확정]" + _title, MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    SearchDetail.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
                    SearchDetail.UPD_USR_ID = SystemProperties.USER;

                    if(await PostHttpInsert("p441106/dtl/u", SearchDetail))
                    {
                        //성공
                        WinUIMessageBox.Show("도면확정이 완료되었습니다.", _title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);

                        SelectMstDetail();
                    }
                }
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }


        [Command]
        public void NewDtlContact()
        {
            // 11.05 도면등록을 위해 잠시 주석
            if (SelectedMstItem == null) return;

            detailDialog = new P441106DetailDialog(new PurVo() { PUR_NO = SelectedMstItem.PUR_NO/*, DE_CO_NM = SelectedMstItem.DE_CO_NM*/, CHNL_CD = SystemProperties.USER_VO.CHNL_CD, CRE_USR_ID = SystemProperties.USER, UPD_USR_ID = SystemProperties.USER, DUE_DT = SelectedMstItem.DUE_DT });
            detailDialog.Title = "도면등록 - " + SelectedMstItem.PUR_NO + " / " + SelectedMstItem.CO_NM;
            detailDialog.Owner = Application.Current.MainWindow;
            detailDialog.BorderEffect = BorderEffect.Default;
            detailDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            detailDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            //Refresh();
            bool isDialog = (bool)detailDialog.ShowDialog();
            if (isDialog)
            {
                SelectMstDetail();
                SelectedMstItem.PUR_SUM_AMT = this.SelectDtlList.Sum<PurVo>(s => Convert.ToDouble(s.PUR_WGT));
            }
        }


        [Command]
        public async void DelDtlContact()
        {
            try
            {
                //수정 삭제
                if (SearchDetail == null) return;
                if (SearchDetail.UPD_DT != null)
                {
                    WinUIMessageBox.Show(" 이미 확정된 도면입니다.", "[삭제]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }


                MessageBoxResult result = WinUIMessageBox.Show("[" + SearchDetail.PUR_NO + "/" + SearchDetail.PUR_SEQ + "]" + " 정말로 삭제 하시겠습니까?", "[삭제]" + _title, MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    SelectedMstItem.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;

                    if(await PostHttpInsert("p441106/dtl/d", SearchDetail))
                    {
                        //성공
                        WinUIMessageBox.Show("삭제가 완료되었습니다.", _title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);

                        SelectMstDetail();
                        SelectedMstItem.PUR_SUM_AMT = this.SelectDtlList.Sum<PurVo>(s => Convert.ToDouble(s.PUR_WGT));
                    }
                }
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }


        public async void SYSTEM_CODE_VO()
        {
            try
            {
                // 사업장
                this.AreaList = await GetHttpSelect<SystemCodeVo>("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "L-002");
                if(AreaList.Count > 0)
                {
                    this.M_SL_AREA_NM = AreaList[0];
                }

                // 사용자정보
                //this.UserVo = (await PostHttpSelect<GroupUserVo>("s136/u", new GroupUserVo { USR_ID = SystemProperties.USER_VO.USR_ID, CHNL_CD = SystemProperties.USER_VO.CHNL_CD, DELT_FLG = "N" })).FirstOrDefault();

                // 사용자정보 (로그인시 정보 가져옴)
                //this.UserVo = SystemProperties.USER_VO;

                Refresh();
            }
            catch
            {
            }
        }


        #region JSON HTTP Server & Client DATA BINDING

        /// <summary>
        /// JSON GET 을 통해 정보를 가져옵니다.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Path"></param>
        /// <returns></returns>
        private async Task<List<T>> GetHttpSelect<T>(string Path)
        {
            var ret = new List<T>();

            try
            {
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync(Path))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        ret = JsonConvert.DeserializeObject<IEnumerable<T>>(await response.Content.ReadAsStringAsync()).Cast<T>().ToList();
                    }
                }
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
            }

            return ret;
        }


        /// <summary>
        /// JSON POST 을 통해 List 정보를 가져옵니다.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Path"></param>
        /// <param name="Obj"></param>
        /// <returns></returns>
        private async Task<List<T>> PostHttpSelect<T>(string Path, object Obj)
        {
            var ret = new List<T>();

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
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
            }

            return ret;
        }


        /// <summary>
        /// JSON POST 을 통해 Object 정보를 가져옵니다.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Path"></param>
        /// <param name="Obj"></param>
        /// <returns></returns>
        private async Task<T> PostHttpSelectItem<T>(string Path, object Obj)
        {
            var ret = default(T);

            try
            {
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync(Path
                                                                                                   , new StringContent(JsonConvert.SerializeObject(Obj), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        ret = JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
                    }
                }
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
            }

            return ret;
        }


        /// <summary>
        /// JSON POST 를 통해 정보를 전달합니다.
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="Obj"></param>
        /// <returns></returns>
        private async Task<Boolean> PostHttpInsert(string Path, object Obj)
        {
            var ret = false;

            try
            {
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync(Path
                                                                                                   , new StringContent(JsonConvert.SerializeObject(Obj), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        int _Num = 0;
                        string resultMsg = await response.Content.ReadAsStringAsync();
                        if (int.TryParse(resultMsg, out _Num) == false)
                        {
                            //실패
                            WinUIMessageBox.Show(resultMsg, _title, MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                        {
                            // 성공
                            ret = true;
                        }
                    }
                }
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
            }

            return ret;
        } 
        #endregion


        #region MVVM OBJECT DATA BINDING

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

        //출력 수
        private string _M_BAR_TEXT = "1";
        public string M_BAR_TEXT
        {
            get { return _M_BAR_TEXT; }
            set { SetProperty(ref _M_BAR_TEXT, value, () => M_BAR_TEXT); }
        }

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

        private IList<PurVo> selectedMstList = new List<PurVo>();
        public IList<PurVo> SelectMstList
        {
            get { return selectedMstList; }
            set { SetProperty(ref selectedMstList, value, () => SelectMstList); }
        }

        PurVo _selectedMstItem;
        public PurVo SelectedMstItem
        {
            get { return _selectedMstItem; }
            set { SetProperty(ref _selectedMstItem, value, () => SelectedMstItem, SelectMstDetail); }
        }

        private IList<PurVo> _selectDtlList = new List<PurVo>();
        public IList<PurVo> SelectDtlList
        {
            get { return _selectDtlList; }
            set { SetProperty(ref _selectDtlList, value, () => SelectDtlList); }
        }

        PurVo _searchDetail;
        public PurVo SearchDetail
        {
            get { return _searchDetail; }
            set { SetProperty(ref _searchDetail, value, () => SearchDetail); }
        }


        //// 발주 클릭후 도면 등록 버튼 활성화
        private bool? _isM_UPDATE = false;
        public bool? isM_UPDATE
        {
            get { return _isM_UPDATE; }
            set { SetProperty(ref _isM_UPDATE, value, () => isM_UPDATE); }
        }

        private bool? _isM_INSERT = false;
        public bool? isM_INSERT
        {
            get { return _isM_INSERT; }
            set { SetProperty(ref _isM_INSERT, value, () => isM_INSERT); }
        }


        private bool? _isM_DELETE = false;
        public bool? isM_DELETE
        {
            get { return _isM_DELETE; }
            set { SetProperty(ref _isM_DELETE, value, () => isM_DELETE); }
        }


        private bool? _isD_INSERT = false;
        public bool? isD_INSERT
        {
            get { return _isD_INSERT; }
            set { SetProperty(ref _isD_INSERT, value, () => isD_INSERT); }
        }

        private bool? _isD_UPDATE = false;
        public bool? isD_UPDATE
        {
            get { return _isD_UPDATE; }
            set { SetProperty(ref _isD_UPDATE, value, () => isD_UPDATE); }
        }

        private bool? _isD_CONFIRM = false;
        public bool? isD_CONFIRM
        {
            get { return _isD_CONFIRM; }
            set { SetProperty(ref _isD_CONFIRM, value, () => isD_CONFIRM); }
        }

        private bool? _isD_DELETE = false;
        public bool? isD_DELETE
        {
            get { return _isD_DELETE; }
            set { SetProperty(ref _isD_DELETE, value, () => isD_DELETE); }
        }

        string _Title = string.Empty;
        public string Title
        {
            get { return _Title; }
            set { SetProperty(ref _Title, value, () => Title); }
        } 
        #endregion
    }

}
