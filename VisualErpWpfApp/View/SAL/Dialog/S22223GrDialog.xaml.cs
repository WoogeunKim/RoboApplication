using AquilaErpWpfApp3.Util;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Sale;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AquilaErpWpfApp3.View.SAL.Dialog
{
    /// <summary>
    /// S22223GrDialog.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class S22223GrDialog : DXWindow
    {
        private string _title = "남품확인서 입력";
        // 추가&수정 여부
        private bool isEdit = false;

        private SaleVo orgDao;
        private SaleVo updateDao;


        public S22223GrDialog(SaleVo Dao)
        {
            InitializeComponent();

            this.orgDao = new SaleVo()
            {
                GR_NO = Dao.GR_NO,
                GR_CNTR_NM = Dao.GR_CNTR_NM,
                GR_CNTR_ROUT_NM = Dao.GR_CNTR_ROUT_NM,
                GR_RLSE_KNT_NM = Dao.GR_RLSE_KNT_NM,
                GR_SND_DT = Dao.GR_SND_DT,
                GR_ARR_DT = Dao.GR_ARR_DT,
                GR_INV_NO = Dao.GR_INV_NO,
                GR_CAR_NO = Dao.GR_CAR_NO,
                INCR_RT = Dao.INCR_RT,
                GR_WGT = Dao.GR_WGT,
                ARR_USR_NM = Dao.ARR_USR_NM,
                ARR_ADDR = Dao.ARR_ADDR,
                CAR_DRV_USR_NM = Dao.CAR_DRV_USR_NM,
                N1ST_TEL_NO = Dao.N1ST_TEL_NO,
                N2ND_TEL_NO = Dao.N2ND_TEL_NO,
                RLSE_CMD_NO = Dao.RLSE_CMD_NO
            };

            // GR_NO == null 경우 [추가] 아니면 [수정]
            if (this.orgDao.GR_NO == null)
            {
                this.Title = _title + " - 추가";
                this.orgDao.GR_NO = this.orgDao.RLSE_CMD_NO;
                this.orgDao.GR_SND_DT = System.DateTime.Now.ToString("yyyy-MM-dd");
                this.orgDao.GR_ARR_DT = System.DateTime.Now.ToString("yyyy-MM-dd");
            }
            else
            {
                this.Title = _title + " - 수정";
                this.isEdit = true;
            }


            this.configCode.DataContext = orgDao;
            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
            this.OKButton.Click += new RoutedEventHandler(OKButton_Click);
            this.CancelButton.Click += new RoutedEventHandler(CancelButton_Click);
        }


        private async void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ValueCheckd())
                {
                    int _Num = 0;
                    if (isEdit == false)
                    {
                        this.updateDao = getDomain();

                        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s22223/dlg/i", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                string result = await response.Content.ReadAsStringAsync();
                                if (int.TryParse(result, out _Num) == false)
                                {
                                    //실패
                                    WinUIMessageBox.Show(result, _title, MessageBoxButton.OK, MessageBoxImage.Error);
                                    return;
                                }

                                //성공
                                this.DialogResult = true;
                                this.Close();
                                //리포트 출력으로 이어짐으로 완료 메시지 X
                                //WinUIMessageBox.Show("완료 되었습니다", _title, MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                        }
                    }
                    else
                    {
                        this.updateDao = getDomain();

                        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s22223/dlg/u", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                string result = await response.Content.ReadAsStringAsync();
                                if (int.TryParse(result, out _Num) == false)
                                {
                                    //실패
                                    WinUIMessageBox.Show(result, _title, MessageBoxButton.OK, MessageBoxImage.Error);
                                    return;
                                }
                                //성공
                                this.DialogResult = true;
                                this.Close();
                                //리포트 출력으로 이어짐으로 완료 메시지 X
                                //WinUIMessageBox.Show("완료 되었습니다", _title, MessageBoxButton.OK, MessageBoxImage.Information);
                            }
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

        // 입력 값 유효검사
        public Boolean ValueCheckd()
        {
            if (string.IsNullOrEmpty(this.text_GR_NO.Text))
            {
                WinUIMessageBox.Show("[GR번호] 값이 맞지 않습니다.", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.text_GR_NO.IsTabStop = true;
                this.text_GR_NO.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(this.text_GR_CNTR_NM.Text))
            {
                WinUIMessageBox.Show("[공사명] 입력 값이 맞지 않습니다.", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.text_GR_CNTR_NM.IsTabStop = true;
                this.text_GR_CNTR_NM.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(this.text_GR_CNTR_ROUT_NM.Text))
            {
                WinUIMessageBox.Show("[공정명] 입력 값이 맞지 않습니다.", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.text_GR_CNTR_ROUT_NM.IsTabStop = true;
                this.text_GR_CNTR_ROUT_NM.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(this.text_GR_RLSE_KNT_NM.Text))
            {
                WinUIMessageBox.Show("[납품차수] 입력 값이 맞지 않습니다.", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.text_GR_RLSE_KNT_NM.IsTabStop = true;
                this.text_GR_RLSE_KNT_NM.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(this.text_GR_SND_DT.Text))
            {
                WinUIMessageBox.Show("[발송일] 입력 값이 맞지 않습니다.", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.text_GR_SND_DT.IsTabStop = true;
                this.text_GR_SND_DT.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(this.text_GR_ARR_DT.Text))
            {
                WinUIMessageBox.Show("[도착일] 입력 값이 맞지 않습니다.", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.text_GR_ARR_DT.IsTabStop = true;
                this.text_GR_ARR_DT.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(this.text_GR_INV_NO.Text))
            {
                WinUIMessageBox.Show("[송정번호] 입력 값이 맞지 않습니다.", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.text_GR_INV_NO.IsTabStop = true;
                this.text_GR_INV_NO.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(this.text_ARR_USR_NM.Text))
            {
                WinUIMessageBox.Show("[착지담당] 입력 값이 맞지 않습니다.", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.text_ARR_USR_NM.IsTabStop = true;
                this.text_ARR_USR_NM.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(this.text_ARR_ADDR.Text))
            {
                WinUIMessageBox.Show("[착지주소] 입력 값이 맞지 않습니다.", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.text_ARR_ADDR.IsTabStop = true;
                this.text_ARR_ADDR.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(this.text_N1ST_TEL_NO.Text))
            {
                WinUIMessageBox.Show("[연락처] 입력 값이 맞지 않습니다.", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.text_N1ST_TEL_NO.IsTabStop = true;
                this.text_N1ST_TEL_NO.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(this.text_GR_CAR_NO.Text))
            {
                WinUIMessageBox.Show("[차량번호] 입력 값이 맞지 않습니다.", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.text_GR_CAR_NO.IsTabStop = true;
                this.text_GR_CAR_NO.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(this.text_CAR_DRV_USR_NM.Text))
            {
                WinUIMessageBox.Show("[운전자] 입력 값이 맞지 않습니다.", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.text_CAR_DRV_USR_NM.IsTabStop = true;
                this.text_CAR_DRV_USR_NM.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(this.text_N2ND_TEL_NO.Text))
            {
                WinUIMessageBox.Show("[연락처] 입력 값이 맞지 않습니다.", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.text_N2ND_TEL_NO.IsTabStop = true;
                this.text_N2ND_TEL_NO.Focus();
                return false;
            }
            return true;
        }

        private SaleVo getDomain()
        {
            SaleVo Dao = new SaleVo();

            Dao.GR_NO = orgDao.GR_NO;
            Dao.INCR_RT = orgDao.INCR_RT;
            Dao.GR_WGT = orgDao.GR_WGT;

            Dao.GR_CNTR_NM = this.text_GR_CNTR_NM.Text;
            Dao.GR_CNTR_ROUT_NM = this.text_GR_CNTR_ROUT_NM.Text;
            Dao.GR_RLSE_KNT_NM = this.text_GR_RLSE_KNT_NM.Text;
            Dao.GR_SND_DT = Convert.ToDateTime(this.text_GR_SND_DT.Text).ToString("yyyy-MM-dd");
            Dao.GR_ARR_DT = Convert.ToDateTime(this.text_GR_ARR_DT.Text).ToString("yyyy-MM-dd");
            Dao.GR_INV_NO = this.text_GR_INV_NO.Text;
            Dao.ARR_USR_NM = this.text_ARR_USR_NM.Text;
            Dao.ARR_ADDR = this.text_ARR_ADDR.Text;
            Dao.N1ST_TEL_NO = this.text_N1ST_TEL_NO.Text;
            Dao.GR_CAR_NO = this.text_GR_CAR_NO.Text;
            Dao.CAR_DRV_USR_NM = this.text_CAR_DRV_USR_NM.Text;
            Dao.N2ND_TEL_NO = this.text_N2ND_TEL_NO.Text;

            Dao.CRE_USR_ID = SystemProperties.USER;
            Dao.UPD_USR_ID = SystemProperties.USER;
            Dao.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;

            return Dao;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void HandleEsc(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.DialogResult = false;
                this.Close();
            }
        }

    }
}
