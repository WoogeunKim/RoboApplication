using ModelsLibrary.Sale;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using VisualServerApplication.Config;

namespace VisualServerApplication.Controllers.Sale
{
    [RoutePrefix("s22313")]
    public class S22313Controller : ApiController
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
        /// 세금계산서발행 - 건별(Home Tax) - 조회
        /// </summary>
        [Route("mst")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetMstSelect([FromBody]SaleVo vo)
        {
            return Ok<IEnumerable<SaleVo>>(Properties.EntityMapper.QueryForList<SaleVo>("S22313SelectMstList", vo));
        }

        /// <summary>
        /// 세금계산서발행 - 건별(Home Tax) - 삭제
        /// </summary>
        [Route("mst/d")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetMstDelete([FromBody]SaleVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Delete("S22313DeleteDtl", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 세금계산서발행 - 건별(Home Tax) - Excel 조회
        /// </summary>
        [Route("mst/excel")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetMstExcelSelect([FromBody]SaleVo vo)
        {
            return Ok<IEnumerable<SaleVo>>(Properties.EntityMapper.QueryForList<SaleVo>("S22313SelectExcelList", vo));
        }

        /// <summary>
        /// 세금계산서발행 - 건별(Home Tax) -  Excel Upload
        /// </summary>
        [Route("mst/excel/u")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetMstExcelUpdate([FromBody]SaleVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Update("S22313UpdateExcel", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }


        /// <summary>
        /// 세금계산서발행 - 계산서생성 (건별) - 조회
        /// </summary>
        [Route("mst/bill")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetMstBillSelect([FromBody]SaleVo vo)
        {
            return Ok<IEnumerable<SaleVo>>(Properties.EntityMapper.QueryForList<SaleVo>("S22313SelectBillList", vo));
        }

        /// <summary>
        /// 세금계산서발행 - 계산서생성 (건별) - 건별(Home Tax) 
        /// </summary>
        [Route("mst/bill/crt")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetMstBillCrtSelect([FromBody]IEnumerable<SaleVo> voList)
        {
            try
            {
                Properties.EntityMapper.BeginTransaction();
                foreach (SaleVo item in voList)
                {
                    Properties.EntityMapper.QueryForObject<int?>("ProcS22313", item);
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
    }
}