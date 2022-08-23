using ModelsLibrary.Auth;
using System;
using System.Net.Http;
using System.Reflection;
namespace AquilaErpWpfApp3.Util
{
    class SystemProperties
    {
        public static string USER
        {
            get;
            set;
        }

        public static string AUTH
        {
            get;
            set;
        }
        public static string GUID
        {
            get;
            set;
        }

        public static GroupUserVo USER_VO
        {
            get;
            set;
        }

        #region  프로그램 종료 시 체크 옵션 작업 함
        public static bool isLOGIN
        {
            get;
            set;
        }

        public static bool isEND
        {
            get;
            set;
        }
        #endregion

        public static string PROGRAM_NAME
        {
            get { return "/AquilaErpWpfApp3;component/"; }
        }

        public static string PROGRAM_TITLE
        {
            get { return "Aquila ERP"; }
        }

        public static HttpClient PROGRAM_HTTP
        {
            set; get;
        }

        public static string IMG_GROUP_16
        {
            get { return "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"; }
        }

        public static string IMG_USER_16
        {
            get { return "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAe1JREFUeNqMU01rFEEQfdUzs9kdNdlIIhEC4i9wPQVyyO5VENx/4N6D4NmDevLkQRDv8R9EELyuOSzk6B/woDkEE5OJ2ex8T/tqJgi7mbA29HR31avXVa96BDXj9WjrQcyF276eDbDb4PnV5t63WazUBY9hhy1I22eoAiIUnAh8oPdyhsTMEujNGrxifKw113G3dQ+r7hIY3E6rrKaGW5NST29eaqzijn8frjRwHP1AlIc4s0lvFm9qCALh14gDzzTgOc1yT4uCg/8hGMbIMU5PcBT+xK/Jd5wnx4hsou7h3BLakOcBbP93dtbWtEUEYZFQmyK4Rd/cLuh4v//oieMu7mY2LwEOS8izP/1nG18+zSX4sP/46e3m+s7ywhoWKaSl7Tw5wml8iJPoYLC98fnjtRq8HXW7ImansBmSIsRFFmDCqXu1qU8x12qQwb5Li4gtu4BJXWRFUr0NahHlY6gvJYamh1dKeDPa6vIRDX2afOcGWs5NuGahIi5ihCSYkHjComjtvdjc+zqVwRgY5CWjhSVQb3fEK325TRFzRvSFPPNFDrhME9DZyTQfK3z5Fin7bqre81wG6f+ASCxci84VDUJBJ1MwAQr2rPxTuCSgPbkk8aSGQDfaMuLKNddSLiXSjKytBHNlWvm/AgwADYjWVI79nQQAAAAASUVORK5CYII="; }
        }

        //public static IList<CodeDao> SYSTEM_DEPT_VO()
        //{
        //    IList<SystemCodeVo> ItemDetailVo = codeClient.SelectDeptList(new SystemCodeVo() { DELT_FLG = "N", CHNL_CD = SystemProperties.USER_VO.CHNL_CD });
        //    IList<CodeDao> resultMap = new List<CodeDao>();
        //    int nCnt = ItemDetailVo.Count;
        //    SystemCodeVo tmpVo;
        //    resultMap.Clear();
        //    for (int x = 0; x < nCnt; x++)
        //    {
        //        tmpVo = ItemDetailVo[x];
        //        resultMap.Add(new CodeDao() { CLSS_CD = tmpVo.CLSS_CD, CLSS_DESC = tmpVo.CLSS_DESC });
        //    }
        //    return resultMap;
        //}

