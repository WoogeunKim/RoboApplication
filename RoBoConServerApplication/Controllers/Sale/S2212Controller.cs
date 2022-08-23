using ModelsLibrary.Sale;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using VisualServerApplication.Config;

namespace VisualServerApplication.Controllers.Sale
{
    [RoutePrefix("s2212")]
    public class S2212Controller : ApiController
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
        /// 출고 처리 MST - 조회
        /// </summary>
        [Route("mst")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetMstSelect([FromBody]SaleVo vo)
        {
            return Ok<IEnumerable<SaleVo>>(Properties.EntityMapper.QueryForList<SaleVo>("S2212SelectMstList", vo));
        }


        /// <summary>
        /// 출고 처리 DTL CO_CD - 조회
        /// </summary>
        [Route("dtl/co")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetDtlCoSelect([FromBody]SaleVo vo)
        {
            return Ok<IEnumerable<SaleVo>>(Properties.EntityMapper.QueryForList<SaleVo>("S2212SelectDtlCoCdList", vo));
        }

        /// <summary>
        /// 출고 처리 DTL - 조회
        /// </summary>
        [Route("dtl")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetDtlSelect([FromBody]SaleVo vo)
        {
            return Ok<IEnumerable<SaleVo>>(Properties.EntityMapper.QueryForList<SaleVo>("S2212SelectDtlList", vo));
        }


        /// <summary>
        /// 출고 처리 MST - 마감
        /// </summary>
        [Route("bill")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetBillSelect([FromBody]IEnumerable<SaleVo> voList)
        {
            try
            {
                Properties.EntityMapper.BeginTransaction();
                foreach (SaleVo item in voList)
                {
                    Properties.EntityMapper.QueryForObject<int?>("ProcS2212", item);
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
        /// 출고 처리 Report - 조회
        /// </summary>
        [Route("report")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetReportSelect([FromBody]SaleVo vo)
        {
            return Ok<IEnumerable<SaleVo>>(Properties.EntityMapper.QueryForList<SaleVo>("S2212SelectReportList", vo));
        }

    }
}