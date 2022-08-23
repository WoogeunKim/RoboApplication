using ModelsLibrary.Code;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using VisualServerApplication.Config;

namespace VisualServerApplication.Controllers.Code
{
    [RoutePrefix("s1144")]
    public class S1144Controller : ApiController
    {
        #region MyRegion
        // GET api/<controller>
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
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
        #endregion

        /// <summary>
        /// 창고 위치 관리 MST - 조회
        /// </summary>
        [Route("mst")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetMstSelect([FromBody]SystemCodeVo vo)
        {
            return Ok<IEnumerable<SystemCodeVo>>(Properties.EntityMapper.QueryForList<SystemCodeVo>("S1144SelectMstList", vo));
        }

        /// <summary>
        /// 창고 위치 관리 MST - 추가
        /// </summary>
        [Route("mst/i")]
        [HttpPost]
        // POST api/<controller>
        public async Task<IHttpActionResult> GetMstInsert([FromBody]SystemCodeVo vo)
        {
            try
            {
                //Properties.EntityMapper.Insert("S1144UpdateMstHistory", vo);
                return Ok<int>(Properties.EntityMapper.Insert("S1144InsertMst", vo) == null ? 1 : 0);
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 창고 위치 관리 MST - 수정
        /// </summary>
        [Route("mst/u")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetMstUpdate([FromBody]SystemCodeVo vo)
        {
            try
            {
                Properties.EntityMapper.Insert("S1144UpdateMstHistory", vo);
                return Ok<int>(Properties.EntityMapper.Update("S1144UpdateMst", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 창고 위치 관리 MST - 삭제
        /// </summary>
        [Route("mst/d")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetMstDelete([FromBody]SystemCodeVo vo)
        {
            try
            {
                Properties.EntityMapper.Insert("S1144DeleteMstHistory", vo);
                return Ok<int>(Properties.EntityMapper.Delete("S1144DeleteMst", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 창고 위치 관리 DTL - 조회
        /// </summary>
        [Route("dtl")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetDtlSelect([FromBody]SystemCodeVo vo)
        {
            return Ok<IEnumerable<SystemCodeVo>>(Properties.EntityMapper.QueryForList<SystemCodeVo>("S1144SelectDtlList", vo));
        }




        /// <summary>
        /// 창고 위치 관리 HISTORY - 조회
        /// </summary>
        [Route("his")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetHisSelect([FromBody]SystemCodeVo vo)
        {
            return Ok<IEnumerable<SystemCodeVo>>(Properties.EntityMapper.QueryForList<SystemCodeVo>("S1144SelectHistList", vo));
        }


        /// <summary>
        /// 금형 위치 관리 HISTORY - 조회
        /// </summary>
        [Route("his/mold")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetMoldHisSelect([FromBody]SystemCodeVo vo)
        {
            return Ok<IEnumerable<SystemCodeVo>>(Properties.EntityMapper.QueryForList<SystemCodeVo>("M6663SelectHistList", vo));
        }

        ///// <summary>
        ///// 창고 위치 관리 HISTORY - 추가
        ///// </summary>
        //[Route("his/i")]
        //[HttpPost]
        //// POST api/<controller>
        //public async Task<IHttpActionResult> GetHisInsert([FromBody]SystemCodeVo vo)
        //{
        //    try
        //    {
        //        if (vo.RN == 1)
        //        {
        //            return Ok<int>(Properties.EntityMapper.Insert("S1144UpdateMstHistory", vo) == null ? 1 : 0);
        //        }else if (vo.RN == 2)
        //        {
        //            return Ok<int>(Properties.EntityMapper.Insert("S1144DeleteMstHistory", vo) == null ? 1 : 0);
        //        }
        //        else
        //        {
        //            return Ok<string>("");
        //        }
        //    }
        //    catch (System.Exception eLog)
        //    {
        //        return Ok<string>(eLog.Message);
        //    }
        //}



    }
}