        //public static Dictionary<string, string> SYSTEM_DEPT_MAP()
        //{
        //    IList<SystemCodeVo> ItemDetailVo = codeClient.SelectDeptList(new SystemCodeVo() { DELT_FLG = "N", CHNL_CD = SystemProperties.USER_VO.CHNL_CD });
        //    Dictionary<string, string> resultMap = new Dictionary<string, string>();
        //    int nCnt = ItemDetailVo.Count;
        //    SystemCodeVo tmpVo;
        //    resultMap.Clear();
        //    for (int x = 0; x < nCnt; x++)
        //    {
        //        tmpVo = ItemDetailVo[x];
        //        if (!resultMap.ContainsKey(tmpVo.CLSS_DESC))
        //        {
        //            resultMap.Add(tmpVo.CLSS_DESC, tmpVo.CLSS_CD);
        //        }
        //    }
        //    return resultMap;
        //}

        //public static Dictionary<string, string> SYSTEM_CODE(string clssTpCd)
        //{
        //    IList<SystemCodeVo> ItemDetailVo = codeClient.SelectDetailCode(new SystemCodeVo() { DELT_FLG = "N", CLSS_TP_CD = clssTpCd, CHNL_CD = SystemProperties.USER_VO.CHNL_CD });
        //    Dictionary<string, string> resultMap = new Dictionary<string, string>();
        //    int nCnt = ItemDetailVo.Count;
        //    SystemCodeVo tmpVo;
        //    resultMap.Clear();
        //    for (int x = 0; x < nCnt; x++)
        //    {
        //        tmpVo = ItemDetailVo[x];
        //        if (!resultMap.ContainsKey(tmpVo.CLSS_CD))
        //        {
        //            resultMap.Add(tmpVo.CLSS_CD, tmpVo.CLSS_DESC);
        //        }
        //    }
        //    return resultMap;
        //}

        //public static Dictionary<string, string> SYSTEM_CODE_MAP(string clssTpCd)
        //{
        //    IList<SystemCodeVo> ItemDetailVo = codeClient.SelectDetailCode(new SystemCodeVo() { DELT_FLG = "N", CLSS_TP_CD = clssTpCd, CHNL_CD = SystemProperties.USER_VO.CHNL_CD });
        //    Dictionary<string, string> resultMap = new Dictionary<string, string>();
        //    int nCnt = ItemDetailVo.Count;
        //    SystemCodeVo tmpVo;
        //    resultMap.Clear();
        //    for (int x = 0; x < nCnt; x++)
        //    {
        //        tmpVo = ItemDetailVo[x];
        //        if (!resultMap.ContainsKey(tmpVo.CLSS_DESC))
        //        {
        //            resultMap.Add(tmpVo.CLSS_DESC, tmpVo.CLSS_CD);
        //        }
        //    }
        //    return resultMap;
        //}

        //public static IList<CodeDao> SYSTEM_CODE_VO(string clssTpCd)
        //{
        //    IList<SystemCodeVo> ItemDetailVo = codeClient.SelectDetailCode(new SystemCodeVo() { DELT_FLG = "N", CLSS_TP_CD = clssTpCd, CHNL_CD = SystemProperties.USER_VO.CHNL_CD });
        //    IList<CodeDao> resultMap = new List<CodeDao>();
        //    int nCnt = ItemDetailVo.Count;
        //    SystemCodeVo tmpVo;
        //    resultMap.Clear();
        //    for (int x = 0; x < nCnt; x++)
        //    {
        //        tmpVo = ItemDetailVo[x];
        //        resultMap.Add(new CodeDao() { CLSS_CD = tmpVo.CLSS_CD, CLSS_DESC = tmpVo.CLSS_DESC });
        //    }
        //    return resultMap;
        //}

        //public static IList<UserCodeDao> USER_CODE_VO()
        //{
        //    IList<GroupUserVo> ItemDetailVo = authClient.SelectUserList(new GroupUserVo() { DELT_FLG = "N", CHNL_CD = SystemProperties.USER_VO.CHNL_CD });
        //    IList<UserCodeDao> resultMap = new List<UserCodeDao>();
        //    int nCnt = ItemDetailVo.Count;
        //    GroupUserVo tmpVo;
        //    resultMap.Clear();
        //    for (int x = 0; x < nCnt; x++)
        //    {
        //        tmpVo = ItemDetailVo[x];
        //        resultMap.Add(new UserCodeDao() { USR_ID = tmpVo.USR_ID, USR_N1ST_NM = tmpVo.USR_N1ST_NM, EMPE_NO = tmpVo.EMPE_NO, OFC_PSN_NM = tmpVo.OFC_PSN_NM });
        //    }
        //    return resultMap;
        //}

