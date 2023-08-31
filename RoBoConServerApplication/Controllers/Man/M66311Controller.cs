using ModelsLibrary.Man;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using VisualServerApplication.Config;

namespace VisualServerApplication.Controllers.Man
{
    [RoutePrefix("m66311")]
    public class M66311Controller : ApiController
    {
        [Route("mst")]
        [HttpPost]
        // POST api/<controller>
        public async Task<IHttpActionResult> GetMstSelect([FromBody] ManVo vo)
        {
            try
            {
                return Ok<IEnumerable<ManVo>>(Properties.EntityMapper.QueryForList<ManVo>("M66311SelectMstList", vo));
            }
            catch (Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        [Route("pop")]
        [HttpPost]
        // POST api/<controller>
        public async Task<IHttpActionResult> GetMstPopSelect([FromBody] ManVo vo)
        {
            try
            {
                return Ok<IEnumerable<ManVo>>(Properties.EntityMapper.QueryForList<ManVo>("M66107SelectMstPop", vo));
            }
            catch (Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }



        [Route("n1st/eq")]
        [HttpPost]
        // POST api/<controller>
        public async Task<IHttpActionResult> GetN1stEqNoSelect([FromBody] ManVo vo)
        {
            try
            {
                return Ok<IEnumerable<ManVo>>(Properties.EntityMapper.QueryForList<ManVo>("M66311SelectN1stEqNoList", vo));
            }
            catch (Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }


        [Route("n2nd/eq")]
        [HttpPost]
        // POST api/<controller>
        public async Task<IHttpActionResult> GetN2ndEqNoSelect([FromBody] ManVo vo)
        {
            try
            {
                return Ok<IEnumerable<ManVo>>(Properties.EntityMapper.QueryForList<ManVo>("M66311SelectN2ndEqNoList", vo));
            }
            catch (Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }


        [Route("eq/u")]
        [HttpPost]
        // POST api/<controller>
        public async Task<IHttpActionResult> GetEqNoUpdate([FromBody] ManVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Update("M66311UpdateEqNo", vo));
            }
            catch (Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

    }
}