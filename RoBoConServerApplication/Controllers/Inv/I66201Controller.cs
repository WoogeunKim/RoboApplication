using ModelsLibrary.Inv;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using VisualServerApplication.Config;

namespace VisualServerApplication.Controllers.Inv
{
    [RoutePrefix("i66201")]
    public class I66201Controller : ApiController
    {
        /* // GET api/<controller>
         public IEnumerable<string> Get()
         {
             return new string[] { "value1", "value2" };
         }

         // GET api/<controller>/5
         public string Get(int id)
         {
             return "value";
         }

         // POST api/<controller>
         public void Post([FromBody] string value)
         {
         }

         // PUT api/<controller>/5
         public void Put(int id, [FromBody] string value)
         {
         }

         // DELETE api/<controller>/5
         public void Delete(int id)
         {
         }*/
        /// <summary>
        /// 재고장 MST  - 조회
        /// </summary>
        [Route("mst")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetMstSelect([FromBody] InvVo vo)
        {
            try 
            { 
            return Ok<IEnumerable<InvVo>>(Properties.EntityMapper.QueryForList<InvVo>("I66201SelectList", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

    }
}