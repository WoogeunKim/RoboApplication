using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Code;
using ModelsLibrary.Sale;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using AquilaErpWpfApp3.Util;
using AquilaErpWpfApp3.View.SAL.Dialog;
using System.IO;
using System.Drawing;
using AquilaErpWpfApp3.View.SAL.Report;
using DevExpress.Xpf.Printing;

namespace AquilaErpWpfApp3.ViewModel
{
    public sealed class S2219ViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private string title = "견적서 발행";

        private IList<SaleVo> selectedMstList = new List<SaleVo>();

        private IList<SaleVo> selectedEstmList = new List<SaleVo>();

        private IList<SaleVo> selectedProdList = new List<SaleVo>();

        private S2219MasterDialog masterDialog;


        public S2219ViewModel()
        {
            StartDt = System.DateTime.Now;
            EndDt = System.DateTime.Now;

            SYSTEM_CODE_VO();

        }

        public async void SYSTEM_CODE_VO()  // 프로그램 실행시 실행될 것들
        {
            try
            {
                //SelectProdlList - 공장 과정 선택 리스트 불러오기
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s2219/prod", new StringContent(JsonConvert.SerializeObject(new SaleVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectProdlList = JsonConvert.DeserializeObject<IEnumerable<SaleVo>>(await response.Content.ReadAsStringAsync()).Cast<SaleVo>().ToList();
                    }
                }
            }
            catch (Exception eLog)
            {
                MessageBox.Show(eLog.Message, title, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        [Command]
        public async void Refresh(string _ESTM_NO = null)
        {
            try
            {
                //SelectMstList - 견적서 MST List 불러오기
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s2219/mst", new StringContent(JsonConvert.SerializeObject(new SaleVo() { FM_DT = StartDt.ToString("yyyy-MM-dd"), TO_DT = EndDt.ToString("yyyy-MM-dd"), CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectMstList = JsonConvert.DeserializeObject<IEnumerable<SaleVo>>(await response.Content.ReadAsStringAsync()).Cast<SaleVo>().ToList();

                        //SelectedMstItem - 견적서 MST 항목 선택시 발생
                        if (this.SelectMstList.Count >= 1)
                        {
                            if (string.IsNullOrEmpty(_ESTM_NO))
                            {
                                this.SelectedMstItem = this.SelectMstList[0]; // 기본값 = 첫 번째 항목
                            }
                            else
                            {
                                this.SelectedMstItem = this.SelectMstList.Where(x => x.ESTM_NO.Equals(_ESTM_NO)).LastOrDefault<SaleVo>(); // 선택한 항목 표출
                            }

                            this.isM_UPDATE = true; // 항목 선택시 수정 버튼 활성화
                            this.isM_DELETE = true; // 항목 선택시 삭제 버튼 활성화
                        }
                        else
                        {
                            this.isM_UPDATE = false; // 기본값 =  수정 버튼 비활성화
                            this.isM_DELETE = false; // 기본값 =  삭제 버튼 비활성화
                        }
                    }
                }
            }
            catch (Exception eLog)
            {
                MessageBox.Show(eLog.Message, title, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        [Command]
        public void NewContact()
        {
            try
            {
                masterDialog = new S2219MasterDialog();
                masterDialog.Title = title + " 마스터 관리 - 추가";
                masterDialog.Owner = Application.Current.MainWindow;
                masterDialog.BorderEffect = BorderEffect.Default;
                masterDialog.BorderEffectActiveColor = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 128, 0));
                masterDialog.BorderEffectInactiveColor = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 170, 170));
                bool isDialog = (bool)masterDialog.ShowDialog();
                if (isDialog)
                {
                    Refresh();
                }
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + "견적서 추가", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }

        [Command]
        public void EditContact()
        {
            try
            {
                if (SelectedDtlItem == null)
                {
                    return;
                }

                masterDialog = new S2219MasterDialog(SelectedDtlItem);
                masterDialog.Title = title + " 마스터 관리 - 수정 - [견적 번호 : " + SelectedDtlItem.ESTM_NO + "]";
                masterDialog.Owner = Application.Current.MainWindow;
                masterDialog.BorderEffect = BorderEffect.Default;
                masterDialog.BorderEffectActiveColor = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 128, 0));
                masterDialog.BorderEffectInactiveColor = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 170, 170));
                bool isDialog = (bool)masterDialog.ShowDialog();
                if (isDialog)
                {
                    Refresh(SelectedDtlItem.ESTM_NO);
                }
            }
            catch (System.Exception eLog)
            {
                //DXSplashScreen.Close();
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + "견적서 수정", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }

        [Command]
        public async void DelContact()
        {
            try
            {
                if (SelectedDtlItem == null)
                {
                    return;
                }

                MessageBoxResult result = WinUIMessageBox.Show("[견적 번호 : " + SelectedDtlItem.ESTM_NO + "]" + " 정말로 삭제 하시겠습니까?", "[삭제]" + "견적서 삭제", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s2219/mst/d", new StringContent(JsonConvert.SerializeObject(SelectedDtlItem), System.Text.Encoding.UTF8, "application/json")))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            int _Num = 0;
                            string resultMsg = await response.Content.ReadAsStringAsync();
                            if (int.TryParse(resultMsg, out _Num) == false)
                            {
                                //실패
                                WinUIMessageBox.Show(resultMsg, title, MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }

                            WinUIMessageBox.Show("삭제가 완료되었습니다.", "[삭제]" + "견적서 삭제", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                            Refresh();
                        }
                    }
                }
            }
            catch (System.Exception eLog)
            {
                //DXSplashScreen.Close();
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + "견적서 삭제", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }

        [Command]
        public async void NewEstmContact(string _obj)
        {
            try
            {
                if (SelectedDtlItem == null)
                {
                    WinUIMessageBox.Show("견적 마스터 정보를 선택하세요", "견적 마스터 정보 미선택", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                SaleVo estmvo = new SaleVo();
                if (_obj.Equals("I"))
                {
                    estmvo = SelectedDtlItem;
                    estmvo.CRE_USR_ID = SystemProperties.USER;
                    estmvo.UPD_USR_ID = SystemProperties.USER;
                }
                else if (_obj.Equals("P"))
                {
                    //SelectedProdItem
                    if (SelectedProdItem == null)
                    {
                        WinUIMessageBox.Show("공정 과정을 선택하세요", "공정 과정 미선택", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    estmvo = SelectedDtlItem;
                    estmvo.ESTM_ITM_NM = SelectedProdItem.ROUT_NM;
                    estmvo.CRE_USR_ID = SystemProperties.USER;
                    estmvo.UPD_USR_ID = SystemProperties.USER;
                }

                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s2219/estm/i", new StringContent(JsonConvert.SerializeObject(estmvo), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        int _Num = 0;
                        string resultMsg = await response.Content.ReadAsStringAsync();
                        if (int.TryParse(resultMsg, out _Num) == false)
                        {
                            //실패
                            WinUIMessageBox.Show(resultMsg, "품목 추가", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        Refresh(SelectedDtlItem.ESTM_NO);
                    }
                }
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }

        [Command]
        public async void DelEstmContact()
        {
            try
            {
                if (SelectedEstmItem == null)
                {
                    WinUIMessageBox.Show("삭제할 품목을 선택하세요", "삭제 품목 미선택", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                MessageBoxResult result = WinUIMessageBox.Show(SelectedEstmItem.ESTM_NO + "(" + SelectedEstmItem.ESTM_ITM_NM + ") 정말로 삭제 하시겠습니까?", "품목 삭제", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s2219/estm/d", new StringContent(JsonConvert.SerializeObject(SelectedEstmItem), System.Text.Encoding.UTF8, "application/json")))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            int _Num = 0;
                            string resultMsg = await response.Content.ReadAsStringAsync();
                            if (int.TryParse(resultMsg, out _Num) == false)
                            {
                                //실패
                                WinUIMessageBox.Show(resultMsg, "품목 삭제", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                        }
                        WinUIMessageBox.Show("완료 되었습니다", "품목 삭제", MessageBoxButton.OK, MessageBoxImage.Information);

                        Refresh(SelectedEstmItem.ESTM_NO);
                        return;
                    }
                }

            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }



        // 견적서 MST 항목 선택 시 조회될 것들
        public async void MstItemChange()
        {
            try
            {
                if (this.SelectedMstItem == null) return;

                // 견적서 Dtl 조회
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s2219/dtl", new StringContent(JsonConvert.SerializeObject(this.SelectedMstItem), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        SaleVo dtlVo = new SaleVo();
                        dtlVo = JsonConvert.DeserializeObject<SaleVo>(await response.Content.ReadAsStringAsync());

                        //if (dtlVo.ITM_FILE != null)
                        //{
                        //    using (MemoryStream memoryStream = new MemoryStream(dtlVo.ITM_FILE))
                        //    {
                        //        dtlVo.ITM_FILE_BIT = new Bitmap(memoryStream);
                        //    };

                        //    //using (Stream str = new MemoryStream(dtlVo.ITM_FILE))
                        //    //{
                        //    //    dtlVo.ITM_FILE_BIT = new Bitmap(str);
                        //    //};
                        //}

                        this.SelectedDtlItem = dtlVo;
                    }
                }

                // 견적서 내역 조회
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s2219/estm", new StringContent(JsonConvert.SerializeObject(this.SelectedMstItem), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.SelectEstmList = JsonConvert.DeserializeObject<IEnumerable<SaleVo>>(await response.Content.ReadAsStringAsync()).Cast<SaleVo>().ToList();
                    }
                }
            }
            catch (Exception eLog)
            {
                MessageBox.Show(eLog.Message, title, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }


        //2022-12-06 견적서 출력 
        [Command]
        public async void PrintContact()
        {
            try
            {
                //견적서 품목내역이 없을 때
                if (selectedEstmList == null)
                {
                    return;
                }

                IList<SaleVo> printlist = new List<SaleVo>();
                printlist = selectedEstmList;
                var linQueryResult = printlist.Count();

                //printlist[printlist.Count - 1].Num_To_Kor = Number2Hangle(selectedEstmList.Sum<SaleVo>(s => Convert.ToDouble(s.ITM_AMT)));

                for (int n = 0; n < linQueryResult; n++)
                {
                    printlist[n].ESTM_DT = SelectedDtlItem.ESTM_DT;
                    printlist[n].CO_NM = SelectedDtlItem.CO_NM;
                    printlist[n].MGR_NM = SelectedDtlItem.MGR_NM;
                    printlist[n].EXP_DT = SelectedDtlItem.EXP_DT;
                    printlist[n].Num_To_Kor = Number2Hangle(selectedEstmList.Sum<SaleVo>(s => Convert.ToDouble(s.ITM_AMT)));
                    //printlist[n].Num_To_Kor += printlist[n].ITM_AMT;
                }



                //printlist[0].Num_To_Kor = Number2Hangle(printlist.Sum<ITM_AMT>);

                S2219MstReport report = new S2219MstReport(printlist);
                report.Landscape = false;
                report.PrintingSystem.ShowPrintStatusDialog = true;
                report.PaperKind = System.Drawing.Printing.PaperKind.A4;

                report.Watermark.Text = Properties.Settings.Default.SettingCompany;
                report.Watermark.TextDirection = DevExpress.XtraPrinting.Drawing.DirectionMode.ForwardDiagonal;
                report.Watermark.Font = new System.Drawing.Font(report.PrintingSystem.Watermark.Font.FontFamily, 40);
                report.Watermark.ForeColor = System.Drawing.Color.PaleTurquoise;
                report.Watermark.TextTransparency = 150;


                var window = new DocumentPreviewWindow();
                window.PreviewControl.DocumentSource = report;
                report.CreateDocument(true);
                window.Title = "견적서 [" + printlist[0].ESTM_NO + "] ";
                window.Owner = Application.Current.MainWindow;
                window.ShowDialog();

            }
            catch
            {
                return;
            }
        }

        // 금액을 한글로 표시
        static public string Number2Hangle(object lngNumber)
        {
            bool UseDecimal = false;
            string Sign = "";
            int i = 0;
            int Level = 0;

            string[] NumberChar = new string[] { "", "일", "이", "삼", "사", "오", "육", "칠", "팔", "구" };
            string[] LevelChar = new string[] { "", "십", "백", "천" };
            string[] DecimalChar = new string[] { "", "만", "억", "조", "경" };

            string strValue = string.Format("{0}", lngNumber);
            string NumToKorea = Sign;
            UseDecimal = false;

            for (i = 0; i < strValue.Length; i++)
            {
                Level = strValue.Length - i;
                if (strValue.Substring(i, 1) != "0")
                {
                    UseDecimal = true;
                    if (((Level - 1) % 4) == 0)
                    {
                        NumToKorea = NumToKorea + NumberChar[int.Parse(strValue.Substring(i, 1))] + DecimalChar[(Level - 1) / 4];
                        UseDecimal = false;
                    }
                    else
                    {
                        if (strValue.Substring(i, 1) == "1")
                        {
                            NumToKorea = NumToKorea + LevelChar[(Level - 1) % 4];
                        }
                        else
                        {
                            NumToKorea = NumToKorea + NumberChar[int.Parse(strValue.Substring(i, 1))] + LevelChar[(Level - 1) % 4];
                        }
                    }
                }
                else
                {
                    if ((Level % 4 == 0) && UseDecimal)
                    {
                        NumToKorea = NumToKorea + DecimalChar[Level / 4];
                        UseDecimal = false;
                    }
                }
            }
            return NumToKorea;
        }

        #region Binding

        // 기간(From)
        DateTime _startDt;
        public DateTime StartDt
        {
            get { return _startDt; }
            set { SetProperty(ref _startDt, value, () => StartDt); }
        }

        // 기간(To)
        DateTime _endDt;
        public DateTime EndDt
        {
            get { return _endDt; }
            set { SetProperty(ref _endDt, value, () => EndDt); }
        }

        // 검색 (빈 검색창일때)
        private string _M_SEARCH_TEXT = string.Empty;

        // 검색 (입력된 검색창일때)
        public string M_SEARCH_TEXT
        {
            get { return _M_SEARCH_TEXT; }
            set { SetProperty(ref _M_SEARCH_TEXT, value, () => M_SEARCH_TEXT); }
        }

        // 견적서 마스터 정보 리스트 조회
        public IList<SaleVo> SelectMstList
        {
            get { return selectedMstList; }
            set { SetProperty(ref selectedMstList, value, () => SelectMstList); }
        }

        // 견적서 마스터 정보 항목 조회
        SaleVo _selectedMstItem;
        public SaleVo SelectedMstItem
        {
            get { return _selectedMstItem; }
            set { SetProperty(ref _selectedMstItem, value, () => SelectedMstItem, MstItemChange); }
        }

        // 견적서 디테일 정보 항목 조회
        SaleVo _selectedDtlItem;
        public SaleVo SelectedDtlItem
        {
            get { return _selectedDtlItem; }
            set { SetProperty(ref _selectedDtlItem, value, () => SelectedDtlItem); }
        }

        // 견적서 내역 정보 리스트 조회
        public IList<SaleVo> SelectEstmList
        {
            get { return selectedEstmList; }
            set { SetProperty(ref selectedEstmList, value, () => SelectEstmList); }
        }

        // 견적서 내역 정보 항목 조회
        SaleVo _selectedEstmItem;
        public SaleVo SelectedEstmItem
        {
            get { return _selectedEstmItem; }
            set { SetProperty(ref _selectedEstmItem, value, () => SelectedEstmItem); }
        }


        // 공정 과정 선택 정보 리스트 조회
        public IList<SaleVo> SelectProdlList
        {
            get { return selectedProdList; }
            set { SetProperty(ref selectedProdList, value, () => SelectProdlList); }
        }

        // 공정 과정 선택 정보 항목 조회
        SaleVo _selectedProdItem;
        public SaleVo SelectedProdItem
        {
            get { return _selectedProdItem; }
            set { SetProperty(ref _selectedProdItem, value, () => SelectedProdItem); }
        }

        // 수정 버튼 보이기 / 안보이게 하기기
        private bool? _isM_UPDATE = false;
        public bool? isM_UPDATE
        {
            get { return _isM_UPDATE; }
            set { SetProperty(ref _isM_UPDATE, value, () => isM_UPDATE); }
        }

        // 삭제 버튼 보이기 / 안보이게 하기
        private bool? _isM_DELETE = false;
        public bool? isM_DELETE
        {
            get { return _isM_DELETE; }
            set { SetProperty(ref _isM_DELETE, value, () => isM_DELETE); }
        }
        #endregion

    }
}