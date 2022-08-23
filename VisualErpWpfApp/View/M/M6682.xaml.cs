using System.Windows.Controls;
using AquilaErpWpfApp3.ViewModel;

namespace AquilaErpWpfApp3.View.M
{
    public partial class M6682 : UserControl
    {
        //private S136GroupDialog groupDialog;
        //private S136UserDialog userDialog;
        //private S136MenuDialog menuDialog;

        public M6682()
        {
            DataContext = new M6682ViewModel();
            //
            InitializeComponent();

            //try
            //{
            //    //this.ConfigViewPage1Edit_Master.MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(GridEdit_user_MouseDoubleClick);
            //    //this.menuGridControl.MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(menuGridControl_MouseDoubleClick);
            //    //this.ConfigViewPage1Edit_Master.SelectedItemChanged += ConfigViewPage1Edit_Master_SelectedItemChanged;
            //    //this.txt_Master_Search.KeyDown += new KeyEventHandler(txt_Master_Search_KeyDown);
            //    //this.btn_ConfigViewPage_Master_search.Click += new RoutedEventHandler(btn_Master_search_Click);
            //    ////
            //    //this.ConfigViewPage1Edit_Master.ShowLoadingPanel = true;
            //    //this.ConfigViewPage1Edit_Master.BeginDataUpdate();

            //    //IList<GroupUserVo> resultList = SystemProperties.AuthClient.SelectGroupUserTreeList(new GroupUserVo() { EMPE_PLC_NM = SystemProperties.USERVO.EMPE_PLC_NM });
            //    //GroupUserVo dao;
            //    //foreach (var item in resultList)
            //    //{
            //    //    dao = (GroupUserVo)item;
            //    //    if (string.IsNullOrEmpty(dao.USR_ID))
            //    //    {
            //    //        dao.IMAGE = Convert.FromBase64String(SystemProperties.IMG_GROUP_16);
            //    //    }
            //    //    else
            //    //    {
            //    //        dao.IMAGE = Convert.FromBase64String(SystemProperties.IMG_USER_16);
            //    //    }
            //    //}
            //    //this.ConfigViewPage1Edit_Master.ItemsSource = resultList;
            //    //this.ConfigViewPage1Edit_Master.EndDataUpdate();
            //}
            //catch (Exception eLog)
            //{
            //    WinUIMessageBox.Show(eLog.Message, "[에러]설비도면이력관리", MessageBoxButton.OK, MessageBoxImage.Error);
            //}
            ////
            //this.ConfigViewPage1Edit_Master.ShowLoadingPanel = false;
        }

        //void menuGridControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        //{
        //    TreeListNode node = menuGridEditView.FocusedNode;
        //    //GroupUserVo dao = (GroupUserVo)ConfigViewPage1Edit_Master.GetFocusedRow();
        //    try
        //    {
        //        if (node.IsExpanded)
        //        {
        //            node.CollapseAll();
        //        }
        //        else
        //        {
        //            node.ExpandAll();
        //        }
        //    }
        //    catch (Exception eLog)
        //    {
        //        WinUIMessageBox.Show(eLog.Message, "[자재 관리 시스템]사용자 권한 관리", MessageBoxButton.OK, MessageBoxImage.Error);
        //        return;
        //    }

        //}
        //void ConfigViewPage1Edit_Master_SelectedItemChanged(object sender, SelectedItemChangedEventArgs e)
        //{

        //    IList<ProgramVo> resultMenuList;
        //    GroupUserVo dao = (GroupUserVo)ConfigViewPage1Edit_Master.GetFocusedRow();

        //    if(dao == null){
        //        return;
        //    }
        //    //
        //    if (dao.IS_GROUP.Equals("G"))
        //    {
        //        DetailView_user.Visibility = Visibility.Hidden;
        //        //
        //        DetailView_group.Visibility = Visibility.Visible;
        //        DetailView_group.Content = dao;
        //        //
        //        resultMenuList = SystemProperties.AuthClient.SelectProgramGroupList(new ProgramVo() { GRP_ID= dao.GRP_ID });
        //    }
        //    else
        //    {
        //        DetailView_group.Visibility = Visibility.Hidden;
        //        //
        //        DetailView_user.Visibility = Visibility.Visible;
        //        DetailView_user.Content = SystemProperties.AuthClient.SelectUserList(dao);
        //        //
        //        resultMenuList = SystemProperties.AuthClient.SelectProgramUserList(new ProgramVo() { GRP_ID = dao.PRNT_GRP_ID, USR_ID = dao.USR_ID });
        //    }
        //    selectItemIsMenu(dao);
        //    //
        //    SystemProperties.MENU_IMG_SET(resultMenuList);
        //    this.menuGridControl.ItemsSource = resultMenuList;
        //}
        //void selectItemIsMenu(GroupUserVo dao)
        //{
        //    if(dao == null){
        //        return;
        //    }
        //    else if (dao.IS_GROUP.Equals("G"))
        //    {
        //        this.group_add.IsEnabled = true;
        //        this.group_edit.IsEnabled = true;
        //        this.group_del.IsEnabled = true;
        //        this.user_add.IsEnabled = true;
        //        this.user_edit.IsEnabled = false;
        //        this.user_del.IsEnabled = false;
        //        this.group_user_auth.IsEnabled = true;
        //    }
        //    else
        //    {
        //        this.group_add.IsEnabled = true;
        //        this.group_edit.IsEnabled = true;
        //        this.group_del.IsEnabled = false;
        //        this.user_add.IsEnabled = true;
        //        this.user_edit.IsEnabled = true;
        //        this.user_del.IsEnabled = true;
        //        this.group_user_auth.IsEnabled = true;
        //    }
        //}
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

