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

    }
}