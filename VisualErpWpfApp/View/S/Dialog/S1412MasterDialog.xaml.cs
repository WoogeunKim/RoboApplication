using AquilaErpWpfApp3.Util;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Spreadsheet;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Code;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;

namespace AquilaErpWpfApp3.View.S.Dialog
{
    /// <summary>
    /// Interaction logic for ConfigView1Dialog.xaml
    /// </summary>
    public partial class S1412MasterDialog : DXWindow
    {
        //private static CodeServiceClient codeClient = SystemProperties.CodeClient;
        private string _title = "보고서 관리";

        private SystemCodeVo orgDao;

        private bool isEdit = false;
        //private byte[] fileByte = null;
        private SystemCodeVo updateDao;

        private System.IO.Stream streamFile;

        public S1412MasterDialog(SystemCodeVo Dao)
        {
            InitializeComponent();
            //

            //this.combo_INP_ID.ItemsSource = SystemProperties.USER_CODE_VO();

            //USER_CODE_VO();

            //this.btn_file.Click += btn_file_Click;
            //this.btn_delete.Click += btn_delete_Click;



            this.orgDao = Dao;
            SystemCodeVo copyDao = new SystemCodeVo()
            {
                RPT_CD = Dao.RPT_CD,
                RPT_NM = Dao.RPT_NM,
                RPT_FILE = Dao.RPT_FILE,
                CHNL_CD = Dao.CHNL_CD,
                CRE_USR_ID = Dao.CRE_USR_ID,
                UPD_USR_ID = Dao.UPD_USR_ID
            };

            //수정
            if (copyDao.RPT_CD != null)
            {

                this.text_RPT_CD.IsReadOnly = true;
                //this.fileByte = Dao.RPT_FILE;
                this.isEdit = true;
                this.streamFile = new System.IO.MemoryStream(Dao.RPT_FILE);
                this.spreadsheetControl1.LoadDocument(streamFile, DevExpress.Spreadsheet.DocumentFormat.Xlsx);
            }
            else
            {
                //추가
                //this.text_RPT_CD.IsReadOnly = false;
                //this.isEdit = false;
                //copyDao.DELT_FLG = "사용";
            }

            this.configCode.DataContext = copyDao;
            //this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
            this.OKButton.Click += new RoutedEventHandler(OKButton_Click);
            this.CancelButton.Click += new RoutedEventHandler(CancelButton_Click);
        }



