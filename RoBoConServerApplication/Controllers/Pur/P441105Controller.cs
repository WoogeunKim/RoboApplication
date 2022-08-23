using ModelsLibrary.Pur;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using VisualServerApplication.Config;

namespace VisualServerApplication.Controllers.Pur
{
    [RoutePrefix("p441105")]
    public class P441105Controller : ApiController
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


        ///// <summary>
        ///// Bulk 역전개 MST - 조회
        ///// </summary>
        //[Route("mst")]
        //[HttpPost]
        //// GET api/<controller>
        //public async Task<IHttpActionResult> GetMstSelect([FromBody]PurVo vo)
        //{
        //    try
        //    {
        //        return Ok<IEnumerable<PurVo>>(Properties.EntityMapper.QueryForList<PurVo>("P441103SelectMstList", vo));
        //    }
        //    catch (System.Exception eLog)
        //    {
        //        return Ok<string>(eLog.Message);
        //    }
        //}


        ///// <summary>
        /////  부자재 발주 관리 MST - 추가
        ///// </summary>
        //[Route("mst/i")]
        //[HttpPost]
        //// POST api/<controller>
        //public async Task<IHttpActionResult> GetMstInsert([FromBody]PurVo vo)
        //{
        //    try
        //    {
        //        vo.PUR_ORD_NO = Properties.EntityMapper.QueryForObject<string>("P441103SelectPurOrdNo", vo);
        //        Properties.EntityMapper.Update("P441103UpdatePurOrdNo", vo);
        //        return Ok<int>(Properties.EntityMapper.Insert("P441103InsertMst", vo) == null ? 1 : 0);
        //    }
        //    catch (System.Exception eLog)
        //    {
        //        return Ok<string>(eLog.Message);
        //    }
        //}

        ///// <summary>
        ///// 부자재 발주 관리 MST - 수정
        ///// </summary>
        //[Route("mst/u")]
        //[HttpPost]
        //// PUT api/<controller>/5
        //public async Task<IHttpActionResult> GetMstUpdate([FromBody]PurVo vo)
        //{
        //    try
        //    {
        //        return Ok<int>(Properties.EntityMapper.Update("P441103UpdateMst", vo));
        //    }
        //    catch (System.Exception eLog)
        //    {
        //        return Ok<string>(eLog.Message);
        //    }
        //}


        ///// <summary>
        ///// 부자재 발주 관리 MST - 삭제
        ///// </summary>
        //[Route("mst/d")]
        //[HttpPost]
        //// PUT api/<controller>/5
        //public async Task<IHttpActionResult> GetMstDelete([FromBody]PurVo vo)
        //{
        //    try
        //    {
        //        Properties.EntityMapper.Delete("P441103DeleteDtl", vo);
        //        return Ok<int>(Properties.EntityMapper.Delete("P441103DeleteMst", vo));
        //    }
        //    catch (System.Exception eLog)
        //    {
        //        return Ok<string>(eLog.Message);
        //    }
        //}



        /// <summary>
        /// Bulk 역전개 DTL - 조회
        /// </summary>
        [Route("mst")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetDtlSelect([FromBody]PurVo vo)
        {
            return Ok<IEnumerable<PurVo>>(Properties.EntityMapper.QueryForList<PurVo>("P441105SelectMstList", vo));
        }

       
     

    }
}