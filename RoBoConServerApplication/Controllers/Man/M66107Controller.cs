using ModelsLibrary.Man;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using VisualServerApplication.Config;

namespace VisualServerApplication.Controllers.Man
{    
    [RoutePrefix("m66107")]
    public class M66107Controller : ApiController
    {
        /// <summary>
        /// Loss 최적화 수행 MST 조회
        /// </summary>
        [Route("mst")]
        [HttpPost]
        // POST api/<controller>
        public async Task<IHttpActionResult> GetMstSelect([FromBody] ManVo vo)
        {
            try
            {
                return Ok<IEnumerable<ManVo>>(Properties.EntityMapper.QueryForList<ManVo>("M66107SelectMst", vo));
            }
            catch (Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        ///// <summary>
        ///// Loss 최적화 수행 MST 조회
        ///// </summary>
        //[Route("mst/key")]
        //[HttpPost]
        //// POST api/<controller>
        //public async Task<IHttpActionResult> GetKeySelect([FromBody] ManVo vo)
        //{
        //    try
        //    {
        //        return Ok<string>(Properties.EntityMapper.QueryForObject<string>("M66107SelectKey", vo));
        //    }
        //    catch (Exception eLog)
        //    {
        //        return Ok<string>(eLog.Message);
        //    }
        //}

        /// <summary>
        /// Loss 최적화 수행 MST - 추가
        /// </summary>
        [Route("mst/i")]
        [HttpPost]
        // POST api/<controller>
        public async Task<IHttpActionResult> GetMstInsert([FromBody] ManVo vo)
        {
            try
            {
                Properties.EntityMapper.BeginTransaction();

                vo.OPMZ_NO = Properties.EntityMapper.QueryForObject<string>("M66107SelectKey", vo);

                Properties.EntityMapper.Insert("M66107InsertMst", vo);

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
        /// Loss 최적화 수행 MST - 수정
        /// </summary>
        [Route("mst/u")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetMstUpdate([FromBody] ManVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Update("M66107UpdateMst", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// Loss 최적화 수행 MST 로그 조회
        /// </summary>
        [Route("mst/log")]
        [HttpPost]
        // POST api/<controller>
        public async Task<IHttpActionResult> GetMstLogSelect([FromBody] ManVo vo)
        {
            try
            {
                return Ok<IEnumerable<ManVo>>(Properties.EntityMapper.QueryForList<ManVo>("M66107SelectMstLogList", vo));
            }
            catch (Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// Loss 최적화 수행 DTL 조회
        /// </summary>
        [Route("dtl")]
        [HttpPost]
        // POST api/<controller>
        public async Task<IHttpActionResult> GetDtlSelect([FromBody] ManVo vo)
        {
            try
            {
                return Ok<IEnumerable<ManVo>>(Properties.EntityMapper.QueryForList<ManVo>("M66107SelectDtl", vo));
            }
            catch (Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// Loss 최적화 수행 DTL 다이얼로그 조회
        /// </summary>
        [Route("dtl/dia")]
        [HttpPost]
        // POST api/<controller>
        public async Task<IHttpActionResult> GetDtlDiaSelect([FromBody] ManVo vo)
        {
            try
            {
                return Ok<IEnumerable<ManVo>>(Properties.EntityMapper.QueryForList<ManVo>("M66107SelectDtlDia", vo));
            }
            catch (Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// Loss 최적화 수행 DTL 다이얼로그 추가
        /// </summary>
        [Route("dtl/dia/i")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetDtlDiaInsert([FromBody] IList<ManVo> voList)
        {
            try
            {
                Properties.EntityMapper.BeginTransaction();
                foreach (ManVo item in voList)
                {
                    Properties.EntityMapper.Insert("M66107InsertDtl", item);
                }
                int? nResult = Properties.EntityMapper.QueryForObject<int?>("ProcM66107", voList[0]);
                
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
        /// Loss 발주최적화 통해 발주원자재리스트 조회
        /// </summary>
        [Route("pur/mtrl")]
        [HttpPost]
        // POST api/<controller>
        public async Task<IHttpActionResult> GetPurMtrlListSelect([FromBody] ManVo vo)
        {
            try
            {
                return Ok<IEnumerable<ManVo>>(Properties.EntityMapper.QueryForList<ManVo>("M66107SelectPurMtrlList", vo));
            }
            catch (Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// Loss 발주최적화 통해 발주원자재리스트 조회
        /// </summary>
        [Route("pur/mtrl/u")]
        [HttpPost]
        // POST api/<controller>
        public async Task<IHttpActionResult> GetPurMtrlUpdate([FromBody] IList<ManVo> voList)
        {
            try
            {
                Properties.EntityMapper.BeginTransaction();

                foreach (ManVo vo in voList)
                {
                    Properties.EntityMapper.Update("M66107UpdatePurMtrl", vo);
                }

                Properties.EntityMapper.CommitTransaction();
                return Ok<int>(1);
            }
            catch (Exception eLog)
            {
                Properties.EntityMapper.RollBackTransaction();
                return Ok<string>(eLog.Message);
            }
        }

    }
}