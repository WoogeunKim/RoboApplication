using ModelsLibrary.Pur;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using VisualServerApplication.Config;

namespace VisualServerApplication.Controllers.Pur
{
    [RoutePrefix("p4430")]
    public class P4430Controller : ApiController
    {

        /// <summary>
        /// 발주서 등록 MST - 조회
        /// </summary>
        [Route("mst")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetMstSelect([FromBody] PurVo vo)
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
        /// 발주서 등록 MST - 추가
        /// </summary>
        [Route("mst/i")]
        [HttpPost]
        // POST api/<controller>
        public async Task<IHttpActionResult> GetMstInsert([FromBody] PurVo vo)
        {
            try
            {
                Properties.EntityMapper.BeginTransaction();

                vo.PUR_ORD_NO = Properties.EntityMapper.QueryForObject<string>("P4411SelectPurOrdNo", vo);
                Properties.EntityMapper.Insert("P4411InsertMst", vo);
                Properties.EntityMapper.Update("P4411UpdatePurOrdNo", vo);

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
        /// 발주서 등록 MST - 삭제
        /// </summary>
        [Route("mst/d")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetMstDelete([FromBody] PurVo vo)
        {
            try
            {
                Properties.EntityMapper.Delete("P4411DeleteDtl", vo);
                return Ok<int>(Properties.EntityMapper.Delete("P4411DeleteMst", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }



        /// <summary>
        /// 발주서 등록 MST - 수정
        /// </summary>
        [Route("mst/u")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetMstUpdate([FromBody] PurVo vo)
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
        /// 발주서 등록 DTL - 조회
        /// </summary>
        [Route("dtl")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetDtlSelect([FromBody] PurVo vo)
        {
            try
            {
                return Ok<IEnumerable<PurVo>>(Properties.EntityMapper.QueryForList<PurVo>("P4430SelectDtlList", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 발주서 등록 DTL - 추가
        /// </summary>
        [Route("dtl/i")]
        [HttpPost]

        public async Task<IHttpActionResult> GetDtlInsert([FromBody] IList<PurVo> voList)
        {
            try
            {
                Properties.EntityMapper.BeginTransaction();

                foreach (PurVo item in voList)
                {
                    Properties.EntityMapper.Insert("P4430InsertDtl", item);
                }

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
        /// 발주서 등록 DTL - 수주 정보 조회
        /// </summary>
        [Route("dtl/i/sl")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetDtlSlSelect([FromBody] PurVo vo)
        {
            try
            {
                return Ok<IEnumerable<PurVo>>(Properties.EntityMapper.QueryForList<PurVo>("P4430SelectSlRlseList", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        ///<summary>
        //발주서 등록 DTL - 단가 조회
        [Route("dtl/i/prc")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetDtlPrcSelect([FromBody] PurVo vo)
        {
            try
            {
                return Ok<PurVo>(Properties.EntityMapper.QueryForObject<PurVo>("P4430SelectCoUtPrc", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 발주서 등록 DTL - 수정
        /// </summary>
        [Route("dtl/u")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetDtlUpdate([FromBody] PurVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Update("P4430UpdateDtl", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 발주서 등록 DTL - 삭제
        /// </summary>
        [Route("dtl/d")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetDtlDelete([FromBody] PurVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Delete("P4430DeleteDtl", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 발주서 등록 MST - 리포트
        /// </summary>
        [Route("report")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetReportSelect([FromBody] PurVo vo)
        {
            return Ok<IEnumerable<PurVo>>(Properties.EntityMapper.QueryForList<PurVo>("P4411SelectReport", vo));
        }

        /// <summary>
        /// 발주서 등록 - 리포트
        /// </summary>
        [Route("report/weekly")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetReportWeeklySelect([FromBody] PurVo vo)
        {
            return Ok<IEnumerable<PurVo>>(Properties.EntityMapper.QueryForList<PurVo>("P4411SelectWeeklyReport", vo));
        }


        /// <summary>
        /// 발주서 등록 - 조회
        /// </summary>
        [Route("popup1")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetPopup1Select([FromBody] PurVo vo)
        {
            return Ok<IEnumerable<PurVo>>(Properties.EntityMapper.QueryForList<PurVo>("P4411SelectPopUp1List", vo));
        }


        /// <summary>
        /// 발주서 등록 POPUP - RESULT - 조회
        /// </summary>
        [Route("popup1/result")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetPopup1ResultSelect([FromBody] PurVo vo)
        {
            return Ok<PurVo>(Properties.EntityMapper.QueryForObject<PurVo>("P4411SelectPopUp1Result", vo));
        }


        /// <summary>
        /// 발주서 등록 - 조회
        /// </summary>
        [Route("popup2")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetPopup2Select([FromBody] PurVo vo)
        {
            return Ok<IEnumerable<PurVo>>(Properties.EntityMapper.QueryForList<PurVo>("P4420SelectPopUp1List", vo));
        }
    }
}