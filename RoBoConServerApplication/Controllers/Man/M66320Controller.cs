using ModelsLibrary.Man;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using VisualServerApplication.Config;

namespace VisualServerApplication.Controllers.Man
{
    [RoutePrefix("m66320")]
    public class M66320Controller : ApiController
    {
        /// <summary>
        /// 투입자재지시관리 Mst 관리 - 조회
        /// </summary>
        [Route("")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetMstSelect([FromBody] ManVo vo)
        {
            try
            {
                return Ok<IEnumerable<ManVo>>(Properties.EntityMapper.QueryForList<ManVo>("M66320SelectMstList", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 투입자재지시관리 DTL 관리 - 조회
        /// </summary>
        [Route("dtl")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetDtlSelect([FromBody] ManVo vo)
        {
            try
            {
                return Ok<IEnumerable<ManVo>>(Properties.EntityMapper.QueryForList<ManVo>("M66320SelectDtlList", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 
        /// 투입자재지시관리 Summary 관리 - 조회
        /// </summary>
        [Route("dtl/summary")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetDtlSummarySelect([FromBody] ManVo vo)
        {
            try
            {
                return Ok<IEnumerable<ManVo>>(Properties.EntityMapper.QueryForList<ManVo>("M66320SelectDtlSummaryList", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }


    }
}