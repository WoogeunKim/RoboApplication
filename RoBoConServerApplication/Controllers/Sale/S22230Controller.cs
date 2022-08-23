using ModelsLibrary.Pur;
using ModelsLibrary.Sale;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using VisualServerApplication.Config;

namespace VisualServerApplication.Controllers.Sale
{
    [RoutePrefix("s22230")]
    public class S22230Controller : ApiController
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
        /// (화성판매)현품표출력 MST - 조회
        /// </summary>
        [Route("")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetMstSelect([FromBody]SaleVo vo)
        {
            return Ok<IEnumerable<SaleVo>>(Properties.EntityMapper.QueryForList<SaleVo>("S22230SelectMstList", vo));
        }


        /// <summary>
        /// (화성판매)현품표출력 프로시저 - 조회
        /// </summary>
        [Route("proc")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetMstProc([FromBody] SaleVo vo)
        {
            try
            {
                return Ok<int?>(Properties.EntityMapper.QueryForObject<int?>("ProcS22230", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// (화성판매)현품표출력 MST - 조회
        /// </summary>
        [Route("barcode")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetMstReportSelect([FromBody] SaleVo vo)
        {
            return Ok<IEnumerable<PurVo>>(Properties.EntityMapper.QueryForList<PurVo>("S22230SelectDtlBarCodeReportList", vo));
        }

    }
}