        //public static IList<CustomerCodeDao> CUSTOMER_CODE_VO(string _SEEK)
        //{
        //    IList<SystemCodeVo> ItemDetailVo = codeClient.SelectCustomerCode(new SystemCodeVo() { DELT_FLG = "N", CHNL_CD = SystemProperties.USER_VO.CHNL_CD });
        //    IList<CustomerCodeDao> resultMap = new List<CustomerCodeDao>();
        //    int nCnt = ItemDetailVo.Count;
        //    SystemCodeVo tmpVo;
        //    resultMap.Clear();
        //    for (int x = 0; x < nCnt; x++)
        //    {
        //        tmpVo = ItemDetailVo[x];
        //        resultMap.Add(new CustomerCodeDao() { CO_NO = tmpVo.CO_NO, CO_NM = tmpVo.CO_NM, CO_RGST_NO = tmpVo.CO_RGST_NO, PRSD_NM = tmpVo.PRSD_NM });
        //    }
        //    return resultMap;
        //}

        //public static IList<CodeDao> EQ_VO()
        //{
        //    IList<SystemCodeVo> ItemDetailVo = codeClient.SelectEqList(new SystemCodeVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD });
        //    IList<CodeDao> resultMap = new List<CodeDao>();
        //    int nCnt = ItemDetailVo.Count;
        //    SystemCodeVo tmpVo;
        //    resultMap.Clear();
        //    for (int x = 0; x < nCnt; x++)
        //    {
        //        tmpVo = ItemDetailVo[x];
        //        resultMap.Add(new CodeDao() { CLSS_CD = tmpVo.CLSS_CD, CLSS_DESC = tmpVo.CLSS_DESC });
        //    }
        //    return resultMap;
        //}


        //public static IList<ItemCodeDao> ITEMS_CODE_VO(string _ITM_GRP_CLSS_CD)
        //{
        //    IList<SystemCodeVo> ItemDetailVo = codeClient.SelectItemList(new SystemCodeVo() { DELT_FLG = "N", CHNL_CD = SystemProperties.USER_VO.CHNL_CD, ITM_GRP_CLSS_CD = _ITM_GRP_CLSS_CD });
        //    IList<ItemCodeDao> resultMap = new List<ItemCodeDao>();
        //    int nCnt = ItemDetailVo.Count;
        //    SystemCodeVo tmpVo;
        //    resultMap.Clear();
        //    for (int x = 0; x < nCnt; x++)
        //    {
        //        tmpVo = ItemDetailVo[x];
        //        resultMap.Add(new ItemCodeDao() { ITM_CD = tmpVo.ITM_CD, ITM_NM = tmpVo.ITM_NM, CAR_ITM_CD = tmpVo.CAR_ITM_CD, CAR_ITM_NM = tmpVo.CAR_ITM_NM, N1ST_ITM_GRP_CD = tmpVo.N1ST_ITM_GRP_CD, ITM_SZ_NM = tmpVo.ITM_SZ_NM, UOM_NM = tmpVo.UOM_NM, UOM_CD = tmpVo.UOM_CD, ITM_ORD_CD = tmpVo.ITM_ORD_CD, ITM_ORD_NM = tmpVo.ITM_ORD_NM });
        //    }
        //    return resultMap;
        //}