        //void Master_Search(string search)
        //{
        //    try
        //    {
        //        this.ConfigViewPage1Edit_Master.ShowLoadingPanel = true;

        //        this.configViewPage1EditView_Master.SearchString = search;
        //        this.txt_Master_Search.SelectAll();
        //        this.txt_Master_Search.Focus();

        //        this.ConfigViewPage1Edit_Master.ShowLoadingPanel = false;
        //    }
        //    catch (Exception eLog)
        //    {
        //        this.ConfigViewPage1Edit_Master.ShowLoadingPanel = false;
        //        WinUIMessageBox.Show(eLog.Message, "[에러]품목 마스터 등록", MessageBoxButton.OK, MessageBoxImage.Error);
        //        this.txt_Master_Search.SelectAll();
        //        this.txt_Master_Search.Focus();
        //        return;
        //    }
        //}
        #endregion


        //private void Hyperlink_Click(object sender, RoutedEventArgs e)
        //{
        //    SystemCodeVo dao = (SystemCodeVo)ConfigViewPage1Edit_Master.GetFocusedRow();
        //    if (dao != null)
        //    {
        //        System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
        //        dialog.ShowNewFolderButton = true;
        //        dialog.Description = "[도면 파일 명] 저장 폴더를 선택 해 주세요.";
        //        dialog.RootFolder = Environment.SpecialFolder.Desktop;
        //        dialog.ShowDialog();
        //        if (!string.IsNullOrEmpty(dialog.SelectedPath))
        //        {
        //            if (!string.IsNullOrEmpty(dao.ITM_PLSTE_FILE_NM))
        //            {
        //                int ArraySize = dao.ITM_PLSTE_FILE.Length;
        //                FileStream fs = new FileStream(dialog.SelectedPath + "/" + dao.ITM_PLSTE_FILE_NM, FileMode.OpenOrCreate, FileAccess.Write);
        //                fs.Write(dao.ITM_PLSTE_FILE, 0, ArraySize);
        //                fs.Close();
        //            }
        //            System.Diagnostics.Process.Start(dialog.SelectedPath);
        //        }
        //    }
        //    return;
        //}

        //void GridEdit_user_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        //{
        //    TreeListNode node = GridEditView_menu.FocusedNode;
        //    //GroupUserVo dao = (GroupUserVo)ConfigViewPage1Edit_Master.GetFocusedRow();
        //    try
        //    {
        //        if (node.IsExpanded)
        //        {
        //            node.CollapseAll();
        //        }
        //        else
        //        {
        //            node.ExpandAll();
        //        }
        //    }
        //    catch (Exception eLog)
        //    {
        //        WinUIMessageBox.Show(eLog.Message, "[자재 관리 시스템]사용자 권한 관리", MessageBoxButton.OK, MessageBoxImage.Error);
        //        return;
        //    }
        //}
        //private void group_add_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        //{
        //    GroupUserVo dao = (GroupUserVo)ConfigViewPage1Edit_Master.GetFocusedRow();
        //    if (dao == null)
        //    {
        //        dao = new GroupUserVo(){PRNT_GRP_NM = ""};
        //        //    WinUIMessageBox.Show("그룹 만 추가/수정/삭제가 가능 합니다.", "[자재 관리 시스템]사용자 권한 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
        //        //    return;
        //    }
        //    int selectedRowhandle = this.GridEditView_menu.FocusedRowHandle;

