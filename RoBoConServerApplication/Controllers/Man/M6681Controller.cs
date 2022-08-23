using ModelsLibrary.Man;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using VisualServerApplication.Config;

namespace VisualServerApplication.Controllers.Man
{
    [RoutePrefix("m6681")]
    public class M6681Controller : ApiController
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
        /// 제품 도면 이력 관리  - 조회
        /// </summary>
        [Route("")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetMstSelect([FromBody]ManVo vo)
        {
            return Ok<IEnumerable<ManVo>>(Properties.EntityMapper.QueryForList<ManVo>("M6681SelectMaster", vo));
        }



        /// <summary>
        /// 제품 도면 이력 관리 DWG  - 조회
        /// </summary>
        [Route("dwg")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetDwgSelect([FromBody]ManVo vo)
        {
            return Ok<IEnumerable<ManVo>>(Properties.EntityMapper.QueryForList<ManVo>("M6681SelectDwg", vo));
        }

        /// <summary>
        /// 제품 도면 이력 관리 DWG  - 추가
        /// </summary>
        [Route("dwg/i")]
        [HttpPost]
        // POST api/<controller>
        public async Task<IHttpActionResult> GetDwgInsert([FromBody]ManVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Insert("M6681InsertDwg", vo) == null ? 1 : 0);
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 제품 도면 이력 관리 DWG  - 수정
        /// </summary>
        [Route("dwg/u")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetDwgUpdate([FromBody]ManVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Update("M6681UpdateDwg", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 제품 도면 이력 관리 DWG  - 삭제
        /// </summary>
        [Route("dwg/d")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetDwgDelete([FromBody]ManVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Delete("M6681DeleteDwg", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }




        /// <summary>
        /// 제품 도면 이력 관리 WRK  - 조회
        /// </summary>
        [Route("wrk")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetWrkSelect([FromBody]ManVo vo)
        {
            return Ok<IEnumerable<ManVo>>(Properties.EntityMapper.QueryForList<ManVo>("M6681SelectWrk", vo));
        }

        /// <summary>
        /// 제품 도면 이력 관리 WRK  - 추가
        /// </summary>
        [Route("wrk/i")]
        [HttpPost]
        // POST api/<controller>
        public async Task<IHttpActionResult> GetWrkInsert([FromBody]ManVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Insert("M6681InsertWrk", vo) == null ? 1 : 0);
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 제품 도면 이력 관리 WRK  - 수정
        /// </summary>
        [Route("wrk/u")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetWrkUpdate([FromBody]ManVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Update("M6681UpdateWrk", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 제품 도면 이력 관리 WRK  - 삭제
        /// </summary>
        [Route("wrk/d")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetWrkDelete([FromBody]ManVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Delete("M6681DeleteWrk", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }




    }
}