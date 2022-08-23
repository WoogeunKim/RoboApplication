using ModelsLibrary.Pda;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using VisualServerApplication.Config;

namespace VisualServerApplication.Controllers.Pda
{
    /*
     * P01 : 원자재발주입고   : 원자재 발주 입고 등록
     * P0 : 생산LOT이동전표  : 재공 LOT별 분할/병합전표 발행 관리
     * P02 : 제품출하         : 제품 출하 관리
     * P0 : 금형입고         : 투입 금형 반환 입고
     * P0 : 금형출고         : 설비 금형 장착
     */
    [RoutePrefix("pda")]
    public class PdaController : ApiController
    {
        #region 주석
        //// GET api/<controller>
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/<controller>/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/<controller>
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/<controller>/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/<controller>/5
        //public void Delete(int id)
        //{
        //} 
        #endregion

        /// <summary>
        /// 로그인 - 조회
        /// </summary>
        [Route("usr")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetUsrSelect([FromBody]PdaVo vo)
        {
            return Ok<PdaVo>(Properties.EntityMapper.QueryForObject<PdaVo>("SelectUser", vo));
        }

        /// <summary>
        /// 거래처 - 조회
        /// </summary>
        [Route("co")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetCoNmSelect([FromBody]PdaVo vo)
        {
            return Ok<IEnumerable<PdaVo>>(Properties.EntityMapper.QueryForList<PdaVo>("SelectCoNm", vo));
        }

        /// <summary>
        /// 설비 - 조회
        /// </summary>
        [Route("eq")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetEqNmSelect([FromBody]PdaVo vo)
        {
            return Ok<IEnumerable<PdaVo>>(Properties.EntityMapper.QueryForList<PdaVo>("SelectEqNm", vo));
        }

        /// <summary>
        /// 시스템 코드 - 조회
        /// </summary>
        [Route("sys")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetSysCdSelect([FromBody] PdaVo vo)
        {
            return Ok<IEnumerable<PdaVo>>(Properties.EntityMapper.QueryForList<PdaVo>("SelectSysCd", vo));
        }


        #region 원자재발주입고
        /// <summary>
        /// 원자재 발주 입고 MST - 조회
        /// </summary>
        [Route("p01/mst")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetP01MstSelect([FromBody]PdaVo vo)
        {
            return Ok<IEnumerable<PdaVo>>(Properties.EntityMapper.QueryForList<PdaVo>("P01SelectMstList", vo));
        }

        /// <summary>
        /// 원자재 발주 입고 DTL - 조회
        /// </summary>
        [Route("p01/dtl")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetP01DtlSelect([FromBody]PdaVo vo)
        {
            return Ok<IEnumerable<PdaVo>>(Properties.EntityMapper.QueryForList<PdaVo>("P01SelectDtlList", vo));
        }

        /// <summary>
        /// 원자재 발주 입고 DTL - 추가
        /// </summary>
        [Route("p01/dtl/i")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetP01Insert([FromBody]PdaVo vo)
        {
            try
            {
                //if(Properties.EntityMapper.QueryForList<PdaVo>("P01SelectDtlList", vo).Count > 0)
                //{
                    //return Ok<string>("[(원)LOT-NO :" + vo.PRNT_LOT_NO + " ]  중복 입니다");
                //}
                //else
                //{
                    return Ok<int>(Properties.EntityMapper.Insert("P01InsertDtl", vo) == null ? 1 : 0);
                //}
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 원자재 발주 입고 DTL - 수정
        /// </summary>
        [Route("p01/dtl/u")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetP01Update([FromBody]PdaVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Update("P01UpdateDtl", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 원자재 발주 입고 DTL - 삭제
        /// </summary>
        [Route("p01/dtl/d")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetP01Delete([FromBody]PdaVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Delete("P01DeleteDtl", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }
        #endregion

        #region SCM발주입고
        /// <summary>
        /// SCM 발주 입고 MST - 조회
        /// </summary>
        [Route("p11/mst")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetP11MstSelect([FromBody] PdaVo vo)
        {
            return Ok<IEnumerable<PdaVo>>(Properties.EntityMapper.QueryForList<PdaVo>("P11SelectMstList", vo));
        }

        /// <summary>
        /// SCM 발주 입고 DTL - 조회
        /// </summary>
        [Route("p11/dtl")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetP11DtlSelect([FromBody] PdaVo vo)
        {
            return Ok<IEnumerable<PdaVo>>(Properties.EntityMapper.QueryForList<PdaVo>("P11SelectDtlList", vo));
        }

        /// <summary>
        /// SCM 발주 입고 DTL - 추가
        /// </summary>
        [Route("p11/dtl/i")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetP11Insert([FromBody] PdaVo vo)
        {
            try
            {
                //if(Properties.EntityMapper.QueryForList<PdaVo>("P01SelectDtlList", vo).Count > 0)
                //{
                //return Ok<string>("[(원)LOT-NO :" + vo.PRNT_LOT_NO + " ]  중복 입니다");
                //}
                //else
                //{
                return Ok<int>(Properties.EntityMapper.Insert("P11InsertDtl", vo) == null ? 1 : 0);
                //}
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// SCM 발주 입고 DTL - 수정
        /// </summary>
        [Route("p11/dtl/u")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetP11Update([FromBody] PdaVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Update("P11UpdateDtl", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// SCM 발주 입고 DTL - 삭제
        /// </summary>
        [Route("p11/dtl/d")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetP11Delete([FromBody] PdaVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Delete("P11DeleteDtl", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }
        #endregion

        #region 정입고
        /// <summary>
        ///  입고 MST - 조회
        /// </summary>
        [Route("p10/mst")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetP10MstSelect([FromBody] PdaVo vo)
        {
            return Ok<IEnumerable<PdaVo>>(Properties.EntityMapper.QueryForList<PdaVo>("P10SelectMstList", vo));
        }


        /// <summary>
        /// 입고 MST - 추가
        /// </summary>
        [Route("p10/mst/i")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetP10Insert([FromBody] PdaVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Insert("P10InsertMst", vo) == null ? 1 : 0);
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }


        /// <summary>
        /// 입고 MST - 수정
        /// </summary>
        [Route("p10/mst/u")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetP10Update([FromBody] PdaVo vo)
        {
            try
            {
                //if(Properties.EntityMapper.QueryForList<PdaVo>("P01SelectDtlList", vo).Count > 0)
                //{
                //return Ok<string>("[(원)LOT-NO :" + vo.PRNT_LOT_NO + " ]  중복 입니다");
                //}
                //else
                //{
                Properties.EntityMapper.Update("P10UpdateMst", vo) ;
                return Ok<int>(Properties.EntityMapper.Update("P10UpdateDtl", vo) == null ? 1 : 0);
                //}
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }


        #endregion





        #region 제품출하
        /// <summary>
        /// 제품출하 MST - 조회
        /// </summary>
        [Route("p02/mst")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetP02MstSelect([FromBody]PdaVo vo)
        {
            return Ok<IEnumerable<PdaVo>>(Properties.EntityMapper.QueryForList<PdaVo>("P02SelectMstList", vo));
        }

        /// <summary>
        /// 제품출하 DTL - 조회
        /// </summary>
        [Route("p02/dtl")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetP02DtlSelect([FromBody]PdaVo vo)
        {
            return Ok<IEnumerable<PdaVo>>(Properties.EntityMapper.QueryForList<PdaVo>("P02SelectDtlList", vo));
        }

        /// <summary>
        /// 제품출하 DTL - 추가
        /// </summary>
        [Route("p02/dtl/i")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetP02Insert([FromBody]PdaVo vo)
        {
            try
            {
                if (Properties.EntityMapper.QueryForList<PdaVo>("P02SelectDtlList", vo).Count > 0)
                {
                    return Ok<string>("[생산LOT-NO :" + vo.PROD_LOT_NO + "/납품처LOT-NO : " + vo.RLSE_LOT_NO + " ]  중복 입니다");
                }
                else
                {
                    return Ok<int>(Properties.EntityMapper.Insert("P02InsertDtl", vo) == null ? 1 : 0);
                }
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 제품출하 DTL - 수정
        /// </summary>
        [Route("p02/dtl/u")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetP02Update([FromBody]PdaVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Update("P02UpdateDtl", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 제품출하 DTL - 삭제
        /// </summary>
        [Route("p02/dtl/d")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetP02Delete([FromBody]PdaVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Delete("P02DeleteDtl", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }
        #endregion


        #region 원자재 LOT 투입
        /// <summary>
        /// 원자재 LOT 투입 - 프로시저 
        /// </summary>
        [Route("p03/proc")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetP03Proc([FromBody]PdaVo vo)
        {
            try
            {
                 int? nResult = Properties.EntityMapper.QueryForObject<int?>("ProcP03", vo);

                if (nResult == 1)
                {
                    //성공
                    return Ok<int>(1);
                }
                else if (nResult == 0)
                {
                    //실패
                    return Ok<string>("조회되지 않는 바코드 번호 입니다");
                }
                else if (nResult == 10)
                {
                    //실패
                    return Ok<string>("One Label LOT-NO 중복 등록 되었습니다");
                }
                else if (nResult == 20)
                {
                    //실패
                    return Ok<string>("해당 생산 LOT-NO에 투입 원자재가 아닙니다");
                }
                else
                {
                    //실패
                    return Ok<string>("관리자 문의 부탁 드립니다");
                }
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 원자재 LOT 투입 MST - 조회
        /// </summary>
        [Route("p03/mst")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetP03MstSelect([FromBody]PdaVo vo)
        {
            return Ok<IEnumerable<PdaVo>>(Properties.EntityMapper.QueryForList<PdaVo>("P03SelectMstList", vo));
        }

        /// <summary>
        /// 원자재 LOT 투입 DTL - 조회
        /// </summary>
        [Route("p03/dtl")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetP03DtlSelect([FromBody]PdaVo vo)
        {
            return Ok<IEnumerable<PdaVo>>(Properties.EntityMapper.QueryForList<PdaVo>("P03SelectDtlList", vo));
        }

        /// <summary>
        /// 원자재 LOT 투입 DTL - 추가
        /// </summary>
        [Route("p03/dtl/i")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetP03Insert([FromBody]PdaVo vo)
        {
            try
            {
                if (Properties.EntityMapper.QueryForList<PdaVo>("P03SelectDtlList", vo).Count > 0)
                {
                    return Ok<string>("[생산LOT-NO :" + vo.N2ND_LOT_NO + "/원재료 투입LOT-NO : " + vo.N1ST_LOT_NO + " ]  중복 입니다");
                }
                else
                {
                    return Ok<int>(Properties.EntityMapper.Insert("P03InsertDtl", vo) == null ? 1 : 0);
                }
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        ///// <summary>
        ///// 원자재 LOT 투입 DTL - 수정
        ///// </summary>
        //[Route("p02/dtl/u")]
        //[HttpPost]
        //// PUT api/<controller>/5
        //public async Task<IHttpActionResult> GetP02Update([FromBody]PdaVo vo)
        //{
        //    try
        //    {
        //        return Ok<int>(Properties.EntityMapper.Update("P02UpdateDtl", vo));
        //    }
        //    catch (System.Exception eLog)
        //    {
        //        return Ok<string>(eLog.Message);
        //    }
        //}

        /// <summary>
        /// 원자재 LOT 투입 DTL - 삭제
        /// </summary>
        [Route("p03/dtl/d")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetP03Delete([FromBody]PdaVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Delete("P03DeleteDtl", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        #endregion


        #region 금형생산 입/출고
        /// <summary>
        /// 금형생산 입/출고 MST -  조회
        /// </summary>
        [Route("p04/mst")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetP04MstSelect([FromBody]PdaVo vo)
        {
            return Ok<IEnumerable<PdaVo>>(Properties.EntityMapper.QueryForList<PdaVo>("P04SelectMstList", vo));
        }

        /// <summary>
        /// 금형생산 입/출고 DTL - 수불 조회
        /// </summary>
        [Route("p04/dtl")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetP04DtlSelect([FromBody]PdaVo vo)
        {
            return Ok<IEnumerable<PdaVo>>(Properties.EntityMapper.QueryForList<PdaVo>("P04SelectDtlList", vo));
        }

        /// <summary>
        /// 금형생산 입/출고 DTL - 추가
        /// </summary>
        [Route("p04/dtl/i")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetP04Insert([FromBody]PdaVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Insert("P04InsertDtl", vo) == null ? 1 : 0);
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 금형생산 입/출고 DTL - 수정
        /// </summary>
        [Route("p04/dtl/u")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetP04Update([FromBody]PdaVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Update("P04UpdateDtl", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }
        #endregion



        #region 외주 출고
        /// <summary>
        /// 외주 출고 MST -  조회
        /// </summary>
        [Route("p40/mst")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> Get40MstSelect([FromBody] PdaVo vo)
        {
            return Ok<IEnumerable<PdaVo>>(Properties.EntityMapper.QueryForList<PdaVo>("P40SelectMstList", vo));
        }
        #endregion



        #region 외주 입고
        /// <summary>
        /// 외주 입고 MST -  조회
        /// </summary>
        [Route("p50/mst")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> Get50MstSelect([FromBody] PdaVo vo)
        {
            return Ok<IEnumerable<PdaVo>>(Properties.EntityMapper.QueryForList<PdaVo>("P50SelectMstList", vo));
        }

        /// <summary>
        /// 외주 입고 MST - 추가
        /// </summary>
        [Route("p50/mst/m")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> Get50Insert([FromBody] IEnumerable<PdaVo> voList)
        {
            try
            {
                Properties.EntityMapper.BeginTransaction();
                foreach (PdaVo item in voList)
                {
                    item.LST_FMT_NO = Properties.EntityMapper.QueryForObject<string>("SelectTmpInsrlNo", item);
                    Properties.EntityMapper.Update("UpdateTmpInsrlNo", item);

                    Properties.EntityMapper.Insert("P50InsertMst", item);
                }
                Properties.EntityMapper.CommitTransaction();
                return Ok<int?>(1);
            //return Ok<int>(Properties.EntityMapper.Insert("P50InsertMst", vo) == null ? 1 : 0);
            }
            catch (System.Exception eLog)
            {
                Properties.EntityMapper.RollBackTransaction();
                return Ok<string>(eLog.Message);
            }
        }


        #endregion


        #region 외주 출고
        /// <summary>
        /// 외주 출고 MST -  조회
        /// </summary>
        [Route("p51/mst")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> P51SelectMstList([FromBody] PdaVo vo)
        {
            return Ok<IEnumerable<PdaVo>>(Properties.EntityMapper.QueryForList<PdaVo>("P51SelectMstList", vo));
        }

        /// <summary>
        /// 외주 입고 MST - 추가
        /// </summary>
        [Route("p51/mst/m")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> Get51Insert([FromBody] IEnumerable<PdaVo> voList)
        {
            try
            {
                Properties.EntityMapper.BeginTransaction();
                foreach (PdaVo item in voList)
                {
                    Properties.EntityMapper.Insert("P51InsertMst", item);
                }
                Properties.EntityMapper.CommitTransaction();
                return Ok<int?>(1);
                //return Ok<int>(Properties.EntityMapper.Insert("P50InsertMst", vo) == null ? 1 : 0);
            }
            catch (System.Exception eLog)
            {
                Properties.EntityMapper.RollBackTransaction();
                return Ok<string>(eLog.Message);
            }
        }



        #endregion



        //#region 공지 사항 / 인원현황  / 월-경영정보 / 일-경영정보
        ///// <summary>
        ///// 공지사항 MST -  조회
        ///// </summary>
        //[Route("p00/mst")]
        //[HttpPost]
        //// GET api/<controller>
        //public async Task<IHttpActionResult> GetP00MstSelect([FromBody]PdaVo vo)
        //{
        //    try
        //    {
        //        return Ok<IEnumerable<PdaVo>>(Properties.EntityMapper.QueryForList<PdaVo>("P00SelectMstList", vo));
        //    }
        //    catch (System.Exception eLog)
        //    {
        //        return Ok<string>(eLog.Message);
        //    }
        //}


        ///// <summary>
        ///// 인원현황 MST -  조회
        ///// </summary>
        //[Route("p10/mst")]
        //[HttpPost]
        //// GET api/<controller>
        //public async Task<IHttpActionResult> GetP10MstSelect([FromBody]PdaVo vo)
        //{
        //    try
        //    {
        //        return Ok<IEnumerable<PdaVo>>(Properties.EntityMapper.QueryForList<PdaVo>("P10SelectMstList", vo));
        //    }
        //    catch (System.Exception eLog)
        //    {
        //        return Ok<string>(eLog.Message);
        //    }
        //}

        ///// <summary>
        ///// 인원현황 DTL -  조회
        ///// </summary>
        //[Route("p10/dtl")]
        //[HttpPost]
        //// GET api/<controller>
        //public async Task<IHttpActionResult> GetP10DtlSelect([FromBody]PdaVo vo)
        //{
        //    try
        //    {
        //        return Ok<IEnumerable<PdaVo>>(Properties.EntityMapper.QueryForList<PdaVo>("P10SelectDtlList", vo));
        //    }
        //    catch (System.Exception eLog)
        //    {
        //        return Ok<string>(eLog.Message);
        //    }
        //}


        ///// <summary>
        ///// 월-경영정보 -  조회
        ///// </summary>
        //[Route("p11/mst")]
        //[HttpPost]
        //// GET api/<controller>
        //public async Task<IHttpActionResult> GetP11MstSelect([FromBody]PdaVo vo)
        //{
        //    try
        //    {
        //        return Ok<IEnumerable<PdaVo>>(Properties.EntityMapper.QueryForList<PdaVo>("P11SelectMstList", vo));
        //    }
        //    catch (System.Exception eLog)
        //    {
        //        return Ok<string>(eLog.Message);
        //    }
        //}

        ///// <summary>
        ///// 월-경영정보 -  조회
        ///// </summary>
        //[Route("p12/mst")]
        //[HttpPost]
        //// GET api/<controller>
        //public async Task<IHttpActionResult> GetP12MstSelect([FromBody]PdaVo vo)
        //{
        //    try
        //    {
        //        return Ok<IEnumerable<PdaVo>>(Properties.EntityMapper.QueryForList<PdaVo>("P12SelectMstList", vo));
        //    }
        //    catch (System.Exception eLog)
        //    {
        //        return Ok<string>(eLog.Message);
        //    }
        //}
        //#endregion

    }
}