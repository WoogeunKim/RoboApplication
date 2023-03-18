using ModelsLibrary.Man;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using VisualServerApplication.Config;

namespace VisualServerApplication.Controllers.Man
{
    [RoutePrefix("m66112")]

    public class M66112Controller : ApiController
    {
        /// <summary>
        /// 고객사 발주 모니터링 MST 조회
        /// </summary>
        /// Post Api / <Controller>
        [Route("mst")]
        [HttpPost]
        public async Task<IHttpActionResult> PostMstSelectList([FromBody] ManVo vo)
        {
            try
            {
                return Ok<IEnumerable<ManVo>>(Properties.EntityMapper.QueryForList<ManVo>("M66112SelectMstList", vo));
            }
            catch(Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

    }
}