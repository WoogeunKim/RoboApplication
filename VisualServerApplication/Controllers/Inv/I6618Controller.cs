using ModelsLibrary.Inv;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using VisualServerApplication.Config;

namespace VisualServerApplication.Controllers.Inv
{
    [RoutePrefix("i6618")]
    public class I6618Controller : ApiController
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
        /// 주문대비 출고현황 MST  - 조회
        /// </summary>
        [Route("mst")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetMstSelect([FromBody]InvVo vo)
        {
            try
            {

                return Ok<IEnumerable<InvVo>>(Properties.EntityMapper.QueryForList<InvVo>("I6618SelectMstList", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }


        /// <summary>
        /// 주문대비 출고현황 DTL(칭량계획)  - 조회
        /// </summary>
        [Route("dtl1")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetDtl1Select([FromBody] InvVo vo)
        {
            try
            {

                return Ok<IEnumerable<InvVo>>(Properties.EntityMapper.QueryForList<InvVo>("I6618SelectDtl1List", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }


        /// <summary>
        /// 주문대비 출고현황 DTL(충전포장 계획 및 실적)  - 조회
        /// </summary>
        [Route("dtl2")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetDtl2Select([FromBody] InvVo vo)
        {
            try
            {

                return Ok<IEnumerable<InvVo>>(Properties.EntityMapper.QueryForList<InvVo>("I6618SelectDtl2List", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        ///// <summary>
        ///// 주문대비 출고현황 MST  - 추가
        ///// </summary>
        //[Route("mst/i")]
        //[HttpPost]
        //// POST api/<controller>
        //public async Task<IHttpActionResult> GetMstInsert([FromBody] InvVo vo)
        //{
        //    try
        //    {
        //        vo.INSRL_NO = Properties.EntityMapper.QueryForObject<string>("I5511SelectInsrlNo", vo);
        //        Properties.EntityMapper.Update("I5511UpdateInsrlNo", vo);

        //        return Ok<int>(Properties.EntityMapper.Insert("I5511InsertMst", vo) == null ? 1 : 0);
        //    }
        //    catch (System.Exception eLog)
        //    {
        //        return Ok<string>(eLog.Message);
        //    }
        //}

        ///// <summary>
        ///// 주문대비 출고현황 MST  - 수정
        ///// </summary>
        //[Route("mst/u")]
        //[HttpPost]
        //// PUT api/<controller>/5
        //public async Task<IHttpActionResult> GetMstUpdate([FromBody] InvVo vo)
        //{
        //    try
        //    {
        //        return Ok<int>(Properties.EntityMapper.Update("I5511UpdateMst", vo));
        //    }
        //    catch (System.Exception eLog)
        //    {
        //        return Ok<string>(eLog.Message);
        //    }
        //}

        ///// <summary>
        ///// 주문대비 출고현황 MST - 삭제
        ///// </summary>
        //[Route("mst/d")]
        //[HttpPost]
        //// PUT api/<controller>/5
        //public async Task<IHttpActionResult> GetMstDelete([FromBody] InvVo vo)
        //{
        //    try
        //    {
        //        if (Properties.EntityMapper.QueryForList<InvVo>("I5511SelectDtlList", vo).Count > 0)
        //        {
        //            return Ok<string>(vo.INSRL_NM + " - 품목 입고 자재 내역이 존재 합니다");
        //        }

        //        return Ok<int>(Properties.EntityMapper.Delete("I5511DeleteMst", vo));
        //    }
        //    catch (System.Exception eLog)
        //    {
        //        return Ok<string>(eLog.Message);
        //    }
        //}



    }
}