using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Auth;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using AquilaErpWpfApp3.Util;
using AquilaErpWpfApp3.View.S.Dialog;

namespace AquilaErpWpfApp3.View.S
{
    public partial class S136 : UserControl
    {
        private string title = "사용자 권한 관리";

        private S136GroupDialog groupDialog;
        private S136UserDialog userDialog;
        private S136MenuDialog menuDialog;

        public S136()
        {
            //DataContext = new S8813ViewModel();
            //
            InitializeComponent();

            try
            {
                this.ConfigViewPage1Edit_Master.MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(GridEdit_user_MouseDoubleClick);
                this.menuGridControl.MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(menuGridControl_MouseDoubleClick);
                this.ConfigViewPage1Edit_Master.SelectedItemChanged += ConfigViewPage1Edit_Master_SelectedItemChanged;
                //this.txt_Master_Search.KeyDown += new KeyEventHandler(txt_Master_Search_KeyDown);
                //this.btn_ConfigViewPage_Master_search.Click += new RoutedEventHandler(btn_Master_search_Click);
                //
                //this.ConfigViewPage1Edit_Master.ShowLoadingPanel = true;
                //this.ConfigViewPage1Edit_Master.BeginDataUpdate();

                //복수 사업장
                //IList<GroupUserVo> resultList = SystemProperties.AuthClient.SelectGroupUserTreeList(new GroupUserVo() { EMPE_PLC_NM = SystemProperties.USERVO.EMPE_PLC_NM });
                //IList<GroupUserVo> resultList = SystemProperties.AuthClient.SelectGroupUserTreeList(new GroupUserVo() { CHNL_CD =  SystemProperties.USER_VO.CHNL_CD });
                //GroupUserVo dao;
                //foreach (var item in resultList)
                //{
                //    dao = (GroupUserVo)item;
                //    if (string.IsNullOrEmpty(dao.USR_ID))
                //    {
                //        dao.IMAGE = Convert.FromBase64String(SystemProperties.IMG_GROUP_16);
                //    }
                //    else
                //    {
                //        dao.IMAGE = Convert.FromBase64String(SystemProperties.IMG_USER_16);
                //    }
                //}

                //this.ConfigViewPage1Edit_Master.ItemsSource = resultList;
                //if (resultList.Count > 0)
                //{
                //    this.GridEditView_menu.FocusedRowHandle = 0;
                //    this.GridEditView_menu.ExpandAllNodes();
                //}
                //this.ConfigViewPage1Edit_Master.EndDataUpdate();

                Master_Search((this.M_SEARCH_TEXT.EditValue == null ? "" : this.M_SEARCH_TEXT.EditValue.ToString()));
            }
            catch (Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            //
            //this.ConfigViewPage1Edit_Master.ShowLoadingPanel = false;
        }

        private void M_REFRESH_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            try
            {
                this.ConfigViewPage1Edit_Master.ShowLoadingPanel = true;
                Master_Search((this.M_SEARCH_TEXT.EditValue == null ? "" : this.M_SEARCH_TEXT.EditValue.ToString()));


                //this.GridEditView_menu.SearchString = (this.M_SEARCH_TEXT.EditValue == null ? "" : this.M_SEARCH_TEXT.EditValue.ToString());
                //this.txt_Search.SelectAll();
                this.M_SEARCH_TEXT.Focus();
                this.ConfigViewPage1Edit_Master.ShowLoadingPanel = false;

                //((S131ViewModel)this.DataContext).setTitle();
            }
            catch (Exception eLog)
            {
                this.ConfigViewPage1Edit_Master.ShowLoadingPanel = false;
                WinUIMessageBox.Show(eLog.Message, title, MessageBoxButton.OK, MessageBoxImage.Error);
                //this.M_SEARCH_TEXT.SelectAll();
                this.M_SEARCH_TEXT.Focus();
                return;
            }
        }

        private void M_SEARCH_TEXT_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                M_REFRESH_ItemClick(sender, null);
            }
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

