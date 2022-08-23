using ModelsLibrary.Inv;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using VisualServerApplication.Config;

namespace VisualServerApplication.Controllers.Inv
{
    [RoutePrefix("i55221")]
    public class I55221Controller : ApiController
    {
        /// <summary>
        /// 자재수불장 MST - 조회
        /// </summary>
        [Route("")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetListSelect([FromBody] InvVo vo)
        {
            try
            {
                return Ok<IEnumerable<InvVo>>(Properties.EntityMapper.QueryForList<InvVo>("I55221SelectList", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

    }
}