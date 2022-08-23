using AquilaErpWpfApp3.Util;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Code;
using ModelsLibrary.Tec;
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

namespace AquilaErpWpfApp3.View.TEC.Dialog
{
    /// <summary>
    /// T6311MasterDialog.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class T6311MasterDialog : DXWindow
    {
        private string _title = "출하검사";
        private TecVo _updateDao;
        private TecVo _orgDao;
        public T6311MasterDialog(TecVo _dao)
        {
            InitializeComponent();

            if (_dao != null)
            {
                this._orgDao = _dao;
                TecVo copyDao = new TecVo()
                {
                      SL_ITM_CD = _dao.SL_ITM_CD
                    , ITM_NM = _dao.ITM_NM
                    , ITM_SZ_NM = _dao.ITM_SZ_NM
                    , CO_NM = _dao.CO_NM
                    , LOT_NO = _dao.LOT_NO
                    , MTRL_MAKE_DT = _dao.MTRL_MAKE_DT
                    , MTRL_EXP_DT = _dao.MTRL_EXP_DT
                    , INSP_NO = _dao.INSP_NO
                    , INSP_DT = (string.IsNullOrEmpty(_dao.INSP_DT) ? _dao.INSP_DT : DateTime.Now.ToString("yyyy-MM-dd"))
                    //, INSP_FLG = _dao.INSP_FLG
                    //, GBN = _dao.GBN
                    , INSP_FLG = _dao.GBN
                    , ITM_RMK = _dao.ITM_RMK

                };

                if(copyDao.INSP_FLG == null)
                {
                    copyDao.INSP_FLG = "시험중";
                }

                this.configCode.DataContext = copyDao;

            }
            else
            {
                return;
            }


            SYSTEM_CODE_VO();

           // this.text_MTRL_EXP_DT.EditValueChanged += Text_MTRL_EXP_DT_EditValueChanged;

           // this.text_MTRL_EXP_DY.KeyUp += Text_MTRL_EXP_DY_KeyUp;


            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
            this.OKButton.Click += new RoutedEventHandler(OKButton_Click);
            this.CancelButton.Click += new RoutedEventHandler(CancelButton_Click);
        }

        private void Text_MTRL_EXP_DY_KeyUp(object sender, KeyEventArgs e)
        {
            //this.text_MTRL_EXP_DT.Text = Convert.ToDateTime(this.text_MTRL_MAKE_DT.Text).AddDays(Convert.ToInt32(this.text_MTRL_EXP_DY.Text)).ToString("yyyy-MM-dd");
        }

        private void Text_MTRL_EXP_DT_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            try
            {
                DateTime T1 = DateTime.Parse(this.text_MTRL_MAKE_DT.Text);
                DateTime T2 = DateTime.Parse(this.text_MTRL_EXP_DT.Text);

                
                TimeSpan TS = T2 - T1;
                // this.text_MTRL_EXP_DY.Text = ("" + TS.Days);
            }
            catch
            {
                return;
            }
        }


        private async void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int _Num = 0;
                _updateDao = getDomain();
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("t6311/mst/u", new StringContent(JsonConvert.SerializeObject(_updateDao), System.Text.Encoding.UTF8, "application/json")))
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
                        this.DialogResult = true;
                        this.Close();
                    }
                }
            }
            catch (Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, _title, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

        }

        private TecVo getDomain()
        {
            TecVo _Dao = new TecVo();

            _Dao.SL_RLSE_SEQ = _orgDao.SL_RLSE_SEQ;
            _Dao.SL_RLSE_NO = _orgDao.SL_RLSE_NO;
            //_Dao.SL_CO_CD 
            _Dao.LOT_NO = text_LOT_NO.Text;                // 제조번호
            //_Dao.MTRL_EXP_DT = Convert.ToDateTime(this.text_MTRL_EXP_DT.Text).ToString("yyyy-MM-dd");      // 사용기한(날짜)
            //_Dao.MTRL_EXP_DY = Convert.ToInt32(this.text_MTRL_EXP_DY.Text);  // 사용기한(남은 일수)
            _Dao.INSP_NO = text_INSP_NO.Text;     // 시험번호
            _Dao.ITM_RMK = text_ITM_RMK.Text;     // 참고사항(비고)

            //SystemCodeVo coNmVo = this.combo_CO_NM.SelectedItem as SystemCodeVo;
            //_Dao.CO_CD = coNmVo.CO_CD;
            //_Dao.CO_NM = coNmVo.CO_NM;

            _Dao.INSP_FLG = (this.combo_INSP_FLG.Text.Equals("시험중") ? "Z" : (this.combo_INSP_FLG.Text.Equals("적합") ? "Y" : "N"));

            _Dao.UPD_USR_ID = SystemProperties.USER_VO.USR_ID;
            _Dao.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;


            //_Dao.INSP_DT = Convert.ToDateTime(this.text_INSP_DT.Text).ToString("yyyy-MM-dd");     // 시험날짜
            
            if(string.IsNullOrEmpty(this.text_INSP_DT.Text) == false)
            {
                _Dao.INSP_DT = Convert.ToDateTime(this.text_INSP_DT.Text).ToString("yyyy-MM-dd"); 
            }


            //_Dao.MTRL_MAKE_DT = Convert.ToDateTime(this.text_MTRL_MAKE_DT.Text).ToString("yyyy-MM-dd");
            
            if(string.IsNullOrEmpty(this.text_MTRL_MAKE_DT.Text) == false)
            {
                _Dao.MTRL_MAKE_DT = Convert.ToDateTime(this.text_MTRL_MAKE_DT.Text).ToString("yyyy-MM-dd");    
            }



            return _Dao;
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

        public async void SYSTEM_CODE_VO()
        {

            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s143", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { DELT_FLG = "N", CO_TP_CD = "AR", AREA_CD = SystemProperties.USER_VO.EMPE_PLC_NM, CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
            {
                //if (response.IsSuccessStatusCode)
                //{
                //    IList<SystemCodeVo> tmpList = JsonConvert.DeserializeObject<IEnumerable<SystemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<SystemCodeVo>().ToList();
                //    tmpList.Insert(0, new SystemCodeVo() { CO_NO = "", CO_NM = "" });
                //    //
                //    this.combo_CO_NM.ItemsSource = tmpList;
                //}
            }

        }


    }
}