        async void ConfigViewPage1Edit_Master_SelectedItemChanged(object sender, SelectedItemChangedEventArgs e)
        {
            //IList<ProgramVo> resultMenuList;
            GroupUserVo dao = (GroupUserVo)ConfigViewPage1Edit_Master.GetFocusedRow();
            if (dao == null)
            {
                return;
            }
            //그룹클릭시
            if (dao.IS_GROUP.Equals("G"))
            {
                DetailView_user.Visibility = Visibility.Hidden;
                //
                DetailView_group.Visibility = Visibility.Visible;
                //
                DetailView_user_ostr.Visibility = Visibility.Hidden;
                DetailView_group.Content = dao;
                //
                //resultMenuList = SystemProperties.AuthClient.SelectProgramGroupList(new ProgramVo() { GRP_ID = dao.GRP_ID, CHNL_CD = SystemProperties.USER_VO.CHNL_CD });

                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s136/g/menu", new StringContent(JsonConvert.SerializeObject(new ProgramVo() { GRP_ID = dao.GRP_ID, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.menuGridControl.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<ProgramVo>>(await response.Content.ReadAsStringAsync()).Cast<ProgramVo>().ToList();
                        this.menuGridEditView.ExpandAllNodes();
                    }
                }
            }

            //유저클릭시 + 일반유저
            if (!dao.IS_GROUP.Equals("G") && !dao.OSTR_FLG.Equals("Y"))
            {
                DetailView_group.Visibility = Visibility.Hidden;
                //
                DetailView_user.Visibility = Visibility.Visible;
                //
                DetailView_user_ostr.Visibility = Visibility.Hidden;
                //DetailView_user.Content = SystemProperties.AuthClient.SelectUserList(new GroupUserVo() { USR_ID = dao.USR_ID, DELT_FLG = "N", CHNL_CD = SystemProperties.USER_VO.CHNL_CD });
                //
                //resultMenuList = SystemProperties.AuthClient.SelectProgramUserList(new ProgramVo() { GRP_ID = dao.PRNT_GRP_ID, USR_ID = dao.USR_ID, CHNL_CD = SystemProperties.USER_VO.CHNL_CD });
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s136/usr", new StringContent(JsonConvert.SerializeObject(new GroupUserVo() { USR_ID = dao.USR_ID, DELT_FLG = "N", CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        //DetailView_user.Content = JsonConvert.DeserializeObject<GroupUserVo>(await response.Content.ReadAsStringAsync());
                        DetailView_user.Content = JsonConvert.DeserializeObject<IEnumerable<GroupUserVo>>(await response.Content.ReadAsStringAsync()).Cast<GroupUserVo>().ToList();
                    }
                }

                //
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s136/u/menu", new StringContent(JsonConvert.SerializeObject(new ProgramVo() { GRP_ID = dao.PRNT_GRP_ID, USR_ID = dao.USR_ID, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.menuGridControl.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<ProgramVo>>(await response.Content.ReadAsStringAsync()).Cast<ProgramVo>().ToList();
                        this.menuGridEditView.ExpandAllNodes();
                    }
                }
            }
            //유저클릭시 + 고객사
            if (!dao.IS_GROUP.Equals("G") && dao.OSTR_FLG.Equals("Y"))
            {
                DetailView_group.Visibility = Visibility.Hidden;
                //
                DetailView_user.Visibility = Visibility.Hidden;
                //
                DetailView_user_ostr.Visibility = Visibility.Visible;

                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s136/usr", new StringContent(JsonConvert.SerializeObject(new GroupUserVo() { USR_ID = dao.USR_ID, DELT_FLG = "N", CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        //DetailView_user.Content = JsonConvert.DeserializeObject<GroupUserVo>(await response.Content.ReadAsStringAsync());
                        DetailView_user_ostr.Content = JsonConvert.DeserializeObject<IEnumerable<GroupUserVo>>(await response.Content.ReadAsStringAsync()).Cast<GroupUserVo>().ToList();
                    }
                }

                //
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s136/u/menu", new StringContent(JsonConvert.SerializeObject(new ProgramVo() { GRP_ID = dao.PRNT_GRP_ID, USR_ID = dao.USR_ID, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.menuGridControl.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<ProgramVo>>(await response.Content.ReadAsStringAsync()).Cast<ProgramVo>().ToList();
                        this.menuGridEditView.ExpandAllNodes();
                    }
                }
            }
            selectItemIsMenu(dao);
            //
            //SystemProperties.MENU_IMG_SET(resultMenuList);
            //this.menuGridControl.ItemsSource = resultMenuList;
            //this.menuGridEditView.ExpandAllNodes();
        } 

        void selectItemIsMenu(GroupUserVo dao)
        {
            if(dao == null){
                return;
            }
            else if (dao.IS_GROUP.Equals("G"))
            {
                this.group_add.IsEnabled = true;
                this.group_edit.IsEnabled = true;
                this.group_del.IsEnabled = true;
                this.user_add.IsEnabled = true;
                this.user_edit.IsEnabled = false;
                this.user_del.IsEnabled = false;
                this.group_user_auth.IsEnabled = true;
            }
            else
            {
                this.group_add.IsEnabled = true;
                this.group_edit.IsEnabled = false;
                this.group_del.IsEnabled = false;
                this.user_add.IsEnabled = true;
                this.user_edit.IsEnabled = true;
                this.user_del.IsEnabled = true;
                this.group_user_auth.IsEnabled = true;
            }
        }
        #region Functon (Master Search)
        //void txt_Master_Search_KeyDown(Object sender, KeyEventArgs e)
        //{
        //    if (e.Key == Key.Enter)
        //    {
        //        Master_Search(this.txt_Master_Search.Text);
        //    }
        //}

        //void btn_Master_search_Click(object sender, RoutedEventArgs e)
        //{
        //    Master_Search(this.txt_Master_Search.Text);
        //}

        async void Master_Search(string search, int? _index = null)
        {
            try
            {
                //this.ConfigViewPage1Edit_Master.ShowLoadingPanel = true;
                //this.ConfigViewPage1Edit_Master.BeginDataUpdate();

                if (string.IsNullOrEmpty(search)) { search = null; }

                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s136", new StringContent(JsonConvert.SerializeObject(new GroupUserVo() { EMPE_PLC_NM = SystemProperties.USER_VO.EMPE_PLC_NM, USR_ID = search, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        //this.ConfigViewPage1Edit_Master.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<GroupUserVo>>(await response.Content.ReadAsStringAsync()).Cast<GroupUserVo>().ToList();
                        GroupUserVo dao;
                        IList<GroupUserVo> resultList = JsonConvert.DeserializeObject<IEnumerable<GroupUserVo>>(await response.Content.ReadAsStringAsync()).Cast<GroupUserVo>().ToList();
                        foreach (var item in resultList)
                        {
                            dao = (GroupUserVo)item;
                            if (string.IsNullOrEmpty(dao.USR_ID))
                            {
                                dao.IMAGE = Convert.FromBase64String(SystemProperties.IMG_GROUP_16);
                            }
                            else
                            {
                                dao.IMAGE = Convert.FromBase64String(SystemProperties.IMG_USER_16);
                            }
                        }
                        this.ConfigViewPage1Edit_Master.ItemsSource = resultList;
                        this.GridEditView_menu.AutoExpandAllNodes = true;


                        if (_index != null)
                        {
                            this.GridEditView_menu.FocusedRowHandle = Convert.ToInt32(_index);
                        }
                    }
                }

                    //IList<GroupUserVo> resultList = SystemProperties.AuthClient.SelectGroupUserTreeList(new GroupUserVo() { EMPE_PLC_NM = SystemProperties.USER_VO.EMPE_PLC_NM, USR_ID = search, CHNL_CD = SystemProperties.USER_VO.CHNL_CD });
                    //GroupUserVo dao;
                    //foreach (var item in resultList)
                    //{
                    //    dao = (GroupUserVo)item;
                    //    if (string.IsNullOrEmpty(dao.USR_ID))
                    //    {
                    //        dao.IMAGE = Convert.FromBase64String(SystemProperties.IMG_GROUP_16);
                    //    }
                    //    else
                    //    {
                    //        dao.IMAGE = Convert.FromBase64String(SystemProperties.IMG_USER_16);
                    //    }
                    //}
                    //this.ConfigViewPage1Edit_Master.ItemsSource = resultList;
                    //this.ConfigViewPage1Edit_Master.EndDataUpdate();

                //if (string.IsNullOrEmpty(search)) 
                //{
                //    this.GridEditView_menu.CollapseAllNodes();
                //}
                //else
                //{
                //    this.GridEditView_menu.ExpandAllNodes();
                //}
                //
                //this.GridEditView_menu.SearchString = scarch;
                //this.txt_Master_Search.SelectAll();
                //this.txt_Master_Search.Focus();
                //this.ConfigViewPage1Edit_Master.ShowLoadingPanel = false;

                selectItemIsMenu((GroupUserVo)ConfigViewPage1Edit_Master.GetFocusedRow());
            }
            catch (Exception eLog)
            {
                //this.ConfigViewPage1Edit_Master.ShowLoadingPanel = false;
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
                //this.txt_Master_Search.SelectAll();
                //this.txt_Master_Search.Focus();
                return;
            }
        }
        #endregion
        void GridEdit_user_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            TreeListNode node = GridEditView_menu.FocusedNode;
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
        private void group_add_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            GroupUserVo dao = (GroupUserVo)ConfigViewPage1Edit_Master.GetFocusedRow();
            if (dao == null)
            {
                dao = new GroupUserVo(){PRNT_GRP_NM = ""};
                //    WinUIMessageBox.Show("그룹 만 추가/수정/삭제가 가능 합니다.", "[자재 관리 시스템]사용자 권한 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
                //    return;
            }
            int selectedRowhandle = this.GridEditView_menu.FocusedRowHandle;

            groupDialog = new S136GroupDialog(new GroupUserVo() { PRNT_GRP_NM = dao.PRNT_GRP_NM, CHNL_CD = SystemProperties.USER_VO.CHNL_CD, OSTR_FLG = dao.OSTR_FLG, GRP_ID = dao.GRP_ID });
            groupDialog.Title = "그룹 추가";
            groupDialog.Owner = Application.Current.MainWindow;
            groupDialog.BorderEffect = BorderEffect.Default;
            groupDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            groupDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            bool isDialog = (bool)groupDialog.ShowDialog();
            if (isDialog)
            {
                //if (groupDialog.IsEdit == false)
                //{
                Master_Search("", selectedRowhandle);
                //this.GridEditView_menu.FocusedRowHandle = selectedRowhandle;
                //}
            }

        }
        private void group_edit_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            GroupUserVo dao = (GroupUserVo)ConfigViewPage1Edit_Master.GetFocusedRow();
            if (dao == null)
            {
                WinUIMessageBox.Show("그룹 만 추가/수정/삭제가 가능 합니다.", "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            int selectedRowhandle = this.GridEditView_menu.FocusedRowHandle;

            groupDialog = new S136GroupDialog(dao);
            groupDialog.Title = "그룹 수정";
            groupDialog.Owner = Application.Current.MainWindow;
            groupDialog.BorderEffect = BorderEffect.Default;
            groupDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            groupDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            bool isDialog = (bool)groupDialog.ShowDialog();
            if (isDialog)
            {
                //if (groupDialog.IsEdit == true)
                //{
                    Master_Search("", selectedRowhandle);
                    //this.GridEditView_menu.FocusedRowHandle = selectedRowhandle;
                    
                //}
            }
        }
        private async void group_del_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            GroupUserVo dao = (GroupUserVo)ConfigViewPage1Edit_Master.GetFocusedRow();
            if (dao != null)
            {
                if (dao.IS_GROUP.Equals("G"))
                {
                    TreeListNode node = GridEditView_menu.FocusedNode;
                    if (node.HasChildren)
                    {
                        WinUIMessageBox.Show("그룹에 포함된 그룹 및 유저가 존재 합니다. ", "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    int selectedRowhandle = this.GridEditView_menu.FocusedRowHandle;
                    MessageBoxResult result = WinUIMessageBox.Show(dao.GRP_NM + " 정말로 삭제 하시겠습니까?", title, MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        int _Num = 0;
                        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s136/g/menu/d", new StringContent(JsonConvert.SerializeObject(new ProgramVo() { GRP_ID = dao.GRP_ID, CHNL_CD = SystemProperties.USER_VO.CHNL_CD, CRE_USR_ID = SystemProperties.USER }), System.Text.Encoding.UTF8, "application/json")))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                string resultMsg = await response.Content.ReadAsStringAsync();
                                if (int.TryParse(resultMsg, out _Num) == false)
                                {
                                    //실패
                                    WinUIMessageBox.Show(resultMsg, title, MessageBoxButton.OK, MessageBoxImage.Error);
                                    return;
                                }

                                //성공
                               // WinUIMessageBox.Show("완료 되었습니다", title, MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                        }

                        dao.CRE_USR_ID = SystemProperties.USER;
                        dao.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
                        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s136/g/d", new StringContent(JsonConvert.SerializeObject(dao), System.Text.Encoding.UTF8, "application/json")))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                string resultMsg = await response.Content.ReadAsStringAsync();
                                if (int.TryParse(resultMsg, out _Num) == false)
                                {
                                    //실패
                                    WinUIMessageBox.Show(resultMsg, title, MessageBoxButton.OK, MessageBoxImage.Error);
                                    return;
                                }
                                //성공
                                //WinUIMessageBox.Show("완료 되었습니다", title, MessageBoxButton.OK, MessageBoxImage.Information);
                                WinUIMessageBox.Show("삭제가 완료되었습니다.", title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                            }
                        }

                        //SystemProperties.AuthClient.DeleteGroupProgram(new ProgramVo() { GRP_ID = dao.GRP_ID, CHNL_CD = SystemProperties.USER_VO.CHNL_CD });
                        //SystemProperties.AuthClient.DeleteGroupCode(dao);
                        //WinUIMessageBox.Show("삭제가 완료되었습니다.", title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                        
                        Master_Search("", selectedRowhandle-1);
                        //this.GridEditView_menu.FocusedRowHandle = selectedRowhandle;
                        //node = GridEditView_menu.FocusedNode;
                        //if (node != null)
                        //{
                        //    node.ExpandAll();
                        //}
                    }
                }
                else
                {
                    WinUIMessageBox.Show("그룹 만 추가/수정/삭제가 가능 합니다.", "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }
        }
        private void user_add_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            GroupUserVo dao = (GroupUserVo)ConfigViewPage1Edit_Master.GetFocusedRow();
            if (dao == null)
            {
                dao = new GroupUserVo() { PRNT_GRP_NM = "" };
            }
            else if(dao.IS_GROUP.Equals("G"))
            {
                dao = new GroupUserVo() { PRNT_GRP_NM = dao.GRP_NM };
            }

            int selectedRowhandle = this.GridEditView_menu.FocusedRowHandle;

            userDialog = new S136UserDialog(new GroupUserVo() { GRP_NM = dao.PRNT_GRP_NM, CHNL_CD = SystemProperties.USER_VO.CHNL_CD, OSTR_FLG = dao.OSTR_FLG });
            userDialog.Title = "유저 추가";
            userDialog.Owner = Application.Current.MainWindow;
            userDialog.BorderEffect = BorderEffect.Default;
            userDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            userDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            bool isDialog = (bool)userDialog.ShowDialog();
            if (isDialog)
            {
                //if (userDialog.IsEdit == false)
                //{
                    Master_Search("", selectedRowhandle);
                    //this.GridEditView_menu.FocusedRowHandle = selectedRowhandle;
                //}
            }
        }
        private async void user_edit_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            GroupUserVo dao = (GroupUserVo)ConfigViewPage1Edit_Master.GetFocusedRow();
            if (dao == null)
            {
                WinUIMessageBox.Show("유저 만 추가/수정/삭제가 가능 합니다.", "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else if (dao.IS_GROUP.Equals("G"))
            {
                WinUIMessageBox.Show("유저 만 추가/수정/삭제가 가능 합니다.", "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s136/usr", new StringContent(JsonConvert.SerializeObject(new GroupUserVo() { USR_ID = dao.USR_ID, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    GroupUserVo daoEdit  = JsonConvert.DeserializeObject<IEnumerable<GroupUserVo>>(await response.Content.ReadAsStringAsync()).Cast<GroupUserVo>().ToList()[0];
                    //GroupUserVo daoEdit = JsonConvert.DeserializeObject<GroupUserVo>(await response.Content.ReadAsStringAsync());

                    if (daoEdit == null)
                    {
                        return;
                    }

                    int selectedRowhandle = this.GridEditView_menu.FocusedRowHandle;

                    userDialog = new S136UserDialog(daoEdit);
                    userDialog.Title = "유저 수정";
                    userDialog.Owner = Application.Current.MainWindow;
                    userDialog.BorderEffect = BorderEffect.Default;
                    userDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
                    userDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
                    bool isDialog = (bool)userDialog.ShowDialog();
                    if (isDialog)
                    {
                        //if (userDialog.IsEdit == true)
                        //{
                            Master_Search("", selectedRowhandle);
                            //this.GridEditView_menu.FocusedRowHandle = selectedRowhandle;
                        //}
                    }

                }
            }

            //GroupUserVo daoEdit = SystemProperties.AuthClient.SelectUserList(new GroupUserVo() { USR_ID = dao.USR_ID, DELT_FLG = "N", CHNL_CD = SystemProperties.USER_VO.CHNL_CD })[0];

            //int selectedRowhandle = this.GridEditView_menu.FocusedRowHandle;

            //userDialog = new S136UserDialog(daoEdit);
            //userDialog.Title = "유저 수정";
            //userDialog.Owner = Application.Current.MainWindow;
            //userDialog.BorderEffect = BorderEffect.Default;
            ////userDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            ////userDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            //bool isDialog = (bool)userDialog.ShowDialog();
            //if (isDialog)
            //{
            //    if (userDialog.IsEdit == true)
            //    {
            //        Master_Search("");
            //        this.GridEditView_menu.FocusedRowHandle = selectedRowhandle;
            //    }
            //}
        }
        private async void user_del_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            GroupUserVo dao = (GroupUserVo)ConfigViewPage1Edit_Master.GetFocusedRow();
            if (dao != null)
            {
                if (!dao.IS_GROUP.Equals("G"))
                {
                    TreeListNode node = GridEditView_menu.FocusedNode;
                    if (node.HasChildren)
                    {
                        WinUIMessageBox.Show("그룹에 포함된 그룹 및 유저가 존재 합니다. ", "[" + SystemProperties.PROGRAM_TITLE + "]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    int selectedRowhandle = node.ParentNode.RowHandle;
                    MessageBoxResult result = WinUIMessageBox.Show("[" + dao.GRP_NM + "]" + dao.USR_ID + " 정말로 삭제 하시겠습니까?", title, MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        int _Num = 0;
                        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s136/u/menu/d", new StringContent(JsonConvert.SerializeObject(new ProgramVo() { GRP_ID = dao.GRP_ID, CHNL_CD = SystemProperties.USER_VO.CHNL_CD, CRE_USR_ID = SystemProperties.USER }), System.Text.Encoding.UTF8, "application/json")))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                string resultMsg = await response.Content.ReadAsStringAsync();
                                if (int.TryParse(resultMsg, out _Num) == false)
                                {
                                    //실패
                                    WinUIMessageBox.Show(resultMsg, title, MessageBoxButton.OK, MessageBoxImage.Error);
                                    return;
                                }

                                //성공
                                // WinUIMessageBox.Show("완료 되었습니다", title, MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                        }

                        dao.CRE_USR_ID = SystemProperties.USER;
                        dao.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
                        using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s136/u/d", new StringContent(JsonConvert.SerializeObject(dao), System.Text.Encoding.UTF8, "application/json")))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                string resultMsg = await response.Content.ReadAsStringAsync();
                                if (int.TryParse(resultMsg, out _Num) == false)
                                {
                                    //실패
                                    WinUIMessageBox.Show(resultMsg, title, MessageBoxButton.OK, MessageBoxImage.Error);
                                    return;
                                }
                                //성공
                                //WinUIMessageBox.Show("완료 되었습니다", title, MessageBoxButton.OK, MessageBoxImage.Information);
                                WinUIMessageBox.Show("삭제가 완료되었습니다.", title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                            }
                        }
                        //SystemProperties.AuthClient.DeleteUserProgram(new ProgramVo() { USR_ID = dao.USR_ID, CHNL_CD = SystemProperties.USER_VO.CHNL_CD });
                        //SystemProperties.AuthClient.DeleteUserInfo(dao);
                        //WinUIMessageBox.Show("삭제가 완료되었습니다.", "[삭제 - 유저]" + title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
                        Master_Search("", selectedRowhandle);
                        //this.GridEditView_menu.FocusedRowHandle = selectedRowhandle;
                        //node = GridEditView_menu.FocusedNode;
                        //node.ExpandAll();
                    }
                }
                else
                {
                    WinUIMessageBox.Show("유저 만 추가/수정/삭제가 가능 합니다.", title, MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }
        }
        private async void menu_edit_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        {
            GroupUserVo dao = (GroupUserVo)ConfigViewPage1Edit_Master.GetFocusedRow();
            //
            if (dao == null)
            {
                return;
            }

            menuDialog = new S136MenuDialog(dao);
            menuDialog.Title = "메뉴 권한 등록";
            menuDialog.Owner = Application.Current.MainWindow;
            menuDialog.BorderEffect = BorderEffect.Default;
            menuDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
            menuDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
            bool isDialog = (bool)menuDialog.ShowDialog();
            if (isDialog)
            {
                //IList<ProgramVo> resultMenuList;
                if (dao == null)
                {
                    return;
                }
                //
                if (dao.IS_GROUP.Equals("G"))
                {
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s136/g/menu", new StringContent(JsonConvert.SerializeObject(new ProgramVo() { GRP_ID = dao.GRP_ID, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
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
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s136/u/menu", new StringContent(JsonConvert.SerializeObject(new ProgramVo() { GRP_ID = dao.PRNT_GRP_ID, USR_ID = dao.USR_ID, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            this.menuGridControl.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<ProgramVo>>(await response.Content.ReadAsStringAsync()).Cast<ProgramVo>().ToList();
                            this.menuGridEditView.ExpandAllNodes();
                        }
                    }
                    //    resultMenuList = SystemProperties.AuthClient.SelectProgramUserList(new ProgramVo() { GRP_ID = dao.PRNT_GRP_ID, USR_ID = dao.USR_ID, CHNL_CD = SystemProperties.USER_VO.CHNL_CD });
                }
                //SystemProperties.MENU_IMG_SET(resultMenuList);
                //this.menuGridControl.ItemsSource = resultMenuList;
                //this.menuGridEditView.ExpandAllNodes();
            }
        }
    }
}