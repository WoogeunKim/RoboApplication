using ModelsLibrary.Inv;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using VisualServerApplication.Config;

namespace VisualServerApplication.Controllers.Inv
{
    [RoutePrefix("I6621")]
    public class I6621Controller : ApiController
    {
        /// <summary>
        /// 품목그룹별 대그룹 재고장 - 조회
        /// </summary>
        [Route("mst")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetMstSelect([FromBody] InvVo vo)
        {
            try
            {
                return Ok<IEnumerable<InvVo>>(Properties.EntityMapper.QueryForList<InvVo>("I6621SelectMstList", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 품목그룹별 중그룹 재고장 - 조회
        /// </summary>
        [Route("dtl")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetDtlSelect([FromBody] InvVo vo)
        {
            try
            {
                return Ok<IEnumerable<InvVo>>(Properties.EntityMapper.QueryForList<InvVo>("I6621SelectDtlList", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

    }
}