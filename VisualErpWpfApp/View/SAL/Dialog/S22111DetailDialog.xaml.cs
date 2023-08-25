using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Code;
using ModelsLibrary.Sale;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using AquilaErpWpfApp3.Util;
using DevExpress.Xpf.Editors;

namespace AquilaErpWpfApp3.View.SAL.Dialog
{
    public partial class S22111DetailDialog : DXWindow
    {
        private SaleVo orgDao;
        private string title = "출하 의뢰 등록";

        public S22111DetailDialog(SaleVo Dao)
        {
            InitializeComponent();
            

            this.orgDao = Dao;
            //this.ViewGridMst.MouseDoubleClick += ViewGridMst_MouseDoubleClick;
            //this.ViewGridMst.MouseLeftButtonUp += ViewGridMst_MouseButtonUp;
            //this.btn_Search.Click += btn_Search_Click;

            SYSTEM_CODE_VO();

            this.OKButton.Click += new RoutedEventHandler(OKButton_Click);
            this.CancelButton.Click += new RoutedEventHandler(CancelButton_Click);
        }

        //void btn_Search_Click(object sender, RoutedEventArgs e)
        //{
        //    //ViewGridLeft_SelectedItemChanged(sender, null);
        //}

        //void ViewGridMst_MouseButtonUp(object sender, MouseButtonEventArgs e)
        //{

        //}

        //void ViewGridMst_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        //{

        //}

        private void PART_Editor_Checked(object sender, RoutedEventArgs e)
        {
            this.OKButton.IsEnabled = true;
        }

        private void CheckEdit_Checked(object sender, RoutedEventArgs e)
        {
            CheckEdit checkEdit = sender as CheckEdit;
            SaleVo tmpImsi;
            for (int x = 0; x < this.ViewGridMst.VisibleRowCount; x++)
            {
                int rowHandle = this.ViewGridMst.GetRowHandleByVisibleIndex(x);
                if (rowHandle > -1)
                {
                    tmpImsi = this.ViewGridMst.GetRow(rowHandle) as SaleVo;
                    if (checkEdit.IsChecked == true)
                    {
                        tmpImsi.isCheckd = true;
                        //
                        this.OKButton.IsEnabled = true;
                    }
                    else
                    {
                        tmpImsi.isCheckd = false;
                    }

                }
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

        #region Functon (OKButton_Click, CancelButton_Click)
        private async void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int _Num = 0;
                IList<SaleVo> CheckVoList = (this.ViewGridMst.ItemsSource as IList<SaleVo>).Where(x => x.isCheckd == true).ToList<SaleVo>();
 
                if (CheckVoList.Count < 1)
                { return; }
                for(int i = 0; i<CheckVoList.Count; i++)
                {
                    CheckVoList[i].SL_BIL_NO = this.orgDao.SL_BIL_NO;
                    CheckVoList[i].CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
                    CheckVoList[i].CRE_USR_ID = SystemProperties.USER;
                    CheckVoList[i].UPD_USR_ID = SystemProperties.USER;
                }


                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s221111/dtl/i", new StringContent(JsonConvert.SerializeObject(CheckVoList), System.Text.Encoding.UTF8, "application/json")))
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
                        WinUIMessageBox.Show("[요청 번호 : " + this.orgDao.SL_BIL_NO + "] 완료 되었습니다", title, MessageBoxButton.OK, MessageBoxImage.Information);
                        this.DialogResult = true;
                        this.Close();
                    }
                }
            }
            catch (Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
        #endregion

        private void GridColumn_Validate(object sender, DevExpress.Xpf.Grid.GridCellValidationEventArgs e)
        {
            //SaleVo masterDomain = (SaleVo)ViewGridMst.GetFocusedRow();

            //try
            //{
            //    bool slItmQty = (e.Column.FieldName.ToString().Equals("SL_ITM_QTY") ? true : false);
            //    bool slItmRmk = (e.Column.FieldName.ToString().Equals("SL_ITM_RMK") ? true : false);

            //    if (slItmRmk)
            //    {
            //        if (e.IsValid)
            //        {
            //            if (string.IsNullOrEmpty(masterDomain.SL_ITM_RMK))
            //            {
            //                masterDomain.SL_ITM_RMK = "";
            //            }
            //            //
            //            if (!masterDomain.SL_ITM_RMK.Equals((e.Value == null ? "" : e.Value.ToString())))
            //            {
            //                masterDomain.CRE_USR_ID = SystemProperties.USER;
            //                masterDomain.UPD_USR_ID = SystemProperties.USER;
            //                masterDomain.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
            //                masterDomain.SL_ITM_RMK = e.Value.ToString();
            //            }
            //        }
            //    }


            //    else if (slItmQty)
            //    {
            //        if (e.IsValid)
            //        {
            //            if (string.IsNullOrEmpty(masterDomain.SL_ITM_QTY + ""))
            //            {
            //                masterDomain.SL_ITM_QTY = 0;
            //            }

            //            if (!masterDomain.SL_ITM_QTY.Equals((e.Value == null ? "0" : e.Value.ToString())))
            //            {

            //                masterDomain.SL_ITM_QTY = int.Parse(e.Value.ToString());
            //                masterDomain.CRE_USR_ID = SystemProperties.USER;
            //                masterDomain.UPD_USR_ID = SystemProperties.USER;
            //                masterDomain.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;

            //            }
            //        }
            //    }
            //}
            //catch (Exception eLog)
            //{
            //    e.ErrorType = DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical;
            //    e.ErrorContent = eLog.Message;
            //    e.SetError(e.ErrorContent, e.ErrorType);
            //    this.MSG.Text = e.ErrorContent.ToString();
            //    return;
            //}
        }

        private void ViewGridMst_HiddenEditor(object sender, DevExpress.Xpf.Grid.EditorEventArgs e)
        {
            this.ViewTableMst.CommitEditing();

        }

        public async void SYSTEM_CODE_VO()
        {
            try
            {
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s221111/dtl/pop", new StringContent(JsonConvert.SerializeObject(this.orgDao), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        this.ViewGridMst.ItemsSource = JsonConvert.DeserializeObject<IEnumerable<SaleVo>>(await response.Content.ReadAsStringAsync()).Cast<SaleVo>().ToList();
                    }
                }
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[에러]" + title, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

        }

        //IsEdit
        public bool IsEdit
        {
            get
            {
                return this.IsEdit;
            }
        }
    }
}
