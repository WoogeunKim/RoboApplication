using AquilaErpWpfApp3.Util;
using DevExpress.Xpf.WindowsUI;
using ModelsLibrary.Code;
using Newtonsoft.Json;
using ShapeImageLib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Reflection;
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
using Binding = System.Windows.Data.Binding;
using Color = System.Drawing.Color;

namespace AquilaErpWpfApp3.View.S
{
    /// <summary>
    /// S1415.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class S1415 : UserControl
    {
        string _title = "형상(Shape) 좌표 관리";

        ShapeImageInfo ShapeData = null;
        string ShapeNum = "";

        public S1415()
        {
            InitializeComponent();

            // 형상 코드 조회
            SYSTEM_CODE_VO();

            // 조회
            this.M_ref.ItemClick += new DevExpress.Xpf.Bars.ItemClickEventHandler(btnSearch_Click);
            
            // 저장
            this.M_save.ItemClick += new DevExpress.Xpf.Bars.ItemClickEventHandler(btnSave_Click);
        }


        private async void SYSTEM_CODE_VO()
        {
            using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s141/mini", new StringContent(JsonConvert.SerializeObject(new SystemCodeVo() { ITM_GRP_CLSS_CD = "P", DELT_FLG = "N", CHNL_CD = SystemProperties.USER_VO.CHNL_CD }), System.Text.Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    var ShapeItmList = JsonConvert.DeserializeObject<IEnumerable<ItemCodeVo>>(await response.Content.ReadAsStringAsync()).Cast<ItemCodeVo>().ToList();

                    this.M_ITM_LIST.ItemsSource = ShapeItmList;
                }
            }
        }


        /// <summary>
        /// 검색
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (this.combo_M_ITM_NM.EditValue == null) return;

            ShapeNum = this.combo_M_ITM_NM.EditValue.ToString();

            if (String.IsNullOrEmpty(ShapeNum))
            {
                // 빈 값
            }
            else
            {
                // 형상 정보 가져온다.
                var SampleData = GetShapeInfo();

                // 형상 정보 좌표값을 가져온다.
                var SampleDataPoint = await GetShapePosInfo();

                // 형상 이미지 정보를 가져온다.
                var SampleDataImage = await GetShapeImage();


                // ************************************************************************************************************************

                ShapeData = new ShapeImageInfo();

                // 코드 정보
                ShapeData.Code = SampleData["Code"].ToString();

                // 이미지 정보
                ShapeData.Image = SampleDataImage;

                // 반드시 순서대로 넣는다.(DB 열 순서대로....)
                ShapeData.Datas.Add(new ShapeImagePointInfo("A 길이", SampleData["A_Len"], SampleDataPoint["A_Len"]));
                ShapeData.Datas.Add(new ShapeImagePointInfo("B 길이", SampleData["B_Len"], SampleDataPoint["B_Len"]));
                ShapeData.Datas.Add(new ShapeImagePointInfo("C 길이", SampleData["C_Len"], SampleDataPoint["C_Len"]));
                ShapeData.Datas.Add(new ShapeImagePointInfo("D 길이", SampleData["D_Len"], SampleDataPoint["D_Len"]));
                ShapeData.Datas.Add(new ShapeImagePointInfo("E 길이", SampleData["E_Len"], SampleDataPoint["E_Len"]));
                ShapeData.Datas.Add(new ShapeImagePointInfo("F 길이", SampleData["F_Len"], SampleDataPoint["F_Len"]));
                ShapeData.Datas.Add(new ShapeImagePointInfo("A 각도", SampleData["A_Angle"], SampleDataPoint["A_Angle"]));
                ShapeData.Datas.Add(new ShapeImagePointInfo("B 각도", SampleData["B_Angle"], SampleDataPoint["B_Angle"]));
                ShapeData.Datas.Add(new ShapeImagePointInfo("C 각도", SampleData["C_Angle"], SampleDataPoint["C_Angle"]));
                ShapeData.Datas.Add(new ShapeImagePointInfo("D 각도", SampleData["D_Angle"], SampleDataPoint["D_Angle"]));
                ShapeData.Datas.Add(new ShapeImagePointInfo("E 각도", SampleData["E_Angle"], SampleDataPoint["E_Angle"]));
                ShapeData.Datas.Add(new ShapeImagePointInfo("F 각도", SampleData["F_Angle"], SampleDataPoint["F_Angle"]));

                // 데이터 로딩
                usertool.DataLoad(ShapeData);

                // ************************************************************************************************************************
            }
        }

        /// <summary>
        /// 데이터 저장 하기
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBoxResult.Yes == messageResultYseNo("저장 하시겠습니까?"))
            {
                SaveData();
            }
        }


        private MessageBoxResult messageResultYseNo(string _message)
        {
            var ret = WinUIMessageBox.Show(_message, _title, MessageBoxButton.YesNo, MessageBoxImage.Question);

            return ret;
        }




        /// <summary>
        /// 레포트시 이미지 변환 예제
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void btnReport_Click(object sender, RoutedEventArgs e)
        //{
        //    var ResultData = new List<byte[]>();

        //    // 예제1
        //    ShapeImageInfo ShapeData = new ShapeImageInfo();
        //    ShapeData.Image = GetShapeImage();                                          // DB 불러오기 - 이미지 정보
        //    ShapeData.Datas.Add(new ShapeImagePointInfo("A 길이", "200", "36,57,1"));   // DB 불러오기 - 형상 좌표 정보(좌표명,값,좌표정보)
        //    ShapeData.Datas.Add(new ShapeImagePointInfo("A 각도", "45", "119,83,1"));
        //    ShapeData.Datas.Add(new ShapeImagePointInfo("B 길이", "4560", "93,32,1"));
        //    ShapeData.Datas.Add(new ShapeImagePointInfo("C 길이", "300", "137,61,2"));
        //    ShapeData.Datas.Add(new ShapeImagePointInfo("D 길이", "400", "156,99,1"));


        //    var ImageResult = ShapeImageLibFunc.TransShapeImage(ShapeData);             // DB 불러오기 - 실제 보고서 바인딩에서 이 함수 사용 하여 변환
        //    ResultData.Add(ImageResult);

        //    // 예제2
        //    ShapeImageInfo ShapeData2 = new ShapeImageInfo();
        //    ShapeData2.Image = GetShapeImage();                                          // DB 불러오기 - 이미지 정보
        //    ShapeData2.Datas.Add(new ShapeImagePointInfo("A 길이", "800", "93,32,1"));   // DB 불러오기 - 형상 좌표 정보(좌표명,값,좌표정보)
        //    var ImageResult2 = ShapeImageLibFunc.TransShapeImage(ShapeData2);            // DB 불러오기 - 실제 보고서 바인딩에서 이 함수 사용 하여 변환
        //    ResultData.Add(ImageResult2);

        //    // 예제3
        //    ShapeImageInfo ShapeData3 = new ShapeImageInfo();
        //    ShapeData3.Image = GetShapeImage();                                          // DB 불러오기 - 이미지 정보
        //    ShapeData3.Datas.Add(new ShapeImagePointInfo("A 각도", "45", "120,80,1"));   // DB 불러오기 - 형상 좌표 정보(좌표명,값,좌표정보)
        //    var ImageResult3 = ShapeImageLibFunc.TransShapeImage(ShapeData3);            // DB 불러오기 - 실제 보고서 바인딩에서 이 함수 사용 하여 변환
        //    ResultData.Add(ImageResult3);


        //    ReportExample frm = new ReportExample(ResultData);
        //    frm.ShowDialog();
        //}


        /// <summary>
        /// 형상 정보를 가져온다.
        /// </summary>
        /// <returns></returns>
        private DataRow GetShapeInfo()
        {
            DataRow ret = null;

            // 형상좌표정보 Table Column(예상)  :  좌표정보 타입  string
            // 형상코드 / A길이 / A각도 / B길이 / B각도 / C길이 / C각도 / D길이 / D각도 / E길이 / E각도 / F길이 / F각도

            if (false)
            {
                // 실제 DB 가져오는 구문
            }
            else
            {
                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("Code", typeof(string)));     // 형상코드
                dt.Columns.Add(new DataColumn("A_Len", typeof(string)));    // A 길이
                dt.Columns.Add(new DataColumn("B_Len", typeof(string)));    // B 길이 
                dt.Columns.Add(new DataColumn("C_Len", typeof(string)));    // C 길이 
                dt.Columns.Add(new DataColumn("D_Len", typeof(string)));    // D 길이 
                dt.Columns.Add(new DataColumn("E_Len", typeof(string)));    // E 길이 
                dt.Columns.Add(new DataColumn("F_Len", typeof(string)));    // F 길이
                dt.Columns.Add(new DataColumn("A_Angle", typeof(string)));  // A 각도
                dt.Columns.Add(new DataColumn("B_Angle", typeof(string)));  // B 각도
                dt.Columns.Add(new DataColumn("C_Angle", typeof(string)));  // C 각도
                dt.Columns.Add(new DataColumn("D_Angle", typeof(string)));  // D 각도
                dt.Columns.Add(new DataColumn("E_Angle", typeof(string)));  // E 각도
                dt.Columns.Add(new DataColumn("F_Angle", typeof(string)));  // F 각도

                // 샘플 데이터
                ret = dt.NewRow();
                ret["Code"] = ShapeNum;       // 코드
                ret["A_Len"] = "1111";        // A 길이 
                ret["B_Len"] = "2222";
                ret["C_Len"] = "3333";
                ret["D_Len"] = "4444";
                ret["E_Len"] = "5555";
                ret["F_Len"] = "6666";
                ret["A_Angle"] = "111";       // A 각도 
                ret["B_Angle"] = "222";
                ret["C_Angle"] = "333";
                ret["D_Angle"] = "444";
                ret["E_Angle"] = "555";
                ret["F_Angle"] = "666";
            }

            return ret;
        }

        /// <summary>
        /// 형상 정보의 좌표값을 가져온다.
        /// </summary>
        /// <returns></returns>
        private async Task<DataRow> GetShapePosInfo()
        {
            DataRow ret = null;

            // 형상좌표정보 Table Column(예상)  :  좌표정보 타입  string
            // 형상코드 / A길이 / A각도 / B길이 / B각도 / C길이 / C각도 / D길이 / D각도 / E길이 / E각도 / F길이 / F각도
            if (ShapeNum != null)
            {
                // DB 에서 저장된 좌표 정보 조회
                var posDao = await GetShapePosDBList(); 

                DataTable dtpt = new DataTable();
                dtpt.Columns.Add(new DataColumn("Code", typeof(string)));     // 형상코드
                dtpt.Columns.Add(new DataColumn("A_Len", typeof(string)));    // A 길이 좌표 정보
                dtpt.Columns.Add(new DataColumn("B_Len", typeof(string)));    // B 길이 좌표 정보
                dtpt.Columns.Add(new DataColumn("C_Len", typeof(string)));    // C 길이 좌표 정보
                dtpt.Columns.Add(new DataColumn("D_Len", typeof(string)));    // D 길이 좌표 정보
                dtpt.Columns.Add(new DataColumn("E_Len", typeof(string)));    // E 길이 좌표 정보
                dtpt.Columns.Add(new DataColumn("F_Len", typeof(string)));    // F 길이 좌표 정보
                dtpt.Columns.Add(new DataColumn("A_Angle", typeof(string)));  // A 각도 좌표 정보
                dtpt.Columns.Add(new DataColumn("B_Angle", typeof(string)));  // B 각도 좌표 정보
                dtpt.Columns.Add(new DataColumn("C_Angle", typeof(string)));  // C 각도 좌표 정보
                dtpt.Columns.Add(new DataColumn("D_Angle", typeof(string)));  // D 각도 좌표 정보
                dtpt.Columns.Add(new DataColumn("E_Angle", typeof(string)));  // E 각도 좌표 정보
                dtpt.Columns.Add(new DataColumn("F_Angle", typeof(string)));  // F 각도 좌표 정보

                // 샘플 데이터
                ret = dtpt.NewRow();
                ret["Code"] = posDao.ITM_CD;   // 코드

                ret["A_Len"] = posDao.A_LEN;   // 예제 코드
                ret["B_Len"] = posDao.B_LEN;
                ret["C_Len"] = posDao.C_LEN;
                ret["D_Len"] = posDao.D_LEN;
                ret["E_Len"] = posDao.E_LEN;
                ret["F_Len"] = posDao.F_LEN;
                ret["A_Angle"] = posDao.A_VAL;
                ret["B_Angle"] = posDao.B_VAL;
                ret["C_Angle"] = posDao.C_VAL;
                ret["D_Angle"] = posDao.D_VAL;
                ret["E_Angle"] = posDao.E_VAL;
                ret["F_Angle"] = posDao.F_VAL;

            }

            return ret;
        }

        /// <summary>
        /// 형상 이미지 정보를 가져온다.
        /// </summary>
        /// <returns></returns>
        private async Task<byte[]> GetShapeImage()
        {
            byte[] ret = null;

            try
            {
                if (ShapeNum != null)
                {
                    var ImgArray = await PostJsonData<SystemCodeVo>("s1415/img", new SystemCodeVo() { ITM_CD = ShapeNum, CHNL_CD = SystemProperties.USER_VO.CHNL_CD });

                    if (ImgArray != null)
                        ret = ImgArray.ITM_IMG;
                }
            }
            catch(Exception ex)
            {

            }

            return ret;
        }

        /// <summary>
        /// 형상 좌표 정보를 저장 합니다.
        /// </summary>
        private async void SaveData()
        {
            try
            {
                var SaveData = usertool.GetData;
                if (SaveData == null)
                    return;

                // 형상좌표정보 Table Column(예상)  :  좌표정보 타입  string
                // 형상코드 / A길이 / A각도 / B길이 / B각도 / C길이 / C각도 / D길이 / D각도 / E길이 / E각도 / F길이 / F각도

                SystemCodeVo updateVo = new SystemCodeVo();

                updateVo.ITM_CD = SaveData.Code;             // 형상코드
                updateVo.A_LEN = SaveData.Datas[0].Point;   // A길이 위치값
                updateVo.B_LEN = SaveData.Datas[1].Point;   // B길이 위치값
                updateVo.C_LEN = SaveData.Datas[2].Point;   // C길이 위치값
                updateVo.D_LEN = SaveData.Datas[3].Point;   // D길이 위치값
                updateVo.E_LEN = SaveData.Datas[4].Point;   // E길이 위치값
                updateVo.F_LEN = SaveData.Datas[5].Point;   // F길이 위치값
                updateVo.A_VAL = SaveData.Datas[6].Point;   // A각도 위치값
                updateVo.B_VAL = SaveData.Datas[7].Point;   // B각도 위치값
                updateVo.C_VAL = SaveData.Datas[8].Point;  // C각도 위치값
                updateVo.D_VAL = SaveData.Datas[9].Point;  // D각도 위치값
                updateVo.E_VAL = SaveData.Datas[10].Point; // E각도 위치값
                updateVo.F_VAL = SaveData.Datas[11].Point; // F각도 위치값


                if (updateVo.ITM_CD != null)
                {
                    // 실제 DB 저장 되는 부분

                    using (HttpResponseMessage response = await SystemProperties.PROGRAM_HTTP.PostAsync("s1415/mst/i", new StringContent(JsonConvert.SerializeObject(updateVo), System.Text.Encoding.UTF8, "application/json")))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            int _Num = 0;

                            string result = await response.Content.ReadAsStringAsync();
                            if (int.TryParse(result, out _Num) == false)
                            {
                                WinUIMessageBox.Show(result, _title, MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                            WinUIMessageBox.Show("저장 되었습니다.", _title, MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }
            }
            catch(Exception ex)
            {

            }
        }



        private async Task<SystemCodeVo> GetShapePosDBList()
        {
            if (ShapeNum == null) return new SystemCodeVo();

            var PItemDao = await PostJsonData<SystemCodeVo>("s1415/mst", new SystemCodeVo() { ITM_CD = ShapeNum });

            
            if(PItemDao == null)
            {
                return new SystemCodeVo() { ITM_CD = ShapeNum };
            }

            return PItemDao;
        }


        /// <summary>
        /// Json를 통하여 서버 정보를 가져옵니다
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Path"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        private async Task<T> PostJsonData<T>(string Path, object obj)
        {
            // SettingHttp 기본값 사용시 
            return await PostJsonData<T>(Properties.Settings.Default.SettingHttp, Path, obj);
        }

        /// <summary>
        /// Json를 통하여 서버 정보를 가져옵니다.[핵심함수]
        /// </summary>
        /// <param name="url"></param>
        /// <param name="Path"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        private async Task<T> PostJsonData<T>(string url, string Path, object obj)
        {
            // default - > 기본값 NULL
            T ret = default(T);

            try
            {
                string DataType = "application/json";

                using (HttpClient _client = new HttpClient())
                {
                    _client.BaseAddress = new Uri(url);
                    _client.Timeout = TimeSpan.FromSeconds(10);
                    _client.DefaultRequestHeaders.Accept.Clear();
                    _client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(DataType));


                    using (HttpResponseMessage response = await _client.PostAsync(Path,
                                                                       new StringContent(JsonConvert.SerializeObject(obj), System.Text.Encoding.UTF8, DataType)))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            ret = JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
                        }
                    }
                }

            }
            catch (Exception eLog)
            {
                WinUIMessageBox.Show(eLog.Message, _title, MessageBoxButton.OK, MessageBoxImage.Error);
                // 에러시... 디버깅...
            }

            return ret;
        }


    }
}
