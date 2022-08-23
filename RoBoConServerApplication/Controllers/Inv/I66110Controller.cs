using ModelsLibrary.Inv;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using VisualServerApplication.Config;

namespace VisualServerApplication.Controllers.Inv
{
    [RoutePrefix("i66110")]
    public class I66110Controller : ApiController
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
        ///품목 입고  - 조회
        /// </summary>
        [Route("mst")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetMstSelect([FromBody]InvVo vo)
        {
            try
            {
                return Ok<IEnumerable<InvVo>>(Properties.EntityMapper.QueryForList<InvVo>("I66110SelectMstList", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }




        /// <summary>
        /// 품목 입고  - 추가
        /// </summary>
        [Route("mst/i")]
        [HttpPost]
        // POST api/<controller>
        public async Task<IHttpActionResult> GetMstInsert([FromBody] IList<InvVo> voList)
        {
            try
            {
                Properties.EntityMapper.BeginTransaction();

                //voList[0].INAUD_TMP_NO = Properties.EntityMapper.QueryForObject<string>("I6610SelectNo", voList[0]);
                //Properties.EntityMapper.Update("I6610UpdateNo", voList[0]);

                foreach (InvVo item in voList)
                {
                    //item.INAUD_TMP_NO = voList[0].INAUD_TMP_NO;
                    Properties.EntityMapper.Insert("I66110InsertMst", item);
                }
                //return Ok<int>(Properties.EntityMapper.Insert("I6610InsertPurMst", vo) == null ? 1 : 0);
                Properties.EntityMapper.CommitTransaction();
                return Ok<int>(1);
            }
            catch (System.Exception eLog)
            {
                Properties.EntityMapper.RollBackTransaction();
                return Ok<string>(eLog.Message);
            }
        }

        /////// <summary>
        /////// 품목 가입고  - 수정
        /////// </summary>
        ////[Route("u")]
        ////[HttpPost]
        ////// PUT api/<controller>/5
        ////public async Task<IHttpActionResult> GetMstUpdate([FromBody]InvVo vo)
        ////{
        ////    try
        ////    {
        ////        return Ok<int>(Properties.EntityMapper.Update("M6611UpdateMaster", vo));
        ////    }
        ////    catch (System.Exception eLog)
        ////    {
        ////        return Ok<string>(eLog.Message);
        ////    }
        ////}

        /// <summary>
        /// 품목 입고  - 삭제
        /// </summary>
        [Route("mst/d")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetMstDelete([FromBody] InvVo vo)
        {
            try
            {
                //Properties.EntityMapper.BeginTransaction();
                //foreach (InvVo item in voList)
                //{
                    Properties.EntityMapper.Delete("I66110DeleteMst", vo);
                //}
                //return Ok<int>(Properties.EntityMapper.Insert("I6610InsertPurMst", vo) == null ? 1 : 0);
                //Properties.EntityMapper.CommitTransaction();
                return Ok<int>(1);
                //return Ok<int>(Properties.EntityMapper.Delete("I6610DeleteMst", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }


        /// <summary>
        ///품목 입고  - 발주 국내 조회
        /// </summary>
        [Route("pur")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetPurSelect([FromBody] InvVo vo)
        {
            try
            {
                return Ok<IEnumerable<InvVo>>(Properties.EntityMapper.QueryForList<InvVo>("I66110SelectDtlPurList", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        ///품목 입고  - 발주 기타 조회
        /// </summary>
        [Route("oth")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetOtherSelect([FromBody] InvVo vo)
        {
            try
            {
                return Ok<IEnumerable<InvVo>>(Properties.EntityMapper.QueryForList<InvVo>("I66110SelectDtlOthList", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }


        ///// <summary>
        /////품목 입고  - 원자재(품질검사) 조회
        ///// </summary>
        //[Route("m")]
        //[HttpPost]
        //// GET api/<controller>
        //public async Task<IHttpActionResult> GetMSelect([FromBody] InvVo vo)
        //{
        //    return Ok<IEnumerable<InvVo>>(Properties.EntityMapper.QueryForList<InvVo>("I6610SelectDtlmList", vo));
        //}
    }
}