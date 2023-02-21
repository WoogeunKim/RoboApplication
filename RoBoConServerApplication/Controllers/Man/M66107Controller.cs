using ModelsLibrary.Man;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using VisualServerApplication.Config;

namespace VisualServerApplication.Controllers.Man
{
    //public class M66107Controller : ApiController
    //{
    //    // GET api/<controller>
    //    public IEnumerable<string> Get()
    //    {
    //        return new string[] { "value1", "value2" };
    //    }

    //    // GET api/<controller>/5
    //    public string Get(int id)
    //    {
    //        return "value";
    //    }

    //    // POST api/<controller>
    //    public void Post([FromBody] string value)
    //    {
    //    }

    //    // PUT api/<controller>/5
    //    public void Put(int id, [FromBody] string value)
    //    {
    //    }

    //    // DELETE api/<controller>/5
    //    public void Delete(int id)
    //    {
    //    }
    //}
    
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

        /// <summary>
        /// Loss 최적화 수행 MST 조회
        /// </summary>
        [Route("mst/key")]
        [HttpPost]
        // POST api/<controller>
        public async Task<IHttpActionResult> GetKeySelect([FromBody] ManVo vo)
        {
            try
            {
                return Ok<string>(Properties.EntityMapper.QueryForObject<string>("M66107SelectKey", vo));
            }
            catch (Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

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
                return Ok<int>(Properties.EntityMapper.Insert("M66107InsertMst", vo) == null ? 1 : 0);
            }
            catch (System.Exception eLog)
            {
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

    }
}