        //public static IList<PckCodeDao> PCK_CODE_VO()
        //{
        //    IList<ManVo> ItemDetailVo = manClient.M6625SelectMaster(new ManVo() { CHNL_CD = SystemProperties.USER_VO.CHNL_CD });
        //    IList<PckCodeDao> resultMap = new List<PckCodeDao>();
        //    int nCnt = ItemDetailVo.Count;
        //    ManVo tmpVo;
        //    resultMap.Clear();
        //    for (int x = 0; x < nCnt; x++)
        //    {
        //        tmpVo = ItemDetailVo[x];
        //        resultMap.Add(new PckCodeDao() { PCK_PLST_CLSS_CD = tmpVo.PCK_PLST_CLSS_CD, PCK_PLST_CLSS_NM = tmpVo.PCK_PLST_CLSS_NM, PCK_PLST_CD = tmpVo.PCK_PLST_CD, PCK_PLST_NM = tmpVo.PCK_PLST_NM, PCK_PLST_VAL = tmpVo.PCK_PLST_VAL });
        //    }
        //    return resultMap;
        //}


        //public static void ReportImgDownload()
        //{
        //    FileInfo tmpFile = new FileInfo(@"c:\MooJin_logo.png");
        //    WebClient webClient = new WebClient();
        //    if (!tmpFile.Exists)
        //    {
        //        webClient.DownloadFile("http://dyerp.cafe24.com/Moojin/MooJin_logo.png", @"c:\MooJin_logo.png");
        //    }
        //}

        //public static void MENU_IMG_SET(IList<ProgramVo> resultMenuList)
        //{
        //    Image image = new Image();

        //    ProgramVo menuVo;
        //    foreach (var item in resultMenuList)
        //    {
        //        menuVo = (ProgramVo)item;
        //        if (menuVo.PGM_CD.Equals("M"))
        //        {
        //            menuVo.UPD_FLG = null;
        //            menuVo.VIS_FLG = null;
        //            menuVo.IMAGE = Convert.FromBase64String(MAIN_IMG);
        //        }
        //        else if (menuVo.PGM_CD.Equals("G"))
        //        {
        //            menuVo.IMAGE = Convert.FromBase64String(MAIN_SUB_IMG);
        //        }
        //        else
        //        {
        //            //Menu IMAGE
        //            //      => 시스템 코드
        //            if (menuVo.MDL_ID.Equals("1.3.1"))
        //            {
        //                menuVo.IMAGE = Convert.FromBase64String(IMG_1_3_1);
        //            }
        //            //else if (menuVo.MDL_ID.Equals("1.3.2"))
        //            //{
        //            //    menuVo.IMAGE = Convert.FromBase64String(IMG_1_3_2);
        //            //}
        //            //
        //            else if (menuVo.MDL_ID.Equals("1.3.2"))
        //            {
        //                menuVo.IMAGE = Convert.FromBase64String(IMG_1_4_3);
        //            }
        //            else if (menuVo.MDL_ID.Equals("1.3.3"))
        //            {
        //                menuVo.IMAGE = Convert.FromBase64String(IMG_1_4_2);
        //            }
        //            //else if (menuVo.MDL_ID.Equals("1.3.4"))
        //            //{
        //            //    menuVo.IMAGE = Convert.FromBase64String(IMG_1_3_4);
        //            //}
        //            //else if (menuVo.MDL_ID.Equals("1.3.5"))
        //            //{
        //            //    menuVo.IMAGE = Convert.FromBase64String(IMG_1_3_5);
        //            //}
        //            else if (menuVo.MDL_ID.Equals("1.3.10"))
        //            {
        //                menuVo.IMAGE = Convert.FromBase64String(IMG_1_3_6);
        //            }
        //            else if (menuVo.MDL_ID.Equals("1.3.6"))
        //            {
        //                menuVo.IMAGE = Convert.FromBase64String(IMG_1_3_6);
        //            }
        //            //else if (menuVo.MDL_ID.Equals("1.3.7"))
        //            //{
        //            //    menuVo.IMAGE = Convert.FromBase64String(IMG_1_3_7);
        //            //}
        //            //else if (menuVo.MDL_ID.Equals("1.3.8"))
        //            //{
        //            //    menuVo.IMAGE = Convert.FromBase64String(IMG_1_3_8);
        //            //}
        //            else if (menuVo.MDL_ID.Equals("1.3.9"))
        //            {
        //                menuVo.IMAGE = Convert.FromBase64String(IMG_1_3_9);
        //            }