        //    groupDialog = new S136GroupDialog(new GroupUserVo() { PRNT_GRP_NM = dao.PRNT_GRP_NM });
        //    groupDialog.Title = "그룹 추가";
        //    groupDialog.Owner = Application.Current.MainWindow;
        //    groupDialog.BorderEffect = BorderEffect.Default;
        //    //groupDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
        //    //groupDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
        //    bool isDialog = (bool)groupDialog.ShowDialog();
        //    if (isDialog)
        //    {
        //        if (groupDialog.IsEdit == false)
        //        {
        //            Master_Search("");
        //            this.GridEditView_menu.FocusedRowHandle = selectedRowhandle;
        //        }
        //    }

        //}
        //private void group_edit_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        //{
        //    GroupUserVo dao = (GroupUserVo)ConfigViewPage1Edit_Master.GetFocusedRow();
        //    if (dao == null)
        //    {
        //        WinUIMessageBox.Show("그룹 만 추가/수정/삭제가 가능 합니다.", "[자재 관리 시스템]사용자 권한 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
        //        return;
        //    }
        //    int selectedRowhandle = this.GridEditView_menu.FocusedRowHandle;

        //    groupDialog = new S136GroupDialog(dao);
        //    groupDialog.Title = "그룹 수정";
        //    groupDialog.Owner = Application.Current.MainWindow;
        //    groupDialog.BorderEffect = BorderEffect.Default;
        //    //groupDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
        //    //groupDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
        //    bool isDialog = (bool)groupDialog.ShowDialog();
        //    if (isDialog)
        //    {
        //        if (groupDialog.IsEdit == true)
        //        {
        //            Master_Search("");
        //            this.GridEditView_menu.FocusedRowHandle = selectedRowhandle;
                    
        //        }
        //    }
        //}
        //private void group_del_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        //{
        //    GroupUserVo dao = (GroupUserVo)ConfigViewPage1Edit_Master.GetFocusedRow();
        //    if (dao != null)
        //    {
        //        if(dao.IS_GROUP.Equals("G")){
        //            TreeListNode node = GridEditView_menu.FocusedNode;
        //            if (node.HasChildren)
        //            {
        //                WinUIMessageBox.Show("그룹에 포함된 그룹 및 유저가 존재 합니다. ", "[자재 관리 시스템]사용자 권한 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
        //                return;
        //            }
        //            int selectedRowhandle = this.GridEditView_menu.FocusedRowHandle;
        //            MessageBoxResult result = WinUIMessageBox.Show(dao.GRP_NM + " 정말로 삭제 하시겠습니까?", "[삭제 - 그룹]사용자 권한 관리", MessageBoxButton.YesNo, MessageBoxImage.Question);
        //            if (result == MessageBoxResult.Yes)
        //            {
        //                SystemProperties.AuthClient.DeleteGroupProgram(new ProgramVo() { GRP_ID = dao.GRP_ID });
        //                SystemProperties.AuthClient.DeleteGroupCodeAsync(dao);
        //                WinUIMessageBox.Show("삭제가 완료되었습니다.", "[삭제 - 그룹]사용자 권한 관리", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
        //                Master_Search("");
        //                this.GridEditView_menu.FocusedRowHandle = selectedRowhandle;
        //                node = GridEditView_menu.FocusedNode;
        //                if(node != null){
        //                    node.ExpandAll();
        //                }
        //            }
        //        }
        //        else
        //        {
        //            WinUIMessageBox.Show("그룹 만 추가/수정/삭제가 가능 합니다.", "[자재 관리 시스템]사용자 권한 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
        //            return;
        //        }
        //    }
        //}
        //private void user_add_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
        //{
        //    GroupUserVo dao = (GroupUserVo)ConfigViewPage1Edit_Master.GetFocusedRow();
        //    if (dao == null)
        //    {
        //        dao = new GroupUserVo() { PRNT_GRP_NM = "" };
        //    }
        //    else if(dao.IS_GROUP.Equals("G"))
        //    {
        //        dao = new GroupUserVo() { PRNT_GRP_NM = dao.GRP_NM };
        //    }

        //    int selectedRowhandle = this.GridEditView_menu.FocusedRowHandle;

        //    userDialog = new S136UserDialog(new GroupUserVo() { GRP_NM = dao.PRNT_GRP_NM });
        //    userDialog.Title = "유저 추가";
        //    userDialog.Owner = Application.Current.MainWindow;
        //    userDialog.BorderEffect = BorderEffect.Default;
        //    //userDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
        //    //userDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
        //    bool isDialog = (bool)userDialog.ShowDialog();
        //    if (isDialog)
        //    {
        //        if (userDialog.IsEdit == false)
        //        {
        //            Master_Search("");
        //            this.GridEditView_menu.FocusedRowHandle = selectedRowhandle;
        //        }
        //    }
        //}
    //    private void user_edit_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
    //    {
    //        GroupUserVo dao = (GroupUserVo)ConfigViewPage1Edit_Master.GetFocusedRow();
    //        if (dao == null)
    //        {
    //            WinUIMessageBox.Show("유저 만 추가/수정/삭제가 가능 합니다.", "[자재 관리 시스템]사용자 권한 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
    //            return;
    //        }
    //        else if (dao.IS_GROUP.Equals("G"))
    //        {
    //            WinUIMessageBox.Show("유저 만 추가/수정/삭제가 가능 합니다.", "[자재 관리 시스템]사용자 권한 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
    //            return;
    //        }

