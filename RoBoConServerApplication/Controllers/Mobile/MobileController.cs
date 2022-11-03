using ModelsLibrary.Mobile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using VisualServerApplication.Config;

namespace VisualServerApplication.Controllers.Mobile
{
    [RoutePrefix("mobile")]

    public class MobileController : ApiController
    {
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
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/<controller>/5
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<controller>/5
        //public void Delete(int id)
        //{
        //}


        /// <summary>
        /// 로그인 - 조회
        /// </summary>
        [Route("login")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetMobileLoginSelect([FromBody] MobileVo vo)
        {
            try
            {
                return Ok<MobileVo>(Properties.EntityMapper.QueryForObject<MobileVo>("MobileLoginSelect", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 프로그램 버전 - 조회
        /// </summary>
        [Route("ver")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetMobileVerSelect([FromBody] MobileVo vo)
        {
            try
            {
                return Ok<MobileVo>(Properties.EntityMapper.QueryForObject<MobileVo>("MobileVerSelect", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }
    }
}