        //            else if (menuVo.MDL_ID.Equals("1.4.1"))
        //            {
        //                menuVo.IMAGE = Convert.FromBase64String(IMG_1_4_1);
        //            }
        //            else if (menuVo.MDL_ID.Equals("1.4.2"))
        //            {
        //                menuVo.IMAGE = Convert.FromBase64String(IMG_1_4_2);
        //            }
        //            else if (menuVo.MDL_ID.Equals("1.4.3"))
        //            {
        //                menuVo.IMAGE = Convert.FromBase64String(IMG_1_4_3);
        //            }


        //            else if (menuVo.MDL_ID.Equals("6.1.1"))
        //            {
        //                menuVo.IMAGE = Convert.FromBase64String(IMG_6_1_1);
        //            }
        //            else if (menuVo.MDL_ID.Equals("6.2.2"))
        //            {
        //                menuVo.IMAGE = Convert.FromBase64String(IMG_6_2_2);
        //            }
        //            else if (menuVo.MDL_ID.Equals("6.2.3"))
        //            {
        //                menuVo.IMAGE = Convert.FromBase64String(IMG_6_2_3);
        //            }

        //            //else if (menuVo.MDL_ID.Equals("2.1"))
        //            //{
        //            //    menuVo.IMAGE = Convert.FromBase64String(IMG_2_1);
        //            //}
        //            //else if (menuVo.MDL_ID.Equals("2.2"))
        //            //{
        //            //    menuVo.IMAGE = Convert.FromBase64String(IMG_2_2);
        //            //}

        //            //else if (menuVo.MDL_ID.Equals("3.1"))
        //            //{
        //            //    menuVo.IMAGE = Convert.FromBase64String(IMG_3_1);
        //            //}
        //            //else if (menuVo.MDL_ID.Equals("3.2"))
        //            //{
        //            //    menuVo.IMAGE = Convert.FromBase64String(IMG_3_2);
        //            //}

        //            //else if (menuVo.MDL_ID.Equals("5.1"))
        //            //{
        //            //    menuVo.IMAGE = Convert.FromBase64String(IMG_5_1);
        //            //}
        //            //else if (menuVo.MDL_ID.Equals("5.2"))
        //            //{
        //            //    menuVo.IMAGE = Convert.FromBase64String(IMG_5_2);
        //            //}
        //            //else if (menuVo.MDL_ID.Equals("5.3"))
        //            //{
        //            //    menuVo.IMAGE = Convert.FromBase64String(IMG_5_3);
        //            //}


        //            //else if (menuVo.MDL_ID.Equals("6.1"))
        //            //{
        //            //    menuVo.IMAGE = Convert.FromBase64String(IMG_6_1);
        //            //}
        //            //else if (menuVo.MDL_ID.Equals("6.2"))
        //            //{
        //            //    menuVo.IMAGE = Convert.FromBase64String(IMG_6_2);
        //            //}


        //            //else if (menuVo.MDL_ID.Equals("4.1"))
        //            //{
        //            //    menuVo.IMAGE = Convert.FromBase64String(IMG_4_1);
        //            //}
        //            //else if (menuVo.MDL_ID.Equals("4.2"))
        //            //{
        //            //    menuVo.IMAGE = Convert.FromBase64String(IMG_4_2);
        //            //}
        //        }
        //    }
        //}


        public static string ProgramVersion
        {
            get
            {
                try
                {
                    return System.Deployment.Application.ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString();
                }
                catch (System.Deployment.Application.DeploymentException)
                {
                    return Assembly.GetExecutingAssembly().GetName().Version.ToString();
                }
                catch (Exception)
                {
                    return "Unable to verify";
                }
            }
        }
    }


    //public class PageInfoConverter : IMultiValueConverter
    //{
    //    public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    //    {
    //        return String.Format("PAGE: {0} OF {1}", values[0], values[1]);
    //    }
    //    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

}
