using AquilaErpWpfApp3.Util;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Code;
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

namespace AquilaErpWpfApp3.View.S.Dialog
{
    public partial class S1150MasterDialog : DXWindow
    {
        private string _title = "운송차량관리";
        private SystemCodeVo orgDao;
        private bool isEdit = false;
        private SystemCodeVo updateDao;
        public S1150MasterDialog(SystemCodeVo Dao)
        {
            InitializeComponent();

            //SYSTEM_CODE_VO();

            this.orgDao = Dao;

            SystemCodeVo copyDao = new SystemCodeVo()
            {
                CAR_ID = Dao.CAR_ID,
                CAR_NO = Dao.CAR_NO,
                CAR_NM = Dao.CAR_NM,
                CAR_PHN_NO = Dao.CAR_PHN_NO,
                CAR_DESC = Dao.CAR_DESC,
                CRE_USR_ID = Dao.CRE_USR_ID,
                UPD_USR_ID = Dao.UPD_USR_ID,
            };


            if (Dao.CAR_ID != null)
            {
                this.isEdit = true;
            }


            this.configCode.DataContext = copyDao;
            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
            this.OKButton.Click += new RoutedEventHandler(OKButton_Click);
            this.CancelButton.Click += new RoutedEventHandler(CancelButton_Click);
        }

        #region 이벤트

        private async void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValueCheckd())
            {

                int _Num = 0;
                //ProgramVo resultVo;
                if (isEdit == false)
                {
                    this.updateDao = getDomain();//this.updateDao
                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("S1150/i", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
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

                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("S1150/u", new StringContent(JsonConvert.SerializeObject(this.updateDao), System.Text.Encoding.UTF8, "application/json")))
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

        #region 유효성검사

        public Boolean ValueCheckd()
        {
            if (string.IsNullOrEmpty(this.text_CAR_NO.Text))
            {
                WinUIMessageBox.Show("[차량번호] 입력 값이 맞지 않습니다.", "[유효검사]" + _title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.text_CAR_NO.IsTabStop = true;
                this.text_CAR_NO.Focus();
                return false;
            }
            //else if (string.IsNullOrEmpty(this.combo_N1ST_LOC_NM.Text))
            //{
            //    WinUIMessageBox.Show("[위치(동)] 입력 값이 맞지 않습니다.", "[유효검사]창고위치관리(완제품)", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.combo_N1ST_LOC_NM.IsTabStop = true;
            //    this.combo_N1ST_LOC_NM.Focus();
            //    return false;
            //}
            //else if (string.IsNullOrEmpty(this.combo_N2ND_LOC_NM.Text))
            //{
            //    WinUIMessageBox.Show("[열] 입력 값이 맞지 않습니다.", "[유효검사]창고위치관리(완제품)", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    this.combo_N2ND_LOC_NM.IsTabStop = true;
            //    this.combo_N2ND_LOC_NM.Focus();
            //    return false;
            //}

            return true;
        }
        #endregion



        #region getDomain(), SYSTEM_CODE_VO()

        private SystemCodeVo getDomain()
        {
            SystemCodeVo Dao = new SystemCodeVo();

            if(this.orgDao.CAR_ID != null)
            {
                Dao.CAR_ID = this.orgDao.CAR_ID;
            }
            
            Dao.CAR_NM = this.text_CAR_NM.Text;
            Dao.CAR_NO = this.text_CAR_NO.Text;
            Dao.CAR_PHN_NO = this.text_CAR_PHN_NO.Text;
            Dao.CAR_DESC = this.text_CAR_DESC.Text;
            Dao.CRE_USR_ID = SystemProperties.USER;
            Dao.UPD_USR_ID = SystemProperties.USER;

            return Dao;
        }


        //public async void SYSTEM_CODE_VO()
        //{

        //}
        #endregion


        //IsEdit
        public bool IsEdit
        {
            get
            {
                return this.isEdit;
            }
        }

        //Vo
        public SystemCodeVo resultDomain
        {
            get
            {
                return this.updateDao;
            }
        }

    }
}
