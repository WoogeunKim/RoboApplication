using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Code;
using ModelsLibrary.Man;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;
using AquilaErpWpfApp3.Util;
using AquilaErpWpfApp3.View.M.Dialog;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using System.Threading.Tasks;
using System.Windows.Media;

namespace AquilaErpWpfApp3.View.M.Dialog
{
    public partial class M661010DetailDialog : DXWindow
    {
        private IList<ManVo> orgDaoList;
        public IList<ManVo> updateDaoList;

        private string title = "생산계획 관리";

        private IList<ManVo> updateDao = new List<ManVo>();

        public M661010DetailDialog(IList<ManVo> DaoList)
        {
            InitializeComponent();

            this.orgDaoList = DaoList;

            if (DaoList.Count > 1)
            {
                this.txt_prod_qty.Visibility = Visibility.Hidden;
                this.text_PROD_QTY.Visibility = Visibility.Hidden;
            }
            else
            {
                this.txt_prod_qty.Visibility = Visibility.Visible;
                this.text_PROD_QTY.Visibility = Visibility.Visible;
            }


            this.PreviewKeyDown += new KeyEventHandler(HandleEsc);
            this.OKButton.Click += new RoutedEventHandler(OKButton_Click);
            this.CancelButton.Click += new RoutedEventHandler(CancelButton_Click);
        }


        #region Functon (OKButton_Click, CancelButton_Click)
        private async void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValueCheckd())
            {
                try
                {

                    this.updateDaoList = getDomain();

                    // 생산계획 추가
                    await PostJsonUpdate("m661010/dtl/prod/i", updateDaoList);

                    //중량 업데이트
                    await PostJsonUpdate("m661010/wgt/u", updateDaoList);

                    // 성공
                    WinUIMessageBox.Show("완료 되었습니다", title, MessageBoxButton.OK, MessageBoxImage.Information);

                    if (DXSplashScreen.IsActive == true) DXSplashScreen.Close();
                    this.DialogResult = true;
                    this.Close();
                }
                catch (Exception eLog)
                {
                    WinUIMessageBox.Show(eLog.Message, title, MessageBoxButton.OK, MessageBoxImage.Error);
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
        #endregion

        #region Functon (ValueCheckd)
        public Boolean ValueCheckd()
        {
            //생산일자
            if (string.IsNullOrEmpty(this.text_ORD_STT_DT.Text))
            {
                WinUIMessageBox.Show("[생산일자]를 선택하지 않았습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                this.text_ORD_STT_DT.IsTabStop = true;
                this.text_ORD_STT_DT.Focus();
                return false;
            }

            //작업지시수량
            if (this.orgDaoList.Count == 1)
            {
                if (string.IsNullOrEmpty(this.text_PROD_QTY.Text) || Convert.ToInt32(this.text_PROD_QTY.Text) <= 0)
                {
                    WinUIMessageBox.Show("[작업지시수량] 값이 0이하거나 입력되지 않았습니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                    this.text_ORD_STT_DT.IsTabStop = true;
                    this.text_ORD_STT_DT.Focus();
                    return false;
                }

                if ((Convert.ToDouble(this.orgDaoList[0].RMN_QTY) < Convert.ToDouble(this.text_PROD_QTY.Text)))
                {
                    WinUIMessageBox.Show("오더잔여수량보다 작업지시수량이 큽니다.", "[유효검사]" + title, MessageBoxButton.OK, MessageBoxImage.Warning);
                    this.text_ORD_STT_DT.IsTabStop = true;
                    this.text_ORD_STT_DT.Focus();
                    return false;
                }
            }
            return true;
        }



        /// <summary>
        /// Post JSON 을 통해 서버로부터 정보를 가져옵니다.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Path"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        private async Task<List<T>> PostJsonList<T>(string Path, Object obj)
        {
            var ret = default(List<T>);

            try
            {
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync(Path, new StringContent(JsonConvert.SerializeObject(obj), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        ret = JsonConvert.DeserializeObject<IEnumerable<T>>(await response.Content.ReadAsStringAsync()).Cast<T>().ToList();
                    }
                }
            }
            catch (Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, title, MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return ret;
        }

        /// <summary>
        /// 변경할 정보를 서버에게 전달합니다.
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        private async Task PostJsonUpdate(string Path, object obj)
        {
            try
            {
                using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync(Path
                                                                                                  , new StringContent(JsonConvert.SerializeObject(obj), System.Text.Encoding.UTF8, "application/json")))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        int _Num = 0;
                        string resultMsg = await response.Content.ReadAsStringAsync();
                        if (int.TryParse(resultMsg, out _Num) == false)
                        {
                            //실패
                            WinUIMessageBox.Show(resultMsg, title + "- 수정 오류", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                        {
                            // 성공
                        }
                    }
                }
            }
            catch (Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, title + "- 수정 오류", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.None);
            }
        }

        #endregion

        #region Functon (getDomain)
        private IList<ManVo> getDomain()
        {
            IList<ManVo> NewList = new List<ManVo>();

            for (int i = 0; i < this.orgDaoList.Count; i++)
            {
                ManVo newItem = new ManVo(); // 새로운 ManVo 객체 생성

                if (this.orgDaoList.Count > 1)
                {
                    newItem.SL_RLSE_NO = this.orgDaoList[i].SL_RLSE_NO;
                    newItem.SL_RLSE_SEQ = this.orgDaoList[i].SL_RLSE_SEQ;
                    newItem.PROD_QTY = this.orgDaoList[i].RMN_QTY;
                }
                else
                {
                    newItem.SL_RLSE_NO = this.orgDaoList[0].SL_RLSE_NO;
                    newItem.SL_RLSE_SEQ = this.orgDaoList[0].SL_RLSE_SEQ;
                    newItem.PROD_QTY = this.text_PROD_QTY.Text;
                }

                if (DateTime.TryParse(this.text_ORD_STT_DT.Text, out DateTime parsedDate))
                {
                    newItem.ORD_STT_DT = parsedDate.ToString("yyyy-MM-dd");
                }
                newItem.CHNL_CD = SystemProperties.USER_VO.CHNL_CD;

                NewList.Add(newItem); 
            }

            return NewList;
        }

        #endregion


    }
}
