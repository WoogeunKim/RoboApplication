using ModelsLibrary.Code;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using VisualServerApplication.Config;

namespace VisualServerApplication.Controllers.Code
{
    [RoutePrefix("s1431")]
    public class S1431Controller : ApiController
    {
        [Route("mst/s")]
        [HttpPost]
        public async Task<IHttpActionResult> GetSelecteMst([FromBody] SystemCodeVo vo)
        {
            return Ok<IEnumerable<SystemCodeVo>>(Properties.EntityMapper.QueryForList<SystemCodeVo>("S1431SelectMstList", vo));
        }

        [Route("dtl/s")]
        [HttpPost]
        public async Task<IHttpActionResult> GetSelecteDtl([FromBody] SystemCodeVo vo)
        {
            return Ok<SystemCodeVo>(Properties.EntityMapper.QueryForObject<SystemCodeVo>("S1431SelectDtlList", vo));
            
        }

        [Route("mst/i")]
        [HttpPost]
        public async Task<IHttpActionResult> GetMstInsert([FromBody] SystemCodeVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Insert("S1431InsertMst", vo) == null ? 1 : 0);
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        [Route("mst/u")]
        [HttpPost]
        public async Task<IHttpActionResult> GetMstUpdate([FromBody] SystemCodeVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Update("S1431UpdateMst", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

    }
}