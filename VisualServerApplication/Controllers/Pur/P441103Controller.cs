using ModelsLibrary.Code;
using ModelsLibrary.Pur;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using VisualServerApplication.Config;

namespace VisualServerApplication.Controllers.Pur
{
    [RoutePrefix("p441103")]
    public class P441103Controller : ApiController
    {
        #region MyRegion
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
        /// 부자재 발주 관리 MST - 조회
        /// </summary>
        [Route("mst")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetMstSelect([FromBody]PurVo vo)
        {
            try
            {
                return Ok<IEnumerable<PurVo>>(Properties.EntityMapper.QueryForList<PurVo>("P441103SelectMstList", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }


        /// <summary>
        ///  부자재 발주 관리 MST - 추가
        /// </summary>
        [Route("mst/i")]
        [HttpPost]
        // POST api/<controller>
        public async Task<IHttpActionResult> GetMstInsert([FromBody]PurVo vo)
        {
            try
            {
                vo.PUR_ORD_NO = Properties.EntityMapper.QueryForObject<string>("P441103SelectPurOrdNo", vo);
                Properties.EntityMapper.Update("P441103UpdatePurOrdNo", vo);
                return Ok<int>(Properties.EntityMapper.Insert("P441103InsertMst", vo) == null ? 1 : 0);
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 부자재 발주 관리 MST - 수정
        /// </summary>
        [Route("mst/u")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetMstUpdate([FromBody]PurVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Update("P441103UpdateMst", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }


        /// <summary>
        /// 부자재 발주 관리 MST - 삭제
        /// </summary>
        [Route("mst/d")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetMstDelete([FromBody]PurVo vo)
        {
            try
            {
                Properties.EntityMapper.Delete("P441103DeleteDtl", vo);
                return Ok<int>(Properties.EntityMapper.Delete("P441103DeleteMst", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }



        /// <summary>
        /// 부자재 발주 관리 DTL - 조회
        /// </summary>
        [Route("dtl")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetDtlSelect([FromBody]PurVo vo)
        {
            return Ok<IEnumerable<PurVo>>(Properties.EntityMapper.QueryForList<PurVo>("P441103SelectDtlList", vo));
        }

        /// <summary>
        /// 부자재 발주 관리 DTL (바코드) - 조회 + 생성
        /// </summary>
        [Route("dtl/barcode")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetDtlBarcodeSelect([FromBody] PurVo vo)
        {
            try
            {
                IList<PurVo> _barCodeList = Properties.EntityMapper.QueryForList<PurVo>("P441103SelectDtlBarCodeList", vo);

                Properties.EntityMapper.BeginTransaction();
                //
                // - 바코드 저장
                foreach (PurVo item in _barCodeList)
                {
                    Properties.EntityMapper.Insert("P441103InsertBarCodeDtl", item);
                }

                Properties.EntityMapper.CommitTransaction();
                return Ok<IEnumerable<PurVo>>(_barCodeList);
            }
            catch (System.Exception eLog)
            {
                Properties.EntityMapper.RollBackTransaction();
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 부자재 발주 관리 DTL (바코드) - 조회
        /// </summary>
        [Route("dtl/barcode/report")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetDtlBarcodeReportSelect([FromBody] PurVo vo)
        {
            try
            {
                //IList<PurVo> _barCodeList = Properties.EntityMapper.QueryForList<PurVo>("P441103SelectDtlBarCodeReportList", vo);

                //Properties.EntityMapper.BeginTransaction();
                ////
                //// - 바코드 저장
                //foreach (PurVo item in _barCodeList)
                //{
                //    Properties.EntityMapper.Insert("P441102InsertBarCodeDtl", item);
                //}

                //Properties.EntityMapper.CommitTransaction();
                return Ok<IEnumerable<PurVo>>(Properties.EntityMapper.QueryForList<PurVo>("P441103SelectDtlBarCodeReportList", vo));
            }
            catch (System.Exception eLog)
            {
                //Properties.EntityMapper.RollBackTransaction();
                return Ok<string>(eLog.Message);
            }
        }


        /// <summary>
        /// 부자재 발주 관리 DTL - 추가
        /// </summary>
        [Route("dtl/i")]
        [HttpPost]
        // POST api/<controller>
        public async Task<IHttpActionResult> GetDtlInsert([FromBody]IList<PurVo> voList)
        {
            try
            {
                //return Ok<int>(Properties.EntityMapper.Insert("P4411InsertDtl", vo) == null ? 1 : 0);

                Properties.EntityMapper.BeginTransaction();

                //if (voList.Count > 0)
                //{
                //    Properties.EntityMapper.Delete("S2211DeleteDtl", new SaleVo() { CHNL_CD = voList[0].CHNL_CD, SL_RLSE_NO = voList[0].SL_RLSE_NO });
                    foreach (PurVo item in voList)
                    {
                        Properties.EntityMapper.Insert("P441103InsertDtl", item);
                    }

                //    Properties.EntityMapper.QueryForObject<int?>("ProcS2211", voList[0]);
                //}

                Properties.EntityMapper.CommitTransaction();
                return Ok<int>(1);
            }
            catch (System.Exception eLog)
            {
                Properties.EntityMapper.RollBackTransaction();
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 부자재 발주 관리 DTL - 수정
        /// </summary>
        [Route("dtl/u")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetDtlUpdate([FromBody]PurVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Update("P441103UpdateDtl", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }


        /// <summary>
        /// 부자재 발주 관리 DTL - 삭제
        /// </summary>
        [Route("dtl/d")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetDtlDelete([FromBody]PurVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Delete("P441103DeleteDtl", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }


        ///// <summary>
        ///// 원재료 발주 관리 MST - 리포트
        ///// </summary>
        //[Route("report")]
        //[HttpPost]
        //// GET api/<controller>
        //public async Task<IHttpActionResult> GetReportSelect([FromBody]PurVo vo)
        //{
        //    return Ok<IEnumerable<PurVo>>(Properties.EntityMapper.QueryForList<PurVo>("P4411SelectReport", vo));
        //}


        ///// <summary>
        /////원재료 발주 관리 MST - 리포트
        ///// </summary>
        //[Route("report/weekly")]
        //[HttpPost]
        //// GET api/<controller>
        //public async Task<IHttpActionResult> GetReportWeeklySelect([FromBody]PurVo vo)
        //{
        //    return Ok<IEnumerable<PurVo>>(Properties.EntityMapper.QueryForList<PurVo>("P4411SelectWeeklyReport", vo));
        //}


        /// <summary>
        ///부자재 발주 관리(차종) POPUP - 조회
        /// </summary>
        [Route("popup")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetPopup1Select([FromBody]PurVo vo)
        {
            return Ok<IEnumerable<PurVo>>(Properties.EntityMapper.QueryForList<PurVo>("P441103SelectPopUp", vo));
        }

        [Route("popup/itm")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetPopup2Select([FromBody] SystemCodeVo vo)
        {
            return Ok<IEnumerable<PurVo>>(Properties.EntityMapper.QueryForList<PurVo>("P441103SelectItmPopup", vo));
        }


    }
}