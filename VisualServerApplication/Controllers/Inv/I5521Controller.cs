using ModelsLibrary.Inv;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using VisualServerApplication.Config;

namespace VisualServerApplication.Controllers.Inv
{
    [RoutePrefix("i5521")]
    public class I5521Controller : ApiController
    {
        #region MyRegion
        //// GET api/<controller>
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/<controller>/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/<controller>
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/<controller>/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/<controller>/5
        //public void Delete(int id)
        //{
        //} 
        #endregion

        /// <summary>
        /// 자재수불명세서  - 조회
        /// </summary>
        [Route("mst")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetMstSelect([FromBody]InvVo vo)
        {
            return Ok<IEnumerable<InvVo>>(Properties.EntityMapper.QueryForList<InvVo>("I5521SelectMstList", vo));
        }

        /// <summary>
        /// 자재수불명세서 - 출력물
        /// </summary>
        [Route("report")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetReportSelect([FromBody]InvVo vo)
        {
            return Ok<IEnumerable<InvVo>>(Properties.EntityMapper.QueryForList<InvVo>("I5521SelectReportList", vo));
        }

    }
}