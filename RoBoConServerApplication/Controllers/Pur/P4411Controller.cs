using ModelsLibrary.Pur;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using VisualServerApplication.Config;

namespace VisualServerApplication.Controllers.Pur
{
    [RoutePrefix("p4411")]
    public class P4411Controller : ApiController
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
        /// 발주 관리 MST - 조회
        /// </summary>
        [Route("mst")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetMstSelect([FromBody]PurVo vo)
        {
            try
            {
                return Ok<IEnumerable<PurVo>>(Properties.EntityMapper.QueryForList<PurVo>("P4411SelectMstList", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }


        /// <summary>
        /// 발주 관리 MST - 추가
        /// </summary>
        [Route("mst/i")]
        [HttpPost]
        // POST api/<controller>
        public async Task<IHttpActionResult> GetMstInsert([FromBody]PurVo vo)
        {
            try
            {
                vo.PUR_ORD_NO = Properties.EntityMapper.QueryForObject<string>("P4411SelectPurOrdNo", vo);
                Properties.EntityMapper.Update("P4411UpdatePurOrdNo", vo);
                return Ok<int>(Properties.EntityMapper.Insert("P4411InsertMst", vo) == null ? 1 : 0);
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 발주 관리 MST - 수정
        /// </summary>
        [Route("mst/u")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetMstUpdate([FromBody]PurVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Update("P4411UpdateMst", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }


        /// <summary>
        /// 발주 관리 MST - 삭제
        /// </summary>
        [Route("mst/d")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetMstDelete([FromBody]PurVo vo)
        {
            try
            {
                Properties.EntityMapper.BeginTransaction();
                Properties.EntityMapper.Delete("P4411DeleteDtl", vo);
                Properties.EntityMapper.Delete("P4411DeleteMst", vo);
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
        /// 발주 관리 DTL - 조회
        /// </summary>
        [Route("dtl")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetDtlSelect([FromBody]PurVo vo)
        {
            return Ok<IEnumerable<PurVo>>(Properties.EntityMapper.QueryForList<PurVo>("P4411SelectDtlList", vo));
        }

        /// <summary>
        /// 발주 관리 DTL - 추가
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
                        Properties.EntityMapper.Insert("P4411InsertDtl", item);
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
        /// 발주 관리 DTL - 수정
        /// </summary>
        [Route("dtl/u")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetDtlUpdate([FromBody]PurVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Update("P4411UpdateDtl", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }


        /// <summary>
        /// 발주 관리 DTL - 삭제
        /// </summary>
        [Route("dtl/d")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetDtlDelete([FromBody]PurVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Delete("P4411DeleteDtl", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }


        /// <summary>
        /// 발주 관리 MST - 리포트
        /// </summary>
        [Route("report")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetReportSelect([FromBody]PurVo vo)
        {
            return Ok<IEnumerable<PurVo>>(Properties.EntityMapper.QueryForList<PurVo>("P4411SelectReport", vo));
        }


        /// <summary>
        /// 발주 관리 MST - 리포트
        /// </summary>
        [Route("report/weekly")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetReportWeeklySelect([FromBody]PurVo vo)
        {
            return Ok<IEnumerable<PurVo>>(Properties.EntityMapper.QueryForList<PurVo>("P4411SelectWeeklyReport", vo));
        }


        /// <summary>
        /// 발주 관리(차종) POPUP - 조회
        /// </summary>
        [Route("popup1")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetPopup1Select([FromBody]PurVo vo)
        {
            return Ok<IEnumerable<PurVo>>(Properties.EntityMapper.QueryForList<PurVo>("P4411SelectPopUp1List", vo));
        }


        /// <summary>
        /// 발주 관리(차종) POPUP - RESULT - 조회
        /// </summary>
        [Route("popup1/result")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetPopup1ResultSelect([FromBody] PurVo vo)
        {
            return Ok<PurVo>(Properties.EntityMapper.QueryForObject<PurVo>("P4411SelectPopUp1Result", vo));
        }


        /// <summary>
        /// 발주 관리(SCM) POPUP - 조회
        /// </summary>
        [Route("popup2")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetPopup2Select([FromBody] PurVo vo)
        {
            return Ok<IEnumerable<PurVo>>(Properties.EntityMapper.QueryForList<PurVo>("P4420SelectPopUp1List", vo));
        }




        /// <summary>
        /// Dialog2 - 조회
        /// </summary>
        [Route("dlg2")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetDlg2Select([FromBody]PurVo vo)
        {
            try
            {
                return Ok<IEnumerable<PurVo>>(Properties.EntityMapper.QueryForList<PurVo>("P4411SelectDtlDlg2List", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }


        /// <summary>
        /// 최적화 발주원자재 요청
        /// </summary>
        [Route("opmz/mtrl")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetOpmzPurMtrlSelect([FromBody] PurVo vo)
        {
            try
            {
                return Ok<IEnumerable<PurVo>>(Properties.EntityMapper.QueryForList<PurVo>("P4411SelectOpmzPurMtrlList", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

    }
}