        #region Functon (OKButton_Click, CancelButton_Click)
        private async void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValueCheckd())
            {
                int _Num = 0;
                //ProgramVo resultVo;
                if (isEdit == false)
                {
                    this.updateDao = getDomain();//this.updateDao
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s1412/mst/i", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
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
                    this.updateDao = getDomain();

                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s1412/mst/u", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
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
            if (string.IsNullOrEmpty(this.text_RPT_CD.Text))
            {
                WinUIMessageBox.Show("[코드] 입력 값이 맞지 않습니다.", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.text_RPT_CD.IsTabStop = true;
                this.text_RPT_CD.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(this.text_RPT_NM.Text))
            {
                WinUIMessageBox.Show("[코드 설명] 입력 값이 맞지 않습니다.", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.text_RPT_NM.IsTabStop = true;
                this.text_RPT_NM.Focus();
                return false;
            }
            //else
            //{
            //    if (this.isEdit == false)
            //    {
            //        ItemGroupCodeVo dao = new ItemGroupCodeVo()
            //        {
            //            ITM_GRP_CD = this.text_ItmGrpCd.Text,
            //            PRNT_ITM_GRP_CD = "T",
            //            CRE_USR_ID = "dup"
            //        };
            //        IList<ItemGroupCodeVo> daoList = (IList<ItemGroupCodeVo>)itemClient.SelectCodeItemGroupList(dao);
            //        if (daoList.Count != 0)
            //        {
            //            WinUIMessageBox.Show("[코드 - 중복] 코드를 다시 입력 하십시오.", "[중복검사]공지 사항 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
            //            this.text_ItmGrpCd.IsTabStop = true;
            //            this.text_ItmGrpCd.Focus();
            //            return false;
            //        }
            //    }
            //}
            return true;
        }
        #endregion

        #region Functon (getDomain - ConfigView1Dao)
        private SystemCodeVo getDomain()
        {
            SystemCodeVo Dao = new SystemCodeVo();
            Dao.RPT_CD = this.text_RPT_CD.Text;
            Dao.RPT_NM = this.text_RPT_NM.Text;

            Dao.RPT_FLG = "X";
            Dao.RPT_FILE = this.spreadsheetControl1.Document.SaveDocument(DevExpress.Spreadsheet.DocumentFormat.Xlsx);
            //Dao.TIT_DESC = this.text_TIT_DESC.Text;
            //Dao.SUBJ_DESC = this.richEdit.RtfText;
            //Dao.RPT_FILE = this.spreadsheetControl1.sa
            //using (System.IO.Stream streamFile = new FileStream("Documents\\SavedDocument.xlsx", FileMode.Create, FileAccess.ReadWrite))
            //{

            //}


            //this.spreadsheetControl1.SaveDocument();


            //streamFile = new System.IO.MemoryStream(Dao.RPT_FILE);
            //this.spreadsheetControl1.SaveDocument(streamFile, DocumentFormat.);

            //GroupUserVo inpIdVo = this.combo_INP_ID.SelectedItem as GroupUserVo;
            //if (inpIdVo != null)
            //{
            //    Dao.INP_ID = inpIdVo.USR_ID;
            //    Dao.INP_NM = inpIdVo.USR_N1ST_NM;
            //}

            //Dao.FILE_NM = this.text_FILE_NM.Text;
            //if (fileByte == null)
            //{
            //    Dao.NTC_FILE = new byte[0];
            //}
            //else
            //{
            //    Dao.NTC_FILE = this.fileByte;
            //}


            Dao.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
            Dao.CRE_USR_ID = SystemProperties.USER;
            Dao.UPD_USR_ID = SystemProperties.USER;
            return Dao;
        }
        #endregion


        public SystemCodeVo ResultVo
        {
            get
            {
                return this.updateDao;
            }
        }

        //IsEdit
        public bool IsEdit
        {
            get
            {
                return this.isEdit;
            }
        }

        //void btn_delete_Click(object sender, RoutedEventArgs e)
        //{
        //    this.text_FILE_NM.Text = "";
        //    this.fileByte = new byte[0];
        //}

        //void btn_file_Click(object sender, RoutedEventArgs e)
        //{
        //    DevExpress.Xpf.Dialogs.DXOpenFileDialog dlg = new DevExpress.Xpf.Dialogs.DXOpenFileDialog();
        //    //Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
        //    //dlg.DefaultExt = ".xls";
        //    dlg.Filter = "All Files|*.*";
        //    dlg.Title = "파일를 선택 해 주세요.";
        //    Nullable<bool> result = dlg.ShowDialog();
        //    if (result == true)
        //    {
        //        string filename = dlg.FileName;
        //        //this.text_Path.Text = filename;
        //        this.text_FILE_NM.Text = Path.GetFileName(filename);
        //        this.fileByte = FileToByteArray(filename);

        //        //5MB
        //        if (fileByte.Length > 5485760)
        //        {
        //            WinUIMessageBox.Show("[파일 크기] 5MB를 초과 하였습니다", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
        //            this.text_FILE_NM.Text = "";
        //            this.fileByte = new byte[0];
        //            return;
        //        }
        //    }
        //}

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


        //public byte[] FileToByteArray(string filePath)
        //{
        //    using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
        //    {
        //        int length = Convert.ToInt32(fs.Length);
        //        BinaryReader br = new BinaryReader(fs);
        //        byte[] buff = br.ReadBytes(length);
        //        fs.Close();

        //        return buff;
        //    }
        //}



        //public async void USER_CODE_VO()
        //{
        //    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s136/usr", new StringContent(JsonConvert.SerializeObject(new GroupUserVo() { DELT_FLG = "N", CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
        //    {
        //        if (response.IsSuccessStatusCode)
        //        {
        //            this.combo_INP_ID.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<GroupUserVo>>(await response.Content.ReadAsStringAsync()).Cast<GroupUserVo>().ToList();
        //        }
        //    }
        //    //return new List<ProgramVo>();
        //    //IList<ProgramVo> MenuVo = authClient.SelectProgramMenuTotalList(new ProgramVo() { PRNT_MDL_ID = "PRNT_MDL_ID", CHNL_CD = SystemProperties.USER_VO.CHNL_CD });
        //    //IList<CodeDao> resultMap = new List<CodeDao>();
        //    //int nCnt = MenuVo.Count;
        //    //ProgramVo tmpVo;
        //    //resultMap.Clear();
        //    //for (int x = 0; x < nCnt; x++)
        //    //{
        //    //    tmpVo = MenuVo[x];
        //    //    resultMap.Add(new CodeDao() { CLSS_CD = tmpVo.MDL_ID, CLSS_DESC = tmpVo.MDL_NM });
        //    //}
        //    ////
        //    //resultMap.Insert(0, new CodeDao() { CLSS_CD = "T", CLSS_DESC = "최상위 메뉴" });
        //    //return resultMap;
        //}

    }
}
