using AquilaErpWpfApp3.Util;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Dialogs;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Code;
using ModelsLibrary.Sale;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AquilaErpWpfApp3.View.SAL.Dialog
{
    /// <summary>
    /// S2219MasterDialog.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class S2219MasterDialog : DXWindow
    {

        private bool isEdit = false;
        private SaleVo updateDao = new SaleVo();

        private string title = "견적서 마스터 정보 등록";


        public S2219MasterDialog()
        {
            InitializeComponent();
        }

        public S2219MasterDialog(SaleVo Dao)
        {
            InitializeComponent();

            updateDao = Dao;
            isEdit = true;

            upDomain(Dao);
        }

        

        private async void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ValueCheckd())
                {
                    int _Num = 0;
                    this.updateDao = getDomain();//this.updateDao
                    //ProgramVo resultVo;
                    if (isEdit == false)
                    {
                        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s2219/mst/i", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                string result = await response.Content.ReadAsStringAsync();
                                if (int.TryParse(result, out _Num) == false)
                                {
                                    //실패
                                    WinUIMessageBox.Show(result, title, MessageBoxButton.OK, MessageBoxImage.Error);

                                    return;
                                }

                                //성공
                                WinUIMessageBox.Show("완료 되었습니다", title, MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                        }
                    }
                    else
                    {

                        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s2219/mst/u", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                string result = await response.Content.ReadAsStringAsync();
                                if (int.TryParse(result, out _Num) == false)
                                {
                                    //실패
                                    WinUIMessageBox.Show(result, title, MessageBoxButton.OK, MessageBoxImage.Error);
                                    return;
                                }

                                //성공
                                WinUIMessageBox.Show("완료 되었습니다", title, MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                        }
                    }
                    this.DialogResult = true;
                    this.Close();

                }
            }
            catch (Exception eLog)
            {
                MessageBox.Show(eLog.Message, title, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private SaleVo getDomain()
        {
            SaleVo Dom = new SaleVo();

            Dom = updateDao;

            Dom.CO_NM = this.text_CO_NM.Text;
            Dom.ESTM_DT = Convert.ToDateTime(this.text_ESTM_DT.Text).ToString("yyyy-MM-dd");
            if (this.text_EXP_DT.Text != "") Dom.EXP_DT = Convert.ToDateTime(this.text_EXP_DT.Text).ToString("yyyy-MM-dd");
            Dom.MGR_NM = this.text_MGR_NM.Text;
            Dom.PHN_NO = this.text_PHN_NO.Text;
            Dom.FAX_NO = this.text_FAX_NO.Text;
            Dom.ESTM_RMK = this.text_ESTM_RMK.Text;
            Dom.ITM_FILE = this.updateDao.ITM_FILE;
            if (Dom.ITM_FILE == null) Dom.ITM_FILE = new byte[0];

            Dom.ITM_FILE_NM = this.text_ITM_FILE_NM.Text;

            Dom.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
            Dom.CRE_USR_ID = SystemProperties.USER;
            Dom.UPD_USR_ID = SystemProperties.USER;

            return Dom;
        }

        private void upDomain(SaleVo upvo)
        {
            try
            {
                this.text_CO_NM.Text = upvo.CO_NM;
                this.text_ESTM_DT.Text = upvo.ESTM_DT;
                this.text_EXP_DT.Text = upvo.EXP_DT;
                this.text_MGR_NM.Text = upvo.MGR_NM;
                this.text_PHN_NO.Text = upvo.PHN_NO;
                this.text_FAX_NO.Text = upvo.FAX_NO;
                this.text_ESTM_RMK.Text = upvo.ESTM_RMK;
                this.text_ITM_FILE_NM.Text = upvo.ITM_FILE_NM;

            }
            catch (Exception eLog)
            {
                MessageBox.Show(eLog.Message, title, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        public Boolean ValueCheckd()
        {
            try
            {
                if (string.IsNullOrEmpty(this.text_CO_NM.Text))
                {
                    WinUIMessageBox.Show("[거래처명]을 입력하세요.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                    this.text_CO_NM.IsTabStop = true;
                    this.text_CO_NM.Focus();
                    return false;
                }

                if (string.IsNullOrEmpty(this.text_ESTM_DT.Text))
                {
                    WinUIMessageBox.Show("[견적 일자]을 입력하세요.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                    this.text_ESTM_DT.IsTabStop = true;
                    this.text_ESTM_DT.Focus();
                    return false;
                }

                return true;
            }
            catch (Exception eLog)
            {
                MessageBox.Show(eLog.Message, title, MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void btn_file_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var fileDialog = new DXOpenFileDialog();
                fileDialog.Filter = "Image Files|*.EMF;|All files|*.*";
                //fileDialog.FileName = "GGG";

                bool? result = fileDialog.ShowDialog();
                if (result == true)
                {
                    this.text_ITM_FILE_NM.Text = System.IO.Path.GetFileName(fileDialog.FileName);
                    this.updateDao.ITM_FILE = FileToByteArray(fileDialog.FileName);

                    //5MB
                    if (this.updateDao.ITM_FILE.Length > 5485760)
                    {
                        WinUIMessageBox.Show("[파일 크기] 5MB를 초과 하였습니다", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                        this.text_ITM_FILE_NM.Text = "";
                        this.updateDao.ITM_FILE = new byte[0];
                        return;
                    }
                }
            }
            catch (Exception eLog)
            {
                MessageBox.Show(eLog.Message, title, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        public byte[] FileToByteArray(string filePath)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                int length = Convert.ToInt32(fs.Length);
                BinaryReader br = new BinaryReader(fs);
                byte[] buff = br.ReadBytes(length);
                fs.Close();

                return buff;
            }
        }

        private void btn_delete_Click(object sender, RoutedEventArgs e)
        {
            this.text_ITM_FILE_NM.Text = "";
            this.updateDao.ITM_FILE = new byte[0];
        }
    }
}
