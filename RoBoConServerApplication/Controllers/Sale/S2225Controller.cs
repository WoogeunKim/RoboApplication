using ModelsLibrary.Sale;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using VisualServerApplication.Config;

namespace VisualServerApplication.Controllers.Sale
{
    [RoutePrefix("s2225")]
    public class S2225Controller : ApiController
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
        /// 세금계산서발행 - 계산서생성 (일괄) - 조회
        /// </summary>
        [Route("mst/bill")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetMstBillSelect([FromBody]SaleVo vo)
        {
            return Ok<IEnumerable<SaleVo>>(Properties.EntityMapper.QueryForList<SaleVo>("S2225SelectBillList", vo));
        }

        /// <summary>
        /// 세금계산서발행 - 계산서생성 (일괄) - 일괄(Home Tax) 
        /// </summary>
        [Route("mst/bill/crt/total")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetMstBillCrtTotalSelect([FromBody]IEnumerable<SaleVo> voList)
        {
            try
            {
                Properties.EntityMapper.BeginTransaction();
                foreach (SaleVo item in voList)
                {
                    Properties.EntityMapper.QueryForObject<int?>("ProcS2225", item);
                }
                Properties.EntityMapper.CommitTransaction();
                return Ok<int?>(1);
            }
            catch (System.Exception eLog)
            {
                Properties.EntityMapper.RollBackTransaction();
                return Ok<string>(eLog.Message);
            }

        }

        /// <summary>
        /// 세금계산서발행 - 계산서생성 (일괄) HISTORY - 조회
        /// </summary>
        [Route("mst/bill/history")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetMstBillHistorySelect([FromBody]SaleVo vo)
        {
            return Ok<IEnumerable<SaleVo>>(Properties.EntityMapper.QueryForList<SaleVo>("S2225SelectBillHistoryList", vo));
        }

    }
}