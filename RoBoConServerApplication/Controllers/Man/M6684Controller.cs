using ModelsLibrary.Man;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using VisualServerApplication.Config;

namespace VisualServerApplication.Controllers.Man
{
    [RoutePrefix("m6684")]

    public class M6684Controller : ApiController
    {
        #region
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
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/<controller>/5
        //public void Put(int id, [FromBody] string value)
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
        public async Task<IHttpActionResult> GetMstSelect([FromBody] ManVo vo)
        {
            try
            {
                return Ok<IEnumerable<ManVo>>(Properties.EntityMapper.QueryForList<ManVo>("M6684SelectMaster", vo));

            }
            catch (Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }



        /// <summary>
        /// 제품 도면 이력 관리 DWG  - 조회
        /// </summary>
        [Route("doc")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetDocSelect([FromBody] ManVo vo)
        {
            try
            {
                return Ok<IEnumerable<ManVo>>(Properties.EntityMapper.QueryForList<ManVo>("M6684SelectDoc", vo));

            }
            catch (Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 제품 도면 이력 관리 DWG  - 추가
        /// </summary>
        [Route("doc/i")]
        [HttpPost]
        // POST api/<controller>
        public async Task<IHttpActionResult> GetDocInsert([FromBody] ManVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Insert("M6684InsertDoc", vo) == null ? 1 : 0);
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 제품 도면 이력 관리 DWG  - 수정
        /// </summary>
        [Route("doc/u")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetDocUpdate([FromBody] ManVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Update("M6684UpdateDoc", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 제품 도면 이력 관리 DWG  - 삭제
        /// </summary>
        [Route("doc/d")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetDwgDelete([FromBody] ManVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Delete("M6684DeleteDoc", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }




    }
}