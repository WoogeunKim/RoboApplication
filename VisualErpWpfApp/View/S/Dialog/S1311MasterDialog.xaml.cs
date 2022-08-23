using DevExpress.Xpf.Core;
using DevExpress.Xpf.Core.Native;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Auth;
using ModelsLibrary.Code;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using AquilaErpWpfApp3.Util;

namespace AquilaErpWpfApp3.View.S.Dialog
{
    /// <summary>
    /// Interaction logic for ConfigView1Dialog.xaml
    /// </summary>
    public partial class S1311MasterDialog : DXWindow
    {
        private string _title = "개발 요청 등록";

        //private static CodeServiceClient codeClient = SystemProperties.CodeClient;
        
        private SystemCodeVo orgDao;

        private bool isEdit = false;
        private byte[] fileByte = null;
        private SystemCodeVo updateDao;

        public S1311MasterDialog(SystemCodeVo Dao)
        {
            InitializeComponent();
            //

            USER_CODE_VO();
            //this.combo_INP_ID.ItemsSource = SystemProperties.USER_CODE_VO();

            //this.btn_file.Click += btn_file_Click;
            //this.btn_delete.Click += btn_delete_Click;



            this.orgDao = Dao;
            SystemCodeVo copyDao = new SystemCodeVo()
            {
                NTC_NO = Dao.NTC_NO,
                NTC_SEQ = Dao.NTC_SEQ,
                TIT_DESC = Dao.TIT_DESC,
                SUBJ_DESC = Dao.SUBJ_DESC,
                INP_ID = Dao.INP_ID,
                INP_NM = Dao.INP_NM,
                CRE_DT = Dao.CRE_DT,
                FILE_NM = Dao.FILE_NM,
                NTC_CD = Dao.NTC_CD,
                NTC_FILE = Dao.NTC_FILE,
                NTC_CLSS_CD = Dao.NTC_CLSS_CD,
                NTC_STS_CD = Dao.NTC_STS_CD,
                NTC_ANS_DESC = Dao.NTC_ANS_DESC,
                CRE_USR_ID = Dao.CRE_USR_ID,
                UPD_USR_ID = Dao.UPD_USR_ID
            };

            //수정
            if (copyDao.NTC_NO != null)
            {
                this.isEdit = true;
                this.combo_INP_ID.IsReadOnly = true;
                this.combo_INP_ID.IsEnabled = false;

                this.lab_INP_ID.Foreground = new SolidColorBrush(Colors.DarkGray);
            }
            else
            {
                //추가
                //this.text_ClssTpCd.IsReadOnly = false;
                //this.isEdit = false;
                //copyDao.DELT_FLG = "사용";
            }

            this.configCode.DataContext = copyDao;
            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
            //this.OKButton.Click += new RoutedEventHandler(OKButton_Click);
            //this.CancelButton.Click += new RoutedEventHandler(CancelButton_Click);
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
                    this.updateDao.NTC_STS_CD = "R";
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s1310/i", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
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

                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s1310/u", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
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


                //SystemCodeVo resultVo;
                //if (isEdit == false)
                //{
                //    this.updateDao = getDomain();//this.updateDao
                //    this.updateDao.NTC_STS_CD = "R";

                //    resultVo = codeClient.S1310InsertMst(this.updateDao);
                //    if (!resultVo.isSuccess)
                //    {
                //        //실패
                //        WinUIMessageBox.Show(resultVo.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + this._title, MessageBoxButton.OK, MessageBoxImage.Error);
                //        return;
                //    }
                //    //성공
                //    WinUIMessageBox.Show("완료 되었습니다", "[추가]" + this._title, MessageBoxButton.OK, MessageBoxImage.Information);
                //}
                //else
                //{
                //    this.updateDao = getDomain();
                //    resultVo = codeClient.S1310UpdateMst(this.updateDao);
                //    if (!resultVo.isSuccess)
                //    {
                //        //실패
                //        WinUIMessageBox.Show(resultVo.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + this._title, MessageBoxButton.OK, MessageBoxImage.Error);
                //        return;
                //    }
                //    //성공
                //    WinUIMessageBox.Show("완료 되었습니다", "[수정]" + this._title, MessageBoxButton.OK, MessageBoxImage.Information);
                //    //
                //    this.orgDao.NTC_NO = this.updateDao.NTC_NO;
                //    this.orgDao.NTC_SEQ = this.updateDao.NTC_SEQ;
                //    this.orgDao.TIT_DESC = this.updateDao.TIT_DESC;
                //    this.orgDao.SUBJ_DESC = this.updateDao.SUBJ_DESC;
                //    this.orgDao.INP_ID = this.updateDao.INP_ID;
                //    this.orgDao.INP_NM = this.updateDao.INP_NM;
                //    this.orgDao.FILE_NM = this.updateDao.FILE_NM;
                //    this.orgDao.NTC_FILE = this.updateDao.NTC_FILE;
                //    this.orgDao.NTC_CLSS_CD = this.updateDao.NTC_CLSS_CD;
                //    this.orgDao.NTC_STS_CD = this.updateDao.NTC_STS_CD;
                //    this.orgDao.NTC_ANS_DESC = this.updateDao.NTC_ANS_DESC;
                //    this.orgDao.NTC_CD = this.updateDao.NTC_CD;
                //    this.orgDao.CRE_USR_ID = this.updateDao.CRE_USR_ID;
                //    this.orgDao.UPD_USR_ID = this.updateDao.UPD_USR_ID;
                //}
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
            if (string.IsNullOrEmpty(this.text_TIT_DESC.Text))
            {
                WinUIMessageBox.Show("[제 목] 입력 값이 맞지 않습니다.", "[유효검사]" + this._title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.text_TIT_DESC.IsTabStop = true;
                this.text_TIT_DESC.Focus();
                return false;
            }
            else if (string.IsNullOrEmpty(this.combo_INP_ID.Text))
            {
                WinUIMessageBox.Show("[작성자] 입력 값이 맞지 않습니다.", "[유효검사]" + this._title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.combo_INP_ID.IsTabStop = true;
                this.combo_INP_ID.Focus();
                return false;
            }
            else if (this.richEdit.RtfText.Length > 2197152)
            {
                WinUIMessageBox.Show("[첨부 파일 보관]내용 크기가 2Mbyte 초과 하였습니다.", "[유효검사]" + this._title, MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }
            //else if (this.fileByte.Length > 5485760)
            //{
            //    WinUIMessageBox.Show("[압축 보관] 파일 크기가 5Mbyte 초과 하였습니다.", "[유효검사]" + this._title, MessageBoxButton.OK, MessageBoxImage.Information);
            //    return false;
            //}
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
            Dao.NTC_NO = this.orgDao.NTC_NO;
            Dao.NTC_SEQ = 1;
            Dao.TIT_DESC = this.text_TIT_DESC.Text;
            Dao.SUBJ_DESC = this.richEdit.RtfText;

            GroupUserVo inpIdVo = this.combo_INP_ID.SelectedItem as GroupUserVo;
            if (inpIdVo != null)
            {
                Dao.INP_ID = inpIdVo.USR_ID;
                Dao.INP_NM = inpIdVo.USR_N1ST_NM;
            }

            Dao.FILE_NM = this.text_FILE_NM.Text;
            if (fileByte == null)
            {
                Dao.NTC_FILE = new byte[0];
            }
            else
            {
                Dao.NTC_FILE = this.fileByte;
            }

            Dao.NTC_CLSS_CD = "B";
            Dao.NTC_CD = "B";

            Dao.NTC_STS_CD = this.orgDao.NTC_STS_CD;
            Dao.NTC_ANS_DESC = this.orgDao.NTC_ANS_DESC;

            Dao.CRE_USR_ID = SystemProperties.USER;
            Dao.UPD_USR_ID = SystemProperties.USER;
            Dao.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
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

        void btn_delete_Click(object sender, RoutedEventArgs e)
        {
            this.text_FILE_NM.Text = "";
            this.fileByte = new byte[0];
        }

        void btn_file_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            //dlg.DefaultExt = ".xls";
            dlg.Filter = "All Files|*.*";
            dlg.Title = "파일를 선택 해 주세요.";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                string filename = dlg.FileName;
                //this.text_Path.Text = filename;
                this.text_FILE_NM.Text = Path.GetFileName(filename);
                this.fileByte = FileToByteArray(filename);

                //5MB
                if (fileByte.Length > 5485760)
                {
                    WinUIMessageBox.Show("[파일 크기] 5MB를 초과 하였습니다", "[유효검사]" + this._title, MessageBoxButton.OK, MessageBoxImage.Warning);
                    this.text_FILE_NM.Text = "";
                    this.fileByte = new byte[0];
                    return;
                }
            }
        }

        private byte[] ImageToByte(ImageSource img)
        {
            if (img == null)
            {
                //WinUIMessageBox.Show("이미지 등록을 권장 합니다.", "[유효검사]장비 등록", MessageBoxButton.OK, MessageBoxImage.Information);
                return new byte[0];
            }
            BitmapImage biImg = img as BitmapImage;
            Stream stream = biImg.StreamSource;
            return stream.GetDataFromStream();
        }


        public byte[] FileToByteArray(string filePath)
        {
            try
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
            catch 
            {
                return new byte[0];
            }
        }

        public async void USER_CODE_VO()
        {
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s136/usr", new StringContent(JsonConvert.SerializeObject(new GroupUserVo() { DELT_FLG = "N", CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    this.combo_INP_ID.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<GroupUserVo>>(await response.Content.ReadAsStringAsync()).Cast<GroupUserVo>().ToList();
                }
            }
            //return new List<ProgramVo>();
            //IList<ProgramVo> MenuVo = authClient.SelectProgramMenuTotalList(new ProgramVo() { PRNT_MDL_ID = "PRNT_MDL_ID", CHNL_CD = SystemProperties.USER_VO.CHNL_CD });
            //IList<CodeDao> resultMap = new List<CodeDao>();
            //int nCnt = MenuVo.Count;
            //ProgramVo tmpVo;
            //resultMap.Clear();
            //for (int x = 0; x < nCnt; x++)
            //{
            //    tmpVo = MenuVo[x];
            //    resultMap.Add(new CodeDao() { CLSS_CD = tmpVo.MDL_ID, CLSS_DESC = tmpVo.MDL_NM });
            //}
            ////
            //resultMap.Insert(0, new CodeDao() { CLSS_CD = "T", CLSS_DESC = "최상위 메뉴" });
            //return resultMap;
        }
    }
}
