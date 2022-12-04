using AquilaErpWpfApp3.Util;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Code;
using ModelsLibrary.Sale;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace AquilaErpWpfApp3.View.M.Dialog
{
    /// <summary>
    /// S2233MasterDialog.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class M6722MasterDialog : DXWindow
    {
        private string _title = "실적현황_Dialog";

        IList<SaleVo> _onlyItemsList = new List<SaleVo>();

        public M6722MasterDialog()
        {
            InitializeComponent();

            txt_stDate.EditValue = System.DateTime.Now;
            txt_enDate.EditValue = System.DateTime.Now;

            SYSTEM_CODE_VO();

            this.btn_reset.Click += new RoutedEventHandler(M_ref_ItemClick);
            this.OKButton.Click += new RoutedEventHandler(OKButton_Click);
            this.CancelButton.Click += new RoutedEventHandler(CancelButton_Click);

            this.search_title.Text = "";

        }


        private async void SYSTEM_CODE_VO()
        {
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.GetAsync("s131/dtl/" + Properties.Settings.Default.SettingChnl + "/" + "L-002"))
            {
                if (response.IsSuccessStatusCode)
                {
                    IList<SystemCodeVo> itemList = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                    this.combo_AREA_NM.ItemsSource = itemList;
                    if (itemList.Count > 0)
                    {
                        this.combo_AREA_NM.SelectedItem = itemList[0];
                    }
                }
            }
        }

        private async void M_ref_ItemClick(object sender, RoutedEventArgs e)
        {
            try
            {
                SaleVo _vo = new SaleVo();
                _vo.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
                _vo.AREA_CD = (combo_AREA_NM.SelectedItem as SystemCodeVo).CLSS_CD;
                _vo.FM_DT = ((DateTime)txt_stDate.EditValue).ToString("yyyy-MM-dd");
                _vo.TO_DT = ((DateTime)txt_enDate.EditValue).ToString("yyyy-MM-dd");

                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s2233/dtl", new StringContent(JsonConvert.SerializeObject(_vo), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        _onlyItemsList = JsonConvert.DeserializeObject<IEnumerable<SaleVo>>(await response.Content.ReadAsStringAsync()).Cast<SaleVo>().ToList();
                        this.ViewGridDialog.ItemsSource = _onlyItemsList;

                        this.ViewGridDialog.SelectedItems = new List<SaleVo>();

                        this.search_title.Text = "[조회 조건]   " + "기간 : " + _vo.FM_DT + "~" + _vo.TO_DT + ", 사업장 : " + (combo_AREA_NM.SelectedItem as SystemCodeVo).CLSS_DESC;

                    }
                }
            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }

        private async void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<SaleVo> _selectItemList  = this.ViewGridDialog.SelectedItems as List<SaleVo>;
                if (_selectItemList.Count == 0)
                {
                    WinUIMessageBox.Show("하나 이상 선택하세요", _title, MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                for (int i = 0; i < _selectItemList.Count; i++)
                {
                    if (ValueCheckd(_selectItemList[i]))
                    {
                        _selectItemList[i].CHNL_CD = SystemProperties.USER_VO.CHNL_CD;
                        _selectItemList[i].CRE_USR_ID = SystemProperties.USER_VO.USR_ID;
                        _selectItemList[i].UPD_USR_ID = SystemProperties.USER_VO.USR_ID;
                    }
                }


                int _Num = 0;

                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s2233/i", new StringContent(JsonConvert.SerializeObject(_selectItemList), System.Text.Encoding.UTF8, "application/json")))
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

                this.DialogResult = true;
                this.Close();

            }
            catch (System.Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, "[" + SystemProperties.PROGRAM_TITLE + "]" + _title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
                return;
            }
        }

        private async void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        public Boolean ValueCheckd(SaleVo _one)
        {
            /*if (_one.RMN_QTY > 0 || string.IsNullOrEmpty((string)_one.ITM_PRC))
            {
                WinUIMessageBox.Show("[수주번호 : " + _one.SL_RLSE_NO + " , 순번 : " + _one.SL_RLSE_SEQ + "] 값이 맞지 않습니다.", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            else return true;*/
            return true;
        }
    }
}