    //        GroupUserVo daoEdit = SystemProperties.AuthClient.SelectUserList(dao)[0];

    //        int selectedRowhandle = this.GridEditView_menu.FocusedRowHandle;

    //        userDialog = new S136UserDialog(daoEdit);
    //        userDialog.Title = "유저 수정";
    //        userDialog.Owner = Application.Current.MainWindow;
    //        userDialog.BorderEffect = BorderEffect.Default;
    //        //userDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
    //        //userDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
    //        bool isDialog = (bool)userDialog.ShowDialog();
    //        if (isDialog)
    //        {
    //            if (userDialog.IsEdit == true)
    //            {
    //                Master_Search("");
    //                this.GridEditView_menu.FocusedRowHandle = selectedRowhandle;
    //            }
    //        }
    //    }
    //    private void user_del_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
    //    {
    //        GroupUserVo dao = (GroupUserVo)ConfigViewPage1Edit_Master.GetFocusedRow();
    //        if (dao != null)
    //        {
    //            if (!dao.IS_GROUP.Equals("G"))
    //            {
    //                TreeListNode node = GridEditView_menu.FocusedNode;
    //                if (node.HasChildren)
    //                {
    //                    WinUIMessageBox.Show("그룹에 포함된 그룹 및 유저가 존재 합니다. ", "[자재 관리 시스템]사용자 권한 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
    //                    return;
    //                }
    //                int selectedRowhandle = node.ParentNode.RowHandle;
    //                MessageBoxResult result = WinUIMessageBox.Show("[" + dao.GRP_NM + "]" + dao.USR_ID + " 정말로 삭제 하시겠습니까?", "[삭제 - 유저]사용자 권한 관리", MessageBoxButton.YesNo, MessageBoxImage.Question);
    //                if (result == MessageBoxResult.Yes)
    //                {
    //                    SystemProperties.AuthClient.DeleteUserProgram(new ProgramVo() { USR_ID = dao.USR_ID });
    //                    SystemProperties.AuthClient.DeleteUserInfo(dao);
    //                    WinUIMessageBox.Show("삭제가 완료되었습니다.", "[삭제 - 유저]사용자 권한 관리", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.None);
    //                    Master_Search("");
    //                    this.GridEditView_menu.FocusedRowHandle = selectedRowhandle;
    //                    node = GridEditView_menu.FocusedNode;
    //                    node.ExpandAll();
    //                }
    //            }
    //            else
    //            {
    //                WinUIMessageBox.Show("유저 만 추가/수정/삭제가 가능 합니다.", "[자재 관리 시스템]사용자 권한 관리", MessageBoxButton.OK, MessageBoxImage.Warning);
    //                return;
    //            }
    //        }
    //    }
    //    private void menu_edit_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
    //    {
    //        GroupUserVo dao = (GroupUserVo)ConfigViewPage1Edit_Master.GetFocusedRow();
    //        //
    //        menuDialog = new S136MenuDialog(dao);
    //        menuDialog.Title = "메뉴 권한 등록";
    //        menuDialog.Owner = Application.Current.MainWindow;
    //        menuDialog.BorderEffect = BorderEffect.Default;
    //        //menuDialog.BorderEffectActiveColor = new SolidColorBrush(Color.FromRgb(255, 128, 0));
    //        //menuDialog.BorderEffectInactiveColor = new SolidColorBrush(Color.FromRgb(255, 170, 170));
    //        bool isDialog = (bool)menuDialog.ShowDialog();
    //        if (isDialog)
    //        {
    //            IList<ProgramVo> resultMenuList;
    //            if (dao == null)
    //            {
    //                return;
    //            }
    //            //
    //            if (dao.IS_GROUP.Equals("G"))
    //            {
    //                resultMenuList = SystemProperties.AuthClient.SelectProgramGroupList(new ProgramVo() { GRP_ID = dao.GRP_ID });
    //            }
    //            else
    //            {
    //                resultMenuList = SystemProperties.AuthClient.SelectProgramUserList(new ProgramVo() { GRP_ID = dao.PRNT_GRP_ID, USR_ID = dao.USR_ID });
    //            }
    //            SystemProperties.MENU_IMG_SET(resultMenuList);
    //            this.menuGridControl.ItemsSource = resultMenuList;
    //        }
    //    }
    }
}