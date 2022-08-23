using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Man;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;
using AquilaErpWpfApp3.Util;

namespace AquilaErpWpfApp3.View.M.Dialog
{
    /// <summary>
    /// Interaction logic for ConfigView1Dialog.xaml
    /// </summary>
    public partial class M6681WrkDialog : DXWindow
    {
        //private static ItemCodeServiceClient itemClient = SystemProperties.ItemClient;
        //private static MakeServiceClient makeClient = SystemProperties.MakeClient;
        private string _title = "제품도면관리(작업표준서)";

        private ManVo orgDao;
        private bool isEdit = false;
        private byte[] fileByte = null;
        public ManVo updateDao;

        public string _ITM_CD;
        //private int? _ITM_DWG_SEQ;

        public M6681WrkDialog(ManVo Dao)
        {
            InitializeComponent();

            //this.combo_PROD_LOC_CD.ItemsSource = SystemProperties.SYSTEM_CODE_VO("L-100");
            //this.combo_ITM_GRP_CLSS_CD.ItemsSource = SystemProperties.SYSTEM_CODE_VO("L-001");
            //this.combo_UOM_CD.ItemsSource = SystemProperties.SYSTEM_CODE_VO("L-003");
            ////this.combo_QLTY_INSP_CD.ItemsSource = SystemProperties.SYSTEM_CODE_VO("L-004");
            ////this.combo_ABC_CD.ItemsSource = SystemProperties.SYSTEM_CODE_VO("L-200");
            //this.combo_MTRL_NM_NM.ItemsSource = SystemProperties.SYSTEM_CODE_VO("C-001");
            //this.combo_HD_LEN_NM.ItemsSource = SystemProperties.SYSTEM_CODE_VO("C-002");
            //this.combo_SCRW_LEN_NM.ItemsSource = SystemProperties.SYSTEM_CODE_VO("C-003");
            //this.combo_SKIN_HDL_NM.ItemsSource = SystemProperties.SYSTEM_CODE_VO("C-004");


            //this.combo_ITM_GRP_CLSS_CD.SelectedIndexChanged += combo_ITM_GRP_CLSS_CD_SelectedIndexChanged;

            //this.combo_N1ST_ITM_GRP_CD.SelectedIndexChanged += combo_N1ST_ITM_GRP_CD_SelectedIndexChanged;

            
            this.btn_file.Click += btn_file_Click;
            this.btn_delete.Click += btn_delete_Click;


            this.orgDao = Dao;
            //
            ManVo copyDao = new ManVo()
            {
                ITM_CD = Dao.ITM_CD,
                ITM_NM = Dao.ITM_NM,
                ITM_WRK_DOC_SEQ = Dao.ITM_WRK_DOC_SEQ,
                WRK_DOC_NM = Dao.WRK_DOC_NM,
                WRK_DOC_FILE = Dao.WRK_DOC_FILE,
                WRK_DOC_FILE_NM = Dao.WRK_DOC_FILE_NM,
                WRK_DOC_RMK = Dao.WRK_DOC_RMK,
                DELT_FLG = Dao.DELT_FLG,
                CRE_USR_ID = Dao.CRE_USR_ID,
                UPD_USR_ID = Dao.UPD_USR_ID
            };

            //
            //////수정
            //if (Dao.ITM_CD != null)
            //{
            //    this.isEdit = true;
            //    this.fileByte = Dao.DWG_FILE;
            //    this._ITM_CD = Dao.ITM_CD;
            //    this._ITM_DWG_SEQ = Dao.ITM_DWG_SEQ;
            //    //if (Dao.DWG_FILE != null)
            //    //{
            //    //    BitmapImage biImg = new BitmapImage();
            //    //    MemoryStream ms = new MemoryStream(Dao.DWG_FILE);
            //    //    biImg.BeginInit();
            //    //    biImg.StreamSource = ms;
            //    //    biImg.EndInit();
            //    //    this.text_Image.Source = biImg;
            //    //}
            //}
            //else
            //{
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

        void btn_delete_Click(object sender, RoutedEventArgs e)
        {
            this.text_EQ_FILE_NM.Text = "";
            this.fileByte = new byte[0];
        }

        void btn_file_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            //dlg.DefaultExt = ".xls";
            dlg.Filter = "Image files (*.tif, *.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.tif; *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
            dlg.Title = "파일를 선택 해 주세요.";
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

        #region Functon (OKButton_Click, CancelButton_Click)
        private async void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValueCheckd())
            {
                int _Num = 0;
                if (isEdit == false)
                {
                    this.updateDao = getDomain();//this.updateDao
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("m6681/wrk/i", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
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

                    //    resultVo = makeClient.P4413_InsertEq(getDomain());
                    //    if (!resultVo.isSuccess)
                    //    {
                    //        //실패
                    //        WinUIMessageBox.Show(resultVo.Message, "[에러]설비도면이력관리", MessageBoxButton.OK, MessageBoxImage.Error);
                    //        return;
                    //    }
                    //    //성공
                    //    WinUIMessageBox.Show("완료 되었습니다", "설비도면이력관리", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                //    //resultVo = makeClient.P4411_UpdateDtl(getDomain());
                //    //if (!resultVo.isSuccess)
                //    //{
                //    //    실패
                //    //    WinUIMessageBox.Show(resultVo.Message, "[에러]도면이력관리", MessageBoxButton.OK, MessageBoxImage.Error);
                //    //    return;
                //    //}
                //    //성공
                //    //WinUIMessageBox.Show("완료 되었습니다", "도면이력관리", MessageBoxButton.OK, MessageBoxImage.Information);
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
        #endregion

        #region Functon (ValueCheckd)
        public Boolean ValueCheckd()
        {
            if (string.IsNullOrEmpty(this.text_EQ_NM.Text))
            {
                WinUIMessageBox.Show("[작업표준서명] 입력 값이 맞지 않습니다.", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.text_EQ_NM.IsTabStop = true;
                this.text_EQ_NM.Focus();
                return false;
            }
            //else if (string.IsNullOrEmpty(this.text_ITM_NM.Text))
            //{
            //    WinUIMessageBox.Show("[물품 명] 입력 값이 맞지 않습니다.", "[유효검사]품목 마스터 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.text_ITM_NM.IsTabStop = true;
            //    this.text_ITM_NM.Focus();
            //    return false;
            //}
            //else if (string.IsNullOrEmpty(this.combo_PROD_LOC_CD.Text))
            //{
            //    WinUIMessageBox.Show("[저장소 번호] 입력 값이 맞지 않습니다.", "[유효검사]품목 마스터 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.combo_PROD_LOC_CD.IsTabStop = true;
            //    this.combo_PROD_LOC_CD.Focus();
            //    return false;
            //}
            //else if (string.IsNullOrEmpty(this.combo_ITM_GRP_CLSS_CD.Text))
            //{
            //    WinUIMessageBox.Show("[분류] 입력 값이 맞지 않습니다.", "[유효검사]품목 마스터 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.combo_ITM_GRP_CLSS_CD.IsTabStop = true;
            //    this.combo_ITM_GRP_CLSS_CD.Focus();
            //    return false;
            //}
            //else if (string.IsNullOrEmpty(this.combo_N1ST_ITM_GRP_CD.Text))
            //{
            //    WinUIMessageBox.Show("[대 그룹] 입력 값이 맞지 않습니다.", "[유효검사]품목 마스터 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.combo_N1ST_ITM_GRP_CD.IsTabStop = true;
            //    this.combo_N1ST_ITM_GRP_CD.Focus();
            //    return false;
            //}
            //else if (string.IsNullOrEmpty(this.combo_N2ND_ITM_GRP_CD.Text))
            //{
            //    WinUIMessageBox.Show("[중 그룹] 입력 값이 맞지 않습니다.", "[유효검사]품목 마스터 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.combo_N2ND_ITM_GRP_CD.IsTabStop = true;
            //    this.combo_N2ND_ITM_GRP_CD.Focus();
            //    return false;
            //}
            //else if (string.IsNullOrEmpty(this.combo_UOM_CD.Text))
            //{
            //    WinUIMessageBox.Show("[단위] 입력 값이 맞지 않습니다.", "[유효검사]품목 마스터 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.combo_UOM_CD.IsTabStop = true;
            //    this.combo_UOM_CD.Focus();
            //    return false;
            //}
            //else if (string.IsNullOrEmpty(this.combo_ABC_CD.Text))
            //{
            //    WinUIMessageBox.Show("[ABC 등급] 입력 값이 맞지 않습니다.", "[유효검사]품목 마스터 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.combo_ABC_CD.IsTabStop = true;
            //    this.combo_ABC_CD.Focus();
            //    return false;
            //}
            //else if (string.IsNullOrEmpty(this.combo_QLTY_INSP_CD.Text))
            //{
            //    WinUIMessageBox.Show("[품질 검사 유형] 입력 값이 맞지 않습니다.", "[유효검사]품목 마스터 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.combo_QLTY_INSP_CD.IsTabStop = true;
            //    this.combo_QLTY_INSP_CD.Focus();
            //    return false;
            //}
            //else if (ImageToByte(this.text_Image.Source).Length > 2197152)
            //{
            //    WinUIMessageBox.Show("이미지 파일 크기가 2Mbyte 초과 하였습니다.", "[유효검사]품목 마스터 관리", MessageBoxButton.OK, MessageBoxImage.Information);
            //    return false;
            //}
            else
            {
                //if (this.isEdit == false)
                //{
                //    SystemCodeVo dao = new SystemCodeVo()
                //    {
                //        ITM_CD = this.text_ITM_CD.Text,
                //    };
                //    IList<SystemCodeVo> daoList = (IList<SystemCodeVo>)itemClient.SelectItemList(dao);
                //    if (daoList.Count != 0)
                //    {
                //        WinUIMessageBox.Show("[코드 - 중복] 코드를 다시 입력 하십시오.", "[중복검사]품목 마스터 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
                //        this.text_ITM_CD.IsTabStop = true;
                //        this.text_ITM_CD.Focus();
                //        return false;
                //    }
                //}
            }
            return true;
        }
        #endregion

        #region Functon (getDomain - CustomerCodeVo)
        private ManVo getDomain()
        {
            ManVo Dao = new ManVo();

            Dao.ITM_CD = this._ITM_CD;
            //Dao.ITM_DWG_SEQ = this._ITM_DWG_SEQ;

            Dao.WRK_DOC_NM = this.text_EQ_NM.Text;
            Dao.WRK_DOC_FILE_NM = this.text_EQ_FILE_NM.Text;
            Dao.WRK_DOC_RMK = this.text_EQ_RMK.Text;
            Dao.DELT_FLG = "N";

            Dao.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
            Dao.CRE_USR_ID = SystemProperties.USER;
            Dao.UPD_USR_ID = SystemProperties.USER;

            //
            Dao.WRK_DOC_FILE_NM = this.text_EQ_FILE_NM.Text;
            if (fileByte == null)
            {
                Dao.WRK_DOC_FILE = new byte[0];
            }
            else
            {
                Dao.WRK_DOC_FILE = this.fileByte;
            }

            return Dao;
        }
        #endregion

        //IsEdit
        public bool IsEdit
        {
            get
            {
                return this.isEdit;
            }
        }

        //private byte[] ImageToByte(ImageSource img)
        //{
        //    if (img == null)
        //    {
        //        //WinUIMessageBox.Show("이미지 등록을 권장 합니다.", "[유효검사]장비 등록", MessageBoxButton.OK, MessageBoxImage.Information);
        //        return new byte[0];
        //    }
        //    BitmapImage biImg = img as BitmapImage;
        //    Stream stream = biImg.StreamSource;
        //    return stream.GetDataFromStream();
        //}


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
    }
}
