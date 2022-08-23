using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Auth;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Windows;
using System.Linq;
using System.Windows.Input;
using AquilaErpWpfApp3.Util;

namespace AquilaErpWpfApp3.View.S.Dialog
{
    /// <summary>
    /// Interaction logic for ConfigView1Dialog.xaml
    /// </summary>
    public partial class S136MenuDialog : DXWindow
    {

        private string title = "사용자 권한 관리";

        //private static AuthorServiceClient authClient = SystemProperties.AuthClient;
        private GroupUserVo searchDao;
        private Dictionary<string, ProgramVo> _saveMap = new Dictionary<string, ProgramVo>();

        //Add
        public S136MenuDialog(GroupUserVo dao)
        {
            InitializeComponent();

            this.searchDao = dao;
            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
            this.btn_ViewPage_search.Click += new RoutedEventHandler(btn_search_Click);
            this.menuGridControl.MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(menuGridControl_MouseDoubleClick);

            btn_search_Click(null, null);
        }

        void menuGridControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TreeListNode node = menuGridEditView.FocusedNode;
            //GroupUserVo dao = (GroupUserVo)ConfigViewPage1Edit_Master.GetFocusedRow();
            try
            {
                if (node.IsExpanded)
                {
                    node.CollapseAll();
                }
                else
                {
                    node.ExpandAll();
                }
            }
            catch (Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
        async void btn_search_Click(object sender, RoutedEventArgs e)
        {
            //IList<ProgramVo> resultMenuList;
            if (this.searchDao == null)
            {
                return;
            }
            //
            if ((this.searchDao.IS_GROUP.Equals("G")))
            {
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s136/g/menu", new StringContent(JsonConvert.SerializeObject(new ProgramVo() { GRP_ID = searchDao.GRP_ID, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.menuGridControl.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<ProgramVo>>(await response.Content.ReadAsStringAsync()).Cast<ProgramVo>().ToList();
                        this.menuGridEditView.ExpandAllNodes();
                    }
                }
                //    resultMenuList = SystemProperties.AuthClient.SelectProgramGroupList(new ProgramVo() { GRP_ID = dao.GRP_ID, CHNL_CD = SystemProperties.USER_VO.CHNL_CD });
            }
            else
            {
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s136/u/menu", new StringContent(JsonConvert.SerializeObject(new ProgramVo() { GRP_ID = searchDao.PRNT_GRP_ID, USR_ID = searchDao.USR_ID, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.menuGridControl.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<ProgramVo>>(await response.Content.ReadAsStringAsync()).Cast<ProgramVo>().ToList();
                        this.menuGridEditView.ExpandAllNodes();
                    }
                }
                //    resultMenuList = SystemProperties.AuthClient.SelectProgramUserList(new ProgramVo() { GRP_ID = dao.PRNT_GRP_ID, USR_ID = dao.USR_ID, CHNL_CD = SystemProperties.USER_VO.CHNL_CD });
            }

            //
            //if (this.searchDao.IS_GROUP.Equals("G"))
            //{
            //    resultMenuList = authClient.SelectProgramGroupList(new ProgramVo() { GRP_ID = this.searchDao.GRP_ID ,CHNL_CD = SystemProperties.USER_VO.CHNL_CD});
            //}
            //else
            //{
            //    resultMenuList = authClient.SelectProgramUserList(new ProgramVo() { GRP_ID = this.searchDao.PRNT_GRP_ID, USR_ID = this.searchDao.USR_ID, CHNL_CD = SystemProperties.USER_VO.CHNL_CD });
            //}
            //
            //SystemProperties.MENU_IMG_SET(resultMenuList);
            //this.menuGridControl.ItemsSource = resultMenuList;
            //this.menuGridEditView.ExpandAllNodes();
        }

        #region Functon (OKButton_Click, CancelButton_Click)
        private async void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int _Num = 0;
                //ProgramVo resultVo;
                ProgramVo tmpVo;
                List<string> list = new List<string>(this._saveMap.Keys);

                if (this.searchDao == null)
                {
                    return;
                }

                int selectedRowhandle = this.menuGridEditView.FocusedRowHandle;

                for (int x = 0; x < list.Count; x++)
                {
                    tmpVo = this._saveMap[list[x]];
                    if (tmpVo.VIS_FLG.Equals(true) || tmpVo.VIS_FLG.Equals(1))
                    {
                        tmpVo.VIS_FLG = "Y";
                        tmpVo.UPD_FLG = "Y";
                    }
                    else
                    {
                        tmpVo.VIS_FLG = "N";
                        tmpVo.UPD_FLG = "N";
                    }
                    ////
                    //if (tmpVo.UPD_FLG.Equals(true) || tmpVo.UPD_FLG.Equals(1))
                    //{
                    //    tmpVo.UPD_FLG = "Y";
                    //}
                    //else
                    //{
                    //    tmpVo.UPD_FLG = "N";
                    //}
                    tmpVo.CRE_USR_ID = SystemProperties.USER;
                    tmpVo.UPD_USR_ID = SystemProperties.USER;
                    tmpVo.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;


                    if (this.searchDao.IS_GROUP.Equals("G"))
                    {
                        //resultVo = authClient.DeleteGroupProgram(new ProgramVo() { GRP_ID = searchDao.GRP_ID, MDL_ID = tmpVo.MDL_ID, CHNL_CD = SystemProperties.USER_VO.CHNL_CD });
                        //if (!resultVo.isSuccess)
                        //{
                        //    //실패
                        //    WinUIMessageBox.Show(resultVo.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
                        //    return;
                        //}
                        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s136/g/menu/d", new StringContent(JsonConvert.SerializeObject(new ProgramVo() { GRP_ID = searchDao.GRP_ID, MDL_ID = tmpVo.MDL_ID, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
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
                                //WinUIMessageBox.Show("완료 되었습니다", title, MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                        }

                        tmpVo.GRP_ID = searchDao.GRP_ID;
                        //resultVo = authClient.InsertGroupProgram(tmpVo);
                        //if (!resultVo.isSuccess)
                        //{
                        //    //실패
                        //    WinUIMessageBox.Show(resultVo.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
                        //    return;
                        //}

                        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s136/g/menu/i", new StringContent(JsonConvert.SerializeObject(tmpVo), System.Text.Encoding.UTF8, "application/json")))
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
                                //WinUIMessageBox.Show("완료 되었습니다", title, MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                        }
                    }
                    else
                    {
                        //resultVo = authClient.DeleteUserProgram(new ProgramVo() { USR_ID = searchDao.USR_ID, MDL_ID = tmpVo.MDL_ID, CHNL_CD = SystemProperties.USER_VO.CHNL_CD });
                        //if (!resultVo.isSuccess)
                        //{
                        //    //실패
                        //    WinUIMessageBox.Show(resultVo.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
                        //    return;
                        //}

                        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s136/u/menu/d", new StringContent(JsonConvert.SerializeObject(new ProgramVo() { USR_ID = searchDao.USR_ID, MDL_ID = tmpVo.MDL_ID, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
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
                                //WinUIMessageBox.Show("완료 되었습니다", title, MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                        }

                        tmpVo.USR_ID = searchDao.USR_ID;
                        //resultVo = authClient.InsertUserProgram(tmpVo);
                        //if (!resultVo.isSuccess)
                        //{
                        //    //실패
                        //    WinUIMessageBox.Show(resultVo.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
                        //    return;
                        //}

                        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s136/u/menu/i", new StringContent(JsonConvert.SerializeObject(tmpVo), System.Text.Encoding.UTF8, "application/json")))
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
                                //WinUIMessageBox.Show("완료 되었습니다", title, MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                        }

                    }
                }

                this._saveMap.Clear();

                btn_search_Click(null, null);
                this.menuGridEditView.FocusedRowHandle = selectedRowhandle;

                WinUIMessageBox.Show("완료되었습니다.", title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
            }
            catch (Exception eLog)
            {
                //실패
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void HandleEsc(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.DialogResult = true;
                Close();
            }
        }
        #endregion

        //private void PART_Editor_Checked(object sender, RoutedEventArgs e)
        //{
        //    ProgramVo dao = (ProgramVo)menuGridControl.GetFocusedRow();
        //    if (dao == null)
        //    {
        //        return;
        //    }
        //    else if (dao.PGM_CD.Equals("G") || dao.PGM_CD.Equals("A") || dao.PGM_CD.Equals("P"))
        //    {
        //        this._saveMap.Add( dao.MDL_ID , dao);
        //    }
        //}

        private void PART_Editor_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            ProgramVo dao = (ProgramVo)menuGridControl.GetFocusedRow();
            if (dao == null)
            {
                return;
            }
            else if (dao.PGM_CD.Equals("G") || dao.PGM_CD.Equals("A") || dao.PGM_CD.Equals("P") || dao.PGM_CD.Equals("M"))
            {
                if (this._saveMap.ContainsKey(dao.MDL_ID))
                {
                    this._saveMap.Remove(dao.MDL_ID);
                }

                this._saveMap.Add(dao.MDL_ID, dao);
            }
        }
    }
}
