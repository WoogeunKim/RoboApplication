using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Auth;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;
using AquilaErpWpfApp3.Util;

namespace AquilaErpWpfApp3.View.S.Dialog
{
    /// <summary>
    /// Interaction logic for ConfigView1Dialog.xaml
    /// </summary>
    public partial class S136GroupDialog : DXWindow
    {
        private string title = "사용자 권한 관리";
        //private static AuthorServiceClient authClient = SystemProperties.AuthClient;
        private bool isEdit = false;
        private string GRP_ID = string.Empty;

        private GroupUserVo updateDao;

        public S136GroupDialog(GroupUserVo Dao)
        {
            InitializeComponent();

            GROUP_VO();
            //this.combo_PRNT_GRP_NM.ItemsSource = GROUP_VO();

            ////수정
            if (Dao.GRP_NM != null)
            {
                this.isEdit = true;
                this.GRP_ID = Dao.GRP_ID;
            }
            else
            {
                //추가
                this.isEdit = false;
            }
            this.configCode.DataContext = Dao;
            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
            this.OKButton.Click += new RoutedEventHandler(OKButton_Click);
            this.CancelButton.Click += new RoutedEventHandler(CancelButton_Click);
        }

        #region Functon (OKButton_Click, CancelButton_Click)
        private async void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValueCheckd())
            {
                int _Num = 0;
                //SystemCodeVo resultVo;
                if (isEdit == false)
                {
                    this.updateDao = getDomain();

                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s136/g/i", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
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
                    this.updateDao = getDomain();

                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s136/g/u", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
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

                //GroupUserVo resultVo;
                //if (isEdit == false)
                //{
                //    resultVo = authClient.InsertGroupCode(getDomain());
                //    if (!resultVo.isSuccess)
                //    {
                //        //실패
                //        WinUIMessageBox.Show(resultVo.Message, "[" + SystemProperties.PROGRAM_TITLE + "]사용자 권한 관리", MessageBoxButton.OK, MessageBoxImage.Error);
                //        return;
                //    }
                //    //성공
                //    WinUIMessageBox.Show("완료 되었습니다", "[추가]사용자 권한 관리", MessageBoxButton.OK, MessageBoxImage.Information);
                //}
                //else
                //{
                //    resultVo = authClient.UpdateGroupCode(getDomain());
                //    if (!resultVo.isSuccess)
                //    {
                //        //실패
                //        WinUIMessageBox.Show(resultVo.Message, "[" + SystemProperties.PROGRAM_TITLE + "]사용자 권한 관리", MessageBoxButton.OK, MessageBoxImage.Error);
                //        return;
                //    }
                //    //성공
                //    WinUIMessageBox.Show("완료 되었습니다", "[수정]사용자 권한 관리", MessageBoxButton.OK, MessageBoxImage.Information);
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
            if (string.IsNullOrEmpty(this.text_GRP_NM.Text))
            {
                WinUIMessageBox.Show("[그룹 명] 입력 값이 맞지 않습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.text_GRP_NM.IsTabStop = true;
                this.text_GRP_NM.Focus();
                return false;
            }
            //else
            //{
            //    if (this.isEdit == false)
            //    {
            //        SystemCodeVo dao = new SystemCodeVo()
            //        {
            //            CLSS_TP_CD = this.text_ClssTpCd.Text,
            //        };
            //        IList<SystemCodeVo> daoList = (IList<SystemCodeVo>)codeClient.SelectMasterCode(dao);
            //        if (daoList.Count != 0)
            //        {
            //            WinUIMessageBox.Show("[코드 - 중복] 코드를 다시 입력 하십시오.", "[중복검사]시스템 분류 코드", MessageBoxButton.OK, MessageBoxImage.Warning);
            //            this.text_ClssTpCd.IsTabStop = true;
            //            this.text_ClssTpCd.Focus();
            //            return false;
            //        }
            //    }
            //}
            return true;
        }
        #endregion

        #region Functon (getDomain - ConfigView1Dao)
        private GroupUserVo getDomain()
        {
            GroupUserVo Dao = new GroupUserVo();

            GroupUserVo PrntGrpNmVo = this.combo_PRNT_GRP_NM.SelectedItem as GroupUserVo;
            if (PrntGrpNmVo != null)
            {
                Dao.PRNT_GRP_ID = PrntGrpNmVo.GRP_ID;
                Dao.PRNT_GRP_NM = PrntGrpNmVo.GRP_NM;
            }
            //
            Dao.GRP_NM = this.text_GRP_NM.Text;
            Dao.GRP_DESC = this.text_GRP_DESC.Text;
            Dao.GRP_TP_CD = "G";
            Dao.CRE_USR_ID = SystemProperties.USER;
            Dao.UPD_USR_ID = SystemProperties.USER;
            Dao.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
            if (this.isEdit)
            {
                Dao.GRP_ID = this.GRP_ID;
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

        public async void GROUP_VO()
        {
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s136/g", new StringContent(JsonConvert.SerializeObject(new GroupUserVo() { GRP_TP_CD = "G", CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    List<GroupUserVo> _groupList =  JsonConvert.DeserializeObject<IEnumerable<GroupUserVo>>(await response.Content.ReadAsStringAsync()).Cast<GroupUserVo>().ToList();
                    _groupList.Insert(0, new GroupUserVo() { GRP_ID = null, GRP_NM = " " });
                    this.combo_PRNT_GRP_NM.ItemsSource = _groupList;
                    //this.combo_PRNT_GRP_NM.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<GroupUserVo>>(await response.Content.ReadAsStringAsync()).Cast<GroupUserVo>().ToList();
                }
            }
        }

        //public static IList<CodeDao> GROUP_VO()
        //{
        //    IList<GroupUserVo> GroupVo = authClient.SelectGroupList(new GroupUserVo() { GRP_TP_CD = "G" , CHNL_CD = SystemProperties.USER_VO.CHNL_CD});
        //    IList<CodeDao> resultMap = new List<CodeDao>();
        //    int nCnt = GroupVo.Count;
        //    GroupUserVo tmpVo;
        //    resultMap.Clear();
        //    for (int x = 0; x < nCnt; x++)
        //    {
        //        tmpVo = GroupVo[x];
        //        resultMap.Add(new CodeDao() { CLSS_CD = tmpVo.GRP_ID, CLSS_DESC = tmpVo.GRP_NM });
        //    }
        //    //
        //    resultMap.Insert(0,new CodeDao() { CLSS_CD = "", CLSS_DESC = "" });
        //    return resultMap;
        //}
    }
}
