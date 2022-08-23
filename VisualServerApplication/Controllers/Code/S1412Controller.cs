using ModelsLibrary.Code;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using VisualServerApplication.Config;

namespace VisualServerApplication.Controllers.Code
{
    [RoutePrefix("s1412")]
    public class S1412Controller : ApiController
    {
        #region MyRegion
        //[Route("")]
        //[HttpGet]
        //// GET api/<controller>
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //[Route("")]
        //[HttpGet]
        //// GET api/<controller>/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //[Route("")]
        //[HttpPost]
        //// POST api/<controller>
        //public void Post([FromBody]string value)
        //{
        //}

        //[Route("")]
        //[HttpPut]
        //// PUT api/<controller>/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //[Route("")]
        //[HttpDelete]
        //// DELETE api/<controller>/5
        //public void Delete(int id)
        //{
        //}

        #endregion

        /// <summary>
        /// 보고서 관리 - 조회
        /// </summary>
        [Route("mst")]
        [HttpPost]
        // GET api/<controller>
        public async Task<IHttpActionResult> GetMstSelect([FromBody] SystemCodeVo vo)
        {
            return Ok<IEnumerable<SystemCodeVo>>(Properties.EntityMapper.QueryForList<SystemCodeVo>("S1412SelectMstList", vo));
        }

        /// <summary>
        /// 보고서 관리 - 추가
        /// </summary>
        [Route("mst/i")]
        [HttpPost]
        // POST api/<controller>
        public async Task<IHttpActionResult> GetMstInsert([FromBody]SystemCodeVo vo)
        {
            try
            {
                if (Properties.EntityMapper.QueryForList<SystemCodeVo>("S1412SelectMstList", vo).Count > 0)
                {
                    return Ok<string>("[코드 - 중복] 코드를 다시 입력 하십시오.");
                }

                return Ok<int>(Properties.EntityMapper.Insert("S1412InsertMst", vo) == null ? 1 : 0);
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 보고서 관리 - 수정
        /// </summary>
        [Route("mst/u")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetMstUpdate([FromBody]SystemCodeVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Update("S1412UpdateMst", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

        /// <summary>
        /// 보고서 관리 - 삭제
        /// </summary>
        [Route("mst/d")]
        [HttpPost]
        // PUT api/<controller>/5
        public async Task<IHttpActionResult> GetMstDelete([FromBody] SystemCodeVo vo)
        {
            try
            {
                return Ok<int>(Properties.EntityMapper.Delete("S1412DeleteMst", vo));
            }
            catch (System.Exception eLog)
            {
                return Ok<string>(eLog.Message);
            }
        }

    }
}