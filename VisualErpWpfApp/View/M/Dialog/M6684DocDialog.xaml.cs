using AquilaErpWpfApp3.Util;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Man;
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

namespace AquilaErpWpfApp3.View.M.Dialog
{
    /// <summary>
    /// M6684DocDialog.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class M6684DocDialog : DXWindow
    {
        private string _title = "문서관리";

        private ManVo orgDao;
        private bool isEdit = false;
        private byte[] fileByte = null;
        public ManVo updateDao;

        public string _ITM_CD;

        //public M6684DocDialog()
        //{
        //    InitializeComponent();
        //}

        public M6684DocDialog(ManVo Dao)
        {
            InitializeComponent();



            this.btn_file.Click += btn_file_Click;
            this.btn_delete.Click += btn_delete_Click;


            this.orgDao = Dao;
            //
            ManVo copyDao = new ManVo()
            {
                ITM_CD = Dao.ITM_CD,
                ITM_NM = Dao.ITM_NM,
                ITM_DWG_SEQ = Dao.ITM_DWG_SEQ,
                DWG_NM = Dao.DWG_NM,
                DWG_FILE = Dao.DWG_FILE,
                DWG_FILE_NM = Dao.DWG_FILE_NM,
                DWG_RMK = Dao.DWG_RMK,
                DELT_FLG = Dao.DELT_FLG,
                CRE_USR_ID = Dao.CRE_USR_ID,
                UPD_USR_ID = Dao.UPD_USR_ID
            };

            //추가
            this.isEdit = false;
            copyDao.DELT_FLG = "N";

            this.fileByte = new byte[0];
            this._ITM_CD = Dao.ITM_CD;
            //this._ITM_DWG_SEQ = Dao.ITM_DWG_SEQ;
            //Dao.JOIN_CO_DT = System.DateTime.Now.ToString("yyyy-MM-dd");
            //}
            this.configCode.DataContext = copyDao;
            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
            this.OKButton.Click += new RoutedEventHandler(OKButton_Click);
            this.CancelButton.Click += new RoutedEventHandler(CancelButton_Click);
        }



        private async void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValueCheckd())
            {
                int _Num = 0;
                if (isEdit == false)
                {
                    this.updateDao = getDomain();//this.updateDao
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6684/doc/i", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
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
                            WinUIMessageBox.Show("완료 되었습니다", _title, MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }

               
                }
                else
                {

                }
                this.DialogResult = true;
                this.Close();
            }
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
                Close();
            }
        }

        private void btn_delete_Click(object sender, RoutedEventArgs e)
        {
            this.text_EQ_FILE_NM.Text = "";
            this.fileByte = new byte[0];
        }

        private void btn_file_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            //dlg.DefaultExt = ".xls";
            //dlg.Filter = "Image files (*.tif, *.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.tif; *.jpg; *.jpeg; *.jpe; *.jfif; *.png";

            // 엑셀, 워드, 한글, 텍스트파일, All files
            dlg.Filter = "All files|*.*|Excel files(*.xlsx, *.xls, *.xlsm)|*.xlsx; *.xls; *.xlsm|PDF files(*.pdf)|*.pdf|Text files(*.txt)|*.txt";
            dlg.Title = "파일을 선택 해 주세요";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                string filename = dlg.FileName;
                //this.text_Path.Text = filename;
                this.text_EQ_FILE_NM.Text = Path.GetFileName(filename);
                this.fileByte = FileToByteArray(filename);

                //5MB
                if (fileByte.Length > 3485760)
                {
                    WinUIMessageBox.Show("[파일 크기] 3MB를 초과 하였습니다", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                    this.text_EQ_FILE_NM.Text = "";
                    this.fileByte = new byte[0];
                    return;
                }
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




        public Boolean ValueCheckd()
        {
            if (string.IsNullOrEmpty(this.text_EQ_NM.Text))
            {
                WinUIMessageBox.Show("[제품도면명] 입력 값이 맞지 않습니다.", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.text_EQ_NM.IsTabStop = true;
                this.text_EQ_NM.Focus();
                return false;
            }
           
            else
            {
               
            }
            return true;
        }


        private ManVo getDomain()
        {
            ManVo Dao = new ManVo();

            Dao.ITM_CD = this._ITM_CD;
            //Dao.ITM_DWG_SEQ = this._ITM_DWG_SEQ;

            Dao.DWG_NM = this.text_EQ_NM.Text;
            Dao.DWG_FILE_NM = this.text_EQ_FILE_NM.Text;
            Dao.DWG_RMK = this.text_EQ_RMK.Text;
            Dao.DELT_FLG = "N";

            Dao.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
            Dao.CRE_USR_ID = SystemProperties.USER;
            Dao.UPD_USR_ID = SystemProperties.USER;

            //
            Dao.DWG_FILE_NM = this.text_EQ_FILE_NM.Text;
            if (fileByte == null)
            {
                Dao.DWG_FILE = new byte[0];
            }
            else
            {
                Dao.DWG_FILE = this.fileByte;
            }

            return Dao;
        }
    }
}
